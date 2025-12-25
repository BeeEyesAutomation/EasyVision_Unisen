using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeeCore.Algorithm
{
    public enum AxisOption { Both, OnlyX, OnlyY }

    // Bạn đã có enum LinePairStrategy ở nơi khác. Nếu cần, khai báo lại:


    public class FilletCornerMeasure
    {
        // == Config ==
        public LinePairStrategy PairStrategy { get; set; } = LinePairStrategy.StrongPlusContourOrth;
        public double PerpAngleToleranceDeg { get; set; } = 1.0; // lọc tiếp tuyến gần vuông góc
        public double RansacThreshold { get; set; } = 2.0; // px
        public int RansacIterations = 200;

        private class LineCandidate
        {
            public LineAB L;
            public List<Point2f> Inliers;
            public int Count => Inliers?.Count ?? 0;
            public double AngleDeg; // hướng P1->P2 (0..180)
        }

        // ====== Line ax+by+c=0 (chuẩn hóa), kèm 2 điểm để vẽ ======
        public struct LineAB
        {
            public float A, B, C;     // ax + by + c = 0, |(A,B)| = 1
            public Point2f P1, P2;    // hai điểm trên line
        }

        public Line2D ToLine2D(LineAB L)
        {
            var dx = L.P2.X - L.P1.X;
            var dy = L.P2.Y - L.P1.Y;
            double len = Math.Sqrt(dx * dx + dy * dy);
            if (len < 1e-6) len = 1;
            return new Line2D((float)(dx / len), (float)(dy / len), L.P1.X, L.P1.Y);
        }

        public struct Result
        {
            public double Dx, Dy;
            public double Sx, Sy;
            public double AI, BI;
            public double AO, BO;
            public double AngleAOB;
            public Point2f Corner;    // O
            public Point2f Touch;     // I
            public Point2f A, B;      // giao tuyến với contour gần O (hoặc ép theo yêu cầu)
            public Point2f FootX, FootY;
            public LineAB LineH, LineV;
            // NEW: góc tuyệt đối & có hướng so với trục ảnh
            public double ThetaOA_Deg;       // [0..360)
            public double ThetaOB_Deg;       // [0..360)
            public double AtoB_CCW_Deg;      // [0..360)
            public double ThetaBisector_Deg; // [0..360)
        }
        public int MaxLineCandidates = 4;
        // == API chính ==
        public Result Measure(Mat image, Mat edges, bool debugDraw = false, AxisOption axis = AxisOption.Both, bool signedDistance = false)
        {
            Mat gray = image.Channels() == 1 ? image : image.CvtColor(ColorConversionCodes.BGR2GRAY);

            // contour lớn nhất
            Cv2.FindContours(edges, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxNone);
            if (contours.Length == 0) return new Result();
            var contour = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();

            // Edge points để fit line
            var pts = ExtractEdgePoints(gray, edges);

            var cands = new List<LineCandidate>();
            var remaining = new List<Point2f>(pts);

            for (int i = 0; i < MaxLineCandidates; i++)
            {
                var L = RansacFitLine(remaining, out var inliers);
                if (inliers == null || inliers.Count < 2) break;

                // refit bằng inliers
                Line2D f = Cv2.FitLine(inliers.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
                var vx = f.Vx; var vy = f.Vy; var x0 = f.X1; var y0 = f.Y1;
                float a = (float)vy, b = -(float)vx, c = -(a * (float)x0 + b * (float)y0);
                float nrm = (float)Math.Sqrt(a * a + b * b);
                if (nrm > 1e-6f) { a /= nrm; b /= nrm; c /= nrm; }
                var P1 = new Point2f((float)x0, (float)y0);
                var P2 = new Point2f((float)x0 + (float)vx * 1000f, (float)y0 + (float)vy * 1000f);

                var Lrefit = new LineAB { A = a, B = b, C = c, P1 = P1, P2 = P2 };

                cands.Add(new LineCandidate
                {
                    L = Lrefit,
                    Inliers = inliers,
                    AngleDeg = LineDirectionDeg(Lrefit)
                });

                // loại bỏ inliers để tìm line khác
                remaining = remaining.Except(inliers).ToList();
                if (remaining.Count < 2) break;
            }

            // non-max suppression theo hướng
            double dirMergeThresh = 10.0;
            cands = cands
                .GroupBy(c => Math.Round(c.AngleDeg / dirMergeThresh))
                .Select(g => g.OrderByDescending(x => x.Count).First())
                .ToList();

            if (cands.Count < 1) return new Result();

            //  Chọn lineH & lineV theo chiến lược 
            LineAB lineH = default, lineV = default;
            bool foundPair = false;

            // NEW: điểm giao “bắt buộc” nếu có (ví dụ farthest point A/B)
            Point2f? forceIntersectH = null;
            Point2f? forceIntersectV = null;

            bool IsHorizontal(LineCandidate c)
                => (Math.Abs(c.AngleDeg - 0.0) <= 30.0) || (Math.Abs(c.AngleDeg - 180.0) <= 30.0);
            bool IsVertical(LineCandidate c)
                => Math.Abs(c.AngleDeg - 90.0) <= 30.0;

            if (PairStrategy == LinePairStrategy.StrongPlusContourOrth)
            {
                var best = cands.OrderByDescending(c => c.Count).First();

                // (1) lọc theo tiếp tuyến ~ vuông góc (giữ đúng cạnh)
                var oriented = EstimateContourOrientations(contour);
                double bestAng = best.AngleDeg;
                double target1 = NormalizeDeg(bestAng + 90.0);
                double target2 = NormalizeDeg(bestAng - 90.0);

                var filtered = new List<Point2f>();
                foreach (var (p, theta) in oriented)
                    if (AngleDiff180(theta, target1) <= PerpAngleToleranceDeg ||
                        AngleDiff180(theta, target2) <= PerpAngleToleranceDeg)
                        filtered.Add(p);

                IEnumerable<Point2f> candPts = (filtered.Count >= 3)
                    ? filtered
                    : contour.Select(pt => new Point2f(pt.X, pt.Y));

                // (2) vector phương dọc của line mạnh: t1 = (-B, A)
                var t1 = Normalize(new Point2f(-best.L.B, best.L.A));

                // (3) chọn điểm xa nhất THEO PHƯƠNG CỦA t1 (max |projection|)
                double maxProj = double.NegativeInfinity;
                double minProj = double.PositiveInfinity;
                Point2f maxP = default, minP = default;

                foreach (var p in candPts)
                {
                    double proj = p.X * t1.X + p.Y * t1.Y; // chiếu song song theo phương của line mạnh
                    if (proj > maxProj) { maxProj = proj; maxP = p; }
                    if (proj < minProj) { minProj = proj; minP = p; }
                }

                // chọn đầu mút có |proj| lớn hơn
                Point2f farP = (Math.Abs(maxProj) >= Math.Abs(minProj)) ? maxP : minP;

                // (4) dựng line còn lại: ÉP 90° với best và đi qua farP
                var other = BuildPerpLineThroughPoint(best.L, farP);
                var otherCand = new LineCandidate { L = other, AngleDeg = LineDirectionDeg(other) };

                // (5) gán nhãn H/V + ép giao đúng farP cho line tương ứng
                if (IsHorizontal(best) && IsVertical(otherCand))
                {
                    lineH = best.L; lineV = other;
                    forceIntersectV = farP;      // A = farP
                }
                else if (IsHorizontal(otherCand) && IsVertical(best))
                {
                    lineH = other; lineV = best.L;
                    forceIntersectH = farP;      // B = farP
                }
                else
                {
                    Func<double, double> distTo0or180 = a0 => Math.Min(Math.Abs(a0 - 0), Math.Abs(a0 - 180));
                    bool hIsBest = distTo0or180(best.AngleDeg) <= distTo0or180(otherCand.AngleDeg);
                    if (hIsBest) { lineH = best.L; lineV = other; forceIntersectV = farP; }
                    else { lineH = other; lineV = best.L; forceIntersectH = farP; }
                }

                foundPair = true;
            }
            else if (PairStrategy == LinePairStrategy.StrongPlusOrth)
            {
                SelectStrongPlusOrth(cands, out lineH, out lineV, IsHorizontal, IsVertical, out foundPair);
            }
            else // BothMaxInliers
            {
                double bestScore = double.NegativeInfinity;
                for (int i = 0; i < cands.Count; i++)
                {
                    for (int j = i + 1; j < cands.Count; j++)
                    {
                        var ci = cands[i];
                        var cj = cands[j];

                        double angDiff90 = AngleDiffTo90(ci.L, cj.L);
                        if (angDiff90 > 30.0) continue;

                        int sumInliers = ci.Count + cj.Count;
                        double score = sumInliers - 50.0 * angDiff90;

                        if (score > bestScore)
                        {
                            bestScore = score;
                            foundPair = true;

                            if (IsHorizontal(ci) && IsVertical(cj))
                            {
                                lineH = ci.L; lineV = cj.L;
                            }
                            else if (IsHorizontal(cj) && IsVertical(ci))
                            {
                                lineH = cj.L; lineV = ci.L;
                            }
                            else
                            {
                                Func<double, double> distTo0or180 = a => Math.Min(Math.Abs(a - 0), Math.Abs(a - 180));
                                var hLine = (distTo0or180(ci.AngleDeg) <= distTo0or180(cj.AngleDeg)) ? ci : cj;
                                lineH = hLine.L;
                                lineV = ReferenceEquals(hLine, ci) ? cj.L : ci.L;
                            }
                        }
                    }
                }
            }

            if (!foundPair) return new Result();

            //  Giao điểm O 
            var O = Intersect(lineH, lineV);

            //  CHUẨN HÓA PHÁP TUYẾN: luôn hướng từ O vào trong contour 
            lineH = EnsureNormalInwardsFromO(lineH, contour, O);
            lineV = EnsureNormalInwardsFromO(lineV, contour, O);

            //  Tính phân giác từ 2 pháp tuyến đã chuẩn hoá 
            var nH = Normalize(new Point2f(lineH.A, lineH.B)); // đã hướng vào trong
            var nV = Normalize(new Point2f(lineV.A, lineV.B)); // đã hướng vào trong
            var u = Normalize(new Point2f(nH.X + nV.X, nH.Y + nV.Y));

            // tìm I dọc theo phân giác “vào trong”
            Point2f I = default; double bestPerp = double.MaxValue; double bestT = -1;
            for (int s = 0; s < 2; s++)
            {
                var dir = (s == 0) ? u : new Point2f(-u.X, -u.Y);
                foreach (var p in contour)
                {
                    var v = new Point2f(p.X - O.X, p.Y - O.Y);
                    double t = v.X * dir.X + v.Y * dir.Y;
                    if (t <= 0) continue;
                    double perp = Math.Abs(v.X * dir.Y - v.Y * dir.X);
                    if (perp < bestPerp) { bestPerp = perp; I = new Point2f(p.X, p.Y); bestT = t; }
                }
            }
            if (bestT < 0) return new Result();

            // A, B (ưu tiên điểm ép nếu có)
            Point2f A, B; bool okA = false, okB = false;
            if (forceIntersectV.HasValue) { A = forceIntersectV.Value; okA = true; }
            else A = NearestIntersectionToPoint(lineV, contour, O, out okA);

            if (forceIntersectH.HasValue) { B = forceIntersectH.Value; okB = true; }
            else B = NearestIntersectionToPoint(lineH, contour, O, out okB);

            if (!okA) A = ClosestPointToLineNear(contour, lineV, O);
            if (!okB) B = ClosestPointToLineNear(contour, lineH, O);

            // Chân vuông góc
            var Ix = FootOfPerpendicular(I, lineV);
            var Iy = FootOfPerpendicular(I, lineH);

            // Khoảng cách có dấu và tuyệt đối
            double sx = lineV.A * I.X + lineV.B * I.Y + lineV.C;
            double sy = lineH.A * I.X + lineH.B * I.Y + lineH.C;
            double dx = Math.Abs(sx), dy = Math.Abs(sy);
            if (axis == AxisOption.OnlyX) { dy = 0; sy = 0; }
            if (axis == AxisOption.OnlyY) { dx = 0; sx = 0; }

            // Độ dài cung ngắn hơn
            double AI = ArcLengthBetween(contour, A, I);
            double BI = ArcLengthBetween(contour, B, I);

            // Khoảng cách đoạn thẳng AO, BO
            double AO = Math.Sqrt((A.X - O.X) * (A.X - O.X) + (A.Y - O.Y) * (A.Y - O.Y));
            double BO = Math.Sqrt((B.X - O.X) * (B.X - O.X) + (B.Y - O.Y) * (B.Y - O.Y));

            // Góc AOB
            var vOA = new Point2f(A.X - O.X, A.Y - O.Y);
            var vOB = new Point2f(B.X - O.X, B.Y - O.Y);
            double dot = vOA.X * vOB.X + vOA.Y * vOB.Y;
            double len1 = Math.Sqrt(vOA.X * vOA.X + vOA.Y * vOA.Y);
            double len2 = Math.Sqrt(vOB.X * vOB.X + vOB.Y * vOB.Y);
            double angleAOB = 0;
            if (len1 > 1e-6 && len2 > 1e-6)
            {
                double cosTheta = dot / (len1 * len2);
                cosTheta = Math.Max(-1.0, Math.Min(1.0, cosTheta));
                angleAOB = Math.Acos(cosTheta) * 180.0 / Math.PI;
            }
            // === NEW: Góc tuyệt đối của OA, OB và phân giác so với trục +X ảnh ===
            var uOA = Normalize(vOA);
            var uOB = Normalize(vOB);

            double thetaOA = Angle360FromVec(uOA);
            double thetaOB = Angle360FromVec(uOB);

            // Góc có hướng từ OA -> OB theo CCW (trong hệ ảnh)
            double a2b_ccw = DeltaCCW360(thetaOA, thetaOB);

            // Phân giác của góc nhỏ hơn giữa OA và OB
            Point2f uBis;
            {
                // tổng 2 vector đơn vị cho phân giác (nếu ~đối nhau, fallback về OA)
                var sum = new Point2f(uOA.X + uOB.X, uOA.Y + uOB.Y);
                float n = (float)Math.Sqrt(sum.X * sum.X + sum.Y * sum.Y);
                uBis = (n > 1e-6f) ? new Point2f(sum.X / n, sum.Y / n) : uOA;
            }
            double thetaBis = Angle360FromVec(uBis);
            if (debugDraw)
            {
                using (var dbg = image.Channels() == 1 ? image.CvtColor(ColorConversionCodes.GRAY2BGR) : image.Clone())
                {
                    Cv2.DrawContours(dbg, new[] { contour }, -1, Scalar.Yellow, 1);
                    DrawLine(dbg, lineH, Scalar.Orange, 2);
                    DrawLine(dbg, lineV, Scalar.Orange, 2);
                    Cv2.Circle(dbg, (Point)O, 5, Scalar.Cyan, -1);
                    Cv2.Circle(dbg, (Point)I, 5, Scalar.Lime, -1);
                    Cv2.Circle(dbg, (Point)A, 5, Scalar.Red, -1);
                    Cv2.Circle(dbg, (Point)B, 5, Scalar.Red, -1);

                    Cv2.Line(dbg, (Point)A, (Point)O, Scalar.Cyan, 1);
                    Cv2.Line(dbg, (Point)B, (Point)O, Scalar.Cyan, 1);
                    Cv2.PutText(dbg, $"AO={AO:0.##}", (Point)((A + O) * 0.5f), HersheyFonts.HersheySimplex, 0.5, Scalar.Cyan, 1);
                    Cv2.PutText(dbg, $"BO={BO:0.##}", (Point)((B + O) * 0.5f), HersheyFonts.HersheySimplex, 0.5, Scalar.Cyan, 1);
                    Cv2.PutText(dbg, $"AOB={angleAOB:0.##}°", new Point((int)O.X + 18, (int)O.Y - 10),
                                HersheyFonts.HersheySimplex, 0.5, Scalar.White, 1);

                    Cv2.ImShow("corner-measure", dbg);
                }
            }

            return new Result
            {
                Dx = signedDistance ? sx : dx,
                Dy = signedDistance ? sy : dy,
                Sx = sx,
                Sy = sy,
                AI = AI,
                BI = BI,
                AO = AO,
                BO = BO,
                AngleAOB = angleAOB,
                Corner = O,
                Touch = I,
                A = A,
                B = B,
                FootX = Ix,
                FootY = Iy,
                LineH = lineH,
                LineV = lineV,
                ThetaOA_Deg = thetaOA,
                ThetaOB_Deg = thetaOB,
                AtoB_CCW_Deg = a2b_ccw,
                ThetaBisector_Deg = thetaBis
            };
        }

        // ==== Helpers ====
        private static double Angle360FromVec(Point2f v)
        {
            double ang = Math.Atan2(v.Y, v.X) * 180.0 / Math.PI; // Y ảnh hướng xuống
            if (ang < 0) ang += 360.0;
            return ang; // [0..360)
        }

        private static double DeltaCCW360(double fromDeg, double toDeg)
        {
            double d = toDeg - fromDeg;
            while (d < 0) d += 360.0;
            while (d >= 360.0) d -= 360.0;
            return d; // [0..360)
        }
        // Bảo đảm pháp tuyến (A,B) của line hướng từ O vào trong contour (dùng PointPolygonTest)
        private FilletCornerMeasure.LineAB EnsureNormalInwardsFromO(
            FilletCornerMeasure.LineAB L, Point[] contour, Point2f O)
        {
            var n = new Point2f(L.A, L.B);
            if (Math.Abs(n.X) + Math.Abs(n.Y) < 1e-12) return L;

            float[] steps = { 3f, 5f }; // thử nhiều bước nhỏ
            bool needFlip = false;
            foreach (var s in steps)
            {
                var probe = new Point2f(O.X + n.X * s, O.Y + n.Y * s);
                double where = Cv2.PointPolygonTest(contour, probe, false); // >0 trong, =0 biên, <0 ngoài
                if (where < 0) { needFlip = true; break; } // đang chĩa ra ngoài -> cần lật
                if (where > 0) { needFlip = false; break; } // đã chĩa vào trong
                // where == 0 -> thử tiếp step khác
            }

            if (needFlip)
            {
                L.A = -L.A; L.B = -L.B; L.C = -L.C;
            }
            return L;
        }

        private FilletCornerMeasure.LineAB BuildPerpLineThroughPoint(
            FilletCornerMeasure.LineAB best, Point2f passPoint)
        {
            // normal của line còn lại n2 = direction của best (tức là (-B1, A1))
            var n2 = Normalize(new Point2f(-best.B, best.A)); // bảo đảm vuông góc 90° chính xác
            float A2 = n2.X, B2 = n2.Y;
            float C2 = -(A2 * passPoint.X + B2 * passPoint.Y);

            // điểm để vẽ: đi qua passPoint, hướng d2 = (-B2, A2)
            var d2 = new Point2f(-B2, A2);
            var P0 = passPoint;
            var P1 = new Point2f(P0.X + d2.X * 1000f, P0.Y + d2.Y * 1000f);

            return new FilletCornerMeasure.LineAB { A = A2, B = B2, C = C2, P1 = P0, P2 = P1 };
        }

        private void SelectStrongPlusOrth(
            List<LineCandidate> cands,
            out LineAB lineH, out LineAB lineV,
            Func<LineCandidate, bool> IsHorizontal,
            Func<LineCandidate, bool> IsVertical,
            out bool foundPair)
        {
            lineH = default; lineV = default; foundPair = false;

            var best = cands.OrderByDescending(c => c.Count).First();

            double bestAngDiff = double.MaxValue;
            LineCandidate bestOther = default;
            bool hasOther = false;

            foreach (var c in cands)
            {
                if (ReferenceEquals(c, best)) continue;
                double angDiff90 = AngleDiffTo90(best.L, c.L);
                if (angDiff90 < bestAngDiff)
                {
                    bestAngDiff = angDiff90;
                    bestOther = c;
                    hasOther = true;
                }
            }

            if (!hasOther) return;

            if (IsHorizontal(best) && IsVertical(bestOther))
            {
                lineH = best.L; lineV = bestOther.L;
            }
            else if (IsHorizontal(bestOther) && IsVertical(best))
            {
                lineH = bestOther.L; lineV = best.L;
            }
            else
            {
                Func<double, double> distTo0or180 = a => Math.Min(Math.Abs(a - 0), Math.Abs(a - 180));
                var hLine = (distTo0or180(best.AngleDeg) <= distTo0or180(bestOther.AngleDeg)) ? best : bestOther;
                lineH = hLine.L;
                lineV = ReferenceEquals(hLine, best) ? bestOther.L : best.L;
            }
            foundPair = true;
        }

        private double LineDirectionDeg(LineAB l)
        {
            var dx = l.P2.X - l.P1.X;
            var dy = l.P2.Y - l.P1.Y;
            double a = Math.Atan2(dy, dx) * 180.0 / Math.PI;
            if (a < 0) a += 180.0;
            return a;
        }

        private double AngleDiffTo90(LineAB a, LineAB b)
        {
            var v1 = new Point2f(a.P2.X - a.P1.X, a.P2.Y - a.P1.Y);
            var v2 = new Point2f(b.P2.X - b.P1.X, b.P2.Y - b.P1.Y);
            double dot = v1.X * v2.X + v1.Y * v2.Y;
            double n1 = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
            double n2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y);
            if (n1 < 1e-6 || n2 < 1e-6) return 0;
            double c = dot / (n1 * n2);
            c = Math.Max(-1, Math.Min(1, c));
            double ang = Math.Acos(c) * 180.0 / Math.PI; // 0..180
            return Math.Abs(ang - 90.0);
        }

        private double NormalizeDeg(double a)
        {
            a %= 180.0;
            if (a < 0) a += 180.0;
            return a;
        }

        private double AngleDiff180(double a, double b)
        {
            double d = Math.Abs(NormalizeDeg(a) - NormalizeDeg(b));
            return Math.Min(d, 180.0 - d);
        }

        private List<(Point2f p, double thetaDeg)> EstimateContourOrientations(Point[] contour)
        {
            int n = contour.Length;
            var list = new List<(Point2f, double)>(n);
            for (int i = 0; i < n; i++)
            {
                var prev = contour[(i - 1 + n) % n];
                var next = contour[(i + 1) % n];
                float dx = next.X - prev.X;
                float dy = next.Y - prev.Y;
                double theta = Math.Atan2(dy, dx) * 180.0 / Math.PI; // -180..180
                if (theta < 0) theta += 180.0; // 0..180 (tiếp tuyến có đối xứng 180°)
                list.Add((new Point2f(contour[i].X, contour[i].Y), theta));
            }
            return list;
        }

        private List<Point2f> ExtractEdgePoints(Mat gray, Mat edges)
        {
            try
            {
                using (var nz = new Mat())
                {
                    Cv2.FindNonZero(edges, nz);
                    var pts = new List<Point2f>(nz.Rows);
                    for (int i = 0; i < nz.Rows; i++)
                    {
                        var p = nz.At<Point>(i, 0);
                        pts.Add(new Point2f(p.X, p.Y));
                    }
                    if (pts.Count == 0) return pts;

                    const int MaxPts = 10000;
                    if (pts.Count > MaxPts)
                    {
                        int step = pts.Count / MaxPts;
                        pts = pts.Where((_, idx) => idx % step == 0).ToList();
                    }

                    Cv2.CornerSubPix(gray, pts.ToArray(), new Size(3, 3), new Size(-1, -1),
                        new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 20, 0.03));
                    return pts;
                }
            }
            catch { return new List<Point2f>(); }
        }

        private (int i1, int i2)[] PrecomputePairs(int n, int m, int seed)
        {
            var pairs = new (int, int)[m];
            uint s = unchecked((uint)seed);
            for (int i = 0; i < m; i++)
            {
                int a = NextIndex(ref s, n);
                int b; do { b = NextIndex(ref s, n); } while (b == a);
                pairs[i] = (a, b);
            }
            return pairs;
        }

        private int NextIndex(ref uint s, int n)
        {
            s ^= s << 13; s ^= s >> 17; s ^= s << 5;
            return (int)(s % (uint)n);
        }
     
        private LineAB RansacFitLine(List<Point2f> pts, out List<Point2f> inliers)
        {
            inliers = new List<Point2f>();
            var best = new LineAB();
            if (pts == null || pts.Count < 2 || RansacIterations <= 0) return best;

            pts = pts.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
            var pairs = PrecomputePairs(pts.Count, RansacIterations, 987654321);

            int bestCount = -1;
            List<Point2f> bestInliers = null;

            System.Threading.Tasks.Parallel.ForEach(
                System.Collections.Concurrent.Partitioner.Create(0, RansacIterations),
                r =>
                {
                    int localBest = -1;
                    List<Point2f> localInliers = null;

                    for (int it = r.Item1; it < r.Item2; it++)
                    {
                        var (i1, i2) = pairs[it];
                        if (i1 == i2) continue;

                        var p1 = pts[i1];
                        var p2 = pts[i2];

                        double a = p2.Y - p1.Y;
                        double b = p1.X - p2.X;
                        double norm = Math.Sqrt(a * a + b * b);
                        if (norm < 1e-6) continue;
                        double c = -(a * p1.X + b * p1.Y);

                        int cnt = 0;
                        var curr = new List<Point2f>();
                        foreach (var pp in pts)
                        {
                            double d = Math.Abs(a * pp.X + b * pp.Y + c) / norm;
                            if (d < RansacThreshold) { cnt++; curr.Add(pp); }
                        }
                        if (cnt == 0) continue;

                        if (cnt > localBest) { localBest = cnt; localInliers = curr; }
                    }

                    if (localBest > -1)
                    {
                        lock (pairs)
                        {
                            if (localBest > bestCount)
                            {
                                bestCount = localBest;
                                bestInliers = localInliers;
                            }
                        }
                    }
                });

            if (bestInliers != null && bestInliers.Count >= 2)
            {
                inliers = bestInliers;
                Line2D f = Cv2.FitLine(inliers.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
                float vx = (float)f.Vx, vy = (float)f.Vy, x0 = (float)f.X1, y0 = (float)f.Y1;

                float a = vy, b = -vx, c = -(a * x0 + b * y0);
                float nrm = (float)Math.Sqrt(a * a + b * b);
                if (nrm > 1e-6f) { a /= nrm; b /= nrm; c /= nrm; }

                var P1 = new Point2f(x0, y0);
                var P2 = new Point2f(x0 + vx * 1000f, y0 + vy * 1000f);
                best = new LineAB { A = a, B = b, C = c, P1 = P1, P2 = P2 };
            }

            return best;
        }

        private static Point2f Intersect(LineAB L1, LineAB L2)
        {
            float det = L1.A * L2.B - L2.A * L1.B;
            if (Math.Abs(det) < 1e-6) return new Point2f(float.NaN, float.NaN);
            float x = (L1.B * L2.C - L2.B * L1.C) / det;
            float y = (L2.A * L1.C - L1.A * L2.C) / det;
            return new Point2f(x, y);
        }

        private static Point2f Normalize(Point2f v)
        {
            float n = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
            return n > 1e-12f ? new Point2f(v.X / n, v.Y / n) : new Point2f(0, 0);
        }

        // Giữ lại nếu bạn còn dùng nơi khác; hiện tại không dùng nữa cho phân giác
        private static Point2f OrientNormalInwards(Point2f n, LineAB L, Point[] contour)
        {
            int neg = 0, pos = 0;
            var rnd = new Random(0);
            for (int k = 0; k < Math.Min(100, contour.Length); ++k)
            {
                var p = contour[rnd.Next(contour.Length)];
                double s = L.A * p.X + L.B * p.Y + L.C;
                if (s < 0) neg++; else pos++;
            }
            if (pos > neg) n = new Point2f(-n.X, -n.Y);
            return n;
        }

        private static double DistancePointLine(Point2f p, LineAB L)
            => Math.Abs(L.A * p.X + L.B * p.Y + L.C);

        private static Point2f FootOfPerpendicular(Point2f p, LineAB L)
        {
            float d = L.A * p.X + L.B * p.Y + L.C;
            return new Point2f(p.X - L.A * d, p.Y - L.B * d);
        }

        private static Point2f ClosestPointToLine(Point[] contour, LineAB L)
        {
            double best = double.MaxValue;
            Point2f bestP = new Point2f();
            foreach (var p in contour)
            {
                double d = Math.Abs(L.A * p.X + L.B * p.Y + L.C);
                if (d < best) { best = d; bestP = new Point2f(p.X, p.Y); }
            }
            return bestP;
        }

        private static bool LineSegmentIntersect(LineAB L, Point2f p1, Point2f p2, out Point2f X)
        {
            X = default;
            double f1 = L.A * p1.X + L.B * p1.Y + L.C;
            double f2 = L.A * p2.X + L.B * p2.Y + L.C;

            if (Math.Abs(f1) < 1e-6) { X = p1; return true; }
            if (Math.Abs(f2) < 1e-6) { X = p2; return true; }

            if ((f1 < 0 && f2 > 0) || (f1 > 0 && f2 < 0))
            {
                double t = f1 / (f1 - f2);
                X = new Point2f(
                    (float)(p1.X + t * (p2.X - p1.X)),
                    (float)(p1.Y + t * (p2.Y - p1.Y))
                );
                return true;
            }
            return false;
        }

        private static List<Point2f> GetIntersections(LineAB L, Point[] contour)
        {
            var list = new List<Point2f>();
            for (int i = 0; i < contour.Length; i++)
            {
                var p1 = (Point2f)contour[i];
                var p2 = (Point2f)contour[(i + 1) % contour.Length];
                if (LineSegmentIntersect(L, p1, p2, out var X))
                    list.Add(X);
            }
            return list;
        }

        private Point2f NearestIntersectionToPoint(LineAB L, Point[] contour, Point2f P0, out bool ok)
        {
            var ints = GetIntersections(L, contour);
            ok = ints.Count > 0;
            if (!ok) return default;

            double best = double.MaxValue;
            Point2f bestX = default;
            foreach (var q in ints)
            {
                double dx = q.X - P0.X, dy = q.Y - P0.Y;
                double d2 = dx * dx + dy * dy;
                if (d2 < best) { best = d2; bestX = q; }
            }
            return bestX;
        }

        private Point2f ClosestPointToLineNear(Point[] contour, LineAB L, Point2f P0, double eps = 0)
        {
            if (eps <= 0) eps = Math.Max(2.0, 2.0 * RansacThreshold);
            double best = double.MaxValue;
            Point2f bestP = default;

            foreach (var p in contour)
            {
                double dLine = Math.Abs(L.A * p.X + L.B * p.Y + L.C);
                if (dLine <= eps)
                {
                    double dO2 = (p.X - P0.X) * (p.X - P0.X) + (p.Y - P0.Y) * (p.Y - P0.Y);
                    if (dO2 < best) { best = dO2; bestP = new Point2f(p.X, p.Y); }
                }
            }
            if (best == double.MaxValue) return ClosestPointToLine(contour, L);
            return bestP;
        }

        private int NearestIndex(Point[] contour, Point2f P)
        {
            int idx = -1; double best = double.MaxValue;
            for (int i = 0; i < contour.Length; i++)
            {
                double dx = contour[i].X - P.X, dy = contour[i].Y - P.Y;
                double d2 = dx * dx + dy * dy;
                if (d2 < best) { best = d2; idx = i; }
            }
            return idx;
        }

        private double ArcLengthBetween(Point[] contour, Point2f P1, Point2f P2)
        {
            int i1 = NearestIndex(contour, P1);
            int i2 = NearestIndex(contour, P2);
            if (i1 < 0 || i2 < 0) return double.NaN;

            double len1 = 0;
            for (int i = i1; i != i2; i = (i + 1) % contour.Length)
            {
                var a = contour[i]; var b = contour[(i + 1) % contour.Length];
                len1 += Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
            }

            double len2 = 0;
            for (int i = i2; i != i1; i = (i + 1) % contour.Length)
            {
                var a = contour[i]; var b = contour[(i + 1) % contour.Length];
                len2 += Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
            }

            return Math.Min(len1, len2);
        }

        private void DrawLine(Mat img, LineAB L, Scalar color, int thickness)
        {
            int w = img.Cols, h = img.Rows;
            Point p1, p2;
            if (Math.Abs(L.B) > Math.Abs(L.A))
            {
                p1 = new Point(0, (int)Math.Round((-L.A * 0 - L.C) / L.B));
                p2 = new Point(w - 1, (int)Math.Round((-L.A * (w - 1) - L.C) / L.B));
            }
            else
            {
                p1 = new Point((int)Math.Round((-L.B * 0 - L.C) / L.A), 0);
                p2 = new Point((int)Math.Round((-L.B * (h - 1) - L.C) / L.A), h - 1);
            }
            Cv2.Line(img, p1, p2, color, thickness, LineTypes.AntiAlias);
        }
    }
}
