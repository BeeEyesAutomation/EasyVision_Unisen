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
           // if (btnHide.IsCLick)
           // {
           //     // btnHide.Corner = Corner.Right;

            //  //   btnHide.Text = "";

            ////     Global.EditTool.LayOutShow.ColumnStyles[1].Width = 0;



            // }
            // else
            // {
            //     //btnHide.Corner = Corner.Right;
            //     Global.EditTool.pEdit.Width = Global.WidthOldTools;

            //     //   Global.EditTool.LayOutShow.ColumnStyles[1].Width = 400;


            // }
            //   G.Header.Layout.ResumeLayout(true);
            //  Global.EditTool.LayOutShow.ResumeLayout(true);
            //Global.EditTool.LayoutMain.ResumeLayout(true);

        }
        int percent = 0;
   
        private void pCamera_SizeChanged(object sender, EventArgs e)
        {
            //if (G.Header == null) return;
            //    BeeCore.CustomGui.RoundRg(pCamera,Global.Config.RoundRad,Corner.Left);
        }

        private void Cameras_Load(object sender, EventArgs e)
        {
        //   if( G.Header!=null)
        //    pCamera.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnIO_Click(object sender, EventArgs e)
        {
           
          
        }
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
