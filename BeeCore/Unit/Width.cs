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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class Width
    {
        [NonSerialized]
        public Line2D Line;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsEnEqualizeHist = false;
        [NonSerialized]
        public bool IsNew = false;
        public int IndexCCD = 0;
        public bool IsIni = false;
        public int Index = -1;
        [NonSerialized]
        public GapResult GapResult = new GapResult();
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
        public ParallelGapDetector ParallelGapDetector;
        [NonSerialized]
        public RansacLine RansacLine;
        [NonSerialized]
        public Line2DCli ReferenceLineL = new Line2DCli();
        public WidthMeasureMode MeasureMode = WidthMeasureMode.ParallelLines;
        public string PointSourceToolName = "";
        public int PointSourceIndex = 0;
        public float PointToLineNominalMm = 0;
        public float PointToLineToleranceMm = 0.05f; // kept for backward-compat; logic now uses owner.Score
        public bool PointToLineCheckAll = false;
        [NonSerialized]
        public List<(PointF center, PointF foot, float dist)> AllPointResults;
        public LineOrientation ReferenceLineOrientation = LineOrientation.Horizontal;
        public int ReferenceLineAngleRange = 10;
        public LineDirScan ReferenceLineScan = LineDirScan.TopBot;
        public PointF PointToLineCenter = PointF.Empty;
        public PointF PointToLineFoot = PointF.Empty;
        public bool PointToLineFound = false;
        public int MaximumLine = 4;
        public GapExtremum GapExtremum = GapExtremum.Middle;
        public LineOrientation LineOrientation = LineOrientation.Vertical;
        public SegmentStatType SegmentStatType = SegmentStatType.Average;
        public int MinInliers = 2;
        public float WidthResult = 0;
        public float WidthTemp = 0;
        public int MinLen = 0, MaxLen = 10000;
        public bool IsCalibs = false;
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
        public void Default()
        {
            MeasureMode = WidthMeasureMode.ParallelLines;
            PointToLineToleranceMm = 0.05f;
            ReferenceLineOrientation = LineOrientation.Horizontal;
            ReferenceLineAngleRange = 10;
            ReferenceLineScan = LineDirScan.TopBot;
            GapExtremum = GapExtremum.Middle;
            LineOrientation = LineOrientation.Vertical;
            SegmentStatType = SegmentStatType.Average;
            MinLen = 0;
            MaxLen = 10000;
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
        public Width()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }





        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            try
            {
                if (ParallelGapDetector == null) ParallelGapDetector = new ParallelGapDetector();
                if (RansacLine == null) RansacLine = new RansacLine();
                WidthResult = 0;
                PointToLineFound = false;
                using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
                {
                    if (raw.Empty()) return;
                    if (IsCalibs | MaxLen == 0)
                    {
                        MinInliers = 2;

                    }
                    Mat matCrop = Cropper.CropRotatedRect(raw, rotArea, rotMask);
                    if (matProcess == null) matProcess = new Mat();
                    if (!matProcess.Empty()) matProcess.Dispose();
                    if (matCrop.Type() == MatType.CV_8UC3)
                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGR2GRAY);
                    Mat matEua = new Mat();
                    if (IsEnEqualizeHist)
                        Cv2.EqualizeHist(matCrop, matEua);
                    else
                        matEua = matCrop;
                        matProcess = Filters.ApplyEdgeMethod(matEua, MethordEdge, ThresholdBinary);
                    if (IsClearNoiseSmall)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearsmall);
                    if (IsClose)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Close, new Size(SizeClose, SizeClose));
                    if (IsOpen)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                    if (IsClearNoiseBig)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearBig);
                    if (MeasureMode == WidthMeasureMode.PointToLine)
                    {
                        RunPointToLine(rotArea);
                        return;
                    }
                    ParallelGapDetector.RansacThreshold = RansacThreshold;
                    ParallelGapDetector.RansacIterations= RansacIterations;
                    GapResult = new GapResult();
                    GapResult = ParallelGapDetector.MeasureParallelGap(matCrop, matProcess, MaximumLine, GapExtremum, LineOrientation, SegmentStatType, MinInliers);
                    if (GapResult.lineMid != null)
                        if (GapResult.lineMid.Count() > 1)
                        {
                            listP_Center = new List<System.Drawing.Point>();
                            rectRotates = new List<RectRotate>();
                            PointF p1 = new PointF(GapResult.lineMid[0].X, GapResult.lineMid[0].Y);
                            PointF p2 = new PointF(GapResult.lineMid[1].X, GapResult.lineMid[1].Y);
                            float Xmin = Math.Min(p1.X, p2.X); float Ymin = Math.Min(p1.Y, p2.Y);
                            PointF pCenter = new PointF(Xmin + Math.Abs(p1.X - p2.X) / 2, Ymin + Math.Abs(p1.Y - p2.Y) / 2);
                            listP_Center.Add(new System.Drawing.Point((int)rotArea._PosCenter.X - (int)rotArea._rect.Width / 2 + (int)pCenter.X, (int)rotArea._PosCenter.Y - (int)rotArea._rect.Height / 2 + (int)pCenter.Y));
                            rectRotates.Add(new RectRotate());
                        }
                }
            }

            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Width", ex.Message));

            }

        
        }
        private void RunPointToLine(RectRotate area)
        {
            if (PointToLineCheckAll) { RunPointToLineAll(area); return; }
            if (!TryResolveSourcePoint(out PointF pointWorld))
                return;

            PointToLineCenter = Geometry2D.WorldToAreaLocal(pointWorld, area);
            LineDirectionMode dirMode = ReferenceLineOrientation == LineOrientation.Vertical
                ? LineDirectionMode.Vertical
                : LineDirectionMode.Horizontal;

            ReferenceLineL = RansacLine.FindBestLine(
                matProcess.Data,
                matProcess.Width,
                matProcess.Height,
                (int)matProcess.Step(),
                RansacIterations,
                (float)RansacThreshold,
                120000,
                Index,
                1 / Scale,
                0.2f,
                dirMode,
                Geometry2D.ToCliScanMode(ReferenceLineScan),
                0,
                ReferenceLineAngleRange);

            if (!ReferenceLineL.Found)
                return;

            PointToLineFoot = Geometry2D.ProjectPointToLine(PointToLineCenter, ReferenceLineL);
            WidthResult = (float)(Geometry2D.DistancePointToLine(PointToLineCenter, ReferenceLineL) / Scale);
            PointToLineFound = true;
            listP_Center = new List<System.Drawing.Point> { new System.Drawing.Point((int)Math.Round(pointWorld.X), (int)Math.Round(pointWorld.Y)) };
            rectRotates = new List<RectRotate>();
        }

        private void RunPointToLineAll(RectRotate area)
        {
            var tools = Common.EnsureToolList(IndexThread);
            if (tools == null) return;

            PropetyTool source = null;
            if (!string.IsNullOrEmpty(PointSourceToolName))
                source = tools.FirstOrDefault(t => t != null && t.Name == PointSourceToolName);
            if (source == null)
                source = tools.FirstOrDefault(t => t != null && t.TypeTool == TypeTool.Pitch);
            if (source?.Propety2 == null) return;

            var pts = source.Propety2.listP_Center as List<System.Drawing.Point>;
            if (pts == null || pts.Count == 0) return;
            if (string.IsNullOrEmpty(PointSourceToolName)) PointSourceToolName = source.Name;

            LineDirectionMode dirMode = ReferenceLineOrientation == LineOrientation.Vertical
                ? LineDirectionMode.Vertical : LineDirectionMode.Horizontal;
            ReferenceLineL = RansacLine.FindBestLine(
                matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                RansacIterations, (float)RansacThreshold, 120000, Index, 1 / Scale, 0.2f,
                dirMode, Geometry2D.ToCliScanMode(ReferenceLineScan), 0, ReferenceLineAngleRange);
            if (!ReferenceLineL.Found) return;

            AllPointResults = new List<(PointF, PointF, float)>();
            float maxDist = 0;
            PointF worstCenter = PointF.Empty, worstFoot = PointF.Empty;
            foreach (var p in pts)
            {
                PointF ptLocal = Geometry2D.WorldToAreaLocal(new PointF(p.X, p.Y), area);
                PointF foot = Geometry2D.ProjectPointToLine(ptLocal, ReferenceLineL);
                float dist = (float)(Geometry2D.DistancePointToLine(ptLocal, ReferenceLineL) / Scale);
                AllPointResults.Add((ptLocal, foot, dist));
                if (dist > maxDist) { maxDist = dist; worstCenter = ptLocal; worstFoot = foot; }
            }
            PointToLineCenter = worstCenter;
            PointToLineFoot = worstFoot;
            WidthResult = maxDist;
            PointToLineFound = true;
            listP_Center = pts.ToList();
            rectRotates = new List<RectRotate>();
        }

        private bool TryResolveSourcePoint(out PointF point)
        {
            point = PointF.Empty;
            var tools = Common.EnsureToolList(IndexThread);
            if (tools == null || tools.Count == 0)
                return false;

            PropetyTool source = null;
            if (!string.IsNullOrEmpty(PointSourceToolName))
                source = tools.FirstOrDefault(t => t != null && t.Name == PointSourceToolName);

            if (source == null)
                source = tools.FirstOrDefault(t => t != null && t.TypeTool == TypeTool.Pitch);

            if (source == null || source.Propety2 == null)
                return false;

            try
            {
                var pts = source.Propety2.listP_Center as List<System.Drawing.Point>;
                if (pts == null || PointSourceIndex < 0 || PointSourceIndex >= pts.Count)
                    return false;

                var p = pts[PointSourceIndex];
                point = new PointF(p.X, p.Y);
                if (string.IsNullOrEmpty(PointSourceToolName))
                    PointSourceToolName = source.Name;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Complete()
        {
            if (MeasureMode == WidthMeasureMode.PointToLine)
            {
                CompletePointToLine();
                return;
            }

            switch (SegmentStatType)
            {
                case SegmentStatType.Average:
                    WidthResult = (float)GapResult.GapMedium /Scale;
                    break;
                case SegmentStatType.Shortest:
                    WidthResult = (float)GapResult.GapMin / Scale;
                    break;
                case SegmentStatType.Longest:
                    WidthResult = (float)GapResult.GapMax / Scale;
                    break;
            }
          
            if (IsCalibs)
            {
                MinInliers = (int)(GapResult.Inlier * (80 / 100.0));
                double Delta =  Common.TryGetTool(IndexThread, Index).Score / 100.0;
                MinLen = (int)((GapResult.GapMin/Scale) * (1 + Delta));
                MaxLen = (int)((GapResult.GapMax/Scale )* (1 + Delta));
                if (!Global.IsRun)
                {


                    WidthTemp = WidthResult;
                }

            }
            float scoreResult = 0;
            if (Math.Abs(WidthTemp) > float.Epsilon)
                scoreResult = (float)Math.Round(Math.Abs((WidthResult - WidthTemp) / WidthTemp) * 100f, 1);
            Common.TryGetTool(IndexThread, Index).ScoreResult = scoreResult;
            if (GapResult.line2Ds ==null)
            {
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
            }
            else if (Common.TryGetTool(IndexThread, Index).ScoreResult <= Common.TryGetTool(IndexThread, Index).Score)
            {
                Common.TryGetTool(IndexThread, Index).Results = Results.OK;
                if (!Global.IsRun)
                {


                    WidthTemp = WidthResult;
                }
            }
            else
            {
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
            }    
            //  IsOK = true;
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


        }
        private void CompletePointToLine()
        {
            PropetyTool owner = Common.TryGetTool(IndexThread, Index);
            if (owner == null)
                return;

            if (!PointToLineFound)
            {
                owner.ScoreResult = 999;
                owner.Results = Results.NG;
                return;
            }

            if (IsCalibs && !Global.IsRun)
            {
                PointToLineNominalMm = WidthResult;
                WidthTemp = WidthResult;
            }

            float nominal = PointToLineNominalMm > 0 ? PointToLineNominalMm : WidthTemp;
            float tolerance = owner.Score;

            if (PointToLineCheckAll && AllPointResults != null && AllPointResults.Count > 0)
            {
                float maxDev = 0;
                foreach (var r in AllPointResults)
                {
                    float dev = nominal > 0 ? Math.Abs(r.dist - nominal) : 0;
                    if (dev > maxDev) maxDev = dev;
                }
                owner.ScoreResult = maxDev;
                owner.Results = maxDev <= tolerance ? Results.OK : Results.NG;
            }
            else
            {
                float deviation = nominal > 0 ? Math.Abs(WidthResult - nominal) : 0;
                owner.ScoreResult =(float)Math.Round( deviation,2);
                owner.Results = deviation <= tolerance ? Results.OK : Results.NG;
            }
        }
        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        public bool IsSendResult;
        public async Task SendResult()
        {
            if (IsSendResult)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                   
                    await Global.Comunication.Protocol.WriteResultFloat(AddPLC, WidthResult);
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
            Pen pen = new Pen(Global.ParaShow.ColorInfor, Global.ParaShow.ThicknessLine);
            String nameTool = (int)(Index + 1) + "." + Common.TryGetTool(Global.IndexProgChoose, Index).Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (Global.ParaShow.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl,  Global.ParaShow.ThicknessLine);

            if (!Global.IsRun||Global.ParaShow.IsShowDetail)
            {
                if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rotA, Global.ScaleZoom * 100, Global.pScroll, cl, Global.ParaShow.Opacity / 100.0f);
            }
            gc.ResetTransform();
            if (MeasureMode == WidthMeasureMode.PointToLine)
                return DrawPointToLine(gc, rotA, cl);

            if (GapResult.line2Ds == null) return gc;
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
            RectangleF rectClient =   new RectangleF(0, 0, rotA._rect.Width, rotA._rect.Height);
            if (!Global.IsRun)
                foreach (var l in GapResult.line2Ds)
                Draws.DrawInfiniteLine(gc, l, new Pen(Color.Gray, 2), rectClient);
            Draws.DrawInfiniteLine(gc,GapResult.LineA, new Pen(cl, Global.ParaShow.ThicknessLine), rectClient);
            Draws.DrawInfiniteLine(gc,GapResult.LineB, new Pen(cl, Global.ParaShow.ThicknessLine), rectClient);
            PointF p1 = new PointF(GapResult.lineMid[0].X,GapResult.lineMid[0].Y);
            PointF p2 = new PointF(GapResult.lineMid[1].X,GapResult.lineMid[1].Y);
            Draws.DrawTicks(gc, p1,LineOrientation, pen);
            Draws.DrawTicks(gc, p2,LineOrientation, pen);
            gc.DrawLine(pen, p1, p2);          
            gc.DrawString($"{WidthResult:F2}mm", new Font("Arial", Global.ParaShow.FontSize), new SolidBrush(Global.ParaShow.ColorInfor), p1.X + 5, (p1.Y + p2.Y) / 2 + 10);
            gc.ResetTransform();
            return gc;
        }

        private Graphics DrawPointToLine(Graphics gc, RectRotate rotA, Color cl)
        {
            if (!PointToLineFound || !ReferenceLineL.Found)
                return gc;

            var mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            mat.Translate(rotA._rect.X, rotA._rect.Y);
            gc.Transform = mat;

            RectangleF rectClient = new RectangleF(0, 0, rotA._rect.Width, rotA._rect.Height);
            using (Pen linePen = new Pen(cl, Global.ParaShow.ThicknessLine))
            using (Pen measurePen = new Pen(Global.ParaShow.ColorInfor, Global.ParaShow.ThicknessLine))
            using (Font font = new Font("Arial", Global.ParaShow.FontSize))
            using (Brush brush = new SolidBrush(Global.ParaShow.ColorInfor))
            {
                // Draw reference line as a finite segment spanning the foot points + margin
                {
                    float vx = ReferenceLineL.Vx, vy = ReferenceLineL.Vy;
                    float x0 = ReferenceLineL.X0, y0 = ReferenceLineL.Y0;
                    float margin = Math.Max(20f, Math.Min(rectClient.Width, rectClient.Height) * 0.05f);
                    float tMin, tMax;
                    if (PointToLineCheckAll && AllPointResults != null && AllPointResults.Count > 0)
                    {
                        tMin = float.MaxValue; tMax = float.MinValue;
                        foreach (var r in AllPointResults)
                        {
                            float t = (r.foot.X - x0) * vx + (r.foot.Y - y0) * vy;
                            if (t < tMin) tMin = t;
                            if (t > tMax) tMax = t;
                        }
                    }
                    else
                    {
                        float t = (PointToLineFoot.X - x0) * vx + (PointToLineFoot.Y - y0) * vy;
                        tMin = tMax = t;
                    }
                    PointF segA = new PointF(x0 + (tMin - margin) * vx, y0 + (tMin - margin) * vy);
                    PointF segB = new PointF(x0 + (tMax + margin) * vx, y0 + (tMax + margin) * vy);
                    gc.DrawLine(linePen, segA, segB);
                }

                if (PointToLineCheckAll && AllPointResults != null && AllPointResults.Count > 0)
                {
                    PropetyTool owner = Common.TryGetTool(IndexThread, Index);
                    float tolerance = owner != null ? owner.Score : float.MaxValue;
                    float nominal = PointToLineNominalMm > 0 ? PointToLineNominalMm : WidthTemp;
                    for (int i = 0; i < AllPointResults.Count; i++)
                    {
                        var r = AllPointResults[i];
                        float dev = nominal > 0 ? Math.Abs(r.dist - nominal) : 0;
                        Color ptColor = dev <= tolerance ? Global.ParaShow.ColorOK : Global.ParaShow.ColorNG;
                        using (Pen ptPen = new Pen(ptColor, Global.ParaShow.ThicknessLine))
                        {
                            gc.DrawLine(ptPen, r.center, r.foot);
                            Draws.Plus(gc, (int)Math.Round(r.center.X), (int)Math.Round(r.center.Y), 14, ptColor, 2);
                            gc.FillEllipse(new SolidBrush(ptColor), r.foot.X - 4, r.foot.Y - 4, 8, 8);
                        }
                        PointF mid = new PointF((r.center.X + r.foot.X) / 2, (r.center.Y + r.foot.Y) / 2);
                        gc.DrawString($"P{i + 1}:{r.dist:F3} mm", font, brush, mid.X + 5, mid.Y + 5);
                    }
                }
                else
                {
                    gc.DrawLine(measurePen, PointToLineCenter, PointToLineFoot);
                    Draws.Plus(gc, (int)Math.Round(PointToLineCenter.X), (int)Math.Round(PointToLineCenter.Y), 16, Color.Yellow, 2);
                    gc.FillEllipse(Brushes.Red, PointToLineFoot.X - 4, PointToLineFoot.Y - 4, 8, 8);
                    PointF mid = new PointF((PointToLineCenter.X + PointToLineFoot.X) / 2, (PointToLineCenter.Y + PointToLineFoot.Y) / 2);
                    gc.DrawString($"{WidthResult:F3}mm", font, brush, mid.X + 5, mid.Y + 5);
                }
            }

            gc.ResetTransform();
            return gc;
        }

        public int MaxThread = 0;
        public void SetModel(bool IsCopy=false)
        {

            if (rotArea == null) rotArea = new RectRotate();
            if (rotCrop == null) rotCrop = new RectRotate();
            if (rotMask == null) rotMask = new RectRotate();

  


            rotMask.Name = "Area Mask";
            rotMask.TypeCrop = TypeCrop.Mask;

            rotArea.Name = "Area Check";
            rotArea.TypeCrop = TypeCrop.Area;
            ParallelGapDetector = new ParallelGapDetector();
            RansacLine = new RansacLine();
            Common.TryGetTool(IndexThread, Index).StepValue = 0.1f;
            Common.TryGetTool(IndexThread, Index).MinValue = 0;
          
            Common.TryGetTool(IndexThread, Index).MaxValue = 20;
            Common.TryGetTool(IndexThread, Index). StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
