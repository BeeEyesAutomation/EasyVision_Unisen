using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Func;
using BeeGlobal;
using BeeInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolEdge : UserControl
    {
        
        public ToolEdge( )
        {
            InitializeComponent();
            if (Propety == null)
                Propety = new Edge();
        }
        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();
        int ThresholdValue = 100;
        double MmPerPixel = 0.05;
        Line2D line1, line2;
        double gapPx=0;
        Mat annotated = new Mat();
        public void LoadPara()
        {

          
            try
            {
                EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotMask };
                EditRectRot1.Refresh();
                EditRectRot1.IsHide = false;
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
                EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
                this.VisibleChanged -= ToolEdge_VisibleChanged;
                this.VisibleChanged += ToolEdge_VisibleChanged;
                trackScore.Min = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MinValue;
                trackScore.Max = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MaxValue;
                trackScore.Step = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StepValue;
                trackScore.Value = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score;

                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusToolChanged += ToolWidth_StatusToolChanged;
                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].ScoreChanged += ToolWidth_ScoreChanged;
                AdjThreshod.Value = Propety.ThresholdBinary;

                AdjScale.Value = (float)Propety.Scale;
                AdjAngleRange.Value = (int)Propety.AngleRange;
                trackMinInlier.Value = Propety.MinInliers;
                btnStable.IsCLick = Propety.MethordEdge == MethordEdge.Stable ? true : false;
               
                switch (Propety.MethordEdge)
                {
                    case MethordEdge.StrongEdges:
                        btnStrongEdge.IsCLick = true; layThreshod.Enabled = false;
                        break;
                    case MethordEdge.CloseEdges:
                        btnCloseEdge.IsCLick = true; layThreshod.Enabled = false;
                        break;
                    case MethordEdge.Binary:
                        btnBinary.IsCLick = true; layThreshod.Enabled = true;
                        break;
                    case MethordEdge.InvertBinary:
                        btnInvert.IsCLick = true; layThreshod.Enabled = true;
                        break;
                }
                if(Propety.LineOrientation==LineOrientation.Horizontal)
                {
                    btnDirScan1.Text = "Top-Bottom";
                    btnDirScan2.Text = "Bottom-Top";
                }
                else
                {
                    btnDirScan1.Text = "Top-Bottom";
                    btnDirScan2.Text = "Bottom-Top";
                }
                if (Propety.LineDirScan == LineDirScan.TopBot|| Propety.LineDirScan == LineDirScan.LeftRight)
                    btnDirScan1.IsCLick = true;
                if (Propety.LineDirScan == LineDirScan.BotTop || Propety.LineDirScan == LineDirScan.RightLeft)
                    btnDirScan2.IsCLick = true;
                if (Propety.LineDirScan == LineDirScan.None )
                    btnNone.IsCLick = true;
                AdjRANSACThreshold.Value=(float) Propety.RansacThreshold ;
                AdjRANSACIterations.Value = Propety.RansacIterations;
                trackMinInlier.Value =(float) Propety.MinInliers;
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
             
           
            }
            catch (Exception ex)
            {
                String s = ex.Message;
            }
        }

        private void ToolEdge_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                EditRectRot1.IsHide = true;
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            }
        }

        private void EditRectRot1_RotateCurentChanged(RectRotate obj)
        {
            switch (obj.TypeCrop)
            {
                case TypeCrop.Area:
                    Propety.rotArea = obj; break;
                case TypeCrop.Crop:
                    Propety.rotCrop = obj; break;
                case TypeCrop.Mask:
                    Propety.rotMask = obj; break;

            }
        }

      

        private void ToolWidth_ScoreChanged(float obj)
        {
           trackScore.Value = obj;
        }

        private void ToolWidth_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {if (Global.IsRun) return;
            if (Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
                if (Propety.IsCalibs)
                {
                    btnCalib.IsCLick = false;
                    Propety.IsCalibs = false;
                    btnCalib.Enabled = true;
                    trackMinInlier.Value = Propety.MinInliers;
                    
                }
            }
           
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score=trackScore.Value;
         }
        public bool IsClear = false;
        public Edge Propety { get; set; }
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();

    
       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

        }

      
       
        private void btnCannyMin_Click(object sender, EventArgs e)
        {
        

        }

        private void btnCannyMedium_Click(object sender, EventArgs e)
        {
         
        }

        private void btnCannyMax_Click(object sender, EventArgs e)
        {
        

        }

    
        
      
    
        Bitmap bmResult ;
        
        public int indexTool = 0;
        private void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            btnTest.IsCLick = false;
         //   G.EditTool.View.imgView.Invalidate();

        //    G.ResultBar.lbCycleTrigger.Text = "[" + Propety.cycleTime + "ms]";
        }

        private void trackScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

       
      
      
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
           // Loads();
            //this.tabP1.BackColor = CustomGui.BackColor(TypeCtr.BG, G.Config.colorGui);
           // this.trackNumObject.BackColor = CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
           // layScore.BackColor = CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
      

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

    

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].RunToolAsync();

            //btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
        private void btnCropHalt_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
            
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);

            
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    
        private void rjButton3_Click_1(object sender, EventArgs e)
        {

        }

       

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

        private void trackAngle_ValueChanged(float obj)
        {
           
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void numAngle_ValueChanged(object sender, EventArgs e)
        {
          
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

      

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

     

      
   
     
      
  
        

        private void btnModeEdge_Click(object sender, EventArgs e)
        {
          
        }

        private void btnModeCany_Click(object sender, EventArgs e)
        {
         
        }

        private void btnModePattern_Click(object sender, EventArgs e)
        {
        }

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
          //  OutLine.LoadEdge();
          
        }

      

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnShort_Click(object sender, EventArgs e)
        {
            Propety.SegmentStatType = SegmentStatType.Shortest;
        }

        private void btnAverage_Click(object sender, EventArgs e)
        {
            Propety.SegmentStatType = SegmentStatType.Average;
        }

        private void btnLong_Click(object sender, EventArgs e)
        {
            Propety.SegmentStatType = SegmentStatType.Longest;
        }

        private void btnMid_Click(object sender, EventArgs e)
        {
            Propety.GapExtremum = GapExtremum.Middle;
        }

        private void btnOutter_Click(object sender, EventArgs e)
        {
            Propety.GapExtremum = GapExtremum.Outermost;
        }

        private void btnNear_Click(object sender, EventArgs e)
        {
            Propety.GapExtremum = GapExtremum.Nearest;
        }

        private void btnFar_Click(object sender, EventArgs e)
        {
            Propety.GapExtremum = GapExtremum.Nearest;
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Vertical;
        }

       

        private void btnHori_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Horizontal;
        }

       

        private void trackMinInlier_ValueChanged(float obj)
        {
            Propety.MinInliers = (int)trackMinInlier.Value;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            Propety.IsCalibs = true;
            Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].RunToolAsync();
          
            
        }

        private void trackScore_Load(object sender, EventArgs e)
        {

        }

        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            layThreshod.Enabled = true;
        }

      



        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

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
        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall = (int)AdjClearNoise.Value;
        }

        private void btnEnMorphology_Click(object sender, EventArgs e)
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
        private void AdjRANSACIterations_ValueChanged(float obj)
        {
            Propety.RansacIterations = (int)AdjRANSACIterations.Value;
        }

        private void AdjRANSACThreshold_ValueChanged(float obj)
        {
            Propety.RansacThreshold = AdjRANSACThreshold.Value;
        }
        private void AdjScale_ValueChanged(float obj)
        {
            Propety.Scale = AdjScale.Value;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            EditRectRot1.Visible = !btn1.IsCLick;
        }

    

        private void btn2_Click(object sender, EventArgs e)
        {
            layMain.Visible = !btn2.IsCLick;
        }

        private void btnVertical_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation= LineOrientation.Vertical;
            btnDirScan1.Text = "Left-Right";
            btnDirScan2.Text = "Right-Left";
        }

        private void btnHorizontal_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Horizontal;
         
            btnDirScan1.Text = "Top-Bottom";
            btnDirScan2.Text = "Bottom-Top";
        }

        private void rjButton6_Click(object sender, EventArgs e)
        {
           
        }

        private void btnLR_Click(object sender, EventArgs e)
        {
            if (Propety.LineOrientation == LineOrientation.Horizontal)
            {
                Propety.LineDirScan = LineDirScan.TopBot;
            }
            else

                Propety.LineDirScan = LineDirScan.LeftRight;

        }

        private void btnRL_Click(object sender, EventArgs e)
        {
            if (Propety.LineOrientation == LineOrientation.Horizontal)
            {
                Propety.LineDirScan = LineDirScan.BotTop;
            }
            else

                Propety.LineDirScan = LineDirScan.RightLeft;

        }

        private void btnTB_Click(object sender, EventArgs e)
        {
            Propety.LineDirScan = LineDirScan.None;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            layScanDir.Visible =! btn4.IsCLick;
        }

        private void AdjAngleRange_ValueChanged(float obj)
        {
            Propety.AngleRange=(int)AdjAngleRange.Value;
        }

        private void btnStable_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Stable;
            layThreshod.Enabled = false;
        }

        private void btn2_Click_1(object sender, EventArgs e)
        {
            layExtractEdge.Visible = !btn2.IsCLick;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            AdjAngleRange.Visible=!btn5.IsCLick;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            AdjScale.Visible = !btn6.IsCLick;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn7.IsCLick;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            layDirection.Visible = !btn3.IsCLick;
        }

       
    }
}
