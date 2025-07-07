using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    [Serializable()]
    public partial class GradientTab :
              System.Windows.Forms.TabControl
    {
        // member variables
        System.Drawing.Color StartColor;
        System.Drawing.Color EndColor;

        public GradientTab()
        {
           
            RepaintControls();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            //base.OnPaint(pe);
            RepaintControls();
        }
        private void RepaintControls()
{
    foreach (TabPage ctl in this.TabPages)
    {
        System.Drawing.Drawing2D.LinearGradientBrush gradBrush;
        gradBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new
        Point(0, 0),
        new Point(ctl.Width, ctl.Height), PageStartColor, PageEndColor);

        Bitmap bmp = new Bitmap(ctl.Width, ctl.Height);

        Graphics g = Graphics.FromImage(bmp);
        g.FillRectangle(gradBrush, new Rectangle(0, 0, ctl.Width,
        ctl.Height));
        ctl.BackgroundImage = bmp;
        ctl.BackgroundImageLayout = ImageLayout.Stretch;
    }
}
        private void GradientTab_Resize(object sender, EventArgs e)
        {
            RepaintControls();
        }
        public System.Drawing.Color PageStartColor
        {
            get
            {
                return StartColor;
            }
            set
            {
                StartColor = value;
                RepaintControls();
            }
        }


        public System.Drawing.Color PageEndColor
        {
            get
            {
                return EndColor;
            }
            set
            {
                EndColor = value;
                RepaintControls();
            }
        }
    }

}
