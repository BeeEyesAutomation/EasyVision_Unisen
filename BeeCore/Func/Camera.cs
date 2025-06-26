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
using System.Windows.Forms;
using static EasyModbus.ModbusServer;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    public class Camera
    {
        public static void Setting()
        {
            G.CCD.ShowSetting();


        }
        public static string Scan()
        {
          if(G.TypeCCD==TypeCamera.TinyIV)
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
            return G.CCD.ScanCCD();


        }
        public static void DestroyAll()
        {
            if (G.TypeCCD == TypeCamera.TinyIV)
            {
                 HEROJE.Disconnect(); 
               
            }
            G.CCD.DestroyAll();
            Common.ClosePython();
            
            HEROJE.DisConnect();
            Thread.Sleep(500);
            Application.ExitThread();
        }
        public static bool Status()
        {

            return G.CCD.IsErrCCD;
        }
        public static bool Connect(int indexCCD, String Resolution)
        {
            if(G.TypeCCD == TypeCamera.TinyIV)
            {
               
                    IsConnected = HEROJE.Connect(indexCCD);
                return IsConnected;
            }    
            //String[] sp = Resolution.Split(' ');
            //String[] sp2 = sp[0].Split('x');

            //BeeCore.G.CCD.colCCD = Convert.ToInt32(sp2[0]);
            //BeeCore.G.CCD.rowCCD = Convert.ToInt32(sp2[1]);
            Mat raw = new Mat();
            if (G.CCD.Connect(0,0, indexCCD))
            {
                if (Common.matRaw != null)
                    if (!Common.matRaw.Empty())
                        Common.matRaw.Release();
                //if (IsHist)
                //    G.CCD.ReadRaw(true);
                //else
                G.CCD.ReadCCD();
                int rows=0, cols=0;int Type = 0;
                IntPtr intPtr = new IntPtr();
                raw = new Mat();
                try
                {

                    unsafe
                    {
                        intPtr = Native.GetRaw(ref rows, ref cols, ref Type);
                        raw = new Mat(rows, cols, Type, intPtr);

                        FrameRate = G.CCD.FPS;
                        BeeCore.Common.Cycle = G.CCD.cycle;
                        BeeCore.Common.matRaw = raw.Clone();
                        G.ParaCam.SizeCCD = new System.Drawing.Size(BeeCore.Common.matRaw.Width, BeeCore.Common.matRaw.Height);
                    }
                    //    return new Mat();


                }
                finally
                {
                    raw.Release();
                    // Giải phóng bộ nhớ sau khi sử dụng
                    //Marshal.FreeHGlobal(intPtr);
                }
                
                //StepExposure = G.CCD.StepExposure;
                //MinExposure = G.CCD.MinExposure;
                //MaxExposure = G.CCD.MaxExposure;
                if (G.ParaCam._Exposure != 0)
                    G.CCD.Exposure = G.ParaCam._Exposure;
               // Cycle = G.CCD.cycle;
                G.CCD.SetPara();
                ///G.CommonPlus.GetImageRaw();
                return true;
            }
            return false;

        }

        public static bool IsConnected = false;
        private static int frameRate = 0;
        public static int FrameRate
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
        public static event PropertyChangedEventHandler FrameChanged;
        public static event PropertyChangedEventHandler PropertyChanged;
        public static void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(G.Common, new PropertyChangedEventArgs(propertyName));
            }
        }



        public static bool SetExpo(int value)
        {
            try
            {


                switch (G.TypeCCD)
                {
                    case TypeCamera.BaslerGigE:
                        if (value > 1000)
                        {
                            G.CCD.Exposure = value; G.CCD.SetPara();
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
        public static int GetExpo()
        {
            try
            {


                switch (G.TypeCCD)
                {
                    case TypeCamera.BaslerGigE:
                        // trackExposure.Min = (int)BeeCore.Common.MinExposure;
                        //trackExposure.Max = (int)BeeCore.Common.MaxExposure;
                        // trackExposure.Step= (int)BeeCore.Common.StepExposure;
                       return BeeCore.G.ParaCam.Exposure;
                        //if (value > 1000)
                        //{
                        //    G.CCD.Exposure = value; G.CCD.SetPara();
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
        public static bool SetGain(int value)
        {
            try
            {


                switch (G.TypeCCD)
                {
                    case TypeCamera.BaslerGigE:
                        //if (value > 1000)
                        //{
                        //    G.CCD.ga = value; G.CCD.SetPara();
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
        public static int GetGain()
        {
            try
            {


                switch (G.TypeCCD)
                {
                    case TypeCamera.BaslerGigE:
                        // trackExposure.Min = (int)BeeCore.Common.MinExposure;
                        //trackExposure.Max = (int)BeeCore.Common.MaxExposure;
                        // trackExposure.Step= (int)BeeCore.Common.StepExposure;
                        return BeeCore.G.ParaCam.Exposure;
                        //if (value > 1000)
                        //{
                        //    G.CCD.Exposure = value; G.CCD.SetPara();
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
        public static System.Drawing.Size GetSzCCD()
        {
          return new System.Drawing.Size(BeeCore.Common.matRaw.Width, BeeCore.Common.matRaw.Height);

        }
        public static  void Read()
        {
            int rows = 0, cols = 0, Type = 0;
            
            Mat raw = new Mat();
            IntPtr intPtr = IntPtr.Zero;

            try
            {
                switch (G.TypeCCD)
                {
                    case TypeCamera.USB:
                        if (Common.matRaw != null)
                            if (!Common.matRaw.Empty())
                                Common.matRaw.Release();
                        //if (IsHist)
                        //    G.CCD.ReadRaw(true);
                        //else
                        G.CCD.ReadCCD();
                        
                        
                        try
                        {

                            unsafe
                            {
                                 intPtr = Native.GetRaw(ref rows, ref cols, ref Type);
                                raw = new Mat(rows, cols, Type, intPtr);

                                FrameRate = G.CCD.FPS;
                                BeeCore.Common.Cycle = G.CCD.cycle;
                                BeeCore.Common.matRaw = raw;
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
                        if (Common.matRaw != null)
                            if (!Common.matRaw.Empty())
                                Common.matRaw.Dispose();
                        //if (IsHist)
                        //    G.CCD.ReadRaw(true);
                        //else
                        Stopwatch stopwatch = new Stopwatch(); 
                        stopwatch.Start();
                        G.CCD.ReadCCD();

                        
                         raw = new Mat();
                      
                        try
                        {

                            unsafe
                            {
                              
                                    intPtr = Native.GetRaw(ref rows, ref cols, ref Type);
                                raw = new Mat(rows, cols, Type, intPtr);

                                FrameRate = G.CCD.FPS;
                            
                                    BeeCore.Common.matRaw = raw.Clone();
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

                   Size SZ=  HEROJE.Read();
                    if(SZ.Width==0&&SZ.Height==0)
                            IsConnected = false;
                        BeeCore.Common.Cycle =Convert.ToInt32(1000.0 / HEROJE.FrameTime);
                                                                                                                                                                                                                                                                                                                                                                            Native.SetImg(BeeCore.Common.matRaw);
                        break;
                }
              



            }
            catch (Exception e) {
            }
               
            
          
           // return new Mat();
        }
        public static void Light(int TypeLight, bool IsOn)
        {
            switch (G.TypeCCD)
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
        public static void SetReSolution(int TypeReSolution)
        {
            switch (G.TypeCCD)
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
