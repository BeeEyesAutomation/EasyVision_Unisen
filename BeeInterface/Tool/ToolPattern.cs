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
            BuildColorUi();
            BuildMultiTemplateUi();

            if (Propety == null)
                Propety = new Patterns();
        }
        

        public void LoadPara()
        {
            EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotCrop, Propety.rotMask };
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
            btnEnScale.IsCLick= Propety.EnableScaleSearch  ;
            btnEnScale.Text = Propety.EnableScaleSearch == true ? "ON" : "OFF";
            numAdjScale.Enabled = Propety.EnableScaleSearch;
            numAdjScale.Value = Propety.ScalePattern;
            numAdjStepScale.Value = Propety.ScaleStep;
            btnEnableKeepFilter.IsCLick = Propety.EnableKeepFilter;
            btnEnableOverLap.IsCLick = Propety.EnableNms;
            btnEnableValidator.IsCLick = Propety.EnableValidator;
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

        private void ToolPattern_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (Global.IsRun) return;
            if (OwnerTool.StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
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
            lay5.Visible =! btn5.IsCLick;
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
            btnEnScale.Text = Propety.EnableScaleSearch == true ? "ON" : "OFF";
            numAdjScale.Enabled = Propety.EnableScaleSearch;

        }

        private void numAdjScale_ValueChanged(float obj)
        {
            Propety.ScalePattern =(int) numAdjScale.Value;
        }

        private void numAdjStepScale_ValueChanged(float obj)
        {
            Propety.ScaleStep = (int)numAdjStepScale.Value;
        }

        private void btnEnableValidator_Click(object sender, EventArgs e)
        {Propety.EnableValidator = btnEnableValidator.IsCLick;

        }

        private void btnEnableKeepFilter_Click(object sender, EventArgs e)
        {
            Propety.EnableKeepFilter = btnEnableKeepFilter.IsCLick;
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

        #region Multi-Template UI
        // Section quản lý list template + label cho multi-template mode.
        // Tạo programmatic (thay vì sửa 4643-line Designer.cs) để cô lập rủi ro.
        // Layout theo memory feedback_collapsible_param_sections: RJButton header IsTouch=true
        // toggle Visible của TableLayoutPanel con. Event subscribe -=/+= theo CLAUDE.md 0.1.4.

        private RJButton secMultiHeader;
        private TableLayoutPanel tlpMulti;
        private CheckBox chkMultiTemplate;
        private DataGridView dgvTemplates;
        private Button btnAddTpl;
        private Button btnAddTplFromFile;
        private Button btnRemoveTpl;
        private Button btnTplUp;
        private Button btnTplDown;
        private bool _bindingMultiUi = false; // suppress event reentrancy khi RefreshTemplatesGrid

        private void BuildMultiTemplateUi()
        {
            // Header
            secMultiHeader = new RJButton();
            secMultiHeader.Text = "▶ Multi-Template";
            secMultiHeader.IsTouch = true;
            secMultiHeader.Dock = DockStyle.Fill;
            secMultiHeader.Height = 28;
            secMultiHeader.Click -= OnSecMultiClicked;
            secMultiHeader.Click += OnSecMultiClicked;

            // Container collapsible
            tlpMulti = new TableLayoutPanel();
            tlpMulti.Dock = DockStyle.Fill;
            tlpMulti.AutoSize = true;
            tlpMulti.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpMulti.Visible = false; // mặc định collapsed
            tlpMulti.ColumnCount = 1;
            tlpMulti.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            chkMultiTemplate = new CheckBox();
            chkMultiTemplate.Text = "Enable multi-template mode";
            chkMultiTemplate.AutoSize = true;
            chkMultiTemplate.Margin = new Padding(3, 4, 3, 4);
            chkMultiTemplate.CheckedChanged -= OnChkMultiTemplateChanged;
            chkMultiTemplate.CheckedChanged += OnChkMultiTemplateChanged;

            // Button row
            var pnlBtns = new FlowLayoutPanel();
            pnlBtns.Dock = DockStyle.Top;
            pnlBtns.AutoSize = true;
            pnlBtns.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlBtns.WrapContents = true;
            pnlBtns.Margin = new Padding(0);

            btnAddTpl = new Button { Text = "Add (last learn)", AutoSize = true };
            btnAddTpl.Click -= OnAddTplClicked; btnAddTpl.Click += OnAddTplClicked;
            btnAddTplFromFile = new Button { Text = "Add from file...", AutoSize = true };
            btnAddTplFromFile.Click -= OnAddTplFromFileClicked; btnAddTplFromFile.Click += OnAddTplFromFileClicked;
            btnRemoveTpl = new Button { Text = "Remove", AutoSize = true };
            btnRemoveTpl.Click -= OnRemoveTplClicked; btnRemoveTpl.Click += OnRemoveTplClicked;
            btnTplUp = new Button { Text = "↑", AutoSize = true, Width = 32 };
            btnTplUp.Click -= OnTplUpClicked; btnTplUp.Click += OnTplUpClicked;
            btnTplDown = new Button { Text = "↓", AutoSize = true, Width = 32 };
            btnTplDown.Click -= OnTplDownClicked; btnTplDown.Click += OnTplDownClicked;

            pnlBtns.Controls.Add(btnAddTpl);
            pnlBtns.Controls.Add(btnAddTplFromFile);
            pnlBtns.Controls.Add(btnRemoveTpl);
            pnlBtns.Controls.Add(btnTplUp);
            pnlBtns.Controls.Add(btnTplDown);

            // Grid
            dgvTemplates = new DataGridView();
            dgvTemplates.Dock = DockStyle.Top;
            dgvTemplates.Height = 220;
            dgvTemplates.AllowUserToAddRows = false;
            dgvTemplates.AllowUserToResizeRows = false;
            dgvTemplates.AutoGenerateColumns = false;
            dgvTemplates.RowTemplate.Height = 60;
            dgvTemplates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTemplates.MultiSelect = false;
            dgvTemplates.Columns.Clear();
            dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLabel", HeaderText = "Label", Width = 90 });
            dgvTemplates.Columns.Add(new DataGridViewImageColumn { Name = "colThumb", HeaderText = "Thumb", Width = 70, ImageLayout = DataGridViewImageCellLayout.Zoom });
            dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colScore", HeaderText = "Score≥", Width = 55 });
            dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colExpected", HeaderText = "Expect", Width = 55 });
            dgvTemplates.CellValueChanged -= OnDgvCellValueChanged;
            dgvTemplates.CellValueChanged += OnDgvCellValueChanged;

            tlpMulti.Controls.Add(chkMultiTemplate, 0, 0);
            tlpMulti.Controls.Add(pnlBtns, 0, 1);
            tlpMulti.Controls.Add(dgvTemplates, 0, 2);

            // Append xuống cuối tableLayoutPanel1. TLP có RowCount cố định = 24, dùng vị trí 24-25.
            // TLP cho phép Add controls quá RowCount; auto-grow nếu có thêm RowStyle.
            try
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 2;
                tableLayoutPanel1.Controls.Add(secMultiHeader, 0, tableLayoutPanel1.RowCount - 2);
                tableLayoutPanel1.Controls.Add(tlpMulti, 0, tableLayoutPanel1.RowCount - 1);
            }
            catch (Exception ex)
            {
                // Nếu tableLayoutPanel1 không có (sau refactor designer), log nhẹ và bỏ qua.
                System.Diagnostics.Debug.WriteLine("BuildMultiTemplateUi: " + ex.Message);
            }
        }

        private void OnSecMultiClicked(object s, EventArgs e)
        {
            tlpMulti.Visible = !tlpMulti.Visible;
            secMultiHeader.Text = (tlpMulti.Visible ? "▼ " : "▶ ") + "Multi-Template";
        }

        private void OnChkMultiTemplateChanged(object s, EventArgs e)
        {
            if (_bindingMultiUi || Propety == null) return;
            Propety.IsMultiTemplate = chkMultiTemplate.Checked;
            UpdateMultiUiEnabled();
        }

        private void UpdateMultiUiEnabled()
        {
            bool on = Propety != null && Propety.IsMultiTemplate;
            dgvTemplates.Enabled = on;
            btnAddTpl.Enabled = on;
            btnAddTplFromFile.Enabled = on;
            btnRemoveTpl.Enabled = on;
            btnTplUp.Enabled = on;
            btnTplDown.Enabled = on;
        }

        private void OnAddTplClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            if (Propety.bmRaw == null)
            {
                MessageBox.Show("Chưa có template — học (Learn) ở single-mode trước rồi quay lại Add.",
                    "Multi-Template", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            AddTemplateEntry(Propety.bmRaw);
        }

        private void OnAddTplFromFileClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image files|*.png;*.bmp;*.jpg;*.jpeg|All files|*.*";
                ofd.Multiselect = true;
                if (ofd.ShowDialog() != DialogResult.OK) return;
                foreach (var path in ofd.FileNames)
                {
                    try
                    {
                        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        using (var bmp = new Bitmap(fs))
                        {
                            AddTemplateEntry(bmp, System.IO.Path.GetFileNameWithoutExtension(path));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi đọc " + path + ":\n" + ex.Message, "Multi-Template",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AddTemplateEntry(Bitmap source, string suggestedLabel = null)
        {
            if (source == null || Propety == null) return;
            var entry = new BeeCore.Pattern2TemplateEntry
            {
                Label = suggestedLabel ?? ("Tpl" + (Propety.MultiTemplates.Count + 1)),
                ScoreThreshold = 70f,
                ExpectedCount = 1,
            };
            entry.SetBitmap(source);
            Propety.MultiTemplates.Add(entry);
            RefreshTemplatesGrid();
        }

        private void OnRemoveTplClicked(object s, EventArgs e)
        {
            if (Propety == null) return;
            int idx = dgvTemplates.CurrentCell?.RowIndex ?? -1;
            if (idx < 0 || idx >= Propety.MultiTemplates.Count) return;
            Propety.MultiTemplates.RemoveAt(idx);
            RefreshTemplatesGrid();
        }

        private void OnTplUpClicked(object s, EventArgs e) { MoveTpl(-1); }
        private void OnTplDownClicked(object s, EventArgs e) { MoveTpl(+1); }
        private void MoveTpl(int delta)
        {
            if (Propety == null) return;
            int idx = dgvTemplates.CurrentCell?.RowIndex ?? -1;
            int newIdx = idx + delta;
            if (idx < 0 || newIdx < 0 || newIdx >= Propety.MultiTemplates.Count) return;
            var item = Propety.MultiTemplates[idx];
            Propety.MultiTemplates.RemoveAt(idx);
            Propety.MultiTemplates.Insert(newIdx, item);
            RefreshTemplatesGrid();
            if (newIdx < dgvTemplates.Rows.Count)
                dgvTemplates.CurrentCell = dgvTemplates.Rows[newIdx].Cells[0];
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
                    entry.Label = row.Cells["colLabel"].Value?.ToString() ?? "";
                    break;
                case 2: // Score
                    entry.ScoreThreshold = ParseFloatSafe(row.Cells["colScore"].Value, 70f);
                    break;
                case 3: // Expected
                    entry.ExpectedCount = ParseIntSafe(row.Cells["colExpected"].Value, 1);
                    break;
            }
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
            if (Propety == null || chkMultiTemplate == null) return;
            _bindingMultiUi = true;
            try
            {
                chkMultiTemplate.Checked = Propety.IsMultiTemplate;
                RefreshTemplatesGrid();
            }
            finally { _bindingMultiUi = false; }
            UpdateMultiUiEnabled();
        }
        #endregion
    }
}
