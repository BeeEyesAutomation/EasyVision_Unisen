
using BeeCore.Func;
using BeeCpp;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Point = System.Drawing.Point;

namespace BeeCore
{
    [Serializable()]
    public class Measure
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int IndexCCD = 0; 
        [NonSerialized]
        public bool IsNew = false;
        public float ValueSample = 0;
        public void SetModel()
        {
            rotMask = null;
            rotCrop = null;
            rotArea = null;
            Common.TryGetTool(IndexThread, Index).StepValue = 0.01f;
            Common.TryGetTool(IndexThread, Index).MinValue = 0;
            Common.TryGetTool(IndexThread, Index).MaxValue = 180;
            Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;
            EnsureSelectionLists();
            listLine1Point = new List<Point>();
            listLine2Point = new List<Point>();
            listLine1Point.Add(new Point(0, 0));
            listLine1Point.Add(new Point(0, 0));
            listLine2Point.Add(new Point(0, 0));
            listLine2Point.Add(new Point(0, 0));
        }
        public int MaxThread = 0;
        public float Scale = 1;
        public TypeMeasure TypeMeasure = TypeMeasure.Angle;
        public DirectMeasure DirectMeasure = DirectMeasure.X;
        public MethordMeasure MethordMeasure = MethordMeasure.Min;
        public MeasureLineInputMode Line1InputMode = MeasureLineInputMode.Point;
        public MeasureLineInputMode Line2InputMode = MeasureLineInputMode.Point;
      
        public Measure() { }
        public bool IsCheckArea = false;
        public String nameTool = "";
        public int Index = 0;
        public double AngleDetect = 0;
        public double Distance = 0;
        public int IndexThread = 0;

        
       
        public List<RectRotate> listRot = new List<RectRotate> { new RectRotate(), new RectRotate(), new RectRotate(), new RectRotate() };
        public List<Point> listLine1Point = new List<Point>();
        public List<Point> listLine2Point = new List<Point>();
        public List<Tuple<String, int>> listPointChoose = new List<Tuple<String, int>>();
        public List<Tuple<String, int>> listLineChoose = new List<Tuple<String, int>>();
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        [System.Runtime.Serialization.OptionalField]
        public MeasureMethod MeasureMethod = MeasureMethod.PointToPoint;
        [System.Runtime.Serialization.OptionalField]
        public string MultiPointSrcToolName = "";
        [NonSerialized]
        public List<double> MultiPointDistances = new List<double>();
        [NonSerialized]
        public List<bool> MultiPointOkList = new List<bool>();
        [NonSerialized]
        public double PointToLineResult = 0;

        [NonSerialized]
        private bool IsDone1=false,  IsDone2 = false,  IsDone3 = false,  IsDone4 = false;
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            try
            {
            X: Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.OK;

                EnsureSelectionLists();
                var tools = BeeCore.Common.TryGetToolList(IndexThread);
                if (tools == null) return;

                bool failed = false;
                bool waiting = false;
                if (!IsDone1 || !IsDone2)
                {
                    if (TryResolveMeasureLine(tools, Line1InputMode, listPointChoose[0], listPointChoose[1], listLineChoose[0], listLine1Point, 0, out waiting))
                    {
                        IsDone1 = true;
                        IsDone2 = true;
                    }
                    else if (!waiting)
                    {
                        failed = true;
                        IsDone1 = true;
                        IsDone2 = true;
                    }
                }

                bool needFullLine2 = TypeMeasure == TypeMeasure.Angle
                    || (TypeMeasure == TypeMeasure.Distance && MeasureMethod == MeasureMethod.LineToLine);
                bool needSingleSrcPt = TypeMeasure == TypeMeasure.Distance && MeasureMethod == MeasureMethod.PointToLine;
                bool skipLine2 = TypeMeasure == TypeMeasure.Distance
                    && (MeasureMethod == MeasureMethod.PointToPoint || MeasureMethod == MeasureMethod.MultiPointToLine);

                if (skipLine2)
                {
                    IsDone3 = true;
                    IsDone4 = true;
                }
                else if (needSingleSrcPt)
                {
                    IsDone4 = true;
                    if (!IsDone3)
                    {
                        PropetyTool srcTool = listPointChoose.Count > 2
                            ? FindToolByName(tools, listPointChoose[2]?.Item1) : null;
                        if (srcTool == null) { failed = true; IsDone3 = true; }
                        else if (!IsToolReady(srcTool)) { waiting = true; }
                        else if (srcTool.Results != Results.OK) { failed = true; IsDone3 = true; }
                        else
                        {
                            int idx = listPointChoose[2]?.Item2 ?? 0;
                            if (idx >= 0 && idx < srcTool.Propety2.listP_Center.Count)
                                listLine2Point[0] = srcTool.Propety2.listP_Center[idx];
                            IsDone3 = true;
                        }
                    }
                }
                else if (needFullLine2 && (!IsDone3 || !IsDone4))
                {
                    if (TryResolveMeasureLine(tools, Line2InputMode, listPointChoose[2], listPointChoose[3], listLineChoose[1], listLine2Point, 2, out waiting))
                    {
                        IsDone3 = true;
                        IsDone4 = true;
                    }
                    else if (!waiting)
                    {
                        failed = true;
                        IsDone3 = true;
                        IsDone4 = true;
                    }
                }

                if (failed)
                    Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.NG;

                if (!IsDone1 || !IsDone2 || !IsDone3 || !IsDone4)
                    goto X;
            }
            catch(Exception ex)
            {
                Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.NG;
              
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "RetTrain", ex.Message.ToString()));
            }
        }

        public void EnsureSelectionLists()
        {
            if (listRot == null)
                listRot = new List<RectRotate>();
            while (listRot.Count < 4)
                listRot.Add(new RectRotate());

            if (listPointChoose == null)
                listPointChoose = new List<Tuple<String, int>>();
            while (listPointChoose.Count < 4)
                listPointChoose.Add(new Tuple<String, int>(null, -1));

            if (listLineChoose == null)
                listLineChoose = new List<Tuple<String, int>>();
            while (listLineChoose.Count < 2)
                listLineChoose.Add(new Tuple<String, int>(null, -1));

            if (listLine1Point == null)
                listLine1Point = new List<Point>();
            while (listLine1Point.Count < 2)
                listLine1Point.Add(new Point(0, 0));

            if (listLine2Point == null)
                listLine2Point = new List<Point>();
            while (listLine2Point.Count < 2)
                listLine2Point.Add(new Point(0, 0));
        }

        private bool TryResolveMeasureLine(IReadOnlyList<PropetyTool> tools, MeasureLineInputMode mode, Tuple<string, int> firstPoint, Tuple<string, int> secondPoint, Tuple<string, int> lineChoice, List<Point> targetPoints, int rotOffset, out bool waiting)
        {
            waiting = false;
            if (mode == MeasureLineInputMode.Line)
                return TryResolveToolLine(tools, lineChoice, targetPoints, rotOffset, out waiting);

            return TryResolvePointPair(tools, firstPoint, secondPoint, targetPoints, rotOffset, out waiting);
        }

        private bool TryResolvePointPair(IReadOnlyList<PropetyTool> tools, Tuple<string, int> firstPoint, Tuple<string, int> secondPoint, List<Point> targetPoints, int rotOffset, out bool waiting)
        {
            waiting = false;
            PropetyTool firstTool = FindToolByName(tools, firstPoint != null ? firstPoint.Item1 : null);
            PropetyTool secondTool = FindToolByName(tools, secondPoint != null ? secondPoint.Item1 : null);
            if (firstTool == null || secondTool == null)
                return false;

            if (!IsToolReady(firstTool) || !IsToolReady(secondTool))
            {
                waiting = true;
                return false;
            }

            if (firstTool.Results != Results.OK || secondTool.Results != Results.OK)
                return false;

            return TryCopyPoint(firstTool, firstPoint.Item2, targetPoints, 0, rotOffset)
                && TryCopyPoint(secondTool, secondPoint.Item2, targetPoints, 1, rotOffset + 1);
        }

        private bool TryResolveToolLine(IReadOnlyList<PropetyTool> tools, Tuple<string, int> lineChoice, List<Point> targetPoints, int rotOffset, out bool waiting)
        {
            waiting = false;
            PropetyTool lineTool = FindToolByName(tools, lineChoice != null ? lineChoice.Item1 : null);
            if (lineTool == null || (lineTool.TypeTool != TypeTool.Edge && lineTool.TypeTool != TypeTool.Edge2))
                return false;

            if (!IsToolReady(lineTool))
            {
                waiting = true;
                return false;
            }

            if (lineTool.Results != Results.OK)
                return false;

            if (TryCopyLineFromToolPoints(lineTool, targetPoints, rotOffset))
                return true;

            return TryCopyLine2D(lineTool, targetPoints, rotOffset);
        }

        private static PropetyTool FindToolByName(IReadOnlyList<PropetyTool> tools, string name)
        {
            if (tools == null || string.IsNullOrEmpty(name))
                return null;
            return tools.FirstOrDefault(a => a != null && a.Name == name);
        }

        private static bool IsToolReady(PropetyTool tool)
        {
            return tool.StatusTool == StatusTool.Done || !Global.IsRun;
        }

        private bool TryCopyPoint(PropetyTool tool, int sourceIndex, List<Point> targetPoints, int targetIndex, int rotIndex)
        {
            if (sourceIndex < 0 || sourceIndex >= tool.Propety2.listP_Center.Count)
                return false;

            targetPoints[targetIndex] = tool.Propety2.listP_Center[sourceIndex];
            if (sourceIndex < tool.Propety2.rectRotates.Count && rotIndex < listRot.Count)
                listRot[rotIndex] = tool.Propety2.rectRotates[sourceIndex];
            return true;
        }

        private bool TryCopyLineFromToolPoints(PropetyTool tool, List<Point> targetPoints, int rotOffset)
        {
            if (tool.Propety2.listP_Center.Count < 2)
                return false;

            targetPoints[0] = tool.Propety2.listP_Center[0];
            targetPoints[1] = tool.Propety2.listP_Center[1];
            RectRotate rot = tool.Propety2.rectRotates.Count > 0 ? tool.Propety2.rectRotates[0] : new RectRotate();
            if (rotOffset < listRot.Count)
                listRot[rotOffset] = rot;
            if (rotOffset + 1 < listRot.Count)
                listRot[rotOffset + 1] = rot;
            return true;
        }

        private bool TryCopyLine2D(PropetyTool tool, List<Point> targetPoints, int rotOffset)
        {
            Line2DCli line;
            if (!TryGetLine2D(tool, out line))
                return false;

            targetPoints[0] = new Point((int)Math.Round(line.X1), (int)Math.Round(line.Y1));
            targetPoints[1] = new Point((int)Math.Round(line.X2), (int)Math.Round(line.Y2));
            RectRotate rot = new RectRotate();
            if (rotOffset < listRot.Count)
                listRot[rotOffset] = rot;
            if (rotOffset + 1 < listRot.Count)
                listRot[rotOffset + 1] = rot;
            return true;
        }

        private static bool TryGetLine2D(PropetyTool tool, out Line2DCli line)
        {
            line = new Line2DCli();
            if (tool == null)
                return false;

            if (tool.TypeTool == TypeTool.Edge)
            {
                Edge edge = tool.Propety as Edge;
                if (edge != null)
                    line = edge.Line2DCli;
            }
            else if (tool.TypeTool == TypeTool.Edge2)
            {
                Edge2 edge2 = tool.Propety as Edge2;
                if (edge2 != null)
                    line = edge2.Line2DCli;
            }

            return line.Found;
        }
        [NonSerialized]
        private   PointF pCenter1, pCenter2, pCenter3, pCenter4, pIntersection;
        public async Task SendResult()
        {
            if (Common.TryGetTool(IndexThread, Index).IsSendResult)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                    if (TypeMeasure == TypeMeasure.Angle)
                    {
                      //  await Global.Comunication.Protocol.WriteResultFloat(Common.TryGetTool(IndexThread, Index).AddPLC, (float)AngleDetect);
                    }
                }
            }
        }
        public void Complete()
        {
            switch (TypeMeasure)
            {
                case TypeMeasure.Angle:
                    IsDone1 = false;
                    IsDone2 = false;
                    IsDone3 = false;
                    IsDone4 = false;
                    Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = 0;
                    if (Common.TryGetTool(Global.IndexProgChoose, Index).Results == Results.OK)
                    {
                        pCenter1 = listLine1Point[0];
                        pCenter2 = listLine1Point[1];
                        pCenter3 = listLine2Point[0];
                        pCenter4 = listLine2Point[1];
                        Cal.FindIntersection(pCenter1, pCenter2, pCenter3, pCenter4, out pIntersection);
                        AngleDetect = Cal.GetAngleBetweenSegments(pCenter1, pCenter2, pCenter3, pCenter4);
                        AngleDetect = Math.Round(AngleDetect, 1);
                        Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = (float)AngleDetect;
                        if (Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult <= Common.TryGetTool(Global.IndexProgChoose, Index).Score)
                            Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.OK;
                        else Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.NG;
                    }
                    break;

                case TypeMeasure.Distance:
                    IsDone1 = IsDone2 = IsDone3 = IsDone4 = false;
                    switch (MeasureMethod)
                    {
                        case MeasureMethod.PointToPoint:
                            switch (DirectMeasure)
                            {
                                case DirectMeasure.XY:
                                    pCenter1 = listLine1Point[0];
                                    pCenter2 = listLine1Point[1];
                                    AngleDetect = Cal.AngleDeg_FromB_ToA_MathYUp(pCenter2, pCenter1);
                                    Distance = Cal.Finddistasnce(pCenter1, pCenter2) / Scale;
                                    break;
                                case DirectMeasure.X:
                                    float width = Math.Abs(listRot[0]._rect.Width);
                                    float width2 = Math.Abs(listRot[1]._rect.Width);
                                    float Ymin1 = Math.Min(listLine1Point[0].Y - listRot[0]._rect.Height / 2, listLine1Point[1].Y - listRot[1]._rect.Height / 2);
                                    float Ymax1 = Math.Max(listLine1Point[0].Y + listRot[0]._rect.Height / 2, listLine1Point[1].Y + -listRot[1]._rect.Height / 2);
                                    float Xmin1 = Math.Min(listLine1Point[0].X, listLine1Point[1].X);
                                    float Xmax1 = Math.Max(listLine1Point[0].X, listLine1Point[1].X);
                                    pCenter1 = new PointF(Xmin1, Ymin1);
                                    pCenter2 = new PointF(Xmin1, Ymax1);
                                    pCenter3 = new PointF(Xmax1, Ymin1);
                                    pCenter4 = new PointF(Xmax1, Ymax1);
                                    AngleDetect = Cal.Finddistasnce(pCenter1, pCenter3) / Scale;
                                    break;
                                case DirectMeasure.Y:
                                    float height = Math.Abs(listRot[0]._rect.Height);
                                    float height2 = Math.Abs(listRot[1]._rect.Height);
                                    float Xmin = Math.Min(listLine1Point[0].X, listLine1Point[1].X);
                                    float Xmax = Math.Max(listLine1Point[0].X, listLine1Point[1].X);
                                    pCenter1 = new PointF(Xmin, listLine1Point[0].Y - height / 2);
                                    pCenter2 = new PointF(Xmax, listLine1Point[0].Y - height / 2);
                                    pCenter3 = new PointF(Xmin, listLine1Point[1].Y - height2 / 2);
                                    pCenter4 = new PointF(Xmax, listLine1Point[1].Y - height2 / 2);
                                    AngleDetect = Cal.Finddistasnce(pCenter1, pCenter3) / Scale;
                                    break;
                            }
                            AngleDetect = Math.Round(AngleDetect, 2);
                            Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = (float)AngleDetect;
                            break;

                        case MeasureMethod.PointToLine:
                            if (Common.TryGetTool(Global.IndexProgChoose, Index).Results == Results.OK
                                && listLine1Point.Count >= 2 && listLine2Point.Count >= 1)
                            {
                                var refA = new OpenCvSharp.Point2d(listLine1Point[0].X, listLine1Point[0].Y);
                                var refB = new OpenCvSharp.Point2d(listLine1Point[1].X, listLine1Point[1].Y);
                                var pt = new OpenCvSharp.Point2d(listLine2Point[0].X, listLine2Point[0].Y);
                                PointToLineResult = Geometry2D.PerpendicularDistance(pt, refA, refB) / Scale;
                                PointToLineResult = Math.Round(PointToLineResult, 2);
                                AngleDetect = PointToLineResult;
                                Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = (float)AngleDetect;
                                if (Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult <= Common.TryGetTool(Global.IndexProgChoose, Index).Score)
                                    Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.OK;
                                else Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.NG;
                            }
                            break;

                        case MeasureMethod.LineToLine:
                            if (Common.TryGetTool(Global.IndexProgChoose, Index).Results == Results.OK
                                && listLine1Point.Count >= 2 && listLine2Point.Count >= 2)
                            {
                                var la1 = new OpenCvSharp.Point2d(listLine1Point[0].X, listLine1Point[0].Y);
                                var la2 = new OpenCvSharp.Point2d(listLine1Point[1].X, listLine1Point[1].Y);
                                var lb1 = new OpenCvSharp.Point2d(listLine2Point[0].X, listLine2Point[0].Y);
                                var lb2 = new OpenCvSharp.Point2d(listLine2Point[1].X, listLine2Point[1].Y);
                                double d = (Geometry2D.PerpendicularDistance(la1, lb1, lb2) + Geometry2D.PerpendicularDistance(la2, lb1, lb2)) / 2.0;
                                AngleDetect = Math.Round(d / Scale, 2);
                                Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = (float)AngleDetect;
                                if (Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult <= Common.TryGetTool(Global.IndexProgChoose, Index).Score)
                                    Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.OK;
                                else Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.NG;
                            }
                            break;

                        case MeasureMethod.MultiPointToLine:
                            MultiPointDistances = new List<double>();
                            MultiPointOkList = new List<bool>();
                            if (Common.TryGetTool(Global.IndexProgChoose, Index).Results == Results.OK
                                && listLine1Point.Count >= 2)
                            {
                                var refLA = new OpenCvSharp.Point2d(listLine1Point[0].X, listLine1Point[0].Y);
                                var refLB = new OpenCvSharp.Point2d(listLine1Point[1].X, listLine1Point[1].Y);
                                var allTools = Common.TryGetToolList(IndexThread);
                                var srcTool = FindToolByName(allTools, MultiPointSrcToolName);
                                double threshold = Common.TryGetTool(Global.IndexProgChoose, Index).Score;
                                if (srcTool != null && srcTool.Propety2 != null)
                                {
                                    foreach (var p in srcTool.Propety2.listP_Center)
                                    {
                                        double d = Math.Round(Geometry2D.PerpendicularDistance(new OpenCvSharp.Point2d(p.X, p.Y), refLA, refLB) / Scale, 2);
                                        bool ok =Math.Abs(ValueSample- d) <= threshold;
                                        MultiPointDistances.Add(d);
                                        MultiPointOkList.Add(ok);
                                    }
                                }
                                AngleDetect = GetMultiPointSummaryDistance();
                                Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = (float)AngleDetect;
                                if (MethordMeasure == MethordMeasure.EachOne)
                                    Common.TryGetTool(Global.IndexProgChoose, Index).Results = MultiPointOkList.All(ok => ok) ? Results.OK : Results.NG;
                                else
                                    Common.TryGetTool(Global.IndexProgChoose, Index).Results = AngleDetect <= threshold ? Results.OK : Results.NG;
                            }
                            break;
                    }
                    break;
            }
        }

        private double GetMultiPointSummaryDistance()
        {
            if (MultiPointDistances == null || MultiPointDistances.Count == 0)
                return 0;

            switch (MethordMeasure)
            {
                case MethordMeasure.Min:
                    return MultiPointDistances.Min();
                case MethordMeasure.Max:
                    return MultiPointDistances.Max();
                case MethordMeasure.Medium:
                    return Math.Round(MultiPointDistances.Average(), 2);
                case MethordMeasure.EachOne:
                    return MultiPointDistances.Max();
                default:
                    return MultiPointDistances.Max();
            }
        }

        private string GetMultiPointSummaryLabel()
        {
            switch (MethordMeasure)
            {
                case MethordMeasure.Min:
                    return "Min";
                case MethordMeasure.Max:
                    return "Max";
                case MethordMeasure.Medium:
                    return "Medium";
                case MethordMeasure.EachOne:
                    return "EachOne";
                default:
                    return "Result";
            }
        }

        [NonSerialized]
        public Mat matProcess = new Mat();

        public LineOrientation LineOrientation = LineOrientation.Vertical;
        [NonSerialized]
        public GapResult GapResult = new GapResult();
        public static Line2D MakeXAxisLine(Point2f B)
        {
            return new Line2D(
                B.X,     // X0
                B.Y,     // Y0
                1.0,     // Vx (hướng +X)
                0.0      // Vy
            );
        }
        public Graphics DrawResult(Graphics gc)
        {
          
         

            gc.ResetTransform();
          
          

            var mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
          
            gc.Transform = mat;
            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;
            switch (Common.TryGetTool(Global.IndexProgChoose, Index).Results)
            {
                case Results.OK:
                    cl = Global.ParaShow.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.ParaShow.ColorNG;
                    break;
            }
            // === Vẽ 2 line dựa vào listLine1Point và listLine2Point ===
            switch (TypeMeasure)
            {
                case TypeMeasure.Angle:
                    if (listLine1Point.Count >= 2 && listLine2Point.Count >= 2)
                    {
                        Point p1 = listLine1Point[0];
                        Point p2 = listLine1Point[1];
                        Point p3 = listLine2Point[0];
                        Point p4 = listLine2Point[1];
                        DrawAngleLinesAndArc(gc, p1, p2, p3, p4);
                    }
                    break;

                case TypeMeasure.Distance:
                    switch (MeasureMethod)
                    {
                        case MeasureMethod.PointToPoint:
                        {
                            Point p11 = listLine1Point[0];
                            Point p21 = listLine1Point[1];
                            int r1 = 5;
                            using (SolidBrush redBrush = new SolidBrush(Color.Red))
                            using (Pen yellowPen = new Pen(Color.Yellow, 1))
                            {
                                gc.FillEllipse(redBrush, p11.X - r1, p11.Y - r1, r1 * 2, r1 * 2);
                                gc.DrawEllipse(yellowPen, p11.X - r1, p11.Y - r1, r1 * 2, r1 * 2);
                                gc.FillEllipse(redBrush, p21.X - r1, p21.Y - r1, r1 * 2, r1 * 2);
                                gc.DrawEllipse(yellowPen, p21.X - r1, p21.Y - r1, r1 * 2, r1 * 2);
                            }
                            gc.DrawLine(new Pen(Global.ParaShow.ColorInfor, 2), p11, p21);
                            string txt1 = $"Distance {AngleDetect:F2}";
                            using (Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold))
                            using (SolidBrush brush = new SolidBrush(Global.ParaShow.ColorInfor))
                                gc.DrawString(txt1, font, brush, p11.X + 5, p11.Y + 5);
                            PointF p22 = new PointF(p11.X + 20, p11.Y);
                            PointF p23 = new PointF(p11.X, p11.Y + 20);
                            Draws.DrawInfiniteLine(gc, p11, p22, new Pen(cl, Global.ParaShow.ThicknessLine));
                            Draws.DrawInfiniteLine(gc, p11, p23, new Pen(cl, Global.ParaShow.ThicknessLine));
                            break;
                        }

                        case MeasureMethod.PointToLine:
                            if (listLine1Point.Count >= 2 && listLine2Point.Count >= 1)
                            {
                                PointF refP1 = listLine1Point[0];
                                PointF refP2 = listLine1Point[1];
                                PointF srcPt = listLine2Point[0];
                                using (Pen refPen = new Pen(Color.DeepSkyBlue, Math.Max(1, Global.ParaShow.ThicknessLine)))
                                using (Pen perpPen = new Pen(cl, Math.Max(1, Global.ParaShow.ThicknessLine)))
                                using (Font fnt = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold))
                                using (SolidBrush br = new SolidBrush(Global.ParaShow.ColorInfor))
                                {
                                    Draws.DrawInfiniteLine(gc, refP1, refP2, refPen, 20000f);
                                    var a = new OpenCvSharp.Point2d(refP1.X, refP1.Y);
                                    var b = new OpenCvSharp.Point2d(refP2.X, refP2.Y);
                                    var p = new OpenCvSharp.Point2d(srcPt.X, srcPt.Y);
                                    OpenCvSharp.Point2d foot; double t;
                                    Geometry2D.TryPerpendicularFoot(p, a, b, out foot, out t);
                                    PointF footPt = new PointF((float)foot.X, (float)foot.Y);
                                    gc.DrawLine(perpPen, srcPt, footPt);
                                    Draws.Plus(gc, (int)srcPt.X, (int)srcPt.Y, 6, cl, 2);
                                    Draws.Plus(gc, (int)footPt.X, (int)footPt.Y, 4, Color.Yellow, 1);
                                    gc.DrawString($"{PointToLineResult:F2}mm", fnt, br, srcPt.X + 6, srcPt.Y + 4);
                                }
                            }
                            break;

                        case MeasureMethod.LineToLine:
                            if (listLine1Point.Count >= 2 && listLine2Point.Count >= 2)
                            {
                                PointF ll1p1 = listLine1Point[0], ll1p2 = listLine1Point[1];
                                PointF ll2p1 = listLine2Point[0], ll2p2 = listLine2Point[1];
                                using (Pen pen1 = new Pen(Color.Red, Math.Max(1, Global.ParaShow.ThicknessLine)))
                                using (Pen pen2 = new Pen(Color.Blue, Math.Max(1, Global.ParaShow.ThicknessLine)))
                                using (Font fnt = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold))
                                using (SolidBrush br = new SolidBrush(Global.ParaShow.ColorInfor))
                                {
                                    Draws.DrawInfiniteLine(gc, ll1p1, ll1p2, pen1, 20000f);
                                    Draws.DrawInfiniteLine(gc, ll2p1, ll2p2, pen2, 20000f);
                                    PointF mid = new PointF((ll1p1.X + ll2p1.X) / 2f, (ll1p1.Y + ll2p1.Y) / 2f);
                                    gc.DrawString($"{AngleDetect:F2}mm", fnt, br, mid.X + 6, mid.Y + 4);
                                }
                            }
                            break;

                        case MeasureMethod.MultiPointToLine:
                            if (listLine1Point.Count >= 2)
                            {
                                DrawMultiPointToLineResult(gc);
                            }
                            break;
                    }
                    break;
            }

            return gc;
        }

        private void DrawMultiPointToLineResult(Graphics gc)
        {
            PointF refP1 = listLine1Point[0];
            PointF refP2 = listLine1Point[1];
            var refA = new OpenCvSharp.Point2d(refP1.X, refP1.Y);
            var refB = new OpenCvSharp.Point2d(refP2.X, refP2.Y);
            var srcTool = FindToolByName(Common.TryGetToolList(IndexThread), MultiPointSrcToolName);
            var points = srcTool?.Propety2?.listP_Center;
            double threshold = Common.TryGetTool(IndexThread, Index)?.Score ?? 0;
            int thickness = Math.Max(1, Global.ParaShow.ThicknessLine);

            using (Pen refPen = new Pen(Color.DeepSkyBlue, thickness))
                Draws.DrawInfiniteLine(gc, refP1, refP2, refPen, 20000f);

            if (points == null || points.Count == 0)
                return;

            using (Font fnt = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold))
            using (SolidBrush summaryBrush = new SolidBrush(Global.ParaShow.ColorInfor))
            {if(MultiPointDistances.Count>= points.Count)
                for (int i = 0; i < points.Count; i++)
                {
                    Point srcPoint = points[i];
                    var src = new OpenCvSharp.Point2d(srcPoint.X, srcPoint.Y);
                    OpenCvSharp.Point2d foot;
                    double t;
                    Geometry2D.TryPerpendicularFoot(src, refA, refB, out foot, out t);

                   // double distance = Math.Round(Geometry2D.PerpendicularDistance(src, refA, refB) / Scale, 2);
                    bool isOk = MethordMeasure != MethordMeasure.EachOne || MultiPointOkList[i];
                    Color pointColor = isOk ? Global.ParaShow.ColorInfor : Global.ParaShow.ColorNG;
                    PointF footPoint = new PointF((float)foot.X, (float)foot.Y);

                    using (Pen measurePen = new Pen(isOk ? Global.ParaShow.ColorOK : Global.ParaShow.ColorNG, thickness))
                    using (SolidBrush pointBrush = new SolidBrush(pointColor))
                    {
                        gc.DrawLine(measurePen, srcPoint, footPoint);
                        Draws.Plus(gc, srcPoint.X, srcPoint.Y, 8, pointColor, thickness);
                        Draws.Plus(gc, (int)Math.Round(footPoint.X), (int)Math.Round(footPoint.Y), 5, pointColor, thickness);

                        string text = $"P{i + 1}:{MultiPointDistances[i]:F2} mm";
                        //if (!isOk)
                        //    text += " NG";
                        PointF textPoint = new PointF(
                            (srcPoint.X + footPoint.X) / 2f + 6f,
                            (srcPoint.Y + footPoint.Y) / 2f - fnt.Height / 2f);
                        gc.DrawString(text, fnt, pointBrush, textPoint);
                    }
                }

                gc.DrawString($"{GetMultiPointSummaryLabel()}={AngleDetect:F2}mm", fnt, summaryBrush, refP1.X + 6, refP1.Y + 4);
            }
        }

        private void DrawAngleLinesAndArc(Graphics gc, PointF p1, PointF p2, PointF p3, PointF p4)
        {
            using (Pen line1Pen = new Pen(Color.Red, Math.Max(2, Global.ParaShow.ThicknessLine)))
            using (Pen line2Pen = new Pen(Color.Blue, Math.Max(2, Global.ParaShow.ThicknessLine)))
            {
                Draws.DrawInfiniteLine(gc, p1, p2, line1Pen, 20000f);
                Draws.DrawInfiniteLine(gc, p3, p4, line2Pen, 20000f);
            }

            PointF intersection;
            if (!Cal.FindIntersection(p1, p2, p3, p4, out intersection))
            {
                DrawAngleEndpoints(gc, p1, p2, p3, p4);
                return;
            }

            float radius = GetAngleArcRadius(intersection, p1, p2, p3, p4);
            float angle1 = AngleDeg(intersection, FartherPoint(intersection, p1, p2));
            float angle2 = AngleDeg(intersection, FartherPoint(intersection, p3, p4));
            float sweep = NormalizeSweep(angle2 - angle1);
            if (Math.Abs(sweep) > 90f)
            {
                angle2 = NormalizeAngle(angle2 + 180f);
                sweep = NormalizeSweep(angle2 - angle1);
            }
            if (Math.Abs(sweep) > 90f)
            {
                angle1 = NormalizeAngle(angle1 + 180f);
                sweep = NormalizeSweep(angle2 - angle1);
            }

            RectangleF arcRect = new RectangleF(intersection.X - radius, intersection.Y - radius, radius * 2f, radius * 2f);
            using (SolidBrush pieBrush = new SolidBrush(Color.FromArgb(45, Global.ParaShow.ColorInfor)))
            using (Pen arcPen = new Pen(Global.ParaShow.ColorInfor, Math.Max(2, Global.ParaShow.ThicknessLine)))
            using (Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Global.ParaShow.ColorInfor))
            {
                gc.FillPie(pieBrush, arcRect.X, arcRect.Y, arcRect.Width, arcRect.Height, angle1, sweep);
                gc.DrawArc(arcPen, arcRect, angle1, sweep);
                Draws.Plus(gc, (int)Math.Round(intersection.X), (int)Math.Round(intersection.Y), 8, Color.Yellow, Math.Max(1, Global.ParaShow.ThicknessLine));

                float midAngle = (float)((angle1 + sweep / 2f) * Math.PI / 180.0);
                PointF textPoint = new PointF(
                    intersection.X + (float)Math.Cos(midAngle) * (radius + 12f),
                    intersection.Y + (float)Math.Sin(midAngle) * (radius + 12f));
                gc.DrawString($"{AngleDetect:F1}°", font, textBrush, textPoint);
            }
        }

        private static void DrawAngleEndpoints(Graphics gc, PointF p1, PointF p2, PointF p3, PointF p4)
        {
            int r = 5;
            using (SolidBrush redBrush = new SolidBrush(Color.Red))
            using (SolidBrush blueBrush = new SolidBrush(Color.Blue))
            using (Pen yellowPen = new Pen(Color.Yellow, 1))
            {
                gc.FillEllipse(redBrush, p1.X - r, p1.Y - r, r * 2, r * 2);
                gc.DrawEllipse(yellowPen, p1.X - r, p1.Y - r, r * 2, r * 2);
                gc.FillEllipse(redBrush, p2.X - r, p2.Y - r, r * 2, r * 2);
                gc.DrawEllipse(yellowPen, p2.X - r, p2.Y - r, r * 2, r * 2);
                gc.FillEllipse(blueBrush, p3.X - r, p3.Y - r, r * 2, r * 2);
                gc.DrawEllipse(yellowPen, p3.X - r, p3.Y - r, r * 2, r * 2);
                gc.FillEllipse(blueBrush, p4.X - r, p4.Y - r, r * 2, r * 2);
                gc.DrawEllipse(yellowPen, p4.X - r, p4.Y - r, r * 2, r * 2);
            }
        }

        private static float GetAngleArcRadius(PointF intersection, PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float minLen = Math.Min(
                Math.Min(DistancePoint(intersection, p1), DistancePoint(intersection, p2)),
                Math.Min(DistancePoint(intersection, p3), DistancePoint(intersection, p4)));
            if (minLen <= 1f || float.IsNaN(minLen) || float.IsInfinity(minLen))
                return 45f;
            return Math.Max(28f, Math.Min(90f, minLen * 0.45f));
        }

        private static PointF FartherPoint(PointF origin, PointF a, PointF b)
        {
            return DistancePoint(origin, a) >= DistancePoint(origin, b) ? a : b;
        }

        private static float DistancePoint(PointF a, PointF b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private static float AngleDeg(PointF origin, PointF point)
        {
            return NormalizeAngle((float)(Math.Atan2(point.Y - origin.Y, point.X - origin.X) * 180.0 / Math.PI));
        }

        private static float NormalizeAngle(float angle)
        {
            while (angle < 0f) angle += 360f;
            while (angle >= 360f) angle -= 360f;
            return angle;
        }

        private static float NormalizeSweep(float sweep)
        {
            while (sweep <= -180f) sweep += 360f;
            while (sweep > 180f) sweep -= 360f;
            return sweep;
        }

    }
}
