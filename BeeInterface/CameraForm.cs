using BeeCore.Funtion;
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
    public partial class CameraForm : Form
    {
        public CameraForm()
        {
            InitializeComponent();
        }

        private void btnCamera1_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 0;
            Shows.ShowChart( Global.ToolSettings.pAllTool , BeeCore.Common.PropetyTools[Global.IndexChoose]);
            Global.EditTool.RefreshGuiEdit(Step.Step1);
            this.Close();
           
        }

        private void btnCamera2_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 1;
            Shows.ShowChart(Global.ToolSettings.pAllTool , BeeCore.Common.PropetyTools[Global.IndexChoose]);

            Global.EditTool.RefreshGuiEdit(Step.Step1);
            this.Close();
        }

        private void btnCamera3_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 2;
            Global.EditTool.RefreshGuiEdit(Step.Step1);
            this.Close();
        }

        private void btnCamera4_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 3;
            Global.EditTool.RefreshGuiEdit(Step.Step1);
            this.Close();
        }

        private void CameraForm_Leave(object sender, EventArgs e)
        {

        }

        private void CameraForm_Load(object sender, EventArgs e)
        {
            if (Global.listParaCamera[0] != null)
                btnCamera1.Text = Global.listParaCamera[0].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[1] != null)
                btnCamera2.Text = Global.listParaCamera[1].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[2] != null)
               btnCamera3.Text = Global.listParaCamera[2].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[3] != null)
               btnCamera4.Text = Global.listParaCamera[3].Name.Substring(0, 8) + "..";
            this.Location = new Point(Global.SizeScreen.Width / 2-this.Width/2, Global.SizeScreen.Height / 2 -this.Height/2);
        }

        private void btnDelect1_Click(object sender, EventArgs e)
        {

        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
           BeeCore.Common.listCamera[1] = null;
           BeeCore.Common.PropetyTools[1] = new List<BeeCore.PropetyTool>();
            Global.EditTool.RefreshGuiEdit(Step.Run);
            this.Close();
            
            
        }

        private void btnDelect3_Click(object sender, EventArgs e)
        {
            BeeCore.Common.listCamera[2] = null;
            BeeCore.Common.PropetyTools[2] = new List<BeeCore.PropetyTool>();
            Global.EditTool.RefreshGuiEdit(Step.Run);
            this.Close();
        }

        private void btnDelect4_Click(object sender, EventArgs e)
        {
            BeeCore.Common.listCamera[3] = null;
            BeeCore.Common.PropetyTools[3] = new List<BeeCore.PropetyTool>();
            Global.EditTool.RefreshGuiEdit(Step.Run);
            this.Close();
        }
    }
}
