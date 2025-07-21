using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Func;
using BeeGlobal;
using BeeInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolWidth : UserControl
    {
        
        public ToolWidth( )
        {
            InitializeComponent();
        
        }
        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();
        int ThresholdValue = 100;
        double MmPerPixel = 0.05;
        Line2D line1, line2;
        double gapPx=0;
        Mat annotated = new Mat();
        public void LoadPara()
        {

            //worker = new BackgroundWorker();
            //worker.DoWork += (sender, e) =>
            //{
            //    //Propety.IsOK = true;

            //    //timer.Restart();
            //    //if (!Global.IsRun)
            //    //    Propety.rotAreaAdjustment = Propety.rotArea;
            //    //Propety.StatusTool = StatusTool.Processing;
            //    //using (Mat raw = BeeCore.Common.listCamera[Propety. IndexThread].matRaw.Clone())
            //    //{
            //    //    if (raw.Empty()) return;

            //    //    Mat matCrop = Common.CropRotatedRect(raw, Propety.rotAreaAdjustment, null);
            //    //    Mat matProcess = new Mat();
            //    //    if (matCrop.Type() == MatType.CV_8UC3)
            //    //        Cv2.CvtColor(matCrop, matProcess, ColorConversionCodes.BGR2GRAY);
            //    //    else
            //    //        matProcess = matCrop;




            //    //    Mat bin = new Mat();
            //    //    Cv2.Threshold(matProcess, bin, ThresholdValue, 255, ThresholdTypes.BinaryInv);

            //    //    // 2. Lấy 2 contour lớn nhất
            //    //    Cv2.FindContours(bin, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxNone);
            //    //    if (contours.Length < 2) throw new InvalidOperationException("Cần ít nhất hai contour để đo shaft gap.");
            //    //    var top2 = contours.OrderByDescending(c => Cv2.ContourArea(c)).Take(2).ToArray();
            //    //    var c1 = top2[0];
            //    //    var c2 = top2[1];

            //    //    // 3. Lấy points
            //    //    List<Point2f> pts1 = c1.Select(p => new Point2f(p.X, p.Y)).ToList();
            //    //    List<Point2f> pts2 = c2.Select(p => new Point2f(p.X, p.Y)).ToList();

            //    //    // 4. Fit center lines (Line2D) nếu cần
            //    //     line1 = Cv2.FitLine(pts1.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
            //    //     line2 = Cv2.FitLine(pts2.ToArray(), DistanceTypes.L2, 0, 0.01, 0.01);
            //    //    // Khoảng cách giữa hai đường tâm
            //    //   double gapPx = Cal.DistanceBetweenLines(line1, line2);
            //    //     annotated = matProcess.Clone();
            //    //    Cv2.CvtColor(annotated, annotated, ColorConversionCodes.GRAY2BGR);
            //    //    // Vẽ 2 đường center
            //    //    Draws. DrawInfiniteLine(annotated, line1, new Scalar(0, 255, 0), 2);
            //    //    Draws.DrawInfiniteLine(annotated, line2, new Scalar(0, 0, 255), 2);
            //    //    // Tính đoạn thẳng đo khoảng cách
            //    //    // Lấy một điểm trên line1

            //    //    // Tính tham số dòng ax+by+c=0 cho line2
            //    //    Cal.GetLineParams(line2, out double a2, out double b2, out double c4);
            //    //    // Tính foot Q của p0 lên line2
            //    //    double denom = a2 * a2 + b2 * b2;
            //    //    double k = (a2 * line1.X1 + b2 * line1.Y1 + c4) / denom;
            //    //    var q = new Point(
            //    //        (int)Math.Round(line1.X1 - a2 * k),
            //    //        (int)Math.Round(line1.Y1 - b2 * k)
            //    //    );
            //    //    // Vẽ đoạn đo
            //    //    Cv2.Line(annotated, new Point(line1.X1, line1.Y1), q, new Scalar(255, 0, 0), 2);
            //    //    // Ghi giá trị lên ảnh
            //    //    var distMm = gapPx * MmPerPixel;
            //    //    string text = $"{distMm:F2} mm";
            //    //    Cv2.PutText(annotated, text, new Point((line1.X1 + q.X) / 2, (line1.Y1 + q.Y) / 2 - 10),
            //    //        HersheyFonts.HersheySimplex, 0.7, new Scalar(255, 0, 0), 2);

            //    //    Cv2.GaussianBlur(matProcess, matProcess, new Size(5, 5), 0);

            //    //    // 2. Threshold để tách foreground (máy kim) khỏi nền
            //    //    bin = new Mat();
            //    //    Cv2.Threshold(matProcess, bin, 150, 255, ThresholdTypes.BinaryInv);


            //    //    HierarchyIndex[] hierarchy;
            //    //    Cv2.FindContours(bin, out contours, out hierarchy,
            //    //                     RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            //    //    if (contours.Length < 2)
            //    //        throw new InvalidOperationException("Cần ít nhất hai contour để đo tip.");

            //    //    // 4. Chọn 2 contour có diện tích lớn nhất
            //    //    top2 = contours.OrderByDescending(c => Cv2.ContourArea(c)).Take(2).ToArray();

            //    //    // 5. Với mỗi contour, tìm điểm có Y nhỏ nhất (đỉnh hướng lên trên)
            //    //    Point tip1 = top2[0].OrderBy(p => p.Y).First();
            //    //    Point tip2 = top2[1].OrderBy(p => p.Y).First();
            //    //    // 6. Tính khoảng cách Euclid
            //    //    double dx1 = tip1.X - tip2.X;
            //    //    double dy1 = tip1.Y - tip2.Y;
            //    //    double distPx = Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            //    //    double distMm2 = distPx * MmPerPixel;

            //    //    // 7. Vẽ lên ảnh

            //    //    Cv2.Circle(annotated, tip1, 6, new Scalar(0, 255, 0), -1);
            //    //    Cv2.Circle(annotated, tip2, 6, new Scalar(0, 0, 255), -1);
            //    //    Line2D lineCen1 = Cal.FindPerpendicularLine(line1, tip1);
            //    //    Line2D lineCen2 = Cal.FindPerpendicularLine(line2, tip2);
            //    //    double gapPx2 = Cal.DistanceBetweenLines(lineCen1, lineCen2);
            //    //    var distMm3 = gapPx2 * MmPerPixel;
            //    //    string text2 = $"{distMm3:F2} mm";
            //    //    if (distMm3 > 0.5)
            //    //        Propety.IsOK = false;
            //    //    if (Propety.IsOK)
            //    //    {
            //    //        Draws.DrawPerpendicularLine(annotated, lineCen1, new Scalar(0, 255, 0), 2);
            //    //        Draws.DrawPerpendicularLine(annotated, lineCen2, new Scalar(0, 255, 0), 2);
            //    //        Cv2.PutText(annotated, text2, tip1,
            //    //         HersheyFonts.HersheySimplex, 0.7, new Scalar(255, 0, 0), 2);
            //    //    }
            //    //    else

            //    //    {
            //    //        Draws.DrawPerpendicularLine(annotated, lineCen1, new Scalar(255, 0, 0), 2);
            //    //        Draws.DrawPerpendicularLine(annotated, lineCen2, new Scalar(255, 0, 0), 2);
            //    //        Cv2.PutText(annotated, text2, tip1,
            //    //         HersheyFonts.HersheySimplex, 0.7, new Scalar(255, 0, 0), 2);
            //    //    }    

                     
                 
            //    //}
            //    Propety.DoWork(Propety.rotAreaAdjustment);
            //};

            //worker.RunWorkerCompleted += (sender, e) =>
            //{
              
            //    Propety.Complete();
            //     if (!Global.IsRun)
            //        Global.StatusDraw = StatusDraw.Check;
            //    timer.Stop();
            //    Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;
            //}; 
            if (!workLoadModel.IsBusy)
                workLoadModel.RunWorkerAsync();

            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].StatusToolChanged += ToolWidth_StatusToolChanged;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            numScale.Value = (decimal)Propety.Scale;
            trackMaxLine.Value = Propety.MaximumLine;
            trackMinInlier.Value = Propety.MinInliers;
           
            numMinRadius.Value = Propety.MinLen;
            numMaxRadius.Value = Propety.MaxLen;
            switch (Propety.MethordEdge)
            {
                case MethordEdge.StrongEdges:
                    btnStrongEdge.IsCLick = true;
                    break;
                case MethordEdge.CloseEdges:
                    btnCloseEdge.IsCLick = true;
                    break;
            }
            switch (Propety.LineOrientation)
            {
                case LineOrientation.Vertical:
                    btnVer.IsCLick = true;
                    break;
                case LineOrientation.Horizontal:
                    btnHori.IsCLick = true;
                    break;
            }
            switch (Propety.SegmentStatType)
            {
                case SegmentStatType.Longest:
                    btnLong.IsCLick = true;
                    break;
                case SegmentStatType.Shortest:
                    btnShort.IsCLick = true;
                    break;
                case SegmentStatType.Average:
                    btnAverage.IsCLick = true;
                    break;
            }
            switch (Propety.GapExtremum)
            {
                case GapExtremum.Outermost:
                    btnOutter.IsCLick = true;
                    break;
                case GapExtremum.Middle:
                    btnMid.IsCLick = true;
                    break;
                case GapExtremum.Nearest:
                    btnNear.IsCLick = true;
                    break;
                case GapExtremum.Farthest:
                    btnFar.IsCLick = true;
                    break;
            }
        }

        private void ToolWidth_StatusToolChanged(StatusTool obj)
        {
            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool == StatusTool.Done)
                if (Propety.IsCalibs)
                {
                    btnCalib.IsCLick = false;
                    Propety.IsCalibs = false;
                    btnCalib.Enabled = true;
                    trackMinInlier.Value = Propety.MinInliers;
                    numMaxRadius.Value = Propety.MaxLen;
                    numMinRadius.Value = Propety.MinLen;
                }
           
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score=trackScore.Value;
            numScore.Value =(int)Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
        }
        public bool IsClear = false;
        public Width Propety=new Width();
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();

    
       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
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

    
        
      
    
        Bitmap bmResult ;
        
        public int indexTool = 0;
        private void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            btnTest.IsCLick = false;
         //   G.EditTool.View.imgView.Invalidate();

        //    G.ResultBar.lbCycleTrigger.Text = "[" + Propety.cycleTime + "ms]";
        }

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
            //this.tabP1.BackColor = CustomGui.BackColor(TypeCtr.BG, G.Config.colorGui);
           // this.trackNumObject.BackColor = CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
           // layScore.BackColor = CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
      

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

    

        private void btnTest_Click(object sender, EventArgs e)
        {
           
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected]. worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
        private void btnCropHalt_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Check;
            
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None,false);

            
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.Check;
           
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
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
        }

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

        private void trackAngle_ValueChanged(float obj)
        {
           
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void numAngle_ValueChanged(object sender, EventArgs e)
        {
          
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

      

        private void trackNumObject_Load(object sender, EventArgs e)
        {

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

   
     
      
  
        

        private void btnModeEdge_Click(object sender, EventArgs e)
        {
          
        }

        private void btnModeCany_Click(object sender, EventArgs e)
        {
         
        }

        private void btnModePattern_Click(object sender, EventArgs e)
        {
        }

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
            OutLine.LoadEdge();
          
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global. TypeCrop)
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
          //  G.EditTool.View.imgView.Invalidate();
        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnShort_Click(object sender, EventArgs e)
        {
            Propety.SegmentStatType = SegmentStatType.Shortest;
        }

        private void btnAverage_Click(object sender, EventArgs e)
        {
            Propety.SegmentStatType = SegmentStatType.Average;
        }

        private void btnLong_Click(object sender, EventArgs e)
        {
            Propety.SegmentStatType = SegmentStatType.Longest;
        }

        private void btnMid_Click(object sender, EventArgs e)
        {
            Propety.GapExtremum = GapExtremum.Middle;
        }

        private void btnOutter_Click(object sender, EventArgs e)
        {
            Propety.GapExtremum = GapExtremum.Outermost;
        }

        private void btnNear_Click(object sender, EventArgs e)
        {
            Propety.GapExtremum = GapExtremum.Nearest;
        }

        private void btnFar_Click(object sender, EventArgs e)
        {
            Propety.GapExtremum = GapExtremum.Nearest;
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Vertical;
        }

       

        private void btnHori_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Horizontal;
        }

        private void trackMaxLine_ValueChanged(float obj)
        {
            Propety.MaximumLine = (int)trackMaxLine.Value;
        }

        private void trackMinInlier_ValueChanged(float obj)
        {
            Propety.MinInliers = (int)trackMinInlier.Value;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            Propety.IsCalibs = true;
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                Propety.IsCalibs = false;
            
        }

        private void trackScore_Load(object sender, EventArgs e)
        {

        }

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
         
        }
    }
}
