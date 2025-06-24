using BeeCore.Funtion;
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
            IOSetting IOSetting = new IOSetting();
            IOSetting.Show();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            FormReport FormReport = new FormReport();
            FormReport.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            G.account = new Account();
            G.account.cbUser.SelectedIndex = G.account.cbUser.FindStringExact(G.Config.nameUser);


            G.account.Location = new Point(G.Main.Location.X + G.Main.Width / 2 - G.account.Width / 2, G.Main.Location.Y + G.Main.Height / 2 - G.account.Height / 2);
            G.account.Show();
           btnUser.Text = G.Config.nameUser;


        }

        private void btnSettingPLC_Click(object sender, EventArgs e)
        {if(btnSettingPLC.IsCLick)
           G.EditTool.RefreshGuiEdit(Step.PLC);
        else
            G.EditTool.RefreshGuiEdit(Step.Run);
        }

        private async void btnfull_Click(object sender, EventArgs e)
        {
            btnSettingPLC.Enabled = !btnfull.IsCLick;
            G.Header.Visible =! btnfull.IsCLick;
          //  await Task.Delay(500);
          //  G.InforBar.Visible =! btnfull.IsCLick;
            await Task.Delay(500);
            //G.ResultBar.Parent.Visible = !btnfull.IsCLick;
            if (btnfull.IsCLick)
                G.EditTool.LayOutShow.ColumnStyles[1].Width = 0;
            else
                G.EditTool.LayOutShow.ColumnStyles[1].Width = 400;
            await Task.Delay(1000);
            Shows.Full(G.EditTool.View.imgView, BeeCore.Common.matRaw.Size());
        }
    }
}
