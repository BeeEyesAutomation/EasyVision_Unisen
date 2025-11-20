using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using static BeeCore.Width;

namespace BeeCore
{
    /// <summary>
    /// Kết quả đo khe song song hoặc hai đỉnh
    /// </summary>
    public struct GapResult
    {  public List<Line2D> line2Ds { get; set; }
        public Line2D LineA { get; set; }
        public Line2D LineB { get; set; }
        public Point[] lineMid { get; set; }
        /// <summary>Khoảng cách tính theo pixel</summary>
        public double GapMin { get; set; }
        public double Inlier { get; set; }
        public double GapMedium { get; set; }
        public double GapMax { get; set; }

    }

    /// <summary>
    /// Lớp đo khe:
    /// - MeasureParallelGap: đo giữa hai đường song song (sub-pixel + RANSAC)
    /// - MeasureVertexGap: đo khoảng cách giữa hai điểm đỉnh xa nhất (convex hull)
    /// </summary>
    public class ParallelGapDetector
    {
        public ParallelGapDetector()
        {

        }
        public double MmPerPixel { get; set; }
       
        public int RansacIterations { get; set; } = 1000;
        public double RansacThreshold { get; set; } = 2.0;
        private void GetLineParams(Line2D ln, out double a, out double b, out double c)
        {
            double vx = ln.Vx;
            double vy = ln.Vy;
            a = vy;
            b = -vx;
            c = -(a * ln.X1 + b * ln.Y1);
        }
        public ParallelGapDetector(double mmPerPixel)
        {
            MmPerPixel = mmPerPixel;
        }
        private List<Point2f> GetInliers(Line2D ln, IEnumerable<Point2f> pts)
        {
            GetLineParams(ln, out double a, out double b, out double c);
            double norm = Math.Sqrt(a * a + b * b);
            return pts.Where(p => Math.Abs(a * p.X + b * p.Y + c) / norm < RansacThreshold).ToList();
        }
        /// <summary>
        /// Đo khoảng cách giữa hai điểm đỉnh (tip) của hai contour lớn nhất
        /// </summary>

        /// <summary>
        /// Fit một đường thẳng bằng RANSAC, chỉ xét các segment có độ dài và góc lệch trong ngưỡng cho trước.
        /// </summary>
        /// <param name="pts">Tập điểm biên (Point2f)</param>
        /// <param name="inliers">Trả về các inlier của line tốt nhất</param>
        /// <param name="minLen">Độ dài đoạn tối thiểu (px)</param>
        /// <param name="maxLen">Độ dài đoạn tối đa (px)</param>
        /// <param name="minAngleDeg">Góc lệch nhỏ nhất (độ)</param>
        /// <param name="maxAngleDeg">Góc lệch lớn nhất (độ)</param>
        /// <returns>Line2D chứa (P1,P2)</returns>
    
        private Line2D RansacFitLine(
    List<Point2f> pts,
    out List<Point2f> inliers,
    double minLen = 0,
    double maxLen = double.MaxValue,
    double minAngleDeg = 0,
    double maxAngleDeg = 180)
        {
            inliers = new List<Point2f>();
            Line2D bestLine = default;

            if (pts == null || pts.Count < 2 || RansacIterations <= 0)
                return bestLine;

            // Sắp xếp để ổn định tuyệt đối theo dữ liệu đầu vào
            pts = pts.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();

            // Tiền tạo cặp chỉ số theo seed cố định (deterministic)
            const int FIXED_SEED = 123456789;
            var pairs = PrecomputePairs(pts.Count, RansacIterations, FIXED_SEED);

            // Best giữ dạng (inliers, segLen, a,b,c) để tie-break ổn định
            int bestCount = -1;
            double bestSegLen = -1;
            double best_a = 0, best_b = 0, best_c = 0;
            List<Point2f> bestInliers = null;

            System.Threading.Tasks.Parallel.ForEach(
                System.Collections.Concurrent.Partitioner.Create(0, RansacIterations),
                range =>
                {
                    // local best
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
                        if (segLen < minLen || segLen > maxLen) continue;

                        double angle = Math.Abs(Math.Atan2(dy, dx) * 180.0 / Math.PI);
                        if (angle < minAngleDeg || angle > maxAngleDeg) continue;

                        double a = p2.Y - p1.Y;
                        double b = p1.X - p2.X;
                        double norm = Math.Sqrt(a * a + b * b);
                        if (norm < 1e-6) continue;
                        double c = -(a * p1.X + b * p1.Y);

                        // Đếm inliers
                        int cnt = 0;
                        var currInliers = new List<Point2f>();
                        for (int k = 0; k < pts.Count; k++)
                        {
                            var pp = pts[k];
                            double d = Math.Abs(a * pp.X + b * pp.Y + c) / norm;
                            if (d < RansacThreshold)
                            {
                                cnt++;
                                currInliers.Add(pp);
                            }
                        }

                        if (cnt == 0) continue;

                        // Tie-break ổn định:
                        // 1) cnt lớn hơn
                        // 2) segLen lớn hơn
                        // 3) a nhỏ hơn, rồi b nhỏ hơn, rồi c nhỏ hơn (đảm bảo thứ tự toàn phần)
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
                            l_inliers = currInliers;
                        }
                    }

                    if (l_count > -1)
                    {
                        // Hợp nhất lên best toàn cục (tie-break cùng logic)
                        lock (pairs) // dùng pairs làm lock object nhẹ
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

            if (bestInliers != null && bestInliers.Count >= 2)
            {
                inliers = bestInliers;
                bestLine = Cv2.FitLine(inliers.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
            }
            else
            {
                inliers = new List<Point2f>();
            }

            return bestLine;
        }

        public GapResult MeasureParallelGap(Mat raw, Mat edge, int numLines, GapExtremum extremum, LineOrientation orientation = LineOrientation.Any, SegmentStatType? segStat = null,int MinInlier=2 )
        {
            int InLierMin = 1000000000;
            List<Point2f> pts = ExtractEdgePoints(raw, edge);

            // 2. Iterative RANSAC thu N lines
            var lines = new List<Line2D>();
            var remaining = new List<Point2f>(pts);
            for (int i = 0; i < numLines; i++)
            {
                var line = RansacFitLine(remaining, out var inliers);
                lines.Add(line);
                remaining = remaining.Except(inliers).ToList();
                if (remaining.Count < InLierMin) InLierMin = remaining.Count;
                if (remaining.Count < MinInlier) break;
            }
         // Mat annotated = raw.Clone();

            //  Cv2.ImShow("raw", annotated);
            if (lines.Count < 2)
                return new GapResult();

            // 3. Lọc theo orientation

            var filtered = ((lines ?? Enumerable.Empty<Line2D>()).Where(l => l != null))
     .Where(l => orientation == LineOrientation.Any
         || (orientation == LineOrientation.Horizontal
                 ? Math.Abs(l.Vy) <= Math.Abs(l.Vx)
                 : Math.Abs(l.Vx) <= Math.Abs(l.Vy)))
     .ToList();

            //      throw new InvalidOperationException("Không tìm đủ lines với orientation yêu cầu.");
            lines = filtered;

            // 4. Vẽ tất cả lines nền (xám nhạt)

            if (filtered.Count < 2)

                return new GapResult {  };
            //foreach (var l in lines)
            //    DrawInfiniteLine(annotated, l, new Scalar(200, 200, 200), 1);

            // 5. Chọn cặp theo pairType
            Line2D lA = null, lB = null;
            switch (extremum)
            {
                case GapExtremum.Nearest:
                case GapExtremum.Farthest:
                    double bestDist2 = extremum == GapExtremum.Farthest ? double.MinValue : double.MaxValue;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        for (int j = i + 1; j < lines.Count; j++)
                        {
                            double d = DistanceBetweenLines(lines[i], lines[j]);
                            if ((extremum == GapExtremum.Farthest && d > bestDist2) ||
                                (extremum == GapExtremum.Nearest && d < bestDist2))
                            {
                                bestDist2 = d;
                                lA = lines[i];
                                lB = lines[j];
                            }
                        }
                    }
                    break;

                case GapExtremum.Outermost:
                case GapExtremum.Middle:
                    // Tính offset của từng line (|c|/sqrt(a^2+b^2))
                    var offsets = lines.Select(ln =>
                    {
                        GetLineParams(ln, out double a, out double b, out double c);
                        double off = Math.Abs(c) / Math.Sqrt(a * a + b * b);
                        return new { ln, off };
                    })
                    .OrderBy(x => x.off)
                    .ToArray();
                    if (extremum == GapExtremum.Outermost)
                    {
                        lA = offsets.First().ln;
                        lB = offsets.Last().ln;
                    }
                    else
                    {
                        // Middle: hai đường giữa
                        int m = offsets.Length / 2;
                        lA = offsets[m - 1].ln;
                        lB = offsets[m].ln;
                    }
                    break;
            }

            // 5. Vẽ cặp chọn (xanh lá & đỏ)
          //  DrawInfiniteLine(annotated, lA, new Scalar(0, 255, 0), 2);
            //DrawInfiniteLine(annotated, lB, new Scalar(0, 0, 255), 2);
            // 1) Tính vùng y chung của hai line (dựa trên inliers nếu đã lưu):
            //    Nếu bạn không có inliers, bạn có thể dùng 2 điểm P1,P2 của mỗi line:

            // 7) Tự động tìm yTop/yBot dựa trên inliers
            var inlA = GetInliers(lA, pts);
            var inlB = GetInliers(lB, pts);
            double yTop = 0, yBot = 0, xLeft = 0, xRight = 0;
            double yShort=0,yLong=0;
            double xShort=0, xLong=0;
            if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
            {
                yTop = Math.Max(inlA.Min(p => p.Y), inlB.Min(p => p.Y));
                yBot = Math.Min(inlA.Max(p => p.Y), inlB.Max(p => p.Y));
            }
            else // Horizontal
            {
                xLeft = Math.Max(inlA.Min(p => p.X), inlB.Min(p => p.X));
                xRight = Math.Min(inlA.Max(p => p.X), inlB.Max(p => p.X));
            }

            //   double yTop = Math.Max(inlA.Min(p => p.Y), inlB.Min(p => p.Y));
            //  double yBot = Math.Min(inlA.Max(p => p.Y), inlB.Max(p => p.Y));

            // 8) Tính xLeft/xRight tại yTop và yBot
            double SolveX(Line2D ln, double y)
            {
                double vx = ln.Vx, vy = ln.Vy;
                double x0 = ln.X1, y0 = ln.Y1;
                if (Math.Abs(vy) < 1e-6) return x0;
                double t = (y - y0) / vy;
                return x0 + vx * t;
            }
            double SolveY(Line2D ln, double x)
            {
                double vx = ln.Vx, vy = ln.Vy;
                double x0 = ln.X1, y0 = ln.Y1;
                if (Math.Abs(vx) < 1e-6) return y0;
                double t = (x - x0) / vx;
                return y0 + vy * t;
            }
            yShort = Math.Min(yBot, yTop);
            yLong = Math.Max(yBot, yTop);

           
            // 9) Compute Short (Top) and Long (Bot) gaps
            // Short = gap tại yTop hoặc xLeft; Long = tại yBot hoặc xRight

            double shortPx, longPx; double mediumPx = 0;
            if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
            {
                shortPx = Math.Abs(SolveX(lB, yTop) - SolveX(lA, yTop));
                longPx = Math.Abs(SolveX(lB, yBot) - SolveX(lA, yBot));
                if (shortPx < longPx)
                {
                    yShort = yTop;
                    yLong = yBot;
                }
                else
                {
                    double temp = longPx;
                    longPx= shortPx;
                    shortPx = temp;
                    yShort = yBot;
                    yLong = yTop;
                }    
            }
            else
            {
                shortPx = Math.Abs(SolveY(lB, xLeft) - SolveY(lA, xLeft));
                longPx = Math.Abs(SolveY(lB, xRight) - SolveY(lA, xRight));
                if (shortPx < longPx)
                {
                    xShort = xLeft;
                    xLong = xRight;
                }
                else
                {
                    double temp = longPx;
                    longPx = shortPx;
                    shortPx = temp;
                    xShort = xRight;
                    xLong = xLeft;
                }
            }
            if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
            {
                double yMid = (yTop + yBot) / 2;
                Point p1 = new Point((int)Math.Round(SolveX(lA, yMid)), (int)Math.Round(yMid));
                Point p2 = new Point((int)Math.Round(SolveX(lB, yMid)), (int)Math.Round(yMid));

                mediumPx = Math.Abs(p2.X - p1.X);
                //Draws.DrawTicks(annotated, lineMids[0], orientation);
                //Draws.DrawTicks(annotated, lineMids[1], orientation);
                //Cv2.Line(annotated, lineMids[0], lineMids[1], new Scalar(0, 0, 255), 2);
                //Cv2.PutText(annotated, $"Mid: {mediumPx:F2} mm", new Point(lineMids[0].X, lineMids[0].Y + 5), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 0, 255), 2);
            }
            else
            {
                double xMid = (xLeft + xRight) / 2;
                Point p1 = new Point((int)Math.Round(xMid), (int)Math.Round(SolveY(lA, xMid)));
                Point p2 = new Point((int)Math.Round(xMid), (int)Math.Round(SolveY(lB, xMid)));

                mediumPx = Math.Abs(p2.Y - p1.Y);
                //Draws.DrawTicks(annotated, lineMids[0], orientation);
                //Draws.DrawTicks(annotated, lineMids[1], orientation);
                //Cv2.Line(annotated, lineMids[0], lineMids[1], new Scalar(0, 0, 255), 2);
                //Cv2.PutText(annotated, $"Mid: {mediumPx:F2} mm", new Point(lineMids[0].X + 5, (lineMids[0].Y + lineMids[1].Y) / 2), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 0, 255), 2);
            }
            Point[] lineMids = new Point[2];
            switch(segStat)
            {
                case SegmentStatType.Shortest:
                    // 10) Vẽ Short ticks & label
                    if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
                    {
                        lineMids[0] = new Point((int)Math.Round(SolveX(lA, yShort)), (int)Math.Round(yShort));
                        lineMids[1] = new Point((int)Math.Round(SolveX(lB, yShort)), (int)Math.Round(yShort));
                        //Draws.DrawTicks(annotated, lineMids[0], orientation);
                        //Draws.DrawTicks(annotated, lineMids[1], orientation);
                        //Cv2.Line(annotated, lineMids[0], lineMids[1], new Scalar(128, 200, 255), 2);
                        //Cv2.PutText(annotated, $"Short: {shortPx:F2} mm", new Point(lineMids[0].X, lineMids[0].Y + 15), HersheyFonts.HersheySimplex, 0.7, new Scalar(128, 200, 255), 2);
                    }
                    else
                    {
                        lineMids[0] = new Point((int)Math.Round(xShort), (int)Math.Round(SolveY(lA, xShort)));
                        lineMids[1] = new Point((int)Math.Round(xShort), (int)Math.Round(SolveY(lB, xShort)));
                        //Draws.DrawTicks(annotated, lineMids[0], orientation);
                        //Draws.DrawTicks(annotated, lineMids[1], orientation);
                        //Cv2.Line(annotated, lineMids[0], lineMids[1], new Scalar(128, 200, 255), 2);
                        //Cv2.PutText(annotated, $"Short: {shortPx:F2} mm", new Point(lineMids[0].X, lineMids[0].Y + 15), HersheyFonts.HersheySimplex, 0.7, new Scalar(128, 200, 255), 2);

                        //  Draws.DrawTicks(annotated, pTs, orientation);
                        //  Draws.DrawTicks(annotated, pTe, orientation);
                        // Cv2.Line(annotated, pTs, pTe, new Scalar(128, 200, 255), 2);
                        // Cv2.PutText(annotated, $"Short: {shortMm:F2} mm", new Point(pTs.X + 5, (pTs.Y + pTe.Y) / 2), HersheyFonts.HersheySimplex, 0.7, new Scalar(128, 200, 255), 2);
                    }
                    break;
                case SegmentStatType.Average:
                    if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
                    {
                        double yMid = (yTop + yBot) / 2;
                        lineMids[0] = new Point((int)Math.Round(SolveX(lA, yMid)), (int)Math.Round(yMid));
                        lineMids[1] = new Point((int)Math.Round(SolveX(lB, yMid)), (int)Math.Round(yMid));
                        
                        mediumPx = Math.Abs(lineMids[1].X - lineMids[0].X);
                        //Draws.DrawTicks(annotated, lineMids[0], orientation);
                        //Draws.DrawTicks(annotated, lineMids[1], orientation);
                        //Cv2.Line(annotated, lineMids[0], lineMids[1], new Scalar(0, 0, 255), 2);
                        //Cv2.PutText(annotated, $"Mid: {mediumPx:F2} mm", new Point(lineMids[0].X, lineMids[0].Y + 5), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 0, 255), 2);
                    }
                    else
                    {
                        double xMid = (xLeft + xRight) / 2;
                        lineMids[0] = new Point((int)Math.Round(xMid), (int)Math.Round(SolveY(lA, xMid)));
                        lineMids[1] = new Point((int)Math.Round(xMid), (int)Math.Round(SolveY(lB, xMid)));
                       
                       // mediumPx = Math.Abs(lineMids[1].Y - lineMids[0].Y);
                        //Draws.DrawTicks(annotated, lineMids[0], orientation);
                        //Draws.DrawTicks(annotated, lineMids[1], orientation);
                        //Cv2.Line(annotated, lineMids[0], lineMids[1], new Scalar(0, 0, 255), 2);
                        //Cv2.PutText(annotated, $"Mid: {mediumPx:F2} mm", new Point(lineMids[0].X + 5, (lineMids[0].Y + lineMids[1].Y) / 2), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 0, 255), 2);
                    }


                    break;
                case SegmentStatType.Longest:
                    // 11) Vẽ Long
                    if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
                    {
                        lineMids[0] = new Point((int)Math.Round(SolveX(lA, yLong)), (int)Math.Round(yLong));
                        lineMids[1] = new Point((int)Math.Round(SolveX(lB, yLong)), (int)Math.Round(yLong));
                        //Draws.DrawTicks(annotated, lineMids[0], orientation);
                        //Draws.DrawTicks(annotated, lineMids[1], orientation);
                        //Cv2.Line(annotated, lineMids[0], lineMids[1], new Scalar(128, 200, 255), 2);
                        //Cv2.PutText(annotated, $"Long: {longPx:F2} mm", new Point(lineMids[0].X, lineMids[0].Y + 15), HersheyFonts.HersheySimplex, 0.7, new Scalar(128, 200, 255), 2);

                        //  Draws.DrawTicks(annotated, pLs, orientation);
                        // Draws.DrawTicks(annotated, pLe, orientation);
                        //  Cv2.Line(annotated, pLs, pLe, new Scalar(0, 128, 255), 3);
                        // Cv2.PutText(annotated, $"Long: {longMm:F2} mm", new Point(pLs.X, pLs.Y - 10), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 128, 255), 2);
                    }
                    else
                    {
                        lineMids[0] = new Point((int)Math.Round(xLong), (int)Math.Round(SolveY(lA, xLong)));
                        lineMids[1] = new Point((int)Math.Round(xLong), (int)Math.Round(SolveY(lB, xLong)));
                        //Draws.DrawTicks(annotated, lineMids[0], orientation);
                        //Draws.DrawTicks(annotated, lineMids[1], orientation);
                        //Cv2.Line(annotated, lineMids[0], lineMids[1], new Scalar(128, 200, 255), 2);
                        //Cv2.PutText(annotated, $"Long: {longPx:F2} mm", new Point(lineMids[0].X, lineMids[0].Y + 15), HersheyFonts.HersheySimplex, 0.7, new Scalar(128, 200, 255), 2);

                        ////  Draws.DrawTicks(annotated, pLs, orientation);
                        //  Draws.DrawTicks(annotated, pLe, orientation);
                        // Cv2.Line(annotated, pLs, pLe, new Scalar(0, 128, 255), 3);
                        // Cv2.PutText(annotated, $"Long: {longMm:F2} mm", new Point(pLs.X + 5, (pLs.Y + pLe.Y) / 2), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 128, 255), 2);
                    }
                    break;
                   
            }
           // Cv2.ImWrite("rs.png", annotated);
            return new GapResult { line2Ds=lines,LineA=lA,LineB=lB,lineMid=lineMids, GapMin= shortPx,GapMedium= mediumPx,GapMax= longPx,Inlier= InLierMin };

            //return new GapResult { GapPx = gapPx, GapMm = gapMm, AnnotatedImage = annotated };
        }




        private List<Point2f> ExtractEdgePoints(Mat gray, Mat edges)
        {
            try
            {
                // Lấy tất cả pixel biên (không cần contour đóng)
                Mat nonZero = new Mat();
                Cv2.FindNonZero(edges, nonZero);

                var ptsList = new List<Point>();
                for (int i = 0; i < nonZero.Rows; i++)
                {
                    ptsList.Add(nonZero.At<Point>(i, 0));
                }
                if (ptsList.Count == 0)
                    return new List<Point2f>();

                // Chuyển thành Point2f
                var ptsf = ptsList.Select(p => new Point2f(p.X, p.Y)).ToArray();

                // Giảm mật độ: lấy mỗi điểm thứ (nếu quá nhiều điểm)
                int maxPoints = 10000;
                Point2f[] sampled = ptsf;
                if (ptsf.Length > maxPoints)
                {
                    int step = ptsf.Length / maxPoints;
                    sampled = ptsf.Where((pt, idx) => idx % step == 0).ToArray();
                }

                // Tinh chỉnh sub-pixel
                Cv2.CornerSubPix(
                    gray,
                    sampled,
                    new Size(3, 3),
                    new Size(-1, -1),
                    new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 20, 0.03)
                );
                return sampled.ToList();
            }

            catch(Exception ex)
            { 
                String s = ex.Message;
            }
            return new List<Point2f>();


        }
     
        private static (int i1, int i2)[] PrecomputePairs(int n, int iterations, int seed)
        {
            var pairs = new (int, int)[iterations];
            uint s = unchecked((uint)seed);
            for (int i = 0; i < iterations; i++)
            {
                int a = NextIndex(ref s, n);
                int b;
                do { b = NextIndex(ref s, n); } while (b == a);
                pairs[i] = (a, b);
            }
            return pairs;
        }

        private static int NextIndex(ref uint s, int n)
        {
            // xorshift32 — nhanh & tái lặp
            s ^= s << 13;
            s ^= s >> 17;
            s ^= s << 5;
            return (int)(s % (uint)n);
        }

        private Line2D RansacFitLine(List<Point2f> pts, out List<Point2f> inliers)
        {
            inliers = new List<Point2f>();
            Line2D bestLine = default;

            if (pts == null || pts.Count < 2 || RansacIterations <= 0)
                return bestLine;

            pts = pts.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();

            const int FIXED_SEED = 987654321; // seed khác với hàm kia để độc lập
            var pairs = PrecomputePairs(pts.Count, RansacIterations, FIXED_SEED);

            int bestCount = -1;
            double bestSegLen = -1;
            double best_a = 0, best_b = 0, best_c = 0;
            List<Point2f> bestInliers = null;

            System.Threading.Tasks.Parallel.ForEach(
                System.Collections.Concurrent.Partitioner.Create(0, RansacIterations),
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

                        double a = p2.Y - p1.Y;
                        double b = p1.X - p2.X;
                        double norm = Math.Sqrt(a * a + b * b);
                        if (norm < 1e-6) continue;
                        double c = -(a * p1.X + b * p1.Y);

                        int cnt = 0;
                        var currInliers = new List<Point2f>();
                        for (int k = 0; k < pts.Count; k++)
                        {
                            var pp = pts[k];
                            double d = Math.Abs(a * pp.X + b * pp.Y + c) / norm;
                            if (d < RansacThreshold)
                            {
                                cnt++;
                                currInliers.Add(pp);
                            }
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
                            l_inliers = currInliers;
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

            if (bestInliers != null && bestInliers.Count >= 2)
            {
                inliers = bestInliers;
                bestLine = Cv2.FitLine(inliers.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
            }
            else
            {
                inliers = new List<Point2f>();
            }

            return bestLine;
        }

  
        private double DistanceBetweenLines(Line2D l1, Line2D l2)
        {
            // Local helper without static keyword
            void Params(Line2D ln, out double a, out double b, out double c)
            {
                double vx = ln.Vx;
                double vy = ln.Vy;
                double x0 = ln.X1;
                double y0 = ln.Y1;
                a = vy; b = -vx; c = -(a * x0 + b * y0);
            }

            Params(l1, out double a1, out double b1, out double c1);
            Params(l2, out double a2, out double b2, out double c2);
            return Math.Abs(c2 - c1) / Math.Sqrt(a1 * a1 + b1 * b1);
        }

        private void DrawInfiniteLine(Mat img, Line2D ln, Scalar col, int thickness)
        {
            double vx = ln.Vx;
            double vy = ln.Vy;
            double x0 = ln.X1;
            double y0 = ln.Y1;
            Point pt1 = new Point(x0 + vx * 1000, y0 + vy * 1000);
            Point pt2 = new Point(x0 - vx * 1000, y0 - vy * 1000);
            Cv2.Line(img, pt1, pt2, col, thickness);
        }
    }
}
