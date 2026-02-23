#include "OpenVinoYoloHP.h"
#include <algorithm>
#include <cmath>

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
    // model->reshape({{model->input().get_any_name(), {1,3,S,S}}});

    // Compile for CPU
    // You can set performance hints:
    ov::AnyMap config;
    config[ov::hint::performance_mode.name()] = ov::hint::PerformanceMode::LATENCY;
    
    config[ov::inference_num_threads.name()] = numThreads;
    compiled = core.compile_model(model, "CPU", config);
    infer = compiled.create_infer_request();

    inputPort = compiled.input();
    outputPort = compiled.output();

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
        Detect(dummy, 0.25f, 0.45f, out);
}

void OpenVinoYoloHP::Letterbox(const cv::Mat& src, cv::Mat& dst, float& scale, int& padw, int& padh)
{
    int w = src.cols, h = src.rows;
    scale = std::min((float)S / w, (float)S / h);

    int nw = (int)std::round(w * scale);
    int nh = (int)std::round(h * scale);

    padw = (S - nw) / 2;
    padh = (S - nh) / 2;

    dst.setTo(cv::Scalar(0, 0, 0));
    cv::Mat resized;
    cv::resize(src, resized, cv::Size(nw, nh), 0, 0, cv::INTER_LINEAR);
    resized.copyTo(dst(cv::Rect(padw, padh, nw, nh)));
}

void OpenVinoYoloHP::BgrToCHWFloat01(const cv::Mat& u8, float* dstCHW)
{
    // u8: SxS CV_8UC3 BGR
    const int HW = S * S;
    float* cB = dstCHW;
    float* cG = dstCHW + HW;
    float* cR = dstCHW + 2 * HW;

    for (int y = 0; y < S; y++)
    {
        const cv::Vec3b* row = u8.ptr<cv::Vec3b>(y);
        int base = y * S;
        for (int x = 0; x < S; x++)
        {
            const cv::Vec3b& v = row[x];
            int i = base + x;
            cB[i] = v[0] / 255.0f;
            cG[i] = v[1] / 255.0f;
            cR[i] = v[2] / 255.0f;
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
void OpenVinoYoloHP:: DecodeDetectionOutput(
    const ov::Tensor& t,
    float conf,
    float scale, int padw, int padh,
    int imgW, int imgH,
    std::vector<YoloBox>& out)
{
    out.clear();
#pragma warning(push)
#pragma warning(disable: 4996)
    const float* p = t.data<const float>();
#pragma warning(pop)
    int num = (int)t.get_shape()[1]; // 300

    for (int i = 0; i < num; i++)
    {
        float x1 = p[i * 6 + 0];
        float y1 = p[i * 6 + 1];
        float x2 = p[i * 6 + 2];
        float y2 = p[i * 6 + 3];
        float score = p[i * 6 + 4];
        int   cls = (int)p[i * 6 + 5];

        if (score < conf) continue;

        // bỏ pad
        x1 = (x1 - padw) / scale;
        y1 = (y1 - padh) / scale;
        x2 = (x2 - padw) / scale;
        y2 = (y2 - padh) / scale;

        x1 = std::max(0.f, std::min((float)imgW - 1, x1));
        y1 = std::max(0.f, std::min((float)imgH - 1, y1));
        x2 = std::max(0.f, std::min((float)imgW - 1, x2));
        y2 = std::max(0.f, std::min((float)imgH - 1, y2));

        out.push_back({ x1,y1,x2,y2,score,cls });
    }
}

void OpenVinoYoloHP::DecodeAnyLayout(const ov::Tensor& outTensor,
    float conf, float scale, int padw, int padh, int srcW, int srcH,
    std::vector<YoloBox>& cand)
{
    cand.clear();
#pragma warning(push)
#pragma warning(disable: 4996)
    const float* out = outTensor.data<const float>();
#pragma warning(pop)
    auto shp = outTensor.get_shape(); // e.g. [1,N,E] or [1,E,N]

    if (shp.size() != 3) return;

    // Two common layouts:
    // A) [1, N, E]  (E = 5 + C)
    // B) [1, E, N]  (transpose)
    const int64_t d1 = (int64_t)shp[1];
    const int64_t d2 = (int64_t)shp[2];

    bool layout_N_E = (d2 >= 6);                // [1,N,E]
    bool layout_E_N = (!layout_N_E && d1 >= 6); // [1,E,N]

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

        // access helper: value at [e, i] with base [1,E,N]
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

void OpenVinoYoloHP::Detect(const cv::Mat& bgr, float conf, float iou, std::vector<YoloBox>& out)
{
    //BeeLog::Timer tAll("Detect");

    if (bgr.empty()) { out.clear(); return; }

    float scale; int padw, padh;

    {
      //  BeeLog::Timer t0("Letterbox");
        Letterbox(bgr, paddedU8, scale, padw, padh);
    }

   // BeeLog::Writef(BeeLog::Level::Info, "in=%dx%d  S=%d  scale=%.6f pad=(%d,%d)",
     //   bgr.cols, bgr.rows, S, scale, padw, padh);
   // cv::cvtColor(paddedU8, paddedU8, cv::COLOR_BGR2RGB);
    {
      //  BeeLog::Timer t1("BgrToCHWFloat01");
        BgrToCHWFloat01(paddedU8, inputBlob.data());
    }

    {
       // BeeLog::Timer t2("SetInputTensor");
        ov::Tensor inTensor(ov::element::f32, { 1,3,(size_t)S,(size_t)S }, inputBlob.data());
        infer.set_input_tensor(inTensor);
    }

    {
       // BeeLog::Timer t3("Infer");
        infer.infer();
    }

    ov::Tensor outTensor;
    {
        //BeeLog::Timer t4("GetOutput");
        outTensor = infer.get_output_tensor();
    }

    // log output tensor shape (debug nhanh)
    //if (BeeLog::IsEnabled())
    //{
    //    auto shp = outTensor.get_shape();
    //    std::string s = "out_shape=[";
    //    for (size_t i = 0; i < shp.size(); i++) { s += std::to_string(shp[i]); if (i + 1 < shp.size()) s += ","; }
    //    s += "]";
    //  //  BeeLog::Write(BeeLog::Level::Info, s);
    //}

    {
      //  BeeLog::Timer t5("Decode");
        DecodeDetectionOutput(outTensor, conf, scale, padw, padh, bgr.cols, bgr.rows, candidates);

       // DecodeAnyLayout(outTensor, conf, scale, padw, padh, bgr.cols, bgr.rows, candidates);
      //  BeeLog::Writef(BeeLog::Level::Info, "candidates=%d", (int)candidates.size());
    }

    {
       // BeeLog::Timer t6("NMS");
        NmsPerClass(candidates, iou, out);
       // BeeLog::Writef(BeeLog::Level::Info, "final=%d", (int)out.size());
    }
}
