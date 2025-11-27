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
                case 1:btnSmall.IsCLick=true; break;
                case 2: btnNormal.IsCLick = true; break;
                case 3: btnbig.IsCLick = true; break;
            }
            AdjLimitDay.Value =Global.Config.LimitDateSave;
            AdjFontSize.Value = Global.Config.FontSize;
            AdjRadEdit.Value = Global.Config.RadEdit;
            btnShowBox.IsCLick = Global.Config.IsShowBox;
            btnShowDetail.IsCLick = Global.Config.IsShowDetail;
            btnShowPositon.IsCLick = Global.Config.IsShowPostion;
            btnShowScore.IsCLick = Global.Config.IsShowScore;
            btnShowLabel.IsCLick= Global.Config.IsShowLabel;

            lbClOK.BackColor = Global.Config.ColorOK;
            btnClOK.ForeColor = Global.Config.ColorOK;
            lbCLNG.BackColor = Global.Config.ColorNG;
            btnClNG.ForeColor = Global.Config.ColorNG;

            lbCLInfor.BackColor = Global.Config.ColorInfor;
            btnCLInfor.ForeColor = Global.Config.ColorInfor;
            lbCLNone.BackColor = Global.Config.ColorNone;
            btnCLNone.ForeColor = Global.Config.ColorNone;
            lbCLText.BackColor = Global.Config.TextColor;
            btnCLText.ForeColor = Global.Config.TextColor;
            lbClChoose.BackColor = Global.Config.ColorChoose;
            btnClChoose.ForeColor = Global.Config.ColorChoose;
            AdjOpacity.Value=Global.Config.Opacity;
            AdjThicknessLine.Value=Global.Config.ThicknessLine;
            btnByPassResult.IsCLick=Global.ParaCommon.IsForceByPassRS;
           
            if (Global.ParaCommon.IsForceByPassRS)
                btnByPassResult.Text = "ON";
            else
                btnByPassResult.Text = "OFF";
            if (Global.ParaCommon.IsForceByPassRS)
            {
                Global.EditTool.lbBypass.Visible = true;
              
            }
            else
            {
                Global.EditTool.lbBypass.Visible = false;
               
            }
            btnResetReady.IsCLick = Global.ParaCommon.IsResetReady;
            numRetryCam.Value = Global.ParaCommon.NumRetryCamera;
            numRetryPLC.Value = Global.ParaCommon.NumRetryPLC;
        
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

        private void btnChooseCLInfor_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.Config.ColorInfor = colorChoose.Color;
                lbCLInfor.BackColor = Global.Config.ColorInfor;
                btnCLInfor.ForeColor = Global.Config.ColorInfor;
            }
        }

        private void btnDefInfor_Click(object sender, EventArgs e)
        {
            Global.Config.ColorInfor = Color.Blue;
            lbCLInfor.BackColor = Global.Config.ColorInfor;
            btnCLInfor.ForeColor = Global.Config.ColorInfor;
        }

        private void btnChooseNone_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.Config.ColorNone = colorChoose.Color;
                lbCLNone.BackColor = Global.Config.ColorNone;
                btnCLNone. ForeColor = Global.Config.ColorNone;
            }
        }

        private void btnDefNone_Click(object sender, EventArgs e)
        {
            Global.Config.ColorNone = Color.LightGray;
            lbCLNone.BackColor = Global.Config.ColorNone;
            btnCLNone.ForeColor = Global.Config.ColorNone;
        }

        private void btnChooseCLChoose_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.Config.ColorChoose = colorChoose.Color;
                lbClChoose.BackColor = Global.Config.ColorChoose;
                btnClChoose.ForeColor = Global.Config.ColorChoose;
            }
        }

        private void btnDefaultChoose_Click(object sender, EventArgs e)
        {
            Global.Config.ColorChoose = Color.FromArgb(246, 204, 120);
            lbClChoose.BackColor = Global.Config.ColorChoose;
            btnClChoose.ForeColor = Global.Config.ColorChoose;
        }

        private void btnChooseCLText_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.Config.TextColor = colorChoose.Color;
                lbCLText.BackColor = Global.Config.TextColor;
                btnCLText.ForeColor = Global.Config.TextColor;
            }
        }

        private void btnDefaultCLText_Click(object sender, EventArgs e)
        {
            Global.Config.TextColor = Color.White;
            lbCLText.BackColor = Global.Config.TextColor;
            btnCLText.ForeColor = Global.Config.TextColor;
        }

        private void btnByPassResult_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsForceByPassRS = btnByPassResult.IsCLick;
            if (Global.ParaCommon.IsForceByPassRS)
                btnByPassResult.Text = "ON";
            else
                btnByPassResult.Text = "OFF";
            if ( Global.ParaCommon.IsForceByPassRS)
            {
                Global.EditTool.lbBypass.Visible = true;
               
               
            }
            else
            {
                Global.EditTool.lbBypass.Visible = false;
              
            }
        }

        private void numRetryCam_ValueChanged(float obj)
        {
            Global.ParaCommon.NumRetryCamera =(int) numRetryCam.Value;
        }

        private void numRetryPLC_ValueChanged(float obj)
        {
            Global.ParaCommon.NumRetryPLC = (int)numRetryPLC.Value;
        }

        private void btnResetReady_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsResetReady = btnResetReady.IsCLick;
        }

        private void btnShowLabel_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowLabel=btnShowLabel.IsCLick;
        }

        private void btnShowScore_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowScore = btnShowScore.IsCLick;
        }
    }
}
