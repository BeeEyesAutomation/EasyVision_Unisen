using BeeCore;
using BeeGlobal;
using BeeUi.Commons;

using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class ScanCCD : Form
    {
        public ScanCCD()
        {
            InitializeComponent();
            
        }
        static void Disable(string interfaceName)
        {

            //set interface name="Ethernet" admin=DISABLE
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("netsh", "interface set interface name=" + interfaceName + " admin=DISABLE");
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            p.StartInfo = psi;
            p.Start();
        }

        static void Enable(string interfaceName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("netsh", "interface set interface name=" + interfaceName + " admin=ENABLE");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            p.Start();
        }
        string[] listStringCCD;

        public List<string> ScanIDCCD()
        {
            cbCCD.Text = "Waiting Scan";
            //  Disable();
            //  Enable();
            String IDCCD =Global.Config.IDCamera;
            // BeeCore.Common.listCamera.Add(new Camera());
            String sRead = "";
            if (BeeCore.Common.listCamera.Count() > Global.IndexChoose)
                if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
            {
                BeeCore.Common.listCamera[Global.IndexChoose].Init();
                 sRead = BeeCore.Common.listCamera[Global.IndexChoose].Scan();
            }    
          
      
            String[] listStringCCD = sRead.Split('\n');
            cbCCD.DataSource = listStringCCD;
            if (sRead == "No Device")
                btnConnect.Enabled = false;
            else
                btnConnect.Enabled = true;
            return listStringCCD.ToList();
        }
        private void ScanCCD_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
            if (Global.IndexChoose >= 0)
                switch (Global.IndexChoose)
                {
                    case 0:
                        btnCamera1.IsCLick = true;
                        break;
                    case 1:
                        btnCamera2.IsCLick = true;
                        break;
                    case 2:
                        btnCamera3.IsCLick = true;
                        break;
                    case 3:
                        btnCamera4.IsCLick = true;
                        break;

                }
            if (Global.IndexChoose >= 0)
            {
                if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
                    BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(new ParaCamera(), 0);
                switch (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera)
                {
                    case TypeCamera.USB:
                        btnUSB2_0.IsCLick = true;
                        break;
                    case TypeCamera.BaslerGigE:
                        btnGigE.IsCLick = true;
                        break;
                    case TypeCamera.TinyIV:
                        btnCameraTiny.IsCLick = true;
                        break;

                }
            }
         
            //   cbCCD.DataSource = ScanIDCCD();
            //if (Global.Config.Resolution == null)Global.Config.Resolution = "1280x720 (1.3 MP)";
            //cbReSolution.SelectedIndex = cbReSolution.FindStringExact(Global.Config.Resolution);
        }
     public   int indexCCD;
        
        public void ConnectCCD()
        {
            indexCCD = cbCCD.SelectedIndex;
           BeeCore.Common.listCamera[Global.IndexChoose].Para.Name = cbCCD.Text;
           Global.Config.IDCamera = cbCCD.Text.Trim();
            G.Load.FormActive.CheckActive(G.Load.addMac);
            if(G.IsActive)
            {
                if (!work.IsBusy)
                    work.RunWorkerAsync();
            }    
           else
            {
            G.Load.addMac = Decompile.GetMacAddress();
             
                if (G.Load.IsLockTrial)
                {
                    G.Load.IsLockTrial = false;


                    G.Load.FormActive.txtLicence.Text = "Locked Trial";

                }
                String ID = G.Load.addMac + "*" +Global.Config.IDCamera;
                G.Load.FormActive.KeyActive = Crypto.EncryptString128Bit(ID, "b@@");
                G.Load.FormActive.Show();

            }    
        }
        public void ConnectAll()
        {
           
            G.Load.FormActive.CheckActive(G.Load.addMac);
            if (G.IsActive)
            {
                if (!workConAll.IsBusy)
                    workConAll.RunWorkerAsync();
            }
            else
            {
                G.Load.addMac = Decompile.GetMacAddress();

                if (G.Load.IsLockTrial)
                {
                    G.Load.IsLockTrial = false;


                    G.Load.FormActive.txtLicence.Text = "Locked Trial";

                }
                String ID = G.Load.addMac + "*" +Global.Config.IDCamera;
                G.Load.FormActive.KeyActive = Crypto.EncryptString128Bit(ID, "b@@");
                G.Load.FormActive.Show();

            }
        }
        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
            if(G.EditTool!=null)
            G.EditTool.View.tmCheckCCD.Enabled = false;
            //BeeCore.Common.TypeCCD =Global.Configlobal.TypeCamera ;
            //if (Global.Configlobal.TypeCamera  == TypeCamera.TinyIV)
            //    BeeCore.Common.PropertyChanged -=G.EditTool.View. Common_PropertyChanged;
          
            btnConnect.Enabled = false;
           
            ConnectCCD();


        }

        private void work_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BeeCore.Common.listCamera.Count() > Global.IndexChoose)
                if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
                    BeeCore.Common.listCamera[Global.IndexChoose].IsConnected= BeeCore.Common.listCamera[Global.IndexChoose].Connect(BeeCore.Common.listCamera[Global.IndexChoose].Para.Name);
            //if (Global.Configlobal.TypeCamera  == TypeCamera.USB||Global.Configlobal.TypeCamera  == TypeCamera.BaslerGigE)
            //    BeeUi.G.IsCCD = BeeCore.Common.ConnectCCD(indexCCD,Global.Config.Resolution);

            //else if (Global.Configlobal.TypeCamera  == TypeCamera.TinyIV)
            //{
            //    BeeUi.G.IsCCD = true;
               
            //}
        }
        Crypto Crypto = new Crypto();
        private void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //String[] sp =Global.Config.Resolution.Split(' ');
            //String[] sp2 = sp[0].Split('x');
        
            //G.MainForm.Show();
            btnConnect.Enabled = true;
            if ( BeeCore.Common.listCamera.Count() > Global.IndexChoose)
                if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
                {
                    if (BeeCore.Common.listCamera[Global.IndexChoose].IsConnected)
                    {
                        if(G.Main==null)
                        {
                            Main Main = new Main();
                            G.EditTool.lbCam.Image = Properties.Resources.CameraConnected;
                            G.EditTool.lbCam.Text = "Camera Connected";

                            String sProgram = Properties.Settings.Default.programCurrent;
                            G.Load.lb.Text = "Loading program.. (" + sProgram + ")";
                            this.Hide();
                            G.Load.Hide();

                            Main.Show();
                        }
                        else
                            this.Hide();

                        // 

                    }
                    else
                    {

                        MessageBox.Show("Fail Connect");
                    }
                }
            else
                {
                    MessageBox.Show("Fail Connect");
                }
               
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
             
                if (G.Main != null)
            {
                this.Close();
            }
            else
            {  if (G.Load != null)
                G.Load.Close();
                //G.Main.Close();
               Process.GetCurrentProcess().Kill();
            }







        }

        private void ScanCCD_Load_1(object sender, EventArgs e)
        {
      
        }

        private void cbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
          //Global.Config.namePort = cbSerialPort.Text.Trim();
        }

        private void workScan_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void cbReSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Global.TypeCamera  == TypeCamera.TinyIV)
            //{
              
            //}
            Global.ParaCommon.CardChoosed = cbReSolution.Text;

            ScanIDCCD();
            //  else
            // Global.Config.Resolution = cbReSolution.Text.Trim();
        }

        private void btnGigE_Click(object sender, EventArgs e)
        {
            label1.Text = "Resolution";
            cbReSolution.Enabled = true;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }    
               
            // BeeCore.Common. = TypeCamera.BaslerGigE;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera = TypeCamera.BaslerGigE;
            ScanIDCCD();
        }

        private void btnUSB2_0_Click(object sender, EventArgs e)
        {
            label1.Text = "Resolution";
            cbReSolution.Enabled = true;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }
            BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera = TypeCamera.USB;
            ScanIDCCD();
        }

        private void pBTN_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnShowList_Click(object sender, EventArgs e)
        {
            cbCCD.DroppedDown = true;
        }

        private void btnCameraTiny_Click(object sender, EventArgs e)
        {
            label1.Text = "Choose Ehternet Card";
         //   cbReSolution.Enabled = false;
            cbReSolution.DataSource= HEROJE.ScanCard();
            if (cbReSolution.Items.Count== 1)
                cbReSolution.SelectedIndex = 0;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }
            BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera = TypeCamera.TinyIV;
            ScanIDCCD();
        }

        private void btnUSB3_0_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cbReSolution.DroppedDown = true;
        }

        private void btnCamera1_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 0;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }
            //BeeCore.Common.listCamera[0] = new Camera(Global.listParaCamera[0],0);
        }

        private void btnCamera2_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 1;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }
           // BeeCore.Common.listCamera[1] = new Camera(Global.listParaCamera[1], 1);
        }

        private void btnCamera3_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 2;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }
        //    BeeCore.Common.listCamera[2] = new Camera(Global.listParaCamera[2], Global.IndexChoose);
        }

        private void btnCamera4_Click(object sender, EventArgs e)
        {
            Global.IndexChoose = 3;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }
            //BeeCore.Common.listCamera[3] = new Camera(Global.listParaCamera[3],3);
        }

        private void workConAll_DoWork(object sender, DoWorkEventArgs e)
        {
            int indexCCD = 0;
         foreach (Camera camera in BeeCore.Common.listCamera)
            {
                if (camera != null)
                {
                    camera.Init();
                    camera.Scan();
                    camera.IsConnected = camera.Connect(camera.Para.Name);
                    indexCCD++;
                }
                    
            }
        }

        private void workConAll_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool IsConnect = false;
            foreach (Camera camera in BeeCore.Common.listCamera)
            {
                if (camera != null)
                    if (camera.IsConnected)
                    IsConnect = true;
               
                   

            }
            if (IsConnect)
            {

                SaveData.Camera(Global.Project,Global.listParaCamera);
               // Global.ParaCommon.Comunication.IO.Connect(Global.Config.IDPort);
               Global.Config.IDCamera = cbCCD.Text.Trim();
                // G.Load.FormActive.CheckActive(G.Load.addMac);
                if (G.Main == null)
                {
                    G.Load.FormActive.CheckActive(G.Load.addMac);
                    if (G.IsActive)
                    {
                        Main Main = new Main();
                        G.EditTool.lbCam.Image = Properties.Resources.CameraConnected;
                        G.EditTool.lbCam.Text = "Camera Connected";

                        String sProgram = Properties.Settings.Default.programCurrent;
                        G.Load.lb.Text = "Loading program.. (" + sProgram + ")";

                        G.Load.Hide();

                        Main.Show();
                        //  Main.WindowState = FormWindowState.Minimized;


                    }
                    else
                    {
                        if (G.Load.IsLockTrial)
                        {
                            G.Load.IsLockTrial = false;


                            G.Load.FormActive.txtLicence.Text = "Locked Trial";

                        }
                        String ID = G.Load.FormActive.KeyActive + "*" +Global.Config.IDCamera;
                        G.Load.FormActive.KeyActive = Crypto.EncryptString128Bit(ID, "b@@");
                        G.Load.FormActive.Show();

                    }
                }
                else
                {
                    this.Hide();
                    G.Main.Show();

                    if (G.IsReConnectCCD)
                    {
                        G.IsReConnectCCD = false;
                        G.Header.tmReadPLC.Enabled = true;
                    }

                    BeeCore.Common.listCamera[Global.IndexChoose].Read();


                }
            }
            else
            {

                if (G.Load != null)
                    G.Load.Hide();
                this.Show();
            }



        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].DisConnect();
            BeeCore.Common.listCamera[Global.IndexChoose] = null;
           Global.listParaCamera[Global.IndexChoose] = null;
            Global.IndexChoose--;
            SaveData.Camera(Global.Project,Global.listParaCamera);

            if (Global.IndexChoose < 0) Global.IndexChoose = 0;
            Global.ToolSettings.pAllTool.Controls.Clear();
            if (G.Header == null)
            {
                return;
                this.Close();
            }
            G.Header.stepShow = 0;
            G.Header.indexToolShow = 0;
            G.Header.tmShow.Enabled = true;
            
        }
    }
}
