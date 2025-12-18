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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Markup;
using static BeeCore.Cropper;
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

        public int ValueCompare =0;
        public bool IsCalibs = false;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
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
        public List<Point> Postion=new List<Point>();
       private Mode _TypeMode=Mode.Pattern;
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
        List <Mat> list_matColor = new List <Mat>();
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
        public Bitmap bmRaw;
       
     
		public TypeCrop TypeCrop;
        public string pathRaw = "";
        public int cycleTime = 0;
        public RectangleF rectArea;
        public Compares Compare = Compares.Equal;
        public int LimitCounter =1;
        public bool IsSendResult;
        public async Task SendResult()
        {
            if (IsSendResult)
            {
                if (Global.ParaCommon.Comunication.Protocol.IsConnected)
                {
                    int i = 0; 
                    int Add=(int)Converts.StringtoDouble(AddPLC);
                    String sAdd = Converts.BeforeFirstDigit(AddPLC);
                    foreach (System.Drawing.Point point in listP_Center)
                    {
                        String Address = sAdd + Add;
                        float[] floats = new float[4] { point.X, point.Y, list_AngleCenter[i],(float) listScore[i] };
                        await Global.ParaCommon.Comunication.Protocol.WriteResultFloatArr(AddPLC, floats);
                        Add += 8;
                         i++;
                    }
                }
            }
        }
        public bool IsAreaWhite=false;
      
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
        private double _angle=10;
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
        [NonSerialized]
        public BeeCpp.Pattern Pattern =new BeeCpp.Pattern();
        [NonSerialized]//note
        public List<BeeCpp.Pattern> list_Patterns = new List<BeeCpp.Pattern>();
        [NonSerialized]//note
        public List<BeeCpp.ColorPixel> list_ColorPixel = new List<BeeCpp.ColorPixel>();

        public bool IsColorPixel;
        public float PxTemp = 1;
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
            if (ResultMulti==null)
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
                                   MaxDegreeOfParallelism = Math.Min(3, Environment.ProcessorCount)
                               },
                               i =>
                               {

                                   if (ResultMulti[i].BTemp != null)
                                   {
                                       matTemp = ResultMulti[i].BTemp.ToMat();
                                       list_Patterns[i].SetImgeSampleNoCrop(
                                             matTemp.Data,
                                             matTemp.Width,
                                             matTemp.Height,
                                             (int)matTemp.Step(),
                                             matTemp.Channels()
                                           
                                         );

                                       list_Patterns[i].LearnPattern();
                                      
                                   }
                               }
                           );
            Parallel.For(
                                 0, ResultMulti.Count,
                                 new ParallelOptions
                                 {
                                     MaxDegreeOfParallelism = Math.Min(3, Environment.ProcessorCount)
                                 },
                                 i =>
                                 {

                                     if (ResultMulti[i].BTempColor != null)
                                     {
                                         matTemp = ResultMulti[i].BTempColor.ToMat();
                                         list_ColorPixel[i].SetImgeSampleNoCrop(
                                               matTemp.Data,
                                               matTemp.Width,
                                               matTemp.Height,
                                               (int)matTemp.Step(),
                                               matTemp.Channels()

                                           );
                                     }
                                 }
                             );

            if (Scale == 0) Scale = 1;
            if (rotCrop == null) rotCrop = new RectRotate();
            if(rotArea == null) rotArea = new RectRotate();
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
      
        public  List<float>list_AngleCenter =new List<float>();
        public ZeroPos ZeroPos=ZeroPos.Zero;
        public float Scale = 1;
        public bool IsLimitCouter = true;
        public float ExpandX = 50, ExpandY = 50, ScalePixel =1;
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

                Mat matProcess = null;
                bool ownsMatProcess = false;

                try
                {
                
                      
                                matProcess = raw;

                                RectRotateCli rrCli = Converts.ToCli(rectRotate);
                                RectRotateCli? rrMaskCli =
                                    (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                                Pattern.SetImgeRaw(
                                    matProcess.Data,
                                    matProcess.Width,
                                    matProcess.Height,
                                    (int)matProcess.Step(),
                                    matProcess.Channels(),
                                    rrCli,
                                    rrMaskCli
                                );

                    if (matProcess == null || matProcess.Empty())
                        return;

                    GC.KeepAlive(matProcess);

                    var listRS = Pattern.Match(
                        IsHighSpeed,
                        StepAngle,
                        AngleLower,
                        AngleUper,
                        Common.PropetyTools[IndexThread][Index].Score / 100.0,
                        ckSIMD,
                        ckBitwiseNot,
                        ckSubPixel,
                        MaxObject,
                        OverLap,
                        false,
                        -1
                    );

                    if (listRS == null)
                        return;
                    if (listRS.Count == 0)
                        return;
                    if (list_Patterns == null)
                        list_Patterns = new List<BeeCpp.Pattern>();

                    float scoreSum = 0f;
                   
                   
                    int i = 0;
                    int count = listRS.Count();

                    list_Patterns = new List<BeeCpp.Pattern>(count);
                    list_ColorPixel = new List<ColorPixel>(count);

                    for (int k = 0; k < count; k++)
                    {
                        list_Patterns.Add(null);
                        list_ColorPixel.Add(null);
                    }
                    var listMatTemp = new Mat[listRS.Count()];
                    var listMatTempColor = new Mat[listRS.Count()];
                    foreach (Rotaterectangle rot in listRS)
                    {
                        scoreSum += (float)rot.Score;

                        float rwF = (float)rot.Width;
                        float rhF = (float)rot.Height;
                        float angle = (float)rot.AngleDeg;

                        var pCenter = new System.Drawing.PointF(
                            (float)rot.Cx, (float)rot.Cy);

                        RectRotate rr = new RectRotate(
                            new RectangleF(-rwF / 2f, -rhF / 2f, rwF, rhF),
                            pCenter,
                            angle,
                            AnchorPoint.None
                        );

                        rectRotates.Add(rr);
                        listScore.Add(Math.Round(rot.Score, 1));
                      

                        int xCenter = (int)(
                            rectRotate._PosCenter.X
                            - rectRotate._rect.Width / 2f
                            + pCenter.X);

                        int yCenter = (int)(
                            rectRotate._PosCenter.Y
                            - rectRotate._rect.Height / 2f
                            + pCenter.Y);

                        float angleCenter = rotArea._rectRotation + angle;

                        if (ZeroPos != ZeroPos.Zero)
                        {
                            angleCenter -= Global.rotOriginAdj._rectRotation;
                            xCenter -= (int)Global.rotOriginAdj._PosCenter.X;
                            yCenter -= (int)Global.rotOriginAdj._PosCenter.Y;
                        }

                        System.Drawing.Point centerScaled =
                            new System.Drawing.Point(
                                (int)(xCenter / Scale),
                                (int)(yCenter / Scale));

                        listP_Center.Add(centerScaled);
                        list_AngleCenter.Add(angleCenter);
                        BeeCpp.Pattern pat = new BeeCpp.Pattern();
                        list_Patterns[i] = pat;
                        RectRotateCli? rrMaskCli2 =
                            (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
                      
                        RectRotateCli rrCli2 = Converts.ToCli(rr);
                        RectRotateCli? rrMaskCliLocal2 = null;

                       

                        int w=0, h=0, s=0, c=0;
                      IntPtr intpr =  pat.SetImgeSample(
                            matProcess.Data,
                            matProcess.Width,
                            matProcess.Height,
                            (int)matProcess.Step(),
                            matProcess.Channels(),
                            rrCli2,
                            rrMaskCliLocal2,
                            false,
                            out w, out h, out s, out c

                        );
                        MatType mt = c == 1 ? MatType.CV_8UC1
                                  : c == 3 ? MatType.CV_8UC3
                                  : MatType.CV_8UC4;
                        using (var m = new Mat(h, w, mt, intpr, s))
                        {
                            listMatTemp[i] = m.Clone();
                            listMatTempColor[i]= m.Clone();
                            if (IsColorPixel)
                            {
                                BeeCpp.ColorPixel clpx = new BeeCpp.ColorPixel();
                                list_ColorPixel[i] = clpx;

                                if (listMatTempColor[i].Type() == MatType.CV_8UC1)
                                {
                                    Cv2.CvtColor(listMatTempColor[i], listMatTempColor[i],
                                                 ColorConversionCodes.GRAY2BGR);
                                }

                                clpx.SetImgeSampleNoCrop(
                                    listMatTempColor[i].Data,
                                    listMatTempColor[i].Width,
                                    listMatTempColor[i].Height,
                                    (int)listMatTempColor[i].Step(),
                                    listMatTempColor[i].Channels()
                                );
                            }
                            // Cv2.ImWrite($@"D:\temp\samp\img_{i}.png", listMatTempColor[i]);

                            i++;
                        }
                       
                        pat.LearnPattern();
                    }
                    
                    i = 0;
                   
                    foreach ( RectRotate rot in rectRotates)
                    {
                        rot.ExpandPixels(ExpandX, ExpandY);

                        ResultMulti.Add(new BeeGlobal.ResultMulti(rot, listMatTemp[i].ToBitmap(), null, listMatTempColor[i].ToBitmap(),null,null));
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

        private static RectRotate CalcRoiRect(Mat src, RectRotate rr)
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

                    int l = Math.Min(list_Patterns.Count, ResultMulti.Count);

                    IntPtr data = raw.Data;
                    int step = (int)raw.Step();
                    int elem = raw.ElemSize();
                    int ch = raw.Channels();

                    MatType type =
                        (ch == 1) ? MatType.CV_8UC1 :
                        (ch == 3) ? MatType.CV_8UC3 :
                                    MatType.CV_8UC4;

                    Parallel.For(0, l,
                            new ParallelOptions { MaxDegreeOfParallelism = Math.Min(3, Environment.ProcessorCount) },
                            i =>
                            {
                                var rm = ResultMulti[i];
                                if (rm == null) return;

                                RectRotate roiRR = CalcRoiRect(raw, rm.RotCalib);

                                int w0 = (int)Math.Round(roiRR._rect.Width);
                                int h0 = (int)Math.Round(roiRR._rect.Height);
                                int x = (int)Math.Round(roiRR._PosCenter.X - w0 * 0.5f);
                                int y = (int)Math.Round(roiRR._PosCenter.Y - h0 * 0.5f);

                                // clamp
                                if (x < 0) x = 0;
                                if (y < 0) y = 0;
                                if (x + w0 > raw.Width) w0 = raw.Width - x;
                                if (y + h0 > raw.Height) h0 = raw.Height - y;
                                if (w0 < 1 || h0 < 1) return;

                                int offset = y * step + x * elem;
                                IntPtr ptr = IntPtr.Add(data, offset);

                                list_Patterns[i].SetRawNoCrop(
                                    ptr,
                                    w0,
                                    h0,
                                    step,
                                    ch
                                );

                                Mat matCrop0 = new Mat(h0, w0, type, ptr, step).Clone();
                                if (matCrop0.Type() == MatType.CV_8UC1)
                                {
                                    Cv2.CvtColor(matCrop0, matCrop0, ColorConversionCodes.GRAY2BGR);
                                }

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

                                if (!Global.IsRun)
                                    rm.RotOrigin = rotCheck.Clone();

                                rm.RotCheck = rotCheck;

                                Mat matCrop1 = Cropper.CropRotatedRect(
                                    matCrop0, rotCheck, null);

                                if (IsColorPixel)
                                {
                                    Mat matChk = matCrop1;
                                    if (matChk.Type() == MatType.CV_8UC1)
                                    {
                                        Cv2.CvtColor(matChk, matChk, ColorConversionCodes.GRAY2BGR);
                                    }

                                    list_ColorPixel[i].SetRawNoCrop(
                                        matChk.Data,
                                        matChk.Width,
                                        matChk.Height,
                                        (int)matChk.Step(),
                                        matChk.Channels()
                                    );

                                    int s3, c3, w3, h3;
                                    IntPtr intpr = list_ColorPixel[i].CheckImageFromMat(
                                        false, 0, false,
                                        (int)PxTemp, 30,
                                        out pxRS,
                                        ref OffsetX,
                                        ref OffsetY,
                                        ref OffsetAngle,
                                        out w3,
                                        out h3,
                                        out s3,
                                        out c3
                                    );
                                    ResultMulti[i].ScoreColor = pxRS;
                                    if (intpr != IntPtr.Zero)
                                    {
                                        MatType mt =
                                            c3 == 1 ? MatType.CV_8UC1 :
                                            c3 == 3 ? MatType.CV_8UC3 :
                                                    MatType.CV_8UC4;

                                       
                                        if (intpr != IntPtr.Zero)
                                        {
                                            using (var m = new Mat(h3, w3, mt, intpr, s3))
                                            {
                                                ResultMulti[i].BCheckColor = m.ToBitmap();
                                            }
                                        }
                                    }
                                }

                                rm.Score = (float)Math.Round(r0.Score, 1);
                                var org = rm.RotOrigin;
                                var chk = rm.RotCheck;

                                if (org != null && chk != null)
                                {
                                    rm.deltaX = (float)Math.Round(
                                        (chk._PosCenter.X - org._PosCenter.X) * ScalePixel, 1);//pixel to mm

                                    rm.deltaY = (float)Math.Round(
                                        (chk._PosCenter.Y - org._PosCenter.Y) * ScalePixel, 1);//pixel to mm
                                }
                                else
                                {
                                    rm.deltaX = 0;
                                    rm.deltaY = 0;
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

        public void DoWork(RectRotate rectRotate)
        {
               if(Global.IsRun)
            {
                RunMode(rectRotate);
            }
            else
            {
                if (IsCalibs)

                { 
                    EditMode(rectRotate);
                }
                else
                    RunMode(rectRotate);
            }
            }
        public bool IsWrongColor = false;
        public void Complete()
        {
            foreach (ResultMulti rs in ResultMulti)
            {
                if (rs.ScoreColor > PxTemp)
                    IsWrongColor = true;
                if (rs.RotCheck == null)
                    continue;   
                ValueCompare++; 
            }
            Common.PropetyTools[IndexThread][Index].Results = Results.OK;

            if (ValueCompare == LimitCounter && !IsWrongColor)
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            }
               
            else
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }
            ValueCompare = 0;
            IsWrongColor = false;
        }
        public Graphics DrawResult(Graphics gc)
        {
			if (rotAreaAdjustment == null && Global.IsRun) return gc;
			if (Global.IsRun)
				gc.ResetTransform();

			RectRotate rotA = rotArea;
			if (Global.IsRun) rotA = rotAreaAdjustment;
			var mat = new Matrix();
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
				cl = Global.Config.ColorNG;
			}
			else
			{
				cl =  Global.Config.ColorOK;
			}
			String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox)
				Draws.Box1Label(gc,rotA, nameTool, font, brushText, cl, Global.Config.ThicknessLine);
			gc.ResetTransform();
			if (listScore == null) return gc;
			if (list_Patterns.Count > 0)
			{
               
                int i = 0;
               
                i = 0;
                foreach (ResultMulti rs in ResultMulti)
                {
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

                    Draws.Box2Label(gc, rot._rect, i + "", "", font, Color.Blue, brushText, Global.Config.FontSize, Global.Config.ThicknessLine, 10, 2);
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
                        Bitmap myBitmap = rs.BCheckColor;
                        myBitmap.MakeTransparent(Color.Black);
                        myBitmap = General.ChangeToColor(myBitmap, Color.Red, 0.3f);
                        gc.DrawImage(myBitmap, rs.RotCheck._rect);
                    }
                    Draws.Box2Label(gc, rs.RotCheck._rect, i + "", Math.Round(rs.Score, 1) + "%", font, cl, brushText, Global.Config.FontSize, Global.Config.ThicknessLine);
                    Draws.Plus(gc, 0, 0, 30, cl, 2);
                    Font font1 = new Font("Arial", Global.Config.FontSize);
                    gc.DrawString(rs.deltaX.ToString() + "," + rs.deltaY.ToString(), font1, new SolidBrush( Global.Config.ColorInfor), new PointF(5, 10));
                    if (!IsCalibs)
                    {
                        if (rs.RotOrigin == null) continue;
                        mat.Translate(-rs.RotCheck._PosCenter.X, -rs.RotCheck._PosCenter.Y);
                        mat.Rotate(-rs.RotCheck._rectRotation);
                        mat.Translate(rs.RotOrigin._PosCenter.X, rs.RotOrigin._PosCenter.Y);
                        mat.Rotate(rs.RotOrigin._rectRotation);
                        gc.Transform = mat;
                        
                        Draws.Box2Label(gc, rs.RotOrigin._rect, "", "", font, Color.Yellow, brushText, Global.Config.FontSize, Global.Config.ThicknessLine);
                        Draws.Plus(gc, 0, 0, 30, Color.Yellow, 2);
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

    //public void Matching( RectRotate rectRotate)
    //    {
    //        using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
    //        {

    //            if (raw.Empty()) return;

    //            Mat matCrop = Cropper.CropRotatedRect(raw, rectRotate, rotMask);
    //            Mat matProcess = new Mat();

    //            switch (TypeMode)
    //            {
    //                case Mode.Pattern:
    //                    if (matCrop.Type() == MatType.CV_8UC3)
    //                        Cv2.CvtColor(matCrop, matProcess, ColorConversionCodes.BGR2GRAY);
    //                    else
    //                        matProcess = matCrop;
    //                    break;
    //                case Mode.OutLine:
    //                    matProcess = Common.CannyWithMorph(matCrop);
    //                    break;
    //                case Mode.Edge:
    //                    using (Py.GIL())
    //                    {
    //                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
    //                        int height = matCrop.Rows;
    //                        int width = matCrop.Cols;
    //                        int channels = matCrop.Channels();
    //                        if (!matCrop.IsContinuous())
    //                        {
    //                            matCrop = matCrop.Clone();
    //                        }
    //                        int size = (int)(matCrop.Total() * matCrop.ElemSize());
    //                        byte[] buffer = new byte[size];
    //                        Marshal.Copy(matCrop.Data, buffer, 0, size);
    //                        // Tạo ndarray từ byte[]
    //                        var npImage = G.np.array(buffer).reshape(height, width, channels);
    //                        // Gọi hàm Python
    //                        dynamic result = G.Classic.EdgeDetection(npImage);
    //                        if (result == null)
    //                            return;

    //                        // Chuyển kết quả ngược về byte[] rồi sang Mat
    //                        byte[] edgeBytes = result.As<byte[]>();
    //                        matProcess = new Mat(height, width, MatType.CV_8UC1, edgeBytes);
    //                    }

    //                    break;
    //            }

    //            // Cv2.ImWrite("Processing.png", matProcess);
    //            //   BeeCore.Native.SetImg(matProcess);
    //            if (!matProcess.IsContinuous())
    //            {
    //                matProcess = matProcess.Clone();
    //            }

				//if (matProcess.Empty()) return;
				//if (matProcess.Type() == MatType.CV_8UC3)
				//	Cv2.CvtColor(matProcess, matProcess, ColorConversionCodes.BGR2GRAY);

				//Pattern.SetRawNoCrop(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels());

				//if (matProcess.Empty()) return;

				

				//rectRotates = new List<RectRotate>();
				//listScore = new List<double>();
				//listP_Center = new List<System.Drawing.Point>();
				//var ListRS = Pattern.Match(IsHighSpeed, 0, AngleLower, AngleUper, Common.PropetyTools[IndexThread][Index].Score / 100.0, ckSIMD, ckBitwiseNot, ckSubPixel, NumObject, OverLap, false, -1);
    //            float scoreRs = 0;
    //            foreach (Rotaterectangle rot in ListRS)
				//{
				//	PointF pCenter = new PointF((float)rot.Cx, (float)rot.Cy);
				//	float angle = (float)rot.AngleDeg;
				//	float width = (float)rot.Width;
				//	float height = (float)rot.Height;
				//	float Score = (float)rot.Score;
    //                scoreRs += Score;

    //                rectRotates.Add(new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None));
				//	listScore.Add(Math.Round(Score, 1));
				//	listP_Center.Add(new System.Drawing.Point((int)rectRotate._PosCenter.X - (int)rectRotate._rect.Width / 2 + (int)pCenter.X, (int)rectRotate._PosCenter.Y - (int)rectRotate._rect.Height / 2 + (int)pCenter.Y));


				//}
    //            if (scoreRs != 0)
    //                Common.PropetyTools[Global.IndexChoose][Index].ScoreResult =(int)Math.Round( scoreRs / rectRotates.Count(),1);


    //            matProcess.Dispose();
    //            matCrop.Dispose();

    //        }

    //    }

    }
}
