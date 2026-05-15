using BeeCore.Algorithm;
using BeeCore.Core;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCpp;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BeeCore.Algorithm.FilletCornerMeasure;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Size = OpenCvSharp.Size;
using ShapeType = BeeGlobal.ShapeType;
namespace BeeCore
{
    [Serializable()]
    public enum ColorAreaCheckMode
    {
        Single = 0,
        MultiList = 1
    }

    [Serializable()]
    public enum ColorAreaScanDirection
    {
        X = 0,
        Y = 1
    }

    [Serializable()]
    public class ColorAreaColorList
    {
        public int Id;
        public string Name;
        public bool IsEnabled = true;
        public ColorGp TypeColor;
        public int Extraction;
        public int PixelTemplate;
        public int OrderTemplate;
        public float CenterTemplateX;
        public float CenterTemplateY;
        public bool HasCenterTemplate;
        public List<HSV> HSVs = new List<HSV>();
        public List<RGB> RGBs = new List<RGB>();
        public List<HSV> ExternHSVs = new List<HSV>();
        public List<RGB> ExternRGBs = new List<RGB>();

        public ColorAreaColorList()
        {
            Name = "Color List";
        }
    }

    [Serializable()]
    public class ColorAreaRegionResult
    {
        public int ListId;
        public int ListIndex;
        public int RegionIndex;
        public int Order;
        public int Label;
        public RectangleF BoundingBox;
        public PointF Center;
        public int PixelCount;
        public int PixelTemplate;
        public int PixelDeviation;
        public Results Result;
        public int ExpectedListId;
        public string ListName;
    }

    [Serializable()]
    public class ColorAreaMultiRunResult
    {
        public ColorAreaScanDirection ScanDirection = ColorAreaScanDirection.X;
        public List<ColorAreaRegionResult> Regions = new List<ColorAreaRegionResult>();

        public void Clear()
        {
            if (Regions == null)
                Regions = new List<ColorAreaRegionResult>();
            else
                Regions.Clear();
        }
    }

    [Serializable()]
    public  class ColorArea
    {
        public ColorArea ()
        {

        }
        public int IndexCCD = 0;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        [NonSerialized]
        public int MaxThread = 1;
        [NonSerialized]
        public bool IsNew = false;
        public bool IsClose = false;
        public bool IsOpen = false;
        public bool IsClearNoiseBig = false;
        public bool IsClearNoiseSmall = false;
        public int SizeClearsmall = 1;
        public int SizeClearBig = 1;
        public int SizeClose = 1;
        public int SizeOpen = 1;
        public List<HSV> HSVs;
        public List<RGB> RGBs;
        public ColorAreaCheckMode CheckMode = ColorAreaCheckMode.Single;
        public ColorAreaScanDirection ScanDirection = ColorAreaScanDirection.X;
        public List<ColorAreaColorList> ColorLists = new List<ColorAreaColorList>();
        public int Extraction = 0;
        public ColorGp TypeColor;
        public bool IsGetColor;
        public int PxTemp = 0;
        [NonSerialized]
        public ColorAreaMultiRunResult MultiResult = new ColorAreaMultiRunResult();
        public List<int> MultiPositionTemplate = new List<int>();
      
        public int Index = -1;
        public int IndexThread = 0;
        public TypeCrop TypeCrop;
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        [NonSerialized]
        public RectRotate rotAreaAdjustment;
        [NonSerialized]
        public RectRotate rotMaskAdjustment;



        [NonSerialized]
        public  BeeCpp.ColorArea ColorAreaPP;






   
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        public List<Color> listCLShow = new List<Color>();
        public Color clShow;
        public void SetModel( bool IsCopy=false)
        {

            if (rotArea == null) rotArea = new RectRotate();
           
            if (rotMask == null) rotMask = new RectRotate();
            if(IsCopy)
            {
               // ColorAreaPP = new BeeCpp.ColorArea();

            }    
          


            rotMask.Name = "Area Mask";
            rotMask.TypeCrop = TypeCrop.Mask;

            rotArea.Name = "Area Check";
            rotArea.TypeCrop = TypeCrop.Area;
            ColorAreaPP = new BeeCpp.ColorArea();
            EnsureMultiListModel();
            if (CheckMode == ColorAreaCheckMode.Single)
            {
                SetColor();
            }
            else
            {
                foreach (ColorAreaColorList list in ColorLists)
                {
                    if (list != null && list.IsEnabled && HasAnyColor(list))
                        SetColor(list);
                }
            }
            Common.TryGetTool(IndexThread, Index).StepValue = 1f;
            Common.TryGetTool(IndexThread, Index).MinValue = 0;
            Common.TryGetTool(IndexThread, Index).MaxValue = 20000;
            Common.TryGetTool(IndexThread, Index).StatusTool = StatusTool.WaitCheck;
        }

        public void EnsureMultiListModel()
        {
            if (ColorLists == null)
                ColorLists = new List<ColorAreaColorList>();

            if (ColorLists.Count == 0)
                ColorLists.Add(CreateDefaultColorListFromLegacyFields());

            for (int i = 0; i < ColorLists.Count; i++)
            {
                ColorAreaColorList list = ColorLists[i];
                if (list == null)
                {
                    list = new ColorAreaColorList();
                    ColorLists[i] = list;
                }

                if (list.Id <= 0)
                    list.Id = i + 1;
                if (string.IsNullOrEmpty(list.Name))
                    list.Name = "Color List " + (i + 1);
                if (list.HSVs == null)
                    list.HSVs = new List<HSV>();
                if (list.RGBs == null)
                    list.RGBs = new List<RGB>();
                if (list.ExternHSVs == null)
                    list.ExternHSVs = new List<HSV>();
                if (list.ExternRGBs == null)
                    list.ExternRGBs = new List<RGB>();
            }

            if (MultiResult == null)
                MultiResult = new ColorAreaMultiRunResult();
            MultiResult.ScanDirection = ScanDirection;
        }

        private ColorAreaColorList CreateDefaultColorListFromLegacyFields()
        {
            ColorAreaColorList list = new ColorAreaColorList();
            list.Id = 1;
            list.Name = "Color List 1";
            list.IsEnabled = true;
            list.TypeColor = TypeColor;
            list.Extraction = Extraction;
            list.PixelTemplate = PxTemp;
            list.HSVs = HSVs != null ? new List<HSV>(HSVs) : new List<HSV>();
            list.RGBs = RGBs != null ? new List<RGB>(RGBs) : new List<RGB>();
            return list;
        }
        [NonSerialized]
        public HSVCli hSV;
        [NonSerialized]
        public RGBCli rGB;
        public System.Drawing.Color GetColor( Mat raw, int x,int y)
        {
            using (Mat mat = raw.Clone())
            {
                if (mat.Empty()) return Color.Empty;
                if (mat.Type() == MatType.CV_8UC1)
                {
                    Cv2.CvtColor(mat, mat, ColorConversionCodes.GRAY2BGR);
                }
                ColorAreaPP.SetImgeRaw(mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels());
                switch (TypeColor)
                {
                    case ColorGp.HSV:
                        hSV=new HSVCli();
                     hSV =    ColorAreaPP.GetHSV( x, y); 
                        if (hSV != null)
                        {
                            clShow = HsvConvert.FromHsvOpenCv((byte)hSV.H, (byte)hSV.S, (byte)hSV.V);

                        }
                      
                        break;
                    case ColorGp.RGB:
                        rGB=new RGBCli();
                        
                       rGB=  ColorAreaPP.GetRGB(x, y);
                        if (rGB != null)
                            clShow = Color.FromArgb(rGB.R, rGB.G, rGB.B);
                        break;
                }
                
              
                
            }
            return clShow;

            
        }
        public void Undo()
        {
            switch (TypeColor)
            {
                case ColorGp.HSV:

                    HSVs.RemoveAt(HSVs.Count-1);

                    break;
                case ColorGp.RGB:

                    RGBs.RemoveAt(RGBs.Count - 1);

                    break;
            }

        }
        public void AddColor()
        {

            switch (TypeColor)
            {
                case ColorGp.HSV:
                    if (HSVs == null)
                        HSVs = new List<HSV>();
                    if (hSV != null)
                    {
                        HSVs.Add(new HSV(hSV.H, hSV.S, hSV.V));
                        clShow = HsvConvert.FromHsvOpenCv((byte)hSV.H, (byte)hSV.S, (byte)hSV.V);
                    }
                    break;
                case ColorGp.RGB:
                    if (RGBs == null)
                        RGBs = new List<RGB>();
                    if (rGB != null)
                    {
                        RGBs.Add(new RGB(rGB.R, rGB.G, rGB.B));
                        clShow = Color.FromArgb(rGB.R, rGB.G, rGB.B);
                    }
                    break;
            }
            listCLShow.Add(clShow);
        }

      
       
    
        public Mat ClearTemp()
        {
            HSVs = new List<HSV>();
            RGBs = new List<RGB>();
            SetColor();
            return new Mat();

        }
        public void PrimeNativeColors()
        {
            if (CheckMode == ColorAreaCheckMode.Single)
            {
                SetColor();
                return;
            }
            EnsureMultiListModel();
            foreach (ColorAreaColorList list in ColorLists)
            {
                if (list != null && list.IsEnabled && HasAnyColor(list))
                    SetColor(list);
            }
        }
        public void SetColor()
        {
            switch(TypeColor)
            { case ColorGp.HSV:
                    if (HSVs != null)
                    {
                        HSVCli[] arrHSV = new HSVCli[HSVs.Count];
                        int i = 0;
                        foreach (var hSV in HSVs)
                        {
                            arrHSV[i] = new HSVCli();
                            arrHSV[i].H = hSV.H;
                            arrHSV[i].S = hSV.S;
                            arrHSV[i].V = hSV.V;
                            i++;
                        }
                        ColorAreaPP.SetTempHSV(arrHSV, Extraction);
                    }
                    break;
                case ColorGp.RGB:
                    if (RGBs != null)
                    {
                        RGBCli[] arrRGB = new RGBCli[RGBs.Count];
                        int j = 0;
                        foreach (var hSV in RGBs)
                        {
                            arrRGB[j] = new RGBCli();
                            arrRGB[j].R = hSV.R;
                            arrRGB[j].G = hSV.G;
                            arrRGB[j].B = hSV.B;
                            j++;
                        }
                        ColorAreaPP.SetTempRGB(arrRGB, Extraction);
                    }    
                    
                    break;
            }
         
        
        }
     
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]

        unsafe public static extern IntPtr GetImageResult(ref int rows, ref int cols, ref int Type);


        public bool IsCalib;
     public   int pxRS = 0;
        public void DoWork(RectRotate retRotAdj, RectRotate rotMask)
        {
            if (CheckMode == ColorAreaCheckMode.MultiList)
                pxRS = CheckColorMulti(retRotAdj);
            else
                pxRS = CheckColor(retRotAdj);

        }
        public void Complete()
        {
            if (CheckMode == ColorAreaCheckMode.MultiList)
            {
                CompleteMulti();
                return;
            }

            if(IsCalib) 
                PxTemp = pxRS;
            if(IsNGMore)
            {
                if(pxRS<PxTemp)
                {
                    Common.TryGetTool(IndexThread, Index).ScoreResult = 0;
                    Common.TryGetTool(IndexThread, Index).Results = Results.OK;
                    return;

                }    
            }
            if (IsNGLess)
            {
                if (pxRS > PxTemp)
                {
                    Common.TryGetTool(IndexThread, Index).ScoreResult = 0;
                    Common.TryGetTool(IndexThread, Index).Results = Results.OK;
                    return;

                }
            }
            Common.TryGetTool(IndexThread, Index).ScoreResult =Math.Abs( pxRS-PxTemp)/10;
           
            if (Common.TryGetTool(IndexThread, Index).ScoreResult > Common.TryGetTool(IndexThread, Index).Score)
                Common.TryGetTool(IndexThread, Index).Results = Results.NG;
            else
                Common.TryGetTool(IndexThread, Index).Results = Results.OK;
        }

        private void CompleteMulti()
        {
            EnsureMultiListModel();
            PropetyTool owner = Common.TryGetTool(IndexThread, Index);
            if (owner == null)
                return;

            if (MultiPositionTemplate == null)
                MultiPositionTemplate = new List<int>();

            if (IsCalib)
            {
                MultiPositionTemplate.Clear();
                for (int i = 0; i < MultiResult.Regions.Count; i++)
                {
                    ColorAreaRegionResult region = MultiResult.Regions[i];
                    MultiPositionTemplate.Add(region.ListId);
                    region.ExpectedListId = region.ListId;
                    region.ListName = ColorLists.FirstOrDefault(x => x != null && x.Id == region.ListId)?.Name ?? "";
                    region.PixelDeviation = 0;
                    region.Result = Results.OK;

                    ColorAreaColorList colorList = ColorLists.FirstOrDefault(x => x != null && x.Id == region.ListId);
                    if (colorList != null)
                        colorList.PixelTemplate = region.PixelCount;
                }
                owner.ScoreResult = 0;
                owner.Results = Results.OK;
                return;
            }

            bool hasNg = false;
            int ngCount = 0;
            for (int i = 0; i < MultiResult.Regions.Count; i++)
            {
                ColorAreaRegionResult region = MultiResult.Regions[i];
                int expectedListId = (i < MultiPositionTemplate.Count) ? MultiPositionTemplate[i] : 0;
                region.ExpectedListId = expectedListId;
                region.ListName = ColorLists.FirstOrDefault(x => x != null && x.Id == region.ListId)?.Name ?? "";

                bool wrongColor = (expectedListId > 0 && region.ListId != expectedListId);
                bool noColor = (region.PixelCount == 0);
                region.Result = (wrongColor || noColor) ? Results.NG : Results.OK;
                if (region.Result == Results.NG)
                {
                    hasNg = true;
                    ngCount++;
                }
            }

            if (MultiPositionTemplate.Count > 0 && MultiResult.Regions.Count < MultiPositionTemplate.Count)
                hasNg = true;

            owner.ScoreResult = ngCount;
            owner.Results = hasNg ? Results.NG : Results.OK;
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
            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;

            if (Common.TryGetTool(IndexThread, Index).Results == Results.NG)
            {
                cl = Global.ParaShow.ColorNG;
            }
            else
            {
                cl =  Global.ParaShow.ColorOK;
            }
            String nameTool = (int)(Index + 1) + "." + Common.TryGetTool(IndexThread, Index).Name;
            Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold);
            //Common.TryGetTool(IndexThread, Index).ScoreResult + "%"
            int totalDeviation;
            if (CheckMode == ColorAreaCheckMode.MultiList && MultiResult != null && MultiResult.Regions != null)
            {
                totalDeviation = 0;
                foreach (ColorAreaRegionResult region in MultiResult.Regions)
                    totalDeviation += region.PixelDeviation;
            }
            else
            {
                totalDeviation = Math.Abs(pxRS - PxTemp);
            }
            Draws.Box3Label(gc, rotA, nameTool, "", totalDeviation + " Px", font, cl, brushText, 30, Global.ParaShow.ThicknessLine, false, Global.ParaShow.FontSize, 1, true);//("+Math.Round( ResultItem[i].Percent) + "%)
            RectRotate rectRotate = rotA.Clone();
            if (OffSetX > 0 && rectRotate._rect.Width > OffSetX * 2)
            {
                rectRotate._rect = new RectangleF(rectRotate._rect.X + OffSetX, rectRotate._rect.Y, rectRotate._rect.Width - OffSetX * 2, rectRotate._rect.Height);
                rectRotate._PosCenter = new PointF(rectRotate._PosCenter.X + OffSetX, rectRotate._PosCenter.Y);
            }
            if (!Global.IsRun || Global.ParaShow.IsShowDetail)
            {
                if (CheckMode == ColorAreaCheckMode.MultiList && matProcess != null && !matProcess.Empty())
                    DrawMultiRegionMats(gc, rectRotate);
                else if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rectRotate, Global.ScaleZoom * 100, Global.pScroll, cl, Global.ParaShow.Opacity / 100.0f);
            }
            if (CheckMode == ColorAreaCheckMode.MultiList && matProcess != null && !matProcess.Empty())
                DrawMultiRegionResults(gc, rectRotate);
          
            return gc;
        }

        private void DrawMultiRegionResults(Graphics gc, RectRotate rectRotate)
        {
            if (MultiResult == null || MultiResult.Regions == null || MultiResult.Regions.Count == 0)
                return;

            float scaleX = rectRotate._rect.Width / Math.Max(1, matProcess.Width);
            float scaleY = rectRotate._rect.Height / Math.Max(1, matProcess.Height);
            using (Font font = new Font("Arial", Math.Max(8, Global.ParaShow.FontSize), FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                foreach (ColorAreaRegionResult region in MultiResult.Regions)
                {
                    string listLabel = string.IsNullOrEmpty(region.ListName) ? region.ListId.ToString() : region.ListName;
                    string text = region.Order + ":" + listLabel;
                    Color regionColor = region.Result == Results.NG ? Global.ParaShow.ColorNG : Global.ParaShow.ColorOK;
                    PointF center = new PointF(
                        rectRotate._rect.X + region.Center.X * scaleX,
                        rectRotate._rect.Y + region.Center.Y * scaleY);
                    SizeF size = gc.MeasureString(text, font);
                    RectangleF back = new RectangleF(center.X - size.Width / 2f, center.Y - size.Height / 2f, size.Width, size.Height);
                    using (Brush backBrush = new SolidBrush(Color.FromArgb(180, regionColor)))
                    {
                        gc.FillRectangle(backBrush, back);
                    }
                    gc.DrawString(text, font, textBrush, back.Location);
                }
            }
        }

        private void DrawMultiRegionMats(Graphics gc, RectRotate rectRotate)
        {
            if (_listMasksCache == null || _listMasksCache.Count == 0 || MultiResult == null || MultiResult.Regions == null)
            {
                Color cl = Common.TryGetTool(IndexThread, Index)?.Results == Results.NG ? Global.ParaShow.ColorNG : Global.ParaShow.ColorOK;
                if (matProcess != null && !matProcess.Empty())
                    Draws.DrawMatInRectRotate(gc, matProcess, rectRotate, Global.ScaleZoom * 100, Global.pScroll, cl, Global.ParaShow.Opacity / 100.0f);
                return;
            }

            float opacity = Global.ParaShow.Opacity / 100.0f;
            foreach (ColorAreaRegionResult region in MultiResult.Regions)
            {
                ColorAreaListMask lm = _listMasksCache.FirstOrDefault(x => x != null && x.ListIndex == region.ListIndex);
                if (lm == null || lm.Mask == null || lm.Mask.Empty())
                    continue;
                Color regionColor = region.Result == Results.NG ? Global.ParaShow.ColorNG : Global.ParaShow.ColorOK;
                Draws.DrawMatInRectRotate(gc, lm.Mask, rectRotate, Global.ScaleZoom * 100, Global.pScroll, regionColor, opacity);
            }
        }
        [NonSerialized]
        public Mat matProcess = new Mat();
        [NonSerialized]
        public Mat matLabels = null;
        [NonSerialized]
        private List<ColorAreaListMask> _listMasksCache;
        [NonSerialized]
        private Native Native = new Native();
        float ValueColor = 0;
        public bool IsNGLess = false;
        public bool IsNGMore = false;
        public int OffSetX = 0;
       
        public int CheckColor(RectRotate rot)
        {
            int pxRs = 0;


            if (matProcess != null) { matProcess.Dispose(); matProcess = null; }

            using (Mat src =BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                if (src.Empty()) return -1;

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
                        bgr = src; // reuse
                    }
                    RectRotate rectRotate = rot.Clone();
                    if (OffSetX > 0 && rectRotate._rect.Width > OffSetX * 2)
                    {
                        rectRotate._rect = new RectangleF(rectRotate._rect.X + OffSetX, rectRotate._rect.Y, rectRotate._rect.Width - OffSetX*2, rectRotate._rect.Height);
                        rectRotate._PosCenter = new PointF(rectRotate._PosCenter.X + OffSetX, rectRotate._PosCenter.Y);
                    }
                        var rrCli = Converts.ToCli(rectRotate); // như ở reply trước
                    
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;
          
                    ColorAreaPP.SetImgeCrop(
                        bgr.Data, bgr.Width, bgr.Height, (int)bgr.Step(), bgr.Channels(), rrCli,rrMaskCli);

                    GC.KeepAlive(bgr);

                    int w, h, s, c;
                    IntPtr ptr = ColorAreaPP.Check(out w, out h, out s, out c);
                    try
                    {
                        // Validate trước, nhưng KHÔNG return trước khi FreeBuffer
                        if (ptr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                        {
                            return 0; // finally phía dưới vẫn chạy để FreeBuffer nếu cần
                        }

                        MatType mt = (c == 1) ? MatType.CV_8UC1
                                   : (c == 3) ? MatType.CV_8UC3
                                              : MatType.CV_8UC4;

                        using (var mNative = Mat.FromPixelData(h, w, mt, ptr, s))
                        {
                            matProcess = mNative.Clone(); // bây giờ dữ liệu đã thuộc về OpenCV (managed)
                        }
                    }
                    finally
                    {
                        // GIẢI PHÓNG BỘ NHỚ DO native CẤP PHÁT — luôn luôn!
                        if (ptr != IntPtr.Zero)
                        {
                            ColorAreaPP.FreeBuffer(ptr);
                            ptr = IntPtr.Zero;
                        }
                    }

                    // Hậu xử lý:
                    if (IsClearNoiseSmall)
                    {
                        Mat t = Filters.ClearNoise(matProcess, SizeClearsmall);
                        if (!object.ReferenceEquals(t, matProcess)) { matProcess.Dispose(); matProcess = t; }
                    }
                    if (IsClose)
                    {
                        Mat t = Filters.Morphology(matProcess, MorphTypes.Close, new Size(SizeClose, SizeClose));
                        if (!object.ReferenceEquals(t, matProcess)) { matProcess.Dispose(); matProcess = t; }
                    }
                    if (IsOpen)
                    {
                        Mat t = Filters.Morphology(matProcess, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                        if (!object.ReferenceEquals(t, matProcess)) { matProcess.Dispose(); matProcess = t; }
                    }
                    if (IsClearNoiseBig)
                    {
                        Mat t = Filters.ClearNoise(matProcess, SizeClearBig);
                        if (!object.ReferenceEquals(t, matProcess)) { matProcess.Dispose(); matProcess = t; }
                    }

                    if (matProcess.Channels() != 1)
                    {
                        var gray = new Mat();
                        try
                        {
                            if (matProcess.Channels() == 3)
                                Cv2.CvtColor(matProcess, gray, ColorConversionCodes.BGR2GRAY);
                            else
                                Cv2.CvtColor(matProcess, gray, ColorConversionCodes.BGRA2GRAY);

                            matProcess.Dispose();
                            matProcess = gray;
                            gray = null;
                        }
                        finally
                        {
                            gray?.Dispose();
                        }
                    }

                    pxRs = Cv2.CountNonZero(matProcess);
                    return pxRs;
                }
                finally
                {
                    if (bgr != null && !object.ReferenceEquals(bgr, src))
                        bgr.Dispose();
                }
            }
        }

        private int CheckColorMulti(RectRotate rot)
        {
            EnsureMultiListModel();
            MultiResult.Clear();
            MultiResult.ScanDirection = ScanDirection;

            ColorAreaPP = new BeeCpp.ColorArea();

            int totalPixels = 0;

            if (matProcess != null) { matProcess.Dispose(); matProcess = null; }
            if (_listMasksCache != null)
            {
                foreach (var lm in _listMasksCache)
                    lm?.Dispose();
                _listMasksCache = null;
            }

            using (Mat src = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                if (src.Empty()) return -1;

                Mat bgr = null;
                Mat combinedMask = null;
                List<ColorAreaListMask> listMasks = new List<ColorAreaListMask>();
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

                    RectRotate rectRotate = rot.Clone();
                    if (OffSetX > 0 && rectRotate._rect.Width > OffSetX * 2)
                    {
                        rectRotate._rect = new RectangleF(rectRotate._rect.X + OffSetX, rectRotate._rect.Y, rectRotate._rect.Width - OffSetX * 2, rectRotate._rect.Height);
                        rectRotate._PosCenter = new PointF(rectRotate._PosCenter.X + OffSetX, rectRotate._PosCenter.Y);
                    }

                    var rrCli = Converts.ToCli(rectRotate);
                    RectRotateCli? rrMaskCli = (rotMask != null) ? Converts.ToCli(rotMask) : (RectRotateCli?)null;

                    ColorAreaPP.SetImgeCrop(
                        bgr.Data, bgr.Width, bgr.Height, (int)bgr.Step(), bgr.Channels(), rrCli, rrMaskCli);

                    GC.KeepAlive(bgr);

                    for (int i = 0; i < ColorLists.Count; i++)
                    {
                        ColorAreaColorList colorList = ColorLists[i];
                        if (colorList == null || !colorList.IsEnabled || !HasAnyColor(colorList))
                            continue;

                        SetColor(colorList);

                        Mat listMask = ReadNativeMask();
                        try
                        {
                            if (listMask == null || listMask.Empty())
                                continue;

                            ApplyColorAreaPostProcess(ref listMask);
                            EnsureSingleChannelMask(ref listMask);

                            if (combinedMask == null || combinedMask.Empty())
                            {
                                combinedMask?.Dispose();
                                combinedMask = listMask.Clone();
                            }
                            else
                            {
                                Cv2.BitwiseOr(combinedMask, listMask, combinedMask);
                            }

                            listMasks.Add(new ColorAreaListMask
                            {
                                ColorList = colorList,
                                ListIndex = i,
                                Mask = listMask.Clone()
                            });
                        }
                        finally
                        {
                            listMask?.Dispose();
                        }
                    }

                    totalPixels = AddContourResults(listMasks);
                    matProcess = combinedMask ?? new Mat();
                    combinedMask = null;
                    _listMasksCache = listMasks;
                    listMasks = null;
                    return totalPixels;
                }
                finally
                {
                    if (listMasks != null)
                    {
                        foreach (ColorAreaListMask listMask in listMasks)
                            listMask.Dispose();
                    }
                    combinedMask?.Dispose();
                    if (bgr != null && !object.ReferenceEquals(bgr, src))
                        bgr.Dispose();
                }
            }
        }

        private sealed class ColorAreaListMask : IDisposable
        {
            public ColorAreaColorList ColorList;
            public int ListIndex;
            public Mat Mask;

            public void Dispose()
            {
                if (Mask != null)
                {
                    Mask.Dispose();
                    Mask = null;
                }
            }
        }

        private sealed class ColorAreaScanSlot
        {
            public int Label;
            public int RegionIndex;
            public RectangleF BoundingBox;
            public PointF Center;
        }

        private bool HasAnyColor(ColorAreaColorList colorList)
        {
            if (colorList.TypeColor == ColorGp.HSV)
                return (colorList.HSVs != null && colorList.HSVs.Count > 0)
                    || (colorList.ExternHSVs != null && colorList.ExternHSVs.Count > 0);

            if (colorList.TypeColor == ColorGp.RGB)
                return (colorList.RGBs != null && colorList.RGBs.Count > 0)
                    || (colorList.ExternRGBs != null && colorList.ExternRGBs.Count > 0);

            return false;
        }

        private void SetColor(ColorAreaColorList colorList)
        {
            if (colorList == null)
                return;

            switch (colorList.TypeColor)
            {
                case ColorGp.HSV:
                    List<HSV> hsvs = new List<HSV>();
                    if (colorList.HSVs != null)
                        hsvs.AddRange(colorList.HSVs);
                    if (colorList.ExternHSVs != null)
                        hsvs.AddRange(colorList.ExternHSVs);
                    if (hsvs.Count == 0)
                        return;

                    HSVCli[] arrHSV = new HSVCli[hsvs.Count];
                    for (int i = 0; i < hsvs.Count; i++)
                    {
                        arrHSV[i] = new HSVCli();
                        arrHSV[i].H = hsvs[i].H;
                        arrHSV[i].S = hsvs[i].S;
                        arrHSV[i].V = hsvs[i].V;
                    }
                    ColorAreaPP.SetTempHSV(arrHSV, colorList.Extraction);
                    break;

                case ColorGp.RGB:
                    List<RGB> rgbs = new List<RGB>();
                    if (colorList.RGBs != null)
                        rgbs.AddRange(colorList.RGBs);
                    if (colorList.ExternRGBs != null)
                        rgbs.AddRange(colorList.ExternRGBs);
                    if (rgbs.Count == 0)
                        return;

                    RGBCli[] arrRGB = new RGBCli[rgbs.Count];
                    for (int i = 0; i < rgbs.Count; i++)
                    {
                        arrRGB[i] = new RGBCli();
                        arrRGB[i].R = rgbs[i].R;
                        arrRGB[i].G = rgbs[i].G;
                        arrRGB[i].B = rgbs[i].B;
                    }
                    ColorAreaPP.SetTempRGB(arrRGB, colorList.Extraction);
                    break;
            }
        }

        private Mat ReadNativeMask()
        {
            int w, h, s, c;
            IntPtr ptr = ColorAreaPP.Check(out w, out h, out s, out c);
            try
            {
                if (ptr == IntPtr.Zero || w <= 0 || h <= 0 || s <= 0 || (c != 1 && c != 3 && c != 4))
                    return null;

                MatType mt = (c == 1) ? MatType.CV_8UC1
                           : (c == 3) ? MatType.CV_8UC3
                                      : MatType.CV_8UC4;

                using (var mNative = Mat.FromPixelData(h, w, mt, ptr, s))
                {
                    return mNative.Clone();
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    ColorAreaPP.FreeBuffer(ptr);
            }
        }

        private void ApplyColorAreaPostProcess(ref Mat mask)
        {
            if (IsClearNoiseSmall)
            {
                Mat t = Filters.ClearNoise(mask, SizeClearsmall);
                if (!object.ReferenceEquals(t, mask)) { mask.Dispose(); mask = t; }
            }
            if (IsClose)
            {
                Mat t = Filters.Morphology(mask, MorphTypes.Close, new Size(SizeClose, SizeClose));
                if (!object.ReferenceEquals(t, mask)) { mask.Dispose(); mask = t; }
            }
            if (IsOpen)
            {
                Mat t = Filters.Morphology(mask, MorphTypes.Open, new Size(SizeOpen, SizeOpen));
                if (!object.ReferenceEquals(t, mask)) { mask.Dispose(); mask = t; }
            }
            if (IsClearNoiseBig)
            {
                Mat t = Filters.ClearNoise(mask, SizeClearBig);
                if (!object.ReferenceEquals(t, mask)) { mask.Dispose(); mask = t; }
            }
        }

        private void EnsureSingleChannelMask(ref Mat mask)
        {
            if (mask.Channels() == 1)
                return;

            var gray = new Mat();
            try
            {
                if (mask.Channels() == 3)
                    Cv2.CvtColor(mask, gray, ColorConversionCodes.BGR2GRAY);
                else
                    Cv2.CvtColor(mask, gray, ColorConversionCodes.BGRA2GRAY);

                mask.Dispose();
                mask = gray;
                gray = null;
            }
            finally
            {
                gray?.Dispose();
            }
        }

        private void AddRegionResults(ColorAreaColorList colorList, int listIndex, Mat mask)
        {
            using (Mat labels = new Mat())
            using (Mat stats = new Mat())
            using (Mat centroids = new Mat())
            {
                int n = Cv2.ConnectedComponentsWithStats(
                    mask, labels, stats, centroids,
                    PixelConnectivity.Connectivity8,
                    MatType.CV_32S);

                int regionIndex = 0;
                for (int i = 1; i < n; i++)
                {
                    int area = stats.At<int>(i, (int)ConnectedComponentsTypes.Area);
                    if (area <= 0)
                        continue;

                    int x = stats.At<int>(i, (int)ConnectedComponentsTypes.Left);
                    int y = stats.At<int>(i, (int)ConnectedComponentsTypes.Top);
                    int w = stats.At<int>(i, (int)ConnectedComponentsTypes.Width);
                    int h = stats.At<int>(i, (int)ConnectedComponentsTypes.Height);
                    double centerX = centroids.At<double>(i, 0);
                    double centerY = centroids.At<double>(i, 1);
                    int deviation = GetRulePixelDeviation(area, colorList.PixelTemplate);

                    MultiResult.Regions.Add(new ColorAreaRegionResult
                    {
                        ListId = colorList.Id,
                        ListIndex = listIndex,
                        RegionIndex = regionIndex,
                        BoundingBox = new RectangleF(x, y, w, h),
                        Center = new PointF((float)centerX, (float)centerY),
                        PixelCount = area,
                        PixelTemplate = colorList.PixelTemplate,
                        PixelDeviation = deviation,
                        Result = EvaluatePixelResult(area, colorList.PixelTemplate)
                    });
                    regionIndex++;
                }
            }
        }

        private int AddPerListResults(List<ColorAreaListMask> listMasks)
        {
            if (listMasks == null || listMasks.Count == 0)
                return 0;

            int totalPixels = 0;
            int n = listMasks.Count;
            for (int i = 0; i < n; i++)
            {
                ColorAreaListMask lm = listMasks[i];
                if (lm == null || lm.Mask == null || lm.Mask.Empty())
                    continue;

                int pixelCount = Cv2.CountNonZero(lm.Mask);
                totalPixels += pixelCount;

                RectangleF bounds = RectangleF.Empty;
                PointF center = PointF.Empty;
                int maskW = lm.Mask.Width;
                int maskH = lm.Mask.Height;
                if (pixelCount > 0)
                {
                    var moments = Cv2.Moments(lm.Mask, true);
                    if (moments.M00 > 0)
                        center = new PointF((float)(moments.M10 / moments.M00), (float)(moments.M01 / moments.M00));
                    OpenCvSharp.Rect br = Cv2.BoundingRect(lm.Mask);
                    bounds = new RectangleF(br.X, br.Y, br.Width, br.Height);
                }
                else
                {
                    if (lm.ColorList.HasCenterTemplate)
                    {
                        center = new PointF(lm.ColorList.CenterTemplateX, lm.ColorList.CenterTemplateY);
                    }
                    else
                    {
                        float frac = (i + 0.5f) / n;
                        if (ScanDirection == ColorAreaScanDirection.Y)
                            center = new PointF(maskW / 2f, frac * maskH);
                        else
                            center = new PointF(frac * maskW, maskH / 2f);
                    }
                }

                int deviation = GetRulePixelDeviation(pixelCount, lm.ColorList.PixelTemplate);
                MultiResult.Regions.Add(new ColorAreaRegionResult
                {
                    ListId = lm.ColorList.Id,
                    ListIndex = lm.ListIndex,
                    RegionIndex = i,
                    Order = i + 1,
                    Label = 0,
                    BoundingBox = bounds,
                    Center = center,
                    PixelCount = pixelCount,
                    PixelTemplate = lm.ColorList.PixelTemplate,
                    PixelDeviation = deviation,
                    Result = EvaluatePixelResult(pixelCount, lm.ColorList.PixelTemplate)
                });
            }
            return totalPixels;
        }

        private int AddContourResults(List<ColorAreaListMask> listMasks)
        {
            if (listMasks == null || listMasks.Count == 0)
                return 0;

            ColorAreaListMask refMask = listMasks.FirstOrDefault(x => x != null && x.Mask != null && !x.Mask.Empty());
            if (refMask == null)
                return 0;

            int W = refMask.Mask.Width;
            int H = refMask.Mask.Height;

            Mat combined = new Mat(H, W, MatType.CV_8UC1, Scalar.All(0));
            try
            {
                foreach (ColorAreaListMask lm in listMasks)
                {
                    if (lm == null || lm.Mask == null || lm.Mask.Empty())
                        continue;
                    Cv2.BitwiseOr(combined, lm.Mask, combined);
                }

                var allContours = new List<(int ListId, int ListIndex, string ListName, PointF Center, RectangleF BBox, int PixelCount)>();

                using (Mat labels = new Mat())
                using (Mat stats = new Mat())
                using (Mat centroids = new Mat())
                {
                    int n = Cv2.ConnectedComponentsWithStats(
                        combined, labels, stats, centroids,
                        PixelConnectivity.Connectivity8,
                        MatType.CV_32S);

                    using (Mat compMask = new Mat())
                    using (Mat hit = new Mat())
                    {
                        for (int i = 1; i < n; i++)
                        {
                            int area = stats.At<int>(i, (int)ConnectedComponentsTypes.Area);
                            if (area <= 0)
                                continue;

                            int x = stats.At<int>(i, (int)ConnectedComponentsTypes.Left);
                            int y = stats.At<int>(i, (int)ConnectedComponentsTypes.Top);
                            int w = stats.At<int>(i, (int)ConnectedComponentsTypes.Width);
                            int h = stats.At<int>(i, (int)ConnectedComponentsTypes.Height);
                            double cx = centroids.At<double>(i, 0);
                            double cy = centroids.At<double>(i, 1);

                            Cv2.InRange(labels, new Scalar(i), new Scalar(i), compMask);

                            ColorAreaListMask winner = null;
                            int winnerCount = 0;
                            foreach (ColorAreaListMask lm in listMasks)
                            {
                                if (lm == null || lm.Mask == null || lm.Mask.Empty())
                                    continue;
                                Cv2.BitwiseAnd(lm.Mask, compMask, hit);
                                int cnt = Cv2.CountNonZero(hit);
                                if (cnt > winnerCount)
                                {
                                    winnerCount = cnt;
                                    winner = lm;
                                }
                            }

                            if (winner == null)
                                continue;

                            allContours.Add((
                                winner.ColorList.Id,
                                winner.ListIndex,
                                winner.ColorList?.Name ?? "",
                                new PointF((float)cx, (float)cy),
                                new RectangleF(x, y, w, h),
                                winnerCount
                            ));
                        }
                    }
                }

                IEnumerable<(int ListId, int ListIndex, string ListName, PointF Center, RectangleF BBox, int PixelCount)> ordered;
                if (ScanDirection == ColorAreaScanDirection.Y)
                    ordered = allContours.OrderBy(c => c.Center.Y).ThenBy(c => c.Center.X).ThenBy(c => c.ListIndex);
                else
                    ordered = allContours.OrderBy(c => c.Center.X).ThenBy(c => c.Center.Y).ThenBy(c => c.ListIndex);

                int order = 1;
                int totalPixels = 0;
                foreach (var c in ordered)
                {
                    MultiResult.Regions.Add(new ColorAreaRegionResult
                    {
                        ListId = c.ListId,
                        ListIndex = c.ListIndex,
                        ListName = c.ListName,
                        Order = order++,
                        BoundingBox = c.BBox,
                        Center = c.Center,
                        PixelCount = c.PixelCount,
                    });
                    totalPixels += c.PixelCount;
                }
                return totalPixels;
            }
            finally
            {
                combined?.Dispose();
            }
        }

        private int AddScanSlotResults(Mat combinedMask, List<ColorAreaListMask> listMasks)
        {
            if (combinedMask == null || combinedMask.Empty() || listMasks == null || listMasks.Count == 0)
                return 0;

            int totalPixels = 0;
            if (matLabels != null) { matLabels.Dispose(); matLabels = null; }
            using (Mat labels = new Mat())
            using (Mat stats = new Mat())
            using (Mat centroids = new Mat())
            {
                int n = Cv2.ConnectedComponentsWithStats(
                    combinedMask, labels, stats, centroids,
                    PixelConnectivity.Connectivity8,
                    MatType.CV_32S);
                matLabels = labels.Clone();

                List<ColorAreaScanSlot> slots = new List<ColorAreaScanSlot>();
                for (int i = 1; i < n; i++)
                {
                    int area = stats.At<int>(i, (int)ConnectedComponentsTypes.Area);
                    if (area <= 0)
                        continue;

                    int x = stats.At<int>(i, (int)ConnectedComponentsTypes.Left);
                    int y = stats.At<int>(i, (int)ConnectedComponentsTypes.Top);
                    int w = stats.At<int>(i, (int)ConnectedComponentsTypes.Width);
                    int h = stats.At<int>(i, (int)ConnectedComponentsTypes.Height);
                    double centerX = centroids.At<double>(i, 0);
                    double centerY = centroids.At<double>(i, 1);

                    slots.Add(new ColorAreaScanSlot
                    {
                        Label = i,
                        BoundingBox = new RectangleF(x, y, w, h),
                        Center = new PointF((float)centerX, (float)centerY)
                    });
                }

                IEnumerable<ColorAreaScanSlot> orderedSlots;
                if (ScanDirection == ColorAreaScanDirection.Y)
                {
                    orderedSlots = slots
                        .OrderBy(s => s.Center.Y)
                        .ThenBy(s => s.Center.X)
                        .ThenBy(s => s.Label);
                }
                else
                {
                    orderedSlots = slots
                        .OrderBy(s => s.Center.X)
                        .ThenBy(s => s.Center.Y)
                        .ThenBy(s => s.Label);
                }

                slots = orderedSlots.ToList();
                for (int i = 0; i < slots.Count; i++)
                    slots[i].RegionIndex = i;

                using (Mat slotMask = new Mat())
                using (Mat expectedInSlot = new Mat())
                {
                    for (int i = 0; i < listMasks.Count; i++)
                    {
                        ColorAreaListMask expected = listMasks[i];
                        ColorAreaScanSlot slot = i < slots.Count ? slots[i] : null;
                        int pixelCount = 0;
                        RectangleF bounds = RectangleF.Empty;
                        PointF center = PointF.Empty;

                        if (slot != null)
                        {
                            Cv2.InRange(labels, new Scalar(slot.Label), new Scalar(slot.Label), slotMask);
                            Cv2.BitwiseAnd(expected.Mask, slotMask, expectedInSlot);
                            pixelCount = Cv2.CountNonZero(expectedInSlot);
                            bounds = slot.BoundingBox;
                            center = slot.Center;
                        }

                        totalPixels += pixelCount;
                        int deviation = GetRulePixelDeviation(pixelCount, expected.ColorList.PixelTemplate);

                        MultiResult.Regions.Add(new ColorAreaRegionResult
                        {
                            ListId = expected.ColorList.Id,
                            ListIndex = expected.ListIndex,
                            RegionIndex = i,
                            Order = i + 1,
                            Label = slot != null ? slot.Label : 0,
                            BoundingBox = bounds,
                            Center = center,
                            PixelCount = pixelCount,
                            PixelTemplate = expected.ColorList.PixelTemplate,
                            PixelDeviation = deviation,
                            Result = EvaluatePixelResult(pixelCount, expected.ColorList.PixelTemplate)
                        });
                    }
                }
            }

            return totalPixels;
        }

        private Results EvaluatePixelResult(int pixelCount, int pixelTemplate)
        {
            PropetyTool owner = Common.TryGetTool(IndexThread, Index);
            if (owner == null)
                return Results.None;

            float scoreResult = GetRulePixelDeviation(pixelCount, pixelTemplate) / 10f;
            Results rs= scoreResult > owner.Score ? Results.NG : Results.OK;
            return rs;
        }

        private int GetRulePixelDeviation(int pixelCount, int pixelTemplate)
        {
            if (IsNGMore && pixelCount <= pixelTemplate)
                return 0;
            if (IsNGLess && pixelCount >= pixelTemplate)
                return 0;

            return Math.Abs(pixelCount - pixelTemplate);
        }

        private void SortRegionResults()
        {
            if (MultiResult == null || MultiResult.Regions == null)
                return;

            IEnumerable<ColorAreaRegionResult> ordered;
            if (ScanDirection == ColorAreaScanDirection.Y)
            {
                ordered = MultiResult.Regions
                    .OrderBy(r => r.Center.Y)
                    .ThenBy(r => r.Center.X)
                    .ThenBy(r => r.ListIndex)
                    .ThenBy(r => r.RegionIndex);
            }
            else
            {
                ordered = MultiResult.Regions
                    .OrderBy(r => r.Center.X)
                    .ThenBy(r => r.Center.Y)
                    .ThenBy(r => r.ListIndex)
                    .ThenBy(r => r.RegionIndex);
            }

            MultiResult.Regions = ordered.ToList();
            for (int i = 0; i < MultiResult.Regions.Count; i++)
                MultiResult.Regions[i].Order = i + 1;
        }

    }
}
