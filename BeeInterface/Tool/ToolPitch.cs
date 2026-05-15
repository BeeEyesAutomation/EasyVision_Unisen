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
    public partial class ToolPitch : UserControl
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
        private EdgeButtonsHelper.ExtraButtons _extraEdgeBtns;
        private ComboBox cbPitchMeasureMode;
        private ComboBox cbPinArrangeMode;
        private NumericUpDown numExpectedPins;
        private NumericUpDown numNominalPitch;
        private NumericUpDown numPitchTolerance;
        private CheckBox chkProjectedPitch;
        private ComboBox cbPinDistanceMode;
        private CheckBox chkUseSharedScale;
        private CheckBox chkUsePinOutlineCenter;
        private NumericUpDown numPinOutlineThresholdOffset;
        private NumericUpDown numPinOutlineDilate;
        private NumericUpDown numPinOutlinePadding;
        private NumericUpDown numPinMaxOutlineExpand;
        // PP-002 / PP-004 era
        private CheckBox chkUseTopHat;
        private NumericUpDown numTopHatKernelPx;
        private NumericUpDown numMinSolidity;
        private CheckBox chkReduceDilateForOutline;
        // PP-007 (edge-boundary mask + edge-geometry center)
        private CheckBox chkUseEdgeBoundary;
        private NumericUpDown numEdgeCannyLow;
        private NumericUpDown numEdgeCannyHigh;
        private CheckBox chkUseEdgeGeometryCenter;
        // PP-008 (gradient refinement)
        private CheckBox chkUseGradientRefinement;
        private NumericUpDown numGradientPatchMargin;
        private NumericUpDown numGradientThreshold;
        private NumericUpDown numClaheClipLimit;
        private NumericUpDown numClaheTileSize;

        public ToolPitch( )
        {
            InitializeComponent();
            _extraEdgeBtns = EdgeButtonsHelper.Attach(tableLayoutPanel15, m =>
            {
                Propety.MethordEdge = m;
                layThreshod.Enabled = false;
            });
            EnsurePinPitchModePanel();
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
                this.VisibleChanged -= ToolVisualMatch_VisibleChanged;
                this.VisibleChanged += ToolVisualMatch_VisibleChanged;

                trackScore.Min = OwnerTool.MinValue;
                trackScore.Max = OwnerTool.MaxValue;
                trackScore.Step = OwnerTool.StepValue;
                trackScore.Value = OwnerTool.Score;

                OwnerTool.StatusTool = StatusTool.WaitCheck;
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
                AdjThreshod.Value = Propety.ThresholdBinary;
                LoadPinPitchModePanel();
                AdjGaussianSmooth.Value = Propety.ValueGau;
                AdjScale.Value = 1/(float)Propety.Scale;
                numCrestCouter.Value= Propety.NumCrestCouter ;
                numRootCounter.Value = Propety.NumRootCouter;
                AdjMagin.Value=Propety.Magin;

                lbCrestCount.Text = Propety.TempCountCrest.ToString();
                lbRootCount.Text = Propety.TempCountRoot.ToString();

                AdjClose.Value = Propety.SizeClose;
                AdjOpen.Value = Propety.SizeOpen;
                AdjClearNoise.Value = Propety.SizeClearsmall;
                AdjClearBig.Value = Propety.SizeClearBig;
                btnClose.IsCLick = Propety.IsClose;
                btnOpen.IsCLick = Propety.IsOpen;
                btnIsClearSmall.IsCLick = Propety.IsClearNoiseSmall;
                btnIsClearBig.IsCLick = Propety.IsClearNoiseBig;
                AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
                AdjClearBig.Enabled = Propety.IsClearNoiseBig;
                AdjOpen.Enabled = Propety.IsOpen;
                AdjClose.Enabled = Propety.IsClose;

                btnEnCrestPitch.IsCLick = Propety.IsEnCrestPitch;
                btnEnCrestHeight.IsCLick= Propety.IsEnCrestHeight;
               
                btnEnRootPitch.IsCLick = Propety.IsEnRootPitch;
                btnEnRootHeight.IsCLick = Propety.IsEnRootHeight;

                btnEnRootCounter.IsCLick = Propety.IsEnRootCounter;
                btnEnCrestCounter.IsCLick = Propety.IsEnCrestCounter;

                btnStrongEdge.IsCLick = btnCloseEdge.IsCLick = btnBinary.IsCLick = btnInvert.IsCLick = false;
                _extraEdgeBtns?.ResetAll();
                layThreshod.Enabled = false;
                switch (Propety.MethordEdge)
                {
                    case MethordEdge.StrongEdges:   btnStrongEdge.IsCLick = true; break;
                    case MethordEdge.CloseEdges:    btnCloseEdge.IsCLick = true; break;
                    case MethordEdge.Binary:        btnBinary.IsCLick = true; layThreshod.Enabled = true; break;
                    case MethordEdge.InvertBinary:  btnInvert.IsCLick = true; layThreshod.Enabled = true; break;
                    case MethordEdge.UltraThin:
                    case MethordEdge.Adaptive:
                    case MethordEdge.DenoiseFirst:  _extraEdgeBtns?.Highlight(Propety.MethordEdge); break;
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
              

             

              
                switch (Propety.Values)
                {
                    case Values.Mean:
                        lbPitchCrestMean.Text = Math.Round(Propety.TempPitchCrest, 3).ToString();
                        lbHeightRootMean.Text = Math.Round(Propety.TempHeightRoot, 3).ToString();
                        lbPitchRootMean.Text = Math.Round(Propety.TempPitchRoot, 3).ToString();
                        lbHeightCrestMean.Text = Math.Round(Propety.TempHeightCrest, 3).ToString();
                        btnMean.IsCLick = true;
                        //Mean
                        lbPitchCrestMean.ForeColor = Color.Black;
                        lbHeightCrestMean.ForeColor = Color.Black;
                        lbPitchRootMean.ForeColor = Color.Black;
                        lbHeightRootMean.ForeColor = Color.Black;
                        //Median
                        lbPitchCrestMedian.ForeColor = Color.LightGray;
                        lbHeightCrestMedian.ForeColor = Color.LightGray;
                        lbPitchRootMedian.ForeColor = Color.LightGray;
                        lbHeightRootMedian.ForeColor = Color.LightGray;
                        //Min
                        lbPitchCrestMin.ForeColor = Color.LightGray;
                        lbHeightCrestMin.ForeColor = Color.LightGray;
                        lbPitchRootMin.ForeColor = Color.LightGray;
                        lbHeightRootMin.ForeColor = Color.LightGray;
                        //Max
                        lbPitchCrestMax.ForeColor = Color.LightGray;
                        lbHeightCrestMax.ForeColor = Color.LightGray;
                        lbPitchRootMax.ForeColor = Color.LightGray;
                        lbHeightRootMax.ForeColor = Color.LightGray;

                        break;
                    case Values.Median:

                        lbHeightRootMedian.Text = Math.Round(Propety.TempHeightRoot, 3).ToString();
                        lbPitchRootMedian.Text = Math.Round(Propety.TempPitchRoot, 3).ToString();
                        lbHeightCrestMedian.Text = Math.Round(Propety.TempHeightCrest, 3).ToString();
                        lbPitchCrestMedian.Text = Math.Round(Propety.TempPitchCrest, 3).ToString();
                        btnMedian.IsCLick = true;
                        //Mean
                        lbPitchCrestMean.ForeColor = Color.LightGray;
                        lbHeightCrestMean.ForeColor = Color.LightGray;
                        lbPitchRootMean.ForeColor = Color.LightGray;
                        lbHeightRootMean.ForeColor = Color.LightGray;
                        //Median
                        lbPitchCrestMedian.ForeColor = Color.Black;
                        lbHeightCrestMedian.ForeColor = Color.Black;
                        lbPitchRootMedian.ForeColor = Color.Black;
                        lbHeightRootMedian.ForeColor = Color.Black;
                        //Min
                        lbPitchCrestMin.ForeColor = Color.LightGray;
                        lbHeightCrestMin.ForeColor = Color.LightGray;
                        lbPitchRootMin.ForeColor = Color.LightGray;
                        lbHeightRootMin.ForeColor = Color.LightGray;
                        //Max
                        lbPitchCrestMax.ForeColor = Color.LightGray;
                        lbHeightCrestMax.ForeColor = Color.LightGray;
                        lbPitchRootMax.ForeColor = Color.LightGray;
                        lbHeightRootMax.ForeColor = Color.LightGray;
                        break;
                    case Values.Min:
                        lbHeightRootMin.Text = Math.Round(Propety.TempHeightRoot, 3).ToString();
                        lbPitchRootMin.Text = Math.Round(Propety.TempPitchRoot, 3).ToString();
                        lbHeightCrestMin.Text = Math.Round(Propety.TempHeightCrest, 3).ToString();
                        lbPitchCrestMin.Text = Math.Round(Propety.TempPitchCrest, 3).ToString();
                        btnMin.IsCLick = true;
                        //Mean
                        lbPitchCrestMean.ForeColor = Color.LightGray;
                        lbHeightCrestMean.ForeColor = Color.LightGray;
                        lbPitchRootMean.ForeColor = Color.LightGray;
                        lbHeightRootMean.ForeColor = Color.LightGray;
                        //Median
                        lbPitchCrestMedian.ForeColor = Color.LightGray;
                        lbHeightCrestMedian.ForeColor = Color.LightGray;
                        lbPitchRootMedian.ForeColor = Color.LightGray;
                        lbHeightRootMedian.ForeColor = Color.LightGray;
                        //Min
                        lbPitchCrestMin.ForeColor = Color.Black;
                        lbHeightCrestMin.ForeColor = Color.Black;
                        lbPitchRootMin.ForeColor = Color.Black;
                        lbHeightRootMin.ForeColor = Color.Black;
                        //Max
                        lbPitchCrestMax.ForeColor = Color.LightGray;
                        lbHeightCrestMax.ForeColor = Color.LightGray;
                        lbPitchRootMax.ForeColor = Color.LightGray;
                        lbHeightRootMax.ForeColor = Color.LightGray;
                        btnMin.PerformClick();
                        break;
                    case Values.Max:

                        lbHeightRootMax.Text = Math.Round(Propety.TempHeightRoot, 3).ToString();
                        lbPitchRootMax.Text = Math.Round(Propety.TempPitchRoot, 3).ToString();
                        lbHeightCrestMax.Text = Math.Round(Propety.TempHeightCrest, 3).ToString();
                        lbPitchCrestMax.Text = Math.Round(Propety.TempPitchCrest, 3).ToString();
                        btnMax.IsCLick = true;
                        //Mean
                        lbPitchCrestMean.ForeColor = Color.LightGray;
                        lbHeightCrestMean.ForeColor = Color.LightGray;
                        lbPitchRootMean.ForeColor = Color.LightGray;
                        lbHeightRootMean.ForeColor = Color.LightGray;
                        //Median
                        lbPitchCrestMedian.ForeColor = Color.LightGray;
                        lbHeightCrestMedian.ForeColor = Color.LightGray;
                        lbPitchRootMedian.ForeColor = Color.LightGray;
                        lbHeightRootMedian.ForeColor = Color.LightGray;
                        //Min
                        lbPitchCrestMin.ForeColor = Color.LightGray;
                        lbHeightCrestMin.ForeColor = Color.LightGray;
                        lbPitchRootMin.ForeColor = Color.LightGray;
                        lbHeightRootMin.ForeColor = Color.LightGray;
                        //Max
                        lbPitchCrestMax.ForeColor = Color.Black;
                        lbHeightCrestMax.ForeColor = Color.Black;
                        lbPitchRootMax.ForeColor = Color.Black;
                        lbHeightRootMax.ForeColor = Color.Black;
                        btnMax.PerformClick();
                        break;
                }
                if (Propety.IsEnCrestPitch)
                {
                    lbPitchCrestMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchCrestMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchCrestMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchCrestMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
                }
                else
                {
                    lbPitchCrestMax.BackColor = Color.WhiteSmoke;
                    lbPitchCrestMin.BackColor = Color.WhiteSmoke;
                    lbPitchCrestMean.BackColor = Color.WhiteSmoke;
                    lbPitchCrestMedian.BackColor = Color.WhiteSmoke;
                }
                if (Propety.IsEnRootPitch)
                {
                    lbPitchRootMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchRootMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchRootMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbPitchRootMedian.BackColor = Color.FromArgb(150, 247, 211, 139);

                }
                else
                {
                    lbPitchRootMax.BackColor = Color.WhiteSmoke;
                    lbPitchRootMin.BackColor = Color.WhiteSmoke;
                    lbPitchRootMean.BackColor = Color.WhiteSmoke;
                    lbPitchRootMedian.BackColor = Color.WhiteSmoke;
                }
                if (Propety.IsEnCrestHeight)
                {
                    lbHeightCrestMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightCrestMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightCrestMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightCrestMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
                }
                else
                {
                    lbHeightCrestMax.BackColor = Color.WhiteSmoke;
                    lbHeightCrestMin.BackColor = Color.WhiteSmoke;
                    lbHeightCrestMean.BackColor = Color.WhiteSmoke;
                    lbHeightCrestMedian.BackColor = Color.WhiteSmoke;
                }
                if (Propety.IsEnRootHeight)
                {
                    lbHeightRootMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightRootMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightRootMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                    lbHeightRootMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
                }
                else
                {
                    lbHeightRootMax.BackColor = Color.WhiteSmoke;
                    lbHeightRootMin.BackColor = Color.WhiteSmoke;
                    lbHeightRootMean.BackColor = Color.WhiteSmoke;
                    lbHeightRootMedian.BackColor = Color.WhiteSmoke;
                }
            }
            catch (Exception ex)
            {
                String s = ex.Message;
            }
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
        private void EnsurePinPitchModePanel()
        {
            if (cbPitchMeasureMode != null)
                return;

            var panel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 12,
                Dock = DockStyle.Top,
                AutoSize = true,
                BackColor = Color.White,
                Padding = new Padding(5)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            cbPitchMeasureMode = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cbPitchMeasureMode.Items.AddRange(Enum.GetNames(typeof(PitchMeasureMode)));
            cbPitchMeasureMode.SelectedIndexChanged += (s, e) =>
            {
                if (Propety == null || cbPitchMeasureMode.SelectedItem == null) return;
                Propety.MeasureMode = (PitchMeasureMode)Enum.Parse(typeof(PitchMeasureMode), cbPitchMeasureMode.SelectedItem.ToString());
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            numExpectedPins = NewNumeric(1, 20, 4, 0);
            numExpectedPins.ValueChanged += (s, e) => { if (Propety != null) Propety.ExpectedPinCount = (int)numExpectedPins.Value; };

            cbPinArrangeMode = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cbPinArrangeMode.Items.AddRange(Enum.GetNames(typeof(PinPitchArrangeMode)));
            cbPinArrangeMode.SelectedIndexChanged += (s, e) =>
            {
                if (Propety == null || cbPinArrangeMode.SelectedItem == null) return;
                Propety.PinArrangeMode = (PinPitchArrangeMode)Enum.Parse(typeof(PinPitchArrangeMode), cbPinArrangeMode.SelectedItem.ToString());
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            numNominalPitch = NewNumeric(0, 1000, 0, 3);
            numNominalPitch.ValueChanged += (s, e) => { if (Propety != null) Propety.NominalPitchMm = (float)numNominalPitch.Value; };

            numPitchTolerance = NewNumeric(0, 100, 0.05m, 3);
            numPitchTolerance.ValueChanged += (s, e) => { if (Propety != null) Propety.PitchToleranceMm = (float)numPitchTolerance.Value; };

            chkProjectedPitch = new CheckBox { Text = "Projected pitch", Checked = true, Dock = DockStyle.Fill };
            chkProjectedPitch.CheckedChanged += (s, e) => { if (Propety != null) Propety.UseProjectedPitch = chkProjectedPitch.Checked; };

            cbPinDistanceMode = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cbPinDistanceMode.Items.AddRange(Enum.GetNames(typeof(PinDistanceMode)));
            cbPinDistanceMode.SelectedIndexChanged += (s, e) =>
            {
                if (Propety == null || cbPinDistanceMode.SelectedItem == null) return;
                Propety.PinDistanceMode = (PinDistanceMode)Enum.Parse(typeof(PinDistanceMode), cbPinDistanceMode.SelectedItem.ToString());
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            chkUseSharedScale = new CheckBox { Text = "Use shared scale (Global.Config.Scale)", Dock = DockStyle.Fill };
            chkUseSharedScale.CheckedChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.UseSharedScale = chkUseSharedScale.Checked;
                if (AdjScale != null) AdjScale.Enabled = !Propety.UseSharedScale;
            };

            chkUsePinOutlineCenter = new CheckBox { Text = "Outline center", Checked = true, Dock = DockStyle.Fill };
            chkUsePinOutlineCenter.CheckedChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.UsePinOutlineCenter = chkUsePinOutlineCenter.Checked;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            numPinOutlineThresholdOffset = NewNumeric(0, 120, 14, 0);
            numPinOutlineThresholdOffset.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.PinOutlineThresholdOffset = (int)numPinOutlineThresholdOffset.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            numPinOutlineDilate = NewNumeric(0, 51, 5, 0);
            numPinOutlineDilate.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.PinOutlineDilate = (int)numPinOutlineDilate.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            numPinOutlinePadding = NewNumeric(0, 100, 8, 0);
            numPinOutlinePadding.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.PinOutlinePadding = (int)numPinOutlinePadding.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            numPinMaxOutlineExpand = NewNumeric(0, 300, 90, 0);
            numPinMaxOutlineExpand.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.PinMaxOutlineExpand = (int)numPinMaxOutlineExpand.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            // ===== PP-002 / PP-004: TopHat + Solidity + ReduceDilate =====
            chkUseTopHat = new CheckBox { Text = "Top-hat (uneven bg)", Dock = DockStyle.Fill };
            chkUseTopHat.CheckedChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.UseTopHat = chkUseTopHat.Checked;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            numTopHatKernelPx = NewNumeric(0, 999, 0, 0);
            numTopHatKernelPx.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.TopHatKernelPx = (int)numTopHatKernelPx.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            numMinSolidity = NewNumeric(0, 1, 0.80m, 2);
            numMinSolidity.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.MinSolidity = (double)numMinSolidity.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            chkReduceDilateForOutline = new CheckBox { Text = "Reduce dilate (≤3)", Dock = DockStyle.Fill };
            chkReduceDilateForOutline.CheckedChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.ReduceDilateForOutline = chkReduceDilateForOutline.Checked;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            // ===== PP-007: Edge boundary + edge-geometry center =====
            chkUseEdgeBoundary = new CheckBox { Text = "Edge boundary mask (Canny)", Dock = DockStyle.Fill };
            chkUseEdgeBoundary.CheckedChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.UseEdgeBoundary = chkUseEdgeBoundary.Checked;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            numEdgeCannyLow = NewNumeric(1, 254, 20, 0);
            numEdgeCannyLow.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.EdgeCannyLow = (int)numEdgeCannyLow.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            numEdgeCannyHigh = NewNumeric(1, 255, 60, 0);
            numEdgeCannyHigh.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.EdgeCannyHigh = (int)numEdgeCannyHigh.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            chkUseEdgeGeometryCenter = new CheckBox { Text = "Edge-geometry center", Dock = DockStyle.Fill };
            chkUseEdgeGeometryCenter.CheckedChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.UseEdgeGeometryCenter = chkUseEdgeGeometryCenter.Checked;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            // ===== PP-008: Gradient refinement (CLAHE+Sobel) =====
            chkUseGradientRefinement = new CheckBox { Text = "Gradient refine (CLAHE+Sobel)", Dock = DockStyle.Fill };
            chkUseGradientRefinement.CheckedChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.UseGradientRefinement = chkUseGradientRefinement.Checked;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            numGradientPatchMargin = NewNumeric(0, 500, 60, 0);
            numGradientPatchMargin.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.GradientPatchMargin = (int)numGradientPatchMargin.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            numGradientThreshold = NewNumeric(1, 254, 25, 0);
            numGradientThreshold.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.GradientThreshold = (int)numGradientThreshold.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            numClaheClipLimit = NewNumeric(0, 40, 3.0m, 1);
            numClaheClipLimit.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.ClaheClipLimit = (double)numClaheClipLimit.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };
            numClaheTileSize = NewNumeric(2, 64, 8, 0);
            numClaheTileSize.ValueChanged += (s, e) =>
            {
                if (Propety == null) return;
                Propety.ClaheTileSize = (int)numClaheTileSize.Value;
                OwnerTool.StatusTool = StatusTool.WaitCheck;
            };

            AddPanelRow(panel, "Mode", cbPitchMeasureMode, 0);
            AddPanelRow(panel, "Pins", numExpectedPins, 1);
            AddPanelRow(panel, "Arrange", cbPinArrangeMode, 2);
            AddPanelRow(panel, "Pitch mm", numNominalPitch, 3);
            AddPanelRow(panel, "Tol mm", numPitchTolerance, 4);
            panel.Controls.Add(chkProjectedPitch, 1, 5);
            AddPanelRow(panel, "Distance", cbPinDistanceMode, 6);
            panel.Controls.Add(chkUseSharedScale, 1, 7);
            panel.Controls.Add(chkUsePinOutlineCenter, 1, 8);
            AddPanelRow(panel, "Outline T", numPinOutlineThresholdOffset, 9);
            AddPanelRow(panel, "Outline Dil", numPinOutlineDilate, 10);
            AddPanelRow(panel, "Outline Pad", numPinOutlinePadding, 11);
            AddPanelRow(panel, "Outline Max", numPinMaxOutlineExpand, 12);
            panel.Controls.Add(chkUseTopHat, 1, 13);
            AddPanelRow(panel, "TopHat K", numTopHatKernelPx, 14);
            AddPanelRow(panel, "Min Solidity", numMinSolidity, 15);
            panel.Controls.Add(chkReduceDilateForOutline, 1, 16);
            panel.Controls.Add(chkUseEdgeBoundary, 1, 17);
            AddPanelRow(panel, "Canny Low", numEdgeCannyLow, 18);
            AddPanelRow(panel, "Canny High", numEdgeCannyHigh, 19);
            panel.Controls.Add(chkUseEdgeGeometryCenter, 1, 20);
            panel.Controls.Add(chkUseGradientRefinement, 1, 21);
            AddPanelRow(panel, "Grad Margin", numGradientPatchMargin, 22);
            AddPanelRow(panel, "Grad Thr", numGradientThreshold, 23);
            AddPanelRow(panel, "CLAHE Clip", numClaheClipLimit, 24);
            AddPanelRow(panel, "CLAHE Tile", numClaheTileSize, 25);
            panel.RowCount = 26;
            for (int i = 0; i < 26; i++)
                panel.RowStyles.Add(new RowStyle());

            tableLayoutPanel1.RowCount += 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Controls.Add(panel, 0, tableLayoutPanel1.RowCount - 1);
        }

        private static NumericUpDown NewNumeric(decimal min, decimal max, decimal value, int decimals)
        {
            return new NumericUpDown
            {
                Minimum = min,
                Maximum = max,
                Value = value,
                DecimalPlaces = decimals,
                Increment = decimals > 0 ? 0.01m : 1m,
                Dock = DockStyle.Fill
            };
        }

        private static void AddPanelRow(TableLayoutPanel panel, string label, Control control, int row)
        {
            panel.RowStyles.Add(new RowStyle());
            panel.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            panel.Controls.Add(control, 1, row);
        }

        private void LoadPinPitchModePanel()
        {
            if (cbPitchMeasureMode == null || Propety == null)
                return;

            cbPitchMeasureMode.SelectedItem = Propety.MeasureMode.ToString();
            cbPinArrangeMode.SelectedItem = Propety.PinArrangeMode.ToString();
            numExpectedPins.Value = Math.Min(numExpectedPins.Maximum, Math.Max(numExpectedPins.Minimum, Propety.ExpectedPinCount));
            numNominalPitch.Value = ClampDecimal((decimal)Propety.NominalPitchMm, numNominalPitch.Minimum, numNominalPitch.Maximum);
            numPitchTolerance.Value = ClampDecimal((decimal)Propety.PitchToleranceMm, numPitchTolerance.Minimum, numPitchTolerance.Maximum);
            chkProjectedPitch.Checked = Propety.UseProjectedPitch;
            if (cbPinDistanceMode != null) cbPinDistanceMode.SelectedItem = Propety.PinDistanceMode.ToString();
            if (chkUseSharedScale != null)
            {
                chkUseSharedScale.Checked = Propety.UseSharedScale;
                if (AdjScale != null) AdjScale.Enabled = !Propety.UseSharedScale;
            }
            if (chkUsePinOutlineCenter != null)
                chkUsePinOutlineCenter.Checked = Propety.UsePinOutlineCenter;
            if (numPinOutlineThresholdOffset != null)
                numPinOutlineThresholdOffset.Value = ClampDecimal(Propety.PinOutlineThresholdOffset, numPinOutlineThresholdOffset.Minimum, numPinOutlineThresholdOffset.Maximum);
            if (numPinOutlineDilate != null)
                numPinOutlineDilate.Value = ClampDecimal(Propety.PinOutlineDilate, numPinOutlineDilate.Minimum, numPinOutlineDilate.Maximum);
            if (numPinOutlinePadding != null)
                numPinOutlinePadding.Value = ClampDecimal(Propety.PinOutlinePadding, numPinOutlinePadding.Minimum, numPinOutlinePadding.Maximum);
            if (numPinMaxOutlineExpand != null)
                numPinMaxOutlineExpand.Value = ClampDecimal(Propety.PinMaxOutlineExpand, numPinMaxOutlineExpand.Minimum, numPinMaxOutlineExpand.Maximum);
            if (chkUseTopHat != null) chkUseTopHat.Checked = Propety.UseTopHat;
            if (numTopHatKernelPx != null)
                numTopHatKernelPx.Value = ClampDecimal(Propety.TopHatKernelPx, numTopHatKernelPx.Minimum, numTopHatKernelPx.Maximum);
            if (numMinSolidity != null)
                numMinSolidity.Value = ClampDecimal((decimal)Propety.MinSolidity, numMinSolidity.Minimum, numMinSolidity.Maximum);
            if (chkReduceDilateForOutline != null) chkReduceDilateForOutline.Checked = Propety.ReduceDilateForOutline;
            if (chkUseEdgeBoundary != null) chkUseEdgeBoundary.Checked = Propety.UseEdgeBoundary;
            if (numEdgeCannyLow != null)
                numEdgeCannyLow.Value = ClampDecimal(Propety.EdgeCannyLow, numEdgeCannyLow.Minimum, numEdgeCannyLow.Maximum);
            if (numEdgeCannyHigh != null)
                numEdgeCannyHigh.Value = ClampDecimal(Propety.EdgeCannyHigh, numEdgeCannyHigh.Minimum, numEdgeCannyHigh.Maximum);
            if (chkUseEdgeGeometryCenter != null) chkUseEdgeGeometryCenter.Checked = Propety.UseEdgeGeometryCenter;
            if (chkUseGradientRefinement != null) chkUseGradientRefinement.Checked = Propety.UseGradientRefinement;
            if (numGradientPatchMargin != null)
                numGradientPatchMargin.Value = ClampDecimal(Propety.GradientPatchMargin, numGradientPatchMargin.Minimum, numGradientPatchMargin.Maximum);
            if (numGradientThreshold != null)
                numGradientThreshold.Value = ClampDecimal(Propety.GradientThreshold, numGradientThreshold.Minimum, numGradientThreshold.Maximum);
            if (numClaheClipLimit != null)
                numClaheClipLimit.Value = ClampDecimal((decimal)Propety.ClaheClipLimit, numClaheClipLimit.Minimum, numClaheClipLimit.Maximum);
            if (numClaheTileSize != null)
                numClaheTileSize.Value = ClampDecimal(Propety.ClaheTileSize, numClaheTileSize.Minimum, numClaheTileSize.Maximum);
        }

        private static decimal ClampDecimal(decimal value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void ToolWidth_ScoreChanged(float obj)
        {
           trackScore.Value = obj;
        }

        private void ToolWidth_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {if (Global.IsRun) return;
            if (OwnerTool.StatusTool == StatusTool.Done)
            {if(Propety.IsCalib)
                {
                    btnCalib.IsCLick = false;
                    btnCalib.Enabled = true;
                    Propety.IsCalib = false;
                    if (Propety.MeasureMode == PitchMeasureMode.PinPitch)
                    {
                        numNominalPitch.Value = ClampDecimal((decimal)Propety.NominalPitchMm, numNominalPitch.Minimum, numNominalPitch.Maximum);
                        btnTest.Enabled = true;
                        return;
                    }
                    lbCrestCount.Text = Propety.PitchResult.Crests.Length.ToString();
                    lbRootCount.Text = Propety.PitchResult.Roots.Length.ToString();

                    lbHeightRootMean.Text = Math.Round(Propety.PitchResult.RootHMeanMM, 3).ToString();
                    lbHeightRootMedian.Text = Math.Round(Propety.PitchResult.RootHMedianMM, 3).ToString();
                    lbHeightRootMin.Text = Math.Round(Propety.PitchResult.RootHMinMM, 3).ToString();
                    lbHeightRootMax.Text = Math.Round(Propety.PitchResult.RootHMaxMM, 3).ToString();

                    lbPitchRootMean.Text = Math.Round(Propety.PitchResult.PitchRootMeanMM, 3).ToString();
                    lbPitchRootMedian.Text = Math.Round(Propety.PitchResult.PitchRootMedianMM, 3).ToString();
                    lbPitchRootMin.Text = Math.Round(Propety.PitchResult.PitchRootMinMM, 3).ToString();
                    lbPitchRootMax.Text = Math.Round(Propety.PitchResult.PitchRootMaxMM, 3).ToString();

                    lbHeightCrestMean.Text = Math.Round(Propety.PitchResult.CrestHMeanMM, 3).ToString();
                    lbHeightCrestMedian.Text = Math.Round(Propety.PitchResult.CrestHMedianMM, 3).ToString();
                    lbHeightCrestMin.Text = Math.Round(Propety.PitchResult.CrestHMinMM, 3).ToString();
                    lbHeightCrestMax.Text = Math.Round(Propety.PitchResult.CrestHMaxMM, 3).ToString();

                    lbPitchCrestMean.Text = Math.Round(Propety.PitchResult.PitchMeanMM, 3).ToString();
                    lbPitchCrestMedian.Text = Math.Round(Propety.PitchResult.PitchMedianMM, 3).ToString();
                    lbPitchCrestMin.Text = Math.Round(Propety.PitchResult.PitchMinMM, 3).ToString();
                    lbPitchCrestMax.Text = Math.Round(Propety.PitchResult.PitchMaxMM, 3).ToString();
                    numCrestCouter.Value = Propety.TempCountCrest;
                    numRootCounter.Value = Propety.TempCountRoot;
                }
              
                btnTest.Enabled = true;
              
            }
           
        }

        private void trackScore_ValueChanged(float obj)
        {
            OwnerTool.Score=trackScore.Value;
         }
        public bool IsClear = false;
        public Pitch Propety { get; set; }
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
            Common.TryGetTool(Global.IndexToolSelected).RunToolAsync();
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
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);

            
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

        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global. TypeCrop)
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

        
    

        private void btnVer_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Vertical;
        }

       

        private void btnHori_Click(object sender, EventArgs e)
        {
            Propety.LineOrientation = LineOrientation.Horizontal;
        }

      

        private void trackScore_Load(object sender, EventArgs e)
        {

        }

        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            layThreshod.Enabled = true;
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
            Propety.MethordEdge = MethordEdge.InvertBinary;
            layThreshod.Enabled = true;
        }

        private void btnStrongEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.StrongEdges;
            layThreshod.Enabled = false;
        }

        private void btnCloseEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.CloseEdges;
            layThreshod.Enabled = false;
        }
        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall = (int)AdjClearNoise.Value;
        }

        private void btnEnMorphology_Click(object sender, EventArgs e)
        {
            Propety.IsClose = btnClose.IsCLick;
            AdjClose.Enabled = Propety.IsClose;
        }

        private void btnEnableNoise_Click(object sender, EventArgs e)
        {

            Propety.IsClearNoiseSmall = btnIsClearSmall.IsCLick;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
        }

        private void AdjMorphology_ValueChanged(float obj)
        {

            Propety.SizeClose = (int)AdjClose.Value;
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
      

      
        private void AdjScale_ValueChanged(float obj)
        {
            Propety.Scale =1/ AdjScale.Value;
        }

        private void btnMean_Click(object sender, EventArgs e)
        {
            Propety.Values=Values.Mean;

            //Mean
            lbPitchCrestMean.ForeColor = Color.Black ;
            lbHeightCrestMean.ForeColor = Color.Black;
            lbPitchRootMean.ForeColor = Color.Black;
            lbHeightRootMean.ForeColor = Color.Black;
            //Median
            lbPitchCrestMedian.ForeColor = Color.LightGray;
            lbHeightCrestMedian.ForeColor = Color.LightGray;
            lbPitchRootMedian.ForeColor = Color.LightGray;
            lbHeightRootMedian.ForeColor = Color.LightGray;
            //Min
            lbPitchCrestMin.ForeColor = Color.LightGray;
            lbHeightCrestMin.ForeColor = Color.LightGray;
            lbPitchRootMin.ForeColor = Color.LightGray;
            lbHeightRootMin.ForeColor = Color.LightGray;
            //Max
            lbPitchCrestMax.ForeColor = Color.LightGray;
            lbHeightCrestMax.ForeColor = Color.LightGray;
            lbPitchRootMax.ForeColor = Color.LightGray;
            lbHeightRootMax.ForeColor = Color.LightGray;

        }

        private void btnMedian_Click(object sender, EventArgs e)
        {
            Propety.Values = Values.Median;
            //Mean
            lbPitchCrestMean.ForeColor = Color.LightGray;
            lbHeightCrestMean.ForeColor = Color.LightGray;
            lbPitchRootMean.ForeColor = Color.LightGray;
            lbHeightRootMean.ForeColor = Color.LightGray;
            //Median
            lbPitchCrestMedian.ForeColor = Color.Black;
            lbHeightCrestMedian.ForeColor = Color.Black;
            lbPitchRootMedian.ForeColor = Color.Black;
            lbHeightRootMedian.ForeColor = Color.Black;
            //Min
            lbPitchCrestMin.ForeColor = Color.LightGray;
            lbHeightCrestMin.ForeColor = Color.LightGray;
            lbPitchRootMin.ForeColor = Color.LightGray;
            lbHeightRootMin.ForeColor = Color.LightGray;
            //Max
            lbPitchCrestMax.ForeColor = Color.LightGray;
            lbHeightCrestMax.ForeColor = Color.LightGray;
            lbPitchRootMax.ForeColor = Color.LightGray;
            lbHeightRootMax.ForeColor = Color.LightGray;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            Propety.Values = Values.Min;
            //Mean
            lbPitchCrestMean.ForeColor = Color.LightGray;
            lbHeightCrestMean.ForeColor = Color.LightGray;
            lbPitchRootMean.ForeColor = Color.LightGray;
            lbHeightRootMean.ForeColor = Color.LightGray;
            //Median
            lbPitchCrestMedian.ForeColor = Color.LightGray;
            lbHeightCrestMedian.ForeColor = Color.LightGray;
            lbPitchRootMedian.ForeColor = Color.LightGray;
            lbHeightRootMedian.ForeColor = Color.LightGray;
            //Min
            lbPitchCrestMin.ForeColor = Color.Black;
            lbHeightCrestMin.ForeColor = Color.Black;
            lbPitchRootMin.ForeColor = Color.Black;
            lbHeightRootMin.ForeColor = Color.Black;
            //Max
            lbPitchCrestMax.ForeColor = Color.LightGray;
            lbHeightCrestMax.ForeColor = Color.LightGray;
            lbPitchRootMax.ForeColor = Color.LightGray;
            lbHeightRootMax.ForeColor = Color.LightGray;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            Propety.Values = Values.Max;
            //Mean
            lbPitchCrestMean.ForeColor = Color.LightGray;
            lbHeightCrestMean.ForeColor = Color.LightGray;
            lbPitchRootMean.ForeColor = Color.LightGray;
            lbHeightRootMean.ForeColor = Color.LightGray;
            //Median
            lbPitchCrestMedian.ForeColor = Color.LightGray;
            lbHeightCrestMedian.ForeColor = Color.LightGray;
            lbPitchRootMedian.ForeColor = Color.LightGray;
            lbHeightRootMedian.ForeColor = Color.LightGray;
            //Min
            lbPitchCrestMin.ForeColor = Color.LightGray;
            lbHeightCrestMin.ForeColor = Color.LightGray;
            lbPitchRootMin.ForeColor = Color.LightGray;
            lbHeightRootMin.ForeColor = Color.LightGray;
            //Max
            lbPitchCrestMax.ForeColor = Color.Black;
            lbHeightCrestMax.ForeColor = Color.Black;
            lbPitchRootMax.ForeColor = Color.Black;
            lbHeightRootMax.ForeColor = Color.Black;
        }

        private void btnEnCrestPitch_Click(object sender, EventArgs e)
        {
            Propety.IsEnCrestPitch=btnEnCrestPitch.IsCLick;
            if (Propety.IsEnCrestPitch)
            {
                lbPitchCrestMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchCrestMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchCrestMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchCrestMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
            }
            else
            {
                lbPitchCrestMax.BackColor = Color.WhiteSmoke;
                lbPitchCrestMin.BackColor = Color.WhiteSmoke;
                lbPitchCrestMean.BackColor = Color.WhiteSmoke;
                lbPitchCrestMedian.BackColor = Color.WhiteSmoke;
            }

          

        }

        private void btnEnRootPitch_Click(object sender, EventArgs e)
        {
            Propety.IsEnRootPitch=btnEnRootPitch.IsCLick;
            if (Propety.IsEnRootPitch)
            {
                lbPitchRootMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchRootMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchRootMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbPitchRootMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
             
            }
            else
            {
                lbPitchRootMax.BackColor = Color.WhiteSmoke;
                lbPitchRootMin.BackColor = Color.WhiteSmoke;
                lbPitchRootMean.BackColor = Color.WhiteSmoke;
                lbPitchRootMedian.BackColor = Color.WhiteSmoke;
            }

          
        }

        private void btnEnCreshHeight_Click(object sender, EventArgs e)
        {
            Propety.IsEnCrestHeight=btnEnCrestHeight.IsCLick;
            if (Propety.IsEnCrestHeight)
            {
                lbHeightCrestMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightCrestMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightCrestMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightCrestMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
            }
            else
            {
                lbHeightCrestMax.BackColor = Color.WhiteSmoke;
                lbHeightCrestMin.BackColor = Color.WhiteSmoke;
                lbHeightCrestMean.BackColor = Color.WhiteSmoke;
                lbHeightCrestMedian.BackColor = Color.WhiteSmoke;
            }
           
        }

        private void btnEnRootHeight_Click(object sender, EventArgs e)
        {
            Propety.IsEnRootHeight=btnEnRootHeight.IsCLick;
            if (Propety.IsEnRootHeight)
            {
                lbHeightRootMax.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightRootMin.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightRootMean.BackColor = Color.FromArgb(150, 247, 211, 139);
                lbHeightRootMedian.BackColor = Color.FromArgb(150, 247, 211, 139);
            }
            else
            {
                lbHeightRootMax.BackColor = Color.WhiteSmoke;
                lbHeightRootMin.BackColor = Color.WhiteSmoke;
                lbHeightRootMean.BackColor = Color.WhiteSmoke;
                lbHeightRootMedian.BackColor = Color.WhiteSmoke;
            }
        }

        private void btnEnCrestCounter_Click(object sender, EventArgs e)
        {
            Propety.IsEnCrestCounter=btnEnCrestCounter.IsCLick;
            if(Propety.IsEnCrestCounter) 
            lbCrestCount.BackColor = Color.FromArgb(150, 247, 211, 139);
            else
                lbCrestCount.BackColor = Color.WhiteSmoke;
            numCrestCouter.Enabled = btnEnCrestCounter.IsCLick;
        }

        private void btnEnRootCounter_Click(object sender, EventArgs e)
        {
            Propety.IsEnRootCounter=btnEnRootCounter.IsCLick;
            if (Propety.IsEnRootCounter)
                lbRootCount.BackColor = Color.FromArgb(150, 247, 211, 139);
            else
                lbRootCount.BackColor = Color.WhiteSmoke;
        
            numRootCounter.Enabled = btnEnRootCounter.IsCLick;
        }
        private void btnLess_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Less;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Equal;
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.More;
        }

        private void numCrestCouter_ValueChanged(float obj)
        {
            Propety.NumCrestCouter = (int)numCrestCouter.Value;
        }

        private void numRootCounter_ValueChanged(float obj)
        {
            Propety.NumRootCouter = (int)numRootCounter.Value;
        }

        private void AdjGaussianSmooth_ValueChanged(float obj)
        {
            Propety.ValueGau=AdjGaussianSmooth.Value;
        }

        private void AdjMagin_ValueChanged(float obj)
        {
            Propety.Magin = (int)AdjMagin.Value;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            Propety.IsCalib= true;
            Common.TryGetTool(Global.IndexToolSelected).RunToolAsync();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            EditRectRot1.Visible = !btn1.IsCLick;
        }

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
         
        }
    }
}
