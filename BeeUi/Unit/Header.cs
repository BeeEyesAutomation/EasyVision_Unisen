
using BeeCore;
using BeeCore.EtherNetIP;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;

using BeeUi.Tool;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using Timer = System.Windows.Forms.Timer;
using UserControl = System.Windows.Forms.UserControl;

namespace BeeUi.Common
{
    [Serializable()]
    public partial class Header : UserControl
    {
      
        public Header()
        {
            InitializeComponent();
         //  btnHide.Click += async (s, e) => await FadePanel(); ;
            G.Header = this;
           
        }
        bool IsLoaded;
        public void SaveProject()
        {
          
        }
        //int indexTool;
        //public void CreateItemTool(BeeCore.PropetyTool PropetyTool)
        //{
        //    TypeTool TypeTool = PropetyTool.TypeTool;
        //    dynamic control = IniTool(TypeTool);
        //    int with = 50, height = 50;
        //    control.Propety = PropetyTool.Propety;
        //    control.Propety.Index = indexTool;
        ////    BeeCore.RectRotate rotCrop = control.Propety.rotCrop;
        //    if (PropetyTool.TypeTool == TypeTool.Yolo || PropetyTool.TypeTool == TypeTool.OCR || PropetyTool.TypeTool == TypeTool.BarCode || PropetyTool.TypeTool == TypeTool.Color_Area)
        //        control.Propety.rotCrop = null;
        //        System.Drawing.Size szCCd= Global.ParaCommon.SizeCCD;
        //    //    if (rotCrop != null)
        //    //{
        //    //    if (rotCrop._PosCenter.X + rotCrop._rect.X + rotCrop._rect.Width > szCCd.Width ||
        //    //        rotCrop._PosCenter.Y + rotCrop._rect.Y + rotCrop._rect.Height > szCCd.Height)
        //    //        control.Propety.rotCrop = new BeeCore.RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szCCd.Width / 2, szCCd.Height / 2), 0, BeeCore.AnchorPoint.None);

        //    //}
        //    //  BeeCore.RectRotate rotArea = control.Propety.rotArea;
        //    //if (rotArea._PosCenter.X + rotArea._rect.X + rotArea._rect.Width > szCCd.Width ||
        //    //    rotArea._PosCenter.Y + rotArea._rect.Y + rotArea._rect.Height > szCCd.Height)
        //    //    control.Propety.rotArea = new BeeCore.RectRotate(new RectangleF(-szCCd.Width / 2 + szCCd.Width / 10, -szCCd.Height / 2 + szCCd.Width / 10, szCCd.Width - szCCd.Width / 5, szCCd.Height - szCCd.Width / 5), new PointF(szCCd.Width / 2, szCCd.Height / 2), 0, BeeCore.AnchorPoint.None);
        //    Commons.ItemTool item = new Commons.ItemTool(TypeTool, TypeTool.ToString() + Convert.ToString(G.listAlltool[Global.IndexChoose].Count - 1));
        //    item.Location = new Point( Global.pShowTool.X,  Global.pShowTool.Y);
        //    item.lbCycle.Text = "---";
        //    item.lbScore.Text = "---";
        //    item.lbStatus.Text = "---";
        //    item.Score.Value = Convert.ToInt32((double)control.Propety.Score);
        //    item.lbScore.ForeColor = Color.Gray;
        //    item.lbStatus.BackColor = Color.Gray;
        //     Global.pShowTool.Y += item.Height + 10;
        //    G.listAlltool.Add(new Tools(item, control, PropetyTool));

        //    //control.pro.indexTool = G.listAlltool[Global.IndexChoose].Count - 1;
        //    BeeCore.Common.CreateTemp(TypeTool);
        //    if (PropetyTool.Name == null) PropetyTool.Name = "";
        //    if (PropetyTool.Name.Trim() == "")
        //        item.name.Text = TypeTool.ToString() + " " + G.listAlltool[Global.IndexChoose].Count();
        //    else
        //        item.name.Text = PropetyTool.Name.Trim();
        //    control.Name = PropetyTool.Name;
        //    PropetyTool.Propety.nameTool= PropetyTool.Name;
            
           

        //    item.lbNumber.Text = G.listAlltool[Global.IndexChoose].Count() + "";
         
        //    item.icon.Image = (Image)Properties.Resources.ResourceManager.GetObject(TypeTool.ToString());
           
           
        //}
        String pathOld = "";
     
        string[] listPorts;
       bool IsWaitPort = false;
        public String IdPort = "";
        public int indexScan = 0;
        Timer tmScanPort = new Timer();
    
      
        public void ShowErr()
        {
            if(!G.IsReConnectCCD)
            {
                if (Global.ParaCommon.Comunication.IO.IsConnected)
                {
                    if (Global.ParaCommon.Comunication.IO.valueOutput[4] == 0)
                    {

                       // Global.ParaCommon.Comunication.IO.WriteOutPut(4, true);

                    }
                }
                SaveData.Config(Global.Config);
                G.Main.Hide();
                ForrmAlarm ForrmAlarm = new ForrmAlarm();
                ForrmAlarm.lbHeader.Text = "Camera Disconnect !!";
                ForrmAlarm.lbContent.Text = "Turn off the Camera and reconnect it.";
                ForrmAlarm.ShowDialog();
                G.Main.Close();
                //G.IsReConnectCCD = true;
                //Global.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
                //G.ScanCCD.Show();
                //G.Header.tmReadPLC.Enabled = false;


            }

            return;
        }
   

        private async void TmScanPort_Tick(object sender, EventArgs e)
        {
         
        }

        public void Acccess(bool IsRun)
        {
            //if(IsRun)
            //{
            //    Global.EditTool.pName.Visible = false;
               
            //}
            //else
            //{
            //    Global.EditTool.pName.Visible = true;
             
            //}
           // Global.EditTool.btnHeaderBar1.btnSettingPLC.Enabled= Global.IsRun;
            Global.EditTool.View.btnLive.Enabled = !Global.IsRun;
            pModel.Enabled = IsRun;
          
            if (Global.Config.nameUser == "Admin")
            {
                G.SettingPLC.pCom.Enabled = true;
                if( G.StatusDashboard!=null)
                 G.StatusDashboard.btnReset.Enabled = true;
                Global.EditTool.View.btnRecord.Enabled = Global.IsRun;
                

                //  G.listProgram.Enabled = IsRun;
                btnMode.Enabled = true;
             

               

              
               // btnIO.Enabled = true;
            }
            else if (Global.Config.nameUser == "Leader")
            {
                G.SettingPLC.pCom.Enabled = false;
                // Global.EditTool.View.btnRecord.Enabled = false;

                Global.EditTool.View.btnRecord.Enabled = false;

                //G.listProgram.Enabled = IsRun;
                if ( G.StatusDashboard != null)
                     G.StatusDashboard.btnReset.Enabled = true;
                btnMode.Enabled = false;
           
             //   btnIO.Enabled = false;
            }
            else 
            {
                Global.EditTool.View.btnRecord.Enabled = false;

                // Global.EditTool.View.btnRecord.Enabled = false;
                G.SettingPLC.pCom.Enabled = false;

                G.listProgram.Enabled = IsRun;
              //  btnReport.Enabled = false;
                btnMode.Enabled = false;
                if ( G.StatusDashboard != null)
                     G.StatusDashboard.btnReset.Enabled = false;
              
              //  btnIO.Enabled = false;
                
            }
            if ( Global.ParaCommon.IsExternal)
            {
               Global.EditTool.View.btnCap.Enabled = false;
               Global.EditTool.View.btnRecord.Enabled = false;
            }
         
            Global.EditTool.btnHeaderBar1.btnUser.Text =Global.Config.nameUser;
        }
 
        public async void Mode()
        {
            if (Global.Config.nameUser != "Admin")
                return;
            if (Global.StatusMode==StatusMode.Once)
            {
                Global.StatusMode = StatusMode.None;
                Global.StatusProcessing = StatusProcessing.None;
                
               // MessageBox.Show("Please Stop Mode Continuous");
              //  return;
            }
            foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
            {
                PropetyTool.ItemTool.IsCLick = false;
            }
            Global.IsRun = !Global.IsRun;
       
            if (Global.IsRun)
            {
              
                Global.EditTool.View.btnCap.Enabled = true;
                Global.EditTool.View.btnRecord.Enabled = true;

               
                Global.ToolSettings.btnAdd.Enabled = false;
                Global.ToolSettings.btnCopy.Enabled = false;
                Global.ToolSettings.btnDelect.Enabled = false;
                Global.ToolSettings.btnEnEdit.Enabled = true;
                btnMode.Text = "RUN";
                btnMode.ForeColor = Color.FromArgb(101, 173, 245); ;// Color.DarkSlateGray;
                Global.EditTool.RefreshGuiEdit(Step.Run);

            }
            else
            {
                if (Global.EditTool.View.btnRecord.IsCLick)
                    if (Global.EditTool.View.btnRecord.Enabled == true)
                        Global.EditTool.View.btnRecord.PerformClick();
                Global.EditTool.btnHeaderBar1.btnSettingPLC.IsCLick = false;
                Global.EditTool.View.btnCap.Enabled = false;
                Global.EditTool.View.btnRecord.Enabled = false;
                Global.EditTool.RefreshGuiEdit(Step.Step1);
                Global.ToolSettings.btnAdd.Enabled = true;
                Global.ToolSettings.btnCopy.Enabled = true;
                Global.ToolSettings.btnDelect.Enabled = true;
                Global.ToolSettings.btnEnEdit.Enabled = false;
                btnMode.Text = "EDIT";
                btnMode.ForeColor = Color.DarkSlateGray;
              

            }
            Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.ChangeMode;
            
           Acccess(Global.IsRun);
           
        }
        private void btnMode_Click(object sender, EventArgs e)
        {
            if (Global.EditTool.View.btnLive.IsCLick)
            {
                Global.EditTool.View.btnLive.PerformClick();
            }
         //   G.Header.tmReadPLC.Enabled = true;
            Mode();
        }
        String[] PathFile;
        bool IsLoad = false;
        public void IniProject()
        {
            string[] files =  Directory.GetDirectories("Program");
          
            PathFile = files.Select(a => Path.GetFileNameWithoutExtension(a)).ToArray();
            items = PathFile.ToList();
            IsLoad = true;
            G.listProgram.DataSource = PathFile;
        }
        private void FadeEffect(object sender, EventArgs e)
        {
            if (Global.EditTool.pEdit.Visible && opacity < 1.0f)
            {
                opacity += 0.1f;
                Global.EditTool.pEdit.BackColor = Color.FromArgb((int)(opacity * 255), Global.EditTool.pEdit.BackColor);
            }
            else if (!Global.EditTool.pEdit.Visible && opacity > 0.0f)
            {
                opacity -= 0.1f;
                Global.EditTool.pEdit.BackColor = Color.FromArgb((int)(opacity * 255), Global.EditTool.pEdit.BackColor);
            }
            else
            {
                fadeTimer.Stop();
                if (opacity == 0.0f) Global.EditTool.pEdit.Visible = false;
            }
        }
        private void Header_Load(object sender, EventArgs e)
        {
           
            //    this.Controls.Add(btnHide);

            //fadeTimer = new Timer { Interval = 20 };
            //fadeTimer.Tick += FadeEffect;
            if (Global.EditTool == null) return;
            if (Global.EditTool.View == null)
            {

                Global.EditTool.View = new View();

                Global.EditTool.View.Dock = DockStyle.None;
                Global.EditTool.View.Size = Global.EditTool.pView.Size;
                Global.EditTool.View.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom;
                Global.EditTool.View.Location = new Point(0,0);

                Global.EditTool.View.Parent = Global.EditTool.pView;
            }
            //if (G.IsLoad) return;
          //  pMenu.Region = BeeCore.CustomGui.RoundRg(pMenu,Global.Config.RoundRad);
           BeeCore.CustomGui.RoundRg(pModel,Global.Config.RoundRad);
        
           BeeCore.CustomGui.RoundRg(pPO,Global.Config.RoundRad);

            
         //   pMenu.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
            pPO.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
            pModel.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
            G.listProgram.Font = new Font("Arial", 16);
            G.listProgram.Parent = G.Main;
            G.listProgram.BringToFront();
            G.listProgram.Visible = false;
            G.listProgram.Location = new Point(this.Location.X +btnMode.Width+txtQrCode.Location.X, this.Location.Y+ txtQrCode.Location.Y+txtQrCode.Height+10);
            G.listProgram.Width = txtQrCode.Width;
            G.listProgram.SelectedIndexChanged += ListProgram_SelectedIndexChanged;
            this.myDelegate = new AddDataDelegate(AddDataMethod);
             //cbSerialPort.SelectedIndex = cbSerialPort.FindStringExact(Global.Config.namePort);
            if (!Directory.Exists("Program"))
            {
                Directory.CreateDirectory("Program");
            }    
            else
            {
                // Access.SaveProg("Program\\Default.prog", new List<PropetyTool>());
                IniProject();

                Global.Project= Properties.Settings.Default.programCurrent;

                
                    txtQrCode.Text = Global.Project;
                txtQrCode.Enabled = false;
                btnShowList.Enabled = false;
                if (!workLoadProgram.IsBusy)
                    workLoadProgram.RunWorkerAsync();


            }
            ThreadPool.SetMinThreads(Environment.ProcessorCount, Environment.ProcessorCount);
            //Acccess(Global.IsRun);
            G.Main.Location = new Point(0,0);

        }
        void ChangeProgram(String program)
        {

            if (IsLoad)
            {
                IsLoad = false;
                return;
                //    G.listProgram.SelectedIndex = G.listProgram.FindStringExact(Properties.Settings.Default.programCurrent);

            }
            txtQrCode.Enabled = false;
            btnShowList.Enabled = false;
            if (!workLoadProgram.IsBusy)
                workLoadProgram.RunWorkerAsync();

            Global.Project= program;
            txtQrCode.Text = Global.Project;
            if (btnEnQrCode.IsCLick)
            {
                G.Main.ActiveControl = txtQrCode;
                txtQrCode.Focus();
            }
            //if (!IsSaveAs)
            //{
            //G.Main.ActiveControl = txtQrCode;
            //txtQrCode.Focus();
            //txtQrCode.SelectAll();
            //G.listProgram.Visible = false;
                

            //}
            //else
            //    IsSaveAs = false;

        }
        
        private void ListProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsIntialProgram) return;
            if(IsKeyPress)
            {
                IsKeyPress = false;
                return;
            }
            if (G.listProgram.SelectedValue == null) return;
          
            ChangeProgram(G.listProgram.SelectedValue.ToString());
        }

        private void cbProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
           
      
        }
      

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        String sRecept = "";
        public delegate void AddDataDelegate(String myString);
        public AddDataDelegate myDelegate;
      
    public void AddDataMethod(String myString)
    {
        //Global.EditTool.txtRecept.Text=myString;
            
        //    if (sRecept.Contains("IO")&& !IsWaitPort)
        //    {
        //        IsWaitPort = true;
        //        tmScanPort.Tick -= TmScanPort_Tick;
        //        tmScanPort.Enabled = false;
             
        //        Global.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
        //        Global.EditTool.toolStripPort.Text = "Port Connected";
        //        SaveData.Config(Global.Config);
        //        Global.EditTool.View.tmCheckPort.Enabled = true;
        //    }
        //   else if  (sRecept.Contains("Trig"))
        //        {
        //        Global.EditTool.View.Cap(false);
        //        }
        //    else if (sRecept.Contains("Mod"))
        //    {
        //        //SerialPort.WriteLine("Done");
        //        btnMode.PerformClick();
        //        if (btnMode.Text=="RUN")
        //            SerialPort1.WriteLine("Run");
        //        else
        //            SerialPort1.WriteLine("Edit");
        //    }
        //    else if (sRecept.Contains("Rec"))
        //    {
        //      Global.EditTool.View.btnLive.PerformClick();
        //        if (Global.EditTool.View.btnLive.IsCLick)
        //            SerialPort1.WriteLine("Live");
        //        else
        //            SerialPort1.WriteLine("Cap");
        //    }
        //    else if (sRecept.Contains("Done"))
        //    {
        //       // G.IsDone = false;
        //    }
        }
    private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            sRecept =SerialPort1.ReadExisting();
            //textBox1.Invoke(this.myDelegate, new Object[] { sRecept });

          
        }

        private void btnIO_Click(object sender, EventArgs e)
        {
          
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
           
           
        }
       
        private void workConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void workConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            
        }

        private void tmQrCode_Tick(object sender, EventArgs e)
        {
            if (!Global.IsRun) return;
            G.Main.ActiveControl = txtQrCode;
            txtQrCode.Focus();
            txtQrCode.SelectAll();
        }
        String sQrcode = "";
        bool IsInsert2, IsEnter, IsChangetext;
        List<String> items=new List<String>();
        bool IsKeyPress = false;
        List<String> listFilter = new List<string>();
        private void txtQrCode_TextChanged(object sender, EventArgs e)
        {
            if(!Global.IsLoadProgFist) return;
           G.listProgram.Visible = true;
            if (IsKeyEnter) return;
                // Lấy chuỗi tìm kiếm từ TextBox
                string filter = txtQrCode.Text.ToLower();

            // Lọc danh sách dựa trên chuỗi tìm kiếm
            listFilter = items.Where(item => item.ToLower().Contains(filter)).ToList();
                IsKeyPress = true;
                // Cập nhật ComboBox với các mục đã lọc
                G.listProgram.DataSource = new BindingSource(listFilter, null);
            

        }

        private void pProgram_SizeChanged(object sender, EventArgs e)
        {
          
        }

        private void Header_SizeChanged(object sender, EventArgs e)
        {
            G.listProgram.Location = new Point(this.Location.X +txtQrCode.Parent.Location.X + txtQrCode.Location.X, this.Location.Y + txtQrCode.Location.Y + txtQrCode.Height + 10);
            G.listProgram.Width = txtQrCode.Width;
        }

        private void txtQrCode_KeyPress(object sender, KeyPressEventArgs e)
        {
           // G.listProgram.SelectedIndex=G.listProgram.FindStringExact()
        }

        private void txtQrCode_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {

            }
        }

        private void btnEnQrCode_Click(object sender, EventArgs e)
        {
            tmQrCode.Enabled = btnEnQrCode.IsCLick;
            G.Main.ActiveControl = txtQrCode;
txtQrCode.Focus();
        }

        private void btnShowList_Click(object sender, EventArgs e)
        {
            G.listProgram.Location = new Point(pModel.Location.X+ txtQrCode.Location.X, this.Location.Y + pModel.Location.Y  + txtQrCode.Location.Y + txtQrCode.Height + 10);
            G.listProgram.Width = txtQrCode.Width;
            G.listProgram.Visible=!G.listProgram.Visible;
            if(G.listProgram.Visible )
            {
                IsLoad = true;
                G.listProgram.DataSource = PathFile;

            }

            G.Main.ActiveControl = txtQrCode;
            txtQrCode.Focus();
            txtQrCode.SelectAll();
        }
        bool IsIntialProgram = false;
        private void workLoadProgram_DoWork(object sender, DoWorkEventArgs e)
        {
            if(IsIntialProgram)
            DataTool.LoadProject(Global.Project);
        }

        private void workLoadProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            if (Global.ParaCommon.matRegister != null)
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = OpenCvSharp.Extensions.BitmapConverter.ToMat(Global.ParaCommon.matRegister);
            else if (G.IsCCD)
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = null;// BeeCore.Common.GetImageRaw();
            if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
            {
                Global.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                //BeeCore.Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw);
            }
            if (Global.ToolSettings == null)
            {
                Global.ToolSettings = new ToolSettings();

            }
          
            Properties.Settings.Default.programCurrent = Global.Project;
            Properties.Settings.Default.Save();
            G.listProgram.Visible = false;
            txtQrCode.Enabled = true;
            btnShowList.Enabled = true;
            txtQrCode.Text = Global.Project;

            if (Global.ParaCommon.Comunication == null)
			{
				Global.ParaCommon.Comunication = new Comunication();
				Global.ParaCommon.Comunication.IO = new IO();
				Global.ParaCommon.Comunication.IO.paraIOs = new List<ParaIO>();

			}
           
            IsIntialProgram = true;
            Acccess(Global.IsRun);
            G.listProgram.Visible = false;
            tmIninitial.Enabled = true;
            Global.ToolSettings.pAllTool.Controls.Clear();
           
            tmShow.Enabled = true;
            if(Global.listParaCamera[0]!=null)
                CameraBar.btnCamera1.Text =Global.listParaCamera[0].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[1] != null)
                CameraBar.btnCamera2.Text =Global.listParaCamera[1].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[2] != null)
                CameraBar.btnCamera3.Text =Global.listParaCamera[2].Name.Substring(0, 8) + "..";
            if (Global.listParaCamera[3] != null)
                CameraBar.btnCamera4.Text =Global.listParaCamera[3].Name.Substring(0, 8) + "..";
    
        }

        private void workSaveProject_DoWork(object sender, DoWorkEventArgs e)
        {
       
        }

        private void workSaveProject_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveData.Project(Global.Project);
            G.EditProg.btnSave.Enabled = true;
        }

        private void panel3_SizeChanged(object sender, EventArgs e)
        {
          //  txtQrCode.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
          //  btnShowList.Anchor= AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
        }
        private Timer fadeTimer;
        private float opacity = 0.0f; // Bắt đầu từ 0
      
        private void AnimateColumn(TableLayoutPanel tableLayoutPanel, int columnIndex, bool show)
        {
            Timer timer = new Timer();
            timer.Interval = 10; // Thời gian refresh animation
            float step = show ? 2F : -2F; // Tăng hoặc giảm kích thước

            timer.Tick += (s, e) =>
            {
                float currentWidth = tableLayoutPanel.ColumnStyles[columnIndex].Width;
                if ((show && currentWidth >= 400) || (!show && currentWidth <= 0))
                {
                    timer.Stop();
                    timer.Dispose();
                }
                else
                {
                    tableLayoutPanel.ColumnStyles[columnIndex].Width = currentWidth + step;
                    tableLayoutPanel.Refresh();
                }
            };

            timer.Start();
        }

      

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pModel_SizeChanged(object sender, EventArgs e)
        {
            if (Global.EditTool == null) return;
           BeeCore.CustomGui.RoundRg(pModel,Global.Config.RoundRad);

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }
        
      

        int percent = 0;

        public int indexToolShow = 0;
      public  int stepShow = 0;
        private void tmShow_Tick(object sender, EventArgs e)
        {
            
            switch (stepShow)
            {
                case 0:
                Global.EditTool.View.imgView.Visible = true;
                Global.EditTool.View.imgView.Size = Global.EditTool.View.pView.Size;
                    stepShow++;
                    tmShow.Interval = 500;
                    break;
                case 1:
                    Global.EditTool.RefreshGuiEdit(Step.Run);
                    stepShow++;
                    break;
                case 2:
                    if (indexToolShow < BeeCore.Common.PropetyTools[Global.IndexChoose].Count)
                    {
                        tmShow.Interval = 50;
                       
                        Global.ToolSettings.pAllTool.Controls.Add(BeeCore.Common.PropetyTools[Global.IndexChoose][indexToolShow].ItemTool);
                        indexToolShow++;
                        Global.ToolSettings.ResumeLayout(true);
                    }
                    else
                    {
                        stepShow = 0;
                        indexToolShow = 0;
                        Global.EditTool.View.btnFull.PerformClick();
                        tmShow.Enabled = false;
                        Global.ToolSettings.ResumeLayout(true);
                    }
                    break;
            } 
           
                
               


         
               
            


        }
      
        private void workPLC_DoWork(object sender, DoWorkEventArgs e)
        {
          

           // Global.ParaCommon.Comunication.IO.Read();
        }
    public    bool CheckLan()
        {
            string tenMangCanCheck = "LAN"; // Đổi thành tên card mạng bạn muốn kiểm tra

            var cardMang = NetworkInterface
                .GetAllNetworkInterfaces();

            foreach (var ni in cardMang)
            {
                if (ni.Name.Equals(tenMangCanCheck, StringComparison.OrdinalIgnoreCase))
                {

                    if (ni.OperationalStatus == OperationalStatus.Up)
                        return true;
                    else
                        return false;

                    break;
                }
            }
            return false;
        }
      
        private void editProg1_Load(object sender, EventArgs e)
        {

        }

        private void workReConnect_DoWork(object sender, DoWorkEventArgs e)
        {
          
           
        }

        int numAlive = 0;
        private void tmOutAlive_Tick(object sender, EventArgs e)
        {
           
          //  tmOutAlive.Enabled = false;
            if (!Global.IsRun) return;
            if (Global.ParaCommon.Comunication.IO.valueInput[3] == 1 )
            {
               
                numAlive++;
            }
              
            else
            {
              
                  
                numAlive = 0;

            }
            if (numAlive>30)
            {
                if (!G.IsPLCNotAlive)
                {
                    ForrmAlarm forrmAlarm = new ForrmAlarm();
                    forrmAlarm.lbHeader.Text = "PLC not Alive !!";
                    forrmAlarm.lbContent.Text = "Checking Mode RUN of PLC";
                    forrmAlarm.Show();
                   
                    G.IsPLCNotAlive = true;
                }
                
            }
        }

        private void tmIninitial_Tick(object sender, EventArgs e)
        {
            tmIninitial.Enabled= false;
            Global.Initialed = true;
        }

        bool IsKeyEnter = false;
        private void txtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.KeyCode == Keys.Enter)
            {
                IsKeyEnter = true;
                IsChangetext = false;
                if ( IsInsert2 == false)
                {
                   
                    sQrcode = txtQrCode.Text.Replace("\n", "");
                   
                   
                    IsKeyPress = false;
                   
                    //    G.listProgram.SelectedIndex = 0;// G.listProgram.FindStringExact(sQrcode+".prog");
                    if (listFilter.Count > 0)
                    {
                      
                        Global.Project = listFilter[0].ToString().Replace(".prog", "");
                        ChangeProgram(Global.Project); 
                        IsKeyEnter = false;
                       
                        G.listProgram.Visible = false;
                        
                    }
                    else
                    {
                        IsKeyEnter = false;
                    }    
                }
              
            }

        }
    }
}
