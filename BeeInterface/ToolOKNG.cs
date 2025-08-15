using BeeCore;
using BeeGlobal;
using BeeInterface;
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

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolOKNG : UserControl
    {
        
        public ToolOKNG( )
        {
            InitializeComponent();
            //CustomGui.RoundRg(layMaximumObj, 10, Corner.Both);
            //CustomGui.RoundRg(layLimitCouter, 10, Corner.Bottom);
        }
        

        public void LoadPara()
        {

            
            //if (!workLoadModel.IsBusy)
            //    workLoadModel.RunWorkerAsync();

            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            trackResize.Value =(int) Propety.ScaleResize;
            btnEnResizeSample.IsCLick = Propety.EnResize;

            trackResize.Enabled = btnEnResizeSample.IsCLick;
            //trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            //trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            //trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            //trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;

            numCPU.Value = Propety.numCPU;
            numCPU.Enabled = Propety.Multi;
            btnMulti.IsCLick=Propety.Multi;
            btnSingle.IsCLick=!Propety.Multi;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolPattern_StatusToolChanged;
        }

        private void ToolPattern_StatusToolChanged(StatusTool obj)
        {
            if (Global.IsRun) return;
            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
            }
        }

        private void trackScore_ValueChanged(float obj)
        {
          //  Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (float)trackScore.Value;
           

        }

        public OKNG Propety=new OKNG();
      
     
       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

        }

        private void btnCropRect_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;
          
          
          //  G.EditTool.View.imgView.Invalidate();
          //  G.EditTool.View.imgView.Cursor = Cursors.Default;

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            
           
          //  G.EditTool.View.imgView.Invalidate();
           // G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

       
      
        
      
    
        Bitmap bmResult ;
        
        public int indexTool = 0;
      

        private void trackScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

       
       
      

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
       public bool IsClear = false;
        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
            
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
        }

       

      

     

     
        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.RunWorkerAsync();
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
            if (IsClear)
                btnClear.PerformClick();
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None,false);

            
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.Check;
            if (IsClear)
                btnClear.PerformClick();
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
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
                    Propety.rotCrop.IsElip = false;
                    break;
                case TypeCrop.Area:
                    Propety.rotArea.IsElip = false;
                    break;
                case TypeCrop.Mask:
                    Propety.rotMask.IsElip = false;
                    break;

            }
         //   G.EditTool.View.imgView.Invalidate();
        }

        private void btnElip_Click(object sender, EventArgs e)
        {
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
            }

            switch (Global. TypeCrop)
            {
                case TypeCrop.Crop:
                    Propety.rotCrop.IsElip = false;
                    break;
                case TypeCrop.Area:
                    Propety.rotArea.IsElip = false;
                    break;
                case TypeCrop.Mask:
                    Propety.rotMask.IsElip = false;
                    break;

            }
            //G.EditTool.View.imgView.Invalidate();
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

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { }
            

        private void btnAddOK_Click(object sender, EventArgs e)
        {
            Propety.AddOK();
            imgOK.Invalidate();
        }

        private void trackMinArea_ValueChanged(float obj)
        {
            Propety.MinArea =(int) trackResize.Value;
        }

      

        private void btnMultiScaleCanny_Click_1(object sender, EventArgs e)
        {
          //  Propety.MultiScaleCanny = btnMultiScaleCanny.IsCLick;
        }

        private void btnAddNG_Click(object sender, EventArgs e)
        {
            Propety.AddNG(); imgNG.Invalidate();
        }

        private void imgOK_Paint(object sender, PaintEventArgs e)
        {
            int x= 5;
          foreach(Bitmap bm in Propety.bmOK)
            {

                int W = bm.Width;
                int H = bm.Height;
                double Scale = (imgOK.Height-10) / (H * 1.0);
                W = (int)(W * Scale);
                e.Graphics.DrawImage(bm, new Rectangle(x, 2, W, H - 10));
              
                x += W + 5;
            }
            imgOK.Width = x + 10;
        }

        private void imgNG_Paint(object sender, PaintEventArgs e)
        {
            int x = 5;
            foreach (Bitmap bm in Propety.bmNG)
            {

                int W = bm.Width;
                int H = bm.Height;
                double Scale = (imgNG.Height - 20) / (H * 1.0);
                W = (int)(W * Scale);
                e.Graphics.DrawImage(bm, new Rectangle(x, 2, W, H - 20));

                x += W + 5;
            }
            imgNG.Width = x + 10;
        }

        private void btnRemoveAllOK_Click(object sender, EventArgs e)
        {
            Propety.RemoveAllOK(); imgOK.Invalidate();
        }

        private void btnRemoveAllNG_Click(object sender, EventArgs e)
        {
            Propety.RemoveAllNG(); imgNG.Invalidate();
        }

        private void btnLearning_Click(object sender, EventArgs e)
        {btnLearning.Enabled = false;
            workModel.RunWorkerAsync();
        }

        private void btnUndoOK_Click(object sender, EventArgs e)
        {
            Propety.RemoteOK();
            imgOK.Invalidate();
        }

        private void btnUndoNG_Click(object sender, EventArgs e)
        {
            Propety.RemoveNG();
            imgNG.Invalidate();
        }

        private void workModel_DoWork(object sender, DoWorkEventArgs e)
        {
            Propety.SetSample();
        }

        private void workModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnLearning.Enabled = true;
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            Propety.Debug();
        }

        private void btnEnResizeSample_Click(object sender, EventArgs e)
        {
            Propety.EnResize = btnEnResizeSample.IsCLick;
            trackResize.Enabled = btnEnResizeSample.IsCLick;
        }

        private void trackResize_ValueChanged(float obj)
        {
            Propety.ScaleResize = trackResize.Value;
          
        }

        private void numLimitCounter_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            Propety.Multi=!btnSingle.IsCLick;
            numCPU.Enabled = Propety.Multi;
        }

        private void btnMulti_Click(object sender, EventArgs e)
        {
            Propety.Multi = btnMulti.IsCLick;
            numCPU.Enabled = Propety.Multi;
        }

        private void numCPU_ValueChanged(object sender, EventArgs e)
        {
            Propety.numCPU =(int) numCPU.Value;
        }
    }
}
