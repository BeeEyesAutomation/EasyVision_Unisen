using OpenCvSharp;
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
        // =========== 1. LÀM MỊN / NHIỄU ===========

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
