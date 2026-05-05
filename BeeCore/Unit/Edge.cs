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
using System.Windows.Forms.VisualStyles;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class Edge
    {
        [NonSerialized]
        public bool IsNew = false;
        [NonSerialized]
        public Line2D Line;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int MaxThread = 0;
        public int IndexCCD = 0;
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
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
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
        public int AngleRange { set; get; }
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
        public LineDirScan LineDirScan { set; get; }

        public float AspectLen=0.6f;

        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            AspectLen = 0.2f;
            if (matProcess != null) { matProcess.Dispose(); matProcess = null; }
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                if (raw.Empty()) return;
                Mat matMark = new Mat();
                Mat matCrop = Cropper.CropRotatedRectUltraFast(raw, rotArea);
                if (matProcess == null) matProcess = new Mat();
                if (!matProcess.Empty()) matProcess.Dispose();
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.BGR2GRAY);
                if(matCrop.Empty()) 
                    return;
                Cv2.EqualizeHist(matCrop, matProcess);
                switch (MethordEdge)
                { 
                    case MethordEdge.CloseEdges:
                        matProcess = Filters.Edge(matCrop);
                        break;
                    case MethordEdge.StrongEdges:
                        matProcess = Filters.GetStrongEdgesStable(matCrop);
                        break;
                    case MethordEdge.Stable:
                        matProcess = Filters.GetStrongEdgesStable(matCrop);
                        break;
                    case MethordEdge.Binary:
                        matProcess = Filters.Threshold(matCrop, ThresholdBinary, ThresholdTypes.Binary);
                        break;
                    case MethordEdge.InvertBinary:
                        matProcess = Filters.Threshold(matCrop, ThresholdBinary, ThresholdTypes.BinaryInv);
                        break;
                }
               
                if (IsClose)
                    matProcess = Filters.Morphology(matProcess, MorphTypes.Close, new Size(SizeClose, SizeClose));
                if (IsOpen)
                    matProcess = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                if (IsClearNoiseSmall)
                    matProcess = Filters.ClearNoise(matProcess, SizeClearsmall);
                //if (IsClearNoiseBig)
                //    matProcess = Filters.ClearNoiseBig(matProcess, SizeClearBig*100);

             
                Line2DCli =  RansacLine.FindBestLine(
                    matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                    iterations: RansacIterations,
                    threshold: (float)RansacThreshold,
                    maxPoints: 120000,
                    seed: Index,
                    mmPerPixel: 1/Scale, AspectLen, (LineDirectionMode)((int)LineOrientation), (BeeCpp.LineScanMode)((int)LineDirScan),0, AngleRange
                );
            
                PointF p1 = new PointF(Line2DCli.X1, Line2DCli.Y1);
                PointF p2 = new PointF(Line2DCli.X2, Line2DCli.Y2);
                System.Drawing.Point pCenter = new System.Drawing.Point((int)rotArea._PosCenter.X - (int)rotArea._rect.Width / 2 + (int)p1.X, (int)rotArea._PosCenter.Y - (int)rotArea._rect.Height / 2 + (int)p1.Y);
                System.Drawing.Point pCenter2 = new System.Drawing.Point((int)rotArea._PosCenter.X - (int)rotArea._rect.Width / 2 + (int)p2.X, (int)rotArea._PosCenter.Y - (int)rotArea._rect.Height / 2 + (int)p2.Y);
                listP_Center = new List<System.Drawing.Point>();
                rectRotates = new List<RectRotate>();
                listP_Center.Add(pCenter);
                listP_Center.Add(pCenter2);
                rectRotates.Add(new RectRotate(new RectangleF(pCenter.X, pCenter.Y, pCenter2.X, pCenter2.Y), new PointF(0, 0), 0,0));
            

            }
            //using (Mat src = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
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
        public float PerValue;
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
          

                if (IsCalibs && Line2DCli.Found == true)
            {
                MinInliers = Line2DCli.Inliers;
                WidthTemp = WidthResult;
            }
            Common.TryGetTool(IndexThread, Index).ScoreResult = (int)(Math.Abs(WidthResult - WidthTemp));
            if (Line2DCli.Found==false)
            {
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
            }
            else if (Common.TryGetTool(IndexThread, Index).ScoreResult <= Common.TryGetTool(IndexThread, Index).Score)
            {
                Common.TryGetTool(IndexThread, Index).Results = Results.OK;
                //if (!Global.IsRun)
                //{


                //    WidthTemp = WidthResult;
                //}
            }
            else
            {
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
            }
            PerValue = (float)(( Line2DCli.LengthPx / Line2DCli.Inliers)*100.0);
            //if (PerValue > 70)
            //{
            //    Common.TryGetTool(IndexThread, Index).Results = Results.OK;
            //}
            //else
            //{
            //    Common.TryGetTool(IndexThread, Index).Results = Results.NG;
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
            Pen pen = new Pen(Global.ParaShow.ColorInfor,Global.ParaShow.ThicknessLine);
            String nameTool = (int)(Index + 1) + "." + Common.TryGetTool(Global.IndexProgChoose, Index).Name;

            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (Global.ParaShow.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl, Global.ParaShow.ThicknessLine);

            if (!Global.IsRun||Global.ParaShow.IsShowDetail)
            {
                if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rotA, Global.ScaleZoom * 100, Global.pScroll, cl, Global.ParaShow.Opacity / 100.0f);
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
            gc.DrawString($"{Line2DCli.LengthMm:F2}mm +"+PerValue+"%", new Font("Arial", Global.ParaShow.FontSize), new SolidBrush(Global.ParaShow.ColorInfor), p1.X + 5, (p1.Y + p2.Y) / 2 + 10);
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


        public void SetModel( bool IsCoppy=false)
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
            if (RansacLine == null) RansacLine = new RansacLine();
            if (FilterCLi == null) FilterCLi = new FilterCLi();
            CropPlus=new CropPlus();
            Common.TryGetTool(IndexThread, Index).StepValue = 0.1f;
            Common.TryGetTool(IndexThread, Index).MinValue = 0;
          
            Common.TryGetTool(IndexThread, Index).MaxValue = 200;
            Common.TryGetTool(IndexThread, Index). StatusTool = StatusTool.WaitCheck;
        }
        public float Scale = 1;
        public int IndexThread = 0;
  
    }
}
