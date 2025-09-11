 using BeeCore;
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
using System.Threading.Tasks;
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
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
           
            this.AutoScaleMode = AutoScaleMode.Dpi; // hoặc AutoScaleMode.Font
            _layout = new LayoutPersistence(this, key: "MainLayout");
            _layout.LoadDelayMs = 300;        // trễ 500ms sau Form.Shown
            _layout.SplitterLocked = true;   // tuỳ chọn
            _layout.EnableAuto();
          ///  _layout.EnableAuto(); // tự load sau Shown, tự save khi Closing
                                  // BeeCore.CustomGui.RoundControl(picLogo,Global.Config.RoundRad);

        }
        void ShowView(Control host, Control next)
        {
            host.SuspendLayout();
            foreach (Control c in host.Controls) c.Visible = false; // không remove
            if (!host.Controls.Contains(next))
            {
                next.Dock = DockStyle.Fill;
                next.Visible = false;
                host.Controls.Add(next);
            }
            next.Visible = true;
            next.BringToFront();
            host.ResumeLayout(true);
            host.PerformLayout();
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
                Global.IndexToolSelected = -1;
                if(Global.EditTool.View.btnLive.IsCLick)
                {
                    Global.EditTool.View. btnLive.PerformClick();

                }
                CameraBar.btnFlowChart.IsCLick = true;
                Global.StatusDraw = StatusDraw.None;
            X: switch (Step)
                {
                   
                    case Step.Run:
                        Global.IsAllowReadPLC = true;
                        Shows.ShowAllChart(Global.ToolSettings.pAllTool as Control);
                        Global.EditTool.View.btnLive.Enabled = false;
                        CameraBar.Visible = true;
                        pHeader.Visible = true;
                        Global.IsRun = true;
                        G.Header.btnMode.Text = "Run";
                        G.Header.btnMode.IsCLick = false;
                        pName.Visible = false;
                        G.SettingPLC.Visible = false;
                        pInfor.Show("Dashboard");
                        pEditTool.Show("Tool");
                        //pEditTool.Controls.Clear();
                        
                        ////pEditTool.Visible = true;
                        //Global.ToolSettings.Dock = DockStyle.Fill;
                        //pEditTool.Controls.Add(Global.ToolSettings);
                       
                      //  Global.ToolSettings.Size = pEditTool.Size;

                      //  Global.ToolSettings.Visible = true;
                      //  G.StepEdit.SettingStep1.Visible = false;
                      //  Global.ToolSettings.BringToFront();
                        
                       // Global.EditTool.View.pHeader.Controls.Clear();
                     
                        //G.StepEdit.Visible = false;
                        //G.StatusDashboard.Visible = true;
                        // G.StatusDashboard.Parent = Global.EditTool.View.pHeader;
                        //G.StatusDashboard.Size = Global.EditTool.View.pHeader.Size;
                        //G.StatusDashboard.BringToFront();
                     
                        try
                        {
                           
                                if (Global.ParaCommon.matRegister != null)
                                if (Global.ParaCommon.matRegister.Width != 0)
                                {
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = new Mat();
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Global.ParaCommon.matRegister.ToMat().Clone();
                                    G.IsCalib = false;
                                    Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                                    Global.EditTool.View.imgView.Invalidate();
                                    Global.EditTool.View.imgView.Update();
                                    Shows.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                                   Global.Config.imgZoom = View.imgView.Zoom;
                                   Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                   Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
                                }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                      G.Header.Acccess(Global.IsRun);
                      View.RefreshExternal(Global.ParaCommon.IsExternal);
                        break;
                    case Step.Step1:
                        Global.IsAllowReadPLC = false;
                        Global.EditTool.View.btnLive.Enabled = true;
                        pHeader.Visible = false;
                        CameraBar.Visible = false;
                        Global.IsRun = false;
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
                                if (Global.ParaCommon.matRegister.Width != 0)
                                {
                                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Global.ParaCommon.matRegister.ToMat().Clone();
                                    G.IsCalib = false;
                                    Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                                    Global.EditTool.View.imgView.Invalidate();
                                    Global.EditTool.View.imgView.Update();
                                    Shows.Full(View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                                   Global.Config.imgZoom = View.imgView.Zoom;
                                   Global.Config.imgOffSetX = View.imgView.AutoScrollPosition.X;
                                   Global.Config.imgOffSetY = View.imgView.AutoScrollPosition.Y;
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
                        //Global.ToolSettings.Parent = pEditTool;
                        //Global.ToolSettings.Size = pEditTool.Size;
                        //Global.ToolSettings.Location = new Point(0, 0);
                        //Global.ToolSettings.pAllTool.Visible = true;
                        //Global.ToolSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;


                        //Global.ToolSettings.BringToFront();
                        if (Global.ParaCommon.matRegister != null)
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
                if (!Global.IsRun)
                {
                    Global.ToolSettings.btnAdd.Enabled = true;
                    Global.ToolSettings.btnCopy.Enabled = true;
                    Global.ToolSettings.btnDelect.Enabled = true;
                    Global.ToolSettings.btnEnEdit.Enabled = false;
                }
                else
                {
                    Global.ToolSettings.btnAdd.Enabled = false;
                    Global.ToolSettings.btnCopy.Enabled = false;
                    Global.ToolSettings.btnDelect.Enabled = false;
                    Global.ToolSettings.btnEnEdit.Enabled = true;
                    Global.EditTool.View.btnTypeTrig.Enabled = Global.IsRun;
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
            Global.LogsDashboard.Dispose();
           foreach (Camera camera in BeeCore.Common.listCamera)
                if(camera!=null)
				camera.DestroyAll();

            View.tmContinuous.Enabled = false;
           
            if (Global.ParaCommon.Comunication.IO.IsConnected)
            {
                Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.Close;
            }

          
            await Task.Delay(100);
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
          
           
          
           
        }

        public View View;
     
     

      
        MultiDockHost DockHost = new MultiDockHost { Dock = DockStyle.Fill };
        DashboardImages DashboardImages;
      
        private void EditTool_Load(object sender, EventArgs e)
        {
            //dashboardList.Items.Add(new LabelItem { Name = "Zone A", IsArea = true, IsWidth = false, IsHeight = true, ValueArea = 10, ValueWidth = 20, ValueHeight = 30 });
            //dashboardList.Items.Add(new LabelItem { Name = "Zone B", IsArea = false, IsWidth = true, IsHeight = false, ValueArea = 5, ValueWidth = 15, ValueHeight = 25 });
            //for (int i = 0; i < 25; i++)
            //    dashboardList.Items.Add(new LabelItem
            //    {
            //        Name = $"Zone {i + 1}",
            //        IsArea = (i % 2 == 0),
            //        IsWidth = (i % 3 == 0),
            //        IsHeight = (i % 4 == 0),
            //        ValueArea = i,
            //        ValueWidth = i * 2,
            //        ValueHeight = i * 3
            //    });

            BeeCore.CustomGui.RoundRg(pInfor, 20);
            this.pInfor.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, Global.Config.colorGui);
            pInfor.Height = (int)(pInfor.Height * Global.PerScaleHeight);
           


            Global.EditTool.lbLicence.Text = "Licence: " + G.Licence;
          
            pHeader.Height =(int)( pHeader.Height * Global.PerScaleHeight);
            pTop.Height = (int)(pTop.Height * Global.PerScaleHeight);
            pEdit.Width= (int)(pEdit.Width * Global.PerScaleWidth);
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
                //  G.StatusDashboard.Dock = DockStyle.None;
                //  G.StatusDashboard.Location = new Point(0, 0); 
                //  G.StatusDashboard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            if(DashboardImages==null)
            {
                DashboardImages = new DashboardImages();
              //  DashboardImages.
            }
            if(Global.LogsDashboard==null)
            Global.LogsDashboard = new LogsDashboard();
            // cấu hình
            Global.LogsDashboard.MaxLogCount = 5000;
            Global.LogsDashboard.ProgressiveBatchSize = 200;
            Global.LogsDashboard.ProgressiveIntervalMs = 10;
            Global.LogsDashboard.IngestBatchSize = 100;
            Global.LogsDashboard.IngestIntervalMs = 16; // ~60Hz
            pEditTool.Register("Tool", () =>Global.ToolSettings);
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
            Global.LogsDashboard.AddLog(LeveLLog.INFO, "Ứng dụng khởi động", "Main");
           
            btnShowToolBar.Checked = btnShowToolBar.Checked;
            if (Global.EditTool.pEdit.Width ==0)
            {
                btnShowToolBar.Checked = false;
                //    Global.EditTool.hideBar.btnShowToolBar.IsCLick = true;
            }
            else
            {
                btnShowToolBar.Checked = true;
            }
          //  Global.ExChanged += Global_ExChanged;
          if (BeeCore.Common.listCamera[Global.IndexChoose]!=null)
            BeeCore.Common.listCamera[Global.IndexChoose].FrameChanged += EditTool_FrameChanged;
            // if (pHeader.Height > 100) pHeader.Height = 100;
            //   LayoutMain.BackColor= CustomGui.BackColor(TypeCtr.BG,Global.Config.colorGui);

        }

        private void EditTool_FrameChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Invoke((Action)(() =>
            {
                lbFrameRate.Text = sender.ToString() ;

            }));
           
        }

        private void Global_ExChanged(string obj)
        {
            this.Invoke((Action)(() =>
            {
                lbEx.Text = obj;
               
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
            Shows.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
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
            stopTool.Enabled = true; View.btnRunSim.IsCLick = true;
            Global.StatusMode = View.btnRunSim.IsCLick ? StatusMode.SimContinuous : StatusMode.None;
         
         

                View.btnRunSim.Image = Properties.Resources.Stop;

                View.btnFolder.Enabled = false;
                if (View.indexFile >= View.listMat.Count) View.indexFile = 0;
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = View.listMat[View.indexFile];// Cv2.ImRead(Files[indexFile]);
                View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                Global.StatusProcessing = StatusProcessing.Checking;
     
            if (View.indexFile >= View.Files.Count)
                View.indexFile = 0;
            Global.EditTool.lbEx.Text = View.indexFile + "." + Path.GetFileNameWithoutExtension(View.Files[View.indexFile]);
        }

        private void stopTool_Click(object sender, EventArgs e)
        {
            openFileTool.Enabled = true;
            openImageTool.Enabled = true;
            View.btnRunSim.Image = Properties.Resources.Play_2;
            View.btnFolder.Enabled = true; 
            View.btnRunSim.IsCLick = false; 
            Global.StatusMode = View.btnRunSim.IsCLick ? StatusMode.SimContinuous : StatusMode.None;

            stopTool.Enabled = false;
            playTool.Enabled = true;
        }

        private void openFileTool_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Cv2.ImRead(openFile.FileName);
                View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
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
            View.btnRunSim.Enabled = true; View.btnPlayStep.Enabled = true; playTool.Enabled = true;
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
