using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using static EasyModbus.ModbusServer;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    public class Camera
    {
        public int IndexCCD=0;
        public Camera( ParaCamera paraCamera,int index)
        {
            this.Para = paraCamera;
            this.IndexCCD = index;
        }
        public   Mat matRaw = new Mat();
        public CvPlus.CCD CCDPlus = new CvPlus.CCD();
        public  void Setting()
        {
            CCDPlus.ShowSetting();


        }
        public  string Scan()
        {
          if(Para.TypeCamera==TypeCamera.TinyIV)
            {
                if (G.ParaCam.CardChoosed == null) G.ParaCam.CardChoosed = "";
                    if ( G.ParaCam.CardChoosed!="")
                {
                    try
                    {
                        int[] IP = G.ParaCam.CardChoosed.Split('.').Select(int.Parse).ToArray();
                        return BeeCore.HEROJE.Scan(IP[0], IP[1], IP[2], IP[3]);

                    }
                   catch(Exception)
                    {
                        return BeeCore.HEROJE.Scan(192, 168, 2, 1);
                    }
                }
               else
                    return BeeCore.HEROJE.Scan(192, 168, 2,1);
            }
           
          else
                return CCDPlus.ScanCCD();


        }
        
        public  void DestroyAll()
        {
            if (Para.TypeCamera == TypeCamera.TinyIV)
            {
                 HEROJE.Disconnect(); 
               
            }
           
            CCDPlus.DestroyAll(IndexCCD);
            Common.ClosePython();
            
            HEROJE.DisConnect();
            Thread.Sleep(500);
            Application.ExitThread();
        }
        public  bool Status()
        {

            return CCDPlus.IsErrCCD;
        }
        public void DisConnect()
        {
            CCDPlus.DestroyAll(IndexCCD);
        }
        public  bool Connect(String NameCCD )
        {
            if(Para.TypeCamera == TypeCamera.TinyIV)
            {
               
                    IsConnected = HEROJE.Connect(0);
                return IsConnected;
            }    
            //String[] sp = Resolution.Split(' ');
            //String[] sp2 = sp[0].Split('x');

            //BeeCore.CCDPlus.colCCD = Convert.ToInt32(sp2[0]);
            //BeeCore.CCDPlus.rowCCD = Convert.ToInt32(sp2[1]);
            Mat raw = new Mat();
            if (CCDPlus.Connect(IndexCCD, NameCCD))
            {
                if (matRaw != null)
                    if (!matRaw.Empty())
                        matRaw.Release();
                //if (IsHist)
                //    CCDPlus.ReadRaw(true);
                //else
                CCDPlus.ReadCCD(IndexCCD);
                int rows=0, cols=0;int Type = 0;
                IntPtr intPtr = new IntPtr();
                raw = new Mat();
                try
                {

                    unsafe
                    {
                        intPtr = Native.GetRaw(ref rows, ref cols, ref Type);
                        raw = new Mat(rows, cols, Type, intPtr);

                        FrameRate = CCDPlus.FPS;
                        BeeCore.Common.Cycle = CCDPlus.cycle;
                       matRaw = raw.Clone();
                        G.ParaCam.SizeCCD = new System.Drawing.Size(matRaw.Width,matRaw.Height);
                    }
                    //    return new Mat();


                }
                finally
                {
                    raw.Release();
                    // Giải phóng bộ nhớ sau khi sử dụng
                    //Marshal.FreeHGlobal(intPtr);
                }
                
                //StepExposure = CCDPlus.StepExposure;
                //MinExposure = CCDPlus.MinExposure;
                //MaxExposure = CCDPlus.MaxExposure;
                if (G.ParaCam._Exposure != 0)
                    CCDPlus.Exposure = G.ParaCam._Exposure;
               // Cycle = CCDPlus.cycle;
                CCDPlus.SetPara();
                ///G.CommonPlus.GetImageRaw();
                return true;
            }
            return false;

        }
        //public static void SetRaw()
        //{
        //    //{if (matRaw == null) matRaw = new Mat();
        //    //    if (matLive.Type() == MatType.CV_8UC4)
        //    //        Cv2.CvtColor(matLive.Clone(), matRaw, ColorConversionCodes.BGRA2BGR);
        //    //else
        //    //    matRaw = matLive.Clone();
        //    // G.CommonPlus.BitmapSrc(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(raw));
        //    SetSrc(matRaw.Data, matRaw.Rows, matRaw.Cols, matRaw.Type());
        //}
        public  bool IsConnected = false;
        private  int frameRate = 0;
        public  int FrameRate
        {
            get => frameRate; set
            {
                frameRate = value;
                if (FrameChanged != null)
                {
                    FrameChanged(G.Common, new PropertyChangedEventArgs("FrameChanged"));
                }
            }
        }
        public  event PropertyChangedEventHandler FrameChanged;
        public  event PropertyChangedEventHandler PropertyChanged;
        public  void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(G.Common, new PropertyChangedEventArgs(propertyName));
            }
        }



        public  bool SetExpo(int value)
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        if (value > 1000)
                        {
                            CCDPlus.Exposure = value; CCDPlus.SetPara();
                        }
                        break;
                    case TypeCamera.TinyIV:
                        HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public  float GetExpo()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        // trackExposure.Min = (int)BeeCore.Common.MinExposure;
                        //trackExposure.Max = (int)BeeCore.Common.MaxExposure;
                        // trackExposure.Step= (int)BeeCore.Common.StepExposure;
                       return Para.Exposure;
                        //if (value > 1000)
                        //{
                        //    CCDPlus.Exposure = value; CCDPlus.SetPara();
                        //}
                        break;
                    case TypeCamera.TinyIV:
                       return Convert.ToInt32( HEROJE.GetExposure());
                        break;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }
        public  bool SetGain(int value)
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        //if (value > 1000)
                        //{
                        //    CCDPlus.ga = value; CCDPlus.SetPara();
                        //}
                        break;
                    case TypeCamera.TinyIV:
                        HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public ParaCamera Para = new ParaCamera();
        public  float GetGain()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        // trackExposure.Min = (int)BeeCore.Common.MinExposure;
                        //trackExposure.Max = (int)BeeCore.Common.MaxExposure;
                        // trackExposure.Step= (int)BeeCore.Common.StepExposure;
                        return Para.Exposure;
                        //if (value > 1000)
                        //{
                        //    CCDPlus.Exposure = value; CCDPlus.SetPara();
                        //}
                        break;
                    case TypeCamera.TinyIV:
                        return Convert.ToInt32(HEROJE.GetExposure());
                        break;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return -1;
        }
        public  System.Drawing.Size GetSzCCD()
        {
          return new System.Drawing.Size(matRaw.Width,matRaw.Height);

        }
        public  void Init()
        {
           
            CCDPlus.typeCCD = (int)Para.TypeCamera;
        }
       
        public   void Read()
        {
            int rows = 0, cols = 0, Type = 0;
            
            Mat raw = new Mat();
            IntPtr intPtr = IntPtr.Zero;

            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.USB:
                        if (matRaw != null)
                            if (!matRaw.Empty())
                                matRaw.Release();
                        //if (IsHist)
                        //    CCDPlus.ReadRaw(true);
                        //else
                        CCDPlus.ReadCCD(IndexCCD);
                        CCDPlus.ReadCCD(IndexCCD);
                        
                        
                        try
                        {

                            unsafe
                            {
                                 intPtr = Native.GetRaw(ref rows, ref cols, ref Type);
                                raw = new Mat(rows, cols, Type, intPtr);

                                FrameRate = CCDPlus.FPS;
                                BeeCore.Common.Cycle = CCDPlus.cycle;
                               matRaw = raw;
                            }
                            //    return new Mat();


                        }
                        finally
                        {
                            //raw.Release();
                            // Giải phóng bộ nhớ sau khi sử dụng
                           // if(intPtr!=null)
                         //  Marshal.FreeHGlobal(intPtr);
                        }
                        break;
                    case TypeCamera.BaslerGigE:
                        if (matRaw != null)
                            if (!matRaw.Empty())
                                matRaw.Dispose();
                        //if (IsHist)
                        //    CCDPlus.ReadRaw(true);
                        //else
                        Stopwatch stopwatch = new Stopwatch(); 
                        stopwatch.Start();
                        CCDPlus.ReadCCD(IndexCCD);

                        
                         raw = new Mat();
                      
                        try
                        {

                            unsafe
                            {
                              
                                    intPtr = Native.GetRaw(ref rows, ref cols, ref Type);
                                raw = new Mat(rows, cols, Type, intPtr);

                                FrameRate = CCDPlus.FPS;
                            
                                   matRaw = raw.Clone();
                                stopwatch.Stop();
                                BeeCore.Common.Cycle =(int)stopwatch.Elapsed.TotalMilliseconds;

                            }
                           

                        }
                        finally
                        {
                           raw.Release();
                           Native. FreeBuffer(intPtr);
                        }
                        break;
                       case TypeCamera.TinyIV:
                        Mat raw2= HEROJE.Read();
                        Size SZ = raw2.Size(); 
                    if(SZ.Width==0&&SZ.Height==0)
                            IsConnected = false;
                        BeeCore.Common.Cycle =Convert.ToInt32(1000.0 / HEROJE.FrameTime);
                         Native.SetImg(matRaw);
                        break;
                }
              



            }
            catch (Exception e) {
            }
               
            
          
           // return new Mat();
        }
        public  void Light(int TypeLight, bool IsOn)
        {
            switch (Para.TypeCamera)
            {
                case TypeCamera.TinyIV:
                    HEROJE.Light(TypeLight, IsOn);
                    break;
                case TypeCamera.BaslerGigE:
                   
                    break;
                default:
                    break;

            }
        }
        public  void SetReSolution(int TypeReSolution)
        {
            switch (Para.TypeCamera)
            {
                case TypeCamera.TinyIV:
                    HEROJE.SetReSolution(TypeReSolution);
                    break;
                default:
                    break;

            }
            
        }
    }
}
