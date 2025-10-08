#include "RansacLineCore.h"
#include <thread>
#include <mutex>
#include <limits>
#include <cmath>

using namespace cv;
using std::vector;
using std::pair;

namespace BeeCpp {

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

    LineResult RansacLineCore::FindBestLine(const Mat& edges8u1,
        int iterations,
        float threshold,
        int maxPoints,
        unsigned seed,
        float mmPerPixel)
    {
        LineResult out;

        if (edges8u1.empty() || edges8u1.type() != CV_8UC1 || iterations <= 0) {
            return out;
        }

        // 1) Lấy non-zero
        vector<Point> nz;
        findNonZero(edges8u1, nz);
        if (nz.empty()) return out;

        // 2) Downsample
        vector<Point2f> pts;
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
        if ((int)pts.size() < 2) return out;

        // 3) Precompute cặp chỉ số
        auto pairs = PrecomputePairs((int)pts.size(), iterations, seed ? seed : 987654321u);

        // 4) RANSAC đa luồng (std::thread)
        const int T = (int)std::thread::hardware_concurrency();
        const int numThreads = std::max(1, T);
        vector<std::thread> threads;
        std::mutex mtx;

        int    bestCount = -1;
        float  bestSegLen = -1.f;
        double best_a = 0., best_b = 0., best_c = 0.;
        vector<Point2f> bestInliers;

        auto worker = [&](int beginIt, int endIt) {
            int   l_count = -1;
            float l_segLen = -1.f;
            double l_a = 0., l_b = 0., l_c = 0.;
            vector<Point2f> l_inliers;

            const double thr = (double)threshold;

            for (int it = beginIt; it < endIt; ++it) {
                const auto& pr = pairs[it];
                int i1 = pr.first, i2 = pr.second;
                const Point2f& p1 = pts[i1];
                const Point2f& p2 = pts[i2];

                double dx = p2.x - p1.x, dy = p2.y - p1.y;
                double segLen = std::sqrt(dx * dx + dy * dy);
                if (segLen < 1e-6) continue;

                // ax + by + c = 0
                double a = (double)p2.y - (double)p1.y;
                double b = (double)p1.x - (double)p2.x;
                double norm = std::sqrt(a * a + b * b);
                if (norm < 1e-6) continue;
                double c = -(a * (double)p1.x + b * (double)p1.y);

                int cnt = 0;
                vector<Point2f> currInliers;
                currInliers.reserve(256);

                const double invNorm = 1.0 / norm;
                for (const auto& q : pts) {
                    double d = std::abs(a * (double)q.x + b * (double)q.y + c) * invNorm;
                    if (d < thr) {
                        ++cnt;
                        currInliers.push_back(q);
                    }
                }
                if (cnt == 0) continue;

                bool better = false;
                if (cnt > l_count) better = true;
                else if (cnt == l_count) {
                    if (segLen > l_segLen) better = true;
                    else if (std::abs(segLen - l_segLen) <= 1e-9) {
                        if (a < l_a) better = true;
                        else if (a == l_a) {
                            if (b < l_b) better = true;
                            else if (b == l_b && c < l_c) better = true;
                        }
                    }
                }

                if (better) {
                    l_count = cnt;
                    l_segLen = (float)segLen;
                    l_a = a; l_b = b; l_c = c;
                    l_inliers.swap(currInliers);
                }
            }

            if (l_count > -1) {
                std::lock_guard<std::mutex> lk(mtx);
                bool better = false;
                if (l_count > bestCount) better = true;
                else if (l_count == bestCount) {
                    if (l_segLen > bestSegLen) better = true;
                    else if (std::abs(l_segLen - bestSegLen) <= 1e-9) {
                        if (l_a < best_a) better = true;
                        else if (l_a == best_a) {
                            if (l_b < best_b) better = true;
                            else if (l_b == best_b && l_c < best_c) better = true;
                        }
                    }
                }
                if (better) {
                    bestCount = l_count;
                    bestSegLen = l_segLen;
                    best_a = l_a; best_b = l_b; best_c = l_c;
                    bestInliers = std::move(l_inliers);
                }
            }
            };

        int chunk = (iterations + numThreads - 1) / numThreads;
        int start = 0;
        for (int t = 0; t < numThreads; ++t) {
            int end = std::min(iterations, start + chunk);
            if (start >= end) break;
            threads.emplace_back(worker, start, end);
            start = end;
        }
        for (auto& th : threads) th.join();

        if (bestCount < 2 || bestInliers.size() < 2) return out;

        // 5) fitLine trên inliers
        Vec4f line;
        fitLine(bestInliers, line, DIST_L2, 0, 0.01, 0.01); // (vx, vy, x0, y0)

        const Point2f v(line[0], line[1]);
        const Point2f p0(line[2], line[3]);

        // 6) Lấy đoạn theo inliers: min/max t
        double minT = std::numeric_limits<double>::max();
        double maxT = -std::numeric_limits<double>::max();
        for (const auto& q : bestInliers) {
            Point2f pq = q - p0;
            double t = pq.x * v.x + pq.y * v.y; // v là đơn vị
            if (t < minT) minT = t;
            if (t > maxT) maxT = t;
        }
        Point2f P1 = p0 + (float)minT * v;
        Point2f P2 = p0 + (float)maxT * v;

        out.found = true;
        out.line = line;
        out.p1 = P1;
        out.p2 = P2;
        out.inliers = (int)bestInliers.size();
        out.score = (float)bestInliers.size() / (float)pts.size();

        // 7) Độ dài
        const float dx = P2.x - P1.x;
        const float dy = P2.y - P1.y;
        out.length_px = std::sqrt(dx * dx + dy * dy);          // tương đương (maxT - minT)
        out.length_mm = out.length_px * mmPerPixel;        // scale mm

        return out;
    }
   
}
