
using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

public enum BorderPolicy { None, RemoveAllTouchers, RemoveExceptLargest }
public enum BinarizeMethod { Auto, Otsu, Adaptive, Sauvola }

public struct DebugOptions
{
    public bool Enable;      // bật/tắt lưu debug
    public string Dir;       // thư mục lưu (để trống = tạo theo timestamp)
    public bool SaveProbes;  // lưu ảnh ở bước probe
    public string Prefix;    // tiền tố file
}

public static class KeepLargestCrop
{
    static void RemoveBorderTouchingComponents(ref Mat bin, int bandPx, double maxAreaPct,
                                         DebugSaver D = null, string tag = null)
    {
        if (bin == null || bin.Empty()) return;
        if (bin.Type() != MatType.CV_8U) throw new ArgumentException("bin must be CV_8U binary");

        int H = bin.Rows, W = bin.Cols;
        bandPx = Math.Max(1, Math.Min(bandPx, Math.Min(W, H) / 4)); // kẹp cho an toàn

        // clamp phần trăm [0..1]
        if (double.IsNaN(maxAreaPct) || double.IsInfinity(maxAreaPct)) maxAreaPct = 0.10;
        maxAreaPct = Math.Max(0.0, Math.Min(1.0, maxAreaPct));

        long imgAreaPx = (long)W * H;
        long areaThreshPx = (long)Math.Round(imgAreaPx * maxAreaPct);

        using (var borderMask = new Mat(H, W, MatType.CV_8U, Scalar.All(0)))
        {
            // tạo “vòng biên” dày bandPx
            borderMask.SetTo(Scalar.All(255));
            var inner = new Rect(bandPx, bandPx, Math.Max(0, W - 2 * bandPx), Math.Max(0, H - 2 * bandPx));
            if (inner.Width > 0 && inner.Height > 0)
                new Mat(borderMask, inner).SetTo(Scalar.All(0));

            using (var labels = new Mat())
            using (var stats = new Mat())
            using (var cents = new Mat())
            {
                int n = Cv2.ConnectedComponentsWithStats(bin, labels, stats, cents,
                                                         PixelConnectivity.Connectivity8, MatType.CV_32S);
                if (n <= 1)
                {
                    if (D != null && tag != null) D.Save(tag + "_no_cc", bin);
                    return;
                }

                using (var tmp = new Mat())
                using (var touch = new Mat())
                {
                    for (int i = 1; i < n; i++)
                    {
                        // mask của nhãn i
                        Cv2.InRange(labels, new Scalar(i), new Scalar(i), tmp);

                        // kiểm tra giao với dải biên
                        Cv2.BitwiseAnd(tmp, borderMask, touch);
                        bool touching = Cv2.CountNonZero(touch) > 0;

                        // diện tích nhãn i
                        int areaCC = stats.At<int>(i, (int)ConnectedComponentsTypes.Area);

                        // chỉ xoá nếu vừa chạm biên vừa nhỏ hơn ngưỡng %
                        if (touching && areaCC <= areaThreshPx)
                            bin.SetTo(Scalar.All(0), tmp);
                    }
                }
            }

            if (D != null && tag != null) D.Save(tag + "_bin_removed_touch_pct", bin);
        }
    }
    /// <summary>
    /// Tách foreground + giữ đối tượng chính, chống dính biên mạnh.
    /// </summary>
    public static RectRotate RunCrop(Mat bgr,
                              BorderPolicy borderPolicy = BorderPolicy.RemoveAllTouchers,
                              BinarizeMethod method = BinarizeMethod.Auto,
                              int cropPad = 6,
                              Scalar? cropBg = null,
                              DebugOptions dbg = default)
    {
        if (bgr == null || bgr.Empty()) throw new ArgumentException("Input empty");
        Scalar bgFill = cropBg ?? Scalar.Black;

        // 0) Pad ảnh màu một lớp an toàn để mọi morph/bridge không đụng mép thật
        //    (giảm nguy cơ dính biên đối với chữ sát mép khung)
        int G = Math.Max(20, (int)Math.Round(0.05 * Math.Min(bgr.Rows, bgr.Cols)));
        Mat bgrPad = new Mat();
        Cv2.CopyMakeBorder(bgr, bgrPad, G, G, G, G, BorderTypes.Constant, Scalar.All(0));

        using (var D = new DebugSaver(dbg))
        {
            D.Save("00_input_bgr", bgrPad);

            // --- Gray & roughness ---
            var gray = new Mat(); Cv2.CvtColor(bgrPad, gray, ColorConversionCodes.BGR2GRAY);
            D.Save("01_gray", gray);

            var gx32 = new Mat(); var gy32 = new Mat(); var mag32 = new Mat();
            Cv2.Sobel(gray, gx32, MatType.CV_32F, 1, 0, 3);
            Cv2.Sobel(gray, gy32, MatType.CV_32F, 0, 1, 3);
            Cv2.Magnitude(gx32, gy32, mag32);
            Scalar m, s; Cv2.MeanStdDev(mag32, out m, out s);
            double rough = s.Val0;
            D.Save("02_mag32", mag32, normalize: true);

            int W = bgrPad.Cols, H = bgrPad.Rows, S = Math.Min(W, H);

            // --- Top-hat trên kênh Lab.a* ---
            var lab = new Mat(); var a = new Mat(); var bg = new Mat(); var tophat = new Mat();
            Cv2.CvtColor(bgrPad, lab, ColorConversionCodes.BGR2Lab);
            Cv2.ExtractChannel(lab, a, 1); D.Save("03_lab_a", a);

            int bgBlur = OddClamp((int)Math.Round(S / 12.0 + rough / 8.0), 15, 101);
            Cv2.GaussianBlur(a, bg, new Size(bgBlur, bgBlur), 0); D.Save("04_bg_blur", bg);
            Cv2.Subtract(a, bg, tophat); D.Save("05_tophat", tophat, normalize: true);

            var n8 = new Mat(); Cv2.Normalize(tophat, n8, 0, 255, NormTypes.MinMax, MatType.CV_8U);
            D.Save("06_tophat_norm8", n8);

            // --- Chọn method + polarity ---
            bool invert;
            if (method == BinarizeMethod.Auto)
                (method, invert) = PickBestMethod(n8, gray, rough, D);
            else
                invert = Cv2.Mean(n8).Val0 < 127;

            var bin = MakeBinary(n8, gray, method, invert, S, D, "07_bin_full");
            try
            {
                int closeK = OddClamp(Math.Max(3, (int)(S * 0.006)), 3, 7);
                int openK = OddClamp(Math.Max(3, closeK - 2), 3, 7);
                int closeIt = 1, openIt = 1;

                // 1) Morph/Bridge trong guard band nội bộ
               // MorphInGuardBand(ref bin, new Size(closeK, closeK), new Size(openK, openK), closeIt, openIt, g: Math.Max(8, (int)Math.Round(0.04 * S)));
                // --- loại bỏ CC chạm biên để tránh nối dính khung ---
                int band = Math.Max(2, (int)Math.Round(Math.Min(bin.Rows, bin.Cols) * 0.012)); // ~1.2% cạnh ngắn
                RemoveBorderTouchingComponents(ref bin, band, 0.2, D, "07_rm_touch");
                BridgeGapsSmart(ref bin, S, D, "07a_bridge");

               
                // 3) Cắt cầu nối yếu theo gradient
                PruneByGradientMask(bin, mag32, 0.72, 1, D, "07b_grad");

                // 4) Hậu xử lý & CC chọn cụm lớn nhất
                var mask = PostMorphToLargest(bin, closeK, openK, closeIt, openIt,
                                              borderPolicy, out _, D, "08_post");

                // 5) Nếu policy không loại hết touchers mà vẫn còn dính,
                //    fallback: loại toàn bộ CC chạm biên rồi giữ largest sạch.
                if (borderPolicy != BorderPolicy.RemoveAllTouchers)
                {
                    bool anyTouch = EdgeTouchExists(mask);
                    if (anyTouch)
                    {
                        KeepLargestButDropBorderTouchers(ref mask);
                        D.Save("09_drop_border_touchers", mask);
                    }
                }

                var rrPad = LargestRotatedRect(mask);

                // Map về toạ độ ảnh gốc (bỏ padding biên)
                rrPad.Center = new Point2f(rrPad.Center.X - G, rrPad.Center.Y - G);
                var rr = rrPad;

                // cleanup
                gray.Dispose(); gx32.Dispose(); gy32.Dispose(); mag32.Dispose();
                lab.Dispose(); a.Dispose(); bg.Dispose(); tophat.Dispose(); n8.Dispose(); mask.Dispose();
                bgrPad.Dispose();

                return ToRectRotate(rr);
            }
            finally
            {
                bin?.Dispose();
            }
        }
    }

    // ===
    //  ANTI-BORDER TOOLKIT
    // ===

    // Làm close/open trong guard band nội bộ để tránh đụng mép thật
    static void MorphInGuardBand(ref Mat bin, Size kClose, Size kOpen, int itC, int itO, int g)
    {
        if (bin == null || bin.Empty()) return;
        var pad = new Mat(bin.Rows + 2 * g, bin.Cols + 2 * g, bin.Type(), Scalar.All(0));
        var roi = new Rect(g, g, bin.Cols, bin.Rows);
        using (var mid = new Mat(pad, roi)) bin.CopyTo(mid);

        using (var kC = Cv2.GetStructuringElement(MorphShapes.Rect, kClose))
        using (var kO = Cv2.GetStructuringElement(MorphShapes.Rect, kOpen))
        {
            if (itC > 0) Cv2.MorphologyEx(pad, pad, MorphTypes.Close, kC, iterations: itC);
            if (itO > 0) Cv2.MorphologyEx(pad, pad, MorphTypes.Open, kO, iterations: itO);
        }

        using (var mid2 = new Mat(pad, roi)) mid2.CopyTo(bin);
        pad.Dispose();
    }


    static bool EdgeTouchExists(Mat img)
    {
        if (img == null || img.Empty()) return false;
        int rows = img.Rows, cols = img.Cols;
        return Cv2.CountNonZero(img.Row(0)) > 0 ||
               Cv2.CountNonZero(img.Row(rows - 1)) > 0 ||
               Cv2.CountNonZero(img.Col(0)) > 0 ||
               Cv2.CountNonZero(img.Col(cols - 1)) > 0;
    }

    // Loại toàn bộ CC chạm biên, giữ lại CC lớn nhất còn sạch
    static void KeepLargestButDropBorderTouchers(ref Mat bin)
    {
        using (var labels = new Mat())
        using (var stats = new Mat())
        using (var cents = new Mat())
        {
            int n = Cv2.ConnectedComponentsWithStats(bin, labels, stats, cents,
                                                     PixelConnectivity.Connectivity8, MatType.CV_32S);
            if (n <= 1) { bin.SetTo(Scalar.All(0)); return; }

            int W = bin.Cols, H = bin.Rows;
            int bestIdx = -1, bestArea = -1;

            for (int i = 1; i < n; i++)
            {
                int x = stats.Get<int>(i, (int)ConnectedComponentsTypes.Left);
                int y = stats.Get<int>(i, (int)ConnectedComponentsTypes.Top);
                int w = stats.Get<int>(i, (int)ConnectedComponentsTypes.Width);
                int h = stats.Get<int>(i, (int)ConnectedComponentsTypes.Height);
                bool touches = (x == 0) || (y == 0) || (x + w >= W) || (y + h >= H);
                if (touches) continue;

                int area = stats.Get<int>(i, (int)ConnectedComponentsTypes.Area);
                if (area > bestArea) { bestArea = area; bestIdx = i; }
            }

            var outMask = new Mat(bin.Size(), MatType.CV_8U, Scalar.All(0));
            if (bestIdx > 0)
            {
                using (var tmp = new Mat())
                {
                    Cv2.InRange(labels, new Scalar(bestIdx), new Scalar(bestIdx), tmp);
                    outMask.SetTo(Scalar.All(255), tmp);
                }
            }
            outMask.CopyTo(bin);
            outMask.Dispose();
        }
    }

    // Bắc cầu định hướng (PCA) trong guard band để không dính mép
    static void BridgeGapsSmart(ref Mat bin, int S, DebugSaver D = null, string tag = null)
    {
        if (bin == null || bin.Empty()) return;

        var nz = new Mat();
        Cv2.FindNonZero(bin, nz);
        if (nz.Empty() || nz.Rows < 25) { nz.Dispose(); return; }

        var data = new Mat(nz.Rows, 2, MatType.CV_32F);
        for (int i = 0; i < nz.Rows; i++)
        {
            var p = nz.At<Point>(i);
            data.Set(i, 0, (float)p.X);
            data.Set(i, 1, (float)p.Y);
        }

        var mean = new Mat();
        var eigVec = new Mat();
        var eigVal = new Mat();
        Cv2.PCACompute(data, mean, eigVec, eigVal);

        float vx = eigVec.At<float>(0, 0), vy = eigVec.At<float>(0, 1);
        double angle = Math.Atan2(vy, vx) * 180.0 / Math.PI;  // CCW

        int L = OddClamp(Math.Max(5, (int)Math.Round(S * 0.018)), 5, 45);
        int t = (S >= 800) ? 2 : 1;
        double[] angs = new[] { angle, angle + 15, angle - 15, angle + 90 };

        int g = Math.Min(25, Math.Max(8, (int)Math.Round(S * 0.04)));  // guard band mạnh
        var padded = new Mat(bin.Rows + 2 * g, bin.Cols + 2 * g, bin.Type(), Scalar.All(0));
        var roi = new Rect(g, g, bin.Cols, bin.Rows);
        using (var mid = new Mat(padded, roi)) bin.CopyTo(mid);

        for (int i = 0; i < angs.Length; i++)
        {
            var k = MakeLineKernelAngle(L, angs[i], t);
            Cv2.MorphologyEx(padded, padded, MorphTypes.Close, k, iterations: 1);
            k.Dispose();
        }

        using (var mid2 = new Mat(padded, roi)) mid2.CopyTo(bin);

        if (D != null && tag != null) D.Save(tag + "_guard", bin);

        nz.Dispose(); data.Dispose(); mean.Dispose(); eigVec.Dispose(); eigVal.Dispose(); padded.Dispose();
    }

    static Mat MakeLineKernelAngle(int L, double angleDeg, int thickness = 1)
    {
        var k = new Mat(L, L, MatType.CV_8U, Scalar.All(0));
        var c = new Point(L / 2, L / 2);
        double rad = angleDeg * Math.PI / 180.0;
        int dx = (int)Math.Round(Math.Cos(rad) * (L / 2.0));
        int dy = (int)Math.Round(Math.Sin(rad) * (L / 2.0));
        Cv2.Line(k, new Point(c.X - dx, c.Y - dy), new Point(c.X + dx, c.Y + dy),
                 Scalar.All(255), thickness, LineTypes.Link8);
        return k;
    }

    static void PruneByGradientMask(Mat bin, Mat mag32, double pct = 0.72, int dil = 1, DebugSaver D = null, string tag = null)
    {
        var mag8 = new Mat();
        Cv2.Normalize(mag32, mag8, 0, 255, NormTypes.MinMax, MatType.CV_8U);
        byte th = PercentileOnMask(mag8, bin, pct);

        var edgeMask = new Mat();
        Cv2.Threshold(mag8, edgeMask, th, 255, ThresholdTypes.Binary);

        if (dil > 0)
        {
            var k = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(2 * dil + 1, 2 * dil + 1));
            Cv2.Dilate(edgeMask, edgeMask, k, iterations: 1);
            k.Dispose();
        }

        Cv2.BitwiseAnd(bin, edgeMask, bin);

        if (D != null && tag != null)
        {
            D.Save(tag + "_mag8", mag8);
            D.Save(tag + "_edge", edgeMask);
            D.Save(tag + "_bin_after", bin);
        }
        mag8.Dispose(); edgeMask.Dispose();
    }

    static byte PercentileOnMask(Mat img8u, Mat mask, double p)
    {
        p = Math.Max(0, Math.Min(1, p));
        var hist = new Mat();
        int[] histSize = { 256 };
        Rangef[] ranges = { new Rangef(0, 256) };
        Cv2.CalcHist(new[] { img8u }, new[] { 0 }, (mask != null && !mask.Empty()) ? mask : new Mat(),
                     hist, 1, histSize, ranges, accumulate: false);
        float acc = 0f, target = (float)(p * Cv2.CountNonZero((mask != null && !mask.Empty()) ? mask : new Mat(img8u.Size(), MatType.CV_8U, Scalar.All(255))));
        for (int i = 0; i < 256; i++)
        {
            acc += hist.At<float>(i);
            if (acc >= target) { hist.Dispose(); return (byte)i; }
        }
        hist.Dispose();
        return 200;
    }

    // ===
    //  HẬU XỬ LÝ & CHỌN LỚN NHẤT
    // ===

    static Mat PostMorphToLargest(Mat bin, int closeK, int openK, int closeIt, int openIt,
                              BorderPolicy borderPolicy, out int largestArea,
                              DebugSaver D, string tag,
                              double maxGapFrac = 0.04)
    {
        return PostMorphToLargest_Core(bin, closeK, openK, closeIt, openIt,
                                       borderPolicy, out largestArea,
                                       maxGapFrac, D, tag);
    }

    static Mat PostMorphToLargest(Mat bin, int closeK, int openK, int closeIt, int openIt,
                                  BorderPolicy borderPolicy, out int largestArea,
                                  double maxGapFrac = 0.04)
    {
        return PostMorphToLargest_Core(bin, closeK, openK, closeIt, openIt,
                                       borderPolicy, out largestArea,
                                       maxGapFrac, null, null);
    }

    static Mat PostMorphToLargest_Core(Mat bin, int closeK, int openK, int closeIt, int openIt,
                                       BorderPolicy borderPolicy, out int largestArea,
                                       double maxGapFrac,
                                       DebugSaver D, string tag)
    {
        var work = bin.Clone();
        var kC = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(closeK, closeK));
        var kO = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(openK, openK));

        int gg = Math.Min(10, Math.Max(3, (int)Math.Round(Math.Min(work.Rows, work.Cols) * 0.02)));
        using (var pad = new Mat(work.Rows + 2 * gg, work.Cols + 2 * gg, work.Type(), Scalar.All(0)))
        {
            var roi = new Rect(gg, gg, work.Cols, work.Rows);
            using (var mid = new Mat(pad, roi)) work.CopyTo(mid);

            Cv2.MorphologyEx(pad, pad, MorphTypes.Close, kC, iterations: closeIt);
            Cv2.MorphologyEx(pad, pad, MorphTypes.Open, kO, iterations: openIt);

            using (var mid2 = new Mat(pad, roi)) mid2.CopyTo(work);
        }
        if (D != null && tag != null) D.Save(tag + "_morph", work);
        kC.Dispose(); kO.Dispose();

        var labels = new Mat(); var stats = new Mat(); var cents = new Mat();
        int n = Cv2.ConnectedComponentsWithStats(work, labels, stats, cents,
                                                 PixelConnectivity.Connectivity8, MatType.CV_32S);

        if (n <= 1)
        {
            largestArea = 0;
            labels.Dispose(); stats.Dispose(); cents.Dispose(); work.Dispose();
            return new Mat(work.Size(), MatType.CV_8U, Scalar.All(0));
        }

        int W = work.Cols, H = work.Rows;
        int gapPx = Math.Max(2, (int)Math.Round(Math.Min(W, H) * maxGapFrac));

        var rects = new Rect[n];
        var areas = new int[n];
        for (int i = 1; i < n; i++)
        {
            int x = stats.Get<int>(i, (int)ConnectedComponentsTypes.Left);
            int y = stats.Get<int>(i, (int)ConnectedComponentsTypes.Top);
            int w = stats.Get<int>(i, (int)ConnectedComponentsTypes.Width);
            int h = stats.Get<int>(i, (int)ConnectedComponentsTypes.Height);
            rects[i] = new Rect(x, y, w, h);
            areas[i] = stats.Get<int>(i, (int)ConnectedComponentsTypes.Area);
        }

        int[] parent = new int[n];
        for (int i = 0; i < n; i++) parent[i] = i;
        Func<int, int> Find = null;
        Find = a => parent[a] == a ? a : (parent[a] = Find(parent[a]));
        Action<int, int> Union = (a, b) => { a = Find(a); b = Find(b); if (a != b) parent[a] = b; };

        Func<int, bool> TouchBorder = (i) =>
        {
            var r = rects[i];
            return r.Left == 0 || r.Top == 0 || r.Right >= W || r.Bottom >= H;
        };
        Func<Rect, Rect, int, bool> OverlapWithGap = (a, b, gap) =>
        {
            var A = Rect.Inflate(a, gap, gap);
            var B = Rect.Inflate(b, gap, gap);
            return A.IntersectsWith(B);
        };

        for (int i = 1; i < n; i++)
            for (int j = i + 1; j < n; j++)
                if (OverlapWithGap(rects[i], rects[j], gapPx)) Union(i, j);

        var clusterArea = new Dictionary<int, int>();
        var clusterTouch = new Dictionary<int, bool>();
        for (int i = 1; i < n; i++)
        {
            int r = Find(i);
            if (!clusterArea.ContainsKey(r)) { clusterArea[r] = 0; clusterTouch[r] = false; }
            clusterArea[r] += areas[i];
            clusterTouch[r] = clusterTouch[r] || TouchBorder(i);
        }

        int bestRoot = -1; int bestAreaSum = -1;
        foreach (var kv in clusterArea)
            if (kv.Value > bestAreaSum) { bestAreaSum = kv.Value; bestRoot = kv.Key; }

        var outMask = new Mat(work.Size(), MatType.CV_8U, Scalar.All(0));
        using (var tmp = new Mat())
        {
            for (int i = 1; i < n; i++)
            {
                if (Find(i) != bestRoot) continue;
                Cv2.InRange(labels, new Scalar(i), new Scalar(i), tmp);
                outMask.SetTo(Scalar.All(255), tmp);
            }
        }

        if (borderPolicy == BorderPolicy.RemoveAllTouchers && clusterTouch[bestRoot])
            outMask.SetTo(Scalar.All(0));

        if (D != null && tag != null)
        {
            using (var labelsVis = VisualizeLabels(labels, n))
                D.Save(tag + "_labels", labelsVis);
            D.Save(tag + "_mask_cluster", outMask);
        }

        largestArea = bestAreaSum;

        labels.Dispose(); stats.Dispose(); cents.Dispose(); work.Dispose();
        return outMask;
    }

    static void ApplyMorph(ref Mat img, int closeK, int openK, int closeIt, int openIt)
    {
        using (var kC = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(closeK, closeK)))
        using (var kO = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(openK, openK)))
        {
            Cv2.MorphologyEx(img, img, MorphTypes.Close, kC, iterations: closeIt);
            Cv2.MorphologyEx(img, img, MorphTypes.Open, kO, iterations: openIt);
        }
    }

    // ===
    //  CHỌN RR LỚN NHẤT + MAP SANG RectRotate
    // ===

    static RotatedRect LargestRotatedRect(Mat mask)
    {
        Point[][] cnts; HierarchyIndex[] hier;
        Cv2.FindContours(mask, out cnts, out hier, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
        if (cnts == null || cnts.Length == 0) return new RotatedRect();

        int bestIdx = -1; double bestArea = -1;
        for (int i = 0; i < cnts.Length; i++)
        {
            double a = Cv2.ContourArea(cnts[i]);
            if (a > bestArea) { bestArea = a; bestIdx = i; }
        }
        RotatedRect rot = Cv2.MinAreaRect(cnts[bestIdx]);
        float angCW = -rot.Angle;          // đổi OpenCV CCW -> CW
        if (angCW >= 45f)
        {
            angCW -= 90f;                  // xoay thêm 90° CW
            var sz = new Size2f(rot.Size.Height, rot.Size.Width);
            float angOpenCv = -angCW;      // đổi lại về CCW cho OpenCV
            return new RotatedRect(rot.Center, sz, angOpenCv);
        }
        return rot;
    }

    static RectRotate ToRectRotate(RotatedRect rr)
    {
        float w = rr.Size.Width, h = rr.Size.Height;
        float angle = rr.Angle;
        if (angle > 45)
        {
            w = rr.Size.Height;
            h = rr.Size.Width;
            angle = rr.Angle - 90;
        }
        var r = new RectRotate();
        r.Shape = ShapeType.Rectangle;
        r._rect = new RectangleF(-w / 2f, -h / 2f, w, h);
        r._PosCenter = new PointF(rr.Center.X, rr.Center.Y);
        r._rectRotation = angle;
        return r;
    }

    // ===
    //  Binarize & Probe chọn method
    // ===

    static (BinarizeMethod method, bool invert) PickBestMethod(Mat n8, Mat gray, double rough, DebugSaver D)
    {
        int maxSide = 320;
        double scale = Math.Min(1.0, maxSide / (double)Math.Max(n8.Cols, n8.Rows));
        Size smallSz = new Size((int)Math.Max(1, Math.Round(n8.Cols * scale)),
                                (int)Math.Max(1, Math.Round(n8.Rows * scale)));

        using (var n8s = new Mat()) using (var grs = new Mat())
        {
            Cv2.Resize(n8, n8s, smallSz, 0, 0, InterpolationFlags.Area);
            Cv2.Resize(gray, grs, smallSz, 0, 0, InterpolationFlags.Area);
            D.SaveIfProbe("P00_n8_small", n8s);
            D.SaveIfProbe("P01_gray_small", grs);

            int block = OddClamp(Math.Max(15, Math.Min(smallSz.Width, smallSz.Height) / 14), 15, 75);
            int Sprobe = Math.Min(smallSz.Width, smallSz.Height);
            int closeK_s = OddClamp(Math.Max(3, (int)(Sprobe * 0.007)), 3, 9);
            int openK_s = OddClamp(Math.Max(3, closeK_s - 2), 3, 7);
            int closeIt_s = 1, openIt_s = 1;

            var best = ProbePass(n8s, grs, block, closeK_s, openK_s, closeIt_s, openIt_s, D, 0.01, 0.35);
            if (best.score < 0) best = ProbePass(n8s, grs, block, closeK_s, openK_s, closeIt_s, openIt_s, D, 0.01, 0.50);

            if (best.score < 0 && rough > 20) return (BinarizeMethod.Adaptive, false);
            if (best.score < 0) return (BinarizeMethod.Otsu, Cv2.Mean(n8s).Val0 < 127);
            D.Note($"Probe chosen: {best.method}, invert={best.invert}, score={best.score}");
            return (best.method, best.invert);
        }
    }

    static (BinarizeMethod method, bool invert, double score)
        ProbePass(Mat n8s, Mat grs, int block, int closeK_s, int openK_s, int closeIt_s, int openIt_s,
                  DebugSaver D, double fracMin, double fracMax)
    {
        (BinarizeMethod method, bool invert, double score) best = (BinarizeMethod.Otsu, true, -1);

        foreach (var meth in new[] { BinarizeMethod.Otsu, BinarizeMethod.Adaptive, BinarizeMethod.Sauvola })
            foreach (bool inv in new[] { true, false })
            {
                using (var bin = MakeBinary(n8s, grs, meth, inv, block, D, $"P_bin_{meth}_{(inv ? "inv" : "nor")}"))
                {
                    double frac = Cv2.CountNonZero(bin) / Math.Max(1.0, bin.Rows * bin.Cols);
                    if (frac < fracMin || frac > fracMax)
                    {
                        D.SaveIfProbe($"P_bin_{meth}_{(inv ? "inv" : "nor")}_SKIP", bin);
                        continue;
                    }
                    using (var mask = PostMorphToLargest(bin, closeK_s, openK_s, closeIt_s, openIt_s,
                                                         BorderPolicy.None, out int area,
                                                         D, $"P_post_{meth}_{(inv ? "inv" : "nor")}"))
                    {
                        double score = area;
                        if (score > best.score) best = (meth, inv, score);
                    }
                }
            }
        return best;
    }

    // ==== Binarize 1 phương pháp ====
    static Mat MakeBinary(Mat n8, Mat gray, BinarizeMethod method, bool invert, int scaleParam, DebugSaver D, string tag)
    {
        if (method == BinarizeMethod.Otsu)
        {
            var bin = new Mat();
            var tt = (invert ? ThresholdTypes.BinaryInv : ThresholdTypes.Binary) | ThresholdTypes.Otsu;
            Cv2.Threshold(n8, bin, 0, 255, tt);
            double frac = Cv2.CountNonZero(bin) / Math.Max(1.0, n8.Rows * n8.Cols);
            if (frac > 0.98 || frac < 0.00005) Cv2.BitwiseNot(bin, bin);
            D.Save(tag, bin);
            return bin;
        }
        else if (method == BinarizeMethod.Adaptive)
        {
            int block = OddClamp(scaleParam / 14, 15, 75);
            var bin = new Mat();
            Cv2.AdaptiveThreshold(n8, bin, 255, AdaptiveThresholdTypes.GaussianC,
                                  invert ? ThresholdTypes.BinaryInv : ThresholdTypes.Binary,
                                  block, 2);
            D.Save(tag, bin);
            return bin;
        }
        else // Sauvola
        {
            int block = OddClamp(scaleParam / 14, 15, 75);
            var f = new Mat(); var f2 = new Mat(); var mean = new Mat(); var mean2 = new Mat();
            var var = new Mat(); var std = new Mat(); var T = new Mat();

            n8.ConvertTo(f, MatType.CV_32F);
            Cv2.Multiply(f, f, f2);
            Cv2.BoxFilter(f, mean, MatType.CV_32F, new Size(block, block));
            Cv2.BoxFilter(f2, mean2, MatType.CV_32F, new Size(block, block));
            Cv2.Multiply(mean, mean, var);
            Cv2.Subtract(mean2, var, var);
            Cv2.Max(var, 0, var);
            Cv2.Sqrt(var, std);

            float k = 0.34f, R = 128f;
            using (var s_div_R = new Mat())
            using (var s_div_R_minus1 = new Mat())
            using (var k_term = new Mat())
            using (var one = new Mat(mean.Size(), MatType.CV_32F, new Scalar(1)))
            {
                Cv2.Divide(std, R, s_div_R);
                Cv2.Subtract(s_div_R, 1, s_div_R_minus1);
                Cv2.Multiply(s_div_R_minus1, k, k_term);
                Cv2.Add(one, k_term, k_term);
                Cv2.Multiply(mean, k_term, T);
            }

            var bin = CompareByThresholdFloat(f, T, invert);
            D.Save(tag + "_T", T, normalize: true);
            D.Save(tag, bin);

            f.Dispose(); f2.Dispose(); mean.Dispose(); mean2.Dispose(); var.Dispose(); std.Dispose(); T.Dispose();
            return bin;
        }
    }

    // ===
    //  Helpers khác
    // ===

    static Mat CompareByThresholdFloat(Mat f, Mat T, bool invert)
    {
        var diff = new Mat(); Cv2.Subtract(f, T, diff);
        var bin32 = new Mat();
        Cv2.Threshold(diff, bin32, 0, 255, invert ? ThresholdTypes.BinaryInv : ThresholdTypes.Binary);
        var bin8 = new Mat(); bin32.ConvertTo(bin8, MatType.CV_8U);
        diff.Dispose(); bin32.Dispose();
        return bin8;
    }

    static Mat VisualizeLabels(Mat labels, int nLabels)
    {
        var vis = new Mat(labels.Size(), MatType.CV_8UC3, Scalar.All(0));
        var rng = new Random(1234);
        for (int i = 1; i < nLabels; i++)
        {
            byte r = (byte)rng.Next(60, 255);
            byte g = (byte)rng.Next(60, 255);
            byte b = (byte)rng.Next(60, 255);
            using (var m = new Mat())
            {
                Cv2.InRange(labels, new Scalar(i), new Scalar(i), m);
                vis.SetTo(new Scalar(b, g, r), m);
            }
        }
        return vis;
    }

    static int OddClamp(int v, int lo, int hi)
    {
        v = Math.Max(lo, Math.Min(hi, v));
        if ((v & 1) == 0) v++;
        if (v > hi) v -= 2;
        return Math.Max(lo | 1, v);
    }

    // ==== Debug saver (tùy chọn) ====
    sealed class DebugSaver : IDisposable
    {
        readonly bool on;
        readonly bool saveProbes;
        readonly string dir;
        int step = 0;
        readonly string prefix;

        public DebugSaver(DebugOptions opt)
        {
            on = opt.Enable;
            saveProbes = opt.SaveProbes;
            prefix = string.IsNullOrWhiteSpace(opt.Prefix) ? "" : (opt.Prefix + "_");
            if (on)
            {
                dir = string.IsNullOrWhiteSpace(opt.Dir)
                    ? Path.Combine(Environment.CurrentDirectory, "debug_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"))
                    : opt.Dir;
                Directory.CreateDirectory(dir);
            }
        }

        public void Save(string name, Mat img, bool normalize = false)
        {
            if (!on || img == null || img.Empty()) return;
            using (var out3 = PrepareToSave(img, normalize))
            {
                string fn = Path.Combine(dir, $"{prefix}{++step:D2}_{name}.png");
                Cv2.ImWrite(fn, out3);
            }
        }

        public void SaveIfProbe(string name, Mat img, bool normalize = false)
        {
            if (!saveProbes) return;
            Save(name, img, normalize);
        }

        public void Note(string text)
        {
            if (!on) return;
            File.AppendAllText(Path.Combine(dir, $"{prefix}notes.txt"), text + Environment.NewLine);
        }

        static Mat PrepareToSave(Mat src, bool normalize)
        {
            Mat toSave;
            if (normalize && (src.Type().Depth != MatType.CV_8U))
            {
                toSave = new Mat();
                Cv2.Normalize(src, toSave, 0, 255, NormTypes.MinMax);
                toSave.ConvertTo(toSave, MatType.CV_8U);
            }
            else if (src.Type().Depth != MatType.CV_8U)
            {
                toSave = new Mat();
                src.ConvertTo(toSave, MatType.CV_8U);
            }
            else
            {
                toSave = src.Clone();
            }

            if (toSave.Channels() == 1)
            {
                var out3 = new Mat();
                Cv2.CvtColor(toSave, out3, ColorConversionCodes.GRAY2BGR);
                toSave.Dispose();
                return out3;
            }
            return toSave;
        }

        public void Dispose() { /* no-op */ }
    }
}
