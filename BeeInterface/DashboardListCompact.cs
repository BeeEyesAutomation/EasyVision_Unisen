using BeeCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BeeInterface
{


    public class DashboardListCompact : Control
    {
        // ===== Data =====
        private BindingList<LabelItem> _items = new BindingList<LabelItem>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingList<LabelItem> Items
        {
            get { return _items; }
            set
            {
                if (_items != null) _items.ListChanged -= Items_ListChanged;
                _items = value ?? new BindingList<LabelItem>();
                _items.ListChanged += Items_ListChanged;
                UpdateScroll();
                Invalidate();
            }
        }
        private void Items_ListChanged(object sender, ListChangedEventArgs e) { UpdateScroll(); Invalidate(); }

        // ===== Baseline (mốc 300px) =====
        private const int BASE_WIDTH = 300;
        private const float BASE_FONT_PT = 9f;
        private const int BASE_NAME_H = 22; // nút Name/Use
        private const int BASE_LINE_H = 22; // 1 dòng Min*
        private const int BASE_LINES = 3;  // số dòng Min*
        private const int BASE_ROWSPACE = 6;
        private const int BASE_PADX = 8;
        private const int BASE_PADY = 6;
        private const int BASE_CHECK = 14;
        private const int BASE_GAP = 6;  // khoảng giữa label và value
        private const int BASE_VALW_MIN = 70;
        private const int BASE_VAL_RMARG = 15;  // margin phải cố định cho ô value

        // ===== Runtime scaled metrics =====
        private float _scale = 1f;
        private Font _scaledFont;
        private Font _scaledFontBold;

        private int _padX, _padY, _check, _gap, _nameH, _lineH, _rowSpace, _valWMin, _valRightMargin;
        private int _itemHeight; // = _nameH + _padY + _lineH*3 + _rowSpace

        // ===== Tỉ lệ chia vùng label/value và width value =====
        private float _labelRatio = 0.38f;   // 58% label/checkbox – 42% value
        public float LabelColumnRatio
        {
            get { return _labelRatio; }
            set { _labelRatio = Math.Max(0.30f, Math.Min(0.80f, value)); Invalidate(); }
        }

        private float _valueWidthRatio = 1f; // ô value = 30% bề rộng dashboard (trừ margin & padding)
        public float ValueWidthRatio
        {
            get { return _valueWidthRatio; }
            set { _valueWidthRatio = Math.Max(0.15f, Math.Min(0.60f, value)); Invalidate(); }
        }

        // ===== Scroll & editor =====
        private readonly VScrollBar _vbar;
        private NumericUpDown _editor;
        private Tuple<int, Segment> _editing; // null = không edit

        // Range value
        public int ValueMin { get; set; } = 0;
        public int ValueMax { get; set; } = 100000;

        public DashboardListCompact()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            BackColor = Color.White;
            ForeColor = Color.Black;

            _vbar = new VScrollBar { Dock = DockStyle.Right, SmallChange = 1, LargeChange = 5 };
            _vbar.ValueChanged += (s, e) => { HideEditor(); Invalidate(); };
            Controls.Add(_vbar);

            _items.ListChanged += Items_ListChanged;
            Resize += (s, e) => { RecomputeScale(); UpdateScroll(); RelayoutEditor(); Invalidate(); };
            FontChanged += (s, e) => { RecomputeScale(true); Invalidate(); };
            MouseDown += OnMouseDown;
            MouseWheel += OnMouseWheel;

            RecomputeScale();
        }

        // ===== Scaling =====
        private static int S(float baseVal, float scale) { int v = (int)Math.Round(baseVal * scale); return v < 1 ? 1 : v; }
        private void RecomputeScale() { RecomputeScale(false); }
        private void RecomputeScale(bool forceFromFont)
        {
            float w = Math.Max(160, ClientSize.Width > 0 ? ClientSize.Width : BASE_WIDTH);
            float sW = w / BASE_WIDTH;
            if (sW < 0.75f) sW = 0.75f; if (sW > 1.6f) sW = 1.6f;
            _scale = sW;

            float basePt = forceFromFont ? Font.SizeInPoints : BASE_FONT_PT;
            float newPt = Math.Max(7f, Math.Min(14f, basePt * _scale));

            if (_scaledFont != null) _scaledFont.Dispose();
            if (_scaledFontBold != null) _scaledFontBold.Dispose();
            _scaledFont = new Font(Font.FontFamily, newPt, FontStyle.Regular);
            _scaledFontBold = new Font(Font.FontFamily, newPt, FontStyle.Bold);

            _padX = S(BASE_PADX, _scale);
            _padY = S(BASE_PADY, _scale);
            _check = S(BASE_CHECK, _scale);
            _gap = S(BASE_GAP, _scale);
            _nameH = S(BASE_NAME_H, _scale);
            _lineH = S(BASE_LINE_H, _scale);
            _rowSpace = S(BASE_ROWSPACE, _scale);
            _valWMin = S(BASE_VALW_MIN, _scale);
            _valRightMargin = S(BASE_VAL_RMARG, _scale);

            _itemHeight = _nameH + _padY + (_lineH * BASE_LINES) + _rowSpace;

            if (_editor != null) { _editor.Font = _scaledFont; RelayoutEditor(); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_scaledFont != null) _scaledFont.Dispose();
                if (_scaledFontBold != null) _scaledFontBold.Dispose();
            }
            base.Dispose(disposing);
        }

        // ===== Paint =====
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(BackColor);

            int rightPad = _vbar.Enabled ? _vbar.Width : 0; // chừa chỗ cho scrollbar
            int w = ClientSize.Width - rightPad;
            if (w <= 0) return;

            int start = _vbar.Value;
            int y0 = 0;

            for (int i = start, row = 0; i < _items.Count; i++, row++)
            {
                int yTop = y0 + row * _itemHeight;
                if (yTop >= ClientSize.Height) break;

                var rectItem = new Rectangle(0, yTop, w, _itemHeight);
                if (i % 2 == 1) { using (var alt = new SolidBrush(Color.FromArgb(250, 250, 250))) g.FillRectangle(alt, rectItem); }

                DrawUseButton(g, i, yTop, _items[i], w);

                int baseY = yTop + _nameH;
                DrawLine(g, i, 0, baseY, "MinArea", _items[i], _items[i].IsArea, _items[i].ValueArea, Segment.Area, Segment.ValueArea, w);
                DrawLine(g, i, 1, baseY, "MinWidth", _items[i], _items[i].IsWidth, _items[i].ValueWidth, Segment.Width, Segment.ValueWidth, w);
                DrawLine(g, i, 2, baseY, "MinHeight", _items[i], _items[i].IsHeight, _items[i].ValueHeight, Segment.Height, Segment.ValueHeight, w);

                using (var p = new Pen(Color.FromArgb(230, 230, 230))) g.DrawLine(p, rectItem.Left, rectItem.Bottom - 1, rectItem.Right, rectItem.Bottom - 1);
            }
        }

        private void DrawUseButton(Graphics g, int itemIndex, int yTop, LabelItem it, int totalWidth)
        {
            var r = new Rectangle(_padX, yTop + 2, totalWidth - _padX * 2, _nameH - 4);
            Color back = it.IsUse ? Color.FromArgb(40, 167, 69) : Color.FromArgb(190, 190, 190);
            Color border = it.IsUse ? Color.FromArgb(33, 136, 56) : Color.FromArgb(160, 160, 160);
            Color fore = it.IsUse ? Color.White : Color.Black;

            using (var b = new SolidBrush(back)) g.FillRectangle(b, r);
            using (var p = new Pen(border)) g.DrawRectangle(p, r.X, r.Y, r.Width - 1, r.Height - 1);

            TextRenderer.DrawText(g,
                string.IsNullOrEmpty(it.Name) ? ("Item " + (itemIndex + 1)) : it.Name,
                _scaledFontBold, r, fore,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void DrawLine(Graphics g, int itemIndex, int lineIndex, int baseY, string label, LabelItem it,
                              bool flag, int value, Segment flagSeg, Segment valSeg, int totalWidth)
        {
            int lineY = baseY + _padY + lineIndex * _lineH;
            var lineRect = new Rectangle(_padX, lineY, totalWidth - _padX * 2, _lineH);

            // Tách theo tỉ lệ + tính width ô value theo tỉ lệ, trừ margin phải cố định
            int splitX = lineRect.X + (int)Math.Round(lineRect.Width * _labelRatio);
            int leftW = Math.Max(1, splitX - lineRect.X - _gap);
            int valX = splitX + _gap;

            int availForValue = totalWidth - _padX * 2 - _valRightMargin; // trừ padding + margin phải
            int desiredValW = Math.Max(_valWMin, (int)(availForValue * _valueWidthRatio));

            int valRight = lineRect.Right - _valRightMargin;
            int maxValW = Math.Max(1, valRight - valX);
            int valW = Math.Min(desiredValW, maxValW);

            var leftRect = new Rectangle(lineRect.X, lineRect.Y, leftW, lineRect.Height);
            var valRect = new Rectangle(valX, lineRect.Y + 2, valW, lineRect.Height - 4);

            bool enabled = it.IsUse;

            // Checkbox căn giữa theo glyph
            Size glyph = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
            int cbX = leftRect.X;
            int cbY = leftRect.Y + (leftRect.Height - glyph.Height) / 2;
            var cbState = !enabled ? CheckBoxState.UncheckedDisabled
                                   : (flag ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
            CheckBoxRenderer.DrawCheckBox(g, new Point(cbX, cbY), cbState);

            // Label
            int textX = cbX + glyph.Width + 4;
            Color labColor = enabled ? ForeColor : Color.FromArgb(150, 150, 150);
            TextRenderer.DrawText(g, label, _scaledFontBold,
                new Rectangle(textX, leftRect.Y, leftRect.Right - textX, leftRect.Height),
                labColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            // Value box
            using (var pen = new Pen(Color.FromArgb(210, 210, 210)))
                g.DrawRectangle(pen, valRect.X, valRect.Y, valRect.Width - 1, valRect.Height - 1);

            bool isEditingThis = _editing != null &&
                                 _editing.Item1 == itemIndex &&
                                 _editing.Item2 == valSeg;

            if (enabled && flag)
            {
                if (!isEditingThis)
                {
                    var textRect = new Rectangle(valRect.X + 4, valRect.Y, valRect.Width - 8, valRect.Height);
                    TextRenderer.DrawText(g, value.ToString(), _scaledFont, textRect, ForeColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                }
            }
            else
            {
                using (var shade = new SolidBrush(Color.FromArgb(18, 0, 0, 0)))
                    g.FillRectangle(shade, valRect);
            }
        }

        // ===== Hit test =====
        private enum Segment
        {
            None,
            UseToggle,
            Area, ValueArea,
            Width, ValueWidth,
            Height, ValueHeight
        }

        private bool HitTest(Point pt, out int idx, out Segment seg, out Rectangle targetRect)
        {
            idx = -1; seg = Segment.None; targetRect = Rectangle.Empty;

            int rightPad = _vbar.Enabled ? _vbar.Width : 0;
            int w = ClientSize.Width - rightPad;
            if (w <= 0) return false;

            int item = pt.Y / _itemHeight;
            idx = _vbar.Value + item;
            if (idx < 0 || idx >= _items.Count) return false;

            int top = item * _itemHeight;

            // Nút Use (dòng đầu)
            var useRect = new Rectangle(_padX, top + 2, w - _padX * 2, _nameH - 4);
            if (useRect.Contains(pt)) { seg = Segment.UseToggle; targetRect = useRect; return true; }

            // 3 dòng Min*
            int baseY = top + _nameH;
            for (int line = 0; line < 3; line++)
            {
                int lineY = baseY + _padY + line * _lineH;
                var lineRect = new Rectangle(_padX, lineY, w - _padX * 2, _lineH);

                int splitX = lineRect.X + (int)Math.Round(lineRect.Width * _labelRatio);
                int leftW = Math.Max(1, splitX - lineRect.X - _gap);
                int valX = splitX + _gap;

                int availForValue = w - _padX * 2 - _valRightMargin;
                int desiredValW = Math.Max(_valWMin, (int)(availForValue * _valueWidthRatio));

                int valRight = lineRect.Right - _valRightMargin;
                int maxValW = Math.Max(1, valRight - valX);
                int valW = Math.Min(desiredValW, maxValW);

                var leftRect = new Rectangle(lineRect.X, lineRect.Y, leftW, lineRect.Height);
                var valRect = new Rectangle(valX, lineRect.Y + 2, valW, lineRect.Height - 4);

                if (leftRect.Contains(pt))
                {
                    seg = line == 0 ? Segment.Area : (line == 1 ? Segment.Width : Segment.Height);
                    targetRect = leftRect; return true;
                }
                if (valRect.Contains(pt))
                {
                    seg = line == 0 ? Segment.ValueArea : (line == 1 ? Segment.ValueWidth : Segment.ValueHeight);
                    targetRect = valRect; return true;
                }
            }
            return false;
        }

        // ===== Interaction =====
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            int idx; Segment seg; Rectangle rect;
            if (!HitTest(e.Location, out idx, out seg, out rect)) return;
            var it = _items[idx];

            switch (seg)
            {
                case Segment.UseToggle:
                    it.IsUse = !it.IsUse;
                    if (!it.IsUse) HideEditor();
                    Invalidate(RowRect(idx));
                    break;

                default:
                    if (!it.IsUse) return;

                    switch (seg)
                    {
                        case Segment.Area:
                            it.IsArea = !it.IsArea;
                            if (!it.IsArea && IsEditing(idx, Segment.ValueArea)) HideEditor();
                            Invalidate(RowRect(idx)); break;

                        case Segment.Width:
                            it.IsWidth = !it.IsWidth;
                            if (!it.IsWidth && IsEditing(idx, Segment.ValueWidth)) HideEditor();
                            Invalidate(RowRect(idx)); break;

                        case Segment.Height:
                            it.IsHeight = !it.IsHeight;
                            if (!it.IsHeight && IsEditing(idx, Segment.ValueHeight)) HideEditor();
                            Invalidate(RowRect(idx)); break;

                        case Segment.ValueArea:
                            if (it.IsArea) BeginEdit(idx, seg, it.ValueArea, rect); break;
                        case Segment.ValueWidth:
                            if (it.IsWidth) BeginEdit(idx, seg, it.ValueWidth, rect); break;
                        case Segment.ValueHeight:
                            if (it.IsHeight) BeginEdit(idx, seg, it.ValueHeight, rect); break;
                    }
                    break;
            }
        }

        private Rectangle RowRect(int index)
        {
            int rightPad = _vbar.Enabled ? _vbar.Width : 0;
            int w = ClientSize.Width - rightPad;
            int rel = index - _vbar.Value;
            if (rel < 0) return Rectangle.Empty;
            int y = rel * _itemHeight;
            return new Rectangle(0, y, w, _itemHeight);
        }

        private bool IsEditing(int idx, Segment seg) { return _editing != null && _editing.Item1 == idx && _editing.Item2 == seg; }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (!_vbar.Enabled) return;
            int deltaRows = Math.Sign(e.Delta);
            int newVal = Math.Max(0, Math.Min(_vbar.Maximum, _vbar.Value - deltaRows));
            if (newVal != _vbar.Value) _vbar.Value = newVal;
        }

        // ===== Editor overlay =====
        private void EnsureEditor()
        {
            if (_editor != null) return;
            _editor = new NumericUpDown
            {
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
                Minimum = ValueMin,
                Maximum = ValueMax,
                DecimalPlaces = 0,
                ThousandsSeparator = false,
                TabStop = false,
                TextAlign = HorizontalAlignment.Left
            };
            _editor.Leave += (s, e) => Commit(true);
            _editor.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { Commit(true); e.Handled = true; }
                else if (e.KeyCode == Keys.Escape) { Commit(false); e.Handled = true; }
            };
            _editor.Font = _scaledFont;
            Controls.Add(_editor);
        }

        private void BeginEdit(int idx, Segment seg, int value, Rectangle valRect)
        {
            EnsureEditor();
            _editing = new Tuple<int, Segment>(idx, seg);
            _editor.Minimum = ValueMin;
            _editor.Maximum = ValueMax;
            _editor.Value = Math.Max(ValueMin, Math.Min(ValueMax, value));
            _editor.Font = _scaledFont;
            _editor.Bounds = new Rectangle(valRect.X + 2, valRect.Y + 1, valRect.Width - 4, valRect.Height - 2);
            _editor.Visible = true;
            _editor.Focus();
            _editor.Select(0, _editor.Text.Length);
            Invalidate(RowRect(idx));
        }

        private void RelayoutEditor()
        {
            if (_editing == null || _editor == null || !_editor.Visible) return;

            int rightPad = _vbar.Enabled ? _vbar.Width : 0;
            int w = ClientSize.Width - rightPad;
            if (w <= 0) { HideEditor(); return; }

            int rel = _editing.Item1 - _vbar.Value;
            if (rel < 0) { HideEditor(); return; }

            int top = rel * _itemHeight;
            int line = 0;
            switch (_editing.Item2)
            {
                case Segment.ValueArea: line = 0; break;
                case Segment.ValueWidth: line = 1; break;
                case Segment.ValueHeight: line = 2; break;
                default: HideEditor(); return;
            }

            int baseY = top + _nameH;
            int lineY = baseY + _padY + line * _lineH;
            var lineRect = new Rectangle(_padX, lineY, w - _padX * 2, _lineH);

            int splitX = lineRect.X + (int)Math.Round(lineRect.Width * _labelRatio);
            int valX = splitX + _gap;

            int availForValue = w - _padX * 2 - _valRightMargin;
            int desiredValW = Math.Max(_valWMin, (int)(availForValue * _valueWidthRatio));

            int valRight = lineRect.Right - _valRightMargin;
            int maxValW = Math.Max(1, valRight - valX);
            int valW = Math.Min(desiredValW, maxValW);

            var valRect = new Rectangle(valX, lineRect.Y + 2, valW, lineRect.Height - 4);
            _editor.Font = _scaledFont;
            _editor.Bounds = new Rectangle(valRect.X + 2, valRect.Y + 1, valRect.Width - 4, valRect.Height - 2);
        }

        private void Commit(bool save)
        {
            if (_editing == null || _editor == null) return;

            int idx = _editing.Item1;
            Segment seg = _editing.Item2;

            if (save && idx >= 0 && idx < _items.Count)
            {
                int v = (int)_editor.Value;
                var it = _items[idx];
                switch (seg)
                {
                    case Segment.ValueArea: if (it.IsUse && it.IsArea) it.ValueArea = v; break;
                    case Segment.ValueWidth: if (it.IsUse && it.IsWidth) it.ValueWidth = v; break;
                    case Segment.ValueHeight: if (it.IsUse && it.IsHeight) it.ValueHeight = v; break;
                }
            }

            _editor.Visible = false;
            Invalidate(RowRect(idx));
            _editing = null;
        }

        private void HideEditor()
        {
            _editing = null;
            if (_editor != null) _editor.Visible = false;
        }

        // ===== Scroll =====
        private void UpdateScroll()
        {
            int vis = Math.Max(1, ClientSize.Height / _itemHeight);
            int total = _items != null ? _items.Count : 0;
            int max = Math.Max(0, total - vis);
            _vbar.Maximum = Math.Max(0, max);
            _vbar.LargeChange = Math.Max(1, vis);
            _vbar.Enabled = max > 0;
            if (_vbar.Value > max) _vbar.Value = max;
        }
    }


}