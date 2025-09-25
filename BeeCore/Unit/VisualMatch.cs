using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using ColorPixels;
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
        public Bitmap bmTemp;
        public List<Point> Postion=new List<Point>();
        private Mode _TypeMode=Mode.Pattern;
        public List<double> listScore = new List<double>();
        [NonSerialized]
        public Mat matTemp;
        public  IntPtr intptrTemp ;
     

        public TypeCrop TypeCrop;
        public string pathRaw = "";
        public double cycleTime = 0;
        public RectangleF rectArea;
        public Compares Compare = Compares.Equal;
        public int LimitCounter = 0;
        public async Task SendResult()
        {
        }
       
        public List< System.Drawing.Point > listP_Center=new List<System.Drawing.Point>();
        [NonSerialized]
        public ColorPixel ColorPixel = new ColorPixel();
        public VisualMatch()
        {
            ColorPixel = new ColorPixel();
        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern void SetDst(int ixThread, int indexTool, IntPtr data, int image_rows, int image_cols, MatType matType);
        public int ColorTolerance = 1;
        public int MaxDiffPixels = 1;
        public void LearnPattern(   Mat temp)
        {
            if (temp == null)
                if (temp.Empty())
                    return;
            matTemp = temp.Clone();
        
            ColorPixel.SetImgeTemple(matTemp.Data, matTemp.Width, matTemp.Height, (int)matTemp.Step(), matTemp.Channels());
            //    Pattern.CreateTemp(Index, IndexThread);
            //    matTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(temp.Clone());

            //    SetDst(IndexThread, Index, temp.Data, temp.Rows, temp.Cols, temp.Type());

            ////   Pattern.LearnPattern( minArea, Index, IndexThread);

        }
    

      
        public Mat GetTemp(RectRotate rotCrop, RectRotate rotMask, Mat matRaw, Mat bmMask)
        {
           
            Mat matClear = new Mat();
            Mat matTemp = new Mat();
            if (rotCrop._rectRotation < 0) rotCrop._rectRotation = 360 + rotCrop._rectRotation;
            if(rotMask!=null)
            if (rotMask._rectRotation < 0) rotMask._rectRotation = 360 + rotMask._rectRotation;
            Mat matCrop = Common.CropRotatedRect(matRaw, rotCrop, null);
           
            Mat matOut = new Mat();
            Mat crop=new Mat();
            Mat matMask1 = new Mat();
            matTemp = matCrop.Clone();

            bmTemp = matTemp.ToBitmap();
            return matTemp;
        }

       
      
        public void SetModel()
        {
            ColorPixel = new ColorPixel();
            rotCrop = null;
            rotMask = null;
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
			Common.PropetyTools[IndexThread][Index].MinValue = 0;

            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }

        //IsProcess,Convert.ToBoolean((int) TypeMode)
        public List<RectRotate> rectRotates = new List<RectRotate>();
    

        public bool IsLimitCouter = true;
        public void DoWork(RectRotate rectRotate)
        {
           
          
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {

                if (raw.Empty()) return;

                Mat matCrop = Common.CropRotatedRect(raw, rectRotate);

                if (!matCrop.IsContinuous())
                {
                    matCrop = matCrop.Clone();
                }
              
                int w = 0, h = 0, s = 0, c = 0;
                IntPtr intpr = ColorPixel.CheckImageFromMat(matCrop.Data, matCrop.Width, matCrop.Height, (int)matCrop.Step(), matCrop.Channels(),
                   MaxDiffPixels, ColorTolerance, out ISOK, out cycleTime, out w, out h, out s, out c);
                matProcess = new Mat();
              
                if (intpr != IntPtr.Zero)
                {
                
                    matProcess = new Mat(h, w, OpenCvSharp.MatType.CV_8UC3, intpr, s);// new OpenCvSharp.Mat(h, w, type, p, s))
                  
                }
       


            

            }
           
        }
        public void Complete()
        {
           
          if(ISOK) Common.PropetyTools[IndexThread][Index].Results = Results.OK;
          else
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
      
         
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
            {
                cl = Color.Red;
                //if (BeeCore.Common.PropetyTools[IndexThread][Index].UsedTool == UsedTool.Invertse &&
                //    G.Config.ConditionOK == ConditionOK.Logic)
                //    cl = Color.LimeGreen;


            }
            else
            {
                cl = Color.LimeGreen;
                //if (BeeCore.Common.PropetyTools[IndexThread][Index].UsedTool == UsedTool.Invertse &&
                //    G.Config.ConditionOK == ConditionOK.Logic)
                //    cl = Color.Red;
            }
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            if (!Global.IsHideTool)
                Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl, 2);
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
            if (listScore == null) return gc;
            if (rectRotates.Count > 0)
            {
                int i = 1;
                foreach (RectRotate rot in rectRotates)
                {
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
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                    //mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    //mat.Rotate(rot._rectRotation);
                    //gc.Transform = mat;
                    Draws.Plus(gc, 0, 0, (int)rot._rect.Width / 2, cl, 2);
                    Draws.Box2Label(gc, rot._rect, i + "", Math.Round(listScore[i - 1], 1) + "%", Global.fontRS, cl, brushText, 16, 2);

                    gc.ResetTransform();
                    i++;
                }
            }



            return gc;
        }
        [NonSerialized]
        Mat matProcess = new Mat();
        public int IndexThread;
        public bool ISOK;
       

    }
}
