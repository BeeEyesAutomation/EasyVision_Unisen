using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BeeInterface
{
    /// <summary>
    /// Lưu/khôi phục layout cho cây control trong host (Width/Height/Bounds + SplitterDistance)
    /// + Lock/Unlock kéo SplitContainer (chặn cứng bằng message filter).
    /// Tự LƯU ngay sau thay đổi (debounce), không chờ Closing form.
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

        // Auto-save sau thay đổi (debounce)
        private readonly Timer _saveTimer;
        private bool _dirtyPending;

        /// <summary>Thời gian trễ gom thay đổi trước khi ghi file (ms).</summary>
        public int AutoSaveDebounceMs
        {
            get => _autoSaveDebounceMs;
            set
            {
                _autoSaveDebounceMs = Math.Max(0, value);
                _saveTimer.Interval = _autoSaveDebounceMs;
            }
        }
        private int _autoSaveDebounceMs = 400;

        /// <summary>Khoá/mở khoá kéo splitter.</summary>
        public bool SplitterLocked
        {
            get => _splitterLocked;
            set
            {
                if (_splitterLocked == value) return;
                if (value) LockSplitters();
                else UnlockSplitters();
            }
        }

        public LayoutPersistence(Control host, string key = null)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _key = string.IsNullOrWhiteSpace(key)
                ? (string.IsNullOrWhiteSpace(host.Name) ? host.GetType().Name : host.Name)
                : key;

            // Debounce saver
            _saveTimer = new Timer { Interval = _autoSaveDebounceMs };
            _saveTimer.Tick += (s, e) =>
            {
                _saveTimer.Stop();
                if (_dirtyPending && !_applying) { _dirtyPending = false; SaveNow(); }
            };

            // Theo dõi vòng đời/child
            _host.HandleCreated += (_, __) => TryHookForm();
            _host.ParentChanged += (_, __) => TryHookForm();
            _host.ControlAdded += Host_ControlAddedRecursive;
            _host.ControlRemoved += Host_ControlRemovedRecursive;

            // Theo dõi thay đổi chung của host
            _host.Resize += (_, __) => SaveSoon();
            _host.Layout += (_, __) => SaveSoon();

            // Gắn watcher cho mọi control hiện có
            WatchTree(_host, attach: true);

            // SplitContainer hiện có
            HookAllSplitContainers(_host);

            TryHookForm();
        }

        // ====== Public API ======
        public void EnableAuto() => TryHookForm(force: true);

        public void LoadNow()
        {
            if (_applying) return;
            _applying = true;
            try
            {
                var path = GetLayoutFilePath();
                if (!File.Exists(path)) return;

                var map = ReadMap(path);
                _host.SuspendLayout();
                ApplyRecursive(_host, _host, map);
                _host.ResumeLayout(true);
                _host.PerformLayout();
                _host.Invalidate();
                _host.Update();
            }
            finally { _applying = false; }
        }

        /// <summary>Lưu ngay (không debounce).</summary>
        public void SaveNow()
        {
            if (_applying) return;

            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            CaptureRecursive(_host, _host, map);

            foreach (var sp in FindAllSplitContainers(_host))
            {
                string path = BuildPath(_host, sp);
                map[path + "|Split"] = sp.SplitterDistance.ToString();
            }
            WriteMap(GetLayoutFilePath(), map);
        }

        /// <summary>Đánh dấu cần lưu, sẽ lưu sau AutoSaveDebounceMs ms.</summary>
        public void SaveSoon()
        {
            if (_applying) return;
            _dirtyPending = true;
            _saveTimer.Stop();
            _saveTimer.Start();
        }

        // ===== Splitter lock/unlock (cứng) =====
        public void LockSplitters()
        {
            _splitterLocked = true;

            _filter.Enable();
            foreach (var sp in _knownSplits) _filter.Track(sp);

            foreach (var sp in _knownSplits)
            {
                if (!_lockedDistances.ContainsKey(sp))
                    _lockedDistances[sp] = sp.SplitterDistance;

                sp.IsSplitterFixed = true;
                sp.Cursor = Cursors.Default;
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
            }
            _lockedDistances.Clear();
        }

        // ===== Hook Form (chỉ load sau Shown; KHÔNG save khi Closing) =====
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
                LoadNow(); // có size thật rồi mới apply
            };
            // KHÔNG đăng ký FormClosing -> không save lúc đóng nữa
        }

        // ===== Track splitters & controls động =====
        private void Host_ControlAddedRecursive(object sender, ControlEventArgs e)
        {
            WatchTree(e.Control, attach: true);
            HookAllSplitContainers(e.Control);

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

        private void HookAllSplitContainers(Control root)
        {
            foreach (var sp in FindAllSplitContainers(root))
                HookSplit(sp);
        }
        private void UnhookAllSplitContainers(Control root)
        {
            foreach (var sp in FindAllSplitContainers(root))
                UnhookSplit(sp);
        }

        private void HookSplit(SplitContainer sp)
        {
            if (_knownSplits.Contains(sp)) return;
            _knownSplits.Add(sp);

            sp.SplitterMoving += Sp_SplitterMoving;
            sp.SplitterMoved += Sp_SplitterMoved;

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

            // kẹp & auto-save (debounce)
            var s = (SplitContainer)sender;
            int min1 = s.Panel1MinSize;
            int min2 = s.Panel2MinSize;
            int space = (s.Orientation == Orientation.Vertical ? s.Width : s.Height);
            int max = space - s.SplitterWidth - min2;
            int d2 = Math.Max(min1, Math.Min(max, s.SplitterDistance));
            if (d2 != s.SplitterDistance) s.SplitterDistance = d2;

            SaveSoon();
        }

        // ====== Watch controls thay đổi (Size/Location/Dock) -> SaveSoon ======
        private void WatchTree(Control root, bool attach)
        {
            if (attach) AttachWatch(root);
            else DetachWatch(root);

            foreach (Control ch in root.Controls)
                WatchTree(ch, attach);
        }
        private void UnwatchTree(Control root) => WatchTree(root, attach: false);

        private void AttachWatch(Control c)
        {
            c.SizeChanged += OnChildChanged;
            c.LocationChanged += OnChildChanged;
            c.DockChanged += OnChildChanged;
            // nếu cần: c.VisibleChanged += OnChildChanged;
        }
        private void DetachWatch(Control c)
        {
            try
            {
                c.SizeChanged -= OnChildChanged;
                c.LocationChanged -= OnChildChanged;
                c.DockChanged -= OnChildChanged;
                // c.VisibleChanged -= OnChildChanged;
            }
            catch { }
        }
        private void OnChildChanged(object sender, EventArgs e)
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
            foreach (Control ch in c.Controls)
                CaptureRecursive(root, ch, map);
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

            foreach (Control ch in c.Controls)
                ApplyRecursive(root, ch, map);
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
            foreach (var kv in map)
                sb.AppendLine(kv.Key + "=" + kv.Value);
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private string GetLayoutFilePath()
        {
            var formName = "App";// string.IsNullOrWhiteSpace(_formNameCache) ? "App" : _formNameCache;
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

        // ====== Message filter chặn chuột trên dải splitter ======
        private sealed class SplitterDragMessageFilter : IMessageFilter
        {
            private readonly HashSet<SplitContainer> _tracked = new HashSet<SplitContainer>();
            private bool _enabled;

            public void Enable()
            {
                if (_enabled) return;
                Application.AddMessageFilter(this);
                _enabled = true;
            }
            public void Disable()
            {
                if (!_enabled) return;
                Application.RemoveMessageFilter(this);
                _enabled = false;
            }
            public void Track(SplitContainer sp) => _tracked.Add(sp);
            public void Untrack(SplitContainer sp) => _tracked.Remove(sp);

            public bool PreFilterMessage(ref Message m)
            {
                if (!_enabled || _tracked.Count == 0) return false;

                const int WM_LBUTTONDOWN = 0x0201;
                const int WM_LBUTTONDBLCLK = 0x0203;
                const int WM_MOUSEMOVE = 0x0200;

                Point screenPt = Control.MousePosition;

                foreach (var sp in _tracked)
                {
                    if (!sp.IsHandleCreated || !sp.Visible) continue;
                    Rectangle split = sp.RectangleToScreen(sp.SplitterRectangle);
                    if (!split.Contains(screenPt)) continue;

                    if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONDBLCLK)
                        return true;

                    if (m.Msg == WM_MOUSEMOVE && (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
                        return true;
                }
                return false;
            }
        }
    }
}
