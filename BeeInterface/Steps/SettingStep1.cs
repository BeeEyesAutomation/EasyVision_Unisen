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
            //this.DoubleBuffered = true;
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.AutoScaleMode = AutoScaleMode.Dpi; // hoặc AutoScaleMode.Font
            this.HandleCreated += SettingStep1_HandleCreated;
            //p.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, p.Width, p.Height, 5, 5));
            //  p2.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, p2.Width, p2.Height, 5, 5));

        }

        private void SettingStep1_HandleCreated(object sender, EventArgs e)
        {
           
        }

        private void btnNextStep_Click(object sender, EventArgs e)
        {
           
            //SaveData.Project(Global.Project);
            //btnNextStep.Enabled = false;
            //this.Parent.Controls.Remove(this);
            Global.EditTool.RefreshGuiEdit(Step.Step2);
            //Global.StepEdit.btnStep2.PerformClick();

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
            BeeCore.Common.listCamera[Global.IndexCCCD].Light(Global.ParaCommon.TypeLight, Global.Config.IsOnLight);

        }
       
      

       

        private void btnHD_Click(object sender, EventArgs e)
        {
        //    Global.ParaCommon.TypeResolution = 2;
        //    Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
        //   Global.Config.imgZoom = G.EditTool.View.imgView.Zoom;
        //   Global.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
        //   Global.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

        private void btn480_Click(object sender, EventArgs e)
        {
        //    Global.ParaCommon.TypeResolution = 3;
        //    Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
        //   Global.Config.imgZoom = G.EditTool.View.imgView.Zoom;
        //   Global.Config.imgOffSetX = G.EditTool.View.imgView.AutoScrollPosition.X;
        //   Global.Config.imgOffSetY = G.EditTool.View.imgView.AutoScrollPosition.Y;
        }

     
       
        private void btnInternal_Click(object sender, EventArgs e)
        {
           Global.Config.IsExternal =! btnInternal.IsCLick;
         
            //if (! Global.Config.IsExternal)
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
           Global.Config.IsExternal = btnExternal.IsCLick;
         
            //if (! Global.Config.IsExternal)
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
           
         
        }

        private void Global_LiveChanged(bool obj)
        {
            //AdjWidth.Enabled =! obj;
           // AdjHeight.Enabled = !obj;
          // AdjOffsetX.Enabled = !obj;
          //  AdjOffSetY.Enabled = !obj;
           // btnCenterX.Enabled = !obj;
           // btnCenterY.Enabled = !obj;
           
        }

       

     
        private void AdDelayTrig_ValueChanged(float obj)
        {
            Global.Comunication.Protocol.DelayTrigger = (int)AdDelayTrig.Value;
        }

        private void AdDelayOutput_ValueChanged(float obj)
        {
            Global.Comunication.Protocol.DelayOutput= (int)AdDelayOutput.Value;
        }

        private void btnOFF_Click(object sender, EventArgs e)
        {
            Global.Config.IsOnLight = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].Light(Global.ParaCommon.TypeLight, Global.Config.IsOnLight);

        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            Global.Config.IsOnLight =true;
            BeeCore.Common.listCamera[Global.IndexCCCD].Light(Global.ParaCommon.TypeLight, Global.Config.IsOnLight);

        }

        private void btnLight1_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.IsLight1 = btnLight1.IsCLick;
        }

        private void btnLight2_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.IsLight1 = btnLight2.IsCLick;
        }

        private void btnLight3_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.IsLight1 = btnLight3.IsCLick;
        }
       

      

      

      







        private async void btnDownLoadPara_Click(object sender, EventArgs e)
        {
            btnDownLoadPara.IsCLick = true;
            btnDownLoadPara.Enabled = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara= true;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera == TypeCamera.USB)
            {
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Zoom = BeeCore.Common.listCamera[Global.IndexCCCD].GetZoom();
                AdjZoom.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Zoom;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Focus = BeeCore.Common.listCamera[Global.IndexCCCD].GetFocus();
                AdjFocus.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Focus;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value = BeeCore.Common.listCamera[Global.IndexCCCD].GetWidthUSB();
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value = BeeCore.Common.listCamera[Global.IndexCCCD].GetHeightUSB();
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value;
              await BeeCore.Common.listCamera[Global.IndexCCCD].GetExpo();
                trackExposure.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value;
            }
            else
            {
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetExpo();
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetGain();
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetShift();
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetWidth();
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetHeight();
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetOffSetX();
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetOffSetY();
                //await BeeCore.Common.listCamera[Global.IndexCCCD].GetCenterX();
                //await BeeCore.Common.listCamera[Global.IndexCCCD].GetCenterY();
                if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera == TypeCamera.USB)
                {
                    BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Step = 1;
                    BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Min = 3;
                    BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Max = 12;
                }
                    trackExposure.IsInital = true;
                trackExposure.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Min;
                trackExposure.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Max;
                if (trackExposure.Max > Global.Config.LimitExposure)
                {

                    trackExposure.Max = Global.Config.LimitExposure;
                }
                if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value > Global.Config.LimitExposure)
                {
                    BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value = Global.Config.LimitExposure;
                }
                trackExposure.IsInital = true;
                trackExposure.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Step;
                trackExposure.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value;

                trackGain.IsInital = true;
                trackGain.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Min;
                trackGain.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Max;
                trackGain.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Step;
                trackGain.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Value;
                trackShift.IsInital = true;
                trackShift.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Min;
                trackShift.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Max;
                trackShift.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Step;
                trackShift.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Value;
                AdjWidth.IsInital = true;
                AdjWidth.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Min;
                AdjWidth.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Max;
                AdjWidth.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Step;
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value;
                AdjHeight.IsInital = true;
                AdjHeight.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Min;
                AdjHeight.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Max;
                AdjHeight.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Step;
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value;
                AdjOffsetX.IsInital = true;
                AdjOffsetX.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Min;
                AdjOffsetX.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Max;
                AdjOffsetX.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Step;
                AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Value;
                AdjOffSetY.IsInital = true;
                AdjOffSetY.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Min;
                AdjOffSetY.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Max;
                AdjOffSetY.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Step;
                AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Value;
            }
            //  btnCenterX.IsCLick = Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexCCCD].Para.CenterX);
            //  btnCenterY.IsCLick = Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexCCCD].Para.CenterY);
            lbErr.Text = BeeCore.Common.listCamera[Global.IndexCCCD].Err;
            btnDownLoadPara.IsCLick = false;
            btnDownLoadPara.Enabled = true;
            await TimingUtils.DelayAccurateAsync(50);
            BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara = false;
        }

        private  void btnCenterX_Click(object sender, EventArgs e)
        {
          
           // BeeCore.Common.listCamera[Global.IndexCCCD].Para.CenterX =Convert.ToInt32( btnCenterX.IsCLick);
           // await BeeCore.Common.listCamera[Global.IndexCCCD].SetCenterX();
           // btnCenterX.IsCLick =Convert.ToBoolean( BeeCore.Common.listCamera[Global.IndexCCCD].Para.CenterX);

        }

        private  void btnCenterY_Click(object sender, EventArgs e)
        {
           
        }

      
        private void btnBlink_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.TypeOutputRS = TypeOutputRS.Blink;
            layoutDelay.Enabled = true;
        }

        private void btnAllTime_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.TypeOutputRS = TypeOutputRS.AllTime;
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
            {
                Global.IsChange = true;
                Global.ScanCCD.TypeCameraNew = BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera;
                Global.ScanCCD.ShowDialog();
            }    
              
        
        }

        private void btnFocus_Click(object sender, EventArgs e)
        {
           
            BeeCore.Common.listCamera[Global.IndexCCCD].SetAutoFocus(btnFocus.IsCLick);
        }

        private void AdjZoom_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Zoom = (int)AdjZoom.Value;
            BeeCore.Common.listCamera[Global.IndexCCCD].SetZoom();
        }

        private void AdjFocus_ValueChanged(float obj)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Focus = (int)AdjFocus.Value;
            BeeCore.Common.listCamera[Global.IndexCCCD].SetFocus();
        }

        private void cbFormat_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private async void AdjWidth_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value = (int)AdjWidth.Value;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera == TypeCamera.USB)
            {
                BeeCore.Common.listCamera[Global.IndexCCCD].SetWidthUSB(); 
                AdjWidth.IsInital = true;
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value;
            }
            else
            {
                await BeeCore.Common.listCamera[Global.IndexCCCD].SetWidth();
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value;
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetOffSetX();
                AdjOffsetX.IsInital = true;
                AdjOffsetX.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Min;
                AdjOffsetX.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Max;
                AdjOffsetX.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Step;
                AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Value;
            }
        }

        private async void AdjHeight_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value = (int)AdjHeight.Value;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera == TypeCamera.USB)
            {
                BeeCore.Common.listCamera[Global.IndexCCCD].SetHeightUSB();
                AdjHeight.IsInital = true;
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value;
            }    
               
            else
            {
                await BeeCore.Common.listCamera[Global.IndexCCCD].SetHeight();
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value;
                await BeeCore.Common.listCamera[Global.IndexCCCD].GetOffSetY();
                AdjOffSetY.IsInital = true;
                AdjOffSetY.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Min;
                AdjOffSetY.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Max;
                AdjOffSetY.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Step;
              
                AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Value;
            }
        }
     
        private void trackExposure_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
        }

        private async void trackExposure_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value = (int)trackExposure.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetExpo();
            trackExposure.IsInital = true;
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value;
        }
        private async void trackExposure_ValueChanged(float obj)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara) return;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;

            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value = (int)trackExposure.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetExpo();
            trackExposure.IsInital = true;
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value;
        }

        private async void trackGain_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Value = (int)trackGain.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetGain();
            trackGain.IsInital = true;
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Value;
        }
        private async void trackGain_ValueChanged(float obj)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara) return;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Value = (int)trackGain.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetGain();
            trackGain.IsInital = true;
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Value;
        }
        private async void trackShift_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Value = (int)trackShift.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetShift();
            trackShift.IsInital = true;
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Value;
        }
     
      
       
        private void trackGain_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
        }
       
        private void trackShift_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;

        }
        private async void trackShift_ValueChanged(float obj)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara) return;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Value = (int)trackShift.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetShift();
            trackShift.IsInital = true;
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Value;
        }

        private void AdjWidth_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
        }

        private void AdjHeight_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
        }
        public void RefreshData()
        {
            AdjZoom.IsInital = true;
            AdjZoom.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Zoom;
            AdjFocus.IsInital = true;
            AdjFocus.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Focus;
            AdDelayTrig.IsInital = true;
            AdDelayTrig.Value = Global.Comunication.Protocol.DelayTrigger;
            if (AdDelayTrig.Max > Global.Config.LimitDelayTrigger)
            {
                AdDelayTrig.Max = Global.Config.LimitDelayTrigger;
            }
            AdDelayOutput.IsInital = true;
            Global.Config.IsOnLight = false;

            AdDelayOutput.Value = Global.Comunication.Protocol.DelayOutput;
            btnLight1.IsCLick = Global.Comunication.Protocol.IsLight1;
            btnLight2.IsCLick = Global.Comunication.Protocol.IsLight2;
            btnLight3.IsCLick = Global.Comunication.Protocol.IsLight3;
            btnOn.IsCLick = Global.Config.IsOnLight;
            btnInternal.IsCLick = !Global.Config.IsExternal;
            btnExternal.IsCLick = Global.Config.IsExternal;

            btnBlink.IsCLick = Global.Comunication.Protocol.TypeOutputRS == TypeOutputRS.Blink ? true : false;
            btnAllTime.IsCLick = Global.Comunication.Protocol.TypeOutputRS == TypeOutputRS.AllTime ? true : false;
            btnOKNG.IsCLick = Global.Comunication.Protocol.TypeOutputRS == TypeOutputRS.OKNG ? true : false;


            layoutDelay.Enabled = Global.Comunication.Protocol.TypeOutputRS == TypeOutputRS.Blink ? true : false;
            layDelayOKNG.Enabled = Global.Comunication.Protocol.TypeOutputRS == TypeOutputRS.OKNG ? true : false;
            numDelayOK.Value = Global.Comunication.Protocol.DelayOutOK;
            numDelayNG.Value = Global.Comunication.Protocol.DelayOutNG;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera == TypeCamera.USB)
            {
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Min = 320;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Min = 240;

                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Step = 1;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Step = 1;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Max = 3000;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Max = 3000;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Step = 1;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Min = 3;
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Max = 12;
                //   trackExposure.Enabled = false;
                trackGain.Enabled = false;
                trackShift.Enabled = false;
                AdjOffsetX.Enabled = false;
                AdjOffSetY.Enabled = false;
                btnCenterX.Enabled = false;
                btnCenterY.Enabled = false;
                AdjFocus.Enabled = true;
                AdjZoom.Enabled = true;
                btnFocus.Enabled = true;
            }
            else
            {
                btnFocus.Enabled = false;
                trackGain.Enabled = true;
                trackShift.Enabled = true;
                AdjOffsetX.Enabled = true;
                AdjOffSetY.Enabled = true;
                btnCenterX.Enabled = true;
                btnCenterY.Enabled = true;
                AdjFocus.Enabled = false;
                AdjZoom.Enabled = false;

            }
            trackExposure.IsInital = true;
            trackExposure.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Min;
            trackExposure.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Max;

            if (trackExposure.Max > Global.Config.LimitExposure)
            {

                trackExposure.Max = Global.Config.LimitExposure;
            }
            if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value > Global.Config.LimitExposure)
            {
                BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value = Global.Config.LimitExposure;
            }

            trackExposure.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Step;
            trackExposure.IsInital = true;
            trackExposure.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Exposure.Value;

            trackGain.IsInital = true;
            trackGain.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Min;
            trackGain.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Max;
            trackGain.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Step;
            trackGain.IsInital = true;
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Value;
            trackShift.IsInital = true;
            trackShift.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Min;
            trackShift.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Max;
            trackShift.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Step;
            trackShift.IsInital = true;
            trackShift.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Shift.Value;
            AdjWidth.IsInital = true;
            AdjWidth.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Min;
            AdjWidth.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Max;
            AdjWidth.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Step;
            AdjWidth.IsInital = true;
            AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value;
            AdjHeight.IsInital = true;
            AdjHeight.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Min;
            AdjHeight.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Max;
            AdjHeight.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Step;
            AdjHeight.IsInital = true;
            AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value;
            AdjOffsetX.IsInital = true;
            AdjOffsetX.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Min;
            AdjOffsetX.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Max;
            AdjOffsetX.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Step;
            AdjOffsetX.IsInital = true;
            AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Value;
            AdjOffSetY.IsInital = true;
            AdjOffSetY.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Min;
            AdjOffSetY.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Max;
            AdjOffSetY.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Step;
            AdjOffSetY.IsInital = true;
            AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Value;
            btnCenterX.IsCLick = Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexCCCD].Para.CenterX);
            btnCenterY.IsCLick = Convert.ToBoolean(BeeCore.Common.listCamera[Global.IndexCCCD].Para.CenterY);


        }

        private void SettingStep1_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                RefreshData();
                btnInternal.IsCLick = !Global.Config.IsExternal;
                btnExternal.IsCLick = Global.Config.IsExternal;
            }    
         
        }

        private async void  AdjOffsetX_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Value = (int)AdjOffsetX.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetOffSetX();
            AdjOffsetX.IsInital = true;
            AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Value;
        }

        private void AdjOffsetX_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
        }
        private async void AdjOffsetX_ValueChanged(float obj)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara) return;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;

            BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Value = (int)AdjOffsetX.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetOffSetX();
            AdjOffsetX.IsInital = true;
            AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Value;

         

        }
        private void AdjOffSetY_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
        }

        private async void AdjOffSetY_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Value = (int)AdjOffSetY.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetOffSetY();
            AdjOffSetY.IsInital = true;
            AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Value;
        }
        private async void AdjOffSetY_ValueChanged(float obj)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara) return;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Value = (int)AdjOffSetY.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetOffSetY();
            AdjOffSetY.IsInital = true;
            AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Value;


        }

        private void btnCenterX_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
        }

        private void btnCenterX_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            AdjOffsetX.Value = AdjOffsetX.Max / 2;
        }

        private void btnCenterY_MouseUp(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            AdjOffSetY.Value = AdjOffSetY.Max / 2;
        }

        private void btnCenterY_MouseDown(object sender, MouseEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
        }

        private async void AdjWidth_ValueChanged(float obj)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara) return;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value = (int)AdjWidth.Value;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera == TypeCamera.USB)
            {
                BeeCore.Common.listCamera[Global.IndexCCCD].SetWidthUSB();
                AdjWidth.IsInital = true;
                AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value;
            }
            else
            {
                await BeeCore.Common.listCamera[Global.IndexCCCD].SetWidth();
                if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value != AdjWidth.Value)
                {
                    AdjWidth.IsInital = true;
                    AdjWidth.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Width.Value;

                }

                await BeeCore.Common.listCamera[Global.IndexCCCD].GetOffSetX();
                AdjOffsetX.IsInital = true;
                AdjOffsetX.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Min;
                AdjOffsetX.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Max;
                AdjOffsetX.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Step;
                AdjOffsetX.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetX.Value;
            }
        }

        private async void AdjHeight_ValueChanged(float obj)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara) return;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value = (int)AdjHeight.Value;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.TypeCamera == TypeCamera.USB)
            {
                BeeCore.Common.listCamera[Global.IndexCCCD].SetHeightUSB();
                AdjHeight.IsInital = true;
                AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value;
            }
            else
            {
                await BeeCore.Common.listCamera[Global.IndexCCCD].SetHeight();
                if (BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value != AdjHeight.Value)
                {
                    AdjHeight.IsInital = true;
                    AdjHeight.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Height.Value;

                }

                await BeeCore.Common.listCamera[Global.IndexCCCD].GetOffSetY();
                AdjOffSetY.IsInital = true;
                AdjOffSetY.Min = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Min;
                AdjOffSetY.Max = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Max;
                AdjOffSetY.Step = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Step;
                AdjOffSetY.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.OffSetY.Value;
            }
        }

        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(Global.IsLive)
            //{
            //    BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
            //    await TimingUtils.DelayAccurateAsync(1000);
            //    BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;
            //}
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            AdjZoom.Visible = !btn1.IsCLick;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            lay2.Visible = !btn2.IsCLick;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            trackExposure.Visible = !btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            trackGain.Visible = !btn4.IsCLick;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            trackShift.Visible = !btn5.IsCLick;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            AdjWidth.Visible = !btn6.IsCLick;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            AdjHeight.Visible = !btn7.IsCLick;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            AdjOffsetX.Visible = !btn8.IsCLick;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            AdjOffSetY.Visible = !btn9.IsCLick;
        }

        private void btn21_Click(object sender, EventArgs e)
        {
            lay21.Visible=!btn21.IsCLick;
        }

        private void btn22_Click(object sender, EventArgs e)
        {
           AdDelayTrig.Visible = !btn22.IsCLick;
        }

        private void btn23_Click(object sender, EventArgs e)
        {
            lay23.Visible = !btn23.IsCLick;
            layoutDelay.Visible=!btn23.IsCLick;
            layDelayOKNG.Visible = !btn23.IsCLick;
        }

        private void btn24_Click(object sender, EventArgs e)
        {
            lay241.Visible = !btn24.IsCLick;
            lay242.Visible = !btn24.IsCLick;
        }

        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnOKNG_Click(object sender, EventArgs e)
        {
            Global.Comunication.Protocol.TypeOutputRS = TypeOutputRS.OKNG;
            layDelayOKNG.Enabled = true;
        }

        private void numDelayOK_ValueChanged(float obj)
        {
            Global.Comunication.Protocol.DelayOutOK =(int) numDelayOK.Value;
        }

        private void numDelayNG_ValueChanged(float obj)
        {
            Global.Comunication.Protocol.DelayOutNG = (int)numDelayNG.Value;
        }

        private async void btnSaveCamera_Click(object sender, EventArgs e)
        {
            btnSaveCamera.Enabled = false;
            using (var dlg = new SaveProgressDialog("Save Para Camera"))
            {
                dlg.SetStatus("Saving  Para Camera " + "...", "Writing data to file...");
             
                dlg.Show(this);          // modeless
                //dlg.BringToFront();

                try
                {
                    await Task.Run(() =>
                    {
                        SaveData.Camera(Global.Project,Global.listParaCamera);
                    });

                    if (dlg.CancelRequested)
                    {
                        dlg.SetStatus("Cancelled", "You have cancelled the save operation.");
                        dlg.MarkCompleted("Cancelled", "No data was written.");
                    }
                    else
                    {
                        btnSaveCamera.Enabled = true;
                        dlg.MarkCompleted("Save completed", "Para Camera " );
                    }
                }
                catch (Exception ex)
                {
                    dlg.SetStatus("Save error", ex.Message);
                    dlg.MarkCompleted("Error", "Please click OK to close.");
                }

            }
        }
    }
}
