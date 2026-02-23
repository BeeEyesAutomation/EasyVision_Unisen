using BeeGlobal; // Corner enum
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

namespace BeeInterface
{
    [Serializable]
    public class RJButton : Button
    {
        // ===== Fields =====
        private int borderSize = 1;
        private int borderRadius = 14;
        private Corner _corner = Corner.Both;
        private Color borderColor = Color.Gray;
        private TextImageRelation _textImageRelation;

        // State
        private bool _isClick = false;
        private bool _isHovered = false;
        private bool _isPressed = false;
        private bool _isNotChange = false;
        private bool _isUnGroup = true;
        private bool _isRect = false;
        private bool _isTouch = false;

        // Layout rects
        private Rectangle _imgRect = Rectangle.Empty;
        private Rectangle _textRect = Rectangle.Empty;

        // Region/path cache
        private GraphicsPath _pathSurface;
        private GraphicsPath _pathBorder;

        // ===== Global layout queue (batch theo Application.Idle) =====
        private static readonly object _layoutQueueLock = new object();
        private static readonly HashSet<RJButton> _layoutQueue = new HashSet<RJButton>();
        private static bool _layoutIdleAttached;
        private bool _layoutPending;

        // ===== repaint debounce =====
        private bool _invalidateQueued = false;

        // ===== debounce resize =====
        private Timer _resizeDebounceTimer;
        private bool _resizePending;

        // ===== Auto Font =====
        [Category("Behavior")] public bool AutoFont { get; set; } = true;
        [Category("Behavior")] public float AutoFontMin { get; set; } = 6f;
        [Category("Behavior")] public float AutoFontMax { get; set; } = 100f;
        [Category("Behavior")] public float AutoFontWidthRatio { get; set; } = 0.92f;
        [Category("Behavior")] public float AutoFontHeightRatio { get; set; } = 0.75f;
        [Category("Behavior")] public bool Multiline { get; set; } = false;

        private bool _autoFitInProgress = false;

        // ===== Auto Image & Tint =====
        public enum ImageFitMode { None, Contain, Cover, Fill, FitWidth, FitHeight }

        [Category("Behavior")] public bool AutoImage { get; set; } = true;
        [Category("Behavior")] public ImageFitMode AutoImageMode { get; set; } = ImageFitMode.Contain;
        [Category("Behavior")] public float AutoImageMaxRatio { get; set; } = 0.75f;
        [Category("Behavior")] public Padding ImagePadding { get; set; } = new Padding(1);

        [Category("Appearance")] public Image ImageNormal { get; set; }
        [Category("Appearance")] public Image ImageHover { get; set; }
        [Category("Appearance")] public Image ImagePressed { get; set; }
        [Category("Appearance")] public Image ImageDisabled { get; set; }

        [Category("Behavior")] public bool AutoImageTint { get; set; } = true;
        [Category("Behavior")] public float ImageTintOpacity { get; set; } = 0.5f; // 0..1
        [Category("Appearance")] public Color ImageTintNormal { get; set; } = Color.Empty;
        [Category("Appearance")] public Color ImageTintHover { get; set; } = Color.Empty;
        [Category("Appearance")] public Color ImageTintPressed { get; set; } = Color.Empty;
        [Category("Appearance")] public Color ImageTintDisabled { get; set; } = Color.FromArgb(160, 160, 160);

        // ===== Căn giữa icon + text như một khối =====
        [Category("Behavior")] public Padding ContentPadding { get; set; } = new Padding(8, 6, 8, 6);
        [Category("Behavior")] public int ImageTextSpacing { get; set; } = 6;
        [Category("DebounceResizeMs")] public int DebounceResizeMs { get; set; } = 6;

        // ===== Click Gradient Colors =====
        [Category("Click Gradient Colors")]
        public Color ClickTopColor { get; set; } = Color.FromArgb(244, 192, 89);
        [Category("Click Gradient Colors")]
        public Color ClickMidColor { get; set; } = Color.FromArgb(246, 204, 120);
        [Category("Click Gradient Colors")]
        public Color ClickBotColor { get; set; } = Color.FromArgb(247, 211, 139);

        // ===== Others =====
        private bool InDesignMode
        {
            get
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
                if (Site != null && Site.DesignMode) return true;
                return false;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.Style |= 0x04000000; // WS_CLIPSIBLINGS
                cp.Style |= 0x02000000; // WS_CLIPCHILDREN
                return cp;
            }
        }

        // =========================================================
        // SafeInvalidate: gộp nhiều Invalidate vào 1 lần UI tick
        // =========================================================
        private void SafeInvalidate()
        {
            if (IsDisposed) return;

            if (!IsHandleCreated)
            {
                // Handle chưa có → cứ gọi Invalidate thường (Designer/Init)
                Invalidate();
                return;
            }

            if (_invalidateQueued) return;
            _invalidateQueued = true;

            BeginInvoke((Action)(() =>
            {
                _invalidateQueued = false;
                if (!IsDisposed) Invalidate();
            }));
        }

        // ===== Public API =====

        public new TextImageRelation TextImageRelation
        {
            get { return base.TextImageRelation; }
            set
            {
                if (_textImageRelation == value) return;
                _textImageRelation = value;
                base.TextImageRelation = value;
                RequestLayout();
                TextImageRelationChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TextImageRelationChanged;

        [Category("RJ Code Advance")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                int v = Math.Max(0, value);
                if (borderSize == v) return;
                borderSize = v;
                UpdateRegionPaths();
                SafeInvalidate();
            }
        }

        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                int v = Math.Max(0, value);
                if (borderRadius == v) return;
                borderRadius = v;
                UpdateRegionPaths();
                SafeInvalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor == value) return;
                borderColor = value;
                SafeInvalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color BackgroundColor
        {
            get { return BackColor; }
            set
            {
                if (BackColor == value) return;
                BackColor = value;
                SafeInvalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color TextColor
        {
            get { return ForeColor; }
            set
            {
                if (ForeColor == value) return;
                ForeColor = value;
                SafeInvalidate();
            }
        }

        [Category("_Corner")]
        public Corner Corner
        {
            get { return _corner; }
            set
            {
                if (_corner == value) return;
                _corner = value;
                UpdateRegionPaths();
                SafeInvalidate();
            }
        }

        [Category("Bool Button Rect")]
        public bool IsRect
        {
            get { return _isRect; }
            set
            {
                if (_isRect == value) return;
                _isRect = value;
                UpdateRegionPaths();
                SafeInvalidate();
            }
        }

        [Category("Bool Button State")]
        public bool IsNotChange
        {
            get { return _isNotChange; }
            set { _isNotChange = value; }
        }

        [Category("Bool Button State")]
        public bool IsUnGroup
        {
            get { return _isUnGroup; }
            set { _isUnGroup = value; }
        }

        public bool IsCLick
        {
            get { return _isClick; }
            set
            {
                if (_isNotChange) return;
                if (_isClick == value) return;
                _isClick = value;

                if (_isClick && !_isUnGroup && Parent != null)
                {
                    foreach (Control c in Parent.Controls)
                    {
                        if (c is RJButton btn && !ReferenceEquals(btn, this))
                            btn.IsCLick = false;
                    }
                }
                SafeInvalidate();
            }
        }

        [Category("IsTouch")]
        public bool IsTouch
        {
            get { return _isTouch; }
            set
            {
                if (_isTouch == value) return;
                _isTouch = value;
                SafeInvalidate();
            }
        }

        // Wrap Image/ImageList/ImageIndex để trigger layout
        public new Image Image
        {
            get { return base.Image; }
            set
            {
                if (!ReferenceEquals(base.Image, value))
                {
                    base.Image = value;
                    RequestLayout();
                    SafeInvalidate();
                }
            }
        }

        public new ImageList ImageList
        {
            get { return base.ImageList; }
            set
            {
                if (!ReferenceEquals(base.ImageList, value))
                {
                    base.ImageList = value;
                    RequestLayout();
                    SafeInvalidate();
                }
            }
        }

        public new int ImageIndex
        {
            get { return base.ImageIndex; }
            set
            {
                if (base.ImageIndex != value)
                {
                    base.ImageIndex = value;
                    RequestLayout();
                    SafeInvalidate();
                }
            }
        }

        // ===== ctor =====
        public RJButton()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            DoubleBuffered = true;
            UpdateStyles();
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(180, 60);

            BackColor = SystemColors.Control;
            borderColor = SystemColors.Control;
            ForeColor = Color.Black;

            _textImageRelation = base.TextImageRelation;
        }

        // ===== Handle / Parent =====
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                if (Parent != null && (BackColor.A < 255 || BackColor == Color.Transparent))
                    BackColor = Parent.BackColor;

                UpdateRegionPaths();

                if (_layoutPending)
                {
                    _layoutPending = false;
                    RequestLayout();
                }
                else
                {
                    RequestLayout();
                }

                SmoothAncestors();
            }
            catch { }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _textImageRelation = base.TextImageRelation;
            UpdateRegionPaths();
            RequestLayout();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            SmoothAncestors();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible) SmoothAncestors();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pathSurface?.Dispose();
                _pathBorder?.Dispose();
                _cachedPath?.Dispose();

                if (_resizeDebounceTimer != null)
                {
                    _resizeDebounceTimer.Stop();
                    _resizeDebounceTimer.Tick -= ResizeDebounceTimer_Tick;
                    _resizeDebounceTimer.Dispose();
                    _resizeDebounceTimer = null;
                }

                lock (_layoutQueueLock)
                {
                    _layoutQueue.Remove(this);
                }
            }
            base.Dispose(disposing);
        }

        // == Region & Border Path ======
        private void UpdateRegionPaths()
        {
            _pathSurface?.Dispose();
            _pathBorder?.Dispose();
            _pathSurface = null;
            _pathBorder = null;

            Rectangle rect = ClientRectangle;
            if (rect.Width <= 1 || rect.Height <= 1)
            {
                Region = null;
                return;
            }

            float offset = 0.5f;
            RectangleF rf = new RectangleF(rect.X + offset, rect.Y + offset, rect.Width - 1f, rect.Height - 1f);

            if (_isRect || borderRadius <= 0 || _corner == Corner.None)
            {
                _pathSurface = new GraphicsPath();
                _pathSurface.AddRectangle(rf);

                _pathBorder = new GraphicsPath();
                _pathBorder.AddRectangle(rf);

                Region = new Region(_pathSurface);
                return;
            }

            float rad = borderRadius;
            _pathSurface = CreateRoundedPath(rf, rad, _corner);
            _pathBorder = CreateRoundedPath(rf, rad, _corner);

            Region = new Region(_pathSurface);
        }

        private GraphicsPath CreateRoundedPath(RectangleF rect, float radius, Corner corners)
        {
            GraphicsPath path = new GraphicsPath();

            float d = radius * 2f;
            bool tl = false, tr = false, bl = false, br = false;

            switch (corners)
            {
                case Corner.Both: tl = tr = bl = br = true; break;
                case Corner.Left: tl = bl = true; break;
                case Corner.Right: tr = br = true; break;
                case Corner.Top: tl = tr = true; break;
                case Corner.Bottom: bl = br = true; break;
                default:
                    path.AddRectangle(rect);
                    path.CloseFigure();
                    return path;
            }

            if (tl) path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            else path.AddLine(rect.X, rect.Y, rect.X + radius, rect.Y);

            if (tr) path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            else path.AddLine(rect.Right - radius, rect.Y, rect.Right, rect.Y);

            if (br) path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            else path.AddLine(rect.Right, rect.Bottom - radius, rect.Right, rect.Bottom);

            if (bl) path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            else path.AddLine(rect.X, rect.Bottom, rect.X, rect.Bottom - radius);

            path.CloseFigure();
            return path;
        }

        // ===== Layout scheduling: global queue + Application.Idle =====
        private void RequestLayout()
        {
            if (IsDisposed) return;

            if (InDesignMode)
            {
                UpdateTextImageLayoutCore();
                Invalidate();
                return;
            }

            if (!IsHandleCreated)
            {
                _layoutPending = true;
                return;
            }

            lock (_layoutQueueLock)
            {
                _layoutQueue.Add(this);

                if (!_layoutIdleAttached)
                {
                    _layoutIdleAttached = true;
                    Application.Idle += OnGlobalLayoutIdle;
                }
            }
        }

        private static void OnGlobalLayoutIdle(object sender, EventArgs e)
        {
            RJButton[] buttons;

            lock (_layoutQueueLock)
            {
                if (_layoutQueue.Count == 0)
                {
                    Application.Idle -= OnGlobalLayoutIdle;
                    _layoutIdleAttached = false;
                    return;
                }

                buttons = new RJButton[_layoutQueue.Count];
                _layoutQueue.CopyTo(buttons);
                _layoutQueue.Clear();
            }

            foreach (RJButton btn in buttons)
            {
                if (btn == null || btn.IsDisposed) continue;
                if (!btn.IsHandleCreated)
                {
                    btn._layoutPending = true;
                    continue;
                }
                btn.UpdateTextImageLayoutCore();
            }
        }

        // ===== Text+Image layout core =====
        private void UpdateTextImageLayoutCore()
        {
            _layoutPending = false;

            Rectangle bounds = ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                SafeInvalidate();
                return;
            }

            Rectangle inner = Rectangle.Inflate(bounds,
                -ContentPadding.Horizontal / 2,
                -ContentPadding.Vertical / 2);

            Image img = GetCurrentImage();
            _imgRect = Rectangle.Empty;
            _textRect = Rectangle.Empty;

            int gap = (string.IsNullOrEmpty(Text) || img == null) ? 0 : ImageTextSpacing;

            if (img != null && !string.IsNullOrEmpty(Text))
            {
                switch (TextImageRelation)
                {
                    case TextImageRelation.ImageAboveText:
                        {
                            Size imgSize = FitRect(img.Size, inner).Size;
                            Size txtSize = MeasureTextSize(Text, Font, inner.Width);

                            int totalH = imgSize.Height + gap + txtSize.Height;
                            int startY = inner.Y + Math.Max(0, (inner.Height - totalH) / 2);

                            _imgRect = new Rectangle(
                                inner.X + (inner.Width - imgSize.Width) / 2,
                                startY, imgSize.Width, imgSize.Height);

                            _textRect = new Rectangle(
                                inner.X,
                                _imgRect.Bottom + gap,
                                inner.Width, txtSize.Height);

                            AutoFitFontTo(_textRect);
                            break;
                        }

                    case TextImageRelation.TextAboveImage:
                        {
                            Size imgSize = FitRect(img.Size, inner).Size;
                            Size txtSize = MeasureTextSize(Text, Font, inner.Width);

                            int totalH = txtSize.Height + gap + imgSize.Height;
                            int startY = inner.Y + Math.Max(0, (inner.Height - totalH) / 2);

                            _textRect = new Rectangle(inner.X, startY, inner.Width, txtSize.Height);

                            _imgRect = new Rectangle(
                                inner.X + (inner.Width - imgSize.Width) / 2,
                                _textRect.Bottom + gap, imgSize.Width, imgSize.Height);

                            AutoFitFontTo(_textRect);
                            break;
                        }

                    case TextImageRelation.ImageBeforeText:
                        {
                            Size imgSize = FitRect(img.Size, inner).Size;
                            int txtMaxW = Math.Max(1, inner.Width - imgSize.Width - gap);
                            Size txtSize = MeasureTextSize(Text, Font, txtMaxW);

                            int totalW = imgSize.Width + gap + txtSize.Width;
                            int startX = inner.X + Math.Max(0, (inner.Width - totalW) / 2);
                            int centerY = inner.Y + inner.Height / 2;

                            _imgRect = new Rectangle(startX, centerY - imgSize.Height / 2, imgSize.Width, imgSize.Height);
                            _textRect = new Rectangle(_imgRect.Right + gap, centerY - txtSize.Height / 2, txtSize.Width, txtSize.Height);

                            AutoFitFontTo(_textRect);
                            break;
                        }

                    case TextImageRelation.TextBeforeImage:
                        {
                            Size imgSize = FitRect(img.Size, inner).Size;
                            int txtMaxW = Math.Max(1, inner.Width - imgSize.Width - gap);
                            Size txtSize = MeasureTextSize(Text, Font, txtMaxW);

                            int totalW = txtSize.Width + gap + imgSize.Width;
                            int startX = inner.X + Math.Max(0, (inner.Width - totalW) / 2);
                            int centerY = inner.Y + inner.Height / 2;

                            _textRect = new Rectangle(startX, centerY - txtSize.Height / 2, txtSize.Width, txtSize.Height);
                            _imgRect = new Rectangle(_textRect.Right + gap, centerY - imgSize.Height / 2, imgSize.Width, imgSize.Height);

                            AutoFitFontTo(_textRect);
                            break;
                        }

                    case TextImageRelation.Overlay:
                    default:
                        {
                            _imgRect = FitRect(img.Size, inner);
                            _textRect = inner;
                            AutoFitFontTo(_textRect);
                            break;
                        }
                }
            }
            else if (img != null)
            {
                _imgRect = FitRect(img.Size, inner);
                _textRect = Rectangle.Empty;
            }
            else
            {
                _imgRect = Rectangle.Empty;
                _textRect = inner;
                AutoFitFontTo(_textRect);
            }

            // ✅ TỐI ƯU: chỉ invalidate chính control, KHÔNG invalidate parent
            SafeInvalidate();
        }

        // ===== đo chữ & auto-font =====
        private Size MeasureTextSize(string text, Font font, int maxWidth)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding;
            if (Multiline)
                flags |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
            else
                flags |= TextFormatFlags.SingleLine;

            Size s = TextRenderer.MeasureText(text, font, new Size(Math.Max(1, maxWidth), int.MaxValue), flags);
            if (!Multiline) s.Height = Math.Max(font.Height, s.Height);
            return s;
        }

        private void AutoFitFontTo(Rectangle area)
        {
            if (!AutoFont || string.IsNullOrEmpty(Text)) return;
            if (area.Width <= 0 || area.Height <= 0) return;
            if (_autoFitInProgress) return;

            int maxW = Math.Max(1, (int)(area.Width * AutoFontWidthRatio));
            int maxH = Math.Max(1, (int)(area.Height * AutoFontHeightRatio));

            TextFormatFlags flags = TextFormatFlags.NoPadding;
            if (Multiline)
                flags |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
            else
                flags |= TextFormatFlags.SingleLine;

            float best = FindMaxFontSizeGDI(Text, Font, maxW, maxH, AutoFontMin, AutoFontMax, flags);
            if (Math.Abs(Font.Size - best) > 0.5f)
            {
                try
                {
                    _autoFitInProgress = true;
                    Font = new Font(Font.FontFamily, best, Font.Style);
                }
                finally
                {
                    _autoFitInProgress = false;
                }
            }
        }

        private static float FindMaxFontSizeGDI(string text, Font baseFont, int maxW, int maxH,
                                                float min, float max, TextFormatFlags flags)
        {
            if (string.IsNullOrEmpty(text)) return baseFont.Size;
            float lo = Math.Max(1f, min);
            float hi = Math.Max(lo, max);
            float best = lo;
            Size proposed = new Size(Math.Max(1, maxW), int.MaxValue);

            while (hi - lo > 0.5f)
            {
                float mid = (lo + hi) / 2f;
                using (Font f = new Font(baseFont.FontFamily, mid, baseFont.Style))
                {
                    Size sz = TextRenderer.MeasureText(text, f, proposed, flags);
                    if (sz.Width <= maxW && sz.Height <= maxH)
                    {
                        best = mid;
                        lo = mid;
                    }
                    else
                    {
                        hi = mid;
                    }
                }
            }
            return best;
        }

        // ===== Auto Image helpers =====
        private Image GetCurrentImage()
        {
            if (!Enabled) return (ImageDisabled != null) ? ImageDisabled : (base.Image ?? ImageNormal);
            if (_isPressed && MouseButtons == MouseButtons.Left)
            {
                if (ImagePressed != null) return ImagePressed;
                if (ImageHover != null) return ImageHover;
                if (ImageNormal != null) return ImageNormal;
                return base.Image;
            }
            if (_isHovered)
            {
                if (ImageHover != null) return ImageHover;
                if (ImageNormal != null) return ImageNormal;
                return base.Image;
            }
            if (ImageNormal != null) return ImageNormal;
            return base.Image;
        }

        private Color GetCurrentTintColor()
        {
            if (!Enabled) return ImageTintDisabled;
            if (_isPressed && MouseButtons == MouseButtons.Left) return ImageTintPressed;
            if (_isHovered) return ImageTintHover;
            return ImageTintNormal;
        }

        private ImageAttributes GetTintAttributes(Color tint, float opacity)
        {
            if (!AutoImageTint || tint.IsEmpty) return null;

            float r = tint.R / 255f;
            float g = tint.G / 255f;
            float b = tint.B / 255f;
            float a = opacity;
            if (a < 0f) a = 0f;
            if (a > 1f) a = 1f;

            ColorMatrix cm = new ColorMatrix(new float[][]
            {
                new float[] {0, 0, 0, 0, r},
                new float[] {0, 0, 0, 0, g},
                new float[] {0, 0, 0, 0, b},
                new float[] {0, 0, 0, a, 0},
                new float[] {0, 0, 0, 0, 1}
            });

            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            ia.SetWrapMode(WrapMode.TileFlipXY);
            return ia;
        }

        private Rectangle FitRect(Size src, Rectangle area)
        {
            if (!AutoImage || src.Width <= 0 || src.Height <= 0 || area.Width <= 0 || area.Height <= 0)
                return Rectangle.Empty;

            Rectangle a = Rectangle.Inflate(area, -ImagePadding.Horizontal / 2, -ImagePadding.Vertical / 2);
            int maxW = (int)Math.Round(a.Width * AutoImageMaxRatio);
            int maxH = (int)Math.Round(a.Height * AutoImageMaxRatio);
            if (maxW <= 0 || maxH <= 0) return Rectangle.Empty;

            Size target;
            double rw = (double)maxW / (double)src.Width;
            double rh = (double)maxH / (double)src.Height;

            switch (AutoImageMode)
            {
                case ImageFitMode.Contain:
                    {
                        double r = rw < rh ? rw : rh;
                        target = new Size(
                            Math.Max(1, (int)Math.Round(src.Width * r)),
                            Math.Max(1, (int)Math.Round(src.Height * r)));
                        break;
                    }
                case ImageFitMode.Cover:
                    {
                        double r = rw > rh ? rw : rh;
                        target = new Size(
                            Math.Max(1, (int)Math.Round(src.Width * r)),
                            Math.Max(1, (int)Math.Round(src.Height * r)));
                        if (target.Width > maxW) target.Width = maxW;
                        if (target.Height > maxH) target.Height = maxH;
                        break;
                    }
                case ImageFitMode.Fill:
                    target = new Size(maxW, maxH);
                    break;
                case ImageFitMode.FitWidth:
                    target = new Size(maxW, Math.Max(1, (int)Math.Round(src.Height * rw)));
                    break;
                case ImageFitMode.FitHeight:
                    target = new Size(Math.Max(1, (int)Math.Round(src.Width * rh)), maxH);
                    break;
                default:
                    return Rectangle.Empty;
            }

            int x = a.X + (a.Width - target.Width) / 2;
            int y = a.Y + (a.Height - target.Height) / 2;
            return new Rectangle(x, y, target.Width, target.Height);
        }

        // ===== cached path (giữ nguyên như bạn) =====
        private GraphicsPath _cachedPath;
        private Rectangle _cachedRect;
        private int _cachedRadius;

        private GraphicsPath GetRoundPath(Rectangle r, int radius)
        {
            if (_cachedPath != null && _cachedRect == r && _cachedRadius == radius)
                return _cachedPath;

            _cachedPath?.Dispose();
            _cachedRect = r;
            _cachedRadius = radius;

            var path = new GraphicsPath();
            int d = radius * 2;
            if (radius <= 0) path.AddRectangle(r);
            else
            {
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
            }

            _cachedPath = path;
            return _cachedPath;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            _cachedPath?.Dispose();
            _cachedPath = null;
        }

        // ===== Painting =====
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // để trống giảm flicker
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            Color top, mid, bot;

            if (!Enabled)
            {
                top = mid = bot = BackColor;
            }
            else if (_isClick)
            {
                top = ClickTopColor;
                mid = ClickMidColor;
                bot = ClickBotColor;
            }
            else if (_isHovered || _isTouch)
            {
                top = Color.FromArgb(208, 211, 213);
                mid = Color.FromArgb(193, 197, 199);
                bot = Color.FromArgb(179, 182, 185);
            }
            else
            {
                top = Color.FromArgb(245, 248, 251);
                mid = Color.FromArgb(218, 221, 224);
                bot = Color.FromArgb(199, 203, 206);
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(bounds, top, bot, LinearGradientMode.Vertical))
            {
                var cb = new ColorBlend
                {
                    Colors = new[] { top, mid, bot },
                    Positions = new[] { 0f, 0.5f, 1f }
                };
                brush.InterpolationColors = cb;

                if (_pathSurface != null) g.FillPath(brush, _pathSurface);
                else g.FillRectangle(brush, bounds);
            }

            // Image
            Image img = GetCurrentImage();
            if (img != null && _imgRect.Width > 0 && _imgRect.Height > 0)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Color tintColor = GetCurrentTintColor();
                using (ImageAttributes ia = GetTintAttributes(tintColor, ImageTintOpacity))
                {
                    if (ia == null) g.DrawImage(img, _imgRect);
                    else
                    {
                        g.DrawImage(img, _imgRect,
                            0, 0, img.Width, img.Height,
                            GraphicsUnit.Pixel, ia);
                    }
                }
            }

            // Text
            if (_textRect.Width > 0 && _textRect.Height > 0 && !string.IsNullOrEmpty(Text))
            {
                TextFormatFlags baseFlags = TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis;
                if (Multiline) baseFlags |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
                else baseFlags |= TextFormatFlags.SingleLine;

                TextFormatFlags alignFlags = MapTextAlign(TextAlign);
                Rectangle drawRect = _textRect;

                if (Multiline)
                {
                    Size measured = TextRenderer.MeasureText(
                        Text, Font,
                        new Size(_textRect.Width, int.MaxValue),
                        TextFormatFlags.NoPadding | TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);

                    int h = measured.Height;
                    if (h > _textRect.Height) h = _textRect.Height;

                    if (IsMiddle(TextAlign))
                        drawRect = new Rectangle(_textRect.X, _textRect.Y + (_textRect.Height - h) / 2, _textRect.Width, h);
                    else if (IsBottom(TextAlign))
                        drawRect = new Rectangle(_textRect.X, _textRect.Bottom - h, _textRect.Width, h);
                    else
                        drawRect = new Rectangle(_textRect.X, _textRect.Y, _textRect.Width, h);
                }

                TextRenderer.DrawText(g, Text, Font, drawRect, ForeColor, baseFlags | alignFlags);
            }

            // Border
            if (borderSize > 0 && _pathBorder != null)
            {
                using (Pen pen = new Pen(borderColor, borderSize))
                {
                    pen.Alignment = PenAlignment.Center;
                    pen.LineJoin = LineJoin.Round;

                    g.PixelOffsetMode = PixelOffsetMode.None;
                    g.DrawPath(pen, _pathBorder);
                }
            }
        }

        // ===== Overrides =====
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // ✅ Debounce resize để tránh bão UpdateRegionPaths/RequestLayout
           // if (InDesignMode)
            {
                UpdateRegionPaths();
                RequestLayout();
                return;
            }

            StartResizeDebounce();
        }

        private void StartResizeDebounce()
        {
            _resizePending = true;

            int ms = DebounceResizeMs;
            if (ms < 0) ms = 0;

            if (_resizeDebounceTimer == null)
            {
                _resizeDebounceTimer = new Timer();
                _resizeDebounceTimer.Tick += ResizeDebounceTimer_Tick;
            }

            _resizeDebounceTimer.Stop();
            _resizeDebounceTimer.Interval = Math.Max(1, ms);
            _resizeDebounceTimer.Start();
        }

        private void ResizeDebounceTimer_Tick(object sender, EventArgs e)
        {
            _resizeDebounceTimer.Stop();
            if (!_resizePending) return;
            _resizePending = false;

            UpdateRegionPaths();
            RequestLayout();
            SafeInvalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            // ✅ bỏ Parent.Invalidate + Parent.PerformLayout (thủ phạm treo/giật)
            RequestLayout();
            SafeInvalidate();
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            base.OnMarginChanged(e);
            SafeInvalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            if (IsUnGroup) IsCLick = !IsCLick;
            else IsCLick = true;

            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (_isPressed) return;
            _isPressed = true;
            SafeInvalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (!_isPressed) return;
            _isPressed = false;
            SafeInvalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            bool changed = false;

            if (_isPressed) { _isPressed = false; changed = true; }
            if (_isHovered) { _isHovered = false; changed = true; }

            if (changed) SafeInvalidate();
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);

            // ✅ chỉ đổi state lần đầu, tránh spam Invalidate mỗi pixel
            if (_isHovered) return;
            _isHovered = true;
            SafeInvalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            RequestLayout();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            // Nếu AutoFit đang set Font thì bỏ qua để tránh vòng lặp
            if (_autoFitInProgress) return;

            RequestLayout();
        }

        // ===== Utils =====
        private static TextFormatFlags MapTextAlign(ContentAlignment a)
        {
            switch (a)
            {
                case ContentAlignment.TopLeft: return TextFormatFlags.Top | TextFormatFlags.Left;
                case ContentAlignment.TopCenter: return TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.TopRight: return TextFormatFlags.Top | TextFormatFlags.Right;

                case ContentAlignment.MiddleLeft: return TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                case ContentAlignment.MiddleCenter: return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.MiddleRight: return TextFormatFlags.VerticalCenter | TextFormatFlags.Right;

                case ContentAlignment.BottomLeft: return TextFormatFlags.Bottom | TextFormatFlags.Left;
                case ContentAlignment.BottomCenter: return TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.BottomRight: return TextFormatFlags.Bottom | TextFormatFlags.Right;
            }
            return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
        }

        private static bool IsMiddle(ContentAlignment a)
        {
            return a == ContentAlignment.MiddleLeft ||
                   a == ContentAlignment.MiddleCenter ||
                   a == ContentAlignment.MiddleRight;
        }

        private static bool IsBottom(ContentAlignment a)
        {
            return a == ContentAlignment.BottomLeft ||
                   a == ContentAlignment.BottomCenter ||
                   a == ContentAlignment.BottomRight;
        }

        private static void TryEnableDoubleBuffer(Control c)
        {
            try
            {
                Type t = c.GetType();
                PropertyInfo pDb = t.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (pDb != null) pDb.SetValue(c, true, null);

                PropertyInfo pRr = t.GetProperty("ResizeRedraw", BindingFlags.Instance | BindingFlags.NonPublic);
                if (pRr != null) pRr.SetValue(c, true, null);

                if (c.BackColor == Color.Transparent && c.Parent != null)
                    c.BackColor = c.Parent.BackColor;
            }
            catch { }
        }

        private void SmoothAncestors()
        {
            Control p = Parent;
            while (p != null)
            {
                TryEnableDoubleBuffer(p);
                p = p.Parent;
            }
        }
    }
}
