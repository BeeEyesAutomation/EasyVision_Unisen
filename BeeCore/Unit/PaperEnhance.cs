using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace BeeCore
{
  

    public static class PaperEnhance
    {
        public static string Err = "";

        // =========================================================
        // MAIN
        // =========================================================
        public static Mat AutoEnhanceForPaper(Mat gray, out Mat paperMask)
        {
            Err = "";
            paperMask = new Mat();

            if (gray == null || gray.Empty())
            {
                Err = "Input image empty";
                return null;
            }

            if (gray.Type() != MatType.CV_8UC1)
            {
                Err = "Input must be grayscale CV_8UC1";
                return null;
            }

            // =========================
            // 1. Detect paper FIRST (raw gray)
            // =========================
            paperMask = DetectPaperMaskRaw(gray);
            if (Cv2.CountNonZero(paperMask) < gray.Rows * gray.Cols * 0.05)
            {
                Err = "Paper not found";
                return null;
            }

            // =========================
            // 2. Mask paper only
            // =========================
            Mat paperOnly = new Mat();
            gray.CopyTo(paperOnly, paperMask);

            // =========================
            // 3. Normalize illumination (inside paper)
            // =========================
            Mat blurBg = new Mat();
            Cv2.GaussianBlur(paperOnly, blurBg, new Size(0, 0), 45);

            Mat norm = new Mat();
            Cv2.Divide(paperOnly, blurBg, norm, 255);

            // =========================
            // 4. Percentile contrast stretch (paper ROI)
            // =========================
             Mat roi = CenterRoi(norm, 0.8);

            double pLow = Percentile(roi, 1.0);
            double pHigh = Percentile(roi, 99.0);

            if (pHigh <= pLow + 5)
            {
                pLow = 10;
                pHigh = 245;
            }

            Mat stretched = ContrastStretch(norm, pLow, pHigh);

            // =========================
            // 5. CLAHE
            // =========================
            var clahe = Cv2.CreateCLAHE(3.0, new Size(8, 8));
            Mat enhanced = new Mat();
            clahe.Apply(stretched, enhanced);

            return enhanced;
        }

        // =========================================================
        // PAPER DETECTION (RAW)
        // =========================================================
        private static Mat DetectPaperMaskRaw(Mat gray)
        {
            Mat bin = new Mat();
            Cv2.Threshold(gray, bin, 0, 255,
                ThresholdTypes.Binary | ThresholdTypes.Otsu);

            // ensure paper = white
            if (Cv2.Mean(bin).Val0 < 127)
                Cv2.BitwiseNot(bin, bin);

            // close to fill paper
            Mat k = Cv2.GetStructuringElement(
                MorphShapes.Rect, new Size(31, 31));
            Cv2.MorphologyEx(bin, bin, MorphTypes.Close, k);

            return PickLargestNonBorderComponent(bin, 10);
        }

        // =========================================================
        // PICK CC NOT TOUCH BORDER
        // =========================================================
        private static Mat PickLargestNonBorderComponent(Mat bin, int border)
        {
            Mat labels = new Mat(), stats = new Mat(), centroids = new Mat();
            int n = Cv2.ConnectedComponentsWithStats(
                bin, labels, stats, centroids);

            int W = bin.Cols, H = bin.Rows;
            int best = -1, bestArea = 0;

            for (int i = 1; i < n; i++)
            {
                int x = stats.At<int>(i, (int)ConnectedComponentsTypes.Left);
                int y = stats.At<int>(i, (int)ConnectedComponentsTypes.Top);
                int w = stats.At<int>(i, (int)ConnectedComponentsTypes.Width);
                int h = stats.At<int>(i, (int)ConnectedComponentsTypes.Height);
                int area = stats.At<int>(i, (int)ConnectedComponentsTypes.Area);

                if (area < 5000) continue;

                bool touch =
                    x <= border || y <= border ||
                    x + w >= W - border ||
                    y + h >= H - border;

                if (touch) continue;

                if (area > bestArea)
                {
                    bestArea = area;
                    best = i;
                }
            }

            Mat mask = Mat.Zeros(bin.Size(), MatType.CV_8UC1);
            if (best >= 0)
            {
                Cv2.Compare(labels, best, mask, CmpType.EQ);
                mask.ConvertTo(mask, MatType.CV_8UC1);
            }
            return mask;
        }

        // =========================================================
        // CONTRAST STRETCH (TRUE PERCENTILE)
        // =========================================================
        private static unsafe Mat ContrastStretch(Mat src, double low, double high)
        {
            Mat dst = new Mat(src.Size(), MatType.CV_8UC1);

            if (src.Type() != MatType.CV_8UC1)
                throw new ArgumentException("ContrastStretch expects CV_8UC1");

            double denom = (high - low);
            if (denom < 1e-6) denom = 1.0; // tránh chia 0

            for (int y = 0; y < src.Rows; y++)
            {
                byte* sp = (byte*)src.Ptr(y).ToPointer();
                byte* dp = (byte*)dst.Ptr(y).ToPointer();

                for (int x = 0; x < src.Cols; x++)
                {
                    double v = sp[x];
                    if (v <= low) dp[x] = 0;
                    else if (v >= high) dp[x] = 255;
                    else dp[x] = (byte)((v - low) * 255.0 / denom);
                }
            }

            return dst;
        }


        // =========================================================
        // PERCENTILE
        // =========================================================
        private static double Percentile(Mat img8u, double percentile)
        {
            Mat hist = new Mat();
            Cv2.CalcHist(
                new[] { img8u }, new[] { 0 }, null,
                hist, 1, new[] { 256 },
                new[] { new Rangef(0, 256) });

            int total = img8u.Rows * img8u.Cols;
            int target = (int)(total * percentile / 100.0);

            double acc = 0;
            for (int i = 0; i < 256; i++)
            {
                acc += hist.At<float>(i);
                if (acc >= target)
                    return i;
            }
            return 255;
        }

        // =========================================================
        // CENTER ROI
        // =========================================================
        private static Mat CenterRoi(Mat img, double frac)
        {
            int w = img.Cols, h = img.Rows;
            int rw = (int)(w * frac);
            int rh = (int)(h * frac);
            int x = (w - rw) / 2;
            int y = (h - rh) / 2;
            return new Mat(img, new Rect(x, y, rw, rh));
        }
    }

}
