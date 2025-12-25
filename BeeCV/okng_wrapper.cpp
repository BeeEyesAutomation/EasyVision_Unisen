// Nếu KHÔNG dùng PCH, hãy xóa dòng sau:
#include "pch.h"

#define OKNG_BUILD 1
#include "okng_wrapper.h"

#include <opencv2/opencv.hpp>
#include <unordered_map>
#include <vector>
#include <mutex>
#include <algorithm>
#include <cmath>

#ifdef _OPENMP
#include <omp.h>
#endif

using namespace cv;
using std::unordered_map;
using std::vector;
using std::mutex;
using std::lock_guard;

//====== Structures ======

struct OKNGModel {
    int  id{};
    int  label{};   // +1=OK, -1=NG
    Size size;      // kích thước template trong "không gian làm việc"
    Mat  edges;     // EDGE mode
    Mat  tmplGray;  // INTENSITY & ORB mode (gray đã preprocess)
};

struct OKNGImpl {
    // Config chung
    int   canny1 = 10, canny2 = 220;
    float matchThr = 0.65f;   // lọc cứng (detect NG→OK)
    bool  useOMP = true;

    // Edge advanced
    int   edgeSpeckleMinArea = 0; // 0=tắt
    int   useMultiScale = 0; // 0/1, cho EDGE

    // Match mode: 0=edge, 1=intensity, 2=orb
    int   matchMode = 0;

    // INTENSITY multi-scale & rotation
    int   intenMS_Enable = 0;
    float intenMS_Min = 0.9f, intenMS_Max = 1.1f, intenMS_Step = 0.05f;

    int   intenRot_Enable = 0;
    float intenRot_MaxDeg = 0.f, intenRot_StepDeg = 0.f;

    // ORB params
    int   orb_nFeatures = 600;
    float orb_scaleFactor = 1.2f;
    int   orb_nLevels = 8;

    // Working resize (downscale) cho học & detect
    int   workResizeEnable = 0;
    float workScale = 1.0f;   // 0.25..1.0

    // Models
    int   nextId = 1000;
    unordered_map<int, OKNGModel> models;
    vector<int> addedOK;  // stack id theo thứ tự add
    vector<int> addedNG;

    // Concurrency
    mutex mtx;
};

//====== Version ======
OKNG_API int OKNG_GetVersion() { return 240; }

//====== Helpers ======

static Mat loadGray(const char* path) { return imread(path, IMREAD_GRAYSCALE); }

static Mat makeGrayFromMem(const uint8_t* data, int w, int h, int step, int ch) {
    if (!data || w <= 0 || h <= 0 || step <= 0) return Mat();
    int type = (ch == 1 ? CV_8UC1 : ch == 3 ? CV_8UC3 : ch == 4 ? CV_8UC4 : 0);
    if (!type) return Mat();
    Mat src(h, w, type, (void*)data, step);
    Mat gray;
    if (ch == 1) gray = src.clone();
    else if (ch == 3) cvtColor(src, gray, COLOR_BGR2GRAY);
    else { Mat bgr; cvtColor(src, bgr, COLOR_BGRA2BGR); cvtColor(bgr, gray, COLOR_BGR2GRAY); }
    return gray;
}

static Mat maybeDownscaleGray(const Mat& gray, int enable, float scale) {
    if (!enable || std::abs(scale - 1.f) < 1e-6f) return gray;
    Mat small; resize(gray, small, Size(), scale, scale, INTER_AREA);
    return small;
}

// --- EDGE preprocess ---
static void autoCannyThresholds(const Mat& srcGray, int& lowThr, int& highThr)
{
    Mat gx, gy, mag, mag8, tmp;
    Sobel(srcGray, gx, CV_32F, 1, 0, 3);
    Sobel(srcGray, gy, CV_32F, 0, 1, 3);
    magnitude(gx, gy, mag);
    normalize(mag, mag, 0, 255, NORM_MINMAX);
    mag.convertTo(mag8, CV_8U);
    double otsu = threshold(mag8, tmp, 0, 255, THRESH_BINARY | THRESH_OTSU);
    highThr = std::max(40, (int)std::lround(otsu));
    lowThr = std::max(10, (int)std::lround(highThr * 0.4));
}

static Mat buildEdgesAdv(const Mat& gray, int t1, int t2,
    int speckleMinArea, bool useMultiScale)
{
    CV_Assert(!gray.empty() && gray.type() == CV_8UC1);

    Mat work = gray.clone();
    Ptr<CLAHE> clahe = createCLAHE(2.0, Size(8, 8));
    clahe->apply(work, work);
    fastNlMeansDenoising(work, work, 3, 7, 21);
    Mat blur; GaussianBlur(work, blur, Size(0, 0), 1.0, 1.0);
    Mat sharp; addWeighted(work, 1.5, blur, -0.5, 0.0, sharp);

    int lowThr = t1, highThr = t2;
    if (t1 <= 0 || t2 <= 0) autoCannyThresholds(sharp, lowThr, highThr);

    Mat edges, acc;
    if (useMultiScale) {
        const double sigmas[] = { 0.8, 1.2, 1.6 };
        for (double s : sigmas) {
            Mat g, e;
            GaussianBlur(sharp, g, Size(0, 0), s, s);
            Canny(g, e, lowThr, highThr, 3, true);
            if (acc.empty()) acc = e; else bitwise_or(acc, e, acc);
        }
        edges = acc;
    }
    else {
        Mat g; GaussianBlur(sharp, g, Size(3, 3), 0, 0);
        Canny(g, edges, lowThr, highThr, 3, true);
    }

    Mat kernel = getStructuringElement(MORPH_RECT, Size(3, 3));
    morphologyEx(edges, edges, MORPH_CLOSE, kernel, Point(-1, -1), 1);

    if (speckleMinArea > 0) {
        Mat labels, stats, cents;
        int n = connectedComponentsWithStats(edges, labels, stats, cents, 8, CV_32S);
        for (int i = 1; i < n; ++i) {
            int area = stats.at<int>(i, CC_STAT_AREA);
            if (area < speckleMinArea) edges.setTo(0, labels == i);
        }
    }
    return edges;
}

// --- INTENSITY/ORB preprocess ---
static Mat preprocessGrayForMatch(const Mat& gray)
{
    CV_Assert(!gray.empty() && gray.type() == CV_8UC1);
    Mat work = gray.clone();
    Ptr<CLAHE> clahe = createCLAHE(2.0, Size(8, 8));
    clahe->apply(work, work);
    fastNlMeansDenoising(work, work, 3, 7, 21);
    Mat blur; GaussianBlur(work, blur, Size(0, 0), 1.0, 1.0);
    Mat sharp; addWeighted(work, 1.5, blur, -0.5, 0.0, sharp);
    return sharp;
}

// --- INTENSITY multi-scale + rotation ---
static float matchTemplate_Intensity_Multi(const Mat& sceneGrayProc,
    const Mat& templGrayProc,
    float minS, float maxS, float stepS,
    bool rotEnable, float maxDeg, float stepDeg,
    Point& outLoc, float& outScale, bool& outRot)
{
    CV_Assert(sceneGrayProc.type() == CV_8UC1 && templGrayProc.type() == CV_8UC1);
    float best = -1.f; outLoc = { -1,-1 }; outScale = 1.f; outRot = false;

    auto tryOne = [&](const Mat& sc, bool rotated, float s)->void {
        if (sc.cols < templGrayProc.cols || sc.rows < templGrayProc.rows) return;
        Mat res; matchTemplate(sc, templGrayProc, res, TM_CCOEFF_NORMED);
        double maxVal; Point maxLoc; minMaxLoc(res, nullptr, &maxVal, nullptr, &maxLoc);
        if ((float)maxVal > best) {
            best = (float)maxVal;
            if (rotated) { outLoc = { -1,-1 }; outRot = true; }
            else { outLoc = maxLoc; outRot = false; }
            outScale = s;
        }
        };

    for (float s = minS; s <= maxS + 1e-6f; s += stepS) {
        Mat scaled;
        if (std::abs(s - 1.0f) < 1e-6f) scaled = sceneGrayProc;
        else resize(sceneGrayProc, scaled, Size(), s, s, INTER_LINEAR);

        if (!rotEnable) {
            tryOne(scaled, false, s);
        }
        else {
            for (float a = -maxDeg; a <= maxDeg + 1e-6f; a += stepDeg) {
                if (std::abs(a) < 1e-6f) { tryOne(scaled, false, s); continue; }
                Mat rot, M = getRotationMatrix2D(Point2f(scaled.cols / 2.f, scaled.rows / 2.f), a, 1.0);
                warpAffine(scaled, rot, M, scaled.size(), INTER_LINEAR, BORDER_REPLICATE);
                tryOne(rot, true, s);
            }
        }
    }
    return best;
}

// --- ORB similarity ---
static float orbSimilarityScore(const Mat& sceneGrayProc, const Mat& templGrayProc,
    int nFeatures, float scaleFactor, int nLevels)
{
    Ptr<ORB> orb = ORB::create(nFeatures, scaleFactor, nLevels);
    vector<KeyPoint> k1, k2;
    Mat d1, d2;
    orb->detectAndCompute(sceneGrayProc, noArray(), k1, d1);
    orb->detectAndCompute(templGrayProc, noArray(), k2, d2);
    if (d1.empty() || d2.empty()) return 0.f;

    BFMatcher matcher(NORM_HAMMING);
    vector<vector<DMatch>> knn;
    matcher.knnMatch(d2, d1, knn, 2); // query=templ, train=scene

    vector<Point2f> ptsT, ptsS;
    for (auto& v : knn) {
        if (v.size() < 2) continue;
        if (v[0].distance < 0.75f * v[1].distance) {
            ptsT.push_back(k2[v[0].queryIdx].pt);
            ptsS.push_back(k1[v[0].trainIdx].pt);
        }
    }
    if (ptsT.size() < 8) return 0.f;

    vector<uchar> mask;
    Mat H = findHomography(ptsT, ptsS, RANSAC, 3.0, mask);
    if (H.empty()) return 0.f;

    int inl = 0; for (auto b : mask) if (b) ++inl;
    return (float)inl / (float)ptsT.size(); // 0..1
}

// --- Scene feature chọn theo mode (có working resize) ---
static Mat makeSceneFeature(OKNGImpl* p, const Mat& grayOrig)
{
    int mode, en; float s;
    { lock_guard<mutex> lk(p->mtx); mode = p->matchMode; en = p->workResizeEnable; s = p->workScale; }
    Mat gray = maybeDownscaleGray(grayOrig, en, s);
    if (mode == 1 || mode == 2) return preprocessGrayForMatch(gray); // INTENSITY/ORB
    return buildEdgesAdv(gray, p->canny1, p->canny2, p->edgeSpeckleMinArea, p->useMultiScale != 0); // EDGE
}

// --- match 1 mẫu (EDGE/INTENSITY; ORB xử lý riêng) ---
static bool matchSingle(const Mat& sceneFeat, const OKNGModel& m, int matchMode,
    Point& loc, float& score, const OKNGImpl* pCfg)
{
    if (matchMode == 2) return false; // ORB xử lý ngoài

    const Mat& templ = (matchMode == 1 ? m.tmplGray : m.edges);
    if (sceneFeat.empty() || templ.empty()) return false;

    if (matchMode == 1 && pCfg && pCfg->intenMS_Enable) {
        float scl = 1.f; bool rotated = false;
        score = matchTemplate_Intensity_Multi(
            sceneFeat, templ,
            pCfg->intenMS_Min, pCfg->intenMS_Max, pCfg->intenMS_Step,
            pCfg->intenRot_Enable != 0, pCfg->intenRot_MaxDeg, pCfg->intenRot_StepDeg,
            loc, scl, rotated);
        if (rotated) loc = { -1,-1 }; // không trả vị trí khi có xoay
        return score >= 0.f;
    }

    if (sceneFeat.cols < templ.cols || sceneFeat.rows < templ.rows) return false;
    Mat res; matchTemplate(sceneFeat, templ, res, TM_CCOEFF_NORMED);
    double maxVal; Point maxLoc; minMaxLoc(res, nullptr, &maxVal, nullptr, &maxLoc);
    score = (float)maxVal; loc = maxLoc;
    return true;
}

// --- map ngược toạ độ từ "không gian làm việc" về ảnh gốc ---
static void undoWorkingScaleIfNeeded(OKNGImpl* p, int* x, int* y, int* w, int* h)
{
    int en; float s;
    { lock_guard<mutex> lk(p->mtx); en = p->workResizeEnable; s = p->workScale; }
    if (!en || std::abs(s - 1.f) < 1e-6f) return;
    if (x && *x >= 0) *x = (int)std::lround(*x / s);
    if (y && *y >= 0) *y = (int)std::lround(*y / s);
    if (w && *w > 0)  *w = (int)std::lround(*w / s);
    if (h && *h > 0)  *h = (int)std::lround(*h / s);
}

// --- best trong nhóm (có threshold) ---
static bool detectBestInSet(OKNGImpl* p, const Mat& sceneFeat, const vector<int>& ids,
    int& bestId, Point& bestLoc, Size& bestSz, float& bestScr)
{
    bestId = -1; bestScr = -1.f; bestLoc = { -1,-1 };
    int mode; { lock_guard<mutex> lk(p->mtx); mode = p->matchMode; }

#ifdef _OPENMP
#pragma omp parallel for if(p->useOMP)
#endif
    for (int i = 0; i < (int)ids.size(); ++i) {
        OKNGModel m;
        {
            lock_guard<mutex> lk(p->mtx);
            auto it = p->models.find(ids[i]);
            if (it == p->models.end()) continue;
            m = it->second;
        }

        Point loc; float s = 0.f; bool ok = false;
        if (mode == 2) {
            s = orbSimilarityScore(sceneFeat, m.tmplGray, p->orb_nFeatures, p->orb_scaleFactor, p->orb_nLevels);
            ok = (s > 0.f); loc = { -1,-1 };
        }
        else {
            ok = matchSingle(sceneFeat, m, mode, loc, s, p);
        }
        if (!ok || s < p->matchThr) continue;

#ifdef _OPENMP
#pragma omp critical
#endif
        {
            if (s > bestScr) {
                bestScr = s; bestLoc = loc; bestId = m.id; bestSz = m.size;
            }
        }
    }
    return bestId != -1;
}

// --- best trong nhóm (không threshold) ---
static bool bestInSet_NoThr(OKNGImpl* p, const Mat& sceneFeat, const vector<int>& ids,
    int& bestId, Point& bestLoc, Size& bestSz, float& bestScr)
{
    bestId = -1; bestScr = -1.f; bestLoc = { -1,-1 };
    int mode; { lock_guard<mutex> lk(p->mtx); mode = p->matchMode; }

#ifdef _OPENMP
#pragma omp parallel for if(p->useOMP)
#endif
    for (int i = 0; i < (int)ids.size(); ++i) {
        OKNGModel m;
        {
            lock_guard<mutex> lk(p->mtx);
            auto it = p->models.find(ids[i]);
            if (it == p->models.end()) continue;
            m = it->second;
        }

        Point loc; float s = 0.f; bool ok = false;
        if (mode == 2) {
            s = orbSimilarityScore(sceneFeat, m.tmplGray, p->orb_nFeatures, p->orb_scaleFactor, p->orb_nLevels);
            ok = (s > 0.f); loc = { -1,-1 };
        }
        else {
            ok = matchSingle(sceneFeat, m, mode, loc, s, p);
        }
        if (!ok) continue;

#ifdef _OPENMP
#pragma omp critical
#endif
        { if (s > bestScr) { bestScr = s; bestLoc = loc; bestId = m.id; bestSz = m.size; } }
    }
    return bestId != -1;
}

static int detectPriorityCommon(OKNGImpl* p, const Mat& grayOrig,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH)
{
    if (grayOrig.empty()) return 0;

    Mat sceneFeat = makeSceneFeature(p, grayOrig);

    vector<int> idsNG, idsOK;
    {
        lock_guard<mutex> lk(p->mtx);
        for (auto& kv : p->models)
            (kv.second.label < 0 ? idsNG : idsOK).push_back(kv.first);
    }

    int id; Point loc; Size sz; float scr;

    // 1) Ưu tiên NG
    if (!idsNG.empty() && detectBestInSet(p, sceneFeat, idsNG, id, loc, sz, scr)) {
        if (outLabel)   *outLabel = -1;
        if (outScore)   *outScore = scr;
        if (outModelId) *outModelId = id;
        if (outX) *outX = loc.x; if (outY) *outY = loc.y;
        if (outW) *outW = sz.width; if (outH) *outH = sz.height;
        undoWorkingScaleIfNeeded(p, outX, outY, outW, outH);
        return 1;
    }
    // 2) Thử OK
    if (!idsOK.empty() && detectBestInSet(p, sceneFeat, idsOK, id, loc, sz, scr)) {
        if (outLabel)   *outLabel = +1;
        if (outScore)   *outScore = scr;
        if (outModelId) *outModelId = id;
        if (outX) *outX = loc.x; if (outY) *outY = loc.y;
        if (outW) *outW = sz.width; if (outH) *outH = sz.height;
        undoWorkingScaleIfNeeded(p, outX, outY, outW, outH);
        return 1;
    }

    // 3) Không có mẫu đạt ngưỡng
    if (outLabel) *outLabel = -1;
    if (outScore) *outScore = 0.f;
    if (outModelId) *outModelId = -1;
    if (outX) *outX = -1; if (outY) *outY = -1; if (outW) *outW = 0; if (outH) *outH = 0;
    return 1;
}

//====== API impl ======

// Lifecycle / Params
OKNG_API OKNGHandle OKNG_Create() { return new OKNGImpl(); }
OKNG_API void       OKNG_Destroy(OKNGHandle h) { if (h) delete (OKNGImpl*)h; }

OKNG_API void OKNG_SetCanny(OKNGHandle h, int t1, int t2) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); p->canny1 = t1; p->canny2 = t2;
}
OKNG_API void OKNG_SetMatchThreshold(OKNGHandle h, float thr) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); p->matchThr = thr;
}
OKNG_API void OKNG_SetUseOMP(OKNGHandle h, int flag) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); p->useOMP = (flag != 0);
}
OKNG_API void OKNG_SetEdgeSpeckleMinArea(OKNGHandle h, int pixels) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); p->edgeSpeckleMinArea = std::max(0, pixels);
}
OKNG_API void OKNG_EnableMultiScaleCanny(OKNGHandle h, int enable) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); p->useMultiScale = (enable != 0) ? 1 : 0;
}
OKNG_API void OKNG_SetMatchMode(OKNGHandle h, int mode) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); p->matchMode = (mode == 1 ? 1 : mode == 2 ? 2 : 0);
}
OKNG_API void OKNG_SetIntensityMultiScale(OKNGHandle h, int enable, float minS, float maxS, float stepS) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx);
    p->intenMS_Enable = enable ? 1 : 0;
    p->intenMS_Min = std::max(0.2f, minS);
    p->intenMS_Max = std::max(p->intenMS_Min, maxS);
    p->intenMS_Step = std::max(0.01f, stepS);
}
OKNG_API void OKNG_SetIntensityRotSearch(OKNGHandle h, int enable, float maxDeg, float stepDeg) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx);
    p->intenRot_Enable = enable ? 1 : 0;
    p->intenRot_MaxDeg = std::max(0.f, maxDeg);
    p->intenRot_StepDeg = std::max(1.f, stepDeg);
}
OKNG_API void OKNG_SetORBParams(OKNGHandle h, int nFeatures, float scaleFactor, int nLevels) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx);
    p->orb_nFeatures = std::max(200, nFeatures);
    p->orb_scaleFactor = std::max(1.05f, scaleFactor);
    p->orb_nLevels = std::max(3, nLevels);
}

// Working resize
OKNG_API void OKNG_SetWorkingResize(OKNGHandle h, int enable, float scale) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx);
    p->workResizeEnable = enable ? 1 : 0;
    if (scale <= 0.f) scale = 1.f;
    if (scale > 1.f)  scale = 1.f;
    if (scale < 0.25f) scale = 0.25f;
    p->workScale = scale;
}
OKNG_API void OKNG_GetWorkingResize(OKNGHandle h, int* outEnable, float* outScale) {
    if (!h) return; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx);
    if (outEnable) *outEnable = p->workResizeEnable;
    if (outScale)  *outScale = p->workScale;
}

// OpenMP control & info
OKNG_API void OKNG_SetOMPThreadCount(OKNGHandle h, int threads) {
    (void)h;
#ifdef _OPENMP
    if (threads > 0) omp_set_num_threads(threads);
    else             omp_set_num_threads(omp_get_max_threads());
#else
    (void)threads;
#endif
}
OKNG_API void OKNG_SetOMPDynamic(OKNGHandle h, int enable) {
    (void)h;
#ifdef _OPENMP
    omp_set_dynamic(enable ? 1 : 0); // 0: cố định số luồng
#else
    (void)enable;
#endif
}
OKNG_API void OKNG_GetOMPInfo(OKNGHandle h, int* outEnabled, int* outMaxThreads, int* outNumProcs) {
    (void)h;
#ifdef _OPENMP
    if (outEnabled)   *outEnabled = 1;
    if (outMaxThreads)*outMaxThreads = omp_get_max_threads();
    if (outNumProcs)  *outNumProcs = omp_get_num_procs();
#else
    if (outEnabled)   *outEnabled = 0;
    if (outMaxThreads)*outMaxThreads = 1;
    if (outNumProcs)  *outNumProcs = 1;
#endif
}

// Learn (áp dụng working resize khi học)
static int learnCommon(OKNGImpl* p, const Mat& grayIn, int label) {
    if (grayIn.empty()) return -1;

    Mat graySmall;
    {
        lock_guard<mutex> lk(p->mtx);
        graySmall = maybeDownscaleGray(grayIn, p->workResizeEnable, p->workScale);
    }

    Mat edges = buildEdgesAdv(graySmall, p->canny1, p->canny2, p->edgeSpeckleMinArea, p->useMultiScale != 0);
    Mat tproc = preprocessGrayForMatch(graySmall);

    OKNGModel m;
    m.id = p->nextId++;
    m.label = (label >= 0 ? +1 : -1);
    m.size = graySmall.size();   // kích thước trong không gian làm việc
    m.edges = std::move(edges);
    m.tmplGray = std::move(tproc);

    {
        lock_guard<mutex> lk(p->mtx);
        p->models[m.id] = std::move(m);
        if (label >= 0) p->addedOK.push_back(p->nextId - 1);
        else          p->addedNG.push_back(p->nextId - 1);
    }
    return p->nextId - 1;
}

OKNG_API int OKNG_LearnAutoFromFile(OKNGHandle h, const char* imagePath, int label) {
    if (!h) return -1; auto* p = (OKNGImpl*)h; Mat g = loadGray(imagePath); return learnCommon(p, g, label);
}
OKNG_API int OKNG_LearnAutoFromMemory(OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels, int label)
{
    if (!h) return -1; auto* p = (OKNGImpl*)h;
    Mat g = makeGrayFromMem(data, width, height, step, channels);
    return learnCommon(p, g, label);
}

// Save/Load
OKNG_API int OKNG_SaveModels(OKNGHandle h, const char* yamlPath) {
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    FileStorage fs(yamlPath, FileStorage::WRITE); if (!fs.isOpened()) return 0;
    lock_guard<mutex> lk(p->mtx);
    fs << "nextId" << p->nextId;
    fs << "models" << "[";
    for (auto& kv : p->models) {
        const OKNGModel& m = kv.second;
        fs << "{" << "id" << m.id
            << "label" << m.label
            << "w" << m.size.width
            << "h" << m.size.height
            << "edges" << m.edges
            << "tmplGray" << m.tmplGray
            << "}";
    }
    fs << "]";
    return 1;
}
OKNG_API int OKNG_LoadModels(OKNGHandle h, const char* yamlPath) {
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    FileStorage fs(yamlPath, FileStorage::READ); if (!fs.isOpened()) return 0;
    int nextId = 1000; fs["nextId"] >> nextId;
    vector<OKNGModel> loaded; FileNode n = fs["models"];
    for (auto it : n) {
        OKNGModel m; int w, hg;
        it["id"] >> m.id; it["label"] >> m.label; it["w"] >> w; it["h"] >> hg; m.size = Size(w, hg);
        it["edges"] >> m.edges;
        if (!it["tmplGray"].empty()) it["tmplGray"] >> m.tmplGray;
        loaded.push_back(std::move(m));
    }
    lock_guard<mutex> lk(p->mtx);
    p->models.clear();
    p->addedOK.clear(); p->addedNG.clear();
    for (auto& m : loaded) {
        p->models[m.id] = std::move(m);
        if (p->models[m.id].label >= 0) p->addedOK.push_back(p->models[m.id].id);
        else                             p->addedNG.push_back(p->models[m.id].id);
    }
    p->nextId = nextId;
    return 1;
}

// Remove APIs
static int removeOne(OKNGImpl* p, int modelId) {
    auto it = p->models.find(modelId);
    if (it == p->models.end()) return 0;
    p->models.erase(it);
    return 1;
}
static int popAndRemoveLast(vector<int>& stack, OKNGImpl* p) {
    while (!stack.empty()) {
        int id = stack.back(); stack.pop_back();
        if (removeOne(p, id)) return id;
    }
    return -1;
}
OKNG_API int OKNG_RemoveModel(OKNGHandle h, int modelId) {
    if (!h) return 0; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); return removeOne(p, modelId);
}
OKNG_API int OKNG_RemoveLastOKModel(OKNGHandle h) {
    if (!h) return 0; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); return popAndRemoveLast(p->addedOK, p) != -1;
}
OKNG_API int OKNG_RemoveLastNGModel(OKNGHandle h) {
    if (!h) return 0; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx); return popAndRemoveLast(p->addedNG, p) != -1;
}
OKNG_API int OKNG_RemoveAllByLabel(OKNGHandle h, int label) {
    if (!h) return 0; auto* p = (OKNGImpl*)h; lock_guard<mutex> lk(p->mtx);
    int cnt = 0;
    for (auto it = p->models.begin(); it != p->models.end(); ) {
        if ((label >= 0 && it->second.label >= 0) || (label < 0 && it->second.label < 0)) {
            it = p->models.erase(it); ++cnt;
        }
        else ++it;
    }
    if (label >= 0) p->addedOK.clear(); else p->addedNG.clear();
    return cnt;
}

// Detect (ưu tiên NG → OK; threshold)
OKNG_API int OKNG_DetectPriorityFromFile(OKNGHandle h, const char* imagePath,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH)
{
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    Mat gray = loadGray(imagePath); if (gray.empty()) return 0;
    return detectPriorityCommon(p, gray, outLabel, outScore, outModelId, outX, outY, outW, outH);
}
OKNG_API int OKNG_DetectPriorityFromMemory(OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH)
{
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    Mat gray = makeGrayFromMem(data, width, height, step, channels); if (gray.empty()) return 0;
    return detectPriorityCommon(p, gray, outLabel, outScore, outModelId, outX, outY, outW, outH);
}

// Nearest / Similarity (no threshold)
static void scaleBackOutRect(OKNGImpl* p, int* outX, int* outY, int* outW, int* outH) {
    undoWorkingScaleIfNeeded(p, outX, outY, outW, outH);
}

OKNG_API int OKNG_ClosestAnyFromFile(OKNGHandle h, const char* imagePath,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH)
{
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    Mat gray = loadGray(imagePath); if (gray.empty()) return 0;
    Mat sceneFeat = makeSceneFeature(p, gray);

    vector<int> allIds; allIds.reserve(p->models.size());
    { lock_guard<mutex> lk(p->mtx); for (auto& kv : p->models) allIds.push_back(kv.first); }

    int id; Point loc; Size sz; float scr;
    if (!allIds.empty() && bestInSet_NoThr(p, sceneFeat, allIds, id, loc, sz, scr)) {
        int label; { lock_guard<mutex> lk(p->mtx); label = p->models.at(id).label; }
        if (outLabel)   *outLabel = label;
        if (outScore)   *outScore = scr;
        if (outModelId) *outModelId = id;
        if (outX) *outX = loc.x; if (outY) *outY = loc.y; if (outW) *outW = sz.width; if (outH) *outH = sz.height;
        scaleBackOutRect(p, outX, outY, outW, outH);
        return 1;
    }
    return 0;
}
OKNG_API int OKNG_ClosestAnyFromMemory(OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH)
{
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    Mat gray = makeGrayFromMem(data, width, height, step, channels); if (gray.empty()) return 0;
    Mat sceneFeat = makeSceneFeature(p, gray);

    vector<int> allIds; allIds.reserve(p->models.size());
    { lock_guard<mutex> lk(p->mtx); for (auto& kv : p->models) allIds.push_back(kv.first); }

    int id; Point loc; Size sz; float scr;
    if (!allIds.empty() && bestInSet_NoThr(p, sceneFeat, allIds, id, loc, sz, scr)) {
        int label; { lock_guard<mutex> lk(p->mtx); label = p->models.at(id).label; }
        if (outLabel)   *outLabel = label;
        if (outScore)   *outScore = scr;
        if (outModelId) *outModelId = id;
        if (outX) *outX = loc.x; if (outY) *outY = loc.y; if (outW) *outW = sz.width; if (outH) *outH = sz.height;
        scaleBackOutRect(p, outX, outY, outW, outH);
        return 1;
    }
    return 0;
}

OKNG_API int OKNG_BestPerLabelFromFile(OKNGHandle h, const char* imagePath,
    int* outBestOKId, float* outBestOKScore, int* outBestNGId, float* outBestNGScore)
{
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    Mat gray = loadGray(imagePath); if (gray.empty()) return 0;
    Mat sceneFeat = makeSceneFeature(p, gray);

    vector<int> idsOK, idsNG;
    {
        lock_guard<mutex> lk(p->mtx);
        for (auto& kv : p->models) (kv.second.label < 0 ? idsNG : idsOK).push_back(kv.first);
    }

    int id; Point loc; Size sz; float scr;
    if (!idsOK.empty() && bestInSet_NoThr(p, sceneFeat, idsOK, id, loc, sz, scr)) {
        if (outBestOKId)    *outBestOKId = id;
        if (outBestOKScore) *outBestOKScore = scr;
    }
    else { if (outBestOKId) *outBestOKId = -1; if (outBestOKScore) *outBestOKScore = 0.f; }

    if (!idsNG.empty() && bestInSet_NoThr(p, sceneFeat, idsNG, id, loc, sz, scr)) {
        if (outBestNGId)    *outBestNGId = id;
        if (outBestNGScore) *outBestNGScore = scr;
    }
    else { if (outBestNGId) *outBestNGId = -1; if (outBestNGScore) *outBestNGScore = 0.f; }

    return 1;
}
OKNG_API int OKNG_BestPerLabelFromMemory(OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels,
    int* outBestOKId, float* outBestOKScore, int* outBestNGId, float* outBestNGScore)
{
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    Mat gray = makeGrayFromMem(data, width, height, step, channels); if (gray.empty()) return 0;
    Mat sceneFeat = makeSceneFeature(p, gray);

    vector<int> idsOK, idsNG;
    {
        lock_guard<mutex> lk(p->mtx);
        for (auto& kv : p->models) (kv.second.label < 0 ? idsNG : idsOK).push_back(kv.first);
    }

    int id; Point loc; Size sz; float scr;
    if (!idsOK.empty() && bestInSet_NoThr(p, sceneFeat, idsOK, id, loc, sz, scr)) {
        if (outBestOKId)    *outBestOKId = id;
        if (outBestOKScore) *outBestOKScore = scr;
    }
    else { if (outBestOKId) *outBestOKId = -1; if (outBestOKScore) *outBestOKScore = 0.f; }

    if (!idsNG.empty() && bestInSet_NoThr(p, sceneFeat, idsNG, id, loc, sz, scr)) {
        if (outBestNGId)    *outBestNGId = id;
        if (outBestNGScore) *outBestNGScore = scr;
    }
    else { if (outBestNGId) *outBestNGId = -1; if (outBestNGScore) *outBestNGScore = 0.f; }

    return 1;
}

// -------- Profile Detect (sửa reduction) --------
static int profileDetectCommon(OKNGImpl* p, const Mat& grayOrig,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH,
    double* outMillis, int* outThreadsUsed,
    int* outTriedNG, int* outPassedNG, int* outTriedOK, int* outPassedOK)
{
    if (grayOrig.empty()) return 0;
    auto t0 = getTickCount();

    Mat sceneFeat = makeSceneFeature(p, grayOrig);

    vector<int> idsNG, idsOK;
    {
        lock_guard<mutex> lk(p->mtx);
        for (auto& kv : p->models)
            (kv.second.label < 0 ? idsNG : idsOK).push_back(kv.first);
    }

    int triedNG = 0, passNG = 0, triedOK = 0, passOK = 0;
    int threadsUsed = 1;

    auto bestWithStats = [&](const vector<int>& ids,
        int& bestId, Point& bestLoc, Size& bestSz, float& bestScr,
        int* pTried, int* pPassed)->bool
        {
            bestId = -1; bestScr = -1.f; bestLoc = { -1,-1 };
            int mode; { lock_guard<mutex> lk(p->mtx); mode = p->matchMode; }

#ifdef _OPENMP
            int localThreads = 1;
#pragma omp parallel
            {
#pragma omp single
                { localThreads = omp_get_num_threads(); }
            }
            threadsUsed = std::max(threadsUsed, localThreads);
#endif

            int redTried = 0;
            int redPassed = 0;

#ifdef _OPENMP
#pragma omp parallel for if(p->useOMP) reduction(+:redTried,redPassed)
#endif
            for (int i = 0; i < (int)ids.size(); ++i) {
                OKNGModel m;
                {
                    lock_guard<mutex> lk(p->mtx);
                    auto it = p->models.find(ids[i]); if (it == p->models.end()) continue; m = it->second;
                }
                Point loc; float s = 0.f; bool ok = false;
                if (mode == 2) {
                    s = orbSimilarityScore(sceneFeat, m.tmplGray, p->orb_nFeatures, p->orb_scaleFactor, p->orb_nLevels);
                    ok = (s > 0.f); loc = { -1,-1 };
                }
                else {
                    ok = matchSingle(sceneFeat, m, mode, loc, s, p);
                }
                if (!ok) continue;

                redTried++;
                if (s >= p->matchThr) {
                    redPassed++;
#ifdef _OPENMP
#pragma omp critical
#endif
                    { if (s > bestScr) { bestScr = s; bestLoc = loc; bestId = m.id; bestSz = m.size; } }
                }
            }

            if (pTried)  *pTried += redTried;
            if (pPassed) *pPassed += redPassed;

            return bestId != -1;
        };

    int id; Point loc; Size sz; float scr;

    if (!idsNG.empty() && bestWithStats(idsNG, id, loc, sz, scr, &triedNG, &passNG)) {
        if (outLabel) *outLabel = -1; if (outScore) *outScore = scr; if (outModelId) *outModelId = id;
        if (outX) *outX = loc.x; if (outY) *outY = loc.y; if (outW) *outW = sz.width; if (outH) *outH = sz.height;
        undoWorkingScaleIfNeeded(p, outX, outY, outW, outH);
    }
    else if (!idsOK.empty() && bestWithStats(idsOK, id, loc, sz, scr, &triedOK, &passOK)) {
        if (outLabel) *outLabel = +1; if (outScore) *outScore = scr; if (outModelId) *outModelId = id;
        if (outX) *outX = loc.x; if (outY) *outY = loc.y; if (outW) *outW = sz.width; if (outH) *outH = sz.height;
        undoWorkingScaleIfNeeded(p, outX, outY, outW, outH);
    }
    else {
        if (outLabel) *outLabel = -1; if (outScore) *outScore = 0.f; if (outModelId) *outModelId = -1;
        if (outX) *outX = -1; if (outY) *outY = -1; if (outW) *outW = 0; if (outH) *outH = 0;
    }

    auto t1 = getTickCount();
    double ms = (t1 - t0) * 1000.0 / getTickFrequency();

    if (outMillis)      *outMillis = ms;
    if (outThreadsUsed) *outThreadsUsed = threadsUsed;
    if (outTriedNG)     *outTriedNG = triedNG;
    if (outPassedNG)    *outPassedNG = passNG;
    if (outTriedOK)     *outTriedOK = triedOK;
    if (outPassedOK)    *outPassedOK = passOK;

    return 1;
}

OKNG_API int OKNG_ProfileDetectFromMemory(
    OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH,
    double* outMillis, int* outThreadsUsed,
    int* outTriedNG, int* outPassedNG, int* outTriedOK, int* outPassedOK)
{
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    Mat gray = makeGrayFromMem(data, width, height, step, channels); if (gray.empty()) return 0;
    return profileDetectCommon(p, gray,
        outLabel, outScore, outModelId, outX, outY, outW, outH,
        outMillis, outThreadsUsed, outTriedNG, outPassedNG, outTriedOK, outPassedOK);
}

OKNG_API int OKNG_ProfileDetectFromFile(
    OKNGHandle h, const char* imagePath,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH,
    double* outMillis, int* outThreadsUsed,
    int* outTriedNG, int* outPassedNG, int* outTriedOK, int* outPassedOK)
{
    if (!h) return 0; auto* p = (OKNGImpl*)h;
    Mat gray = loadGray(imagePath); if (gray.empty()) return 0;
    return profileDetectCommon(p, gray,
        outLabel, outScore, outModelId, outX, outY, outW, outH,
        outMillis, outThreadsUsed, outTriedNG, outPassedNG, outTriedOK, outPassedOK);
}
