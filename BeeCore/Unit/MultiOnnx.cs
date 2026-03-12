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
using System.Runtime.InteropServices.WindowsRuntime;
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
using static System.Windows.Forms.MonthCalendar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Point = OpenCvSharp.Point;
using ShapeType = BeeGlobal.ShapeType;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class MultiOnnx
    {
        public object Clone()
        {
            return this.MemberwiseClone();
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
            //if (IsSendResult)
            //{
            //    if (Global.Comunication.Protocol.IsConnected)
            //    {
            //        int i = 0;
            //        int Add = (int)Converts.StringtoDouble(AddPLC);
            //        String sAdd = Converts.BeforeFirstDigit(AddPLC);
            //        foreach (System.Drawing.Point point in listP_Center)
            //        {
            //            String Address = sAdd + Add;
            //            float[] floats = new float[4] { point.X, point.Y, list_AngleCenter[i], (float)listScore[i] };
            //            await Global.Comunication.Protocol.WriteResultFloatArr(AddPLC, floats);
            //            Add += 8;
            //            i++;
            //        }
            //    }
            //}
        }
        public bool IsAreaWhite = false;

   
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
      
        
        private int numOK;
      
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
      


        public List<ResultMulti> ResultMulti = new List<ResultMulti>();//note
        public MultiOnnx()
        {

        }
        [NonSerialized]
        Mat matProcess = new Mat();
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
        private NativeYolo NativeOnnx2;
        [NonSerialized]
        private NativeYolo.YoloBox[] OnnxBoxes;
        [NonSerialized]
        private NativeYolo.YoloBox[] OnnxBoxes2;
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
            if (OffSetBoxLine == 0) OffSetBoxLine = 100;
            if (rotCrop == null) rotCrop = new RectRotate();
            if (rotArea == null) rotArea = new RectRotate();
            
            if (ExpandX == 0) ExpandX = 50;
            if (ExpandY == 0) ExpandY = 50;

            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;

            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            if (Common.PropetyTools[IndexThread][Index].Score == 0)
                Common.PropetyTools[IndexThread][Index].Score = 80;
           
            if (ResultMulti == null)
                ResultMulti = new List<ResultMulti>();
         
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.NotInitial;
            //    if (pathFullModel != null)
                {
                    //if (pathFullModel.Trim().Contains(".pth"))
                    //{
                    //    TypeYolo = TypeYolo.RCNN;

                    //}
                    //else if (pathFullModel.Trim().Contains(".pt"))
                    //{
                    //    TypeYolo = TypeYolo.YOLO;

                    //}
                    //else
                    //{
                    //    TypeYolo = TypeYolo.YOLO;
                    //    Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                    //    return;
                    //}
                                try
                                {
                    if (pathChipOnnx != "")
                    {
                        String pathChip = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathChipOnnx);

                        int NumThreadCPU = 16;
                        if (File.Exists(pathChip + "\\best.xml"))
                        {
                            NativeOnnx2= new NativeYolo(pathChip + "\\best.xml", 0, 0, NumThreadCPU);

                            NativeOnnx2.Warmup(10);
                            OnnxBoxes2 = new NativeYolo.YoloBox[80];
                            TypeYolo = TypeYolo.Onnx;
                            ListNameOnnx = NativeOnnx2.LoadNames(pathChip + "\\metadata.yaml");
                        }
                        else
                            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
                    }

                                }
                                catch (Exception ex)
                                {
                                    Global.LogsDashboard?.AddLog(
                            new LogEntry(DateTime.Now, LeveLLog.ERROR, "IniBlack", ex.ToString()));

                                }


                        try
                        {
                            String pathBlack = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathBlackDot);

                            int NumThreadCPU = 16;
                            if (File.Exists(pathBlack + "\\best.xml"))
                            {
                                NativeOnnx = new NativeYolo(pathBlack + "\\best.xml", 0, 0, NumThreadCPU);

                                NativeOnnx.Warmup(10);
                                OnnxBoxes = new NativeYolo.YoloBox[20];
                                TypeYolo = TypeYolo.Onnx;
                                // ListNameOnnx = NativeOnnx.LoadNames(pathBlack + "\\metadata.yaml");
                            }
                            else
                                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;

                        }
                        catch (Exception ex)
                        {
                            Global.LogsDashboard?.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR, "IniBlack", ex.ToString()));

                        }
                Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;





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
        public int WidthDetectBoxBR = 300;
        public bool IsLimitCouter = true;
        public float ExpandX = 20, ExpandY = 20;
        public float LimitX = 1;
        public float LimitXSub = 1;
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
        RectRotate rotBotCalib;
        RectRotate rotRightCalib;
        [NonSerialized]
        Line2D line1, line2, line3, line4;
        RectRotate rectPage = new RectRotate();
        public float AspectLen = 0.6f;

      public Mat  Processing(Mat raw,MethordEdge methord,float thresh=0.98f)
        {
             using (Mat m = raw)
                {
                    switch (methord)
                    {
                        case MethordEdge.CloseEdges:
                            return Filters.EdgeAnyAngleFast(m);
                        break;
                    case MethordEdge.Binary:
                       return  Filters.Threshold(m, ThresholdBinary, ThresholdTypes.Binary);
                        break;
                    case MethordEdge.StrongEdges:
                            return Filters.GetStrongEdgesOnly(m, thresh);
                            break;
                    }
                }
           
            
            return raw;
        }
        public int OffSetBoxLine=50;
        public CornerAdj CornerAdj;
        public int OffSetBR = 1;
        // public int space = 500;
        public RectRotate CheckPage(Mat raw)
        {
            LineDirectionMode lineDirectionMode = LineDirectionMode.Horizontal;
           
            int W = raw.Width;
            int H = raw.Height;
            RansacIterations = 2000;
            
            if (raw.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);
            rotBot = new RectRotate(new RectangleF(-WidthDetectBoxBR / 2f , -H / 4f, WidthDetectBoxBR, H / 2f), new PointF(W / 2 + OffSetBR, 3 * H / 4f), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotBot, null))
            {

                Mat Edge = Processing(crop, MethordEdge.StrongEdges);
                //  Edge = Filters.Morphology(Edge, MorphTypes.Open, new Size(7, 7));
             //   Cv2.ImWrite("Bot.png", Edge);
                LineBot = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Horizontal, LineScanMode.BottomToTop, 0, 40
                );

            }
            OffsetLine(ref LineBot, rotBot);
           
            rotTop = new RectRotate(new RectangleF(-WidthDetectBox / 2f, -H / 4f, WidthDetectBox, H / 2f), new PointF(W / 2f, H / 4f), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotTop, null))
            {

                Mat Edge = Processing(crop, MethordEdge.StrongEdges);
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
            OffsetLine(ref LineTop, rotTop);
               rotLeft = new RectRotate(new RectangleF(-W / 4f, -WidthDetectBox / 2f, W / 2f, WidthDetectBox), new PointF(W / 4f, H / 2), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotLeft, null))
            {

                Mat Edge = Processing(crop, MethordEdge.StrongEdges);
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
            OffsetLine(ref LineLeft, rotLeft);
          
            rotRigth = new RectRotate(new RectangleF(-W / 4f, -WidthDetectBoxBR / 2f , W / 2f, WidthDetectBoxBR), new PointF(3 * W / 4f, H / 2 ), 0, AnchorPoint.None);
            using (Mat crop = Cropper.CropRotatedRect(raw, rotRigth, null))
            {
                //   Cv2.ImWrite("Crop3.png", crop);
                Mat Edge = Processing(crop, MethordEdge.StrongEdges);
              //   Edge = Filters.Morphology(Edge, MorphTypes.Open, new Size(7, 7));
             // Cv2.ImWrite("Right3.png", Edge);
                LineRight = RansacLine.FindBestLine(
                    Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Vertical, LineScanMode.RightToLeft, 0, 40
                );

            }
            OffsetLine(ref LineRight, rotRigth);
            if (LineLeft.Found && LineTop.Found && LineRight.Found && LineBot.Found)
            {

            
           
            line1 = new Line2D(LineTop.Vx, LineTop.Vy, LineTop.X0, LineTop.Y0);
            line2 = new Line2D(LineBot.Vx, LineBot.Vy, LineBot.X0, LineBot.Y0);
            line3 = new Line2D(LineLeft.Vx, LineLeft.Vy, LineLeft.X0, LineLeft.Y0);
            line4 = new Line2D(LineRight.Vx, LineRight.Vy, LineRight.X0, LineRight.Y0);
                rotBotCalib = ShrinkRectByLine(rotBot, LineBot, OffSetBoxLine, true);
                rotRightCalib = ShrinkRectByLine(rotRigth, LineRight, OffSetBoxLine);
                rotBotCalib = FitRectInsideImageVer(rotBotCalib, new Size(W, H));
                rotRightCalib = FitRectInsideImageVer(rotRightCalib, new Size(W, H));
                rectPage = InsertLine.CreateRectRotate_BotAxis(line1, line2, line3, line4);
               if(CornerAdj==CornerAdj.Bottom)
                    rectPage = InsertLine.CreateRectRotate_FromBotRight(line2, line4, rectPage._rect.Width, rectPage._rect.Height);
                else if (CornerAdj == CornerAdj.Right)
                    rectPage = InsertLine.CreateRectRotate_FromRightBot(line2, line4, rectPage._rect.Width, rectPage._rect.Height);
                else if (CornerAdj == CornerAdj.MidBotRight)
                    rectPage = InsertLine.CreateRectRotate_FromRightBot_VisionPro(line2, line4, rectPage._rect.Width, rectPage._rect.Height);

                pInsert = InsertLine.pInsert;
                RectRotate rtReturn = rectPage.Clone();
            //rtReturn.OffsetPixels(-ExpandPage,- ExpandPage);

            return rtReturn;
            }
            else
                {
                return new RectRotate(); }
        }
      
        public bool IsHardNoise;
        public int ScoreYolo = 10;
        public RectRotate rotOrigin = new RectRotate();
        public List<ResultItem> CheckBoxOnnx( Mat raw,float Conf )
        {
            List<ResultItem> resultTemp = new List<ResultItem>();
            if(NativeOnnx2==null)
             return resultTemp;
            using (Mat matCrop = raw)
            {
             
                if (matCrop.Type() == MatType.CV_8UC1)
                    Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                int countDetect = NativeOnnx2.Detect(
                matCrop.Data,
                matCrop.Width,
                matCrop.Height,
                (int)matCrop.Step(),
                Conf,
                0.9f,true,
                OnnxBoxes2);
                foreach (NativeYolo.YoloBox box in OnnxBoxes2)
                {
                    if (box.score == 0) continue;
                    string name = "";
                    if (ListNameOnnx == null)
                        name = "unknown";
                    else
                        name = ListNameOnnx.TryGetValue(box.classId, out var s) ? s : "unknown";
                    
                    resultTemp.Add(new BeeCore.ResultItem((name)));
                    RectRotate rot = NativeYolo.YoloBoxToRectRotate(box);
                    resultTemp[resultTemp.Count - 1].rot = rot;
                    resultTemp[resultTemp.Count - 1].Score = (box.score) * 100f;
                    resultTemp[resultTemp.Count - 1].IsOK = true;
                    resultTemp[resultTemp.Count - 1].Area = rot._rect.Width * rot._rect.Height;
                }

            }
            resultTemp.RemoveAll(item => item.Area < minArea);
      //      resultTemp = ResultFilter.FilterRectRotate(resultTemp, 0.6f);
            resultTemp = ResultItemHelper.SortByCenterXY(resultTemp);

            return resultTemp;
          
        }
       
        public PointF pInsert;
        public PointF pInsert1;
        public void EditMode(RectRotate rectRotate)
        {
            if (ResultItems != null)
                ResultItems.Clear();
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
           
            rectRotates = new List<RectRotate>();
           
           
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
                   
                    Mat gray = Cropper.CropRotatedRect(raw.Clone(), rotAuto, null);
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
                     
                        rotArea = CheckPage(gray);//rotArea= 
                    }

                  //  Cv2.ImWrite("Raw.png", raw);
                    Mat matCheckCip = Cropper.CropRotatedRect(raw, rotArea,null);
                   // Mat matCheckCip = Cropper.CropRotatedRectUltraFast3(raw, rotArea);
                    if (matCheckCip.Channels() == 1)
                    {

                        Cv2.CvtColor(matCheckCip, matCheckCip, ColorConversionCodes.GRAY2BGR);
                        // Cv2.ImWrite("Gray.png", matCheckCip);
                    }
                    float conf = (float)((Common.PropetyTools[IndexThread][Index].Score*0.8) / 100.0);
                    ResultItemChips = CheckBoxOnnx(matCheckCip, conf);

                   
                  

                    int i = 0;
                    
                 
                   
                    foreach (ResultItem rs in ResultItemChips)
                    {

                      
                        ResultMulti.Add(new BeeGlobal.ResultMulti());// rot, listMatTemp[i].ToBitmap(), null, listMatTempColor[i].ToBitmap(), null, null));
                        ResultMulti[i].RotOrigin  = rs.rot.Clone();
                        ResultMulti[i].RotCalib= rs.rot.Clone();
                        float w = (float)(ResultMulti[i].RotCalib._rect.Width * 2);
                        float h = (float)(ResultMulti[i].RotCalib._rect.Height * 2);
                        RectangleF rect = new RectangleF(-w / 2, -h / 2, w, h);
                        ResultMulti[i].RotCalib._rect= rect;
                        ResultMulti[i].Score = (rs.Score);
                        ResultMulti[i].IsOK = true;
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
        public static Mat CropRoiView(Mat src, RectRotate rot)
        {
            Rect roi = new Rect(new Point(rot._PosCenter.X - rot._rect.Width / 2, rot._PosCenter.Y - rot._rect.Height / 2),new Size(rot._rect.Width,rot._rect.Height));
            if (src == null || src.Empty()) return new Mat();
            return new Mat(src, roi); // view, dùng xong nhớ Dispose
        }
        public static PointF GetTopRightWorld(RectRotate r)
        {
            float w = r._rect.Width;
            float h = r._rect.Height;

            // local top-right
            float lx = +w / 2f;
            float ly = -h / 2f;

            float rad = (float)(r._rectRotation * Math.PI / 180.0);
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            // rotate local
            float rx = lx * cos - ly * sin;
            float ry = lx * sin + ly * cos;

            // world
            return new PointF(
                r._PosCenter.X + rx,
                r._PosCenter.Y + ry);
        }
        public static void MoveTopRightTo(RectRotate r, PointF target)
        {
            PointF tr = GetTopRightWorld(r);

            float dx = tr.X - target.X;
            float dy = tr.Y - target.Y;

            r._PosCenter = new PointF(
                r._PosCenter.X - dx,
                r._PosCenter.Y - dy);
        }
        public float ThreshStrongRight = 0.98f;
        public float AspectBox = 01f;
        static void OffsetLine(ref Line2DCli L, RectRotate rr)
        {
            float offX = rr._PosCenter.X - rr._rect.Width * 0.5f;
            float offY = rr._PosCenter.Y - rr._rect.Height * 0.5f;

            L.X1 += offX;
            L.Y1 += offY;
            L.X2 += offX;
            L.Y2 += offY;
            L.X0 += offX;
            L.Y0 += offY;
        }
        [NonSerialized]
        List<ResultItem> ResultItemChips = new List<ResultItem>();
        public static PointF ProjectPointToLine(PointF p, Line2DCli line)
        {
            float vx = line.Vx;
            float vy = line.Vy;

            float len = (float)Math.Sqrt(vx * vx + vy * vy);
            if (len < 1e-6f)
                return p;

            vx /= len;
            vy /= len;

            float dx = p.X - line.X0;
            float dy = p.Y - line.Y0;

            float t = dx * vx + dy * vy;

            return new PointF(
                line.X0 + t * vx,
                line.Y0 + t * vy);
        }
        public static RectRotate FitRectInsideImageVer(
      RectRotate rr,
      Size imgSize,
      float margin = 1f)
        {
            if (rr == null)
                return null;

            PointF c = rr._PosCenter;

            float left = rr._rect.Left;
            float right = rr._rect.Right;
            float top = rr._rect.Top;
            float bottom = rr._rect.Bottom;

            float newCx = c.X;
            float newCy = c.Y;

            // Biên thực tế của rect theo center hiện tại
            float imgLeft = c.X + left;
            float imgRight = c.X + right;
            float imgTop = c.Y + top;
            float imgBottom = c.Y + bottom;

            bool isOut =
                imgLeft < margin ||
                imgRight > imgSize.Width - margin ||
                imgTop < margin ||
                imgBottom > imgSize.Height - margin;

            // Nếu vẫn nằm hoàn toàn trong ảnh thì trả nguyên bản
            if (!isOut)
                return rr;

            // ===== Chỉnh center theo X =====
            if (imgLeft < margin)
                newCx += (margin - imgLeft);

            if (imgRight > imgSize.Width - margin)
                newCx -= (imgRight - (imgSize.Width - margin));

            // ===== Chỉnh center theo Y =====
            if (imgTop < margin)
                newCy += (margin - imgTop);

            if (imgBottom > imgSize.Height - margin)
                newCy -= (imgBottom - (imgSize.Height - margin));

            return new RectRotate(
                rr._rect,                         // giữ nguyên rect local
                new PointF(newCx, newCy),         // chỉ đổi center
                rr._rectRotation,
                AnchorPoint.None);
        }
        public static RectRotate FitRectInsideImage(
      RectRotate rr,
      Size imgSize)
        {
            PointF center = rr._PosCenter;

            float width = rr._rect.Width;
            float height = rr._rect.Height;

            float rad = rr._rectRotation * (float)Math.PI / 180f;

            float cos = Math.Abs((float)Math.Cos(rad));
            float sin = Math.Abs((float)Math.Sin(rad));

            // half extents sau rotate
            float halfW = width / 2f;
            float halfH = height / 2f;

            float boundX = cos * halfW + sin * halfH;
            float boundY = sin * halfW + cos * halfH;

            float maxLeft = center.X;
            float maxRight = imgSize.Width - center.X;

            float maxTop = center.Y;
            float maxBottom = imgSize.Height - center.Y;

            float maxX = Math.Min(maxLeft, maxRight);
            float maxY = Math.Min(maxTop, maxBottom);

            float scaleX = maxX / boundX;
            float scaleY = maxY / boundY;

            float scale = Math.Min(scaleX, scaleY);

            if (scale < 1f)
            {
                width *= scale;
                height *= scale;
            }

            return new RectRotate(
                new RectangleF(
                    -width / 2f,
                    -height / 2f,
                    width,
                    height),
                center,
                rr._rectRotation,
                AnchorPoint.None);
        }
        public static PointF[] GetCorners(RectRotate rr)
        {
            float cx = rr._PosCenter.X;
            float cy = rr._PosCenter.Y;

            float w = rr._rect.Width / 2f;
            float h = rr._rect.Height / 2f;

            float rad = rr._rectRotation * (float)Math.PI / 180f;

            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            PointF[] pts = new PointF[4];

            pts[0] = Rotate(-w, -h);
            pts[1] = Rotate(w, -h);
            pts[2] = Rotate(w, h);
            pts[3] = Rotate(-w, h);

            for (int i = 0; i < 4; i++)
            {
                pts[i].X += cx;
                pts[i].Y += cy;
            }

            return pts;

            PointF Rotate(float x, float y)
            {
                return new PointF(
                    x * cos - y * sin,
                    x * sin + y * cos
                );
            }
        }
        public static RectRotate ShrinkRectByLine(
    RectRotate src,
    Line2DCli line,
    float newWidth,bool IsHorial=false)
        {
            // center cũ
            PointF center = src._PosCenter;

            // center mới nằm trên line detect
            PointF newCenter = ProjectPointToLine(center, line);

            float vx = line.Vx;
            float vy = line.Vy;

            float len = (float)Math.Sqrt(vx * vx + vy * vy);
            if (len < 1e-6f)
                return src;

            vx /= len;
            vy /= len;

            // góc của rect = hướng line
            float angle = (float)(Math.Atan2(vy, vx) * 180.0 / Math.PI);

            float height = src._rect.Height;
            if(IsHorial)
            {
                return new RectRotate(
              new RectangleF(
                  - src._rect.Width / 2f,
                  -newWidth / 2f,
                   src._rect.Width,
                  newWidth),
              newCenter,
              angle,
              AnchorPoint.None);
            }    
            return new RectRotate(
                new RectangleF(
                    -newWidth / 2f,
                    -height / 2f,
                    newWidth,
                    height),
                newCenter,
                angle,
                AnchorPoint.None);
        }
        public static int FindIndex(
    ResultItem blue,
    List<ResultMulti> greens)
        {
            PointF c = blue.rot._PosCenter;

            for (int i = 0; i < greens.Count; i++)
            {
                RectangleF g = new RectangleF(
                    greens[i].RotCalib._PosCenter.X + greens[i].RotCalib._rect.Left,
                    greens[i].RotCalib._PosCenter.Y + greens[i].RotCalib._rect.Top,
                    greens[i].RotCalib._rect.Width,
                    greens[i].RotCalib._rect.Height);

                if (g.Contains(c))
                    return i;
            }

            return -1;
        }
        public async void RunMode(RectRotate rectRotate)
        {
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            rectRotates = new List<RectRotate>();
         
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            if (ResultItems != null)
            ResultItems.Clear();
         
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (raw.Empty()) return;

          

                try
                {
                    
                    {
                      
                        rectRotate = rotArea.Clone(); ;
                        rotAreaAdjustment = rotArea.Clone();
                        int W = raw.Width;
                        int H = raw.Height;
                        RansacThreshold = 1;
                        RansacIterations = 200;
                        LineRight = new Line2DCli();
                        LineRight.Found = false;
                        LineBot = new Line2DCli();

                        LineBot.Found = false;
                        Parallel.Invoke(
                            new ParallelOptions
                            {
                                MaxDegreeOfParallelism = 2
                            },

                            // ===== TOP =====
                            () =>
                            {

                            //    rotBot = new RectRotate(new RectangleF(-WidthDetectBoxBR / 2f, -H / 4f, WidthDetectBoxBR, H / 2f), new PointF(W / 2 + OffSetBR, 3 * H / 4f), 0, AnchorPoint.None);
                             
                              
                                //rotBot = new RectRotate(new RectangleF(-WidthDetectBoxBR / 2f + OffSetBR, -H / 4f, WidthDetectBoxBR, H / 2f), new PointF(W / 2 + OffSetBR, 3 * H / 4f), 0, AnchorPoint.None);
                                using (Mat crop1 = CropRoiView(raw, rotBotCalib))
                                using (Mat Edge = Processing(crop1, MethordEdge))
                                {


                                    LineBot = RansacLine.FindBestLine(
                                        Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                                        iterations: RansacIterations,
                                        threshold: (float)RansacThreshold,
                                        maxPoints: 120000,
                                        seed: Index,
                                        mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Horizontal, LineScanMode.BottomToTop, 0, 30
                                    );

                                }
                                OffsetLine(ref LineBot, rotBotCalib);

                            },

                            // ===== RIGHT =====
                            () =>
                            {
                                //  rotRigth = new RectRotate(new RectangleF(-W / 4f, -WidthDetectBoxBR / 2f, W / 2f, WidthDetectBoxBR), new PointF(3 * W / 4f, H / 2), 0, AnchorPoint.None);


                                //rotRigth = new RectRotate(new RectangleF(-W / 4f, -WidthDetectBoxBR / 4f, W / 2f, WidthDetectBoxBR), new PointF(3 * W / 4f, H / 2), 0, AnchorPoint.None);
                                using (Mat crop1 = CropRoiView(raw, rotRightCalib))
                                using (Mat Edge = Processing(crop1, MethordEdge, ThreshStrongRight))
                                {


                                    LineRight = RansacLine.FindBestLine(
                                        Edge.Data, Edge.Width, Edge.Height, (int)Edge.Step(),
                                        iterations: RansacIterations,
                                        threshold: (float)RansacThreshold,
                                        maxPoints: 120000,
                                        seed: Index,
                                        mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Vertical, LineScanMode.RightToLeft, 0, 30
                                    );

                                }
                                OffsetLine(ref LineRight, rotRightCalib);

                            }
                        );

                        if (LineBot.Found && LineRight.Found)
                        {

                            Line2D LineHorial = new Line2D(LineBot.Vx, LineBot.Vy, LineBot.X0, LineBot.Y0);
                            Line2D LineVertial = new Line2D(LineRight.Vx, LineRight.Vy, LineRight.X0, LineRight.Y0);
                            if (CornerAdj == CornerAdj.Right)
                                rotAreaAdjustment = InsertLine.CreateRectRotate_FromRightBot(LineHorial, LineVertial, rotAreaAdjustment._rect.Width, rotAreaAdjustment._rect.Height);
                            else if (CornerAdj == CornerAdj.Bottom)
                                rotAreaAdjustment = InsertLine.CreateRectRotate_FromBotRight(LineHorial, LineVertial, rotAreaAdjustment._rect.Width, rotAreaAdjustment._rect.Height);
                            else if (CornerAdj == CornerAdj.MidBotRight)
                                rotAreaAdjustment = InsertLine.CreateRectRotate_FromRightBot_VisionPro(LineHorial, LineVertial, rotAreaAdjustment._rect.Width, rotAreaAdjustment._rect.Height);


                            rectRotate = rotAreaAdjustment;
                            if (!Global.IsRun)
                            {
                                pInsert = InsertLine.pInsert;
                                rotArea = rectRotate;
                            }
                            else
                                pInsert1 = InsertLine.pInsert;

                        }


                    }
                    ResultItemChips = new List<ResultItem>();

                    using (Mat matBlack = Cropper.CropRotatedRectUltraFast3(raw, rectRotate))
                    {

                        ResultItems = new List<ResultItem>();

                        int onnxOnce = 0;
                        //matBlack = crop.Clone();
                        if (matBlack.Channels() == 1)
                        {

                            Cv2.CvtColor(matBlack, matBlack, ColorConversionCodes.GRAY2BGR);
                        }
                        Mat matChip = matBlack.Clone();
                       
                        bool IsBlack = false;
                        Parallel.Invoke(
                              new ParallelOptions
                              {
                                  MaxDegreeOfParallelism = 2
                              },

                              // ===== BlackDot =====
                              () =>
                              {
                                  if (IsBlackDot && NativeOnnx != null)
                                  {
                                      try
                                      {
                                          float Score = (float)(ScoreYolo / 100.0);
                                          int countDetect = NativeOnnx.Detect(
                                              matBlack.Data, matBlack.Width, matBlack.Height, (int)matBlack.Step(),
                                              Score, 0.9f,true, OnnxBoxes);

                                          if (countDetect > 0) IsBlack = true;

                                          foreach (var box in OnnxBoxes)
                                          {
                                              if (box.score == 0) continue;

                                              string name = "blackdot";
                                              var item = new BeeCore.ResultItem(name);
                                              item.rot = NativeYolo.YoloBoxToRectRotate(box);
                                              item.Score = box.score * 100f;
                                              item.IsOK = true;

                                              lock (ResultItems) ResultItems.Add(item); // thread-safe
                                          }
                                      }
                                      catch (Exception ex)
                                      {
                                          Global.LogsDashboard?.AddLog(
                               new LogEntry(DateTime.Now, LeveLLog.ERROR, "PatternBlack", ex.ToString()));
                                      }
                                  }
                              },

                                // ===== RIGHT =====
                                () =>
                                {
                                    float conf = (float)((Common.PropetyTools[IndexThread][Index].Score ) / 100.0);
                                 
                                    ResultItemChips = CheckBoxOnnx(matChip, conf);

                                    if (ResultMulti.Count() == ResultItemChips.Count())
                                    {


                                        for (int i = 0; i < ResultMulti.Count; i++)
                                        {
                                            {
                                                ResultMulti[i].IsOK = true;
                                                ResultMulti[i].RotCheck = ResultItemChips[i].rot;
                                                if (ResultMulti[i].RotOrigin != null && ResultMulti[i].RotCheck != null)
                                                {
                                                    ResultMulti[i].AspectW = Math.Abs(ResultMulti[i].RotCheck._rect.Width - ResultMulti[i].RotOrigin._rect.Width) / ResultMulti[i].RotOrigin._rect.Width;

                                                    ResultMulti[i].AspectH = Math.Abs(ResultMulti[i].RotCheck._rect.Height - ResultMulti[i].RotOrigin._rect.Height) / ResultMulti[i].RotOrigin._rect.Height;
                                                    ResultMulti[i].IsAspect = false;
                                                    if (ResultMulti[i].AspectW >= AspectBox || ResultMulti[i].AspectH >= AspectBox)
                                                        ResultMulti[i].IsAspect = true;
                                                    ResultMulti[i].Score = ResultItemChips[i].Score;

                                                    if (!Global.IsRun || Global.IsAutoTemp || IsCalibs)
                                                        ResultMulti[i].RotOrigin = ResultMulti[i].RotCheck;
                                                    ResultMulti[i].deltaX = (float)Math.Round(
                                                             (ResultMulti[i].RotCheck._PosCenter.X - ResultMulti[i].RotOrigin._PosCenter.X) / Scale, 1);//pixel to mm
                                                    ResultMulti[i].deltaY = (float)Math.Round(
                                                            (ResultMulti[i].RotCheck._PosCenter.Y - ResultMulti[i].RotOrigin._PosCenter.Y) / Scale, 1);//pixel to mm
                                                }
                                                else
                                                {
                                                    ResultMulti[i].deltaX = 100;
                                                    ResultMulti[i].deltaY = 100;
                                                }
                                            }
                                        }
                                    }
                                    else if (ResultMulti.Count() > ResultItemChips.Count())
                                    {
                                        Dictionary<int, RectRotate> map = new Dictionary<int, RectRotate>();
                                        foreach (var rs in ResultMulti)
                                        {
                                          
                                            rs.IsOK = false;
                                            rs.Score = 0;
                                        }
                                            foreach (var blue in ResultItemChips)
                                        {
                                            int idx = FindIndex(blue, ResultMulti);

                                            if (idx>-1)
                                            {
                                             
                                                   ResultMulti[idx].IsOK = true;
                                                ResultMulti[idx].Score = blue.Score;
                                            }    
                                               
                                        }
                                    }    
                                });
                       
                    
                      
                    }
                              
                 

                   
                 

                }
                catch (Exception ex)
                {
                    Global.LogsDashboard?.AddLog(
                        new LogEntry(DateTime.Now, LeveLLog.ERROR, "Pattern", ex.Message.ToString()));
                }
                finally
                {
                 

                    if (matProcess != null)
                        matProcess.Dispose();
                }
            }
        }
        public String pathBlackDot = "";
        public String pathChipOnnx = "";

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
        private bool IsFailLine = false;
        public void Complete()
        {
            if (LineBot.Found && LineRight.Found)
                IsFailLine = false;
            else
                IsFailLine = true;
                bool IsNG = false;
            ValueCompare = 0;
            foreach (ResultMulti rs in ResultMulti)
            {
                if (!rs.IsOK)
                { IsNG = true; 
                    continue; }    
                if (rs.IsAspect )
                {
                    IsNG = true;
                    rs.IsOK = false;
                   
                }
                else if (rs.deltaX> LimitX)
                {

                    IsNG = true;
                    rs.IsOK = false;
                }
                else if (rs.deltaX < -LimitXSub)
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
            if (ResultItemChips == null)
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            else
            {
                if (ResultItemChips.Count() == ResultMulti.Count && !IsNG)
                {
                    Common.PropetyTools[IndexThread][Index].Results = Results.OK;
                }

                else
                {
                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                }
                if (ResultItems != null)
                    if (ResultItems.Count > 0)
                        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }

           

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
           
                mat = new Matrix();
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                gc.Transform = mat;
            if (pInsert != null)
                Draws.Plus(gc, (int)pInsert.X, (int)pInsert.Y, 50, Color.Yellow, 4);
            if(pInsert1!=null)
            Draws.Plus(gc, (int)pInsert1.X, (int)pInsert1.Y, 50, Color.Green, 4);
            if (!Global.IsRun)
            {
                if (rotLeft != null)
                {
                    mat.Translate(rotLeft._PosCenter.X, rotLeft._PosCenter.Y);
                    gc.Transform = mat;
                    gc.DrawRectangle(new Pen(Color.Red, 1), Rectangle.Round(rotLeft._rect));
                    mat.Translate(-rotLeft._PosCenter.X, -rotLeft._PosCenter.Y);

                }
                if (rotTop != null)
                {
                    mat.Translate(rotTop._PosCenter.X, rotTop._PosCenter.Y);
                    gc.Transform = mat;
                    gc.DrawRectangle(new Pen(Color.Red, 1), Rectangle.Round(rotTop._rect));
                    mat.Translate(-rotTop._PosCenter.X, -rotTop._PosCenter.Y);

                }
            }

            if (rotBotCalib != null)
            {
                mat.Translate(rotBotCalib._PosCenter.X, rotBotCalib._PosCenter.Y);
                gc.Transform = mat;
                gc.DrawRectangle(new Pen(Global.ParaShow.ColorChoose, Global.ParaShow.ThicknessLine), Rectangle.Round(rotBotCalib._rect));


                mat.Translate(-rotBotCalib._PosCenter.X, -rotBotCalib._PosCenter.Y);
            }


            if (rotRightCalib != null)
            {
                mat.Translate(rotRightCalib._PosCenter.X, rotRightCalib._PosCenter.Y);
                gc.Transform = mat;
                gc.DrawRectangle(new Pen(Global.ParaShow.ColorChoose, Global.ParaShow.ThicknessLine), Rectangle.Round(rotRightCalib._rect));
            }


            mat = new Matrix();
                gc.ResetTransform();
            
            //if (Global.IsRun)
            {
             
                mat = new Matrix();
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                gc.Transform = mat;

              //  Draws.Plus(gc, (int)pInsert1.X, (int)pInsert1.Y, 10, Color.Yellow, 2);
             
                gc.Transform = mat;
                
                if (LineBot.Found)
                    gc.DrawLine(new Pen(Brushes.Blue, Global.ParaShow.ThicknessLine), LineBot.X1, LineBot.Y1, LineBot.X2, LineBot.Y2);
              
                if (LineRight.Found)
                    gc.DrawLine(new Pen(Brushes.Blue, Global.ParaShow.ThicknessLine), LineRight.X1, LineRight.Y1, LineRight.X2, LineRight.Y2);

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
            Brush brushText =new SolidBrush( Global.ParaShow.TextColor);
            Color cl = Color.LimeGreen;
            if (Common.PropetyTools[Global.IndexChoose][Index].Results == Results.NG)
            {
                cl = Global.ParaShow.ColorNG;
            }
            else
            {
                cl = Global.ParaShow.ColorOK;
            }
            if (ResultItemChips == null) ResultItemChips = new List<ResultItem>();
            String Content = ResultItemChips.Count() + "/" + ResultMulti.Count;
            if (IsFailLine)
            {
                Content += " (Fail Corner)";

            }    
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (Global.ParaShow.IsShowBox)
                Draws.Box2Label(gc, rotA, nameTool, Content, font, cl, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);
             gc.ResetTransform();


           
          

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
                //if (ResultMulti.Count() != ResultItemChips.Count())
                //    clPCs = Global.ParaShow.ColorNG;
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
              
                if (rs.RotOrigin == null)
                {
                    i++; continue;
                }
                mat.Translate(rs.RotOrigin._PosCenter.X, rs.RotOrigin._PosCenter.Y);
                    mat.Rotate(rs.RotOrigin._rectRotation);
                    gc.Transform = mat;
                     Draws.Plus(gc, 0, 0, 10, Color.Yellow, 2);
                //if(IsCalibs)
                // Draws.Box1Label(gc, rs.RotOrigin._rect, Math.Round(rs.Score) + "%", font, brushText, clPCs, Global.ParaShow.ThicknessLine);

                if (rs.RotCheck == null)
                    {
                    rs.RotCheck = rs.RotOrigin;

                    }
                if (rs.RotCheck._rect.Width == 0|| rs.RotCheck._rect.Height == 0)
                {
                    i++; continue;

                }
                mat.Translate(-rs.RotOrigin._PosCenter.X, -rs.RotOrigin._PosCenter.Y);
                    mat.Rotate(-rs.RotOrigin._rectRotation);
                    mat.Translate(rs.RotCheck._PosCenter.X, rs.RotCheck._PosCenter.Y);
                    mat.Rotate(rs.RotCheck._rectRotation);
                gc.Transform = mat;

                float w = (float)(rs.RotCheck._rect.Width * 1.2);
              
                float h = (float)(rs.RotCheck._rect.Height * 1.2);
               
                RectangleF rect = new RectangleF(-w / 2, -h / 2, w, h);
              
                Draws.Box2Label(gc, rect, i+ " ", Math.Round(rs.Score) + "%", font, clPCs, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);
                    Draws.Plus(gc, 0, 0, 10, clPCs, 2);
              
                    Font font1 = new Font("Arial", Global.ParaShow.FontSize/2);
                if (ResultMulti.Count() == ResultItemChips.Count())
                    gc.DrawString(rs.deltaX.ToString() + "\n" + rs.deltaY.ToString() + "\n" + Math.Round(rs.AspectW,2).ToString()+"-"+ Math.Round(rs.AspectH, 2).ToString(), font1, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 10));
                    gc.ResetTransform();
                    i++;
                }
                if (IsCalibs)

                {
                    IsCalibs = false;

                }

            if (ResultItemChips != null)
                foreach (ResultItem rs in ResultItemChips)
                {
                 
                    Color cl2 = Global.ParaShow.ColorInfor;
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
                    Draws.Box1Label(gc, rs.rot._rect, "", font, brushText, cl2, Global.ParaShow.ThicknessLine);

                    gc.ResetTransform();
                }
            if (ResultItems!=null)
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
             


                //  gc.Transform = mat;

                //if (!Global.IsRun || Global.ParaShow.IsShowDetail)
                //    if (rs.matProcess != null && !rs.matProcess.Empty())
                //    {
                //        Draws.DrawMatInRectRotateNotMatrix(gc, rs.matProcess, rs.rot, clShow, Global.ParaShow.Opacity / 100.0f);

                //    }
                font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
                String label = rs.Name;
                String valueScore = Math.Round(rs.Score) + "%";
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
