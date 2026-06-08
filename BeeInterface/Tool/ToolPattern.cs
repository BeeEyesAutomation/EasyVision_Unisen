using BeeCore;
using BeeCore.Core;
using BeeCpp;
using BeeGlobal;
using BeeInterface;
using Cyotek.Windows.Forms;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShapeType = BeeGlobal.ShapeType;
using Size = System.Drawing.Size;

namespace BeeInterface
{
    [Serializable()]
    // ToolPattern owns pattern-tool parameter UI and handler orchestration;
    // keep image matching and scoring behavior in BeeCore.Unit.Patterns.
    public partial class ToolPattern : UserControl
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
        private TableLayoutPanel colorLayout;
        private TableLayoutPanel colorModeLayout;
        private TableLayoutPanel colorPickLayout;
        private TableLayoutPanel colorNgPickLayout;
       
        private RJButton btnColorHSV;
        private RJButton btnColorRGB;
        private RJButton btnGetColor;
        private RJButton btnGetColorNG;
        private PictureBox picColorPreview;
        private PictureBox picColorNgPreview;
        private Button btnUndoColor;
        private Button btnClearColor;
        private Button btnUndoColorNG;
        private Button btnClearColorNG;
        private AdjustBarEx trackColorExtraction;
        private AdjustBarEx trackColorExtractionNG;
        private AdjustBarEx AdjColorScoreNG;
        private Label lbColorMode;
        private Label lbColorList;
        private Label lbColorExtraction;
        private Label lbColorSet;
        private Label lbColorNgExtraction;
        private Label lbColorNgSet;
        private Label lbColorScore;
        private enum ColorPickTarget
        {
            None,
            Mask,
            Ng
        }
        private ColorPickTarget colorPickTarget = ColorPickTarget.None;
        public ToolPattern( )
        {
            InitializeComponent();
            // BuildMultiTemplateUi defined in Designer.cs nhưng gọi từ constructor để VS
            // Forms Designer không cố parse custom method trong InitializeComponent (sẽ
            // báo "Method not found" khi mở form trong Designer).
            BuildMultiTemplateUi();
            BuildResizeTemplateUi();
            BuildColorUi();
            BuildMethodSelectorUi();
            BuildSubTemplateTabUi();

            if (Propety == null)
                Propety = new Patterns();
        }
        

        public void LoadPara()
        {
            EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotCrop, Propety.rotLimit, Propety.rotMask };
            EditRectRot1.Refresh();
            EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
            EditRectRot1.IsHide = false;
            this.VisibleChanged -= ToolPattern_VisibleChanged;
            this.VisibleChanged += ToolPattern_VisibleChanged;
            Global.SetColorChange -= Global_SetColorChange;
            Global.SetColorChange += Global_SetColorChange;
            btnBestObj.IsCLick = Propety.SearchPattern == SearchPattern.BestObj?true:false;
            btnAllObj.IsCLick = Propety.SearchPattern == SearchPattern.AllObj ? true : false;
            if (Propety.bmRaw != null)
            {
                imgTemp.Image = Propety.bmRaw;
            }
            OwnerTool.StatusTool = StatusTool.WaitCheck;
            trackAngle.Value =(int) Propety.Angle;

            layOverLap.Visible = Propety.EnableNms;
            if (Propety.Angle > 360) Propety.Angle = 360;

            if (Propety.Angle == 0)
            {
                Propety.Angle = 1;
            }
            AdjOffSetLeft.Value =(int) Propety.ColorNgOffsetLeft;
            AdjOffSetTop.Value = (int)Propety.ColorNgOffsetTop;
            AdjOffSetBot.Value = (int)Propety.ColorNgOffsetBottom;
            AdjOffSetRight.Value = (int)Propety.ColorNgOffsetRight;
            btnHard.IsCLick=Propety.DifficultyPattern==DifficultyPattern.Hard?true:false;
            btnNormal.IsCLick=Propety.DifficultyPattern==DifficultyPattern.Normal?true:false;
            btnEasy.IsCLick=Propety.DifficultyPattern==DifficultyPattern.Easy?true:false;
            if (Propety.EnableResizeTemplate && Propety.EnableScaleSearch)
                Propety.EnableScaleSearch = false;
            btnEnScale.IsCLick= Propety.EnableScaleSearch  ;
            btnEnScale.Text = Propety.EnableScaleSearch == true ? "ON" : "OFF";
            numAdjScale.Enabled = Propety.EnableScaleSearch;
            numAdjStepScale.Enabled = Propety.EnableScaleSearch;
            numAdjScale.Value = Propety.ScalePattern;
            numAdjStepScale.Value = Propety.ScaleStep;
            SyncResizeTemplateUi();
            btnEnableKeepFilter.IsCLick = Propety.EnableKeepFilter;
            btnEnableOverLap.IsCLick = Propety.EnableNms;
            btnEnableValidator.IsCLick = Propety.EnableValidator;
            btnAntiGlue.IsCLick = Propety.EnableAngleRobustRefine;
            btnEnScale.IsCLick = Propety.EnableScaleSearch;
            float angle = (Propety.rotCrop._rectRotation) - (Propety.rotArea._rectRotation);
            Propety.AngleLower = angle - Propety.Angle;
            Propety.AngleUper = angle + Propety.Angle;
            trackScore.Min = OwnerTool.MinValue;
            trackScore.Max = OwnerTool.MaxValue;
            trackScore.Step = OwnerTool.StepValue;
            trackScore.Value = OwnerTool.Score;
            if (Propety.MaxObject == 0) Propety.MaxObject = 1;
            AdjMaximumObj.Value = Propety.MaxObject;
            AdjStepAngle.Value = Propety.StepAngle;
            AdjLimitCounter.Value= Propety.LimitCounter;
            trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
           
            ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            ckSIMD.IsCLick = Propety.ckSIMD;
            ckSubPixel.IsCLick = Propety.ckSubPixel;
          switch(Propety.Compare)
            {
                case Compares.Equal:
                    btnEqual.IsCLick = true;
                    break;
                case Compares.Less:
                    btnLess.IsCLick = true;
                    break;
                case Compares.More:
                    btnMore.IsCLick = true;
                    break;
            }    
         
            AdjMorphology.Value = Propety.SizeClose;
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
            AdjMorphology.Enabled = Propety.IsClose;
         

            txtAddPLC.Text = Propety.AddPLC;
            //adjScale.Value = Propety.Scale;
            if (Propety.listCLMaskShow == null)
                Propety.listCLMaskShow = new List<Color>();
            if (Propety.listCLNgShow == null)
                Propety.listCLNgShow = new List<Color>();
            btnEnableColorCheck.IsCLick = Propety.EnableColorCheck;
            btnEnableColorCheck.Text = Propety.EnableColorCheck ? "ON" : "OFF";
            btnColorHSV.IsCLick = Propety.TypeColor == ColorGp.HSV;
            btnColorRGB.IsCLick = Propety.TypeColor == ColorGp.RGB;
            btnGetColor.IsCLick = false;
            btnGetColorNG.IsCLick = false;
            Propety.IsGetColor = false;
            trackColorExtraction.Value = Propety.ExtractionMask;
            trackColorExtractionNG.Value = Propety.ExtractionNG;
            AdjColorScoreNG.Value = Propety.ScoreNG;
          
            picColorPreview.Invalidate();
            picColorNgPreview.Invalidate();
            ApplyColorUiState();
            BindMultiTemplateUi();
            btnCPU.IsCLick = Propety.UseCpu;
            btnGPU.IsCLick = Propety.UseGpu;
            btnMultiThread.IsCLick = Propety.EnableMultiThread;
            numThread.Value = Propety.NumThreads <= 0 ? 1 : Propety.NumThreads;
            SyncThreadUi();

            //btnZero0.IsCLick=Propety.ZeroPos==ZeroPos.Zero?true:false;
            //btnZeroAdj.IsCLick = Propety.ZeroPos == ZeroPos.ZeroADJ ? true : false;

             if (OwnerTool != null)

             {

                 OwnerTool.StatusToolChanged -= ToolPattern_StatusToolChanged;

                 OwnerTool.StatusToolChanged += ToolPattern_StatusToolChanged;

             }
        }

        private void ToolPattern_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                Global.SetColorChange -= Global_SetColorChange;
                Global.IsGetColor = false;
                Propety.IsGetColor = false;
                colorPickTarget = ColorPickTarget.None;
                if (btnGetColor != null)
                    btnGetColor.IsCLick = false;
                if (btnGetColorNG != null)
                    btnGetColorNG.IsCLick = false;
                EditRectRot1.IsHide = true;
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
                // FIX C6: thiếu unsubscribe StatusToolChanged khi không visible -> sẽ leak khi LoadPara chạy lại với OwnerTool khác
                if (OwnerTool != null)
                    OwnerTool.StatusToolChanged -= ToolPattern_StatusToolChanged;
            }
        }

        private void BuildColorUi()
        {
            colorLayout = new TableLayoutPanel();
            colorModeLayout = new TableLayoutPanel();
            colorPickLayout = new TableLayoutPanel();
            colorNgPickLayout = new TableLayoutPanel();
         
            btnColorHSV = new RJButton();
            btnColorRGB = new RJButton();
            btnGetColor = new RJButton();
            btnGetColorNG = new RJButton();
            picColorPreview = new PictureBox();
            picColorNgPreview = new PictureBox();
            btnUndoColor = new Button();
            btnClearColor = new Button();
            btnUndoColorNG = new Button();
            btnClearColorNG = new Button();
            trackColorExtraction = new AdjustBarEx();
            trackColorExtractionNG = new AdjustBarEx();
            AdjColorScoreNG = new AdjustBarEx();
            AdjColorScoreNG.Max = 1000;
            lbColorMode = new Label();
            lbColorList = new Label();
            lbColorExtraction = new Label();
            lbColorSet = new Label();
            lbColorNgExtraction = new Label();
            lbColorNgSet = new Label();
            lbColorScore = new Label();

            panelColorHost.Controls.Clear();
            panelColorHost.AutoScroll = true;
            panelColorHost.AutoSize = true;
            panelColorHost.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        //    panelColorHost.Padding = new Padding(0);

            colorLayout.ColumnCount = 1;
            colorLayout.RowCount = 14;
            colorLayout.AutoSize = true;
            colorLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            colorLayout.Dock = DockStyle.Top;
            colorLayout.BackColor = SystemColors.Control;
            for (int i = 0; i < 14; i++)
                colorLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            colorLayout.RowStyles.Add(new RowStyle(SizeType.Absolute,45f));
            panelColorHost.Controls.Add(colorLayout);

          
            ConfigureSectionLabel(lbColorList, "Color Type");
            ConfigureSectionLabel(lbColorSet, "List Color Mask");
            ConfigureSectionLabel(lbColorExtraction, "ExternColor Mask");
            ConfigureSectionLabel(lbColorNgSet, "List Color NG");
            ConfigureSectionLabel(lbColorNgExtraction, "ExternColor NG");
            ConfigureSectionLabel(lbColorScore, "Score NG");

           

            colorModeLayout.ColumnCount = 2;
            colorModeLayout.RowCount = 1;
            colorModeLayout.Dock = DockStyle.Fill;
            
            colorModeLayout.BackColor = Color.White;
            colorModeLayout.Height = 50;
            colorModeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            colorModeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            colorModeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            ConfigureToggleButton(btnColorHSV, "HSV", btnColorHSV_Click, Corner.Left, false);
            ConfigureToggleButton(btnColorRGB, "RGB", btnColorRGB_Click, Corner.Right, false);
            colorModeLayout.Controls.Add(btnColorHSV, 0, 0);
            colorModeLayout.Controls.Add(btnColorRGB, 1, 0);

            colorPickLayout.ColumnCount = 4;
            colorPickLayout.RowCount = 1;
            colorPickLayout.Dock = DockStyle.Fill;
            colorPickLayout.BackColor = Color.White;
            colorPickLayout.Height = 50;
            colorPickLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            colorPickLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            colorPickLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            colorPickLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            colorPickLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));

            ConfigureToggleButton(btnGetColor, "Get Color", btnGetColor_Click);
            ConfigureToggleButton(btnGetColorNG, "Get Color", btnGetColorNG_Click);

            picColorPreview.BackColor = Color.Gainsboro;
            picColorPreview.Dock = DockStyle.Fill;
           // picColorPreview.Margin = new Padding(5, 0, 5, 0);
            picColorPreview.Paint += picColorPreview_Paint;

            picColorNgPreview.BackColor = Color.Gainsboro;
            picColorNgPreview.Dock = DockStyle.Fill;
           // picColorNgPreview.Margin = new Padding(5, 0, 5, 0);
            picColorNgPreview.Paint += picColorNgPreview_Paint;

            btnUndoColor.Dock = DockStyle.Fill;
          //  btnUndoColor.Margin = new Padding(5, 0, 5, 0);
            btnUndoColor.Text = "Undo";
            btnUndoColor.Click += btnUndoColor_Click;

            btnClearColor.Dock = DockStyle.Fill;
            btnClearColor.Margin = new Padding(0);
            btnClearColor.Text = "Clear";
            btnClearColor.Click += btnClearColor_Click;

            btnUndoColorNG.Dock = DockStyle.Fill;
           // btnUndoColorNG.Margin = new Padding(5, 0, 5, 0);
            btnUndoColorNG.Text = "Undo";
            btnUndoColorNG.Click += btnUndoColorNG_Click;

            btnClearColorNG.Dock = DockStyle.Fill;
            btnClearColorNG.Margin = new Padding(0);
            btnClearColorNG.Text = "Clear";
            btnClearColorNG.Click += btnClearColorNG_Click;

            colorPickLayout.Controls.Add(btnGetColor, 0, 0);
            colorPickLayout.Controls.Add(picColorPreview, 1, 0);
            colorPickLayout.Controls.Add(btnUndoColor, 2, 0);
            colorPickLayout.Controls.Add(btnClearColor, 3, 0);

            colorNgPickLayout.ColumnCount = 4;
            colorNgPickLayout.RowCount = 1;
            colorNgPickLayout.Dock = DockStyle.Fill;
            colorNgPickLayout.BackColor = Color.White;
            colorNgPickLayout.Height = 50;
            colorNgPickLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            colorNgPickLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            colorNgPickLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            colorNgPickLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,60F));
            colorNgPickLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            colorNgPickLayout.Controls.Add(btnGetColorNG, 0, 0);
            colorNgPickLayout.Controls.Add(picColorNgPreview, 1, 0);
            colorNgPickLayout.Controls.Add(btnUndoColorNG, 2, 0);
            colorNgPickLayout.Controls.Add(btnClearColorNG, 3, 0);

            ConfigureAdjustBar(trackColorExtraction, 0, 100, 1, trackColorExtraction_ValueChanged);
            ConfigureAdjustBar(trackColorExtractionNG, 0, 100, 1, trackColorExtractionNG_ValueChanged);
            ConfigureAdjustBar(AdjColorScoreNG, 0, 1000000, 1, AdjColorScoreNG_ValueChanged);
            colorLayout.Controls.Add(lbColorList, 0, 2);
            colorLayout.Controls.Add(colorModeLayout, 0, 3);
            colorLayout.Controls.Add(lbColorSet, 0, 4);
            colorLayout.Controls.Add(colorPickLayout, 0, 5);
            colorLayout.Controls.Add(lbColorExtraction, 0, 6);
            colorLayout.Controls.Add(trackColorExtraction, 0, 7);
            colorLayout.Controls.Add(lbColorNgSet, 0, 8);
            colorLayout.Controls.Add(colorNgPickLayout, 0, 9);
            colorLayout.Controls.Add(lbColorNgExtraction, 0, 10);
            colorLayout.Controls.Add(trackColorExtractionNG, 0, 11);
            colorLayout.Controls.Add(lbColorScore, 0, 12);
            colorLayout.Controls.Add(AdjColorScoreNG, 0, 13);
        }

        private void ConfigureSectionLabel(Label label, string text)
        {
            label.AutoSize = true;
            label.BackColor = Color.FromArgb(100, 114, 114, 114);
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label.ForeColor = Color.Transparent;
           // label.Margin = new Padding(5, 10, 5, 0);
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void ConfigureToggleButton(RJButton button, string text, EventHandler handler, Corner corner = Corner.Both, bool isUngroup = true)
        {
            button.AutoFont = false;
            button.AutoFontHeightRatio = 0.75F;
            button.AutoFontWidthRatio = 0.92F;
            button.AutoImage = true;
            button.AutoImageMode = RJButton.ImageFitMode.Contain;
            button.AutoImageTint = true;
            button.BackColor = Color.White;
            button.BackgroundColor = Color.White;
            button.BorderColor = Color.White;
            button.BorderRadius = 10;
            button.BorderSize = 1;
            button.ClickBotColor = Color.FromArgb(247, 211, 139);
            button.ClickMidColor = Color.FromArgb(246, 204, 120);
            button.ClickTopColor = Color.FromArgb(244, 192, 89);
            button.ContentPadding = new Padding(8, 6, 8, 6);
            button.Corner = corner;
            button.Dock = DockStyle.Fill;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button.ForeColor = Color.Black;
            button.IsNotChange = false;
            button.IsRect = false;
            button.IsTouch = false;
            button.IsUnGroup = isUngroup;
            button.Multiline = false;
            button.Text = text;
            button.TextColor = Color.Black;
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            button.UseVisualStyleBackColor = false;
            button.Click += handler;
        }

        private void ConfigureAdjustBar(AdjustBarEx bar, float min, float max, float step, Action<float> handler)
        {
            bar.AutoRepeatAccelDeltaMs = -5;
            bar.AutoRepeatAccelerate = true;
            bar.AutoRepeatEnabled = true;
            bar.AutoRepeatInitialDelay = 400;
            bar.AutoRepeatInterval = 60;
            bar.AutoRepeatMinInterval = 20;
            bar.AutoShowTextbox = true;
            bar.AutoSizeTextbox = true;
            bar.BackColor = Color.White;
            bar.BarLeftGap = 20;
            bar.BarRightGap = 6;
            bar.ChromeGap = 8;
            bar.ChromeWidthRatio = 0.14F;
            bar.ColorBorder = Color.LightGray;
            bar.ColorFill = Color.FromArgb(246, 213, 143);
            bar.ColorScale = Color.LightGray;
            bar.ColorThumb = Color.FromArgb(246, 201, 110);
            bar.ColorThumbBorder = Color.FromArgb(246, 201, 110);
            bar.ColorTrack = Color.LightGray;
            bar.Decimals = 0;
            bar.Dock = DockStyle.Fill;
            bar.EdgePadding = 2;
            bar.Font = new Font("Segoe UI", 10F);
            bar.InnerPadding = new Padding(10, 6, 10, 6);
            bar.KeyboardStep = 1F;
          //  bar.Margin = new Padding(5, 0, 5, 0);
            bar.MatchTextboxFontToThumb = true;
            bar.Max = max;
            bar.Min = min;
            bar.MinimumSize = new Size(140, 36);
            bar.Name = Guid.NewGuid().ToString("N");
            bar.Radius = 8;
            bar.ShowValueOnThumb = true;
            bar.SnapToStep = true;
            bar.StartWithTextboxHidden = true;
            bar.Step = step;
            bar.TextboxFontSize = 20F;
            bar.TextboxSidePadding = 10;
            bar.TextboxWidth = 600;
            bar.ThumbDiameterRatio = 2F;
            bar.ThumbValueBold = true;
            bar.ThumbValueFontScale = 1.5F;
            bar.TightEdges = true;
            bar.TrackHeightRatio = 0.45F;
            bar.TrackWidthRatio = 1F;
            bar.ValueChanged += handler;
        }

        private void ApplyColorUiState()
        {
            bool enabled = Propety.EnableColorCheck;
            btnEnableColorCheck.Text = enabled ? "ON" : "OFF";

            panelColorHost.Visible = enabled;
        }

        private void SyncThreadUi()
        {
            if (Propety.NumThreads <= 0)
                Propety.NumThreads = 1;
            numThread.Enabled = Propety.EnableMultiThread;
            Propety.MaxThread = Propety.EnableMultiThread ? Propety.NumThreads : 1;
        }

        private void Global_SetColorChange(bool obj)
        {
            if (!obj || !Propety.IsGetColor)
                return;

            Propety.hSV = BeeCore.Common.HSVSample;
            Propety.rGB = BeeCore.Common.RGBSample;
            if (colorPickTarget == ColorPickTarget.Mask)
            {
                Propety.AddMaskColor();
                Propety.SetMaskColor();
                picColorPreview.Invalidate();
            }
            else if (colorPickTarget == ColorPickTarget.Ng)
            {
                Propety.AddNgColor();
                Propety.SetNgColor();
                picColorNgPreview.Invalidate();
            }
        }

        private void EditRectRot1_RotateCurentChanged(RectRotate obj)
        {
            if (_syncingAreaLimitEditor || obj == null)
                return;

            switch (obj.TypeCrop)
            {
                case TypeCrop.Area:
                    RectRotate oldArea = Propety.rotArea?.Clone();
                    RectRotate constrainedArea = Propety.ConstrainAreaToContainLocalLimits(obj);
                    if (constrainedArea != null)
                    {
                        Propety.RebaseLocalLimitsForAreaSize(oldArea, constrainedArea);
                        obj._rect = constrainedArea._rect;
                        obj._PosCenter = constrainedArea._PosCenter;
                        obj._rectRotation = constrainedArea._rectRotation;
                        obj.Shape = constrainedArea.Shape;
                        obj.Name = constrainedArea.Name;
                        obj.TypeCrop = constrainedArea.TypeCrop;
                    }
                    Propety.rotArea = obj;
                    if (Propety.IsMultiTemplate && Propety.CheckByAreaLimit)
                        PushRotLimitForCurrentRow();
                    break;
                case TypeCrop.Crop:
                    Propety.rotCrop = obj; break;
                case TypeCrop.Mask:
                    Propety.rotMask = obj; break;
                case TypeCrop.Limit:
                    {
                        // Lưu rotLimit vào entry đang chọn trong multi-mode.
                        if (Propety != null && Propety.IsMultiTemplate && Propety.CheckByAreaLimit)
                        {
                            int idx = dgvTemplates?.CurrentCell?.RowIndex ?? -1;
                            if (idx >= 0 && idx < Propety.MultiTemplates.Count)
                            {
                                var limitLocal = Propety.AreaGlobalToLocal(obj);
                                if (limitLocal != null)
                                {
                                    limitLocal.Name = "Limit: " + (Propety.MultiTemplates[idx].Label ?? "");
                                    limitLocal.TypeCrop = TypeCrop.Limit;
                                }
                                Propety.MultiTemplates[idx].RotLimit = limitLocal;
                                Propety.MarkBatchDirty();
                            }
                        }
                        break;
                    }
            }
        }

        private void ToolPattern_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (Global.IsRun) return;
            // FIX C5: OwnerTool có thể null nếu event fire sau khi tool bị remove; cũng filter sender khác OwnerTool
            if (OwnerTool == null || tool != OwnerTool) return;
            if (OwnerTool.StatusTool == StatusTool.Done)
            {
                if (btnTest != null) btnTest.Enabled = true;
            }
        }

        private void trackScore_ValueChanged(float obj)
        {
            OwnerTool.Score = (float)trackScore.Value;
           

        }

        public Patterns Propety { get; set; }
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public void GetTemp(RectRotate rotateRect, Mat matRegister)
        {
           
                //float angle = rotateRect._rectRotation;
                //if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
                //Mat matCrop =BeeCore.Cropper.CropRotatedRect(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
                //if (matCrop.Type() == MatType.CV_8UC3)
                //    Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
                //if (Propety.IsAreaWhite)
                //    Cv2.BitwiseNot(matTemp, matTemp);
           
        }
       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

        }


        Bitmap bmResult ;
        
        public int indexTool = 0;


        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {


            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }


        private void ckSIMD_Click(object sender, EventArgs e)
        {
            Propety.ckSIMD = !Propety.ckSIMD;
              if(Propety.ckSIMD)
                {
                ckSIMD.BackColor = Color.Goldenrod;
                ckSIMD.BorderColor = Color.DarkGoldenrod;
                }
                else
                { 
                ckSIMD.BackColor = Color.WhiteSmoke;
                ckSIMD.BorderColor = Color.Silver;
                ckSIMD.TextColor = Color.Black;
                }
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void ckBitwiseNot_Click(object sender, EventArgs e)
        {
            Propety.ckBitwiseNot = !Propety.ckBitwiseNot;
            if (Propety.ckBitwiseNot)
            {
                ckBitwiseNot.BackColor = Color.Goldenrod;
                ckBitwiseNot.BorderColor = Color.DarkGoldenrod;
            }
            else
            {
                ckBitwiseNot.BackColor = Color.WhiteSmoke;
                ckBitwiseNot.BorderColor = Color.Silver;
                ckBitwiseNot.TextColor = Color.Black;
            }
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void ckSubPixel_Click(object sender, EventArgs e)
        {
            Propety.ckSubPixel = !Propety.ckSubPixel;
            if (Propety.ckSubPixel)
            {
                ckSubPixel.BackColor = Color.Goldenrod;
                ckSubPixel.BorderColor = Color.DarkGoldenrod;
            }
            else
            {
                ckSubPixel.BackColor = Color.WhiteSmoke;
                ckSubPixel.BorderColor = Color.Silver;
                ckSubPixel.TextColor = Color.Black;
            }
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
      
      
        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
            
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
        }


        private void btnNormal_Click(object sender, EventArgs e)
        {
          
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = true;

        }
        private void btnAreaWhite_Click(object sender, EventArgs e)
        {
            Propety.IsAreaWhite = true;
            GetTemp(Propety.rotCrop, BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw);
          //  G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
          
         //   G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

   
        private void btnLearning_Click(object sender, EventArgs e)
        {

            if (Propety.rotCrop != null)
                if (Propety.rotCrop._rect.Width != 0 && Propety.rotCrop._rect.Height != 0)
                {
                    Propety.bmRaw = Propety.LearnPattern(BeeCore.Common.listCamera[Propety.IndexThread].matRaw.Clone(), false).ToBitmap();
                    imgTemp.Image = Propety.bmRaw;

                    // Multi-mode: auto-add template vừa Sample vào list (rotCrop hiện tại
                    // được capture làm angle range per-entry).
                    TryAutoAddSampleToMulti();
                }


        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            Common.TryGetTool(Global.IndexToolSelected).RunToolAsync();
        }

        private void trackAngle_ValueChanged(float obj)
        {
            Propety.Angle = trackAngle.Value;
          
            if (Propety.Angle > 360) Propety.Angle = 360;
            if (Propety.Angle == 0)
            {
                Propety.Angle = 1;
            }
            float angle = (Propety.rotCrop._rectRotation) - (Propety.rotArea._rectRotation);
            Propety.AngleLower = angle - Propety.Angle;
            Propety.AngleUper = angle + Propety.Angle;

        }

  
        private void trackMaxOverLap_ValueChanged(float obj)
        {

           Propety.OverLap= trackMaxOverLap.Value/100.0 ;
          
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

   
        private void numLimitCounter_ValueChanged(float obj)
        {
            Propety.LimitCounter = (int)AdjLimitCounter.Value;
        }



       
        private void btnModeEdge_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Edge;
        }
     

        private void btnModePattern_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Pattern;
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
        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            layThreshod.Enabled = true;
        }

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
          //  Patterns.LoadEdge();
          
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

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (Propety.TypeMode == Mode.Edge)
            //    if(!G.IniEdge)
            //    {
            //        workLoadModel.RunWorkerAsync();
            //        return;

            //    }    
          
          
        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

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

        private void AdjMaximumObj_ValueChanged(float obj)
        {
            Propety.MaxObject = (int)AdjMaximumObj.Value;
        }

        private void AdjStepAngle_ValueChanged(float obj)
        {

            Propety.StepAngle = 
                AdjStepAngle.Value;
        }

        private void txtAddPLC_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtAddPLC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==Keys.Enter)
            {
                Propety.AddPLC=txtAddPLC.Text;
            }    
        }

     
      

        private void btn3_Click(object sender, EventArgs e)
        {
            lay3.Visible = !btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            trackAngle.Visible = !btn4.IsCLick;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            laySample.Visible =! btn5.IsCLick;
        }

       

      

        private void btn6_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn6.IsCLick;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            layLimitCouter.Visible = !btn8.IsCLick;
            AdjLimitCounter.Visible = !btn8.IsCLick;
        }

        private void btnZeroAdj_Click(object sender, EventArgs e)
        {
            Propety.ZeroPos = ZeroPos.ZeroADJ;
        }

        //private void adjScale_ValueChanged(float obj)
        //{
        //    Propety.Scale =(float)adjScale.Value;
        //}

        //private void btn7_Click_1(object sender, EventArgs e)
        //{
        //    lay71.Visible = !btn7.IsCLick;
        //    lay72.Visible = !btn7.IsCLick;
        //}

        private void btnZero0_Click(object sender, EventArgs e)
        {
            Propety.ZeroPos = ZeroPos.Zero;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            EditRectRot1.Visible = !btn1.IsCLick;
        }

        private void btnBestObj_Click(object sender, EventArgs e)
        {
            Propety.SearchPattern = SearchPattern.BestObj;
        }

        private void btnAllObj_Click(object sender, EventArgs e)
        {
            Propety.SearchPattern = SearchPattern.AllObj;
        }

        private void btnEasy_Click(object sender, EventArgs e)
        {
            Propety.DifficultyPattern= DifficultyPattern.Easy;
        }

        private void btnNormal_Click_1(object sender, EventArgs e)
        {
            Propety.DifficultyPattern = DifficultyPattern.Normal;
        }

        private void btnHard_Click(object sender, EventArgs e)
        {
            Propety.DifficultyPattern = DifficultyPattern.Hard;
        }

        private void btnEnScale_Click(object sender, EventArgs e)
        {
            Propety.EnableScaleSearch = btnEnScale.IsCLick;
            if (Propety.EnableScaleSearch)
            {
                Propety.EnableResizeTemplate = false;
                SyncResizeTemplateUi();
            }
            btnEnScale.Text = Propety.EnableScaleSearch == true ? "ON" : "OFF";
            numAdjScale.Enabled = Propety.EnableScaleSearch;
            numAdjStepScale.Enabled = Propety.EnableScaleSearch;
            Propety.MarkBatchDirty();

        }

        private void numAdjScale_ValueChanged(float obj)
        {
            Propety.ScalePattern =(int) numAdjScale.Value;
        }

        private void numAdjStepScale_ValueChanged(float obj)
        {
            Propety.ScaleStep = (int)numAdjStepScale.Value;
        }

        //private void btnResizeTemplate_Click(object sender, EventArgs e)
        //{
        //    if (layResizeTemplate != null && btnResizeTemplate != null)
        //        layResizeTemplate.Visible = !btnResizeTemplate.IsCLick;
    //    }

        private void btnEnableResizeTemplate_Click(object sender, EventArgs e)
        {
            if (Propety == null) return;
            Propety.EnableResizeTemplate = btnEnableResizeTemplate.IsCLick;
            if (Propety.EnableResizeTemplate)
            {
                Propety.EnableScaleSearch = false;
                btnEnScale.IsCLick = false;
                btnEnScale.Text = "OFF";
                numAdjScale.Enabled = false;
                numAdjStepScale.Enabled = false;
            }
            SyncResizeTemplateUi();
            Propety.MarkBatchDirty();
        }

        private void AdjResizeTemplate_ValueChanged(float obj)
        {
            if (Propety == null || AdjResizeTemplate == null) return;
            int value = (int)AdjResizeTemplate.Value;
            if (value < 25) value = 25;
            if (value > 100) value = 100;
            Propety.ResizeTemplatePercent = value;
            Propety.MarkBatchDirty();
        }

        private void SyncResizeTemplateUi()
        {
            if (Propety == null || btnEnableResizeTemplate == null || AdjResizeTemplate == null)
                return;

            int value = Propety.ResizeTemplatePercent;
            if (value < 25) value = 25;
            if (value > 100) value = 100;
            Propety.ResizeTemplatePercent = value;

            btnEnableResizeTemplate.IsCLick = Propety.EnableResizeTemplate;
            btnEnableResizeTemplate.Text = Propety.EnableResizeTemplate ? "ON" : "OFF";
            AdjResizeTemplate.Enabled = Propety.EnableResizeTemplate;
            if ((int)AdjResizeTemplate.Value != value)
                AdjResizeTemplate.Value = value;
        }

        private void btnEnableValidator_Click(object sender, EventArgs e)
        {Propety.EnableValidator = btnEnableValidator.IsCLick;

        }

        private void btnEnableKeepFilter_Click(object sender, EventArgs e)
        {
            Propety.EnableKeepFilter = btnEnableKeepFilter.IsCLick;
        }

        private void btnAntiGlue_Click(object sender, EventArgs e)
        {
            Propety.EnableAngleRobustRefine = btnAntiGlue.IsCLick;
        }

        private void btnEnableOverLap_Click(object sender, EventArgs e)
        {
            Propety.EnableNms=btnEnableOverLap.IsCLick;
            layOverLap.Visible = Propety.EnableNms;
        }

        private void btnCPU_Click(object sender, EventArgs e)
        {
            Propety.UseCpu = true;
            Propety.UseGpu = false;
            btnCPU.IsCLick = true;
            btnGPU.IsCLick = false;
        }

        private void btnGPU_Click(object sender, EventArgs e)
        {
            Propety.UseGpu = true;
            Propety.UseCpu = false;
            btnGPU.IsCLick = true;
            btnCPU.IsCLick = false;
        }

        private void btnMultiThread_Click(object sender, EventArgs e)
        {
            Propety.EnableMultiThread = btnMultiThread.IsCLick;
            SyncThreadUi();
        }

        private void numThread_ValueChanged(float obj)
        {
            Propety.NumThreads = Math.Max(1, (int)numThread.Value);
            SyncThreadUi();
        }

        private void btnEnableColorCheck_Click(object sender, EventArgs e)
        {
            Propety.EnableColorCheck = btnEnableColorCheck.IsCLick;
            ApplyColorUiState();
        }

        private void btnColorHSV_Click(object sender, EventArgs e)
        {
            Propety.TypeColor = ColorGp.HSV;
            btnColorHSV.IsCLick = true;
            btnColorRGB.IsCLick = false;
            ClearMaskColors();
            ClearNgColors();
        }

        private void btnColorRGB_Click(object sender, EventArgs e)
        {
            Propety.TypeColor = ColorGp.RGB;
            btnColorRGB.IsCLick = true;
            btnColorHSV.IsCLick = false;
            ClearMaskColors();
            ClearNgColors();
        }

        private void btnGetColor_Click(object sender, EventArgs e)
        {
            btnGetColorNG.IsCLick = false;
            colorPickTarget = btnGetColor.IsCLick ? ColorPickTarget.Mask : ColorPickTarget.None;
            Propety.IsGetColor = btnGetColor.IsCLick;
            Global.IsGetColor = btnGetColor.IsCLick;
            Global.ColorGp = Propety.TypeColor;
            Global.StatusDraw = Propety.IsGetColor ? StatusDraw.Color : StatusDraw.Edit;
        }

        private void btnGetColorNG_Click(object sender, EventArgs e)
        {
            btnGetColor.IsCLick = false;
            colorPickTarget = btnGetColorNG.IsCLick ? ColorPickTarget.Ng : ColorPickTarget.None;
            Propety.IsGetColor = btnGetColorNG.IsCLick;
            Global.IsGetColor = btnGetColorNG.IsCLick;
            Global.ColorGp = Propety.TypeColor;
            Global.StatusDraw = Propety.IsGetColor ? StatusDraw.Color : StatusDraw.Edit;
        }

        private void trackColorExtraction_ValueChanged(float obj)
        {
            Propety.ExtractionMask = (int)trackColorExtraction.Value;
            Propety.SetMaskColor();
        }

        private void trackColorExtractionNG_ValueChanged(float obj)
        {
            Propety.ExtractionNG = (int)trackColorExtractionNG.Value;
            Propety.SetNgColor();
        }

        private void AdjColorScoreNG_ValueChanged(float obj)
        {
            Propety.ScoreNG = (int)AdjColorScoreNG.Value;
        }

        private void btnUndoColor_Click(object sender, EventArgs e)
        {
            if (Propety.listCLMaskShow == null || Propety.listCLMaskShow.Count == 0)
                return;

            Propety.listCLMaskShow.RemoveAt(Propety.listCLMaskShow.Count - 1);
            Propety.UndoMaskColor();
            Propety.SetMaskColor();
            picColorPreview.Invalidate();
        }

        private void btnClearColor_Click(object sender, EventArgs e)
        {
            ClearMaskColors();
        }

        private void btnUndoColorNG_Click(object sender, EventArgs e)
        {
            if (Propety.listCLNgShow == null || Propety.listCLNgShow.Count == 0)
                return;

            Propety.listCLNgShow.RemoveAt(Propety.listCLNgShow.Count - 1);
            Propety.UndoNgColor();
            Propety.SetNgColor();
            picColorNgPreview.Invalidate();
        }

        private void btnClearColorNG_Click(object sender, EventArgs e)
        {
            ClearNgColors();
        }

        private void ClearMaskColors()
        {
            Propety.ClearMaskColors();
            picColorPreview.Invalidate();
        }

        private void ClearNgColors()
        {
            Propety.ClearNgColors();
            picColorNgPreview.Invalidate();
        }

        private void picColorPreview_Paint(object sender, PaintEventArgs e)
        {
            if (Propety.listCLMaskShow == null || Propety.listCLMaskShow.Count == 0)
                return;

            int x = 0;
            int h = picColorPreview.Height;
            int w = h;
            foreach (Color cl in Propety.listCLMaskShow)
            {
                using (var brush = new SolidBrush(cl))
                    e.Graphics.FillRectangle(brush, new RectangleF(x, 0, w, h));
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, 0, w, h));
                x += w;
            }
        }

        private void picColorNgPreview_Paint(object sender, PaintEventArgs e)
        {
            if (Propety.listCLNgShow == null || Propety.listCLNgShow.Count == 0)
                return;

            int x = 0;
            int h = picColorNgPreview.Height;
            int w = h;
            foreach (Color cl in Propety.listCLNgShow)
            {
                using (var brush = new SolidBrush(cl))
                    e.Graphics.FillRectangle(brush, new RectangleF(x, 0, w, h));
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, 0, w, h));
                x += w;
            }
        }

        private void AdjClose_ValueChanged(float obj)
        {
            Propety.SizeClose = (int)AdjMorphology.Value;

        }

        private void AdjOffSetLeft_ValueChanged(float obj)
        {
            Propety.ColorNgOffsetLeft = (int)AdjOffSetLeft.Value;
        }

        private void AdjOffSetTop_ValueChanged(float obj)
        {
            Propety.ColorNgOffsetTop = (int)AdjOffSetTop.Value;
        }

        private void AdjOffSetBot_ValueChanged(float obj)
        {
            Propety.ColorNgOffsetBottom = (int)AdjOffSetBot.Value;
        }

        private void AdjOffSetRight_ValueChanged(float obj)
        {
            Propety.ColorNgOffsetRight = (int)AdjOffSetRight.Value;
        }

        #region Multi-Template UI (handlers only — controls + layout in Designer.cs)
        // Suppress event reentrancy khi RefreshTemplatesGrid programmatic populate cells.
        private bool _bindingMultiUi = false;
        private bool _syncingAreaLimitEditor = false;

        private void OnModeSingleClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            Propety.TemplateMode = PatternMode.Single;
            UpdateModeToggleUi();
        }

        private void OnModeMultiClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            Propety.TemplateMode = PatternMode.Multi;
            UpdateModeToggleUi();
        }

        private void OnModeMultiNoLimitClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            Propety.TemplateMode = PatternMode.MultiNoLimit;
            // MultiNoLimit không dùng per-label RotLimit → ép CheckByAreaLimit off để
            // dispatch trong DoWork đi đúng nhánh MatchBatchStable + ẩn rotLimit overlay.
            Propety.CheckByAreaLimit = false;
            Propety.MarkBatchDirty();
            UpdateModeToggleUi();
            PushRotLimitForCurrentRow();
        }

        private void UpdateModeToggleUi()
        {
            if (Propety == null) return;
            var method = Propety.Method;
            bool isSingleAny = (method == PatternMethod.SingleLabel
                                || method == PatternMethod.SingleLabelAreaLimit
                                || method == PatternMethod.SingleLabelNoLimit);
            bool isMultiAny = !isSingleAny;

            // Highlight 6 new method buttons (built in BuildMethodSelectorUi).
            HighlightMethodButton(method);

            // Sample area: Single → imgTemp; Multi → dgvTemplates.
            if (isMultiAny)
            {
                this.laySample.Controls.Remove(this.imgTemp);
                if (!this.laySample.Controls.Contains(this.dgvTemplates))
                    this.laySample.Controls.Add(this.dgvTemplates, 0, 0);
                btnAddSample.Visible = true;
                btnDeleteSample.Visible = true;
            }
            else
            {
                this.laySample.Controls.Remove(this.dgvTemplates);
                if (!this.laySample.Controls.Contains(this.imgTemp))
                    this.laySample.Controls.Add(this.imgTemp, 0, 0);
                btnAddSample.Visible = false;
                btnDeleteSample.Visible = false;
            }

            // tlpAreaToggle (Full/Crop button cũ) đã được thay thế bằng Method selector,
            // nhưng giữ control trong Designer cho compat — luôn ẩn.
            if (tlpAreaToggle != null) tlpAreaToggle.Visible = false;

            // Sub-templates tab: hiện ở mọi method. Refresh visibility/UI per method.
            RefreshSubTemplateUiForMethod();
            UpdateAreaToggleUi();
        }

        private void HighlightMethodButton(PatternMethod m)
        {
            if (btnMSingleLabel != null)         btnMSingleLabel.IsCLick         = (m == PatternMethod.SingleLabel);
            if (btnMSingleLabelAreaLimit != null) btnMSingleLabelAreaLimit.IsCLick = (m == PatternMethod.SingleLabelAreaLimit);
            if (btnMSingleLabelNoLimit != null)   btnMSingleLabelNoLimit.IsCLick   = (m == PatternMethod.SingleLabelNoLimit);
            if (btnMMultiLabel != null)          btnMMultiLabel.IsCLick          = (m == PatternMethod.MultiLabel);
            if (btnMMultiLabelAreaLimit != null)  btnMMultiLabelAreaLimit.IsCLick  = (m == PatternMethod.MultiLabelAreaLimit);
            if (btnMMultiLabelNoLimit != null)    btnMMultiLabelNoLimit.IsCLick    = (m == PatternMethod.MultiLabelNoLimit);
        }

        private void UpdateAreaToggleUi()
        {
            if (Propety == null || btnAreaFull == null || btnAreaCrop == null) return;
            btnAreaFull.IsCLick = !Propety.CheckByAreaLimit;
            btnAreaCrop.IsCLick = Propety.CheckByAreaLimit;
        }

        private void OnAreaFullClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            Propety.CheckByAreaLimit = false;
            UpdateAreaToggleUi();
            // Tắt edit rotLimit khỏi EditRectRot1.
            PushRotLimitForCurrentRow();
        }

        private void OnAreaCropClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            Propety.CheckByAreaLimit = true;
            UpdateAreaToggleUi();
            // Push rotLimit của row đang chọn vào EditRectRot1 để user vẽ.
            PushRotLimitForCurrentRow();
        }

        /// <summary>
        /// Khi user chọn row khác trong dgvTemplates, push entry.RotLimit hiện tại lên
        /// EditRectRot1 để user thấy/vẽ. Nếu RotLimit null + CheckByAreaLimit on → tạo default
        /// rotLimit nằm giữa ảnh để user kéo thả.
        /// </summary>
        private void OnDgvSelectionChanged(object s, EventArgs e)
        {
            PushRotLimitForCurrentRow();
            RefreshSubTemplateUiForMethod();
        }

        private void PushRotLimitForCurrentRow()
        {
            if (Propety == null || EditRectRot1 == null) return;
            int idx = dgvTemplates?.CurrentCell?.RowIndex ?? -1;
            if (Propety.TemplateMode != PatternMode.Multi || !Propety.CheckByAreaLimit
                || idx < 0 || idx >= Propety.MultiTemplates.Count)
            {
                // Không hiển thị rotLimit khi single-mode/Full-mode/chưa chọn row.
                var rotsNoLimit = new List<RectRotate> { Propety.rotArea, Propety.rotCrop, Propety.rotMask };
                EditRectRot1.Rot = rotsNoLimit;
                EditRectRot1.Refresh();
                return;
            }

            var entry = Propety.MultiTemplates[idx];
            if (entry.RotLimit == null)
            {
                // Default local to Area Check center; render/edit uses global conversion below.
                entry.RotLimit = new RectRotate(
                    new RectangleF(-100, -100, 200, 200),
                    new PointF(-(Propety.rotArea?._rect.X ?? 0),
                               -(Propety.rotArea?._rect.Y ?? 0)),
                    0,
                    AnchorPoint.None);
            }
            entry.RotLimit.Name = "Limit: " + (entry.Label ?? "");
            entry.RotLimit.TypeCrop = TypeCrop.Limit;

            var limitGlobal = Propety.AreaLocalToGlobal(entry.RotLimit);
            if (limitGlobal != null)
            {
                limitGlobal.Name = entry.RotLimit.Name;
                limitGlobal.TypeCrop = TypeCrop.Limit;
            }

            _syncingAreaLimitEditor = true;
            try
            {
                var rots = new List<RectRotate> { Propety.rotArea, Propety.rotCrop, Propety.rotMask, limitGlobal };
                EditRectRot1.Rot = rots;
                EditRectRot1.Refresh();
            }
            finally
            {
                _syncingAreaLimitEditor = false;
            }
        }

        /// <summary>
        /// Gọi từ btnLearning_Click khi user nhấn Sample. Trong multi-mode, sau khi
        /// LearnPattern xong và bmRaw được set, tự động push thành 1 entry mới trong list.
        /// </summary>
        private void TryAutoAddSampleToMulti()
        {
            if (Propety == null || !Propety.IsMultiTemplate || Propety.bmRaw == null) return;
            AddTemplateEntry(Propety.bmRaw);
        }

        private void OnDeleteTplClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            int idx = dgvTemplates.CurrentCell?.RowIndex ?? -1;
            if (idx < 0 || idx >= Propety.MultiTemplates.Count) return;
            Propety.MultiTemplates.RemoveAt(idx);
            Propety.MarkBatchDirty();
            RefreshTemplatesGrid();
        }

        private void AddTemplateEntry(Bitmap source, string suggestedLabel = null, bool captureCurrentAngle = true)
        {
            if (source == null || Propety == null) return;
            var entry = new BeeCore.Pattern2TemplateEntry
            {
                Label = suggestedLabel ?? ("Tpl" + (Propety.MultiTemplates.Count + 1)),
                ScoreThreshold = 70f,
                ExpectedCount = 1,
            };
            entry.SetBitmap(source);

            // Capture per-template angle range theo công thức single-mode:
            //   DeltaAngle = rotCrop._rectRotation - rotArea._rectRotation
            //   AngleLower/Upper = DeltaAngle ∓ Propety.Angle
            // Mỗi mẫu thường crop ở góc khác nhau → range expected khác. Native side sẽ
            // override cfg.AngleStart/EndDeg cho từng entry trong MatchBatchStable.
            if (captureCurrentAngle && Propety.rotCrop != null && Propety.rotArea != null)
            {
                float deltaAngle = Propety.rotCrop._rectRotation - Propety.rotArea._rectRotation;
                entry.HasAngleRange = true;
                entry.AngleLower = deltaAngle - Propety.Angle;
                entry.AngleUpper = deltaAngle + Propety.Angle;
            }

            Propety.MultiTemplates.Add(entry);
            Propety.MarkBatchDirty();
            RefreshTemplatesGrid();
        }

        private void OnDgvCellValueChanged(object s, DataGridViewCellEventArgs e)
        {
            if (_bindingMultiUi || Propety == null) return;
            if (e.RowIndex < 0 || e.RowIndex >= Propety.MultiTemplates.Count) return;
            var entry = Propety.MultiTemplates[e.RowIndex];
            var row = dgvTemplates.Rows[e.RowIndex];
            switch (e.ColumnIndex)
            {
                case 0: // Label
                    {
                        string oldLabel = entry.Label ?? "";
                        string newLabel = row.Cells["colLabel"].Value?.ToString() ?? "";
                        entry.Label = newLabel;
                        // MultiNoLimit: nhiều row chia sẻ cùng Label (group). Khi user đổi
                        // label trên 1 row, các row khác trùng oldLabel phải đổi theo để
                        // group không bị vỡ.
                        if (Propety.TemplateMode == PatternMode.MultiNoLimit
                            && oldLabel != newLabel)
                        {
                            for (int i = 0; i < Propety.MultiTemplates.Count; i++)
                            {
                                var other = Propety.MultiTemplates[i];
                                if (other == entry) continue;
                                if ((other.Label ?? "") == oldLabel)
                                    other.Label = newLabel;
                            }
                            RefreshTemplatesGrid();
                        }
                        break;
                    }
                case 2: // Score
                    entry.ScoreThreshold = ParseFloatSafe(row.Cells["colScore"].Value, 70f);
                    break;
                case 3: // Expected
                    entry.ExpectedCount = ParseIntSafe(row.Cells["colExpected"].Value, 1);
                    break;
            }
            Propety.MarkBatchDirty();
        }

        private static float ParseFloatSafe(object v, float fallback)
        {
            if (v == null) return fallback;
            if (float.TryParse(v.ToString(), out float r)) return r;
            return fallback;
        }

        private static int ParseIntSafe(object v, int fallback)
        {
            if (v == null) return fallback;
            if (int.TryParse(v.ToString(), out int r)) return r;
            return fallback;
        }

        private void RefreshTemplatesGrid()
        {
            if (Propety == null || dgvTemplates == null) return;
            _bindingMultiUi = true;
            try
            {
                dgvTemplates.Rows.Clear();
                if(Propety.MultiTemplates!=null)
                foreach (var entry in Propety.MultiTemplates)
                {
                    int r = dgvTemplates.Rows.Add();
                    var row = dgvTemplates.Rows[r];
                    row.Cells["colLabel"].Value = entry.Label;
                    row.Cells["colThumb"].Value = entry.GetBitmap();
                    row.Cells["colScore"].Value = entry.ScoreThreshold;
                    row.Cells["colExpected"].Value = entry.ExpectedCount;
                }
            }
            finally { _bindingMultiUi = false; }
        }

        /// <summary>Gọi từ LoadPara() để bind multi-template state khi mở tool.</summary>
        public void BindMultiTemplateUi()
        {
            if (Propety == null || btnModeSingle == null) return;
            _bindingMultiUi = true;
            try
            {
                RefreshTemplatesGrid();
            }
            finally { _bindingMultiUi = false; }
            UpdateModeToggleUi();
        }
        #endregion

        private void btn11_Click(object sender, EventArgs e)
        {
            lay11.Visible = !btn11.IsCLick;
        }

        private void btnChangeSample_Click(object sender, EventArgs e)
        {
            if (Propety.rotCrop == null) return;
            if (Propety.rotCrop._rect.Width == 0 || Propety.rotCrop._rect.Height == 0) return;

            // Learn template từ ROI hiện tại (chung cho single + multi).
            var newBmp = Propety.LearnPattern(BeeCore.Common.listCamera[Propety.IndexThread].matRaw.Clone(), false).ToBitmap();
            Propety.bmRaw = newBmp;
            imgTemp.Image = Propety.bmRaw;

            if (Propety.IsMultiTemplate)
            {
                // Multi-mode: thay template của row đang chọn trong dgvTemplates.
                // Nếu chưa có row nào hoặc chưa chọn → fallback Add mới (giữ workflow Sample).
                int idx = dgvTemplates?.CurrentCell?.RowIndex ?? -1;
                if (idx >= 0 && idx < Propety.MultiTemplates.Count)
                {
                    var entry = Propety.MultiTemplates[idx];
                    entry.SetBitmap(newBmp);
                    // Update angle range theo rotCrop mới (mẫu mới có thể khác góc).
                    if (Propety.rotCrop != null && Propety.rotArea != null)
                    {
                        float deltaAngle = Propety.rotCrop._rectRotation - Propety.rotArea._rectRotation;
                        entry.HasAngleRange = true;
                        entry.AngleLower = deltaAngle - Propety.Angle;
                        entry.AngleUpper = deltaAngle + Propety.Angle;
                    }
                    Propety.MarkBatchDirty();
                    RefreshTemplatesGrid();
                    if (idx < dgvTemplates.Rows.Count)
                        dgvTemplates.CurrentCell = dgvTemplates.Rows[idx].Cells[0];
                }
                else
                {
                    // Không có row chọn → thêm mới như Sample workflow.
                    TryAutoAddSampleToMulti();
                }
            }
        }

        // ================================================================
        // 6-Method selector (Q5: 6 RJButton trong TableLayoutPanel 2×3)
        // Thay thế lay11 cũ (3 button Single/Multi/MultiNoLimit) bằng grid mới.
        // ================================================================
        private TableLayoutPanel layMethod;
        private RJButton btnMSingleLabel, btnMSingleLabelAreaLimit, btnMSingleLabelNoLimit;
        private RJButton btnMMultiLabel, btnMMultiLabelAreaLimit, btnMMultiLabelNoLimit;

        private void BuildMethodSelectorUi()
        {
            // Hide lay11 cũ và parent (giữ trong Designer cho backward-compat — chỉ ẩn).
            if (lay11 != null)
            {
                lay11.Visible = false;
                var parent = lay11.Parent as TableLayoutPanel;
                if (parent != null)
                {
                    int row = parent.GetRow(lay11);
                    int col = parent.GetColumn(lay11);

                    layMethod = new TableLayoutPanel();
                    layMethod.BackColor = lay11.BackColor;
                    layMethod.Dock = DockStyle.Fill;
                    layMethod.Margin = lay11.Margin;
                    layMethod.Padding = new Padding(5);
                    layMethod.ColumnCount = 3;
                    layMethod.RowCount = 2;
                    for (int i = 0; i < 3; i++)
                        layMethod.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
                    for (int i = 0; i < 2; i++)
                        layMethod.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                    layMethod.Name = "layMethod";

                    btnMSingleLabel         = CreateMethodButton("Single Label",         OnMSingleLabelClicked);
                    btnMSingleLabelAreaLimit = CreateMethodButton("Single + AreaLimit",  OnMSingleLabelAreaLimitClicked);
                    btnMSingleLabelNoLimit   = CreateMethodButton("Single + NoLimit",    OnMSingleLabelNoLimitClicked);
                    btnMMultiLabel          = CreateMethodButton("Multi Label",          OnMMultiLabelClicked);
                    btnMMultiLabelAreaLimit  = CreateMethodButton("Multi + AreaLimit",   OnMMultiLabelAreaLimitClicked);
                    btnMMultiLabelNoLimit    = CreateMethodButton("Multi + NoLimit",     OnMMultiLabelNoLimitClicked);

                    layMethod.Controls.Add(btnMSingleLabel,         0, 0);
                    layMethod.Controls.Add(btnMSingleLabelAreaLimit, 1, 0);
                    layMethod.Controls.Add(btnMSingleLabelNoLimit,   2, 0);
                    layMethod.Controls.Add(btnMMultiLabel,          0, 1);
                    layMethod.Controls.Add(btnMMultiLabelAreaLimit,  1, 1);
                    layMethod.Controls.Add(btnMMultiLabelNoLimit,    2, 1);

                    // Cần 2 row chiều cao gấp đôi lay11 cũ.
                    parent.RowStyles[row] = new RowStyle(SizeType.Absolute, 100);
                    parent.Controls.Add(layMethod, col, row);
                }
            }
        }

        private RJButton CreateMethodButton(string text, EventHandler onClick)
        {
            var b = new RJButton();
            b.AutoFont = true;
            b.AutoFontHeightRatio = 0.65F;
            b.AutoFontMin = 6F;
            b.AutoFontMax = 14F;
            b.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            b.BackgroundColor = System.Drawing.Color.FromArgb(250, 250, 250);
            b.BorderColor = System.Drawing.Color.FromArgb(220, 220, 220);
            b.BorderRadius = 8;
            b.BorderSize = 1;
            b.ClickBotColor = System.Drawing.Color.FromArgb(247, 211, 139);
            b.ClickMidColor = System.Drawing.Color.FromArgb(246, 204, 120);
            b.ClickTopColor = System.Drawing.Color.FromArgb(244, 192, 89);
            b.Dock = DockStyle.Fill;
            b.FlatAppearance.BorderSize = 0;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            b.ForeColor = System.Drawing.Color.Black;
            b.TextColor = System.Drawing.Color.Black;
            b.Margin = new Padding(2);
            b.Text = text;
            b.UseVisualStyleBackColor = false;
            b.Click -= onClick;
            b.Click += onClick;
            return b;
        }

        private void OnMSingleLabelClicked(object s, EventArgs e)         { SetMethod(PatternMethod.SingleLabel); }
        private void OnMSingleLabelAreaLimitClicked(object s, EventArgs e) { SetMethod(PatternMethod.SingleLabelAreaLimit); }
        private void OnMSingleLabelNoLimitClicked(object s, EventArgs e)   { SetMethod(PatternMethod.SingleLabelNoLimit); }
        private void OnMMultiLabelClicked(object s, EventArgs e)          { SetMethod(PatternMethod.MultiLabel); }
        private void OnMMultiLabelAreaLimitClicked(object s, EventArgs e)  { SetMethod(PatternMethod.MultiLabelAreaLimit); }
        private void OnMMultiLabelNoLimitClicked(object s, EventArgs e)    { SetMethod(PatternMethod.MultiLabelNoLimit); }

        private void SetMethod(PatternMethod m)
        {
            if (Propety == null) return;
            Propety.Method = m;
            UpdateModeToggleUi();
            PushRotLimitForCurrentRow();
        }

        // ================================================================
        // Sub-Templates tab (Q6: tab riêng trong tabControl2)
        // ================================================================
        private TabPage tabSubTemplates;
        private DataGridView dgvSubTemplates;
        private RJButton btnSubAdd, btnSubReplace, btnSubDelete;
        private NumericUpDown numSubPad;
        private Label lblSubContext;

        private void BuildSubTemplateTabUi()
        {
            if (tabControl2 == null) return;

            tabSubTemplates = new TabPage("Sub-Templates");
            tabSubTemplates.Name = "tabSubTemplates";
            tabSubTemplates.BackColor = System.Drawing.Color.White;

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 3;
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36)); // context label
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // grid
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // toolbar

            lblSubContext = new Label();
            lblSubContext.Dock = DockStyle.Fill;
            lblSubContext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            lblSubContext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblSubContext.Padding = new Padding(8, 0, 0, 0);
            lblSubContext.Text = "Sub-templates for: (none)";

            dgvSubTemplates = new DataGridView();
            dgvSubTemplates.Dock = DockStyle.Fill;
            dgvSubTemplates.AllowUserToAddRows = false;
            dgvSubTemplates.AllowUserToDeleteRows = false;
            dgvSubTemplates.RowHeadersVisible = false;
            dgvSubTemplates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSubTemplates.RowTemplate.Height = 60;
            dgvSubTemplates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSubTemplates.Columns.Add(new DataGridViewImageColumn { Name = "colSubImg", HeaderText = "Image", ImageLayout = DataGridViewImageCellLayout.Zoom, FillWeight = 25 });
            dgvSubTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubLabel", HeaderText = "Label", FillWeight = 30 });
            dgvSubTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubScore", HeaderText = "Score", FillWeight = 15 });
            dgvSubTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubAngleLow", HeaderText = "AngLow", FillWeight = 15 });
            dgvSubTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSubAngleUp", HeaderText = "AngUp", FillWeight = 15 });
            dgvSubTemplates.CellValueChanged -= OnDgvSubCellValueChanged;
            dgvSubTemplates.CellValueChanged += OnDgvSubCellValueChanged;

            var toolbar = new TableLayoutPanel();
            toolbar.Dock = DockStyle.Fill;
            toolbar.ColumnCount = 4;
            toolbar.RowCount = 1;
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            btnSubAdd = CreateMethodButton("+ Add Sub", OnSubAddClicked);
            btnSubReplace = CreateMethodButton("⟳ Replace", OnSubReplaceClicked);
            btnSubDelete = CreateMethodButton("- Delete", OnSubDeleteClicked);

            var padPanel = new TableLayoutPanel();
            padPanel.Dock = DockStyle.Fill;
            padPanel.ColumnCount = 2;
            padPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            padPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            var lblPad = new Label { Text = "Pad px", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleRight };
            numSubPad = new NumericUpDown { Minimum = 0, Maximum = 500, Value = 20, Dock = DockStyle.Fill };
            numSubPad.ValueChanged -= OnSubPadChanged;
            numSubPad.ValueChanged += OnSubPadChanged;
            padPanel.Controls.Add(lblPad, 0, 0);
            padPanel.Controls.Add(numSubPad, 1, 0);

            toolbar.Controls.Add(btnSubAdd, 0, 0);
            toolbar.Controls.Add(btnSubReplace, 1, 0);
            toolbar.Controls.Add(btnSubDelete, 2, 0);
            toolbar.Controls.Add(padPanel, 3, 0);

            root.Controls.Add(lblSubContext, 0, 0);
            root.Controls.Add(dgvSubTemplates, 0, 1);
            root.Controls.Add(toolbar, 0, 2);

            tabSubTemplates.Controls.Add(root);
            tabControl2.TabPages.Add(tabSubTemplates);
        }

        /// <summary>
        /// Lấy list sub-template hiện tại theo method + selection. Multi → entry đang chọn.
        /// Single → SubTemplatesSingle.
        /// </summary>
        private List<SubTemplateEntry> GetActiveSubList(out string context)
        {
            context = "(none)";
            if (Propety == null) return null;
            var method = Propety.Method;
            if (method == PatternMethod.SingleLabel
                || method == PatternMethod.SingleLabelAreaLimit
                || method == PatternMethod.SingleLabelNoLimit)
            {
                context = "Single template";
                return Propety.SubTemplatesSingle;
            }
            int idx = dgvTemplates?.CurrentCell?.RowIndex ?? -1;
            if (idx < 0 || idx >= Propety.MultiTemplates.Count) return null;
            var entry = Propety.MultiTemplates[idx];
            context = "Main label: " + (entry.Label ?? "");
            return entry.SubTemplates;
        }

        private int GetActiveSubPadPx()
        {
            if (Propety == null) return 20;
            var method = Propety.Method;
            if (method == PatternMethod.SingleLabel
                || method == PatternMethod.SingleLabelAreaLimit
                || method == PatternMethod.SingleLabelNoLimit)
                return Propety.SubSearchPadPxSingle;
            int idx = dgvTemplates?.CurrentCell?.RowIndex ?? -1;
            if (idx < 0 || idx >= Propety.MultiTemplates.Count) return 20;
            return Propety.MultiTemplates[idx].SubSearchPadPx;
        }

        private void SetActiveSubPadPx(int value)
        {
            if (Propety == null) return;
            var method = Propety.Method;
            if (method == PatternMethod.SingleLabel
                || method == PatternMethod.SingleLabelAreaLimit
                || method == PatternMethod.SingleLabelNoLimit)
            { Propety.SubSearchPadPxSingle = value; return; }
            int idx = dgvTemplates?.CurrentCell?.RowIndex ?? -1;
            if (idx < 0 || idx >= Propety.MultiTemplates.Count) return;
            Propety.MultiTemplates[idx].SubSearchPadPx = value;
        }

        private void RefreshSubTemplateUiForMethod()
        {
            if (dgvSubTemplates == null) return;
            string ctx;
            var list = GetActiveSubList(out ctx);
            if (lblSubContext != null) lblSubContext.Text = "Sub-templates for: " + ctx;

            dgvSubTemplates.Rows.Clear();
            if (list != null)
            {
                foreach (var sub in list)
                {
                    var bmp = sub.GetBitmap();
                    int rowIdx = dgvSubTemplates.Rows.Add();
                    var row = dgvSubTemplates.Rows[rowIdx];
                    row.Cells["colSubImg"].Value = bmp;
                    row.Cells["colSubLabel"].Value = sub.Label;
                    row.Cells["colSubScore"].Value = sub.ScoreThreshold;
                    row.Cells["colSubAngleLow"].Value = sub.AngleLower;
                    row.Cells["colSubAngleUp"].Value = sub.AngleUpper;
                }
            }
            if (numSubPad != null)
            {
                int pad = GetActiveSubPadPx();
                if (pad < (int)numSubPad.Minimum) pad = (int)numSubPad.Minimum;
                if (pad > (int)numSubPad.Maximum) pad = (int)numSubPad.Maximum;
                if ((int)numSubPad.Value != pad) numSubPad.Value = pad;
            }
        }

        private void OnSubAddClicked(object s, EventArgs e)
        {
            string ctx;
            var list = GetActiveSubList(out ctx);
            if (list == null) { MessageBox.Show("Select a main template row first."); return; }
            var bmp = CropSubFromCurrentRotCrop();
            if (bmp == null) return;
            try
            {
                var sub = new SubTemplateEntry
                {
                    Label = "sub" + (list.Count + 1),
                    ScoreThreshold = 70f,
                };
                sub.SetBitmap(bmp);
                list.Add(sub);
                RefreshSubTemplateUiForMethod();
            }
            finally { bmp.Dispose(); }
        }

        /// <summary>
        /// Crop bitmap từ raw bằng rotCrop hiện tại. Trả null + show message nếu invalid.
        /// Shared bởi OnSubAddClicked + OnSubReplaceClicked.
        /// </summary>
        private Bitmap CropSubFromCurrentRotCrop()
        {
            if (Propety == null) return null;
            var rotCrop = Propety.rotCrop;
            if (rotCrop == null || rotCrop._rect.Width <= 0 || rotCrop._rect.Height <= 0)
            {
                MessageBox.Show("Hãy kéo box rotCrop trên ảnh trước.");
                return null;
            }
            try
            {
                using (Mat raw = BeeCore.Common.listCamera[Propety.IndexCCD].matRaw.Clone())
                using (Mat crop = Cropper.CropRotatedRect(raw, rotCrop, null))
                {
                    if (crop == null || crop.Empty())
                    {
                        MessageBox.Show("Crop trả về empty — kiểm tra rotCrop có nằm trong ảnh không.");
                        return null;
                    }
                    return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(crop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Crop failed: " + ex.Message);
                return null;
            }
        }

        private void OnSubReplaceClicked(object s, EventArgs e)
        {
            string ctx;
            var list = GetActiveSubList(out ctx);
            if (list == null) { MessageBox.Show("Select a main template row first."); return; }
            int idx = dgvSubTemplates?.CurrentCell?.RowIndex ?? -1;
            if (idx < 0 || idx >= list.Count)
            {
                MessageBox.Show("Chọn 1 row sub-template để replace.");
                return;
            }
            var bmp = CropSubFromCurrentRotCrop();
            if (bmp == null) return;
            try
            {
                var sub = list[idx];
                sub.SetBitmap(bmp); // SetBitmap đã invalidate _learned + _cachedBitmap
                RefreshSubTemplateUiForMethod();
                if (idx < dgvSubTemplates.Rows.Count)
                    dgvSubTemplates.CurrentCell = dgvSubTemplates.Rows[idx].Cells[1];
            }
            finally { bmp.Dispose(); }
        }

        private void OnSubDeleteClicked(object s, EventArgs e)
        {
            string ctx;
            var list = GetActiveSubList(out ctx);
            if (list == null) return;
            int idx = dgvSubTemplates?.CurrentCell?.RowIndex ?? -1;
            if (idx < 0 || idx >= list.Count) return;
            list[idx].InvalidateLearned();
            list.RemoveAt(idx);
            RefreshSubTemplateUiForMethod();
        }

        private void OnSubPadChanged(object s, EventArgs e)
        {
            SetActiveSubPadPx((int)numSubPad.Value);
        }

        private void OnDgvSubCellValueChanged(object s, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string ctx;
            var list = GetActiveSubList(out ctx);
            if (list == null || e.RowIndex >= list.Count) return;
            var sub = list[e.RowIndex];
            var row = dgvSubTemplates.Rows[e.RowIndex];
            switch (e.ColumnIndex)
            {
                case 1: sub.Label = row.Cells["colSubLabel"].Value?.ToString() ?? ""; break;
                case 2: sub.ScoreThreshold = ParseFloatSafe(row.Cells["colSubScore"].Value, 70f); break;
                case 3:
                    sub.AngleLower = ParseFloatSafe(row.Cells["colSubAngleLow"].Value, -180f);
                    sub.HasAngleRange = (sub.AngleLower != -180.0 || sub.AngleUpper != 180.0);
                    break;
                case 4:
                    sub.AngleUpper = ParseFloatSafe(row.Cells["colSubAngleUp"].Value, 180f);
                    sub.HasAngleRange = (sub.AngleLower != -180.0 || sub.AngleUpper != 180.0);
                    break;
            }
        }
    }
}
