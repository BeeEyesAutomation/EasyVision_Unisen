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
    public partial class ToolPitch : UserControl
    {
        
        public ToolPitch( )
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
               
                trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
                trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
                trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
                trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;

                Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
                Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolWidth_StatusToolChanged;
                Common.PropetyTools[Global.IndexChoose][Propety.Index].ScoreChanged += ToolWidth_ScoreChanged;
                AdjThreshod.Value = Propety.ThresholdBinary;
                AdjGaussianSmooth.Value = Propety.ValueGau;
                AdjScale.Value = 1/(float)Propety.Scale;
                numCrestCouter.Value= Propety.NumCrestCouter ;
                numRootCounter.Value = Propety.NumRootCouter;
                AdjMagin.Value=Propety.Magin;

                lbCrestCount.Text = Propety.TempCountCrest.ToString();
                lbRootCount.Text = Propety.TempCountRoot.ToString();

                AdjClose.Value = Propety.SizeClose;
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
                AdjClose.Enabled = Propety.IsClose;

                btnEnCrestPitch.IsCLick = Propety.IsEnCrestPitch;
                btnEnCrestHeight.IsCLick= Propety.IsEnCrestHeight;
               
                btnEnRootPitch.IsCLick = Propety.IsEnRootPitch;
                btnEnRootHeight.IsCLick = Propety.IsEnRootHeight;

                btnEnRootCounter.IsCLick = Propety.IsEnRootCounter;
                btnEnCrestCounter.IsCLick = Propety.IsEnCrestCounter;

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
              

             

              
                switch (Propety.Values)
                {
                    case Values.Mean:
                        lbPitchCrestMean.Text = Math.Round(Propety.TempPitchCrest, 3).ToString();
                        lbHeightRootMean.Text = Math.Round(Propety.TempHeightRoot, 3).ToString();
                        lbPitchRootMean.Text = Math.Round(Propety.TempPitchRoot, 3).ToString();
                        lbHeightCrestMean.Text = Math.Round(Propety.TempHeightCrest, 3).ToString();
                        btnMean.IsCLick = true;
                        //Mean
                        lbPitchCrestMean.ForeColor = Color.Black;
                        lbHeightCrestMean.ForeColor = Color.Black;
                        lbPitchRootMean.ForeColor = Color.Black;
                        lbHeightRootMean.ForeColor = Color.Black;
                        //Median
                        lbPitchCrestMedian.ForeColor = Color.LightGray;
                        lbHeightCrestMedian.ForeColor = Color.LightGray;
                        lbPitchRootMedian.ForeColor = Color.LightGray;
                        lbHeightRootMedian.ForeColor = Color.LightGray;
                        //Min
                        lbPitchCrestMin.ForeColor = Color.LightGray;
                        lbHeightCrestMin.ForeColor = Color.LightGray;
                        lbPitchRootMin.ForeColor = Color.LightGray;
                        lbHeightRootMin.ForeColor = Color.LightGray;
                        //Max
                        lbPitchCrestMax.ForeColor = Color.LightGray;
                        lbHeightCrestMax.ForeColor = Color.LightGray;
                        lbPitchRootMax.ForeColor = Color.LightGray;
                        lbHeightRootMax.ForeColor = Color.LightGray;

                        break;
                    case Values.Median:

                        lbHeightRootMedian.Text = Math.Round(Propety.TempHeightRoot, 3).ToString();
                        lbPitchRootMedian.Text = Math.Round(Propety.TempPitchRoot, 3).ToString();
                        lbHeightCrestMedian.Text = Math.Round(Propety.TempHeightCrest, 3).ToString();
                        lbPitchCrestMedian.Text = Math.Round(Propety.TempPitchCrest, 3).ToString();
                        btnMedian.IsCLick = true;
                        //Mean
                        lbPitchCrestMean.ForeColor = Color.LightGray;
                        lbHeightCrestMean.ForeColor = Color.LightGray;
                        lbPitchRootMean.ForeColor = Color.LightGray;
                        lbHeightRootMean.ForeColor = Color.LightGray;
                        //Median
                        lbPitchCrestMedian.ForeColor = Color.Black;
                        lbHeightCrestMedian.ForeColor = Color.Black;
                        lbPitchRootMedian.ForeColor = Color.Black;
                        lbHeightRootMedian.ForeColor = Color.Black;
                        //Min
                        lbPitchCrestMin.ForeColor = Color.LightGray;
                        lbHeightCrestMin.ForeColor = Color.LightGray;
                        lbPitchRootMin.ForeColor = Color.LightGray;
                        lbHeightRootMin.ForeColor = Color.LightGray;
                        //Max
                        lbPitchCrestMax.ForeColor = Color.LightGray;
                        lbHeightCrestMax.ForeColor = Color.LightGray;
                        lbPitchRootMax.ForeColor = Color.LightGray;
                        lbHeightRootMax.ForeColor = Color.LightGray;
                        break;
                    case Values.Min:
                        lbHeightRootMin.Text = Math.Round(Propety.TempHeightRoot, 3).ToString();
                        lbPitchRootMin.Text = Math.Round(Propety.TempPitchRoot, 3).ToString();
                        lbHeightCrestMin.Text = Math.Round(Propety.TempHeightCrest, 3).ToString();
                        lbPitchCrestMin.Text = Math.Round(Propety.TempPitchCrest, 3).ToString();
                        btnMin.IsCLick = true;
                        //Mean
                        lbPitchCrestMean.ForeColor = Color.LightGray;
                        lbHeightCrestMean.ForeColor = Color.LightGray;
                        lbPitchRootMean.ForeColor = Color.LightGray;
                        lbHeightRootMean.ForeColor = Color.LightGray;
                        //Median
                        lbPitchCrestMedian.ForeColor = Color.LightGray;
                        lbHeightCrestMedian.ForeColor = Color.LightGray;
                        lbPitchRootMedian.ForeColor = Color.LightGray;
                        lbHeightRootMedian.ForeColor = Color.LightGray;
                        //Min
                        lbPitchCrestMin.ForeColor = Color.Black;
                        lbHeightCrestMin.ForeColor = Color.Black;
                        lbPitchRootMin.ForeColor = Color.Black;
                        lbHeightRootMin.ForeColor = Color.Black;
                        //Max
                        lbPitchCrestMax.ForeColor = Color.LightGray;
                        lbHeightCrestMax.ForeColor = Color.LightGray;
                        lbPitchRootMax.ForeColor = Color.LightGray;
                        lbHeightRootMax.ForeColor = Color.LightGray;
                        btnMin.PerformClick();
                        break;
                    case Values.Max:

                        lbHeightRootMax.Text = Math.Round(Propety.TempHeightRoot, 3).ToString();
                        lbPitchRootMax.Text = Math.Round(Propety.TempPitchRoot, 3).ToString();
                        lbHeightCrestMax.Text = Math.Round(Propety.TempHeightCrest, 3).ToString();
                        lbPitchCrestMax.Text = Math.Round(Propety.TempPitchCrest, 3).ToString();
                        btnMax.IsCLick = true;
                        //Mean
                        lbPitchCrestMean.ForeColor = Color.LightGray;
                        lbHeightCrestMean.ForeColor = Color.LightGray;
                        lbPitchRootMean.ForeColor = Color.LightGray;
                        lbHeightRootMean.ForeColor = Color.LightGray;
                        //Median
                        lbPitchCrestMedian.ForeColor = Color.LightGray;
                        lbHeightCrestMedian.ForeColor = Color.LightGray;
                        lbPitchRootMedian.ForeColor = Color.LightGray;
                        lbHeightRootMedian.ForeColor = Color.LightGray;
                        //Min
                        lbPitchCrestMin.ForeColor = Color.LightGray;
                        lbHeightCrestMin.ForeColor = Color.LightGray;
                        lbPitchRootMin.ForeColor = Color.LightGray;
                        lbHeightRootMin.ForeColor = Color.LightGray;
                        //Max
                        lbPitchCrestMax.ForeColor = Color.Black;
                        lbHeightCrestMax.ForeColor = Color.Black;
                        lbPitchRootMax.ForeColor = Color.Black;
                        lbHeightRootMax.ForeColor = Color.Black;
                        btnMax.PerformClick();
                        break;
                }
                if (Propety.IsEnCrestPitch)
                {
                    lbPitchCrestMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchCrestMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchCrestMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchCrestMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
                }
                else
                {
                    lbPitchCrestMax.BackColor = Color.WhiteSmoke;
                    lbPitchCrestMin.BackColor = Color.WhiteSmoke;
                    lbPitchCrestMean.BackColor = Color.WhiteSmoke;
                    lbPitchCrestMedian.BackColor = Color.WhiteSmoke;
                }
                if (Propety.IsEnRootPitch)
                {
                    lbPitchRootMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchRootMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchRootMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchRootMedian.BackColor = Color.FromArgb(150, 247, 211, 139);

                }
                else
                {
                    lbPitchRootMax.BackColor = Color.WhiteSmoke;
                    lbPitchRootMin.BackColor = Color.WhiteSmoke;
                    lbPitchRootMean.BackColor = Color.WhiteSmoke;
                    lbPitchRootMedian.BackColor = Color.WhiteSmoke;
                }
                if (Propety.IsEnCrestHeight)
                {
                    lbHeightCrestMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightCrestMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightCrestMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightCrestMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
                }
                else
                {
                    lbHeightCrestMax.BackColor = Color.WhiteSmoke;
                    lbHeightCrestMin.BackColor = Color.WhiteSmoke;
                    lbHeightCrestMean.BackColor = Color.WhiteSmoke;
                    lbHeightCrestMedian.BackColor = Color.WhiteSmoke;
                }
                if (Propety.IsEnRootHeight)
                {
                    lbHeightRootMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightRootMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightRootMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightRootMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
                }
                else
                {
                    lbHeightRootMax.BackColor = Color.WhiteSmoke;
                    lbHeightRootMin.BackColor = Color.WhiteSmoke;
                    lbHeightRootMean.BackColor = Color.WhiteSmoke;
                    lbHeightRootMedian.BackColor = Color.WhiteSmoke;
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
            {if(Propety.IsCalib)
                {
                    btnCalib.IsCLick = false;
                    btnCalib.Enabled = true;
                    Propety.IsCalib = false;
                    lbCrestCount.Text = Propety.PitchResult.Crests.Length.ToString();
                    lbRootCount.Text = Propety.PitchResult.Roots.Length.ToString();

                    lbHeightRootMean.Text = Math.Round(Propety.PitchResult.RootHMeanMM, 3).ToString();
                    lbHeightRootMedian.Text = Math.Round(Propety.PitchResult.RootHMedianMM, 3).ToString();
                    lbHeightRootMin.Text = Math.Round(Propety.PitchResult.RootHMinMM, 3).ToString();
                    lbHeightRootMax.Text = Math.Round(Propety.PitchResult.RootHMaxMM, 3).ToString();

                    lbPitchRootMean.Text = Math.Round(Propety.PitchResult.PitchRootMeanMM, 3).ToString();
                    lbPitchRootMedian.Text = Math.Round(Propety.PitchResult.PitchRootMedianMM, 3).ToString();
                    lbPitchRootMin.Text = Math.Round(Propety.PitchResult.PitchRootMinMM, 3).ToString();
                    lbPitchRootMax.Text = Math.Round(Propety.PitchResult.PitchRootMaxMM, 3).ToString();

                    lbHeightCrestMean.Text = Math.Round(Propety.PitchResult.CrestHMeanMM, 3).ToString();
                    lbHeightCrestMedian.Text = Math.Round(Propety.PitchResult.CrestHMedianMM, 3).ToString();
                    lbHeightCrestMin.Text = Math.Round(Propety.PitchResult.CrestHMinMM, 3).ToString();
                    lbHeightCrestMax.Text = Math.Round(Propety.PitchResult.CrestHMaxMM, 3).ToString();

                    lbPitchCrestMean.Text = Math.Round(Propety.PitchResult.PitchMeanMM, 3).ToString();
                    lbPitchCrestMedian.Text = Math.Round(Propety.PitchResult.PitchMedianMM, 3).ToString();
                    lbPitchCrestMin.Text = Math.Round(Propety.PitchResult.PitchMinMM, 3).ToString();
                    lbPitchCrestMax.Text = Math.Round(Propety.PitchResult.PitchMaxMM, 3).ToString();
                    numCrestCouter.Value = Propety.TempCountCrest;
                    numRootCounter.Value = Propety.TempCountRoot;
                }
              
                btnTest.Enabled = true;
              
            }
           
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score=trackScore.Value;
         }
        public bool IsClear = false;
        public Pitch Propety=new Pitch();
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
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);

            
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

        
    

        private void btnVer_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Vertical;
        }

       

        private void btnHori_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Horizontal;
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
            AdjClose.Enabled = Propety.IsClose;
        }

        private void btnEnableNoise_Click(object sender, EventArgs e)
        {

            Propety.IsClearNoiseSmall = btnIsClearSmall.IsCLick;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
        }

        private void AdjMorphology_ValueChanged(float obj)
        {

            Propety.SizeClose = (int)AdjClose.Value;
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
            Propety.Scale =1/ AdjScale.Value;
        }

        private void btnMean_Click(object sender, EventArgs e)
        {
            Propety.Values=Values.Mean;

            //Mean
            lbPitchCrestMean.ForeColor = Color.Black ;
            lbHeightCrestMean.ForeColor = Color.Black;
            lbPitchRootMean.ForeColor = Color.Black;
            lbHeightRootMean.ForeColor = Color.Black;
            //Median
            lbPitchCrestMedian.ForeColor = Color.LightGray;
            lbHeightCrestMedian.ForeColor = Color.LightGray;
            lbPitchRootMedian.ForeColor = Color.LightGray;
            lbHeightRootMedian.ForeColor = Color.LightGray;
            //Min
            lbPitchCrestMin.ForeColor = Color.LightGray;
            lbHeightCrestMin.ForeColor = Color.LightGray;
            lbPitchRootMin.ForeColor = Color.LightGray;
            lbHeightRootMin.ForeColor = Color.LightGray;
            //Max
            lbPitchCrestMax.ForeColor = Color.LightGray;
            lbHeightCrestMax.ForeColor = Color.LightGray;
            lbPitchRootMax.ForeColor = Color.LightGray;
            lbHeightRootMax.ForeColor = Color.LightGray;

        }

        private void btnMedian_Click(object sender, EventArgs e)
        {
            Propety.Values = Values.Median;
            //Mean
            lbPitchCrestMean.ForeColor = Color.LightGray;
            lbHeightCrestMean.ForeColor = Color.LightGray;
            lbPitchRootMean.ForeColor = Color.LightGray;
            lbHeightRootMean.ForeColor = Color.LightGray;
            //Median
            lbPitchCrestMedian.ForeColor = Color.Black;
            lbHeightCrestMedian.ForeColor = Color.Black;
            lbPitchRootMedian.ForeColor = Color.Black;
            lbHeightRootMedian.ForeColor = Color.Black;
            //Min
            lbPitchCrestMin.ForeColor = Color.LightGray;
            lbHeightCrestMin.ForeColor = Color.LightGray;
            lbPitchRootMin.ForeColor = Color.LightGray;
            lbHeightRootMin.ForeColor = Color.LightGray;
            //Max
            lbPitchCrestMax.ForeColor = Color.LightGray;
            lbHeightCrestMax.ForeColor = Color.LightGray;
            lbPitchRootMax.ForeColor = Color.LightGray;
            lbHeightRootMax.ForeColor = Color.LightGray;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            Propety.Values = Values.Min;
            //Mean
            lbPitchCrestMean.ForeColor = Color.LightGray;
            lbHeightCrestMean.ForeColor = Color.LightGray;
            lbPitchRootMean.ForeColor = Color.LightGray;
            lbHeightRootMean.ForeColor = Color.LightGray;
            //Median
            lbPitchCrestMedian.ForeColor = Color.LightGray;
            lbHeightCrestMedian.ForeColor = Color.LightGray;
            lbPitchRootMedian.ForeColor = Color.LightGray;
            lbHeightRootMedian.ForeColor = Color.LightGray;
            //Min
            lbPitchCrestMin.ForeColor = Color.Black;
            lbHeightCrestMin.ForeColor = Color.Black;
            lbPitchRootMin.ForeColor = Color.Black;
            lbHeightRootMin.ForeColor = Color.Black;
            //Max
            lbPitchCrestMax.ForeColor = Color.LightGray;
            lbHeightCrestMax.ForeColor = Color.LightGray;
            lbPitchRootMax.ForeColor = Color.LightGray;
            lbHeightRootMax.ForeColor = Color.LightGray;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            Propety.Values = Values.Max;
            //Mean
            lbPitchCrestMean.ForeColor = Color.LightGray;
            lbHeightCrestMean.ForeColor = Color.LightGray;
            lbPitchRootMean.ForeColor = Color.LightGray;
            lbHeightRootMean.ForeColor = Color.LightGray;
            //Median
            lbPitchCrestMedian.ForeColor = Color.LightGray;
            lbHeightCrestMedian.ForeColor = Color.LightGray;
            lbPitchRootMedian.ForeColor = Color.LightGray;
            lbHeightRootMedian.ForeColor = Color.LightGray;
            //Min
            lbPitchCrestMin.ForeColor = Color.LightGray;
            lbHeightCrestMin.ForeColor = Color.LightGray;
            lbPitchRootMin.ForeColor = Color.LightGray;
            lbHeightRootMin.ForeColor = Color.LightGray;
            //Max
            lbPitchCrestMax.ForeColor = Color.Black;
            lbHeightCrestMax.ForeColor = Color.Black;
            lbPitchRootMax.ForeColor = Color.Black;
            lbHeightRootMax.ForeColor = Color.Black;
        }

        private void btnEnCrestPitch_Click(object sender, EventArgs e)
        {
            Propety.IsEnCrestPitch=btnEnCrestPitch.IsCLick;
            if (Propety.IsEnCrestPitch)
            {
                lbPitchCrestMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchCrestMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchCrestMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchCrestMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
            }
            else
            {
                lbPitchCrestMax.BackColor = Color.WhiteSmoke;
                lbPitchCrestMin.BackColor = Color.WhiteSmoke;
                lbPitchCrestMean.BackColor = Color.WhiteSmoke;
                lbPitchCrestMedian.BackColor = Color.WhiteSmoke;
            }

          

        }

        private void btnEnRootPitch_Click(object sender, EventArgs e)
        {
            Propety.IsEnRootPitch=btnEnRootPitch.IsCLick;
            if (Propety.IsEnRootPitch)
            {
                lbPitchRootMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchRootMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchRootMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchRootMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
             
            }
            else
            {
                lbPitchRootMax.BackColor = Color.WhiteSmoke;
                lbPitchRootMin.BackColor = Color.WhiteSmoke;
                lbPitchRootMean.BackColor = Color.WhiteSmoke;
                lbPitchRootMedian.BackColor = Color.WhiteSmoke;
            }

          
        }

        private void btnEnCreshHeight_Click(object sender, EventArgs e)
        {
            Propety.IsEnCrestHeight=btnEnCrestHeight.IsCLick;
            if (Propety.IsEnCrestHeight)
            {
                lbHeightCrestMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightCrestMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightCrestMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightCrestMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
            }
            else
            {
                lbHeightCrestMax.BackColor = Color.WhiteSmoke;
                lbHeightCrestMin.BackColor = Color.WhiteSmoke;
                lbHeightCrestMean.BackColor = Color.WhiteSmoke;
                lbHeightCrestMedian.BackColor = Color.WhiteSmoke;
            }
           
        }

        private void btnEnRootHeight_Click(object sender, EventArgs e)
        {
            Propety.IsEnRootHeight=btnEnRootHeight.IsCLick;
            if (Propety.IsEnRootHeight)
            {
                lbHeightRootMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightRootMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightRootMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightRootMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
            }
            else
            {
                lbHeightRootMax.BackColor = Color.WhiteSmoke;
                lbHeightRootMin.BackColor = Color.WhiteSmoke;
                lbHeightRootMean.BackColor = Color.WhiteSmoke;
                lbHeightRootMedian.BackColor = Color.WhiteSmoke;
            }
        }

        private void btnEnCrestCounter_Click(object sender, EventArgs e)
        {
            Propety.IsEnCrestCounter=btnEnCrestCounter.IsCLick;
            if(Propety.IsEnCrestCounter) 
            lbCrestCount.BackColor = Color.FromArgb(150, 247, 211, 139);
            else
                lbCrestCount.BackColor = Color.WhiteSmoke;
            numCrestCouter.Enabled = btnEnCrestCounter.IsCLick;
        }

        private void btnEnRootCounter_Click(object sender, EventArgs e)
        {
            Propety.IsEnRootCounter=btnEnRootCounter.IsCLick;
            if (Propety.IsEnRootCounter)
                lbRootCount.BackColor = Color.FromArgb(150, 247, 211, 139);
            else
                lbRootCount.BackColor = Color.WhiteSmoke;
        
            numRootCounter.Enabled = btnEnRootCounter.IsCLick;
        }
        private void btnLess_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Less;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Equal;
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.More;
        }

        private void numCrestCouter_ValueChanged(float obj)
        {
            Propety.NumCrestCouter = (int)numCrestCouter.Value;
        }

        private void numRootCounter_ValueChanged(float obj)
        {
            Propety.NumRootCouter = (int)numRootCounter.Value;
        }

        private void AdjGaussianSmooth_ValueChanged(float obj)
        {
            Propety.ValueGau=AdjGaussianSmooth.Value;
        }

        private void AdjMagin_ValueChanged(float obj)
        {
            Propety.Magin = (int)AdjMagin.Value;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            Propety.IsCalib= true;
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnCalib.IsCLick = false;
        }

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
         
        }
    }
}
