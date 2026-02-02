using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.XImgProc;
using OpenCvSharp.XPhoto;          // cho White‑Balance
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

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
        public static Mat RemoveBorderTouchAndKeepCenter(
        Mat bin,
        int borderMargin = 10,
        int minArea = 5000)
        {
            if (bin.Empty())
                throw new ArgumentException("bin empty");
            if (bin.Type() != MatType.CV_8UC1)
                throw new ArgumentException("bin must be CV_8UC1");

            // đảm bảo binary
            Mat bw = new Mat();
            Cv2.Threshold(bin, bw, 0, 255, ThresholdTypes.Binary);

            Mat labels = new Mat();
            Mat stats = new Mat();
            Mat centroids = new Mat();

            int n = Cv2.ConnectedComponentsWithStats(
                bw, labels, stats, centroids,
                PixelConnectivity.Connectivity8,
                MatType.CV_32S);

            int W = bw.Cols;
            int H = bw.Rows;

            int bestLabel = -1;
            int bestArea = 0;

            for (int i = 1; i < n; i++) // 0 = background
            {
                int x = stats.At<int>(i, (int)ConnectedComponentsTypes.Left);
                int y = stats.At<int>(i, (int)ConnectedComponentsTypes.Top);
                int w = stats.At<int>(i, (int)ConnectedComponentsTypes.Width);
                int h = stats.At<int>(i, (int)ConnectedComponentsTypes.Height);
                int area = stats.At<int>(i, (int)ConnectedComponentsTypes.Area);

                if (area < minArea)
                    continue;

                // ❌ loại component chạm biên
                bool touchBorder =
                    x <= borderMargin ||
                    y <= borderMargin ||
                    (x + w) >= (W - borderMargin) ||
                    (y + h) >= (H - borderMargin);

                if (touchBorder)
                    continue;

                // ✅ chọn component lớn nhất còn lại
                if (area > bestArea)
                {
                    bestArea = area;
                    bestLabel = i;
                }
            }

            Mat resultMask = Mat.Zeros(bw.Size(), MatType.CV_8UC1);
            if (bestLabel >= 0)
            {
                Cv2.Compare(labels, bestLabel, resultMask, CmpType.EQ);
                resultMask.ConvertTo(resultMask, MatType.CV_8UC1); // 0/255
            }

            return resultMask;
        }
        public static Mat AutoEnhanceForPaper(Mat gray)
        {
            if (gray.Empty())
                throw new ArgumentException("Input image empty");

            Mat g = gray.Clone();

            // =========================
            // 1. Normalize illumination (remove shading)
            // =========================
            Mat blurBg = new Mat();
            Cv2.GaussianBlur(g, blurBg, new Size(0, 0), 45);
            Mat norm = new Mat();
            Cv2.Divide(g, blurBg, norm, scale: 255);

            // =========================
            // 2. Auto contrast stretch (percentile)
            // =========================
            double pLow = Percentile(norm, 1.0);
            double pHigh = Percentile(norm, 99.0);

            Mat stretched = new Mat();
            Cv2.Normalize(norm, stretched, pLow, pHigh, NormTypes.MinMax);
            stretched.ConvertTo(stretched, MatType.CV_8UC1);

            // =========================
            // 3. CLAHE (adaptive)
            // =========================
            var clahe = Cv2.CreateCLAHE(clipLimit: 3.0, tileGridSize: new Size(4,4));
            Mat enhanced = new Mat();
            clahe.Apply(stretched, enhanced);

            return enhanced;
        }

        /// <summary>
        /// Auto detect paper mask
        /// </summary>
        public static Mat DetectPaperMask(Mat enhanced)
        {
            Mat bin1 = new Mat();
            Mat bin2 = new Mat();

            // Adaptive threshold
            Cv2.AdaptiveThreshold(
                enhanced, bin1, 255,
                AdaptiveThresholdTypes.GaussianC,
                ThresholdTypes.Binary,
                51, -5
            );

            // Otsu
            Cv2.Threshold(
                enhanced, bin2, 0, 255,
                ThresholdTypes.Binary | ThresholdTypes.Otsu
            );

            // Pick better one
            Mat bin = (Cv2.CountNonZero(bin1) > Cv2.CountNonZero(bin2)) ? bin1 : bin2;

            // Morph clean
            Mat k = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(9, 9));
            Cv2.MorphologyEx(bin, bin, MorphTypes.Close, k);

            return bin;
        }

        // =========================
        // Helper: percentile
        // =========================
        private static double Percentile(Mat img8u, double percentile)
        {
            if (img8u.Empty()) return 0;
            if (img8u.Type() != MatType.CV_8UC1)
                throw new ArgumentException("Percentile expects CV_8UC1");

            // hist as Mat (256x1 float)
              var hist = new Mat();
            Cv2.CalcHist(
                images: new[] { img8u },
                channels: new[] { 0 },
                mask: null,
                hist: hist,
                dims: 1,
                histSize: new[] { 256 },
                ranges: new[] { new Rangef(0, 256) }
            );

            int total = img8u.Rows * img8u.Cols;
            int target = (int)Math.Round(total * (percentile / 100.0));
            if (target <= 0) return 0;

            double acc = 0;
            for (int i = 0; i < 256; i++)
            {
                // hist is float
                float h = hist.At<float>(i, 0);
                acc += h;
                if (acc >= target)
                    return i;
            }
            return 255;
        }
        public static String Err = "";
        public static Mat GetStrongEdgesOnly(Mat raw, double percentile = 0.98)
        {
            Mat blur = new Mat(); Mat magnitude = new Mat();
            try
            {  // 0) Bảo đảm 1 kênh (grayscale)
                using (Mat gray = (raw.Channels() == 1) ? raw.Clone() : raw.CvtColor(ColorConversionCodes.BGR2GRAY))
                {

                    // 1. Làm mượt ảnh

                    Cv2.GaussianBlur(gray, blur, new Size(3, 3), sigmaX: 1.0);

                    // 2. Tính gradient
                    Mat gradX = new Mat(), gradY = new Mat();
                    Cv2.Sobel(blur, gradX, MatType.CV_32F, 1, 0, ksize: 3);
                    Cv2.Sobel(blur, gradY, MatType.CV_32F, 0, 1, ksize: 3);

                    // 3. Magnitude

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

                    return result;
                }
            }
            catch(Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Filter", ex.Message));
              
            }
            finally
            {
                blur.Dispose();
                magnitude.Dispose();

            }
            // Cv2.ImWrite("edge.png", result);
            return new Mat();
        }
        // ==== 1. LÀM MỊN / NHIỄU ====
        public static (int lower, int upper) AutoCannyThresholdFromHistogram(Mat gray, double k1 = 0.66, double k2 = 1.33)
        {
            // 1. Histogram
            using (Mat hist = new Mat())
            {
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
        }
        public static Mat Edge(Mat raw)
        {
            Mat gray = new Mat();
            try
            {
                Mat edges = new Mat();
              
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
                return edges;
            }
            catch(Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Filter", ex.Message));

            }
            finally
            {
                gray.Dispose();
            }
            //  Cv2.ImWrite("Edge.png", edges);
            return null;
        }
        public static Mat Threshold(Mat raw,int Threshold, ThresholdTypes thresholdTypes=ThresholdTypes.Binary)
        {
            Mat edges = new Mat();
            Mat gray = new Mat();
            try
            {  
           
            if (raw.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(raw, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = raw.Clone();
            Cv2.Threshold(gray, gray, Threshold, 255, thresholdTypes);
           // Cv2.BitwiseNot(gray, gray);
            // 3. Làm mượt bằng Gaussian Blur
            Mat smooth = new Mat();
          //  Cv2.GaussianBlur(gray, smooth, new Size(5, 5), sigmaX: 1.0);

            // 4. Tự động tính threshold Canny dựa trên histogram
          //  var (lower, upper) = AutoCannyThresholdFromHistogram(smooth, k1: 0.66, k2: 1.33);
            // 1. Histogram truncation để giảm vùng trắng chói
           
            Cv2.Canny(gray, edges, 0, 255);
                // 6. Morphological closing để nối đoạn đứt
                var kernelClose = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
                Cv2.MorphologyEx(edges, edges, MorphTypes.Close, kernelClose);

                // 7. Làm dày/mịn cạnh
                var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
                Cv2.Dilate(edges, edges, kernel, iterations: 1);
                Cv2.Erode(edges, edges, kernel, iterations: 1);
               
                return edges;
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Filter", ex.Message));

            }
            finally
            {
                gray.Dispose();
            }
            return null;
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

        // ==== 2. CÂN BẰNG / TƯƠNG PHẢN ====

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

        // ==== 3. EDGE / GRADIENT ====

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

        // ==== 4. NGƯỠNG / THRESHOLD ====

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

        // ==== 5. MORPHOLOGY ====

        public static ImageFilter Morph(MorphTypes type, Size ksize, int iterations = 1) =>
            delegate (Mat src, Mat dst)
            {
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, ksize);
                Cv2.MorphologyEx(src, dst, type, kernel, iterations: iterations);
                kernel.Dispose();

            };
        public static Mat Morphology( Mat src ,MorphTypes type, Size ksize, int iterations = 1) 
         { Mat dst =new Mat();

            using (Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, ksize))
            {
                Cv2.MorphologyEx(src, dst, type, kernel, iterations: iterations);
                kernel.Dispose();
                return dst;
            }
          }


        public static Mat KeepThinEdges(Mat edges, int minContourLen = 60)
        {
            // 2) Tìm contour và lọc theo độ dài chu vi
            var contours = Cv2.FindContoursAsArray(edges, RetrievalModes.List, ContourApproximationModes.ApproxNone);

            var outBin = new Mat(edges.Size(), MatType.CV_8UC1, Scalar.Black);
            foreach (var c in contours)
            {
                double len = Cv2.ArcLength(c, false); // false vì biên sau Canny thường hở
                if (len >= minContourLen)
                {
                    // Vẽ lại contour đạt ngưỡng
                    Cv2.Polylines(outBin, new[] { c }, isClosed: false, color: Scalar.White, thickness: 1);
                }
            }

            // 3) (tuỳ chọn) Skeletonize đơn giản để mảnh 1 px (không cần XImgProc)
            //   Bỏ nếu bạn muốn giữ đúng bề dày do Canny tạo ra.
            //return Skeletonize(outBin);
            return outBin;
           
        }


        public static Mat ClearNoise( Mat edges,int minCompArea=1000)
        {
            Mat labels = new Mat(), stats = new Mat(), centroids = new Mat();
            try
            {
                // 4) Xóa nhiễu bằng Connected Components (trên ảnh nhị phân edge)
              
                int num = Cv2.ConnectedComponentsWithStats(edges, labels, stats, centroids, PixelConnectivity.Connectivity8, MatType.CV_32S);
                var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
                Mat clean = Mat.Zeros(edges.Size(), MatType.CV_8U);
                for (int i = 1; i < num; i++)
                {
                    int area = stats.At<int>(i, (int)ConnectedComponentsTypes.Area);
                    if (area >= minCompArea)
                    {
                        using (Mat mask = new Mat())
                        {
                            Cv2.InRange(labels, i, i, mask); // giữ lại đúng nhãn i
                            clean.SetTo(255, mask);
                        }
                    }
                }
                return clean;
            }
            finally
            {
                // Giải phóng bộ nhớ trung gian
                labels.Dispose();
                stats.Dispose();
                centroids.Dispose();
            }
            // (Tùy chọn) mở nhẹ để mảnh hơn
            //Cv2.MorphologyEx(clean, clean, MorphTypes.Open, kernel, iterations: 1);
            
        }
        // true = erode, false = dilate
        public static ImageFilter ErodeDilate(bool erode, Size ksize, int iterations = 1) =>
            delegate (Mat src, Mat dst)
            {
                Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, ksize);
                if (erode) Cv2.Erode(src, dst, kernel, iterations: iterations);
                else Cv2.Dilate(src, dst, kernel, iterations: iterations);
                kernel.Dispose();
            };

        // ==== 6. WHITE BALANCE ====

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

        // ==== 7. LỌC / GIỮ MÀU HSV ====

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

        // ==== 8. CHUYỂN XÁM & RESIZE ====

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
