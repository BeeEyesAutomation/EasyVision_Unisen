// DebugDraw.cpp
#include "DebugDraw.h"
#include <cmath>

namespace BeeCpp {

    static inline cv::Point2f perpNormal(const cv::Point2f& v) {
        // v đơn vị từ fitLine -> pháp tuyến đơn vị n = (-vy, vx)
        return cv::Point2f(-v.y, v.x);
    }

    void RenderLineDebug(const cv::Mat& baseGrayOrEdge,
        const std::vector<cv::Point2f>& allSamples,
        const LineResult& r,
        float thresholdPx,
        cv::Mat& outBgr,
        const DebugDrawOptions& opt)
    {
        CV_Assert(!baseGrayOrEdge.empty());
        // 1) Chuẩn bị nền BGR
        if (baseGrayOrEdge.type() == CV_8UC1) {
            cv::cvtColor(baseGrayOrEdge, outBgr, cv::COLOR_GRAY2BGR);
        }
        else if (baseGrayOrEdge.type() == CV_8UC3) {
            outBgr = baseGrayOrEdge.clone();
        }
        else if (baseGrayOrEdge.type() == CV_8UC4) {
            cv::cvtColor(baseGrayOrEdge, outBgr, cv::COLOR_BGRA2BGR);
        }
        else {
            // convert an toàn về 8UC1 rồi lên 3 kênh
            cv::Mat tmp8;
            baseGrayOrEdge.convertTo(tmp8, CV_8U);
            cv::cvtColor(tmp8, outBgr, cv::COLOR_GRAY2BGR);
        }

        if (!r.found) {
            cv::putText(outBgr, "No line found", { 10, 20 }, cv::FONT_HERSHEY_SIMPLEX,
                opt.fontScale, opt.colorText, opt.fontThickness, cv::LINE_AA);
            return;
        }

        // 2) Vẽ inlier/outlier
        if (opt.drawInliers || opt.drawOutliers) {
            // Khoảng cách điểm -> line ax+by+c=0 (từ (vx,vy,x0,y0))
            const cv::Point2f v(r.line[0], r.line[1]);     // đơn vị
            const cv::Point2f p0(r.line[2], r.line[3]);
            // a,b,c từ v & p0:
            double a = v.y;        // vì vector pháp tuyến = (vy, -vx); ax + by + c = 0
            double b = -v.x;
            double c = -(a * p0.x + b * p0.y);
            double invDen = 1.0 / std::sqrt(a * a + b * b);

            for (const auto& q : allSamples) {
                double d = std::abs(a * q.x + b * q.y + c) * invDen;
                bool isIn = d < (double)thresholdPx;
                if (isIn && opt.drawInliers) {
                    cv::circle(outBgr, q, opt.pointRadius, opt.colorInlier, cv::FILLED, cv::LINE_AA);
                }
                else if (!isIn && opt.drawOutliers) {
                    cv::circle(outBgr, q, opt.pointRadius, opt.colorOutlier, cv::FILLED, cv::LINE_AA);
                }
            }
        }

        // 3) Vẽ băng song song ±threshold
        {
            cv::Point2f v(r.line[0], r.line[1]); // đơn vị
            cv::Point2f n = perpNormal(v);       // đơn vị

            // Hai đường song song cách threshold: P1±n*t, P2±n*t
            cv::Point p1((int)std::lround(r.p1.x), (int)std::lround(r.p1.y));
            cv::Point p2((int)std::lround(r.p2.x), (int)std::lround(r.p2.y));

            cv::Point2f offset = n * thresholdPx;
            cv::Point p1p = p1 + cv::Point((int)std::lround(offset.x), (int)std::lround(offset.y));
            cv::Point p2p = p2 + cv::Point((int)std::lround(offset.x), (int)std::lround(offset.y));
            cv::Point p1m = p1 - cv::Point((int)std::lround(offset.x), (int)std::lround(offset.y));
            cv::Point p2m = p2 - cv::Point((int)std::lround(offset.x), (int)std::lround(offset.y));

            cv::line(outBgr, p1p, p2p, opt.colorBand, opt.bandThickness, cv::LINE_AA);
            cv::line(outBgr, p1m, p2m, opt.colorBand, opt.bandThickness, cv::LINE_AA);
        }

        // 4) Vẽ đoạn thẳng chính + endpoints
        cv::line(outBgr,
            cv::Point((int)std::lround(r.p1.x), (int)std::lround(r.p1.y)),
            cv::Point((int)std::lround(r.p2.x), (int)std::lround(r.p2.y)),
            opt.colorLine, opt.lineThickness, cv::LINE_AA);

        cv::circle(outBgr, r.p1, std::max(2, opt.lineThickness + 1), opt.colorP1P2, cv::FILLED, cv::LINE_AA);
        cv::circle(outBgr, r.p2, std::max(2, opt.lineThickness + 1), opt.colorP1P2, cv::FILLED, cv::LINE_AA);

        // 5) Chú thích text
        char buf1[256];
        std::snprintf(buf1, sizeof(buf1),
            "len=%.2f px / %.3f mm | score=%.2f | inliers=%d",
            r.length_px, r.length_mm, r.score, r.inliers);

        cv::putText(outBgr, buf1, { 10, 20 }, cv::FONT_HERSHEY_SIMPLEX,
            opt.fontScale, opt.colorText, opt.fontThickness, cv::LINE_AA);

        char buf2[256];
        std::snprintf(buf2, sizeof(buf2),
            "threshold=%.2f px", thresholdPx);
        cv::putText(outBgr, buf2, { 10, 40 }, cv::FONT_HERSHEY_SIMPLEX,
            opt.fontScale, opt.colorText, opt.fontThickness, cv::LINE_AA);
    }

} // namespace BeeCpp
