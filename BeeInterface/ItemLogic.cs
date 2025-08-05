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
        public void Arrange()
        {
            Global.ParaCommon.indexLogic1 = new List<int>();
            Global.ParaCommon.indexLogic2 = new List<int>();
            Global.ParaCommon.indexLogic3 = new List<int>();
            Global.ParaCommon.indexLogic4 = new List<int>();
            Global.ParaCommon.indexLogic5 = new List<int>();
            Global.ParaCommon.indexLogic6 = new List<int>();
            foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
            {
                if (propetyTool.IndexLogics[0] == true)
                    Global.ParaCommon.indexLogic1.Add(propetyTool.Propety.Index);
                if (propetyTool.IndexLogics[1] == true)
                    Global.ParaCommon.indexLogic2.Add(propetyTool.Propety.Index);
                if (propetyTool.IndexLogics[2] == true)
                    Global.ParaCommon.indexLogic3.Add(propetyTool.Propety.Index);
                if (propetyTool.IndexLogics[3] == true)
                    Global.ParaCommon.indexLogic4.Add(propetyTool.Propety.Index);
                if (propetyTool.IndexLogics[4] == true)
                    Global.ParaCommon.indexLogic5.Add(propetyTool.Propety.Index);
                if (propetyTool.IndexLogics[5] == true)
                    Global.ParaCommon.indexLogic6.Add(propetyTool.Propety.Index);

            }
        }
       public PropetyTool propetyTool;

        private void ck1_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[0]=ck1.Checked;
            Arrange();
        }

        private void ck2_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[1] = ck2.Checked; Arrange();
        }

        private void ck3_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[2] = ck3.Checked; Arrange();
        }

        private void ck4_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[3] = ck4.Checked; Arrange();
        }

        private void ck5_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[4] = ck5.Checked; Arrange();
        }

        private void ck6_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.IndexLogics[5] = ck6.Checked; Arrange();
        }
    }
}
