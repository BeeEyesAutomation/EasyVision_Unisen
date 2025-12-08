using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using BeeCpp;
using CvPlus;
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
        public bool IsIni = false;
        public int Index = -1;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
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
   
       public int ColorTolerance = 1;
        public int MaxDiffPixels = 1;

        [NonSerialized]
        Mat matTemp;
        public Mat LearnPattern(Mat raw, bool IsNoCrop)
        {

            using (Mat img = raw.Clone())
            {
                //if (img.Channels() == 3)
                //    Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
                //Cv2.Threshold(img, img, ThreshBinary, 255, ThresholdTypes.Binary);
                // Chuẩn hóa kênh về BGR 3 kênh
                if (img.Channels() == 1)
                    Cv2.CvtColor(img, img, ColorConversionCodes.GRAY2BGR);
                else if (img.Channels() == 4)
                    Cv2.CvtColor(img, img, ColorConversionCodes.BGRA2BGR);

                int w = 0, h = 0, s = 0, c = 0;
                IntPtr intpr = IntPtr.Zero;
                Mat mat = new Mat();

                try
                {
                    var rrCli = Converts.ToCli(rotArea); // như ở reply trước
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                    intpr = ColorPixel.SetImgeSample(
                             img.Data, img.Width, img.Height, (int)img.Step(), img.Channels()
                        , rrCli, rrMaskCli,IsNoCrop,
                            out w, out h, out s, out c);

                    // Gọi native – chú ý truyền đúng kích thước crop
               
                    if (intpr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                        return mat; // trả Mat rỗng

                    // Map kênh trả về
                    MatType mt = c == 1 ? MatType.CV_8UC1
                                : c == 3 ? MatType.CV_8UC3
                                : MatType.CV_8UC4;

                    // Wrap con trỏ rồi copy/clone để sở hữu bộ nhớ managed
                    using (var m = new Mat(h, w, mt, intpr, s))
                    {
                        // CopyTo hoặc Clone đều OK; Clone gọn hơn:
                        mat = m.Clone();
                    }

                    // Giữ sống input đến sau khi native xong
                    GC.KeepAlive(img);
                }
                finally
                {
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
            rotMask = null;
            if (rotArea == null)
                rotArea = new RectRotate();
            if(bmRaw != null)
            {
                matTemp = bmRaw.ToMat();
              
                LearnPattern(matTemp, true);
            }
           
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
			Common.PropetyTools[IndexThread][Index].MinValue = 0;

            Common.PropetyTools[IndexThread][Index].MaxValue = 10000;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }

        //IsProcess,Convert.ToBoolean((int) TypeMode)
        public List<RectRotate> rectRotates = new List<RectRotate>();
    

        public bool IsLimitCouter = true;
        public float PxTemp = 0;
        [NonSerialized]
        public float pxRS;
        public int ThreshBinary=100;
        float OffsetX, OffsetY, OffsetAngle;
        public void DoWork(RectRotate rectRotate)
        {

            try
            {
                pxRS=0;
                using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
                {
                    if (raw.Empty()) return;
                    //if (raw.Channels() == 3)
                    //    Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);
                    //Cv2.Threshold(raw, raw, ThreshBinary, 255, ThresholdTypes.Binary);
                  
                    if (raw.Type() == MatType.CV_8UC1)
                    {
                        Cv2.CvtColor(raw, raw, ColorConversionCodes.GRAY2BGR);
                    }
                    var rrCli = Converts.ToCli(rectRotate); // như ở reply trước
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                    ColorPixel.SetImgeRaw(raw.Data, raw.Width, raw.Height, (int)raw.Step(), raw.Channels(), rrCli,rrMaskCli);

                    if (raw.Empty()) return;

                   
                    int w = 0, h = 0, s = 0, c = 0;
                    IntPtr intpr = ColorPixel.CheckImageFromMat(ColorTolerance,SzClearNoise, out pxRS,ref OffsetX, ref OffsetY, ref OffsetAngle, out w, out h, out s, out c);
                    matProcess = new Mat();

                    if (intpr != IntPtr.Zero)
                    {
                        using (Mat mat = new Mat(h, w, OpenCvSharp.MatType.CV_8UC3, intpr, s))
                            mat.CopyTo(matProcess);

                    }





                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

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

            if (Common.PropetyTools[Global.IndexChoose][Index].Results == Results.NG)
                cl = Color.Red;
            else
                cl = Color.LimeGreen;
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl, Global.Config.ThicknessLine);

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
