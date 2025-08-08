using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BeeInterface
{
    [Designer(typeof(CustomTablePanelDesigner))]
    [ProvideProperty("Column", typeof(Control))]
    [ProvideProperty("Row", typeof(Control))]
    [DefaultProperty("ColumnCount")]
    [DefaultEvent("CellClick")]
    public class CustomTablePanel : ScrollableControl, IExtenderProvider
    {
        #region Fields
        private int _columnCount = 1;
        private int _rowCount = 1;
        private List<ColumnStyle> _columnStyles = new List<ColumnStyle> { new ColumnStyle(SizeType.Percent, 100) };
        private List<RowStyle> _rowStyles = new List<RowStyle> { new RowStyle(SizeType.Percent, 100) };
        private ContextMenuStrip _ctxMenu;
        private Rectangle[] _colSplitterBounds = Array.Empty<Rectangle>();
        private Rectangle[] _rowSplitterBounds = Array.Empty<Rectangle>();
        private int _draggingCol = -1;
        private int _draggingRow = -1;
        private bool _isResizing = false;
        private bool _showGrid = false;
        private Color _gridColor = Color.LightGray;
        #endregion

        #region Events
        public event EventHandler<CellEventArgs> CellClick;
        #endregion

        #region IExtenderProvider Implementation
        public bool CanExtend(object extendee) => extendee is Control ctrl && ctrl.Parent == this;

        public int GetColumn(Control ctrl) => (ctrl.Tag is Point pt) ? pt.X : 0;
        public void SetColumn(Control ctrl, int value)
        {
            if (!(ctrl.Tag is Point)) ctrl.Tag = new Point(0, 0);
            Point pt = (Point)ctrl.Tag;
            ctrl.Tag = new Point(value, pt.Y);
            LayoutCells();
        }

        public int GetRow(Control ctrl) => (ctrl.Tag is Point ptt) ? ptt.Y : 0;
        public void SetRow(Control ctrl, int value)
        {
            if (!(ctrl.Tag is Point)) ctrl.Tag = new Point(0, 0);
            Point pt = (Point)ctrl.Tag;
            ctrl.Tag = new Point(pt.X, value);
            LayoutCells();
        }
        #endregion

        #region Properties
        [Category("Layout"), Description("Number of columns.")]
        public int ColumnCount
        {
            get => _columnCount;
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException();
                _columnCount = value;
                Helpers.AdjustStyles(_columnStyles, _columnCount, () => new ColumnStyle(SizeType.Percent, 0));
                LayoutCells();
                Invalidate();
            }
        }

        [Category("Layout"), Description("Number of rows.")]
        public int RowCount
        {
            get => _rowCount;
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException();
                _rowCount = value;
                Helpers.AdjustStyles(_rowStyles, _rowCount, () => new RowStyle(SizeType.Percent, 0));
                LayoutCells();
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Show cell grid lines.")]
        public bool ShowGrid
        {
            get => _showGrid;
            set { _showGrid = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Grid line color.")]
        public Color GridColor
        {
            get => _gridColor;
            set { _gridColor = value; Invalidate(); }
        }

        [Category("Layout"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Editor("System.Windows.Forms.Design.ColumnStyleCollectionEditor, System.Design", typeof(UITypeEditor))]
        public ColumnStyleCollection ColumnStyles => new ColumnStyleCollection(_columnStyles);

        [Category("Layout"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Editor("System.Windows.Forms.Design.RowStyleCollectionEditor, System.Design", typeof(UITypeEditor))]
        public RowStyleCollection RowStyles => new RowStyleCollection(_rowStyles);
        #endregion

        #region Constructor & Context Menu
        public CustomTablePanel()
        {
            DoubleBuffered = true;
            SetupContextMenu();
            Resize += (s, e) => LayoutCells();
        }

        private void SetupContextMenu()
        {
            _ctxMenu = new ContextMenuStrip();
            _ctxMenu.Items.Add("Add Column", null, (s, e) => ColumnCount++);
            _ctxMenu.Items.Add("Remove Column", null, (s, e) => ColumnCount--);
            _ctxMenu.Items.Add(new ToolStripSeparator());
            _ctxMenu.Items.Add("Add Row", null, (s, e) => RowCount++);
            _ctxMenu.Items.Add("Remove Row", null, (s, e) => RowCount--);
            ContextMenuStrip = _ctxMenu;
        }
        #endregion

        #region Control Management
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            AssignCell(e.Control);
            e.Control.LocationChanged += (s, ev) => AssignCell((Control)s);
        }

        private void AssignCell(Control ctrl)
        {
            int col = DetermineIndex(ctrl.Left, ClientSize.Width, _columnStyles.Cast<Style>().ToList());
            int row = DetermineIndex(ctrl.Top, ClientSize.Height, _rowStyles.Cast<Style>().ToList());
            ctrl.Tag = new Point(col, row);
            LayoutCells();
        }

        private int DetermineIndex(int pos, int totalSize, List<Style> styles)
        {
            float[] sizes = ComputeSizes(styles, totalSize);
            int cum = 0;
            for (int i = 0; i < sizes.Length; i++)
            {
                cum += (int)sizes[i];
                if (pos < cum) return i;
            }
            return sizes.Length - 1;
        }
        #endregion

        #region Layout & Virtualization
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            LayoutCells();
        }

        private void LayoutCells()
        {
            int wTotal = ClientSize.Width, hTotal = ClientSize.Height;
            var cols = ComputeSizes(_columnStyles.Cast<Style>().ToList(), wTotal);
            var rows = ComputeSizes(_rowStyles.Cast<Style>().ToList(), hTotal);
            GenerateSplitters(cols, rows);
            SuspendLayout();
            int x = 0;
            for (int c = 0; c < _columnCount; c++)
            {
                int w = (int)cols[c], y = 0;
                for (int r = 0; r < _rowCount; r++)
                {
                    int h = (int)rows[r];
                    var rect = new Rectangle(x, y, w, h);
                    foreach (var ctrl in Controls.Cast<Control>().Where(cn => cn.Tag is Point pt && pt.X == c && pt.Y == r))
                        ctrl.Bounds = rect;
                    y += h;
                }
                x += w;
            }
            ResumeLayout();
        }

        private float[] ComputeSizes(List<Style> styles, int total)
        {
            var sizes = new float[styles.Count];
            float autoTotal = total, pctSum = 0;
            for (int i = 0; i < styles.Count; i++)
            {
                if (styles[i].SizeType == SizeType.Absolute)
                {
                    sizes[i] = styles[i].WidthOrHeight;
                    autoTotal -= sizes[i];
                }
                else if (styles[i].SizeType == SizeType.Percent)
                    pctSum += styles[i].WidthOrHeight;
            }
            for (int i = 0; i < styles.Count; i++)
            {
                if (styles[i].SizeType == SizeType.AutoSize)
                    sizes[i] = autoTotal / styles.Count;
                else if (styles[i].SizeType == SizeType.Percent)
                    sizes[i] = autoTotal * (styles[i].WidthOrHeight / pctSum);
            }
            return sizes;
        }
        #endregion

        #region Splitter Resize
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            for (int i = 0; i < _colSplitterBounds.Length; i++)
                if (_colSplitterBounds[i].Contains(e.Location))
                { _draggingCol = i; _isResizing = true; Cursor = Cursors.VSplit; return; }
            for (int i = 0; i < _rowSplitterBounds.Length; i++)
                if (_rowSplitterBounds[i].Contains(e.Location))
                { _draggingRow = i; _isResizing = true; Cursor = Cursors.HSplit; return; }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isResizing)
            {
                if (_draggingCol >= 0)
                {
                    var style = _columnStyles[_draggingCol];
                    style.SizeType = SizeType.Percent;
                    style.WidthOrHeight = (float)e.X / ClientSize.Width * 100;
                }
                else if (_draggingRow >= 0)
                {
                    var style = _rowStyles[_draggingRow];
                    style.SizeType = SizeType.Percent;
                    style.WidthOrHeight = (float)e.Y / ClientSize.Height * 100;
                }
                LayoutCells();
                Invalidate();
            }
            else
            {
                Cursor = _colSplitterBounds.Any(r => r.Contains(e.Location)) ? Cursors.VSplit :
                         _rowSplitterBounds.Any(r => r.Contains(e.Location)) ? Cursors.HSplit : Cursors.Default;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isResizing = false;
            _draggingCol = _draggingRow = -1;
            Cursor = Cursors.Default;
        }

        private void GenerateSplitters(float[] colSizes, float[] rowSizes)
        {
            _colSplitterBounds = new Rectangle[colSizes.Length - 1];
            _rowSplitterBounds = new Rectangle[rowSizes.Length - 1];
            int x = 0;
            for (int i = 0; i < colSizes.Length - 1; i++)
            {
                x += (int)colSizes[i];
                _colSplitterBounds[i] = new Rectangle(x - 2, 0, 4, ClientSize.Height);
            }
            int y = 0;
            for (int i = 0; i < rowSizes.Length - 1; i++)
            {
                y += (int)rowSizes[i];
                _rowSplitterBounds[i] = new Rectangle(0, y - 2, ClientSize.Width, 4);
            }
        }
        #endregion

        #region Painting & Grid
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_showGrid)
            {
                var g = e.Graphics;
                int wTotal = ClientSize.Width, hTotal = ClientSize.Height;
                float[] cols = ComputeSizes(_columnStyles.Cast<Style>().ToList(), wTotal);
                float[] rows = ComputeSizes(_rowStyles.Cast<Style>().ToList(), hTotal);
                int x = 0;
                using (var pen = new Pen(_gridColor))
                {
                    for (int i = 0; i <= _columnCount; i++)
                    {
                        g.DrawLine(pen, x, 0, x, hTotal);
                        if (i < _columnCount) x += (int)cols[i];
                    }
                    int y = 0;
                    for (int j = 0; j <= _rowCount; j++)
                    {
                        g.DrawLine(pen, 0, y, wTotal, y);
                        if (j < _rowCount) y += (int)rows[j];
                    }
                }
            }
        }
        #endregion

        #region Animation Helpers
        public async Task ShowColumnAsync(int index) { /* unchanged */ }
        public async Task HideColumnAsync(int index) { /* unchanged */ }
        #endregion

        #region Public API
        public void AddControl(Control ctrl, int col, int row) { ctrl.Tag = new Point(col, row); Controls.Add(ctrl); }
        public void RemoveControlAt(int col, int row) { /* unchanged */ }
        #endregion
    }

    #region Designer Class
    public class CustomTablePanelDesigner : ParentControlDesigner
    {
        private CustomTablePanel _panel;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            _panel = (CustomTablePanel)component;
            // Enable design mode for the panel itself
            EnableDesignMode(_panel, "CustomTablePanel");
            // Enable design mode for existing child controls
            foreach (Control child in _panel.Controls)
                EnableDesignMode(child, child.Name);
            // Listen for new controls added at design time
            _panel.ControlAdded += Panel_ControlAdded;
        }

        private void Panel_ControlAdded(object sender, ControlEventArgs e)
        {
            // Enable design mode for newly added controls
            EnableDesignMode(e.Control, e.Control.Name);
        }

        protected override bool GetHitTest(Point point)
        {
            return base.GetHitTest(point);
        }
    }
    #endregion

    #region Supporting Types
    public abstract class Style { public SizeType SizeType { get; set; } public float WidthOrHeight { get; set; } }
    public class ColumnStyle : Style { public ColumnStyle() : this(SizeType.AutoSize, 0) { } public ColumnStyle(SizeType sizeType) : this(sizeType, 0) { } public ColumnStyle(SizeType sizeType, float width) { SizeType = sizeType; WidthOrHeight = width; } }
    public class RowStyle : Style { public RowStyle() : this(SizeType.AutoSize, 0) { } public RowStyle(SizeType sizeType) : this(sizeType, 0) { } public RowStyle(SizeType sizeType, float height) { SizeType = sizeType; WidthOrHeight = height; } }
    public class ColumnStyleCollection : List<ColumnStyle> { public ColumnStyleCollection(IEnumerable<ColumnStyle> list) : base(list) { } }
    public class RowStyleCollection : List<RowStyle> { public RowStyleCollection(IEnumerable<RowStyle> list) : base(list) { } }
    public class CellEventArgs : EventArgs { public int Column { get; } public int Row { get; } public Control CellControl { get; } public CellEventArgs(int c, int r, Control ctrl) { Column = c; Row = r; CellControl = ctrl; } }
    #endregion

    #region Helpers
    static class Helpers
    {
        public static void AdjustStyles<T>(List<T> list, int count, Func<T> factory)
        {
            while (list.Count < count) list.Add(factory());
            while (list.Count > count) list.RemoveAt(list.Count - 1);
        }
    }
    #endregion
}