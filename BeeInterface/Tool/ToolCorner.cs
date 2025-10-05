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
    public partial class ToolCorner : UserControl
    {
        
        public ToolCorner( )
        {
            InitializeComponent();

        }
        public MeasureCorner Propety = new MeasureCorner();
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
                
                trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
                trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
                trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
                trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;

                Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
                Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolWidth_StatusToolChanged;
                Common.PropetyTools[Global.IndexChoose][Propety.Index].ScoreChanged += ToolWidth_ScoreChanged;
                AdjThreshod.Value = Propety.ThresholdBinary;
                AdjRANSACThreshold.Value = (float)Propety.RansacThreshold;
                AdjRANSACIterations.Value=Propety.RansacIterations;
                AdjThreshodAngle.Value =(float) Propety.PerpAngleToleranceDeg;
                AdjMaxLineCandidates.Value = Propety.MaxLineCandidates;
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
                AdjScale.Value= Propety.Scale;
                
                AdjMorphology.Value = Propety.SizeClose;
                AdjOpen.Value = Propety.SizeOpen;
                AdjClearNoise.Value = Propety.SizeClearsmall;
                AdjClearBig.Value = Propety.SizeClearBig;
                AdjThreshodAngle.Value =(float) Propety.PerpAngleToleranceDeg;
                btnClose.IsCLick = Propety.IsClose;
                btnOpen.IsCLick = Propety.IsOpen;
                btnIsClearSmall.IsCLick = Propety.IsClearNoiseSmall;
                btnIsClearBig.IsCLick = Propety.IsClearNoiseBig;
                AdjClearNoise.Enabled= Propety.IsClearNoiseSmall;
                AdjClearBig.Enabled= Propety.IsClearNoiseBig;
                AdjOpen.Enabled= Propety.IsOpen;
                AdjMorphology.Enabled= Propety.IsClose;

                btnAlway90.IsCLick = Propety.LinePairStrategy == LinePairStrategy.StrongPlusContourOrth ? true : false;
                btnNoAlway90.IsCLick = Propety.LinePairStrategy == LinePairStrategy.StrongPlusOrth ? true : false;
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
                //if (Propety.IsCalibs)
                //{
                //    btnCalib.IsCLick = false;
                //    Propety.IsCalibs = false;
                //    btnCalib.Enabled = true;
                //    trackMinInlier.Value = Propety.MinInliers;
                //    numMaxLen.Value = Propety.MaxLen;
                //    numMinLen.Value = Propety.MinLen;
                //}
            }
           
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score=trackScore.Value;
         }
        public bool IsClear = false;
     

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
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None);

            
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
                //    Propety.rotCrop.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
                //    break;
                //case TypeCrop.Area:
                //    Propety.rotArea.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
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

        private void AdjScale_ValueChanged(float obj)
        {
            Propety.Scale=AdjScale.Value;
        }

        private void AdjKensize_ValueChanged(float obj)
        {

        }

        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall =(int) AdjClearNoise.Value;
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

            Propety.SizeClose =(int) AdjMorphology.Value;
        }

        private void btnAlway90_Click(object sender, EventArgs e)
        {
            Propety.LinePairStrategy = LinePairStrategy.StrongPlusContourOrth;
        }

        private void AdjThreshodAngle_ValueChanged(float obj)
        {
            Propety.PerpAngleToleranceDeg = AdjThreshodAngle.Value;
        }

        private void btnNoAlway90_Click(object sender, EventArgs e)
        {

            Propety.LinePairStrategy = LinePairStrategy.StrongPlusOrth;
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
            Propety.RansacIterations =(int) AdjRANSACIterations.Value;
        }

        private void AdjMaxLineCandidates_ValueChanged(float obj)
        {
            Propety.MaxLineCandidates = (int)AdjMaxLineCandidates.Value;
        }

        private void AdjRANSACThreshold_ValueChanged(float obj)
        {
            Propety.RansacThreshold = AdjRANSACThreshold.Value;
        }

      
    }
}
