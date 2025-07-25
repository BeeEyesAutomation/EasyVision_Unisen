﻿using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.XPhoto;          // cho White‑Balance

namespace BeeCore.Algorithm
{
    /// <summary>
    /// Chứa hàm factory trả về delegate ImageFilter (Mat→Mat)
    /// Dùng:  pipeline.Add(Filters.GaussianBlur(new Size(5,5),1.5));
    /// </summary>
    public static class Filters
    {
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            else if (value.CompareTo(max) > 0) return max;
            else return value;
        }
        public static Mat GetStrongEdgesOnly(Mat gray, double percentile = 0.98)
        {
            // 1. Làm mượt ảnh
            Mat blur = new Mat();
            Cv2.GaussianBlur(gray, blur, new Size(3, 3), sigmaX: 1.0);

            // 2. Tính gradient
            Mat gradX = new Mat(), gradY = new Mat();
            Cv2.Sobel(blur, gradX, MatType.CV_32F, 1, 0, ksize: 3);
            Cv2.Sobel(blur, gradY, MatType.CV_32F, 0, 1, ksize: 3);

            // 3. Magnitude
            Mat magnitude = new Mat();
            Cv2.Magnitude(gradX, gradY, magnitude);

            // 4. Tự động tính ngưỡng từ histogram gradient
            float[] magData = new float[magnitude.Rows * magnitude.Cols];
            magnitude.GetArray(out magData);

            Array.Sort(magData);
            int index = (int)(magData.Length * percentile);
            float threshold = magData[Clamp(index, 0, magData.Length - 1)];
            // 5. Chuẩn hóa và tạo nhị phân
            Mat result = new Mat();
            Cv2.Threshold(magnitude, result, threshold, 255, ThresholdTypes.Binary);

            // Chuyển về kiểu 8-bit để hiển thị hoặc xử lý tiếp
            result.ConvertTo(result, MatType.CV_8U);
            Cv2.ImWrite("edge.png", result);
            return result;
        }
        // =========== 1. LÀM MỊN / NHIỄU ===========
        public static (int lower, int upper) AutoCannyThresholdFromHistogram(Mat gray, double k1 = 0.66, double k2 = 1.33)
        {
            // 1. Histogram
            Mat hist = new Mat();
            int[] histSize = { 256 };
            Rangef[] ranges = { new Rangef(0, 256) };
            Cv2.CalcHist(new Mat[] { gray }, new int[] { 0 }, null, hist, 1, histSize, ranges);

            // 2. Đỉnh histogram
            double minVal, maxVal;
            Point minLoc, maxLoc;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal, out minLoc, out maxLoc);
            int peak = maxLoc.Y != 0 ? maxLoc.Y : maxLoc.X; // fallback cho mọi version OpenCvSharp

            // 3. Ngưỡng
            int lower = (int)Math.Max(0, peak * k1);
            int upper = (int)Math.Min(255, peak * k2);
            return (lower, upper);
        }
        public static Mat Edge(Mat raw)
        {
            Mat edges = new Mat();
            Mat gray = new Mat();
            if (raw.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(raw, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = raw.Clone();

            // 1. Histogram truncation để giảm vùng trắng chói
            Cv2.Threshold(gray, gray, 245, 245, ThresholdTypes.Trunc);

            // 2. Tăng tương phản nếu cần
            Cv2.Normalize(gray, gray, 0, 255, NormTypes.MinMax);

            // 3. Làm mượt bằng Gaussian Blur
            Mat smooth = new Mat();
            Cv2.GaussianBlur(gray, smooth, new Size(5, 5), sigmaX: 1.0);

            // 4. Tự động tính threshold Canny dựa trên histogram
            var (lower, upper) = AutoCannyThresholdFromHistogram(smooth, k1: 0.66, k2: 1.33);

           
            Cv2.Canny(smooth, edges, lower, upper);

            // 6. Morphological closing để nối đoạn đứt
            var kernelClose = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.MorphologyEx(edges, edges, MorphTypes.Close, kernelClose);

            // 7. Làm dày/mịn cạnh
            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.Dilate(edges, edges, kernel, iterations: 1);
            Cv2.Erode(edges, edges, kernel, iterations: 1);
          //  Cv2.ImWrite("Edge.png", edges);
            return edges;
        }
        public static Mat Threshold(Mat raw)
        {
            Mat edges = new Mat();
            Mat gray = new Mat();
            if (raw.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(raw, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = raw.Clone();
            Cv2.Threshold(gray, gray, 0, 245, ThresholdTypes.Otsu);
            Cv2.BitwiseNot(gray, gray);
            // 3. Làm mượt bằng Gaussian Blur
            Mat smooth = new Mat();
            Cv2.GaussianBlur(gray, smooth, new Size(5, 5), sigmaX: 1.0);

            // 4. Tự động tính threshold Canny dựa trên histogram
            var (lower, upper) = AutoCannyThresholdFromHistogram(smooth, k1: 0.66, k2: 1.33);
            // 1. Histogram truncation để giảm vùng trắng chói
           
            Cv2.Canny(smooth, edges, lower, upper);
            return edges;
        }
        public static ImageFilter GaussianBlur(Size ksize, double sigmaX, double sigmaY = 0) =>
            delegate (Mat src, Mat dst) { Cv2.GaussianBlur(src, dst, ksize, sigmaX, sigmaY); };

        public static ImageFilter MedianBlur(int ksize) =>
            delegate (Mat src, Mat dst) { Cv2.MedianBlur(src, dst, ksize); };

        public static ImageFilter Bilateral(int d, double sigmaColor, double sigmaSpace) =>
            delegate (Mat src, Mat dst) { Cv2.BilateralFilter(src, dst, d, sigmaColor, sigmaSpace); };

        // Sharpen bằng Un‑sharp mask
        public static ImageFilter Sharpen(double amount = 1.0)
        {
            return delegate (Mat src, Mat dst)
            {
                Mat blurred = new Mat();
                Cv2.GaussianBlur(src, blurred, new Size(0, 0), 3);
                Cv2.AddWeighted(src, 1 + amount, blurred, -amount, 0, dst);
                blurred.Dispose();
            };
        }

        // =========== 2. CÂN BẰNG / TƯƠNG PHẢN ===========

        public static ImageFilter Clahe(double clipLimit, Size tile) =>
            delegate (Mat src, Mat dst)
            {
                using (var clahe = Cv2.CreateCLAHE(clipLimit, tile))
                    clahe.Apply(src, dst);
            };

        public static ImageFilter HistEq() =>
            delegate (Mat src, Mat dst) { Cv2.EqualizeHist(src, dst); };

        public static ImageFilter Gamma(double gamma) =>
            delegate (Mat src, Mat dst)
            {
                Mat lut = new Mat(1, 256, MatType.CV_8UC1);
                for (int i = 0; i < 256; i++)
                    lut.Set(i, 0, (byte)(Math.Pow(i / 255.0, 1.0 / gamma) * 255.0));
                Cv2.LUT(src, lut, dst);
                lut.Dispose();
            };

        // =========== 3. EDGE / GRADIENT ===========

        public static ImageFilter Canny(double th1, double th2, int aperture = 3, bool l2 = false) =>
            delegate (Mat src, Mat dst) { Cv2.Canny(src, dst, th1, th2, aperture, l2); };

        // xDir=true => Sobel X ; false => Sobel Y
        public static ImageFilter Sobel(bool xDir, int ksize = 3, double scale = 1, double delta = 0) =>
            delegate (Mat src, Mat dst)
            {
                int dx = xDir ? 1 : 0;
                int dy = xDir ? 0 : 1;
                Cv2.Sobel(src, dst, MatType.CV_16S, dx, dy, ksize, scale, delta);
                Cv2.ConvertScaleAbs(dst, dst);
            };

        public static ImageFilter Laplacian(int ksize = 3) =>
            delegate (Mat src, Mat dst)
            {
                Cv2.Laplacian(src, dst, MatType.CV_16S, ksize);
                Cv2.ConvertScaleAbs(dst, dst);
            };

        // =========== 4. NGƯỠNG / THRESHOLD ===========

        public static ImageFilter AdaptiveThresh(AdaptiveThresholdTypes method,
                                                 int block = 11, double C = 2) =>
            delegate (Mat src, Mat dst)
            {
                Cv2.AdaptiveThreshold(src, dst, 255,
                    method, ThresholdTypes.Binary, block, C);
            };

        public static ImageFilter OtsuThresh() =>
            delegate (Mat src, Mat dst)
            {
                Cv2.Threshold(src, dst, 0, 255,
                    ThresholdTypes.Binary | ThresholdTypes.Otsu);
            };

        // =========== 5. MORPHOLOGY ===========

        public static ImageFilter Morph(MorphTypes type, Size ksize, int iterations = 1) =>
            delegate (Mat src, Mat dst)
            {
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, ksize);
                Cv2.MorphologyEx(src, dst, type, kernel, iterations: iterations);
                kernel.Dispose();
            };

        // true = erode, false = dilate
        public static ImageFilter ErodeDilate(bool erode, Size ksize, int iterations = 1) =>
            delegate (Mat src, Mat dst)
            {
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, ksize);
                if (erode) Cv2.Erode(src, dst, kernel, iterations: iterations);
                else Cv2.Dilate(src, dst, kernel, iterations: iterations);
                kernel.Dispose();
            };

        // =========== 6. WHITE BALANCE ===========

        public static ImageFilter WhiteBalanceGrayWorld() =>
            delegate (Mat src, Mat dst)
            {
                if (src.Channels() == 1) { src.CopyTo(dst); return; }
                using (var wb = GrayworldWB.Create()) wb.BalanceWhite(src, dst);
            };

        public static ImageFilter WhiteBalanceSimple() =>
            delegate (Mat src, Mat dst)
            {
                if (src.Channels() == 1) { src.CopyTo(dst); return; }
                using (var wb = SimpleWB.Create()) wb.BalanceWhite(src, dst);
            };

        // =========== 7. LỌC / GIỮ MÀU HSV ===========

        /// <summary>Giữ lại vùng màu HSV (hMin‑hMax), còn lại thành đen.</summary>
        public static ImageFilter KeepColorRange(int hMin, int hMax,
                                                 int sMin = 50, int vMin = 50) =>
            delegate (Mat src, Mat dst)
            {
                Mat hsv = new Mat();
                Cv2.CvtColor(src, hsv, ColorConversionCodes.BGR2HSV);
                Scalar lower = new Scalar(hMin, sMin, vMin);
                Scalar upper = new Scalar(hMax, 255, 255);
                Mat mask = new Mat();
                Cv2.InRange(hsv, lower, upper, mask);
                src.CopyTo(dst, mask);
                hsv.Dispose();
                mask.Dispose();
            };

        // =========== 8. CHUYỂN XÁM & RESIZE ===========

        public static ImageFilter ConvertGray() =>
            delegate (Mat src, Mat dst)
            {
                if (src.Channels() == 1) src.CopyTo(dst);
                else Cv2.CvtColor(src, dst, ColorConversionCodes.BGR2GRAY);
            };

        public static ImageFilter Resize(double scaleX, double scaleY,
                                         InterpolationFlags interp = InterpolationFlags.Linear) =>
            delegate (Mat src, Mat dst)
            { Cv2.Resize(src, dst, Size.Zero, scaleX, scaleY, interp); };
    }
}
