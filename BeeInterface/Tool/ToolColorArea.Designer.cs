using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolColorArea
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolColorArea));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.EditRectRot1 = new BeeInterface.Group.EditRectRot();
            this.AdjValueTemp = new BeeInterface.AdjustBarEx();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnGetColor = new BeeInterface.RJButton();
            this.picColor = new System.Windows.Forms.PictureBox();
            this.btnUndo = new System.Windows.Forms.Button();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRGB = new BeeInterface.RJButton();
            this.btnHSV = new BeeInterface.RJButton();
            this.trackPixel = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.btnInspect = new BeeInterface.RJButton();
            this.btnCalib = new BeeInterface.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.AdjClearBig = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnIsClearBig = new BeeInterface.RJButton();
            this.AdjOpen = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.btnOpen = new BeeInterface.RJButton();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.btnIsClearSmall = new BeeInterface.RJButton();
            this.AdjClearNoise = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.btnClose = new BeeInterface.RJButton();
            this.AdjClose = new BeeInterface.AdjustBarEx();
            this.label10 = new System.Windows.Forms.Label();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(450, 827);
            this.tabControl2.TabIndex = 17;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Controls.Add(this.tableLayoutPanel13);
            this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage3.Location = new System.Drawing.Point(4, 38);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(442, 785);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Basic";
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.EditRectRot1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.AdjValueTemp, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 16);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.trackPixel, 0, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.RowCount = 19;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(436, 592);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // EditRectRot1
            // 
            this.EditRectRot1._rotCurrent = null;
            this.EditRectRot1.BackColor = System.Drawing.SystemColors.Control;
            this.EditRectRot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditRectRot1.IsHide = false;
            this.EditRectRot1.Location = new System.Drawing.Point(4, 46);
            this.EditRectRot1.Name = "EditRectRot1";
            this.EditRectRot1.rotCurrent = null;
            this.EditRectRot1.Size = new System.Drawing.Size(414, 330);
            this.EditRectRot1.TabIndex = 129;
            this.EditRectRot1.Visible = false;
            // 
            // AdjValueTemp
            // 
            this.AdjValueTemp.AutoRepeatAccelDeltaMs = -5;
            this.AdjValueTemp.AutoRepeatAccelerate = true;
            this.AdjValueTemp.AutoRepeatEnabled = true;
            this.AdjValueTemp.AutoRepeatInitialDelay = 400;
            this.AdjValueTemp.AutoRepeatInterval = 60;
            this.AdjValueTemp.AutoRepeatMinInterval = 20;
            this.AdjValueTemp.AutoShowTextbox = true;
            this.AdjValueTemp.AutoSizeTextbox = true;
            this.AdjValueTemp.BackColor = System.Drawing.Color.White;
            this.AdjValueTemp.BarLeftGap = 20;
            this.AdjValueTemp.BarRightGap = 6;
            this.AdjValueTemp.ChromeGap = 8;
            this.AdjValueTemp.ChromeWidthRatio = 0.14F;
            this.AdjValueTemp.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjValueTemp.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjValueTemp.ColorScale = System.Drawing.Color.LightGray;
            this.AdjValueTemp.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjValueTemp.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjValueTemp.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjValueTemp.Decimals = 0;
            this.AdjValueTemp.DisabledDesaturateMix = 0.3F;
            this.AdjValueTemp.DisabledDimFactor = 0.9F;
            this.AdjValueTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjValueTemp.EdgePadding = 2;
            this.AdjValueTemp.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjValueTemp.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjValueTemp.KeyboardStep = 1F;
            this.AdjValueTemp.Location = new System.Drawing.Point(6, 672);
            this.AdjValueTemp.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjValueTemp.MatchTextboxFontToThumb = true;
            this.AdjValueTemp.Max = 1000000F;
            this.AdjValueTemp.MaxTextboxWidth = 0;
            this.AdjValueTemp.MaxThumb = 1000;
            this.AdjValueTemp.MaxTrackHeight = 1000;
            this.AdjValueTemp.Min = 0F;
            this.AdjValueTemp.MinChromeWidth = 64;
            this.AdjValueTemp.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjValueTemp.MinTextboxWidth = 32;
            this.AdjValueTemp.MinThumb = 30;
            this.AdjValueTemp.MinTrackHeight = 8;
            this.AdjValueTemp.Name = "AdjValueTemp";
            this.AdjValueTemp.Radius = 8;
            this.AdjValueTemp.ShowValueOnThumb = true;
            this.AdjValueTemp.Size = new System.Drawing.Size(410, 49);
            this.AdjValueTemp.SnapToStep = true;
            this.AdjValueTemp.StartWithTextboxHidden = true;
            this.AdjValueTemp.Step = 1F;
            this.AdjValueTemp.TabIndex = 80;
            this.AdjValueTemp.TextboxFontSize = 20F;
            this.AdjValueTemp.TextboxSidePadding = 10;
            this.AdjValueTemp.TextboxWidth = 600;
            this.AdjValueTemp.ThumbDiameterRatio = 2F;
            this.AdjValueTemp.ThumbValueBold = true;
            this.AdjValueTemp.ThumbValueFontScale = 1.5F;
            this.AdjValueTemp.ThumbValuePadding = 0;
            this.AdjValueTemp.TightEdges = true;
            this.AdjValueTemp.TrackHeightRatio = 0.45F;
            this.AdjValueTemp.TrackWidthRatio = 1F;
            this.AdjValueTemp.UnitText = "";
            this.AdjValueTemp.Value = 0F;
            this.AdjValueTemp.WheelStep = 1F;
            this.AdjValueTemp.ValueChanged += new System.Action<float>(this.AdjValueTemp_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(6, 647);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(410, 25);
            this.label4.TabIndex = 79;
            this.label4.Text = "Value Color Sample";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(6, 741);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(410, 25);
            this.label8.TabIndex = 78;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(6, 546);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 15, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(410, 32);
            this.label3.TabIndex = 74;
            this.label3.Text = "Extraction Color";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(6, 394);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 15, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(410, 32);
            this.label2.TabIndex = 73;
            this.label2.Text = "Color Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(410, 32);
            this.label1.TabIndex = 72;
            this.label1.Text = "Choose Area";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel7.Controls.Add(this.btnDeleteAll, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnGetColor, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.picColor, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnUndo, 2, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(6, 481);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(410, 50);
            this.tableLayoutPanel7.TabIndex = 42;
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Image = global::BeeInterface.Properties.Resources.Delete;
            this.btnDeleteAll.Location = new System.Drawing.Point(360, 5);
            this.btnDeleteAll.Margin = new System.Windows.Forms.Padding(0);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(45, 40);
            this.btnDeleteAll.TabIndex = 35;
            this.btnDeleteAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnGetColor
            // 
            this.btnGetColor.AutoFont = false;
            this.btnGetColor.AutoFontHeightRatio = 0.75F;
            this.btnGetColor.AutoFontMax = 100F;
            this.btnGetColor.AutoFontMin = 6F;
            this.btnGetColor.AutoFontWidthRatio = 0.92F;
            this.btnGetColor.AutoImage = true;
            this.btnGetColor.AutoImageMaxRatio = 0.75F;
            this.btnGetColor.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnGetColor.AutoImageTint = true;
            this.btnGetColor.BackColor = System.Drawing.Color.White;
            this.btnGetColor.BackgroundColor = System.Drawing.Color.White;
            this.btnGetColor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetColor.BackgroundImage")));
            this.btnGetColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetColor.BorderColor = System.Drawing.Color.White;
            this.btnGetColor.BorderRadius = 5;
            this.btnGetColor.BorderSize = 1;
            this.btnGetColor.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnGetColor.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnGetColor.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnGetColor.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnGetColor.Corner = BeeGlobal.Corner.Both;
            this.btnGetColor.DebounceResizeMs = 16;
            this.btnGetColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetColor.FlatAppearance.BorderSize = 0;
            this.btnGetColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.14844F);
            this.btnGetColor.ForeColor = System.Drawing.Color.Black;
            this.btnGetColor.Image = null;
            this.btnGetColor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetColor.ImageDisabled = null;
            this.btnGetColor.ImageHover = null;
            this.btnGetColor.ImageNormal = null;
            this.btnGetColor.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnGetColor.ImagePressed = null;
            this.btnGetColor.ImageTextSpacing = 6;
            this.btnGetColor.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnGetColor.ImageTintHover = System.Drawing.Color.Empty;
            this.btnGetColor.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnGetColor.ImageTintOpacity = 0.5F;
            this.btnGetColor.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnGetColor.IsCLick = false;
            this.btnGetColor.IsNotChange = false;
            this.btnGetColor.IsRect = false;
            this.btnGetColor.IsTouch = false;
            this.btnGetColor.IsUnGroup = true;
            this.btnGetColor.Location = new System.Drawing.Point(5, 5);
            this.btnGetColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnGetColor.Multiline = false;
            this.btnGetColor.Name = "btnGetColor";
            this.btnGetColor.Size = new System.Drawing.Size(114, 40);
            this.btnGetColor.TabIndex = 4;
            this.btnGetColor.Text = "Get Color";
            this.btnGetColor.TextColor = System.Drawing.Color.Black;
            this.btnGetColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnGetColor.UseVisualStyleBackColor = false;
            this.btnGetColor.Click += new System.EventHandler(this.btnGetColor_Click);
            // 
            // picColor
            // 
            this.picColor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picColor.Location = new System.Drawing.Point(124, 5);
            this.picColor.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.picColor.Name = "picColor";
            this.picColor.Size = new System.Drawing.Size(176, 40);
            this.picColor.TabIndex = 31;
            this.picColor.TabStop = false;
            this.picColor.Paint += new System.Windows.Forms.PaintEventHandler(this.picColor_Paint);
            // 
            // btnUndo
            // 
            this.btnUndo.BackgroundImage = global::BeeInterface.Properties.Resources.Undo_3;
            this.btnUndo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUndo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUndo.Location = new System.Drawing.Point(310, 5);
            this.btnUndo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(45, 40);
            this.btnUndo.TabIndex = 33;
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
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
            this.trackScore.Location = new System.Drawing.Point(6, 766);
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
            this.trackScore.Size = new System.Drawing.Size(410, 49);
            this.trackScore.SnapToStep = true;
            this.trackScore.StartWithTextboxHidden = true;
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 69;
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
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnRGB, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnHSV, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(6, 426);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(410, 50);
            this.tableLayoutPanel6.TabIndex = 60;
            // 
            // btnRGB
            // 
            this.btnRGB.AutoFont = false;
            this.btnRGB.AutoFontHeightRatio = 0.75F;
            this.btnRGB.AutoFontMax = 100F;
            this.btnRGB.AutoFontMin = 6F;
            this.btnRGB.AutoFontWidthRatio = 0.92F;
            this.btnRGB.AutoImage = true;
            this.btnRGB.AutoImageMaxRatio = 0.75F;
            this.btnRGB.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRGB.AutoImageTint = true;
            this.btnRGB.BackColor = System.Drawing.Color.White;
            this.btnRGB.BackgroundColor = System.Drawing.Color.White;
            this.btnRGB.BorderColor = System.Drawing.Color.White;
            this.btnRGB.BorderRadius = 10;
            this.btnRGB.BorderSize = 1;
            this.btnRGB.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnRGB.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnRGB.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnRGB.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRGB.Corner = BeeGlobal.Corner.Right;
            this.btnRGB.DebounceResizeMs = 16;
            this.btnRGB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRGB.FlatAppearance.BorderSize = 0;
            this.btnRGB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.375F);
            this.btnRGB.ForeColor = System.Drawing.Color.Black;
            this.btnRGB.Image = null;
            this.btnRGB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRGB.ImageDisabled = null;
            this.btnRGB.ImageHover = null;
            this.btnRGB.ImageNormal = null;
            this.btnRGB.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRGB.ImagePressed = null;
            this.btnRGB.ImageTextSpacing = 6;
            this.btnRGB.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRGB.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRGB.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRGB.ImageTintOpacity = 0.5F;
            this.btnRGB.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRGB.IsCLick = false;
            this.btnRGB.IsNotChange = false;
            this.btnRGB.IsRect = false;
            this.btnRGB.IsTouch = false;
            this.btnRGB.IsUnGroup = false;
            this.btnRGB.Location = new System.Drawing.Point(205, 5);
            this.btnRGB.Margin = new System.Windows.Forms.Padding(0);
            this.btnRGB.Multiline = false;
            this.btnRGB.Name = "btnRGB";
            this.btnRGB.Size = new System.Drawing.Size(200, 40);
            this.btnRGB.TabIndex = 3;
            this.btnRGB.Text = "RGB";
            this.btnRGB.TextColor = System.Drawing.Color.Black;
            this.btnRGB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRGB.UseVisualStyleBackColor = false;
            this.btnRGB.Click += new System.EventHandler(this.btnRGB_Click);
            // 
            // btnHSV
            // 
            this.btnHSV.AutoFont = false;
            this.btnHSV.AutoFontHeightRatio = 0.75F;
            this.btnHSV.AutoFontMax = 100F;
            this.btnHSV.AutoFontMin = 6F;
            this.btnHSV.AutoFontWidthRatio = 0.92F;
            this.btnHSV.AutoImage = true;
            this.btnHSV.AutoImageMaxRatio = 0.75F;
            this.btnHSV.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnHSV.AutoImageTint = true;
            this.btnHSV.BackColor = System.Drawing.Color.White;
            this.btnHSV.BackgroundColor = System.Drawing.Color.White;
            this.btnHSV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHSV.BorderColor = System.Drawing.Color.White;
            this.btnHSV.BorderRadius = 10;
            this.btnHSV.BorderSize = 1;
            this.btnHSV.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnHSV.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnHSV.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnHSV.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnHSV.Corner = BeeGlobal.Corner.Left;
            this.btnHSV.DebounceResizeMs = 16;
            this.btnHSV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHSV.FlatAppearance.BorderSize = 0;
            this.btnHSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.375F);
            this.btnHSV.ForeColor = System.Drawing.Color.Black;
            this.btnHSV.Image = null;
            this.btnHSV.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHSV.ImageDisabled = null;
            this.btnHSV.ImageHover = null;
            this.btnHSV.ImageNormal = null;
            this.btnHSV.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnHSV.ImagePressed = null;
            this.btnHSV.ImageTextSpacing = 6;
            this.btnHSV.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnHSV.ImageTintHover = System.Drawing.Color.Empty;
            this.btnHSV.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnHSV.ImageTintOpacity = 0.5F;
            this.btnHSV.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnHSV.IsCLick = true;
            this.btnHSV.IsNotChange = false;
            this.btnHSV.IsRect = false;
            this.btnHSV.IsTouch = false;
            this.btnHSV.IsUnGroup = false;
            this.btnHSV.Location = new System.Drawing.Point(5, 5);
            this.btnHSV.Margin = new System.Windows.Forms.Padding(0);
            this.btnHSV.Multiline = false;
            this.btnHSV.Name = "btnHSV";
            this.btnHSV.Size = new System.Drawing.Size(200, 40);
            this.btnHSV.TabIndex = 2;
            this.btnHSV.Text = "HSV";
            this.btnHSV.TextColor = System.Drawing.Color.Black;
            this.btnHSV.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHSV.UseVisualStyleBackColor = false;
            this.btnHSV.Click += new System.EventHandler(this.btnHSV_Click);
            // 
            // trackPixel
            // 
            this.trackPixel.AutoRepeatAccelDeltaMs = -5;
            this.trackPixel.AutoRepeatAccelerate = true;
            this.trackPixel.AutoRepeatEnabled = true;
            this.trackPixel.AutoRepeatInitialDelay = 400;
            this.trackPixel.AutoRepeatInterval = 60;
            this.trackPixel.AutoRepeatMinInterval = 20;
            this.trackPixel.AutoShowTextbox = true;
            this.trackPixel.AutoSizeTextbox = true;
            this.trackPixel.BackColor = System.Drawing.Color.White;
            this.trackPixel.BarLeftGap = 20;
            this.trackPixel.BarRightGap = 6;
            this.trackPixel.ChromeGap = 8;
            this.trackPixel.ChromeWidthRatio = 0.14F;
            this.trackPixel.ColorBorder = System.Drawing.Color.LightGray;
            this.trackPixel.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackPixel.ColorScale = System.Drawing.Color.LightGray;
            this.trackPixel.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackPixel.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackPixel.ColorTrack = System.Drawing.Color.LightGray;
            this.trackPixel.Decimals = 0;
            this.trackPixel.DisabledDesaturateMix = 0.3F;
            this.trackPixel.DisabledDimFactor = 0.9F;
            this.trackPixel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackPixel.EdgePadding = 2;
            this.trackPixel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackPixel.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackPixel.KeyboardStep = 1F;
            this.trackPixel.Location = new System.Drawing.Point(6, 578);
            this.trackPixel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.trackPixel.MatchTextboxFontToThumb = true;
            this.trackPixel.Max = 100F;
            this.trackPixel.MaxTextboxWidth = 0;
            this.trackPixel.MaxThumb = 1000;
            this.trackPixel.MaxTrackHeight = 1000;
            this.trackPixel.Min = 0F;
            this.trackPixel.MinChromeWidth = 64;
            this.trackPixel.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackPixel.MinTextboxWidth = 32;
            this.trackPixel.MinThumb = 30;
            this.trackPixel.MinTrackHeight = 8;
            this.trackPixel.Name = "trackPixel";
            this.trackPixel.Radius = 8;
            this.trackPixel.ShowValueOnThumb = true;
            this.trackPixel.Size = new System.Drawing.Size(410, 49);
            this.trackPixel.SnapToStep = true;
            this.trackPixel.StartWithTextboxHidden = true;
            this.trackPixel.Step = 1F;
            this.trackPixel.TabIndex = 70;
            this.trackPixel.TextboxFontSize = 20F;
            this.trackPixel.TextboxSidePadding = 10;
            this.trackPixel.TextboxWidth = 600;
            this.trackPixel.ThumbDiameterRatio = 2F;
            this.trackPixel.ThumbValueBold = true;
            this.trackPixel.ThumbValueFontScale = 1.5F;
            this.trackPixel.ThumbValuePadding = 0;
            this.trackPixel.TightEdges = true;
            this.trackPixel.TrackHeightRatio = 0.45F;
            this.trackPixel.TrackWidthRatio = 1F;
            this.trackPixel.UnitText = "";
            this.trackPixel.Value = 0F;
            this.trackPixel.WheelStep = 1F;
            this.trackPixel.ValueChanged += new System.Action<float>(this.trackPixel_ValueChanged);
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 1;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Controls.Add(this.btnInspect, 0, 1);
            this.tableLayoutPanel13.Controls.Add(this.btnCalib, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(3, 595);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 2;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(436, 187);
            this.tableLayoutPanel13.TabIndex = 2;
            // 
            // btnInspect
            // 
            this.btnInspect.AutoFont = false;
            this.btnInspect.AutoFontHeightRatio = 0.75F;
            this.btnInspect.AutoFontMax = 100F;
            this.btnInspect.AutoFontMin = 6F;
            this.btnInspect.AutoFontWidthRatio = 0.92F;
            this.btnInspect.AutoImage = true;
            this.btnInspect.AutoImageMaxRatio = 0.75F;
            this.btnInspect.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnInspect.AutoImageTint = true;
            this.btnInspect.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnInspect.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnInspect.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnInspect.BorderRadius = 10;
            this.btnInspect.BorderSize = 1;
            this.btnInspect.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnInspect.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnInspect.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnInspect.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnInspect.Corner = BeeGlobal.Corner.Both;
            this.btnInspect.DebounceResizeMs = 16;
            this.btnInspect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInspect.FlatAppearance.BorderSize = 0;
            this.btnInspect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInspect.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.52344F, System.Drawing.FontStyle.Bold);
            this.btnInspect.ForeColor = System.Drawing.Color.Black;
            this.btnInspect.Image = null;
            this.btnInspect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInspect.ImageDisabled = null;
            this.btnInspect.ImageHover = null;
            this.btnInspect.ImageNormal = null;
            this.btnInspect.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnInspect.ImagePressed = null;
            this.btnInspect.ImageTextSpacing = 6;
            this.btnInspect.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnInspect.ImageTintHover = System.Drawing.Color.Empty;
            this.btnInspect.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnInspect.ImageTintOpacity = 0.5F;
            this.btnInspect.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnInspect.IsCLick = false;
            this.btnInspect.IsNotChange = true;
            this.btnInspect.IsRect = false;
            this.btnInspect.IsTouch = false;
            this.btnInspect.IsUnGroup = true;
            this.btnInspect.Location = new System.Drawing.Point(5, 98);
            this.btnInspect.Margin = new System.Windows.Forms.Padding(5);
            this.btnInspect.Multiline = false;
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(426, 84);
            this.btnInspect.TabIndex = 116;
            this.btnInspect.Text = "Inspect";
            this.btnInspect.TextColor = System.Drawing.Color.Black;
            this.btnInspect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInspect.UseVisualStyleBackColor = false;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
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
            this.btnCalib.Location = new System.Drawing.Point(5, 5);
            this.btnCalib.Margin = new System.Windows.Forms.Padding(5);
            this.btnCalib.Multiline = false;
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(426, 83);
            this.btnCalib.TabIndex = 115;
            this.btnCalib.Text = "Set Sample";
            this.btnCalib.TextColor = System.Drawing.Color.Black;
            this.btnCalib.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCalib.UseVisualStyleBackColor = false;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel4);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(442, 785);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(436, 779);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel8);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 785);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Filter";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.AdjClearBig, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.AdjOpen, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel10, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel11, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.AdjClearNoise, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel12, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.AdjClose, 0, 3);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
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
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.Size = new System.Drawing.Size(436, 779);
            this.tableLayoutPanel8.TabIndex = 2;
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
            this.AdjClearBig.Max = 100000F;
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
            this.AdjClearBig.Size = new System.Drawing.Size(426, 53);
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
            this.tableLayoutPanel9.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.btnIsClearBig, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 345);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(426, 54);
            this.tableLayoutPanel9.TabIndex = 89;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(5, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(208, 44);
            this.label5.TabIndex = 84;
            this.label5.Text = "Clear Noise big";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnIsClearBig.Location = new System.Drawing.Point(213, 5);
            this.btnIsClearBig.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnIsClearBig.Multiline = false;
            this.btnIsClearBig.Name = "btnIsClearBig";
            this.btnIsClearBig.Size = new System.Drawing.Size(205, 44);
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
            this.AdjOpen.Size = new System.Drawing.Size(426, 53);
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
            this.tableLayoutPanel10.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.btnOpen, 1, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(5, 234);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(426, 48);
            this.tableLayoutPanel10.TabIndex = 87;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(5, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(208, 38);
            this.label6.TabIndex = 84;
            this.label6.Text = "Open Edge";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnOpen.Location = new System.Drawing.Point(213, 5);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnOpen.Multiline = false;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(205, 38);
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
            this.tableLayoutPanel11.Size = new System.Drawing.Size(426, 54);
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
            this.label14.Size = new System.Drawing.Size(208, 44);
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
            this.btnIsClearSmall.Location = new System.Drawing.Point(213, 5);
            this.btnIsClearSmall.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnIsClearSmall.Multiline = false;
            this.btnIsClearSmall.Name = "btnIsClearSmall";
            this.btnIsClearSmall.Size = new System.Drawing.Size(205, 44);
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
            this.AdjClearNoise.Size = new System.Drawing.Size(426, 53);
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
            this.tableLayoutPanel12.Size = new System.Drawing.Size(426, 54);
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
            this.label15.Size = new System.Drawing.Size(208, 44);
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
            this.btnClose.Location = new System.Drawing.Point(213, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnClose.Multiline = false;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(205, 44);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Enable";
            this.btnClose.TextColor = System.Drawing.Color.Black;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.AdjClose.Size = new System.Drawing.Size(426, 53);
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
            this.AdjClose.ValueChanged += new System.Action<float>(this.AdjClose_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 20);
            this.label10.TabIndex = 25;
            this.label10.Text = "Add Area Mask";
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 827);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(450, 52);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolColorArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.oK_Cancel1);
            this.DoubleBuffered = true;
            this.Name = "ToolColorArea";
            this.Size = new System.Drawing.Size(450, 879);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label10;
        private GroupControl.OK_Cancel oK_Cancel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button btnDeleteAll;
        public RJButton btnGetColor;
        public System.Windows.Forms.PictureBox picColor;
        private System.Windows.Forms.Button btnUndo;
        private AdjustBarEx trackScore;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnRGB;
        private RJButton btnHSV;
        private AdjustBarEx trackPixel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private AdjustBarEx AdjClearBig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Label label5;
        private RJButton btnIsClearBig;
        private AdjustBarEx AdjOpen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Label label6;
        private RJButton btnOpen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label14;
        private RJButton btnIsClearSmall;
        private AdjustBarEx AdjClearNoise;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Label label15;
        private RJButton btnClose;
        private AdjustBarEx AdjClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private RJButton btnInspect;
        private RJButton btnCalib;
        private AdjustBarEx AdjValueTemp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private Group.EditRectRot EditRectRot1;
    }
}
