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
using System.Xml.Linq;

namespace BeeInterface.GroupControl
{
    public partial class OK_Cancel: UserControl
    {
        public OK_Cancel()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.EditTool.pEditTool.Show("Tool");
           
           

            Global.IndexToolSelected = -1;
         
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Global.EditTool.pEditTool.Show("Tool");
           
            if(Global.OldPropetyTool != null)
            {
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety = Global.OldPropetyTool.Clone();
            }
            Global.IndexToolSelected = -1;
           
        }
    }
}
