using BeeGlobal; // Corner enum
using Newtonsoft.Json.Linq;
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

        // ===== Auto Font =====
        [Category("Behavior")] public bool AutoFont { get; set; } = true;
        [Category("Behavior")] public float AutoFontMin { get; set; } = 6f;
        [Category("Behavior")] public float AutoFontMax { get; set; } = 100f;
        [Category("Behavior")] public float AutoFontWidthRatio { get; set; } = 0.92f;
        [Category("Behavior")] public float AutoFontHeightRatio { get; set; } = 0.75f;
        [Category("Behavior")] public bool Multiline { get; set; } = false;

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
                if (TextImageRelationChanged != null)
                    TextImageRelationChanged(this, EventArgs.Empty);
            }
        }
        public event EventHandler TextImageRelationChanged;

        [Category("RJ Code Advance")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                if (borderSize == value) return;
                borderSize = Math.Max(0, value);
                UpdateRegionPaths();
                Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                if (borderRadius == value) return;
                borderRadius = Math.Max(0, value);
                UpdateRegionPaths();
                Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color BackgroundColor
        {
            get { return BackColor; }
            set
            {
                BackColor = value;
                Invalidate();
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
                Invalidate();
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
                Invalidate();
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
                Invalidate();
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
                        RJButton btn = c as RJButton;
                        if (btn != null && !object.ReferenceEquals(btn, this))
                            btn.IsCLick = false;
                    }
                }
                Invalidate();
            }
        }

        [Category("IsTouch")]
        public bool IsTouch
        {
            get { return _isTouch; }
            set { _isTouch = value; Invalidate(); }
        }

        // Wrap Image/ImageList/ImageIndex để trigger layout
        public new Image Image
        {
            get { return base.Image; }
            set
            {
                if (!object.ReferenceEquals(base.Image, value))
                {
                    base.Image = value;
                    RequestLayout();
                    Invalidate();
                }
            }
        }

        public new ImageList ImageList
        {
            get { return base.ImageList; }
            set
            {
                if (!object.ReferenceEquals(base.ImageList, value))
                {
                    base.ImageList = value;
                    RequestLayout();
                    Invalidate();
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
                    Invalidate();
                }
            }
        }

        // ===== ctor =====
        public RJButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(180, 60);

            if (this.Parent != null)
            {
                borderColor = this.Parent.BackColor;
                BackColor = this.Parent.BackColor;
            }
            else
            {
                BackColor = SystemColors.Control;
                borderColor = SystemColors.Control;
            }

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
            catch
            {
            }
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
                if (_pathSurface != null) _pathSurface.Dispose();
                if (_pathBorder != null) _pathBorder.Dispose();

                // Bỏ khỏi queue nếu còn
                lock (_layoutQueueLock)
                {
                    _layoutQueue.Remove(this);
                }
            }
            base.Dispose(disposing);
        }

        // ======================= Region & Border Path ===========================
        private void UpdateRegionPaths()
        {
            if (_pathSurface != null) { _pathSurface.Dispose(); _pathSurface = null; }
            if (_pathBorder != null) { _pathBorder.Dispose(); _pathBorder = null; }

            Rectangle rect = this.ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                this.Region = null;
                return;
            }

            // ⬅  THÊM: _corner == Corner.None coi như góc vuông
            if (_isRect || borderRadius <= 0 || _corner == Corner.None)
            {
                _pathSurface = new GraphicsPath();
                _pathSurface.AddRectangle(rect);

                Rectangle b = Rectangle.Inflate(rect, -1, -1);
                _pathBorder = new GraphicsPath();
                _pathBorder.AddRectangle(b);

                this.Region = new Region(_pathSurface);
                return;
            }

            _pathSurface = CreateRoundedPath(rect, borderRadius, _corner);

            Rectangle innerRect = Rectangle.Inflate(rect, -borderSize, -borderSize);
            int innerRadius = borderRadius - borderSize;
            if (innerRadius < 1) innerRadius = 1;
            _pathBorder = CreateRoundedPath(innerRect, innerRadius, _corner);

            this.Region = new Region(_pathSurface);
        }
    

        private GraphicsPath CreateRoundedPath(Rectangle rect, int radius, Corner corners)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            bool tl = false;
            bool tr = false;
            bool bl = false;
            bool br = false;

            // Map theo enum Corner của bạn
            switch (corners)
            {
                case Corner.Both:
                    tl = tr = bl = br = true;
                    break;

                case Corner.Left:
                    tl = bl = true;
                    break;

                case Corner.Right:
                    tr = br = true;
                    break;

                case Corner.Top:
                    tl = tr = true;
                    break;

                case Corner.Bottom:
                    bl = br = true;
                    break;

                case Corner.None:
                default:
                    // Không bo góc, chỉ vẽ rectangle
                    path.AddRectangle(rect);
                    path.CloseFigure();
                    return path;
            }

            // Top-left
            if (tl)
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            else
                path.AddLine(rect.X, rect.Y, rect.X + radius, rect.Y);

            // Top-right
            if (tr)
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            else
                path.AddLine(rect.Right - radius, rect.Y, rect.Right, rect.Y);

            // Bottom-right
            if (br)
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            else
                path.AddLine(rect.Right, rect.Bottom - radius, rect.Right, rect.Bottom);

            // Bottom-left
            if (bl)
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            else
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Bottom - radius);

            path.CloseFigure();
            return path;
        }

        // ===== Layout scheduling: global queue + Application.Idle =====
        private void RequestLayout()
        {
            if (IsDisposed) return;

            // Design mode: layout trực tiếp
            if (InDesignMode)
            {
                UpdateTextImageLayoutCore();
                Invalidate();
                return;
            }

            // Handle chưa tạo → đánh dấu chờ
            if (!IsHandleCreated)
            {
                _layoutPending = true;
                return;
            }

            // Thêm vào queue
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

            // Đang ở UI thread
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
                Invalidate();
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
            else if (img != null)  // chỉ ảnh
            {
                _imgRect = FitRect(img.Size, inner);
                _textRect = Rectangle.Empty;
            }
            else                   // chỉ chữ
            {
                _imgRect = Rectangle.Empty;
                _textRect = inner;
                AutoFitFontTo(_textRect);
            }

            Invalidate();
            if (Parent != null)
                Parent.Invalidate(Bounds, true);
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
            if (!Multiline)
                s.Height = Math.Max(font.Height, s.Height);
            return s;
        }

        private void AutoFitFontTo(Rectangle area)
        {
            if (!AutoFont || string.IsNullOrEmpty(Text)) return;
            if (area.Width <= 0 || area.Height <= 0) return;

            int maxW = Math.Max(1, (int)(area.Width * AutoFontWidthRatio));
            int maxH = Math.Max(1, (int)(area.Height * AutoFontHeightRatio));

            TextFormatFlags flags = TextFormatFlags.NoPadding;
            if (Multiline)
                flags |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
            else
                flags |= TextFormatFlags.SingleLine;

            float best = FindMaxFontSizeGDI(Text, this.Font, maxW, maxH, AutoFontMin, AutoFontMax, flags);
            if (Math.Abs(this.Font.Size - best) > 0.5f)
                this.Font = new Font(this.Font.FontFamily, best, this.Font.Style);
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
                    {
                        target = new Size(maxW, maxH);
                        break;
                    }
                case ImageFitMode.FitWidth:
                    {
                        target = new Size(maxW, Math.Max(1, (int)Math.Round(src.Height * rw)));
                        break;
                    }
                case ImageFitMode.FitHeight:
                    {
                        target = new Size(Math.Max(1, (int)Math.Round(src.Width * rh)), maxH);
                        break;
                    }
                default:
                    return Rectangle.Empty;
            }

            int x = a.X + (a.Width - target.Width) / 2;
            int y = a.Y + (a.Height - target.Height) / 2;
            return new Rectangle(x, y, target.Width, target.Height);
        }

        // ===== Painting =====
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            using (SolidBrush b = new SolidBrush(this.BackColor))
            {
                pevent.Graphics.FillRectangle(b, this.ClientRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            // Gradient theo state
            Color top;
            Color mid;
            Color bot;

            if (!Enabled)
            {
                top = BackColor;
                mid = BackColor;
                bot = BackColor;
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
                ColorBlend cb = new ColorBlend();
                cb.Colors = new Color[] { top, mid, bot };
                cb.Positions = new float[] { 0f, 0.5f, 1f };
                brush.InterpolationColors = cb;

                if (_pathSurface != null)
                    g.FillPath(brush, _pathSurface);
                else
                    g.FillRectangle(brush, bounds);
            }

            // Image
            Image img = GetCurrentImage();
            if (img != null && _imgRect.Width > 0 && _imgRect.Height > 0)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Color tintColor = GetCurrentTintColor();
                using (ImageAttributes ia = GetTintAttributes(tintColor, ImageTintOpacity))
                {
                    if (ia == null)
                    {
                        g.DrawImage(img, _imgRect);
                    }
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
                if (Multiline)
                    baseFlags |= TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
                else
                    baseFlags |= TextFormatFlags.SingleLine;

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
                g.PixelOffsetMode = PixelOffsetMode.Half;
                using (Pen pen = new Pen(borderColor, borderSize))
                {
                    pen.Alignment = PenAlignment.Inset;
                    pen.LineJoin = LineJoin.Round;
                    g.DrawPath(pen, _pathBorder);
                }
            }
        }

        // ===== Overrides =====
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegionPaths();
            RequestLayout();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            RequestLayout();
            Invalidate();
            if (Parent != null)
            {
                Parent.Invalidate(Bounds, true);
                Parent.PerformLayout();
            }
        }

        protected override void OnMarginChanged(EventArgs e)
        {
            base.OnMarginChanged(e);
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            // if (!Global.Initialed) return;
            if (IsUnGroup)
                IsCLick = !IsCLick;
            else
                IsCLick = true;

            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            _isPressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isPressed = false;
            _isHovered = false;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            RequestLayout();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
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
                PropertyInfo pDb = t.GetProperty("DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                if (pDb != null) pDb.SetValue(c, true, null);

                PropertyInfo pRr = t.GetProperty("ResizeRedraw",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                if (pRr != null) pRr.SetValue(c, true, null);

                if (c.BackColor == Color.Transparent && c.Parent != null)
                    c.BackColor = c.Parent.BackColor;
            }
            catch
            {
            }
        }

        private void SmoothAncestors()
        {
            Control p = this.Parent;
            while (p != null)
            {
                TryEnableDoubleBuffer(p);
                p = p.Parent;
            }
        }
    }
}
