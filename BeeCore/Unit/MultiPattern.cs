using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;

using BeeGlobal;
using CvPlus;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using Python.Runtime;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Markup;
using static BeeCore.Cropper;
using static LibUsbDotNet.Main.UsbTransferQueue;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Point = OpenCvSharp.Point;
using ShapeType = BeeGlobal.ShapeType;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class MultiPattern
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public void ReSetAngle()
        {
            if (Angle > 360) Angle = 360;
            if (Angle == 0)
            {
                Angle = 1;
            }
            float angle = (rotCrop._rectRotation) - (rotArea._rectRotation);
            AngleLower = angle - Angle;
            AngleUper = angle + Angle;
        }
        public bool IsIni = false;
        public int Index = -1;

        public int ValueCompare = 0;
        public bool IsCalibs = false;
        public RectRotate rotArea, rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public int ThresholdBinary;
        public double RansacThreshold = 2.0; // px
        public int RansacIterations = 200;
        public bool IsClose = false;
        public bool IsOpen = false;
        public bool IsClearNoiseBig = false;
        public bool IsClearNoiseSmall = false;
        public int SizeClearsmall = 1;
        public int SizeClearBig = 1;
        public int SizeClose = 1;
        public int SizeOpen = 1;
        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        [NonSerialized]
        public Mat matTemp;
        public List<Point> Postion = new List<Point>();
        private Mode _TypeMode = Mode.Pattern;
        public List<double> listScore = new List<double>();

        /// 
        ///
        [NonSerialized]
        public float pxRS;
        [NonSerialized]
        float OffsetX, OffsetY, OffsetAngle;
        public VisualMatch visualMatch = new VisualMatch();
        public float ScoreVisualMatch = 1;
        [NonSerialized]
        List<Mat> list_matColor = new List<Mat>();
        /// 
        /// 
        public Mode TypeMode
        {
            get
            {
                return _TypeMode;
            }
            set
            {
                _TypeMode = value;

            }
        }
        int _NumObject = 1;
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
        bool isHighSpeed = false;
        public bool IsHighSpeed
        {
            get
            {
                return isHighSpeed;
            }
            set
            {
                isHighSpeed = value;

            }
        }
        public Bitmap bmRaw;


        public TypeCrop TypeCrop;
        public string pathRaw = "";
        public int cycleTime = 0;
        public RectangleF rectArea;
        public Compares Compare = Compares.Equal;
        public int LimitCounter = 1;
        public bool IsSendResult;
        public bool IsNew;
        public async Task SendResult()
        {
            if (IsSendResult)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                    int i = 0;
                    int Add = (int)Converts.StringtoDouble(AddPLC);
                    String sAdd = Converts.BeforeFirstDigit(AddPLC);
                    foreach (System.Drawing.Point point in listP_Center)
                    {
                        String Address = sAdd + Add;
                        float[] floats = new float[4] { point.X, point.Y, list_AngleCenter[i], (float)listScore[i] };
                        await Global.Comunication.Protocol.WriteResultFloatArr(AddPLC, floats);
                        Add += 8;
                        i++;
                    }
                }
            }
        }
        public bool IsAreaWhite = false;

        int _threshMin;
        public int threshMin
        {
            get
            {
                return _threshMin;
            }
            set
            {
                _threshMin = value;

            }
        }
        int _threshMax;
        public int threshMax
        {
            get
            {
                return _threshMax;
            }
            set
            {
                _threshMax = value;

            }
        }
        private double _angle = 10;
        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
            }
        }
        double _AngleLower;
        public double AngleLower
        {
            get
            {
                return _AngleLower;
            }
            set
            {
                _AngleLower = value;

            }
        }
        double _AngleUper;
        public double AngleUper
        {
            get
            {
                return _AngleUper;
            }
            set
            {
                _AngleUper = value;

            }
        }
        int _maxCount = 9;
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
                //Pattern. m_iMinReduceArea = _minArea;
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


        bool _ckSIMD = true;
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
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        [NonSerialized]
        public BeeCpp.Pattern Pattern = new BeeCpp.Pattern();
        [NonSerialized]//note
        public List<BeeCpp.Pattern> list_Patterns = new List<BeeCpp.Pattern>();
        [NonSerialized]//note
        public List<BeeCpp.ColorPixel> list_ColorPixel = new List<BeeCpp.ColorPixel>();

        public bool IsColorPixel;
        public float ThreshColor = 1;
        public List<ResultMulti> ResultMulti = new List<ResultMulti>();//note
        public MultiPattern()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;

        }
        public int MaxObject = 1;
        public int StepAngle = 0;
        [NonSerialized]
        Mat matProcess = new Mat();
        public Mat LearnPattern(Mat raw, bool IsNoCrop)
        {
            using (Mat img = raw.Clone())
            {
                // Chuẩn hóa góc
                //if (rotCrop._rectRotation < 0)
                //    rotCrop._rectRotation += 360;

                // Chuẩn hóa kênh về BGR 3 kênh
                if (img.Channels() == 3)
                    Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
                else if (img.Channels() == 4)
                    Cv2.CvtColor(img, img, ColorConversionCodes.BGRA2GRAY);

                int w = 0, h = 0, s = 0, c = 0;
                IntPtr intpr = IntPtr.Zero;
                Mat mat = new Mat();
                if (TypeMode == Mode.Edge)
                {
                    using (Mat gray = img)
                    {

                        Mat matCrop = new Mat();
                        PatchCropContext ctx = new PatchCropContext();


                        matCrop = Cropper.CropOuterPatch(gray, rotCrop, out ctx);
                        if (matProcess == null) matProcess = new Mat();
                        if (!matProcess.IsDisposed)
                            if (!matProcess.Empty()) matProcess.Dispose();

                        switch (MethordEdge)
                        {
                            case MethordEdge.CloseEdges:
                                matProcess = Filters.Edge(matCrop);
                                break;
                            case MethordEdge.StrongEdges:
                                matProcess = Filters.GetStrongEdgesOnly(matCrop);
                                break;
                            case MethordEdge.Binary:
                                matProcess = Filters.Threshold(matCrop, ThresholdBinary, ThresholdTypes.Binary);
                                break;
                            case MethordEdge.InvertBinary:
                                matProcess = Filters.Threshold(matCrop, ThresholdBinary, ThresholdTypes.BinaryInv);
                                break;
                        }

                        matProcess = ApplyShapeMaskAndCompose(matProcess, ctx, rotCrop, rotMask, returnMaskOnly: false);
                    }
                    return matProcess;
                }

                try
                {
                    if (TypeMode == Mode.Edge)
                    {
                        Pattern.SetImgeSampleNoCrop(img.Data, img.Width, img.Height, (int)img.Step(), img.Channels());
                        Pattern.LearnPattern();
                    }
                    else
                    {


                        var rrCli = Converts.ToCli(rotCrop); // như ở reply trước
                        RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                        intpr = Pattern.SetImgeSample(img.Data, img.Width, img.Height, (int)img.Step(), img.Channels(), rrCli, rrMaskCli, IsNoCrop,
                                out w, out h, out s, out c);

                        if (intpr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                            return mat; // trả Mat rỗng
                        Pattern.LearnPattern();
                        // Map kênh trả về
                        MatType mt = c == 1 ? MatType.CV_8UC1
                                    : c == 3 ? MatType.CV_8UC3
                                    : MatType.CV_8UC4;

                        // Wrap con trỏ rồi copy/clone để sở hữu bộ nhớ managed
                        using (var m = new Mat(h, w, mt, intpr, s))
                        {
                            // CopyTo hoặc Clone đều OK; Clone gọn hơn:
                            mat = m.Clone();
                        }

                        // Giữ sống input đến sau khi native xong
                        GC.KeepAlive(img);
                    }

                }
                finally
                {
                    if (intpr != IntPtr.Zero)
                        Pattern.FreeBuffer(intpr); // rất quan trọng
                }

                return mat;
            }


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



        public int numTempOK;
        public bool IsAutoTrig { get => isAutoTrig; set => isAutoTrig = value; }
        public int NumOK { get => numOK; set => numOK = value; }
        public int DelayTrig { get => delayTrig; set => delayTrig = value; }

        public void SetModel()
        {
            if (Pattern == null)
            {
                Pattern = new BeeCpp.Pattern();

            }
            if (ExpandX == 0) ExpandX = 50;
            if (ExpandY == 0) ExpandY = 50;

            list_ColorPixel = new List<ColorPixel>();
            list_Patterns = new List<Pattern>();
            if (ResultMulti == null)
                ResultMulti = new List<ResultMulti>();
            for (int i = 0; i < ResultMulti.Count; i++)
            {
                list_Patterns.Add(new Pattern());

            }
            for (int i = 0; i < ResultMulti.Count; i++)
            {
                list_ColorPixel.Add(new ColorPixel());

            }
            Parallel.For(
                               0, ResultMulti.Count,
                               new ParallelOptions
                               {
                                   MaxDegreeOfParallelism = Math.Min(300, Environment.ProcessorCount)
                               },
                               i =>
                               {

                                   if (ResultMulti[i].BTemp != null)
                                   {
                                       using (Mat mat = ResultMulti[i].BTemp.ToMat())
                                       {
                                           if (!mat.Empty())
                                           {

                                               list_Patterns[i].SetImgeSampleNoCrop(
                                                     mat.Data,
                                                     mat.Width,
                                                     mat.Height,
                                                     (int)mat.Step(),
                                                     mat.Channels()

                                                 );

                                               list_Patterns[i].LearnPattern();
                                               Mat mat1 = new Mat();
                                               if (mat.Type() == MatType.CV_8UC1)
                                                   Cv2.CvtColor(mat, mat1, ColorConversionCodes.GRAY2BGR);

                                               list_ColorPixel[i].SetImgeSampleNoCrop(
                                                    mat1.Data,
                                                    mat1.Width,
                                                    mat1.Height,
                                                    (int)mat1.Step(),
                                                    mat1.Channels());
                                           }
                                       }

                                   }
                               }
                           );
            //Parallel.For(
            //                     0, ResultMulti.Count,
            //                     new ParallelOptions
            //                     {
            //                         MaxDegreeOfParallelism = Math.Min(3, Environment.ProcessorCount)
            //                     },
            //                     i =>
            //                     {

            //                         if (ResultMulti[i].BTempColor != null)
            //                         {
            //                             using (Mat mat = ResultMulti[i].BTempColor.ToMat())
            //                             {
            //                                // ResultMulti[i].matTempColor = mat.Clone();

            //                                 if (!mat.Empty())
            //                                 {try
            //                                     {
            //                                         list_ColorPixel[i].SetImgeSampleNoCrop(
            //                                         mat.Data,
            //                                         mat.Width,
            //                                         mat.Height,
            //                                         (int)mat.Step(),
            //                                         mat.Channels());
            //                                     }
            //                                     catch (Exception e) {

            //                                       };

            //                                 }
            //                             }
            //                         }
            //                     }
            //                 );

            if (Scale == 0) Scale = 1;
            if (rotCrop == null) rotCrop = new RectRotate();
            if (rotArea == null) rotArea = new RectRotate();
            if (bmRaw != null)
            {
                matTemp = bmRaw.ToMat();
                LearnPattern(matTemp, true);
            }
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;

            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            if (Common.PropetyTools[IndexThread][Index].Score == 0)
                Common.PropetyTools[IndexThread][Index].Score = 80;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        public List<RectRotate> rectRotates = new List<RectRotate>();

        public List<float> list_AngleCenter = new List<float>();
        public ZeroPos ZeroPos = ZeroPos.Zero;
        public float Scale = 1;
        public bool IsLimitCouter = true;
        public float ExpandX = 20, ExpandY = 20;
        public float LimitX = 1;
        public float LimitY = 1;

        [NonSerialized]
        Line2DCli LineBot = new Line2DCli();
        [NonSerialized]
        Line2DCli LineTop = new Line2DCli();
        [NonSerialized]
        Line2DCli LineLeft = new Line2DCli();
        [NonSerialized]
        Line2DCli LineRight = new Line2DCli();
        [NonSerialized]
        RectRotate rotBot, rotTop, rotLeft, rotRigth;
        [NonSerialized]
        Line2D line1, line2, line3, line4;
        RectRotate rectPage = new RectRotate();

        public int OffSetPage = -50;
        public RectRotate CheckPage(Mat raw)
        {
            LineDirectionMode lineDirectionMode = LineDirectionMode.Horizontal;
            OffSetPage = -20;
            int W = raw.Width;
            int H = raw.Height;
            int space = 300;

            rotBot = new RectRotate(new RectangleF(-space / 2f, -H / 4f, space, H / 2f), new PointF(W / 2, 3 * H / 4f), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotBot, null))
            {
                if (crop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(crop, crop, ColorConversionCodes.BGR2GRAY);
                Mat Edge = Filters.GetStrongEdgesOnly(crop);
                LineBot = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, BeeCpp.LineDirectionMode.Horizontal, 0, 40
                );

            }
            LineBot.X1 += rotBot._PosCenter.X + rotBot._rect.X; LineBot.Y1 += rotBot._PosCenter.Y + rotBot._rect.Y;
            LineBot.X2 += rotBot._PosCenter.X + rotBot._rect.X; LineBot.Y2 += rotBot._PosCenter.Y + rotBot._rect.Y;
            LineBot.X0 = LineBot.X1; LineBot.Y0 = LineBot.Y1;
            rotTop = new RectRotate(new RectangleF(-space / 2f, -H / 4f, space, H / 2f), new PointF(W / 2f, H / 4f), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotTop, null))
            {
                if (crop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(crop, crop, ColorConversionCodes.BGR2GRAY);
                Mat Edge = Filters.GetStrongEdgesOnly(crop);
                LineTop = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, BeeCpp.LineDirectionMode.Horizontal, 0, 40
                );

            }
            LineTop.X1 += rotTop._PosCenter.X + rotTop._rect.X; LineTop.Y1 += rotTop._PosCenter.Y + rotTop._rect.Y;
            LineTop.X2 += rotTop._PosCenter.X + rotTop._rect.X; LineTop.Y2 += rotTop._PosCenter.Y + rotTop._rect.Y;
            LineTop.X0 = LineTop.X1; LineTop.Y0 = LineTop.Y1;
            rotLeft = new RectRotate(new RectangleF(-W / 4f, -space / 2f, W / 2f, space), new PointF(W / 4f, H / 2), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotLeft, null))
            {
                if (crop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(crop, crop, ColorConversionCodes.BGR2GRAY);
                Mat Edge = Filters.GetStrongEdgesOnly(crop);
                LineLeft = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, BeeCpp.LineDirectionMode.Vertical, 0, 40
                );

            }
            LineLeft.X1 += rotLeft._PosCenter.X + rotLeft._rect.X; LineLeft.Y1 += rotLeft._PosCenter.Y + rotLeft._rect.Y;
            LineLeft.X2 += rotLeft._PosCenter.X + rotLeft._rect.X; LineLeft.Y2 += rotLeft._PosCenter.Y + rotLeft._rect.Y;
            LineLeft.X0 = LineLeft.X1; LineLeft.Y0 = LineLeft.Y1;
            rotRigth = new RectRotate(new RectangleF(-W / 4f, -space / 2f, W / 2f, space), new PointF(3 * W / 4f, H / 2), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotRigth, null))
            {
                if (crop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(crop, crop, ColorConversionCodes.BGR2GRAY);
                Mat Edge = Filters.GetStrongEdgesOnly(crop);
                LineRight = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, BeeCpp.LineDirectionMode.Vertical, 0, 40
                );

            }
            LineRight.X1 += rotRigth._PosCenter.X + rotRigth._rect.X; LineRight.Y1 += rotRigth._PosCenter.Y + rotRigth._rect.Y;
            LineRight.X2 += rotRigth._PosCenter.X + rotRigth._rect.X; LineRight.Y2 += rotRigth._PosCenter.Y + rotRigth._rect.Y;
            LineRight.X0 = LineRight.X1; LineRight.Y0 = LineRight.Y1;
            line1 = new Line2D(LineTop.Vx, LineTop.Vy, LineTop.X0, LineTop.Y0);
            line2 = new Line2D(LineBot.Vx, LineBot.Vy, LineBot.X0, LineBot.Y0);
            line3 = new Line2D(LineLeft.Vx, LineLeft.Vy, LineLeft.X0, LineLeft.Y0);
            line4 = new Line2D(LineRight.Vx, LineRight.Vy, LineRight.X0, LineRight.Y0);

            rectPage = InsertLine.CreateRectRotate(line1, line2, line3, line4);
            RectRotate rtReturn = rectPage.Clone();
            rtReturn.OffsetPixels(OffSetPage, OffSetPage);

            return rtReturn;
        }
        // Cv2.CvtColor(Edge, Edge, ColorConversionCodes.GRAY2BGR);
        // Cv2.Line(Edge, new Point((int)Line2DCli.X1, (int)Line2DCli.Y1), new Point((int)Line2DCli.X2, (int)Line2DCli.Y2), Scalar.Red, 2);
        // Cv2.ImWrite("Edge.png", Edge);
        //if (Line2DCli.Found)
        // line1 = new Line2D(Line2DCli.Vx, Line2DCli.Vx, Line2DCli.X0, Line2DCli.Y0);

        //   Mat gray = new Mat();
        //   //MonoSegParams p = new MonoSegParams
        //   //{
        //   //    useBlackHat = false,
        //   //    blackHatK = 31,
        //   //    openK = 3,
        //   //    closeK = 5
        //   //};
        //   MonoSegParams p = new MonoSegParams { bgBlurK = 51, openK = 2, closeK = 7, mode = 0, useBlackHat = false, blackHatK = 31 };

        //   Mat mask, score;
        ////   Cv2.BitwiseNot(raw,raw);
        //   int areaa = MonoSegmentation.SegmentLowContrastMono(raw, out gray, p, out score);

        // //  gray = Filters.Edge(raw);
        //   OpenCvSharp.Point[][] contours;
        //   HierarchyIndex[] hierarchyIndices;
        //   //Mat show = gray.Clone();
        //   //show = show.PyrDown();
        // //  Cv2.BitwiseNot(gray, gray);
        //  Cv2.ImWrite("raw.png", gray);
        //   Cv2.FindContours(gray, out contours, out hierarchyIndices, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

        //   double AreaMax = 0;
        //   int indexMax = 0;
        //   for (int i = 0; i < contours.Length; i++)
        //   {
        //       double area = Cv2.ContourArea(contours[i]);
        //       if (area >=( gray.Width * gray.Height*0.95))
        //           continue;
        //       if (area > AreaMax)
        //       {
        //           AreaMax = area;
        //           indexMax = i;
        //       }
        //   }
        //   RotatedRect rotatedRect = new RotatedRect();
        //   rotatedRect = Cv2.MinAreaRect(contours[indexMax]);
        //   rotatedRect.Size.Width -=10;
        //   rotatedRect.Size.Height -= 10;
        //   float rwF = (float)rotatedRect.BoundingRect().Width-30;
        //   float rhF = (float)rotatedRect.BoundingRect().Height-30;
        //   float angle = (float)rotatedRect.Angle;

        //   var pCenter = new System.Drawing.PointF(
        //       (float)rotatedRect.Center.X , (float)rotatedRect.Center.Y);
        //if (angle > 45)
        //    angle = 360 - angle;
        //RectRotate rr = new RectRotate(
        //    new RectangleF(-rwF / 2f, -rhF / 2f, rwF, rhF),
        //    pCenter,
        //   angle ,
        //    AnchorPoint.None
        //);
        public bool IsHardNoise;
        public void EditMode(RectRotate rectRotate)
        {
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            list_Patterns = new List<BeeCpp.Pattern>();
            rectRotates = new List<RectRotate>();
            list_ColorPixel = new List<ColorPixel>();
            listScore = new List<double>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            ResultMulti = new List<ResultMulti>();
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (raw.Empty()) return;

                if (raw.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);


                bool ownsMatProcess = false;

                try
                {




                    //RectRotateCli rrCli = Converts.ToCli(rectRotate);
                    //RectRotateCli? rrMaskCli =
                    //    (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                    //Pattern.SetImgeRaw(
                    //    raw.Data,
                    //    raw.Width,
                    //    raw.Height,
                    //    (int)raw.Step(),
                    //    raw.Channels(),
                    //    rrCli,
                    //    rrMaskCli
                    //);

                    RectRotate rotAuto = new RectRotate(new RectangleF(-raw.Width / 2, -raw.Height / 2, raw.Width, raw.Height), new PointF(raw.Width / 2, raw.Height / 2), 0, AnchorPoint.None);
                    Mat gray = Cropper.CropRotatedRect(raw, rotAuto, null);
                    rotArea = CheckPage(gray);//rotArea= 
                    gray = Cropper.CropRotatedRect(raw, rotArea, null);
                    //  Cv2.EqualizeHist(gray, gray);//note cv.imwite o day
                    // 1) Segment -> maskPtr (như bạn đang làm)
                    var segP = new MonoSegCliParams { BgBlurK = 41, OpenK = 2, CloseK = 4, Mode = 0, UseBlackHat = true, BlackHatK = 31 };
                    IntPtr maskPtr = IntPtr.Zero; int maskStep;
                    MonoSegCli.SegmentMonoLowContrast(gray.Data, gray.Width, gray.Height, (int)gray.Step(), out maskPtr, out maskStep, segP, IsHardNoise);

                    // 2) Extract paper + chips (RectRotateCli[])
                    var extP = new ChipExtractCliParams
                    {
                        MinArea = 300,
                        MinW = 8,
                        MinH = 10,
                        MinAspect = 1.2f,
                        VertKW = 3,
                        //VertKH = 15,
                        VertKH = 15,
                        OpenK = 5,
                        MinFillRatio = 0.50f,
                        SizeTol = 0.40f,
                        PaperMinAreaFrac = 0.02f
                    };
                    RectRotateCli[] ListRotRect = MonoSegCli.ExtractPaperAndChipRects(maskPtr, gray.Width, gray.Height, maskStep, extP);

                    // 3) Draw to color result
                    //    IntPtr colorPtr = IntPtr.Zero; int colorStep;
                    //    MonoSegCli.DrawRectRotateToColorImage(maskPtr, gray.Width, gray.Height, maskStep, ListRotRect, out colorPtr, out colorStep);

                    //   Mat color = new Mat(gray.Height, gray.Width, MatType.CV_8UC3, colorPtr, colorStep);

                    // Cv2.ImWrite("check.png", color);
                    // Cv2.ImWrite("Result.png", color);
                    //  Cv2.WaitKey();

                    //  MonoSegCli.FreeBuffer(colorPtr);
                    MonoSegCli.FreeBuffer(maskPtr);

                    // Cv2.WaitKey();


                    //var listRS = Pattern.Match(
                    //    IsHighSpeed,
                    //    StepAngle,
                    //    AngleLower,
                    //    AngleUper,
                    //    Common.PropetyTools[IndexThread][Index].Score / 100.0,
                    //    ckSIMD,
                    //    ckBitwiseNot,
                    //    ckSubPixel,
                    //    MaxObject,
                    //    OverLap,
                    //    false,
                    //    -1
                    //);

                    //if (listRS == null)
                    //    return;
                    //if (listRS.Count == 0)
                    //    return;
                    if (list_Patterns == null)
                        list_Patterns = new List<BeeCpp.Pattern>();

                    float scoreSum = 0f;


                    int i = 0;
                    int count = ListRotRect.Count();

                    list_Patterns = new List<BeeCpp.Pattern>(count);
                    list_ColorPixel = new List<ColorPixel>(count);

                    for (int k = 0; k < count; k++)
                    {
                        list_Patterns.Add(null);
                        list_ColorPixel.Add(null);
                    }
                    var listMatTemp = new Mat[ListRotRect.Count()];
                    var listMatTempColor = new Mat[ListRotRect.Count()];
                    foreach (RectRotateCli rot in ListRotRect)
                    {
                        // scoreSum += (float)rot.Score;

                        float rwF = (float)rot.RectWH.Width;
                        float rhF = (float)rot.RectWH.Height;
                        float angle = (float)rot.RectRotationDeg;

                        var pCenter = new System.Drawing.PointF(
                            (float)rot.PosCenter.X - 2, (float)rot.PosCenter.Y - 2);
                        //if (angle > 45)
                        //    angle = 360 - angle;
                        RectRotate rr = new RectRotate(
                            new RectangleF(-rwF / 2f, -rhF / 2f, rwF, rhF),
                            pCenter,
                           (angle - 180),
                            AnchorPoint.None
                        );

                        rectRotates.Add(rr);
                        // listScore.Add(Math.Round(rot.Score, 1));

                    }
                    i = 0;
                    list_Patterns = new List<Pattern>();
                    list_ColorPixel = new List<ColorPixel>();
                    foreach (RectRotate rot in rectRotates)
                    {
                        list_ColorPixel.Add(new ColorPixel());
                        list_Patterns.Add(new BeeCpp.Pattern());
                        RectRotateCli? rrMaskCli2 =
                            (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
                        RectRotate rotTemp = rot.Clone();
                        rotTemp.ExpandPixels(6, 6);
                        RectRotateCli rrCli2 = Converts.ToCli(rotTemp);
                        RectRotateCli? rrMaskCliLocal2 = null;


                        ResultMulti.Add(new BeeGlobal.ResultMulti());// rot, listMatTemp[i].ToBitmap(), null, listMatTempColor[i].ToBitmap(), null, null));

                        int w = 0, h = 0, s = 0, c = 0;

                        IntPtr intpr = list_Patterns[i].SetImgeSample(
                              gray.Data,
                              gray.Width,
                              gray.Height,
                              (int)gray.Step(),
                              gray.Channels(),
                              rrCli2,
                              rrMaskCliLocal2,
                              false,
                              out w, out h, out s, out c

                          );
                        list_Patterns[i].LearnPattern();
                        MatType mt = c == 1 ? MatType.CV_8UC1
                                  : c == 3 ? MatType.CV_8UC3
                                  : MatType.CV_8UC4;
                        using (var m = new Mat(h, w, mt, intpr, s))
                        {
                            ResultMulti[i].BTemp = m.ToBitmap();
                            //  list_ColorPixel[i] = new BeeCpp.ColorPixel();

                            if (m.Type() == MatType.CV_8UC1)
                            {
                                Cv2.CvtColor(m, m,
                                             ColorConversionCodes.GRAY2BGR);
                            }
                            list_ColorPixel[i].SetImgeSampleNoCrop(
                               m.Data,
                               m.Width,
                              m.Height,
                               (int)m.Step(),
                               m.Channels()
                                );
                            list_ColorPixel[i].SaveRandom(i);
                            //ResultMulti[i].BTempColor = m.Clone();
                        }
                        //  list_ColorPixel[i] = new BeeCpp.ColorPixel();


                        rot.ExpandPixels(ExpandX, ExpandY);
                        rot._rectRotation = 0;
                        ResultMulti[i].RotCalib = rot.Clone();
                        ResultMulti[i].rotAdj = rot.Clone();
                        //MatType mt = c == 1 ? MatType.CV_8UC1
                        //          : c == 3 ? MatType.CV_8UC3
                        //          : MatType.CV_8UC4;
                        //using (var m = new Mat(h, w, mt, intpr, s))
                        //{
                        //    ResultMulti[i].BTemp = m.ToBitmap();
                        //    //ResultMulti[i].BTempColor = m.Clone();
                        // //   listMatTempColor[i] = m.Clone();
                        //    if (IsColorPixel)
                        //    {

                        //      //  list_ColorPixel[i] = new BeeCpp.ColorPixel();

                        //        if (m.Type() == MatType.CV_8UC1)
                        //        {
                        //            Cv2.CvtColor(m, m,
                        //                         ColorConversionCodes.GRAY2BGR);
                        //        }

                        //        list_ColorPixel[i].SetImgeSampleNoCrop(
                        //            m.Data,
                        //            m.Width,
                        //           m.Height,
                        //            (int)m.Step(),
                        //            m.Channels()
                        //        );
                        //        list_ColorPixel[i].SaveRandom(i);
                        //    }
                        //    rot.ExpandPixels(ExpandX, ExpandY);
                        //    ResultMulti[i].RotCalib = rot.Clone();
                        //    ResultMulti[i].rotAdj = rot.Clone();
                        //    // Cv2.ImWrite($@"D:\temp\samp\img_{i}.png", listMatTempColor[i]);


                        //}


                        i++;
                    }


                    //foreach ( RectRotate rot in rectRotates)
                    //{
                    //    rot.ExpandPixels(ExpandX, ExpandY);

                    //    ResultMulti.Add(new BeeGlobal.ResultMulti(rot, listMatTemp[i].ToBitmap(), null, listMatTempColor[i].ToBitmap(),null,null));
                    //    ResultMulti[i].rotAdj = rot.Clone();
                    //    i++;
                    //}
                    LimitCounter = rectRotates.Count();
                }
                catch (Exception ex)
                {
                    Global.LogsDashboard?.AddLog(
                        new LogEntry(DateTime.Now, LeveLLog.ERROR, "Pattern", ex.ToString()));
                }
                finally
                {
                    if (ownsMatProcess && matProcess != null)
                        matProcess.Dispose();
                }
            }
        }

        private RectRotate CalcRoiRect(Mat src, RectRotate rr)
        {
            int rw = (int)Math.Round(rr._rect.Width);
            int rh = (int)Math.Round(rr._rect.Height);

            int cx = (int)Math.Round(rr._PosCenter.X);
            int cy = (int)Math.Round(rr._PosCenter.Y);

            int x = cx - (rw >> 1);
            int y = cy - (rh >> 1);

            if (x < 0) x = 0;
            if (y < 0) y = 0;

            if (x + rw > src.Width) rw = src.Width - x;
            if (y + rh > src.Height) rh = src.Height - y;

            if (rw < 1) rw = 1;
            if (rh < 1) rh = 1;

            float roiCx = x + rw * 0.5f;
            float roiCy = y + rh * 0.5f;

            RectangleF localRect = new RectangleF(-rw * 0.5f, -rh * 0.5f, rw, rh);

            return new RectRotate(localRect, new PointF(roiCx, roiCy), 0f, AnchorPoint.None);
        }

        public void RunMode(RectRotate rectRotate)
        {
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            rectRotates = new List<RectRotate>();
            listScore = new List<double>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();

            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (raw.Empty()) return;

                if (raw.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);

                try
                {
                    //RectRotate rotAuto = new RectRotate(new RectangleF(-raw.Width / 2, -raw.Height / 2, raw.Width, raw.Height), new PointF(raw.Width / 2, raw.Height / 2), 0, AnchorPoint.None);
                    //Mat gray = Cropper.CropRotatedRect(raw, rotAuto, null);
                    //rectRotate = CheckPage(gray);//rotArea= 

                    int l = Math.Min(list_Patterns.Count, ResultMulti.Count);
                    Mat crop = Cropper.CropRotatedRect(raw, rectRotate, null);
                    IntPtr data = crop.Data;
                    int step = (int)crop.Step();
                    int elem = crop.ElemSize();
                    int ch = crop.Channels();

                    MatType type =
                        (ch == 1) ? MatType.CV_8UC1 :
                        (ch == 3) ? MatType.CV_8UC3 :
                                    MatType.CV_8UC4;

                    //Cv2.ImWrite($"Temp\\Raw.png", crop);
                    Parallel.For(0, l,
                   new ParallelOptions { MaxDegreeOfParallelism = Math.Min(300, Environment.ProcessorCount) },
                   i =>
                   {
                       try
                       {
                          // Debug.WriteLine($"[ENTER] i={i}");

                           var rm = ResultMulti[i];

                           if (rm == null)
                               return;
                           RectRotate rotCrop = rm.RotCalib;
                           RectRotate roiRR = CalcRoiRect(crop, rotCrop);

                           int w0 = (int)Math.Round(roiRR._rect.Width);
                           int h0 = (int)Math.Round(roiRR._rect.Height);
                           int x = (int)Math.Round(roiRR._PosCenter.X - w0 * 0.5f);
                           int y = (int)Math.Round(roiRR._PosCenter.Y - h0 * 0.5f);

                           // clamp
                           if (x < 0) x = 0;
                           if (y < 0) y = 0;
                           if (x + w0 > crop.Width) w0 = crop.Width - x;
                           if (y + h0 > crop.Height) h0 = crop.Height - y;
                           if (w0 < 1 || h0 < 1)
                               return;

                           int offset = y * step + x * elem;
                           IntPtr ptr = IntPtr.Add(data, offset);



                           Mat matCrop0 = new Mat(h0, w0, type, ptr, step);

                           switch (TypeMode)
                           {
                               case Mode.Pattern:
                                   list_Patterns[i].SetRawNoCrop(
                                           ptr,
                                           w0,
                                           h0,
                                           step,
                                           ch
                                       );
                                   var rot = list_Patterns[i].Match(
                                           IsHighSpeed,
                                           StepAngle,
                                           AngleLower,
                                           AngleUper,
                                           Common.PropetyTools[IndexThread][Index].Score / 100.0,
                                           ckSIMD,
                                           ckBitwiseNot,
                                           ckSubPixel,
                                           1,
                                           OverLap,
                                           false,
                                           -1
                                       );
                                   if (rot == null || rot.Count == 0)
                                   {
                                       rm.Score = 0;
                                       rm.RotCheck = null;
                                       return;
                                   }

                                   var r0 = rot[0];

                                   float w1 = (float)r0.Width;
                                   float h1 = (float)r0.Height;
                                   float angle = (float)r0.AngleDeg;

                                   var center = new System.Drawing.PointF(
                                           (float)r0.Cx,
                                           (float)r0.Cy
                                       );

                                   RectRotate rotCheck = new RectRotate(
                                           new System.Drawing.RectangleF(-w1 / 2f, -h1 / 2f, w1, h1),
                                           center,
                                           angle,
                                           AnchorPoint.None
                                       );

                                   if (!Global.IsRun || Global.IsAutoTemp)
                                       rm.RotOrigin = rotCheck.Clone();

                                   rm.RotCheck = rotCheck;
                                   rm.Score = (float)Math.Round(r0.Score, 1);
                                   break;
                               case Mode.Edge:
                                   var segP = new MonoSegCliParams { BgBlurK = 41, OpenK = 2, CloseK = 4, Mode = 0, UseBlackHat = false, BlackHatK = 31 };
                                   IntPtr maskPtr = IntPtr.Zero; int maskStep;
                                   // Mat crop2 = Cropper.CropRotatedRect(matCrop0, rotCrop, null);
                                   MonoSegCli.SegmentMonoLowContrast(matCrop0.Data, matCrop0.Width, matCrop0.Height, (int)matCrop0.Step(), out maskPtr, out maskStep, segP, IsHardNoise);
                                   Mat grays = new Mat(h0, w0, type, maskPtr, maskStep);

                                   // 2) Extract paper + chips (RectRotateCli[])
                                   var extP = new ChipExtractCliParams
                                   {
                                       MinArea = 50,
                                       MinW = 5,
                                       MinH = 10,
                                       MinAspect = 1.2f,
                                       VertKW = 3,
                                       //VertKH = 15,
                                       VertKH = 15,
                                       OpenK = 3,
                                       MinFillRatio = 0.32f,
                                       SizeTol = 0.40f,
                                       PaperMinAreaFrac = 0.02f
                                   };
                                   RectRotateCli[] rot2 = MonoSegCli.ExtractPaperAndChipRects(maskPtr, matCrop0.Width, matCrop0.Height, maskStep, extP);
                                   IntPtr colorPtr = IntPtr.Zero; int colorStep;
                                   MonoSegCli.DrawRectRotateToColorImage(maskPtr, grays.Width, grays.Height, maskStep, rot2, out colorPtr, out colorStep);

                                   Mat color = new Mat(grays.Height, grays.Width, MatType.CV_8UC3, colorPtr, colorStep);

                                   //  Cv2.ImWrite("Temp\\Crop" + i + ".png", color);
                                   if (rot2 == null || rot2.Length == 0)
                                   {
                                       rm.Score = 0;
                                       rm.RotCheck = null;
                                       return;
                                   }

                                   RectRotateCli r2 = rot2[0];

                                   float w2 = (float)r2.RectWH.Width;
                                   float h2 = (float)r2.RectWH.Height;
                                   float angle2 = (float)r2.RectRotationDeg;
                                   if (angle2 > 90) angle2 = 180 - angle2;
                                   var center2 = new System.Drawing.PointF(
                                           (float)r2.PosCenter.X,
                                           (float)r2.PosCenter.Y
                                       );

                                   RectRotate rotCheck2 = new RectRotate(
                                           new System.Drawing.RectangleF(-w2 / 2f, -h2 / 2f, w2, h2),
                                           center2,
                                           angle2,
                                           AnchorPoint.None
                                       );

                                   if (!Global.IsRun|| Global.IsAutoTemp)
                                       rm.RotOrigin = rotCheck2.Clone();

                                   rm.RotCheck = rotCheck2;
                                   rm.Score = 99;
                                   break;
                           }
                           //if(Global.IsRun&&rm.rotAdj!=null)
                           //{
                           //    rotCrop = rm.rotAdj;
                           //}    

                           //    Cv2.ImWrite($"Temp\\crop_{i}.png", matCrop0);



                           //if (IsColorPixel && list_ColorPixel[i] != null)
                           //{
                           //    if (matCrop0.Type() == MatType.CV_8UC1)
                           //    {
                           //        Cv2.CvtColor(matCrop0, matCrop0, ColorConversionCodes.GRAY2BGR);
                           //    }
                           //    Mat matCrop1 = Cropper.CropRotatedRect(
                           //            matCrop0, rm.RotCheck, null);
                           //    if (matCrop1.Type() == MatType.CV_8UC1)
                           //    {
                           //        Cv2.CvtColor(matCrop1, matCrop1, ColorConversionCodes.GRAY2BGR);
                           //    }
                           //    //    Cv2.ImWrite("Temp\\" + i + ".png", matCrop1);
                           //    list_ColorPixel[i].SetRawNoCrop(
                           //            matCrop1.Data,
                           //            matCrop1.Width,
                           //            matCrop1.Height,
                           //            (int)matCrop1.Step(),
                           //            matCrop1.Channels()
                           //        );
                           //    float pxOut = 0;
                           //    int s3, c3, w3, h3;
                           //    IntPtr intpr = list_ColorPixel[i].CheckImageFromMat(
                           //            false, 1, false,
                           //            (int)ThreshColor, 30,
                           //            LimitAspect,
                           //            out pxOut,
                           //            ref OffsetX,
                           //            ref OffsetY,
                           //            ref OffsetAngle,
                           //            out w3,
                           //            out h3,
                           //            out s3,
                           //            out c3
                           //        );
                           //    // list_ColorPixel[i].SaveRandom(i);
                           //    rm.ScoreColor = pxOut;
                           //    if (intpr != IntPtr.Zero)
                           //    {
                           //        MatType mt =
                           //                c3 == 1 ? MatType.CV_8UC1 :
                           //                c3 == 3 ? MatType.CV_8UC3 :
                           //                        MatType.CV_8UC4;


                           //        if (intpr != IntPtr.Zero)
                           //        {
                           //            if (rm.BCheckColor == null) rm.BCheckColor = new Mat();
                           //            if (!rm.BCheckColor.Empty()) rm.BCheckColor.Dispose();
                           //            rm.BCheckColor = new Mat(h3, w3, mt, intpr, s3);
                           //            //using (var m = new Mat(h3, w3, mt, intpr, s3))
                           //            //{
                           //            //    rm.BCheckColor = m.ToBitmap();
                           //            //}
                           //            // list_ColorPixel[i].FreeBuffer(ptr);
                           //        }
                           //    }
                           //}


                           var org = rm.RotOrigin;
                           var chk = rm.RotCheck;

                           if (org != null && chk != null)
                           {
                               rm.deltaX = (float)Math.Round(
                                       (chk._PosCenter.X - org._PosCenter.X) / Scale, 1);//pixel to mm

                               rm.deltaY = (float)Math.Round(
                                       (chk._PosCenter.Y - org._PosCenter.Y) / Scale, 1);//pixel to mm
                           }
                           else
                           {
                               rm.deltaX = 0;
                               rm.deltaY = 0;
                           }
                       }


                       catch (Exception ex)
                       {
                           Console.WriteLine($"[EXCEPTION] i={i} {ex}");
                           throw; // hoặc giữ lại để xem
                       }
                   });



                }
                catch (Exception ex)
                {
                    Global.LogsDashboard?.AddLog(
                        new LogEntry(DateTime.Now, LeveLLog.ERROR, "Pattern", ex.ToString()));
                }
                finally
                {


                    if (matProcess != null)
                        matProcess.Dispose();
                }
            }
        }



        public ModeCalibVisualMatch ModeCalibVisualMatch = ModeCalibVisualMatch.Normal;

        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            if (Global.IsAutoTemp)
            {
                IsCalibs = true;
                EditMode(rotArea);
                RunMode(rotArea);
            }
           else if (Global.IsRun)
            {
                RunMode(rotArea);
            }
            else if (!Global.IsRun)
            {
                if (IsCalibs)

                {
                    EditMode(rotArea);
                }
                else
                    RunMode(rotArea);
            }
        }
        public int LimitColor = 0;
        public float LimitAspect = 0;
        public void Complete()
        {
            bool IsNG = false;
            foreach (ResultMulti rs in ResultMulti)
            {
                if (rs.ScoreColor > LimitColor)
                {
                    IsNG = true;
                    rs.IsOK = false;
                    //Mat process = rs.BCheckColor.Clone();
                    //float Aspect = (float)process.Width / (float)process.Height;
                    //if (Aspect < LimitAspect)
                    //{
                    //    using (process)
                    //    {
                    //        PatchCropContext ctx = new PatchCropContext();
                    //        switch (MethordEdge)
                    //        {
                    //            case MethordEdge.CloseEdges:
                    //                Filters.Edge(process);
                    //                break;
                    //            case MethordEdge.StrongEdges:
                    //                Filters.GetStrongEdgesOnly(process);
                    //                break;
                    //            case MethordEdge.Binary:
                    //                Filters.Threshold(process, ThresholdBinary, ThresholdTypes.Binary);
                    //                break;
                    //            case MethordEdge.InvertBinary:
                    //                Filters.Threshold(process, ThresholdBinary, ThresholdTypes.BinaryInv);
                    //                break;
                    //        }
                    //        if (rs.RotCheck == null || ctx == null)
                    //        {

                    //        }
                    //        rs.BCheckColor = ApplyShapeMaskAndCompose(process, ctx, rs.RotCheck, null, returnMaskOnly: false);
                    //        //Cv2.ImWrite($@"E:\check\img_{rs}.png", rs.BCheckColor);
                    //    }
                    //    IsNG = false;
                    //    rs.IsOK = true;
                    //    if (process != null)
                    //    {
                    //        process.Dispose();
                    //    }
                    //}
                }
                else if (Math.Abs(rs.deltaX) > LimitX)
                {

                    IsNG = true;
                    rs.IsOK = false;
                }
                else if (Math.Abs(rs.deltaY) > LimitY)
                {

                    IsNG = true;
                    rs.IsOK = false;
                }
                else
                    rs.IsOK = true;
                if (rs.RotCheck == null)
                    continue;
                ValueCompare++;

            }
            Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            Common.PropetyTools[IndexThread][Index].ScoreResult = ValueCompare;
            if (ValueCompare == ResultMulti.Count && !IsNG)
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            }

            else
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }
            ValueCompare = 0;

        }
        public Graphics DrawResult(Graphics gc)
        {
            if (rotAreaAdjustment == null && Global.IsRun) return gc;
            if (Global.IsRun)
                gc.ResetTransform();
            RectRotate rotA = rotArea;
            if (Global.IsRun) rotA = rotAreaAdjustment;
            var mat = new Matrix();
            // 
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
            gc.Transform = mat;

            mat.Translate(rectPage._PosCenter.X, rectPage._PosCenter.Y);
            mat.Rotate(rectPage._rectRotation);
            gc.Transform = mat;
           // Draws.Box1Label(gc, rectPage, "Page", new Font("Arial", Global.ParaShow.FontSize), new SolidBrush(Global.ParaShow.TextColor), Color.Red, Global.ParaShow.ThicknessLine);
            gc.ResetTransform();
            mat = new Matrix();
            //if (!Global.IsRun)
            //{
            //    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
            //    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            //}
            //gc.Transform = mat;
            ////   Draws.DrawRectRotate(gc, rectPage, new Pen(Brushes.Red, 4));

            //gc.DrawLine(new Pen(Brushes.Green, 8), LineBot.X1, LineBot.Y1, LineBot.X2, LineBot.Y2);

            //gc.DrawLine(new Pen(Brushes.Blue, 2), LineTop.X1, LineTop.Y1, LineTop.X2, LineTop.Y2);

            //gc.DrawLine(new Pen(Brushes.Blue, 2), LineLeft.X1, LineLeft.Y1, LineLeft.X2, LineLeft.Y2);

            ////
            //gc.DrawLine(new Pen(Brushes.Blue, 2), LineRight.X1, LineRight.Y1, LineRight.X2, LineRight.Y2);

            //gc.ResetTransform();

            mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;
            if (Common.PropetyTools[Global.IndexChoose][Index].Results == Results.NG)
            {
                cl = Global.ParaShow.ColorNG;
            }
            else
            {
                cl = Global.ParaShow.ColorOK;
            }
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (Global.ParaShow.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl, Global.ParaShow.ThicknessLine);
            gc.ResetTransform();


            if (listScore == null) return gc;
            if (list_Patterns.Count > 0)
            {

                int i = 0;

                i = 0;
                foreach (ResultMulti rs in ResultMulti)
                {
                    Color clPCs = Color.LimeGreen;
                    if (!rs.IsOK)
                    {
                        clPCs = Global.ParaShow.ColorNG;
                    }
                    else
                    {
                        clPCs = Global.ParaShow.ColorOK;
                    }
                    RectRotate rot = rs.RotCalib;
                    RectRotate rotOrigin = rs.RotOrigin;
                    mat = new Matrix();
                    if (!Global.IsRun)
                    {
                        mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                        mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                    }
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);
                    mat.Translate(rotA._rect.X, rotA._rect.Y);
                    gc.Transform = mat;
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);

                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                    if (Global.ParaShow.IsShowDetail)
                        Draws.Box2Label(gc, rot._rect, i + "", "", font, Global.ParaShow.ColorNone, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine, 10, 2);
                    if (rs.RotCheck == null)
                    {
                        i++; continue;
                    }

                    mat.Translate(rot._rect.X, rot._rect.Y);
                    mat.Translate(rs.RotCheck._PosCenter.X, rs.RotCheck._PosCenter.Y);
                    mat.Rotate(rs.RotCheck._rectRotation);

                    gc.Transform = mat;
                    if (IsColorPixel && rs.BCheckColor != null)
                    {
                        Bitmap myBitmap = rs.BCheckColor.ToBitmap();
                        myBitmap.MakeTransparent(Color.Black);
                        myBitmap = General.ChangeToColor(myBitmap, Color.Red, (float)(Global.ParaShow.Opacity / 100.0));
                        gc.DrawImage(myBitmap, rs.RotCheck._rect);
                    }
                    Draws.Box2Label(gc, rs.RotCheck._rect, Math.Round(rs.Score, 1) + "%", "", font, clPCs, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);
                    Draws.Plus(gc, 0, 0, 10, clPCs, 2);
                  //  gc.DrawString(rs.ScoreColor + " px", font, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 50));
                    Font font1 = new Font("Arial", Global.ParaShow.FontSize);
                    gc.DrawString(rs.deltaX.ToString() + "," + rs.deltaY.ToString(), font1, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 10));
                    if (!IsCalibs && Global.ParaShow.IsShowDetail)
                    {
                        if (rs.RotOrigin == null) continue;
                        mat.Translate(-rs.RotCheck._PosCenter.X, -rs.RotCheck._PosCenter.Y);
                        mat.Rotate(-rs.RotCheck._rectRotation);
                        mat.Translate(rs.RotOrigin._PosCenter.X, rs.RotOrigin._PosCenter.Y);
                        mat.Rotate(rs.RotOrigin._rectRotation);
                        gc.Transform = mat;

                        Draws.Box2Label(gc, rs.RotOrigin._rect, "", "", font, Color.Yellow, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);
                        Draws.Plus(gc, 0, 0, 10, Color.Yellow, 2);
                    }

                    gc.ResetTransform();
                    i++;
                }
                if (IsCalibs)

                {
                    IsCalibs = false;

                }
            }






            return gc;
        }

        public int IndexThread;

        // using Python.Runtime;  // nếu bạn dùng Py.GIL()


        // C# 7.3 helper: đảm bảo ảnh là BGR 3 kênh
        private static Mat EnsureBgr(Mat src)
        {
            if (src.Empty()) return src;
            if (src.Channels() == 3) return src;
            Mat bgr = new Mat();
            Cv2.CvtColor(src, bgr, ColorConversionCodes.GRAY2BGR);
            return bgr;
        }

        // Tách hàm Python bridge để dễ dùng với try/finally
        private void CopyToPythonAndDetect(Mat bgr, int height, int width, int channels,
                                           ref Mat matProcess, ref byte[] rentedBuffer)
        {
            int size = checked((int)(bgr.Total() * bgr.ElemSize()));
            rentedBuffer = ArrayPool<byte>.Shared.Rent(size);
            Marshal.Copy(bgr.Data, rentedBuffer, 0, size);

            using (Py.GIL())
            {
                dynamic np = G.np;

                // frombuffer -> reshape; bọc bằng using để giải phóng PyObject
                using (var pyBuf = np.frombuffer(rentedBuffer, dtype: np.uint8))
                using (var npImage = pyBuf.reshape(height, width, channels))
                {
                    dynamic pyResult = null;
                    try
                    {
                        pyResult = G.Classic.EdgeDetection(npImage);
                        if (pyResult == null) return;

                        byte[] edgeBytes = pyResult.As<byte[]>();
                        matProcess = new Mat(height, width, MatType.CV_8UC1, edgeBytes);
                    }
                    finally
                    {
                        if (pyResult != null)
                        {
                            // PyObject có IDisposable khi dùng Python.Runtime.PyObject
                            var disp = pyResult as IDisposable;
                            if (disp != null) disp.Dispose();
                        }
                    }
                }
            }
        }



    }
}
