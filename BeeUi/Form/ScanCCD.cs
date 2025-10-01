using BeeCore;
using BeeGlobal;
using BeeUi.Commons;

using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;

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
using Camera = BeeCore.Camera;

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

        public List<string> ScanIDCCD(TypeCamera typeCamera)
        {
            cbCCD.Text = "Waiting Scan";
     
            List<string> listStringCCD = new List<string>();
            if (BeeCore.Common.listCamera.Count() > Global.IndexChoose)
                if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
            {
                BeeCore.Common.listCamera[Global.IndexChoose].Init(typeCamera);
                    listStringCCD = BeeCore.Common.listCamera[Global.IndexChoose].Scan(typeCamera).ToList();
            }
                else
                {
                    return new List<string>();
                }


            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera == TypeCamera.USB)
            {
                int index = Array.FindIndex(listStringCCD.ToArray(), s => s.Contains(BeeCore.Common.listCamera[Global.IndexChoose].Para.Name));
                if (index != -1)
                    indexCCD = index;
            }
            cbCCD.DataSource = listStringCCD;
            if (listStringCCD.Count()==0)
            { cbCCD.Text = "No Camera";
                btnConnect.Enabled = false;
            }
              
            else
                btnConnect.Enabled = true;
            return listStringCCD;
        }
        private void ScanCCD_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
         
            if (Global.IndexChoose >= 0)
            {
                if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
                    BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(new ParaCamera(), 0);
                switch (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera)
                {
                    case TypeCamera.USB:
                        btnUSB2_0.IsCLick = true;
                        break;
                    case TypeCamera.MVS:
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
        {if (cbCCD.SelectedValue == null) return;
            indexCCD = cbCCD.SelectedIndex;
           BeeCore.Common.listCamera[Global.IndexChoose].Para.Name = cbCCD.SelectedValue.ToString();
            BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera = TypeCameraNew;
            if (Global.listParaCamera[Global.IndexChoose] == null)
                Global.listParaCamera[Global.IndexChoose] = BeeCore.Common.listCamera[Global.IndexChoose].Para;
          
           // Global.Config.IDCamera = BeeCore.Common.listCamera[Global.IndexChoose].Para.Name;
            G.Load.FormActive.CheckActive(G.Load.addMac);
            if(G.IsActive)
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
                String ID = G.Load.addMac + "*" + BeeCore.Common.listCamera[Global.IndexChoose].Para.Name;
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
                String ID = G.Load.addMac + "*" + BeeCore.Common.listCamera[Global.IndexChoose].Para.Name;
                G.Load.FormActive.KeyActive = Crypto.EncryptString128Bit(ID, "b@@");
                G.Load.FormActive.Show();

            }
        }

        private  void work_DoWork(object sender, DoWorkEventArgs e)
        {
             //if (Global.Configlobal.TypeCamera  == TypeCamera.USB||Global.Configlobal.TypeCamera  == TypeCamera.MVS)
            //    BeeUi.G.IsCCD = BeeCore.Common.ConnectCCD(indexCCD,Global.Config.Resolution);

            //else if (Global.Configlobal.TypeCamera  == TypeCamera.TinyIV)
            //{
            //    BeeUi.G.IsCCD = true;
               
            //}
        }
        Crypto Crypto = new Crypto();
        private async void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            if (BeeCore.Common.listCamera.Count() > Global.IndexChoose)
                if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
                {
                    if (BeeCore.Common.listCamera[Global.IndexChoose].IsConnected)
                    {
                        BeeCore.Common.listCamera[Global.IndexChoose].DisConnect(BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera);
                        await Task.Delay(1000);
                    }
                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = new OpenCvSharp.Mat();
                    BeeCore.Common.listCamera[Global.IndexChoose].IndexConnect = indexCCD;
                        BeeCore.Common.listCamera[Global.IndexChoose].IsConnected = await BeeCore.Common.listCamera[Global.IndexChoose].Connect(BeeCore.Common.listCamera[Global.IndexChoose].Para.Name, BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera);

                    
                }    
                   
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
                            Global.EditTool.lbCam.Image = Properties.Resources.CameraConnected;
                            Global.EditTool.lbCam.Text = "Camera Connected";

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
            Global.IsChange = false;
            if (G.Main != null)
            {
              G.Main.Show();
              this.Hide();
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

            //ScanIDCCD();
            //  else
            // Global.Config.Resolution = cbReSolution.Text.Trim();
        }
     public   TypeCamera TypeCameraNew= new TypeCamera();
        private void btnGigE_Click(object sender, EventArgs e)
        {
            TypeCameraNew = TypeCamera.MVS;
            //label1.Text = "Resolution";
            cbReSolution.Enabled = true;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }    
               
            // BeeCore.Common. = TypeCamera.MVS;
           // BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera = TypeCamera.MVS;
            ScanIDCCD(TypeCameraNew);
        }

        private void btnUSB2_0_Click(object sender, EventArgs e)
        {
            TypeCameraNew = TypeCamera.USB;
            //label1.Text = "Resolution";
            cbReSolution.Enabled = true;
            if (Global.listParaCamera[Global.IndexChoose] == null)
            {
               Global.listParaCamera[Global.IndexChoose] = new ParaCamera();
                BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(Global.listParaCamera[Global.IndexChoose], Global.IndexChoose);

            }
           // BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera = TypeCamera.USB;
            ScanIDCCD(TypeCameraNew);
        }

        private void pBTN_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

       

        private void btnCameraTiny_Click(object sender, EventArgs e)
        {
            //label1.Text = "Choose Ehternet Card";
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
            ScanIDCCD(TypeCameraNew);
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
        String sRead = "";
        int NumNoneNull = 0;
     public   List<String> ListCamUSB = new List<string>();
        private async  void workConAll_DoWork(object sender, DoWorkEventArgs e)
        {
            NumNoneNull = 0;
            foreach (Camera camera in BeeCore.Common.listCamera)
            {
                if (camera != null)
                {
                    NumNoneNull++;
                    if (camera.IsConnected)
                    {
                        camera.DisConnect(camera.Para.TypeCamera);

                        camera.matRaw = new OpenCvSharp.Mat();
                    }
                    if (camera.Para.TypeCamera == TypeCamera.USB)
                    {
                        indexCCD = -1;
                        camera.Init(camera.Para.TypeCamera);
                        int index = Array.FindIndex(ListCamUSB.ToArray(), s => s.Contains(camera.Para.Name));
                        if (index != -1)
                            indexCCD = index;
                    }
                    else
                    {
                        camera.Init(camera.Para.TypeCamera);
                       // List<String> listStringCCD = camera.Scan(camera.Para.TypeCamera).ToList();
                    }    
                       
                   
                  
                    if (indexCCD != -1| camera.Para.TypeCamera != TypeCamera.USB)
                    {
                        camera.IndexConnect = indexCCD;
                        camera.matRaw = new OpenCvSharp.Mat();
                        camera.IsConnected = await camera.Connect(camera.Para.Name, camera.Para.TypeCamera);
                        if (camera.IsConnected)
                        {
                            
                                await camera.SetFullPara();
                        }

                    }
                }
                if (Global.ParaCommon.IsMultiCamera == false)
                    break;
            }


        }
        public void DisConnectAllCCd()
        {
            foreach (Camera camera in BeeCore.Common.listCamera)
            {
                if (camera != null)
                {
                    if (camera.IsConnected)
                    {
                        camera.DisConnect(camera.Para.TypeCamera);

                        camera.matRaw = new OpenCvSharp.Mat();
                    }
                
                 
                    
                }
              
            }
        }
        public async Task< bool > ChangeCCD()
        {
            bool IsConnect = true;
            
            foreach (Camera camera in BeeCore.Common.listCamera)
            {
                if (camera != null)
                {
                    if (camera.IsConnected)
                    {
                        camera.DisConnect(camera.Para.TypeCamera);

                        camera.matRaw = new OpenCvSharp.Mat();
                    }
                    camera.Init(camera.Para.TypeCamera);
                    
                    String[] listStringCCD = camera.Scan(camera.Para.TypeCamera);
                    if (camera.Para.TypeCamera == TypeCamera.USB)
                    {
                        int index = Array.FindIndex(listStringCCD, s => s.Contains(camera.Para.Name));
                        if (index != -1)
                            indexCCD = index;
                    }
                   
                    if (indexCCD != -1 | camera.Para.TypeCamera != TypeCamera.USB)
                    {
                        camera.IndexConnect = indexCCD;
                        camera.matRaw = new OpenCvSharp.Mat();
                        camera.IsConnected = await camera.Connect(camera.Para.Name, camera.Para.TypeCamera);
                        if (camera.IsConnected)
                        {

                            await camera.SetFullPara();
                        }
                    }
                }
                if (Global.ParaCommon.IsMultiCamera == false)
                    break;
            }
            int numNull = 0;
            foreach (Camera camera in BeeCore.Common.listCamera)
            {
                if (camera != null)
                {
                    if (!camera.IsConnected)
                        IsConnect = false;
                }
                else
                    numNull++;



                if (Global.ParaCommon.IsMultiCamera == false)
                    break;


            }
            if (numNull == 4) IsConnect = false;
            return IsConnect;
        }

        private async  void workConAll_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          

                bool IsConnect = true;
            int numNull = 0;
            foreach (Camera camera in BeeCore.Common.listCamera)
            {
                if (camera != null)
                {
                    if (!camera.IsConnected)
                        IsConnect = false;
                }
                else 
                    numNull++;

            }
         
            if (numNull == 4) IsConnect = false;
            if (IsConnect)
            {
                if (NumNoneNull == 1)
                {
                    Global.ParaCommon.IsMultiCamera = false;
                }
                else
                {
                    Global.ParaCommon.IsMultiCamera = true;

                }
                if (Global.ParaCommon.NumTrig < 1) Global.ParaCommon.NumTrig = 1;
                Global.CameraStatus = CameraStatus.Ready;
                SaveData.Camera(Global.Project,Global.listParaCamera);
               
                if (G.Main == null)
                {
                    G.Load.FormActive.CheckActive(G.Load.addMac);
                    if (G.IsActive)
                    {
                         Main Main = new Main();
                        Global.EditTool.lbCam.Image = Properties.Resources.CameraConnected;
                        Global.EditTool.lbCam.Text = "Camera Connected";

                        String sProgram = Properties.Settings.Default.programCurrent;
                        G.Load.lb.Text = "Loading program.. (" + sProgram + ")";

                        G.Load.Hide();

                        Main.Show();
                        this.Hide();
                        //  Main.WindowState = FormWindowState.Minimized;


                    }
                    else
                    {
                        if (G.Load.IsLockTrial)
                        {
                            G.Load.IsLockTrial = false;


                            G.Load.FormActive.txtLicence.Text = "Locked Trial";

                        }
                        String ID = G.Load.FormActive.KeyActive + "*" + BeeCore.Common.listCamera[Global.IndexChoose].Para.Name;
                        G.Load.FormActive.KeyActive = Crypto.EncryptString128Bit(ID, "b@@");
                        G.Load.FormActive.Show();

                    }
                }
                else
                {
                    this.Hide();
                    G.Main.Show();

                    //if (G.IsReConnectCCD)
                    //{
                    //    G.IsReConnectCCD = false;
                    //    G.Header.tmReadPLC.Enabled = true;
                    //}

                    BeeCore.Common.listCamera[Global.IndexChoose].Read();


                }
            }
            else
            {
                MessageBox.Show("Camera connect Fail "+ Global.Ex);
                btnConnect.Enabled = true;
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
            BeeCore.Common.listCamera[Global.IndexChoose].DisConnect(BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera);
            BeeCore.Common.listCamera[Global.IndexChoose] = null;
           Global.listParaCamera[Global.IndexChoose] = null;
            Global.IndexChoose--;
            SaveData.Camera(Global.Project,Global.listParaCamera);

            if (Global.IndexChoose < 0) Global.IndexChoose = 0;
            if (Global.ToolSettings!=null)
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

        private void btnPylon_Click(object sender, EventArgs e)
        {
            TypeCameraNew = TypeCamera.Pylon;
           // BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera = TypeCamera.Pylon;
            ScanIDCCD(TypeCameraNew);
        }

        private  async void btnConnect_Click(object sender, EventArgs e)
        {
            int index = 0;
            foreach (Camera camera in BeeCore.Common.listCamera)
            {
                if (camera != null)
                {
                    if (camera.IsConnected)
                    {
                        camera.DisConnect(camera.Para.TypeCamera);

                        camera.matRaw = new OpenCvSharp.Mat();
                    }


                    index++;
                }
                if (Global.ParaCommon.IsMultiCamera == false)
                    break;
            }
         
          
            btnConnect.Enabled = false;
            if (!Global.IsChange)
                ConnectCCD();
            else
            {
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Name = cbCCD.SelectedValue.ToString();

                BeeCore.Common.listCamera[Global.IndexChoose].Init(TypeCameraNew);
               
                String[] listStringCCD = BeeCore.Common.listCamera[Global.IndexChoose].Scan(TypeCameraNew);
                if (TypeCameraNew == TypeCamera.USB)
                {
                    int index2 = Array.FindIndex(listStringCCD, s => s.Contains(BeeCore.Common.listCamera[Global.IndexChoose].Para.Name));
                    if (index2 != -1)
                        indexCCD = index2;
                }
                BeeCore.Common.listCamera[Global.IndexChoose].IndexConnect = indexCCD;
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = new OpenCvSharp.Mat();
                BeeCore.Common.listCamera[Global.IndexChoose].IsConnected = await BeeCore.Common.listCamera[Global.IndexChoose].Connect(BeeCore.Common.listCamera[Global.IndexChoose].Para.Name,TypeCameraNew);
                if (BeeCore.Common.listCamera[Global.IndexChoose].IsConnected)
               {
                    BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera = TypeCameraNew;
                    if (Global.listParaCamera[Global.IndexChoose] == null)
                        Global.listParaCamera[Global.IndexChoose] = BeeCore.Common.listCamera[Global.IndexChoose].Para;

                    Global.CameraStatus = CameraStatus.Ready;
                    SaveData.Camera(Global.Project, Global.listParaCamera);
                   // Global.Config.IDCamera = cbCCD.Text.Trim();

                        this.Hide();
                    Global.IsChange = false;
                }
                 else
                    {
                        MessageBox.Show("Camera connect Fail" + TypeCameraNew.ToString() + Global.Ex);
                        btnConnect.Enabled = true;
                       
                        this.Show();
                    }
                }    
            

        }

        private void ScanCCD_Shown(object sender, EventArgs e)
        {
            if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
            {
                lbCamera.Text = "NULL";

            }
            else
            {
                lbCamera.Text = BeeCore.Common.listCamera[Global.IndexChoose].Para.Name;
                TypeCamera typeCamera = BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera;
                switch (typeCamera)
                {
                    case TypeCamera.USB:
                        btnUSB2_0.IsCLick = true;
                        break;
                    case TypeCamera.MVS:
                        btnGigE.IsCLick = true;
                        break;
                    case TypeCamera.Pylon:
                        btnPylon.IsCLick = true;
                        break;
                }
            }
         
        }
        String NameCCDChoose;
        private void cbCCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            NameCCDChoose = cbCCD.SelectedValue.ToString();
            lbCamera.Text = NameCCDChoose;
        }
    }
}
