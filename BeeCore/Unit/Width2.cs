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
    public class Width2
    {
        [NonSerialized]
        public Line2D Line;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsIni = false;
        public int Index = -1;

        [NonSerialized]
        public BeeCpp.GapResultCli GapResultCli;
        [NonSerialized]
        public BeeCpp.WidthCli WidthCli;
       // public GapResult GapResult = new GapResult();
        [NonSerialized]
        public Mat matProcess = new Mat();
        public RectRotate rotArea, rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public TypeCrop TypeCrop;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<RectRotate> rectRotates = new List<RectRotate>();

       
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
        public Width2()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
       
        
   

       
        public void DoWork( RectRotate rectRotate)
        {
            try
            {
               
                WidthResult = 0;
                using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
                {
                    if (raw.Empty()) return;
                    if (IsCalibs | MaxLen == 0)
                    {
                        MinInliers = 2;

                    }
                    Mat matCrop = Common.CropRotatedRect(raw, rectRotate, rotMask);
                    if (matProcess == null) matProcess = new Mat();
                    if (!matProcess.Empty()) matProcess.Dispose();
                    if (matCrop.Type() == MatType.CV_8UC3)
                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGR2GRAY);
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
                    if (IsClearNoiseSmall)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearsmall);
                    if (IsClose)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Close, new Size(SizeClose, SizeClose));
                    if (IsOpen)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                    if (IsClearNoiseBig)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearBig);
                    WidthCli.SetImgeRaw(matProcess.Data,matProcess.Width,matProcess.Height, (int)matProcess.Step(), matProcess.Channels());
                    WidthCli.SetRansac(RansacIterations,RansacThreshold);
                    WidthCli.SetMmPerPixel(1.0/Scale);

                    GapResultCli = new GapResultCli();
                    GapResultCli = WidthCli.MeasureParallelGap(MaximumLine, (GapExtremumCli)(int)GapExtremum, (LineOrientationCli)(int)LineOrientation, (SegmentStatTypeCli)(int)SegmentStatType, MinInliers);
                    if (GapResultCli.LineMid != null)
                        if (GapResultCli.LineMid.Count() > 1)
                        {
                            listP_Center = new List<System.Drawing.Point>();
                            rectRotates = new List<RectRotate>();
                            PointF p1 = new PointF(GapResultCli.LineMid[0].X, GapResultCli.LineMid[0].Y);
                            PointF p2 = new PointF(GapResultCli.LineMid[1].X, GapResultCli.LineMid[1].Y);
                            float Xmin = Math.Min(p1.X, p2.X); float Ymin = Math.Min(p1.Y, p2.Y);
                            PointF pCenter = new PointF(Xmin + Math.Abs(p1.X - p2.X) / 2, Ymin + Math.Abs(p1.Y - p2.Y) / 2);
                            listP_Center.Add(new System.Drawing.Point((int)rectRotate._PosCenter.X - (int)rectRotate._rect.Width / 2 + (int)pCenter.X, (int)rectRotate._PosCenter.Y - (int)rectRotate._rect.Height / 2 + (int)pCenter.Y));
                            rectRotates.Add(new RectRotate());
                        }
                }
            }

            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Width", ex.Message));

            }

        
        }
        public void Complete()
        {
            switch (SegmentStatType)
            {
                case SegmentStatType.Average:
                    WidthResult = (float)GapResultCli.GapMediumMm;
                    break;
                case SegmentStatType.Shortest:
                    WidthResult = (float)GapResultCli.GapMinMm;
                    break;
                case SegmentStatType.Longest:
                    WidthResult = (float)GapResultCli.GapMaxMm;
                    break;
            }
          
            if (IsCalibs)
            {
                MinInliers = (int)(GapResultCli.InlierRemain * (80 / 100.0));
                double Delta =  Common.PropetyTools[IndexThread][Index].Score / 100.0;
                MinLen = (int)((GapResultCli.GapMinMm) * (1 + Delta));
                MaxLen = (int)((GapResultCli.GapMinMm) * (1 + Delta));
                if (!Global.IsRun)
                {


                    WidthTemp = WidthResult;
                }

            }
            Common.PropetyTools[IndexThread][Index].ScoreResult= (int)((Math.Abs(WidthResult - WidthTemp) / (WidthTemp * 1.0))*100);
            if (GapResultCli.Lines ==null)
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }
            else if (Common.PropetyTools[IndexThread][Index].ScoreResult <= Common.PropetyTools[IndexThread][Index].Score)
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
                if (!Global.IsRun)
                {


                    WidthTemp = WidthResult;
                }
            }
            else
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
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
        public async Task SendResult()
        {
            if (Common.PropetyTools[IndexThread][Index].IsSendResult)
            {
                if (Global.ParaCommon.Comunication.Protocol.IsConnected)
                {
                    await Global.ParaCommon.Comunication.Protocol.WriteResultFloat(Common.PropetyTools[IndexThread][Index].AddPLC, WidthResult);
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
            switch(Common.PropetyTools[Global.IndexChoose][Index].Results)
            {
                case Results.OK:
                    cl =  Global.Config.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.Config.ColorNG;
                    break;
            }
            Pen pen = new Pen(Color.Blue, 2);
            String nameTool = (int)(Index + 1) + "." + Common.PropetyTools[Global.IndexChoose][Index].Name;
            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl, Global.pScroll, Global.ScaleZoom * 100, Global.Config.ThicknessLine);

            if (!Global.IsRun)
            {
                if (!matProcess.IsDisposed)
                if (!matProcess.Empty())
                {
                    gc.ResetTransform();
                    mat = new Matrix();
                    if (!Global.IsRun)
                    {
                        mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                        mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                    }
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);
             
                    gc.Transform = mat;
                    Bitmap myBitmap = matProcess.ToBitmap();
                    myBitmap.MakeTransparent(Color.Black);
                    myBitmap = General.ChangeToColor(myBitmap, Color.Red, 0.7f);
                    gc.DrawImage(myBitmap, rotA._rect);
                }
            }
            gc.ResetTransform();
            if (GapResultCli.Lines == null) return gc;

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
            if (!Global.IsRun)
                foreach (var l in GapResultCli.Lines)
                    Draws.DrawInfiniteLine(gc, new Line2D(l.Vx,l.Vy,l.X0,l.Y0), new Pen(Color.Gray, 2));
            Draws.DrawInfiniteLine(gc, new Line2D(GapResultCli.LineA.Vx, GapResultCli.LineA.Vy, GapResultCli.LineA.X0, GapResultCli.LineA.Y0) , new Pen(cl, 2));
            Draws.DrawInfiniteLine(gc, new Line2D(GapResultCli.LineB.Vx, GapResultCli.LineB.Vy, GapResultCli.LineB.X0, GapResultCli.LineB.Y0), new Pen(cl, 2));
            PointF p1 = new PointF(GapResultCli.LineMid[0].X, GapResultCli.LineMid[0].Y);
            PointF p2 = new PointF(GapResultCli.LineMid[1].X, GapResultCli.LineMid[1].Y);
            Draws.DrawTicks(gc, p1,LineOrientation, pen);
            Draws.DrawTicks(gc, p2,LineOrientation, pen);
            gc.DrawLine(new Pen(Color.Blue, 4), p1, p2);          
           gc.DrawString($"{WidthResult:F2}mm", new Font("Arial", 16), Brushes.Blue, p1.X + 5, (p1.Y + p2.Y) / 2 + 10);
            gc.ResetTransform();
            //mat = new Matrix();
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


        public void SetModel()
        {
            rotMask = null;
            WidthCli = new WidthCli();
            Common.PropetyTools[IndexThread][Index].StepValue = 0.1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
          
            Common.PropetyTools[IndexThread][Index].MaxValue = 20;
            Common.PropetyTools[IndexThread][Index]. StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
