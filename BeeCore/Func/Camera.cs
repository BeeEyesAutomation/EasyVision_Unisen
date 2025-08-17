using BeeCore.Func;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Features2D;
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
                if (Global.ParaCommon.CardChoosed == null) Global.ParaCommon.CardChoosed = "";
                    if ( Global.ParaCommon.CardChoosed!="")
                {
                    try
                    {
                        int[] IP = Global.ParaCommon.CardChoosed.Split('.').Select(int.Parse).ToArray();
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
                HEROJE.DisConnect();
            }
           
            CCDPlus.DestroyAll(IndexCCD);
            Common.ClosePython();
            
    
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
        public  async Task<bool> Connect(String NameCCD )
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
            String[] sp = NameCCD.Split('$');
            if (sp.Length > 1)
            {
                String Typ = sp[sp.Length - 2];
                if (Typ.Contains("Basler"))
                {
                    TypeCCD = 0;
                }
                else if (Typ.Contains("Hik"))
                {
                    TypeCCD = 1;
                }
                else
                    TypeCCD = -1;
            }
            if (CCDPlus.Connect(IndexCCD, NameCCD))
            {
                if (matRaw != null)
                    if (!matRaw.Empty())
                        matRaw.Release();
                //if (IsHist)
                //    CCDPlus.ReadRaw(true);
                //else
                Read();
                bool IsFail = false;
                if (Para.Exposure == null)
                {
                    IsFail = true;
                    Para.Exposure = new ValuePara();
                }    
                   
                if (Para.Gain == null)
                {
                    IsFail = true;
                    Para.Gain = new ValuePara();
                }    
                  
                if (Para.Shift == null)
                {
                    IsFail = true;
                    Para.Shift = new ValuePara();
                }    
                   
                if (Para.Width == null)
                {
                    IsFail = true;
                    Para.Width = new ValuePara();
                }    
                   
                if (Para.Height == null)
                {
                    IsFail = true;
                    Para.Height = new ValuePara();
                }    
                    
                if (Para.OffSetX == null)
                {
                    IsFail = true;
                    Para.OffSetX = new ValuePara();
                }    
                   
                if (Para.OffSetY == null)
                {
                    IsFail = true;
                    Para.OffSetY = new ValuePara();
                }    
                  
                if(!IsFail&&Para.Width.Value>Para.Width.Min+1 && Para.Height.Value > Para.Height.Min + 1)
                 await SetFullPara();
                //if (Global.ParaCommon._Exposure != 0)
                //    CCDPlus.Exposure = Global.ParaCommon._Exposure;

                return true;
            }
            return false;

        }
        public async Task<bool> SetFullPara()

        {
            await  SetWidth();
            await SetHeight();
            await SetOffSetX();
            await SetOffSetY();
            await SetCenterX();
            await SetCenterY();
            await SetExpo();
            await SetGain();
            await SetShift();
            return true;
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
                    FrameChanged(FrameRate, new PropertyChangedEventArgs("FrameChanged"));
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

        public int TypeCCD = -1;
        public String Err = "";
        public async Task<  bool> SetExpo()
        {
            try
            {

                
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler

                                Para.Exposure.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "ExposureTimeRaw", Para.Exposure.Value), cancel.Token);
                               
                                break;
                             case 1://Hik
                                //Para.Exposure.Value = CCDPlus.SetPara(IndexCCD, "ExposureTimeRaw", Para.Exposure.Value);

                                break;
                        }
                            return true;
                        break;
                    //case TypeCamera.TinyIV:
                    //    HEROJE.SetExposure((int)Para.Exposure);
                    //    break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return false;// Result.Success.ToString();
        }
        public async Task< bool> GetExpo()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "ExposureTimeRaw", ref Para.Exposure.Min, ref Para.Exposure.Max, ref Para.Exposure.Step, ref Para.Exposure.Value), cancel.Token);
                               
                                break;
                            case 1://Hik
                                //Para.Exposure.Value = CCDPlus.SetPara(IndexCCD, "ExposureTimeRaw", Para.Exposure.Value);

                                break;
                        }
                       
                        break;
                    //case TypeCamera.TinyIV:

                    //   return Convert.ToInt32( HEROJE.GetExposure());
                    //    break;
                }
            }
            catch (Exception ex)
            {
           
                return false;
            }
            return false;
        }
        public async Task< bool> SetGain()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                Para.Gain.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "GainRaw", Para.Gain.Value), cancel.Token);

                                break;
                            case 1://Hik
                                
                                break;
                        }
                      break;
                    case TypeCamera.TinyIV:
                       // HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        [NonSerialized]
        CancellationTokenSource cancel = new CancellationTokenSource(2000);
        public async Task<bool> SetShift()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                Para.Shift.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value), cancel.Token);
                              //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
                                break;
                            case 1://Hik

                                break;
                        }
                        break;
                    case TypeCamera.TinyIV:
                        // HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public async Task< bool> GetShift()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "DigitalShift", ref Para.Shift.Min, ref Para.Shift.Max, ref Para.Shift.Step, ref Para.Shift.Value), cancel.Token);

                                break;
                            case 1://Hik

                                break;
                        }
                      
                        return true;
                        //if (value > 1000)
                        //{
                        //    CCDPlus.Exposure = value; CCDPlus.SetPara();
                        //}
                        break;
                        //case TypeCamera.TinyIV:
                        //    return Convert.ToInt32(HEROJE.GetExposure());
                        //    break;
                }
            }
            catch (Exception ex)
            {
               
                return false;
            }
            return false;
        }

        public async Task<bool> SetWidth()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                Para.Width.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "Width", Para.Width.Value), cancel.Token);
                                //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
                                break;
                            case 1://Hik

                                break;
                        }
                        break;
                    case TypeCamera.TinyIV:
                        // HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetWidth()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "Width", ref Para.Width.Min, ref Para.Width.Max, ref Para.Width.Step, ref Para.Width.Value), cancel.Token);

                                break;
                            case 1://Hik

                                break;
                        }

                        return true;
                        //if (value > 1000)
                        //{
                        //    CCDPlus.Exposure = value; CCDPlus.SetPara();
                        //}
                        break;
                        //case TypeCamera.TinyIV:
                        //    return Convert.ToInt32(HEROJE.GetExposure());
                        //    break;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return false;
        }
        public async Task<bool> SetHeight()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                Para.Height.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "Height", Para.Height.Value), cancel.Token);
                                //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
                                break;
                            case 1://Hik

                                break;
                        }
                        break;
                    case TypeCamera.TinyIV:
                        // HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetHeight()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "Height", ref Para.Height.Min, ref Para.Height.Max, ref Para.Height.Step, ref Para.Height.Value), cancel.Token);

                                break;
                            case 1://Hik

                                break;
                        }

                        return true;
                        //if (value > 1000)
                        //{
                        //    CCDPlus.Exposure = value; CCDPlus.SetPara();
                        //}
                        break;
                        //case TypeCamera.TinyIV:
                        //    return Convert.ToInt32(HEROJE.GetExposure());
                        //    break;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return false;
        }

        public async Task<bool> SetOffSetX()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                Para.OffSetX.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "OffsetX", Para.OffSetX.Value), cancel.Token);
                                //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
                                break;
                            case 1://Hik

                                break;
                        }
                        break;
                    case TypeCamera.TinyIV:
                        // HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetOffSetX()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "OffsetX", ref Para.OffSetX.Min, ref Para.OffSetX.Max, ref Para.OffSetX.Step, ref Para.OffSetX.Value), cancel.Token);

                                break;
                            case 1://Hik

                                break;
                        }

                        return true;
                        //if (value > 1000)
                        //{
                        //    CCDPlus.Exposure = value; CCDPlus.SetPara();
                        //}
                        break;
                        //case TypeCamera.TinyIV:
                        //    return Convert.ToInt32(HEROJE.GetExposure());
                        //    break;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return false;
        }

        public async Task<bool> SetOffSetY()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                Para.OffSetY.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "OffsetY", Para.OffSetY.Value), cancel.Token);
                                //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
                                break;
                            case 1://Hik

                                break;
                        }
                        break;
                    case TypeCamera.TinyIV:
                        // HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetOffSetY()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "OffsetY", ref Para.OffSetY.Min, ref Para.OffSetY.Max, ref Para.OffSetY.Step, ref Para.OffSetY.Value), cancel.Token);

                                break;
                            case 1://Hik

                                break;
                        }

                        return true;
                        //if (value > 1000)
                        //{
                        //    CCDPlus.Exposure = value; CCDPlus.SetPara();
                        //}
                        break;
                        //case TypeCamera.TinyIV:
                        //    return Convert.ToInt32(HEROJE.GetExposure());
                        //    break;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return false;
        }
        float none = 0;
        public async Task<bool> SetCenterX()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                Para.CenterX = (int)(await  Task.Run(() => CCDPlus.SetPara(IndexCCD, "CenterX",(int) Para.CenterX), cancel.Token));
                                //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
                                break;
                            case 1://Hik

                                break;
                        }
                        break;
                    case TypeCamera.TinyIV:
                        // HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetCenterX()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "CenterX", ref none, ref none, ref none, ref Para.CenterX), cancel.Token);

                                break;
                            case 1://Hik

                                break;
                        }

                        return true;
                
                        break;
                      
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return false;
        }

        public async Task<bool> SetCenterY()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                Para.CenterY = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "CenterY", Para.CenterY), cancel.Token);
                                //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
                                break;
                            case 1://Hik

                                break;
                        }
                        break;
                    case TypeCamera.TinyIV:
                        // HEROJE.SetExposure(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetCenterY()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "CenterY", ref none, ref none, ref none, ref Para.CenterY), cancel.Token);

                                break;
                            case 1://Hik

                                break;
                        }

                        return true;

                        break;

                }
            }
            catch (Exception ex)
            {

                return false;
            }
            return false;
        }
        public ParaCamera Para = new ParaCamera();
        public async Task< bool> GetGain()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                        cancel = new CancellationTokenSource(2000);
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "GainRaw", ref Para.Gain.Min, ref Para.Gain.Max, ref Para.Gain.Step, ref Para.Gain.Value), cancel.Token);
                                break;
                            case 1://Hik

                                break;
                        }
                        
                        return true;
                        //if (value > 1000)
                        //{
                        //    CCDPlus.Exposure = value; CCDPlus.SetPara();
                        //}
                        break;
                    //case TypeCamera.TinyIV:
                    //    return Convert.ToInt32(HEROJE.GetExposure());
                    //    break;
                }
            }
            catch (Exception ex)
            {
                
                return false;
            }
            return false;
        }
        public  System.Drawing.Size GetSzCCD()
        {
            if (matRaw == null) return new System.Drawing.Size();
                if (matRaw.Empty()) return new System.Drawing.Size();
            return new System.Drawing.Size(matRaw.Width,matRaw.Height);

        }
        public  void Init()
        {
           
            CCDPlus.TypeCamera = (int)Para.TypeCamera;
        }
        [NonSerialized]
        Stopwatch stopwatch = new Stopwatch();
        public   void Read()
        {
            int rows = 0, cols = 0, Type = 0;
            
            Mat raw = new Mat();
            IntPtr intPtr = IntPtr.Zero;

            try
            {     //  if (matRaw != null)
            //            if (!matRaw.Empty())
            //            matRaw.Release();
                        switch (Para.TypeCamera)
                {
                    case TypeCamera.USB:
                      
                     
                        stopwatch.Restart();
                        CCDPlus.ReadCCD(IndexCCD);
                        //CCDPlus.ReadCCD(IndexCCD);
                        CameraIOFast.TryGrabFast_NoStride(ref matRaw);
                       
                        stopwatch.Stop();
                        BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;
                        FrameRate = CCDPlus.FPS;
                        //try
                        //{

                        //    unsafe
                        //    {
                        //         intPtr = Native.GetRaw(ref rows, ref cols, ref Type);
                        //        raw = new Mat(rows, cols, Type, intPtr);

                        //        FrameRate = CCDPlus.FPS;
                        //        BeeCore.Common.CycleCamera = CCDPlus.cycle;
                        //       matRaw = raw.Clone();
                        //    }
                        //    //    return new Mat();


                        //}
                        //finally
                        //{
                        //    raw.Release();
                        //    Native.FreeBuffer(intPtr);
                        //}
                        break;
                    case TypeCamera.BaslerGigE:

                        //if (IsHist)
                        //    CCDPlus.ReadRaw(true);
                        //else

                        stopwatch.Restart();
                        CCDPlus.ReadCCD(IndexCCD);
                        CameraIOFast.TryGrabFast_NoStride(ref matRaw);
                        FrameRate = CCDPlus.FPS;
                        stopwatch.Stop();
                        BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;
                        FrameRate = CCDPlus.FPS;
                        // raw = new Mat();

                        //try
                        //{

                        //    unsafe
                        //    {

                        //            intPtr = Native.GetRaw(ref rows, ref cols, ref Type);
                        //        raw = new Mat(rows, cols, Type, intPtr);

                        //        FrameRate = CCDPlus.FPS;

                        //           matRaw = raw.Clone();
                        //        stopwatch.Stop();
                        //        BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;

                        //    }


                        //}
                        //finally
                        //{
                        //   raw.Release();
                        //   Native. FreeBuffer(intPtr);
                        //}
                        break;
                       case TypeCamera.TinyIV:
                        Mat raw2= HEROJE.Read();
                        Size SZ = raw2.Size(); 
                    if(SZ.Width==0&&SZ.Height==0)
                            IsConnected = false;
                        BeeCore.Common.CycleCamera = Convert.ToInt32(1000.0 / HEROJE.FrameTime);
                         Native.SetImg(matRaw);
                        break;
                }
              



            }
            catch (Exception ex) {
                Global.Ex = "Cam_" + ex.Message;
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
                    Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.Light;
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
