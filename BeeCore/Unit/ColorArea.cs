using BeeCore.Func;
using BeeCore.Funtion;
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

namespace BeeCore
{
    [Serializable()]
    public  class ColorArea
    {
        public ColorArea ()
        {
            ColorAreaPlus = new CvPlus.ColorArea();

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int Index = -1;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public bool IsGetColor;
        public int IndexThread = 0;
        public TypeCrop TypeCrop;
        public int pxTemp = 0;
        public String listColor = "";
        private int _areaPixel=1;
        private int styleColor;
        [NonSerialized]
        public  CvPlus.ColorArea ColorAreaPlus = new CvPlus.ColorArea();
        public int AreaPixel
        {
            get
            {
                return _areaPixel;
            }
            set
            {

                _areaPixel = value;
            }
        }


        public int StyleColor { get => styleColor; 
            set { styleColor = value;
                  ColorAreaPlus.StyleColor = styleColor;
            }  }

        public void Undo()
        {

              ColorAreaPlus.Undo(AreaPixel);
            //SetColor();
           // return OpenCvSharp.Extensions.BitmapConverter.ToMat(G.CommonPlus.GetImageRsTemp());
           
        }

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
            if(ColorAreaPlus==null)
            ColorAreaPlus = new CvPlus.ColorArea();
            Common.PropetyTools[IndexThread][Index].StepValue = 1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        public System.Drawing.Color GetColor( Mat raw, int x,int y)
        {
            using (Mat mat = raw.Clone())
            {
                if (mat.Empty()) return Color.Empty;
                if (mat.Type() == MatType.CV_8UC1)
                {
                    Cv2.CvtColor(mat, mat, ColorConversionCodes.GRAY2BGR);
                }
                //  Mat contrastImg = new Mat();
                //Cv2.CvtColor(raw, contrastImg, ColorConversionCodes.BGR2GRAY);
                //Cv2.EqualizeHist(contrastImg, contrastImg);
                //Cv2.CvtColor(contrastImg, contrastImg, ColorConversionCodes.GRAY2BGR);
                ColorAreaPlus.StyleColor = styleColor;
                //  Cv2.ImWrite("Color.png", raw);
                //    G.CommonPlus.BitmapSrc(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat.Clone()));

                String S = ColorAreaPlus.GetColor(mat.Data, mat.Cols, mat.Rows, (int)mat.Step(), mat.Type(), x, y);
                
                clShow = System.Drawing.Color.Black;
                if (S == null || S == "") return clShow;
                String[] sp = S.Split(',');
                clShow = System.Drawing.Color.FromArgb(255, Convert.ToInt32(sp[0]), Convert.ToInt32(sp[1]), Convert.ToInt32(sp[2]));
            }
            return clShow;

            
        }
        public void AddColor()
        {
         
            ColorAreaPlus.AddColor();
            listColor = ColorAreaPlus.SaveTemp();
            listCLShow.Add(clShow);
        }

      
       
        public void LoadTemp()
        {
            
         //   if (BeeCore.Common.listCamera[IndexThread] == null) return;
          //  if(BeeCore.Common.listCamera[IndexThread].matRaw.Empty())return;
         //   BeeCore.Native.SetImg(BeeCore.Common.listCamera[IndexThread].matRaw);
              ColorAreaPlus.StyleColor = styleColor;
    //   Mat matCrop=     Common.CropRotatedRect(BeeCore.Common.listCamera[IndexThread].matRaw, rotArea,rotMask);
         //   if (matCrop.Empty()) return;

         //   Native.SetImg(matCrop, TypeImg.Crop);
        //    BeeCore.G.CommonPlus.CropRotate((int)rotArea._PosCenter.X, (int)rotArea._PosCenter.Y, (int)rotArea._rect.Width, (int)rotArea._rect.Height, rotArea._angle);

            //  BeeCore.Camera.Read();
           
              ColorAreaPlus.LoadTemp(listColor);
          //    ColorAreaPlus.SetColorArea(AreaPixel);
        }
        public Mat ClearTemp()
        {
              ColorAreaPlus.StyleColor = styleColor;
            listColor = "";
         
              ColorAreaPlus.LoadTemp(listColor);
              ColorAreaPlus.SetColorArea(AreaPixel);
            listColor = ColorAreaPlus.SaveTemp();
            return new Mat();

        }
        public Mat SetColor()
        {
            return new Mat();
            //using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            //{
            //    matProcess = new Mat();
            //    if (raw.Empty()) return new Mat();
            //    Mat matCrop = Common.CropRotatedRect(raw, rotArea, rotMask);





            //    // Tăng độ tương phản
            //    int clipLimit = 2;
            //    // Mat contrastImg = new Mat();
            //    if (matCrop.Type() == MatType.CV_8UC1)
            //    {
            //        Cv2.CvtColor(matCrop, matProcess, ColorConversionCodes.GRAY2BGR);
            //    }
            //    else
            //        matProcess = matCrop;
            //    // Mat contrastImg = new Mat(); 
            //    //    if (raw.Type() == MatType.CV_8UC1)
            //    //{
            //    //    Cv2.CvtColor(raw, raw, ColorConversionCodes.GRAY2BGR);
            //    //}
            //    //Cv2.CvtColor(raw, contrastImg, ColorConversionCodes.BGR2GRAY);
            //    //Cv2.EqualizeHist(contrastImg, contrastImg);
            //    //Cv2.CvtColor(contrastImg, contrastImg, ColorConversionCodes.GRAY2BGR);
            //    ColorAreaPlus.StyleColor = styleColor;
            //    Native.SetImg(matProcess, TypeImg.Crop);
            //    pxTemp = ColorAreaPlus.SetColorArea(AreaPixel);


            //    listColor = ColorAreaPlus.SaveTemp();
            //    return OpenCvSharp.Extensions.BitmapConverter.ToMat(G.CommonPlus.GetImageRsTemp());

            //}
          
        }
     
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]

        unsafe public static extern IntPtr GetImageResult(ref int rows, ref int cols, ref int Type);
      
      
        [NonSerialized]      
       
        public Mat matRs  = new Mat();
      
        public void DoWork(RectRotate rotCrop)
        {

            matRs = CheckColor(rotCrop);

        }
        public void Complete()
        {
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

            if (Common.PropetyTools[Global.IndexChoose][Index].Results == Results.NG)
            {
                cl = Global.ColorNG;
            }
            else
            {
                cl = Global.ColorOK;
            }
            String nameTool = (int)(Index + 1) + "." + Common.PropetyTools[Global.IndexChoose][Index].Name;
            if (!Global.IsHideTool)
                Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl, 1);
            if(matRs!=null)
            if (!matRs .Empty())
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
                //if(matRs.Type()==MatType.CV_8UC1)
                //{
                //    Cv2.CvtColor(matRs, matRs, ColorConversionCodes.GRAY2BGR);
                //}
                Bitmap myBitmap = matRs.ToBitmap(); ;
                myBitmap.MakeTransparent(Color.Black);
                myBitmap = General.ChangeToColor(myBitmap,cl, 0.5f);
                gc.DrawImage(myBitmap, rotA._rect);
            }

            return gc;
        }
        [NonSerialized]
        public Mat matProcess = new Mat();
        float ValueColor = 0;
        public Mat CheckColor(RectRotate rotCrop)
        {
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {
                matProcess = new Mat();
                if (raw.Empty()) return new Mat();
                Mat matCrop = Common.CropRotatedRect(raw, rotCrop, rotMask);
                if (matProcess == null) matProcess = new Mat();
                ColorAreaPlus.StyleColor = styleColor;

                double contrastFactor = 4.0;
                double sharpenFactor = 4.0;



               
                // Tăng độ tương phản
                int clipLimit = 2;
                // Mat contrastImg = new Mat();
                if (matCrop.Type() == MatType.CV_8UC1)
                {
                    Cv2.CvtColor(matCrop, matProcess, ColorConversionCodes.GRAY2BGR);
                }
                else
                    matProcess = matCrop;
                //Cv2.CvtColor(input, contrastImg,ColorConversionCodes.BGR2GRAY);
                //Cv2.EqualizeHist(contrastImg, contrastImg);
                //Cv2.CvtColor(contrastImg, contrastImg, ColorConversionCodes.GRAY2BGR);
                //// 2. Tăng tương phản bằng CLAHE
                //CLAHE clahe = Cv2.CreateCLAHE(clipLimit, new OpenCvSharp.Size(8, 8));
                //Mat contrast = new Mat();
                //clahe.Apply(contrastImg, contrast);

                //  Cv2.ImWrite(nameTool + ".png", contrastImg);
                if (!matProcess.IsContinuous())
                {
                    matProcess = matProcess.Clone();
                }
                //ColorAreaPlus.LoadTemp(listColor);
                BeeCore.Native.SetImg(matProcess);
                 ValueColor = ColorAreaPlus.CheckColor(  AreaPixel);
                if (!Global.IsRun) 
                    pxTemp =(int) ValueColor;
                Common.PropetyTools[IndexThread][Index].ScoreResult = (float)((ValueColor / (pxTemp * 1.0)) * 100);
                if (Common.PropetyTools[IndexThread][Index].ScoreResult > 100)
                    Common.PropetyTools[IndexThread][Index].ScoreResult = 100;
                if (Common.PropetyTools[IndexThread][Index].ScoreResult < 0)
                    Common.PropetyTools[IndexThread][Index].ScoreResult = 0;
                //if (pxMathching>(pxTemp* Score) / 100)
                //{
                //  //  mask = Mat(matRS.rows, matRS.cols, CV_8UC3, Scalar(255, 255,255));
                //    bitwise_and(mask, matRS, matResult);
                //    cycle = int(clock() - d1);
                //    return true;
                //}
                //else
                //{
                //    //mask = Mat(matRS.rows, matRS.cols, CV_8UC3, Scalar(255, 0, 255));
                //    bitwise_and(mask, matRS, matResult);
                //    cycle = int(clock() - d1);
                // 
                //    return false;
                //}
                //    return false;
                // Common.PropetyTools[IndexThread][Index].ScoreResult =   ColorAreaPlus.ScoreRS;
                int rows = 0, cols = 0, Type = 0;

                IntPtr intPtr = GetImageResult(ref rows, ref cols, ref Type);
                unsafe
                {

                    Mat raws = new Mat(rows, cols, Type, intPtr);
                  //  Cv2.ImWrite("color.png", raws);
                    return raws;
                }
            }

        }
    }
}
