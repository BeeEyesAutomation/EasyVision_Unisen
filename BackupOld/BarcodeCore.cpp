#include "BarcodeCore.h"

// OpenCV
#include <opencv2/imgproc.hpp>
#include <opencv2/highgui.hpp>
#if __has_include(<opencv2/barcode.hpp>)
#include <opencv2/barcode.hpp>
#define HAS_OPENCV_BARCODE 1
#else
#define HAS_OPENCV_BARCODE 0
#endif
#include <ZXing/ReadBarcode.h>
#include <ZXing/ImageView.h>
#include <ZXing/DecodeHints.h>
#include <ZXing/BarcodeFormat.h>

#include <algorithm>
#include <numeric>
#include <cmath>

#if __has_include(<filesystem>)
#include <filesystem>
namespace fs = std::filesystem;
#endif

namespace BeeCpp {

    // ---------- small utils ----------
    static inline int clampi(int v, int lo, int hi) { return std::max(lo, std::min(hi, v)); }
    static inline int evenUp(int x) { return (x % 2) == 0 ? x + 1 : x; }

    void DebugViz::Save(const Debugger& D, const cv::Mat& img, const std::string& tag)
    {
        if (!D.save) return;
        cv::Mat out = img;
        if (img.channels() == 1) cv::cvtColor(img, out, cv::COLOR_GRAY2BGR);
        cv::imwrite(D.nextName(tag), out);
    }
    void DebugViz::SaveMask(const Debugger& D, const cv::Mat& mask, const std::string& tag)
    {
        if (!D.save) return;
        cv::Mat m8, color;
        if (mask.type() != CV_8U) mask.convertTo(m8, CV_8U);
        else m8 = mask;
        cv::applyColorMap(m8, color, cv::COLORMAP_MAGMA);
        cv::imwrite(D.nextName(tag), color);
    }
    void DebugViz::DrawRRects(cv::Mat& bgr, const std::vector<cv::RotatedRect>& rrs,
        const cv::Scalar& col, int thick)
    {
        for (auto& rr : rrs) {
            cv::Point2f v[4]; rr.points(v);
            for (int k = 0; k < 4; k++) cv::line(bgr, v[k], v[(k + 1) & 3], col, thick, cv::LINE_AA);
        }
    }
    void DebugViz::PutText(cv::Mat& bgr, const std::string& s, cv::Point org,
        double scale, const cv::Scalar& col, int thick)
    {
        cv::putText(bgr, s, org, cv::FONT_HERSHEY_SIMPLEX, scale, col, thick, cv::LINE_AA);
    }

    // ---------- mapping & geometry ----------
    void BarcodeCore::OrderQuadTLTRBRBL(std::vector<cv::Point2f>& q)
    {
        if (q.size() < 4) return;
        std::sort(q.begin(), q.end(), [](auto& a, auto& b) { return (a.y == b.y) ? a.x < b.x : a.y < b.y; });
        cv::Point2f tl = q[0].x < q[1].x ? q[0] : q[1];
        cv::Point2f tr = q[0].x < q[1].x ? q[1] : q[0];
        cv::Point2f bl = q[2].x < q[3].x ? q[2] : q[3];
        cv::Point2f br = q[2].x < q[3].x ? q[3] : q[2];
        q = { tl,tr,br,bl };
    }
    Symbology BarcodeCore::MapFormat(int f)
    {
        using ZXing::BarcodeFormat;
        switch (static_cast<BarcodeFormat>(f)) {
        case BarcodeFormat::EAN8:        return Symbology::EAN8;
        case BarcodeFormat::EAN13:       return Symbology::EAN13;
        case BarcodeFormat::UPCA:        return Symbology::UPC_A;
        case BarcodeFormat::UPCE:        return Symbology::UPC_E;
        case BarcodeFormat::Code39:     return Symbology::Code39;
        case BarcodeFormat::Code93:     return Symbology::Code93;
        case BarcodeFormat::Code128:    return Symbology::Code128;
        case BarcodeFormat::ITF:         return Symbology::ITF;
        case BarcodeFormat::QRCode:     return Symbology::QR;
        case BarcodeFormat::DataMatrix: return Symbology::DataMatrix;
        case BarcodeFormat::PDF417:      return Symbology::PDF417;
        case BarcodeFormat::Aztec:       return Symbology::Aztec;
        case BarcodeFormat::MaxiCode:    return Symbology::MaxiCode;
        case BarcodeFormat::Codabar:     return Symbology::Codabar;
        case BarcodeFormat::MicroQRCode:    return Symbology::MicroQR;

        default:                         return Symbology::Unknown;
        }
    }

    // ---------- crop 1 box theo góc của chính nó ----------
    cv::RotatedRect BarcodeCore::NormalizeRR(cv::RotatedRect rr)
    {
        if (rr.size.width < rr.size.height) {
            std::swap(rr.size.width, rr.size.height);
            rr.angle -= 90.f;  // giữ width là cạnh dài
        }
        if (rr.angle <= -180.f) rr.angle += 180.f;
        if (rr.angle > 0.f)     rr.angle -= 180.f;
        return rr; // angle ∈ (-90,0]
    }
    cv::Mat BarcodeCore::CropRotatedBox(const cv::Mat& gray, cv::RotatedRect rr, float pad)
    {
        CV_Assert(!gray.empty());

        // 1) Ép width là cạnh dài; nếu không thì hoán đổi size và cộng 90°
        float W = rr.size.width, H = rr.size.height;
        if (W < H) {
            std::swap(W, H);
            rr.angle += 90.f;                  // minAreaRect angle ∈ (-90,0], +90° để quy ước width là cạnh dài
        }

        // 2) Tăng padding
        W *= (1.f + pad);
        H *= (1.f + pad);

        // 3) Ma trận xoay-quy vùng (ROI warp):
        //    getRotationMatrix2D dùng quy ước dương = CCW.
        //    Với công thức crop ROI này, dùng TRỰC TIẾP rr.angle (không đổi dấu).
        cv::Mat M = cv::getRotationMatrix2D(rr.center, (double)rr.angle, 1.0);

        // 4) Dịch để tâm rr nằm đúng giữa ảnh đầu ra kích thước (W,H)
        const double tx = (W - 1.0) * 0.5 - rr.center.x;
        const double ty = (H - 1.0) * 0.5 - rr.center.y;
        M.at<double>(0, 2) += tx;
        M.at<double>(1, 2) += ty;

        // 5) Warp trực tiếp ra patch đã thẳng theo chiều ngang
        cv::Mat patch;
        cv::warpAffine(gray, patch, M, cv::Size((int)std::round(W), (int)std::round(H)),
            cv::INTER_LINEAR, cv::BORDER_REPLICATE);

        return patch;
    }
    // ===== 2 hàm bạn yêu cầu =====
    cv::RotatedRect BarcodeCore::NormalizeRR(cv::RotatedRect rr, Symbology sym)
    {
        cv::RotatedRect norm = rr;

        // Chuẩn hóa góc về (-180, 180]
        while (norm.angle <= -180.f) norm.angle += 360.f;
        while (norm.angle > 180.f) norm.angle -= 360.f;

        // Nếu height > width thì xoay lại để width luôn là cạnh dài
        if (norm.size.height > norm.size.width) {
            std::swap(norm.size.width, norm.size.height);
            norm.angle += 90.f;
        }
        // Chuẩn hoá lại góc về (-180,180] sau khi cộng 90
        while (norm.angle <= -180.f) norm.angle += 360.f;
        while (norm.angle > 180.f) norm.angle -= 360.f;
        // Không ép góc nữa, để đúng góc thực tế trong ảnh
        return norm;
    }
    cv::RotatedRect BarcodeCore::RectFromCorners(const std::vector<cv::Point2f>& pts)
    {
        if (pts.size() >= 3) return cv::minAreaRect(pts);
        if (!pts.empty()) {
            cv::Rect bb = cv::boundingRect(pts);
            cv::Point2f c(bb.x + 0.5f * bb.width, bb.y + 0.5f * bb.height);
            return cv::RotatedRect(c, cv::Size2f((float)bb.width, (float)bb.height), 0.f);
        }
        return cv::RotatedRect();
    }


    // ---------- preprocess fallback (đơn giản) ----------
    cv::Mat BarcodeCore::Preprocess1D_Auto(const cv::Mat& gray)
    {
        const int S = std::min(gray.cols, gray.rows);
        cv::Mat clahe; {
            cv::Scalar m, s; cv::meanStdDev(gray, m, s);
            double clip = std::max(1.5, std::min(5.0, 1.5 + (s[0] / 70.0) * 3.5));
            int tile = clampi(S / 16, 8, 32);
            cv::createCLAHE(clip, cv::Size(tile, tile))->apply(gray, clahe);
        }
        cv::Mat smooth; {
            int d = evenUp(clampi(S / 160, 3, 9));
            double sc = 12 + (S / 800.0) * 6;
            cv::bilateralFilter(clahe, smooth, d, sc, sc);
        }
        int k = evenUp(clampi(S / 40, 9, 41));
        cv::Mat se = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(k, k));
        cv::Mat black; cv::morphologyEx(smooth, black, cv::MORPH_BLACKHAT, se);
        cv::Mat gx, gy, anis; cv::Sobel(black, gx, CV_32F, 1, 0, 3); cv::Sobel(black, gy, CV_32F, 0, 1, 3);
        anis = cv::abs(gx) - 0.5f * cv::abs(gy);
        cv::threshold(anis, anis, 0, 0, cv::THRESH_TOZERO);
        double mn, mx; cv::minMaxLoc(anis, &mn, &mx);
        anis.convertTo(anis, CV_8U, mx > 1e-6 ? 255.0 / mx : 1.0);
        cv::Mat bw; cv::threshold(anis, bw, 0, 255, cv::THRESH_BINARY | cv::THRESH_OTSU);
        cv::Mat ker = cv::getStructuringElement(cv::MORPH_RECT,
            cv::Size(evenUp(clampi(S / 20, 15, 61)), evenUp(clampi(S / 80, 3, 21))));
        cv::morphologyEx(bw, bw, cv::MORPH_CLOSE, ker);
        return bw;
    }

    // ---------- Mask 1D không xoay ảnh ----------
    cv::Mat BarcodeCore::MakeMask1D_NoRotate(const cv::Mat& gray8)
    {
        CV_Assert(gray8.type() == CV_8U);
        // Gradients (Scharr ổn trên nền hột)
        cv::Mat gx, gy; cv::Scharr(gray8, gx, CV_32F, 1, 0); cv::Scharr(gray8, gy, CV_32F, 0, 1);

        // Structure tensor
        int s = std::max(3, std::min(gray8.cols, gray8.rows) / 200);
        cv::Mat gxx = gx.mul(gx), gyy = gy.mul(gy), gxy = gx.mul(gy);
        cv::GaussianBlur(gxx, gxx, { 0,0 }, s); cv::GaussianBlur(gyy, gyy, { 0,0 }, s); cv::GaussianBlur(gxy, gxy, { 0,0 }, s);

        // coherence ≈ (λ1-λ2)/(λ1+λ2)
        cv::Mat d; cv::magnitude(gxx - gyy, 2.0f * gxy, d);
        cv::Mat t = gxx + gyy;
        cv::Mat coh = (d + 1e-6f) / (t + 1e-6f);

        // score = coherence × |grad|
        cv::Mat mag; cv::magnitude(gx, gy, mag);
        cv::Mat score = coh.mul(mag);

        // Ngưỡng robust + close mảnh
        cv::Scalar m, sd; cv::meanStdDev(score, m, sd);
        double thr = m[0] + 0.8 * sd[0];
        cv::Mat bw; cv::threshold(score, bw, thr, 255, cv::THRESH_BINARY);
        bw.convertTo(bw, CV_8U);

        int S = std::min(gray8.cols, gray8.rows);
        cv::morphologyEx(bw, bw, cv::MORPH_CLOSE,
            cv::getStructuringElement(cv::MORPH_RECT,
                cv::Size(std::max(9, S / 60) | 1, 3)));
        return bw;
    }
    // Otsu cho vector 1D float an toàn: trả thr trên THANG GIÁ TRỊ GỐC
    static bool Otsu1D_F32_ToNativeScale(const cv::Mat& vecF32 /* 1xN hoặc Nx1, CV_32F */,
        double& thr_out)
    {
        CV_Assert(vecF32.channels() == 1);
        if (vecF32.empty())
            return false;

        cv::Mat v32f;
        if (vecF32.type() != CV_32F) vecF32.convertTo(v32f, CV_32F);
        else v32f = vecF32;

        // thay NaN/Inf bằng 0
        cv::patchNaNs(v32f, 0);

        double minv = 0, maxv = 0;
        cv::minMaxLoc(v32f, &minv, &maxv);
        if (!(maxv > minv)) { // zero-variance hoặc dữ liệu hỏng
            thr_out = minv;   // tùy bạn, có thể trả false
            return false;
        }

        // scale → 8-bit để dùng Otsu
        cv::Mat v8;
        const double alpha = 255.0 / (maxv - minv);
        const double beta = -minv * alpha;
        v32f.convertTo(v8, CV_8U, alpha, beta);

        // chạy Otsu trên 8-bit (đưa dst vào dummy để tránh allocate lớn)
        cv::Mat dummy;
        double thr8 = cv::threshold(v8, dummy, 0, 255, cv::THRESH_BINARY | cv::THRESH_OTSU);

        // đổi ngưỡng về thang gốc
        thr_out = minv + (thr8 / 255.0) * (maxv - minv);
        return true;
    }

    // ---------- ước lượng & lọc nhiều box 1D trên ảnh gốc ----------
    std::vector<BarcodeCore::OneDBox>
        BarcodeCore::Detect1D_PerBox_NoGlobalRotate(const cv::Mat& gray8)
    {
        cv::Mat mask = MakeMask1D_NoRotate(gray8);
        Debugger dummy; // không lưu debug ở đây; lưu phía ngoài nếu cần

        // Connected components
        cv::Mat lbl, stats, cent;
        int n = cv::connectedComponentsWithStats(mask, lbl, stats, cent, 8, CV_32S);

        // Helpers cho lọc thêm
        auto PassStripeFrequency = [&](const cv::Rect& r)->bool {
            cv::Rect rr = r & cv::Rect(0, 0, gray8.cols, gray8.rows);
            if (rr.empty()) return false;
            cv::Mat roi = gray8(rr);
            cv::Mat proj;
            cv::reduce(roi, proj, 1, cv::REDUCE_AVG, CV_32F); // H×1

            double thr = 0.0;
            bool ok = Otsu1D_F32_ToNativeScale(proj, thr);
            if (!ok) {
                // fallback: dùng median làm ngưỡng
                cv::Mat sorted; cv::sort(proj.reshape(1, 1), sorted, cv::SORT_ASCENDING);
                int mid = std::max(0, sorted.cols / 2 - 1);
                thr = sorted.at<float>(0, mid);
            }
            int trans = 0;
            for (int i = 1; i < proj.rows; i++) {
                float a = proj.at<float>(i - 1, 0), b = proj.at<float>(i, 0);
                if ((a - (float)thr) * (b - (float)thr) < 0) trans++;
            }


            return trans >= 6 && trans <= 220;
            };
        auto PassColumnVariance = [&](const cv::Rect& r)->bool {
            cv::Rect rr = r & cv::Rect(0, 0, gray8.cols, gray8.rows);
            if (rr.empty()) return false;
            cv::Mat roi = gray8(rr);
            cv::Mat gx; cv::Sobel(roi, gx, CV_32F, 1, 0, 3);
            cv::Mat colVar; cv::reduce(cv::abs(gx), colVar, 0, cv::REDUCE_AVG, CV_32F); // 1×W
            cv::Scalar mean, sd; cv::meanStdDev(colVar, mean, sd);
            return sd[0] > 2.0; // lỏng, tuỳ ảnh có thể tăng 3–5
            };

        std::vector<OneDBox> boxes;
        boxes.reserve(std::max(1, n - 1));

        for (int i = 1; i < n; i++) {
            int x = stats.at<int>(i, cv::CC_STAT_LEFT);
            int y = stats.at<int>(i, cv::CC_STAT_TOP);
            int w = stats.at<int>(i, cv::CC_STAT_WIDTH);
            int h = stats.at<int>(i, cv::CC_STAT_HEIGHT);
            int area = stats.at<int>(i, cv::CC_STAT_AREA);
            if (area < 700) continue;

            cv::Rect rc(x, y, w, h);
            if (!PassStripeFrequency(rc)) continue;
            if (!PassColumnVariance(rc))  continue;

            // Lấy toàn bộ điểm của CC để tính PCA (góc riêng)
            std::vector<cv::Point> pts;
            pts.reserve(area);
            for (int yy = y; yy < y + h; ++yy) {
                const int* L = lbl.ptr<int>(yy);
                for (int xx = x; xx < x + w; ++xx) if (L[xx] == i) pts.emplace_back(xx, yy);
            }
            if (pts.size() < 50) continue;

            cv::Mat data(pts.size(), 2, CV_32F);
            for (size_t k = 0; k < pts.size(); ++k) { data.at<float>(k, 0) = pts[k].x; data.at<float>(k, 1) = pts[k].y; }
            cv::PCA pca(data, cv::Mat(), cv::PCA::DATA_AS_ROW);
            cv::Point2f v(pca.eigenvectors.at<float>(0, 0), pca.eigenvectors.at<float>(0, 1));
            float angle = std::atan2(v.y, v.x) * 180.0f / (float)CV_PI; // dương = CCW

            // Fit minAreaRect để lấy size/center & gán góc PCA
            cv::RotatedRect rr = cv::minAreaRect(pts);
            rr.angle = angle;
            rr = NormalizeRR(rr);

            float ar = rr.size.width / std::max(1.f, rr.size.height);
            if (ar < 2.0f) continue;

            boxes.push_back({ rr, float(area) * ar });
        }

        // NMS (bbox) để bỏ trùng
        auto IoU_BBox = [](const cv::RotatedRect& A, const cv::RotatedRect& B) {
            cv::Rect a = A.boundingRect(), b = B.boundingRect();
            int inter = (a & b).area(), uni = a.area() + b.area() - inter; return uni > 0 ? float(inter) / uni : 0.f;
            };

        std::sort(boxes.begin(), boxes.end(), [](auto& a, auto& b) {return a.score > b.score; });
        std::vector<OneDBox> keep; std::vector<char> sup(boxes.size(), 0);
        for (size_t i = 0; i < boxes.size(); ++i) {
            if (sup[i]) continue;
            keep.push_back(boxes[i]);
            for (size_t j = i + 1; j < boxes.size(); ++j)
                if (IoU_BBox(boxes[i].rr, boxes[j].rr) > 0.35f) sup[j] = 1;
        }
        return keep;
    }

    // ---------- Trim patch trước ZXing (bỏ biên ngang ít biến thiên) ----------
    cv::Mat BarcodeCore::TrimHorizBorders(const cv::Mat& patch)
    {
        if (patch.empty() || patch.cols < 16) return patch.clone();
        cv::Mat gx; cv::Sobel(patch, gx, CV_32F, 1, 0, 3);
        cv::Mat colVar; cv::reduce(cv::abs(gx), colVar, 0, cv::REDUCE_AVG, CV_32F); // 1×W
        double maxv; cv::minMaxLoc(colVar, nullptr, &maxv);
        float thr = (float)(0.25 * maxv);
        int L = 0, R = patch.cols - 1;
        while (L < R && colVar.at<float>(0, L) < thr) L++;
        while (R > L && colVar.at<float>(0, R) < thr) R--;
        cv::Rect roi(std::max(0, L - 2), 0, std::max(10, R - L + 4), patch.rows);
        roi &= cv::Rect(0, 0, patch.cols, patch.rows);
        return patch(roi).clone();
    }
    struct RotBox { cv::RotatedRect rr; float angle; float score; };

    static float PCAAngleDeg(const std::vector<cv::Point>& pts) {
        cv::Mat data((int)pts.size(), 2, CV_32F);
        for (int i = 0; i < (int)pts.size(); ++i) { data.at<float>(i, 0) = pts[i].x; data.at<float>(i, 1) = pts[i].y; }
        cv::PCA pca(data, cv::Mat(), cv::PCA::DATA_AS_ROW);
        cv::Point2f v(pca.eigenvectors.at<float>(0, 0), pca.eigenvectors.at<float>(0, 1));
        float ang = std::atan2(v.y, v.x) * 180.0f / (float)CV_PI;
        if (ang > 90) ang -= 180; if (ang <= -90) ang += 180; // (-90,90]
        return ang;
    }
    // Mask 1D đơn giản (coherence × |grad|) để lấy CC
    cv::Mat MakeMask1D(const cv::Mat& gray8) {
        cv::Mat gx, gy; cv::Scharr(gray8, gx, CV_32F, 1, 0); cv::Scharr(gray8, gy, CV_32F, 0, 1);
        cv::Mat gxx = gx.mul(gx), gyy = gy.mul(gy), gxy = gx.mul(gy), d, t;
        cv::GaussianBlur(gxx, gxx, { 0,0 }, 3); cv::GaussianBlur(gyy, gyy, { 0,0 }, 3); cv::GaussianBlur(gxy, gxy, { 0,0 }, 3);
        cv::magnitude(gxx - gyy, 2.0f * gxy, d);  t = gxx + gyy;
        cv::Mat coh = (d + 1e-6f) / (t + 1e-6f), mag; cv::magnitude(gx, gy, mag);
        cv::Mat score = coh.mul(mag);
        cv::Scalar m, sd; cv::meanStdDev(score, m, sd);
        double thr = m[0] + 0.8 * sd[0];
        cv::Mat bw; cv::threshold(score, bw, thr, 255, cv::THRESH_BINARY); bw.convertTo(bw, CV_8U);
        cv::morphologyEx(bw, bw, cv::MORPH_CLOSE, cv::getStructuringElement(cv::MORPH_RECT, { 15,3 }));
        cv::imwrite("gray.png", bw);
        return bw;
    }
    std::vector<RotBox> FindRotatedBoxes_1D(const cv::Mat& gray8) {
        cv::Mat mask = MakeMask1D(gray8);
        cv::Mat lbl, stats, cent;
        int n = cv::connectedComponentsWithStats(mask, lbl, stats, cent, 8, CV_32S);

        std::vector<RotBox> out;
        for (int i = 1; i < n; i++) {
            int x = stats.at<int>(i, cv::CC_STAT_LEFT);
            int y = stats.at<int>(i, cv::CC_STAT_TOP);
            int w = stats.at<int>(i, cv::CC_STAT_WIDTH);
            int h = stats.at<int>(i, cv::CC_STAT_HEIGHT);
            int area = stats.at<int>(i, cv::CC_STAT_AREA);
            if (area < 700) continue;

            std::vector<cv::Point> pts; pts.reserve(area);
            for (int yy = y; yy < y + h; ++yy) {
                const int* L = lbl.ptr<int>(yy);
                for (int xx = x; xx < x + w; ++xx) if (L[xx] == i) pts.emplace_back(xx, yy);
            }
            if (pts.size() < 50) continue;

            // Góc theo PCA
            float ang = PCAAngleDeg(pts);

            // Box xoay tốt nhất từ toàn bộ điểm
            cv::RotatedRect rr = cv::minAreaRect(pts);

            // Ưu tiên độ thuôn
            float ar = rr.size.width / std::max(1.f, rr.size.height);
            out.push_back({ rr, ang, area * ar });
        }
        // (tuỳ chọn) NMS theo IoU của boundingRect để loại trùng
        std::sort(out.begin(), out.end(), [](auto& a, auto& b) {return a.score > b.score; });
        return out;
    }

    // ---------- API tương thích cũ ----------

    static inline cv::RotatedRect MakeHorizontal(cv::RotatedRect rr)
    {
        if (rr.size.width < rr.size.height && rr.angle <= 90) {       // xoay quy ước width là cạnh dài
            std::swap(rr.size.width, rr.size.height);
            rr.angle -= 90.f;
        }
        else if (rr.size.width > rr.size.height && rr.angle > 90)
        {
            std::swap(rr.size.width, rr.size.height);
            rr.angle -= 90.f;
        }

        // rr.angle = 0.f;                              // snap về 0°
        return rr;
    }
    // ---------- API chính: detect per-box → crop → ZXing ----------
    void BarcodeCore::DetectAll(const cv::Mat& src,
        std::vector<CodeResult>& out,
        const DetectOptions& opt, bool FindBox) const
    {
        out.clear();
        if (src.empty()) return;

#if defined(__cpp_lib_filesystem) || _HAS_CXX17
        if (opt.debugSave && !opt.debugDir.empty()) {
            fs::create_directories(opt.debugDir);
        }
#endif

        Debugger D; D.init(opt.debugSave, opt.debugDraw, opt.debugDir);

        // Gray
        cv::Mat gray;
        if (src.channels() == 1) gray = src;
        else cv::cvtColor(src, gray, cv::COLOR_BGR2GRAY);

        // Preprocess nhẹ (tùy chọn)
        cv::Mat grayBoost = gray.clone();
        // ZXing hints
        ZXing::DecodeHints hints;
        hints.setTryHarder(true);
        hints.setTryRotate(false);
        hints.setTryDownscale(true);
        hints.setFormats(ZXing::BarcodeFormat::Any);
        DebugViz::Save(D, grayBoost, "00_gray");
        if (!FindBox) {
            // ZXing hints
            ZXing::DecodeHints hTryAll; hTryAll.setTryHarder(true); hTryAll.setTryRotate(true); hTryAll.setTryDownscale(true);  hTryAll.setFormats(ZXing::BarcodeFormat::Any);
            ZXing::DecodeHints hNoDown; hNoDown = hTryAll; hNoDown.setTryDownscale(false);
            ZXing::DecodeHints h1D;     h1D = hTryAll;
            h1D.setFormats(ZXing::BarcodeFormat::EAN13 | ZXing::BarcodeFormat::EAN8 |
                ZXing::BarcodeFormat::UPCA | ZXing::BarcodeFormat::UPCE |
                ZXing::BarcodeFormat::Code128 | ZXing::BarcodeFormat::Code39 |
                ZXing::BarcodeFormat::ITF | ZXing::BarcodeFormat::Codabar);
            auto run_pass = [&](const cv::Mat& g, const ZXing::DecodeHints& hints, std::vector<CodeResult>& sink)
                {
                    ZXing::ImageView iv(g.data, g.cols, g.rows, ZXing::ImageFormat::Lum, (int)g.step);
                    auto results = ZXing::ReadBarcodes(iv, hints);
                    for (auto& zr : results) {
                        if (!zr.isValid()) continue;

                        CodeResult cr;
                        cr.text = zr.text();
                        cr.symbology = MapFormat((int)zr.format());

                        std::vector<cv::Point2f> poly;
                        const auto& pos = zr.position();
                        poly.reserve(pos.size());
                        for (const auto& pt : pos) poly.emplace_back((float)pt.x, (float)pt.y);

                        if (poly.size() >= 4) {
                            OrderQuadTLTRBRBL(poly);
                        }
                        else if (poly.size() == 3) {
                            cv::Point2f c(0, 0); for (auto& p : poly) c += p; c *= (1.f / 3.f);
                            int far = 0; float md = -1.f;
                            for (int i = 0; i < 3; ++i) { float d = (float)cv::norm(poly[i] - c); if (d > md) { md = d; far = i; } }
                            cv::Point2f p4 = c * 2 - poly[far];
                            poly.push_back(p4); OrderQuadTLTRBRBL(poly);
                        }
                        else if (poly.size() == 1) {
                            float s = 12.f; auto p = poly[0];
                            poly = { {p.x - s,p.y - s},{p.x + s,p.y - s},{p.x + s,p.y + s},{p.x - s,p.y + s} };
                        }
                        else {
                            continue;
                        }

                        cr.corners = std::move(poly);
                        cr.rrect = NormalizeRR(RectFromCorners(cr.corners), cr.symbology);
                        sink.push_back(std::move(cr));
                    }
                };
                std::vector<CodeResult> tmp;
                cv::Mat g0 = gray;
                cv::Mat g1 = g0; // có thể boost nhẹ nếu cần (đang tắt)
                cv::Mat g2; cv::resize(g0, g2, cv::Size(), 0.75, 0.75, cv::INTER_AREA);
                run_pass(g0, hTryAll, tmp);
                if (tmp.empty()) run_pass(g1, hTryAll, tmp);
                if (tmp.empty()) run_pass(g0, h1D, tmp);
                if (tmp.empty()) run_pass(g2, h1D, tmp);
                if (tmp.empty()) run_pass(g0, hNoDown, tmp);
                out.swap(tmp);
            return;
        }


        // ===== 1D detect per-box (không xoay toàn cục) =====
        auto boxes = FindRotatedBoxes_1D(grayBoost);


        // Debug: overlay các box phát hiện
        if (D.save || D.draw) {
            std::vector<cv::RotatedRect> rrs; rrs.reserve(boxes.size());
            for (auto& b : boxes) rrs.push_back(b.rr);
            cv::Mat ov; cv::cvtColor(grayBoost, ov, cv::COLOR_GRAY2BGR);
            DebugViz::DrawRRects(ov, rrs);
            DebugViz::Save(D, ov, "01_boxes_overlay");
        }



        // ===== CROP + ZXing từng ROI =====
        int roiIdx = 0;
        for (auto& b : boxes)
        {
            cv::Mat patch = CropRotatedBox(grayBoost, b.rr, 0.06f);
            //    patch = TrimHorizBorders(patch);

            ZXing::ImageView iv(patch.data, patch.cols, patch.rows,
                ZXing::ImageFormat::Lum, (int)patch.step);
            auto zs = ZXing::ReadBarcodes(iv, hints);
            cv::RotatedRect rrSnap = MakeHorizontal(b.rr);

            cv::Point2f v[4];
            rrSnap.points(v);                    // trả: bottomLeft, topLeft, topRight, bottomRight
            std::vector<cv::Point2f> poly{ v, v + 4 };
            OrderQuadTLTRBRBL(poly);             // chuyển về TL,TR,BR,BL


            if (!zs.empty()) {
                cv::Mat patchBgr; cv::cvtColor(patch, patchBgr, cv::COLOR_GRAY2BGR);
                int line = 0;
                for (auto& z : zs) {
                    if (!z.isValid()) continue;
                    CodeResult cr;

                    cr.text = z.text();
                    cr.symbology = MapFormat((int)z.format());
                    cr.rrect = rrSnap;
                    cr.corners = poly;
                    cr.score = b.score;
                    out.push_back(std::move(cr));

                    std::string s = z.text();
                    if (s.size() > 48) s = s.substr(0, 48) + "...";
                    DebugViz::PutText(patchBgr, s, { 8, 22 + line }, 0.6, { 0,255,255 }, 2);
                    line += 24;
                }
                DebugViz::Save(D, patchBgr, "11_patch_decoded_" + std::to_string(roiIdx));
            }
            ++roiIdx;
        }

        // ===== Overlay cuối =====
        if (opt.debugSave || opt.debugDraw) {
            cv::Mat dbg;
            if (src.channels() == 3) dbg = src.clone();
            else cv::cvtColor(src, dbg, cv::COLOR_GRAY2BGR);

            std::vector<cv::RotatedRect> rrs; rrs.reserve(out.size());
            for (auto& r : out) rrs.push_back(r.rrect);
            DebugViz::DrawRRects(dbg, rrs, { 0,255,0 }, 2);

            for (auto& r : out) {
                std::string s = r.text.empty() ? "(no-text)" : r.text;
                DebugViz::PutText(dbg, s, r.rrect.center);
            }
            DebugViz::Save(D, dbg, "12_final_overlay");
        }
    }

} // namespace BeeCpp