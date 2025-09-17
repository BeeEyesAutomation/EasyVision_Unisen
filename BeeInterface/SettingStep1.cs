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
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
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
            this.HandleCreated += SettingStep1_HandleCreated;
            //p.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, p.Width, p.Height, 5, 5));
            //  p2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, p2.Width, p2.Height, 5, 5));

        }

        private void SettingStep1_HandleCreated(object sender, EventArgs e)
        {
            tmShowPara.Enabled = true;
        }

        private void btnNextStep_Click(object sender, EventArgs e)
        {
           
            //SaveData.Project(Global.Project);
            //btnNextStep.Enabled = false;
            //this.Parent.Controls.Remove(this);
            Global.EditTool.RefreshGuiEdit(Step.Step2);
            //G.StepEdit.btnStep2.PerformClick();

        }
   
     

     

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Global.EditTool.RefreshGuiEdit(Step.Run);
        }
        public int indexTool;
   

        

       
    



        private void btnBoth_Click(object sender, EventArgs e)
        {
           
            //    if (G.Header.SerialPort1.IsOpen)
            //        G.Header.SerialPort1.WriteLine("Both");
            //else
            //    MessageBox.Show("Port Close");
            Global.ParaCommon.TypeLight = 3;
            BeeCore.Common.listCamera[Global.IndexChoose].Light(Global.ParaCommon.TypeLight, Global.ParaCommon.IsOnLight);

        }
       
      

       

        private void btnHD_Click(object sender, EventArgs e)
        {
        //    Global.ParaCommon.TypeResolution = 2;
        //    Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
        //   Global.Config.imgZoom = G.EditTool.View.imgView.Zoom;
        //   Global.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
        //   Global.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void btn480_Click(object sender, EventArgs e)
        {
        //    Global.ParaCommon.TypeResolution = 3;
        //    Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
        //   Global.Config.imgZoom = G.EditTool.View.imgView.Zoom;
        //   Global.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
        //   Global.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

     
       
        private void btnInternal_Click(object sender, EventArgs e)
        {
           Global.ParaCommon.IsExternal =! btnInternal.IsCLick;
         
            //if (! Global.ParaCommon.IsExternal)
            //{
            //    G.EditTool.View.btnTypeTrig.Enabled=false;
            //    G.EditTool.View.btnTypeTrig.Text = "Trig Internal";
            //}
            //else
            //{
            //    G.EditTool.View.btnTypeTrig.Enabled = true;
            //    G.EditTool.View.btnTypeTrig.Text = "Trig External";
            //}
        }

        private void btnExternal_Click(object sender, EventArgs e)
        {
           Global.ParaCommon.IsExternal = btnExternal.IsCLick;
         
            //if (! Global.ParaCommon.IsExternal)
            //{
            //    G.EditTool.View.btnTypeTrig.Enabled = false;
            //    G.EditTool.View.btnTypeTrig.Text = "Trig Internal";
            //}
            //else
            //{
            //    G.EditTool.View.btnTypeTrig.Enabled = true;
            //    G.EditTool.View.btnTypeTrig.Text = "Trig External";
            //}
        }
        int valueDelyaOld = 0;
     
       


        bool IsReaded = false;
        private void SettingStep1_Load(object sender, EventArgs e)
        {
            if (BeeCore.Common.listCamera[Global.IndexChoose] == null)
            { BeeCore.Common.listCamera[Global.IndexChoose] = new Camera(new ParaCamera(), Global.IndexChoose);
                Global.ScanCCD.ShowDialog();
                return;
            }
            // if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure = new ValuePara();
            //if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain = new ValuePara();
            //if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift == null) BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift = new ValuePara();
            AdjZoom.IsInital = true;
            AdjZoom.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Zoom;
            AdjFocus.IsInital = true;
            AdjFocus.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Focus;
            AdDelayTrig.IsInital = true;
            AdDelayTrig.Value = Global.ParaCommon.Comunication.Protocol.DelayTrigger;
            AdDelayOutput.IsInital = true;
            AdDelayOutput.Value = Global.ParaCommon.Comunication.Protocol.DelayOutput;
            btnLight1.IsCLick = Global.ParaCommon.Comunication.Protocol.IsLight1;
            btnLight2.IsCLick = Global.ParaCommon.Comunication.Protocol.IsLight2;
            btnLight3.IsCLick = Global.ParaCommon.Comunication.Protocol.IsLight3;
            btnOn.IsCLick = Global.ParaCommon.IsOnLight;
            btnInternal.IsCLick =! Global.ParaCommon.IsExternal;
            btnExternal.IsCLick = Global.ParaCommon.IsExternal;
            Global.LiveChanged += Global_LiveChanged;
           btnBlink.IsCLick= Global.ParaCommon.Comunication.Protocol.IsBlink ;
            btnAllTime.IsCLick =! Global.ParaCommon.Comunication.Protocol.IsBlink;
            layoutDelay.Enabled = !Global.ParaCommon.Comunication.Protocol.IsBlink;
          
            //cbFormat.DataSource = (ColorConversionCodes[])Enum.GetValues(typeof(ColorConversionCodes));
            //cbFormat.Text = ((ColorConversionCodes)BeeCore.Common.listCamera[Global.IndexChoose].Para.ColorCode).ToString();
            //   workReadPara.RunWorkerAsync();
        }

        private void Global_LiveChanged(bool obj)
        {
            AdjWidth.Enabled =! obj;
            AdjHeight.Enabled = !obj;
           
           
        }

       

     
        private void AdDelayTrig_ValueChanged(float obj)
        {
            Global.ParaCommon.Comunication.Protocol.DelayTrigger = (int)AdDelayTrig.Value;
        }

        private void AdDelayOutput_ValueChanged(float obj)
        {
            Global.ParaCommon.Comunication.Protocol.DelayOutput= (int)AdDelayOutput.Value;
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
            Global.ParaCommon.Comunication.Protocol.IsLight1 = btnLight1.IsCLick;
        }

        private void btnLight2_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.IsLight1 = btnLight2.IsCLick;
        }

        private void btnLight3_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.IsLight1 = btnLight3.IsCLick;
        }
       

      

        private async void AdjOffsetX_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Value = (int)AdjOffsetX.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetOffSetX();
            AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Value;
        }

        private async void AdjOffSetY_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Value = (int)AdjOffSetY.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetOffSetY();
            AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Value;
        }







        private async void btnDownLoadPara_Click(object sender, EventArgs e)
        {
            btnDownLoadPara.IsCLick = true;
            btnDownLoadPara.Enabled = false;
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera == TypeCamera.USB)
            {
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Zoom = BeeCore.Common.listCamera[Global.IndexChoose].GetZoom();
                AdjZoom.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Zoom;
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Focus = BeeCore.Common.listCamera[Global.IndexChoose].GetFocus();
                AdjFocus.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Focus;
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value = BeeCore.Common.listCamera[Global.IndexChoose].GetWidthUSB();
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value;
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value = BeeCore.Common.listCamera[Global.IndexChoose].GetHeightUSB();
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value;
              await BeeCore.Common.listCamera[Global.IndexChoose].GetExpo();
                trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;
            }
            else
            {
                await BeeCore.Common.listCamera[Global.IndexChoose].GetExpo();
                await BeeCore.Common.listCamera[Global.IndexChoose].GetGain();
                await BeeCore.Common.listCamera[Global.IndexChoose].GetShift();
                await BeeCore.Common.listCamera[Global.IndexChoose].GetWidth();
                await BeeCore.Common.listCamera[Global.IndexChoose].GetHeight();
                await BeeCore.Common.listCamera[Global.IndexChoose].GetOffSetX();
                await BeeCore.Common.listCamera[Global.IndexChoose].GetOffSetY();
                await BeeCore.Common.listCamera[Global.IndexChoose].GetCenterX();
                await BeeCore.Common.listCamera[Global.IndexChoose].GetCenterY();
                if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera == TypeCamera.USB)
                {
                
                    BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Min = 3;
                    BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Max = 12;
                }
                    trackExposure.IsInital = true;
                trackExposure.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Min;
                trackExposure.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Max;
                if (trackExposure.Max > 50000)
                {

                    trackExposure.Max = 50000;
                }
                if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value > 50000)
                {
                    BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value = 50000;
                }

                trackExposure.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Step;
                trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;

                trackGain.IsInital = true;
                trackGain.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Min;
                trackGain.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Max;
                trackGain.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Step;
                trackGain.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value;
                trackShift.IsInital = true;
                trackShift.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Min;
                trackShift.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Max;
                trackShift.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Step;
                trackShift.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value;
                AdjWidth.IsInital = true;
                AdjWidth.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Min;
                AdjWidth.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Max;
                AdjWidth.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Step;
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value;
                AdjHeight.IsInital = true;
                AdjHeight.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Min;
                AdjHeight.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Max;
                AdjHeight.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Step;
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value;
                AdjOffsetX.IsInital = true;
                AdjOffsetX.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Min;
                AdjOffsetX.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Max;
                AdjOffsetX.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Step;
                AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Value;
                AdjOffSetY.IsInital = true;
                AdjOffSetY.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Min;
                AdjOffSetY.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Max;
                AdjOffSetY.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Step;
                AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Value;
            } 
          //  btnCenterX.IsCLick = Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexChoose].Para.CenterX);
          //  btnCenterY.IsCLick = Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexChoose].Para.CenterY);

            btnDownLoadPara.IsCLick = false;
            btnDownLoadPara.Enabled = true;
           
        }

        private  void btnCenterX_Click(object sender, EventArgs e)
        {
            AdjOffsetX.Value = AdjOffsetX.Max / 2;
           // BeeCore.Common.listCamera[Global.IndexChoose].Para.CenterX =Convert.ToInt32( btnCenterX.IsCLick);
           // await BeeCore.Common.listCamera[Global.IndexChoose].SetCenterX();
           // btnCenterX.IsCLick =Convert.ToBoolean( BeeCore.Common.listCamera[Global.IndexChoose].Para.CenterX);

        }

        private  void btnCenterY_Click(object sender, EventArgs e)
        {
            AdjOffSetY.Value = AdjOffSetY.Max / 2;
          //  BeeCore.Common.listCamera[Global.IndexChoose].Para.CenterY = Convert.ToInt32(btnCenterY.IsCLick);
          //  await BeeCore.Common.listCamera[Global.IndexChoose].SetCenterY();
          //  btnCenterY.IsCLick= Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexChoose].Para.CenterY);
        }

        private void tmShowPara_Tick(object sender, EventArgs e)
        {
        
           
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera==TypeCamera.USB)
            {
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Min = 320;
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Min = 240;

                BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Max = 3000;
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Max = 3000;

                BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Min = 3;
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Max = 12;
             //   trackExposure.Enabled = false;
                trackGain.Enabled = false;
                trackShift.Enabled = false;
                AdjOffsetX.Enabled = false;
                AdjOffSetY.Enabled = false;
                btnCenterX.Enabled = false;
                btnCenterY.Enabled = false;
            }
            trackExposure.IsInital = true;
            trackExposure.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Min;
            trackExposure.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Max;
            if (trackExposure.Max > 50000)
            {

                trackExposure.Max = 50000;
            }
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value > 50000)
            {
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value = 50000;
            }

            trackExposure.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Step;
            trackExposure.IsInital = true;
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;

            trackGain.IsInital = true;
            trackGain.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Min;
            trackGain.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Max;
            trackGain.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Step;
            trackGain.IsInital = true;
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value;
            trackShift.IsInital = true;
            trackShift.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Min;
            trackShift.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Max;
            trackShift.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Step;
            trackShift.IsInital = true;
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value;
            AdjWidth.IsInital = true;
            AdjWidth.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Min;
            AdjWidth.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Max;
            AdjWidth.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Step;
            AdjWidth.IsInital = true;
            AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value;
            AdjHeight.IsInital = true;
            AdjHeight.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Min;
            AdjHeight.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Max;
            AdjHeight.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Step;
            AdjHeight.IsInital = true;
            AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value;
            AdjOffsetX.IsInital = true;
            AdjOffsetX.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Min;
            AdjOffsetX.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Max;
            AdjOffsetX.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Step;
            AdjOffsetX.IsInital = true;
            AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Value;
            AdjOffSetY.IsInital = true;
            AdjOffSetY.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Min;
            AdjOffSetY.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Max;
            AdjOffSetY.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Step;
            AdjOffSetY.IsInital = true;
            AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Value;
            btnCenterX.IsCLick = Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexChoose].Para.CenterX);
            btnCenterY.IsCLick = Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexChoose].Para.CenterY);

            tmShowPara.Enabled = false;
        }

        private void btnBlink_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.IsBlink = true;
            layoutDelay.Enabled = true;
        }

        private void btnAllTime_Click(object sender, EventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.IsBlink = false;
            layoutDelay.Enabled = false;
        }

        private void tableLayout1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnChnageCamera_Click(object sender, EventArgs e)
        {

            if (Global.ScanCCD == null)
                return;
            if (Global.IsLive)
                MessageBox.Show("Please Stop Live");
            else
                Global.ScanCCD.ShowDialog();
        
        }

        private void btnFocus_Click(object sender, EventArgs e)
        {
           
            BeeCore.Common.listCamera[Global.IndexChoose].SetAutoFocus(btnFocus.IsCLick);
        }

        private void AdjZoom_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Zoom = (int)AdjZoom.Value;
            BeeCore.Common.listCamera[Global.IndexChoose].SetZoom();
        }

        private void AdjFocus_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Focus = (int)AdjFocus.Value;
            BeeCore.Common.listCamera[Global.IndexChoose].SetFocus();
        }

        private void cbFormat_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private async void AdjWidth_MouseUp(object sender, MouseEventArgs e)
        {
            IsDownWidth = false;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value = (int)AdjWidth.Value;
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera == TypeCamera.USB)
                BeeCore.Common.listCamera[Global.IndexChoose].SetWidthUSB();
            {
                await BeeCore.Common.listCamera[Global.IndexChoose].SetWidth();
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value;
                await BeeCore.Common.listCamera[Global.IndexChoose].GetOffSetX();
                AdjOffsetX.IsInital = true;
                AdjOffsetX.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Min;
                AdjOffsetX.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Max;
                AdjOffsetX.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Step;
                AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Value;
            }
        }

        private async void AdjHeight_MouseUp(object sender, MouseEventArgs e)
        {
            IsDownHeight = false;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value = (int)AdjHeight.Value;
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera == TypeCamera.USB)
                BeeCore.Common.listCamera[Global.IndexChoose].SetHeightUSB();
            {
                await BeeCore.Common.listCamera[Global.IndexChoose].SetHeight();
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value;
                await BeeCore.Common.listCamera[Global.IndexChoose].GetOffSetY();
                AdjOffSetY.IsInital = true;
                AdjOffSetY.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Min;
                AdjOffSetY.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Max;
                AdjOffSetY.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Step;
                AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Value;
            }
        }

        private async void trackExposure_MouseUp(object sender, MouseEventArgs e)
        {
            IsDownEx = false;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value = (int)trackExposure.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetExpo();
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;
        }

        private async void trackGain_MouseUp(object sender, MouseEventArgs e)
        {
            IsDownGain = false;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value = (int)trackGain.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetGain();
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value;
        }
        private async void trackGain_ValueChanged(float obj)
        {
            if (IsDownGain) return;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value = (int)trackGain.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetGain();
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Gain.Value;
        }
        private async void trackShift_MouseUp(object sender, MouseEventArgs e)
        {
            IsDownShift = false;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value = (int)trackShift.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetShift();
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value;
        }
        bool IsDownEx = false;
        private void trackExposure_MouseDown(object sender, MouseEventArgs e)
        {
            IsDownEx = true;
        }

        private async void trackExposure_ValueChanged(float obj)
        {
            if (IsDownEx) return;
                BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value = (int)trackExposure.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetExpo();
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Exposure.Value;
        }
        bool IsDownGain= false;
        private void trackGain_MouseDown(object sender, MouseEventArgs e)
        {
            IsDownGain = true;
        }
        bool IsDownShift = false,IsDownWidth,IsDownHeight;
        private void trackShift_MouseDown(object sender, MouseEventArgs e)
        {
            IsDownShift = true;

        }
        private async void trackShift_ValueChanged(float obj)
        {
            if (IsDownShift) return;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value = (int)trackShift.Value;
            await BeeCore.Common.listCamera[Global.IndexChoose].SetShift();
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Shift.Value;
        }

        private void AdjWidth_MouseDown(object sender, MouseEventArgs e)
        {
            IsDownWidth = true;
        }

        private void AdjHeight_MouseDown(object sender, MouseEventArgs e)
        {
            IsDownHeight = true;
        }

        private void SettingStep1_VisibleChanged(object sender, EventArgs e)
        {
            btnInternal.IsCLick = !Global.ParaCommon.IsExternal;
            btnExternal.IsCLick = Global.ParaCommon.IsExternal;
        }

        private async void AdjWidth_ValueChanged(float obj)
        {
            if (IsDownWidth) return;

            BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value = (int)AdjWidth.Value;
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera == TypeCamera.USB)
                BeeCore.Common.listCamera[Global.IndexChoose].SetWidthUSB();
            {
                await BeeCore.Common.listCamera[Global.IndexChoose].SetWidth();
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Width.Value;
                await BeeCore.Common.listCamera[Global.IndexChoose].GetOffSetX();
                AdjOffsetX.IsInital = true;
                AdjOffsetX.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Min;
                AdjOffsetX.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Max;
                AdjOffsetX.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Step;
                AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetX.Value;
            }
        }

        private async void AdjHeight_ValueChanged(float obj)
        {
            if (IsDownHeight) return;
            BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value = (int)AdjHeight.Value;
            if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera == TypeCamera.USB)
                BeeCore.Common.listCamera[Global.IndexChoose].SetHeightUSB();
            {
                await BeeCore.Common.listCamera[Global.IndexChoose].SetHeight();
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.Height.Value;
                await BeeCore.Common.listCamera[Global.IndexChoose].GetOffSetY();
                AdjOffSetY.IsInital = true;
                AdjOffSetY.Min = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Min;
                AdjOffSetY.Max = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Max;
                AdjOffSetY.Step = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Step;
                AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexChoose].Para.OffSetY.Value;
            }
        }
    }
}
