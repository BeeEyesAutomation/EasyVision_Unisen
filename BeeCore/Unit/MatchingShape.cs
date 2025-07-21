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
using BeeGlobal;

namespace BeeCore
{
    [Serializable()]
    public  class MatchingShape
    {
        public MatchingShape()
        {

        }
        public int Index = -1;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp, matMask;
        
     public ThresholdTypes Methord=ThresholdTypes.Otsu;
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern IntPtr GetTemp(ref int rows, ref int cols, ref int Type, int threshold, int minArea, int method,bool Invert);

        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern IntPtr SetTemp(IntPtr data, int image_rows, int image_cols, MatType matType);
        public  int Threshold=100,  MinArea=100;
    
        public  bool Invert=false;
        public void DoWork()
        {
          
          //  Matching(indexTool);

        }
        public void Complete()
        {
            

        }
        public  Mat GetImgTemp()
        {
            G.CommonPlus.BitmapSrc(BeeCore.Common.listCamera[IndexThread].matRaw.Clone().ToBitmap());
            Common.CropRotate(rotArea);
            int rows = 0, cols = 0, Type = 0;
            IntPtr intPtr = GetTemp(ref rows , ref cols, ref Type , Threshold, MinArea,(int)Methord, Invert);
            unsafe
            {

                Mat raw = new Mat(rows, cols, Type, intPtr);
                matTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(raw);
                return raw;
            }

           
        }
        public void LoadTemp( Mat temp)
        {
            //////Cv2.ImShow("A"+ indexTool, temp);
            if (temp == null) return;
            if (temp.Empty()) return;

         

            SetTemp( temp.Data, temp.Rows, temp.Cols, temp.Type());


        }
        public int IndexThread = 0;
        public void Check(bool IsRun, RectRotate rot)
        {
            if(!IsRun)
            G.CommonPlus.BitmapSrc(BeeCore.Common.listCamera[IndexThread].matRaw.Clone().ToBitmap());
            Common.CropRotate(rot);
            float numNG = G.MatchingShape.CheckShape(Threshold, MinArea, (int)Methord, Invert);
            cycleTime =(int) G.MatchingShape.cycleTool;
            if (numNG > 0)
                IsOK = false;
            else
                IsOK = true;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
     
        public bool IsGetColor;
      
        public TypeCrop TypeCrop;
        public string pathRaw = "";
        public int cycleTime = 0;
        public RectangleF rectArea=new RectangleF();

        private int _areaPixel=1;
        private int _score;
        public int ScoreRs = 0;
        
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

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {

                _score = value;
            }
        }

  
  
    
      
        public bool IsOK = false;
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]

        unsafe public static extern IntPtr GetImageResult(ref int rows, ref int cols, ref int Type);
      
    }
}
