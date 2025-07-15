using BeeCore.Algorithm;
using BeeCore.Funtion;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class Circle
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsIni = false;
        public int Index = -1;
        public TypeTool TypeTool=TypeTool.Circle;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp,matMask;
        public List<Point> Postion=new List<Point>();
     
        public List<double> listScore = new List<double>();
      
        int _NumObject = 0;
        public int NumObject
        {
            get
            {
                return _NumObject;
            }
            set
            {
                _NumObject = value;
               
            }
        }
         bool isHighSpeed=false;
        public bool IsHighSpeed {
            get
            {
                return isHighSpeed;
            }
            set
            {
                isHighSpeed = value;
              
            }
        }

        public TypeCrop TypeCrop;
        public string pathRaw = "";
        public int cycleTime = 0;
        public RectangleF rectArea;
        public Compares Compare = Compares.Equal;
        public int LimitCounter = 0;
        public bool IsOK = false;
        public bool IsAreaWhite=false;
        public int ScoreRs = 0;
        int _threshMin;
        public int Cany = 100;
       
        
        int _maxCount=9;
        public int MaxCount
        {
            get
            {
                return _maxCount;
            }
            set
            {
                _maxCount = value;
            
            }
        }
        int _minArea;
        public int minArea
        {
            get
            {
                return _minArea;
            }
            set
            {
                _minArea = value;
                G.pattern.m_iMinReduceArea = _minArea;
            }
        }
        double _OverLap;
        public double OverLap
        {
            get
            {
                return _OverLap;
            }
            set
            {
                _OverLap = value;
             
            }
        }

        public bool IsProcess
        {
            get
            {
                return G.pattern.IsProcess;
            }
            set
            {
                G.pattern.IsProcess = value;
            }
        }
        bool _ckSIMD=true;
        public bool ckSIMD
        {
            get
            {
                return _ckSIMD;
            }
            set
            {
                _ckSIMD = value;
                
            }
        }
        bool _ckBitwiseNot;
        public bool ckBitwiseNot
        {
            get
            {
                return _ckBitwiseNot;
            }
            set
            {
                _ckBitwiseNot = value;
              
            }
        }
        bool _ckSubPixel = true;
        public bool ckSubPixel
        {
            get
            {
                return _ckSubPixel;
            }
            set
            {
                _ckSubPixel = value;
              
            }
        }
        private bool isAutoTrig;
        private int numOK;
        private int delayTrig;
        public List< System.Drawing.Point > listP_Center=new List<System.Drawing.Point>();
        public Circle()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
       
        
        int getMaxAreaContourId(OpenCvSharp.Point[][] contours)
        {
            double maxArea = 0;
            int maxAreaContourId = -1;
            for (int j = 0; j < contours.Count(); j++)
            {
                double newArea = Cv2.ContourArea(contours[j]);
                if (newArea > maxArea)
                {
                    maxArea = newArea;
                    maxAreaContourId = j;
                } // End if
            } // End for
            return maxAreaContourId;
        }
      
      
        private int _score = 70;
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
              
            }
        }
        public int numTempOK;
        public bool IsAutoTrig { get => isAutoTrig; set => isAutoTrig = value; }
        public int NumOK { get => numOK; set => numOK = value; }
        public int DelayTrig { get => delayTrig; set => delayTrig = value; }


        //IsProcess,Convert.ToBoolean((int) TypeMode)
        public List<RectRotate> rectRotates = new List<RectRotate>();
    
        public String nameTool = "";
        public StatusTool StatusTool = StatusTool.None;
        public bool IsLimitCouter = true;
        public void DoWork( RectRotate rectRotate)
        {
            StatusTool = StatusTool.Processing;
            Matching( rectRotate);

        }
        public void Complete()
        {
            IsOK = true;
            //switch (Compare)
            //{
            //    case Compares.Equal:
            //        if (rectRotates.Count() != LimitCounter)
            //            IsOK = false;
            //        break;
            //    case Compares.Less:
            //        if (rectRotates.Count() >= LimitCounter)
            //            IsOK = false;
            //        break;
            //    case Compares.More:
            //        if (rectRotates.Count() <= LimitCounter)
            //            IsOK = false;
            //        break;
            //}
            StatusTool = StatusTool.Done;

        }
        public float Dp = 1.2f;
        public int MinRadius = 0;
        public int MaxRadius = 0;
        public int Distance = 0;
        struct Circle2
        {
            public double Cx, Cy, Radius;
            public Circle2(double cx, double cy, double r) { Cx = cx; Cy = cy; Radius = r; }
        }
        public Mat Preprocess(Mat input, int clipLimit = 2, int sigma = 3, int blur = 3)
        {
            // 1. Chuyển sang grayscale nếu cần
            Mat gray = new Mat();
            if (input.Channels() == 3)
                Cv2.CvtColor(input, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = input.Clone();

            // 2. Tăng tương phản bằng CLAHE
            CLAHE clahe = Cv2.CreateCLAHE(clipLimit, new Size(8, 8));
            Mat contrast = new Mat();
            clahe.Apply(gray, contrast);

            // 3. Làm sắc nét bằng Unsharp Mask
            Mat blurred = new Mat();
            Cv2.GaussianBlur(contrast, blurred, new Size(0, 0), sigma);
            Mat sharp = new Mat();
            Cv2.AddWeighted(contrast, 1.5, blurred, -0.5, 0, sharp);

            // 4. Chuyển sang đen trắng rõ ràng bằng Otsu threshold
            Mat binary = new Mat();
          //   Cv2.Threshold(sharp, binary, 220, 255, ThresholdTypes.Binary );

            // 5. Lọc nhiễu bằng MedianBlur
            Mat clean = new Mat();
            if (blur % 2 == 0) blur += 1;  // Chuyển thành số lẻ nếu chẵn
            if (blur < 3) blur = 3;        // Tối thiểu là 3
            Cv2.MedianBlur(sharp, clean, blur);
            if (Common.IsDebug)
                Cv2.ImWrite(nameTool + ".png", clean);
            return clean;
        }
        static void Main(string[] args)
        {
        

          
        }

        // ---------- Sobel + gradient magnitude ----------
        static Mat EdgeBySobel(Mat gray)
        {
            Mat dx = new Mat(), dy = new Mat(), mag = new Mat();
            Cv2.Sobel(gray, dx, MatType.CV_32F, 1, 0, 3);
            Cv2.Sobel(gray, dy, MatType.CV_32F, 0, 1, 3);
            Cv2.Magnitude(dx, dy, mag);

            Cv2.Normalize(mag, mag, 0, 255, NormTypes.MinMax);
            mag.ConvertTo(mag, MatType.CV_8U);
            Cv2.Threshold(mag, mag, 60, 255, ThresholdTypes.Binary);

            Mat k = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3));
            Cv2.Dilate(mag, mag, k, iterations: 1);
            Cv2.MorphologyEx(mag, mag, MorphTypes.Close, k, iterations: 1);
            return mag;
        }

        // ---------- Laplacian of Gaussian ----------
        static Mat EdgeByLoG(Mat gray)
        {
            Mat blur = new Mat();
            Cv2.GaussianBlur(gray, blur, new Size(7, 7), 1.2);

            Mat log = new Mat();
            Cv2.Laplacian(blur, log, MatType.CV_32F, 3);
            Cv2.Absdiff(log, new Mat(log.Size(), log.Type(), Scalar.All(0)), log);
            Cv2.Normalize(log, log, 0, 255, NormTypes.MinMax);
            log.ConvertTo(log, MatType.CV_8U);
            Cv2.Threshold(log, log, 40, 255, ThresholdTypes.Binary);

            Mat k = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3));
            Cv2.MorphologyEx(log, log, MorphTypes.Close, k, iterations: 1);
            return log;
        }

        // ---------- RANSAC + refit ----------
        static Circle2? RansacCircle(Point2f[] pts, int iters, double thresh, double minRatio)
        {
            if (pts.Length < 3) return null;
            var rand = new Random();
            Circle2? best = null; int bestInliers = 0;

            for (int i = 0; i < iters; i++)
            {
                int[] idx = Enumerable.Range(0, pts.Length)
                                      .OrderBy(_ => rand.Next())
                                      .Take(3).ToArray();
                if (!TryCircleFrom3(pts[idx[0]], pts[idx[1]], pts[idx[2]],
                                    out double cx, out double cy, out double r))
                    continue;

                int inliers = pts.Count(p =>
                    Math.Abs(Math.Sqrt(Math.Pow(p.X - cx, 2) + Math.Pow(p.Y - cy, 2)) - r) < thresh);

                if (inliers > bestInliers)
                {
                    bestInliers = inliers;
                    best = new Circle2(cx, cy, r);
                }
            }

            if (best == null || bestInliers < minRatio * pts.Length) return null;

            // ---------- refit least-squares trên inliers ----------
            var inlierPts = pts.Where(p =>
                Math.Abs(Math.Sqrt(Math.Pow(p.X - best.Value.Cx, 2) + Math.Pow(p.Y - best.Value.Cy, 2)) - best.Value.Radius) < thresh)
                .ToArray();

            int n = inlierPts.Length;
            var A = new Mat(n, 3, MatType.CV_64F);
            var b = new Mat(n, 1, MatType.CV_64F);
            for (int i = 0; i < n; i++)
            {
                double x = inlierPts[i].X, y = inlierPts[i].Y;
                A.Set<double>(i, 0, 2 * x);
                A.Set<double>(i, 1, 2 * y);
                A.Set<double>(i, 2, 1);
                b.Set<double>(i, 0, x * x + y * y);
            }
            var xVec = new Mat();
            Cv2.Solve(A, b, xVec, DecompTypes.Normal);

            double cx2 = xVec.Get<double>(0), cy2 = xVec.Get<double>(1), c = xVec.Get<double>(2);
            double r2 = Math.Sqrt(cx2 * cx2 + cy2 * cy2 + c);

            return new Circle2(cx2, cy2, r2);
        }

        // ---------- tính tâm & bán kính từ 3 điểm ----------
        static bool TryCircleFrom3(Point2f p1, Point2f p2, Point2f p3,
                                   out double cx, out double cy, out double r)
        {
            double x1 = p1.X, y1 = p1.Y;
            double x2 = p2.X, y2 = p2.Y;
            double x3 = p3.X, y3 = p3.Y;

            double a = x1 * (y2 - y3) - y1 * (x2 - x3) + x2 * y3 - x3 * y2;
            if (Math.Abs(a) < 1e-6) { cx = cy = r = 0; return false; }

            double b = (x1 * x1 + y1 * y1) * (y3 - y2) +
                       (x2 * x2 + y2 * y2) * (y1 - y3) +
                       (x3 * x3 + y3 * y3) * (y2 - y1);
            double c = (x1 * x1 + y1 * y1) * (x2 - x3) +
                       (x2 * x2 + y2 * y2) * (x3 - x1) +
                       (x3 * x3 + y3 * y3) * (x1 - x2);

            cx = -b / (2 * a);
            cy = -c / (2 * a);
            r = Math.Sqrt((cx - x1) * (cx - x1) + (cy - y1) * (cy - y1));
            return true;
        }
        public void SetModel()
        {
            StatusTool = StatusTool.Initialed;
        }
        public float Scale = 1;
        public int IndexThread = 0;
        public void Matching( RectRotate rectRotate)
        {
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {

                if (raw.Empty()) return;

                Mat matCrop = Common.CropRotatedRect(raw, rectRotate, rotMask);
                Mat matProcess = new Mat();
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matProcess, ColorConversionCodes.BGR2GRAY);
                else
                    matProcess = matCrop;
          
                 ScoreRs =100;
           //      var pipeline = new ImagePreprocessPipeline()
           //.Add(Filters.Clahe(2.0, new Size(8, 8)))            // làm phẳng sáng cục bộ
           //.Add(Filters.GaussianBlur(new Size(5, 5), 0))        // giảm nhiễu
           //.Add(Filters.Canny(50, 150))                         // trích biên
           //.Add(Filters.Morph(MorphTypes.Close, new Size(3, 3))); // nối nét đứt

           //     Mat pre = pipeline.Apply(matProcess);

                //Cv2.WaitKey();
               // matProcess = EdgeBySobel(matProcess);
                //matProcess = Preprocess(matProcess,5,5,3);
                rectRotates = new List<RectRotate>();
                // BƯỚC 2 – HoughCircles
                // dp      = 1.2   (tăng nhẹ để giảm false‑positive)
                // minDist = gray.Rows / 8  (khoảng cách tối thiểu giữa tâm hai hình tròn)
                // param1  = 100   (ngưỡng Canny high threshold)
                // param2  = 45    (ngưỡng tích lũy Hough – càng thấp càng nhạy)
                // min/maxRadius = 0 -> để Hough tự ước lượng
                listScore = new List<double>();
                listP_Center = new List<System.Drawing.Point>();
                float ScoreCircle = (float)(Score / 100.0);
                CircleSegment[] circles = Cv2.HoughCircles(
                    matProcess,
                    HoughModes.GradientAlt,
                    dp: Dp,
                    minDist: Distance,
                    param1: Cany,
                    param2: ScoreCircle,
                    minRadius: MinRadius,
                    maxRadius: MaxRadius);
                //foreach (var c in circles)
                //{

                //    Cv2.Circle(matProcess, (Point)c.Center, (int)c.Radius, Scalar.Red, 3);
                //    Cv2.Circle(matProcess, (Point)c.Center, 2, Scalar.Blue, -1);
                //}
               // Cv2.ImWrite("Circle.png", matProcess);
                foreach (var c in circles)
                {
                    PointF pCenter = new PointF(Convert.ToSingle(c.Center.X), Convert.ToSingle(c.Center.Y));
                    float angle = 0;
                    float width = Convert.ToSingle(c.Radius * 2);
                    float height = Convert.ToSingle(c.Radius * 2);
                    float Score = Convert.ToSingle(100);
                    rectRotates.Add(new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None, false));
                    listScore.Add(Math.Round(Score, 1));
                    listP_Center.Add(new System.Drawing.Point((int)rotAreaAdjustment._PosCenter.X - (int)rotAreaAdjustment._rect.Width / 2 + (int)pCenter.X, (int)rotAreaAdjustment._PosCenter.Y - (int)rotAreaAdjustment._rect.Height / 2 + (int)pCenter.Y));
                }


                matProcess.Dispose();
                matCrop.Dispose();

            }

        }
    }
}
