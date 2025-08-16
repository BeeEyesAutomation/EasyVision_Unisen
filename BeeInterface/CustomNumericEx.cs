using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace BeeInterface
{
    [DefaultEvent(nameof(ValueChanged))]
    public class CustomNumericEx : UserControl
    {
        // ====== Value ======
        private float _min = 0f, _max = 100f, _step = 1f, _value = 0f;
        private int _decimals = 0;
        private string _unitText = "";
        private bool _snapToStep = true;

        [Category("Behavior")] public float Min { get { return _min; } set { _min = value; if (_max <= _min) _max = _min + 1; Value = Clamp(Value); } }
        [Category("Behavior")] public float Max { get { return _max; } set { _max = value; if (_max <= _min) _min = _max - 1; Value = Clamp(Value); } }
        [Category("Behavior")] public float Step { get { return _step; } set { _step = Math.Max(0.000001f, value); if (_snapToStep) Value = Snap(Value); } }
        [Category("Behavior")] public int Decimals { get { return _decimals; } set { _decimals = Math.Max(0, value); UpdateText(); } }
        [Category("Behavior")] public string UnitText { get { return _unitText; } set { _unitText = value ?? ""; UpdateText(); } }
        [Category("Behavior")] public bool SnapToStep { get { return _snapToStep; } set { _snapToStep = value; if (value) Value = Snap(Value); } }
        [Category("Behavior")] public float WheelStep { get; set; } = 1f;
        [Category("Behavior")] public float KeyboardStep { get; set; } = 1f;

        [Browsable(false)]
        public float Value
        {
            get { return _value; }
            set
            {
                float v = Clamp(value);
                if (_snapToStep) v = Snap(v);
                if (Math.Abs(v - _value) < 1e-6f) return;
                _value = v;
                UpdateText();
                if (_isInit) LayoutChildren();
                if (ValueChanged != null) ValueChanged(_value);
                Invalidate();
            }
        }
        public event Action<float> ValueChanged;

        // ====== Appearance / Layout ======
        [Category("Appearance")] public Padding InnerPadding { get; set; } = new Padding(6, 6, 6, 6);
        [Category("Layout")] public int MinTextboxWidth { get; set; } = 16;
        [Category("Layout")] public int MaxTextboxWidth { get; set; } = 0; // 0 = unlimited
        [Category("Layout")] public int TextboxSidePadding { get; set; } = 12;
        [Category("Layout")] public bool AutoSizeTextbox { get; set; } = true;
        [Category("Layout")] public int TextboxWidth { get; set; } = 56;  // when AutoSizeTextbox=false

        // Font textbox độc lập
        private float _textboxFontSize = 11f;
        [Category("Appearance")]
        public float TextboxFontSize
        {
            get { return _textboxFontSize; }
            set
            {
                float v = Math.Max(6f, value);
                if (Math.Abs(v - _textboxFontSize) < 0.1f) return;
                _textboxFontSize = v;
                ApplyTextboxFont();
                if (_isInit) { LayoutChildren(); Invalidate(); }
            }
        }

        // Auto show/hide chỉ cho textbox (không dùng timer)
        [Category("Behavior")] public bool AutoShowTextbox { get; set; } = true;
        [Category("Behavior")] public bool StartWithTextboxHidden { get; set; } = false;

        private bool _textboxVisible = true;
        private bool _suppressLeaveOnce = false; // nuốt MouseLeave do layout dịch

        // ====== Colors (nếu muốn vẽ border nền control) ======
        [Category("Appearance")] public Color BorderColor { get; set; } = Color.FromArgb(210, 210, 210);
        [Category("Appearance")] public int BorderRadius { get; set; } = 6;

        // ====== Inner controls ======
        private TextBox _tb;
        private PlusMinusButton _btnMinus, _btnPlus;

        // ====== Init guard ======
        private bool _isInit;

        public CustomNumericEx()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);

            _tb = new TextBox
            {
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center,
                Multiline = false,
                TabStop = true
            };
            _tb.KeyDown += Tb_KeyDown;
            _tb.Leave += Tb_Leave;
            _tb.TextChanged += delegate { if (_isInit) { LayoutChildren(); Invalidate(); } };

            // Hover/Enter: hiện textbox
            _tb.MouseEnter += (s, e) => { if (AutoShowTextbox && !InDesigner()) { _suppressLeaveOnce = true; ShowTextbox(); } };
            _tb.MouseLeave += (s, e) => { if (_suppressLeaveOnce) { _suppressLeaveOnce = false; return; } TryHideTextboxOnLeave(); };
            _tb.Enter += (s, e) => { if (AutoShowTextbox && !InDesigner()) { _suppressLeaveOnce = true; ShowTextbox(); } };
            Controls.Add(_tb);

            _btnMinus = new PlusMinusButton(-1);
            _btnPlus = new PlusMinusButton(+1);

            // Hover vào nút: hiện textbox; rời: thử ẩn
            _btnMinus.MouseEnter += (s, e) => { if (AutoShowTextbox && !InDesigner()) { _suppressLeaveOnce = true; ShowTextbox(); } };
            _btnMinus.MouseLeave += (s, e) => { if (_suppressLeaveOnce) { _suppressLeaveOnce = false; return; } TryHideTextboxOnLeave(); };
            _btnPlus.MouseEnter += (s, e) => { if (AutoShowTextbox && !InDesigner()) { _suppressLeaveOnce = true; ShowTextbox(); } };
            _btnPlus.MouseLeave += (s, e) => { if (_suppressLeaveOnce) { _suppressLeaveOnce = false; return; } TryHideTextboxOnLeave(); };

            _btnMinus.Click += (s, e) => { Value = _value - _step; if (AutoShowTextbox && !InDesigner()) { _suppressLeaveOnce = true; ShowTextbox(); } Focus(); };
            _btnPlus.Click += (s, e) => { Value = _value + _step; if (AutoShowTextbox && !InDesigner()) { _suppressLeaveOnce = true; ShowTextbox(); } Focus(); };

            Controls.Add(_btnMinus);
            Controls.Add(_btnPlus);

            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10f);
            Height = 36;

            MouseWheel += CustomNumericEx_MouseWheel;
            KeyDown += CustomNumericEx_KeyDown;
            MouseLeave += (s, e) => { TryHideTextboxOnLeave(); };

            _textboxVisible = !StartWithTextboxHidden || InDesigner();
            _tb.Visible = _textboxVisible;

            ApplyTextboxFont();
            UpdateText();

            _isInit = true;
            LayoutChildren();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (InDesigner())
            {
                AutoShowTextbox = false;        // design-time: luôn hiện
                _textboxVisible = true;
                _tb.Visible = true;
                _btnMinus.Visible = true;
                _btnPlus.Visible = true;
                MinimumSize = new Size(120, 32);
                LayoutChildren();
                Invalidate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!_isInit) return;
            LayoutChildren();
            Invalidate();
        }

        // ====== Behavior ======
        private void CustomNumericEx_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!ClientRectangle.Contains(PointToClient(Cursor.Position))) return;
            Value = e.Delta > 0 ? _value + WheelStep : _value - WheelStep;
        }

        private void CustomNumericEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Down) { Value = _value - KeyboardStep; e.Handled = true; }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Up) { Value = _value + KeyboardStep; e.Handled = true; }
            if (e.KeyCode == Keys.PageDown) { Value = _value - 10 * KeyboardStep; e.Handled = true; }
            if (e.KeyCode == Keys.PageUp) { Value = _value + 10 * KeyboardStep; e.Handled = true; }
            if (e.KeyCode == Keys.Home) { Value = Min; e.Handled = true; }
            if (e.KeyCode == Keys.End) { Value = Max; e.Handled = true; }
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ParseTextbox(); e.Handled = true; e.SuppressKeyPress = true; Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                UpdateText(); e.Handled = true; e.SuppressKeyPress = true; Focus();
            }
        }
        private void Tb_Leave(object sender, EventArgs e) { ParseTextbox(); TryHideTextboxOnLeave(); }

        private void ParseTextbox()
        {
            string s = _tb.Text != null ? _tb.Text.Trim() : "0";
            if (!string.IsNullOrEmpty(_unitText) &&
                s.EndsWith(_unitText, StringComparison.OrdinalIgnoreCase))
            {
                s = s.Substring(0, s.Length - _unitText.Length).Trim(); // C# 7.3-safe
            }

            float v;
            if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ||
                float.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out v))
            {
                Value = v;
            }
            else
            {
                UpdateText();
            }
        }

        private string FormatValue(float v)
        {
            if (_decimals > 0)
                return Math.Round(v, _decimals).ToString("F" + _decimals, CultureInfo.InvariantCulture) + _unitText;
            return Math.Round(v, 0).ToString("F0", CultureInfo.InvariantCulture) + _unitText;
        }
        private void UpdateText()
        {
            if (_tb == null) return;
            _tb.Text = FormatValue(_value);
        }

        private float Clamp(float v) { return Math.Max(_min, Math.Min(_max, v)); }
        private float Snap(float v)
        {
            if (_step <= 0) return v;
            float n = (float)Math.Round((v - _min) / _step, MidpointRounding.AwayFromZero);
            float s = _min + n * _step;
            return Clamp(s);
        }

        // ====== Layout ======
        private void LayoutChildren()
        {
            if (!_isInit || _tb == null || _btnMinus == null || _btnPlus == null) return;

            int top = InnerPadding.Top;
            int bottom = Height - InnerPadding.Bottom;
            int h = Math.Max(16, bottom - top);

            // Nút vuông theo chiều cao, 24..36
            int btnH = h;
            int btnW = Math.Min(36, Math.Max(24, btnH));

            // đảm bảo font textbox trước khi đo
            ApplyTextboxFont();

            // đo text nếu đang hiện
            int textW = 0;
            if (_textboxVisible)
            {
                using (Graphics g = CreateGraphics())
                {
                    string t = _tb.Text ?? "0";
                    Size sz = TextRenderer.MeasureText(
                        g, t, _tb.Font,
                        new Size(int.MaxValue, int.MaxValue),
                        TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                    textW = sz.Width;
                }
            }

            int tbW = 0;
            if (_textboxVisible)
            {
                if (AutoSizeTextbox)
                {
                    tbW = textW + TextboxSidePadding;
                    if (MaxTextboxWidth > 0) tbW = Math.Min(tbW, MaxTextboxWidth);
                    tbW = Math.Max(MinTextboxWidth, tbW);
                }
                else
                {
                    tbW = Math.Max(MinTextboxWidth, TextboxWidth);
                }
            }

            int left = InnerPadding.Left;
            int right = Width - InnerPadding.Right;

            // tổng bề rộng: minus + (textbox) + plus + các khe 1px
            int total = btnW + 1 + (_textboxVisible ? tbW + 1 : 0) + btnW;
            // căn giữa trong control
            int x0 = left + Math.Max(0, (right - left - total) / 2);

            Rectangle rMinus = new Rectangle(x0, top, btnW, h);
            Rectangle rPlus = new Rectangle(x0 + btnW + 1 + (_textboxVisible ? tbW + 1 : 0), top, btnW, h);

            _btnMinus.Bounds = rMinus;
            _btnPlus.Bounds = rPlus;

            if (_textboxVisible)
            {
                Rectangle rTb = new Rectangle(rMinus.Right + 1, top, tbW, h);
                _tb.SetBounds(rTb.X, rTb.Y, rTb.Width, rTb.Height);
            }
            else
            {
                _tb.SetBounds(rMinus.Right + 1, top, 1, h);
            }

            _btnMinus.Invalidate();
            _btnPlus.Invalidate();
        }

        // ====== Paint (viền nhẹ tuỳ chọn) ======
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(b, ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (BorderRadius <= 0)
            {
                using (Pen p = new Pen(BorderColor))
                    e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rc = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath gp = RoundRect(rc, BorderRadius))
            using (Pen p = new Pen(BorderColor))
                e.Graphics.DrawPath(p, gp);
        }

        private static GraphicsPath RoundRect(Rectangle rect, int radius)
        {
            int r = Math.Max(1, radius);
            int d = r * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static bool InDesigner()
        {
            return (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                || (Application.ExecutablePath != null &&
                    Application.ExecutablePath.EndsWith("devenv.exe", StringComparison.OrdinalIgnoreCase));
        }

        private void ApplyTextboxFont()
        {
            if (_tb == null) return;
            Font want = new Font(Font.FontFamily, _textboxFontSize, FontStyle.Regular, GraphicsUnit.Point);
            if (_tb.Font == null || Math.Abs(_tb.Font.Size - want.Size) > 0.1f || _tb.Font.Style != want.Style)
                _tb.Font = want;
            _tb.TextAlign = HorizontalAlignment.Center;
        }

        private void ShowTextbox()
        {
            if (_textboxVisible) return;
            _textboxVisible = true;
            _tb.Visible = true;
            LayoutChildren();
            Invalidate();
        }
        private void HideTextbox()
        {
            if (!_textboxVisible) return;
            _textboxVisible = false;
            _tb.Visible = false;
            LayoutChildren();
            Invalidate();
        }
        private void TryHideTextboxOnLeave()
        {
            if (InDesigner()) return;
            if (!AutoShowTextbox) return;
            if (_tb.Focused) return;

            Point p = PointToClient(Cursor.Position);
            if (!_btnMinus.Bounds.Contains(p) && !_btnPlus.Bounds.Contains(p) && !_tb.Bounds.Contains(p))
                HideTextbox();
        }

        // ====== Inner Button ======
        private sealed class PlusMinusButton : Control
        {
            private readonly int _dir; // -1 or +1
            private bool _hover, _press;

            public int CornerRadius { get; set; } = 8;
            public Color BaseColor { get; set; } = Color.FromArgb(245, 245, 245);
            public Color HoverColor { get; set; } = Color.FromArgb(235, 235, 235);
            public Color PressColor { get; set; } = Color.FromArgb(220, 220, 220);
            public Color BorderColor { get; set; } = Color.FromArgb(180, 180, 180);
            public float BorderWidth { get; set; } = 1.25f;

            public PlusMinusButton(int dir)
            {
                _dir = Math.Sign(dir) >= 0 ? +1 : -1;

                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.UserPaint |
                         ControlStyles.SupportsTransparentBackColor, true);

                BackColor = Color.Transparent;
                Cursor = Cursors.Hand;

                // Không cho focus hệ thống -> không viền xanh chấm
                TabStop = false;
                SetStyle(ControlStyles.Selectable, false);

                Size = new Size(32, 28);
                MinimumSize = new Size(16, 16);
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                if (Width <= 4 || Height <= 4) { Region = null; return; }

                Rectangle rgnRect = new Rectangle(1, 1, Width - 2, Height - 2);
                int rr = Math.Min(CornerRadius, Math.Min(rgnRect.Width, rgnRect.Height) / 2);
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rgnRect.X, rgnRect.Y, 2 * rr, 2 * rr, 180, 90);
                    path.AddArc(rgnRect.Right - 2 * rr, rgnRect.Y, 2 * rr, 2 * rr, 270, 90);
                    path.AddArc(rgnRect.Right - 2 * rr, rgnRect.Bottom - 2 * rr, 2 * rr, 2 * rr, 0, 90);
                    path.AddArc(rgnRect.X, rgnRect.Bottom - 2 * rr, 2 * rr, 2 * rr, 90, 90);
                    path.CloseFigure();
                    Region = new Region(path);
                }
            }

            protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
            protected override void OnMouseLeave(EventArgs e) { _hover = false; _press = false; Invalidate(); base.OnMouseLeave(e); }
            protected override void OnMouseDown(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left) { _press = true; Invalidate(); }
                base.OnMouseDown(e);
            }
            protected override void OnMouseUp(MouseEventArgs e)
            {
                if (_press && e.Button == MouseButtons.Left)
                {
                    _press = false; Invalidate();
                    if (ClientRectangle.Contains(e.Location)) OnClick(EventArgs.Empty);
                }
                base.OnMouseUp(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                if (Width <= 2 || Height <= 2) return;
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                RectangleF rect = new RectangleF(1.5f, 1.5f, Width - 3f, Height - 3f);
                float r = Math.Min(CornerRadius, Math.Min(rect.Width, rect.Height) / 2f);

                Color c0 = _press ? PressColor : _hover ? HoverColor : BaseColor;
                Color cTop = ControlPaint.Light(c0, 0.05f);
                Color cBot = ControlPaint.Dark(c0, 0.02f);

                using (GraphicsPath path = RoundRectF(rect, r))
                using (LinearGradientBrush lg = new LinearGradientBrush(Rectangle.Truncate(rect), cTop, cBot, 90f))
                using (Pen pen = new Pen(BorderColor, BorderWidth) { Alignment = PenAlignment.Inset })
                {
                    RectangleF sh = rect; sh.Offset(0, 1f);
                    using (SolidBrush shadow = new SolidBrush(Color.FromArgb(30, Color.Black)))
                    using (GraphicsPath shPath = RoundRectF(sh, r))
                        g.FillPath(shadow, shPath);

                    g.FillPath(lg, path);
                    g.DrawPath(pen, path);
                }

                float cx = Width / 2f, cy = Height / 2f;
                Color glyph = _press ? Color.FromArgb(40, 40, 40) : Color.FromArgb(60, 60, 60);
                using (Pen p2 = new Pen(glyph, 2.2f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                {
                    int s = Math.Max(8, Math.Min(Width, Height) / 2);
                    g.DrawLine(p2, cx - s / 2f, cy, cx + s / 2f, cy); // minus
                    if (_dir > 0)
                        g.DrawLine(p2, cx, cy - s / 2f, cx, cy + s / 2f); // plus
                }
            }

            private static GraphicsPath RoundRectF(RectangleF rect, float radius)
            {
                GraphicsPath path = new GraphicsPath();
                if (radius <= 0f || rect.Width <= 0f || rect.Height <= 0f)
                {
                    path.AddRectangle(Rectangle.Truncate(rect));
                    path.CloseFigure();
                    return path;
                }

                float rr = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2f);
                path.AddArc(rect.Left, rect.Top, 2 * rr, 2 * rr, 180, 90);
                path.AddArc(rect.Right - 2 * rr, rect.Top, 2 * rr, 2 * rr, 270, 90);
                path.AddArc(rect.Right - 2 * rr, rect.Bottom - 2 * rr, 2 * rr, 2 * rr, 0, 90);
                path.AddArc(rect.Left, rect.Bottom - 2 * rr, 2 * rr, 2 * rr, 90, 90);
                path.CloseFigure();
                return path;
            }
        }
    }
}
