
using BeeGlobal;

namespace BeeInterface
{
    partial class SettingStep1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingStep1));
            this.workRead = new System.ComponentModel.BackgroundWorker();
            this.tableLayout1 = new BeeInterface.DbTableLayoutPanel();
            this.btnDownLoadPara = new BeeInterface.RJButton();
            this.AdjOffSetY = new BeeInterface.AdjustBar();
            this.label13 = new System.Windows.Forms.Label();
            this.AdjOffsetX = new BeeInterface.AdjustBar();
            this.label12 = new System.Windows.Forms.Label();
            this.AdjHeight = new BeeInterface.AdjustBar();
            this.label11 = new System.Windows.Forms.Label();
            this.AdjWidth = new BeeInterface.AdjustBar();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.trackShift = new BeeInterface.AdjustBar();
            this.label3 = new System.Windows.Forms.Label();
            this.trackGain = new BeeInterface.AdjustBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackExposure = new BeeInterface.AdjustBar();
            this.tableLayoutPanel5 = new BeeInterface.DbTableLayoutPanel();
            this.btnCenterY = new BeeInterface.RJButton();
            this.btnCenterX = new BeeInterface.RJButton();
            this.tableLayoutPanel11 = new BeeInterface.DbTableLayoutPanel();
            this.btnCancel = new BeeInterface.RJButton();
            this.btnNextStep = new BeeInterface.RJButton();
            this.tableLayoutPanel10 = new BeeInterface.DbTableLayoutPanel();
            this.tableLayoutPanel4 = new BeeInterface.DbTableLayoutPanel();
            this.btnOn = new BeeInterface.RJButton();
            this.btnOFF = new BeeInterface.RJButton();
            this.tableLayoutPanel3 = new BeeInterface.DbTableLayoutPanel();
            this.btnLight3 = new BeeInterface.RJButton();
            this.btnLight1 = new BeeInterface.RJButton();
            this.btnLight2 = new BeeInterface.RJButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.layoutDelay = new BeeInterface.DbTableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.AdDelayOutput = new BeeInterface.AdjustBar();
            this.tableLayoutPanel1 = new BeeInterface.DbTableLayoutPanel();
            this.btnAllTime = new BeeInterface.RJButton();
            this.btnBlink = new BeeInterface.RJButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AdDelayTrig = new BeeInterface.AdjustBar();
            this.tableLayoutPanel6 = new BeeInterface.DbTableLayoutPanel();
            this.btnExternal = new BeeInterface.RJButton();
            this.btnInternal = new BeeInterface.RJButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tmShowPara = new System.Windows.Forms.Timer(this.components);
            this.tableLayout1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.layoutDelay.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayout1
            // 
            this.tableLayout1.AutoScroll = true;
            this.tableLayout1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayout1.ColumnCount = 1;
            this.tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout1.Controls.Add(this.btnDownLoadPara, 0, 0);
            this.tableLayout1.Controls.Add(this.AdjOffSetY, 0, 15);
            this.tableLayout1.Controls.Add(this.label13, 0, 14);
            this.tableLayout1.Controls.Add(this.AdjOffsetX, 0, 13);
            this.tableLayout1.Controls.Add(this.label12, 0, 12);
            this.tableLayout1.Controls.Add(this.AdjHeight, 0, 11);
            this.tableLayout1.Controls.Add(this.label11, 0, 10);
            this.tableLayout1.Controls.Add(this.AdjWidth, 0, 9);
            this.tableLayout1.Controls.Add(this.label10, 0, 8);
            this.tableLayout1.Controls.Add(this.label5, 0, 7);
            this.tableLayout1.Controls.Add(this.trackShift, 0, 6);
            this.tableLayout1.Controls.Add(this.label3, 0, 5);
            this.tableLayout1.Controls.Add(this.trackGain, 0, 4);
            this.tableLayout1.Controls.Add(this.label2, 0, 3);
            this.tableLayout1.Controls.Add(this.label4, 0, 1);
            this.tableLayout1.Controls.Add(this.trackExposure, 0, 2);
            this.tableLayout1.Controls.Add(this.tableLayoutPanel5, 0, 16);
            this.tableLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayout1.Location = new System.Drawing.Point(3, 3);
            this.tableLayout1.Name = "tableLayout1";
            this.tableLayout1.RowCount = 18;
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout1.Size = new System.Drawing.Size(480, 704);
            this.tableLayout1.TabIndex = 52;
            this.tableLayout1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayout1_Paint);
            // 
            // btnDownLoadPara
            // 
            this.btnDownLoadPara.AutoFont = true;
            this.btnDownLoadPara.AutoFontHeightRatio = 0.75F;
            this.btnDownLoadPara.AutoFontMax = 100F;
            this.btnDownLoadPara.AutoFontMin = 6F;
            this.btnDownLoadPara.AutoFontWidthRatio = 0.92F;
            this.btnDownLoadPara.AutoImage = true;
            this.btnDownLoadPara.AutoImageMaxRatio = 0.75F;
            this.btnDownLoadPara.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnDownLoadPara.AutoImageTint = true;
            this.btnDownLoadPara.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDownLoadPara.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnDownLoadPara.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDownLoadPara.BackgroundImage")));
            this.btnDownLoadPara.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnDownLoadPara.BorderRadius = 5;
            this.btnDownLoadPara.BorderSize = 1;
            this.btnDownLoadPara.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnDownLoadPara.Corner = BeeGlobal.Corner.Both;
            this.btnDownLoadPara.DebounceResizeMs = 16;
            this.btnDownLoadPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDownLoadPara.FlatAppearance.BorderSize = 0;
            this.btnDownLoadPara.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownLoadPara.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.6875F, System.Drawing.FontStyle.Bold);
            this.btnDownLoadPara.ForeColor = System.Drawing.Color.Black;
            this.btnDownLoadPara.Image = null;
            this.btnDownLoadPara.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownLoadPara.ImageDisabled = null;
            this.btnDownLoadPara.ImageHover = null;
            this.btnDownLoadPara.ImageNormal = null;
            this.btnDownLoadPara.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnDownLoadPara.ImagePressed = null;
            this.btnDownLoadPara.ImageTextSpacing = 6;
            this.btnDownLoadPara.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnDownLoadPara.ImageTintHover = System.Drawing.Color.Empty;
            this.btnDownLoadPara.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnDownLoadPara.ImageTintOpacity = 0.5F;
            this.btnDownLoadPara.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnDownLoadPara.IsCLick = false;
            this.btnDownLoadPara.IsNotChange = true;
            this.btnDownLoadPara.IsRect = false;
            this.btnDownLoadPara.IsUnGroup = false;
            this.btnDownLoadPara.Location = new System.Drawing.Point(3, 3);
            this.btnDownLoadPara.Multiline = false;
            this.btnDownLoadPara.Name = "btnDownLoadPara";
            this.btnDownLoadPara.Size = new System.Drawing.Size(474, 58);
            this.btnDownLoadPara.TabIndex = 79;
            this.btnDownLoadPara.Text = "DownLoad Parameter Camera";
            this.btnDownLoadPara.TextColor = System.Drawing.Color.Black;
            this.btnDownLoadPara.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownLoadPara.UseVisualStyleBackColor = false;
            this.btnDownLoadPara.Click += new System.EventHandler(this.btnDownLoadPara_Click);
            // 
            // AdjOffSetY
            // 
            this.AdjOffSetY.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjOffSetY.BackColor = System.Drawing.SystemColors.Control;
            this.AdjOffSetY.Location = new System.Drawing.Point(20, 661);
            this.AdjOffSetY.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.AdjOffSetY.Max = 100F;
            this.AdjOffSetY.Min = 0F;
            this.AdjOffSetY.Name = "AdjOffSetY";
            this.AdjOffSetY.Size = new System.Drawing.Size(457, 55);
            this.AdjOffSetY.Step = 1F;
            this.AdjOffSetY.TabIndex = 78;
            this.AdjOffSetY.Value = 1F;
            this.AdjOffSetY.ValueChanged += new System.Action<float>(this.AdjOffSetY_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(15, 635);
            this.label13.Margin = new System.Windows.Forms.Padding(15, 5, 3, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(462, 20);
            this.label13.TabIndex = 77;
            this.label13.Text = "OffSet Y";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdjOffsetX
            // 
            this.AdjOffsetX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjOffsetX.Location = new System.Drawing.Point(20, 572);
            this.AdjOffsetX.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.AdjOffsetX.Max = 100F;
            this.AdjOffsetX.Min = 0F;
            this.AdjOffsetX.Name = "AdjOffsetX";
            this.AdjOffsetX.Size = new System.Drawing.Size(457, 55);
            this.AdjOffsetX.Step = 1F;
            this.AdjOffsetX.TabIndex = 76;
            this.AdjOffsetX.Value = 1F;
            this.AdjOffsetX.ValueChanged += new System.Action<float>(this.AdjOffsetX_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(15, 546);
            this.label12.Margin = new System.Windows.Forms.Padding(15, 5, 3, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(462, 20);
            this.label12.TabIndex = 75;
            this.label12.Text = "OffSet X";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdjHeight
            // 
            this.AdjHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjHeight.Location = new System.Drawing.Point(20, 483);
            this.AdjHeight.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.AdjHeight.Max = 100F;
            this.AdjHeight.Min = 0F;
            this.AdjHeight.Name = "AdjHeight";
            this.AdjHeight.Size = new System.Drawing.Size(457, 55);
            this.AdjHeight.Step = 1F;
            this.AdjHeight.TabIndex = 74;
            this.AdjHeight.Value = 1F;
            this.AdjHeight.ValueChanged += new System.Action<float>(this.AdjHeight_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(15, 460);
            this.label11.Margin = new System.Windows.Forms.Padding(15, 5, 3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(462, 20);
            this.label11.TabIndex = 73;
            this.label11.Text = "Hight";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdjWidth
            // 
            this.AdjWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjWidth.Location = new System.Drawing.Point(20, 397);
            this.AdjWidth.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.AdjWidth.Max = 100F;
            this.AdjWidth.Min = 0F;
            this.AdjWidth.Name = "AdjWidth";
            this.AdjWidth.Size = new System.Drawing.Size(457, 55);
            this.AdjWidth.Step = 1F;
            this.AdjWidth.TabIndex = 72;
            this.AdjWidth.Value = 1F;
            this.AdjWidth.ValueChanged += new System.Action<float>(this.AdjWidth_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(15, 374);
            this.label10.Margin = new System.Windows.Forms.Padding(15, 5, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(462, 20);
            this.label10.TabIndex = 71;
            this.label10.Text = "Width";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 342);
            this.label5.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(477, 24);
            this.label5.TabIndex = 70;
            this.label5.Text = "ReSolution";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackShift
            // 
            this.trackShift.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackShift.Location = new System.Drawing.Point(10, 279);
            this.trackShift.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackShift.Max = 100F;
            this.trackShift.Min = 0F;
            this.trackShift.Name = "trackShift";
            this.trackShift.Size = new System.Drawing.Size(467, 55);
            this.trackShift.Step = 1F;
            this.trackShift.TabIndex = 69;
            this.trackShift.Value = 1F;
            this.trackShift.ValueChanged += new System.Action<float>(this.trackShift_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 252);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(477, 24);
            this.label3.TabIndex = 68;
            this.label3.Text = "Shift";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackGain
            // 
            this.trackGain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackGain.Location = new System.Drawing.Point(10, 189);
            this.trackGain.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackGain.Max = 100F;
            this.trackGain.Min = 0F;
            this.trackGain.Name = "trackGain";
            this.trackGain.Size = new System.Drawing.Size(467, 55);
            this.trackGain.Step = 1F;
            this.trackGain.TabIndex = 67;
            this.trackGain.Value = 1F;
            this.trackGain.ValueChanged += new System.Action<float>(this.trackGain_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 162);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(477, 24);
            this.label2.TabIndex = 66;
            this.label2.Text = "Gain";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 69);
            this.label4.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(477, 24);
            this.label4.TabIndex = 65;
            this.label4.Text = "Exposue Time";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackExposure
            // 
            this.trackExposure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackExposure.Location = new System.Drawing.Point(10, 99);
            this.trackExposure.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackExposure.Max = 100F;
            this.trackExposure.Min = 0F;
            this.trackExposure.Name = "trackExposure";
            this.trackExposure.Size = new System.Drawing.Size(467, 55);
            this.trackExposure.Step = 1F;
            this.trackExposure.TabIndex = 64;
            this.trackExposure.Value = 1F;
            this.trackExposure.ValueChanged += new System.Action<float>(this.trackExposure_ValueChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.btnCenterY, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnCenterX, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(20, 722);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(457, 60);
            this.tableLayoutPanel5.TabIndex = 80;
            // 
            // btnCenterY
            // 
            this.btnCenterY.AutoFont = true;
            this.btnCenterY.AutoFontHeightRatio = 0.75F;
            this.btnCenterY.AutoFontMax = 100F;
            this.btnCenterY.AutoFontMin = 6F;
            this.btnCenterY.AutoFontWidthRatio = 0.92F;
            this.btnCenterY.AutoImage = true;
            this.btnCenterY.AutoImageMaxRatio = 0.75F;
            this.btnCenterY.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCenterY.AutoImageTint = true;
            this.btnCenterY.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCenterY.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCenterY.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCenterY.BackgroundImage")));
            this.btnCenterY.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnCenterY.BorderRadius = 5;
            this.btnCenterY.BorderSize = 1;
            this.btnCenterY.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCenterY.Corner = BeeGlobal.Corner.Both;
            this.btnCenterY.DebounceResizeMs = 16;
            this.btnCenterY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenterY.FlatAppearance.BorderSize = 0;
            this.btnCenterY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCenterY.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnCenterY.ForeColor = System.Drawing.Color.Black;
            this.btnCenterY.Image = null;
            this.btnCenterY.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCenterY.ImageDisabled = null;
            this.btnCenterY.ImageHover = null;
            this.btnCenterY.ImageNormal = null;
            this.btnCenterY.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCenterY.ImagePressed = null;
            this.btnCenterY.ImageTextSpacing = 6;
            this.btnCenterY.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCenterY.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCenterY.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCenterY.ImageTintOpacity = 0.5F;
            this.btnCenterY.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCenterY.IsCLick = false;
            this.btnCenterY.IsNotChange = false;
            this.btnCenterY.IsRect = false;
            this.btnCenterY.IsUnGroup = true;
            this.btnCenterY.Location = new System.Drawing.Point(231, 3);
            this.btnCenterY.Multiline = false;
            this.btnCenterY.Name = "btnCenterY";
            this.btnCenterY.Size = new System.Drawing.Size(223, 54);
            this.btnCenterY.TabIndex = 60;
            this.btnCenterY.Text = "CenterY";
            this.btnCenterY.TextColor = System.Drawing.Color.Black;
            this.btnCenterY.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCenterY.UseVisualStyleBackColor = false;
            this.btnCenterY.Click += new System.EventHandler(this.btnCenterY_Click);
            // 
            // btnCenterX
            // 
            this.btnCenterX.AutoFont = true;
            this.btnCenterX.AutoFontHeightRatio = 0.75F;
            this.btnCenterX.AutoFontMax = 100F;
            this.btnCenterX.AutoFontMin = 6F;
            this.btnCenterX.AutoFontWidthRatio = 0.92F;
            this.btnCenterX.AutoImage = true;
            this.btnCenterX.AutoImageMaxRatio = 0.75F;
            this.btnCenterX.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCenterX.AutoImageTint = true;
            this.btnCenterX.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCenterX.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCenterX.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCenterX.BackgroundImage")));
            this.btnCenterX.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnCenterX.BorderRadius = 5;
            this.btnCenterX.BorderSize = 1;
            this.btnCenterX.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCenterX.Corner = BeeGlobal.Corner.Both;
            this.btnCenterX.DebounceResizeMs = 16;
            this.btnCenterX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenterX.FlatAppearance.BorderSize = 0;
            this.btnCenterX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCenterX.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnCenterX.ForeColor = System.Drawing.Color.Black;
            this.btnCenterX.Image = null;
            this.btnCenterX.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCenterX.ImageDisabled = null;
            this.btnCenterX.ImageHover = null;
            this.btnCenterX.ImageNormal = null;
            this.btnCenterX.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCenterX.ImagePressed = null;
            this.btnCenterX.ImageTextSpacing = 6;
            this.btnCenterX.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCenterX.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCenterX.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCenterX.ImageTintOpacity = 0.5F;
            this.btnCenterX.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCenterX.IsCLick = false;
            this.btnCenterX.IsNotChange = false;
            this.btnCenterX.IsRect = false;
            this.btnCenterX.IsUnGroup = true;
            this.btnCenterX.Location = new System.Drawing.Point(3, 3);
            this.btnCenterX.Multiline = false;
            this.btnCenterX.Name = "btnCenterX";
            this.btnCenterX.Size = new System.Drawing.Size(222, 54);
            this.btnCenterX.TabIndex = 59;
            this.btnCenterX.Text = "CenterX";
            this.btnCenterX.TextColor = System.Drawing.Color.Black;
            this.btnCenterX.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCenterX.UseVisualStyleBackColor = false;
            this.btnCenterX.Click += new System.EventHandler(this.btnCenterX_Click);
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.2844F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.7156F));
            this.tableLayoutPanel11.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.btnNextStep, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 751);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(494, 68);
            this.tableLayoutPanel11.TabIndex = 59;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoFont = true;
            this.btnCancel.AutoFontHeightRatio = 0.75F;
            this.btnCancel.AutoFontMax = 100F;
            this.btnCancel.AutoFontMin = 6F;
            this.btnCancel.AutoFontWidthRatio = 0.92F;
            this.btnCancel.AutoImage = true;
            this.btnCancel.AutoImageMaxRatio = 0.75F;
            this.btnCancel.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCancel.AutoImageTint = true;
            this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.BorderRadius = 5;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCancel.Corner = BeeGlobal.Corner.Both;
            this.btnCancel.DebounceResizeMs = 16;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.99219F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = null;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.ImageDisabled = null;
            this.btnCancel.ImageHover = null;
            this.btnCancel.ImageNormal = null;
            this.btnCancel.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCancel.ImagePressed = null;
            this.btnCancel.ImageTextSpacing = 6;
            this.btnCancel.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCancel.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCancel.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCancel.ImageTintOpacity = 0.5F;
            this.btnCancel.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCancel.IsCLick = false;
            this.btnCancel.IsNotChange = true;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(330, 3);
            this.btnCancel.Multiline = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(161, 62);
            this.btnCancel.TabIndex = 58;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.Black;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNextStep
            // 
            this.btnNextStep.AutoFont = true;
            this.btnNextStep.AutoFontHeightRatio = 0.75F;
            this.btnNextStep.AutoFontMax = 100F;
            this.btnNextStep.AutoFontMin = 6F;
            this.btnNextStep.AutoFontWidthRatio = 0.92F;
            this.btnNextStep.AutoImage = true;
            this.btnNextStep.AutoImageMaxRatio = 0.75F;
            this.btnNextStep.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnNextStep.AutoImageTint = true;
            this.btnNextStep.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNextStep.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnNextStep.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNextStep.BackgroundImage")));
            this.btnNextStep.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnNextStep.BorderRadius = 5;
            this.btnNextStep.BorderSize = 1;
            this.btnNextStep.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnNextStep.Corner = BeeGlobal.Corner.Both;
            this.btnNextStep.DebounceResizeMs = 16;
            this.btnNextStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNextStep.FlatAppearance.BorderSize = 0;
            this.btnNextStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.99219F, System.Drawing.FontStyle.Bold);
            this.btnNextStep.ForeColor = System.Drawing.Color.Black;
            this.btnNextStep.Image = null;
            this.btnNextStep.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNextStep.ImageDisabled = null;
            this.btnNextStep.ImageHover = null;
            this.btnNextStep.ImageNormal = null;
            this.btnNextStep.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnNextStep.ImagePressed = null;
            this.btnNextStep.ImageTextSpacing = 6;
            this.btnNextStep.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnNextStep.ImageTintHover = System.Drawing.Color.Empty;
            this.btnNextStep.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnNextStep.ImageTintOpacity = 0.5F;
            this.btnNextStep.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnNextStep.IsCLick = true;
            this.btnNextStep.IsNotChange = true;
            this.btnNextStep.IsRect = false;
            this.btnNextStep.IsUnGroup = false;
            this.btnNextStep.Location = new System.Drawing.Point(3, 3);
            this.btnNextStep.Multiline = false;
            this.btnNextStep.Name = "btnNextStep";
            this.btnNextStep.Size = new System.Drawing.Size(321, 62);
            this.btnNextStep.TabIndex = 57;
            this.btnNextStep.Text = "NextStep";
            this.btnNextStep.TextColor = System.Drawing.Color.Black;
            this.btnNextStep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNextStep.UseVisualStyleBackColor = false;
            this.btnNextStep.Click += new System.EventHandler(this.btnNextStep_Click);
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.AutoScroll = true;
            this.tableLayoutPanel10.AutoSize = true;
            this.tableLayoutPanel10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel4, 0, 9);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel3, 0, 8);
            this.tableLayoutPanel10.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel10.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.layoutDelay, 0, 6);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel1, 0, 5);
            this.tableLayoutPanel10.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel10.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.AdDelayTrig, 0, 3);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 11;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(480, 704);
            this.tableLayoutPanel10.TabIndex = 53;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.btnOn, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnOFF, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(10, 444);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(460, 60);
            this.tableLayoutPanel4.TabIndex = 74;
            // 
            // btnOn
            // 
            this.btnOn.AutoFont = true;
            this.btnOn.AutoFontHeightRatio = 0.65F;
            this.btnOn.AutoFontMax = 100F;
            this.btnOn.AutoFontMin = 6F;
            this.btnOn.AutoFontWidthRatio = 0.92F;
            this.btnOn.AutoImage = true;
            this.btnOn.AutoImageMaxRatio = 0.75F;
            this.btnOn.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnOn.AutoImageTint = true;
            this.btnOn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnOn.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnOn.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnOn.BorderRadius = 10;
            this.btnOn.BorderSize = 1;
            this.btnOn.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOn.Corner = BeeGlobal.Corner.Right;
            this.btnOn.DebounceResizeMs = 16;
            this.btnOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOn.FlatAppearance.BorderSize = 0;
            this.btnOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnOn.ForeColor = System.Drawing.Color.Black;
            this.btnOn.Image = null;
            this.btnOn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOn.ImageDisabled = null;
            this.btnOn.ImageHover = null;
            this.btnOn.ImageNormal = null;
            this.btnOn.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnOn.ImagePressed = null;
            this.btnOn.ImageTextSpacing = 6;
            this.btnOn.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnOn.ImageTintHover = System.Drawing.Color.Empty;
            this.btnOn.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnOn.ImageTintOpacity = 0.5F;
            this.btnOn.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnOn.IsCLick = false;
            this.btnOn.IsNotChange = false;
            this.btnOn.IsRect = false;
            this.btnOn.IsUnGroup = false;
            this.btnOn.Location = new System.Drawing.Point(230, 0);
            this.btnOn.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnOn.Multiline = false;
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(227, 60);
            this.btnOn.TabIndex = 3;
            this.btnOn.Text = "ON";
            this.btnOn.TextColor = System.Drawing.Color.Black;
            this.btnOn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOn.UseVisualStyleBackColor = false;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // btnOFF
            // 
            this.btnOFF.AutoFont = true;
            this.btnOFF.AutoFontHeightRatio = 0.65F;
            this.btnOFF.AutoFontMax = 100F;
            this.btnOFF.AutoFontMin = 6F;
            this.btnOFF.AutoFontWidthRatio = 0.92F;
            this.btnOFF.AutoImage = true;
            this.btnOFF.AutoImageMaxRatio = 0.75F;
            this.btnOFF.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnOFF.AutoImageTint = true;
            this.btnOFF.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnOFF.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnOFF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOFF.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnOFF.BorderRadius = 10;
            this.btnOFF.BorderSize = 1;
            this.btnOFF.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOFF.Corner = BeeGlobal.Corner.Left;
            this.btnOFF.DebounceResizeMs = 16;
            this.btnOFF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOFF.FlatAppearance.BorderSize = 0;
            this.btnOFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnOFF.ForeColor = System.Drawing.Color.Black;
            this.btnOFF.Image = null;
            this.btnOFF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOFF.ImageDisabled = null;
            this.btnOFF.ImageHover = null;
            this.btnOFF.ImageNormal = null;
            this.btnOFF.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnOFF.ImagePressed = null;
            this.btnOFF.ImageTextSpacing = 6;
            this.btnOFF.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnOFF.ImageTintHover = System.Drawing.Color.Empty;
            this.btnOFF.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnOFF.ImageTintOpacity = 0.5F;
            this.btnOFF.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnOFF.IsCLick = true;
            this.btnOFF.IsNotChange = false;
            this.btnOFF.IsRect = false;
            this.btnOFF.IsUnGroup = false;
            this.btnOFF.Location = new System.Drawing.Point(3, 0);
            this.btnOFF.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnOFF.Multiline = false;
            this.btnOFF.Name = "btnOFF";
            this.btnOFF.Size = new System.Drawing.Size(227, 60);
            this.btnOFF.TabIndex = 2;
            this.btnOFF.Text = "OFF";
            this.btnOFF.TextColor = System.Drawing.Color.Black;
            this.btnOFF.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOFF.UseVisualStyleBackColor = false;
            this.btnOFF.Click += new System.EventHandler(this.btnOFF_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.Controls.Add(this.btnLight3, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLight1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLight2, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 379);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(472, 60);
            this.tableLayoutPanel3.TabIndex = 73;
            // 
            // btnLight3
            // 
            this.btnLight3.AutoFont = true;
            this.btnLight3.AutoFontHeightRatio = 0.65F;
            this.btnLight3.AutoFontMax = 100F;
            this.btnLight3.AutoFontMin = 6F;
            this.btnLight3.AutoFontWidthRatio = 0.92F;
            this.btnLight3.AutoImage = true;
            this.btnLight3.AutoImageMaxRatio = 0.75F;
            this.btnLight3.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLight3.AutoImageTint = true;
            this.btnLight3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight3.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLight3.BackgroundImage")));
            this.btnLight3.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight3.BorderRadius = 10;
            this.btnLight3.BorderSize = 1;
            this.btnLight3.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLight3.Corner = BeeGlobal.Corner.Right;
            this.btnLight3.DebounceResizeMs = 16;
            this.btnLight3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLight3.FlatAppearance.BorderSize = 0;
            this.btnLight3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLight3.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnLight3.ForeColor = System.Drawing.Color.Black;
            this.btnLight3.Image = null;
            this.btnLight3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLight3.ImageDisabled = null;
            this.btnLight3.ImageHover = null;
            this.btnLight3.ImageNormal = null;
            this.btnLight3.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLight3.ImagePressed = null;
            this.btnLight3.ImageTextSpacing = 6;
            this.btnLight3.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLight3.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLight3.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLight3.ImageTintOpacity = 0.5F;
            this.btnLight3.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLight3.IsCLick = false;
            this.btnLight3.IsNotChange = false;
            this.btnLight3.IsRect = false;
            this.btnLight3.IsUnGroup = true;
            this.btnLight3.Location = new System.Drawing.Point(318, 0);
            this.btnLight3.Margin = new System.Windows.Forms.Padding(0);
            this.btnLight3.Multiline = false;
            this.btnLight3.Name = "btnLight3";
            this.btnLight3.Size = new System.Drawing.Size(154, 60);
            this.btnLight3.TabIndex = 4;
            this.btnLight3.Text = "Light 3";
            this.btnLight3.TextColor = System.Drawing.Color.Black;
            this.btnLight3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLight3.UseVisualStyleBackColor = false;
            this.btnLight3.Click += new System.EventHandler(this.btnLight3_Click);
            // 
            // btnLight1
            // 
            this.btnLight1.AutoFont = true;
            this.btnLight1.AutoFontHeightRatio = 0.65F;
            this.btnLight1.AutoFontMax = 100F;
            this.btnLight1.AutoFontMin = 6F;
            this.btnLight1.AutoFontWidthRatio = 0.92F;
            this.btnLight1.AutoImage = true;
            this.btnLight1.AutoImageMaxRatio = 0.75F;
            this.btnLight1.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLight1.AutoImageTint = true;
            this.btnLight1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLight1.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight1.BorderRadius = 10;
            this.btnLight1.BorderSize = 1;
            this.btnLight1.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLight1.Corner = BeeGlobal.Corner.Left;
            this.btnLight1.DebounceResizeMs = 16;
            this.btnLight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLight1.FlatAppearance.BorderSize = 0;
            this.btnLight1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLight1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnLight1.ForeColor = System.Drawing.Color.Black;
            this.btnLight1.Image = null;
            this.btnLight1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLight1.ImageDisabled = null;
            this.btnLight1.ImageHover = null;
            this.btnLight1.ImageNormal = null;
            this.btnLight1.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLight1.ImagePressed = null;
            this.btnLight1.ImageTextSpacing = 6;
            this.btnLight1.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLight1.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLight1.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLight1.ImageTintOpacity = 0.5F;
            this.btnLight1.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLight1.IsCLick = true;
            this.btnLight1.IsNotChange = false;
            this.btnLight1.IsRect = false;
            this.btnLight1.IsUnGroup = true;
            this.btnLight1.Location = new System.Drawing.Point(0, 0);
            this.btnLight1.Margin = new System.Windows.Forms.Padding(0);
            this.btnLight1.Multiline = false;
            this.btnLight1.Name = "btnLight1";
            this.btnLight1.Size = new System.Drawing.Size(152, 60);
            this.btnLight1.TabIndex = 2;
            this.btnLight1.Text = "Light 1";
            this.btnLight1.TextColor = System.Drawing.Color.Black;
            this.btnLight1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLight1.UseVisualStyleBackColor = false;
            this.btnLight1.Click += new System.EventHandler(this.btnLight1_Click);
            // 
            // btnLight2
            // 
            this.btnLight2.AutoFont = true;
            this.btnLight2.AutoFontHeightRatio = 0.65F;
            this.btnLight2.AutoFontMax = 100F;
            this.btnLight2.AutoFontMin = 6F;
            this.btnLight2.AutoFontWidthRatio = 0.92F;
            this.btnLight2.AutoImage = true;
            this.btnLight2.AutoImageMaxRatio = 0.75F;
            this.btnLight2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLight2.AutoImageTint = true;
            this.btnLight2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight2.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLight2.BackgroundImage")));
            this.btnLight2.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight2.BorderRadius = 5;
            this.btnLight2.BorderSize = 1;
            this.btnLight2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLight2.Corner = BeeGlobal.Corner.None;
            this.btnLight2.DebounceResizeMs = 16;
            this.btnLight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLight2.FlatAppearance.BorderSize = 0;
            this.btnLight2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLight2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnLight2.ForeColor = System.Drawing.Color.Black;
            this.btnLight2.Image = null;
            this.btnLight2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLight2.ImageDisabled = null;
            this.btnLight2.ImageHover = null;
            this.btnLight2.ImageNormal = null;
            this.btnLight2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLight2.ImagePressed = null;
            this.btnLight2.ImageTextSpacing = 6;
            this.btnLight2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLight2.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLight2.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLight2.ImageTintOpacity = 0.5F;
            this.btnLight2.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLight2.IsCLick = false;
            this.btnLight2.IsNotChange = false;
            this.btnLight2.IsRect = false;
            this.btnLight2.IsUnGroup = true;
            this.btnLight2.Location = new System.Drawing.Point(152, 0);
            this.btnLight2.Margin = new System.Windows.Forms.Padding(0);
            this.btnLight2.Multiline = false;
            this.btnLight2.Name = "btnLight2";
            this.btnLight2.Size = new System.Drawing.Size(166, 60);
            this.btnLight2.TabIndex = 3;
            this.btnLight2.Text = "Light 2";
            this.btnLight2.TextColor = System.Drawing.Color.Black;
            this.btnLight2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLight2.UseVisualStyleBackColor = false;
            this.btnLight2.Click += new System.EventHandler(this.btnLight2_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(0, 352);
            this.label9.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(477, 24);
            this.label9.TabIndex = 72;
            this.label9.Text = "Light";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(0, 5);
            this.label8.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(477, 24);
            this.label8.TabIndex = 71;
            this.label8.Text = "Methord Trigger";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layoutDelay
            // 
            this.layoutDelay.BackColor = System.Drawing.SystemColors.Control;
            this.layoutDelay.ColumnCount = 2;
            this.layoutDelay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.26667F));
            this.layoutDelay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.73333F));
            this.layoutDelay.Controls.Add(this.label7, 0, 0);
            this.layoutDelay.Controls.Add(this.AdDelayOutput, 1, 0);
            this.layoutDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDelay.Location = new System.Drawing.Point(5, 287);
            this.layoutDelay.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.layoutDelay.Name = "layoutDelay";
            this.layoutDelay.RowCount = 1;
            this.layoutDelay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDelay.Size = new System.Drawing.Size(472, 60);
            this.layoutDelay.TabIndex = 70;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(8, 5, 3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 52);
            this.label7.TabIndex = 69;
            this.label7.Text = "Delay";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AdDelayOutput
            // 
            this.AdDelayOutput.BackColor = System.Drawing.SystemColors.Control;
            this.AdDelayOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdDelayOutput.Location = new System.Drawing.Point(117, 3);
            this.AdDelayOutput.Max = 1000F;
            this.AdDelayOutput.Min = 1F;
            this.AdDelayOutput.Name = "AdDelayOutput";
            this.AdDelayOutput.Size = new System.Drawing.Size(352, 54);
            this.AdDelayOutput.Step = 1F;
            this.AdDelayOutput.TabIndex = 70;
            this.AdDelayOutput.Value = 0F;
            this.AdDelayOutput.ValueChanged += new System.Action<float>(this.AdDelayOutput_ValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnAllTime, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBlink, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 227);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(472, 60);
            this.tableLayoutPanel1.TabIndex = 69;
            // 
            // btnAllTime
            // 
            this.btnAllTime.AutoFont = true;
            this.btnAllTime.AutoFontHeightRatio = 0.65F;
            this.btnAllTime.AutoFontMax = 100F;
            this.btnAllTime.AutoFontMin = 6F;
            this.btnAllTime.AutoFontWidthRatio = 0.92F;
            this.btnAllTime.AutoImage = true;
            this.btnAllTime.AutoImageMaxRatio = 0.75F;
            this.btnAllTime.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAllTime.AutoImageTint = true;
            this.btnAllTime.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAllTime.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnAllTime.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnAllTime.BorderRadius = 10;
            this.btnAllTime.BorderSize = 1;
            this.btnAllTime.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAllTime.Corner = BeeGlobal.Corner.Right;
            this.btnAllTime.DebounceResizeMs = 16;
            this.btnAllTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAllTime.FlatAppearance.BorderSize = 0;
            this.btnAllTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAllTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnAllTime.ForeColor = System.Drawing.Color.Black;
            this.btnAllTime.Image = null;
            this.btnAllTime.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAllTime.ImageDisabled = null;
            this.btnAllTime.ImageHover = null;
            this.btnAllTime.ImageNormal = null;
            this.btnAllTime.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAllTime.ImagePressed = null;
            this.btnAllTime.ImageTextSpacing = 6;
            this.btnAllTime.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAllTime.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAllTime.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAllTime.ImageTintOpacity = 0.5F;
            this.btnAllTime.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAllTime.IsCLick = false;
            this.btnAllTime.IsNotChange = false;
            this.btnAllTime.IsRect = false;
            this.btnAllTime.IsUnGroup = false;
            this.btnAllTime.Location = new System.Drawing.Point(236, 0);
            this.btnAllTime.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnAllTime.Multiline = false;
            this.btnAllTime.Name = "btnAllTime";
            this.btnAllTime.Size = new System.Drawing.Size(233, 60);
            this.btnAllTime.TabIndex = 3;
            this.btnAllTime.Text = "All Time";
            this.btnAllTime.TextColor = System.Drawing.Color.Black;
            this.btnAllTime.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAllTime.UseVisualStyleBackColor = false;
            this.btnAllTime.Click += new System.EventHandler(this.btnAllTime_Click);
            // 
            // btnBlink
            // 
            this.btnBlink.AutoFont = true;
            this.btnBlink.AutoFontHeightRatio = 0.65F;
            this.btnBlink.AutoFontMax = 100F;
            this.btnBlink.AutoFontMin = 6F;
            this.btnBlink.AutoFontWidthRatio = 0.92F;
            this.btnBlink.AutoImage = true;
            this.btnBlink.AutoImageMaxRatio = 0.75F;
            this.btnBlink.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnBlink.AutoImageTint = true;
            this.btnBlink.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnBlink.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnBlink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBlink.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnBlink.BorderRadius = 10;
            this.btnBlink.BorderSize = 1;
            this.btnBlink.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnBlink.Corner = BeeGlobal.Corner.Left;
            this.btnBlink.DebounceResizeMs = 16;
            this.btnBlink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBlink.FlatAppearance.BorderSize = 0;
            this.btnBlink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlink.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnBlink.ForeColor = System.Drawing.Color.Black;
            this.btnBlink.Image = null;
            this.btnBlink.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBlink.ImageDisabled = null;
            this.btnBlink.ImageHover = null;
            this.btnBlink.ImageNormal = null;
            this.btnBlink.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnBlink.ImagePressed = null;
            this.btnBlink.ImageTextSpacing = 6;
            this.btnBlink.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnBlink.ImageTintHover = System.Drawing.Color.Empty;
            this.btnBlink.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnBlink.ImageTintOpacity = 0.5F;
            this.btnBlink.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnBlink.IsCLick = true;
            this.btnBlink.IsNotChange = false;
            this.btnBlink.IsRect = false;
            this.btnBlink.IsUnGroup = false;
            this.btnBlink.Location = new System.Drawing.Point(3, 0);
            this.btnBlink.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnBlink.Multiline = false;
            this.btnBlink.Name = "btnBlink";
            this.btnBlink.Size = new System.Drawing.Size(233, 60);
            this.btnBlink.TabIndex = 2;
            this.btnBlink.Text = "Blink";
            this.btnBlink.TextColor = System.Drawing.Color.Black;
            this.btnBlink.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBlink.UseVisualStyleBackColor = false;
            this.btnBlink.Click += new System.EventHandler(this.btnBlink_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 200);
            this.label6.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(477, 24);
            this.label6.TabIndex = 68;
            this.label6.Text = "Methord OutPut";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 107);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(477, 24);
            this.label1.TabIndex = 66;
            this.label1.Text = "Delay Trigger (ms)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdDelayTrig
            // 
            this.AdDelayTrig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdDelayTrig.Location = new System.Drawing.Point(3, 137);
            this.AdDelayTrig.Max = 2000F;
            this.AdDelayTrig.Min = 1F;
            this.AdDelayTrig.Name = "AdDelayTrig";
            this.AdDelayTrig.Size = new System.Drawing.Size(474, 55);
            this.AdDelayTrig.Step = 1F;
            this.AdDelayTrig.TabIndex = 67;
            this.AdDelayTrig.Value = 1F;
            this.AdDelayTrig.ValueChanged += new System.Action<float>(this.AdDelayTrig_ValueChanged);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnExternal, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnInternal, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 37);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(470, 60);
            this.tableLayoutPanel6.TabIndex = 44;
            // 
            // btnExternal
            // 
            this.btnExternal.AutoFont = true;
            this.btnExternal.AutoFontHeightRatio = 0.65F;
            this.btnExternal.AutoFontMax = 100F;
            this.btnExternal.AutoFontMin = 6F;
            this.btnExternal.AutoFontWidthRatio = 0.92F;
            this.btnExternal.AutoImage = true;
            this.btnExternal.AutoImageMaxRatio = 0.75F;
            this.btnExternal.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnExternal.AutoImageTint = true;
            this.btnExternal.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExternal.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnExternal.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnExternal.BorderRadius = 10;
            this.btnExternal.BorderSize = 1;
            this.btnExternal.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnExternal.Corner = BeeGlobal.Corner.Right;
            this.btnExternal.DebounceResizeMs = 16;
            this.btnExternal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExternal.FlatAppearance.BorderSize = 0;
            this.btnExternal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExternal.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnExternal.ForeColor = System.Drawing.Color.Black;
            this.btnExternal.Image = null;
            this.btnExternal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExternal.ImageDisabled = null;
            this.btnExternal.ImageHover = null;
            this.btnExternal.ImageNormal = null;
            this.btnExternal.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnExternal.ImagePressed = null;
            this.btnExternal.ImageTextSpacing = 6;
            this.btnExternal.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnExternal.ImageTintHover = System.Drawing.Color.Empty;
            this.btnExternal.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnExternal.ImageTintOpacity = 0.5F;
            this.btnExternal.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnExternal.IsCLick = false;
            this.btnExternal.IsNotChange = false;
            this.btnExternal.IsRect = false;
            this.btnExternal.IsUnGroup = false;
            this.btnExternal.Location = new System.Drawing.Point(235, 0);
            this.btnExternal.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnExternal.Multiline = false;
            this.btnExternal.Name = "btnExternal";
            this.btnExternal.Size = new System.Drawing.Size(232, 60);
            this.btnExternal.TabIndex = 3;
            this.btnExternal.Text = "External";
            this.btnExternal.TextColor = System.Drawing.Color.Black;
            this.btnExternal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExternal.UseVisualStyleBackColor = false;
            this.btnExternal.Click += new System.EventHandler(this.btnExternal_Click);
            // 
            // btnInternal
            // 
            this.btnInternal.AutoFont = true;
            this.btnInternal.AutoFontHeightRatio = 0.65F;
            this.btnInternal.AutoFontMax = 100F;
            this.btnInternal.AutoFontMin = 6F;
            this.btnInternal.AutoFontWidthRatio = 0.92F;
            this.btnInternal.AutoImage = true;
            this.btnInternal.AutoImageMaxRatio = 0.75F;
            this.btnInternal.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnInternal.AutoImageTint = true;
            this.btnInternal.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnInternal.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnInternal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInternal.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnInternal.BorderRadius = 10;
            this.btnInternal.BorderSize = 1;
            this.btnInternal.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnInternal.Corner = BeeGlobal.Corner.Left;
            this.btnInternal.DebounceResizeMs = 16;
            this.btnInternal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInternal.FlatAppearance.BorderSize = 0;
            this.btnInternal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInternal.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnInternal.ForeColor = System.Drawing.Color.Black;
            this.btnInternal.Image = null;
            this.btnInternal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInternal.ImageDisabled = null;
            this.btnInternal.ImageHover = null;
            this.btnInternal.ImageNormal = null;
            this.btnInternal.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnInternal.ImagePressed = null;
            this.btnInternal.ImageTextSpacing = 6;
            this.btnInternal.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnInternal.ImageTintHover = System.Drawing.Color.Empty;
            this.btnInternal.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnInternal.ImageTintOpacity = 0.5F;
            this.btnInternal.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnInternal.IsCLick = true;
            this.btnInternal.IsNotChange = false;
            this.btnInternal.IsRect = false;
            this.btnInternal.IsUnGroup = false;
            this.btnInternal.Location = new System.Drawing.Point(3, 0);
            this.btnInternal.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnInternal.Multiline = false;
            this.btnInternal.Name = "btnInternal";
            this.btnInternal.Size = new System.Drawing.Size(232, 60);
            this.btnInternal.TabIndex = 2;
            this.btnInternal.Text = "Internal";
            this.btnInternal.TextColor = System.Drawing.Color.Black;
            this.btnInternal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInternal.UseVisualStyleBackColor = false;
            this.btnInternal.Click += new System.EventHandler(this.btnInternal_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(494, 748);
            this.tabControl1.TabIndex = 60;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel10);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(486, 710);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Input";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.tableLayout1);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(486, 710);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Camera";
            // 
            // tmShowPara
            // 
            this.tmShowPara.Interval = 200;
            this.tmShowPara.Tick += new System.EventHandler(this.tmShowPara_Tick);
            // 
            // SettingStep1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tableLayoutPanel11);
            this.DoubleBuffered = true;
            this.Name = "SettingStep1";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(500, 822);
            this.Load += new System.EventHandler(this.SettingStep1_Load);
            this.tableLayout1.ResumeLayout(false);
            this.tableLayout1.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.layoutDelay.ResumeLayout(false);
            this.layoutDelay.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker workRead;
        private DbTableLayoutPanel tableLayout1;
        private DbTableLayoutPanel tableLayoutPanel6;
        private RJButton btnExternal;
        private RJButton btnInternal;
        private DbTableLayoutPanel tableLayoutPanel10;
        private DbTableLayoutPanel tableLayoutPanel11;
        private RJButton btnCancel;
        private RJButton btnNextStep;
        private AdjustBar trackExposure;
        private AdjustBar trackGain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private AdjustBar trackShift;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private AdjustBar AdDelayTrig;
        private System.Windows.Forms.Label label6;
        private DbTableLayoutPanel tableLayoutPanel1;
        private RJButton btnAllTime;
        private RJButton btnBlink;
        private DbTableLayoutPanel layoutDelay;
        private System.Windows.Forms.Label label7;
        private AdjustBar AdDelayOutput;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private DbTableLayoutPanel tableLayoutPanel3;
        private RJButton btnLight3;
        private RJButton btnLight1;
        private RJButton btnLight2;
        private DbTableLayoutPanel tableLayoutPanel4;
        private RJButton btnOn;
        private RJButton btnOFF;
        private AdjustBar AdjHeight;
        private System.Windows.Forms.Label label11;
        private AdjustBar AdjWidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private AdjustBar AdjOffSetY;
        private System.Windows.Forms.Label label13;
        private AdjustBar AdjOffsetX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private RJButton btnDownLoadPara;
        private DbTableLayoutPanel tableLayoutPanel5;
        private RJButton btnCenterY;
        private RJButton btnCenterX;
        private System.Windows.Forms.Timer tmShowPara;
    }
}
