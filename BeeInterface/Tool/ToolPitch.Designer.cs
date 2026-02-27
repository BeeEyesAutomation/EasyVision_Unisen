using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolPitch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolPitch));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.AdjGaussianSmooth = new BeeInterface.AdjustBarEx();
            this.AdjMagin = new BeeInterface.AdjustBarEx();
            this.AdjScale = new BeeInterface.AdjustBarEx();
            this.label18 = new System.Windows.Forms.Label();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.label8 = new System.Windows.Forms.Label();
            this.layThreshod = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.AdjThreshod = new BeeInterface.AdjustBarEx();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHori = new BeeInterface.RJButton();
            this.btnVer = new BeeInterface.RJButton();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.btnInvert = new BeeInterface.RJButton();
            this.btnBinary = new BeeInterface.RJButton();
            this.btnStrongEdge = new BeeInterface.RJButton();
            this.btnCloseEdge = new BeeInterface.RJButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCalib = new BeeInterface.RJButton();
            this.layLimitCouter = new System.Windows.Forms.TableLayoutPanel();
            this.btnMore = new BeeInterface.RJButton();
            this.btnEqual = new BeeInterface.RJButton();
            this.btnLess = new BeeInterface.RJButton();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.numRootCounter = new BeeInterface.CustomNumericEx();
            this.btnEnRootCounter = new BeeInterface.RJButton();
            this.lbRootCount = new BeeInterface.AutoFontLabel();
            this.autoFontLabel17 = new BeeInterface.AutoFontLabel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEnCrestHeight = new BeeInterface.RJButton();
            this.lbHeightCrestMean = new BeeInterface.AutoFontLabel();
            this.lbHeightCrestMedian = new BeeInterface.AutoFontLabel();
            this.lbHeightCrestMin = new BeeInterface.AutoFontLabel();
            this.lbHeightCrestMax = new BeeInterface.AutoFontLabel();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEnRootHeight = new BeeInterface.RJButton();
            this.lbHeightRootMean = new BeeInterface.AutoFontLabel();
            this.lbHeightRootMedian = new BeeInterface.AutoFontLabel();
            this.lbHeightRootMin = new BeeInterface.AutoFontLabel();
            this.lbHeightRootMax = new BeeInterface.AutoFontLabel();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.lbPitchRootMean = new BeeInterface.AutoFontLabel();
            this.lbPitchRootMedian = new BeeInterface.AutoFontLabel();
            this.lbPitchRootMin = new BeeInterface.AutoFontLabel();
            this.lbPitchRootMax = new BeeInterface.AutoFontLabel();
            this.btnEnRootPitch = new BeeInterface.RJButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.lbPitchCrestMax = new BeeInterface.AutoFontLabel();
            this.lbPitchCrestMin = new BeeInterface.AutoFontLabel();
            this.lbPitchCrestMedian = new BeeInterface.AutoFontLabel();
            this.btnEnCrestPitch = new BeeInterface.RJButton();
            this.lbPitchCrestMean = new BeeInterface.AutoFontLabel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMax = new BeeInterface.RJButton();
            this.btnMin = new BeeInterface.RJButton();
            this.btnMean = new BeeInterface.RJButton();
            this.btnMedian = new BeeInterface.RJButton();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numCrestCouter = new BeeInterface.CustomNumericEx();
            this.lbCrestCount = new BeeInterface.AutoFontLabel();
            this.btnEnCrestCounter = new BeeInterface.RJButton();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.AdjClearBig = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.btnIsClearBig = new BeeInterface.RJButton();
            this.AdjOpen = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.btnOpen = new BeeInterface.RJButton();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.btnIsClearSmall = new BeeInterface.RJButton();
            this.AdjClearNoise = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.btnClose = new BeeInterface.RJButton();
            this.AdjClose = new BeeInterface.AdjustBarEx();
            this.pInspect = new System.Windows.Forms.Panel();
            this.btnTest = new BeeInterface.RJButton();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.layThreshod.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.layLimitCouter.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.pInspect.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabP1);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(428, 808);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 38);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(420, 766);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.AdjGaussianSmooth, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.AdjMagin, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.AdjScale, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.label18, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.layThreshod, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel15, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tableLayoutPanel1.RowCount = 18;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(414, 760);
            this.tableLayoutPanel1.TabIndex = 1;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint_1);
            // 
            // AdjGaussianSmooth
            // 
            this.AdjGaussianSmooth.AutoRepeatAccelDeltaMs = -5;
            this.AdjGaussianSmooth.AutoRepeatAccelerate = true;
            this.AdjGaussianSmooth.AutoRepeatEnabled = true;
            this.AdjGaussianSmooth.AutoRepeatInitialDelay = 400;
            this.AdjGaussianSmooth.AutoRepeatInterval = 60;
            this.AdjGaussianSmooth.AutoRepeatMinInterval = 20;
            this.AdjGaussianSmooth.AutoShowTextbox = true;
            this.AdjGaussianSmooth.AutoSizeTextbox = true;
            this.AdjGaussianSmooth.BackColor = System.Drawing.Color.White;
            this.AdjGaussianSmooth.BarLeftGap = 20;
            this.AdjGaussianSmooth.BarRightGap = 6;
            this.AdjGaussianSmooth.ChromeGap = 1;
            this.AdjGaussianSmooth.ChromeWidthRatio = 0.14F;
            this.AdjGaussianSmooth.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjGaussianSmooth.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjGaussianSmooth.ColorScale = System.Drawing.Color.LightGray;
            this.AdjGaussianSmooth.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjGaussianSmooth.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjGaussianSmooth.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjGaussianSmooth.Decimals = 1;
            this.AdjGaussianSmooth.DisabledDesaturateMix = 0.3F;
            this.AdjGaussianSmooth.DisabledDimFactor = 0.9F;
            this.AdjGaussianSmooth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjGaussianSmooth.EdgePadding = 2;
            this.AdjGaussianSmooth.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjGaussianSmooth.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjGaussianSmooth.KeyboardStep = 1F;
            this.AdjGaussianSmooth.Location = new System.Drawing.Point(6, 324);
            this.AdjGaussianSmooth.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjGaussianSmooth.MatchTextboxFontToThumb = true;
            this.AdjGaussianSmooth.Max = 10F;
            this.AdjGaussianSmooth.MaxTextboxWidth = 0;
            this.AdjGaussianSmooth.MaxThumb = 1000;
            this.AdjGaussianSmooth.MaxTrackHeight = 1000;
            this.AdjGaussianSmooth.Min = 1F;
            this.AdjGaussianSmooth.MinChromeWidth = 64;
            this.AdjGaussianSmooth.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjGaussianSmooth.MinTextboxWidth = 32;
            this.AdjGaussianSmooth.MinThumb = 30;
            this.AdjGaussianSmooth.MinTrackHeight = 8;
            this.AdjGaussianSmooth.Name = "AdjGaussianSmooth";
            this.AdjGaussianSmooth.Radius = 8;
            this.AdjGaussianSmooth.ShowValueOnThumb = true;
            this.AdjGaussianSmooth.Size = new System.Drawing.Size(390, 53);
            this.AdjGaussianSmooth.SnapToStep = true;
            this.AdjGaussianSmooth.StartWithTextboxHidden = true;
            this.AdjGaussianSmooth.Step = 1F;
            this.AdjGaussianSmooth.TabIndex = 84;
            this.AdjGaussianSmooth.TextboxFontSize = 22F;
            this.AdjGaussianSmooth.TextboxSidePadding = 10;
            this.AdjGaussianSmooth.TextboxWidth = 600;
            this.AdjGaussianSmooth.ThumbDiameterRatio = 2F;
            this.AdjGaussianSmooth.ThumbValueBold = true;
            this.AdjGaussianSmooth.ThumbValueFontScale = 1.5F;
            this.AdjGaussianSmooth.ThumbValuePadding = 0;
            this.AdjGaussianSmooth.TightEdges = true;
            this.AdjGaussianSmooth.TrackHeightRatio = 0.45F;
            this.AdjGaussianSmooth.TrackWidthRatio = 1F;
            this.AdjGaussianSmooth.UnitText = "";
            this.AdjGaussianSmooth.Value = 1F;
            this.AdjGaussianSmooth.WheelStep = 1F;
            this.AdjGaussianSmooth.ValueChanged += new System.Action<float>(this.AdjGaussianSmooth_ValueChanged);
            // 
            // AdjMagin
            // 
            this.AdjMagin.AutoRepeatAccelDeltaMs = -5;
            this.AdjMagin.AutoRepeatAccelerate = true;
            this.AdjMagin.AutoRepeatEnabled = true;
            this.AdjMagin.AutoRepeatInitialDelay = 400;
            this.AdjMagin.AutoRepeatInterval = 60;
            this.AdjMagin.AutoRepeatMinInterval = 20;
            this.AdjMagin.AutoShowTextbox = true;
            this.AdjMagin.AutoSizeTextbox = true;
            this.AdjMagin.BackColor = System.Drawing.Color.White;
            this.AdjMagin.BarLeftGap = 20;
            this.AdjMagin.BarRightGap = 6;
            this.AdjMagin.ChromeGap = 1;
            this.AdjMagin.ChromeWidthRatio = 0.14F;
            this.AdjMagin.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjMagin.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjMagin.ColorScale = System.Drawing.Color.LightGray;
            this.AdjMagin.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjMagin.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjMagin.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjMagin.Decimals = 0;
            this.AdjMagin.DisabledDesaturateMix = 0.3F;
            this.AdjMagin.DisabledDimFactor = 0.9F;
            this.AdjMagin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjMagin.EdgePadding = 2;
            this.AdjMagin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjMagin.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjMagin.KeyboardStep = 1F;
            this.AdjMagin.Location = new System.Drawing.Point(6, 229);
            this.AdjMagin.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjMagin.MatchTextboxFontToThumb = true;
            this.AdjMagin.Max = 100F;
            this.AdjMagin.MaxTextboxWidth = 0;
            this.AdjMagin.MaxThumb = 1000;
            this.AdjMagin.MaxTrackHeight = 1000;
            this.AdjMagin.Min = 1F;
            this.AdjMagin.MinChromeWidth = 64;
            this.AdjMagin.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjMagin.MinTextboxWidth = 32;
            this.AdjMagin.MinThumb = 30;
            this.AdjMagin.MinTrackHeight = 8;
            this.AdjMagin.Name = "AdjMagin";
            this.AdjMagin.Radius = 8;
            this.AdjMagin.ShowValueOnThumb = true;
            this.AdjMagin.Size = new System.Drawing.Size(390, 53);
            this.AdjMagin.SnapToStep = true;
            this.AdjMagin.StartWithTextboxHidden = true;
            this.AdjMagin.Step = 1F;
            this.AdjMagin.TabIndex = 83;
            this.AdjMagin.TextboxFontSize = 22F;
            this.AdjMagin.TextboxSidePadding = 10;
            this.AdjMagin.TextboxWidth = 600;
            this.AdjMagin.ThumbDiameterRatio = 2F;
            this.AdjMagin.ThumbValueBold = true;
            this.AdjMagin.ThumbValueFontScale = 1.5F;
            this.AdjMagin.ThumbValuePadding = 0;
            this.AdjMagin.TightEdges = true;
            this.AdjMagin.TrackHeightRatio = 0.45F;
            this.AdjMagin.TrackWidthRatio = 1F;
            this.AdjMagin.UnitText = "";
            this.AdjMagin.Value = 1F;
            this.AdjMagin.WheelStep = 1F;
            this.AdjMagin.ValueChanged += new System.Action<float>(this.AdjMagin_ValueChanged);
            // 
            // AdjScale
            // 
            this.AdjScale.AutoRepeatAccelDeltaMs = -5;
            this.AdjScale.AutoRepeatAccelerate = true;
            this.AdjScale.AutoRepeatEnabled = true;
            this.AdjScale.AutoRepeatInitialDelay = 400;
            this.AdjScale.AutoRepeatInterval = 60;
            this.AdjScale.AutoRepeatMinInterval = 20;
            this.AdjScale.AutoShowTextbox = true;
            this.AdjScale.AutoSizeTextbox = true;
            this.AdjScale.BackColor = System.Drawing.Color.White;
            this.AdjScale.BarLeftGap = 20;
            this.AdjScale.BarRightGap = 6;
            this.AdjScale.ChromeGap = 1;
            this.AdjScale.ChromeWidthRatio = 0.14F;
            this.AdjScale.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjScale.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjScale.ColorScale = System.Drawing.Color.LightGray;
            this.AdjScale.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjScale.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjScale.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjScale.Decimals = 1;
            this.AdjScale.DisabledDesaturateMix = 0.3F;
            this.AdjScale.DisabledDimFactor = 0.9F;
            this.AdjScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjScale.EdgePadding = 2;
            this.AdjScale.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjScale.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjScale.KeyboardStep = 1F;
            this.AdjScale.Location = new System.Drawing.Point(6, 575);
            this.AdjScale.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjScale.MatchTextboxFontToThumb = true;
            this.AdjScale.Max = 1000F;
            this.AdjScale.MaxTextboxWidth = 0;
            this.AdjScale.MaxThumb = 1000;
            this.AdjScale.MaxTrackHeight = 1000;
            this.AdjScale.Min = 1F;
            this.AdjScale.MinChromeWidth = 64;
            this.AdjScale.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjScale.MinTextboxWidth = 32;
            this.AdjScale.MinThumb = 30;
            this.AdjScale.MinTrackHeight = 8;
            this.AdjScale.Name = "AdjScale";
            this.AdjScale.Radius = 8;
            this.AdjScale.ShowValueOnThumb = true;
            this.AdjScale.Size = new System.Drawing.Size(390, 53);
            this.AdjScale.SnapToStep = true;
            this.AdjScale.StartWithTextboxHidden = true;
            this.AdjScale.Step = 1F;
            this.AdjScale.TabIndex = 82;
            this.AdjScale.TextboxFontSize = 22F;
            this.AdjScale.TextboxSidePadding = 10;
            this.AdjScale.TextboxWidth = 600;
            this.AdjScale.ThumbDiameterRatio = 2F;
            this.AdjScale.ThumbValueBold = true;
            this.AdjScale.ThumbValueFontScale = 1.5F;
            this.AdjScale.ThumbValuePadding = 0;
            this.AdjScale.TightEdges = true;
            this.AdjScale.TrackHeightRatio = 0.45F;
            this.AdjScale.TrackWidthRatio = 1F;
            this.AdjScale.UnitText = "";
            this.AdjScale.Value = 1F;
            this.AdjScale.WheelStep = 1F;
            this.AdjScale.ValueChanged += new System.Action<float>(this.AdjScale_ValueChanged);
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Transparent;
            this.label18.Location = new System.Drawing.Point(6, 543);
            this.label18.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(390, 32);
            this.label18.TabIndex = 81;
            this.label18.Text = "Scale Pixel to mm";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.trackScore.BackColor = System.Drawing.Color.White;
            this.trackScore.BarLeftGap = 20;
            this.trackScore.BarRightGap = 6;
            this.trackScore.ChromeGap = 1;
            this.trackScore.ChromeWidthRatio = 0.14F;
            this.trackScore.ColorBorder = System.Drawing.Color.LightGray;
            this.trackScore.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackScore.ColorScale = System.Drawing.Color.LightGray;
            this.trackScore.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorTrack = System.Drawing.Color.LightGray;
            this.trackScore.Decimals = 1;
            this.trackScore.DisabledDesaturateMix = 0.3F;
            this.trackScore.DisabledDimFactor = 0.9F;
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(6, 673);
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
            this.trackScore.Size = new System.Drawing.Size(390, 53);
            this.trackScore.SnapToStep = true;
            this.trackScore.StartWithTextboxHidden = true;
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 80;
            this.trackScore.TextboxFontSize = 22F;
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(6, 648);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(390, 25);
            this.label8.TabIndex = 77;
            this.label8.Text = "Tolerance";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layThreshod
            // 
            this.layThreshod.BackColor = System.Drawing.Color.White;
            this.layThreshod.ColumnCount = 2;
            this.layThreshod.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layThreshod.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layThreshod.Controls.Add(this.label11, 0, 0);
            this.layThreshod.Controls.Add(this.AdjThreshod, 1, 0);
            this.layThreshod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layThreshod.Enabled = false;
            this.layThreshod.Location = new System.Drawing.Point(6, 473);
            this.layThreshod.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layThreshod.Name = "layThreshod";
            this.layThreshod.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.layThreshod.RowCount = 1;
            this.layThreshod.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layThreshod.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layThreshod.Size = new System.Drawing.Size(390, 50);
            this.layThreshod.TabIndex = 76;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(5, 5);
            this.label11.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 32);
            this.label11.TabIndex = 75;
            this.label11.Text = "Threshod";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AdjThreshod
            // 
            this.AdjThreshod.AutoRepeatAccelDeltaMs = -5;
            this.AdjThreshod.AutoRepeatAccelerate = true;
            this.AdjThreshod.AutoRepeatEnabled = true;
            this.AdjThreshod.AutoRepeatInitialDelay = 400;
            this.AdjThreshod.AutoRepeatInterval = 60;
            this.AdjThreshod.AutoRepeatMinInterval = 20;
            this.AdjThreshod.AutoShowTextbox = true;
            this.AdjThreshod.AutoSizeTextbox = true;
            this.AdjThreshod.BackColor = System.Drawing.Color.White;
            this.AdjThreshod.BarLeftGap = 20;
            this.AdjThreshod.BarRightGap = 6;
            this.AdjThreshod.ChromeGap = 1;
            this.AdjThreshod.ChromeWidthRatio = 0.14F;
            this.AdjThreshod.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjThreshod.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjThreshod.ColorScale = System.Drawing.Color.LightGray;
            this.AdjThreshod.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjThreshod.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjThreshod.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjThreshod.Decimals = 0;
            this.AdjThreshod.DisabledDesaturateMix = 0.3F;
            this.AdjThreshod.DisabledDimFactor = 0.9F;
            this.AdjThreshod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjThreshod.EdgePadding = 2;
            this.AdjThreshod.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjThreshod.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjThreshod.KeyboardStep = 1F;
            this.AdjThreshod.Location = new System.Drawing.Point(100, 0);
            this.AdjThreshod.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.AdjThreshod.MatchTextboxFontToThumb = true;
            this.AdjThreshod.Max = 255F;
            this.AdjThreshod.MaxTextboxWidth = 0;
            this.AdjThreshod.MaxThumb = 1000;
            this.AdjThreshod.MaxTrackHeight = 1000;
            this.AdjThreshod.Min = 0F;
            this.AdjThreshod.MinChromeWidth = 64;
            this.AdjThreshod.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjThreshod.MinTextboxWidth = 32;
            this.AdjThreshod.MinThumb = 30;
            this.AdjThreshod.MinTrackHeight = 8;
            this.AdjThreshod.Name = "AdjThreshod";
            this.AdjThreshod.Radius = 8;
            this.AdjThreshod.ShowValueOnThumb = true;
            this.AdjThreshod.Size = new System.Drawing.Size(285, 40);
            this.AdjThreshod.SnapToStep = true;
            this.AdjThreshod.StartWithTextboxHidden = true;
            this.AdjThreshod.Step = 1F;
            this.AdjThreshod.TabIndex = 74;
            this.AdjThreshod.TextboxFontSize = 18F;
            this.AdjThreshod.TextboxSidePadding = 10;
            this.AdjThreshod.TextboxWidth = 600;
            this.AdjThreshod.ThumbDiameterRatio = 2F;
            this.AdjThreshod.ThumbValueBold = true;
            this.AdjThreshod.ThumbValueFontScale = 1.5F;
            this.AdjThreshod.ThumbValuePadding = 0;
            this.AdjThreshod.TightEdges = true;
            this.AdjThreshod.TrackHeightRatio = 0.45F;
            this.AdjThreshod.TrackWidthRatio = 1F;
            this.AdjThreshod.UnitText = "";
            this.AdjThreshod.Value = 0F;
            this.AdjThreshod.WheelStep = 1F;
            this.AdjThreshod.ValueChanged += new System.Action<float>(this.AdjThreshod_ValueChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(6, 387);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(390, 32);
            this.label2.TabIndex = 75;
            this.label2.Text = "Extract Edge";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(6, 292);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(390, 32);
            this.label3.TabIndex = 74;
            this.label3.Text = "Gaussian Smooth";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(6, 197);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(390, 32);
            this.label10.TabIndex = 73;
            this.label10.Text = "Magin";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(6, 101);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(390, 32);
            this.label1.TabIndex = 72;
            this.label1.Text = "Direction Pitch";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(6, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(390, 32);
            this.label7.TabIndex = 71;
            this.label7.Text = "Search Range";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnHori, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnVer, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 133);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(390, 54);
            this.tableLayoutPanel3.TabIndex = 63;
            // 
            // btnHori
            // 
            this.btnHori.AutoFont = true;
            this.btnHori.AutoFontHeightRatio = 0.75F;
            this.btnHori.AutoFontMax = 100F;
            this.btnHori.AutoFontMin = 6F;
            this.btnHori.AutoFontWidthRatio = 0.92F;
            this.btnHori.AutoImage = true;
            this.btnHori.AutoImageMaxRatio = 0.75F;
            this.btnHori.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnHori.AutoImageTint = true;
            this.btnHori.BackColor = System.Drawing.Color.White;
            this.btnHori.BackgroundColor = System.Drawing.Color.White;
            this.btnHori.BorderColor = System.Drawing.Color.White;
            this.btnHori.BorderRadius = 10;
            this.btnHori.BorderSize = 1;
            this.btnHori.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnHori.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnHori.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnHori.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnHori.Corner = BeeGlobal.Corner.Right;
            this.btnHori.DebounceResizeMs = 16;
            this.btnHori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHori.FlatAppearance.BorderSize = 0;
            this.btnHori.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHori.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnHori.ForeColor = System.Drawing.Color.Black;
            this.btnHori.Image = null;
            this.btnHori.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHori.ImageDisabled = null;
            this.btnHori.ImageHover = null;
            this.btnHori.ImageNormal = null;
            this.btnHori.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnHori.ImagePressed = null;
            this.btnHori.ImageTextSpacing = 6;
            this.btnHori.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnHori.ImageTintHover = System.Drawing.Color.Empty;
            this.btnHori.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnHori.ImageTintOpacity = 0.5F;
            this.btnHori.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnHori.IsCLick = false;
            this.btnHori.IsNotChange = false;
            this.btnHori.IsRect = false;
            this.btnHori.IsTouch = false;
            this.btnHori.IsUnGroup = false;
            this.btnHori.Location = new System.Drawing.Point(195, 5);
            this.btnHori.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnHori.Multiline = false;
            this.btnHori.Name = "btnHori";
            this.btnHori.Size = new System.Drawing.Size(187, 44);
            this.btnHori.TabIndex = 3;
            this.btnHori.Text = "Horizontal";
            this.btnHori.TextColor = System.Drawing.Color.Black;
            this.btnHori.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHori.UseVisualStyleBackColor = false;
            this.btnHori.Click += new System.EventHandler(this.btnHori_Click);
            // 
            // btnVer
            // 
            this.btnVer.AutoFont = true;
            this.btnVer.AutoFontHeightRatio = 0.75F;
            this.btnVer.AutoFontMax = 100F;
            this.btnVer.AutoFontMin = 6F;
            this.btnVer.AutoFontWidthRatio = 0.92F;
            this.btnVer.AutoImage = true;
            this.btnVer.AutoImageMaxRatio = 0.75F;
            this.btnVer.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnVer.AutoImageTint = true;
            this.btnVer.BackColor = System.Drawing.Color.White;
            this.btnVer.BackgroundColor = System.Drawing.Color.White;
            this.btnVer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVer.BorderColor = System.Drawing.Color.White;
            this.btnVer.BorderRadius = 10;
            this.btnVer.BorderSize = 1;
            this.btnVer.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnVer.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnVer.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnVer.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnVer.Corner = BeeGlobal.Corner.Left;
            this.btnVer.DebounceResizeMs = 16;
            this.btnVer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVer.FlatAppearance.BorderSize = 0;
            this.btnVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnVer.ForeColor = System.Drawing.Color.Black;
            this.btnVer.Image = null;
            this.btnVer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVer.ImageDisabled = null;
            this.btnVer.ImageHover = null;
            this.btnVer.ImageNormal = null;
            this.btnVer.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnVer.ImagePressed = null;
            this.btnVer.ImageTextSpacing = 6;
            this.btnVer.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnVer.ImageTintHover = System.Drawing.Color.Empty;
            this.btnVer.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnVer.ImageTintOpacity = 0.5F;
            this.btnVer.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnVer.IsCLick = true;
            this.btnVer.IsNotChange = false;
            this.btnVer.IsRect = false;
            this.btnVer.IsTouch = false;
            this.btnVer.IsUnGroup = false;
            this.btnVer.Location = new System.Drawing.Point(8, 5);
            this.btnVer.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnVer.Multiline = false;
            this.btnVer.Name = "btnVer";
            this.btnVer.Size = new System.Drawing.Size(187, 44);
            this.btnVer.TabIndex = 2;
            this.btnVer.Text = "Vertical";
            this.btnVer.TextColor = System.Drawing.Color.Black;
            this.btnVer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVer.UseVisualStyleBackColor = false;
            this.btnVer.Click += new System.EventHandler(this.btnVer_Click);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel15.ColumnCount = 4;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel15.Controls.Add(this.btnInvert, 3, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnBinary, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnStrongEdge, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnCloseEdge, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(6, 419);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(390, 54);
            this.tableLayoutPanel15.TabIndex = 62;
            // 
            // btnInvert
            // 
            this.btnInvert.AutoFont = true;
            this.btnInvert.AutoFontHeightRatio = 0.75F;
            this.btnInvert.AutoFontMax = 100F;
            this.btnInvert.AutoFontMin = 6F;
            this.btnInvert.AutoFontWidthRatio = 0.92F;
            this.btnInvert.AutoImage = true;
            this.btnInvert.AutoImageMaxRatio = 0.75F;
            this.btnInvert.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnInvert.AutoImageTint = true;
            this.btnInvert.BackColor = System.Drawing.Color.White;
            this.btnInvert.BackgroundColor = System.Drawing.Color.White;
            this.btnInvert.BorderColor = System.Drawing.Color.White;
            this.btnInvert.BorderRadius = 10;
            this.btnInvert.BorderSize = 1;
            this.btnInvert.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnInvert.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnInvert.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnInvert.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnInvert.Corner = BeeGlobal.Corner.Right;
            this.btnInvert.DebounceResizeMs = 16;
            this.btnInvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInvert.FlatAppearance.BorderSize = 0;
            this.btnInvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnInvert.ForeColor = System.Drawing.Color.Black;
            this.btnInvert.Image = null;
            this.btnInvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInvert.ImageDisabled = null;
            this.btnInvert.ImageHover = null;
            this.btnInvert.ImageNormal = null;
            this.btnInvert.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnInvert.ImagePressed = null;
            this.btnInvert.ImageTextSpacing = 6;
            this.btnInvert.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnInvert.ImageTintHover = System.Drawing.Color.Empty;
            this.btnInvert.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnInvert.ImageTintOpacity = 0.5F;
            this.btnInvert.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnInvert.IsCLick = false;
            this.btnInvert.IsNotChange = false;
            this.btnInvert.IsRect = false;
            this.btnInvert.IsTouch = false;
            this.btnInvert.IsUnGroup = false;
            this.btnInvert.Location = new System.Drawing.Point(290, 5);
            this.btnInvert.Margin = new System.Windows.Forms.Padding(0);
            this.btnInvert.Multiline = false;
            this.btnInvert.Name = "btnInvert";
            this.btnInvert.Size = new System.Drawing.Size(95, 44);
            this.btnInvert.TabIndex = 5;
            this.btnInvert.Text = "Invert";
            this.btnInvert.TextColor = System.Drawing.Color.Black;
            this.btnInvert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInvert.UseVisualStyleBackColor = false;
            this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
            // 
            // btnBinary
            // 
            this.btnBinary.AutoFont = true;
            this.btnBinary.AutoFontHeightRatio = 0.75F;
            this.btnBinary.AutoFontMax = 100F;
            this.btnBinary.AutoFontMin = 6F;
            this.btnBinary.AutoFontWidthRatio = 0.92F;
            this.btnBinary.AutoImage = true;
            this.btnBinary.AutoImageMaxRatio = 0.75F;
            this.btnBinary.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnBinary.AutoImageTint = true;
            this.btnBinary.BackColor = System.Drawing.Color.White;
            this.btnBinary.BackgroundColor = System.Drawing.Color.White;
            this.btnBinary.BorderColor = System.Drawing.Color.White;
            this.btnBinary.BorderRadius = 10;
            this.btnBinary.BorderSize = 1;
            this.btnBinary.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnBinary.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnBinary.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnBinary.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnBinary.Corner = BeeGlobal.Corner.None;
            this.btnBinary.DebounceResizeMs = 16;
            this.btnBinary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBinary.FlatAppearance.BorderSize = 0;
            this.btnBinary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBinary.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnBinary.ForeColor = System.Drawing.Color.Black;
            this.btnBinary.Image = null;
            this.btnBinary.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBinary.ImageDisabled = null;
            this.btnBinary.ImageHover = null;
            this.btnBinary.ImageNormal = null;
            this.btnBinary.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnBinary.ImagePressed = null;
            this.btnBinary.ImageTextSpacing = 6;
            this.btnBinary.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnBinary.ImageTintHover = System.Drawing.Color.Empty;
            this.btnBinary.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnBinary.ImageTintOpacity = 0.5F;
            this.btnBinary.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnBinary.IsCLick = false;
            this.btnBinary.IsNotChange = false;
            this.btnBinary.IsRect = false;
            this.btnBinary.IsTouch = false;
            this.btnBinary.IsUnGroup = false;
            this.btnBinary.Location = new System.Drawing.Point(195, 5);
            this.btnBinary.Margin = new System.Windows.Forms.Padding(0);
            this.btnBinary.Multiline = false;
            this.btnBinary.Name = "btnBinary";
            this.btnBinary.Size = new System.Drawing.Size(95, 44);
            this.btnBinary.TabIndex = 4;
            this.btnBinary.Text = "Binay";
            this.btnBinary.TextColor = System.Drawing.Color.Black;
            this.btnBinary.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBinary.UseVisualStyleBackColor = false;
            this.btnBinary.Click += new System.EventHandler(this.btnBinary_Click);
            // 
            // btnStrongEdge
            // 
            this.btnStrongEdge.AutoFont = true;
            this.btnStrongEdge.AutoFontHeightRatio = 0.75F;
            this.btnStrongEdge.AutoFontMax = 100F;
            this.btnStrongEdge.AutoFontMin = 6F;
            this.btnStrongEdge.AutoFontWidthRatio = 0.92F;
            this.btnStrongEdge.AutoImage = true;
            this.btnStrongEdge.AutoImageMaxRatio = 0.75F;
            this.btnStrongEdge.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnStrongEdge.AutoImageTint = true;
            this.btnStrongEdge.BackColor = System.Drawing.Color.White;
            this.btnStrongEdge.BackgroundColor = System.Drawing.Color.White;
            this.btnStrongEdge.BorderColor = System.Drawing.Color.White;
            this.btnStrongEdge.BorderRadius = 10;
            this.btnStrongEdge.BorderSize = 1;
            this.btnStrongEdge.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnStrongEdge.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnStrongEdge.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnStrongEdge.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnStrongEdge.Corner = BeeGlobal.Corner.None;
            this.btnStrongEdge.DebounceResizeMs = 16;
            this.btnStrongEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStrongEdge.FlatAppearance.BorderSize = 0;
            this.btnStrongEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStrongEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnStrongEdge.ForeColor = System.Drawing.Color.Black;
            this.btnStrongEdge.Image = null;
            this.btnStrongEdge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStrongEdge.ImageDisabled = null;
            this.btnStrongEdge.ImageHover = null;
            this.btnStrongEdge.ImageNormal = null;
            this.btnStrongEdge.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnStrongEdge.ImagePressed = null;
            this.btnStrongEdge.ImageTextSpacing = 6;
            this.btnStrongEdge.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnStrongEdge.ImageTintHover = System.Drawing.Color.Empty;
            this.btnStrongEdge.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnStrongEdge.ImageTintOpacity = 0.5F;
            this.btnStrongEdge.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnStrongEdge.IsCLick = false;
            this.btnStrongEdge.IsNotChange = false;
            this.btnStrongEdge.IsRect = false;
            this.btnStrongEdge.IsTouch = false;
            this.btnStrongEdge.IsUnGroup = false;
            this.btnStrongEdge.Location = new System.Drawing.Point(100, 5);
            this.btnStrongEdge.Margin = new System.Windows.Forms.Padding(0);
            this.btnStrongEdge.Multiline = false;
            this.btnStrongEdge.Name = "btnStrongEdge";
            this.btnStrongEdge.Size = new System.Drawing.Size(95, 44);
            this.btnStrongEdge.TabIndex = 3;
            this.btnStrongEdge.Text = "Strong";
            this.btnStrongEdge.TextColor = System.Drawing.Color.Black;
            this.btnStrongEdge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStrongEdge.UseVisualStyleBackColor = false;
            this.btnStrongEdge.Click += new System.EventHandler(this.btnStrongEdge_Click);
            // 
            // btnCloseEdge
            // 
            this.btnCloseEdge.AutoFont = true;
            this.btnCloseEdge.AutoFontHeightRatio = 0.75F;
            this.btnCloseEdge.AutoFontMax = 100F;
            this.btnCloseEdge.AutoFontMin = 6F;
            this.btnCloseEdge.AutoFontWidthRatio = 0.92F;
            this.btnCloseEdge.AutoImage = true;
            this.btnCloseEdge.AutoImageMaxRatio = 0.75F;
            this.btnCloseEdge.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCloseEdge.AutoImageTint = true;
            this.btnCloseEdge.BackColor = System.Drawing.Color.White;
            this.btnCloseEdge.BackgroundColor = System.Drawing.Color.White;
            this.btnCloseEdge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseEdge.BorderColor = System.Drawing.Color.White;
            this.btnCloseEdge.BorderRadius = 10;
            this.btnCloseEdge.BorderSize = 1;
            this.btnCloseEdge.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCloseEdge.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCloseEdge.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCloseEdge.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCloseEdge.Corner = BeeGlobal.Corner.Left;
            this.btnCloseEdge.DebounceResizeMs = 16;
            this.btnCloseEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseEdge.FlatAppearance.BorderSize = 0;
            this.btnCloseEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnCloseEdge.ForeColor = System.Drawing.Color.Black;
            this.btnCloseEdge.Image = null;
            this.btnCloseEdge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseEdge.ImageDisabled = null;
            this.btnCloseEdge.ImageHover = null;
            this.btnCloseEdge.ImageNormal = null;
            this.btnCloseEdge.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCloseEdge.ImagePressed = null;
            this.btnCloseEdge.ImageTextSpacing = 6;
            this.btnCloseEdge.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCloseEdge.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCloseEdge.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCloseEdge.ImageTintOpacity = 0.5F;
            this.btnCloseEdge.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCloseEdge.IsCLick = true;
            this.btnCloseEdge.IsNotChange = false;
            this.btnCloseEdge.IsRect = false;
            this.btnCloseEdge.IsTouch = false;
            this.btnCloseEdge.IsUnGroup = false;
            this.btnCloseEdge.Location = new System.Drawing.Point(5, 5);
            this.btnCloseEdge.Margin = new System.Windows.Forms.Padding(0);
            this.btnCloseEdge.Multiline = false;
            this.btnCloseEdge.Name = "btnCloseEdge";
            this.btnCloseEdge.Size = new System.Drawing.Size(95, 44);
            this.btnCloseEdge.TabIndex = 2;
            this.btnCloseEdge.Text = "Close";
            this.btnCloseEdge.TextColor = System.Drawing.Color.Black;
            this.btnCloseEdge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseEdge.UseVisualStyleBackColor = false;
            this.btnCloseEdge.Click += new System.EventHandler(this.btnCloseEdge_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnCropFull, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCropHalt, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 37);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(390, 54);
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
            this.btnCropFull.BackColor = System.Drawing.Color.White;
            this.btnCropFull.BackgroundColor = System.Drawing.Color.White;
            this.btnCropFull.BorderColor = System.Drawing.Color.White;
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
            this.btnCropFull.IsTouch = false;
            this.btnCropFull.IsUnGroup = false;
            this.btnCropFull.Location = new System.Drawing.Point(195, 5);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Multiline = false;
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(187, 44);
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
            this.btnCropHalt.BackColor = System.Drawing.Color.White;
            this.btnCropHalt.BackgroundColor = System.Drawing.Color.White;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.White;
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
            this.btnCropHalt.IsTouch = false;
            this.btnCropHalt.IsUnGroup = false;
            this.btnCropHalt.Location = new System.Drawing.Point(8, 5);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCropHalt.Multiline = false;
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(187, 44);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(420, 766);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Limit";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.AutoScroll = true;
            this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.btnCalib, 0, 12);
            this.tableLayoutPanel8.Controls.Add(this.layLimitCouter, 0, 9);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel17, 0, 11);
            this.tableLayoutPanel8.Controls.Add(this.autoFontLabel17, 0, 14);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel16, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.label9, 0, 13);
            this.tableLayoutPanel8.Controls.Add(this.label5, 0, 8);
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel6, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label17, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.label16, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel4, 0, 10);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel8.RowCount = 17;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(414, 760);
            this.tableLayoutPanel8.TabIndex = 1;
            // 
            // btnCalib
            // 
            this.btnCalib.AutoFont = false;
            this.btnCalib.AutoFontHeightRatio = 0.75F;
            this.btnCalib.AutoFontMax = 100F;
            this.btnCalib.AutoFontMin = 6F;
            this.btnCalib.AutoFontWidthRatio = 0.92F;
            this.btnCalib.AutoImage = true;
            this.btnCalib.AutoImageMaxRatio = 0.75F;
            this.btnCalib.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCalib.AutoImageTint = true;
            this.btnCalib.BackColor = System.Drawing.SystemColors.Control;
            this.btnCalib.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCalib.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCalib.BorderRadius = 10;
            this.btnCalib.BorderSize = 1;
            this.btnCalib.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCalib.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCalib.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCalib.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCalib.Corner = BeeGlobal.Corner.Both;
            this.btnCalib.DebounceResizeMs = 16;
            this.btnCalib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalib.FlatAppearance.BorderSize = 0;
            this.btnCalib.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalib.ForeColor = System.Drawing.Color.Black;
            this.btnCalib.Image = null;
            this.btnCalib.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCalib.ImageDisabled = null;
            this.btnCalib.ImageHover = null;
            this.btnCalib.ImageNormal = null;
            this.btnCalib.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCalib.ImagePressed = null;
            this.btnCalib.ImageTextSpacing = 6;
            this.btnCalib.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCalib.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCalib.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCalib.ImageTintOpacity = 0.5F;
            this.btnCalib.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCalib.IsCLick = false;
            this.btnCalib.IsNotChange = true;
            this.btnCalib.IsRect = false;
            this.btnCalib.IsTouch = false;
            this.btnCalib.IsUnGroup = true;
            this.btnCalib.Location = new System.Drawing.Point(11, 632);
            this.btnCalib.Margin = new System.Windows.Forms.Padding(10);
            this.btnCalib.Multiline = false;
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(388, 80);
            this.btnCalib.TabIndex = 114;
            this.btnCalib.Text = "Set Sample";
            this.btnCalib.TextColor = System.Drawing.Color.Black;
            this.btnCalib.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCalib.UseVisualStyleBackColor = false;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // layLimitCouter
            // 
            this.layLimitCouter.BackColor = System.Drawing.Color.White;
            this.layLimitCouter.ColumnCount = 3;
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layLimitCouter.Controls.Add(this.btnMore, 2, 0);
            this.layLimitCouter.Controls.Add(this.btnEqual, 1, 0);
            this.layLimitCouter.Controls.Add(this.btnLess, 0, 0);
            this.layLimitCouter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layLimitCouter.Location = new System.Drawing.Point(6, 463);
            this.layLimitCouter.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layLimitCouter.Name = "layLimitCouter";
            this.layLimitCouter.Padding = new System.Windows.Forms.Padding(5);
            this.layLimitCouter.RowCount = 1;
            this.layLimitCouter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layLimitCouter.Size = new System.Drawing.Size(398, 55);
            this.layLimitCouter.TabIndex = 113;
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
            this.btnMore.IsTouch = false;
            this.btnMore.IsUnGroup = false;
            this.btnMore.Location = new System.Drawing.Point(263, 5);
            this.btnMore.Margin = new System.Windows.Forms.Padding(0);
            this.btnMore.Multiline = false;
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(130, 45);
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
            this.btnEqual.IsTouch = false;
            this.btnEqual.IsUnGroup = false;
            this.btnEqual.Location = new System.Drawing.Point(134, 5);
            this.btnEqual.Margin = new System.Windows.Forms.Padding(0);
            this.btnEqual.Multiline = false;
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(129, 45);
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
            this.btnLess.IsTouch = false;
            this.btnLess.IsUnGroup = false;
            this.btnLess.Location = new System.Drawing.Point(5, 5);
            this.btnLess.Margin = new System.Windows.Forms.Padding(0);
            this.btnLess.Multiline = false;
            this.btnLess.Name = "btnLess";
            this.btnLess.Size = new System.Drawing.Size(129, 45);
            this.btnLess.TabIndex = 31;
            this.btnLess.Text = "Less";
            this.btnLess.TextColor = System.Drawing.Color.Black;
            this.btnLess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLess.UseVisualStyleBackColor = false;
            this.btnLess.Click += new System.EventHandler(this.btnLess_Click);
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel17.ColumnCount = 3;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel17.Controls.Add(this.numRootCounter, 2, 0);
            this.tableLayoutPanel17.Controls.Add(this.btnEnRootCounter, 0, 0);
            this.tableLayoutPanel17.Controls.Add(this.lbRootCount, 1, 0);
            this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel17.Location = new System.Drawing.Point(6, 568);
            this.tableLayoutPanel17.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel17.RowCount = 1;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel17.Size = new System.Drawing.Size(398, 54);
            this.tableLayoutPanel17.TabIndex = 111;
            // 
            // numRootCounter
            // 
            this.numRootCounter.AutoShowTextbox = false;
            this.numRootCounter.AutoSizeTextbox = true;
            this.numRootCounter.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numRootCounter.BackColor = System.Drawing.SystemColors.Control;
            this.numRootCounter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numRootCounter.BorderRadius = 6;
            this.numRootCounter.ButtonMaxSize = 64;
            this.numRootCounter.ButtonMinSize = 24;
            this.numRootCounter.Decimals = 0;
            this.numRootCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numRootCounter.ElementGap = 6;
            this.numRootCounter.FillTextboxToAvailable = true;
            this.numRootCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numRootCounter.ForeColor = System.Drawing.Color.Red;
            this.numRootCounter.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numRootCounter.KeyboardStep = 1F;
            this.numRootCounter.Location = new System.Drawing.Point(263, 5);
            this.numRootCounter.Margin = new System.Windows.Forms.Padding(0);
            this.numRootCounter.Max = 99F;
            this.numRootCounter.MaxTextboxWidth = 0;
            this.numRootCounter.Min = 1F;
            this.numRootCounter.MinimumSize = new System.Drawing.Size(120, 32);
            this.numRootCounter.MinTextboxWidth = 16;
            this.numRootCounter.Name = "numRootCounter";
            this.numRootCounter.Size = new System.Drawing.Size(130, 44);
            this.numRootCounter.SnapToStep = true;
            this.numRootCounter.StartWithTextboxHidden = false;
            this.numRootCounter.Step = 1F;
            this.numRootCounter.TabIndex = 116;
            this.numRootCounter.TextboxFontSize = 20F;
            this.numRootCounter.TextboxSidePadding = 12;
            this.numRootCounter.TextboxWidth = 56;
            this.numRootCounter.UnitText = "";
            this.numRootCounter.Value = 99F;
            this.numRootCounter.WheelStep = 1F;
            this.numRootCounter.ValueChanged += new System.Action<float>(this.numRootCounter_ValueChanged);
            // 
            // btnEnRootCounter
            // 
            this.btnEnRootCounter.AutoFont = true;
            this.btnEnRootCounter.AutoFontHeightRatio = 0.75F;
            this.btnEnRootCounter.AutoFontMax = 100F;
            this.btnEnRootCounter.AutoFontMin = 6F;
            this.btnEnRootCounter.AutoFontWidthRatio = 0.92F;
            this.btnEnRootCounter.AutoImage = true;
            this.btnEnRootCounter.AutoImageMaxRatio = 0.75F;
            this.btnEnRootCounter.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnRootCounter.AutoImageTint = true;
            this.btnEnRootCounter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEnRootCounter.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEnRootCounter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnRootCounter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEnRootCounter.BorderRadius = 10;
            this.btnEnRootCounter.BorderSize = 1;
            this.btnEnRootCounter.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnRootCounter.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnRootCounter.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnRootCounter.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnRootCounter.Corner = BeeGlobal.Corner.Left;
            this.btnEnRootCounter.DebounceResizeMs = 16;
            this.btnEnRootCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnRootCounter.FlatAppearance.BorderSize = 0;
            this.btnEnRootCounter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnRootCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnEnRootCounter.ForeColor = System.Drawing.Color.Black;
            this.btnEnRootCounter.Image = null;
            this.btnEnRootCounter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnRootCounter.ImageDisabled = null;
            this.btnEnRootCounter.ImageHover = null;
            this.btnEnRootCounter.ImageNormal = null;
            this.btnEnRootCounter.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnRootCounter.ImagePressed = null;
            this.btnEnRootCounter.ImageTextSpacing = 6;
            this.btnEnRootCounter.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnRootCounter.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnRootCounter.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnRootCounter.ImageTintOpacity = 0.5F;
            this.btnEnRootCounter.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnRootCounter.IsCLick = false;
            this.btnEnRootCounter.IsNotChange = false;
            this.btnEnRootCounter.IsRect = false;
            this.btnEnRootCounter.IsTouch = false;
            this.btnEnRootCounter.IsUnGroup = true;
            this.btnEnRootCounter.Location = new System.Drawing.Point(5, 5);
            this.btnEnRootCounter.Margin = new System.Windows.Forms.Padding(0);
            this.btnEnRootCounter.Multiline = false;
            this.btnEnRootCounter.Name = "btnEnRootCounter";
            this.btnEnRootCounter.Size = new System.Drawing.Size(129, 44);
            this.btnEnRootCounter.TabIndex = 115;
            this.btnEnRootCounter.Text = "Root";
            this.btnEnRootCounter.TextColor = System.Drawing.Color.Black;
            this.btnEnRootCounter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnRootCounter.UseVisualStyleBackColor = false;
            this.btnEnRootCounter.Click += new System.EventHandler(this.btnEnRootCounter_Click);
            // 
            // lbRootCount
            // 
            this.lbRootCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRootCount.AutoFont = true;
            this.lbRootCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbRootCount.EnableHorizontalScroll = false;
            this.lbRootCount.EnableVerticalScroll = false;
            this.lbRootCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.83984F);
            this.lbRootCount.Location = new System.Drawing.Point(134, 5);
            this.lbRootCount.Margin = new System.Windows.Forms.Padding(0);
            this.lbRootCount.MaxFontSize = 200F;
            this.lbRootCount.MinFontSize = 6F;
            this.lbRootCount.Name = "lbRootCount";
            this.lbRootCount.Size = new System.Drawing.Size(129, 44);
            this.lbRootCount.TabIndex = 16;
            this.lbRootCount.Text = "0";
            this.lbRootCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel17
            // 
            this.autoFontLabel17.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoFontLabel17.AutoFont = true;
            this.autoFontLabel17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.autoFontLabel17.EnableHorizontalScroll = false;
            this.autoFontLabel17.EnableVerticalScroll = false;
            this.autoFontLabel17.Font = new System.Drawing.Font("Microsoft Sans Serif", 125.7344F);
            this.autoFontLabel17.Location = new System.Drawing.Point(4, 773);
            this.autoFontLabel17.MaxFontSize = 200F;
            this.autoFontLabel17.MinFontSize = 6F;
            this.autoFontLabel17.Name = "autoFontLabel17";
            this.autoFontLabel17.Size = new System.Drawing.Size(402, 194);
            this.autoFontLabel17.TabIndex = 110;
            this.autoFontLabel17.Text = "...";
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel14.ColumnCount = 5;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel14.Controls.Add(this.btnEnCrestHeight, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.lbHeightCrestMean, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.lbHeightCrestMedian, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.lbHeightCrestMin, 3, 0);
            this.tableLayoutPanel14.Controls.Add(this.lbHeightCrestMax, 4, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(6, 307);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(398, 54);
            this.tableLayoutPanel14.TabIndex = 109;
            // 
            // btnEnCrestHeight
            // 
            this.btnEnCrestHeight.AutoFont = true;
            this.btnEnCrestHeight.AutoFontHeightRatio = 0.75F;
            this.btnEnCrestHeight.AutoFontMax = 100F;
            this.btnEnCrestHeight.AutoFontMin = 6F;
            this.btnEnCrestHeight.AutoFontWidthRatio = 0.92F;
            this.btnEnCrestHeight.AutoImage = true;
            this.btnEnCrestHeight.AutoImageMaxRatio = 0.75F;
            this.btnEnCrestHeight.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnCrestHeight.AutoImageTint = true;
            this.btnEnCrestHeight.BackColor = System.Drawing.Color.White;
            this.btnEnCrestHeight.BackgroundColor = System.Drawing.Color.White;
            this.btnEnCrestHeight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnCrestHeight.BorderColor = System.Drawing.Color.White;
            this.btnEnCrestHeight.BorderRadius = 10;
            this.btnEnCrestHeight.BorderSize = 1;
            this.btnEnCrestHeight.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnCrestHeight.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnCrestHeight.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnCrestHeight.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnCrestHeight.Corner = BeeGlobal.Corner.Left;
            this.btnEnCrestHeight.DebounceResizeMs = 16;
            this.btnEnCrestHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnCrestHeight.FlatAppearance.BorderSize = 0;
            this.btnEnCrestHeight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnCrestHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnEnCrestHeight.ForeColor = System.Drawing.Color.Black;
            this.btnEnCrestHeight.Image = null;
            this.btnEnCrestHeight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnCrestHeight.ImageDisabled = null;
            this.btnEnCrestHeight.ImageHover = null;
            this.btnEnCrestHeight.ImageNormal = null;
            this.btnEnCrestHeight.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnCrestHeight.ImagePressed = null;
            this.btnEnCrestHeight.ImageTextSpacing = 6;
            this.btnEnCrestHeight.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnCrestHeight.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnCrestHeight.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnCrestHeight.ImageTintOpacity = 0.5F;
            this.btnEnCrestHeight.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnCrestHeight.IsCLick = false;
            this.btnEnCrestHeight.IsNotChange = false;
            this.btnEnCrestHeight.IsRect = false;
            this.btnEnCrestHeight.IsTouch = false;
            this.btnEnCrestHeight.IsUnGroup = true;
            this.btnEnCrestHeight.Location = new System.Drawing.Point(5, 5);
            this.btnEnCrestHeight.Margin = new System.Windows.Forms.Padding(0);
            this.btnEnCrestHeight.Multiline = false;
            this.btnEnCrestHeight.Name = "btnEnCrestHeight";
            this.btnEnCrestHeight.Size = new System.Drawing.Size(77, 44);
            this.btnEnCrestHeight.TabIndex = 18;
            this.btnEnCrestHeight.Text = "Crest";
            this.btnEnCrestHeight.TextColor = System.Drawing.Color.Black;
            this.btnEnCrestHeight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnCrestHeight.UseVisualStyleBackColor = false;
            this.btnEnCrestHeight.Click += new System.EventHandler(this.btnEnCreshHeight_Click);
            // 
            // lbHeightCrestMean
            // 
            this.lbHeightCrestMean.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbHeightCrestMean.AutoFont = true;
            this.lbHeightCrestMean.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbHeightCrestMean.EnableHorizontalScroll = false;
            this.lbHeightCrestMean.EnableVerticalScroll = false;
            this.lbHeightCrestMean.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbHeightCrestMean.Location = new System.Drawing.Point(85, 8);
            this.lbHeightCrestMean.MaxFontSize = 200F;
            this.lbHeightCrestMean.MinFontSize = 6F;
            this.lbHeightCrestMean.Name = "lbHeightCrestMean";
            this.lbHeightCrestMean.Size = new System.Drawing.Size(71, 38);
            this.lbHeightCrestMean.TabIndex = 17;
            this.lbHeightCrestMean.Text = "--.--";
            this.lbHeightCrestMean.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbHeightCrestMedian
            // 
            this.lbHeightCrestMedian.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbHeightCrestMedian.AutoFont = true;
            this.lbHeightCrestMedian.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbHeightCrestMedian.EnableHorizontalScroll = false;
            this.lbHeightCrestMedian.EnableVerticalScroll = false;
            this.lbHeightCrestMedian.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbHeightCrestMedian.Location = new System.Drawing.Point(162, 8);
            this.lbHeightCrestMedian.MaxFontSize = 200F;
            this.lbHeightCrestMedian.MinFontSize = 6F;
            this.lbHeightCrestMedian.Name = "lbHeightCrestMedian";
            this.lbHeightCrestMedian.Size = new System.Drawing.Size(71, 38);
            this.lbHeightCrestMedian.TabIndex = 16;
            this.lbHeightCrestMedian.Text = "--.--";
            this.lbHeightCrestMedian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbHeightCrestMin
            // 
            this.lbHeightCrestMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbHeightCrestMin.AutoFont = true;
            this.lbHeightCrestMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbHeightCrestMin.EnableHorizontalScroll = false;
            this.lbHeightCrestMin.EnableVerticalScroll = false;
            this.lbHeightCrestMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbHeightCrestMin.Location = new System.Drawing.Point(239, 8);
            this.lbHeightCrestMin.MaxFontSize = 200F;
            this.lbHeightCrestMin.MinFontSize = 6F;
            this.lbHeightCrestMin.Name = "lbHeightCrestMin";
            this.lbHeightCrestMin.Size = new System.Drawing.Size(71, 38);
            this.lbHeightCrestMin.TabIndex = 15;
            this.lbHeightCrestMin.Text = "--.--";
            this.lbHeightCrestMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbHeightCrestMax
            // 
            this.lbHeightCrestMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbHeightCrestMax.AutoFont = true;
            this.lbHeightCrestMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbHeightCrestMax.EnableHorizontalScroll = false;
            this.lbHeightCrestMax.EnableVerticalScroll = false;
            this.lbHeightCrestMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbHeightCrestMax.Location = new System.Drawing.Point(316, 8);
            this.lbHeightCrestMax.MaxFontSize = 200F;
            this.lbHeightCrestMax.MinFontSize = 6F;
            this.lbHeightCrestMax.Name = "lbHeightCrestMax";
            this.lbHeightCrestMax.Size = new System.Drawing.Size(74, 38);
            this.lbHeightCrestMax.TabIndex = 14;
            this.lbHeightCrestMax.Text = "--.--";
            this.lbHeightCrestMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel16.ColumnCount = 5;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel16.Controls.Add(this.btnEnRootHeight, 0, 0);
            this.tableLayoutPanel16.Controls.Add(this.lbHeightRootMean, 1, 0);
            this.tableLayoutPanel16.Controls.Add(this.lbHeightRootMedian, 2, 0);
            this.tableLayoutPanel16.Controls.Add(this.lbHeightRootMin, 3, 0);
            this.tableLayoutPanel16.Controls.Add(this.lbHeightRootMax, 4, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(6, 361);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel16.RowCount = 1;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(398, 54);
            this.tableLayoutPanel16.TabIndex = 108;
            // 
            // btnEnRootHeight
            // 
            this.btnEnRootHeight.AutoFont = true;
            this.btnEnRootHeight.AutoFontHeightRatio = 0.75F;
            this.btnEnRootHeight.AutoFontMax = 100F;
            this.btnEnRootHeight.AutoFontMin = 6F;
            this.btnEnRootHeight.AutoFontWidthRatio = 0.92F;
            this.btnEnRootHeight.AutoImage = true;
            this.btnEnRootHeight.AutoImageMaxRatio = 0.75F;
            this.btnEnRootHeight.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnRootHeight.AutoImageTint = true;
            this.btnEnRootHeight.BackColor = System.Drawing.Color.White;
            this.btnEnRootHeight.BackgroundColor = System.Drawing.Color.White;
            this.btnEnRootHeight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnRootHeight.BorderColor = System.Drawing.Color.White;
            this.btnEnRootHeight.BorderRadius = 10;
            this.btnEnRootHeight.BorderSize = 1;
            this.btnEnRootHeight.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnRootHeight.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnRootHeight.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnRootHeight.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnRootHeight.Corner = BeeGlobal.Corner.Left;
            this.btnEnRootHeight.DebounceResizeMs = 16;
            this.btnEnRootHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnRootHeight.FlatAppearance.BorderSize = 0;
            this.btnEnRootHeight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnRootHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnEnRootHeight.ForeColor = System.Drawing.Color.Black;
            this.btnEnRootHeight.Image = null;
            this.btnEnRootHeight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnRootHeight.ImageDisabled = null;
            this.btnEnRootHeight.ImageHover = null;
            this.btnEnRootHeight.ImageNormal = null;
            this.btnEnRootHeight.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnRootHeight.ImagePressed = null;
            this.btnEnRootHeight.ImageTextSpacing = 6;
            this.btnEnRootHeight.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnRootHeight.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnRootHeight.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnRootHeight.ImageTintOpacity = 0.5F;
            this.btnEnRootHeight.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnRootHeight.IsCLick = false;
            this.btnEnRootHeight.IsNotChange = false;
            this.btnEnRootHeight.IsRect = false;
            this.btnEnRootHeight.IsTouch = false;
            this.btnEnRootHeight.IsUnGroup = true;
            this.btnEnRootHeight.Location = new System.Drawing.Point(5, 5);
            this.btnEnRootHeight.Margin = new System.Windows.Forms.Padding(0);
            this.btnEnRootHeight.Multiline = false;
            this.btnEnRootHeight.Name = "btnEnRootHeight";
            this.btnEnRootHeight.Size = new System.Drawing.Size(77, 44);
            this.btnEnRootHeight.TabIndex = 18;
            this.btnEnRootHeight.Text = "Root";
            this.btnEnRootHeight.TextColor = System.Drawing.Color.Black;
            this.btnEnRootHeight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnRootHeight.UseVisualStyleBackColor = false;
            this.btnEnRootHeight.Click += new System.EventHandler(this.btnEnRootHeight_Click);
            // 
            // lbHeightRootMean
            // 
            this.lbHeightRootMean.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbHeightRootMean.AutoFont = true;
            this.lbHeightRootMean.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbHeightRootMean.EnableHorizontalScroll = false;
            this.lbHeightRootMean.EnableVerticalScroll = false;
            this.lbHeightRootMean.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbHeightRootMean.Location = new System.Drawing.Point(85, 8);
            this.lbHeightRootMean.MaxFontSize = 200F;
            this.lbHeightRootMean.MinFontSize = 6F;
            this.lbHeightRootMean.Name = "lbHeightRootMean";
            this.lbHeightRootMean.Size = new System.Drawing.Size(71, 38);
            this.lbHeightRootMean.TabIndex = 17;
            this.lbHeightRootMean.Text = "--.--";
            this.lbHeightRootMean.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbHeightRootMedian
            // 
            this.lbHeightRootMedian.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbHeightRootMedian.AutoFont = true;
            this.lbHeightRootMedian.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbHeightRootMedian.EnableHorizontalScroll = false;
            this.lbHeightRootMedian.EnableVerticalScroll = false;
            this.lbHeightRootMedian.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbHeightRootMedian.Location = new System.Drawing.Point(162, 8);
            this.lbHeightRootMedian.MaxFontSize = 200F;
            this.lbHeightRootMedian.MinFontSize = 6F;
            this.lbHeightRootMedian.Name = "lbHeightRootMedian";
            this.lbHeightRootMedian.Size = new System.Drawing.Size(71, 38);
            this.lbHeightRootMedian.TabIndex = 16;
            this.lbHeightRootMedian.Text = "--.--";
            this.lbHeightRootMedian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbHeightRootMin
            // 
            this.lbHeightRootMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbHeightRootMin.AutoFont = true;
            this.lbHeightRootMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbHeightRootMin.EnableHorizontalScroll = false;
            this.lbHeightRootMin.EnableVerticalScroll = false;
            this.lbHeightRootMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbHeightRootMin.Location = new System.Drawing.Point(239, 8);
            this.lbHeightRootMin.MaxFontSize = 200F;
            this.lbHeightRootMin.MinFontSize = 6F;
            this.lbHeightRootMin.Name = "lbHeightRootMin";
            this.lbHeightRootMin.Size = new System.Drawing.Size(71, 38);
            this.lbHeightRootMin.TabIndex = 15;
            this.lbHeightRootMin.Text = "--.--";
            this.lbHeightRootMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbHeightRootMax
            // 
            this.lbHeightRootMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbHeightRootMax.AutoFont = true;
            this.lbHeightRootMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbHeightRootMax.EnableHorizontalScroll = false;
            this.lbHeightRootMax.EnableVerticalScroll = false;
            this.lbHeightRootMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbHeightRootMax.Location = new System.Drawing.Point(316, 8);
            this.lbHeightRootMax.MaxFontSize = 200F;
            this.lbHeightRootMax.MinFontSize = 6F;
            this.lbHeightRootMax.Name = "lbHeightRootMax";
            this.lbHeightRootMax.Size = new System.Drawing.Size(74, 38);
            this.lbHeightRootMax.TabIndex = 14;
            this.lbHeightRootMax.Text = "--.--";
            this.lbHeightRootMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel13.ColumnCount = 5;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel13.Controls.Add(this.lbPitchRootMean, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.lbPitchRootMedian, 2, 0);
            this.tableLayoutPanel13.Controls.Add(this.lbPitchRootMin, 3, 0);
            this.tableLayoutPanel13.Controls.Add(this.lbPitchRootMax, 4, 0);
            this.tableLayoutPanel13.Controls.Add(this.btnEnRootPitch, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(6, 205);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(398, 54);
            this.tableLayoutPanel13.TabIndex = 107;
            // 
            // lbPitchRootMean
            // 
            this.lbPitchRootMean.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPitchRootMean.AutoFont = true;
            this.lbPitchRootMean.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPitchRootMean.EnableHorizontalScroll = false;
            this.lbPitchRootMean.EnableVerticalScroll = false;
            this.lbPitchRootMean.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbPitchRootMean.Location = new System.Drawing.Point(85, 8);
            this.lbPitchRootMean.MaxFontSize = 200F;
            this.lbPitchRootMean.MinFontSize = 6F;
            this.lbPitchRootMean.Name = "lbPitchRootMean";
            this.lbPitchRootMean.Size = new System.Drawing.Size(71, 38);
            this.lbPitchRootMean.TabIndex = 17;
            this.lbPitchRootMean.Text = "--.--";
            this.lbPitchRootMean.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPitchRootMedian
            // 
            this.lbPitchRootMedian.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPitchRootMedian.AutoFont = true;
            this.lbPitchRootMedian.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPitchRootMedian.EnableHorizontalScroll = false;
            this.lbPitchRootMedian.EnableVerticalScroll = false;
            this.lbPitchRootMedian.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbPitchRootMedian.Location = new System.Drawing.Point(162, 8);
            this.lbPitchRootMedian.MaxFontSize = 200F;
            this.lbPitchRootMedian.MinFontSize = 6F;
            this.lbPitchRootMedian.Name = "lbPitchRootMedian";
            this.lbPitchRootMedian.Size = new System.Drawing.Size(71, 38);
            this.lbPitchRootMedian.TabIndex = 16;
            this.lbPitchRootMedian.Text = "--.--";
            this.lbPitchRootMedian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPitchRootMin
            // 
            this.lbPitchRootMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPitchRootMin.AutoFont = true;
            this.lbPitchRootMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPitchRootMin.EnableHorizontalScroll = false;
            this.lbPitchRootMin.EnableVerticalScroll = false;
            this.lbPitchRootMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbPitchRootMin.Location = new System.Drawing.Point(239, 8);
            this.lbPitchRootMin.MaxFontSize = 200F;
            this.lbPitchRootMin.MinFontSize = 6F;
            this.lbPitchRootMin.Name = "lbPitchRootMin";
            this.lbPitchRootMin.Size = new System.Drawing.Size(71, 38);
            this.lbPitchRootMin.TabIndex = 15;
            this.lbPitchRootMin.Text = "--.--";
            this.lbPitchRootMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPitchRootMax
            // 
            this.lbPitchRootMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPitchRootMax.AutoFont = true;
            this.lbPitchRootMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPitchRootMax.EnableHorizontalScroll = false;
            this.lbPitchRootMax.EnableVerticalScroll = false;
            this.lbPitchRootMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbPitchRootMax.Location = new System.Drawing.Point(316, 8);
            this.lbPitchRootMax.MaxFontSize = 200F;
            this.lbPitchRootMax.MinFontSize = 6F;
            this.lbPitchRootMax.Name = "lbPitchRootMax";
            this.lbPitchRootMax.Size = new System.Drawing.Size(74, 38);
            this.lbPitchRootMax.TabIndex = 14;
            this.lbPitchRootMax.Text = "--.--";
            this.lbPitchRootMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEnRootPitch
            // 
            this.btnEnRootPitch.AutoFont = true;
            this.btnEnRootPitch.AutoFontHeightRatio = 0.75F;
            this.btnEnRootPitch.AutoFontMax = 100F;
            this.btnEnRootPitch.AutoFontMin = 6F;
            this.btnEnRootPitch.AutoFontWidthRatio = 0.92F;
            this.btnEnRootPitch.AutoImage = true;
            this.btnEnRootPitch.AutoImageMaxRatio = 0.75F;
            this.btnEnRootPitch.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnRootPitch.AutoImageTint = true;
            this.btnEnRootPitch.BackColor = System.Drawing.Color.White;
            this.btnEnRootPitch.BackgroundColor = System.Drawing.Color.White;
            this.btnEnRootPitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnRootPitch.BorderColor = System.Drawing.Color.White;
            this.btnEnRootPitch.BorderRadius = 10;
            this.btnEnRootPitch.BorderSize = 1;
            this.btnEnRootPitch.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnRootPitch.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnRootPitch.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnRootPitch.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnRootPitch.Corner = BeeGlobal.Corner.Left;
            this.btnEnRootPitch.DebounceResizeMs = 16;
            this.btnEnRootPitch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnRootPitch.FlatAppearance.BorderSize = 0;
            this.btnEnRootPitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnRootPitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnEnRootPitch.ForeColor = System.Drawing.Color.Black;
            this.btnEnRootPitch.Image = null;
            this.btnEnRootPitch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnRootPitch.ImageDisabled = null;
            this.btnEnRootPitch.ImageHover = null;
            this.btnEnRootPitch.ImageNormal = null;
            this.btnEnRootPitch.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnRootPitch.ImagePressed = null;
            this.btnEnRootPitch.ImageTextSpacing = 6;
            this.btnEnRootPitch.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnRootPitch.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnRootPitch.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnRootPitch.ImageTintOpacity = 0.5F;
            this.btnEnRootPitch.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnRootPitch.IsCLick = false;
            this.btnEnRootPitch.IsNotChange = false;
            this.btnEnRootPitch.IsRect = false;
            this.btnEnRootPitch.IsTouch = false;
            this.btnEnRootPitch.IsUnGroup = true;
            this.btnEnRootPitch.Location = new System.Drawing.Point(5, 5);
            this.btnEnRootPitch.Margin = new System.Windows.Forms.Padding(0);
            this.btnEnRootPitch.Multiline = false;
            this.btnEnRootPitch.Name = "btnEnRootPitch";
            this.btnEnRootPitch.Size = new System.Drawing.Size(77, 44);
            this.btnEnRootPitch.TabIndex = 2;
            this.btnEnRootPitch.Text = "Root";
            this.btnEnRootPitch.TextColor = System.Drawing.Color.Black;
            this.btnEnRootPitch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnRootPitch.UseVisualStyleBackColor = false;
            this.btnEnRootPitch.Click += new System.EventHandler(this.btnEnRootPitch_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(6, 732);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(398, 38);
            this.label9.TabIndex = 105;
            this.label9.Text = "Infor";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(6, 425);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(398, 38);
            this.label5.TabIndex = 104;
            this.label5.Text = "Counter (N)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(6, 269);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(398, 38);
            this.label4.TabIndex = 100;
            this.label4.Text = "Height";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel6.ColumnCount = 5;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.Controls.Add(this.lbPitchCrestMax, 4, 0);
            this.tableLayoutPanel6.Controls.Add(this.lbPitchCrestMin, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.lbPitchCrestMedian, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnEnCrestPitch, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.lbPitchCrestMean, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(6, 151);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(398, 54);
            this.tableLayoutPanel6.TabIndex = 98;
            // 
            // lbPitchCrestMax
            // 
            this.lbPitchCrestMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPitchCrestMax.AutoFont = true;
            this.lbPitchCrestMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPitchCrestMax.EnableHorizontalScroll = false;
            this.lbPitchCrestMax.EnableVerticalScroll = false;
            this.lbPitchCrestMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbPitchCrestMax.Location = new System.Drawing.Point(316, 8);
            this.lbPitchCrestMax.MaxFontSize = 200F;
            this.lbPitchCrestMax.MinFontSize = 6F;
            this.lbPitchCrestMax.Name = "lbPitchCrestMax";
            this.lbPitchCrestMax.Size = new System.Drawing.Size(74, 38);
            this.lbPitchCrestMax.TabIndex = 13;
            this.lbPitchCrestMax.Text = "--.--";
            this.lbPitchCrestMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPitchCrestMin
            // 
            this.lbPitchCrestMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPitchCrestMin.AutoFont = true;
            this.lbPitchCrestMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPitchCrestMin.EnableHorizontalScroll = false;
            this.lbPitchCrestMin.EnableVerticalScroll = false;
            this.lbPitchCrestMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbPitchCrestMin.Location = new System.Drawing.Point(239, 8);
            this.lbPitchCrestMin.MaxFontSize = 200F;
            this.lbPitchCrestMin.MinFontSize = 6F;
            this.lbPitchCrestMin.Name = "lbPitchCrestMin";
            this.lbPitchCrestMin.Size = new System.Drawing.Size(71, 38);
            this.lbPitchCrestMin.TabIndex = 12;
            this.lbPitchCrestMin.Text = "--.--";
            this.lbPitchCrestMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPitchCrestMedian
            // 
            this.lbPitchCrestMedian.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPitchCrestMedian.AutoFont = true;
            this.lbPitchCrestMedian.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPitchCrestMedian.EnableHorizontalScroll = false;
            this.lbPitchCrestMedian.EnableVerticalScroll = false;
            this.lbPitchCrestMedian.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbPitchCrestMedian.Location = new System.Drawing.Point(162, 8);
            this.lbPitchCrestMedian.MaxFontSize = 200F;
            this.lbPitchCrestMedian.MinFontSize = 6F;
            this.lbPitchCrestMedian.Name = "lbPitchCrestMedian";
            this.lbPitchCrestMedian.Size = new System.Drawing.Size(71, 38);
            this.lbPitchCrestMedian.TabIndex = 11;
            this.lbPitchCrestMedian.Text = "--.--";
            this.lbPitchCrestMedian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEnCrestPitch
            // 
            this.btnEnCrestPitch.AutoFont = true;
            this.btnEnCrestPitch.AutoFontHeightRatio = 0.75F;
            this.btnEnCrestPitch.AutoFontMax = 100F;
            this.btnEnCrestPitch.AutoFontMin = 6F;
            this.btnEnCrestPitch.AutoFontWidthRatio = 0.92F;
            this.btnEnCrestPitch.AutoImage = true;
            this.btnEnCrestPitch.AutoImageMaxRatio = 0.75F;
            this.btnEnCrestPitch.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnCrestPitch.AutoImageTint = true;
            this.btnEnCrestPitch.BackColor = System.Drawing.Color.White;
            this.btnEnCrestPitch.BackgroundColor = System.Drawing.Color.White;
            this.btnEnCrestPitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnCrestPitch.BorderColor = System.Drawing.Color.White;
            this.btnEnCrestPitch.BorderRadius = 10;
            this.btnEnCrestPitch.BorderSize = 1;
            this.btnEnCrestPitch.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnCrestPitch.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnCrestPitch.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnCrestPitch.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnCrestPitch.Corner = BeeGlobal.Corner.Left;
            this.btnEnCrestPitch.DebounceResizeMs = 16;
            this.btnEnCrestPitch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnCrestPitch.FlatAppearance.BorderSize = 0;
            this.btnEnCrestPitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnCrestPitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnEnCrestPitch.ForeColor = System.Drawing.Color.Black;
            this.btnEnCrestPitch.Image = null;
            this.btnEnCrestPitch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnCrestPitch.ImageDisabled = null;
            this.btnEnCrestPitch.ImageHover = null;
            this.btnEnCrestPitch.ImageNormal = null;
            this.btnEnCrestPitch.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnCrestPitch.ImagePressed = null;
            this.btnEnCrestPitch.ImageTextSpacing = 6;
            this.btnEnCrestPitch.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnCrestPitch.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnCrestPitch.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnCrestPitch.ImageTintOpacity = 0.5F;
            this.btnEnCrestPitch.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnCrestPitch.IsCLick = false;
            this.btnEnCrestPitch.IsNotChange = false;
            this.btnEnCrestPitch.IsRect = false;
            this.btnEnCrestPitch.IsTouch = false;
            this.btnEnCrestPitch.IsUnGroup = true;
            this.btnEnCrestPitch.Location = new System.Drawing.Point(5, 5);
            this.btnEnCrestPitch.Margin = new System.Windows.Forms.Padding(0);
            this.btnEnCrestPitch.Multiline = false;
            this.btnEnCrestPitch.Name = "btnEnCrestPitch";
            this.btnEnCrestPitch.Size = new System.Drawing.Size(77, 44);
            this.btnEnCrestPitch.TabIndex = 2;
            this.btnEnCrestPitch.Text = "Crest";
            this.btnEnCrestPitch.TextColor = System.Drawing.Color.Black;
            this.btnEnCrestPitch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnCrestPitch.UseVisualStyleBackColor = false;
            this.btnEnCrestPitch.Click += new System.EventHandler(this.btnEnCrestPitch_Click);
            // 
            // lbPitchCrestMean
            // 
            this.lbPitchCrestMean.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPitchCrestMean.AutoFont = true;
            this.lbPitchCrestMean.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPitchCrestMean.EnableHorizontalScroll = false;
            this.lbPitchCrestMean.EnableVerticalScroll = false;
            this.lbPitchCrestMean.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.53516F);
            this.lbPitchCrestMean.Location = new System.Drawing.Point(85, 8);
            this.lbPitchCrestMean.MaxFontSize = 200F;
            this.lbPitchCrestMean.MinFontSize = 6F;
            this.lbPitchCrestMean.Name = "lbPitchCrestMean";
            this.lbPitchCrestMean.Size = new System.Drawing.Size(71, 38);
            this.lbPitchCrestMean.TabIndex = 10;
            this.lbPitchCrestMean.Text = "--.--";
            this.lbPitchCrestMean.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel5.ColumnCount = 5;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.Controls.Add(this.btnMax, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnMin, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnMean, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnMedian, 2, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(6, 49);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(398, 54);
            this.tableLayoutPanel5.TabIndex = 97;
            // 
            // btnMax
            // 
            this.btnMax.AutoFont = true;
            this.btnMax.AutoFontHeightRatio = 0.75F;
            this.btnMax.AutoFontMax = 100F;
            this.btnMax.AutoFontMin = 6F;
            this.btnMax.AutoFontWidthRatio = 0.92F;
            this.btnMax.AutoImage = true;
            this.btnMax.AutoImageMaxRatio = 0.75F;
            this.btnMax.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMax.AutoImageTint = true;
            this.btnMax.BackColor = System.Drawing.Color.White;
            this.btnMax.BackgroundColor = System.Drawing.Color.White;
            this.btnMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMax.BackgroundImage")));
            this.btnMax.BorderColor = System.Drawing.Color.White;
            this.btnMax.BorderRadius = 10;
            this.btnMax.BorderSize = 1;
            this.btnMax.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMax.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMax.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMax.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMax.Corner = BeeGlobal.Corner.Right;
            this.btnMax.DebounceResizeMs = 16;
            this.btnMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMax.FlatAppearance.BorderSize = 0;
            this.btnMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnMax.ForeColor = System.Drawing.Color.Black;
            this.btnMax.Image = null;
            this.btnMax.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMax.ImageDisabled = null;
            this.btnMax.ImageHover = null;
            this.btnMax.ImageNormal = null;
            this.btnMax.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMax.ImagePressed = null;
            this.btnMax.ImageTextSpacing = 6;
            this.btnMax.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMax.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMax.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMax.ImageTintOpacity = 0.5F;
            this.btnMax.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMax.IsCLick = false;
            this.btnMax.IsNotChange = false;
            this.btnMax.IsRect = false;
            this.btnMax.IsTouch = false;
            this.btnMax.IsUnGroup = false;
            this.btnMax.Location = new System.Drawing.Point(313, 5);
            this.btnMax.Margin = new System.Windows.Forms.Padding(0);
            this.btnMax.Multiline = false;
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(80, 44);
            this.btnMax.TabIndex = 5;
            this.btnMax.Text = "Max";
            this.btnMax.TextColor = System.Drawing.Color.Black;
            this.btnMax.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMax.UseVisualStyleBackColor = false;
            this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
            // 
            // btnMin
            // 
            this.btnMin.AutoFont = true;
            this.btnMin.AutoFontHeightRatio = 0.75F;
            this.btnMin.AutoFontMax = 100F;
            this.btnMin.AutoFontMin = 6F;
            this.btnMin.AutoFontWidthRatio = 0.92F;
            this.btnMin.AutoImage = true;
            this.btnMin.AutoImageMaxRatio = 0.75F;
            this.btnMin.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMin.AutoImageTint = true;
            this.btnMin.BackColor = System.Drawing.Color.White;
            this.btnMin.BackgroundColor = System.Drawing.Color.White;
            this.btnMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMin.BackgroundImage")));
            this.btnMin.BorderColor = System.Drawing.Color.White;
            this.btnMin.BorderRadius = 10;
            this.btnMin.BorderSize = 1;
            this.btnMin.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMin.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMin.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMin.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMin.Corner = BeeGlobal.Corner.None;
            this.btnMin.DebounceResizeMs = 16;
            this.btnMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMin.FlatAppearance.BorderSize = 0;
            this.btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnMin.ForeColor = System.Drawing.Color.Black;
            this.btnMin.Image = null;
            this.btnMin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMin.ImageDisabled = null;
            this.btnMin.ImageHover = null;
            this.btnMin.ImageNormal = null;
            this.btnMin.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMin.ImagePressed = null;
            this.btnMin.ImageTextSpacing = 6;
            this.btnMin.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMin.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMin.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMin.ImageTintOpacity = 0.5F;
            this.btnMin.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMin.IsCLick = false;
            this.btnMin.IsNotChange = false;
            this.btnMin.IsRect = false;
            this.btnMin.IsTouch = false;
            this.btnMin.IsUnGroup = false;
            this.btnMin.Location = new System.Drawing.Point(236, 5);
            this.btnMin.Margin = new System.Windows.Forms.Padding(0);
            this.btnMin.Multiline = false;
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(77, 44);
            this.btnMin.TabIndex = 4;
            this.btnMin.Text = "Min";
            this.btnMin.TextColor = System.Drawing.Color.Black;
            this.btnMin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMin.UseVisualStyleBackColor = false;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnMean
            // 
            this.btnMean.AutoFont = true;
            this.btnMean.AutoFontHeightRatio = 0.75F;
            this.btnMean.AutoFontMax = 100F;
            this.btnMean.AutoFontMin = 6F;
            this.btnMean.AutoFontWidthRatio = 0.92F;
            this.btnMean.AutoImage = true;
            this.btnMean.AutoImageMaxRatio = 0.75F;
            this.btnMean.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMean.AutoImageTint = true;
            this.btnMean.BackColor = System.Drawing.Color.White;
            this.btnMean.BackgroundColor = System.Drawing.Color.White;
            this.btnMean.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMean.BorderColor = System.Drawing.Color.White;
            this.btnMean.BorderRadius = 10;
            this.btnMean.BorderSize = 1;
            this.btnMean.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMean.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMean.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMean.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMean.Corner = BeeGlobal.Corner.Left;
            this.btnMean.DebounceResizeMs = 16;
            this.btnMean.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMean.FlatAppearance.BorderSize = 0;
            this.btnMean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMean.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.34375F);
            this.btnMean.ForeColor = System.Drawing.Color.Black;
            this.btnMean.Image = null;
            this.btnMean.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMean.ImageDisabled = null;
            this.btnMean.ImageHover = null;
            this.btnMean.ImageNormal = null;
            this.btnMean.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMean.ImagePressed = null;
            this.btnMean.ImageTextSpacing = 6;
            this.btnMean.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMean.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMean.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMean.ImageTintOpacity = 0.5F;
            this.btnMean.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMean.IsCLick = true;
            this.btnMean.IsNotChange = false;
            this.btnMean.IsRect = false;
            this.btnMean.IsTouch = false;
            this.btnMean.IsUnGroup = false;
            this.btnMean.Location = new System.Drawing.Point(82, 5);
            this.btnMean.Margin = new System.Windows.Forms.Padding(0);
            this.btnMean.Multiline = false;
            this.btnMean.Name = "btnMean";
            this.btnMean.Size = new System.Drawing.Size(77, 44);
            this.btnMean.TabIndex = 2;
            this.btnMean.Text = "Mean";
            this.btnMean.TextColor = System.Drawing.Color.Black;
            this.btnMean.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMean.UseVisualStyleBackColor = false;
            this.btnMean.Click += new System.EventHandler(this.btnMean_Click);
            // 
            // btnMedian
            // 
            this.btnMedian.AutoFont = true;
            this.btnMedian.AutoFontHeightRatio = 0.75F;
            this.btnMedian.AutoFontMax = 100F;
            this.btnMedian.AutoFontMin = 6F;
            this.btnMedian.AutoFontWidthRatio = 0.92F;
            this.btnMedian.AutoImage = true;
            this.btnMedian.AutoImageMaxRatio = 0.75F;
            this.btnMedian.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMedian.AutoImageTint = true;
            this.btnMedian.BackColor = System.Drawing.Color.White;
            this.btnMedian.BackgroundColor = System.Drawing.Color.White;
            this.btnMedian.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMedian.BackgroundImage")));
            this.btnMedian.BorderColor = System.Drawing.Color.White;
            this.btnMedian.BorderRadius = 5;
            this.btnMedian.BorderSize = 1;
            this.btnMedian.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMedian.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMedian.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMedian.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMedian.Corner = BeeGlobal.Corner.None;
            this.btnMedian.DebounceResizeMs = 16;
            this.btnMedian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMedian.FlatAppearance.BorderSize = 0;
            this.btnMedian.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMedian.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.14063F);
            this.btnMedian.ForeColor = System.Drawing.Color.Black;
            this.btnMedian.Image = null;
            this.btnMedian.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMedian.ImageDisabled = null;
            this.btnMedian.ImageHover = null;
            this.btnMedian.ImageNormal = null;
            this.btnMedian.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMedian.ImagePressed = null;
            this.btnMedian.ImageTextSpacing = 6;
            this.btnMedian.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMedian.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMedian.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMedian.ImageTintOpacity = 0.5F;
            this.btnMedian.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMedian.IsCLick = false;
            this.btnMedian.IsNotChange = false;
            this.btnMedian.IsRect = false;
            this.btnMedian.IsTouch = false;
            this.btnMedian.IsUnGroup = false;
            this.btnMedian.Location = new System.Drawing.Point(159, 5);
            this.btnMedian.Margin = new System.Windows.Forms.Padding(0);
            this.btnMedian.Multiline = false;
            this.btnMedian.Name = "btnMedian";
            this.btnMedian.Size = new System.Drawing.Size(77, 44);
            this.btnMedian.TabIndex = 3;
            this.btnMedian.Text = "Median";
            this.btnMedian.TextColor = System.Drawing.Color.Black;
            this.btnMedian.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMedian.UseVisualStyleBackColor = false;
            this.btnMedian.Click += new System.EventHandler(this.btnMedian_Click);
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Transparent;
            this.label17.Location = new System.Drawing.Point(6, 113);
            this.label17.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(398, 38);
            this.label17.TabIndex = 95;
            this.label17.Text = "Pitch";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(6, 11);
            this.label16.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(398, 38);
            this.label16.TabIndex = 93;
            this.label16.Text = "Mode Value";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.numCrestCouter, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.lbCrestCount, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnEnCrestCounter, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 518);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(398, 50);
            this.tableLayoutPanel4.TabIndex = 55;
            // 
            // numCrestCouter
            // 
            this.numCrestCouter.AutoShowTextbox = false;
            this.numCrestCouter.AutoSizeTextbox = true;
            this.numCrestCouter.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numCrestCouter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numCrestCouter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numCrestCouter.BorderRadius = 6;
            this.numCrestCouter.ButtonMaxSize = 64;
            this.numCrestCouter.ButtonMinSize = 24;
            this.numCrestCouter.Decimals = 0;
            this.numCrestCouter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numCrestCouter.ElementGap = 6;
            this.numCrestCouter.FillTextboxToAvailable = true;
            this.numCrestCouter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCrestCouter.ForeColor = System.Drawing.Color.Red;
            this.numCrestCouter.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numCrestCouter.KeyboardStep = 1F;
            this.numCrestCouter.Location = new System.Drawing.Point(263, 5);
            this.numCrestCouter.Margin = new System.Windows.Forms.Padding(0);
            this.numCrestCouter.Max = 99F;
            this.numCrestCouter.MaxTextboxWidth = 0;
            this.numCrestCouter.Min = 1F;
            this.numCrestCouter.MinimumSize = new System.Drawing.Size(120, 32);
            this.numCrestCouter.MinTextboxWidth = 16;
            this.numCrestCouter.Name = "numCrestCouter";
            this.numCrestCouter.Size = new System.Drawing.Size(130, 40);
            this.numCrestCouter.SnapToStep = true;
            this.numCrestCouter.StartWithTextboxHidden = false;
            this.numCrestCouter.Step = 1F;
            this.numCrestCouter.TabIndex = 43;
            this.numCrestCouter.TextboxFontSize = 20F;
            this.numCrestCouter.TextboxSidePadding = 12;
            this.numCrestCouter.TextboxWidth = 56;
            this.numCrestCouter.UnitText = "";
            this.numCrestCouter.Value = 99F;
            this.numCrestCouter.WheelStep = 1F;
            this.numCrestCouter.ValueChanged += new System.Action<float>(this.numCrestCouter_ValueChanged);
            // 
            // lbCrestCount
            // 
            this.lbCrestCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCrestCount.AutoFont = true;
            this.lbCrestCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbCrestCount.EnableHorizontalScroll = false;
            this.lbCrestCount.EnableVerticalScroll = false;
            this.lbCrestCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.05078F);
            this.lbCrestCount.Location = new System.Drawing.Point(134, 5);
            this.lbCrestCount.Margin = new System.Windows.Forms.Padding(0);
            this.lbCrestCount.MaxFontSize = 200F;
            this.lbCrestCount.MinFontSize = 6F;
            this.lbCrestCount.Name = "lbCrestCount";
            this.lbCrestCount.Size = new System.Drawing.Size(129, 40);
            this.lbCrestCount.TabIndex = 42;
            this.lbCrestCount.Text = "0";
            this.lbCrestCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEnCrestCounter
            // 
            this.btnEnCrestCounter.AutoFont = true;
            this.btnEnCrestCounter.AutoFontHeightRatio = 0.75F;
            this.btnEnCrestCounter.AutoFontMax = 100F;
            this.btnEnCrestCounter.AutoFontMin = 6F;
            this.btnEnCrestCounter.AutoFontWidthRatio = 0.92F;
            this.btnEnCrestCounter.AutoImage = true;
            this.btnEnCrestCounter.AutoImageMaxRatio = 0.75F;
            this.btnEnCrestCounter.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnCrestCounter.AutoImageTint = true;
            this.btnEnCrestCounter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEnCrestCounter.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEnCrestCounter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnCrestCounter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnEnCrestCounter.BorderRadius = 10;
            this.btnEnCrestCounter.BorderSize = 1;
            this.btnEnCrestCounter.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnCrestCounter.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnCrestCounter.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnCrestCounter.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnCrestCounter.Corner = BeeGlobal.Corner.Left;
            this.btnEnCrestCounter.DebounceResizeMs = 16;
            this.btnEnCrestCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnCrestCounter.FlatAppearance.BorderSize = 0;
            this.btnEnCrestCounter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnCrestCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnEnCrestCounter.ForeColor = System.Drawing.Color.Black;
            this.btnEnCrestCounter.Image = null;
            this.btnEnCrestCounter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnCrestCounter.ImageDisabled = null;
            this.btnEnCrestCounter.ImageHover = null;
            this.btnEnCrestCounter.ImageNormal = null;
            this.btnEnCrestCounter.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnCrestCounter.ImagePressed = null;
            this.btnEnCrestCounter.ImageTextSpacing = 6;
            this.btnEnCrestCounter.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnCrestCounter.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnCrestCounter.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnCrestCounter.ImageTintOpacity = 0.5F;
            this.btnEnCrestCounter.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnCrestCounter.IsCLick = false;
            this.btnEnCrestCounter.IsNotChange = false;
            this.btnEnCrestCounter.IsRect = false;
            this.btnEnCrestCounter.IsTouch = false;
            this.btnEnCrestCounter.IsUnGroup = true;
            this.btnEnCrestCounter.Location = new System.Drawing.Point(5, 5);
            this.btnEnCrestCounter.Margin = new System.Windows.Forms.Padding(0);
            this.btnEnCrestCounter.Multiline = false;
            this.btnEnCrestCounter.Name = "btnEnCrestCounter";
            this.btnEnCrestCounter.Size = new System.Drawing.Size(129, 40);
            this.btnEnCrestCounter.TabIndex = 41;
            this.btnEnCrestCounter.Text = "Crest";
            this.btnEnCrestCounter.TextColor = System.Drawing.Color.Black;
            this.btnEnCrestCounter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnCrestCounter.UseVisualStyleBackColor = false;
            this.btnEnCrestCounter.Click += new System.EventHandler(this.btnEnCrestCounter_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel7);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(420, 766);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Filter";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.AdjClearBig, 0, 7);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel9, 0, 6);
            this.tableLayoutPanel7.Controls.Add(this.AdjOpen, 0, 5);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel10, 0, 4);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel11, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.AdjClearNoise, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel12, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.AdjClose, 0, 3);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 9;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Size = new System.Drawing.Size(414, 760);
            this.tableLayoutPanel7.TabIndex = 1;
            // 
            // AdjClearBig
            // 
            this.AdjClearBig.AutoRepeatAccelDeltaMs = -5;
            this.AdjClearBig.AutoRepeatAccelerate = true;
            this.AdjClearBig.AutoRepeatEnabled = true;
            this.AdjClearBig.AutoRepeatInitialDelay = 400;
            this.AdjClearBig.AutoRepeatInterval = 60;
            this.AdjClearBig.AutoRepeatMinInterval = 20;
            this.AdjClearBig.AutoShowTextbox = true;
            this.AdjClearBig.AutoSizeTextbox = true;
            this.AdjClearBig.BackColor = System.Drawing.Color.White;
            this.AdjClearBig.BarLeftGap = 20;
            this.AdjClearBig.BarRightGap = 6;
            this.AdjClearBig.ChromeGap = 1;
            this.AdjClearBig.ChromeWidthRatio = 0.14F;
            this.AdjClearBig.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjClearBig.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjClearBig.ColorScale = System.Drawing.Color.LightGray;
            this.AdjClearBig.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjClearBig.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjClearBig.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjClearBig.Decimals = 0;
            this.AdjClearBig.DisabledDesaturateMix = 0.3F;
            this.AdjClearBig.DisabledDimFactor = 0.9F;
            this.AdjClearBig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjClearBig.EdgePadding = 2;
            this.AdjClearBig.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjClearBig.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjClearBig.KeyboardStep = 1F;
            this.AdjClearBig.Location = new System.Drawing.Point(5, 399);
            this.AdjClearBig.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjClearBig.MatchTextboxFontToThumb = true;
            this.AdjClearBig.Max = 10000F;
            this.AdjClearBig.MaxTextboxWidth = 0;
            this.AdjClearBig.MaxThumb = 1000;
            this.AdjClearBig.MaxTrackHeight = 1000;
            this.AdjClearBig.Min = 1F;
            this.AdjClearBig.MinChromeWidth = 64;
            this.AdjClearBig.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjClearBig.MinTextboxWidth = 32;
            this.AdjClearBig.MinThumb = 30;
            this.AdjClearBig.MinTrackHeight = 8;
            this.AdjClearBig.Name = "AdjClearBig";
            this.AdjClearBig.Radius = 8;
            this.AdjClearBig.ShowValueOnThumb = true;
            this.AdjClearBig.Size = new System.Drawing.Size(404, 53);
            this.AdjClearBig.SnapToStep = true;
            this.AdjClearBig.StartWithTextboxHidden = true;
            this.AdjClearBig.Step = 1F;
            this.AdjClearBig.TabIndex = 90;
            this.AdjClearBig.TextboxFontSize = 22F;
            this.AdjClearBig.TextboxSidePadding = 10;
            this.AdjClearBig.TextboxWidth = 600;
            this.AdjClearBig.ThumbDiameterRatio = 2F;
            this.AdjClearBig.ThumbValueBold = true;
            this.AdjClearBig.ThumbValueFontScale = 1.5F;
            this.AdjClearBig.ThumbValuePadding = 0;
            this.AdjClearBig.TightEdges = true;
            this.AdjClearBig.TrackHeightRatio = 0.45F;
            this.AdjClearBig.TrackWidthRatio = 1F;
            this.AdjClearBig.UnitText = "";
            this.AdjClearBig.Value = 1F;
            this.AdjClearBig.WheelStep = 1F;
            this.AdjClearBig.ValueChanged += new System.Action<float>(this.AdjClearBig_ValueChanged);
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.btnIsClearBig, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 345);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(404, 54);
            this.tableLayoutPanel9.TabIndex = 89;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(5, 5);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(197, 44);
            this.label12.TabIndex = 84;
            this.label12.Text = "Clear Noise big";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnIsClearBig
            // 
            this.btnIsClearBig.AutoFont = true;
            this.btnIsClearBig.AutoFontHeightRatio = 0.75F;
            this.btnIsClearBig.AutoFontMax = 100F;
            this.btnIsClearBig.AutoFontMin = 6F;
            this.btnIsClearBig.AutoFontWidthRatio = 0.92F;
            this.btnIsClearBig.AutoImage = true;
            this.btnIsClearBig.AutoImageMaxRatio = 0.75F;
            this.btnIsClearBig.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnIsClearBig.AutoImageTint = true;
            this.btnIsClearBig.BackColor = System.Drawing.Color.White;
            this.btnIsClearBig.BackgroundColor = System.Drawing.Color.White;
            this.btnIsClearBig.BorderColor = System.Drawing.Color.White;
            this.btnIsClearBig.BorderRadius = 10;
            this.btnIsClearBig.BorderSize = 1;
            this.btnIsClearBig.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnIsClearBig.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnIsClearBig.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnIsClearBig.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnIsClearBig.Corner = BeeGlobal.Corner.Right;
            this.btnIsClearBig.DebounceResizeMs = 16;
            this.btnIsClearBig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIsClearBig.FlatAppearance.BorderSize = 0;
            this.btnIsClearBig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsClearBig.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnIsClearBig.ForeColor = System.Drawing.Color.Black;
            this.btnIsClearBig.Image = null;
            this.btnIsClearBig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIsClearBig.ImageDisabled = null;
            this.btnIsClearBig.ImageHover = null;
            this.btnIsClearBig.ImageNormal = null;
            this.btnIsClearBig.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnIsClearBig.ImagePressed = null;
            this.btnIsClearBig.ImageTextSpacing = 6;
            this.btnIsClearBig.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnIsClearBig.ImageTintHover = System.Drawing.Color.Empty;
            this.btnIsClearBig.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnIsClearBig.ImageTintOpacity = 0.5F;
            this.btnIsClearBig.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnIsClearBig.IsCLick = false;
            this.btnIsClearBig.IsNotChange = false;
            this.btnIsClearBig.IsRect = false;
            this.btnIsClearBig.IsTouch = false;
            this.btnIsClearBig.IsUnGroup = true;
            this.btnIsClearBig.Location = new System.Drawing.Point(202, 5);
            this.btnIsClearBig.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnIsClearBig.Multiline = false;
            this.btnIsClearBig.Name = "btnIsClearBig";
            this.btnIsClearBig.Size = new System.Drawing.Size(194, 44);
            this.btnIsClearBig.TabIndex = 3;
            this.btnIsClearBig.Text = "Enable";
            this.btnIsClearBig.TextColor = System.Drawing.Color.Black;
            this.btnIsClearBig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIsClearBig.UseVisualStyleBackColor = false;
            this.btnIsClearBig.Click += new System.EventHandler(this.btnIsClearBig_Click);
            // 
            // AdjOpen
            // 
            this.AdjOpen.AutoRepeatAccelDeltaMs = -5;
            this.AdjOpen.AutoRepeatAccelerate = true;
            this.AdjOpen.AutoRepeatEnabled = true;
            this.AdjOpen.AutoRepeatInitialDelay = 400;
            this.AdjOpen.AutoRepeatInterval = 60;
            this.AdjOpen.AutoRepeatMinInterval = 20;
            this.AdjOpen.AutoShowTextbox = true;
            this.AdjOpen.AutoSizeTextbox = true;
            this.AdjOpen.BackColor = System.Drawing.Color.White;
            this.AdjOpen.BarLeftGap = 20;
            this.AdjOpen.BarRightGap = 6;
            this.AdjOpen.ChromeGap = 1;
            this.AdjOpen.ChromeWidthRatio = 0.14F;
            this.AdjOpen.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjOpen.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjOpen.ColorScale = System.Drawing.Color.LightGray;
            this.AdjOpen.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjOpen.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjOpen.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjOpen.Decimals = 1;
            this.AdjOpen.DisabledDesaturateMix = 0.3F;
            this.AdjOpen.DisabledDimFactor = 0.9F;
            this.AdjOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjOpen.EdgePadding = 2;
            this.AdjOpen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjOpen.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjOpen.KeyboardStep = 1F;
            this.AdjOpen.Location = new System.Drawing.Point(5, 282);
            this.AdjOpen.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjOpen.MatchTextboxFontToThumb = true;
            this.AdjOpen.Max = 15F;
            this.AdjOpen.MaxTextboxWidth = 0;
            this.AdjOpen.MaxThumb = 1000;
            this.AdjOpen.MaxTrackHeight = 1000;
            this.AdjOpen.Min = 1F;
            this.AdjOpen.MinChromeWidth = 64;
            this.AdjOpen.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjOpen.MinTextboxWidth = 32;
            this.AdjOpen.MinThumb = 30;
            this.AdjOpen.MinTrackHeight = 8;
            this.AdjOpen.Name = "AdjOpen";
            this.AdjOpen.Radius = 8;
            this.AdjOpen.ShowValueOnThumb = true;
            this.AdjOpen.Size = new System.Drawing.Size(404, 53);
            this.AdjOpen.SnapToStep = true;
            this.AdjOpen.StartWithTextboxHidden = true;
            this.AdjOpen.Step = 2F;
            this.AdjOpen.TabIndex = 88;
            this.AdjOpen.TextboxFontSize = 22F;
            this.AdjOpen.TextboxSidePadding = 10;
            this.AdjOpen.TextboxWidth = 600;
            this.AdjOpen.ThumbDiameterRatio = 2F;
            this.AdjOpen.ThumbValueBold = true;
            this.AdjOpen.ThumbValueFontScale = 1.5F;
            this.AdjOpen.ThumbValuePadding = 0;
            this.AdjOpen.TightEdges = true;
            this.AdjOpen.TrackHeightRatio = 0.45F;
            this.AdjOpen.TrackWidthRatio = 1F;
            this.AdjOpen.UnitText = "";
            this.AdjOpen.Value = 1F;
            this.AdjOpen.WheelStep = 1F;
            this.AdjOpen.ValueChanged += new System.Action<float>(this.AdjOpen_ValueChanged);
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.btnOpen, 1, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(5, 234);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(404, 48);
            this.tableLayoutPanel10.TabIndex = 87;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(5, 5);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(197, 38);
            this.label13.TabIndex = 84;
            this.label13.Text = "Open Edge";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpen
            // 
            this.btnOpen.AutoFont = true;
            this.btnOpen.AutoFontHeightRatio = 0.75F;
            this.btnOpen.AutoFontMax = 100F;
            this.btnOpen.AutoFontMin = 6F;
            this.btnOpen.AutoFontWidthRatio = 0.92F;
            this.btnOpen.AutoImage = true;
            this.btnOpen.AutoImageMaxRatio = 0.75F;
            this.btnOpen.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnOpen.AutoImageTint = true;
            this.btnOpen.BackColor = System.Drawing.Color.White;
            this.btnOpen.BackgroundColor = System.Drawing.Color.White;
            this.btnOpen.BorderColor = System.Drawing.Color.White;
            this.btnOpen.BorderRadius = 10;
            this.btnOpen.BorderSize = 1;
            this.btnOpen.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnOpen.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnOpen.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnOpen.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOpen.Corner = BeeGlobal.Corner.Right;
            this.btnOpen.DebounceResizeMs = 16;
            this.btnOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpen.FlatAppearance.BorderSize = 0;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.14063F);
            this.btnOpen.ForeColor = System.Drawing.Color.Black;
            this.btnOpen.Image = null;
            this.btnOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpen.ImageDisabled = null;
            this.btnOpen.ImageHover = null;
            this.btnOpen.ImageNormal = null;
            this.btnOpen.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnOpen.ImagePressed = null;
            this.btnOpen.ImageTextSpacing = 6;
            this.btnOpen.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnOpen.ImageTintHover = System.Drawing.Color.Empty;
            this.btnOpen.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnOpen.ImageTintOpacity = 0.5F;
            this.btnOpen.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnOpen.IsCLick = false;
            this.btnOpen.IsNotChange = false;
            this.btnOpen.IsRect = false;
            this.btnOpen.IsTouch = false;
            this.btnOpen.IsUnGroup = true;
            this.btnOpen.Location = new System.Drawing.Point(202, 5);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnOpen.Multiline = false;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(194, 38);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "Enable";
            this.btnOpen.TextColor = System.Drawing.Color.Black;
            this.btnOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.btnIsClearSmall, 1, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(5, 0);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(404, 54);
            this.tableLayoutPanel11.TabIndex = 86;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Transparent;
            this.label14.Location = new System.Drawing.Point(5, 5);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(197, 44);
            this.label14.TabIndex = 84;
            this.label14.Text = "Clear Noise Small";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnIsClearSmall
            // 
            this.btnIsClearSmall.AutoFont = true;
            this.btnIsClearSmall.AutoFontHeightRatio = 0.75F;
            this.btnIsClearSmall.AutoFontMax = 100F;
            this.btnIsClearSmall.AutoFontMin = 6F;
            this.btnIsClearSmall.AutoFontWidthRatio = 0.92F;
            this.btnIsClearSmall.AutoImage = true;
            this.btnIsClearSmall.AutoImageMaxRatio = 0.75F;
            this.btnIsClearSmall.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnIsClearSmall.AutoImageTint = true;
            this.btnIsClearSmall.BackColor = System.Drawing.Color.White;
            this.btnIsClearSmall.BackgroundColor = System.Drawing.Color.White;
            this.btnIsClearSmall.BorderColor = System.Drawing.Color.White;
            this.btnIsClearSmall.BorderRadius = 10;
            this.btnIsClearSmall.BorderSize = 1;
            this.btnIsClearSmall.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnIsClearSmall.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnIsClearSmall.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnIsClearSmall.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnIsClearSmall.Corner = BeeGlobal.Corner.Right;
            this.btnIsClearSmall.DebounceResizeMs = 16;
            this.btnIsClearSmall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIsClearSmall.FlatAppearance.BorderSize = 0;
            this.btnIsClearSmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsClearSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnIsClearSmall.ForeColor = System.Drawing.Color.Black;
            this.btnIsClearSmall.Image = null;
            this.btnIsClearSmall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIsClearSmall.ImageDisabled = null;
            this.btnIsClearSmall.ImageHover = null;
            this.btnIsClearSmall.ImageNormal = null;
            this.btnIsClearSmall.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnIsClearSmall.ImagePressed = null;
            this.btnIsClearSmall.ImageTextSpacing = 6;
            this.btnIsClearSmall.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnIsClearSmall.ImageTintHover = System.Drawing.Color.Empty;
            this.btnIsClearSmall.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnIsClearSmall.ImageTintOpacity = 0.5F;
            this.btnIsClearSmall.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnIsClearSmall.IsCLick = false;
            this.btnIsClearSmall.IsNotChange = false;
            this.btnIsClearSmall.IsRect = false;
            this.btnIsClearSmall.IsTouch = false;
            this.btnIsClearSmall.IsUnGroup = true;
            this.btnIsClearSmall.Location = new System.Drawing.Point(202, 5);
            this.btnIsClearSmall.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnIsClearSmall.Multiline = false;
            this.btnIsClearSmall.Name = "btnIsClearSmall";
            this.btnIsClearSmall.Size = new System.Drawing.Size(194, 44);
            this.btnIsClearSmall.TabIndex = 3;
            this.btnIsClearSmall.Text = "Enable";
            this.btnIsClearSmall.TextColor = System.Drawing.Color.Black;
            this.btnIsClearSmall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIsClearSmall.UseVisualStyleBackColor = false;
            this.btnIsClearSmall.Click += new System.EventHandler(this.btnEnableNoise_Click);
            // 
            // AdjClearNoise
            // 
            this.AdjClearNoise.AutoRepeatAccelDeltaMs = -5;
            this.AdjClearNoise.AutoRepeatAccelerate = true;
            this.AdjClearNoise.AutoRepeatEnabled = true;
            this.AdjClearNoise.AutoRepeatInitialDelay = 400;
            this.AdjClearNoise.AutoRepeatInterval = 60;
            this.AdjClearNoise.AutoRepeatMinInterval = 20;
            this.AdjClearNoise.AutoShowTextbox = true;
            this.AdjClearNoise.AutoSizeTextbox = true;
            this.AdjClearNoise.BackColor = System.Drawing.Color.White;
            this.AdjClearNoise.BarLeftGap = 20;
            this.AdjClearNoise.BarRightGap = 6;
            this.AdjClearNoise.ChromeGap = 1;
            this.AdjClearNoise.ChromeWidthRatio = 0.14F;
            this.AdjClearNoise.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjClearNoise.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjClearNoise.ColorScale = System.Drawing.Color.LightGray;
            this.AdjClearNoise.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjClearNoise.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjClearNoise.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjClearNoise.Decimals = 0;
            this.AdjClearNoise.DisabledDesaturateMix = 0.3F;
            this.AdjClearNoise.DisabledDimFactor = 0.9F;
            this.AdjClearNoise.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjClearNoise.EdgePadding = 2;
            this.AdjClearNoise.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjClearNoise.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjClearNoise.KeyboardStep = 1F;
            this.AdjClearNoise.Location = new System.Drawing.Point(5, 54);
            this.AdjClearNoise.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjClearNoise.MatchTextboxFontToThumb = true;
            this.AdjClearNoise.Max = 10000F;
            this.AdjClearNoise.MaxTextboxWidth = 0;
            this.AdjClearNoise.MaxThumb = 1000;
            this.AdjClearNoise.MaxTrackHeight = 1000;
            this.AdjClearNoise.Min = 1F;
            this.AdjClearNoise.MinChromeWidth = 64;
            this.AdjClearNoise.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjClearNoise.MinTextboxWidth = 32;
            this.AdjClearNoise.MinThumb = 30;
            this.AdjClearNoise.MinTrackHeight = 8;
            this.AdjClearNoise.Name = "AdjClearNoise";
            this.AdjClearNoise.Radius = 8;
            this.AdjClearNoise.ShowValueOnThumb = true;
            this.AdjClearNoise.Size = new System.Drawing.Size(404, 53);
            this.AdjClearNoise.SnapToStep = true;
            this.AdjClearNoise.StartWithTextboxHidden = true;
            this.AdjClearNoise.Step = 1F;
            this.AdjClearNoise.TabIndex = 84;
            this.AdjClearNoise.TextboxFontSize = 22F;
            this.AdjClearNoise.TextboxSidePadding = 10;
            this.AdjClearNoise.TextboxWidth = 600;
            this.AdjClearNoise.ThumbDiameterRatio = 2F;
            this.AdjClearNoise.ThumbValueBold = true;
            this.AdjClearNoise.ThumbValueFontScale = 1.5F;
            this.AdjClearNoise.ThumbValuePadding = 0;
            this.AdjClearNoise.TightEdges = true;
            this.AdjClearNoise.TrackHeightRatio = 0.45F;
            this.AdjClearNoise.TrackWidthRatio = 1F;
            this.AdjClearNoise.UnitText = "";
            this.AdjClearNoise.Value = 1F;
            this.AdjClearNoise.WheelStep = 1F;
            this.AdjClearNoise.ValueChanged += new System.Action<float>(this.AdjClearNoise_ValueChanged);
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(5, 117);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(404, 54);
            this.tableLayoutPanel12.TabIndex = 85;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(5, 5);
            this.label15.Margin = new System.Windows.Forms.Padding(0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(197, 44);
            this.label15.TabIndex = 84;
            this.label15.Text = "Close Edge";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.AutoFont = true;
            this.btnClose.AutoFontHeightRatio = 0.75F;
            this.btnClose.AutoFontMax = 100F;
            this.btnClose.AutoFontMin = 6F;
            this.btnClose.AutoFontWidthRatio = 0.92F;
            this.btnClose.AutoImage = true;
            this.btnClose.AutoImageMaxRatio = 0.75F;
            this.btnClose.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnClose.AutoImageTint = true;
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.BackgroundColor = System.Drawing.Color.White;
            this.btnClose.BorderColor = System.Drawing.Color.White;
            this.btnClose.BorderRadius = 10;
            this.btnClose.BorderSize = 1;
            this.btnClose.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnClose.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnClose.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnClose.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnClose.Corner = BeeGlobal.Corner.Right;
            this.btnClose.DebounceResizeMs = 16;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Image = null;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.ImageDisabled = null;
            this.btnClose.ImageHover = null;
            this.btnClose.ImageNormal = null;
            this.btnClose.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnClose.ImagePressed = null;
            this.btnClose.ImageTextSpacing = 6;
            this.btnClose.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnClose.ImageTintHover = System.Drawing.Color.Empty;
            this.btnClose.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnClose.ImageTintOpacity = 0.5F;
            this.btnClose.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnClose.IsCLick = false;
            this.btnClose.IsNotChange = false;
            this.btnClose.IsRect = false;
            this.btnClose.IsTouch = false;
            this.btnClose.IsUnGroup = true;
            this.btnClose.Location = new System.Drawing.Point(202, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnClose.Multiline = false;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(194, 44);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Enable";
            this.btnClose.TextColor = System.Drawing.Color.Black;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnEnMorphology_Click);
            // 
            // AdjClose
            // 
            this.AdjClose.AutoRepeatAccelDeltaMs = -5;
            this.AdjClose.AutoRepeatAccelerate = true;
            this.AdjClose.AutoRepeatEnabled = true;
            this.AdjClose.AutoRepeatInitialDelay = 400;
            this.AdjClose.AutoRepeatInterval = 60;
            this.AdjClose.AutoRepeatMinInterval = 20;
            this.AdjClose.AutoShowTextbox = true;
            this.AdjClose.AutoSizeTextbox = true;
            this.AdjClose.BackColor = System.Drawing.Color.White;
            this.AdjClose.BarLeftGap = 20;
            this.AdjClose.BarRightGap = 6;
            this.AdjClose.ChromeGap = 1;
            this.AdjClose.ChromeWidthRatio = 0.14F;
            this.AdjClose.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjClose.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjClose.ColorScale = System.Drawing.Color.LightGray;
            this.AdjClose.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjClose.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjClose.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjClose.Decimals = 1;
            this.AdjClose.DisabledDesaturateMix = 0.3F;
            this.AdjClose.DisabledDimFactor = 0.9F;
            this.AdjClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjClose.EdgePadding = 2;
            this.AdjClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjClose.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjClose.KeyboardStep = 1F;
            this.AdjClose.Location = new System.Drawing.Point(5, 171);
            this.AdjClose.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjClose.MatchTextboxFontToThumb = true;
            this.AdjClose.Max = 15F;
            this.AdjClose.MaxTextboxWidth = 0;
            this.AdjClose.MaxThumb = 1000;
            this.AdjClose.MaxTrackHeight = 1000;
            this.AdjClose.Min = 1F;
            this.AdjClose.MinChromeWidth = 64;
            this.AdjClose.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjClose.MinTextboxWidth = 32;
            this.AdjClose.MinThumb = 30;
            this.AdjClose.MinTrackHeight = 8;
            this.AdjClose.Name = "AdjClose";
            this.AdjClose.Radius = 8;
            this.AdjClose.ShowValueOnThumb = true;
            this.AdjClose.Size = new System.Drawing.Size(404, 53);
            this.AdjClose.SnapToStep = true;
            this.AdjClose.StartWithTextboxHidden = true;
            this.AdjClose.Step = 2F;
            this.AdjClose.TabIndex = 82;
            this.AdjClose.TextboxFontSize = 22F;
            this.AdjClose.TextboxSidePadding = 10;
            this.AdjClose.TextboxWidth = 600;
            this.AdjClose.ThumbDiameterRatio = 2F;
            this.AdjClose.ThumbValueBold = true;
            this.AdjClose.ThumbValueFontScale = 1.5F;
            this.AdjClose.ThumbValuePadding = 0;
            this.AdjClose.TightEdges = true;
            this.AdjClose.TrackHeightRatio = 0.45F;
            this.AdjClose.TrackWidthRatio = 1F;
            this.AdjClose.UnitText = "";
            this.AdjClose.Value = 1F;
            this.AdjClose.WheelStep = 1F;
            this.AdjClose.ValueChanged += new System.Action<float>(this.AdjMorphology_ValueChanged);
            // 
            // pInspect
            // 
            this.pInspect.Controls.Add(this.btnTest);
            this.pInspect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pInspect.Location = new System.Drawing.Point(0, 808);
            this.pInspect.Name = "pInspect";
            this.pInspect.Padding = new System.Windows.Forms.Padding(10);
            this.pInspect.Size = new System.Drawing.Size(428, 100);
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
            this.btnTest.IsTouch = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(10, 10);
            this.btnTest.Margin = new System.Windows.Forms.Padding(10);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(408, 80);
            this.btnTest.TabIndex = 81;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 908);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(428, 62);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolPitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.pInspect);
            this.Controls.Add(this.oK_Cancel1);
            this.DoubleBuffered = true;
            this.Name = "ToolPitch";
            this.Size = new System.Drawing.Size(428, 970);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.layThreshod.ResumeLayout(false);
            this.layThreshod.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.layLimitCouter.ResumeLayout(false);
            this.tableLayoutPanel17.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.pInspect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TabPage tabPage4;
        private GroupControl.OK_Cancel oK_Cancel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private RJButton btnStrongEdge;
        private RJButton btnCloseEdge;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnHori;
        private RJButton btnVer;
        private RJButton btnBinary;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private RJButton btnInvert;
        private System.Windows.Forms.TableLayoutPanel layThreshod;
        private System.Windows.Forms.Label label11;
        private AdjustBarEx AdjThreshod;
        private System.Windows.Forms.Label label8;
        private AdjustBarEx trackScore;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private AdjustBarEx AdjClearBig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Label label12;
        private RJButton btnIsClearBig;
        private AdjustBarEx AdjOpen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Label label13;
        private RJButton btnOpen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label14;
        private RJButton btnIsClearSmall;
        private AdjustBarEx AdjClearNoise;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Label label15;
        private RJButton btnClose;
        private AdjustBarEx AdjClose;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private AdjustBarEx AdjScale;
        private System.Windows.Forms.Panel pInspect;
        private RJButton btnTest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnMax;
        private RJButton btnMin;
        private RJButton btnMean;
        private RJButton btnMedian;
        private AdjustBarEx AdjMagin;
        private AdjustBarEx AdjGaussianSmooth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnEnCrestPitch;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label4;
        private RJButton btnEnCrestCounter;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private RJButton btnEnRootPitch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private AutoFontLabel lbPitchCrestMean;
        private AutoFontLabel lbPitchCrestMax;
        private AutoFontLabel lbPitchCrestMin;
        private AutoFontLabel lbPitchCrestMedian;
        private AutoFontLabel lbPitchRootMean;
        private AutoFontLabel lbPitchRootMedian;
        private AutoFontLabel lbPitchRootMin;
        private AutoFontLabel lbPitchRootMax;
        private AutoFontLabel lbHeightCrestMean;
        private AutoFontLabel lbHeightCrestMedian;
        private AutoFontLabel lbHeightCrestMin;
        private AutoFontLabel lbHeightCrestMax;
        private AutoFontLabel lbHeightRootMean;
        private AutoFontLabel lbHeightRootMedian;
        private AutoFontLabel lbHeightRootMin;
        private AutoFontLabel lbHeightRootMax;
        private AutoFontLabel autoFontLabel17;
        private RJButton btnEnCrestHeight;
        private RJButton btnEnRootHeight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel17;
        private AutoFontLabel lbRootCount;
        private System.Windows.Forms.TableLayoutPanel layLimitCouter;
        private RJButton btnMore;
        private RJButton btnEqual;
        private RJButton btnLess;
        private CustomNumericEx numRootCounter;
        private RJButton btnEnRootCounter;
        private CustomNumericEx numCrestCouter;
        private AutoFontLabel lbCrestCount;
        private RJButton btnCalib;
    }
}
