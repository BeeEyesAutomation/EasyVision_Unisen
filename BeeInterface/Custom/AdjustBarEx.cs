using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace BeeInterface
{
    [DefaultEvent(nameof(ValueChanged))]
    public class AdjustBarEx : UserControl
    {
        // ===== Values / Behavior =====
        private float _min = 0f, _max = 100f, _step = 1f, _value = 0f;
        private int _decimals = 0;
        private bool _snapToStep = true;
        private bool _showValueOnThumb = true;

        private float _wheelStep = 1f;
        private float _keyboardStep = 1f;
        private string _unitText = "";

        // ===== Appearance (normal palette) =====
        [Category("Appearance")] public Color ColorTrack { get; set; } = Color.FromArgb(235, 235, 235);
        [Category("Appearance")] public Color ColorScale { get; set; } = Color.FromArgb(180, 180, 180);
        [Category("Appearance")] public Color ColorFill { get; set; } = Color.FromArgb(246, 213, 143);
        [Category("Appearance")] public Color ColorThumb { get; set; } = Color.FromArgb(246, 201, 110);
        [Category("Appearance")] public Color ColorThumbBorder { get; set; } = Color.FromArgb(246, 201, 110);
        [Category("Appearance")] public Color ColorBorder { get; set; } = Color.FromArgb(210, 210, 210);
        [Category("Appearance")] public int Radius { get; set; } = 8;
        [Category("Appearance")] public Padding InnerPadding { get; set; } = new Padding(10, 6, 10, 6);

        // ===== Auto-dim when disabled (only trackbar, buttons, textbox) =====
        [Category("Appearance"), Description("Tỉ lệ pha về xám khi Disabled (0..1). 0=giữ màu, 1=thành xám.")]
        public float DisabledDesaturateMix { get; set; } = 0.30f;

        [Category("Appearance"), Description("Hệ số giảm sáng khi Disabled (0..1). Nhỏ hơn = tối hơn.")]
        public float DisabledDimFactor { get; set; } = 0.90f;

        // ===== Auto-Repeat (press & hold +/-) =====
        [Category("Behavior")] public bool AutoRepeatEnabled { get; set; } = true;
        [Category("Behavior")] public int AutoRepeatInitialDelay { get; set; } = 400; // ms
        [Category("Behavior")] public int AutoRepeatInterval { get; set; } = 60;      // ms
        [Category("Behavior")] public bool AutoRepeatAccelerate { get; set; } = true;
        [Category("Behavior")] public int AutoRepeatMinInterval { get; set; } = 20;   // ms
        [Category("Behavior")] public int AutoRepeatAccelDeltaMs { get; set; } = -5;  // mỗi tick giảm bấy nhiêu ms

        // ===== Responsive ratios =====
        [Category("Appearance")] public float ThumbValueFontScale { get; set; } = 1.00f;
        [Category("Appearance")] public int ThumbValuePadding { get; set; } = -1;    // -1 = auto, >=0 = px
        [Category("Appearance")] public bool ThumbValueBold { get; set; } = true;

        [Category("Layout")] public float TrackWidthRatio { get; set; } = 1.00f;
        [Category("Layout")] public float TrackHeightRatio { get; set; } = 0.40f;
        [Category("Layout")] public int MinTrackHeight { get; set; } = 8;
        [Category("Layout")] public int MaxTrackHeight { get; set; } = 1000;

        [Category("Layout")] public float ThumbDiameterRatio { get; set; } = 1.15f;
        [Category("Layout")] public int MinThumb { get; set; } = 24;
        [Category("Layout")] public int MaxThumb { get; set; } = 1000;

        [Browsable(false)] public int TrackHeightPx { get; private set; } = 12;
        [Browsable(false)] public int ThumbSizePx { get; private set; } = 28;

        // ===== Bar edges & gaps =====
        [Category("Layout")] public bool TightEdges { get; set; } = true;
        [Category("Layout")] public int EdgePadding { get; set; } = 2;
        [Category("Layout")] public int BarLeftGap { get; set; } = 10;
        [Category("Layout")] public int BarRightGap { get; set; } = 6;

        // ===== Chrome (textbox + +/-) =====
        [Category("Layout")] public float ChromeWidthRatio { get; set; } = 0.14f;
        [Category("Layout")] public int MinChromeWidth { get; set; } = 64;
        [Category("Layout")] public int ChromeGap { get; set; } = 10;
        [Category("Layout")] public int TextboxSidePadding { get; set; } = 12;

        // ===== Back-compat properties =====
        [Category("Layout")] public bool AutoSizeTextbox { get; set; } = true;
        [Category("Layout")] public bool MatchTextboxFontToThumb { get; set; } = false; // không dùng
        [Category("Layout")] public int MinTextboxWidth { get; set; } = 16;
        [Category("Layout")] public int MaxTextboxWidth { get; set; } = 0;  // 0 = unlimited
        [Category("Layout")] public int TextboxWidth { get; set; } = 56;    // dùng khi AutoSizeTextbox=false

        // ===== TextBox font size =====
        private float _textboxFontSize = 20f;
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

        // ===== Show/Hide textbox =====
        [Category("Behavior")] public bool AutoShowTextbox { get; set; } = true;
        [Category("Behavior")] public bool StartWithTextboxHidden { get; set; } = true;

        private bool _textboxVisible = true;
        private int _chromeWidthActual = 0;

        // ===== Internal controls =====
        private TextBox _tb;
        private PlusMinusButton _btnMinus, _btnPlus;

        // ===== Drag =====
        private bool _dragging = false;
        private int _dragXStart;
        private float _dragValueStart;

        // ===== Auto-repeat =====
        private Timer _repeatTimer;
        private int _repeatDirection; // -1 hoặc +1
        private int _repeatPhase;     // 0 = delay đầu, 1 = lặp/accelerate

        // ===== Init guard =====
        private bool _isInit;

        // ===== Public API =====
        [Category("Behavior")]
        public float Min
        {
            get { return _min; }
            set { _min = value; if (_max <= _min) _max = _min + 1; Value = Clamp(Value); Invalidate(); }
        }
        [Category("Behavior")]
        public float Max
        {
            get { return _max; }
            set { _max = value; if (_max <= _min) _min = _max - 1; Value = Clamp(Value); Invalidate(); }
        }
        [Category("Behavior")]
        public float Step
        {
            get { return _step; }
            set
            {
                _step = Math.Max(0.000001f, value);
                if (_keyboardStep == 0f) _keyboardStep = _step;
                if (_wheelStep == 0f) _wheelStep = _step;
                if (_snapToStep) Value = Snap(Value);
                Invalidate();
            }
        }
        [Category("Behavior")]
        public bool SnapToStep
        {
            get { return _snapToStep; }
            set { _snapToStep = value; if (value) Value = Snap(Value); }
        }
        [Category("Behavior")]
        public int Decimals
        {
            get { return _decimals; }
            set { _decimals = Math.Max(0, value); UpdateText(); Invalidate(); }
        }
        [Category("Behavior")]
        public string UnitText
        {
            get { return _unitText; }
            set { _unitText = value ?? ""; UpdateText(); Invalidate(); }
        }
        [Category("Behavior")]
        public bool ShowValueOnThumb
        {
            get { return _showValueOnThumb; }
            set { _showValueOnThumb = value; Invalidate(); }
        }
        [Category("Behavior")]
        public float WheelStep { get { return _wheelStep; } set { _wheelStep = Math.Max(0.000001f, value); } }
        [Category("Behavior")]
        public float KeyboardStep { get { return _keyboardStep; } set { _keyboardStep = Math.Max(0.000001f, value); } }
        public bool IsInital = false;

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
                if (_isInit) LayoutChildren(); // auto-width theo text mới
                Invalidate();
                if (!IsInital)
                    ValueChanged?.Invoke(_value);
                else
                    IsInital = false;
            }
        }

        public event Action<float> ValueChanged;

        public AdjustBarEx()
        {
            SetStyle(ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

            DoubleBuffered = true;
            UpdateStyles();

            _tb = new TextBox
            {
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center,
                Multiline = false,
                TabStop = true
            };
            _tb.KeyDown += Tb_KeyDown;
            _tb.Leave += Tb_Leave;

            _tb.TextChanged += (s, e) =>
            {
                if (!_isInit) return;
                LayoutChildren();
                Invalidate();
            };

            _tb.MouseEnter += (s, e) => { if (AutoShowTextbox && !InDesigner()) ShowTextbox(); };
            _tb.Enter += (s, e) => { if (AutoShowTextbox && !InDesigner()) ShowTextbox(); };
            _tb.MouseLeave += (s, e) => { TryHideTextboxOnLeave(); };
            Controls.Add(_tb);

            _btnMinus = new PlusMinusButton(-1);
            _btnPlus = new PlusMinusButton(+1);

            _btnMinus.MouseEnter += (s, e) => { if (AutoShowTextbox && !InDesigner()) ShowTextbox(); };
            _btnMinus.MouseLeave += (s, e) => { TryHideTextboxOnLeave(); };
            _btnPlus.MouseEnter += (s, e) => { if (AutoShowTextbox && !InDesigner()) ShowTextbox(); };
            _btnPlus.MouseLeave += (s, e) => { TryHideTextboxOnLeave(); };

            // Auto-repeat: xử lý trên MouseDown/Up/Leave
            _btnMinus.MouseDown += (s, e) =>
            {
                if (!Enabled || e.Button != MouseButtons.Left) return;
                if (AutoShowTextbox && !InDesigner()) ShowTextbox();
                Focus();
                ApplyStep(-1);
                BeginRepeat(-1);
            };
            _btnMinus.MouseUp += (s, e) => StopRepeat();
            _btnMinus.MouseLeave += (s, e) => { if ((Control.MouseButtons & MouseButtons.Left) == 0) StopRepeat(); };

            _btnPlus.MouseDown += (s, e) =>
            {
                if (!Enabled || e.Button != MouseButtons.Left) return;
                if (AutoShowTextbox && !InDesigner()) ShowTextbox();
                Focus();
                ApplyStep(+1);
                BeginRepeat(+1);
            };
            _btnPlus.MouseUp += (s, e) => StopRepeat();
            _btnPlus.MouseLeave += (s, e) => { if ((Control.MouseButtons & MouseButtons.Left) == 0) StopRepeat(); };

            Controls.Add(_btnMinus);
            Controls.Add(_btnPlus);

            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10f);
            Height = 44;

            // Inputs
            MouseDown += AdjustBarEx_MouseDown;
            MouseMove += AdjustBarEx_MouseMove;
            MouseUp += AdjustBarEx_MouseUp;
            MouseWheel += AdjustBarEx_MouseWheel;
           
            KeyDown += AdjustBarEx_KeyDown;
            MouseLeave += (s, e) => { TryHideTextboxOnLeave(); };

            // Trạng thái ban đầu
            _textboxVisible = !StartWithTextboxHidden || InDesigner();
            _tb.Visible = _textboxVisible;

            ApplyTextboxFont();
            RecalcMetrics();
            UpdateText();

            _isInit = true;
            LayoutChildren();
        }

    

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_repeatTimer != null)
                {
                    _repeatTimer.Stop();
                    _repeatTimer.Tick -= RepeatTimer_Tick;
                    _repeatTimer.Dispose();
                    _repeatTimer = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            //if (InDesigner())
            //{
            //    AutoShowTextbox = false;         // design-time: luôn hiện textbox
            //    _textboxVisible = true;
            //    if (_tb != null) _tb.Visible = true;
            //    if (_btnMinus != null) _btnMinus.Visible = true;
            //    if (_btnPlus != null) _btnPlus.Visible = true;
            //    MinimumSize = new Size(140, 36);
            //    LayoutChildren();
            //    Invalidate();
            //}
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ApplyTextboxFont();
            if (_isInit) LayoutChildren();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (!Enabled) StopRepeat();

            // chỉ dim textbox; background giữ nguyên
            if (_tb != null)
            {
                _tb.Enabled = this.Enabled;
                _tb.ReadOnly = !this.Enabled;
                _tb.ForeColor = this.Enabled ? ForeColor : DimAuto(ForeColor);
                _tb.BackColor = this.Enabled ? Color.White : DimAuto(Color.White);
            }
            if (_btnMinus != null) _btnMinus.Enabled = this.Enabled;
            if (_btnPlus != null) _btnPlus.Enabled = this.Enabled;

            Invalidate();
        }

        // ===== Helpers =====
        private static bool InDesigner()
        {
            return (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                || (Application.ExecutablePath != null &&
                    Application.ExecutablePath.EndsWith("devenv.exe", StringComparison.OrdinalIgnoreCase));
        }

        private void ShowTextbox()
        {
            if (_textboxVisible) return;
            _textboxVisible = true;
            _tb.Visible = true;
            LayoutChildren();
            _btnMinus.Invalidate();
            _btnPlus.Invalidate();
            Invalidate();
        }
        private void HideTextbox()
        {
            if (!_textboxVisible) return;
            _textboxVisible = false;
            _tb.Visible = false;
            LayoutChildren();
            _btnMinus.Invalidate();
            _btnPlus.Invalidate();
            Invalidate();
        }
        private void TryHideTextboxOnLeave()
        {
            if (_dragging) { _dragging = false; Capture = false; }
            if (InDesigner()) return;
            if (!AutoShowTextbox) return;
            if (_tb.Focused) return;

            Point p = PointToClient(Cursor.Position);
            if (!_btnMinus.Bounds.Contains(p) && !_btnPlus.Bounds.Contains(p) && !_tb.Bounds.Contains(p))
                HideTextbox();
        }

        private void ApplyTextboxFont()
        {
            if (_tb == null) return;
            Font want = new Font(Font.FontFamily, _textboxFontSize, FontStyle.Regular, GraphicsUnit.Point);
            if (_tb.Font == null || Math.Abs(_tb.Font.Size - want.Size) > 0.1f || _tb.Font.Style != want.Style)
                _tb.Font = want;
            _tb.TextAlign = HorizontalAlignment.Center;
        }

        private void RecalcMetrics()
        {
            int usableH = Math.Max(1, Height - InnerPadding.Vertical);
            int h = (int)Math.Round(usableH * TrackHeightRatio);
            h = Math.Max(MinTrackHeight, Math.Min(MaxTrackHeight, h));
            TrackHeightPx = h;

            int th = (int)Math.Round(h * ThumbDiameterRatio);
            th = Math.Max(MinThumb, Math.Min(MaxThumb, th));
            ThumbSizePx = th;
        }

        // ===== Input handlers =====
        private void AdjustBarEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Enabled) return;
            if (e.KeyCode == Keys.Left) { Value = _value - KeyboardStep; e.Handled = true; }
            if (e.KeyCode == Keys.Right) { Value = _value + KeyboardStep; e.Handled = true; }
            if (e.KeyCode == Keys.PageDown) { Value = _value - 10 * KeyboardStep; e.Handled = true; }
            if (e.KeyCode == Keys.PageUp) { Value = _value + 10 * KeyboardStep; e.Handled = true; }
            if (e.KeyCode == Keys.Home) { Value = Min; e.Handled = true; }
            if (e.KeyCode == Keys.End) { Value = Max; e.Handled = true; }
        }

        private void AdjustBarEx_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;
            if (!ClientRectangle.Contains(PointToClient(Cursor.Position))) return;
            Value = e.Delta > 0 ? _value + WheelStep : _value - WheelStep;
        }

        private void AdjustBarEx_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;
            if (e.Button != MouseButtons.Left) return;

            var rects = GetRects();
            Rectangle trackRect = rects.Item1;
            Rectangle thumb = rects.Item2;

            if (thumb.Contains(e.Location) || trackRect.Contains(e.Location))
                HideTextbox();

            if (thumb.Contains(e.Location))
            {
                _dragging = true;
                _dragXStart = e.X;
                _dragValueStart = _value;
                Capture = true;
            }
            else if (trackRect.Contains(e.Location))
            {
                Value = XToValue(e.X, trackRect);
            }
        }

        private void AdjustBarEx_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;
            if (_dragging)
            {
                if (_textboxVisible) HideTextbox();
                Rectangle trackRect = GetRects().Item1;
                float dx = e.X - _dragXStart;
                float dv = dx / Math.Max(1f, trackRect.Width) * (Max - Min);
                Value = _dragValueStart + dv;
            }
        }

        private void AdjustBarEx_MouseUp(object sender, EventArgs e)
        {
            if (!Enabled) return;
            if (_dragging) { _dragging = false; Capture = false; }
        }

        // ===== TextBox events =====
        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Enabled) return;
            if (e.KeyCode == Keys.Enter)
            {
                ParseTextbox(); e.Handled = true; e.SuppressKeyPress = true; Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                UpdateText(); e.Handled = true; e.SuppressKeyPress = true; Focus();
            }
        }
        private void Tb_Leave(object sender, EventArgs e) { if (!Enabled) return; ParseTextbox(); TryHideTextboxOnLeave(); }

        private void ParseTextbox()
        {
            string s = _tb.Text != null ? _tb.Text.Trim() : "0";
            if (!string.IsNullOrEmpty(_unitText) &&
                s.EndsWith(_unitText, StringComparison.OrdinalIgnoreCase))
            {
                s = s.Substring(0, s.Length - _unitText.Length).Trim();
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

        // ===== Layout =====
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!_isInit) return;
            RecalcMetrics();
            LayoutChildren();
            Invalidate();
        }

        private void LayoutChildren()
        {
            if (!_isInit) return;
            if (_tb == null || _btnMinus == null || _btnPlus == null) return;

            _btnMinus.Visible = _btnPlus.Visible = true;
            _tb.Visible = _textboxVisible;

            int top = InnerPadding.Top;
            int bottom = Height - InnerPadding.Bottom;
            int chromeH = bottom - top;

            int btnH = chromeH;
            int btnW = Math.Min(36, Math.Max(24, btnH));

            ApplyTextboxFont();
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

            int chromeBase = Math.Max(MinChromeWidth, (int)(Width * ChromeWidthRatio));
            int chromeNeed = btnW + 1 + tbW + 1 + btnW;
            int chromeW = Math.Max(chromeBase, chromeNeed);
            _chromeWidthActual = chromeW;

            Rectangle rBox = new Rectangle(
                Width - (TightEdges ? EdgePadding : InnerPadding.Right) - chromeW,
                top, chromeW, chromeH);

            Rectangle rMinus = new Rectangle(rBox.X, rBox.Y, btnW, rBox.Height);
            Rectangle rPlus = new Rectangle(rBox.Right - btnW, rBox.Y, btnW, rBox.Height);

            Rectangle rTbArea = Rectangle.FromLTRB(rMinus.Right + 1, rBox.Y, rPlus.Left - 1, rBox.Bottom);
            if (_textboxVisible) tbW = Math.Min(tbW, rTbArea.Width);

            int tbH = rBox.Height;
            int tbX = rTbArea.X + (rTbArea.Width - tbW) / 2;
            int tbY = rTbArea.Y + (rTbArea.Height - tbH) / 2;

            _btnMinus.Bounds = rMinus;
            _btnPlus.Bounds = rPlus;

            _btnMinus.Invalidate();
            _btnPlus.Invalidate();
            if (_textboxVisible)
                _tb.SetBounds(tbX, tbY, tbW, tbH);
            else
                _tb.SetBounds(rTbArea.X, rTbArea.Y, 1, 1);
        }

        private ValueTuple<Rectangle, Rectangle> GetRects()
        {
            int left = (TightEdges ? EdgePadding : InnerPadding.Left) + BarLeftGap;
            int right = Width - (TightEdges ? EdgePadding : InnerPadding.Right) - BarRightGap;
            int top = InnerPadding.Top;
            int bot = Height - InnerPadding.Bottom;

            int cw = (_chromeWidthActual > 0)
                     ? _chromeWidthActual
                     : Math.Max(MinChromeWidth, (int)(Width * ChromeWidthRatio));
            right -= (cw + (TightEdges ? EdgePadding : 4) + ChromeGap);

            int innerW = Math.Max(1, right - left);
            int trackW = TightEdges ? innerW : Math.Max(1, (int)Math.Round(innerW * TrackWidthRatio));
            int trackX = TightEdges ? left : left + (innerW - trackW) / 2;
            int h = TrackHeightPx;

            Rectangle trackRect = new Rectangle(trackX, top + (bot - top - h) / 2, trackW, h);

            int th = ThumbSizePx;
            float t = (Max <= Min) ? 0f : (Value - Min) / (Max - Min);
            int cx = trackRect.Left + (int)Math.Round(t * trackRect.Width);
            Rectangle thumb = new Rectangle(cx - th / 2, trackRect.Top + (trackRect.Height - th) / 2, th, th);

            return new ValueTuple<Rectangle, Rectangle>(trackRect, thumb);
        }

        private float XToValue(int x, Rectangle trackRect)
        {
            float t = (x - trackRect.Left) / Math.Max(1f, (float)trackRect.Width);
            float v = Min + t * (Max - Min);
            return _snapToStep ? Snap(v) : v;
        }

        // ===== Paint =====
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // KHÔNG dim nền: luôn dùng BackColor
            using (SolidBrush b = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(b, ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (InDesigner())
            {
                try { PaintCore(e.Graphics); }
                catch
                {
                    e.Graphics.Clear(BackColor);
                    using (var p = new Pen(Color.Silver)) e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                    using (var b = new SolidBrush(Color.Gray))
                        e.Graphics.DrawString("AdjustBarEx (design)", Font, b, 6, 6);
                }
                return;
            }
            PaintCore(e.Graphics);
        }

        private void PaintCore(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rects = GetRects();
            Rectangle trackRect = rects.Item1;
            Rectangle thumb = rects.Item2;

            // palette theo trạng thái (dim chỉ khi disabled)
            Color track = Enabled ? ColorTrack : DimAuto(ColorTrack);
            Color scale = Enabled ? ColorScale : DimAuto(ColorScale);
            Color fill = Enabled ? ColorFill : DimAuto(ColorFill);
            Color thumbC = Enabled ? ColorThumb : DimAuto(ColorThumb);
            Color thumbBd = Enabled ? ColorThumbBorder : DimAuto(ColorThumbBorder);
            Color border = Enabled ? ColorBorder : DimAuto(ColorBorder);

            float r = Math.Max(1f, Radius - 2);

            // Track
            using (GraphicsPath gp = RoundRect(trackRect, r))
            using (SolidBrush bTrack = new SolidBrush(track))
            using (Pen pBorder = new Pen(border, 1f))
            {
                g.FillPath(bTrack, gp);
                g.DrawPath(pBorder, gp);
            }

            // Center scale
            using (Pen pScale = new Pen(scale, 1f) { Alignment = PenAlignment.Center })
            {
                g.DrawLine(pScale,
                    trackRect.Left, trackRect.Top + trackRect.Height / 2f,
                    trackRect.Right, trackRect.Top + trackRect.Height / 2f);
            }

            // Fill
            int fillRight = thumb.X + thumb.Width / 2;
            Rectangle fillRect = Rectangle.FromLTRB(
                trackRect.Left, trackRect.Top,
                Math.Max(trackRect.Left, fillRight), trackRect.Bottom);

            using (GraphicsPath gpFill = RoundRect(fillRect, r))
            using (SolidBrush bFill = new SolidBrush(fill))
            {
                if (fillRect.Width > 0) g.FillPath(bFill, gpFill);
            }

            // Thumb
            using (SolidBrush bThumb = new SolidBrush(thumbC))
            using (Pen pThumb = new Pen(thumbBd))
            {
                g.FillEllipse(bThumb, thumb);
                g.DrawEllipse(pThumb, thumb);
            }

            // Value in thumb
            if (_showValueOnThumb)
            {
                string s = FormatValue(_value);
                int inset = (ThumbValuePadding >= 0) ? ThumbValuePadding : Math.Max(2, ThumbSizePx / 10);
                Rectangle box = Rectangle.Inflate(thumb, -inset, -inset);

                float lo = 6f;
                float hi = Math.Max(thumb.Width, thumb.Height);
                float scaleText = (ThumbValueFontScale > 0f) ? ThumbValueFontScale : 1f;
                hi *= scaleText;

                for (int i = 0; i < 12; i++)
                {
                    float mid = (lo + hi) * 0.5f;
                    using (Font f = new Font(Font.FontFamily, mid, ThumbValueBold ? FontStyle.Bold : FontStyle.Regular))
                    {
                        SizeF sz = g.MeasureString(s, f);
                        if (sz.Width <= box.Width && sz.Height <= box.Height) lo = mid;
                        else hi = mid;
                    }
                }

                using (Font ff = new Font(Font.FontFamily, (float)Math.Floor(lo),
                                          ThumbValueBold ? FontStyle.Bold : FontStyle.Regular))
                {
                    var normalText = (ColorThumb.GetBrightness() < 0.5f) ? Color.White : Color.Black;
                    Color txt = Enabled ? normalText : Color.FromArgb(170, normalText);

                    using (SolidBrush br = new SolidBrush(txt))
                    {
                        SizeF ms = g.MeasureString(s, ff);
                        float tx = box.X + (box.Width - ms.Width) / 2f;
                        float ty = box.Y + (box.Height - ms.Height) / 2f;
                        g.DrawString(s, ff, br, tx, ty);
                    }
                }
            }
        }

        // ===== Math & color helpers =====
        private float Clamp(float v) { return Math.Max(_min, Math.Min(_max, v)); }
        private float Snap(float v)
        {
            if (_step <= 0) return v;
            float n = (float)Math.Round((v - _min) / _step, MidpointRounding.AwayFromZero);
            float s = _min + n * _step;
            return Clamp(s);
        }

        private static GraphicsPath RoundRect(Rectangle rect, float radius)
        {
            var path = new GraphicsPath();
            if (radius <= 0f || rect.Width <= 0 || rect.Height <= 0)
            {
                path.AddRectangle(rect);
                path.CloseFigure();
                return path;
            }

            float rr = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2f);
            float d = rr * 2f;

            RectangleF a = new RectangleF(rect.Left, rect.Top, d, d);
            RectangleF b = new RectangleF(rect.Right - d, rect.Top, d, d);
            RectangleF c = new RectangleF(rect.Right - d, rect.Bottom - d, d, d);
            RectangleF d2 = new RectangleF(rect.Left, rect.Bottom - d, d, d);

            path.AddArc(a, 180, 90);
            path.AddLine(rect.Left + rr, rect.Top, rect.Right - rr, rect.Top);
            path.AddArc(b, 270, 90);
            path.AddLine(rect.Right, rect.Top + rr, rect.Right, rect.Bottom - rr);
            path.AddArc(c, 0, 90);
            path.AddLine(rect.Right - rr, rect.Bottom, rect.Left + rr, rect.Bottom);
            path.AddArc(d2, 90, 90);
            path.AddLine(rect.Left, rect.Bottom - rr, rect.Left, rect.Top + rr);

            path.CloseFigure();
            return path;
        }

        // ---- Auto-dim helpers ----
        private static int Clamp255(int v) { return v < 0 ? 0 : (v > 255 ? 255 : v); }
        private Color ToGray(Color c)
        {
            int y = Clamp255((int)Math.Round(0.299 * c.R + 0.587 * c.G + 0.114 * c.B));
            return Color.FromArgb(c.A, y, y, y);
        }
        private Color Lerp(Color a, Color b, float t)
        {
            t = t < 0f ? 0f : (t > 1f ? 1f : t);
            int r = Clamp255((int)Math.Round(a.R + (b.R - a.R) * t));
            int g = Clamp255((int)Math.Round(a.G + (b.G - a.G) * t));
            int bC = Clamp255((int)Math.Round(a.B + (b.B - a.B) * t));
            return Color.FromArgb(a.A, r, g, bC);
        }
        private Color DimAuto(Color c)
        {
            // pha về xám rồi giảm sáng (chỉ áp dụng cho trackbar/nút/textbox)
            Color desat = Lerp(c, ToGray(c), DisabledDesaturateMix);
            int r = Clamp255((int)Math.Round(desat.R * DisabledDimFactor));
            int g = Clamp255((int)Math.Round(desat.G * DisabledDimFactor));
            int b = Clamp255((int)Math.Round(desat.B * DisabledDimFactor));
            return Color.FromArgb(c.A, r, g, b);
        }

        // ===== Auto-repeat helpers =====
        private void ApplyStep(int dir)
        {
            if (!Enabled) return;
            Value = _value + dir * _step;
        }
        private void BeginRepeat(int dir)
        {
            if (!AutoRepeatEnabled || !Enabled) return;

            _repeatDirection = (dir >= 0) ? +1 : -1;
            if (_repeatTimer == null)
            {
                _repeatTimer = new Timer();
                _repeatTimer.Tick += RepeatTimer_Tick;
            }
            _repeatPhase = 0;
            _repeatTimer.Interval = Math.Max(1, AutoRepeatInitialDelay);
            _repeatTimer.Start();
        }
        private void StopRepeat()
        {
            if (_repeatTimer != null) _repeatTimer.Stop();
        }
        private void RepeatTimer_Tick(object sender, EventArgs e)
        {
            if (!Enabled) { StopRepeat(); return; }

            ApplyStep(_repeatDirection);

            if (_repeatPhase == 0)
            {
                _repeatTimer.Interval = Math.Max(1, AutoRepeatInterval);
                _repeatPhase = 1;
            }
            else if (AutoRepeatAccelerate)
            {
                int next = _repeatTimer.Interval + AutoRepeatAccelDeltaMs; // âm => nhanh dần
                _repeatTimer.Interval = Math.Max(AutoRepeatMinInterval, next);
            }
        }

        // ===== PlusMinusButton =====
        private sealed class PlusMinusButton : Control
        {
            private readonly int _dir; // -1 or +1
            private bool _hover, _press;

            public int CornerRadius { get; set; } = 4;
            public Color BaseColor { get; set; } = Color.FromArgb(245, 245, 245);
            public Color HoverColor { get; set; } = Color.FromArgb(235, 235, 235);
            public Color PressColor { get; set; } = Color.FromArgb(220, 220, 220);
            public Color BorderColor { get; set; } = Color.Transparent;
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
                using (GraphicsPath path = RoundRectF(rgnRect, rr))
                {
                    Region = new Region(path);
                }
            }

            protected override void OnMouseEnter(EventArgs e) { if (!Enabled) return; _hover = true; Invalidate(); base.OnMouseEnter(e); }
            protected override void OnMouseLeave(EventArgs e) { _hover = false; _press = false; Invalidate(); base.OnMouseLeave(e); }
            protected override void OnMouseDown(MouseEventArgs e)
            {
                if (!Enabled) { base.OnMouseDown(e); return; }
                if (e.Button == MouseButtons.Left) { _press = true; Invalidate(); }
                base.OnMouseDown(e);
            }
            protected override void OnMouseUp(MouseEventArgs e)
            {
                bool fire = _press && e.Button == MouseButtons.Left && ClientRectangle.Contains(e.Location);
                _press = false; Invalidate();
                if (Enabled && fire) OnClick(EventArgs.Empty);
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

                // chọn màu theo trạng thái
                Color c0 = _press ? PressColor : _hover ? HoverColor : BaseColor;
                if (!Enabled) c0 = DimColor(c0, 0.6f, 0.5f); // dim nhẹ khi disabled
                Color cTop = ControlPaint.Light(c0, 0.05f);
                Color cBot = ControlPaint.Dark(c0, 0.02f);
                Color border = !Enabled ? DimColor(BorderColor, 0.6f, 0.5f) : BorderColor;

                using (GraphicsPath path = RoundRectF(Rectangle.Truncate(rect), r))
                using (LinearGradientBrush lg = new LinearGradientBrush(Rectangle.Truncate(rect), cTop, cBot, 90f))
                using (Pen pen = new Pen(border, BorderWidth) { Alignment = PenAlignment.Inset })
                {
                    RectangleF sh = rect; sh.Offset(0, 1f);
                    using (SolidBrush shadow = new SolidBrush(Color.FromArgb(30, Color.Black)))
                    using (GraphicsPath shPath = RoundRectF(Rectangle.Truncate(sh), r))
                        g.FillPath(shadow, shPath);

                    g.FillPath(lg, path);
                    g.DrawPath(pen, path);
                }

                float cx = Width / 2f, cy = Height / 2f;
                Color glyph = !Enabled ? Color.FromArgb(120, 90, 90, 90)
                                       : (_press ? Color.FromArgb(40, 40, 40) : Color.FromArgb(60, 60, 60));
                using (Pen p2 = new Pen(glyph, 2.2f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                {
                    int s = Math.Max(8, Math.Min(Width, Height) / 2);
                    g.DrawLine(p2, cx - s / 2f, cy, cx + s / 2f, cy); // minus
                    if (_dir > 0)
                        g.DrawLine(p2, cx, cy - s / 2f, cx, cy + s / 2f); // plus
                }
            }

            // RoundRectF overloads using Rectangle / RectangleF + 3-arg AddArc
            private static GraphicsPath RoundRectF(Rectangle rect, float radius)
            {
                var path = new GraphicsPath();
                if (radius <= 0f || rect.Width <= 0 || rect.Height <= 0)
                {
                    path.AddRectangle(rect);
                    path.CloseFigure();
                    return path;
                }

                float rr = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2f);
                float d = rr * 2f;

                RectangleF a = new RectangleF(rect.Left, rect.Top, d, d);
                RectangleF b = new RectangleF(rect.Right - d, rect.Top, d, d);
                RectangleF c = new RectangleF(rect.Right - d, rect.Bottom - d, d, d);
                RectangleF d2 = new RectangleF(rect.Left, rect.Bottom - d, d, d);

                path.AddArc(a, 180, 90);
                path.AddLine(rect.Left + rr, rect.Top, rect.Right - rr, rect.Top);
                path.AddArc(b, 270, 90);
                path.AddLine(rect.Right, rect.Top + rr, rect.Right, rect.Bottom - rr);
                path.AddArc(c, 0, 90);
                path.AddLine(rect.Right - rr, rect.Bottom, rect.Left + rr, rect.Bottom);
                path.AddArc(d2, 90, 90);
                path.AddLine(rect.Left, rect.Bottom - rr, rect.Left, rect.Top + rr);

                path.CloseFigure();
                return path;
            }

            private static GraphicsPath RoundRectF(RectangleF rect, float radius)
            {
                var path = new GraphicsPath();
                if (radius <= 0f || rect.Width <= 0f || rect.Height <= 0f)
                {
                    path.AddRectangle(Rectangle.Truncate(rect));
                    path.CloseFigure();
                    return path;
                }

                float rr = Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2f);
                float d = rr * 2f;

                RectangleF a = new RectangleF(rect.Left, rect.Top, d, d);
                RectangleF b = new RectangleF(rect.Right - d, rect.Top, d, d);
                RectangleF c = new RectangleF(rect.Right - d, rect.Bottom - d, d, d);
                RectangleF d2 = new RectangleF(rect.Left, rect.Bottom - d, d, d);

                path.AddArc(a, 180, 90);
                path.AddLine(rect.Left + rr, rect.Top, rect.Right - rr, rect.Top);
                path.AddArc(b, 270, 90);
                path.AddLine(rect.Right, rect.Top + rr, rect.Right, rect.Bottom - rr);
                path.AddArc(c, 0, 90);
                path.AddLine(rect.Right - rr, rect.Bottom, rect.Left + rr, rect.Bottom);
                path.AddArc(d2, 90, 90);
                path.AddLine(rect.Left, rect.Bottom - rr, rect.Left, rect.Top + rr);

                path.CloseFigure();
                return path;
            }

            // local helper dim cho nút
            private static Color DimColor(Color c, float dim, float mixGray)
            {
                int y = Clamp255((int)Math.Round(0.299 * c.R + 0.587 * c.G + 0.114 * c.B));
                Color gray = Color.FromArgb(c.A, y, y, y);
                float t = mixGray < 0f ? 0f : (mixGray > 1f ? 1f : mixGray);
                int r = Clamp255((int)Math.Round((c.R + (gray.R - c.R) * t) * dim));
                int g = Clamp255((int)Math.Round((c.G + (gray.G - c.G) * t) * dim));
                int b = Clamp255((int)Math.Round((c.B + (gray.B - c.B) * t) * dim));
                return Color.FromArgb(c.A, r, g, b);
            }

            private static int Clamp255(int v) => v < 0 ? 0 : (v > 255 ? 255 : v);
        }
    }
}
