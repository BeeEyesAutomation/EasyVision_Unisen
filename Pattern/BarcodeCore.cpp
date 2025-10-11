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

// ZXing-C++
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
        case BarcodeFormat::Code39:      return Symbology::Code39;
        case BarcodeFormat::Code93:      return Symbology::Code93;
        case BarcodeFormat::Code128:     return Symbology::Code128;
        case BarcodeFormat::ITF:         return Symbology::ITF;
        case BarcodeFormat::QRCode:      return Symbology::QR;
        case BarcodeFormat::DataMatrix:  return Symbology::DataMatrix;
        case BarcodeFormat::PDF417:      return Symbology::PDF417;
        case BarcodeFormat::Aztec:       return Symbology::Aztec;
        case BarcodeFormat::MaxiCode:    return Symbology::MaxiCode;
        case BarcodeFormat::Codabar:     return Symbology::Codabar;
        case BarcodeFormat::MicroQRCode: return Symbology::MicroQR;
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

        float W = rr.size.width, H = rr.size.height;
        if (W < H) {
            std::swap(W, H);
            rr.angle += 90.f;
        }
        W *= (1.f + pad);
        H *= (1.f + pad);

        cv::Mat M = cv::getRotationMatrix2D(rr.center, (double)rr.angle, 1.0);
        const double tx = (W - 1.0) * 0.5 - rr.center.x;
        const double ty = (H - 1.0) * 0.5 - rr.center.y;
        M.at<double>(0, 2) += tx;
        M.at<double>(1, 2) += ty;

        cv::Mat patch;
        cv::warpAffine(gray, patch, M, cv::Size((int)std::round(W), (int)std::round(H)),
            cv::INTER_LINEAR, cv::BORDER_REPLICATE);
        return patch;
    }
    cv::RotatedRect BarcodeCore::NormalizeRR(cv::RotatedRect norm, Symbology /*sym*/)
    {
        while (norm.angle <= -180.f) norm.angle += 360.f;
        while (norm.angle > 180.f) norm.angle -= 360.f;
        if (norm.size.height > norm.size.width) {
            std::swap(norm.size.width, norm.size.height);
            norm.angle += 90.f;
        }
        while (norm.angle <= -180.f) norm.angle += 360.f;
        while (norm.angle > 180.f) norm.angle -= 360.f;
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



    // Otsu cho vector 1D float an toàn
    static bool Otsu1D_F32_ToNativeScale(const cv::Mat& vecF32, double& thr_out)
    {
        CV_Assert(vecF32.channels() == 1);
        if (vecF32.empty()) return false;

        cv::Mat v32f;
        if (vecF32.type() != CV_32F) vecF32.convertTo(v32f, CV_32F);
        else v32f = vecF32;
        cv::patchNaNs(v32f, 0);

        double minv = 0, maxv = 0;
        cv::minMaxLoc(v32f, &minv, &maxv);
        if (!(maxv > minv)) { thr_out = minv; return false; }

        cv::Mat v8;
        const double alpha = 255.0 / (maxv - minv);
        const double beta = -minv * alpha;
        v32f.convertTo(v8, CV_8U, alpha, beta);

        cv::Mat dummy;
        double thr8 = cv::threshold(v8, dummy, 0, 255, cv::THRESH_BINARY | cv::THRESH_OTSU);
        thr_out = minv + (thr8 / 255.0) * (maxv - minv);
        return true;
    }

    // ---------- Mask 1D không xoay ảnh (dùng filter params) ----------
    cv::Mat BarcodeCore::MakeMask(const cv::Mat& gray8, const FilterParams& fp)
    {

        cv::Mat gx, gy; cv::Scharr(gray8, gx, CV_32F, 1, 0); cv::Scharr(gray8, gy, CV_32F, 0, 1);
        cv::Mat gxx = gx.mul(gx), gyy = gy.mul(gy), gxy = gx.mul(gy), d, t;
        cv::GaussianBlur(gxx, gxx, { 0,0 }, 3); cv::GaussianBlur(gyy, gyy, { 0,0 }, 3); cv::GaussianBlur(gxy, gxy, { 0,0 }, 3);
        cv::magnitude(gxx - gyy, 2.0f * gxy, d);  t = gxx + gyy;
        cv::Mat coh = (d + 1e-6f) / (t + 1e-6f), mag; cv::magnitude(gx, gy, mag);
        cv::Mat score = coh.mul(mag);
        cv::Scalar m, sd; cv::meanStdDev(score, m, sd);
        double thr = m[0] + 0.8 * sd[0];
        cv::Mat bw; cv::threshold(score, bw, thr, 255, cv::THRESH_BINARY); bw.convertTo(bw, CV_8U);
        cv::morphologyEx(bw, bw, cv::MORPH_CLOSE, cv::getStructuringElement(cv::MORPH_RECT, { fp.CloseKernelWDiv,fp.CloseKernelH }));

        return bw;

    }

    // ---------- PCA Angle "nguyên tác" ----------
    float BarcodeCore::PCAAngleDeg(const std::vector<cv::Point>& pts)
    {
        CV_Assert(!pts.empty());
        cv::Mat data((int)pts.size(), 2, CV_32F);
        for (int i = 0; i < (int)pts.size(); ++i) {
            data.at<float>(i, 0) = (float)pts[i].x;
            data.at<float>(i, 1) = (float)pts[i].y;
        }
        cv::PCA pca(data, cv::Mat(), cv::PCA::DATA_AS_ROW);
        cv::Point2f ev(pca.eigenvectors.at<float>(0, 0),
            pca.eigenvectors.at<float>(0, 1));
        float ang = std::atan2(ev.y, ev.x) * 180.0f / (float)CV_PI;
        return ang;
    }
    float BarcodeCore::PCAAngleDeg(const std::vector<cv::Point2f>& pts)
    {
        CV_Assert(!pts.empty());
        cv::Mat data((int)pts.size(), 2, CV_32F);
        for (int i = 0; i < (int)pts.size(); ++i) {
            data.at<float>(i, 0) = pts[i].x;
            data.at<float>(i, 1) = pts[i].y;
        }
        cv::PCA pca(data, cv::Mat(), cv::PCA::DATA_AS_ROW);
        cv::Point2f ev(pca.eigenvectors.at<float>(0, 0),
            pca.eigenvectors.at<float>(0, 1));
        float ang = std::atan2(ev.y, ev.x) * 180.0f / (float)CV_PI;
        return ang;
    }

    // ---------- ước lượng & lọc nhiều box 1D trên ảnh gốc ----------
    std::vector<BarcodeCore::RotBox>
        BarcodeCore::DetectBox(const cv::Mat& gray8, const FilterParams& fp)
    {
        cv::Mat mask = MakeMask(gray8, fp);


        cv::Mat lbl, stats, cent;
        int n = cv::connectedComponentsWithStats(mask, lbl, stats, cent, 8, CV_32S);

        std::vector<RotBox> out;
        for (int i = 1; i < n; i++) {
            int x = stats.at<int>(i, cv::CC_STAT_LEFT);
            int y = stats.at<int>(i, cv::CC_STAT_TOP);
            int w = stats.at<int>(i, cv::CC_STAT_WIDTH);
            int h = stats.at<int>(i, cv::CC_STAT_HEIGHT);
            int area = stats.at<int>(i, cv::CC_STAT_AREA);
            if (area < fp.MinArea) continue;

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

    static inline cv::RotatedRect MakeHorizontal(cv::RotatedRect rr)
    {
        if (rr.size.width < rr.size.height && rr.angle <= 90) {
            std::swap(rr.size.width, rr.size.height);
            rr.angle -= 90.f;
        }
        else if (rr.size.width > rr.size.height && rr.angle > 90) {
            std::swap(rr.size.width, rr.size.height);
            rr.angle -= 90.f;
        }
        return rr;
    }



    // ---------- API chính: detect per-box → crop → ZXing ----------
    void BarcodeCore::DetectAll(const cv::Mat& src,
        std::vector<CodeResult>& out,
        const DetectOptions& opt) const
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

        // ZXing hints
        ZXing::DecodeHints hints;
        hints.setTryHarder(true);
        hints.setTryRotate(false);
        hints.setTryDownscale(true);
        hints.setFormats(ZXing::BarcodeFormat::Any);

        DebugViz::Save(D, gray, "00_gray");

        // Nếu không tìm box: chạy ZXing trực tiếp nhiều pass
        if (!opt.findBoxes) {
            ZXing::DecodeHints hTryAll; hTryAll.setTryHarder(true); hTryAll.setTryRotate(true); hTryAll.setTryDownscale(true);  hTryAll.setFormats(ZXing::BarcodeFormat::Any);
            ZXing::DecodeHints hNoDown = hTryAll; hNoDown.setTryDownscale(false);

            auto run_pass = [&](const cv::Mat& g, const ZXing::DecodeHints& zxh, std::vector<CodeResult>& sink)
                {
                    ZXing::ImageView iv(g.data, g.cols, g.rows, ZXing::ImageFormat::Lum, (int)g.step);
                    auto results = ZXing::ReadBarcodes(iv, zxh);
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
            cv::Mat g2; cv::resize(g0, g2, cv::Size(), 0.75, 0.75, cv::INTER_AREA);
            run_pass(g0, hTryAll, tmp);
            if (tmp.empty()) run_pass(g2, hTryAll, tmp);
            if (tmp.empty()) run_pass(g0, hNoDown, tmp);
            out.swap(tmp);
            return;
        }

        // ===== 1D detect per-box (không xoay toàn cục) =====
        const auto& fp = opt.filter;
        auto boxes = DetectBox(gray, fp);

        if (D.save || D.draw) {
            std::vector<cv::RotatedRect> rrs; rrs.reserve(boxes.size());
            for (auto& b : boxes) rrs.push_back(b.rr);
            cv::Mat ov; cv::cvtColor(gray, ov, cv::COLOR_GRAY2BGR);
            DebugViz::DrawRRects(ov, rrs);
            DebugViz::Save(D, ov, "01_boxes_overlay");
        }

        // ===== CROP + ZXing từng ROI =====
        auto MakeHorizontal = [](cv::RotatedRect rr) {
            if (rr.size.width < rr.size.height && rr.angle <= 90) {
                std::swap(rr.size.width, rr.size.height);
                rr.angle -= 90.f;
            }
            else if (rr.size.width > rr.size.height && rr.angle > 90) {
                std::swap(rr.size.width, rr.size.height);
                rr.angle -= 90.f;
            }
            return rr;
            };

        for (auto& b : boxes)
        {
            cv::Mat patch = CropRotatedBox(gray, b.rr, 0.06f);
            // (tùy chọn) patch = TrimHorizBorders(patch);

            ZXing::ImageView iv(patch.data, patch.cols, patch.rows, ZXing::ImageFormat::Lum, (int)patch.step);
            auto zs = ZXing::ReadBarcodes(iv, hints);

            cv::RotatedRect rrSnap = MakeHorizontal(b.rr);
            cv::Point2f v[4]; rrSnap.points(v);
            std::vector<cv::Point2f> poly{ v, v + 4 };
            OrderQuadTLTRBRBL(poly);

            if (!zs.empty()) {
                for (auto& z : zs) {
                    if (!z.isValid()) continue;
                    CodeResult cr;
                    cr.text = z.text();
                    cr.symbology = MapFormat((int)z.format());
                    cr.rrect = rrSnap;
                    cr.corners = poly;
                    cr.score = b.score;
                    out.push_back(std::move(cr));
                }
            }
        }

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