using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
namespace BeeCore.Algorithm
{
    

    public enum CircleScanDirection
    {
        InsideOut,
        OutsideIn
    }

    public class RansacCircleFitter
    {
        public static List<(Point2f center, float radius,int inliers)> DetectCircles(
            Mat edgeImage,
            int maxCircles = 5,
            int iterations = 300,
            float threshold = 2.0f,
            int minInliers = 50,
            CircleScanDirection direction = CircleScanDirection.InsideOut,int MinRadius=0,int MaxRadius=500)
        {
            var edgePoints = GetEdgePoints(edgeImage);
            return RansacSingleBestCircle(edgePoints, maxCircles, iterations, threshold, minInliers, direction, MinRadius, MaxRadius);
        }

        private static List<Point2f> GetEdgePoints(Mat edgeImage)
        {
            var points = new List<Point2f>();
            for (int y = 0; y < edgeImage.Rows; y++)
            {
                for (int x = 0; x < edgeImage.Cols; x++)
                {
                    if (edgeImage.At<byte>(y, x) > 0)
                        points.Add(new Point2f(x, y));
                }
            }
            return points;
        }

        //        Parallel.For(0, iterations, i =>
        //                {
        //                    var sample = points.OrderBy(_ => rnd.Next()).Take(3).ToArray();
        //                    if (!CircleFromPoints(sample[0], sample[1], sample[2], out Point2f center, out float radius))
        //                        return;
        //                    if (radius< 5 || radius> 500) return;

        //                    int inliers = CountInliers(points, center, radius, threshold);

        //                    lock (lockObj)
        //                    {
        //                        if (inliers > minInliers)
        //                        {
        //                            bestInliers[i] = inliers;
        //                            bestCenters[i] = center;
        //                            bestRadius[i] = radius;
        //                        }
        //}
        //                });
        //int indexBest = 0;
        //int j = 0;
        //foreach (int inliers in bestInliers)
        //{
        //    if (inliers > bestInlier)
        //    {
        //        bestInlier = inliers;
        //        indexBest = j;
        //    }
        //    j++;
        //}
        //if (bestInliers[indexBest] >= minInliers)
        //{
        //    foundCircles.Add((bestCenters[indexBest], bestRadius[indexBest]));
        //    points = points.Where(p => Math.Abs(Distance(p, bestCenters[indexBest]) - bestRadius[indexBest]) > threshold).ToList();
        //}
        //else
        //{
        //    break;
        //}
        private static Dictionary<(int, int), List<Point2f>> BuildGrid(List<Point2f> points)
        {
            var grid = new Dictionary<(int, int), List<Point2f>>();
            foreach (var pt in points)
            {
                var key = ((int)(pt.X / GridSize), (int)(pt.Y / GridSize));
                if (!grid.ContainsKey(key))
                    grid[key] = new List<Point2f>();
                grid[key].Add(pt);
            }
            return grid;
        }

        private static int CountInliersGrid(Dictionary<(int, int), List<Point2f>> grid, Point2f center, float radius, float threshold, int earlyExitLimit)
        {
            int count = 0;
            int r = (int)(radius + threshold);
            int minX = (int)((center.X - r) / GridSize);
            int maxX = (int)((center.X + r) / GridSize);
            int minY = (int)((center.Y - r) / GridSize);
            int maxY = (int)((center.Y + r) / GridSize);

            for (int gx = minX; gx <= maxX; gx++)
            {
                for (int gy = minY; gy <= maxY; gy++)
                {
                    if (!grid.TryGetValue((gx, gy), out var cellPoints)) continue;
                    foreach (var pt in cellPoints)
                    {
                        float dist = Distance(pt, center);
                        if (Math.Abs(dist - radius) < threshold)
                        {
                            count++;
                            if (count > earlyExitLimit)
                                return count;
                        }
                    }
                }
            }
            return count;
        }
        private const int GridSize = 10;
        public static List<(Point2f center, float radius,int minInliers)> RansacSingleBestCircle(
           List<Point2f> edgePoints,
           int maxCircles = 5,
           int? maxIterations = null,
           float threshold = 2.0f,
           int? minInliers = null,
            CircleScanDirection direction = CircleScanDirection.InsideOut,
            int MinRadius = 0, int MaxRadius = 500,
            bool earlyBreak = false)
        {
            var points = new List<Point2f>(edgePoints);
            Random rnd = new Random();
            var grid = BuildGrid(points);

            int iterations = maxIterations ?? Math.Min(Math.Max(points.Count / 5, 100), 1000);
            int inliersNeeded = minInliers ?? Math.Max(15, (int)(points.Count * 0.02));

           // (Point2f center, float radius, int inliers) best = (new Point2f(), 0f, 0);

            int globalSeed = 12345;
            var threadRandom = new ThreadLocal<Random>(() => new Random(globalSeed + Thread.CurrentThread.ManagedThreadId));
            var results = new ConcurrentBag<(Point2f center, float radius, int inliers)>();

            Parallel.ForEach(Partitioner.Create(0, iterations), (range, state) =>
            {
                var localRnd = threadRandom.Value;
                (Point2f center, float radius, int inliers) localBest = (new Point2f(), 0f, 0);

                for (int i = range.Item1; i < range.Item2; i++)
                {
                    if (points.Count < 3) break;

                    var indices = Enumerable.Range(0, points.Count).OrderBy(_ => localRnd.Next()).Take(3).ToArray();
                    var sample = new[] { points[indices[0]], points[indices[1]], points[indices[2]] };

                    if (!CircleFromPoints(sample[0], sample[1], sample[2], out Point2f center, out float radius))
                        continue;
                    if (radius < MinRadius || radius > MaxRadius) continue;

                    int inliers = CountInliersGrid(grid, center, radius, threshold, earlyBreak ? int.MaxValue : int.MaxValue);

                    if (inliers >= inliersNeeded)
                    {
                        results.Add((center, radius, inliers));
                    }
                }
            });

            var sorted = direction == CircleScanDirection.InsideOut
                ? results.OrderByDescending(r => r.inliers).ThenBy(r => r.radius)
                : results.OrderByDescending(r => r.inliers).ThenByDescending(r => r.radius);
            var foundCircles = new List<(Point2f, float, int)>();
            var best = sorted.FirstOrDefault();

            if (best.inliers >= inliersNeeded)
                foundCircles.Add((best.center, best.radius, best.inliers));
           // return new List<(Point2f, float, int)> { (best.center, best.radius, best.inliers) };
            //lock (points)
            //{
            //    bool isBetter = false;
            //    if (direction == CircleScanDirection.InsideOut)
            //        isBetter = localBest.inliers > best.inliers || (localBest.inliers == best.inliers && localBest.radius < best.radius);
            //    else
            //        isBetter = localBest.inliers > best.inliers || (localBest.inliers == best.inliers && localBest.radius > best.radius);

            //    if (isBetter)
            //        best = localBest;
            //}
        
            //var sorted = direction == CircleScanDirection.InsideOut
            //? results.OrderBy(r => r.radius).ThenByDescending(r => r.inliers)
            //: results.OrderByDescending(r => r.radius).ThenByDescending(r => r.inliers);

            //var best = sorted.FirstOrDefault();

            //if (best.inliers >= inliersNeeded)
            //    return new List<(Point2f, float, int)> { (best.center, best.radius, best.inliers) };
          
            //if (best.inliers >= inliersNeeded)
            //{
            //    foundCircles.Add((best.center, best.radius, best.inliers));
            //    //{

            //    //    Inliers = best.inliers
            //    //};
            //}
            return foundCircles;
            
        }
        //public static List<(Point2f center, float radius)> RansacMultiCircle2(
        //    List<Point2f> edgePoints,
        //    int maxCircles = 5,
        //    int? maxIterations = null,
        //    float threshold = 2.0f,
        //    int? minInliers = null,
            
        //    CircleScanDirection direction = CircleScanDirection.InsideOut,
        //    bool earlyBreak = false)
        //{
        //    var foundCircles = new List<(Point2f, float)>();
        //    var points = new List<Point2f>(edgePoints);
        //    Random rnd = new Random();

        //    var grid = BuildGrid(points);

        //    int iterations = maxIterations ?? Math.Min(Math.Max(points.Count / 5, 100), 1000); //Math.Clamp(points.Count / 5, 100, 1000);
        //    int inliersNeeded = minInliers ?? Math.Max(15, (int)(points.Count * 0.02));

        //    for (int circleCount = 0; circleCount < maxCircles && points.Count > inliersNeeded; circleCount++)
        //    {
        //        (Point2f center, float radius, int inliers) best = (new Point2f(), 0f, 0);

        //        Parallel.ForEach(Partitioner.Create(0, iterations), range =>
        //        {
        //            (Point2f center, float radius, int inliers) localBest = (new Point2f(), 0f, 0);
        //            var localRnd = new Random(Guid.NewGuid().GetHashCode());

        //            for (int i = range.Item1; i < range.Item2; i++)
        //            {
        //                int i1 = localRnd.Next(points.Count);
        //                int i2, i3;
        //                do { i2 = localRnd.Next(points.Count); } while (i2 == i1);
        //                do { i3 = localRnd.Next(points.Count); } while (i3 == i1 || i3 == i2);

        //                var sample = new[] { points[i1], points[i2], points[i3] };
        //                if (!CircleFromPoints(sample[0], sample[1], sample[2], out Point2f center, out float radius))
        //                    continue;
        //                if (radius < 5 || radius > 500) continue;

        //                int inliers = CountInliersGrid(grid, center, radius, threshold, earlyBreak ? best.inliers : int.MaxValue);

        //                bool isBetter = false;
        //                if (direction == CircleScanDirection.InsideOut)
        //                {
        //                    isBetter = (inliers > localBest.inliers) ||
        //                               (inliers == localBest.inliers && radius < localBest.radius);
        //                }
        //                else // OutsideIn
        //                {
        //                    isBetter = (inliers > localBest.inliers) ||
        //                               (inliers == localBest.inliers && radius > localBest.radius);
        //                }

        //                if (isBetter)
        //                    localBest = (center, radius, inliers);
        //            }

        //            lock (points)
        //            {
        //                bool isBetter = false;
        //                if (direction == CircleScanDirection.InsideOut)
        //                {
        //                    isBetter = (localBest.inliers > best.inliers) ||
        //                               (localBest.inliers == best.inliers && localBest.radius < best.radius);
        //                }
        //                else // OutsideIn
        //                {
        //                    isBetter = (localBest.inliers > best.inliers) ||
        //                               (localBest.inliers == best.inliers && localBest.radius > best.radius);
        //                }

        //                if (isBetter)
        //                    best = localBest;
        //            }
        //        });

        //        if (best.inliers >= inliersNeeded)
        //        {
        //            foundCircles.Add((best.center, best.radius));
        //            points = points.Where(p => Math.Abs(Distance(p, best.center) - best.radius) > threshold).ToList();
        //            grid = BuildGrid(points);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    if (direction == CircleScanDirection.OutsideIn)
        //        foundCircles.Sort((a, b) => b.Item2.CompareTo(a.Item2));  // b.radius → b.Item2
        //    else
        //        foundCircles.Sort((a, b) => a.Item2.CompareTo(b.Item2));  // a.radius → a.Item2
        //    if (foundCircles.Count() == 0)
        //        return new List<(Point2f center, float radius)>();
        //    return new List<(Point2f center, float radius)> { foundCircles[0] };
           
        //}
        private static List<(Point2f center, float radius)> RansacMultiCircle(
            List<Point2f> edgePoints,
            int maxCircles,
            int iterations,
            float threshold,
            int minInliers,
            CircleScanDirection direction)
        {
            var foundCircles = new List<(Point2f, float)>();
            var points = new List<Point2f>(edgePoints);
            Random rnd = new Random();

            for (int circleCount = 0; circleCount < maxCircles && points.Count > minInliers; circleCount++)
            {
                int bestInliers = 0;
                Point2f bestCenter = new Point2f();
                float bestRadius = 0;

                for (int i = 0; i < iterations; i++)
                {
                    int i1 = rnd.Next(points.Count);
                    int i2, i3;
                    do { i2 = rnd.Next(points.Count); } while (i2 == i1);
                    do { i3 = rnd.Next(points.Count); } while (i3 == i1 || i3 == i2);
                    var sample = new[] { points[i1], points[i2], points[i3] };
                    if (!CircleFromPoints(sample[0], sample[1], sample[2], out Point2f center, out float radius))
                        continue;

                    if (radius < 5 || radius > 500) continue;

                    int inliers = CountInliers(points, center, radius, threshold);
                    if (inliers > bestInliers)
                    {
                        bestInliers = inliers;
                        bestCenter = center;
                        bestRadius = radius;
                    }
                }

                if (bestInliers >= minInliers)
                {
                    foundCircles.Add((bestCenter, bestRadius));

                    // Loại bỏ inliers khỏi tập
                    points = points
                        .Where(p => Math.Abs(Distance(p, bestCenter) - bestRadius) > threshold)
                        .ToList();
                }
                else
                {
                    break;
                }
            }

            if (direction == CircleScanDirection.OutsideIn)
                foundCircles.Sort((a, b) => b.Item2.CompareTo(a.Item2));  // b.radius → b.Item2
            else
                foundCircles.Sort((a, b) => a.Item2.CompareTo(b.Item2));  // a.radius → a.Item2
            if (foundCircles.Count() == 0)
                return new List<(Point2f center, float radius)>();
            return new List<(Point2f center, float radius)> { foundCircles[0] };
        }

        private static bool CircleFromPoints(Point2f p1, Point2f p2, Point2f p3, out Point2f center, out float radius)
        {
            float x1 = p1.X, y1 = p1.Y;
            float x2 = p2.X, y2 = p2.Y;
            float x3 = p3.X, y3 = p3.Y;

            float a = x1 * (y2 - y3) - y1 * (x2 - x3) + x2 * y3 - x3 * y2;

            if (Math.Abs(a) < 1e-6)
            {
                center = new Point2f();
                radius = 0;
                return false;
            }

            float A = x1 * x1 + y1 * y1;
            float B = x2 * x2 + y2 * y2;
            float C = x3 * x3 + y3 * y3;

            float D = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            float ux = (A * (y2 - y3) + B * (y3 - y1) + C * (y1 - y2)) / D;
            float uy = (A * (x3 - x2) + B * (x1 - x3) + C * (x2 - x1)) / D;

            center = new Point2f(ux, uy);
            radius = (float)Math.Sqrt((ux - x1) * (ux - x1) + (uy - y1) * (uy - y1));
            return true;
        }

        private static int CountInliers(List<Point2f> points, Point2f center, float radius, float threshold)
        {
            int count = 0;
            foreach (var pt in points)
            {
                float dist = Distance(pt, center);
                if (Math.Abs(dist - radius) < threshold)
                    count++;
            }
            return count;
        }

        private static float Distance(Point2f a, Point2f b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }

}
