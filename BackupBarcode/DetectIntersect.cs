using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeeCore
{
    public struct OrthCornerOptions
    {
        public int MaxCandidateLines;
        public int RansacIterations;
        public double RansacThreshold;
        public int MinInliersPerLine;

        // Cắt đoạn liên tục: splitThr = ContinuityGapFactor * meanFiltered
        public double ContinuityGapFactor;

        // Two-stage mean (no-trim). Nếu NearQuantile <= 0 → dùng median thay thế
        public double NearQuantile;    // vd 0.60  (để tắt đặt = 0)
        public double NearMultiplier;  // vd 2.5

        public double AngleTargetDeg;
        public double AngleToleranceDeg;


        public static OrthCornerOptions Default => new OrthCornerOptions
        {
            MaxCandidateLines = 200,
            RansacIterations = 1200,
            RansacThreshold = 1.5,
            MinInliersPerLine = 20,
            ContinuityGapFactor = 1.2,

            NearQuantile = 0.60,  // bật two-stage mean
            NearMultiplier = 2.5,

            AngleTargetDeg = 90.0,
            AngleToleranceDeg = 20.0
           
        };
    }

    public struct CornerResult
    {
        public bool Found;
        public Line2D L1, L2;
        public Point2f Corner;
        public double AngleDeg;
        public int Inliers1, Inliers2;
        public double Score;
       
    }

    internal struct SegRun
    {
        public Line2D Line;
        public Point2f P0, P1;
        public double T0, T1;
        public double RunLen;
        public int RunCount;
        public List<Point2f> RunPts;
    }

    internal sealed class DebugState
    {
        public List<(Line2D ln, List<Point2f> inl, int totalInl)> RansacAcceptedRaw
            = new List<(Line2D, List<Point2f>, int)>();
        public List<(Line2D ln, int totalInl, string reason)> RansacRejectedRaw
            = new List<(Line2D, int, string)>();
        public List<(Line2D ln, List<Point2f> contiguous)> ContinuityAccepted
            = new List<(Line2D, List<Point2f>)>();
        public List<(Line2D ln, List<Point2f> rawInl, string reason)> ContinuityRejected
            = new List<(Line2D, List<Point2f>, string)>();
        public List<SegRun> RunsKept = new List<SegRun>();
        public List<(SegRun A, SegRun B, string reason, double ang, Point2f P, bool hasP)> PairRejected
            = new List<(SegRun, SegRun, string, double, Point2f, bool)>();
        public CornerResult Best;
    }

    public sealed class DetectIntersect
    {
        // Helper tạo Line2D đúng thứ tự ctor (vx,vy,x,y)
        private static Line2D MakeLine(double x, double y, double vx, double vy)
            => new Line2D(vx, vy, x, y);

        // ===== PUBLIC =====
        public CornerResult FindBestCorner_RansacRuns(Mat gray, Mat edges, OrthCornerOptions? opt = null)
        {
            if (gray == null || gray.Empty()) throw new ArgumentException("gray empty");
            var o = opt ?? OrthCornerOptions.Default;

            if (edges == null || edges.Empty())
            {
                edges = new Mat();
                Cv2.Canny(gray, edges, 50, 150, 3, true);
            }

            var dbg = new DebugState();
            
            List<Point2f> cloud = ExtractEdgePoints(gray, edges);
          
            if (cloud.Count < 2) return new CornerResult { Found = false };

            var remain = new List<Point2f>(cloud);
            var pool = new List<(Line2D ln, List<Point2f> inl)>();

            for (int r = 0; r < o.MaxCandidateLines; r++)
            {
                
                List<Point2f> contiguous;
                Line2D ln = RansacFitLine(remain, o, out contiguous, dbg);

                if (contiguous == null || contiguous.Count < o.MinInliersPerLine)
                {
                  
                    break;
                }

          
                pool.Add((ln, contiguous));

                RemoveSet(remain, contiguous);
                if (remain.Count < 8) break;
            }
            if (pool.Count < 2) return new CornerResult { Found = false};

            var runs = BuildRunsFromRansac(pool);
            dbg.RunsKept.AddRange(runs);
            if (runs.Count < 2) return new CornerResult { Found = false };

            var best = SelectBestPairFromRuns_WithDebug(runs, gray.Size(), o.AngleTargetDeg, o.AngleToleranceDeg, dbg);


           
            return best;
        }

        // ===== CORE =====
        // chỉ giữ 1 đoạn liên tục dài nhất theo t; không merge
        // Không trim 2 đầu: near-mean -> lọc theo baseMean*mult -> meanFiltered
        // Tính mean theo 2 bước (near-quantile -> lọc theo baseMean*mult), không trim 2 đầu
        // Không bridge: dùng splitThr = gapFactor * meanFiltered để cắt thẳng,
        // và loại điểm cô lập trước bằng finalCut = finalMultiplier * meanFiltered (mặc định = gapFactor)
        // Tính meanFiltered từ các Δt theo 2 bước (near-mean -> lọc theo baseMean*mult), KHÔNG trim
        private static double FilteredMeanGapTwoStage_NoTrim(
            List<double> dts,
            double nearQuantile,       // ví dụ 0.60
            double nearMultiplier,     // ví dụ 2.5
            out double baseMean, out double upperCut,
            bool verbose = false)
        {
            baseMean = 1.0; upperCut = 0.0;
            if (dts == null || dts.Count == 0)
            {
                if (verbose) Console.WriteLine("Δt: <empty>");
                return 1.0;
            }

            var arr = new List<double>(dts);
            arr.Sort();

            int n = arr.Count;
            int m = Math.Max(1, (int)Math.Round(nearQuantile * n));

            if (verbose)
            {
                Console.WriteLine($"Δt(sorted) [n={n}]:");
                for (int i = 0; i < n; i++) Console.WriteLine($"  dt[{i}] = {arr[i]:0.###}");
                Console.WriteLine($"nearQuantile={nearQuantile:0.###} → m={m}");
            }

            double sumNear = 0.0;
            for (int i = 0; i < m; i++) sumNear += arr[i];
            baseMean = Math.Max(1e-9, sumNear / m);
            upperCut = baseMean * nearMultiplier;

            if (verbose)
            {
                Console.WriteLine($"near-set mean(baseMean) = {baseMean:0.###}");
                Console.WriteLine($"upperCut = baseMean * {nearMultiplier:0.###} = {upperCut:0.###}");
            }

            double sum = 0.0; int cnt = 0, kept = 0, dropped = 0;
            if (verbose) Console.WriteLine("keep mask (arr[i] <= upperCut):");
            for (int i = 0; i < n; i++)
            {
                bool keep = (arr[i] <= upperCut);
                if (verbose) Console.WriteLine($"  dt[{i}] = {arr[i]:0.###} -> {(keep ? "KEEP" : "DROP")}");
                if (keep) { sum += arr[i]; cnt++; kept++; } else { dropped++; }
            }
            if (cnt == 0) { sum = 0.0; cnt = n; kept = n; dropped = 0; for (int i = 0; i < n; i++) sum += arr[i]; }

            double mean = sum / cnt;
            if (mean < 1e-9) mean = 1.0;

            if (verbose) Console.WriteLine($"kept={kept}, dropped={dropped}, meanFiltered={mean:0.###}");
            return mean;
        }

        private static void KeepLongestRun_StraightCut_NoBridge(
            Line2D ln, List<Point2f> pts,
            double meanFiltered,
            double gapFactor,
            out List<Point2f> bestRunPts,
            out double usedSplitThr,
          
            double? finalMultiplierOpt = null)
        {
            bestRunPts = new List<Point2f>();
            usedSplitThr = 0.0;
            if (pts == null || pts.Count == 0) return;

            // Project theo trục line và sort
            var proj = new List<(double t, Point2f p)>(pts.Count);
            for (int i = 0; i < pts.Count; i++)
            {
                double t = (pts[i].X - ln.X1) * ln.Vx + (pts[i].Y - ln.Y1) * ln.Vy;
                proj.Add((t, pts[i]));
            }
            proj.Sort((a, b) => a.t.CompareTo(b.t));
            if (proj.Count == 1) { bestRunPts.Add(proj[0].p); return; }

            // Hậu-lọc điểm cô lập theo finalCut
            double finalCut = (finalMultiplierOpt ?? gapFactor) * meanFiltered;
            if (proj.Count >= 3)
            {
                var keep = new List<(double t, Point2f p)>(proj.Count);
                keep.Add(proj[0]);
                for (int i = 1; i < proj.Count - 1; i++)
                {
                    double left = proj[i].t - proj[i - 1].t;
                    double right = proj[i + 1].t - proj[i].t;
                    bool isolated = (left > finalCut) && (right > finalCut);
                    if (!isolated) keep.Add(proj[i]);
                }
                keep.Add(proj[proj.Count - 1]);
                if (keep.Count >= 2) proj = keep;
            }

          
            // Cắt thẳng bằng splitThr, lấy run có span lớn nhất (không merge)
            double splitThr = Math.Max(1.0, gapFactor * meanFiltered);
            usedSplitThr = splitThr;

            int bestS = 0, bestE = 0, curS = 0;
            double bestSpan = 0.0;
            for (int i = 1; i < proj.Count; i++)
            {
                double dt = proj[i].t - proj[i - 1].t;
                if (dt > splitThr)
                {
                    double span = proj[i - 1].t - proj[curS].t;
                    if (span > bestSpan) { bestSpan = span; bestS = curS; bestE = i - 1; }
                    curS = i;
                }
            }
            double lastSpan = proj[proj.Count - 1].t - proj[curS].t;
            if (lastSpan > bestSpan) { bestSpan = lastSpan; bestS = curS; bestE = proj.Count - 1; }

            for (int i = bestS; i <= bestE; i++) bestRunPts.Add(proj[i].p);

       
        }

        private Line2D RansacFitLine(List<Point2f> pts, OrthCornerOptions o,
                              out List<Point2f> contiguous, DebugState dbg)
        {
            contiguous = new List<Point2f>();
          

            if (pts == null || pts.Count < 2 || o.RansacIterations <= 0)
            {
                dbg?.RansacRejectedRaw.Add((default(Line2D), 0, "no_points"));
                return default(Line2D);
            }

            // 1) RANSAC (ưu tiên số inliers)
            pts = pts.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();

            const int FIXED_SEED = 987654321;
            var pairs = PrecomputePairs(pts.Count, o.RansacIterations, FIXED_SEED);

            int bestCount = -1;
            double bestSegLen = -1.0;
            double best_a = 0, best_b = 0, best_c = 0;
            List<Point2f> bestInliers = null;

            System.Threading.Tasks.Parallel.ForEach(
                System.Collections.Concurrent.Partitioner.Create(0, o.RansacIterations),
                range =>
                {
                    int l_count = -1; double l_segLen = -1; double l_a = 0, l_b = 0, l_c = 0;
                    List<Point2f> l_inliers = null;

                    for (int it = range.Item1; it < range.Item2; it++)
                    {
                        int i1 = pairs[it].i1, i2 = pairs[it].i2;
                        if (i1 == i2) continue;

                        var p1 = pts[i1];
                        var p2 = pts[i2];

                        double dx = p2.X - p1.X, dy = p2.Y - p1.Y;
                        double segLen = Math.Sqrt(dx * dx + dy * dy);
                        if (segLen < 1e-6) continue;

                        // a x + b y + c = 0  với (a,b) vuông góc vector (dx,dy)
                        double a = dy;
                        double b = -dx;
                        double norm = Math.Sqrt(a * a + b * b);
                        if (norm < 1e-6) continue;
                        double c = -(a * p1.X + b * p1.Y);

                        int cnt = 0;
                        var curr = new List<Point2f>();
                        for (int k = 0; k < pts.Count; k++)
                        {
                            var q = pts[k];
                            double d = Math.Abs(a * q.X + b * q.Y + c) / norm;
                            if (d < o.RansacThreshold) { cnt++; curr.Add(q); }
                        }
                        if (cnt == 0) continue;

                        bool better = false;
                        if (cnt > l_count) better = true;
                        else if (cnt == l_count)
                        {
                            if (segLen > l_segLen) better = true;
                            else if (Math.Abs(segLen - l_segLen) <= 1e-9)
                            {
                                if (a < l_a) better = true;
                                else if (a == l_a)
                                {
                                    if (b < l_b) better = true;
                                    else if (b == l_b && c < l_c) better = true;
                                }
                            }
                        }

                        if (better)
                        {
                            l_count = cnt; l_segLen = segLen;
                            l_a = a; l_b = b; l_c = c;
                            l_inliers = curr;
                        }
                    }

                    if (l_count > -1)
                    {
                        lock (pairs)
                        {
                            bool better = false;
                            if (l_count > bestCount) better = true;
                            else if (l_count == bestCount)
                            {
                                if (l_segLen > bestSegLen) better = true;
                                else if (Math.Abs(l_segLen - bestSegLen) <= 1e-9)
                                {
                                    if (l_a < best_a) better = true;
                                    else if (l_a == best_a)
                                    {
                                        if (l_b < best_b) better = true;
                                        else if (l_b == best_b && l_c < best_c) better = true;
                                    }
                                }
                            }

                            if (better)
                            {
                                bestCount = l_count;
                                bestSegLen = l_segLen;
                                best_a = l_a; best_b = l_b; best_c = l_c;
                                bestInliers = l_inliers;
                            }
                        }
                    }
                });

            if (bestInliers == null || bestInliers.Count < o.MinInliersPerLine)
            {
                dbg?.RansacRejectedRaw.Add((default(Line2D), bestInliers?.Count ?? 0, "ransac_too_few_inliers"));
                return default(Line2D);
            }
          
            // 2) Refine lần 1
            var preliminary = Cv2.FitLine(bestInliers.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
            double Lv = Math.Sqrt(preliminary.Vx * preliminary.Vx + preliminary.Vy * preliminary.Vy);
            if (Lv < 1e-12 || double.IsNaN(Lv))
            {
                dbg?.RansacRejectedRaw.Add((default(Line2D), bestInliers.Count, "fitline_fail_raw"));
                return default(Line2D);
            }
            var lineInit = MakeLine(preliminary.X1, preliminary.Y1, preliminary.Vx, preliminary.Vy);
            dbg?.RansacAcceptedRaw.Add((lineInit, bestInliers, bestInliers.Count));

            // 3) Tính Δt theo lineInit
            var proj = new List<(double t, Point2f p)>(bestInliers.Count);
            for (int i = 0; i < bestInliers.Count; i++)
            {
                double t = (bestInliers[i].X - lineInit.X1) * lineInit.Vx
                         + (bestInliers[i].Y - lineInit.Y1) * lineInit.Vy;
                proj.Add((t, bestInliers[i]));
            }
            proj.Sort((a, b) => a.t.CompareTo(b.t));

            var dts = new List<double>(Math.Max(0, proj.Count - 1));
            for (int i = 1; i < proj.Count; i++)
            {
                double dt = proj[i].t - proj[i - 1].t;
                if (dt > 0) dts.Add(dt);
            }

            // 4) Two-stage (no-trim) mean + log
            double baseMean, upperCut;
            double meanFiltered = FilteredMeanGapTwoStage_NoTrim(
                dts, o.NearQuantile, o.NearMultiplier, out baseMean, out upperCut);

            // 5) Cắt thẳng (NO BRIDGE) + hậu-lọc điểm cô lập ⇒ run dài nhất
            double splitThr;
            KeepLongestRun_StraightCut_NoBridge(
                lineInit, bestInliers,
                meanFiltered,
                o.ContinuityGapFactor,
                out contiguous, out splitThr
                , null);

            if (contiguous == null || contiguous.Count < o.MinInliersPerLine)
            {
                
                dbg?.RansacRejectedRaw.Add((lineInit, bestInliers.Count, "contiguous_too_few"));
                contiguous = new List<Point2f>();
                return default(Line2D);
            }

          
            // 6) Refine lần cuối trên run liên tục
            var finalLine = Cv2.FitLine(contiguous.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
            double Lf = Math.Sqrt(finalLine.Vx * finalLine.Vx + finalLine.Vy * finalLine.Vy);
            if (Lf < 1e-12 || double.IsNaN(Lf))
            {
                dbg?.RansacRejectedRaw.Add((lineInit, contiguous.Count, "fitline_fail_final"));
                return default(Line2D);
            }
            finalLine = MakeLine(finalLine.X1, finalLine.Y1, finalLine.Vx, finalLine.Vy);
            return finalLine;
        }

        private static List<SegRun> BuildRunsFromRansac(List<(Line2D ln, List<Point2f> inl)> pool)
        {
            var runs = new List<SegRun>(pool.Count);
            for (int i = 0; i < pool.Count; i++)
            {
                var r = LongestRunOnLine(pool[i].ln, pool[i].inl);
                if (r.RunCount >= 3 && r.RunLen >= 2.0)
                {
                    runs.Add(r);
                   
                }
            }
            runs.Sort((a, b) => b.RunLen.CompareTo(a.RunLen));
            return runs;
        }

        private static SegRun LongestRunOnLine(Line2D ln, List<Point2f> inliers)
        {
            var sr = new SegRun { Line = ln, RunPts = new List<Point2f>(), RunCount = 0, RunLen = 0, T0 = 0, T1 = 0 };
            if (inliers == null || inliers.Count < 2) return sr;

            var arr = new List<Tuple<double, Point2f>>(inliers.Count);
            for (int i = 0; i < inliers.Count; i++)
            {
                double t = (inliers[i].X - ln.X1) * ln.Vx + (inliers[i].Y - ln.Y1) * ln.Vy;
                arr.Add(Tuple.Create(t, inliers[i]));
            }
            arr.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            var dts = new List<double>(Math.Max(1, arr.Count - 1));
            for (int i = 1; i < arr.Count; i++) dts.Add(arr[i].Item1 - arr[i - 1].Item1);
            dts.Sort();
            double medDt = dts.Count > 0 ? dts[dts.Count / 2] : 1.0;
            double gap = Math.Max(1.0, 3.0 * medDt);

            int bestS = 0, bestE = 0, curS = 0;
            for (int i = 1; i < arr.Count; i++)
            {
                double dt = arr[i].Item1 - arr[i - 1].Item1;
                if (dt > gap)
                {
                    if ((arr[i - 1].Item1 - arr[curS].Item1) > (arr[bestE].Item1 - arr[bestS].Item1))
                    { bestS = curS; bestE = i - 1; }
                    curS = i;
                }
            }
            if ((arr[arr.Count - 1].Item1 - arr[curS].Item1) > (arr[bestE].Item1 - arr[bestS].Item1))
            { bestS = curS; bestE = arr.Count - 1; }

            var run = new List<Point2f>(bestE - bestS + 1);
            for (int i = bestS; i <= bestE; i++) run.Add(arr[i].Item2);

            double t0 = arr[bestS].Item1, t1 = arr[bestE].Item1;
            double len = Math.Max(0.0, t1 - t0);

            sr.RunPts = run; sr.RunCount = run.Count; sr.RunLen = len; sr.T0 = t0; sr.T1 = t1;
            sr.P0 = new Point2f((float)(ln.X1 + t0 * ln.Vx), (float)(ln.Y1 + t0 * ln.Vy));
            sr.P1 = new Point2f((float)(ln.X1 + t1 * ln.Vx), (float)(ln.Y1 + t1 * ln.Vy));
            return sr;
        }
        private static CornerResult SelectBestPairFromRuns_WithDebug(
    List<SegRun> runs, Size imgSize,
    double targetDeg, double tolDeg, DebugState dbg)
        {
            var best = new CornerResult { Found = false };
            if (runs.Count < 2) { dbg.Best = best; return best; }

            var imgRect = new Rect(0, 0, imgSize.Width, imgSize.Height);

            // các biến chấm điểm theo “ưu tiên inliers”
            int bestMinCnt = -1;                // ƯU TIÊN 1: lớn nhất
            int bestSumCnt = -1;                // ƯU TIÊN 2: tổng lớn nhất
            double bestMinLen = double.NegativeInfinity; // ƯU TIÊN 3: độ dài min lớn nhất
            double bestDev = double.PositiveInfinity; // ƯU TIÊN 4: lệch 90° nhỏ nhất
            double bestScore = double.NegativeInfinity; // chỉ để log/hiển thị

            for (int i = 0; i < runs.Count; i++)
            {
                for (int j = i + 1; j < runs.Count; j++)
                {
                    var A = runs[i]; var B = runs[j];

                    // góc giữa 2 hướng
                    double dot = A.Line.Vx * B.Line.Vx + A.Line.Vy * B.Line.Vy;
                    if (dot > 1.0) dot = 1.0; else if (dot < -1.0) dot = -1.0;
                    double ang = Math.Acos(dot) * 180.0 / Math.PI;
                    if (ang > 90.0) ang = 180.0 - ang;

                    double dev = Math.Abs(ang - targetDeg);
                    if (dev > tolDeg)
                    {
                        dbg?.PairRejected.Add((A, B, $"angle_dev>{tolDeg}", ang, new Point2f(), false));
                        continue;
                    }

                    // giao điểm phải nằm trong ảnh
                    Point2f P;
                    if (!Intersect(A.Line, B.Line, out P))
                    {
                        dbg?.PairRejected.Add((A, B, "parallel_or_no_intersect", ang, new Point2f(), false));
                        continue;
                    }
                    if (!imgRect.Contains((Point)P))
                    {
                        dbg?.PairRejected.Add((A, B, "intersection_outside_image", ang, P, true));
                        continue;
                    }

                    // các tiêu chí ưu tiên
                    int minCnt = Math.Min(A.RunCount, B.RunCount);
                    int sumCnt = A.RunCount + B.RunCount;
                    double minLen = Math.Min(A.RunLen, B.RunLen);

                    // điểm tổng hợp chỉ để theo dõi/log — KHÔNG dùng làm ưu tiên
                    double score = minCnt * 1.0
                                 + 0.01 * sumCnt
                                 + 0.001 * minLen
                                 - 0.1 * (dev);   // nhỏ hơn tốt hơn

                   

                    // So sánh theo thứ tự ưu tiên:
                    bool better = false;
                    if (minCnt > bestMinCnt) better = true;
                    else if (minCnt == bestMinCnt)
                    {
                        if (sumCnt > bestSumCnt) better = true;
                        else if (sumCnt == bestSumCnt)
                        {
                            if (minLen > bestMinLen) better = true;
                            else if (Math.Abs(minLen - bestMinLen) <= 1e-9 && dev < bestDev)
                                better = true;
                        }
                    }

                    if (better)
                    {
                        bestMinCnt = minCnt;
                        bestSumCnt = sumCnt;
                        bestMinLen = minLen;
                        bestDev = dev;
                        bestScore = score;

                        best = new CornerResult
                        {
                            Found = true,
                            L1 = A.Line,
                            L2 = B.Line,
                            Corner = P,
                            AngleDeg = ang,
                            Inliers1 = A.RunCount,
                            Inliers2 = B.RunCount,
                            Score = score
                        };
                    }
                }
            }

            dbg.Best = best;
            return best;
        }


        // ===== DEBUG RENDER =====
        private static Mat RenderAllDebug(Mat gray, DebugState dbg)
        {
            Mat canvas = gray.CvtColor(ColorConversionCodes.GRAY2BGR);

            foreach (var rr in dbg.RansacRejectedRaw)
                if (IsValid(rr.ln)) DrawInfiniteLine(canvas, rr.ln, new Scalar(200, 180, 80), 1);

            foreach (var ar in dbg.RansacAcceptedRaw)
                DrawInfiniteLine(canvas, ar.ln, new Scalar(180, 220, 255), 1);

            foreach (var cr in dbg.ContinuityRejected)
                DrawInfiniteLine(canvas, cr.ln, new Scalar(128, 128, 128), 2);

            foreach (var ca in dbg.ContinuityAccepted)
            {
                DrawInfiniteLine(canvas, ca.ln, new Scalar(170, 255, 170), 2);
                foreach (var p in ca.contiguous) canvas.Circle((Point)p, 1, new Scalar(0, 165, 255), -1);
            }

            foreach (var r in dbg.RunsKept)
                Cv2.Line(canvas, (Point)r.P0, (Point)r.P1, new Scalar(0, 140, 255), 2, LineTypes.AntiAlias);

            foreach (var pr in dbg.PairRejected)
            {
                Scalar col = pr.reason.StartsWith("angle") ? new Scalar(255, 255, 0)
                             : pr.reason.StartsWith("parallel") ? new Scalar(0, 255, 255)
                             : new Scalar(0, 0, 255);
                DrawInfiniteLine(canvas, pr.A.Line, col, 1);
                DrawInfiniteLine(canvas, pr.B.Line, col, 1);
                if (pr.hasP) Cv2.Circle(canvas, (Point)pr.P, 3, col, -1);
            }

            if (dbg.Best.Found)
            {
                DrawInfiniteLine(canvas, dbg.Best.L1, new Scalar(0, 255, 0), 2);
                DrawInfiniteLine(canvas, dbg.Best.L2, new Scalar(0, 0, 255), 2);
                Cv2.Circle(canvas, (Point)dbg.Best.Corner, 6, new Scalar(255, 0, 255), -1);
                Cv2.PutText(canvas, $"{dbg.Best.AngleDeg:0.1}°", (Point)(dbg.Best.Corner + new Point2f(8, -8)),
                            HersheyFonts.HersheySimplex, 0.6, new Scalar(255, 0, 255), 1);
            }
            return canvas;
        }

        private static bool IsValid(Line2D ln) => !(Math.Abs(ln.Vx) < 1e-12 && Math.Abs(ln.Vy) < 1e-12);

        // ===== Helpers =====
        private static bool Intersect(Line2D a, Line2D b, out Point2f P)
        {
            double ax = a.X1, ay = a.Y1, avx = a.Vx, avy = a.Vy;
            double bx = b.X1, by = b.Y1, bvx = b.Vx, bvy = b.Vy;

            double A11 = avx, A12 = -bvx;
            double A21 = avy, A22 = -bvy;
            double B1 = bx - ax, B2 = by - ay;

            double det = A11 * A22 - A12 * A21;
            if (Math.Abs(det) < 1e-9) { P = new Point2f(); return false; }

            double t = (B1 * A22 - A12 * B2) / det;
            P = new Point2f((float)(ax + t * avx), (float)(ay + t * avy));
            return true;
        }

        private static void DrawInfiniteLine(Mat img, Line2D ln, Scalar col, int thickness)
        {
            double t1 = -20000, t2 = 20000;
            var p1 = new Point2f((float)(ln.X1 + t1 * ln.Vx), (float)(ln.Y1 + t1 * ln.Vy));
            var p2 = new Point2f((float)(ln.X1 + t2 * ln.Vx), (float)(ln.Y1 + t2 * ln.Vy));
            Cv2.Line(img, (Point)p1, (Point)p2, col, thickness, LineTypes.AntiAlias);
        }

        private static List<Point2f> ExtractEdgePoints(Mat gray, Mat edges)
        {
            try
            {
                Mat nonZero = new Mat();
                Cv2.FindNonZero(edges, nonZero);

                var ptsList = new List<Point>();
                for (int i = 0; i < nonZero.Rows; i++) ptsList.Add(nonZero.At<Point>(i, 0));
                if (ptsList.Count == 0) return new List<Point2f>();

                var ptsf = ptsList.Select(p => new Point2f(p.X, p.Y)).ToArray();

                int maxPoints = 10000;
                Point2f[] sampled = ptsf;
                if (ptsf.Length > maxPoints)
                {
                    int step = Math.Max(1, ptsf.Length / maxPoints);
                    sampled = ptsf.Where((pt, idx) => idx % step == 0).ToArray();
                }

                Cv2.CornerSubPix(
                    gray,
                    sampled,
                    new Size(3, 3),
                    new Size(-1, -1),
                    new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 20, 0.03)
                );
                return sampled.ToList();
            }
            catch
            {
                return new List<Point2f>();
            }
        }

        private static (int i1, int i2)[] PrecomputePairs(int n, int iterations, int seed)
        {
            var pairs = new (int, int)[iterations];
            uint s = unchecked((uint)seed);
            for (int i = 0; i < iterations; i++)
            {
                int a = NextIndex(ref s, n);
                int b; do { b = NextIndex(ref s, n); } while (b == a);
                pairs[i] = (a, b);
            }
            return pairs;
        }

        private static int NextIndex(ref uint s, int n)
        {
            s ^= s << 13; s ^= s >> 17; s ^= s << 5;
            return (int)(s % (uint)n);
        }

        private static void RemoveSet(List<Point2f> cloud, List<Point2f> rm)
        {
            var kill = new HashSet<long>();
            for (int i = 0; i < rm.Count; i++)
            {
                int xi = (int)Math.Round(rm[i].X);
                int yi = (int)Math.Round(rm[i].Y);
                long key = ((long)yi << 32) | (uint)xi;
                kill.Add(key);
            }
            var kept = new List<Point2f>(cloud.Count);
            for (int i = 0; i < cloud.Count; i++)
            {
                int xi = (int)Math.Round(cloud[i].X);
                int yi = (int)Math.Round(cloud[i].Y);
                long key = ((long)yi << 32) | (uint)xi;
                if (!kill.Contains(key)) kept.Add(cloud[i]);
            }
            cloud.Clear();
            cloud.AddRange(kept);
        }
    }
}
