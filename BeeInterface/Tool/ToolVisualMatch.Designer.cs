using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolVisualMatch
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolVisualMatch));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.AdjMaxDiffPixels = new BeeInterface.AdjustBarEx();
            this.AdjColorTolerance = new BeeInterface.AdjustBarEx();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lay3 = new System.Windows.Forms.TableLayoutPanel();
            this.imgTemp = new System.Windows.Forms.PictureBox();
            this.btnLearning = new BeeInterface.RJButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClear = new BeeInterface.RJButton();
            this.btnCropRect = new BeeInterface.RJButton();
            this.btnCropArea = new BeeInterface.RJButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.btnNone = new BeeInterface.RJButton();
            this.btnElip = new BeeInterface.RJButton();
            this.btnRect = new BeeInterface.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackNumObject = new BeeInterface.AdjustBarEx();
            this.trackMaxOverLap = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.numLimitCounter = new BeeInterface.AdjustBarEx();
            this.layLimitCouter = new System.Windows.Forms.TableLayoutPanel();
            this.btnMore = new BeeInterface.RJButton();
            this.btnEqual = new BeeInterface.RJButton();
            this.btnLess = new BeeInterface.RJButton();
            this.workLoadModel = new System.ComponentModel.BackgroundWorker();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.pInspect = new System.Windows.Forms.Panel();
            this.btnTest = new BeeInterface.RJButton();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.lay3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.layLimitCouter.SuspendLayout();
            this.pInspect.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabP1);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(488, 787);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(480, 749);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.AdjMaxDiffPixels, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.AdjColorTolerance, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.lay3, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tableLayoutPanel1.RowCount = 16;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(474, 743);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // AdjMaxDiffPixels
            // 
            this.AdjMaxDiffPixels.AutoRepeatAccelDeltaMs = -5;
            this.AdjMaxDiffPixels.AutoRepeatAccelerate = true;
            this.AdjMaxDiffPixels.AutoRepeatEnabled = true;
            this.AdjMaxDiffPixels.AutoRepeatInitialDelay = 400;
            this.AdjMaxDiffPixels.AutoRepeatInterval = 60;
            this.AdjMaxDiffPixels.AutoRepeatMinInterval = 20;
            this.AdjMaxDiffPixels.AutoShowTextbox = true;
            this.AdjMaxDiffPixels.AutoSizeTextbox = true;
            this.AdjMaxDiffPixels.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.AdjMaxDiffPixels.BarLeftGap = 20;
            this.AdjMaxDiffPixels.BarRightGap = 6;
            this.AdjMaxDiffPixels.ChromeGap = 8;
            this.AdjMaxDiffPixels.ChromeWidthRatio = 0.14F;
            this.AdjMaxDiffPixels.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjMaxDiffPixels.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjMaxDiffPixels.ColorScale = System.Drawing.Color.LightGray;
            this.AdjMaxDiffPixels.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjMaxDiffPixels.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjMaxDiffPixels.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjMaxDiffPixels.Decimals = 0;
            this.AdjMaxDiffPixels.DisabledDesaturateMix = 0.3F;
            this.AdjMaxDiffPixels.DisabledDimFactor = 0.9F;
            this.AdjMaxDiffPixels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjMaxDiffPixels.EdgePadding = 2;
            this.AdjMaxDiffPixels.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjMaxDiffPixels.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjMaxDiffPixels.KeyboardStep = 1F;
            this.AdjMaxDiffPixels.Location = new System.Drawing.Point(6, 299);
            this.AdjMaxDiffPixels.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjMaxDiffPixels.MatchTextboxFontToThumb = true;
            this.AdjMaxDiffPixels.Max = 100000F;
            this.AdjMaxDiffPixels.MaxTextboxWidth = 0;
            this.AdjMaxDiffPixels.MaxThumb = 1000;
            this.AdjMaxDiffPixels.MaxTrackHeight = 1000;
            this.AdjMaxDiffPixels.Min = 1F;
            this.AdjMaxDiffPixels.MinChromeWidth = 64;
            this.AdjMaxDiffPixels.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjMaxDiffPixels.MinTextboxWidth = 32;
            this.AdjMaxDiffPixels.MinThumb = 30;
            this.AdjMaxDiffPixels.MinTrackHeight = 8;
            this.AdjMaxDiffPixels.Name = "AdjMaxDiffPixels";
            this.AdjMaxDiffPixels.Radius = 8;
            this.AdjMaxDiffPixels.ShowValueOnThumb = true;
            this.AdjMaxDiffPixels.Size = new System.Drawing.Size(446, 50);
            this.AdjMaxDiffPixels.SnapToStep = true;
            this.AdjMaxDiffPixels.StartWithTextboxHidden = true;
            this.AdjMaxDiffPixels.Step = 1F;
            this.AdjMaxDiffPixels.TabIndex = 79;
            this.AdjMaxDiffPixels.TextboxFontSize = 20F;
            this.AdjMaxDiffPixels.TextboxSidePadding = 10;
            this.AdjMaxDiffPixels.TextboxWidth = 600;
            this.AdjMaxDiffPixels.ThumbDiameterRatio = 2F;
            this.AdjMaxDiffPixels.ThumbValueBold = true;
            this.AdjMaxDiffPixels.ThumbValueFontScale = 1.5F;
            this.AdjMaxDiffPixels.ThumbValuePadding = 0;
            this.AdjMaxDiffPixels.TightEdges = true;
            this.AdjMaxDiffPixels.TrackHeightRatio = 0.45F;
            this.AdjMaxDiffPixels.TrackWidthRatio = 1F;
            this.AdjMaxDiffPixels.UnitText = "";
            this.AdjMaxDiffPixels.Value = 1F;
            this.AdjMaxDiffPixels.WheelStep = 1F;
            this.AdjMaxDiffPixels.ValueChanged += new System.Action<float>(this.AdjMaxDiffPixels_ValueChanged);
            // 
            // AdjColorTolerance
            // 
            this.AdjColorTolerance.AutoRepeatAccelDeltaMs = -5;
            this.AdjColorTolerance.AutoRepeatAccelerate = true;
            this.AdjColorTolerance.AutoRepeatEnabled = true;
            this.AdjColorTolerance.AutoRepeatInitialDelay = 400;
            this.AdjColorTolerance.AutoRepeatInterval = 60;
            this.AdjColorTolerance.AutoRepeatMinInterval = 20;
            this.AdjColorTolerance.AutoShowTextbox = true;
            this.AdjColorTolerance.AutoSizeTextbox = true;
            this.AdjColorTolerance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.AdjColorTolerance.BarLeftGap = 20;
            this.AdjColorTolerance.BarRightGap = 6;
            this.AdjColorTolerance.ChromeGap = 8;
            this.AdjColorTolerance.ChromeWidthRatio = 0.14F;
            this.AdjColorTolerance.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjColorTolerance.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjColorTolerance.ColorScale = System.Drawing.Color.LightGray;
            this.AdjColorTolerance.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjColorTolerance.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjColorTolerance.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjColorTolerance.Decimals = 0;
            this.AdjColorTolerance.DisabledDesaturateMix = 0.3F;
            this.AdjColorTolerance.DisabledDimFactor = 0.9F;
            this.AdjColorTolerance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjColorTolerance.EdgePadding = 2;
            this.AdjColorTolerance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjColorTolerance.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjColorTolerance.KeyboardStep = 1F;
            this.AdjColorTolerance.Location = new System.Drawing.Point(6, 401);
            this.AdjColorTolerance.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjColorTolerance.MatchTextboxFontToThumb = true;
            this.AdjColorTolerance.Max = 255F;
            this.AdjColorTolerance.MaxTextboxWidth = 1;
            this.AdjColorTolerance.MaxThumb = 1000;
            this.AdjColorTolerance.MaxTrackHeight = 1000;
            this.AdjColorTolerance.Min = 0F;
            this.AdjColorTolerance.MinChromeWidth = 64;
            this.AdjColorTolerance.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjColorTolerance.MinTextboxWidth = 32;
            this.AdjColorTolerance.MinThumb = 30;
            this.AdjColorTolerance.MinTrackHeight = 8;
            this.AdjColorTolerance.Name = "AdjColorTolerance";
            this.AdjColorTolerance.Radius = 8;
            this.AdjColorTolerance.ShowValueOnThumb = true;
            this.AdjColorTolerance.Size = new System.Drawing.Size(446, 50);
            this.AdjColorTolerance.SnapToStep = true;
            this.AdjColorTolerance.StartWithTextboxHidden = true;
            this.AdjColorTolerance.Step = 1F;
            this.AdjColorTolerance.TabIndex = 78;
            this.AdjColorTolerance.TextboxFontSize = 20F;
            this.AdjColorTolerance.TextboxSidePadding = 10;
            this.AdjColorTolerance.TextboxWidth = 600;
            this.AdjColorTolerance.ThumbDiameterRatio = 2F;
            this.AdjColorTolerance.ThumbValueBold = true;
            this.AdjColorTolerance.ThumbValueFontScale = 1.5F;
            this.AdjColorTolerance.ThumbValuePadding = 0;
            this.AdjColorTolerance.TightEdges = true;
            this.AdjColorTolerance.TrackHeightRatio = 0.45F;
            this.AdjColorTolerance.TrackWidthRatio = 1F;
            this.AdjColorTolerance.UnitText = "";
            this.AdjColorTolerance.Value = 0F;
            this.AdjColorTolerance.WheelStep = 1F;
            this.AdjColorTolerance.ValueChanged += new System.Action<float>(this.AdjColorTolerance_ValueChanged);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(6, 369);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(446, 32);
            this.label10.TabIndex = 77;
            this.label10.Text = "Threshold Color";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(6, 723);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(446, 32);
            this.label8.TabIndex = 76;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lay3
            // 
            this.lay3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.lay3.ColumnCount = 2;
            this.lay3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.31535F));
            this.lay3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.68465F));
            this.lay3.Controls.Add(this.imgTemp, 0, 0);
            this.lay3.Controls.Add(this.btnLearning, 1, 0);
            this.lay3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay3.Location = new System.Drawing.Point(6, 503);
            this.lay3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lay3.Name = "lay3";
            this.lay3.Padding = new System.Windows.Forms.Padding(5);
            this.lay3.RowCount = 1;
            this.lay3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay3.Size = new System.Drawing.Size(446, 200);
            this.lay3.TabIndex = 75;
            // 
            // imgTemp
            // 
            this.imgTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.imgTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgTemp.Location = new System.Drawing.Point(10, 10);
            this.imgTemp.Margin = new System.Windows.Forms.Padding(5);
            this.imgTemp.Name = "imgTemp";
            this.imgTemp.Size = new System.Drawing.Size(270, 180);
            this.imgTemp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgTemp.TabIndex = 31;
            this.imgTemp.TabStop = false;
            // 
            // btnLearning
            // 
            this.btnLearning.AutoFont = false;
            this.btnLearning.AutoFontHeightRatio = 0.75F;
            this.btnLearning.AutoFontMax = 100F;
            this.btnLearning.AutoFontMin = 6F;
            this.btnLearning.AutoFontWidthRatio = 0.92F;
            this.btnLearning.AutoImage = true;
            this.btnLearning.AutoImageMaxRatio = 0.75F;
            this.btnLearning.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLearning.AutoImageTint = true;
            this.btnLearning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLearning.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLearning.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLearning.BorderRadius = 10;
            this.btnLearning.BorderSize = 1;
            this.btnLearning.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnLearning.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnLearning.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnLearning.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLearning.Corner = BeeGlobal.Corner.Both;
            this.btnLearning.DebounceResizeMs = 16;
            this.btnLearning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLearning.FlatAppearance.BorderSize = 0;
            this.btnLearning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLearning.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLearning.ForeColor = System.Drawing.Color.Black;
            this.btnLearning.Image = null;
            this.btnLearning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLearning.ImageDisabled = null;
            this.btnLearning.ImageHover = null;
            this.btnLearning.ImageNormal = null;
            this.btnLearning.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLearning.ImagePressed = null;
            this.btnLearning.ImageTextSpacing = 6;
            this.btnLearning.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLearning.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLearning.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLearning.ImageTintOpacity = 0.5F;
            this.btnLearning.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLearning.IsCLick = false;
            this.btnLearning.IsNotChange = true;
            this.btnLearning.IsRect = false;
            this.btnLearning.IsUnGroup = true;
            this.btnLearning.Location = new System.Drawing.Point(295, 15);
            this.btnLearning.Margin = new System.Windows.Forms.Padding(10);
            this.btnLearning.Multiline = false;
            this.btnLearning.Name = "btnLearning";
            this.btnLearning.Size = new System.Drawing.Size(136, 170);
            this.btnLearning.TabIndex = 5;
            this.btnLearning.Text = "Teach Sample";
            this.btnLearning.TextColor = System.Drawing.Color.Black;
            this.btnLearning.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLearning.UseVisualStyleBackColor = false;
            this.btnLearning.Click += new System.EventHandler(this.btnLearning_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(6, 471);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(446, 32);
            this.label9.TabIndex = 74;
            this.label9.Text = "Training Sample";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(6, 267);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(446, 32);
            this.label5.TabIndex = 73;
            this.label5.Text = "Threshold Distance";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(6, 127);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(446, 25);
            this.label13.TabIndex = 72;
            this.label13.Text = "Choose Area";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(6, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(446, 32);
            this.label7.TabIndex = 71;
            this.label7.Text = "Search Range";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackScore
            // 
            this.trackScore.AutoRepeatAccelDeltaMs = -5;
            this.trackScore.AutoRepeatAccelerate = true;
            this.trackScore.AutoRepeatEnabled = true;
            this.trackScore.AutoRepeatInitialDelay = 400;
            this.trackScore.AutoRepeatInterval = 60;
            this.trackScore.AutoRepeatMinInterval = 20;
            this.trackScore.AutoShowTextbox = true;
            this.trackScore.AutoSizeTextbox = true;
            this.trackScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.trackScore.BarLeftGap = 20;
            this.trackScore.BarRightGap = 6;
            this.trackScore.ChromeGap = 8;
            this.trackScore.ChromeWidthRatio = 0.14F;
            this.trackScore.ColorBorder = System.Drawing.Color.LightGray;
            this.trackScore.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackScore.ColorScale = System.Drawing.Color.LightGray;
            this.trackScore.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorTrack = System.Drawing.Color.LightGray;
            this.trackScore.Decimals = 0;
            this.trackScore.DisabledDesaturateMix = 0.3F;
            this.trackScore.DisabledDimFactor = 0.9F;
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(6, 755);
            this.trackScore.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.trackScore.MatchTextboxFontToThumb = true;
            this.trackScore.Max = 100F;
            this.trackScore.MaxTextboxWidth = 0;
            this.trackScore.MaxThumb = 1000;
            this.trackScore.MaxTrackHeight = 1000;
            this.trackScore.Min = 0F;
            this.trackScore.MinChromeWidth = 64;
            this.trackScore.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackScore.MinTextboxWidth = 32;
            this.trackScore.MinThumb = 30;
            this.trackScore.MinTrackHeight = 8;
            this.trackScore.Name = "trackScore";
            this.trackScore.Radius = 8;
            this.trackScore.ShowValueOnThumb = true;
            this.trackScore.Size = new System.Drawing.Size(446, 49);
            this.trackScore.SnapToStep = true;
            this.trackScore.StartWithTextboxHidden = true;
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 70;
            this.trackScore.TextboxFontSize = 20F;
            this.trackScore.TextboxSidePadding = 10;
            this.trackScore.TextboxWidth = 600;
            this.trackScore.ThumbDiameterRatio = 2F;
            this.trackScore.ThumbValueBold = true;
            this.trackScore.ThumbValueFontScale = 1.5F;
            this.trackScore.ThumbValuePadding = 0;
            this.trackScore.TightEdges = true;
            this.trackScore.TrackHeightRatio = 0.45F;
            this.trackScore.TrackWidthRatio = 1F;
            this.trackScore.UnitText = "";
            this.trackScore.Value = 0F;
            this.trackScore.WheelStep = 1F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.Controls.Add(this.btnClear, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropRect, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropArea, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 152);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(446, 55);
            this.tableLayoutPanel3.TabIndex = 41;
            // 
            // btnClear
            // 
            this.btnClear.AutoFont = true;
            this.btnClear.AutoFontHeightRatio = 0.75F;
            this.btnClear.AutoFontMax = 100F;
            this.btnClear.AutoFontMin = 6F;
            this.btnClear.AutoFontWidthRatio = 0.92F;
            this.btnClear.AutoImage = true;
            this.btnClear.AutoImageMaxRatio = 0.75F;
            this.btnClear.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnClear.AutoImageTint = true;
            this.btnClear.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClear.BackgroundImage")));
            this.btnClear.BorderColor = System.Drawing.Color.Silver;
            this.btnClear.BorderRadius = 10;
            this.btnClear.BorderSize = 1;
            this.btnClear.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnClear.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnClear.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnClear.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnClear.Corner = BeeGlobal.Corner.Right;
            this.btnClear.DebounceResizeMs = 16;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Image = null;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.ImageDisabled = null;
            this.btnClear.ImageHover = null;
            this.btnClear.ImageNormal = null;
            this.btnClear.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnClear.ImagePressed = null;
            this.btnClear.ImageTextSpacing = 6;
            this.btnClear.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnClear.ImageTintHover = System.Drawing.Color.Empty;
            this.btnClear.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnClear.ImageTintOpacity = 0.5F;
            this.btnClear.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnClear.IsCLick = false;
            this.btnClear.IsNotChange = false;
            this.btnClear.IsRect = false;
            this.btnClear.IsUnGroup = false;
            this.btnClear.Location = new System.Drawing.Point(299, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Multiline = false;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(142, 45);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Area Mask";
            this.btnClear.TextColor = System.Drawing.Color.Black;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCropRect
            // 
            this.btnCropRect.AutoFont = true;
            this.btnCropRect.AutoFontHeightRatio = 0.75F;
            this.btnCropRect.AutoFontMax = 100F;
            this.btnCropRect.AutoFontMin = 6F;
            this.btnCropRect.AutoFontWidthRatio = 0.92F;
            this.btnCropRect.AutoImage = true;
            this.btnCropRect.AutoImageMaxRatio = 0.75F;
            this.btnCropRect.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropRect.AutoImageTint = true;
            this.btnCropRect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropRect.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropRect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropRect.BackgroundImage")));
            this.btnCropRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropRect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropRect.BorderRadius = 10;
            this.btnCropRect.BorderSize = 1;
            this.btnCropRect.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCropRect.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCropRect.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCropRect.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropRect.Corner = BeeGlobal.Corner.Left;
            this.btnCropRect.DebounceResizeMs = 16;
            this.btnCropRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropRect.FlatAppearance.BorderSize = 0;
            this.btnCropRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnCropRect.ForeColor = System.Drawing.Color.Black;
            this.btnCropRect.Image = null;
            this.btnCropRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropRect.ImageDisabled = null;
            this.btnCropRect.ImageHover = null;
            this.btnCropRect.ImageNormal = null;
            this.btnCropRect.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropRect.ImagePressed = null;
            this.btnCropRect.ImageTextSpacing = 6;
            this.btnCropRect.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropRect.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropRect.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropRect.ImageTintOpacity = 0.5F;
            this.btnCropRect.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropRect.IsCLick = false;
            this.btnCropRect.IsNotChange = false;
            this.btnCropRect.IsRect = false;
            this.btnCropRect.IsUnGroup = false;
            this.btnCropRect.Location = new System.Drawing.Point(5, 5);
            this.btnCropRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropRect.Multiline = false;
            this.btnCropRect.Name = "btnCropRect";
            this.btnCropRect.Size = new System.Drawing.Size(141, 45);
            this.btnCropRect.TabIndex = 2;
            this.btnCropRect.Text = "Area Temp";
            this.btnCropRect.TextColor = System.Drawing.Color.Black;
            this.btnCropRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropRect.UseVisualStyleBackColor = false;
            this.btnCropRect.Click += new System.EventHandler(this.btnCropRect_Click);
            // 
            // btnCropArea
            // 
            this.btnCropArea.AutoFont = true;
            this.btnCropArea.AutoFontHeightRatio = 0.75F;
            this.btnCropArea.AutoFontMax = 100F;
            this.btnCropArea.AutoFontMin = 6F;
            this.btnCropArea.AutoFontWidthRatio = 0.92F;
            this.btnCropArea.AutoImage = true;
            this.btnCropArea.AutoImageMaxRatio = 0.75F;
            this.btnCropArea.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropArea.AutoImageTint = true;
            this.btnCropArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropArea.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropArea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropArea.BackgroundImage")));
            this.btnCropArea.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropArea.BorderRadius = 5;
            this.btnCropArea.BorderSize = 1;
            this.btnCropArea.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCropArea.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCropArea.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCropArea.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropArea.Corner = BeeGlobal.Corner.None;
            this.btnCropArea.DebounceResizeMs = 16;
            this.btnCropArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnCropArea.ForeColor = System.Drawing.Color.Black;
            this.btnCropArea.Image = null;
            this.btnCropArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropArea.ImageDisabled = null;
            this.btnCropArea.ImageHover = null;
            this.btnCropArea.ImageNormal = null;
            this.btnCropArea.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropArea.ImagePressed = null;
            this.btnCropArea.ImageTextSpacing = 6;
            this.btnCropArea.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropArea.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropArea.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropArea.ImageTintOpacity = 0.5F;
            this.btnCropArea.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropArea.IsCLick = true;
            this.btnCropArea.IsNotChange = false;
            this.btnCropArea.IsRect = false;
            this.btnCropArea.IsUnGroup = false;
            this.btnCropArea.Location = new System.Drawing.Point(146, 5);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropArea.Multiline = false;
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(153, 45);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnCropFull, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCropHalt, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 52);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(446, 55);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // btnCropFull
            // 
            this.btnCropFull.AutoFont = true;
            this.btnCropFull.AutoFontHeightRatio = 0.75F;
            this.btnCropFull.AutoFontMax = 100F;
            this.btnCropFull.AutoFontMin = 6F;
            this.btnCropFull.AutoFontWidthRatio = 0.92F;
            this.btnCropFull.AutoImage = true;
            this.btnCropFull.AutoImageMaxRatio = 0.75F;
            this.btnCropFull.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropFull.AutoImageTint = true;
            this.btnCropFull.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropFull.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropFull.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCropFull.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCropFull.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCropFull.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.DebounceResizeMs = 16;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnCropFull.ForeColor = System.Drawing.Color.Black;
            this.btnCropFull.Image = null;
            this.btnCropFull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropFull.ImageDisabled = null;
            this.btnCropFull.ImageHover = null;
            this.btnCropFull.ImageNormal = null;
            this.btnCropFull.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropFull.ImagePressed = null;
            this.btnCropFull.ImageTextSpacing = 6;
            this.btnCropFull.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropFull.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropFull.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropFull.ImageTintOpacity = 0.5F;
            this.btnCropFull.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropFull.IsCLick = false;
            this.btnCropFull.IsNotChange = false;
            this.btnCropFull.IsRect = false;
            this.btnCropFull.IsUnGroup = false;
            this.btnCropFull.Location = new System.Drawing.Point(223, 5);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Multiline = false;
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(215, 45);
            this.btnCropFull.TabIndex = 3;
            this.btnCropFull.Text = "Partial";
            this.btnCropFull.TextColor = System.Drawing.Color.Black;
            this.btnCropFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropFull.UseVisualStyleBackColor = false;
            this.btnCropFull.Click += new System.EventHandler(this.btnCropFull_Click);
            // 
            // btnCropHalt
            // 
            this.btnCropHalt.AutoFont = true;
            this.btnCropHalt.AutoFontHeightRatio = 0.75F;
            this.btnCropHalt.AutoFontMax = 100F;
            this.btnCropHalt.AutoFontMin = 6F;
            this.btnCropHalt.AutoFontWidthRatio = 0.92F;
            this.btnCropHalt.AutoImage = true;
            this.btnCropHalt.AutoImageMaxRatio = 0.75F;
            this.btnCropHalt.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropHalt.AutoImageTint = true;
            this.btnCropHalt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropHalt.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCropHalt.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCropHalt.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCropHalt.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.DebounceResizeMs = 16;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnCropHalt.ForeColor = System.Drawing.Color.Black;
            this.btnCropHalt.Image = null;
            this.btnCropHalt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropHalt.ImageDisabled = null;
            this.btnCropHalt.ImageHover = null;
            this.btnCropHalt.ImageNormal = null;
            this.btnCropHalt.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropHalt.ImagePressed = null;
            this.btnCropHalt.ImageTextSpacing = 6;
            this.btnCropHalt.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropHalt.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropHalt.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropHalt.ImageTintOpacity = 0.5F;
            this.btnCropHalt.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropHalt.IsCLick = true;
            this.btnCropHalt.IsNotChange = false;
            this.btnCropHalt.IsRect = false;
            this.btnCropHalt.IsUnGroup = false;
            this.btnCropHalt.Location = new System.Drawing.Point(8, 5);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCropHalt.Multiline = false;
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(215, 45);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnNone, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnElip, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnRect, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(6, 207);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(446, 40);
            this.tableLayoutPanel5.TabIndex = 54;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(23, 3);
            this.label12.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 34);
            this.label12.TabIndex = 58;
            this.label12.Text = "Shape";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNone
            // 
            this.btnNone.AutoFont = true;
            this.btnNone.AutoFontHeightRatio = 0.75F;
            this.btnNone.AutoFontMax = 100F;
            this.btnNone.AutoFontMin = 6F;
            this.btnNone.AutoFontWidthRatio = 0.92F;
            this.btnNone.AutoImage = true;
            this.btnNone.AutoImageMaxRatio = 0.75F;
            this.btnNone.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnNone.AutoImageTint = true;
            this.btnNone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnNone.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnNone.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnNone.BorderRadius = 10;
            this.btnNone.BorderSize = 1;
            this.btnNone.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnNone.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnNone.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnNone.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnNone.Corner = BeeGlobal.Corner.Right;
            this.btnNone.DebounceResizeMs = 16;
            this.btnNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNone.FlatAppearance.BorderSize = 0;
            this.btnNone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.671875F);
            this.btnNone.ForeColor = System.Drawing.Color.Black;
            this.btnNone.Image = null;
            this.btnNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNone.ImageDisabled = null;
            this.btnNone.ImageHover = null;
            this.btnNone.ImageNormal = null;
            this.btnNone.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnNone.ImagePressed = null;
            this.btnNone.ImageTextSpacing = 6;
            this.btnNone.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnNone.ImageTintHover = System.Drawing.Color.Empty;
            this.btnNone.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnNone.ImageTintOpacity = 0.5F;
            this.btnNone.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnNone.IsCLick = false;
            this.btnNone.IsNotChange = false;
            this.btnNone.IsRect = false;
            this.btnNone.IsUnGroup = false;
            this.btnNone.Location = new System.Drawing.Point(329, 3);
            this.btnNone.Margin = new System.Windows.Forms.Padding(0);
            this.btnNone.Multiline = false;
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(114, 34);
            this.btnNone.TabIndex = 5;
            this.btnNone.Text = "None";
            this.btnNone.TextColor = System.Drawing.Color.Black;
            this.btnNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNone.UseVisualStyleBackColor = false;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnElip
            // 
            this.btnElip.AutoFont = true;
            this.btnElip.AutoFontHeightRatio = 0.75F;
            this.btnElip.AutoFontMax = 100F;
            this.btnElip.AutoFontMin = 6F;
            this.btnElip.AutoFontWidthRatio = 0.92F;
            this.btnElip.AutoImage = true;
            this.btnElip.AutoImageMaxRatio = 0.75F;
            this.btnElip.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnElip.AutoImageTint = true;
            this.btnElip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnElip.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnElip.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnElip.BorderRadius = 10;
            this.btnElip.BorderSize = 1;
            this.btnElip.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnElip.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnElip.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnElip.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnElip.Corner = BeeGlobal.Corner.None;
            this.btnElip.DebounceResizeMs = 16;
            this.btnElip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnElip.FlatAppearance.BorderSize = 0;
            this.btnElip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnElip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.671875F);
            this.btnElip.ForeColor = System.Drawing.Color.Black;
            this.btnElip.Image = null;
            this.btnElip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnElip.ImageDisabled = null;
            this.btnElip.ImageHover = null;
            this.btnElip.ImageNormal = null;
            this.btnElip.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnElip.ImagePressed = null;
            this.btnElip.ImageTextSpacing = 6;
            this.btnElip.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnElip.ImageTintHover = System.Drawing.Color.Empty;
            this.btnElip.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnElip.ImageTintOpacity = 0.5F;
            this.btnElip.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnElip.IsCLick = false;
            this.btnElip.IsNotChange = false;
            this.btnElip.IsRect = false;
            this.btnElip.IsUnGroup = false;
            this.btnElip.Location = new System.Drawing.Point(216, 3);
            this.btnElip.Margin = new System.Windows.Forms.Padding(0);
            this.btnElip.Multiline = false;
            this.btnElip.Name = "btnElip";
            this.btnElip.Size = new System.Drawing.Size(113, 34);
            this.btnElip.TabIndex = 3;
            this.btnElip.Text = "Elip";
            this.btnElip.TextColor = System.Drawing.Color.Black;
            this.btnElip.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnElip.UseVisualStyleBackColor = false;
            this.btnElip.Click += new System.EventHandler(this.btnElip_Click);
            // 
            // btnRect
            // 
            this.btnRect.AutoFont = true;
            this.btnRect.AutoFontHeightRatio = 0.75F;
            this.btnRect.AutoFontMax = 100F;
            this.btnRect.AutoFontMin = 6F;
            this.btnRect.AutoFontWidthRatio = 0.92F;
            this.btnRect.AutoImage = true;
            this.btnRect.AutoImageMaxRatio = 0.75F;
            this.btnRect.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRect.AutoImageTint = true;
            this.btnRect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnRect.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnRect.BorderRadius = 10;
            this.btnRect.BorderSize = 1;
            this.btnRect.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnRect.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnRect.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnRect.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRect.Corner = BeeGlobal.Corner.Left;
            this.btnRect.DebounceResizeMs = 16;
            this.btnRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRect.FlatAppearance.BorderSize = 0;
            this.btnRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.671875F);
            this.btnRect.ForeColor = System.Drawing.Color.Black;
            this.btnRect.Image = null;
            this.btnRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRect.ImageDisabled = null;
            this.btnRect.ImageHover = null;
            this.btnRect.ImageNormal = null;
            this.btnRect.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRect.ImagePressed = null;
            this.btnRect.ImageTextSpacing = 6;
            this.btnRect.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRect.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRect.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRect.ImageTintOpacity = 0.5F;
            this.btnRect.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRect.IsCLick = true;
            this.btnRect.IsNotChange = false;
            this.btnRect.IsRect = false;
            this.btnRect.IsUnGroup = false;
            this.btnRect.Location = new System.Drawing.Point(103, 3);
            this.btnRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnRect.Multiline = false;
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(113, 34);
            this.btnRect.TabIndex = 4;
            this.btnRect.Text = "Rectangle";
            this.btnRect.TextColor = System.Drawing.Color.Black;
            this.btnRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRect.UseVisualStyleBackColor = false;
            this.btnRect.Click += new System.EventHandler(this.btnRect_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(480, 749);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.AutoScroll = true;
            this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.label1, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.trackNumObject, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.trackMaxOverLap, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 3);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 9;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(474, 743);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(5, 296);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(464, 32);
            this.label1.TabIndex = 81;
            this.label1.Text = "Maximum Object";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(5, 199);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(464, 32);
            this.label3.TabIndex = 80;
            this.label3.Text = "Option More";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(5, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(464, 32);
            this.label2.TabIndex = 79;
            this.label2.Text = "Search Algorithm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(5, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(464, 32);
            this.label4.TabIndex = 78;
            this.label4.Text = "OverLap Range";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackNumObject
            // 
            this.trackNumObject.AutoRepeatAccelDeltaMs = -5;
            this.trackNumObject.AutoRepeatAccelerate = true;
            this.trackNumObject.AutoRepeatEnabled = true;
            this.trackNumObject.AutoRepeatInitialDelay = 400;
            this.trackNumObject.AutoRepeatInterval = 60;
            this.trackNumObject.AutoRepeatMinInterval = 20;
            this.trackNumObject.AutoShowTextbox = true;
            this.trackNumObject.AutoSizeTextbox = true;
            this.trackNumObject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.trackNumObject.BarLeftGap = 20;
            this.trackNumObject.BarRightGap = 6;
            this.trackNumObject.ChromeGap = 8;
            this.trackNumObject.ChromeWidthRatio = 0.14F;
            this.trackNumObject.ColorBorder = System.Drawing.Color.LightGray;
            this.trackNumObject.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackNumObject.ColorScale = System.Drawing.Color.LightGray;
            this.trackNumObject.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackNumObject.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackNumObject.ColorTrack = System.Drawing.Color.LightGray;
            this.trackNumObject.Decimals = 0;
            this.trackNumObject.DisabledDesaturateMix = 0.3F;
            this.trackNumObject.DisabledDimFactor = 0.9F;
            this.trackNumObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackNumObject.EdgePadding = 2;
            this.trackNumObject.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackNumObject.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackNumObject.KeyboardStep = 1F;
            this.trackNumObject.Location = new System.Drawing.Point(5, 328);
            this.trackNumObject.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.trackNumObject.MatchTextboxFontToThumb = true;
            this.trackNumObject.Max = 100F;
            this.trackNumObject.MaxTextboxWidth = 0;
            this.trackNumObject.MaxThumb = 1000;
            this.trackNumObject.MaxTrackHeight = 1000;
            this.trackNumObject.Min = 1F;
            this.trackNumObject.MinChromeWidth = 64;
            this.trackNumObject.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackNumObject.MinTextboxWidth = 32;
            this.trackNumObject.MinThumb = 30;
            this.trackNumObject.MinTrackHeight = 8;
            this.trackNumObject.Name = "trackNumObject";
            this.trackNumObject.Radius = 8;
            this.trackNumObject.ShowValueOnThumb = true;
            this.trackNumObject.Size = new System.Drawing.Size(464, 50);
            this.trackNumObject.SnapToStep = true;
            this.trackNumObject.StartWithTextboxHidden = true;
            this.trackNumObject.Step = 1F;
            this.trackNumObject.TabIndex = 76;
            this.trackNumObject.TextboxFontSize = 20F;
            this.trackNumObject.TextboxSidePadding = 10;
            this.trackNumObject.TextboxWidth = 600;
            this.trackNumObject.ThumbDiameterRatio = 2F;
            this.trackNumObject.ThumbValueBold = true;
            this.trackNumObject.ThumbValueFontScale = 1.5F;
            this.trackNumObject.ThumbValuePadding = 0;
            this.trackNumObject.TightEdges = true;
            this.trackNumObject.TrackHeightRatio = 0.45F;
            this.trackNumObject.TrackWidthRatio = 1F;
            this.trackNumObject.UnitText = "";
            this.trackNumObject.Value = 1F;
            this.trackNumObject.WheelStep = 1F;
            // 
            // trackMaxOverLap
            // 
            this.trackMaxOverLap.AutoRepeatAccelDeltaMs = -5;
            this.trackMaxOverLap.AutoRepeatAccelerate = true;
            this.trackMaxOverLap.AutoRepeatEnabled = true;
            this.trackMaxOverLap.AutoRepeatInitialDelay = 400;
            this.trackMaxOverLap.AutoRepeatInterval = 60;
            this.trackMaxOverLap.AutoRepeatMinInterval = 20;
            this.trackMaxOverLap.AutoShowTextbox = true;
            this.trackMaxOverLap.AutoSizeTextbox = true;
            this.trackMaxOverLap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.trackMaxOverLap.BarLeftGap = 20;
            this.trackMaxOverLap.BarRightGap = 6;
            this.trackMaxOverLap.ChromeGap = 8;
            this.trackMaxOverLap.ChromeWidthRatio = 0.14F;
            this.trackMaxOverLap.ColorBorder = System.Drawing.Color.LightGray;
            this.trackMaxOverLap.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackMaxOverLap.ColorScale = System.Drawing.Color.LightGray;
            this.trackMaxOverLap.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackMaxOverLap.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackMaxOverLap.ColorTrack = System.Drawing.Color.LightGray;
            this.trackMaxOverLap.Decimals = 0;
            this.trackMaxOverLap.DisabledDesaturateMix = 0.3F;
            this.trackMaxOverLap.DisabledDimFactor = 0.9F;
            this.trackMaxOverLap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackMaxOverLap.EdgePadding = 2;
            this.trackMaxOverLap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackMaxOverLap.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackMaxOverLap.KeyboardStep = 1F;
            this.trackMaxOverLap.Location = new System.Drawing.Point(5, 42);
            this.trackMaxOverLap.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.trackMaxOverLap.MatchTextboxFontToThumb = true;
            this.trackMaxOverLap.Max = 100F;
            this.trackMaxOverLap.MaxTextboxWidth = 0;
            this.trackMaxOverLap.MaxThumb = 1000;
            this.trackMaxOverLap.MaxTrackHeight = 1000;
            this.trackMaxOverLap.Min = 1F;
            this.trackMaxOverLap.MinChromeWidth = 64;
            this.trackMaxOverLap.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackMaxOverLap.MinTextboxWidth = 32;
            this.trackMaxOverLap.MinThumb = 30;
            this.trackMaxOverLap.MinTrackHeight = 8;
            this.trackMaxOverLap.Name = "trackMaxOverLap";
            this.trackMaxOverLap.Radius = 8;
            this.trackMaxOverLap.ShowValueOnThumb = true;
            this.trackMaxOverLap.Size = new System.Drawing.Size(464, 50);
            this.trackMaxOverLap.SnapToStep = true;
            this.trackMaxOverLap.StartWithTextboxHidden = true;
            this.trackMaxOverLap.Step = 1F;
            this.trackMaxOverLap.TabIndex = 72;
            this.trackMaxOverLap.TextboxFontSize = 20F;
            this.trackMaxOverLap.TextboxSidePadding = 10;
            this.trackMaxOverLap.TextboxWidth = 600;
            this.trackMaxOverLap.ThumbDiameterRatio = 2F;
            this.trackMaxOverLap.ThumbValueBold = true;
            this.trackMaxOverLap.ThumbValueFontScale = 1.5F;
            this.trackMaxOverLap.ThumbValuePadding = 0;
            this.trackMaxOverLap.TightEdges = true;
            this.trackMaxOverLap.TrackHeightRatio = 0.45F;
            this.trackMaxOverLap.TrackWidthRatio = 1F;
            this.trackMaxOverLap.UnitText = "";
            this.trackMaxOverLap.Value = 1F;
            this.trackMaxOverLap.WheelStep = 1F;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tableLayoutPanel14.ColumnCount = 3;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 231);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(464, 55);
            this.tableLayoutPanel14.TabIndex = 53;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 134);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(469, 55);
            this.tableLayoutPanel13.TabIndex = 51;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel16);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(480, 749);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Limit";
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel16.ColumnCount = 1;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel16.Controls.Add(this.numLimitCounter, 0, 2);
            this.tableLayoutPanel16.Controls.Add(this.layLimitCouter, 0, 1);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 4;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(474, 743);
            this.tableLayoutPanel16.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(5, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(464, 32);
            this.label6.TabIndex = 79;
            this.label6.Text = "Limit Couter";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numLimitCounter
            // 
            this.numLimitCounter.AutoRepeatAccelDeltaMs = -5;
            this.numLimitCounter.AutoRepeatAccelerate = true;
            this.numLimitCounter.AutoRepeatEnabled = true;
            this.numLimitCounter.AutoRepeatInitialDelay = 400;
            this.numLimitCounter.AutoRepeatInterval = 60;
            this.numLimitCounter.AutoRepeatMinInterval = 20;
            this.numLimitCounter.AutoShowTextbox = true;
            this.numLimitCounter.AutoSizeTextbox = true;
            this.numLimitCounter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.numLimitCounter.BarLeftGap = 20;
            this.numLimitCounter.BarRightGap = 6;
            this.numLimitCounter.ChromeGap = 8;
            this.numLimitCounter.ChromeWidthRatio = 0.14F;
            this.numLimitCounter.ColorBorder = System.Drawing.Color.LightGray;
            this.numLimitCounter.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.numLimitCounter.ColorScale = System.Drawing.Color.LightGray;
            this.numLimitCounter.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.numLimitCounter.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.numLimitCounter.ColorTrack = System.Drawing.Color.LightGray;
            this.numLimitCounter.Decimals = 0;
            this.numLimitCounter.DisabledDesaturateMix = 0.3F;
            this.numLimitCounter.DisabledDimFactor = 0.9F;
            this.numLimitCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numLimitCounter.EdgePadding = 2;
            this.numLimitCounter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numLimitCounter.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.numLimitCounter.KeyboardStep = 1F;
            this.numLimitCounter.Location = new System.Drawing.Point(5, 97);
            this.numLimitCounter.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.numLimitCounter.MatchTextboxFontToThumb = true;
            this.numLimitCounter.Max = 100F;
            this.numLimitCounter.MaxTextboxWidth = 0;
            this.numLimitCounter.MaxThumb = 1000;
            this.numLimitCounter.MaxTrackHeight = 1000;
            this.numLimitCounter.Min = 1F;
            this.numLimitCounter.MinChromeWidth = 64;
            this.numLimitCounter.MinimumSize = new System.Drawing.Size(140, 36);
            this.numLimitCounter.MinTextboxWidth = 32;
            this.numLimitCounter.MinThumb = 30;
            this.numLimitCounter.MinTrackHeight = 8;
            this.numLimitCounter.Name = "numLimitCounter";
            this.numLimitCounter.Padding = new System.Windows.Forms.Padding(2);
            this.numLimitCounter.Radius = 8;
            this.numLimitCounter.ShowValueOnThumb = true;
            this.numLimitCounter.Size = new System.Drawing.Size(464, 50);
            this.numLimitCounter.SnapToStep = true;
            this.numLimitCounter.StartWithTextboxHidden = true;
            this.numLimitCounter.Step = 1F;
            this.numLimitCounter.TabIndex = 73;
            this.numLimitCounter.TextboxFontSize = 20F;
            this.numLimitCounter.TextboxSidePadding = 10;
            this.numLimitCounter.TextboxWidth = 600;
            this.numLimitCounter.ThumbDiameterRatio = 2F;
            this.numLimitCounter.ThumbValueBold = true;
            this.numLimitCounter.ThumbValueFontScale = 1.5F;
            this.numLimitCounter.ThumbValuePadding = 0;
            this.numLimitCounter.TightEdges = true;
            this.numLimitCounter.TrackHeightRatio = 0.45F;
            this.numLimitCounter.TrackWidthRatio = 1F;
            this.numLimitCounter.UnitText = "";
            this.numLimitCounter.Value = 1F;
            this.numLimitCounter.WheelStep = 1F;
            this.numLimitCounter.ValueChanged += new System.Action<float>(this.numLimitCounter_ValueChanged);
            // 
            // layLimitCouter
            // 
            this.layLimitCouter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.layLimitCouter.ColumnCount = 3;
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layLimitCouter.Controls.Add(this.btnMore, 2, 0);
            this.layLimitCouter.Controls.Add(this.btnEqual, 1, 0);
            this.layLimitCouter.Controls.Add(this.btnLess, 0, 0);
            this.layLimitCouter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layLimitCouter.Location = new System.Drawing.Point(5, 42);
            this.layLimitCouter.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layLimitCouter.Name = "layLimitCouter";
            this.layLimitCouter.Padding = new System.Windows.Forms.Padding(5);
            this.layLimitCouter.RowCount = 1;
            this.layLimitCouter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layLimitCouter.Size = new System.Drawing.Size(464, 55);
            this.layLimitCouter.TabIndex = 48;
            // 
            // btnMore
            // 
            this.btnMore.AutoFont = true;
            this.btnMore.AutoFontHeightRatio = 0.75F;
            this.btnMore.AutoFontMax = 100F;
            this.btnMore.AutoFontMin = 6F;
            this.btnMore.AutoFontWidthRatio = 0.92F;
            this.btnMore.AutoImage = true;
            this.btnMore.AutoImageMaxRatio = 0.75F;
            this.btnMore.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMore.AutoImageTint = true;
            this.btnMore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnMore.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnMore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMore.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnMore.BorderRadius = 10;
            this.btnMore.BorderSize = 1;
            this.btnMore.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMore.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMore.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMore.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMore.Corner = BeeGlobal.Corner.Right;
            this.btnMore.DebounceResizeMs = 16;
            this.btnMore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMore.FlatAppearance.BorderSize = 0;
            this.btnMore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnMore.ForeColor = System.Drawing.Color.Black;
            this.btnMore.Image = null;
            this.btnMore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMore.ImageDisabled = null;
            this.btnMore.ImageHover = null;
            this.btnMore.ImageNormal = null;
            this.btnMore.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMore.ImagePressed = null;
            this.btnMore.ImageTextSpacing = 6;
            this.btnMore.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMore.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMore.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMore.ImageTintOpacity = 0.5F;
            this.btnMore.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMore.IsCLick = false;
            this.btnMore.IsNotChange = false;
            this.btnMore.IsRect = false;
            this.btnMore.IsUnGroup = false;
            this.btnMore.Location = new System.Drawing.Point(307, 5);
            this.btnMore.Margin = new System.Windows.Forms.Padding(0);
            this.btnMore.Multiline = false;
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(152, 45);
            this.btnMore.TabIndex = 33;
            this.btnMore.Text = "More";
            this.btnMore.TextColor = System.Drawing.Color.Black;
            this.btnMore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMore.UseVisualStyleBackColor = false;
            this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
            // 
            // btnEqual
            // 
            this.btnEqual.AutoFont = true;
            this.btnEqual.AutoFontHeightRatio = 0.75F;
            this.btnEqual.AutoFontMax = 100F;
            this.btnEqual.AutoFontMin = 6F;
            this.btnEqual.AutoFontWidthRatio = 0.92F;
            this.btnEqual.AutoImage = true;
            this.btnEqual.AutoImageMaxRatio = 0.75F;
            this.btnEqual.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEqual.AutoImageTint = true;
            this.btnEqual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEqual.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEqual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEqual.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEqual.BorderRadius = 5;
            this.btnEqual.BorderSize = 1;
            this.btnEqual.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEqual.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEqual.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEqual.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEqual.Corner = BeeGlobal.Corner.None;
            this.btnEqual.DebounceResizeMs = 16;
            this.btnEqual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEqual.FlatAppearance.BorderSize = 0;
            this.btnEqual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnEqual.ForeColor = System.Drawing.Color.Black;
            this.btnEqual.Image = null;
            this.btnEqual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEqual.ImageDisabled = null;
            this.btnEqual.ImageHover = null;
            this.btnEqual.ImageNormal = null;
            this.btnEqual.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEqual.ImagePressed = null;
            this.btnEqual.ImageTextSpacing = 6;
            this.btnEqual.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEqual.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEqual.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEqual.ImageTintOpacity = 0.5F;
            this.btnEqual.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEqual.IsCLick = true;
            this.btnEqual.IsNotChange = false;
            this.btnEqual.IsRect = false;
            this.btnEqual.IsUnGroup = false;
            this.btnEqual.Location = new System.Drawing.Point(156, 5);
            this.btnEqual.Margin = new System.Windows.Forms.Padding(0);
            this.btnEqual.Multiline = false;
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(151, 45);
            this.btnEqual.TabIndex = 32;
            this.btnEqual.Text = "Equal";
            this.btnEqual.TextColor = System.Drawing.Color.Black;
            this.btnEqual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEqual.UseVisualStyleBackColor = false;
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
            // 
            // btnLess
            // 
            this.btnLess.AutoFont = true;
            this.btnLess.AutoFontHeightRatio = 0.75F;
            this.btnLess.AutoFontMax = 100F;
            this.btnLess.AutoFontMin = 6F;
            this.btnLess.AutoFontWidthRatio = 0.92F;
            this.btnLess.AutoImage = true;
            this.btnLess.AutoImageMaxRatio = 0.75F;
            this.btnLess.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLess.AutoImageTint = true;
            this.btnLess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLess.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLess.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLess.BorderRadius = 10;
            this.btnLess.BorderSize = 1;
            this.btnLess.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnLess.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnLess.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnLess.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLess.Corner = BeeGlobal.Corner.Left;
            this.btnLess.DebounceResizeMs = 16;
            this.btnLess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLess.FlatAppearance.BorderSize = 0;
            this.btnLess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLess.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnLess.ForeColor = System.Drawing.Color.Black;
            this.btnLess.Image = null;
            this.btnLess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLess.ImageDisabled = null;
            this.btnLess.ImageHover = null;
            this.btnLess.ImageNormal = null;
            this.btnLess.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLess.ImagePressed = null;
            this.btnLess.ImageTextSpacing = 6;
            this.btnLess.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLess.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLess.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLess.ImageTintOpacity = 0.5F;
            this.btnLess.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLess.IsCLick = false;
            this.btnLess.IsNotChange = false;
            this.btnLess.IsRect = false;
            this.btnLess.IsUnGroup = false;
            this.btnLess.Location = new System.Drawing.Point(5, 5);
            this.btnLess.Margin = new System.Windows.Forms.Padding(0);
            this.btnLess.Multiline = false;
            this.btnLess.Name = "btnLess";
            this.btnLess.Size = new System.Drawing.Size(151, 45);
            this.btnLess.TabIndex = 31;
            this.btnLess.Text = "Less";
            this.btnLess.TextColor = System.Drawing.Color.Black;
            this.btnLess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLess.UseVisualStyleBackColor = false;
            this.btnLess.Click += new System.EventHandler(this.btnLess_Click);
            // 
            // workLoadModel
            // 
            this.workLoadModel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadModel_DoWork);
            this.workLoadModel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadModel_RunWorkerCompleted);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 887);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(488, 52);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // pInspect
            // 
            this.pInspect.Controls.Add(this.btnTest);
            this.pInspect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pInspect.Location = new System.Drawing.Point(0, 787);
            this.pInspect.Name = "pInspect";
            this.pInspect.Padding = new System.Windows.Forms.Padding(10);
            this.pInspect.Size = new System.Drawing.Size(488, 100);
            this.pInspect.TabIndex = 82;
            // 
            // btnTest
            // 
            this.btnTest.AutoFont = false;
            this.btnTest.AutoFontHeightRatio = 0.75F;
            this.btnTest.AutoFontMax = 100F;
            this.btnTest.AutoFontMin = 6F;
            this.btnTest.AutoFontWidthRatio = 0.92F;
            this.btnTest.AutoImage = true;
            this.btnTest.AutoImageMaxRatio = 0.75F;
            this.btnTest.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTest.AutoImageTint = true;
            this.btnTest.BackColor = System.Drawing.SystemColors.Control;
            this.btnTest.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnTest.BorderColor = System.Drawing.SystemColors.Control;
            this.btnTest.BorderRadius = 10;
            this.btnTest.BorderSize = 1;
            this.btnTest.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnTest.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnTest.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnTest.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.DebounceResizeMs = 16;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.Image = null;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.ImageDisabled = null;
            this.btnTest.ImageHover = null;
            this.btnTest.ImageNormal = null;
            this.btnTest.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTest.ImagePressed = null;
            this.btnTest.ImageTextSpacing = 6;
            this.btnTest.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTest.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTest.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTest.ImageTintOpacity = 0.5F;
            this.btnTest.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTest.IsCLick = false;
            this.btnTest.IsNotChange = true;
            this.btnTest.IsRect = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(10, 10);
            this.btnTest.Margin = new System.Windows.Forms.Padding(10);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(468, 80);
            this.btnTest.TabIndex = 81;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // ToolVisualMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.pInspect);
            this.Controls.Add(this.oK_Cancel1);
            this.DoubleBuffered = true;
            this.Name = "ToolVisualMatch";
            this.Size = new System.Drawing.Size(488, 939);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.lay3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.layLimitCouter.ResumeLayout(false);
            this.pInspect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private RJButton btnCropArea;
        private RJButton btnCropRect;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TabPage tabPage4;
        private RJButton btnClear;
        public System.ComponentModel.BackgroundWorker threadProcess;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.TableLayoutPanel layLimitCouter;
        private RJButton btnMore;
        private RJButton btnEqual;
        private RJButton btnLess;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnElip;
        private RJButton btnRect;
        private System.ComponentModel.BackgroundWorker workLoadModel;
        private RJButton btnNone;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private GroupControl.OK_Cancel oK_Cancel1;
        private AdjustBarEx trackScore;
        private AdjustBarEx trackMaxOverLap;
        private AdjustBarEx numLimitCounter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel lay3;
        private System.Windows.Forms.PictureBox imgTemp;
        private RJButton btnLearning;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private AdjustBarEx AdjColorTolerance;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pInspect;
        private RJButton btnTest;
        private AdjustBarEx trackNumObject;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private AdjustBarEx AdjMaxDiffPixels;
    }
}
