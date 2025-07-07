using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeCore;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolColorArea : UserControl
    {
     
        public TypeTool TypeTool;
        public ToolColorArea( )
        {
            InitializeComponent();
        }
        public int indexTool;
        
        public ColorArea Propety=new ColorArea();
        public Mat matTemp = new Mat();
       
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public bool IsClear;
        public BackgroundWorker worker = new BackgroundWorker();
        Stopwatch timer = new Stopwatch();
        public void LoadPara( )
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
                if (e.Error != null)
                {
                    //  MessageBox.Show("Worker error: " + e.Error.Message);
                    return;
                }
                Propety.Complete();
                Global.StatusDraw = StatusDraw.Check;
                timer.Stop();

                Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;
            };
          
            if(Propety.listCLShow==null)
                Propety.listCLShow = new List<Color>();
            //if(G.Config.TypeCamera==TypeCamera.USB)
            Propety.TypeTool = TypeTool.Color_Area;
            Propety.LoadTemp();
            trackScore.Value = Propety.Score ;
            trackPixel.Value = (int)Propety.AreaPixel;
            if (!Convert.ToBoolean(Propety.StyleColor))
                btnColor.IsCLick = true;
            else
                btnClWhite.IsCLick = true;

            trackScore.Value = Propety.Score;
           
        }
        public Mat RotateMat(Mat raw, RotatedRect rot)
        {
            Mat matRs = new Mat(), matR = Cv2.GetRotationMatrix2D(rot.Center, rot.Angle, 1);

            float fTranslationX = (rot.Size.Width - 1) / 2.0f - rot.Center.X;
            float fTranslationY = (rot.Size.Height - 1) / 2.0f - rot.Center.Y;
            matR.At<double>(0, 2) += fTranslationX;
            matR.At<double>(1, 2) += fTranslationY;
            Cv2.WarpAffine(raw, matRs, matR, new OpenCvSharp.Size(rot.Size.Width, rot.Size.Height), InterpolationFlags.Linear, BorderTypes.Constant);
            return matRs;
        }
        public Mat matCrop=new Mat();
        public Mat GetTemp(RectRotate rotateRect)
        {
          
            float angle = rotateRect._rectRotation;
            if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
             matCrop =  RotateMat( BeeCore.Common.listCamera[Global.IndexChoose].matRaw, new RotatedRect(new Point2f(rotateRect._PosCenter.X, rotateRect._PosCenter.Y), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), rotateRect._angle));
            //Cv2.ImWrite("cropColor.png", matCrop);
            
            picColor.Invalidate();
            return Propety.SetColor(Global.IsRun, matCrop);
           
        }
        
        public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        {
            if (Propety.rotAreaAdjustment == null && Global.IsRun) return gc;
            gc.ResetTransform();
            var mat = new Matrix();
          
            RectRotate rotA = Propety.rotArea;
            if (Global.IsRun) rotA = Propety.rotAreaAdjustment;
            if (!Global.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
           
            gc.Transform = mat;

            gc.DrawRectangle(new Pen(Color.Silver, 1), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
       
              gc.ResetTransform();

            Color cl = Color.LimeGreen;
            if (!Propety.IsOK)
            {
                cl = Color.Red;
                //if (BeeCore.Common.PropetyTools[IndexChoose][Propety.Index].UsedTool == UsedTool.Invertse &&
                //    G.Config.ConditionOK == ConditionOK.Logic)
                //    cl = Color.LimeGreen;


            }
            else
            {
                cl = Color.LimeGreen;
                //if (BeeCore.Common.PropetyTools[IndexChoose][Propety.Index].UsedTool == UsedTool.Invertse &&
                //    G.Config.ConditionOK == ConditionOK.Logic)
                //    cl = Color.Red;
            }
            int i = 0;

            mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
           // mat.Translate(rotA._rect.X, rotA._rect.Y);
            gc.Transform = mat;

            //mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            //mat.Rotate(rotA._rectRotation);
            //gc.Transform = mat;
            gc.DrawRectangle(new Pen(cl, 2), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
                if (Propety.bmRS == null) return gc;
                //if (G.IsDrawProcess)
                //{
                //  //  mat.Translate(rotA._rect.X, rotA._rect.Y);
                //  //  gc.Transform = mat;
                //    Bitmap myBitmap = Propety.bmRS;
                //    myBitmap.MakeTransparent(Color.Black);
                //    myBitmap = ConvertImg.ChangeToColor(myBitmap, cl, 1f);
                //    gc.DrawImage(myBitmap, rotA._rect);
                //}

                
               
            
            String s= (int)(  Propety.Index+1)+"."+ Propety.nameTool;
         SizeF sz=   gc.MeasureString(s, new Font("Arial", 10, FontStyle.Bold));
            gc.FillRectangle(Brushes.White, new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)sz.Width,(int) sz.Height));
            gc.DrawString(s, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));

            gc.ResetTransform();
            return gc;
        }

        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp.Empty())
            {
                float angle = Propety.rotArea._rectRotation;
                if (Propety.rotArea._rectRotation < 0) angle = 360 + Propety.rotArea._rectRotation;
                 matCrop = RotateMat(BeeCore.Common.listCamera[Global. IndexChoose].matRaw, new RotatedRect(new Point2f(Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y), new Size2f(Propety.rotArea._rect.Width, Propety.rotArea._rect.Height), Propety.rotArea._angle));

                matTemp = Propety.SetColor(false, matCrop);
               
            }
           

            try
                {
                    Mat matShow = matTemp.Clone();
                   
                    Bitmap bmTemp = matShow.ToBitmap();
                    bmTemp.MakeTransparent(Color.Black);
                    bmTemp = ConvertImg.ChangeToColor(bmTemp, Color.LimeGreen, 0.6f);

                    gc.DrawImage(bmTemp, _rect);
                    //if (matMask != null)
                    //{
                    //    Bitmap myBitmap2 = matMask.ToBitmap();
                    //    myBitmap2.MakeTransparent(Color.Black);
                    //    myBitmap2 = ConvertImg.ChangeToColor(myBitmap2, Color.Orange, 0.9f);
                    //    myBitmap2.MakeTransparent(Color.Black);

                    //    gc.DrawImage(myBitmap2, _rect);
                    //}

                }
                catch (Exception ex) { 
            }
            return gc;
        }
       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
           
           
            btnGetColor.IsCLick = false;
            Propety.IsGetColor = btnGetColor.IsCLick;
            Global.StatusDraw = StatusDraw.Check;
        }
        
      
     
      
       
    
       

        private void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           /// G.EditTool.View.imgView.Invalidate();
          //  Cv2.ImShow("a", bmRs);
          //  G.EditTool.View.lbCycleTrigger.Text = "[" + Propety.cycleTime + "ms]";
        }

        private void trackScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }

      
        private void trackMaxOverLap_MouseUp(object sender, MouseEventArgs e)
        {
          

            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }
        
      

     

      
      
      
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
            TypeTool = TypeTool.Color_Area;
          
          
          //  Propety.pathRaw = G.EditTool.View.pathRaw;
        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
      
      //  public ExtractColor ExtractColor;
       
        private void btnClear_Click(object sender, EventArgs e)
        {
           // Propety.SetRaw(BeeCore.Common.listCamera[Global. IndexChoose].matRaw);
            
            //if (ExtractColor == null)
            //{

            //    ExtractColor = new ExtractColor(Propety);
            //    ExtractColor.Parent = this;
            //    ExtractColor.BringToFront();
            //    ExtractColor.Location = new System.Drawing.Point(this.Width / 2 - ExtractColor.Width / 2, this.Height / 2 - ExtractColor.Height / 2);
            //}




            btnGetColor.IsCLick = !Propety.IsGetColor;
            Propety.IsGetColor = btnGetColor.IsCLick;
            if (Propety.IsGetColor)
            {

                // ExtractColor.Show();
                // G.EditTool.View.imgView.Cursor = new Cursor(Properties.Resources.Color_Dropper.Handle);

                Global.StatusDraw = StatusDraw.Edit;
                //  G.EditTool.View.imgView.Invalidate();
            }
            //else
              //  G.EditTool.View.imgView.Cursor = Cursors.Default;





           // G.EditTool.View.imgView.Invalidate();

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCropRect_Click_1(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            if (Propety.rotAreaTemp != null)
                Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;


        }

        private void rjButton1_Click(object sender, EventArgs e)
        {

        }

        bool IsFullSize;
        private void btnCropArea_Click_1(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width / 2, -BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height / 2, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height), new PointF(BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width / 2, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height / 2), 0, AnchorPoint.None, false);

           
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
           
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
            btnGetColor.IsCLick = false;
            Propety.IsGetColor = btnGetColor.IsCLick;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            Propety.StyleColor = 0;

        }

        private void btnClWhite_Click(object sender, EventArgs e)
        {
            Propety.StyleColor = 1;

            btnDeleteAll.PerformClick();
        }

        private void btnClBlack_Click(object sender, EventArgs e)
        {
            Propety.StyleColor = 2;

            btnDeleteAll.PerformClick();
        }

        private void trackScore_ValueChanged(float obj)
        {
           
            Propety.Score = (int)trackScore.Value ;
          

        }
       
        private void picColor_Click(object sender, EventArgs e)
        {
            
        }

        private void picColor_Paint(object sender, PaintEventArgs e)
        {
            int x = 0;int h = picColor.Height;int w = h;
            foreach (Color cl in Propety.listCLShow)
            {

                e.Graphics.FillRectangle(new SolidBrush( cl), new RectangleF(x, 0, w, h));
                e.Graphics.DrawRectangle(new Pen(Color.Black,1), new Rectangle(x, 0, w, h));
                x += w ;
            }
        }

        private void trackPixel_Validating(object sender, CancelEventArgs e)
        {

        }

        private void trackPixel_ValueChanged(float obj)
        {
            Propety.AreaPixel = (int)trackPixel.Value;
            if(matCrop.Empty())
            {
                float angle = Propety.rotArea._rectRotation;
                if (Propety.rotArea._rectRotation < 0) angle = 360 + Propety.rotArea._rectRotation;
                matCrop = RotateMat(BeeCore.Common.listCamera[Global. IndexChoose].matRaw, new RotatedRect(new Point2f(Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y), new Size2f(Propety.rotArea._rect.Width, Propety.rotArea._rect.Height), Propety.rotArea._angle));


            }
            matTemp =  Propety.SetColor(false, matCrop);
           
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            Propety.listCLShow.RemoveAt(Propety.listCLShow.Count - 1);
        
            picColor.Invalidate();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {

            
           
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            Propety.listCLShow = new List<Color>();
          //  G.EditTool.View.ClearTemp(Propety);
            picColor.Invalidate();
        }

        private void pMode_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMask_Click(object sender, EventArgs e)
        {

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
          
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
    }
}
