// DashboardImages_Full_WithCtrlR.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BeeInterface
{
    // ==== Model cho từng ảnh ====
    public class ImageItem
    {
        public string Path;
        public string Caption;
        public float Zoom = 1.0f;           // hệ số zoom
        public PointF Pan = PointF.Empty;    // tịnh tiến sau zoom
        public Size OrigSize;                // kích thước gốc (cache)
        public Rectangle Bounds;             // block item
        public Rectangle ImgRect;            // vùng vẽ ảnh thực tế
    }

    public enum FilterMode { All = 0, NewestAny = 1, NewestOK = 2, NewestNG = 3 }

    public class DashboardImages : Control
    {
        // ===== Top UI =====
        private readonly Panel _top = new Panel { Height = 40, Dock = DockStyle.Top, Padding = new Padding(8, 6, 8, 6) };
        private readonly ComboBox _cbDate = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 80, Height = 28, Font = new Font("Arial", 12),Dock = DockStyle.Left };
        private readonly RadioButton _rbRaw = new RadioButton { Text = "Raw", AutoSize = true };
        private readonly RadioButton _rbResult = new RadioButton { Text = "Result", AutoSize = true };
        private readonly ComboBox _cbFilter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100, Dock = DockStyle.Right ,Font=new Font("Arial",12) };
        private readonly Button _btnRefresh = new Button { Text = "Refresh", AutoSize = false, Width =75, Height = 24, Dock = DockStyle.Right };

        // ===== Bottom UI =====
        private readonly Panel _bottom = new Panel { Height = 36, Dock = DockStyle.Bottom, Padding = new Padding(8, 5, 8, 5) };
        private readonly Button _btnPrev = new Button { Text = "◀ Back", AutoSize = true , Dock = DockStyle.Left };
        private readonly Button _btnNext = new Button { Text = "Next ▶", AutoSize = true, Dock = DockStyle.Left };
        private readonly Label _lblPage = new Label { AutoSize = true, ForeColor = Color.Black ,Font = new Font("Arial", 12) ,TextAlign=ContentAlignment.MiddleRight, Dock = DockStyle.Fill };

        // ===== Scroll & Data =====
        private readonly VScrollBar _vScroll = new VScrollBar();
        private readonly List<string> _allFiles = new List<string>();
        private readonly List<ImageItem> _items = new List<ImageItem>();

        // Cache ảnh
        private readonly Dictionary<string, Image> _cache = new Dictionary<string, Image>();
        private readonly Queue<string> _cacheQueue = new Queue<string>();
        private int _cacheSize = 1;

        // Config
        private const int PageSize = 10;
        private string _rootFolder = @"Report";
        private string _lastSelectedDate;

        private int _currentPage = 0;
        private int _totalPages = 0;

        private ImageItem _activeItem;
        private Point _lastMouse;

        // Padding mặc định cho mỗi item
        private const int ItemTopPad = 10;
        private const int ItemBottomPad = 10;

        public DashboardImages()
        {
            DoubleBuffered = true;
            BackColor = SystemColors.Control;
            TabStop = true; // để nhận bàn phím

            // Compose UI
            Controls.Add(_top);
            Controls.Add(_bottom);

            _vScroll.Dock = DockStyle.Right;
            _vScroll.Scroll += (s, e) => Invalidate();
            Controls.Add(_vScroll);

            // Top: thứ tự để Dock=Right sắp bên phải
            _top.Controls.Add(_btnRefresh);
            _top.Controls.Add(_cbFilter);
            _top.Controls.AddRange(new Control[] { _cbDate, _rbRaw, _rbResult });
            _rbResult.Checked = true;

            // Bottom
            _bottom.Controls.AddRange(new Control[] { _btnPrev, _btnNext, _lblPage });
            _btnPrev.Left = _bottom.Padding.Left;
            _btnPrev.Top = _bottom.Padding.Top;
            _btnNext.Left = _btnPrev.Right + 8;
            _btnNext.Top = _bottom.Padding.Top;
            _lblPage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Filter items
            _cbFilter.Items.AddRange(new object[] { "All", "Newest", "Newest OK", "Newest NG" });
            _cbFilter.SelectedIndex = 0;

            // Events
            _btnRefresh.Click += (s, e) => RefreshDates();
            _cbDate.SelectedIndexChanged += (s, e) => { _lastSelectedDate = _cbDate.SelectedItem?.ToString(); ReloadAccordingToUI(); };
            _rbRaw.CheckedChanged += (s, e) => { if (_rbRaw.Checked) ReloadAccordingToUI(); };
            _rbResult.CheckedChanged += (s, e) => { if (_rbResult.Checked) ReloadAccordingToUI(); };
            _cbFilter.SelectedIndexChanged += (s, e) => ReloadAccordingToUI();

            _btnPrev.Click += (s, e) => { if (_currentPage > 0) { _currentPage--; LoadPage(); } };
            _btnNext.Click += (s, e) => { if (_currentPage < _totalPages - 1) { _currentPage++; LoadPage(); } };

            MouseWheel += Dashboard_MouseWheel;
            MouseDown += Dashboard_MouseDown;
            MouseMove += Dashboard_MouseMove;
            MouseDoubleClick += Dashboard_MouseDoubleClick; // reset zoom/pan

            _top.Resize += Top_Resize;
            _bottom.Resize += Bottom_Resize;

            // First run
            RefreshDates();
            Top_Resize(null, EventArgs.Empty);
            Bottom_Resize(null, EventArgs.Empty);
        }

        // ===== HOTKEY: Ctrl+R =====
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.R))
            {
                RefreshDates(); // giống nút Refresh
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ===== Public =====
        public void SetRoot(string rootFolder)
        {
            _rootFolder = rootFolder ?? "";
            RefreshDates();
        }

        // ===== Data =====
        private void RefreshDates()
        {
            var rx = new Regex(@"^\d{8}$");
            var allDates = Directory.Exists(_rootFolder)
                ? Directory.GetDirectories(_rootFolder)
                           .Select(p => new DirectoryInfo(p))
                           .Where(di => rx.IsMatch(di.Name))
                           .Select(di => di.Name)
                           .OrderByDescending(n => n)
                           .ToArray()
                : Array.Empty<string>();

            var remember = _lastSelectedDate;
            _cbDate.BeginUpdate();
            _cbDate.Items.Clear();
            _cbDate.Items.AddRange(allDates);
            _cbDate.EndUpdate();

            if (!string.IsNullOrEmpty(remember) && _cbDate.Items.Contains(remember))
                _cbDate.SelectedItem = remember;
            else if (_cbDate.Items.Count > 0)
                _cbDate.SelectedIndex = 0;

            ReloadAccordingToUI();
        }

        private string CurrentDate => _cbDate.SelectedItem?.ToString();
        private bool UseRaw => _rbRaw.Checked;
        private FilterMode CurrentFilter => (FilterMode)_cbFilter.SelectedIndex;

        private void ReloadAccordingToUI()
        {
            if (string.IsNullOrEmpty(CurrentDate))
            {
                ClearAll();
                return;
            }

            BuildFileList(CurrentDate, UseRaw, CurrentFilter);
            _currentPage = 0;
            _totalPages = (CurrentFilter == FilterMode.All)
                ? Math.Max(1, (int)Math.Ceiling(_allFiles.Count / (float)PageSize))
                : 1;

            LoadPage();
        }

        private void ClearAll()
        {
            ClearCache();
            _allFiles.Clear();
            _items.Clear();
            _currentPage = 0;
            _totalPages = 0;
            _lblPage.Text = "";
            Invalidate();
        }

        private void BuildFileList(string dateFolder, bool useRaw, FilterMode mode)
        {
            ClearCache();
            _allFiles.Clear();
            _items.Clear();

            var type = useRaw ? "Raw" : "Result";
            var path = Path.Combine(_rootFolder ?? "", dateFolder ?? "", type);
            if (!Directory.Exists(path)) return;

            var exts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff", ".webp" };

            var all = Directory.EnumerateFiles(path)
                               .Where(f => exts.Contains(Path.GetExtension(f)))
                               .OrderByDescending(f => File.GetLastWriteTimeUtc(f));

            switch (mode)
            {
                case FilterMode.All: _allFiles.AddRange(all); break;
                case FilterMode.NewestAny: { var x = all.FirstOrDefault(); if (x != null) _allFiles.Add(x); } break;
                case FilterMode.NewestOK: { var x = all.FirstOrDefault(f => Path.GetFileName(f).IndexOf("OK", StringComparison.OrdinalIgnoreCase) >= 0); if (x != null) _allFiles.Add(x); } break;
                case FilterMode.NewestNG: { var x = all.FirstOrDefault(f => Path.GetFileName(f).IndexOf("NG", StringComparison.OrdinalIgnoreCase) >= 0); if (x != null) _allFiles.Add(x); } break;
            }
        }

        private void LoadPage()
        {
            _items.Clear();

            if (CurrentFilter == FilterMode.All)
            {
                int start = _currentPage * PageSize;
                int end = Math.Min(start + PageSize, _allFiles.Count);
                for (int i = start; i < end; i++)
                    _items.Add(new ImageItem { Path = _allFiles[i], Caption = Path.GetFileName(_allFiles[i]) });
            }
            else
            {
                if (_allFiles.Count > 0)
                    _items.Add(new ImageItem { Path = _allFiles[0], Caption = Path.GetFileName(_allFiles[0]) });
            }

            UpdateLayout();
            UpdatePageLabel();
        }

        private void UpdatePageLabel()
        {
            if (CurrentFilter == FilterMode.All)
                _lblPage.Text = $"Page {_currentPage + 1}/{Math.Max(1, _totalPages)}  •  {_allFiles.Count} imgs";
            else
                _lblPage.Text = $"{CurrentFilter}  •  {(_allFiles.Count > 0 ? 1 : 0)} img";

            Bottom_Resize(null, EventArgs.Empty);
        }

        // ===== Layout (pad động theo Pan.Y để không chồng lấn) =====
        private void UpdateLayout()
        {
            int y = _top.Bottom + 6;
            int rightMargin = _vScroll.Width + 10; // né vscroll +10
            int frameW = Math.Max(50, ClientSize.Width - rightMargin);

            foreach (var it in _items)
            {
                if (it.OrigSize.IsEmpty)
                {
                    try { using (var img = Image.FromFile(it.Path)) it.OrigSize = img.Size; }
                    catch { it.OrigSize = new Size(100, 100); }
                }

                float baseScale = (float)frameW / it.OrigSize.Width;
                float drawW = it.OrigSize.Width * baseScale * it.Zoom;
                float drawH = it.OrigSize.Height * baseScale * it.Zoom;

                int left = (int)((frameW - drawW) / 2f + it.Pan.X);

                float topPadDynamic = ItemTopPad + Math.Max(0f, -it.Pan.Y); // pan lên (âm)
                float bottomPadDynamic = ItemBottomPad + Math.Max(0f, it.Pan.Y); // pan xuống (dương)

                int totalH = (int)Math.Ceiling(topPadDynamic + drawH + bottomPadDynamic);
                it.Bounds = new Rectangle(0, y, frameW, totalH);

                int top = (int)(y + topPadDynamic);
                it.ImgRect = new Rectangle(left, top, (int)drawW, (int)drawH);

                y += totalH + 10;
            }

            int contentHeight = Math.Max(y - _top.Bottom, 0);
            int viewport = Math.Max(0, ClientSize.Height - _top.Height - _bottom.Height);
            _vScroll.Maximum = Math.Max(0, contentHeight - viewport);
            _vScroll.LargeChange = viewport;

            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateLayout();
        }

        // Top: Date fill; Radio cố định trước Refresh
        private void Top_Resize(object sender, EventArgs e)
        {
            int leftEdge = _top.Padding.Left;
            int rightEdge = _btnRefresh.Left - 8;

            _rbResult.Top = _top.Padding.Top + 3;
            _rbResult.Left = rightEdge - _rbResult.Width;

            _rbRaw.Top = _top.Padding.Top + 3;
            _rbRaw.Left = _rbResult.Left - 12 - _rbRaw.Width;

            int dateLeft = leftEdge;
            int dateRight = _rbRaw.Left - 12;
            int dateWidth = Math.Max(120, dateRight - dateLeft);

            _cbDate.Left = dateLeft;
            _cbDate.Top = _top.Padding.Top;
            _cbDate.Width = dateWidth;
            _cbDate.Height = 28;
        }

        private void Bottom_Resize(object sender, EventArgs e)
        {
            _lblPage.Left = _bottom.ClientSize.Width - _lblPage.Width - _bottom.Padding.Right;
            _lblPage.Top = _bottom.Padding.Top + 2;
        }

        // ===== Paint (DrawString để bám transform/scroll; text scale theo zoom) =====
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            e.Graphics.TranslateTransform(0, -_vScroll.Value);

            foreach (var it in _items)
            {
                if (!it.ImgRect.IsEmpty)
                    DrawImageCached(e.Graphics, it.Path, it.ImgRect);

                using (var p = new Pen(Color.FromArgb(70, 70, 70)))
                    e.Graphics.DrawRectangle(p, it.ImgRect);

                // Tên file: góc phải-dưới (scale 10–24pt)
                float namePt = Math.Max(10f, Math.Min(24f, 14f * it.Zoom));
                using (var nameFont = new Font("Segoe UI", namePt, FontStyle.Bold))
                using (var white = new SolidBrush(Color.White))
                using (var shadow = new SolidBrush(Color.FromArgb(90, 0, 0, 0)))
                {
                    SizeF nameSize = e.Graphics.MeasureString(it.Caption, nameFont);
                    float nx = it.ImgRect.Right - nameSize.Width - 10;
                    float ny = it.ImgRect.Bottom - nameSize.Height - 8;
                    e.Graphics.FillRectangle(shadow, new RectangleF(nx - 4, ny - 2, nameSize.Width + 8, nameSize.Height + 4));
                    e.Graphics.DrawString(it.Caption, nameFont, white, nx, ny);
                }

                // OK/NG: góc phải-trên (scale 18–64pt)
                string status = it.Caption.IndexOf("NG", StringComparison.OrdinalIgnoreCase) >= 0 ? "NG" :
                                (it.Caption.IndexOf("OK", StringComparison.OrdinalIgnoreCase) >= 0 ? "OK" : "");
                if (!string.IsNullOrEmpty(status))
                {
                    float statusPt = Math.Max(18f, Math.Min(64f, 30f * it.Zoom));
                    using (var big = new Font("Segoe UI", statusPt, FontStyle.Bold))
                    {
                        SizeF s = e.Graphics.MeasureString(status, big);
                        float sx = it.ImgRect.Right - s.Width - 10;
                        float sy = it.ImgRect.Top + 6;
                        using (var brush = (status == "OK") ? new SolidBrush(Color.LimeGreen) : new SolidBrush(Color.Red))
                            e.Graphics.DrawString(status, big, brush, sx, sy);
                    }
                }
            }

            e.Graphics.ResetTransform();
        }

        // ===== Image cache =====
        private void DrawImageCached(Graphics g, string path, Rectangle dest)
        {
            if (!_cache.TryGetValue(path, out var img))
            {
                try
                {
                    img = Image.FromFile(path);
                    _cache[path] = img;
                    _cacheQueue.Enqueue(path);

                    if (_cacheQueue.Count > _cacheSize)
                    {
                        var old = _cacheQueue.Dequeue();
                        if (_cache.TryGetValue(old, out var oimg))
                        {
                            oimg.Dispose();
                            _cache.Remove(old);
                        }
                    }
                }
                catch
                {
                    using (var br = new SolidBrush(Color.FromArgb(200, 200, 200)))
                        g.FillRectangle(br, dest);
                    using (var txt = new SolidBrush(Color.OrangeRed))
                    using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                        g.DrawString("Cannot open image", Font, txt, dest, fmt);
                    return;
                }
            }

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.DrawImage(img, dest);
        }

        private void ClearCache()
        {
            foreach (var kv in _cache) kv.Value.Dispose();
            _cache.Clear();
            _cacheQueue.Clear();
        }

        // ===== Interaction =====
        private void Dashboard_MouseWheel(object sender, MouseEventArgs e)
        {
            bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;

            if (!ctrl || _activeItem == null)
            {
                int step = SystemInformation.MouseWheelScrollLines * 30;
                _vScroll.Value = Math.Max(_vScroll.Minimum,
                                  Math.Min(_vScroll.Maximum, _vScroll.Value - Math.Sign(e.Delta) * step));
                Invalidate();
                return;
            }

            // Zoom theo tâm ảnh
            AdjustZoomCenter(_activeItem, e.Delta > 0 ? 1.1f : 1f / 1.1f);
        }

        private void AdjustZoomCenter(ImageItem it, float factor)
        {
            if (it.OrigSize.IsEmpty) return;

            int rightMargin = _vScroll.Width + 10;
            int frameW = Math.Max(50, ClientSize.Width - rightMargin);
            float baseScale = (float)frameW / it.OrigSize.Width;

            float oldDrawW = it.OrigSize.Width * baseScale * it.Zoom;
            float oldDrawH = it.OrigSize.Height * baseScale * it.Zoom;
            float oldLeft = (frameW - oldDrawW) / 2f + it.Pan.X;
            float oldTop = ItemTopPad + it.Pan.Y;

            float oldCx = oldLeft + oldDrawW / 2f;
            float oldCy = oldTop + oldDrawH / 2f;

            float newZoom = Math.Max(0.05f, Math.Min(20f, it.Zoom * factor));
            float newDrawW = it.OrigSize.Width * baseScale * newZoom;
            float newDrawH = it.OrigSize.Height * baseScale * newZoom;
            float newLeft = (frameW - newDrawW) / 2f + it.Pan.X;
            float newTop = ItemTopPad + it.Pan.Y;

            float newCx = newLeft + newDrawW / 2f;
            float newCy = newTop + newDrawH / 2f;

            it.Pan = new PointF(it.Pan.X + (oldCx - newCx), it.Pan.Y + (oldCy - newCy));
            it.Zoom = newZoom;

            UpdateLayout(); // tính lại ngay để không chồng lấn
        }

        private void Dashboard_MouseDown(object sender, MouseEventArgs e)
        {
            Focus(); // đảm bảo control nhận bàn phím (Ctrl+R)
            _lastMouse = e.Location;
            _activeItem = null;

            foreach (var it in _items)
            {
                var r = it.Bounds; r.Y -= _vScroll.Value;
                if (!r.Contains(e.Location)) continue;
                _activeItem = it;
                break;
            }
        }

        private void Dashboard_MouseMove(object sender, MouseEventArgs e)
        {
            if (_activeItem != null && e.Button == MouseButtons.Left)
            {
                _activeItem.Pan = new PointF(
                    _activeItem.Pan.X + (e.X - _lastMouse.X),
                    _activeItem.Pan.Y + (e.Y - _lastMouse.Y)
                );
                _lastMouse = e.Location;
                UpdateLayout(); // cập nhật layout ngay để ImgRect/Bounds mới, tránh chồng lấn
            }
        }

        // Double-click: reset ảnh đang chọn
        private void Dashboard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (var it in _items)
            {
                var r = it.Bounds; r.Y -= _vScroll.Value;
                if (!r.Contains(e.Location)) continue;

                it.Zoom = 1.0f;
                it.Pan = PointF.Empty;
                UpdateLayout();
                break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var kv in _cache) kv.Value.Dispose();
                _cache.Clear();
            }
            base.Dispose(disposing);
        }
    }

  
}
