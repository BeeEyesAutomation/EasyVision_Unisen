using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using CvPlus;
using Newtonsoft.Json.Linq;
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
    public class VisualMatch
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsBinary = false;
        public int Border = 0;
        public int IndexCCD = 0;
        public bool IsIni = false;
        public int Index = -1;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap bmRaw;
        public ModeCalibVisualMatch ModeCalibVisualMatch = ModeCalibVisualMatch.Normal;

        public  IntPtr intptrTemp ;
        public TypeCrop TypeCrop;
        public string pathRaw = "";
        public double cycleTime = 0;
        public RectangleF rectArea;
        public Compares Compare = Compares.Equal;
        public int LimitCounter = 0;
        public int SzClearNoise = 1;
        public async Task SendResult()
        {
        }

        [NonSerialized]
        public ColorPixel ColorPixel = new ColorPixel();
        public VisualMatch()
        {
            
        }
        public float Aspect = 0.1f;
        public int ColorTolerance = 1;
        public int MaxDiffPixels = 1;
        public TypeMat TypeImg = TypeMat.Color;
        public bool IsAutoThreshBinary = false;
        [NonSerialized]
        Mat matTemp;
        public Mat LearnPattern(Mat raw, bool IsNoCrop)
        {

            using (Mat img = raw.Clone())
            {
              
                Mat matFilter = new Mat() ;
                matFilter = img.Clone();
                switch (TypeImg)
                {
                    case TypeMat.Color:
                        if (matFilter.Channels() == 1)
                            Cv2.CvtColor(matFilter, matFilter, ColorConversionCodes.GRAY2BGR);
                        else if (matFilter.Channels() == 4)
                            Cv2.CvtColor(matFilter, matFilter, ColorConversionCodes.BGRA2BGR);
                        break;
                    case TypeMat.Binary:
                        if (img.Channels() == 3)
                            Cv2.CvtColor(matFilter, matFilter, ColorConversionCodes.BGR2GRAY);
                        if (IsAutoThreshBinary)
                        {
                            matFilter = Filters.BinaryTextAuto(img, 31, 8, SzClearNoise);
                        }
                        else
                            Cv2.Threshold(matFilter, matFilter, ThreshBinary, 255, ThresholdTypes.Binary);
                        break;
                }
               

                int w = 0, h = 0, s = 0, c = 0;
                IntPtr intpr = IntPtr.Zero;
                Mat mat = new Mat();

                try
                {
                    var rrCli = Converts.ToCli(rotArea); // như ở reply trước
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
                    if(ColorPixel==null)
                        ColorPixel = new ColorPixel();
                    intpr = ColorPixel.SetImgeSample(
                             matFilter.Data, matFilter.Width, matFilter.Height, (int)matFilter.Step(), matFilter.Channels()
                        , rrCli, rrMaskCli,IsNoCrop,0,
                            out w, out h, out s, out c);

                 
               
                    if (intpr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                        return mat; // trả Mat rỗng

                
                    MatType mt = c == 1 ? MatType.CV_8UC1
                                : c == 3 ? MatType.CV_8UC3
                                : MatType.CV_8UC4;

                    // Wrap con trỏ rồi copy/clone để sở hữu bộ nhớ managed
                    using (var m = new Mat(h, w, mt, intpr, s))
                    {
                        
                        mat = m.Clone();
                    }

                    // Giữ sống input đến sau khi native xong
                    GC.KeepAlive(img);
                }
                finally
                {
                    if(!matFilter.IsDisposed)
                        matFilter.Dispose();
                    if (intpr != IntPtr.Zero)
                        ColorPixel.FreeBuffer(intpr); // rất quan trọng
                }

                return mat;
            }





        }





        public void SetModel()
        {
            ColorPixel = new ColorPixel();
            rotCrop = null;
           
            if (rotArea == null)
                rotArea = new RectRotate();
            if(bmRaw != null)
            {
                matTemp = bmRaw.ToMat();
              
                LearnPattern(matTemp, true);
            }
           
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
			Common.PropetyTools[IndexThread][Index].MinValue = 0;

            Common.PropetyTools[IndexThread][Index].MaxValue = 5000;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        public List<RectRotate> rectRotates = new List<RectRotate>();
        public bool IsLimitCouter = true;
        public float PxTemp = 0;
        [NonSerialized]
        public float pxRS;
        public int ThreshBinary=100;
        float OffsetX, OffsetY, OffsetAngle;
        bool IsAlign = false;
        public bool IsMultiCPU = false;
        [NonSerialized]
        public bool IsNew = false;
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {

           
                pxRS=0;
                using (Mat img = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
                {
                   
                    if (img.Empty()) return;
                    Mat matFilter = img.Clone();
                try
                {
                    switch (TypeImg)
                    {
                        case TypeMat.Color:
                            if (matFilter.Channels() == 1)
                                Cv2.CvtColor(matFilter, matFilter, ColorConversionCodes.GRAY2BGR);
                            else if (matFilter.Channels() == 4)
                                Cv2.CvtColor(matFilter, matFilter, ColorConversionCodes.BGRA2BGR);
                            break;
                        case TypeMat.Binary:
                            if (img.Channels() == 3)
                                Cv2.CvtColor(matFilter, matFilter, ColorConversionCodes.BGR2GRAY);
                            if (IsAutoThreshBinary)
                            {
                                matFilter = Filters.BinaryTextAuto(img, 31, 8, SzClearNoise);
                            }
                            else
                                Cv2.Threshold(matFilter, matFilter, ThreshBinary, 255, ThresholdTypes.Binary);
                            break;
                    }
                    var rrCli = Converts.ToCli(rotArea); // như ở reply trước
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
                    if (ColorPixel == null)
                        ColorPixel = new ColorPixel();
                    ColorPixel.SetImgeRaw(matFilter.Data, matFilter.Width, matFilter.Height, (int)matFilter.Step(), matFilter.Channels(), rrCli,rrMaskCli,0);

                  
                    int w = 0, h = 0, s = 0, c = 0;
                    IsAlign = ModeCalibVisualMatch == ModeCalibVisualMatch.OFF ? false : true;
                    IntPtr intpr = IntPtr.Zero;
                    if (IsBinary)
                         intpr = ColorPixel.CheckImageFromMat(IsAlign,(int) ModeCalibVisualMatch, IsMultiCPU, 200, SzClearNoise,Aspect, out pxRS,ref OffsetX, ref OffsetY, ref OffsetAngle, out w, out h, out s, out c);
                   else
                        intpr = ColorPixel.CheckImageFromMat(IsAlign, (int)ModeCalibVisualMatch, IsMultiCPU, ThreshBinary, SzClearNoise, Aspect, out pxRS, ref OffsetX, ref OffsetY, ref OffsetAngle, out w, out h, out s, out c);

                    matProcess = new Mat();

                    if (intpr != IntPtr.Zero)
                    {
                        using (Mat mat = new Mat(h, w, OpenCvSharp.MatType.CV_8UC3, intpr, s))
                            mat.CopyTo(matProcess);

                    }



                }
                catch (Exception ex)
                {

                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "VisalMatch", ex.ToString()));
                }
                finally
                {
                    if (!matFilter.IsDisposed)
                        matFilter.Dispose();
                   
                }

            }
            
        }
        public bool IsCalib;
        public void Complete()
        {
            try
            {
                if (IsCalib)
                    PxTemp = pxRS;
                Common.PropetyTools[IndexThread][Index].ScoreResult = pxRS;
                if (Common.PropetyTools[IndexThread][Index].ScoreResult < 0)
                    Common.PropetyTools[IndexThread][Index].ScoreResult = 0;
                if (Common.PropetyTools[IndexThread][Index].ScoreResult> Common.PropetyTools[IndexThread][Index].Score) 
                    Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                else
                    Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            }
            catch(Exception ex)
            {
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

            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;

            if (Common.PropetyTools[Global.IndexProgChoose][Index].Results == Results.NG)
                cl = Color.Red;
            else
                cl = Color.LimeGreen;
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (Global.ParaShow.IsShowBox)
                Draws.Box2Label(gc, rotA, nameTool, pxRS + " Px", font, cl, brushText, Global.ParaShow.Opacity, Global.ParaShow.ThicknessLine);
            //Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl, Global.ParaShow.ThicknessLine);

            gc.ResetTransform();
          
            {if(matProcess!=null)
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
                        mat.Translate(rotA._PosCenter.X+OffsetX, rotA._PosCenter.Y+OffsetY);
                        mat.Rotate(rotA._rectRotation+OffsetAngle);

                        gc.Transform = mat;
                        Bitmap myBitmap = matProcess.ToBitmap();
                        myBitmap.MakeTransparent(Color.Black);
                        myBitmap = General.ChangeToColor(myBitmap, Color.Red, 0.3f);
                        gc.DrawImage(myBitmap, rotA._rect);
                    }
            }
            gc.ResetTransform();
           
          


            return gc;
        }
        [NonSerialized]
        Mat matProcess = new Mat();
        public int IndexThread;
        public bool ISOK;
       

    }
}
