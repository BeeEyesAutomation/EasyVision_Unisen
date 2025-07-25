﻿using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using BeeUi.Commons;
using BeeUi.Tool;
using BeeUi.Unit;
using Newtonsoft.Json.Linq;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BeeUi
{
    [Serializable()]
    public partial class EditTool : UserControl
    {
        public EditTool()
        {
            InitializeComponent();
           // BeeCore.CustomGui.RoundControl(picLogo,Global.Config.RoundRad);
         
        }
        public void RefreshGuiEdit( Step Step)
        {
            try
            {
                Global.IndexToolSelected = -1;
            X: switch (Step)
                {
                    case Step.PLC:
                        pName.Visible = false;
                        if (G.SettingPLC == null)
                            G.SettingPLC = new SettingPLC();
                        pEditTool.Controls.Clear();
                        pEditTool.Visible = true;
                        G.SettingPLC.Visible = true;
                        G.SettingPLC.Parent = pEditTool;
                        G.SettingPLC.Dock = DockStyle.Fill;
                        break;
                    case Step.Run:
                        pName.Visible = false;
                        if (Global.ToolSettings == null)
                            Global.ToolSettings = new ToolSettings();
                        pEditTool.Controls.Clear();
                      
                        Global.ToolSettings.Visible = true;
                      
                        G.SettingPLC.Visible = false;
                        //pEditTool.Visible = true;
                        Global.ToolSettings.Parent = pEditTool;
                        Global.ToolSettings.Size = pEditTool.Size;
                        Global.ToolSettings.Location = new Point(0, 0);
                        Global.ToolSettings.BringToFront();
                        Global.ToolSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                        Global.ToolSettings.pAllTool.Visible = true;
                        Global.ToolSettings.Dock = DockStyle.None;
                        G.EditTool.View.pHeader.Controls.Clear();
                        if ( G.StatusDashboard == null)
                        {
                             G.StatusDashboard = new StatusDashboard();
                            G.StatusDashboard.InfoBlockBackColor = Color.FromArgb(Global.Config. AlphaBar-50,Global.Config.colorGui.R,Global.Config.colorGui.G,Global.Config.colorGui.B);
                            G.StatusDashboard.StatusBlockBackColor = Color.FromArgb(Global.Config.AlphaBar - 50,Global.Config.colorGui.R,Global.Config.colorGui.G,Global.Config.colorGui.B);
                            G.StatusDashboard.MidHeaderBackColor = Color.FromArgb(Global.Config.AlphaBar,Global.Config.colorGui.R,Global.Config.colorGui.G,Global.Config.colorGui.B);

                        }

                         G.StatusDashboard.Dock = DockStyle.None;
                         G.StatusDashboard.Parent = G.EditTool.View.pHeader;
                         G.StatusDashboard.Location = new Point(0, 0);
                         G.StatusDashboard.Size = G.EditTool.View.pHeader.Size;
                         G.StatusDashboard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                        try
                        {
                            if (Global.ParaCommon.matRegister != null)
                                if (Global.ParaCommon.matRegister.Width != 0)
                                {
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Global.ParaCommon.matRegister.ToMat().Clone();
                                    G.IsCalib = false;
                                    G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                                    G.EditTool.View.imgView.Invalidate();
                                    G.EditTool.View.imgView.Update();
                                    Shows.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                                   Global.Config.imgZoom = View.imgView.Zoom;
                                   Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                   Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
                                }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    case Step.Step1:
                       
                                G.EditTool.View.pHeader.Controls.Clear();
                        G.StepEdit.btnStep1.IsCLick = true;
                        if (G.StepEdit.SettingStep1 == null)
                        {
                            G.StepEdit.SettingStep1 = new SettingStep1();
                            G.StepEdit.SettingStep1.Dock = DockStyle.Fill;

                        }

                        pEditTool.Controls.Clear();
                        G.StepEdit.SettingStep1.Parent = pEditTool;

                        G.EditTool.View.pHeader.Controls.Clear();
                        if (G.StepEdit == null)
                        {
                            G.StepEdit = new Common.StepEdit();


                        }
                        //foreach (Tools tool in G.listAlltool[Global.IndexChoose])
                        //{
                        //    tool.ItemTool.Score.Enabled = false;
                        //}
                        G.StepEdit.Dock = DockStyle.None;
                        G.StepEdit.Parent = G.EditTool.View.pHeader;
                        G.StepEdit.Location = new Point(0, 0);
                        G.StepEdit.Size = G.EditTool.View.pHeader.Size;
                        G.StepEdit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

                        lbNumStep.Text = "Step 1";
                        lbNameStep.Text = "Image Optimization";
                        iconTool.BackgroundImage = Properties.Resources._1;
                        lbTool.Text = "Setup Camera";

                        pName.Visible = true;
                        break;
                    case Step.Step2:
                        pName.Visible = true;
                        G.IsCalib = false;
                        if (G.StepEdit.SettingStep2 == null)
                            G.StepEdit.SettingStep2 = new SettingStep2();
                        pEditTool.Controls.Clear();
                        G.StepEdit.SettingStep2.Parent = G.EditTool.pEditTool;
                        G.StepEdit.SettingStep2.Dock = DockStyle.Fill;
                        lbNumStep.Text = "Step 2";
                        lbNameStep.Text = "Master Resgistration";
                        iconTool.BackgroundImage = Properties.Resources._2;
                        lbTool.Text = "Register Image";
                        try
                        {
                            if (Global.ParaCommon.matRegister != null)
                                if (Global.ParaCommon.matRegister.Width != 0)
                                {
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Global.ParaCommon.matRegister.ToMat().Clone();
                                    G.IsCalib = false;
                                    G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                                    G.EditTool.View.imgView.Invalidate();
                                    G.EditTool.View.imgView.Update();
                                    Shows.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                                   Global.Config.imgZoom = View.imgView.Zoom;
                                   Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                   Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
                                }
                        }
                        catch (Exception ex)
                        {

                        }
                        //G.EditTool.View.imgView.Image = Global.ParaCommon.matRegister;
                        //G.EditTool.View.imgView.Invalidate();
                        //G.EditTool.View.imgView.Update();
                        break;
                    case Step.Step3:
                        pName.Visible = true;
                        if (Global.ToolSettings == null)
                            Global.ToolSettings = new ToolSettings();
                        pEditTool.Controls.Clear();
                        Global.ToolSettings.Parent = pEditTool;
                        Global.ToolSettings.Size = pEditTool.Size;
                        Global.ToolSettings.Location = new Point(0, 0);
                        Global.ToolSettings.pAllTool.Visible = true;
                        Global.ToolSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                      
                     
                        Global.ToolSettings.BringToFront();
                        if (Global.ParaCommon.matRegister != null)
                        {
                            G.IsCalib = false;
                            pEditTool.Visible = true;
                            lbNumStep.Text = "Step 3";
                            lbNameStep.Text = "Tool Setting";
                            iconTool.BackgroundImage = Properties.Resources._3;
                            lbTool.Text = " Add tool and Modify  Tool";
                        }
                        else
                        {
                            MessageBox.Show("Please,Register Image!");
                            Step = Step.Step2;
                            goto X;
                        }
                        if (Global.ToolSettings.btnEnEdit.IsCLick)
                            Global.ToolSettings.btnEnEdit.PerformClick();

                        break;
                    case Step.Step4:
                        pName.Visible = true;
                        G.IsCalib = false;
                        if (G.StepEdit.SettingStep4 == null)
                            G.StepEdit.SettingStep4 = new SettingStep4();
                        pEditTool.Controls.Clear();
                        G.StepEdit.SettingStep4.Parent = G.EditTool.pEditTool;
                        G.StepEdit.SettingStep4.Dock = DockStyle.Fill;
                        lbNumStep.Text = "Step 4";
                        lbNameStep.Text = "Output Assignment";
                        iconTool.BackgroundImage = Properties.Resources._4;
                        lbTool.Text = "Setup Status OutPut";
                        G.StepEdit.SettingStep4.RefreshLogic();
                        break;
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }
        public async void DesTroy()
        {
            View.tmContinuous.Enabled = false;
           
           foreach(Camera camera in BeeCore.Common.listCamera)
                if(camera!=null)
				camera.DestroyAll();

            View.tmContinuous.Enabled = false;
           
            if (Global.ParaCommon.Comunication.IO.IsConnected)
            {
                Global.ParaCommon.Comunication.IO.WriteIO(IO_Processing.Close);
            }

          
            await Task.Delay(100);
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
          
           
          
           
        }

        public View View;
     
        private void stepEdit1_Load(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void stepEdit1_Load_1(object sender, EventArgs e)
        {

        }

        private void View_Load(object sender, EventArgs e)
        {

        }

        private void outLine11_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

       
        private void EditTool_Load(object sender, EventArgs e)
        {
            G.EditTool.lbLicence.Text = "Licence: " + G.Licence;
            LayoutMain.BackColor= CustomGui.BackColor(TypeCtr.BG,Global.Config.colorGui);
           
        }

        private void outLine_Load(object sender, EventArgs e)
        {

        }
       
       
    
        int indexTool = 0;
        private void btnTool_Click(object sender, EventArgs e)
        {
            //indexTool++;
            //if (indexTool < Enum.GetNames(typeof(TypeTool)).Length)
            //{
            //    LoadTool((TypeTool)indexTool);
            //    lbTool.Text = Convert.ToString((TypeTool)(indexTool));
            //}
            //else
            //{
            //    indexTool = 0;
            //    LoadTool((TypeTool)indexTool);
            //    lbTool.Text = Convert.ToString((TypeTool)(indexTool));
            //}
              
        }

        private void pEditTool_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pEditTool_ControlAdded(object sender, ControlEventArgs e)
        {
          
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           
        }

        private void header1_Load(object sender, EventArgs e)
        {

        }

        private void stepEdit1_Load_2(object sender, EventArgs e)
        {

        }

        private void header1_Load_1(object sender, EventArgs e)
        {

        }

        private void pEditTool_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (File.Exists("Default.config"))
                File.Delete("Default.config");
            Access.SaveConfig("Default.config",Global.Config);
            G.Main.Close();
        }

       

        private void lbHistory_Click(object sender, EventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
          
        }

        private void pName_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {
          
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
        
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
           
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void header1_Load_2(object sender, EventArgs e)
        {

        }
        public int targetWidth = 0;
        private void LayOutShow_Layout(object sender, LayoutEventArgs e)
        {

        }
        int value = -1;
        int valueOld = -1;
        DateTime dtOld, dt2;

        private void pView_SizeChanged(object sender, EventArgs e)
        {

            if (G.EditTool == null) return;
            if (G.EditTool.View == null)
                    return;
           
            G.EditTool.View.Size = G.EditTool.pView.Size;
            G.EditTool.View.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            G.EditTool.View.Location = new Point(0, 0);
            G.EditTool.View.Dock = DockStyle.None;
            G.EditTool.View.Parent = G.EditTool.pView;

        }

        private void lbLicence_Click(object sender, EventArgs e)
        {
          
        }

        private void btnShuttdown_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Shutdown", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                G.IsShutDown = true;
                btnExit.PerformClick();
            }
        }

        private void toolStripPort_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void toolStripPort_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure", "byPass PLC",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                toolStripPort.Text = "ByPass PLC";
                Global.ParaCommon.Comunication.IO.IsBypass = true;
            }
        }

        private void lbLicence_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure", "Initial Python", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
              
                //foreach (Tools tool in G.listAlltool[Global.IndexChoose])
                //    tool.tool.LoadPara();
              //  G.Header.workLoadProgram.RunWorkerAsync();
            }
        }

        private void btnHeaderBar1_Load(object sender, EventArgs e)
        {

        }

        private void tmReaPLC_Tick(object sender, EventArgs e)
        {
            //value= G.Header.Modbus.ReadHolding(4096);
            //if(value!=0&&value!= valueOld)
            //{
            //    TimeSpan sp = DateTime.Now - dtOld;
            //    label4.Text = value.ToString();
            //    label3.Text =Math.Round( sp.TotalMilliseconds).ToString() + "ms";
            //    dtOld = DateTime.Now;
            //  valueOld = value;
            //}
        }
    }
}
