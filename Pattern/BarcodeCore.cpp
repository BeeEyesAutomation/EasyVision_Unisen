#include "BarcodeCore.h"
#include <opencv2/imgproc.hpp>
#include <opencv2/opencv.hpp>

#include <algorithm>
#include <cmath>

// ZXing-C++
#include <ZXing/ReadBarcode.h>
#include <ZXing/DecodeHints.h>
#include <ZXing/Result.h>
#include <ZXing/ImageView.h>
#include <ZXing/BarcodeFormat.h>

namespace BeeCpp {

    //---------- helpers ----------//

    CodeSymbology BarcodeCore::MapFormat(int fmt)
    {
        using ZXing::BarcodeFormat;
        switch (static_cast<BarcodeFormat>(fmt)) {
        case BarcodeFormat::QRCode:     return CodeSymbology::QR_CODE;
        case BarcodeFormat::EAN13:      return CodeSymbology::EAN_13;
        case BarcodeFormat::EAN8:       return CodeSymbology::EAN_8;
        case BarcodeFormat::UPCA:       return CodeSymbology::UPC_A;
        case BarcodeFormat::UPCE:       return CodeSymbology::UPC_E;
        case BarcodeFormat::Code128:    return CodeSymbology::CODE_128;
        case BarcodeFormat::Code39:     return CodeSymbology::CODE_39;
        case BarcodeFormat::ITF:         return CodeSymbology::ITF;
        case BarcodeFormat::Codabar:     return CodeSymbology::CODABAR;
        case BarcodeFormat::PDF417:      return CodeSymbology::PDF417;
        case BarcodeFormat::Aztec:       return CodeSymbology::AZTEC;
        case BarcodeFormat::DataMatrix: return CodeSymbology::DATA_MATRIX;
        default:                         return CodeSymbology::Unknown;
        }
    }

    cv::RotatedRect BarcodeCore::RectFromCorners(const std::vector<cv::Point2f>& pts)
    {
        if (pts.size() >= 3) return cv::minAreaRect(pts);
        if (!pts.empty()) {
            cv::Rect bb = cv::boundingRect(pts);
            cv::Point2f center(
                bb.x + 0.5f * bb.width,
                bb.y + 0.5f * bb.height
            );
            return cv::RotatedRect(center, cv::Size2f((float)bb.width, (float)bb.height), 0.f);
        }
        return cv::RotatedRect();
    }
    static inline bool Is1D(CodeSymbology s)
    {
        switch (s) {
        case CodeSymbology::EAN_13: case CodeSymbology::EAN_8:
        case CodeSymbology::UPC_A: case CodeSymbology::UPC_E:
        case CodeSymbology::CODE_128: case CodeSymbology::CODE_39:
        case CodeSymbology::ITF: case CodeSymbology::CODABAR:
            return true;
        default: return false;
        }
    }
    // Đưa width là trục dài, angle ổn định (-180,180]
    cv::RotatedRect BarcodeCore::NormalizeRR(cv::RotatedRect rr, CodeSymbology sym)
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

    // Sắp các đỉnh thành TL,TR,BR,BL
    void BarcodeCore::OrderQuadTLTRBRBL(std::vector<cv::Point2f>& q)
    {
        if (q.size() < 4) return;
        cv::Point2f c(0, 0);
        for (auto& p : q) c += p;
        c *= (1.f / (float)q.size());

        std::sort(q.begin(), q.end(), [&](const cv::Point2f& a, const cv::Point2f& b) {
            float aa = std::atan2(a.y - c.y, a.x - c.x);
            float bb = std::atan2(b.y - c.y, b.x - c.x);
            return aa < bb;
            });
        auto it = std::min_element(q.begin(), q.end(),
            [](const cv::Point2f& a, const cv::Point2f& b) { return (a.x + a.y) < (b.x + b.y); });
        std::rotate(q.begin(), it, q.end());
    }

    // Dựng tứ giác từ 2 điểm (1D)
    std::vector<cv::Point2f> BarcodeCore::SynthesizeQuadFromLine(
        const cv::Point2f& p0, const cv::Point2f& p1, float heightRatio, float quiet)
    {
        cv::Point2f c = (p0 + p1) * 0.5f;
        cv::Point2f v = p1 - p0;
        float w = std::sqrt(v.x * v.x + v.y * v.y);
        if (w < 1e-3f) w = 1.f;

        float w2 = 0.5f * w * (1.f + 2.f * quiet); // mở rộng quiet zone
        float h2 = 0.5f * w * heightRatio;       // bề dày tương đối theo chiều dài

        cv::Point2f u = v * (1.f / w);           // đơn vị dọc theo mã
        cv::Point2f n(-u.y, u.x);                // pháp tuyến

        cv::Point2f tl = c - u * w2 - n * h2;
        cv::Point2f tr = c + u * w2 - n * h2;
        cv::Point2f br = c + u * w2 + n * h2;
        cv::Point2f bl = c - u * w2 + n * h2;
        return { tl,tr,br,bl };
    }

    // Ước lượng height theo text (fallback rất nhanh)
    float BarcodeCore::GuessHeightRatioByText(const std::string& txt)
    {
        float r = 0.08f + 0.003f * (float)txt.size(); // dài hơn → dày hơn
        if (r < 0.12f) r = 0.12f;
        if (r > 0.28f) r = 0.28f;
        return r;
    }
    uchar BilinearSample(const cv::Mat& img, float x, float y)
    {
        int x0 = static_cast<int>(x);
        int y0 = static_cast<int>(y);
        int x1 = std::min(x0 + 1, img.cols - 1);
        int y1 = std::min(y0 + 1, img.rows - 1);

        float dx = x - x0;
        float dy = y - y0;

        float val =
            (1 - dx) * (1 - dy) * img.at<uchar>(y0, x0) +
            dx * (1 - dy) * img.at<uchar>(y0, x1) +
            (1 - dx) * dy * img.at<uchar>(y1, x0) +
            dx * dy * img.at<uchar>(y1, x1);

        return static_cast<uchar>(cv::saturate_cast<uchar>(std::round(val)));
    }

    // Quét profile theo pháp tuyến để đo bề dày thực (px). Trả -1 nếu thất bại.
 
    void Estimate1DHalfHeightsAlongLine(
        const cv::Mat& gray,
        const cv::Point2f& mid,     // trung điểm p0-p1
        const cv::Point2f& u,       // hướng mã vạch (tangential)
        const cv::Point2f& n,       // pháp tuyến (orthogonal)
        float w,                    // chiều dài mã (dùng để scale height)
        float& hTop, float& hBot    // output
    )
    {
        CV_Assert(gray.type() == CV_8UC1);

        // Sampling theo n (pháp tuyến) ± chiều
        const int N = static_cast<int>(w * 0.5f); // sample từ −N → N
        std::vector<float> prof;

        for (int i = -N; i <= N; ++i) {
            float px = mid.x + i * n.x;
            float py = mid.y + i * n.y;

            if (px >= 1 && px < gray.cols - 2 &&
                py >= 1 && py < gray.rows - 2)
            {
                prof.push_back(BilinearSample(gray, px, py));
            }
            else {
                prof.push_back(0.f); // pad với 0
            }
        }

        // Tính gradient
        std::vector<float> grad(prof.size(), 0.f);
        for (size_t i = 1; i + 1 < prof.size(); ++i) {
            grad[i] = std::abs(prof[i + 1] - prof[i - 1]) * 0.5f;
        }

        // Ngưỡng Otsu
        cv::Mat gradMat(grad, true); // copy từ vector
        gradMat.convertTo(gradMat, CV_8U);
        double thr = cv::threshold(gradMat, cv::noArray(), 0, 255, cv::THRESH_OTSU);

        // Tìm biên trên / dưới
        int midIdx = static_cast<int>(prof.size() / 2);
        int top = midIdx, bot = midIdx;

        for (int i = midIdx + 1; i < static_cast<int>(grad.size()); ++i) {
            if (grad[i] > thr) {
                top = i;
                break;
            }
        }

        for (int i = midIdx - 1; i >= 0; --i) {
            if (grad[i] > thr) {
                bot = i;
                break;
            }
        }

        // Nếu không có biên rõ, fallback dùng peak gradient
        if (top == midIdx) {
            for (int i = midIdx + 1; i < (int)grad.size(); ++i)
                if (grad[i] > grad[top]) top = i;
        }

        if (bot == midIdx) {
            for (int i = midIdx - 1; i >= 0; --i)
                if (grad[i] > grad[bot]) bot = i;
        }

        // Tính chiều cao (số bước theo pháp tuyến)
        float pixelGap = 1.0f;
        hTop = (top - midIdx) * pixelGap;
        hBot = (midIdx - bot) * pixelGap;

        // Clamp theo tỷ lệ với chiều dài
        float minH = 0.06f * w, maxH = 0.25f * w;
        hTop = std::clamp(hTop, minH, maxH);
        hBot = std::clamp(hBot, minH, maxH);

        // Optional điều chỉnh
        hTop *= 1.1f;
        hBot *= 0.9f;
    }




    //---------- main ----------//
 

    static std::vector<cv::Point2f> SynthesizeQuadFromLineAsym(
        const cv::Point2f& p0, const cv::Point2f& p1,
        float hTop, float hBot, float quiet)
    {
        cv::Point2f c = (p0 + p1) * 0.5f;
        cv::Point2f v = p1 - p0;
        float w = std::sqrt(v.x * v.x + v.y * v.y);
        if (w < 1e-3f) w = 1.f;

        float w2 = 0.5f * w * (1.f + 2.f * quiet);
        cv::Point2f u = v * (1.f / w);
        cv::Point2f n(-u.y, u.x);

        cv::Point2f tl = c - u * w2 - n * hBot; // lưu ý: bot nằm phía -n
        cv::Point2f bl = c - u * w2 + n * hTop; // top nằm phía +n
        cv::Point2f tr = c + u * w2 - n * hBot;
        cv::Point2f br = c + u * w2 + n * hTop;

        // trả về TL,TR,BR,BL
        return { tl,tr,br,bl };
    }
    // Trả u (trục dọc theo barcode), n (pháp tuyến), p0/p1 là hai đầu mút theo u.
    static void FitAxisByPCA(const std::vector<cv::Point2f>& pts,
        cv::Point2f& u, cv::Point2f& n,
        cv::Point2f& p0, cv::Point2f& p1)
    {
        CV_Assert(pts.size() >= 2);
        cv::Mat data((int)pts.size(), 2, CV_32F);
        cv::Point2f mean(0, 0);
        for (int i = 0; i < (int)pts.size(); ++i) {
            data.at<float>(i, 0) = pts[i].x;
            data.at<float>(i, 1) = pts[i].y;
            mean += pts[i];
        }
        mean *= 1.f / (float)pts.size();

        cv::PCA pca(data, cv::Mat(), cv::PCA::DATA_AS_ROW);
        cv::Point2f eig((float)pca.eigenvectors.at<float>(0, 0),
            (float)pca.eigenvectors.at<float>(0, 1));
        float len = std::sqrt(eig.x * eig.x + eig.y * eig.y);
        if (len < 1e-6f) { u = { 1,0 }; }
        else { u = eig * (1.f / len); }
        n = cv::Point2f(-u.y, u.x);

        // chiếu toàn bộ điểm lên u, lấy min/max -> hai đầu mút
        float minT = 1e30f, maxT = -1e30f;
        for (auto& p : pts) {
            cv::Point2f d = p - mean;
            float t = d.x * u.x + d.y * u.y; // dot(d, u)
            if (t < minT) { minT = t; p0 = mean + u * t; }
            if (t > maxT) { maxT = t; p1 = mean + u * t; }
        }
        // đảm bảo p0–p1 nằm theo u
        if (maxT - minT < 1e-3f) { p0 = mean - u * 10.f; p1 = mean + u * 10.f; }
    }
    static bool Estimate1DAnglePCA(const std::vector<cv::Point>& pts, float& outAngle)
    {
        if (pts.size() < 5)
            return false;

        cv::Mat data(pts.size(), 2, CV_64F);
        for (size_t i = 0; i < pts.size(); ++i) {
            data.at<double>(i, 0) = pts[i].x;
            data.at<double>(i, 1) = pts[i].y;
        }

        cv::PCA pca(data, cv::Mat(), cv::PCA::DATA_AS_ROW);
        cv::Vec2d dir = pca.eigenvectors.row(0); // trục chính
        outAngle = std::atan2(dir[1], dir[0]) * 180.0f / CV_PI;
        return true;
    }
    // ===== Percentile cho ảnh 8-bit (dùng chọn ngưỡng biên mạnh) =====
  

  
    // ========================= Helpers =========================
    struct ROIProposal {
        cv::Rect rect;
        float angleDeg;   // trục dài (đã chuẩn hóa [-90..90])
        bool is1D;        // vùng dạng dài (ứng viên 1D)
    };

    // Percentile cho ảnh 8-bit
    static int Percentile8U(const cv::Mat& img8u, double q) // q in [0..1]
    {
        CV_Assert(img8u.type() == CV_8U);
        int hist[256] = { 0 };
        for (int y = 0; y < img8u.rows; ++y) {
            const uchar* p = img8u.ptr<uchar>(y);
            for (int x = 0; x < img8u.cols; ++x) ++hist[p[x]];
        }
        const int total = img8u.rows * img8u.cols;
        const int cutoff = std::max(0, std::min(total - 1, (int)std::round(q * total)));
        int acc = 0;
        for (int i = 0; i < 256; ++i) {
            acc += hist[i];
            if (acc >= cutoff) return i;
        }
        return 255;
    }

    // Đề xuất ROI bằng Sobel + PCA (nhiều vùng)
    static std::vector<ROIProposal> ProposeROIsBySobelPCA(const cv::Mat& gray)
    {
        std::vector<ROIProposal> rois;

        // 1) Gradient magnitude
        cv::Mat gx, gy, mag;
        cv::Sobel(gray, gx, CV_32F, 1, 0, 3);
        cv::Sobel(gray, gy, CV_32F, 0, 1, 3);
        cv::magnitude(gx, gy, mag);

        // 2) Lấy biên mạnh theo percentile (ổn định nền/ánh sáng)
        cv::Mat mag8;
        cv::normalize(mag, mag8, 0, 255, cv::NORM_MINMAX);
        mag8.convertTo(mag8, CV_8U);
        const int thr = Percentile8U(mag8, 0.75); // 75th
        cv::Mat strong;
        cv::threshold(mag8, strong, thr, 255, cv::THRESH_BINARY);

        // 3) Contours
        std::vector<std::vector<cv::Point>> cc;
        cv::findContours(strong, cc, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_NONE);

        for (const auto& c : cc)
        {
            if ((int)c.size() < 100) continue;
            cv::Rect r = cv::boundingRect(c);
            if (r.width < 24 || r.height < 12) continue;
            if (r.width > gray.cols * 0.95 || r.height > gray.rows * 0.95) continue;

            cv::Mat data = cv::Mat(c).reshape(1);
            data.convertTo(data, CV_32F);
            cv::PCA pca(data, cv::Mat(), cv::PCA::DATA_AS_ROW);

            // trục dài & mức dị hướng
            float lam0 = pca.eigenvalues.at<float>(0);
            float lam1 = pca.eigenvalues.at<float>(1);
            cv::Vec2f ev0 = pca.eigenvectors.row(0);
            float ratio = (lam1 > 1e-6f) ? (lam0 / lam1) : 1e6f;

            float angle = std::atan2(ev0[1], ev0[0]) * 180.f / CV_PI;
            float rotToHorizontal = angle - std::round(angle / 180.f) * 180.f; // [-90..90]

            bool maybe1D = (ratio >= 3.0f) &&
                (r.width > r.height * 1.5f || r.height > r.width * 1.5f);

            rois.push_back({ r, rotToHorizontal, maybe1D });
        }

        if (rois.empty()) // fallback: quét toàn ảnh
            rois.push_back({ cv::Rect(0,0,gray.cols,gray.rows), 0.f, false });

        return rois;
    }

    // Xoay ROI 1D chắc chắn về ngang: thử θ và θ+90°, chọn phương án tốt
    static cv::Mat Rotate1DPatchToHorizontal(const cv::Mat& patchGray, float& usedDeg)
    {
        CV_Assert(patchGray.type() == CV_8UC1);
        const cv::Point2f c(patchGray.cols * 0.5f, patchGray.rows * 0.5f);

        // Lấy điểm biên mạnh cho PCA tin cậy
        cv::Mat gx, gy, mag, mag8, strong;
        cv::Sobel(patchGray, gx, CV_32F, 1, 0, 3);
        cv::Sobel(patchGray, gy, CV_32F, 0, 1, 3);
        cv::magnitude(gx, gy, mag);
        cv::normalize(mag, mag8, 0, 255, cv::NORM_MINMAX);
        mag8.convertTo(mag8, CV_8U);
        int thr = Percentile8U(mag8, 0.75);
        cv::threshold(mag8, strong, thr, 255, cv::THRESH_BINARY);

        std::vector<cv::Point> pts; cv::findNonZero(strong, pts);
        if ((int)pts.size() < 40) { usedDeg = 0.f; return patchGray.clone(); }

        cv::Mat data = cv::Mat(pts).reshape(1); data.convertTo(data, CV_32F);
        cv::PCA pca(data, cv::Mat(), cv::PCA::DATA_AS_ROW);
        float lam0 = pca.eigenvalues.at<float>(0), lam1 = pca.eigenvalues.at<float>(1);
        cv::Vec2f ev = (lam0 >= lam1) ? pca.eigenvectors.row(0) : pca.eigenvectors.row(1);
        float theta = std::atan2(ev[1], ev[0]) * 180.f / CV_PI;

        auto norm90 = [](float a) { return a - std::round(a / 180.f) * 180.f; };
        float cand0 = norm90(theta);
        float cand1 = norm90(theta + 90.f);

        auto score = [&](float deg)->double {
            cv::Mat R = cv::getRotationMatrix2D(c, -deg, 1.0), tmp;
            cv::warpAffine(patchGray, tmp, R, patchGray.size(), cv::INTER_LINEAR, cv::BORDER_REPLICATE);
            // sau xoay đúng, bbox biên sẽ rộng >> cao
            cv::Mat e; cv::Canny(tmp, e, 50, 150);
            std::vector<std::vector<cv::Point>> cs; cv::findContours(e, cs, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);
            if (cs.empty()) return -1.0;
            cv::Rect bb; for (auto& c : cs) bb |= cv::boundingRect(c);
            if (bb.empty()) return -1.0;
            return (double)bb.width / std::max(1, bb.height);
            };

        double s0 = score(cand0);
        double s1 = score(cand1);
        float best = (s1 > s0) ? cand1 : cand0;

        cv::Mat R = cv::getRotationMatrix2D(c, -best, 1.0), rot;
        cv::warpAffine(patchGray, rot, R, patchGray.size(), cv::INTER_LINEAR, cv::BORDER_REPLICATE);
        usedDeg = best;
        return rot;
    }

    // ========================= DetectAll =========================
    void BarcodeCore::DetectAll(const cv::Mat& src, std::vector<CodeResult>& out, bool enablePreprocess) const
    {
        out.clear();
        if (src.empty()) return;

        cv::Mat gray;
        if (src.type() == CV_8UC1) gray = src;
        else if (src.channels() == 3) cv::cvtColor(src, gray, cv::COLOR_BGR2GRAY);
        else                          cv::cvtColor(src, gray, cv::COLOR_BGR2GRAY);

        auto run_pass = [&](const cv::Mat& g, ZXing::DecodeHints hints, std::vector<CodeResult>& sink)
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
                    for (const auto& pt : pos)
                        poly.emplace_back((float)pt.x, (float)pt.y);

                    if (Is1D(cr.symbology)) {
                        cv::Point2f u, n, p0, p1;

                        if (poly.size() >= 2)
                            FitAxisByPCA(poly, u, n, p0, p1);
                        else if (poly.size() == 1) {
                            p0 = poly[0] - cv::Point2f(20.f, 0);
                            p1 = poly[0] + cv::Point2f(20.f, 0);
                            u = { 1.f, 0.f }; n = { 0.f, 1.f };
                        }
                        else continue;

                        float w = std::max(1e-3f, static_cast<float>(cv::norm(p1 - p0)));

                        float hTop = -1.f, hBot = -1.f;

                        try {
                            Estimate1DHalfHeightsAlongLine(gray, (p0 + p1) * 0.5f, u, n, w, hTop, hBot);
                        }
                        catch (...) {
                            hTop = hBot = -1.f;
                        }

                        if (hTop <= 0.f || hBot <= 0.f || !std::isfinite(hTop) || !std::isfinite(hBot)) {
                            float hrt = std::max(GuessHeightRatioByText(cr.text), 0.24f);
                            float h2 = 0.5f * w * hrt;
                            hTop = hBot = h2;
                        }
                        else {
                            float minH = 0.06f * w, maxH = 0.25f * w;
                            hTop = std::clamp(hTop, minH, maxH);
                            hBot = std::clamp(hBot, minH, maxH);
                            hTop *= 0.95f;
                            hBot *= 0.95f;
                        }

                        poly = SynthesizeQuadFromLineAsym(p0, p1, hTop, hBot, 0.02f);
                    }
                    else if (poly.size() >= 4) {
                        OrderQuadTLTRBRBL(poly);
                    }
                    else if (poly.size() == 3) {
                        cv::Point2f c(0, 0); for (auto& p : poly) c += p; c *= (1.f / 3.f);
                        int far = 0; float md = -1.f;
                        for (int i = 0; i < 3; ++i) {
                            float d = cv::norm(poly[i] - c);
                            if (d > md) { md = d; far = i; }
                        }
                        cv::Point2f p4 = c * 2 - poly[far];
                        poly.push_back(p4);
                        OrderQuadTLTRBRBL(poly);
                    }
                    else if (poly.size() == 1) {
                        float s = 12.f;
                        auto p = poly[0];
                        poly = {
                            {p.x - s, p.y - s},
                            {p.x + s, p.y - s},
                            {p.x + s, p.y + s},
                            {p.x - s, p.y + s}
                        };
                    }
                    else {
                        continue;
                    }

                    cr.corners = std::move(poly);
                    cr.rrect = NormalizeRR(RectFromCorners(cr.corners),cr.symbology);
                    sink.push_back(std::move(cr));
                }
            };

        // Hints ZXing
        ZXing::DecodeHints h1;
        h1.setTryHarder(true);
        h1.setTryRotate(true);
        h1.setTryDownscale(true);
        h1.setFormats(ZXing::BarcodeFormat::Any);

        ZXing::DecodeHints h2 = h1;
        h2.setTryDownscale(false);

        ZXing::DecodeHints h3 = h1;
        h3.setFormats(ZXing::BarcodeFormat::EAN13 | ZXing::BarcodeFormat::EAN8 |
            ZXing::BarcodeFormat::UPCA | ZXing::BarcodeFormat::UPCE |
            ZXing::BarcodeFormat::Code128 | ZXing::BarcodeFormat::Code39 |
            ZXing::BarcodeFormat::ITF | ZXing::BarcodeFormat::Codabar);

        // Ảnh tăng tương phản + resize
        cv::Mat g0 = gray.clone();
        cv::Mat g1 = g0;
        if (enablePreprocess) {
            cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE(2.0, cv::Size(8, 8));
            clahe->apply(g0, g1);
            cv::Mat blur;
            cv::GaussianBlur(g1, blur, cv::Size(0, 0), 1.2);
            cv::addWeighted(g1, 1.6, blur, -0.6, 0, g1);
        }

        cv::Mat g2;
        cv::resize(g0, g2, cv::Size(), 0.75, 0.75, cv::INTER_AREA);

        std::vector<CodeResult> tmp;
        run_pass(g0, h1, tmp);
        if (tmp.empty()) run_pass(g1, h1, tmp);
        if (tmp.empty()) run_pass(g0, h3, tmp);
        if (tmp.empty()) run_pass(g2, h3, tmp);
        if (tmp.empty()) run_pass(g0, h2, tmp);

        out.swap(tmp);
    }

}