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
    public partial class ToolPattern : UserControl
    {
        
        public ToolPattern( )
        {
            InitializeComponent();
           
           
        }
        

        public void LoadPara()
        {

            
            if (!workLoadModel.IsBusy)
                workLoadModel.RunWorkerAsync();

            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            trackAngle.Value =(int) Propety.Angle;

            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;

            trackNumObject.Value= Propety.NumObject;
            trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
            //txtAngle.Text = (int)Propety.Angle + "";
            if (Propety.NumObject == 0) Propety.NumObject = 1;
            Propety.ckBitwiseNot = Propety.ckBitwiseNot;
            Propety.ckSIMD = Propety.ckSIMD;
            Propety.ckSubPixel = Propety.ckSubPixel;
            ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            ckSIMD.IsCLick = Propety.ckSIMD;
            ckSubPixel.IsCLick = Propety.ckSubPixel;
          
            if (Propety.IsHighSpeed)
                btnHighSpeed.IsCLick = true;
            else
                btnNormal.IsCLick = true;
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
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (float)trackScore.Value;
           

        }

        public Patterns Propety=new Patterns();
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public void GetTemp(RectRotate rotateRect, Mat matRegister)
        {
           
                float angle = rotateRect._rectRotation;
                if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
                Mat matCrop =BeeCore.Common.CropRotatedRectSharp(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
                if (Propety.IsAreaWhite)
                    Cv2.BitwiseNot(matTemp, matTemp);
           
        }
       
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
            btnElip.IsCLick = Propety.rotCrop.IsElip;
            btnRect.IsCLick = !Propety.rotCrop.IsElip;
          
          //  G.EditTool.View.imgView.Invalidate();
          //  G.EditTool.View.imgView.Cursor = Cursors.Default;

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            btnElip.IsCLick = Propety.rotArea.IsElip;
            btnRect.IsCLick = !Propety.rotArea.IsElip;
          //  G.EditTool.View.imgView.Invalidate();
           // G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

       
        private void btnCannyMin_Click(object sender, EventArgs e)
        {
            Propety.threshMin = 180;
            Propety.threshMax = 255;
            Propety.LearnPattern(matTemp);

        }

        private void btnCannyMedium_Click(object sender, EventArgs e)
        {
            Propety.threshMin = 100;
            Propety.threshMax = 255;
            Propety.LearnPattern( matTemp);
        }

        private void btnCannyMax_Click(object sender, EventArgs e)
        {
            Propety.threshMin = 0;
            Propety.threshMax = 255;
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();

        }

    
        
      
    
        Bitmap bmResult ;
        
        public int indexTool = 0;
      

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
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
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
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
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
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
       public bool IsClear = false;
        private void btnClear_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Mask;
            Propety.TypeCrop = Global.TypeCrop;
            if(Propety.rotMask==null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
            }
            btnElip.IsCLick = Propety.rotMask.IsElip;
            btnRect.IsCLick = !Propety.rotMask.IsElip;

          //  G.EditTool.View.imgView.Invalidate();
          //  G.EditTool.View.imgView.Cursor = Cursors.Default;
            //btnClear.IsCLick = !btnClear.IsCLick;
            //IsClear = btnClear.IsCLick;
            //G.EditTool.View.Cursor = new Cursor(Properties.Resources.Erase1.Handle);



            //G.EditTool.View.imgView.Invalidate();



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
            
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
            Propety.IsAreaWhite = false;
             GetTemp(Propety.rotCrop,BeeCore.Common.listCamera[Global. IndexChoose].matRaw );
       //     G.EditTool.View.imgView.Invalidate();
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = false;
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = true;

        }
        private void btnAreaWhite_Click(object sender, EventArgs e)
        {
            Propety.IsAreaWhite = true;
            GetTemp(Propety.rotCrop, BeeCore.Common.listCamera[Global. IndexChoose].matRaw);
          //  G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
          
         //   G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(float obj)
        {
            Propety.NumObject = (int)trackNumObject.Value;
        }

        private void btnLearning_Click(object sender, EventArgs e)
        {
          
                  matTemp = Propety.GetTemp(Propety.rotCrop,Propety.rotMask, BeeCore.Common.listCamera[Global. IndexChoose].matRaw,null);
                if (Propety.rotCrop != null)
                    if (Propety.rotCrop._rect.Width != 0 && Propety.rotCrop._rect.Height != 0)
                    {
                        Propety.LearnPattern(matTemp);

                    }
                imgTemp.Image = matTemp.ToBitmap();
            

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

        private void trackNumObject_ValueChanged(object sender, EventArgs e)
        {
            Propety.NumObject = (int)trackNumObject.Value;
        }

        private void rjButton3_Click_1(object sender, EventArgs e)
        {

        }

     

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

        private void trackAngle_ValueChanged(float obj)
        {
            Propety.Angle = trackAngle.Value;
          
            if (Propety.Angle > 360) Propety.Angle = 360;
         
            if (Propety.Angle == 0)
            {
                Propety.AngleLower = Propety.rotCrop._rectRotation - 1;
                Propety.AngleUper = Propety.rotCrop._rectRotation + 1;
            }
            else
            {
                Propety.AngleLower = Propety.rotCrop._rectRotation - Propety.Angle;
                Propety.AngleUper = Propety.rotCrop._rectRotation + Propety.Angle;
            }
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

  
        private void trackMaxOverLap_ValueChanged(float obj)
        {

           Propety.OverLap= trackMaxOverLap.Value/100.0 ;
          
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

   
        private void numLimitCounter_ValueChanged(float obj)
        {
            Propety.LimitCounter = (int)numLimitCounter.Value;
        }

        //private void btnLimitCounter_Click(object sender, EventArgs e)
        //{
        //    Propety.IsLimitCouter = btnLimitCounter.IsCLick;
        //    layLimitCouter.Enabled = btnLimitCounter.IsCLick;
        //}

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
                    Propety.rotCrop.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Area:
                    Propety.rotArea.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Mask:
                    Propety.rotMask.IsElip = btnElip.IsCLick;
                    break;

            }
            //G.EditTool.View.imgView.Invalidate();
        }

        private void btnModeEdge_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Edge;
        }
        private void btnModeCany_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.OutLine;
        }

        private void btnModePattern_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Pattern;
        }

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
            Patterns.LoadEdge();
          
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
        {
            if (Propety.TypeMode == Mode.Edge)
                if(!G.IniEdge)
                {
                    workLoadModel.RunWorkerAsync();
                    return;

                }    
          
                Bitmap bmTemp = Propety.matTemp;
            imgTemp.Image = bmTemp;

            if (bmTemp != null)
            {
                Propety.LearnPattern(OpenCvSharp.Extensions.BitmapConverter.ToMat(bmTemp));


            }
        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void trackAngle_Click(object sender, EventArgs e)
        {
            Propety.Angle = trackAngle.Value;

            if (Propety.Angle > 360) Propety.Angle = 360;

            if (Propety.Angle == 0)
            {
                Propety.AngleLower = Propety.rotCrop._rectRotation - 1;
                Propety.AngleUper = Propety.rotCrop._rectRotation + 1;
            }
            else
            {
                Propety.AngleLower = Propety.rotCrop._rectRotation - Propety.Angle;
                Propety.AngleUper = Propety.rotCrop._rectRotation + Propety.Angle;
            }
        }
    }
}
