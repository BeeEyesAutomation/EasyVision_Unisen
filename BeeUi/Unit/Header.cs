﻿
using BeeCore;
using BeeCore.EtherNetIP;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using BeeUi.Commons;
using BeeUi.Data;
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
        //    item.Location = new Point(G.ToolSettings.X, G.ToolSettings.Y);
        //    item.lbCycle.Text = "---";
        //    item.lbScore.Text = "---";
        //    item.lbStatus.Text = "---";
        //    item.Score.Value = Convert.ToInt32((double)control.Propety.Score);
        //    item.lbScore.ForeColor = Color.Gray;
        //    item.lbStatus.BackColor = Color.Gray;
        //    G.ToolSettings.Y += item.Height + 10;
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
                if (Global.Comunication.IO.IsConnected)
                {
                    if (Global.Comunication.IO.valueOutput[4] == 0)
                    {

                       // Global.Comunication.IO.WriteOutPut(4, true);

                    }
                }
                SaveData.Config(G.Config);
                G.Main.Hide();
                ForrmAlarm ForrmAlarm = new ForrmAlarm();
                ForrmAlarm.lbHeader.Text = "Camera Disconnect !!";
                ForrmAlarm.lbContent.Text = "Turn off the Camera and reconnect it.";
                ForrmAlarm.ShowDialog();
                G.Main.Close();
                //G.IsReConnectCCD = true;
                //G.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
                //G.ScanCCD.Show();
                //G.Header.tmReadPLC.Enabled = false;


            }

            return;
        }
        public async void ConnectCom()
        {
            
       
          
            //    try
            //{
            //    if (G.Config.IDPort != null)
            //    {
            //        if (G.Config.IDPort.Trim() != "")
            //        {
            //            Global.Comunication.IO.Connect(G.Config.IDPort);
            //           // Modbus.ConnectPLC(G.Config.namePort.Trim());

            //        }
                   
            //    }
            //    //Thread.Sleep(200);
            //    //switch (Global.ParaCommon.TypeLight)
            //    //{
            //    //    case 1:
            //    //       SerialPort1.WriteLine("Botl");
            //    //        break;
            //    //    case 2:
            //    //        SerialPort1.WriteLine("Topl");
            //    //        break;
            //    //    case 3:
            //    //        SerialPort1.WriteLine("Both");
            //    //        break;
            //    //}
            //}
            //catch(Exception ex)
            //{

            //}
        }

        private async void TmScanPort_Tick(object sender, EventArgs e)
        {
         
        }

        public void Acccess(bool IsRun)
        {
            if(IsRun)
            {
                G.EditTool.pName.Visible = false;
               
            }
            else
            {
                G.EditTool.pName.Visible = true;
             
            }
            G.EditTool.btnHeaderBar1.btnSettingPLC.Enabled= Global.IsRun;
            G.EditTool.View.btnLive.Enabled = !Global.IsRun;
            pModel.Enabled = IsRun;
          
            if (G.Config.nameUser == "Admin")
            {
                G.SettingPLC.pCom.Enabled = true;
                if(G.ResultBar!=null)
                G.ResultBar.btnResetQty.Enabled = true;
                G.EditTool.View.btnRecord.Enabled = Global.IsRun;
                

                //  G.listProgram.Enabled = IsRun;
                btnMode.Enabled = true;
             

               

              
               // btnIO.Enabled = true;
            }
            else if (G.Config.nameUser == "Leader")
            {
                G.SettingPLC.pCom.Enabled = false;
                // G.EditTool.View.btnRecord.Enabled = false;

                G.EditTool.View.btnRecord.Enabled = false;

                //G.listProgram.Enabled = IsRun;
                if (G.ResultBar != null)
                    G.ResultBar.btnResetQty.Enabled = true;
                btnMode.Enabled = false;
           
             //   btnIO.Enabled = false;
            }
            else 
            {
                G.EditTool.View.btnRecord.Enabled = false;

                // G.EditTool.View.btnRecord.Enabled = false;
                G.SettingPLC.pCom.Enabled = false;

                G.listProgram.Enabled = IsRun;
              //  btnReport.Enabled = false;
                btnMode.Enabled = false;
                if (G.ResultBar != null)
                    G.ResultBar.btnResetQty.Enabled = false;
              
              //  btnIO.Enabled = false;
                
            }
            if (G.Config.IsExternal)
            {
               G.EditTool.View.btnCap.Enabled = false;
               G.EditTool.View.btnRecord.Enabled = false;
            }
         
            G.EditTool.btnHeaderBar1.btnUser.Text = G.Config.nameUser;
        }
 
        public async void Mode()
        {
            if (G.Config.nameUser != "Admin")
                return;
            if (Global.StatusMode==StatusMode.Once)
            {
                MessageBox.Show("Please Stop Mode Continuous");
                return;
            }
            foreach (Tools tool in G.listAlltool[Global.IndexChoose])
            {
                tool.ItemTool.IsCLick = false;
            }
            Global.IsRun = !Global.IsRun;
       
            if (Global.IsRun)
            {
              
                G.EditTool.View.btnCap.Enabled = true;
                G.EditTool.View.btnRecord.Enabled = true;

               
                G.ToolSettings.btnAdd.Enabled = false;
                G.ToolSettings.btnCopy.Enabled = false;
                G.ToolSettings.btnDelect.Enabled = false;
                G.ToolSettings.btnEnEdit.Enabled = true;
                btnMode.Text = "RUN";
                btnMode.ForeColor = Color.FromArgb(101, 173, 245); ;// Color.DarkSlateGray;
                G.EditTool.RefreshGuiEdit(Step.Run);

            }
            else
            {
                if (G.EditTool.View.btnRecord.IsCLick)
                    if (G.EditTool.View.btnRecord.Enabled == true)
                        G.EditTool.View.btnRecord.PerformClick();
                G.EditTool.btnHeaderBar1.btnSettingPLC.IsCLick = false;
                G.EditTool.View.btnCap.Enabled = false;
                G.EditTool.View.btnRecord.Enabled = false;
                G.EditTool.RefreshGuiEdit(Step.Step1);
                G.ToolSettings.btnAdd.Enabled = true;
                G.ToolSettings.btnCopy.Enabled = true;
                G.ToolSettings.btnDelect.Enabled = true;
                G.ToolSettings.btnEnEdit.Enabled = false;
                btnMode.Text = "EDIT";
                btnMode.ForeColor = Color.DarkSlateGray;
              

            }
            Global.Comunication.IO.WriteIO(IO_Processing.ChangeMode, Global.IsRun);
            Acccess(Global.IsRun);
           
        }
        private void btnMode_Click(object sender, EventArgs e)
        {
            if (G.EditTool.View.btnLive.IsCLick)
            {
                G.EditTool.View.btnLive.PerformClick();
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
            if (G.EditTool.pEdit.Visible && opacity < 1.0f)
            {
                opacity += 0.1f;
                G.EditTool.pEdit.BackColor = Color.FromArgb((int)(opacity * 255), G.EditTool.pEdit.BackColor);
            }
            else if (!G.EditTool.pEdit.Visible && opacity > 0.0f)
            {
                opacity -= 0.1f;
                G.EditTool.pEdit.BackColor = Color.FromArgb((int)(opacity * 255), G.EditTool.pEdit.BackColor);
            }
            else
            {
                fadeTimer.Stop();
                if (opacity == 0.0f) G.EditTool.pEdit.Visible = false;
            }
        }
        private void Header_Load(object sender, EventArgs e)
        {
           
            //    this.Controls.Add(btnHide);

            //fadeTimer = new Timer { Interval = 20 };
            //fadeTimer.Tick += FadeEffect;
            if (G.EditTool == null) return;
            if (G.EditTool.View == null)
            {

                G.EditTool.View = new View();

                G.EditTool.View.Dock = DockStyle.None;
                G.EditTool.View.Size = G.EditTool.pView.Size;
                G.EditTool.View.Anchor = AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right|AnchorStyles.Bottom;
                G.EditTool.View.Location = new Point(0,0);

                G.EditTool.View.Parent = G.EditTool.pView;
            }
            //if (G.IsLoad) return;
          //  pMenu.Region = BeeCore.CustomGui.RoundRg(pMenu, G.Config.RoundRad);
           BeeCore.CustomGui.RoundRg(pModel, G.Config.RoundRad);
        
           BeeCore.CustomGui.RoundRg(pPO, G.Config.RoundRad);

            
         //   pMenu.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
            pPO.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
            pModel.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
            G.listProgram.Font = new Font("Arial", 16);
            G.listProgram.Parent = G.Main;
            G.listProgram.BringToFront();
            G.listProgram.Visible = false;
            G.listProgram.Location = new Point(this.Location.X +btnMode.Width+txtQrCode.Location.X, this.Location.Y+ txtQrCode.Location.Y+txtQrCode.Height+10);
            G.listProgram.Width = txtQrCode.Width;
            G.listProgram.SelectedIndexChanged += ListProgram_SelectedIndexChanged;
            this.myDelegate = new AddDataDelegate(AddDataMethod);
             //cbSerialPort.SelectedIndex = cbSerialPort.FindStringExact(G.Config.namePort);
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
        G.EditTool.txtRecept.Text=myString;
            
            if (sRecept.Contains("IO")&& !IsWaitPort)
            {
                IsWaitPort = true;
                tmScanPort.Tick -= TmScanPort_Tick;
                tmScanPort.Enabled = false;
             
                G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
                G.EditTool.toolStripPort.Text = "Port Connected";
                SaveData.Config(G.Config);
                G.EditTool.View.tmCheckPort.Enabled = true;
            }
           else if  (sRecept.Contains("Trig"))
                {
                G.EditTool.View.Cap(false);
                }
            else if (sRecept.Contains("Mod"))
            {
                //SerialPort.WriteLine("Done");
                btnMode.PerformClick();
                if (btnMode.Text=="RUN")
                    SerialPort1.WriteLine("Run");
                else
                    SerialPort1.WriteLine("Edit");
            }
            else if (sRecept.Contains("Rec"))
            {
              G.EditTool.View.btnLive.PerformClick();
                if (G.EditTool.View.btnLive.IsCLick)
                    SerialPort1.WriteLine("Live");
                else
                    SerialPort1.WriteLine("Cap");
            }
            else if (sRecept.Contains("Done"))
            {
               // G.IsDone = false;
            }
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
            if(!G.IsLoad) return;
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
            ClassProject.Load(Global.Project);
        }

        private void workLoadProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsIntialProgram = true;
            if (Global.ParaCommon.matRegister != null)
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = OpenCvSharp.Extensions.BitmapConverter.ToMat(Global.ParaCommon.matRegister);
            else if (G.IsCCD)
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = null;// BeeCore.Common.GetImageRaw();
            if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
            {
                G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                //BeeCore.Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw);
            }
            if (G.ToolSettings == null)
            {
                G.ToolSettings = new ToolSettings();

            }
          
            Properties.Settings.Default.programCurrent = Global.Project;
            Properties.Settings.Default.Save();
            G.listProgram.Visible = false;
            txtQrCode.Enabled = true;
            btnShowList.Enabled = true;
            txtQrCode.Text = Global.Project;

           
            if (Global.Comunication.IO.IsConnected)
            {
                Global.Comunication.IO.WriteIO(IO_Processing.Reset);
                G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
                tmReadPLC.Enabled = true;
            }

            else
            {
                G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                if(!Global.Comunication.IO.IsBypass)
                tmReConnectPLC.Enabled = true;
            }
            Acccess(Global.IsRun);
            G.listProgram.Visible = false;
            tmIninitial.Enabled = true;
            G.ToolSettings.pAllTool.Controls.Clear();

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
            if (G.EditTool == null) return;
           BeeCore.CustomGui.RoundRg(pModel, G.Config.RoundRad);

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
                G.EditTool.View.imgView.Visible = true;
                G.EditTool.View.imgView.Size = G.EditTool.View.pView.Size;
                    stepShow++;
                    tmShow.Interval = 500;
                    break;
                case 1:
                    G.EditTool.RefreshGuiEdit(Step.Run);
                    stepShow++;
                    break;
                case 2:
                    if (indexToolShow < G.listAlltool[Global.IndexChoose].Count)
                    {
                        tmShow.Interval = 50;
                       
                        G.ToolSettings.pAllTool.Controls.Add(G.listAlltool[Global.IndexChoose][indexToolShow].ItemTool);
                        indexToolShow++;
                        G.ToolSettings.ResumeLayout(true);
                    }
                    else
                    {
                        stepShow = 0;
                        indexToolShow = 0;
                        G.EditTool.View.btnFull.PerformClick();
                        tmShow.Enabled = false;
                        G.ToolSettings.ResumeLayout(true);
                    }
                    break;
            } 
           
                
               


         
               
            


        }
      
        private void workPLC_DoWork(object sender, DoWorkEventArgs e)
        {
          

           // Global.Comunication.IO.Read();
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
        public bool IsWaitingRead = false;
        private async void workPLC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (G.SettingPLC != null)
                if (G.SettingPLC.Visible)
                    G.SettingPLC.RefreshValuePLC();
            //if(!CheckLan())
            //{
            //    ShowErr();
            //    return;
            //}
            //if(G.IsPLCNotAlive)
            //    {
            //        if (Global.Comunication.IO.valueInput[3] == 0)
            //        {
            //            G.IsPLCNotAlive = false;
            //            numAlive = 0;


            //        }
            //        return;
            //    }
            if (!BeeCore.Common.listCamera[Global.IndexChoose].IsConnected)
            {
                G.EditTool.lbCam.Text = "Camera Disconnected";
                G.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
            }
               
            else
            {
                G.EditTool.lbCam.Text = "Camera Connected";
                G.EditTool.lbCam.Image = Properties.Resources.CameraConnected;

            }    
               
            if (!Global.Comunication.IO.IsConnected)
            {
                if (!G.SettingPLC.pCom.Enabled)
                    G.SettingPLC.pCom.Enabled = true;
                tmReadPLC.Enabled = false;
                tmReConnectPLC.Enabled = true;
                G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
            }    
            else
            {
                if ( G.SettingPLC.pCom.Enabled)
                G.SettingPLC.pCom.Enabled = false;
                if (Global.Comunication.IO.valueInput.Count ()< Global.Comunication.IO.LenReads[0]) return;
                if (Global.Comunication.IO.valueOutput.Count() < Global.Comunication.IO.LenReads[1]) return;
                if (G.IsSendRS)
                {
                    Global.Comunication.IO.WriteIO(IO_Processing.Result, G.TotalOK, G.Config.DelayOutput);
                    //if (G.TotalOK)
                    //{
                    //    Global.Comunication.IO.SetOutPut(0, false); //OK
                    //    Global.Comunication.IO.SetOutPut(5, false); //Light
                    //    Global.Comunication.IO.SetOutPut(6, false); //Busy
                    //    Global.Comunication.IO.WriteOutPut();
                    //    await Task.Delay(G.Config.DelayOutput);
                    //    Global.Comunication.IO.SetOutPut(4, true);//Ready false
                    //    Global.Comunication.IO.SetOutPut(0, false); //OK
                       
                    //    Global.Comunication.IO.WriteOutPut();




                    //}
                    //else
                    //{

                    //    if (Global.Comunication.IO.valueInput[3] == 1)
                    //    {
                    //        Global.Comunication.IO.SetOutPut(0, false); //OK
                    //        Global.Comunication.IO.SetOutPut(5, false); //Light
                    //        Global.Comunication.IO.SetOutPut(6, false); //Busy
                    //        Global.Comunication.IO.WriteOutPut();
                    //        await Task.Delay(G.Config.DelayOutput);
                    //        Global.Comunication.IO.SetOutPut(4, true);//Ready false
                    //        Global.Comunication.IO.SetOutPut(0, false); //OK
                          
                    //        Global.Comunication.IO.WriteOutPut();
                    //    }
                    //    else
                    //    {
                    //        Global.Comunication.IO.SetOutPut(0, true); //NG
                    //        Global.Comunication.IO.SetOutPut(5, false); //Light
                    //        Global.Comunication.IO.SetOutPut(6, false); //Busy
                    //        Global.Comunication.IO.WriteOutPut();
                    //        await Task.Delay(G.Config.DelayOutput);
                    //        Global.Comunication.IO.SetOutPut(4, true);//Ready false
                    //        Global.Comunication.IO.SetOutPut(0, false); //False
         
                    //        Global.Comunication.IO.WriteOutPut();
                    //    }
                    //}
                    G.IsSendRS = false;
                }
                if(!Global.IsRun)
                if ( Global.ParaCommon.IsOnLight!=Convert.ToBoolean( Global.Comunication.IO.valueOutput[5]))
                    {
                        Global.Comunication.IO.WriteIO(IO_Processing.Light, Global.ParaCommon.IsOnLight);
                    }
               
              
                if (!Global.Comunication.IO.CheckErr(BeeCore.Common.listCamera[Global.IndexChoose].IsConnected))
                {
                    ShowErr();
                    return;
                }
              

                if(Global.IsRun&& G.Config.IsExternal)
                {
                    if (Global.Comunication.IO.CheckReady())
                    {
                        Global.Comunication.IO.WriteIO(IO_Processing.Trigger);

                        await Task.Delay(G.Config.delayTrigger);
                        if (G.Config.IsExternal)
                            G.EditTool.View.btnTypeTrig.IsCLick = true;
                        if (Global.IsRun)
                            G.EditTool.View.Cap(false);
                        else
                            tmReadPLC.Enabled = true;
                        IsWaitingRead = true;
                    }
                    else
                    {
                        tmReadPLC.Enabled = true;
                    }
                }
                else
                 tmReadPLC.Enabled = true;
                if (btnEnQrCode.IsCLick)
                {
                    if (Global.Comunication.IO.valueOutput[6] == 0)
                    {
                        int[] bits = new int[] { Global.Comunication.IO.valueInput[4], Global.Comunication.IO.valueInput[5], Global.Comunication.IO.valueInput[6], Global.Comunication.IO.valueInput[7] };  // MSB -> LSB (bit3 bit2 bit1 bit0)

                        int value = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            value |= (bits[i] & 1) << (3 - i);  // bit 3 là cao nhất
                        }
                        int id = listFilter.FindIndex(a => a == Global.Project);
                        if (id != value)
                        {

                            Global.Comunication.IO.WriteIO(IO_Processing.ChangeProg);
                            tmReadPLC.Enabled = false;
                            Global.Project = listFilter[value];
                            txtQrCode.Text = Global.Project.ToString();
                            txtQrCode.Enabled = false;
                            btnShowList.Enabled = false;

                            workLoadProgram.RunWorkerAsync();
                        }
                    }
                }
              
                 
                G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
            }
          
            if (G.SettingPLC!=null)
                if (G.SettingPLC.Visible)
                    G.SettingPLC.RefreshValuePLC();
        }

        private async void tmReadPLC_Tick(object sender, EventArgs e)
        {
            //  G.EditTool.View.lbNum.Text = BeeCore.Common.listRaw.Count()+ "img";
            if (!G.Initial) return;
            Parallel.For(0, 1, i =>
            {
                Global.Comunication.IO.Read();
              //  Console.WriteLine($"Task {i} running on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            });
            if (G.SettingPLC != null)
                if (G.SettingPLC.Visible)
                    G.SettingPLC.RefreshValuePLC();
            //if(!CheckLan())
            //{
            //    ShowErr();
            //    return;
            //}
            //if(G.IsPLCNotAlive)
            //    {
            //        if (Global.Comunication.IO.valueInput[3] == 0)
            //        {
            //            G.IsPLCNotAlive = false;
            //            numAlive = 0;


            //        }
            //        return;
            //    }
            if (!BeeCore.Common.listCamera[Global.IndexChoose].IsConnected)
            {
                G.EditTool.lbCam.Text = "Camera Disconnected";
                G.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
            }

            else
            {
                G.EditTool.lbCam.Text = "Camera Connected";
                G.EditTool.lbCam.Image = Properties.Resources.CameraConnected;

            }

            if (!Global.Comunication.IO.IsConnected)
            {
                if (!G.SettingPLC.pCom.Enabled)
                    G.SettingPLC.pCom.Enabled = true;
                tmReadPLC.Enabled = false;
                tmReConnectPLC.Enabled = true;
                G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
            }
            else
            {
                if (G.SettingPLC.pCom.Enabled)
                    G.SettingPLC.pCom.Enabled = false;
                if (Global.Comunication.IO.valueInput.Count() < Global.Comunication.IO.LenReads[0]) return;
                if (Global.Comunication.IO.valueOutput.Count() < Global.Comunication.IO.LenReads[1]) return;
                if (G.IsSendRS)
                {
                    Global.Comunication.IO.WriteIO(IO_Processing.Result, G.TotalOK, G.Config.DelayOutput);
                    //if (G.TotalOK)
                    //{
                    //    Global.Comunication.IO.SetOutPut(0, false); //OK
                    //    Global.Comunication.IO.SetOutPut(5, false); //Light
                    //    Global.Comunication.IO.SetOutPut(6, false); //Busy
                    //    Global.Comunication.IO.WriteOutPut();
                    //    await Task.Delay(G.Config.DelayOutput);
                    //    Global.Comunication.IO.SetOutPut(4, true);//Ready false
                    //    Global.Comunication.IO.SetOutPut(0, false); //OK

                    //    Global.Comunication.IO.WriteOutPut();




                    //}
                    //else
                    //{

                    //    if (Global.Comunication.IO.valueInput[3] == 1)
                    //    {
                    //        Global.Comunication.IO.SetOutPut(0, false); //OK
                    //        Global.Comunication.IO.SetOutPut(5, false); //Light
                    //        Global.Comunication.IO.SetOutPut(6, false); //Busy
                    //        Global.Comunication.IO.WriteOutPut();
                    //        await Task.Delay(G.Config.DelayOutput);
                    //        Global.Comunication.IO.SetOutPut(4, true);//Ready false
                    //        Global.Comunication.IO.SetOutPut(0, false); //OK

                    //        Global.Comunication.IO.WriteOutPut();
                    //    }
                    //    else
                    //    {
                    //        Global.Comunication.IO.SetOutPut(0, true); //NG
                    //        Global.Comunication.IO.SetOutPut(5, false); //Light
                    //        Global.Comunication.IO.SetOutPut(6, false); //Busy
                    //        Global.Comunication.IO.WriteOutPut();
                    //        await Task.Delay(G.Config.DelayOutput);
                    //        Global.Comunication.IO.SetOutPut(4, true);//Ready false
                    //        Global.Comunication.IO.SetOutPut(0, false); //False

                    //        Global.Comunication.IO.WriteOutPut();
                    //    }
                    //}
                    G.IsSendRS = false;
                }
                if (!Global.IsRun)
                    if (Global.ParaCommon.IsOnLight != Convert.ToBoolean(Global.Comunication.IO.valueOutput[5]))
                    {
                        Global.Comunication.IO.WriteIO(IO_Processing.Light, Global.ParaCommon.IsOnLight);
                    }


                if (!Global.Comunication.IO.CheckErr(BeeCore.Common.listCamera[Global.IndexChoose].IsConnected))
                {
                    ShowErr();
                    return;
                }


                if (Global.IsRun && G.Config.IsExternal)
                {
                    if (Global.Comunication.IO.CheckReady())
                    {
                        Global.Comunication.IO.WriteIO(IO_Processing.Trigger);

                        await Task.Delay(G.Config.delayTrigger);
                        if (G.Config.IsExternal)
                            G.EditTool.View.btnTypeTrig.IsCLick = true;
                        if (Global.IsRun)
                            G.EditTool.View.Cap(false);
                        else
                            tmReadPLC.Enabled = true;
                        IsWaitingRead = true;
                    }
                    else
                    {
                        tmReadPLC.Enabled = true;
                    }
                }
                else
                    tmReadPLC.Enabled = true;
                if (btnEnQrCode.IsCLick)
                {
                    if (Global.Comunication.IO.valueOutput[6] == 0)
                    {
                        int[] bits = new int[] { Global.Comunication.IO.valueInput[4], Global.Comunication.IO.valueInput[5], Global.Comunication.IO.valueInput[6], Global.Comunication.IO.valueInput[7] };  // MSB -> LSB (bit3 bit2 bit1 bit0)

                        int value = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            value |= (bits[i] & 1) << (3 - i);  // bit 3 là cao nhất
                        }
                        int id = listFilter.FindIndex(a => a == Global.Project);
                        if (id != value)
                        {

                            Global.Comunication.IO.WriteIO(IO_Processing.ChangeProg);
                            tmReadPLC.Enabled = false;
                            Global.Project = listFilter[value];
                            txtQrCode.Text = Global.Project.ToString();
                            txtQrCode.Enabled = false;
                            btnShowList.Enabled = false;

                            workLoadProgram.RunWorkerAsync();
                        }
                    }
                }


                G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
            }

            if (G.SettingPLC != null)
                if (G.SettingPLC.Visible)
                    G.SettingPLC.RefreshValuePLC();
            //if (!workPLC.IsBusy)
            //    {
            //        workPLC.RunWorkerAsync();
            //        tmReadPLC.Enabled = false;
            //    }

        }

        private void tmReConnectPLC_Tick(object sender, EventArgs e)
        {
            Parallel.For(0, 1, i =>
            {
                Global.Comunication.IO.Connect(G.Config.IDPort);                                                                    
                //  Console.WriteLine($"Task {i} running on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            });
         
            if (Global.Comunication.IO.IsConnected)
            {
                Global.Comunication.IO.WriteIO(IO_Processing.Reset);
                G.EditTool.toolStripPort.Text = "PLC Connected";
                // Global.Comunication.IO.WriteInPut(3, true);

                tmReConnectPLC.Enabled = false;
                tmReadPLC.Enabled = true;
                G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;

            }
            else
            {
                tmReConnectPLC.Enabled = true;
                G.EditTool.toolStripPort.Text = "PLC Reconect....";
                G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
            }
            //if (!workReConnect.IsBusy)
            //workReConnect.RunWorkerAsync();
               

        }

        private void editProg1_Load(object sender, EventArgs e)
        {

        }

        private void workReConnect_DoWork(object sender, DoWorkEventArgs e)
        {
          
           
        }

        private void workReConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            if (Global.Comunication.IO.IsConnected)
            {
                Global.Comunication.IO.WriteIO(IO_Processing.Reset);
                G.EditTool.toolStripPort.Text = "PLC Connected";
               // Global.Comunication.IO.WriteInPut(3, true);

                tmReConnectPLC.Enabled = false;
                tmReadPLC.Enabled = true;
                G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;

            }
            else
            {
                G.EditTool.toolStripPort.Text = "PLC Reconect....";
                G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
            }

        }
        int numAlive = 0;
        private void tmOutAlive_Tick(object sender, EventArgs e)
        {
           
          //  tmOutAlive.Enabled = false;
            if (!Global.IsRun) return;
            if (Global.Comunication.IO.valueInput[3] == 1 )
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
            G.Initial = true;
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
