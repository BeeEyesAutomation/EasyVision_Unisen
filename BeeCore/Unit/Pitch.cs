using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;
using static BeeCore.Algorithm.FilletCornerMeasure;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class Pitch
    {
        public int MaxThread = 0;
        [NonSerialized]
        public Line2D Line;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        [NonSerialized]
        public bool IsNew = false;
        public bool IsIni = false;
        public int Index = -1;
        [NonSerialized]
        public BeeCppCli.PitchCliResult PitchResult;
        [NonSerialized]
        public BeeCppCli.PinPitchCliResult PinPitchResult;
        [NonSerialized]
        public Mat matProcess = new Mat();
        public RectRotate rotArea, rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public TypeCrop TypeCrop;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<RectRotate> rectRotates = new List<RectRotate>();

        [NonSerialized]
       // public ParallelGapDetector ParallelGapDetector;
        public BeeCppCli.PitchCli PitchMeasure;
        [NonSerialized]
        public BeeCppCli.PinPitchCli PinPitchMeasure;
        public PitchMeasureMode MeasureMode = PitchMeasureMode.PeakRoot;
        public int ExpectedPinCount = 4;
        public PinPitchArrangeMode PinArrangeMode = PinPitchArrangeMode.X;
        public bool UsePinAutoThreshold = true;
        public bool UseProjectedPitch = true;
        public int PinManualThreshold = 180;
        public double PinMinAreaPx = 12.0;
        public double PinMaxAreaRatio = 0.10;
        public double PinMinAspect = 0.45;
        public double PinMaxAspect = 2.20;
        public double PinMinFillRatio = 0.20;
        public bool UsePinOutlineCenter = true;
        public int PinOutlineThresholdOffset = 14;
        public int PinOutlineClose = 7;
        public int PinOutlineDilate = 5;
        public int PinOutlinePadding = 8;
        public int PinMaxOutlineExpand = 90;
        // Bug B+C fix: top-hat tách pin khỏi background không thuần đen (halo / phản chiếu).
        // Default OFF — chỉ bật khi có background xám/halo thực sự, vì top-hat có thể
        // loại bỏ pin nếu kernel nhỏ hơn kích thước pin. Khi bật, để TopHatKernelPx=0
        // cho auto, hoặc set thủ công > kích thước pin lớn nhất.
        public bool UseTopHat = false;
        public int TopHatKernelPx = 0;       // 0 = auto = max(81, min(image dim) * 0.6)
        // Bug C+D fix: reject blob halo (solidity < threshold). Pin vuông thật ~ 1.0;
        // halo merge với pin -> contour lồi lõm -> solidity giảm.
        public double MinSolidity = 0.80;    // 0 = không filter
        // Bug B fix: kẹp dilate <= 3 khi outline mode để không phình halo.
        // Default OFF để giữ behavior cũ; bật khi thấy box detect lan ra ngoài pin.
        public bool ReduceDilateForOutline = false;
        // Bug 1+3+4 fix (2026-05-08 runtime): mask từ Canny edges thay threshold pixel sáng.
        // Threshold blob bias về vùng phản chiếu + bóng mờ; Canny chỉ phản hồi gradient sắc
        // -> theo biên thật của pin pad, bóng mờ tự bị loại (gradient yếu).
        public bool UseEdgeBoundary = true;
        public int EdgeCannyLow = 20;
        public int EdgeCannyHigh = 60;
        // Bug 1+2 fix: center từ midpoint của projection bounds trên 2 trục minAreaRect.
        // Robust với pin xéo nhiều hướng + bright spot không nằm ở giữa pin.
        public bool UseEdgeGeometryCenter = true;
        // Runtime trial 2 fix: pin pad contrast yếu -> mask global chỉ bắt bright core,
        // box quá nhỏ. Refinement quanh seed bằng CLAHE+Sobel để tìm full pad boundary.
        public bool UseGradientRefinement = true;
        public int GradientPatchMargin = 60;
        public int GradientThreshold = 25;
        public double ClaheClipLimit = 3.0;
        public int ClaheTileSize = 8;
        public float NominalPitchMm = 0;
        public float PitchToleranceMm = 0.05f;   // legacy mm-based tolerance (giữ để backward-compat, không còn dùng cho score)
        public bool ShowPinDebugOverlay = true;
        // ===== New options =====
        // Vẽ khoảng cách Pin-Pin: Relative (chiếu lên trục hàng pin) | Absolute (Euclidean)
        public PinDistanceMode PinDistanceMode = PinDistanceMode.Relative;
        // Dùng Global.Config.Scale (chung) thay vì Pitch.Scale (per-tool, cũ)
        public bool UseSharedScale = false;
        public bool IsEnCrestPitch = true;
        public bool IsEnRootPitch = true;
        public bool IsEnCrestHeight = true;
        public bool IsEnRootHeight = true;
        public bool IsEnCrestCounter = true;
        public bool IsEnRootCounter = true;
        public Values Values = Values.Mean;
        public int IndexCCD = 0;
        public LineOrientation LineOrientation = LineOrientation.Vertical;
        public float ValueGau=3;
        public int NumCrestCouter;
        public int NumRootCouter;
        public int Magin = 0;
        public Compares Compare = Compares.Equal;
        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public int ThresholdBinary;
   
        public bool IsClose = false;
        public bool IsOpen = false;
        public bool IsClearNoiseBig = false;
        public bool IsClearNoiseSmall = false;
        public int SizeClearsmall = 1;
        public int SizeClearBig = 1;
        public int SizeClose = 1;
        public int SizeOpen = 1;

        public float TempPitchCrest = 0;
        public float TempPitchRoot = 0;
        public float TempHeightCrest = 0;
        public float TempHeightRoot = 0;
        public float TempCountCrest = 0;
        public float TempCountRoot = 0;
        public float Pitch12Mm = 0;
        public float Pitch23Mm = 0;
        public float Pitch34Mm = 0;
        public float SpanP1P4Mm = 0;
        public void Default()
        {
            MeasureMode = PitchMeasureMode.PeakRoot;
            ExpectedPinCount = 4;
            PinArrangeMode = PinPitchArrangeMode.X;
            UsePinAutoThreshold = true;
            UseProjectedPitch = true;
            PinManualThreshold = 180;
            PitchToleranceMm = 0.05f;
            UsePinOutlineCenter = true;
            PinOutlineThresholdOffset = 14;
            PinOutlineClose = 7;
            PinOutlineDilate = 5;
            PinOutlinePadding = 8;
            PinMaxOutlineExpand = 90;
            UseEdgeBoundary = true;
            EdgeCannyLow = 20;
            EdgeCannyHigh = 60;
            UseEdgeGeometryCenter = true;
            UseGradientRefinement = true;
            GradientPatchMargin = 60;
            GradientThreshold = 25;
            ClaheClipLimit = 3.0;
            ClaheTileSize = 8;
            Magin = 0;
            ValueGau = 3;
            Compare = Compares.Equal;
            Values = Values.Mean;
            LineOrientation = LineOrientation.Vertical;
            IsEnCrestPitch = true;
            IsEnCrestHeight= false;
            IsEnRootPitch = false;
            IsEnRootHeight= false;
            ThresholdBinary = 150;
            MethordEdge = MethordEdge.StrongEdges;
            SizeClearsmall = 100;
            SizeClearBig = 1000;
            SizeClose = 3;
            SizeOpen = 3;
         

            IsClearNoiseBig = false;
            IsClearNoiseSmall = false;
            IsClose = false;
            IsOpen = false;
        }
        public Pitch()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }

        float CurPitchCrest;
        float CurPitchRoot;
        float CurHeightCrest;
        float CurHeightRoot;

        public bool IsCalib = false;

       public bool IsNGCrestPitch, IsNGCrestHeight, IsNGRootPitch, IsNGRootHeight,IsNGCountCrest,IsNGCountRoot;
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            try
            {

                using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
                {
                    if (raw.Empty()) return;

                    Mat matCrop = Cropper.CropRotatedRect(raw, rotArea, rotMask);
                    if (matProcess == null) matProcess = new Mat();
                    if (!matProcess.Empty()) matProcess.Dispose();
                    if (matCrop.Type() == MatType.CV_8UC3)
                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGR2GRAY);


                    if (MeasureMode == PitchMeasureMode.PinPitch)
                    {
                        RunPinPitch(matCrop, rotArea);
                        return;
                    }

                    matProcess = Filters.ApplyEdgeMethod(matCrop, MethordEdge, ThresholdBinary);
                    if (IsClearNoiseSmall)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearsmall);
                    if (IsClose)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Close, new Size(SizeClose, SizeClose));
                    if (IsOpen)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                    if (IsClearNoiseBig)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearBig);
                    
                    PitchMeasure.SetGaussianSigma(ValueGau, -1.0);
                    PitchMeasure.SetScaleMmPerPx(EffectiveScale);
                    if (LineOrientation==LineOrientation.Vertical)
                    {
                        PitchMeasure.SetMargins(0,Magin);
                        PitchMeasure.SetScanAxis(BeeCppCli.ScanAxisCli.Y);
                    }    
                       
                    else
                    {
                        PitchMeasure.SetMargins(Magin, 0);
                        PitchMeasure.SetScanAxis(BeeCppCli.ScanAxisCli.X);
                    }    
                      
                    PitchMeasure.SetRejectedPolicy(true, 100);
                    PitchMeasure.SetImage(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels());
                    PitchResult = PitchMeasure.Measure();
                }
            }

            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Width", ex.Message));

            }


        }
        private void RunPinPitch(Mat matCrop, RectRotate area)
        {
            Mat gray = matCrop;
            if (gray.Type() == MatType.CV_8UC3)
            {
                gray = new Mat();
                Cv2.CvtColor(matCrop, gray, ColorConversionCodes.BGR2GRAY);
            }

            matProcess = gray.Clone();
            if (PinPitchMeasure == null)
                PinPitchMeasure = new BeeCppCli.PinPitchCli();

            PinPitchMeasure.SetOptions(
                ExpectedPinCount,
                ToPinArrangeModeCli(PinArrangeMode),
                PinDistanceMode == PinDistanceMode.Relative,
                UsePinAutoThreshold,
                PinManualThreshold,
                IsClose ? SizeClose : 0,
                IsOpen ? SizeOpen : 0,
                PinMinAreaPx,
                PinMaxAreaRatio,
                PinMinAspect,
                PinMaxAspect,
                PinMinFillRatio,
                UsePinOutlineCenter,
                PinOutlineThresholdOffset,
                PinOutlineClose,
                PinOutlineDilate,
                PinOutlinePadding,
                PinMaxOutlineExpand,
                EffectiveScale,
                UseTopHat,
                TopHatKernelPx,
                MinSolidity,
                ReduceDilateForOutline,
                UseEdgeBoundary,
                EdgeCannyLow,
                EdgeCannyHigh,
                UseEdgeGeometryCenter,
                UseGradientRefinement,
                GradientPatchMargin,
                GradientThreshold,
                ClaheClipLimit,
                ClaheTileSize);

            PinPitchMeasure.SetImage(gray.Data, gray.Width, gray.Height, (int)gray.Step(), gray.Channels());
            PinPitchResult = PinPitchMeasure.Measure();
            ApplyPinPitchResultToPoints(area);
        }

        private static BeeCppCli.PinArrangeModeCli ToPinArrangeModeCli(PinPitchArrangeMode mode)
        {
            switch (mode)
            {
                case PinPitchArrangeMode.Y:
                    return BeeCppCli.PinArrangeModeCli.Y;
                case PinPitchArrangeMode.RowProjection:
                    return BeeCppCli.PinArrangeModeCli.RowProjection;
                default:
                    return BeeCppCli.PinArrangeModeCli.X;
            }
        }

        private void ApplyPinPitchResultToPoints(RectRotate area)
        {
            listP_Center = new List<System.Drawing.Point>();
            rectRotates = new List<RectRotate>();
            Pitch12Mm = Pitch23Mm = Pitch34Mm = SpanP1P4Mm = 0;

            if (PinPitchResult == null)
                return;

            if (PinPitchResult.AdjacentPitchMm != null)
            {
                if (PinPitchResult.AdjacentPitchMm.Length > 0) Pitch12Mm = (float)PinPitchResult.AdjacentPitchMm[0];
                if (PinPitchResult.AdjacentPitchMm.Length > 1) Pitch23Mm = (float)PinPitchResult.AdjacentPitchMm[1];
                if (PinPitchResult.AdjacentPitchMm.Length > 2) Pitch34Mm = (float)PinPitchResult.AdjacentPitchMm[2];
            }
            SpanP1P4Mm = (float)PinPitchResult.SpanP1P4Mm;

            if (PinPitchResult.Pins == null)
                return;

            foreach (var pin in PinPitchResult.Pins)
            {
                var p = TransformLocalToWorld(area, new PointF((float)pin.X, (float)pin.Y));
                listP_Center.Add(new System.Drawing.Point((int)Math.Round(p.X), (int)Math.Round(p.Y)));
                // Bug A fix: dùng kích thước & góc xoay thật của pin (per-pin minAreaRect).
                // Trước đây hardcode 6x6 marker + rotation=0 -> consumers (Width PointToLine, ...)
                // không lock được pin xéo theo nhiều hướng khác nhau.
                float wPx = (float)pin.WidthPx;
                float hPx = (float)pin.HeightPx;
                if (wPx < 1f) wPx = 6f;
                if (hPx < 1f) hPx = 6f;
                // RectRotate convention: _rect = (-w/2, -h/2, w, h) ở local frame, _rectRotation theo độ.
                // pin.AngleDeg là góc trong crop-local; cộng area._rectRotation để ra world frame.
                float rotDeg = (float)pin.AngleDeg + area._rectRotation;
                rectRotates.Add(new RectRotate(
                    new RectangleF(-wPx * 0.5f, -hPx * 0.5f, wPx, hPx),
                    p,
                    rotDeg,
                    AnchorPoint.None));
            }
        }

        private static PointF TransformLocalToWorld(RectRotate area, PointF p)
        {
            using (var matLocal = new Matrix())
            {
                matLocal.Translate(area._PosCenter.X, area._PosCenter.Y);
                matLocal.Rotate(area._rectRotation);
                matLocal.Translate(area._rect.X, area._rect.Y);
                var pts = new[] { p };
                matLocal.TransformPoints(pts);
                return pts[0];
            }
        }

        public void Complete()
        {
            if (MeasureMode == PitchMeasureMode.PinPitch)
            {
                CompletePinPitch();
                return;
            }

           if(!Global.IsRun&& IsCalib)
            {
                TempPitchCrest =(float)( Values == Values.Mean ? PitchResult.PitchMeanMM : (Values == Values.Median ? PitchResult.PitchMedianMM : (Values == Values.Min ? PitchResult.PitchMinMM : PitchResult.PitchMaxMM)));
                TempPitchRoot = (float)(Values == Values.Mean ? PitchResult.PitchRootMeanMM : (Values == Values.Median ? PitchResult.PitchRootMedianMM : (Values == Values.Min ? PitchResult.PitchRootMinMM : PitchResult.PitchRootMaxMM)));
                TempHeightCrest = (float)(Values == Values.Mean ? PitchResult.CrestHMeanMM : (Values == Values.Median ? PitchResult.CrestHMedianMM : (Values == Values.Min ? PitchResult.CrestHMinMM : PitchResult.CrestHMaxMM)));
                TempHeightRoot = (float)(Values == Values.Mean ? PitchResult.RootHMeanMM : (Values == Values.Median ? PitchResult.RootHMedianMM : (Values == Values.Min ? PitchResult.RootHMinMM : PitchResult.RootHMaxMM)));
                TempCountCrest = PitchResult.Crests.Length;
                TempCountRoot = PitchResult.Roots.Length;
            }
            Common.TryGetTool(IndexThread, Index).Results = Results.OK;
            IsNGCrestPitch = false; IsNGCrestHeight = false; IsNGRootPitch = false; IsNGRootHeight = false; IsNGCountCrest = false; IsNGCountRoot = false; 
            if (IsEnCrestCounter)
            {
                switch (Compare)
                {
                    case Compares.Equal:
                        if(PitchResult.Crests.Length!=NumCrestCouter)
                        {
                            IsNGCountCrest = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                        }
                           
                        break;
                    case Compares.Less:
                       
                        if (PitchResult.Crests.Length >= NumCrestCouter)
                        {
                            IsNGCountCrest = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                        }
                      
                        break;
                    case Compares.More:
                       
                        if (PitchResult.Crests.Length <= NumCrestCouter)
                        {
                            IsNGCountCrest = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                        }
                       
                        break;
                }
            }
            if (IsEnRootCounter)
            {
                switch (Compare)
                {
                    case Compares.Equal:
                        if (PitchResult.Roots.Length != NumRootCouter)
                        {
                            IsNGCountRoot = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                        }
                           
                        break;
                    case Compares.Less:
                        if (PitchResult.Roots.Length >= NumRootCouter)
                        {
                            IsNGCountRoot = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                        }
                           
                        break;
                    case Compares.More:
                        if (PitchResult.Roots.Length <= NumRootCouter)
                        {
                            IsNGCountRoot = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                        }
                       
                        break;
                }
            }
            CurPitchCrest = (float)(Values == Values.Mean ? PitchResult.PitchMeanMM : (Values == Values.Median ? PitchResult.PitchMedianMM : (Values == Values.Min ? PitchResult.PitchMinMM : PitchResult.PitchMaxMM)));
             CurPitchRoot = (float)(Values == Values.Mean ? PitchResult.PitchRootMeanMM : (Values == Values.Median ? PitchResult.PitchRootMedianMM : (Values == Values.Min ? PitchResult.PitchRootMinMM : PitchResult.PitchRootMaxMM)));
             CurHeightCrest = (float)(Values == Values.Mean ? PitchResult.CrestHMeanMM : (Values == Values.Median ? PitchResult.CrestHMedianMM : (Values == Values.Min ? PitchResult.CrestHMinMM : PitchResult.CrestHMaxMM)));
             CurHeightRoot = (float)(Values == Values.Mean ? PitchResult.RootHMeanMM : (Values == Values.Median ? PitchResult.RootHMedianMM : (Values == Values.Min ? PitchResult.RootHMinMM : PitchResult.RootHMaxMM)));
            Common.TryGetTool(IndexThread, Index).ScoreResult = 0;
            float value=0;
            if (IsEnCrestPitch)
            {
                value = (Math.Abs(TempPitchCrest - CurPitchCrest) / TempPitchCrest) * 100;
                if (value > Common.TryGetTool(IndexThread, Index).ScoreResult) Common.TryGetTool(IndexThread, Index).ScoreResult = value;
                if (Common.TryGetTool(IndexThread, Index).ScoreResult > Common.TryGetTool(IndexThread, Index).Score)
                {
                    IsNGCrestPitch = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                }
              
            }
            if (IsEnRootPitch)
            {
                value = (Math.Abs(TempPitchRoot - CurPitchRoot) / TempPitchRoot) * 100;
                if (value > Common.TryGetTool(IndexThread, Index).ScoreResult) Common.TryGetTool(IndexThread, Index).ScoreResult = value;

                if (Common.TryGetTool(IndexThread, Index).ScoreResult > Common.TryGetTool(IndexThread, Index).Score) 
                {
                    IsNGRootPitch = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                }
                
            }
          
            if (IsEnRootHeight)
            {
               value= (Math.Abs(TempHeightRoot - CurHeightRoot) / TempHeightRoot) * 100;
                if (value > Common.TryGetTool(IndexThread, Index).ScoreResult) Common.TryGetTool(IndexThread, Index).ScoreResult = value;

                if (Common.TryGetTool(IndexThread, Index).ScoreResult >Common.TryGetTool(IndexThread, Index).Score) 
                {
                    IsNGRootHeight = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                }
               
            }

            if (IsEnCrestHeight)
            {
                value = (Math.Abs(TempHeightCrest - CurHeightCrest) / TempHeightCrest) * 100;
                if (value > Common.TryGetTool(IndexThread, Index).ScoreResult)
                    Common.TryGetTool(IndexThread, Index).ScoreResult = value;

                if (Common.TryGetTool(IndexThread, Index).ScoreResult > Common.TryGetTool(IndexThread, Index).Score)
                {
                    IsNGCrestHeight = true; Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                }
               
            }
         

        }
        private void CompletePinPitch()
        {
            PropetyTool owner = Common.TryGetTool(IndexThread, Index);
            if (owner == null)
                return;

            owner.Results = Results.OK;
            if (PinPitchResult == null || !PinPitchResult.Found || PinPitchResult.Pins == null || PinPitchResult.Pins.Length < ExpectedPinCount)
            {
                owner.ScoreResult = 999;
                owner.Results = Results.NG;
                return;
            }

            // Calib: lưu nominal = trung bình các pitch hiện tại (giống cách PeakRoot lưu Temp*)
            if (!Global.IsRun && IsCalib)
            {
                float sum = 0;
                int count = 0;
                if (PinPitchResult.AdjacentPitchMm != null)
                {
                    foreach (double pitch in PinPitchResult.AdjacentPitchMm)
                    {
                        sum += (float)pitch;
                        count++;
                    }
                }
                if (count > 0)
                    NominalPitchMm = sum / count;
            }

            // Chung Score với PeakRoot: tính worst % deviation rồi so sánh với owner.Score
            owner.ScoreResult = 0;
            if (NominalPitchMm > 0 && PinPitchResult.AdjacentPitchMm != null)
            {
                int numPin = 0;
                float Delta = 0;
                foreach (double pitch in PinPitchResult.AdjacentPitchMm)
                {
                    numPin++; Delta +=(float) Math.Abs(pitch - NominalPitchMm);
                   
                }
                owner.ScoreResult= (float)Math.Round((Delta / numPin), 2); ;
                if (owner.ScoreResult > owner.Score)
                    owner.Results = Results.NG;
            }
        }

        public async Task SendResult()
        {
            if (Common.TryGetTool(IndexThread, Index).IsSendResult)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                   // await Global.Comunication.Protocol.WriteResultFloat(Common.TryGetTool(IndexThread, Index).AddPLC, WidthResult);
                }
            }
        }
        public Graphics DrawResult(Graphics gc)
        {

            if (rotAreaAdjustment == null && Global.IsRun) return gc;
          
            gc.ResetTransform();       
            RectRotate rotA =rotArea;
            if (Global.IsRun) rotA =rotAreaAdjustment;
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
            switch(Common.TryGetTool(Global.IndexProgChoose, Index).Results)
            {
                case Results.OK:
                    cl =  Global.ParaShow.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.ParaShow.ColorNG;
                    break;
            }
            Pen pen = new Pen(Color.Blue, 2);
            String nameTool = (int)(Index + 1) + "." + Common.TryGetTool(Global.IndexProgChoose, Index).Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (MeasureMode == PitchMeasureMode.PinPitch)
                return DrawPinPitchResult(gc, rotA, nameTool, font, brushText, cl);

            if (Global.ParaShow.IsShowBox)
            {
                String crest = "", root = "";
                try { crest = $"P={CurPitchCrest:0.###} mm"; } catch { }
                try { root = $"P={CurPitchRoot:0.###} mm"; } catch { }

                int nC = PitchResult.Crests?.Length ?? 0;
                int nR = PitchResult.Roots?.Length ?? 0;

                String Content = $"Crest N={nC}  {crest} | Root N={nR}  {root} ";
                Draws.Box2Label(gc, rotA, nameTool, Content, font, cl, brushText,16, Global.ParaShow.ThicknessLine);

            }

            if (!Global.IsRun || Global.ParaShow.IsShowDetail)
            {
                if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rotA, Global.ScaleZoom * 100, Global.pScroll, cl, Global.ParaShow.Opacity / 100.0f);
            }
         
            gc.ResetTransform();
           
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
            if (PitchResult != null)
            {
                var opt = new BeeCore.PainterOptions
                {   CrestRadius=10,
                    RootRadius=10,
                    ShowCenterline = true,
                    ShowCrestPoints = true,
                    ShowRootPoints = true,
                    ShowHeightsCrest = true,
                    ShowHeightsRoot = true,
                    ShowRejected = true,
                    ShowTitle = false,
                    ShowPitchLabels = true,
                    ShowHeightLabels = true,
                    PitchTextColor = Color.DarkOrange,
                    HeightTextColor = Color.Blue,
                    CenterlineColor = Color.Magenta,
                    CrestPointColor = Color.Red,
                    RootPointColor = Color.Gold
                };

                BeeCore.PitchGdiPainter.Draw(gc, PitchResult, new Font("Segoe UI", 28f, FontStyle.Regular, GraphicsUnit.Pixel), opt);
            }    
         
           // if (!Global.IsRun)
           //     foreach (var l in GapResult.line2Ds)
           //         Draws.DrawInfiniteLine(gc, l, new Pen(Color.Gray, 2));
           // Draws.DrawInfiniteLine(gc,GapResult.LineA, new Pen(cl, 2));
           // Draws.DrawInfiniteLine(gc,GapResult.LineB, new Pen(cl, 2));
           // PointF p1 = new PointF(GapResult.lineMid[0].X,GapResult.lineMid[0].Y);
           // PointF p2 = new PointF(GapResult.lineMid[1].X,GapResult.lineMid[1].Y);
           // Draws.DrawTicks(gc, p1,LineOrientation, pen);
           // Draws.DrawTicks(gc, p2,LineOrientation, pen);
           // gc.DrawLine(new Pen(Color.Blue, 4), p1, p2);          
           //gc.DrawString($"{WidthResult:F2}mm", new Font("Arial", 16), Brushes.Blue, p1.X + 5, (p1.Y + p2.Y) / 2 + 10);
           // gc.ResetTransform();
           // //mat = new Matrix();
           //if (!Global.IsRun)
           //{
           //    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
           //    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
           //    gc.Transform = mat;
           //}
           //gc.DrawString("X:" + listP_Center[0].X + ":" + listP_Center[0].X, new Font("Arial", 24, FontStyle.Bold), new SolidBrush(cl), new PointF(0, 0));

            //gc.DrawEllipse(new Pen(Brushes.Red, 4), new Rectangle(listP_Center[0].X, listP_Center[0].Y, 10, 10));

            return gc;
        }

        private Graphics DrawPinPitchResult(Graphics gc, RectRotate rotA, string nameTool, Font font, Brush brushText, Color cl)
        {
            string content = PinPitchResult != null
                ? $"Pins={PinPitchResult.Pins?.Length ?? 0} P12={Pitch12Mm:0.###} P23={Pitch23Mm:0.###} P34={Pitch34Mm:0.###} Span={SpanP1P4Mm:0.###}mm"
                : "Pins=0";

            if (Global.ParaShow.IsShowBox)
                Draws.Box2Label(gc, rotA, nameTool, content, font, cl, brushText, 16, Global.ParaShow.ThicknessLine);

            gc.ResetTransform();
            using (var mat = new Matrix())
            {
                if (!Global.IsRun)
                {
                    mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                    mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                }
                mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                mat.Rotate(rotA._rectRotation);
                mat.Translate(rotA._rect.X, rotA._rect.Y);
                gc.Transform = mat;
            }

            // Gôm lại: 1 vòng lặp vẽ Pin + biên dạng detect + đường đo + label khoảng cách.
            if (PinPitchResult?.Pins != null && PinPitchResult.Pins.Length > 0)
            {
                var pins = PinPitchResult.Pins;
                int n = pins.Length;

                // Trục hàng pin. Relative đo giữa các hình chiếu vuông góc của tâm pin lên trục này.
                double rvx = PinPitchResult.RowVx;
                double rvy = PinPitchResult.RowVy;
                double rlen = Math.Sqrt(rvx * rvx + rvy * rvy);
                bool rowValid = rlen > 1e-6;
                if (rowValid) { rvx /= rlen; rvy /= rlen; }
                double rnx = -rvy;
                double rny = rvx;
                var rowOrigin = new PointF((float)PinPitchResult.RowX0, (float)PinPitchResult.RowY0);

                double mmPerPx = (PinPitchResult.ScaleMmPerPx > 1e-9) ? PinPitchResult.ScaleMmPerPx : EffectiveScale;
                var labelFont = new Font("Segoe UI", Global.ParaShow.FontSize, FontStyle.Bold, GraphicsUnit.Pixel);

                using (var penLine = new Pen(Color.DeepPink, Math.Max(1, Global.ParaShow.ThicknessLine)))
                using (var penBox = new Pen(Color.Lime, Math.Max(1, Global.ParaShow.ThicknessLine)))
                using (var penTick = new Pen(Color.Red, Math.Max(2, Global.ParaShow.ThicknessLine + 1)))
                using (var brushPin = new SolidBrush(Color.Yellow))
                using (var brushLabel = new SolidBrush(Color.DarkOrange))
                using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    for (int i = 0; i < n; i++)
                    {
                        var pin = pins[i];
                        var p = new PointF((float)pin.X, (float)pin.Y);

                        // (1) Vẽ Pin: biên dạng detect + dấu cộng + ID
                        DrawPinDetectedBox(gc, pin, penBox);
                        Draws.Plus(gc, (int)Math.Round(p.X), (int)Math.Round(p.Y), 16, Color.Yellow, 2);
                        gc.DrawString($"P{pin.Id}", font, brushPin, p.X + 5, p.Y - 18);

                        // (2) Vẽ đường đo tới pin kế + label khoảng cách trên đường đo
                        if (i >= n - 1) continue;
                        var b = pins[i + 1];
                        var pb = new PointF((float)b.X, (float)b.Y);

                        // Tính khoảng cách (mm) theo mode
                        double dx = pb.X - p.X;
                        double dy = pb.Y - p.Y;
                        double distMm;
                        PointF measureA = p;
                        PointF measureB = pb;
                        double angleDx = dx;
                        double angleDy = dy;
                        if (PinDistanceMode == PinDistanceMode.Absolute || !rowValid)
                        {
                            distMm = Math.Sqrt(dx * dx + dy * dy) * mmPerPx;
                            gc.DrawLine(penLine, measureA, measureB);
                        }
                        else
                        {
                            measureA = ProjectToRowAxis(p, rowOrigin, rvx, rvy);
                            measureB = ProjectToRowAxis(pb, rowOrigin, rvx, rvy);
                            double mdx = measureB.X - measureA.X;
                            double mdy = measureB.Y - measureA.Y;
                            double proj = Math.Sqrt(mdx * mdx + mdy * mdy);
                            distMm = proj * mmPerPx;
                            angleDx = rvx;
                            angleDy = rvy;

                            gc.DrawLine(penLine, measureA, measureB);
                            DrawRelativePitchTick(gc, penTick, measureA, rnx, rny);
                            DrawRelativePitchTick(gc, penTick, measureB, rnx, rny);
                        }

                        // Vẽ label trên đoạn đo (xoay theo hướng đoạn, đặt ngay giữa)
                        double angRad = Math.Atan2(angleDy, angleDx);
                        float angDeg = (float)(angRad * 180.0 / Math.PI);
                        float mx = (measureA.X + measureB.X) * 0.5f;
                        float my = (measureA.Y + measureB.Y) * 0.5f;
                        string text = $"{distMm:0.###} mm";
                        var state = gc.Save();
                        gc.TranslateTransform(mx, my);
                        gc.RotateTransform(angDeg);
                        gc.DrawString(text, labelFont, brushLabel, 0f, -2f, sf);
                        gc.Restore(state);
                    }
                }
                labelFont.Dispose();
            }

            gc.ResetTransform();
            return gc;
        }

        private static PointF ProjectToRowAxis(PointF p, PointF origin, double vx, double vy)
        {
            double dx = p.X - origin.X;
            double dy = p.Y - origin.Y;
            double t = dx * vx + dy * vy;
            return new PointF((float)(origin.X + t * vx), (float)(origin.Y + t * vy));
        }

        private static void DrawRelativePitchTick(Graphics gc, Pen pen, PointF center, double nx, double ny)
        {
            const float halfLen = 28f;
            var a = new PointF((float)(center.X - nx * halfLen), (float)(center.Y - ny * halfLen));
            var b = new PointF((float)(center.X + nx * halfLen), (float)(center.Y + ny * halfLen));
            gc.DrawLine(pen, a, b);
        }

        private static void DrawPinDetectedBox(Graphics gc, BeeCppCli.PinCenterCli pin, Pen pen)
        {
            float w = (float)Math.Max(1.0, pin.WidthPx);
            float h = (float)Math.Max(1.0, pin.HeightPx);
            float cx = (float)pin.X;
            float cy = (float)pin.Y;
            double angle = pin.AngleDeg * Math.PI / 180.0;
            double ca = Math.Cos(angle);
            double sa = Math.Sin(angle);
            float hx = w * 0.5f;
            float hy = h * 0.5f;

            var pts = new[]
            {
                RotateLocal(cx, cy, -hx, -hy, ca, sa),
                RotateLocal(cx, cy,  hx, -hy, ca, sa),
                RotateLocal(cx, cy,  hx,  hy, ca, sa),
                RotateLocal(cx, cy, -hx,  hy, ca, sa),
            };
            gc.DrawPolygon(pen, pts);
        }

        private static PointF RotateLocal(float cx, float cy, float x, float y, double ca, double sa)
        {
            return new PointF((float)(cx + x * ca - y * sa), (float)(cy + x * sa + y * ca));
        }


        public void SetModel(bool IsCopy=false)
        {
            if (rotArea == null) rotArea = new RectRotate();
            if (rotCrop == null) rotCrop = new RectRotate();
            if (rotMask == null) rotMask = new RectRotate();

            rotCrop.Name = "Area Temp";
            rotCrop.TypeCrop = TypeCrop.Crop;


            rotMask.Name = "Area Mask";
            rotMask.TypeCrop = TypeCrop.Mask;

            rotArea.Name = "Area Check";
            rotArea.TypeCrop = TypeCrop.Area;
         
            PitchMeasure = new BeeCppCli.PitchCli();
            PinPitchMeasure = new BeeCppCli.PinPitchCli();
            Common.TryGetTool(IndexThread, Index).StepValue = 0.01f;
    
            Common.TryGetTool(IndexThread, Index).MinValue = 0;
          
            Common.TryGetTool(IndexThread, Index).MaxValue = 20;
            Common.TryGetTool(IndexThread, Index). StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
        // Scale hiệu lực: nếu UseSharedScale → lấy Global.Config.Scale; ngược lại → Scale per-tool
        public float EffectiveScale => UseSharedScale ? Global.Config.Scale : Scale;
  
    }
}
