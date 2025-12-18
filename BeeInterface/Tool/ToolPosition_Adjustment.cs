using BeeCore;
using BeeGlobal;
using BeeInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class ToolPosition_Adjustment : UserControl
    {
        
        public ToolPosition_Adjustment( )
        {
            InitializeComponent();
          
        }
        public void LoadPara()
        {
            if (Propety.bmRaw != null)
            {
                imgTemp.Image = Propety.bmRaw;
            }
            if (Propety.rotPositionAdjustment != null)
               Global.rotOriginAdj = new RectRotate(Propety.rotCrop._rect, new PointF(Propety.rotArea._PosCenter.X - Propety.rotArea._rect.Width / 2 + Propety.rotPositionAdjustment._PosCenter.X, Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + Propety.rotPositionAdjustment._PosCenter.Y), Propety.rotPositionAdjustment._rectRotation, AnchorPoint.None);
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            trackAngle.Value = (int)Propety.Angle;
            if (Propety.Angle > 360) Propety.Angle = 360;

            if (Propety.Angle == 0)
            {
                Propety.Angle = 1;
            }
            float angle = (Propety.rotCrop._rectRotation) - (Propety.rotArea._rectRotation);
            Propety.AngleLower = angle - Propety.Angle;
            Propety.AngleUper = angle + Propety.Angle;
            trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
            Propety.ckBitwiseNot = Propety.ckBitwiseNot;
            Propety.ckSIMD = Propety.ckSIMD;
            Propety.ckSubPixel = Propety.ckSubPixel;
            ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            ckSIMD.IsCLick = Propety.ckSIMD;
            ckSubPixel.IsCLick = Propety.ckSubPixel;
            AdjMorphology.Value = Propety.SizeClose;
            AdjOpen.Value = Propety.SizeOpen;
            AdjClearNoise.Value = Propety.SizeClearsmall;
            AdjClearBig.Value = Propety.SizeClearBig;

            btnClose.IsCLick = Propety.IsClose;
            btnOpen.IsCLick = Propety.IsOpen;
            btnIsClearSmall.IsCLick = Propety.IsClearNoiseSmall;
            btnIsClearBig.IsCLick = Propety.IsClearNoiseBig;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
            AdjClearBig.Enabled = Propety.IsClearNoiseBig;
            AdjOpen.Enabled = Propety.IsOpen;
            AdjMorphology.Enabled = Propety.IsClose;
            AdjStepAngle.Value = Propety.StepAngle;
            AdjMaximumObj.Value = Propety.MaxObject;
            switch (Propety.MethodSample)
            {
                case MethodSample.Pattern:
                    layThreshod.Visible = false;
                    layEdge.Visible = false;
                    lbEdge.Visible = false;
                    btnPattern.IsCLick = true;
                    layPattern.Visible = true;
                    lbPattern.Visible = true;
                    lbAngle.Visible = true;
                    trackAngle.Visible = true;
                    break;
                case MethodSample.Corner:
                    layThreshod.Visible = true;
                    layEdge.Visible = true;
                    lbEdge.Visible = true;
                    btnCorner.IsCLick = true;
                    layPattern.Visible = false;
                    lbPattern.Visible = false;
                    lbAngle.Visible = false;
                    trackAngle.Visible = false;
                    break;
                case MethodSample.Edge:
                    layThreshod.Visible = true;
                    layEdge.Visible = true;
                    lbEdge.Visible = true;
                    btnEdge.IsCLick = true;
                    layPattern.Visible = false;
                    lbPattern.Visible = false;
                    lbAngle.Visible = false;
                    trackAngle.Visible = false;
                    break;
            }

            if (Propety.IsHighSpeed)
                btnHighSpeed.IsCLick = true;
            else
                btnNormal.IsCLick = true;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolPosition_Adjustment_StatusToolChanged;
            btnCropArea.IsCLick = true;
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            btnAutoMean.IsCLick = Propety.AutoMean == true ? true : false;
            btnFixedMean.IsCLick = Propety.AutoMean == false ? true : false;
            AdjContinuityGapFactor.Value = Propety.ContinuityGapFactor;
            AdjFixedMean.Value = Propety.FixMean;
            AdjAngleTargetDeg.Value = Propety.AngleTargetDeg;
            AdjAngleToleranceDeg.Value = Propety.AngleToleranceDeg;

            trackMinInlierA.Value = Propety.MinInliersA;
            trackMinInlierB.Value = Propety.MinInliersB;
            trackMaxLine.Value = Propety.MaximumLine;
            AdjRANSACIterations.Value = Propety.RansacIterations;
            AdjRANSACThreshold.Value = (float)Propety.RansacThreshold;

        }

        private void ToolPosition_Adjustment_StatusToolChanged(StatusTool obj)
        {
            if (Global.IsRun) return;
           
            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true; btnTest.IsCLick = false;
                if (Propety.IsCalib)
                {
                    btnCalib.IsCLick = false;
                    Propety.IsCalib = false;
                    btnCalib.Enabled = true;
                    trackMinInlierA.Value = Propety.MinInliersA;
                    trackMinInlierB.Value = Propety.MinInliersB;
                }
            }
           
        }

        public PositionAdj Propety=new PositionAdj();
     
        
        private void btnCropRect_Click(object sender, EventArgs e)
        {
            Global.TypeCrop= TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
          Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

        }

        public RJButton ButtonPress( RJButton control ,Control parent)
        {
            foreach (Control con in parent.Controls)
            {
                if(con==control)
                {
                    control.BackColor = Color.Goldenrod;
                    control.BorderColor = Color.DarkGoldenrod;
                }  
                else
                {
                    RJButton btn = con as RJButton;
                    btn.BackColor = Color.WhiteSmoke;
                    btn.BorderColor = Color.Silver;
                    btn.TextColor = Color.Black;
                }    
            }
            return control;


        }
 


     
    

       
        
       
       

       

        private void ckSIMD_Click(object sender, EventArgs e)
        {
            Propety.ckSIMD = !Propety.ckSIMD;
              if(Propety.ckSIMD)
                {
                ckSIMD.BackColor = Color.Goldenrod;
                ckSIMD.BorderColor = Color.DarkGoldenrod;
                }
                else
                { 
                ckSIMD.BackColor = Color.WhiteSmoke;
                ckSIMD.BorderColor = Color.Silver;
                ckSIMD.TextColor = Color.Black;
                }
          
        }

        private void ckBitwiseNot_Click(object sender, EventArgs e)
        {
            Propety.ckBitwiseNot = !Propety.ckBitwiseNot;
            if (Propety.ckBitwiseNot)
            {
                ckBitwiseNot.BackColor = Color.Goldenrod;
                ckBitwiseNot.BorderColor = Color.DarkGoldenrod;
            }
            else
            {
                ckBitwiseNot.BackColor = Color.WhiteSmoke;
                ckBitwiseNot.BorderColor = Color.Silver;
                ckBitwiseNot.TextColor = Color.Black;
            }
            
        }

        private void ckSubPixel_Click(object sender, EventArgs e)
        {
            Propety.ckSubPixel = !Propety.ckSubPixel;
            if (Propety.ckSubPixel)
            {
                ckSubPixel.BackColor = Color.Goldenrod;
                ckSubPixel.BorderColor = Color.DarkGoldenrod;
            }
            else
            {
                ckSubPixel.BackColor = Color.WhiteSmoke;
                ckSubPixel.BorderColor = Color.Silver;
                ckSubPixel.TextColor = Color.Black;
            }
           
        }
       
       
      
        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();
    
            private void ToolOutLine_Load(object sender, EventArgs e)
        {
            


        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
        public bool IsFullSize=false;
        public    bool IsClear = false;
        //public Eraser Eraser;
        //private void btnClear_Click(object sender, EventArgs e)
        //{
          
        //    if (Eraser == null)
        //    {
               
        //        Eraser = new Eraser();
        //        Eraser.Parent =this;
        //        Eraser.BringToFront();
        //        Eraser.Location = new System.Drawing.Point(this.Width / 2 - Eraser.Width / 2, this.Height / 2 - Eraser.Height / 2);
        //    }
           
       
       

        //    //btnClear.IsCLick = !IsClear;
        //    //IsClear = btnClear.IsCLick;
        //    //if(IsClear)
        //    //{
        //    //    tmClear.Enabled = true;
        //    //    Eraser.Show();
        //    //    G.EditTool.View.Cursor = new Cursor(Properties.Resources.Erase1.Handle);
        //    //   Global.TypeCrop= TypeCrop.Crop;
        //    //    G.IsCheck = false;
        //    //    G.EditTool.View.imgView.Invalidate();
        //    //}
        //    //else
        //    //    Eraser.Hide();




        //    G.EditTool.View.imgView.Invalidate();
         

        //}

        //private void btnUndo_Click(object sender, EventArgs e)
        //{
        //    G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Cols, MatType.CV_8UC1);

        //}

        //private void tabPage3_Click(object sender, EventArgs e)
        //{
           
        //}

        private void tmClear_Tick(object sender, EventArgs e)
        {
            
        }

     

        private void btnNormal_Click(object sender, EventArgs e)
        {
         Propety. IsHighSpeed = false;
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = true;

        }

        private void btnMask_Click(object sender, EventArgs e)
        {
           
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.StatusDraw = StatusDraw.Check;

        }

      
       

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
            //numScore.Value =Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;

        }

     

     
        private void btnLearning_Click(object sender, EventArgs e)
        {
          
                
                if (Propety.rotCrop != null)
                    if (Propety.rotCrop._rect.Width != 0 && Propety.rotCrop._rect.Height != 0)
                    {
                    Propety.bmRaw = Propety.LearnPattern( BeeCore.Common.listCamera[Propety.IndexThread].matRaw.Clone(),false).ToBitmap();
                    imgTemp.Image = Propety.bmRaw;
                    }
           
            
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
          
            if (!Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.IsBusy)
            {
                btnTest.Enabled = false;
                Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.RunWorkerAsync();

            }    
               
            else
                btnTest.IsCLick = false;
        }

    
        private void numScore_ValueChanged(object sender, EventArgs e)
        {

          //  Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
            //numScore.Value =Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
        }
        private void trackAngle_ValueChanged(float obj)
        {
            Propety.Angle = trackAngle.Value;

            if (Propety.Angle > 360) Propety.Angle = 360;

            if (Propety.Angle == 0)
            {
                Propety.Angle = 1;
            }
            float angle = (Propety.rotCrop._rectRotation) - (Propety.rotArea._rectRotation);
            Propety.AngleLower = angle - Propety.Angle;
            Propety.AngleUper = angle + Propety.Angle;
            
        }

   
        private void trackMaxOverLap_ValueChanged(float obj)
        {

            Propety.OverLap = (trackMaxOverLap.Value*1.0)/100.0;
          
        }

        

        private void btnCropHalt_Click(object sender, EventArgs e)
        {
            Global.StatusDraw = StatusDraw.None;
            Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            Global.StatusDraw = StatusDraw.None;
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.Edit;

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

        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall = (int)AdjClearNoise.Value;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Propety.IsClose = btnClose.IsCLick;
            AdjMorphology.Enabled = Propety.IsClose;
        }

        private void btnEnableNoise_Click(object sender, EventArgs e)
        {

            Propety.IsClearNoiseSmall = btnIsClearSmall.IsCLick;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
        }

        private void AdjMorphology_ValueChanged(float obj)
        {

            Propety.SizeClose = (int)AdjMorphology.Value;
        }
        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            layThreshod.Enabled = true;
        }



      

        private void AdjThreshod_ValueChanged(float obj)
        {
            Propety.ThresholdBinary = (int)AdjThreshod.Value;
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.InvertBinary;
            layThreshod.Enabled = true;
        }

        private void btnStrongEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.StrongEdges;
            layThreshod.Enabled = false;
        }

        private void btnCloseEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.CloseEdges;
            layThreshod.Enabled = false;
        }

        private void btnPattern_Click(object sender, EventArgs e)
        {
            Propety.MethodSample=MethodSample.Pattern;
            layThreshod.Visible =false;
            layEdge.Visible =false;
            lbEdge.Visible =false;
            layPattern.Visible = true;
            lbPattern.Visible = true;
            lbAngle.Visible = true;
            trackAngle.Visible = true;
        }

        private void btnCorner_Click(object sender, EventArgs e)
        {
            Propety.MethodSample = MethodSample.Corner;
            layThreshod.Visible = true;
            layEdge.Visible = true;
            lbEdge.Visible = true;
            layPattern.Visible = false;
            lbPattern.Visible = false;
            lbAngle.Visible = false;
            trackAngle.Visible = false;
        }

        private void btnEdge_Click(object sender, EventArgs e)
        {
            Propety.MethodSample = MethodSample.Edge;
            layPattern.Visible = false;
            lbPattern.Visible = false;
        }

        private void AdjMaximumObj_ValueChanged(float obj)
        {
            Propety.MaxObject = (int)AdjMaximumObj.Value;
        }

        private void AdjStepAngle_ValueChanged(float obj)
        {
            Propety.StepAngle = (int)AdjStepAngle.Value;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            lay1.Visible = !btn1.IsCLick;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            lay2.Visible=!btn2.IsCLick;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            lay3.Visible=!btn3.IsCLick;
            lbAngle.Visible = !btn3.IsCLick;
            trackAngle.Visible= !btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            trackAngle.Visible = !lbAngle.IsCLick;
        }

        private void btn4_Click_1(object sender, EventArgs e)
        {
            layPattern.Visible = !btn4.IsCLick;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            trackScore.Visible=!lbPattern.IsCLick;
        }
        private void AdjAngleTargetDeg_ValueChanged(float obj)
        {
            Propety.AngleTargetDeg = (int)AdjAngleTargetDeg.Value;
        }

        private void AdjAngleToleranceDeg_ValueChanged(float obj)
        {
            Propety.AngleToleranceDeg = (int)AdjAngleToleranceDeg.Value;
        }

        private void btnAutoMean_Click(object sender, EventArgs e)
        {
            Propety.AutoMean = btnAutoMean.IsCLick;
            AdjFixedMean.Enabled = !btnAutoMean.IsCLick;
        }
        private void AdjContinuityGapFactor_ValueChanged(float obj)
        {
            Propety.ContinuityGapFactor = AdjContinuityGapFactor.Value;
        }

        private void AdjFixedMean_ValueChanged(float obj)
        {
            Propety.FixMean = (int)AdjFixedMean.Value;
        }

        private void btnFixedMean_Click(object sender, EventArgs e)
        {
            Propety.AutoMean = !btnFixedMean.IsCLick;
            AdjFixedMean.Enabled = btnFixedMean.IsCLick;
        }
        private void AdjRANSACIterations_ValueChanged(float obj)
        {
            Propety.RansacIterations = (int)AdjRANSACIterations.Value;
        }

        private void AdjRANSACThreshold_ValueChanged(float obj)
        {
            Propety.RansacThreshold = AdjRANSACThreshold.Value;
        }
        private void trackMaxLine_ValueChanged(float obj)
        {
            Propety.MaximumLine = (int)trackMaxLine.Value;
        }

        private void trackMinInlier_ValueChanged(float obj)
        {
            Propety.MinInliersA = trackMinInlierA.Value;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            Propety.IsCalib= true;
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                Propety.IsCalib = false;
        }

        private void trackMinInlierB_ValueChanged(float obj)
        {
            Propety.MinInliersB = (int)trackMinInlierB.Value;
        }
    }
}
