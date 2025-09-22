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
    public partial class ToolWidth : UserControl
    {
        
        public ToolWidth( )
        {
            InitializeComponent();
        
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
                if (!workLoadModel.IsBusy)
                    workLoadModel.RunWorkerAsync();
                trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
                trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
                trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
                trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;

                Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
                Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolWidth_StatusToolChanged;
                Common.PropetyTools[Global.IndexChoose][Propety.Index].ScoreChanged += ToolWidth_ScoreChanged;
                AdjThreshod.Value = Propety.ThresholdBinary;

                numScale.Value = (float)Propety.Scale;
                trackMaxLine.Value = Propety.MaximumLine;
                trackMinInlier.Value = Propety.MinInliers;
                numScale.Value = (int)Propety.Scale;
                numMinLen.Value = Propety.MinLen;
                numMaxLen.Value = Propety.MaxLen;
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
                switch (Propety.LineOrientation)
                {
                    case LineOrientation.Vertical:
                        btnVer.IsCLick = true;
                        break;
                    case LineOrientation.Horizontal:
                        btnHori.IsCLick = true;
                        break;
                }
                switch (Propety.SegmentStatType)
                {
                    case SegmentStatType.Longest:
                        btnLong.IsCLick = true;
                        break;
                    case SegmentStatType.Shortest:
                        btnShort.IsCLick = true;
                        break;
                    case SegmentStatType.Average:
                        btnAverage.IsCLick = true;
                        break;
                }
                switch (Propety.GapExtremum)
                {
                    case GapExtremum.Outermost:
                        btnOutter.IsCLick = true;
                        break;
                    case GapExtremum.Middle:
                        btnMid.IsCLick = true;
                        break;
                    case GapExtremum.Nearest:
                        btnNear.IsCLick = true;
                        break;
                    case GapExtremum.Farthest:
                        btnFar.IsCLick = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                String s = ex.Message;
            }
        }

        private void ToolWidth_ScoreChanged(float obj)
        {
           trackScore.Value = obj;
        }

        private void ToolWidth_StatusToolChanged(StatusTool obj)
        {if (Global.IsRun) return;
            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
                if (Propety.IsCalibs)
                {
                    btnCalib.IsCLick = false;
                    Propety.IsCalibs = false;
                    btnCalib.Enabled = true;
                    trackMinInlier.Value = Propety.MinInliers;
                    numMaxLen.Value = Propety.MaxLen;
                    numMinLen.Value = Propety.MinLen;
                }
            }
           
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score=trackScore.Value;
         }
        public bool IsClear = false;
        public Width Propety=new Width();
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
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected]. worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
        private void btnCropHalt_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Check;
            
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None,false);

            
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.Check;
           
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

     

        private void btnLess_Click(object sender, EventArgs e)
        {
            Propety.Compare=Compares.Less;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Equal;
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.More;
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

        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global. TypeCrop)
            {
                //case TypeCrop.Crop:
                //    Propety.rotCrop.IsElip = btnElip.IsCLick;
                //    break;
                //case TypeCrop.Area:
                //    Propety.rotArea.IsElip = btnElip.IsCLick;
                //    break;
                case TypeCrop.Mask:
                    Propety.rotMask = null;// = btnElip.IsCLick;
                    break;

            }
          //  G.EditTool.View.imgView.Invalidate();
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

        private void trackMaxLine_ValueChanged(float obj)
        {
            Propety.MaximumLine = (int)trackMaxLine.Value;
        }

        private void trackMinInlier_ValueChanged(float obj)
        {
            Propety.MinInliers = (int)trackMinInlier.Value;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.IsCalibs = true;
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                Propety.IsCalibs = false;
            
        }

        private void trackScore_Load(object sender, EventArgs e)
        {

        }

        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            layThreshod.Enabled = true;
        }

        private void numScale_ValueChanged(object sender, EventArgs e)
        {
            Propety.Scale = (int)numScale.Value;
        }

        private void numMinLen_ValueChanged(float obj)
        {
            Propety.MinLen = (int)numMinLen.Value;
        }

        private void numMaxLen_ValueChanged(float obj)
        {
            Propety.MaxLen = (int)numMaxLen.Value;
        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void numScale_ValueChanged(float obj)
        {
            Propety.Scale = numScale.Value;
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

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
         
        }
    }
}
