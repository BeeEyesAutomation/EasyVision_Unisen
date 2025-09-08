//using BeeGlobal;
//using OpenCvSharp;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using static System.Windows.Forms.AxHost;
//namespace BeeCore.Algorithm
//{


//    public enum CircleScanDirection
//    {
//        InsideOut,
//        OutsideIn
//    }

//    public class RansacCircleFitter
//    {
//        public static List<(Point2f center, float radius,int inliers)> DetectCircles(
//            Mat edgeImage,
//            int maxCircles = 5,
//            int iterations = 300,
//            float threshold = 2.0f,
//            int minInliers = 50,
//            CircleScanDirection direction = CircleScanDirection.InsideOut,int MinRadius=0,int MaxRadius=500)
//        {
//            var edgePoints = GetEdgePoints(edgeImage);
//            return RansacSingleBestCircle(edgePoints, maxCircles, iterations, threshold, minInliers, direction, MinRadius, MaxRadius);
//        }

//        private static List<Point2f> GetEdgePoints(Mat edgeImage)
//        {
//            var points = new List<Point2f>();
//            for (int y = 0; y < edgeImage.Rows; y++)
//            {
//                for (int x = 0; x < edgeImage.Cols; x++)
//                {
//                    if (edgeImage.At<byte>(y, x) > 0)
//                        points.Add(new Point2f(x, y));
//                }
//            }
//            return points;
//        }

//        //        Parallel.For(0, iterations, i =>
//        //                {
//        //                    var sample = points.OrderBy(_ => rnd.Next()).Take(3).ToArray();
//        //                    if (!CircleFromPoints(sample[0], sample[1], sample[2], out Point2f center, out float radius))
//        //                        return;
//        //                    if (radius< 5 || radius> 500) return;

//        //                    int inliers = CountInliers(points, center, radius, threshold);

//        //                    lock (lockObj)
//        //                    {
//        //                        if (inliers > minInliers)
//        //                        {
//        //                            bestInliers[i] = inliers;
//        //                            bestCenters[i] = center;
//        //                            bestRadius[i] = radius;
//        //                        }
//        //}
//        //                });
//        //int indexBest = 0;
//        //int j = 0;
//        //foreach (int inliers in bestInliers)
//        //{
//        //    if (inliers > bestInlier)
//        //    {
//        //        bestInlier = inliers;
//        //        indexBest = j;
//        //    }
//        //    j++;
//        //}
//        //if (bestInliers[indexBest] >= minInliers)
//        //{
//        //    foundCircles.Add((bestCenters[indexBest], bestRadius[indexBest]));
//        //    points = points.Where(p => Math.Abs(Distance(p, bestCenters[indexBest]) - bestRadius[indexBest]) > threshold).ToList();
//        //}
//        //else
//        //{
//        //    break;
//        //}
//        private static Dictionary<(int, int), List<Point2f>> BuildGrid(List<Point2f> points)
//        {
//            var grid = new Dictionary<(int, int), List<Point2f>>();
//            foreach (var pt in points)
//            {
//                var key = ((int)(pt.X / GridSize), (int)(pt.Y / GridSize));
//                if (!grid.ContainsKey(key))
//                    grid[key] = new List<Point2f>();
//                grid[key].Add(pt);
//            }
//            return grid;
//        }

//        private static int CountInliersGrid(Dictionary<(int, int), List<Point2f>> grid, Point2f center, float radius, float threshold, int earlyExitLimit)
//        {
//            int count = 0;
//            int r = (int)(radius + threshold);
//            int minX = (int)((center.X - r) / GridSize);
//            int maxX = (int)((center.X + r) / GridSize);
//            int minY = (int)((center.Y - r) / GridSize);
//            int maxY = (int)((center.Y + r) / GridSize);

//            for (int gx = minX; gx <= maxX; gx++)
//            {
//                for (int gy = minY; gy <= maxY; gy++)
//                {
//                    if (!grid.TryGetValue((gx, gy), out var cellPoints)) continue;
//                    foreach (var pt in cellPoints)
//                    {
//                        float dist = Distance(pt, center);
//                        if (Math.Abs(dist - radius) < threshold)
//                        {
//                            count++;
//                            if (count > earlyExitLimit)
//                                return count;
//                        }
//                    }
//                }
//            }
//            return count;
//        }
//        private const int GridSize = 10;
//        //public static List<(Point2f center, float radius,int minInliers)> RansacSingleBestCircle(
//        //   List<Point2f> edgePoints,
//        //   int maxCircles = 5,
//        //   int? maxIterations = null,
//        //   float threshold = 2.0f,
//        //   int? minInliers = null,
//        //    CircleScanDirection direction = CircleScanDirection.InsideOut,
//        //    int MinRadius = 0, int MaxRadius = 500,
//        //    bool earlyBreak = false)
//        //{
//        //    var points = new List<Point2f>(edgePoints);
//        //    Random rnd = new Random();
//        //    var grid = BuildGrid(points);

//        //    int iterations = maxIterations ?? Math.Min(Math.Max(points.Count / 5, 100), 1000);
//        //    int inliersNeeded = minInliers ?? Math.Max(15, (int)(points.Count * 0.02));

//        //   // (Point2f center, float radius, int inliers) best = (new Point2f(), 0f, 0);

//        //    int globalSeed = 12345;
//        //    var threadRandom = new ThreadLocal<Random>(() => new Random(globalSeed + Thread.CurrentThread.ManagedThreadId));
//        //    var results = new ConcurrentBag<(Point2f center, float radius, int inliers)>();

//        //    Parallel.ForEach(Partitioner.Create(0, iterations), (range, state) =>
//        //    {
//        //        var localRnd = threadRandom.Value;
//        //        (Point2f center, float radius, int inliers) localBest = (new Point2f(), 0f, 0);

//        //        for (int i = range.Item1; i < range.Item2; i++)
//        //        {
//        //            if (points.Count < 3) break;

//        //            var indices = Enumerable.Range(0, points.Count).OrderBy(_ => localRnd.Next()).Take(3).ToArray();
//        //            var sample = new[] { points[indices[0]], points[indices[1]], points[indices[2]] };

//        //            if (!CircleFromPoints(sample[0], sample[1], sample[2], out Point2f center, out float radius))
//        //                continue;
//        //            if (radius < MinRadius || radius > MaxRadius) continue;

//        //            int inliers = CountInliersGrid(grid, center, radius, threshold, earlyBreak ? int.MaxValue : int.MaxValue);

//        //            if (inliers >= inliersNeeded)
//        //            {
//        //                results.Add((center, radius, inliers));
//        //            }
//        //        }
//        //    });

//        //    var sorted = direction == CircleScanDirection.InsideOut
//        //        ? results.OrderByDescending(r => r.inliers).ThenBy(r => r.radius)
//        //        : results.OrderByDescending(r => r.inliers).ThenByDescending(r => r.radius);
//        //    var foundCircles = new List<(Point2f, float, int)>();
//        //    var best = sorted.FirstOrDefault();

//        //    if (best.inliers >= inliersNeeded)
//        //        foundCircles.Add((best.center, best.radius, best.inliers));
//        //   // return new List<(Point2f, float, int)> { (best.center, best.radius, best.inliers) };
//        //    //lock (points)
//        //    //{
//        //    //    bool isBetter = false;
//        //    //    if (direction == CircleScanDirection.InsideOut)
//        //    //        isBetter = localBest.inliers > best.inliers || (localBest.inliers == best.inliers && localBest.radius < best.radius);
//        //    //    else
//        //    //        isBetter = localBest.inliers > best.inliers || (localBest.inliers == best.inliers && localBest.radius > best.radius);

//        //    //    if (isBetter)
//        //    //        best = localBest;
//        //    //}

//        //    //var sorted = direction == CircleScanDirection.InsideOut
//        //    //? results.OrderBy(r => r.radius).ThenByDescending(r => r.inliers)
//        //    //: results.OrderByDescending(r => r.radius).ThenByDescending(r => r.inliers);

//        //    //var best = sorted.FirstOrDefault();

//        //    //if (best.inliers >= inliersNeeded)
//        //    //    return new List<(Point2f, float, int)> { (best.center, best.radius, best.inliers) };

//        //    //if (best.inliers >= inliersNeeded)
//        //    //{
//        //    //    foundCircles.Add((best.center, best.radius, best.inliers));
//        //    //    //{

//        //    //    //    Inliers = best.inliers
//        //    //    //};
//        //    //}
//        //    return foundCircles;

//        //}
//        public static List<(Point2f center, float radius, int minInliers)> RansacSingleBestCircle(
//     List<Point2f> edgePoints,
//     int maxCircles = 5,
//     int? maxIterations = null,
//     float threshold = 2.0f,
//     int? minInliers = null,
//     CircleScanDirection direction = CircleScanDirection.InsideOut,
//     int MinRadius = 0, int MaxRadius = 500,
//     bool earlyBreak = false)
//        {
//            var points = new List<Point2f>(edgePoints);
//            if (points.Count < 3) return new List<(Point2f, float, int)>();

//            // Giữ nguyên BuildGrid & cách lấy điểm của bạn (ổn định theo thứ tự quét y,x)
//            var grid = BuildGrid(points);

//            int iterations = maxIterations ?? Math.Min(Math.Max(points.Count / 5, 100), 1000);
//            int inliersNeeded = minInliers ?? Math.Max(15, (int)(points.Count * 0.02));

//            // ---- TIỀN TẠO TRIPLET DETERMINISTIC (seed cố định) ----
//            const int FIXED_SEED = 123456789; // đổi nếu muốn, nhưng cố định để tái lặp
//            var triplets = PrecomputeTriplets(points.Count, iterations, FIXED_SEED);

//            var results = new ConcurrentBag<(Point2f center, float radius, int inliers)>();

//            Parallel.ForEach(Partitioner.Create(0, iterations), (range) =>
//            {
//                for (int i = range.Item1; i < range.Item2; i++)
//                {
//                    if (points.Count < 3) break;

//                    // Dùng triplet đã tiền tạo → không phụ thuộc lịch lập lịch/thread
//                    var (i1, i2, i3) = triplets[i];
//                    var p1 = points[i1];
//                    var p2 = points[i2];
//                    var p3 = points[i3];

//                    if (!TripletGood(p1, p2, p3, 6f)) continue;

//                    Point2f center; float radius;
//                    if (!CircleFromPoints(p1, p2, p3, out center, out radius)) continue;
//                    if (radius < MinRadius || radius > MaxRadius) continue;

//                    int earlyExitLimit = earlyBreak ? inliersNeeded : int.MaxValue;
//                    int inliers = CountInliersGrid(grid, center, radius, threshold, earlyExitLimit);

//                    if (inliers >= inliersNeeded)
//                        results.Add((center, radius, inliers));
//                }
//            });

//            if (!results.Any())
//                return new List<(Point2f, float, int)>();

//            // ---- Sort ổn định: inliers ↓, radius (tuỳ direction), rồi center.X, center.Y ----
//            IOrderedEnumerable<(Point2f center, float radius, int inliers)> sorted;
//            if (direction == CircleScanDirection.InsideOut)
//            {
//                sorted = results
//                    .OrderByDescending(r => r.inliers)
//                    .ThenBy(r => r.radius)
//                    .ThenBy(r => r.center.X)
//                    .ThenBy(r => r.center.Y);
//            }
//            else
//            {
//                sorted = results
//                    .OrderByDescending(r => r.inliers)
//                    .ThenByDescending(r => r.radius)
//                    .ThenBy(r => r.center.X)
//                    .ThenBy(r => r.center.Y);
//            }

//            var best = sorted.First();

//            // --- Refine nhẹ cho ổn định hơn, vẫn deterministic ---
//            var inliersList = CollectInliersGrid(grid, best.center, best.radius, threshold, int.MaxValue);
//            Point2f rc; float rr;
//            if (RefitCircleLeastSquares(inliersList, out rc, out rr))
//            {
//                float thresholdFine = Math.Max(1f, threshold * 0.75f);
//                int inlFine = CountInliersGrid(grid, rc, rr, thresholdFine, int.MaxValue);

//                // Tie-break giống trên để không “nhảy”
//                bool takeRefined = false;
//                if (inlFine > best.inliers) takeRefined = true;
//                else if (inlFine == best.inliers)
//                {
//                    if (direction == CircleScanDirection.InsideOut)
//                        takeRefined = (rr < best.radius) ||
//                                      (Math.Abs(rr - best.radius) <= 1e-4f && (rc.X < best.center.X ||
//                                          (Math.Abs(rc.X - best.center.X) <= 1e-4f && rc.Y < best.center.Y)));
//                    else
//                        takeRefined = (rr > best.radius) ||
//                                      (Math.Abs(rr - best.radius) <= 1e-4f && (rc.X < best.center.X ||
//                                          (Math.Abs(rc.X - best.center.X) <= 1e-4f && rc.Y < best.center.Y)));
//                }

//                if (takeRefined)
//                    best = (rc, rr, inlFine);
//            }

//            // Hàm này yêu cầu trả List và bạn chỉ muốn 1 đường tròn tốt nhất → trả 1 phần tử
//            return new List<(Point2f center, float radius, int minInliers)> { (best.center, best.radius, best.inliers) };
//        }
//        // Sinh (i1,i2,i3) theo seed cố định → deterministic, không phụ thuộc thread
//        // Sinh (i1,i2,i3) theo seed cố định → kết quả không phụ thuộc lịch thread.
//        private static (int i1, int i2, int i3)[] PrecomputeTriplets(int n, int iterations, int seed)
//        {
//            var trips = new (int, int, int)[iterations];
//            uint s = unchecked((uint)seed);
//            for (int i = 0; i < iterations; i++)
//            {
//                int a = NextIndex(ref s, n);
//                int b; do { b = NextIndex(ref s, n); } while (b == a);
//                int c; do { c = NextIndex(ref s, n); } while (c == a || c == b);
//                trips[i] = (a, b, c);
//            }
//            return trips;
//        }

//        private static int NextIndex(ref uint s, int n)
//        {
//            // xorshift32 – nhanh và tái lặp
//            s ^= s << 13;
//            s ^= s >> 17;
//            s ^= s << 5;
//            return (int)(s % (uint)n);
//        }


//        // ----------------- Helpers thêm (giữ trong class) -----------------

//        static bool TripletGood(Point2f a, Point2f b, Point2f c, float minChord)
//        {
//            float dAB = Distance(a, b), dBC = Distance(b, c), dCA = Distance(c, a);
//            if (dAB < minChord || dBC < minChord || dCA < minChord) return false;
//            float area2 = Math.Abs((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)); // 2*area
//            return area2 >= 2.0f; // ngưỡng nhỏ để tránh gần thẳng hàng
//        }

//        static List<Point2f> CollectInliersGrid(
//            Dictionary<(int, int), List<Point2f>> grid,
//            Point2f center, float radius, float threshold, int maxCollect)
//        {
//            var list = new List<Point2f>();
//            int r = (int)(radius + threshold);
//            int minX = (int)((center.X - r) / GridSize);
//            int maxX = (int)((center.X + r) / GridSize);
//            int minY = (int)((center.Y - r) / GridSize);
//            int maxY = (int)((center.Y + r) / GridSize);

//            for (int gx = minX; gx <= maxX; gx++)
//            {
//                for (int gy = minY; gy <= maxY; gy++)
//                {
//                    List<Point2f> cell;
//                    if (!grid.TryGetValue((gx, gy), out cell)) continue;
//                    for (int k = 0; k < cell.Count; k++)
//                    {
//                        var pt = cell[k];
//                        float d = Distance(pt, center);
//                        if (Math.Abs(d - radius) < threshold)
//                        {
//                            list.Add(pt);
//                            if (list.Count >= maxCollect) return list;
//                        }
//                    }
//                }
//            }
//            return list;
//        }

//        static bool Solve3x3(
//            double a11, double a12, double a13,
//            double a21, double a22, double a23,
//            double a31, double a32, double a33,
//            double b1, double b2, double b3,
//            out double x1, out double x2, out double x3)
//        {
//            double det = a11 * (a22 * a33 - a23 * a32)
//                       - a12 * (a21 * a33 - a23 * a31)
//                       + a13 * (a21 * a32 - a22 * a31);
//            if (Math.Abs(det) < 1e-12) { x1 = x2 = x3 = 0; return false; }

//            double d1 = b1 * (a22 * a33 - a23 * a32)
//                      - a12 * (b2 * a33 - a23 * b3)
//                      + a13 * (b2 * a32 - a22 * b3);
//            double d2 = a11 * (b2 * a33 - a23 * b3)
//                      - b1 * (a21 * a33 - a23 * a31)
//                      + a13 * (a21 * b3 - b2 * a31);
//            double d3 = a11 * (a22 * b3 - b2 * a32)
//                      - a12 * (a21 * b3 - b2 * a31)
//                      + b1 * (a21 * a32 - a22 * a31);

//            x1 = d1 / det; x2 = d2 / det; x3 = d3 / det; return true;
//        }

//        static bool RefitCircleLeastSquares(IList<Point2f> pts, out Point2f c, out float r)
//        {
//            c = new Point2f(); r = 0f;
//            int n = pts.Count; if (n < 3) return false;

//            double Sx = 0, Sy = 0, Sxx = 0, Syy = 0, Sxy = 0, Sz = 0, Szx = 0, Szy = 0;
//            for (int i = 0; i < n; i++)
//            {
//                double x = pts[i].X, y = pts[i].Y, z = x * x + y * y;
//                Sx += x; Sy += y; Sxx += x * x; Syy += y * y; Sxy += x * y; Sz += z; Szx += z * x; Szy += z * y;
//            }
//            double a, b, c0;
//            if (!Solve3x3(
//                Sxx, Sxy, Sx,
//                Sxy, Syy, Sy,
//                Sx, Sy, n,
//               -Szx, -Szy, -Sz, out a, out b, out c0)) return false;

//            double cx = -a / 2.0, cy = -b / 2.0;
//            double rad2 = cx * cx + cy * cy - c0;
//            if (rad2 <= 0) return false;

//            c = new Point2f((float)cx, (float)cy);
//            r = (float)Math.Sqrt(rad2);
//            return true;
//        }

//        //public static List<(Point2f center, float radius)> RansacMultiCircle2(
//        //    List<Point2f> edgePoints,
//        //    int maxCircles = 5,
//        //    int? maxIterations = null,
//        //    float threshold = 2.0f,
//        //    int? minInliers = null,

//        //    CircleScanDirection direction = CircleScanDirection.InsideOut,
//        //    bool earlyBreak = false)
//        //{
//        //    var foundCircles = new List<(Point2f, float)>();
//        //    var points = new List<Point2f>(edgePoints);
//        //    Random rnd = new Random();

//        //    var grid = BuildGrid(points);

//        //    int iterations = maxIterations ?? Math.Min(Math.Max(points.Count / 5, 100), 1000); //Math.Clamp(points.Count / 5, 100, 1000);
//        //    int inliersNeeded = minInliers ?? Math.Max(15, (int)(points.Count * 0.02));

//        //    for (int circleCount = 0; circleCount < maxCircles && points.Count > inliersNeeded; circleCount++)
//        //    {
//        //        (Point2f center, float radius, int inliers) best = (new Point2f(), 0f, 0);

//        //        Parallel.ForEach(Partitioner.Create(0, iterations), range =>
//        //        {
//        //            (Point2f center, float radius, int inliers) localBest = (new Point2f(), 0f, 0);
//        //            var localRnd = new Random(Guid.NewGuid().GetHashCode());

//        //            for (int i = range.Item1; i < range.Item2; i++)
//        //            {
//        //                int i1 = localRnd.Next(points.Count);
//        //                int i2, i3;
//        //                do { i2 = localRnd.Next(points.Count); } while (i2 == i1);
//        //                do { i3 = localRnd.Next(points.Count); } while (i3 == i1 || i3 == i2);

//        //                var sample = new[] { points[i1], points[i2], points[i3] };
//        //                if (!CircleFromPoints(sample[0], sample[1], sample[2], out Point2f center, out float radius))
//        //                    continue;
//        //                if (radius < 5 || radius > 500) continue;

//        //                int inliers = CountInliersGrid(grid, center, radius, threshold, earlyBreak ? best.inliers : int.MaxValue);

//        //                bool isBetter = false;
//        //                if (direction == CircleScanDirection.InsideOut)
//        //                {
//        //                    isBetter = (inliers > localBest.inliers) ||
//        //                               (inliers == localBest.inliers && radius < localBest.radius);
//        //                }
//        //                else // OutsideIn
//        //                {
//        //                    isBetter = (inliers > localBest.inliers) ||
//        //                               (inliers == localBest.inliers && radius > localBest.radius);
//        //                }

//        //                if (isBetter)
//        //                    localBest = (center, radius, inliers);
//        //            }

//        //            lock (points)
//        //            {
//        //                bool isBetter = false;
//        //                if (direction == CircleScanDirection.InsideOut)
//        //                {
//        //                    isBetter = (localBest.inliers > best.inliers) ||
//        //                               (localBest.inliers == best.inliers && localBest.radius < best.radius);
//        //                }
//        //                else // OutsideIn
//        //                {
//        //                    isBetter = (localBest.inliers > best.inliers) ||
//        //                               (localBest.inliers == best.inliers && localBest.radius > best.radius);
//        //                }

//        //                if (isBetter)
//        //                    best = localBest;
//        //            }
//        //        });

//        //        if (best.inliers >= inliersNeeded)
//        //        {
//        //            foundCircles.Add((best.center, best.radius));
//        //            points = points.Where(p => Math.Abs(Distance(p, best.center) - best.radius) > threshold).ToList();
//        //            grid = BuildGrid(points);
//        //        }
//        //        else
//        //        {
//        //            break;
//        //        }
//        //    }
//        //    if (direction == CircleScanDirection.OutsideIn)
//        //        foundCircles.Sort((a, b) => b.Item2.CompareTo(a.Item2));  // b.radius → b.Item2
//        //    else
//        //        foundCircles.Sort((a, b) => a.Item2.CompareTo(b.Item2));  // a.radius → a.Item2
//        //    if (foundCircles.Count() == 0)
//        //        return new List<(Point2f center, float radius)>();
//        //    return new List<(Point2f center, float radius)> { foundCircles[0] };

//        //}
//        private static List<(Point2f center, float radius)> RansacMultiCircle(
//            List<Point2f> edgePoints,
//            int maxCircles,
//            int iterations,
//            float threshold,
//            int minInliers,
//            CircleScanDirection direction)
//        {
//            var foundCircles = new List<(Point2f, float)>();
//            var points = new List<Point2f>(edgePoints);
//            Random rnd = new Random();

//            for (int circleCount = 0; circleCount < maxCircles && points.Count > minInliers; circleCount++)
//            {
//                int bestInliers = 0;
//                Point2f bestCenter = new Point2f();
//                float bestRadius = 0;

//                for (int i = 0; i < iterations; i++)
//                {
//                    int i1 = rnd.Next(points.Count);
//                    int i2, i3;
//                    do { i2 = rnd.Next(points.Count); } while (i2 == i1);
//                    do { i3 = rnd.Next(points.Count); } while (i3 == i1 || i3 == i2);
//                    var sample = new[] { points[i1], points[i2], points[i3] };
//                    if (!CircleFromPoints(sample[0], sample[1], sample[2], out Point2f center, out float radius))
//                        continue;

//                    if (radius < 5 || radius > 500) continue;

//                    int inliers = CountInliers(points, center, radius, threshold);
//                    if (inliers > bestInliers)
//                    {
//                        bestInliers = inliers;
//                        bestCenter = center;
//                        bestRadius = radius;
//                    }
//                }

//                if (bestInliers >= minInliers)
//                {
//                    foundCircles.Add((bestCenter, bestRadius));

//                    // Loại bỏ inliers khỏi tập
//                    points = points
//                        .Where(p => Math.Abs(Distance(p, bestCenter) - bestRadius) > threshold)
//                        .ToList();
//                }
//                else
//                {
//                    break;
//                }
//            }

//            if (direction == CircleScanDirection.OutsideIn)
//                foundCircles.Sort((a, b) => b.Item2.CompareTo(a.Item2));  // b.radius → b.Item2
//            else
//                foundCircles.Sort((a, b) => a.Item2.CompareTo(b.Item2));  // a.radius → a.Item2
//            if (foundCircles.Count() == 0)
//                return new List<(Point2f center, float radius)>();
//            return new List<(Point2f center, float radius)> { foundCircles[0] };
//        }

//        private static bool CircleFromPoints(Point2f p1, Point2f p2, Point2f p3, out Point2f center, out float radius)
//        {
//            float x1 = p1.X, y1 = p1.Y;
//            float x2 = p2.X, y2 = p2.Y;
//            float x3 = p3.X, y3 = p3.Y;

//            float a = x1 * (y2 - y3) - y1 * (x2 - x3) + x2 * y3 - x3 * y2;

//            if (Math.Abs(a) < 1e-6)
//            {
//                center = new Point2f();
//                radius = 0;
//                return false;
//            }

//            float A = x1 * x1 + y1 * y1;
//            float B = x2 * x2 + y2 * y2;
//            float C = x3 * x3 + y3 * y3;

//            float D = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
//            float ux = (A * (y2 - y3) + B * (y3 - y1) + C * (y1 - y2)) / D;
//            float uy = (A * (x3 - x2) + B * (x1 - x3) + C * (x2 - x1)) / D;

//            center = new Point2f(ux, uy);
//            radius = (float)Math.Sqrt((ux - x1) * (ux - x1) + (uy - y1) * (uy - y1));
//            return true;
//        }

//        private static int CountInliers(List<Point2f> points, Point2f center, float radius, float threshold)
//        {
//            int count = 0;
//            foreach (var pt in points)
//            {
//                float dist = Distance(pt, center);
//                if (Math.Abs(dist - radius) < threshold)
//                    count++;
//            }
//            return count;
//        }

//        private static float Distance(Point2f a, Point2f b)
//        {
//            float dx = a.X - b.X;
//            float dy = a.Y - b.Y;
//            return (float)Math.Sqrt(dx * dx + dy * dy);
//        }
//    }

//}
///////////////////////////////////////////////////////////////
///////////////////////////////////////
///
using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeeCore.Algorithm
{
    public enum CircleScanDirection
    {
        InsideOut,
        OutsideIn
    }

    public class RansacCircleFitter
    {
        public static List<(Point2f center, float radius, int inliers)> DetectCircles(
            Mat edgeImage,
            int maxCircles = 5,
            int iterations = 300,
            float threshold = 2.0f,
            int minInliers = 50,
            CircleScanDirection direction = CircleScanDirection.InsideOut, int MinRadius = 0, int MaxRadius = 500)
        {
            var edgePoints = GetEdgePoints(edgeImage);
            return RansacSingleBestCircle(edgePoints, maxCircles, iterations, threshold, minInliers, direction, MinRadius, MaxRadius);
        }

        // Lấy điểm biên nhanh và ổn định (FindNonZero + sort Y,X)
        private static List<Point2f> GetEdgePoints(Mat edgeImage)
        {
            Mat edges = edgeImage;
            if (edges == null || edges.Empty())
                return new List<Point2f>();

            if (edges.Type() != MatType.CV_8UC1)
            {
                var gray = new Mat();
                if (edges.Channels() >= 3) Cv2.CvtColor(edges, gray, ColorConversionCodes.BGR2GRAY);
                else gray = edges.Clone();
                Cv2.Threshold(gray, edges = new Mat(), 1, 255, ThresholdTypes.Binary);
                gray.Dispose();
            }

            using (var nz = new Mat())
            {
                Cv2.FindNonZero(edges, nz);
                var pts = new List<Point2f>(nz.Rows);
                for (int i = 0; i < nz.Rows; i++)
                {
                    var p = nz.Get<Point>(i);
                    pts.Add(new Point2f(p.X, p.Y));
                }
                pts.Sort((a, b) => a.Y == b.Y ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));
                return pts;
            }
        }

        private const int GridSize = 10;

        private static Dictionary<(int, int), List<Point2f>> BuildGrid(List<Point2f> points)
        {
            var grid = new Dictionary<(int, int), List<Point2f>>();
            for (int i = 0; i < points.Count; i++)
            {
                var pt = points[i];
                var key = ((int)(pt.X / GridSize), (int)(pt.Y / GridSize));
                List<Point2f> cell;
                if (!grid.TryGetValue(key, out cell))
                {
                    cell = new List<Point2f>();
                    grid[key] = cell;
                }
                cell.Add(pt);
            }
            return grid;
        }

        private static int CountInliersGrid(Dictionary<(int, int), List<Point2f>> grid, Point2f center, float radius, float threshold, int earlyExitLimit)
        {
            int count = 0;
            int r = (int)Math.Ceiling(radius + threshold); // tránh lọt cell biên
            int minX = (int)((center.X - r) / GridSize);
            int maxX = (int)((center.X + r) / GridSize);
            int minY = (int)((center.Y - r) / GridSize);
            int maxY = (int)((center.Y + r) / GridSize);

            for (int gx = minX; gx <= maxX; gx++)
            {
                for (int gy = minY; gy <= maxY; gy++)
                {
                    List<Point2f> cellPoints;
                    if (!grid.TryGetValue((gx, gy), out cellPoints)) continue;

                    for (int k = 0; k < cellPoints.Count; k++)
                    {
                        var pt = cellPoints[k];
                        float dist = Distance(pt, center);
                        if (Math.Abs(dist - radius) < threshold)
                        {
                            count++;
                            if (count >= earlyExitLimit) // dừng sớm ổn định
                                return count;
                        }
                    }
                }
            }
            return count;
        }

        public static List<(Point2f center, float radius, int minInliers)> RansacSingleBestCircle(
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
            if (points.Count < 3) return new List<(Point2f, float, int)>();

            var grid = BuildGrid(points);

            int iterations = maxIterations ?? Math.Min(Math.Max(points.Count / 5, 100), 1000);
            int inliersNeeded = minInliers ?? Math.Max(15, (int)(points.Count * 0.02));

            const int FIXED_SEED = 123456789;
            var triplets = PrecomputeTriplets(points.Count, iterations, FIXED_SEED);

            // MẢNG ỨNG VIÊN CỐ ĐỊNH THEO ITERATION → DETERMINISTIC DÙ SONG SONG
            var candidates = new (Point2f center, float radius, int inliers)?[iterations];

            System.Threading.Tasks.Parallel.ForEach(
                System.Collections.Concurrent.Partitioner.Create(0, iterations),
                range =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        if (points.Count < 3) break;

                        var t = triplets[i];
                        int i1 = t.i1, i2 = t.i2, i3 = t.i3;
                        var p1 = points[i1];
                        var p2 = points[i2];
                        var p3 = points[i3];

                        if (!TripletGood(p1, p2, p3, 6f)) continue;

                        Point2f center; float radius;
                        if (!CircleFromPoints(p1, p2, p3, out center, out radius)) continue;
                        if (radius < MinRadius || radius > MaxRadius) continue;

                        int earlyExitLimit = earlyBreak ? inliersNeeded : int.MaxValue;
                        int inliers = CountInliersGrid(grid, center, radius, threshold, earlyExitLimit);
                        if (inliers >= inliersNeeded)
                        {
                            // Quantize nhẹ để loại nhiễu FP
                            center = new Point2f((float)Math.Round(center.X, 6),
                                                  (float)Math.Round(center.Y, 6));
                            radius = (float)Math.Round(radius, 6);
                            candidates[i] = (center, radius, inliers);
                        }
                    }
                });

            // CHỌN BEST THEO THỨ TỰ i TĂNG DẦN (ỔN ĐỊNH)
            (Point2f center, float radius, int inliers)? bestOpt = null;
            for (int i = 0; i < iterations; i++)
            {
                var cand = candidates[i];
                if (cand == null) continue;

                if (bestOpt == null || IsBetterStable(cand.Value, bestOpt.Value, direction))
                    bestOpt = cand;
            }

            if (bestOpt == null)
                return new List<(Point2f, float, int)>();

            var best = bestOpt.Value;

            // --- Refine (deterministic) + tie-break ổn định ---
            var inliersList = CollectInliersGrid(grid, best.center, best.radius, threshold, int.MaxValue);
            Point2f rc; float rr;
            if (RefitCircleLeastSquares(inliersList, out rc, out rr))
            {
                float thresholdFine = Math.Max(1f, threshold * 0.75f);
                int inlFine = CountInliersGrid(grid, rc, rr, thresholdFine, int.MaxValue);

                rc = new Point2f((float)Math.Round(rc.X, 6), (float)Math.Round(rc.Y, 6));
                rr = (float)Math.Round(rr, 6);

                var refined = (rc, rr, inlFine);
                if (IsBetterStable(refined, best, direction))
                    best = refined;
            }

            return new List<(Point2f center, float radius, int minInliers)> { (best.center, best.radius, best.inliers) };
        }

        // Tie-break ổn định hoàn toàn
        private static bool IsBetterStable(
            (Point2f center, float radius, int inliers) A,
            (Point2f center, float radius, int inliers) B,
            CircleScanDirection dir)
        {
            if (A.inliers != B.inliers) return A.inliers > B.inliers;

            if (dir == CircleScanDirection.InsideOut)
            {
                if (A.radius != B.radius) return A.radius < B.radius;
            }
            else
            {
                if (A.radius != B.radius) return A.radius > B.radius;
            }

            if (A.center.X != B.center.X) return A.center.X < B.center.X;
            if (A.center.Y != B.center.Y) return A.center.Y < B.center.Y;

            // fallback (hầu như không tới do đã quantize)
            return false;
        }


        // Triplet deterministic theo seed cố định (C# 7.3 OK)
        private static (int i1, int i2, int i3)[] PrecomputeTriplets(int n, int iterations, int seed)
        {
            var trips = new (int, int, int)[iterations];
            uint s = unchecked((uint)seed);
            for (int i = 0; i < iterations; i++)
            {
                int a = NextIndex(ref s, n);
                int b; do { b = NextIndex(ref s, n); } while (b == a);
                int c; do { c = NextIndex(ref s, n); } while (c == a || c == b);
                trips[i] = (a, b, c);
            }
            return trips;
        }

        private static int NextIndex(ref uint s, int n)
        {
            s ^= s << 13;
            s ^= s >> 17;
            s ^= s << 5;
            return (int)(s % (uint)n);
        }

        // ---------- Helpers ----------
        static bool TripletGood(Point2f a, Point2f b, Point2f c, float minChord)
        {
            float dAB = Distance(a, b), dBC = Distance(b, c), dCA = Distance(c, a);
            if (dAB < minChord || dBC < minChord || dCA < minChord) return false;
            float area2 = Math.Abs((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X));
            return area2 >= 2.0f; // tránh gần thẳng hàng
        }

        static List<Point2f> CollectInliersGrid(
            Dictionary<(int, int), List<Point2f>> grid,
            Point2f center, float radius, float threshold, int maxCollect)
        {
            var list = new List<Point2f>();
            int r = (int)Math.Ceiling(radius + threshold);
            int minX = (int)((center.X - r) / GridSize);
            int maxX = (int)((center.X + r) / GridSize);
            int minY = (int)((center.Y - r) / GridSize);
            int maxY = (int)((center.Y + r) / GridSize);

            for (int gx = minX; gx <= maxX; gx++)
            {
                for (int gy = minY; gy <= maxY; gy++)
                {
                    List<Point2f> cell;
                    if (!grid.TryGetValue((gx, gy), out cell)) continue;
                    for (int k = 0; k < cell.Count; k++)
                    {
                        var pt = cell[k];
                        float d = Distance(pt, center);
                        if (Math.Abs(d - radius) < threshold)
                        {
                            list.Add(pt);
                            if (list.Count >= maxCollect) return list;
                        }
                    }
                }
            }
            return list;
        }

        static bool Solve3x3(
            double a11, double a12, double a13,
            double a21, double a22, double a23,
            double a31, double a32, double a33,
            double b1, double b2, double b3,
            out double x1, out double x2, out double x3)
        {
            double det = a11 * (a22 * a33 - a23 * a32)
                       - a12 * (a21 * a33 - a23 * a31)
                       + a13 * (a21 * a32 - a22 * a31);
            if (Math.Abs(det) < 1e-12) { x1 = x2 = x3 = 0; return false; }

            double d1 = b1 * (a22 * a33 - a23 * a32)
                      - a12 * (b2 * a33 - a23 * b3)
                      + a13 * (b2 * a32 - a22 * b3);
            double d2 = a11 * (b2 * a33 - a23 * b3)
                      - b1 * (a21 * a33 - a23 * a31)
                      + a13 * (a21 * b3 - b2 * a31);
            double d3 = a11 * (a22 * b3 - b2 * a32)
                      - a12 * (a21 * b3 - b2 * a31)
                      + b1 * (a21 * a32 - a22 * a31);

            x1 = d1 / det; x2 = d2 / det; x3 = d3 / det; return true;
        }

        static bool RefitCircleLeastSquares(IList<Point2f> pts, out Point2f c, out float r)
        {
            c = new Point2f(); r = 0f;
            int n = pts.Count; if (n < 3) return false;

            double Sx = 0, Sy = 0, Sxx = 0, Syy = 0, Sxy = 0, Sz = 0, Szx = 0, Szy = 0;
            for (int i = 0; i < n; i++)
            {
                double x = pts[i].X, y = pts[i].Y, z = x * x + y * y;
                Sx += x; Sy += y; Sxx += x * x; Syy += y * y; Sxy += x * y; Sz += z; Szx += z * x; Szy += z * y;
            }
            double a, b, c0;
            if (!Solve3x3(
                Sxx, Sxy, Sx,
                Sxy, Syy, Sy,
                Sx, Sy, n,
               -Szx, -Szy, -Sz, out a, out b, out c0)) return false;

            double cx = -a / 2.0, cy = -b / 2.0;
            double rad2 = cx * cx + cy * cy - c0;
            if (rad2 <= 0) return false;

            c = new Point2f((float)cx, (float)cy);
            r = (float)Math.Sqrt(rad2);
            return true;
        }

        private static bool CircleFromPoints(Point2f p1, Point2f p2, Point2f p3, out Point2f center, out float radius)
        {
            float x1 = p1.X, y1 = p1.Y;
            float x2 = p2.X, y2 = p2.Y;
            float x3 = p3.X, y3 = p3.Y;

            float D = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            if (Math.Abs(D) < 1e-6f) { center = default(Point2f); radius = 0; return false; }

            float A = x1 * x1 + y1 * y1;
            float B = x2 * x2 + y2 * y2;
            float C = x3 * x3 + y3 * y3;

            float ux = (A * (y2 - y3) + B * (y3 - y1) + C * (y1 - y2)) / D;
            float uy = (A * (x3 - x2) + B * (x1 - x3) + C * (x2 - x1)) / D;

            center = new Point2f(ux, uy);
            radius = (float)Math.Sqrt((ux - x1) * (ux - x1) + (uy - y1) * (uy - y1));
            return true;
        }

        private static int CountInliers(List<Point2f> points, Point2f center, float radius, float threshold)
        {
            int count = 0;
            for (int i = 0; i < points.Count; i++)
            {
                var pt = points[i];
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

        // (Không dùng trong bản “1 vòng tròn tốt nhất” nhưng giữ lại cho đầy đủ)
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
            var rnd = new Random(12345);

            for (int circleCount = 0; circleCount < maxCircles && points.Count > minInliers; circleCount++)
            {
                int bestInliers = 0;
                Point2f bestCenter = new Point2f();
                float bestRadius = 0;

                for (int i = 0; i < iterations; i++)
                {
                    int i1 = rnd.Next(points.Count);
                    int i2; do { i2 = rnd.Next(points.Count); } while (i2 == i1);
                    int i3; do { i3 = rnd.Next(points.Count); } while (i3 == i1 || i3 == i2);

                    var sample = new[] { points[i1], points[i2], points[i3] };
                    Point2f c; float r;
                    if (!CircleFromPoints(sample[0], sample[1], sample[2], out c, out r))
                        continue;
                    if (r < 5 || r > 500) continue;

                    int inliers = CountInliers(points, c, r, threshold);
                    if (inliers > bestInliers)
                    {
                        bestInliers = inliers;
                        bestCenter = c;
                        bestRadius = r;
                    }
                }

                if (bestInliers >= minInliers)
                {
                    foundCircles.Add((bestCenter, bestRadius));
                    points = points.Where(p => Math.Abs(Distance(p, bestCenter) - bestRadius) > threshold).ToList();
                }
                else break;
            }

            if (direction == CircleScanDirection.OutsideIn)
                foundCircles.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            else
                foundCircles.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            if (foundCircles.Count == 0)
                return new List<(Point2f center, float radius)>();
            return new List<(Point2f center, float radius)> { foundCircles[0] };
        }
    }
}
