﻿
using BeeCore;
using BeeCore.EtherNetIP;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;

using BeeUi.Tool;
using Google.Apis.Auth.OAuth2;
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
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Configuration;
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
                if (Global.ParaCommon.Comunication.Protocol.IsConnected)
                {
                    //if (Global.ParaCommon.Comunication.Protocol.valueOutput[4] == 0)
                    //{

                    //   // Global.ParaCommon.Comunication.Protocol.WriteOutPut(4, true);

                    //}
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

       
 
    
        private void btnMode_Click(object sender, EventArgs e)
        {
            if(G.listProgram!=null)
            if (G.listProgram.Visible == true) G.listProgram.Visible = false;
            if (Global.EditTool.View.btnLive.IsCLick)
            {
                Global.EditTool.View.btnLive.PerformClick();
            }
           
            if (Global.StatusMode == StatusMode.Once)
            {
                Global.StatusMode = StatusMode.None;
                Global.StatusProcessing = StatusProcessing.None;


            }
            foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
            {
                PropetyTool.ItemTool.IsCLick = false;
            }
            if (Global.IsRun)
            {
                if (Global.ParaCommon.IsMultiCamera)
                {
                    FormChoose formChoose = new FormChoose();
                    formChoose.ShowDialog();
                    if (Global.Step == Step.Step1)
                    {
                        CameraForm cameraForm = new CameraForm();
                        cameraForm.ShowDialog();
                    }
                    else
                    {
                        btnMode.Text = "RUN"; btnMode.IsCLick = false;
                        btnMode.ForeColor = Color.FromArgb(101, 173, 245); ;// Color.DarkSlateGray;
                        return;
                    }
                }
                else
                {
                    FormChoose formChoose = new FormChoose();
                    formChoose.ShowDialog();
                }
                if (Global.Step == Step.Step1)
                {

                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
                    {
                        PropetyTool.ItemTool.IsEdit = false;
                    }
                    btnMode.Text = "EDIT";
                    btnMode.ForeColor = Color.DarkSlateGray;
                }
                else
                {
                    btnMode.Text = "RUN"; btnMode.IsCLick = false;
                    btnMode.ForeColor = Color.FromArgb(101, 173, 245); ;// Color.DarkSlateGray;
                    return;

                }

            }

          Global.EditTool.  Acccess(Global.IsRun);
        }
        String[] PathFile;
        bool IsLoad = false;
        public void RefreshListPJ()
        {
            if (G.listProgram == null)
            {

                G.listProgram = new System.Windows.Forms.ListBox();
                G.listProgram.Font = new Font("Arial", 16);
                G.listProgram.Parent = G.Main;
                G.listProgram.BringToFront();
                G.listProgram.Visible = false;
                G.listProgram.Location = new Point(this.Location.X + btnMode.Width + txtQrCode.Location.X, this.Location.Y + txtQrCode.Location.Y + txtQrCode.Height + 10);
                G.listProgram.Width = txtQrCode.Width;
               
                G.listProgram.SelectedIndexChanged += ListProgram_SelectedIndexChanged;
            }
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
          
        
          ///  RefreshListPJ();
            //    this.Controls.Add(btnHide);

            //fadeTimer = new Timer { Interval = 20 };
            //fadeTimer.Tick += FadeEffect;
            if (G.Main == null) return;
            if (Global.EditTool == null) return;
            if (Global.EditTool.View == null)
            {

                Global.EditTool.View = new View();

                Global.EditTool.View.Dock = DockStyle.None;
                Global.EditTool.View.Size = Global.EditTool.pView.Size;
                Global.EditTool.View.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                Global.EditTool.View.Location = new Point(0, 0);

                Global.EditTool.View.Parent = Global.EditTool.pView;
            }
            //if (G.IsLoad) return;
            //  pMenu.Region = BeeCore.CustomGui.RoundRg(pMenu,Global.Config.RoundRad);
   //         BeeCore.CustomGui.RoundRg(pModel, Global.Config.RoundRad);
   //if (Global.Config!=null)
   //         BeeCore.CustomGui.RoundRg(pPO, Global.Config.RoundRad);


            //   pMenu.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
           // pPO.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, Global.Config.colorGui);
           // pModel.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, Global.Config.colorGui);
         
        
            if (Global.ToolSettings == null)
            {
                Global.ToolSettings = new ToolSettings();

            }
            if (G.SettingPLC == null)
            {
                G.SettingPLC = new ProtocolPLC();
            }

            if (Global.ParaCommon.Comunication == null)
            {
                Global.ParaCommon.Comunication = new Comunication();
                Global.ParaCommon.Comunication.Protocol = new ParaProtocol();
              

            }
            //cbSerialPort.SelectedIndex = cbSerialPort.FindStringExact(Global.Config.namePort);
            if (!Directory.Exists("Program"))
            {
                Directory.CreateDirectory("Program");
            }
            else
            {
                // Access.SaveProg("Program\\Default.prog", new List<PropetyTool>());


                Global.Project = Properties.Settings.Default.programCurrent;


                txtQrCode.Text = Global.Project;
                txtQrCode.Enabled = false;
                btnShowList.Enabled = false;
                if (!workLoadProgram.IsBusy)
                    workLoadProgram.RunWorkerAsync();


            }
            tmShow.Interval = 1000;
           // ThreadPool.SetMinThreads(Environment.ProcessorCount, Environment.ProcessorCount);
            //Acccess(Global.IsRun);
            G.Main.Location = new Point(0, 0);

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
           
           
            Global.Project= program;
             Properties.Settings.Default.programCurrent = Global.Project;
             Properties.Settings.Default.Save();
            if (!workLoadProgram.IsBusy)
                workLoadProgram.RunWorkerAsync();
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
            if (Global.Initialed)
            {
                if (G.listProgram == null)
                {

                    G.listProgram = new System.Windows.Forms.ListBox();
                    G.listProgram.Font = new Font("Arial", 16);
                    G.listProgram.Parent = G.Main;
                    G.listProgram.BringToFront();
                    G.listProgram.Visible = false;
                    G.listProgram.Location = new Point(this.Location.X + btnMode.Width + txtQrCode.Location.X, this.Location.Y + txtQrCode.Location.Y + txtQrCode.Height + 10);
                    G.listProgram.Width = txtQrCode.Width;
                    RefreshListPJ();
                    G.listProgram.SelectedIndexChanged += ListProgram_SelectedIndexChanged;
                }
            }
            if (Global.IsLoadProgFist)
            {
                Global.IsLoadProgFist = false;
                return;
            }
            if (Global.Initialed)
                G.listProgram.Visible = true;
            if (IsKeyEnter) return;
                // Lấy chuỗi tìm kiếm từ TextBox
                string filter = txtQrCode.Text.ToLower();

            // Lọc danh sách dựa trên chuỗi tìm kiếm
            listFilter = items.Where(item => item.ToLower().Contains(filter)).ToList();
                IsKeyPress = true;
            // Cập nhật ComboBox với các mục đã lọc
            if (Global.Initialed)
                G.listProgram.DataSource = new BindingSource(listFilter, null);
            

        }

        private void pProgram_SizeChanged(object sender, EventArgs e)
        {
          
        }

        private void Header_SizeChanged(object sender, EventArgs e)
        {
           // G.listProgram.Location = new Point(this.Location.X +txtQrCode.Parent.Location.X + txtQrCode.Location.X, this.Location.Y + txtQrCode.Location.Y + txtQrCode.Height + 10);
           // G.listProgram.Width = txtQrCode.Width;
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
            if (G.listProgram == null)
            {

                G.listProgram = new System.Windows.Forms.ListBox();
                G.listProgram.Font = new Font("Arial", 16);
                G.listProgram.Parent = G.Main;
                G.listProgram.BringToFront();
                G.listProgram.Visible = false;
              //  G.listProgram.Location = new Point(this.Location.X + btnMode.Width + txtQrCode.Location.X, this.Location.Y + txtQrCode.Location.Y + txtQrCode.Height + 10);
                G.listProgram.Width = txtQrCode.Width;
              //  RefreshListPJ();
                G.listProgram.SelectedIndexChanged += ListProgram_SelectedIndexChanged;
            }
           G.listProgram.Location = new Point(pModel.Location.X+ txtQrCode.Location.X, this.Location.Y + pModel.Location.Y  + txtQrCode.Location.Y + txtQrCode.Height + 10);
            G.listProgram.Width = txtQrCode.Width;
            G.listProgram.Visible=!G.listProgram.Visible;
           
            if (G.listProgram.Visible )
            {
                RefreshListPJ();
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
            {
                Global.ScanCCD.DisConnectAllCCd();
                DataTool.LoadProject(Global.Project);
                

            }    
           
         
        }

        private async void workLoadProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsIntialProgram)
            {
               
                if (! await Global.ScanCCD.ChangeCCD())
                {
                    Global.CameraStatus = CameraStatus.ErrorConnect;
                }
                else
                {
                    Global.CameraStatus = CameraStatus.Ready;
                }    
            }
           
            if ( G.listProgram!=null)

            G.listProgram.Visible = false;
            txtQrCode.Enabled = true;
            btnShowList.Enabled = true;
            txtQrCode.Text = Global.Project;

           
           
            IsIntialProgram = true;
            Global.EditTool. Acccess(Global.IsRun);
           
            tmIninitial.Enabled = true;
        
           
            tmShow.Enabled = true;
           
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
      
        private void AnimateColumn( System.Windows.Forms.TableLayoutPanel tableLayoutPanel, int columnIndex, bool show)
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
          //  if (Global.EditTool == null) return;
          // BeeCore.CustomGui.RoundRg(pModel,Global.Config.RoundRad);

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
                    tmShow.Interval = 100;
                    Global.ToolSettings.Size = Global.EditTool.pEditTool.Size;
                  //  Global.ToolSettings.ResumeLayout(true);
                    break;
                case 1:
                    Global.EditTool.RefreshGuiEdit(Step.Run);
                    stepShow++;
                    break;
                case 2:
                    if (pEdit.Width <= pEdit. btnMenu.Width + 1)
                    {
                        pEdit.btnMenu.IsCLick = true;
                      
                    }
                    pEdit.OldWidth = Global.Config.WidthEditProg;
                    stepShow = 0;
                        indexToolShow = 0;
                    if (BeeCore.Common.listCamera[Global.IndexChoose]!=null)
                    {
                        Global.EditTool.View.btnFull.PerformClick();

                    }  
                        tmShow.Enabled = false;
                       // Global.ToolSettings.ResumeLayout(true);

                    break;
            } 
           
                
               


         
               
            


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
        
        private void tmIninitial_Tick(object sender, EventArgs e)
        {
            tmIninitial.Enabled= false;
            if (Global.LogsDashboard == null)
            {
                Global.LogsDashboard = new LogsDashboard();
                Global.LogsDashboard.MaxLogCount = 5000;
                Global.LogsDashboard.ProgressiveBatchSize = 200;
                Global.LogsDashboard.ProgressiveIntervalMs = 10;
                Global.LogsDashboard.IngestBatchSize = 100;
                Global.LogsDashboard.IngestIntervalMs = 16; // ~60Hz
            }
            Global.Initialed = true;
        }

        private void editProg1_Load_1(object sender, EventArgs e)
        {

        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            Global.IsAutoTemp = btnTraining.IsCLick;
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
