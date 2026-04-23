using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace BeeUi.Unit
{
    public partial class SimImgs : UserControl
    {
        public SimImgs()
        {
            InitializeComponent();
        }

        private void RegisterImgs_Load(object sender, EventArgs e)
        {
            this.Location = new Point( Global.EditTool.View.Width - this.Width,Global.EditTool.View.pBtn.Height + 1);
            if (Global.listSimImg == null) Global.listSimImg = new List<ItemRegsImg>();
           registerImg.LoadAllItem(Global.listSimImg);
            numDelay.Value = 5000;


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
          
            Global.EditTool.View.btnChangeImg.Enabled = true;
         
        }

        private void registerImg_SelectedItemChanged(object sender, BeeInterface.RegisterImgSelectionChangedEventArgs e)
        {if (e.Image == null) return;
            if (e.Image.Empty()) return;
            using (Mat clone = e.Image?.Clone())
            {
                // ph?n Global c?a b?n — gi? nguyên
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = clone.Clone();
              //  Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                Global.Config.SizeCCD = new System.Drawing.Size(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Height);
               // Global.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone();
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
                
            }

        }

        private void RegisterImgs_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                this.Location = new Point(Global.EditTool.View.Width - this.Width, Global.EditTool.View.pBtn.Height + 1);

               // registerImg.LoadAllItem((BeeCore.Common.TryGetTool(Global.IndexProgChoose, Global.IndexToolSelected).IndexImgRegis));

            }
        }

        private void btnCap_Click(object sender, EventArgs e)
        {
            if (registerImg.IndexSelected == -1) return;
            Global.EditTool.View.timer = CycleTimerSplit.Start();
            if (Global.Comunication.Protocol.IsBypass&&Global.Config.NumTrig>1)
            {
                switch (Global.TriggerNum)
                {
                    case TriggerNum.Trigger0:
                        Global.TriggerNum = TriggerNum.Trigger1;
                        Global.IndexProgChoose = 0;
                        break;
                    case TriggerNum.Trigger1:
                        Global.TriggerNum = TriggerNum.Trigger2;
                        if (BeeCore.Common.TryGetToolList(1) != null)
                            Global.IndexProgChoose = 1;
                        else
                        {
                            Global.TriggerNum = TriggerNum.Trigger1;
                            Global.IndexProgChoose = 0;


                        }

                        break;
                    case TriggerNum.Trigger2:
                        Global.TriggerNum = TriggerNum.Trigger3;

                        if (BeeCore.Common.TryGetToolList(2) != null)
                            Global.IndexProgChoose = 2;
                        else
                        {
                            Global.TriggerNum = TriggerNum.Trigger1;
                            Global.IndexProgChoose = 0;


                        }

                        break;
                    case TriggerNum.Trigger3:
                        Global.TriggerNum = TriggerNum.Trigger4;
                        if (BeeCore.Common.TryGetToolList(3) != null)
                            Global.IndexProgChoose = 3;
                        else
                        {
                            Global.TriggerNum = TriggerNum.Trigger1;
                            Global.IndexProgChoose = 0;


                        }

                        break;
                }
                // Global.StatusProcessing = StatusProcessing.Read;
            }
            else
                Global.TriggerNum = TriggerNum.Trigger1;
            Global.IsSim = true;
             Global.StatusProcessing = StatusProcessing.Checking;
        }
       
        private void btnContinuous_Click(object sender, EventArgs e)
        {
            Global.IsSim = btnContinuous.IsCLick;
            Global.StatusMode = btnContinuous.IsCLick ? StatusMode.SimContinuous : StatusMode.None;
            btnCap.Enabled = !btnContinuous.IsCLick; 
            if (Global.StatusMode == StatusMode.SimContinuous)
            {

              //  Global.listSimImg = new List<ItemRegsImg>();
              //  foreach (ImgItem img in registerImg._items)
              //  {
              ////      Global.listSimImg.Add(new ItemRegsImg("", img.Image);
              //  }
                btnContinuous.Image = BeeInterface. Properties.Resources.Stop;
                if (Global.EditTool.View.indexFile >= Global.listSimImg.Count) Global.EditTool.View.indexFile = 0;
                ItemRegsImg itemRegsImg = Global.listSimImg[Global.EditTool.View.indexFile];
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = itemRegsImg.Image.ToMat();// Cv2.ImRead(Files[indexFile]);
               Global.EditTool.View.timer = CycleTimerSplit.Start();
                Global.TriggerNum = TriggerNum.Trigger1;
                Global.StatusProcessing = StatusProcessing.Checking;

            
            }
            else
                btnContinuous.Image = BeeInterface.Properties.Resources.Play_2;
        }

        private void numDelay_ValueChanged(float obj)
        {
            Global.EditTool.View.tmSimulation.Interval =(int) numDelay.Value;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            Global.StatusMode = btnSimCam.IsCLick ? StatusMode.SimCam : StatusMode.None;
        }
    }
}
