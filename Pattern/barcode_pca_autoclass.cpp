// barcode_pca_autoclass.cpp
#include "barcode_pca_autoclass.hpp"
#include <algorithm>
#include <numeric>
#include <cmath>
#include <iomanip>
#include <sstream>

#include <opencv2/core/utils/filesystem.hpp> // createDirectories
using namespace cv;
using std::vector;
// ==== Helpers & utilities (put these at TOP of barcode_pca_autoclass.cpp) ====
#include <algorithm>
#include <numeric>
#include <cmath>

static inline int    iClamp(int v, int lo, int hi) { return std::max(lo, std::min(hi, v)); }
static inline double dClamp(double v, double lo, double hi) { return std::max(lo, std::min(hi, v)); }

static inline Mat toGray(const Mat& img) {
    if (img.channels() == 1) return img;
    Mat g; cvtColor(img, g, COLOR_BGR2GRAY); return g;
}

static inline Mat gradMag(const Mat& gray, int sobelKsize) {
    Mat gx, gy;
    if (sobelKsize < 0) { Scharr(gray, gx, CV_32F, 1, 0); Scharr(gray, gy, CV_32F, 0, 1); }
    else { Sobel(gray, gx, CV_32F, 1, 0, sobelKsize); Sobel(gray, gy, CV_32F, 0, 1, sobelKsize); }
    Mat mag; magnitude(gx, gy, mag); return mag;
}

static inline void estimateNoiseMAD(const Mat& gray, double& madGxGy) {
    Mat gx, gy; Scharr(gray, gx, CV_32F, 1, 0); Scharr(gray, gy, CV_32F, 0, 1);
    Mat mag; magnitude(gx, gy, mag);
    vector<float> v; v.reserve((size_t)gray.total() / 4);
    const int step = std::max(1, (int)(mag.total() / 300000));
    for (int r = 0; r < mag.rows; ++r) {
        const float* p = mag.ptr<float>(r);
        for (int c = 0; c < mag.cols; c += step) v.push_back(p[c]);
    }
    if (v.empty()) { madGxGy = 0.0; return; }
    auto mid = v.begin() + v.size() / 2;
    std::nth_element(v.begin(), mid, v.end());
    float median = *mid;
    for (auto& x : v) x = std::fabs(x - median);
    std::nth_element(v.begin(), mid, v.end());
    madGxGy = *mid;
}

static inline float percentile(const Mat& m32f, float p) {
    CV_Assert(m32f.type() == CV_32F);
    vector<float> v; v.reserve((size_t)m32f.total() / 2);
    const int step = std::max(1, (int)(m32f.total() / 400000));
    for (int r = 0; r < m32f.rows; ++r) {
        const float* row = m32f.ptr<float>(r);
        for (int c = 0; c < m32f.cols; c += step) v.push_back(row[c]);
    }
    if (v.empty()) return 0.f;
    p = (float)dClamp(p, 0.0, 1.0);
    size_t k = (size_t)(p * (v.size() - 1));
    std::nth_element(v.begin(), v.begin() + k, v.end());
    return v[k];
}

static inline void autoTuneParams(const Mat& gray, BarcodeDetectParams& P) {
    const int maxSide = std::max(gray.cols, gray.rows);
    if (P.blurKsize == 0) P.blurKsize = (maxSide > 1800) ? 5 : 3;

    double mad = 0; estimateNoiseMAD(gray, mad);
    if (P.gradPercentile <= 0.f) {
        float base = 0.88f;
        float adj = (float)dClamp(0.06 * (mad / 5.0), 0.0, 0.06);
        P.gradPercentile = base - adj; // 0.82..0.88
    }
    if (P.morphCloseW == 0 || P.morphCloseH == 0) {
        int w = iClamp(maxSide / 64, 15, 45);
        int h = iClamp(maxSide / 360, 2, 5);
        P.morphCloseW = w; P.morphCloseH = h;
    }
}

// --------- classification features ---------
static inline double stripePeriodicityScore(const Mat& crop) {
    if (crop.empty()) return 0.0;
    Mat g = toGray(crop);

    bool wide = (g.cols >= g.rows);
    Mat proj;
    if (wide) reduce(g, proj, 1, REDUCE_AVG, CV_32F);
    else      reduce(g, proj, 0, REDUCE_AVG, CV_32F);
    Mat f; proj.convertTo(f, CV_32F);
    f = f.reshape(1, 1); // 1xN

    int N = f.cols;
    if (N < 32) return 0.0;
    Mat win(1, N, CV_32F);
    for (int i = 0; i < N; ++i) win.at<float>(0, i) = 0.54f - 0.46f * std::cos(2 * CV_PI * i / (N - 1));
    Mat x; multiply(f, win, x);
    Mat X; dft(x, X, DFT_COMPLEX_OUTPUT);

    vector<float> mag; mag.reserve(N / 2);
    for (int k = 2; k < N / 2; ++k) {
        Vec2f c = X.at<Vec2f>(0, k);
        mag.push_back(std::sqrt(c[0] * c[0] + c[1] * c[1]));
    }
    if (mag.empty()) return 0.0;
    float maxv = *std::max_element(mag.begin(), mag.end());
    float mean = std::accumulate(mag.begin(), mag.end(), 0.0f) / (float)mag.size();
    double peakness = (maxv > 1e-6f) ? (maxv / (mean + 1e-6f)) : 0.0;
    return dClamp((peakness - 1.0) / 4.0, 0.0, 1.0);
}

static inline double qrFinder1DScanScore(const Mat& crop) {
    if (crop.empty()) return 0.0;
    Mat g = toGray(crop);
    bool wide = (g.cols >= g.rows);
    double best = 0.0;
    for (int pass = 0; pass < 3; ++pass) {
        int idx = (pass == 0) ? (wide ? g.rows / 2 : g.cols / 2)
            : (pass == 1) ? (wide ? g.rows / 3 : g.cols / 3)
            : (wide ? (2 * g.rows) / 3 : (2 * g.cols) / 3);
        Mat line = wide ? g.row(idx) : g.col(idx).t();
        Mat s; line.convertTo(s, CV_32F);
        Scalar mean, stdv; meanStdDev(s, mean, stdv);
        float thr = (float)mean[0];
        Mat bin; threshold(s, bin, thr, 255, THRESH_BINARY_INV);
        const uchar* p = bin.ptr<uchar>(0);
        vector<int> runs; runs.reserve(bin.cols);
        int cur = p[0], len = 1;
        for (int i = 1; i < bin.cols; ++i) {
            if (p[i] == cur) ++len; else { runs.push_back(len); cur = p[i]; len = 1; }
        }
        runs.push_back(len);
        for (size_t i = 0; i + 4 < runs.size(); ++i) {
            float a = runs[i], b = runs[i + 1], c = runs[i + 2], d = runs[i + 3], e = runs[i + 4];
            float m = std::min(a, std::min(b, std::min(c, std::min(d, e))));
            if (m < 2) continue;
            float ra = a / m, rb = b / m, rc = c / m, rd = d / m, re = e / m;
            float err = std::fabs(ra - 1) + std::fabs(rb - 1) + std::fabs(rc - 3) + std::fabs(rd - 1) + std::fabs(re - 1);
            double sc = dClamp(1.0 - err / 6.0, 0.0, 1.0);
            best = std::max(best, sc);
        }
    }
    return best;
}

static inline double dataMatrixLScore(const Mat& crop) {
    if (crop.empty()) return 0.0;
    Mat g = toGray(crop);
    Mat bw; adaptiveThreshold(g, bw, 255, ADAPTIVE_THRESH_MEAN_C, THRESH_BINARY_INV, 15, 5);
    int tBlack = countNonZero(bw.row(0));
    int bBlack = countNonZero(bw.row(bw.rows - 1));
    int lBlack = countNonZero(bw.col(0));
    int rBlack = countNonZero(bw.col(bw.cols - 1));
    double densT = (double)tBlack / bw.cols, densB = (double)bBlack / bw.cols;
    double densL = (double)lBlack / bw.rows, densR = (double)rBlack / bw.rows;
    double s1 = (densT > 0.6 && densL > 0.6) ? (densT + densL) / 2.0 : 0.0;
    double s2 = (densB > 0.6 && densR > 0.6) ? (densB + densR) / 2.0 : 0.0;
    return dClamp(std::max(s1, s2), 0.0, 1.0);
}

static inline double pdf417Score(const RotatedRect& rr, const Mat& crop) {
    double w = rr.size.width, h = rr.size.height;
    double aspect = (w > h) ? (w / h) : (h / w);
    double peri = stripePeriodicityScore(crop);
    double s = 0.0;
    if (aspect > 4.0) s = 0.4 + 0.6 * peri;
    return dClamp(s, 0.0, 1.0);
}

static inline Mat cropRotated(const Mat& img, const RotatedRect& rr, double padScale) {
    RotatedRect r = rr; r.size.width *= padScale; r.size.height *= padScale;
    Mat M = getRotationMatrix2D(r.center, r.angle, 1.0);
    Mat rot; warpAffine(img, rot, M, img.size(), INTER_LINEAR, BORDER_REPLICATE);
    Size2f sz = r.size;
    Rect roi(cvRound(r.center.x - sz.width * 0.5f),
        cvRound(r.center.y - sz.height * 0.5f),
        cvRound(sz.width), cvRound(sz.height));
    roi &= Rect(0, 0, rot.cols, rot.rows);
    if (roi.width <= 0 || roi.height <= 0) return Mat();
    return rot(roi).clone();
}

// ==== (giữ nguyên các hàm util trước đó) ====
// iClamp, dClamp, toGray, estimateNoiseMAD, percentile, gradMag,
// autoTuneParams, anisotropyPCA, stripePeriodicityScore,
// qrFinder1DScanScore, dataMatrixLScore, pdf417Score, cropRotated
// (…không lặp lại ở đây cho gọn)

// --------- Helpers vẽ & lưu ---------
static inline Scalar colorFor(BarcodeKind k) {
    switch (k) {
    case BarcodeKind::Linear1D:  return Scalar(80, 220, 20);   // xanh lá
    case BarcodeKind::QR:        return Scalar(220, 160, 20);  // xanh dương nhẹ (BGR)
    case BarcodeKind::DataMatrix:return Scalar(200, 60, 200);  // tím
    case BarcodeKind::PDF417:    return Scalar(20, 180, 220);  // cam-ish
    default:                     return Scalar(30, 30, 240);   // đỏ
    }
}

static inline std::string kindName(BarcodeKind k) {
    switch (k) {
    case BarcodeKind::Linear1D: return "1D";
    case BarcodeKind::QR: return "QR";
    case BarcodeKind::DataMatrix: return "DataMatrix";
    case BarcodeKind::PDF417: return "PDF417";
    default: return "Unknown";
    }
}

static inline void drawRotatedRect(Mat& img, const RotatedRect& rr, const Scalar& col, int th) {
    Point2f pts[4]; rr.points(pts);
    for (int i = 0; i < 4; ++i) line(img, pts[i], pts[(i + 1) % 4], col, th, LINE_AA);
}

static inline void saveStage8U(const Mat& m, const std::string& path) {
    if (m.empty()) return;
    Mat out;
    if (m.type() == CV_8U) out = m;
    else if (m.type() == CV_32F || m.type() == CV_64F) {
        Mat n; normalize(m, n, 0, 255, NORM_MINMAX);
        n.convertTo(out, CV_8U);
    }
    else {
        m.convertTo(out, CV_8U);
    }
    imwrite(path, out);
}

static inline Mat scaled(const Mat& img, double s) {
    if (s == 1.0 || img.empty()) return img;
    Mat r; resize(img, r, Size(), s, s, INTER_AREA); return r;
}

// ---------- Core detect (private) ----------
static std::vector<BarcodeRegion>
_detectCore(const Mat& bgrOrGray, const BarcodeDetectParams& Puser,
    const DebugDrawOptions* Dopt) {
    CV_Assert(!bgrOrGray.empty());
    cv::Mat gray = toGray(bgrOrGray);

    BarcodeDetectParams P = Puser;
    autoTuneParams(gray, P);

    if (Dopt) { cv::utils::fs::createDirectories(Dopt->outDir); }

    if (P.blurKsize >= 3 && (P.blurKsize % 2 == 1))
        GaussianBlur(gray, gray, Size(P.blurKsize, P.blurKsize), 0);

    Mat mag = gradMag(gray, P.sobelKsize);
    float thrP = (P.gradPercentile > 0.f) ? P.gradPercentile : 0.86f;
    float t = percentile(mag, thrP);
    Mat bin; threshold(mag, bin, t, 255.0, THRESH_BINARY);
    bin.convertTo(bin, CV_8U);

    Mat ker = getStructuringElement(MORPH_RECT, Size(P.morphCloseW, P.morphCloseH));
    morphologyEx(bin, bin, MORPH_CLOSE, ker);
    morphologyEx(bin, bin, MORPH_OPEN, getStructuringElement(MORPH_RECT, Size(3, 3)));

    // ---- Lưu stages nếu cần ----
    if (Dopt && Dopt->saveStages) {
        saveStage8U(gray, Dopt->outDir + "/01_gray.png");
        saveStage8U(mag, Dopt->outDir + "/02_grad.png");
        saveStage8U(bin, Dopt->outDir + "/03_thresh.png");
        saveStage8U(bin, Dopt->outDir + "/04_morph.png"); // sau morph
    }

    vector<vector<Point>> contours;
    findContours(bin, contours, RETR_EXTERNAL, CHAIN_APPROX_SIMPLE);

    struct Cand { RotatedRect rr; double score; vector<Point> pts; };
    vector<Cand> cands; cands.reserve(contours.size());

    // ---- chọn ứng viên theo PCA/area/aspect/anisotropy ----
    for (auto& cnt : contours) {
        if ((int)cnt.size() < 20) continue;
        RotatedRect rr;
        // (anisotropyPCA là hàm đã có từ trước)
        double ani = [&] {
            Mat data((int)cnt.size(), 2, CV_32F);
            for (int i = 0; i < (int)cnt.size(); ++i) { data.at<float>(i, 0) = cnt[i].x; data.at<float>(i, 1) = cnt[i].y; }
            PCA pca(data, Mat(), PCA::DATA_AS_ROW);
            Mat ev = pca.eigenvectors, ew = pca.eigenvalues;
            Point2f c((float)pca.mean.at<double>(0, 0), (float)pca.mean.at<double>(0, 1));
            Point2f v0(ev.at<float>(0, 0), ev.at<float>(0, 1));
            float minU = FLT_MAX, maxU = -FLT_MAX, minV = FLT_MAX, maxV = -FLT_MAX;
            for (const auto& p : cnt) {
                Point2f d((float)p.x - c.x, (float)p.y - c.y);
                float u = d.x * v0.x + d.y * v0.y;
                float v = d.x * (-v0.y) + d.y * (v0.x);
                minU = std::min(minU, u); maxU = std::max(maxU, u);
                minV = std::min(minV, v); maxV = std::max(maxV, v);
            }
            float W = (maxU - minU), H = (maxV - minV);
            float ang = (float)std::atan2(v0.y, v0.x) * 180.f / (float)CV_PI;
            if (H > W) { std::swap(W, H); ang += 90.f; }
            rr = RotatedRect(c, Size2f(W, H), ang);
            double a = std::max(1e-6f, ew.at<float>(0));
            double b = std::max(1e-6f, ew.at<float>(1));
            return a / b;
            }();
        double area = rr.size.area();
        if (area < P.minArea) continue;
        double w = rr.size.width, h = rr.size.height;
        double aspect = (w > h) ? (w / h) : (h / w);
        if (aspect < P.minAspect && ani < P.minAnisotropy) continue;

        double score = ani * std::log(std::max(10.0, area));
        cands.push_back({ rr,score,cnt });
    }

    if (cands.empty()) {
        // vẫn lưu overlay rỗng nếu bật
        if (Dopt && Dopt->saveOverlay) {
            Mat empty = bgrOrGray.clone();
            putText(empty, "No candidates", Point(20, 40), FONT_HERSHEY_SIMPLEX, 1.0, Scalar(0, 0, 255), 2, LINE_AA);
            imwrite(Dopt->outDir + "/overlay_all.png", scaled(empty, Dopt->visScale));
        }
        return {};
    }

    std::sort(cands.begin(), cands.end(),
        [](const Cand& a, const Cand& b) { return a.score > b.score; });

    vector<BarcodeRegion> out; out.reserve(std::min((int)cands.size(), P.maxCandidates));

    Mat overlayAll = bgrOrGray.clone();

    for (int i = 0; i < (int)cands.size() && (int)out.size() < P.maxCandidates; ++i) {
        const auto& c = cands[i];
        Mat crop = [&] {
            RotatedRect r = c.rr; r.size.width *= P.padScale; r.size.height *= P.padScale;
            Mat M = getRotationMatrix2D(r.center, r.angle, 1.0);
            Mat rot; warpAffine(bgrOrGray, rot, M, bgrOrGray.size(), INTER_LINEAR, BORDER_REPLICATE);
            Size2f sz = r.size;
            Rect roi(cvRound(r.center.x - sz.width * 0.5f),
                cvRound(r.center.y - sz.height * 0.5f),
                cvRound(sz.width), cvRound(sz.height));
            roi &= Rect(0, 0, rot.cols, rot.rows);
            return (roi.width > 0 && roi.height > 0) ? rot(roi).clone() : Mat();
            }();
        if (crop.empty()) continue;

        // ==== tính điểm phân loại (dùng các hàm đã có) ====
        double w = c.rr.size.width, h = c.rr.size.height;
        double aspect = (w > h) ? (w / h) : (h / w);
        double squareness = 1.0 - std::fabs(w - h) / std::max(w, h);
        // tái dùng công thức ở phiên bản trước
        auto periodic = stripePeriodicityScore(crop);
        auto qrS = qrFinder1DScanScore(crop) * squareness;
        auto dmS = dataMatrixLScore(crop) * squareness;
        auto pdfS = pdf417Score(c.rr, crop);
        double aniGuess = std::max(1.0, c.score / std::log(std::max(10.0f, static_cast<float>(c.rr.size.area()))));
        double oneD = dClamp(0.5 * (dClamp((aniGuess - 2.0) / 6.0, 0, 1))
            + 0.4 * periodic
            + 0.1 * dClamp((aspect - 2.0) / 6.0, 0, 1), 0.0, 1.0);
        double best2D = std::max(qrS, std::max(dmS, pdfS));

        BarcodeKind kind = BarcodeKind::Unknown;
        double conf = 0.0;
        if (oneD > best2D && oneD > 0.35) { kind = BarcodeKind::Linear1D; conf = oneD; }
        else {
            if (qrS >= dmS && qrS >= pdfS && qrS > 0.35) { kind = BarcodeKind::QR; conf = qrS; }
            else if (dmS >= qrS && dmS >= pdfS && dmS > 0.35) { kind = BarcodeKind::DataMatrix; conf = dmS; }
            else if (pdfS > 0.35) { kind = BarcodeKind::PDF417; conf = pdfS; }
            else { kind = (oneD > best2D) ? BarcodeKind::Linear1D : BarcodeKind::Unknown; conf = std::max(oneD, best2D); }
        }

        // ---- push kết quả ----
        out.push_back({ c.rr, crop, kind, dClamp(conf,0,1), c.score });

        // ---- overlay từng ứng viên ----
        if (Dopt && Dopt->saveOverlay) {
            Mat ov = bgrOrGray.clone();
            Scalar col = colorFor(kind);
            drawRotatedRect(ov, c.rr, col, Dopt->thickness);
            std::ostringstream ss; ss << "#" << i << " " << kindName(kind)
                << " conf=" << std::fixed << std::setprecision(2) << conf
                << " ang=" << std::setprecision(1) << c.rr.angle;
            putText(ov, ss.str(), c.rr.center + Point2f(8, -8),
                FONT_HERSHEY_SIMPLEX, 0.7, col, 2, LINE_AA);
            imwrite(Dopt->outDir + "/overlay_#" + std::to_string(i) + ".png", scaled(ov, Dopt->visScale));
        }

        // ---- overlay cộng dồn ----
        if (Dopt && Dopt->saveOverlay) {
            Scalar col = colorFor(kind);
            drawRotatedRect(overlayAll, c.rr, col, Dopt->thickness);
            std::ostringstream ss; ss << "#" << i << " " << kindName(kind)
                << " (" << std::fixed << std::setprecision(2) << conf << ")";
            putText(overlayAll, ss.str(), c.rr.center + Point2f(8, 12),
                FONT_HERSHEY_SIMPLEX, 0.6, col, 2, LINE_AA);
        }

        // ---- lưu crop ----
        if (Dopt && Dopt->saveCrops) {
            std::ostringstream fn;
            fn << Dopt->outDir << "/crop_#" << i << "_" << kindName(kind)
                << "_conf" << std::fixed << std::setprecision(2) << conf << ".png";
            imwrite(fn.str(), crop);
        }

        if (!P.returnAll && out.size() == 1) break;
    }

    if (Dopt && Dopt->saveOverlay) {
        imwrite(Dopt->outDir + "/overlay_all.png", scaled(overlayAll, Dopt->visScale));
    }
    return out;
}

// ---------- Public APIs ----------
std::vector<BarcodeRegion>
detectAndClassifyBarcodes(const Mat& bgrOrGray, const BarcodeDetectParams& P) {
    DebugDrawOptions* noDbg = nullptr;
    return _detectCore(bgrOrGray, P, noDbg);
}

std::vector<BarcodeRegion>
detectAndClassifyBarcodes(const Mat& bgrOrGray,
    const BarcodeDetectParams& P,
    const DebugDrawOptions& D) {
    return _detectCore(bgrOrGray, P, &D);
}
