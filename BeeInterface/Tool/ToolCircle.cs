using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Funtion.Engines;
using BeeGlobal;
using BeeInterface.Group;
using System;
using System.Collections.Generic;
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
            EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotMask };
            EditRectRot1.Refresh();
            EditRectRot1.IsHide = false;
            EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
            this.VisibleChanged -= ToolVisualMatch_VisibleChanged;
            this.VisibleChanged += ToolVisualMatch_VisibleChanged;

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

         
            lay62.Enabled = false;
            switch (state.MethordEdge)
            {
                case MethordEdge.StrongEdges:   btnStrongEdge.IsCLick = true; break;
                case MethordEdge.CloseEdges:    btnCloseEdge.IsCLick = true; break;
                case MethordEdge.Binary:        btnBinary.IsCLick = true; lay62.Enabled = true; break;
              
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

           
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            
        }
        private void ToolVisualMatch_VisibleChanged(object sender, EventArgs e)
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

        private void btnStable_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Stable;
            lay62.Enabled = false;
        }
    }
}
