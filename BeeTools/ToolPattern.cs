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
using BeeCore;
using BeeGlobal;
using BeeInterface;


using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolPattern : UserControl
    {
        
        public ToolPattern( )
        {
            InitializeComponent();
            CustomGui.RoundRg(layMaximumObj, 10, Corner.Both);
            CustomGui.RoundRg(layLimitCouter, 10, Corner.Bottom);
        }
        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();

        public void LoadPara()
        {

            worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {

                timer.Restart();
                if (!Global.IsRun)
                    Propety.rotAreaAdjustment = Propety.rotArea;
                Propety.DoWork(Propety.rotAreaAdjustment);
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
              
                Propety.Complete();
                if (!Global.IsRun)
                    Global.StatusDraw = StatusDraw.Check;
                timer.Stop();

                Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;

            }; 
            if (!workLoadModel.IsBusy)
                workLoadModel.RunWorkerAsync();
            Propety.TypeTool = TypeTool.Pattern;
            Propety.StatusTool = StatusTool.Initialed;
            trackAngle.Value =(int) Propety.Angle;
            numAngle.Value = (int)Propety.Angle;
            trackScore.Value =Propety.Score; 
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
        }
        private void trackScore_ValueChanged(float obj)
        {
            Propety.Score = (float)trackScore.Value;
            numScore.Value =(int) Propety.Score;
         

        }

        public OutLine Propety=new OutLine();
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
        static (double, double) RotateRelativeToPoint(double x, double y, double cx, double cy, double alpha)
        {
            // Dịch điểm về gốc
            double xTranslated = x - cx;
            double yTranslated = y - cy;

            // Xoay điểm quanh gốc
            double xRotated = xTranslated * Math.Cos(alpha) - yTranslated * Math.Sin(alpha);
            double yRotated = xTranslated * Math.Sin(alpha) + yTranslated * Math.Cos(alpha);

            // Tọa độ mới so với điểm gốc
            double xFinal = xRotated ;
            double yFinal = yRotated ;

            return (Math.Round( xFinal,1), Math.Round(yFinal,1));
        }
        static (double, double) ConvertToGlobalCoordinates(double xPrime, double yPrime, double xB2, double yB2, double alpha)
        {     // Tọa độ A trong hệ B
            double xB = xPrime, yB = yPrime;

            // Tọa độ tâm xoay B trong hệ C
            double xC = xB2, yC = yB2;
            // Ma trận xoay
            double cosTheta = Math.Cos(alpha);
            double sinTheta = Math.Sin(alpha);

            // Tính A trong hệ C
            double x = cosTheta * xB - sinTheta * yB + xC;
            double y = sinTheta * xB + cosTheta * yB + yC;
            // Tính tọa độ trong hệ gốc
            //double x = xPrime * Math.Cos(alpha) - yPrime * Math.Sin(alpha) + xB;
            //double y = xPrime * Math.Sin(alpha) + yPrime * Math.Cos(alpha) + yB;

            return (Math.Round( x), Math.Round(y));
        }
        static (double, double) ConvertToB(double xA, double yA, double xB, double yB, double alpha)
        {
          //  if (alpha < 0) alpha = 180 + alpha;
            // Dịch chuyển A về gốc B
            double dx = xA - xB;
            double dy = yA - yB;
            double radians = alpha * Math.PI / 180;
            // Xoay ngược lại góc alpha
            double xAInB = dx * Math.Cos(-radians) - dy * Math.Sin(-radians);
            double yAInB = dx * Math.Sin(-radians) + dy * Math.Cos(-radians);

            return (Math.Round(xAInB), Math.Round(yAInB));
           // return ( xRotated, yRotated);
        }
       
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
            
            if (!Propety.IsOK)
            {
                cl = Color.Red;
                //if (BeeCore.Common.PropetyTools[Propety.IndexThread][Propety.Index].UsedTool == UsedTool.Invertse &&
                //    G.Config.ConditionOK == ConditionOK.Logic)
                //    cl = Color.LimeGreen;


            }
            else
            {
                cl = Color.LimeGreen;
                //if (BeeCore.Common.PropetyTools[Propety.IndexThread][Propety.Index].UsedTool == UsedTool.Invertse &&
                //    G.Config.ConditionOK == ConditionOK.Logic)
                //    cl = Color.Red;
            }
            String nameTool = (int)(Propety.Index + 1) + "." + BeeCore.Common.PropetyTools[Propety.IndexThread][Propety.Index].Name;
            Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl,4);
            gc.ResetTransform();
            if (Propety.listScore == null) return gc;
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
                    Draws.Plus(gc, 0, 0, (int)rot._rect.Width / 2, cl, 6);
                    Draws.Box2Label(gc, rot._rect, i+"", Math.Round(Propety.listScore[i-1], 1) + "%", Global.fontRS, cl, brushText, 16, 7);

                    gc.ResetTransform();
                    i++;
                }
            }
       


            return gc;
        }

     
        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp == null) return gc;

            if (Global.TypeCrop != TypeCrop.Area)
                try
                {
                    Mat matShow = matTemp.Clone();
                    if (Propety.TypeMode == Mode.OutLine)
                    {
                        Bitmap bmTemp = matShow.ToBitmap();

                        bmTemp.MakeTransparent(Color.Black);
                        bmTemp = ConvertImg.ChangeToColor(bmTemp, Color.FromArgb(0, 255, 0), 1f);

                        gc.DrawImage(bmTemp, _rect);
                    }
                    if (matMask != null)
                    {
                        Bitmap myBitmap2 = matMask.ToBitmap();
                        myBitmap2.MakeTransparent(Color.Black);
                        myBitmap2 = ConvertImg.ChangeToColor(myBitmap2, Color.OrangeRed, 1f);

                        gc.DrawImage(myBitmap2, _rect);
                    }

                }
                catch (Exception ex) { }
            return gc;
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
            Propety.NumObject = trackNumObject.Value;
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
           
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
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
            Propety.NumObject = trackNumObject.Value;
        }

        private void rjButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void numScore_ValueChanged(object sender, EventArgs e)
        {
            numScore.Maxnimum = (int)trackScore.Max;
            numScore.Minimum = (int)trackScore.Min;
            Propety.Score = numScore.Value;
            trackScore.Value = Propety.Score;
        }

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

        private void trackAngle_ValueChanged(float obj)
        {
            Propety.Angle = trackAngle.Value;
            numAngle.Value =(int) Propety.Angle;
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

        private void numAngle_ValueChanged(object sender, EventArgs e)
        {
            Propety.Angle = numAngle.Value;
            trackAngle.Value = (int)Propety.Angle;
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
            numOverLap.Value =(int)( Propety.OverLap*100.0);
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void numOverLap_ValueChanged(object sender, EventArgs e)
        {
            Propety.OverLap = numOverLap.Value / 100.0;
            trackMaxOverLap.Value = (int)(Propety.OverLap * 100.0);
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

   
        private void numLimitCounter_ValueChanged(object sender, EventArgs e)
        {
            Propety.LimitCounter = numLimitCounter.Value;
        }

        private void btnLimitCounter_Click(object sender, EventArgs e)
        {
            Propety.IsLimitCouter = btnLimitCounter.IsCLick;
            layLimitCouter.Enabled = btnLimitCounter.IsCLick;
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
            OutLine.LoadEdge();
          
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
    }
}
