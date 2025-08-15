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

      
        private void tmShowHis_Tick(object sender, EventArgs e)
        {
            imgHis.Invalidate();

        }

        private void imgHis_Click(object sender, EventArgs e)
        {

        }

        private void imgHis_Paint(object sender, PaintEventArgs e)
        { int y = 10;
            for (int i= Global.HistoryChecks.Count-1;i>=0;i--)
            {

                int W = Global.HistoryChecks[i].bm.Width;
                int H = Global.HistoryChecks[i].bm.Height;
                double Scale=(imgHis.Height-20)/ (H * 1.0) ;
                W = (int)(W * Scale);
                e.Graphics.DrawImage(Global.HistoryChecks[i].bm, new Rectangle(5, y, W, imgHis.Height - 20));
                e.Graphics.DrawString(Global.HistoryChecks[i].date.ToString("HH:mm:ss"), new Font("Arial",12), Brushes.Black, 5, y);
                y += H+30;
            }
            //imgHis.Size=new Size(5, y);
        }
    }
}
