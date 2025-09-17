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
            this.btnInspect = new BeeInterface.RJButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.trackPixel = new BeeInterface.AdjustBarEx();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMask = new BeeInterface.RJButton();
            this.btnCropArea = new BeeInterface.RJButton();
            this.label10 = new System.Windows.Forms.Label();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.btnNone = new BeeInterface.RJButton();
            this.btnElip = new BeeInterface.RJButton();
            this.btnRect = new BeeInterface.RJButton();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(450, 879);
            this.tabControl2.TabIndex = 17;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage3.Location = new System.Drawing.Point(4, 38);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(442, 837);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Basic";
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnInspect, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.trackPixel, 0, 6);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(436, 831);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(5, 360);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(426, 25);
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
            this.label3.Location = new System.Drawing.Point(5, 259);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 15, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(426, 32);
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
            this.label2.Location = new System.Drawing.Point(5, 107);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 15, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(426, 32);
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
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(426, 32);
            this.label1.TabIndex = 72;
            this.label1.Text = "Search Range";
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
            this.tableLayoutPanel7.Location = new System.Drawing.Point(5, 194);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(426, 50);
            this.tableLayoutPanel7.TabIndex = 42;
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Image = global::BeeInterface.Properties.Resources.Delete;
            this.btnDeleteAll.Location = new System.Drawing.Point(376, 5);
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
            this.picColor.Size = new System.Drawing.Size(192, 40);
            this.picColor.TabIndex = 31;
            this.picColor.TabStop = false;
            this.picColor.Paint += new System.Windows.Forms.PaintEventHandler(this.picColor_Paint);
            // 
            // btnUndo
            // 
            this.btnUndo.BackgroundImage = global::BeeInterface.Properties.Resources.Undo_3;
            this.btnUndo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUndo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUndo.Location = new System.Drawing.Point(326, 5);
            this.btnUndo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(45, 40);
            this.btnUndo.TabIndex = 33;
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // trackScore
            // 
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
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(5, 385);
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
            this.trackScore.Size = new System.Drawing.Size(426, 49);
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
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 139);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(426, 50);
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
            this.btnRGB.IsUnGroup = false;
            this.btnRGB.Location = new System.Drawing.Point(213, 5);
            this.btnRGB.Margin = new System.Windows.Forms.Padding(0);
            this.btnRGB.Multiline = false;
            this.btnRGB.Name = "btnRGB";
            this.btnRGB.Size = new System.Drawing.Size(208, 40);
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
            this.btnHSV.IsUnGroup = false;
            this.btnHSV.Location = new System.Drawing.Point(5, 5);
            this.btnHSV.Margin = new System.Windows.Forms.Padding(0);
            this.btnHSV.Multiline = false;
            this.btnHSV.Name = "btnHSV";
            this.btnHSV.Size = new System.Drawing.Size(208, 40);
            this.btnHSV.TabIndex = 2;
            this.btnHSV.Text = "HSV";
            this.btnHSV.TextColor = System.Drawing.Color.Black;
            this.btnHSV.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHSV.UseVisualStyleBackColor = false;
            this.btnHSV.Click += new System.EventHandler(this.btnHSV_Click);
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
            this.btnInspect.IsUnGroup = true;
            this.btnInspect.Location = new System.Drawing.Point(5, 449);
            this.btnInspect.Margin = new System.Windows.Forms.Padding(5, 15, 5, 0);
            this.btnInspect.Multiline = false;
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(426, 85);
            this.btnInspect.TabIndex = 37;
            this.btnInspect.Text = "Inspect";
            this.btnInspect.TextColor = System.Drawing.Color.Black;
            this.btnInspect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInspect.UseVisualStyleBackColor = false;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 42);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(426, 50);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // btnCropFull
            // 
            this.btnCropFull.AutoFont = false;
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
            this.btnCropFull.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.DebounceResizeMs = 16;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.62891F);
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
            this.btnCropFull.Location = new System.Drawing.Point(213, 5);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Multiline = false;
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(205, 40);
            this.btnCropFull.TabIndex = 3;
            this.btnCropFull.Text = "Partial";
            this.btnCropFull.TextColor = System.Drawing.Color.Black;
            this.btnCropFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropFull.UseVisualStyleBackColor = false;
            this.btnCropFull.Click += new System.EventHandler(this.btnCropFull_Click);
            // 
            // btnCropHalt
            // 
            this.btnCropHalt.AutoFont = false;
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
            this.btnCropHalt.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.DebounceResizeMs = 16;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.62891F);
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
            this.btnCropHalt.Size = new System.Drawing.Size(205, 40);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // trackPixel
            // 
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
            this.trackPixel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackPixel.EdgePadding = 2;
            this.trackPixel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackPixel.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackPixel.KeyboardStep = 1F;
            this.trackPixel.Location = new System.Drawing.Point(5, 291);
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
            this.trackPixel.Size = new System.Drawing.Size(426, 49);
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
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel4);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(442, 837);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(436, 831);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(5, 20);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(426, 25);
            this.label13.TabIndex = 59;
            this.label13.Text = "Choose Area";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.17392F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.82608F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.btnMask, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropArea, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 45);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(426, 55);
            this.tableLayoutPanel3.TabIndex = 41;
            // 
            // btnMask
            // 
            this.btnMask.AutoFont = false;
            this.btnMask.AutoFontHeightRatio = 0.75F;
            this.btnMask.AutoFontMax = 100F;
            this.btnMask.AutoFontMin = 6F;
            this.btnMask.AutoFontWidthRatio = 0.92F;
            this.btnMask.AutoImage = true;
            this.btnMask.AutoImageMaxRatio = 0.75F;
            this.btnMask.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMask.AutoImageTint = true;
            this.btnMask.BackColor = System.Drawing.SystemColors.Control;
            this.btnMask.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnMask.BorderColor = System.Drawing.SystemColors.Control;
            this.btnMask.BorderRadius = 10;
            this.btnMask.BorderSize = 1;
            this.btnMask.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMask.Corner = BeeGlobal.Corner.Right;
            this.btnMask.DebounceResizeMs = 16;
            this.btnMask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMask.FlatAppearance.BorderSize = 0;
            this.btnMask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMask.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.62891F);
            this.btnMask.ForeColor = System.Drawing.Color.Black;
            this.btnMask.Image = null;
            this.btnMask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMask.ImageDisabled = null;
            this.btnMask.ImageHover = null;
            this.btnMask.ImageNormal = null;
            this.btnMask.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMask.ImagePressed = null;
            this.btnMask.ImageTextSpacing = 6;
            this.btnMask.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMask.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMask.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMask.ImageTintOpacity = 0.5F;
            this.btnMask.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMask.IsCLick = false;
            this.btnMask.IsNotChange = false;
            this.btnMask.IsRect = false;
            this.btnMask.IsUnGroup = false;
            this.btnMask.Location = new System.Drawing.Point(222, 5);
            this.btnMask.Margin = new System.Windows.Forms.Padding(0);
            this.btnMask.Multiline = false;
            this.btnMask.Name = "btnMask";
            this.btnMask.Size = new System.Drawing.Size(199, 45);
            this.btnMask.TabIndex = 4;
            this.btnMask.Text = "Area Mask";
            this.btnMask.TextColor = System.Drawing.Color.Black;
            this.btnMask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMask.UseVisualStyleBackColor = false;
            this.btnMask.Click += new System.EventHandler(this.btnMask_Click);
            // 
            // btnCropArea
            // 
            this.btnCropArea.AutoFont = false;
            this.btnCropArea.AutoFontHeightRatio = 0.75F;
            this.btnCropArea.AutoFontMax = 100F;
            this.btnCropArea.AutoFontMin = 6F;
            this.btnCropArea.AutoFontWidthRatio = 0.92F;
            this.btnCropArea.AutoImage = true;
            this.btnCropArea.AutoImageMaxRatio = 0.75F;
            this.btnCropArea.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropArea.AutoImageTint = true;
            this.btnCropArea.BackColor = System.Drawing.SystemColors.Control;
            this.btnCropArea.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCropArea.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCropArea.BorderRadius = 10;
            this.btnCropArea.BorderSize = 1;
            this.btnCropArea.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropArea.Corner = BeeGlobal.Corner.Left;
            this.btnCropArea.DebounceResizeMs = 16;
            this.btnCropArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.62891F);
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
            this.btnCropArea.Location = new System.Drawing.Point(5, 5);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropArea.Multiline = false;
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(217, 45);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
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
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.White;
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
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 100);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(426, 40);
            this.tableLayoutPanel5.TabIndex = 60;
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
            this.btnNone.Location = new System.Drawing.Point(315, 3);
            this.btnNone.Margin = new System.Windows.Forms.Padding(0);
            this.btnNone.Multiline = false;
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(108, 34);
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
            this.btnElip.Location = new System.Drawing.Point(209, 3);
            this.btnElip.Margin = new System.Windows.Forms.Padding(0);
            this.btnElip.Multiline = false;
            this.btnElip.Name = "btnElip";
            this.btnElip.Size = new System.Drawing.Size(106, 34);
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
            this.btnRect.Size = new System.Drawing.Size(106, 34);
            this.btnRect.TabIndex = 4;
            this.btnRect.Text = "Rectangle";
            this.btnRect.TextColor = System.Drawing.Color.Black;
            this.btnRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRect.UseVisualStyleBackColor = false;
            this.btnRect.Click += new System.EventHandler(this.btnRect_Click);
            // 
            // ToolColorArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.tabControl2);
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
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
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
        private RJButton btnInspect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnMask;
        private RJButton btnCropArea;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private AdjustBarEx trackPixel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label12;
        private RJButton btnNone;
        private RJButton btnElip;
        private RJButton btnRect;
    }
}
