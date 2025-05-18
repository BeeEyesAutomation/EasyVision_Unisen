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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeCore;
using BeeUi.Common;
using BeeUi.Commons;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;

namespace BeeUi.Tool
{
    [Serializable()]
    public partial class ToolPattern : UserControl
    {
        
        public ToolPattern( )
        {
            InitializeComponent();
            
        }
        public BackgroundWorker worker = new BackgroundWorker();
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
            Bitmap bmTemp = Propety.matTemp;
            if (bmTemp != null)
            {
                Propety.LearnPattern( OpenCvSharp.Extensions.BitmapConverter.ToMat(bmTemp));
               
                   
            }
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
            Propety.TypeMode = Propety.TypeMode;
            if (Propety.IsHighSpeed)
                btnHighSpeed.IsCLick = true;
            else
                btnNormal.IsCLick = true;
        }
        private void trackScore_ValueChanged(int obj)
        {
            Propety.Score = trackScore.Value;
            numScore.Value = Propety.Score;
            G.IsCheck = true;
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();

        }

        public OutLine Propety=new OutLine();
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public void GetTemp(RectRotate rotateRect, Mat matRegister)
        {
           
                float angle = rotateRect._rectRotation;
                if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
                Mat matCrop = G.EditTool.View.CropRotatedRect(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
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
           

            if (Propety.rotAreaAdjustment == null && G.IsRun) return gc;
            if(G.IsRun)
            gc.ResetTransform();
            // gc.FillEllipse(Brushes.Black, Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y, 6, 6);
            gc.FillEllipse(Brushes.Green, 295, 950, 10, 10);
            var mat = new Matrix();
            if (!G.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }

            RectRotate rotA = Propety.rotArea;
            if (G.IsRun) rotA = Propety.rotAreaAdjustment;
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
          //  gc.FillEllipse(Brushes.Green, -5, -5, 10, 10);
         //   gc.DrawString(rotA._PosCenter.X + "," + rotA._PosCenter.Y, new Font("Arial", 22, FontStyle.Bold), Brushes.Red, new System.Drawing.Point(0, 20));

            //gc.FillEllipse(Brushes.Blue, -3, -3, 6, 6);
            gc.DrawString(indexTool+"", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));

            gc.DrawRectangle(new Pen(Color.Blue, 5), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
            gc.ResetTransform();
            if (!Propety.IsOK)
            {
                Color cl = Color.Red;
                if (G.PropetyTools[this.indexTool].UsedTool == UsedTool.Invertse &&
                    G.Config.ConditionOK == ConditionOK.Logic)
                    cl = Color.LimeGreen;
                if (Propety.rectRotates.Count > 0)
                {
                    int i = 1;
                    foreach (RectRotate rot in Propety.rectRotates)
                    {
                        mat = new Matrix();
                        if (!G.IsRun)
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
                        gc.DrawString(i + "", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rot._rect.X, (int)rot._rect.Y));
                        i++;
                        gc.FillEllipse(Brushes.Black,-3,-3,6,6);
                        gc.DrawRectangle(new Pen(cl, 4), new Rectangle((int)rot._rect.X, (int)rot._rect.Y, (int)rot._rect.Width, (int)rot._rect.Height));
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
                    //mat.Translate(Propety.rotCrop._PosCenter.X, Propety.rotCrop._PosCenter.Y);
                    //mat.Rotate(Propety.rotCrop._rectRotation);
                    gc.Transform = mat;
                    RectangleF _rect =rotA._rect;
                    if (G.PropetyTools[this.indexTool].UsedTool == UsedTool.Invertse &&
                          G.Config.ConditionOK == ConditionOK.Logic)
                        gc.DrawRectangle(new Pen(Color.LimeGreen, 4), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
                    else
                        gc.DrawRectangle(new Pen(Color.Red, 4), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
                  //  gc.DrawRectangle(new Pen(Color.Red, 4), new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                }
                gc.ResetTransform();
                return gc;
            }
            else
            {
                Color cl = Color.LimeGreen;
                if (G.PropetyTools[this.indexTool].UsedTool == UsedTool.Invertse &&
                    G.Config.ConditionOK == ConditionOK.Logic)
                    cl = Color.Red;
                int i = 0;
                System.Drawing.Point pZero = new System.Drawing.Point(0, 0);
                PointF[] pMatrix = { pZero };
                foreach (RectRotate rot in Propety.rectRotates)
                {
                    mat = new Matrix();
                    if (!G.IsRun)
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
                    mat.TransformPoints(pMatrix);
                    gc.Transform = mat;
                    gc.DrawString(i + "", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rot._rect.X, (int)rot._rect.Y));
                    gc.DrawString(Propety.ScoreRs + "%", new Font("Arial", 10, FontStyle.Bold), Brushes.Green, new System.Drawing.Point((int)rot._rect.X, (int)rot._rect.Y+10));
                    i++;
                 //   gc.FillEllipse(Brushes.Green, -5, -5, 10, 10);
                   // var (x, y) = ConvertToGlobalCoordinates(rot._PosCenter.X- Propety.rotAreaAdjustment._rect.Width / 2, rot._PosCenter.Y - Propety.rotAreaAdjustment._rect.Height / 2, Propety.rotAreaAdjustment._PosCenter.X, Propety.rotAreaAdjustment._PosCenter.Y , Propety.rotAreaAdjustment._angle);
                   int x= (int)pMatrix[0].X;
                    int y = (int)pMatrix[0].Y;
                   // gc.DrawString(x+ "," + y , new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new System.Drawing.Point(0, 0));
                   
                    var (x2Rot, y2Rot) = ConvertToB(x, y, G.pOrigin.X, G.pOrigin.Y,G.AngleOrigin);

                    //  var (x2Rot, y2Rot) = RotateRelativeToPoint(x, y, G.pOrigin.X, G.pOrigin.Y, G.AngleOrigin);
                    //if (x > G.AngleOrigin && x2Rot < 0)
                    //    x2Rot = -x2Rot;
                    //if (y > G.AngleOrigin && y2Rot < 0)
                    //    y2Rot = -y2Rot;

                    //if (x < G.AngleOrigin && x2Rot > 0)
                    //    x2Rot = -x2Rot;
                    //if (y < G.AngleOrigin && y2Rot > 0)
                    //    y2Rot = -y2Rot;
                    double distance = Math.Round( Math.Sqrt(Math.Pow(x - G.pOrigin.X, 2) + Math.Pow(y - G.pOrigin.Y, 2)));
                    int angle =  (int)rot._angle;//(int)G.AngleOrigin -
                    angle =- angle;
                    if (angle < 0) angle = 360 + angle;
                   gc.DrawRectangle(new Pen(cl, 4), new Rectangle((int)rot._rect.X, (int)rot._rect.Y, (int)rot._rect.Width, (int)rot._rect.Height));
                   
                  //  Draws.Plus(gc, 0, 0, 20, Color.LimeGreen, 4);

                  //  gc.ResetTransform();
                  //  mat = new Matrix();
                  ////  mat.Rotate(G.AngleOrigin);
                  //  mat.Translate(G.pOrigin.X, G.pOrigin.Y);
                  //  gc.Transform = mat;
                  // // gc.DrawLine(new Pen(Brushes.LightGray, 2), 0, 0, (float)x2Rot, (float)y2Rot);
                  //  SizeF sz = gc.MeasureString("D = " + distance + " pixel", new Font("Arial", 16, FontStyle.Bold));
                  //  gc.FillRectangle(Brushes.White, new Rectangle(8, 8, (int)sz.Width + 30, 60));
                  //  gc.DrawString("R = " + distance + " pixel", new Font("Arial", 16, FontStyle.Bold), Brushes.OrangeRed, new System.Drawing.Point(10, 10));
                  //  gc.DrawString("Alpha =" + angle + "°", new Font("Arial", 16, FontStyle.Bold), Brushes.OrangeRed, new System.Drawing.Point(10, 40));
                  //  // 
                  //  G.EditTool.View.lbAngle.Text = angle+"";
                    gc.ResetTransform();
                }
            }



            return gc;
        }

     
        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp == null) return gc;

            if (G.TypeCrop != TypeCrop.Area)
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

    
        
      
        public void Process()
        {
            //Propety.rectRotates = new List<RectRotate>();
            //if (G.IsRun)
            //{
            //    if (G.rotOriginAdj != null)
            //        Propety.rotAreaAdjustment = G.EditTool.View.GetPositionAdjustment(Propety.rotArea, G.rotOriginAdj);
            //    else
            //        Propety.rotAreaAdjustment = Propety.rotArea;
            //    Propety.rotAreaAdjustment._angle = 0;
            //    Propety.Matching(G.IsRun, BeeCore.Common.matRaw, indexTool, Propety.rotAreaAdjustment);

            //}
            //else
            //    Propety.Matching(G.IsRun, BeeCore.Common.matRaw, indexTool, Propety.rotArea);
        }
        Bitmap bmResult ;
        private void threadProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            if (G.IsLoad)
                Process();
        }
        public int indexTool = 0;
        private void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            btnTest.IsCLick = false;
            G.EditTool.View.imgView.Invalidate();

            G.ResultBar.lbCycleTrigger.Text = "[" + Propety.cycleTime + "ms]";
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
     
        public void Loads()
        {
            Propety.TypeTool = TypeTool.Pattern;
     
            Propety.TypeMode = Mode.Pattern;
            Propety.pathRaw = G.EditTool.View.pathRaw;
            imgTemp.Image = Propety.matTemp;

            //Propety.NumObject = 1;
        }
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
            Loads();
            this.tabP1.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.BG, G.Config.colorGui);
           // this.trackNumObject.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
           // layScore.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
       public bool IsClear = false;
        private void btnClear_Click(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.imgView.Cursor = Cursors.Default;
            //btnClear.IsCLick = !btnClear.IsCLick;
            //IsClear = btnClear.IsCLick;
            //G.EditTool.View.Cursor = new Cursor(Properties.Resources.Erase1.Handle);



            //G.EditTool.View.imgView.Invalidate();



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            G.IsCancel = true;
            
            G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
            Propety.IsAreaWhite = false;
             GetTemp(Propety.rotCrop,BeeCore.Common.matRaw );
            G.EditTool.View.imgView.Invalidate();
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
            GetTemp(Propety.rotCrop, BeeCore.Common.matRaw);
            G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            G.IsCheck = true;
            G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(int obj)
        {
            Propety.NumObject = trackNumObject.Value;
        }

        private void btnLearning_Click(object sender, EventArgs e)
        {
          
                      matTemp = Propety.GetTemp(Propety.rotCrop, BeeCore.Common.matRaw, G.EditTool.View.bmMask);
                if (Propety.rotCrop != null)
                    if (Propety.rotCrop._rect.Width != 0 && Propety.rotCrop._rect.Height != 0)
                    {
                        Propety.LearnPattern(matTemp);

                    }
                imgTemp.Image = matTemp.ToBitmap();
            

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            G.IsCheck = true;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
        private void btnCropHalt_Click(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            G.IsCheck = false;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
            if (IsClear)
                btnClear.PerformClick();
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-BeeCore.G.ParaCam.SizeCCD.Width / 2, -BeeCore.G.ParaCam.SizeCCD.Height / 2, BeeCore.G.ParaCam.SizeCCD.Width, BeeCore.G.ParaCam.SizeCCD.Height), new PointF(BeeCore.G.ParaCam.SizeCCD.Width / 2, BeeCore.G.ParaCam.SizeCCD.Height / 2), 0, AnchorPoint.None);

            G.IsCheck = false;
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
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
            numScore.Maxnimum = trackScore.Max;
            numScore.Minimum = trackScore.Min;
            Propety.Score = numScore.Value;
            trackScore.Value = Propety.Score;
        }

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

        private void trackAngle_ValueChanged(int obj)
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

        private void trackMaxOverLap_ValueChanged(int obj)
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
