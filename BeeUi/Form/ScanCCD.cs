using BeeCore;
using BeeUi.Commons;
using BeeUi.Data;
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
            String IDCCD = G.Config.IDCamera;
            // BeeCore.Common.listCamera.Add(new Camera());
            String sRead = "";
            if (BeeCore.Common.listCamera.Count() > G.indexChoose)
                if (BeeCore.Common.listCamera[G.indexChoose] != null)
            {
                BeeCore.Common.listCamera[G.indexChoose].Init();
                 sRead = BeeCore.Common.listCamera[G.indexChoose].Scan();
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

          
         //   cbCCD.DataSource = ScanIDCCD();
            //if (G.Config.Resolution == null) G.Config.Resolution = "1280x720 (1.3 MP)";
            //cbReSolution.SelectedIndex = cbReSolution.FindStringExact(G.Config.Resolution);
        }
     public   int indexCCD;
        
        public void ConnectCCD()
        {
            indexCCD = cbCCD.SelectedIndex;
           BeeCore.Common.listCamera[G.indexChoose].Para.Name = cbCCD.Text;
            G.Config.IDCamera = cbCCD.Text.Trim();
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
                String ID = G.Load.addMac + "*" + G.Config.IDCamera;
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
                String ID = G.Load.addMac + "*" + G.Config.IDCamera;
                G.Load.FormActive.KeyActive = Crypto.EncryptString128Bit(ID, "b@@");
                G.Load.FormActive.Show();

            }
        }
        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
            if(G.EditTool!=null)
            G.EditTool.View.tmCheckCCD.Enabled = false;
            //BeeCore.Common.TypeCCD = G.Config.TypeCamera;
            //if (G.Config.TypeCamera == BeeCore.TypeCamera.TinyIV)
            //    BeeCore.Common.PropertyChanged -=G.EditTool.View. Common_PropertyChanged;
          
            btnConnect.Enabled = false;
           
            ConnectCCD();


        }

        private void work_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BeeCore.Common.listCamera.Count() > G.indexChoose)
                if (BeeCore.Common.listCamera[G.indexChoose] != null)
                    BeeCore.Common.listCamera[G.indexChoose].IsConnected= BeeCore.Common.listCamera[G.indexChoose].Connect(BeeCore.Common.listCamera[G.indexChoose].Para.Name);
            //if (G.Config.TypeCamera == BeeCore.TypeCamera.USB|| G.Config.TypeCamera == BeeCore.TypeCamera.BaslerGigE)
            //    BeeUi.G.IsCCD = BeeCore.Common.ConnectCCD(indexCCD, G.Config.Resolution);

            //else if (G.Config.TypeCamera == BeeCore.TypeCamera.TinyIV)
            //{
            //    BeeUi.G.IsCCD = true;
               
            //}
        }
        Crypto Crypto = new Crypto();
        private void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //String[] sp = G.Config.Resolution.Split(' ');
            //String[] sp2 = sp[0].Split('x');
        
            //G.MainForm.Show();
            btnConnect.Enabled = true;
            if ( BeeCore.Common.listCamera.Count() > G.indexChoose)
                if (BeeCore.Common.listCamera[G.indexChoose] != null)
                {
                    if (BeeCore.Common.listCamera[G.indexChoose].IsConnected)
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
                //if (G.Load != null)
                //    G.Load.Close();
                //if (G.Main != null)
                //    G.Main.Close();
                this.Close();
               // Process.GetCurrentProcess().Kill();
               
                
           
         
        }

        private void ScanCCD_Load_1(object sender, EventArgs e)
        {
      
        }

        private void cbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
          //G.Config.namePort = cbSerialPort.Text.Trim();
        }

        private void workScan_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void cbReSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (G.TypeCamera == TypeCamera.TinyIV)
            //{
              
            //}
            BeeCore.G.ParaCam.CardChoosed = cbReSolution.Text;

            ScanIDCCD();
            //  else
            //  G.Config.Resolution = cbReSolution.Text.Trim();
        }

        private void btnGigE_Click(object sender, EventArgs e)
        {
            label1.Text = "Resolution";
            cbReSolution.Enabled = true;
            if (BeeCore.Common.listParaCamera[G.indexChoose] == null)
            {
                BeeCore.Common.listParaCamera[G.indexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[G.indexChoose] = new Camera(BeeCore.Common.listParaCamera[G.indexChoose], G.indexChoose);

            }    
               
            // BeeCore.Common. = BeeCore.TypeCamera.BaslerGigE;
            BeeCore.Common.listCamera[G.indexChoose].Para.TypeCamera = BeeCore.TypeCamera.BaslerGigE;
            ScanIDCCD();
        }

        private void btnUSB2_0_Click(object sender, EventArgs e)
        {
            label1.Text = "Resolution";
            cbReSolution.Enabled = true;
            if (BeeCore.Common.listParaCamera[G.indexChoose] == null)
            {
                BeeCore.Common.listParaCamera[G.indexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[G.indexChoose] = new Camera(BeeCore.Common.listParaCamera[G.indexChoose], G.indexChoose);

            }
            BeeCore.Common.listCamera[G.indexChoose].Para.TypeCamera = BeeCore.TypeCamera.USB;
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
            if (BeeCore.Common.listParaCamera[G.indexChoose] == null)
            {
                BeeCore.Common.listParaCamera[G.indexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[G.indexChoose] = new Camera(BeeCore.Common.listParaCamera[G.indexChoose], G.indexChoose);

            }
            BeeCore.Common.listCamera[G.indexChoose].Para.TypeCamera = BeeCore.TypeCamera.TinyIV;
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
            G.indexChoose = 0;
            if (BeeCore.Common.listParaCamera[G.indexChoose] == null)
            {
                BeeCore.Common.listParaCamera[G.indexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[G.indexChoose] = new Camera(BeeCore.Common.listParaCamera[G.indexChoose], G.indexChoose);

            }
            //BeeCore.Common.listCamera[0] = new Camera(BeeCore.Common.listParaCamera[0],0);
        }

        private void btnCamera2_Click(object sender, EventArgs e)
        {
            G.indexChoose = 1;
            if (BeeCore.Common.listParaCamera[G.indexChoose] == null)
            {
                BeeCore.Common.listParaCamera[G.indexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[G.indexChoose] = new Camera(BeeCore.Common.listParaCamera[G.indexChoose], G.indexChoose);

            }
           // BeeCore.Common.listCamera[1] = new Camera(BeeCore.Common.listParaCamera[1], 1);
        }

        private void btnCamera3_Click(object sender, EventArgs e)
        {
            G.indexChoose = 2;
            if (BeeCore.Common.listParaCamera[G.indexChoose] == null)
            {
                BeeCore.Common.listParaCamera[G.indexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[G.indexChoose] = new Camera(BeeCore.Common.listParaCamera[G.indexChoose], G.indexChoose);

            }
        //    BeeCore.Common.listCamera[2] = new Camera(BeeCore.Common.listParaCamera[2], G.indexChoose);
        }

        private void btnCamera4_Click(object sender, EventArgs e)
        {
            G.indexChoose = 3;
            if (BeeCore.Common.listParaCamera[G.indexChoose] == null)
            {
                BeeCore.Common.listParaCamera[G.indexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[G.indexChoose] = new Camera(BeeCore.Common.listParaCamera[G.indexChoose], G.indexChoose);

            }
            //BeeCore.Common.listCamera[3] = new Camera(BeeCore.Common.listParaCamera[3],3);
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

                SaveData.Camera(G.Project, BeeCore.Common.listParaCamera);
               // G.PLC.Connect(G.Config.IDPort);
                G.Config.IDCamera = cbCCD.Text.Trim();
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
                        String ID = G.Load.FormActive.KeyActive + "*" + G.Config.IDCamera;
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

                    BeeCore.Common.listCamera[G.indexChoose].Read();


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
            BeeCore.Common.listCamera[G.indexChoose].DisConnect();
            BeeCore.Common.listCamera[G.indexChoose] = null;
            BeeCore.Common.listParaCamera[G.indexChoose] = null;
            G.indexChoose--;
            Data.SaveData.Camera(G.Project, BeeCore.Common.listParaCamera);

            if (G.indexChoose < 0) G.indexChoose = 0;
            G.ToolSettings.pAllTool.Controls.Clear();
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
