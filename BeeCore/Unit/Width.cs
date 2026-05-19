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
        public RansacLine RansacLine;
   
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
        [System.Runtime.Serialization.OptionalField]
        public WidthScanDirection ScanDirection = WidthScanDirection.OutToInside;
        [System.Runtime.Serialization.OptionalField]
        public int LengthScan = 20;
        [NonSerialized]
        public List<RectangleF> ScanBoxes = new List<RectangleF>();
        [NonSerialized]
        public Line2DCli EdgeLineA = new Line2DCli();
        [NonSerialized]
        public Line2DCli EdgeLineB = new Line2DCli();
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
          
            
            ReferenceLineOrientation = LineOrientation.Horizontal;
            ReferenceLineAngleRange = 10;
            ReferenceLineScan = LineDirScan.TopBot;
            GapExtremum = GapExtremum.Middle;
            LineOrientation = LineOrientation.Vertical;
            ScanDirection = WidthScanDirection.OutToInside;
            LengthScan = 20;
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
                  
                    RunParallelGapByEdges();
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
        private void RunParallelGapByEdges()
        {
            GapResult = new GapResult();
            if (ScanBoxes == null) ScanBoxes = new List<RectangleF>();
            ScanBoxes.Clear();
            EdgeLineA = new Line2DCli();
            EdgeLineB = new Line2DCli();

            if (matProcess == null || matProcess.Empty())
                return;

            var scanJobs = BuildWidthScanBoxes();
            if (scanJobs.Count == 0)
                return;

            if (scanJobs.Count == 1)
            {
                var job = scanJobs[0];
                EdgeLineA = FindLineInScanBox(job.Box, job.FirstScanMode);
                EdgeLineB = FindLineInScanBox(job.Box, job.SecondScanMode);
            }
            else
            {
                EdgeLineA= FindLineInScanBox(scanJobs[0].Box, scanJobs[0].FirstScanMode);
                EdgeLineB= FindLineInScanBox(scanJobs[1].Box, scanJobs[1].FirstScanMode);
                //Task<Line2DCli> edgeATask = Task.Run(() => 
                //Task<Line2DCli> edgeBTask = Task.Run(() => 
                //Task.WaitAll(edgeATask, edgeBTask);
                //EdgeLineA = edgeATask.Result;
                //EdgeLineB = edgeBTask.Result;
            }

            if (!EdgeLineA.Found || !EdgeLineB.Found)
                return;

            GapResult = BuildGapResultFromEdgeLines(EdgeLineA, EdgeLineB);
        }

        private List<WidthScanJob> BuildWidthScanBoxes()
        {
            var jobs = new List<WidthScanJob>();
            int width = matProcess.Width;
            int height = matProcess.Height;
            if (width <= 0 || height <= 0)
                return jobs;

            bool vertical = LineOrientation != LineOrientation.Horizontal;
            int length = Math.Max(1, LengthScan);
            if (vertical)
            {
                length = Math.Min(length, width);
                if (ScanDirection == WidthScanDirection.InsideToOut)
                {
                    float x = (width - length) / 2f;
                    var box = new RectangleF(x, 0, length, height);
                    ScanBoxes.Add(box);
                    jobs.Add(new WidthScanJob(box, LineScanMode.RightToLeft, LineScanMode.LeftToRight));
                }
                else
                {
                    var left = new RectangleF(0, 0, length, height);
                    var right = new RectangleF(width - length, 0, length, height);
                    ScanBoxes.Add(left);
                    ScanBoxes.Add(right);
                    jobs.Add(new WidthScanJob(left, LineScanMode.LeftToRight, LineScanMode.None));
                    jobs.Add(new WidthScanJob(right, LineScanMode.RightToLeft, LineScanMode.None));
                }
            }
            else
            {
                length = Math.Min(length, height);
                if (ScanDirection == WidthScanDirection.InsideToOut)
                {
                    float y = (height - length) / 2f;
                    var box = new RectangleF(0, y, width, length);
                    ScanBoxes.Add(box);
                    jobs.Add(new WidthScanJob(box, LineScanMode.BottomToTop, LineScanMode.TopToBottom));
                }
                else
                {
                    var top = new RectangleF(0, 0, width, length);
                    var bottom = new RectangleF(0, height - length, width, length);
                    ScanBoxes.Add(top);
                    ScanBoxes.Add(bottom);
                    jobs.Add(new WidthScanJob(top, LineScanMode.TopToBottom, LineScanMode.None));
                    jobs.Add(new WidthScanJob(bottom, LineScanMode.BottomToTop, LineScanMode.None));
                }
            }

            return jobs;
        }

        private Line2DCli FindLineInScanBox(RectangleF box, LineScanMode scanMode)
        {
            Line2DCli empty = new Line2DCli();
            int x = Math.Max(0, (int)Math.Floor(box.X));
            int y = Math.Max(0, (int)Math.Floor(box.Y));
            int w = Math.Min(matProcess.Width - x, Math.Max(1, (int)Math.Round(box.Width)));
            int h = Math.Min(matProcess.Height - y, Math.Max(1, (int)Math.Round(box.Height)));
            if (w <= 0 || h <= 0)
                return empty;

            using (Mat scanMat = new Mat(matProcess, new OpenCvSharp.Rect(x, y, w, h)))
            {
                LineDirectionMode dirMode = LineOrientation == LineOrientation.Horizontal
                    ? LineDirectionMode.Horizontal
                    : LineDirectionMode.Vertical;
                float mmPerPixel = Math.Abs(Scale) > float.Epsilon ? 1 / Scale : 1;
                Line2DCli line = RansacLine.FindBestLine(
                    scanMat.Data,
                    scanMat.Width,
                    scanMat.Height,
                    (int)scanMat.Step(),
                    RansacIterations,
                    (float)RansacThreshold,
                    120000,
                    Index,
                    mmPerPixel,
                    0.2f,
                    dirMode,
                    scanMode,
                    0,
                    10);

                if (!line.Found || line.Inliers < MinInliers)
                    return empty;

                return OffsetLine(line, x, y);
            }
        }

        private static Line2DCli OffsetLine(Line2DCli line, float dx, float dy)
        {
            line.X1 += dx;
            line.X2 += dx;
            line.X0 += dx;
            line.Y1 += dy;
            line.Y2 += dy;
            line.Y0 += dy;
            return line;
        }

        private static Line2D ToLine2D(Line2DCli line)
        {
            return new Line2D(line.Vx, line.Vy, line.X0, line.Y0);
        }

        private GapResult BuildGapResultFromEdgeLines(Line2DCli edgeA, Line2DCli edgeB)
        {
            bool vertical = LineOrientation != LineOrientation.Horizontal;
            Line2D lineA = ToLine2D(edgeA);
            Line2D lineB = ToLine2D(edgeB);

            double SolveX(Line2DCli ln, double y)
            {
                if (Math.Abs(ln.Vy) < 1e-6) return ln.X0;
                double t = (y - ln.Y0) / ln.Vy;
                return ln.X0 + ln.Vx * t;
            }

            double SolveY(Line2DCli ln, double x)
            {
                if (Math.Abs(ln.Vx) < 1e-6) return ln.Y0;
                double t = (x - ln.X0) / ln.Vx;
                return ln.Y0 + ln.Vy * t;
            }

            double minAxisA = vertical ? Math.Min(edgeA.Y1, edgeA.Y2) : Math.Min(edgeA.X1, edgeA.X2);
            double maxAxisA = vertical ? Math.Max(edgeA.Y1, edgeA.Y2) : Math.Max(edgeA.X1, edgeA.X2);
            double minAxisB = vertical ? Math.Min(edgeB.Y1, edgeB.Y2) : Math.Min(edgeB.X1, edgeB.X2);
            double maxAxisB = vertical ? Math.Max(edgeB.Y1, edgeB.Y2) : Math.Max(edgeB.X1, edgeB.X2);
            double axisStart = Math.Max(minAxisA, minAxisB);
            double axisEnd = Math.Min(maxAxisA, maxAxisB);
            if (axisEnd < axisStart)
            {
                axisStart = 0;
                axisEnd = vertical ? matProcess.Height : matProcess.Width;
            }

            double shortPx;
            double longPx;
            double mediumPx;
            Point[] lineMids = new Point[2];

            if (vertical)
            {
                double distStart = Math.Abs(SolveX(edgeB, axisStart) - SolveX(edgeA, axisStart));
                double distEnd = Math.Abs(SolveX(edgeB, axisEnd) - SolveX(edgeA, axisEnd));
                shortPx = Math.Min(distStart, distEnd);
                longPx = Math.Max(distStart, distEnd);
                double shortAxis = distStart <= distEnd ? axisStart : axisEnd;
                double longAxis = distStart <= distEnd ? axisEnd : axisStart;
                double midAxis = (axisStart + axisEnd) / 2.0;

                switch (SegmentStatType)
                {
                    case SegmentStatType.Shortest:
                        lineMids[0] = new Point((int)Math.Round(SolveX(edgeA, shortAxis)), (int)Math.Round(shortAxis));
                        lineMids[1] = new Point((int)Math.Round(SolveX(edgeB, shortAxis)), (int)Math.Round(shortAxis));
                        break;
                    case SegmentStatType.Longest:
                        lineMids[0] = new Point((int)Math.Round(SolveX(edgeA, longAxis)), (int)Math.Round(longAxis));
                        lineMids[1] = new Point((int)Math.Round(SolveX(edgeB, longAxis)), (int)Math.Round(longAxis));
                        break;
                    default:
                        lineMids[0] = new Point((int)Math.Round(SolveX(edgeA, midAxis)), (int)Math.Round(midAxis));
                        lineMids[1] = new Point((int)Math.Round(SolveX(edgeB, midAxis)), (int)Math.Round(midAxis));
                        break;
                }
                mediumPx = Math.Abs(lineMids[1].X - lineMids[0].X);
            }
            else
            {
                double distStart = Math.Abs(SolveY(edgeB, axisStart) - SolveY(edgeA, axisStart));
                double distEnd = Math.Abs(SolveY(edgeB, axisEnd) - SolveY(edgeA, axisEnd));
                shortPx = Math.Min(distStart, distEnd);
                longPx = Math.Max(distStart, distEnd);
                double shortAxis = distStart <= distEnd ? axisStart : axisEnd;
                double longAxis = distStart <= distEnd ? axisEnd : axisStart;
                double midAxis = (axisStart + axisEnd) / 2.0;

                switch (SegmentStatType)
                {
                    case SegmentStatType.Shortest:
                        lineMids[0] = new Point((int)Math.Round(shortAxis), (int)Math.Round(SolveY(edgeA, shortAxis)));
                        lineMids[1] = new Point((int)Math.Round(shortAxis), (int)Math.Round(SolveY(edgeB, shortAxis)));
                        break;
                    case SegmentStatType.Longest:
                        lineMids[0] = new Point((int)Math.Round(longAxis), (int)Math.Round(SolveY(edgeA, longAxis)));
                        lineMids[1] = new Point((int)Math.Round(longAxis), (int)Math.Round(SolveY(edgeB, longAxis)));
                        break;
                    default:
                        lineMids[0] = new Point((int)Math.Round(midAxis), (int)Math.Round(SolveY(edgeA, midAxis)));
                        lineMids[1] = new Point((int)Math.Round(midAxis), (int)Math.Round(SolveY(edgeB, midAxis)));
                        break;
                }
                mediumPx = Math.Abs(lineMids[1].Y - lineMids[0].Y);
            }

            return new GapResult
            {
                line2Ds = new List<Line2D> { lineA, lineB },
                LineA = lineA,
                LineB = lineB,
                lineMid = lineMids,
                GapMin = shortPx,
                GapMedium = mediumPx,
                GapMax = longPx,
                Inlier = Math.Min(edgeA.Inliers, edgeB.Inliers)
            };
        }

        private struct WidthScanJob
        {
            public WidthScanJob(RectangleF box, LineScanMode firstScanMode, LineScanMode secondScanMode)
            {
                Box = box;
                FirstScanMode = firstScanMode;
                SecondScanMode = secondScanMode;
            }

            public RectangleF Box;
            public LineScanMode FirstScanMode;
            public LineScanMode SecondScanMode;
        }

      

      
    
        public void Complete()
        {
         

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
            DrawScanBoxes(gc);
            if (GapResult.line2Ds == null)
            {
                gc.ResetTransform();
                return gc;
            }
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

        private void DrawScanBoxes(Graphics gc)
        {
            if (ScanBoxes == null || ScanBoxes.Count == 0)
                return;

            Color scanColor = Color.Orange;// LineOrientation.Horizontal ? Color.Orange : Color.Red;
            using (Pen scanPen = new Pen(scanColor, Math.Max(1, Global.ParaShow.ThicknessLine)))
            {
                foreach (RectangleF box in ScanBoxes)
                    gc.DrawRectangle(scanPen, box.X, box.Y, box.Width, box.Height);
            }
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
