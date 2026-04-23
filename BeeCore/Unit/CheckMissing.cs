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
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static BeeCore.Cropper;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class CheckMissing
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
        [NonSerialized]
        public int MaxThread = 1;
        public bool IsIni = false;
        public int Index = -1;
        public int ScorePattern = 0;
        public LineOrientation LineOrientation = LineOrientation.Horizontal;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
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
        public int IndexCCD = 0;
        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        [NonSerialized]
        public Mat matTemp;
        public List<Point> Postion=new List<Point>();
       private Mode _TypeMode=Mode.Pattern;
        public List<double> listScore = new List<double>();

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
                if (Global.Comunication.Protocol.IsConnected)
                {
                    int i = 0; 
                    int Add=(int)Converts.StringtoDouble(AddPLC);
                    String sAdd = Converts.BeforeFirstDigit(AddPLC);
                    foreach (System.Drawing.Point point in listP_Center)
                    {
                        String Address = sAdd + Add;
                        float[] floats = new float[4] { point.X, point.Y, list_AngleCenter[i],(float) listScore[i] };
                        await Global.Comunication.Protocol.WriteResultFloatArr(AddPLC, floats);
                        Add += 8;
                         i++;
                    }
                }
            }
        }
        public bool IsAreaWhite=false;
    
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

        public List< System.Drawing.Point > listP_Center=new List<System.Drawing.Point>();
        [NonSerialized]
        public BeeCpp.Pattern2 Pattern =new BeeCpp.Pattern2();
        public CheckMissing()
        {
           
        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
        public int MaxObject = 1;
        public float StepAngle = 0;
        [NonSerialized]
        Mat matProcess = new Mat();
        public Mat LearnPattern(Mat raw, bool IsNoCrop)
        {

            using (Mat img = raw.Clone())
            {
              
                if (img.Channels() == 3)
                    Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
                else if (img.Channels() == 4)
                    Cv2.CvtColor(img, img, ColorConversionCodes.BGRA2GRAY);

                int w = 0, h = 0, s = 0, c = 0;
                IntPtr intpr = IntPtr.Zero;
                Mat mat = new Mat();
                if(TypeMode==Mode.Edge)
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
                        Pattern.LearnPatternStable();
                    }
                    else
                    {


                        var rrCli = Converts.ToCli(rotCrop); // như ở reply trước
                        RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                        intpr = Pattern.SetImgeSample(img.Data, img.Width, img.Height, (int)img.Step(), img.Channels(), rrCli, rrMaskCli, IsNoCrop,
                                out w, out h, out s, out c);

                        if (intpr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                            return mat; // trả Mat rỗng
                        Pattern.LearnPatternStable();
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

       

        public void SetModel()
        {
			if (Pattern == null)
			{
				Pattern = new BeeCpp.Pattern2();

			}
            if (bmRaw != null)
            {
				matTemp = bmRaw.ToMat();
				LearnPattern(matTemp, true);
			}
            if (rotArea == null) rotArea = new RectRotate();
            if (rotCrop == null) rotCrop = new RectRotate();
            if (rotMask == null) rotMask = new RectRotate();

            rotCrop.Name = "Area Temp";
            rotCrop.TypeCrop = TypeCrop.Crop;
            rotMask.Name = "Area Mask";
            rotMask.TypeCrop = TypeCrop.Mask;
            rotArea.Name = "Area Check";
            rotArea.TypeCrop = TypeCrop.Area;
            if (Scale == 0) Scale = 1;
            if (rotCrop == null) rotCrop = new RectRotate();
            if(rotArea == null) rotArea = new RectRotate();
            if (ScorePattern == 0)
                ScorePattern = 80;
            Common.TryGetTool(IndexThread, Index).StepValue = 1;
			Common.TryGetTool(IndexThread, Index).MinValue = 0;

            Common.TryGetTool(IndexThread, Index).MaxValue = 100;
            if (Common.TryGetTool(IndexThread, Index).Score == 0)
                Common.TryGetTool(IndexThread, Index).Score = 80;
            Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;
        }

        public List<RectRotate> rectRotates = new List<RectRotate>();
         public  List<float>list_AngleCenter =new List<float>();
        public ZeroPos ZeroPos=ZeroPos.Zero;
        public float Scale = 1;
        public bool IsLimitCouter = true;
        public int DistanceMedium= 0;
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = 0;
            // 5) Gom kết quả
            rectRotates = new List<RectRotate>();
            listScore = new List<double>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                if (raw.Empty()) return;
                if (raw.Type() == MatType.CV_8UC3)
                {
                   
                    Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);
                }
               


                Mat matProcess = null;
                byte[] rentedBuffer = null;

                try
                {
                    switch (TypeMode)
                    {
                        case Mode.Pattern:
                           
                                matProcess = raw; // reuse backing store
                            
                            var rrCli = Converts.ToCli(rotArea); // như ở reply trước
                            RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                            Pattern.SetImgeRaw(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels(), rrCli, rrMaskCli);
                            break;
                        case Mode.Edge:
                            Mat matCrop = new Mat();                               
                                PatchCropContext ctx = new PatchCropContext();
                                matCrop = Cropper.CropOuterPatch(raw, rotArea, out ctx);
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

                                matProcess = ApplyShapeMaskAndCompose(matProcess, ctx, rotArea, rotMask, returnMaskOnly: false);
                            //Cv2.ImWrite("process.png", matProcess);
                                Pattern.SetRawNoCrop(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels());

                            break;
                    }

                    if (matProcess == null || matProcess.Empty()) return;
                    GC.KeepAlive(matProcess);
                    Pattern2StableConfig cfg = new Pattern2StableConfig(true);
                    cfg.AngleStartDeg = AngleLower;
                    cfg.AngleEndDeg = AngleUper;
                    cfg.AngleStepDeg = StepAngle;      // auto
                    cfg.MinAcceptScore = ScorePattern / 100.0;
                    cfg.MaxPos = MaxObject;
                    cfg.MaxOverlap = OverLap;
                    cfg.BitwiseNot = ckBitwiseNot;
                    cfg.SubPixel = ckSubPixel;
                    cfg.EnableValidator = true;
                  
                    cfg.EnableKeepFilter = true;
                    cfg.EnableNms = false;
                    cfg.Difficulty = Pattern2DifficultyLevel.Hard;
                    cfg.EnableAutoThreshold = true;

                    // scale mẫu
                    cfg.EnableScaleSearch = false;
                    cfg.DebugLog = false;
                    cfg.ScaleMin = 0.90;
                    cfg.ScaleMax = 1.10;
                    cfg.ScaleStep = 0.1;
                    cfg.DebugLogPath = "E:\\pattern2_debug.txt";
                    var listRS = Pattern.MatchStable(
                     cfg
                    );
                    float scoreSum = 0f;
                    if (listRS != null)
                    {
                        foreach (Rotaterectangle rot in listRS)
                        {
                            float w = (float)rot.Width;
                            float h = (float)rot.Height;
                            var pCenter = new System.Drawing.PointF((float)rot.Cx, (float)rot.Cy);
                            float angle = (float)rot.AngleDeg;
                            float score = (float)rot.Score;
                            scoreSum += score;
                            if (angle < AngleLower || angle > AngleUper)
                                continue;
                            if (rot.Score < ScorePattern )
                                continue;
                            rectRotates.Add(new RectRotate(
                                new System.Drawing.RectangleF(-w / 2f, -h / 2f, w, h),
                                pCenter, angle, AnchorPoint.None));

                            listScore.Add(Math.Round(score, 1));
                            int xCenter = (int)(rotArea._PosCenter.X - rotArea._rect.Width / 2f + pCenter.X);
                            int yCenter = (int)(rotArea._PosCenter.Y - rotArea._rect.Height / 2f + pCenter.Y);
                            float angleCenter = rotArea._rectRotation + angle;
                            
                            if (ZeroPos==ZeroPos.Zero)
                            {
                                list_AngleCenter.Add(angleCenter);
                                listP_Center.Add(new System.Drawing.Point(
                                 (int) (xCenter/Scale), (int)(yCenter/Scale)));
                            }
                            else
                            {
                                //angleCenter = angleCenter - Global.rotOriginAdj._rectRotation;
                                //xCenter = xCenter-(int) Global.rotOriginAdj._PosCenter.X;
                                //yCenter = yCenter - (int)Global.rotOriginAdj._PosCenter.Y;
                                //list_AngleCenter.Add(angleCenter);
                                //listP_Center.Add(new System.Drawing.Point(
                                // (int)(xCenter / Scale), (int)(yCenter / Scale)));
                            }    
                          
                        }
                    }

                    if (scoreSum != 0 && rectRotates.Count > 0)
                    {
                        Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult =
                            (int)Math.Round(scoreSum / rectRotates.Count, 1);
                    }
                }
                catch(Exception ex)
                {

                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Pattern", ex.ToString()));

                }
                finally
                {
                    if (rentedBuffer != null)
                        System.Buffers.ArrayPool<byte>.Shared.Return(rentedBuffer);

                    if (matProcess != null )
                        matProcess.Dispose();

                   
                }
            }
        }
        [NonSerialized]
        private AdjacentGapCheckResult BoxGapCheckResult;
        public float AverageGap = 0;
        public void Complete()
        {
            Common.TryGetTool(IndexThread, Index).Results = Results.OK;
            if (rectRotates.Count()< LimitCounter)
            {
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
            }
            else
            {
                
                BoxGapCheckResult = RectRotateGapChecker.CheckAdjacentCenterGap(rectRotates, LineOrientation,!Global.IsRun,AverageGap, Common.TryGetTool(IndexThread, Index).Score);
            if(!Global.IsRun)
                if(BoxGapCheckResult != null)
                {
                    AverageGap = BoxGapCheckResult.AverageDistance;
                } 
                    
                if(!BoxGapCheckResult.IsOK)
                    Common.TryGetTool(IndexThread, Index).Results = Results.NG;
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
            if (Common.TryGetTool(Global.IndexProgChoose, Index).Results == Results.NG)
			{
				cl = Global.ParaShow.ColorNG;
			}
			else
			{
				cl =  Global.ParaShow.ColorOK;
			}
			String nameTool = (int)(Index + 1) + "." + BeeCore.Common.TryGetTool(IndexThread, Index).Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            String Infor = "";
            if(BoxGapCheckResult!=null)
            if(BoxGapCheckResult.Gaps.Count>0)
            {
                if (BoxGapCheckResult.IsOK)
                    Infor += "Gap" + (int)BoxGapCheckResult.AverageDistance + "/"+ (int)AverageGap;
                else
                    Infor += "Gap" + (int)BoxGapCheckResult.AverageDistance + "-"+ "Fail: " + (int)BoxGapCheckResult.FirstFailed.ScorePercent+"%";

            }    
            if (Global.ParaShow.IsShowBox)
                Draws.Box2Label(gc, rotA._rect, nameTool, Infor, font, cl, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);

          //  Draws.Box1Label(gc,rotA, nameTool, font, brushText, cl, Global.ParaShow.ThicknessLine);
			gc.ResetTransform();
			if (listScore == null) return gc;
			if (rectRotates.Count > 0)
			{
                // === Tính offset (như cũ) ===
              
                int i = 1;
				foreach (RectRotate rot in rectRotates)
				{

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
                    //if (OffSetX < 0 || OffSetY < 0)
                    //{

                    //    mat.Translate(OffSetX, OffSetY);
                    //    gc.Transform = mat;
                    //}

                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    //  gc.Transform = mat;
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                     if(Global.ParaShow.IsShowPostion)
                    {
                        int min = (int)Math.Min(rot._rect.Width / 4, rot._rect.Height / 4);
                        Draws.Plus(gc, 0, 0, min, cl, Global.ParaShow.ThicknessLine);
                        String sPos = "X,Y,A _ " + listP_Center[i - 1].X + "," + listP_Center[i - 1].Y + "," + Math.Round(list_AngleCenter[i - 1], 1);
                        if (ZeroPos == ZeroPos.ZeroADJ)
                            sPos = "*X,Y,A _ " + listP_Center[i - 1].X + "," + listP_Center[i - 1].Y + "," + Math.Round(list_AngleCenter[i - 1], 1);

                        gc.DrawString(sPos, font, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 5));

                    }
                    //Math.Round(listScore[i - 1], 1) +"%"
                    Draws.Box2Label(gc, rot._rect, i + "", "", font, Global.ParaShow.ColorOK, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);


					gc.ResetTransform();
					i++;
				}
			}

            if (Global.ParaShow.IsShowBox &&
                BoxGapCheckResult != null &&
                !BoxGapCheckResult.IsOK &&
                BoxGapCheckResult.BoxNG != null)
            {
                RectRotate rotNG = BoxGapCheckResult.BoxNG;
                mat = new Matrix();
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                mat.Rotate(rotA._rectRotation);
                mat.Translate(rotA._rect.X, rotA._rect.Y);
                mat.Translate(rotNG._PosCenter.X, rotNG._PosCenter.Y);
                mat.Rotate(rotNG._rectRotation);
                gc.Transform = mat;

                Draws.Box2Label(gc, rotNG._rect, "NG", "", font, Global.ParaShow.ColorNG, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);
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
