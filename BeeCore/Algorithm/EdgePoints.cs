using System;
using System.Collections.Generic;
using OpenCvSharp;

public static class EdgePointExtensions
{
    /// <summary>
    /// Trích điểm biên tối ưu cho RANSAC line-fitting từ ảnh xám (this Mat gray).
    /// </summary>
    public static List<Point2f> ExtractEdgePoints(
        this Mat gray,
        Mat edges,
        int maxPoints = 8000,
        float minGradMag = 15f,
        int sectors = 36,
        double subPixFraction = 0.3,
        int seed = 1234)
    {
        if (gray == null || gray.Empty()) return new List<Point2f>(0);
        if (edges == null || edges.Empty()) return new List<Point2f>(0);
        if (gray.Type() != MatType.CV_8UC1) throw new ArgumentException("gray must be CV_8UC1.");
        if (edges.Type() != MatType.CV_8UC1) throw new ArgumentException("edges must be CV_8UC1.");

        using (var gx = new Mat())
        using (var gy = new Mat())
        using (var nz = new Mat())
        {
            // 1) Gradient
            Cv2.Scharr(gray, gx, MatType.CV_32F, 1, 0);
            Cv2.Scharr(gray, gy, MatType.CV_32F, 0, 1);

            // 2) FindNonZero -> nz (Nx1, CV_32SC2)
            Cv2.FindNonZero(edges, nz);
            if (nz.Empty()) return new List<Point2f>(0);

            if (nz.Type() != MatType.CV_32SC2)
                throw new InvalidOperationException("FindNonZero result must be CV_32SC2.");

            int total = nz.Rows;                 // nz là Nx1
            var idx = nz.GetGenericIndexer<Vec2i>(); // đọc nhanh hơn At<>

            // 3) Buckets theo hướng
            int bucketCap = Math.Max(1, maxPoints / Math.Max(1, sectors));
            var buckets = new List<Point2f>[sectors];
            for (int i = 0; i < sectors; i++) buckets[i] = new List<Point2f>(bucketCap);

            float eps = 1e-6f;
            if (seed < 0) seed = Environment.TickCount;

            for (int i = 0; i < total; i++)
            {
                // đọc (x,y) từ nz
                var v = idx[i, 0]; // Vec2i
                int x = v.Item0, y = v.Item1;
                if ((uint)x >= (uint)gray.Cols || (uint)y >= (uint)gray.Rows) continue;

                float gxx = gx.At<float>(y, x);
                float gyy = gy.At<float>(y, x);
                float mag = (float)Math.Sqrt(gxx * gxx + gyy * gyy);
                if (mag < minGradMag) continue;

                // Tangent ~ hướng line: (-Gy, Gx) / |grad|
                float inv = 1.0f / Math.Max(eps, mag);
                float tx = -gyy * inv, ty = gxx * inv;

                // Sector theo góc tangent
                double ang = Math.Atan2(ty, tx);
                if (ang < 0) ang += 2 * Math.PI;
                int sec = (int)(ang * sectors / (2 * Math.PI));
                if (sec >= sectors) sec = sectors - 1;

                var B = buckets[sec];
                var p = new Point2f(x, y);

                if (B.Count < bucketCap)
                {
                    B.Add(p);
                }
                else
                {
                    int j = FastRandom(ref seed) % (B.Count + 1);
                    if (j < B.Count) B[j] = p;
                }
            }

            // 4) Gom bucket
            var outPts = new List<Point2f>(maxPoints);
            for (int s = 0; s < sectors; s++)
            {
                var B = buckets[s];
                int take = Math.Min(B.Count, bucketCap);
                for (int i = 0; i < take; i++) outPts.Add(B[i]);
            }
            if (outPts.Count == 0) return outPts;

            // 5) Sub-pix một phần
            if (subPixFraction > 0)
            {
                int nSub = (int)Math.Round(outPts.Count * Clamp01(subPixFraction));
                if (nSub > 0)
                {
                    var idxs = UniformIndices(outPts.Count, nSub, seed ^ 0x517cc1b7);
                    var subBuf = new Point2f[nSub];
                    for (int i = 0; i < nSub; i++) subBuf[i] = outPts[idxs[i]];

                    Cv2.CornerSubPix(
                        gray,
                        subBuf,
                        new Size(3, 3),
                        new Size(-1, -1),
                        new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 15, 0.03)
                    );

                    for (int i = 0; i < nSub; i++)
                        outPts[idxs[i]] = subBuf[i];
                }
            }

            return outPts;
        }
    }

    // ===== Helpers =====
    private static double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);

    private static int FastRandom(ref int state)
    {
        // xorshift32
        uint x = (uint)(state == 0 ? 0x12345678 : state);
        x ^= x << 13; x ^= x >> 17; x ^= x << 5;
        state = (int)x;
        return (int)(x & 0x7fffffff);
    }

    private static int[] UniformIndices(int n, int m, int seed)
    {
        if (m >= n) { var all = new int[n]; for (int i = 0; i < n; i++) all[i] = i; return all; }
        var rnd = new Random(seed);
        var set = new HashSet<int>();
        for (int j = n - m; j < n; j++)
        {
            int t = rnd.Next(0, j + 1);
            if (!set.Add(t)) set.Add(j);
        }
        var arr = new int[m]; int k = 0; foreach (var v in set) arr[k++] = v;
        return arr;
    }
}
