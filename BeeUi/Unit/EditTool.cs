﻿ using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using BeeUi.Commons;
using BeeUi.Tool;
using BeeUi.Unit;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using Point = System.Drawing.Point;

namespace BeeUi
{
    [Serializable()]
    public partial class EditTool : UserControl
    {
        private LayoutPersistence _layout;
        public EditTool()
        {
          


           

            InitializeComponent();
            //this.DoubleBuffered = true;
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
           
            //this.AutoScaleMode = AutoScaleMode.Dpi; // hoặc AutoScaleMode.Font
            _layout = new LayoutPersistence(this, key: "MainLayout");
            _layout.LoadDelayMs = 300;        // trễ 500ms sau Form.Shown
            _layout.SplitterLocked = true;   // tuỳ chọn
            _layout.EnableAuto();
            ///  _layout.EnableAuto(); // tự load sau Shown, tự save khi Closing
            // BeeCore.CustomGui.RoundControl(picLogo,Global.Config.RoundRad);

        }
		public void Acccess(bool IsRun)
		{
            BtnHeaderBar.btnUser.Text = Global.Config.Users.ToString();
			View.btnLive.Enabled = !Global.IsRun;
        
            Global.ToolSettings.btnAdd.Enabled = !Global.IsRun;
            Global.ToolSettings.btnCopy.Enabled = !Global.IsRun;
            Global.ToolSettings.btnDelect.Enabled = !Global.IsRun;
            Global.ToolSettings.btnEnEdit.Enabled = Global.IsRun;
            Global.ToolSettings.btnRename.Enabled = !Global.IsRun;
            switch (Global.Config.Users)
            {
                case Users.Admin:
					G.StatusDashboard.btnReset.Enabled = true;
					Global.EditTool.View.btnContinuous.Enabled = Global.IsRun;
					G.Header.btnMode.Enabled = true;
                    View.pBtn.Enabled= true;
					G.Header.pEdit.Enabled = true;
					View.btnTypeTrig.Enabled = true;
					BarRight.btnFlowChart.Enabled = true;
					BarRight.btnHistory.Enabled = true;
					BarRight.btnHardware.Enabled = true;
					BarRight.btnLog.Enabled = true;
                    btnLogo.Enabled = true;
					break;
				case Users.Leader:
					Global.EditTool.View.btnContinuous.Enabled = false;
					G.StatusDashboard.btnReset.Enabled = true;
					G.Header.btnMode.Enabled = true;
					View.pBtn.Enabled = true;
					View.btnTypeTrig.Enabled = false;
					G.Header.pEdit.Enabled = true;
					BarRight.btnFlowChart.Enabled = true;
					BarRight.btnHistory.Enabled = true;
					BarRight.btnHardware.Enabled = false;
					BarRight.btnLog.Enabled = false;
					btnLogo.Enabled = false;
					break;
				case Users.User:
					Global.EditTool.View.btnContinuous.Enabled = false;
					G.Header.btnMode.Enabled = false;
			         G.StatusDashboard.btnReset.Enabled = false;
					View.pBtn.Enabled = false;
					View.btnTypeTrig.Enabled = false;
					G.Header.pEdit.Enabled = false;
					BarRight.btnFlowChart.Enabled = true;
					BarRight.btnHistory.Enabled = false;
					BarRight.btnHardware.Enabled = false;
					BarRight.btnLog.Enabled = false;
					btnLogo.Enabled = false;
					break;
			
			}
			
		
			if (Global.ParaCommon.IsExternal)
			{
				Global.EditTool.View.btnCap.Enabled = false;
				Global.EditTool.View.btnContinuous.Enabled = false;
            }
            else
            {
                Global.EditTool.View.btnCap.Enabled = Global.IsRun;
                Global.EditTool.View.btnContinuous.Enabled = Global.IsRun;
            }    
          

                Global.EditTool.BtnHeaderBar.btnUser.Text = Global.Config.Users.ToString();
		}
		void EnableDoubleBuffer(Control c)
        {
            var t = c.GetType();
            var piDB = t.GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            piDB?.SetValue(c, true, null);

            // giúp redraw mượt khi resize
            var piRR = t.GetProperty("ResizeRedraw",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            piRR?.SetValue(c, true, null);
        }
        public void RefreshGuiEdit( Step Step)
        {
            try
            {
            
                if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
                {
                    BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(new ParaCamera(), Global.IndexChoose);
                    Global.ScanCCD.ShowDialog();
                    return;
                }
              
                Global.IndexToolSelected = -1;
                if(Global.EditTool.View.btnLive.IsCLick)
                {
                    Global.EditTool.View. btnLive.PerformClick();

                }
                BarRight.btnFlowChart.IsCLick = true;
                Global.StatusDraw = StatusDraw.None;
            X: switch (Step)
                {
                   
                    case Step.Run:
                        View.pMenu.Visible = false;
                        Global.IsAllowReadPLC = true;
                        ShowTool.ShowAllChart(Global.ToolSettings.pAllTool);
                        Global.EditTool.View.btnLive.Enabled = false;
                        BarRight.Visible = true;
                        pHeader.Visible = true;
                        Global.IsRun = true; Acccess(Global.IsRun);
                        G.Header.btnMode.Text = "Run";
                        G.Header.btnMode.IsCLick = false;
                        pName.Visible = false;
                        G.SettingPLC.Visible = false;
                        pInfor.Show("Dashboard");
                        pEditTool.Show("Tool");
                   
                        try
                        {
                            if (Global.ParaCommon.matRegister!=null)
                                if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
                            if (!Global.ParaCommon.matRegister.IsDisposed())
                            {
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = new Mat();
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Global.ParaCommon.matRegister.ToMat().Clone();
                                    G.IsCalib = false;
                                    Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                                    Global.EditTool.View.imgView.Invalidate();
                                    Global.EditTool.View.imgView.Update();
                                    ShowTool.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                                   Global.Config.imgZoom = View.imgView.Zoom;
                                   Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                   Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
                                }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                     
                      View.RefreshExternal(Global.ParaCommon.IsExternal);
                        break;
                    case Step.Step1:
                        View.pMenu.Visible = true;
                        Global.IsAllowReadPLC = false;
                       
                        pHeader.Visible = false;
                        BarRight.Visible = false;
                        Global.IsRun = false; Acccess(Global.IsRun);
                        Global.EditTool.View.btnLive.Enabled = true;
                        G.Header.btnMode.Text = "Edit";
                        G.Header.btnMode.IsCLick = true;
                   
                        G.StepEdit.btnStep1.IsCLick = true;

                        
                       
                       
                      

                        //G.StepEdit.SettingStep1.Size = Global.EditTool.pEditTool.Size;
                        //G.StepEdit.Visible = true;
                        //G.StatusDashboard.Visible = false;
                        pEditTool.Show("Step1");
                        pInfor.Show("StepEdit");
                        //G.StepEdit.SettingStep1.Visible = true;
                        //foreach (Control child in G.StepEdit.SettingStep1.Controls)
                        //{
                        //    child.Visible = true;
                        //}

                        //Global.ToolSettings.Visible = false;
                        //G.StepEdit.SettingStep1.Visible = true;
                        //G.StepEdit.SettingStep1.Parent = pEditTool;
                        //G.StepEdit.SettingStep1.BringToFront();

                        //foreach (Tools tool in G.listAlltool[Global.IndexChoose])
                        //{
                        //    tool.ItemTool.Score.Enabled = false;
                        //}

                        //G.StepEdit.Parent = Global.EditTool.View.pHeader;

                        //G.StepEdit.Size = Global.EditTool.View.pHeader.Size;
                        //G.StepEdit.BringToFront();

                        //iconTool.BackgroundImage = Properties.Resources._1;
                        lbTool.Text = "Setup Camera";

                      ///  pName.Visible = true;
                       

                       //this.ResumeLayout();
                        break;
                    case Step.Step2:
                        Global.IsAllowReadPLC = false;
                        Global.EditTool.View.btnLive.Enabled = false;
                        G.StepEdit.btnStep2.IsCLick = true;
                        //   pName.Visible = true;
                        G.IsCalib = false;

                        pEditTool.Show("Step2");

                        //iconTool.BackgroundImage = Properties.Resources._2;
                        lbTool.Text = "Register Image";
                        try
                        {
                            if (Global.ParaCommon.matRegister != null)
                                if (!Global.ParaCommon.matRegister.IsDisposed())
                            {
                                if (Global.ParaCommon.matRegister.Width != 0)
                                {
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Global.ParaCommon.matRegister.ToMat().Clone();
                                    G.IsCalib = false;
                                    Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                                    Global.EditTool.View.imgView.Invalidate();
                                    Global.EditTool.View.imgView.Update();
                                    ShowTool.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                                    Global.Config.imgZoom = View.imgView.Zoom;
                                    Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                    Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
                                }
                            }
                            else
                            {

                            }    
                        }
                        catch (Exception ex)
                        {

                        }
                        //Global.EditTool.View.imgView.Image = Global.ParaCommon.matRegister;
                        //Global.EditTool.View.imgView.Invalidate();
                        //Global.EditTool.View.imgView.Update();
                        break;
                    case Step.Step3:
                        Global.IsAllowReadPLC = false;
                        Global.EditTool.View.btnLive.Enabled = false;
                        G.StepEdit.btnStep3.IsCLick = true;
                        pName.Visible = true;
                       
                        pEditTool.Show("Tool");
                        ShowTool.ShowChart( Global.ToolSettings.pAllTool, BeeCore.Common.PropetyTools[Global.IndexChoose]);
                        //Global.ToolSettings.Parent = pEditTool;
                        //Global.ToolSettings.Size = pEditTool.Size;
                        //Global.ToolSettings.Location = new Point(0, 0);
                        //Global.ToolSettings.pAllTool.Visible = true;
                        //Global.ToolSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

                        if (Global.ParaCommon.matRegister != null)
                            if (!Global.ParaCommon.matRegister .IsDisposed())
                        {
                            G.IsCalib = false;
                            pEditTool.Visible = true;
                           
                                if (Global.ParaCommon.matRegister.Width != 0)
                                {
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Global.ParaCommon.matRegister.ToMat().Clone();
                                }
                                    // iconTool.BackgroundImage = Properties.Resources._3;
                                    lbTool.Text = " Add tool and Modify  Tool";
                        }
                        else
                        {
                            MessageBox.Show("Please,Register Image!");
                            G.StepEdit.btnStep2.IsCLick = true;
                            Step = Step.Step2;
                            goto X;
                        }
                        if (Global.ToolSettings.btnEnEdit.IsCLick)
                            Global.ToolSettings.btnEnEdit.PerformClick();

                        break;
                    case Step.Step4:
                        Global.IsAllowReadPLC = false;
                        Global.EditTool.View.btnLive.Enabled = false;
                        G.StepEdit.btnStep4.IsCLick = true;
                        pName.Visible = true;
                        G.IsCalib = false;

                        pEditTool.Show("Step4");

                        //   iconTool.BackgroundImage = Properties.Resources._4;
                        lbTool.Text = "Setup Status OutPut";
                        G.StepEdit.SettingStep4.RefreshLogic();
                        break;
                }
                //if (Global.IsRun)
                //{

                   



                //   G.Header. btnMode.Text = "RUN";
                //    G.Header.btnMode.ForeColor = Color.FromArgb(101, 173, 245); ;// Color.DarkSlateGray;
                //    Global.Step = Step.Run;

                //}
                //else
                //{
                //    if (Global.EditTool.View.btnContinuous.IsCLick)
                //        if (Global.EditTool.View.btnContinuous.Enabled == true)
                //            Global.EditTool.View.btnContinuous.PerformClick();
                //    //     Global.EditTool.btnHeaderBar.btnSettingPLC.IsCLick = false;
                


                //}
                Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.ChangeMode;
         
               
            }
            
            catch (Exception ex)
            {
             //   MessageBox.Show(ex.Message);
            }
        
        }
        public async void DesTroy()
        {
            View.tmContinuous.Enabled = false;
            if(Global.LogsDashboard!=null)
            Global.LogsDashboard.Dispose();
           foreach (Camera camera in BeeCore.Common.listCamera)
                if(camera!=null)
				camera.DestroyAll();

            View.tmContinuous.Enabled = false;
           
            if (Global.ParaCommon.Comunication.Protocol.IsConnected)
            {
                Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.Close;
            }

          
            await Task.Delay(100);
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
          
           
          
           
        }

        public View View;
     
     

      
        MultiDockHost DockHost = new MultiDockHost { Dock = DockStyle.Fill };
      public  DashboardImages DashboardImages;
      public void LockSpilter(bool IsLock)
        {
			split0.Enabled = !IsLock;
            split1.Enabled = !IsLock;
			split2.Enabled = !IsLock;
			split3.Enabled = !IsLock;
			split4.Enabled = !IsLock;
			split5.Enabled = !IsLock;
			split6.Enabled = !IsLock;
			View.split2.Enabled = !IsLock;
			View.split3.Enabled = !IsLock;
			View.split4.Enabled = !IsLock;
			View.split5.Enabled = !IsLock;
			G.Header.split1.Enabled = !IsLock;
			G.Header.split2.Enabled = !IsLock;
			Global.ToolSettings.split1.Enabled = !IsLock;
			G.StatusDashboard.IsLockSplit = IsLock;
		}

		private void EditTool_Load(object sender, EventArgs e)
        {
           

			tmReLoadSplit.Enabled = true;
            BeeCore.CustomGui.RoundRg(pInfor, 20);
            //   this.pInfor.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, Global.Config.colorGui);
            pInfor.Height = (int)(pInfor.Height * Global.PerScaleHeight);



            Global.EditTool.lbLicence.Text = "Licence: " + G.Licence;

            pHeader.Height = (int)(pHeader.Height * Global.PerScaleHeight);
            pTop.Height = (int)(pTop.Height * Global.PerScaleHeight);
            pEdit.Width = (int)(pEdit.Width * Global.PerScaleWidth);
            if (Global.ToolSettings == null)
            {
                Global.ToolSettings = new ToolSettings();
                Global.ToolSettings.Location = new Point(0, 0);
                // Global.ToolSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                Global.ToolSettings.pAllTool.Visible = true;
                Global.ToolSettings.Dock = DockStyle.Fill;
            }
            if (G.StatusDashboard == null)
            {
                G.StatusDashboard = new StatusDashboard();
                G.StatusDashboard.InfoBlockBackColor = Color.FromArgb(Global.Config.AlphaBar - 50, Global.Config.colorGui.R, Global.Config.colorGui.G, Global.Config.colorGui.B);
                G.StatusDashboard.StatusBlockBackColor = Color.FromArgb(Global.Config.AlphaBar - 50, Global.Config.colorGui.R, Global.Config.colorGui.G, Global.Config.colorGui.B);
                G.StatusDashboard.MidHeaderBackColor = Color.FromArgb(Global.Config.AlphaBar, Global.Config.colorGui.R, Global.Config.colorGui.G, Global.Config.colorGui.B);
                  }
           

            pEditTool.Register("Tool", () => Global.ToolSettings);
            pEditTool.Register("Step1", () => G.StepEdit.SettingStep1);
            pEditTool.Register("Step2", () => G.StepEdit.SettingStep2);
            pEditTool.Register("PLC", () => G.SettingPLC);
            pEditTool.Register("Step4", () => G.StepEdit.SettingStep4);
            pEditTool.Register("Images", () => DashboardImages);
            pEditTool.Register("Logs", () => Global.LogsDashboard);
            pInfor.Register("Dashboard", () => G.StatusDashboard);
            pInfor.Register("StepEdit", () => G.StepEdit);

            btnShowTop.Checked = Global.EditTool.pTop.Visible;
            btnShowDashBoard.Checked = Global.EditTool.pInfor.Visible;
            btnMenu.Checked = Global.EditTool.View.pBtn.Visible;
            //   Global.LogsDashboard.AddLog(LeveLLog.INFO, "Ứng dụng khởi động", "Main");

            btnShowToolBar.Checked = btnShowToolBar.Checked;
            if (Global.EditTool.pEdit.Width == 0)
            {
                btnShowToolBar.Checked = false;
                //    Global.EditTool.hideBar.btnShowToolBar.IsCLick = true;
            }
            else
            {
                btnShowToolBar.Checked = true;
            }
            //  Global.ExChanged += Global_ExChanged;
            if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
                BeeCore.Common.listCamera[Global.IndexChoose].FrameChanged += EditTool_FrameChanged;
            Global.StepModeChanged += Global_StepModeChanged;
			//
		}

        private void Global_StepModeChanged(Step obj)
        {
            RefreshGuiEdit(obj);
        }

        private void EditTool_FrameChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Invoke((Action)(() =>
            {
                lbFrameRate.Text = sender.ToString() ;

            }));
           
        }

      

        private void outLine_Load(object sender, EventArgs e)
        {

        }
       
       
    
        int indexTool = 0;
      
        public int targetWidth = 0;
      
        int value = -1;
        int valueOld = -1;
        DateTime dtOld, dt2;

        private void pView_SizeChanged(object sender, EventArgs e)
        {

         
            if (View == null)
                    return;
           
            View.Size = pView.Size;

            View.Dock = DockStyle.Fill;
           

        }

     
        private void toolStripPort_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure", "byPass PLC",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                toolStripPort.Text = "ByPass PLC";
                Global.ParaCommon.Comunication.Protocol.IsBypass = true;
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

      
        private void pInfor_SizeChanged(object sender, EventArgs e)
        {
            BeeCore.CustomGui.RoundRg(pInfor, 20);
        }

        private void resetUI_Click(object sender, EventArgs e)
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BeeInterface");
            if (MessageBox.Show("Are You Sure!", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                    AppRestart.Delayed(2000);
                   // Application.Restart(); Environment.Exit(0);
                }
            }
        }

        private void UnlockSpiltter_Click(object sender, EventArgs e)
        {
            _layout.SplitterLocked = !_layout.SplitterLocked;
			LockSpilter(_layout.SplitterLocked);
			if (!_layout.SplitterLocked)
                UnlockSpiltter.Text = "Lock UI";
            else
                UnlockSpiltter.Text = "UnLock";
        }

        private async void btnFull_Click(object sender, EventArgs e)
        {
            btnFull.Checked=!btnFull.Checked; 
            pTop.Visible = !btnFull.Checked;
           pHeader.Visible = !btnFull.Checked;
            View.pBtn.Visible = !btnFull.Checked;
            btnShowTop.Checked = Global.EditTool.pTop.Visible;
            btnShowDashBoard.Checked = Global.EditTool.pInfor.Visible;
            btnMenu.Checked = Global.EditTool.View.pBtn.Visible;
            await Task.Delay(100);
            ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
        }

        private void btnShowTop_Click(object sender, EventArgs e)
        {
            btnShowTop.Checked = !btnShowTop.Checked;
            Global.EditTool.pTop.Visible= btnShowTop.Checked ;
          
        }

        private void btnShowDashBoard_Click(object sender, EventArgs e)
        {
            btnShowDashBoard.Checked = !btnShowDashBoard.Checked;
            Global.EditTool.pInfor.Visible= btnShowDashBoard.Checked ;
            
        }

        private void btnMenu_Click_1(object sender, EventArgs e)
        {
            btnMenu.Checked = !btnMenu.Checked;
           Global.EditTool.View.pBtn.Visible = btnMenu.Checked ;
        }

        private void btnShowToolBar_Click(object sender, EventArgs e)
        {
            btnShowToolBar.Checked = !btnShowToolBar.Checked;
            if (!btnShowToolBar.Checked)
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

        private void saveToolStrip_Click(object sender, EventArgs e)
        {
            SaveData.Project(Global.Project);
            MessageBox.Show("Save Complete");
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile=new SaveFileDialog();
            saveFile.Title = "Save As Program";
            saveFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Program";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);
                Directory.CreateDirectory("Program\\" + Global.Project);
                Access.SaveProg("Program\\" + Global.Project + "\\" + Global.Project + ".prog", BeeCore.Common.PropetyTools);
                //  Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);

                G.Header.RefreshListPJ();

                if (!G.Header.workLoadProgram.IsBusy)
                    G.Header.workLoadProgram.RunWorkerAsync();
              
            }
        }
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void saveImageTool_Click(object sender, EventArgs e)
        {
            if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw == null) return;
            saveFileDialog.Filter = " PNG|*.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Cv2.ImWrite(saveFileDialog.FileName, BeeCore.Common.listCamera[Global.IndexChoose].matRaw);
            }
        }

        private void openImageTool_Click(object sender, EventArgs e)
        {
            openFileTool.Enabled = false;
            openFile.Multiselect = true;
            openFile.Filter = "Image files (*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFile.ShowDialog() == DialogResult.OK)
            {

                View.Files = new List<string>();
                View.Files = openFile.FileNames.ToList();
                workLoadFile.RunWorkerAsync();
                //BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Cv2.ImRead(View.Files[View.indexFile]);
                //View.listMat = new List<Mat>();
                //View.listMat.Add(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                //BeeCore.Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                //View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                //View.btnFile.Enabled = false;
                //Global.StatusMode = StatusMode.SimOne;
                //View.timer.Restart();
                //Global.StatusProcessing = StatusProcessing.Checking;

                //View.btnRunSim.Enabled = true;
            }
        }
        private Native Native = new Native();
        private void openFolderImage_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                View.indexFile = 0;
              View.  Files = new List<string>();
                View.Files = Directory.GetFiles(folderBrowserDialog1.SelectedPath).ToList(); ;

               

            }
            if (!Global.IsRun)
            {
                View.indexFile = 0;
                View.pathFileSeleted = View.Files[View.indexFile];
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Common.listCamera[Global.IndexChoose].matRaw = View.listMat[View.indexFile]; ;// Cv2.ImRead(Files[indexFile]);
              Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            }
        }

     public   bool IsRunSim = false;
        private void playTool_Click(object sender, EventArgs e)
        {
            if (View.Files == null)
            {
                playTool.Enabled = true; return;
            }
            if (View.Files.Count == 0)
            { playTool.Enabled = true; return; }
            playTool.Enabled = false;
            openFileTool.Enabled = false;
            openImageTool.Enabled = false;
            stopTool.Enabled = true; IsRunSim = true;
            Global.StatusMode = IsRunSim ? StatusMode.SimContinuous : StatusMode.None;
         
         

              
                if (View.indexFile >= View.listMat.Count) View.indexFile = 0;
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = View.listMat[View.indexFile];// Cv2.ImRead(Files[indexFile]);
                View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            View.timer = CycleTimerSplit.Start();
            Global.TriggerNum = TriggerNum.Trigger1;
                Global.StatusProcessing = StatusProcessing.Checking;
     
            if (View.indexFile >= View.Files.Count)
                View.indexFile = 0;
        }

        private void stopTool_Click(object sender, EventArgs e)
        {
            openFileTool.Enabled = true;
            openImageTool.Enabled = true;
          
            IsRunSim = false; 
            Global.StatusMode = IsRunSim ? StatusMode.SimContinuous : StatusMode.None;

            stopTool.Enabled = false;
            playTool.Enabled = true;
        }

        private void openFileTool_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
            
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Cv2.ImRead(openFile.FileName);
              //  View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                View.timer = CycleTimerSplit.Start();
                if (Global.IsRun)
                {
                    Global.TriggerNum = TriggerNum.Trigger1;
                    Global.StatusProcessing = StatusProcessing.Checking;
                }
                else
                {
                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Cv2.ImRead(openFile.FileName);
                    View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                }
            }
            }

        private void dashboardList_Click(object sender, EventArgs e)
        {

        }

        private void debugTool_Click(object sender, EventArgs e)
        {
            debugTool.Checked = !debugTool.Checked;
            Global.IsDebug = debugTool.Checked;
        }

        private void workLoadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            if (View.Files.Count > 0)
            {
                View.listMat = new List<Mat>();
               
                View.indexFile = 0;
               
                foreach (string file in View.Files)
                {
                    View.listMat.Add(new Mat(file));
                }

             
            }
        }

        private void workLoadFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            openFileTool.Enabled = true;
            playTool.Enabled = true;
        }

        private void tmReLoadSplit_Tick(object sender, EventArgs e)
        {
			LockSpilter(true);
			split0.Height = 5;
			split1.Height = 5;
			split2.Height = 5;
			split3.Height = 5;
			split4.Height = 5;
			split5.Height = 5;
			split6.Height = 5;
			View.split2.Height = 5;
			View.split3.Height = 5;
			View.split4.Height = 5;
			View.split5.Height = 5;
			G.Header.split1.Height = 5;
			G.Header.split2.Height = 5;
			Global.ToolSettings.split1.Height = 5;
			
			tmReLoadSplit.Enabled = false;
        }

        private void pRight_SizeChanged(object sender, EventArgs e)
        {
         //   BeeCore.CustomGui.RoundRg(pRight, 20,Corner.Both);
        }

        private void EditTool_SizeChanged(object sender, EventArgs e)
        {

        }

        private void tmLoad_Tick(object sender, EventArgs e)
        {
            tmLoad.Enabled = false;
          
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            // Lấy vị trí ngay dưới nút
            Point menuPoint = new Point(0, btnLogo.Height);

            //if(btnLogo.IsCLick)
            mouseLeft.Show(btnLogo, menuPoint);
            //else
            //    mouseLeft.Hide();
        }
        Camera CameraLive;
        private async void btnLiveCam_Click(object sender, EventArgs e)
        {

            CameraLive = new Camera(new ParaCamera(), 2);
            CameraLive.IndexConnect = (int)numCam.Value;
            CameraLive.Para.TypeCamera = TypeCamera.USB;
            CameraLive.Init(CameraLive.Para.TypeCamera);
            CameraLive.IsConnected=await CameraLive.Connect("", CameraLive.Para.TypeCamera);
            if(!CameraLive.IsConnected)
            {
                btnLive.IsCLick = false;
                MessageBox.Show("Fail live camera");
                return;
            }
            else
            {
                if(btnLive.IsCLick)
                {
                    StartLive();
                    if (!workLive.IsBusy)
                        workLive.RunWorkerAsync();
                }
                else
                {
                    StopLive();
                    CameraLive.DisConnect(TypeCamera.USB);
                   
                }    
              

            }    
        }
        private Thread _displayThread;
        private readonly AutoResetEvent _frameReady = new AutoResetEvent(false);
        private Bitmap _sharedFrame;
        private int _uiPending; // 0: idle, 1: đang đẩy frame lên UI
        void PublishFrame(Bitmap src)
        {
            if (!Global.IsLive) { src.Dispose(); return; }
            // Clone 1 lần ở producer, không clone trong display thread
            var clone = (Bitmap)src.Clone();
            var old = Interlocked.Exchange(ref _sharedFrame, clone); // giữ frame mới nhất, drop cũ
            old?.Dispose();
            _frameReady.Set();
        }

        void StartLive()
        {

            _displayThread = new Thread(DisplayLoop) { IsBackground = true, Name = "DisplayLoop" };
            _displayThread.Start();
        }

        void StopLive()
        {

            _frameReady.Set();
            _displayThread?.Join();
            _displayThread = null;

            // Clear ảnh trên UI
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new Action(() =>
                {
                    var old = imgLive.Image;
                    imgLive.Image = null;
                    old?.Dispose();
                }));

            // Dọn rác còn sót
            var leftover = Interlocked.Exchange(ref _sharedFrame, null);
            leftover?.Dispose();
            if (CameraLive.matRaw!= null)
                if (CameraLive.matRaw != null)
                    if (!CameraLive.matRaw.IsDisposed)
                        if (!CameraLive.matRaw.Empty())
                        {
                            CameraLive.Read();
                            imgLive.Image = CameraLive.matRaw.ToBitmap();
                            
                            ShowTool.Full(imgLive, CameraLive.GetSzCCD());
                        }

        }

        void DisplayLoop()
        {
            while (btnLive.IsCLick)
            {
                _frameReady.WaitOne(50);        // chờ tín hiệu có frame (hoặc timeout để thoát nhanh)
                if (!btnLive.IsCLick) break;

                // Lấy quyền sở hữu frame mới nhất và làm rỗng buffer chung
                var frame = Interlocked.Exchange(ref _sharedFrame, null);
                if (frame == null) continue;

                // Chỉ cho phép 1 cập nhật UI pending; nếu UI chưa kịp xử lý → drop frame
                if (Interlocked.Exchange(ref _uiPending, 1) == 1)
                {
                    frame.Dispose();
                    continue;
                }

                try
                {
                    if (IsHandleCreated && !IsDisposed)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                var old = imgLive.Image;
                                imgLive.Image = frame;   // chuyển quyền sở hữu cho PictureBox
                                old?.Dispose();          // hủy ảnh cũ sau khi gán
                            }
                            finally
                            {
                                Interlocked.Exchange(ref _uiPending, 0);
                            }
                        }));
                    }
                    else
                    {
                        frame.Dispose();
                        Interlocked.Exchange(ref _uiPending, 0);
                    }
                }
                catch
                {
                    frame.Dispose();
                    Interlocked.Exchange(ref _uiPending, 0);
                }
            }
        }
        private void workLive_DoWork(object sender, DoWorkEventArgs e)
        {
            CameraLive.Read();
        }

        private void workLive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           

         

                if (CameraLive.matRaw != null)
                    if (!CameraLive.matRaw.IsDisposed)
                        if (!CameraLive.matRaw.Empty())
                        {
                           // Global.ParaCommon.SizeCCD = CameraLive.GetSzCCD();
                            // matRaw là OpenCvSharp.Mat
                            var bmp = BitmapConverter.ToBitmap(CameraLive.matRaw);

                            // Đẩy frame mới nhất và hủy frame cũ một cách an toàn, không cần lock
                            var old = Interlocked.Exchange(ref _sharedFrame, bmp);
                            old?.Dispose();

                            // (tuỳ chọn) báo cho display thread là có frame mới
                            _frameReady?.Set();
                            //using (Bitmap frame = BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw))
                            //{

                            //        _sharedFrame?.Dispose();
                            //        _sharedFrame = (Bitmap)frame.Clone(); // Clone để thread-safe

                            //}
                        }

              
                workLive.RunWorkerAsync();
                return;
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "New Program";
            BeeCore.Common.PropetyTools[Global.IndexChoose] = new List<BeeCore.PropetyTool>();
            saveFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Program";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);
                Directory.CreateDirectory("Program\\" + Global.Project);
                Access.SaveProg("Program\\" + Global.Project + "\\" + Global.Project + ".prog", BeeCore.Common.PropetyTools);
                G.Header.RefreshListPJ();
                if (!G.Header.workLoadProgram.IsBusy)
                    G.Header.workLoadProgram.RunWorkerAsync();
            }
        }

       
    }
}
