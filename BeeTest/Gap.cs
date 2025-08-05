using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;

namespace BeeTest
{
    /// <summary>
    /// Kết quả đo khe song song hoặc hai đỉnh
    /// </summary>
    public struct GapResult
    {
        /// <summary>Khoảng cách tính theo pixel</summary>
        public double GapPx { get; set; }
        /// <summary>Khoảng cách tính theo mm</summary>
        public double GapMm { get; set; }
        /// <summary>Ảnh gốc đã vẽ minh hoạ kết quả</summary>
        public Mat AnnotatedImage { get; set; }
    }
    public enum ShaftMeasureType
    {
        /// <summary>Khoảng cách giữa hai đường tâm</summary>
        CenterLine,
        /// <summary>Khe nhỏ nhất giữa hai biên</summary>
        MinEdge,
        /// <summary>Khoảng cách mép lớn nhất (max span)</summary>
        MaxEdge
    }
    public enum LineOrientation
    {
        Any, Horizontal, Vertical
    }
    public enum GapExtremum
    {
        Nearest,
        Farthest,
        Outermost,
        Middle
    }
    public enum SegmentStatType { Shortest, Longest, Average }
    /// <summary>
    /// Lớp đo khe:
    /// - MeasureParallelGap: đo giữa hai đường song song (sub-pixel + RANSAC)
    /// - MeasureVertexGap: đo khoảng cách giữa hai điểm đỉnh xa nhất (convex hull)
    /// </summary>
    public class ParallelGapDetector
    { /// <summary>
      /// Trục đo
      /// </summary>
      
       

        /// <summary>
        /// Kiểu cực trị: xa nhất hoặc gần nhất
        /// </summary>
   
  
      

        public int ThresholdValue { get; set; } = 100;
        private void GetLineParams(Line2D ln, out double a, out double b, out double c)
        {
            double vx = ln.Vx;
            double vy = ln.Vy;
            a = vy;
            b = -vx;
            c = -(a * ln.X1 + b * ln.Y1);
        }

        /// Đo theo loại cho trước: CenterLine, MinEdge, MaxEdge
        /// </summary>
        public GapResult MeasureShaftGap(string imagePath, ShaftMeasureType measureType)
        {
            // 1. Load & preprocess
            Mat src = Cv2.ImRead(imagePath);
            if (src.Empty()) throw new ArgumentException($"Không tìm thấy ảnh: {imagePath}");
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Mat bin = new Mat();
            Cv2.Threshold(gray, bin, ThresholdValue, 255, ThresholdTypes.BinaryInv);

            // 2. Lấy 2 contour lớn nhất
            Cv2.FindContours(bin, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxNone);
            if (contours.Length < 2) throw new InvalidOperationException("Cần ít nhất hai contour để đo shaft gap.");
            var top2 = contours.OrderByDescending(c => Cv2.ContourArea(c)).Take(2).ToArray();
            var c1 = top2[0];
            var c2 = top2[1];

            // 3. Lấy points
            List<Point2f> pts1 = c1.Select(p => new Point2f(p.X, p.Y)).ToList();
            List<Point2f> pts2 = c2.Select(p => new Point2f(p.X, p.Y)).ToList();

            // 4. Fit center lines (Line2D) nếu cần
            Line2D line1 = Cv2.FitLine(pts1.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
            Line2D line2 = Cv2.FitLine(pts2.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);

            // 5. Compute metrics
            double gapPx;
            Point pA = new Point(), pB = new Point();
            Mat annotated = src.Clone();

            switch (measureType)
            {
                case ShaftMeasureType.CenterLine:
                    // Khoảng cách giữa hai đường tâm
                    gapPx = DistanceBetweenLines(line1, line2);
                    // Vẽ 2 đường center
                    DrawInfiniteLine(annotated, line1, new Scalar(0, 255, 0), 2);
                    DrawInfiniteLine(annotated, line2, new Scalar(0, 0, 255), 2);
                    // Tính đoạn thẳng đo khoảng cách
                    // Lấy một điểm trên line1
                  
                    // Tính tham số dòng ax+by+c=0 cho line2
                    GetLineParams(line2, out double a2, out double b2, out double c4);
                    // Tính foot Q của p0 lên line2
                    double denom = a2 * a2 + b2 * b2;
                    double k = (a2 * line1.X1 + b2 * line1.Y1 + c4) / denom;
                    var q = new Point(
                        (int)Math.Round(line1.X1 - a2 * k),
                        (int)Math.Round(line1.Y1 - b2 * k)
                    );
                    // Vẽ đoạn đo
                    Cv2.Line(annotated, new Point(line1.X1, line1.Y1), q, new Scalar(255, 0, 0), 2);
                    // Ghi giá trị lên ảnh
                    var distMm = gapPx * MmPerPixel;
                  string   text = $"{distMm:F2} mm";
                    Cv2.PutText(annotated, text, new Point((line1.X1 + q.X) / 2, (line1.Y1 + q.Y) / 2 - 10),
                        HersheyFonts.HersheySimplex, 0.7, new Scalar(255, 0, 0), 2);
                    break;

                case ShaftMeasureType.MinEdge:
                    {
                        // Khoảng hở nhỏ nhất giữa các biên
                        var best = (from p in c1
                                    from q2 in c2
                                    let dx = p.X - q2.X
                                    let dy = p.Y - q2.Y
                                    select new { p, q2, d = Math.Sqrt(dx * dx + dy * dy) })
                                   .OrderBy(x => x.d)
                                   .First();
                        gapPx = best.d;
                        pA = best.p;
                        pB = best.q2;
                        Cv2.Circle(annotated, pA, 5, new Scalar(0, 255, 0), -1);
                        Cv2.Circle(annotated, pB, 5, new Scalar(0, 0, 255), -1);
                        Cv2.Line(annotated, pA, pB, new Scalar(255, 0, 0), 2);
                    }
                    break;

                case ShaftMeasureType.MaxEdge:
                    {
                        // Khoảng span lớn nhất giữa các biên
                        var best = (from p in c1
                                    from q1 in c2
                                    let dx = p.X - q1.X
                                    let dy = p.Y - q1.Y
                                    select new { p, q1, d = Math.Sqrt(dx * dx + dy * dy) })
                                   .OrderByDescending(x => x.d)
                                   .First();
                        gapPx = best.d;
                        pA = best.p;
                        pB = best.q1;
                        Cv2.Circle(annotated, pA, 5, new Scalar(0, 255, 0), -1);
                        Cv2.Circle(annotated, pB, 5, new Scalar(0, 0, 255), -1);
                        Cv2.Line(annotated, pA, pB, new Scalar(255, 0, 0), 2);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
             src = Cv2.ImRead(imagePath);
            if (src.Empty())
                throw new ArgumentException($"Không tìm thấy ảnh: {imagePath}");
             gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 0);

            // 2. Threshold để tách foreground (máy kim) khỏi nền
             bin = new Mat();
            Cv2.Threshold(gray, bin, 150, 255, ThresholdTypes.BinaryInv);

            
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(bin, out contours, out hierarchy,
                             RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            if (contours.Length < 2)
                throw new InvalidOperationException("Cần ít nhất hai contour để đo tip.");

            // 4. Chọn 2 contour có diện tích lớn nhất
             top2 = contours.OrderByDescending(c => Cv2.ContourArea(c)).Take(2).ToArray();

            // 5. Với mỗi contour, tìm điểm có Y nhỏ nhất (đỉnh hướng lên trên)
            Point tip1 = top2[0].OrderBy(p => p.Y).First();
            Point tip2 = top2[1].OrderBy(p => p.Y).First();

            // 6. Tính khoảng cách Euclid
            double dx1 = tip1.X - tip2.X;
            double dy1 = tip1.Y - tip2.Y;
            double distPx = Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            double distMm2 = distPx * MmPerPixel;

            // 7. Vẽ lên ảnh
            
            Cv2.Circle(annotated, tip1, 6, new Scalar(0, 255, 0), -1);
            Cv2.Circle(annotated, tip2, 6, new Scalar(0, 0, 255), -1);
             Line2D lineCen1=  FindPerpendicularLine(line1, tip1);
            Line2D lineCen2 = FindPerpendicularLine(line2, tip2);
          double gapPx2=  DistanceBetweenLines(lineCen1, lineCen2);
            DrawPerpendicularLine(annotated, lineCen1, new Scalar(0, 255, 0), 2);
            DrawPerpendicularLine(annotated, lineCen2, new Scalar(0, 255, 0), 2);
            var distMm3= gapPx2 * MmPerPixel;
            string text2 = $"{distMm3:F2} mm";
            Cv2.PutText(annotated, text2, tip1,
                HersheyFonts.HersheySimplex, 0.7, new Scalar(255, 0, 0), 2);
            //Cv2.Line(annotated, tip1, tip2, new Scalar(255, 0, 0), 2);
            double gapMm = gapPx * MmPerPixel;
            return new GapResult { GapPx = gapPx, GapMm = gapMm, AnnotatedImage = annotated };
        }
        public Line2D FindPerpendicularLine( Line2D baseLine, Point throughPoint)
        {
            // Vector chỉ phương của baseLine
            double vx = baseLine.Vx;
            double vy = baseLine.Vy;
            // Vector pháp tuyến (perpendicular)
            double px = -vy;
            double py = vx;
            // Chuẩn hóa
            double norm = Math.Sqrt(px * px + py * py);
            px /= norm; py /= norm;
            Point[] points = new Point[2];
            // Tạo hai điểm rất xa trên đường thẳng vuông góc
            points[0] = new Point((int)Math.Round(throughPoint.X + px * 1000),
                                (int)Math.Round(throughPoint.Y + py * 1000));
            points[1] = new Point((int)Math.Round(throughPoint.X - px * 1000),
                                (int)Math.Round(throughPoint.Y - py * 1000));
            // 4. Fit center lines (Line2D) nếu cần
           return Cv2.FitLine(points.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
          
        }
        public void DrawPerpendicularLine(Mat img, Line2D Line, Scalar color, int thickness)
        {
            
            DrawInfiniteLine(img, Line, color, thickness);
        }
        /// <summary>
        /// Khoảng cách giữa hai đường vô hạn từ Line2D
        /// </summary>


        /// <summary>
        /// Vẽ đường vô hạn từ Line2D
        /// </summary>

        public double MmPerPixel { get; set; }
        public int CannyThreshold1 { get; set; } = 50;
        public int CannyThreshold2 { get; set; } = 150;
        public int RansacIterations { get; set; } = 1000;
        public double RansacThreshold { get; set; } = 2.0;

        public ParallelGapDetector(double mmPerPixel)
        {
            MmPerPixel = mmPerPixel;
        }
  // Tách contour nền ngoài
        private List<Point[]> FindExternalContours(Mat src)
        {
            Mat gray = new Mat(); Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Mat bin = new Mat(); Cv2.Threshold(gray, bin, ThresholdValue, 255, ThresholdTypes.BinaryInv);
            Cv2.FindContours(bin, out Point[][] contours, out _,
                             RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            return contours.ToList();
        }

        // Vẽ và tính gap
        private GapResult AnnotateAndCompute(Mat src, Point p1, Point p2)
        {
            Mat annotated = src.Clone();
            Cv2.Circle(annotated, p1, 5, new Scalar(0, 255, 0), -1);
            Cv2.Circle(annotated, p2, 5, new Scalar(0, 0, 255), -1);
            Cv2.Line(annotated, p1, p2, new Scalar(255, 0, 0), 2);

            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            double distPx = Math.Sqrt(dx * dx + dy * dy);
            double distMm = distPx * MmPerPixel;

            return new GapResult { GapPx = distPx, GapMm = distMm, AnnotatedImage = annotated };
        }
     

        public GapResult MeasureTipGap(string imagePath)
        {
            // 1. Load ảnh, convert grayscale, blur
            Mat src = Cv2.ImRead(imagePath);
            if (src.Empty())
                throw new ArgumentException($"Không tìm thấy ảnh: {imagePath}");
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 0);

            // 2. Threshold để tách foreground (máy kim) khỏi nền
            Mat bin = new Mat();
            Cv2.Threshold(gray, bin, 150, 255, ThresholdTypes.BinaryInv);

            // 3. Tìm tất cả contour
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(bin, out contours, out hierarchy,
                             RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            if (contours.Length < 2)
                throw new InvalidOperationException("Cần ít nhất hai contour để đo tip.");

            // 4. Chọn 2 contour có diện tích lớn nhất
            var top2 = contours.OrderByDescending(c => Cv2.ContourArea(c)).Take(2).ToArray();

            // 5. Với mỗi contour, tìm điểm có Y nhỏ nhất (đỉnh hướng lên trên)
            Point tip1 = top2[0].OrderBy(p => p.Y).First();
            Point tip2 = top2[1].OrderBy(p => p.Y).First();

            // 6. Tính khoảng cách Euclid
            double dx = tip1.X - tip2.X;
            double dy = tip1.Y - tip2.Y;
            double distPx = Math.Sqrt(dx * dx + dy * dy);
            double distMm = distPx * MmPerPixel;

            // 7. Vẽ lên ảnh
            Mat annotated = src.Clone();
            Cv2.Circle(annotated, tip1, 6, new Scalar(0, 255, 0), -1);
            Cv2.Circle(annotated, tip2, 6, new Scalar(0, 0, 255), -1);
            Cv2.Line(annotated, tip1, tip2, new Scalar(255, 0, 0), 2);

            return new GapResult
            {
                GapPx = distPx,
                GapMm = distMm,
                AnnotatedImage = annotated
            };
        }
        /// <summary>
        /// Đo khe giữa hai đường song song sub-pixel với RANSAC
        /// </summary>
        private List<(Point P1, Point P2)> GetHoughSegments(Mat edges, LineOrientation ori)
        {
            var segs = new List<(Point, Point)>();
            // HoughLinesP trả về LineSegmentPoint[]
            var hough = Cv2.HoughLinesP(edges, 1, Math.PI / 180, 50, minLineLength: 20, maxLineGap: 5);
            if (hough != null)
            {
                foreach (var seg in hough)
                {
                    Point p = seg.P1;
                    Point q = seg.P2;
                    double dx = q.X - p.X;
                    double dy = q.Y - p.Y;
                    bool keep = ori == LineOrientation.Any
                        || (ori == LineOrientation.Horizontal && Math.Abs(dy) < Math.Abs(dx))
                        || (ori == LineOrientation.Vertical && Math.Abs(dx) < Math.Abs(dy));
                    if (keep)
                        segs.Add((p, q));
                }
            }
            return segs;
        }

        private (double minPx, double maxPx, double avgPx) ComputeSegmentStats(IEnumerable<(Point P1, Point P2)> segs)
    {
        var lens = segs.Select(s => Math.Sqrt((s.P2.X - s.P1.X) * (s.P2.X - s.P1.X) + (s.P2.Y - s.P1.Y) * (s.P2.Y - s.P1.Y))).ToArray();
        return lens.Length > 0 ? (lens.Min(), lens.Max(), lens.Average()) : (0, 0, 0);
        }
        private List<Point2f> GetInliers(Line2D ln, IEnumerable<Point2f> pts)
        {
            GetLineParams(ln, out double a, out double b, out double c);
            double norm = Math.Sqrt(a * a + b * b);
            return pts.Where(p => Math.Abs(a * p.X + b * p.Y + c) / norm < RansacThreshold).ToList();
        }
        private void DrawTicks(Mat img, Point p, LineOrientation ori)
        {
            int tickLen = 10;
            if (ori == LineOrientation.Any || ori == LineOrientation.Vertical)
            {
                // Vertical ticks
                Cv2.Line(img, new Point(p.X, p.Y - tickLen), new Point(p.X, p.Y + tickLen), new Scalar(255, 128, 0), 2);
            }
            else
            {
                // Horizontal ticks
                Cv2.Line(img, new Point(p.X - tickLen, p.Y), new Point(p.X + tickLen, p.Y), new Scalar(255, 128, 0), 2);
            }
        }
        public GapResult MeasureParallelGap(Mat raw, int numLines, GapExtremum extremum, LineOrientation orientation = LineOrientation.Any, SegmentStatType? segStat = null)
        {
            // 1. Preprocess
            Mat src = LoadAndPreprocess(raw, out Mat gray, out Mat edges);
            List<Point2f> pts = ExtractEdgePoints(gray, edges);

            // 2. Iterative RANSAC thu N lines
            var lines = new List<Line2D>();
            var remaining = new List<Point2f>(pts);
            for (int i = 0; i < numLines; i++)
            {
                var line = RansacFitLine(remaining, out var inliers,10);
                lines.Add(line);
                remaining = remaining.Except(inliers).ToList();
                if (remaining.Count < 2) break;
            }
            Mat annotated = src.Clone();
          
          //  Cv2.ImShow("raw", annotated);
            if (lines.Count < 2)
                throw new InvalidOperationException("Không tìm đủ số lines yêu cầu.");

            // 3. Lọc theo orientation
            var filtered = orientation == LineOrientation.Any
                ? lines
                : lines.Where(l =>
                {
                    double vx = l.Vx;
                    double vy = l.Vy;
                    if (orientation == LineOrientation.Horizontal)
                        return Math.Abs(vy) < Math.Abs(vx);
                    else // Vertical
                        return Math.Abs(vx) < Math.Abs(vy);
                })
                .ToList();
           
          //      throw new InvalidOperationException("Không tìm đủ lines với orientation yêu cầu.");
            lines = filtered;

            // 4. Vẽ tất cả lines nền (xám nhạt)

            if (filtered.Count < 2)

                return new GapResult { GapPx = 0, GapMm = 0, AnnotatedImage = annotated };
            foreach (var l in lines)
                DrawInfiniteLine(annotated, l, new Scalar(200, 200, 200), 1);

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
            DrawInfiniteLine(annotated, lA, new Scalar(0, 255, 0), 2);
            DrawInfiniteLine(annotated, lB, new Scalar(0, 0, 255), 2);
            // 1) Tính vùng y chung của hai line (dựa trên inliers nếu đã lưu):
            //    Nếu bạn không có inliers, bạn có thể dùng 2 điểm P1,P2 của mỗi line:

            // 7) Tự động tìm yTop/yBot dựa trên inliers
            var inlA = GetInliers(lA, pts);
            var inlB = GetInliers(lB, pts);
            double yTop = 0, yBot = 0, xLeft = 0, xRight = 0;
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
            // 9) Compute Short (Top) and Long (Bot) gaps
            // Short = gap tại yTop hoặc xLeft; Long = tại yBot hoặc xRight
            double shortPx, longPx;
            if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
            {
                shortPx = Math.Abs(SolveX(lB, yTop) - SolveX(lA, yTop));
                longPx = Math.Abs(SolveX(lB, yBot) - SolveX(lA, yBot));
            }
            else
            {
                shortPx = Math.Abs(SolveY(lB, xLeft) - SolveY(lA, xLeft));
                longPx = Math.Abs(SolveY(lB, xRight) - SolveY(lA, xRight));
            }
            double shortMm = shortPx * MmPerPixel;
            double longMm = longPx * MmPerPixel;

            // 10) Vẽ Short ticks & label
            if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
            {
                var pTs = new Point((int)Math.Round(SolveX(lA, yTop)), (int)Math.Round(yTop));
                var pTe = new Point((int)Math.Round(SolveX(lB, yTop)), (int)Math.Round(yTop));
                DrawTicks(annotated, pTs, orientation);
                DrawTicks(annotated, pTe, orientation);
                Cv2.Line(annotated, pTs, pTe, new Scalar(128, 200, 255), 2);
                Cv2.PutText(annotated, $"Short: {shortMm:F2} mm", new Point(pTs.X, pTs.Y + 15), HersheyFonts.HersheySimplex, 0.7, new Scalar(128, 200, 255), 2);
            }
            else
            {
                var pTs = new Point((int)Math.Round(xLeft), (int)Math.Round(SolveY(lA, xLeft)));
                var pTe = new Point((int)Math.Round(xLeft), (int)Math.Round(SolveY(lB, xLeft)));
                DrawTicks(annotated, pTs, orientation);
                DrawTicks(annotated, pTe, orientation);
                Cv2.Line(annotated, pTs, pTe, new Scalar(128, 200, 255), 2);
                Cv2.PutText(annotated, $"Short: {shortMm:F2} mm", new Point(pTs.X + 5, (pTs.Y + pTe.Y) / 2), HersheyFonts.HersheySimplex, 0.7, new Scalar(128, 200, 255), 2);
            }

            // 11) Vẽ Long
            if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
            {
                var pLs = new Point((int)Math.Round(SolveX(lA, yBot)), (int)Math.Round(yBot));
                var pLe = new Point((int)Math.Round(SolveX(lB, yBot)), (int)Math.Round(yBot));
                DrawTicks(annotated, pLs, orientation);
                DrawTicks(annotated, pLe, orientation);
                Cv2.Line(annotated, pLs, pLe, new Scalar(0, 128, 255), 3);
                Cv2.PutText(annotated, $"Long: {longMm:F2} mm", new Point(pLs.X, pLs.Y - 10), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 128, 255), 2);
            }
            else
            {
                var pLs = new Point((int)Math.Round(xRight), (int)Math.Round(SolveY(lA, xRight)));
                var pLe = new Point((int)Math.Round(xRight), (int)Math.Round(SolveY(lB, xRight)));
                DrawTicks(annotated, pLs, orientation);
                DrawTicks(annotated, pLe, orientation);
                Cv2.Line(annotated, pLs, pLe, new Scalar(0, 128, 255), 3);
                Cv2.PutText(annotated, $"Long: {longMm:F2} mm", new Point(pLs.X + 5, (pLs.Y + pLe.Y) / 2), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 128, 255), 2);
            }

            // 12) Vẽ Mid
            if (orientation == LineOrientation.Any || orientation == LineOrientation.Vertical)
            {
                double yMid = (yTop + yBot) / 2;
                var pMs = new Point((int)Math.Round(SolveX(lA, yMid)), (int)Math.Round(yMid));
                var pMe = new Point((int)Math.Round(SolveX(lB, yMid)), (int)Math.Round(yMid));
                DrawTicks(annotated, pMs, orientation);
                DrawTicks(annotated, pMe, orientation);
                Cv2.Line(annotated, pMs, pMe, new Scalar(0, 0, 255), 2);
                double midMm = Math.Abs(pMe.X - pMs.X) * MmPerPixel;
                Cv2.PutText(annotated, $"Mid: {midMm:F2} mm", new Point(pMs.X, pMs.Y + 5), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 0, 255), 2);
            }
            else
            {
                double xMid = (xLeft + xRight) / 2;
                var pMs = new Point((int)Math.Round(xMid), (int)Math.Round(SolveY(lA, xMid)));
                var pMe = new Point((int)Math.Round(xMid), (int)Math.Round(SolveY(lB, xMid)));
                DrawTicks(annotated, pMs, orientation);
                DrawTicks(annotated, pMe, orientation);
                Cv2.Line(annotated, pMs, pMe, new Scalar(0, 0, 255), 2);
                double midMm = Math.Abs(pMe.Y - pMs.Y) * MmPerPixel;
                Cv2.PutText(annotated, $"Mid: {midMm:F2} mm", new Point(pMs.X + 5, (pMs.Y + pMe.Y) / 2), HersheyFonts.HersheySimplex, 0.7, new Scalar(0, 0, 255), 2);
            }

            // 13) Return kết quả theo extremum
            double resultPx = extremum == GapExtremum.Farthest ? longPx : shortPx;
            double resultMm = resultPx * MmPerPixel;
            return new GapResult { GapPx = resultPx, GapMm = resultMm, AnnotatedImage = annotated };

            //return new GapResult { GapPx = gapPx, GapMm = gapMm, AnnotatedImage = annotated };
        }
        /// <summary>
        /// Đo khoảng cách giữa hai điểm đỉnh xa nhất (convex hull)
        /// </summary>
        public GapResult MeasureVertexGap(string imagePath)
        {
            Mat src = Cv2.ImRead(imagePath);
            if (src.Empty())
                throw new ArgumentException($"Không tìm thấy ảnh: {imagePath}");

            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 0);

            Mat bin = new Mat();
            Cv2.Threshold(gray, bin, 100, 255, ThresholdTypes.BinaryInv);

            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(bin, out contours, out hierarchy,
                             RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            var main = contours.OrderByDescending(c => Cv2.ContourArea(c)).FirstOrDefault();
            if (main == null || main.Length < 2)
                throw new InvalidOperationException("Không tìm được contour phù hợp.");

            Point[] hull = Cv2.ConvexHull(main, clockwise: false);

            double maxD = 0;
            Point pA = new Point(), pB = new Point();
            for (int i = 0; i < hull.Length; i++)
            {
                for (int j = i + 1; j < hull.Length; j++)
                {
                    int dx = hull[i].X - hull[j].X;
                    int dy = hull[i].Y - hull[j].Y;
                    double d = Math.Sqrt(dx * dx + dy * dy);
                    if (d > maxD)
                    {
                        maxD = d;
                        pA = hull[i];
                        pB = hull[j];
                    }
                }
            }

            double dMm = maxD * MmPerPixel;
            Mat annotated = src.Clone();
            Cv2.Line(annotated, pA, pB, new Scalar(255, 0, 0), 3);
            Cv2.Circle(annotated, pA, 5, new Scalar(0, 255, 0), -1);
            Cv2.Circle(annotated, pB, 5, new Scalar(0, 0, 255), -1);

            return new GapResult { GapPx = maxD, GapMm = dMm, AnnotatedImage = annotated };
        }

        /// <summary>
        /// Tiền xử lý ảnh: grayscale → CLAHE → bilateral → auto-Canny → closing → dilate/erode
        /// </summary>
        private Mat LoadAndPreprocess(Mat raw, out Mat gray, out Mat edges)
        {
            // 1) Chuyển sang grayscale
            gray = new Mat();
            Cv2.CvtColor(raw, gray, ColorConversionCodes.BGR2GRAY);

            // 2) CLAHE (adaptive histogram equalization) để tăng tương phản
            var clahe = Cv2.CreateCLAHE(clipLimit: 2.0, tileGridSize: new Size(8, 8));
            clahe.Apply(gray, gray);

            // 3) Giảm nhiễu nhưng vẫn giữ cạnh: bilateral filter
            var smooth = new Mat();
            Cv2.BilateralFilter(gray, smooth, d: 9, sigmaColor: 75, sigmaSpace: 75);

            // 4) Tự động tính ngưỡng Canny dựa trên median của ảnh
            int total = gray.Rows * gray.Cols;
            var pixelData = new byte[total];
            System.Runtime.InteropServices.Marshal.Copy(gray.Data, pixelData, 0, total);
            Array.Sort(pixelData);
            double median = pixelData[total / 2];
            double sigma = 0.33;
            int lower = (int)Math.Max(0, (1.0 - sigma) * median);
            int upper = (int)Math.Min(255, (1.0 + sigma) * median);

            // 5) Dò Canny
            edges = new Mat();
            Cv2.Canny(smooth, edges, lower, upper);

            // 6) Morphological closing để nối đoạn đứt + dilate/erode để làm dày mỏng cạnh
            var kernelClose = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
            Cv2.MorphologyEx(edges, edges, MorphTypes.Close, kernelClose);
            var kernelDil = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.Dilate(edges, edges, kernelDil, iterations: 1);
            Cv2.Erode(edges, edges, kernelDil, iterations: 1);

            // Debug: hiển thị kết quả
            Cv2.ImShow("Edges (auto-optimized)", edges);

            // Trả về raw để vẽ annotate lên
            return raw;
        }

        private List<Point2f> ExtractEdgePoints(Mat gray, Mat edges)
        {
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(edges, out contours, out hierarchy,
                             RetrievalModes.External, ContourApproximationModes.ApproxNone);
            var ptsArray = contours.SelectMany(c => c)
                .Select(p => new Point2f(p.X, p.Y)).ToArray();
            Cv2.CornerSubPix(gray, ptsArray, new Size(5, 5), new Size(-1, -1),
                new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 30, 0.1));
            return ptsArray.ToList();
        }

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
            var rand = new Random();
            int bestCount = 0;
            inliers = new List<Point2f>();
            Line2D bestLine = default;

            for (int i = 0; i < RansacIterations; i++)
            {
                if (pts.Count < 2) break;
                int i1 = rand.Next(pts.Count), i2 = rand.Next(pts.Count);
                if (i1 == i2) continue;

                var p1 = pts[i1];
                var p2 = pts[i2];

                // 1) Kiểm tra độ dài segment
                double dx = p2.X - p1.X, dy = p2.Y - p1.Y;
                double segLen = Math.Sqrt(dx * dx + dy * dy);
                if (segLen < minLen || segLen > maxLen)
                    continue;

                // 2) Kiểm tra góc lệch so với Ox
                double angle = Math.Abs(Math.Atan2(dy, dx) * 180.0 / Math.PI);
                if (angle < minAngleDeg || angle > maxAngleDeg)
                    continue;

                // 3) Tính tham số line ax+by+c=0
                double a = p2.Y - p1.Y;
                double b = p1.X - p2.X;
                double norm = Math.Sqrt(a * a + b * b);
                if (norm < 1e-6) continue;
                double c = -(a * p1.X + b * p1.Y);

                // 4) Tập inliers
                var currInliers = pts
                    .Where(p => Math.Abs(a * p.X + b * p.Y + c) / norm < RansacThreshold)
                    .ToList();

                if (currInliers.Count > bestCount)
                {
                    bestCount = currInliers.Count;
                    inliers = currInliers;
                }
            }

            // 5) Fit line cuối cùng trên inliers tốt nhất
            if (inliers.Count >= 2)
            {
                // FitLine trả về vx,vy,x0,y0
                bestLine = Cv2.FitLine(inliers.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
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
