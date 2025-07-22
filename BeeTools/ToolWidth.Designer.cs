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
            this.layScore = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton4 = new BeeInterface.RJButton();
            this.numScore = new BeeInterface.CustomNumeric();
            this.rjButton5 = new BeeInterface.RJButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.trackScore = new BeeInterface.TrackBar2();
            this.label8 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCalib = new BeeInterface.RJButton();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton2 = new BeeInterface.RJButton();
            this.numScale = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numMinRadius = new BeeInterface.CustomNumeric();
            this.numMaxRadius = new BeeInterface.CustomNumeric();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton8 = new BeeInterface.RJButton();
            this.rjButton9 = new BeeInterface.RJButton();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.trackMinInlier = new BeeInterface.TrackBar2();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton6 = new BeeInterface.RJButton();
            this.rjButton7 = new BeeInterface.RJButton();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.trackMaxLine = new BeeInterface.TrackBar2();
            this.label4 = new System.Windows.Forms.Label();
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
            this.layScore.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
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
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(400, 871);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(392, 833);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            this.tabP1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
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
            this.tableLayoutPanel1.Controls.Add(this.layScore, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 827);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel6.Controls.Add(this.btnLong, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnShort, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnAverage, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 305);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(378, 55);
            this.tableLayoutPanel6.TabIndex = 66;
            // 
            // btnLong
            // 
            this.btnLong.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLong.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLong.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLong.BackgroundImage")));
            this.btnLong.BorderColor = System.Drawing.Color.Silver;
            this.btnLong.BorderRadius = 10;
            this.btnLong.BorderSize = 1;
            this.btnLong.ButtonImage = null;
            this.btnLong.Corner = BeeGlobal.Corner.Right;
            this.btnLong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLong.FlatAppearance.BorderSize = 0;
            this.btnLong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLong.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLong.ForeColor = System.Drawing.Color.Black;
            this.btnLong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLong.IsCLick = false;
            this.btnLong.IsNotChange = false;
            this.btnLong.IsRect = false;
            this.btnLong.IsUnGroup = false;
            this.btnLong.Location = new System.Drawing.Point(255, 0);
            this.btnLong.Margin = new System.Windows.Forms.Padding(0);
            this.btnLong.Name = "btnLong";
            this.btnLong.Size = new System.Drawing.Size(123, 55);
            this.btnLong.TabIndex = 4;
            this.btnLong.Text = "Longest";
            this.btnLong.TextColor = System.Drawing.Color.Black;
            this.btnLong.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLong.UseVisualStyleBackColor = false;
            this.btnLong.Click += new System.EventHandler(this.btnLong_Click);
            // 
            // btnShort
            // 
            this.btnShort.BackColor = System.Drawing.Color.Transparent;
            this.btnShort.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnShort.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnShort.BackgroundImage")));
            this.btnShort.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShort.BorderColor = System.Drawing.Color.Transparent;
            this.btnShort.BorderRadius = 10;
            this.btnShort.BorderSize = 1;
            this.btnShort.ButtonImage = null;
            this.btnShort.Corner = BeeGlobal.Corner.Left;
            this.btnShort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShort.FlatAppearance.BorderSize = 0;
            this.btnShort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShort.ForeColor = System.Drawing.Color.Black;
            this.btnShort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShort.IsCLick = false;
            this.btnShort.IsNotChange = false;
            this.btnShort.IsRect = false;
            this.btnShort.IsUnGroup = false;
            this.btnShort.Location = new System.Drawing.Point(0, 0);
            this.btnShort.Margin = new System.Windows.Forms.Padding(0);
            this.btnShort.Name = "btnShort";
            this.btnShort.Size = new System.Drawing.Size(122, 55);
            this.btnShort.TabIndex = 2;
            this.btnShort.Text = "Shortest";
            this.btnShort.TextColor = System.Drawing.Color.Black;
            this.btnShort.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShort.UseVisualStyleBackColor = false;
            this.btnShort.Click += new System.EventHandler(this.btnShort_Click);
            // 
            // btnAverage
            // 
            this.btnAverage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAverage.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnAverage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAverage.BackgroundImage")));
            this.btnAverage.BorderColor = System.Drawing.Color.Silver;
            this.btnAverage.BorderRadius = 5;
            this.btnAverage.BorderSize = 1;
            this.btnAverage.ButtonImage = null;
            this.btnAverage.Corner = BeeGlobal.Corner.None;
            this.btnAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAverage.FlatAppearance.BorderSize = 0;
            this.btnAverage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAverage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAverage.ForeColor = System.Drawing.Color.Black;
            this.btnAverage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAverage.IsCLick = true;
            this.btnAverage.IsNotChange = false;
            this.btnAverage.IsRect = false;
            this.btnAverage.IsUnGroup = false;
            this.btnAverage.Location = new System.Drawing.Point(122, 0);
            this.btnAverage.Margin = new System.Windows.Forms.Padding(0);
            this.btnAverage.Name = "btnAverage";
            this.btnAverage.Size = new System.Drawing.Size(133, 55);
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
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 215);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(378, 55);
            this.tableLayoutPanel5.TabIndex = 65;
            // 
            // btnFar
            // 
            this.btnFar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFar.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnFar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFar.BackgroundImage")));
            this.btnFar.BorderColor = System.Drawing.Color.Silver;
            this.btnFar.BorderRadius = 10;
            this.btnFar.BorderSize = 1;
            this.btnFar.ButtonImage = null;
            this.btnFar.Corner = BeeGlobal.Corner.Right;
            this.btnFar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFar.FlatAppearance.BorderSize = 0;
            this.btnFar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFar.ForeColor = System.Drawing.Color.Black;
            this.btnFar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFar.IsCLick = false;
            this.btnFar.IsNotChange = false;
            this.btnFar.IsRect = false;
            this.btnFar.IsUnGroup = false;
            this.btnFar.Location = new System.Drawing.Point(282, 0);
            this.btnFar.Margin = new System.Windows.Forms.Padding(0);
            this.btnFar.Name = "btnFar";
            this.btnFar.Size = new System.Drawing.Size(96, 55);
            this.btnFar.TabIndex = 5;
            this.btnFar.Text = "Far";
            this.btnFar.TextColor = System.Drawing.Color.Black;
            this.btnFar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFar.UseVisualStyleBackColor = false;
            this.btnFar.Click += new System.EventHandler(this.btnFar_Click);
            // 
            // btnNear
            // 
            this.btnNear.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNear.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnNear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNear.BackgroundImage")));
            this.btnNear.BorderColor = System.Drawing.Color.Silver;
            this.btnNear.BorderRadius = 10;
            this.btnNear.BorderSize = 1;
            this.btnNear.ButtonImage = null;
            this.btnNear.Corner = BeeGlobal.Corner.None;
            this.btnNear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNear.FlatAppearance.BorderSize = 0;
            this.btnNear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNear.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNear.ForeColor = System.Drawing.Color.Black;
            this.btnNear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNear.IsCLick = false;
            this.btnNear.IsNotChange = false;
            this.btnNear.IsRect = false;
            this.btnNear.IsUnGroup = false;
            this.btnNear.Location = new System.Drawing.Point(188, 0);
            this.btnNear.Margin = new System.Windows.Forms.Padding(0);
            this.btnNear.Name = "btnNear";
            this.btnNear.Size = new System.Drawing.Size(94, 55);
            this.btnNear.TabIndex = 4;
            this.btnNear.Text = "Near";
            this.btnNear.TextColor = System.Drawing.Color.Black;
            this.btnNear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNear.UseVisualStyleBackColor = false;
            this.btnNear.Click += new System.EventHandler(this.btnNear_Click);
            // 
            // btnMid
            // 
            this.btnMid.BackColor = System.Drawing.Color.Transparent;
            this.btnMid.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnMid.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMid.BackgroundImage")));
            this.btnMid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMid.BorderColor = System.Drawing.Color.Transparent;
            this.btnMid.BorderRadius = 10;
            this.btnMid.BorderSize = 1;
            this.btnMid.ButtonImage = null;
            this.btnMid.Corner = BeeGlobal.Corner.Left;
            this.btnMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMid.FlatAppearance.BorderSize = 0;
            this.btnMid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMid.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMid.ForeColor = System.Drawing.Color.Black;
            this.btnMid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMid.IsCLick = true;
            this.btnMid.IsNotChange = false;
            this.btnMid.IsRect = false;
            this.btnMid.IsUnGroup = false;
            this.btnMid.Location = new System.Drawing.Point(0, 0);
            this.btnMid.Margin = new System.Windows.Forms.Padding(0);
            this.btnMid.Name = "btnMid";
            this.btnMid.Size = new System.Drawing.Size(94, 55);
            this.btnMid.TabIndex = 2;
            this.btnMid.Text = "Middle";
            this.btnMid.TextColor = System.Drawing.Color.Black;
            this.btnMid.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMid.UseVisualStyleBackColor = false;
            this.btnMid.Click += new System.EventHandler(this.btnMid_Click);
            // 
            // btnOutter
            // 
            this.btnOutter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnOutter.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnOutter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOutter.BackgroundImage")));
            this.btnOutter.BorderColor = System.Drawing.Color.Silver;
            this.btnOutter.BorderRadius = 5;
            this.btnOutter.BorderSize = 1;
            this.btnOutter.ButtonImage = null;
            this.btnOutter.Corner = BeeGlobal.Corner.None;
            this.btnOutter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOutter.FlatAppearance.BorderSize = 0;
            this.btnOutter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOutter.ForeColor = System.Drawing.Color.Black;
            this.btnOutter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOutter.IsCLick = false;
            this.btnOutter.IsNotChange = false;
            this.btnOutter.IsRect = false;
            this.btnOutter.IsUnGroup = false;
            this.btnOutter.Location = new System.Drawing.Point(94, 0);
            this.btnOutter.Margin = new System.Windows.Forms.Padding(0);
            this.btnOutter.Name = "btnOutter";
            this.btnOutter.Size = new System.Drawing.Size(94, 55);
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
            this.label10.Location = new System.Drawing.Point(3, 185);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(154, 25);
            this.label10.TabIndex = 64;
            this.label10.Text = "Gap Extremum";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnHori, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnVer, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 125);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(378, 55);
            this.tableLayoutPanel3.TabIndex = 63;
            // 
            // btnHori
            // 
            this.btnHori.BackColor = System.Drawing.Color.Transparent;
            this.btnHori.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnHori.BorderColor = System.Drawing.Color.Silver;
            this.btnHori.BorderRadius = 10;
            this.btnHori.BorderSize = 1;
            this.btnHori.ButtonImage = null;
            this.btnHori.Corner = BeeGlobal.Corner.Right;
            this.btnHori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHori.FlatAppearance.BorderSize = 0;
            this.btnHori.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHori.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHori.ForeColor = System.Drawing.Color.Black;
            this.btnHori.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHori.IsCLick = false;
            this.btnHori.IsNotChange = false;
            this.btnHori.IsRect = false;
            this.btnHori.IsUnGroup = false;
            this.btnHori.Location = new System.Drawing.Point(189, 0);
            this.btnHori.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnHori.Name = "btnHori";
            this.btnHori.Size = new System.Drawing.Size(186, 55);
            this.btnHori.TabIndex = 3;
            this.btnHori.Text = "Horizontal";
            this.btnHori.TextColor = System.Drawing.Color.Black;
            this.btnHori.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHori.UseVisualStyleBackColor = false;
            this.btnHori.Click += new System.EventHandler(this.btnHori_Click);
            // 
            // btnVer
            // 
            this.btnVer.BackColor = System.Drawing.Color.Transparent;
            this.btnVer.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnVer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVer.BorderColor = System.Drawing.Color.Transparent;
            this.btnVer.BorderRadius = 10;
            this.btnVer.BorderSize = 1;
            this.btnVer.ButtonImage = null;
            this.btnVer.Corner = BeeGlobal.Corner.Left;
            this.btnVer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVer.FlatAppearance.BorderSize = 0;
            this.btnVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVer.ForeColor = System.Drawing.Color.Black;
            this.btnVer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVer.IsCLick = true;
            this.btnVer.IsNotChange = false;
            this.btnVer.IsRect = false;
            this.btnVer.IsUnGroup = false;
            this.btnVer.Location = new System.Drawing.Point(3, 0);
            this.btnVer.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnVer.Name = "btnVer";
            this.btnVer.Size = new System.Drawing.Size(186, 55);
            this.btnVer.TabIndex = 2;
            this.btnVer.Text = "Vertical";
            this.btnVer.TextColor = System.Drawing.Color.Black;
            this.btnVer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVer.UseVisualStyleBackColor = false;
            this.btnVer.Click += new System.EventHandler(this.btnVer_Click);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel15.ColumnCount = 3;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.Controls.Add(this.btnBinary, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnStrongEdge, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnCloseEdge, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(5, 395);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(378, 55);
            this.tableLayoutPanel15.TabIndex = 62;
            // 
            // btnBinary
            // 
            this.btnBinary.BackColor = System.Drawing.Color.Transparent;
            this.btnBinary.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnBinary.BorderColor = System.Drawing.Color.Silver;
            this.btnBinary.BorderRadius = 10;
            this.btnBinary.BorderSize = 1;
            this.btnBinary.ButtonImage = null;
            this.btnBinary.Corner = BeeGlobal.Corner.Right;
            this.btnBinary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBinary.FlatAppearance.BorderSize = 0;
            this.btnBinary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBinary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBinary.ForeColor = System.Drawing.Color.Black;
            this.btnBinary.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBinary.IsCLick = false;
            this.btnBinary.IsNotChange = false;
            this.btnBinary.IsRect = false;
            this.btnBinary.IsUnGroup = false;
            this.btnBinary.Location = new System.Drawing.Point(252, 0);
            this.btnBinary.Margin = new System.Windows.Forms.Padding(0);
            this.btnBinary.Name = "btnBinary";
            this.btnBinary.Size = new System.Drawing.Size(126, 55);
            this.btnBinary.TabIndex = 4;
            this.btnBinary.Text = "Binay";
            this.btnBinary.TextColor = System.Drawing.Color.Black;
            this.btnBinary.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBinary.UseVisualStyleBackColor = false;
            this.btnBinary.Click += new System.EventHandler(this.btnBinary_Click);
            // 
            // btnStrongEdge
            // 
            this.btnStrongEdge.BackColor = System.Drawing.Color.Transparent;
            this.btnStrongEdge.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnStrongEdge.BorderColor = System.Drawing.Color.Silver;
            this.btnStrongEdge.BorderRadius = 10;
            this.btnStrongEdge.BorderSize = 1;
            this.btnStrongEdge.ButtonImage = null;
            this.btnStrongEdge.Corner = BeeGlobal.Corner.Right;
            this.btnStrongEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStrongEdge.FlatAppearance.BorderSize = 0;
            this.btnStrongEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStrongEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStrongEdge.ForeColor = System.Drawing.Color.Black;
            this.btnStrongEdge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStrongEdge.IsCLick = false;
            this.btnStrongEdge.IsNotChange = false;
            this.btnStrongEdge.IsRect = false;
            this.btnStrongEdge.IsUnGroup = false;
            this.btnStrongEdge.Location = new System.Drawing.Point(126, 0);
            this.btnStrongEdge.Margin = new System.Windows.Forms.Padding(0);
            this.btnStrongEdge.Name = "btnStrongEdge";
            this.btnStrongEdge.Size = new System.Drawing.Size(126, 55);
            this.btnStrongEdge.TabIndex = 3;
            this.btnStrongEdge.Text = "Strong Edge";
            this.btnStrongEdge.TextColor = System.Drawing.Color.Black;
            this.btnStrongEdge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStrongEdge.UseVisualStyleBackColor = false;
            // 
            // btnCloseEdge
            // 
            this.btnCloseEdge.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseEdge.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCloseEdge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseEdge.BorderColor = System.Drawing.Color.Transparent;
            this.btnCloseEdge.BorderRadius = 10;
            this.btnCloseEdge.BorderSize = 1;
            this.btnCloseEdge.ButtonImage = null;
            this.btnCloseEdge.Corner = BeeGlobal.Corner.Left;
            this.btnCloseEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseEdge.FlatAppearance.BorderSize = 0;
            this.btnCloseEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseEdge.ForeColor = System.Drawing.Color.Black;
            this.btnCloseEdge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseEdge.IsCLick = true;
            this.btnCloseEdge.IsNotChange = false;
            this.btnCloseEdge.IsRect = false;
            this.btnCloseEdge.IsUnGroup = false;
            this.btnCloseEdge.Location = new System.Drawing.Point(3, 0);
            this.btnCloseEdge.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCloseEdge.Name = "btnCloseEdge";
            this.btnCloseEdge.Size = new System.Drawing.Size(123, 55);
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
            this.label2.Location = new System.Drawing.Point(3, 365);
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
            this.label3.Location = new System.Drawing.Point(3, 275);
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
            this.label13.Location = new System.Drawing.Point(3, 95);
            this.label13.Margin = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(97, 25);
            this.label13.TabIndex = 58;
            this.label13.Text = "Direction";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layScore
            // 
            this.layScore.BackColor = System.Drawing.Color.Transparent;
            this.layScore.ColumnCount = 4;
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.layScore.Controls.Add(this.rjButton4, 3, 0);
            this.layScore.Controls.Add(this.numScore, 2, 0);
            this.layScore.Controls.Add(this.rjButton5, 0, 0);
            this.layScore.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.layScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layScore.Location = new System.Drawing.Point(5, 483);
            this.layScore.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layScore.Name = "layScore";
            this.layScore.RowCount = 1;
            this.layScore.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layScore.Size = new System.Drawing.Size(376, 66);
            this.layScore.TabIndex = 46;
            // 
            // rjButton4
            // 
            this.rjButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton4.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton4.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton4.BorderRadius = 10;
            this.rjButton4.BorderSize = 1;
            this.rjButton4.ButtonImage = null;
            this.rjButton4.Corner = BeeGlobal.Corner.Right;
            this.rjButton4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton4.Enabled = false;
            this.rjButton4.FlatAppearance.BorderSize = 0;
            this.rjButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton4.ForeColor = System.Drawing.Color.Black;
            this.rjButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton4.IsCLick = false;
            this.rjButton4.IsNotChange = true;
            this.rjButton4.IsRect = false;
            this.rjButton4.IsUnGroup = false;
            this.rjButton4.Location = new System.Drawing.Point(371, 0);
            this.rjButton4.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton4.Name = "rjButton4";
            this.rjButton4.Size = new System.Drawing.Size(5, 66);
            this.rjButton4.TabIndex = 37;
            this.rjButton4.TextColor = System.Drawing.Color.Black;
            this.rjButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton4.UseVisualStyleBackColor = false;
            // 
            // numScore
            // 
            this.numScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numScore.Location = new System.Drawing.Point(231, 0);
            this.numScore.Margin = new System.Windows.Forms.Padding(0);
            this.numScore.Maxnimum = 100F;
            this.numScore.Minimum = 0F;
            this.numScore.Name = "numScore";
            this.numScore.Size = new System.Drawing.Size(140, 66);
            this.numScore.Step = 1F;
            this.numScore.TabIndex = 35;
            this.numScore.Value = 100F;
            this.numScore.ValueChanged += new System.EventHandler(this.numScore_ValueChanged);
            // 
            // rjButton5
            // 
            this.rjButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton5.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton5.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton5.BorderRadius = 5;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.ButtonImage = null;
            this.rjButton5.Corner = BeeGlobal.Corner.Left;
            this.rjButton5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton5.Enabled = false;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton5.IsCLick = false;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = false;
            this.rjButton5.IsUnGroup = false;
            this.rjButton5.Location = new System.Drawing.Point(0, 0);
            this.rjButton5.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(5, 66);
            this.rjButton5.TabIndex = 2;
            this.rjButton5.TextColor = System.Drawing.Color.Black;
            this.rjButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton5.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.trackScore, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(5, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.39683F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.60317F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(226, 66);
            this.tableLayoutPanel7.TabIndex = 36;
            // 
            // trackScore
            // 
            this.trackScore.BackColor = System.Drawing.Color.Transparent;
            this.trackScore.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackScore.Location = new System.Drawing.Point(3, 16);
            this.trackScore.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.trackScore.Max = 20F;
            this.trackScore.Min = 1F;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(220, 50);
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 28;
            this.trackScore.Value = 1F;
            this.trackScore.ValueScore = 0F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            this.trackScore.Load += new System.EventHandler(this.trackScore_Load);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 455);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(378, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.Transparent;
            this.btnTest.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderRadius = 1;
            this.btnTest.BorderSize = 1;
            this.btnTest.ButtonImage = null;
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.IsCLick = false;
            this.btnTest.IsNotChange = true;
            this.btnTest.IsRect = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(20, 569);
            this.btnTest.Margin = new System.Windows.Forms.Padding(20);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(346, 238);
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
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 55);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // btnCropFull
            // 
            this.btnCropFull.BackColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BorderColor = System.Drawing.Color.Silver;
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.ButtonImage = null;
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropFull.ForeColor = System.Drawing.Color.Black;
            this.btnCropFull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropFull.IsCLick = false;
            this.btnCropFull.IsNotChange = false;
            this.btnCropFull.IsRect = false;
            this.btnCropFull.IsUnGroup = false;
            this.btnCropFull.Location = new System.Drawing.Point(189, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(186, 55);
            this.btnCropFull.TabIndex = 3;
            this.btnCropFull.Text = "Partial";
            this.btnCropFull.TextColor = System.Drawing.Color.Black;
            this.btnCropFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropFull.UseVisualStyleBackColor = false;
            this.btnCropFull.Click += new System.EventHandler(this.btnCropFull_Click);
            // 
            // btnCropHalt
            // 
            this.btnCropHalt.BackColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.ButtonImage = null;
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropHalt.ForeColor = System.Drawing.Color.Black;
            this.btnCropHalt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropHalt.IsCLick = true;
            this.btnCropHalt.IsNotChange = false;
            this.btnCropHalt.IsRect = false;
            this.btnCropHalt.IsUnGroup = false;
            this.btnCropHalt.Location = new System.Drawing.Point(3, 0);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(186, 55);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 833);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.btnCalib, 0, 9);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 8);
            this.tableLayoutPanel8.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel4, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel11, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 11;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(386, 827);
            this.tableLayoutPanel8.TabIndex = 1;
            this.tableLayoutPanel8.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel8_Paint);
            // 
            // btnCalib
            // 
            this.btnCalib.BackColor = System.Drawing.Color.Transparent;
            this.btnCalib.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCalib.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalib.BorderColor = System.Drawing.Color.Transparent;
            this.btnCalib.BorderRadius = 10;
            this.btnCalib.BorderSize = 1;
            this.btnCalib.ButtonImage = null;
            this.btnCalib.Corner = BeeGlobal.Corner.Left;
            this.btnCalib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalib.FlatAppearance.BorderSize = 0;
            this.btnCalib.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalib.ForeColor = System.Drawing.Color.Black;
            this.btnCalib.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCalib.IsCLick = false;
            this.btnCalib.IsNotChange = true;
            this.btnCalib.IsRect = false;
            this.btnCalib.IsUnGroup = false;
            this.btnCalib.Location = new System.Drawing.Point(3, 511);
            this.btnCalib.Margin = new System.Windows.Forms.Padding(3, 20, 0, 0);
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
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 436);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 15, 3, 0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(378, 55);
            this.tableLayoutPanel14.TabIndex = 61;
            // 
            // rjButton2
            // 
            this.rjButton2.BackColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 10;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ButtonImage = null;
            this.rjButton2.Corner = BeeGlobal.Corner.Left;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.Enabled = false;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton2.ForeColor = System.Drawing.Color.White;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = false;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(3, 0);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 220);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 25);
            this.label1.TabIndex = 59;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 320);
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
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 376);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(378, 45);
            this.tableLayoutPanel4.TabIndex = 55;
            // 
            // numMinRadius
            // 
            this.numMinRadius.BackColor = System.Drawing.Color.Transparent;
            this.numMinRadius.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numMinRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMinRadius.ForeColor = System.Drawing.Color.Red;
            this.numMinRadius.Location = new System.Drawing.Point(0, 0);
            this.numMinRadius.Margin = new System.Windows.Forms.Padding(0);
            this.numMinRadius.Maxnimum = 1000F;
            this.numMinRadius.Minimum = 0F;
            this.numMinRadius.Name = "numMinRadius";
            this.numMinRadius.Size = new System.Drawing.Size(189, 45);
            this.numMinRadius.Step = 1F;
            this.numMinRadius.TabIndex = 38;
            this.numMinRadius.Value = 0F;
            // 
            // numMaxRadius
            // 
            this.numMaxRadius.BackColor = System.Drawing.Color.Transparent;
            this.numMaxRadius.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numMaxRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMaxRadius.ForeColor = System.Drawing.Color.Red;
            this.numMaxRadius.Location = new System.Drawing.Point(189, 0);
            this.numMaxRadius.Margin = new System.Windows.Forms.Padding(0);
            this.numMaxRadius.Maxnimum = 10000F;
            this.numMaxRadius.Minimum = 0F;
            this.numMaxRadius.Name = "numMaxRadius";
            this.numMaxRadius.Size = new System.Drawing.Size(189, 45);
            this.numMaxRadius.Step = 1F;
            this.numMaxRadius.TabIndex = 35;
            this.numMaxRadius.Value = 10000F;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel11.ColumnCount = 3;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel11.Controls.Add(this.rjButton8, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.rjButton9, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel12, 1, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(5, 145);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(378, 65);
            this.tableLayoutPanel11.TabIndex = 49;
            // 
            // rjButton8
            // 
            this.rjButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton8.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton8.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton8.BorderRadius = 10;
            this.rjButton8.BorderSize = 1;
            this.rjButton8.ButtonImage = null;
            this.rjButton8.Corner = BeeGlobal.Corner.Right;
            this.rjButton8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton8.Enabled = false;
            this.rjButton8.FlatAppearance.BorderSize = 0;
            this.rjButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton8.ForeColor = System.Drawing.Color.Black;
            this.rjButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton8.IsCLick = false;
            this.rjButton8.IsNotChange = true;
            this.rjButton8.IsRect = false;
            this.rjButton8.IsUnGroup = false;
            this.rjButton8.Location = new System.Drawing.Point(373, 0);
            this.rjButton8.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton8.Name = "rjButton8";
            this.rjButton8.Size = new System.Drawing.Size(5, 65);
            this.rjButton8.TabIndex = 37;
            this.rjButton8.TextColor = System.Drawing.Color.Black;
            this.rjButton8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton8.UseVisualStyleBackColor = false;
            // 
            // rjButton9
            // 
            this.rjButton9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton9.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton9.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton9.BorderRadius = 10;
            this.rjButton9.BorderSize = 1;
            this.rjButton9.ButtonImage = null;
            this.rjButton9.Corner = BeeGlobal.Corner.Left;
            this.rjButton9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton9.Enabled = false;
            this.rjButton9.FlatAppearance.BorderSize = 0;
            this.rjButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton9.ForeColor = System.Drawing.Color.Black;
            this.rjButton9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton9.IsCLick = false;
            this.rjButton9.IsNotChange = true;
            this.rjButton9.IsRect = false;
            this.rjButton9.IsUnGroup = false;
            this.rjButton9.Location = new System.Drawing.Point(0, 0);
            this.rjButton9.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton9.Name = "rjButton9";
            this.rjButton9.Size = new System.Drawing.Size(10, 65);
            this.rjButton9.TabIndex = 2;
            this.rjButton9.TextColor = System.Drawing.Color.Black;
            this.rjButton9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton9.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel12.ColumnCount = 1;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Controls.Add(this.trackMinInlier, 0, 1);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 2;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.39683F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.60317F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(363, 65);
            this.tableLayoutPanel12.TabIndex = 36;
            // 
            // trackMinInlier
            // 
            this.trackMinInlier.BackColor = System.Drawing.Color.Transparent;
            this.trackMinInlier.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackMinInlier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackMinInlier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackMinInlier.Location = new System.Drawing.Point(3, 16);
            this.trackMinInlier.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.trackMinInlier.Max = 10000F;
            this.trackMinInlier.Min = 1F;
            this.trackMinInlier.Name = "trackMinInlier";
            this.trackMinInlier.Size = new System.Drawing.Size(357, 49);
            this.trackMinInlier.Step = 1F;
            this.trackMinInlier.TabIndex = 28;
            this.trackMinInlier.Value = 2F;
            this.trackMinInlier.ValueScore = 0F;
            this.trackMinInlier.ValueChanged += new System.Action<float>(this.trackMinInlier_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 115);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 25);
            this.label6.TabIndex = 48;
            this.label6.Text = "Min Inliers";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.Controls.Add(this.rjButton6, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.rjButton7, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel10, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 40);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(378, 65);
            this.tableLayoutPanel9.TabIndex = 47;
            // 
            // rjButton6
            // 
            this.rjButton6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton6.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton6.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton6.BorderRadius = 10;
            this.rjButton6.BorderSize = 1;
            this.rjButton6.ButtonImage = null;
            this.rjButton6.Corner = BeeGlobal.Corner.Right;
            this.rjButton6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton6.Enabled = false;
            this.rjButton6.FlatAppearance.BorderSize = 0;
            this.rjButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton6.ForeColor = System.Drawing.Color.Black;
            this.rjButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton6.IsCLick = false;
            this.rjButton6.IsNotChange = true;
            this.rjButton6.IsRect = false;
            this.rjButton6.IsUnGroup = false;
            this.rjButton6.Location = new System.Drawing.Point(373, 0);
            this.rjButton6.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton6.Name = "rjButton6";
            this.rjButton6.Size = new System.Drawing.Size(5, 65);
            this.rjButton6.TabIndex = 37;
            this.rjButton6.TextColor = System.Drawing.Color.Black;
            this.rjButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton6.UseVisualStyleBackColor = false;
            // 
            // rjButton7
            // 
            this.rjButton7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton7.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton7.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton7.BorderRadius = 10;
            this.rjButton7.BorderSize = 1;
            this.rjButton7.ButtonImage = null;
            this.rjButton7.Corner = BeeGlobal.Corner.Left;
            this.rjButton7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton7.Enabled = false;
            this.rjButton7.FlatAppearance.BorderSize = 0;
            this.rjButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton7.ForeColor = System.Drawing.Color.Black;
            this.rjButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton7.IsCLick = false;
            this.rjButton7.IsNotChange = true;
            this.rjButton7.IsRect = false;
            this.rjButton7.IsUnGroup = false;
            this.rjButton7.Location = new System.Drawing.Point(0, 0);
            this.rjButton7.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton7.Name = "rjButton7";
            this.rjButton7.Size = new System.Drawing.Size(10, 65);
            this.rjButton7.TabIndex = 2;
            this.rjButton7.TextColor = System.Drawing.Color.Black;
            this.rjButton7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton7.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Controls.Add(this.trackMaxLine, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.39683F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.60317F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(363, 65);
            this.tableLayoutPanel10.TabIndex = 36;
            // 
            // trackMaxLine
            // 
            this.trackMaxLine.BackColor = System.Drawing.Color.Transparent;
            this.trackMaxLine.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackMaxLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackMaxLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackMaxLine.Location = new System.Drawing.Point(3, 16);
            this.trackMaxLine.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.trackMaxLine.Max = 10F;
            this.trackMaxLine.Min = 1F;
            this.trackMaxLine.Name = "trackMaxLine";
            this.trackMaxLine.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackMaxLine.Size = new System.Drawing.Size(357, 49);
            this.trackMaxLine.Step = 1F;
            this.trackMaxLine.TabIndex = 28;
            this.trackMaxLine.Value = 4F;
            this.trackMaxLine.ValueScore = 0F;
            this.trackMaxLine.ValueChanged += new System.Action<float>(this.trackMaxLine_ValueChanged);
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
            this.btnAreaBlack.BackColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaBlack.BackgroundImage")));
            this.btnAreaBlack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAreaBlack.BorderColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BorderRadius = 5;
            this.btnAreaBlack.BorderSize = 1;
            this.btnAreaBlack.ButtonImage = null;
            this.btnAreaBlack.Corner = BeeGlobal.Corner.Both;
            this.btnAreaBlack.FlatAppearance.BorderSize = 0;
            this.btnAreaBlack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaBlack.ForeColor = System.Drawing.Color.Black;
            this.btnAreaBlack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaBlack.IsCLick = true;
            this.btnAreaBlack.IsNotChange = false;
            this.btnAreaBlack.IsRect = false;
            this.btnAreaBlack.IsUnGroup = false;
            this.btnAreaBlack.Location = new System.Drawing.Point(3, 5);
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
            this.btnAreaWhite.BackColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaWhite.BackgroundImage")));
            this.btnAreaWhite.BorderColor = System.Drawing.Color.Silver;
            this.btnAreaWhite.BorderRadius = 5;
            this.btnAreaWhite.BorderSize = 1;
            this.btnAreaWhite.ButtonImage = null;
            this.btnAreaWhite.Corner = BeeGlobal.Corner.Both;
            this.btnAreaWhite.FlatAppearance.BorderSize = 0;
            this.btnAreaWhite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaWhite.ForeColor = System.Drawing.Color.Black;
            this.btnAreaWhite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaWhite.IsCLick = false;
            this.btnAreaWhite.IsNotChange = false;
            this.btnAreaWhite.IsRect = false;
            this.btnAreaWhite.IsUnGroup = false;
            this.btnAreaWhite.Location = new System.Drawing.Point(195, 5);
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
            this.layScore.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private RJButton btnStrongEdge;
        private RJButton btnCloseEdge;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel layScore;
        private RJButton rjButton4;
        private CustomNumeric numScore;
        private RJButton rjButton5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        public TrackBar2 trackScore;
        private System.Windows.Forms.Label label8;
        private RJButton btnTest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private RJButton btnCalib;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private RJButton rjButton2;
        private System.Windows.Forms.NumericUpDown numScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private CustomNumeric numMinRadius;
        private CustomNumeric numMaxRadius;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private RJButton rjButton8;
        private RJButton rjButton9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        public TrackBar2 trackMinInlier;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private RJButton rjButton6;
        private RJButton rjButton7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        public TrackBar2 trackMaxLine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnHori;
        private RJButton btnVer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnNear;
        private RJButton btnMid;
        private RJButton btnOutter;
        private RJButton btnFar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnLong;
        private RJButton btnShort;
        private RJButton btnAverage;
        private RJButton btnBinary;
    }
}
