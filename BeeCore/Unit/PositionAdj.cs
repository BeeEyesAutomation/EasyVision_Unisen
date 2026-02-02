using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static BeeCore.Cropper;
using static LibUsbDotNet.Main.UsbTransferQueue;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;
namespace BeeCore
{
    [Serializable()]
    public class PositionAdj
    {
        [NonSerialized]
        public Pattern Pattern=new Pattern();
        public PositionAdj()
        {


        }
        [NonSerialized]
        public bool IsNew = false;
        public void SetModel()
        {
            if (Pattern == null)
            {
                Pattern = new Pattern();
              
            }
            if (bmRaw != null)
            {
                matTemp = bmRaw.ToMat();
                LearnPattern(matTemp, true);
            }
            rotMask = null;
            if (rotArea == null) rotArea = new RectRotate();
            if (rotCrop == null) rotCrop = new RectRotate();
            DetectIntersect = new DetectIntersect();
            Common.PropetyTools[IndexThread][Index].StepValue = 1;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 100;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsIni = false;
        public int Index = -1;
        public MethodSample MethodSample = new MethodSample();
        public RectRotate rotArea, rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public Bitmap bmRaw;
        [NonSerialized]
        public Mat matTemp;
        public List<Point> Postion = new List<Point>();
        
        public List<double> listScore = new List<double>();
        public int IndexCCD = 0;
        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public int ThresholdBinary;
        public int MaxLineCandidates = 4;
        public double RansacThreshold = 2.0; // px
        public int RansacIterations = 200;
        public bool IsClose = false;
        public bool IsOpen = false;
        public bool IsClearNoiseBig = false;
        public bool IsClearNoiseSmall = false;
        public int SizeClearsmall = 1;
        public int SizeClearBig = 1;
        public int SizeClose = 1;
        public int SizeOpen = 1;

        public int MaxObject = 1;
        public int StepAngle = 1;
        bool isHighSpeed = false;
        public bool IsHighSpeed
        {
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


        public bool AutoMean = true;
        public int FixMean = 3;
        public float ContinuityGapFactor = 1.2f;
        public int AngleTargetDeg = 90;
        public int AngleToleranceDeg = 10;
        public int MaximumLine = 20;

        public float MinInliersA = 5;
        public float MinInliersB = 5;
        private double _angle = 10;
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

     
        bool _ckSIMD = true;
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
        public void ReSetAngle()
        {
            if (Angle > 360) Angle = 360;
            if (Angle == 0)
            {
                Angle = 1;
            }
            float angle = (rotCrop._rectRotation) - (rotArea._rectRotation);
            AngleLower = angle - Angle;
            AngleUper = angle + Angle;
        }
        public  RectRotate rotOriginAdj;
        private bool isAutoTrig;
        private int numOK;
        private int delayTrig;
        public List<System.Drawing.Point> listP_Center = new List<System.Drawing.Point>();
        public List<float> list_AngleCenter = new List<float>();
        public static void LoadEdge()
        {
            if (G.IniEdge) return;

        }
        public static Mat RotateRegionLocal(Mat src, RotatedRect rr, double addAngleDeg,
                                        InterpolationFlags interp = InterpolationFlags.Linear,
                                        BorderTypes border = BorderTypes.Replicate)
        {
            if (src == null || src.Empty()) throw new ArgumentException("src empty");

            // 1) Lấy bounding box của rr và clamp vào ảnh
            Rect box = rr.BoundingRect();
            box = new Rect(
                Math.Max(0, box.X),
                Math.Max(0, box.Y),
                Math.Min(box.Width, src.Cols - Math.Max(0, box.X)),
                Math.Min(box.Height, src.Rows - Math.Max(0, box.Y))
            );
            if (box.Width <= 1 || box.Height <= 1) return src.Clone();

            // 2) ROI nguồn + mask RR trong tọa độ ROI
            Mat roiSrc = new Mat(src, box);
            Mat mask = Mat.Zeros(box.Size, MatType.CV_8UC1);

            // rr points chuyển về toạ độ ROI
            Point2f[] pf = rr.Points();
            Point[] p = new Point[4];
            for (int i = 0; i < 4; i++)
                p[i] = new Point(
                    (int)Math.Round(pf[i].X - box.X),
                    (int)Math.Round(pf[i].Y - box.Y)
                );
            Cv2.FillConvexPoly(mask, p, Scalar.All(255), LineTypes.AntiAlias);

            // 3) Xoay ROI quanh tâm cục bộ (tâm RR trừ toạ độ box)
            Point2f centerLocal = new Point2f(rr.Center.X - box.X, rr.Center.Y - box.Y);
            Mat M = Cv2.GetRotationMatrix2D(centerLocal, addAngleDeg, 1.0);

            Mat roiRot = new Mat();
            Cv2.WarpAffine(roiSrc, roiRot, M, roiSrc.Size(), interp, border, Scalar.All(0));

            // 4) Dán ROI đã xoay lên ảnh gốc chỉ trong vùng mask
            Mat dst = src.Clone();
            roiRot.CopyTo(new Mat(dst, box), mask);
            return dst;
        }
        public Mat LearnPattern(Mat raw, bool IsNoCrop)
        {

            using (Mat img = raw.Clone())
            {
                // Chuẩn hóa góc
                //if (rotCrop._rectRotation < 0)
                //    rotCrop._rectRotation += 360;

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
                   


                       var rrCli = Converts.ToCli(rotCrop); // như ở reply trước
                        RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                        intpr = Pattern.SetImgeSample(img.Data, img.Width, img.Height, (int)img.Step(), img.Channels(), rrCli, rrMaskCli, IsNoCrop,
                                out w, out h, out s, out c);

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
                        Pattern.FreeBuffer(intpr); // rất quan trọng
                }

                return mat;
            }





        }
   
        public bool IsSendResult;
        public String AddPLC;
        public async Task SendResult()
        {
            if (IsSendResult)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                    int i = 0;
                    int Add = (int)Converts.StringtoDouble(AddPLC);
                    String sAdd = Converts.BeforeFirstDigit(AddPLC);
                    foreach (System.Drawing.Point point in listP_Center)
                    {
                        String Address = sAdd + Add;
                        float[] floats = new float[4] { point.X, point.Y, list_AngleCenter[i], (float)listScore[i] };
                        await Global.Comunication.Protocol.WriteResultFloatArr(AddPLC, floats);
                        Add += 8;
                        i++;
                    }
                }
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
      

        //IsProcess,Convert.ToBoolean((int) TypeMode)
        public List<RectRotate> rectRotates = new List<RectRotate>();


        public bool IsLimitCouter = true;
        [NonSerialized]
        DetectIntersect DetectIntersect = new DetectIntersect();
        [NonSerialized]
        CornerResult Result = new CornerResult();
        [NonSerialized]
      Mat   matProcess =new Mat();
        public bool IsCalib = false;
        public SideLR SideLR=SideLR.Left;
        public SideTB SideTB = SideTB.Above;
        public SideLR SideTempLR = SideLR.Left;
        public SideTB SideTempTB = SideTB.Above;
        [NonSerialized]
        private Line2D LineVertial,LineHorial;
        [NonSerialized]
        private Line2DCli LineCliHorial, LineCliVertical;
        public PointF pInsert = new PointF();
        public float AspectLen = 0.6f;
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
       
             rotArea = rotArea; // <-- gán này không tác dụng ra ngoài, bỏ đi

            rectRotates = new List<RectRotate>();
            listScore = new List<double>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            Common.PropetyTools[Global.IndexChoose][Index].ScoreResult = 0;
            matProcess=new Mat();
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                if (raw.Empty()) 
                    return;

                Mat gray = null;
                try
                {
                    if (raw.Type() == MatType.CV_8UC3)
                    {
                        gray = new Mat();
                        Cv2.CvtColor(raw, gray, ColorConversionCodes.BGR2GRAY);
                    }
                    else
                    {
                        gray = raw; // reuse backing store (ref-counted)
                    }

                    float scoreSum = 0f;

                    switch (MethodSample)
                    {
                        case MethodSample.Pattern:
                            {

                                var rrCli = Converts.ToCli(rotArea); // như ở reply trước
                                RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
                                Pattern.SetImgeRaw(gray.Data, gray.Width, gray.Height, (int)gray.Step(), gray.Channels(), rrCli, rrMaskCli);
                                //var opt = new PatternMatchOptions
                                //{
                                //    EnableValidate = true,

                                //    EnableScoreStability = false,     // Cách 1

                                //    EnableMinEdge = true,
                                //    MinEdgeRatio = 0.60,             // 0.55–0.70

                                //    EnableSoftAntiText = true,
                                //    EntropyWeight = 0.60,            // 0.40–0.80

                                //    EnableMultiScaleCheck = true,
                                //    MaxScaleScoreDiff = 12.0,        // nếu struct có field này
                                //};
                                //Pattern.SetMatchOptions(opt);
                                var listRS = Pattern.Match(
                                    IsHighSpeed, StepAngle,
                                    AngleLower, AngleUper,
                                    Common.PropetyTools[IndexThread][Index].Score / 100.0,
                                    ckSIMD, ckBitwiseNot, ckSubPixel,
                                    MaxObject, OverLap, false, -1);
                                float ScoreMax = 0;
                                if (listRS.Count > 0)
                                {
                                    rectRotates.Add(new RectRotate());
                                    listScore.Add(0);
                                    listP_Center.Add(new System.Drawing.Point());
                                    foreach (Rotaterectangle rot in listRS)
                                    {
                                        var pCenter = new System.Drawing.PointF((float)rot.Cx, (float)rot.Cy);
                                        float angle = (float)rot.AngleDeg;

                                        float width = (float)rot.Width;
                                        float height = (float)rot.Height;
                                        float score = (float)rot.Score;
                                        if (score > ScoreMax)
                                        {
                                            ScoreMax = score;

                                            rectRotates[0] = new RectRotate(
                                                new System.Drawing.RectangleF(-width / 2f, -height / 2f, width, height),
                                                pCenter, angle, AnchorPoint.None);

                                            listScore[0] = Math.Round(score, 1);
                                            list_AngleCenter.Add(rotArea._rectRotation +angle);
                                            listP_Center[0] = new System.Drawing.Point(
                                                (int)(rotArea._PosCenter.X - rotArea._rect.Width / 2f + pCenter.X),
                                                (int)(rotArea._PosCenter.Y - rotArea._rect.Height / 2f + pCenter.Y));
                                        }
                                    }
                                    scoreSum = ScoreMax;
                                }
                                break;
                            }

                        case MethodSample.Corner:
                            {
                                // Crop theo vùng làm việc hiện tại (nếu bạn thực sự muốn dùng rotArea, giữ như dưới;
                                // nếu muốn dùng đúng tham số rotArea, thay 'rotArea' bằng 'rotArea')
                                using (Mat matCrop = Cropper.CropRotatedRect(gray, rotArea, rotMask))
                                {
                                    if (matProcess == null) matProcess = new Mat();
                                    if (!matProcess.IsDisposed)
                                        if (!matProcess.Empty()) matProcess.Dispose();
                                   
                                    try
                                    {
                                        
                                     
                                        try
                                        {
                                            switch (MethordEdge)
                                            {
                                                case MethordEdge.CloseEdges:
                                                    matProcess = Filters.Edge(matCrop);
                                                    break;
                                                case MethordEdge.StrongEdges:
                                                    matProcess = Filters.GetStrongEdgesOnly(matCrop);
                                                    break;
                                                case MethordEdge.Binary:
                                                    matProcess = Filters.Threshold(matCrop, ThresholdBinary, ThresholdTypes.Binary);
                                                    break;
                                                case MethordEdge.InvertBinary:
                                                    matProcess = Filters.Threshold(matCrop, ThresholdBinary, ThresholdTypes.BinaryInv);
                                                    break;
                                                default:
                                                    matProcess = matCrop; // fallback
                                                    break;
                                            }

                                            // Hậu xử lý hình thái học / khử nhiễu (mỗi hàm có thể trả Mat mới → nhớ dispose cái cũ)
                                            if (IsClearNoiseSmall)
                                            {
                                                Mat t = Filters.ClearNoise(matProcess, SizeClearsmall);
                                                if (!object.ReferenceEquals(t, matProcess)) matProcess.Dispose();
                                                matProcess = t;
                                            }
                                            if (IsClose)
                                            {
                                                Mat t = Filters.Morphology(matProcess, MorphTypes.Close, new OpenCvSharp.Size(SizeClose, SizeClose));
                                                if (!object.ReferenceEquals(t, matProcess)) matProcess.Dispose();
                                                matProcess = t;
                                            }
                                            if (IsOpen)
                                            {
                                                Mat t = Filters.Morphology(matProcess, MorphTypes.Open, new OpenCvSharp.Size(SizeOpen, SizeOpen));
                                                if (!object.ReferenceEquals(t, matProcess)) matProcess.Dispose();
                                                matProcess = t;
                                            }
                                            if (IsClearNoiseBig)
                                            {
                                                Mat t = Filters.ClearNoise(matProcess, SizeClearBig);
                                                if (!object.ReferenceEquals(t, matProcess)) matProcess.Dispose();
                                                matProcess = t;
                                            }
                                            if (IsCalib)
                                            {
                                                MinInliersA = 2;
                                                MinInliersB = 2;
                                            }
                                            
                                            //OrthCornerOptions orthCornerOptions = new OrthCornerOptions
                                            //{
                                            //    MaxCandidateLines = MaximumLine,
                                            //    RansacIterations = RansacIterations,
                                            //    RansacThreshold = RansacThreshold,
                                            //    MinInlierA = MinInliersA,
                                            //    MinInlierB = MinInliersB,
                                            //    AutoMean = AutoMean,
                                            //    FixMean = FixMean,
                                            //    AngleTargetDeg = AngleTargetDeg,
                                            //    AngleToleranceDeg = AngleToleranceDeg,
                                            //    ContinuityGapFactor = ContinuityGapFactor,
                                            //};
                                            try
                                            {
                                                AspectLen = 0.01f;
                                                 LineCliHorial = RansacLine.FindBestLine(
                                               matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                                               iterations: RansacIterations,
                                               threshold: (float)RansacThreshold,
                                               maxPoints: 120000,
                                               seed: Index,
                                               mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Horizontal, LineScanMode.TopToBottom, 0, AngleToleranceDeg
                                                );
                                                LineCliVertical = RansacLine.FindBestLine(
                                                 matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
                                                 iterations: RansacIterations,
                                                 threshold: (float)RansacThreshold,
                                                 maxPoints: 120000,
                                                 seed: Index,
                                                 mmPerPixel: 1, AspectLen, BeeCpp.LineDirectionMode.Vertical, LineScanMode.RightToLeft, 0, AngleToleranceDeg
                                                  );
                                              
                                                // Result = DetectIntersect.FindBestCorner_RansacRuns(matCrop, matProcess, orthCornerOptions);

                                                //  Cv2.ImWrite("RS.png", Result.Debug);
                                                if (LineCliHorial.Found&&LineCliVertical.Found)
                                                {
                                                   
                                                    LineHorial = new Line2D(LineCliHorial.Vx, LineCliHorial.Vy, LineCliHorial.X0, LineCliHorial.Y0);
                                                    LineVertial = new Line2D(LineCliVertical.Vx, LineCliVertical.Vy, LineCliVertical.X0, LineCliVertical.Y0);
                                                    pInsert = InsertLine.Intersect(LineHorial, LineVertial);

                                                    // Kết quả vẽ 1 rect bé tại góc phát hiện
                                                    int width1 = 10, height1 = 10;
                                                    float angle1 = (float)InsertLine.GetAngleAndSide(LineHorial, LineVertial, pInsert, out SideLR, out SideTB);
                                                    //   angle1 = 270f - angle1;

                                                    scoreSum = 100f;
                                                    PointF pCenter = new System.Drawing.PointF(pInsert.X, pInsert.Y);
                                                    rectRotates = new List<RectRotate>
                                            {
                                                new RectRotate(
                                                    new System.Drawing.RectangleF(-width1 / 2f, -height1 / 2f, width1, height1),pCenter
                                                   ,
                                                    angle1, AnchorPoint.None)
                                            };
                                                    listP_Center.Add(new System.Drawing.Point(
                                       (int)(rotArea._PosCenter.X - rotArea._rect.Width / 2f + pCenter.X),
                                       (int)(rotArea._PosCenter.Y - rotArea._rect.Height / 2f + pCenter.Y)));
                                                    list_AngleCenter.Add(rotArea._rectRotation + angle1);


                                                }
                                                else
                                                {
                                                    rectRotates = new List<RectRotate>();
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.ToString());
                                            }
                                           
                                        }
                                        catch(Exception ex)
                                        {
                                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Position Adj", ex.Message));
                                        }
                                        finally
                                        {
                                            // Giải phóng pipeline mat
                                            // Chỉ dispose nếu nó khác với work
                                            // (nếu Filters trả về cùng instance thì ReferenceEquals sẽ true)
                                            // Ở đây đã dispose từng bước khi thay thế, nên chỉ cần:
                                            // nothing else to do
                                        }
                                    }
                                    finally
                                    {
                                        // Nếu work là matCrop thì không dispose ở đây, vì matCrop đã trong using
                                        // Nếu work là Mat mới (gray từ BGR), dispose nó:
                                        //if (matCrop != null )
                                        //    matCrop.Dispose();
                                    }
                                }
                                break;
                            }
                    }

                    if (scoreSum != 0f && rectRotates.Count > 0)
                    {
                        Common.PropetyTools[Global.IndexChoose][Index].ScoreResult =
                            (int)Math.Round(scoreSum / rectRotates.Count, 1);
                    }
                }
                catch( Exception ex)
                {
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Position Adj", ex.Message));

                }
                finally
                {
                    // Nếu gray trỏ shared tới raw thì không dispose (đã dispose qua using của raw).
                    if (gray != null && !object.ReferenceEquals(gray, raw))
                        gray.Dispose();
                }
            }
        }

      
        public void Complete()
        {
            Results results = Results.None;
            results = Results.OK;
            if (rectRotates.Count() != 1)
                results = Results.NG;
            if(!Global.IsAutoTemp)
            if(MethodSample == MethodSample.Corner)
           if(LineCliVertical.Inliers< MinInliersA|| LineCliHorial.Inliers< MinInliersB)
                results = Results.NG;
            if (results == Results.OK)
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
                Global.AngleOrigin = rectRotates[0]._rectRotation;
                Global.pOrigin = new OpenCvSharp.Point(x, y);

            }
            if (MethodSample == MethodSample.Corner)
                if (IsCalib)
            {
                SideTempLR = SideLR;
                SideTempTB = SideTB;
                MinInliersA = (float)(LineCliVertical.Inliers * (60 / 100.0));
               MinInliersB = (float)(LineCliHorial.Inliers * (60 / 100.0));



            }
            if (MethodSample == MethodSample.Corner)
                if (SideTempLR != SideLR || SideTempTB != SideTB)
                results = Results.NG;
            if (!Global.IsRun || Global.IsAutoTemp)
             {
             
                Global.StatusDraw = StatusDraw.Check;
                    if (results == Results.OK)
                    {
                        rotPositionAdjustment = rectRotates[0].Clone();
                    // Global.rotAreaAdj = rotArea.Clone();
                    //   Global.rotOriginAdj = rectRotates[0].Clone();

                    rotOriginAdj = new RectRotate(rotCrop._rect, new PointF(rotArea._PosCenter.X - rotArea._rect.Width / 2 + rotPositionAdjustment._PosCenter.X, rotArea._PosCenter.Y - rotArea._rect.Height / 2 + rotPositionAdjustment._PosCenter.Y), rotPositionAdjustment._rectRotation, AnchorPoint.None);
                    }
              }
            //else
            //{
            //    if (results == Results.OK)
            //    {
            //        Global.rotCurrentAdj = rectRotates[0].Clone();

            //    }
            //}
              
                Common.PropetyTools[IndexThread][Index].Results = results;
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
            Brush brushText =new SolidBrush( Global.ParaShow.TextColor);
            Color cl = Color.LimeGreen;

            if (Common.PropetyTools[Global.IndexChoose][Index].Results == Results.NG)
            {
                cl = Global.ParaShow.ColorNG;
            }
            else
            {
                cl =  Global.ParaShow.ColorOK;
            }
            String nameTool = (int)(Index + 1) + "." + BeeCore.Common.PropetyTools[IndexThread][Index].Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            if (Global.ParaShow.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl,  Global.ParaShow.ThicknessLine);

            if (!Global.IsRun && Global.ParaShow.IsShowMatProcess||Global.IsRun&&Global.ParaShow.IsShowDetail)
            {
                if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rotA, Global.ScaleZoom * 100, Global.pScroll, cl, Global.ParaShow.Opacity / 100.0f);
            }

            if (MethodSample == MethodSample.Corner )
            {
               
                mat.Translate(rotA._rect.X, rotA._rect.Y);
           
                gc.Transform = mat;
                 gc.DrawLine(new Pen(new SolidBrush(Global.ParaShow.ColorInfor), Global.ParaShow.ThicknessLine), LineCliHorial.X1, LineCliHorial.Y1, LineCliHorial.X2, LineCliHorial.Y2);
                gc.DrawLine(new Pen(new SolidBrush(Global.ParaShow.ColorInfor), Global.ParaShow.ThicknessLine), LineCliVertical.X1, LineCliVertical.Y1, LineCliVertical.X2, LineCliVertical.Y2);
                //Draws.DrawInfiniteLine(gc,LineHorial, new Pen(Brushes.Blue, 2), rotA._rect);
                //Draws.DrawInfiniteLine(gc, LineVertial, new Pen(Brushes.Gold, 2), rotA._rect);
                Draws.Plus(gc,(int) pInsert.X,(int)pInsert.Y, 10, Color.Yellow, 2);
                //var flags = DrawFlags.None;
                //if (Global.IsRun)
                //    flags = DrawFlags.BestCorner | DrawFlags.BestLines;
                //else
                //{
                //    if (Global.ParaShow.IsShowDetail) flags = flags | DrawFlags.Inliers;
                //    if (Global.ParaShow.IsShowNotMatching) flags = flags | DrawFlags.RansacRejected | DrawFlags.Runs;
                //    if (Global.ParaShow.IsShowResult) flags = flags | DrawFlags.BestCorner | DrawFlags.BestLines;
                //}

                //DrawStyle drawStyle = new DrawStyle
                //    {
                //        Inlier = Color.Red,
                //    LineChoose = Global.ParaShow.ColorChoose,
                //    LineResult = Global.ParaShow.ColorInfor,
                //    LineNone = Global.ParaShow.ColorNone,

                //        LineDash = DashStyle.Solid,
                //        InlierSize = Global.ParaShow.ThicknessLine / 2,
                //        Thickness = Global.ParaShow.ThicknessLine,
                //    };

                //DetectIntersect.RenderDebugToGraphics(gc,new RectangleF(0,0, rotA._rect.Width,rotA._rect.Height), DetectIntersect.LineEdge, flags, drawStyle);


            }

            if (MethodSample == MethodSample.Pattern)
            {
                gc.ResetTransform();
            if (listScore == null) return gc;
                if (listScore.Count != rectRotates.Count) return gc;
                if (rectRotates.Count > 0)
            {
                int i = 0;
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

                        if (Global.ParaShow.IsShowPostion)
                        {
                            int min = (int)Math.Min(rot._rect.Width / 4, rot._rect.Height / 4);
                            Draws.Plus(gc, 0, 0, min, cl, Global.ParaShow.ThicknessLine);
                            String sPos = "X,Y,A - " + listP_Center[i ].X + "," + listP_Center[ i].Y + " , " + Math.Round(list_AngleCenter[i ], 1);
                            gc.DrawString(sPos, font, brushText, new PointF(5, 5));

                        }
                        Draws.Box2Label(gc, rot._rect, "", Math.Round(listScore[i], 1) + "%", font, cl, brushText, Global.ParaShow.FontSize, Global.ParaShow.ThicknessLine);

                    }
                  
                        gc.ResetTransform();
                    i++;
                }
            }



            return gc;
        }

        public int IndexThread;
      

    }
}
