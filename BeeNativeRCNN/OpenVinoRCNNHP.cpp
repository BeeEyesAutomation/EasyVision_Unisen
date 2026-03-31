#include "OpenVinoRCNNHP.h"
#include <algorithm>
#include <cmath>

static inline float IoU_Box(const RCNNBox& a, const RCNNBox& b)
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
}static std::string ShapeToString(const ov::Shape& shp)
{
    std::string s = "[";
    for (size_t i = 0; i < shp.size(); i++)
    {
        s += std::to_string(shp[i]);
        if (i + 1 < shp.size()) s += ",";
    }
    s += "]";
    return s;
}

static std::string TypeToString(const ov::element::Type& t)
{
    return t.get_type_name();
}

static inline void clamp_box(float& x1, float& y1, float& x2, float& y2, int w, int h)
{
    x1 = std::max(0.f, std::min(x1, (float)(w - 1)));
    y1 = std::max(0.f, std::min(y1, (float)(h - 1)));
    x2 = std::max(0.f, std::min(x2, (float)(w - 1)));
    y2 = std::max(0.f, std::min(y2, (float)(h - 1)));
}

OpenVinoRCNNHP::OpenVinoRCNNHP(
    const std::wstring& xmlPath,
    int inputW,
    int inputH,
    int numClasses,
    int numThreads)
{
    W = inputW;
    H = inputH;
    C = numClasses;

    model = core.read_model(xmlPath);

    ov::AnyMap config;
    config[ov::hint::performance_mode.name()] = ov::hint::PerformanceMode::LATENCY;
    config[ov::inference_num_threads.name()] = numThreads;

    compiled = core.compile_model(model, "CPU", config);
    infer = compiled.create_infer_request();

    inputPort = compiled.input();

    outputPorts.clear();
    for (size_t i = 0; i < compiled.outputs().size(); i++)
        outputPorts.push_back(compiled.output(i));

    inputBlob.resize((size_t)1 * 3 * W * H);
    paddedU8.create(H, W, CV_8UC3);

    candidates.reserve(4096);

    AnalyzeModel();
}

void OpenVinoRCNNHP::AnalyzeModel()
{
    modelKind = ModelKind::YoloRaw;
    preprocessKind = PreprocessKind::LetterboxYolo;

    auto outs = compiled.outputs();

    if (outs.size() >= 3)
    {
        // ưu tiên nhận diện RCNN multi-output
        modelKind = ModelKind::RCNN_MultiOutput;
        preprocessKind = PreprocessKind::ResizePlain;
        return;
    }

    if (outs.size() == 1)
    {
        auto shp = outs[0].get_shape();

        // [1,N,6] hoặc [N,6] => detection output
        if ((shp.size() == 3 && shp[shp.size() - 1] == 6) ||
            (shp.size() == 2 && shp[1] == 6))
        {
            modelKind = ModelKind::DetectionOutput;
            preprocessKind = PreprocessKind::ResizePlain;
            return;
        }

        // YOLO raw
        if (shp.size() == 3)
        {
            modelKind = ModelKind::YoloRaw;
            preprocessKind = PreprocessKind::LetterboxYolo;
            return;
        }
    }
}

void OpenVinoRCNNHP::Warmup(int iters)
{
    if (!compiled)
        return;

    cv::Mat dummy(480, 640, CV_8UC3, cv::Scalar(0, 0, 0));
    std::vector<RCNNBox> out;
    out.reserve(128);

    for (int i = 0; i < iters; i++)
    {
        try
        {
            Detect(dummy, 0.25f, 0.45f, false, out);
        }
        catch (...)
        {
        }
    }
}

void OpenVinoRCNNHP::Letterbox(const cv::Mat& src, cv::Mat& dst, float& scale, int& padw, int& padh)
{
    int w = src.cols, h = src.rows;
    scale = std::min((float)W / w, (float)H / h);

    int nw = (int)std::round(w * scale);
    int nh = (int)std::round(h * scale);

    padw = (W - nw) / 2;
    padh = (H - nh) / 2;

    dst.setTo(cv::Scalar(0, 0, 0));
    cv::Mat resized;
    cv::resize(src, resized, cv::Size(nw, nh), 0, 0, cv::INTER_LINEAR);
    resized.copyTo(dst(cv::Rect(padw, padh, nw, nh)));
}

void OpenVinoRCNNHP::ResizePlain(const cv::Mat& src, cv::Mat& dst, float& scaleX, float& scaleY)
{
    cv::resize(src, dst, cv::Size(W, H), 0, 0, cv::INTER_LINEAR);
    scaleX = (float)W / (float)src.cols;
    scaleY = (float)H / (float)src.rows;
}

void OpenVinoRCNNHP::BgrToCHWFloat01(const cv::Mat& u8, float* dstCHW)
{
    const int HW = W * H;

    float* cB = dstCHW;
    float* cG = dstCHW + HW;
    float* cR = dstCHW + 2 * HW;

    for (int y = 0; y < H; y++)
    {
        const cv::Vec3b* row = u8.ptr<cv::Vec3b>(y);
        int base = y * W;

        for (int x = 0; x < W; x++)
        {
            const cv::Vec3b& v = row[x];
            int i = base + x;

            cB[i] = v[0] / 255.0f;
            cG[i] = v[1] / 255.0f;
            cR[i] = v[2] / 255.0f;
        }
    }
}
void OpenVinoRCNNHP::DecodeYoloAuto(
    const ov::Tensor& t,
    float conf,
    float scale, int padw, int padh,
    int imgW, int imgH,
    std::vector<RCNNBox>& out)
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

    bool is_C_N = (d1 >= 5 && d1 <= 512 && d2 > d1); // [1,C,N]
    bool is_N_C = (d2 >= 5 && d2 <= 512 && d1 > d2); // [1,N,C]

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

void OpenVinoRCNNHP::DecodeDetectionOutput(
    const ov::Tensor& t,
    float conf,
    float scaleX, float scaleY,
    int padw, int padh,
    int imgW, int imgH,
    std::vector<RCNNBox>& out)
{
    out.clear();

#pragma warning(push)
#pragma warning(disable:4996)
    const float* p = t.data<const float>();
#pragma warning(pop)

    auto shp = t.get_shape();

    int num = 0;
    if (shp.size() == 3)      num = (int)shp[1]; // [1,N,6]
    else if (shp.size() == 2) num = (int)shp[0]; // [N,6]
    else return;

    for (int i = 0; i < num; i++)
    {
        const float* row = p + i * 6;

        float x1 = row[0];
        float y1 = row[1];
        float x2 = row[2];
        float y2 = row[3];
        float score = row[4];
        int cls = (int)row[5];

        if (score < conf) continue;

        // RCNN/DetectionOutput dùng resize thường, không phải letterbox
        x1 = (x1 - padw) / scaleX;
        y1 = (y1 - padh) / scaleY;
        x2 = (x2 - padw) / scaleX;
        y2 = (y2 - padh) / scaleY;

        clamp_box(x1, y1, x2, y2, imgW, imgH);
        out.push_back({ x1, y1, x2, y2, score, cls });
    }
}

void OpenVinoRCNNHP::DecodeRCNNMultiOutput(
    const ov::Tensor& boxesT,
    const ov::Tensor& labelsT,
    const ov::Tensor& scoresT,
    float conf,
    float scaleX, float scaleY,
    int padw, int padh,
    int imgW, int imgH,
    std::vector<RCNNBox>& out)
{
    out.clear();

    const auto bsh = boxesT.get_shape();
    const auto lsh = labelsT.get_shape();
    const auto ssh = scoresT.get_shape();

    int num = 0;
    if (bsh.size() == 2 && bsh[1] == 4)
        num = (int)bsh[0];
    else if (bsh.size() == 3 && bsh[0] == 1 && bsh[2] == 4)
        num = (int)bsh[1];
    else
    {
        BeeLog::Writef(BeeLog::Level::Error,
            "DecodeRCNNMultiOutput: unsupported boxes shape=%s",
            ShapeToString(bsh).c_str());
        return;
    }

    int numLabels = 0;
    if (lsh.size() == 1) numLabels = (int)lsh[0];
    else if (lsh.size() == 2 && lsh[0] == 1) numLabels = (int)lsh[1];
    else
    {
        BeeLog::Writef(BeeLog::Level::Error,
            "DecodeRCNNMultiOutput: unsupported labels shape=%s",
            ShapeToString(lsh).c_str());
        return;
    }

    int numScores = 0;
    if (ssh.size() == 1) numScores = (int)ssh[0];
    else if (ssh.size() == 2 && ssh[0] == 1) numScores = (int)ssh[1];
    else
    {
        BeeLog::Writef(BeeLog::Level::Error,
            "DecodeRCNNMultiOutput: unsupported scores shape=%s",
            ShapeToString(ssh).c_str());
        return;
    }

    num = std::min(num, std::min(numLabels, numScores));
    if (num <= 0) return;

#pragma warning(push)
#pragma warning(disable:4996)
    const float* boxes = nullptr;
    if (boxesT.get_element_type() == ov::element::f32)
        boxes = boxesT.data<const float>();
    else
    {
        BeeLog::Writef(BeeLog::Level::Error,
            "DecodeRCNNMultiOutput: boxes type must be f32, got %s",
            TypeToString(boxesT.get_element_type()).c_str());
        return;
    }
#pragma warning(pop)

    auto labelType = labelsT.get_element_type();
    auto scoreType = scoresT.get_element_type();

    auto getLabel = [&](int i) -> int
        {
#pragma warning(push)
#pragma warning(disable:4996)
            if (labelType == ov::element::i64)
                return (int)labelsT.data<const int64_t>()[i];
            if (labelType == ov::element::i32)
                return (int)labelsT.data<const int32_t>()[i];
            if (labelType == ov::element::u32)
                return (int)labelsT.data<const uint32_t>()[i];
            if (labelType == ov::element::f32)
                return (int)labelsT.data<const float>()[i];
#pragma warning(pop)

            BeeLog::Writef(BeeLog::Level::Warn,
                "DecodeRCNNMultiOutput: unsupported labels type=%s",
                TypeToString(labelType).c_str());
            return 0;
        };

    auto getScore = [&](int i) -> float
        {
#pragma warning(push)
#pragma warning(disable:4996)
            if (scoreType == ov::element::f32)
                return scoresT.data<const float>()[i];
            if (scoreType == ov::element::f16)
                return (float)scoresT.data<const ov::float16>()[i];
            if (scoreType == ov::element::i64)
                return (float)scoresT.data<const int64_t>()[i];
            if (scoreType == ov::element::i32)
                return (float)scoresT.data<const int32_t>()[i];
#pragma warning(pop)

            BeeLog::Writef(BeeLog::Level::Warn,
                "DecodeRCNNMultiOutput: unsupported scores type=%s",
                TypeToString(scoreType).c_str());
            return 0.f;
        };

    for (int i = 0; i < num; i++)
    {
        const float* b = boxes + i * 4;

        float score = getScore(i);
        if (score < conf) continue;

        int cls = getLabel(i);

        float x1 = (b[0] - padw) / scaleX;
        float y1 = (b[1] - padh) / scaleY;
        float x2 = (b[2] - padw) / scaleX;
        float y2 = (b[3] - padh) / scaleY;

        if (x2 <= 1.5f && y2 <= 1.5f)
        {
            x1 *= imgW; x2 *= imgW;
            y1 *= imgH; y2 *= imgH;
        }

        clamp_box(x1, y1, x2, y2, imgW, imgH);
        out.push_back({ x1, y1, x2, y2, score, cls });
    }
}

void OpenVinoRCNNHP::NmsPerClass(std::vector<RCNNBox>& cand, float iou, std::vector<RCNNBox>& out)
{
    out.clear();
    if (cand.empty()) return;

    std::sort(cand.begin(), cand.end(),
        [](const RCNNBox& a, const RCNNBox& b) { return a.score > b.score; });

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

void OpenVinoRCNNHP::Detect(const cv::Mat& bgr, float conf, float iou, bool Is3, std::vector<RCNNBox>& out)
{
    try
    {
        BeeLog::Timer tAll("Detect");

        if (bgr.empty())
        {
            BeeLog::Write(BeeLog::Level::Warn, "bgr empty");
            out.clear();
            return;
        }

        BeeLog::Writef(BeeLog::Level::Info,
            "input image: w=%d h=%d step=%d", bgr.cols, bgr.rows, (int)bgr.step);

        float scale = 1.f;
        float scaleX = 1.f, scaleY = 1.f;
        int padw = 0, padh = 0;

        if (preprocessKind == PreprocessKind::LetterboxYolo)
        {
            BeeLog::Timer t0("Letterbox");
            Letterbox(bgr, paddedU8, scale, padw, padh);
            scaleX = scaleY = scale;

            BeeLog::Writef(BeeLog::Level::Info,
                "preprocess=LetterboxYolo W=%d H=%d scale=%.6f padw=%d padh=%d",
                W, H, scale, padw, padh);
        }
        else
        {
            BeeLog::Timer t0("ResizePlain");
            ResizePlain(bgr, paddedU8, scaleX, scaleY);
            padw = padh = 0;

            BeeLog::Writef(BeeLog::Level::Info,
                "preprocess=ResizePlain W=%d H=%d scaleX=%.6f scaleY=%.6f",
                W, H, scaleX, scaleY);
        }

        {
            BeeLog::Timer t1("BgrToCHWFloat01");
            BgrToCHWFloat01(paddedU8, inputBlob.data());
        }

        ov::Tensor inTensor(
            ov::element::f32,
            { 1, 3, (size_t)H, (size_t)W },
            inputBlob.data());

        BeeLog::Writef(BeeLog::Level::Info,
            "input tensor shape=%s type=%s",
            ShapeToString(inTensor.get_shape()).c_str(),
            TypeToString(inTensor.get_element_type()).c_str());

        {
            BeeLog::Timer t2("SetInputTensor");
            infer.set_input_tensor(inTensor);
        }

        {
            BeeLog::Timer t3("Infer");
            infer.infer();
        }

        candidates.clear();

        if (modelKind == ModelKind::RCNN_MultiOutput && outputPorts.size() >= 3)
        {
            BeeLog::Write(BeeLog::Level::Info, "decode path = RCNN_MultiOutput");

            ov::Tensor ta = infer.get_output_tensor(0);
            ov::Tensor tb = infer.get_output_tensor(1);
            ov::Tensor tc = infer.get_output_tensor(2);

            auto logTensorInfo = [&](const char* name, const ov::Tensor& t)
                {
                    BeeLog::Writef(BeeLog::Level::Info, "%s shape=%s type=%s",
                        name,
                        ShapeToString(t.get_shape()).c_str(),
                        TypeToString(t.get_element_type()).c_str());
                };

            logTensorInfo("output0", ta);
            logTensorInfo("output1", tb);
            logTensorInfo("output2", tc);

            ov::Tensor boxesT, labelsT, scoresT;
            bool hasBoxes = false, hasLabels = false, hasScores = false;

            std::vector<ov::Tensor> ts = { ta, tb, tc };
            for (auto& t : ts)
            {
                auto shp = t.get_shape();
                auto typ = t.get_element_type();

                bool isBoxes =
                    (shp.size() == 2 && shp[1] == 4) ||
                    (shp.size() == 3 && shp[0] == 1 && shp[2] == 4);

                bool isVector =
                    (shp.size() == 1) ||
                    (shp.size() == 2 && shp[0] == 1);

                if (!hasBoxes && isBoxes)
                {
                    boxesT = t;
                    hasBoxes = true;
                }
                else if (!hasLabels && isVector &&
                    (typ == ov::element::i64 || typ == ov::element::i32 || typ == ov::element::u32))
                {
                    labelsT = t;
                    hasLabels = true;
                }
                else if (!hasScores && isVector &&
                    (typ == ov::element::f32 || typ == ov::element::f16))
                {
                    scoresT = t;
                    hasScores = true;
                }
            }

            if (hasBoxes && hasLabels && hasScores)
            {
                BeeLog::Write(BeeLog::Level::Info, "RCNN outputs mapped successfully");

                BeeLog::Timer t4("DecodeRCNNMultiOutput");
                DecodeRCNNMultiOutput(
                    boxesT, labelsT, scoresT,
                    conf, scaleX, scaleY, padw, padh,
                    bgr.cols, bgr.rows,
                    candidates);
            }
            else
            {
                BeeLog::Writef(BeeLog::Level::Error,
                    "RCNN_MultiOutput mapping failed: hasBoxes=%d hasLabels=%d hasScores=%d",
                    (int)hasBoxes, (int)hasLabels, (int)hasScores);
                candidates.clear();
            }
        }
        else
        {
            ov::Tensor outTensor = infer.get_output_tensor(0);
            auto shp = outTensor.get_shape();

            BeeLog::Writef(BeeLog::Level::Info, "output0 shape=%s type=%s",
                ShapeToString(shp).c_str(),
                TypeToString(outTensor.get_element_type()).c_str());

            if ((shp.size() == 3 && shp[2] == 6) || (shp.size() == 2 && shp[1] == 6))
            {
                BeeLog::Write(BeeLog::Level::Info, "decode path = DetectionOutput");

                BeeLog::Timer t4("DecodeDetectionOutput");
                DecodeDetectionOutput(outTensor, conf, scaleX, scaleY, padw, padh, bgr.cols, bgr.rows, candidates);
            }
            else
            {
                BeeLog::Write(BeeLog::Level::Info, "decode path = YoloAuto");

                BeeLog::Timer t4("DecodeYoloAuto");
                DecodeYoloAuto(outTensor, conf, scale, padw, padh, bgr.cols, bgr.rows, candidates);
            }
        }

        BeeLog::Writef(BeeLog::Level::Info, "candidates=%d", (int)candidates.size());

        {
            BeeLog::Timer t5("NmsPerClass");
            NmsPerClass(candidates, iou, out);
        }

        BeeLog::Writef(BeeLog::Level::Info, "final boxes=%d", (int)out.size());
    }
    catch (const std::exception& ex)
    {
        BeeLog::Writef(BeeLog::Level::Error, "Detect exception: %s", ex.what());
        out.clear();
    }
    catch (...)
    {
        BeeLog::Write(BeeLog::Level::Error, "Detect exception: unknown");
        out.clear();
    }
}