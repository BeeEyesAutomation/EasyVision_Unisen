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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using static BeeCore.Algorithm.FilletCornerMeasure;

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
        public  string[] Scan(TypeCamera TypeCamera)
        {switch(TypeCamera)
            {
                case TypeCamera.USB:
                    return CCDPlus.ScanCCD().Split('\n');
                   
                case TypeCamera.MVS:
                    return CCDPlus.ScanCCD().Split('\n'); 
                   
                case TypeCamera.Pylon:
                    if(PylonCam==null)
                    {
                        PylonCam = new PylonCli.Camera();
                    }    
                   return PylonCli. Camera.List();
                   
                case TypeCamera.TinyIV:
                    if (Global.ParaCommon.CardChoosed == null)
                        Global.ParaCommon.CardChoosed = "";
                    if (Global.ParaCommon.CardChoosed != "")
                    {
                        try
                        {
                            int[] IP = Global.ParaCommon.CardChoosed.Split('.').Select(int.Parse).ToArray();
                            return BeeCore.HEROJE.Scan(IP[0], IP[1], IP[2], IP[3]).Split('\n');

                        }
                        catch (Exception)
                        {
                            return BeeCore.HEROJE.Scan(192, 168, 2, 1).Split('\n');
                        }
                    }
                    else
                        return BeeCore.HEROJE.Scan(192, 168, 2, 1).Split('\n');
                    break;
            }
            return new string[1];





        }
        
        public  void DestroyAll()
        {
            if (Para.TypeCamera == TypeCamera.TinyIV)
            {
                 HEROJE.Disconnect();
                HEROJE.DisConnect();
            }
            switch (Para.TypeCamera)
            {
                case TypeCamera.USB:
                    CCDPlus.DestroyAll(IndexCCD,0);
                    break;
                case TypeCamera.MVS:
                    CCDPlus.DestroyAll(IndexCCD, 1);
                    break;
                case TypeCamera.Pylon:
                    PylonCam.Stop();
                   PylonCam.Close();
                    PylonCam.Dispose();
                    break;

            }
           
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
        public Results Results = Results.OK;
        public void SumResult()
        {
            Results = Results.OK;
            foreach (List<PropetyTool> PropetyTools in BeeCore.Common.PropetyTools)
            {
                foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[IndexCCD])
                {
                    if (PropetyTool.UsedTool == UsedTool.NotUsed)
                    {
                        continue;
                    }
                    if (PropetyTool.Results == Results.NG)
                    {
                        Results = Results.NG;
                        break;
                    }
                }
                if (Global.ParaCommon.IsMultiCamera == false)
                    break;
            }
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
              
                        xf.Scale(s, s);
                        g.Transform = xf;
                        g.DrawString(Global.TriggerNum.ToString(), new Font("Arial", 16), Brushes.DarkGray, new PointF(10, 10));
                        if (Global.TotalOK)
                            g.DrawString("OK", new Font("Arial", 24), Brushes.Green, new PointF(10,30));
                        else
                            g.DrawString("NG", new Font("Arial", 24), Brushes.Red, new PointF(10, 30));
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
                        String pathRaw, pathRS;
                        String date = DateTime.Now.ToString("yyyyMMdd");
                        String Hour = DateTime.Now.ToString("HHmmss");
                        pathRaw = "Report//" + date + "//Raw";
                        pathRS = "Report//" + date + "//Result";
                        if (!Directory.Exists(pathRaw))
                            Directory.CreateDirectory(pathRaw);
                        if (!Directory.Exists(pathRS))
                            Directory.CreateDirectory(pathRS);
                        if (Results == Results.OK && Global.Config.IsSaveOK || Results == Results.NG && Global.Config.IsSaveNG)
                        {
                            Mat matRs = bmResult.ToMat();
                            using (Mat raw = matRaw.Clone())
                            {


                                switch (Global.Config.TypeSave)
                                {
                                    case 1:
                                        Cv2.PyrDown(matRs, matRs);
                                        Cv2.PyrDown(matRs, matRs);
                                        Cv2.PyrDown(raw, raw);
                                        Cv2.PyrDown(raw, raw);
                                        break;
                                    case 2:
                                        Cv2.PyrDown(matRs, matRs);
                                        Cv2.PyrDown(raw, raw);
                                        break;
                                }

                                if (Global.Config.IsSaveRaw)
                                    Cv2.ImWrite(pathRaw + "//" + Global.Project + "_" + Results.ToString() + "_" + Hour + ".png", raw);
                                if (Global.Config.IsSaveRS)
                                    Cv2.ImWrite(pathRS + "//" + Global.Project + "_" + Results.ToString() + "_" + Hour + ".png", matRs);
                                matRs.Dispose();
                            }
                        }
                        //  bmResult.Save("Result"+ IndexCCD + ".png");
                    }
                    canvas = null; // tránh dispose ở finally
                 

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
        public void DisConnect(TypeCamera typeCamera)
        {
            CCDPlus.DestroyAll(IndexCCD, (int)typeCamera);
        }
        
        public int IndexConnect = -1;
        public  async Task<bool> Connect(String NameCCD , TypeCamera typeCamera)
        {switch(typeCamera)
            {
                case TypeCamera.TinyIV:
                    IsConnected = HEROJE.Connect(0);
                    return IsConnected;
                    break;
                case TypeCamera.MVS:
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

                        //if (!IsFail && Para.Width.Value > Para.Width.Min + 1 && Para.Height.Value > Para.Height.Min + 1)
                        //    await SetFullPara();

                        return true;
                    }
                    break;
                case TypeCamera.USB:
                    return CCDPlus.Connect(IndexConnect, NameCCD);
                    break;
                case TypeCamera.Pylon:
                    if (PylonCam == null)
                    {
                        PylonCam = new PylonCli.Camera();
                    }
                    PylonCam.Open(NameCCD);
                    bool IsConnect = PylonCam.IsOpen();
                    if (IsConnect)
                    {
                        PylonCam.SetOutputPixel(PylonCli. OutputPixel.Auto);
                        PylonCam.Start(PylonCli.GrabMode.UserLoop);



                    }
                 Global.Ex= PylonCam.LastError;


                    return IsConnect;
                    break;

            }    
          
          
            return false;

        }

       

    
    

        public async Task<bool> SetFullPara()

        {
            if (Para.Width.Value > Para.Width.Min + 1 && Para.Height.Value > Para.Height.Min + 1)
            {
                if (Para.TypeCamera == TypeCamera.USB)
                {
                    CCDPlus.SetWidth((int)Para.Width.Value);
                    CCDPlus.SetHeight((int)Para.Height.Value);
                    await Task.Delay(100);
                    CCDPlus.SetFocus((int)Para.Focus);
                    CCDPlus.SetZoom((int)Para.Zoom);
                    CCDPlus.SetExposure(-(int)Para.Exposure.Value);

                }
                else
                {
                    
                    await SetWidth();
                    await SetHeight();
                    await SetOffSetX();
                    await SetOffSetY();

                    await SetExpo();
                    await SetGain();
                    await SetShift();
                }
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
                if (FrameChanged != null&&value!=0)
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

                Global.IsSetPara = true;


                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        Para.Exposure.Value = PylonCam.SetExposure(Para.Exposure.Value);
                        break;
                    case TypeCamera.MVS:
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
                           
                        break;
                    case TypeCamera.USB:
                        CCDPlus.SetExposure(-(int)Para.Exposure.Value);
                     
                        break;
                }
            }
            catch (Exception ex)
            {
                Err += ex.Message;
                return false;// ex.Message;
            }
            Global.IsSetPara = false;
            return true;
        }
   
        public async Task< bool> GetExpo()
        {
            try
            {

                cancel = new CancellationTokenSource(2000);
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        PylonCam.GetExposure(out Para.Exposure.Min, out Para.Exposure.Max, out Para.Exposure.Step, out Para.Exposure.Value);
                        Err= PylonCam.LastError;
                        return true;
                        break;
                    case TypeCamera.MVS:
                       
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
                    case TypeCamera.USB:
                        Para.Exposure.Value=- CCDPlus.GetExposure();
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

                Global.IsSetPara = true;
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        Para.Gain.Value = PylonCam.SetGain(Para.Gain.Value);
                        break;
                    case TypeCamera.MVS:
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
            Global.IsSetPara = false;
            return true;
        }
        [NonSerialized]
        CancellationTokenSource cancel = new CancellationTokenSource(2000);
        public async Task<bool> SetShift()
        {
            try
            {

                Global.IsSetPara = true;
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        Para.Shift.Value = PylonCam.SetDigitalShift(Para.Shift.Value);
                        break;
                    case TypeCamera.MVS:
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
            Global.IsSetPara = false;
            return true;
        }
        public async Task< bool> GetShift()
        {
            try
            {
               
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        PylonCam.GetDigitalShift(ref Para.Shift.Min, ref Para.Shift.Max, ref Para.Shift.Step, ref Para.Shift.Value);
                        return true;
                        break;
                    case TypeCamera.MVS:
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

                Global.IsSetPara = true;
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        Para.Width.Value = PylonCam.SetWidth(Para.Width.Value);
                        if (Para.Width.Value<10)
                        {
                            MessageBox.Show(PylonCam.LastError);
                        }    
                            break;
                    case TypeCamera.MVS:
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
            Global.IsSetPara = false;
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetWidth()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        PylonCam.GetWidth(out Para.Width.Min, out Para.Width.Max, out Para.Width.Step, out Para.Width.Value);
                        return true;
                        break;
                    case TypeCamera.MVS:
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
                Global.IsSetPara = true;


                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        Para.Height.Value = PylonCam.SetHeight(Para.Height.Value);
                        break;
                    case TypeCamera.MVS:
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
            Global.IsSetPara = false;
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetHeight()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        PylonCam.GetHeight(out Para.Height.Min, out Para.Height.Max, out Para.Height.Step, out Para.Height.Value);
                        return true;
                        break;
                    case TypeCamera.MVS:
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
                Global.IsSetPara = true;

                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        Para.OffSetX.Value = PylonCam.SetOffsetX(Para.OffSetX.Value);
                        break;
                    case TypeCamera.MVS:
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
            Global.IsSetPara = false;
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetOffSetX()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        PylonCam.GetOffsetX(out Para.OffSetX.Min, out Para.OffSetX.Max, out Para.OffSetX.Step, out Para.OffSetX.Value);
                        return true;
                        break;
                    case TypeCamera.MVS:
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
                Global.IsSetPara = true;

                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        Para.OffSetY.Value = PylonCam.SetOffsetY(Para.OffSetY.Value);
                        break;
                    case TypeCamera.MVS:
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
            Global.IsSetPara = false;
            return true;// Result.Success.ToString();
        }
        public async Task<bool> GetOffSetY()
        {
            try
            {
                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                        PylonCam.GetOffsetY(out Para.OffSetY.Min, out Para.OffSetY.Max, out Para.OffSetY.Step, out Para.OffSetY.Value);
                        return true;
                        break;
                    case TypeCamera.MVS:
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
        //public async Task<bool> SetCenterX()
        //{
        //    try
        //    {


        //        switch (Para.TypeCamera)
        //        {
        //            case TypeCamera.MVS:
        //                cancel = new CancellationTokenSource(2000);
        //                switch (TypeCCD)
        //                {
        //                    case 0://Basler
        //                        Para.CenterX = (int)(await  Task.Run(() => CCDPlus.SetPara(IndexCCD, "CenterX",(int) Para.CenterX), cancel.Token));
        //                        //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
        //                        break;
        //                    case 1://Hik
        //                        Para.CenterX = (int)(await Task.Run(() => CCDPlus.SetPara(IndexCCD, "CenterX", (int)Para.CenterX), cancel.Token));
        //                        break;
        //                }
        //                break;
        //            case TypeCamera.TinyIV:
        //                // HEROJE.SetExposure(value);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Err += ex.Message;
        //        return false;// ex.Message;
        //    }
        //    return true;// Result.Success.ToString();
        //}
        //public async Task<bool> GetCenterX()
        //{
        //    try
        //    {
        //        switch (Para.TypeCamera)
        //        {
        //            case TypeCamera.MVS:
        //                cancel = new CancellationTokenSource(2000);
        //                switch (TypeCCD)
        //                {
        //                    case 0://Basler
        //                        return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "CenterX", ref none, ref none, ref none, ref Para.CenterX), cancel.Token);

        //                        break;
        //                    case 1://Hik
        //                        return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "CenterX", ref none, ref none, ref none, ref Para.CenterX), cancel.Token);
        //                        break;
        //                }

        //                return true;
                
        //                break;
                      
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //    return false;
        //}

        //public async Task<bool> SetCenterY()
        //{
        //    try
        //    {


        //        switch (Para.TypeCamera)
        //        {
        //            case TypeCamera.MVS:
        //                cancel = new CancellationTokenSource(2000);
        //                switch (TypeCCD)
        //                {
        //                    case 0://Basler
        //                        Para.CenterY = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "CenterY", Para.CenterY), cancel.Token);
        //                        //  Para.Shift.Value = CCDPlus.SetPara(IndexCCD, "DigitalShift", Para.Shift.Value);
        //                        break;
        //                    case 1://Hik
        //                        Para.CenterY = await Task.Run(() => CCDPlus.SetPara(IndexCCD, "CenterY", Para.CenterY), cancel.Token);
        //                        break;
        //                }
        //                break;
        //            case TypeCamera.TinyIV:
        //                // HEROJE.SetExposure(value);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Err += ex.Message;
        //        return false;// ex.Message;
        //    }
        //    return true;// Result.Success.ToString();
        //}
        //public async Task<bool> GetCenterY()
        //{
        //    try
        //    {
        //        switch (Para.TypeCamera)
        //        {
        //            case TypeCamera.MVS:
        //                cancel = new CancellationTokenSource(2000);
        //                switch (TypeCCD)
        //                {
        //                    case 0://Basler
        //                        return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "CenterY", ref none, ref none, ref none, ref Para.CenterY), cancel.Token);

        //                        break;
        //                    case 1://Hik
        //                        return await Task.Run(() => CCDPlus.GetPara(IndexCCD, "CenterY", ref none, ref none, ref none, ref Para.CenterY), cancel.Token);
        //                        break;
        //                }

        //                return true;

        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return false;
        //    }
        //    return false;
        //}
        public ParaCamera Para = new ParaCamera();
        public async Task< bool> GetGain()
        {
            try
            {


                switch (Para.TypeCamera)
                {
                    case TypeCamera.Pylon:
                       PylonCam.GetGain(out Para.Gain.Min, out Para.Gain.Max, out Para.Gain.Step, out Para.Gain.Value);
                        return true;
                        break;
                    case TypeCamera.MVS:
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
        public  void Init(TypeCamera typeCamera)
        {
           
            CCDPlus.TypeCamera = (int)typeCamera;
        }
        public void GetFpsPylon()
        {
            FrameRate = (int)PylonCam.GetMeasuredFps();

        }
        [NonSerialized]
        private  PylonCli.Camera PylonCam;
        [NonSerialized]
        Stopwatch stopwatch = new Stopwatch();
        private CameraIOFast cameraIOFast = new CameraIOFast();
        //public unsafe bool TryGrabFast_NoStride(ref Mat matRaw)
        //{
        //    IntPtr intPtr = IntPtr.Zero;   // buffer từ native (MVS/USB)
        //    IntPtr pylonPtr = IntPtr.Zero; // buffer từ Pylon (khác hàm free)
        //    int rows = 0, cols = 0;
        //    int matTypeCode = (int)MatType.CV_8UC1; // native trả về code; ta cast về MatType khi tạo Mat
        //    int srcStride = 0;   // stride (bytes/row) của nguồn nếu có

        //    try
        //    {
        //        // Nếu đang ở chế độ chỉ set tham số (không grab), nên return false thay vì true
        //        // để caller biết chưa có frame mới. Nếu bạn cố tình muốn "OK", giữ true.
        //        if (Global.IsSetPara)
        //            return false;

        //        switch (Para.TypeCamera)
        //        {
        //            case TypeCamera.MVS:
        //            case TypeCamera.USB:
        //                {
        //                    // Read từ DLL: trả về con trỏ + fill rows/cols/matTypeCode
        //                    intPtr = new IntPtr(CCDPlus.ReadCCD(IndexCCD, &rows, &cols, &matTypeCode));
        //                    FrameRate = CCDPlus.FPS;

        //                    if (intPtr == IntPtr.Zero || rows <= 0 || cols <= 0)
        //                        return false;

        //                    // Stride nguồn nếu API có (giả sử CCDPlus có hàm trả stride; nếu không, dùng packed)
        //                    srcStride = 0;// CCDPlus.GetStride != null ? CCDPlus.GetStride(IndexCCD) : cols * (int)new Mat(rows, cols, (MatType)matTypeCode).ElemSize();

        //                    // Đảm bảo matRaw đúng kích thước & kiểu
        //                    EnsureMat(ref matRaw, rows, cols, (MatType)matTypeCode);

        //                    // Copy từng dòng, tôn trọng stride nguồn & đích
        //                    CopyRows((byte*)intPtr, matRaw, rows, cols);
        //                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "ReadCCD", "OK"));
        //                    return true;
        //                }

        //            case TypeCamera.Pylon:
        //                {
        //                    int w = 0, h = 0, s = 0, c = 0;
        //                    pylonPtr = PylonCam.GrabOneUcharPtr(1000, out w, out h, out s, out c);
        //                    if (pylonPtr == IntPtr.Zero || w <= 0 || h <= 0)
        //                    {
        //                        Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadCCD", PylonCam.LastError));
        //                        return false;
        //                    }

        //                    var mt = (c == 1) ? MatType.CV_8UC1 : MatType.CV_8UC3;
        //                    FrameRate = (int)PylonCam.GetMeasuredFps();

        //                    // Cấp phát (hoặc reuse) đích đúng size/kiểu
        //                    EnsureMat(ref matRaw, h, w, mt);

        //                    // Copy theo stride nguồn s và step đích
        //                    CopyRows((byte*)pylonPtr, matRaw, h, w, s);

        //                    // Nếu SDK yêu cầu trả buffer (tuỳ API của bạn), gọi release ở đây
        //                    // PylonCam.ReleaseBuffer(); // nếu có
        //                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "ReadCCD", "OK"));
        //                    return true;
        //                }
        //        }

        //        // Nếu rơi ra ngoài switch (loại camera chưa hỗ trợ)
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.LogsDashboard = Global.LogsDashboard ?? new LogsDashboard();
        //        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadCCD", ex.Message));
        //        return false;
        //    }
        //    finally
        //    {
        //        // Chỉ free buffer thuộc về CCDPlus nếu API yêu cầu bạn giải phóng
        //        if (intPtr != IntPtr.Zero)
        //            Native.FreeBuffer(intPtr);

        //        // Với pylonPtr: thường là buffer thuộc SDK; chỉ release nếu SDK yêu cầu.
        //        // if (pylonPtr != IntPtr.Zero) PylonCam.ReleaseBuffer(pylonPtr);
        //    }
        //}

        /// <summary>
        /// Đảm bảo mat != null, chưa Dispose, và có đúng size/type; nếu khác thì Dispose + tạo mới.
        /// </summary>
        private static void EnsureMat(ref Mat m, int rows, int cols, MatType type)
        {
            if (m == null || m.IsDisposed || m.Rows != rows || m.Cols != cols || m.Type() != type)
            {
                m?.Dispose();
                m = new Mat(rows, cols, type);
            }
        }

        /// <summary>
        /// Copy theo từng dòng, tôn trọng stride nguồn & step đích.
        /// Nếu không truyền stride nguồn (srcStride=0), suy ra packed: srcStride = cols * elemSize.
        /// </summary>
        private static unsafe void CopyRows(byte* srcBase, Mat dst, int rows, int cols, int srcStride = 0)
        {
            int elem = (int)dst.ElemSize();               // bytes per pixel
            long dstStep = (long)dst.Step();              // bytes per row ở đích
            long bytesPerRow = (long)cols * elem;

            if (srcStride <= 0) srcStride = (int)bytesPerRow;

            // Không copy quá giới hạn step đích
            long copyCount = bytesPerRow <= dstStep ? bytesPerRow : dstStep;

            byte* dstBase = (byte*)dst.DataPointer;

            for (int r = 0; r < rows; r++)
            {
                byte* src = srcBase + (long)r * srcStride;
                byte* dstRow = dstBase + (long)r * dstStep;
                Buffer.MemoryCopy(src, dstRow, dstStep, copyCount);
            }
        }

        public unsafe bool TryGrabFast_NoStride(ref Mat matRaw)
        {



            IntPtr intPtr = IntPtr.Zero;
            int rows = 0, cols = 0;
            int matType = MatType.CV_8UC1;

            try
            {
                if (Global.IsSetPara)
                    return true;
                switch (Para.TypeCamera)
                {
                    case TypeCamera.MVS:
                        intPtr = new IntPtr(CCDPlus.ReadCCD(IndexCCD, &rows, &cols, &matType));
                        FrameRate = CCDPlus.FPS;
                        break;
                    case TypeCamera.USB:
                        intPtr = new IntPtr(CCDPlus.ReadCCD(IndexCCD, &rows, &cols, &matType));
                        FrameRate = CCDPlus.FPS;
                        break;
                    case TypeCamera.Pylon:
                        if (matRaw == null || matRaw.Rows != rows || matRaw.Cols != cols || matRaw.Type() != matType || matRaw.IsDisposed)
                        {
                            matRaw?.Dispose();
                            matRaw = new Mat(rows, cols, matType);
                        }
                        int w = 0, h = 0, s = 0, c = 0;
                        IntPtr p = PylonCam.GrabOneUcharPtr(1000, out w, out h, out s, out c);
                        matType = (c == 1) ? OpenCvSharp.MatType.CV_8UC1 : OpenCvSharp.MatType.CV_8UC3;
                        FrameRate = (int)PylonCam.GetMeasuredFps();
                        matRaw = new Mat(h, w, matType); // hoặc CV_8UC1 nếu Mono
                        unsafe
                        {
                            Buffer.MemoryCopy(p.ToPointer(), matRaw.DataPointer, (long)h * w * c, (long)h * w * c);
                        }
                        if (p == IntPtr.Zero)
                        {
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadCCD", PylonCam.LastError));
                            return false;
                        }
                        return true;
                       
                        break;
                }

                // stopwatch.Stop();
                //  BeeCore.Common.CycleCamera = (int)stopwatch.Elapsed.TotalMilliseconds;
                // intPtr = Native.GetRaw(ref rows, ref cols, ref matType);
                if (intPtr == IntPtr.Zero || rows <= 0 || cols <= 0)
                {

                    return false;

                }
                if (matRaw == null || matRaw.Rows != rows || matRaw.Cols != cols || matRaw.Type() != matType || matRaw.IsDisposed)
                {
                    matRaw?.Dispose();
                    matRaw = new Mat(rows, cols, matType);
                }

                // Allocate/reuse destination Mat



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
        public int numTry = 0;
        bool IsReadCCD = false;
       
        public   bool Read()
        {
       if(Global.IsSetPara)
                return false;
           
             if (!TryGrabFast_NoStride(ref matRaw))
                {
                
                    numTry++;
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "ReadCCD", "Retry is "+numTry));
               
                if (numTry >= 5)
                {
                    Global.CameraStatus = CameraStatus.ErrorConnect;
                    return true;

                }
                return true;

            }
                else
            {
              
                numTry = 0;
                   // FrameRate = CCDPlus.FPS;
                }
            return false;

         
        }

        private void PylonCam_FrameReady(IntPtr buffer, int width, int height, int stride, int channels)
        {
            throw new NotImplementedException();
        }

        private Native Native = new Native();
        public  void Light(int TypeLight, bool IsOn)
        {
            switch (Para.TypeCamera)
            {
                case TypeCamera.TinyIV:
                    HEROJE.Light(TypeLight, IsOn);
                    break;
                case TypeCamera.MVS:
                    Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.Light;
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
