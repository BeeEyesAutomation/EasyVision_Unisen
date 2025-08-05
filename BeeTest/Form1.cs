
using BeeCore.Algorithm;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Size = OpenCvSharp.Size;

namespace BeeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Mat LoadAndPreprocess(Mat raw, out Mat edges)
        {
            // 1) Chuyển sang grayscale
           Mat  gray = new Mat();
            Cv2.CvtColor(raw, gray, ColorConversionCodes.BGR2GRAY);

            // 2) CLAHE (adaptive histogram equalization) để tăng tương phản
            var clahe = Cv2.CreateCLAHE(clipLimit: 2.0, tileGridSize: new Size(8, 8));
            clahe.Apply(gray, gray);

            // 3) Giảm nhiễu nhưng vẫn giữ cạnh: bilateral filter
            var smooth = new Mat();
            Cv2.BilateralFilter(gray, smooth, d: 9, sigmaColor: 75, sigmaSpace: 75);

            // 4) Tự động tính ngưỡng Canny dựa trên median của ảnh
            int total = gray.Rows * gray.Cols;
            var pixelData = new byte[total];
            System.Runtime.InteropServices.Marshal.Copy(gray.Data, pixelData, 0, total);
            Array.Sort(pixelData);
            double median = pixelData[total / 2];
            double sigma = 0.33;
            int lower = (int)Math.Max(0, (1.0 - sigma) * median);
            int upper = (int)Math.Min(255, (1.0 + sigma) * median);

            // 5) Dò Canny
            edges = new Mat();
            Cv2.Canny(smooth, edges, lower, upper);

            // 6) Morphological closing để nối đoạn đứt + dilate/erode để làm dày mỏng cạnh
            var kernelClose = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
            Cv2.MorphologyEx(edges, edges, MorphTypes.Close, kernelClose);
            var kernelDil = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.Dilate(edges, edges, kernelDil, iterations: 1);
            Cv2.Erode(edges, edges, kernelDil, iterations: 1);

            // Debug: hiển thị kết quả
            Cv2.ImShow("Edges (auto-optimized)", edges);

            // Trả về raw để vẽ annotate lên
            return raw;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var src = Cv2.ImRead("c2.bmp", ImreadModes.Color);


            Mat edge = new Mat();
            LoadAndPreprocess(src,out edge);
         //   Cv2.Canny(gray, edge, 100, 200);

            //var circles = RansacCircleFitter.DetectCircles(
            //    edge,
            //    maxCircles: 2,
            //   iterations: 500,
            //   threshold: 0.5f,
            //   minInliers:200,
            //    direction: CircleScanDirection.InsideOut
               
            //);

            //foreach (var (center, radius) in circles)
            //{
            //    Cv2.Circle(src, (OpenCvSharp.Point)center, (int)radius, Scalar.Red, 2);
            //}

            Cv2.ImShow("Detected Circles", src);
            Cv2.WaitKey();
            //var detector = new ParallelGapDetector(mmPerPixel: 0.05);
            //if (!File.Exists("raw4.bmp")) return;
            //Mat raw = Cv2.ImRead("raw4.bmp");
            //// Đo khe song song
            //var lineResult = detector.MeasureParallelGap(raw,6,ParallelGapDetector.GapExtremum.Outermost,ParallelGapDetector.LineOrientation.Horizontal);
            //Console.WriteLine($"Parallel gap: {lineResult.GapMm:F3} mm");
            //Cv2.ImShow("ParallelGap", lineResult.AnnotatedImage);

            //////// Đo hai điểm đỉnh
            ////var vertResult = detector.MeasureTipGap("raw3.bmp");
            ////Console.WriteLine($"Vertex gap: {vertResult.GapMm:F3} mm");
            ////Cv2.ImShow("VertexGap", vertResult.AnnotatedImage);

            //Cv2.WaitKey();
        }
    }
}
