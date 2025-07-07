using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;

namespace BeeCore
{
    [Serializable()]
    public class PositionAdj
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsIni = false;
        public TypeTool TypeTool;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap matTemp,matMask;
        public List<Point> Postion=new List<Point>();
       private Mode _TypeMode=Mode.Pattern;
        public Mode TypeMode
        {
            get
            {
                return _TypeMode;
            }
            set
            {
                _TypeMode = value;
                
            }
        }
        int _NumObject = 0;
        public int NumObject
        {
            get
            {
                return _NumObject;
            }
            set
            {
                _NumObject = value;
               
            }
        }
         bool isHighSpeed=false;
        public bool IsHighSpeed {
            get
            {
                return isHighSpeed;
            }
            set
            {
                isHighSpeed = value;
              
            }
        }

        public TypeCrop TypeCrop;
        public string pathRaw = "";
        public int cycleTime = 0;
        public RectangleF rectArea;
      
        public bool IsOK = false;
        public bool IsAreaWhite=false;
        public int ScoreRs = 0;
        int _threshMin;
        public int threshMin
        {
            get
            {
                return _threshMin;
            }
            set
            {
                _threshMin = value;
                
            }
        }
        int _threshMax;
        public int threshMax
        {
            get
            {
                return _threshMax;
            }
            set
            {
                _threshMax = value;
              
            }
        }
        private double _angle=10;
        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
            }
        }
        double _AngleLower;
        public double AngleLower
        {
            get
            {
                return _AngleLower;
            }
            set
            {
                _AngleLower = value;
                
            }
        }
        double _AngleUper;
        public double AngleUper
        {
            get
            {
                return _AngleUper;
            }
            set
            {
                _AngleUper = value;
                
            }
        }
        int _maxCount=9;
        public int MaxCount
        {
            get
            {
                return _maxCount;
            }
            set
            {
                _maxCount = value;
            
            }
        }
        int _minArea;
        public int minArea
        {
            get
            {
                return _minArea;
            }
            set
            {
                _minArea = value;
                G.pattern.m_iMinReduceArea = _minArea;
            }
        }
        double _OverLap;
        public double OverLap
        {
            get
            {
                return _OverLap;
            }
            set
            {
                _OverLap = value;
             
            }
        }

        public bool IsProcess
        {
            get
            {
                return G.pattern.IsProcess;
            }
            set
            {
                G.pattern.IsProcess = value;
            }
        }
        bool _ckSIMD=true;
        public bool ckSIMD
        {
            get
            {
                return _ckSIMD;
            }
            set
            {
                _ckSIMD = value;
                
            }
        }
        bool _ckBitwiseNot;
        public bool ckBitwiseNot
        {
            get
            {
                return _ckBitwiseNot;
            }
            set
            {
                _ckBitwiseNot = value;
              
            }
        }
        bool _ckSubPixel = true;
        public bool ckSubPixel
        {
            get
            {
                return _ckSubPixel;
            }
            set
            {
                _ckSubPixel = value;
              
            }
        }
        private bool isAutoTrig;
        private int numOK;
        private int delayTrig;
        public PositionAdj()
        {

        }
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern void SetDst(int ixThread, int indexTool, IntPtr data, int image_rows, int image_cols, MatType matType);

        public void LearnPattern(  int indexTool, Mat temp)
        {
           ////Cv2.ImShow("A"+ indexTool, temp);
            if (temp == null) return;
            if (temp.Empty()) return;
           
            matTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(temp.Clone());
            SetDst(IndexThread, indexTool, temp.Data, temp.Rows, temp.Cols, temp.Type());

           G.pattern.LearnPattern(minArea, indexTool, IndexThread);

        }
    

      
    
        public Mat GetTemp(RectRotate rotCrop, RectRotate rotMask, Mat matRaw, Mat bmMask)
        {
           
            Mat matClear = new Mat();
            Mat matTemp = new Mat();
            Mat matOut = new Mat();
            Mat matCrop = new Mat();
            Mat matMask1 = new Mat();
            switch (TypeMode)
            {
                case Mode.Pattern:
                    if (matCrop.Type() == MatType.CV_8UC3)
                        Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
                    else
                        matTemp = matCrop.Clone();
                    break;
                case Mode.OutLine:
                    matTemp = Common.CannyWithMorph(matCrop);
                    break;
                case Mode.Edge:
                    using (Py.GIL())
                    {
                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
                        int height = matCrop.Rows;
                        int width = matCrop.Cols;
                        int channels = matCrop.Channels();
                        if (!matCrop.IsContinuous())
                        {
                            matCrop = matCrop.Clone();
                        }
                        int size = (int)(matCrop.Total() * matCrop.ElemSize());
                        byte[] buffer = new byte[size];
                        Marshal.Copy(matCrop.Data, buffer, 0, size);
                        // Tạo ndarray từ byte[]
                        var npImage = G.np.array(buffer).reshape(height, width, channels);
                        // Gọi hàm Python
                        dynamic result = G.Classic.EdgeDetection(npImage);
                        if (result == null)
                            return null;

                        // Chuyển kết quả ngược về byte[] rồi sang Mat
                        byte[] edgeBytes = result.As<byte[]>();
                        matTemp = new Mat(height, width, MatType.CV_8UC1, edgeBytes);
                    }
                 
                    break;

            }

            return matTemp;
        }

        private int _score = 70;
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
        public int numTempOK;
        public bool IsAutoTrig { get => isAutoTrig; set => isAutoTrig = value; }
        public int NumOK { get => numOK; set => numOK = value; }
        public int DelayTrig { get => delayTrig; set => delayTrig = value; }
        public List<RectRotate> rectRotates = new List<RectRotate>();
        public int Index = 0;
        public int IndexThread = 0;
        public void Matching( RectRotate rot)
        {
            //if (!IsRun)
            //{
            //   // Mat matRS = Processing2(BeeCore.Common.matRaw.Clone());
            //    G.CommonPlus.BitmapSrc(BeeCore.Common.matRaw.Clone().ToBitmap());
            //}
            //if (BeeCore.Common.TypeCCD == TypeCamera.TinyIV)
            //    BeeCore.Common.SetRaw();
            Mat matCrop = Common.CropRotatedRect(BeeCore.Common.listCamera[IndexThread].matRaw, rot, rotMask);
         //   BeeCore.Native.SetImg(matCrop);

            String sResult = G.pattern.Match(matCrop.Data, matCrop.Rows, matCrop.Cols, (int)matCrop.Step(), matCrop.Type(), IndexThread, Index, IsHighSpeed, AngleLower, AngleUper, Score / 100.0, ckSIMD, ckBitwiseNot, ckSubPixel, NumObject, OverLap);
            ScoreRs = G.pattern.ScoreRS;
            rectRotates = new List<RectRotate>();
           
            IsOK = false;
            if (sResult != "")
            {
                cycleTime = (int)G.pattern.cycleOutLine;

                if (sResult != "")
                {
                    String[] sSplit = sResult.Split('\n');
                    foreach (String s in sSplit)
                    {
                        if (s.Trim() == "") break;
                        String[] sSp = s.Split(',');
                        PointF pCenter = new PointF(Convert.ToSingle(sSp[0]), Convert.ToSingle(sSp[1]));
                        float angle = Convert.ToSingle(sSp[2]);
                        float width = Convert.ToSingle(sSp[3]);
                        float height = Convert.ToSingle(sSp[4]);
                        float Score = Convert.ToSingle(sSp[5]);
                        rectRotates.Add(new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None, false));
                     
                   }

                }
            }




        }
    }
}
