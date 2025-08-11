using System;
using System.ComponentModel; // LicenseManager
using System.Windows.Forms;

namespace BeeInterface
{
    public class TableLayoutPanel2 :  System.Windows.Forms.TableLayoutPanel
    {
        private static bool InDesigner
            => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        public TableLayoutPanel2()
        {
            // Tránh set style nặng ở ctor vì DesignMode ở đây không đáng tin với control lồng nhau
            ResizeRedraw = true; // cái này nhẹ, giữ được
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!InDesigner)
            {
                // Bật double buffer sau khi có handle
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer, true);
                DoubleBuffered = true;
                UpdateStyles();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (!InDesigner)
                {
                    cp.Style |= 0x04000000; // WS_CLIPSIBLINGS
                    cp.Style |= 0x02000000; // WS_CLIPCHILDREN
                }
                return cp;
            }
        }
    }
}
