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

namespace BeeInterface
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
            this.Width = Global.EditTool.BtnHeaderBar.Width + 1;
           this.Height=Global.SizeScreen.Height- Global.EditTool.BtnHeaderBar.Height-20;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - Global.EditTool.BtnHeaderBar.Width - 1, Global.EditTool.pTop.Height);// Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
            btnNG.IsCLick = Global.Config.IsONNG;
            btnOK.IsCLick = !Global.Config.IsONNG;
            btnSaveOK.IsCLick = Global.Config.IsSaveOK;
            btnSaveNG.IsCLick = Global.Config.IsSaveNG;
            btnSaveRaw.IsCLick = Global.Config.IsSaveRaw;
            btnSaveRS.IsCLick = Global.Config.IsSaveRS;
            btnShowFullScreen.IsCLick = Global.IsShowImageResult;

            btnWaitingCenter.IsCLick=Global.Config.IsShowFull;
            if (Global.Config.IsShowFull)
            {
                btnShowFullScreen.Text = "ON";
            }
            else
            { btnShowFullScreen.Text = "OFF"; }
            if (Global.Config.IsWaitCenter)
                btnWaitingCenter.Text = "ON";
            else
                btnWaitingCenter.Text = "OFF";
            layAutoTrig.Visible = Global.Config.IsAutoTrigger;
            btnDisPLC.IsCLick = Global.IsDisnablePLc; ;
            if (btnDisPLC.IsCLick)
                btnDisPLC.Text = "ON";
            else btnDisPLC.Text = "OFF";
            numFlowChart.Value = Global.Config.NumTrig;
            btnMulti.IsCLick = Global.Config.IsMultiProg;
            btnSingle.IsCLick = !Global.Config.IsMultiProg;
            switch (Global.Config.TypeSave)
            {
                case 1: btnSmall.IsCLick = true; break;
                case 2: btnNormal.IsCLick = true; break;
                case 3: btnbig.IsCLick = true; break;
            }
            AdjLimitDay.Value = Global.Config.LimitDateSave;
            AdjFontSize.Value = Global.ParaShow.FontSize;
            AdjRadEdit.Value = Global.ParaShow.RadEdit;
            btnShowBox.IsCLick = Global.ParaShow.IsShowBox;
            btnShowDetail.IsCLick = Global.ParaShow.IsShowDetail;
            btnShowPositon.IsCLick = Global.ParaShow.IsShowPostion;
            btnShowScore.IsCLick = Global.ParaShow.IsShowScore;
            btnShowLabel.IsCLick = Global.ParaShow.IsShowLabel;

            lbClOK.BackColor = Global.ParaShow.ColorOK;
            btnClOK.ForeColor = Global.ParaShow.ColorOK;
            lbCLNG.BackColor = Global.ParaShow.ColorNG;
            btnClNG.ForeColor = Global.ParaShow.ColorNG;

            lbCLInfor.BackColor = Global.ParaShow.ColorInfor;
            btnCLInfor.ForeColor = Global.ParaShow.ColorInfor;
            lbCLNone.BackColor = Global.ParaShow.ColorNone;
            btnCLNone.ForeColor = Global.ParaShow.ColorNone;
            lbCLText.BackColor = Global.ParaShow.TextColor;
            btnCLText.ForeColor = Global.ParaShow.TextColor;
            lbClChoose.BackColor = Global.ParaShow.ColorChoose;
            btnClChoose.ForeColor = Global.ParaShow.ColorChoose;
            AdjOpacity.Value = Global.ParaShow.Opacity;
            AdjThicknessLine.Value = Global.ParaShow.ThicknessLine;
            btnByPassResult.IsCLick = Global.Config.IsForceByPassRS;
            btnFullDisplay.IsCLick = Global.Config.DisplayResolution == DisplayResolution.Full ? true : false;
            btnNormalDisplay.IsCLick = Global.Config.DisplayResolution == DisplayResolution.Normal ? true : false;
            btnShowProgessing.IsCLick = Global.Config.IsShowProgressingPLC;
            btnHideProgessing.IsCLick =! Global.Config.IsShowProgressingPLC;
            btnOnAutoTrigger.IsCLick = Global.Config.IsAutoTrigger;
            btnOffAutoTrigger.IsCLick = !Global.Config.IsAutoTrigger;
            btnONResetImg.IsCLick = Global.Config.IsResetImg;
            btnOFFResetImg.IsCLick = !Global.Config.IsResetImg;
            if (Global.Config.IsForceByPassRS)
                btnByPassResult.Text = "ON";
            else
                btnByPassResult.Text = "OFF";
            if (Global.Config.IsForceByPassRS)
            {
                Global.EditTool.lbBypass.Visible = true;
            }
            else
            {
                Global.EditTool.lbBypass.Visible = false;

            }
            btnResetReady.IsCLick = Global.Config.IsResetReady;
            numRetryCam.Value = Global.Config.NumRetryCamera;
            numRetryPLC.Value = Global.Config.NumRetryPLC;
            btnSaveCommon.IsCLick = Global.Config.IsSaveCommon;
            btnSaveParaCamera.IsCLick = Global.Config.IsSaveParaCam;
            btnSaveGraphic.IsCLick = Global.Config.IsSaveParaShow;
            btnSaveImgRegister.IsCLick = Global.Config.IsSaveListRegister;
            btnSaveListImgSim.IsCLick = Global.Config.IsSaveListSim;
            btnEnPO.IsCLick = Global.Config.IsEnPO;
            btnEnDummy.IsCLick = Global.Config.IsEnDummy;
            btnEnTrain.IsCLick = Global.Config.IsEnTrain;
            btnCommonProg.IsCLick=Global.Config.IsSaveProg;
            btnParaPLC.IsCLick = Global.Config.IsSaveCommunication;
            ShowNo();
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
            Global.Config.IsMultiProg = btnMulti.IsCLick;
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            Global.Config.IsMultiProg = !btnSingle.IsCLick;
        }





        private void numTrigger_ValueChanged(float obj)
        {
            Global.Config.NumTrig = (int)numFlowChart.Value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //SaveData.ParaPJ(Global.Project, Global.ParaCommon);
            SaveData.Config(Global.Config);
            SaveData.ParaShow(Global.Project, Global.ParaShow);
          
            ShowTool.ShowAllChart(Global.ToolSettings.pAllTool);
            this.Close();
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
            Global.Config.IsONNG = !btnOK.IsCLick;
        }

        private void btnNG_Click(object sender, EventArgs e)
        {
            Global.Config.IsONNG = btnNG.IsCLick;
        }

        private void AdjRadEdit_ValueChanged(float obj)
        {
            Global.ParaShow.RadEdit = (int)AdjRadEdit.Value;
        }

        private void btnShowBox_Click(object sender, EventArgs e)
        {
            Global.ParaShow.IsShowBox = btnShowBox.IsCLick;
        }

        private void btnShowDetail_Click(object sender, EventArgs e)
        {
            Global.ParaShow.IsShowDetail = btnShowDetail.IsCLick;
        }

        private void btnShowPositon_Click(object sender, EventArgs e)
        {
            Global.ParaShow.IsShowPostion = btnShowPositon.IsCLick;
        }

        private void AdjFontSize_ValueChanged(float obj)
        {
            Global.ParaShow.FontSize = (int)AdjFontSize.Value;
        }

        private void btnChooseCLOK_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.ParaShow.ColorOK = colorChoose.Color;
                lbClOK.BackColor = Global.ParaShow.ColorOK;
                btnClOK.ForeColor = Global.ParaShow.ColorOK;
            }
        }

        private void btnChooseNG_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.ParaShow.ColorNG = colorChoose.Color;
                lbCLNG.BackColor = Global.ParaShow.ColorNG;
                btnClNG.ForeColor = Global.ParaShow.ColorNG;
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            Global.ParaShow.ColorOK = Color.FromArgb(0, 172, 73);
            lbClOK.BackColor = Global.ParaShow.ColorOK;
            btnClOK.ForeColor = Global.ParaShow.ColorOK;
        }

        private void btnDefaultNg_Click(object sender, EventArgs e)
        {
            Global.ParaShow.ColorNG = Color.DarkRed;
            lbCLNG.BackColor = Global.ParaShow.ColorNG;
            btnClNG.ForeColor = Global.ParaShow.ColorNG;
        }

        private void AdjOpacity_ValueChanged(float obj)
        {
            Global.ParaShow.Opacity = (int)AdjOpacity.Value;
        }

        private void AdjThinkness_ValueChanged(float obj)
        {
            Global.ParaShow.ThicknessLine = (int)AdjThicknessLine.Value;
        }

        private void btnChooseCLInfor_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.ParaShow.ColorInfor = colorChoose.Color;
                lbCLInfor.BackColor = Global.ParaShow.ColorInfor;
                btnCLInfor.ForeColor = Global.ParaShow.ColorInfor;
            }
        }

        private void btnDefInfor_Click(object sender, EventArgs e)
        {
            Global.ParaShow.ColorInfor = Color.Blue;
            lbCLInfor.BackColor = Global.ParaShow.ColorInfor;
            btnCLInfor.ForeColor = Global.ParaShow.ColorInfor;
        }

        private void btnChooseNone_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.ParaShow.ColorNone = colorChoose.Color;
                lbCLNone.BackColor = Global.ParaShow.ColorNone;
                btnCLNone.ForeColor = Global.ParaShow.ColorNone;
            }
        }

        private void btnDefNone_Click(object sender, EventArgs e)
        {
            Global.ParaShow.ColorNone = Color.LightGray;
            lbCLNone.BackColor = Global.ParaShow.ColorNone;
            btnCLNone.ForeColor = Global.ParaShow.ColorNone;
        }

        private void btnChooseCLChoose_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.ParaShow.ColorChoose = colorChoose.Color;
                lbClChoose.BackColor = Global.ParaShow.ColorChoose;
                btnClChoose.ForeColor = Global.ParaShow.ColorChoose;
            }
        }

        private void btnDefaultChoose_Click(object sender, EventArgs e)
        {
            Global.ParaShow.ColorChoose = Color.FromArgb(246, 204, 120);
            lbClChoose.BackColor = Global.ParaShow.ColorChoose;
            btnClChoose.ForeColor = Global.ParaShow.ColorChoose;
        }

        private void btnChooseCLText_Click(object sender, EventArgs e)
        {
            if (colorChoose.ShowDialog() == DialogResult.OK)
            {
                Global.ParaShow.TextColor = colorChoose.Color;
                lbCLText.BackColor = Global.ParaShow.TextColor;
                btnCLText.ForeColor = Global.ParaShow.TextColor;
            }
        }

        private void btnDefaultCLText_Click(object sender, EventArgs e)
        {
            Global.ParaShow.TextColor = Color.White;
            lbCLText.BackColor = Global.ParaShow.TextColor;
            btnCLText.ForeColor = Global.ParaShow.TextColor;
        }

        private void btnByPassResult_Click(object sender, EventArgs e)
        {
            Global.Config.IsForceByPassRS = btnByPassResult.IsCLick;
            if (Global.Config.IsForceByPassRS)
                btnByPassResult.Text = "ON";
            else
                btnByPassResult.Text = "OFF";
            if (Global.Config.IsForceByPassRS)
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
            Global.Config.NumRetryCamera = (int)numRetryCam.Value;
        }

        private void numRetryPLC_ValueChanged(float obj)
        {
            Global.Config.NumRetryPLC = (int)numRetryPLC.Value;
        }

        private void btnResetReady_Click(object sender, EventArgs e)
        {
            Global.Config.IsResetReady = btnResetReady.IsCLick;
        }

        private void btnShowLabel_Click(object sender, EventArgs e)
        {
            Global.ParaShow.IsShowLabel = btnShowLabel.IsCLick;
        }

        private void btnShowScore_Click(object sender, EventArgs e)
        {
            Global.ParaShow.IsShowScore = btnShowScore.IsCLick;
        }

        private void btnFullDisplay_Click(object sender, EventArgs e)
        {
            Global.Config.DisplayResolution = DisplayResolution.Full;
        }

        private void btnNormalDisplay_Click(object sender, EventArgs e)
        {
            Global.Config.DisplayResolution = DisplayResolution.Normal;
        }

     
        private void btnOFFResetImg_Click(object sender, EventArgs e)
        {
            Global.Config.IsResetImg = false;
        }

        private void btnONResetImg_Click(object sender, EventArgs e)
        {
            Global.Config.IsResetImg = true;
        }

        private void btnOnAutoTrigger_Click(object sender, EventArgs e)
        {
            Global.Config.IsAutoTrigger = true;
            layAutoTrig.Visible = Global.Config.IsAutoTrigger;
        }

        private void btnOffAutoTrigger_Click(object sender, EventArgs e)
        {
            Global.Config.IsAutoTrigger = false;
            Global.Config.IsWaitCenter = false;
            btnWaitingCenter.IsCLick = Global.Config.IsWaitCenter;
            if (Global.Config.IsWaitCenter)
                btnWaitingCenter.Text = "ON";
            else
                btnWaitingCenter.Text = "OFF";
            layAutoTrig.Visible = Global.Config.IsAutoTrigger;
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSaveImgRegister_Click(object sender, EventArgs e)
        {
            Global.Config.IsSaveListRegister = btnSaveImgRegister.IsCLick;
        }

        private void btnSaveListImgSim_Click(object sender, EventArgs e)
        {
            Global.Config.IsSaveListSim = btnSaveListImgSim.IsCLick;
        }

        private void btnParaPLC_Click(object sender, EventArgs e)
        {
            Global.Config.IsSaveCommunication = btnParaPLC.IsCLick;
        }

        private void btnSaveGraphic_Click(object sender, EventArgs e)
        {
            Global.Config.IsSaveParaShow = btnSaveGraphic.IsCLick;

        }

        private void btnSaveCommon_Click(object sender, EventArgs e)
        {
            Global.Config.IsSaveCommon = btnSaveCommon.IsCLick;
        }

        
        public void ShowNo()
        {
            lbListNo.Text = "";
            foreach (ProgNo no in Global.ListProgNo)
            {
                lbListNo.Text += no.No + "." + no.Name+Environment.NewLine;
            }
        }
        private void btnImportProgNo_Click(object sender, EventArgs e)
        {
            Global.ListProgNo = new List<ProgNo>();
            if (openFileDialog.ShowDialog()==DialogResult.OK)
            {
                String[] sConent=File.ReadAllLines(openFileDialog.FileName);
                foreach (String s in sConent)
                {
                   String[] col = s.Split('\t');
                    String Name = col[1].Replace("_", "").Trim();
                    int No = 0;
                    if (int.TryParse(col[0], out No))
                        Global.ListProgNo.Add(new ProgNo(No, Name));

                }
                if (Global.ListProgNo.Count > 0)
                    SaveData.ProgNo(Global.ListProgNo);
                ShowNo();
            }    
        }

        private async void btnDisPLC_Click(object sender, EventArgs e)
        {
            Global.IsDisnablePLc = btnDisPLC.IsCLick;
            if (btnDisPLC.IsCLick) btnDisPLC.Text = "ON";
            else btnDisPLC.Text = "OFF";
            if(Global.Comunication.Protocol.IsConnected)
            {
                Global.Comunication.Protocol.SetOutPut(Global.Comunication.Protocol.AddressOutPut[(int)I_O_Output.ByPass], btnDisPLC.IsCLick);
                await Global.Comunication.Protocol.WriteOutPut();

            }
            else
            {
                btnDisPLC.IsCLick = false;
                Global.IsDisnablePLc = false;
                btnDisPLC.Text = "OFF";
            }

        }

        private void btnShowProgessing_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowProgressingPLC = btnShowProgessing.IsCLick;
            Global.EditTool.StepProccessBar.Visible = Global.Config.IsShowProgressingPLC;
        }

        private void btnHideProgessing_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowProgressingPLC = !btnHideProgessing.IsCLick;
            Global.EditTool.StepProccessBar.Visible = Global.Config.IsShowProgressingPLC;
        }

        private void btnP1_Click(object sender, EventArgs e)
        {
            lay1.Visible = !btnP1.IsCLick;
            layAutoTrig.Visible = !btnP1.IsCLick;
        }

        private void btnP2_Click(object sender, EventArgs e)
        {
            layP2.Visible= !btnP2.IsCLick;
        }

        private void btnP3_Click(object sender, EventArgs e)
        {
            numFlowChart.Visible = !btnP3.IsCLick;
        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

      

        private void btn4_Click(object sender, EventArgs e)
        {
            layShowProgressing.Visible = !btnP4.IsCLick;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {

        }

        private void btnD1_Click(object sender, EventArgs e)
        {
            laySetSaveProg.Visible = !btnD1.IsCLick;
        }

        private void btnSetSaveReport_Click(object sender, EventArgs e)
        {
            laySetReport.Visible = !btnD2.IsCLick;
        }

        private void btnP5_Click(object sender, EventArgs e)
        {
            layEnFunction.Visible= !btnP5.IsCLick;
        }

        private void btnEnDummy_Click(object sender, EventArgs e)
        {
            Global.Config.IsEnDummy=btnEnDummy.IsCLick;
            Global.EditTool.pHeader. pDummy.Visible = Global.Config.IsEnDummy;
            Global.EditTool.pHeader.spDummy.Visible = Global.Config.IsEnDummy;
            
        }

        private void btnEnTrain_Click(object sender, EventArgs e)
        {
            Global.Config.IsEnTrain = btnEnTrain.IsCLick;
            Global.EditTool.pHeader.pTrain.Visible = Global.Config.IsEnTrain;
            Global.EditTool.pHeader.spTrain.Visible = Global.Config.IsEnTrain;
           
        }

        private void btnEnPO_Click(object sender, EventArgs e)
        {
            Global.Config.IsEnPO = btnEnPO.IsCLick;
            Global.EditTool.pHeader.pPO.Visible = Global.Config.IsEnPO;
        }

        private void btnWaitingCenter_Click(object sender, EventArgs e)
        {
            Global.Config.IsWaitCenter=btnWaitingCenter.IsCLick;
            if (Global.Config.IsWaitCenter)
            btnWaitingCenter.Text = "ON";
            else
                btnWaitingCenter.Text = "OFF";
        }

        private void btnSaveParaCamera_Click(object sender, EventArgs e)
        {
            Global.Config.IsSaveParaCam = btnSaveParaCamera.IsCLick;
        }

        private void btnCommonProg_Click(object sender, EventArgs e)
        {
            Global.Config.IsSaveProg=btnCommonProg.IsCLick;
        }

        private void btnShowFullScreen_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowFull=btnShowFullScreen.IsCLick;
            if (Global.Config.IsShowFull)
            {
                btnShowFullScreen.Text = "ON";
            }
            else
            { btnShowFullScreen.Text = "OFF"; }
        }
    }
}
