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
using System.Web.Caching;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class EdgePixel
    {
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
        public int IndexCCD = 0;
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
        public void Default()
        {
           
          
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
        public EdgePixel()
        {

        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }



        public int PxResult;
        sealed class FillCache
        {
            public Mat Mask;      // (h+2, w+2), CV_8UC1
            public Mat Inv;       // scratch CV_8UC1
            public Mat Tmp;       // scratch CV_8UC1
            public Mat KernClose; // kernel close
            public Mat KernOpen;  // kernel open
        }

         void EnsureCache(FillCache cache, Size sz, int kClose, int kOpen)
        {
            if (cache.Mask == null || cache.Mask.Rows != sz.Height + 2 || cache.Mask.Cols != sz.Width + 2)
                cache.Mask = new Mat(new Size(sz.Width + 2, sz.Height + 2), MatType.CV_8UC1, Scalar.Black);

            if (cache.Inv == null || cache.Inv.Rows != sz.Height || cache.Inv.Cols != sz.Width)
                cache.Inv = new Mat(sz, MatType.CV_8UC1);

            if (cache.Tmp == null || cache.Tmp.Rows != sz.Height || cache.Tmp.Cols != sz.Width)
                cache.Tmp = new Mat(sz, MatType.CV_8UC1);

            if (cache.KernClose == null || cache.KernClose.Rows != kClose || cache.KernClose.Cols != kClose)
                cache.KernClose = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(kClose, kClose));

            if (cache.KernOpen == null || cache.KernOpen.Rows != kOpen || cache.KernOpen.Cols != kOpen)
                cache.KernOpen = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(kOpen, kOpen));
        }

         void ForceBinary(Mat src)
        {
            Cv2.Compare(src, new Scalar(0), src, OpenCvSharp.CmpType.GT);
            // Biến mọi >0 thành 255 (in-place), kết quả 0/255 CV_8UC1
            //Cv2.Compare(src, 0, src, CmpTypes.GT);
        }

        static Mat FillHoles(Mat binary)
        {
            //Mat bin = EnsureBinaryU8(binary);

            // Tạo mask cho FloodFill: kích thước (w+2,h+2)
            Mat mask = new Mat(new Size(binary.Cols + 2, binary.Rows + 2), MatType.CV_8UC1, Scalar.Black);

            // Invert để flood từ nền đen hay nền trắng đều ổn
            Mat inv = new Mat();
            Cv2.BitwiseNot(binary, inv);

            // FloodFill từ biên (0,0) – giả định nền chạm biên
            Mat flood = inv.Clone();
            Cv2.FloodFill(flood, mask, new Point(0, 0), new Scalar(255));

            // Vùng lỗ = phần chưa được flood ở inv -> invert lại để lấy “lỗ”
            Mat floodInv = new Mat();
            Cv2.BitwiseNot(flood, floodInv);

            // Kết hợp lỗ đã tô với ảnh gốc để ra ảnh fill-holes
            Mat filled = new Mat();
            Cv2.BitwiseOr(binary, floodInv, filled);

            return filled;
        }
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
                    // matProcess đã tạo từ các bước Filters.*
                //    ForceBinary(matProcess);

                   //  FillCache _cache = new FillCache();
                    FillHoles(matProcess);

                    // PxResult= (int)(Cv2.CountNonZero(matProcess) / 100.0);
                    PxResult = (int)(Cv2.CountNonZero(matProcess) / 100.0);
                }
                
            }

            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Edge", ex.Message));

            }

        
        }
        public void Complete()
            {

            Common.PropetyTools[IndexThread][Index].ScoreResult = (float)(PxResult );
         
            if (Common.PropetyTools[IndexThread][Index].ScoreResult < 0)
                Common.PropetyTools[IndexThread][Index].ScoreResult = 0;
            Common.PropetyTools[IndexThread][Index].ScoreResult = (float)Math.Round(Common.PropetyTools[IndexThread][Index].ScoreResult);
            if (Common.PropetyTools[IndexThread][Index].ScoreResult <= Common.PropetyTools[IndexThread][Index].Score)
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            else
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;

        }
        public async Task SendResult()
        {
            if (Common.PropetyTools[IndexThread][Index].IsSendResult)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                   // await Global.Comunication.Protocol.WriteResultFloat(Common.PropetyTools[IndexThread][Index].AddPLC, WidthResult);
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

            if (Common.PropetyTools[IndexThread][Index].Results == Results.NG)
            {
                cl = Global.ParaShow.ColorNG;
            }
            else
            {
                cl =  Global.ParaShow.ColorOK;
            }
            String nameTool = (int)(Index + 1) + "." + Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            
            Draws.Box2Label(gc, rotA, nameTool,PxResult+ " Px", font, cl, brushText,16, Global.ParaShow.ThicknessLine);


            if (!Global.IsRun || Global.ParaShow.IsShowDetail)
                if (matProcess != null && !matProcess.Empty())
                Draws.DrawMatInRectRotate(gc, matProcess, rotA, Global.ScaleZoom * 100, Global.pScroll, cl, Global.ParaShow.Opacity / 100.0f);

            return gc;
        }


        public void SetModel()
        {
          
            rotCrop = null;
            rotMask = null;
           // if (rotCrop == null) rotCrop = new RectRotate();
            if (rotArea == null) rotArea = new RectRotate();
            Common.PropetyTools[IndexThread][Index].StepValue =1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
          
            Common.PropetyTools[IndexThread][Index].MaxValue = 100000;
            Common.PropetyTools[IndexThread][Index]. StatusTool = StatusTool.WaitCheck;
        }
      
        public int IndexThread = 0;
  
    }
}
