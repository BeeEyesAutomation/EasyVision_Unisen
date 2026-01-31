
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

namespace BeeCore
{
  public   class Common
    {
        public static RectRotate TransformToolRect(
        RectRotate cropRect,            // RectRotate vùng Position Adj (dùng để crop + warpAffine)
        RectRotate sampleOriginCrop,    // object mẫu trong ảnh crop (teach)  - toạ độ pixel trong crop
        RectRotate sampleCurrentCrop,   // object hiện tại trong ảnh crop (runtime)
        RectRotate toolRectMaster       // RectRotate tool khác trong master (lúc teach)
    )
        {
            // 1) Bounding box của cropRect trong master
            RectangleF cropBox = GetBoundingBox(cropRect);

            // 2) Tâm object trong master (pivot)
            //    => dùng offset cropBox + toạ độ pixel trong crop
            PointF pivotOrigin = new PointF(
                cropBox.X + sampleOriginCrop._PosCenter.X,
                cropBox.Y + sampleOriginCrop._PosCenter.Y
            );

            PointF pivotCurrent = new PointF(
                cropBox.X + sampleCurrentCrop._PosCenter.X,
                cropBox.Y + sampleCurrentCrop._PosCenter.Y
            );

            // 3) Độ xoay của object (delta góc)
            //    Tuỳ chiều góc của VisualMatch:
            //    - Nếu thấy xoay ngược chiều, đổi dấu như dòng comment dưới.
            float dAngle = sampleCurrentCrop._rectRotation - sampleOriginCrop._rectRotation;
            // float dAngle = sampleOriginCrop._rectRotation - sampleCurrentCrop._rectRotation; // nếu trên bị ngược

            // 4) Áp transform cho tool
            RectRotate r = toolRectMaster.Clone();

            // 4a) Xoay tâm tool quanh pivotOrigin
            r._PosCenter = RotateAround(r._PosCenter, pivotOrigin, dAngle);

            // 4b) Dịch theo sự dịch chuyển của object
            r._PosCenter = new PointF(
                r._PosCenter.X + (pivotCurrent.X - pivotOrigin.X),
                r._PosCenter.Y + (pivotCurrent.Y - pivotOrigin.Y)
            );

            // 4c) Cộng delta góc vào góc tool
            r._rectRotation += dAngle;

            return r;
        }


        public static  void InitialTranslate()
        {
        }

        public static RectangleF GetBoundingBox(RectRotate rr)
        {
            float w = rr._rect.Width;
            float h = rr._rect.Height;

            PointF[] pts =
            {
        new PointF(-w/2, -h/2),
        new PointF( w/2, -h/2),
        new PointF( w/2,  h/2),
        new PointF(-w/2,  h/2)
    };

            PointF[] ptsW = new PointF[4];
            for (int i = 0; i < 4; i++)
                ptsW[i] = RectRotate.Add(rr._PosCenter, RectRotate.Rotate(pts[i], rr._rectRotation));

            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            foreach (var p in ptsW)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public static PointF RotateAround(PointF p, PointF center, float deg)
        {
            float dx = p.X - center.X;
            float dy = p.Y - center.Y;

            double rad = deg * Math.PI / 180.0;
            double c = Math.Cos(rad), s = Math.Sin(rad);

            float xr = (float)(dx * c - dy * s);
            float yr = (float)(dx * s + dy * c);

            return new PointF(center.X + xr, center.Y + yr);
        }
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
       
       static String er;
        public static bool IsRun=false;
       
        public static TypeCrop TypeCrop = TypeCrop.Area;
        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        public static RectRotate GetPositionAdjustment(RectRotate rotOrigin, RectRotate rotTemp,float XAdj,float YAdj,float AngAdj)
        {
            System.Drawing.Size sz = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
            RectRotate rot = new RectRotate();
            rot.Shape = rotOrigin.Shape;
            rot.PolyLocalPoints=rotOrigin.PolyLocalPoints;
            rot.HexVertexOffsets=rotOrigin.HexVertexOffsets;
            rot.IsWhite = rotOrigin.IsWhite;
           
            rot._rect = rotOrigin._rect;
            rot._rectRotation = rotOrigin._rectRotation + AngAdj;
            PointF pPos = new PointF(rotTemp._PosCenter.X + XAdj, rotTemp._PosCenter.Y + YAdj);
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
            double angle2 = angle1 - AngAdj;
            if (DeltaX > 0 && DeltaY < 0)
                angle2 = angle1 - AngAdj;
            else if (DeltaX > 0 && DeltaY > 0)
                angle2 = -angle1 - AngAdj;
            else if (DeltaX < 0 && DeltaY < 0)
                angle2 = angle1 + AngAdj;
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


                           
                                try
                                {
                                    Global.IsIntialPython = true;
                                    G.np = Py.Import("numpy");
                                    dynamic mod = Py.Import("Tool.Learning");
                                    dynamic cls = mod.GetAttr("ObjectDetector"); // class
                                    G.objYolo = cls.Invoke();
                                    Global.IsLearning = true;
                                }
                                catch (Exception ex)
                                {
                                    Global.IsLearning = false;
                                }
                            
                            if(Directory.Exists( pythonHome+ "\\Lib\\site-packages\\craft_text_detector"))//
                             
                            {
                                try
                                {

                                    dynamic module = Py.Import("Tool.Craft_OCR");
                                    dynamic cls2 = module.GetAttr("CraftOCRDetector"); // class
                                    G.objCraftOCR = cls2.Invoke();              // khởi tạo instance
                                    Global.IsOCR = true;
                                   
                                }
                                catch (Exception ex)
                                {
                                }

                            }
                            else
                                Global.IsOCR = false;
                            //dynamic module3 = Py.Import("Tool.OCR");
                            //dynamic cls3 = module3.GetAttr("OCR"); // class
                            //G.objOCR = cls3.Invoke();
                            Global.IsIntialPython = true;


                        }

                    }
                }
            }
            catch (PythonException ex)
            {
                 er=ex.Message;
                String a = "b";
                //MessageBox.Show(ex.Message);
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
            G.CommonPlus.CropRotate((int)rot._PosCenter.X, (int)rot._PosCenter.Y, (int)rot._rect.Width, (int)rot._rect.Height, rot._rectRotation);
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
     
     
      
        public static Mat LoadImage(string path, ImreadModes mode)
        { Mat raw = OpenCvSharp.Cv2.ImRead(path, mode);

       
            
            return raw;
        }
    }
}
