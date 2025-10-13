using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class Intersect
    {
       
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsIni = false;
        public int Index = -1;
        [NonSerialized]
        DetectIntersect DetectIntersect = new DetectIntersect();
        [NonSerialized]
        CornerResult Result = new CornerResult();
        
        [NonSerialized]
        public Mat matProcess = new Mat();
        public RectRotate rotArea, rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public TypeCrop TypeCrop;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<RectRotate> rectRotates = new List<RectRotate>();
      
        public int MaximumLine = 20;
   
        public int MinInliers = 5;
        public float WidthResult = 0;
        public float WidthTemp = 0;
 
        public bool IsCalibs = false;
        public MethordEdge MethordEdge = MethordEdge.StrongEdges;
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
        public void Default()
        {
           
         
            WidthTemp = 0;
            RansacIterations = 200;
            RansacThreshold = 2;
            ThresholdBinary = 150;
            MethordEdge = MethordEdge.StrongEdges;
            SizeClearsmall = 100;
            SizeClearBig = 1000;
            SizeClose = 3;
            SizeOpen = 3;
            MaximumLine = 10;

            IsClearNoiseBig = false;
            IsClearNoiseSmall = false;
            IsClose = false;
            IsOpen = false;
        }
        public Intersect()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
        public bool AutoMean = true;
        public int FixMean = 3;
        public float ContinuityGapFactor = 1.2f;
        public int AngleTargetDeg = 90;
        public int AngleToleranceDeg = 10;
        public void DoWork(RectRotate rectRotate)
        {
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (raw.Empty())
                    return;

                Mat gray = null;
                try
                {
                    if (raw.Type() == MatType.CV_8UC3)
                    {
                        gray = new Mat();
                        Cv2.CvtColor(raw, gray, ColorConversionCodes.BGR2GRAY);
                    }
                    else
                    {
                        gray = raw; // reuse backing store (ref-counted)
                    }

                    Mat matCrop = Cropper.CropRotatedRect(gray, rotArea, rotMask);
                    if (matProcess == null) matProcess = new Mat();
                    if (!matProcess.IsDisposed)
                        if (!matProcess.Empty()) matProcess.Dispose();

                    try
                    {


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
                            default:
                                matProcess = matCrop; // fallback
                                break;
                        }
                        // Hậu xử lý hình thái học / khử nhiễu (mỗi hàm có thể trả Mat mới → nhớ dispose cái cũ)
                        if (IsClearNoiseSmall)
                        {
                            Mat t = Filters.ClearNoise(matProcess, SizeClearsmall);
                            if (!object.ReferenceEquals(t, matProcess)) matProcess.Dispose();
                            matProcess = t;
                        }
                        if (IsClose)
                        {
                            Mat t = Filters.Morphology(matProcess, MorphTypes.Close, new OpenCvSharp.Size(SizeClose, SizeClose));
                            if (!object.ReferenceEquals(t, matProcess)) matProcess.Dispose();
                            matProcess = t;
                        }
                        if (IsOpen)
                        {
                            Mat t = Filters.Morphology(matProcess, MorphTypes.Open, new OpenCvSharp.Size(SizeOpen, SizeOpen));
                            if (!object.ReferenceEquals(t, matProcess)) matProcess.Dispose();
                            matProcess = t;
                        }
                        if (IsClearNoiseBig)
                        {
                            Mat t = Filters.ClearNoise(matProcess, SizeClearBig);
                            if (!object.ReferenceEquals(t, matProcess)) matProcess.Dispose();
                            matProcess = t;
                        }
                        OrthCornerOptions orthCornerOptions = new OrthCornerOptions { 
                        MaxCandidateLines=MaximumLine,
                        RansacIterations=RansacIterations,
                        RansacThreshold=RansacThreshold,
                        MinInliersPerLine=MinInliers,
                        AutoMean=AutoMean,
                        FixMean=FixMean,
                        AngleTargetDeg= AngleTargetDeg,
                        AngleToleranceDeg= AngleToleranceDeg,
                        ContinuityGapFactor=ContinuityGapFactor,
                        };

                        Result = DetectIntersect.FindBestCorner_RansacRuns(matCrop, matProcess, orthCornerOptions);


                       

                        PointF pCenter = new System.Drawing.PointF(Result.Corner.X, Result.Corner.Y);
                       
                        listP_Center.Add(new System.Drawing.Point(
                       (int)(rectRotate._PosCenter.X - rectRotate._rect.Width / 2f + pCenter.X),
                       (int)(rectRotate._PosCenter.Y - rectRotate._rect.Height / 2f + pCenter.Y)));

                    }
                    finally
                    {
                        // Giải phóng pipeline mat
                        // Chỉ dispose nếu nó khác với work
                        // (nếu Filters trả về cùng instance thì ReferenceEquals sẽ true)
                        // Ở đây đã dispose từng bước khi thay thế, nên chỉ cần:
                        // nothing else to do
                    }


                }

                catch (Exception ex)
                {
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Width", ex.Message));

                }


            }
        }
        public void Complete()
        {
           
          
           
            Common.PropetyTools[IndexThread][Index].ScoreResult= (int)((Math.Abs(WidthResult - WidthTemp) / (WidthTemp * 1.0))*100);
         if(Result.Found)
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
         else
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            
        }
        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        public bool IsSendResult;
        public async Task SendResult()
        {
            if (IsSendResult)
            {
                if (Global.ParaCommon.Comunication.Protocol.IsConnected)
                {
                   
                    await Global.ParaCommon.Comunication.Protocol.WriteResultFloat(AddPLC, WidthResult);
                }
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
            Brush brushText = new SolidBrush(Global.Config.TextColor);
            Color cl = Color.LimeGreen;

            if (Common.PropetyTools[Global.IndexChoose][Index].Results == Results.NG)
                cl = Global.Config.ColorNG;
            else
                cl = Global.Config.ColorOK;
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl, Global.Config.ThicknessLine);
            if (!Global.IsRun && Global.Config.IsShowMatProcess || Global.IsRun && Global.Config.IsShowDetail)
            {
                if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rotA, Global.ScaleZoom * 100, Global.pScroll, cl, Global.Config.Opacity / 100.0f);
            }
            if ( Result.Found)
            {

                mat.Translate(rotA._rect.X, rotA._rect.Y);

                gc.Transform = mat;
                var flags = DrawFlags.None;
                if (Global.IsRun)
                    flags = DrawFlags.BestCorner | DrawFlags.BestLines;
                else
                {
                    if (Global.Config.IsShowDetail) flags = flags | DrawFlags.Inliers;                 
                    if (Global.Config.IsShowNotMatching) flags = flags | DrawFlags.RansacRejected | DrawFlags.Runs;
                    if (Global.Config.IsShowResult) flags = flags | DrawFlags.BestCorner | DrawFlags.BestLines;
                }

                DrawStyle drawStyle = new DrawStyle
                {
                    Inlier = Color.Red,
                    LineChoose = Global.Config.ColorChoose,
                    LineResult = Global.Config.ColorInfor,
                    LineNone = Global.Config.ColorNone,
                    LineDash = DashStyle.Solid,
                    InlierSize = Global.Config.ThicknessLine / 2,
                    Thickness = Global.Config.ThicknessLine,
                };
                DetectIntersect.RenderDebugToGraphics(gc, new RectangleF(0, 0, rotA._rect.Width, rotA._rect.Height), DetectIntersect.LineEdge, flags, drawStyle);
            }

            return gc;
        }


        public void SetModel()
        {
            
            if (rotArea == null) rotArea = new RectRotate();
            rotMask = null;
            DetectIntersect = new DetectIntersect();
            Common.PropetyTools[IndexThread][Index].StepValue = 0.1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
          
            Common.PropetyTools[IndexThread][Index].MaxValue = 20;
            Common.PropetyTools[IndexThread][Index]. StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
