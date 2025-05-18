using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;

namespace BeeCore
{
    [Serializable()]
    public class Positions
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
        public Bitmap matTemp,matMask;
        public List<Point> Postion=new List<Point>();
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
        public System.Drawing.Point pOrigin;
        public  System.Drawing.Point pCenter;
        public double DistanceDetect = 0, AngleDetect = 0;
        public double deltaX, deltaY, AngleOrigin;
        public Positions()
        {

        }
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern void SetDst(int indexTool, IntPtr data, int image_rows, int image_cols, MatType matType);

        public void LearnPattern(  Mat temp)
        {
           ////Cv2.ImShow("A"+ indexTool, temp);
            if (temp == null) return;
            if (temp.Empty()) return;
           
            matTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(temp.Clone());
            SetDst(Index, temp.Data, temp.Rows, temp.Cols, temp.Type());
            //  G.CommonPlus.LoadDst(path);
           // Mat mat = new Mat(temp.Rows, temp.Cols, temp.Type(), temp.Data);
           
           G.pattern.LearnPattern(minArea, Index);

        }
    

        int getMaxAreaContourId(OpenCvSharp.Point[][] contours)
        {
            double maxArea = 0;
            int maxAreaContourId = -1;
            for (int j = 0; j < contours.Count(); j++)
            {
                double newArea = Cv2.ContourArea(contours[j]);
                if (newArea > maxArea)
                {
                    maxArea = newArea;
                    maxAreaContourId = j;
                } // End if
            } // End for
            return maxAreaContourId;
        }
    
        public Mat GetTemp(RectRotate rotateRect, Mat matRaw, Mat bmMask)
        {
           
            Mat matClear = new Mat();
            Mat matTemp = new Mat();
         
                float angle = rotateRect._rectRotation;
                if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
                Mat matCrop = Common.CropRotatedRect(matRaw, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
                else
                    matTemp = matCrop.Clone();
                if (IsAreaWhite)
                    Cv2.BitwiseNot(matTemp, matTemp);
               // matTemp = Processing(matTemp);
            
           
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

        public List<RectRotate> rectRotates = new List<RectRotate>();
        public TypeTool TypeTool= TypeTool.Positions;
        public String nameTool = "";
        public StatusTool StatusTool = StatusTool.None;
       
        public void DoWork()
        {
            StatusTool = StatusTool.Processing;
            Matching(Index);

        }
        public void Complete()
        {
            StatusTool = StatusTool.Done;

        }

        (double, double) ConvertToB(double xA, double yA, double xB, double yB, double alpha)
        {
            //  if (alpha < 0) alpha = 180 + alpha;
            // Dịch chuyển A về gốc B
            double dx = xA - xB;
            double dy = yA - yB;
            double radians = alpha * Math.PI / 180;
            // Xoay ngược lại góc alpha
            double xAInB = dx * Math.Cos(-radians) - dy * Math.Sin(-radians);
            double yAInB = dx * Math.Sin(-radians) + dy * Math.Cos(-radians);

            return (Math.Round(xAInB), Math.Round(yAInB));
            // return ( xRotated, yRotated);
        }
        public void Matching( int indexTol)
        {


            //if (!IsRun)
            //{
            //   // Mat matRS = Processing2(BeeCore.Common.matRaw.Clone());
            //    G.CommonPlus.BitmapSrc(BeeCore.Common.matRaw.Clone().ToBitmap());
            //}
            //if (BeeCore.Common.TypeCCD == TypeCamera.TinyIV)
            //    BeeCore.Common.SetRaw();
            if (BeeCore.Common.matRaw.Empty()) return;
            //if(!IsRun)
            BeeCore.Native.SetImg(BeeCore.Common.matRaw);
            IsOK = G.pattern.Match((int)rotAreaAdjustment._PosCenter.X, (int)rotAreaAdjustment._PosCenter.Y, (int)rotAreaAdjustment._rect.Width, (int)rotAreaAdjustment._rect.Height, rotAreaAdjustment._angle, indexTol, false,IsHighSpeed,AngleLower,AngleUper,Score/100.0,threshMin,threshMax,ckSIMD,ckBitwiseNot,ckSubPixel,1,OverLap);
            ScoreRs = G.pattern.ScoreRS;
            if (IsOK)
            {
                cycleTime = (int)G.pattern.cycleOutLine;
                rectRotates = new List<RectRotate>();
                if (G.pattern.listMatch != null)
                {
                    String[] sSplit = G.pattern.listMatch.Split('\n');
                    System.Drawing.Point pZero = new System.Drawing.Point(0, 0);
                    PointF[] pMatrix = { pZero };
                    foreach (String s in sSplit)
                    {
                        if (s.Trim() == "") break;
                        String[] sSp = s.Split(',');
                        PointF pCenter = new PointF(Convert.ToSingle(sSp[0]), Convert.ToSingle(sSp[1]));
                        float angle = Convert.ToSingle(sSp[2]);
                        float width = Convert.ToSingle(sSp[3]);
                        float height =Convert.ToSingle(sSp[4]);
                        rectRotates.Add(new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None));
                    }
                    if (rectRotates.Count == 0) return;
                    RectRotate rot = rectRotates[0];
                    Matrix mat = new Matrix();
                    RectRotate rotA = rotArea;
                    if(rotAreaAdjustment!=null)
                    rotA = rotAreaAdjustment;
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);
                    mat.Translate(rotA._rect.X, rotA._rect.Y);
            
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    mat.Rotate(rot._rectRotation);
                    mat.TransformPoints(pMatrix);
                    int x = (int)pMatrix[0].X;
                    int y = (int)pMatrix[0].Y;
                    // Điểm B và C
                    double x2 = 10, y2 = 0;
                    double x3 = 0, y3 = 1;
                    (deltaX, deltaY) = ConvertToB(x, y, pOrigin.X, pOrigin.Y, AngleOrigin);
                    deltaY = -deltaY;
                    double angleAB = Math.Atan2(y2 - 0, x2 - 0) * 180 / Math.PI;
                    double angleAC = Math.Atan2(deltaY - 0, deltaX - 0) * 180 / Math.PI;

                    double delta = angleAC- angleAB  ;

                    // Chuẩn hóa về [-180, 180]
                    if (delta > 180) delta -= 360;
                    if (delta < -180) delta += 360;
                  
                    DistanceDetect = Math.Round(Math.Sqrt(Math.Pow(x -pOrigin.X, 2) + Math.Pow(y - pOrigin.Y, 2)));

                    AngleDetect =Math.Round(delta, 0);
                    if (rectRotates.Count ==0)
                        IsOK = false;
                }
            }
            



        }
    }
}
