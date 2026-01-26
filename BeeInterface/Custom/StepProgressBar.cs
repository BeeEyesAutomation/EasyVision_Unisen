using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace BeeInterface
{
    public class StepProgressBar : UserControl
    {
        // ================= DATA =================
        private readonly List<string> _steps = new List<string>();

        // SỐ BƯỚC ĐÃ HOÀN THÀNH
        // 0 = chưa xong bước nào
        private int _currentStep  = 0;

        // ================= COLORS =================
        private Color _doneColor = Color.FromArgb(120, 78, 45);
        private Color _pendingStepColor = Color.FromArgb(210, 195, 180);
        private Color _runningColor = Color.FromArgb(180, 150, 110);
        [Category("Appearance")]
        [Description("Màu của bước đang chạy")]
        public Color RunningColor
        {
            get => _runningColor;
            set { _runningColor = value; Invalidate(); }
        }
        // ================= LAYOUT (AUTO) =================
        private int _gap = 6;
        private int _arrowTip = 18;
        private int _corner = 10;
        private int _border = 2;
        private int _fontPadding = 10;

        // ================= ANIMATION =================
        private readonly Timer _animTimer;
        private int _animOffset = 0;
        private bool _enableAnimation = true;

        // ================= CTOR =================
        public StepProgressBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);

            Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            // Theme mặc định
            BackColor = Color.FromArgb(245, 235, 225);
            ForeColor = Color.FromArgb(114, 114, 114);

            Height = 56;

            // Animation timer (UI-safe)
            _animTimer = new Timer();
            _animTimer.Interval = 30; // ~33 FPS
            _animTimer.Tick += (s, e) =>
            {
                if (!_enableAnimation) return;

                _animOffset += 2;
                if (_animOffset > Width)
                    _animOffset = 0;

                Invalidate();
            };
            _animTimer.Start();

            // Demo mặc định
            SetSteps(new[] { "Start", "Marking 1", "Camera 1", "Marking 2", "Camera 2", "Done" });
            SetCurrentStep(0);
        }

        // ================= PUBLIC API =================

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<string> Steps => _steps;

        [Category("Behavior")]
        [Description("Số bước đã hoàn thành. 0 = chưa hoàn thành bước nào")]
        public int DoneCount
        {
            get => _currentStep ;
            set => SetCurrentStep(value);
        }

        [Category("Appearance")]
        public Color DoneColor
        {
            get => _doneColor;
            set { _doneColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color PendingStepColor
        {
            get => _pendingStepColor;
            set { _pendingStepColor = value; Invalidate(); }
        }

        [Category("Behavior")]
        [Description("Bật/tắt hiệu ứng progress cho bước đang chạy")]
        public bool EnableAnimation
        {
            get => _enableAnimation;
            set
            {
                _enableAnimation = value;
                if (!_enableAnimation)
                    Invalidate();
            }
        }

        // ================= SETUP =================

        public void SetSteps(IEnumerable<string> steps)
        {
            _steps.Clear();
            if (steps != null)
                _steps.AddRange(steps.Where(s => !string.IsNullOrWhiteSpace(s)));

            _currentStep  = Math.Min(_currentStep , _steps.Count);
            Invalidate();
        }

        public void SetCurrentStep(int currentStep)
        {
            int n = _steps.Count;
            // cho phép 0..n+1
            if (n <= 0)
            {
                _currentStep = 0;
                Invalidate();
                return;
            }

            _currentStep = Math.Max(0, Math.Min(currentStep, n + 1));
            Invalidate();
        }

        /// <summary>
        /// Map enum state → DoneCount
        /// </summary>
        public void SetByEnum<TEnum>(TEnum state, Dictionary<TEnum, int> map)
            where TEnum : struct
        {
            if (map == null) return;
            if (!map.TryGetValue(state, out int count)) return;
            SetCurrentStep(count);
        }

        // ================= PAINT =================

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            e.Graphics.Clear(this.BackColor);

            if (_steps.Count == 0) return;

            Rectangle rc = ClientRectangle;
            rc.Inflate(-2, -2);

            AutoScaleByStepCount(rc);

            int n = _steps.Count;
            int totalGap = _gap * (n - 1);
            int stepW = (rc.Width - totalGap) / n;
            int stepH = rc.Height;

            using (var borderPen = new Pen(this.ForeColor, _border))
            using (var textBrush = new SolidBrush(this.ForeColor))
            using (var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            })
            {
                int x = rc.Left;
                
                int runningIndex = _currentStep - 1; // -1 nghĩa là không có step running (currentStep=0)
                bool hasRunning = (_currentStep >= 1 && _currentStep <= n);
                bool allDone = (_currentStep == n + 1);
                for (int i = 0; i < n; i++)
                {
                    Rectangle r = new Rectangle(x, rc.Top, stepW, stepH);
                    bool first = (i == 0);
                    bool last = (i == n - 1);

                    using (GraphicsPath path = BuildChevronPath(r, first, last, _corner, _arrowTip))
                    {
                        Color fill;

                        if (allDone)
                        {
                            fill = _doneColor; // tất cả done
                        }
                        else if (!hasRunning)
                        {
                            fill = _pendingStepColor; // currentStep=0 => tất cả pending
                        }
                        else
                        {
                            if (i < runningIndex) fill = _doneColor;
                            else if (i == runningIndex) fill = _runningColor;
                            else fill = _pendingStepColor;
                        }
                       

                        using (var fillBrush = new SolidBrush(fill))
                            e.Graphics.FillPath(fillBrush, path);

                        // Hiệu ứng progress cho step đang chạy
                        if (_enableAnimation && hasRunning && i == runningIndex)
                        {
                            DrawRunningEffect(e.Graphics, path, r);
                        }
                        e.Graphics.DrawPath(borderPen, path);

                        Rectangle textRc = r;
                        textRc.Inflate(-_fontPadding, -4);
                        e.Graphics.DrawString(_steps[i], Font, textBrush, textRc, sf);
                    }

                    x += stepW + _gap;
                }
            }
        }

        // ================= EFFECT =================

        private void DrawRunningEffect(Graphics g, GraphicsPath stepPath, Rectangle r)
        {
            GraphicsState state = g.Save();
            g.SetClip(stepPath);

            int stripeWidth = r.Width / 3;
            int x = r.X + (_animOffset % (r.Width + stripeWidth)) - stripeWidth;

            Rectangle animRect = new Rectangle(x, r.Y, stripeWidth, r.Height);

            using (var brush = new LinearGradientBrush(
                animRect,
                Color.FromArgb(60, Color.White),
                Color.FromArgb(0, Color.White),
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(brush, animRect);
            }

            g.Restore(state);
        }

        // ================= AUTO SCALE =================

        private void AutoScaleByStepCount(Rectangle rc)
        {
            int n = _steps.Count;
            if (n == 0) return;

            int totalGap = _gap * (n - 1);
            int stepW = Math.Max(40, (rc.Width - totalGap) / n);

            _arrowTip = Clamp((int)(stepW * 0.22f), 10, stepW / 2);
            _fontPadding = Clamp((int)(stepW * 0.08f), 6, 16);

            float fontSize = ClampF(stepW * 0.18f, 7.5f, 12.5f);
            if (Math.Abs(Font.Size - fontSize) > 0.1f)
                Font = new Font(Font.FontFamily, fontSize, FontStyle.Bold);
        }

        // ================= SHAPE =================

        private static GraphicsPath BuildChevronPath(
      Rectangle r, bool first, bool last, int corner, int tip)
        {
            int x = r.X, y = r.Y, w = r.Width, h = r.Height;
            int right = x + w;
            int bottom = y + h;
            int midY = y + h / 2;
            tip = Math.Max(8, Math.Min(tip, w / 2));
            corner = Math.Max(0, Math.Min(corner, h / 2));
            var path = new GraphicsPath();
            path.StartFigure();
            // ===== STEP ĐẦU =====
            if (first && !last)
            {
                path.AddArc(x, y, corner * 2, corner * 2, 180, 90);
                path.AddLine(x + corner, y, right - tip, y);
                path.AddLine(right - tip, y, right, midY);
                path.AddLine(right, midY, right - tip, bottom);
                path.AddLine(right - tip, bottom, x + corner, bottom);
                path.AddArc(x, bottom - corner * 2, corner * 2, corner * 2, 90, 90);
            }
            // ===== STEP CUỐI (BO GÓC PHẢI) =====
            else if (last && !first)
            {
                path.AddLine(x, y, right - corner, y);
                path.AddArc(right - corner * 2, y, corner * 2, corner * 2, 270, 90);
                path.AddLine(right, y + corner, right, bottom - corner);
                path.AddArc(right - corner * 2, bottom - corner * 2, corner * 2, corner * 2, 0, 90);
                path.AddLine(right - corner, bottom, x, bottom);
                path.AddLine(x, bottom, x + tip, midY);
            }
            // ===== STEP DUY NHẤT (1 step) =====
            else if (first && last)
            {
                path.AddArc(x, y, corner * 2, corner * 2, 180, 90);
                path.AddArc(right - corner * 2, y, corner * 2, corner * 2, 270, 90);
                path.AddArc(right - corner * 2, bottom - corner * 2, corner * 2, corner * 2, 0, 90);
                path.AddArc(x, bottom - corner * 2, corner * 2, corner * 2, 90, 90);
            }
            // ===== STEP GIỮA =====
            else
            {
                path.AddLine(x, y, right - tip, y);
                path.AddLine(right - tip, y, right, midY);
                path.AddLine(right, midY, right - tip, bottom);
                path.AddLine(right - tip, bottom, x, bottom);
                path.AddLine(x, bottom, x + tip, midY);
            }

            path.CloseFigure();
            return path;
        }


        // ================= UTILS =================

        private static int Clamp(int v, int min, int max)
            => v < min ? min : (v > max ? max : v);

        private static float ClampF(float v, float min, float max)
            => v < min ? min : (v > max ? max : v);
    }
}
