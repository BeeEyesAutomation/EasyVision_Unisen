using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeGlobal;

namespace BeeInterface
{
    [Serializable()]
    public class RJButton : Button
    {
        // Fields
        private int borderSize = 0;
        private int borderRadius = 0;
        private Corner _Corner = Corner.None;
        private Color borderColor = Color.PaleVioletRed;
        private TextImageRelation _textImageRelation;

        // State variables
        private bool isCLick = false;
        private bool isHovered = false;
        private bool isPressed = false;
        private bool isNotChange = false;
        private bool isUnGroup = false;
        private bool _IsRect = false;

        // Layout rectangles
        private Rectangle imgRect = Rectangle.Empty;
        private Rectangle textRect = Rectangle.Empty;
        private Rectangle rectSurface;
        private Rectangle rectBorder;
        private Rectangle rect;
        private GraphicsPath pathSurface;

        public new TextImageRelation TextImageRelation
        {
            get => base.TextImageRelation;
            set
            {
                if (_textImageRelation != value)
                {
                    _textImageRelation = value;
                    base.TextImageRelation = value;
                    UpdateTextImageLayout();
                    TextImageRelationChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler TextImageRelationChanged;

        [Category("RJ Code Advance")] public int BorderSize { get => borderSize; set { borderSize = value; this.Invalidate(); } }
        [Category("RJ Code Advance")] public int BorderRadius { get => borderRadius; set { borderRadius = value; UpdateRegion(); this.Invalidate(); } }
        [Category("RJ Code Advance")] public Color BorderColor { get => borderColor; set { borderColor = value; this.Invalidate(); } }
        [Category("RJ Code Advance")] public Color BackgroundColor { get => this.BackColor; set { this.BackColor = value; } }
        [Category("RJ Code Advance")] public Color TextColor { get => this.ForeColor; set { this.ForeColor = value; } }
        [Category("_Corner")] public Corner Corner { get => _Corner; set { _Corner = value; UpdateRegion(); this.Invalidate(); } }

        [Category("Bool Button Rect")] public bool IsRect { get => _IsRect; set { _IsRect = value; this.Invalidate(); } }
        [Category("Bool Button State")] public bool IsNotChange { get => isNotChange; set { isNotChange = value; this.Invalidate(); } }
        [Category("Bool Button State")] public bool IsUnGroup { get => isUnGroup; set { isUnGroup = value; this.Invalidate(); } }

        public bool IsCLick
        {
            get => isCLick;
            set
            {
                if (isNotChange) return;
                isCLick = value;
                if (isCLick && !isUnGroup && this.Parent != null)
                {
                    foreach (Control c in this.Parent.Controls)
                    {
                        if (c is RJButton btn && btn != this)
                            btn.IsCLick = false;
                    }
                }
                this.Invalidate();
            }
        }

        public RJButton()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackgroundColor = Color.Transparent;
            this.ForeColor = Color.White;

            _textImageRelation = base.TextImageRelation;
            this.Resize += Button_Resize;
            this.SizeChanged += RJButton_SizeChanged;
            this.MarginChanged += RJButton_MarginChanged;
            this.EnabledChanged += RJButton_EnabledChanged;
            this.MouseMove += RJButton_MouseMove;
            this.MouseLeave += RJButton_MouseLeave;
            this.Click += RJButton_Click;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _textImageRelation = base.TextImageRelation;
        }

        private void UpdateRegion()
        {
            rectSurface = this.ClientRectangle;
            rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            if (borderSize >= 1 && borderRadius > 0)
            {
                pathSurface = GetFigurePath(rectSurface, borderRadius);
                this.Region = new Region(pathSurface);
            }
            else
                this.Region = new Region(rectSurface);
        }
        private Size ScaleSize(Size orig, int maxW, int maxH)
        {
            if (orig.Width <= 0 || orig.Height <= 0) return Size.Empty;
            double ratio = Math.Min((double)maxW / orig.Width, (double)maxH / orig.Height);
            if (ratio > 1) ratio = 1;
            return new Size((int)(orig.Width * ratio), (int)(orig.Height * ratio));
        }
        private float FindMaxFontSize(Graphics g, string text, FontFamily family, FontStyle style, float maxWidth, float maxHeight, float maxSize = 100f)
        {
            float low = 1f, high = maxSize, bestFit = 1f;

            while (high - low > 0.5f)
            {
                float mid = (low + high) / 2;
                using (Font testFont = new Font(family, mid, style))
                {
                    SizeF size = g.MeasureString(text, testFont);
                    if (size.Width <= maxWidth && size.Height <= maxHeight)
                    {
                        bestFit = mid;
                        low = mid;
                    }
                    else
                    {
                        high = mid;
                    }
                }
            }

            return bestFit;
        }

        private void UpdateTextImageLayout()
        {
            var bounds = this.ClientRectangle;
            imgRect = Rectangle.Empty;
            textRect = bounds;
            int spacing = 5;

            if (Image != null && ! string.IsNullOrEmpty(this.Text))
            {
                var orig = Image.Size;
                Size scaled;

                switch (TextImageRelation)
                {
                    case TextImageRelation.ImageBeforeText:
                        // Ảnh bên trái, chiếm max nửa chiều rộng, full chiều cao
                        scaled = ScaleSize(orig, bounds.Width / 2 - spacing, bounds.Height - 2 * spacing);
                        imgRect = new Rectangle(
                            spacing,
                            (bounds.Height - scaled.Height) / 2,
                            scaled.Width, scaled.Height);
                        textRect = new Rectangle(
                            imgRect.Right + spacing,
                            0,
                            bounds.Width - imgRect.Right - spacing,
                            bounds.Height);
                        break;

                    case TextImageRelation.TextBeforeImage:
                        // Ảnh bên phải, chiếm max nửa chiều rộng
                        scaled = ScaleSize(orig, bounds.Width / 2 - spacing, bounds.Height - 2 * spacing);
                        imgRect = new Rectangle(
                            bounds.Width - scaled.Width - spacing,
                            (bounds.Height - scaled.Height) / 2,
                            scaled.Width, scaled.Height);
                        textRect = new Rectangle(
                            0,
                            0,
                            imgRect.Left - spacing,
                            bounds.Height);
                        break;

                    case TextImageRelation.ImageAboveText:
                        // Ảnh phía trên, chiếm max nửa chiều cao, full chiều rộng
                        scaled = ScaleSize(orig, bounds.Width - 2 * spacing, bounds.Height / 2 - spacing);
                        imgRect = new Rectangle(
                            (bounds.Width - scaled.Width) / 2,
                            spacing,
                            scaled.Width, scaled.Height);
                        // Chữ chỉ cao 1 dòng, nằm ngay dưới ảnh
                        int th = Font.Height + 2;
                        textRect = new Rectangle(
                            0,
                            imgRect.Bottom + spacing,
                            bounds.Width,
                            th);
                        break;

                    case TextImageRelation.TextAboveImage:
                        // Chữ phía trên, cao 1 dòng
                        int th2 = Font.Height + 2;
                        textRect = new Rectangle(
                            0,
                            spacing,
                            bounds.Width,
                            th2);
                        // Ảnh dưới chữ
                        scaled = ScaleSize(orig, bounds.Width - 2 * spacing, bounds.Height / 2 - spacing);
                        imgRect = new Rectangle(
                            (bounds.Width - scaled.Width) / 2,
                            textRect.Bottom + spacing,
                            scaled.Width, scaled.Height);
                        break;

                    case TextImageRelation.Overlay:
                        // Ảnh overlay chính giữa
                        scaled = ScaleSize(orig, bounds.Width - 2 * spacing, bounds.Height - 2 * spacing);
                        imgRect = new Rectangle(
                            (bounds.Width - scaled.Width) / 2,
                            (bounds.Height - scaled.Height) / 2,
                            scaled.Width, scaled.Height);
                        textRect = bounds;
                        break;

                    default:
                        // fallback: image không hiển thị
                        imgRect = Rectangle.Empty;
                        textRect = bounds;
                        break;
                }
            }
            else if (this.Image == null && !string.IsNullOrEmpty(this.Text))
            {
                float targetHeight = this.ClientRectangle.Height * 0.5f; // 60% chiều cao
                float targetWidth = this.ClientRectangle.Width * 0.8f;   // 90% chiều rộng

                using (Graphics g = this.CreateGraphics())
                {
                    float bestSize = FindMaxFontSize(g, this.Text, this.Font.FontFamily, this.Font.Style, targetWidth, targetHeight);
                    if (Math.Abs(this.Font.Size - bestSize) > 0.5f) // chỉ cập nhật nếu khác nhiều
                    {
                        this.Font = new Font(this.Font.FontFamily, bestSize, this.Font.Style);
                    }
                }
            }
            else if (this.Image != null && string.IsNullOrEmpty(this.Text))
            {
                // Không cần set textRect — ảnh sẽ là trung tâm
                imgRect = new Rectangle(
                    (bounds.Width - Image.Width) / 2,
                    (bounds.Height - Image.Height) / 2,
                    Image.Width, Image.Height);
                textRect = Rectangle.Empty;
            }
            this.Invalidate();
        }
        //private void UpdateTextImageLayout()
        //{
        //    var bounds = this.ClientRectangle;
        //    imgRect = Rectangle.Empty;
        //    textRect = bounds;
        //    if (Image != null)
        //    {
        //        int spacing = 5;
        //        var orig = Image.Size;
        //        Size scaled;
        //        switch (TextImageRelation)
        //        {
        //            case TextImageRelation.ImageBeforeText:
        //                scaled = ScaleSize(orig, bounds.Width / 2, bounds.Height - 2 * spacing);
        //                imgRect = new Rectangle(spacing, (bounds.Height - scaled.Height) / 2, scaled.Width, scaled.Height);
        //                textRect = new Rectangle(imgRect.Right + spacing, 0, bounds.Width - imgRect.Right - spacing, bounds.Height);
        //                break;
        //            case TextImageRelation.TextBeforeImage:
        //                scaled = ScaleSize(orig, bounds.Width / 2, bounds.Height - 2 * spacing);
        //                imgRect = new Rectangle(bounds.Right - scaled.Width - spacing, (bounds.Height - scaled.Height) / 2, scaled.Width, scaled.Height);
        //                textRect = new Rectangle(0, 0, imgRect.Left - spacing, bounds.Height);
        //                break;
        //            case TextImageRelation.ImageAboveText:
        //                scaled = ScaleSize(orig, bounds.Width - 2 * spacing, bounds.Height / 2);
        //                imgRect = new Rectangle((bounds.Width - scaled.Width) / 2, spacing, scaled.Width, scaled.Height);
        //                textRect = new Rectangle(0, imgRect.Bottom + spacing, bounds.Width, bounds.Height - imgRect.Bottom - spacing);
        //                break;
        //            case TextImageRelation.TextAboveImage:
        //                scaled = ScaleSize(orig, bounds.Width - 2 * spacing, bounds.Height / 2);
        //                textRect = new Rectangle(0, 0, bounds.Width, bounds.Height - scaled.Height - spacing);
        //                imgRect = new Rectangle((bounds.Width - scaled.Width) / 2, textRect.Bottom + spacing, scaled.Width, scaled.Height);
        //                break;
        //            case TextImageRelation.Overlay:
        //                scaled = ScaleSize(orig, bounds.Width, bounds.Height);
        //                imgRect = new Rectangle((bounds.Width - scaled.Width) / 2, (bounds.Height - scaled.Height) / 2, scaled.Width, scaled.Height);
        //                textRect = bounds;
        //                break;
        //            default:
        //                imgRect = Rectangle.Empty;
        //                textRect = bounds;
        //                break;
        //        }
        //    }
        //    this.Invalidate();
        //}

        private void Button_Resize(object sender, EventArgs e) => UpdateTextImageLayout();
        private void RJButton_SizeChanged(object sender, EventArgs e) { UpdateRegion(); this.Invalidate(); }
        private void RJButton_MarginChanged(object sender, EventArgs e) => this.Invalidate();
        private void RJButton_EnabledChanged(object sender, EventArgs e) => this.Invalidate();

        private void RJButton_Click(object sender, EventArgs e) { IsCLick = !IsCLick; }
        private void RJButton_MouseLeave(object sender, EventArgs e) { isPressed = false; isHovered = false; this.Invalidate(); }
        private void RJButton_MouseMove(object sender, MouseEventArgs e) { isHovered = true; isPressed = false; this.Invalidate(); }

        private GraphicsPath GetFigurePath(Rectangle rect, int curveSize)
        {
            GraphicsPath path = new GraphicsPath();
            switch (Corner)
            {
                case Corner.Both:
                    path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
                    path.AddLine(rect.X + curveSize, rect.Y, rect.Right - curveSize, rect.Y);
                    path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
                    path.AddLine(rect.Right, rect.Y + curveSize, rect.Right, rect.Bottom - curveSize);
                    path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
                    path.AddLine(rect.Right - curveSize, rect.Bottom, rect.X + curveSize, rect.Bottom);
                    path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
                    path.AddLine(rect.X, rect.Bottom - curveSize, rect.X, rect.Y + curveSize);
                    break;
                case Corner.Left:
                    path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
                    path.AddLine(rect.X + curveSize, rect.Y, rect.Right, rect.Y);
                    path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom);
                    path.AddLine(rect.Right, rect.Bottom, rect.X + curveSize, rect.Bottom);
                    path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
                    path.AddLine(rect.X, rect.Bottom - curveSize, rect.X, rect.Y + curveSize);
                    break;
                case Corner.Right:
                    path.AddLine(rect.X, rect.Y, rect.Right - curveSize, rect.Y);
                    path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
                    path.AddLine(rect.Right, rect.Y + curveSize, rect.Right, rect.Bottom - curveSize);
                    path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
                    path.AddLine(rect.Right - curveSize, rect.Bottom, rect.X, rect.Bottom);
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y);
                    break;
                default:
                    path.AddRectangle(rect); break;
            }
            path.CloseFigure(); return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            // Skip base.OnPaint to avoid default drawing of text/image
            // base.OnPaint(pevent);
            // Draw background
            OnPaintBackground(pevent);
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var bounds = ClientRectangle;

            // Gradient background
            Color top, mid, bot;
            if (!Enabled) top = mid = bot = BackColor;
            else if (isCLick) { top = Color.FromArgb(244, 192, 89); mid = Color.FromArgb(246, 204, 120); bot = Color.FromArgb(247, 211, 139); }
            else if (isHovered) { top = Color.FromArgb(208, 211, 213); mid = Color.FromArgb(193, 197, 199); bot = Color.FromArgb(179, 182, 185); }
            else { top = Color.FromArgb(245, 248, 251); mid = Color.FromArgb(218, 221, 224); bot = Color.FromArgb(199, 203, 206); }

            if (bounds.Width > 0 && bounds.Height > 0)
            {
                using (var brush = new LinearGradientBrush(bounds, top, bot, LinearGradientMode.Vertical))
                {
                    brush.InterpolationColors = new ColorBlend
                    {
                        Colors = new[] { top, mid, bot },
                        Positions = new[] { 0f, 0.5f, 1f }
                    };
                    g.FillRectangle(brush, bounds);
                }
            }

            // Draw image once
            if (Image != null && imgRect.Width > 0 && imgRect.Height > 0)
                g.DrawImage(Image, imgRect);

            // Draw text once
            var flags = TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.EndEllipsis;
            flags |= ConvertAlignment(TextAlign);
            if (textRect.Width > 0 && textRect.Height > 0)
                TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, flags);

            // Draw border
            if (borderSize > 0)
            {
                Color drawBorderColor = this.Parent?.BackColor ?? borderColor;
                using (var pen = new Pen(drawBorderColor, borderSize))
                
                    {
                    var borderRect = Rectangle.Inflate(bounds, -borderSize / 2, -borderSize / 2);
                    if (borderRect.Width > 0 && borderRect.Height > 0)
                    {
                        if (borderRadius > 0)
                            g.DrawPath(pen, GetFigurePath(borderRect, borderRadius));
                        else
                            g.DrawRectangle(pen, borderRect);
                    }
                }
            }
        }
        private static TextFormatFlags ConvertAlignment(ContentAlignment align)
        {
            var flags = TextFormatFlags.GlyphOverhangPadding;
            if (align.HasFlag(ContentAlignment.TopLeft)) flags |= TextFormatFlags.Top | TextFormatFlags.Left;
            if (align.HasFlag(ContentAlignment.TopCenter)) flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
            if (align.HasFlag(ContentAlignment.TopRight)) flags |= TextFormatFlags.Top | TextFormatFlags.Right;
            if (align.HasFlag(ContentAlignment.MiddleLeft)) flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
            if (align.HasFlag(ContentAlignment.MiddleCenter)) flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
            if (align.HasFlag(ContentAlignment.MiddleRight)) flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
            if (align.HasFlag(ContentAlignment.BottomLeft)) flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
            if (align.HasFlag(ContentAlignment.BottomCenter)) flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
            if (align.HasFlag(ContentAlignment.BottomRight)) flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
            return flags;
        }
    }
}
