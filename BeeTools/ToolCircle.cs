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
using BeeCore.Funtion;
using BeeGlobal;

using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolCircle : UserControl
    {
        
        public ToolCircle( )
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
                if (e.Error != null)
                {
                    //  MessageBox.Show("Worker error: " + e.Error.Message);
                    return;
                }
                Propety.Complete();
              
                timer.Stop();

                Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;

            };
            Propety.TypeTool = TypeTool.Circle;
            Propety.StatusTool = StatusTool.Initialed;
            trackCany.Value =(int) Propety.Cany;
            trackDp.Value =(float) Propety.Dp;
          //  numAngle.Value = (int)Propety.Angle;
            trackScore.Value =Propety.Score; 
            trackNumObject.Value= Propety.NumObject;
      
            trackDistance.Value =Propety.Distance;
            numMinRadius.Value = Propety.MinRadius;
            numMaxRadius.Value = Propety.MaxRadius;

        }
        private void trackScore_ValueChanged(float obj)
        {
            Propety.Score = (int)trackScore.Value;
            numScore.Value = Propety.Score;
          

        }

        public Circle Propety=new Circle();
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
    
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
                


            }
            else
            {
                cl = Color.LimeGreen;
          
            }
            String nameTool = (int)(Propety.Index + 1) + "." + Propety.nameTool;
            Draws.Box1Label(gc, rotA._rect, nameTool, Global.fontTool, brushText, cl,1);
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
                    Draws.Plus(gc, 0, 0, (int)rot._rect.Width / 6, cl, 2);
                    gc.DrawEllipse(new Pen(cl, 2), rot._rect);
                    gc.DrawString("R:"+rot._rect.Width/2,new Font("Arial",16,FontStyle.Bold),new SolidBrush(cl),rot._PosCenter);
                  //  Draws.Box2Label(gc, rot._rect, i+"", Math.Round(Propety.listScore[i-1], 1) + "%", Global.fontRS, cl, brushText, 16, 2);

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
            Global.TypeCrop = TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;
            btnElip.IsCLick = Propety.rotCrop.IsElip;
            btnRect.IsCLick = !Propety.rotCrop.IsElip;
          
         

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            btnElip.IsCLick = Propety.rotArea.IsElip;
            btnRect.IsCLick = !Propety.rotArea.IsElip;
         
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
            //    Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global.IndexChoose].matRaw, indexTool, Propety.rotAreaAdjustment);

            //}
            //else
            //    Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global.IndexChoose].matRaw, indexTool, Propety.rotArea);
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

     
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
           // Loads();
            //this.tabP1.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.BG, G.Config.colorGui);
           // this.trackNumObject.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
           // layScore.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
       public bool IsClear = false;
        private void btnClear_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Mask;
            Propety.TypeCrop = Global.TypeCrop;
            if(Propety.rotMask==null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
            }
            btnElip.IsCLick = Propety.rotMask.IsElip;
            btnRect.IsCLick = !Propety.rotMask.IsElip;

          
            //btnClear.IsCLick = !btnClear.IsCLick;
            //IsClear = btnClear.IsCLick;
            //G.EditTool.View.Cursor = new Cursor(Properties.Resources.Erase1.Handle);



            //G.EditTool.View.imgView.Invalidate();



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }

       
        private void btnNormal_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = false;
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = true;

        }
      

        private void btnOK_Click(object sender, EventArgs e)
        {
        
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(float obj)
        {
            Propety.NumObject = trackNumObject.Value;
        }

    

        private void btnTest_Click(object sender, EventArgs e)
        {
            Global.StatusDraw = StatusDraw.Check;
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

            if (IsClear)
                btnClear.PerformClick();
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None,false);

         
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            if (IsClear)
                btnClear.PerformClick();
            Global.StatusDraw = StatusDraw.Edit;
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



        private void trackMaxOverLap_ValueChanged(float obj)
        {

           Propety.OverLap= trackDp.Value/100.0 ;
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
            trackDp.Value = (int)(Propety.OverLap * 100.0);
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
            
        }

        private void btnElip_Click(object sender, EventArgs e)
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
           
        }

      

      

        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global.TypeCrop)
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
           
        }

     

        private void trackCany_ValueChanged(float obj)
        {
            Propety.Cany = (int)trackCany.Value;
        }

        private void trackDp_ValueChanged(float obj)
        {
            Propety.Dp=(float)trackDp.Value;
        }

        private void numMinRadius_ValueChanged(object sender, EventArgs e)
        {Propety.MinRadius= (int)numMinRadius.Value;

        }

        private void numMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            Propety.MaxRadius = (int)numMaxRadius.Value;
        }

        private void trackDistance_ValueChanged(float obj)
        {
            Propety.Distance= (int)trackDistance.Value;
        }
    }
}
