using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Funtion.Engines;
using BeeGlobal;
using System;

namespace BeeInterface
{
    public partial class ToolCircle
    {
        private void ToolCircle_ScoreChanged(float obj)
        {
            trackScore.Value = obj;
        }

        private void ToolCircle_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
           if(OwnerTool.StatusTool==StatusTool.Done)
                if (Propety.IsCalibs)
                {
                    btnCalib.IsCLick = false;
                    Propety.IsCalibs = false;
                    btnCalib.Enabled = true;
                    trackMinInlier.Value = Propety.MinInliers;
                    numMaxRadius.Value = Propety.MaxRadius;
                    numMinRadius.Value = Propety.MinRadius;
                }
        }

        private void trackScore_ValueChanged(float obj)
        {
            CircleEngineRunner.ApplyScoreToOwner(OwnerTool, trackScore.Value);
          
        }

        private void trackIterations_ValueChanged(float obj)
        {
            Propety.Iterations= (int)trackIterations.Value;
        }

        private void trackMinInlier_ValueChanged(float obj)
        {
            Propety.MinInliers= (int)trackMinInlier.Value;
        }

        private void trackThreshold_ValueChanged(float obj)
        {
            Propety.Threshold= (float)trackThreshold.Value;
        }

        private void btnInsideOut_Click(object sender, EventArgs e)
        {
            Propety.CircleScanDirection = BeeCore.Algorithm.CircleScanDirection.InsideOut;
        }

        private void btnOutsideIn_Click(object sender, EventArgs e)
        {
            Propety.CircleScanDirection = BeeCore.Algorithm.CircleScanDirection.OutsideIn;
        }

        private void btnCloseEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.CloseEdges; lay32.Enabled = false;
        }

        private void btnStrongEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.StrongEdges; lay32.Enabled = false;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
          
            Propety.IsCalibs = true;
            btnCalib.Enabled = false;
            Common.TryGetTool(Global.IndexToolSelected).RunToolAsync();
        }

        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            lay32.Enabled = true;
        }

        private void trackIterations_Load(object sender, EventArgs e)
        {

        }

        private void numMinRadius_ValueChanged_1(float obj)
        {
            Propety.MinRadius = numMinRadius.Value;
        }

        private void numMaxRadius_ValueChanged(float obj)
        {
            Propety.MaxRadius = numMaxRadius.Value;
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.InvertBinary;
            lay32.Enabled = true;
        }

        private void AdjThreshod_ValueChanged(float obj)
        {
            Propety.ThresholdBinary =(int) AdjThreshod.Value;

        }
      
        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall = (int)AdjClearNoise.Value;
        }

        private void btnIsClose_Click(object sender, EventArgs e)
        {
            Propety.IsClose = btnClose.IsCLick;
            AdjMorphology.Enabled = Propety.IsClose;
        }

        private void btnIsClearSmall_Click(object sender, EventArgs e)
        {

            Propety.IsClearNoiseSmall = btnIsClearSmall.IsCLick;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
        }

        private void AdjMorphology_ValueChanged(float obj)
        {

            Propety.SizeClose = (int)AdjMorphology.Value;
        }


        private void AdjOpen_ValueChanged(float obj)
        {
            Propety.SizeOpen = (int)AdjOpen.Value;
        }

        private void AdjClearBig_ValueChanged(float obj)
        {
            Propety.SizeClearBig = (int)AdjClearBig.Value;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Propety.IsOpen = btnOpen.IsCLick;
            AdjOpen.Enabled = Propety.IsOpen;
        }

        private void btnIsClearBig_Click(object sender, EventArgs e)
        {
            Propety.IsClearNoiseBig = btnIsClearBig.IsCLick;
            AdjClearBig.Enabled = Propety.IsClearNoiseBig;

        }

        private void AdjScale_ValueChanged(float obj)
        {
            Propety.Scale=AdjScale.Value;
        }

        private void lb1_Click(object sender, EventArgs e)
        {
            //Lay1.Visible=lb1
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Lay1.Visible = !btn1.IsCLick;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            Lay2.Visible = !btn2.IsCLick;
            Lay21.Visible = !btn2.IsCLick;
            Lay22.Visible = !btn2.IsCLick;
            Lay23.Visible = !btn2.IsCLick;
            Lay24.Visible = !btn2.IsCLick;

        }

        private void btn3_Click(object sender, EventArgs e)
        {
            lay31.Visible=!btn3.IsCLick;
            lay32.Visible=!btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            AdjScale.Visible = !btn4.IsCLick;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn5.IsCLick;
        }
    }
}

