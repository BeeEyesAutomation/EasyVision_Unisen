#include "ThreadPitchCore.h"
#include <algorithm>
#include <numeric>
#include <stdexcept>
#include <cmath>

using namespace cv;
using namespace std;

namespace BeeCpp {

    static cv::Mat g_input; // BGR8

    // ---- helpers ----
    static std::vector<int> findPeaks(const std::vector<double>& s, int minDist, double minProm) {
        const int N = (int)s.size();
        std::vector<int> peaks;
        for (int i = 1; i < N - 1; ++i) {
            if (s[i] > s[i - 1] && s[i] > s[i + 1]) {
                int l = i; while (l > 0 && s[l] > s[l - 1]) --l;
                int r = i; while (r<N - 1 && s[r] > s[r + 1]) ++r;
                double leftMin = *std::min_element(s.begin() + l, s.begin() + i);
                double rightMin = *std::min_element(s.begin() + i + 1, s.begin() + r + 1);
                double prom = s[i] - std::max(leftMin, rightMin);
                if (prom >= minProm) {
                    if (!peaks.empty() && i - peaks.back() < minDist) {
                        if (s[i] > s[peaks.back()]) peaks.back() = i;
                    }
                    else peaks.push_back(i);
                }
            }
        }
        return peaks;
    }

    static void detrendAndNormalize(std::vector<double>& s, int blurWin = 41) {
        const int N = (int)s.size();
        if (N <= 2) return;
        blurWin = std::max(3, blurWin | 1);
        Mat m(N, 1, CV_64F), mb;
        for (int i = 0; i < N; ++i) m.at<double>(i, 0) = s[i];
        blur(m, mb, Size(1, blurWin));
        Mat d = m - mb;
        Scalar meanV, stdV; meanStdDev(d, meanV, stdV);
        double sd = std::max(1e-6, stdV[0]);
        for (int i = 0; i < N; ++i) s[i] = d.at<double>(i, 0) / sd;
    }

    static double medianOf(std::vector<double> v) {
        if (v.empty()) return 0.0;
        size_t n = v.size();
        std::nth_element(v.begin(), v.begin() + n / 2, v.end());
        double m = v[n / 2];
        if ((n & 1) == 0) {
            auto it = std::max_element(v.begin(), v.begin() + n / 2);
            m = (m + *it) * 0.5;
        }
        return m;
    }

    static Mat rotateKeep(const Mat& src, double angleDeg) {
        Point2f c(src.cols / 2.f, src.rows / 2.f);
        Mat R = getRotationMatrix2D(c, angleDeg, 1.0);
        Rect bb = RotatedRect(c, src.size(), angleDeg).boundingRect();
        R.at<double>(0, 2) += bb.width / 2.0 - c.x;
        R.at<double>(1, 2) += bb.height / 2.0 - c.y;
        Mat dst; warpAffine(src, dst, R, bb.size(), INTER_LINEAR, BORDER_REPLICATE);
        return dst;
    }

    static double estimateAxisAngleDeg(const Mat& gray) {
        Mat gx, gy, mag;
        Sobel(gray, gx, CV_32F, 1, 0, 3);
        Sobel(gray, gy, CV_32F, 0, 1, 3);
        magnitude(gx, gy, mag);
        double infN = norm(mag, NORM_INF);
        Mat m8; mag.convertTo(m8, CV_8U, 255.0 / (1e-6 + infN));
        Mat edges; Canny(m8, edges, 50, 150);

        std::vector<Vec2f> lines;
        HoughLines(edges, lines, 1, (float)(CV_PI / 180.0), std::max(80, edges.rows / 4));
        if (lines.empty()) return 0.0;

        double bestTheta = 0; int bestCnt = 0;
        for (int b = 0; b < 180; ++b) {
            double t0 = b * CV_PI / 180.0, t1 = (b + 1) * CV_PI / 180.0; int cnt = 0;
            for (auto& L : lines) if (L[1] >= t0 && L[1] < t1) cnt++;
            if (cnt > bestCnt) { bestCnt = cnt; bestTheta = (t0 + t1) / 2.0; }
        }
        double lineAngle = bestTheta - CV_PI / 2.0;
        return -(lineAngle * 180.0 / CV_PI);
    }
    static void DrawPitchOverlay(cv::Mat& dbg,
        const std::vector<int>& peaks,
        const cv::Rect& band,
        const BeeCpp::PitchResult& S)
    {
        if (dbg.empty()) return;

        // --- Vẽ band ---
        cv::rectangle(dbg, band, cv::Scalar(255, 0, 0), 1, cv::LINE_AA);
        int xC = band.x + band.width / 2;        // trục giữa band để vẽ mốc
        int xTxt = std::min(dbg.cols - 180, band.x + band.width + 8);

        // --- Đỉnh ren (crest) và valley + từng pitch ---
        for (size_t i = 0; i < peaks.size(); ++i)
        {
            int y = peaks[i];
            // chấm tròn & index đỉnh
            cv::circle(dbg, cv::Point(xC, y), 3, cv::Scalar(0, 255, 0), -1, cv::LINE_AA);
            cv::putText(dbg, cv::format("%zu", i),
                cv::Point(xC + 6, y - 4), cv::FONT_HERSHEY_SIMPLEX, 0.45,
                cv::Scalar(0, 255, 0), 1, cv::LINE_AA);

            if (i + 1 < peaks.size())
            {
                int y2 = peaks[i + 1];
                int ym = (y + y2) / 2;

                // double-arrow biểu diễn pitch giữa 2 đỉnh
                cv::arrowedLine(dbg, cv::Point(xC, y), cv::Point(xC, ym),
                    cv::Scalar(0, 200, 255), 1, cv::LINE_AA, 0, 0.35);
                cv::arrowedLine(dbg, cv::Point(xC, y2), cv::Point(xC, ym),
                    cv::Scalar(0, 200, 255), 1, cv::LINE_AA, 0, 0.35);

                // nhãn giá trị pitch
                cv::putText(dbg, cv::format("%.1f px", (double)(y2 - y)),
                    cv::Point(xC + 8, ym), cv::FONT_HERSHEY_SIMPLEX, 0.45,
                    cv::Scalar(0, 200, 255), 1, cv::LINE_AA);

                // valley (chính giữa 2 đỉnh)
                cv::circle(dbg, cv::Point(xC, ym), 2, cv::Scalar(255, 200, 0), -1, cv::LINE_AA);
            }
        }

        // --- Tổng chiều cao đoạn ren (từ đỉnh đầu đến đỉnh cuối) ---
        if (peaks.size() >= 2)
        {
            int y0 = peaks.front();
            int y1 = peaks.back();
            int xS = band.x + std::max(4, band.width / 6);

            cv::arrowedLine(dbg, cv::Point(xS, y0), cv::Point(xS, (y0 + y1) / 2),
                cv::Scalar(255, 0, 255), 1, cv::LINE_AA, 0, 0.35);
            cv::arrowedLine(dbg, cv::Point(xS, y1), cv::Point(xS, (y0 + y1) / 2),
                cv::Scalar(255, 0, 255), 1, cv::LINE_AA, 0, 0.35);

            cv::putText(dbg,
                (S.segmentHeightMm > 0)
                ? cv::format("H=%.1f px (%.3f mm)", S.segmentHeightPx, S.segmentHeightMm)
                : cv::format("H=%.1f px", S.segmentHeightPx),
                cv::Point(xS + 6, (y0 + y1) / 2), cv::FONT_HERSHEY_SIMPLEX, 0.48,
                cv::Scalar(255, 0, 255), 1, cv::LINE_AA);
        }

        // --- Bảng thống kê góc/đếm ren/pitch ---
        cv::Rect info(std::max(8, xTxt), 8, 360, (S.meanPitchMm > 0 ? 118 : 98));
        info.width = std::min(info.width, dbg.cols - info.x - 8);
        info.height = std::min(info.height, dbg.rows - info.y - 8);

        if (info.width > 20 && info.height > 20)
        {
            cv::Mat roi = dbg(info);
            cv::Mat ov; roi.copyTo(ov);
            cv::rectangle(ov, cv::Rect(0, 0, info.width, info.height), cv::Scalar(0, 0, 0), -1);
            cv::addWeighted(ov, 0.45, roi, 0.55, 0, roi);

            int y = info.y + 22;
            auto put = [&](const std::string& s, cv::Scalar col = cv::Scalar(255, 255, 255)) {
                cv::putText(dbg, s, cv::Point(info.x + 10, y), cv::FONT_HERSHEY_SIMPLEX, 0.55, col, 1, cv::LINE_AA);
                y += 20;
                };

            put("THREAD PITCH", cv::Scalar(0, 255, 255));
            put(cv::format("Count: %d   Angle: %.2f deg", S.threadCount, S.angleDeg));
            put(cv::format("Mean: %.2f px   Std: %.2f", S.meanPitchPx, S.stdPitchPx));
            put(cv::format("Med: %.2f px   Min/Max: %.2f / %.2f",
                S.medianPitchPx, S.minPitchPx, S.maxPitchPx));
            if (S.meanPitchMm > 0.0)
                put(cv::format("Mean: %.3f mm   H: %.3f mm", S.meanPitchMm, S.segmentHeightMm));
            else
                put(cv::format("H: %.1f px", S.segmentHeightPx));
        }
    }
    static void DrawPitchOverlayAxis(cv::Mat& dbg,
        const std::vector<int>& peaks,
        const cv::Rect& band,
        const BeeCpp::PitchResult& S,
        BeeCpp::Axis axis)
    {
        cv::rectangle(dbg, band, cv::Scalar(255, 0, 0), 1, cv::LINE_AA);

        if (axis == BeeCpp::Axis::Vertical) {
            int xC = band.x + band.width / 2;
            for (size_t i = 0; i < peaks.size(); ++i) {
                int y = peaks[i];
                cv::circle(dbg, { xC,y }, 3, { 0,255,0 }, -1, cv::LINE_AA);
                if (i + 1 < peaks.size()) {
                    int y2 = peaks[i + 1], ym = (y + y2) / 2;
                    cv::arrowedLine(dbg, { xC,y }, { xC,ym }, { 0,200,255 }, 1, cv::LINE_AA, 0, 0.35);
                    cv::arrowedLine(dbg, { xC,y2 }, { xC,ym }, { 0,200,255 }, 1, cv::LINE_AA, 0, 0.35);
                    cv::putText(dbg, cv::format("%.1f px", (double)(y2 - y)),
                        { xC + 8, ym }, cv::FONT_HERSHEY_SIMPLEX, 0.45, { 0,200,255 }, 1, cv::LINE_AA);
                }
            }
        }
        else {
            int yC = band.y + band.height / 2;
            for (size_t i = 0; i < peaks.size(); ++i) {
                int x = peaks[i];
                cv::circle(dbg, { x,yC }, 3, { 0,255,0 }, -1, cv::LINE_AA);
                if (i + 1 < peaks.size()) {
                    int x2 = peaks[i + 1], xm = (x + x2) / 2;
                    cv::arrowedLine(dbg, { x,yC }, { xm,yC }, { 0,200,255 }, 1, cv::LINE_AA, 0, 0.35);
                    cv::arrowedLine(dbg, { x2,yC }, { xm,yC }, { 0,200,255 }, 1, cv::LINE_AA, 0, 0.35);
                    cv::putText(dbg, cv::format("%.1f px", (double)(x2 - x)),
                        { xm, yC - 6 }, cv::FONT_HERSHEY_SIMPLEX, 0.45, { 0,200,255 }, 1, cv::LINE_AA);
                }
            }
        }
    }

    // axis: BeeCpp::Axis::Vertical  -> đo theo Y (mặc định cũ)
 //       BeeCpp::Axis::Horizontal-> đo theo X
    static BeeCpp::PitchResult measureThreadPitch_internal(const cv::Mat& bgr,
        double mm_per_px,
        int bandHalfWidth,
        int expectedMinPitchPx,
        bool useGabor,
        BeeCpp::Axis axis)
    {
        using namespace cv;
        using namespace std;

        BeeCpp::PitchResult R;
        CV_Assert(!bgr.empty());

        // --------- Gray + CLAHE ----------
        Mat gray;
        if (bgr.channels() == 3) cvtColor(bgr, gray, COLOR_BGR2GRAY);
        else if (bgr.channels() == 1) gray = bgr;
        else throw std::runtime_error("Unsupported channel count");

        Ptr<CLAHE> clahe = createCLAHE(3.0, Size(8, 8));
        clahe->apply(gray, gray);

        // --------- (Optional) Gabor để tăng tương phản vân tuần hoàn ----------
        if (useGabor) {
            // lambda ~ pitch ước lượng (px). Nếu chưa biết, dùng expectedMinPitchPx*1.2 như ước lượng "mềm".
            double lambda = std::max(6, expectedMinPitchPx) * 1.2;
            double sigma = 3.0, gamma = 0.5, psi = 0.0, theta = 0.0; // hướng 0 rad (ngang)
            Mat g = getGaborKernel(Size(21, 21), sigma, theta, lambda, gamma, psi, CV_32F);
            Mat fg; filter2D(gray, fg, CV_8U, g);
            max(gray, fg, gray);
        }

        // --------- Ước lượng góc trục & xoay ảnh về hệ "thẳng" ----------
        R.angleDeg = estimateAxisAngleDeg(gray);
        Mat rot = rotateKeep(gray, R.angleDeg);

        // --------- Chọn band & tạo profile 1D theo trục ----------
        Rect band;
        vector<double> s;

        if (axis == BeeCpp::Axis::Vertical) {
            // đo theo Y: chọn dải cột nơi biến thiên theo Y lớn
            Mat gy; Sobel(rot, gy, CV_32F, 0, 1, 3);
            Mat varX; reduce(gy.mul(gy), varX, 0, REDUCE_AVG);
            double mn, mx; Point pmin, pmax; minMaxLoc(varX, &mn, &mx, &pmin, &pmax);
            int xc = pmax.x;
            int x0 = std::max(0, xc - bandHalfWidth);
            int x1 = std::min(rot.cols - 1, xc + bandHalfWidth);
            band = Rect(x0, 0, x1 - x0 + 1, rot.rows);

            Mat roi = rot(band), prof;
            reduce(roi, prof, 1, REDUCE_AVG, CV_64F);      // N(rows) x 1
            s.resize(prof.rows);
            for (int i = 0; i < prof.rows; ++i) s[i] = prof.at<double>(i, 0);
        }
        else {
            // đo theo X: chọn dải hàng nơi biến thiên theo X lớn
            Mat gx; Sobel(rot, gx, CV_32F, 1, 0, 3);
            Mat varY; reduce(gx.mul(gx), varY, 1, REDUCE_AVG);
            double mn, mx; Point pmin, pmax; minMaxLoc(varY, &mn, &mx, &pmin, &pmax);
            int yc = pmax.y;
            int y0 = std::max(0, yc - bandHalfWidth);
            int y1 = std::min(rot.rows - 1, yc + bandHalfWidth);
            band = Rect(0, y0, rot.cols, y1 - y0 + 1);

            Mat roi = rot(band), prof;
            reduce(roi, prof, 0, REDUCE_AVG, CV_64F);      // 1 x N(cols)
            s.resize(prof.cols);
            for (int i = 0; i < prof.cols; ++i) s[i] = prof.at<double>(0, i);
        }

        // Invert nếu rãnh tối/đỉnh sáng
        {
            Scalar m = mean(rot(band));
            if (m[0] < 128) for (double& v : s) v = 255.0 - v;
        }

        // --------- Khử nền + mượt + tìm đỉnh ----------
        detrendAndNormalize(s, 41);
        {
            Mat sm((int)s.size(), 1, CV_64F);
            for (int i = 0; i < sm.rows; ++i) sm.at<double>(i, 0) = s[i];
            GaussianBlur(sm, sm, Size(1, 9), 0);
            for (int i = 0; i < sm.rows; ++i) s[i] = sm.at<double>(i, 0);
        }

        const int minDist = std::max(3, expectedMinPitchPx / 2);
        std::vector<int> peaks = findPeaks(s, minDist, /*minProm*/0.6);
        R.crestY = peaks;                              // (tên giữ nguyên, chứa x nếu đo Horizontal)
        R.threadCount = (peaks.size() >= 2) ? (int)peaks.size() - 1 : 0;

        // --------- Thống kê pitch & đoạn ren ----------
        if (peaks.size() >= 2) {
            std::vector<double> d; d.reserve(peaks.size() - 1);
            for (size_t i = 1; i < peaks.size(); ++i) d.push_back((double)(peaks[i] - peaks[i - 1]));

            R.meanPitchPx = std::accumulate(d.begin(), d.end(), 0.0) / d.size();
            double sq = 0; for (double v : d) sq += (v - R.meanPitchPx) * (v - R.meanPitchPx);
            R.stdPitchPx = std::sqrt(sq / std::max<size_t>(1, d.size() - 1));
            auto mm = std::minmax_element(d.begin(), d.end());
            R.minPitchPx = *mm.first;
            R.maxPitchPx = *mm.second;

            // median
            {
                std::vector<double> tmp = d;
                size_t n = tmp.size();
                std::nth_element(tmp.begin(), tmp.begin() + n / 2, tmp.end());
                R.medianPitchPx = (n & 1) ? tmp[n / 2]
                    : (tmp[n / 2] + *std::max_element(tmp.begin(), tmp.begin() + n / 2)) * 0.5;
            }

            // chiều dài đoạn ren (dọc hoặc ngang theo trục)
            double seg = (double)(peaks.back() - peaks.front());
            R.segmentHeightPx = seg;
            if (axis == BeeCpp::Axis::Vertical) {
                R.segmentTopY = peaks.front();
                R.segmentBottomY = peaks.back();
            }
            else {
                // để tương thích struct hiện tại, gán “y” bằng tâm band
                int yC = band.y + band.height / 2;
                R.segmentTopY = yC;   // không dùng khi xem Horizontal, nhưng giữ giá trị hợp lý
                R.segmentBottomY = yC;
            }

            if (mm_per_px > 0) {
                R.meanPitchMm = R.meanPitchPx * mm_per_px;
                R.medianPitchMm = R.medianPitchPx * mm_per_px;
                R.minPitchMm = R.minPitchPx * mm_per_px;
                R.maxPitchMm = R.maxPitchPx * mm_per_px;
                R.segmentHeightMm = R.segmentHeightPx * mm_per_px;
            }
        }

        // --------- Ảnh debug & overlay ----------
        cvtColor(rot, R.debugBGR, COLOR_GRAY2BGR);

        // Vẽ overlay chi tiết theo trục
        auto DrawPitchOverlayAxis = [](cv::Mat& dbg,
            const std::vector<int>& pk,
            const cv::Rect& b,
            const BeeCpp::PitchResult& S,
            BeeCpp::Axis ax)
            {
                rectangle(dbg, b, Scalar(255, 0, 0), 1, LINE_AA);

                if (pk.empty()) return;

                if (ax == BeeCpp::Axis::Vertical) {
                    int xC = b.x + b.width / 2;
                    for (size_t i = 0; i < pk.size(); ++i) {
                        int y = pk[i];
                        circle(dbg, { xC,y }, 3, { 0,255,0 }, -1, LINE_AA);
                        putText(dbg, format("%zu", i), { xC + 6, y - 4 }, FONT_HERSHEY_SIMPLEX, 0.45, { 0,255,0 }, 1, LINE_AA);
                        if (i + 1 < pk.size()) {
                            int y2 = pk[i + 1], ym = (y + y2) / 2;
                            arrowedLine(dbg, { xC,y }, { xC,ym }, { 0,200,255 }, 1, LINE_AA, 0, 0.35);
                            arrowedLine(dbg, { xC,y2 }, { xC,ym }, { 0,200,255 }, 1, LINE_AA, 0, 0.35);
                            putText(dbg, format("%.1f px", (double)(y2 - y)), { xC + 8, ym }, FONT_HERSHEY_SIMPLEX, 0.45, { 0,200,255 }, 1, LINE_AA);
                        }
                    }
                }
                else {
                    int yC = b.y + b.height / 2;
                    for (size_t i = 0; i < pk.size(); ++i) {
                        int x = pk[i];
                        circle(dbg, { x,yC }, 3, { 0,255,0 }, -1, LINE_AA);
                        putText(dbg, format("%zu", i), { x + 4, yC - 6 }, FONT_HERSHEY_SIMPLEX, 0.45, { 0,255,0 }, 1, LINE_AA);
                        if (i + 1 < pk.size()) {
                            int x2 = pk[i + 1], xm = (x + x2) / 2;
                            arrowedLine(dbg, { x,yC }, { xm,yC }, { 0,200,255 }, 1, LINE_AA, 0, 0.35);
                            arrowedLine(dbg, { x2,yC }, { xm,yC }, { 0,200,255 }, 1, LINE_AA, 0, 0.35);
                            putText(dbg, format("%.1f px", (double)(x2 - x)), { xm - 10, yC - 10 }, FONT_HERSHEY_SIMPLEX, 0.45, { 0,200,255 }, 1, LINE_AA);
                        }
                    }
                }

                // Bảng thống kê
                int xInfo = std::min(dbg.cols - 260, b.x + b.width + 8);
                Rect info(std::max(8, xInfo), 8, 250, (S.meanPitchMm > 0 ? 120 : 100));
                info.width = std::min(info.width, dbg.cols - info.x - 8);
                info.height = std::min(info.height, dbg.rows - info.y - 8);
                if (info.width < 60 || info.height < 60) return;

                Mat roi = dbg(info), ov; roi.copyTo(ov);
                rectangle(ov, Rect(0, 0, info.width, info.height), Scalar(0, 0, 0), -1);
                addWeighted(ov, 0.45, roi, 0.55, 0, roi);

                int yy = info.y + 22;
                auto put = [&](const std::string& s, Scalar col = Scalar(255, 255, 255)) {
                    putText(dbg, s, { info.x + 8, yy }, FONT_HERSHEY_SIMPLEX, 0.55, col, 1, LINE_AA);
                    yy += 20;
                    };
                put("THREAD PITCH", Scalar(0, 255, 255));
                put(format("Count: %d   Angle: %.2f deg", S.threadCount, S.angleDeg));
                put(format("Mean: %.2f px   Std: %.2f", S.meanPitchPx, S.stdPitchPx));
                put(format("Med: %.2f px   Min/Max: %.2f/%.2f", S.medianPitchPx, S.minPitchPx, S.maxPitchPx));
                if (S.meanPitchMm > 0.0)
                    put(format("Mean: %.3f mm   H=%.3f mm", S.meanPitchMm, S.segmentHeightMm));
                else
                    put(format("H=%.1f px", S.segmentHeightPx));
            };

        DrawPitchOverlayAxis(R.debugBGR, peaks, band, R, axis);
        cv::imwrite("Result.png", R.debugBGR);
        return R;
    }

    // ---- API global input ----
    void SetInputImage(const cv::Mat& src) {
        if (src.empty()) throw std::runtime_error("Input image is empty");
        if (src.channels() == 3) g_input = src.clone();
        else if (src.channels() == 1) cvtColor(src, g_input, COLOR_GRAY2BGR);
        else throw std::runtime_error("Unsupported channel count");
        cv::imwrite("temp.png", g_input);
    }

    PitchResult MeasureThreadPitch(double mm_per_px,
        int bandHalfWidth,
        int expectedMinPitchPx,
        bool useGabor)
    {
        if (g_input.empty()) throw std::runtime_error("No input image set. Call SetInputImage first.");
        return measureThreadPitch_internal(g_input, mm_per_px, bandHalfWidth, expectedMinPitchPx, useGabor, BeeCpp::Axis::Horizontal);
    }

} // namespace BeeCpp
