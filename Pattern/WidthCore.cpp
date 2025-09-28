#include "WidthCore.h"
#include <algorithm>
#include <cmath>
#include <thread>
#include <mutex>

using namespace cv;
using namespace std;

namespace {
    // Ứng viên cho tie-break ổn định
    struct Candidate {
        int cnt{ -1 };
        double segLen{ -1 };
        double a{ 0 }, b{ 0 }, c{ 0 };
        int i1{ -1 }, i2{ -1 };
        std::vector<cv::Point2f> inliers;
    };

    // So sánh “tốt hơn” có total-order → deterministic
    static inline bool betterThan(const Candidate& lhs, const Candidate& rhs) {
        if (lhs.cnt != rhs.cnt) return lhs.cnt > rhs.cnt;
        if (lhs.segLen != rhs.segLen) return lhs.segLen > rhs.segLen;
        if (lhs.a != rhs.a) return lhs.a < rhs.a;
        if (lhs.b != rhs.b) return lhs.b < rhs.b;
        if (lhs.c != rhs.c) return lhs.c < rhs.c;
        if (lhs.i1 != rhs.i1) return lhs.i1 < rhs.i1;
        return lhs.i2 < rhs.i2;
    }

    static inline double hypot2(double x, double y) { return std::sqrt(x * x + y * y); }
} // anonymous

namespace BeeCpp {

    WidthCore::WidthCore() {}

    void WidthCore::SetMmPerPixel(double mpp) { mmPerPixel_ = (mpp > 0 ? mpp : 1.0); }
    void WidthCore::SetRansac(int iterations, double threshold) {
        ransacIter_ = std::max(1, iterations);
        ransacThr_ = (threshold > 0 ? threshold : 2.0);
    }

    void WidthCore::SetImageRaw(const unsigned char* data, int width, int height, int stride, int channels) {
        if (!data || width <= 0 || height <= 0 || stride <= 0) { raw_.release(); edges_.release(); return; }
        int type = (channels == 1) ? CV_8UC1 : CV_8UC3;
        Mat src(height, width, type, const_cast<unsigned char*>(data), stride);
        if (channels == 3) {
            Mat bgr = src.clone();
            cvtColor(bgr, raw_, COLOR_BGR2GRAY);
        }
        else {
            raw_ = src.clone();
        }
        buildEdges();
    }

    void WidthCore::SetImageCrop(const unsigned char* data, int width, int height, int stride, int channels,
        float cx, float cy, float w, float h, float angleDeg)
    {
        if (!data || width <= 0 || height <= 0 || stride <= 0) { raw_.release(); edges_.release(); return; }
        int type = (channels == 1) ? CV_8UC1 : CV_8UC3;
        Mat src(height, width, type, const_cast<unsigned char*>(data), stride);

        Mat gray;
        if (channels == 3) {
            Mat bgr = src.clone();
            cvtColor(bgr, gray, COLOR_BGR2GRAY);
        }
        else gray = src.clone();

        // (cx,cy) là tâm, (w,h) size, angleDeg CCW
        RotatedRect rr(Point2f(cx, cy), Size2f(w, h), angleDeg);

        Mat M = getRotationMatrix2D(rr.center, rr.angle, 1.0);
        Mat rotated;
        warpAffine(gray, rotated, M, gray.size(), INTER_LINEAR, BORDER_REPLICATE);

        Rect roiRect(int(rr.center.x - rr.size.width * 0.5f),
            int(rr.center.y - rr.size.height * 0.5f),
            int(std::round(rr.size.width)),
            int(std::round(rr.size.height)));
        roiRect &= Rect(0, 0, rotated.cols, rotated.rows);
        if (roiRect.width <= 0 || roiRect.height <= 0) { raw_.release(); edges_.release(); return; }

        raw_ = rotated(roiRect).clone();
        buildEdges();
    }

    double WidthCore::median8U(const Mat& gray) {
        CV_Assert(gray.type() == CV_8UC1);
        int hist[256] = { 0 };
        for (int y = 0; y < gray.rows; ++y) {
            const uchar* row = gray.ptr<uchar>(y);
            for (int x = 0; x < gray.cols; ++x) ++hist[row[x]];
        }
        int total = gray.rows * gray.cols;
        int half = (total + 1) / 2;
        int cum = 0;
        for (int i = 0; i < 256; ++i) { cum += hist[i]; if (cum >= half) return (double)i; }
        return 0.0;
    }

    void WidthCore::buildEdges() {
        if (raw_.empty()) { edges_.release(); return; }
        Mat bl;
        medianBlur(raw_, bl, 3);
        double med = median8U(bl);
        double lo = std::max(0.0, med * 0.66);
        double hi = std::min(255.0, med * 1.33);
        Canny(bl, edges_, lo, hi, 3, true);
    }

    std::vector<std::pair<int, int>> WidthCore::precomputePairs(int n, int iterations, uint32_t seed) {
        std::vector<std::pair<int, int>> v(iterations);
        if (n <= 1) return v;
        uint32_t s = seed;
        auto nextIdx = [&](uint32_t& x)->int {
            x ^= x << 13; x ^= x >> 17; x ^= x << 5;
            return int(x % (uint32_t)n);
            };
        for (int i = 0; i < iterations; ++i) {
            int a = nextIdx(s), b;
            do { b = nextIdx(s); } while (b == a);
            v[i] = { a,b };
        }
        return v;
    }

    std::vector<Point2f> WidthCore::extractEdgePoints() const {
        std::vector<Point2f> out;
        if (edges_.empty()) return out;
        Mat nz;
        findNonZero(edges_, nz);
        out.reserve((size_t)nz.rows);
        for (int i = 0; i < nz.rows; ++i) {
            Point p = nz.at<Point>(i, 0);
            out.emplace_back((float)p.x, (float)p.y);
        }
        // refine sub-pixel nhẹ
        if (!raw_.empty() && !out.empty()) {
            std::vector<Point2f> tmp = out;
            cornerSubPix(raw_, tmp, Size(3, 3), Size(-1, -1),
                TermCriteria(TermCriteria::EPS | TermCriteria::MAX_ITER, 20, 0.03));
            return tmp;
        }
        return out;
    }

    Line2DCore WidthCore::FitLineCv(const std::vector<Point2f>& inliers) {
        if (inliers.size() < 2) return {};
        Vec4f line;
        fitLine(inliers, line, DIST_L2, 0, 0.01, 0.01);
        Line2DCore L; L.vx = line[0]; L.vy = line[1]; L.x0 = line[2]; L.y0 = line[3];
        return L;
    }

    Line2DCore WidthCore::ransacFitLineParallel(const std::vector<Point2f>& pts,
        std::vector<Point2f>& outInliers) const
    {
        outInliers.clear();
        if (pts.size() < 2 || ransacIter_ <= 0) return {};

        // Ổn định đầu vào
        std::vector<Point2f> p = pts;
        std::sort(p.begin(), p.end(), [](const Point2f& a, const Point2f& b) {
            return (a.y == b.y) ? (a.x < b.x) : (a.y < b.y);
            });

        // Cặp chỉ số cố định seed → deterministic
        auto pairs = precomputePairs((int)p.size(), ransacIter_, 0x075BCD15u /*123456789*/);

        // Chia block cho thread
        const int T = std::max(1u, std::thread::hardware_concurrency());
        const int N = (int)pairs.size();
        const int B = (N + T - 1) / T;

        std::mutex mtx;
        Candidate globalBest; // mặc định cnt=-1 → mọi ứng viên thật đều better

        auto worker = [&](int startIdx, int endIdx) {
            Candidate localBest;
            localBest.cnt = -1;
            for (int it = startIdx; it < endIdx; ++it) {
                int i1 = pairs[it].first, i2 = pairs[it].second;
                const Point2f& P1 = p[i1];
                const Point2f& P2 = p[i2];
                double dx = P2.x - P1.x, dy = P2.y - P1.y;
                double segLen = hypot2(dx, dy);
                if (segLen < 1e-6) continue;

                double a = dy, b = -dx;
                double norm = hypot2(a, b);
                if (norm < 1e-6) continue;
                double c = -(a * P1.x + b * P1.y);

                std::vector<Point2f> inl; inl.reserve(p.size());
                int cnt = 0;
                const double thr = ransacThr_;
                for (const auto& q : p) {
                    double d = std::abs(a * q.x + b * q.y + c) / norm;
                    if (d < thr) { ++cnt; inl.push_back(q); }
                }
                if (!cnt) continue;

                Candidate cand;
                cand.cnt = cnt; cand.segLen = segLen;
                cand.a = a; cand.b = b; cand.c = c;
                cand.i1 = i1; cand.i2 = i2;
                cand.inliers.swap(inl);

                if (betterThan(cand, localBest)) localBest = std::move(cand);
            }

            if (localBest.cnt >= 2) {
                std::lock_guard<std::mutex> lk(mtx);
                if (betterThan(localBest, globalBest)) globalBest = std::move(localBest);
            }
            };

        std::vector<std::thread> ths;
        ths.reserve(T);
        for (int t = 0; t < T; ++t) {
            int s = t * B;
            int e = std::min(N, s + B);
            if (s < e) ths.emplace_back(worker, s, e);
        }
        for (auto& th : ths) th.join();

        if (globalBest.cnt >= 2) {
            outInliers = std::move(globalBest.inliers);
            return FitLineCv(outInliers);
        }
        return {};
    }

    static inline void paramsABC(const Line2DCore& ln, double& A, double& B, double& C) {
        A = ln.vy; B = -ln.vx; C = -(A * ln.x0 + B * ln.y0);
    }

    double WidthCore::DistanceBetweenLines(const Line2DCore& l1, const Line2DCore& l2) {
        double a1, b1, c1, a2, b2, c2;
        paramsABC(l1, a1, b1, c1);
        paramsABC(l2, a2, b2, c2);
        return std::abs(c2 - c1) / hypot2(a1, b1);
    }

    double WidthCore::solveX(const Line2DCore& ln, double y) {
        if (std::abs(ln.vy) < 1e-6) return ln.x0;
        double t = (y - ln.y0) / ln.vy;
        return ln.x0 + ln.vx * t;
    }
    double WidthCore::solveY(const Line2DCore& ln, double x) {
        if (std::abs(ln.vx) < 1e-6) return ln.y0;
        double t = (x - ln.x0) / ln.vx;
        return ln.y0 + ln.vy * t;
    }

    GapResultCore WidthCore::MeasureParallelGap(int numLines,
        GapExtremum extremum,
        LineOrientation orientation,
        SegmentStatType segStat,
        int minInlierRemain)
    {
        GapResultCore R;
        if (raw_.empty()) return R;

        // 1) điểm biên
        auto pts = extractEdgePoints();
        if ((int)pts.size() < 2) return R;

        // 2) lặp RANSAC thu numLines line
        std::vector<Line2DCore> lines;
        std::vector<Point2f> remaining = pts;
        int inlierRemainMin = INT_MAX;

        for (int i = 0; i < numLines; i++) {
            std::vector<Point2f> inl;
            Line2DCore L = ransacFitLineParallel(remaining, inl);
            lines.push_back(L);

            if (!inl.empty()) {
                // remove inliers by threshold to the selected line
                std::vector<Point2f> next;
                next.reserve(remaining.size());
                double A, B, C; paramsABC(L, A, B, C);
                double norm = hypot2(A, B);
                for (const auto& p : remaining) {
                    double d = std::abs(A * p.x + B * p.y + C) / norm;
                    if (d >= ransacThr_) next.push_back(p);
                }
                remaining.swap(next);
            }
            inlierRemainMin = std::min(inlierRemainMin, (int)remaining.size());
            if ((int)remaining.size() < std::max(2, minInlierRemain)) break;
        }
        if ((int)lines.size() < 2) { R.inlierRemain = inlierRemainMin; return R; }

        // 3) lọc theo orientation
        auto keepByOrient = [&](const Line2DCore& l)->bool {
            if (orientation == LineOrientation::Any) return true;
            if (orientation == LineOrientation::Horizontal)
                return std::abs(l.vy) < std::abs(l.vx);
            // Vertical
            return std::abs(l.vx) < std::abs(l.vy);
            };
        std::vector<Line2DCore> filtered;
        for (auto& l : lines) if (keepByOrient(l)) filtered.push_back(l);
        lines.swap(filtered);
        if ((int)lines.size() < 2) { R.inlierRemain = inlierRemainMin; return R; }

        // 4) chọn cặp
        Line2DCore lA{}, lB{};
        switch (extremum) {
        case GapExtremum::Nearest:
        case GapExtremum::Farthest: {
            double best = (extremum == GapExtremum::Farthest) ? -1e300 : 1e300;
            for (size_t i = 0; i < lines.size(); ++i)
                for (size_t j = i + 1; j < lines.size(); ++j) {
                    double d = DistanceBetweenLines(lines[i], lines[j]);
                    if ((extremum == GapExtremum::Farthest && d > best) ||
                        (extremum == GapExtremum::Nearest && d < best)) {
                        best = d; lA = lines[i]; lB = lines[j];
                    }
                }
        } break;
        case GapExtremum::Outermost:
        case GapExtremum::Middle: {
            struct Off { Line2DCore ln; double off; };
            std::vector<Off> offs; offs.reserve(lines.size());
            for (auto& ln : lines) {
                double A, B, C; paramsABC(ln, A, B, C);
                double off = std::abs(C) / hypot2(A, B);
                offs.push_back({ ln,off });
            }
            std::sort(offs.begin(), offs.end(), [](const Off& u, const Off& v) { return u.off < v.off; });
            if (extremum == GapExtremum::Outermost) {
                lA = offs.front().ln; lB = offs.back().ln;
            }
            else {
                if (offs.size() < 2) { R.inlierRemain = inlierRemainMin; return R; }
                size_t m = offs.size() / 2; lA = offs[m - 1].ln; lB = offs[m].ln;
            }
        } break;
        }

        // 5) miền giao để đo
        auto collectInliers = [&](const Line2DCore& L) {
            std::vector<Point2f> out;
            out.reserve(pts.size());
            double A, B, C; paramsABC(L, A, B, C);
            double norm = hypot2(A, B);
            for (auto& p : pts) {
                double d = std::abs(A * p.x + B * p.y + C) / norm;
                if (d < ransacThr_) out.push_back(p);
            }
            return out;
            };
        auto inlA = collectInliers(lA);
        auto inlB = collectInliers(lB);

        double yTop = 0, yBot = 0, xLeft = 0, xRight = 0;
        if (orientation == LineOrientation::Any || orientation == LineOrientation::Vertical) {
            if (inlA.empty() || inlB.empty()) { R.inlierRemain = inlierRemainMin; return R; }
            auto aMinY = min_element(inlA.begin(), inlA.end(), [](auto& p1, auto& p2) {return p1.y < p2.y; })->y;
            auto bMinY = min_element(inlB.begin(), inlB.end(), [](auto& p1, auto& p2) {return p1.y < p2.y; })->y;
            auto aMaxY = max_element(inlA.begin(), inlA.end(), [](auto& p1, auto& p2) {return p1.y < p2.y; })->y;
            auto bMaxY = max_element(inlB.begin(), inlB.end(), [](auto& p1, auto& p2) {return p1.y < p2.y; })->y;
            yTop = std::max(aMinY, bMinY);
            yBot = std::min(aMaxY, bMaxY);
            if (yBot <= yTop) { R.inlierRemain = inlierRemainMin; return R; }
        }
        else {
            auto aMinX = min_element(inlA.begin(), inlA.end(), [](auto& p1, auto& p2) {return p1.x < p2.x; })->x;
            auto bMinX = min_element(inlB.begin(), inlB.end(), [](auto& p1, auto& p2) {return p1.x < p2.x; })->x;
            auto aMaxX = max_element(inlA.begin(), inlA.end(), [](auto& p1, auto& p2) {return p1.x < p2.x; })->x;
            auto bMaxX = max_element(inlB.begin(), inlB.end(), [](auto& p1, auto& p2) {return p1.x < p2.x; })->x;
            xLeft = std::max(aMinX, bMinX);
            xRight = std::min(aMaxX, bMaxX);
            if (xRight <= xLeft) { R.inlierRemain = inlierRemainMin; return R; }
        }

        // 6) đo short/long/med
        double shortPx, longPx, medPx = 0;
        Point pM0, pM1;

        if (orientation == LineOrientation::Any || orientation == LineOrientation::Vertical) {
            double xTopA = solveX(lA, yTop), xTopB = solveX(lB, yTop);
            double xBotA = solveX(lA, yBot), xBotB = solveX(lB, yBot);
            double gapTop = std::abs(xTopB - xTopA);
            double gapBot = std::abs(xBotB - xBotA);

            double yShort = yTop, yLong = yBot;
            shortPx = gapTop; longPx = gapBot;
            if (shortPx > longPx) { std::swap(shortPx, longPx); std::swap(yShort, yLong); }

            double yMid = 0.5 * (yTop + yBot);
            double xMidA = solveX(lA, yMid), xMidB = solveX(lB, yMid);
            medPx = std::abs(xMidB - xMidA);

            if (segStat == SegmentStatType::Shortest) {
                pM0 = Point((int)std::round(solveX(lA, yShort)), (int)std::round(yShort));
                pM1 = Point((int)std::round(solveX(lB, yShort)), (int)std::round(yShort));
            }
            else if (segStat == SegmentStatType::Average) {
                pM0 = Point((int)std::round(xMidA), (int)std::round(yMid));
                pM1 = Point((int)std::round(xMidB), (int)std::round(yMid));
            }
            else {
                pM0 = Point((int)std::round(solveX(lA, yLong)), (int)std::round(yLong));
                pM1 = Point((int)std::round(solveX(lB, yLong)), (int)std::round(yLong));
            }
        }
        else {
            double yLeftA = solveY(lA, xLeft), yLeftB = solveY(lB, xLeft);
            double yRightA = solveY(lA, xRight), yRightB = solveY(lB, xRight);
            double gapLeft = std::abs(yLeftB - yLeftA);
            double gapRight = std::abs(yRightB - yRightA);

            double xShort = xLeft, xLong = xRight;
            shortPx = gapLeft; longPx = gapRight;
            if (shortPx > longPx) { std::swap(shortPx, longPx); std::swap(xShort, xLong); }

            double xMid = 0.5 * (xLeft + xRight);
            double yMidA = solveY(lA, xMid), yMidB = solveY(lB, xMid);
            medPx = std::abs(yMidB - yMidA);

            if (segStat == SegmentStatType::Shortest) {
                pM0 = Point((int)std::round(xShort), (int)std::round(yLeftA));
                pM1 = Point((int)std::round(xShort), (int)std::round(yLeftB));
            }
            else if (segStat == SegmentStatType::Average) {
                pM0 = Point((int)std::round(xMid), (int)std::round(yMidA));
                pM1 = Point((int)std::round(xMid), (int)std::round(yMidB));
            }
            else {
                pM0 = Point((int)std::round(xLong), (int)std::round(yRightA));
                pM1 = Point((int)std::round(xLong), (int)std::round(yRightB));
            }
        }

        R.lines = lines; R.lineA = lA; R.lineB = lB;
        R.lineMid[0] = pM0; R.lineMid[1] = pM1;

        // Pixel
        R.gapMinPx = shortPx; R.gapMedPx = medPx; R.gapMaxPx = longPx;
        // Millimeter
        R.gapMinMm = shortPx * mmPerPixel_; R.gapMedMm = medPx * mmPerPixel_; R.gapMaxMm = longPx * mmPerPixel_;

        R.inlierRemain = inlierRemainMin;
        return R;
    }

} // namespace BeeCpp
