#include "RansacLineCli.h"
#include <DebugDraw.h>

using namespace BeeCpp;


static void ApplyCliOpts(const DebugDrawOptionsCli^ in, BeeCpp::DebugDrawOptions& out) {
    if (!in) return;
    out.lineThickness = in->LineThickness;
    out.drawInliers = in->DrawInliers;
    out.drawOutliers = in->DrawOutliers;
    out.pointRadius = in->PointRadius;
    out.bandThickness = in->BandThickness;
    out.fontScale = in->FontScale;
    out.fontThickness = in->FontThickness;
}
Line2DCli RansacLine::FindBestLine(
    IntPtr edgeData, int width, int height, int stride,
    int iterations, float threshold, int maxPoints, int seed,
    float mmPerPixel)
{
    Line2DCli ret{};
    ret.Found = false;

    if (edgeData == IntPtr::Zero || width <= 1 || height <= 1 || stride < width)
        return ret;

    cv::Mat edges(height, width, CV_8UC1, edgeData.ToPointer(), (size_t)stride);

    auto res = RansacLineCore::FindBestLine(
        edges, iterations, threshold, maxPoints, (unsigned)seed, mmPerPixel);

    if (!res.found) return ret;

    ret.Found = true;

    ret.X1 = res.p1.x; ret.Y1 = res.p1.y;
    ret.X2 = res.p2.x; ret.Y2 = res.p2.y;

    ret.Vx = res.line[0]; ret.Vy = res.line[1];
    ret.X0 = res.line[2]; ret.Y0 = res.line[3];

    ret.Inliers = res.inliers;
    ret.Score = res.score;
    ret.LengthPx = res.length_px;
    ret.LengthMm = res.length_mm;

    return ret;
}


Line2DCli RansacLine::FindBestLineAndDebug(
    IntPtr edgeData, int width, int height, int stride,
    int iterations, float threshold, int maxPoints, int seed, float mmPerPixel,
    IntPtr dbgBaseData, int dbgBaseType, size_t dbgBaseStep,
    IntPtr dbgOutData, int dbgOutType, size_t dbgOutStep,
    DebugDrawOptionsCli^ opts,
    System:: String^ savePath)
{
    Line2DCli ret{}; ret.Found = false;

    if (edgeData == IntPtr::Zero) return ret;
    if (dbgBaseData == IntPtr::Zero || dbgOutData == IntPtr::Zero) return ret;
    if (width <= 1 || height <= 1) return ret;
    if (stride < width) return ret;

    cv::Mat edges(height, width, CV_8UC1, edgeData.ToPointer(), (size_t)stride);
    // base có thể là 8UC1/8UC3/8UC4
    cv::Mat base(height, width, dbgBaseType, dbgBaseData.ToPointer(), (size_t)dbgBaseStep);
    // out BGR phải là 8UC3
    if (dbgOutType != CV_8UC3) return ret;
    cv::Mat dbgOut(height, width, CV_8UC3, dbgOutData.ToPointer(), (size_t)dbgOutStep);

    // 1) Tìm line
    auto res = RansacLineCore::FindBestLine(edges, iterations, threshold, maxPoints,
        (unsigned)seed, mmPerPixel);
    if (!res.found) {
        // vẫn copy base → out để nhìn nền
        if (base.type() == CV_8UC1) cv::cvtColor(base, dbgOut, cv::COLOR_GRAY2BGR);
        else if (base.type() == CV_8UC3) base.copyTo(dbgOut);
        else if (base.type() == CV_8UC4) cv::cvtColor(base, dbgOut, cv::COLOR_BGRA2BGR);
        else { cv::Mat tmp8; base.convertTo(tmp8, CV_8U); cv::cvtColor(tmp8, dbgOut, cv::COLOR_GRAY2BGR); }

        // ghi chú “No line”
        cv::putText(dbgOut, "No line found", { 10, 20 }, cv::FONT_HERSHEY_SIMPLEX,
            0.5, cv::Scalar(255, 255, 255), 1, cv::LINE_AA);
    }
    else {
        // 2) Lấy lại tập sample đã dùng (downsample) để vẽ inlier/outlier
        // => ta cần tái tạo đúng sample như core: lấy non-zero & downsample cùng logic
        std::vector<cv::Point> nz;
        cv::findNonZero(edges, nz);
        std::vector<cv::Point2f> pts;
        pts.reserve(std::min((int)nz.size(), maxPoints));
        if ((int)nz.size() > maxPoints) {
            int step = (int)nz.size() / maxPoints;
            if (step < 1) step = 1;
            for (int i = 0; i < (int)nz.size(); i += step) {
                const auto& p = nz[i];
                pts.emplace_back((float)p.x, (float)p.y);
            }
        }
        else {
            for (const auto& p : nz) pts.emplace_back((float)p.x, (float)p.y);
        }

        // 3) Vẽ
        BeeCpp::DebugDrawOptions dopt;
        ApplyCliOpts(opts, dopt);

        cv::Mat outBgr;
        RenderLineDebug(base, pts, res, threshold, outBgr, dopt);

        // copy ra buffer đích
        if (outBgr.size() != dbgOut.size()) {
            cv::resize(outBgr, outBgr, dbgOut.size(), 0, 0, cv::INTER_NEAREST);
        }
        if (outBgr.type() != dbgOut.type()) {
            outBgr.convertTo(dbgOut, dbgOut.type());
        }
        else {
            outBgr.copyTo(dbgOut);
        }
    }

    // 4) Lưu file nếu có đường dẫn
    if (savePath != nullptr && savePath->Length > 0) {
      
        // đảm bảo cv::imwrite ảnh BGR
        cv::imwrite("Edge.png", dbgOut);
    }

    // 5) Trả thông số
    ret.Found = res.found;
    ret.X1 = res.p1.x; ret.Y1 = res.p1.y;
    ret.X2 = res.p2.x; ret.Y2 = res.p2.y;
    ret.Vx = res.line[0]; ret.Vy = res.line[1];
    ret.X0 = res.line[2]; ret.Y0 = res.line[3];
    ret.Inliers = res.inliers;
    ret.Score = res.score;
    ret.LengthPx = res.length_px;
    ret.LengthMm = res.length_mm;
    return ret;
}