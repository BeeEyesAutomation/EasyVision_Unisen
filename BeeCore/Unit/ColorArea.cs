using BeeCore.Algorithm;
using BeeCore.Core;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public  class ColorArea
    {
        public ColorArea ()
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsClose = false;
        public bool IsOpen = false;
        public bool IsClearNoiseBig = false;
        public bool IsClearNoiseSmall = false;
        public int SizeClearsmall = 1;
        public int SizeClearBig = 1;
        public int SizeClose = 1;
        public int SizeOpen = 1;
        public List<HSV> HSVs;
        public List<RGB> RGBs;
        public int Extraction = 0;
        public ColorGp TypeColor;
        public bool IsGetColor;
        public int PxTemp = 0;
      
        public int Index = -1;
        public int IndexThread = 0;
        public TypeCrop TypeCrop;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        

        
      
        [NonSerialized]
        public  BeeCpp.ColorArea ColorAreaPP;






   
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        public List<Color> listCLShow = new List<Color>();
        public Color clShow;
        public void SetModel()
        {

            rotMask = null;
            rotCrop = null;
            ColorAreaPP = new BeeCpp.ColorArea();
            SetColor();
            Common.PropetyTools[IndexThread][Index].StepValue = 1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        [NonSerialized]
        private HSVCli hSV;
        [NonSerialized]
       private RGBCli rGB;
        public System.Drawing.Color GetColor( Mat raw, int x,int y)
        {
            using (Mat mat = raw.Clone())
            {
                if (mat.Empty()) return Color.Empty;
                if (mat.Type() == MatType.CV_8UC1)
                {
                    Cv2.CvtColor(mat, mat, ColorConversionCodes.GRAY2BGR);
                }
                ColorAreaPP.SetImgeRaw(mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels());
                switch (TypeColor)
                {
                    case ColorGp.HSV:
                        hSV=new HSVCli();
                     hSV =    ColorAreaPP.GetHSV( x, y); 
                        if (hSV != null)
                        {
                            clShow = HsvConvert.FromHsvOpenCv((byte)hSV.H, (byte)hSV.S, (byte)hSV.V);

                        }
                      
                        break;
                    case ColorGp.RGB:
                        rGB=new RGBCli();
                        
                       rGB=  ColorAreaPP.GetRGB(x, y);
                        if (rGB != null)
                            clShow = Color.FromArgb(rGB.R, rGB.G, rGB.B);
                        break;
                }
                
              
                
            }
            return clShow;

            
        }
        public void Undo()
        {
            switch (TypeColor)
            {
                case ColorGp.HSV:

                    HSVs.RemoveAt(HSVs.Count-1);

                    break;
                case ColorGp.RGB:

                    RGBs.RemoveAt(RGBs.Count - 1);

                    break;
            }

        }
        public void AddColor()
        {

            switch (TypeColor)
            {
                case ColorGp.HSV:
                    if (HSVs == null)
                        HSVs = new List<HSV>();
                    if(hSV!=null)
                    HSVs.Add(new HSV(hSV.H, hSV.S, hSV.V));
                
                    break;
                case ColorGp.RGB:
                    if (RGBs == null)
                        RGBs = new List<RGB>();
                    if (rGB != null)
                        RGBs.Add(new RGB(rGB.R, rGB.G, rGB.B));
                 
                    break;
            }
            listCLShow.Add(clShow);
        }

      
       
    
        public Mat ClearTemp()
        {
            HSVs = new List<HSV>();
            RGBs = new List<RGB>();
            SetColor();
            return new Mat();

        }
        public void SetColor()
        {
            switch(TypeColor)
            { case ColorGp.HSV:
                    if (HSVs != null)
                    {
                        HSVCli[] arrHSV = new HSVCli[HSVs.Count];
                        int i = 0;
                        foreach (var hSV in HSVs)
                        {
                            arrHSV[i] = new HSVCli();
                            arrHSV[i].H = hSV.H;
                            arrHSV[i].S = hSV.S;
                            arrHSV[i].V = hSV.V;
                            i++;
                        }
                        ColorAreaPP.SetTempHSV(arrHSV, Extraction);
                    }
                    break;
                case ColorGp.RGB:
                    if (RGBs != null)
                    {
                        RGBCli[] arrRGB = new RGBCli[RGBs.Count];
                        int j = 0;
                        foreach (var hSV in RGBs)
                        {
                            arrRGB[j] = new RGBCli();
                            arrRGB[j].R = hSV.R;
                            arrRGB[j].G = hSV.G;
                            arrRGB[j].B = hSV.B;
                            j++;
                        }
                        ColorAreaPP.SetTempRGB(arrRGB, Extraction);
                    }    
                    
                    break;
            }
         
        
        }
     
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]

        unsafe public static extern IntPtr GetImageResult(ref int rows, ref int cols, ref int Type);


        public bool IsCalib;
     public   int pxRS = 0;
        public void DoWork(RectRotate rotCrop)
        {

            pxRS= CheckColor(rotCrop);

        }
        public void Complete()
        {
            if(IsCalib) 
                PxTemp = pxRS;
            Common.PropetyTools[IndexThread][Index].ScoreResult = (float)((pxRS / (PxTemp * 1.0)) * 100);
            if (Common.PropetyTools[IndexThread][Index].ScoreResult > 100)
                Common.PropetyTools[IndexThread][Index].ScoreResult = 100;
            if (Common.PropetyTools[IndexThread][Index].ScoreResult < 0)
                Common.PropetyTools[IndexThread][Index].ScoreResult = 0;
            Common.PropetyTools[IndexThread][Index].ScoreResult = (float)Math.Round(Common.PropetyTools[IndexThread][Index].ScoreResult);
            if (Common.PropetyTools[IndexThread][Index].ScoreResult > Common.PropetyTools[IndexThread][Index].Score)
                Common.PropetyTools[IndexThread][Index].Results = Results.OK;
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

            if (Common.PropetyTools[IndexThread][Index].Results == Results.NG)
            {
                cl = Global.ColorNG;
            }
            else
            {
                cl = Global.ColorOK;
            }
            String nameTool = (int)(Index + 1) + "." + Common.PropetyTools[IndexThread][Index].Name;
            if (!Global.IsHideTool)
                Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl, 1);
            if(matProcess!=null)
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
              
                Bitmap myBitmap = matProcess.ToBitmap(); ;
                myBitmap.MakeTransparent(Color.Black);
                myBitmap = General.ChangeToColor(myBitmap,cl, 0.5f);
                gc.DrawImage(myBitmap, rotA._rect);
            }

            return gc;
        }
        [NonSerialized]
        public Mat matProcess = new Mat();
        [NonSerialized]
        private Native Native = new Native();
        float ValueColor = 0;
        public int CheckColor(RectRotate rotCrop)
        {
            int pxRs = 0;
            using (Mat mat =  BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                if (mat.Empty()) return -1 ;
                if (mat.Type() == MatType.CV_8UC1)
                {
                    Cv2.CvtColor(mat, mat, ColorConversionCodes.GRAY2BGR);
                }
                ColorAreaPP.SetImgeCrop(mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels(), rotCrop._PosCenter.X, rotCrop._PosCenter.Y,
                            rotCrop._rect.Width, rotCrop._rect.Height, 
                            rotCrop._rectRotation);
                int w = 0, h, s = 0, c = 0;
                IntPtr intPtr = IntPtr.Zero;
                intPtr = ColorAreaPP.Check(  out w, out h, out s, out c);
                if (intPtr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                    matProcess = new Mat();
                MatType mt = c == 1 ? MatType.CV_8UC1
                            : c == 3 ? MatType.CV_8UC3
                            : MatType.CV_8UC4;
                using (var m = new Mat(h, w, mt, intPtr, s))
                {
                    matProcess = m.Clone();
                    if (IsClearNoiseSmall)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearsmall);
                    if (IsClose)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Close, new Size(SizeClose, SizeClose));
                    if (IsOpen)
                        matProcess = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                    if (IsClearNoiseBig)
                        matProcess = Filters.ClearNoise(matProcess, SizeClearBig);
                    pxRs=Cv2.CountNonZero(matProcess);
                }


                return pxRs;
            }
       
        }
    }
}
