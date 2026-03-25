using BeeCore;
using BeeGlobal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class QuickSetting : Form
    {
        public QuickSetting()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void QuickSetting_Load(object sender, EventArgs e)
        {
            if (BeeCore.Common.PropetyTools[0] != null)
                if (BeeCore.Common.PropetyTools[0].Count > 0)
                    if (BeeCore.Common.PropetyTools[0][0].TypeTool == TypeTool.MultiLearning)
                        multiOnnx = BeeCore.Common.PropetyTools[0][0].Propety2 as MultiOnnx;
                    else
                        multiOnnx = null;
                else
                    multiOnnx = null;
            else
                multiOnnx = null;
            if (multiOnnx != null)
            {
                AdjBinary.Value = multiOnnx.ThresholdBinary;
                AdjBinary.Visible = multiOnnx.MethordEdge == MethordEdge.Binary ? true : false;
             
                btnEdgeStrong.IsCLick = multiOnnx.MethordEdge == MethordEdge.StrongEdges ? true : false;
                btnBinary.IsCLick = multiOnnx.MethordEdge == MethordEdge.Binary ? true : false;
                btnEdgeNormal.IsCLick = multiOnnx.MethordEdge == MethordEdge.CloseEdges ? true : false;

                btnOnBlackDot.IsCLick = multiOnnx.IsBlackDot;
                btnOffBlackDot.IsCLick = !multiOnnx.IsBlackDot;
                AdjScoreBlack.Value = multiOnnx.ScoreYolo;
                AdjAspect.Value = multiOnnx.AspectBox;
                AdjLimitLeft.Value = multiOnnx.LimitXSub;
                AdjLimitRight.Value = multiOnnx.LimitX;
                AdjLimitY.Value = multiOnnx.LimitY;
            }
            this.Width = Global.EditTool.BtnHeaderBar.Width + 1;
            //  this.Height = Global.SizeScreen.Height - Global.EditTool.BtnHeaderBar.Height - 20;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - Global.EditTool.BtnHeaderBar.Width - 1, Global.EditTool.pTop.Height);// Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
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
            AdDelayTrig.Value = Global.Comunication.Protocol.DelayTrigger;
            if (AdDelayTrig.Max > Global.Config.LimitDelayTrigger)
            {
                AdDelayTrig.Max = Global.Config.LimitDelayTrigger;
            }
        }

        private void AdDelayTrig_ValueChanged(float obj)
        {
            Global.Comunication.Protocol.DelayTrigger = (int)AdDelayTrig.Value;
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

        private async void trackGain_ValueChanged(float obj)
        {
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara) return;
            if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;
            BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Value = (int)trackGain.Value;
            await BeeCore.Common.listCamera[Global.IndexCCCD].SetGain();
            trackGain.IsInital = true;
            trackGain.Value = BeeCore.Common.listCamera[Global.IndexCCCD].Para.Gain.Value;
        }
        MultiOnnx multiOnnx = new MultiOnnx();
        private void AdjAspect_ValueChanged(float obj)
        {
           if(multiOnnx!=null)
            {
                multiOnnx.AspectBox=AdjAspect.Value;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }    
           
        }

        private void AdjLimitLeft_ValueChanged(float obj)
        {
            if (multiOnnx != null)
            {
                multiOnnx.LimitXSub = AdjLimitLeft.Value;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
        }

        private void AdjLimitRight_ValueChanged(float obj)
        {
            if (multiOnnx != null)
            {
                multiOnnx.LimitX = AdjLimitRight.Value;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
        }

        private void AdjLimitY_ValueChanged(float obj)
        {
            if (multiOnnx != null)
            {
                multiOnnx.LimitY = AdjLimitY.Value;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
        }

        private void AdjScoreBlack_ValueChanged(float obj)
        {
            if (multiOnnx != null)
            {
                multiOnnx.ScoreYolo =(int) AdjScoreBlack.Value;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
        }

        private void btnOnBlackDot_Click(object sender, EventArgs e)
        {
            if (multiOnnx != null)
            {
                multiOnnx.IsBlackDot = btnOnBlackDot.IsCLick;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
          
        }

        private void btnOffBlackDot_Click(object sender, EventArgs e)
        {
            if (multiOnnx != null)
            {
                multiOnnx.IsBlackDot =!btnOffBlackDot.IsCLick;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            using (var dlg = new SaveProgressDialog("Save Program"))
            {
                dlg.SetStatus("Saving Program " + Global.Project + "...", "Writing data to file...");
                dlg.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - dlg.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - dlg.Height / 2);

                dlg.Show(this);          // modeless
                //dlg.BringToFront();

                try
                {
                    await Task.Run(() =>
                    {
                        SaveData.Project(Global.Project);
                    });

                    if (dlg.CancelRequested)
                    {
                        dlg.SetStatus("Cancelled", "You have cancelled the save operation.");
                        dlg.MarkCompleted("Cancelled", "No data was written.");
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        dlg.MarkCompleted("Save completed", "Program " + Global.Project);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    dlg.SetStatus("Save error", ex.Message);
                    dlg.MarkCompleted("Error", "Please click OK to close.");
                }

            }
        }

        private void btnBinary_Click(object sender, EventArgs e)
        {
            if (multiOnnx != null)
            {
                multiOnnx.MethordEdge = MethordEdge.Binary;
                AdjBinary.Visible = multiOnnx.MethordEdge == MethordEdge.Binary ? true : false;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
        }

        private void btnEdgeStrong_Click(object sender, EventArgs e)
        {
            if (multiOnnx != null)
            {
                multiOnnx.MethordEdge = MethordEdge.StrongEdges;
                AdjBinary.Visible = multiOnnx.MethordEdge == MethordEdge.Binary ? true : false;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }

        }

        private void AdjBinary_ValueChanged(float obj)
        {
            if (multiOnnx != null)
            {
                multiOnnx.ThresholdBinary =(int) AdjBinary.Value;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
        }

        private void btnEdgeNormal_Click(object sender, EventArgs e)
        {
            if (multiOnnx != null)
            {
                multiOnnx.MethordEdge = MethordEdge.CloseEdges;
                AdjBinary.Visible = multiOnnx.MethordEdge == MethordEdge.Binary ? true : false;
                BeeCore.Common.PropetyTools[0][0].Propety2 = multiOnnx;
            }
          
        }
    }
}
