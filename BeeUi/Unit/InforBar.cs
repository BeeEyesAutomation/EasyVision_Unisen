using BeeGlobal;
using BeeUi.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    public partial class InforBar : UserControl
    {
        public InforBar()
        {
            InitializeComponent();
            G.InforBar = this;
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            if (btnHide.IsCLick)
            {

                //  Global.EditTool.LayOutShow.ColumnStyles[0].Width = 100;
                Global.EditTool.LayOutShow.ColumnStyles[1].Width = 255;
                Global.EditTool.LayOutShow.ResumeLayout(true);
            }
            else
            {
                //  Global.EditTool.LayOutShow.ColumnStyles[0].Width = 70;
                Global.EditTool.LayOutShow.ColumnStyles[1].Width = 45;
                Global.EditTool.LayOutShow.ResumeLayout(true);
            }

            //if (btnHide.IsCLick)
            //    this.Height = 250;
            //else
            //    this.Height = 40;
        }

        private void tmShowHis_Tick(object sender, EventArgs e)
        {
            imgHis.Invalidate();

        }

        private void imgHis_Click(object sender, EventArgs e)
        {

        }

        private void imgHis_Paint(object sender, PaintEventArgs e)
        { int x = 10;
            for (int i= G.listHis.Count-1;i>=0;i--)
            {

                int W = G.listHis[i].bm.Width;
                int H = G.listHis[i].bm.Height;
                double Scale=(imgHis.Height-20)/ (H * 1.0) ;
                W = (int)(W * Scale);
                e.Graphics.DrawImage(G.listHis[i].bm, new Rectangle(x, 5, W, imgHis.Height - 20));
                e.Graphics.DrawString(G.listHis[i].date.ToString("HH:mm:ss"), new Font("Arial",12), Brushes.Black, x, imgHis.Height - 25);
                x += W+20;
            }
            imgHis.Size=new Size(x, imgHis.Height);
        }
    }
}
