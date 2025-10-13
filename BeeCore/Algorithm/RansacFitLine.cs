using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Algorithm
{
     class Ransac
    {
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

        public static Line2D FitLine(List<Point2f> pts, OrthCornerOptions o,
                              out List<Point2f> contiguous, LineEdge dbg)
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

            double meanFiltered = o.FixMean;
            if (o.AutoMean)
            {
                meanFiltered = FilteredMeanGapTwoStage_NoTrim(
                dts, o.NearQuantile, o.NearMultiplier, out baseMean, out upperCut);
            }

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
        private static Line2D MakeLine(double x, double y, double vx, double vy)
          => new Line2D(vx, vy, x, y);
        private static int NextIndex(ref uint s, int n)
        {
            s ^= s << 13; s ^= s >> 17; s ^= s << 5;
            return (int)(s % (uint)n);
        }

       
    }
}
