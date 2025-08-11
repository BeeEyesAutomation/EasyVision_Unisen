using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public class DbTableLayoutPanel : TableLayoutPanel
    {
        public DbTableLayoutPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.Style |= 0x04000000;  // WS_CLIPSIBLINGS
                cp.Style |= 0x02000000;  // WS_CLIPCHILDREN
                return cp;
            }
        }
    }

}
