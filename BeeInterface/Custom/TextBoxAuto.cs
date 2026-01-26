using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BeeInterface
{
    public class TextBoxAuto : TextBox
    {
        private bool _autoFont = true;
        private bool _adjusting = false;
        private Control _oldParent = null;

        private float _minFontSize = 6f;
        private float _maxFontSize = 100f;

        [Category("Behavior")]
        public bool AutoFont
        {
            get => _autoFont;
            set { _autoFont = value; Invalidate(); if (_autoFont) AdjustFont(); }
        }

        [Category("Appearance")]
        [DefaultValue(6f)]
        public float MinFontSize
        {
            get => _minFontSize;
            set
            {
                _minFontSize = Math.Max(1f, value);
                if (_maxFontSize < _minFontSize) _maxFontSize = _minFontSize;
                if (_autoFont) AdjustFont();
            }
        }

        [Category("Appearance")]
        [DefaultValue(100f)]
        public float MaxFontSize
        {
            get => _maxFontSize;
            set
            {
                _maxFontSize = Math.Max(_minFontSize, value);
                if (_autoFont) AdjustFont();
            }
        }

        public TextBoxAuto()
        {
            AutoSize = false;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            // unsubscribe parent cũ
            if (_oldParent != null)
                _oldParent.SizeChanged -= Parent_SizeChanged;

            base.OnParentChanged(e);

            // subscribe parent mới
            if (Parent != null)
            {
                Parent.SizeChanged += Parent_SizeChanged;
                _oldParent = Parent;

                Height = Parent.ClientSize.Height;
            }

            if (_autoFont) AdjustFont();
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                Height = Parent.ClientSize.Height;
                if (_autoFont) AdjustFont();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (_autoFont) AdjustFont();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_autoFont) AdjustFont();
        }

        private void AdjustFont()
        {
            if (_adjusting) return;
            if (!_autoFont) return;
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;
            if (string.IsNullOrEmpty(Text)) return;

            _adjusting = true;
            try
            {
                using (Graphics g = CreateGraphics())
                {
                    float minF = _minFontSize;
                    float maxF = _maxFontSize;
                    float best = minF;

                    var proposed = ClientSize;
                    var flags = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;

                    while (maxF - minF > 0.5f)
                    {
                        float mid = (minF + maxF) / 2f;
                        using (Font test = new Font(Font.FontFamily, mid, Font.Style))
                        {
                            Size measured = TextRenderer.MeasureText(g, Text, test, proposed, flags);

                            if (measured.Width <= proposed.Width && measured.Height <= proposed.Height)
                            {
                                best = mid;
                                minF = mid;
                            }
                            else
                            {
                                maxF = mid;
                            }
                        }
                    }

                    // set 1 lần
                    Font = new Font(Font.FontFamily, best, Font.Style);
                }
            }
            finally
            {
                _adjusting = false;
            }
        }
    }
}
