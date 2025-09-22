
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    namespace BeeInterface
    {
        public class TextBoxAuto : TextBox
        {
            private bool autoFont = true;

            [Category("Behavior")]
            public bool AutoFont
            {
                get => autoFont;
                set { autoFont = value; Invalidate(); }
            }

            public TextBoxAuto()
            {
                this.AutoSize = false;

               // this.TextAlign =HorizontalAlignment.Center;
            }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            // Subscribe to parent's SizeChanged to auto-stretch height
            if (this.Parent != null)
            {
                this.Parent.SizeChanged += Parent_SizeChanged;
                // Initial sync
                this.Height = this.Parent.ClientSize.Height;
            }
            if (autoFont) AdjustFont();
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Height = this.Parent.ClientSize.Height;
                if (autoFont) AdjustFont();
            }
        }

        protected override void OnTextChanged(EventArgs e)
            {
                base.OnTextChanged(e);
                if (autoFont)
                    AdjustFont();
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);

                if (autoFont)
                    AdjustFont();

            }

            private void AdjustFont()
            {
                if (string.IsNullOrEmpty(this.Text) || this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0)
                    return;

                using (Graphics g = this.CreateGraphics())
                {
                    float minFont = 1f;
                    float maxFont = 100f;
                    float best = minFont;

                    while (maxFont - minFont > 0.5f)
                    {
                        float mid = (minFont + maxFont) / 2;
                        using (Font testFont = new Font(this.Font.FontFamily, mid, this.Font.Style))
                        {
                            Size proposedSize = this.ClientSize;
                            TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
                            Size measured = TextRenderer.MeasureText(g, this.Text, testFont, proposedSize, flags);

                            if (measured.Height <= this.ClientSize.Height && measured.Width <= this.ClientSize.Width)
                            {
                                best = mid;
                                minFont = mid;
                            }
                            else
                            {
                                maxFont = mid;
                            }
                        }
                    }

                    this.Font = new Font(this.Font.FontFamily, best, this.Font.Style);
                }
            }
        }
    }


