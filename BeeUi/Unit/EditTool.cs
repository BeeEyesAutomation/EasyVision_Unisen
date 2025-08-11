using BeeCore;
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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;

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
            _layout.EnableAuto(); // tự load sau Shown, tự save khi Closing
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
                            MessageBox.Show(ex.Message);
                        }
                       
                        break;
                    case Step.Step1:
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
                        G.StepEdit.btnStep4.IsCLick = true;
                        pName.Visible = true;
                        G.IsCalib = false;

                        pEditTool.Show("Step4");

                        //   iconTool.BackgroundImage = Properties.Resources._4;
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
                Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.Close;
            }

          
            await Task.Delay(100);
        }
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
          
           
          
           
        }

        public View View;
     
     

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

       

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
        MultiDockHost DockHost = new MultiDockHost { Dock = DockStyle.Fill };

        private void EditTool_Load(object sender, EventArgs e)
        {

            //    this.Controls.Add(DockHost);

            //    // Nội dung chính
            //    DockHost.Center = pView;

            //    // Thêm nhiều dock trái/phải/trên/dưới
            //    DockHost.AddDock(DockSide.Top, "Top", pTop, "Top", size: 120, minSize: 140);
            //    DockHost.AddDock(DockSide.Top, "Header", pHeader, "Header", size: 200, minSize: 140);
            //    DockHost.AddDock(DockSide.Right, "Edit", pEdit, "Edit", size: 500);
            //    DockHost.AddDock(DockSide.Bottom, "Bottom", LayoutEnd, "Bottom", size: 40);
            //  //  DockHost.AddDock(DockSide.Bottom, "Log", new LogView(), "Log", size: 180);

            //    // Layout tùy ý theo vùng
            ////    DockHost.LeftLayout = ZoneLayout.Accordion;       // nhiều pane + splitter
            ////    DockHost.RightLayout = ZoneLayout.Accordion;  // 1 pane mở, còn lại co gọn
            //   // DockHost.TopLayout = ZoneLayout.Accordion;
            //   // DockHost.BottomLayout = ZoneLayout.Accordion;

            //    // Thao tác runtime
            //   // DockHost.ToggleCollapsed("Top");      // thu gọn/mở rộng pane "Layers"
            //   // DockHost.SetDockVisible("Top", false);   // ẩn pane "Log"
            //   // DockHost.SetDockSize("Inspector", 360);  // chỉnh size
            //   // DockHost.MoveDock("Layers", 0);          // đổi thứ tự trong Left
            BeeCore.CustomGui.RoundRg(pInfor, 20);
            this.pInfor.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, Global.Config.colorGui);
            pInfor.Height = (int)(pInfor.Height * Global.PerScaleHeight);
           
            Global.EditTool.lbLicence.Text = "Licence: " + G.Licence;
            if (Global.listParaCamera[0] != null)
                CameraBar.btnCamera1.Text = Global.listParaCamera[0].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[1] != null)
                CameraBar.btnCamera2.Text = Global.listParaCamera[1].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[2] != null)
                CameraBar.btnCamera3.Text = Global.listParaCamera[2].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[3] != null)
                CameraBar.btnCamera4.Text = Global.listParaCamera[3].Name.Substring(0, 8) + "..";
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
            pEditTool.Register("Tool", () =>Global.ToolSettings);
            pEditTool.Register("Step1", () => G.StepEdit.SettingStep1);
            pEditTool.Register("Step2", () => G.StepEdit.SettingStep2);
            pEditTool.Register("PLC", () => G.SettingPLC);
            pEditTool.Register("Step4", () => G.StepEdit.SettingStep4);

            pInfor.Register("Dashboard", () => G.StatusDashboard);
            pInfor.Register("StepEdit", () => G.StepEdit);
           
            // if (pHeader.Height > 100) pHeader.Height = 100;
            //   LayoutMain.BackColor= CustomGui.BackColor(TypeCtr.BG,Global.Config.colorGui);

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

         
            if (View == null)
                    return;
           
            View.Size = pView.Size;

            View.Dock = DockStyle.Fill;
           

        }

        private void lbLicence_Click(object sender, EventArgs e)
        {
          
        }

        private void btnShuttdown_Click(object sender, EventArgs e)
        {
           
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

        private void btnHeaderBar_Load(object sender, EventArgs e)
        {

        }

        private void adjustBar4_Load(object sender, EventArgs e)
        {

        }

        private void rjButton1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
                    }

        private void customTablePanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CustomTablePanel_CellClick(object sender, CellEventArgs e)
        {

        }

        private void pInfor_SizeChanged(object sender, EventArgs e)
        {
            BeeCore.CustomGui.RoundRg(pInfor, 20);
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
