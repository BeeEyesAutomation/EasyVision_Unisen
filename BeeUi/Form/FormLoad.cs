
using BeeCore;
using BeeCore.EtherNetIP;
using BeeGlobal;
using BeeInterface;
using BeeUi.Commons;
using BeeUi.Unit;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;

using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class FormLoad : Form
    {
        public String addMac;
        public String decompile;
        public bool IsLockTrial = false;
        public String[] sKeys;
        public String sKey="";
        public List<String> drivers;
        public Timer tmActive = new Timer();
        public Timer tmLoad = new Timer();
        public BackgroundWorker wLoad = new BackgroundWorker();
        Access access=new Access();
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // progressBar.Value = e.ProgressPercentage;
        }
        bool Is64bit = false;
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {

        }
      
       
        public FormLoad()
        {
            InitializeComponent();
           this.Region = System.Drawing.Region.FromHrgn(Draws.CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
          //  EDS.GenerateEDS("Visionsensor.eds");
            //  Disable();
            //  Enable();

            // listStringCCD;

            // RsDirPermissions rsDirPermissions = new RsDirPermissions();
            // rsDirPermissions.SetEveryoneAccess(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)+"\\Bee Eyes Automation\\EasyVision");
            G.Load = this;
           
            tmActive.Interval = 1000;
            tmLoad.Interval = 1000;
            tmActive.Tick += TmActive_Tick;
            tmLoad.Tick += TmLoad_Tick;
            wLoad.DoWork += WLoad_DoWork;
            wLoad.RunWorkerCompleted += WLoad_RunWorkerCompleted;

            try
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
              
                lbVersion.Text = version.ToString();
              
            }
            catch (Exception ex)  //just for demonstration...it's always best to handle specific exceptions
            {
                //react appropriately
            }


        }

        private void TmScanCCD_Tick(object sender, EventArgs e)
        {
            //if(File.Exists("cameras.bee"))
            //  {
            //    G.listCCD = File.ReadAllLines("cameras.bee").ToList();
            //G.listCCD = G.listCCD.Distinct().ToList();
            //tmG.ScanCCD.Enabled = false;
            //}
        }
        public FormActive FormActive =new FormActive();
        Crypto Crypto=new Crypto();
        
        private void WLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Global.ScanCCD.cbCCD.SelectedIndex == -1)

            {
                String NameCamera = BeeCore.Common.listCamera[Global.IndexChoose].Para.Name.Split('$')[0];
                MessageBox.Show("Connect Failed Camera" + NameCamera + "!");
                Global.ScanCCD.Show();
                return;
            }    
                if (G.Load.IsLockTrial)
            {
                lbActive.Text = sActive;
                
            }    
                if (G.Load.sKey == null) 
                G.Load.sKey = "";
                if (G.Load.sKey != "")
            {
                if (File.Exists("Keys.bee"))
                    File.Delete("Keys.bee");
                access.SaveKeys(Crypto.EncryptString128Bit(G.Load.sKey, ""), "Keys.bee");
            }
            FormActive.KeyActive= addMac;
           
            if (G.IsActive)
            {
                lb.Text = "Scan Camera Complete";
                Global.ScanCCD.ConnectAll();
             

            }
            else
            {
                tmActive.Enabled = true;

            }
            lbActive.Text = sActive;
        }
        public String sActive = "";

        private void WLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                //8521-0A34-5A8B-B41E-19E6-B375-48FF-92F1 -Pro1
                addMac = Decompile.GetMacAddress();
              


                FormActive.CheckActive(addMac);
                if (G.IsActive)
                {
                    if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Name != null)
                        if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Name != "")
                        {
                            if (Global.ScanCCD == null) Global.ScanCCD = new ScanCCD();

                            int indexCCD = listCCD.FindIndex(a => a.Contains(BeeCore.Common.listCamera[Global.IndexChoose].Para.Name));
                            // G.ScanCCD.cbReSolution.SelectedIndex = G.ScanCCD.cbReSolution.FindStringExact(Global.Config.Resolution);

                            if (indexCCD != -1)
                                Global.ScanCCD.cbCCD.SelectedIndex = indexCCD;
                            else
                            {
                                Global.ScanCCD.cbCCD.SelectedIndex = -1;
                            }




                        }
                }
              

            }
            catch (Exception)
            {

            }
        }
        List<String> listCCD;

        private void TmLoad_Tick(object sender, EventArgs e)
        {
            Global.Project =Properties.Settings.Default.programCurrent.Replace(".prog", "");
           
            if (File.Exists("Default.config"))
               Global.Config = Access.LoadConfig("Default.config");
            else
               Global.Config = new Config();
            Global.ParaCommon = LoadData.Para(Global.Project);
            if (Global.Config.RoundRad == 0)Global.Config.RoundRad = 10;
            tmLoad.Enabled = false;
            //
           
            Global.SizeScreen= Screen.PrimaryScreen.Bounds.Size;
            Global.PerScaleWidth = (float)((Global.SizeScreen.Width * 1.0)/ 2240.0 );
            Global.PerScaleHeight = (float)((Global.SizeScreen.Height * 1.0)/ 1400.0 );
            lb.Text = "Waiting Initial Learning AI";
            workIniModel.RunWorkerAsync();
          
        }

        private void TmActive_Tick(object sender, EventArgs e)
        {
            this.Hide();
         
          
           if(Global.ScanCCD==null)
                Global.ScanCCD  = new ScanCCD();
            Global.ScanCCD.Show();
         //   FormActive.Show();
            tmActive.Enabled = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FormLoad_Load(object sender, EventArgs e)
        {

            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
         
            tmLoad.Interval =500;
            tmLoad.Enabled = true;


        }

        private void workIniModel_DoWork(object sender, DoWorkEventArgs e)
        {
            BeeCore.Common.IniPython();

        }

        private void workIniModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Global.Project = Properties.Settings.Default.programCurrent;

            DataTool.LoadProject(Global.Project);
            foreach (List<PropetyTool> ListTool in BeeCore.Common.PropetyTools)
            {
                if (ListTool == null) continue;
                Parallel.For(0, ListTool.Count, i =>
            {
                PropetyTool propety = ListTool[i];

            X: if (propety.StatusTool == StatusTool.NotInitial)
                {

                    goto X;
                }

            });
            }
            Global.ScanCCD = new ScanCCD();
                G.IsIniPython = true;
            lb.Text = "Initial Learning AI Complete";
            Task.Delay(200);
            listCCD = Global.ScanCCD.ScanIDCCD();
            addMac = Decompile.GetMacAddress();



            FormActive.CheckActive(addMac);
            Global.ScanCCD.workConAll.RunWorkerAsync();

        }
    }
}
