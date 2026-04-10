using BeeCore;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BeeInterface
{
    public class DashboardListCompact : Control
    {
        private CustomNumericEx _numMinColor;
        private CustomNumericEx _numExtColor;

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
                RecomputeScale();
                UpdateScroll();
                Invalidate();
            }
        }

        private void Items_ListChanged(object sender, ListChangedEventArgs e)
        {
            RecomputeScale();
            UpdateScroll();
            Invalidate();
        }

        public event Action<int, LabelItem> ChooseColorBegin;
        public event Action<int, LabelItem> ChooseColorEnd;
        public event Action<int, LabelItem> ChooseAreaBegin;
        public event Action<int, LabelItem> ChooseAreaEnd;
   
        public event Action<int> ExternColorCharge;
        private void DrawMultiColorButton(Graphics g, Rectangle rect, LabelItem it)
        {
            using (Pen p = new Pen(Color.Gray))
                g.DrawRectangle(p, rect);

            Rectangle fillRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
            if (fillRect.Width <= 0 || fillRect.Height <= 0) return;

            if (it.ListColor != null && it.ListColor.Count > 0)
            {
                int count = it.ListColor.Count;
                float partW = (float)fillRect.Width / count;

                for (int i = 0; i < count; i++)
                {
                    int x1 = fillRect.X + (int)Math.Round(i * partW);
                    int x2 = fillRect.X + (int)Math.Round((i + 1) * partW);
                    Rectangle part = new Rectangle(x1, fillRect.Y, Math.Max(1, x2 - x1), fillRect.Height);

                    using (SolidBrush b = new SolidBrush(it.ListColor[i]))
                        g.FillRectangle(b, part);
                }
            }
            else
            {
                using (SolidBrush b = new SolidBrush(Color.LightGray))
                    g.FillRectangle(b, fillRect);

                TextRenderer.DrawText(
                    g,
                    "Color",
                    _scaledFontBold,
                    rect,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            if (it.IsChoosingColor)
            {
                using (Pen p = new Pen(Color.Blue, 2))
                    g.DrawRectangle(p, rect);
            }
        }

        // ===== Baseline (mốc 300px) =====
        private const int BASE_WIDTH = 300;
        private const float BASE_FONT_PT = 9f;
        private const int BASE_NAME_H = 22;
        private const int BASE_LINE_H = 30;
        private const int BASE_LINES = 10;
        private const int BASE_ROWSPACE = 10;
        private const int BASE_PADX = 8;
        private const int BASE_PADY = 6;
        private const int BASE_CHECK = 14;
        private const int BASE_GAP = 6;
        private const int BASE_VALW_MIN = 70;
        private const int BASE_VAL_RMARG = 1;

        // ===== Runtime scaled metrics =====
        private float _scale = 1f;
        private Font _scaledFont;
        private Font _scaledFontBold;

        private int _padX, _padY, _check, _gap, _nameH, _lineH, _rowSpace, _valWMin, _valRightMargin;
        private int _itemHeight;

        // ===== Tỉ lệ chia vùng label/value và width value =====
        private float _labelRatio = 0.25f;
        public float LabelColumnRatio
        {
            get { return _labelRatio; }
            set { _labelRatio = Math.Max(0.30f, Math.Min(0.80f, value)); Invalidate(); }
        }

        private float _valueWidthRatio = 1f;
        public float ValueWidthRatio
        {
            get { return _valueWidthRatio; }
            set { _valueWidthRatio = Math.Max(0.15f, Math.Min(0.60f, value)); Invalidate(); }
        }

        // ===== AutoDashboardHeight =====
        private bool _autoDashboardHeight = true;
        public bool AutoDashboardHeight
        {
            get { return _autoDashboardHeight; }
            set
            {
                _autoDashboardHeight = value;
                UpdateScroll();
                Invalidate();
            }
        }

        // ===== Scroll & editor =====
        private readonly VScrollBar _vbar;
        private AdjustBarEx _editorBar;
        private Tuple<int, Segment> _editing;

        public int ValueMin { get; set; } = 1;
        public int ValueMax { get; set; } = 1000;

        public DashboardListCompact()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            BackColor = Color.White;
            ForeColor = Color.Black;

            _vbar = new VScrollBar { Dock = DockStyle.Right, SmallChange = 1, LargeChange = 5 };
            _vbar.ValueChanged += (s, e) =>
            {
                HideEditor();
                HideColorEditors();
                Invalidate();
            };
            Controls.Add(_vbar);

            _items.ListChanged += Items_ListChanged;

            Resize += (s, e) =>
            {
                RecomputeScale();
                UpdateScroll();
                RelayoutEditor();
                RelayoutColorEditors();
                Invalidate();
            };

            FontChanged += (s, e) =>
            {
                RecomputeScale(true);
                RelayoutColorEditors();
                Invalidate();
            };

            MouseDown += OnMouseDown;
            MouseWheel += OnMouseWheel;

            RecomputeScale();
        }

        private static int S(float baseVal, float scale)
        {
            int v = (int)Math.Round(baseVal * scale);
            return v < 1 ? 1 : v;
        }

        private void RecomputeScale()
        {
            RecomputeScale(false);
        }

        private void RecomputeScale(bool forceFromFont)
        {
            float w = Math.Max(160, ClientSize.Width > 0 ? ClientSize.Width : BASE_WIDTH);
            float sW = w / BASE_WIDTH;
            if (sW < 0.75f) sW = 0.75f;
            if (sW > 1.6f) sW = 1.6f;
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

            if (_editorBar != null)
            {
                _editorBar.Font = _scaledFont;
                RelayoutEditor();
            }

            if (_numMinColor != null)
            {
                _numMinColor.Height = _lineH;
                _numExtColor.Height = _lineH;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_scaledFont != null) _scaledFont.Dispose();
                if (_scaledFontBold != null) _scaledFontBold.Dispose();
                if (_editorBar != null) { _editorBar.Dispose(); _editorBar = null; }
                if (_numMinColor != null) { _numMinColor.Dispose(); _numMinColor = null; }
                if (_numExtColor != null) { _numExtColor.Dispose(); _numExtColor = null; }
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            int rightPad = _vbar.Enabled ? _vbar.Width : 0;
            int w = ClientSize.Width - rightPad;
            if (w <= 0) return;

            int start = _vbar.Value;
            int y0 = 0;

            for (int i = start, row = 0; i < _items.Count; i++, row++)
            {
                int yTop = y0 + row * _itemHeight;
                if (yTop >= ClientSize.Height) break;

                Rectangle rectItem = new Rectangle(0, yTop, w, _itemHeight);

                if (i % 2 == 1)
                {
                    using (SolidBrush alt = new SolidBrush(Color.FromArgb(250, 250, 250)))
                        g.FillRectangle(alt, rectItem);
                }

                DrawUseButton(g, i, yTop, _items[i], w);

                int baseY = yTop + _nameH;
                DrawLine(g, i, 0, baseY, "MinX", _items[i], _items[i].IsX, _items[i].ValueX, Segment.X, Segment.ValueX, w);
                DrawLine(g, i, 1, baseY, "MaxX", _items[i], _items[i].IsXMax, _items[i].ValueXMax, Segment.XMax, Segment.ValueXMax, w);
                DrawLine(g, i, 2, baseY, "MinY", _items[i], _items[i].IsY, _items[i].ValueY, Segment.Y, Segment.ValueY, w);
                DrawLine(g, i, 3, baseY, "MaxY", _items[i], _items[i].IsYMax, _items[i].ValueYMax, Segment.YMax, Segment.ValueYMax, w);
                DrawLine(g, i, 4, baseY, "MinArea", _items[i], _items[i].IsArea, _items[i].ValueArea, Segment.Area, Segment.ValueArea, w);
                DrawLine(g, i, 5, baseY, "MinWidth", _items[i], _items[i].IsWidth, _items[i].ValueWidth, Segment.Width, Segment.ValueWidth, w);
                DrawLine(g, i, 6, baseY, "MinHeight", _items[i], _items[i].IsHeight, _items[i].ValueHeight, Segment.Height, Segment.ValueHeight, w);
                DrawLine(g, i, 7, baseY, "MinCount", _items[i], _items[i].IsCounter, _items[i].ValueCounter, Segment.Counter, Segment.ValueCounter, w);
                DrawLine(g, i, 8, baseY, "MinDistance", _items[i], _items[i].IsDistance, _items[i].ValueDistance, Segment.Distance, Segment.ValueDistance, w);
                DrawLine(g, i, 9, baseY, "MinColor", _items[i], _items[i].IsMinColor, _items[i].ValueMinColor, Segment.MinColor, Segment.ValueMinColor, w);

                using (Pen p = new Pen(Color.FromArgb(230, 230, 230)))
                    g.DrawLine(p,rectItem.Left, rectItem.Bottom - 1, rectItem.Right, rectItem.Bottom - 1);
            }
        }

        private void GetHeaderRects(int itemIndex, int yTop, int totalWidth,
            out Rectangle headerRect,
            out Rectangle nameRect,
            out Rectangle btnRect,
            out bool showButton)
        {
            headerRect = new Rectangle(_padX, yTop + 2, totalWidth - _padX * 2, _nameH - 4);

            int btnW = (int)(100 * _scale);
            LabelItem it = _items[itemIndex];

            showButton = it.IsUse && (_chooseActiveIndex < 0 || _chooseActiveIndex == itemIndex);

            if (showButton)
            {
                btnRect = new Rectangle(headerRect.Right - btnW - 4, headerRect.Y + 2, btnW, headerRect.Height - 4);
                nameRect = new Rectangle(headerRect.X + 6, headerRect.Y, headerRect.Width - btnW - 12, headerRect.Height);
            }
            else
            {
                btnRect = Rectangle.Empty;
                nameRect = new Rectangle(headerRect.X + 6, headerRect.Y, headerRect.Width - 12, headerRect.Height);
            }
        }

        private void DrawValueBoxWithLabel(Graphics g, Rectangle rect, string label, int value, bool enabled)
        {
            using (Pen pen = new Pen(Color.FromArgb(210, 210, 210)))
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);

            if (!enabled)
            {
                using (SolidBrush shade = new SolidBrush(Color.FromArgb(18, 0, 0, 0)))
                    g.FillRectangle(shade, rect);
                return;
            }

            int labelW = (int)(rect.Width * 0.45f);

            Rectangle rLabel = new Rectangle(rect.X + 2, rect.Y, labelW, rect.Height);
            Rectangle rValue = new Rectangle(rect.X + labelW, rect.Y, rect.Width - labelW - 2, rect.Height);

            TextRenderer.DrawText(g, label + ":", _scaledFont, rLabel, Color.Gray,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            TextRenderer.DrawText(g, value.ToString(), _scaledFont, rValue, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        private void DrawUseButton(Graphics g, int itemIndex, int yTop, LabelItem it, int totalWidth)
        {
            Rectangle headerRect, nameRect, btnRect;
            bool showButton;

            GetHeaderRects(itemIndex, yTop, totalWidth, out headerRect, out nameRect, out btnRect, out showButton);

            Color back = it.IsUse ? Color.FromArgb(246, 201, 110) : Color.FromArgb(190, 190, 190);
            Color border = it.IsUse ? Color.FromArgb(220, 178, 98) : Color.FromArgb(160, 160, 160);

            using (SolidBrush b = new SolidBrush(back))
                g.FillRectangle(b, headerRect);

            using (Pen p = new Pen(border))
                g.DrawRectangle(p, headerRect.X, headerRect.Y, headerRect.Width - 1, headerRect.Height - 1);

            TextRenderer.DrawText(
                g,
                string.IsNullOrEmpty(it.Name) ? ("Item " + (itemIndex + 1)) : it.Name,
                _scaledFontBold,
                nameRect,
                Color.Black,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            if (!showButton) return;

            bool pressed = (_chooseActiveIndex == itemIndex);
            Color btnBack = pressed ? Color.FromArgb(50, 130, 210) : Color.FromArgb(250, 230, 150);

            using (SolidBrush b = new SolidBrush(btnBack))
                g.FillRectangle(b, btnRect);

            using (Pen p = new Pen(Color.FromArgb(210, 180, 90)))
                g.DrawRectangle(p, btnRect);

            TextRenderer.DrawText(
                g,
                "Choose Area",
                _scaledFontBold,
                btnRect,
                Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawLine(Graphics g, int itemIndex, int lineIndex, int baseY, string label, LabelItem it,
            bool flag, int value, Segment flagSeg, Segment valSeg, int totalWidth)
        {
            int lineY = baseY + _padY + lineIndex * _lineH;
            Rectangle lineRect = new Rectangle(_padX, lineY, totalWidth - _padX * 2, _lineH);

            int splitX = lineRect.X + (int)Math.Round(lineRect.Width * _labelRatio);
            int leftW = Math.Max(1, splitX - lineRect.X - _gap);
            int valX = splitX + _gap;

            int availForValue = totalWidth - _padX * 2 - _valRightMargin;
            int desiredValW = Math.Max(_valWMin, (int)(availForValue * _valueWidthRatio));

            int valRight = lineRect.Right - _valRightMargin;
            int maxValW = Math.Max(1, valRight - valX);
            int valW = Math.Min(desiredValW, maxValW);

            Rectangle leftRect = new Rectangle(lineRect.X, lineRect.Y, leftW, lineRect.Height);
            Rectangle valRect = new Rectangle(valX, lineRect.Y + 2, valW, lineRect.Height - 4);

            bool enabled = it.IsUse;

            Size glyph = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
            int cbX = leftRect.X;
            int cbY = leftRect.Y + (leftRect.Height - glyph.Height) / 2;

            CheckBoxState cbState = !enabled ? CheckBoxState.UncheckedDisabled
                                             : (flag ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);

            CheckBoxRenderer.DrawCheckBox(g, new Point(cbX, cbY), cbState);

            int textX = cbX + glyph.Width + 4;
            Color labColor = enabled ? ForeColor : Color.FromArgb(150, 150, 150);

            TextRenderer.DrawText(
                g,
                label,
                _scaledFontBold,
                new Rectangle(textX, leftRect.Y, leftRect.Right - textX, leftRect.Height),
                labColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            if (lineIndex == 9)
            {
                int btnW = (int)(_lineH * 2.2f);
                int valueAreaW = valRect.Width - btnW - 6;

                Rectangle valueArea = new Rectangle(valRect.X, valRect.Y, valueAreaW, valRect.Height);
                Rectangle btnRect = new Rectangle(valueArea.Right + 6, valRect.Y, btnW, valRect.Height);

                int half = valueArea.Width / 2;

                Rectangle rMin = new Rectangle(valueArea.X, valueArea.Y, half - 2, valueArea.Height);
                Rectangle rExt = new Rectangle(valueArea.X + half + 2, valueArea.Y, half - 2, valueArea.Height);

                DrawValueBoxWithLabel(g, rMin, "Min", it.ValueMinColor, it.IsMinColor);
                DrawValueBoxWithLabel(g, rExt, "Ext", it.ValueExternColor, it.IsMinColor);
                DrawMultiColorButton(g, btnRect, it);
                //Color back = it.SampleColor == Color.Empty ? Color.LightGray : it.SampleColor;

                //using (SolidBrush b = new SolidBrush(back))
                //    g.FillRectangle(b, btnRect);

                //using (Pen p = new Pen(Color.Gray))
                //    g.DrawRectangle(p, btnRect);

                //Color txtColor = ((back.R + back.G + back.B) / 3) > 140 ? Color.Black : Color.White;
                //TextRenderer.DrawText(g, "Color", _scaledFontBold, btnRect, txtColor,
                //    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                //if (it.IsChoosingColor)
                //{
                //    using (Pen p = new Pen(Color.Blue, 2))
                //        g.DrawRectangle(p, btnRect);
                //}

                return;
            }

            using (Pen pen = new Pen(Color.FromArgb(210, 210, 210)))
                g.DrawRectangle(pen, valRect.X, valRect.Y, valRect.Width - 1, valRect.Height - 1);

            bool isEditingThis = _editing != null && _editing.Item1 == itemIndex && _editing.Item2 == valSeg;

            if (enabled && flag)
            {
                if (!isEditingThis)
                {
                    Rectangle textRect = new Rectangle(valRect.X + 4, valRect.Y, valRect.Width - 8, valRect.Height);
                    TextRenderer.DrawText(
                        g,
                        value.ToString(),
                        _scaledFont,
                        textRect,
                        ForeColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                }
            }
            else
            {
                using (SolidBrush shade = new SolidBrush(Color.FromArgb(18, 0, 0, 0)))
                    g.FillRectangle(shade, valRect);
            }
        }

        private enum Segment
        {
            None,
            UseToggle,
            ChooseAreaLimit,
            Area, ValueArea,
            Width, ValueWidth,
            Height, ValueHeight,
            X, ValueX,
            Y, ValueY,
            Counter, ValueCounter,
            XMax, ValueXMax,
            YMax, ValueYMax,
            Distance, ValueDistance,
            MinColor,
            ValueMinColor,
            ValueExternColor,
            PickColor
        }

        private bool HitTest(Point pt, out int idx, out Segment seg, out Rectangle targetRect)
        {
            idx = -1;
            seg = Segment.None;
            targetRect = Rectangle.Empty;

            int rightPad = _vbar.Enabled ? _vbar.Width : 0;
            int w = ClientSize.Width - rightPad;
            if (w <= 0) return false;

            int item = pt.Y / _itemHeight;
            idx = _vbar.Value + item;
            if (idx < 0 || idx >= _items.Count) return false;

            int top = item * _itemHeight;

            Rectangle headerRect, nameRect, btnRect;
            bool showButton;

            GetHeaderRects(idx, top, w, out headerRect, out nameRect, out btnRect, out showButton);

            if (showButton && btnRect.Contains(pt))
            {
                seg = Segment.ChooseAreaLimit;
                targetRect = btnRect;
                return true;
            }

            if (nameRect.Contains(pt))
            {
                seg = Segment.UseToggle;
                targetRect = nameRect;
                return true;
            }

            int baseY = top + _nameH;

            for (int line = 0; line < BASE_LINES; line++)
            {
                int lineY = baseY + _padY + line * _lineH;
                Rectangle lineRect = new Rectangle(_padX, lineY, w - _padX * 2, _lineH);

                int splitX = lineRect.X + (int)Math.Round(lineRect.Width * _labelRatio);
                int leftW = Math.Max(1, splitX - lineRect.X - _gap);
                int valX = splitX + _gap;

                int availForValue = w - _padX * 2 - _valRightMargin;
                int desiredValW = Math.Max(_valWMin, (int)(availForValue * _valueWidthRatio));

                int valRight = lineRect.Right - _valRightMargin;
                int maxValW = Math.Max(1, valRight - valX);
                int valW = Math.Min(desiredValW, maxValW);

                Rectangle leftRect = new Rectangle(lineRect.X, lineRect.Y, leftW, lineRect.Height);
                Rectangle valRect = new Rectangle(valX, lineRect.Y + 2, valW, lineRect.Height - 4);

                if (leftRect.Contains(pt))
                {
                    if (line == 0) seg = Segment.X;
                    else if (line == 1) seg = Segment.XMax;
                    else if (line == 2) seg = Segment.Y;
                    else if (line == 3) seg = Segment.YMax;
                    else if (line == 4) seg = Segment.Area;
                    else if (line == 5) seg = Segment.Width;
                    else if (line == 6) seg = Segment.Height;
                    else if (line == 7) seg = Segment.Counter;
                    else if (line == 8) seg = Segment.Distance;
                    else if (line == 9) seg = Segment.MinColor;

                    targetRect = leftRect;
                    return true;
                }

                if (line == 9)
                {
                    int colColorW = (int)(_lineH * 2.2f);
                    int colValueW = valRect.Width - colColorW - 6;

                    Rectangle valArea = new Rectangle(valRect.X, valRect.Y, colValueW, valRect.Height);
                    Rectangle btnRect2 = new Rectangle(valArea.Right + 6, valRect.Y, colColorW, valRect.Height);

                    int half = valArea.Width / 2;

                    Rectangle valRect1 = new Rectangle(valArea.X, valArea.Y, half - 2, valArea.Height);
                    Rectangle valRect2 = new Rectangle(valArea.X + half + 2, valArea.Y, half - 2, valArea.Height);

                    if (valRect1.Contains(pt))
                    {
                        seg = Segment.ValueMinColor;
                        targetRect = valRect1;
                        return true;
                    }

                    if (valRect2.Contains(pt))
                    {
                        seg = Segment.ValueExternColor;
                        targetRect = valRect2;
                        return true;
                    }

                    if (btnRect2.Contains(pt))
                    {
                        seg = Segment.PickColor;
                        targetRect = btnRect2;
                        return true;
                    }

                    continue;
                }

                if (valRect.Contains(pt))
                {
                    if (line == 0) seg = Segment.ValueX;
                    else if (line == 1) seg = Segment.ValueXMax;
                    else if (line == 2) seg = Segment.ValueY;
                    else if (line == 3) seg = Segment.ValueYMax;
                    else if (line == 4) seg = Segment.ValueArea;
                    else if (line == 5) seg = Segment.ValueWidth;
                    else if (line == 6) seg = Segment.ValueHeight;
                    else if (line == 7) seg = Segment.ValueCounter;
                    else if (line == 8) seg = Segment.ValueDistance;

                    targetRect = valRect;
                    return true;
                }
            }

            return false;
        }

        private int _chooseActiveIndex = -1;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            int idx;
            Segment seg;
            Rectangle rect;

            if (!HitTest(e.Location, out idx, out seg, out rect))
                return;

            if (_chooseActiveIndex >= 0 && _chooseActiveIndex != idx && seg != Segment.UseToggle)
                return;

            LabelItem it = _items[idx];

            if (seg == Segment.ChooseAreaLimit)
            {
                if (_chooseActiveIndex != idx)
                {
                    _chooseActiveIndex = idx;
                    ChooseAreaBegin?.Invoke(idx, it);
                }
                else
                {
                    _chooseActiveIndex = -1;
                    ChooseAreaEnd?.Invoke(idx, it);
                }

                Invalidate();
                return;
            }

            if (seg == Segment.PickColor)
            {
                if (!it.IsMinColor) return;

                it.IsChoosingColor = !it.IsChoosingColor;

                if (it.IsChoosingColor)
                    ChooseColorBegin?.Invoke(idx, it);
                else
                    ChooseColorEnd?.Invoke(idx, it);

                if (_numMinColor != null && _numMinColor.Visible && _editing != null && _editing.Item1 == idx)
                {
                    _numMinColor.Enabled = !it.IsChoosingColor;
                    _numExtColor.Enabled = !it.IsChoosingColor;
                }

                Invalidate(RowRect(idx));
                return;
            }

            if (seg == Segment.UseToggle)
            {
                it.IsUse = !it.IsUse;
                if (!it.IsUse)
                {
                    HideEditor();
                    HideColorEditors();
                }
                UpdateScroll();
                Invalidate();
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            HideColorEditors();

            int idx = 0;
            Segment seg;
            Rectangle rect;

            if (!HitTest(e.Location, out idx, out seg, out rect))
            {
                HideEditor();
                return;
            }

            LabelItem it = _items[idx];

            if (!it.IsUse) return;

            switch (seg)
            {
                case Segment.Area:
                    it.IsArea = !it.IsArea;
                    if (!it.IsArea && IsEditing(idx, Segment.ValueArea)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.Width:
                    it.IsWidth = !it.IsWidth;
                    if (!it.IsWidth && IsEditing(idx, Segment.ValueWidth)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.Height:
                    it.IsHeight = !it.IsHeight;
                    if (!it.IsHeight && IsEditing(idx, Segment.ValueHeight)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.X:
                    it.IsX = !it.IsX;
                    if (!it.IsX && IsEditing(idx, Segment.ValueX)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.XMax:
                    it.IsXMax = !it.IsXMax;
                    if (!it.IsXMax && IsEditing(idx, Segment.ValueXMax)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.Y:
                    it.IsY = !it.IsY;
                    if (!it.IsY && IsEditing(idx, Segment.ValueY)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.YMax:
                    it.IsYMax = !it.IsYMax;
                    if (!it.IsYMax && IsEditing(idx, Segment.ValueYMax)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.Counter:
                    it.IsCounter = !it.IsCounter;
                    if (!it.IsCounter && IsEditing(idx, Segment.ValueCounter)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.Distance:
                    it.IsDistance = !it.IsDistance;
                    if (!it.IsDistance && IsEditing(idx, Segment.ValueDistance)) HideEditor();
                    UpdateScroll();
                    Invalidate(RowRect(idx));
                    break;

                case Segment.ValueArea:
                    if (it.IsArea) BeginEdit(idx, seg, it.ValueArea, rect);
                    break;

                case Segment.ValueWidth:
                    if (it.IsWidth) BeginEdit(idx, seg, it.ValueWidth, rect);
                    break;

                case Segment.ValueHeight:
                    if (it.IsHeight) BeginEdit(idx, seg, it.ValueHeight, rect);
                    break;

                case Segment.ValueX:
                    if (it.IsX) BeginEdit(idx, seg, it.ValueX, rect);
                    break;

                case Segment.ValueY:
                    if (it.IsY) BeginEdit(idx, seg, it.ValueY, rect);
                    break;

                case Segment.ValueXMax:
                    if (it.IsXMax) BeginEdit(idx, seg, it.ValueXMax, rect);
                    break;

                case Segment.ValueYMax:
                    if (it.IsYMax) BeginEdit(idx, seg, it.ValueYMax, rect);
                    break;

                case Segment.ValueCounter:
                    if (it.IsCounter) BeginEdit(idx, seg, it.ValueCounter, rect);
                    break;

                case Segment.ValueDistance:
                    if (it.IsDistance) BeginEdit(idx, seg, it.ValueDistance, rect);
                    break;

                case Segment.MinColor:
                    it.IsMinColor = !it.IsMinColor;
                    if (!it.IsMinColor)
                    {
                        it.IsChoosingColor = false;

                     
                        //  it.SampleColor = Color.Empty; // nếu còn dùng ở nơi khác
                        HideColorEditors();
                    }
                    else
                    {
                        BeginEditColor(idx);
                    }
                    Invalidate(RowRect(idx));
                    break;

                case Segment.ValueMinColor:
                case Segment.ValueExternColor:
                    if (it.IsMinColor)
                        BeginEditColor(idx);
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

        private bool IsEditing(int idx, Segment seg)
        {
            return _editing != null && _editing.Item1 == idx && _editing.Item2 == seg;
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (!_vbar.Enabled) return;
            int deltaRows = Math.Sign(e.Delta);
            int newVal = Math.Max(0, Math.Min(_vbar.Maximum, _vbar.Value - deltaRows));
            if (newVal != _vbar.Value) _vbar.Value = newVal;
        }

        private void EnsureEditor()
        {
            if (_editorBar != null) return;

            _editorBar = new AdjustBarEx
            {
                Visible = false,
                Decimals = 0,
                Min = 1,
                Max = 1000,
                AutoShowTextbox = true,
                AutoSizeTextbox = true,
                BackColor = SystemColors.Control,
                BarLeftGap = 14,
                BarRightGap = 6,
                ChromeGap = 4,
                ChromeWidthRatio = 0.14F,
                ColorBorder = Color.DarkGray,
                ColorFill = Color.FromArgb(246, 213, 143),
                ColorScale = Color.DarkGray,
                ColorThumb = Color.FromArgb(246, 201, 110),
                ColorThumbBorder = Color.FromArgb(246, 201, 110),
                ColorTrack = Color.DarkGray,
                EdgePadding = 1,
                Font = new Font("Segoe UI", 10F),
                InnerPadding = new Padding(10, 6, 10, 6),
                KeyboardStep = 1F,
                MatchTextboxFontToThumb = true,
                MaxTextboxWidth = 0,
                MaxThumb = 1000,
                MaxTrackHeight = 1000,
                MinChromeWidth = 64,
                MinThumb = 30,
                MinTrackHeight = 6,
                Name = "edit",
                Radius = 6,
                ShowValueOnThumb = true,
                SnapToStep = true,
                StartWithTextboxHidden = true,
                Step = 1F,
                TabIndex = 68,
                TextboxFontSize = 16F,
                TextboxSidePadding = 2,
                ThumbDiameterRatio = 1.5F,
                ThumbValueBold = true,
                ThumbValueFontScale = 1.2F,
                ThumbValuePadding = 0,
                TightEdges = true,
                TrackHeightRatio = 0.7F,
                TrackWidthRatio = 1F,
                UnitText = "",
                Value = 0F,
                WheelStep = 1F
            };

            _editorBar.Font = _scaledFont;

            _editorBar.ValueChanged += delegate (float v)
            {
                if (_editing == null) return;
                int idx = _editing.Item1;
                Segment seg = _editing.Item2;
                if (idx < 0 || idx >= _items.Count) return;
                LabelItem it = _items[idx];

                int iv = (int)Math.Round(v);
                if (seg == Segment.ValueArea && it.IsUse && it.IsArea) it.ValueArea = iv;
                if (seg == Segment.ValueWidth && it.IsUse && it.IsWidth) it.ValueWidth = iv;
                if (seg == Segment.ValueHeight && it.IsUse && it.IsHeight) it.ValueHeight = iv;
                if (seg == Segment.ValueX && it.IsUse && it.IsX) it.ValueX = iv;
                if (seg == Segment.ValueY && it.IsUse && it.IsY) it.ValueY = iv;
                if (seg == Segment.ValueXMax && it.IsUse && it.IsXMax) it.ValueXMax = iv;
                if (seg == Segment.ValueYMax && it.IsUse && it.IsYMax) it.ValueYMax = iv;
                if (seg == Segment.ValueCounter && it.IsUse && it.IsCounter) it.ValueCounter = iv;
                if (seg == Segment.ValueDistance && it.IsUse && it.IsDistance) it.ValueDistance = iv;

                Invalidate(RowRect(idx));
            };

            _editorBar.KeyDown += delegate (object s, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape) HideEditor();
            };

            Controls.Add(_editorBar);
            _editorBar.BringToFront();
        }

        private void BeginEdit(int idx, Segment seg, int value, Rectangle valRect)
        {
            EnsureEditor();
            _editing = new Tuple<int, Segment>(idx, seg);

            _editorBar.Min = ValueMin;
            _editorBar.Max = Math.Max(ValueMin + 1, ValueMax);
            _editorBar.Step = 1f;
            _editorBar.Decimals = 0;
            _editorBar.UnitText = "";
            _editorBar.Value = Math.Max(ValueMin, Math.Min(ValueMax, value));

            Rectangle inner = new Rectangle(
                valRect.X + 2,
                valRect.Y + 1,
                Math.Max(1, valRect.Width - 4),
                Math.Max(1, valRect.Height - 2));

            _editorBar.Bounds = inner;

            _editorBar.Visible = true;
            _editorBar.BringToFront();
            _editorBar.Focus();

            Invalidate(RowRect(idx));
        }

        private void EnsureColorEditors()
        {
            if (_numMinColor != null) return;

            _numMinColor = CreateNumeric();
            _numExtColor = CreateNumeric();

            _numMinColor.Max = 2000;
            _numExtColor.Max = 255;

            _numMinColor.ValueChanged += (v) =>
            {
                if (_editing == null) return;
                int idx = _editing.Item1;
                if (idx < 0 || idx >= _items.Count) return;

                _items[idx].ValueMinColor = (int)v;
                Invalidate(RowRect(idx));
            };

            _numExtColor.ValueChanged += (v) =>
            {
                if (_editing == null) return;
                int idx = _editing.Item1;
                if (idx < 0 || idx >= _items.Count) return;

                _items[idx].ValueExternColor = (int)v;
                ExternColorCharge?.Invoke(_items[idx].ValueExternColor);
         
                Invalidate(RowRect(idx));
            };

            Controls.Add(_numMinColor);
            Controls.Add(_numExtColor);
            _numMinColor.BringToFront();
            _numExtColor.BringToFront();
        }

        private CustomNumericEx CreateNumeric()
        {
            return new CustomNumericEx()
            {
                Min = 0,
                Max = 255,
                Step = 1,
                Width = 80,
                Height = _lineH,
                Visible = false,
                AutoShowTextbox = false,
                
                ButtonMaxSize=22,
                TextboxFontSize=14,
                
                BackColor = Color.White
            };
        }

        private bool IsEditingColor()
        {
            if (_editing == null) return false;
            return _editing.Item2 == Segment.ValueMinColor || _editing.Item2 == Segment.ValueExternColor || _editing.Item2 == Segment.MinColor;
        }

        private bool TryGetColorRects(int idx, out Rectangle rMin, out Rectangle rExt, out Rectangle btnRect)
        {
            rMin = Rectangle.Empty;
            rExt = Rectangle.Empty;
            btnRect = Rectangle.Empty;

            if (idx < 0 || idx >= _items.Count) return false;

            int rightPad = _vbar.Enabled ? _vbar.Width : 0;
            int w = ClientSize.Width - rightPad;
            if (w <= 0) return false;

            int rel = idx - _vbar.Value;
            if (rel < 0) return false;

            int top = rel * _itemHeight;
            int baseY = top + _nameH;

            int lineIndex = 9;
            int lineY = baseY + _padY + lineIndex * _lineH;
            Rectangle lineRect = new Rectangle(_padX, lineY, w - _padX * 2, _lineH);

            int splitX = lineRect.X + (int)Math.Round(lineRect.Width * _labelRatio);
            int valX = splitX + _gap;

            int availForValue = w - _padX * 2 - _valRightMargin;
            int desiredValW = Math.Max(_valWMin, (int)(availForValue * _valueWidthRatio));

            int valRight = lineRect.Right - _valRightMargin;
            int maxValW = Math.Max(1, valRight - valX);
            int valW = Math.Min(desiredValW, maxValW);

            Rectangle valRect = new Rectangle(valX, lineRect.Y + 2, valW, lineRect.Height - 4);

            int btnW = (int)(_lineH * 2.2f);
            int valueAreaW = valRect.Width - btnW - 6;
            if (valueAreaW <= 4) return false;

            Rectangle valueArea = new Rectangle(valRect.X, valRect.Y, valueAreaW, valRect.Height);
            btnRect = new Rectangle(valueArea.Right + 6, valRect.Y, btnW, valRect.Height);

            int half = valueArea.Width / 2;
            rMin = new Rectangle(valueArea.X, valueArea.Y, half - 2, valueArea.Height);
            rExt = new Rectangle(valueArea.X + half + 2, valueArea.Y, half - 2, valueArea.Height);

            return true;
        }

        private void BeginEditColor(int idx)
        {
            EnsureColorEditors();
            HideEditor();

            Rectangle rMin, rExt, btnRect;
            if (!TryGetColorRects(idx, out rMin, out rExt, out btnRect))
            {
                HideColorEditors();
                return;
            }

            _editing = new Tuple<int, Segment>(idx, Segment.ValueMinColor);

            var it = _items[idx];

            _numMinColor.Bounds = rMin;
            _numExtColor.Bounds = rExt;

            _numMinColor.Value = it.ValueMinColor;
            _numExtColor.Value = it.ValueExternColor;

            _numMinColor.Enabled = !it.IsChoosingColor;
            _numExtColor.Enabled = !it.IsChoosingColor;

            _numMinColor.Visible = true;
            _numExtColor.Visible = true;
            _numMinColor.BringToFront();
            _numExtColor.BringToFront();
        }

        private void RelayoutColorEditors()
        {
            if (_numMinColor == null || !_numMinColor.Visible || _editing == null)
                return;

            if (!IsEditingColor()) return;

            Rectangle rMin, rExt, btnRect;
            if (!TryGetColorRects(_editing.Item1, out rMin, out rExt, out btnRect))
            {
                HideColorEditors();
                return;
            }

            _numMinColor.Bounds = rMin;
            _numExtColor.Bounds = rExt;
        }

        private void HideColorEditors()
        {
            if (_numMinColor != null) _numMinColor.Visible = false;
            if (_numExtColor != null) _numExtColor.Visible = false;

            if (_editing != null && IsEditingColor())
                _editing = null;
        }

        private void RelayoutEditor()
        {
            if (_editing == null || _editorBar == null || !_editorBar.Visible) return;
            if (IsEditingColor()) return;

            int rightPad = _vbar.Enabled ? _vbar.Width : 0;
            int w = ClientSize.Width - rightPad;
            if (w <= 0)
            {
                HideEditor();
                return;
            }

            int rel = _editing.Item1 - _vbar.Value;
            if (rel < 0)
            {
                HideEditor();
                return;
            }

            int line = -1;
            switch (_editing.Item2)
            {
                case Segment.ValueX: line = 0; break;
                case Segment.ValueXMax: line = 1; break;
                case Segment.ValueY: line = 2; break;
                case Segment.ValueYMax: line = 3; break;
                case Segment.ValueArea: line = 4; break;
                case Segment.ValueWidth: line = 5; break;
                case Segment.ValueHeight: line = 6; break;
                case Segment.ValueCounter: line = 7; break;
                case Segment.ValueDistance: line = 8; break;
            }

            if (line < 0)
            {
                HideEditor();
                return;
            }

            int top = rel * _itemHeight;
            int baseY = top + _nameH;
            int lineY = baseY + _padY + line * _lineH;

            Rectangle lineRect = new Rectangle(_padX, lineY, w - _padX * 2, _lineH);

            int splitX = lineRect.X + (int)Math.Round(lineRect.Width * _labelRatio);
            int valX = splitX + _gap;

            int availForValue = w - _padX * 2 - _valRightMargin;
            int desiredValW = Math.Max(_valWMin, (int)(availForValue * _valueWidthRatio));

            int valRight = lineRect.Right - _valRightMargin;
            int maxValW = Math.Max(1, valRight - valX);
            int valW = Math.Min(desiredValW, maxValW);

            Rectangle valRect = new Rectangle(valX, lineRect.Y + 2, valW, lineRect.Height - 4);

            Rectangle inner = new Rectangle(
                valRect.X + 2,
                valRect.Y + 1,
                Math.Max(1, valRect.Width - 4),
                Math.Max(1, valRect.Height - 2));

            _editorBar.Bounds = inner;
            _editorBar.Font = _scaledFont;
        }

        private void HideEditor()
        {
            if (!IsEditingColor())
                _editing = null;

            if (_editorBar != null) _editorBar.Visible = false;
        }

        private void UpdateScroll()
        {
            int total = (_items != null) ? _items.Count : 0;

            if (_autoDashboardHeight)
            {
                int newH = _itemHeight * Math.Max(1, total);
                this.Height = newH;
                _vbar.Enabled = false;
                _vbar.Visible = false;
                return;
            }

            int vis = Math.Max(1, ClientSize.Height / _itemHeight);
            int max = Math.Max(0, total - vis);

            _vbar.Maximum = Math.Max(0, max);
            _vbar.LargeChange = Math.Max(1, vis);

            _vbar.Enabled = max > 0;
            _vbar.Visible = max > 0;

            if (_vbar.Value > max) _vbar.Value = max;
        }
    }
}