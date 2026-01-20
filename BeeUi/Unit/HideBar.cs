using BeeCore;
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

namespace BeeUi.Unit
{
    public partial class HideBar : UserControl
    {
        public HideBar()
        {
            InitializeComponent();
        }

     

        private async void btnfull_Click(object sender, EventArgs e)
        {

            Global.EditTool.pTop.Visible = !btnfull.IsCLick;
            Global.EditTool.pHeader.Visible = !btnfull.IsCLick;
            Global.EditTool.View.pBtn.Visible = !btnfull.IsCLick;
            btnShowTop.IsCLick =! Global.EditTool.pTop.Visible;
            btnShowDashBoard.IsCLick = !Global.EditTool.pInfor.Visible;
            btnMenu.IsCLick = !Global.EditTool.View.pBtn.Visible;
              await Task.Delay(100);
            ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
        }

        private void btnShowTop_Click(object sender, EventArgs e)
        {
            Global.EditTool.pTop.Visible = !btnShowTop.IsCLick;
        }

        private void btnShowDashBoard_Click(object sender, EventArgs e)
        {
            Global.EditTool.pInfor.Visible = !btnShowDashBoard.IsCLick;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Global.EditTool.View.pBtn.Visible = !btnMenu.IsCLick;
        }

        private void btnShowToolBar_Click(object sender, EventArgs e)
        {   
            if (btnShowToolBar.IsCLick)
            {
                Global.WidthOldTools = Global.EditTool.pEdit.Width;
                Global.EditTool.pEdit.Width = 0;
            //    Global.EditTool.hideBar.btnShowToolBar.IsCLick = true;
            }
            else
            {
                if (Global.WidthOldTools == 0) Global.WidthOldTools = 400;
                Global.EditTool.pEdit.Width = Global.WidthOldTools;
            }


           
           
        }

        private void btnShuttdown_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Shutdown", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                G.IsShutDown = true;
                btnExit.PerformClick();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            G.Main.Close();
        }

        private void HideBar_Load(object sender, EventArgs e)
        {
        }
    }
}
