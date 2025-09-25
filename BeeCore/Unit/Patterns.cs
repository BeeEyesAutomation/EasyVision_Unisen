using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
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
        public Mat matTemp;
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
        public Bitmap bmRaw;
       
     
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
               //Pattern. m_iMinReduceArea = _minArea;
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
        public BeeCpp.Pattern Pattern =new BeeCpp.Pattern();
        public Patterns()
        {
           
        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
		public Mat LearnPattern(Mat raw, bool IsNoCrop)
		{

			using (Mat img = raw.Clone())
			{
				// Chuẩn hóa góc
				if (rotCrop._rectRotation < 0)
					rotCrop._rectRotation += 360;

				// Chuẩn hóa kênh về BGR 3 kênh
				if (img.Channels() == 3)
					Cv2.CvtColor(img, img, ColorConversionCodes.BGR2GRAY);
				else if (img.Channels() == 4)
					Cv2.CvtColor(img, img, ColorConversionCodes.BGRA2GRAY);

				int w = 0, h = 0, s = 0, c = 0;
				IntPtr intpr = IntPtr.Zero;
				Mat mat = new Mat();

				try
				{
					// Gọi native – chú ý truyền đúng kích thước crop
					if (IsNoCrop)
					{
						intpr = Pattern.SetImgeSample(
							img.Data, img.Width, img.Height, (int)img.Step(), img.Channels(),
							rotCrop._PosCenter.X, rotCrop._PosCenter.Y,
							img.Width, img.Height,              // size giữ nguyên
							rotCrop._rectRotation, IsNoCrop,
							out w, out h, out s, out c);
					}
					else
					{
						intpr = Pattern.SetImgeSample(
							img.Data, img.Width, img.Height, (int)img.Step(), img.Channels(),
							rotCrop._PosCenter.X, rotCrop._PosCenter.Y,
							rotCrop._rect.Width, rotCrop._rect.Height,   // size crop
							rotCrop._rectRotation, IsNoCrop,
							out w, out h, out s, out c);
					}

					if (intpr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
						return mat; // trả Mat rỗng
					Pattern.LearnPattern();
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
      
  
       
        public int numTempOK;
        public bool IsAutoTrig { get => isAutoTrig; set => isAutoTrig = value; }
        public int NumOK { get => numOK; set => numOK = value; }
        public int DelayTrig { get => delayTrig; set => delayTrig = value; }
      
        public void SetModel()
        {
			if (Pattern == null)
			{
				Pattern = new BeeCpp.Pattern();

			}
            if (bmRaw != null)
            {
				matTemp = bmRaw.ToMat();
				LearnPattern(matTemp, true);
			}
			
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
				cl = Global.ColorNG;
			}
			else
			{
				cl = Global.ColorOK;
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
					Draws.Box2Label(gc, rot._rect, i + "", Math.Round(listScore[i - 1], 1) + "% A:" + Math.Round(rot._angle), Global.fontRS, cl, brushText, 16, 2);


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

				if (raw.Empty()) return;
				if (raw.Type() == MatType.CV_8UC3)
					Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);

				Pattern.SetRawNoCrop(raw.Data, raw.Width, raw.Height, (int)raw.Step(), raw.Channels());

				if (raw.Empty()) return;

				

				rectRotates = new List<RectRotate>();
				listScore = new List<double>();
				listP_Center = new List<System.Drawing.Point>();
				var ListRS = Pattern.Match(IsHighSpeed, 0, AngleLower, AngleUper, Common.PropetyTools[IndexThread][Index].Score / 100.0, ckSIMD, ckBitwiseNot, ckSubPixel, NumObject, OverLap, false, -1);
				foreach (Rotaterectangle rot in ListRS)
				{
					PointF pCenter = new PointF((float)rot.Cx, (float)rot.Cy);
					float angle = (float)rot.AngleDeg;
					float width = (float)rot.Width;
					float height = (float)rot.Height;
					float Score = (float)rot.Score;
					rectRotates.Add(new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None, false));
					listScore.Add(Math.Round(Score, 1));
					listP_Center.Add(new System.Drawing.Point((int)rectRotate._PosCenter.X - (int)rectRotate._rect.Width / 2 + (int)pCenter.X, (int)rectRotate._PosCenter.Y - (int)rectRotate._rect.Height / 2 + (int)pCenter.Y));


				}


				
                matProcess.Dispose();
                matCrop.Dispose();

            }

        }

    }
}
