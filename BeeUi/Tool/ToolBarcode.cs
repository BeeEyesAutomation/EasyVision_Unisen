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
using BeeUi.Common;
using BeeUi.Commons;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace BeeUi.Tool
{
    [Serializable()]
    public partial class ToolBarcode : UserControl
    {
        
        public ToolBarcode( )
        {
            InitializeComponent();
            
        }

        public BackgroundWorker worker = new BackgroundWorker();
        public BarCode Propety = new BarCode();
        public void LoadPara()
        {
            worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
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

            };
            // Bitmap bmTemp = Propety.matTemp;
            txtQRCODE.Text = Propety.MathQRCODE;
            //if (bmTemp != null)
            //{
            //    matTemp = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmTemp);
            //    Propety.LoadTemp(matTemp);


            //}
            // btnInvert.IsCLick = Propety.Invert;
            // trackScore.Value =Propety.Score;
            //trackNumObject.Value = Propety.NumObject;
            //trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
            //   txtAngle.Text = (int)Propety.MinArea + "";
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
            //Propety.Score = trackScore.Value;
            G.IsCheck = true;
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();

        }

     
        public Mat matTemp = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        //public void GetTemp(RectRotate rotateRect, Mat matRegister)
        //{

        //        float angle = rotateRect._rectRotation;
        //        if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
        //        Mat matCrop = G.EditTool.View.CropRotatedRect(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
        //        if (matCrop.Type() == MatType.CV_8UC3)
        //            Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
        //        if (Propety.IsAreaWhite)
        //            Cv2.BitwiseNot(matTemp, matTemp);

        //}
        public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        {
            if (Propety.rotAreaAdjustment == null && G.IsRun) return gc;
            gc.ResetTransform();
            // gc.FillEllipse(Brushes.Black, Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y, 6, 6);

            var mat = new Matrix();
            RectRotate rotA = Propety.rotArea;
            if (G.IsRun) rotA = Propety.rotAreaAdjustment;
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            //gc.FillEllipse(Brushes.Blue, -3, -3, 6, 6);
            gc.DrawString(indexTool + "", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));

            gc.DrawRectangle(new Pen(Color.Silver, 1), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
            gc.ResetTransform();
            Color cl = Color.Red;
            if (!Propety.IsOK)
            {
                 cl = Color.Red;
                if (G.PropetyTools[Propety.Index].UsedTool == UsedTool.Invertse &&
                    G.Config.ConditionOK == ConditionOK.Logic)
                    cl = Color.LimeGreen;
            }
            else
            {
                 
               
                     cl = Color.LimeGreen;
                    if (G.PropetyTools[Propety.Index].UsedTool == UsedTool.Invertse &&
                        G.Config.ConditionOK == ConditionOK.Logic)
                        cl = Color.Red;
                
            }
            if (HEROJE.BarCodeRegion == null) return gc;
                if (HEROJE.BarCodeRegion.Length > 0)
                {
                    int i = 0;
                    
                    foreach (Polygon pol in HEROJE.BarCodeRegion)
                {
                        mat = new Matrix();
                        if (!G.IsRun)
                        {
                            mat.Translate(pScroll.X, pScroll.Y);
                            mat.Scale(Scale, Scale);
                        }
                        //mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                        //mat.Rotate(rotA._rectRotation);
                        //mat.Translate(rotA._rect.X, rotA._rect.Y);
                        gc.Transform = mat;

                      
                           System.Drawing. Point[] array4 = pol.ToPointArray();
                            
                            gc.SmoothingMode = SmoothingMode.AntiAlias;
                            gc.DrawLines(new Pen(cl,4), array4);
                    mat = new Matrix();
                    mat.Translate(array4[0].X, array4[0].Y);
                    gc.Transform = mat;

                    int index = i + 1;
                            String content = "" + Propety.Content + " \n";
                    int widthBox = Math.Max(array4[0].X, array4[1].X);
                    int xCenter = Math.Min(array4[0].X, array4[1].X) + Math.Abs(array4[0].X - array4[1].X) / 2;
                    int YCenter = Math.Min(array4[0].Y, array4[1].Y) + Math.Abs(array4[0].Y - array4[1].Y) / 2;
                   
                    int w = Math.Abs(array4[0].X - array4[1].X);
                    int h = Math.Abs(array4[0].Y - array4[1].Y);
                    Font font = new Font("Arial", 10, FontStyle.Bold);
                            SizeF sz = gc.MeasureString(content, font);
                    gc.FillRectangle(Brushes.White, 0,0, sz.Width + 4, sz.Height + 4);
                            gc.DrawString(content, font, new SolidBrush(cl), new System.Drawing.Point(0, 0));
                            i++;
                            //gc.FillEllipse(Brushes.Black, -3, -3, 6, 6);
                            gc.ResetTransform();
                        
                    }
                }
                else
                {
                    mat = new Matrix();
                    if (!G.IsRun)
                    {
                        mat.Translate(pScroll.X, pScroll.Y);
                        mat.Scale(Scale, Scale);
                    }
                    mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                    mat.Rotate(rotA._rectRotation);

                    gc.Transform = mat;
                    RectangleF _rect = Propety.rotArea._rect;
                    if (G.PropetyTools[Propety.Index].UsedTool == UsedTool.Invertse &&
                     G.Config.ConditionOK == ConditionOK.Logic)
                        gc.DrawRectangle(new Pen(Color.LimeGreen, 4), new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));

                    else
                        gc.DrawRectangle(new Pen(Color.Red, 4), new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                    // 
                }
                gc.ResetTransform();

            
           
            



            return gc;
        }
        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp == null) return gc;

            //if (G.TypeCrop != TypeCrop.Area)
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

    
        
      
        public void Process()
        {
           
           // Propety.LoadTemp(matTemp);
            if (G.IsRun)
            {
                if (G.rotOriginAdj != null)
                    Propety.rotAreaAdjustment = G.EditTool.View.GetPositionAdjustment(Propety.rotArea, G.rotOriginAdj);
                else
                    Propety.rotAreaAdjustment = Propety.rotArea;
                matRS = Propety.Read( Propety.rotAreaAdjustment);

            }
            else
                matRS = Propety.Read(Propety.rotArea);
        // BeeCore.Common.GetImageResult().Clone();
        }
        Bitmap bmResult ;
        private void threadProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            Process();
        }
        public int indexTool = 0;
        private void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          
              
            btnTest.IsCLick = false;
            G.EditTool.View.imgView.Invalidate();
         
            //G.EditTool.View.lbCycleTrigger.Text = "[" + Propety.cycleTime + "ms]";
        }

        private void trackScore_ValueChanged(object sender, EventArgs e)
        {

        }

      
     
       
        
       
      
        public void Loads()
        {
            Propety.TypeTool = TypeTool.BarCode;
     
          
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



            G.EditTool.View.imgView.Invalidate();



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            G.IsCancel = true;
            this.Parent.Controls.Remove(this);
            G.ToolSettings.Visible = true;
        }

        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
            //Propety.IsAreaWhite = false;
            // GetTemp(Propety.rotCrop,BeeCore.Common.matRaw );
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
            //GetTemp(Propety.rotCrop, BeeCore.Common.matRaw);
            //G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            G.IsCheck = true;
            this.Parent.Controls.Remove(this);
          
            G.ToolSettings.Visible = true;
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(float obj)
        {
            //Propety.NumObject = trackNumObject.Value;
        }

      

        private void btnTest_Click(object sender, EventArgs e)
        {
            G.IsCheck = true;
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize;
        private void btnEntire_Click(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;
            IsFullSize = false;
            if (Propety.rotAreaTemp != null)
                Propety.rotArea = Propety.rotAreaTemp.Clone();
            G.IsCheck = false;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
        }

        private void btnPartial_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-BeeCore.Common.matRaw.Width / 2, -BeeCore.Common.matRaw.Height / 2, BeeCore.Common.matRaw.Width, BeeCore.Common.matRaw.Height), new PointF(BeeCore.Common.matRaw.Width / 2, BeeCore.Common.matRaw.Height / 2), 0, AnchorPoint.None, false);

            G.IsCheck = false;
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
        }

      

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void txtAngle_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCropHalt_Click(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            G.IsCheck = false;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-BeeCore.G.ParaCam.SizeCCD.Width / 2, -BeeCore.G.ParaCam.SizeCCD.Height / 2, BeeCore.G.ParaCam.SizeCCD.Width, BeeCore.G.ParaCam.SizeCCD.Height), new PointF(BeeCore.G.ParaCam.SizeCCD.Width / 2, BeeCore.G.ParaCam.SizeCCD.Height / 2), 0, AnchorPoint.None, false);

            G.IsCheck = false;
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {
            G.IsCheck = true;
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            G.IsCheck = true;
            G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            G.IsCancel = true;

            G.EditTool.RefreshGuiEdit(Step.Step3);
        }


        private void txtQRCODE_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtQRCODE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQRCODE.Text = txtQRCODE.Text.Replace("\n", "");
                Propety.MathQRCODE = txtQRCODE.Text;
            }
            }

        private void btnSet_Click(object sender, EventArgs e)
        {
            Propety.MathQRCODE = Propety.Content;
            txtQRCODE.Text = Propety.MathQRCODE;
        }
    }
}
