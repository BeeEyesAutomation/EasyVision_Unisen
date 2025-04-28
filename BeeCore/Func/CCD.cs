using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            return BeeCore.HEROJE.Scan(192, 168, 2, 7);
          else
            return G.CCD.ScanCCD();


        }
        public static void DestroyAll()
        {
            if (G.TypeCCD == TypeCamera.TinyIV)
            {
                 HEROJE.Disconnect(); 
               
            }

            Common.ClosePython();
            G.CCD.DestroyAll();
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
            String[] sp = Resolution.Split(' ');
            String[] sp2 = sp[0].Split('x');

            BeeCore.G.CCD.colCCD = Convert.ToInt32(sp2[0]);
            BeeCore.G.CCD.rowCCD = Convert.ToInt32(sp2[1]);

            if (G.CCD.Connect(Convert.ToInt32(sp2[1]), Convert.ToInt32(sp2[0]), indexCCD))
            {
                G.CCD.ReadCCD();
                G.ParaCam.SizeCCD = new System.Drawing.Size(BeeCore.G.CCD.colCCD, BeeCore.G.CCD.rowCCD);
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
        public static async void Read()
        {
            int rows = 0, cols = 0, Type = 0;
            IntPtr intPtr = new IntPtr(); 
            Mat raw = new Mat();
            //if (Common.matRaw != null)
            //    if (!Common.matRaw.Empty())
            //        Common.matRaw.Release();
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
                                BeeCore.Common.matRaw = raw.Clone();
                            }
                            //    return new Mat();


                        }
                        finally
                        {
                            raw.Release();
                            // Giải phóng bộ nhớ sau khi sử dụng
                        //    Marshal.FreeHGlobal(intPtr);
                        }
                        break;
                    case TypeCamera.BaslerGigE:
                        if (Common.matRaw != null)
                            if (!Common.matRaw.Empty())
                                Common.matRaw.Release();
                        //if (IsHist)
                        //    G.CCD.ReadRaw(true);
                        //else
                        G.CCD.ReadCCD();
                       
                         intPtr = new IntPtr();
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
                            }
                            //    return new Mat();


                        }
                        finally
                        {
                            raw.Release();
                            // Giải phóng bộ nhớ sau khi sử dụng
                            //Marshal.FreeHGlobal(intPtr);
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
