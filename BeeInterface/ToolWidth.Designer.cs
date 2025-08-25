using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolWidth
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolWidth));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLong = new BeeInterface.RJButton();
            this.btnShort = new BeeInterface.RJButton();
            this.btnAverage = new BeeInterface.RJButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFar = new BeeInterface.RJButton();
            this.btnNear = new BeeInterface.RJButton();
            this.btnMid = new BeeInterface.RJButton();
            this.btnOutter = new BeeInterface.RJButton();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHori = new BeeInterface.RJButton();
            this.btnVer = new BeeInterface.RJButton();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBinary = new BeeInterface.RJButton();
            this.btnStrongEdge = new BeeInterface.RJButton();
            this.btnCloseEdge = new BeeInterface.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCalib = new BeeInterface.RJButton();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton2 = new BeeInterface.RJButton();
            this.numScale = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numMinRadius = new BeeInterface.CustomNumericEx();
            this.numMaxRadius = new BeeInterface.CustomNumericEx();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackMaxLine = new BeeInterface.AdjustBarEx();
            this.trackMinInlier = new BeeInterface.AdjustBarEx();
            this.pCany = new System.Windows.Forms.Panel();
            this.btnAreaBlack = new BeeInterface.RJButton();
            this.btnAreaWhite = new BeeInterface.RJButton();
            this.workLoadModel = new System.ComponentModel.BackgroundWorker();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pCany.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabP1);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(400, 871);
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
            this.tabP1.Size = new System.Drawing.Size(392, 829);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel15, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 823);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel6.Controls.Add(this.btnLong, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnShort, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnAverage, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 302);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(378, 54);
            this.tableLayoutPanel6.TabIndex = 66;
            // 
            // btnLong
            // 
            this.btnLong.AutoFont = true;
            this.btnLong.AutoFontHeightRatio = 0.75F;
            this.btnLong.AutoFontMax = 100F;
            this.btnLong.AutoFontMin = 6F;
            this.btnLong.AutoFontWidthRatio = 0.92F;
            this.btnLong.AutoImage = true;
            this.btnLong.AutoImageMaxRatio = 0.75F;
            this.btnLong.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLong.AutoImageTint = true;
            this.btnLong.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLong.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLong.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLong.BackgroundImage")));
            this.btnLong.BorderColor = System.Drawing.Color.Silver;
            this.btnLong.BorderRadius = 10;
            this.btnLong.BorderSize = 1;
            this.btnLong.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLong.Corner = BeeGlobal.Corner.Right;
            this.btnLong.DebounceResizeMs = 16;
            this.btnLong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLong.FlatAppearance.BorderSize = 0;
            this.btnLong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLong.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnLong.ForeColor = System.Drawing.Color.Black;
            this.btnLong.Image = null;
            this.btnLong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLong.ImageDisabled = null;
            this.btnLong.ImageHover = null;
            this.btnLong.ImageNormal = null;
            this.btnLong.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLong.ImagePressed = null;
            this.btnLong.ImageTextSpacing = 6;
            this.btnLong.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLong.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLong.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLong.ImageTintOpacity = 0.5F;
            this.btnLong.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLong.IsCLick = false;
            this.btnLong.IsNotChange = false;
            this.btnLong.IsRect = false;
            this.btnLong.IsUnGroup = false;
            this.btnLong.Location = new System.Drawing.Point(255, 0);
            this.btnLong.Margin = new System.Windows.Forms.Padding(0);
            this.btnLong.Multiline = false;
            this.btnLong.Name = "btnLong";
            this.btnLong.Size = new System.Drawing.Size(123, 54);
            this.btnLong.TabIndex = 4;
            this.btnLong.Text = "Longest";
            this.btnLong.TextColor = System.Drawing.Color.Black;
            this.btnLong.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLong.UseVisualStyleBackColor = false;
            this.btnLong.Click += new System.EventHandler(this.btnLong_Click);
            // 
            // btnShort
            // 
            this.btnShort.AutoFont = true;
            this.btnShort.AutoFontHeightRatio = 0.75F;
            this.btnShort.AutoFontMax = 100F;
            this.btnShort.AutoFontMin = 6F;
            this.btnShort.AutoFontWidthRatio = 0.92F;
            this.btnShort.AutoImage = true;
            this.btnShort.AutoImageMaxRatio = 0.75F;
            this.btnShort.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnShort.AutoImageTint = true;
            this.btnShort.BackColor = System.Drawing.Color.Transparent;
            this.btnShort.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnShort.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnShort.BackgroundImage")));
            this.btnShort.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShort.BorderColor = System.Drawing.Color.Transparent;
            this.btnShort.BorderRadius = 10;
            this.btnShort.BorderSize = 1;
            this.btnShort.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnShort.Corner = BeeGlobal.Corner.Left;
            this.btnShort.DebounceResizeMs = 16;
            this.btnShort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShort.FlatAppearance.BorderSize = 0;
            this.btnShort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShort.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnShort.ForeColor = System.Drawing.Color.Black;
            this.btnShort.Image = null;
            this.btnShort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShort.ImageDisabled = null;
            this.btnShort.ImageHover = null;
            this.btnShort.ImageNormal = null;
            this.btnShort.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnShort.ImagePressed = null;
            this.btnShort.ImageTextSpacing = 6;
            this.btnShort.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnShort.ImageTintHover = System.Drawing.Color.Empty;
            this.btnShort.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnShort.ImageTintOpacity = 0.5F;
            this.btnShort.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnShort.IsCLick = false;
            this.btnShort.IsNotChange = false;
            this.btnShort.IsRect = false;
            this.btnShort.IsUnGroup = false;
            this.btnShort.Location = new System.Drawing.Point(0, 0);
            this.btnShort.Margin = new System.Windows.Forms.Padding(0);
            this.btnShort.Multiline = false;
            this.btnShort.Name = "btnShort";
            this.btnShort.Size = new System.Drawing.Size(122, 54);
            this.btnShort.TabIndex = 2;
            this.btnShort.Text = "Shortest";
            this.btnShort.TextColor = System.Drawing.Color.Black;
            this.btnShort.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShort.UseVisualStyleBackColor = false;
            this.btnShort.Click += new System.EventHandler(this.btnShort_Click);
            // 
            // btnAverage
            // 
            this.btnAverage.AutoFont = true;
            this.btnAverage.AutoFontHeightRatio = 0.75F;
            this.btnAverage.AutoFontMax = 100F;
            this.btnAverage.AutoFontMin = 6F;
            this.btnAverage.AutoFontWidthRatio = 0.92F;
            this.btnAverage.AutoImage = true;
            this.btnAverage.AutoImageMaxRatio = 0.75F;
            this.btnAverage.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAverage.AutoImageTint = true;
            this.btnAverage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAverage.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnAverage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAverage.BackgroundImage")));
            this.btnAverage.BorderColor = System.Drawing.Color.Silver;
            this.btnAverage.BorderRadius = 5;
            this.btnAverage.BorderSize = 1;
            this.btnAverage.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAverage.Corner = BeeGlobal.Corner.None;
            this.btnAverage.DebounceResizeMs = 16;
            this.btnAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAverage.FlatAppearance.BorderSize = 0;
            this.btnAverage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAverage.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.48438F);
            this.btnAverage.ForeColor = System.Drawing.Color.Black;
            this.btnAverage.Image = null;
            this.btnAverage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAverage.ImageDisabled = null;
            this.btnAverage.ImageHover = null;
            this.btnAverage.ImageNormal = null;
            this.btnAverage.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAverage.ImagePressed = null;
            this.btnAverage.ImageTextSpacing = 6;
            this.btnAverage.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAverage.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAverage.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAverage.ImageTintOpacity = 0.5F;
            this.btnAverage.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAverage.IsCLick = true;
            this.btnAverage.IsNotChange = false;
            this.btnAverage.IsRect = false;
            this.btnAverage.IsUnGroup = false;
            this.btnAverage.Location = new System.Drawing.Point(122, 0);
            this.btnAverage.Margin = new System.Windows.Forms.Padding(0);
            this.btnAverage.Multiline = false;
            this.btnAverage.Name = "btnAverage";
            this.btnAverage.Size = new System.Drawing.Size(133, 54);
            this.btnAverage.TabIndex = 3;
            this.btnAverage.Text = "Average";
            this.btnAverage.TextColor = System.Drawing.Color.Black;
            this.btnAverage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAverage.UseVisualStyleBackColor = false;
            this.btnAverage.Click += new System.EventHandler(this.btnAverage_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Controls.Add(this.btnFar, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnNear, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnMid, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnOutter, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 213);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(378, 54);
            this.tableLayoutPanel5.TabIndex = 65;
            // 
            // btnFar
            // 
            this.btnFar.AutoFont = true;
            this.btnFar.AutoFontHeightRatio = 0.75F;
            this.btnFar.AutoFontMax = 100F;
            this.btnFar.AutoFontMin = 6F;
            this.btnFar.AutoFontWidthRatio = 0.92F;
            this.btnFar.AutoImage = true;
            this.btnFar.AutoImageMaxRatio = 0.75F;
            this.btnFar.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnFar.AutoImageTint = true;
            this.btnFar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFar.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnFar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFar.BackgroundImage")));
            this.btnFar.BorderColor = System.Drawing.Color.Silver;
            this.btnFar.BorderRadius = 10;
            this.btnFar.BorderSize = 1;
            this.btnFar.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnFar.Corner = BeeGlobal.Corner.Right;
            this.btnFar.DebounceResizeMs = 16;
            this.btnFar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFar.FlatAppearance.BorderSize = 0;
            this.btnFar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFar.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnFar.ForeColor = System.Drawing.Color.Black;
            this.btnFar.Image = null;
            this.btnFar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFar.ImageDisabled = null;
            this.btnFar.ImageHover = null;
            this.btnFar.ImageNormal = null;
            this.btnFar.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnFar.ImagePressed = null;
            this.btnFar.ImageTextSpacing = 6;
            this.btnFar.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnFar.ImageTintHover = System.Drawing.Color.Empty;
            this.btnFar.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnFar.ImageTintOpacity = 0.5F;
            this.btnFar.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnFar.IsCLick = false;
            this.btnFar.IsNotChange = false;
            this.btnFar.IsRect = false;
            this.btnFar.IsUnGroup = false;
            this.btnFar.Location = new System.Drawing.Point(282, 0);
            this.btnFar.Margin = new System.Windows.Forms.Padding(0);
            this.btnFar.Multiline = false;
            this.btnFar.Name = "btnFar";
            this.btnFar.Size = new System.Drawing.Size(96, 54);
            this.btnFar.TabIndex = 5;
            this.btnFar.Text = "Far";
            this.btnFar.TextColor = System.Drawing.Color.Black;
            this.btnFar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFar.UseVisualStyleBackColor = false;
            this.btnFar.Click += new System.EventHandler(this.btnFar_Click);
            // 
            // btnNear
            // 
            this.btnNear.AutoFont = true;
            this.btnNear.AutoFontHeightRatio = 0.75F;
            this.btnNear.AutoFontMax = 100F;
            this.btnNear.AutoFontMin = 6F;
            this.btnNear.AutoFontWidthRatio = 0.92F;
            this.btnNear.AutoImage = true;
            this.btnNear.AutoImageMaxRatio = 0.75F;
            this.btnNear.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnNear.AutoImageTint = true;
            this.btnNear.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNear.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnNear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNear.BackgroundImage")));
            this.btnNear.BorderColor = System.Drawing.Color.Silver;
            this.btnNear.BorderRadius = 10;
            this.btnNear.BorderSize = 1;
            this.btnNear.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnNear.Corner = BeeGlobal.Corner.None;
            this.btnNear.DebounceResizeMs = 16;
            this.btnNear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNear.FlatAppearance.BorderSize = 0;
            this.btnNear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNear.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.21875F);
            this.btnNear.ForeColor = System.Drawing.Color.Black;
            this.btnNear.Image = null;
            this.btnNear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNear.ImageDisabled = null;
            this.btnNear.ImageHover = null;
            this.btnNear.ImageNormal = null;
            this.btnNear.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnNear.ImagePressed = null;
            this.btnNear.ImageTextSpacing = 6;
            this.btnNear.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnNear.ImageTintHover = System.Drawing.Color.Empty;
            this.btnNear.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnNear.ImageTintOpacity = 0.5F;
            this.btnNear.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnNear.IsCLick = false;
            this.btnNear.IsNotChange = false;
            this.btnNear.IsRect = false;
            this.btnNear.IsUnGroup = false;
            this.btnNear.Location = new System.Drawing.Point(188, 0);
            this.btnNear.Margin = new System.Windows.Forms.Padding(0);
            this.btnNear.Multiline = false;
            this.btnNear.Name = "btnNear";
            this.btnNear.Size = new System.Drawing.Size(94, 54);
            this.btnNear.TabIndex = 4;
            this.btnNear.Text = "Near";
            this.btnNear.TextColor = System.Drawing.Color.Black;
            this.btnNear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNear.UseVisualStyleBackColor = false;
            this.btnNear.Click += new System.EventHandler(this.btnNear_Click);
            // 
            // btnMid
            // 
            this.btnMid.AutoFont = true;
            this.btnMid.AutoFontHeightRatio = 0.75F;
            this.btnMid.AutoFontMax = 100F;
            this.btnMid.AutoFontMin = 6F;
            this.btnMid.AutoFontWidthRatio = 0.92F;
            this.btnMid.AutoImage = true;
            this.btnMid.AutoImageMaxRatio = 0.75F;
            this.btnMid.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMid.AutoImageTint = true;
            this.btnMid.BackColor = System.Drawing.SystemColors.Control;
            this.btnMid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnMid.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMid.BackgroundImage")));
            this.btnMid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMid.BorderColor = System.Drawing.Color.Transparent;
            this.btnMid.BorderRadius = 10;
            this.btnMid.BorderSize = 1;
            this.btnMid.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMid.Corner = BeeGlobal.Corner.Left;
            this.btnMid.DebounceResizeMs = 16;
            this.btnMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMid.FlatAppearance.BorderSize = 0;
            this.btnMid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMid.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.8125F);
            this.btnMid.ForeColor = System.Drawing.Color.Black;
            this.btnMid.Image = null;
            this.btnMid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMid.ImageDisabled = null;
            this.btnMid.ImageHover = null;
            this.btnMid.ImageNormal = null;
            this.btnMid.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMid.ImagePressed = null;
            this.btnMid.ImageTextSpacing = 6;
            this.btnMid.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMid.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMid.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMid.ImageTintOpacity = 0.5F;
            this.btnMid.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMid.IsCLick = true;
            this.btnMid.IsNotChange = false;
            this.btnMid.IsRect = false;
            this.btnMid.IsUnGroup = false;
            this.btnMid.Location = new System.Drawing.Point(0, 0);
            this.btnMid.Margin = new System.Windows.Forms.Padding(0);
            this.btnMid.Multiline = false;
            this.btnMid.Name = "btnMid";
            this.btnMid.Size = new System.Drawing.Size(94, 54);
            this.btnMid.TabIndex = 2;
            this.btnMid.Text = "Middle";
            this.btnMid.TextColor = System.Drawing.Color.Black;
            this.btnMid.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMid.UseVisualStyleBackColor = false;
            this.btnMid.Click += new System.EventHandler(this.btnMid_Click);
            // 
            // btnOutter
            // 
            this.btnOutter.AutoFont = true;
            this.btnOutter.AutoFontHeightRatio = 0.75F;
            this.btnOutter.AutoFontMax = 100F;
            this.btnOutter.AutoFontMin = 6F;
            this.btnOutter.AutoFontWidthRatio = 0.92F;
            this.btnOutter.AutoImage = true;
            this.btnOutter.AutoImageMaxRatio = 0.75F;
            this.btnOutter.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnOutter.AutoImageTint = true;
            this.btnOutter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnOutter.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnOutter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOutter.BackgroundImage")));
            this.btnOutter.BorderColor = System.Drawing.Color.Silver;
            this.btnOutter.BorderRadius = 5;
            this.btnOutter.BorderSize = 1;
            this.btnOutter.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOutter.Corner = BeeGlobal.Corner.None;
            this.btnOutter.DebounceResizeMs = 16;
            this.btnOutter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOutter.FlatAppearance.BorderSize = 0;
            this.btnOutter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutter.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnOutter.ForeColor = System.Drawing.Color.Black;
            this.btnOutter.Image = null;
            this.btnOutter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOutter.ImageDisabled = null;
            this.btnOutter.ImageHover = null;
            this.btnOutter.ImageNormal = null;
            this.btnOutter.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnOutter.ImagePressed = null;
            this.btnOutter.ImageTextSpacing = 6;
            this.btnOutter.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnOutter.ImageTintHover = System.Drawing.Color.Empty;
            this.btnOutter.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnOutter.ImageTintOpacity = 0.5F;
            this.btnOutter.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnOutter.IsCLick = false;
            this.btnOutter.IsNotChange = false;
            this.btnOutter.IsRect = false;
            this.btnOutter.IsUnGroup = false;
            this.btnOutter.Location = new System.Drawing.Point(94, 0);
            this.btnOutter.Margin = new System.Windows.Forms.Padding(0);
            this.btnOutter.Multiline = false;
            this.btnOutter.Name = "btnOutter";
            this.btnOutter.Size = new System.Drawing.Size(94, 54);
            this.btnOutter.TabIndex = 3;
            this.btnOutter.Text = "Outer";
            this.btnOutter.TextColor = System.Drawing.Color.Black;
            this.btnOutter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOutter.UseVisualStyleBackColor = false;
            this.btnOutter.Click += new System.EventHandler(this.btnOutter_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 183);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(154, 25);
            this.label10.TabIndex = 64;
            this.label10.Text = "Gap Extremum";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnHori, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnVer, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 124);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(378, 54);
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
            this.btnHori.BackColor = System.Drawing.Color.Transparent;
            this.btnHori.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnHori.BorderColor = System.Drawing.Color.Silver;
            this.btnHori.BorderRadius = 10;
            this.btnHori.BorderSize = 1;
            this.btnHori.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnHori.Corner = BeeGlobal.Corner.Right;
            this.btnHori.DebounceResizeMs = 16;
            this.btnHori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHori.FlatAppearance.BorderSize = 0;
            this.btnHori.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHori.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
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
            this.btnHori.IsUnGroup = false;
            this.btnHori.Location = new System.Drawing.Point(189, 0);
            this.btnHori.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnHori.Multiline = false;
            this.btnHori.Name = "btnHori";
            this.btnHori.Size = new System.Drawing.Size(186, 54);
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
            this.btnVer.BackColor = System.Drawing.SystemColors.Control;
            this.btnVer.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnVer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVer.BorderColor = System.Drawing.Color.Transparent;
            this.btnVer.BorderRadius = 10;
            this.btnVer.BorderSize = 1;
            this.btnVer.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnVer.Corner = BeeGlobal.Corner.Left;
            this.btnVer.DebounceResizeMs = 16;
            this.btnVer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVer.FlatAppearance.BorderSize = 0;
            this.btnVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
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
            this.btnVer.IsUnGroup = false;
            this.btnVer.Location = new System.Drawing.Point(3, 0);
            this.btnVer.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnVer.Multiline = false;
            this.btnVer.Name = "btnVer";
            this.btnVer.Size = new System.Drawing.Size(186, 54);
            this.btnVer.TabIndex = 2;
            this.btnVer.Text = "Vertical";
            this.btnVer.TextColor = System.Drawing.Color.Black;
            this.btnVer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVer.UseVisualStyleBackColor = false;
            this.btnVer.Click += new System.EventHandler(this.btnVer_Click);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel15.ColumnCount = 3;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.Controls.Add(this.btnBinary, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnStrongEdge, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnCloseEdge, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(5, 391);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(378, 54);
            this.tableLayoutPanel15.TabIndex = 62;
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
            this.btnBinary.BackColor = System.Drawing.Color.Transparent;
            this.btnBinary.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnBinary.BorderColor = System.Drawing.Color.Silver;
            this.btnBinary.BorderRadius = 10;
            this.btnBinary.BorderSize = 1;
            this.btnBinary.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnBinary.Corner = BeeGlobal.Corner.Right;
            this.btnBinary.DebounceResizeMs = 16;
            this.btnBinary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBinary.FlatAppearance.BorderSize = 0;
            this.btnBinary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBinary.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
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
            this.btnBinary.IsUnGroup = false;
            this.btnBinary.Location = new System.Drawing.Point(252, 0);
            this.btnBinary.Margin = new System.Windows.Forms.Padding(0);
            this.btnBinary.Multiline = false;
            this.btnBinary.Name = "btnBinary";
            this.btnBinary.Size = new System.Drawing.Size(126, 54);
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
            this.btnStrongEdge.BackColor = System.Drawing.SystemColors.Control;
            this.btnStrongEdge.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnStrongEdge.BorderColor = System.Drawing.Color.Silver;
            this.btnStrongEdge.BorderRadius = 10;
            this.btnStrongEdge.BorderSize = 1;
            this.btnStrongEdge.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnStrongEdge.Corner = BeeGlobal.Corner.Right;
            this.btnStrongEdge.DebounceResizeMs = 16;
            this.btnStrongEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStrongEdge.FlatAppearance.BorderSize = 0;
            this.btnStrongEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStrongEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
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
            this.btnStrongEdge.IsUnGroup = false;
            this.btnStrongEdge.Location = new System.Drawing.Point(126, 0);
            this.btnStrongEdge.Margin = new System.Windows.Forms.Padding(0);
            this.btnStrongEdge.Multiline = false;
            this.btnStrongEdge.Name = "btnStrongEdge";
            this.btnStrongEdge.Size = new System.Drawing.Size(126, 54);
            this.btnStrongEdge.TabIndex = 3;
            this.btnStrongEdge.Text = "Strong Edge";
            this.btnStrongEdge.TextColor = System.Drawing.Color.Black;
            this.btnStrongEdge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStrongEdge.UseVisualStyleBackColor = false;
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
            this.btnCloseEdge.BackColor = System.Drawing.SystemColors.Control;
            this.btnCloseEdge.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCloseEdge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseEdge.BorderColor = System.Drawing.Color.Transparent;
            this.btnCloseEdge.BorderRadius = 10;
            this.btnCloseEdge.BorderSize = 1;
            this.btnCloseEdge.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCloseEdge.Corner = BeeGlobal.Corner.Left;
            this.btnCloseEdge.DebounceResizeMs = 16;
            this.btnCloseEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseEdge.FlatAppearance.BorderSize = 0;
            this.btnCloseEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
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
            this.btnCloseEdge.IsUnGroup = false;
            this.btnCloseEdge.Location = new System.Drawing.Point(3, 0);
            this.btnCloseEdge.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCloseEdge.Multiline = false;
            this.btnCloseEdge.Name = "btnCloseEdge";
            this.btnCloseEdge.Size = new System.Drawing.Size(123, 54);
            this.btnCloseEdge.TabIndex = 2;
            this.btnCloseEdge.Text = "Close Edge";
            this.btnCloseEdge.TextColor = System.Drawing.Color.Black;
            this.btnCloseEdge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseEdge.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 361);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 25);
            this.label2.TabIndex = 61;
            this.label2.Text = "Methord Get Edge";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 272);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 25);
            this.label3.TabIndex = 59;
            this.label3.Text = "Segment";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(3, 94);
            this.label13.Margin = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(97, 25);
            this.label13.TabIndex = 58;
            this.label13.Text = "Direction";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 450);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(378, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTest
            // 
            this.btnTest.AutoFont = true;
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
            this.btnTest.BorderColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderRadius = 1;
            this.btnTest.BorderSize = 1;
            this.btnTest.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.DebounceResizeMs = 16;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 59.97656F, System.Drawing.FontStyle.Bold);
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
            this.btnTest.Location = new System.Drawing.Point(20, 564);
            this.btnTest.Margin = new System.Windows.Forms.Padding(20);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(346, 239);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 25);
            this.label7.TabIndex = 38;
            this.label7.Text = "Search Range";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 35);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 54);
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
            this.btnCropFull.BackColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BorderColor = System.Drawing.Color.Silver;
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.DebounceResizeMs = 16;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
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
            this.btnCropFull.Location = new System.Drawing.Point(189, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Multiline = false;
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(186, 54);
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
            this.btnCropHalt.BackColor = System.Drawing.SystemColors.Control;
            this.btnCropHalt.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.DebounceResizeMs = 16;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
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
            this.btnCropHalt.Location = new System.Drawing.Point(3, 0);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCropHalt.Multiline = false;
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(186, 54);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // trackScore
            // 
            this.trackScore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackScore.AutoShowTextbox = false;
            this.trackScore.AutoSizeTextbox = true;
            this.trackScore.BackColor = System.Drawing.Color.WhiteSmoke;
            this.trackScore.BarLeftGap = 10;
            this.trackScore.BarRightGap = 6;
            this.trackScore.ChromeGap = 10;
            this.trackScore.ChromeWidthRatio = 0.14F;
            this.trackScore.ColorBorder = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.trackScore.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackScore.ColorScale = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.trackScore.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.trackScore.Decimals = 0;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(3, 481);
            this.trackScore.MatchTextboxFontToThumb = false;
            this.trackScore.Max = 100F;
            this.trackScore.MaxTextboxWidth = 0;
            this.trackScore.MaxThumb = 1000;
            this.trackScore.MaxTrackHeight = 1000;
            this.trackScore.Min = 0F;
            this.trackScore.MinChromeWidth = 64;
            this.trackScore.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackScore.MinTextboxWidth = 16;
            this.trackScore.MinThumb = 24;
            this.trackScore.MinTrackHeight = 8;
            this.trackScore.Name = "trackScore";
            this.trackScore.Radius = 8;
            this.trackScore.ShowValueOnThumb = true;
            this.trackScore.Size = new System.Drawing.Size(380, 60);
            this.trackScore.SnapToStep = true;
            this.trackScore.StartWithTextboxHidden = true;
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 67;
            this.trackScore.TextboxFontSize = 22F;
            this.trackScore.TextboxSidePadding = 12;
            this.trackScore.TextboxWidth = 56;
            this.trackScore.ThumbDiameterRatio = 1.15F;
            this.trackScore.ThumbValueBold = true;
            this.trackScore.ThumbValueFontScale = 1F;
            this.trackScore.ThumbValuePadding = -1;
            this.trackScore.TightEdges = true;
            this.trackScore.TrackHeightRatio = 0.4F;
            this.trackScore.TrackWidthRatio = 1F;
            this.trackScore.UnitText = "";
            this.trackScore.Value = 0F;
            this.trackScore.WheelStep = 1F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 829);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.btnCalib, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel4, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.trackMaxLine, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.trackMinInlier, 0, 3);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 9;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(386, 823);
            this.tableLayoutPanel8.TabIndex = 1;
            this.tableLayoutPanel8.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel8_Paint);
            // 
            // btnCalib
            // 
            this.btnCalib.AutoFont = true;
            this.btnCalib.AutoFontHeightRatio = 0.75F;
            this.btnCalib.AutoFontMax = 100F;
            this.btnCalib.AutoFontMin = 6F;
            this.btnCalib.AutoFontWidthRatio = 0.92F;
            this.btnCalib.AutoImage = true;
            this.btnCalib.AutoImageMaxRatio = 0.75F;
            this.btnCalib.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCalib.AutoImageTint = true;
            this.btnCalib.BackColor = System.Drawing.Color.Transparent;
            this.btnCalib.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCalib.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalib.BorderColor = System.Drawing.Color.Transparent;
            this.btnCalib.BorderRadius = 10;
            this.btnCalib.BorderSize = 1;
            this.btnCalib.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCalib.Corner = BeeGlobal.Corner.Left;
            this.btnCalib.DebounceResizeMs = 16;
            this.btnCalib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalib.FlatAppearance.BorderSize = 0;
            this.btnCalib.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 29.13281F);
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
            this.btnCalib.IsUnGroup = false;
            this.btnCalib.Location = new System.Drawing.Point(3, 350);
            this.btnCalib.Margin = new System.Windows.Forms.Padding(3, 20, 0, 0);
            this.btnCalib.Multiline = false;
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(383, 73);
            this.btnCalib.TabIndex = 62;
            this.btnCalib.Text = "Auto Calib";
            this.btnCalib.TextColor = System.Drawing.Color.Black;
            this.btnCalib.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCalib.UseVisualStyleBackColor = false;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel14.ColumnCount = 2;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.Controls.Add(this.rjButton2, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.numScale, 1, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 275);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 15, 3, 0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(378, 55);
            this.tableLayoutPanel14.TabIndex = 61;
            // 
            // rjButton2
            // 
            this.rjButton2.AutoFont = true;
            this.rjButton2.AutoFontHeightRatio = 0.75F;
            this.rjButton2.AutoFontMax = 100F;
            this.rjButton2.AutoFontMin = 6F;
            this.rjButton2.AutoFontWidthRatio = 0.92F;
            this.rjButton2.AutoImage = true;
            this.rjButton2.AutoImageMaxRatio = 0.75F;
            this.rjButton2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton2.AutoImageTint = true;
            this.rjButton2.BackColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 10;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton2.Corner = BeeGlobal.Corner.Left;
            this.rjButton2.DebounceResizeMs = 16;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.Enabled = false;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.6875F);
            this.rjButton2.ForeColor = System.Drawing.Color.White;
            this.rjButton2.Image = null;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.ImageDisabled = null;
            this.rjButton2.ImageHover = null;
            this.rjButton2.ImageNormal = null;
            this.rjButton2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton2.ImagePressed = null;
            this.rjButton2.ImageTextSpacing = 6;
            this.rjButton2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton2.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton2.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton2.ImageTintOpacity = 0.5F;
            this.rjButton2.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = false;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(3, 0);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton2.Multiline = false;
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(186, 55);
            this.rjButton2.TabIndex = 2;
            this.rjButton2.Text = "Scale";
            this.rjButton2.TextColor = System.Drawing.Color.White;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // numScale
            // 
            this.numScale.DecimalPlaces = 3;
            this.numScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numScale.Location = new System.Drawing.Point(192, 3);
            this.numScale.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numScale.Name = "numScale";
            this.numScale.Size = new System.Drawing.Size(183, 47);
            this.numScale.TabIndex = 47;
            this.numScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numScale.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.label9, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 159);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 5);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Size = new System.Drawing.Size(378, 46);
            this.tableLayoutPanel13.TabIndex = 56;
            // 
            // label9
            // 
            this.label9.AutoEllipsis = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label9.Location = new System.Drawing.Point(194, 10);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(181, 36);
            this.label9.TabIndex = 52;
            this.label9.Text = "Max Len";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label5.Location = new System.Drawing.Point(5, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 36);
            this.label5.TabIndex = 51;
            this.label5.Text = "Min Len";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.numMinRadius, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.numMaxRadius, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 215);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(378, 45);
            this.tableLayoutPanel4.TabIndex = 55;
            // 
            // numMinRadius
            // 
            this.numMinRadius.AutoShowTextbox = false;
            this.numMinRadius.AutoSizeTextbox = true;
            this.numMinRadius.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numMinRadius.BackColor = System.Drawing.Color.Transparent;
            this.numMinRadius.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numMinRadius.BorderRadius = 6;
            this.numMinRadius.ButtonMaxSize = 64;
            this.numMinRadius.ButtonMinSize = 24;
            this.numMinRadius.Decimals = 0;
            this.numMinRadius.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numMinRadius.ElementGap = 6;
            this.numMinRadius.FillTextboxToAvailable = true;
            this.numMinRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMinRadius.ForeColor = System.Drawing.Color.Red;
            this.numMinRadius.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numMinRadius.KeyboardStep = 1F;
            this.numMinRadius.Location = new System.Drawing.Point(0, 0);
            this.numMinRadius.Margin = new System.Windows.Forms.Padding(0);
            this.numMinRadius.Max = 1000F;
            this.numMinRadius.MaxTextboxWidth = 0;
            this.numMinRadius.Min = 0F;
            this.numMinRadius.MinimumSize = new System.Drawing.Size(120, 32);
            this.numMinRadius.MinTextboxWidth = 16;
            this.numMinRadius.Name = "numMinRadius";
            this.numMinRadius.Size = new System.Drawing.Size(189, 45);
            this.numMinRadius.SnapToStep = true;
            this.numMinRadius.StartWithTextboxHidden = false;
            this.numMinRadius.Step = 1F;
            this.numMinRadius.TabIndex = 38;
            this.numMinRadius.TextboxFontSize = 11F;
            this.numMinRadius.TextboxSidePadding = 12;
            this.numMinRadius.TextboxWidth = 56;
            this.numMinRadius.UnitText = "";
            this.numMinRadius.Value = 0F;
            this.numMinRadius.WheelStep = 1F;
            // 
            // numMaxRadius
            // 
            this.numMaxRadius.AutoShowTextbox = false;
            this.numMaxRadius.AutoSizeTextbox = true;
            this.numMaxRadius.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numMaxRadius.BackColor = System.Drawing.Color.Transparent;
            this.numMaxRadius.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numMaxRadius.BorderRadius = 6;
            this.numMaxRadius.ButtonMaxSize = 64;
            this.numMaxRadius.ButtonMinSize = 24;
            this.numMaxRadius.Decimals = 0;
            this.numMaxRadius.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numMaxRadius.ElementGap = 6;
            this.numMaxRadius.FillTextboxToAvailable = true;
            this.numMaxRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMaxRadius.ForeColor = System.Drawing.Color.Red;
            this.numMaxRadius.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numMaxRadius.KeyboardStep = 1F;
            this.numMaxRadius.Location = new System.Drawing.Point(189, 0);
            this.numMaxRadius.Margin = new System.Windows.Forms.Padding(0);
            this.numMaxRadius.Max = 10000F;
            this.numMaxRadius.MaxTextboxWidth = 0;
            this.numMaxRadius.Min = 0F;
            this.numMaxRadius.MinimumSize = new System.Drawing.Size(120, 32);
            this.numMaxRadius.MinTextboxWidth = 16;
            this.numMaxRadius.Name = "numMaxRadius";
            this.numMaxRadius.Size = new System.Drawing.Size(189, 45);
            this.numMaxRadius.SnapToStep = true;
            this.numMaxRadius.StartWithTextboxHidden = false;
            this.numMaxRadius.Step = 1F;
            this.numMaxRadius.TabIndex = 35;
            this.numMaxRadius.TextboxFontSize = 11F;
            this.numMaxRadius.TextboxSidePadding = 12;
            this.numMaxRadius.TextboxWidth = 56;
            this.numMaxRadius.UnitText = "";
            this.numMaxRadius.Value = 10000F;
            this.numMaxRadius.WheelStep = 1F;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 87);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 25);
            this.label6.TabIndex = 48;
            this.label6.Text = "Min Inliers";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(378, 25);
            this.label4.TabIndex = 39;
            this.label4.Text = "Maximum Lines";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackMaxLine
            // 
            this.trackMaxLine.AutoShowTextbox = false;
            this.trackMaxLine.AutoSizeTextbox = true;
            this.trackMaxLine.BackColor = System.Drawing.Color.WhiteSmoke;
            this.trackMaxLine.BarLeftGap = 10;
            this.trackMaxLine.BarRightGap = 6;
            this.trackMaxLine.ChromeGap = 10;
            this.trackMaxLine.ChromeWidthRatio = 0.14F;
            this.trackMaxLine.ColorBorder = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.trackMaxLine.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackMaxLine.ColorScale = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.trackMaxLine.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackMaxLine.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackMaxLine.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.trackMaxLine.Decimals = 0;
            this.trackMaxLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackMaxLine.EdgePadding = 2;
            this.trackMaxLine.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackMaxLine.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackMaxLine.KeyboardStep = 1F;
            this.trackMaxLine.Location = new System.Drawing.Point(3, 38);
            this.trackMaxLine.MatchTextboxFontToThumb = false;
            this.trackMaxLine.Max = 20F;
            this.trackMaxLine.MaxTextboxWidth = 0;
            this.trackMaxLine.MaxThumb = 1000;
            this.trackMaxLine.MaxTrackHeight = 1000;
            this.trackMaxLine.Min = 1F;
            this.trackMaxLine.MinChromeWidth = 64;
            this.trackMaxLine.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackMaxLine.MinTextboxWidth = 16;
            this.trackMaxLine.MinThumb = 24;
            this.trackMaxLine.MinTrackHeight = 8;
            this.trackMaxLine.Name = "trackMaxLine";
            this.trackMaxLine.Radius = 8;
            this.trackMaxLine.ShowValueOnThumb = true;
            this.trackMaxLine.Size = new System.Drawing.Size(380, 36);
            this.trackMaxLine.SnapToStep = true;
            this.trackMaxLine.StartWithTextboxHidden = true;
            this.trackMaxLine.Step = 1F;
            this.trackMaxLine.TabIndex = 63;
            this.trackMaxLine.TextboxFontSize = 20F;
            this.trackMaxLine.TextboxSidePadding = 12;
            this.trackMaxLine.TextboxWidth = 56;
            this.trackMaxLine.ThumbDiameterRatio = 1.15F;
            this.trackMaxLine.ThumbValueBold = true;
            this.trackMaxLine.ThumbValueFontScale = 1F;
            this.trackMaxLine.ThumbValuePadding = -1;
            this.trackMaxLine.TightEdges = true;
            this.trackMaxLine.TrackHeightRatio = 0.4F;
            this.trackMaxLine.TrackWidthRatio = 1F;
            this.trackMaxLine.UnitText = "";
            this.trackMaxLine.Value = 1F;
            this.trackMaxLine.WheelStep = 1F;
            this.trackMaxLine.ValueChanged += new System.Action<float>(this.trackMaxLine_ValueChanged);
            // 
            // trackMinInlier
            // 
            this.trackMinInlier.AutoShowTextbox = false;
            this.trackMinInlier.AutoSizeTextbox = true;
            this.trackMinInlier.BackColor = System.Drawing.Color.WhiteSmoke;
            this.trackMinInlier.BarLeftGap = 10;
            this.trackMinInlier.BarRightGap = 6;
            this.trackMinInlier.ChromeGap = 10;
            this.trackMinInlier.ChromeWidthRatio = 0.14F;
            this.trackMinInlier.ColorBorder = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.trackMinInlier.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackMinInlier.ColorScale = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.trackMinInlier.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackMinInlier.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackMinInlier.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.trackMinInlier.Decimals = 0;
            this.trackMinInlier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackMinInlier.EdgePadding = 2;
            this.trackMinInlier.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackMinInlier.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackMinInlier.KeyboardStep = 1F;
            this.trackMinInlier.Location = new System.Drawing.Point(3, 115);
            this.trackMinInlier.MatchTextboxFontToThumb = false;
            this.trackMinInlier.Max = 10000F;
            this.trackMinInlier.MaxTextboxWidth = 0;
            this.trackMinInlier.MaxThumb = 1000;
            this.trackMinInlier.MaxTrackHeight = 1000;
            this.trackMinInlier.Min = 1F;
            this.trackMinInlier.MinChromeWidth = 64;
            this.trackMinInlier.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackMinInlier.MinTextboxWidth = 16;
            this.trackMinInlier.MinThumb = 24;
            this.trackMinInlier.MinTrackHeight = 8;
            this.trackMinInlier.Name = "trackMinInlier";
            this.trackMinInlier.Radius = 8;
            this.trackMinInlier.ShowValueOnThumb = true;
            this.trackMinInlier.Size = new System.Drawing.Size(380, 36);
            this.trackMinInlier.SnapToStep = true;
            this.trackMinInlier.StartWithTextboxHidden = true;
            this.trackMinInlier.Step = 1F;
            this.trackMinInlier.TabIndex = 64;
            this.trackMinInlier.TextboxFontSize = 20F;
            this.trackMinInlier.TextboxSidePadding = 12;
            this.trackMinInlier.TextboxWidth = 56;
            this.trackMinInlier.ThumbDiameterRatio = 1.15F;
            this.trackMinInlier.ThumbValueBold = true;
            this.trackMinInlier.ThumbValueFontScale = 1F;
            this.trackMinInlier.ThumbValuePadding = -1;
            this.trackMinInlier.TightEdges = true;
            this.trackMinInlier.TrackHeightRatio = 0.4F;
            this.trackMinInlier.TrackWidthRatio = 1F;
            this.trackMinInlier.UnitText = "";
            this.trackMinInlier.Value = 2F;
            this.trackMinInlier.WheelStep = 1F;
            this.trackMinInlier.ValueChanged += new System.Action<float>(this.trackMinInlier_ValueChanged);
            // 
            // pCany
            // 
            this.pCany.Controls.Add(this.btnAreaBlack);
            this.pCany.Controls.Add(this.btnAreaWhite);
            this.pCany.Enabled = false;
            this.pCany.Location = new System.Drawing.Point(529, 529);
            this.pCany.Name = "pCany";
            this.pCany.Size = new System.Drawing.Size(386, 50);
            this.pCany.TabIndex = 12;
            this.pCany.Visible = false;
            // 
            // btnAreaBlack
            // 
            this.btnAreaBlack.AutoFont = true;
            this.btnAreaBlack.AutoFontHeightRatio = 0.75F;
            this.btnAreaBlack.AutoFontMax = 100F;
            this.btnAreaBlack.AutoFontMin = 6F;
            this.btnAreaBlack.AutoFontWidthRatio = 0.92F;
            this.btnAreaBlack.AutoImage = true;
            this.btnAreaBlack.AutoImageMaxRatio = 0.75F;
            this.btnAreaBlack.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAreaBlack.AutoImageTint = true;
            this.btnAreaBlack.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAreaBlack.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAreaBlack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaBlack.BackgroundImage")));
            this.btnAreaBlack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAreaBlack.BorderColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BorderRadius = 5;
            this.btnAreaBlack.BorderSize = 1;
            this.btnAreaBlack.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAreaBlack.Corner = BeeGlobal.Corner.Both;
            this.btnAreaBlack.DebounceResizeMs = 16;
            this.btnAreaBlack.FlatAppearance.BorderSize = 0;
            this.btnAreaBlack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaBlack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnAreaBlack.ForeColor = System.Drawing.Color.Black;
            this.btnAreaBlack.Image = null;
            this.btnAreaBlack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaBlack.ImageDisabled = null;
            this.btnAreaBlack.ImageHover = null;
            this.btnAreaBlack.ImageNormal = null;
            this.btnAreaBlack.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAreaBlack.ImagePressed = null;
            this.btnAreaBlack.ImageTextSpacing = 6;
            this.btnAreaBlack.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAreaBlack.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAreaBlack.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAreaBlack.ImageTintOpacity = 0.5F;
            this.btnAreaBlack.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAreaBlack.IsCLick = true;
            this.btnAreaBlack.IsNotChange = false;
            this.btnAreaBlack.IsRect = false;
            this.btnAreaBlack.IsUnGroup = false;
            this.btnAreaBlack.Location = new System.Drawing.Point(3, 5);
            this.btnAreaBlack.Multiline = false;
            this.btnAreaBlack.Name = "btnAreaBlack";
            this.btnAreaBlack.Size = new System.Drawing.Size(185, 40);
            this.btnAreaBlack.TabIndex = 7;
            this.btnAreaBlack.Text = "Vùng Tối";
            this.btnAreaBlack.TextColor = System.Drawing.Color.Black;
            this.btnAreaBlack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAreaBlack.UseVisualStyleBackColor = false;
            // 
            // btnAreaWhite
            // 
            this.btnAreaWhite.AutoFont = true;
            this.btnAreaWhite.AutoFontHeightRatio = 0.75F;
            this.btnAreaWhite.AutoFontMax = 100F;
            this.btnAreaWhite.AutoFontMin = 6F;
            this.btnAreaWhite.AutoFontWidthRatio = 0.92F;
            this.btnAreaWhite.AutoImage = true;
            this.btnAreaWhite.AutoImageMaxRatio = 0.75F;
            this.btnAreaWhite.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAreaWhite.AutoImageTint = true;
            this.btnAreaWhite.BackColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaWhite.BackgroundImage")));
            this.btnAreaWhite.BorderColor = System.Drawing.Color.Silver;
            this.btnAreaWhite.BorderRadius = 5;
            this.btnAreaWhite.BorderSize = 1;
            this.btnAreaWhite.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAreaWhite.Corner = BeeGlobal.Corner.Both;
            this.btnAreaWhite.DebounceResizeMs = 16;
            this.btnAreaWhite.FlatAppearance.BorderSize = 0;
            this.btnAreaWhite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaWhite.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnAreaWhite.ForeColor = System.Drawing.Color.Black;
            this.btnAreaWhite.Image = null;
            this.btnAreaWhite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaWhite.ImageDisabled = null;
            this.btnAreaWhite.ImageHover = null;
            this.btnAreaWhite.ImageNormal = null;
            this.btnAreaWhite.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAreaWhite.ImagePressed = null;
            this.btnAreaWhite.ImageTextSpacing = 6;
            this.btnAreaWhite.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAreaWhite.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAreaWhite.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAreaWhite.ImageTintOpacity = 0.5F;
            this.btnAreaWhite.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAreaWhite.IsCLick = false;
            this.btnAreaWhite.IsNotChange = false;
            this.btnAreaWhite.IsRect = false;
            this.btnAreaWhite.IsUnGroup = false;
            this.btnAreaWhite.Location = new System.Drawing.Point(195, 5);
            this.btnAreaWhite.Multiline = false;
            this.btnAreaWhite.Name = "btnAreaWhite";
            this.btnAreaWhite.Size = new System.Drawing.Size(188, 40);
            this.btnAreaWhite.TabIndex = 9;
            this.btnAreaWhite.Text = "Vùng sáng";
            this.btnAreaWhite.TextColor = System.Drawing.Color.Black;
            this.btnAreaWhite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAreaWhite.UseVisualStyleBackColor = false;
            // 
            // workLoadModel
            // 
            this.workLoadModel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadModel_DoWork);
            this.workLoadModel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadModel_RunWorkerCompleted);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 877);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(400, 62);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolWidth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.pCany);
            this.Controls.Add(this.tabControl2);
            this.DoubleBuffered = true;
            this.Name = "ToolWidth";
            this.Size = new System.Drawing.Size(400, 939);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pCany.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TabPage tabPage4;
        public System.ComponentModel.BackgroundWorker threadProcess;
        private System.Windows.Forms.Panel pCany;
        private RJButton btnAreaBlack;
        private RJButton btnAreaWhite;
        private System.ComponentModel.BackgroundWorker workLoadModel;
        private GroupControl.OK_Cancel oK_Cancel1;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private RJButton btnStrongEdge;
        private RJButton btnCloseEdge;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private RJButton btnTest;
        private System.Windows.Forms.Label label7;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private RJButton btnCalib;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private RJButton rjButton2;
        private System.Windows.Forms.NumericUpDown numScale;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private CustomNumericEx numMinRadius;
        private CustomNumericEx numMaxRadius;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnHori;
        private RJButton btnVer;
        private System.Windows.Forms.Label label10;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnNear;
        private RJButton btnMid;
        private RJButton btnOutter;
        private RJButton btnFar;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnLong;
        private RJButton btnShort;
        private RJButton btnAverage;
        private RJButton btnBinary;
        private AdjustBarEx trackMaxLine;
        private AdjustBarEx trackMinInlier;
        private AdjustBarEx trackScore;
    }
}
