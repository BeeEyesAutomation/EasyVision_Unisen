using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BeeInterface
{
    /// <summary>
    /// Lưu/khôi phục layout (Width/Height/Bounds + SplitterDistance).
    /// - Auto-save (debounce) sau thay đổi.
    /// - Lock/Unlock SplitContainer (message filter chặn cả client & non-client).
    /// - Load trễ sau Form.Shown (LoadDelayMs).
    /// - QUÉT TỪ FORM/TOPLEVEL để khoá hết splitter trong toàn bộ cây (kể cả UserControl lồng nhau).
    /// </summary>
    public sealed class LayoutPersistence
    {
        private readonly Control _host;
        private readonly string _key;
        private bool _hooked;
        private string _formNameCache;
        private bool _applying;

        // SplitContainer tracking
        private readonly HashSet<SplitContainer> _knownSplits = new HashSet<SplitContainer>();
        private readonly Dictionary<SplitContainer, int> _lockedDistances = new Dictionary<SplitContainer, int>();
        private bool _splitterLocked;
        private readonly SplitterDragMessageFilter _filter = new SplitterDragMessageFilter();

        // Auto-save debounce
        private readonly Timer _saveTimer;
        private bool _dirtyPending;
        private int _autoSaveDebounceMs = 400;
        public int AutoSaveDebounceMs
        {
            get => _autoSaveDebounceMs;
            set { _autoSaveDebounceMs = Math.Max(0, value); _saveTimer.Interval = Math.Max(1, _autoSaveDebounceMs); }
        }

        // Load trễ sau Shown
        public bool IsLoad = false;
        private readonly Timer _loadTimer;
        private int _loadDelayMs;
        public int LoadDelayMs
        {
            get => _loadDelayMs;
            set { _loadDelayMs = Math.Max(0, value); if (_loadTimer != null) _loadTimer.Interval = Math.Max(1, _loadDelayMs); }
        }

        // Khoá/mở khoá splitter (sâu)
        public bool SplitterLocked
        {
            get => _splitterLocked;
            set { if (_splitterLocked != value) { if (value) LockAllSplittersDeep(); else UnlockSplitters(); } }
        }

        public LayoutPersistence(Control host, string key = null)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _key = string.IsNullOrWhiteSpace(key)
                ? (string.IsNullOrWhiteSpace(host.Name) ? host.GetType().Name : host.Name)
                : key;

            // Debounce saver
            _saveTimer = new Timer { Interval = Math.Max(1, _autoSaveDebounceMs) };
            _saveTimer.Tick += (s, e) =>
            {
                _saveTimer.Stop();
                if (_dirtyPending && !_applying) { _dirtyPending = false; SaveNow(); }
            };

            // Load delay timer
            _loadTimer = new Timer { Interval = 1 };
            _loadTimer.Tick += (s, e) => { _loadTimer.Stop(); if (!_applying) LoadNow(); };

            // Theo dõi vòng đời/child
            _host.HandleCreated += (_, __) => TryHookForm();
            _host.ParentChanged += (_, __) => TryHookForm();
            _host.ControlAdded += Host_ControlAddedRecursive;
            _host.ControlRemoved += Host_ControlRemovedRecursive;

            // Theo dõi thay đổi chung của host
            _host.Resize += (_, __) => SaveSoon();
            _host.Layout += (_, __) => SaveSoon();

            // Watch mọi control hiện có (phạm vi _host) để lưu layout
            WatchTree(_host, attach: true);

            // QUÉT SPLITTER từ Form/TopLevel ngay từ đầu (toàn cây)
            HookAllSplitContainers(GetScanRoot());

            TryHookForm();
        }

        // ===== Public API =====
        public void EnableAuto() => TryHookForm(force: true);

        public void LoadNow()
        {
            if (_applying) return;
            _applying = true;
            try
            {
                var path = GetLayoutFilePath();
                if (!File.Exists(path)) { IsLoad = true; return; }

                var map = ReadMap(path);
                _host.SuspendLayout();
                ApplyRecursive(_host, _host, map);
                _host.ResumeLayout(true);
                _host.PerformLayout();
                _host.Invalidate();
                _host.Update();

                // Sau khi apply, rescan để chắc đã hook đủ mọi splitter
                RescanSplitContainers();

                IsLoad = true;
            }
            finally { _applying = false; }
        }

        public void SaveNow()
        {
            if (_applying || !IsLoad) return;

            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            CaptureRecursive(_host, _host, map);

            foreach (var sp in FindAllSplitContainers(_host)) // chỉ lưu layout thuộc phạm vi _host
            {
                string path = BuildPath(_host, sp);
                map[path + "|Split"] = sp.SplitterDistance.ToString();
            }
            WriteMap(GetLayoutFilePath(), map);
        }

        public void SaveSoon()
        {
            if (_applying || !IsLoad) return;
            _dirtyPending = true;
            _saveTimer.Stop();
            _saveTimer.Start();
        }

        // ===== Lock/Unlock (sâu) =====
        public void LockAllSplittersDeep(bool scanFromFormRoot = true)
        {
            var root = scanFromFormRoot ? GetScanRoot() : _host;

            // Rescan trước khi khóa
            foreach (var sp in new List<SplitContainer>(_knownSplits)) UnhookSplit(sp);
            HookAllSplitContainers(root);

            _splitterLocked = true;

            _filter.Enable();
            foreach (var sp in _knownSplits) _filter.Track(sp);

            foreach (var sp in _knownSplits)
            {
                if (!_lockedDistances.ContainsKey(sp))
                    _lockedDistances[sp] = sp.SplitterDistance;

                sp.IsSplitterFixed = true;
                sp.Cursor = Cursors.Default;
                sp.Panel1.Cursor = Cursors.Default;
                sp.Panel2.Cursor = Cursors.Default;
            }
        }

        public void UnlockSplitters()
        {
            _splitterLocked = false;

            foreach (var sp in _knownSplits) _filter.Untrack(sp);
            _filter.Disable();

            foreach (var sp in _knownSplits)
            {
                sp.IsSplitterFixed = false;
                sp.Cursor = Cursors.VSplit;
                sp.Panel1.Cursor = Cursors.Default;
                sp.Panel2.Cursor = Cursors.Default;
            }
            _lockedDistances.Clear();
        }

        // ===== Hook Form (load sau Shown, rescan toàn cây) =====
        private void TryHookForm(bool force = false)
        {
            if (_hooked && !force) return;
            var form = _host.FindForm();
            if (form == null) return;

            _hooked = true;
            _formNameCache = string.IsNullOrWhiteSpace(form.Name) ? "App" : form.Name;

            bool loaded = false;
            form.Shown += (s, e) =>
            {
                if (loaded) return;
                loaded = true;

                RescanSplitContainers(); // quét lại sau khi UI dựng xong

                form.BeginInvoke((MethodInvoker)(() =>
                {
                    if (_loadDelayMs <= 0) LoadNow();
                    else { _loadTimer.Interval = Math.Max(1, _loadDelayMs); _loadTimer.Start(); }
                }));
            };

            // Bắt splitter thêm/loại bỏ động ở cấp Form
            form.ControlAdded += (_, ce) => HookAllSplitContainers(ce.Control);
            form.ControlRemoved += (_, ce) => UnhookAllSplitContainers(ce.Control);

            form.Disposed += (_, __) =>
            {
                try { _loadTimer?.Stop(); } catch { }
                try { _saveTimer?.Stop(); } catch { }
                try { _filter?.Disable(); } catch { }
            };
        }

        // ===== Track động trong _host (phục vụ save layout) =====
        private void Host_ControlAddedRecursive(object sender, ControlEventArgs e)
        {
            WatchTree(e.Control, attach: true);
            HookAllSplitContainers(e.Control); // nếu nhánh này có splitter

            e.Control.ControlAdded += Host_ControlAddedRecursive;
            e.Control.ControlRemoved += Host_ControlRemovedRecursive;

            SaveSoon();
        }

        private void Host_ControlRemovedRecursive(object sender, ControlEventArgs e)
        {
            UnwatchTree(e.Control);
            UnhookAllSplitContainers(e.Control);

            e.Control.ControlAdded -= Host_ControlAddedRecursive;
            e.Control.ControlRemoved -= Host_ControlRemovedRecursive;

            SaveSoon();
        }

        // ===== Hook/Unhook splitter =====
        private void HookAllSplitContainers(Control root)
        {
            foreach (var sp in FindAllSplitContainers(root)) HookSplit(sp);
        }

        private void UnhookAllSplitContainers(Control root)
        {
            foreach (var sp in FindAllSplitContainers(root)) UnhookSplit(sp);
        }

        public void RescanSplitContainers()
        {
            foreach (var sp in new List<SplitContainer>(_knownSplits)) UnhookSplit(sp);
            HookAllSplitContainers(GetScanRoot());
        }

        private void HookSplit(SplitContainer sp)
        {
            if (_knownSplits.Contains(sp)) return;
            _knownSplits.Add(sp);

            sp.SplitterMoving += Sp_SplitterMoving;
            sp.SplitterMoved += Sp_SplitterMoved;

            // theo dõi panel bên trong để bắt thay đổi layout sâu
            WatchSplitterPanels(sp, attach: true);

            if (_splitterLocked)
            {
                if (!_lockedDistances.ContainsKey(sp))
                    _lockedDistances[sp] = sp.SplitterDistance;
                sp.IsSplitterFixed = true;
                sp.Cursor = Cursors.Default;
                _filter.Track(sp);
            }
        }

        private void UnhookSplit(SplitContainer sp)
        {
            if (!_knownSplits.Remove(sp)) return;
            try
            {
                sp.SplitterMoving -= Sp_SplitterMoving;
                sp.SplitterMoved -= Sp_SplitterMoved;
            }
            catch { }

            WatchSplitterPanels(sp, attach: false);
            _lockedDistances.Remove(sp);
            _filter.Untrack(sp);
        }

        private void Sp_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            if (_splitterLocked)
            {
                e.Cancel = true;
                var sp = (SplitContainer)sender;
                if (_lockedDistances.TryGetValue(sp, out int d))
                    sp.SplitterDistance = d;
            }
        }

        private void Sp_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (_splitterLocked)
            {
                var sp = (SplitContainer)sender;
                if (_lockedDistances.TryGetValue(sp, out int d))
                    sp.SplitterDistance = d;
                return;
            }

            // clamp + auto-save
            var s = (SplitContainer)sender;
            int min1 = s.Panel1MinSize;
            int min2 = s.Panel2MinSize;
            int space = (s.Orientation == Orientation.Vertical ? s.Width : s.Height);
            int max = space - s.SplitterWidth - min2;
            int d2 = Math.Max(min1, Math.Min(max, s.SplitterDistance));
            if (d2 != s.SplitterDistance) s.SplitterDistance = d2;

            SaveSoon();
        }

        // ===== Watch thay đổi layout/size/dock =====
        private void WatchTree(Control root, bool attach)
        {
            if (attach) AttachWatch(root); else DetachWatch(root);
            foreach (Control ch in root.Controls) WatchTree(ch, attach);
        }
        private void UnwatchTree(Control root) => WatchTree(root, attach: false);

        private void AttachWatch(Control c)
        {
            c.SizeChanged += OnChildChanged;
            c.LocationChanged += OnChildChanged;
            c.DockChanged += OnChildChanged;
            c.Layout += OnChildLayout;
        }
        private void DetachWatch(Control c)
        {
            try
            {
                c.SizeChanged -= OnChildChanged;
                c.LocationChanged -= OnChildChanged;
                c.DockChanged -= OnChildChanged;
                c.Layout -= OnChildLayout;
            }
            catch { }
        }

        private void WatchSplitterPanels(SplitContainer sp, bool attach)
        {
            if (sp == null) return;
            if (attach)
            {
                AttachWatch(sp.Panel1);
                AttachWatch(sp.Panel2);
                WatchTree(sp.Panel1, attach: true);
                WatchTree(sp.Panel2, attach: true);
            }
            else
            {
                UnwatchTree(sp.Panel1);
                UnwatchTree(sp.Panel2);
                DetachWatch(sp.Panel1);
                DetachWatch(sp.Panel2);
            }
        }

        private void OnChildChanged(object sender, EventArgs e)
        {
            if (_applying) return;
            SaveSoon();
        }
        private void OnChildLayout(object sender, LayoutEventArgs e)
        {
            if (_applying) return;
            SaveSoon();
        }

        // ===== Capture/Apply =====
        private static void CaptureRecursive(Control root, Control c, Dictionary<string, string> map)
        {
            string path = BuildPath(root, c);
            switch (c.Dock)
            {
                case DockStyle.Left:
                case DockStyle.Right:
                    map[path + "|W"] = c.Width.ToString();
                    break;
                case DockStyle.Top:
                case DockStyle.Bottom:
                    map[path + "|H"] = c.Height.ToString();
                    break;
                case DockStyle.None:
                    map[path + "|B"] = $"{c.Left},{c.Top},{c.Width},{c.Height}";
                    break;
            }
            foreach (Control ch in c.Controls) CaptureRecursive(root, ch, map);
        }

        private static void ApplyRecursive(Control root, Control c, Dictionary<string, string> map)
        {
            string path = BuildPath(root, c);

            if (c.Dock == DockStyle.Left || c.Dock == DockStyle.Right)
            {
                if (map.TryGetValue(path + "|W", out var ws) && int.TryParse(ws, out int w))
                {
                    w = Math.Max(c.MinimumSize.Width, w);
                    if (c.MaximumSize.Width > 0) w = Math.Min(c.MaximumSize.Width, w);
                    c.Width = w;
                }
            }
            else if (c.Dock == DockStyle.Top || c.Dock == DockStyle.Bottom)
            {
                if (map.TryGetValue(path + "|H", out var hs) && int.TryParse(hs, out int h))
                {
                    h = Math.Max(c.MinimumSize.Height, h);
                    if (c.MaximumSize.Height > 0) h = Math.Min(c.MaximumSize.Height, h);
                    c.Height = h;
                }
            }
            else if (c.Dock == DockStyle.None)
            {
                if (map.TryGetValue(path + "|B", out var bs))
                {
                    var p = bs.Split(',');
                    if (p.Length == 4 &&
                        int.TryParse(p[0], out int x) &&
                        int.TryParse(p[1], out int y) &&
                        int.TryParse(p[2], out int w2) &&
                        int.TryParse(p[3], out int h2))
                    {
                        w2 = Math.Max(c.MinimumSize.Width, w2);
                        h2 = Math.Max(c.MinimumSize.Height, h2);
                        if (c.MaximumSize.Width > 0) w2 = Math.Min(c.MaximumSize.Width, w2);
                        if (c.MaximumSize.Height > 0) h2 = Math.Min(c.MaximumSize.Height, h2);
                        c.SetBounds(x, y, w2, h2);
                    }
                }
            }

            if (c is SplitContainer sp)
            {
                string k = path + "|Split";
                if (map.TryGetValue(k, out var s) && int.TryParse(s, out int dist))
                {
                    int min1 = sp.Panel1MinSize;
                    int min2 = sp.Panel2MinSize;
                    int space = (sp.Orientation == Orientation.Vertical ? sp.Width : sp.Height);
                    int max = space - sp.SplitterWidth - min2;
                    if (max > 0)
                    {
                        dist = Math.Max(min1, Math.Min(max, dist));
                        sp.SplitterDistance = dist;
                    }
                }
            }

            foreach (Control ch in c.Controls) ApplyRecursive(root, ch, map);
        }

        // ===== IO & utils =====
        private Dictionary<string, string> ReadMap(string path)
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in File.ReadAllLines(path, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                int p = line.IndexOf('=');
                if (p <= 0) continue;
                string k = line.Substring(0, p).Trim();
                string v = line.Substring(p + 1).Trim();
                map[k] = v;
            }
            return map;
        }

        private void WriteMap(string path, Dictionary<string, string> map)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var sb = new StringBuilder();
            sb.AppendLine("# BeeLayouts - kv");
            foreach (var kv in map) sb.AppendLine(kv.Key + "=" + kv.Value);
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private string GetLayoutFilePath()
        {
            var formName = "App"; // hoặc _formNameCache
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BeeInterface");
            var file = $"{formName}.{_key}.layout";
            return Path.Combine(dir, file);
        }

        private static IEnumerable<SplitContainer> FindAllSplitContainers(Control root)
        {
            var list = new List<SplitContainer>();
            void walk(Control cc)
            {
                if (cc is SplitContainer sp) list.Add(sp);
                foreach (Control ch in cc.Controls) walk(ch);
            }
            walk(root);
            return list;
        }

        private static string BuildPath(Control root, Control c)
        {
            var stack = new Stack<string>();
            var cur = c;
            while (cur != null && cur != root.Parent)
            {
                string seg = !string.IsNullOrWhiteSpace(cur.Name)
                    ? cur.Name
                    : cur.GetType().Name + "#" + IndexInParent(cur);
                stack.Push(seg);
                if (cur == root) break;
                cur = cur.Parent;
            }
            return string.Join("/", stack);
        }
        private static int IndexInParent(Control c)
        {
            if (c?.Parent == null) return -1;
            int i = 0; foreach (Control x in c.Parent.Controls) { if (x == c) return i; i++; }
            return -1;
        }

        private Control GetScanRoot()
            => (Control)(_host.FindForm() ?? _host.TopLevelControl ?? _host);

        // ===== Message filter chặn chuột trên dải splitter =====
        private sealed class SplitterDragMessageFilter : IMessageFilter
        {
            private readonly HashSet<SplitContainer> _tracked = new HashSet<SplitContainer>();
            private bool _enabled;

            public void Enable() { if (_enabled) return; Application.AddMessageFilter(this); _enabled = true; }
            public void Disable() { if (!_enabled) return; Application.RemoveMessageFilter(this); _enabled = false; }
            public void Track(SplitContainer sp) => _tracked.Add(sp);
            public void Untrack(SplitContainer sp) => _tracked.Remove(sp);

            public bool PreFilterMessage(ref Message m)
            {
                if (!_enabled || _tracked.Count == 0) return false;

                // Client
                const int WM_MOUSEMOVE = 0x0200;
                const int WM_LBUTTONDOWN = 0x0201;
                const int WM_LBUTTONDBLCLK = 0x0203;
                // Non-client
                const int WM_NCMOUSEMOVE = 0x00A0;
                const int WM_NCLBUTTONDOWN = 0x00A1;
                const int WM_NCLBUTTONDBLCLK = 0x00A3;

                Point screenPt = Control.MousePosition;

                foreach (var sp in _tracked)
                {
                    if (!sp.IsHandleCreated || !sp.Visible) continue;
                    Rectangle split = sp.RectangleToScreen(sp.SplitterRectangle);
                    if (!split.Contains(screenPt)) continue;

                    switch (m.Msg)
                    {
                        case WM_LBUTTONDOWN:
                        case WM_LBUTTONDBLCLK:
                            return true;
                        case WM_MOUSEMOVE:
                            if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left) return true;
                            break;
                        case WM_NCLBUTTONDOWN:
                        case WM_NCLBUTTONDBLCLK:
                            return true;
                        case WM_NCMOUSEMOVE:
                            if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left) return true;
                            break;
                    }
                }
                return false;
            }
        }
    }
}
