using BeeCore;
using BeeCore.Func;
using BeeCore.Funtion.Engines;
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
        
        #region OwnerTool cache (Phase 2 refactor)
        private PropetyTool _ownerTool;
        private PropetyTool OwnerTool
        {
            get
            {
                if (_ownerTool == null)
                    _ownerTool = Common.TryGetTool(Global.IndexProgChoose, Propety.Index);
                return _ownerTool;
            }
        }
        private void InvalidateOwnerToolCache() => _ownerTool = null;
        #endregion
        private PropetyTool GetTool(int indexTool)
            => Common.TryGetTool(Propety.IndexThread, indexTool);

        private int FindToolIndexByName(string nameTool)
        {
            for (int i = 0; ; i++)
            {
                var tool = GetTool(i);
                if (tool == null) return -1;
                if (tool.Name == nameTool) return i;
            }
        }

        private List<string> GetToolNames()
        {
            var names = new List<string>();
            for (int i = 0; ; i++)
            {
                var tool = GetTool(i);
                if (tool == null) break;
                names.Add(tool.Name);
            }
            return names;
        }

        private ComboBox cbLine1Mode;
        private ComboBox cbLine2Mode;
        private ComboBox cbMultiSrc;
        private TableLayoutPanel line1Header;
        private TableLayoutPanel line2Header;
        private bool _loadingMeasureUi;

        public ToolMeasure( )
        {
            InitializeComponent();
            _loadingMeasureUi = true;
            InstallLineModeControls();
            InstallMultiSrcCombo();
            cbMeasure.DataSource = Enum.GetValues(typeof(TypeMeasure));
            cbMethod.DataSource = Enum.GetValues(typeof(MeasureMethod));
            cbDirect.DataSource = Enum.GetValues(typeof(DirectMeasure));
            cbMethord.DataSource = Enum.GetValues(typeof(MethordMeasure));
            _loadingMeasureUi = false;
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
            Propety.EnsureSelectionLists();
            AdjValueSample.Step = 0.01f;
           // numScale.Value=(decimal)Propety.Scale ;
            MeasureEngineRunner.MarkOwnerWaiting(OwnerTool);
            var state = MeasureEngineRunner.ReadFromOwner(OwnerTool, Propety);
            trackScore.Min = state.ScoreMin;
            trackScore.Max = state.ScoreMax;
            trackScore.Step = state.ScoreStep;
            trackScore.Value = state.Score;
            _loadingMeasureUi = true;
            cbLine1Mode.SelectedItem = state.Line1InputMode;
            cbLine2Mode.SelectedItem = state.Line2InputMode;
            ApplyLineModeUi(1);
            ApplyLineModeUi(2);
            if (Propety.listPointChoose.Count>=4)
            {
                RestoreLineSelection(1);
                RestoreLineSelection(2);
            }
            AdjScale.Value = Propety.Scale;
            cbMeasure.SelectedIndex = cbMeasure.FindStringExact(state.TypeMeasure.ToString());
            ApplyMethodComboFilter(state.TypeMeasure);
            cbMethod.SelectedIndex = cbMethod.FindStringExact(Propety.MeasureMethod.ToString());
            cbDirect.SelectedIndex = cbDirect.FindStringExact(state.DirectMeasure.ToString());
            cbMethord.SelectedIndex = cbMethord.FindStringExact(state.MethordMeasure.ToString());
            ApplyMeasureModeUi(state.TypeMeasure, Propety.MeasureMethod);
            // Load MultiPointSrcToolName
            if (cbMultiSrc != null)
            {
                var names = GetToolNames();
                cbMultiSrc.DataSource = null;
                cbMultiSrc.DataSource = names;
                if (!string.IsNullOrEmpty(Propety.MultiPointSrcToolName))
                    cbMultiSrc.SelectedItem = Propety.MultiPointSrcToolName;
            }
            _loadingMeasureUi = false;
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
             if (OwnerTool != null)
             {
                 OwnerTool.StatusToolChanged -= ToolYolo_StatusToolChanged;
                 OwnerTool.StatusToolChanged += ToolYolo_StatusToolChanged;
             }

        }
        private void ToolYolo_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (Global.IsRun) return;
            if (OwnerTool == null) return;
            if (OwnerTool.StatusTool == StatusTool.Done)
            {
               // Propety.rectTrain = Propety.rectRotates;//note
                btnTest.Enabled = true;
            }
        }
        private void trackScore_ValueChanged(float obj)
        {
            MeasureEngineRunner.ApplyScoreToOwner(OwnerTool, trackScore.Value);

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
            //    //if (GetTool(Propety.Index).UsedTool == UsedTool.Invertse &&
            //    //    G.Config.ConditionOK == ConditionOK.Logic)
            //    //    cl = Color.LimeGreen;


            //}
            //else
            //{
            //    cl = Color.LimeGreen;
            //    //if (GetTool(Propety.Index).UsedTool == UsedTool.Invertse &&
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
            //    Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw, indexTool, Propety.rotAreaAdjustment);

            //}
            //else
            //    Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw, indexTool, Propety.rotArea);
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
            Common.TryGetTool(Global.IndexToolSelected).RunToolAsync();
            //btnTest.Enabled = false;
            //if (!MeasureEngineRunner.TryStartSelectedTool())
            //{
            //    btnTest.Enabled = true;
            //    btnTest.IsCLick = false;
            //}
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
            _loadingMeasureUi = true;
            IsLoad1 = true;
            IsLoad2 = true;
            IsLoad3 = true;
            IsLoad4 = true;
            RefreshLineSourceLists();
            _loadingMeasureUi = false;
            IsLoad1 = false;
            IsLoad2 = false;
            IsLoad3 = false;
            IsLoad4 = false;
        }

        int indexTool1 = -1, indexTool2= -1, indexTool3 = -1, indexTool4 = -1;
        private void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingMeasureUi || IsLoad1) return;
            HandleSourceComboChanged(cb1, cb2, ref indexTool1, Propety.Line1InputMode, 0, 1);
            //dynamic outLine1 = GetTool(FindToolIndexByName(Properties.CH listPointChoose[0).Item1)].Propety;



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
            HandleSourceComboChanged(cb1, cb2, ref indexTool1, Propety.Line1InputMode, 0, 1);
        }

        private void cb2_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void cb3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HandleSourceComboChanged(cb3, cb4, ref indexTool2, MeasureLineInputMode.Point, 1, 1);
        }

        private void cb5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HandleSourceComboChanged(cb5, cb6, ref indexTool3, Propety.Line2InputMode, 2, 2);
        }

        private void cb7_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HandleSourceComboChanged(cb7, cb8, ref indexTool4, MeasureLineInputMode.Point, 3, 2);
            //switch (propetyTool.TypeTool)
        }

        private void cbMeasure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMeasure.SelectedItem == null) return;
            if (_loadingMeasureUi) return;
            Propety.TypeMeasure = (TypeMeasure)cbMeasure.SelectedItem;
            ApplyMethodComboFilter(Propety.TypeMeasure);
            ApplyMeasureModeUi(Propety.TypeMeasure, Propety.MeasureMethod);
        }

        private void cbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMethod.SelectedItem == null) return;
            if (_loadingMeasureUi) return;
            Propety.MeasureMethod = (MeasureMethod)cbMethod.SelectedItem;
            ApplyMeasureModeUi(Propety.TypeMeasure, Propety.MeasureMethod);
        }

        private void ApplyMethodComboFilter(TypeMeasure type)
        {
            if (type == TypeMeasure.Angle)
            {
                cbMethod.DataSource = new MeasureMethod[] { MeasureMethod.LineToLine };
                cbMethod.SelectedIndex = 0;
                Propety.MeasureMethod = MeasureMethod.LineToLine;
                cbMethod.Enabled = false;
            }
            else
            {
                cbMethod.DataSource = Enum.GetValues(typeof(MeasureMethod));
                cbMethod.Enabled = true;
                int idx = cbMethod.FindStringExact(Propety.MeasureMethod.ToString());
                if (idx >= 0) cbMethod.SelectedIndex = idx;
            }
        }

        private void ApplyMeasureModeUi(TypeMeasure type, MeasureMethod method)
        {
            if (cbLine1Mode == null || cbLine2Mode == null || cbMultiSrc == null)
                return;

            bool isAngle = type == TypeMeasure.Angle;
            bool needLine2Full = isAngle || method == MeasureMethod.LineToLine;
            bool needSinglePoint = !isAngle && method == MeasureMethod.PointToLine;
            bool needMultiSrc = !isAngle && method == MeasureMethod.MultiPointToLine;
            bool noLine2 = !isAngle && (method == MeasureMethod.PointToPoint || method == MeasureMethod.MultiPointToLine);

            if (needMultiSrc)
            {
                Propety.Line1InputMode = MeasureLineInputMode.Line;
                if (cbLine1Mode != null)
                    cbLine1Mode.SelectedItem = MeasureLineInputMode.Line;
            }

            cbDirect.Enabled = !isAngle && method == MeasureMethod.PointToPoint;
            cbMethord.Enabled = needMultiSrc;

            ApplyLineModeUi(1);
            ApplyLineModeUi(2);

            SetLineHeaderVisible(1, true);
            SetLineHeaderVisible(2, needLine2Full || needSinglePoint);
            cbLine1Mode.Visible = !needMultiSrc;
            label1.Text = needMultiSrc ? "Reference Line" : "Line 1";
            label2.Visible = needLine2Full || needSinglePoint;
            tableLayoutPanel4.Visible = needLine2Full || needSinglePoint;
            tableLayoutPanel5.Visible = needLine2Full;

            if (label2 != null)
                label2.Text = needSinglePoint ? "Source Point" : "Line 2";

            if (noLine2)
            {
                SetLineHeaderVisible(2, false);
                label2.Visible = false;
                tableLayoutPanel4.Visible = false;
                tableLayoutPanel5.Visible = false;
            }

            if (cbMultiSrc != null && cbMultiSrc.Parent != null)
                cbMultiSrc.Parent.Visible = needMultiSrc;
        }

        private void InstallMultiSrcCombo()
        {
            cbMultiSrc = new ComboBox();
            cbMultiSrc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMultiSrc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            cbMultiSrc.Dock = DockStyle.Fill;

            var wrapper = new System.Windows.Forms.TableLayoutPanel();
            wrapper.ColumnCount = 2;
            wrapper.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            wrapper.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            wrapper.RowCount = 1;
            wrapper.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            wrapper.Dock = DockStyle.Fill;
            wrapper.Visible = false;

            var lbl = new Label();
            lbl.Text = "Multi-Point Source";
            lbl.Dock = DockStyle.Fill;
            lbl.Font = cbMultiSrc.Font;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            wrapper.Controls.Add(lbl, 0, 0);
            wrapper.Controls.Add(cbMultiSrc, 1, 0);

            // Insert after tableLayoutPanel5 in tableLayoutPanel1
            int row = tableLayoutPanel1.GetRow(tableLayoutPanel5);
            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Controls.Add(wrapper, 0, row + 1);

            cbMultiSrc.SelectedIndexChanged -= cbMultiSrc_SelectedIndexChanged;
            cbMultiSrc.SelectedIndexChanged += cbMultiSrc_SelectedIndexChanged;
        }

        private void cbMultiSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingMeasureUi) return;
            if (cbMultiSrc.SelectedItem is string name)
                Propety.MultiPointSrcToolName = name;
        }

        private void cbDirect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingMeasureUi) return;
            if(cbDirect.SelectedItem != null) 
            Propety.DirectMeasure = (DirectMeasure)cbDirect.SelectedItem;
        }

        private void cbMethord_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingMeasureUi) return;
            if (cbMethord.SelectedItem != null)
                Propety.MethordMeasure = (MethordMeasure)cbMethord.SelectedItem;
        }

        private void cb3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingMeasureUi || IsLoad2) return;
            HandleSourceComboChanged(cb3, cb4, ref indexTool2, MeasureLineInputMode.Point, 1, 1);

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
            SelectPointFromCombo(cb4, indexTool2, 1, Propety.listLine1Point, 1);


            //switch (GetTool(indexTool2).TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = GetTool(indexTool2).Propety as Patterns;
            //        Propety.listLine1Point[1] = Patterns.listP_Center[cb4.SelectedIndex];
            //        Propety.listRot[1] = Patterns.rectRotates[cb4.SelectedIndex];
            //        Propety.listPointChoose[1] = new Tuple<String, int>(GetTool(indexTool2).Name, cb4.SelectedIndex);

            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = GetTool(indexTool2).Propety as Circle;
            //        Propety.listLine1Point[1] = circle.listP_Center[cb4.SelectedIndex];
            //        Propety.listRot[1] = circle.rectRotates[cb4.SelectedIndex];
            //        Propety.listPointChoose[1] = new Tuple<String, int>(GetTool(indexTool2).Name, cb4.SelectedIndex);

            //        break;
            //}

            Global.StatusDraw = StatusDraw.Check;

        }

        private void cb5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingMeasureUi || IsLoad3) return;
            HandleSourceComboChanged(cb5, cb6, ref indexTool3, Propety.Line2InputMode, 2, 2);

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
            SelectPointFromCombo(cb6, indexTool3, 2, Propety.listLine2Point, 0);

            //switch (GetTool(indexTool3).TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = GetTool(indexTool3).Propety as Patterns;
            //        Propety.listLine2Point[0] = Patterns.listP_Center[cb6.SelectedIndex];
            //        Propety.listRot[2] = Patterns.rectRotates[cb6.SelectedIndex];
            //        Propety.listPointChoose[2] = new Tuple<String, int>(GetTool(indexTool3).Name, cb6.SelectedIndex);

            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = GetTool(indexTool3).Propety as Circle;
            //        Propety.listLine2Point[0] = circle.listP_Center[cb6.SelectedIndex];
            //        Propety.listRot[2] = circle.rectRotates[cb6.SelectedIndex];
            //        Propety.listPointChoose[2] = new Tuple<String, int>(GetTool(indexTool3).Name, cb6.SelectedIndex);

            //        break;
            //}
            Global.StatusDraw = StatusDraw.Check;

        }

        private void cb8_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectPointFromCombo(cb8, indexTool4, 3, Propety.listLine2Point, 1);


          //  switch (GetTool(indexTool4).TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = GetTool(indexTool4).Propety as Patterns;
            //        Propety.listLine2Point[1] = Patterns.listP_Center[cb8.SelectedIndex];
            //        Propety.listRot[3] = Patterns.rectRotates[cb8.SelectedIndex];
            //        Propety.listPointChoose[3] = new Tuple<String, int>(GetTool(indexTool4).Name, cb8.SelectedIndex);

            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = GetTool(indexTool4).Propety as Circle;
            //        Propety.listLine2Point[1] = circle.listP_Center[cb8.SelectedIndex];
            //        Propety.listRot[3] = circle.rectRotates[cb8.SelectedIndex];
            //        Propety.listPointChoose[3] = new Tuple<String, int>(GetTool(indexTool4).Name, cb8.SelectedIndex);

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
            SelectPointFromCombo(cb2, indexTool1, 0, Propety.listLine1Point, 0);

            //switch (GetTool(indexTool1).TypeTool)
            //{
            //    case TypeTool.Pattern:
            //        Patterns Patterns = GetTool(indexTool1).Propety as Patterns;
            //        Propety.listLine1Point[0] = Patterns.listP_Center[cb2.SelectedIndex];
            //        Propety.listRot[0] = Patterns.rectRotates[cb2.SelectedIndex];
            //        Propety.listPointChoose[0] = new Tuple<String, int>(GetTool(indexTool1).Name, cb2.SelectedIndex);

            //        break;
            //    case TypeTool.Circle:
            //        Circle circle = GetTool(indexTool1).Propety as Circle;
            //        Propety.listLine1Point[0] = circle.listP_Center[cb2.SelectedIndex];
            //        Propety.listRot[0] = circle.rectRotates[cb2.SelectedIndex];
            //        Propety.listPointChoose[0] = new Tuple<String, int>(GetTool(indexTool1).Name, cb2.SelectedIndex);

            //        break;
            //}

            Global.StatusDraw = StatusDraw.Check;

        }

        private void InstallLineModeControls()
        {
            cbLine1Mode = CreateLineModeCombo();
            cbLine2Mode = CreateLineModeCombo();
            cbLine1Mode.SelectedIndexChanged += cbLine1Mode_SelectedIndexChanged;
            cbLine2Mode.SelectedIndexChanged += cbLine2Mode_SelectedIndexChanged;
            line1Header = ReplaceHeaderWithMode(label1, cbLine1Mode);
            line2Header = ReplaceHeaderWithMode(label2, cbLine2Mode);
        }

        private ComboBox CreateLineModeCombo()
        {
            ComboBox combo = new ComboBox();
            combo.Dock = DockStyle.Right;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            combo.Width = 120;
            combo.DataSource = Enum.GetValues(typeof(MeasureLineInputMode));
            return combo;
        }

        private TableLayoutPanel ReplaceHeaderWithMode(Label label, ComboBox modeCombo)
        {
            TableLayoutPanel parent = label.Parent as TableLayoutPanel;
            if (parent == null) return null;

            int row = parent.GetRow(label);
            parent.Controls.Remove(label);
            TableLayoutPanel header = new TableLayoutPanel();
            header.ColumnCount = 2;
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            header.RowCount = 1;
            header.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            header.BackColor = label.BackColor;
            header.Dock = DockStyle.Fill;
            header.Margin = label.Margin;

            label.Margin = new Padding(0);
            label.Dock = DockStyle.Fill;
            header.Controls.Add(label, 0, 0);
            header.Controls.Add(modeCombo, 1, 0);
            parent.Controls.Add(header, 0, row);
            return header;
        }

        private void SetLineHeaderVisible(int lineNumber, bool visible)
        {
            TableLayoutPanel header = lineNumber == 1 ? line1Header : line2Header;
            if (header != null)
                header.Visible = visible;
        }

        private void cbLine1Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLine1Mode.SelectedItem == null) return;
            if (_loadingMeasureUi) return;
            Propety.Line1InputMode = (MeasureLineInputMode)cbLine1Mode.SelectedItem;
            ApplyMeasureModeUi(Propety.TypeMeasure, Propety.MeasureMethod);
            if (!_loadingMeasureUi)
                Global.StatusDraw = StatusDraw.Check;
        }

        private void AdjScale_ValueChanged(float obj)
        {
            Propety.Scale= obj;

        }

        private void btn7_Click(object sender, EventArgs e)
        {
            AdjScale.Visible=!btn7.IsCLick;
        }

        private void rjButton4_Click(object sender, EventArgs e)
        {
            AdjValueSample.Visible = !rjButton4.IsCLick;
        }

        private void AdjValueSample_ValueChanged(float obj)
        {
            Propety.ValueSample = obj;
        }

        private void cbLine2Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLine2Mode.SelectedItem == null) return;
            if (_loadingMeasureUi) return;
            Propety.Line2InputMode = (MeasureLineInputMode)cbLine2Mode.SelectedItem;
            ApplyMeasureModeUi(Propety.TypeMeasure, Propety.MeasureMethod);
            if (!_loadingMeasureUi)
                Global.StatusDraw = StatusDraw.Check;
        }
        private void ApplyLineModeUi(int lineNumber)
        {
            bool isLineMode = lineNumber == 1
                ? Propety.Line1InputMode == MeasureLineInputMode.Line
                : Propety.Line2InputMode == MeasureLineInputMode.Line;

            if (lineNumber == 1)
            {
                rjButton11.Text = isLineMode ? "Line" : "Point 1";
                cb2.Enabled = !isLineMode;
                cb2.Visible = !isLineMode;
                tableLayoutPanel3.Visible = !isLineMode;
                RefreshSourceCombo(cb1, Propety.Line1InputMode);
                if (!isLineMode)
                    RefreshSourceCombo(cb3, MeasureLineInputMode.Point);
            }
            else
            {
                rjButton13.Text = isLineMode ? "Line" : "Point 1";
                cb6.Enabled = !isLineMode;
                cb6.Visible = !isLineMode;
                tableLayoutPanel5.Visible = !isLineMode;
                RefreshSourceCombo(cb5, Propety.Line2InputMode);
                if (!isLineMode)
                    RefreshSourceCombo(cb7, MeasureLineInputMode.Point);
            }
        }

        private void RefreshLineSourceLists()
        {
            ApplyLineModeUi(1);
            ApplyLineModeUi(2);
        }

        private void RefreshSourceCombo(ComboBox combo, MeasureLineInputMode mode)
        {
            object selected = combo.SelectedItem;
            combo.DataSource = null;
            combo.DataSource = GetMeasureSourceNames(mode);
            if (selected != null)
                combo.SelectedItem = selected;
        }

        private List<string> GetMeasureSourceNames(MeasureLineInputMode mode)
        {
            return GetToolNames()
                .Where(name =>
                {
                    PropetyTool tool = GetTool(FindToolIndexByName(name));
                    if (tool == null)
                        return false;

                    if (mode == MeasureLineInputMode.Line)
                        return tool.TypeTool == TypeTool.Edge || tool.TypeTool == TypeTool.Edge2;

                    return tool.TypeTool == TypeTool.Pattern
                        || tool.TypeTool == TypeTool.Circle
                        || tool.TypeTool == TypeTool.Width
                        || tool.TypeTool == TypeTool.Edge
                        || tool.TypeTool == TypeTool.Edge2;
                })
                .ToList();
        }

        private void RestoreLineSelection(int lineNumber)
        {
            if (lineNumber == 1)
            {
                if (Propety.Line1InputMode == MeasureLineInputMode.Line)
                    RestoreLineToolSelection(cb1, Propety.listLineChoose[0]);
                else
                {
                    RestorePointSelection(cb1, cb2, Propety.listPointChoose[0], ref indexTool1);
                    RestorePointSelection(cb3, cb4, Propety.listPointChoose[1], ref indexTool2);
                }
            }
            else
            {
                if (Propety.Line2InputMode == MeasureLineInputMode.Line)
                    RestoreLineToolSelection(cb5, Propety.listLineChoose[1]);
                else
                {
                    RestorePointSelection(cb5, cb6, Propety.listPointChoose[2], ref indexTool3);
                    RestorePointSelection(cb7, cb8, Propety.listPointChoose[3], ref indexTool4);
                }
            }
        }

        private void RestoreLineToolSelection(ComboBox sourceCombo, Tuple<string, int> choice)
        {
            if (choice == null || choice.Item1 == null) return;
            sourceCombo.SelectedItem = choice.Item1;
        }

        private void RestorePointSelection(ComboBox sourceCombo, ComboBox pointCombo, Tuple<string, int> choice, ref int indexTool)
        {
            if (choice == null || choice.Item1 == null) return;
            sourceCombo.SelectedItem = choice.Item1;
            indexTool = FindToolIndexByName(choice.Item1);
            PropetyTool tool = GetTool(indexTool);
            if (tool == null) return;
            pointCombo.DataSource = null;
            pointCombo.DataSource = tool.Propety2.listP_Center;
            pointCombo.SafeSelectIndex(choice.Item2);
        }

        private void HandleSourceComboChanged(ComboBox sourceCombo, ComboBox pointCombo, ref int indexTool, MeasureLineInputMode mode, int pointChooseIndex, int lineNumber)
        {
            if (sourceCombo.SelectedIndex == -1 || sourceCombo.SelectedItem == null) return;
            string nameTool = sourceCombo.SelectedItem.ToString();
            indexTool = FindToolIndexByName(nameTool);
            PropetyTool tool = GetTool(indexTool);
            if (tool == null || !IsAllowedMeasureSource(tool, mode)) return;

            if (mode == MeasureLineInputMode.Line)
            {
                int lineIndex = lineNumber - 1;
                Propety.listLineChoose[lineIndex] = new Tuple<string, int>(tool.Name, -1);
                CopyLinePreview(tool, lineNumber);
                Global.StatusDraw = StatusDraw.Check;
                return;
            }

            pointCombo.DataSource = null;
            pointCombo.DataSource = tool.Propety2.listP_Center;
        }

        private static bool IsAllowedMeasureSource(PropetyTool tool, MeasureLineInputMode mode)
        {
            if (mode == MeasureLineInputMode.Line)
                return tool.TypeTool == TypeTool.Edge || tool.TypeTool == TypeTool.Edge2;

            return tool.TypeTool == TypeTool.Pattern
                || tool.TypeTool == TypeTool.Circle
                || tool.TypeTool == TypeTool.Width
                || tool.TypeTool == TypeTool.Edge
                || tool.TypeTool == TypeTool.Edge2;
        }

        private void SelectPointFromCombo(ComboBox pointCombo, int toolIndex, int pointChooseIndex, List<Point> targetPoints, int targetPointIndex)
        {
            if (_loadingMeasureUi || pointCombo.SelectedIndex == -1) return;
            PropetyTool tool = GetTool(toolIndex);
            if (tool == null) return;
            if (pointCombo.SelectedIndex >= tool.Propety2.listP_Center.Count) return;

            targetPoints[targetPointIndex] = tool.Propety2.listP_Center[pointCombo.SelectedIndex];
            if (pointChooseIndex < Propety.listRot.Count && pointCombo.SelectedIndex < tool.Propety2.rectRotates.Count)
                Propety.listRot[pointChooseIndex] = tool.Propety2.rectRotates[pointCombo.SelectedIndex];
            Propety.listPointChoose[pointChooseIndex] = new Tuple<string, int>(tool.Name, pointCombo.SelectedIndex);
            Global.StatusDraw = StatusDraw.Check;
        }

        private void CopyLinePreview(PropetyTool tool, int lineNumber)
        {
            if (tool == null || tool.Propety2.listP_Center.Count < 2) return;

            List<Point> targetPoints = lineNumber == 1 ? Propety.listLine1Point : Propety.listLine2Point;
            int rotOffset = lineNumber == 1 ? 0 : 2;
            targetPoints[0] = tool.Propety2.listP_Center[0];
            targetPoints[1] = tool.Propety2.listP_Center[1];
            if (tool.Propety2.rectRotates.Count > 0)
            {
                Propety.listRot[rotOffset] = tool.Propety2.rectRotates[0];
                Propety.listRot[rotOffset + 1] = tool.Propety2.rectRotates[0];
            }
        }
    }
}
