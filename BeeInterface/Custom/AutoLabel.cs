using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BeeInterface
{
    public class AutoFontLabel : ScrollableControl
    {
        private bool _autoFont = true;
        private bool _adjusting;
        private Control _oldParent;

        private float _minFontSize = 6f;
        private float _maxFontSize = 200f;

        private bool _enableVScroll = false;
        private bool _enableHScroll = false;

        private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;
        private BorderStyle _borderStyle = BorderStyle.None;

        // ================= PROPERTIES =================
        [Category("Behavior")]
        public bool AutoFont
        {
            get => _autoFont;
            set {
                _autoFont = true; Invalidate(); AdjustFont(); }
        }

        [Category("Appearance")]
        public float MinFontSize
        {
            get => _minFontSize;
            set {
                _minFontSize = Math.Max(1, value);
                AdjustFont(); }
        }

        [Category("Appearance")]
        public float MaxFontSize
        {
            get => _maxFontSize;
            set { _maxFontSize = Math.Max(_minFontSize, value); AdjustFont(); }
        }

        [Category("Behavior")]
        public bool EnableVerticalScroll
        {
            get => _enableVScroll;
            set { _enableVScroll = value; UpdateScroll(); Invalidate(); }
        }

        [Category("Behavior")]
        public bool EnableHorizontalScroll
        {
            get => _enableHScroll;
            set { _enableHScroll = value; UpdateScroll(); Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
        public ContentAlignment TextAlign
        {
            get => _textAlign;
            set { _textAlign = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(BorderStyle.None)]
        public BorderStyle BorderStyle
        {
            get => _borderStyle;
            set { _borderStyle = value; Invalidate(); }
        }

        // ================= CONSTRUCTOR =================
        public AutoFontLabel()
        {
            DoubleBuffered = true;
            AutoScroll = false;

            SetStyle(ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        // ================= PARENT =================
        protected override void OnParentChanged(EventArgs e)
        {
            if (_oldParent != null)
                _oldParent.SizeChanged -= Parent_SizeChanged;

            base.OnParentChanged(e);

            if (Parent != null)
            {
                Parent.SizeChanged += Parent_SizeChanged;
                _oldParent = Parent;

                // giữ logic cũ: label cao bằng parent
                Height = Parent.ClientSize.Height;
            }

            AdjustFont();
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                Height = Parent.ClientSize.Height;
                AdjustFont();
            }
        }

        // ================= TEXT / SIZE =================
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            AdjustFont();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AdjustFont();
        }

        // ================= HELPERS =================
        private Padding GetContentPadding()
        {
            // chừa border giống cảm giác label
            if (_borderStyle == BorderStyle.FixedSingle) return new Padding(1);
            if (_borderStyle == BorderStyle.Fixed3D) return new Padding(2);
            return Padding.Empty;
        }

        private TextFormatFlags BuildFlags()
        {
            // WordBreak để giống code cũ
            TextFormatFlags f = TextFormatFlags.WordBreak;

            // Align ngang
            switch (_textAlign)
            {
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    f |= TextFormatFlags.HorizontalCenter; break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    f |= TextFormatFlags.Right; break;

                default:
                    f |= TextFormatFlags.Left; break;
            }

            // Align dọc
            switch (_textAlign)
            {
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    f |= TextFormatFlags.VerticalCenter; break;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    f |= TextFormatFlags.Bottom; break;

                default:
                    f |= TextFormatFlags.Top; break;
            }

            return f;
        }

        // ================= FONT AUTO =================
        private void AdjustFont()
        {
            if (!_autoFont || _adjusting) return;
            if (string.IsNullOrEmpty(Text)) return;
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

            _adjusting = true;

            try
            {
                var pad = GetContentPadding();
                var box = new Size(
                    Math.Max(1, ClientSize.Width - pad.Left - pad.Right),
                    Math.Max(1, ClientSize.Height - pad.Top - pad.Bottom)
                );

                using (Graphics g = CreateGraphics())
                {
                    float min = _minFontSize;
                    float max = _maxFontSize;
                    float best = min;

                    // đo theo vùng nội dung (đã trừ border)
                    var flags = TextFormatFlags.WordBreak;

                    while (max - min > 0.5f)
                    {
                        float mid = (min + max) / 2f;
                        using (Font f = new Font(Font.FontFamily, mid, Font.Style))
                        {
                            Size sz = TextRenderer.MeasureText(g, Text, f, box, flags);

                            if (sz.Width <= box.Width && sz.Height <= box.Height)
                            {
                                best = mid;
                                min = mid;
                            }
                            else
                            {
                                max = mid;
                            }
                        }
                    }

                    Font = new Font(Font.FontFamily, best, Font.Style);
                }
            }
            finally
            {
                _adjusting = false;
                UpdateScroll();
                Invalidate();
            }
        }

        // ================= SCROLL =================
        private void UpdateScroll()
        {
            if (!_enableHScroll && !_enableVScroll)
            {
                AutoScroll = false;
                return;
            }

            var pad = GetContentPadding();

            using (Graphics g = CreateGraphics())
            {
                // đo text “tự nhiên” (không giới hạn)
                Size textSize = TextRenderer.MeasureText(
                    g,
                    Text,
                    Font,
                    new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.WordBreak
                );

                int contentW = Math.Max(1, ClientSize.Width - pad.Left - pad.Right);
                int contentH = Math.Max(1, ClientSize.Height - pad.Top - pad.Bottom);

                int w = _enableHScroll ? Math.Max(textSize.Width, contentW) : contentW;
                int h = _enableVScroll ? Math.Max(textSize.Height, contentH) : contentH;

                AutoScroll = true;

                // AutoScrollMinSize là kích thước vùng nội dung cuộn
                AutoScrollMinSize = new Size(w + pad.Left + pad.Right, h + pad.Top + pad.Bottom);
            }
        }

        // ================= PAINT =================
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // vẽ nền (ScrollableControl không luôn tự clear như Label)
            using (var br = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(br, ClientRectangle);

            // vẽ border
            if (_borderStyle == BorderStyle.FixedSingle)
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, SystemColors.WindowFrame, ButtonBorderStyle.Solid);
            }
            else if (_borderStyle == BorderStyle.Fixed3D)
            {
                ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
            }

            var pad = GetContentPadding();

            // vùng nội dung
            Rectangle content = new Rectangle(
                pad.Left,
                pad.Top,
                Math.Max(1, ClientSize.Width - pad.Left - pad.Right),
                Math.Max(1, ClientSize.Height - pad.Top - pad.Bottom)
            );

            // áp scroll
            content.Offset(AutoScrollPosition.X, AutoScrollPosition.Y);

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                content,
                ForeColor,
                BuildFlags()
            );
        }
    }
}
