using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeCore;
using BeeUi.Common;
using BeeUi.Commons;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace BeeUi.Tool
{
    [Serializable()]
    public partial class ToolEdgePixels : UserControl
    {
        
        public ToolEdgePixels( )
        {
            InitializeComponent();
            
        }
        private void trackScore_ValueChanged(float obj)
        {
            Propety.Score = (int)trackScore.Value;
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();

        }
        public TypeTool TypeTool = TypeTool.Edge_Pixels;

        public EdgePixel Propety;
        public int indexTool;
        public bool IsClear;
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
           
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }
        public Mat matTemp = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public void GetTemp(RectRotate rotateRect)
        {
             Mat matCrop = RotateMat(BeeCore.Common.listCamera[G.indexChoose].matRaw, new RotatedRect(new Point2f(rotateRect._PosCenter.X  , rotateRect._PosCenter.Y ), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), rotateRect._angle));
            Mat matOut = new Mat();
            if (Propety.IsBitNot)
                Cv2.BitwiseNot(matCrop, matCrop);
            Cv2.Canny(matCrop, matOut, Propety.threshMin, Propety.threshMax);
            Propety.NumPixelTemp = Cv2.CountNonZero(matOut);
            Mat crop = G.EditTool.View.CropRotatedRect(G.EditTool.View.bmMask, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), rotateRect._rectRotation));

            Cv2.BitwiseNot(crop, matClear);
            Mat rs = new Mat();
            Cv2.BitwiseAnd(matClear, matOut, matTemp);

            
            Cv2.BitwiseAnd(crop, matOut, matMask);
        }
   

        public Graphics ShowResult(Graphics gc)
        {
            gc.ResetTransform();
            var mat = new Matrix();
            RectRotate rotA = rotArea;
            if (G.IsRun) rotA = Propety.rotAreaAdjustment;
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;

            gc.DrawRectangle(new Pen(Color.Silver, 1), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
            gc.ResetTransform();
            if (!Propety.IsOK)
            {
                mat = new Matrix();

                mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                mat.Rotate(rotA._rectRotation);
                gc.Transform = mat;
                Bitmap bmTemp = matRs.ToBitmap();
                bmTemp.MakeTransparent(Color.Black);
                bmTemp = ConvertImg.ChangeToColor(bmTemp, Color.Red, 0.7f);


                RectangleF _rect = rotA._rect;
                gc.DrawRectangle(new Pen(Color.Red, 2), new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                gc.DrawImage(bmTemp, rotA._rect);
                gc.ResetTransform();
                return gc;
            }
            else
            {
                mat = new Matrix();

                mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                mat.Rotate(rotA._rectRotation);

                gc.Transform = mat;
               
                Mat matShow = matRs.Clone();
                //if (TypeTool == TypeTool.Pattern)
                //{
                //    Cv2.BitwiseNot(matShow, matShow);
                //}
                Bitmap myBitmap = matShow.ToBitmap();
                myBitmap.MakeTransparent(Color.Black);
                myBitmap = ConvertImg.ChangeToColor(myBitmap, Color.FromArgb(0, 255, 0), 1f);


                gc.DrawImage(myBitmap, rotA._rect);
                gc.DrawRectangle(new Pen(Color.LimeGreen, 1), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
                gc.ResetTransform();
            }
            Propety.rectRotates = new List<RectRotate>();


            return gc;
        }
        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp == null) return gc;

            
                try
                {
                    Mat matShow = matTemp.Clone();

                    Bitmap bmTemp = matShow.ToBitmap();
                    bmTemp.MakeTransparent(Color.Black);
                    bmTemp = ConvertImg.ChangeToColor(bmTemp, Color.FromArgb(0, 255, 0), 1f);

                    gc.DrawImage(bmTemp, _rect);
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
        Mat RotateMat(Mat raw, RotatedRect rot)
        {
            Mat matRs=new Mat(), matR = Cv2.GetRotationMatrix2D(rot.Center, rot.Angle, 1);

            float fTranslationX = (rot.Size.Width - 1) / 2.0f - rot.Center.X;
            float fTranslationY = (rot.Size.Height - 1) / 2.0f - rot.Center.Y;
            matR.At<double>(0, 2) += fTranslationX;
            matR.At<double>(1, 2) += fTranslationY;
            Cv2.WarpAffine(raw, matRs, matR, new OpenCvSharp.Size(rot.Size.Width,rot.Size.Height),InterpolationFlags.Linear, BorderTypes.Constant);
            return matRs;
        }
        Mat matRs = new Mat();
        public Mat Compare(Mat raw, bool IsAreWhite)
        {
            Mat matProces = raw.Clone();
          
            if (raw.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(raw, matProces, ColorConversionCodes.BGR2GRAY);
            if (IsAreWhite)
                Cv2.BitwiseNot(matProces, matProces);
            Cv2.Canny(matProces.Clone(), matRs, Propety.threshMin, Propety.threshMax);
            Propety.NumPixelComPare = Cv2.CountNonZero(matRs);
            if(Propety.NumPixelComPare> (Propety.NumPixelTemp* Propety.Score)/100)
           {
                Propety.IsOK = true;
            }
            else
                Propety.IsOK = false;
            return matRs;
        }
        private void btnCropRect_Click(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Crop;
            Propety.TypeCrop = G.TypeCrop;

         
            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;
            
            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

        public RJButton ButtonPress( RJButton control ,Control parent)
        {
            foreach (Control con in parent.Controls)
            {
                if(con==control)
                {
                    control.BackColor = Color.Goldenrod;
                    control.BorderColor = Color.DarkGoldenrod;
                }  
                else
                {
                    RJButton btn = con as RJButton;
                    btn.BackColor = Color.WhiteSmoke;
                    btn.BorderColor = Color.Silver;
                    btn.TextColor = Color.Black;
                }    
            }
            return control;


        }
        private void btnCannyMin_Click(object sender, EventArgs e)
        {
            Propety.threshMin = 180;
            Propety.threshMax = 255;
            if (Propety.TypeCrop == TypeCrop.Crop)
                GetTemp(rotArea);
            else
                 if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();

        }

        private void btnCannyMedium_Click(object sender, EventArgs e)
        {
            if (Propety.TypeCrop == TypeCrop.Crop)
                GetTemp(rotArea);
            else
                GetTemp(rotArea);
                  if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();

        }

        private void btnCannyMax_Click(object sender, EventArgs e)
        {
            Propety.threshMin = 0;
            Propety.threshMax = 255;
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();

        }

      
        public RectRotate rotArea, rotCrop, rotMask, rotAreaTemp;
        Bitmap bmResult ;
        private void threadProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            Process();
        }
        public void Process()
        {
            Propety.rotAreaAdjustment = new RectRotate();
            Propety.rotAreaAdjustment._rect = rotArea._rect;
            Propety.rotAreaAdjustment._PosCenter = new PointF(rotArea._PosCenter.X + G.X_Adjustment, rotArea._PosCenter.Y + G.Y_Adjustment);
            Propety.rotAreaAdjustment._angle = rotArea._rectRotation + G.angle_Adjustment;



            Mat matCrop = new Mat(); 
                if(G.IsRun)
            {
                matCrop= RotateMat(BeeCore.Common.listCamera[G.indexChoose].matRaw, new RotatedRect(new Point2f(Propety.rotAreaAdjustment._PosCenter.X, Propety.rotAreaAdjustment._PosCenter.Y), new Size2f(Propety.rotAreaAdjustment._rect.Width, Propety.rotAreaAdjustment._rect.Height), Propety.rotAreaAdjustment._angle));

            }
                else
            {
                matCrop= RotateMat(BeeCore.Common.listCamera[G.indexChoose].matRaw, new RotatedRect(new Point2f(rotArea._PosCenter.X, rotArea._PosCenter.Y), new Size2f(rotArea._rect.Width, rotArea._rect.Height), rotArea._angle));

            }
            Compare(matCrop, Propety.IsBitNot);
        }
        private void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            G.EditTool.View.imgView.Invalidate();
            // G.EditTool.View.imgView.ImageIpl = null;
            //  G.EditTool.View.imgView.Image = bmResult;
            G.ResultBar.lbCycleTrigger.Text = "[" + Propety.cycleTime + "ms]";
        }

        private void trackScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }

      

   
       
          public void Loads()
        {
            TypeTool = TypeTool.Edge_Pixels;
           
        }
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
            Loads();


        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            G.IsCancel = true;
            G.EditTool.RefreshGuiEdit(Step.Step3);
        }
        bool IsFullSize;

        private void btnCropArea_Click_1(object sender, EventArgs e)
        {
            IsFullSize = true;
            rotAreaTemp = rotArea.Clone();
            rotArea = new RectRotate(new RectangleF(-BeeCore.Common.listCamera[G.indexChoose].matRaw.Width / 2, -BeeCore.Common.listCamera[G.indexChoose].matRaw.Height / 2, BeeCore.Common.listCamera[G.indexChoose].matRaw.Width, BeeCore.Common.listCamera[G.indexChoose].matRaw.Height), new PointF(BeeCore.Common.listCamera[G.indexChoose].matRaw.Width / 2, BeeCore.Common.listCamera[G.indexChoose].matRaw.Height / 2), 0, AnchorPoint.None, false);

            G.IsCheck = false;
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
        }

        private void btnCropRect_Click_1(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;
            IsFullSize = false;
            if(rotAreaTemp!=null)
            rotArea = rotAreaTemp.Clone();
            G.IsCheck = false;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
           
        }
        bool IsMask;
        private void btnMask_Click(object sender, EventArgs e)
        {
            btnMask.IsCLick = !IsMask;
            IsMask = btnMask.IsCLick;
            if (IsMask)
            {
                int with = 50, height = 50;
                if (rotMask == null)
                    rotMask = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(BeeCore.Common.listCamera[G.indexChoose].matRaw.Width / 2, BeeCore.Common.listCamera[G.indexChoose].matRaw.Height / 2), 0, AnchorPoint.None, false);
                G.TypeCrop = TypeCrop.Mask;
                G.EditTool.View.imgView.Invalidate();
            }
            else
            {
                rotMask = null;
                G.TypeCrop = TypeCrop.Area;
                G.EditTool.View.imgView.Invalidate();
            }
           
        }

        private void btnNot_Click(object sender, EventArgs e)
        {
            Propety.IsBitNot =! Propety.IsBitNot;
            if(Propety.TypeCrop==TypeCrop.Crop)
            GetTemp(rotArea);
            else
                if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }
    }
}
