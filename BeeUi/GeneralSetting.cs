using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class GeneralSetting : Form
    {
        public GeneralSetting()
        {
            InitializeComponent();
        }

        private void cbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      //  BeeCore.Config ConfigPrev;
        private void IOSetting_Load(object sender, EventArgs e)
        {
            this.Width = Global.EditTool.BtnHeaderBar.Width+1;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - Global.EditTool.BtnHeaderBar.Width-1, Global.EditTool.pTop.Height);// Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
             btnNG.IsCLick= Global.ParaCommon.IsONNG ;
            btnOK.IsCLick =! Global.ParaCommon.IsONNG;
            btnSaveOK.IsCLick =Global.Config.IsSaveOK;
            btnSaveNG.IsCLick =Global.Config.IsSaveNG;
            btnSaveRaw.IsCLick =Global.Config.IsSaveRaw;
            btnSaveRS.IsCLick =Global.Config.IsSaveRS;
            btnTriggerMulti.IsCLick = !Global.ParaCommon.IsOnlyTrigger;
            btnTriggerOne.IsCLick=Global.ParaCommon.IsOnlyTrigger;
        
            numTrigger.Value = Global.ParaCommon.NumTrig;
            btnMulti.IsCLick = Global.ParaCommon.IsMultiCamera;
            btnSingle.IsCLick = !Global.ParaCommon.IsMultiCamera;
            switch (Global.Config.TypeSave){
                case 1:btnSmall.PerformClick(); break;
                case 2: btnNormal.PerformClick(); break;
                case 3: btnbig.PerformClick(); break;
            }
            AdjLimitDay.Value =Global.Config.LimitDateSave;
            AdjFontSize.Value = Global.Config.FontSize;
            AdjRadEdit.Value = Global.Config.RadEdit;
            btnShowBox.IsCLick = Global.Config.IsShowBox;
            btnShowDetail.IsCLick = Global.Config.IsShowDetail;
            btnShowPositon.IsCLick = Global.Config.IsShowPostion;
            lbClOK.BackColor = Global.Config.ColorOK;
            btnClOK.ForeColor = Global.Config.ColorOK;
            lbCLNG.BackColor = Global.Config.ColorNG;
            btnClNG.ForeColor = Global.Config.ColorNG;
            AdjOpacity.Value=Global.Config.Opacity;
            AdjThicknessLine.Value=Global.Config.ThicknessLine;
        }

     

        private void btnSaveOK_Click(object sender, EventArgs e)
        {
           Global.Config.IsSaveOK = btnSaveOK.IsCLick;
        }

        private void btnSaveNG_Click(object sender, EventArgs e)
        {
           Global.Config.IsSaveNG = btnSaveNG.IsCLick;
        }

       
        private void btnSmall_Click(object sender, EventArgs e)
        {
           Global.Config.TypeSave = 1;
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
           Global.Config.TypeSave = 2;
        }

        private void btnbig_Click(object sender, EventArgs e)
        {
           Global.Config.TypeSave = 3;
        }

       
        private void btnCancel_Click(object sender, EventArgs e)
        {
        
            this.Close();
        }

     

        private void btnSaveRaw_Click(object sender, EventArgs e)
        {
           Global.Config.IsSaveRaw = btnSaveRaw.IsCLick;
        }

        private void btnSaveRS_Click(object sender, EventArgs e)
        {
           Global.Config.IsSaveRS = btnSaveRS.IsCLick;
        }

       
        private void btnMulti_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsMultiCamera =btnMulti.IsCLick;
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
         Global.ParaCommon.IsMultiCamera=!   btnSingle.IsCLick;
        }



      

        private void numTrigger_ValueChanged(float obj)
        {
            Global.ParaCommon.NumTrig =(int) numTrigger.Value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData.ParaPJ(Global.Project, Global.ParaCommon);
            SaveData.Config(Global.Config);
            ShowTool.ShowAllChart(Global.ToolSettings.pAllTool);
            this.Close();
        }

     
        private void btnTriggerOne_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsOnlyTrigger = true;
        }

        private void btnTriggerMulti_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsOnlyTrigger = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdjLimitDay_ValueChanged(float obj)
        {
            Global.Config.LimitDateSave = (int)AdjLimitDay.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsONNG = !btnOK.IsCLick;
        }

        private void btnNG_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsONNG = btnNG.IsCLick;
        }

        private void AdjRadEdit_ValueChanged(float obj)
        {
            Global.Config.RadEdit = (int)AdjRadEdit.Value;
        }

        private void btnShowBox_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowBox = btnShowBox.IsCLick;
        }

        private void btnShowDetail_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowDetail = btnShowDetail.IsCLick;
        }

        private void btnShowPositon_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowPostion = btnShowPositon.IsCLick;
        }

        private void AdjFontSize_ValueChanged(float obj)
        {
            Global.Config.FontSize = (int)AdjFontSize.Value;
        }

        private void btnChooseCLOK_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.Config.ColorOK = colorChoose.Color;
                lbClOK.BackColor = Global.Config.ColorOK;
                btnClOK.ForeColor = Global.Config.ColorOK;
            }
        }

        private void btnChooseNG_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.Config.ColorNG = colorChoose.Color;
                lbCLNG.BackColor = Global.Config.ColorNG;
                btnClNG.ForeColor = Global.Config.ColorNG;
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            Global.Config.ColorOK= Color.FromArgb(0, 172, 73);
            lbClOK.BackColor = Global.Config.ColorOK;
            btnClOK.ForeColor = Global.Config.ColorOK;
        }

        private void btnDefaultNg_Click(object sender, EventArgs e)
        {
            Global.Config.ColorNG = Color.DarkRed;
            lbCLNG.BackColor = Global.Config.ColorNG;
            btnClNG.ForeColor = Global.Config.ColorNG;
        }

        private void AdjOpacity_ValueChanged(float obj)
        {
            Global.Config.Opacity = (int)AdjOpacity.Value;
        }

        private void AdjThinkness_ValueChanged(float obj)
        {
            Global.Config.ThicknessLine=(int)AdjThicknessLine.Value;
        }
    }
}
