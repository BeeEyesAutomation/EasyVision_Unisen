using System;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;

namespace Measurement
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

    /// <summary>
    /// Lớp đo khe:
    /// - MeasureParallelGap: đo giữa hai đường song song (sub-pixel + RANSAC)
    /// - MeasureVertexGap: đo khoảng cách giữa hai điểm đỉnh xa nhất (convex hull)
    /// </summary>
    public class ParallelGapDetector
    {
        public double MmPerPixel { get; set; }
        public int CannyThreshold1 { get; set; } = 50;
        public int CannyThreshold2 { get; set; } = 150;
        public int RansacIterations { get; set; } = 1000;
        public double RansacThreshold { get; set; } = 2.0;

        public ParallelGapDetector(double mmPerPixel)
        {
            MmPerPixel = mmPerPixel;
        }

        /// <summary>
        /// Đo khe giữa hai đường song song sub-pixel với RANSAC
        /// </summary>
        public GapResult MeasureParallelGap(string imagePath)
        {
            Mat src = LoadAndPreprocess(imagePath, out Mat gray, out Mat edges);
            List<Point2f> points = ExtractEdgePoints(gray, edges);

            Line2D line1 = RansacFitLine(points, out List<Point2f> inliers1);
            List<Point2f> remaining = points.Except(inliers1).ToList();
            Line2D line2 = RansacFitLine(remaining, out _);

            double dPx = DistanceBetweenLines(line1, line2);
            double dMm = dPx * MmPerPixel;

            Mat annotated = src.Clone();
            DrawInfiniteLine(annotated, line1, new Scalar(0, 255, 0), 2);
            DrawInfiniteLine(annotated, line2, new Scalar(0, 0, 255), 2);

            return new GapResult { GapPx = dPx, GapMm = dMm, AnnotatedImage = annotated };
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

        private Mat LoadAndPreprocess(string path, out Mat gray, out Mat edges)
        {
            Mat src = Cv2.ImRead(path);
            if (src.Empty()) throw new ArgumentException($"Không tìm thấy ảnh: {path}");
            gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 0);
            edges = new Mat();
            Cv2.Canny(gray, edges, CannyThreshold1, CannyThreshold2);
            return src;
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

        private Line2D RansacFitLine(List<Point2f> pts, out List<Point2f> inliers)
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

                var p1 = pts[i1]; var p2 = pts[i2];
                double a = p2.Y - p1.Y, b = p1.X - p2.X;
                double norm = Math.Sqrt(a * a + b * b);
                if (norm < 1e-6) continue;
                double c = -(a * p1.X + b * p1.Y);

                var currInliers = pts.Where(p => Math.Abs(a * p.X + b * p.Y + c) / norm < RansacThreshold).ToList();
                if (currInliers.Count > bestCount)
                {
                    bestCount = currInliers.Count;
                    inliers = currInliers;
                }
            }

            if (inliers.Count >= 2)
                bestLine = Cv2.FitLine(inliers.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);

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
