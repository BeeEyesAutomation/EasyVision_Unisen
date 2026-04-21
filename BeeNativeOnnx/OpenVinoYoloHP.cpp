#include "OpenVinoYoloHP.h"
#include <algorithm>
#include <cmath>
#include <stdexcept>

static inline float IoU_Box(const YoloBox& a, const YoloBox& b)
{
    float xx1 = std::max(a.x1, b.x1);
    float yy1 = std::max(a.y1, b.y1);
    float xx2 = std::min(a.x2, b.x2);
    float yy2 = std::min(a.y2, b.y2);

    float w = std::max(0.f, xx2 - xx1);
    float h = std::max(0.f, yy2 - yy1);
    float inter = w * h;

    float areaA = std::max(0.f, a.x2 - a.x1) * std::max(0.f, a.y2 - a.y1);
    float areaB = std::max(0.f, b.x2 - b.x1) * std::max(0.f, b.y2 - b.y1);
    float uni = areaA + areaB - inter;
    return (uni <= 0.f) ? 0.f : (inter / uni);
}

OpenVinoYoloHP::OpenVinoYoloHP(
    const std::wstring& xmlPath,
    int inputSize,
    int numClasses,
    int numThreads)
{
    S = inputSize;
    C = numClasses;

    // Load IR
    model = core.read_model(xmlPath);

    // Optional: force static shape if model is dynamic
    try
    {
        auto pshape = model->input().get_partial_shape();
        if (pshape.is_dynamic())
        {
            model->reshape({
                { model->input().get_any_name(), ov::Shape{1, 3, (size_t)S, (size_t)S} }
                });
        }
    }
    catch (...)
    {
        // bỏ qua nếu model không reshape được
    }

    // Debug available devices nếu cần
    // auto devs = core.get_available_devices();
    // for (const auto& d : devs)
    // {
    //     std::string s = "OpenVINO device: " + d;
    //     BeeLog::Write(BeeLog::Level::Info, s);
    // }

    // AUTO ưu tiên GPU Intel, fallback CPU
    ov::AnyMap config;
    config[ov::hint::performance_mode.name()] = ov::hint::PerformanceMode::LATENCY;

    // numThreads chỉ hợp CPU, không set cho AUTO/GPU
    (void)numThreads;

    // cache để compile lần sau nhanh hơn
    config[ov::cache_dir.name()] = std::string("./ov_cache");

    compiled = core.compile_model(model, "AUTO:GPU,CPU", config);
    infer = compiled.create_infer_request();

    inputPort = compiled.input();

    auto outs = compiled.outputs();
    if (outs.empty())
        throw std::runtime_error("OpenVINO compiled model has no outputs.");

    outputPort = outs[0];

    // Prepare input buffer (CHW float)
    inputBlob.resize((size_t)1 * 3 * S * S);
    paddedU8.create(S, S, CV_8UC3);

    candidates.reserve(4096);
}

void OpenVinoYoloHP::Warmup(int iters)
{
    cv::Mat dummy(480, 640, CV_8UC3, cv::Scalar(0, 0, 0));
    std::vector<YoloBox> out;
    out.reserve(128);

    for (int i = 0; i < iters; i++)
        Detect(dummy, 0.25f, 0.45f, false, out);
}

void OpenVinoYoloHP::Letterbox(const cv::Mat& src, cv::Mat& dst, float& scale, int& padw, int& padh)
{
    const int w = src.cols;
    const int h = src.rows;

    scale = std::min((float)S / (float)w, (float)S / (float)h);

    const int nw = std::max(1, (int)(w * scale + 0.5f));
    const int nh = std::max(1, (int)(h * scale + 0.5f));

    padw = (S - nw) >> 1;
    padh = (S - nh) >> 1;

    dst.setTo(cv::Scalar(0, 0, 0));
    cv::resize(src, dst(cv::Rect(padw, padh, nw, nh)), cv::Size(nw, nh), 0, 0, cv::INTER_LINEAR);
}

void OpenVinoYoloHP::BgrToCHWFloat01(const cv::Mat& u8, float* dstCHW)
{
    const int HW = S * S;

    float* cB = dstCHW;
    float* cG = dstCHW + HW;
    float* cR = dstCHW + 2 * HW;

    const float inv255 = 1.0f / 255.0f;

    for (int y = 0; y < S; y++)
    {
        const uchar* row = u8.ptr<uchar>(y);

        float* pb = cB + y * S;
        float* pg = cG + y * S;
        float* pr = cR + y * S;

        for (int x = 0; x < S; x++)
        {
            const uchar b = row[x * 3 + 0];
            const uchar g = row[x * 3 + 1];
            const uchar r = row[x * 3 + 2];

            pb[x] = b * inv255;
            pg[x] = g * inv255;
            pr[x] = r * inv255;
        }
    }
}

static inline void clamp_box(float& x1, float& y1, float& x2, float& y2, int w, int h)
{
    x1 = std::max(0.f, std::min(x1, (float)(w - 1)));
    y1 = std::max(0.f, std::min(y1, (float)(h - 1)));
    x2 = std::max(0.f, std::min(x2, (float)(w - 1)));
    y2 = std::max(0.f, std::min(y2, (float)(h - 1)));
}

void OpenVinoYoloHP::DecodeDetectionOutput(
    const ov::Tensor& t,
    float conf,
    float scale, int padw, int padh,
    int imgW, int imgH,
    std::vector<YoloBox>& out)
{
#pragma warning(push)
#pragma warning(disable: 4996)
    const float* p = t.data<const float>();
#pragma warning(pop)

    int num = (int)t.get_shape()[1];
    int dim = (int)t.get_shape()[2];

    out.clear();

    for (int i = 0; i < num; i++)
    {
        float x1 = p[i * dim + 0];
        float y1 = p[i * dim + 1];
        float x2 = p[i * dim + 2];
        float y2 = p[i * dim + 3];
        float score = p[i * dim + 4];
        int cls = (int)p[i * dim + 5];

        if (score < conf) continue;

        x1 = (x1 - padw) / scale;
        y1 = (y1 - padh) / scale;
        x2 = (x2 - padw) / scale;
        y2 = (y2 - padh) / scale;

        clamp_box(x1, y1, x2, y2, imgW, imgH);

        YoloBox box;
        box.x1 = x1;
        box.y1 = y1;
        box.x2 = x2;
        box.y2 = y2;
        box.score = score;
        box.classId = cls;

        out.push_back(box);
    }
}

void OpenVinoYoloHP::DecodeAnyLayout(
    const ov::Tensor& outTensor,
    float conf, float scale, int padw, int padh, int srcW, int srcH,
    std::vector<YoloBox>& cand)
{
    cand.clear();

#pragma warning(push)
#pragma warning(disable: 4996)
    const float* out = outTensor.data<const float>();
#pragma warning(pop)

    auto shp = outTensor.get_shape();
    if (shp.size() != 3) return;

    const int64_t d1 = (int64_t)shp[1];
    const int64_t d2 = (int64_t)shp[2];

    bool layout_N_E = (d2 >= 6);
    bool layout_E_N = (!layout_N_E && d1 >= 6);

    if (layout_N_E)
    {
        int N = (int)d1;
        int E = (int)d2;

        for (int i = 0; i < N; i++)
        {
            const float* p = out + (size_t)i * E;
            float obj = p[4];
            if (obj < conf) continue;

            int bestCls = -1;
            float best = 0.f;
            int clsCount = E - 5;

            for (int c = 0; c < clsCount; c++)
            {
                float s = p[5 + c];
                if (s > best) { best = s; bestCls = c; }
            }

            float score = obj * best;
            if (score < conf) continue;

            float cx = p[0], cy = p[1], w = p[2], h = p[3];

            float x1 = (cx - w * 0.5f - padw) / scale;
            float y1 = (cy - h * 0.5f - padh) / scale;
            float x2 = (cx + w * 0.5f - padw) / scale;
            float y2 = (cy + h * 0.5f - padh) / scale;

            clamp_box(x1, y1, x2, y2, srcW, srcH);
            cand.push_back({ x1,y1,x2,y2,score,bestCls });
        }
    }
    else if (layout_E_N)
    {
        int E = (int)d1;
        int N = (int)d2;

        auto at = [&](int e, int i) -> float {
            return out[(size_t)e * N + i];
            };

        int clsCount = E - 5;

        for (int i = 0; i < N; i++)
        {
            float obj = at(4, i);
            if (obj < conf) continue;

            int bestCls = -1;
            float best = 0.f;
            for (int c = 0; c < clsCount; c++)
            {
                float s = at(5 + c, i);
                if (s > best) { best = s; bestCls = c; }
            }

            float score = obj * best;
            if (score < conf) continue;

            float cx = at(0, i), cy = at(1, i), w = at(2, i), h = at(3, i);

            float x1 = (cx - w * 0.5f - padw) / scale;
            float y1 = (cy - h * 0.5f - padh) / scale;
            float x2 = (cx + w * 0.5f - padw) / scale;
            float y2 = (cy + h * 0.5f - padh) / scale;

            clamp_box(x1, y1, x2, y2, srcW, srcH);
            cand.push_back({ x1,y1,x2,y2,score,bestCls });
        }
    }
}

void OpenVinoYoloHP::NmsPerClass(std::vector<YoloBox>& cand, float iou, std::vector<YoloBox>& out)
{
    out.clear();
    if (cand.empty()) return;

    std::sort(cand.begin(), cand.end(),
        [](const YoloBox& a, const YoloBox& b) { return a.score > b.score; });

    const int CAP = 2000;
    if ((int)cand.size() > CAP) cand.resize(CAP);

    std::vector<char> removed(cand.size(), 0);

    for (size_t i = 0; i < cand.size(); i++)
    {
        if (removed[i]) continue;
        const auto& a = cand[i];
        out.push_back(a);

        for (size_t j = i + 1; j < cand.size(); j++)
        {
            if (removed[j]) continue;
            const auto& b = cand[j];
            if (a.classId != b.classId) continue;
            if (IoU_Box(a, b) > iou) removed[j] = 1;
        }
    }
}

void NmsPerClass2(std::vector<YoloBox>& cand, float iou, std::vector<YoloBox>& out)
{
    out.clear();
    if (cand.empty()) return;

    std::sort(cand.begin(), cand.end(),
        [](const YoloBox& a, const YoloBox& b) { return a.score > b.score; });

    const int CAP = 2000;
    if ((int)cand.size() > CAP) cand.resize(CAP);

    std::vector<char> removed(cand.size(), 0);

    for (size_t i = 0; i < cand.size(); i++)
    {
        if (removed[i]) continue;
        const auto& a = cand[i];
        out.push_back(a);

        for (size_t j = i + 1; j < cand.size(); j++)
        {
            if (removed[j]) continue;
            const auto& b = cand[j];
            if (a.classId != b.classId) continue;
            if (IoU_Box(a, b) > iou) removed[j] = 1;
        }
    }
}

void OpenVinoYoloHP::DecodeYolo(
    const ov::Tensor& t,
    float conf,
    float scale, int padw, int padh,
    int imgW, int imgH,
    std::vector<YoloBox>& out)
{
    out.clear();

#pragma warning(push)
#pragma warning(disable:4996)
    const float* data = t.data<const float>();
#pragma warning(pop)

    const auto& shape = t.get_shape();

    if (shape.size() != 3)
        return;

    const int C = (int)shape[1];
    const int N = (int)shape[2];

    if (C < 5)
        return;

    out.reserve(128);

    const float* cx = data + 0 * N;
    const float* cy = data + 1 * N;
    const float* w = data + 2 * N;
    const float* h = data + 3 * N;
    const float* sc = data + 4 * N;

    for (int i = 0; i < N; i++)
    {
        float score = sc[i];
        if (score < conf)
            continue;

        float bw = w[i];
        float bh = h[i];

        float x1 = (cx[i] - bw * 0.5f - padw) / scale;
        float y1 = (cy[i] - bh * 0.5f - padh) / scale;
        float x2 = (cx[i] + bw * 0.5f - padw) / scale;
        float y2 = (cy[i] + bh * 0.5f - padh) / scale;

        clamp_box(x1, y1, x2, y2, imgW, imgH);

        out.push_back({ x1, y1, x2, y2, score, 0 });
    }
}

void OpenVinoYoloHP::DecodeYoloAuto(
    const ov::Tensor& t,
    float conf,
    float scale, int padw, int padh,
    int imgW, int imgH,
    std::vector<YoloBox>& out)
{
    out.clear();

#pragma warning(push)
#pragma warning(disable:4996)
    const float* data = t.data<const float>();
#pragma warning(pop)

    const auto shape = t.get_shape();
    if (shape.size() != 3) return;

    int d1 = (int)shape[1];
    int d2 = (int)shape[2];

    bool is_C_N = (d1 >= 5 && d1 <= 512 && d2 > d1);
    bool is_N_C = (d2 >= 5 && d2 <= 512 && d1 > d2);

    auto push_box = [&](float cx, float cy, float bw, float bh, float score, int cls)
        {
            if (score < conf) return;

            float x1 = (cx - bw * 0.5f - padw) / scale;
            float y1 = (cy - bh * 0.5f - padh) / scale;
            float x2 = (cx + bw * 0.5f - padw) / scale;
            float y2 = (cy + bh * 0.5f - padh) / scale;

            clamp_box(x1, y1, x2, y2, imgW, imgH);
            out.push_back({ x1, y1, x2, y2, score, cls });
        };

    if (is_C_N)
    {
        int C = d1;
        int N = d2;

        if (C < 5) return;

        const float* cx = data + 0 * N;
        const float* cy = data + 1 * N;
        const float* w = data + 2 * N;
        const float* h = data + 3 * N;

        if (C == 5)
        {
            const float* sc = data + 4 * N;
            for (int i = 0; i < N; i++)
                push_box(cx[i], cy[i], w[i], h[i], sc[i], 0);
        }
        else
        {
            const float* obj = data + 4 * N;
            int clsCount = C - 5;

            for (int i = 0; i < N; i++)
            {
                float bestClsScore = 0.f;
                int bestCls = 0;

                for (int c = 0; c < clsCount; c++)
                {
                    float s = data[(5 + c) * N + i];
                    if (s > bestClsScore)
                    {
                        bestClsScore = s;
                        bestCls = c;
                    }
                }

                float score = obj[i] * bestClsScore;
                push_box(cx[i], cy[i], w[i], h[i], score, bestCls);
            }
        }
    }
    else if (is_N_C)
    {
        int N = d1;
        int C = d2;

        if (C < 5) return;

        for (int i = 0; i < N; i++)
        {
            const float* p = data + (size_t)i * C;

            float cx = p[0];
            float cy = p[1];
            float bw = p[2];
            float bh = p[3];

            if (C == 5)
            {
                push_box(cx, cy, bw, bh, p[4], 0);
            }
            else
            {
                float obj = p[4];
                int clsCount = C - 5;

                float bestClsScore = 0.f;
                int bestCls = 0;

                for (int c = 0; c < clsCount; c++)
                {
                    float s = p[5 + c];
                    if (s > bestClsScore)
                    {
                        bestClsScore = s;
                        bestCls = c;
                    }
                }

                float score = obj * bestClsScore;
                push_box(cx, cy, bw, bh, score, bestCls);
            }
        }
    }
}

static inline float sigmoidf(float x)
{
    return 1.f / (1.f + std::exp(-x));
}

static inline float clampf(float v, float lo, float hi)
{
    return v < lo ? lo : (v > hi ? hi : v);
}
void DecodeAuto_Yolo_RTDETR(
    const ov::Tensor& t,
    float conf,
    float iou,
    float scale, int padw, int padh,
    int imgW, int imgH,
    int S, int numClasses,
    std::vector<YoloBox>& out)
{
    out.clear();

#pragma warning(push)
#pragma warning(disable: 4996)
    const float* data = t.data<const float>();
#pragma warning(pop)

    auto shp = t.get_shape();
    if (shp.size() != 3) return;

    int N = (int)shp[1];
    int D = (int)shp[2];

    std::vector<YoloBox> candidates;
    candidates.reserve(512);

    // Xác định RT-DETR hay YOLO
  // RT-DETR: shape [1, N_query, numClasses+4]  → D nhỏ, N lớn
  // YOLO:    shape [1, numClasses+4, 8400]     → D lớn, N nhỏ
    bool isRTDETR = (D == numClasses + 4) && (N > D);

    if (isRTDETR)
    {
        for (int i = 0; i < N; i++)
        {
            const float* p = data + i * D;

            // RT-DETR Ultralytics export: normalized [0,1]
            float cx = p[0] * S;
            float cy = p[1] * S;
            float w = p[2] * S;
            float h = p[3] * S;

            // Tìm class tốt nhất
            int bestCls = 0;
            float bestRaw = p[4];
            for (int c = 1; c < numClasses; c++)
            {
                if (p[4 + c] > bestRaw) { bestRaw = p[4 + c]; bestCls = c; }
            }

            // ← LUÔN sigmoid, không điều kiện
            float score = 1.f / (1.f + std::exp(-bestRaw));
            if (score < conf) continue;

            float x1 = clampf((cx - w * 0.5f - padw) / scale, 0, (float)imgW);
            float y1 = clampf((cy - h * 0.5f - padh) / scale, 0, (float)imgH);
            float x2 = clampf((cx + w * 0.5f - padw) / scale, 0, (float)imgW);
            float y2 = clampf((cy + h * 0.5f - padh) / scale, 0, (float)imgH);

            candidates.push_back({ x1, y1, x2, y2, score, bestCls });
        }

        // RT-DETR đã NMS trong model → không gọi NmsPerClass2
        out = candidates;
        return;
    }


    // CASE 2: YOLO
    bool isTranspose = (shp[1] < shp[2]);

    int numPred = isTranspose ? (int)shp[2] : (int)shp[1];
    int dim = isTranspose ? (int)shp[1] : (int)shp[2];

    int numClasses2 = dim - 4;

    for (int i = 0; i < numPred; i++)
    {
        float cx, cy, w, h;

        if (isTranspose)
        {
            cx = data[0 * numPred + i];
            cy = data[1 * numPred + i];
            w = data[2 * numPred + i];
            h = data[3 * numPred + i];
        }
        else
        {
            const float* p = data + i * dim;
            cx = p[0];
            cy = p[1];
            w = p[2];
            h = p[3];
        }

        bool isNorm = (cx <= 1.5f && cy <= 1.5f && w <= 1.5f && h <= 1.5f);
        if (isNorm)
        {
            cx *= S;
            cy *= S;
            w *= S;
            h *= S;
        }

        int bestCls = 0;
        float bestScore = isTranspose ? data[4 * numPred + i] : data[i * dim + 4];

        for (int c = 1; c < numClasses2; c++)
        {
            float v = isTranspose ?
                data[(4 + c) * numPred + i] :
                data[i * dim + 4 + c];

            if (v > bestScore)
            {
                bestScore = v;
                bestCls = c;
            }
        }

        float score = bestScore;
        if (score < 0.f || score > 1.f)
            score = sigmoidf(score);

        if (score < conf)
            continue;

        float x1 = cx - w * 0.5f;
        float y1 = cy - h * 0.5f;
        float x2 = cx + w * 0.5f;
        float y2 = cy + h * 0.5f;

        x1 = (x1 - padw) / scale;
        y1 = (y1 - padh) / scale;
        x2 = (x2 - padw) / scale;
        y2 = (y2 - padh) / scale;

        x1 = clampf(x1, 0, (float)imgW);
        y1 = clampf(y1, 0, (float)imgH);
        x2 = clampf(x2, 0, (float)imgW);
        y2 = clampf(y2, 0, (float)imgH);

        candidates.push_back({ x1, y1, x2, y2, score, bestCls });
    }

    NmsPerClass2(candidates, iou, out);
}

void OpenVinoYoloHP::Detect(const cv::Mat& bgr, float conf, float iou, bool Is3, std::vector<YoloBox>& out)
{
    if (bgr.empty())
    {
        out.clear();
        return;
    }

    float scale = 1.f;
    int padw = 0, padh = 0;

    Letterbox(bgr, paddedU8, scale, padw, padh);
    BgrToCHWFloat01(paddedU8, inputBlob.data());

    ov::Tensor inTensor(ov::element::f32, { 1,3,(size_t)S,(size_t)S }, inputBlob.data());
    infer.set_input_tensor(inTensor);
    infer.infer();

    ov::Tensor outTensor = infer.get_output_tensor();

    auto shp = outTensor.get_shape();
    {
        std::string s = "Output shape=[";
        for (size_t i = 0; i < shp.size(); i++)
        {
            s += std::to_string(shp[i]);
            if (i + 1 < shp.size()) s += ",";
        }
        s += "]";
        // BeeLog::Write(BeeLog::Level::Info, s);
    }

    out.clear();

    DecodeAuto_Yolo_RTDETR(
        outTensor,
        conf, iou,
        scale, padw, padh,
        bgr.cols, bgr.rows, S, C,
        out);

    (void)Is3;
}
//void DecodeAuto_Yolo_RTDETR(
//    const ov::Tensor& t,
//    float conf,
//    float iou,
//    float scale, int padw, int padh,
//    int imgW, int imgH,
//    int S,
//    std::vector<YoloBox>& out)
//{
//    out.clear();
//
//#pragma warning(push)
//#pragma warning(disable: 4996)
//    const float* data = t.data<const float>();
//#pragma warning(pop)
//
//    auto shp = t.get_shape();
//    if (shp.size() != 3) return;
//
//    int N = (int)shp[1];
//    int D = (int)shp[2];
//
//    std::vector<YoloBox> candidates;
//    candidates.reserve(512);
//
//    // CASE 1: RT-DETR
//    if (N == 300 && D >= 6 && D <= 32)
//    {
//        int numClasses = D - 4;
//
//        for (int i = 0; i < N; i++)
//        {
//            const float* p = data + i * D;
//
//            float cx = p[0];
//            float cy = p[1];
//            float w = p[2];
//            float h = p[3];
//
//            bool isNorm = (cx <= 1.5f && cy <= 1.5f && w <= 1.5f && h <= 1.5f);
//            if (isNorm)
//            {
//                cx *= S;
//                cy *= S;
//                w *= S;
//                h *= S;
//            }
//
//            int bestCls = 0;
//            float bestScore = p[4];
//
//            for (int c = 1; c < numClasses; c++)
//            {
//                if (p[4 + c] > bestScore)
//                {
//                    bestScore = p[4 + c];
//                    bestCls = c;
//                }
//            }
//
//            float score = bestScore;
//            if (score < 0.f || score > 1.f)
//                score = sigmoidf(score);
//
//            if (score < conf)
//                continue;
//
//            float x1 = cx - w * 0.5f;
//            float y1 = cy - h * 0.5f;
//            float x2 = cx + w * 0.5f;
//            float y2 = cy + h * 0.5f;
//
//            x1 = (x1 - padw) / scale;
//            y1 = (y1 - padh) / scale;
//            x2 = (x2 - padw) / scale;
//            y2 = (y2 - padh) / scale;
//
//            x1 = clampf(x1, 0, (float)imgW);
//            y1 = clampf(y1, 0, (float)imgH);
//            x2 = clampf(x2, 0, (float)imgW);
//            y2 = clampf(y2, 0, (float)imgH);
//
//            candidates.push_back({ x1, y1, x2, y2, score, bestCls });
//        }
//
//        out = candidates; // RT-DETR đã NMS
//        return;
//    }
//
//    // CASE 2: YOLO
//    bool isTranspose = (shp[1] < shp[2]);
//
//    int numPred = isTranspose ? (int)shp[2] : (int)shp[1];
//    int dim = isTranspose ? (int)shp[1] : (int)shp[2];
//
//    int numClasses = dim - 4;
//
//    for (int i = 0; i < numPred; i++)
//    {
//        float cx, cy, w, h;
//
//        if (isTranspose)
//        {
//            cx = data[0 * numPred + i];
//            cy = data[1 * numPred + i];
//            w = data[2 * numPred + i];
//            h = data[3 * numPred + i];
//        }
//        else
//        {
//            const float* p = data + i * dim;
//            cx = p[0];
//            cy = p[1];
//            w = p[2];
//            h = p[3];
//        }
//
//        bool isNorm = (cx <= 1.5f && cy <= 1.5f && w <= 1.5f && h <= 1.5f);
//        if (isNorm)
//        {
//            cx *= S;
//            cy *= S;
//            w *= S;
//            h *= S;
//        }
//
//        int bestCls = 0;
//        float bestScore = isTranspose ? data[4 * numPred + i] : data[i * dim + 4];
//
//        for (int c = 1; c < numClasses; c++)
//        {
//            float v = isTranspose ?
//                data[(4 + c) * numPred + i] :
//                data[i * dim + 4 + c];
//
//            if (v > bestScore)
//            {
//                bestScore = v;
//                bestCls = c;
//            }
//        }
//
//        float score = bestScore;
//        if (score < 0.f || score > 1.f)
//            score = sigmoidf(score);
//
//        if (score < conf)
//            continue;
//
//        float x1 = cx - w * 0.5f;
//        float y1 = cy - h * 0.5f;
//        float x2 = cx + w * 0.5f;
//        float y2 = cy + h * 0.5f;
//
//        x1 = (x1 - padw) / scale;
//        y1 = (y1 - padh) / scale;
//        x2 = (x2 - padw) / scale;
//        y2 = (y2 - padh) / scale;
//
//        x1 = clampf(x1, 0, (float)imgW);
//        y1 = clampf(y1, 0, (float)imgH);
//        x2 = clampf(x2, 0, (float)imgW);
//        y2 = clampf(y2, 0, (float)imgH);
//
//        candidates.push_back({ x1, y1, x2, y2, score, bestCls });
//    }
//
//    NmsPerClass2(candidates, iou, out);
//}
//
//void OpenVinoYoloHP::Detect(const cv::Mat& bgr, float conf, float iou, bool Is3, std::vector<YoloBox>& out)
//{
//    if (bgr.empty())
//    {
//        out.clear();
//        return;
//    }
//
//    float scale = 1.f;
//    int padw = 0, padh = 0;
//
//    Letterbox(bgr, paddedU8, scale, padw, padh);
//    BgrToCHWFloat01(paddedU8, inputBlob.data());
//
//    ov::Tensor inTensor(ov::element::f32, { 1,3,(size_t)S,(size_t)S }, inputBlob.data());
//    infer.set_input_tensor(inTensor);
//    infer.infer();
//
//    ov::Tensor outTensor = infer.get_output_tensor();
//
//    auto shp = outTensor.get_shape();
//    {
//        std::string s = "Output shape=[";
//        for (size_t i = 0; i < shp.size(); i++)
//        {
//            s += std::to_string(shp[i]);
//            if (i + 1 < shp.size()) s += ",";
//        }
//        s += "]";
//        // BeeLog::Write(BeeLog::Level::Info, s);
//    }
//
//    out.clear();
//
//    DecodeAuto_Yolo_RTDETR(
//        outTensor,
//        conf, iou,
//        scale, padw, padh,
//        bgr.cols, bgr.rows, S,
//        out);
//
//    (void)Is3;
//}