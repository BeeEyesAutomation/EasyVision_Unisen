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
    public class Edge
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
        public Line2DCli Line2DCli = new Line2DCli();
        [NonSerialized]
        public RansacLine RansacLine;
        [NonSerialized]
        public FilterCLi FilterCLi;
        [NonSerialized]
        public EdgePipelineOptionsCli EdgePipelineOptionsCli;
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
        public Edge()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
        [NonSerialized]
        private CropPlus CropPlus = new CropPlus();
        
   
        
       
        public void DoWork( RectRotate rectRotate)
        {
          
                if (matProcess != null) { matProcess.Dispose(); matProcess = null; }
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (raw.Empty()) return;
              
                Mat matCrop = Cropper.CropRotatedRect(raw, rectRotate, rotMask);
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
               

              
                Line2DCli =  RansacLine.FindBestLine(
                    matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1/Scale

                  
                );
                PointF p1 = new PointF(Line2DCli.X1, Line2DCli.Y1);
                PointF p2 = new PointF(Line2DCli.X2, Line2DCli.Y2);
                System.Drawing.Point pCenter = new System.Drawing.Point((int)rectRotate._PosCenter.X - (int)rectRotate._rect.Width / 2 + (int)p1.X, (int)rectRotate._PosCenter.Y - (int)rectRotate._rect.Height / 2 + (int)p1.Y);
                System.Drawing.Point pCenter2 = new System.Drawing.Point((int)rectRotate._PosCenter.X - (int)rectRotate._rect.Width / 2 + (int)p2.X, (int)rectRotate._PosCenter.Y - (int)rectRotate._rect.Height / 2 + (int)p2.Y);
                listP_Center = new List<System.Drawing.Point>();
                rectRotates = new List<RectRotate>();
                listP_Center.Add(pCenter);
                listP_Center.Add(pCenter2);
                rectRotates.Add(new RectRotate(new RectangleF(pCenter.X, pCenter.Y, pCenter2.X, pCenter2.Y), new PointF(0, 0), 0,0));
            

            }
            //using (Mat src = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            //    {
            //        if (src.Empty()) return;

            //        Mat gray = null;
            //        try
            //        {
            //            if (src.Channels() != 1)
            //            {

            //                if (src.Channels() == 3)
            //                    Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            //                else
            //                    Cv2.CvtColor(src, gray, ColorConversionCodes.BGRA2GRAY);



            //            }
            //            else
            //                gray = src;

            //            var rrCli = Converts.ToCli(rectRotate); // như ở reply trước
            //            RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
            //            int w, h, s, c;
            //            IntPtr ptr = CropPlus.CropRotatedInt(
            //                gray.Data, gray.Width, gray.Height, (int)gray.Step(), gray.Channels(), rrCli, rrMaskCli, out w, out h, out s, out c);
            //            GC.KeepAlive(gray);



            //            try
            //            {
            //                // Validate trước, nhưng KHÔNG return trước khi FreeBuffer
            //                if (ptr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
            //                {
            //                    return; // finally phía dưới vẫn chạy để FreeBuffer nếu cần
            //                }
                         
            //                MatType mt = (c == 1) ? MatType.CV_8UC1
            //                           : (c == 3) ? MatType.CV_8UC3
            //                                      : MatType.CV_8UC4;
            //                EdgePipelineOptionsCli = new EdgePipelineOptionsCli
            //                {
            //                    SizeClose = 0,
            //                    ClearNoiseSmallArea = 0,
            //                    ClearNoiseBigArea = 0,
            //                    SizeOpen = 0,
            //                    ThresholdBinary = 0,
            //                    Method = (MethordEdgeCli)MethordEdge
            //                }; 
            //             var dst = new Mat(h, w, MatType.CV_8UC1);
            //            long sDst = dst.Step();

                     
                   
            //            FilterCLi.RunEdgePipeline(ptr, w, h, mt, (ulong)s, EdgePipelineOptionsCli, dst.Data, dst.Type(), (ulong)sDst);
            //            Line2DCli = RansacLine.FindBestLine(dst.Data, dst.Width, dst.Height,(int) dst.Step(), RansacIterations, (float)RansacThreshold, 1000, 0, Scale);
            //            matProcess = dst.Clone();
            //            //using (var mNative = new Mat(h, w, dstType, intPtrOut, (int)dstStep))
            //            //    {
            //            //        matProcess = mNative.Clone(); // bây giờ dữ liệu đã thuộc về OpenCV (managed)
            //            //    }
            //            }
            //            finally
            //            {
            //                // GIẢI PHÓNG BỘ NHỚ DO native CẤP PHÁT — luôn luôn!
            //                if (ptr != IntPtr.Zero)
            //                {
            //                    CropPlus.FreeBuffer(ptr);
            //                    ptr = IntPtr.Zero;
            //                }
            //            }





            //        }

            //        catch (Exception ex)
            //        {
            //            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Width", ex.Message));

            //        }

            //    }
            
        
        }
        public void Complete()
        {
            //switch (SegmentStatType)
            //{
            //    case SegmentStatType.Average:
            //        WidthResult = (float)GapResult.GapMedium /Scale;
            //        break;
            //    case SegmentStatType.Shortest:
            //        WidthResult = (float)GapResult.GapMin / Scale;
            //        break;
            //    case SegmentStatType.Longest:
            //        WidthResult = (float)GapResult.GapMax / Scale;
            //        break;
            //}
            if(Line2DCli.Found == true)
            WidthResult = Line2DCli.LengthMm;
            if (IsCalibs&& Line2DCli.Found==true)
            {
                MinInliers = Line2DCli.Inliers;
                WidthTemp = WidthResult;
            }
            Common.PropetyTools[IndexThread][Index].ScoreResult= (int)((Math.Abs(WidthResult - WidthTemp) / (WidthTemp * 1.0))*100);
            if (Line2DCli.Found==false)
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
            Pen pen = new Pen(Global.Config.ColorInfor,Global.Config.ThicknessLine);
            String nameTool = (int)(Index + 1) + "." + Common.PropetyTools[Global.IndexChoose][Index].Name;

            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl, Global.Config.ThicknessLine);

            if (!Global.IsRun||Global.Config.IsShowDetail)
            {
                if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rotA, Global.ScaleZoom * 100, Global.pScroll, cl, Global.Config.Opacity / 100.0f);
            }
            gc.ResetTransform();
            if (Line2DCli.Found == false) return gc;

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

            //   Draws.DrawInfiniteLine(gc,new Line2D( Line2DCli.Vx, Line2DCli.Vy, Line2DCli.X1, Line2DCli.Y1), new Pen(cl, 2));
         
            PointF p1 = new PointF(Line2DCli.X1, Line2DCli.Y1);
            PointF p2 = new PointF(Line2DCli.X2, Line2DCli.Y2);
             Draws.DrawTicks(gc, p1,LineOrientation, pen);
            Draws.DrawTicks(gc, p2,LineOrientation, pen);
            gc.DrawLine(pen, p1, p2);
            gc.DrawString($"{Line2DCli.LengthMm:F2}mm", new Font("Arial", Global.Config.FontSize), new SolidBrush(Global.Config.ColorInfor), p1.X + 5, (p1.Y + p2.Y) / 2 + 10);
            // gc.ResetTransform();
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
            if (rotArea == null) rotArea = new RectRotate();
            rotMask = null;
            if (RansacLine == null) RansacLine = new RansacLine();
            if (FilterCLi == null) FilterCLi = new FilterCLi();
            CropPlus=new CropPlus();
            Common.PropetyTools[IndexThread][Index].StepValue = 0.1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
          
            Common.PropetyTools[IndexThread][Index].MaxValue = 20;
            Common.PropetyTools[IndexThread][Index]. StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
