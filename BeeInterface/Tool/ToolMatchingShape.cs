using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeCore;
using BeeGlobal;
using BeeInterface;
using BeeInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolMatchingShape : UserControl
    {
        
        public ToolMatchingShape( )
        {
            InitializeComponent();
            
        }
       
        public MatchingShape Propety = new MatchingShape();
        public BackgroundWorker worker = new BackgroundWorker();
        public void LoadPara()
        {
            worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
              //  Propety.DoWork();
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    //  MessageBox.Show("Worker error: " + e.Error.Message);
                    return;
                }
                Propety.Complete();

            };
            Bitmap bmTemp = Propety.matTemp;
            if (bmTemp != null)
            {
                matTemp = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmTemp);
                Propety.LoadTemp(matTemp);
              
               
            }
            btnInvert.IsCLick = Propety.Invert;
            trackScore.Value =Propety.Score;
            //trackNumObject.Value = Propety.NumObject;
            //trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
            txtAngle.Text = (int)Propety.MinArea + "";
            //if (Propety.NumObject == 0) Propety.NumObject = 1;
            //Propety.ckBitwiseNot = Propety.ckBitwiseNot;
            //Propety.ckSIMD = Propety.ckSIMD;
            //Propety.ckSubPixel = Propety.ckSubPixel;
            //ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            //ckSIMD.IsCLick = Propety.ckSIMD;
            //ckSubPixel.IsCLick = Propety.ckSubPixel;
            //Propety.TypeMode = Propety.TypeMode;
            //if (Propety.IsHighSpeed)
            //    btnHighSpeed.IsCLick = true;
            //else
            //    btnNormal.IsCLick = true;
        }
        Mat matRS = new Mat(); 
        private void trackScore_ValueChanged(float obj)
        {
           Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
           

        }

     
        public Mat matTemp = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        //public void GetTemp(RectRotate rotateRect, Mat matRegister)
        //{
           
        //        float angle = rotateRect._rectRotation;
        //        if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
        //        Mat matCrop =BeeCore.Cropper.CropRotatedRectSharp(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
        //        if (matCrop.Type() == MatType.CV_8UC3)
        //            Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
        //        if (Propety.IsAreaWhite)
        //            Cv2.BitwiseNot(matTemp, matTemp);
           
        //}
        public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        {
           

            if (Propety.rotAreaAdjustment == null && Global.IsRun) return gc;
            gc.ResetTransform();
            // gc.FillEllipse(Brushes.Black, Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y, 6, 6);

            var mat = new Matrix();
            mat.Translate(pScroll.X, pScroll.Y);
            mat.Scale(Scale, Scale);
          
            RectRotate rotA = Propety.rotArea;
            if (Global.IsRun) rotA = Propety.rotAreaAdjustment;
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            //gc.FillEllipse(Brushes.Blue, -3, -3, 6, 6);
            gc.DrawString(indexTool + "", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));

            gc.DrawRectangle(new Pen(Color.Silver, 4), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
            gc.ResetTransform();
            mat = new Matrix();
            mat.Translate(pScroll.X, pScroll.Y);
            mat.Scale(Scale, Scale);
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            Bitmap bmTemp = matTemp.ToBitmap();
            bmTemp.MakeTransparent(Color.Black);
            bmTemp = ConvertImg.ChangeToColor(bmTemp, Color.Green, 0.8f);
            gc.DrawImage(bmTemp, rotA._rect);
            if (!Propety.IsOK)
            {
              
               
               
                if (!matRS.Empty())
                {
                    Bitmap bmRS = matRS.ToBitmap();
                   
                    bmRS.MakeTransparent(Color.Black);

                    bmRS = ConvertImg.ChangeToColor(bmRS, Color.Red, 0.9f);
                    gc.DrawImage(bmRS, rotA._rect);
                }
             
              

                RectangleF _rect = rotA._rect;
                gc.DrawRectangle(new Pen(Color.Red, 4), new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));

                gc.ResetTransform();
                return gc;
            }
            else
            {
             
                RectangleF _rect = rotA._rect;
                gc.DrawRectangle(new Pen(Color.Green, 4), new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));

                gc.ResetTransform();
                return gc;
            }    
            //else
            //{
            //    foreach (RectRotate rot in Propety.rectRotates)
            //    {
            //        mat = new Matrix();
            //        mat.Translate(pScroll.X, pScroll.Y);
            //        mat.Scale(Scale, Scale);
            //        mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            //        mat.Rotate(rotA._rectRotation);
            //        mat.Translate(rotA._rect.X, rotA._rect.Y);
            //        gc.Transform = mat;
            //        mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
            //        mat.Rotate(rot._rectRotation);
            //        gc.Transform = mat;
            //        Mat matShow = matTemp.Clone();

            //        if (Propety.TypeMode == Mode.OutLine)
            //        {
            //            Bitmap myBitmap = matShow.ToBitmap();
            //            myBitmap.MakeTransparent(Color.Black);
            //            myBitmap = ConvertImg.ChangeToColor(myBitmap, Color.FromArgb(0, 255, 0), 1f);


            //            gc.DrawImage(myBitmap, rot._rect);
            //        }
            //        gc.FillEllipse(Brushes.Blue, -3, -3, 6, 6);
            //        gc.DrawRectangle(new Pen(Color.LimeGreen, 2), new Rectangle((int)rot._rect.X, (int)rot._rect.Y, (int)rot._rect.Width, (int)rot._rect.Height));
            //        gc.ResetTransform();
            //    }
            //}


            return gc;
        }

      
        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp == null) return gc;

            //if (TypeCrop != TypeCrop.Area)
            //    try
            //    {
            //        Mat matShow = matTemp.Clone();
            //        if (Propety.TypeMode == Mode.OutLine)
            //        {
            //            Bitmap bmTemp = matShow.ToBitmap();

            //            bmTemp.MakeTransparent(Color.Black);
            //            bmTemp = ConvertImg.ChangeToColor(bmTemp, Color.FromArgb(0, 255, 0), 1f);

            //            gc.DrawImage(bmTemp, _rect);
            //        }
            //        if (matMask != null)
            //        {
            //            Bitmap myBitmap2 = matMask.ToBitmap();
            //            myBitmap2.MakeTransparent(Color.Black);
            //            myBitmap2 = ConvertImg.ChangeToColor(myBitmap2, Color.OrangeRed, 1f);

            //            gc.DrawImage(myBitmap2, _rect);
            //        }

            //    }
            //    catch (Exception ex) { }
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
            Global.TypeCrop = TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;

         
           // G.EditTool.View.imgView.Invalidate();
           // G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            
           // G.EditTool.View.imgView.Invalidate();
           // G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

       
        private void btnCannyMin_Click(object sender, EventArgs e)
        {
            //Propety.threshMin = 180;
            //Propety.threshMax = 255;
            //Propety.LearnPattern(indexTool, matTemp);

        }

        private void btnCannyMedium_Click(object sender, EventArgs e)
        {
            //Propety.threshMin = 100;
            //Propety.threshMax = 255;
            //Propety.LearnPattern(indexTool, matTemp);
        }

        private void btnCannyMax_Click(object sender, EventArgs e)
        {
            //Propety.threshMin = 0;
            //Propety.threshMax = 255;
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();

        }

    
        
      
        
        Bitmap bmResult ;
      
        public int indexTool = 0;
      
        private void trackScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackMaxOverLap_Scroll(object sender, EventArgs e)
        {
            lbOverLap.Text = trackMaxOverLap.Value + " %";
            //Propety.OverLap = (trackMaxOverLap.Value * 1) / 100.0;
        }

        private void trackMaxOverLap_MouseUp(object sender, MouseEventArgs e)
        {
          

            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }
        
       
        private void btnSubAngle_Click(object sender, EventArgs e)
        {
            Propety.MinArea -= 10;
            if (Propety.MinArea < 0) Propety.MinArea = 0;
               txtAngle.Text = Propety.MinArea+"";
            //if(Propety.Angle==0)
            //{
            //    Propety.AngleLower = Propety.rotCrop._rectRotation - 1;
            //    Propety.AngleUper = Propety.rotCrop._rectRotation + 1;
            //}
            //else
            //{
            //    Propety.AngleLower = Propety.rotCrop._rectRotation - Propety.Angle;
            //    Propety.AngleUper = Propety.rotCrop._rectRotation + Propety.Angle;
            //}    

            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }

        private void btnPlusAngle_Click(object sender, EventArgs e)
        {
            Propety.MinArea += 10;
            if (Propety.MinArea > 1000) Propety.MinArea = 1000;
            txtAngle.Text = Propety.MinArea + "";
            //if (Propety.Angle == 0)
            //{
            //    Propety.AngleLower = Propety.rotCrop._rectRotation - 1;
            //    Propety.AngleUper = Propety.rotCrop._rectRotation + 1;
            //}
            //else
            //{
            //    Propety.AngleLower = Propety.rotCrop._rectRotation - Propety.Angle;
            //    Propety.AngleUper = Propety.rotCrop._rectRotation + Propety.Angle;
            //}
            if (!threadProcess.IsBusy)
               threadProcess.RunWorkerAsync();
        }

        private void ckSIMD_Click(object sender, EventArgs e)
        {
            //Propety.ckSIMD = !Propety.ckSIMD;
            //  if(Propety.ckSIMD)
            //    {
            //    ckSIMD.BackColor = Color.Goldenrod;
            //    ckSIMD.BorderColor = Color.DarkGoldenrod;
            //    }
            //    else
            //    { 
            //    ckSIMD.BackColor = Color.WhiteSmoke;
            //    ckSIMD.BorderColor = Color.Silver;
            //    ckSIMD.TextColor = Color.Black;
            //    }
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void ckBitwiseNot_Click(object sender, EventArgs e)
        {
            //Propety.ckBitwiseNot = !Propety.ckBitwiseNot;
            //if (Propety.ckBitwiseNot)
            //{
            //    ckBitwiseNot.BackColor = Color.Goldenrod;
            //    ckBitwiseNot.BorderColor = Color.DarkGoldenrod;
            //}
            //else
            //{
            //    ckBitwiseNot.BackColor = Color.WhiteSmoke;
            //    ckBitwiseNot.BorderColor = Color.Silver;
            //    ckBitwiseNot.TextColor = Color.Black;
            //}
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void ckSubPixel_Click(object sender, EventArgs e)
        {
            //Propety.ckSubPixel = !Propety.ckSubPixel;
            //if (Propety.ckSubPixel)
            //{
            //    ckSubPixel.BackColor = Color.Goldenrod;
            //    ckSubPixel.BorderColor = Color.DarkGoldenrod;
            //}
            //else
            //{
            //    ckSubPixel.BackColor = Color.WhiteSmoke;
            //    ckSubPixel.BorderColor = Color.Silver;
            //    ckSubPixel.TextColor = Color.Black;
            //}
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }
     
        public void Loads()
        {
          
          
            imgTemp.Image = Propety.matTemp;

            //Propety.NumObject = 1;
        }
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
            Loads();

        
        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
       public bool IsClear = false;
        private void btnClear_Click(object sender, EventArgs e)
        {
            //btnClear.IsCLick = !btnClear.IsCLick;
            //IsClear = btnClear.IsCLick;
            //G.EditTool.View.Cursor = new Cursor(Properties.Resources.Erase1.Handle);



           



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
        }

        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
            //Propety.IsAreaWhite = false;
            // GetTemp(Propety.rotCrop,BeeCore.Common.listCamera[Global. IndexChoose].matRaw );
            //G.EditTool.View.imgView.Invalidate();
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
          //  Propety.IsHighSpeed = false;
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
          //  Propety.IsHighSpeed = true;

        }
        private void btnAreaWhite_Click(object sender, EventArgs e)
        {
            //Propety.IsAreaWhite = true;
            //GetTemp(Propety.rotCrop, BeeCore.Common.listCamera[Global. IndexChoose].matRaw);
            //G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
          
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(float obj)
        {
            //Propety.NumObject = trackNumObject.Value;
        }

        private void btnLearning_Click(object sender, EventArgs e)
        {

            matTemp = Propety.GetImgTemp();

            //if (Propety.rotCrop != null)
            //    if (Propety.rotCrop._rect.Width != 0 && Propety.rotCrop._rect.Height != 0)
            //    {
            //        Propety.LearnPattern(indexTool, matTemp);

            //    }
            imgTemp.Image = matTemp.ToBitmap();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
           
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize;
        private void btnEntire_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            if (Propety.rotAreaTemp != null)
                Propety.rotArea = Propety.rotAreaTemp.Clone();
           
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void btnPartial_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width / 2, -BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height / 2, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height), new PointF(BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width / 2, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height / 2), 0, AnchorPoint.None);

           
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            Propety.Invert = btnInvert.IsCLick;
            if(btnInvert.IsCLick)
            {
                btnInvert.Text = "Outside";
            }
            else
            {
                btnInvert.Text = "Inside";
            }    
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void txtAngle_KeyDown(object sender, KeyEventArgs e)
        {
           
        }
    }
}
