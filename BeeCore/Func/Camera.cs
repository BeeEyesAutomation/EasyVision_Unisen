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
using System.Drawing.Drawing2D;
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
        public void SetAutoFocus(bool IsFocus)
        {
            CCDPlus.AutoFocus(IsFocus);
        }
        public void SetFocus()
        {
            CCDPlus.SetFocus(Para.Focus);
        }
        public void SetZoom()
        {
            CCDPlus.SetZoom(Para.Zoom);
        }
        public int GetFocus()
        {
           return CCDPlus.GetFocus();
        }
        public int GetZoom()
        {
          return  CCDPlus.GetZoom();
        }
        public int GetWidthUSB()
        {
            return CCDPlus.GetWidth();
        }
        public int GetHeightUSB()
        {
            return CCDPlus.GetHeight();
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
        public  Bitmap bmResult;
        private Mat EnsureWorkingBuffer(Mat src)
        {
            // Nếu đang hiển thị A thì vẽ vào B, ngược lại
            bool useB = ReferenceEquals(_displayMat, _bufA);
            Mat target = useB ? _bufB : _bufA;

            if (target == null || target.IsDisposed)
            {
                target = new Mat();                     // tạo mới nếu đã Dispose
                if (useB) _bufB = target; else _bufA = target;
            }

            // target.Create sẽ cấp phát đúng kích thước/kiểu; không cần Release trước
            target.Create(src.Rows, src.Cols, src.Type());
            return target;
        }
        // Locks
        private readonly object _bmLock = new object();   // bảo vệ bmResult
        private readonly object _camLock = new object();   // bảo vệ nguồn camera (nếu cần)
        private readonly object _swapLock = new object();   // bảo vệ A/B & _displayMat

        // Double-buffer Mat (KHÔNG readonly để có thể thay thế khi bị Dispose)
        private Mat _bufA = new Mat();
        private Mat _bufB = new Mat();
        private Mat _displayMat; // trỏ tới buffer đang hiển thị (A hoặc B)

        private bool _disposed;
        private static Mat EnsureBgr8Uc3AliasOrConvert(Mat working, out bool createdTemp)
        {
            createdTemp = false;
            if (working.Type() == MatType.CV_8UC3) return working;

            var dst = new Mat();
            createdTemp = true;

            if (working.Channels() == 1)
            {
                if (working.Depth() == MatType.CV_8U)
                    Cv2.CvtColor(working, dst, ColorConversionCodes.GRAY2BGR);
                else
                {
                    var tmp8 = new Mat();
                    try
                    {
                        Cv2.Normalize(working, tmp8, 0, 255, NormTypes.MinMax);
                        tmp8.ConvertTo(tmp8, MatType.CV_8U);
                        Cv2.CvtColor(tmp8, dst, ColorConversionCodes.GRAY2BGR);
                    }
                    finally { tmp8.Dispose(); }
                }
            }
            else if (working.Channels() == 4 && working.Depth() == MatType.CV_8U)
            {
                Cv2.CvtColor(working, dst, ColorConversionCodes.BGRA2BGR);
            }
            else
            {
                var tmp8 = new Mat();
                try
                {
                    if (working.Channels() == 3)
                    {
                        working.ConvertTo(tmp8, MatType.CV_8UC3);
                        tmp8.CopyTo(dst);
                    }
                    else
                    {
                        Cv2.Normalize(working, tmp8, 0, 255, NormTypes.MinMax);
                        tmp8.ConvertTo(tmp8, MatType.CV_8U);
                        Cv2.CvtColor(tmp8, dst, ColorConversionCodes.GRAY2BGR);
                    }
                }
                finally { tmp8.Dispose(); }
            }
            return dst;
        }

        public void DrawResult( )
        {
            if (_disposed) return;

            // 1) Lấy frame nguồn
            Mat src;
            lock (_camLock)
            {
                src = matRaw?.Clone();
            }
            if (src == null || src.Empty() || src.Width <= 0 || src.Height <= 0)
            {
                src?.Dispose();
                return;
            }

            // 2) Chuẩn bị buffer
            Mat working;
            lock (_swapLock)
            {
                working = EnsureWorkingBuffer(src);
                src.CopyTo(working);
            }
            src.Dispose();

            // 3) Convert -> Bitmap & vẽ overlay
            using (Mat bgr = EnsureBgr8Uc3AliasOrConvert(working, out bool createdTemp))
            {
                Bitmap canvas = null;
                try
                {
                    canvas = BitmapConverter.ToBitmap(bgr);

                    using (var g = Graphics.FromImage(canvas))
                    using (var xf = new Matrix())
                    {
                        g.SmoothingMode = SmoothingMode.None;
                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        g.CompositingQuality = CompositingQuality.HighSpeed;
                        g.PixelOffsetMode = PixelOffsetMode.Half;

                        xf.Translate(Global.pScroll.X, Global.pScroll.Y);
                        float s = Global.ScaleZoom;
                        //try
                        //{
                        //    var pi = imgView.GetType().GetProperty("Zoom");
                        //    if (pi != null) s = Convert.ToSingle(pi.GetValue(imgView)) / 100f;
                        //}
                       // catch { }
                        xf.Scale(s, s);
                        g.Transform = xf;

                        var tools = BeeCore.Common.PropetyTools[IndexCCD];
                        foreach (var tool in tools)
                            if (tool.UsedTool != UsedTool.NotUsed)
                                tool.Propety.DrawResult(g);

                        //String Content = "OK Date:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //     if (!Global.TotalOK)
                        //    Content = "NG Date:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //g.DrawString(Content, new Font("Arial", 12, FontStyle.Regular),new SolidBrush(Color.WhiteSmoke),new Point(10,10));
                    }

                    // 4) Tạo bmResult bằng copy pixel data trực tiếp từ canvas
                    Bitmap storeCopy = new Bitmap(canvas.Width, canvas.Height, canvas.PixelFormat);
                    using (var gCopy = Graphics.FromImage(storeCopy))
                    {
                        gCopy.DrawImageUnscaled(canvas, 0, 0);
                    }

                    lock (_bmLock)
                    {
                       bmResult?.Dispose();
                       bmResult = storeCopy;
                        bmResult.Save("Result"+ IndexCCD + ".png");
                    }
                    canvas = null; // tránh dispose ở finally
                    //// 5) Dùng chính canvas cho UI (không clone lại)
                    //if (imgView.IsHandleCreated && !imgView.IsDisposed)
                    //{
                    //    if (imgView.InvokeRequired)
                    //    {
                    //        var uiBmp = canvas; // giữ canvas cho UI
                    //        canvas = null; // tránh dispose ở finally
                    //        imgView.BeginInvoke(new Action(() =>
                    //        {
                    //            if (imgView.IsDisposed) { uiBmp.Dispose(); return; }
                    //            var oldUi = imgView.Image;
                    //            imgView.Image = uiBmp;
                    //            oldUi?.Dispose();
                    //        }));
                    //    }
                    //    else
                    //    {
                    //        var oldUi = imgView.Image;
                    //        imgView.Image = canvas;
                    //        oldUi?.Dispose();
                    //        canvas = null; // tránh dispose ở finally
                    //    }
                    //}

                    // 6) Xác nhận buffer hiển thị
                    lock (_swapLock)
                    {
                        _displayMat = working;
                    }
                }
                finally
                {
                    canvas?.Dispose();
                }
            }
        }
        public  bool Status()
        {

            return CCDPlus.IsErrCCD;
        }
        public void DisConnect()
        {
            CCDPlus.DestroyAll(IndexCCD);
        }
        public int IndexConnect = -1;
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
            if (CCDPlus.Connect(IndexConnect, NameCCD))
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
            if (Para.TypeCamera == TypeCamera.USB)
            {
                CCDPlus.SetWidth((int)Para.Width.Value);
                CCDPlus.SetHeight((int)Para.Height.Value);
                await Task.Delay(500);
                CCDPlus.SetFocus((int)Para.Focus);
                CCDPlus.SetZoom((int)Para.Zoom);

            }
            else
            {
                await SetWidth();
                await SetHeight();
                await SetOffSetX();
                await SetOffSetY();
                await SetCenterX();
                await SetCenterY();
                await SetExpo();
                await SetGain();
                await SetShift();
            }
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
                                Para.Exposure.Value = await Task.Run(() => CCDPlus.SetParaFloat(IndexCCD, "ExposureTime", Para.Exposure.Value), cancel.Token);
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

                cancel = new CancellationTokenSource(2000);
                switch (Para.TypeCamera)
                {
                    case TypeCamera.BaslerGigE:
                       
                        switch (TypeCCD)
                        {
                            case 0://Basler
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "ExposureTimeRaw", ref Para.Exposure.Min, ref Para.Exposure.Max, ref Para.Exposure.Step, ref Para.Exposure.Value), cancel.Token);
                               
                                break;
                            case 1://Hik
                                return await Task.Run(() => CCDPlus.GetParaFloat(IndexCCD, "ExposureTime", ref Para.Exposure.Min, ref Para.Exposure.Max, ref Para.Exposure.Step, ref Para.Exposure.Value), cancel.Token);

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
                                Para.Gain.Value = await Task.Run(() => CCDPlus.SetParaFloat(IndexCCD, "Gain", Para.Gain.Value), cancel.Token);

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
                                Para.Shift.Value = await Task.Run(() => CCDPlus.SetParaFloat(IndexCCD, "DigitalShift", Para.Shift.Value), cancel.Token);
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
                                return await Task.Run(() => CCDPlus.GetParaFloat(IndexCCD, "DigitalShift", ref Para.Shift.Min, ref Para.Shift.Max, ref Para.Shift.Step, ref Para.Shift.Value), cancel.Token);

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
        public void SetWidthUSB()
        {
            CCDPlus.SetWidth((int)Para.Width.Value);
        }
        public void SetHeightUSB()
        {
            CCDPlus.SetHeight((int)Para.Height.Value);
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
                                Para.Width.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "Width", Para.Width.Value), cancel.Token);
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
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "Width", ref Para.Width.Min, ref Para.Width.Max, ref Para.Width.Step, ref Para.Width.Value), cancel.Token);
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
                                Para.Height.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "Height", Para.Height.Value), cancel.Token);
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
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "Height", ref Para.Height.Min, ref Para.Height.Max, ref Para.Height.Step, ref Para.Height.Value), cancel.Token);
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
                                Para.OffSetX.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "OffsetX", Para.OffSetX.Value), cancel.Token);
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
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "OffsetX", ref Para.OffSetX.Min, ref Para.OffSetX.Max, ref Para.OffSetX.Step, ref Para.OffSetX.Value), cancel.Token);
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
                                Para.OffSetY.Value = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "OffsetY", Para.OffSetY.Value), cancel.Token);
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
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "OffsetY", ref Para.OffSetY.Min, ref Para.OffSetY.Max, ref Para.OffSetY.Step, ref Para.OffSetY.Value), cancel.Token);
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
        public bool IsTrigger = false;
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
                                Para.CenterX = (int)(await Task.Run(() => CCDPlus.SetPara(IndexCCD, "CenterX", (int)Para.CenterX), cancel.Token));
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
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "CenterX", ref none, ref none, ref none, ref Para.CenterX), cancel.Token);
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
                                Para.CenterY = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "CenterY", Para.CenterY), cancel.Token);
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
                                return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "CenterY", ref none, ref none, ref none, ref Para.CenterY), cancel.Token);
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
                                return await Task.Run(() => CCDPlus.GetParaFloat(IndexCCD, "Gain", ref Para.Gain.Min, ref Para.Gain.Max, ref Para.Gain.Step, ref Para.Gain.Value), cancel.Token);
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
        public void SetFormat()
        {
          //  CCDPlus.set();
        }
        public  void Init()
        {
           
            CCDPlus.TypeCamera = (int)Para.TypeCamera;
        }
        [NonSerialized]
        Stopwatch stopwatch = new Stopwatch();
        private CameraIOFast cameraIOFast = new CameraIOFast();
        public unsafe bool TryGrabFast_NoStride(ref Mat matRaw)
        {



            IntPtr intPtr = IntPtr.Zero;
            int rows = 0, cols = 0;
            int matType = MatType.CV_8UC1;

            try
            {
                //stopwatch.Restart();
                intPtr = new IntPtr (CCDPlus.ReadCCD(IndexCCD, &rows, &cols,&matType));
               // stopwatch.Stop();
              //  BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;
                // intPtr = Native.GetRaw(ref rows, ref cols, ref matType);
                if (intPtr == IntPtr.Zero || rows <= 0 || cols <= 0)
                    return false;

                // Allocate/reuse destination Mat

                if (matRaw == null || matRaw.Rows != rows || matRaw.Cols != cols || matRaw.Type() != matType||matRaw.IsDisposed)
                {
                    matRaw?.Dispose();
                    matRaw = new Mat(rows, cols, matType);
                }

                byte* src = (byte*)intPtr;
                byte* dst = (byte*)matRaw.Data;

                int elem = (int)matRaw.ElemSize();
                long bytesPerRow = (long)cols * elem;
                long dstStep = (long)matRaw.Step();      // có thể >= bytesPerRow do alignment

                // Copy từng dòng để an toàn với step của đích
                long copyBytes = Math.Min(bytesPerRow, dstStep);
                for (int r = 0; r < rows; r++)
                {
                    Buffer.MemoryCopy(src + r * bytesPerRow,
                                      dst + r * dstStep,
                                      dstStep,
                                      copyBytes);
                }

                if (Global.LogsDashboard == null) Global.LogsDashboard = new LogsDashboard();
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "ReadCCD", "OK"));
                return true;
            }
            catch (Exception ex)
            {
                if (Global.LogsDashboard == null) Global.LogsDashboard = new LogsDashboard();
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadCCD", ex.Message));
                // Global.Ex ="CAMERAIOFAST_" +ex.Message;
                return false;
            }
            finally
            {
                if (intPtr != IntPtr.Zero)
                    Native.FreeBuffer(intPtr);
            }

        }
        public   void Read()
        {
         //   stopwatch.Restart();
         if(! TryGrabFast_NoStride(ref matRaw))
            {
                Global.CameraStatus = CameraStatus.ErrorConnect;
            }    
        else
           FrameRate = CCDPlus.FPS;
         //   BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;
            //int rows = 0, cols = 0, Type = 0;

            //Mat raw = new Mat();
            //IntPtr intPtr = IntPtr.Zero;

            //try
            //{     //  if (matRaw != null)
            ////            if (!matRaw.Empty())
            ////            matRaw.Release();
            //            switch (Para.TypeCamera)
            //    {
            //        case TypeCamera.USB:
                      
                     
                       

            //            //CCDPlus.ReadCCD(IndexCCD);
            //            TryGrabFast_NoStride(ref matRaw);
            //            //   Cv2.ImWrite("Raw" + IndexCCD + ".png", matRaw);
                      
            //            FrameRate = CCDPlus.FPS;
            //            //try
            //            //{

            //            //    unsafe
            //            //    {
            //            //        intPtr = new IntPtr(CCDPlus.ReadCCD(IndexCCD, &rows, &cols, &Type));
            //            //        // intPtr = Native.GetRaw(ref rows, ref cols, ref matType);
            //            //        if (intPtr == IntPtr.Zero || rows <= 0 || cols <= 0)
            //            //            return;


            //            //        raw = new Mat(rows, cols, Type, intPtr);

            //            //        FrameRate = CCDPlus.FPS;

            //            //        matRaw = raw.Clone();
            //            //        //   Cv2.ImWrite("Raw" + IndexCCD + ".png", matRaw);
            //            //        stopwatch.Stop();
            //            //        BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;
            //            //    }
            //            //    //    return new Mat();
            //            //    stopwatch.Stop();
            //            //    BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;
            //            //    FrameRate = CCDPlus.FPS;

            //            //}
            //            //finally
            //            //{
            //            //    raw.Release();
            //            //    Native.FreeBuffer(intPtr);
            //            //}
            //            break;
            //        case TypeCamera.BaslerGigE:

            //            //if (IsHist)
            //            //    CCDPlus.ReadRaw(true);
            //            //else

                       
            //            TryGrabFast_NoStride(ref matRaw);
                      
            //            // Cv2.ImWrite("Raw" + IndexCCD + ".png", matRaw);


            //            FrameRate = CCDPlus.FPS;
            //            // raw = new Mat();

            //            //try
            //            //{

            //            //    unsafe
            //            //    {
            //            //        intPtr = new IntPtr(CCDPlus.ReadCCD(IndexCCD, &rows, &cols, &Type));
            //            //        // intPtr = Native.GetRaw(ref rows, ref cols, ref matType);
            //            //        if (intPtr == IntPtr.Zero || rows <= 0 || cols <= 0)
            //            //            return;


            //            //        raw = new Mat(rows, cols, Type, intPtr);

            //            //        FrameRate = CCDPlus.FPS;

            //            //        matRaw = raw.Clone();

            //            //        stopwatch.Stop();
            //            //        BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;

            //            //    }


            //            //}
            //            //finally
            //            //{
            //            //    raw.Release();
            //            //    Native.FreeBuffer(intPtr);
            //            //}
            //            break;
            //           case TypeCamera.TinyIV:
            //            Mat raw2= HEROJE.Read();
            //            Size SZ = raw2.Size(); 
            //        if(SZ.Width==0&&SZ.Height==0)
            //                IsConnected = false;
            //            BeeCore.Common.CycleCamera = Convert.ToInt32(1000.0 / HEROJE.FrameTime);
            //             Native.SetImg(matRaw);
            //            break;
            //    }


            //    stopwatch.Stop();
            //    BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;

            //}
            //catch (Exception ex) {
                
            //        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadCCD", ex.Message));
            //}

          

            // return new Mat();
        }
        private Native Native = new Native();
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
