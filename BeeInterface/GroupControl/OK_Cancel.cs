using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeGlobal;

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
            Control ctrl = this.Parent;
            ctrl.Parent.Controls.Remove(ctrl);

            Global.IndexToolSelected = -1;
            Global.EditTool. pEditTool.Controls.Clear();
            Global.ToolSettings.Dock = DockStyle.Fill;
            Global.EditTool.pEditTool.Controls.Add(Global.ToolSettings);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Control ctrl = this.Parent;
            ctrl.Parent.Controls.Remove(ctrl);

            if(Global.OldPropetyTool != null)
            {
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety = Global.OldPropetyTool.Clone();
            }
            Global.IndexToolSelected = -1;
            Global.EditTool.pEditTool.Controls.Clear();
            Global.ToolSettings.Dock = DockStyle.Fill;
            Global.EditTool.pEditTool.Controls.Add(Global.ToolSettings);
        }
    }
}
