using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BeeInterface
{
    public enum DockSide { Left, Right, Top, Bottom }
    public enum ZoneLayout { Stack, Accordion }

    [DesignerCategory("Code")]
    public class MultiDockHost : Panel
    {
        // ---------- Center ----------
        private readonly Panel _center = new Panel { Dock = DockStyle.Fill };

        // ---------- 4 zones ----------
        private readonly DockZone _leftZone;
        private readonly DockZone _rightZone;
        private readonly DockZone _topZone;
        private readonly DockZone _bottomZone;

        [Category("DockHost")] public int MinCenterWidth { get; set; } = 240;
        [Category("DockHost")] public int MinCenterHeight { get; set; } = 180;

        // API Center
        [Browsable(false)]
        public Control Center
        {
            get { return _center.Controls.Count > 0 ? _center.Controls[0] : null; }
            set
            {
                using (new Freeze(this))
                {
                    _center.Controls.Clear();
                    if (value != null)
                    {
                        value.Dock = DockStyle.Fill;
                        _center.Controls.Add(value);
                    }
                }
            }
        }

        // Layout per zone
        [Category("DockHost")] public ZoneLayout LeftLayout { get { return _leftZone.LayoutMode; } set { _leftZone.LayoutMode = value; } }
        [Category("DockHost")] public ZoneLayout RightLayout { get { return _rightZone.LayoutMode; } set { _rightZone.LayoutMode = value; } }
        [Category("DockHost")] public ZoneLayout TopLayout { get { return _topZone.LayoutMode; } set { _topZone.LayoutMode = value; } }
        [Category("DockHost")] public ZoneLayout BottomLayout { get { return _bottomZone.LayoutMode; } set { _bottomZone.LayoutMode = value; } }

        // Splitter & Header theme
        [Category("DockHost")] public int SplitterThickness { get; set; } = 6;
        [Category("DockHost")] public Color SplitterColor { get; set; } = Color.FromArgb(70, 0, 0, 0);
        [Category("DockHost")] public Color SplitterHoverColor { get; set; } = Color.FromArgb(130, 0, 0, 0);

        [Category("DockHost")] public int HeaderHeight { get; set; } = 28;
        [Category("DockHost")] public Font HeaderFont { get; set; }
        [Category("DockHost")] public Color HeaderBack { get; set; } = Color.FromArgb(245, 245, 245);
        [Category("DockHost")] public Color HeaderBorder { get; set; } = Color.FromArgb(210, 210, 210);
        [Category("DockHost")] public Color HeaderText { get; set; } = Color.Black;

        public MultiDockHost()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            _leftZone = new DockZone(this, DockSide.Left);
            _rightZone = new DockZone(this, DockSide.Right);
            _topZone = new DockZone(this, DockSide.Top);
            _bottomZone = new DockZone(this, DockSide.Bottom);

            Controls.Add(_center);
            Controls.Add(_leftZone);
            Controls.Add(_rightZone);
            Controls.Add(_topZone);
            Controls.Add(_bottomZone);

            _leftZone.LayoutMode = ZoneLayout.Stack;
            _rightZone.LayoutMode = ZoneLayout.Stack;
            _topZone.LayoutMode = ZoneLayout.Stack;
            _bottomZone.LayoutMode = ZoneLayout.Stack;

            EnableDbRecursive(this);
        }

        // ---------- Public API ----------
        public void AddDock(DockSide side, string key, Control content, string title = null,
                            int size = 220, int minSize = 120, bool collapsed = false, int index = -1)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            if (content == null) throw new ArgumentNullException("content");
            DockZone zone = GetZone(side);

            using (new Freeze(this))
            {
                DockItem item = new DockItem();
                item.Key = key;
                item.Title = string.IsNullOrEmpty(title) ? key : title;
                item.Content = content;
                item.Size = Math.Max(minSize, size);
                item.MinSize = Math.Max(24, minSize);
                item.Collapsed = collapsed;

                zone.AddItem(item, index);
                PerformLayout();
                Invalidate();
            }
        }

        public bool RemoveDock(string key)
        {
            DockZone[] zones = new DockZone[] { _leftZone, _rightZone, _topZone, _bottomZone };
            for (int i = 0; i < zones.Length; i++)
            {
                if (zones[i].RemoveItem(key))
                {
                    PerformLayout();
                    Invalidate();
                    return true;
                }
            }
            return false;
        }

        public bool ToggleCollapsed(string key)
        {
            DockZone zone; DockItem item;
            if (!TryFindItem(key, out zone, out item)) return false;
            using (new Freeze(this)) { item.Collapsed = !item.Collapsed; zone.RequestLayout(); }
            return true;
        }

        public bool SetDockVisible(string key, bool visible)
        {
            DockZone zone; DockItem item;
            if (!TryFindItem(key, out zone, out item)) return false;
            using (new Freeze(this)) { item.Hidden = !visible; zone.RequestLayout(); }
            return true;
        }

        public bool SetDockSize(string key, int size)
        {
            DockZone zone; DockItem item;
            if (!TryFindItem(key, out zone, out item)) return false;
            using (new Freeze(this)) { item.Size = Math.Max(item.MinSize, size); zone.RequestLayout(); }
            return true;
        }

        public bool MoveDock(string key, int newIndex)
        {
            DockZone zone; DockItem item;
            if (!TryFindItem(key, out zone, out item)) return false;
            using (new Freeze(this)) { zone.MoveItem(item, newIndex); }
            return true;
        }

        public void SetZoneLayout(DockSide side, ZoneLayout layout)
        {
            GetZone(side).LayoutMode = layout;
        }

        // ---------- Layout host ----------
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            Rectangle rc = ClientRectangle;

            // Top / Bottom
            int topH = _topZone.MeasurePrimary();
            int botH = _bottomZone.MeasurePrimary();

            int maxTop = Math.Max(0, rc.Height - MinCenterHeight - botH);
            if (topH > maxTop) topH = maxTop;

            _topZone.Bounds = new Rectangle(rc.X, rc.Y, rc.Width, topH);
            rc = new Rectangle(rc.X, rc.Y + topH, rc.Width, rc.Height - topH);

            int maxBot = Math.Max(0, rc.Height - MinCenterHeight);
            if (botH > maxBot) botH = maxBot;

            _bottomZone.Bounds = new Rectangle(rc.X, rc.Bottom - botH, rc.Width, botH);
            rc = new Rectangle(rc.X, rc.Y, rc.Width, rc.Height - botH);

            // Left / Right
            int leftW = _leftZone.MeasurePrimary();
            int rightW = _rightZone.MeasurePrimary();

            int maxLeft = Math.Max(0, rc.Width - MinCenterWidth - rightW);
            if (leftW > maxLeft) leftW = maxLeft;

            _leftZone.Bounds = new Rectangle(rc.X, rc.Y, leftW, rc.Height);
            rc = new Rectangle(rc.X + leftW, rc.Y, rc.Width - leftW, rc.Height);

            int maxRight = Math.Max(0, rc.Width - MinCenterWidth);
            if (rightW > maxRight) rightW = maxRight;

            _rightZone.Bounds = new Rectangle(rc.Right - rightW, rc.Y, rightW, rc.Height);
            rc = new Rectangle(rc.X, rc.Y, rc.Width - rightW, rc.Height);

            _center.Bounds = rc;

            _topZone.PerformLayout();
            _bottomZone.PerformLayout();
            _leftZone.PerformLayout();
            _rightZone.PerformLayout();
        }

        // ---------- helpers ----------
        private DockZone GetZone(DockSide side)
        {
            switch (side)
            {
                case DockSide.Left: return _leftZone;
                case DockSide.Right: return _rightZone;
                case DockSide.Top: return _topZone;
                case DockSide.Bottom: return _bottomZone;
                default: return _leftZone;
            }
        }

        private bool TryFindItem(string key, out DockZone zone, out DockItem item)
        {
            DockZone[] zones = new DockZone[] { _leftZone, _rightZone, _topZone, _bottomZone };
            for (int i = 0; i < zones.Length; i++)
            {
                DockItem it = zones[i].Find(key);
                if (it != null) { zone = zones[i]; item = it; return true; }
            }
            zone = null; item = null; return false;
        }

        internal void ApplyThemeTo(DockZone z)
        {
            z.SplitterThickness = SplitterThickness;
            z.SplitterColor = SplitterColor;
            z.SplitterHoverColor = SplitterHoverColor;
            z.HeaderHeight = HeaderHeight;
            z.HeaderBack = HeaderBack;
            z.HeaderBorder = HeaderBorder;
            z.HeaderText = HeaderText;
            z.HeaderFont = HeaderFont ?? this.Font;
        }

        static void EnableDbRecursive(Control root)
        {
            if (root == null) return;
            TrySet(root, "DoubleBuffered", true);
            TrySet(root, "ResizeRedraw", true);
            foreach (Control c in root.Controls) EnableDbRecursive(c);
        }
        static void TrySet(Control c, string name, object value)
        {
            try
            {
                PropertyInfo pi = c.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (pi != null) pi.SetValue(c, value, null);
            }
            catch { }
        }

        internal sealed class Freeze : IDisposable
        {
            const int WM_SETREDRAW = 0x000B;
            [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
            readonly Control _c; readonly bool _had;
            public Freeze(Control c) { _c = c; _had = c.IsHandleCreated; if (_had) SendMessage(_c.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero); _c.SuspendLayout(); }
            public void Dispose() { _c.ResumeLayout(true); if (_had) { SendMessage(_c.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero); _c.Invalidate(true); _c.Update(); } }
        }

        // ================== DockZone ==================
        internal class DockZone : Panel
        {
            private readonly MultiDockHost _owner;
            private readonly DockSide _side;

            private ZoneLayout _layoutMode = ZoneLayout.Stack;
            internal ZoneLayout LayoutMode
            {
                get { return _layoutMode; }
                set
                {
                    if (_layoutMode == value) return;
                    _layoutMode = value;
                    EnsureSplittersForMode();
                    RequestLayout();
                }
            }

            // Theme (được áp từ host)
            internal int SplitterThickness = 6;
            internal Color SplitterColor = Color.FromArgb(70, 0, 0, 0);
            internal Color SplitterHoverColor = Color.FromArgb(130, 0, 0, 0);
            internal int HeaderHeight = 28;
            internal Color HeaderBack = Color.FromArgb(245, 245, 245);
            internal Color HeaderBorder = Color.FromArgb(210, 210, 210);
            internal Color HeaderText = Color.Black;
            internal Font HeaderFont;

            private readonly List<DockItem> _items = new List<DockItem>();

            public DockZone(MultiDockHost owner, DockSide side)
            {
                _owner = owner; _side = side;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                         ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            }

            protected override void OnCreateControl()
            {
                base.OnCreateControl();
                _owner.ApplyThemeTo(this);
            }

            internal DockItem Find(string key)
            {
                for (int i = 0; i < _items.Count; i++)
                    if (string.Equals(_items[i].Key, key, StringComparison.OrdinalIgnoreCase))
                        return _items[i];
                return null;
            }

            internal void AddItem(DockItem item, int index)
            {
                _owner.ApplyThemeTo(this);

                item.Zone = this;
                item.Header = new HeaderBar(this, item);
                item.Content.Dock = DockStyle.Fill;
                item.Header.Height = HeaderHeight;

                if (index < 0 || index > _items.Count) index = _items.Count;
                _items.Insert(index, item);

                Controls.Add(item.Content);
                Controls.Add(item.Header);

                if (LayoutMode == ZoneLayout.Stack)
                {
                    item.Splitter = new ZoneSplitter(this, item);
                    Controls.Add(item.Splitter);
                }
                else item.Splitter = null;

                RequestLayout();
            }

            internal bool RemoveItem(string key)
            {
                DockItem it = Find(key);
                if (it == null) return false;

                Controls.Remove(it.Content);
                Controls.Remove(it.Header);
                if (it.Splitter != null) Controls.Remove(it.Splitter);

                it.Content.Dispose();
                it.Header.Dispose();
                if (it.Splitter != null) it.Splitter.Dispose();

                _items.Remove(it);
                RequestLayout();
                return true;
            }

            internal void MoveItem(DockItem item, int newIndex)
            {
                int old = _items.IndexOf(item);
                if (old < 0) return;
                if (newIndex < 0) newIndex = 0;
                if (newIndex >= _items.Count) newIndex = _items.Count - 1;
                if (old == newIndex) return;

                _items.RemoveAt(old);
                _items.Insert(newIndex, item);
                RequestLayout();
            }

            internal void RequestLayout()
            {
                PerformLayout();
                Invalidate();
                _owner.PerformLayout();
                _owner.Invalidate();
            }

            // đảm bảo splitter tồn tại/được gỡ đúng theo LayoutMode
            private void EnsureSplittersForMode()
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    DockItem it = _items[i];
                    if (_layoutMode == ZoneLayout.Stack)
                    {
                        if (it.Splitter == null)
                        {
                            it.Splitter = new ZoneSplitter(this, it);
                            Controls.Add(it.Splitter);
                        }
                    }
                    else
                    {
                        if (it.Splitter != null)
                        {
                            Controls.Remove(it.Splitter);
                            it.Splitter.Dispose();
                            it.Splitter = null;
                        }
                    }
                }
            }

            public int MeasurePrimary()
            {
                int size = 0;
                for (int i = 0; i < _items.Count; i++)
                {
                    DockItem it = _items[i];
                    if (it.Hidden) continue;

                    if (_side == DockSide.Top || _side == DockSide.Bottom)
                    {
                        int h = HeaderHeight + (it.Collapsed ? 0 : it.Size);
                        if (h > size) size = h; // cao tối đa (bố trí ngang)
                    }
                    else
                    {
                        size += HeaderHeight + (it.Collapsed ? 0 : it.Size);
                        if (LayoutMode == ZoneLayout.Stack) size += SplitterThickness;
                    }
                }
                return size;
            }

            protected override void OnLayout(LayoutEventArgs levent)
            {
                base.OnLayout(levent);
                Rectangle rc = ClientRectangle;
                if (rc.Width <= 0 || rc.Height <= 0) return;

                bool vertical = (_side == DockSide.Left || _side == DockSide.Right);

                if (vertical)
                {
                    int y = rc.Y;
                    for (int i = 0; i < _items.Count; i++)
                    {
                        DockItem it = _items[i];

                        // visibility
                        it.Header.Visible = !it.Hidden;
                        it.Content.Visible = !it.Hidden && !it.Collapsed;
                        if (it.Splitter != null) it.Splitter.Visible = !it.Hidden && (LayoutMode == ZoneLayout.Stack);

                        if (it.Hidden) continue;

                        // Header
                        it.Header.SetBounds(rc.X, y, rc.Width, HeaderHeight);
                        y += HeaderHeight;

                        int remain = RemainingHeaderHeight(it);
                        int maxH = Math.Max(0, rc.Height - (y - rc.Y) - remain);
                        int contentH = it.Collapsed ? 0 : Clamp(it.Size, it.MinSize, maxH);

                        if (!it.Collapsed)
                        {
                            it.Content.SetBounds(rc.X, y, rc.Width, contentH);
                            y += contentH;
                        }

                        if (LayoutMode == ZoneLayout.Stack)
                        {
                            if (it.Splitter == null) // phòng hờ khi đổi mode runtime
                            {
                                it.Splitter = new ZoneSplitter(this, it);
                                Controls.Add(it.Splitter);
                            }
                            it.Splitter.Visible = true;
                            it.Splitter.Bounds = new Rectangle(rc.X, y, rc.Width, _owner.SplitterThickness);
                            y += _owner.SplitterThickness;
                        }
                    }
                }
                else
                {
                    int x = rc.X;
                    int maxContentH = rc.Height - HeaderHeight; if (maxContentH < 0) maxContentH = 0;

                    for (int i = 0; i < _items.Count; i++)
                    {
                        DockItem it = _items[i];

                        it.Header.Visible = !it.Hidden;
                        it.Content.Visible = !it.Hidden && !it.Collapsed;
                        if (it.Splitter != null) it.Splitter.Visible = !it.Hidden && (LayoutMode == ZoneLayout.Stack);

                        if (it.Hidden) continue;

                        int remain = RemainingHeaderWidth(it);
                        int maxW = Math.Max(0, rc.Right - x - remain);
                        int w = Clamp(it.Size, it.MinSize, maxW);

                        it.Header.SetBounds(x, rc.Y, w, HeaderHeight);
                        if (!it.Collapsed) it.Content.SetBounds(x, rc.Y + HeaderHeight, w, maxContentH);

                        if (LayoutMode == ZoneLayout.Stack)
                        {
                            if (it.Splitter == null)
                            {
                                it.Splitter = new ZoneSplitter(this, it);
                                Controls.Add(it.Splitter);
                            }
                            it.Splitter.Visible = true;
                            it.Splitter.Bounds = new Rectangle(x + w, rc.Y, _owner.SplitterThickness, rc.Height);
                            x += w + _owner.SplitterThickness;
                        }
                        else
                        {
                            x += w;
                        }
                    }
                }
            }

            int RemainingHeaderHeight(DockItem current)
            {
                int rem = 0;
                for (int i = 0; i < _items.Count; i++)
                {
                    DockItem it = _items[i];
                    if (it == current || it.Hidden) continue;
                    rem += HeaderHeight;
                    if (LayoutMode == ZoneLayout.Stack && (_side == DockSide.Left || _side == DockSide.Right))
                        rem += SplitterThickness;
                }
                return rem;
            }

            int RemainingHeaderWidth(DockItem current)
            {
                int rem = 0;
                for (int i = 0; i < _items.Count; i++)
                {
                    DockItem it = _items[i];
                    if (it == current || it.Hidden) continue;
                    rem += it.Size;
                    if (LayoutMode == ZoneLayout.Stack && (_side == DockSide.Top || _side == DockSide.Bottom))
                        rem += SplitterThickness;
                }
                return rem;
            }

            int Clamp(int v, int min, int max)
            {
                if (max < min) max = min;
                if (v < min) return min;
                if (v > max) return max;
                return v;
            }

            // -------- Header --------
            internal sealed class HeaderBar : Control
            {
                readonly DockZone _zone; readonly DockItem _item;
                public HeaderBar(DockZone zone, DockItem item)
                {
                    _zone = zone; _item = item;
                    SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
                    Cursor = Cursors.Hand; TabStop = false;
                }
                protected override void OnPaint(PaintEventArgs e)
                {
                    Graphics g = e.Graphics;
                    Rectangle rc = ClientRectangle;

                    using (SolidBrush b = new SolidBrush(_zone.HeaderBack))
                        g.FillRectangle(b, rc);
                    using (Pen p = new Pen(_zone.HeaderBorder))
                        g.DrawRectangle(p, rc.X, rc.Y, rc.Width - 1, rc.Height - 1);

                    int sz = 8;
                    int cx = rc.X + 10; int cy = rc.Y + rc.Height / 2;
                    using (Pen p = new Pen(Color.Gray, 2))
                    {
                        if (_item.Collapsed)
                            g.DrawLines(p, new Point[] { new Point(cx, cy - sz / 2), new Point(cx + sz / 2, cy), new Point(cx, cy + sz / 2) });
                        else
                            g.DrawLines(p, new Point[] { new Point(cx - sz / 2, cy), new Point(cx, cy + sz / 2), new Point(cx + sz / 2, cy) });
                    }

                    Rectangle textRect = new Rectangle(cx + 12, rc.Y, rc.Width - (cx + 16), rc.Height);
                    TextRenderer.DrawText(g, _item.Title, _zone.HeaderFont ?? _zone._owner.Font, textRect, _zone.HeaderText,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
                }
                protected override void OnClick(EventArgs e)
                {
                    base.OnClick(e);
                    if (_zone.LayoutMode == ZoneLayout.Accordion)
                    {
                        for (int i = 0; i < _zone._items.Count; i++)
                            _zone._items[i].Collapsed = (_zone._items[i] != _item);
                    }
                    else
                    {
                        _item.Collapsed = !_item.Collapsed;
                    }
                    _zone.RequestLayout();
                }
            }

            // -------- Splitter --------
            internal sealed class ZoneSplitter : Control
            {
                readonly DockZone _zone; readonly DockItem _item;
                bool _hover, _drag; Point _start;
                public ZoneSplitter(DockZone zone, DockItem item)
                {
                    _zone = zone; _item = item;
                    SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
                    TabStop = false;
                    Cursor = (_zone._side == DockSide.Left || _zone._side == DockSide.Right) ? Cursors.HSplit : Cursors.VSplit;
                }
                protected override void OnPaint(PaintEventArgs e)
                {
                    using (SolidBrush b = new SolidBrush(_hover ? _zone.SplitterHoverColor : _zone.SplitterColor))
                        e.Graphics.FillRectangle(b, ClientRectangle);
                }
                protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; Invalidate(); }
                protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); if (!_drag) { _hover = false; Invalidate(); } }
                protected override void OnMouseDown(MouseEventArgs e)
                {
                    base.OnMouseDown(e);
                    if (e.Button != MouseButtons.Left) return;
                    _drag = true; _start = e.Location; Capture = true;
                }
                protected override void OnMouseUp(MouseEventArgs e)
                {
                    base.OnMouseUp(e);
                    _drag = false; Capture = false;
                    _hover = ClientRectangle.Contains(e.Location); Invalidate();
                }
                protected override void OnMouseMove(MouseEventArgs e)
                {
                    base.OnMouseMove(e);
                    if (!_drag) return;

                    // Với Left/Right: thay đổi CHIỀU CAO content (các pane xếp dọc)
                    // Với Top/Bottom: thay đổi CHIỀU RỘNG content (các pane xếp ngang)
                    int delta = (_zone._side == DockSide.Left || _zone._side == DockSide.Right)
                        ? (e.Y - _start.Y)
                        : (e.X - _start.X);

                    int newSize = _item.Size + delta;
                    if (newSize < _item.MinSize) newSize = _item.MinSize;
                    _item.Size = newSize;
                    _zone.RequestLayout();
                }
                protected override void SetBoundsCore(int x, int y, int w, int h, BoundsSpecified s)
                {
                    if (_zone._side == DockSide.Left || _zone._side == DockSide.Right) h = _zone.SplitterThickness;
                    else w = _zone.SplitterThickness;
                    base.SetBoundsCore(x, y, w, h, s);
                }
            }
        }

        // ================== DockItem ==================
        internal class DockItem
        {
            public string Key;
            public string Title;
            public Control Content;
            public bool Collapsed;
            public bool Hidden;
            public int Size;    // chiều cao (Left/Right) hoặc chiều rộng (Top/Bottom)
            public int MinSize;

            public MultiDockHost.DockZone Zone;
            public Control Splitter;
            public Control Header;
        }
    }
}
