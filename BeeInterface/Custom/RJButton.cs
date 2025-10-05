using BeeGlobal; // Corner enum
using Newtonsoft.Json.Linq;
using System;
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
        private bool _isUnGroup = false;
        private bool _isRect = false;

        // Layout rects
        private Rectangle _imgRect = Rectangle.Empty;
        private Rectangle _textRect = Rectangle.Empty;

        // Region/path cache (FLOAT để viền nét)
        private GraphicsPath _pathSurface;
        private GraphicsPath _pathBorder;

        // Debounce layout
        private System.Windows.Forms.Timer _layoutTimer; // lazy-init
        private int _layoutDebounceMs = 16;
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
        // ===== Click Gradient Colors =====
        [Category("Click Gradient Colors")]
        public Color ClickTopColor { get; set; } = Color.FromArgb(244, 192, 89);

        [Category("Click Gradient Colors")]
        public Color ClickMidColor { get; set; } = Color.FromArgb(246, 204, 120);

        [Category("Click Gradient Colors")]
        public Color ClickBotColor { get; set; } = Color.FromArgb(247, 211, 139);
        // ===== Helpers =====
        private bool InDesignMode =>
            LicenseManager.UsageMode == LicenseUsageMode.Designtime || (Site?.DesignMode ?? false);

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
            get => base.TextImageRelation;
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
            get => borderSize;
            set { if (borderSize == value) return; borderSize = Math.Max(0, value); UpdateRegionPaths(); Invalidate(); }
        }

        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get => borderRadius;
            set { if (borderRadius == value) return; borderRadius = Math.Max(0, value); UpdateRegionPaths(); Invalidate(); }
        }

        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get => borderColor;
            set { //if (borderColor == value) return;
                // if (this.Parent == null) return;
                borderColor = value;//this.Parent.BackColor;
                Invalidate(); }
        }

        [Category("RJ Code Advance")]
        public Color BackgroundColor
        {
            get => BackColor;
            set
            {
                BackColor = value;  Invalidate(); }//if (BackColor == value) return; if (this.Parent == null) return;
               // BackColor = this.Parent.BackColor;
        }

        [Category("RJ Code Advance")]
        public Color TextColor
        {
            get => ForeColor;
            set { if (ForeColor == value) return; ForeColor = value; Invalidate(); }
        }

        [Category("_Corner")]
        public Corner Corner
        {
            get => _corner;
            set { if (_corner == value) return; _corner = value; UpdateRegionPaths(); Invalidate(); }
        }

        [Category("Bool Button Rect")]
        public bool IsRect
        {
            get => _isRect;
            set { if (_isRect == value) return; _isRect = value; Invalidate(); }
        }

        [Category("Bool Button State")]
        public bool IsNotChange
        {
            get => _isNotChange;
            set { _isNotChange = value; }
        }

        [Category("Bool Button State")]
        public bool IsUnGroup
        {
            get => _isUnGroup;
            set { _isUnGroup = value; }
        }

        [Category("Behavior")]
        public int DebounceResizeMs
        {
            get => _layoutDebounceMs;
            set
            {
                _layoutDebounceMs = Math.Max(0, value);
                if (!InDesignMode) { EnsureLayoutTimer(); _layoutTimer.Interval = _layoutDebounceMs; }
            }
        }

        public bool IsCLick
        {
            get => _isClick;
            set
            {
              //  if (!Global.Initialed) return;
                if (_isNotChange) return;
                if (_isClick == value) return;
                _isClick = value;

                if (_isClick && !_isUnGroup && Parent != null)
                {
                    foreach (Control c in Parent.Controls)
                        if (c is RJButton btn && !ReferenceEquals(btn, this))
                            btn.IsCLick = false;
                }
                Invalidate();
            }
        }

        // Wrap Image/ImageList/ImageIndex để trigger layout
        public new Image Image
        {
            get => base.Image;
            set
            {
                if (!ReferenceEquals(base.Image, value))
                {
                    base.Image = value;
                    RequestLayout();
                    Invalidate();
                }
            }
        }
        public new ImageList ImageList
        {
            get => base.ImageList;
            set
            {
                if (!ReferenceEquals(base.ImageList, value))
                {
                    base.ImageList = value;
                    RequestLayout();
                    Invalidate();
                }
            }
        }
        public new int ImageIndex
        {
            get => base.ImageIndex;
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
            //SetStyle(ControlStyles.AllPaintingInWmPaint |
            //         ControlStyles.UserPaint |
            //         ControlStyles.OptimizedDoubleBuffer |
            //         ControlStyles.ResizeRedraw, true);

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

            if (!InDesignMode) EnsureLayoutTimer();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                if (Parent != null && (BackColor.A < 255 || BackColor == Color.Transparent))
                    BackColor = Parent.BackColor;
                UpdateRegionPaths();
                RequestLayout();
                SmoothAncestors(); // auto bật double-buffer cho cha
            }
            catch { }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _layoutTimer?.Stop();
                _layoutTimer?.Dispose();
                _layoutTimer = null;

                _pathSurface?.Dispose();
                _pathBorder?.Dispose();
            }
            base.Dispose(disposing);
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

        // ===== Timer =====
        private void EnsureLayoutTimer()
        {
            if (_layoutTimer != null) return;
            _layoutTimer = new System.Windows.Forms.Timer { Interval = _layoutDebounceMs };
            _layoutTimer.Tick += (s, e) =>
            {
                _layoutTimer.Stop();
                _layoutPending = false;
                if (!IsDisposed) UpdateTextImageLayoutCore();
            };
        }

        // ===== Region/Path (float) =====
        private void UpdateRegionPaths()
        {
            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            _pathSurface?.Dispose();
            _pathBorder?.Dispose();

            // ⬇️ bán kính không vượt quá (cạnh ngắn - độ dày viền)/2
            float maxR = 0.5f * (Math.Min(rect.Width, rect.Height) - BorderSize);
            float radius = Math.Max(0f, Math.Min(BorderRadius, maxR));

            RectangleF rf = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            _pathSurface = BuildPath(rf, radius, _corner);
            Region = new Region(_pathSurface);

            // viền: inset nửa nét
            float inset = Math.Max(0.5f, BorderSize / 2f);
            RectangleF rb = RectangleF.Inflate(rf, -inset, -inset);
            float radiusBorder = Math.Max(0f, radius - inset);
            _pathBorder = BuildPath(rb, radiusBorder, _corner);
          
        }

        private static GraphicsPath BuildPath(RectangleF rect, float radius, Corner corner)
        {
            var path = new GraphicsPath();
            if (radius <= 0f || corner == Corner.None)
            {
                path.AddRectangle(rect);
                path.CloseFigure();
                return path;
            }

            float d = radius * 2f;

            switch (corner)
            {
                case Corner.Both:
                    path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                    path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
                    path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                    path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
                    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                    path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
                    path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                    path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
                    break;

                case Corner.Left:
                    path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                    path.AddLine(rect.X + radius, rect.Y, rect.Right, rect.Y);
                    path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom);
                    path.AddLine(rect.Right, rect.Bottom, rect.X + radius, rect.Bottom);
                    path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                    path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);
                    break;

                case Corner.Right:
                    path.AddLine(rect.X, rect.Y, rect.Right - radius, rect.Y);
                    path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                    path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
                    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                    path.AddLine(rect.Right - radius, rect.Bottom, rect.X, rect.Bottom);
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y);
                    break;
            }

            path.CloseFigure();
            return path;
        }

        // ===== Layout =====
        private void RequestLayout()
        {
            if (IsDisposed) return;

            if (InDesignMode)
            {
                UpdateTextImageLayoutCore();
                Invalidate();
                return;
            }

            EnsureLayoutTimer();

            if (!IsHandleCreated || !Visible)
            {
                _layoutPending = true;
                _layoutTimer.Stop();
                _layoutTimer.Start();
                return;
            }

            if (_layoutDebounceMs > 0)
            {
                _layoutPending = true;
                _layoutTimer.Stop();
                _layoutTimer.Interval = _layoutDebounceMs;
                _layoutTimer.Start();
            }
            else
            {
                UpdateTextImageLayoutCore();
            }
        }

        private void UpdateTextImageLayoutCore()
        {
            _layoutPending = false;

            var bounds = ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0) { Invalidate(); return; }

            var inner = Rectangle.Inflate(bounds,
                -ContentPadding.Horizontal / 2,
                -ContentPadding.Vertical / 2);

            var img = GetCurrentImage();
            _imgRect = Rectangle.Empty;
            _textRect = Rectangle.Empty;

            int gap = string.IsNullOrEmpty(Text) || img == null ? 0 : ImageTextSpacing;

            if (img != null && !string.IsNullOrEmpty(Text))
            {
                switch (TextImageRelation)
                {
                    case TextImageRelation.ImageAboveText:
                        {
                            var imgSize = FitRect(img.Size, inner).Size;
                            var txtSize = MeasureTextSize(Text, Font, inner.Width);

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
                            var imgSize = FitRect(img.Size, inner).Size;
                            var txtSize = MeasureTextSize(Text, Font, inner.Width);

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
                            var imgSize = FitRect(img.Size, inner).Size;
                            int txtMaxW = Math.Max(1, inner.Width - imgSize.Width - gap);
                            var txtSize = MeasureTextSize(Text, Font, txtMaxW);

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
                            var imgSize = FitRect(img.Size, inner).Size;
                            int txtMaxW = Math.Max(1, inner.Width - imgSize.Width - gap);
                            var txtSize = MeasureTextSize(Text, Font, txtMaxW);

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
            Parent?.Invalidate(Bounds, true);
        }

        // ===== đo chữ & auto-font =====
        private Size MeasureTextSize(string text, Font font, int maxWidth)
        {
            var flags = TextFormatFlags.NoPadding |
                        (Multiline ? (TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl)
                                   : TextFormatFlags.SingleLine);
            var s = TextRenderer.MeasureText(text, font, new Size(Math.Max(1, maxWidth), int.MaxValue), flags);
            if (!Multiline) s.Height = Math.Max(font.Height, s.Height);
            return s;
        }

        private void AutoFitFontTo(Rectangle area)
        {
            if (!AutoFont || string.IsNullOrEmpty(Text)) return;
            if (area.Width <= 0 || area.Height <= 0) return;

            int maxW = Math.Max(1, (int)(area.Width * AutoFontWidthRatio));
            int maxH = Math.Max(1, (int)(area.Height * AutoFontHeightRatio));

            var flags = TextFormatFlags.NoPadding |
                        (Multiline ? (TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl)
                                   : TextFormatFlags.SingleLine);

            float best = FindMaxFontSizeGDI(Text, this.Font, maxW, maxH, AutoFontMin, AutoFontMax, flags);
            if (Math.Abs(this.Font.Size - best) > 0.5f)
                this.Font = new Font(this.Font.FontFamily, best, this.Font.Style);
        }

        private static float FindMaxFontSizeGDI(string text, Font baseFont, int maxW, int maxH,
                                                float min, float max, TextFormatFlags flags)
        {
            if (string.IsNullOrEmpty(text)) return baseFont.Size;
            float lo = Math.Max(1f, min), hi = Math.Max(lo, max), best = lo;
            var proposed = new Size(Math.Max(1, maxW), int.MaxValue);

            while (hi - lo > 0.5f)
            {
                float mid = (lo + hi) / 2f;
                using (var f = new Font(baseFont.FontFamily, mid, baseFont.Style))
                {
                    Size sz = TextRenderer.MeasureText(text, f, proposed, flags);
                    if (sz.Width <= maxW && sz.Height <= maxH) { best = mid; lo = mid; }
                    else hi = mid;
                }
            }
            return best;
        }

        // ===== Auto Image helpers =====
        private Image GetCurrentImage()
        {
            if (!Enabled) return ImageDisabled ?? base.Image ?? ImageNormal;
            if (_isPressed && MouseButtons == MouseButtons.Left) return ImagePressed ?? ImageHover ?? ImageNormal ?? base.Image;
            if (_isHovered) return ImageHover ?? ImageNormal ?? base.Image;
            return ImageNormal ?? base.Image;
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

            float r = tint.R / 255f, g = tint.G / 255f, b = tint.B / 255f;
            float a = Math.Max(0f, Math.Min(1f, opacity));

            var cm = new ColorMatrix(new float[][]
            {
                new float[] {0, 0, 0, 0, r},
                new float[] {0, 0, 0, 0, g},
                new float[] {0, 0, 0, 0, b},
                new float[] {0, 0, 0, a, 0},
                new float[] {0, 0, 0, 0, 1}
            });

            var ia = new ImageAttributes();
            ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            ia.SetWrapMode(WrapMode.TileFlipXY);
            return ia;
        }

        private Rectangle FitRect(Size src, Rectangle area)
        {
            if (!AutoImage || src.Width <= 0 || src.Height <= 0 || area.Width <= 0 || area.Height <= 0)
                return Rectangle.Empty;

            var a = Rectangle.Inflate(area, -ImagePadding.Horizontal / 2, -ImagePadding.Vertical / 2);
            int maxW = (int)Math.Round(a.Width * AutoImageMaxRatio);
            int maxH = (int)Math.Round(a.Height * AutoImageMaxRatio);
            if (maxW <= 0 || maxH <= 0) return Rectangle.Empty;

            Size target;
            double rw = (double)maxW / src.Width;
            double rh = (double)maxH / src.Height;

            switch (AutoImageMode)
            {
                case ImageFitMode.Contain:
                    {
                        double r = Math.Min(rw, rh);
                        target = new Size(Math.Max(1, (int)Math.Round(src.Width * r)),
                                          Math.Max(1, (int)Math.Round(src.Height * r)));
                        break;
                    }
                case ImageFitMode.Cover:
                    {
                        double r = Math.Max(rw, rh);
                        target = new Size(Math.Max(1, (int)Math.Round(src.Width * r)),
                                          Math.Max(1, (int)Math.Round(src.Height * r)));
                        if (target.Width > maxW) target.Width = maxW;
                        if (target.Height > maxH) target.Height = maxH;
                        break;
                    }
                case ImageFitMode.Fill:
                    target = new Size(maxW, maxH); break;
                case ImageFitMode.FitWidth:
                    target = new Size(maxW, Math.Max(1, (int)Math.Round(src.Height * rw))); break;
                case ImageFitMode.FitHeight:
                    target = new Size(Math.Max(1, (int)Math.Round(src.Width * rh)), maxH); break;
                default:
                    return Rectangle.Empty;
            }

            int x = a.X + (a.Width - target.Width) / 2;
            int y = a.Y + (a.Height - target.Height) / 2;
            return new Rectangle(x, y, target.Width, target.Height);
        }

        // ===== Painting =====
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (var b = new SolidBrush(this.BackColor))
                e.Graphics.FillRectangle(b, this.ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var bounds = ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            // Gradient theo state
            Color top, mid, bot;
            if (!Enabled) top = mid = bot = BackColor;
            else if (_isClick)
            {
                top = ClickTopColor;
                mid = ClickMidColor;
                bot = ClickBotColor;
            }
            else if (_isHovered) { top = Color.FromArgb(208, 211, 213); mid = Color.FromArgb(193, 197, 199); bot = Color.FromArgb(179, 182, 185); }
            else { top = Color.FromArgb(245, 248, 251); mid = Color.FromArgb(218, 221, 224); bot = Color.FromArgb(199, 203, 206); }

            using (var brush = new LinearGradientBrush(bounds, top, bot, LinearGradientMode.Vertical))
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
            var img = GetCurrentImage();
            if (img != null && _imgRect.Width > 0 && _imgRect.Height > 0)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var tintColor = GetCurrentTintColor();
                using (var ia = GetTintAttributes(tintColor, ImageTintOpacity))
                {
                    if (ia == null)
                        g.DrawImage(img, _imgRect);
                    else
                        g.DrawImage(img, _imgRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                }
            }

            // Text
            if (_textRect.Width > 0 && _textRect.Height > 0 && !string.IsNullOrEmpty(Text))
            {
                var baseFlags = TextFormatFlags.NoPadding |
                                (Multiline ? (TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl)
                                           : TextFormatFlags.SingleLine) |
                                TextFormatFlags.EndEllipsis;

                var alignFlags = MapTextAlign(TextAlign);
                Rectangle drawRect = _textRect;

                if (Multiline)
                {
                    Size measured = TextRenderer.MeasureText(Text, Font, new Size(_textRect.Width, int.MaxValue),
                                                             TextFormatFlags.NoPadding | TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
                    int h = Math.Min(measured.Height, _textRect.Height);
                    if (IsMiddle(TextAlign)) drawRect = new Rectangle(_textRect.X, _textRect.Y + (_textRect.Height - h) / 2, _textRect.Width, h);
                    else if (IsBottom(TextAlign)) drawRect = new Rectangle(_textRect.X, _textRect.Bottom - h, _textRect.Width, h);
                    else drawRect = new Rectangle(_textRect.X, _textRect.Y, _textRect.Width, h);
                }

                TextRenderer.DrawText(g, Text, Font, drawRect, ForeColor, baseFlags | alignFlags);
            }
            if (borderSize > 0 && _pathBorder != null)
            {
                g.PixelOffsetMode = PixelOffsetMode.Half;           // ⬅ nét 1px sắc
                using (var pen = new Pen(borderColor, borderSize))
                {
                    pen.Alignment = PenAlignment.Inset;
                    pen.LineJoin = LineJoin.Round;                 // ⬅ bo chỗ nối path
                    g.DrawPath(pen, _pathBorder);
                }
            }
            //// Border
            //if (borderSize > 0 && _pathBorder != null)
            //{
            //    using (var pen = new Pen(borderColor, borderSize))
            //    {
            //        pen.Alignment = PenAlignment.Inset;
            //        g.DrawPath(pen, _pathBorder);
            //    }
            //}
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
            Parent?.Invalidate(Bounds, true);
            Parent?.PerformLayout();
        }
        //protected override void OnVisibleChanged(EventArgs e)
        //{
        //    base.OnVisibleChanged(e);
        //    if (Visible)
        //    {
        //        RequestLayout();
        //        Invalidate();
        //        Parent?.Invalidate(Bounds, true);
        //        Parent?.PerformLayout();
        //    }
        //}
        protected override void OnMarginChanged(EventArgs e)
        {
            base.OnMarginChanged(e);
            Invalidate();
        }
        protected override void OnClick(EventArgs e)
        {
          //  if (!Global.Initialed) return;
            if (IsUnGroup) IsCLick = !IsCLick; else IsCLick = true;
            base.OnClick(e);
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            _isPressed = true; Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _isPressed = false; Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isPressed = false; _isHovered = false; Invalidate();
        }
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            _isHovered = true; Invalidate();
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
                default: return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
            }
        }
        private static bool IsMiddle(ContentAlignment a) =>
            a == ContentAlignment.MiddleLeft || a == ContentAlignment.MiddleCenter || a == ContentAlignment.MiddleRight;
        private static bool IsBottom(ContentAlignment a) =>
            a == ContentAlignment.BottomLeft || a == ContentAlignment.BottomCenter || a == ContentAlignment.BottomRight;

        // ==== Auto enable double-buffer cho CHA (Panel/TLP/...) ====
        private static void TryEnableDoubleBuffer(Control c)
        {
            try
            {
                var t = c.GetType();
                t.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                 ?.SetValue(c, true, null);
                t.GetProperty("ResizeRedraw", BindingFlags.Instance | BindingFlags.NonPublic)
                 ?.SetValue(c, true, null);

                if (c.BackColor == Color.Transparent && c.Parent != null)
                    c.BackColor = c.Parent.BackColor;
            }
            catch { /* ignore */ }
        }
        private void SmoothAncestors()
        {
            for (Control p = this.Parent; p != null; p = p.Parent)
                TryEnableDoubleBuffer(p);
        }
    }
}
