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
    public partial class ToolPositions : UserControl
    {
        
        public ToolPositions( )
        {
            InitializeComponent();

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
                Propety.DoWork();
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    //  MessageBox.Show("Worker error: " + e.Error.Message);
                    return;
                }
                Propety.Complete();
                if (!Global.IsRun)
                    Global.StatusDraw = StatusDraw.Check;
                //    G.EditTool.View.imgView.Invalidate();
                timer.Stop();

                Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;

            };
           
            Bitmap bmTemp = Propety.matTemp;
            if (bmTemp != null)
            {
                Propety.LearnPattern( OpenCvSharp.Extensions.BitmapConverter.ToMat(bmTemp));
               
                   
            }
            trackAngle.Value =(int) Propety.Angle;
            numAngle.Value = (int)Propety.Angle;
            trackScore.Value =Propety.Score; 
         
            trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
           
            Propety.ckBitwiseNot = Propety.ckBitwiseNot;
            Propety.ckSIMD = Propety.ckSIMD;
            Propety.ckSubPixel = Propety.ckSubPixel;
            ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            ckSIMD.IsCLick = Propety.ckSIMD;
            ckSubPixel.IsCLick = Propety.ckSubPixel;
           // Propety.TypeTool =;
            if (Propety.IsHighSpeed)
                btnHighSpeed.IsCLick = true;
            else
                btnNormal.IsCLick = true;
        }
        private void trackScore_ValueChanged(float obj)
        {
           Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
            numScore.Value =(int)Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
          

        }

        public Positions Propety=new Positions();
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
       
        //public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        //{
           

        //    if (Propety.rotAreaAdjustment == null && Global.IsRun) return gc;
        //    if(Global.IsRun)
        //    gc.ResetTransform();
        //    // gc.FillEllipse(Brushes.Black, Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y, 6, 6);
        //    gc.FillEllipse(Brushes.Green, 295, 950, 10, 10);
        //    var mat = new Matrix();
        //    if (!Global.IsRun)
        //    {
        //        mat.Translate(pScroll.X, pScroll.Y);
        //        mat.Scale(Scale, Scale);
        //    }

        //    RectRotate rotA = Propety.rotArea;
        //    if (Global.IsRun) rotA = Propety.rotAreaAdjustment;
        // //   mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
        // //   mat.Rotate(rotA._rectRotation);
        // //   gc.Transform = mat;
        // // //  gc.FillEllipse(Brushes.Green, -5, -5, 10, 10);
        // ////   gc.DrawString(rotA._PosCenter.X + "," + rotA._PosCenter.Y, new Font("Arial", 22, FontStyle.Bold), Brushes.Red, new System.Drawing.Point(0, 20));

        // //   //gc.FillEllipse(Brushes.Blue, -3, -3, 6, 6);
        // //   gc.DrawString(indexTool+"", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));

        // //   gc.DrawRectangle(new Pen(Color.line, 5), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
        // //   gc.ResetTransform();
        //    Color cl = Color.LimeGreen;
        //    Brush brushText = Brushes.White;
        //    if (!Propety.IsOK)
        //    {
        //        cl = Color.Red;
                


        //    }
        //    else
        //    {
        //        cl = Color.LimeGreen;
              
        //    }
         
              
        //        int i = 0;
               
        //    foreach (RectRotate rot in Propety.rectRotates)
        //    {
        //        mat = new Matrix();
        //        if (!Global.IsRun)
        //        {
        //            mat.Translate(pScroll.X, pScroll.Y);
        //            mat.Scale(Scale, Scale);
        //        }
        //        mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
        //        mat.Rotate(rotA._rectRotation);
        //        mat.Translate(rotA._rect.X, rotA._rect.Y);
        //        gc.Transform = mat;
        //        mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
        //        mat.Rotate(rot._rectRotation);
        //       // mat.TransformPoints(pMatrix);
        //        gc.Transform = mat;
        //        gc.DrawString(i + "", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rot._rect.X, (int)rot._rect.Y));
        //        gc.DrawString(Propety.ScoreRs + "%", new Font("Arial", 10, FontStyle.Bold), Brushes.Green, new System.Drawing.Point((int)rot._rect.X, (int)rot._rect.Y + 10));
        //        i++;
              
             
        //        gc.DrawRectangle(new Pen(cl, 4), new Rectangle((int)rot._rect.X, (int)rot._rect.Y, (int)rot._rect.Width, (int)rot._rect.Height));

        //        Draws.Plus(gc, 0, 0, 20, cl, 4);

        //        gc.ResetTransform();
        //        mat = new Matrix();
        //        //  mat.Rotate(G.AngleOrigin);
        //        mat.Translate(Global.pOrigin.X, Global.pOrigin.Y);
        //        gc.Transform = mat;
        //        Draws.Plus(gc, 0, 0,(int) rotA._rect.Width/2, Color.Gray, 2);
        //       // gc.DrawLine(new Pen(cl, 2), 0, 0,Math.Abs( (float)Propety.deltaX),0);
        //        //  gc.DrawLine(new Pen(Brushes.Blue, 2), 0, 0, 0, (float)Propety.deltaY);
        //        int radius = (int)rotA._rect.Width / 6;
        //        gc.DrawLine(new Pen(cl, 2), 0, 0, (float)Propety.deltaX, -(float)Propety.deltaY);
        //        Rectangle rect = new Rectangle(
        //            0 - radius,
        //            0 - radius,
        //            radius * 2,
        //            radius * 2
        //        );

        //        float startAngle = 30; // độ
        //        float sweepAngle = 120; // độ

        //        gc.DrawArc(new Pen(cl,1), rect, 0,-(float) Propety.AngleDetect);
        //        gc.FillPie(new SolidBrush(Color.FromArgb(30, cl)), rect, 0, -(float)Propety.AngleDetect);


        //        // Vẽ tâm
        //        // gc.FillEllipse(Brushes.Red, 0 - 3, 0 - 3, 6, 6);
        //        //SizeF sz = gc.MeasureString("D = " + Propety.DistanceDetect + " pixel", new Font("Arial", 16, FontStyle.Bold));
        //        //gc.FillRectangle(Brushes.White, new Rectangle((int)Propety.deltaX + 8,(int) Propety.deltaY + 8, (int)sz.Width + 30, 60));
        //        //gc.DrawString("R = " + Propety.DistanceDetect + " pixel", new Font("Arial", 16, FontStyle.Bold), Brushes.OrangeRed,new System.Drawing.Point((int)Propety.deltaX + 10, (int)Propety.deltaY +10));
        //        //gc.DrawString("Alpha =" + Propety.AngleDetect + "°", new Font("Arial", 16, FontStyle.Bold), Brushes.OrangeRed, new System.Drawing.Point((int)Propety.deltaX+10, (int)Propety.deltaY+40));
        //        //// 
        //        ////  G.EditTool.View.lbAngle.Text = angle+"";
        //        gc.ResetTransform();
        //    }
        //    gc.ResetTransform();
        //    mat = new Matrix();
        //    if (!Global.IsRun)
        //    {
        //        mat.Translate(pScroll.X, pScroll.Y);
        //        mat.Scale(Scale, Scale);
        //    }
        //    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
        //    mat.Rotate(rotA._rectRotation);
        //    gc.Transform = mat;
        //    String nameTool = (int)(Propety.Index + 1) + "." + Propety.nameTool;
           
        //    String Content = "X,Y,A,R : " + Propety.deltaX + "," + Propety.deltaY + "," + Propety.AngleDetect + "°," + Propety.DistanceDetect;
        //    Draws.Box2Label(gc, rotA._rect, nameTool, Content, Global.fontRS, cl, brushText, 40, 3);

        ////    Draws.Box1Label(gc, rotA._rect, sContent, Global.fontTool,brushText,cl);




        //    return gc;
        //}

     
        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp == null) return gc;

            if (Global. TypeCrop != TypeCrop.Area)
                try
                {
                    Mat matShow = matTemp.Clone();
                   
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

         
            //G.EditTool.View.imgView.Invalidate();
          //  G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            
          //  G.EditTool.View.imgView.Invalidate();
          //  G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

       
        private void btnCannyMin_Click(object sender, EventArgs e)
        {
            Propety.threshMin = 180;
            Propety.threshMax = 255;
            Propety.LearnPattern( matTemp);

        }

        private void btnCannyMedium_Click(object sender, EventArgs e)
        {
            Propety.threshMin = 100;
            Propety.threshMax = 255;
            Propety.LearnPattern(matTemp);
        }

        private void btnCannyMax_Click(object sender, EventArgs e)
        {
            Propety.threshMin = 0;
            Propety.threshMax = 255;
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();

        }

    
        
      
        public void Process()
        {
            //Propety.rectRotates = new List<RectRotate>();
            //if (Global.IsRun)
            //{
            //    if (G.rotOriginAdj != null)
            //        Propety.rotAreaAdjustment = G.EditTool.View.GetPositionAdjustment(Propety.rotArea, G.rotOriginAdj);
            //    else
            //        Propety.rotAreaAdjustment = Propety.rotArea;
            //    Propety.rotAreaAdjustment._angle = 0;
            //    Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global. IndexChoose].matRaw, indexTool, Propety.rotAreaAdjustment);

            //}
            //else
            //    Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global. IndexChoose].matRaw, indexTool, Propety.rotArea);
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
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            //G.EditTool.View.imgView.Invalidate();
           // G.EditTool.View.imgView.Cursor = Cursors.Default;
            //btnClear.IsCLick = !btnClear.IsCLick;
            //IsClear = btnClear.IsCLick;
            //G.EditTool.View.Cursor = new Cursor(Properties.Resources.Erase1.Handle);



            //G.EditTool.View.imgView.Invalidate();



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //G.IsCancel = true;
            
           // G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
            Propety.IsAreaWhite = false;
             GetTemp(Propety.rotCrop,BeeCore.Common.listCamera[Global. IndexChoose].matRaw );
           // G.EditTool.View.imgView.Invalidate();
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
            //G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
          
           // G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

       

        private void btnLearning_Click(object sender, EventArgs e)
        {
          
                      matTemp = Propety.GetTemp(Propety.rotCrop, Propety.rotMask,BeeCore.Common.listCamera[Global. IndexChoose].matRaw,null);
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
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;
            // G.EditTool.View.imgView.Invalidate();
            //G.EditTool.View.Cursor = Cursors.Default;
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

            //G.EditTool.View.imgView.Invalidate();
           // G.EditTool.View.Cursor = Cursors.Default;
            if (IsClear)
                btnClear.PerformClick();
            Global.StatusDraw = StatusDraw.Check;
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void rjButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void numScore_ValueChanged(object sender, EventArgs e)
        {
            numScore.Maxnimum = (int)trackScore.Max;
            numScore.Minimum = (int)trackScore.Min;
           Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = numScore.Value;
            trackScore.Value =Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
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

           Propety.OverLap= trackMaxOverLap.Value ;
            numOverLap.Value =(int) Propety.OverLap;
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }
    }
}
