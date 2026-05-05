using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Funtion.Engines;
using BeeGlobal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolCircle : UserControl
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

        public ToolCircle()
        {
            InitializeComponent();
            if (Propety == null)
                Propety = new Circle();
        }

        Stopwatch timer = new Stopwatch();
        public BackgroundWorker worker = new BackgroundWorker();
        public Circle Propety { get; set; }

        public void LoadPara()
        {
            CircleEngineRunner.MarkOwnerWaiting(OwnerTool);
            if (OwnerTool != null)
            {
                OwnerTool.StatusToolChanged -= ToolCircle_StatusToolChanged;
                OwnerTool.StatusToolChanged += ToolCircle_StatusToolChanged;
            }
            if (OwnerTool != null)
            {
                OwnerTool.ScoreChanged -= ToolCircle_ScoreChanged;
                OwnerTool.ScoreChanged += ToolCircle_ScoreChanged;
            }

            var state = CircleEngineRunner.ReadFromOwner(OwnerTool, Propety);
            trackScore.Min = state.ScoreMin;
            trackScore.Max = state.ScoreMax;
            trackScore.Step = state.ScoreStep;
            trackScore.Value = state.Score;

            AdjScale.Value = state.Scale;
            trackThreshold.IsInital = true;
            trackThreshold.Value = state.Threshold;
            trackMinInlier.IsInital = true;
            trackMinInlier.Value = state.MinInliers;
            trackIterations.IsInital = true;
            trackIterations.Value = state.Iterations;
            numMinRadius.Value = state.MinRadius;
            numMaxRadius.Value = state.MaxRadius;

            AdjThreshod.Value = state.ThresholdBinary;
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
            AdjMorphology.Value = state.SizeClose;
            AdjOpen.Value = state.SizeOpen;
            AdjClearNoise.Value = state.SizeClearSmall;
            AdjClearBig.Value = state.SizeClearBig;

            switch (state.MethordEdge)
            {
                case MethordEdge.StrongEdges:
                    btnStrongEdge.IsCLick = true; lay32.Enabled = false;
                    break;
                case MethordEdge.CloseEdges:
                    btnCloseEdge.IsCLick = true; lay32.Enabled = false;
                    break;
                case MethordEdge.Binary:
                    btnBinary.IsCLick = true; lay32.Enabled = true;
                    break;
                case MethordEdge.InvertBinary:
                    btnInvert.IsCLick = true; lay32.Enabled = true;
                    break;
            }

            switch (state.CircleScanDirection)
            {
                case CircleScanDirection.InsideOut:
                    btnInsideOut.IsCLick = true;
                    break;
                case CircleScanDirection.OutsideIn:
                    btnOutsideIn.IsCLick = true;
                    break;
            }

            btnArea.IsCLick = true;
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            btnElip.IsCLick = state.AreaShape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = state.AreaShape == ShapeType.Rectangle ? true : false;
            btnHexagon.IsCLick = state.AreaShape == ShapeType.Hexagon ? true : false;
            btnPolygon.IsCLick = state.AreaShape == ShapeType.Polygon ? true : false;
            btnWhite.IsCLick = state.AreaIsWhite;
            btnBlack.IsCLick = !state.AreaIsWhite;
            btn1.IsCLick = true;
            btn2.IsCLick = true;
            btn3.IsCLick = true;
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            //  cv3.Pattern();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            Common.TryGetTool(Global.IndexToolSelected).RunToolAsync();
        }

        private void rjButton3_Click_2(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
