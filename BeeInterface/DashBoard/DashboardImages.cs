// DashboardImages_TableLayout.cs — TableLayoutPanel top/bottom
// Tối ưu: KHÔNG block UI thread khi I/O, preload size header, decode full-res nền ưu tiên thấp, throttling + cancel theo trang
// Chế độ "blocking page": chỉ cho scroll/interaction sau khi decode full-res xong TẤT CẢ ảnh của trang
// C# 7.3 compatible
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;

namespace BeeInterface
{
    // ==== Model cho từng ảnh ====
    public class ImageItem
    {
        public string Path;
        public string Caption;
        public float Zoom = 1.0f;
        public PointF Pan = PointF.Empty;
        public Size OrigSize;       // kích thước gốc (từ header — không decode full)
        public Rectangle Bounds;    // block
        public Rectangle ImgRect;   // vùng vẽ ảnh
    }

    public enum FilterMode { All = 0, NewestAny = 1, OnlyOK = 2, OnlyNG = 3 }

    public class DashboardImages : Control
    {
        // Sort mode
        private enum SortMode { TimeDesc = 0, TimeAsc = 1, NameAsc = 2, NameDesc = 3 }

        // Cue text cho TextBox
        const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        // ==== Shell thumbnail (IShellItemImageFactory) ====
        [Flags]
        private enum SIIGBF : uint
        {
            SIIGBF_RESIZETOFIT = 0x00,
            SIIGBF_BIGGERSIZEOK = 0x01,
            SIIGBF_MEMORYONLY = 0x02,
            SIIGBF_ICONONLY = 0x04,
            SIIGBF_THUMBNAILONLY = 0x08,
            SIIGBF_INCACHEONLY = 0x10
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct SIZE { public int cx; public int cy; }
        [ComImport, Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItemImageFactory { void GetImage(SIZE size, SIIGBF flags, out IntPtr hbitmap); }
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        [DllImport("gdi32.dll")] private static extern bool DeleteObject(IntPtr hObject);
        private static Bitmap GetShellThumbnail(string path, int width, int height)
        {
            try
            {
                object obj;
                SHCreateItemFromParsingName(path, IntPtr.Zero, typeof(IShellItemImageFactory).GUID, out obj);
                var factory = (IShellItemImageFactory)obj;
                var size = new SIZE { cx = Math.Max(16, width), cy = Math.Max(16, height) };
                factory.GetImage(size, SIIGBF.SIIGBF_THUMBNAILONLY | SIIGBF.SIIGBF_RESIZETOFIT | SIIGBF.SIIGBF_INCACHEONLY, out IntPtr hbmp);
                if (hbmp == IntPtr.Zero) return null;
                var bmp = Image.FromHbitmap(hbmp);
                DeleteObject(hbmp);
                return (Bitmap)bmp;
            }
            catch { return null; }
        }

        // ====== TOP: TableLayoutPanel (3 rows) ======
        private readonly TableLayoutPanel _topTL = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Padding = new Padding(8, 6, 8, 6),
            ColumnCount = 1,
            RowCount = 3,
        };

        // Row controls
        private readonly ComboBox _cbDate = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Arial", 14), Dock = DockStyle.Fill };
        private readonly RadioButton _rbRaw = new RadioButton { Text = "Raw", AutoSize = true, Font = new Font("Arial", 14), Dock = DockStyle.Fill };
        private readonly RadioButton _rbResult = new RadioButton { Text = "Result", AutoSize = true, Font = new Font("Arial", 14), Dock = DockStyle.Fill };
        private readonly ComboBox _cbFilter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Arial", 14), Dock = DockStyle.Fill };
        private readonly Button _btnRefresh = new Button { Font = new Font("Arial", 14), Height = 35, Text = "Refresh", Width = 120, TextAlign = ContentAlignment.MiddleCenter, TextImageRelation = TextImageRelation.ImageBeforeText, Image = Properties.Resources.Refresh25, Dock = DockStyle.Fill };

        private readonly TableLayoutPanel _row1Flow = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
        private readonly TableLayoutPanel _row2Flow = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
        private readonly TableLayoutPanel _row3Flow = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };

        private readonly Label _lblFrom = new Label { Text = "From", AutoSize = true, Font = new Font("Arial", 14), Margin = new Padding(2, 6, 2, 2) };
        private readonly Label _lbDate = new Label { Text = "Date", AutoSize = true, Font = new Font("Arial", 14), Margin = new Padding(2, 6, 2, 2) };
        private readonly DateTimePicker _dtFrom = new DateTimePicker { Format = DateTimePickerFormat.Time, ShowUpDown = true, Font = new Font("Arial", 14), Width = 130, Dock = DockStyle.Fill };
        private readonly Label _lblTo = new Label { Text = "To", AutoSize = true, Font = new Font("Arial", 14), Margin = new Padding(2, 6, 2, 2) };
        private readonly DateTimePicker _dtTo = new DateTimePicker { Format = DateTimePickerFormat.Time, ShowUpDown = true, Font = new Font("Arial", 14), Width = 130, Dock = DockStyle.Fill };

        private readonly TextBox _tbSearch = new TextBox { BorderStyle = BorderStyle.FixedSingle, Font = new Font("Arial", 14), Dock = DockStyle.Fill };
        private readonly Timer _searchTimer = new Timer { Interval = 300 };
        private readonly Timer tmLoad = new Timer { Interval = 300 };
        private readonly ComboBox _cbSort = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180, Font = new Font("Arial", 14), Dock = DockStyle.Fill };

        // ====== BOTTOM ======
        private readonly TableLayoutPanel _bottomTL = new TableLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Padding = new Padding(2, 5, 2, 5),
        };
        private readonly Button _btnPrev = new Button { Width = 80, Text = "◀ Back", AutoSize = false, Height = 35, Font = new Font("Arial", 14), Dock = DockStyle.Fill, Margin = new Padding(10, 0, 5, 0) };
        private readonly Button _btnNext = new Button { Width = 80, Text = "Next ▶", AutoSize = false, Height = 35, Font = new Font("Arial", 14), Dock = DockStyle.Fill, Margin = new Padding(5, 0, 5, 0) };
        private readonly Label _lblSzie = new Label { Margin = new Padding(2, 4, 2, 2), Text = "Per page", AutoSize = true, Font = new Font("Arial", 14), Dock = DockStyle.Fill };
        private readonly ComboBox _cbPageSize = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 60, Font = new Font("Arial", 14), Dock = DockStyle.Fill, Margin = new Padding(10, 2, 0, 0) };
        private readonly Label _lblPage = new Label { Margin = new Padding(2, 4, 2, 2), AutoSize = false, ForeColor = Color.Black, Font = new Font("Arial", 14), TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill };

        // ===== Busy overlay (blocking page) =====
        private Panel _busyOverlay;
        private Label _busyText;
        private ProgressBar _busyBar;
        private Button _busyCancel;
        private CancellationTokenSource _ctsBusy;

        // ===== Scroll & Data =====
        private readonly VScrollBar _vScroll = new VScrollBar();
        private readonly List<string> _allFiles = new List<string>();
        private readonly List<ImageItem> _items = new List<ImageItem>();

        // ===== Cache full-res =====
        private readonly Dictionary<string, Image> _cache = new Dictionary<string, Image>();
        private readonly Queue<string> _cacheQueue = new Queue<string>();
        private readonly HashSet<string> _loadingSet = new HashSet<string>();
        private readonly object _cacheLock = new object();
        private int _cacheSize = 100;

        // ===== Thumbnail cache (nhanh) =====
        private readonly Dictionary<string, Image> _thumbCache = new Dictionary<string, Image>();
        private readonly HashSet<string> _thumbLoading = new HashSet<string>();
        private readonly object _thumbLock = new object();

        // ===== Async build list =====
        private CancellationTokenSource _ctsList;
        private volatile bool _isLoadingList = false;

        // ===== Size cache (đọc header nền) =====
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, Size> _sizeCache
            = new System.Collections.Concurrent.ConcurrentDictionary<string, Size>();
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, byte> _sizeLoading
            = new System.Collections.Concurrent.ConcurrentDictionary<string, byte>();
        private readonly SemaphoreSlim _sizeGate = new SemaphoreSlim(4); // đọc header tối đa 4
        private CancellationTokenSource _ctsSizes;

        // ===== Throttling decode + thumb + in-flight =====
        private readonly SemaphoreSlim _thumbGate = new SemaphoreSlim(8); // shell thumb song song
        private readonly SemaphoreSlim _decodeGate = new SemaphoreSlim(3); // full-res song song
        private int _maxInflightQueue = 24; // tối đa job full-res chờ
        private CancellationTokenSource _ctsImages;

        // Debounce invalidate (gom vẽ lại)
        private readonly Timer _invalidateTimer = new Timer { Interval = 16 }; // ~60fps
        private Rectangle _pendingInvalidate = Rectangle.Empty;

        // Blocking page state
        private bool _isBlockingPageLoad = false;
        private bool _allowInteractions => !_isBlockingPageLoad;
        private CancellationTokenSource _ctsPageBlock;

        // File định dạng không hỗ trợ
        private readonly HashSet<string> _unsupportedFull = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Config
        private int _pageSize = 10;
        private string _rootFolder = @"Report";
        private string _lastSelectedDate;
        private int _currentPage = 0;
        private int _totalPages = 0;

        private ImageItem _activeItem;
        private Point _lastMouse;
        private bool _didInitialShownRefresh = false;

        // Layout constants
        private const int ItemTopPad = 10;
        private const int ItemBottomPad = 10;
        bool IsLoad = false;
        public DashboardImages()
        {
            DoubleBuffered = true;
            BackColor = SystemColors.Control;
            TabStop = true;
            tmLoad.Tick += TmLoad_Tick;
            // ===== compose Top (3 row) =====
            _topTL.ColumnStyles.Clear();
            _topTL.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _topTL.RowStyles.Clear();
            _topTL.RowStyles.Add(new RowStyle(SizeType.AutoSize, 40));
            _topTL.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            _topTL.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _topTL.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            _cbSort.Items.AddRange(new object[] { "Newest → Oldest", "Oldest → Newest", "Name A → Z", "Name Z → A" });
            _cbSort.SelectedIndex = 0;

            _cbFilter.Items.AddRange(new object[] { "All", "Newest", "Only OK", "Only NG" });
            _cbFilter.SelectedIndex = 0;
            _rbResult.Checked = true;

            // row1
            
            _row1Flow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _row1Flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _row1Flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _row1Flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _row1Flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _row1Flow.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
         //   _row1Flow.Controls.Add(_lbDate, 0, 0);
            _row1Flow.Controls.Add(_cbDate, 0, 0);
            _row1Flow.Controls.Add(_lblFrom, 1, 0);
            _row1Flow.Controls.Add(_dtFrom, 2, 0);
            _row1Flow.Controls.Add(_lblTo, 3, 0);
            _row1Flow.Controls.Add(_dtTo, 4, 0);

            // row2
            _row2Flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _row2Flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _row2Flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _row2Flow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _row2Flow.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _row2Flow.Controls.Add(_cbFilter, 0, 0);
            _row2Flow.Controls.Add(_rbRaw, 1, 0);
            _row2Flow.Controls.Add(_rbResult, 2, 0);
            _row2Flow.Controls.Add(_cbSort, 3, 0);

            // row3
            _row3Flow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _row3Flow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _row3Flow.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _row3Flow.Controls.Add(_tbSearch, 0, 0);
            _row3Flow.Controls.Add(_btnRefresh, 1, 0);

            _topTL.Controls.Add(_row1Flow, 0, 0);
            _topTL.Controls.Add(_row2Flow, 0, 1);
            _topTL.Controls.Add(_row3Flow, 0, 2);
            Controls.Add(_topTL);

            // ===== compose Bottom =====
            _bottomTL.ColumnStyles.Clear();
            _bottomTL.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _bottomTL.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _bottomTL.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _bottomTL.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _bottomTL.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            _bottomTL.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _bottomTL.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            _cbPageSize.Items.AddRange(new object[] { "10", "20", "50", "100" });
            _cbPageSize.SelectedIndex = 0;
            _bottomTL.Controls.Add(_cbPageSize, 0, 0);
            _bottomTL.Controls.Add(_lblSzie, 1, 0);
            _bottomTL.Controls.Add(_btnPrev, 2, 0);
            _bottomTL.Controls.Add(_btnNext, 3, 0);
            _bottomTL.Controls.Add(_lblPage, 4, 0);
            Controls.Add(_bottomTL);

            // ===== Busy overlay =====
            BuildBusyOverlay();

            // ===== Scroll bar =====
            _vScroll.Dock = DockStyle.Right;
            _vScroll.Scroll += (s, e) => { if (_allowInteractions) Invalidate(); };
            Controls.Add(_vScroll);

            // ===== Events =====
            _btnRefresh.Click += (s, e) =>
            {
                ShowBusyBlocking("Loading page…");
                ReloadAccordingToUI();
            };

            _cbDate.SelectedIndexChanged += (s, e) =>
            {   if (!IsLoad) return;
                _lastSelectedDate = _cbDate.SelectedItem != null ? _cbDate.SelectedItem.ToString() : null;
                ResetTimePickersToFullDay();
                ShowBusyBlocking("Loading page…");
                ReloadAccordingToUI();
            };
            _rbRaw.CheckedChanged += (s, e) => { if (_rbRaw.Checked) {
                    ShowBusyBlocking("Loading page…"); ReloadAccordingToUI(); } };
            _rbResult.CheckedChanged += (s, e) => { if (_rbResult.Checked) {
                    ShowBusyBlocking("Loading page…"); ReloadAccordingToUI(); } };
            _cbFilter.SelectedIndexChanged += (s, e) => {
                ShowBusyBlocking("Loading page…"); ReloadAccordingToUI(); };

            _cbPageSize.SelectedIndexChanged += (s, e) =>
            {
                if (int.TryParse(_cbPageSize.SelectedItem?.ToString() ?? "10", out var val) && val > 0)
                {
                    if (!IsLoad) return;
                    _pageSize = val;
                    _currentPage = 0;
                    bool multiPages = (CurrentFilter == FilterMode.All || CurrentFilter == FilterMode.OnlyOK || CurrentFilter == FilterMode.OnlyNG);
                    _totalPages = multiPages ? Math.Max(1, (int)Math.Ceiling(_allFiles.Count / (float)_pageSize)) : 1;
                    ShowBusyBlocking("Loading page…");
                    LoadPage();
                }
            };

            _dtFrom.ValueChanged += (s, e) => {
                if (!IsLoad) return; ShowBusyBlocking("Loading page…"); ReloadAccordingToUI(); };
            _dtTo.ValueChanged += (s, e) => {
                if (!IsLoad) return; ShowBusyBlocking("Loading page…"); ReloadAccordingToUI(); };

            _tbSearch.TextChanged += (s, e) => { _searchTimer.Stop(); _searchTimer.Start(); };
            _searchTimer.Tick += (s, e) => { _searchTimer.Stop(); ShowBusyBlocking("Loading page…"); ReloadAccordingToUI(); };
            _tbSearch.HandleCreated += (s, e) =>
            {
                try { SendMessage(_tbSearch.Handle, EM_SETCUEBANNER, (IntPtr)1, "Lọc theo tên…"); } catch { }
            };

            _cbSort.SelectedIndexChanged += (s, e) => {
                if (!IsLoad) return; ShowBusyBlocking("Loading page…"); ReloadAccordingToUI(); };

            _btnPrev.Click += (s, e) => { if (_currentPage > 0) { 
                    if (!IsLoad) return; _currentPage--; ShowBusyBlocking("Loading page…"); LoadPage(); } };
            _btnNext.Click += (s, e) => { if (_currentPage < _totalPages - 1) {
                    if (!IsLoad) return; _currentPage++; ShowBusyBlocking("Loading page…"); LoadPage(); } };

            MouseWheel += Dashboard_MouseWheel;
            MouseDown += Dashboard_MouseDown;
            MouseMove += Dashboard_MouseMove;
            MouseDoubleClick += Dashboard_MouseDoubleClick;

            // Debounce Invalidate
            _invalidateTimer.Tick += (s, e) =>
            {
                _invalidateTimer.Stop();
                if (!IsDisposed && IsHandleCreated)
                {
                    if (_pendingInvalidate.IsEmpty) Invalidate();
                    else { Invalidate(_pendingInvalidate); _pendingInvalidate = Rectangle.Empty; }
                }
            };
            tmLoad.Enabled = true;
            // First run
            RefreshDates();
            ResetTimePickersToFullDay();
           // HookFormShownOnce();
        }

        private void TmLoad_Tick(object sender, EventArgs e)
        {
            tmLoad.Enabled = false;
            IsLoad = true;
        }

        // ===== Busy overlay builder =====
        private void BuildBusyOverlay()
        {
            _busyOverlay = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(160, Color.Black), Visible = false };
            _busyText = new Label { ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Top, Height = 40, Text = "Loading…" };
            _busyBar = new ProgressBar { Dock = DockStyle.Top, Height = 20, Style = ProgressBarStyle.Marquee, MarqueeAnimationSpeed = 30 };
            _busyCancel = new Button { Text = "Cancel", Height = 32, Width = 100, BackColor = Color.White, FlatStyle = FlatStyle.Standard };

            var inner = new Panel { Size = new Size(320, 120), BackColor = Color.FromArgb(24, 24, 24), Padding = new Padding(16) };
            inner.Controls.Add(_busyCancel);
            inner.Controls.Add(_busyBar);
            inner.Controls.Add(_busyText);
            inner.Dock = DockStyle.None;

            _busyOverlay.Controls.Add(inner);
            Controls.Add(_busyOverlay);
            _busyOverlay.BringToFront();

            // layout
            _busyOverlay.Resize += (s, e) =>
            {
                inner.Left = (_busyOverlay.Width - inner.Width) / 2;
                inner.Top = (_busyOverlay.Height - inner.Height) / 2;
                _busyCancel.Top = _busyBar.Bottom + 12;
                _busyCancel.Left = (inner.Width - _busyCancel.Width) / 2;
                _busyBar.Top = _busyText.Bottom + 8;
            };

            _busyCancel.Click += (s, e) =>
            {
                _ctsBusy?.Cancel();
                _ctsPageBlock?.Cancel();
                _ctsImages?.Cancel();
                _ctsSizes?.Cancel();
                HideBusyUnblock(); // cho user điều khiển lại ngay
            };
        }

        private void ShowBusyBlocking(string text)
        {
            _ctsBusy?.Cancel();
            _ctsBusy = new CancellationTokenSource();

            _isBlockingPageLoad = true;
            _busyText.Text = string.IsNullOrEmpty(text) ? "Loading page…" : text;
            _busyOverlay.Visible = true;
            _busyOverlay.BringToFront();

            // disable tương tác
            _vScroll.Enabled = false;
            _btnPrev.Enabled = false;
            _btnNext.Enabled = false;
            _cbPageSize.Enabled = false;
            _cbFilter.Enabled = false;
            _cbSort.Enabled = false;
            _rbRaw.Enabled = false;
            _rbResult.Enabled = false;
            _cbDate.Enabled = false;
            _dtFrom.Enabled = false;
            _dtTo.Enabled = false;
            _tbSearch.Enabled = false;
            _btnRefresh.Enabled = false;
        }

        private void HideBusyUnblock()
        {
            _isBlockingPageLoad = false;
            _busyOverlay.Visible = false;

            _vScroll.Enabled = true;
            _btnPrev.Enabled = true;
            _btnNext.Enabled = true;
            _cbPageSize.Enabled = true;
            _cbFilter.Enabled = true;
            _cbSort.Enabled = true;
            _rbRaw.Enabled = true;
            _rbResult.Enabled = true;
            _cbDate.Enabled = true;
            _dtFrom.Enabled = true;
            _dtTo.Enabled = true;
            _tbSearch.Enabled = true;
            _btnRefresh.Enabled = true;
        }

        // ===== HOTKEY: Ctrl+R =====
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!_allowInteractions) return true; // chặn phím khi đang block
            if (keyData == (Keys.Control | Keys.R))
            {
                ShowBusyBlocking("Loading page…");
                RefreshDates();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ===== Public =====
        public void SetRoot(string rootFolder)
        {
            _rootFolder = rootFolder ?? "";
            ShowBusyBlocking("Loading page…");
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

            //ReloadAccordingToUI();
        }

        private string CurrentDate => _cbDate.SelectedItem != null ? _cbDate.SelectedItem.ToString() : null;
        private bool UseRaw => _rbRaw.Checked;
        private FilterMode CurrentFilter => (FilterMode)_cbFilter.SelectedIndex;
        public void ReloadAccordingToUI()
        {
            if (string.IsNullOrEmpty(CurrentDate)|| !IsHandleCreated)
            {
                ClearAll();
                HideBusyUnblock();
                return;
            }

            _ctsList?.Cancel();
            _ctsList = new CancellationTokenSource();
            var ct = _ctsList.Token;

            _isLoadingList = true;
            _lblPage.Text = "Loading…";
            Invalidate();

            string date = CurrentDate;
            bool useRaw = UseRaw;
            FilterMode filter = CurrentFilter;
            string nameFilter = (_tbSearch.Text ?? string.Empty).Trim();
            var sortMode = (SortMode)_cbSort.SelectedIndex;

            DateTime day;
            if (!DateTime.TryParseExact(
                    date,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out day))
            {
                day = DateTime.Today;
            }

            var startLocal = day.Date + _dtFrom.Value.TimeOfDay;
            var endLocal = day.Date + _dtTo.Value.TimeOfDay;
            if (endLocal < startLocal)
            {
                var t = startLocal;
                startLocal = endLocal;
                endLocal = t;
            }

            Task.Run(() =>
            {
                List<string> files = null;

                try
                {
                    files = BuildFileListCore(
                        date, useRaw, filter, nameFilter,
                        startLocal, endLocal, sortMode, ct
                    );
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch
                {
                    files = null;
                }

                if (ct.IsCancellationRequested)
                    return;

                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke((Action)(() =>
                    {
                        _isLoadingList = false;

                        if (files == null || files.Count == 0)
                        {
                            // ✅ QUAN TRỌNG: không để UI kẹt
                            ClearAll();
                            _lblPage.Text = "No images";
                            HideBusyUnblock();
                            Invalidate();
                            return;
                        }

                        _allFiles.Clear();
                        _allFiles.AddRange(files);

                        _currentPage = 0;
                        bool multiPages =
                            (filter == FilterMode.All ||
                             filter == FilterMode.OnlyOK ||
                             filter == FilterMode.OnlyNG);

                        _totalPages = multiPages
                            ? Math.Max(1, (int)Math.Ceiling(_allFiles.Count / (float)_pageSize))
                            : 1;

                        LoadPage();   // bản production bạn đã sửa
                    }));
                }
            });
        }


        //private void ReloadAccordingToUI()
        //{
        //    if (string.IsNullOrEmpty(CurrentDate))
        //    {
        //        ClearAll();
        //        HideBusyUnblock();
        //        return;
        //    }

        //    _ctsList?.Cancel();
        //    _ctsList = new CancellationTokenSource();
        //    var ct = _ctsList.Token;
        //    _isLoadingList = true;
        //    _lblPage.Text = "Loading…";
        //    Invalidate();

        //    string date = CurrentDate;
        //    bool useRaw = UseRaw;
        //    FilterMode filter = CurrentFilter;
        //    string nameFilter = (_tbSearch.Text ?? string.Empty).Trim();
        //    var sortMode = (SortMode)_cbSort.SelectedIndex;

        //    if (!DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var day))
        //        day = DateTime.Today;

        //    var startLocal = day.Date + _dtFrom.Value.TimeOfDay;
        //    var endLocal = day.Date + _dtTo.Value.TimeOfDay;
        //    if (endLocal < startLocal) { var t = startLocal; startLocal = endLocal; endLocal = t; }

        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            var files = BuildFileListCore(date, useRaw, filter, nameFilter, startLocal, endLocal, sortMode, ct);
        //            if(files.Count==0)
        //                return;
        //            if (ct.IsCancellationRequested) return;

        //            if (!IsDisposed && IsHandleCreated)
        //            {
        //                BeginInvoke((Action)(() =>
        //                {
        //                    _allFiles.Clear();
        //                    _allFiles.AddRange(files);

        //                    _currentPage = 0;
        //                    bool multiPages = (filter == FilterMode.All || filter == FilterMode.OnlyOK || filter == FilterMode.OnlyNG);
        //                    _totalPages = multiPages ? Math.Max(1, (int)Math.Ceiling(_allFiles.Count / (float)_pageSize)) : 1;

        //                    LoadPage(); // blocking page flow bên trong
        //                    _isLoadingList = false;
        //                }));
        //            }
        //        }
        //        catch (OperationCanceledException) { }
        //        catch
        //        {
        //            if (!IsDisposed && IsHandleCreated)
        //            {
        //                BeginInvoke((Action)(() =>
        //                {
        //                    _isLoadingList = false;
        //                    _lblPage.Text = "Error loading files";
        //                    HideBusyUnblock();
        //                    Invalidate();
        //                }));
        //            }
        //        }
        //    });
        //}

        private List<string> BuildFileListCore(
            string dateFolder,
            bool useRaw,
            FilterMode mode,
            string nameFilter,
            DateTime startLocal,
            DateTime endLocal,
            SortMode sortMode,
            CancellationToken ct)
        {
            var list = new List<string>();
            var type = useRaw ? "Raw" : "Result";
            var path = Path.Combine(_rootFolder ?? "", dateFolder ?? "", type);
            if (!Directory.Exists(path)) return list;

            // Nếu bạn không có decoder .webp, đừng thêm vào exts
            var exts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { ".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff" /*, ".webp"*/ };

            IEnumerable<string> all = Directory.EnumerateFiles(path).Where(f => exts.Contains(Path.GetExtension(f)));

            string nf = (nameFilter ?? "").Trim();
            Func<string, bool> NameMatch = string.IsNullOrEmpty(nf)
                ? (Func<string, bool>)(_ => true)
                : (f => Path.GetFileName(f).IndexOf(nf, StringComparison.OrdinalIgnoreCase) >= 0);

            bool IsOK(string f)
            {
                var name = Path.GetFileName(f);
                return name.IndexOf("_OK", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       name.IndexOf("_OK", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       name.EndsWith("_OK", StringComparison.OrdinalIgnoreCase);
            }
            bool IsNG(string f)
            {
                var name = Path.GetFileName(f);
                return name.IndexOf("_NG", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       name.IndexOf("_NG", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       name.EndsWith("_NG", StringComparison.OrdinalIgnoreCase);
            }

            bool TimeInRange(string f)
            {
                DateTime t = File.GetLastWriteTime(f);
                return t >= startLocal && t <= endLocal;
            }

            IEnumerable<string> filtered = all.Where(f => TimeInRange(f) && NameMatch(f));

            if (mode == FilterMode.OnlyOK) filtered = filtered.Where(IsOK);
            else if (mode == FilterMode.OnlyNG) filtered = filtered.Where(IsNG);

            IEnumerable<string> sorted;
            switch (sortMode)
            {
                case SortMode.TimeAsc: sorted = filtered.OrderBy(f => File.GetLastWriteTime(f)); break;
                case SortMode.NameAsc: sorted = filtered.OrderBy(f => Path.GetFileName(f), StringComparer.CurrentCultureIgnoreCase); break;
                case SortMode.NameDesc: sorted = filtered.OrderByDescending(f => Path.GetFileName(f), StringComparer.CurrentCultureIgnoreCase); break;
                case SortMode.TimeDesc:
                default: sorted = filtered.OrderByDescending(f => File.GetLastWriteTime(f)); break;
            }

            if (mode == FilterMode.NewestAny)
            {
                var newest = filtered.OrderByDescending(f => File.GetLastWriteTime(f)).FirstOrDefault();
                if (newest != null) list.Add(newest);
                return list;
            }

            foreach (var f in sorted)
            {
                if (ct.IsCancellationRequested) break;
                list.Add(f);
            }

            return list;
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

        private void LoadPage()
        {
            // Hủy job cũ
            _ctsImages?.Cancel(); _ctsImages = new CancellationTokenSource();
            _ctsSizes?.Cancel(); _ctsSizes = new CancellationTokenSource();
            _ctsPageBlock?.Cancel(); _ctsPageBlock = new CancellationTokenSource();

            // Bật blocking nếu chưa bật
            if (!_isBlockingPageLoad) ShowBusyBlocking("Loading page…");

            // Tính file của trang
            var pageFiles = new List<string>();
            bool multiPages = (CurrentFilter == FilterMode.All || CurrentFilter == FilterMode.OnlyOK || CurrentFilter == FilterMode.OnlyNG);
            if (multiPages)
            {
                int start = _currentPage * _pageSize;
                int end = Math.Min(start + _pageSize, _allFiles.Count);
                for (int i = start; i < end; i++) pageFiles.Add(_allFiles[i]);
            }
            else
            {
                if (_allFiles.Count > 0) pageFiles.Add(_allFiles[0]);
            }

            // Dựng item (model) trước — chưa decode ảnh
            _items.Clear();
            foreach (var fp in pageFiles)
                _items.Add(new ImageItem { Path = fp, Caption = Path.GetFileName(fp) });

            UpdateLayout();
            UpdatePageLabel();

            // 1) Preload header size (nhanh)
            var ctBlock = _ctsPageBlock.Token;
            Task.Run(async () =>
            {
                try
                {
                    await PreloadSizesForPageAsync(_items.ToArray(), _ctsSizes.Token).ConfigureAwait(false);

                    // 2) Preload FULL-RES cho TẤT CẢ ảnh của trang (ưu tiên thấp, throttling, retry)
                    await PreloadFullForPageAsync(_items.Select(it => it.Path).ToArray(), _ctsImages.Token).ConfigureAwait(false);

                    // 3) Xong → vẽ lại và mở khóa
                    if (!IsDisposed && IsHandleCreated)
                    {
                        BeginInvoke((Action)(() =>
                        {
                            UpdateLayout();
                            HideBusyUnblock();
                        }));
                    }
                }
                catch (OperationCanceledException) { /* đổi trang/Cancel */ }
                catch
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        BeginInvoke((Action)(() =>
                        {
                            // Dù lỗi vẫn mở khóa để không kẹt UI
                            HideBusyUnblock();
                        }));
                    }
                }
            }, ctBlock);
        }

        private void UpdatePageLabel()
        {
            bool multiPages = (CurrentFilter == FilterMode.All || CurrentFilter == FilterMode.OnlyOK || CurrentFilter == FilterMode.OnlyNG);
            if (multiPages)
                _lblPage.Text = $"Page {_currentPage + 1}/{Math.Max(1, _totalPages)}  •  {_allFiles.Count} imgs  •  {_pageSize}/page";
            else
                _lblPage.Text = $"{CurrentFilter}  •  {(_allFiles.Count > 0 ? 1 : 0)} img";
        }

        // ===== Layout & Paint =====
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            int y = _topTL.Bottom + 6;
            int rightMargin = _vScroll.Width + 10;
            int frameW = Math.Max(50, ClientSize.Width - rightMargin);

            foreach (var it in _items)
            {
                if (it.OrigSize.IsEmpty)
                {
                    if (_sizeCache.TryGetValue(it.Path, out var sz) && sz.Width > 0 && sz.Height > 0)
                        it.OrigSize = sz;
                    else
                        it.OrigSize = GetEstimatedSizeForLayout();
                }

                float baseScale = (float)frameW / Math.Max(1, it.OrigSize.Width);
                float drawW = it.OrigSize.Width * baseScale * it.Zoom;
                float drawH = it.OrigSize.Height * baseScale * it.Zoom;

                int left = (int)((frameW - drawW) / 2f + it.Pan.X);

                float topPadDynamic = ItemTopPad + Math.Max(0f, -it.Pan.Y);
                float bottomPadDynamic = ItemBottomPad + Math.Max(0f, it.Pan.Y);

                int totalH = (int)Math.Ceiling(topPadDynamic + drawH + bottomPadDynamic);
                it.Bounds = new Rectangle(0, y, frameW, totalH);

                int top = (int)(y + topPadDynamic);
                it.ImgRect = new Rectangle(left, top, (int)drawW, (int)drawH);

                y += totalH + 10;
            }

            int contentHeight = Math.Max(y - _topTL.Bottom, 0);
            int viewport = Math.Max(0, ClientSize.Height - _topTL.Height - _bottomTL.Height);
            _vScroll.Maximum = Math.Max(0, contentHeight - viewport);
            _vScroll.LargeChange = viewport;

            if (_vScroll.Value > _vScroll.Maximum) _vScroll.Value = _vScroll.Maximum;
            if (_vScroll.Value < _vScroll.Minimum) _vScroll.Value = _vScroll.Minimum;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            e.Graphics.TranslateTransform(0, -_vScroll.Value);

            foreach (var it in _items)
            {
                if (!it.ImgRect.IsEmpty)
                    DrawImageSmart(e.Graphics, it, true);

                using (var p = new Pen(Color.FromArgb(70, 70, 70)))
                    e.Graphics.DrawRectangle(p, it.ImgRect);

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

                string status = it.Caption.IndexOf("_OK", StringComparison.OrdinalIgnoreCase) >= 0 ? "OK" :
                                (it.Caption.IndexOf("_NG", StringComparison.OrdinalIgnoreCase) >= 0 ? "NG" : "");
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

        // ===== Image helpers =====
        private static Rectangle FitRectKeepAspect(Size src, Rectangle dest)
        {
            if (src.Width <= 0 || src.Height <= 0 || dest.Width <= 0 || dest.Height <= 0)
                return dest;
            double rw = (double)dest.Width / src.Width;
            double rh = (double)dest.Height / src.Height;
            double r = Math.Min(rw, rh);
            int w = Math.Max(1, (int)Math.Round(src.Width * r));
            int h = Math.Max(1, (int)Math.Round(src.Height * r));
            int x = dest.X + (dest.Width - w) / 2;
            int y = dest.Y + (dest.Height - h) / 2;
            return new Rectangle(x, y, w, h);
        }

        private void DrawPlaceholder(Graphics g, Rectangle dest)
        {
            using (var br = new SolidBrush(Color.FromArgb(225, 225, 225)))
                g.FillRectangle(br, dest);
            using (var pen = new Pen(Color.Silver))
                g.DrawRectangle(pen, dest);
            using (var txt = new SolidBrush(Color.Gray))
            using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString("Loading…", Font, txt, dest, fmt);
        }

        private void DrawImageSmart(Graphics g, ImageItem it, bool highPriority)
        {
            var dest = it.ImgRect;
            var path = it.Path;

            // 1) Có full-res rồi → vẽ luôn
            lock (_cacheLock)
            {
                if (_cache.TryGetValue(path, out var fullImg) && fullImg != null)
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    g.DrawImage(fullImg, dest);
                    return;
                }
            }

            // 2) Có thumbnail thì fit vẽ tạm
            lock (_thumbLock)
            {
                if (_thumbCache.TryGetValue(path, out var th) && th != null)
                {
                    var fit = FitRectKeepAspect(th.Size, dest);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    g.DrawImage(th, fit);
                    return;
                }
            }

            // 3) Placeholder hoặc thông báo unsupported
            if (_unsupportedFull.Contains(path))
            {
                using (var br = new SolidBrush(Color.FromArgb(235, 235, 235)))
                    g.FillRectangle(br, dest);
                using (var pen = new Pen(Color.OrangeRed))
                    g.DrawRectangle(pen, dest);
                using (var f = new Font("Segoe UI", 10, FontStyle.Bold))
                using (var txt = new SolidBrush(Color.OrangeRed))
                using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString("Unsupported image format", f, txt, dest, fmt);
            }
            else
            {
                DrawPlaceholder(g, dest);
            }
        }

        // === Size helpers (đọc header nền, không block UI) ===
        private Size GetEstimatedSizeForLayout()
        {
            // Ước lượng tạm (nhẹ) — sẽ thay ngay khi preload header xong
            return new Size(640, 360); // ~16:9
        }

        private async Task<Size> ReadImageSizeAsync(string path, CancellationToken ct)
        {
            await _sizeGate.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                ct.ThrowIfCancellationRequested();
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var img = Image.FromStream(fs, false, false)) // chỉ đọc header, không validate
                    return img.Size;
            }
            finally { _sizeGate.Release(); }
        }

        private async Task PreloadSizesForPageAsync(ImageItem[] items, CancellationToken ct)
        {
            try
            {
                // Ưu tiên gần viewport (dựa theo Bounds đã dựng)
                int visTop = _topTL.Bottom - _vScroll.Value;
                int visBottom = Height - _bottomTL.Height - _vScroll.Value;
                int mid = (visTop + visBottom) / 2;

                var ordered = items.OrderBy(it => Math.Abs(mid - ((it.Bounds.Top + it.Bounds.Bottom) / 2))).ToArray();

                var tasks = ordered.Select(async it =>
                {
                    ct.ThrowIfCancellationRequested();
                    if (_sizeCache.ContainsKey(it.Path)) return;
                    var sz = await ReadImageSizeAsync(it.Path, ct).ConfigureAwait(false);
                    if (sz.Width > 0 && sz.Height > 0)
                    {
                        _sizeCache[it.Path] = sz;
                        it.OrigSize = sz;
                    }
                });
                await Task.WhenAll(tasks).ConfigureAwait(false);

                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke((Action)(() =>
                    {
                        _invalidateTimer.Stop(); _invalidateTimer.Start();
                    }));
                }
            }
            catch { /* ignore */ }
        }

        // === Full-res loader cho TOÀN TRANG (await all) — có retry + FileShare.ReadWrite ===
        private async Task PreloadFullForPageAsync(string[] files, CancellationToken ct)
        {
            // Decode full-res toàn trang (giới hạn song song)
            var tasks = files.Select(path => Task.Run(async () =>
            {
                // Đã có cache? hoặc đã unsupported? → bỏ
                lock (_cacheLock)
                {
                    if (_cache.ContainsKey(path)) return;
                }
                if (_unsupportedFull.Contains(path)) return;

                // Đánh dấu in-flight
                lock (_cacheLock)
                {
                    if (_loadingSet.Contains(path)) return;
                    _loadingSet.Add(path);
                }

                Image loaded = null;
                try
                {
                    await _decodeGate.WaitAsync(ct).ConfigureAwait(false);
                    try
                    {
                        if (ct.IsCancellationRequested) return;

                        const int MAX_ATTEMPTS = 6;
                        const int SLEEP_MS = 80;
                        for (int attempt = 1; attempt <= MAX_ATTEMPTS && loaded == null; attempt++)
                        {
                            ct.ThrowIfCancellationRequested();
                            try
                            {
                                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    long len = fs.Length;
                                    fs.Position = 0;
                                    using (var ms = new MemoryStream(len > 0 ? (int)Math.Min(len, 4 * 1024 * 1024) : 0))
                                    {
                                        await fs.CopyToAsync(ms, 64 * 1024, ct).ConfigureAwait(false);
                                        ms.Position = 0;
                                        loaded = Image.FromStream(ms, false, false);
                                    }
                                }
                            }
                            catch (IOException)
                            {
                                await Task.Delay(SLEEP_MS, ct).ConfigureAwait(false); // file đang ghi
                            }
                            catch (ExternalException)
                            {
                                _unsupportedFull.Add(path); // có thể là .webp
                                break;
                            }
                            catch
                            {
                                await Task.Delay(SLEEP_MS, ct).ConfigureAwait(false);
                            }
                        }

                        if (ct.IsCancellationRequested) { loaded?.Dispose(); loaded = null; return; }
                    }
                    finally
                    {
                        _decodeGate.Release();
                    }

                    // Lưu cache
                    if (loaded != null)
                    {
                        lock (_cacheLock)
                        {
                            if (!_cache.ContainsKey(path))
                            {
                                _cache[path] = loaded;
                                _cacheQueue.Enqueue(path);
                                while (_cacheQueue.Count > _cacheSize)
                                {
                                    var oldKey = _cacheQueue.Dequeue();
                                    if (_cache.TryGetValue(oldKey, out var oldImg))
                                    {
                                        _cache.Remove(oldKey);
                                        try { oldImg.Dispose(); } catch { }
                                    }
                                }
                            }
                            else
                            {
                                try { loaded.Dispose(); } catch { }
                            }
                        }
                    }
                }
                finally
                {
                    lock (_cacheLock) _loadingSet.Remove(path);
                }
            }, ct));

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        // ===== Interaction =====
        protected override void WndProc(ref Message m)
        {
            if (_isBlockingPageLoad)
            {
                const int WM_MOUSEWHEEL = 0x020A;
                const int WM_LBUTTONDOWN = 0x0201;
                const int WM_RBUTTONDOWN = 0x0204;
                const int WM_KEYDOWN = 0x0100;
                if (m.Msg == WM_MOUSEWHEEL || m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_KEYDOWN)
                    return; // nuốt sự kiện khi đang block
            }
            base.WndProc(ref m);
        }

        private void Dashboard_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!_allowInteractions) return;

            bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;
            if (!ctrl || _activeItem == null)
            {
                int step = SystemInformation.MouseWheelScrollLines * 30;
                int newVal = _vScroll.Value - Math.Sign(e.Delta) * step;
                if (newVal < _vScroll.Minimum) newVal = _vScroll.Minimum;
                if (newVal > _vScroll.Maximum) newVal = _vScroll.Maximum;
                _vScroll.Value = newVal;
                Invalidate();
                return;
            }

            AdjustZoomCenter(_activeItem, e.Delta > 0 ? 1.1f : 1f / 1.1f);
        }

        private void Dashboard_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_allowInteractions) return;
            Focus();
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
            if (!_allowInteractions) return;
            if (_activeItem != null && e.Button == MouseButtons.Left)
            {
                _activeItem.Pan = new PointF(
                    _activeItem.Pan.X + (e.X - _lastMouse.X),
                    _activeItem.Pan.Y + (e.Y - _lastMouse.Y)
                );
                _lastMouse = e.Location;
                UpdateLayout();
            }
        }

        private void Dashboard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!_allowInteractions) return;
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

        // Zoom ảnh quanh tâm khung hiện tại của item
        private void AdjustZoomCenter(ImageItem it, float factor)
        {
            if (it == null) return;

            // Bảo đảm có kích thước gốc
            if (it.OrigSize.IsEmpty)
            {
                if (!_sizeCache.TryGetValue(it.Path, out var sz) || sz.IsEmpty) return;
                it.OrigSize = sz;
            }

            // Tính scale theo bề rộng frame (né scrollbar)
            int rightMargin = _vScroll.Width + 10;
            int frameW = Math.Max(50, ClientSize.Width - rightMargin);
            float baseScale = (float)frameW / Math.Max(1, it.OrigSize.Width);

            // Rect hiện tại (sau pan/zoom cũ)
            var oldRect = it.ImgRect;
            float oldCx = oldRect.Left + oldRect.Width / 2f;
            float oldCy = oldRect.Top + oldRect.Height / 2f;

            // Giới hạn zoom
            float newZoom = Math.Max(0.05f, Math.Min(20f, it.Zoom * factor));

            // Kích thước mới sau zoom
            float newDrawW = it.OrigSize.Width * baseScale * newZoom;
            float newDrawH = it.OrigSize.Height * baseScale * newZoom;

            // Vị trí toạ độ ảnh mới trước khi bù tâm
            float newLeft = (frameW - newDrawW) / 2f + it.Pan.X;
            float newTop = ItemTopPad + it.Pan.Y;

            float newCx = newLeft + newDrawW / 2f;
            float newCy = newTop + newDrawH / 2f;

            // Bù pan để giữ tâm ảnh không nhảy
            it.Pan = new PointF(it.Pan.X + (oldCx - newCx), it.Pan.Y + (oldCy - newCy));
            it.Zoom = newZoom;

            UpdateLayout();
        }

        // ===== Shown hook =====
        private void HookFormShownOnce()
        {
            if (_didInitialShownRefresh) return;
            _didInitialShownRefresh = true;
            ResetTimePickersToFullDay();
            ShowBusyBlocking("Loading page…");
            ReloadAccordingToUI();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            HookFormShownOnce();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible) HookFormShownOnce();
        }

        private void ResetTimePickersToFullDay()
        {
            if (!DateTime.TryParseExact(
             _cbDate.SelectedItem?.ToString(),
             "yyyyMMdd",
             CultureInfo.InvariantCulture,
             DateTimeStyles.None,
             out var day))
            {
                day = DateTime.Today;
            }

            _dtFrom.Value = day.Date;
            _dtTo.Value = day.Date.AddDays(1).AddTicks(-1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ctsList?.Cancel();
                _ctsImages?.Cancel();
                _ctsSizes?.Cancel();
                _ctsBusy?.Cancel();
                _ctsPageBlock?.Cancel();
                ClearCache();
            }
            base.Dispose(disposing);
        }

        private void ClearCache()
        {
            _ctsImages?.Cancel();
            _ctsSizes?.Cancel();
            _ctsBusy?.Cancel();
            _ctsPageBlock?.Cancel();

            // Dọn background để UI không giật
            Task.Run(() =>
            {
                lock (_cacheLock)
                {
                    foreach (var kv in _cache) { try { kv.Value.Dispose(); } catch { } }
                    _cache.Clear(); _cacheQueue.Clear(); _loadingSet.Clear();
                }
                lock (_thumbLock)
                {
                    foreach (var kv in _thumbCache) { try { kv.Value.Dispose(); } catch { } }
                    _thumbCache.Clear(); _thumbLoading.Clear();
                }
                _sizeCache.Clear();
                _sizeLoading.Clear();
                _unsupportedFull.Clear();
            });
        }
    }
}
