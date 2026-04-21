// ============================================================
//  OpenVinoYoloHP.cpp
//  Fixes vs original:
//   1. BGR→RGB channel order (BgrToCHWFloat01)
//   2. Letterbox pad color 114 (Ultralytics chuẩn)
//   3. RT-DETR: LUÔN sigmoid score, không điều kiện
//   4. RT-DETR: auto-detect normalized vs pixel coord
//   5. RT-DETR: isRTDETR dùng numClasses thay vì hardcode N==300
//   6. BeeLog debug đầy đủ
//   7. Dọn sạch code cũ/comment thừa
// ============================================================
#include "OpenVinoYoloHP.h"
#include "YoloDebugLog.h"
#include <algorithm>
#include <cmath>
#include <stdexcept>
#include <sstream>
#include <cstdio>
#include <fstream>

#include <cctype>

#ifdef _WIN32
#include <io.h>
#endif
// ─────────────────────────────────────────────────────────────
// Static helpers
// ─────────────────────────────────────────────────────────────

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

static inline float sigmoidf(float x)
{
    return 1.f / (1.f + std::exp(-x));
}

static inline float clampf(float v, float lo, float hi)
{
    return v < lo ? lo : (v > hi ? hi : v);
}

static inline void clamp_box(float& x1, float& y1, float& x2, float& y2, int w, int h)
{
    x1 = std::max(0.f, std::min(x1, (float)(w - 1)));
    y1 = std::max(0.f, std::min(y1, (float)(h - 1)));
    x2 = std::max(0.f, std::min(x2, (float)(w - 1)));
    y2 = std::max(0.f, std::min(y2, (float)(h - 1)));
}

// ─────────────────────────────────────────────────────────────
// NMS (per-class, score-sorted)
// ─────────────────────────────────────────────────────────────

static void NmsPerClass2(std::vector<YoloBox>& cand, float iouThresh,
    std::vector<YoloBox>& out)
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
        out.push_back(cand[i]);
        for (size_t j = i + 1; j < cand.size(); j++)
        {
            if (removed[j]) continue;
            if (cand[i].classId != cand[j].classId) continue;
            if (IoU_Box(cand[i], cand[j]) > iouThresh) removed[j] = 1;
        }
    }
}

// ─────────────────────────────────────────────────────────────
// DecodeAuto_Yolo_RTDETR  — global function
// ─────────────────────────────────────────────────────────────
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
    if (shp.size() != 3)
    {
      /*  BeeLog::Writef(BeeLog::Level::Error,
            "DecodeAuto: tensor rank=%zu (expected 3)", shp.size());*/
        return;
    }

    const int N = (int)shp[1];
    const int D = (int)shp[2];

    //BeeLog::Writef(BeeLog::Level::Info,
    //    "DecodeAuto: shape=[1,%d,%d]  numClasses=%d  conf=%.3f  "
    //    "scale=%.4f  padw=%d  padh=%d  img=%dx%d",
    //    N, D, numClasses, conf, scale, padw, padh, imgW, imgH);

    if (N > 0 && D >= 5)
    {
        char buf[512]; int pos = 0;
        pos += snprintf(buf + pos, sizeof(buf) - pos, "box[0] raw:");
        for (int k = 0; k < std::min(D, 12); k++)
            pos += snprintf(buf + pos, sizeof(buf) - pos, " %.4f", data[k]);
     //   BeeLog::Write(BeeLog::Level::Info, buf);

        pos = 0;
        pos += snprintf(buf + pos, sizeof(buf) - pos, "box[0] sigmoid(cls):");
        for (int k = 4; k < std::min(D, 4 + numClasses); k++)
            pos += snprintf(buf + pos, sizeof(buf) - pos, " %.4f", sigmoidf(data[k]));
      //  BeeLog::Write(BeeLog::Level::Info, buf);
    }

    // RT-DETR: [1, N_query, 4+numClasses]  D nho, N lon
    // YOLO:    [1, 4+numClasses, N_anchor] D lon, N nho
    const bool isRTDETR = (D == numClasses + 4) && (N > D);

  /*  BeeLog::Writef(BeeLog::Level::Info,
        "isRTDETR=%s  (D=%d==numCls+4=%d? %s)  (N=%d>D=%d? %s)",
        isRTDETR ? "YES" : "NO",
        D, numClasses + 4, (D == numClasses + 4) ? "Y" : "N",
        N, D, (N > D) ? "Y" : "N");*/

    std::vector<YoloBox> candidates;
    candidates.reserve(512);

    // ════════════════════════════════════════════════════════
    // CASE 1: RT-DETR
    // ════════════════════════════════════════════════════════
    if (isRTDETR)
    {
        // Ultralytics OpenVINO RT-DETR export ra normalized [0,1]
        // Safe: kiem tra box[0]
        bool coordIsNorm = true;
        if (N > 0)
        {
            float mx = std::max({ data[0], data[1], data[2], data[3] });
            coordIsNorm = (mx <= 2.0f);
        }
       /* BeeLog::Writef(BeeLog::Level::Info,
            "RT-DETR coordIsNorm=%s", coordIsNorm ? "YES" : "NO (pixel)");*/

        int kept = 0;
        for (int i = 0; i < N; i++)
        {
            const float* p = data + i * D;

            float cx = p[0];
            float cy = p[1];
            float w = p[2];
            float h = p[3];

            if (coordIsNorm)
            {
                cx *= S; cy *= S;
                w *= S; h *= S;
            }
            int   bestCls = 0;
            float bestRaw = p[4];
            for (int c = 1; c < numClasses; c++)
            {
                if (p[4 + c] > bestRaw) { bestRaw = p[4 + c]; bestCls = c; }
            }

            // ✅ Model đã sigmoid trong graph → dùng thẳng
            // Chỉ sigmoid nếu giá trị ra ngoài [0,1] (logit thô)
            float score = bestRaw;
            if (score < 0.f || score > 1.f)
                score = sigmoidf(score);

            if (score < conf) continue;
            //int   bestCls = 0;
            //float bestRaw = p[4];
            //for (int c = 1; c < numClasses; c++)
            //{
            //    if (p[4 + c] > bestRaw) { bestRaw = p[4 + c]; bestCls = c; }
            //}

            //// RT-DETR: LUON sigmoid, khong dieu kien
            //float score = sigmoidf(bestRaw);
            //if (score < conf) continue;

            float x1 = clampf((cx - w * 0.5f - padw) / scale, 0.f, (float)imgW);
            float y1 = clampf((cy - h * 0.5f - padh) / scale, 0.f, (float)imgH);
            float x2 = clampf((cx + w * 0.5f - padw) / scale, 0.f, (float)imgW);
            float y2 = clampf((cy + h * 0.5f - padh) / scale, 0.f, (float)imgH);

            if (x2 <= x1 || y2 <= y1) continue;

            candidates.push_back({ x1, y1, x2, y2, score, bestCls });
            kept++;
        }

        /*BeeLog::Writef(BeeLog::Level::Info,
            "RT-DETR: %d/%d queries passed conf=%.3f -> %d boxes",
            kept, N, conf, kept);*/

        // RT-DETR da NMS trong model -> KHONG goi NmsPerClass2
        out = candidates;
        return;
    }

    // ════════════════════════════════════════════════════════
    // CASE 2: YOLO
    // ════════════════════════════════════════════════════════
    const bool isTranspose = (N < D);
    const int  numPred = isTranspose ? D : N;
    const int  dim = isTranspose ? N : D;
    const int  numCls2 = dim - 4;

    /*BeeLog::Writef(BeeLog::Level::Info,
        "YOLO: isTranspose=%s  numPred=%d  dim=%d  numCls=%d",
        isTranspose ? "YES" : "NO", numPred, dim, numCls2);*/

    if (numCls2 <= 0)
    {
       // BeeLog::Write(BeeLog::Level::Error, "YOLO: numCls2<=0, skip");
        return;
    }

    for (int i = 0; i < numPred; i++)
    {
        float cx, cy, w, h;

        if (isTranspose)
        {
            const float* p = data + (size_t)i * dim;
            cx = p[0]; cy = p[1]; w = p[2]; h = p[3];
        }
        else
        {
            cx = data[0 * numPred + i];
            cy = data[1 * numPred + i];
            w = data[2 * numPred + i];
            h = data[3 * numPred + i];
        }

        bool isNorm = (cx <= 1.5f && cy <= 1.5f && w <= 1.5f && h <= 1.5f);
        if (isNorm) { cx *= S; cy *= S; w *= S; h *= S; }

        int   bestCls = 0;
        float bestScore = isTranspose
            ? data[(size_t)i * dim + 4]
            : data[4 * numPred + i];

        for (int c = 1; c < numCls2; c++)
        {
            float v = isTranspose
                ? data[(size_t)i * dim + 4 + c]
                : data[(4 + c) * numPred + i];
            if (v > bestScore) { bestScore = v; bestCls = c; }
        }

        float score = bestScore;
        if (score < 0.f || score > 1.f) score = sigmoidf(score);
        if (score < conf) continue;

        float x1 = clampf((cx - w * 0.5f - padw) / scale, 0.f, (float)imgW);
        float y1 = clampf((cy - h * 0.5f - padh) / scale, 0.f, (float)imgH);
        float x2 = clampf((cx + w * 0.5f - padw) / scale, 0.f, (float)imgW);
        float y2 = clampf((cy + h * 0.5f - padh) / scale, 0.f, (float)imgH);

        if (x2 <= x1 || y2 <= y1) continue;

        candidates.push_back({ x1, y1, x2, y2, score, bestCls });
    }

    NmsPerClass2(candidates, iou, out);

   /* BeeLog::Writef(BeeLog::Level::Info,
        "YOLO: candidates=%zu -> after NMS=%zu", candidates.size(), out.size());*/
}

// ── Hàm đọc metadata.yaml cạnh file .xml ──────────────────
static std::pair<int, int> ReadMetadataYaml(const std::wstring& xmlPath)
{
    int metaS = -1;
    int metaC = -1;

    try
    {
        BeeLog::Write(BeeLog::Level::Info, "YAML step1: find slash");

        size_t slashPos = xmlPath.find_last_of(L"\\/");
        std::wstring folder = (slashPos != std::wstring::npos)
            ? xmlPath.substr(0, slashPos + 1) : L"./";
        std::wstring yamlPathW = folder + L"metadata.yaml";

        BeeLog::Write(BeeLog::Level::Info, "YAML step2: build path OK");

        // Log path an toan: chi convert phan ASCII
        try
        {
            std::string pathLog(yamlPathW.begin(), yamlPathW.end());
            BeeLog::Writef(BeeLog::Level::Info,
                "Reading metadata: %s", pathLog.c_str());
        }
        catch (...)
        {
            BeeLog::Write(BeeLog::Level::Info, "Reading metadata: (path has non-ASCII)");
        }

        BeeLog::Write(BeeLog::Level::Info, "YAML step3: open file");

#ifdef _WIN32
        FILE* fp = nullptr;
        errno_t err = _wfopen_s(&fp, yamlPathW.c_str(), L"r");
        BeeLog::Writef(BeeLog::Level::Info, "YAML step4: _wfopen_s err=%d fp=%s",
            (int)err, fp ? "OK" : "NULL");
        if (err != 0 || !fp)
        {
            BeeLog::Write(BeeLog::Level::Warn,
                "metadata.yaml not found or cannot open");
            return { -1, -1 };
        }
#else
        std::string yamlPathA(yamlPathW.begin(), yamlPathW.end());
        FILE* fp = fopen(yamlPathA.c_str(), "r");
        if (!fp)
        {
            BeeLog::Write(BeeLog::Level::Warn, "metadata.yaml not found");
            return { -1, -1 };
        }
#endif

        BeeLog::Write(BeeLog::Level::Info, "YAML step5: read content");

        std::string content;
        {
            char buf[512];
            while (fgets(buf, sizeof(buf), fp))
                content += buf;
            fclose(fp);
        }

        BeeLog::Writef(BeeLog::Level::Info,
            "YAML step6: read done, size=%zu bytes", content.size());

        // parse giữ nguyên như cũ ...
        bool inImgsz = false;
        bool inNames = false;
        std::vector<int> imgszVals;
        int nameCount = 0;

        std::istringstream ss(content);
        std::string line;

        while (std::getline(ss, line))
        {
            if (!line.empty() && line.back() == '\r') line.pop_back();

            int indent = 0;
            while (indent < (int)line.size() && line[indent] == ' ') indent++;
            if (indent >= (int)line.size()) continue;
            std::string trimmed = line.substr(indent);
            if (trimmed.empty() || trimmed[0] == '#') continue;

            // ✅ Check imgsz item TRƯỚC — "- 640" có thể ở indent=0
            if (inImgsz && !trimmed.empty() && trimmed[0] == '-')
            {
                std::string val = trimmed.substr(1);
                size_t s = val.find_first_not_of(" \t");
                if (s != std::string::npos && std::isdigit((unsigned char)val[s]))
                {
                    int v = std::stoi(val.substr(s));
                    imgszVals.push_back(v);
                    BeeLog::Writef(BeeLog::Level::Info, "YAML imgsz item: %d", v);
                }
                continue;  // ← QUAN TRỌNG: không fall-through xuống top-level
            }

            // Top-level key (indent == 0, không phải '-')
            if (indent == 0)
            {
                // Reset state khi gặp key mới
                inImgsz = false;
                inNames = false;

                if (trimmed.find("imgsz:") == 0)
                {
                    inImgsz = true;
                    // Scalar cùng dòng: "imgsz: 640"
                    size_t colon = trimmed.find(':');
                    std::string rest = trimmed.substr(colon + 1);
                    size_t s = rest.find_first_not_of(" \t");
                    if (s != std::string::npos && std::isdigit((unsigned char)rest[s]))
                    {
                        metaS = std::stoi(rest.substr(s));
                        inImgsz = false;  // scalar, không cần đọc tiếp
                    }
                }
                else if (trimmed.find("names:") == 0)
                {
                    inNames = true;
                }
                continue;
            }

            // names entries: "  0: OK" (indent > 0)
            if (inNames)
            {
                size_t colon = trimmed.find(':');
                if (colon == std::string::npos) continue;

                std::string key = trimmed.substr(0, colon);
                // Trim key
                size_t ks = key.find_first_not_of(" \t");
                if (ks != std::string::npos) key = key.substr(ks);
                size_t ke = key.find_last_not_of(" \t");
                if (ke != std::string::npos) key = key.substr(0, ke + 1);

                bool isNum = !key.empty();
                for (char ch : key)
                    if (!std::isdigit((unsigned char)ch)) { isNum = false; break; }
                if (!isNum) continue;

                std::string val = trimmed.substr(colon + 1);
                size_t vs = val.find_first_not_of(" \t");
                std::string className = (vs != std::string::npos) ? val.substr(vs) : "?";

                BeeLog::Writef(BeeLog::Level::Info,
                    "  class[%s] = %s", key.c_str(), className.c_str());
                nameCount++;
            }
        }

        // Lấy max của imgszVals
        if (!imgszVals.empty() && metaS <= 0)
            metaS = *std::max_element(imgszVals.begin(), imgszVals.end());

        if (nameCount > 0)
            metaC = nameCount;

        BeeLog::Writef(BeeLog::Level::Info,
            "metadata.yaml OK -> imgsz=%d  nc=%d", metaS, metaC);
    }
    catch (const std::exception& ex)
    {
        BeeLog::Writef(BeeLog::Level::Warn,
            "Exception reading metadata.yaml: %s", ex.what());
    }
    catch (...)
    {
        BeeLog::Write(BeeLog::Level::Warn,
            "Unknown exception reading metadata.yaml");
    }

    return { metaS, metaC };
}
OpenVinoYoloHP::OpenVinoYoloHP(
    const std::wstring& xmlPath,
    int inputSize,
    int numClasses,
    int numThreads)
{
    // ════════════════════════════════════════════════════════
    // STEP 1: Đọc metadata.yaml
    // ════════════════════════════════════════════════════════
    auto [metaS, metaC] = ReadMetadataYaml(xmlPath);  // ← gọi hàm mới

    // ════════════════════════════════════════════════════════
    // STEP 2: Xác định S và C
    // ════════════════════════════════════════════════════════
    if (inputSize > 0)
        S = inputSize;
    else if (metaS > 0)
        S = metaS;
    else
        S = 640;

    if (numClasses > 0)
        C = numClasses;
    else if (metaC > 0)
        C = metaC;
    else
        C = 0;  // detect sau compile

    BeeLog::Writef(BeeLog::Level::Info,
        "OpenVinoYoloHP init: S=%d  C=%d  "
        "(param: S=%d C=%d | meta: S=%d C=%d)  threads=%d",
        S, C, inputSize, numClasses, metaS, metaC, numThreads);


    // ════════════════════════════════════════════════════════
    // STEP 3: Load model
    // ════════════════════════════════════════════════════════
    model = core.read_model(xmlPath);

    // Ép static shape nếu model dynamic
    try
    {
        auto pshape = model->input().get_partial_shape();
        if (pshape.is_dynamic())
        {
            model->reshape({
                { model->input().get_any_name(),
                  ov::Shape{1, 3, (size_t)S, (size_t)S} }
                });
            BeeLog::Write(BeeLog::Level::Info,
                "Model reshaped to static [1,3,S,S]");
        }
    }
    catch (...) {}

    // ════════════════════════════════════════════════════════
    // STEP 4: Compile model
    // ════════════════════════════════════════════════════════
    ov::AnyMap config;
    config[ov::hint::performance_mode.name()] = ov::hint::PerformanceMode::LATENCY;
    config[ov::cache_dir.name()] = std::string("./ov_cache");
    (void)numThreads;   // AUTO/GPU không dùng numThreads

    compiled = core.compile_model(model, "AUTO:GPU,CPU", config);
    infer = compiled.create_infer_request();
    inputPort = compiled.input();

    auto outs = compiled.outputs();
    if (outs.empty())
        throw std::runtime_error("OpenVINO compiled model has no outputs.");
    outputPort = outs[0];

    // ════════════════════════════════════════════════════════
    // STEP 5: Auto C từ compiled shape nếu vẫn = 0
    // ════════════════════════════════════════════════════════
    if (C <= 0)
    {
        bool gotC = false;

        // Thử đọc từ compiled output partial shape
        try
        {
            auto outShape = compiled.output().get_partial_shape();
            BeeLog::Writef(BeeLog::Level::Info,
                "Compiled output partial shape rank=%zu",
                (size_t)outShape.rank().get_length());

            if (outShape.rank().is_static() && outShape.rank().get_length() == 3)
            {
                auto d = outShape[2];
                if (d.is_static())
                {
                    int D = (int)d.get_length();
                    C = D - 4;
                    gotC = true;
                    BeeLog::Writef(BeeLog::Level::Info,
                        "Auto C=%d from compiled output shape (D=%d)", C, D);
                }
            }
        }
        catch (...) {}

        // Nếu compiled shape dynamic → probe infer 1 lần
        if (!gotC)
        {
            try
            {
                std::vector<float> tmpBlob((size_t)3 * S * S, 0.447f); // 114/255
                ov::Tensor tmpIn(ov::element::f32,
                    { 1, 3, (size_t)S, (size_t)S }, tmpBlob.data());
                infer.set_input_tensor(tmpIn);
                infer.infer();
                auto tmpOut = infer.get_output_tensor();
                auto realShp = tmpOut.get_shape();
                if (realShp.size() == 3)
                {
                    int D = (int)realShp[2];
                    C = D - 4;
                    gotC = true;
                    BeeLog::Writef(BeeLog::Level::Info,
                        "Auto C=%d from probe infer shape [1,%d,%d]",
                        C, (int)realShp[1], D);
                }
            }
            catch (...) {}
        }

        if (!gotC || C <= 0)
        {
            C = 80;
            BeeLog::Write(BeeLog::Level::Warn,
                "Cannot detect C, fallback C=80");
        }
    }

    // ════════════════════════════════════════════════════════
    // STEP 6: Cấp phát buffer
    // ════════════════════════════════════════════════════════
    inputBlob.resize((size_t)1 * 3 * S * S);
    paddedU8.create(S, S, CV_8UC3);
    candidates.reserve(4096);

    BeeLog::Writef(BeeLog::Level::Info,
        "OpenVinoYoloHP FINAL: S=%d  C=%d", S, C);
    BeeLog::Write(BeeLog::Level::Info, "OpenVinoYoloHP ready");
}
// ─────────────────────────────────────────────────────────────
// Warmup
// ─────────────────────────────────────────────────────────────
void OpenVinoYoloHP::Warmup(int iters)
{
    cv::Mat dummy(S, S, CV_8UC3, cv::Scalar(114, 114, 114));
    std::vector<YoloBox> out;
    out.reserve(32);
    for (int i = 0; i < iters; i++)
        Detect(dummy, 0.25f, 0.45f, false, out);
  //  BeeLog::Writef(BeeLog::Level::Info, "Warmup done (%d iters)", iters);
}

// ─────────────────────────────────────────────────────────────
// Letterbox — pad xam 114 (chuan Ultralytics)
// ─────────────────────────────────────────────────────────────
void OpenVinoYoloHP::Letterbox(const cv::Mat& src, cv::Mat& dst,
    float& scale, int& padw, int& padh)
{
    const int w = src.cols;
    const int h = src.rows;

    scale = std::min((float)S / w, (float)S / h);

    const int nw = std::max(1, (int)(w * scale + 0.5f));
    const int nh = std::max(1, (int)(h * scale + 0.5f));

    padw = (S - nw) >> 1;
    padh = (S - nh) >> 1;

    // FIX: pad mau xam 114 thay vi den 0
    dst.setTo(cv::Scalar(114, 114, 114));
    cv::resize(src, dst(cv::Rect(padw, padh, nw, nh)),
        cv::Size(nw, nh), 0, 0, cv::INTER_LINEAR);
}

// ─────────────────────────────────────────────────────────────
// BgrToCHWFloat01 — BGR(OpenCV) -> RGB CHW float [0,1]
//   FIX: dao thu tu kenh R<->B de khop voi model train RGB
// ─────────────────────────────────────────────────────────────
void OpenVinoYoloHP::BgrToCHWFloat01(const cv::Mat& u8, float* dstCHW)
{
    const int HW = S * S;

    // Model expect: ch0=R, ch1=G, ch2=B
    float* cR = dstCHW;           // channel 0 -> R
    float* cG = dstCHW + HW;      // channel 1 -> G
    float* cB = dstCHW + 2 * HW;  // channel 2 -> B

    const float inv255 = 1.0f / 255.0f;

    for (int y = 0; y < S; y++)
    {
        const uchar* row = u8.ptr<uchar>(y);
        const int    yS = y * S;
        for (int x = 0; x < S; x++)
        {
            // OpenCV BGR: [0]=B [1]=G [2]=R
            cR[yS + x] = row[x * 3 + 2] * inv255;  // R
            cG[yS + x] = row[x * 3 + 1] * inv255;  // G
            cB[yS + x] = row[x * 3 + 0] * inv255;  // B
        }
    }
}

// ─────────────────────────────────────────────────────────────
// Detect — entry point chinh
// ─────────────────────────────────────────────────────────────
void OpenVinoYoloHP::Detect(const cv::Mat& bgr, float conf, float iou,
    bool Is3, std::vector<YoloBox>& out)
{
    if (bgr.empty()) { out.clear(); return; }

    //BeeLog::Timer _t("Detect");

    float scale = 1.f;
    int   padw = 0, padh = 0;

    // 1. Letterbox -> paddedU8 (SxS, pad 114)
    Letterbox(bgr, paddedU8, scale, padw, padh);

    // 2. BGR -> RGB CHW float [0,1]
    BgrToCHWFloat01(paddedU8, inputBlob.data());

    // 3. Infer
    ov::Tensor inTensor(ov::element::f32,
        { 1, 3, (size_t)S, (size_t)S }, inputBlob.data());
    infer.set_input_tensor(inTensor);
    infer.infer();

    // 4. Output tensor + log shape
    ov::Tensor outTensor = infer.get_output_tensor();
   /* {
        auto shp = outTensor.get_shape();
        std::string s = "Output shape=[";
        for (size_t i = 0; i < shp.size(); i++)
        {
            s += std::to_string(shp[i]);
            if (i + 1 < shp.size()) s += ",";
        }
        s += "]";
        BeeLog::Write(BeeLog::Level::Info, s);
    }*/

    // 5. Decode
    out.clear();
    DecodeAuto_Yolo_RTDETR(
        outTensor,
        conf, iou,
        scale, padw, padh,
        bgr.cols, bgr.rows,
        S, C,
        out);

  /*  BeeLog::Writef(BeeLog::Level::Info,
        "Detect: img=%dx%d  conf=%.2f  iou=%.2f  -> %zu boxes",
        bgr.cols, bgr.rows, conf, iou, out.size());*/

    (void)Is3;
}

// ─────────────────────────────────────────────────────────────
// NmsPerClass (member — goi vao NmsPerClass2)
// ─────────────────────────────────────────────────────────────
void OpenVinoYoloHP::NmsPerClass(std::vector<YoloBox>& cand, float iou,
    std::vector<YoloBox>& out)
{
    NmsPerClass2(cand, iou, out);
}

// ─────────────────────────────────────────────────────────────
// Legacy stubs — giu de linker khong bao loi unresolved
// ─────────────────────────────────────────────────────────────
void OpenVinoYoloHP::DecodeYolo(
    const ov::Tensor&, float, float, int, int, int, int,
    std::vector<YoloBox>&) {
}

void OpenVinoYoloHP::DecodeYoloAuto(
    const ov::Tensor&, float, float, int, int, int, int,
    std::vector<YoloBox>&) {
}

void OpenVinoYoloHP::DecodeDetectionOutput(
    const ov::Tensor&, float, float, int, int, int, int,
    std::vector<YoloBox>&) {
}

void OpenVinoYoloHP::DecodeAnyLayout(
    const ov::Tensor&, float, float, int, int, int, int,
    std::vector<YoloBox>&) {
}
