using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
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
    public partial class BtnHeaderBar : UserControl
    {
        public BtnHeaderBar()
        {
            InitializeComponent();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {

            GeneralSetting IOSetting = new GeneralSetting();
            IOSetting.ShowDialog();
            btnSetting.IsCLick = false;
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            FormReport FormReport = new FormReport();
            FormReport.ShowDialog();
            btnUser.IsCLick = false;
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.cbUser.Text = Global.Config.Users.ToString();


            account.Location = new Point(G.Main.Location.X + G.Main.Width / 2 - G.account.Width / 2, G.Main.Location.Y + G.Main.Height / 2 - G.account.Height / 2);
            account.ShowDialog();
           btnUser.Text =Global.Config.Users.ToString();
            btnUser.IsCLick = false;

        }

     

        private async void btnfull_Click(object sender, EventArgs e)
        {
         //   btnSettingPLC.Enabled = !btnExit.IsCLick;
         ////   G.Header.Visible =! btnfull.IsCLick;
         // //  await Task.Delay(500);
         // //  G.InforBar.Visible =! btnfull.IsCLick;
         //  // await Task.Delay(500);
         //  // G.StatusDashboard.Parent.Visible = !btnfull.IsCLick;
         //   Global.EditTool.LayoutEnd.Visible=! btnExit.IsCLick;
         //   if (btnExit.IsCLick)
         //   {
         //       Global.EditTool.pHeader.Visible = false;
         //      // Global.EditTool.pEdit.Width = 200;
         //       //Global.EditTool.LayoutEnd.Visible = false;
         //       //Global.EditTool.LayOutShow.ColumnStyles[1].Width = 0;
         //       //Global.ToolSettings.Dock = DockStyle.Fill;
         //       //Global.ToolSettings.Width = 300;
         //       //Global.EditTool.View.LayOutShow.Controls.Add(Global.ToolSettings,1,0);
         //       Global.EditTool.View.pBtn.Visible = false;
         //   }    
               
         //   else
         //   {
         //       Global.EditTool.pHeader.Visible = true;
         //      // Global.EditTool.pEdit.Width = 400;
         //       //Global.EditTool.LayoutEnd.Visible = true;
         //       //Global.EditTool.LayOutShow.ColumnStyles[1].Width = 400;
         //       Global.EditTool.View.pBtn.Visible = true;
         //       //Global.EditTool.View.LayOutShow.Controls.Remove(Global.ToolSettings);
         //       //Global.EditTool.LayOutShow.Controls.Add(Global.ToolSettings, 1, 0);

         //   }    
               
         //   await Task.Delay(100);
         //   Shows.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
        }
         //if (Global.WidthOldTools == 0) Global.WidthOldTools = 400;
         //   Global.EditTool.pEdit.Width = Global.WidthOldTools;
         //   btnShuttdown.Visible = false;
            //if (Global.listParaCamera[Global.IndexChoose] == null)
            //    Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
            //BeeCore.Common.listCamera[Global.IndexCCCD] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);
            //G.ScanCCD.cbCCD.Text = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Name;

            //switch (BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera)
            //{
            //    case TypeCamera.USB:
            //        G.ScanCCD.btnUSB2_0.IsCLick = true;
            //        break;
            //    case TypeCamera.MVS:
            //        G.ScanCCD.btnGigE.IsCLick = true;
            //        break;
            //    case TypeCamera.TinyIV:
            //        G.ScanCCD.btnCameraTiny.IsCLick = true;
            //        break;
            //}
            //switch (Global.IndexChoose)
            //{
            //    case 0:
            //        G.ScanCCD.btnCamera1.IsCLick = true;
            //        break;
            //    case 1:
            //        G.ScanCCD.btnCamera2.IsCLick = true;
            //        break;
            //    case 2:
            //        G.ScanCCD.btnCamera3.IsCLick = true;
            //        break;
            //    case 3:
            //        G.ScanCCD.btnCamera4.IsCLick = true;
            //        break;
            //}
            //G.ScanCCD.ShowDialog();
        private void btnCamera_Click(object sender, EventArgs e)
        {
           
        }

        private void btncheck_Click(object sender, EventArgs e)
        {
            FormCheckUpdate formCheckUpdate = new FormCheckUpdate();
            formCheckUpdate.ShowDialog();
            btncheck.IsCLick = false;
        }

        private void btnShuttdown_Click(object sender, EventArgs e)
        {
          
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            G.Main.Close();
        }
    }
}
