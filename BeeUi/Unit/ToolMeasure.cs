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
using BeeCore;
using BeeCore.Func;
using BeeUi.Common;
using BeeUi.Commons;
using CvPlus;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.ML;
using Point = System.Drawing.Point;

namespace BeeUi.Tool
{
    [Serializable()]
    public partial class ToolMeasure : UserControl
    {
        
        public ToolMeasure( )
        {
            InitializeComponent();

        }
        PointF pIntersection;
        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();
        bool IsDone1, IsDone2, IsDone3, IsDone4;
        Tuple<double, double, double> ListAngle = new Tuple<double, double, double>(0, 0, 0);
      
        public void LoadPara()
        {
            Propety.IniTool();
            worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                timer.Restart();
                Propety.IsOK = true;
                Propety. StatusTool = StatusTool.Processing;
                dynamic outLine1 = G.PropetyTools[G.PropetyTools.FindIndex(a=>a.Name== Propety.listPointChoose[0].Item1 )].Propety ;
                dynamic outLine2 = G.PropetyTools[G.PropetyTools.FindIndex(a => a.Name == Propety.listPointChoose[1].Item1)].Propety ;
                dynamic outLine3 = G.PropetyTools[G.PropetyTools.FindIndex(a => a.Name == Propety.listPointChoose[2].Item1)].Propety ;
                dynamic outLine4 = G.PropetyTools[G.PropetyTools.FindIndex(a => a.Name == Propety.listPointChoose[3].Item1)].Propety ;
                if (outLine1.StatusTool == StatusTool.Done && !IsDone1)
                {
                    IsDone1 = true;
                    if(outLine1.IsOK)
                    {
                        int index = Propety.listPointChoose[0].Item2;
                        if(index< outLine1.listP_Center.Count)
                        Propety.listLine1Point[0] = outLine1.listP_Center[index];
                        else
                            Propety.IsOK = false;
                    }
                    else
                    {
                        Propety.IsOK = false;
                    }
                    
                        
                }

                if (outLine2.StatusTool == StatusTool.Done && !IsDone2)
                {
                    IsDone2 = true;
                    if (outLine2.IsOK)
                    {
                        int index = Propety.listPointChoose[1].Item2;
                        if (index < outLine2.listP_Center.Count)
                            Propety.listLine1Point[1] = outLine2.listP_Center[index];
                        else
                            Propety.IsOK = false;
                    }
                    else
                    {
                        Propety.IsOK = false;
                    }
                }

                if (outLine3.StatusTool == StatusTool.Done && !IsDone3)
                {
                    IsDone3 = true;
                    if (outLine3.IsOK)
                    {
                        int index = Propety.listPointChoose[2].Item2;
                        if (index < outLine3.listP_Center.Count)
                            Propety.listLine2Point[0] = outLine3.listP_Center[index];
                        else
                            Propety.IsOK = false;
                    }
                    else
                    {
                        Propety.IsOK = false;
                    }
                   
                }

                if (outLine4.StatusTool == StatusTool.Done && !IsDone4)
                {
                    try
                    {
                        IsDone4 = true;
                        if (outLine4.IsOK)
                        {
                            int index = Propety.listPointChoose[3].Item2;
                            if (index < outLine4.listP_Center.Count)
                                Propety.listLine2Point[1] = outLine4.listP_Center[index];
                            else
                                Propety.IsOK = false;
                        }
                        else
                        {
                            Propety.IsOK = false;
                        }
                    }
                    catch(Exception ex)
                    {
                        String s = ex.Message;
                        //MessageBox.Show(s);
                    }
                   
                }
            };
            Propety.StatusTool = StatusTool.Initialed;
            worker.RunWorkerCompleted += (sender, e) =>
            {

                try
                {
                    if (IsDone1 && IsDone2 && IsDone3 && IsDone4||!Propety.IsOK)
                    { 
                        IsDone1 = false;
                        IsDone2 = false;
                        IsDone3 = false;
                        IsDone4 = false;
                        if (Propety.IsOK)
                        {
                            Cal.FindIntersection(Propety.listLine1Point[0], Propety.listLine1Point[1], Propety.listLine2Point[0], Propety.listLine2Point[1], out pIntersection);
                            Propety.AngleDetect = Cal.GetAngleBetweenSegments(Propety.listLine1Point[0], Propety.listLine1Point[1], Propety.listLine2Point[0], Propety.listLine2Point[1]);
                            Propety.AngleDetect = Math.Round(Propety.AngleDetect, 1);
                            Propety.ScoreRs = (float)Propety.AngleDetect;
                            if (Propety.ScoreRs <= Propety.Score)
                                Propety.IsOK = true;
                            else Propety.IsOK = false;
                        }
                        Propety.StatusTool = StatusTool.Done;
                    }
                    else
                        Propety.StatusTool = StatusTool.Processing;
                }
                catch (Exception ex)
                {
                    // MessageBox.Show("Kết quả không hợp lệ: " + ex.Message);
                }
                if (Propety.StatusTool == StatusTool.Processing)
                {
                    Task.Delay(5);
                    worker.RunWorkerAsync();
                    return;
                }
                   
                if (!G.IsRun)
                    G.EditTool.View.imgView.Invalidate();
                timer.Stop();

                Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;
               
            };
        }
   
        private void trackScore_ValueChanged(float obj)
        {
            Propety.Score = trackScore.Value;

        }

        public Measure Propety =new Measure();
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
     
       
        public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        {
           

           // if (Propety.rotAreaAdjustment == null && G.IsRun) return gc;
            if(G.IsRun)
            gc.ResetTransform();
            
            var mat = new Matrix();
            //if (!G.IsRun)
            //{
            //    mat.Translate(pScroll.X, pScroll.Y);
            //    mat.Scale(Scale, Scale);
            //}

           
            Color cl = Color.LimeGreen;
            Brush brushText = Brushes.White;
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
         
              
                int i = 0;
               
            foreach (Point p in Propety.listLine1Point)
            {
                mat = new Matrix();
                if (!G.IsRun)
                {
                    mat.Translate(pScroll.X, pScroll.Y);
                    mat.Scale(Scale, Scale);
                }
                gc.Transform = mat;
                mat.Translate(p.X, p.Y);
             
                gc.Transform = mat;
                Draws.Plus(gc, 0, 0, 10, cl, 4);

                gc.ResetTransform();


            }
            foreach (Point p in Propety.listLine2Point)
            {
                mat = new Matrix();
                if (!G.IsRun)
                {
                    mat.Translate(pScroll.X, pScroll.Y);
                    mat.Scale(Scale, Scale);
                }
                gc.Transform = mat;
                mat.Translate(p.X, p.Y);

                gc.Transform = mat;
                Draws.Plus(gc, 0, 0, 10, Color.Blue, 4);

                gc.ResetTransform();


            }
            mat = new Matrix();
            if (!G.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }
            gc.Transform = mat;
            Point p1=new Point( Propety.listLine1Point[0].X, Propety.listLine1Point[0].Y);
            Point p2 = new Point(Propety.listLine1Point[1].X, Propety.listLine1Point[1].Y);
            Draws.DrawInfiniteLine(gc, p1, p2, G.EditTool.View.imgView.ClientRectangle, new Pen(Color.DeepSkyBlue, 2));
            p1 = new Point(Propety.listLine2Point[0].X, Propety.listLine2Point[0].Y);
             p2 = new Point(Propety.listLine2Point[1].X, Propety.listLine2Point[1].Y);
            Draws.DrawInfiniteLine(gc, p1, p2, G.EditTool.View.imgView.ClientRectangle, new Pen(Color.DeepPink, 2));
            //float startAngle =(float) ListAngle.Item1; // độ
            //float endAngle = (float)ListAngle.Item2; // độ
            //float sweepAngle = (float)ListAngle.Item3; // độ
            //if (sweepAngle > 180)
            //    sweepAngle = 360 - sweepAngle;
            int radius = 40;
            Rectangle rect = new Rectangle(
                (int)  pIntersection.X - radius,
               (int)  pIntersection.Y - radius,
                  radius * 2,
                  radius * 2
              );
            
         //   gc.DrawArc(new Pen(cl, 1), rect, startAngle, sweepAngle);
         //   gc.FillPie(new SolidBrush(Color.FromArgb(30, cl)), rect, 0,(float) Propety.AngleDetect );
            gc.DrawString(Propety.AngleDetect + "°", new Font("Arial", 22, FontStyle.Bold), Brushes.OrangeRed, new System.Drawing.Point((int)pIntersection.X + 40, (int)pIntersection.Y + 40));

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

            if (G.TypeCrop != TypeCrop.Area)
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
          
        }

        private void ckBitwiseNot_Click(object sender, EventArgs e)
        {
        }

        private void ckSubPixel_Click(object sender, EventArgs e)
        {
         
        }
     
       
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
           
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
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            G.IsCancel = true;
            
            G.EditTool.RefreshGuiEdit(Step.Step3);
        }

      

        private void btnNormal_Click(object sender, EventArgs e)
        {
           
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
           

        }
     

        private void btnOK_Click(object sender, EventArgs e)
        {
            G.IsCheck = true;
            G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

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
            cb1.DataSource=  G.PropetyTools.Select(p => p.Name).ToList();
            IsLoad2 = true;
            cb3.DataSource = G.PropetyTools.Select(p => p.Name).ToList();
            IsLoad3 = true;
            cb5.DataSource = G.PropetyTools.Select(p => p.Name).ToList();
            IsLoad4 = true;
            cb7.DataSource = G.PropetyTools.Select(p => p.Name).ToList();
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
            indexTool1 = G.PropetyTools.FindIndex(a=>a.Name==nameTool);
            PropetyTool propetyTool = G.PropetyTools[indexTool1];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle) return;

            switch (propetyTool.Propety.TypeTool)
            {
                case TypeTool.Pattern:
                    OutLine outLine = propetyTool.Propety as OutLine;
                    if (outLine == null) return;
                    cb2.DataSource = null;
                    cb2.DataSource = outLine.listP_Center.ToList();
                    break;
                case TypeTool.Circle:
                    Circle circle = propetyTool.Propety as Circle;
                    if (circle == null) return;
                    cb2.DataSource = null;
                    cb2.DataSource = circle.listP_Center.ToList();
                    break;
            }
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
            indexTool2 = G.PropetyTools.FindIndex(a => a.Name == nameTool);
            PropetyTool propetyTool = G.PropetyTools[indexTool2];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern&&propetyTool.TypeTool != TypeTool.Circle) return;
           
         
            switch (propetyTool.Propety.TypeTool)
            {
                case TypeTool.Pattern:
                    OutLine outLine = propetyTool.Propety as OutLine;
                    if (outLine == null) return;
                    cb4.DataSource = null;
                    cb4.DataSource = outLine.listP_Center.ToList();
                    break;
                case TypeTool.Circle:
                    Circle circle = propetyTool.Propety as Circle;
                    if (circle == null) return;
                    cb4.DataSource = null;
                    cb4.DataSource = circle.listP_Center.ToList();
                    break;
            }

        }

        private void cb4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb4.SelectedIndex == -1) return;
            switch (G.PropetyTools[indexTool2].TypeTool)
            {
                case TypeTool.Pattern:
                    OutLine outLine = G.PropetyTools[indexTool2].Propety as OutLine;
                    Propety.listLine1Point[1] = outLine.listP_Center[cb4.SelectedIndex];
                    Propety.listPointChoose[1] = new Tuple<String, int>(outLine.nameTool, cb4.SelectedIndex);

                    break;
                case TypeTool.Circle:
                    Circle circle = G.PropetyTools[indexTool2].Propety as Circle;
                    Propety.listLine1Point[1] = circle.listP_Center[cb4.SelectedIndex];
                    Propety.listPointChoose[1] = new Tuple<String, int>(circle.nameTool, cb4.SelectedIndex);

                    break;
            }
          
            G.IsCheck = true;
            G.EditTool.View.imgView.Invalidate();
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
            indexTool3 = G.PropetyTools.FindIndex(a => a.Name == nameTool);
            PropetyTool propetyTool = G.PropetyTools[indexTool3];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle) return;

            switch (propetyTool.Propety.TypeTool)
            {
                case TypeTool.Pattern:
                    OutLine outLine = propetyTool.Propety as OutLine;
                    if (outLine == null) return;
                    cb6.DataSource = null;
                    cb6.DataSource = outLine.listP_Center.ToList();
                    break;
                case TypeTool.Circle:
                    Circle circle = propetyTool.Propety as Circle;
                    if (circle == null) return;
                    cb6.DataSource = null;
                    cb6.DataSource = circle.listP_Center.ToList();
                    break;
            }

        }

        private void cb7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoad4)
            {
                IsLoad4 = false;
                return;
            }
            if (cb7.SelectedIndex == -1) return;
            String nameTool = cb7.Text;
            indexTool4 = G.PropetyTools.FindIndex(a => a.Name == nameTool);
            PropetyTool propetyTool = G.PropetyTools[indexTool4];
            if (propetyTool == null) return;
            if (propetyTool.TypeTool != TypeTool.Pattern && propetyTool.TypeTool != TypeTool.Circle) return;

            switch (propetyTool.Propety.TypeTool)
            {
                case TypeTool.Pattern:
                    OutLine outLine = propetyTool.Propety as OutLine;
                    if (outLine == null) return;
                    cb8.DataSource = null;
                    cb8.DataSource = outLine.listP_Center.ToList();
                    break;
                case TypeTool.Circle:
                    Circle circle = propetyTool.Propety as Circle;
                    if (circle == null) return;
                    cb8.DataSource = null;
                    cb8.DataSource = circle.listP_Center.ToList();
                    break;
            }
           
        }

        private void cb6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb6.SelectedIndex == -1) return;
         
            switch (G.PropetyTools[indexTool3].TypeTool)
            {
                case TypeTool.Pattern:
                    OutLine outLine = G.PropetyTools[indexTool3].Propety as OutLine;
                    Propety.listLine2Point[0] = outLine.listP_Center[cb6.SelectedIndex];
                    Propety.listPointChoose[2] = new Tuple<String, int>(outLine.nameTool, cb6.SelectedIndex);

                    break;
                case TypeTool.Circle:
                    Circle circle = G.PropetyTools[indexTool3].Propety as Circle;
                    Propety.listLine2Point[0] = circle.listP_Center[cb6.SelectedIndex];
                    Propety.listPointChoose[2] = new Tuple<String, int>(circle.nameTool, cb6.SelectedIndex);

                    break;
            }
            G.IsCheck = true;
            G.EditTool.View.imgView.Invalidate();

        }

        private void cb8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb8.SelectedIndex == -1) return;
            switch (G.PropetyTools[indexTool4].TypeTool)
            {
                case TypeTool.Pattern:
                    OutLine outLine = G.PropetyTools[indexTool4].Propety as OutLine;
                    Propety.listLine2Point[1] = outLine.listP_Center[cb8.SelectedIndex];
                    Propety.listPointChoose[3] = new Tuple<String, int>(outLine.nameTool, cb8.SelectedIndex);

                    break;
                case TypeTool.Circle:
                    Circle circle = G.PropetyTools[indexTool4].Propety as Circle;
                    Propety.listLine2Point[1] = circle.listP_Center[cb8.SelectedIndex];
                    Propety.listPointChoose[3] = new Tuple<String, int>(circle.nameTool, cb8.SelectedIndex);

                    break;
            }
 
           G.IsCheck = true;
            G.EditTool.View.imgView.Invalidate();
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
            switch (G.PropetyTools[indexTool1].TypeTool)
            {
                case TypeTool.Pattern:
                    OutLine outLine = G.PropetyTools[indexTool1].Propety as OutLine;
                    Propety.listLine1Point[0] = outLine.listP_Center[cb2.SelectedIndex];
                    Propety.listPointChoose[0] = new Tuple<String, int>(outLine.nameTool, cb2.SelectedIndex);

                    break;
                case TypeTool.Circle:
                    Circle circle = G.PropetyTools[indexTool1].Propety as Circle;
                    Propety.listLine1Point[0] = circle.listP_Center[cb2.SelectedIndex];
                    Propety.listPointChoose[0] = new Tuple<String, int>(circle.nameTool, cb2.SelectedIndex);

                    break;
            }
           
           G.IsCheck = true;
            G.EditTool.View.imgView.Invalidate();
        }
    }
}
