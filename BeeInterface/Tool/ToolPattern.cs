using BeeCore;
using BeeCpp;
using BeeGlobal;
using BeeInterface;
using Cyotek.Windows.Forms;
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
using ShapeType = BeeGlobal.ShapeType;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolPattern : UserControl
    {

        #region OwnerTool cache (Phase 2 refactor)
        private PropetyTool _ownerTool;
        private PropetyTool OwnerTool
        {
            get
            {
                if (_ownerTool == null)
                    _ownerTool = Common.TryGetTool(Global.IndexProgChoose, Propety.Index);
                return _ownerTool;
            }
        }
        private void InvalidateOwnerToolCache() => _ownerTool = null;
        #endregion
        public ToolPattern( )
        {
            InitializeComponent();

            if (Propety == null)
                Propety = new Patterns();
        }
        

        public void LoadPara()
        {
            EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotCrop, Propety.rotMask };
            EditRectRot1.Refresh();
            EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
            EditRectRot1.IsHide = false;
            this.VisibleChanged -= ToolPattern_VisibleChanged;
            this.VisibleChanged += ToolPattern_VisibleChanged;
            btnBestObj.IsCLick = Propety.SearchPattern == SearchPattern.BestObj?true:false;
            btnAllObj.IsCLick = Propety.SearchPattern == SearchPattern.AllObj ? true : false;
            if (Propety.bmRaw != null)
            {
                imgTemp.Image = Propety.bmRaw;
            }
            OwnerTool.StatusTool = StatusTool.WaitCheck;
            trackAngle.Value =(int) Propety.Angle;
           

            if (Propety.Angle > 360) Propety.Angle = 360;

            if (Propety.Angle == 0)
            {
                Propety.Angle = 1;
            }
            btnHard.IsCLick=Propety.DifficultyPattern==DifficultyPattern.Hard?true:false;
            btnNormal.IsCLick=Propety.DifficultyPattern==DifficultyPattern.Normal?true:false;
            btnEasy.IsCLick=Propety.DifficultyPattern==DifficultyPattern.Easy?true:false;
            btnEnScale.IsCLick= Propety.EnableScaleSearch  ;
            btnEnScale.Text = Propety.EnableScaleSearch == true ? "ON" : "OFF";
            numAdjScale.Enabled = Propety.EnableScaleSearch;
            numAdjScale.Value = Propety.ScalePattern;
            numAdjStepScale.Value = Propety.ScaleStep;
            btnEnableKeepFilter.IsCLick = Propety.EnableKeepFilter;
            btnEnableOverLap.IsCLick = Propety.EnableNms;
            btnEnableValidator.IsCLick = Propety.EnableValidator;
            btnEnScale.IsCLick = Propety.EnableScaleSearch;
            float angle = (Propety.rotCrop._rectRotation) - (Propety.rotArea._rectRotation);
            Propety.AngleLower = angle - Propety.Angle;
            Propety.AngleUper = angle + Propety.Angle;
            trackScore.Min = OwnerTool.MinValue;
            trackScore.Max = OwnerTool.MaxValue;
            trackScore.Step = OwnerTool.StepValue;
            trackScore.Value = OwnerTool.Score;
            if (Propety.MaxObject == 0) Propety.MaxObject = 1;
            AdjMaximumObj.Value = Propety.MaxObject;
            AdjStepAngle.Value = Propety.StepAngle;
            AdjLimitCounter.Value= Propety.LimitCounter;
            trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
           
            ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            ckSIMD.IsCLick = Propety.ckSIMD;
            ckSubPixel.IsCLick = Propety.ckSubPixel;
          switch(Propety.Compare)
            {
                case Compares.Equal:
                    btnEqual.IsCLick = true;
                    break;
                case Compares.Less:
                    btnLess.IsCLick = true;
                    break;
                case Compares.More:
                    btnMore.IsCLick = true;
                    break;
            }    
         
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
         

            txtAddPLC.Text = Propety.AddPLC;
            adjScale.Value = Propety.Scale;

            btnZero0.IsCLick=Propety.ZeroPos==ZeroPos.Zero?true:false;
            btnZeroAdj.IsCLick = Propety.ZeroPos == ZeroPos.ZeroADJ ? true : false;

             if (OwnerTool != null)

             {

                 OwnerTool.StatusToolChanged -= ToolPattern_StatusToolChanged;

                 OwnerTool.StatusToolChanged += ToolPattern_StatusToolChanged;

             }
        }

        private void ToolPattern_VisibleChanged(object sender, EventArgs e)
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

        private void ToolPattern_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (Global.IsRun) return;
            if (OwnerTool.StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
            }
        }

        private void trackScore_ValueChanged(float obj)
        {
            OwnerTool.Score = (float)trackScore.Value;
           

        }

        public Patterns Propety { get; set; }
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public void GetTemp(RectRotate rotateRect, Mat matRegister)
        {
           
                //float angle = rotateRect._rectRotation;
                //if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
                //Mat matCrop =BeeCore.Cropper.CropRotatedRect(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
                //if (matCrop.Type() == MatType.CV_8UC3)
                //    Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
                //if (Propety.IsAreaWhite)
                //    Cv2.BitwiseNot(matTemp, matTemp);
           
        }
       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

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
      
      
        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
            
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
        }


        private void btnNormal_Click(object sender, EventArgs e)
        {
          
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = true;

        }
        private void btnAreaWhite_Click(object sender, EventArgs e)
        {
            Propety.IsAreaWhite = true;
            GetTemp(Propety.rotCrop, BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw);
          //  G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
          
         //   G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

   
        private void btnLearning_Click(object sender, EventArgs e)
        {

            if (Propety.rotCrop != null)
                if (Propety.rotCrop._rect.Width != 0 && Propety.rotCrop._rect.Height != 0)
                {
                    Propety.bmRaw = Propety.LearnPattern(BeeCore.Common.listCamera[Propety.IndexThread].matRaw.Clone(), false).ToBitmap();
                    imgTemp.Image = Propety.bmRaw;
                }


        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            Common.TryGetTool(Global.IndexToolSelected).RunToolAsync();
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
            Propety.LimitCounter = (int)AdjLimitCounter.Value;
        }



       
        private void btnModeEdge_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Edge;
        }
     

        private void btnModePattern_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Pattern;
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
        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            layThreshod.Enabled = true;
        }

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
          //  Patterns.LoadEdge();
          
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

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (Propety.TypeMode == Mode.Edge)
            //    if(!G.IniEdge)
            //    {
            //        workLoadModel.RunWorkerAsync();
            //        return;

            //    }    
          
          
        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

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

        private void AdjMaximumObj_ValueChanged(float obj)
        {
            Propety.MaxObject = (int)AdjMaximumObj.Value;
        }

        private void AdjStepAngle_ValueChanged(float obj)
        {

            Propety.StepAngle = 
                AdjStepAngle.Value;
        }

        private void txtAddPLC_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtAddPLC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==Keys.Enter)
            {
                Propety.AddPLC=txtAddPLC.Text;
            }    
        }

     
      

        private void btn3_Click(object sender, EventArgs e)
        {
            lay3.Visible = !btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            trackAngle.Visible = !btn4.IsCLick;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            lay5.Visible =! btn5.IsCLick;
        }

      

        private void btn6_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn6.IsCLick;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            layLimitCouter.Visible = !btn8.IsCLick;
            AdjLimitCounter.Visible = !btn8.IsCLick;
        }

        private void btnZeroAdj_Click(object sender, EventArgs e)
        {
            Propety.ZeroPos = ZeroPos.ZeroADJ;
        }

        private void adjScale_ValueChanged(float obj)
        {
            Propety.Scale =(float)adjScale.Value;
        }

        private void btn7_Click_1(object sender, EventArgs e)
        {
            lay71.Visible = !btn7.IsCLick;
            lay72.Visible = !btn7.IsCLick;
        }

        private void btnZero0_Click(object sender, EventArgs e)
        {
            Propety.ZeroPos = ZeroPos.Zero;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            EditRectRot1.Visible = !btn1.IsCLick;
        }

        private void btnBestObj_Click(object sender, EventArgs e)
        {
            Propety.SearchPattern = SearchPattern.BestObj;
        }

        private void btnAllObj_Click(object sender, EventArgs e)
        {
            Propety.SearchPattern = SearchPattern.AllObj;
        }

        private void btnEasy_Click(object sender, EventArgs e)
        {
            Propety.DifficultyPattern= DifficultyPattern.Easy;
        }

        private void btnNormal_Click_1(object sender, EventArgs e)
        {
            Propety.DifficultyPattern = DifficultyPattern.Normal;
        }

        private void btnHard_Click(object sender, EventArgs e)
        {
            Propety.DifficultyPattern = DifficultyPattern.Hard;
        }

        private void btnEnScale_Click(object sender, EventArgs e)
        {
            Propety.EnableScaleSearch = btnEnScale.IsCLick;
            btnEnScale.Text = Propety.EnableScaleSearch == true ? "ON" : "OFF";
            numAdjScale.Enabled = Propety.EnableScaleSearch;

        }

        private void numAdjScale_ValueChanged(float obj)
        {
            Propety.ScalePattern =(int) numAdjScale.Value;
        }

        private void numAdjStepScale_ValueChanged(float obj)
        {
            Propety.ScaleStep = (int)numAdjStepScale.Value;
        }

        private void btnEnableValidator_Click(object sender, EventArgs e)
        {Propety.EnableValidator = btnEnableValidator.IsCLick;

        }

        private void btnEnableKeepFilter_Click(object sender, EventArgs e)
        {
            Propety.EnableKeepFilter = btnEnableKeepFilter.IsCLick;
        }

        private void btnEnableOverLap_Click(object sender, EventArgs e)
        {
            Propety.EnableNms=btnEnableOverLap.IsCLick;
        }
    }
}
