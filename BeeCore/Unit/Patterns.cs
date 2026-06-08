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

            // bmRaw is the learned template image, not runtime state. The copied
            // ToolPattern UI can already hold it in imgTemp.Image before SetModel(true)
            // runs, so disposing it here leaves PictureBox with an invalid Bitmap.
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
        public RectRotate rotArea,rotCheck, rotCrop, rotMask, rotLimit;
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

        // Sub-template hits parallel với rectRotates / ResultItems. Index i = sub hits của
        // main instance thứ i. Reset mỗi DoWork. [NonSerialized] vì hits chỉ runtime.
        [NonSerialized]
        public List<List<SubHit>> SubHitsPerItem = new List<List<SubHit>>();

        // ===== Multi-template mode =====
        // 3-state mode (Single / Multi / MultiNoLimit). MultiNoLimit = Multi nhưng không có
        // per-label RotLimit + cho phép nhiều row chia sẻ cùng Label (judge per unique label).
        // Backward-compat: project JSON cũ chỉ có IsMultiTemplate (bool) → shim getter/setter
        // map sang TemplateMode (Single ↔ Multi). MultiNoLimit chỉ set qua TemplateMode.
        public PatternMode TemplateMode { get; set; } = PatternMode.Single;

        // Shim: chỉ phục vụ load project cũ. Engine/UI mới đọc TemplateMode trực tiếp.
        public bool IsMultiTemplate
        {
            get => TemplateMode != PatternMode.Single;
            set
            {
                if (value && TemplateMode == PatternMode.Single) TemplateMode = PatternMode.Multi;
                else if (!value) TemplateMode = PatternMode.Single;
            }
        }

        // Lazy getter: BinaryFormatter/[Serializable] dùng FormatterServices.GetUninitializedObject
        // bypass auto-property initializer → field có thể null sau deserialize. Phải lazy-init
        // để tránh NRE ở UI/engine call site.
        private List<Pattern2TemplateEntry> _multiTemplates;
        public List<Pattern2TemplateEntry> MultiTemplates
        {
            get { return _multiTemplates ?? (_multiTemplates = new List<Pattern2TemplateEntry>()); }
            set { _multiTemplates = value; MarkBatchDirty(); }
        }

        // Cờ batch đã học (lifetime runtime, không serialize). UI mutate MultiTemplates → gọi
        // MarkBatchDirty() → DoWork sẽ học lại batch ở lần check kế tiếp.
        [NonSerialized]
        private bool _batchLearned = false;

        public void MarkBatchDirty() { _batchLearned = false; }

        // Multi-mode: nếu true, mỗi label crop ảnh raw theo entry.RotLimit trước khi match
        // (Pattern.SetImgeRaw với rotLimitCli). Nếu false (default), match toàn bộ ảnh raw
        // 1 lần qua MatchBatchStable. Theo yêu cầu UX: toggle "Full | Crop".
        public bool CheckByAreaLimit { get; set; } = false;

        // ===== 6-Method Shim (Q3: giữ TemplateMode + CheckByAreaLimit làm storage) =====
        // PatternMethod là enum 6-state expose ra UI; runtime đọc thẳng TemplateMode +
        // CheckByAreaLimit nên backend không đổi. Project JSON cũ chỉ có 2 field này.
        // - SingleLabel{None,AreaLimit,NoLimit}: chỉ phân biệt qua field _singleAreaPolicy
        //   (transient field — không cần persist vì Single mode hiện tại đã match toàn ROI).
        public PatternMethod Method
        {
            get
            {
                if (TemplateMode == PatternMode.Single)
                {
                    switch (_singleAreaPolicy)
                    {
                        case SingleAreaPolicy.AreaLimit: return PatternMethod.SingleLabelAreaLimit;
                        case SingleAreaPolicy.NoLimit: return PatternMethod.SingleLabelNoLimit;
                        default: return PatternMethod.SingleLabel;
                    }
                }
                if (TemplateMode == PatternMode.MultiNoLimit) return PatternMethod.MultiLabelNoLimit;
                return CheckByAreaLimit ? PatternMethod.MultiLabelAreaLimit : PatternMethod.MultiLabel;
            }
            set
            {
                switch (value)
                {
                    case PatternMethod.SingleLabel:
                        TemplateMode = PatternMode.Single;
                        _singleAreaPolicy = SingleAreaPolicy.None;
                        break;
                    case PatternMethod.SingleLabelAreaLimit:
                        TemplateMode = PatternMode.Single;
                        _singleAreaPolicy = SingleAreaPolicy.AreaLimit;
                        break;
                    case PatternMethod.SingleLabelNoLimit:
                        TemplateMode = PatternMode.Single;
                        _singleAreaPolicy = SingleAreaPolicy.NoLimit;
                        break;
                    case PatternMethod.MultiLabel:
                        TemplateMode = PatternMode.Multi;
                        CheckByAreaLimit = false;
                        break;
                    case PatternMethod.MultiLabelAreaLimit:
                        TemplateMode = PatternMode.Multi;
                        CheckByAreaLimit = true;
                        break;
                    case PatternMethod.MultiLabelNoLimit:
                        TemplateMode = PatternMode.MultiNoLimit;
                        CheckByAreaLimit = false;
                        break;
                }
                MarkBatchDirty();
            }
        }

        // Persist SingleArea policy chỉ cần khi user chọn AreaLimit/NoLimit trong Single mode.
        // Field public để serializer ghi nhận; legacy project (=Single mode) load với None.
        public SingleAreaPolicy SingleAreaMode
        {
            get { return _singleAreaPolicy; }
            set { _singleAreaPolicy = value; }
        }
        private SingleAreaPolicy _singleAreaPolicy = SingleAreaPolicy.None;

        // ===== Sub-templates cho Single modes =====
        // Multi modes lưu sub-list per entry (Pattern2TemplateEntry.SubTemplates). Single modes
        // chỉ có 1 main template duy nhất → sub-list ở level Patterns. Lazy-init để load JSON cũ.
        private List<SubTemplateEntry> _subTemplatesSingle;
        public List<SubTemplateEntry> SubTemplatesSingle
        {
            get { return _subTemplatesSingle ?? (_subTemplatesSingle = new List<SubTemplateEntry>()); }
            set { _subTemplatesSingle = value; }
        }
        public int SubSearchPadPxSingle { get; set; } = 20;

        // ===== Sub-template validation helper =====
        // Q2: per-instance check. Trả true nếu (không có sub configured) hoặc (ít nhất 1 sub
        // detected trong patch quanh main). subCount = -1 khi không có sub configured.
        private bool ValidateSubTemplatesForInstance(
            List<SubTemplateEntry> subs, int padPx, RectRotate absoluteMain, Mat raw,
            out int subCount, List<SubHit> hitsOut = null)
        {
            subCount = -1;
            if (subs == null || subs.Count == 0) return true;
            if (raw == null || raw.Empty() || absoluteMain == null) return false;

            // 1) Crop rotated rect của absoluteMain (inflated bởi padPx). Dùng
            // Cropper.CropRotatedRect → patch là rectified Mat, pixel (0,0) = corner của
            // rotated rect. Bằng cách này search area = ĐÚNG main area, không tràn ra
            // ngoài main bbox như axis-aligned crop trước đây.
            int pad = Math.Max(0, padPx);
            var mainInflated = absoluteMain.Clone();
            mainInflated._rect = new System.Drawing.RectangleF(
                absoluteMain._rect.X - pad,
                absoluteMain._rect.Y - pad,
                absoluteMain._rect.Width + 2 * pad,
                absoluteMain._rect.Height + 2 * pad);
            if (mainInflated._rect.Width < 8 || mainInflated._rect.Height < 8) return false;

            // 2) Crop. Cropper trả tight-packed Mat (step = width*channels).
            subCount = 0;
            using (var patch = Cropper.CropRotatedRect(raw, mainInflated, null))
            {
                if (patch == null || patch.Empty()
                    || patch.Width < 8 || patch.Height < 8) return false;
                Mat patchGray = patch;
                bool ownsGray = false;
                if (patch.Channels() != 1)
                {
                    patchGray = new Mat();
                    Cv2.CvtColor(patch, patchGray, ColorConversionCodes.BGR2GRAY);
                    ownsGray = true;
                }
                // patch-local → raw global: shift về main-center frame, rotate, translate.
                double radMain = mainInflated._rectRotation * Math.PI / 180.0;
                float cosM = (float)Math.Cos(radMain);
                float sinM = (float)Math.Sin(radMain);
                float halfPw = patch.Width * 0.5f;
                float halfPh = patch.Height * 0.5f;
                float cxMain = mainInflated._PosCenter.X;
                float cyMain = mainInflated._PosCenter.Y;
                try
                {
                    foreach (var sub in subs)
                    {
                        var learned = sub.EnsureLearned();
                        if (learned == null) continue;
                        try
                        {
                            learned.SetRawNoCrop(
                                patchGray.Data, patchGray.Width, patchGray.Height,
                                (int)patchGray.Step(), patchGray.Channels());
                            GC.KeepAlive(patchGray);

                            var cfg = new Pattern2StableConfig(true);
                            cfg.MinAcceptScore = sub.ScoreThreshold / 100.0;
                            if (sub.HasAngleRange)
                            {
                                cfg.AngleStartDeg = sub.AngleLower;
                                cfg.AngleEndDeg = sub.AngleUpper;
                            }
                            else
                            {
                                cfg.AngleStartDeg = -180.0;
                                cfg.AngleEndDeg = 180.0;
                            }
                            cfg.MaxPos = 1; // chỉ cần 1 hit
                            var rs = learned.MatchStable(cfg);
                            // Pattern2 trả Score ở scale 0..100 (Pattern2.cpp dMatchScore*100).
                            // sub.ScoreThreshold cũng 0..100 (UI scale). So sánh trực tiếp.
                            if (rs != null && rs.Count > 0
                                && rs[0].Score >= sub.ScoreThreshold)
                            {
                                subCount++;
                                // Convert hit patch-local → raw global.
                                // Patch là rectified của mainInflated. Patch center = main center,
                                // patch X-axis = main X-axis sau khi rotate ngược _rectRotation.
                                //   localX = hit.Cx - patch.Width/2
                                //   localY = hit.Cy - patch.Height/2
                                //   globalX = localX*cos - localY*sin + main.Cx
                                //   globalY = localX*sin + localY*cos + main.Cy
                                // Sub angle = hit.AngleDeg + main._rectRotation.
                                var hit = rs[0];
                                float localX = (float)hit.Cx - halfPw;
                                float localY = (float)hit.Cy - halfPh;
                                float globalX = localX * cosM - localY * sinM + cxMain;
                                float globalY = localX * sinM + localY * cosM + cyMain;
                                hitsOut?.Add(new SubHit
                                {
                                    Label = sub.Label ?? "",
                                    Center = new System.Drawing.PointF(globalX, globalY),
                                    Width = (float)hit.Width,
                                    Height = (float)hit.Height,
                                    AngleDeg = (float)hit.AngleDeg + mainInflated._rectRotation,
                                    Score = (float)hit.Score,
                                });
                                // Nếu caller không cần list hits → early exit ở hit đầu (Q2).
                                if (hitsOut == null) return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Global.LogsDashboard?.AddLog(
                                new LogEntry(DateTime.Now, LeveLLog.WARN,
                                    "Pattern.SubTemplate.Match",
                                    $"Label='{sub.Label}': {ex.Message}"));
                        }
                    }
                }
                finally { if (ownsGray) patchGray.Dispose(); }
            }
            // Trả true nếu có hit (Q2: ≥1).
            return subCount >= 1;
        }

        // ===== Resize Template (speed-up) =====
        public bool EnableResizeTemplate { get; set; } = false;
        public int ResizeTemplatePercent { get; set; } = 50; // 25..100

        private double EffectiveResizeRatio()
        {
            if (!EnableResizeTemplate) return 1.0;
            int p = ResizeTemplatePercent;
            if (p >= 100) return 1.0;
            if (p < 25) p = 25;
            return p / 100.0;
        }

        private const int MIN_TEMPLATE_DIM_AFTER_SCALE = 16;
        private static bool IsTemplateTooSmall(int w, int h, double ratio)
        {
            int minDim = Math.Min((int)Math.Floor(w * ratio), (int)Math.Floor(h * ratio));
            return minDim < MIN_TEMPLATE_DIM_AFTER_SCALE;
        }

        [NonSerialized]
        private double _batchLearnResizeRatio = 1.0;
        [NonSerialized]
        private double _singleLearnResizeRatio = 1.0;

        private double ResolveResizeRatioForTemplate(int w, int h, string context)
        {
            double ratio = EffectiveResizeRatio();
            if (ratio >= 1.0) return 1.0;
            if (!IsTemplateTooSmall(w, h, ratio)) return ratio;

            Global.LogsDashboard?.AddLog(
                new LogEntry(DateTime.Now, LeveLLog.WARN,
                    "Pattern.ResizeTemplate",
                    $"{context}: template {w}x{h} would become smaller than {MIN_TEMPLATE_DIM_AFTER_SCALE}px; using original size."));
            return 1.0;
        }

        private double ResolveResizeRatioForBatch()
        {
            double ratio = EffectiveResizeRatio();
            if (ratio >= 1.0) return 1.0;
            if (MultiTemplates == null || MultiTemplates.Count == 0) return ratio;

            foreach (var entry in MultiTemplates)
            {
                var bmp = entry?.GetBitmap();
                if (bmp == null) continue;
                if (!IsTemplateTooSmall(bmp.Width, bmp.Height, ratio)) continue;

                Global.LogsDashboard?.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.WARN,
                        "Pattern.ResizeTemplate",
                        $"Batch fallback: template '{entry.Label ?? ""}' {bmp.Width}x{bmp.Height} would become smaller than {MIN_TEMPLATE_DIM_AFTER_SCALE}px; using original size for the batch."));
                return 1.0;
            }

            return ratio;
        }

        private static RectRotate ScaleRectRotate(RectRotate src, double ratio)
        {
            if (src == null || ratio >= 1.0 || ratio <= 0.0) return src;
            var c = src.Clone();
            c._rect = new System.Drawing.RectangleF(
                (float)(src._rect.X * ratio),
                (float)(src._rect.Y * ratio),
                (float)(src._rect.Width * ratio),
                (float)(src._rect.Height * ratio));
            c._PosCenter = new System.Drawing.PointF(
                (float)(src._PosCenter.X * ratio),
                (float)(src._PosCenter.Y * ratio));
            if (c.PolyLocalPoints != null && c.PolyLocalPoints.Count > 0)
            {
                c.PolyLocalPoints = c.PolyLocalPoints
                    .Select(p => new System.Drawing.PointF(
                        (float)(p.X * ratio),
                        (float)(p.Y * ratio)))
                    .ToList();
            }
            if (c.HexVertexOffsets != null)
            {
                for (int i = 0; i < c.HexVertexOffsets.Length; i++)
                {
                    c.HexVertexOffsets[i] = new System.Drawing.PointF(
                        (float)(c.HexVertexOffsets[i].X * ratio),
                        (float)(c.HexVertexOffsets[i].Y * ratio));
                }
            }
            return c;
        }

        private static double ScaleBack(double value, double ratio)
        {
            return (ratio > 0.0 && ratio < 1.0) ? value / ratio : value;
        }

        private static Mat ResizeMat(Mat src, double ratio)
        {
            if (src == null || src.Empty() || ratio >= 1.0 || ratio <= 0.0) return null;
            var dst = new Mat();
            Cv2.Resize(src, dst, new Size(), ratio, ratio, InterpolationFlags.Area);
            return dst;
        }

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
            // Single Learn vẫn chạy bình thường ngay cả khi IsMultiTemplate=true — kết quả
            // bmRaw được user dùng cho "Add (last learn)" trong section Multi-Template.
            // Batch sẽ được học lazy trong DoWork (xem _batchLearned flag).

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
                        using (var m = Mat.FromPixelData(h, w, mt, intpr, s))
                        {
                            // CopyTo hoặc Clone đều OK; Clone gọn hơn:
                            mat = m.Clone();
                        }
                    }



                    Mat resizedTemplate = null;
                    try
                    {
                        Mat templateForLearn = IsNoCrop ? img : mat;
                        if (TypeMode == Mode.Pattern && templateForLearn != null && !templateForLearn.Empty())
                        {
                            double learnRatio = ResolveResizeRatioForTemplate(
                                templateForLearn.Width,
                                templateForLearn.Height,
                                "Single");
                            _singleLearnResizeRatio = learnRatio;
                            resizedTemplate = ResizeMat(templateForLearn, learnRatio);
                            if (resizedTemplate != null && !resizedTemplate.Empty())
                            {
                                Pattern.SetImgeSampleNoCrop(
                                    resizedTemplate.Data,
                                    resizedTemplate.Width,
                                    resizedTemplate.Height,
                                    (int)resizedTemplate.Step(),
                                    resizedTemplate.Channels());
                                GC.KeepAlive(resizedTemplate);
                            }
                        }

                        Pattern.LearnPatternStable();
                    }
                    finally
                    {
                        if (resizedTemplate != null && !resizedTemplate.IsDisposed)
                            resizedTemplate.Dispose();
                    }
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
            if (area == null || local == null) return local?.Clone();
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
                Score = local.Score,
                TypeCrop = local.TypeCrop,
                Name = local.Name
            };
        }

        public RectRotate AreaLocalToGlobal(RectRotate local)
        {
            return AreaLocalToGlobal(rotArea, local);
        }

        public RectRotate AreaGlobalToLocal(RectRotate global)
        {
            return AreaGlobalToLocal(rotArea, global);
        }

        private static RectRotate AreaLocalToGlobal(RectRotate area, RectRotate local)
        {
            if (area == null || local == null) return local == null ? null : new RectRotate(
                local._rect,
                local._PosCenter,
                local._rectRotation,
                local._dragAnchor)
            {
                Shape = local.Shape,
                Score = local.Score,
                TypeCrop = local.TypeCrop,
                Name = local.Name
            };

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
                local._dragAnchor)
            {
                Shape = local.Shape,
                Score = local.Score,
                TypeCrop = local.TypeCrop,
                Name = local.Name
            };
        }

        private static RectRotate AreaGlobalToLocal(RectRotate area, RectRotate global)
        {
            if (area == null || global == null) return global == null ? null : new RectRotate(
                global._rect,
                global._PosCenter,
                global._rectRotation,
                global._dragAnchor)
            {
                Shape = global.Shape,
                Score = global.Score,
                TypeCrop = global.TypeCrop,
                Name = global.Name
            };
            float thetaA = area._rectRotation;
            double sinA = Math.Sin(-thetaA * Math.PI / 180.0);
            double cosA = Math.Cos(-thetaA * Math.PI / 180.0);
            double dx = global._PosCenter.X - area._PosCenter.X;
            double dy = global._PosCenter.Y - area._PosCenter.Y;
            double interX = dx * cosA - dy * sinA;
            double interY = dx * sinA + dy * cosA;

            return new RectRotate(
                new RectangleF(-global._rect.Width / 2f, -global._rect.Height / 2f, global._rect.Width, global._rect.Height),
                new PointF((float)(interX - area._rect.X), (float)(interY - area._rect.Y)),
                global._rectRotation - area._rectRotation,
                global._dragAnchor)
            {
                Shape = global.Shape,
                Score = global.Score,
                TypeCrop = global.TypeCrop,
                Name = global.Name
            };
        }

        public RectRotate ConstrainAreaToContainLocalLimits(RectRotate proposedArea)
        {
            if (proposedArea == null) return null;
            var constrained = proposedArea.Clone();
            if (!IsMultiTemplate || !CheckByAreaLimit || MultiTemplates == null || MultiTemplates.Count == 0)
                return constrained;

            float baseW = Math.Max(1f, rotArea?._rect.Width ?? constrained._rect.Width);
            float baseH = Math.Max(1f, rotArea?._rect.Height ?? constrained._rect.Height);
            float maxAbsX = constrained._rect.Width / 2f;
            float maxAbsY = constrained._rect.Height / 2f;

            foreach (var entry in MultiTemplates)
            {
                var limit = entry?.RotLimit;
                if (limit == null || limit._rect.Width <= 0 || limit._rect.Height <= 0)
                    continue;

                float cx = limit._PosCenter.X - baseW / 2f;
                float cy = limit._PosCenter.Y - baseH / 2f;
                float hw = limit._rect.Width / 2f;
                float hh = limit._rect.Height / 2f;
                double rad = limit._rectRotation * Math.PI / 180.0;
                float cos = (float)Math.Cos(rad);
                float sin = (float)Math.Sin(rad);
                var corners = new[]
                {
                    new PointF(-hw, -hh),
                    new PointF(hw, -hh),
                    new PointF(hw, hh),
                    new PointF(-hw, hh)
                };

                foreach (var p in corners)
                {
                    float x = cx + p.X * cos - p.Y * sin;
                    float y = cy + p.X * sin + p.Y * cos;
                    maxAbsX = Math.Max(maxAbsX, Math.Abs(x));
                    maxAbsY = Math.Max(maxAbsY, Math.Abs(y));
                }
            }

            float width = Math.Max(constrained._rect.Width, (float)Math.Ceiling(maxAbsX * 2f));
            float height = Math.Max(constrained._rect.Height, (float)Math.Ceiling(maxAbsY * 2f));
            constrained._rect = new RectangleF(-width / 2f, -height / 2f, width, height);
            return constrained;
        }

        public void RebaseLocalLimitsForAreaSize(RectRotate oldArea, RectRotate newArea)
        {
            if (oldArea == null || newArea == null || MultiTemplates == null) return;
            float dx = (newArea._rect.Width - oldArea._rect.Width) / 2f;
            float dy = (newArea._rect.Height - oldArea._rect.Height) / 2f;
            if (Math.Abs(dx) < 0.001f && Math.Abs(dy) < 0.001f) return;

            foreach (var entry in MultiTemplates)
            {
                if (entry?.RotLimit == null) continue;
                entry.RotLimit._PosCenter = new PointF(
                    entry.RotLimit._PosCenter.X + dx,
                    entry.RotLimit._PosCenter.Y + dy);
            }
            MarkBatchDirty();
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

        //                using (var nativeMask = Mat.FromPixelData(h, w, mt, ptr, s))
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

                        using (var nativeMask = Mat.FromPixelData(h, w, mt, ptr, s))
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
                if (rotArea == null) rotArea = new RectRotate();
                if (rotCrop == null) rotCrop = new RectRotate();
                if (rotMask == null) rotMask = new RectRotate();
                if (rotLimit == null) rotLimit = new RectRotate();
                rotCrop.Name = "Area Temp";
                rotCrop.TypeCrop = TypeCrop.Crop;
                rotMask.Name = "Area Mask";
                rotMask.TypeCrop = TypeCrop.Mask;
                rotArea.Name = "Area Check";
                rotArea.TypeCrop = TypeCrop.Area;

                rotLimit.Name = "Area Limit";
                rotLimit.TypeCrop = TypeCrop.Limit;
                //rotCrop.Name = "Area ";
                //rotCrop.TypeCrop = TypeCrop.Crop;

                //rotMask.Name = "Area Mask";
                //rotMask.TypeCrop = TypeCrop.Mask;

                //rotArea.Name = "Area Check";
                //rotArea.TypeCrop = TypeCrop.Area;
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
        // Chống lệch góc khi keo/nhiễu ăn vào biên dạng template (occlusion-robust angle refine).
        // Mặc định tắt để giữ nguyên kết quả pattern cũ; bật riêng cho tool bị dính keo.
        public bool EnableAngleRobustRefine = false;
        public bool UseCpu = true;
        public bool UseGpu = false;
        public bool EnableMultiThread = false;
        public int NumThreads = 1;
        public int ScalePattern = 0;
        public int ScaleStep = 0;

        // ===== Multi-template helpers =====

        /// <summary>
        /// Build Pattern2StableConfig từ tool fields (angle, preset, GPU, etc.).
        /// Caller set <see cref="Pattern2StableConfig.MinAcceptScore"/> và
        /// <see cref="Pattern2StableConfig.MaxPos"/> riêng (tuỳ run-time score/MaxObject).
        /// </summary>
        private Pattern2StableConfig BuildStableConfig()
        {
            var cfg = new Pattern2StableConfig(true);
            cfg.AngleStartDeg = AngleLower;
            cfg.AngleEndDeg = AngleUper;
            cfg.AngleStepDeg = StepAngle;
            cfg.MaxOverlap = OverLap;
            cfg.BitwiseNot = ckBitwiseNot;
            cfg.SubPixel = ckSubPixel;
            cfg.Difficulty = (Pattern2DifficultyLevel)(int)DifficultyPattern;
            cfg.EnableValidator = EnableValidator;
            cfg.EnableKeepFilter = EnableKeepFilter;
            cfg.EnableNms = EnableNms;
            cfg.EnableAutoThreshold = true;
            cfg.EnableScaleSearch = EnableScaleSearch && !EnableResizeTemplate;
            cfg.ScaleMin = (100 - ScalePattern) / 100.0;
            cfg.ScaleMax = (100 + ScalePattern) / 100.0;
            cfg.ScaleStep = ScaleStep / 100.0;
            cfg.EnableGpu = UseGpu;
            cfg.EnableCpuMultiThread = EnableMultiThread;
            cfg.CpuThreads = NumThreads;
            cfg.EnableAngleRobustRefine = EnableAngleRobustRefine;
            cfg.DebugLog = false;
            return cfg;
        }

        /// <summary>
        /// Convert (Cx, Cy, angle) from rotLimit-local (native crop frame) to rotArea-local
        /// (frame expected by ProcessBatchResults / AddMatchResult / ToAbsoluteRectRotate).
        /// Native CropRotated returns coords where (0,0) là top-left của cropped image.
        /// </summary>
        private static void TransformLimitLocalToAreaLocal(
            RectRotate rotLimit, RectRotate rotArea,
            double cxLimit, double cyLimit, double angleLimit,
            out double cxArea, out double cyArea, out double angleArea)
        {
            // Step 1: rotLimit-local → global image coords.
            // Cropped image origin (0,0) ứng với top-left của rotLimit._rect → tâm rect = (W/2, H/2)
            // trong local. Shift về center-relative rồi rotate quanh rotLimit._PosCenter.
            float thetaL = rotLimit._rectRotation;
            double sinL = Math.Sin(thetaL * Math.PI / 180.0);
            double cosL = Math.Cos(thetaL * Math.PI / 180.0);
            double dxL = cxLimit + rotLimit._rect.X;   // _rect.X usually = -W/2
            double dyL = cyLimit + rotLimit._rect.Y;   // _rect.Y usually = -H/2
            double gx = rotLimit._PosCenter.X + dxL * cosL - dyL * sinL;
            double gy = rotLimit._PosCenter.Y + dxL * sinL + dyL * cosL;

            // Step 2: global → rotArea-local (inverse của ToAbsoluteRectRotate).
            //   intermediate = inverse-rotate(global - rotArea._PosCenter, -rotArea._rectRotation)
            //   local._PosCenter = intermediate - rotArea._rect.{X,Y}
            float thetaA = rotArea._rectRotation;
            double sinA = Math.Sin(-thetaA * Math.PI / 180.0);
            double cosA = Math.Cos(-thetaA * Math.PI / 180.0);
            double dxG = gx - rotArea._PosCenter.X;
            double dyG = gy - rotArea._PosCenter.Y;
            double interX = dxG * cosA - dyG * sinA;
            double interY = dxG * sinA + dyG * cosA;
            cxArea = interX - rotArea._rect.X;
            cyArea = interY - rotArea._rect.Y;

            // Angle: ToAbsoluteRectRotate dùng area.rotation + local.rotation = global.
            // global angle = rotLimit.rotation + native angle. → local-area angle = global - area.rotation.
            angleArea = thetaL + angleLimit - thetaA;
        }

        /// <summary>
        /// Per-label area limit flow: từng entry crop raw theo entry.RotLimit, học template
        /// + match qua single API, gắn label vào kết quả. Trả về list giống MatchBatchStable
        /// để tái dùng ProcessBatchResults logic.
        /// Chậm hơn MatchBatchStable (re-learn mỗi DoWork) nhưng cần thiết vì native batch
        /// chỉ hỗ trợ 1 source chung. Sẽ mark batch dirty để khi tắt CheckByAreaLimit, batch
        /// chuẩn được học lại.
        /// </summary>
        private List<Pattern2BatchResult> RunBatchWithPerLabelAreaLimit(
            Pattern2StableConfig cfg, Mat raw, RectRotate rotArea, RectRotate rotMask)
        {
            _batchLearned = false; // dùng single API → batch state bị invalidate
            var results = new List<Pattern2BatchResult>();
            if (MultiTemplates == null || MultiTemplates.Count == 0) return results;
            if (raw == null || raw.Empty()) return results;

            for (int idx = 0; idx < MultiTemplates.Count; idx++)
            {
                var entry = MultiTemplates[idx];
                if (entry == null) continue;
                var bmp = entry.GetBitmap();
                if (bmp == null) continue;

                // Per-entry config: clone + override threshold/MaxPos/angle theo entry.
                // Crop mode: phải cho phép tìm nhiều match/label (nhiều hình trong vùng
                // rotLimit). MaxPos default = max(MaxObject, ExpectedCount, 1) để bao quát
                // case user set Expect lớn hơn MaxObject global.
                Pattern2StableConfig perCfg = cfg;
                perCfg.MinAcceptScore = (entry.ScoreThreshold > 0f) ? entry.ScoreThreshold / 100.0 : cfg.MinAcceptScore;
                int perMax = Math.Max(1, MaxObject);
                if (entry.ExpectedCount > perMax) perMax = entry.ExpectedCount;
                if (entry.MaxPerTemplate > 0) perMax = entry.MaxPerTemplate;
                perCfg.MaxPos = perMax;
                if (entry.HasAngleRange)
                {
                    // entry.AngleLower/Upper lưu ở frame rotArea (DeltaAngle = rotCrop - rotArea).
                    // Khi crop bằng rotLimit, native search ở frame rotLimit nên phải shift:
                    //   rotCrop - rotLimit = (rotCrop - rotArea) + (rotArea - rotLimit)
                    // → cộng offset (rotArea.rotation - rotLimit.rotation).
                    double offset = 0.0;
                    RectRotate limitGlobalForAngle =  AreaLocalToGlobal(rotArea, entry.RotLimit);
                    bool hasLimitForAngle = limitGlobalForAngle != null
                        && limitGlobalForAngle._rect.Width > 0 && limitGlobalForAngle._rect.Height > 0;
                    if (hasLimitForAngle && rotArea != null)
                        offset = rotArea._rectRotation - limitGlobalForAngle._rectRotation;
                    perCfg.AngleStartDeg = entry.AngleLower + offset;
                    perCfg.AngleEndDeg = entry.AngleUpper + offset;
                }

                double entryRatio = ResolveResizeRatioForTemplate(
                    bmp.Width,
                    bmp.Height,
                    "AreaLimit[" + (entry.Label ?? idx.ToString()) + "]");

                try
                {
                    // 1) Học template từ entry bitmap.
                    using (Mat m = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp))
                    {
                        Mat gray = m;
                        bool ownsGray = false;
                        Mat resizedTemplate = null;
                        if (m.Channels() != 1)
                        {
                            gray = new Mat();
                            Cv2.CvtColor(m, gray,
                                m.Channels() == 4 ? ColorConversionCodes.BGRA2GRAY
                                                  : ColorConversionCodes.BGR2GRAY);
                            ownsGray = true;
                        }
                        try
                        {
                            Mat templateForLearn = gray;
                            resizedTemplate = ResizeMat(gray, entryRatio);
                            if (resizedTemplate != null && !resizedTemplate.Empty())
                                templateForLearn = resizedTemplate;

                            GC.KeepAlive(templateForLearn);
                            Pattern.SetImgeSampleNoCrop(templateForLearn.Data, templateForLearn.Width, templateForLearn.Height,
                                (int)templateForLearn.Step(), templateForLearn.Channels());
                            Pattern.LearnPatternStable(perCfg);
                        }
                        finally
                        {
                            if (resizedTemplate != null && !resizedTemplate.IsDisposed)
                                resizedTemplate.Dispose();
                            if (ownsGray) gray.Dispose();
                        }
                    }

                    // 2) Nạp ảnh: nếu có RotLimit hợp lệ, dùng SetImgeRaw để native crop +
                    //    map kết quả về toạ độ global. Nếu thiếu RotLimit, fallback dùng raw
                    //    full image (tương đương Full mode cho entry này).
                    RectRotate limitGlobal = AreaLocalToGlobal(rotArea, entry.RotLimit);
                    Mat rawForMatch = raw;
                    Mat resizedRaw = null;
                    try
                    {
                        resizedRaw = ResizeMat(raw, entryRatio);
                        if (resizedRaw != null && !resizedRaw.Empty())
                            rawForMatch = resizedRaw;

                    if (limitGlobal != null &&
                        limitGlobal._rect.Width > 0 && limitGlobal._rect.Height > 0)
                    {
                        var rrCli = Converts.ToCli(ScaleRectRotate(limitGlobal, entryRatio));
                        Pattern.SetImgeRaw(rawForMatch.Data, rawForMatch.Width, rawForMatch.Height,
                            (int)rawForMatch.Step(), rawForMatch.Channels(), rrCli, null);
                    }
                    else
                    {
                        Pattern.SetRawNoCrop(rawForMatch.Data, rawForMatch.Width, rawForMatch.Height,
                            (int)rawForMatch.Step(), rawForMatch.Channels());
                    }
                    GC.KeepAlive(rawForMatch);

                    // 3) Match single API.
                    var rs = Pattern.MatchStable(perCfg);
                    if (rs == null) continue;

                    // 4) Transform native coords (rotLimit-local) → rotArea-local nếu có
                    //    rotLimit; nếu không (Full fallback), giữ nguyên (rotArea-local).
                    bool hasLimit = limitGlobal != null
                        && limitGlobal._rect.Width > 0 && limitGlobal._rect.Height > 0;
                    foreach (var r in rs)
                    {
                        double cx = ScaleBack(r.Cx, entryRatio);
                        double cy = ScaleBack(r.Cy, entryRatio);
                        double angle = r.AngleDeg;
                        if (hasLimit && rotArea != null)
                        {
                            TransformLimitLocalToAreaLocal(
                                limitGlobal, rotArea, cx, cy, r.AngleDeg,
                                out cx, out cy, out angle);
                        }
                        var br = new Pattern2BatchResult();
                        br.TemplateIndex = idx;
                        br.Label = entry.Label ?? "";
                        br.Cx = cx;
                        br.Cy = cy;
                        br.AngleDeg = angle;
                        br.Width = ScaleBack(r.Width, entryRatio);
                        br.Height = ScaleBack(r.Height, entryRatio);
                        br.Score = r.Score;
                        results.Add(br);
                    }
                    }
                    finally
                    {
                        if (resizedRaw != null && !resizedRaw.IsDisposed)
                            resizedRaw.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Global.LogsDashboard?.AddLog(
                        new LogEntry(DateTime.Now, LeveLLog.ERROR,
                            "Pattern2.AreaLimit[" + (entry.Label ?? idx.ToString()) + "]",
                            ex.ToString()));
                }
            }
            return results;
        }

        /// <summary>
        /// Học tất cả template trong MultiTemplates qua native batch API.
        /// Tất cả template dùng cùng preprocess (shared cfg) — đây là contract của native batch.
        /// Public để UI gọi sau khi mutate MultiTemplates; cũng được DoWork gọi lazy.
        /// </summary>
        public void LearnPatternsBatch()
        {
            _batchLearned = false;
            if (MultiTemplates == null || MultiTemplates.Count == 0) return;

            var learnCfg = BuildStableConfig();
            double batchRatio = ResolveResizeRatioForBatch();
            _batchLearnResizeRatio = batchRatio;
            Pattern.LearnPatternBatchBegin();
            try
            {
                foreach (var e in MultiTemplates)
                {
                    var bmp = e.GetBitmap();
                    if (bmp == null) continue;
                    using (Mat m = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp))
                    {
                        Mat gray = m;
                        bool ownsGray = false;
                        Mat resizedTemplate = null;
                        if (m.Channels() != 1)
                        {
                            gray = new Mat();
                            Cv2.CvtColor(m, gray,
                                m.Channels() == 4 ? ColorConversionCodes.BGRA2GRAY
                                                  : ColorConversionCodes.BGR2GRAY);
                            ownsGray = true;
                        }
                        try
                        {
                            Mat templateForLearn = gray;
                            resizedTemplate = ResizeMat(gray, batchRatio);
                            if (resizedTemplate != null && !resizedTemplate.Empty())
                                templateForLearn = resizedTemplate;

                            var tplCfg = new Pattern2BatchTemplateConfig(
                                e.Label ?? "",
                                e.ScoreThreshold / 100.0,
                                e.ExpectedCount);
                            tplCfg.MaxPerTemplate = e.MaxPerTemplate;
                            if (e.HasAngleRange)
                            {
                                tplCfg.HasAngleRange = true;
                                tplCfg.AngleStartDeg = e.AngleLower;
                                tplCfg.AngleEndDeg = e.AngleUpper;
                            }
                            GC.KeepAlive(templateForLearn);
                            Pattern.AddBatchTemplate(
                                templateForLearn.Data, templateForLearn.Width, templateForLearn.Height,
                                (int)templateForLearn.Step(), templateForLearn.Channels(),
                                learnCfg, tplCfg);
                        }
                        finally
                        {
                            if (resizedTemplate != null && !resizedTemplate.IsDisposed)
                                resizedTemplate.Dispose();
                            if (ownsGray) gray.Dispose();
                        }
                    }
                }
                Pattern.LearnPatternBatchEnd();
                _batchLearned = true;
            }
            catch (Exception ex)
            {
                _batchLearned = false;
                _batchLearnResizeRatio = 1.0;
                Global.LogsDashboard?.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR, "Pattern2.Batch.Learn", ex.ToString()));
            }
        }

        /// <summary>
        /// Xử lý kết quả từ MatchBatchStable: filter theo angle range + global threshold,
        /// đếm match per label, judge OK/NG theo expected count.
        /// Push từng match vào ResultItems với <see cref="ResultItem.Name"/> = label.
        /// </summary>
        /// <summary>
        /// Single mode sub-template validation. SubTemplatesSingle lưu ở Patterns instance
        /// (không phải per-entry như Multi). Mark item.IsOK=false nếu fail; Complete() pick up.
        /// </summary>
        private void ApplySingleSubValidation(ResultItem item, RectRotate rotArea, RectRotate localRect, Mat raw)
        {
            // Stash hits cho mọi instance để DrawResult vẽ; index align với rectRotates.
            var hits = new List<SubHit>();
            SubHitsPerItem.Add(hits);
            if (item == null) return;
            if (_subTemplatesSingle == null || _subTemplatesSingle.Count == 0) return;
            var absolute = ToAbsoluteRectRotate(rotArea, localRect);
            bool ok = ValidateSubTemplatesForInstance(
                _subTemplatesSingle, SubSearchPadPxSingle, absolute, raw, out _, hits);
            if (!ok) item.IsOK = false;
        }

        private void ProcessBatchResults(List<Pattern2BatchResult> listBatch, RectRotate rotArea, double resultScaleRatio, Mat raw)
        {
            var owner = Common.TryGetTool(Global.IndexProgChoose, Index);
            var globalThreshold = Common.TryGetTool(IndexThread, Index).Score; // 0..100

            // Map label → entry để lookup sub-templates O(1) trong loop.
            var entryByLabel = new Dictionary<string, Pattern2TemplateEntry>(StringComparer.Ordinal);
            foreach (var e in MultiTemplates)
            {
                var key = e.Label ?? "";
                if (!entryByLabel.ContainsKey(key)) entryByLabel[key] = e;
            }

            // BestObj filter: chỉ giữ match top-score cho MỖI label (không phải top-1 tổng thể).
            // Bước này áp lên listBatch trước khi đếm/đẩy vào ResultItems.
            if (SearchPattern == SearchPattern.BestObj && listBatch != null && listBatch.Count > 0)
            {
                var bestPerLabel = new Dictionary<string, Pattern2BatchResult>(StringComparer.Ordinal);
                foreach (var r in listBatch)
                {
                    string lbl = r.Label ?? "";
                    if (!bestPerLabel.TryGetValue(lbl, out var cur) || r.Score > cur.Score)
                        bestPerLabel[lbl] = r;
                }
                listBatch = new List<Pattern2BatchResult>(bestPerLabel.Values);
            }

            // Init counters theo MultiTemplates (đảm bảo label thiếu match cũng có entry).
            var perLabelCount = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var e in MultiTemplates)
            {
                if (!perLabelCount.ContainsKey(e.Label ?? ""))
                    perLabelCount[e.Label ?? ""] = 0;
            }

            float scoreSum = 0f;
            int kept = 0;

            if (listBatch != null)
            {
                foreach (var r in listBatch)
                {
                    // Skip global AngleLower/Upper filter: native batch đã enforce per-template
                    // angle range (xem Pattern2BatchTemplateConfig.HasAngleRange).
                    if (r.Score < globalThreshold) continue;

                    float w = (float)ScaleBack(r.Width, resultScaleRatio);
                    float h = (float)ScaleBack(r.Height, resultScaleRatio);
                    var pCenter = new System.Drawing.PointF(
                        (float)ScaleBack(r.Cx, resultScaleRatio),
                        (float)ScaleBack(r.Cy, resultScaleRatio));
                    float angle = (float)r.AngleDeg;
                    float score = (float)r.Score;
                    scoreSum += score;
                    kept++;

                    var localRect = new RectRotate(
                        new System.Drawing.RectangleF(-w / 2f, -h / 2f, w, h),
                        pCenter, angle, AnchorPoint.None);

                    // Sub-template per-instance check (Q2). Tìm entry theo label; nếu entry
                    // có SubTemplates → validate trước khi count. Fail → vẫn AddMatchResult
                    // (để UI thấy NG instance) nhưng không cộng vào perLabelCount.
                    string lbl = r.Label ?? "";
                    bool subOk = true;
                    var hits = new List<SubHit>();
                    if (entryByLabel.TryGetValue(lbl, out var entry)
                        && entry != null
                        && entry.SubTemplates != null
                        && entry.SubTemplates.Count > 0)
                    {
                        var absolute = ToAbsoluteRectRotate(rotArea, localRect);
                        subOk = ValidateSubTemplatesForInstance(
                            entry.SubTemplates, entry.SubSearchPadPx, absolute, raw, out _, hits);
                    }

                    var item = AddMatchResult(rotArea, localRect, score);
                    SubHitsPerItem.Add(hits);
                    if (item != null)
                    {
                        // Tận dụng ResultItem.Name làm label (tránh thêm field mới phá serialize).
                        item.Name = r.Label ?? "";
                        if (!subOk) item.IsOK = false;
                    }

                    if (subOk && perLabelCount.ContainsKey(lbl))
                        perLabelCount[lbl] = perLabelCount[lbl] + 1;
                }
            }

            // Judge OK/NG. MultiNoLimit: aggregate per unique Label (ExpectedCount = max của
            // các row cùng label), vì nhiều row có thể chia sẻ Label. Multi: judge per-row
            // (giữ behavior cũ — mỗi row 1 label độc lập).
            bool overallOk = true;
            if (TemplateMode == PatternMode.MultiNoLimit)
            {
                var expectedPerLabel = new Dictionary<string, int>(StringComparer.Ordinal);
                foreach (var e in MultiTemplates)
                {
                    string lbl = e.Label ?? "";
                    int cur = expectedPerLabel.TryGetValue(lbl, out int v) ? v : 0;
                    if (e.ExpectedCount > cur) expectedPerLabel[lbl] = e.ExpectedCount;
                    else if (!expectedPerLabel.ContainsKey(lbl)) expectedPerLabel[lbl] = e.ExpectedCount;
                }
                foreach (var kv in expectedPerLabel)
                {
                    if (kv.Value <= 0) continue;
                    int found = perLabelCount.TryGetValue(kv.Key, out int c) ? c : 0;
                    if (found < kv.Value) { overallOk = false; break; }
                }
            }
            else
            {
                foreach (var e in MultiTemplates)
                {
                    e.IsOK = true;
                    if (e.ExpectedCount <= 0) continue; // 0 = optional
                    string lbl = e.Label ?? "";
                    int found = perLabelCount.TryGetValue(lbl, out int c) ? c : 0;
                    if (found < e.ExpectedCount) {
                        e.IsOK = false; overallOk = false;  }
                }
            }

            owner.Results = overallOk ? Results.OK : Results.NG;
            owner.ScoreResult = (kept > 0) ? (int)Math.Round(scoreSum / kept, 1) : 0;
        }

        public void DoWork(RectRotate rotArea, RectRotate rotMask)
        {
          //  lock (RuntimeLock)
            {
                if (!Global.IsRun)
                {
                    float DeltaAngle = (rotCrop._rectRotation) - (rotArea._rectRotation);
                    AngleLower = DeltaAngle - Angle;
                    AngleUper = DeltaAngle + Angle;
                }
            Common.TryGetTool(Global.IndexProgChoose, Index).ScoreResult = 0;
            Common.TryGetTool(Global.IndexProgChoose, Index).Results = Results.NG;
            // 5) Gom kết quả
            rectRotates = new List<RectRotate>();
            ClearResultItems();
            listP_Center = new List<System.Drawing.Point>();
            list_AngleCenter = new List<float>();
            SubHitsPerItem = new List<List<SubHit>>();
            using (Mat raw = BeeCore.Common.listCamera[IndexCCD].matRaw.Clone())
            {
                if (raw.Empty()) return;
                if (raw.Type() == MatType.CV_8UC3)
                {
                   
                    Cv2.CvtColor(raw, raw, ColorConversionCodes.BGR2GRAY);
                }
               


                Mat matProcess = null;
                byte[] rentedBuffer = null;
                double resizeRatio = 1.0;

                try
                {
                    // 1) Crop ROI

                    //  matCrop = Cropper.CropRotatedRect(raw, rotArea, rotMask);

                    // 2) Tiền xử lý theo chế độ
                    if (TypeMode == Mode.Pattern)
                    {
                        if (IsMultiTemplate && CheckByAreaLimit && TemplateMode == PatternMode.Multi)
                        {
                            resizeRatio = 1.0;
                        }
                        else if (IsMultiTemplate)
                        {
                            resizeRatio = ResolveResizeRatioForBatch();
                        }
                        else if (bmRaw != null)
                        {
                            resizeRatio = _singleLearnResizeRatio > 0.0 ? _singleLearnResizeRatio : 1.0;
                        }
                        else
                        {
                            resizeRatio = EffectiveResizeRatio();
                        }
                    }

                    switch (TypeMode)
                    {
                        case Mode.Pattern:
                            Mat rawForPattern = raw;
                            Mat resizedRawForPattern = ResizeMat(raw, resizeRatio);
                            if (resizedRawForPattern != null && !resizedRawForPattern.Empty())
                                rawForPattern = resizedRawForPattern;
                            else if (resizedRawForPattern != null)
                                resizedRawForPattern.Dispose();

                            matProcess = rawForPattern; // reuse backing store

                            // Q7: Single-mode sub-policy quyết định crop source.
                            // - None (default): SetImgeRaw(rotArea, rotMask) — match toàn ROI.
                            // - AreaLimit: SetImgeRaw(rotLimit) thay rotArea.
                            // - NoLimit: SetRawNoCrop — bỏ qua ROI hoàn toàn.
                            // Multi modes vẫn dùng nhánh None (backend Multi tự lo crop sau).
                            bool isSingleMode = (TemplateMode == PatternMode.Single);
                            var singlePolicy = _singleAreaPolicy;

                            if (isSingleMode && singlePolicy == SingleAreaPolicy.NoLimit)
                            {
                                Pattern.SetRawNoCrop(matProcess.Data, matProcess.Width, matProcess.Height,
                                    (int)matProcess.Step(), matProcess.Channels());
                            }
                            else if (isSingleMode && singlePolicy == SingleAreaPolicy.AreaLimit
                                     && rotLimit != null
                                     && rotLimit._rect.Width > 0 && rotLimit._rect.Height > 0)
                            {
                                var rrLimit = Converts.ToCli(ScaleRectRotate(rotLimit, resizeRatio));
                                RectRotateCli? rrMaskCliLim = (rotMask != null)
                                    ? Converts.ToCli(ScaleRectRotate(rotMask, resizeRatio))
                                    : (RectRotateCli?)null;
                                Pattern.SetImgeRaw(matProcess.Data, matProcess.Width, matProcess.Height,
                                    (int)matProcess.Step(), matProcess.Channels(), rrLimit, rrMaskCliLim);
                            }
                            else
                            {
                                var rrCli = Converts.ToCli(ScaleRectRotate(rotArea, resizeRatio));
                                RectRotateCli? rrMaskCli = (rotMask != null)
                                    ? Converts.ToCli(ScaleRectRotate(rotMask, resizeRatio))
                                    : (RectRotateCli?)null;
                                Pattern.SetImgeRaw(matProcess.Data, matProcess.Width, matProcess.Height,
                                    (int)matProcess.Step(), matProcess.Channels(), rrCli, rrMaskCli);
                            }

                            break;



                        case Mode.Edge:
                            Mat matCrop = new Mat();                               
                                PatchCropContext ctx = new PatchCropContext();
                                matCrop = Cropper.CropOuterPatch(raw, rotArea, out ctx);
                                if (matProcess == null) matProcess = new Mat();
                                if (!matProcess.IsDisposed)
                                    if (!matProcess.Empty()) matProcess.Dispose();

                                matProcess = Filters.ApplyEdgeMethod(matCrop, MethordEdge, ThresholdBinary);

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
                    cfg.EnableAngleRobustRefine = EnableAngleRobustRefine;

                    float ScaleMin =(float) ((100 - ScalePattern)/100.0);
                    float ScaleMax = (float)((100 +ScalePattern) / 100.0);
                    // scale mẫu
                    cfg.EnableScaleSearch = EnableScaleSearch && !EnableResizeTemplate;
                    cfg.DebugLog = false;
                    cfg.ScaleMin = ScaleMin;
                    cfg.ScaleMax = ScaleMax;
                    cfg.ScaleStep = ScaleStep/100.0;
                   // cfg.DebugLogPath = "E:\\pattern2_debug.txt";

                    // Multi-template branch: lazy-learn batch nếu chưa học, gọi native batch
                    // API, judge OK/NG theo expected count per label, rồi skip phần
                    // single-template processing bên dưới.
                    if (IsMultiTemplate)
                    {
                        List<Pattern2BatchResult> listBatch;
                        if (CheckByAreaLimit && TemplateMode == PatternMode.Multi)
                        {
                            // Mỗi label crop ảnh theo RotLimit trước khi match → native
                            // MatchBatchStable với source = crop của entry hiện tại. Kết quả
                            // đã trong toạ độ raw global (SetImgeRaw map ngược).
                            listBatch = RunBatchWithPerLabelAreaLimit(cfg, raw, rotArea, rotMask);
                            ProcessBatchResults(listBatch, rotArea, 1.0, raw);
                        }
                        else
                        {
                            if (!_batchLearned) LearnPatternsBatch();
                            listBatch = Pattern.MatchBatchStable(cfg);
                            ProcessBatchResults(listBatch, rotArea,
                                _batchLearnResizeRatio > 0.0 ? _batchLearnResizeRatio : resizeRatio,
                                raw);
                        }
                        return; // exit DoWork; finally block vẫn chạy.
                    }

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
                                        float w = (float)ScaleBack(rotBest.Width, resizeRatio);
                                        float h = (float)ScaleBack(rotBest.Height, resizeRatio);
                                        var pCenter = new System.Drawing.PointF(
                                            (float)ScaleBack(rotBest.Cx, resizeRatio),
                                            (float)ScaleBack(rotBest.Cy, resizeRatio));
                                        float angle = (float)rotBest.AngleDeg;
                                        float score = (float)rotBest.Score;
                                        scoreSum += score;
                                        var localRect = new RectRotate(
                                           new System.Drawing.RectangleF(-w / 2f, -h / 2f, w, h),
                                           pCenter, angle, AnchorPoint.None);
                                        var itemBest = AddMatchResult(rotArea, localRect, score);
                                        ApplySingleSubValidation(itemBest, rotArea, localRect, raw);
                                    }
                            }
                        else
                        {
                            foreach (Rotaterectangle rot in listRS)
                            {
                                float w = (float)ScaleBack(rot.Width, resizeRatio);
                                float h = (float)ScaleBack(rot.Height, resizeRatio);
                                var pCenter = new System.Drawing.PointF(
                                    (float)ScaleBack(rot.Cx, resizeRatio),
                                    (float)ScaleBack(rot.Cy, resizeRatio));
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
                                var item = AddMatchResult(rotArea, localRect, score);
                                ApplySingleSubValidation(item, rotArea, localRect, raw);
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
            var ownerTool = Common.TryGetTool(IndexThread, Index);

            // Multi-template: judge theo expected count PER LABEL thay vì global rectRotates
            // count. Mỗi label trong MultiTemplates có ExpectedCount; OK ↔ mọi label đạt đủ
            // (ExpectedCount > 0 → found ≥ expected; ExpectedCount == 0 → optional, skip).
            if (IsMultiTemplate)
            {
                ownerTool.Results = Results.OK;
                if (MultiTemplates != null && MultiTemplates.Count > 0)
                {
                    var foundPerLabel = new Dictionary<string, int>(StringComparer.Ordinal);
                    if (ResultItems != null)
                    {
                        foreach (var it in ResultItems)
                        {
                            if (it == null) continue;
                            string lbl = it.Name ?? "";
                            foundPerLabel[lbl] = foundPerLabel.TryGetValue(lbl, out int c) ? c + 1 : 1;
                        }
                    }
                    foreach (var e in MultiTemplates)
                    {
                        if (e.ExpectedCount <= 0) continue;
                        string lbl = e.Label ?? "";
                        int found = foundPerLabel.TryGetValue(lbl, out int c) ? c : 0;
                        if (found < e.ExpectedCount)
                        {
                            ownerTool.Results = Results.NG;
                            break;
                        }
                    }
                }
                if (EnableColorCheck && ResultItems != null && ResultItems.Any(x => x != null && !x.IsOK))
                    ownerTool.Results = Results.NG;
                // Sub-template per-instance NG (Q2): bất kỳ instance nào IsOK=false do
                // ValidateSubTemplatesForInstance fail → tool NG.
                if (ResultItems != null && ResultItems.Any(x => x != null && !x.IsOK))
                    ownerTool.Results = Results.NG;
                return;
            }

            ownerTool.Results = Results.OK;
            switch (Compare)
            {
                case Compares.Equal:
                    if (rectRotates.Count() != LimitCounter)
                        ownerTool.Results = Results.NG;
                    break;
                case Compares.Less:
                    if (rectRotates.Count() >= LimitCounter)
                        ownerTool.Results = Results.NG;
                    break;
                case Compares.More:
                    if (rectRotates.Count() <= LimitCounter)
                        ownerTool.Results = Results.NG;
                    break;
            }
            if (EnableColorCheck && ResultItems != null && ResultItems.Any(x => x != null && !x.IsOK))
                ownerTool.Results = Results.NG;
            // Single-mode sub-template per-instance NG (Q2).
            if (ResultItems != null && ResultItems.Any(x => x != null && !x.IsOK))
                ownerTool.Results = Results.NG;
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
            List<List<SubHit>> subHitsSnapshot;

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
                subHitsSnapshot = SubHitsPerItem != null
                    ? SubHitsPerItem.Select(h => h != null ? new List<SubHit>(h) : new List<SubHit>()).ToList()
                    : new List<List<SubHit>>();
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

			// Vẽ rotLimit của từng label (multi-mode + CheckByAreaLimit) ở toạ độ global.
			// User vẽ rotLimit trực tiếp trên ảnh raw → coords đã ở global frame.
			if (IsMultiTemplate && CheckByAreaLimit && MultiTemplates != null)
			{
				foreach (var entry in MultiTemplates)
				{
                        Color clNG = Color.DeepSkyBlue;
                        if (!entry.IsOK) clNG = Global.ParaShow.ColorNG;
                    if (entry == null || entry.RotLimit == null) continue;
					var rl = AreaLocalToGlobal(rotA, entry.RotLimit);
					if (rl == null) continue;
					if (rl._rect.Width <= 0 || rl._rect.Height <= 0) continue;
					var matLim = new Matrix();
					if (!Global.IsRun)
					{
						matLim.Translate(Global.pScroll.X, Global.pScroll.Y);
						matLim.Scale(Global.ScaleZoom, Global.ScaleZoom);
					}
					matLim.Translate(rl._PosCenter.X, rl._PosCenter.Y);
					matLim.Rotate(rl._rectRotation);
					gc.Transform = matLim;
					Draws.Box1Label(gc, rl, entry.Label ?? "", font, brushText,
                        clNG, Global.ParaShow.ThicknessLine);
					gc.ResetTransform();
				}
			}
			
			if (rectRotatesSnapshot != null && rectRotatesSnapshot.Count > 0)
			{
                int i = 1;
				foreach (RectRotate rot in rectRotatesSnapshot)
				{
                    ResultItem item = (resultItemsSnapshot != null && resultItemsSnapshot.Count >= i) ? resultItemsSnapshot[i - 1] : null;
                    Color clItem = (item == null || item.IsOK) ? Global.ParaShow.ColorOK : Global.ParaShow.ColorNG;

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
                            double x =Math.Round( ( listPCenterSnapshot[i - 1].X / Global.Config.Scale),1);
                            double y = Math.Round((listPCenterSnapshot[i - 1].Y / Global.Config.Scale),1);
                            String sPos = "X,Y,A _ " + x + "," + y + "," + Math.Round(listAngleCenterSnapshot[i - 1], 1);
                        //if (ZeroPos == ZeroPos.ZeroADJ)
                        //    sPos = "*X,Y,A _ " + listPCenterSnapshot[i - 1].X + "," + listPCenterSnapshot[i - 1].Y + "," + Math.Round(listAngleCenterSnapshot[i - 1], 1);

                        gc.DrawString(sPos, font, new SolidBrush(Global.ParaShow.ColorInfor), new PointF(5, 5));
                    }
                    string valueBottom = EnableColorCheck ? ((item != null) ? "NG: " + item.ValueColor + " Px" + "/" + ScoreNG : "NG: 0 Px"+ "/"+ ScoreNG) :"";
                    string scoreText = (item != null) ? Math.Round(item.Score, 1) + "%" : Math.Round(rot.Score, 1) + "%";
                    if (Global.ParaShow.IsShowScore || EnableColorCheck)
                    {
                        string scoreTextShow = Global.ParaShow.IsShowScore ? scoreText : "";
                        // Multi-template: show label (item.Name) thay vì index. Single mode
                        // hoặc item.Name rỗng → fallback index "1","2","3"...
                        string topLabel = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name : (i + "");
                        Draws.Box3Label(gc, rot, topLabel, scoreTextShow, valueBottom, font, clItem, brushText, 30, Global.ParaShow.ThicknessLine, false, Global.ParaShow.FontSize, 1, EnableColorCheck);
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

                    // ===== Draw sub-template hits (Q-extra) =====
                    // hits ở raw-global coords. Vẽ với transform screen-only (pScroll/zoom)
                    // → translate đến Center → rotate AngleDeg → vẽ rect (-w/2,-h/2,w,h).
                    if (subHitsSnapshot != null && (i - 1) < subHitsSnapshot.Count)
                    {
                        var hitsForItem = subHitsSnapshot[i - 1];
                        if (hitsForItem != null && hitsForItem.Count > 0)
                        {
                            using (var penSub = new Pen(Color.Lime, Math.Max(1, Global.ParaShow.ThicknessLine)))
                            using (var brushSub = new SolidBrush(Color.Lime))
                            using (var fontSub = new Font("Arial", Math.Max(8, Global.ParaShow.FontSize - 2)))
                            {
                                foreach (var h in hitsForItem)
                                {
                                    var matSub = new Matrix();
                                    if (!Global.IsRun)
                                    {
                                        matSub.Translate(Global.pScroll.X, Global.pScroll.Y);
                                        matSub.Scale(Global.ScaleZoom, Global.ScaleZoom);
                                    }
                                    matSub.Translate(h.Center.X, h.Center.Y);
                                    matSub.Rotate(h.AngleDeg);
                                    gc.Transform = matSub;
                                    float hw = h.Width * 0.5f, hh = h.Height * 0.5f;
                                    gc.DrawRectangle(penSub, -hw, -hh, h.Width, h.Height);
                                    // Plus marker tại center
                                    int crossSize = (int)Math.Max(4, Math.Min(h.Width, h.Height) / 6);
                                    gc.DrawLine(penSub, -crossSize, 0, crossSize, 0);
                                    gc.DrawLine(penSub, 0, -crossSize, 0, crossSize);
                                    // Label + score
                                    // h.Score đã ở scale 0..100; không nhân thêm.
                                    string lblTxt = (h.Label ?? "") + " " + Math.Round(h.Score, 1) + "%";
                                    gc.DrawString(lblTxt, fontSub, brushSub, hw + 2, -hh);
                                    gc.ResetTransform();
                                }
                            }
                        }
                    }

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
                    matProcess = Mat.FromPixelData(height, width, MatType.CV_8UC1, edgeBytes);
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
    //                        matProcess = Mat.FromPixelData(height, width, MatType.CV_8UC1, edgeBytes);
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

    /// <summary>
    /// 3-state mode cho Pattern tool.
    /// Single: 1 template duy nhất (single-image flow).
    /// Multi: nhiều template, mỗi row 1 Label độc lập, có thể bật CheckByAreaLimit để
    ///   crop ảnh theo per-label RotLimit trước khi match.
    /// MultiNoLimit: nhiều template, nhiều row có thể chia sẻ cùng Label (group). Bỏ qua
    ///   RotLimit hoàn toàn, judge OK/NG per unique label (ExpectedCount = max của các row
    ///   cùng label, threshold lấy min).
    /// </summary>
    public enum PatternMode
    {
        Single = 0,
        Multi = 1,
        MultiNoLimit = 2,
    }

    /// <summary>
    /// 6-method taxonomy. Là shim trên (TemplateMode + CheckByAreaLimit), không thay backend.
    /// </summary>
    public enum PatternMethod
    {
        SingleLabel              = 0,  // 1 template, match toàn ROI
        SingleLabelAreaLimit     = 1,  // 1 template, RotLimit crop trên ROI
        SingleLabelNoLimit       = 2,  // 1 template, raw full (bỏ qua ROI)
        MultiLabel               = 3,  // batch, mỗi row 1 Label độc lập
        MultiLabelAreaLimit      = 4,  // batch + per-row RotLimit
        MultiLabelNoLimit        = 5,  // batch + group rows by Label
    }

    /// <summary>
    /// 1 sub-template detected hit, lưu coords trong raw global frame. Dùng để vẽ overlay.
    /// </summary>
    public class SubHit
    {
        public string Label { get; set; } = "";
        public System.Drawing.PointF Center { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float AngleDeg { get; set; }
        public float Score { get; set; }
    }

    /// <summary>
    /// Sub-policy chỉ áp dụng cho Single mode (UI-only — không đổi backend flow hiện tại).
    /// </summary>
    public enum SingleAreaPolicy
    {
        None       = 0,  // match toàn ROI (default — Single mode legacy)
        AreaLimit  = 1,  // crop theo rotLimit (single-template RotLimit)
        NoLimit    = 2,  // bỏ qua ROI, match full raw
    }

    /// <summary>
    /// Một template trong multi-template mode của <see cref="Patterns"/>.
    /// Lưu bitmap dạng PNG byte[] để JSON-serialize được (Bitmap không serialize trực tiếp).
    /// </summary>
    [Serializable]
    public class Pattern2TemplateEntry
    {
        public string Label { get; set; } = "";
        public float ScoreThreshold { get; set; } = 70f;   // 0..100 (UI scale)
        public int ExpectedCount { get; set; } = 1;        // ≥0; 0 = optional
        public int MaxPerTemplate { get; set; } = 0;       // 0 = follow global MaxObject
        public bool IsOK = false;
        // Per-template angle range (deg). Lưu lúc Add từ DeltaAngle ± Angle. Override
        // cfg.AngleStart/EndDeg trong native batch khi check.
        public bool HasAngleRange { get; set; } = false;
        public double AngleLower { get; set; } = 0.0;
        public double AngleUpper { get; set; } = 0.0;

        // Per-label area limit in rotArea-local coordinates. UI/native/draw convert this
        // to global coordinates at runtime so the limit follows Area Check move/rotation.
        public RectRotate RotLimit { get; set; }

        // PNG bytes của template image. JSON sẽ serialize thành base64.
        public byte[] TemplatePng { get; set; }

        // ===== Sub-templates (validation after main detected) =====
        // Sau khi main được detect, crop bbox quanh main (inflate SubSearchPadPx) → match
        // từng sub-template; có ≥1 hit → instance OK. RequireSubPerInstance=true: mọi
        // main instance đều phải có ≥1 sub. false: chỉ cần 1 main bất kỳ có sub là OK.
        // Backward-compat: project JSON cũ thiếu field → lazy-init trong getter.
        private List<SubTemplateEntry> _subTemplates;
        public List<SubTemplateEntry> SubTemplates
        {
            get { return _subTemplates ?? (_subTemplates = new List<SubTemplateEntry>()); }
            set { _subTemplates = value; }
        }
        public int SubSearchPadPx { get; set; } = 20;
        public bool RequireSubPerInstance { get; set; } = true;

        [NonSerialized]
        private Bitmap _cachedBitmap;

        /// <summary>Lazy decode PNG -> Bitmap. Trả null nếu chưa có template.</summary>
        public Bitmap GetBitmap()
        {
            if (_cachedBitmap != null) return _cachedBitmap;
            if (TemplatePng == null || TemplatePng.Length == 0) return null;
            try
            {
                using (var ms = new System.IO.MemoryStream(TemplatePng))
                    _cachedBitmap = new Bitmap(ms);
            }
            catch { _cachedBitmap = null; }
            return _cachedBitmap;
        }

        public void SetBitmap(Bitmap bmp)
        {
            _cachedBitmap = null;
            if (bmp == null) { TemplatePng = null; return; }
            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                TemplatePng = ms.ToArray();
            }
        }
    }

    /// <summary>
    /// Sub-template gắn vào 1 main template. Q1: detect bằng Pattern2 trên cropped patch
    /// quanh main bbox. Q4: lazy-learn 1 lần, cache Pattern2 instance trong [NonSerialized]
    /// field, invalidate khi PNG bytes thay đổi.
    /// </summary>
    [Serializable]
    public class SubTemplateEntry
    {
        public string Label { get; set; } = "";
        public float ScoreThreshold { get; set; } = 70f;        // 0..100
        public bool HasAngleRange { get; set; } = false;
        public double AngleLower { get; set; } = -180.0;
        public double AngleUpper { get; set; } = 180.0;

        // PNG bytes (giống Pattern2TemplateEntry để JSON-serialize được).
        public byte[] TemplatePng { get; set; }

        [NonSerialized]
        private Bitmap _cachedBitmap;

        // Q4: lazy-learn cache. Key = handle Pattern2 đã LearnPatternStable từ PNG này.
        // Invalidate khi SetBitmap đổi PNG hoặc khi user chỉnh angle/score (chỉ cần re-match,
        // không cần re-learn — nên chỉ invalidate khi PNG đổi).
        [NonSerialized]
        private BeeCpp.Pattern2 _learned;
        [NonSerialized]
        private bool _learnedFlag;

        public Bitmap GetBitmap()
        {
            if (_cachedBitmap != null) return _cachedBitmap;
            if (TemplatePng == null || TemplatePng.Length == 0) return null;
            try
            {
                using (var ms = new System.IO.MemoryStream(TemplatePng))
                    _cachedBitmap = new Bitmap(ms);
            }
            catch { _cachedBitmap = null; }
            return _cachedBitmap;
        }

        public void SetBitmap(Bitmap bmp)
        {
            _cachedBitmap = null;
            _learned = null;
            _learnedFlag = false;
            if (bmp == null) { TemplatePng = null; return; }
            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                TemplatePng = ms.ToArray();
            }
        }

        /// <summary>
        /// Lấy Pattern2 đã learn (lazy). Trả null nếu PNG trống hoặc learn fail.
        /// </summary>
        internal BeeCpp.Pattern2 EnsureLearned()
        {
            if (_learnedFlag && _learned != null) return _learned;
            var bmp = GetBitmap();
            if (bmp == null) { _learnedFlag = true; _learned = null; return null; }

            Mat m = null;
            Mat gray = null;
            try
            {
                m = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp);
                if (m == null || m.Empty()) { _learnedFlag = true; _learned = null; return null; }
                if (m.Channels() != 1)
                {
                    gray = new Mat();
                    Cv2.CvtColor(m, gray, ColorConversionCodes.BGR2GRAY);
                }
                else gray = m;

                var p = new BeeCpp.Pattern2();
                p.SetImgeSampleNoCrop(gray.Data, gray.Width, gray.Height,
                    (int)gray.Step(), gray.Channels());
                p.LearnPatternStable();
                _learned = p;
                _learnedFlag = true;
                return _learned;
            }
            catch (Exception ex)
            {
                Global.LogsDashboard?.AddLog(
                    new LogEntry(DateTime.Now, LeveLLog.ERROR, "Pattern.SubTemplate.Learn",
                        $"Label='{Label}': {ex.Message}"));
                _learned = null;
                _learnedFlag = true;
                return null;
            }
            finally
            {
                if (gray != null && gray != m) gray.Dispose();
                m?.Dispose();
            }
        }

        /// <summary>Invalidate cached learn (gọi khi user edit Add/Delete/Reload).</summary>
        public void InvalidateLearned() { _learned = null; _learnedFlag = false; }
    }
}
