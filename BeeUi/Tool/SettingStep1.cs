using BeeCore;
using BeeCore.Funtion;
using BeeUi.Data;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Forms;

namespace BeeUi.Tool
{
   
    [Serializable()]
    public partial class SettingStep1 : UserControl
    {
   
        public SettingStep1()
        {
            InitializeComponent();
            //p.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, p.Width, p.Height, 5, 5));
            p2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, p2.Width, p2.Height, 5, 5));

        }

        private void btnNextStep_Click(object sender, EventArgs e)
        {
           
            SaveData.Project(G.Project, BeeCore.G.ParaCam);
            btnNextStep.Enabled = false;
            this.Parent.Controls.Remove(this);
            if (btnLive.IsCLick)
                btnLive.PerformClick();
            G.StepEdit.btnStep2.PerformClick();

        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
       (
           int nLeftRect,     // x-coordinate of upper-left corner
           int nTopRect,      // y-coordinate of upper-left corner
           int nRightRect,    // x-coordinate of lower-right corner
           int nBottomRect,   // y-coordinate of lower-right corner
           int nWidthEllipse, // height of ellipse
           int nHeightEllipse // width of ellipse
       );
        private void SettingStep2_Load(object sender, EventArgs e)
        {
            BeeCore.Common.listCamera[G.indexChoose].Para.Exposure= BeeCore.Common.listCamera[G.indexChoose].GetExpo();
            // trackExposure.Min = (int)BeeCore.Common.MinExposure;
            //trackExposure.Max = (int)BeeCore.Common.MaxExposure;
            // trackExposure.Step= (int)BeeCore.Common.StepExposure;
            btnInternal.IsCLick=!G.Config.IsExternal ;
            btnExternal.IsCLick = G.Config.IsExternal;

            switch (BeeCore.G.ParaCam.TypeResolution)
            {
                case 1:
                    btnFull.IsCLick = true;
                    break;
                case 2:
                    btnHD.IsCLick = true;
                    break;
                case 3:
                    btn480.IsCLick = true;
                    break;
            }
          
            if (BeeCore.Common.listCamera[G.indexChoose].Para.Exposure>trackExposure.Max)
                trackExposure.Value =trackExposure.Max ;
            if (BeeCore.Common.listCamera[G.indexChoose].Para.Exposure < trackExposure.Min)
                trackExposure.Value = trackExposure.Min;
            else
                trackExposure.Value = BeeCore.Common.listCamera[G.indexChoose].Para.Exposure;
            switch (BeeCore.G.ParaCam.TypeLight)
            {
                case 1:
                    btnBackLight.IsCLick = true;
                    break;
                case 2:
                    btnTopLight.IsCLick = true; 
                    break;
                case 3:
                    btnBoth.IsCLick = true;
                    break; 
            }
            btnONLight.IsCLick = BeeCore.G.ParaCam.IsOnLight;

            numTrigger.Value = G.Config.delayTrigger;
            btnShowGrid.IsCLick = G.Config.IsShowGird ;
            btnShowCenter.IsCLick = G.Config.IsShowCenter;
            btnShowArea.IsCLick = G.Config.IsShowArea;
            if(G.Config.DelayOutput<numDelay.Minimum) G.Config.DelayOutput=numDelay.Minimum;
            if (G.Config.DelayOutput > numDelay.Maxnimum) G.Config.DelayOutput = numDelay.Maxnimum;

            numDelay.Value = G.Config.DelayOutput;
            valueDelyaOld= G.Config.DelayOutput;
            //numDelay.Value = G.Config.delayTrigger;
            //btnEqubtnalize.IsCLick = G.Config.IsHist ;
        }
       
        private void btnLoadImge_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                G.EditTool.View. pathRaw = fileDialog.FileName;
              BeeCore.Common.listCamera[G.indexChoose].matRaw = BeeCore.Common.LoadImage(G.EditTool.View.pathRaw, ImreadModes.AnyColor);
                G.EditTool.View.matRes = BeeCore.Common.listCamera[G.indexChoose].matRaw.Clone();
              
                G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
                G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);

                G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[G.indexChoose].matRaw.ToBitmap();

                G.EditTool.View.imgView.Invalidate();
                btnNextStep.Enabled = true;
             //   btnNextStep.BackgroundImage = Properties.Resources.btnChoose1;

            

            }
        }

        private void btnCapCamera_Click(object sender, EventArgs e)
        {
            if (!workRead.IsBusy)
                workRead.RunWorkerAsync();



        }

        private void workRead_DoWork(object sender, DoWorkEventArgs e)
        {
            BeeCore.Common.listCamera[G.indexChoose].Read();
         


        }

        private void workRead_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BeeCore.Common.listCamera[G.indexChoose].matRaw = BeeCore.Native.GetImg();
            BeeCore.G.ParaCam.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[G.indexChoose].matRaw);
         
            G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.matResgiter = BeeCore.Common.listCamera[G.indexChoose].matRaw.Clone();
            btnNextStep.Enabled = true;
            G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[G.indexChoose].matRaw.ToBitmap();
          //  btnNextStep.BackgroundImage = Properties.Resources.btnChoose1;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            G.Header.btnMode.PerformClick();
        }
        public int indexTool;
   

        private void btnEqualize_Click(object sender, EventArgs e)
        {
            //G.Config.IsHist  = btnEqubtnalize.IsCLick;

            //if (File.Exists("Default.config"))
            //    File.Delete("Default.config");
            //Access.SaveConfig("Default.config", G.Config);
            //BeeCore.Common.ReadCCD(G.Config.IsHist, G.Config.TypeCamera);
            //BeeCore.Common.listCamera[G.indexChoose].matRaw= BeeCore.Common.GetImageRaw();
            //G.EditTool.View.imgView.ImageIpl= BeeCore.Common.listCamera[G.indexChoose].matRaw;
        }
        public void PressLive()
        {
          //  if (btnCalib.IsCLick) btnCalib.PerformClick();
            G.EditTool.View.btnLive.Enabled = true;

            G.EditTool.View.btnLive.PerformClick();
            if (btnLive.IsCLick)
                G.EditTool.View.btnLive.Enabled = false;
            else
                G.EditTool.View.btnLive.Enabled = true;
        }
        private void btnLive_Click(object sender, EventArgs e)
        {
            PressLive();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            G.EditTool.View.btnShowSetting.PerformClick();
        }

     

        private void trackScoreErr_Load(object sender, EventArgs e)
        {

        }

       

        private void btnShowGrid_Click(object sender, EventArgs e)
        {
          
        }

        private void btnShowCenter_Click(object sender, EventArgs e)
        {
            G.Config.IsShowCenter = btnShowCenter.IsCLick;
            G.EditTool.View.imgView.Invalidate();
        }

        private void btnShowArea_Click(object sender, EventArgs e)
        {
           
        }

        private void SettingStep1_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                G.Config.IsShowCenter = false;
                G.Config.IsShowArea = false;
                G.Config.IsShowGird = false;
                G.EditTool.View.imgView.Invalidate();
                if (btnLive.IsCLick)
                {
                    btnLive.IsCLick = false; PressLive();
                }
            }
                   
        }
        FormCalib formCalib;
        private void btnCalib_Click(object sender, EventArgs e)
        {
            //if(btnCalib.IsCLick)
            //{
            //    if (formCalib != null)
            //        formCalib.Close();

            //    formCalib = new FormCalib();
            //    formCalib.Show();
            //}    
            //else
            //{
            //    if(formCalib!=null)
            //    formCalib.Close();
            //}    
        }

        private void trackExposure_ValueChanged(float obj)
        {
            //  trackExposure.Value= trackExposure.Value - (trackExposure.Value % trackExposure.Step);
            BeeCore.Common.listCamera[G.indexChoose].Para.Exposure = (int)trackExposure.Value;
          //  numExposure.Value = BeeCore.G.ParaCam.Exposure;

        }

        private void trackExposure_Load(object sender, EventArgs e)
        {

        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
          //  G.Config.delayTrigger = (int)numDelay.Value;
            if (File.Exists("Default.config"))
                File.Delete("Default.config");
            Access.SaveConfig("Default.config", G.Config);
        }

        private void btnBackLight_Click(object sender, EventArgs e)
        {
           
                //if (G.Header.SerialPort1.IsOpen)
                //    G.Header.SerialPort1.WriteLine("Botl");
                btnBackLight.Enabled = true;
                btnBackLight.Enabled = true;
                BeeCore.G.ParaCam.TypeLight = 1;
            BeeCore.Common.listCamera[G.indexChoose].Light(BeeCore.G.ParaCam.TypeLight, BeeCore.G.ParaCam.IsOnLight);

            //  BeeCore.Common.listCamera[G.indexChoose].Light(BeeCore.G.ParaCam.TypeLight, btnBackLight.IsCLick);
        }

        private void btnTopLight_Click(object sender, EventArgs e)
        {
           

            //if (G.Header.SerialPort1.IsOpen)
            //        G.Header.SerialPort1.WriteLine("Topl");
                btnTopLight.Enabled = true;
                btnTopLight.Enabled = true;
                BeeCore.G.ParaCam.TypeLight = 2;
            //  BeeCore.Common.listCamera[G.indexChoose].Light(BeeCore.G.ParaCam.TypeLight, btnTopLight.IsCLick);
            BeeCore.Common.listCamera[G.indexChoose].Light(BeeCore.G.ParaCam.TypeLight, BeeCore.G.ParaCam.IsOnLight);

        }

        private void btnBoth_Click(object sender, EventArgs e)
        {
           
            //    if (G.Header.SerialPort1.IsOpen)
            //        G.Header.SerialPort1.WriteLine("Both");
            //else
            //    MessageBox.Show("Port Close");
            BeeCore.G.ParaCam.TypeLight = 3;
            BeeCore.Common.listCamera[G.indexChoose].Light(BeeCore.G.ParaCam.TypeLight, BeeCore.G.ParaCam.IsOnLight);

        }
        bool _isLight;
        private void btnONLight_Click(object sender, EventArgs e)
        {
            BeeCore.G.ParaCam.IsOnLight = btnONLight.IsCLick;
            BeeCore.Common.listCamera[G.indexChoose].Light(BeeCore.G.ParaCam.TypeLight, BeeCore.G.ParaCam.IsOnLight);

        }

        private void numExposure_ValueChanged(object sender, EventArgs e)
        {
         //   BeeCore.G.ParaCam.Exposure =(int) numExposure.Value;
          
            trackExposure.Value = BeeCore.Common.listCamera[G.indexChoose].Para.Exposure;
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFull_Click(object sender, EventArgs e)
        {
            BeeCore.G.ParaCam.TypeResolution = 1;
            Shows.Full(G.EditTool.View.imgView,BeeCore.Common.listCamera[G.indexChoose].matRaw.Size());
            G.Config.imgZoom =G.EditTool.View. imgView.Zoom;
            G.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
            G.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void btnHD_Click(object sender, EventArgs e)
        {
            BeeCore.G.ParaCam.TypeResolution = 2;
            Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[G.indexChoose].matRaw.Size());
            G.Config.imgZoom = G.EditTool.View.imgView.Zoom;
            G.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
            G.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void btn480_Click(object sender, EventArgs e)
        {
            BeeCore.G.ParaCam.TypeResolution = 3;
            Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[G.indexChoose].matRaw.Size());
            G.Config.imgZoom = G.EditTool.View.imgView.Zoom;
            G.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
            G.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void numDelay_Load(object sender, EventArgs e)
        {

        }

        private void numDelay_ValueChanged_1(object sender, EventArgs e)
        {
            G.Config.DelayOutput = numDelay.Value;
            //tmDelaySend.Enabled = false;
            //tmDelaySend.Enabled = true;

        }

        private void btnInternal_Click(object sender, EventArgs e)
        {
            G.Config.IsExternal =! btnInternal.IsCLick;
         
            if (!G.Config.IsExternal)
            {
                G.EditTool.View.btnTypeTrig.Enabled=false;
                G.EditTool.View.btnTypeTrig.Text = "Trig Internal";
            }
            else
            {
                G.EditTool.View.btnTypeTrig.Enabled = true;
                G.EditTool.View.btnTypeTrig.Text = "Trig External";
            }
        }

        private void btnExternal_Click(object sender, EventArgs e)
        {
            G.Config.IsExternal = btnExternal.IsCLick;
         
            if (!G.Config.IsExternal)
            {
                G.EditTool.View.btnTypeTrig.Enabled = false;
                G.EditTool.View.btnTypeTrig.Text = "Trig Internal";
            }
            else
            {
                G.EditTool.View.btnTypeTrig.Enabled = true;
                G.EditTool.View.btnTypeTrig.Text = "Trig External";
            }
        }

        private void numDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void numDelay_KeyDown(object sender, KeyEventArgs e)
        {
           
        }
        int valueDelyaOld = 0;
        private void tmDelaySend_Tick(object sender, EventArgs e)
        {
            //tmDelaySend.Enabled = false;
            //G.Config.DelayOutput = numDelay.Value;
            //if (G.PLC.IsConnected)
            //{
            //    G.PLC.WritePara(4160, G.Config.DelayOutput);
            //}
            
        }

        private void trackGain_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[G.indexChoose].Para.Gain = (int)trackGain.Value;
        }

        private void btnRevese_Click(object sender, EventArgs e)
        {
            BeeCore.G.ParaCam.IsRevese = btnRevese.IsCLick;
        }

        private void btnEqualization_Click(object sender, EventArgs e)
        {
            BeeCore.G.ParaCam.IsEqualization = btnRevese.IsCLick;
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            BeeCore.G.ParaCam.IsMirror = btnMirror.IsCLick;
        }

        private void btnEnhance_Click(object sender, EventArgs e)
        {
            BeeCore.G.ParaCam.IsHance = btnEnhance.IsCLick;
           
        }

        private void numTrigger_ValueChanged(object sender, EventArgs e)
        {
            G.Config.delayTrigger = (int)numTrigger.Value;
        }
    }
}
