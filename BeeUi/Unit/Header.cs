
using BeeCore;
using BeeCore.EtherNetIP;
using BeeCore.Funtion;
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
      

      
        public List<Commons.ItemTool> itemTools = new List<Commons.ItemTool>();
        //int indexTool;
        //public void CreateItemTool(BeeCore.PropetyTool PropetyTool)
        //{
        //    BeeCore.TypeTool TypeTool = PropetyTool.TypeTool;
        //    dynamic control = IniTool(TypeTool);
        //    int with = 50, height = 50;
        //    control.Propety = PropetyTool.Propety;
        //    control.Propety.Index = indexTool;
        ////    BeeCore.RectRotate rotCrop = control.Propety.rotCrop;
        //    if (PropetyTool.TypeTool == TypeTool.Yolo || PropetyTool.TypeTool == TypeTool.OCR || PropetyTool.TypeTool == TypeTool.BarCode || PropetyTool.TypeTool == TypeTool.Color_Area)
        //        control.Propety.rotCrop = null;
        //        System.Drawing.Size szCCd= BeeCore.G.ParaCam.SizeCCD;
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
        //    Commons.ItemTool item = new Commons.ItemTool(TypeTool, TypeTool.ToString() + Convert.ToString(G.listAlltool[G.indexChoose].Count - 1));
        //    item.Location = new Point(G.ToolSettings.X, G.ToolSettings.Y);
        //    item.lbCycle.Text = "---";
        //    item.lbScore.Text = "---";
        //    item.lbStatus.Text = "---";
        //    item.Score.Value = Convert.ToInt32((double)control.Propety.Score);
        //    item.lbScore.ForeColor = Color.Gray;
        //    item.lbStatus.BackColor = Color.Gray;
        //    G.ToolSettings.Y += item.Height + 10;
        //    G.listAlltool.Add(new Tools(item, control, PropetyTool));

        //    //control.pro.indexTool = G.listAlltool[G.indexChoose].Count - 1;
        //    BeeCore.Common.CreateTemp(TypeTool);
        //    if (PropetyTool.Name == null) PropetyTool.Name = "";
        //    if (PropetyTool.Name.Trim() == "")
        //        item.name.Text = TypeTool.ToString() + " " + G.listAlltool[G.indexChoose].Count();
        //    else
        //        item.name.Text = PropetyTool.Name.Trim();
        //    control.Name = PropetyTool.Name;
        //    PropetyTool.Propety.nameTool= PropetyTool.Name;
            
           

        //    item.lbNumber.Text = G.listAlltool[G.indexChoose].Count() + "";
         
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
                if (G.PLC.IsConnected)
                {
                    if (G.PLC.valueOutput[4] == 0)
                    {

                       // G.PLC.WriteOutPut(4, true);

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
            //            G.PLC.Connect(G.Config.IDPort);
            //           // Modbus.ConnectPLC(G.Config.namePort.Trim());

            //        }
                   
            //    }
            //    //Thread.Sleep(200);
            //    //switch (BeeCore.G.ParaCam.TypeLight)
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
            G.EditTool.btnHeaderBar1.btnSettingPLC.Enabled= G.IsRun;
            G.EditTool.View.btnLive.Enabled = !G.IsRun;
            pModel.Enabled = IsRun;
          
            if (G.Config.nameUser == "Admin")
            {
                G.SettingPLC.pCom.Enabled = true;
                if(G.ResultBar!=null)
                G.ResultBar.btnResetQty.Enabled = true;
                G.EditTool.View.btnRecord.Enabled = G.IsRun;
                

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
            if (G.StatusMode==StatusMode.Once)
            {
                MessageBox.Show("Please Stop Mode Continuous");
                return;
            }
            foreach (Tools tool in G.listAlltool[G.indexChoose])
            {
                tool.ItemTool.IsCLick = false;
            }
            G.IsRun = !G.IsRun;
       
            if (G.IsRun)
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
            //if (!btnMode.IsCLick)
            //{
            //    if (G.Header.SerialPort1.IsOpen)
            //        G.Header.SerialPort1.WriteLine("Runn");
            //    //if (btnHide.IsCLick)
            //   //     btnHide.PerformClick();
            //}
            //else
            //{

            //    if (G.Header.SerialPort1.IsOpen)
            //        G.Header.SerialPort1.WriteLine("Edit");
            //    //if (!btnHide.IsCLick)
            //    //    btnHide.PerformClick();
            //}

            Acccess(G.IsRun);
           
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

                G.Project= Properties.Settings.Default.programCurrent;

                
                    txtQrCode.Text = G.Project;
                txtQrCode.Enabled = false;
                btnShowList.Enabled = false;
                if (!workLoadProgram.IsBusy)
                    workLoadProgram.RunWorkerAsync();


            }
            ThreadPool.SetMinThreads(Environment.ProcessorCount, Environment.ProcessorCount);
            //Acccess(G.IsRun);
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

            G.Project= program;
            txtQrCode.Text = G.Project;
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
            if (!G.IsRun) return;
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
            ClassProject.Load(G.Project);
        }

        private void workLoadProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsIntialProgram = true;
            if (BeeCore.G.ParaCam.matRegister != null)
                BeeCore.Common.listCamera[G.indexChoose].matRaw = OpenCvSharp.Extensions.BitmapConverter.ToMat(BeeCore.G.ParaCam.matRegister);
            else if (G.IsCCD)
                BeeCore.Common.listCamera[G.indexChoose].matRaw = null;// BeeCore.Common.GetImageRaw();
            if (BeeCore.Common.listCamera[G.indexChoose].matRaw != null)
            {
                G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
                //BeeCore.Native.SetImg(BeeCore.Common.listCamera[G.indexChoose].matRaw);
            }
            if (G.ToolSettings == null)
            {
                G.ToolSettings = new ToolSettings();

            }
          
            Properties.Settings.Default.programCurrent = G.Project;
            Properties.Settings.Default.Save();
            G.listProgram.Visible = false;
            txtQrCode.Enabled = true;
            btnShowList.Enabled = true;
            txtQrCode.Text = G.Project;

           
            if (G.PLC.IsConnected)
            {
                //G.PLC.WriteInPut(3, true);
                //G.Config.DelayOutput = G.PLC.ReadPara(4160);

                //G.PLC.WritePara(4160, G.Config.DelayOutput);
                G.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
                tmReadPLC.Enabled = true;
            }

            else
            {
                G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                if(!G.IsByPassPLC)
                tmReConnectPLC.Enabled = true;
            }
            Acccess(G.IsRun);
            G.listProgram.Visible = false;
            tmIninitial.Enabled = true;
            G.ToolSettings.pAllTool.Controls.Clear();

            tmShow.Enabled = true;
            if(BeeCore.Common.listParaCamera[0]!=null)
                CameraBar.btnCamera1.Text = BeeCore.Common.listParaCamera[0].Name.Substring(0, 8) + "..";
            if (BeeCore.Common.listParaCamera[1] != null)
                CameraBar.btnCamera2.Text = BeeCore.Common.listParaCamera[1].Name.Substring(0, 8) + "..";
            if (BeeCore.Common.listParaCamera[2] != null)
                CameraBar.btnCamera3.Text = BeeCore.Common.listParaCamera[2].Name.Substring(0, 8) + "..";
            if (BeeCore.Common.listParaCamera[3] != null)
                CameraBar.btnCamera4.Text = BeeCore.Common.listParaCamera[3].Name.Substring(0, 8) + "..";
        }

        private void workSaveProject_DoWork(object sender, DoWorkEventArgs e)
        {
       
        }

        private void workSaveProject_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveData.Project(G.Project);
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
                    if (indexToolShow < G.listAlltool[G.indexChoose].Count)
                    {
                        tmShow.Interval = 50;
                       
                        G.ToolSettings.pAllTool.Controls.Add(G.listAlltool[G.indexChoose][indexToolShow].ItemTool);
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
          

            G.PLC.Read(true);
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
            if(!CheckLan())
            {
                ShowErr();
                return;
            }
        //if(G.IsPLCNotAlive)
        //    {
        //        if (G.PLC.valueInput[3] == 0)
        //        {
        //            G.IsPLCNotAlive = false;
        //            numAlive = 0;
               

        //        }
        //        return;
        //    }
                if (!BeeCore.Common.listCamera[G.indexChoose].IsConnected)
            {
                G.EditTool.lbCam.Text = "Camera Disconnected";
                G.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
            }
               
            else
            {
                G.EditTool.lbCam.Text = "Camera Connected";
                G.EditTool.lbCam.Image = Properties.Resources.CameraConnected;

            }    
               
            if (!G.PLC.IsConnected)
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
                if (G.PLC.valueInput.Count ()< G.PLC.LenReads[0]) return;
                if (G.PLC.valueOutput.Count() < G.PLC.LenReads[1]) return;
                if (G.IsSendRS)
                {
                    if (G.TotalOK)
                    {


                        G.PLC.SetOutPut(0, false); //OK
                        G.PLC.SetOutPut(5, false); //Light
                        G.PLC.SetOutPut(6, false); //Busy
                        G.PLC.WriteOutPut();
                        await Task.Delay(G.Config.DelayOutput);
                        G.PLC.SetOutPut(4, true);//Ready false
                        G.PLC.SetOutPut(0, false); //OK
                       
                        G.PLC.WriteOutPut();




                    }
                    else
                    {

                        if (G.PLC.valueInput[3] == 1)
                        {
                            G.PLC.SetOutPut(0, false); //OK
                            G.PLC.SetOutPut(5, false); //Light
                            G.PLC.SetOutPut(6, false); //Busy
                            G.PLC.WriteOutPut();
                            await Task.Delay(G.Config.DelayOutput);
                            G.PLC.SetOutPut(4, true);//Ready false
                            G.PLC.SetOutPut(0, false); //OK
                          
                            G.PLC.WriteOutPut();
                        }
                        else
                        {
                            G.PLC.SetOutPut(0, true); //NG
                            G.PLC.SetOutPut(5, false); //Light
                            G.PLC.SetOutPut(6, false); //Busy
                            G.PLC.WriteOutPut();
                            await Task.Delay(G.Config.DelayOutput);
                            G.PLC.SetOutPut(4, true);//Ready false
                            G.PLC.SetOutPut(0, false); //False
         
                            G.PLC.WriteOutPut();
                        }
                    }
                    G.IsSendRS = false;
                }
                if(!G.IsRun)
                if (BeeCore. G.ParaCam.IsOnLight!=Convert.ToBoolean( G.PLC.valueOutput[5]))
                {
                    G.PLC.SetOutPut(5, BeeCore.G.ParaCam.IsOnLight); //Busy
                    G.PLC.WriteOutPut();
                }
                ////Alive
                //if (G.PLC.valueInput[3] == 0)
                //{
                //    G.PLC.WriteInPut(3, true); tmOutAlive.Enabled = false;
                //    numAlive = 0;
                //}
                //else
                //{
                //    tmOutAlive.Enabled = true;
                //}
                if (!G.IsRun)
                {
                    G.PLC.SetOutPut(6, true); //Busy
                    G.PLC.WriteOutPut();
                    //if (G.PLC.valueOutput[2] == 0)
                    //    G.PLC.WriteOutPut(2, true);
                }
                else
                {
                    G.PLC.SetOutPut(6, false); //Not Busy
                    G.PLC.WriteOutPut();
                    //if (G.PLC.valueOutput[2] == 1)
                    //    G.PLC.WriteOutPut(2, false);

                }
                if (!BeeCore.Common.listCamera[G.indexChoose].IsConnected)
                {
                    if (G.PLC.valueOutput[7] == 0)
                    {
                        G.PLC.SetOutPut(7, true);//CCD Err
                        G.PLC.WriteOutPut();
                        ShowErr();
                        return;
                    }
                }     
                else
                {
                    if (G.PLC.valueOutput[7] == 1)
                    {
                        G.PLC.SetOutPut(7, false);//CCD Err
                        G.PLC.WriteOutPut();
                    }
                }

                if(G.IsRun&& G.Config.IsExternal)
                {
                    if (G.PLC.valueInput[0] == 1 && G.PLC.valueOutput[6] == 0)
                    {
                        G.PLC.SetOutPut(4, false);//Ready false
                        G.PLC.SetOutPut(5, true); //Busy
                        G.PLC.SetOutPut(6,true); //Busy
                        G.PLC.WriteOutPut();
                        await Task.Delay(G.Config.delayTrigger);
                        if (G.Config.IsExternal)
                            G.EditTool.View.btnTypeTrig.IsCLick = true;
                        if (G.IsRun)
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
                    if (G.PLC.valueOutput[6] == 0)
                    {
                        int[] bits = new int[] { G.PLC.valueInput[4], G.PLC.valueInput[5], G.PLC.valueInput[6], G.PLC.valueInput[7] };  // MSB -> LSB (bit3 bit2 bit1 bit0)

                        int value = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            value |= (bits[i] & 1) << (3 - i);  // bit 3 là cao nhất
                        }
                        int id = listFilter.FindIndex(a => a == G.Project);
                        if (id != value)
                        {
                           
                            G.PLC.SetOutPut(6, true); //Busy
                            G.PLC.WriteOutPut();
                            tmReadPLC.Enabled = false;
                            G.Project = listFilter[value];
                            txtQrCode.Text = G.Project.ToString();
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

        private void tmReadPLC_Tick(object sender, EventArgs e)
        {
            //  G.EditTool.View.lbNum.Text = BeeCore.Common.listRaw.Count()+ "img";
            if (!G.Initial) return;
          
                if (!workPLC.IsBusy)
                {
                    workPLC.RunWorkerAsync();
                    tmReadPLC.Enabled = false;
                }

        }

        private void tmReConnectPLC_Tick(object sender, EventArgs e)
        {
            if(!workReConnect.IsBusy)
            workReConnect.RunWorkerAsync();
               

        }

        private void editProg1_Load(object sender, EventArgs e)
        {

        }

        private void workReConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            G.PLC.Connect(G.Config.IDPort);
           
        }

        private void workReConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            if (G.PLC.IsConnected)
            {
                G.EditTool.toolStripPort.Text = "PLC Connected";
                G.PLC.WriteInPut(3, true);

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
            if (!G.IsRun) return;
            if (G.PLC.valueInput[3] == 1 )
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
                      
                        G.Project = listFilter[0].ToString().Replace(".prog", "");
                        ChangeProgram(G.Project); 
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
