using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Funtion;
using BeeGlobal;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolCircle : UserControl
    {
        
        public ToolCircle( )
        {
            InitializeComponent();
        }
        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();

        public void LoadPara()
        {

       
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolCircle_StatusToolChanged;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].ScoreChanged += ToolCircle_ScoreChanged;
            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            
            numScale.Value= (float) Propety.Scale;
            trackThreshold.IsInital = true;
            trackThreshold.Value = Propety.Threshold;
            trackMinInlier.IsInital = true;
            trackMinInlier.Value = Propety.MinInliers;
            trackIterations.IsInital = true;
            trackIterations.Value = Propety.Iterations;
            numMinRadius.Value = Propety.MinRadius;
            numMaxRadius.Value = Propety.MaxRadius;
            numScale.Value = (int)Propety.Scale;
            AdjThreshod.Value = Propety.ThresholdBinary;
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
            switch (Propety.CircleScanDirection)
            {
                case CircleScanDirection.InsideOut:
                    btnInsideOut.IsCLick = true;
                    break;
                case CircleScanDirection.OutsideIn:
                    btnOutsideIn.IsCLick = true;
                    break;
            }
        }

        private void ToolCircle_ScoreChanged(float obj)
        {
            trackScore.Value = obj;
        }

        private void ToolCircle_StatusToolChanged(StatusTool obj)
        {
           if(Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool==StatusTool.Done)
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
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score =trackScore.Value;
          
        }
        public Circle Propety=new Circle();

        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void btnCropRect_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;
            btnElip.IsCLick = Propety.rotCrop.IsElip;
            btnRect.IsCLick = !Propety.rotCrop.IsElip;
          
         

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            btnElip.IsCLick = Propety.rotArea.IsElip;
            btnRect.IsCLick = !Propety.rotArea.IsElip;
         
        }

       
    
        
      
      
        Bitmap bmResult ;
      
     
        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

     
     

      
       public bool IsClear = false;
     

     


        private void btnTest_Click(object sender, EventArgs e)
        {
          
          if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
        private void btnCropHalt_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;

            if (IsClear)
                btnMask.PerformClick();
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None,false);

         
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            if (IsClear)
                btnMask.PerformClick();
            Global.StatusDraw = StatusDraw.Edit;
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

   

        private void btnRect_Click(object sender, EventArgs e)
        {
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
            }
            switch (Global.TypeCrop)
            {
                case TypeCrop.Crop:
                    Propety.rotCrop.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Area:
                    Propety.rotArea.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Mask:
                    Propety.rotMask.IsElip = btnElip.IsCLick;
                    break;

            }
            
        }

        private void btnElip_Click(object sender, EventArgs e)
        {
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
            }

            switch (Global.TypeCrop)
            {
                case TypeCrop.Crop:
                    Propety.rotCrop.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Area:
                    Propety.rotArea.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Mask:
                    Propety.rotMask.IsElip = btnElip.IsCLick;
                    break;

            }
           
        }

      

      

        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global.TypeCrop)
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
           
        }

     

       

       

 
 

        private void numScale_ValueChanged(object sender, EventArgs e)
        {
            Propety.Scale =(float) numScale.Value;
        }

        private void rjButton3_Click_2(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

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
            Propety.MethordEdge = MethordEdge.CloseEdges; layThreshod.Enabled = false;
        }

        private void btnStrongEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.StrongEdges; layThreshod.Enabled = false;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            Propety.IsCalibs = btnCalib.IsCLick;
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }

        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            layThreshod.Enabled = true;
        }

        private void btnMask_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Mask;
            Propety.TypeCrop = Global.TypeCrop;
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); 
            }
            btnElip.IsCLick = Propety.rotMask.IsElip;
            btnRect.IsCLick = !Propety.rotMask.IsElip;
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
            layThreshod.Enabled = true;
        }

        private void AdjThreshod_ValueChanged(float obj)
        {
            Propety.Threshold = AdjThreshod.Value;

        }
    }
}
