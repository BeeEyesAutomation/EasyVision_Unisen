using OpenCvSharp.XFeatures2D;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace BeeGlobal
{
    public enum LeveLLog { All, TRACE, DEBUG, INFO, WARN, ERROR, FATAL }

    [DataContract]
    public class LogEntry
    {
        [DataMember(Order = 0)] public DateTime Time { get; set; }
        [DataMember(Order = 1)] public string Level { get; set; }
        [DataMember(Order = 2)] public string Source { get; set; }
        [DataMember(Order = 3)] public string Message { get; set; }

        public LogEntry() { }
        public LogEntry(DateTime time, LeveLLog level, string source, string message)
        {
            Time = time;
            Level = level.ToString();
            Source = source;
            Message = message;
        }
    }

    /// <summary>
    /// LogCanvas: control vẽ tay + ảo hoá (không DataGridView)
    /// </summary>
    internal class LogCanvas : Control
    {
        private readonly VScrollBar _vscroll = new VScrollBar();
        private readonly List<LogEntry> _items = new List<LogEntry>();
        private int _rowHeight = 20;
        private Font _boldFont;
        private int _firstVisibleIndex;
        private bool _autoScroll = true;

        private int _colTime = 170;
        private int _colLevel = 64;
        private int _colSource = 160;

        public bool AutoScrollToEnd
        {
            get { return _autoScroll; }
            set { _autoScroll = value; }
        }

        public LogCanvas()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.UserPaint
                     | ControlStyles.ResizeRedraw, true);

            BackColor = Color.White;
            ForeColor = Color.Black;

            _boldFont = new Font(Font, FontStyle.Bold);

            _vscroll.Dock = DockStyle.Right;
            _vscroll.SmallChange = 1;
            _vscroll.LargeChange = 1;
            _vscroll.ValueChanged += delegate
            {
                _firstVisibleIndex = _vscroll.Value;
                Invalidate();
            };
            Controls.Add(_vscroll);

            Resize += delegate
            {
                UpdateScroll();
                Invalidate();
            };
            // tự lấy focus khi trỏ chuột vào / click để nhận MouseWheel
            MouseEnter += (s, e) => { if (!Focused) Focus(); };
            MouseDown += (s, e) => { if (!Focused) Focus(); };
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            // Chỉ cho cuộn khi AutoScrollToEnd đang tắt (manual scroll)
            if (_autoScroll) return;
            if (_items.Count == 0) return;

            // số dòng cuộn mỗi "nấc" theo setting của Windows
            int linesPerNotch = SystemInformation.MouseWheelScrollLines;
            if (linesPerNotch <= 0) linesPerNotch = 3;

            // giữ Ctrl để cuộn theo "trang"
            int step = (ModifierKeys & Keys.Control) == Keys.Control
                ? PageSize()
                : linesPerNotch;

            // e.Delta dương = cuộn lên; âm = cuộn xuống
            int notches = e.Delta / 120;
            int deltaRows = -notches * step; // đảo dấu vì Value tăng nghĩa là xuống dưới

            int page = PageSize();
            int maxFirst = Math.Max(0, _items.Count - page);

            int newVal = _vscroll.Value + deltaRows;
            if (newVal < _vscroll.Minimum) newVal = _vscroll.Minimum;
            if (newVal > maxFirst) newVal = maxFirst;

            if (newVal != _vscroll.Value)
            {
                _vscroll.Value = newVal;   // sẽ kích hoạt ValueChanged và Invalidate()
            }
        }

        public void SetRowHeight(int h)
        {
            _rowHeight = Math.Max(14, h);
            UpdateScroll();
            Invalidate();
        }

        public void SetColumns(int timePx, int levelPx, int sourcePx)
        {
            _colTime = Math.Max(70, timePx);
            _colLevel = Math.Max(40, levelPx);
            _colSource = Math.Max(60, sourcePx);
            Invalidate();
        }

        public void Clear()
        {
            _items.Clear();
            UpdateScroll();
            Invalidate();
        }

        public void ReplaceAll(IList<LogEntry> list)
        {
            _items.Clear();
            if (list != null && list.Count > 0) _items.AddRange(list);
            UpdateScroll();
            if (_autoScroll) ScrollToEnd();
            Invalidate();
        }

        public void AppendBatch(IList<LogEntry> batch)
        {
            if (batch == null || batch.Count == 0) return;
            bool atBottomBefore = _autoScroll && IsNearBottom();
            _items.AddRange(batch);
            UpdateScroll();
            if (atBottomBefore && _autoScroll) ScrollToEnd();
            Invalidate();
        }

        public void AppendOne(LogEntry it)
        {
            if (it == null) return;
            bool atBottomBefore = _autoScroll && IsNearBottom();
            _items.Add(it);
            UpdateScroll();
            if (atBottomBefore && _autoScroll) ScrollToEnd();
            Invalidate();
        }

        public int Count { get { return _items.Count; } }

        public void ScrollToEnd()
        {
            int page = PageSize();
            int maxFirst = Math.Max(0, _items.Count - page);
            _vscroll.Value = Math.Min(_vscroll.Maximum, maxFirst);
        }

        private bool IsNearBottom()
        {
            int page = PageSize();
            return _firstVisibleIndex >= Math.Max(0, _items.Count - (page + 2));
        }
        private int _bottomPadding = 10; // khoảng cách dưới cùng (px)
        public void SetBottomPadding(int px)
        {
            _bottomPadding = Math.Max(0, px);
            UpdateScroll();
            Invalidate();
        }
        private int PageSize()
        {
            // trừ chiều cao header và khoảng cách dưới cùng
            int usable = ClientSize.Height - _rowHeight - _bottomPadding;
            if (usable < 1) usable = 1;
            return Math.Max(1, usable / _rowHeight);
        }

        private void UpdateScroll()
        {
            int total = _items.Count;
            int page = PageSize();

            _vscroll.Minimum = 0;
            _vscroll.LargeChange = Math.Max(1, page);
            _vscroll.SmallChange = 1;

            // số index đầu tiên lớn nhất có thể hiển thị
            int maxFirst = Math.Max(0, total - page);

            // QUAN TRỌNG: công thức dành cho WinForms scrollbar
            // để có thể set Value = maxFirst
            _vscroll.Maximum = maxFirst + _vscroll.LargeChange - 1;

            // giữ nguyên trạng thái/auto scroll
            int desired = _vscroll.Value;
            if (_autoScroll && IsNearBottom()) desired = maxFirst;
            desired = Math.Max(_vscroll.Minimum, Math.Min(desired, maxFirst));
            _vscroll.Value = desired;

            _firstVisibleIndex = _vscroll.Value;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(BackColor);

            int page = PageSize();
            int start = _firstVisibleIndex;
            int end = Math.Min(_items.Count, start + page);

            int xTime = 0;
            int xLevel = xTime + _colTime + 8;
            int xSource = xLevel + _colLevel + 8;
            int xMsg = xSource + _colSource + 8;
            int msgWidth = Math.Max(0, this.ClientSize.Width - _vscroll.Width - xMsg - 4);

            using (var headerBrush = new SolidBrush(Color.FromArgb(245, 245, 245)))
            using (var headerPen = new Pen(Color.FromArgb(220, 220, 220)))
            {
                var hdr = new Rectangle(0, 0, ClientSize.Width - _vscroll.Width, _rowHeight);
                g.FillRectangle(headerBrush, hdr);
                g.DrawLine(headerPen, 0, _rowHeight - 1, hdr.Width, _rowHeight - 1);

                TextRenderer.DrawText(g, "Time", Font, new Rectangle(xTime + 4, 2, _colTime, _rowHeight - 4), Color.Black, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "Level", Font, new Rectangle(xLevel + 4, 2, _colLevel, _rowHeight - 4), Color.Black, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "Source", Font, new Rectangle(xSource + 4, 2, _colSource, _rowHeight - 4), Color.Black, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "Message", Font, new Rectangle(xMsg + 4, 2, msgWidth, _rowHeight - 4), Color.Black, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
            }

            int y = _rowHeight;
            for (int i = start; i < end; i++)
            {
                var it = _items[i];

                Color fore = ForeColor;
                Color back = (i % 2 == 0) ? Color.White : Color.FromArgb(250, 250, 250);
                var lvl = (it.Level ?? "").ToUpperInvariant();
                if (lvl == "TRACE") fore = Color.Gray;
                else if (lvl == "DEBUG") fore = Color.DarkSlateGray;
                else if (lvl == "WARN") fore = Color.OrangeRed;
                else if (lvl == "ERROR") { fore = Color.Red; back = Color.FromArgb(255, 240, 240); }
                else if (lvl == "FATAL") { fore = Color.White; back = Color.Maroon; }

                using (var backBrush = new SolidBrush(back))
                    g.FillRectangle(backBrush, new Rectangle(0, y, ClientSize.Width - _vscroll.Width, _rowHeight));

                var font = (lvl == "ERROR" || lvl == "FATAL") ? _boldFont : Font;

                TextRenderer.DrawText(g, it.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"), font, new Rectangle(xTime + 4, y + 2, _colTime, _rowHeight - 4), fore, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, it.Level ?? "", font, new Rectangle(xLevel + 4, y + 2, _colLevel, _rowHeight - 4), fore, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, it.Source ?? "", font, new Rectangle(xSource + 4, y + 2, _colSource, _rowHeight - 4), fore, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, it.Message ?? "", font, new Rectangle(xMsg + 4, y + 2, msgWidth, _rowHeight - 4), fore, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);

                y += _rowHeight;
            }
            // Thêm margin cho dòng cuối
            y += 10;
         //   AutoScrollMinSize = new Size(0, y);
        }
    }

    /// <summary>
    /// LogsDashboard owner-draw: Filter + Canvas + autosave/load + autoreload on/off + ingest queue
    /// </summary>
    public class LogsDashboard : UserControl
    {
        // ===== Config =====
        private string _storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "logs.json");
        [Browsable(true), Category("LogsDashboard")]
        public string StoragePath
        {
            get { return _storagePath; }
            set
            {
                if (string.Equals(_storagePath, value, StringComparison.OrdinalIgnoreCase)) return;
                _storagePath = value ?? "";
                // rebuild watcher theo state hiện tại
                StopWatcher();
                if (_autoReloadOnChange) StartWatcher();
            }
        }

        private int _autoSaveDelayMs = 1000;
        [Browsable(true), Category("LogsDashboard")]
        public int AutoSaveDelayMs { get { return _autoSaveDelayMs; } set { _autoSaveDelayMs = Math.Max(100, value); if (_saveDebounce != null) _saveDebounce.Interval = _autoSaveDelayMs; } }

        private bool _autoScrollToEnd = true;
        [Browsable(true), Category("LogsDashboard")]
        public bool AutoScrollToEnd
        {
            get { return _autoScrollToEnd; }
            set { _autoScrollToEnd = value; if (_canvas != null) _canvas.AutoScrollToEnd = value; if (value) _canvas.ScrollToEnd(); }
        }

        private int _maxLogCount = 10000;
        [Browsable(true), Category("LogsDashboard")]
        public int MaxLogCount { get { return _maxLogCount; } set { _maxLogCount = Math.Max(1, value); } }

        // ===== Data =====
        private readonly BindingList<LogEntry> _all = new BindingList<LogEntry>(); // nguồn đầy đủ
        private readonly List<LogEntry> _view = new List<LogEntry>();              // dữ liệu đã lọc để vẽ
        private volatile bool _dirty;
        private readonly object _ioLock = new object();

        // ===== Progressive loading =====
        private List<LogEntry> _pendingLoad;
        private int _pendingIndex;
        private readonly Timer _progressiveTimer = new Timer();
        private int _progressiveBatchSize = 800;
        private bool _progressiveLoading;

        // ===== Ingest từ luồng khác =====
        private readonly ConcurrentQueue<LogEntry> _ingestQueue = new ConcurrentQueue<LogEntry>();
        private readonly Timer _ingestTimer = new Timer();
        private int _ingestBatchSize = 400;

        // ===== Auto reload watcher =====
        private FileSystemWatcher _fsw;
        private readonly Timer _reloadDebounce = new Timer();
        private bool _autoReloadOnChange = false;
        private volatile bool _suppressWatcher;
        private DateTime _suppressUntilUtc;

        [Browsable(true), Category("LogsDashboard"), Description("Tự reload khi file logs.json bị thay đổi.")]
        public bool AutoReloadOnChange
        {
            get { return _autoReloadOnChange; }
            set
            {
                if (_autoReloadOnChange == value) return;
                _autoReloadOnChange = value;
                if (_chkAutoReload != null && _chkAutoReload.Checked != value) _chkAutoReload.Checked = value;

                if (!value)
                {
                    StopWatcher(); // dispose hẳn
                }
                else
                {
                    StartWatcher(); // tạo lại mới tinh
                }
            }
        }

        // ===== UI =====
        private FlowLayoutPanel _filter;
        private DateTimePicker _dtpFrom, _dtpTo;
        private ComboBox _cbLevel;
        private TextBox _tbFind;
        private Button _btnToday, _btn7d, _btn30d, _btnExport, _btnImport, _btnClear;
        private CheckBox _chkSaveLog, _chkAutoScroll, _chkAutoReload;
        private readonly LogCanvas _canvas = new LogCanvas();

        // ===== Timers =====
        private readonly Timer _saveDebounce = new Timer();
        private readonly Timer _saveHeartbeat = new Timer();

        public LogsDashboard()
        {
            DoubleBuffered = true;
            BackColor = SystemColors.Window;

            BuildFilterBar();
            BuildCanvas();

            var root = new Panel { Dock = DockStyle.Fill };
            Controls.Add(root);
            root.Controls.Add(_canvas);
            root.Controls.Add(_filter);

            _dtpFrom.ValueChanged += delegate { RebuildView(); };
            _dtpTo.ValueChanged += delegate { RebuildView(); };
            _cbLevel.SelectedIndexChanged += delegate { RebuildView(); };
            _tbFind.TextChanged += delegate { RebuildView(); };
            _chkSaveLog.CheckedChanged += _chkSaveLog_CheckedChanged;
            _chkAutoScroll.CheckedChanged += delegate { AutoScrollToEnd = _chkAutoScroll.Checked; };
            _chkAutoReload.CheckedChanged += delegate {   AutoReloadOnChange = _chkAutoReload.Checked; Global.ParaCommon.IsAutoReload = AutoReloadOnChange; };

            _btnToday.Click += delegate { SetRangeToday(); };
            _btn7d.Click += delegate { SetRangeDays(7); };
            _btn30d.Click += delegate { SetRangeDays(30); };
            _btnExport.Click += delegate { ExportCsv(); };
            _btnImport.Click += delegate { ImportJson(); };
            _btnClear.Click += delegate
            {
                if (Confirm("Xoá toàn bộ log?"))
                {
                    _all.Clear();
                    _view.Clear();
                    _canvas.Clear();
                    _dirty = true;
                    _saveDebounce.Stop(); _saveDebounce.Start();
                }
            };

            _saveDebounce.Interval = _autoSaveDelayMs;
            _saveDebounce.Tick += delegate {
                _saveDebounce.Stop(); SaveNow(); };

            _saveHeartbeat.Interval = 5000;
            _saveHeartbeat.Tick += delegate {
                if (
                _dirty) SaveNow(); };
            _saveHeartbeat.Start();

            try { AppDomain.CurrentDomain.ProcessExit += delegate { try { SaveNow(); } catch { } }; } catch { }

            _progressiveTimer.Interval = 10;
            _progressiveTimer.Tick += ProgressiveTimer_Tick;

            _ingestTimer.Interval = 16;
            _ingestTimer.Tick += IngestTimer_Tick;
            _ingestTimer.Start();

            _reloadDebounce.Interval = 300;
            _reloadDebounce.Tick += delegate
            {
                _reloadDebounce.Stop();
                if (!_autoReloadOnChange) return;                             // guard
                if (_suppressWatcher || DateTime.UtcNow < _suppressUntilUtc) return;
                if (IsHandleCreated) BeginInvoke((Action)TryReloadFromDiskExternal);
            };

            // khởi tạo watcher theo state
            if (_autoReloadOnChange) StartWatcher();

            SetRangeToday();
        }

        private void _chkSaveLog_CheckedChanged(object sender, EventArgs e)
        {
           Global.ParaCommon.IsSaveLog=_chkSaveLog.Checked;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            TryLoadFromDisk();
            RebuildView();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { SaveNow(); } catch { }
                _saveDebounce.Dispose();
                _saveHeartbeat.Dispose();
                _progressiveTimer.Dispose();
                _ingestTimer.Dispose();
                StopWatcher(); // đảm bảo dispose
                _reloadDebounce.Dispose();
            }
            base.Dispose(disposing);
        }

        // ===== Watcher lifecycle (dứt điểm khi OFF) =====
        private void StopWatcher()
        {
            _ingestTimer.Stop();
            _reloadDebounce.Stop();
            _suppressWatcher = false;
            _suppressUntilUtc = DateTime.MinValue;

            if (_progressiveLoading)
            {
                _progressiveLoading = false;
                _progressiveTimer.Stop();
            }

            if (_fsw != null)
            {
                try
                {
                    _fsw.EnableRaisingEvents = false;
                    _fsw.Changed -= OnLogFileChanged;
                    _fsw.Created -= OnLogFileChanged;
                    _fsw.Renamed -= OnLogFileChangedRenamed;
                    _fsw.Dispose();
                }
                catch { }
                finally { _fsw = null; }
            }
        }

        private void StartWatcher()
        {
            if (!_autoReloadOnChange) return;
            try
            {
                _ingestTimer.Start();
                _reloadDebounce.Start();
                var dir = Path.GetDirectoryName(_storagePath);
                var file = Path.GetFileName(_storagePath);
                if (string.IsNullOrEmpty(dir) || string.IsNullOrEmpty(file)) return;

                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                _fsw = new FileSystemWatcher(dir, file);
                _fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
                _fsw.Changed += OnLogFileChanged;
                _fsw.Created += OnLogFileChanged;
                _fsw.Renamed += OnLogFileChangedRenamed;
                _fsw.EnableRaisingEvents = true;
            }
            catch { }
        }

        private void OnLogFileChangedRenamed(object sender, RenamedEventArgs e)
        {
            OnLogFileChanged(sender, e);
        }

        // ===== Public tuning =====
        [Browsable(true), Category("LogsDashboard")] public int ProgressiveBatchSize { get { return _progressiveBatchSize; } set { _progressiveBatchSize = Math.Max(50, value); } }
        [Browsable(true), Category("LogsDashboard")] public int ProgressiveIntervalMs { get { return _progressiveTimer.Interval; } set { _progressiveTimer.Interval = Math.Max(1, value); } }
        [Browsable(true), Category("LogsDashboard")] public int IngestBatchSize { get { return _ingestBatchSize; } set { _ingestBatchSize = Math.Max(50, value); } }
        [Browsable(true), Category("LogsDashboard")] public int IngestIntervalMs { get { return _ingestTimer.Interval; } set { _ingestTimer.Interval = Math.Max(5, value); } }

        // ===== Public API (offload ThreadPool) =====
        public void AddLog(LeveLLog level, string message, string source = "")
        {
            if (Global.ParaCommon != null)
                if (Global.ParaCommon.IsSaveLog)
                    ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(AddLogWorkItem),
                new State { self = this, level = level, message = message, source = source });
        }
        public void AddLog(LogEntry entry)
        {
            if (Global.ParaCommon != null)
                if (Global.ParaCommon.IsSaveLog)
            ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(AddLogEntryWorkItem),
                new StateEntry { self = this, entry = entry });
        }

        private struct State { public LogsDashboard self; public LeveLLog level; public string message; public string source; }
        private struct StateEntry { public LogsDashboard self; public LogEntry entry; }

        private static void AddLogWorkItem(object stateObj)
        {
            State s = (State)stateObj;
            var entry = new LogEntry(DateTime.Now, s.level, s.source ?? string.Empty, s.message ?? string.Empty);
            s.self._ingestQueue.Enqueue(entry);
        }
        private static void AddLogEntryWorkItem(object stateObj)
        {
            StateEntry s = (StateEntry)stateObj;
            var e = s.entry; if (e == null) return;
            if (e.Time == default(DateTime)) e.Time = DateTime.Now;
            if (string.IsNullOrEmpty(e.Level)) e.Level = "INFO";
            s.self._ingestQueue.Enqueue(e);
        }

        // ===== UI build =====
        private void BuildFilterBar()
        {
            _filter = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                AutoScroll = false,
                Padding = new Padding(4),
                Margin = Padding.Empty
            };

            FlowLayoutPanel Pair(string labelText, Control ctl, int ctlWidth)
            {
                var pair = new FlowLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    WrapContents = false,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(2, 2, 8, 2),
                    Padding = Padding.Empty
                };
                var lbl = new Label { Text = labelText, AutoSize = true, Margin = new Padding(0, 6, 4, 0) , Font =new Font("Arial", 14) };
                ctl.Margin = new Padding(0, 2, 0, 2);
                ctl.Width = ctlWidth;
                pair.Controls.Add(lbl);
                pair.Controls.Add(ctl);
                return pair;
            }

            _dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm", Font = new Font("Arial", 14) };
            _dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm", Font = new Font("Arial", 14) };
            _cbLevel = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Arial", 14) };
            _cbLevel.Items.AddRange(new object[] { "All", "TRACE", "DEBUG", "INFO", "WARN", "ERROR", "FATAL"});
            _cbLevel.SelectedIndex = 0;
            _tbFind = new TextBox { Width = 240, Font = new Font("Arial", 14) };

            _btnToday = new Button { Text = "Today", AutoSize = true, Margin = new Padding(6, 3, 0, 3), Font = new Font("Arial", 14) };
            _btn7d = new Button { Text = "7d", AutoSize = true, Margin = new Padding(6, 3, 0, 3), Font = new Font("Arial", 14) };
            _btn30d = new Button { Text = "30d", AutoSize = true, Margin = new Padding(6, 3, 0, 3), Font = new Font("Arial", 14) };
            _btnExport = new Button { Text = "Export CSV", AutoSize = true, Margin = new Padding(6, 3, 0, 3), Font = new Font("Arial", 14) };
            _btnImport = new Button { Text = "Import JSON", AutoSize = true, Margin = new Padding(6, 3, 0, 3),Font = new Font("Arial", 14) };
            _btnClear = new Button { Text = "Clear", AutoSize = true, Margin = new Padding(6, 3, 0, 3), Font = new Font("Arial", 14) };
            if(Global.ParaCommon!=null)
            _chkSaveLog = new CheckBox { Text = "Save Logs", Checked = Global.ParaCommon.IsSaveLog, AutoSize = true, Margin = new Padding(10, 6, 0, 3), Font = new Font("Arial", 14) };
           else
                _chkSaveLog = new CheckBox { Text = "Save Logs", Checked = false, AutoSize = true, Margin = new Padding(10, 6, 0, 3), Font = new Font("Arial", 14) };
            if (Global.ParaCommon != null)
                _chkAutoReload = new CheckBox { Text = "AutoReload", Checked = Global.ParaCommon.IsAutoReload, AutoSize = true, Margin = new Padding(10, 6, 0, 3), Font = new Font("Arial", 14) };
            else
                _chkAutoReload = new CheckBox { Text = "AutoReload", Checked = _autoReloadOnChange, AutoSize = true, Margin = new Padding(10, 6, 0, 3), Font = new Font("Arial", 14) };

            AutoReloadOnChange = Global.ParaCommon.IsAutoReload;
            _chkAutoScroll = new CheckBox { Text = "AutoScroll", Checked = _autoScrollToEnd, AutoSize = true, Margin = new Padding(10, 6, 0, 3), Font = new Font("Arial", 14) };
          
            _filter.Controls.Add(Pair("From:", _dtpFrom, 140));
            _filter.Controls.Add(Pair("To:", _dtpTo, 140));
            _filter.Controls.Add(Pair("Level:", _cbLevel, 110));
            _filter.Controls.Add(Pair("Find:", _tbFind, 150));
            _filter.Controls.Add(_btnToday);
            _filter.Controls.Add(_btn7d);
            _filter.Controls.Add(_btn30d);
            _filter.Controls.Add(_btnExport);
            _filter.Controls.Add(_btnImport);
            _filter.Controls.Add(_btnClear);
            _filter.Controls.Add(_chkSaveLog);
            _filter.Controls.Add(_chkAutoScroll);
            _filter.Controls.Add(_chkAutoReload);

            Controls.Add(_filter);
            _filter.BringToFront();
        }

        private void BuildCanvas()
        {
            _canvas.SetBottomPadding(10);  // mặc định 10px đúng nhu cầu của bạn
            _canvas.Dock = DockStyle.Fill;
            _canvas.SetRowHeight(24);
            _canvas.SetColumns(130, 50, 50);
            _canvas.AutoScrollToEnd = _autoScrollToEnd;
        }

        // ===== Filtering & view rebuild =====
        private bool PassFilter(LogEntry x)
        {
            if (x == null) return false;
            var from = _dtpFrom.Value;
            var to = _dtpTo.Value;
            if (x.Time < from || x.Time > to) return false;

            if (_cbLevel.SelectedIndex > 0)
            {
                var want = _cbLevel.SelectedItem as string ?? string.Empty;
                if (!string.Equals(x.Level ?? string.Empty, want, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            var q = _tbFind.Text == null ? null : _tbFind.Text.Trim();
            if (!string.IsNullOrEmpty(q))
            {
                return (x.Message ?? string.Empty).IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0
                    || (x.Source ?? string.Empty).IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return true;
        }

        private void RebuildView()
        {
            _view.Clear();
            foreach (var it in _all)
                if (PassFilter(it)) _view.Add(it);
            _canvas.ReplaceAll(_view);
        }

        private void AppendFiltered(LogEntry it)
        {
            if (PassFilter(it))
            {
                _view.Add(it);
                _canvas.AppendOne(it);
            }
        }

        private void AppendFilteredBatch(IList<LogEntry> batch)
        {
            if (batch == null || batch.Count == 0) return;
            var passed = new List<LogEntry>(batch.Count);
            foreach (var it in batch)
                if (PassFilter(it)) passed.Add(it);
            if (passed.Count > 0)
            {
                _view.AddRange(passed);
                _canvas.AppendBatch(passed);
            }
        }

        private void SetRangeToday()
        {
            var now = DateTime.Now;
            _dtpFrom.Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            _dtpTo.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            RebuildView();
        }
        private void SetRangeDays(int days)
        {
            var now = DateTime.Now;
            _dtpFrom.Value = now.Date.AddDays(-Math.Max(1, days) + 1);
            _dtpTo.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            RebuildView();
        }

        private void EnforceMaxLogCount()
        {
            int excess = _all.Count - _maxLogCount;
            if (excess <= 0) return;
            _all.RaiseListChangedEvents = false;
            try
            {
                for (int i = 0; i < excess; i++)
                    _all.RemoveAt(0);
            }
            finally { _all.RaiseListChangedEvents = true; }

            RebuildView();
        }

        // ===== Load disk (progressive) =====
        bool IsLoad = false;

        public void TryLoadFromDisk()
        {
            try
            {
                if (IsLoad) return;
                if (!File.Exists(_storagePath)) { IsLoad = true; return; }

                List<LogEntry> list;
                using (var fs = File.OpenRead(_storagePath))
                {
                    var ser = new DataContractJsonSerializer(typeof(List<LogEntry>));
                    list = ser.ReadObject(fs) as List<LogEntry>;
                }
                if (list == null) list = new List<LogEntry>();

                int take = Math.Min(list.Count, _maxLogCount);
                int start = list.Count - take;
                _pendingLoad = take > 0 ? list.GetRange(start, take) : new List<LogEntry>();
                _pendingIndex = 0;

                _all.Clear();
                _view.Clear();
                _canvas.Clear();

                _progressiveLoading = true;
                _progressiveTimer.Stop();
                _progressiveTimer.Start();

                _dirty = false;
            }
            catch { }
            finally { IsLoad = true; }
        }

        private void ProgressiveTimer_Tick(object sender, EventArgs e)
        {
            if (!_progressiveLoading || _pendingLoad == null)
            {
                _progressiveTimer.Stop();
                return;
            }

            int remain = _pendingLoad.Count - _pendingIndex;
            if (remain <= 0)
            {
                _progressiveTimer.Stop();
                _progressiveLoading = false;
                return;
            }

            int batch = Math.Min(_progressiveBatchSize, remain);

            var addedAll = new List<LogEntry>(batch);
            for (int i = 0; i < batch; i++)
            {
                var it = _pendingLoad[_pendingIndex++];
                _all.Add(it);
                addedAll.Add(it);
            }

            AppendFilteredBatch(addedAll);
        }

        // ingest từ queue về UI
        private void IngestTimer_Tick(object sender, EventArgs e)
        {
            if (!_autoReloadOnChange)
            {
                _ingestTimer.Stop();
                return;
            }    
               
            if (!IsHandleCreated || _ingestQueue.IsEmpty) return;

            int taken = 0;
            var addedAll = new List<LogEntry>(Math.Min(_ingestBatchSize, _ingestQueue.Count));

            LogEntry it;
            while (taken < _ingestBatchSize && _ingestQueue.TryDequeue(out it))
            {
                _all.Add(it);
                addedAll.Add(it);
                taken++;
            }

            if (taken > 0)
            {
                EnforceMaxLogCount();
                _dirty = true;
                _saveDebounce.Stop(); _saveDebounce.Start();
                AppendFilteredBatch(addedAll);
                if (_autoScrollToEnd) _canvas.ScrollToEnd();
            }
        }

        // ===== Save ngay (public) =====
        public void SaveNow()
        {
            try
            {
                if (!IsLoad) return;
                lock (_ioLock)
                {
                    _suppressWatcher = true;
                    _suppressUntilUtc = DateTime.UtcNow.AddMilliseconds(800);

                    var dir = Path.GetDirectoryName(_storagePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    var list = _all.ToList();

                    using (var ms = new MemoryStream())
                    {
                        var ser = new DataContractJsonSerializer(typeof(List<LogEntry>));
                        ser.WriteObject(ms, list);
                        ms.Position = 0;

                        string tmp = _storagePath + ".tmp";
                        string bak = _storagePath + ".bak";

                        using (var fs = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            ms.CopyTo(fs);
                            fs.Flush(true);
                        }

                        if (File.Exists(_storagePath))
                            File.Replace(tmp, _storagePath, bak, true);
                        else
                            File.Move(tmp, _storagePath);

                        _dirty = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SaveNow error: " + ex.Message);
            }
            finally
            {
                if (_autoReloadOnChange && _fsw != null)
                {
                    var t = new Timer { Interval = 900 };
                    t.Tick += delegate { t.Stop(); t.Dispose(); _suppressWatcher = false; };
                    t.Start();
                }
                else
                {
                    _suppressWatcher = false;
                    _suppressUntilUtc = DateTime.MinValue;
                }
            }
        }

        // ===== Import/Export =====
        private void ExportCsv()
        {
            try
            {
                using (var sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*", FileName = "logs.csv" })
                {
                    if (sfd.ShowDialog(FindForm()) != DialogResult.OK) return;
                    var sb = new StringBuilder();
                    sb.AppendLine("Time,Level,Source,Message");
                    foreach (var it in _view)
                    {
                        var line = string.Join(",", new[]
                        {
                            EscapeCsv(it.Time.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                            EscapeCsv(it.Level),
                            EscapeCsv(it.Source),
                            EscapeCsv(it.Message)
                        });
                        sb.AppendLine(line);
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export CSV thất bại:\n" + ex.Message, "LogsDashboard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ImportJson()
        {
            try
            {
                using (var ofd = new OpenFileDialog { Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*" })
                {
                    if (ofd.ShowDialog(FindForm()) != DialogResult.OK) return;

                    List<LogEntry> list;
                    using (var fs = File.OpenRead(ofd.FileName))
                    {
                        var ser = new DataContractJsonSerializer(typeof(List<LogEntry>));
                        list = ser.ReadObject(fs) as List<LogEntry>;
                    }
                    if (list == null) return;

                    var addedAll = new List<LogEntry>(list.Count);
                    foreach (var it in list)
                    {
                        _all.Add(it);
                        addedAll.Add(it);
                    }

                    _dirty = true;
                    EnforceMaxLogCount();
                    _saveDebounce.Stop(); _saveDebounce.Start();

                    AppendFilteredBatch(addedAll);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import JSON thất bại:\n" + ex.Message, "LogsDashboard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ===== Helpers =====
        private static string EscapeCsv(string s)
        {
            if (s == null) return "";
            if (s.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0) return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
        private static bool Confirm(string msg)
        {
            return MessageBox.Show(msg, "LogsDashboard", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
        }

        private void OnLogFileChanged(object sender, EventArgs e)
        {
            if (!_autoReloadOnChange) return;                             // guard
            if (_suppressWatcher || DateTime.UtcNow < _suppressUntilUtc) return;
            _reloadDebounce.Stop();
            _reloadDebounce.Start();
        }

        private void TryReloadFromDiskExternal()
        {
            if (!_autoReloadOnChange) return;                             // guard
            try
            {
                if (!File.Exists(_storagePath)) return;

                List<LogEntry> list;
                using (var fs = File.Open(_storagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var ser = new DataContractJsonSerializer(typeof(List<LogEntry>));
                    list = ser.ReadObject(fs) as List<LogEntry>;
                }
                if (list == null) list = new List<LogEntry>();

                int take = Math.Min(list.Count, _maxLogCount);
                int start = list.Count - take;
                _pendingLoad = take > 0 ? list.GetRange(start, take) : new List<LogEntry>();
                _pendingIndex = 0;

                _all.Clear();
                _view.Clear();
                _canvas.Clear();

                _progressiveLoading = true;
                _progressiveTimer.Stop();
                _progressiveTimer.Start();
            }
            catch { }
        }
    }
}
