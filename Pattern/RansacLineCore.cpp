#include "RansacLineCore.h"
#include <thread>
#include <mutex>
#include <limits>
#include <cmath>
#include <algorithm>
#include <cfloat>

using namespace cv;
using std::vector;
using std::pair;

namespace BeeCpp {
    static inline float ComputeScanPriority(
        double a, double b, double c,
        int width, int height,
        LineScanPriority mode)
    {
        constexpr double eps = 1e-9;

        switch (mode)
        {
        case LineScanPriority::LeftToRight:
            if (std::abs(a) < eps) return 1e9f;
            // x tại y = mid
            return (float)((-b * (height * 0.5) - c) / a);

        case LineScanPriority::RightToLeft:
            if (std::abs(a) < eps) return 1e9f;
            return (float)-((-b * (height * 0.5) - c) / a);

        case LineScanPriority::TopToBottom:
            if (std::abs(b) < eps) return 1e9f;
            return (float)((-a * (width * 0.5) - c) / b);

        case LineScanPriority::BottomToTop:
            if (std::abs(b) < eps) return 1e9f;
            return (float)-((-a * (width * 0.5) - c) / b);

        default:
            return 0.f;
        }
    }

    vector<pair<int, int>> RansacLineCore::PrecomputePairs(int n, int iterations, uint32_t seed) {
        vector<pair<int, int>> pairs;
        pairs.reserve(iterations);
        uint32_t s = seed ? seed : 1u;
        for (int i = 0; i < iterations; ++i) {
            int a = (int)NextIdx(s, n);
            int b; do { b = (int)NextIdx(s, n); } while (b == a);
            pairs.emplace_back(a, b);
        }
        return pairs;
    }
     bool RansacLineCore::CheckDirection(
        double dx, double dy,
         LineDirNative mode,
        float angleCenterDeg,
        float tolDeg
    )
    {
        if (mode == LineDirNative::Any)
            return true;

        double ang = std::atan2(dy, dx) * 180.0 / CV_PI;
        if (ang < 0) ang += 180.0;   // quy về [0..180)

        if (mode == LineDirNative::Horizontal)
        {
            return (ang <= tolDeg || std::abs(ang - 180.0) <= tolDeg);
        }
        if (mode == LineDirNative::Vertical)
        {
            return std::abs(ang - 90.0) <= tolDeg;
        }
        if (mode == LineDirNative::AngleRange)
        {
            return std::abs(ang - angleCenterDeg) <= tolDeg;
        }
        return true;
    }
     //LineResult RansacLineCore::FindBestLine(
     //    const Mat& edges8u1,
     //    int iterations,
     //    float threshold,
     //    int maxPoints,
     //    unsigned seed,
     //    float mmPerPixel,
     //    LineDirNative dirMode,
     //    float angleCenterDeg,
     //    float angleToleranceDeg
     //)
     //{
     //    LineResult out;

     //    if (edges8u1.empty() || edges8u1.type() != CV_8UC1 || iterations <= 0)
     //        return out;

     //    // 1) Lấy non-zero
     //    std::vector<cv::Point> nz;
     //    cv::findNonZero(edges8u1, nz);
     //    if (nz.empty()) return out;

     //    // 2) Downsample
     //    std::vector<cv::Point2f> pts;
     //    pts.reserve(std::min((int)nz.size(), maxPoints));

     //    if ((int)nz.size() > maxPoints)
     //    {
     //        int step = (int)nz.size() / maxPoints;
     //        if (step < 1) step = 1;
     //        for (int i = 0; i < (int)nz.size(); i += step)
     //        {
     //            const auto& p = nz[i];
     //            pts.emplace_back((float)p.x, (float)p.y);
     //        }
     //    }
     //    else
     //    {
     //        for (const auto& p : nz)
     //            pts.emplace_back((float)p.x, (float)p.y);
     //    }

     //    if ((int)pts.size() < 2) return out;

     //    // 3) Precompute cặp chỉ số
     //    auto pairs = PrecomputePairs((int)pts.size(), iterations, seed ? seed : 987654321u);

     //    // 4) RANSAC đa luồng
     //    const int T = (int)std::thread::hardware_concurrency();
     //    const int numThreads = std::max(1, T);

     //    std::vector<std::thread> threads;
     //    std::mutex mtx;

     //    // ===== GLOBAL BEST theo "chất lượng" =====
     //    int    bestCount = -1;
     //    float  bestSegLen = -1.f;
     //    double bestMeanDist = 1e100;   // càng nhỏ càng tốt
     //    double bestDensity = -1.0;    // cnt / segLen, càng lớn càng tốt

     //    double best_a = 0., best_b = 0., best_c = 0.;
     //    std::vector<cv::Point2f> bestInliers;

     //    // capture an toàn (tránh vấn đề CLI)
     //    const LineDirNative dir = dirMode;
     //    const float centerDeg = angleCenterDeg;
     //    const float tolDeg = angleToleranceDeg;
     //    const double thr = (double)threshold;

     //    auto worker = [&](int beginIt, int endIt)
     //        {
     //            // ===== LOCAL BEST theo "chất lượng" =====
     //            int    l_count = -1;
     //            float  l_segLen = -1.f;
     //            double l_meanDist = 1e100;
     //            double l_density = -1.0;

     //            double l_a = 0., l_b = 0., l_c = 0.;
     //            std::vector<cv::Point2f> l_inliers;

     //            for (int it = beginIt; it < endIt; ++it)
     //            {
     //                const auto& pr = pairs[it];
     //                int i1 = pr.first, i2 = pr.second;

     //                const cv::Point2f& p1 = pts[i1];
     //                const cv::Point2f& p2 = pts[i2];

     //                double dx = (double)p2.x - (double)p1.x;
     //                double dy = (double)p2.y - (double)p1.y;

     //                double segLen = std::sqrt(dx * dx + dy * dy);
     //                if (segLen < 1e-6) continue;

     //                // CHECK HƯỚNG
     //                if (!CheckDirection(dx, dy, dir, centerDeg, tolDeg))
     //                    continue;

     //                // ax + by + c = 0
     //                double a = (double)p2.y - (double)p1.y;
     //                double b = (double)p1.x - (double)p2.x;
     //                double norm = std::sqrt(a * a + b * b);
     //                if (norm < 1e-6) continue;
     //                double c = -(a * (double)p1.x + b * (double)p1.y);

     //                const double invNorm = 1.0 / norm;

     //                int cnt = 0;
     //                double sumDist = 0.0;

     //                std::vector<cv::Point2f> currInliers;
     //                currInliers.reserve(256);

     //                for (const auto& q : pts)
     //                {
     //                    double d = std::abs(a * (double)q.x + b * (double)q.y + c) * invNorm;
     //                    if (d < thr)
     //                    {
     //                        ++cnt;
     //                        sumDist += d;
     //                        currInliers.push_back(q);
     //                    }
     //                }
     //                if (cnt < 2) continue;

     //                double meanDist = sumDist / (double)cnt;
     //                double density = (double)cnt / (segLen + 1e-6); // <<< quan trọng: "đậm"

     //                // ===== TIÊU CHÍ CHỌN BEST (INLIERS TỐT NHẤT) =====
     //                // 1) density lớn hơn
     //                // 2) nếu density gần bằng -> meanDist nhỏ hơn (thẳng hơn)
     //                // 3) nếu vẫn bằng -> cnt lớn hơn
     //                // 4) cuối cùng mới segLen (backup)
     //                bool better = false;

     //                if (density > l_density) better = true;
     //                else if (std::abs(density - l_density) <= 1e-12)
     //                {
     //                    if (meanDist < l_meanDist) better = true;
     //                    else if (std::abs(meanDist - l_meanDist) <= 1e-12)
     //                    {
     //                        if (cnt > l_count) better = true;
     //                        else if (cnt == l_count)
     //                        {
     //                            if (segLen > (double)l_segLen) better = true;
     //                            else if (std::abs(segLen - (double)l_segLen) <= 1e-12)
     //                            {
     //                                // tie-break ổn định
     //                                if (a < l_a) better = true;
     //                                else if (a == l_a)
     //                                {
     //                                    if (b < l_b) better = true;
     //                                    else if (b == l_b && c < l_c) better = true;
     //                                }
     //                            }
     //                        }
     //                    }
     //                }

     //                if (better)
     //                {
     //                    l_density = density;
     //                    l_meanDist = meanDist;
     //                    l_count = cnt;
     //                    l_segLen = (float)segLen;

     //                    l_a = a; l_b = b; l_c = c;
     //                    l_inliers.swap(currInliers);
     //                }
     //            }

     //            if (l_count > -1)
     //            {
     //                std::lock_guard<std::mutex> lk(mtx);

     //                bool better = false;

     //                if (l_density > bestDensity) better = true;
     //                else if (std::abs(l_density - bestDensity) <= 1e-12)
     //                {
     //                    if (l_meanDist < bestMeanDist) better = true;
     //                    else if (std::abs(l_meanDist - bestMeanDist) <= 1e-12)
     //                    {
     //                        if (l_count > bestCount) better = true;
     //                        else if (l_count == bestCount)
     //                        {
     //                            if (l_segLen > bestSegLen) better = true;
     //                            else if (std::abs((double)l_segLen - (double)bestSegLen) <= 1e-12)
     //                            {
     //                                if (l_a < best_a) better = true;
     //                                else if (l_a == best_a)
     //                                {
     //                                    if (l_b < best_b) better = true;
     //                                    else if (l_b == best_b && l_c < best_c) better = true;
     //                                }
     //                            }
     //                        }
     //                    }
     //                }

     //                if (better)
     //                {
     //                    bestDensity = l_density;
     //                    bestMeanDist = l_meanDist;
     //                    bestCount = l_count;
     //                    bestSegLen = l_segLen;

     //                    best_a = l_a; best_b = l_b; best_c = l_c;
     //                    bestInliers = std::move(l_inliers);
     //                }
     //            }
     //        };

     //    int chunk = (iterations + numThreads - 1) / numThreads;
     //    int start = 0;
     //    for (int t = 0; t < numThreads; ++t)
     //    {
     //        int end = std::min(iterations, start + chunk);
     //        if (start >= end) break;
     //        threads.emplace_back(worker, start, end);
     //        start = end;
     //    }
     //    for (auto& th : threads) th.join();

     //    if (bestCount < 2 || bestInliers.size() < 2)
     //        return out;

     //    // 5) fitLine trên inliers
     //    cv::Vec4f line;
     //    cv::fitLine(bestInliers, line, cv::DIST_L2, 0, 0.01, 0.01); // (vx, vy, x0, y0)

     //    const cv::Point2f v(line[0], line[1]);
     //    const cv::Point2f p0(line[2], line[3]);

     //    // 6) Lấy đoạn theo inliers: min/max t
     //    double minT = std::numeric_limits<double>::max();
     //    double maxT = -std::numeric_limits<double>::max();
     //    for (const auto& q : bestInliers)
     //    {
     //        cv::Point2f pq = q - p0;
     //        double t = (double)pq.x * (double)v.x + (double)pq.y * (double)v.y;
     //        if (t < minT) minT = t;
     //        if (t > maxT) maxT = t;
     //    }

     //    cv::Point2f P1 = p0 + (float)minT * v;
     //    cv::Point2f P2 = p0 + (float)maxT * v;

     //    out.found = true;
     //    out.line = line;
     //    out.p1 = P1;
     //    out.p2 = P2;
     //    out.inliers = (int)bestInliers.size();
     //    out.score = (float)bestInliers.size() / (float)pts.size();

     //    const float dx2 = P2.x - P1.x;
     //    const float dy2 = P2.y - P1.y;
     //    out.length_px = std::sqrt(dx2 * dx2 + dy2 * dy2);
     //    out.length_mm = out.length_px * mmPerPixel;

     //    // (tuỳ bạn) lưu thêm quality nếu struct LineResult có field:
     //    // out.meanDist = (float)bestMeanDist;
     //    // out.density  = (float)bestDensity;

     //    return out;
     //}
//static void KeepLongestContiguousRun(
//    const std::vector<cv::Point2f>& inliers,
//    const cv::Vec4f& line,        // (vx, vy, x0, y0)
//    float gapPx,                  // ngưỡng đứt đoạn (pixel)
//    std::vector<cv::Point2f>& outRun
//)
//{
//    outRun.clear();
//    if (inliers.size() < 2) return;
//
//    cv::Point2f v(line[0], line[1]);
//    cv::Point2f p0(line[2], line[3]);
//
//    // 1) project → t
//    std::vector<std::pair<float, cv::Point2f>> proj;
//    proj.reserve(inliers.size());
//
//    for (const auto& p : inliers)
//    {
//        cv::Point2f d = p - p0;
//        float t = d.x * v.x + d.y * v.y;
//        proj.emplace_back(t, p);
//    }
//
//    std::sort(proj.begin(), proj.end(),
//        [](auto& a, auto& b) { return a.first < b.first; });
//
//    // 2) tách run theo gap
//    std::vector<cv::Point2f> curr, best;
//    curr.push_back(proj[0].second);
//
//    for (size_t i = 1; i < proj.size(); ++i)
//    {
//        float dt = proj[i].first - proj[i - 1].first;
//        if (dt <= gapPx)
//        {
//            curr.push_back(proj[i].second);
//        }
//        else
//        {
//            if (curr.size() > best.size())
//                best.swap(curr);
//            curr.clear();
//            curr.push_back(proj[i].second);
//        }
//    }
//    if (curr.size() > best.size())
//        best.swap(curr);
//
//    outRun.swap(best);
//}
//LineResult RansacLineCore::FindBestLine(
//    const Mat& edges8u1,
//    int iterations,
//    float threshold,
//    int maxPoints,
//    unsigned seed,
//    float mmPerPixel,
//    LineDirNative dirMode,
//    float angleCenterDeg,
//    float angleToleranceDeg,
//    LineScanPriority scanMode   // <<< NEW
//)
//{
//    LineResult out;
//
//    if (edges8u1.empty() || edges8u1.type() != CV_8UC1 || iterations <= 0)
//        return out;
//
//    // 1) Lấy non-zero
//    vector<Point> nz;
//    findNonZero(edges8u1, nz);
//    if (nz.empty()) return out;
//
//    // 2) Downsample
//    vector<Point2f> pts;
//    pts.reserve(std::min((int)nz.size(), maxPoints));
//    if ((int)nz.size() > maxPoints) {
//        int step = (int)nz.size() / maxPoints;
//        if (step < 1) step = 1;
//        for (int i = 0; i < (int)nz.size(); i += step)
//            pts.emplace_back((float)nz[i].x, (float)nz[i].y);
//    }
//    else {
//        for (const auto& p : nz)
//            pts.emplace_back((float)p.x, (float)p.y);
//    }
//    if ((int)pts.size() < 2) return out;
//
//    // 3) Precompute random pairs
//    auto pairs = PrecomputePairs((int)pts.size(), iterations, seed ? seed : 987654321u);
//
//    // 4) Multithread RANSAC
//    const int numThreads = std::max(1u, std::thread::hardware_concurrency());
//    vector<std::thread> threads;
//    std::mutex mtx;
//
//    int    bestCount = -1;
//    float  bestSegLen = -1.f;
//    float  bestScanPrio = FLT_MAX;
//    double best_a = 0., best_b = 0., best_c = 0.;
//    vector<Point2f> bestInliers;
//
//    auto worker = [&](int beginIt, int endIt)
//        {
//            int    l_count = -1;
//            float  l_segLen = -1.f;
//            float  l_scanPrio = FLT_MAX;
//            double l_a = 0., l_b = 0., l_c = 0.;
//            vector<Point2f> l_inliers;
//
//            const double thr = (double)threshold;
//
//            for (int it = beginIt; it < endIt; ++it)
//            {
//                const auto& pr = pairs[it];
//                const Point2f& p1 = pts[pr.first];
//                const Point2f& p2 = pts[pr.second];
//
//                double dx = p2.x - p1.x;
//                double dy = p2.y - p1.y;
//
//                double segLen = std::sqrt(dx * dx + dy * dy);
//                if (segLen < 1e-6) continue;
//
//                if (!CheckDirection(dx, dy, dirMode, angleCenterDeg, angleToleranceDeg))
//                    continue;
//
//                double a = p2.y - p1.y;
//                double b = p1.x - p2.x;
//                double norm = std::sqrt(a * a + b * b);
//                if (norm < 1e-6) continue;
//                double c = -(a * p1.x + b * p1.y);
//
//                int cnt = 0;
//                vector<Point2f> currInliers;
//                currInliers.reserve(256);
//
//                const double invNorm = 1.0 / norm;
//                for (const auto& q : pts) {
//                    double d = std::abs(a * q.x + b * q.y + c) * invNorm;
//                    if (d < thr) {
//                        ++cnt;
//                        currInliers.push_back(q);
//                    }
//                }
//                if (cnt == 0) continue;
//
//                float scanPrio = ComputeScanPriority(
//                    a, b, c,
//                    edges8u1.cols,
//                    edges8u1.rows,
//                    scanMode
//                );
//
//                bool better = false;
//                if (cnt > l_count) better = true;
//                else if (cnt == l_count) {
//                    if (segLen > l_segLen) better = true;
//                    else if (std::abs(segLen - l_segLen) <= 1e-9) {
//                        if (scanPrio < l_scanPrio) better = true;
//                    }
//                }
//
//                if (better) {
//                    l_count = cnt;
//                    l_segLen = (float)segLen;
//                    l_scanPrio = scanPrio;
//                    l_a = a; l_b = b; l_c = c;
//                    l_inliers.swap(currInliers);
//                }
//            }
//
//            if (l_count > -1) {
//                std::lock_guard<std::mutex> lk(mtx);
//
//                bool better = false;
//                if (l_count > bestCount) better = true;
//                else if (l_count == bestCount) {
//                    if (l_segLen > bestSegLen) better = true;
//                    else if (std::abs(l_segLen - bestSegLen) <= 1e-9) {
//                        if (l_scanPrio < bestScanPrio) better = true;
//                    }
//                }
//
//                if (better) {
//                    bestCount = l_count;
//                    bestSegLen = l_segLen;
//                    bestScanPrio = l_scanPrio;
//                    best_a = l_a; best_b = l_b; best_c = l_c;
//                    bestInliers = std::move(l_inliers);
//                }
//            }
//        };
//
//    int chunk = (iterations + numThreads - 1) / numThreads;
//    int start = 0;
//    for (int t = 0; t < numThreads; ++t) {
//        int end = std::min(iterations, start + chunk);
//        if (start >= end) break;
//        threads.emplace_back(worker, start, end);
//        start = end;
//    }
//    for (auto& th : threads) th.join();
//
//    if (bestCount < 2 || bestInliers.size() < 2)
//        return out;
//
//    // 5) fitLine trên inliers
//      //Vec4f line;
//      //fitLine(bestInliers, line, DIST_L2, 0, 0.01, 0.01); // (vx, vy, x0, y0)
//      // 5a) Fit line tạm (để lấy hướng)
//    Vec4f tmpLine;
//    fitLine(bestInliers, tmpLine, DIST_L2, 0, 0.01, 0.01);
//
//    // 5b) LỌC LIÊN TỤC
//    std::vector<Point2f> contiguous;
//    KeepLongestContiguousRun(
//        bestInliers,
//        tmpLine,
//        25.0f,        // <<< gapPx – chỉnh theo ảnh
//        contiguous
//    );
//
//    // nếu run quá ngắn → reject
//    if (contiguous.size() < 10)
//        return out;
//
//    // 5c) Fit lại line CHỈ trên run tốt nhất
//    Vec4f line;
//    fitLine(contiguous, line, DIST_L2, 0, 0.01, 0.01);
//
//    // dùng contiguous thay cho bestInliers
//    bestInliers.swap(contiguous);
//    ////
//    const Point2f v(line[0], line[1]);
//    const Point2f p0(line[2], line[3]);
//
//    // 6) Lấy đoạn theo inliers: min/max t
//    double minT = std::numeric_limits<double>::max();
//    double maxT = -std::numeric_limits<double>::max();
//    for (const auto& q : bestInliers) {
//        Point2f pq = q - p0;
//        double t = pq.x * v.x + pq.y * v.y; // v là đơn vị
//        if (t < minT) minT = t;
//        if (t > maxT) maxT = t;
//    }
//    Point2f P1 = p0 + (float)minT * v;
//    Point2f P2 = p0 + (float)maxT * v;
//
//    out.found = true;
//    out.line = line;
//    out.p1 = P1;
//    out.p2 = P2;
//    out.inliers = (int)bestInliers.size();
//    out.score = (float)bestInliers.size() / (float)pts.size();
//
//    // 7) Độ dài
//    const float dx = P2.x - P1.x;
//    const float dy = P2.y - P1.y;
//    out.length_px = std::sqrt(dx * dx + dy * dy);          // tương đương (maxT - minT)
//    out.length_mm = out.length_px * mmPerPixel;        // scale mm
//
//    return out;
//}

    //LineResult RansacLineCore::FindBestLine(const Mat& edges8u1,
    //    int iterations,
    //    float threshold,
    //    int maxPoints,
    //    unsigned seed,
    //    float mmPerPixel ,
    //    LineDirNative dirMode ,
    //    float angleCenterDeg ,
    //    float angleToleranceDeg )
    //{
    //    LineResult out;

    //    if (edges8u1.empty() || edges8u1.type() != CV_8UC1 || iterations <= 0) {
    //        return out;
    //    }

    //    // 1) Lấy non-zero
    //    vector<Point> nz;
    //    findNonZero(edges8u1, nz);
    //    if (nz.empty()) return out;

    //    // 2) Downsample
    //    vector<Point2f> pts;
    //    pts.reserve(std::min((int)nz.size(), maxPoints));
    //    if ((int)nz.size() > maxPoints) {
    //        int step = (int)nz.size() / maxPoints;
    //        if (step < 1) step = 1;
    //        for (int i = 0; i < (int)nz.size(); i += step) {
    //            const auto& p = nz[i];
    //            pts.emplace_back((float)p.x, (float)p.y);
    //        }
    //    }
    //    else {
    //        for (const auto& p : nz) pts.emplace_back((float)p.x, (float)p.y);
    //    }
    //    if ((int)pts.size() < 2) return out;

    //    // 3) Precompute cặp chỉ số
    //    auto pairs = PrecomputePairs((int)pts.size(), iterations, seed ? seed : 987654321u);

    //    // 4) RANSAC đa luồng (std::thread)
    //    const int T = (int)std::thread::hardware_concurrency();
    //    const int numThreads = std::max(1, T);
    //    vector<std::thread> threads;
    //    std::mutex mtx;

    //    int    bestCount = -1;
    //    float  bestSegLen = -1.f;
    //    double best_a = 0., best_b = 0., best_c = 0.;
    //    vector<Point2f> bestInliers;

    //    auto worker = [&](int beginIt, int endIt) {
    //        int   l_count = -1;
    //        float l_segLen = -1.f;
    //        double l_a = 0., l_b = 0., l_c = 0.;
    //        vector<Point2f> l_inliers;

    //        const double thr = (double)threshold;

    //        for (int it = beginIt; it < endIt; ++it) {
    //            const auto& pr = pairs[it];
    //            int i1 = pr.first, i2 = pr.second;
    //            const Point2f& p1 = pts[i1];
    //            const Point2f& p2 = pts[i2];

    //            double dx = p2.x - p1.x, dy = p2.y - p1.y;

    //            double segLen = std::sqrt(dx * dx + dy * dy);
    //            if (segLen < 1e-6) continue;
    //           

    //            // 👉 CHECK HƯỚNG
    //            if (!CheckDirection(dx, dy, dirMode, angleCenterDeg, angleToleranceDeg))
    //                continue;
    //            // ax + by + c = 0
    //            double a = (double)p2.y - (double)p1.y;
    //            double b = (double)p1.x - (double)p2.x;
    //            double norm = std::sqrt(a * a + b * b);
    //            if (norm < 1e-6) continue;
    //            double c = -(a * (double)p1.x + b * (double)p1.y);

    //            int cnt = 0;
    //            vector<Point2f> currInliers;
    //            currInliers.reserve(256);

    //            const double invNorm = 1.0 / norm;
    //            for (const auto& q : pts) {
    //                double d = std::abs(a * (double)q.x + b * (double)q.y + c) * invNorm;
    //                if (d < thr) {
    //                    ++cnt;
    //                    currInliers.push_back(q);
    //                }
    //            }
    //            if (cnt == 0) continue;

    //            bool better = false;
    //            if (cnt > l_count) better = true;
    //            else if (cnt == l_count) {
    //                if (segLen > l_segLen) better = true;
    //                else if (std::abs(segLen - l_segLen) <= 1e-9) {
    //                    if (a < l_a) better = true;
    //                    else if (a == l_a) {
    //                        if (b < l_b) better = true;
    //                        else if (b == l_b && c < l_c) better = true;
    //                    }
    //                }
    //            }

    //            if (better) {
    //                l_count = cnt;
    //                l_segLen = (float)segLen;
    //                l_a = a; l_b = b; l_c = c;
    //                l_inliers.swap(currInliers);
    //            }
    //        }

    //        if (l_count > -1) {
    //            std::lock_guard<std::mutex> lk(mtx);
    //            bool better = false;
    //            if (l_count > bestCount) better = true;
    //            else if (l_count == bestCount) {
    //                if (l_segLen > bestSegLen) better = true;
    //                else if (std::abs(l_segLen - bestSegLen) <= 1e-9) {
    //                    if (l_a < best_a) better = true;
    //                    else if (l_a == best_a) {
    //                        if (l_b < best_b) better = true;
    //                        else if (l_b == best_b && l_c < best_c) better = true;
    //                    }
    //                }
    //            }
    //            if (better) {
    //                bestCount = l_count;
    //                bestSegLen = l_segLen;
    //                best_a = l_a; best_b = l_b; best_c = l_c;
    //                bestInliers = std::move(l_inliers);
    //            }
    //        }
    //        };

    //    int chunk = (iterations + numThreads - 1) / numThreads;
    //    int start = 0;
    //    for (int t = 0; t < numThreads; ++t) {
    //        int end = std::min(iterations, start + chunk);
    //        if (start >= end) break;
    //        threads.emplace_back(worker, start, end);
    //        start = end;
    //    }
    //    for (auto& th : threads) th.join();

    //    if (bestCount < 2 || bestInliers.size() < 2) return out;

    //    // 5) fitLine trên inliers
    //    //Vec4f line;
    //    //fitLine(bestInliers, line, DIST_L2, 0, 0.01, 0.01); // (vx, vy, x0, y0)
    //    // 5a) Fit line tạm (để lấy hướng)
    //    Vec4f tmpLine;
    //    fitLine(bestInliers, tmpLine, DIST_L2, 0, 0.01, 0.01);

    //    // 5b) LỌC LIÊN TỤC
    //    std::vector<Point2f> contiguous;
    //    KeepLongestContiguousRun(
    //        bestInliers,
    //        tmpLine,
    //        25.0f,        // <<< gapPx – chỉnh theo ảnh
    //        contiguous
    //    );

    //    // nếu run quá ngắn → reject
    //    if (contiguous.size() < 10)
    //        return out;

    //    // 5c) Fit lại line CHỈ trên run tốt nhất
    //    Vec4f line;
    //    fitLine(contiguous, line, DIST_L2, 0, 0.01, 0.01);

    //    // dùng contiguous thay cho bestInliers
    //    bestInliers.swap(contiguous);
    //    ////
    //    const Point2f v(line[0], line[1]);
    //    const Point2f p0(line[2], line[3]);

    //    // 6) Lấy đoạn theo inliers: min/max t
    //    double minT = std::numeric_limits<double>::max();
    //    double maxT = -std::numeric_limits<double>::max();
    //    for (const auto& q : bestInliers) {
    //        Point2f pq = q - p0;
    //        double t = pq.x * v.x + pq.y * v.y; // v là đơn vị
    //        if (t < minT) minT = t;
    //        if (t > maxT) maxT = t;
    //    }
    //    Point2f P1 = p0 + (float)minT * v;
    //    Point2f P2 = p0 + (float)maxT * v;

    //    out.found = true;
    //    out.line = line;
    //    out.p1 = P1;
    //    out.p2 = P2;
    //    out.inliers = (int)bestInliers.size();
    //    out.score = (float)bestInliers.size() / (float)pts.size();

    //    // 7) Độ dài
    //    const float dx = P2.x - P1.x;
    //    const float dy = P2.y - P1.y;
    //    out.length_px = std::sqrt(dx * dx + dy * dy);          // tương đương (maxT - minT)
    //    out.length_mm = out.length_px * mmPerPixel;        // scale mm

    //    return out;
    //}
//new 

static void KeepLongestContiguousRun(
    const std::vector<cv::Point2f>& inliers,
    const cv::Vec4f& line,   // (vx, vy, x0, y0)
    float gapPx,
    std::vector<cv::Point2f>& outRun)
{
    outRun.clear();
    if (inliers.size() < 2) return;

    cv::Point2f v(line[0], line[1]);
    cv::Point2f p0(line[2], line[3]);

    std::vector<std::pair<float, cv::Point2f>> proj;
    proj.reserve(inliers.size());

    for (auto& p : inliers) {
        cv::Point2f d = p - p0;
        float t = d.x * v.x + d.y * v.y;
        proj.emplace_back(t, p);
    }

    std::sort(proj.begin(), proj.end(),
        [](const auto& a, const auto& b) { return a.first < b.first; });

    std::vector<cv::Point2f> curr, best;
    curr.push_back(proj[0].second);

    for (size_t i = 1; i < proj.size(); ++i) {
        if (proj[i].first - proj[i - 1].first <= gapPx)
            curr.push_back(proj[i].second);
        else {
            if (curr.size() > best.size()) best.swap(curr);
            curr.clear();
            curr.push_back(proj[i].second);
        }
    }
    if (curr.size() > best.size()) best.swap(curr);

    outRun.swap(best);
}
static float ComputeScanPos(
    const std::vector<cv::Point2f>& pts,
    LineScanPriority scanMode,
    int roiW, int roiH)
{
    float minX = FLT_MAX, maxX = -FLT_MAX;
    float minY = FLT_MAX, maxY = -FLT_MAX;

    for (auto& p : pts) {
        minX = std::min(minX, p.x);
        maxX = std::max(maxX, p.x);
        minY = std::min(minY, p.y);
        maxY = std::max(maxY, p.y);
    }

    switch (scanMode)
    {
    case LineScanPriority::BottomToTop:  return roiH - maxY; // gần đáy → nhỏ
    case LineScanPriority::TopToBottom:  return minY;
    case LineScanPriority::LeftToRight:  return minX;
    case LineScanPriority::RightToLeft:  return roiW - maxX;
    default:                             return 0.f;
    }
}
static float ComputeRunLengthPx(
    const std::vector<cv::Point2f>& pts,
    const cv::Vec4f& line)
{
    if (pts.size() < 2) return 0.f;

    cv::Point2f v(line[0], line[1]);
    cv::Point2f p0(line[2], line[3]);

    double minT = DBL_MAX, maxT = -DBL_MAX;
    for (auto& p : pts) {
        cv::Point2f d = p - p0;
        double t = d.x * v.x + d.y * v.y;
        minT = std::min(minT, t);
        maxT = std::max(maxT, t);
    }
    return (float)(maxT - minT);
}
static float ComputeMinLenPx(
    int w, int h,
    LineDirNative dirMode,
    LineScanPriority scanMode,
    float ratio = 0.6f)
{
    switch (scanMode)
    {
    case LineScanPriority::BottomToTop:
    case LineScanPriority::TopToBottom:
        return ratio * w;

    case LineScanPriority::LeftToRight:
    case LineScanPriority::RightToLeft:
        return ratio * h;

    default:
        return (dirMode == LineDirNative::Vertical)
            ? ratio * w
            : ratio * h;
    }
}

struct ParallelCandidate {
    std::vector<cv::Point2f> run;
    cv::Vec4f line;
    float runLen;
    int inliers;
};

static float AxisAngleDeg(const cv::Vec4f& line)
{
    double ang = std::atan2((double)line[1], (double)line[0]) * 180.0 / CV_PI;
    if (ang < 0.0) ang += 180.0;
    if (ang >= 180.0) ang -= 180.0;
    return (float)ang;
}

static float AxisAngleDiffDeg(float a, float b)
{
    float d = std::abs(a - b);
    return std::min(d, 180.0f - d);
}

static void ProjectionRange(
    const std::vector<cv::Point2f>& pts,
    const cv::Point2f& v,
    double& minT,
    double& maxT)
{
    minT = DBL_MAX;
    maxT = -DBL_MAX;
    for (const auto& p : pts) {
        double t = (double)p.x * (double)v.x + (double)p.y * (double)v.y;
        minT = std::min(minT, t);
        maxT = std::max(maxT, t);
    }
}

static LineResult MakeLineResultFromRange(
    const cv::Vec4f& line,
    float t1,
    float t2,
    int inliers,
    int totalPoints,
    float mmPerPixel)
{
    LineResult out;
    cv::Point2f v(line[0], line[1]);
    cv::Point2f p0(line[2], line[3]);

    out.found = true;
    out.line = line;
    out.p1 = p0 + (t1 - (p0.x * v.x + p0.y * v.y)) * v;
    out.p2 = p0 + (t2 - (p0.x * v.x + p0.y * v.y)) * v;
    out.inliers = inliers;
    out.length_px = std::sqrt(
        (out.p2.x - out.p1.x) * (out.p2.x - out.p1.x) +
        (out.p2.y - out.p1.y) * (out.p2.y - out.p1.y));
    out.length_mm = out.length_px * mmPerPixel;
    out.score = totalPoints > 0 ? (float)inliers / (float)totalPoints : 0.f;
    return out;
}

ParallelLinePairResult RansacLineCore::FindLongestParallelPair(
    const cv::Mat& edges8u1,
    int iterations,
    float threshold,
    int maxPoints,
    unsigned seed,
    float mmPerPixel,
    float minLengthRatio,
    float parallelToleranceDeg,
    float minGapPx,
    float maxGapPx,
    float minOverlapRatio,
    float contiguousGapPx)
{
    ParallelLinePairResult out;
    if (edges8u1.empty() || edges8u1.type() != CV_8UC1 || iterations <= 0)
        return out;

    std::vector<cv::Point> nz;
    cv::findNonZero(edges8u1, nz);
    if (nz.size() < 4) return out;

    maxPoints = std::max(2, maxPoints);
    std::vector<cv::Point2f> pts;
    pts.reserve(std::min((int)nz.size(), maxPoints));

    int step = (int)nz.size() > maxPoints ? (int)nz.size() / maxPoints : 1;
    if (step < 1) step = 1;
    for (size_t i = 0; i < nz.size(); i += step)
        pts.emplace_back((float)nz[i].x, (float)nz[i].y);
    if (pts.size() < 4) return out;

    auto pairs = PrecomputePairs((int)pts.size(), iterations, seed ? seed : 987654321u);

    std::vector<ParallelCandidate> candidates;
    std::mutex mtx;
    int numThreads = std::max(1u, std::thread::hardware_concurrency());
    int chunk = (iterations + numThreads - 1) / numThreads;
    std::vector<std::thread> threads;

    const double thr = (double)threshold;
    // Đổi từ imageDiag sang min(W,H): khi crop lớn + có nhiều noise rìa, ngưỡng theo
    // diagonal làm ngưỡng tối thiểu quá lớn (ví dụ 0.6×1414 = 848px) khiến line thực tế
    // (~300px) bị reject. Dùng min(W,H) cho phép line ngắn hơn pass, nhất quán với
    // FindBestLine::ComputeMinLenPx vốn dùng W hoặc H tuỳ direction.
    const float minDim = (float)std::min(edges8u1.cols, edges8u1.rows);
    const float minLenPx = std::max(2.0f, minLengthRatio * minDim);

    auto worker = [&](int beg, int end)
        {
            std::vector<ParallelCandidate> local;
            for (int it = beg; it < end; ++it)
            {
                const auto& pr = pairs[it];
                const cv::Point2f& p1 = pts[pr.first];
                const cv::Point2f& p2 = pts[pr.second];

                double a = (double)p2.y - (double)p1.y;
                double b = (double)p1.x - (double)p2.x;
                double n = std::sqrt(a * a + b * b);
                if (n < 1e-6) continue;
                double c = -(a * (double)p1.x + b * (double)p1.y);
                double invN = 1.0 / n;

                std::vector<cv::Point2f> inl;
                inl.reserve(256);
                for (const auto& q : pts) {
                    double d = std::abs(a * (double)q.x + b * (double)q.y + c) * invN;
                    if (d < thr) inl.push_back(q);
                }
                if (inl.size() < 2) continue;

                cv::Vec4f tmp;
                cv::fitLine(inl, tmp, cv::DIST_L2, 0, 0.01, 0.01);

                std::vector<cv::Point2f> run;
                KeepLongestContiguousRun(inl, tmp, contiguousGapPx, run);
                if (run.size() < 2) continue;

                cv::Vec4f line;
                cv::fitLine(run, line, cv::DIST_L2, 0, 0.01, 0.01);

                float runLen = ComputeRunLengthPx(run, line);
                if (runLen < minLenPx) continue;

                local.push_back({ run, line, runLen, (int)inl.size() });
            }

            if (!local.empty()) {
                std::lock_guard<std::mutex> lk(mtx);
                candidates.insert(candidates.end(),
                    std::make_move_iterator(local.begin()),
                    std::make_move_iterator(local.end()));
            }
        };

    int st = 0;
    for (int t = 0; t < numThreads; ++t) {
        int ed = std::min(iterations, st + chunk);
        if (st >= ed) break;
        threads.emplace_back(worker, st, ed);
        st = ed;
    }
    for (auto& th : threads) th.join();

    if (candidates.size() < 2) return out;

    std::sort(candidates.begin(), candidates.end(),
        [](const ParallelCandidate& a, const ParallelCandidate& b)
        {
            if (std::abs(a.runLen - b.runLen) > 1e-3f) return a.runLen > b.runLen;
            return a.inliers > b.inliers;
        });

    const int candidateLimit = std::min((int)candidates.size(), 80);
    float bestScore = -FLT_MAX;
    int bestI = -1, bestJ = -1;
    double bestOverlapMin = 0.0, bestOverlapMax = 0.0;
    cv::Point2f bestV(0.f, 0.f), bestN(0.f, 0.f);
    double bestCenterC = 0.0;
    double bestGap = 0.0;

    for (int i = 0; i < candidateLimit; ++i) {
        const auto& a = candidates[i];
        float angA = AxisAngleDeg(a.line);
        cv::Point2f va(a.line[0], a.line[1]);
        double vaLen = std::sqrt((double)va.x * va.x + (double)va.y * va.y);
        if (vaLen < 1e-6) continue;
        va *= (float)(1.0 / vaLen);

        for (int j = i + 1; j < candidateLimit; ++j) {
            const auto& b = candidates[j];
            float angleDiff = AxisAngleDiffDeg(angA, AxisAngleDeg(b.line));
            if (angleDiff > parallelToleranceDeg) continue;

            cv::Point2f vb(b.line[0], b.line[1]);
            double vbLen = std::sqrt((double)vb.x * vb.x + (double)vb.y * vb.y);
            if (vbLen < 1e-6) continue;
            vb *= (float)(1.0 / vbLen);
            if (va.dot(vb) < 0.f) vb *= -1.f;

            cv::Point2f v = va + vb;
            double vLen = std::sqrt((double)v.x * v.x + (double)v.y * v.y);
            if (vLen < 1e-6) v = va;
            else v *= (float)(1.0 / vLen);

            cv::Point2f n(-v.y, v.x);
            cv::Point2f p0a(a.line[2], a.line[3]);
            cv::Point2f p0b(b.line[2], b.line[3]);
            double cA = -((double)n.x * p0a.x + (double)n.y * p0a.y);
            double cB = -((double)n.x * p0b.x + (double)n.y * p0b.y);
            double gap = std::abs(cB - cA);
            if (gap < minGapPx) continue;
            if (maxGapPx > 0.f && gap > maxGapPx) continue;

            double minA, maxA, minB, maxB;
            ProjectionRange(a.run, v, minA, maxA);
            ProjectionRange(b.run, v, minB, maxB);
            double overlapMin = std::max(minA, minB);
            double overlapMax = std::min(maxA, maxB);
            double overlap = overlapMax - overlapMin;
            if (overlap <= 1.0) continue;

            double minPairLen = std::max(1.0f, std::min(a.runLen, b.runLen));
            double overlapRatio = overlap / minPairLen;
            if (overlapRatio < minOverlapRatio) continue;

            float densityA = (float)a.run.size() / (a.runLen + 1e-6f);
            float densityB = (float)b.run.size() / (b.runLen + 1e-6f);
            float score = (float)(overlap * 2.0 + minPairLen + 0.25 * (a.runLen + b.runLen)) +
                (densityA + densityB) * 25.0f -
                angleDiff * 10.0f;

            if (score > bestScore) {
                bestScore = score;
                bestI = i;
                bestJ = j;
                bestOverlapMin = overlapMin;
                bestOverlapMax = overlapMax;
                bestV = v;
                bestN = n;
                bestCenterC = (cA + cB) * 0.5;
                bestGap = gap;
            }
        }
    }

    if (bestI < 0 || bestJ < 0) return out;

    const auto& ca = candidates[bestI];
    const auto& cb = candidates[bestJ];

    cv::Point2f vaOut(ca.line[0], ca.line[1]);
    cv::Point2f vbOut(cb.line[0], cb.line[1]);
    double vaOutLen = std::sqrt((double)vaOut.x * vaOut.x + (double)vaOut.y * vaOut.y);
    double vbOutLen = std::sqrt((double)vbOut.x * vbOut.x + (double)vbOut.y * vbOut.y);
    if (vaOutLen < 1e-6 || vbOutLen < 1e-6) return out;
    vaOut *= (float)(1.0 / vaOutLen);
    vbOut *= (float)(1.0 / vbOutLen);

    double minA, maxA, minB, maxB;
    ProjectionRange(ca.run, vaOut, minA, maxA);
    ProjectionRange(cb.run, vbOut, minB, maxB);

    cv::Vec4f center(
        bestV.x,
        bestV.y,
        (float)(-bestCenterC * bestN.x),
        (float)(-bestCenterC * bestN.y));

    out.found = true;
    out.lineA = MakeLineResultFromRange(ca.line, (float)minA, (float)maxA, ca.inliers, (int)pts.size(), mmPerPixel);
    out.lineB = MakeLineResultFromRange(cb.line, (float)minB, (float)maxB, cb.inliers, (int)pts.size(), mmPerPixel);
    out.centerLine = MakeLineResultFromRange(center, (float)bestOverlapMin, (float)bestOverlapMax,
        std::min(ca.inliers, cb.inliers), (int)pts.size(), mmPerPixel);
    out.gap_px = (float)bestGap;
    out.gap_mm = out.gap_px * mmPerPixel;
    out.angle_deg = AxisAngleDeg(center);

    return out;
}

LineResult RansacLineCore::FindBestLine(
    const cv::Mat& edges8u1,
    int iterations,
    float threshold,
    int maxPoints,
    unsigned seed,
    float mmPerPixel,
    float AspectLen,
    LineDirNative dirMode,
    float angleCenterDeg,
    float angleToleranceDeg,
    LineScanPriority scanMode)
{
    LineResult out;
    if (edges8u1.empty() || edges8u1.type() != CV_8UC1)
        return out;

    // ---- Collect edge points ----
    std::vector<cv::Point> nz;
    findNonZero(edges8u1, nz);
    if (nz.size() < 2) return out;

    std::vector<cv::Point2f> pts;
    pts.reserve(std::min((int)nz.size(), maxPoints));

    int step = (int)nz.size() > maxPoints ? (int)nz.size() / maxPoints : 1;
    if (step < 1) step = 1;

    for (size_t i = 0; i < nz.size(); i += step)
        pts.emplace_back((float)nz[i].x, (float)nz[i].y);

    // ---- Precompute RANSAC pairs ----
    auto pairs = PrecomputePairs((int)pts.size(), iterations, seed ? seed : 123456u);

    struct Candidate {
        std::vector<cv::Point2f> run;
        cv::Vec4f line;
        float scanPos;
        float runLen;
        int inliers;
    };

    std::vector<Candidate> candidates;
    std::mutex mtx;

    // ---- RANSAC: sinh candidate ----
    int numThreads = std::max(1u, std::thread::hardware_concurrency());
    int chunk = (iterations + numThreads - 1) / numThreads;
    std::vector<std::thread> threads;

    auto worker = [&](int beg, int end)
        {
            for (int it = beg; it < end; ++it)
            {
                auto& pr = pairs[it];
                auto& p1 = pts[pr.first];
                auto& p2 = pts[pr.second];

                double dx = p2.x - p1.x;
                double dy = p2.y - p1.y;

                if (!CheckDirection(dx, dy, dirMode, angleCenterDeg, angleToleranceDeg))
                    continue;

                double a = p2.y - p1.y;
                double b = p1.x - p2.x;
                double n = std::sqrt(a * a + b * b);
                if (n < 1e-6) continue;
                double c = -(a * p1.x + b * p1.y);

                std::vector<cv::Point2f> inl;
                double invN = 1.0 / n;

                for (auto& q : pts)
                    if (std::abs(a * q.x + b * q.y + c) * invN < threshold)
                        inl.push_back(q);

                if (inl.size() < 2) continue;

                cv::Vec4f tmp;
                fitLine(inl, tmp, DIST_L2, 0, 0.01, 0.01);

                std::vector<cv::Point2f> run;
                KeepLongestContiguousRun(inl, tmp, 25.0f, run);
                if (run.size() < 2) continue;

                cv::Vec4f line;
                fitLine(run, line, DIST_L2, 0, 0.01, 0.01);

                float runLen = ComputeRunLengthPx(run, line);
                float scanPos = (scanMode != LineScanPriority::None)
                    ? ComputeScanPos(run, scanMode, edges8u1.cols, edges8u1.rows)
                    : 0.f;

                std::lock_guard<std::mutex> lk(mtx);
                candidates.push_back({
                    run, line, scanPos, runLen, (int)inl.size()
                    });
            }
        };

    int st = 0;
    for (int t = 0; t < numThreads; ++t) {
        int ed = std::min(iterations, st + chunk);
        if (st >= ed) break;
        threads.emplace_back(worker, st, ed);
        st = ed;
    }
    for (auto& th : threads) th.join();

    if (candidates.empty()) return out;

    // ---- Gate min length ----
    float minLenPx = ComputeMinLenPx(
        edges8u1.cols, edges8u1.rows, dirMode, scanMode, 0.3f);

    // ---- Sort theo chiều quét ----
    if (scanMode != LineScanPriority::None)
    {
        std::sort(candidates.begin(), candidates.end(),
            [](const Candidate& a, const Candidate& b)
            {
                return a.scanPos < b.scanPos;
            });
    }
  //  Candidate* best = nullptr;

    //for (auto& c : candidates)
    //{
    //    if (c.runLen < minLenPx)
    //        continue;

    //    if (!best)
    //    {
    //        best = &c;
    //        continue;
    //    }

    //    // scan priority trước
    //    if (c.scanPos < best->scanPos)
    //    {
    //        best = &c;
    //        continue;
    //    }

    //    // nếu cùng vị trí thì chọn line dài hơn
    //    if (std::abs(c.scanPos - best->scanPos) < 3)
    //    {
    //        if (c.runLen > best->runLen)
    //            best = &c;
    //    }
    //}
    /*Candidate* best = nullptr;
    float bestScore = -FLT_MAX;

    for (auto& c : candidates)
    {
        if (c.runLen < minLenPx)
            continue;

        float density = c.run.size() / (c.runLen + 1e-6f);

        float score =
            c.runLen * 2.0f +
            density * 200.0f -
            c.scanPos * 0.5f;

        if (score > bestScore)
        {
            bestScore = score;
            best = &c;
        }
    }*/
    //// ---- Chọn line tốt nhất theo thứ tự quét ----
    Candidate* best = nullptr;

// ---- 1. lọc line đủ dài ----
    std::vector<Candidate*> valid;

    for (auto& c : candidates)
    {
        if (c.runLen >= minLenPx)
            valid.push_back(&c);
    }

    if (valid.empty())
        return out;

    // ---- 2. sort theo độ dài ----
    std::sort(valid.begin(), valid.end(),
        [](Candidate* a, Candidate* b)
        {
            return a->runLen > b->runLen;
        });

    // ---- 3. lấy TOP N ----
    int topN = std::min((int)valid.size(), 8);

    // ---- 4. chọn theo density + scan ----
    float bestScore = -FLT_MAX;

    for (int i = 0; i < topN; i++)
    {
        Candidate* c = valid[i];

        float density = (float)c->run.size() / (c->runLen + 1e-6f);

        // normalize scan position
        float scanNorm = 1.0f;

        if (scanMode == LineScanPriority::BottomToTop ||
            scanMode == LineScanPriority::TopToBottom)
        {
            scanNorm = 1.0f - (c->scanPos / (float)edges8u1.rows);
        }
        else if (scanMode == LineScanPriority::LeftToRight ||
            scanMode == LineScanPriority::RightToLeft)
        {
            scanNorm = 1.0f - (c->scanPos / (float)edges8u1.cols);
        }

        scanNorm = std::clamp(scanNorm, 0.0f, 1.0f);

        float score =
            density * 0.7f +
            scanNorm * 0.3f;

        if (score > bestScore)
        {
            bestScore = score;
            best = c;
        }
    }
    //Candidate* best = nullptr;

    //for (auto& c : candidates)
    //{
    //    if (c.runLen < minLenPx)
    //        continue;

    //    if (!best)
    //        best = &c;
    //    else if (c.runLen > best->runLen)
    //        best = &c;
    //    else if (std::abs(c.runLen - best->runLen) < 1e-3 &&
    //        c.inliers > best->inliers)
    //        best = &c;
    //}

    // ---- Fallback khi scanMode == None ----
    if (!best && scanMode == LineScanPriority::None)
    {
        best = &(*std::max_element(
            candidates.begin(), candidates.end(),
            [](const Candidate& a, const Candidate& b)
            {
                return a.runLen < b.runLen;
            }));
    }

    if (!best) return out;

    // ---- Output ----
    cv::Point2f v(best->line[0], best->line[1]);
    cv::Point2f p0(best->line[2], best->line[3]);

    double minT = DBL_MAX, maxT = -DBL_MAX;
    for (auto& p : best->run) {
        cv::Point2f d = p - p0;
        double t = d.x * v.x + d.y * v.y;
        minT = std::min(minT, t);
        maxT = std::max(maxT, t);
    }

    out.found = true;
    out.line = best->line;
    out.p1 = p0 + (float)minT * v;
    out.p2 = p0 + (float)maxT * v;
    out.inliers = best->inliers;
    out.length_px = best->runLen;
    out.length_mm = best->runLen * mmPerPixel;
    out.score = (float)best->run.size() / (float)pts.size();

    return out;
}


}
