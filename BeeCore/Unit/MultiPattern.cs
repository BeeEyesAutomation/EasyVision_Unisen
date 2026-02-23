using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;

using BeeGlobal;
using CvPlus;
using Newtonsoft.Json.Linq;
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
using System.Runtime.CompilerServices;
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
        public TypeYolo TypeYolo = TypeYolo.YOLO;
        public string pathFullModel = "";
        public String PathModel;
        public List<string> listModels = new List<string>();
        public List<LabelItem> labelItems = new List<LabelItem>();
        public List<Labels> listLabelCompare = new List<Labels>();
        public FilterBox FilterBox = FilterBox.Merge;
        public float ThreshOverlap = 0.1f;
        [NonSerialized]
        private NativeYolo NativeOnnx;
        [NonSerialized]
        private NativeYolo.YoloBox[] OnnxBoxes;
        public Dictionary<int, string> ListNameOnnx = new Dictionary<int, string>();
        public static string[] DictToArray(Dictionary<int, string> dict)
        {
            if (dict == null || dict.Count == 0)
                return Array.Empty<string>();

            int max = dict.Keys.Max();

            string[] arr = new string[max + 1];

            foreach (var kv in dict)
                arr[kv.Key] = kv.Value;

            return arr;
        }

        public void SetModel()
        {
            if (Scale == 0) Scale = 1;
            if (rotCrop == null) rotCrop = new RectRotate();
            if (rotArea == null) rotArea = new RectRotate();
            if (Pattern == null)
            {
                Pattern = new BeeCpp.Pattern();

            }
            if (ExpandX == 0) ExpandX = 50;
            if (ExpandY == 0) ExpandY = 50;

            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;

            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            if (Common.PropetyTools[IndexThread][Index].Score == 0)
                Common.PropetyTools[IndexThread][Index].Score = 80;
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
            try
            {
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.NotInitial;
                if (pathFullModel != null)
                {
                    if (pathFullModel.Trim().Contains(".pth"))
                    {
                        TypeYolo = TypeYolo.RCNN;

                    }
                    else if (pathFullModel.Trim().Contains(".pt"))
                    {
                        TypeYolo = TypeYolo.YOLO;

                    }

                    else
                    {
                        TypeYolo = TypeYolo.YOLO;
                        Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                        return;
                    }
                    if (Global.IsIntialPython)
                        using (Py.GIL())
                        {

                            if (!File.Exists(pathFullModel))
                            {
                                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                                return;
                            }
                            G.objYolo.load_model(Common.PropetyTools[IndexThread][Index].Name, pathFullModel, (int)TypeYolo);
                            try
                            {
                                String pathBlackDot = @"E:\Code\EasyVision_Unisen\bin\Release\Program\AD_MAYBOI\BlackDot1024";
                            int    NumThreadCPU = 16;
                              
                              
                                NativeOnnx = new NativeYolo(pathBlackDot + "\\best.xml", 0, 0, NumThreadCPU);

                                NativeOnnx.Warmup(10);
                                OnnxBoxes = new NativeYolo.YoloBox[20];
                                TypeYolo = TypeYolo.Onnx;
                                ListNameOnnx = NativeOnnx.LoadNames(pathBlackDot + "\\metadata.yaml");
                              
                                //Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            // if(pathBlackDot!=null)
                            //if (File.Exists(pathBlackDot))
                            //    for (int i=0;i<60;i++)
                            //{
                            //    G.objYolo.load_model(Common.PropetyTools[IndexThread][Index].Name.Trim() + i, pathBlackDot, (int)TypeYolo);


                            //}
                            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                           
                              


                            
                        }
                }
            }
            catch (PythonException pyEx)
            {
                MessageBox.Show("Python Error: " + pyEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            Parallel.For(
                               0, ResultMulti.Count,
                               new ParallelOptions
                               {
                                   MaxDegreeOfParallelism = Math.Min(10, Environment.ProcessorCount)
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
     

        
            if (bmRaw != null)
            {
                matTemp = bmRaw.ToMat();
                LearnPattern(matTemp, true);
            }
        
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        public String[] LoadNameModel(String nameTool)
        {

            if (Global.IsIntialPython && TypeYolo == TypeYolo.YOLO)
                using (Py.GIL())
                {



                    dynamic result = G.objYolo.loadNames(nameTool);

                    // Dùng list() để ép dict_values về list
                    PyObject obj = Py.Import("builtins").GetAttr("list").Invoke(result.InvokeMethod("values"));
                    var labels = new List<string>();
                    int counts = (int)obj.Length();
                    for (int j = 0; j < counts; j++)
                    {

                        labels.Add(obj[j].ToString());  // hoặc item.As<string>() nếu bạn chắc chắn là string
                    }


                    return labels.ToArray();

                }
            else
                return new string[0];
        }
        public List<RectRotate> rectRotates = new List<RectRotate>();

        public List<float> list_AngleCenter = new List<float>();
        public ZeroPos ZeroPos = ZeroPos.Zero;
        public float Scale = 1;
        public int ExpandPage = 10;
        public int ExpandPattern = 2;
        public int WidthDetectBox = 300;
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
        public float AspectLen = 0.6f;
      
        // public int space = 500;
        public RectRotate CheckPage(Mat raw)
        {
            LineDirectionMode lineDirectionMode = LineDirectionMode.Horizontal;
           
            int W = raw.Width;
            int H = raw.Height;
            RansacIterations = 2000;
            
            if (raw.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);
            rotBot = new RectRotate(new RectangleF(-WidthDetectBox / 2f, -H / 4f, WidthDetectBox, H / 2f), new PointF(W / 2, 3 * H / 4f), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotBot, null))
            {

                Mat Edge = Filters.GetStrongEdgesOnly(crop);
                //  Edge = Filters.Morphology(Edge, MorphTypes.Open, new Size(7, 7));
                Cv2.ImWrite("Bot.png", Edge);
                LineBot = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Horizontal, LineScanMode.BottomToTop, 0, 40
                );

            }
            LineBot.X1 += rotBot._PosCenter.X + rotBot._rect.X; LineBot.Y1 += rotBot._PosCenter.Y + rotBot._rect.Y;
            LineBot.X2 += rotBot._PosCenter.X + rotBot._rect.X; LineBot.Y2 += rotBot._PosCenter.Y + rotBot._rect.Y;
            LineBot.X0 = LineBot.X1; LineBot.Y0 = LineBot.Y1;
            rotTop = new RectRotate(new RectangleF(-WidthDetectBox / 2f, -H / 4f, WidthDetectBox, H / 2f), new PointF(W / 2f, H / 4f), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotTop, null))
            {

                Mat Edge = Filters.GetStrongEdgesOnly(crop);
                //  Edge = Filters.Morphology(Edge, MorphTypes.Open, new Size(7, 7));
            //    Cv2.ImWrite("Top.png", Edge);
                LineTop = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Horizontal, LineScanMode.TopToBottom, 0, 40
                );
            }
            LineTop.X1 += rotTop._PosCenter.X + rotTop._rect.X; LineTop.Y1 += rotTop._PosCenter.Y + rotTop._rect.Y;
            LineTop.X2 += rotTop._PosCenter.X + rotTop._rect.X; LineTop.Y2 += rotTop._PosCenter.Y + rotTop._rect.Y;
            LineTop.X0 = LineTop.X1; LineTop.Y0 = LineTop.Y1;
            rotLeft = new RectRotate(new RectangleF(-W / 4f, -WidthDetectBox / 2f, W / 2f, WidthDetectBox), new PointF(W / 4f, H / 2), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotLeft, null))
            {

                Mat Edge = Filters.GetStrongEdgesOnly(crop);
                //  Edge = Filters.Morphology(Edge, MorphTypes.Open, new Size(7,7));
               // Cv2.ImWrite("Left.png", Edge);
                LineLeft = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Vertical, LineScanMode.LeftToRight, 0, 40
                );

            }
            LineLeft.X1 += rotLeft._PosCenter.X + rotLeft._rect.X; LineLeft.Y1 += rotLeft._PosCenter.Y + rotLeft._rect.Y;
            LineLeft.X2 += rotLeft._PosCenter.X + rotLeft._rect.X; LineLeft.Y2 += rotLeft._PosCenter.Y + rotLeft._rect.Y;
            LineLeft.X0 = LineLeft.X1; LineLeft.Y0 = LineLeft.Y1;
            rotRigth = new RectRotate(new RectangleF(-W / 4f, -WidthDetectBox / 2f, W / 2f, WidthDetectBox), new PointF(3 * W / 4f, H / 2), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotRigth, null))
            {

                Mat Edge = Filters.GetStrongEdgesOnly(crop);
                //   Edge = Filters.Morphology(Edge, MorphTypes.Open, new Size(7, 7));
              //  Cv2.ImWrite("Right.png", Edge);
                LineRight = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Vertical, LineScanMode.RightToLeft, 0, 40
                );

            }
            if (LineLeft.Found && LineTop.Found && LineRight.Found && LineBot.Found)
            {

            
            LineRight.X1 += rotRigth._PosCenter.X + rotRigth._rect.X; LineRight.Y1 += rotRigth._PosCenter.Y + rotRigth._rect.Y;
            LineRight.X2 += rotRigth._PosCenter.X + rotRigth._rect.X; LineRight.Y2 += rotRigth._PosCenter.Y + rotRigth._rect.Y;
            LineRight.X0 = LineRight.X1; LineRight.Y0 = LineRight.Y1;
            line1 = new Line2D(LineTop.Vx, LineTop.Vy, LineTop.X0, LineTop.Y0);
            line2 = new Line2D(LineBot.Vx, LineBot.Vy, LineBot.X0, LineBot.Y0);
            line3 = new Line2D(LineLeft.Vx, LineLeft.Vy, LineLeft.X0, LineLeft.Y0);
            line4 = new Line2D(LineRight.Vx, LineRight.Vy, LineRight.X0, LineRight.Y0);

            rectPage = InsertLine.CreateRectRotate(line1, line2, line3, line4);
            
            RectRotate rtReturn = rectPage.Clone();
            rtReturn.OffsetPixels(-ExpandPage,- ExpandPage);

            return rtReturn;
            }
            else
                {
                return new RectRotate(); }
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
        public int ScoreYolo = 10;
        public List<ResultItem> CheckBoxYolo( Mat raw )
        {
            List<ResultItem> resultTemp = new List<ResultItem>();
            using (Py.GIL())
            {
                PyObject result = null;
                PyObject boxes = null;
                PyObject scores = null;
                PyObject labels = null;
                try
                {
                    // === Crop ROI ===
                    using (Mat matCrop = raw.Clone())
                    {
                        dynamic dyn = G.objYolo;
                        if (dyn == null)
                            return new List<ResultItem>(); 
                        if (matCrop.Empty()) return new List<ResultItem>();
                        if (matCrop.Type().Depth != MatType.CV_8U)
                        {
                            using (var tmp8u = new Mat())
                            {
                                Cv2.ConvertScaleAbs(matCrop, tmp8u); // 16U/32F -> 8U
                                matCrop.AssignTo(tmp8u);             // ghi đè dữ liệu (OpenCvSharp: AssignTo giữ shape/type mới)
                            }
                        }
                        // 2) đảm bảo đúng số kênh
                        if (matCrop.Channels() == 1)
                        {
                            Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                        }
                        else if (matCrop.Channels() == 4)
                        {
                            Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGRA2BGR);
                        }
                        int h = matCrop.Rows;
                        int w = matCrop.Cols;
                        int ch = matCrop.Channels(); // 3
                        int stride = (int)matCrop.Step(); // bytes/row (có thể > w*ch)
                        IntPtr p = matCrop.Data;
                        float conf = (float)(ScoreYolo / 100.0);
                        string toolName = Common.PropetyTools[IndexThread][Index].Name ?? "default";
                        // Ký hiệu: result là tuple-like (3 phần)
                      
                        result = dyn.predict((long)p, h, w, ch, stride, conf, toolName);
                        // Ép về PyObject để chủ động Dispose
                        boxes = (PyObject)result[0];
                        scores = (PyObject)result[1];
                        labels = (PyObject)result[2];
                        int n = (int)boxes.Length();

                        // === Chuẩn bị danh sách output ===
                        //  if (resultTemp == null) resultTemp = new List<ResultItem>(n);
                      
                       
                        for (int j = 0; j < n; j++)
                        {
                            var b = boxes[j];   // PyObject
                            float x1 = (float)b[0].As<double>();
                            float y1 = (float)b[1].As<double>();
                            float x2 = (float)b[2].As<double>();
                            float y2 = (float)b[3].As<double>();

                            float bw = x2 - x1;
                            float bh = y2 - y1;
                            float cx = x1 + bw * 0.5f;
                            float cy = y1 + bh * 0.5f;

                            var rt = new RectRotate(
                                new System.Drawing.RectangleF(-bw / 2f, -bh / 2f, bw, bh),
                                new System.Drawing.PointF(cx, cy),
                                0f, AnchorPoint.None);
                            ResultItem resultItem = new BeeCore.ResultItem(((PyObject)labels[j]).ToString());
                            resultItem.rot = rt;
                            int index = labelItems.FindIndex(item => string.Equals(item.Name, resultItem.Name, StringComparison.OrdinalIgnoreCase));
                            if (index < -1)
                                continue;
                            LabelItem item1 = labelItems[index];
                            if (!item1.IsUse)
                                continue;
                            if (item1.IsArea)
                            {
                              int  Area =(int)( resultItem.rot._rect.Size.Width * resultItem.rot._rect.Size.Height );
                                if (Area < item1.ValueArea * 100)
                                    continue;
                            }
                            
                                    resultTemp.Add(new BeeCore.ResultItem(((PyObject)labels[j]).ToString()));
                            resultTemp[resultTemp.Count - 1].rot = rt;
                            resultTemp[resultTemp.Count - 1].Name= ((PyObject)labels[j]).As<String>();
                            resultTemp[resultTemp.Count - 1].Score = (float)((PyObject)scores[j]).As<double>() * 100f;
                        }
                        switch (FilterBox)
                        {
                            case FilterBox.Merge:
                                resultTemp = ResultFilter.MergeSameNameOverlap(resultTemp, ThreshOverlap);
                                break;
                            case FilterBox.Remove:
                                resultTemp = ResultFilter.FilterRectRotate(resultTemp, ThreshOverlap);
                                break;
                        }
                        GC.KeepAlive(matCrop);
                    }
                }
                catch (PythonException pyEx)
                {
                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", pyEx.ToString()));
                }
                catch (Exception ex)
                {
                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Learning", ex.ToString()));
                }
                finally
                {
                    // Giải phóng PyObject để tránh rò rỉ
                    if (labels != null) labels.Dispose();
                    if (scores != null) scores.Dispose();
                    if (boxes != null) boxes.Dispose();
                    if (result != null) result.Dispose();
                }
            }
          return  resultTemp;
        }
        public void EditMode(RectRotate rectRotate)
        {
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            list_Patterns = new List<BeeCpp.Pattern>();
            rectRotates = new List<RectRotate>();
           
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





                    RectRotate rotAuto = new RectRotate(new RectangleF(-raw.Width / 2, -raw.Height / 2, raw.Width, raw.Height), new PointF(raw.Width / 2, raw.Height / 2), 0, AnchorPoint.None);
                   
                    Mat gray = Cropper.CropRotatedRect(raw, rotAuto, null);
                    rotArea = CheckPage(gray);
                    int Area =(int)( rotArea._rect.Width * rotArea._rect.Height);
                    int Sz = gray.Width * gray.Height;
                    if (Area<=Sz*0.2f)
                    {
                        Mat enhan = Filters.AutoEnhanceForPaper(gray);
                      //  Cv2.ImWrite("Enhan.png", enhan);
                        Mat mask = Filters.DetectPaperMask(enhan);
                      //  Cv2.ImWrite("Mask.png", mask);
                        gray =
                       Filters.RemoveBorderTouchAndKeepCenter(
                           mask,
                           borderMargin: 20,
                           minArea: 20000
                            );
                       // Cv2.ImWrite("CANY.png", gray);
                        rotArea = CheckPage(gray);//rotArea= 
                    }    
                  
                    gray = Cropper.CropRotatedRect(raw, rotArea, null);
                  
                    List<ResultItem> itChip=  CheckBoxYolo(gray);
                
                    if (list_Patterns == null)
                        list_Patterns = new List<BeeCpp.Pattern>();

                    float scoreSum = 0f;


                    int i = 0;
                    int count = itChip.Count();

                    list_Patterns = new List<BeeCpp.Pattern>(count);
                  

                 
                  
                    foreach (ResultItem rs in itChip)
                    {
                      
                        rectRotates.Add(rs.rot);
                      
                    }
                    i = 0;
                    list_Patterns = new List<Pattern>();
                   
                    foreach (RectRotate rot in rectRotates)
                    {
                       
                        list_Patterns.Add(new BeeCpp.Pattern());
                        RectRotateCli? rrMaskCli2 =
                            (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
                        RectRotate rotTemp = rot.Clone();
                        rotTemp.ExpandPixels(ExpandPattern, ExpandPattern);
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
                       

                        rot.ExpandPixels(ExpandX, ExpandY);
                        MatType mt = c == 1 ? MatType.CV_8UC1
                                   : c == 3 ? MatType.CV_8UC3
                                   : MatType.CV_8UC4;
                        using (var m = new Mat(h, w, mt, intpr, s))
                        {

                            ResultMulti[i].BTemp = m.Clone().ToBitmap();
                        }

                       
                        
                         ResultMulti[i].RotCalib = rot.Clone();
                        ResultMulti[i].rotAdj = rot.Clone();

                      
                        i++;
                    }


                 
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
        public bool CheckBaclk(Mat matCrop,int i)
        {
            using (Py.GIL())
            {
                PyObject result = null;
                PyObject boxes = null;
                PyObject scores = null;
                PyObject labels = null;

                try
                {


                    // === Crop ROI ===


                    if (matCrop.Empty()) 
                        return false;

                    if (matCrop.Type().Depth != MatType.CV_8U)
                    {
                        using (var tmp8u = new Mat())
                        {
                            Cv2.ConvertScaleAbs(matCrop, tmp8u); // 16U/32F -> 8U
                            matCrop.AssignTo(tmp8u);             // ghi đè dữ liệu (OpenCvSharp: AssignTo giữ shape/type mới)
                        }
                    }

                    // 2) đảm bảo đúng số kênh
                    if (matCrop.Channels() == 1)
                    {
                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                    }
                    else if (matCrop.Channels() == 4)
                    {
                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGRA2BGR);
                    }


                    // nếu đã 3 kênh BGR thì giữ nguyên

                    int h = matCrop.Rows;
                    int w = matCrop.Cols;
                    int ch = matCrop.Channels(); // 3
                    int stride = (int)matCrop.Step(); // bytes/row (có thể > w*ch)
                    IntPtr p = matCrop.Data;

                    float conf = (float)(Common.PropetyTools[IndexThread][Index].Score / 100.0);
                    string toolName = Common.PropetyTools[IndexThread][Index].Name.Trim()+i;

                    // === Gọi YOLO (nhận: (boxes, scores, labels)) ===
                    // Ký hiệu: result là tuple-like (3 phần)
                    dynamic dyn = G.objYolo;
                    if (dyn == null)
                        throw new InvalidOperationException("objYolo chưa được khởi tạo.");

                    result = dyn.predict((long)p, h, w, ch, stride, conf, toolName);

                    // Ép về PyObject để chủ động Dispose
                    boxes = (PyObject)result[0];
                    //scores = (PyObject)result[1];
                    //labels = (PyObject)result[2];

                    int n = (int)boxes.Length();
                    if (n > 0)
                        return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }

               
            }
            return false;
        }
        [NonSerialized]
        List<ResultItem> ResultItems = new List<ResultItem>();
        [NonSerialized]
        public bool IsDone = false;
        public async void RunMode(RectRotate rectRotate)
        {
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            rectRotates = new List<RectRotate>();
            listScore = new List<double>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();

            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (raw.Empty()) return;

                Mat matBlack = new Mat();

                try
                {
                    //RectRotate rotAuto = new RectRotate(new RectangleF(-raw.Width / 2, -raw.Height / 2, raw.Width, raw.Height), new PointF(raw.Width / 2, raw.Height / 2), 0, AnchorPoint.None);
                    //Mat gray = Cropper.CropRotatedRect(raw, rotAuto, null);
                    //rectRotate = CheckPage(gray);//rotArea= 
                    Mat crop = Cropper.CropRotatedRect(raw, rectRotate, null);
                    ResultItems = new List<ResultItem>();
                    bool IsBlack = false;
                    int onnxOnce = 0;
                     matBlack = crop.Clone();
                    if (crop.Type() == MatType.CV_8UC3)
                        Cv2.CvtColor(crop, crop, ColorConversionCodes.BGR2GRAY);
                    int l = Math.Min(list_Patterns.Count, ResultMulti.Count)+1;

                    IntPtr data = crop.Data;
                    int step = (int)crop.Step();
                    int elem = crop.ElemSize();
                    int ch = crop.Channels();

                    MatType type =
                        (ch == 1) ? MatType.CV_8UC1 :
                        (ch == 3) ? MatType.CV_8UC3 :
                                    MatType.CV_8UC4;
                    // CHỈ 1 thread vào được đây (thread nào cũng có thể là i nào)
                  

   

                        
                        //Cv2.ImWrite($"Temp\\Raw.png", crop);
                        Parallel.For(0, l,
                       new ParallelOptions { MaxDegreeOfParallelism = Math.Min(300, Environment.ProcessorCount) },
                       i =>
                       {
                           if (IsBlackDot && System.Threading.Interlocked.CompareExchange(ref onnxOnce, 1, 0) == 0)
                           {
                               try
                               {
                                   int countDetect = NativeOnnx.Detect(
                                       matBlack.Data, matBlack.Width, matBlack.Height, (int)matBlack.Step(),
                                       0.1f, 0.9f, OnnxBoxes);

                                   if (countDetect > 0) IsBlack = true;

                                   foreach (var box in OnnxBoxes)
                                   {
                                       if (box.score == 0) continue;

                                       string name = (ListNameOnnx == null) ? "unknown"
                                           : (ListNameOnnx.TryGetValue(box.classId, out var s) ? s : "unknown");

                                       var item = new BeeCore.ResultItem(name);
                                       item.rot = NativeYolo.YoloBoxToRectRotate(box);
                                       item.Score = box.score * 100f;
                                       item.IsOK = true;

                                       lock (ResultItems) ResultItems.Add(item); // thread-safe
                                   }
                               }
                               catch (Exception ex) { Console.WriteLine(ex.Message); }
                           }
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

                               list_Patterns[i].SetRawNoCrop(
                                               ptr,
                                               w0,
                                               h0,
                                               step,
                                               ch
                                           );
                               var rot = list_Patterns[i].Match(
                                       IsHighSpeed,
                                       0,
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

                               rm.IsDot = false;
                               if (rot == null || rot.Count == 0)
                               {
                                   rm.Score = 0;
                                   rm.RotCheck = null;

                               }
                               else
                               {
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
                    if(matBlack!=null)
                        matBlack.Dispose();

                    if (matProcess != null)
                        matProcess.Dispose();
                }
            }
        }
        public String pathBlackDot = "";


        public ModeCalibVisualMatch ModeCalibVisualMatch = ModeCalibVisualMatch.Normal;

        public void DoWork(RectRotate rectRotate, RectRotate rotMask)
        {
            if (Global.IsAutoTemp)
            {
                IsCalibs = true;
                EditMode(rectRotate);
                RunMode(this.rotArea);
                rotAreaAdjustment = this.rotArea;
            }
           else if (Global.IsRun)
            {
                RunMode(rectRotate);
            }
            else if (!Global.IsRun)
            {
                if (IsCalibs)

                {
                    EditMode(rectRotate);
                }
                else
                    RunMode(rectRotate);
            }
        }
        public bool IsBlackDot=false;
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
                if (rs.IsDot)
                    rs.IsOK = false;
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
            if(ResultItems.Count > 0)
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;

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
            if (!Global.IsRun&IsCalibs)
            {
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotBot._PosCenter.X, rotBot._PosCenter.Y);
                gc.Transform = mat;
                gc.DrawRectangle(new Pen(Color.Red, 1), Rectangle.Round(rotBot._rect));


                mat.Translate(-rotBot._PosCenter.X, -rotBot._PosCenter.Y);
                mat.Translate(rotTop._PosCenter.X, rotTop._PosCenter.Y);
                gc.Transform = mat;
                gc.DrawRectangle(new Pen(Color.Red, 1), Rectangle.Round(rotTop._rect));
                mat.Translate(-rotTop._PosCenter.X, -rotTop._PosCenter.Y);
                mat.Translate(rotLeft._PosCenter.X, rotLeft._PosCenter.Y);
                gc.Transform = mat;
                gc.DrawRectangle(new Pen(Color.Red, 1), Rectangle.Round(rotLeft._rect));
                mat.Translate(-rotLeft._PosCenter.X, -rotLeft._PosCenter.Y);
                mat.Translate(rotRigth._PosCenter.X, rotRigth._PosCenter.Y);
                gc.Transform = mat;
                gc.DrawRectangle(new Pen(Color.Red, 1), Rectangle.Round(rotRigth._rect));
                mat = new Matrix();
                gc.ResetTransform();
            }
            // 
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
            mat.Translate(rectPage._PosCenter.X, rectPage._PosCenter.Y);
            mat.Rotate(rectPage._rectRotation);
            gc.Transform = mat;
           // Draws.Box1Label(gc, rectPage, "Page", new Font("Arial", Global.ParaShow.FontSize), new SolidBrush(Global.ParaShow.TextColor), Color.Red, Global.ParaShow.ThicknessLine);
            gc.ResetTransform();
            mat = new Matrix();
            if (!Global.IsRun&& IsCalibs)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);

                gc.Transform = mat;
                if(LineBot.Found)
                    gc.DrawLine(new Pen(Brushes.Green, 2), LineBot.X1, LineBot.Y1, LineBot.X2, LineBot.Y2);
                if (LineTop.Found)
                    gc.DrawLine(new Pen(Brushes.Blue, 2), LineTop.X1, LineTop.Y1, LineTop.X2, LineTop.Y2);
                if (LineLeft.Found)
                    gc.DrawLine(new Pen(Brushes.Blue, 2), LineLeft.X1, LineLeft.Y1, LineLeft.X2, LineLeft.Y2);
                if (LineRight.Found)
                    gc.DrawLine(new Pen(Brushes.Blue, 2), LineRight.X1, LineRight.Y1, LineRight.X2, LineRight.Y2);

                gc.ResetTransform();
            }

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

                        //Draws.Box2Label(gc, rs.RotOrigin._rect, "", "", font, Color.Yellow, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);
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


            foreach (ResultItem rs in ResultItems)
            {
                Color clShow = Global.ParaShow.ColorNG;
                
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

                mat.Translate(rs.rot._PosCenter.X, rs.rot._PosCenter.Y);
                mat.Rotate(rs.rot._rectRotation);
                gc.Transform = mat;
                if (Global.ParaShow.IsShowPostion)
                {
                    int min = (int)Math.Min(rs.rot._rect.Width / 4, rs.rot._rect.Height / 4);
                    Draws.Plus(gc, 0, 0, min, cl, Global.ParaShow.ThicknessLine);
                    String sPos = "X,Y,A _ " + rs.rot._PosCenter.X + "," + rs.rot._PosCenter.Y + "," + Math.Round(rs.rot._rectRotation, 1);

                    gc.DrawString(sPos, font, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 5));

                }


                //  gc.Transform = mat;

                if (!Global.IsRun || Global.ParaShow.IsShowDetail)
                    if (rs.matProcess != null && !rs.matProcess.Empty())
                    {
                        Draws.DrawMatInRectRotateNotMatrix(gc, rs.matProcess, rs.rot, clShow, Global.ParaShow.Opacity / 100.0f);

                    }
                font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
                String label = rs.Name;
                String valueScore = Math.Round(rs.Score, 1) + "%";
                if (!Global.ParaShow.IsShowScore) valueScore = "";
                if (!Global.ParaShow.IsShowLabel) label = "";
                Draws.Box3Label(gc, rs.rot._rect, label, valueScore, (int)(rs.Area / 100) + "px", font, clShow, brushText, 30, Global.ParaShow.ThicknessLine, Global.ParaShow.FontSize, 1, false);//("+Math.Round( ResultItem[i].Percent) + "%)
                gc.ResetTransform();

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
