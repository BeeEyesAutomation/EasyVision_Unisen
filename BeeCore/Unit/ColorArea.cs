using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using OpenCvSharp.Extensions;
using System.ComponentModel;
using Python.Runtime;
using System.Windows.Forms;
using BeeCore.Funtion;
using BeeGlobal;

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
        public int Index = -1;
       
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public bool IsGetColor;
        public Mode TypeMode = Mode.Pattern;
        public TypeCrop TypeCrop;

        private int _areaPixel=1;
        private int styleColor;
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
                  G.colorArea[Index].StyleColor = styleColor;
            }  }

        public Mat Undo(Mat raw, bool IsCalib=false)
        {

              G.colorArea[Index].Undo(AreaPixel);
            SetColor(false, raw);
            return OpenCvSharp.Extensions.BitmapConverter.ToMat(G.CommonPlus.GetImageRsTemp());
           
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
            if (Index > G.colorArea.Count - 1) G.colorArea.Add(new CvPlus.ColorArea());
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        public System.Drawing.Color GetColor( Mat raw, int x,int y)
        {
            if(raw.Empty())return Color.Empty;
            if (raw.Type()==MatType.CV_8UC1){
                Cv2.CvtColor(raw, raw, ColorConversionCodes.GRAY2BGR);
            }
          //  Mat contrastImg = new Mat();
            //Cv2.CvtColor(raw, contrastImg, ColorConversionCodes.BGR2GRAY);
            //Cv2.EqualizeHist(contrastImg, contrastImg);
            //Cv2.CvtColor(contrastImg, contrastImg, ColorConversionCodes.GRAY2BGR);
              G.colorArea[Index].StyleColor = styleColor;
          //  Cv2.ImWrite("Color.png", raw);
            G.CommonPlus.BitmapSrc(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(raw.Clone()));
           
            String S =   G.colorArea[Index].GetColor(x, y);
            clShow = System.Drawing.Color.Black;
            if (S == null||S=="") return clShow;
            String[] sp = S.Split(',');
            clShow = System.Drawing.Color.FromArgb(255, Convert.ToInt32(sp[0]), Convert.ToInt32(sp[1]), Convert.ToInt32(sp[2]));
          
            return clShow;

            
        }
        public void AddColor()
        {
            if (Index > G.colorArea.Count-1) G.colorArea.Add(new CvPlus.ColorArea());
            G.colorArea[Index].AddColor();
            listCLShow.Add(clShow);
        }

        public int pxTemp=0;
        public String listColor="";
        public int IndexThread = 0;
        public void LoadTemp()
        {
            if (Index > G.colorArea.Count-1) G.colorArea.Add(new CvPlus.ColorArea());
            if (BeeCore.Common.listCamera[IndexThread] == null) return;
            if(BeeCore.Common.listCamera[IndexThread].matRaw.Empty())return;
            BeeCore.Native.SetImg(BeeCore.Common.listCamera[IndexThread].matRaw);
              G.colorArea[Index].StyleColor = styleColor;
       Mat matCrop=     Common.CropRotatedRect(BeeCore.Common.listCamera[IndexThread].matRaw, rotArea,rotMask);
            if (matCrop.Empty()) return;

            Native.SetImg(matCrop, TypeImg.Crop);
        //    BeeCore.G.CommonPlus.CropRotate((int)rotArea._PosCenter.X, (int)rotArea._PosCenter.Y, (int)rotArea._rect.Width, (int)rotArea._rect.Height, rotArea._angle);

            //  BeeCore.Camera.Read();
              G.colorArea[Index].LoadTemp(listColor);
              G.colorArea[Index].SetColorArea(AreaPixel);
        }
        public Mat ClearTemp()
        {
              G.colorArea[Index].StyleColor = styleColor;
            listColor = "";
         
              G.colorArea[Index].LoadTemp(listColor);
              G.colorArea[Index].SetColorArea(AreaPixel);
            return OpenCvSharp.Extensions.BitmapConverter.ToMat(G.CommonPlus.GetImageRsTemp());

        }
        public Mat SetColor(bool IsCCD, Mat raw)
        {
           // Mat contrastImg = new Mat(); 
            if (raw.Type() == MatType.CV_8UC1)
            {
                Cv2.CvtColor(raw, raw, ColorConversionCodes.GRAY2BGR);
            }
            //Cv2.CvtColor(raw, contrastImg, ColorConversionCodes.BGR2GRAY);
            //Cv2.EqualizeHist(contrastImg, contrastImg);
            //Cv2.CvtColor(contrastImg, contrastImg, ColorConversionCodes.GRAY2BGR);
              G.colorArea[Index].StyleColor = styleColor;
            Native.SetImg(raw, TypeImg.Crop);
             pxTemp =   G.colorArea[Index].SetColorArea(AreaPixel);
           
         
                listColor =   G.colorArea[Index].SaveTemp();
                 return OpenCvSharp.Extensions.BitmapConverter.ToMat(G.CommonPlus.GetImageRsTemp());
      
            
          
        }
        public bool IsOK = false;
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]

        unsafe public static extern IntPtr GetImageResult(ref int rows, ref int cols, ref int Type);
      
        public Bitmap CheckColor( Mat raw)
        {
            //Mat contrastImg = new Mat();
            if (raw.Type() == MatType.CV_8UC1)
            {
                Cv2.CvtColor(raw, raw, ColorConversionCodes.GRAY2BGR);
            }
            //Cv2.CvtColor(raw, contrastImg, ColorConversionCodes.BGR2GRAY);
            //Cv2.EqualizeHist(contrastImg, contrastImg);
            //Cv2.CvtColor(contrastImg, contrastImg, ColorConversionCodes.GRAY2BGR);
              G.colorArea[Index].StyleColor = styleColor;

            Bitmap btmRaw = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(raw);

                G.CommonPlus.BitmapSrc(btmRaw);
            

                G.colorArea[Index].LoadTemp(listColor);
            IsOK =   G.colorArea[Index].CheckColor(false,0,0,0,0,0,AreaPixel,(int)Common.PropetyTools[IndexThread][Index].Score, pxTemp);
            return BeeCore.Native.GetImg(TypeImg.Result).ToBitmap();
        
        }
      
        public String nameTool = "";
        public Bitmap bmRS = null;
      
        public void DoWork(RectRotate rotCrop)
        {
           
            bmRS = CheckColor(rotCrop).ToBitmap();

        }
        public void Complete()
        {

        }
       
        public int ScoreRs;
        public Mat CheckColor(RectRotate rotCrop)
        {
              G.colorArea[Index].StyleColor = styleColor;

            double contrastFactor = 4.0;
            double sharpenFactor = 4.0;


          
            Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone();
            // Tăng độ tương phản
            int clipLimit = 2;
           // Mat contrastImg = new Mat();
            if (raw.Type() == MatType.CV_8UC1)
            {
                Cv2.CvtColor(raw, raw, ColorConversionCodes.GRAY2BGR);
            }
            //Cv2.CvtColor(input, contrastImg,ColorConversionCodes.BGR2GRAY);
            //Cv2.EqualizeHist(contrastImg, contrastImg);
            //Cv2.CvtColor(contrastImg, contrastImg, ColorConversionCodes.GRAY2BGR);
            //// 2. Tăng tương phản bằng CLAHE
            //CLAHE clahe = Cv2.CreateCLAHE(clipLimit, new OpenCvSharp.Size(8, 8));
            //Mat contrast = new Mat();
            //clahe.Apply(contrastImg, contrast);

            //  Cv2.ImWrite(nameTool + ".png", contrastImg);

              G.colorArea[Index].LoadTemp(listColor);
            BeeCore.Native.SetImg(raw);
            IsOK =   G.colorArea[Index].CheckColor(true,(int)rotCrop._PosCenter.X, (int)rotCrop._PosCenter.Y, (int)rotCrop._rect.Width, (int)rotCrop._rect.Height, rotCrop._angle, AreaPixel, (int)Common.PropetyTools[IndexThread][Index].Score, pxTemp);
            ScoreRs =   G.colorArea[Index].ScoreRS;
            int rows = 0, cols = 0 ,Type = 0;
           
            IntPtr intPtr= GetImageResult( ref rows, ref cols, ref Type);
            unsafe
            {
              
                Mat raws = new Mat(rows, cols, Type, intPtr);
                return raws;
            }
        

        }
    }
}
