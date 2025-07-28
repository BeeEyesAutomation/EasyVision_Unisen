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

            //worker = new BackgroundWorker();
            //worker.DoWork += (sender, e) =>
            //{

            //    timer.Restart();
            //    if (!Global.IsRun)
            //        Propety.rotAreaAdjustment = Propety.rotArea;
            //    Propety.DoWork(Propety.rotAreaAdjustment);
            //};
            //worker.RunWorkerCompleted += (sender, e) =>
            //{
            //    btnTest.Enabled = true;
            //    Propety.Complete();
            //  if(!Global.IsRun)
            //        Global.StatusDraw = StatusDraw.Check;
            
            //        timer.Stop();
            //    Common.PropetyTools[Global.IndexChoose][Propety.Index].CycleTime = (int)timer.Elapsed.TotalMilliseconds;
            //};
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolCircle_StatusToolChanged;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].ScoreChanged += ToolCircle_ScoreChanged;
            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;

           
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            
            numScale.Value= (decimal) Propety.Scale;
            trackThreshold.Value = Propety.Threshold;
            trackMinInlier.Value = Propety.MinInliers;
            trackIterations.Value = Propety.Iterations;
            numMinRadius.Value = Propety.MinRadius;
            numMaxRadius.Value = Propety.MaxRadius;
            numScale.Value = (int)Propety.Scale;
            switch (Propety.MethordEdge)
            {
                case MethordEdge.StrongEdges:
                    btnStrongEdge.IsCLick = true;
                    break;
                case MethordEdge.CloseEdges:
                    btnCloseEdge.IsCLick = true;
                    break;
                case MethordEdge.Binary:
                    btnBinary.IsCLick = true;
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
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
    
        public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        {
            if (Propety.rotAreaAdjustment == null && Global.IsRun) return gc;
            if(Global.IsRun)
            gc.ResetTransform();
            var mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }
            RectRotate rotA = Propety.rotArea;
            if (Global.IsRun) rotA = Propety.rotAreaAdjustment;
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;
            switch(Common.PropetyTools[Global.IndexChoose][Propety.Index].Results)
            {
                case Results.OK:
                    cl = Color.LimeGreen;
                    break;
                case Results.NG:
                    cl = Color.Red;
                    break;
            }
            
            String nameTool = (int)(Propety.Index + 1) + "." + Common.PropetyTools[Global.IndexChoose][Propety.Index].Name;
            Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl,1);
            gc.ResetTransform();
            if (!Global.IsRun)
            {
                if (!Propety.matProcess.Empty())
                {
                    mat = new Matrix();
                    mat.Translate(pScroll.X, pScroll.Y);
                    mat.Scale(Scale, Scale);
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);
                    gc.Transform = mat;

                    Bitmap myBitmap = Propety.matProcess.ToBitmap();
                    myBitmap.MakeTransparent(Color.Black);
                    myBitmap = ConvertImg.ChangeToColor(myBitmap, Color.Red, 0.7f);
                    gc.DrawImage(myBitmap, rotA._rect);
                }
            }
               
            if (Propety.rectRotates.Count > 0)
            {
                int i = 1;
                foreach (RectRotate rot in Propety.rectRotates)
                {
                    mat = new Matrix();
                    if (!Global.IsRun)
                    {
                        mat.Translate(pScroll.X, pScroll.Y);
                        mat.Scale(Scale, Scale);
                    }
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);
                    mat.Translate(rotA._rect.X, rotA._rect.Y);
                    gc.Transform = mat;
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                    Draws.Plus(gc, 0, 0, (int)rot._rect.Width / 6, cl, 2);
                    gc.DrawEllipse(new Pen(cl, 2), rot._rect);
                    float radius = (float)((rot._rect.Width) / Propety.Scale);
                    radius = (float)Math.Round(radius, 1);
                    gc.DrawString("D:" + radius, new Font("Arial", 24, FontStyle.Bold), new SolidBrush(cl), new PointF(0, 0));
                    gc.ResetTransform();
                    i++;
                }
            }
            return gc;
        }

     
   

       
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

     
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
           // Loads();
            //this.tabP1.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.BG, G.Config.colorGui);
           // this.trackNumObject.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
           // layScore.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

        }

      
       public bool IsClear = false;
     

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }

       
      

        private void btnOK_Click(object sender, EventArgs e)
        {
        
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(float obj)
        {
          //  Propety.NumObject = trackNumObject.Value;
        }

    

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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(object sender, EventArgs e)
        {
           // Propety.NumObject = trackNumObject.Value;
        }

        private void rjButton3_Click_1(object sender, EventArgs e)
        {

        }

       

        private void rjButton5_Click(object sender, EventArgs e)
        {

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

     

       

       

        private void numMinRadius_ValueChanged(object sender, EventArgs e)
        {Propety.MinRadius= numMinRadius.Value;

        }

        private void numMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            Propety.MaxRadius = numMaxRadius.Value;
        }

    
        private void trackBar21_Load_1(object sender, EventArgs e)
        {

        }

        private void trackBar21_ValueChanged(float obj)
        {
          
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
            Propety.MethordEdge = MethordEdge.CloseEdges;
        }

        private void btnStrongEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.StrongEdges;
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
    }
}
