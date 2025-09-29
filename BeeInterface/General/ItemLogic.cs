using BeeCore;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class ItemLogic : UserControl
    {
        public ItemLogic()
        {
            InitializeComponent();
        }
     
       public PropetyTool propetyTool;

        private void ck1_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[0]=ck1.Checked;
            BeeInterface.Load.ArrangeLogic();
        }

        private void ck2_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[1] = ck2.Checked; BeeInterface.Load.ArrangeLogic();
        }

        private void ck3_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[2] = ck3.Checked; BeeInterface.Load.ArrangeLogic();
        }

        private void ck4_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[3] = ck4.Checked; BeeInterface. Load.ArrangeLogic();
        }

        private void ck5_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[4] = ck5.Checked; BeeInterface. Load.ArrangeLogic();
        }

        private void ck6_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[5] = ck6.Checked; BeeInterface. Load.ArrangeLogic();
        }
    }
}
