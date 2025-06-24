using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class IOSetting : Form
    {
        public IOSetting()
        {
            InitializeComponent();
        }

        private void cbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        BeeCore.Config ConfigPrev;
        private void IOSetting_Load(object sender, EventArgs e)
        {
            ConfigPrev = G.Config;
          
            btnSaveOK.IsCLick = G.Config.IsSaveOK;
            btnSaveNG.IsCLick = G.Config.IsSaveNG;
            btnSaveRaw.IsCLick = G.Config.IsSaveRaw;
            btnSaveRS.IsCLick = G.Config.IsSaveRS;
            switch (G.Config.TypeSave){
                case 1:btnSmall.PerformClick(); break;
                case 2: btnNormal.PerformClick(); break;
                case 3: btnbig.PerformClick(); break;
            }
            trackLimitDay.Value = G.Config.LimitDateSave;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
        //    G.Config.namePort =cbSerialPort.Text.Trim();
            G.Header.ConnectCom();
            if (File.Exists("Default.config"))
                File.Delete("Default.config");
            Access.SaveConfig("Default.config", G.Config);
        }

        private void btnSaveOK_Click(object sender, EventArgs e)
        {
            G.Config.IsSaveOK = btnSaveOK.IsCLick;
        }

        private void btnSaveNG_Click(object sender, EventArgs e)
        {
            G.Config.IsSaveNG = btnSaveNG.IsCLick;
        }

        private void trackLimitDay_ValueChanged(float obj)
        {
            G.Config.LimitDateSave = (int)trackLimitDay.Value;
        }

        private void btnSmall_Click(object sender, EventArgs e)
        {
            G.Config.TypeSave = 1;
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            G.Config.TypeSave = 2;
        }

        private void btnbig_Click(object sender, EventArgs e)
        {
            G.Config.TypeSave = 3;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            G.Config = ConfigPrev;
            this.Close();
        }

        private void IOSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists("Default.config"))
                File.Delete("Default.config");
            Access.SaveConfig("Default.config", G.Config);

        }

        private void btnSaveRaw_Click(object sender, EventArgs e)
        {
            G.Config.IsSaveRaw = btnSaveRaw.IsCLick;
        }

        private void btnSaveRS_Click(object sender, EventArgs e)
        {
            G.Config.IsSaveRS = btnSaveRS.IsCLick;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            ScanCCD scanCCD = new ScanCCD();
            scanCCD.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }
    }
}
