using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using CvPlus;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class Patterns
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
       private Mode _TypeMode=Mode.Pattern;
        public List<double> listScore = new List<double>();
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
        public Compares Compare = Compares.Equal;
        public int LimitCounter = 0;
        public async Task SendResult()
        {
        }
        public bool IsAreaWhite=false;
      
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
               Pattern. m_iMinReduceArea = _minArea;
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
        public List< System.Drawing.Point > listP_Center=new List<System.Drawing.Point>();
        [NonSerialized]
        public Pattern Pattern =new CvPlus.Pattern();
        public Patterns()
        {
            Pattern = new CvPlus.Pattern();
        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern void SetDst(int ixThread, int indexTool, IntPtr data, int image_rows, int image_cols, MatType matType);

        public void LearnPattern(   Mat temp)
        {
            if (temp == null)
                if (temp.Empty())
                    return;
                    ////Cv2.ImShow("A"+ indexTool, temp);
                    //if (temp == null) return;
                    //if (temp.Empty()) return;
                    if (Pattern == null)
			{
				Pattern = new CvPlus.Pattern();
				
			}
            Pattern.CreateTemp(Index, IndexThread);
            matTemp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(temp.Clone());
           
           // Cv2.ImWrite("matTemp.png", temp);
            SetDst(IndexThread, Index, temp.Data, temp.Rows, temp.Cols, temp.Type());
            //  G.CommonPlus.LoadDst(path);
           // Mat mat = new Mat(temp.Rows, temp.Cols, temp.Type(), temp.Data);
           
           Pattern.LearnPattern( minArea, Index, IndexThread);

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
            switch (TypeMode)
            {
                case Mode.Pattern:
                    if (matCrop.Type() == MatType.CV_8UC3)
                        Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
                    else
                        matTemp = matCrop.Clone();
                    break;
                case Mode.OutLine:
                    matTemp =Common. CannyWithMorph(matCrop);
                    //crop = Common.CropRotatedRect(bmMask, rotCrop, rotMask);
                    //Cv2.BitwiseNot(crop, matClear);
                    //Cv2.BitwiseAnd(matClear, matOut, matTemp);
                    //Cv2.BitwiseAnd(crop, matOut, matMask1);
                    //matMask = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(matMask1);
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
                    // crop = Common.CropRotatedRect(bmMask, rotCrop, rotMask);

                    //Cv2.BitwiseNot(crop, matClear);
                   
                    //Cv2.BitwiseAnd(matClear, matOut, matTemp);
                  
                    //Cv2.BitwiseAnd(crop, matOut, matMask1);
                    //matMask = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(matMask1);
                    break;

            }
          
            return matTemp;
        }

       
        public int numTempOK;
        public bool IsAutoTrig { get => isAutoTrig; set => isAutoTrig = value; }
        public int NumOK { get => numOK; set => numOK = value; }
        public int DelayTrig { get => delayTrig; set => delayTrig = value; }
      
        public void SetModel()
        {
			
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
            if (Common.PropetyTools[IndexThread][Index].TypeTool == TypeTool.Position_Adjustment)
            {
                rotAreaAdjustment = rotArea;
                rectRotate = rotAreaAdjustment;
            }    
              
           
            Matching(rectRotate);

        }
        public void Complete()
        {
            Common.PropetyTools[IndexThread][Index].Results = Results.OK;
            switch (Compare)
            {
                case Compares.Equal:
                    if (rectRotates.Count() != LimitCounter)
                        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                    break;
                case Compares.Less:
                    if (rectRotates.Count() >= LimitCounter)
                        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                    break;
                case Compares.More:
                    if (rectRotates.Count() <= LimitCounter)
                        Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                    break;
            }
            if (Common.PropetyTools[IndexThread][Index].Results == Results.OK)
            {
                Matrix mat = new Matrix();
                System.Drawing.Point pZero = new System.Drawing.Point(0, 0);
                PointF[] pMatrix = { pZero };
                mat.Translate(rotArea._PosCenter.X, rotArea._PosCenter.Y);
                mat.Rotate(rotArea._rectRotation);
                mat.Translate(rotArea._rect.X, rotArea._rect.Y);

                mat.Translate(rectRotates[0]._PosCenter.X, rectRotates[0]._PosCenter.Y);
                mat.Rotate(rectRotates[0]._rectRotation);
                mat.TransformPoints(pMatrix);

                int x = (int)pMatrix[0].X;// (int)rotArea._PosCenter.X -(int) rotArea ._rect.Width/2 + (int)rot._PosCenter.X;
                int y = (int)pMatrix[0].Y; ;// (int)rotArea._PosCenter.Y - (int)rotArea._rect.Height / 2 + (int)rot._PosCenter.Y;
                Global.AngleOrigin = rectRotates[0]._angle;
                Global.pOrigin = new OpenCvSharp.Point(x, y);
            }
            if(Common.PropetyTools[IndexThread][Index].TypeTool==TypeTool.Position_Adjustment)
            if (!Global.IsRun)
            {
                Global.StatusDraw = StatusDraw.Check;
                if (Common.PropetyTools[Global.IndexChoose][Index].Results==Results.OK)
                {
                    rotPositionAdjustment = rectRotates[0].Clone();
                    Global.rotOriginAdj = new RectRotate(rotCrop._rect, new PointF(rotArea._PosCenter.X -rotArea._rect.Width / 2 + rotPositionAdjustment._PosCenter.X,rotArea._PosCenter.Y - rotArea._rect.Height / 2 + rotPositionAdjustment._PosCenter.Y), rotPositionAdjustment._rectRotation, AnchorPoint.None, false);
                }    
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

        public int IndexThread;
        public void Matching( RectRotate rectRotate)
        {
            using (Mat raw = BeeCore.Common.listCamera[IndexThread].matRaw.Clone())
            {

                if (raw.Empty()) return;

                Mat matCrop = Common.CropRotatedRect(raw, rectRotate, rotMask);
                Mat matProcess = new Mat();

                switch (TypeMode)
                {
                    case Mode.Pattern:
                        if (matCrop.Type() == MatType.CV_8UC3)
                            Cv2.CvtColor(matCrop, matProcess, ColorConversionCodes.BGR2GRAY);
                        else
                            matProcess = matCrop;
                        break;
                    case Mode.OutLine:
                        matProcess = Common.CannyWithMorph(matCrop);
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
                                return;

                            // Chuyển kết quả ngược về byte[] rồi sang Mat
                            byte[] edgeBytes = result.As<byte[]>();
                            matProcess = new Mat(height, width, MatType.CV_8UC1, edgeBytes);
                        }

                        break;
                }

                // Cv2.ImWrite("Processing.png", matProcess);
                //   BeeCore.Native.SetImg(matProcess);
                if (!matCrop.IsContinuous())
                {
                    matCrop = matCrop.Clone();
                }
                // Cv2.ImWrite("Crop.png", matCrop);

                String sResult = Pattern.Match(matCrop.Data, matCrop.Cols, matCrop.Rows,  (int)matCrop.Step(), matCrop.Type(),IndexThread,Index, IsHighSpeed, AngleLower, AngleUper, Common.PropetyTools[IndexThread][Index].Score / 100.0,ckSIMD, ckBitwiseNot, ckSubPixel, NumObject, OverLap);
                Common.PropetyTools[IndexThread][Index].ScoreResult = Pattern.ScoreRS;
                rectRotates = new List<RectRotate>();
                listScore = new List<double>();
                listP_Center = new List<System.Drawing.Point>();
                Common.PropetyTools[IndexThread][Index].Results = Results.NG;
                if (sResult != "")
                {
                    cycleTime = (int)Pattern.cycleOutLine;

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
                            listScore.Add(Math.Round(Score, 1));
                            listP_Center.Add(new System.Drawing.Point((int)rectRotate._PosCenter.X - (int)rectRotate._rect.Width / 2 + (int)pCenter.X, (int)rectRotate._PosCenter.Y - (int)rectRotate._rect.Height / 2 + (int)pCenter.Y));
                        }

                    }
                }
                matProcess.Dispose();
                matCrop.Dispose();

            }

        }

    }
}
