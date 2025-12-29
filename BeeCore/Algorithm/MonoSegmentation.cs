using OpenCvSharp;
using System;

public struct MonoSegParams
{
    public int bgBlurK;
    public int blackHatK;
    public bool useBlackHat;
    public int mode;      // 0: bg-img, 1: img-bg
    public int openK;
    public int closeK;
}

public static class MonoSegmentation
{
    // ===================== MAIN =====================
    public static int SegmentLowContrastMono(
        Mat gray8U,
        out Mat outMask8U,
        MonoSegParams pin,
        out Mat outScore
    )
    {
        if (gray8U.Type() != MatType.CV_8UC1)
            throw new ArgumentException("Input must be CV_8UC1");

        MonoSegParams p = pin;
        p.bgBlurK = Math.Max(3, p.bgBlurK | 1);
        p.blackHatK = Math.Max(3, p.blackHatK | 1);

        // ---------- 1) SCORE ----------
        Mat score = new Mat();

        if (p.useBlackHat)
        {
            Mat k = Cv2.GetStructuringElement(
                MorphShapes.Rect,
                new Size(p.blackHatK, p.blackHatK));
            Cv2.MorphologyEx(gray8U, score, MorphTypes.BlackHat, k);
        }
        else
        {
            Mat bg = new Mat();
            Cv2.Blur(gray8U, bg, new Size(p.bgBlurK, p.bgBlurK));
            if (p.mode == 0)
                Cv2.Subtract(bg, gray8U, score);
            else
                Cv2.Subtract(gray8U, bg, score);
        }

        Cv2.Normalize(score, score, 0, 255, NormTypes.MinMax);
        score.ConvertTo(score, MatType.CV_8UC1);

        // ---------- 2) THRESHOLD ----------
        int thr = AutoThresholdHistogramValley(score);

        outMask8U = new Mat();
        Cv2.Threshold(score, outMask8U, thr, 255, ThresholdTypes.Binary);

        // ---------- 3) MORPH BASIC ----------
        if (p.openK > 1)
        {
            Mat k = Cv2.GetStructuringElement(
                MorphShapes.Rect, new Size(p.openK, p.openK));
            Cv2.MorphologyEx(outMask8U, outMask8U, MorphTypes.Open, k);
        }

        if (p.closeK > 1)
        {
            Mat k = Cv2.GetStructuringElement(
                MorphShapes.Rect, new Size(p.closeK, p.closeK));
            Cv2.MorphologyEx(outMask8U, outMask8U, MorphTypes.Close, k);
        }

        // =====================================================
        // ====== PHẦN QUAN TRỌNG – NỐI KÍN KHUNG NGOÀI =========
        // =====================================================

        BridgeBorder_Constrained(outMask8U,band: 40);     // nối đứt ngang + dọc
      //  FillOuterShape(outMask8U);        // fill kín toàn khung
     //   ExtractOuterBorder(outMask8U, 3); // lấy lại viền (optional)
       // KeepLargestBlob(outMask8U);       // giữ khung lớn nhất

        outScore = score;
        return Cv2.CountNonZero(outMask8U);
    }
    static Mat MakeEdgeBandMask(Size sz, int band)
    {
        Mat mask = Mat.Zeros(sz, MatType.CV_8UC1);

        Cv2.Rectangle(mask,
            new Rect(band, band, sz.Width - 2 * band, sz.Height - 2 * band),
            Scalar.White, -1);

        Cv2.BitwiseNot(mask, mask); // chỉ giữ viền
        return mask;
    }
    static void BridgeBorder_Constrained(Mat bin, int band)
    {
        Mat edgeMask = MakeEdgeBandMask(bin.Size(), band);

        Mat work = new Mat();
        Cv2.BitwiseAnd(bin, edgeMask, work);

        // bridge nhỏ hơn
        Mat kH = Cv2.GetStructuringElement(
            MorphShapes.Rect, new Size(15, 1));
        Mat kV = Cv2.GetStructuringElement(
            MorphShapes.Rect, new Size(1, 15));

        Cv2.MorphologyEx(work, work, MorphTypes.Close, kH);
        Cv2.MorphologyEx(work, work, MorphTypes.Close, kV);

        // ghép lại với ảnh gốc
        Cv2.BitwiseOr(bin, work, bin);
    }
    // ===================== CORE TOPOLOGY =====================

    // Nối viền theo HAI HƯỚNG
    static void BridgeBorderSmart(Mat bin)
    {
        Mat kH = Cv2.GetStructuringElement(
            MorphShapes.Rect, new Size(25, 1));
        Cv2.MorphologyEx(bin, bin, MorphTypes.Close, kH);

        Mat kV = Cv2.GetStructuringElement(
            MorphShapes.Rect, new Size(1, 25));
        Cv2.MorphologyEx(bin, bin, MorphTypes.Close, kV);
    }

    // FloodFill để vá bo góc + đứt chéo
    static void FillOuterShape(Mat bin)
    {
        Mat flood = bin.Clone();
        Cv2.FloodFill(flood, new Point(0, 0), Scalar.White);

        Mat floodInv = new Mat();
        Cv2.BitwiseNot(flood, floodInv);

        Cv2.BitwiseOr(bin, floodInv, bin);
    }

    // Lấy lại viền ngoài sau khi fill
    static void ExtractOuterBorder(Mat filled, int thickness)
    {
        Mat er = new Mat();
        Mat k = Cv2.GetStructuringElement(
            MorphShapes.Rect,
            new Size(thickness * 2 + 1, thickness * 2 + 1));

        Cv2.Erode(filled, er, k);
        Cv2.Subtract(filled, er, filled);
    }

    // Giữ blob lớn nhất (khung)
    static void KeepLargestBlob(Mat bin)
    {
        Cv2.FindContours(
            bin,
            out Point[][] contours,
            out _,
            RetrievalModes.External,
            ContourApproximationModes.ApproxSimple);

        if (contours.Length == 0)
            return;

        int best = -1;
        double maxArea = 0;

        for (int i = 0; i < contours.Length; i++)
        {
            double a = Cv2.ContourArea(contours[i]);
            if (a > maxArea)
            {
                maxArea = a;
                best = i;
            }
        }

        bin.SetTo(Scalar.Black);
        Cv2.DrawContours(bin, contours, best, Scalar.White, -1);
    }

    // ===================== AUTO THRESHOLD =====================
    static int AutoThresholdHistogramValley(Mat img8U)
    {
        int[] hist = new int[256];
        for (int y = 0; y < img8U.Rows; y++)
        {
            for (int x = 0; x < img8U.Cols; x++)
                hist[img8U.At<byte>(y, x)]++;
        }

        int peak1 = 0, peak2 = 0;
        for (int i = 1; i < 255; i++)
        {
            if (hist[i] > hist[peak1])
                peak1 = i;
        }

        for (int i = 1; i < 255; i++)
        {
            if (i != peak1 && hist[i] > hist[peak2])
                peak2 = i;
        }

        int l = Math.Min(peak1, peak2);
        int r = Math.Max(peak1, peak2);

        int valley = l;
        int minVal = hist[l];
        for (int i = l + 1; i < r; i++)
        {
            if (hist[i] < minVal)
            {
                minVal = hist[i];
                valley = i;
            }
        }
        return valley;
    }
}
