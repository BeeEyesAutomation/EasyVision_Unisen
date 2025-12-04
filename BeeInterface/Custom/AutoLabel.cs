using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BeeInterface
{
    public class AutoFontLabel : Label
    {
        private bool autoFont = true;
        private bool _adjusting = false;      // Chặn vòng lặp
        private Control _oldParent = null;    // Để unsubscribe event

        [Category("Behavior")]
        public bool AutoFont
        {
            get => autoFont;
            set { autoFont = value; Invalidate(); }
        }

        public AutoFontLabel()
        {
            this.AutoSize = false;
            //this.TextAlign = ContentAlignment.MiddleCenter;
        }

        //===============================
        // XỬ LÝ PARENT
        //===============================
        protected override void OnParentChanged(EventArgs e)
        {
            // Gỡ bỏ event cũ
            if (_oldParent != null)
                _oldParent.SizeChanged -= Parent_SizeChanged;

            base.OnParentChanged(e);

            // Gắn event mới
            if (this.Parent != null)
            {
                this.Parent.SizeChanged += Parent_SizeChanged;
                _oldParent = this.Parent;

                this.Height = this.Parent.ClientSize.Height;
            }

            if (autoFont) AdjustFont();
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Height = this.Parent.ClientSize.Height;

                if (autoFont)
                    AdjustFont();
            }
        }

        //===============================
        // TEXT / SIZE CHANGE
        //===============================
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (autoFont)
                AdjustFont();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (!_adjusting && autoFont)
                AdjustFont();
        }

        //===============================
        // TÍNH TOÁN FONT
        //===============================
        private void AdjustFont()
        {
            if (_adjusting) return;  // CHẶN VÒNG LẶP
            if (!autoFont) return;
            if (string.IsNullOrEmpty(Text)) return;
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

            _adjusting = true;

            try
            {
                using (Graphics g = this.CreateGraphics())
                {
                    float minF = 1f;
                    float maxF = 300f;
                    float best = minF;

                    var flags = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
                    var box = this.ClientSize;

                    while (maxF - minF > 0.5f)
                    {
                        float mid = (minF + maxF) / 2;
                        using (Font test = new Font(this.Font.FontFamily, mid, this.Font.Style))
                        {
                            Size measured = TextRenderer.MeasureText(g, this.Text, test, box, flags);

                            if (measured.Width <= box.Width &&
                                measured.Height <= box.Height)
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

                    // Set font 1 lần (KHÔNG gây lặp vì đã khóa _adjusting)
                    this.Font = new Font(this.Font.FontFamily, best, this.Font.Style);
                }
            }
            finally
            {
                _adjusting = false;
            }
        }
    }
}
