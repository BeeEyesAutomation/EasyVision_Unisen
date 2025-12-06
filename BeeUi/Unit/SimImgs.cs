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
            if (Global.ParaCommon.listSimImg == null) Global.ParaCommon.listSimImg = new List<ItemRegsImg>();
           registerImg.LoadAllItem(Global.ParaCommon.listSimImg);
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
                // phần Global của bạn — giữ nguyên
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = clone.Clone();
              //  Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                Global.ParaCommon.SizeCCD = new System.Drawing.Size(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Height);
               // Global.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                
            }

        }

        private void RegisterImgs_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                this.Location = new Point(Global.EditTool.View.Width - this.Width, Global.EditTool.View.pBtn.Height + 1);

               // registerImg.LoadAllItem((BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].IndexImgRegis));

            }
        }

        private void btnCap_Click(object sender, EventArgs e)
        {
            if (registerImg.IndexSelected == -1) return;
            Global.EditTool.View.timer = CycleTimerSplit.Start();
            Global.TriggerNum = TriggerNum.Trigger1;
            Global.StatusProcessing = StatusProcessing.Checking;
        }
       
        private void btnContinuous_Click(object sender, EventArgs e)
        {
            Global.IsSim = btnContinuous.IsCLick;
            Global.StatusMode = btnContinuous.IsCLick ? StatusMode.SimContinuous : StatusMode.None;
            btnCap.Enabled = !btnContinuous.IsCLick; 
            if (Global.StatusMode == StatusMode.SimContinuous)
            {
                
                Global.EditTool.View.listMat = new List<Mat>();
                foreach (ImgItem img in registerImg._items)
                {
                    Global.EditTool.View.listMat.Add(img.Image);
                }
                btnContinuous.Image = Properties.Resources.Stop;
                if (Global.EditTool.View.indexFile >= Global.EditTool.View.listMat.Count) Global.EditTool.View.indexFile = 0;
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Global.EditTool.View.listMat[Global.EditTool.View.indexFile].Clone();// Cv2.ImRead(Files[indexFile]);
               Global.EditTool.View.timer = CycleTimerSplit.Start();
                Global.TriggerNum = TriggerNum.Trigger1;
                Global.StatusProcessing = StatusProcessing.Checking;

                if (Global.EditTool.View.indexFile >= Global.EditTool.View.Files.Count)
                    Global.EditTool.View.indexFile = 0;
            }
            else
                btnContinuous.Image = Properties.Resources.Play_2;
        }

        private void numDelay_ValueChanged(float obj)
        {
            Global.EditTool.View.tmSimulation.Interval =(int) numDelay.Value;
        }
    }
}
