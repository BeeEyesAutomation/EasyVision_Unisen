#include "PitchCore.h"
#include <numeric>
#include <algorithm>
#include <cmath>
#include <limits>

using namespace cv;
using namespace std;

namespace BeeCpp {

    // ---------- utils ----------
    static inline double percentile(std::vector<double> a, double p) {
        if (a.empty()) return 0;
        std::sort(a.begin(), a.end());
        double idx = (p / 100.0) * (a.size() - 1);
        size_t lo = (size_t)floor(idx), hi = (size_t)ceil(idx);
        if (hi == lo) return a[lo];
        double t = idx - lo;
        return a[lo] * (1 - t) + a[hi] * t;
    }

    // ---------- PitchCore ----------
    void PitchCore::setImage(const cv::Mat& edgeGray) {
        CV_Assert(!edgeGray.empty() && edgeGray.type() == CV_8UC1);
        _edgeGray = edgeGray.clone();
    }

    void PitchCore::setMargins(int marginX, int marginY) {
        _marginX = std::max(0, marginX);
        _marginY = std::max(0, marginY);
    }

    void PitchCore::setRejectedPolicy(bool enablePromote, int minNeighborDist) {
        _promoteRejected = enablePromote;
        _rejMinNeighborDist = std::max(0, minNeighborDist);
    }

    void PitchCore::setGaussianSigma(double signalSigma, double centerlineFallbackSigma) {
        if (signalSigma > 0) _sigmaSignal = signalSigma;
        if (centerlineFallbackSigma > 0) _sigmaCenterFallback = centerlineFallbackSigma;
    }

    void PitchCore::setScaleMmPerPx(double mmPerPx) {
        _mmPerPx = (mmPerPx > 0) ? mmPerPx : 1.0;
    }

    std::vector<double> PitchCore::gaussianSmooth(const std::vector<double>& s, double sigma) {
        if (sigma <= 0) return s;
        int r = int(std::ceil(sigma * 3));
        vector<double> k(2 * r + 1);
        double sum = 0, inv2s2 = 1.0 / (2 * sigma * sigma);
        for (int i = -r; i <= r; ++i) { k[i + r] = std::exp(-i * i * inv2s2); sum += k[i + r]; }
        for (double& v : k) v /= sum;
        vector<double> out(s.size(), 0);
        for (int i = 0; i < (int)s.size(); ++i) {
            double acc = 0, ws = 0;
            for (int j = -r; j <= r; ++j) {
                int ix = i + j; if (ix < 0 || ix >= (int)s.size()) continue;
                acc += s[ix] * k[j + r]; ws += k[j + r];
            }
            out[i] = acc / (ws > 1e-9 ? ws : 1.0);
        }
        return out;
    }

    void PitchCore::robustStats(const std::vector<double>& v, double& mean, double& med, double& mn, double& mx, double& stdev) {
        if (v.empty()) { mean = med = mn = mx = stdev = 0; return; }
        mn = *min_element(v.begin(), v.end());
        mx = *max_element(v.begin(), v.end());
        mean = accumulate(v.begin(), v.end(), 0.0) / v.size();
        vector<double> t = v; nth_element(t.begin(), t.begin() + t.size() / 2, t.end());
        med = t[t.size() / 2];
        double s2 = 0; for (double x : v) { double d = x - mean; s2 += d * d; }
        stdev = sqrt(max(0.0, s2 / (v.size() > 1 ? (v.size() - 1) : 1)));
    }

    void PitchCore::autoTune(const std::vector<double>& s, int& mpd, double& prom) {
        vector<double> tmp = s;
        double p10 = percentile(tmp, 10), p90 = percentile(tmp, 90);
        prom = max(1.5, 0.25 * (p90 - p10));
        int W = (int)s.size();
        double bestScore = 1e100; int bestMpd = max(8, W / 30);
        for (int m = max(8, W / 50); m <= W / 5; m += 2) {
            auto dummyReject = vector<int>();
            auto pk = findPeaks1D(s, m, prom, dummyReject);
            if ((int)pk.size() < 4) continue;
            vector<double> pitch;
            for (size_t i = 1; i < pk.size(); ++i) pitch.push_back(double(pk[i] - pk[i - 1]));
            double mean = 0, med = 0, mn = 0, mx = 0, sd = 0;
            robustStats(pitch, mean, med, mn, mx, sd);
            if (mean <= 1e-6) continue;
            double score = sd / mean;
            if (pitch.size() < 5) score += 0.05;
            if (score < bestScore) { bestScore = score; bestMpd = m; }
        }
        mpd = bestMpd;
    }

    std::vector<int> PitchCore::findPeaks1D(const std::vector<double>& s, int minDist, double minProm, std::vector<int>& outRejected) {
        const int N = (int)s.size();
        vector<int> idx;
        if (N < 3) return idx;

        vector<pair<double, int>> cand;
        outRejected.clear();

        for (int i = 1; i < N - 1; ++i) {
            if (s[i] > s[i - 1] && s[i] > s[i + 1]) {
                int l = i; while (l > 0 && s[l] > s[l - 1]) --l;
                int r = i; while (r < N - 1 && s[r] > s[r + 1]) ++r;
                double leftMin = *min_element(s.begin() + l, s.begin() + i);
                double rightMin = *min_element(s.begin() + i + 1, s.begin() + r + 1);
                double prom = s[i] - max(leftMin, rightMin);
                if (prom >= minProm * 0.5)
                    cand.emplace_back(prom, i);
                else
                    outRejected.push_back(i);
            }
        }

        sort(cand.begin(), cand.end(), [](auto& a, auto& b) { return a.first > b.first; });
        vector<int> peaks;
        for (auto& pr : cand) {
            int i = pr.second;
            bool ok = true;
            for (int j : peaks) if (abs(j - i) < minDist) { ok = false; break; }
            if (ok) peaks.push_back(i);
        }
        sort(peaks.begin(), peaks.end());
        return peaks;
    }

    void PitchCore::linearInterpolateInplace(std::vector<double>& arr) {
        const int N = (int)arr.size();
        if (N == 0) return;
        int first = -1;
        for (int i = 0; i < N; ++i) if (std::isfinite(arr[i])) { first = i; break; }
        if (first < 0) return; // toàn NaN
        for (int i = 0; i < first; ++i) arr[i] = arr[first];
        int last = first;
        for (int i = first + 1; i < N; ++i) {
            if (std::isfinite(arr[i])) {
                if (i - last > 1) {
                    double L = arr[last], R = arr[i];
                    int len = i - last;
                    for (int k = 1; k < len; ++k) arr[last + k] = L + (R - L) * (double(k) / double(len));
                }
                last = i;
            }
        }
        for (int i = last + 1; i < N; ++i) arr[i] = arr[last];
    }

    std::vector<double> PitchCore::buildCenterline(const std::vector<double>& ys,
        const std::vector<int>& crestIdx,
        const std::vector<int>& rootIdx,
        double fallbackSigma) {
        const int W = (int)ys.size();
        if (W == 0) return {};

        if (!crestIdx.empty() && !rootIdx.empty()) {
            vector<double> crestEnv(W, std::numeric_limits<double>::quiet_NaN());
            vector<double> rootEnv(W, std::numeric_limits<double>::quiet_NaN());
            for (int i : crestIdx) if (i >= 0 && i < W) crestEnv[i] = ys[i];
            for (int i : rootIdx) if (i >= 0 && i < W) rootEnv[i] = ys[i];
            linearInterpolateInplace(crestEnv);
            linearInterpolateInplace(rootEnv);
            vector<double> center(W);
            for (int i = 0; i < W; ++i) center[i] = 0.5 * (crestEnv[i] + rootEnv[i]);
            return gaussianSmooth(center, 5);
        }
        return gaussianSmooth(ys, fallbackSigma);
    }

    PitchResultCore PitchCore::measure() {
        PitchResultCore R;
        if (_edgeGray.empty()) { R.status = -1; R.message = "No edge image"; return R; }

        const int W0 = _edgeGray.cols, H0 = _edgeGray.rows;

        // ==== 1) Lấy chuỗi đo theo trục đã chọn ====
        vector<double> signal; // y(x) hoặc x(y)
        int idx0 = 0;          // offset (x0 hoặc y0)
        int usableN = 0;

        if (_axis == ScanAxis::X) {
            const int x0 = std::min(std::max(0, _marginX), W0 - 1);
            const int x1 = std::max(x0, W0 - 1 - std::min(std::max(0, _marginX), W0 - 1));
            const int usableW = std::max(0, x1 - x0 + 1);
            if (usableW < 3 || H0 < 3) { R.status = -2; R.message = "Image too small after margins"; return R; }

            signal.assign(usableW, std::numeric_limits<double>::quiet_NaN());
            for (int xx = 0; xx < usableW; ++xx) {
                int x = x0 + xx;
                const uchar* col = _edgeGray.ptr<uchar>(0) + x;
                for (int y = 0; y < H0; ++y, col += _edgeGray.step) {
                    if (*col > 0) {
                        if (y < _marginY || y >(H0 - 1 - _marginY))
                            signal[xx] = std::numeric_limits<double>::quiet_NaN();
                        else
                            signal[xx] = (double)y;
                        break;
                    }
                }
            }
            idx0 = x0; usableN = usableW;
        }
        else { // ScanAxis::Y
            const int y0 = std::min(std::max(0, _marginY), H0 - 1);
            const int y1 = std::max(y0, H0 - 1 - std::min(std::max(0, _marginY), H0 - 1));
            const int usableH = std::max(0, y1 - y0 + 1);
            if (usableH < 3 || W0 < 3) { R.status = -2; R.message = "Image too small after margins"; return R; }

            signal.assign(usableH, std::numeric_limits<double>::quiet_NaN());
            for (int yy = 0; yy < usableH; ++yy) {
                int y = y0 + yy;
                const uchar* row = _edgeGray.ptr<uchar>(y);
                for (int x = 0; x < W0; ++x) {
                    if (row[x] > 0) {
                        if (x < _marginX || x >(W0 - 1 - _marginX))
                            signal[yy] = std::numeric_limits<double>::quiet_NaN();
                        else
                            signal[yy] = (double)x;
                        break;
                    }
                }
            }
            idx0 = y0; usableN = usableH;
        }

        // Nội suy và làm mượt (sigma cấu hình)
        linearInterpolateInplace(signal);
        if (usableN == 0 || !std::isfinite(signal[0])) { R.status = -2; R.message = "Edge not found"; return R; }
        auto sSmooth = gaussianSmooth(signal, _sigmaSignal);

        // ==== 2) Tín hiệu crest/root & auto-tune ====
        vector<double> sigCrest(usableN), sigRoot(usableN);
        for (int i = 0; i < usableN; ++i) { sigCrest[i] = -sSmooth[i]; sigRoot[i] = sSmooth[i]; }

        int mpd = 0; double prom = 0; autoTune(sigCrest, mpd, prom);
        R.usedMinPeakDist = mpd; R.usedMinProm = prom;

        vector<int> rejectedCrestIdx;
        auto idxC_local = findPeaks1D(sigCrest, mpd, prom, rejectedCrestIdx);
        auto dummyReject = vector<int>();
        auto idxR_local = findPeaks1D(sigRoot, mpd, prom * 0.8, dummyReject);

        // Promote crest bị reject nếu bật
        if (_promoteRejected && _rejMinNeighborDist > 0) {
            vector<int> cur = idxC_local;
            auto hasClose = [&](int pos)->bool {
                for (int j : cur) if (std::abs(j - pos) < _rejMinNeighborDist) return true;
                return false;
                };
            for (int r : rejectedCrestIdx) if (!hasClose(r)) cur.push_back(r);
            sort(cur.begin(), cur.end());
            idxC_local.swap(cur);
        }

        // ==== 3) Map index → toạ độ ảnh, build peaks ====
        auto addCrest = [&](int iLocal) {
            if (_axis == ScanAxis::X) {
                int x = idx0 + iLocal;
                int y = (int)std::lround(sSmooth[iLocal]);
                R.crests.push_back({ x, y, sigCrest[iLocal] });
            }
            else {
                int y = idx0 + iLocal;
                int x = (int)std::lround(sSmooth[iLocal]);
                R.crests.push_back({ x, y, sigCrest[iLocal] });
            }
            };
        auto addRoot = [&](int iLocal) {
            if (_axis == ScanAxis::X) {
                int x = idx0 + iLocal;
                int y = (int)std::lround(sSmooth[iLocal]);
                R.roots.push_back({ x, y, sigRoot[iLocal] });
            }
            else {
                int y = idx0 + iLocal;
                int x = (int)std::lround(sSmooth[iLocal]);
                R.roots.push_back({ x, y, sigRoot[iLocal] });
            }
            };
        for (int i : idxC_local) addCrest(i);
        for (int i : idxR_local) addRoot(i);

        for (int i : rejectedCrestIdx) {
            if (_axis == ScanAxis::X) {
                int x = idx0 + i, y = (int)std::lround(sSmooth[i]);
                R.rejected_crests.push_back({ x, y, sigCrest[i] });
            }
            else {
                int y = idx0 + i, x = (int)std::lround(sSmooth[i]);
                R.rejected_crests.push_back({ x, y, sigCrest[i] });
            }
        }

        // ==== 4) Pitch theo loại peak (PX) ====
        if ((int)R.crests.size() >= 2) {
            for (size_t i = 1; i < R.crests.size(); ++i)
                R.pitches.push_back((_axis == ScanAxis::X) ? double(R.crests[i].x - R.crests[i - 1].x)
                    : double(R.crests[i].y - R.crests[i - 1].y));
        }
        if ((int)R.roots.size() >= 2) {
            for (size_t i = 1; i < R.roots.size(); ++i)
                R.pitches_root.push_back((_axis == ScanAxis::X) ? double(R.roots[i].x - R.roots[i - 1].x)
                    : double(R.roots[i].y - R.roots[i - 1].y));
        }

        // ==== 5) Dựng centerline & map heights (PX) ====
        auto centerLocal = buildCenterline(sSmooth, idxC_local, idxR_local, _sigmaCenterFallback);

        R.crest_cl_x.reserve(R.crests.size());
        R.crest_cl_y.reserve(R.crests.size());
        R.root_cl_x.reserve(R.roots.size());
        R.root_cl_y.reserve(R.roots.size());

        if (_axis == ScanAxis::X) {
            // centerLocal = y_at_x
            for (const auto& c : R.crests) {
                int i = c.x - idx0;
                double cy = (i >= 0 && i < usableN) ? centerLocal[i] : (double)c.y;
                R.crest_cl_x.push_back(c.x);
                R.crest_cl_y.push_back(cy);
            }
            for (const auto& r : R.roots) {
                int i = r.x - idx0;
                double cy = (i >= 0 && i < usableN) ? centerLocal[i] : (double)r.y;
                R.root_cl_x.push_back(r.x);
                R.root_cl_y.push_back(cy);
            }
            R.crest_heights.reserve(R.crests.size());
            for (size_t i = 0; i < R.crests.size(); ++i) {
                double h = R.crest_cl_y[i] - R.crests[i].y; if (h < 0) h = 0; R.crest_heights.push_back(h);
            }
            R.root_heights.reserve(R.roots.size());
            for (size_t i = 0; i < R.roots.size(); ++i) {
                double h = R.roots[i].y - R.root_cl_y[i]; if (h < 0) h = 0; R.root_heights.push_back(h);
            }
        }
        else {
            // centerLocal = x_at_y
            for (const auto& c : R.crests) {
                int i = c.y - idx0;
                double cx = (i >= 0 && i < usableN) ? centerLocal[i] : (double)c.x;
                R.crest_cl_x.push_back((int)std::lround(cx));
                R.crest_cl_y.push_back((double)c.y);
            }
            for (const auto& r : R.roots) {
                int i = r.y - idx0;
                double cx = (i >= 0 && i < usableN) ? centerLocal[i] : (double)r.x;
                R.root_cl_x.push_back((int)std::lround(cx));
                R.root_cl_y.push_back((double)r.y);
            }
            R.crest_heights.reserve(R.crests.size());
            for (size_t i = 0; i < R.crests.size(); ++i) {
                double h = (double)R.crest_cl_x[i] - R.crests[i].x; if (h < 0) h = 0; R.crest_heights.push_back(h);
            }
            R.root_heights.reserve(R.roots.size());
            for (size_t i = 0; i < R.roots.size(); ++i) {
                double h = R.roots[i].x - (double)R.root_cl_x[i]; if (h < 0) h = 0; R.root_heights.push_back(h);
            }
        }

        // ==== 6) Quy đổi ra MM & thống kê MM ====
        R.scale_mm_per_px = _mmPerPx;

        auto scaleVec = [&](const std::vector<double>& src, std::vector<double>& dst) {
            dst.clear(); dst.reserve(src.size());
            for (double v : src) dst.push_back(v * _mmPerPx);
            };

        scaleVec(R.pitches, R.pitches_mm);
        scaleVec(R.pitches_root, R.pitches_root_mm);
        scaleVec(R.crest_heights, R.crest_heights_mm);
        scaleVec(R.root_heights, R.root_heights_mm);

        robustStats(R.pitches_mm, R.pitch_mean_mm, R.pitch_median_mm, R.pitch_min_mm, R.pitch_max_mm, R.pitch_std_mm);
        robustStats(R.pitches_root_mm, R.pitch_root_mean_mm, R.pitch_root_median_mm, R.pitch_root_min_mm, R.pitch_root_max_mm, R.pitch_root_std_mm);
        robustStats(R.crest_heights_mm, R.crest_h_mean_mm, R.crest_h_med_mm, R.crest_h_min_mm, R.crest_h_max_mm, R.crest_h_std_mm);
        robustStats(R.root_heights_mm, R.root_h_mean_mm, R.root_h_med_mm, R.root_h_min_mm, R.root_h_max_mm, R.root_h_std_mm);

        // ==== 7) Debug image ====
        //Mat dbg; cvtColor(_edgeGray, dbg, COLOR_GRAY2BGR);

        //// centerline polyline
        //if (_axis == ScanAxis::X) {
        //    vector<Point> cpoly; cpoly.reserve(usableN);
        //    for (int i = 0; i < usableN; ++i) cpoly.emplace_back(idx0 + i, (int)std::lround(centerLocal[i]));
        //    polylines(dbg, cpoly, false, Scalar(255, 0, 255), 2, LINE_AA);
        //}
        //else {
        //    vector<Point> cpoly; cpoly.reserve(usableN);
        //    for (int i = 0; i < usableN; ++i) cpoly.emplace_back((int)std::lround(centerLocal[i]), idx0 + i);
        //    polylines(dbg, cpoly, false, Scalar(255, 0, 255), 2, LINE_AA);
        //}

        //// peaks
        //for (auto& p : R.crests) circle(dbg, Point(p.x, p.y), 4, Scalar(0, 0, 255), -1, LINE_AA);
        //for (auto& p : R.roots)  circle(dbg, Point(p.x, p.y), 3, Scalar(255, 255, 0), -1, LINE_AA);

        //// rejected crest: X trắng
        //for (auto& p : R.rejected_crests) {
        //    line(dbg, Point(p.x - 4, p.y - 4), Point(p.x + 4, p.y + 4), Scalar(255, 255, 255), 1, LINE_AA);
        //    line(dbg, Point(p.x - 4, p.y + 4), Point(p.x + 4, p.y - 4), Scalar(255, 255, 255), 1, LINE_AA);
        //}

        //// pitch segments (màu như cũ)
        //if (R.crests.size() >= 2) {
        //    for (size_t i = 1; i < R.crests.size(); ++i) {
        //        Point a(R.crests[i - 1].x, R.crests[i - 1].y), b(R.crests[i].x, R.crests[i].y);
        //        line(dbg, a, b, Scalar(0, 165, 255), 2, LINE_AA);
        //    }
        //}
        //if (R.roots.size() >= 2) {
        //    for (size_t i = 1; i < R.roots.size(); ++i) {
        //        Point a(R.roots[i - 1].x, R.roots[i - 1].y), b(R.roots[i].x, R.roots[i].y);
        //        line(dbg, a, b, Scalar(0, 200, 0), 2, LINE_AA);
        //    }
        //}

        //// height segments
        //for (size_t i = 0; i < R.crests.size(); ++i) {
        //    Point p(R.crests[i].x, R.crests[i].y);
        //    Point q(R.crest_cl_x[i], (int)std::lround(R.crest_cl_y[i]));
        //    line(dbg, p, q, Scalar(200, 0, 200), 2, LINE_AA);
        //}
        //for (size_t i = 0; i < R.roots.size(); ++i) {
        //    Point p(R.roots[i].x, R.roots[i].y);
        //    Point q(R.root_cl_x[i], (int)std::lround(R.root_cl_y[i]));
        //    line(dbg, p, q, Scalar(0, 200, 200), 2, LINE_AA);
        //}

        // title
       /* char title[320];
        snprintf(title, sizeof(title),
            "[%s] Crest N=%zu  P(mm med/mean)=%.3f/%.3f sd=%.3f | Root N=%zu  rP(mm med/mean)=%.3f/%.3f sd=%.3f | hC(mm med/mean)=%.3f/%.3f hR(mm med/mean)=%.3f/%.3f  sigma=%.1f/%.1f  scale=%.4f",
            (_axis == ScanAxis::X ? "scanX" : "scanY"),
            R.crests.size(), R.pitch_median_mm, R.pitch_mean_mm, R.pitch_std_mm,
            R.roots.size(), R.pitch_root_median_mm, R.pitch_root_mean_mm, R.pitch_root_std_mm,
            R.crest_h_med_mm, R.crest_h_mean_mm, R.root_h_med_mm, R.root_h_mean_mm,
            _sigmaSignal, _sigmaCenterFallback, _mmPerPx);
        putText(dbg, title, Point(10, 22), FONT_HERSHEY_SIMPLEX, 0.5, Scalar(50, 255, 50), 2, LINE_AA);

        R.debugBGR = std::move(dbg);*/

        if ((int)R.crests.size() < 2 && (int)R.roots.size() < 2) {
            R.status = -3; R.message = "Too few peaks";
        }
        else if (R.message.empty()) {
            R.message = "OK";
        }
        return R;
    }

} // namespace BeeCpp
