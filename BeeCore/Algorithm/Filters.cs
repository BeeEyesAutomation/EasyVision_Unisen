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
        public static Mat RemoveWhiteNoiseThenEdge(Mat src)
        {
            Mat gray = new Mat();
            if (src.Channels() == 3)
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = src.Clone();

            // 1) Blur nhẹ để đỡ noise lấm tấm
            Mat blur = new Mat();
            Cv2.GaussianBlur(gray, blur, new Size(3, 3), 0);

            // 2) Tách phần sáng trắng nhỏ / phản sáng
            // kernel nhỏ nếu noise nhỏ, tăng lên nếu vệt trắng to hơn
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
            Mat topHat = new Mat();
            Cv2.MorphologyEx(blur, topHat, MorphTypes.TopHat, kernel);

            // 3) Trừ phần sáng đó khỏi ảnh gốc
            Mat noWhite = new Mat();
            Cv2.Subtract(blur, topHat, noWhite);

            // 4) Làm mượt thêm chút trước khi bắt cạnh
            Mat smooth = new Mat();
            Cv2.GaussianBlur(noWhite, smooth, new Size(5, 5), 1.0);
            return smooth;
        }
        public static Mat RemoveVerticalGlare(Mat src)
        {
            Mat gray = new Mat();
            if (src.Channels() == 3)
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = src.Clone();

            // lấy riêng các cấu trúc sáng mảnh theo chiều dọc
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 17));
            Mat topHat = new Mat();
            Cv2.MorphologyEx(gray, topHat, MorphTypes.TopHat, kernel);

            // chỉ lấy phần topHat đủ mạnh mới coi là glare
            Mat glareMask = new Mat();
            Cv2.Threshold(topHat, glareMask, 25, 255, ThresholdTypes.Binary);

            Mat repaired = new Mat();
            Cv2.Inpaint(gray, glareMask, repaired, 3, InpaintMethod.Telea);

            kernel.Dispose();
            topHat.Dispose();
            glareMask.Dispose();
            gray.Dispose();

            return repaired;
        }

        public static Mat SuppressHighlightOnly(Mat src)
        {
            Mat gray = new Mat();
            if (src.Channels() == 3)
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = src.Clone();

            Mat mask = new Mat();
            Cv2.Threshold(gray, mask, 180, 255, ThresholdTypes.Binary);

            Mat result = gray.Clone();

            // vùng sáng chói bị hạ xuống 180
            result.SetTo(new Scalar(80), mask);

            mask.Dispose();
            gray.Dispose();

            return result;
        }

        public static Mat SuppressHighlight(Mat src, int blurSize = 51, int delta = 18)
    {
        Mat gray = new Mat();
        if (src.Channels() == 3)
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
        else
            gray = src.Clone();

        // blur lớn để lấy nền sáng chậm, không phải để blur ảnh output
        Mat bg = new Mat();
        Cv2.GaussianBlur(gray, bg, new Size(blurSize, blurSize), 0);

        Mat result = gray.Clone();

        int rows = gray.Rows;
        int cols = gray.Cols;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                byte g = gray.At<byte>(y, x);
                byte b = bg.At<byte>(y, x);

                int limit = b + delta; // sáng hơn nền quá delta thì hạ xuống
                if (g > limit)
                    result.Set(y, x, (byte)limit);
            }
        }

        bg.Dispose();
        gray.Dispose();
        return result;
    }
    public static Mat BinaryTextAuto(Mat src, int blockSize = 31, int c = 8, int minArea = 20)
        {
            Mat gray = new Mat();

            if (src.Channels() == 3)
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = src.Clone();

            // blur nhẹ giảm noise
            Cv2.GaussianBlur(gray, gray, new Size(3, 3), 0);

            // adaptive threshold
            Mat binInv = new Mat();
            Cv2.AdaptiveThreshold(
                gray,
                binInv,
                255,
                AdaptiveThresholdTypes.GaussianC,
                ThresholdTypes.BinaryInv,
                blockSize,
                c
            );

            // text=0 background=255
            Mat bin = new Mat();
            Cv2.BitwiseNot(binInv, bin);

            // remove small noise
         Mat clean = RemoveSmallBlackDots(bin, minArea);

            gray.Dispose();
            binInv.Dispose();
           bin.Dispose();

            return clean;
        }
        public static Mat FillInnerHolesOnly(Mat binTextBlack, double maxHoleArea = 5000)
        {
            if (binTextBlack == null || binTextBlack.Empty())
                return null;

            Mat result = binTextBlack.Clone();
            Mat inv = new Mat();

            try
            {
                // Đảo ảnh: chữ trắng, nền đen
                Cv2.BitwiseNot(binTextBlack, inv);

                Point[][] contours;
                HierarchyIndex[] hierarchy;

                // RETR_CCOMP hoặc RETR_TREE đều được
                Cv2.FindContours(
                    inv,
                    out contours,
                    out hierarchy,
                    RetrievalModes.CComp,
                    ContourApproximationModes.ApproxSimple
                );

                if (contours == null || contours.Length == 0 || hierarchy == null)
                    return result;

                for (int i = 0; i < contours.Length; i++)
                {
                    // parent >= 0 nghĩa là contour này là contour con
                    // trong ảnh inv, contour con thường chính là hole bên trong vùng trắng
                    if (hierarchy[i].Parent >= 0)
                    {
                        double area = System.Math.Abs(Cv2.ContourArea(contours[i]));

                        // lọc diện tích để tránh fill nhầm vùng lớn
                        if (area > 0 && area <= maxHoleArea)
                        {
                            // Fill contour hole thành đen trên ảnh gốc
                            Cv2.DrawContours(result, contours, i, Scalar.Black, -1);
                        }
                    }
                }

                return result;
            }
            finally
            {
                inv.Dispose();
            }
        }

        public static unsafe Mat RemoveSmallBlackDots(Mat binTextBlack, int minArea)
        {
            if (binTextBlack == null || binTextBlack.Empty())
                return new Mat();

            if (binTextBlack.Type() != MatType.CV_8UC1)
                throw new ArgumentException("binTextBlack must be CV_8UC1");

            using (Mat inv = new Mat())
            using (Mat labels = new Mat())
            using (Mat stats = new Mat())
            using (Mat centroids = new Mat())
            {
                Cv2.BitwiseNot(binTextBlack, inv);

                int nLabels = Cv2.ConnectedComponentsWithStats(
                    inv,
                    labels,
                    stats,
                    centroids,
                    PixelConnectivity.Connectivity8,
                    MatType.CV_32S
                );

                byte[] keep = new byte[nLabels];
                keep[0] = 0;

                for (int i = 1; i < nLabels; i++)
                {
                    int area = stats.Get<int>(i, (int)ConnectedComponentsTypes.Area);
                    if (area >= minArea)
                        keep[i] = 1;
                }

                Mat result = new Mat(binTextBlack.Size(), MatType.CV_8UC1, Scalar.All(255));

                int rows = labels.Rows;
                int cols = labels.Cols;

                for (int y = 0; y < rows; y++)
                {
                    int* pLabel = (int*)labels.Ptr(y).ToPointer();
                    byte* pDst = (byte*)result.Ptr(y).ToPointer();

                    for (int x = 0; x < cols; x++)
                    {
                        int lb = pLabel[x];
                        pDst[x] = (lb > 0 && keep[lb] != 0) ? (byte)0 : (byte)255;
                    }
                }

                return result;
            }
        }
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
        public static Mat EdgeForCenterline(Mat src)
        {
            Mat gray = new Mat();

            if (src.Channels() == 3)
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = src.Clone();

            Mat blur = new Mat();
            Cv2.GaussianBlur(gray, blur, new Size(3, 3), 0.8);

            Mat gx = new Mat();
            Mat gy = new Mat();

            Cv2.Scharr(blur, gx, MatType.CV_16S, 1, 0);
            Cv2.Scharr(blur, gy, MatType.CV_16S, 0, 1);

            Cv2.ConvertScaleAbs(gx, gx);
            Cv2.ConvertScaleAbs(gy, gy);

            Mat grad = new Mat();
            Cv2.Add(gx, gy, grad);

            Mat bin = new Mat();
            Cv2.Threshold(grad, bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.MorphologyEx(bin, bin, MorphTypes.Close, kernel);

            return bin;
        }
        public static List<Point2f> ExtractCenterLineHorizontal(Mat edge)
        {
            List<Point2f> pts = new List<Point2f>();

            int rows = edge.Rows;
            int cols = edge.Cols;

            for (int x = 0; x < cols; x++)
            {
                int yTop = -1;
                int yBottom = -1;

                for (int y = 0; y < rows; y++)
                {
                    if (edge.At<byte>(y, x) > 0)
                    {
                        if (yTop < 0) yTop = y;
                        yBottom = y;
                    }
                }

                if (yTop >= 0)
                {
                    float yc = (yTop + yBottom) * 0.5f;
                    pts.Add(new Point2f(x, yc));
                }
            }

            return pts;
        }
        public static List<Point2f> ExtractCenterLineVertical(Mat edge)
        {
            List<Point2f> pts = new List<Point2f>();

            int rows = edge.Rows;
            int cols = edge.Cols;

            for (int y = 0; y < rows; y++)
            {
                int xLeft = -1;
                int xRight = -1;

                for (int x = 0; x < cols; x++)
                {
                    if (edge.At<byte>(y, x) > 0)
                    {
                        if (xLeft < 0) xLeft = x;
                        xRight = x;
                    }
                }

                if (xLeft >= 0)
                {
                    float xc = (xLeft + xRight) * 0.5f;
                    pts.Add(new Point2f(xc, y));
                }
            }

            return pts;
        }
        public static Mat GetUltraThinEdgesFast(Mat src, double percentile = 0.995)
        {
            Mat gray = new Mat();

            if (src.Channels() == 3)
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = src.Clone();

            // 1. Blur nhẹ
            Mat blur = new Mat();
            Cv2.GaussianBlur(gray, blur, new Size(3, 3), 0.8);

            // 2. Morphological Gradient (edge rất nhanh)
            Mat grad = new Mat();
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.MorphologyEx(blur, grad, MorphTypes.Gradient, kernel);

            // 3. Percentile threshold
            byte[] data = new byte[grad.Rows * grad.Cols];
            grad.GetArray(out data);

            Array.Sort(data);
            int idx = (int)(data.Length * percentile);
            idx = Math.Max(0, Math.Min(idx, data.Length - 1));
            byte th = data[idx];
            Mat bin = new Mat();
            Cv2.Threshold(grad, bin, th, 255, ThresholdTypes.Binary);

            // 4. làm mỏng cạnh
            Cv2.Erode(bin, bin, kernel, iterations: 1);

            // 5. remove noise nhỏ
            Cv2.MorphologyEx(bin, bin, MorphTypes.Close, kernel);
            Cv2.MedianBlur(bin, bin, 3);
            return bin;
        }
        public static Mat EdgeAnyAngleFast(Mat src)
        {
            Mat gray;

            if (src.Channels() == 3)
            {
                gray = new Mat();
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            }
            else
            {
                gray = src;
            }

            Mat blur = new Mat();
            Cv2.GaussianBlur(gray, blur, new Size(5, 5), 0);

            Mat gx = new Mat();
            Mat gy = new Mat();

            // Scharr cho gradient tốt hơn Sobel
            Cv2.Scharr(blur, gx, MatType.CV_16S, 1, 0);
            Cv2.Scharr(blur, gy, MatType.CV_16S, 0, 1);

            Cv2.ConvertScaleAbs(gx, gx);
            Cv2.ConvertScaleAbs(gy, gy);

            Mat grad = new Mat();
            Cv2.Add(gx, gy, grad);   // nhanh hơn AddWeighted

            Mat bin = new Mat();
            Cv2.Threshold(grad, bin, 40, 255, ThresholdTypes.Binary);

            // remove noise nhỏ
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.MorphologyEx(bin, bin, MorphTypes.Open, kernel);

            return bin;
        }
        public static String Err = "";
        public static Mat EdgeAnyAngle(Mat src)
        {
            Mat gray = new Mat();
            Mat blur = new Mat();
            Mat gx = new Mat();
            Mat gy = new Mat();
            Mat mag = new Mat();
            Mat bin = new Mat();

            if (src.Channels() == 3)
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = src.Clone();

            Cv2.GaussianBlur(gray, blur, new Size(5, 5), 0);

            Cv2.Sobel(blur, gx, MatType.CV_16S, 1, 0);
            Cv2.Sobel(blur, gy, MatType.CV_16S, 0, 1);

            Cv2.ConvertScaleAbs(gx, gx);
            Cv2.ConvertScaleAbs(gy, gy);

            Cv2.AddWeighted(gx, 0.5, gy, 0.5, 0, mag);

            Cv2.Threshold(mag, bin, 40, 255, ThresholdTypes.Binary);

            return bin;
        }
        public static Mat GetStrongEdgesStable(Mat raw)
        {
            Mat gray = new Mat();
            Mat blur = new Mat();
            Mat grad = new Mat();
            Mat bin = new Mat();

            if (raw.Channels() == 3)
                Cv2.CvtColor(raw, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = raw.Clone();

            Cv2.GaussianBlur(gray, blur, new Size(5, 5), 0);

            Mat gx = new Mat();
            Mat gy = new Mat();

            Cv2.Sobel(blur, gx, MatType.CV_16S, 1, 0);
            Cv2.Sobel(blur, gy, MatType.CV_16S, 0, 1);

            Cv2.ConvertScaleAbs(gx, gx);
            Cv2.ConvertScaleAbs(gy, gy);

            Cv2.AddWeighted(gx, 0.5, gy, 0.5, 0, grad);

            Cv2.Threshold(grad, bin, 0, 255, ThresholdTypes.Otsu);

            return bin;
        }
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

        public static Mat ClearNoiseBig(Mat edges, int minCompArea = 1000)
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
                    if (area < minCompArea)
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
