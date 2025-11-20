using BeeCore;
using BeeCore.Func;
using BeeGlobal;
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolMeasure : UserControl
    {
        
        public ToolMeasure( )
        {
            InitializeComponent();
            cbMeasure.DataSource = Enum.GetValues(typeof(TypeMeasure));
            cbDirect.DataSource = Enum.GetValues(typeof(DirectMeasure));
            cbMethord.DataSource = Enum.GetValues(typeof(MethordMeasure));

        }
        PointF pIntersection;
        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();
        bool IsDone1, IsDone2, IsDone3, IsDone4;
        Tuple<double, double, double> ListAngle = new Tuple<double, double, double>(0, 0, 0);
        PointF pCenter1, pCenter2, pCenter3, pCenter4;
        /// <summary>
        /// 
        /// </summary>
        public void LoadPara()
        {
         
         
           // numScale.Value=(decimal)Propety.Scale ;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            if (Propety.listPointChoose.Count>=4)
            {
                if (Propety.listPointChoose[0].Item1 != null)
                {
                    cb1.SelectedItem = Propety.listPointChoose[0].Item1;
                    cb3.SelectedItem = Propety.listPointChoose[1].Item1;
                    cb5.SelectedItem = Propety.listPointChoose[2].Item1;
                    cb7.SelectedItem = Propety.listPointChoose[3].Item1;

                    int index = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == Propety.listPointChoose[0].Item1);
                    PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][index];
                    cb2.DataSource = null;
                    cb2.DataSource = propetyTool.Propety.listP_Center;
                    index = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == Propety.listPointChoose[1].Item1);
                    propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][index];
                    cb4.DataSource = null;
                    cb4.DataSource = propetyTool.Propety.listP_Center;
                    index = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == Propety.listPointChoose[2].Item1);
                    propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][index];
                    cb6.DataSource = null;
                    cb6.DataSource = propetyTool.Propety.listP_Center;
                    index = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == Propety.listPointChoose[3].Item1);
                    propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][index];
                    cb8.DataSource = null;
                    cb8.DataSource = propetyTool.Propety.listP_Center;
                    cb2.SafeSelectIndex(Propety.listPointChoose[0].Item2);
                    cb4.SafeSelectIndex(Propety.listPointChoose[1].Item2);
                    cb6.SafeSelectIndex(Propety.listPointChoose[2].Item2);
                    cb8.SafeSelectIndex(Propety.listPointChoose[3].Item2);

                  
                }
            }
          
            //worker.RunWorkerCompleted += (sender, e) =>
            //{
            //    try
            //    {


            //    }
            //    catch (Exception ex)
            //    {
            //        // MessageBox.Show("Kết quả không hợp lệ: " + ex.Message);
            //    }
            //    //if (Propety.StatusTool == StatusTool.Processing)
            //    //{
            //    //    Task.Delay(5);
            //    //    worker.RunWorkerAsync();
            //    //    return;
            //    //}
            //    if(!Global.IsRun)
            //    Global.StatusDraw = StatusDraw.Check;
            //    timer.Stop();

            //   // Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;

            //};
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolYolo_StatusToolChanged;

        }
        private void ToolYolo_StatusToolChanged(StatusTool obj)
        {
            if (Global.IsRun) return;
            if (Propety.Index >= Common.PropetyTools[Global.IndexChoose].Count)
                return;
            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
               // Propety.rectTrain = Propety.rectRotates;//note
                btnTest.Enabled = true;
            }
        }
        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = trackScore.Value;

        }

        public Measure Propety =new Measure();
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
     
       
        public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        {
           

           // if (Propety.rotAreaAdjustment == null && Global.IsRun) return gc;
            if(Global.IsRun)
            gc.ResetTransform();
            
            var mat = new Matrix();
            //if (!Global.IsRun)
            //{
            //    mat.Translate(pScroll.X, pScroll.Y);
            //    mat.Scale(Scale, Scale);
            //}

           
            Color cl = Color.LimeGreen;
            Brush brushText = Brushes.White;
            //if (!Propety.IsOK)
            //{
            //    cl = Color.Red;
            //    //if (BeeCore.Common.PropetyTools[Propety.IndexThread][Propety.Index].UsedTool == UsedTool.Invertse &&
            //    //    G.Config.ConditionOK == ConditionOK.Logic)
            //    //    cl = Color.LimeGreen;


            //}
            //else
            //{
            //    cl = Color.LimeGreen;
            //    //if (BeeCore.Common.PropetyTools[Propety.IndexThread][Propety.Index].UsedTool == UsedTool.Invertse &&
            //    //    G.Config.ConditionOK == ConditionOK.Logic)
            //    //    cl = Color.Red;
            //}
         
              
                int i = 0;
               
            //foreach (Point p in Propety.listLine1Point)
            //{
            //    mat = new Matrix();
            //    if (!Global.IsRun)
            //    {
            //        mat.Translate(pScroll.X, pScroll.Y);
            //        mat.Scale(Scale, Scale);
            //    }
            //    gc.Transform = mat;
            //    mat.Translate(p.X, p.Y);
             
            //    gc.Transform = mat;
            //    Draws.Plus(gc, 0, 0, 10, cl, 4);

            //    gc.ResetTransform();


            //}
            //foreach (Point p in Propety.listLine2Point)
            //{
            //    mat = new Matrix();
            //    if (!Global.IsRun)
            //    {
            //        mat.Translate(pScroll.X, pScroll.Y);
            //        mat.Scale(Scale, Scale);
            //    }
            //    gc.Transform = mat;
            //    mat.Translate(p.X, p.Y);

            //    gc.Transform = mat;
            //    Draws.Plus(gc, 0, 0, 10, Color.Blue, 4);

            //    gc.ResetTransform();


            //}
            mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }
            gc.Transform = mat;
            float radius = 40;
            float x = Math.Min(pCenter1.X, pCenter2.X) + Math.Abs(pCenter1.X - pCenter2.X) / 2;
            float y = Math.Min(pCenter1.Y, pCenter2.Y) + Math.Abs(pCenter1.Y - pCenter2.Y) / 2;
            RectangleF rect = new RectangleF(
                x,
               y,
                  radius * 2,
                  radius * 2
              );
            switch (Propety.TypeMeasure)
            {
                case TypeMeasure.Distance:

                    switch (Propety.DirectMeasure)
                    {
                        case DirectMeasure.XY:
                            gc.DrawLine(new Pen(Color.Blue, 2), pCenter1, pCenter2);
                            gc.DrawString("D=" + Propety.AngleDetect, new Font("Arial", 22, FontStyle.Bold),new SolidBrush(cl), new System.Drawing.PointF(x, y + 5));
                            break;
                        case DirectMeasure.Y:
                            gc.DrawLine(new Pen(Color.Blue, 2), pCenter1, pCenter2);
                            gc.DrawLine(new Pen(Color.Blue, 2), pCenter3, pCenter4);
                            gc.DrawString("D=" + Propety.AngleDetect, new Font("Arial", 22, FontStyle.Bold), new SolidBrush(cl), new System.Drawing.PointF(x, y + 5));
                            break;
                        case DirectMeasure.X:
                          //  Draws.Plus(gc,(int) pCenter2.X, (int)pCenter2.Y,10, cl, 4);
                          //  Draws.Plus(gc, (int)pCenter4.X, (int)pCenter4.Y, 10, cl, 4);
                            gc.DrawLine(new Pen(cl, 4), pCenter1, pCenter2);
                            gc.DrawLine(new Pen(cl, 4), pCenter3, pCenter4);
                            gc.DrawLine(new Pen(cl, 4), pCenter2, pCenter4);
                            gc.DrawString("D=" + Propety.AngleDetect, new Font("Arial", 22, FontStyle.Bold), new SolidBrush(cl), new System.Drawing.PointF(pCenter2.X, pCenter2.Y +10));
                            break;

                    }

                    break;
                case TypeMeasure.Angle:
                    Draws.DrawInfiniteLine(gc, pCenter1, pCenter2, Global.ClientRectangleMain, new Pen(Color.DeepPink, 2));
                    Draws.DrawInfiniteLine(gc, pCenter3, pCenter4, Global.ClientRectangleMain, new Pen(Color.DeepPink, 2));
                    gc.DrawString( Propety.AngleDetect+"o", new Font("Arial", 22, FontStyle.Bold), Brushes.OrangeRed, new System.Drawing.PointF(x, y + 50));

                    break;
            }
           // p1 = new Point(Propety.listLine2Point[0].X, Propety.listLine2Point[0].Y);
           //  p2 = new Point(Propety.listLine2Point[1].X, Propety.listLine2Point[1].Y);
           // Draws.DrawInfiniteLine(gc, p1, p2, Global.ClientRectangleMain, new Pen(Color.DeepPink, 2));
            //float startAngle =(float) ListAngle.Item1; // độ
            //float endAngle = (float)ListAngle.Item2; // độ
            //float sweepAngle = (float)ListAngle.Item3; // độ
            //if (sweepAngle > 180)
            //    sweepAngle = 360 - sweepAngle;
            //int radius = 40;
            //int x = Math.Min(p1.X, p2.X) + Math.Abs(p1.X - p2.X) / 2;
            //int y = Math.Min(p1.Y, p2.Y) + Math.Abs(p1.Y - p2.Y) / 2;
            //Rectangle rect = new Rectangle(
            //    x,
            //   y,
            //      radius * 2,
            //      radius * 2
            //  );
            
         //   gc.DrawArc(new Pen(cl, 1), rect, startAngle, sweepAngle);
         //   gc.FillPie(new SolidBrush(Color.FromArgb(30, cl)), rect, 0,(float) Propety.AngleDetect );
          
            //// Vùng chứa cung tròn
            //RectangleF arcRect = new RectangleF(
            //    B.X - radius,
            //    B.Y - radius,
            //    2 * radius,
            //    2 * radius
            //);
            //return tuple;

            //// Vẽ cung tròn
            //g.DrawArc(arcPen, arcRect, (float)startAngle, (float)sweep);

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
           
        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
          
        }

       
        private void btnCannyMin_Click(object sender, EventArgs e)
        {
         
        }

        private void btnCannyMedium_Click(object sender, EventArgs e)
        {
          
        }

        private void btnCannyMax_Click(object sender, EventArgs e)
        {
         

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
            //    Propety.rotAreaAdjustment._rectRotation = 0;
            //    Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global.IndexChoose].matRaw, indexTool, Propety.rotAreaAdjustment);

            //}
            //else
            //    Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global.IndexChoose].matRaw, indexTool, Propety.rotArea);
        }
        Bitmap bmResult ;
        private void threadProcess_DoWork(object sender, DoWorkEventArgs e)
        {
           // if (G.IsLoad)
           //     Process();
        }
        public int indexTool = 0;
        private void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            btnTest.IsCLick = false;
           // G.EditTool.View.imgView.Invalidate();

          //  G.ResultBar.lbCycleTrigger.Text = "[" + Propety.cycleTime + "ms]";
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
          
        }

        private void ckBitwiseNot_Click(object sender, EventArgs e)
        {
        }

        private void ckSubPixel_Click(object sender, EventArgs e)
        {
         
        }
     
       
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
           
         //   this.tabP1.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.BG, G.Config.colorGui);
           // this.trackNumObject.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
           // layScore.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
       public bool IsClear = false;
        private void btnClear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
         //   G.IsCancel = true;
            
         //   G.EditTool.RefreshGuiEdit(Step.Step3);
        }

      

        private void btnNormal_Click(object sender, EventArgs e)
        {
           
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
           

        }
     

        private void btnOK_Click(object sender, EventArgs e)
        {
           
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

       

      

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
      

     
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void rjButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void numScore_ValueChanged(object sender, EventArgs e)
        {
          
        }

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

        private void trackAngle_ValueChanged(float obj)
        {
         
        }

        private void numAngle_ValueChanged(object sender, EventArgs e)
        {
          
        }

        private void trackMaxOverLap_ValueChanged(float obj)
        {

          
        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }
        bool IsLoad1 , IsLoad2, IsLoad3, IsLoad4;
        private void ToolCalculator_Load(object sender, EventArgs e)
        {
            IsLoad1 = true;
            cb1.DataSource=  BeeCore.Common.PropetyTools[Propety.IndexThread].Select(p => p.Name).ToList();
            IsLoad2 = true;
            cb3.DataSource = BeeCore.Common.PropetyTools[Propety.IndexThread].Select(p => p.Name).ToList();
            IsLoad3 = true;
            cb5.DataSource = BeeCore.Common.PropetyTools[Propety.IndexThread].Select(p => p.Name).ToList();
            IsLoad4 = true;
            cb7.DataSource = BeeCore.Common.PropetyTools[Propety.IndexThread].Select(p => p.Name).ToList();
        }

        int indexTool1 = -1, indexTool2= -1, indexTool3 = -1, indexTool4 = -1;
        private void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoad1)
            {
                IsLoad1 = false;
                return;
            }
            if (cb1.SelectedIndex == -1) return;
            String nameTool=cb1.Text;
            indexTool1 = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a=>a.Name==nameTool);
            //PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1];
            PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle && propetyTool.TypeTool != TypeTool.Width) return;


            cb2.DataSource = null;
            cb2.DataSource = propetyTool.Propety.listP_Center;
            //dynamic outLine1 = BeeCore.Common.PropetyTools[Propety.IndexThread][BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name ==Properties.CH listPointChoose[0].Item1)].Propety;



            //        int index = listPointChoose[0].Item2;
            //        if (index < outLine1.listP_Center.Count)
            //        {
            //            listLine1Point[0] = outLine1.listP_Center[index];
            //            listRot[0] = outLine1.rectRotates[index];
            //        }

            //switch (propetyTool.TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = propetyTool.Propety as Patterns;
            //        if (Patterns == null) return;
            //        cb2.DataSource = null;
            //        cb2.DataSource = Patterns.listP_Center.ToList();
            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = propetyTool.Propety as Circle;
            //        if (circle == null) return;
            //        cb2.DataSource = null;
            //        cb2.DataSource = circle.listP_Center.ToList();
            //        break;
            //}
            //switch (propetyTool.TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = propetyTool.Propety as Patterns;
            //        if (Patterns == null) return;
            //        cb2.DataSource = null;
            //        cb2.DataSource = Patterns.listP_Center.ToList();
            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = propetyTool.Propety as Circle;
            //        if (circle == null) return;
            //        cb2.DataSource = null;
            //        cb2.DataSource = circle.listP_Center.ToList();
            //        break;
            //}
        }

        private void numScale_ValueChanged(object sender, EventArgs e)
        {
             //  Propety.Scale =(float) numScale.Value;
        }

        private void cb1_SelectionChangeCommitted(object sender, EventArgs e)
        {

            String nameTool = cb1.SelectedValue.ToString() ;
            indexTool1 = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == nameTool);
            //PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1];
            PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle && propetyTool.TypeTool != TypeTool.Width && propetyTool.TypeTool != TypeTool.Edge) return;


            cb2.DataSource = null;
            cb2.DataSource = propetyTool.Propety.listP_Center;
        }

        private void cb2_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void cb3_SelectionChangeCommitted(object sender, EventArgs e)
        {
           
            String nameTool = cb3.SelectedValue.ToString(); ;
            indexTool2 = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == nameTool);
            PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle && propetyTool.TypeTool != TypeTool.Width && propetyTool.TypeTool != TypeTool.Edge) return;
            cb4.DataSource = null;
            cb4.DataSource = propetyTool.Propety.listP_Center;

        }

        private void cb5_SelectionChangeCommitted(object sender, EventArgs e)
        {

         
            String nameTool = cb5.SelectedValue.ToString();
            indexTool3 = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == nameTool);
            PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle && propetyTool.TypeTool != TypeTool.Width && propetyTool.TypeTool != TypeTool.Edge) return;
            cb6.DataSource = null;
            cb6.DataSource = propetyTool.Propety.listP_Center;
        }

        private void cb7_SelectionChangeCommitted(object sender, EventArgs e)
        {
            String nameTool = cb7.SelectedValue.ToString();
            indexTool4 = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == nameTool);
            PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle && propetyTool.TypeTool != TypeTool.Width && propetyTool.TypeTool != TypeTool.Edge) return;
            cb8.DataSource = null;
            cb8.DataSource = propetyTool.Propety.listP_Center;
            //switch (propetyTool.TypeTool)
        }

        private void cbMeasure_SelectedIndexChanged(object sender, EventArgs e)
        {
            Propety.TypeMeasure = (TypeMeasure)cbMeasure.SelectedItem;
        }

        private void cbDirect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Propety.DirectMeasure = (DirectMeasure)cbDirect.SelectedItem;
        }

        private void cbMethord_SelectedIndexChanged(object sender, EventArgs e)
        {
            Propety.MethordMeasure = (MethordMeasure)cbDirect.SelectedItem;
        }

        private void cb3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoad2)
            {
                IsLoad2= false;
                return;
            }
            if (cb3.SelectedIndex == -1) return;
            String nameTool = cb3.Text;
            indexTool2 = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == nameTool);
            PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle && propetyTool.TypeTool != TypeTool.Width && propetyTool.TypeTool != TypeTool.Edge) return;
            cb4.DataSource = null;
            cb4.DataSource = propetyTool.Propety.listP_Center;

            //switch (propetyTool.TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = propetyTool.Propety as Patterns;
            //        if (Patterns == null) return;
            //        cb4.DataSource = null;
            //        cb4.DataSource = Patterns.listP_Center.ToList();
            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = propetyTool.Propety as Circle;
            //        if (circle == null) return;
            //        cb4.DataSource = null;
            //        cb4.DataSource = circle.listP_Center.ToList();
            //        break;
            //}

        }

        private void cb4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb4.SelectedIndex == -1) return;
          
            Propety.listLine1Point[1] = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2].Propety.listP_Center[cb4.SelectedIndex];
            Propety.listRot[1] = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2].Propety.rectRotates[cb4.SelectedIndex];
            Propety.listPointChoose[1] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2].Name, cb4.SelectedIndex);


            //switch (BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2].TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2].Propety as Patterns;
            //        Propety.listLine1Point[1] = Patterns.listP_Center[cb4.SelectedIndex];
            //        Propety.listRot[1] = Patterns.rectRotates[cb4.SelectedIndex];
            //        Propety.listPointChoose[1] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2].Name, cb4.SelectedIndex);

            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2].Propety as Circle;
            //        Propety.listLine1Point[1] = circle.listP_Center[cb4.SelectedIndex];
            //        Propety.listRot[1] = circle.rectRotates[cb4.SelectedIndex];
            //        Propety.listPointChoose[1] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool2].Name, cb4.SelectedIndex);

            //        break;
            //}

            Global.StatusDraw = StatusDraw.Check;

        }

        private void cb5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoad3)
            {
                IsLoad3 = false;
                return;
            }
            if (cb5.SelectedIndex == -1) return;
            String nameTool = cb5.Text;
            indexTool3 = BeeCore.Common.PropetyTools[Propety.IndexThread].FindIndex(a => a.Name == nameTool);
            PropetyTool propetyTool = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle && propetyTool.TypeTool != TypeTool.Width && propetyTool.TypeTool != TypeTool.Edge) return;
            cb6.DataSource = null;
            cb6.DataSource = propetyTool.Propety.listP_Center;

            //switch (propetyTool.TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = propetyTool.Propety as Patterns;
            //        if (Patterns == null) return;
            //        cb6.DataSource = null;
            //        cb6.DataSource = Patterns.listP_Center.ToList();
            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = propetyTool.Propety as Circle;
            //        if (circle == null) return;
            //        cb6.DataSource = null;
            //        cb6.DataSource = circle.listP_Center.ToList();
            //        break;
            //}

        }

        private void cb7_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = propetyTool.Propety as Patterns;
            //        if (Patterns == null) return;
            //        cb8.DataSource = null;
            //        cb8.DataSource = Patterns.listP_Center.ToList();
            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = propetyTool.Propety as Circle;
            //        if (circle == null) return;
            //        cb8.DataSource = null;
            //        cb8.DataSource = circle.listP_Center.ToList();
            //        break;
            //}
           
        }

        private void cb6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb6.SelectedIndex == -1) return;
            Propety.listLine2Point[0] = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3].Propety.listP_Center[cb6.SelectedIndex];
            Propety.listRot[2] = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3].Propety.rectRotates[cb6.SelectedIndex];
            Propety.listPointChoose[2] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3].Name, cb6.SelectedIndex);

            //switch (BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3].TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3].Propety as Patterns;
            //        Propety.listLine2Point[0] = Patterns.listP_Center[cb6.SelectedIndex];
            //        Propety.listRot[2] = Patterns.rectRotates[cb6.SelectedIndex];
            //        Propety.listPointChoose[2] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3].Name, cb6.SelectedIndex);

            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3].Propety as Circle;
            //        Propety.listLine2Point[0] = circle.listP_Center[cb6.SelectedIndex];
            //        Propety.listRot[2] = circle.rectRotates[cb6.SelectedIndex];
            //        Propety.listPointChoose[2] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool3].Name, cb6.SelectedIndex);

            //        break;
            //}
            Global.StatusDraw = StatusDraw.Check;

        }

        private void cb8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb8.SelectedIndex == -1) return;
            Propety.listLine2Point[1] = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4].Propety.listP_Center[cb8.SelectedIndex];
            Propety.listRot[3] = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4].Propety.rectRotates[cb8.SelectedIndex];
            Propety.listPointChoose[3] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4].Name, cb8.SelectedIndex);


          //  switch (BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4].TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4].Propety as Patterns;
            //        Propety.listLine2Point[1] = Patterns.listP_Center[cb8.SelectedIndex];
            //        Propety.listRot[3] = Patterns.rectRotates[cb8.SelectedIndex];
            //        Propety.listPointChoose[3] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4].Name, cb8.SelectedIndex);

            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4].Propety as Circle;
            //        Propety.listLine2Point[1] = circle.listP_Center[cb8.SelectedIndex];
            //        Propety.listRot[3] = circle.rectRotates[cb8.SelectedIndex];
            //        Propety.listPointChoose[3] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool4].Name, cb8.SelectedIndex);

            //        break;
            //}
            Global.StatusDraw = StatusDraw.Check;
        }

        private void btnAreaBlack_Click(object sender, EventArgs e)
        {

        }

        private void btnAreaWhite_Click(object sender, EventArgs e)
        {

        }

        private void cb2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb2.SelectedIndex == -1) return;
          
            Propety.listLine1Point[0] = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1].Propety.listP_Center[cb2.SelectedIndex];
            Propety.listRot[0] = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1].Propety.rectRotates[cb2.SelectedIndex];
            Propety.listPointChoose[0] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1].Name, cb2.SelectedIndex);

            //switch (BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1].TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1].Propety as Patterns;
            //        Propety.listLine1Point[0] = Patterns.listP_Center[cb2.SelectedIndex];
            //        Propety.listRot[0] = Patterns.rectRotates[cb2.SelectedIndex];
            //        Propety.listPointChoose[0] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1].Name, cb2.SelectedIndex);

            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1].Propety as Circle;
            //        Propety.listLine1Point[0] = circle.listP_Center[cb2.SelectedIndex];
            //        Propety.listRot[0] = circle.rectRotates[cb2.SelectedIndex];
            //        Propety.listPointChoose[0] = new Tuple<String, int>(BeeCore.Common.PropetyTools[Propety.IndexThread][indexTool1].Name, cb2.SelectedIndex);

            //        break;
            //}

            Global.StatusDraw = StatusDraw.Check;

        }
    }
}
