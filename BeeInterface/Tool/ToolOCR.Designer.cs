using BeeGlobal;
using System.Windows.Forms;

namespace BeeInterface
{
    partial class ToolOCR
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
            this.components = new System.ComponentModel.Container();
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.btnApply = new BeeInterface.RJButton();
            this.txtAllow = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnReadThresh = new BeeInterface.RJButton();
            this.tabLabelResult = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSet = new BeeInterface.RJButton();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.layoutLineLimit = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton1 = new BeeInterface.RJButton();
            this.btnMoreArea = new BeeInterface.RJButton();
            this.btnLessArea = new BeeInterface.RJButton();
            this.numLimtArea = new BeeInterface.CustomNumericEx();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEnLimitArea = new BeeInterface.RJButton();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.numBlur = new BeeInterface.CustomNumericEx();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numUnsharp = new BeeInterface.CustomNumericEx();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.numCLAHE = new BeeInterface.CustomNumericEx();
            this.label1 = new System.Windows.Forms.Label();
            this.tmCheckFist = new System.Windows.Forms.Timer(this.components);
            this.workLoadModel = new System.ComponentModel.BackgroundWorker();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.pInspect = new System.Windows.Forms.Panel();
            this.btnTest = new BeeInterface.RJButton();
            this.tabControl1.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.layoutLineLimit.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.pInspect.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabP1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(400, 837);
            this.tabControl1.TabIndex = 18;
            // 
            // tabP1
            // 
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(392, 799);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel12, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel11, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tabLabelResult, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 793);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.trackScore.BackColor = System.Drawing.SystemColors.Control;
            this.trackScore.BarLeftGap = 20;
            this.trackScore.BarRightGap = 6;
            this.trackScore.ChromeGap = 8;
            this.trackScore.ChromeWidthRatio = 0.14F;
            this.trackScore.ColorBorder = System.Drawing.Color.DarkGray;
            this.trackScore.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackScore.ColorScale = System.Drawing.Color.DarkGray;
            this.trackScore.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorTrack = System.Drawing.Color.DarkGray;
            this.trackScore.Decimals = 0;
            this.trackScore.DisabledDesaturateMix = 0.3F;
            this.trackScore.DisabledDimFactor = 0.9F;
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(3, 520);
            this.trackScore.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
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
            this.trackScore.Size = new System.Drawing.Size(380, 51);
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
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel12.Controls.Add(this.btnApply, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.txtAllow, 0, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 193);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(380, 134);
            this.tableLayoutPanel12.TabIndex = 53;
            // 
            // btnApply
            // 
            this.btnApply.AutoFont = true;
            this.btnApply.AutoFontHeightRatio = 0.75F;
            this.btnApply.AutoFontMax = 100F;
            this.btnApply.AutoFontMin = 6F;
            this.btnApply.AutoFontWidthRatio = 0.92F;
            this.btnApply.AutoImage = true;
            this.btnApply.AutoImageMaxRatio = 0.75F;
            this.btnApply.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnApply.AutoImageTint = true;
            this.btnApply.BackColor = System.Drawing.SystemColors.Control;
            this.btnApply.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnApply.BorderColor = System.Drawing.Color.Silver;
            this.btnApply.BorderRadius = 10;
            this.btnApply.BorderSize = 1;
            this.btnApply.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnApply.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnApply.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnApply.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnApply.Corner = BeeGlobal.Corner.Right;
            this.btnApply.DebounceResizeMs = 16;
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnApply.ForeColor = System.Drawing.Color.Black;
            this.btnApply.Image = null;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApply.ImageDisabled = null;
            this.btnApply.ImageHover = null;
            this.btnApply.ImageNormal = null;
            this.btnApply.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnApply.ImagePressed = null;
            this.btnApply.ImageTextSpacing = 6;
            this.btnApply.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnApply.ImageTintHover = System.Drawing.Color.Empty;
            this.btnApply.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnApply.ImageTintOpacity = 0.5F;
            this.btnApply.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnApply.IsCLick = false;
            this.btnApply.IsNotChange = true;
            this.btnApply.IsRect = false;
            this.btnApply.IsTouch = false;
            this.btnApply.IsUnGroup = false;
            this.btnApply.Location = new System.Drawing.Point(290, 10);
            this.btnApply.Margin = new System.Windows.Forms.Padding(10);
            this.btnApply.Multiline = false;
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 114);
            this.btnApply.TabIndex = 48;
            this.btnApply.Text = "Apply";
            this.btnApply.TextColor = System.Drawing.Color.Black;
            this.btnApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtAllow
            // 
            this.txtAllow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAllow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAllow.Location = new System.Drawing.Point(20, 3);
            this.txtAllow.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.txtAllow.Multiline = true;
            this.txtAllow.Name = "txtAllow";
            this.txtAllow.Size = new System.Drawing.Size(257, 128);
            this.txtAllow.TabIndex = 47;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 165);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 25);
            this.label10.TabIndex = 52;
            this.label10.Text = "Chars Allow";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel11.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.btnReadThresh, 1, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(8, 335);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(8, 5, 3, 3);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(375, 49);
            this.tableLayoutPanel11.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(204, 49);
            this.label4.TabIndex = 45;
            this.label4.Text = "Set Temp";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnReadThresh
            // 
            this.btnReadThresh.AutoFont = true;
            this.btnReadThresh.AutoFontHeightRatio = 0.75F;
            this.btnReadThresh.AutoFontMax = 100F;
            this.btnReadThresh.AutoFontMin = 6F;
            this.btnReadThresh.AutoFontWidthRatio = 0.92F;
            this.btnReadThresh.AutoImage = true;
            this.btnReadThresh.AutoImageMaxRatio = 0.75F;
            this.btnReadThresh.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReadThresh.AutoImageTint = true;
            this.btnReadThresh.BackColor = System.Drawing.SystemColors.Control;
            this.btnReadThresh.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnReadThresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReadThresh.BorderColor = System.Drawing.Color.Transparent;
            this.btnReadThresh.BorderRadius = 10;
            this.btnReadThresh.BorderSize = 1;
            this.btnReadThresh.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnReadThresh.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnReadThresh.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnReadThresh.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReadThresh.Corner = BeeGlobal.Corner.None;
            this.btnReadThresh.DebounceResizeMs = 16;
            this.btnReadThresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadThresh.FlatAppearance.BorderSize = 0;
            this.btnReadThresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadThresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnReadThresh.ForeColor = System.Drawing.Color.Black;
            this.btnReadThresh.Image = null;
            this.btnReadThresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadThresh.ImageDisabled = null;
            this.btnReadThresh.ImageHover = null;
            this.btnReadThresh.ImageNormal = null;
            this.btnReadThresh.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReadThresh.ImagePressed = null;
            this.btnReadThresh.ImageTextSpacing = 6;
            this.btnReadThresh.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReadThresh.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReadThresh.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReadThresh.ImageTintOpacity = 0.5F;
            this.btnReadThresh.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReadThresh.IsCLick = false;
            this.btnReadThresh.IsNotChange = false;
            this.btnReadThresh.IsRect = false;
            this.btnReadThresh.IsTouch = false;
            this.btnReadThresh.IsUnGroup = true;
            this.btnReadThresh.Location = new System.Drawing.Point(207, 3);
            this.btnReadThresh.Multiline = false;
            this.btnReadThresh.Name = "btnReadThresh";
            this.btnReadThresh.Size = new System.Drawing.Size(165, 43);
            this.btnReadThresh.TabIndex = 2;
            this.btnReadThresh.Text = "Read Threshold";
            this.btnReadThresh.TextColor = System.Drawing.Color.Black;
            this.btnReadThresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReadThresh.UseVisualStyleBackColor = false;
            this.btnReadThresh.Visible = false;
            // 
            // tabLabelResult
            // 
            this.tabLabelResult.ColumnCount = 2;
            this.tabLabelResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabLabelResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabLabelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLabelResult.Location = new System.Drawing.Point(3, 390);
            this.tabLabelResult.Name = "tabLabelResult";
            this.tabLabelResult.RowCount = 2;
            this.tabLabelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabLabelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabLabelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tabLabelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tabLabelResult.Size = new System.Drawing.Size(380, 94);
            this.tabLabelResult.TabIndex = 48;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.Controls.Add(this.btnSet, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtContent, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 107);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(380, 50);
            this.tableLayoutPanel3.TabIndex = 47;
            // 
            // btnSet
            // 
            this.btnSet.AutoFont = true;
            this.btnSet.AutoFontHeightRatio = 0.75F;
            this.btnSet.AutoFontMax = 100F;
            this.btnSet.AutoFontMin = 6F;
            this.btnSet.AutoFontWidthRatio = 0.92F;
            this.btnSet.AutoImage = true;
            this.btnSet.AutoImageMaxRatio = 0.75F;
            this.btnSet.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSet.AutoImageTint = true;
            this.btnSet.BackColor = System.Drawing.SystemColors.Control;
            this.btnSet.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSet.BorderColor = System.Drawing.Color.Silver;
            this.btnSet.BorderRadius = 10;
            this.btnSet.BorderSize = 1;
            this.btnSet.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSet.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSet.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnSet.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSet.Corner = BeeGlobal.Corner.Right;
            this.btnSet.DebounceResizeMs = 16;
            this.btnSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSet.FlatAppearance.BorderSize = 0;
            this.btnSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnSet.ForeColor = System.Drawing.Color.Black;
            this.btnSet.Image = null;
            this.btnSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSet.ImageDisabled = null;
            this.btnSet.ImageHover = null;
            this.btnSet.ImageNormal = null;
            this.btnSet.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSet.ImagePressed = null;
            this.btnSet.ImageTextSpacing = 6;
            this.btnSet.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSet.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSet.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSet.ImageTintOpacity = 0.5F;
            this.btnSet.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSet.IsCLick = false;
            this.btnSet.IsNotChange = true;
            this.btnSet.IsRect = false;
            this.btnSet.IsTouch = false;
            this.btnSet.IsUnGroup = false;
            this.btnSet.Location = new System.Drawing.Point(280, 0);
            this.btnSet.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnSet.Multiline = false;
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(97, 50);
            this.btnSet.TabIndex = 48;
            this.btnSet.Text = "Set Maching";
            this.btnSet.TextColor = System.Drawing.Color.Black;
            this.btnSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContent.Location = new System.Drawing.Point(20, 3);
            this.txtContent.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(257, 44);
            this.txtContent.TabIndex = 47;
            this.txtContent.TextChanged += new System.EventHandler(this.txtQRCODE_TextChanged);
            this.txtContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContent_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 492);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(378, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 79);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 25);
            this.label5.TabIndex = 40;
            this.label5.Text = " Maching OCR";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(5, 5);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(149, 25);
            this.label9.TabIndex = 38;
            this.label9.Text = "Search Range";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnCropFull, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCropHalt, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 30);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 44);
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
            this.btnCropFull.BackColor = System.Drawing.SystemColors.Control;
            this.btnCropFull.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCropFull.BorderColor = System.Drawing.Color.Silver;
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
            this.btnCropFull.Location = new System.Drawing.Point(189, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Multiline = false;
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(186, 44);
            this.btnCropFull.TabIndex = 3;
            this.btnCropFull.Text = "Partial";
            this.btnCropFull.TextColor = System.Drawing.Color.Black;
            this.btnCropFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropFull.UseVisualStyleBackColor = false;
            this.btnCropFull.Click += new System.EventHandler(this.btnCropFull_Click_1);
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
            this.btnCropHalt.BackColor = System.Drawing.SystemColors.Control;
            this.btnCropHalt.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.Transparent;
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
            this.btnCropHalt.Location = new System.Drawing.Point(3, 0);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCropHalt.Multiline = false;
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(186, 44);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click_1);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 799);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.layoutLineLimit, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel5, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel10, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 9;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(386, 793);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // layoutLineLimit
            // 
            this.layoutLineLimit.BackColor = System.Drawing.SystemColors.Control;
            this.layoutLineLimit.ColumnCount = 4;
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLineLimit.Controls.Add(this.rjButton1, 3, 0);
            this.layoutLineLimit.Controls.Add(this.btnMoreArea, 1, 0);
            this.layoutLineLimit.Controls.Add(this.btnLessArea, 0, 0);
            this.layoutLineLimit.Controls.Add(this.numLimtArea, 2, 0);
            this.layoutLineLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLineLimit.Enabled = false;
            this.layoutLineLimit.Location = new System.Drawing.Point(5, 257);
            this.layoutLineLimit.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layoutLineLimit.Name = "layoutLineLimit";
            this.layoutLineLimit.RowCount = 1;
            this.layoutLineLimit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLineLimit.Size = new System.Drawing.Size(376, 48);
            this.layoutLineLimit.TabIndex = 51;
            // 
            // rjButton1
            // 
            this.rjButton1.AutoFont = true;
            this.rjButton1.AutoFontHeightRatio = 0.75F;
            this.rjButton1.AutoFontMax = 100F;
            this.rjButton1.AutoFontMin = 6F;
            this.rjButton1.AutoFontWidthRatio = 0.92F;
            this.rjButton1.AutoImage = true;
            this.rjButton1.AutoImageMaxRatio = 0.75F;
            this.rjButton1.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton1.AutoImageTint = true;
            this.rjButton1.BackColor = System.Drawing.SystemColors.Control;
            this.rjButton1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton1.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton1.BorderRadius = 10;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton1.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton1.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton1.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton1.Corner = BeeGlobal.Corner.Right;
            this.rjButton1.DebounceResizeMs = 16;
            this.rjButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton1.Enabled = false;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.Image = null;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.ImageDisabled = null;
            this.rjButton1.ImageHover = null;
            this.rjButton1.ImageNormal = null;
            this.rjButton1.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton1.ImagePressed = null;
            this.rjButton1.ImageTextSpacing = 6;
            this.rjButton1.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton1.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton1.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton1.ImageTintOpacity = 0.5F;
            this.rjButton1.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = true;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsTouch = false;
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(371, 0);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton1.Multiline = false;
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(5, 48);
            this.rjButton1.TabIndex = 38;
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // btnMoreArea
            // 
            this.btnMoreArea.AutoFont = true;
            this.btnMoreArea.AutoFontHeightRatio = 0.75F;
            this.btnMoreArea.AutoFontMax = 100F;
            this.btnMoreArea.AutoFontMin = 6F;
            this.btnMoreArea.AutoFontWidthRatio = 0.92F;
            this.btnMoreArea.AutoImage = true;
            this.btnMoreArea.AutoImageMaxRatio = 0.75F;
            this.btnMoreArea.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMoreArea.AutoImageTint = true;
            this.btnMoreArea.BackColor = System.Drawing.Color.LightGray;
            this.btnMoreArea.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnMoreArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMoreArea.BorderColor = System.Drawing.Color.Transparent;
            this.btnMoreArea.BorderRadius = 5;
            this.btnMoreArea.BorderSize = 1;
            this.btnMoreArea.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMoreArea.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMoreArea.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMoreArea.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMoreArea.Corner = BeeGlobal.Corner.None;
            this.btnMoreArea.DebounceResizeMs = 16;
            this.btnMoreArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMoreArea.FlatAppearance.BorderSize = 0;
            this.btnMoreArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoreArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnMoreArea.ForeColor = System.Drawing.Color.Black;
            this.btnMoreArea.Image = null;
            this.btnMoreArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoreArea.ImageDisabled = null;
            this.btnMoreArea.ImageHover = null;
            this.btnMoreArea.ImageNormal = null;
            this.btnMoreArea.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMoreArea.ImagePressed = null;
            this.btnMoreArea.ImageTextSpacing = 6;
            this.btnMoreArea.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMoreArea.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMoreArea.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMoreArea.ImageTintOpacity = 0.5F;
            this.btnMoreArea.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMoreArea.IsCLick = true;
            this.btnMoreArea.IsNotChange = false;
            this.btnMoreArea.IsRect = false;
            this.btnMoreArea.IsTouch = false;
            this.btnMoreArea.IsUnGroup = false;
            this.btnMoreArea.Location = new System.Drawing.Point(75, 0);
            this.btnMoreArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnMoreArea.Multiline = false;
            this.btnMoreArea.Name = "btnMoreArea";
            this.btnMoreArea.Size = new System.Drawing.Size(75, 48);
            this.btnMoreArea.TabIndex = 33;
            this.btnMoreArea.Text = "More";
            this.btnMoreArea.TextColor = System.Drawing.Color.Black;
            this.btnMoreArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMoreArea.UseVisualStyleBackColor = false;
            // 
            // btnLessArea
            // 
            this.btnLessArea.AutoFont = true;
            this.btnLessArea.AutoFontHeightRatio = 0.75F;
            this.btnLessArea.AutoFontMax = 100F;
            this.btnLessArea.AutoFontMin = 6F;
            this.btnLessArea.AutoFontWidthRatio = 0.92F;
            this.btnLessArea.AutoImage = true;
            this.btnLessArea.AutoImageMaxRatio = 0.75F;
            this.btnLessArea.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLessArea.AutoImageTint = true;
            this.btnLessArea.BackColor = System.Drawing.Color.LightGray;
            this.btnLessArea.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnLessArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLessArea.BorderColor = System.Drawing.Color.Transparent;
            this.btnLessArea.BorderRadius = 10;
            this.btnLessArea.BorderSize = 1;
            this.btnLessArea.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnLessArea.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnLessArea.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnLessArea.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLessArea.Corner = BeeGlobal.Corner.Left;
            this.btnLessArea.DebounceResizeMs = 16;
            this.btnLessArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLessArea.FlatAppearance.BorderSize = 0;
            this.btnLessArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLessArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.8125F);
            this.btnLessArea.ForeColor = System.Drawing.Color.Black;
            this.btnLessArea.Image = null;
            this.btnLessArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLessArea.ImageDisabled = null;
            this.btnLessArea.ImageHover = null;
            this.btnLessArea.ImageNormal = null;
            this.btnLessArea.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLessArea.ImagePressed = null;
            this.btnLessArea.ImageTextSpacing = 6;
            this.btnLessArea.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLessArea.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLessArea.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLessArea.ImageTintOpacity = 0.5F;
            this.btnLessArea.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLessArea.IsCLick = false;
            this.btnLessArea.IsNotChange = false;
            this.btnLessArea.IsRect = false;
            this.btnLessArea.IsTouch = false;
            this.btnLessArea.IsUnGroup = false;
            this.btnLessArea.Location = new System.Drawing.Point(0, 0);
            this.btnLessArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnLessArea.Multiline = false;
            this.btnLessArea.Name = "btnLessArea";
            this.btnLessArea.Size = new System.Drawing.Size(75, 48);
            this.btnLessArea.TabIndex = 31;
            this.btnLessArea.Text = "Less";
            this.btnLessArea.TextColor = System.Drawing.Color.Black;
            this.btnLessArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLessArea.UseVisualStyleBackColor = false;
            // 
            // numLimtArea
            // 
            this.numLimtArea.AutoShowTextbox = false;
            this.numLimtArea.AutoSizeTextbox = true;
            this.numLimtArea.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numLimtArea.BackColor = System.Drawing.SystemColors.Control;
            this.numLimtArea.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numLimtArea.BorderRadius = 6;
            this.numLimtArea.ButtonMaxSize = 64;
            this.numLimtArea.ButtonMinSize = 24;
            this.numLimtArea.Decimals = 0;
            this.numLimtArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numLimtArea.ElementGap = 6;
            this.numLimtArea.FillTextboxToAvailable = true;
            this.numLimtArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numLimtArea.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numLimtArea.KeyboardStep = 1F;
            this.numLimtArea.Location = new System.Drawing.Point(150, 0);
            this.numLimtArea.Margin = new System.Windows.Forms.Padding(0);
            this.numLimtArea.Max = 2000F;
            this.numLimtArea.MaxTextboxWidth = 0;
            this.numLimtArea.Min = 0F;
            this.numLimtArea.MinimumSize = new System.Drawing.Size(120, 32);
            this.numLimtArea.MinTextboxWidth = 16;
            this.numLimtArea.Name = "numLimtArea";
            this.numLimtArea.Size = new System.Drawing.Size(221, 48);
            this.numLimtArea.SnapToStep = true;
            this.numLimtArea.StartWithTextboxHidden = false;
            this.numLimtArea.Step = 1F;
            this.numLimtArea.TabIndex = 34;
            this.numLimtArea.TextboxFontSize = 24F;
            this.numLimtArea.TextboxSidePadding = 12;
            this.numLimtArea.TextboxWidth = 56;
            this.numLimtArea.UnitText = "";
            this.numLimtArea.Value = 500F;
            this.numLimtArea.WheelStep = 1F;
            this.numLimtArea.ValueChanged += new System.Action<float>(this.numLimtArea_ValueChanged);
            this.numLimtArea.Load += new System.EventHandler(this.numLimtArea_Load);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel5.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnEnLimitArea, 2, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(8, 215);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(8, 15, 3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(375, 39);
            this.tableLayoutPanel5.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 39);
            this.label3.TabIndex = 45;
            this.label3.Text = "Limit Area";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnEnLimitArea
            // 
            this.btnEnLimitArea.AutoFont = true;
            this.btnEnLimitArea.AutoFontHeightRatio = 0.75F;
            this.btnEnLimitArea.AutoFontMax = 100F;
            this.btnEnLimitArea.AutoFontMin = 6F;
            this.btnEnLimitArea.AutoFontWidthRatio = 0.92F;
            this.btnEnLimitArea.AutoImage = true;
            this.btnEnLimitArea.AutoImageMaxRatio = 0.75F;
            this.btnEnLimitArea.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnLimitArea.AutoImageTint = true;
            this.btnEnLimitArea.BackColor = System.Drawing.SystemColors.Control;
            this.btnEnLimitArea.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnEnLimitArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnLimitArea.BorderColor = System.Drawing.Color.Transparent;
            this.btnEnLimitArea.BorderRadius = 10;
            this.btnEnLimitArea.BorderSize = 1;
            this.btnEnLimitArea.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnLimitArea.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnLimitArea.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnLimitArea.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnLimitArea.Corner = BeeGlobal.Corner.None;
            this.btnEnLimitArea.DebounceResizeMs = 16;
            this.btnEnLimitArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnLimitArea.FlatAppearance.BorderSize = 0;
            this.btnEnLimitArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnLimitArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.9375F);
            this.btnEnLimitArea.ForeColor = System.Drawing.Color.Black;
            this.btnEnLimitArea.Image = null;
            this.btnEnLimitArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnLimitArea.ImageDisabled = null;
            this.btnEnLimitArea.ImageHover = null;
            this.btnEnLimitArea.ImageNormal = null;
            this.btnEnLimitArea.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnLimitArea.ImagePressed = null;
            this.btnEnLimitArea.ImageTextSpacing = 6;
            this.btnEnLimitArea.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnLimitArea.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnLimitArea.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnLimitArea.ImageTintOpacity = 0.5F;
            this.btnEnLimitArea.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnLimitArea.IsCLick = false;
            this.btnEnLimitArea.IsNotChange = false;
            this.btnEnLimitArea.IsRect = false;
            this.btnEnLimitArea.IsTouch = false;
            this.btnEnLimitArea.IsUnGroup = true;
            this.btnEnLimitArea.Location = new System.Drawing.Point(261, 3);
            this.btnEnLimitArea.Multiline = false;
            this.btnEnLimitArea.Name = "btnEnLimitArea";
            this.btnEnLimitArea.Size = new System.Drawing.Size(111, 33);
            this.btnEnLimitArea.TabIndex = 2;
            this.btnEnLimitArea.Text = "Read";
            this.btnEnLimitArea.TextColor = System.Drawing.Color.Black;
            this.btnEnLimitArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnLimitArea.UseVisualStyleBackColor = false;
            this.btnEnLimitArea.Click += new System.EventHandler(this.btnEnLimitArea_Click);
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Controls.Add(this.numBlur, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(10, 145);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(371, 50);
            this.tableLayoutPanel10.TabIndex = 49;
            // 
            // numBlur
            // 
            this.numBlur.AutoShowTextbox = false;
            this.numBlur.AutoSizeTextbox = true;
            this.numBlur.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numBlur.BackColor = System.Drawing.SystemColors.Control;
            this.numBlur.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numBlur.BorderRadius = 6;
            this.numBlur.ButtonMaxSize = 64;
            this.numBlur.ButtonMinSize = 24;
            this.numBlur.Decimals = 0;
            this.numBlur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numBlur.ElementGap = 6;
            this.numBlur.FillTextboxToAvailable = true;
            this.numBlur.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBlur.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numBlur.KeyboardStep = 1F;
            this.numBlur.Location = new System.Drawing.Point(185, 0);
            this.numBlur.Margin = new System.Windows.Forms.Padding(0);
            this.numBlur.Max = 100F;
            this.numBlur.MaxTextboxWidth = 0;
            this.numBlur.Min = 0F;
            this.numBlur.MinimumSize = new System.Drawing.Size(120, 32);
            this.numBlur.MinTextboxWidth = 16;
            this.numBlur.Name = "numBlur";
            this.numBlur.Size = new System.Drawing.Size(186, 50);
            this.numBlur.SnapToStep = true;
            this.numBlur.StartWithTextboxHidden = false;
            this.numBlur.Step = 1F;
            this.numBlur.TabIndex = 45;
            this.numBlur.TextboxFontSize = 24F;
            this.numBlur.TextboxSidePadding = 12;
            this.numBlur.TextboxWidth = 56;
            this.numBlur.UnitText = "";
            this.numBlur.Value = 3F;
            this.numBlur.WheelStep = 1F;
            this.numBlur.ValueChanged += new System.Action<float>(this.numBlur_ValueChanged);
            this.numBlur.Load += new System.EventHandler(this.numBlur_Load);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(185, 50);
            this.label7.TabIndex = 44;
            this.label7.Text = "Noise (0 for Unuser)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Click += new System.EventHandler(this.label7_Click_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(386, 38);
            this.label6.TabIndex = 48;
            this.label6.Text = "Bộ Lọc Ảnh";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.numUnsharp, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(10, 97);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(371, 38);
            this.tableLayoutPanel4.TabIndex = 47;
            // 
            // numUnsharp
            // 
            this.numUnsharp.AutoShowTextbox = false;
            this.numUnsharp.AutoSizeTextbox = true;
            this.numUnsharp.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numUnsharp.BackColor = System.Drawing.SystemColors.Control;
            this.numUnsharp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numUnsharp.BorderRadius = 6;
            this.numUnsharp.ButtonMaxSize = 64;
            this.numUnsharp.ButtonMinSize = 24;
            this.numUnsharp.Decimals = 0;
            this.numUnsharp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numUnsharp.ElementGap = 6;
            this.numUnsharp.FillTextboxToAvailable = true;
            this.numUnsharp.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUnsharp.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numUnsharp.KeyboardStep = 1F;
            this.numUnsharp.Location = new System.Drawing.Point(185, 0);
            this.numUnsharp.Margin = new System.Windows.Forms.Padding(0);
            this.numUnsharp.Max = 100F;
            this.numUnsharp.MaxTextboxWidth = 0;
            this.numUnsharp.Min = 0F;
            this.numUnsharp.MinimumSize = new System.Drawing.Size(120, 32);
            this.numUnsharp.MinTextboxWidth = 16;
            this.numUnsharp.Name = "numUnsharp";
            this.numUnsharp.Size = new System.Drawing.Size(186, 38);
            this.numUnsharp.SnapToStep = true;
            this.numUnsharp.StartWithTextboxHidden = false;
            this.numUnsharp.Step = 1F;
            this.numUnsharp.TabIndex = 45;
            this.numUnsharp.TextboxFontSize = 24F;
            this.numUnsharp.TextboxSidePadding = 12;
            this.numUnsharp.TextboxWidth = 56;
            this.numUnsharp.UnitText = "";
            this.numUnsharp.Value = 3F;
            this.numUnsharp.WheelStep = 1F;
            this.numUnsharp.ValueChanged += new System.Action<float>(this.numUnsharp_ValueChanged);
            this.numUnsharp.Load += new System.EventHandler(this.numUnsharp_Load);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 38);
            this.label2.TabIndex = 44;
            this.label2.Text = "Sharp (0 for Unuser)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.numCLAHE, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(10, 41);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(373, 48);
            this.tableLayoutPanel9.TabIndex = 45;
            // 
            // numCLAHE
            // 
            this.numCLAHE.AutoShowTextbox = false;
            this.numCLAHE.AutoSizeTextbox = true;
            this.numCLAHE.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numCLAHE.BackColor = System.Drawing.SystemColors.Control;
            this.numCLAHE.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numCLAHE.BorderRadius = 6;
            this.numCLAHE.ButtonMaxSize = 64;
            this.numCLAHE.ButtonMinSize = 24;
            this.numCLAHE.Decimals = 0;
            this.numCLAHE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numCLAHE.ElementGap = 6;
            this.numCLAHE.FillTextboxToAvailable = true;
            this.numCLAHE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCLAHE.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numCLAHE.KeyboardStep = 1F;
            this.numCLAHE.Location = new System.Drawing.Point(186, 0);
            this.numCLAHE.Margin = new System.Windows.Forms.Padding(0);
            this.numCLAHE.Max = 100F;
            this.numCLAHE.MaxTextboxWidth = 0;
            this.numCLAHE.Min = 0F;
            this.numCLAHE.MinimumSize = new System.Drawing.Size(120, 32);
            this.numCLAHE.MinTextboxWidth = 16;
            this.numCLAHE.Name = "numCLAHE";
            this.numCLAHE.Size = new System.Drawing.Size(187, 48);
            this.numCLAHE.SnapToStep = true;
            this.numCLAHE.StartWithTextboxHidden = false;
            this.numCLAHE.Step = 1F;
            this.numCLAHE.TabIndex = 45;
            this.numCLAHE.TextboxFontSize = 24F;
            this.numCLAHE.TextboxSidePadding = 12;
            this.numCLAHE.TextboxWidth = 56;
            this.numCLAHE.UnitText = "";
            this.numCLAHE.Value = 2F;
            this.numCLAHE.WheelStep = 1F;
            this.numCLAHE.ValueChanged += new System.Action<float>(this.numCLAHE_ValueChanged);
            this.numCLAHE.Load += new System.EventHandler(this.numCLAHE_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 48);
            this.label1.TabIndex = 44;
            this.label1.Text = "Contrast (0 for Unuser)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tmCheckFist
            // 
            this.tmCheckFist.Tick += new System.EventHandler(this.tmCheckFist_Tick);
            // 
            // workLoadModel
            // 
            this.workLoadModel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadModel_DoWork);
            this.workLoadModel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadModel_RunWorkerCompleted);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 780);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(400, 57);
            this.oK_Cancel1.TabIndex = 20;
            // 
            // pInspect
            // 
            this.pInspect.Controls.Add(this.btnTest);
            this.pInspect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pInspect.Location = new System.Drawing.Point(0, 680);
            this.pInspect.Name = "pInspect";
            this.pInspect.Padding = new System.Windows.Forms.Padding(10);
            this.pInspect.Size = new System.Drawing.Size(400, 100);
            this.pInspect.TabIndex = 85;
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
            this.btnTest.Size = new System.Drawing.Size(380, 80);
            this.btnTest.TabIndex = 81;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click_1);
            // 
            // ToolOCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pInspect);
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "ToolOCR";
            this.Size = new System.Drawing.Size(400, 837);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.layoutLineLimit.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.pInspect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.ComponentModel.BackgroundWorker threadProcess;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabP1;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TabPage tabPage4;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Timer tmCheckFist;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnSet;
        private System.Windows.Forms.TextBox txtContent;
        private System.ComponentModel.BackgroundWorker workLoadModel;
        private  System.Windows.Forms.TableLayoutPanel tabLabelResult;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private CustomNumericEx numUnsharp;
        private System.Windows.Forms.Label label2;
        private CustomNumericEx numCLAHE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private CustomNumericEx numBlur;
        private System.Windows.Forms.Label label7;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label3;
        private RJButton btnEnLimitArea;
        private  System.Windows.Forms.TableLayoutPanel layoutLineLimit;
        private RJButton rjButton1;
        private RJButton btnMoreArea;
        private RJButton btnLessArea;
        private CustomNumericEx numLimtArea;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label4;
        private RJButton btnReadThresh;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private RJButton btnApply;
        private System.Windows.Forms.TextBox txtAllow;
        private System.Windows.Forms.Label label10;
        private AdjustBarEx trackScore;
        private GroupControl.OK_Cancel oK_Cancel1;
        private Panel pInspect;
        private RJButton btnTest;
    }
}
