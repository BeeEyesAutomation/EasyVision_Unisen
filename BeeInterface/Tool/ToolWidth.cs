using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion.Engines;
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

          
            try
            {
                EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotMask };
                EditRectRot1.Refresh();
                EditRectRot1.IsHide = false;
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
                EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
                this.VisibleChanged -= ToolWidth_VisibleChanged;
                this.VisibleChanged += ToolWidth_VisibleChanged;
                var state = WidthEngineRunner.ReadFromOwner(OwnerTool, Propety);
                trackScore.Min = state.ScoreMin;
                trackScore.Max = state.ScoreMax;
                trackScore.Step = state.ScoreStep;
                trackScore.Value = state.Score;
                btnEnEqualizeHist.IsCLick = Propety.IsEnEqualizeHist;
                WidthEngineRunner.MarkOwnerWaiting(OwnerTool);
                 if (OwnerTool != null)
                 {
                     OwnerTool.StatusToolChanged -= ToolWidth_StatusToolChanged;
                     OwnerTool.StatusToolChanged += ToolWidth_StatusToolChanged;
                 }
                 if (OwnerTool != null)
                 {
                     OwnerTool.ScoreChanged -= ToolWidth_ScoreChanged;
                     OwnerTool.ScoreChanged += ToolWidth_ScoreChanged;
                 }
                AdjThreshod.Value = state.ThresholdBinary;

                AdjScale.Value = state.Scale;
                trackMinInlier.Value = state.MinInliers;
                trackMaxLine.Value = state.MaximumLine;
                AdjRANSACIterations.Value = state.RansacIterations;
                AdjRANSACThreshold.Value = (float)state.RansacThreshold;
                numMinLen.Value = state.MinLen;
                numMaxLen.Value = state.MaxLen;
                switch (state.MethordEdge)
                {
                    case MethordEdge.StrongEdges:
                        btnStrongEdge.IsCLick = true; lay62.Enabled = false;
                        break;
                    case MethordEdge.CloseEdges:
                        btnCloseEdge.IsCLick = true; lay62.Enabled = false;
                        break;
                    case MethordEdge.Binary:
                        btnBinary.IsCLick = true; lay62.Enabled = true;
                        break;
                    case MethordEdge.Stable:
                        btnStable.IsCLick = true; lay62.Enabled = true;
                        break;
                }
                switch (state.LineOrientation)
                {
                    case LineOrientation.Vertical:
                        btnVer.IsCLick = true;
                        break;
                    case LineOrientation.Horizontal:
                        btnHori.IsCLick = true;
                        break;
                }
                switch (state.SegmentStatType)
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
                switch (state.GapExtremum)
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
                AdjMorphology.Value = state.SizeClose;
                AdjOpen.Value = state.SizeOpen;
                AdjClearNoise.Value = state.SizeClearSmall;
                AdjClearBig.Value = state.SizeClearBig;
               
                btnClose.IsCLick = state.IsClose;
                btnOpen.IsCLick = state.IsOpen;
                btnIsClearSmall.IsCLick = state.IsClearNoiseSmall;
                btnIsClearBig.IsCLick = state.IsClearNoiseBig;
                AdjClearNoise.Enabled = state.IsClearNoiseSmall;
                AdjClearBig.Enabled = state.IsClearNoiseBig;
                AdjOpen.Enabled = state.IsOpen;
                AdjMorphology.Enabled = state.IsClose;
               
            }
            catch (Exception ex)
            {
                String s = ex.Message;
            }
        }

        private void ToolWidth_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                EditRectRot1.IsHide = true;
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            }
        }

        private void EditRectRot1_RotateCurentChanged(RectRotate obj)
        {
            switch (obj.TypeCrop)
            {
                case TypeCrop.Area:
                    Propety.rotArea = obj; break;
                case TypeCrop.Crop:
                    Propety.rotCrop = obj; break;
                case TypeCrop.Mask:
                    Propety.rotMask = obj; break;

            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global.TypeCrop)
            {
                //case TypeCrop.Crop:
                //    Propety.rotCrop.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
                //    break;
                //case TypeCrop.Area:
                //    Propety.rotArea.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
                //    break;
                case TypeCrop.Mask:
                    Propety.rotMask = null;// = btnElip.IsCLick;
                    break;

            }
            //  G.EditTool.View.imgView.Invalidate();
        }

        private void SetShapeFor(TypeCrop which, ShapeType shape)
        {

            RectRotate rr = null;
            if (which == TypeCrop.Area) { if (Propety.rotArea == null) Propety.rotArea = new RectRotate(); rr = Propety.rotArea; }
            else if (which == TypeCrop.Mask) { if (Propety.rotMask == null) Propety.rotMask = new RectRotate(); rr = Propety.rotMask; }
            else { if (Propety.rotCrop == null) Propety.rotCrop = new RectRotate(); rr = Propety.rotCrop; }

            rr.Shape = shape;
            if (shape == ShapeType.Polygon)
            {
                if (rr.PolyLocalPoints == null || rr.PolyLocalPoints.Count() == 0)
                    NewShape(shape);
                else
                {
                    rr.UpdateFromPolygon(true);
                }
            }
            if (shape == ShapeType.Hexagon)
            {
                if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Count() == 0)
                    NewShape(shape);
            }


            Global.TypeCrop = which;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;



        }
        private void NewShape(ShapeType newShape)
        {
            // 1) Chốt shape hiện tại
            var prop = Common.TryGetTool(Global.IndexToolSelected).Propety2;
            RectRotate rr = null;
            if (Global.TypeCrop == TypeCrop.Area) rr = prop?.rotArea;
            else if (Global.TypeCrop == TypeCrop.Mask) rr = prop?.rotMask;
            else rr = prop?.rotCrop;

            if (rr != null)
            {
                // Nếu đang drag: chấm dứt
                rr._dragAnchor = AnchorPoint.None;
                rr.ActiveVertexIndex = -1;

                // Nếu là polygon đang dựng dở
                if (rr.Shape == ShapeType.Polygon && rr.IsPolygonClosed == false)
                {
                    // CHỌN 1 TRONG 3 CHÍNH SÁCH:

                    // (A) Giữ tạm nguyên trạng (không chuẩn hoá, không xoá điểm)
                    // -> Không làm gì thêm

                    // (B) Tự đóng & chuẩn hoá (nếu muốn)
                    // nếu có >=3 điểm thì tự đóng:
                    // if (rr.PolyLocalPoints != null && rr.PolyLocalPoints.Count >= 3) {
                    //     var p0 = rr.PolyLocalPoints[0];
                    //     rr.PolyLocalPoints.Add(p0);
                    //     rr.IsPolygonClosed = true;
                    //     rr.UpdateFromPolygon(updateAngle: rr.AutoOrientPolygon);
                    // }

                    // (C) Huỷ polygon đang dựng
                    // rr.PolygonClear();
                }
            }



            // 3) Gán shape mới & chuẩn bị khung
            if (rr == null)
            {
                // tuỳ code lưu trữ của bạn mà tạo mới:
                rr = new RectRotate();
                if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
                else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
                else prop.rotCrop = rr;
            }

            rr.Shape = newShape;

            switch (newShape)
            {
                case ShapeType.Polygon:
                    // Local sạch, xoá điểm cũ: chờ click điểm đầu tiên
                    rr.ResetFrameForNewPolygonHard();
                    rr.AutoOrientPolygon = false; // thường tắt lúc dựng, bạn có thể để true nếu quen
                    break;

                case ShapeType.Rectangle:
                case ShapeType.Ellipse:
                case ShapeType.Hexagon:
                    // Không cần xoá toàn bộ; chỉ đảm bảo không kéo theo trạng thái cũ
                    rr._dragAnchor = AnchorPoint.None;
                    rr.ActiveVertexIndex = -1;

                    // Option: reset rotation cho phiên mới (tuỳ UX)
                    // rr._rectRotation = 0f;

                    // Để trống _rect: user kéo trái→phải để tạo mới theo logic MouseDown/Move của bạn
                    rr._rect = RectangleF.Empty;

                    // Hexagon: offsets về 0
                    if (newShape == ShapeType.Hexagon)
                    {
                        if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Length != 6)
                            rr.HexVertexOffsets = new PointF[6];
                        for (int i = 0; i < 6; i++) rr.HexVertexOffsets[i] = PointF.Empty;
                    }

                    break;
            }

            // Cập nhật về prop
            if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
            else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
            else prop.rotCrop = rr;


        }

        ShapeType ShapeType = ShapeType.Rectangle;
        private void btnHexagon_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Hexagon;
            SetShapeFor(Global.TypeCrop, ShapeType);
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Polygon;

            SetShapeFor(Global.TypeCrop, ShapeType);
        }

        private void btnNewShape_Click(object sender, EventArgs e)
        {
            NewShape(ShapeType);
        }
        private void btnRect_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Rectangle;
            SetShapeFor(Global.TypeCrop, ShapeType);

        }

        private void btnElip_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Ellipse;
            SetShapeFor(Global.TypeCrop, ShapeType);

        }

    
        private void ToolWidth_ScoreChanged(float obj)
        {
           trackScore.Value = obj;
        }

        private void ToolWidth_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {if (Global.IsRun) return;
            if (OwnerTool.StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
                if (Propety.IsCalibs)
                {
                    btnCalib.IsCLick = false;
                    Propety.IsCalibs = false;
                    btnCalib.Enabled = true;
                    trackMinInlier.Value = Propety.MinInliers;
                    numMaxLen.Value = Propety.MaxLen;
                    numMinLen.Value = Propety.MinLen;
                }
            }
           
        }

        private void trackScore_ValueChanged(float obj)
        {
            WidthEngineRunner.ApplyScoreToOwner(OwnerTool, trackScore.Value);
         }
        public bool IsClear = false;
        public Width Propety { get; set; }
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
            btnTest.Enabled = false;
            if (!WidthEngineRunner.TryStartSelectedTool())
            {
                btnTest.Enabled = true;
                btnTest.IsCLick = false;
            }
        }
        bool IsFullSize = false;
        private void btnCropHalt_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
            
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);

            
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    
        private void rjButton3_Click_1(object sender, EventArgs e)
        {

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
          //  OutLine.LoadEdge();
          
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
            WidthEngineRunner.BeginCalibration(Propety);

            if (!WidthEngineRunner.TryStartSelectedTool())
            {
                Propety.IsCalibs = false;
                btnCalib.Enabled = true;
            }
            
        }

        private void trackScore_Load(object sender, EventArgs e)
        {

        }

        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            lay62.Enabled = true;
        }

      

        private void numMinLen_ValueChanged(float obj)
        {
            Propety.MinLen = (int)numMinLen.Value;
        }

        private void numMaxLen_ValueChanged(float obj)
        {
            Propety.MaxLen = (int)numMaxLen.Value;
        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

       
        private void AdjThreshod_ValueChanged(float obj)
        {
            Propety.ThresholdBinary = (int)AdjThreshod.Value;
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Stable;
            lay62.Enabled = false;
        }

        private void btnStrongEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.StrongEdges;
            lay62.Enabled = false;
        }

        private void btnCloseEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.CloseEdges;
            lay62.Enabled = false;
        }
        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall = (int)AdjClearNoise.Value;
        }

        private void btnEnMorphology_Click(object sender, EventArgs e)
        {
            Propety.IsClose = btnClose.IsCLick;
            AdjMorphology.Enabled = Propety.IsClose;
        }

        private void btnEnableNoise_Click(object sender, EventArgs e)
        {

            Propety.IsClearNoiseSmall = btnIsClearSmall.IsCLick;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
        }

        private void AdjMorphology_ValueChanged(float obj)
        {

            Propety.SizeClose = (int)AdjMorphology.Value;
        }

      
        private void AdjOpen_ValueChanged(float obj)
        {
            Propety.SizeOpen = (int)AdjOpen.Value;
        }

        private void AdjClearBig_ValueChanged(float obj)
        {
            Propety.SizeClearBig = (int)AdjClearBig.Value;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Propety.IsOpen = btnOpen.IsCLick;
            AdjOpen.Enabled = Propety.IsOpen;
        }

        private void btnIsClearBig_Click(object sender, EventArgs e)
        {
            Propety.IsClearNoiseBig = btnIsClearBig.IsCLick;
            AdjClearBig.Enabled = Propety.IsClearNoiseBig;

        }
        private void AdjRANSACIterations_ValueChanged(float obj)
        {
            Propety.RansacIterations = (int)AdjRANSACIterations.Value;
        }

        private void AdjRANSACThreshold_ValueChanged(float obj)
        {
            Propety.RansacThreshold = AdjRANSACThreshold.Value;
        }
        private void AdjScale_ValueChanged(float obj)
        {
            Propety.Scale = AdjScale.Value;
        }

       
        private void btn3_Click(object sender, EventArgs e)
        {
            lay3.Visible = !btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            lay4.Visible=!btn4.IsCLick;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            lay5.Visible = !btn5.IsCLick;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
          
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            lay61.Visible = !btn6.IsCLick;
            lay62.Visible=!btn6.IsCLick;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            AdjScale.Visible = !btn7.IsCLick;
        }

       

        private void btn8_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn8.IsCLick;
        }

        private void btnEnEqualizeHist_Click(object sender, EventArgs e)
        {
            Propety.IsEnEqualizeHist = btnEnEqualizeHist.IsCLick;
        }

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
         
        }
    }
}
