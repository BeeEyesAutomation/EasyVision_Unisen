using BeeCore.Algorithm;
using BeeCore.Core;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using CvPlus;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using Python.Runtime;
using System;
using System.Buffers;
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
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeCore
{
    [Serializable()]
    public class Patterns
	{
        [NonSerialized]
        private object _runtimeLock = new object();

        private object RuntimeLock
        {
            get
            {
                if (_runtimeLock == null)
                    _runtimeLock = new object();
                return _runtimeLock;
            }
        }

        private static RectRotate CloneRectRotate(RectRotate src) => src?.Clone();

        private static List<HSV> CloneHSVList(List<HSV> src)
        {
            if (src == null) return null;
            return src.Select(h => h == null ? null : new HSV(h.H, h.S, h.V)).ToList();
        }

        private static List<RGB> CloneRGBList(List<RGB> src)
        {
            if (src == null) return null;
            return src.Select(c => c == null ? null : new RGB(c.R, c.G, c.B)).ToList();
        }

        private static List<Color> CloneColorList(List<Color> src)
        {
            return src == null ? null : new List<Color>(src);
        }

        private static List<Point[]> CloneContours(List<Point[]> src)
        {
            if (src == null) return null;
            return src.Select(arr => arr == null ? null : arr.ToArray()).ToList();
        }

        private static Bitmap CloneBitmap(Bitmap src)
        {
            return src == null ? null : src.Clone(new Rectangle(0, 0, src.Width, src.Height), src.PixelFormat);
        }

        private void ResetRuntimeStateForCopy()
        {
            Pattern = new BeeCpp.Pattern2();
            ColorAreaPP = new BeeCpp.ColorArea();
            ColorNgPP = new BeeCpp.ColorArea();
            ColorMaskPP = new BeeCpp.ColorArea();

            ResultItems = new List<ResultItem>();
            rectRotates = new List<RectRotate>();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            Postion = new List<Point>();
            ColorMaskContours = new List<Point[]>();

            if (matTemp != null && !matTemp.IsDisposed)
                matTemp.Dispose();
            matTemp = new Mat();

            if (bmRaw != null)
            {
                bmRaw.Dispose();
                bmRaw = null;
            }
        }

        public object Clone()
        {
            var clone = (Patterns)this.MemberwiseClone();
            clone._runtimeLock = new object();

            clone.rotArea = CloneRectRotate(rotArea);
            clone.rotCheck = CloneRectRotate(rotCheck);
            clone.rotCrop = CloneRectRotate(rotCrop);
            clone.rotMask = CloneRectRotate(rotMask);
            clone.rotAreaTemp = CloneRectRotate(rotAreaTemp) ?? new RectRotate();
            clone.rotAreaAdjustment = CloneRectRotate(rotAreaAdjustment);
            clone.rotMaskAdjustment = CloneRectRotate(rotMaskAdjustment);
            clone.rotPositionAdjustment = CloneRectRotate(rotPositionAdjustment);

            clone.MaskHSVs = CloneHSVList(MaskHSVs);
            clone.MaskRGBs = CloneRGBList(MaskRGBs);
            clone.NgHSVs = CloneHSVList(NgHSVs);
            clone.NgRGBs = CloneRGBList(NgRGBs);
            clone.listCLMaskShow = CloneColorList(listCLMaskShow) ?? new List<Color>();
            clone.listCLNgShow = CloneColorList(listCLNgShow) ?? new List<Color>();
            clone.ColorMaskContours = CloneContours(ColorMaskContours);

            clone.bmRaw = CloneBitmap(bmRaw);

            clone.Pattern = new BeeCpp.Pattern2();
            clone.ColorAreaPP = new BeeCpp.ColorArea();
            clone.ColorMaskPP = new BeeCpp.ColorArea();
            clone.ColorNgPP = new BeeCpp.ColorArea();
            clone.ResultItems = new List<ResultItem>();
            clone.rectRotates = new List<RectRotate>();
            clone.listP_Center = new List<System.Drawing.Point>();
            clone.list_AngleCenter = new List<float>();
            clone.Postion = Postion != null ? new List<Point>(Postion) : new List<Point>();
            clone.matTemp = new Mat();

            return clone;
        }
        public List<Point[]> ColorMaskContours { get; set; }
        public int ColorMaskTemplateWidth { get; set; }
        public int ColorMaskTemplateHeight { get; set; }
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
        [NonSerialized]
        public int MaxThread = 1;
        public bool IsIni = false;
        public int Index = -1;
        public SearchPattern SearchPattern = SearchPattern.BestObj;
        public RectRotate rotArea,rotCheck, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public MethordEdge MethordEdge = MethordEdge.CloseEdges;
        public int ThresholdBinary;
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
        public bool EnableColorCheck = false;
        public int ScoreNG = 0;
        public int ExtractionMask = 0;
        public int ExtractionNG = 0;
        public int ColorNgOffsetLeft = 0;
        public int ColorNgOffsetTop = 0;
        public int ColorNgOffsetRight = 0;
        public int ColorNgOffsetBottom = 0;
        public bool IsGetColor = false;
        public ColorGp TypeColor = ColorGp.HSV;
        public List<HSV> MaskHSVs;
        public List<RGB> MaskRGBs;
        public List<Color> listCLMaskShow = new List<Color>();
        public List<HSV> NgHSVs;
        public List<RGB> NgRGBs;
        public List<Color> listCLNgShow = new List<Color>();

        public int IndexCCD = 0;
        public string AddPLC = "";
        public TypeSendPLC TypeSendPLC = TypeSendPLC.Float;
        [NonSerialized]
        public Mat matTemp;
        public List<Point> Postion=new List<Point>();
       private Mode _TypeMode=Mode.Pattern;


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
        int _NumObject = 1;
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
        public int LimitCounter =1;
        public bool IsSendResult;
        public async Task SendResult()
        {
            List<System.Drawing.Point> points;
            List<float> angles;
            if (IsSendResult)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                    lock (RuntimeLock)
                    {
                        points = listP_Center != null ? new List<System.Drawing.Point>(listP_Center) : new List<System.Drawing.Point>();
                        angles = list_AngleCenter != null ? new List<float>(list_AngleCenter) : new List<float>();
                    }
                    int i = 0; 
                    int Add=(int)Converts.StringtoDouble(AddPLC);
                    String sAdd = Converts.BeforeFirstDigit(AddPLC);
                    foreach (System.Drawing.Point point in points)
                    {
                        if (i >= angles.Count) break;
                        String Address = sAdd + Add;
                        float[] floats = new float[4] { point.X, point.Y, angles[i],0 };
                        await Global.Comunication.Protocol.WriteResultFloatArr(AddPLC, floats);
                        Add += 8;
                         i++;
                    }
                }
            }
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
        [NonSerialized]
        public bool IsNew = false;
        private bool isAutoTrig;
        private int numOK;
        private int delayTrig;
        public List< System.Drawing.Point > listP_Center=new List<System.Drawing.Point>();
        [NonSerialized]
        public BeeCpp.Pattern2 Pattern =new BeeCpp.Pattern2();
        [NonSerialized]
        public BeeCpp.ColorArea ColorAreaPP = new BeeCpp.ColorArea();
        [NonSerialized]
        public BeeCpp.ColorArea ColorMaskPP = new BeeCpp.ColorArea();
        [NonSerialized]
        public BeeCpp.ColorArea ColorNgPP = new BeeCpp.ColorArea();
        [NonSerialized]
        public HSVCli hSV;
        [NonSerialized]
        public RGBCli rGB;
        [NonSerialized]
        public List<ResultItem> ResultItems = new List<ResultItem>();
        public Patterns()
        {
           
        }
        public static void LoadEdge()
        {
            if (G.IniEdge) return;
           
        }
        public int MaxObject = 1;
        public float StepAngle = 0;
        [NonSerialized]
        Mat matProcess = new Mat();
        public Mat LearnPattern(Mat raw, bool IsNoCrop)
        {
            lock (RuntimeLock)
            {

            using (Mat img = raw)
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
                    if (IsNoCrop)
                    {
                        Pattern.SetImgeSampleNoCrop(img.Data, img.Width, img.Height, (int)img.Step(), img.Channels());
                    }
                    else
                    {
                        intpr = Pattern.SetImgeSample(img.Data, img.Width, img.Height, (int)img.Step(), img.Channels(), rrCli, rrMaskCli, IsNoCrop,
                             out w, out h, out s, out c);
                        if (intpr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                            return mat; // trả Mat rỗng
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
                    }



                    Pattern.LearnPatternStable();
                    if (!IsNoCrop)
                    {
                        if (mat != null && !mat.Empty())
                        {
                            LearnColorMaskContours(mat);
                        }
                        else
                        {
                            ColorMaskContours = null;
                        }
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

        private void EnsureColorEngine()
        {
            if (ColorAreaPP == null)
                ColorAreaPP = new BeeCpp.ColorArea();
            if (ColorMaskPP == null)
                ColorMaskPP = new BeeCpp.ColorArea();
            if (ColorNgPP == null)
                ColorNgPP = new BeeCpp.ColorArea();
        }

        public System.Drawing.Color GetColor(Mat raw, int x, int y)
        {
            EnsureColorEngine();
            using (Mat mat = raw.Clone())
            {
                if (mat.Empty()) return System.Drawing.Color.Empty;
                if (mat.Type() == MatType.CV_8UC1)
                    Cv2.CvtColor(mat, mat, ColorConversionCodes.GRAY2BGR);

                ColorAreaPP.SetImgeRaw(mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels());
                switch (TypeColor)
                {
                    case ColorGp.HSV:
                        hSV = ColorAreaPP.GetHSV(x, y);
                        if (hSV != null)
                            return HsvConvert.FromHsvOpenCv((byte)hSV.H, (byte)hSV.S, (byte)hSV.V);
                        break;
                    case ColorGp.RGB:
                        rGB = ColorAreaPP.GetRGB(x, y);
                        if (rGB != null)
                            return System.Drawing.Color.FromArgb(rGB.R, rGB.G, rGB.B);
                        break;
                }
            }

            return System.Drawing.Color.Empty;
        }

        private void AddColorCore(List<HSV> hsvs, List<RGB> rgbs, List<Color> colors)
        {
            EnsureColorEngine();
            System.Drawing.Color clShow = System.Drawing.Color.Empty;
            switch (TypeColor)
            {
                case ColorGp.HSV:
                    if (hsvs == null)
                        hsvs = new List<HSV>();
                    if (hSV != null)
                    {
                        hsvs.Add(new HSV(hSV.H, hSV.S, hSV.V));
                        clShow = HsvConvert.FromHsvOpenCv((byte)hSV.H, (byte)hSV.S, (byte)hSV.V);
                    }
                    break;
                case ColorGp.RGB:
                    if (rgbs == null)
                        rgbs = new List<RGB>();
                    if (rGB != null)
                    {
                        rgbs.Add(new RGB(rGB.R, rGB.G, rGB.B));
                        clShow = System.Drawing.Color.FromArgb(rGB.R, rGB.G, rGB.B);
                    }
                    break;
            }

            if (clShow != System.Drawing.Color.Empty)
                colors.Add(clShow);
        }

        public void AddMaskColor()
        {
            if (MaskHSVs == null)
                MaskHSVs = new List<HSV>();
            if (MaskRGBs == null)
                MaskRGBs = new List<RGB>();
            if (listCLMaskShow == null)
                listCLMaskShow = new List<Color>();
            AddColorCore(MaskHSVs, MaskRGBs, listCLMaskShow);
        }

        public void AddNgColor()
        {
            if (NgHSVs == null)
                NgHSVs = new List<HSV>();
            if (NgRGBs == null)
                NgRGBs = new List<RGB>();
            if (listCLNgShow == null)
                listCLNgShow = new List<Color>();
            AddColorCore(NgHSVs, NgRGBs, listCLNgShow);
        }

        private void UndoColorCore(List<HSV> hsvs, List<RGB> rgbs)
        {
            switch (TypeColor)
            {
                case ColorGp.HSV:
                    if (hsvs != null && hsvs.Count > 0)
                        hsvs.RemoveAt(hsvs.Count - 1);
                    break;
                case ColorGp.RGB:
                    if (rgbs != null && rgbs.Count > 0)
                        rgbs.RemoveAt(rgbs.Count - 1);
                    break;
            }
        }

        public void UndoMaskColor()
        {
            UndoColorCore(MaskHSVs, MaskRGBs);
        }

        public void UndoNgColor()
        {
            UndoColorCore(NgHSVs, NgRGBs);
        }

        public void ClearMaskColors()
        {
            MaskHSVs = new List<HSV>();
            MaskRGBs = new List<RGB>();
            listCLMaskShow = new List<System.Drawing.Color>();
            SetMaskColor();
        }

        public void ClearNgColors()
        {
            NgHSVs = new List<HSV>();
            NgRGBs = new List<RGB>();
            listCLNgShow = new List<System.Drawing.Color>();
            SetNgColor();
        }

        private void SetColorCore(BeeCpp.ColorArea engine, List<HSV> hsvs, List<RGB> rgbs, int extraction)
        {
            EnsureColorEngine();
            switch (TypeColor)
            {
                case ColorGp.HSV:
                    if (hsvs != null)
                    {
                        HSVCli[] arrHSV = new HSVCli[hsvs.Count];
                        for (int i = 0; i < hsvs.Count; i++)
                        {
                            arrHSV[i] = new HSVCli
                            {
                                H = hsvs[i].H,
                                S = hsvs[i].S,
                                V = hsvs[i].V
                            };
                        }
                        engine.SetTempHSV(arrHSV, extraction);
                    }
                    break;
                case ColorGp.RGB:
                    if (rgbs != null)
                    {
                        RGBCli[] arrRGB = new RGBCli[rgbs.Count];
                        for (int i = 0; i < rgbs.Count; i++)
                        {
                            arrRGB[i] = new RGBCli
                            {
                                R = rgbs[i].R,
                                G = rgbs[i].G,
                                B = rgbs[i].B
                            };
                        }
                        engine.SetTempRGB(arrRGB, extraction);
                    }
                    break;
            }
        }

        public void SetMaskColor()
        {
            SetColorCore(ColorMaskPP, MaskHSVs, MaskRGBs, ExtractionMask);
        }

        public void SetNgColor()
        {
            SetColorCore(ColorNgPP, NgHSVs, NgRGBs, ExtractionNG);
        }

        private void ClearResultItems()
        {
            lock (RuntimeLock)
            {
                if (ResultItems == null)
                {
                    ResultItems = new List<ResultItem>();
                    return;
                }

                foreach (var item in ResultItems)
                {
                    if (item?.matProcess != null && !item.matProcess.IsDisposed)
                        item.matProcess.Dispose();

                    if (item?.ColorDebugOverlay != null && !item.ColorDebugOverlay.IsDisposed)
                        item.ColorDebugOverlay.Dispose();
                }

                ResultItems.Clear();
            }
        }
        private bool HasMaskColor()
        {
            return TypeColor == ColorGp.HSV
                ? MaskHSVs != null && MaskHSVs.Count > 0
                : MaskRGBs != null && MaskRGBs.Count > 0;
        }

        private bool HasNgColor()
        {
            return TypeColor == ColorGp.HSV
                ? NgHSVs != null && NgHSVs.Count > 0
                : NgRGBs != null && NgRGBs.Count > 0;
        }

        private void LearnColorMaskContours(Mat templateCrop)
        {
            if (!EnableColorCheck || templateCrop == null || templateCrop.Empty() || !HasMaskColor())
            {
                ColorMaskContours = null;
                ColorMaskTemplateWidth = 0;
                ColorMaskTemplateHeight = 0;
                return;
            }

            ColorMaskTemplateWidth = templateCrop.Width;
            ColorMaskTemplateHeight = templateCrop.Height;

            SetMaskColor();

            Mat rawMask = new Mat();
            try
            {
                CheckColorOnCrop(ColorMaskPP, templateCrop.Clone(), ref rawMask);
                ApplyColorMaskFilter(ref rawMask);

                if (rawMask == null || rawMask.Empty())
                {
                    ColorMaskContours = null;
                    ColorMaskTemplateWidth = 0;
                    ColorMaskTemplateHeight = 0;
                    return;
                }

                Cv2.FindContours(
                    rawMask,
                    out Point[][] contours,
                    out _,
                    RetrievalModes.External,
                    ContourApproximationModes.ApproxSimple);
              
                ColorMaskContours = (contours != null && contours.Length > 0)
                    ? new List<Point[]>(contours)
                    : null;
            }
            finally
            {
                if (rawMask != null && !rawMask.IsDisposed)
                    rawMask.Dispose();
            }
        }

        private Point[] ScaleContourToSize(Point[] contour, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            if (contour == null || contour.Length == 0)
                return Array.Empty<Point>();

            if (srcWidth <= 0 || srcHeight <= 0 || dstWidth <= 0 || dstHeight <= 0)
                return contour.Select(p => new Point(p.X, p.Y)).ToArray();

            float scaleX = (float)dstWidth / srcWidth;
            float scaleY = (float)dstHeight / srcHeight;
            float srcCenterX = (srcWidth - 1) / 2f;
            float srcCenterY = (srcHeight - 1) / 2f;
            float dstCenterX = (dstWidth - 1) / 2f;
            float dstCenterY = (dstHeight - 1) / 2f;

            return contour
                .Select(p => new Point(
                    Math.Max(0, Math.Min(dstWidth - 1, (int)Math.Round(((p.X - srcCenterX) * scaleX) + dstCenterX))),
                    Math.Max(0, Math.Min(dstHeight - 1, (int)Math.Round(((p.Y - srcCenterY) * scaleY) + dstCenterY)))))
                .ToArray();
        }

        private Rect ApplyColorNgOffsets(Rect bbox, int maxWidth, int maxHeight)
        {
            if (bbox.Width <= 0 || bbox.Height <= 0)
                return bbox;

            int left = Math.Max(0, ColorNgOffsetLeft);
            int top = Math.Max(0, ColorNgOffsetTop);
            int right = Math.Max(0, ColorNgOffsetRight);
            int bottom = Math.Max(0, ColorNgOffsetBottom);

            int x1 = bbox.X + left;
            int y1 = bbox.Y + top;
            int x2 = bbox.Right - right;
            int y2 = bbox.Bottom - bottom;

            x1 = Math.Max(0, Math.Min(maxWidth, x1));
            y1 = Math.Max(0, Math.Min(maxHeight, y1));
            x2 = Math.Max(0, Math.Min(maxWidth, x2));
            y2 = Math.Max(0, Math.Min(maxHeight, y2));

            int width = x2 - x1;
            int height = y2 - y1;
            if (width <= 0 || height <= 0)
                return new Rect();

            return new Rect(x1, y1, width, height);
        }
     
        private RectRotate ToAbsoluteRectRotate(RectRotate area, RectRotate local)
        {
            PointF[] pt = { new PointF(area._rect.X + local._PosCenter.X, area._rect.Y + local._PosCenter.Y) };
            using (var mat = new Matrix())
            {
                mat.Translate(area._PosCenter.X, area._PosCenter.Y);
                mat.Rotate(area._rectRotation);
                mat.TransformPoints(pt);
            }

            return new RectRotate(
                new RectangleF(-local._rect.Width / 2f, -local._rect.Height / 2f, local._rect.Width, local._rect.Height),
                pt[0],
                area._rectRotation + local._rectRotation,
                AnchorPoint.None)
            {
                Shape = local.Shape,
                Score = local.Score
            };
        }

        private ResultItem AddMatchResult(RectRotate area, RectRotate local, float score)
        {
            lock (RuntimeLock)
            {
                rectRotates.Add(local);
                rectRotates[rectRotates.Count - 1].Score = score;

                var absolute = ToAbsoluteRectRotate(area, local);
                var item = new ResultItem("Pattern")
                {
                    rot = absolute,
                    Score = score,
                    IsOK = true
                };
                ResultItems.Add(item);

                float angleCenter = absolute._rectRotation;
                list_AngleCenter.Add(angleCenter);
                listP_Center.Add(new System.Drawing.Point(
                    (int)(absolute._PosCenter.X / Scale),
                    (int)(absolute._PosCenter.Y / Scale)));

                return item;
            }
        }

        //private int CheckColorOnCrop(BeeCpp.ColorArea colorEngine, Mat crop, ref Mat colorMask)
        //{
        //    EnsureColorEngine();
        //    using (Mat src = crop)
        //    {
        //        if (src.Empty())
        //            return -1;

        //        Mat bgr = null;
        //        try
        //        {
        //            if (src.Type() == MatType.CV_8UC1)
        //            {
        //                bgr = new Mat();
        //                Cv2.CvtColor(src, bgr, ColorConversionCodes.GRAY2BGR);
        //            }
        //            else
        //            {
        //                bgr = src;
        //            }

        //            colorEngine.SetImgeNoCrop(
        //                bgr.Data, bgr.Width, bgr.Height, (int)bgr.Step(), bgr.Channels());

        //            GC.KeepAlive(bgr);

        //            int w, h, s, c;
        //            IntPtr ptr = colorEngine.Check(out w, out h, out s, out c);
        //            try
        //            {
        //                if (ptr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
        //                    return 0;

        //                MatType mt = c == 1 ? MatType.CV_8UC1
        //                    : c == 3 ? MatType.CV_8UC3
        //                    : MatType.CV_8UC4;

        //                using (var nativeMask = new Mat(h, w, mt, ptr, s))
        //                {
        //                    colorMask = nativeMask.Clone();
        //                }
        //            }
        //            finally
        //            {
        //                if (ptr != IntPtr.Zero)
        //                    colorEngine.FreeBuffer(ptr);
        //            }

        //            if (colorMask == null || colorMask.Empty())
        //                return 0;

        //            if (colorMask.Channels() != 1)
        //            {
        //                using (var gray = new Mat())
        //                {
        //                    if (colorMask.Channels() == 3)
        //                        Cv2.CvtColor(colorMask, gray, ColorConversionCodes.BGR2GRAY);
        //                    else
        //                        Cv2.CvtColor(colorMask, gray, ColorConversionCodes.BGRA2GRAY);

        //                    colorMask.Dispose();
        //                    colorMask = gray.Clone();
        //                }
        //            }
        //            // Hậu xử lý:
        //            if (IsClearNoiseSmall)
        //            {
        //                colorMask= Filters.ClearNoise(colorMask, SizeClearsmall);
        //               // if (!object.ReferenceEquals(t, colorMask)) { colorMask.Dispose(); colorMask = t; }
        //            }
        //            if (IsClose)
        //            {
        //                colorMask=Filters.Morphology(colorMask, MorphTypes.Close, new Size(SizeClose, SizeClose));
        //               // if (!object.ReferenceEquals(t, colorMask)) { colorMask.Dispose(); colorMask = t; }
        //            }
        //            if (IsOpen)
        //            {
        //                colorMask = Filters.Morphology(     colorMask, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
        //              //  if (!object.ReferenceEquals(t, colorMask)) { colorMask.Dispose(); colorMask = t; }
        //            }
        //            if (IsClearNoiseBig)
        //            {
        //                colorMask = Filters.ClearNoise(colorMask, SizeClearBig);
        //              //  if (!object.ReferenceEquals(t, colorMask)) { colorMask.Dispose(); colorMask = t; }
        //            }

        //            return Cv2.CountNonZero(colorMask);
        //        }
        //        finally
        //        {
        //            if (bgr != null && !object.ReferenceEquals(bgr, src))
        //                bgr.Dispose();
        //        }
        //    }
        //}
        private int CheckColorOnCrop(BeeCpp.ColorArea colorEngine, Mat crop, ref Mat colorMask)
        {
            EnsureColorEngine();
            using (Mat src = crop)
            {
                if (src.Empty())
                    return -1;

                Mat bgr = null;
                try
                {
                    if (src.Type() == MatType.CV_8UC1)
                    {
                        bgr = new Mat();
                        Cv2.CvtColor(src, bgr, ColorConversionCodes.GRAY2BGR);
                    }
                    else
                    {
                        bgr = src;
                    }

                    colorEngine.SetImgeNoCrop(
                        bgr.Data, bgr.Width, bgr.Height, (int)bgr.Step(), bgr.Channels());

                    GC.KeepAlive(bgr);

                    int w, h, s, c;
                    IntPtr ptr = colorEngine.Check(out w, out h, out s, out c);
                    try
                    {
                        if (ptr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                            return 0;

                        MatType mt = c == 1 ? MatType.CV_8UC1
                            : c == 3 ? MatType.CV_8UC3
                            : MatType.CV_8UC4;

                        using (var nativeMask = new Mat(h, w, mt, ptr, s))
                        {
                            colorMask = nativeMask.Clone();
                        }
                    }
                    finally
                    {
                        if (ptr != IntPtr.Zero)
                            colorEngine.FreeBuffer(ptr);
                    }

                    if (colorMask == null || colorMask.Empty())
                        return 0;

                    if (colorMask.Channels() != 1)
                    {
                        using (var gray = new Mat())
                        {
                            if (colorMask.Channels() == 3)
                                Cv2.CvtColor(colorMask, gray, ColorConversionCodes.BGR2GRAY);
                            else
                                Cv2.CvtColor(colorMask, gray, ColorConversionCodes.BGRA2GRAY);

                            colorMask.Dispose();
                            colorMask = gray.Clone();
                        }
                    }
                    // Hậu xử lý:
                    if (IsClearNoiseSmall)
                    {
                        Mat t = Filters.ClearNoise(colorMask, SizeClearsmall);
                        if (!object.ReferenceEquals(t, colorMask)) { colorMask.Dispose(); colorMask = t; }
                    }
                    if (IsClose)
                    {
                        Mat t = Filters.Morphology(colorMask, MorphTypes.Close, new Size(SizeClose, SizeClose));
                        if (!object.ReferenceEquals(t, colorMask)) { colorMask.Dispose(); colorMask = t; }
                    }
                    if (IsOpen)
                    {
                        Mat t = Filters.Morphology(colorMask, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                        if (!object.ReferenceEquals(t, colorMask)) { colorMask.Dispose(); colorMask = t; }
                    }
                    if (IsClearNoiseBig)
                    {
                        Mat t = Filters.ClearNoise(colorMask, SizeClearBig);
                        if (!object.ReferenceEquals(t, colorMask)) { colorMask.Dispose(); colorMask = t; }
                    }

                    return Cv2.CountNonZero(colorMask);
                }
                finally
                {
                    if (bgr != null && !object.ReferenceEquals(bgr, src))
                        bgr.Dispose();
                }
            }
        }

        private void ApplyColorMaskFilter(ref Mat mask)
        {
            if (mask == null || mask.Empty())
                return;

            if (mask.Channels() != 1)
            {
                using (var gray = new Mat())
                {
                    if (mask.Channels() == 3)
                        Cv2.CvtColor(mask, gray, ColorConversionCodes.BGR2GRAY);
                    else
                        Cv2.CvtColor(mask, gray, ColorConversionCodes.BGRA2GRAY);
                    mask.Dispose();
                    mask = gray.Clone();
                }
            }

            if (IsClearNoiseSmall && SizeClearsmall > 0)
            {
                Mat filtered = Filters.ClearNoise(mask, SizeClearsmall);
                if (!object.ReferenceEquals(filtered, mask))
                {
                    mask.Dispose();
                    mask = filtered;
                }
            }

            if (IsClose && SizeClose > 0)
            {
                Mat filtered = Filters.Morphology(mask, MorphTypes.Close, new Size(SizeClose, SizeClose));
                if (!object.ReferenceEquals(filtered, mask))
                {
                    mask.Dispose();
                    mask = filtered;
                }
            }

            if (IsOpen && SizeOpen > 0)
            {
                Mat filtered = Filters.Morphology(mask, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                if (!object.ReferenceEquals(filtered, mask))
                {
                    mask.Dispose();
                    mask = filtered;
                }
            }

            if (IsClearNoiseBig && SizeClearBig > 0)
            {
                Mat filtered = Filters.ClearNoise(mask, SizeClearBig);
                if (!object.ReferenceEquals(filtered, mask))
                {
                    mask.Dispose();
                    mask = filtered;
                }
            }
        }
        private bool TryBuildOffsetContourMask(
        Point[] contour,
        int width,
        int height,
        out Mat offsetMask,
        out Rect offsetRect,
        out Point[] offsetContour)
        {
            offsetMask = null;
            offsetRect = new Rect();
            offsetContour = null;

            if (contour == null || contour.Length == 0 || width <= 0 || height <= 0)
                return false;

            using (Mat mask = new Mat(height, width, MatType.CV_8UC1, Scalar.Black))
            {
                Cv2.DrawContours(
                    mask,
                    new[] { contour },
                    -1,
                    Scalar.White,
                    thickness: -1,
                    lineType: LineTypes.Link8);

                offsetMask = mask.Clone();

                ApplyDirectionalOffset(ref offsetMask, ColorNgOffsetLeft, Direction.Left);
                ApplyDirectionalOffset(ref offsetMask, ColorNgOffsetTop, Direction.Top);
                ApplyDirectionalOffset(ref offsetMask, ColorNgOffsetRight, Direction.Right);
                ApplyDirectionalOffset(ref offsetMask, ColorNgOffsetBottom, Direction.Bottom);

                if (offsetMask.Empty() || Cv2.CountNonZero(offsetMask) <= 0)
                {
                    offsetMask.Dispose();
                    offsetMask = null;
                    return false;
                }

                Cv2.FindContours(
                    offsetMask,
                    out Point[][] contours,
                    out _,
                    RetrievalModes.External,
                    ContourApproximationModes.ApproxSimple);

                if (contours == null || contours.Length == 0)
                {
                    offsetMask.Dispose();
                    offsetMask = null;
                    return false;
                }

                Point[] best = contours
                    .OrderByDescending(c => Math.Abs(Cv2.ContourArea(c)))
                    .FirstOrDefault();

                if (best == null || best.Length == 0)
                {
                    offsetMask.Dispose();
                    offsetMask = null;
                    return false;
                }

                offsetRect = Cv2.BoundingRect(best) & new Rect(0, 0, width, height);
                if (offsetRect.Width <= 0 || offsetRect.Height <= 0)
                {
                    offsetMask.Dispose();
                    offsetMask = null;
                    return false;
                }

                offsetContour = best;
                return true;
            }
        }
        private void ApplyDirectionalOffset(ref Mat mask, int offset, Direction direction)
        {
            if (offset == 0 || mask == null || mask.Empty())
                return;

            int pixels = Math.Abs(offset);

            Size kernelSize;
            Point anchor;

            switch (direction)
            {
                case Direction.Left:
                    kernelSize = new Size(pixels + 1, 1);
                    anchor = offset > 0
                        ? new Point(pixels, 0)   // offset dương: co từ trái vào
                        : new Point(0, 0);       // offset âm: nở sang trái
                    break;

                case Direction.Right:
                    kernelSize = new Size(pixels + 1, 1);
                    anchor = offset > 0
                        ? new Point(0, 0)        // offset dương: co từ phải vào
                        : new Point(pixels, 0);  // offset âm: nở sang phải
                    break;

                case Direction.Top:
                    kernelSize = new Size(1, pixels + 1);
                    anchor = offset > 0
                        ? new Point(0, pixels)   // offset dương: co từ trên xuống
                        : new Point(0, 0);       // offset âm: nở lên trên
                    break;

                case Direction.Bottom:
                    kernelSize = new Size(1, pixels + 1);
                    anchor = offset > 0
                        ? new Point(0, 0)        // offset dương: co từ dưới lên
                        : new Point(0, pixels);  // offset âm: nở xuống dưới
                    break;

                default:
                    return;
            }

            using (Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, kernelSize))
            {
                Mat result = new Mat();

                if (offset > 0)
                    Cv2.Erode(mask, result, kernel, anchor);
                else
                    Cv2.Dilate(mask, result, kernel, anchor);

                mask.Dispose();
                mask = result;
            }
        }

        private enum Direction
        {
            Left,
            Top,
            Right,
            Bottom
        }

        private void ApplyDirectionalErode(ref Mat mask, int pixels, Direction direction)
        {
            if (pixels <= 0 || mask == null || mask.Empty())
                return;

            Size kernelSize;
            Point anchor;

            switch (direction)
            {
                case Direction.Left:
                    kernelSize = new Size(pixels + 1, 1);
                    anchor = new Point(pixels, 0);
                    break;

                case Direction.Right:
                    kernelSize = new Size(pixels + 1, 1);
                    anchor = new Point(0, 0);
                    break;

                case Direction.Top:
                    kernelSize = new Size(1, pixels + 1);
                    anchor = new Point(0, pixels);
                    break;

                case Direction.Bottom:
                    kernelSize = new Size(1, pixels + 1);
                    anchor = new Point(0, 0);
                    break;

                default:
                    return;
            }

            using (Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, kernelSize))
            {
                Mat eroded = new Mat();
                Cv2.Erode(mask, eroded, kernel, anchor);

                mask.Dispose();
                mask = eroded;
            }
        }
        private void ApplyColorCheck(ResultItem item)
        {
            if (item == null)
                return;

            item.IsOK = true;
            item.ValueColor = 0;
            item.ColorMarkRect = Rectangle.Empty;
            item.ColorMarkContour = null;

            if (!EnableColorCheck || item.rot == null)
                return;

            bool hasNgColor = HasNgColor();

            using (Mat crop = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexCCD].matRaw, item.rot, null))
            {
                if (crop == null || crop.Empty())
                    return;

                if (item.matProcess != null && !item.matProcess.IsDisposed)
                    item.matProcess.Dispose();
                item.matProcess = null;

                if (item.ColorDebugOverlay != null && !item.ColorDebugOverlay.IsDisposed)
                    item.ColorDebugOverlay.Dispose();
                item.ColorDebugOverlay = null;

                if (hasNgColor && ColorMaskContours != null && ColorMaskContours.Count > 0)
                {
                    item.matProcess = new Mat();
                    SetNgColor();

                    Point[] maxContour = null;
                    double maxArea = 0;

                    foreach (var contour in ColorMaskContours)
                    {
                        if (contour == null || contour.Length == 0)
                            continue;

                        double area = Math.Abs(Cv2.ContourArea(contour));
                        if (area > maxArea)
                        {
                            maxArea = area;
                            maxContour = contour;
                        }
                    }

                    if (maxContour != null && maxContour.Length > 0)
                    {
                        Point[] scaledContour = ScaleContourToSize(
                            maxContour,
                            ColorMaskTemplateWidth,
                            ColorMaskTemplateHeight,
                            crop.Width,
                            crop.Height);

                        Point[][] contours = new Point[][] { scaledContour };

                        Mat fullOffsetMask = null;

                        try
                        {
                            if (TryBuildOffsetContourMask(
                                scaledContour,
                                crop.Width,
                                crop.Height,
                                out fullOffsetMask,
                                out Rect bbox,
                                out Point[] offsetContour))
                            {
                                item.ColorMarkRect = new Rectangle(bbox.X, bbox.Y, bbox.Width, bbox.Height);

                                // Vẽ đúng biên dạng chi tiết sau offset, không còn là rectangle nữa
                                item.ColorMarkContour = offsetContour;

                                using (Mat contourMask = new Mat(fullOffsetMask, bbox))
                                using (Mat cropRoi = new Mat(crop, bbox))
                                using (Mat matCheck = new Mat(bbox.Size, crop.Type(), Scalar.All(0)))
                                {
                                    cropRoi.CopyTo(matCheck, contourMask);

                                    int colorPixels = CheckColorOnCrop(ColorNgPP, matCheck, ref item.matProcess);
                                    item.ValueColor = Math.Max(0, colorPixels);
                                }
                            }
                        }
                        finally
                        {
                            if (fullOffsetMask != null && !fullOffsetMask.IsDisposed)
                                fullOffsetMask.Dispose();
                        }
                    }

                    item.IsOK = item.ValueColor <= ScoreNG;
                }
                else
                {
                    item.matProcess = new Mat();
                    item.IsOK = true;
                }
            }
        }
      
        private bool TryExtractColorMark(Mat inputMask, out Mat markMask, out Rectangle markRect, out int markPixels)
        {
            markMask = null;
            markRect = Rectangle.Empty;
            markPixels = 0;

            if (inputMask == null || inputMask.Empty())
                return false;

            using (Mat gray = new Mat())
            using (Mat bin = new Mat())
            using (Mat labels = new Mat())
            using (Mat stats = new Mat())
            using (Mat centroids = new Mat())
            {
                if (inputMask.Channels() == 1)
                {
                    inputMask.CopyTo(gray);
                }
                else if (inputMask.Channels() == 3)
                {
                    Cv2.CvtColor(inputMask, gray, ColorConversionCodes.BGR2GRAY);
                }
                else
                {
                    Cv2.CvtColor(inputMask, gray, ColorConversionCodes.BGRA2GRAY);
                }

                Cv2.Threshold(gray, bin, 0, 255, ThresholdTypes.Binary);
                int count = Cv2.ConnectedComponentsWithStats(
                    bin,
                    labels,
                    stats,
                    centroids,
                    PixelConnectivity.Connectivity8,
                    MatType.CV_32S);

                if (count <= 1)
                    return false;

                int bestLabel = -1;
                int bestArea = 0;
                for (int i = 1; i < count; i++)
                {
                    int area = stats.Get<int>(i, (int)ConnectedComponentsTypes.Area);
                    if (area > bestArea)
                    {
                        bestArea = area;
                        bestLabel = i;
                    }
                }

                if (bestLabel <= 0 || bestArea <= 0)
                    return false;

                int left = stats.Get<int>(bestLabel, (int)ConnectedComponentsTypes.Left);
                int top = stats.Get<int>(bestLabel, (int)ConnectedComponentsTypes.Top);
                int width = stats.Get<int>(bestLabel, (int)ConnectedComponentsTypes.Width);
                int height = stats.Get<int>(bestLabel, (int)ConnectedComponentsTypes.Height);
                markRect = new Rectangle(left, top, width, height);
                markPixels = bestArea;

                using (Mat eq = new Mat())
                {
                    Cv2.Compare(labels, bestLabel, eq, CmpType.EQ);
                    markMask = new Mat();
                    eq.ConvertTo(markMask, MatType.CV_8UC1, 255.0);
                }
            }

            return markMask != null && !markMask.Empty();
        }
      
  
       
        public int numTempOK;
        public bool IsAutoTrig { get => isAutoTrig; set => isAutoTrig = value; }
        public int NumOK { get => numOK; set => numOK = value; }
        public int DelayTrig { get => delayTrig; set => delayTrig = value; }
      
        public void SetModel( bool IsCopy=false)
        {
            lock (RuntimeLock)
            {
                if (Pattern == null)
                {
                    Pattern = new BeeCpp.Pattern2();
                }
                if (IsCopy)
                {
                    rotArea = CloneRectRotate(rotArea) ?? new RectRotate();
                    rotCheck = CloneRectRotate(rotCheck);
                    rotCrop = CloneRectRotate(rotCrop) ?? new RectRotate();
                    rotMask = CloneRectRotate(rotMask) ?? new RectRotate();
                    rotAreaTemp = CloneRectRotate(rotAreaTemp) ?? new RectRotate();
                    rotAreaAdjustment = CloneRectRotate(rotAreaAdjustment);
                    rotMaskAdjustment = CloneRectRotate(rotMaskAdjustment);
                    rotPositionAdjustment = CloneRectRotate(rotPositionAdjustment);

                    MaskHSVs = CloneHSVList(MaskHSVs);
                    MaskRGBs = CloneRGBList(MaskRGBs);
                    NgHSVs = CloneHSVList(NgHSVs);
                    NgRGBs = CloneRGBList(NgRGBs);
                    listCLMaskShow = CloneColorList(listCLMaskShow) ?? new List<Color>();
                    listCLNgShow = CloneColorList(listCLNgShow) ?? new List<Color>();
                    Postion = Postion != null ? new List<Point>(Postion) : new List<Point>();

                    ResetRuntimeStateForCopy();
                }
                if (bmRaw != null)
                {
                    matTemp = bmRaw.ToMat();
                    LearnPattern(matTemp, true);
                }
                if (rotArea == null) rotArea = new RectRotate();
                if (rotCrop == null) rotCrop = new RectRotate();
                if (rotMask == null) rotMask = new RectRotate();

                rotCrop.Name = "Area Temp";
                rotCrop.TypeCrop = TypeCrop.Crop;

                rotMask.Name = "Area Mask";
                rotMask.TypeCrop = TypeCrop.Mask;

                rotArea.Name = "Area Check";
                rotArea.TypeCrop = TypeCrop.Area;
                if (listCLMaskShow == null)
                    listCLMaskShow = new List<Color>();
                if (listCLNgShow == null)
                    listCLNgShow = new List<Color>();
                EnsureColorEngine();
                SetMaskColor();
                SetNgColor();
                if (Scale == 0) Scale = 1;
                if (rotCrop == null) rotCrop = new RectRotate();
                if (rotArea == null) rotArea = new RectRotate();

                Common.TryGetTool(IndexThread, Index).StepValue = 1;
                Common.TryGetTool(IndexThread, Index).MinValue = 0;
                if (NumThreads <= 0)
                    NumThreads = 1;
                MaxThread = EnableMultiThread ? NumThreads : 1;

                Common.TryGetTool(IndexThread, Index).MaxValue = 100;
                if (Common.TryGetTool(IndexThread, Index).Score == 0)
                    Common.TryGetTool(IndexThread, Index).Score = 80;
                Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;
            }
        }

        //IsProcess,Convert.ToBoolean((int) TypeMode)
        public List<RectRotate> rectRotates = new List<RectRotate>();
         public  List<float>list_AngleCenter =new List<float>();
        public ZeroPos ZeroPos=ZeroPos.Zero;
        public float Scale = 1;
        public bool IsLimitCouter = true;
        public DifficultyPattern DifficultyPattern;
        public bool EnableNms = true;
        public bool EnableKeepFilter = false;
        public bool EnableValidator = false;
        public bool EnableScaleSearch = false;
        public bool UseCpu = true;
        public bool UseGpu = false;
        public bool EnableMultiThread = false;
        public int NumThreads = 1;
        public int ScalePattern = 0;
        public int ScaleStep = 0;
        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
            lock (RuntimeLock)
            {

            float DeltaAngle = (rotCrop._rectRotation) - (rotArea._rectRotation);
            AngleLower = DeltaAngle - Angle;
           AngleUper = DeltaAngle + Angle;
            Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = 0;
            Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.NG;
            // 5) Gom kết quả
            rectRotates = new List<RectRotate>();
            ClearResultItems();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                if (raw.Empty()) return;
                if (raw.Type() == MatType.CV_8UC3)
                {
                   
                    Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);
                }
               


                Mat matProcess = null;
                byte[] rentedBuffer = null;

                try
                {
                    // 1) Crop ROI

                    //  matCrop = Cropper.CropRotatedRect(raw, rotArea, rotMask);

                    // 2) Tiền xử lý theo chế độ
                    switch (TypeMode)
                    {
                        case Mode.Pattern:
                           
                                matProcess = raw; // reuse backing store
                            
                            var rrCli = Converts.ToCli(rotArea); // như ở reply trước
                            RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                            Pattern.SetImgeRaw(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels(), rrCli, rrMaskCli);

                            break;



                        case Mode.Edge:
                            Mat matCrop = new Mat();                               
                                PatchCropContext ctx = new PatchCropContext();
                                matCrop = Cropper.CropOuterPatch(raw, rotArea, out ctx);
                                if (matProcess == null) matProcess = new Mat();
                                if (!matProcess.IsDisposed)
                                    if (!matProcess.Empty()) matProcess.Dispose();

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
                                }

                                matProcess = ApplyShapeMaskAndCompose(matProcess, ctx, rotArea, rotMask, returnMaskOnly: false);
                            //Cv2.ImWrite("process.png", matProcess);
                                Pattern.SetRawNoCrop(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels());

                            break;
                    }

                    if (matProcess == null || matProcess.Empty()) return;

                    // Bảo đảm grayscale cho Pattern engine
                    //if (matProcess.Type() == MatType.CV_8UC3)
                    //{
                    //    using (Mat gray = new Mat())
                    //    {
                    //        Cv2.CvtColor(matProcess, gray, ColorConversionCodes.BGR2GRAY);
                    //        matProcess = gray.Clone(); // own memory
                    //    }
                    //}

                    // 3) Nạp ảnh vào Pattern (con trỏ phải còn sống đến sau khi Match)
                    GC.KeepAlive(matProcess);
                    Pattern2StableConfig cfg = new Pattern2StableConfig(true);
                    cfg.AngleStartDeg = AngleLower;
                    cfg.AngleEndDeg = AngleUper;
                    cfg.AngleStepDeg = StepAngle;      // auto
                    cfg.MinAcceptScore = Common.TryGetTool(IndexThread, Index).Score / 100.0;
                    cfg.MaxPos = MaxObject;
                    cfg.MaxOverlap = OverLap;
                    cfg.BitwiseNot = ckBitwiseNot;
                    cfg.SubPixel = ckSubPixel;
                    cfg.Difficulty = (Pattern2DifficultyLevel)(int)(DifficultyPattern);
                    // threshold cuối
                    //cfg.MinAcceptScore = 0.0; // <=0 => auto theo mẫu

                    // bật tắt các lớp lọc
                    cfg.EnableValidator = EnableValidator;
                    cfg.EnableKeepFilter = EnableKeepFilter;
                    cfg.EnableNms = EnableNms;
                    cfg.EnableAutoThreshold = true;

                    float ScaleMin =(float) ((100 - ScalePattern)/100.0);
                    float ScaleMax = (float)((100 +ScalePattern) / 100.0);
                    // scale mẫu
                    cfg.EnableScaleSearch = EnableScaleSearch;
                    cfg.DebugLog = false;
                    cfg.ScaleMin = ScaleMin;
                    cfg.ScaleMax = ScaleMax;
                    cfg.ScaleStep = ScaleStep/100.0;
                   // cfg.DebugLogPath = "E:\\pattern2_debug.txt";
                    var listRS = Pattern.MatchStable(
                     cfg
                    );
                    float scoreSum = 0f;
                    if (listRS != null)
                        if (listRS.Count>0)
                        {
                        if(SearchPattern==SearchPattern.BestObj)
                        {
                            Rotaterectangle rotBest = listRS[0];
                            foreach (Rotaterectangle rot in listRS)
                            {
                                if (rot.AngleDeg < AngleLower || rot.AngleDeg > AngleUper)
                                    continue;
                                if (rot.Score < Common.TryGetTool(IndexThread, Index).Score)

                                    if (rot.Score > rotBest.Score)
                                    {
                                        rotBest = rot;
                                    }
                                continue;
                            }
                                if (rotBest.AngleDeg >= AngleLower && rotBest.AngleDeg <= AngleUper)

                                    if (rotBest.Score >= Common.TryGetTool(IndexThread, Index).Score)
                                    {
                                        float w = (float)rotBest.Width;
                                        float h = (float)rotBest.Height;
                                        var pCenter = new System.Drawing.PointF((float)rotBest.Cx, (float)rotBest.Cy);
                                        float angle = (float)rotBest.AngleDeg;
                                        float score = (float)rotBest.Score;
                                        scoreSum += score;
                                        var localRect = new RectRotate(
                                           new System.Drawing.RectangleF(-w / 2f, -h / 2f, w, h),
                                           pCenter, angle, AnchorPoint.None);
                                        AddMatchResult(rotArea, localRect, score);
                                    }
                            }
                        else
                        {
                            foreach (Rotaterectangle rot in listRS)
                            {
                                float w = (float)rot.Width;
                                float h = (float)rot.Height;
                                var pCenter = new System.Drawing.PointF((float)rot.Cx, (float)rot.Cy);
                                float angle = (float)rot.AngleDeg;
                                float score = (float)rot.Score;
                                scoreSum += score;
                                if (angle < AngleLower || angle > AngleUper)
                                    continue;
                                if (score < Common.TryGetTool(IndexThread, Index).Score)
                                    continue;
                                var localRect = new RectRotate(
                                    new System.Drawing.RectangleF(-w / 2f, -h / 2f, w, h),
                                    pCenter, angle, AnchorPoint.None);
                                AddMatchResult(rotArea, localRect, score);
                            }
                        }
                        
                    }

                    if (EnableColorCheck && ResultItems.Count > 0)
                    {
                        foreach (var item in ResultItems)
                            ApplyColorCheck(item);
                    }

                    if (scoreSum != 0 && ResultItems.Count > 0)
                    {
                        if(EnableColorCheck)
                        {
                            Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = ResultItems[0].ValueColor;
                        }
                        else
                        {
                            Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult =
                                (int)Math.Round(scoreSum / ResultItems.Count, 1);

                        }    
                          
                    }
                }
                catch(Exception ex)
                {

                    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Pattern", ex.ToString()));

                }
                finally
                {
                    if (rentedBuffer != null)
                        System.Buffers.ArrayPool<byte>.Shared.Return(rentedBuffer);

                    if (matProcess != null )
                        matProcess.Dispose();

                   
                }
            }
            }
        }
        public void Complete()
        {
            lock (RuntimeLock)
            {
            Common.TryGetTool(IndexThread, Index).Results = Results.OK;
            switch (Compare)
            {
                case Compares.Equal:
                    if (rectRotates.Count() != LimitCounter)
                        Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                    break;
                case Compares.Less:
                    if (rectRotates.Count() >= LimitCounter)
                        Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                    break;
                case Compares.More:
                    if (rectRotates.Count() <= LimitCounter)
                        Common.TryGetTool(IndexThread, Index).Results = Results.NG;
                    break;
            }
            if (EnableColorCheck && ResultItems != null && ResultItems.Any(x => x != null && !x.IsOK))
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
            //if (Common.TryGetTool(IndexThread, Index).Results == Results.OK)
            //{if (rectRotates != null)
            //        if (rectRotates.Count > 0)
            //        {
            //            Matrix mat = new Matrix();
            //            System.Drawing.Point pZero = new System.Drawing.Point(0, 0);
            //            PointF[] pMatrix = { pZero };
            //            mat.Translate(rotArea._PosCenter.X, rotArea._PosCenter.Y);
            //            mat.Rotate(rotArea._rectRotation);
            //            mat.Translate(rotArea._rect.X, rotArea._rect.Y);

            //            mat.Translate(rectRotates[0]._PosCenter.X, rectRotates[0]._PosCenter.Y);
            //            mat.Rotate(rectRotates[0]._rectRotation);
            //            mat.TransformPoints(pMatrix);

            //            int x = (int)pMatrix[0].X;// (int)rotArea._PosCenter.X -(int) rotArea ._rect.Width/2 + (int)rot._PosCenter.X;
            //            int y = (int)pMatrix[0].Y; ;// (int)rotArea._PosCenter.Y - (int)rotArea._rect.Height / 2 + (int)rot._PosCenter.Y;
            //            Global.AngleOrigin = rectRotates[0]._rectRotation;
            //            Global.pOrigin = new OpenCvSharp.Point(x, y);
            //        }
            //}
            //if(Common.TryGetTool(IndexThread, Index).TypeTool==TypeTool.Position_Adjustment)
            //if (!Global.IsRun)
            //{
            //    Global.StatusDraw = StatusDraw.Check;
            //    if (Common.TryGetTool(Global.IndexProgChoose, Index).Results==Results.OK)
            //    {
            //        rotPositionAdjustment = rectRotates[0].Clone();
            //        Global.rotOriginAdj = new RectRotate(rotCrop._rect, new PointF(rotArea._PosCenter.X -rotArea._rect.Width / 2 + rotPositionAdjustment._PosCenter.X,rotArea._PosCenter.Y - rotArea._rect.Height / 2 + rotPositionAdjustment._PosCenter.Y), rotPositionAdjustment._rectRotation, AnchorPoint.None);
            //    }    
            //}
            }
        }
        public Graphics DrawResult(Graphics gc)
        {
            RectRotate rotA;
            List<RectRotate> rectRotatesSnapshot;
            List<ResultItem> resultItemsSnapshot;
            List<System.Drawing.Point> listPCenterSnapshot;
            List<float> listAngleCenterSnapshot;

            lock (RuntimeLock)
            {
			if (rotAreaAdjustment == null && Global.IsRun) return gc;
			if (Global.IsRun)
				gc.ResetTransform();

			rotA = rotArea;
			if (Global.IsRun) rotA = rotAreaAdjustment;
                rectRotatesSnapshot = rectRotates != null ? rectRotates.Select(r => r?.Clone()).ToList() : new List<RectRotate>();
                resultItemsSnapshot = ResultItems != null ? new List<ResultItem>(ResultItems) : new List<ResultItem>();
                listPCenterSnapshot = listP_Center != null ? new List<System.Drawing.Point>(listP_Center) : new List<System.Drawing.Point>();
                listAngleCenterSnapshot = list_AngleCenter != null ? new List<float>(list_AngleCenter) : new List<float>();
            }
			var mat = new Matrix();
			if (!Global.IsRun)
			{
				mat.Translate(Global.pScroll.X, Global.pScroll.Y);
				mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
			}
			mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
			mat.Rotate(rotA._rectRotation);
			gc.Transform = mat;
			Color cl = Color.LimeGreen;
            //int OffSetX = (int)(rotA._PosCenter.X - rotA._rect.Width / 2);
            //int OffSetY = (int)(rotA._PosCenter.Y - rotA._rect.Height / 2);
            //OffSetX = (OffSetX > 0) ? 0 : -OffSetX;
            //OffSetY = (OffSetY > 0) ? 0 : -OffSetY;
            if (Common.TryGetTool(Global.IndexProgChoose, Index).Results == Results.NG)
			{
				cl = Global.ParaShow.ColorNG;
			}
			else
			{
				cl =  Global.ParaShow.ColorOK;
			}
			String nameTool = (int)(Index + 1) + "." + BeeCore.Common.TryGetTool(IndexThread, Index).Name;
            using (Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold))
            using (Brush brushText = new SolidBrush(Global.ParaShow.TextColor))
            {
			if (Global.ParaShow.IsShowBox)
				Draws.Box1Label(gc,rotA, nameTool, font, brushText, cl, Global.ParaShow.ThicknessLine);
			gc.ResetTransform();
			
			if (rectRotatesSnapshot != null && rectRotatesSnapshot.Count > 0)
			{
                int i = 1;
				foreach (RectRotate rot in rectRotatesSnapshot)
				{
                    ResultItem item = (resultItemsSnapshot != null && resultItemsSnapshot.Count >= i) ? resultItemsSnapshot[i - 1] : null;
                    Color clItem = (item == null || item.IsOK) ? cl : Global.ParaShow.ColorNG;

                    mat = new Matrix();
                    if (!Global.IsRun)
                    {
                        mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                        mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
                    }
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);
                    mat.Translate(rotA._rect.X, rotA._rect.Y);
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                    if (Global.ParaShow.IsShowPostion && listPCenterSnapshot.Count >= i && listAngleCenterSnapshot.Count >= i)
                    {
                        int min = (int)Math.Min(rot._rect.Width / 4, rot._rect.Height / 4);
                        Draws.Plus(gc, 0, 0, min, clItem, Global.ParaShow.ThicknessLine);
                        String sPos = "X,Y,A _ " + listPCenterSnapshot[i - 1].X + "," + listPCenterSnapshot[i - 1].Y + "," + Math.Round(listAngleCenterSnapshot[i - 1], 1);
                        if (ZeroPos == ZeroPos.ZeroADJ)
                            sPos = "*X,Y,A _ " + listPCenterSnapshot[i - 1].X + "," + listPCenterSnapshot[i - 1].Y + "," + Math.Round(listAngleCenterSnapshot[i - 1], 1);

                        gc.DrawString(sPos, font, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 5));
                    }
                    string valueBottom = EnableColorCheck ? ((item != null) ? "NG: " + item.ValueColor + " Px" + "/" + ScoreNG : "NG: 0 Px"+ "/"+ ScoreNG) :"";
                    string scoreText = (item != null) ? Math.Round(item.Score, 1) + "%" : Math.Round(rot.Score, 1) + "%";
                    if (Global.ParaShow.IsShowScore || EnableColorCheck)
                    {
                        string scoreTextShow = Global.ParaShow.IsShowScore ? scoreText : "";
                        Draws.Box3Label(gc, rot, i + "", scoreTextShow, valueBottom, font, clItem, brushText, 30, Global.ParaShow.ThicknessLine, false, Global.ParaShow.FontSize, 1, EnableColorCheck);
                    }
                    if (EnableColorCheck && item != null && item.ColorMarkContour != null && item.ColorMarkContour.Length > 0)
                    {
                        gc.Transform = mat;
                        using (Pen penMark = new Pen(Global.ParaShow.ColorInfor, Math.Max(1, Global.ParaShow.ThicknessLine )))
                        {
                            PointF[] contourPoints = item.ColorMarkContour
                                .Select(p => new PointF(rot._rect.X + p.X, rot._rect.Y + p.Y))
                                .ToArray();

                            if (contourPoints.Length == 1)
                            {
                                float x = contourPoints[0].X;
                                float y = contourPoints[0].Y;
                                gc.DrawEllipse(penMark, x - 1, y - 1, 2, 2);
                            }
                            else if (contourPoints.Length == 2)
                            {
                                gc.DrawLines(penMark, contourPoints);
                            }
                            else
                            {
                                gc.DrawPolygon(penMark, contourPoints);
                            }
                        }
                    }
                    //gc.ResetTransform();

                    if ( Global.ParaShow.IsShowDetail && item != null )
                    {
                        if (item.matProcess != null && !item.matProcess.Empty())
                        {
                            RectRotate rotMat = new RectRotate();
                            rotMat._rect = new RectangleF(
                                rot._rect.X + item.ColorMarkRect.X,
                                rot._rect.Y + item.ColorMarkRect.Y,
                                item.ColorMarkRect.Width,
                                item.ColorMarkRect.Height);
                            rotMat._PosCenter = new PointF(
                                rotMat._rect.X + rotMat._rect.Width / 2,
                                rotMat._rect.Y + rotMat._rect.Height / 2);
                            Draws.DrawMatInRectRotateNotMatrix(gc, item.matProcess, rotMat, Color.Red, Global.ParaShow.Opacity / 100.0f);
                        }


                    }
                    gc.ResetTransform();
					i++;
				}
			}
            }

           
           



            return gc;
        }

        public int IndexThread;

// using Python.Runtime;  // nếu bạn dùng Py.GIL()


    // C# 7.3 helper: đảm bảo ảnh là BGR 3 kênh
    private static Mat EnsureBgr(Mat src)
    {
        if (src.Empty()) return src;
        if (src.Channels() == 3) return src;
        Mat bgr = new Mat();
        Cv2.CvtColor(src, bgr, ColorConversionCodes.GRAY2BGR);
        return bgr;
    }

    // Tách hàm Python bridge để dễ dùng với try/finally
    private void CopyToPythonAndDetect(Mat bgr, int height, int width, int channels,
                                       ref Mat matProcess, ref byte[] rentedBuffer)
    {
        int size = checked((int)(bgr.Total() * bgr.ElemSize()));
        rentedBuffer = ArrayPool<byte>.Shared.Rent(size);
        Marshal.Copy(bgr.Data, rentedBuffer, 0, size);

        using (Py.GIL())
        {
            dynamic np = G.np;

            // frombuffer -> reshape; bọc bằng using để giải phóng PyObject
            using (var pyBuf = np.frombuffer(rentedBuffer, dtype: np.uint8))
            using (var npImage = pyBuf.reshape(height, width, channels))
            {
                dynamic pyResult = null;
                try
                {
                    pyResult = G.Classic.EdgeDetection(npImage);
                    if (pyResult == null) return;

                    byte[] edgeBytes = pyResult.As<byte[]>();
                    matProcess = new Mat(height, width, MatType.CV_8UC1, edgeBytes);
                }
                finally
                {
                    if (pyResult != null)
                    {
                        // PyObject có IDisposable khi dùng Python.Runtime.PyObject
                        var disp = pyResult as IDisposable;
                        if (disp != null) disp.Dispose();
                    }
                }
            }
        }
    }

    //public void Matching( RectRotate rectRotate)
    //    {
    //        using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
    //        {

    //            if (raw.Empty()) return;

    //            Mat matCrop = Cropper.CropRotatedRect(raw, rectRotate, rotMask);
    //            Mat matProcess = new Mat();

    //            switch (TypeMode)
    //            {
    //                case Mode.Pattern:
    //                    if (matCrop.Type() == MatType.CV_8UC3)
    //                        Cv2.CvtColor(matCrop, matProcess, ColorConversionCodes.BGR2GRAY);
    //                    else
    //                        matProcess = matCrop;
    //                    break;
    //                case Mode.OutLine:
    //                    matProcess = Common.CannyWithMorph(matCrop);
    //                    break;
    //                case Mode.Edge:
    //                    using (Py.GIL())
    //                    {
    //                        Cv2.CvtColor(matCrop, matCrop, ColorConversionCodes.GRAY2BGR);
    //                        int height = matCrop.Rows;
    //                        int width = matCrop.Cols;
    //                        int channels = matCrop.Channels();
    //                        if (!matCrop.IsContinuous())
    //                        {
    //                            matCrop = matCrop.Clone();
    //                        }
    //                        int size = (int)(matCrop.Total() * matCrop.ElemSize());
    //                        byte[] buffer = new byte[size];
    //                        Marshal.Copy(matCrop.Data, buffer, 0, size);
    //                        // Tạo ndarray từ byte[]
    //                        var npImage = G.np.array(buffer).reshape(height, width, channels);
    //                        // Gọi hàm Python
    //                        dynamic result = G.Classic.EdgeDetection(npImage);
    //                        if (result == null)
    //                            return;

    //                        // Chuyển kết quả ngược về byte[] rồi sang Mat
    //                        byte[] edgeBytes = result.As<byte[]>();
    //                        matProcess = new Mat(height, width, MatType.CV_8UC1, edgeBytes);
    //                    }

    //                    break;
    //            }

    //            // Cv2.ImWrite("Processing.png", matProcess);
    //            //   BeeCore.Native.SetImg(matProcess);
    //            if (!matProcess.IsContinuous())
    //            {
    //                matProcess = matProcess.Clone();
    //            }

				//if (matProcess.Empty()) return;
				//if (matProcess.Type() == MatType.CV_8UC3)
				//	Cv2.CvtColor(matProcess, matProcess, ColorConversionCodes.BGR2GRAY);

				//Pattern.SetRawNoCrop(matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(), matProcess.Channels());

				//if (matProcess.Empty()) return;

				

				//rectRotates = new List<RectRotate>();
				//listScore = new List<double>();
				//listP_Center = new List<System.Drawing.Point>();
				//var ListRS = Pattern.Match(IsHighSpeed, 0, AngleLower, AngleUper, Common.TryGetTool(IndexThread, Index).Score / 100.0, ckSIMD, ckBitwiseNot, ckSubPixel, NumObject, OverLap, false, -1);
    //            float scoreRs = 0;
    //            foreach (Rotaterectangle rot in ListRS)
				//{
				//	PointF pCenter = new PointF((float)rot.Cx, (float)rot.Cy);
				//	float angle = (float)rot.AngleDeg;
				//	float width = (float)rot.Width;
				//	float height = (float)rot.Height;
				//	float Score = (float)rot.Score;
    //                scoreRs += Score;

    //                rectRotates.Add(new RectRotate(new RectangleF(-width / 2, -height / 2, width, height), pCenter, angle, AnchorPoint.None));
				//	listScore.Add(Math.Round(Score, 1));
				//	listP_Center.Add(new System.Drawing.Point((int)rectRotate._PosCenter.X - (int)rectRotate._rect.Width / 2 + (int)pCenter.X, (int)rectRotate._PosCenter.Y - (int)rectRotate._rect.Height / 2 + (int)pCenter.Y));


				//}
    //            if (scoreRs != 0)
    //                Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult =(int)Math.Round( scoreRs / rectRotates.Count(),1);


    //            matProcess.Dispose();
    //            matCrop.Dispose();

    //        }

    //    }

    }
}
