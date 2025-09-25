
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ComponentModel;
using BeeCore.Funtion;
using OpenCvSharp.Extensions;
using Python.Runtime;
using Point = OpenCvSharp.Point;
using System.Linq;
using Size = OpenCvSharp.Size;
using System.IO;
using BeeGlobal;
using static EasyModbus.ModbusServer;
namespace BeeCore
{
  public   class Common
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        public static void HideConsole()
        {
            var handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                ShowWindow(handle, SW_HIDE);
            }
        }
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern IntPtr GetImage(ref int rows, ref int cols, ref int Type);

        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]

        unsafe public static extern IntPtr GetImageCrop( ref int rows, ref int cols, ref int Type );
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]

        unsafe public static extern IntPtr GetImageResult(ref int rows, ref int cols, ref int Type);
        public static readonly object BmLock = new object();  // khóa ngắn khi swap/clone
       
        public static List<Camera> listCamera = new List<Camera> { null, null, null, null };
        public int numError = 0;
        public  bool IsErrorCCD = false;
        public bool Check2Image = false;
        private static int frameRate = 0;
        public static Image ImageShow = new Bitmap(376, 240);
        public static List<List<PropetyTool>> PropetyTools = new List<List<PropetyTool>>();
      

        public static int currentTrig = 0;
       
      //  public static  List <Bitmap> listRaw = new List<Bitmap>();
      //public static Bitmap bmRaw = null;
      //  public static Bitmap bmResult = null;
        public static IntPtr intPtrRaw;
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern void SetSrc( IntPtr data, int image_rows, int image_cols, MatType matType);
        public static bool IsDebug = false;
        public unsafe static IntPtr ArrayToIntPtr(byte[] input)
        {
            IntPtr* ptr = stackalloc IntPtr[1];
            Marshal.Copy(input, 0, (IntPtr)ptr, sizeof(IntPtr));
            return *ptr;
        }
        public static Mat CropRotatedRectSharp(Mat src, RotatedRect rotatedRect)
        {
            // Tính ma trận xoay để đưa rotatedRect về thẳng
            Mat rotationMatrix = Cv2.GetRotationMatrix2D(rotatedRect.Center, rotatedRect.Angle, 1.0);

            // Xoay toàn bộ ảnh
            Mat rotatedImage = new Mat();
            Cv2.WarpAffine(src, rotatedImage, rotationMatrix, src.Size(), InterpolationFlags.Linear, BorderTypes.Replicate);

            // Tính toạ độ vùng crop (đã xoay thẳng)
            Size2f rectSize = rotatedRect.Size;
            Point2f topLeft = new Point2f(
                rotatedRect.Center.X - rectSize.Width / 2,
                rotatedRect.Center.Y - rectSize.Height / 2
            );

            // Crop vùng ảnh từ ảnh đã xoay
            Rect roi = new Rect(
                (int)topLeft.X,
                (int)topLeft.Y,
                (int)rectSize.Width,
                (int)rectSize.Height
            );

            // Đảm bảo ROI nằm trong giới hạn ảnh
            roi = roi.Intersect(new Rect(0, 0, rotatedImage.Width, rotatedImage.Height));

            if (roi.Width <= 0 || roi.Height <= 0)
                return new Mat(); // Trả về ảnh rỗng nếu vùng crop không hợp lệ

            return new Mat(rotatedImage, roi);
        }
       static String er;
        public static bool IsRun=false;
       
        public static TypeCrop TypeCrop = TypeCrop.Area;
        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        public static RectRotate GetPositionAdjustment(RectRotate rotOrigin, RectRotate rotTemp)
        {
            System.Drawing.Size sz = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
            RectRotate rot = new RectRotate();

            rot._rect = rotOrigin._rect;
            rot._rectRotation = rotOrigin._rectRotation + Global.angle_Adjustment;
            PointF pPos = new PointF(rotTemp._PosCenter.X + Global.X_Adjustment, rotTemp._PosCenter.Y + Global.Y_Adjustment);
            double DeltaX = rotOrigin._PosCenter.X - rotTemp._PosCenter.X;
            double DeltaY = rotOrigin._PosCenter.Y - rotTemp._PosCenter.Y;
            int dauX = 1; int dauY = 1;
            if (DeltaX != 0)
                dauX = DeltaX > 0 ? 1 : -1;
            if (DeltaY != 0)
                dauY = DeltaY > 0 ? 1 : -1;

            double angle1 = Math.Atan(Math.Abs(DeltaY / DeltaX)) * 180 / Math.PI;


            //    angle1 = 180 - angle1;
            //else if (DeltaX < 0 && DeltaY > 0)
            //    angle1 = - angle1;
            //if(angle1<0) angle1 = 360 + angle1;
            double distance = GetDistance(rotOrigin._PosCenter.X, rotOrigin._PosCenter.Y, rotTemp._PosCenter.X, rotTemp._PosCenter.Y);
            double angle2 = angle1 - Global.angle_Adjustment;
            if (DeltaX > 0 && DeltaY < 0)
                angle2 = angle1 - Global.angle_Adjustment;
            else if (DeltaX > 0 && DeltaY > 0)
                angle2 = -angle1 - Global.angle_Adjustment;
            else if (DeltaX < 0 && DeltaY < 0)
                angle2 = angle1 + Global.angle_Adjustment;
            // else if (DeltaX < 0 && DeltaY > 0)
            //     angle2 = -angle1- G.angle_Adjustment;
            double cos1 = Math.Cos((angle2) * Math.PI / 180);
            double sin1 = Math.Sin((angle2) * Math.PI / 180);
            double DeltaX1 = distance * cos1;
            double DeltaY1 = distance * sin1;

            if (DeltaX > 0 && DeltaY > 0)
                DeltaY1 = -DeltaY1;
            // else if (DeltaX < 0 && DeltaY < 0)
            //DeltaY1 = -DeltaY1;
            rot._PosCenter = new PointF(pPos.X + (float)DeltaX1 * dauX, pPos.Y + (float)DeltaY1 * dauY);

            return rot;
        }
        public static Comunication Comunication=new Comunication();
        public static void IniPython()
        {

            try
            {
                //string pyHome = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib");
                //Environment.SetEnvironmentVariable("PYTHONHOME", pyHome);
                //Environment.SetEnvironmentVariable("PYTHONPATH",
                //    $"{pyHome}\\Lib;{pyHome}\\site-packages");


                //string pythonDll = Path.Combine(pyHome, "python39.dll");

                string pythonHome = Environment.GetEnvironmentVariable("Python39");
                if (!string.IsNullOrEmpty(pythonHome))
                {
                    string pythonDll = Path.Combine(pythonHome, "python39.dll");
                    if (File.Exists(pythonDll))
                    {
                        Python.Runtime.Runtime.PythonDLL = pythonDll;
                        //var pythonDll = Path.Combine("C:\\Program Files\\Python312", "python312.dll");
                        Runtime.PythonDLL = pythonDll;
                        HideConsole();
                        //string pyHome = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib");
                        //Environment.SetEnvironmentVariable("PYTHONHOME", pyHome);
                        //Environment.SetEnvironmentVariable("PYTHONPATH",
                        //    $"{pyHome}\\Lib;{pyHome}\\site-packages");
                        //   string pythonDll = Path.Combine(pyHome, "python39.dll");
                        //Runtime.PythonDLL = pythonDll;
                        PythonEngine.Initialize();
                        PythonEngine.BeginAllowThreads();

                        using (Py.GIL())
                        {


                           
                            if(Global.IsLearning)
                            {
                                G.np = Py.Import("numpy");
                                dynamic mod = Py.Import("Tool.Learning");
                                dynamic cls = mod.GetAttr("ObjectDetector"); // class
                                G.objYolo = cls.Invoke();              // khởi tạo instance
                            }
                            if (Global.IsOCR)
                            {
                                dynamic mod2 = Py.Import("Tool.OCR");
                                dynamic cls2 = mod2.GetAttr("OCR"); // class
                                G.objOCR = cls2.Invoke();              // khởi tạo instance
                                dynamic mod3 = Py.Import("Tool.Classic");
                                dynamic cls3 = mod3.GetAttr("Filter"); // class
                                G.Classic = cls3.Invoke();              // khởi tạo instance
                                G.IniEdge = true;
                                // khởi tạo instance
                                G.Classic.LoadEdge();
                            }


                            Global.IsIntialPython = true;


                        }

                    }
                }
            }
            catch (PythonException ex)
            {
                 er=ex.Message;
                String a = "b";
                MessageBox.Show(ex.Message);
            }
        }
        public static void ClosePython()
        {
           // PythonEngine.Shutdown();   
        }

        public static Mat AutoCanny(Mat grayImage, double sigma = 0.33)
        {
            // Lấy toàn bộ byte ảnh ra mảng 1 chiều
            byte[] pixels = new byte[grayImage.Rows * grayImage.Cols * grayImage.ElemSize()];
            Marshal.Copy(grayImage.Data, pixels, 0, pixels.Length);

            // Tính trung vị
            byte[] sorted = pixels.OrderBy(p => p).ToArray();
            byte median = sorted[sorted.Length / 2];

            // Tính ngưỡng dưới và ngưỡng trên
            int lower = Math.Max(0, (int)((1.0 - sigma) * median));
            int upper = Math.Min(255, (int)((1.0 + sigma) * median));

            // Áp dụng Canny
            Mat edges = new Mat();
            Cv2.Canny(grayImage, edges, lower, upper);
            return edges;
        }
        // Canny + Morph gradient
        public static Mat CannyWithMorph(Mat gray)
        {
            Mat canny = AutoCanny(gray);
            Mat morph = new Mat();
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.MorphologyEx(canny, morph, MorphTypes.Gradient, kernel);
            return morph;
        }
        //  [DllImport(@".\BeeCam.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        //  unsafe public static extern bool ScanCCD(int index);
        //static  String s;
        //  public static void Scan()
        //  {

        //      bool result = ScanCCD(0);

        //      //MessageBox.Show(result.ToString());
        //  }

       
        public static void CropRotate( RectRotate rot)
        {
            G.CommonPlus.CropRotate((int)rot._PosCenter.X, (int)rot._PosCenter.Y, (int)rot._rect.Width, (int)rot._rect.Height, rot._angle);
        }
       
        public static Mat GetCrop()
        {

            int rows = 0, cols = 0, Type = 0;
            IntPtr intPtr = GetImageCrop(ref rows, ref cols, ref Type);
            try
            {
                unsafe
                {

                    Mat raw = new Mat(rows, cols, Type, intPtr);
                    return raw;
                }

            }
            finally
            {
                // Giải phóng bộ nhớ sau khi sử dụng
                Marshal.FreeHGlobal(intPtr);
            }


           
        }
      
     
        public static event PropertyChangedEventHandler FrameChanged;
        public static event PropertyChangedEventHandler PropertyChanged;
        public static void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(G.Common, new PropertyChangedEventArgs(propertyName));
            }
        }
        public static bool IsLive = false;
        //public static void GetImgeTinyCam(Bitmap bmp)
        //{
        //    try
        //    {
        //        if (bmp != null)
        //        {
                  
                       
        //                    matRaw = BitmapConverter.ToMat(bmp).Clone();
        //                    //matRaw = raw.Clone();
        //                    //Cv2.CvtColor(raw.Clone(), matRaw, ColorConversionCodes.BGRA2BGR);
        //                    NotifyPropertyChanged("Image");
                        
                    
                    
        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }
        //       }
        public static float CycleCamera = 0;
       
        public static double StepExposure=1,MinExposure=1,MaxExposure=100;
        //public static bool ConnectCCD( int indexCCD,String Resolution)
        //{
        //    //String[] sp = Resolution.Split(' ');
        //    //String[] sp2 = sp[0].Split('x');

        //    BeeCore.G.CCD.colCCD = Convert.ToInt32(sp2[0]);
        //    BeeCore.G.CCD.rowCCD =Convert.ToInt32( sp2[1]); 
          
        //    if (G.CCD.Connect(Convert.ToInt32(sp2[1]), Convert.ToInt32(sp2[0]),indexCCD))
        //    {
        //        G.CCD.ReadCCD();
        //        StepExposure = G.CCD.StepExposure;
        //        MinExposure = G.CCD.MinExposure;
        //        MaxExposure=G.CCD.MaxExposure;
        //        if(Global.ParaCommon._Exposure != 0)
        //     G.CCD.Exposure =  Global.ParaCommon._Exposure;
        //        Cycle = G.CCD.cycle;
        //        G.CCD.SetPara();
        //        ///G.CommonPlus.GetImageRaw();
        //        return true;
        //    }
        //    return false;
             
        //}
       
       

        public static int FrameRate { get => frameRate; set
            {
                frameRate = value;
                if (FrameChanged != null)
                {
                    FrameChanged(G.Common, new PropertyChangedEventArgs("FrameChanged"));
                }
            }
        }

    

       
        //public static void ReadCCD(bool IsSimulation, TypeCamera typeCamera)
        //{
        //    if(IsSimulation)
        //    {
        //        G.CommonPlus.BitmapSrc(BeeCore.Common.matRaw.Clone().ToBitmap());
        //        IsSimulation=false;
        //        return;
        //    }    
           
        //    switch (typeCamera)
        //    {
        //        case TypeCamera.USB:
        //    //if (IsHist)
        //    //    G.CCD.ReadRaw(true);
        //    //else
        //        G.CCD.ReadCCD();
        //            break;
        //        case TypeCamera.MVS:
        //            //if (IsHist)
        //            //    G.CCD.ReadRaw(true);
        //            //else
        //            G.CCD.ReadCCD();
        //            break;
        //        case TypeCamera.TinyIV:
                    
        //            SetRaw();
        //                break;
        //}
          
        //}
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        public static Mat CropRotatedRect(Mat source, RectRotate rot)
        {

            MatType TypeMat = source.Type();
            Mat matResult = new Mat();

            Point2f pCenter = new Point2f(rot._PosCenter.X, rot._PosCenter.Y);
            Size2f rect_size = new Size2f(rot._rect.Size.Width, rot._rect.Size.Height);
            RotatedRect rot2 = new RotatedRect(pCenter, rect_size, rot._rectRotation);
            double angle = rot._rectRotation;
            if (angle < -45)
            {
                angle += 90.0;

                Swap(ref rect_size.Width, ref rect_size.Height);
            }
            InputArray M = Cv2.GetRotationMatrix2D(rot2.Center, angle, 1.0);
            Mat mCrop = new Mat();
            Mat crop1 = new Mat();
            try
            {
               
                Cv2.WarpAffine(source, crop1, M, source.Size(), InterpolationFlags.Cubic);

                Cv2.GetRectSubPix(crop1, new OpenCvSharp.Size(rect_size.Width, rect_size.Height), rot2.Center, mCrop);
               
            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.Message);
            }
            return mCrop;
        }
        public static Mat CropRotatedRect(Mat source, RectRotate rot, RectRotate rotMask)
        {
            MatType TypeMat = source.Type();
            Mat matResult = new Mat();
          
            Point2f pCenter = new Point2f(rot._PosCenter.X, rot._PosCenter.Y);
            Size2f rect_size = new Size2f(rot._rect.Size.Width, rot._rect.Size.Height);
            RotatedRect rot2 = new RotatedRect(pCenter, rect_size, rot._rectRotation);
            double angle = rot._rectRotation;
            if (angle < -45)
            {
                angle += 90.0;

                Swap(ref rect_size.Width, ref rect_size.Height);
            }



            InputArray M = Cv2.GetRotationMatrix2D(rot2.Center, angle, 1.0);

            Mat crop1 = new Mat();
            try
            {
                Mat mCrop = new Mat();
                Cv2.WarpAffine(source, crop1, M, source.Size(), InterpolationFlags.Cubic);

                Cv2.GetRectSubPix(crop1, new OpenCvSharp.Size(rect_size.Width, rect_size.Height), rot2.Center, mCrop);
                //if (TypeMat == MatType.CV_8UC3)
                //{
                //    Cv2.CvtColor(mCrop, mCrop, ColorConversionCodes.BGR2GRAY);
                //}
              
                if (rot.IsElip)
                {
                    Mat matMask = new Mat((int)rot._rect.Height, (int)rot._rect.Width, TypeMat, new Scalar(0));
                    int deltaX = (int)rot._rect.Width / 2;
                    int deltaY = (int)rot._rect.Height / 2;
                    RotatedRect rectElip = new RotatedRect(new Point2f(deltaX, deltaY), new Size2f(rot._rect.Width, rot._rect.Height), rot._rectRotation);
                    Cv2.Ellipse(matMask, rectElip, new Scalar(255), -1);
                    Cv2.BitwiseAnd(mCrop, matMask, matResult);
                }
                else
                    matResult = mCrop;
              //  return matResult;
            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message);
            }
            if(rotMask != null)
            {
               Mat matMask=new Mat((int)rot._rect.Height, (int)rot._rect.Width, TypeMat, new Scalar(255));
                int deltaX = (int)rot._rect.Width / 2 - (int)(rot._PosCenter.X - rotMask._PosCenter.X);
                int deltaY = (int)rot._rect.Height / 2 - (int)(rot._PosCenter.Y - rotMask._PosCenter.Y);
                RotatedRect retMask = new RotatedRect(new Point2f(deltaX, deltaY), new Size2f(rotMask._rect.Width, rotMask._rect.Height), rotMask._rectRotation);

                if (rotMask.IsElip)
                {
                    Cv2.Ellipse(matMask, retMask,new Scalar(0), -1);  
                }
                else
                {
                    // Lấy ra các điểm góc sau khi xoay
                    Point2f[] vertices = new Point2f[4];
                    vertices= retMask.Points( );

                    // Chuyển Point2f sang Point để vẽ
                    Point[] pts = Array.ConvertAll(vertices, v => new Point((int)v.X, (int)v.Y));

                    // Tô hình chữ nhật xoay (dùng fillPoly)
                    Cv2.FillPoly(matMask, new[] { pts }, new Scalar(0)); // Đỏ

                }
                //if (matResult.Type() == MatType.CV_8UC3)
                //{
                //    Cv2.CvtColor(matResult, matResult, ColorConversionCodes.BGR2GRAY);

                //}
                Mat matAnd = new Mat();
                    Cv2.BitwiseAnd(matResult, matMask, matAnd);
               
                    return matAnd;
                
            }
            return matResult;
        }

      
        public static Mat LoadImage(string path, ImreadModes mode)
        { Mat raw = OpenCvSharp.Cv2.ImRead(path, mode);

       
            
            return raw;
        }
    }
}
