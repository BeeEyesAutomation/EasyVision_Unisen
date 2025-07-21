using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
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
          //  p2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, p2.Width, p2.Height, 5, 5));

        }

        private void btnNextStep_Click(object sender, EventArgs e)
        {
           
            SaveData.Project(Global.Project);
            btnNextStep.Enabled = false;
            this.Parent.Controls.Remove(this);
          
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

        }

        private void btnLoadImge_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                G.EditTool.View. pathRaw = fileDialog.FileName;
              BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Common.LoadImage(G.EditTool.View.pathRaw, ImreadModes.AnyColor);
                G.EditTool.View.matRes = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
              
                G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);

                G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();

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
            BeeCore.Common.listCamera[Global.IndexChoose].Read();
         


        }

        private void workRead_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Native.GetImg();
            Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw);
         
            G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
            btnNextStep.Enabled = true;
            G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
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
            //BeeCore.Common.ReadCCD(G.Config.IsHist, G.ConfiGlobal.TypeCamera );
            //BeeCore.Common.listCamera[Global.IndexChoose].matRaw= BeeCore.Common.GetImageRaw();
            //G.EditTool.View.imgView.ImageIpl= BeeCore.Common.listCamera[Global.IndexChoose].matRaw;
        }
        public void PressLive()
        {
          //  if (btnCalib.IsCLick) btnCalib.PerformClick();
            G.EditTool.View.btnLive.Enabled = true;

            G.EditTool.View.btnLive.PerformClick();
           
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
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value = (int)trackExposure.Value;
           BeeCore.Common.listCamera[Global.IndexChoose].SetExpo();
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;
            //  numExposure.Value = Global.ParaCommon.Exposure;

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
                Global.ParaCommon.TypeLight = 1;
            BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, Global.ParaCommon.IsOnLight);

            //  BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, btnBackLight.IsCLick);
        }

        private void btnTopLight_Click(object sender, EventArgs e)
        {
           

            //if (G.Header.SerialPort1.IsOpen)
            //        G.Header.SerialPort1.WriteLine("Topl");
                btnTopLight.Enabled = true;
                btnTopLight.Enabled = true;
                Global.ParaCommon.TypeLight = 2;
            //  BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, btnTopLight.IsCLick);
            BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, Global.ParaCommon.IsOnLight);

        }

        private void btnBoth_Click(object sender, EventArgs e)
        {
           
            //    if (G.Header.SerialPort1.IsOpen)
            //        G.Header.SerialPort1.WriteLine("Both");
            //else
            //    MessageBox.Show("Port Close");
            Global.ParaCommon.TypeLight = 3;
            BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, Global.ParaCommon.IsOnLight);

        }
        bool _isLight;
        private void btnONLight_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsOnLight = btnONLight.IsCLick;
            BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, Global.ParaCommon.IsOnLight);

        }

        private void numExposure_ValueChanged(object sender, EventArgs e)
        {
            //   Global.ParaCommon.Exposure =(int) numExposure.Value;
            //BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value= (int)numExposure.Value;
           // trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFull_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.TypeResolution = 1;
            Shows.Full(G.EditTool.View.imgView,BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
            G.Config.imgZoom =G.EditTool.View. imgView.Zoom;
            G.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
            G.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void btnHD_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.TypeResolution = 2;
            Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
            G.Config.imgZoom = G.EditTool.View.imgView.Zoom;
            G.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
            G.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void btn480_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.TypeResolution = 3;
            Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
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
            G.Config.DelayOutput = (int)numDelay.Value;
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
            //if (Global.Comunication.IO.IsConnected)
            //{
            //    Global.Comunication.IO.WritePara(4160, G.Config.DelayOutput);
            //}
            
        }

        private void trackGain_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value = (int)trackGain.Value;
            BeeCore.Common.listCamera[Global.IndexChoose].SetGain();
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value;
        }

        private void btnRevese_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsRevese = btnRevese.IsCLick;
        }

        private void btnEqualization_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsEqualization = btnRevese.IsCLick;
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsMirror = btnMirror.IsCLick;
        }

        private void btnEnhance_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsHance = btnEnhance.IsCLick;
           
        }

        private void numTrigger_ValueChanged(object sender, EventArgs e)
        {
            G.Config.delayTrigger = (int)numTrigger.Value;
        }

        private void SettingStep1_Load(object sender, EventArgs e)
        {
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure = new ValuePara();
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain = new ValuePara();
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift = new ValuePara();


            BeeCore.Common.listCamera[Global.IndexChoose].GetExpo();
            BeeCore.Common.listCamera[Global.IndexChoose].GetGain();
            BeeCore.Common.listCamera[Global.IndexChoose].GetShift();
            trackExposure.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Min;
            trackExposure.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Max;
            if (trackExposure.Max > 20000) 
                trackExposure.Max = 20000;
            trackExposure.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Step;
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;

          
            trackGain.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Min;
            trackGain.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Max;
            trackGain.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Step;
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value;

            trackShift.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Min;
            trackShift.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Max;
            trackShift.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Step;
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value;
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

        private void trackShift_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value = (int)trackShift.Value;
            BeeCore.Common.listCamera[Global.IndexChoose].SetShift();
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value;

        }
    }
}
