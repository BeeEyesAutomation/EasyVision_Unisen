using BeeCore;
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
            String IDCCD = G.Config.IDCamera;
           
            BeeCore.Funtion.Init.CCD(G.Config.TypeCamera);
            
            // G.HEROJE=new DeviceFindAndCom()
            String sRead = BeeCore.Camera.Scan();
      
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

          
            cbCCD.DataSource = ScanIDCCD();
            if (G.Config.Resolution == null) G.Config.Resolution = "1280x720 (1.3 MP)";
            cbReSolution.SelectedIndex = cbReSolution.FindStringExact(G.Config.Resolution);
        }
     public   int indexCCD;
        
        public void ConnectCCD()
        {
            indexCCD = cbCCD.SelectedIndex;
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
            BeeCore.Camera.IsConnected= BeeCore.Camera.Connect(indexCCD, G.Config.Resolution);
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
            String[] sp = G.Config.Resolution.Split(' ');
            String[] sp2 = sp[0].Split('x');
        
            //G.MainForm.Show();
            btnConnect.Enabled = true;
            if (BeeCore.Camera.IsConnected)
            {
                if (File.Exists("Default.config"))
                    File.Delete("Default.config");
                Access.SaveConfig("Default.config", G.Config);
                G.PLC.Connect(G.Config.IDPort);
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
                        String ID=    G.Load.FormActive.KeyActive + "*"+G.Config.IDCamera;
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
                      
                        BeeCore.Camera.Read();
                    
                   
                }
              
              
                    this.Hide();
                // 

            }
            else
            {
              
                if (G.Load!=null)
                G.Load.Hide();
                this.Show();
            }    
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
                if (G.Load != null)
                    G.Load.Close();
                if (G.Main != null)
                    G.Main.Close();
                this.Close();
                Process.GetCurrentProcess().Kill();
               
                
           
         
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
            if (G.Config.TypeCamera == TypeCamera.TinyIV)
            {
                BeeCore.G.ParaCam.CardChoosed = cbReSolution.Text;
              
                ScanIDCCD();
            }
             
            else
            G.Config.Resolution = cbReSolution.Text.Trim();
        }

        private void btnGigE_Click(object sender, EventArgs e)
        {
            label1.Text = "Resolution";
            cbReSolution.Enabled = true;
            // BeeCore.Common. = BeeCore.TypeCamera.BaslerGigE;
            G.Config.TypeCamera = BeeCore.TypeCamera.BaslerGigE;
            ScanIDCCD();
        }

        private void btnUSB2_0_Click(object sender, EventArgs e)
        {
            label1.Text = "Resolution";
            cbReSolution.Enabled = true;
            G.Config.TypeCamera = BeeCore.TypeCamera.USB;
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
            G.Config.TypeCamera=BeeCore.TypeCamera.TinyIV;
            ScanIDCCD();
        }

        private void btnUSB3_0_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cbReSolution.DroppedDown = true;
        }
    }
}
