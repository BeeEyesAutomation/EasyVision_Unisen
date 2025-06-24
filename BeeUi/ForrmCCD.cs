using LibSharp.Filter;
using LibSharp.Gui;
using Microsoft.Win32;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace LibSharp
{
    public partial class ForrmCCD : Form
    {
        public String addMac;
        public String decompile;
        public FormBig FormBig;
        public FormActive FormActive =new FormActive();
        public Access access = new Access();
        public String[] sKeys;
        public String sKey;
        public List<String> drivers;
        public Timer tmActive = new Timer();
        public Timer tmLoad = new Timer();
        public BackgroundWorker wLoad = new BackgroundWorker();
    
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
           // progressBar.Value = e.ProgressPercentage;
        }
        bool Is64bit = false;
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            
        }
    
        Timer tmScanCCD = new Timer();
        public ForrmCCD()
        {
            InitializeComponent();
           

        }

        private void TmScanCCD_Tick(object sender, EventArgs e)
        {
            if(File.Exists("cameras.bee"))
              {
                G.listCCD = File.ReadAllLines("cameras.bee").ToList();
            G.listCCD = G.listCCD.Distinct().ToList();
            tmScanCCD.Enabled = false;
            }
        }

        private void WLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }
        String sActive = "";
        private void WLoad_DoWork(object sender, DoWorkEventArgs e)
        {
           
        }

        private void TmLoad_Tick(object sender, EventArgs e)
        {
            tmLoad.Enabled = false;
           
            wLoad.RunWorkerAsync();

        }

        private void TmActive_Tick(object sender, EventArgs e)
        {
            this.Hide();

            FormActive.Show();
            tmActive.Enabled = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FormLoad_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);



        }
        Timer tmColor = new Timer();
        private void btnRestartPro_Click(object sender, EventArgs e)
        {
            Unit unit = G.elps[cbCCD.SelectedIndex].unit;
            if (unit.node[3].img[0] == null) unit.node[3].img[0] = new Mat();
            unit.node[3].img[0].Empty();
            if (unit.node[1].CameraUsb[0].IsOpened())
            {

                try
                {
                    unit.node[1].CameraUsb[0].Read(unit.node[3].img[0]);

                    unit.node[1].CameraUsb[0].Read(unit.node[3].img[0]);
                    if (unit.node[3].img[0].Norm() == 0)
                    {
                        btnTest.BackColor = Color.Red;
                    }
                    else
                    {
                        btnTest.BackColor = Color.Lime;
                        if (unit.picture!=null)
                        unit.picture.ImageIpl = unit.node[3].img[0];
                    }    
                }
                catch (Exception)
                {
                    btnTest.BackColor = Color.Red;

                }
            }
            else
            {
                try
                {
                    int indexCCD = G.listCCD.FindIndex(a => a.Contains(unit.para[0].path));
                    if (indexCCD >= 0)
                    {

                        unit.node[1].CameraUsb[0].Open(indexCCD);
                        unit.node[1].CameraUsb[0].Read(unit.node[3].img[0]);
                        unit.node[1].CameraUsb[0].Read(unit.node[3].img[0]);
                        if (unit.node[3].img[0].Norm() == 0)
                        {
                            btnTest.BackColor = Color.Red;
                        }
                        else
                        {
                            btnTest.BackColor = Color.Lime;
                            if (unit.picture != null)
                                unit.picture.ImageIpl = unit.node[3].img[0];
                        }
                    }
                    else
                    {
                        btnTest.BackColor = Color.Red;
                    }    
                }
                catch(Exception)
                {

                }
            }
            tmColor.Interval = 1000;
            tmColor.Tick += TmColor_Tick;
            tmColor.Enabled = true;
        }

        private void TmColor_Tick(object sender, EventArgs e)
        {
            tmColor.Enabled = false;
            btnTest.BackColor = Color.LightGray;
        }

        private static void StartShutDown(string param)
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "cmd";
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Arguments = "/C shutdown " + param;
            Process.Start(proc);
        }
        public  void Restart()
        {
            StartShutDown("-f -r -t 5");
        }
        private void btnRestartPC_Click(object sender, EventArgs e)
        {
           
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn Restart PC ?", "Restart PC", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Restart();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
         
           
            this.Close();
        }

        private void BTNeDIT_Click(object sender, EventArgs e)
        {
            G.IsError = false;

            G.form.Show();
            this.Close();
           
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            //List<Elp> elps = new List<Elp>();
            //List<Elp> elps2 = new List<Elp>();
            //foreach (Unit unit in G.Unit)
            //{
            //    if (unit.para.typeUnit == typeUnit.Elp)
            //    {
                   
            //        if (!unit.isConnect)
            //        {
            //            elps2.Add((Elp)unit.uc);
            //            foreach (String s in G.listCCD)
            //            {

            //            }
            //        }
            //        else
            //        {
            //            elps.Add((Elp)unit.uc);
            //        }
            //    }
            //}
            //if (!unit.isConnect)
            //{
            //    foreach (String s in G.listCCD)
            //    {

            //    }
            //}
        }
        String name;
        private void cbCCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbNameCCD.SelectedIndex = cbNameCCD.FindStringExact(G.elps[cbCCD.SelectedIndex].unit.para[0].path);
        }
        bool IsSetting = false;
        private void btnSettingCCD_Click(object sender, EventArgs e)
        {
            IsSetting = !IsSetting;
            if (G.elps[cbCCD.SelectedIndex].unit.node[1].CameraUsb[0].IsOpened())
            {

                
                if (IsSetting)
                {
                    G.elps[cbCCD.SelectedIndex].unit.node[1].CameraUsb[0].Settings = 1;
                    btnSettingCCD.BackColor = Color.SteelBlue;
                }    
                   
                else
                {
                     name = G.listCCD[G.listCCD.FindIndex(a=>a.Contains(cbNameCCD.Text))].Split('$')[1];
                    AutoIt.AutoItX.WinClose(name + " Properties");
                    btnSettingCCD.BackColor = Color.LightGray;
                }    
                   
                if (!G.elps[cbCCD.SelectedIndex].isLive)
                {
                    G.elps[cbCCD.SelectedIndex].btnCap.Enabled = true;
                    G.elps[cbCCD.SelectedIndex].btnCap.PerformClick();
                }    
                    
                else
                {
                    G.elps[cbCCD.SelectedIndex].btnStop.Enabled = true;
                    G.elps[cbCCD.SelectedIndex].btnStop.PerformClick();
                }    
                   
            }
           
        }

        private void cbNameCCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            String path = cbNameCCD.Text;
            foreach (Elp elp in G.elps)
            {
                if(G.elps[cbCCD.SelectedIndex]!=elp)
                if (elp.unit.para[0].path.Contains(cbNameCCD.Text))
                {
                    path = "";
                    break;

                }
            }
            if (path == "")
            {
                btnTest.BackColor = Color.Red;
                tmColor.Enabled = true;
            }    
              else
            {
                G.elps[cbCCD.SelectedIndex].unit.para[0].path = path;
                btnTest.PerformClick();
            }    
        
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ButtonSavePro buttonSavePro = new ButtonSavePro();
            buttonSavePro.PerformClick();
            G.IsRestart = true;
            Application.Restart();
            G.form.Close();
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
