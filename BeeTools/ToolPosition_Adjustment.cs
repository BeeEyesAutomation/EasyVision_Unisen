using BeeCore;
using BeeGlobal;
using BeeInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class ToolPosition_Adjustment : UserControl
    {
        
        public ToolPosition_Adjustment( )
        {
            InitializeComponent();
            
        }
  
        public OutLine Propety=new OutLine();
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
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
          //  G.EditTool.View.imgView.Cursor = Cursors.Default;

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
            Propety.LearnPattern( matTemp);

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
            //if (G.IsLoad)
            //    if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();

        }

        private void trackScore_Scroll(object sender, EventArgs e)
        {
           
           
        }
      //public void Process()
      //  {
      //   //   Propety.rectRotates = new List<RectRotate>();
      //      if (BeeCore.Common.listCamera[Global. IndexChoose].matRaw == null) return;
      //      //Mat raw = BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Clone();
      //      //if (BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Type() == MatType.CV_8UC3)
      //      //{
      //      //    raw = raw.CvtColor(ColorConversionCodes.BGR2GRAY);
      //      //}
      //  //    Propety.rectArea = new RectangleF(Propety. rotArea._PosCenter.X + Propety. rotArea._rect.Left, Propety. rotArea._PosCenter.Y + Propety. rotArea._rect.Top, Propety. rotArea._rect.Width, Propety. rotArea._rect.Height);

      //      //Propety.Matching(Global.IsRun,BeeCore.Common.listCamera[Global. IndexChoose].matRaw, Propety.indexTool, Propety. rotArea);
      //      if (Global.IsRun)
      //      {
      //          if (Propety.IsOK) 
      //          {
      //              if (G.rotOriginAdj == null) return;
      //              G.X_Adjustment = Propety.rotArea._PosCenter.X - Propety.rotArea._rect.Width / 2 + Propety.rectRotates[0]._PosCenter.X - G.rotOriginAdj._PosCenter.X;
      //              G.Y_Adjustment = Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + Propety.rectRotates[0]._PosCenter.Y - G.rotOriginAdj._PosCenter.Y;
      //              G.angle_Adjustment = Propety.rotArea._rectRotation + Propety.rectRotates[0]._rectRotation - G.rotOriginAdj._rectRotation;
      //          }
      //      }
      //      else
      //      {
      //          if(Propety.IsOK)
      //          {

      //              if (!Propety.IsOK) return;
      //              Propety.rotPositionAdjustment = Propety.rectRotates[0].Clone();
      //              G.rotOriginAdj =new RectRotate(Propety. rotCrop._rect,new PointF (Propety. rotArea._PosCenter.X - Propety. rotArea._rect.Width / 2 + Propety.rotPositionAdjustment._PosCenter.X, Propety. rotArea._PosCenter.Y - Propety. rotArea._rect.Height / 2 + Propety.rotPositionAdjustment._PosCenter.Y), Propety.rotPositionAdjustment._rectRotation,AnchorPoint.None);
                   
      //          }    
                
      //      }    

      //  }
        Bitmap bmResult;
        //private void threadProcess_DoWork(object sender, DoWorkEventArgs e)
        //{
           
        //}
        public Mat matTemp=new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public Mat matTrig;
      
        public Graphics ShowResult(Graphics gc, float Scale,System.Drawing.Point pScroll)
        {
           
            if (Global.IsRun)
                gc.ResetTransform();
            var mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }
            RectRotate rotA = Propety.rotArea;
           
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;



            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;

            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].Results==Results.NG)
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
            String nameTool = (int)(Propety.Index + 1) + "." + Common.PropetyTools[Global.IndexChoose][Propety.Index].Name;
            Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl, 2);
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
                    Draws.Plus(gc, 0, 0, (int)rot._rect.Width / 2, cl, 2);
                    //Draws.Box2Label(gc, rot._rect, i + "", Math.Round(Propety.listScore[i - 1], 1) + "%", Global.fontRS, cl, brushText, 16, 3);
                    gc.ResetTransform();
                    i++;
                }
            }



            //gc.ResetTransform();
            //if (G.rotOriginAdj!=null)
            //gc.FillEllipse(Brushes.Gold, G.rotOriginAdj._PosCenter.X+ pScroll.X, G.rotOriginAdj._PosCenter.Y+ pScroll.Y, 6, 6);
            //var mat = new Matrix();
            //RectRotate rotA = Propety. rotArea;
            //if (!Global.IsRun)
            //{
            //    mat.Translate(pScroll.X, pScroll.Y);
            //    mat.Scale(Scale, Scale);
            //}
            //mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            //mat.Rotate(rotA._rectRotation);
            //gc.Transform = mat;

            //gc.DrawString(Propety.Index+"", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));
            //Color clRect = Color.Silver;
            //switch ( G.StatusTrig )
            //{
            //    case Trig.Processing:
            //        clRect = Color.Red;
            //        break;
            //    case Trig.Trigged:
            //        clRect = Color.Green;
            //        break;
            //    case Trig.NotTrig:
            //        clRect = Color.Green;
            //        break;
               
            //}    
            //gc.DrawRectangle(new Pen(clRect, 2), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
          
            //gc.ResetTransform();
            //if (!Propety.IsOK)
            //{
            //    mat = new Matrix();
            //    if (!Global.IsRun)
            //    {
            //        mat.Translate(pScroll.X, pScroll.Y);
            //        mat.Scale(Scale, Scale);
            //    }
            //    mat.Translate(Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y);
            //    mat.Rotate(Propety.rotArea._rectRotation);
            //    gc.Transform = mat;
            //    //Mat matShow = new Mat();
            //    //if (Propety.TypeMode == Mode.OutLine)

            //    //{
            //    //    matShow = matTemp.Clone();
            //    //    Bitmap bmTemp = matShow.ToBitmap();
            //    //    bmTemp.MakeTransparent(Color.Black);
            //    //    bmTemp = ConvertImg.ChangeToColor(bmTemp, Color.Red, 0.7f);
            //    //    gc.DrawImage(bmTemp, Propety.rotCrop._rect);
            //    //}
            //    RectangleF _rect = Propety.rotArea._rect;
            //    gc.DrawRectangle(new Pen(Color.Red, 2), new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));

            //    gc.ResetTransform();
            //    return gc;
            //}
            //else
            //{
            //    System.Drawing.Point pZero = new System.Drawing.Point(0, 0);
            //    PointF[] pMatrix = { pZero };
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
            //        mat.TransformPoints(pMatrix);
            //        gc.Transform = mat;
            //        Mat matShow = matTemp.Clone();

            //        if (Propety.TypeMode == Mode.OutLine)
            //        {
            //            Bitmap myBitmap = matShow.ToBitmap();
            //            myBitmap.MakeTransparent(Color.Black);
            //            myBitmap = ConvertImg.ChangeToColor(myBitmap, Color.FromArgb(0, 255, 0), 1f);


            //            gc.DrawImage(myBitmap, rot._rect);
            //        }
            //        gc.FillEllipse(Brushes.Blue, -5, -5, 10, 10);
            //        int x = (int)pMatrix[0].X;// (int)Propety.rotArea._PosCenter.X -(int) Propety.rotArea ._rect.Width/2 + (int)rot._PosCenter.X;
            //        int y = (int)pMatrix[0].Y; ;// (int)Propety.rotArea._PosCenter.Y - (int)Propety.rotArea._rect.Height / 2 + (int)rot._PosCenter.Y;
            //        G.AngleOrigin = rot._angle;
            //        G.pOrigin = new OpenCvSharp.Point(x, y);
            //        Draws.Plus(gc, 0, 0, 20, Color.LightGoldenrodYellow, 2);
            //        gc.DrawString(0 + ","+0, new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new System.Drawing.Point(0,10));
            //        gc.DrawRectangle(new Pen(Color.LimeGreen, 2), new Rectangle((int)rot._rect.X, (int)rot._rect.Y, (int)rot._rect.Width, (int)rot._rect.Height));
            //        gc.ResetTransform();
            //    }
            //}

           
            return gc;
        }
        public TypeTool TypeTool = TypeTool.Position_Adjustment;
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

      

        private void trackScore_ValueChanged(object sender, EventArgs e)
        {
            //lbScore.Text = trackScore.Value + " %";
            
        }

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
          
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
           
        }
       
       
        public void Loads()
        {
           
            Propety.TypeMode = Mode.Pattern;
            Propety.NumObject = 1;
          //  Propety.pathRaw = G.EditTool.View.pathRaw;
            imgTemp.Image = Propety.matTemp;
        }
        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();
        public  void LoadPara()
        {
            worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                timer = new Stopwatch();
                timer.Restart();
                Propety.DoWork(Propety.rotArea);
               
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
                
                Propety.Complete();
                timer.Stop();

                Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;
                
              
             
                

            }
                ;
            Bitmap bmTemp = Propety.matTemp;
            //Herlo
            if (bmTemp != null)
            {
                Propety.LearnPattern(OpenCvSharp.Extensions.BitmapConverter.ToMat(bmTemp));
             if(Propety.rotPositionAdjustment != null)
                    Global.rotOriginAdj = new RectRotate(Propety.rotCrop._rect, new PointF(Propety.rotArea._PosCenter.X - Propety.rotArea._rect.Width / 2 + Propety.rotPositionAdjustment._PosCenter.X, Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + Propety.rotPositionAdjustment._PosCenter.Y), Propety.rotPositionAdjustment._rectRotation, AnchorPoint.None, false);
            }
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score ;
            trackAngle.Value =(int)Propety.Angle;
            trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
            //txtAngle.Text = (int)Propety.Angle + "";
            Propety.LimitCounter = 1;
            Propety.ckBitwiseNot = Propety.ckBitwiseNot;
            Propety.ckSIMD = Propety.ckSIMD;
            Propety.ckSubPixel = Propety.ckSubPixel;
            ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            ckSIMD.IsCLick = Propety.ckSIMD;
            ckSubPixel.IsCLick = Propety.ckSubPixel;
            Propety.TypeMode = Propety.TypeMode;
         
            Propety.rotMask = null;
            //if (Propety.IsAutoTrig)
            //    btnAutoTrigger.IsCLick = true;
            //else
            //    btnAutoTrigger.IsCLick = false;
            numDelay.Value = Propety.DelayTrig;
            numOK.Value=Propety.NumOK;
            //if (Propety.TypeMode==Mode.Pattern)
            //    btnPattern.IsCLick=true;
            //else
            //    btnOutLine.IsCLick = true;
            if (Propety.IsHighSpeed )
                btnHighSpeed.IsCLick = true;
            else
                btnNormal.IsCLick = true;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;

        }
            private void ToolOutLine_Load(object sender, EventArgs e)
        {
            Loads();


        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
        public bool IsFullSize=false;
        public    bool IsClear = false;
        //public Eraser Eraser;
        //private void btnClear_Click(object sender, EventArgs e)
        //{
          
        //    if (Eraser == null)
        //    {
               
        //        Eraser = new Eraser();
        //        Eraser.Parent =this;
        //        Eraser.BringToFront();
        //        Eraser.Location = new System.Drawing.Point(this.Width / 2 - Eraser.Width / 2, this.Height / 2 - Eraser.Height / 2);
        //    }
           
       
       

        //    //btnClear.IsCLick = !IsClear;
        //    //IsClear = btnClear.IsCLick;
        //    //if(IsClear)
        //    //{
        //    //    tmClear.Enabled = true;
        //    //    Eraser.Show();
        //    //    G.EditTool.View.Cursor = new Cursor(Properties.Resources.Erase1.Handle);
        //    //   Global.TypeCrop= TypeCrop.Crop;
        //    //    G.IsCheck = false;
        //    //    G.EditTool.View.imgView.Invalidate();
        //    //}
        //    //else
        //    //    Eraser.Hide();




        //    G.EditTool.View.imgView.Invalidate();
         

        //}

        //private void btnUndo_Click(object sender, EventArgs e)
        //{
        //    G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Cols, MatType.CV_8UC1);

        //}

        //private void tabPage3_Click(object sender, EventArgs e)
        //{
           
        //}

        private void tmClear_Tick(object sender, EventArgs e)
        {
            
        }

     

        private void btnNormal_Click(object sender, EventArgs e)
        {
         Propety. IsHighSpeed = false;
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = true;

        }

        private void btnMask_Click(object sender, EventArgs e)
        {
           
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.StatusDraw = StatusDraw.Check;

        }

        private void btnOutLine_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.OutLine;
           
        }

        private void btnPattern_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Pattern;
         
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
         //   G.IsCancel = true;
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
           
        }

        private void ckAutoTrigger_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void numOK_ValueChanged(object sender, EventArgs e)
        {
            Propety.NumOK = (int)numOK.Value;
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            Propety.DelayTrig = (int)numDelay.Value;
        }

        private void trackScore_MouseMove(object sender, MouseEventArgs e)
        {
            Common.PropetyTools[Global.IndexChoose][Propety. Index].Score = (int)trackScore.Value ;
        }

        private void trackScore_Load(object sender, EventArgs e)
        {

        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
            //numScore.Value =Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;

        }

     

        private void btnAutoTrigger_Click(object sender, EventArgs e)
        {
          // Propety.IsAutoTrig = btnAutoTrigger.IsCLick;
        }

        private void btnLearning_Click(object sender, EventArgs e)
        {
          
                matTemp = Propety.GetTemp(Propety.rotCrop, Propety.rotMask,BeeCore.Common.listCamera[Propety.IndexThread].matRaw,null);
                if (Propety.rotCrop != null)
                    if (Propety.rotCrop._rect.Width != 0 && Propety.rotCrop._rect.Height != 0)
                    {
                        Propety.LearnPattern( matTemp);

                    }
                imgTemp.Image = matTemp.ToBitmap();
            
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
           


            if (!worker.IsBusy)
                worker.RunWorkerAsync();
           
        }

        private void btnOnTrig_Click(object sender, EventArgs e)
        {
            Propety.IsAutoTrig = btnOnTrig.IsCLick;
        }

        private void btnOffTrig_Click(object sender, EventArgs e)
        {
            Propety.IsAutoTrig = btnOffTrig.IsCLick;
        }

        private void numScore_ValueChanged(object sender, EventArgs e)
        {

            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
            //numScore.Value =Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
        }
        private void trackAngle_ValueChanged(float obj)
        {
            Propety.Angle = trackAngle.Value;
            numAngle.Value = (int)Propety.Angle;
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
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
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
            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }

        private void trackMaxOverLap_ValueChanged(float obj)
        {

            Propety.OverLap = (trackMaxOverLap.Value*1.0)/100.0;
            numOverLap.Value = (int)(Propety.OverLap*100.0);
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void numOverLap_ValueChanged(object sender, EventArgs e)
        {
            Propety.OverLap = (numOverLap.Value * 1.0)/ 100.0;
            trackMaxOverLap.Value = (int)(Propety.OverLap*100.0);
        }

        private void btnCropHalt_Click(object sender, EventArgs e)
        {
            Global.StatusDraw = StatusDraw.None;
            Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            Global.StatusDraw = StatusDraw.None;
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None, false);


            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.Edit;

        }
    }
}
