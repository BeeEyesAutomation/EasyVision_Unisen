using BeeCore;
using BeeGlobal;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class StatusDashboard : UserControl
    {
        // ===== Data =====
        private int _totalTimes, _okCount, _ngCount;
        private int _cycleTime, _camTime;
        private string _statusText = "---";

        // ===== Controls =====
        public RJButton btnReset;

        // ===== Layout state =====
        private int _bigW, _w1, _w2, _w3, _rightW = 100; // Big, Total(w1), OK(w2), NG(w3), RightInfo
        private bool _widthsInitialized;
        private bool _preserveProportion = true;

        // Splitter drag
        private enum DragTarget { None, S0, S1, S2, S3, SBtn }
        private DragTarget _drag = DragTarget.None;
        private int _dragStartX;
        private int _startA, _startB;
        private int _startBtn;

        // ===== Persistence =====
        [Category("Behavior"), Description("Bật/tắt lưu & khôi phục layout vào LocalAppData")]
        public bool EnableLayoutPersistence { get; set; } = true;

        [Category("Behavior"), Description("Khóa định danh layout (nếu trống sẽ dùng Name hoặc TypeName)")]
        public string PersistKey { get; set; }

        private bool _didLoadLayout;
        private bool _formHooked;
        private string _formNameCache;
        private bool _applyingLayout; // đang apply layout (đừng save/đừng reflow phá tỉ lệ)

        // ===== Appearance =====
        [Category("Data")] public int TotalTimes { get => _totalTimes; set { _totalTimes = value; Invalidate(); } }
        [Category("Data")] public int OkCount { get => _okCount; set { _okCount = value; Invalidate(); } }
        [Category("Data")] public int NgCount { get => _ngCount; set { _ngCount = value; Invalidate(); } }
        [Category("Data")] public int CycleTime { get => _cycleTime; set { _cycleTime = value; Invalidate(); } }
        [Category("Data")] public int CamTime { get => _camTime; set { _camTime = value; Invalidate(); } }
        [Category("Data")] public string StatusText { get => _statusText; set { _statusText = value; Invalidate(); } }
        [Browsable(false)] public float PercentOk => TotalTimes == 0 ? 0 : _okCount * 100f / _totalTimes;

        [Category("Appearance")] public Color StatusBlockBackColor { get; set; } = Color.Green;
        [Category("Appearance")] public Color MidHeaderBackColor { get; set; } = Color.White;
        [Category("Appearance")] public Color TotalValueBackColor { get; set; } = Color.White;
        [Category("Appearance")] public Color OkCountValueBackColor { get; set; } = Color.White;
        [Category("Appearance")] public Color NgValueBackColor { get; set; } = Color.White;
        [Category("Appearance")] public Color InfoBlockBackColor { get; set; } = Color.White;

        [Category("Appearance")] public bool AutoInfoFont { get; set; } = true;
        [Category("Appearance")] public float AutoInfoFontMin { get; set; } = 8f;
        [Category("Appearance")] public float AutoInfoFontMax { get; set; } = 48f;

        private Color _borderColor = Color.LightGray;
        private int _borderThickness = 3;

        [Category("Appearance")]
        public Color BorderLineColor { get => _borderColor; set { _borderColor = value; Invalidate(); } }

        [Category("Appearance")]
        public int BorderThickness { get => _borderThickness; set { _borderThickness = Math.Max(1, value); Invalidate(); } }

        [Category("Appearance")]
        public Font InfoFont { get; set; } = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Pixel);

        // ===== Const =====
        private const int MIN_BIG = 60;
        private const int MIN_MID = 60;
        private const int MIN_RIGHT = 80;
        private const int MIN_BTN = 48;
        private int SplitterThickness => Math.Max(1, BorderThickness);

        public StatusDashboard()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            Font = new Font("Segoe UI", 10, FontStyle.Bold);
            MinimumSize = new Size(240, 50);

            btnReset = new RJButton
            {
                Corner=Corner.Right,
                IsCLick = false,
                BorderRadius=10,
                IsNotChange = true,
                IsUnGroup = true,
               
                Text = "Reset",
                Dock = DockStyle.Right,
                Width = 64
            };
            btnReset.Click += ResetButton_Click;
            btnReset.SizeChanged += (s, e) =>
            {
                if (_applyingLayout) return; // đang load -> đừng reflow
                AdjustWidthsForDockRightChange();
            };
            Controls.Add(btnReset);

            MouseMove += StatusDashboard_MouseMove;
            MouseDown += StatusDashboard_MouseDown;
            MouseUp += StatusDashboard_MouseUp;
            SizeChanged += (s, e) => OnSelfResized();
        }

        // ===== Path (FormName + Key) -> 1 file duy nhất =====
        private string GetLayoutFilePath()
        {
            string formName = _formNameCache;
            if (string.IsNullOrEmpty(formName))
            {
                var f = FindForm();
                formName = (f != null && !string.IsNullOrWhiteSpace(f.Name)) ? f.Name : "App";
            }

            string id = !string.IsNullOrWhiteSpace(PersistKey)
                      ? PersistKey
                      : (!string.IsNullOrWhiteSpace(Name) ? Name : GetType().Name);

            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BeeInterface");
            try { if (!Directory.Exists(dir)) Directory.CreateDirectory(dir); } catch { }
            return Path.Combine(dir, $"{formName}.{id}.layout");
        }

        private void SaveLayout()
        {
            if (!EnableLayoutPersistence) return;
            try
            {
                if (!_widthsInitialized) EnsureWidths();
                File.WriteAllText(GetLayoutFilePath(),
                    $"{_bigW},{_w1},{_w2},{_w3},{_rightW},{(btnReset != null ? btnReset.Width : 64)}");
            }
            catch { }
        }
        private void SaveLayoutIfReady()
        {
            if (!_didLoadLayout || _applyingLayout) return;
            SaveLayout();
        }

        private bool LoadLayout()
        {
            if (!EnableLayoutPersistence) return false;
            try
            {
                string p = GetLayoutFilePath();
                if (!File.Exists(p)) return false;

                string[] parts = File.ReadAllText(p).Trim().Split(',');
                if (parts.Length != 6) return false;

                _applyingLayout = true;

                _bigW = Math.Max(MIN_BIG, int.Parse(parts[0]));
                _w1 = Math.Max(MIN_MID, int.Parse(parts[1]));
                _w2 = Math.Max(MIN_MID, int.Parse(parts[2]));
                _w3 = Math.Max(MIN_MID, int.Parse(parts[3]));
                _rightW = Math.Max(MIN_RIGHT, int.Parse(parts[4]));
                if (btnReset != null) btnReset.Width = Math.Max(MIN_BTN, int.Parse(parts[5]));

                _widthsInitialized = true;
                AdjustWidthsForDockRightChange(); // sẽ scale giữ tỉ lệ
                Invalidate();
                return true;
            }
            catch { return false; }
            finally { _applyingLayout = false; }
        }

        // ===== Hook Form để load/save đúng lúc =====
        private void TryAttachFormEvents()
        {
            if (_formHooked) return;
            var f = FindForm();
            if (f == null) return;

            _formHooked = true;
            _formNameCache = !string.IsNullOrWhiteSpace(f.Name) ? f.Name : "App";

            f.Shown += (s, e) =>
            {
                if (_didLoadLayout) return;
                if (!LoadLayout()) EnsureWidths();
                _didLoadLayout = true;
                Invalidate();
            };
            f.FormClosing += (s, e) => SaveLayoutIfReady();
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            TryAttachFormEvents();
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            TryAttachFormEvents();
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible && !_didLoadLayout)
            {
                if (!LoadLayout()) EnsureWidths();
                _didLoadLayout = true;
                Invalidate();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) SaveLayoutIfReady();
            base.Dispose(disposing);
        }

        // ===== Layout helpers =====
        private int RightDockedWidth()
        {
            int sum = 0;
            foreach (Control c in Controls)
                if (c.Visible && c.Dock == DockStyle.Right)
                    sum += c.Width + c.Margin.Horizontal;
            return sum;
        }

        private void EnsureWidths()
        {
            if (_widthsInitialized) return;

            int wAvail = Width - RightDockedWidth() - 4 * SplitterThickness;
            if (wAvail <= 0) return;

            _rightW = Math.Max(MIN_RIGHT, _rightW);
            int remain = Math.Max(0, wAvail - _rightW);

            // Big ~20%, OK~24, NG~16, Total nhận phần dư
            _bigW = Math.Max(MIN_BIG, (int)Math.Round(remain * 0.20));
            _w2 = Math.Max(MIN_MID, (int)Math.Round(remain * 0.24));
            _w3 = Math.Max(MIN_MID, (int)Math.Round(remain * 0.16));
            _w1 = Math.Max(MIN_MID, remain - _bigW - _w2 - _w3);

            _widthsInitialized = true;
        }

        private void RescaleAllKeepRatio(int targetWidth)
        {
            // Thứ tự: [Big, Total(w1), OK(w2), NG(w3), Right]
            int[] cur = { _bigW, _w1, _w2, _w3, _rightW };
            int[] min = { MIN_BIG, MIN_MID, MIN_MID, MIN_MID, MIN_RIGHT };
            int sum = 0; for (int i = 0; i < 5; i++) sum += cur[i];
            if (sum <= 0) return;

            // Nếu không gian khả dụng nhỏ hơn tổng min -> kẹp hết về min và thoát
            int sumMin = 0; for (int i = 0; i < 5; i++) sumMin += min[i];
            if (targetWidth <= sumMin)
            {
                _bigW = MIN_BIG; _w1 = MIN_MID; _w2 = MIN_MID; _w3 = MIN_MID; _rightW = MIN_RIGHT;
                return;
            }

            // Lặp-kẹp: phần nào chạm min thì "đóng băng" nó, scale lại các phần còn lại
            bool[] frozen = new bool[5];
            int remaining = targetWidth;

            while (true)
            {
                int freeSum = 0;
                for (int i = 0; i < 5; i++) if (!frozen[i]) freeSum += cur[i];

                if (freeSum <= 0) break; // an toàn

                double k = (double)remaining / freeSum;
                bool anyClampedThisRound = false;

                // thử scale và kẹp về min cho group chưa frozen
                for (int i = 0; i < 5; i++)
                {
                    if (frozen[i]) continue;
                    int scaled = (int)Math.Floor(cur[i] * k);
                    if (scaled < min[i])
                    {
                        // đóng băng tại min và trừ vào remaining
                        frozen[i] = true;
                        remaining -= min[i];
                        // và thay "cur[i]" thành min để vòng sau coi như cố định
                        cur[i] = min[i];
                        anyClampedThisRound = true;
                    }
                }

                if (!anyClampedThisRound)
                {
                    // không ai chạm min nữa -> chốt giá trị cho các phần còn lại
                    int[] res = new int[5];
                    double[] frac = new double[5];
                    int acc = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        if (frozen[i]) { res[i] = cur[i]; continue; }
                        double raw = cur[i] * k;
                        int v = (int)Math.Floor(raw);
                        if (v < min[i]) v = min[i]; // an toàn, thực tế không xảy ra ở nhánh này
                        res[i] = v;
                        frac[i] = raw - v;
                        acc += v;
                    }

                    // phân phối thiếu/thừa theo largest remainder
                    int diff = remaining - (acc - FrozenSum(frozen, cur));
                    if (diff > 0)
                    {
                        // ưu tiên những phần có phần thập phân lớn hơn
                        int[] idx = { 0, 1, 2, 3, 4 };
                        Array.Sort(idx, (a, b) => frac[b].CompareTo(frac[a]));
                        int p = 0;
                        while (diff-- > 0)
                        {
                            if (!frozen[idx[p]]) res[idx[p]]++;
                            p = (p + 1) % idx.Length;
                        }
                    }
                    else if (diff < 0)
                    {
                        diff = -diff;
                        // bớt từ phần có "slack" lớn nhất (res[i]-min[i])
                        while (diff > 0)
                        {
                            int best = -1, bestSlack = 0;
                            for (int i = 0; i < 5; i++)
                            {
                                int slack = res[i] - min[i];
                                if (slack > bestSlack) { bestSlack = slack; best = i; }
                            }
                            if (best == -1) break;
                            int take = Math.Min(bestSlack, diff);
                            res[best] -= take;
                            diff -= take;
                        }
                    }

                    // apply
                    _bigW = res[0];
                    _w1 = res[1];
                    _w2 = res[2];
                    _w3 = res[3];
                    _rightW = res[4];
                    return;
                }
            }

            // nếu thoát vòng lặp do mọi phần đã frozen (hiếm)
            _bigW = cur[0]; _w1 = cur[1]; _w2 = cur[2]; _w3 = cur[3]; _rightW = cur[4];

            int FrozenSum(bool[] fr, int[] val)
            {
                int s = 0; for (int i = 0; i < fr.Length; i++) if (fr[i]) s += val[i]; return s;
            }
        }

        private void AdjustWidthsForDockRightChange()
        {
            if (!_widthsInitialized) return;

            int wAvail = Width - RightDockedWidth() - 4 * SplitterThickness;
            if (wAvail <= 0) return;

            int sum = _bigW + _w1 + _w2 + _w3 + _rightW;
            int diff = wAvail - sum;
            if (diff == 0) return;

            if (_applyingLayout)
            {
                // Khi đang load/áp: giữ tỉ lệ cũ
               // RescaleAllKeepRatio(wAvail);
            }
            else
            {
                // Tương tác runtime: dồn diff để Total (w1) "fill full"
                GrowShrink(ref _w1, MIN_MID, diff, out diff);
                if (diff != 0) GrowShrink(ref _w2, MIN_MID, diff, out diff);
                if (diff != 0) GrowShrink(ref _w3, MIN_MID, diff, out diff);
                if (diff != 0) GrowShrink(ref _bigW, MIN_BIG, diff, out diff);
            }
            Invalidate();
        }

        private static void GrowShrink(ref int w, int min, int delta, out int remain)
        {
            int target = w + delta;
            if (target < min) { remain = target - min; w = min; }
            else { w = target; remain = 0; }
        }

        private void OnSelfResized()
        {
            if (!_widthsInitialized) { EnsureWidths(); return; }

            int wAvail = Width - RightDockedWidth() - 4 * SplitterThickness;
            if (wAvail <= 0) return;

            if (_preserveProportion)
            {
                int oldTotal = _bigW + _w1 + _w2 + _w3 + _rightW;
                if (oldTotal <= 0) oldTotal = 1;
                float k = wAvail / (float)oldTotal;

                _bigW = Math.Max(MIN_BIG, (int)Math.Round(_bigW * k));
                _w1 = Math.Max(MIN_MID, (int)Math.Round(_w1 * k));
                _w2 = Math.Max(MIN_MID, (int)Math.Round(_w2 * k));
                _w3 = Math.Max(MIN_MID, (int)Math.Round(_w3 * k));

                int diff = wAvail - (_bigW + _w1 + _w2 + _w3 + _rightW);
                GrowShrink(ref _w1, MIN_MID, diff, out diff);
                if (diff != 0) GrowShrink(ref _w2, MIN_MID, diff, out diff);
                if (diff != 0) GrowShrink(ref _w3, MIN_MID, diff, out diff);
                if (diff != 0) GrowShrink(ref _bigW, MIN_BIG, diff, out diff);
            }
            else
            {
                AdjustWidthsForDockRightChange();
            }
            Invalidate();
        }

        private Rectangle[] BuildRects(out Rectangle[] splits, out Rectangle sBtn)
        {
            EnsureWidths();

            int btnDocked = RightDockedWidth();
            int w = Width - btnDocked;
            int h = Height;

            Rectangle rBig = new Rectangle(0, 0, _bigW, h);
            Rectangle r1 = new Rectangle(rBig.Right + SplitterThickness, 0, _w1, h);
            Rectangle r2 = new Rectangle(r1.Right + SplitterThickness, 0, _w2, h);
            Rectangle r3 = new Rectangle(r2.Right + SplitterThickness, 0, _w3, h);
            Rectangle rR = new Rectangle(r3.Right + SplitterThickness, 0, _rightW, h);

            Rectangle s0 = new Rectangle(rBig.Right, 0, SplitterThickness, h);
            Rectangle s1 = new Rectangle(r1.Right, 0, SplitterThickness, h);
            Rectangle s2 = new Rectangle(r2.Right, 0, SplitterThickness, h);
            Rectangle s3 = new Rectangle(r3.Right, 0, SplitterThickness, h);

            int btnLeft = (btnReset != null) ? btnReset.Left : Width - btnDocked;
            sBtn = new Rectangle(btnLeft - SplitterThickness, 0, SplitterThickness, h);

            splits = new Rectangle[] { s0, s1, s2, s3 };
            return new Rectangle[] { rBig, r1, r2, r3, rR };
        }

        private DragTarget HitSplitter(Point p, Rectangle[] splits, Rectangle sBtn)
        {
            if (splits[0].Contains(p)) return DragTarget.S0;
            if (splits[1].Contains(p)) return DragTarget.S1;
            if (splits[2].Contains(p)) return DragTarget.S2;
            if (splits[3].Contains(p)) return DragTarget.S3;
            if (sBtn.Contains(p)) return DragTarget.SBtn;
            return DragTarget.None;
        }

        // ===== Mouse (splitter) =====
        private void StatusDashboard_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle[] splits; Rectangle sBtn;
            BuildRects(out splits, out sBtn);

            if (_drag == DragTarget.None)
            {
                var hit = HitSplitter(e.Location, splits, sBtn);
                Cursor = (hit == DragTarget.None) ? Cursors.Default : Cursors.VSplit;
                return;
            }

            int dx = e.X - _dragStartX;

            switch (_drag)
            {
                case DragTarget.S0:
                    _bigW = Clamp(_startA + dx, MIN_BIG);
                    _w1 = Clamp(_startB - dx, MIN_MID);
                    break;
                case DragTarget.S1:
                    _w1 = Clamp(_startA + dx, MIN_MID);
                    _w2 = Clamp(_startB - dx, MIN_MID);
                    break;
                case DragTarget.S2:
                    _w2 = Clamp(_startA + dx, MIN_MID);
                    _w3 = Clamp(_startB - dx, MIN_MID);
                    break;
                case DragTarget.S3:
                    _w3 = Clamp(_startA + dx, MIN_MID);
                    _rightW = Clamp(_startB - dx, MIN_RIGHT);
                    break;
                case DragTarget.SBtn:
                    {
                        int desiredBtn = Clamp(_startBtn - dx, MIN_BTN);
                        int deltaBtn = desiredBtn - _startBtn;
                        int desiredRight = Clamp(_startB - deltaBtn, MIN_RIGHT);

                        if (desiredRight == MIN_RIGHT && (_startB - deltaBtn) < MIN_RIGHT)
                        {
                            int over = MIN_RIGHT - (_startB - deltaBtn);
                            desiredBtn = Clamp(desiredBtn + over, MIN_BTN);
                            desiredRight = MIN_RIGHT;
                        }
                        btnReset.Width = desiredBtn;
                        _rightW = desiredRight;
                        AdjustWidthsForDockRightChange();
                        break;
                    }
            }
            Invalidate();
        }

        private void StatusDashboard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Rectangle[] splits; Rectangle sBtn;
            BuildRects(out splits, out sBtn);
            var hit = HitSplitter(e.Location, splits, sBtn);
            if (hit == DragTarget.None) return;

            _drag = hit;
            _dragStartX = e.X;

            switch (hit)
            {
                case DragTarget.S0: _startA = _bigW; _startB = _w1; break;
                case DragTarget.S1: _startA = _w1; _startB = _w2; break;
                case DragTarget.S2: _startA = _w2; _startB = _w3; break;
                case DragTarget.S3: _startA = _w3; _startB = _rightW; break;
                case DragTarget.SBtn:
                    _startBtn = btnReset.Width;
                    _startB = _rightW;
                    break;
            }
            Capture = true;
        }

        private void StatusDashboard_MouseUp(object sender, MouseEventArgs e)
        {
            if (_drag != DragTarget.None)
            {
                _drag = DragTarget.None;
                Capture = false;
                Cursor = Cursors.Default;
                SaveLayoutIfReady();
            }
        }

        private static int Clamp(int v, int min) => v < min ? min : v;

        // ===== Drawing =====
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle[] splits; Rectangle sBtn;
            var rects = BuildRects(out splits, out sBtn);
            if (rects.Length == 0) return;

            var rBig = rects[0];
            var r1 = rects[1];
            var r2 = rects[2];
            var r3 = rects[3];
            var rR = rects[4];

            DrawBlock(g, rBig, StatusBlockBackColor, BorderLineColor, StatusText, null, Color.White);

            string[] headers = { "Total Times", "OK", "NG" };
            string[] values = { _totalTimes.ToString(), _okCount.ToString(), _ngCount.ToString() };
            Color[] valueBgs = { TotalValueBackColor, OkCountValueBackColor, NgValueBackColor };
            Color[] fgCols = { Color.Blue, Color.Green, Color.Red };
            var rectMids = new[] { r1, r2, r3 };
            for (int i = 0; i < 3; i++)
                DrawMidBlock(g, rectMids[i], headers[i], values[i], valueBgs[i], fgCols[i]);

            DrawRightInfo(g, rR);

            using (var br = new SolidBrush(BorderLineColor))
            {
                foreach (var s in splits) g.FillRectangle(br, s);
                g.FillRectangle(br, sBtn);
            }
        }

        private void DrawMidBlock(Graphics g, Rectangle r, string header, string value, Color valueBackColor, Color valColor)
        {
            int headerH = (int)(r.Height * 0.3f);
            var rH = new Rectangle(r.X, r.Y, r.Width, headerH);
            var rC = new Rectangle(r.X, r.Y + headerH, r.Width, r.Height - headerH);

            using (var bgH = new SolidBrush(MidHeaderBackColor)) g.FillRectangle(bgH, rH);
            using (var bgC = new SolidBrush(valueBackColor)) g.FillRectangle(bgC, rC);

            using (var pen = new Pen(BorderLineColor, BorderThickness))
            {
                g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
                g.DrawLine(pen, r.X, rH.Bottom, r.Right - 1, rH.Bottom);
            }

            using (var fH = GetFittingFont(g, header, Font.FontFamily, FontStyle.Bold, rH.Size))
            using (var sfH = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString(header, fH, Brushes.Black, rH, sfH);

            using (var fV = GetFittingFont(g, value, Font.FontFamily, FontStyle.Bold, rC.Size))
            using (var sfV = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var fg = new SolidBrush(valColor))
                g.DrawString(value, fV, fg, rC, sfV);
        }

        private void DrawBlock(Graphics g, Rectangle r, Color backColor, Color borderColor, string text, Font fontOverride, Color textColor)
        {
            using (var bg = new SolidBrush(backColor)) g.FillRectangle(bg, r);
            using (var pen = new Pen(borderColor, BorderThickness)) g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
            var f = fontOverride ?? GetFittingFont(g, text, Font.FontFamily, FontStyle.Bold, r.Size);
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var br = new SolidBrush(textColor))
                g.DrawString(text, f, br, r, sf);
        }

        private void DrawRightInfo(Graphics g, Rectangle r)
        {
            using (var bg = new SolidBrush(InfoBlockBackColor)) g.FillRectangle(bg, r);
         //   using (var pen = new Pen(BorderLineColor, BorderThickness)) g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);

            int pad = 4;
            var rIn = new Rectangle(r.X + pad, r.Y + pad, r.Width - pad * 2, r.Height - pad * 2);
            var lines = new[]
            {
                $"CT      {_cycleTime} ms",
                $"CT cam  {_camTime} ms",
                $"% OK    {PercentOk:0.0} %"
            };

            var f = AutoInfoFont ? GetAutoInfoAutoFont(g, rIn, lines) : InfoFont;
            bool temp = AutoInfoFont;

            try
            {
                Size ag = TextRenderer.MeasureText(g, "Ag", f, new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                int lineH = ag.Height;
                int totalH = lineH * lines.Length;
                int top = rIn.Y + Math.Max(0, (rIn.Height - totalH) / 2);

                for (int i = 0; i < lines.Length; i++)
                {
                    var cell = new Rectangle(rIn.X, top + i * lineH, rIn.Width, lineH);
                    using (var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                        g.DrawString(lines[i], f, Brushes.Black, cell, sf);
                }
            }
            finally { if (temp && f != null) f.Dispose(); }
        }

        // ===== Fonts =====
        private Font GetFittingFont(Graphics g, string text, FontFamily fam, FontStyle style, Size bounding)
        {
            float em = bounding.Height;
            for (; em > 4f; em -= 0.5f)
            {
                using (var f = new Font(fam, em, style, GraphicsUnit.Pixel))
                {
                    SizeF sz = g.MeasureString(text, f);
                    if (sz.Width <= bounding.Width && sz.Height <= bounding.Height)
                        return new Font(fam, em, style, GraphicsUnit.Pixel);
                }
            }
            return new Font(fam, 4f, style, GraphicsUnit.Pixel);
        }

        private Font GetAutoInfoAutoFont(Graphics g, Rectangle box, string[] lines)
        {
            Font baseFont = (InfoFont ?? Font);
            FontFamily fam = baseFont.FontFamily;
            FontStyle style = baseFont.Style;

            float hi = Math.Max(AutoInfoFontMin, Math.Min(AutoInfoFontMax, box.Height / Math.Max(1f, lines.Length)));
            float lo = AutoInfoFontMin, best = lo;

            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;

            while (hi - lo > 0.5f)
            {
                float mid = (lo + hi) / 2f;
                using (var f = new Font(fam, mid, style, GraphicsUnit.Pixel))
                {
                    int maxW = 0, lineH = 0;
                    foreach (var s in lines)
                    {
                        Size sz = TextRenderer.MeasureText(g, s, f, new Size(int.MaxValue, int.MaxValue), flags);
                        if (sz.Width > maxW) maxW = sz.Width;
                        if (sz.Height > lineH) lineH = sz.Height;
                    }
                    bool fits = (maxW <= box.Width) && (lineH * lines.Length <= box.Height);
                    if (fits) { best = mid; lo = mid; } else hi = mid;
                }
            }
            return new Font(fam, best, style, GraphicsUnit.Pixel);
        }

        // ===== Reset data =====
        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn Xóa dữ liệu Sản Xuất",
                "Xóa Tất cả dữ liệu hôm nay", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            TotalTimes = 0; OkCount = 0; NgCount = 0; CycleTime = 0; CamTime = 0;
            Global.Config.SumOK = 0;
            Global.Config.SumNG = 0;
            Global.Config.TotalTime = 0;
            Global.NumSend = 0;
            try
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                string mdf = Path.Combine(Environment.CurrentDirectory, "Report", date + ".mdf");
                if (!File.Exists(mdf)) return;
                using (var con = new SqlConnection())
                {
                    // TODO: xử lý DB nếu cần
                }
            }
            catch { }
            Refresh();
        }
    }
}
