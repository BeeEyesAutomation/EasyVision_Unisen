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

namespace BeeUi.Unit
{
    public partial class Cameras : UserControl
    {
        public Cameras()
        {
            InitializeComponent();
        }
      
        private  void btnHide_Click(object sender, EventArgs e)
        {
            Global.WidthOldTools = Global.EditTool.pEdit.Width;
            Global.EditTool.pEdit.Width = 0;
            Global.EditTool.hideBar.btnShowToolBar.IsCLick = true;
        
        }
        int percent = 0;
   
  

 
        ScanCCD scanCCD = new ScanCCD();
        private void btnCamera1_Click(object sender, EventArgs e)
        {
            
            Global.IndexChoose = 0;
            if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
            {
                btnCamera1.IsCLick = false;
                return;
            }    
              
            Global.ToolSettings.pAllTool.Controls.Clear();
            G.Header.stepShow = 0;
            G.Header.indexToolShow = 0;
            G.Header.  tmShow.Enabled = true;
           
        }

        private void btnCamera2_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 1;
            if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
            {
                btnCamera2.IsCLick = false;
                return;
            }
            Global.ToolSettings.pAllTool.Controls.Clear();
            G.Header.stepShow = 0;
            G.Header.indexToolShow = 0;
            G.Header.tmShow.Enabled = true;
        }

        private void btnCamera3_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 2;
            if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
            {
                btnCamera2.IsCLick = false;
                return;
            }
            Global.ToolSettings.pAllTool.Controls.Clear();
            G.Header.stepShow = 0;
            G.Header.indexToolShow = 0;
            G.Header.tmShow.Enabled = true;
        }

        private void btnCamera4_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 3;
            if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
            {
                btnCamera4.IsCLick = false;
                return;
            }
            Global.ToolSettings.pAllTool.Controls.Clear();
            G.Header.stepShow = 0;
            G.Header.indexToolShow = 0;
            G.Header.tmShow.Enabled = true;
        }
    }
}
