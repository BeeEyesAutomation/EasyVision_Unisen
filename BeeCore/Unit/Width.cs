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
    public class Width
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
        public GapResult GapResult = new GapResult();
        [NonSerialized]
        public Mat matProcess = new Mat();
        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public TypeCrop TypeCrop;
        public Compares Compare = Compares.Equal;
        public int ThresholdBinary;
        public Width()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
       
        
   

        [NonSerialized]
        public ParallelGapDetector ParallelGapDetector ;

        public int MaximumLine = 4;
        public GapExtremum GapExtremum = GapExtremum.Middle;
        public LineOrientation LineOrientation = LineOrientation.Vertical;
        public SegmentStatType SegmentStatType = SegmentStatType.Average;
        public int MinInliers = 2;
        public float WidthResult = 0;
        public float WidthTemp = 0;
        public int MinLen = 0, MaxLen = 10000;
        public bool IsCalibs = false;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<RectRotate> rectRotates = new List<RectRotate>();
        public void DoWork( RectRotate rectRotate)
        {
            if (ParallelGapDetector == null) ParallelGapDetector = new ParallelGapDetector();
            WidthResult = 0;
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (raw.Empty()) return;
                if (IsCalibs|MaxLen==0)
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


                GapResult = ParallelGapDetector.MeasureParallelGap(matCrop, matProcess, MaximumLine, GapExtremum, LineOrientation, SegmentStatType, MinInliers);
               if(GapResult.lineMid!=null)
                    if (GapResult.lineMid.Count()>1)
                    {
                        listP_Center = new List<System.Drawing.Point>();
                        rectRotates = new List<RectRotate>();
                        PointF p1 = new PointF(GapResult.lineMid[0].X, GapResult.lineMid[0].Y);
                        PointF p2 = new PointF(GapResult.lineMid[1].X, GapResult.lineMid[1].Y);
                        float Xmin = Math.Min(p1.X, p2.X); float Ymin = Math.Min(p1.Y, p2.Y);
                        PointF pCenter = new PointF(Xmin+Math.Abs(p1.X - p2.X) / 2, Ymin+ Math.Abs(p1.Y - p2.Y) / 2);
                        listP_Center.Add(new System.Drawing.Point((int)rectRotate._PosCenter.X - (int)rectRotate._rect.Width / 2 + (int)pCenter.X, (int)rectRotate._PosCenter.Y - (int)rectRotate._rect.Height / 2 + (int)pCenter.Y));
                        rectRotates.Add(new RectRotate());
                    }

            }
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
                MinInliers = (int)(GapResult.Inlier * ((100-Common.PropetyTools[IndexThread][Index].Score) / 100.0));
                double Delta =  Common.PropetyTools[IndexThread][Index].Score / 100.0;
                MinLen = (int)((GapResult.GapMin/Scale) * (1 + Delta));
                MaxLen = (int)((GapResult.GapMax/Scale )* (1 + Delta));
                if (!Global.IsRun)
                {


                    WidthTemp = WidthResult;
                }

            }
            Common.PropetyTools[IndexThread][Index].ScoreResult= (int)((Math.Abs(WidthResult - WidthTemp) / (WidthTemp * 1.0))*100);
            if (GapResult.line2Ds ==null)
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
                    cl = Global.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.ColorNG;
                    break;
            }
            Pen pen = new Pen(Color.Blue, 2);
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            if (!Global.IsHideTool)
                Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl, 2);
          
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
            if (!Global.IsRun)
                foreach (var l in GapResult.line2Ds)
                    Draws.DrawInfiniteLine(gc, l, new Pen(Color.Gray, 2));
            Draws.DrawInfiniteLine(gc,GapResult.LineA, new Pen(cl, 2));
            Draws.DrawInfiniteLine(gc,GapResult.LineB, new Pen(cl, 2));
            PointF p1 = new PointF(GapResult.lineMid[0].X,GapResult.lineMid[0].Y);
            PointF p2 = new PointF(GapResult.lineMid[1].X,GapResult.lineMid[1].Y);
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
            ParallelGapDetector = new ParallelGapDetector();
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 20;
            Common.PropetyTools[IndexThread][Index]. StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
