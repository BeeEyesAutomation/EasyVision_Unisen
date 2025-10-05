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
using static BeeCore.Algorithm.FilletCornerMeasure;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class MeasureCorner
    {
      
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsIni = false;
        public int Index = -1;
        [NonSerialized]
        public FilletCornerMeasure.Result Result = new FilletCornerMeasure.Result();
        [NonSerialized]
        public FilletCornerMeasure FilletCornerMeasure = new FilletCornerMeasure();
        [NonSerialized]
        public Mat matProcess = new Mat();
        public RectRotate rotArea, rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<RectRotate> rectRotates = new List<RectRotate>();
        public TypeCrop TypeCrop;


        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public int ThresholdBinary;
        public int MaxLineCandidates = 4;
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
        public LinePairStrategy LinePairStrategy;
        public double PerpAngleToleranceDeg = 1;
        public MeasureCorner()
        {

        }
        public void Default()
        {
            MaxLineCandidates = 4;
            RansacIterations = 200;
            RansacThreshold = 2;
            ThresholdBinary = 150;
            MethordEdge = MethordEdge.StrongEdges;
            SizeClearsmall = 100;
            SizeClearBig = 1000;
            SizeClose = 3;
            SizeOpen = 3;
            PerpAngleToleranceDeg = 15;
            IsClearNoiseBig = false;
            IsClearNoiseSmall = false;
            IsClose = false;
            IsOpen = false;
        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
       
        
   

       
       
        
     
     
        
       
        public void DoWork( RectRotate rectRotate)
        {
            try
            {

               
                using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
                {
                    if (raw.Empty()) return;
                  
                    Mat matCrop = Common.CropRotatedRect(raw, rotArea, rotMask);
                    if (matProcess == null) matProcess = new Mat();
                    if (!matProcess.IsDisposed)
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

                    //  SizeMorphology += 2;
                    FilletCornerMeasure.MaxLineCandidates = MaxLineCandidates;
                    FilletCornerMeasure.RansacThreshold = RansacThreshold;
                    FilletCornerMeasure.RansacIterations = RansacIterations;
                    FilletCornerMeasure.PairStrategy = LinePairStrategy;
                    FilletCornerMeasure.PerpAngleToleranceDeg = PerpAngleToleranceDeg;
                    Result = FilletCornerMeasure.Measure(matCrop, matProcess);//, matProcess, MaximumLine, GapExtremum, LineOrientation, SegmentStatType, MinInliers);
                    Result.AO = Result.AO / Scale;
                    Result.BO = Result.BO / Scale;
                    Result.AI = Result.AI / Scale;
                    Result.BI = Result.BI / Scale;
                }
            }

            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Width", ex.Message));

            }


        }
        public void Complete()
        {

         
            Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            Common.PropetyTools[IndexThread][Index].ScoreResult= (float)Math.Round( ((Math.Abs(Result.AO) + Math.Abs(Result.BO) )/ 2),1);
            if(Math.Abs(Result.AO)>Common.PropetyTools[IndexThread][Index].Score)
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }
            if (Math.Abs(Result.BO) > Common.PropetyTools[IndexThread][Index].Score)
            {
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            }
            //if (GapResult.line2Ds ==null)
            //{
            //    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            //}
            //else if (Common.PropetyTools[IndexThread][Index].ScoreResult <= Common.PropetyTools[IndexThread][Index].Score)
            //{
            //    Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            //    if (!Global.IsRun)
            //    {


            //        WidthTemp = WidthResult;
            //    }
            //}
            //else
            //{
            //    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
            //}    
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
                    //await Global.ParaCommon.Comunication.Protocol.WriteResultFloat(Common.PropetyTools[IndexThread][Index].AddPLC, WidthResult);
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
                if(matProcess!=null)
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
          // if (Result == null) return gc;

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
            //if (!Global.IsRun)
            //    foreach (var l in GapResult.line2Ds)
            //        Draws.DrawInfiniteLine(gc, l, new Pen(Color.Gray, 2));
          
          
            Draws.DrawInfiniteLine(gc, FilletCornerMeasure. ToLine2D (Result.LineH), new Pen(Brushes.Blue, 2));
            Draws.DrawInfiniteLine(gc, FilletCornerMeasure.ToLine2D(Result.LineV), new Pen(Brushes.Blue, 2));
         
            //Draws.DrawTicks(gc, p1, LineOrientation, pen);
            //Draws.DrawTicks(gc, p2, LineOrientation, pen);
            gc.DrawEllipse(new Pen(Brushes.Blue, 2), new RectangleF((float)Result.A.X, (float)Result.A.Y,3,3));
            gc.DrawEllipse(new Pen(Brushes.Blue, 2), new RectangleF((float)Result.B.X, (float)Result.B.Y, 3, 3));
            gc.DrawEllipse(new Pen(Brushes.Red, 2), new RectangleF((float)Result.Touch.X, (float)Result.Touch.Y, 3, 3));
            gc.DrawEllipse(new Pen(Brushes.Green, 2), new RectangleF((float)Result.Corner.X, (float)Result.Corner.Y, 3, 3));
            gc.DrawLine(new Pen(cl,6),new PointF( Result.A.X,Result.A.Y), new PointF(Result.Corner.X, Result.Corner.Y));
            gc.DrawLine(new Pen(cl, 6), new PointF(Result.B.X, Result.B.Y), new PointF(Result.Corner.X, Result.Corner.Y));
            gc.DrawString($"Y={Result.AO:0.##}", new Font("Arial", 28), Brushes.Blue, new PointF(Result.Corner.X, Result.Corner.Y));
            gc.DrawString($"X={Result.BO:0.##}", new Font("Arial", 28), Brushes.Blue, new PointF(Result.Corner.X, Result.Corner.Y+50));
            gc.DrawString($"Angle={Result.AngleAOB:0.##}°", new Font("Arial", 28), Brushes.Blue, new PointF(Result.Corner.X, Result.Corner.Y + 100));


        
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
            FilletCornerMeasure = new FilletCornerMeasure();
            Common.PropetyTools[IndexThread][Index].StepValue = 0.1f;
     
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
          
            Common.PropetyTools[IndexThread][Index].MaxValue = 2;
            Common.PropetyTools[IndexThread][Index]. StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
