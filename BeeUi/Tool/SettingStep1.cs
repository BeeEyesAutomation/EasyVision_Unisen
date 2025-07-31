using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;

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
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.AutoScaleMode = AutoScaleMode.Dpi; // hoặc AutoScaleMode.Font

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
   

        

       
    

        private async void trackExposure_ValueChanged(float obj)
        {
            //  trackExposure.Value= trackExposure.Value - (trackExposure.Value % trackExposure.Step);
            if (trackExposure.Value == BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value) 
                return;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value = (int)trackExposure.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetExpo();
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;
            //  numExposure.Value = Global.ParaCommon.Exposure;

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
       
      

        private void btnFull_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.TypeResolution = 1;
            Shows.Full(G.EditTool.View.imgView,BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
           Global.Config.imgZoom =G.EditTool.View. imgView.Zoom;
           Global.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
           Global.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void btnHD_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.TypeResolution = 2;
            Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
           Global.Config.imgZoom = G.EditTool.View.imgView.Zoom;
           Global.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
           Global.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void btn480_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.TypeResolution = 3;
            Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
           Global.Config.imgZoom = G.EditTool.View.imgView.Zoom;
           Global.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
           Global.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

     
       
        private void btnInternal_Click(object sender, EventArgs e)
        {
           Global.ParaCommon.IsExternal =! btnInternal.IsCLick;
         
            if (! Global.ParaCommon.IsExternal)
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
           Global.ParaCommon.IsExternal = btnExternal.IsCLick;
         
            if (! Global.ParaCommon.IsExternal)
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
        int valueDelyaOld = 0;
     
        private async void trackGain_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value = (int)trackGain.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetGain();
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value;
        }


        bool IsReaded = false;
        private void SettingStep1_Load(object sender, EventArgs e)
        {
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure = new ValuePara();
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain = new ValuePara();
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift = new ValuePara();


             AdDelayTrig.Value = Global.ParaCommon.Comunication.IO.DelayTrigger;
            AdDelayOutput.Value = Global.ParaCommon.Comunication.IO.DelayOutput;
            btnLight1.IsCLick = Global.ParaCommon.Comunication.IO.IsLight1;
            btnLight2.IsCLick = Global.ParaCommon.Comunication.IO.IsLight2;
            btnLight3.IsCLick = Global.ParaCommon.Comunication.IO.IsLight3;
            btnOn.IsCLick = Global.ParaCommon.IsOnLight;
            btnInternal.IsCLick =! Global.ParaCommon.IsExternal;
            btnExternal.IsCLick = Global.ParaCommon.IsExternal;
            if(!IsReaded)
            {
                IsReaded = true;
                if (!workReadPara.IsBusy)
                    workReadPara.RunWorkerAsync();
            }
           
        }


        private async void trackShift_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value = (int)trackShift.Value;
          await  BeeCore.Common.listCamera[Global.IndexChoose].SetShift();
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value;

        }

     
        private void AdDelayTrig_ValueChanged(float obj)
        {
            Global.ParaCommon.Comunication.IO.DelayTrigger = (int)AdDelayTrig.Value;
        }

        private void AdDelayOutput_ValueChanged(float obj)
        {
            Global.ParaCommon.Comunication.IO.DelayOutput= (int)AdDelayOutput.Value;
        }

        private void btnOFF_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsOnLight = false;
            BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, Global.ParaCommon.IsOnLight);

        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.IsOnLight =true;
            BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, Global.ParaCommon.IsOnLight);

        }

        private void btnLight1_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.IsLight1 = btnLight1.IsCLick;
        }

        private void btnLight2_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.IsLight1 = btnLight2.IsCLick;
        }

        private void btnLight3_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.IO.IsLight1 = btnLight3.IsCLick;
        }

        private void AdjWidth_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value = AdjWidth.Value ;
        }

        private void AdjHeight_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value = AdjWidth.Value;
        }

        private void AdjOffsetX_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Value = AdjWidth.Value;
        }

        private void AdjOffSetY_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Value = AdjWidth.Value;
        }

   
      
        private async void workReadPara_DoWork(object sender, DoWorkEventArgs e)
        {
            await BeeCore.Common.listCamera[Global.IndexChoose].GetExpo();
            await BeeCore.Common.listCamera[Global.IndexChoose].GetGain();
            await BeeCore.Common.listCamera[Global.IndexChoose].GetShift();

        }

        private  void workReadPara_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            trackExposure.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Min;
            trackExposure.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Max;
            if (trackExposure.Max > 20000)
            {
               
                trackExposure.Max = 20000;
            }
            if(BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value>20000)
            {
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value = 20000;
            }

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
    }
}
