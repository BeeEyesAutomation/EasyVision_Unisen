namespace BeeUi.Tool
{
    partial class ToolYolo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolYolo));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.layScore = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton4 = new BeeUi.Common.RJButton();
            this.numScore = new BeeUi.Commons.CustomNumeric();
            this.rjButton5 = new BeeUi.Common.RJButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.trackScore = new BeeUi.TrackBar2();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new BeeUi.Common.RJButton();
            this.btnCancel = new BeeUi.Common.RJButton();
            this.btnTest = new BeeUi.Common.RJButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropMask = new BeeUi.Common.RJButton();
            this.btnCropRect = new BeeUi.Common.RJButton();
            this.btnCropArea = new BeeUi.Common.RJButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeUi.Common.RJButton();
            this.btnCropHalt = new BeeUi.Common.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton1 = new BeeUi.Common.RJButton();
            this.rjButton2 = new BeeUi.Common.RJButton();
            this.rjButton3 = new BeeUi.Common.RJButton();
            this.rjButton6 = new BeeUi.Common.RJButton();
            this.customNumeric1 = new BeeUi.Commons.CustomNumeric();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSetLabel = new BeeUi.Common.RJButton();
            this.btnPathModel = new BeeUi.Common.RJButton();
            this.tabLbs = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton10 = new BeeUi.Common.RJButton();
            this.btnMore = new BeeUi.Common.RJButton();
            this.btnEqual = new BeeUi.Common.RJButton();
            this.btnLess = new BeeUi.Common.RJButton();
            this.trackNumObject = new BeeUi.Commons.CustomNumeric();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCheckArea = new BeeUi.Common.RJButton();
            this.tmCheckFist = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.layScore.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // threadProcess
            // 
            this.threadProcess.WorkerReportsProgress = true;
            this.threadProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.threadProcess_DoWork);
            this.threadProcess.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.threadProcess_RunWorkerCompleted);
            // 
            // txtLabel
            // 
            this.txtLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLabel.Location = new System.Drawing.Point(67, 43);
            this.txtLabel.Multiline = true;
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(143, 62);
            this.txtLabel.TabIndex = 36;
            this.txtLabel.TextChanged += new System.EventHandler(this.txtLabel_TextChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 40);
            this.label1.TabIndex = 34;
            this.label1.Text = "Path";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtModel
            // 
            this.txtModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModel.Location = new System.Drawing.Point(67, 3);
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(143, 29);
            this.txtModel.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 68);
            this.label7.TabIndex = 30;
            this.label7.Text = "Labels";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label7.Click += new System.EventHandler(this.label7_Click);
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
            this.tabControl1.Size = new System.Drawing.Size(400, 613);
            this.tabControl1.TabIndex = 18;
            // 
            // tabP1
            // 
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(392, 575);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            this.tabP1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.layScore, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 569);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // layScore
            // 
            this.layScore.BackColor = System.Drawing.Color.Transparent;
            this.layScore.ColumnCount = 4;
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.layScore.Controls.Add(this.rjButton4, 3, 0);
            this.layScore.Controls.Add(this.numScore, 2, 0);
            this.layScore.Controls.Add(this.rjButton5, 0, 0);
            this.layScore.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.layScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layScore.Location = new System.Drawing.Point(5, 190);
            this.layScore.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layScore.Name = "layScore";
            this.layScore.RowCount = 1;
            this.layScore.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layScore.Size = new System.Drawing.Size(376, 63);
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
            this.rjButton4.Corner = BeeCore.Corner.Right;
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
            this.rjButton4.Size = new System.Drawing.Size(5, 63);
            this.rjButton4.TabIndex = 37;
            this.rjButton4.TextColor = System.Drawing.Color.Black;
            this.rjButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton4.UseVisualStyleBackColor = false;
            // 
            // numScore
            // 
            this.numScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numScore.Location = new System.Drawing.Point(171, 0);
            this.numScore.Margin = new System.Windows.Forms.Padding(0);
            this.numScore.Maxnimum = 100;
            this.numScore.Minimum = 0;
            this.numScore.Name = "numScore";
            this.numScore.Size = new System.Drawing.Size(200, 63);
            this.numScore.Step = 1;
            this.numScore.TabIndex = 35;
            this.numScore.Value = 100;
            this.numScore.ValueChanged += new System.EventHandler(this.numScore_ValueChanged_1);
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
            this.rjButton5.Corner = BeeCore.Corner.Left;
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
            this.rjButton5.Size = new System.Drawing.Size(5, 63);
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
            this.tableLayoutPanel7.Size = new System.Drawing.Size(166, 63);
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
            this.trackScore.Max = 100;
            this.trackScore.Min = 0;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(160, 47);
            this.trackScore.Step = 1;
            this.trackScore.TabIndex = 28;
            this.trackScore.Value = 100;
            this.trackScore.ValueScore = 0;
            this.trackScore.ValueChanged += new System.Action<int>(this.trackScore_ValueChanged);
            this.trackScore.Load += new System.EventHandler(this.trackScore_Load_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 165);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(378, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 509);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(376, 55);
            this.tableLayoutPanel6.TabIndex = 45;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnOK.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.BorderRadius = 4;
            this.btnOK.BorderSize = 1;
            this.btnOK.ButtonImage = null;
            this.btnOK.Corner = BeeCore.Corner.Left;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.IsCLick = true;
            this.btnOK.IsNotChange = true;
            this.btnOK.IsRect = false;
            this.btnOK.IsUnGroup = false;
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(185, 49);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.TextColor = System.Drawing.Color.Black;
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 4;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.ButtonImage = null;
            this.btnCancel.Corner = BeeCore.Corner.Right;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.IsCLick = false;
            this.btnCancel.IsNotChange = true;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(188, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(185, 49);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.Black;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.Transparent;
            this.btnTest.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderRadius = 1;
            this.btnTest.BorderSize = 1;
            this.btnTest.ButtonImage = null;
            this.btnTest.Corner = BeeCore.Corner.Both;
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
            this.btnTest.Location = new System.Drawing.Point(5, 263);
            this.btnTest.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(376, 231);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click_1);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.Controls.Add(this.btnCropMask, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropRect, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropArea, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 104);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(378, 56);
            this.tableLayoutPanel3.TabIndex = 41;
            // 
            // btnCropMask
            // 
            this.btnCropMask.BackColor = System.Drawing.Color.Transparent;
            this.btnCropMask.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropMask.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropMask.BackgroundImage")));
            this.btnCropMask.BorderColor = System.Drawing.Color.Silver;
            this.btnCropMask.BorderRadius = 10;
            this.btnCropMask.BorderSize = 1;
            this.btnCropMask.ButtonImage = null;
            this.btnCropMask.Corner = BeeCore.Corner.Right;
            this.btnCropMask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropMask.Enabled = false;
            this.btnCropMask.FlatAppearance.BorderSize = 0;
            this.btnCropMask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropMask.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropMask.ForeColor = System.Drawing.Color.Black;
            this.btnCropMask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropMask.IsCLick = false;
            this.btnCropMask.IsNotChange = false;
            this.btnCropMask.IsRect = false;
            this.btnCropMask.IsUnGroup = false;
            this.btnCropMask.Location = new System.Drawing.Point(255, 3);
            this.btnCropMask.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnCropMask.Name = "btnCropMask";
            this.btnCropMask.Size = new System.Drawing.Size(120, 50);
            this.btnCropMask.TabIndex = 4;
            this.btnCropMask.Text = "Area Mask";
            this.btnCropMask.TextColor = System.Drawing.Color.Black;
            this.btnCropMask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropMask.UseVisualStyleBackColor = false;
            // 
            // btnCropRect
            // 
            this.btnCropRect.BackColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropRect.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BorderRadius = 10;
            this.btnCropRect.BorderSize = 1;
            this.btnCropRect.ButtonImage = null;
            this.btnCropRect.Corner = BeeCore.Corner.Left;
            this.btnCropRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropRect.Enabled = false;
            this.btnCropRect.FlatAppearance.BorderSize = 0;
            this.btnCropRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropRect.ForeColor = System.Drawing.Color.Black;
            this.btnCropRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropRect.IsCLick = false;
            this.btnCropRect.IsNotChange = false;
            this.btnCropRect.IsRect = false;
            this.btnCropRect.IsUnGroup = false;
            this.btnCropRect.Location = new System.Drawing.Point(3, 3);
            this.btnCropRect.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnCropRect.Name = "btnCropRect";
            this.btnCropRect.Size = new System.Drawing.Size(119, 50);
            this.btnCropRect.TabIndex = 2;
            this.btnCropRect.Text = "Area Temp";
            this.btnCropRect.TextColor = System.Drawing.Color.Black;
            this.btnCropRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropRect.UseVisualStyleBackColor = false;
            this.btnCropRect.Click += new System.EventHandler(this.btnCropRect_Click_1);
            // 
            // btnCropArea
            // 
            this.btnCropArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropArea.BackgroundImage")));
            this.btnCropArea.BorderColor = System.Drawing.Color.Silver;
            this.btnCropArea.BorderRadius = 5;
            this.btnCropArea.BorderSize = 1;
            this.btnCropArea.ButtonImage = null;
            this.btnCropArea.Corner = BeeCore.Corner.None;
            this.btnCropArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropArea.ForeColor = System.Drawing.Color.Black;
            this.btnCropArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropArea.IsCLick = true;
            this.btnCropArea.IsNotChange = false;
            this.btnCropArea.IsRect = false;
            this.btnCropArea.IsUnGroup = false;
            this.btnCropArea.Location = new System.Drawing.Point(122, 3);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(133, 50);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 79);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 25);
            this.label5.TabIndex = 40;
            this.label5.Text = "Choose Area";
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
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
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
            this.btnCropFull.BackColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BorderColor = System.Drawing.Color.Silver;
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.ButtonImage = null;
            this.btnCropFull.Corner = BeeCore.Corner.Right;
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
            this.btnCropHalt.BackColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.ButtonImage = null;
            this.btnCropHalt.Corner = BeeCore.Corner.Left;
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
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 799);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel10, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel15, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel5, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.btnCheckArea, 0, 5);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 8;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 304F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(386, 793);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 5;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel10.Controls.Add(this.rjButton1, 4, 0);
            this.tableLayoutPanel10.Controls.Add(this.rjButton2, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.rjButton3, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.rjButton6, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.customNumeric1, 3, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(5, 547);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(376, 52);
            this.tableLayoutPanel10.TabIndex = 46;
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton1.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton1.BorderRadius = 10;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.ButtonImage = null;
            this.rjButton1.Corner = BeeCore.Corner.Right;
            this.rjButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton1.Enabled = false;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = true;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(371, 0);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(5, 52);
            this.rjButton1.TabIndex = 38;
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // rjButton2
            // 
            this.rjButton2.BackColor = System.Drawing.Color.LightGray;
            this.rjButton2.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 5;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ButtonImage = null;
            this.rjButton2.Corner = BeeCore.Corner.None;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = false;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(150, 0);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(75, 52);
            this.rjButton2.TabIndex = 33;
            this.rjButton2.Text = "More";
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // rjButton3
            // 
            this.rjButton3.BackColor = System.Drawing.Color.Gray;
            this.rjButton3.BackgroundColor = System.Drawing.Color.Gray;
            this.rjButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton3.BorderColor = System.Drawing.Color.LightGray;
            this.rjButton3.BorderRadius = 5;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.ButtonImage = null;
            this.rjButton3.Corner = BeeCore.Corner.None;
            this.rjButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.ForeColor = System.Drawing.Color.Black;
            this.rjButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton3.IsCLick = true;
            this.rjButton3.IsNotChange = false;
            this.rjButton3.IsRect = false;
            this.rjButton3.IsUnGroup = false;
            this.rjButton3.Location = new System.Drawing.Point(75, 0);
            this.rjButton3.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(75, 52);
            this.rjButton3.TabIndex = 32;
            this.rjButton3.Text = "Equal";
            this.rjButton3.TextColor = System.Drawing.Color.Black;
            this.rjButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton3.UseVisualStyleBackColor = false;
            // 
            // rjButton6
            // 
            this.rjButton6.BackColor = System.Drawing.Color.LightGray;
            this.rjButton6.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton6.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton6.BorderRadius = 10;
            this.rjButton6.BorderSize = 1;
            this.rjButton6.ButtonImage = null;
            this.rjButton6.Corner = BeeCore.Corner.Left;
            this.rjButton6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton6.FlatAppearance.BorderSize = 0;
            this.rjButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton6.ForeColor = System.Drawing.Color.Black;
            this.rjButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton6.IsCLick = false;
            this.rjButton6.IsNotChange = false;
            this.rjButton6.IsRect = false;
            this.rjButton6.IsUnGroup = false;
            this.rjButton6.Location = new System.Drawing.Point(0, 0);
            this.rjButton6.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton6.Name = "rjButton6";
            this.rjButton6.Size = new System.Drawing.Size(75, 52);
            this.rjButton6.TabIndex = 31;
            this.rjButton6.Text = "Less";
            this.rjButton6.TextColor = System.Drawing.Color.Black;
            this.rjButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton6.UseVisualStyleBackColor = false;
            this.rjButton6.Click += new System.EventHandler(this.rjButton6_Click_1);
            // 
            // customNumeric1
            // 
            this.customNumeric1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.customNumeric1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customNumeric1.Location = new System.Drawing.Point(225, 0);
            this.customNumeric1.Margin = new System.Windows.Forms.Padding(0);
            this.customNumeric1.Maxnimum = 100;
            this.customNumeric1.Minimum = 0;
            this.customNumeric1.Name = "customNumeric1";
            this.customNumeric1.Size = new System.Drawing.Size(146, 52);
            this.customNumeric1.Step = 1;
            this.customNumeric1.TabIndex = 34;
            this.customNumeric1.Value = 0;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 339);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(378, 44);
            this.tableLayoutPanel9.TabIndex = 45;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 1;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.92369F));
            this.tableLayoutPanel15.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.tabLbs, 0, 1);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(3, 38);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 2;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.09524F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.90476F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(380, 298);
            this.tableLayoutPanel15.TabIndex = 6;
            this.tableLayoutPanel15.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel15_Paint);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.36437F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.63563F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162F));
            this.tableLayoutPanel4.Controls.Add(this.btnSetLabel, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtLabel, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnPathModel, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtModel, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(1, 5);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(1, 5, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(376, 108);
            this.tableLayoutPanel4.TabIndex = 43;
            // 
            // btnSetLabel
            // 
            this.btnSetLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSetLabel.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnSetLabel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSetLabel.BackgroundImage")));
            this.btnSetLabel.BorderColor = System.Drawing.Color.Transparent;
            this.btnSetLabel.BorderRadius = 10;
            this.btnSetLabel.BorderSize = 1;
            this.btnSetLabel.ButtonImage = null;
            this.btnSetLabel.Corner = BeeCore.Corner.Both;
            this.btnSetLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetLabel.FlatAppearance.BorderSize = 0;
            this.btnSetLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetLabel.ForeColor = System.Drawing.Color.Black;
            this.btnSetLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSetLabel.IsCLick = false;
            this.btnSetLabel.IsNotChange = true;
            this.btnSetLabel.IsRect = false;
            this.btnSetLabel.IsUnGroup = true;
            this.btnSetLabel.Location = new System.Drawing.Point(216, 43);
            this.btnSetLabel.Name = "btnSetLabel";
            this.btnSetLabel.Size = new System.Drawing.Size(157, 62);
            this.btnSetLabel.TabIndex = 37;
            this.btnSetLabel.Text = "SetLabel";
            this.btnSetLabel.TextColor = System.Drawing.Color.Black;
            this.btnSetLabel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSetLabel.UseVisualStyleBackColor = false;
            this.btnSetLabel.Click += new System.EventHandler(this.btnSetLabel_Click);
            // 
            // btnPathModel
            // 
            this.btnPathModel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPathModel.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnPathModel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPathModel.BackgroundImage")));
            this.btnPathModel.BorderColor = System.Drawing.Color.Transparent;
            this.btnPathModel.BorderRadius = 10;
            this.btnPathModel.BorderSize = 1;
            this.btnPathModel.ButtonImage = null;
            this.btnPathModel.Corner = BeeCore.Corner.Both;
            this.btnPathModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPathModel.FlatAppearance.BorderSize = 0;
            this.btnPathModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPathModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPathModel.ForeColor = System.Drawing.Color.Black;
            this.btnPathModel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPathModel.IsCLick = false;
            this.btnPathModel.IsNotChange = true;
            this.btnPathModel.IsRect = false;
            this.btnPathModel.IsUnGroup = true;
            this.btnPathModel.Location = new System.Drawing.Point(216, 3);
            this.btnPathModel.Name = "btnPathModel";
            this.btnPathModel.Size = new System.Drawing.Size(157, 34);
            this.btnPathModel.TabIndex = 5;
            this.btnPathModel.Text = "Load Model";
            this.btnPathModel.TextColor = System.Drawing.Color.Black;
            this.btnPathModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPathModel.UseVisualStyleBackColor = false;
            this.btnPathModel.Click += new System.EventHandler(this.btnPathModel_Click);
            // 
            // tabLbs
            // 
            this.tabLbs.ColumnCount = 4;
            this.tabLbs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabLbs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabLbs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabLbs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabLbs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLbs.Location = new System.Drawing.Point(3, 123);
            this.tabLbs.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tabLbs.Name = "tabLbs";
            this.tabLbs.RowCount = 4;
            this.tabLbs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabLbs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabLbs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabLbs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tabLbs.Size = new System.Drawing.Size(374, 172);
            this.tabLbs.TabIndex = 44;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 5;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel5.Controls.Add(this.rjButton10, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnMore, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnEqual, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnLess, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.trackNumObject, 3, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 423);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(376, 60);
            this.tableLayoutPanel5.TabIndex = 44;
            // 
            // rjButton10
            // 
            this.rjButton10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton10.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton10.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton10.BorderRadius = 10;
            this.rjButton10.BorderSize = 1;
            this.rjButton10.ButtonImage = null;
            this.rjButton10.Corner = BeeCore.Corner.Right;
            this.rjButton10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton10.Enabled = false;
            this.rjButton10.FlatAppearance.BorderSize = 0;
            this.rjButton10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton10.ForeColor = System.Drawing.Color.Black;
            this.rjButton10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton10.IsCLick = false;
            this.rjButton10.IsNotChange = true;
            this.rjButton10.IsRect = false;
            this.rjButton10.IsUnGroup = false;
            this.rjButton10.Location = new System.Drawing.Point(371, 0);
            this.rjButton10.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton10.Name = "rjButton10";
            this.rjButton10.Size = new System.Drawing.Size(5, 60);
            this.rjButton10.TabIndex = 38;
            this.rjButton10.TextColor = System.Drawing.Color.Black;
            this.rjButton10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton10.UseVisualStyleBackColor = false;
            // 
            // btnMore
            // 
            this.btnMore.BackColor = System.Drawing.Color.LightGray;
            this.btnMore.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnMore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMore.BorderColor = System.Drawing.Color.Transparent;
            this.btnMore.BorderRadius = 5;
            this.btnMore.BorderSize = 1;
            this.btnMore.ButtonImage = null;
            this.btnMore.Corner = BeeCore.Corner.None;
            this.btnMore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMore.FlatAppearance.BorderSize = 0;
            this.btnMore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMore.ForeColor = System.Drawing.Color.Black;
            this.btnMore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMore.IsCLick = false;
            this.btnMore.IsNotChange = false;
            this.btnMore.IsRect = false;
            this.btnMore.IsUnGroup = false;
            this.btnMore.Location = new System.Drawing.Point(150, 0);
            this.btnMore.Margin = new System.Windows.Forms.Padding(0);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(75, 60);
            this.btnMore.TabIndex = 33;
            this.btnMore.Text = "More";
            this.btnMore.TextColor = System.Drawing.Color.Black;
            this.btnMore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMore.UseVisualStyleBackColor = false;
            this.btnMore.Click += new System.EventHandler(this.rjButton3_Click_2);
            // 
            // btnEqual
            // 
            this.btnEqual.BackColor = System.Drawing.Color.Gray;
            this.btnEqual.BackgroundColor = System.Drawing.Color.Gray;
            this.btnEqual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEqual.BorderColor = System.Drawing.Color.LightGray;
            this.btnEqual.BorderRadius = 5;
            this.btnEqual.BorderSize = 1;
            this.btnEqual.ButtonImage = null;
            this.btnEqual.Corner = BeeCore.Corner.None;
            this.btnEqual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEqual.FlatAppearance.BorderSize = 0;
            this.btnEqual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEqual.ForeColor = System.Drawing.Color.Black;
            this.btnEqual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEqual.IsCLick = true;
            this.btnEqual.IsNotChange = false;
            this.btnEqual.IsRect = false;
            this.btnEqual.IsUnGroup = false;
            this.btnEqual.Location = new System.Drawing.Point(75, 0);
            this.btnEqual.Margin = new System.Windows.Forms.Padding(0);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(75, 60);
            this.btnEqual.TabIndex = 32;
            this.btnEqual.Text = "Equal";
            this.btnEqual.TextColor = System.Drawing.Color.Black;
            this.btnEqual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEqual.UseVisualStyleBackColor = false;
            this.btnEqual.Click += new System.EventHandler(this.rjButton6_Click);
            // 
            // btnLess
            // 
            this.btnLess.BackColor = System.Drawing.Color.LightGray;
            this.btnLess.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnLess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLess.BorderColor = System.Drawing.Color.Transparent;
            this.btnLess.BorderRadius = 10;
            this.btnLess.BorderSize = 1;
            this.btnLess.ButtonImage = null;
            this.btnLess.Corner = BeeCore.Corner.Left;
            this.btnLess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLess.FlatAppearance.BorderSize = 0;
            this.btnLess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLess.ForeColor = System.Drawing.Color.Black;
            this.btnLess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLess.IsCLick = false;
            this.btnLess.IsNotChange = false;
            this.btnLess.IsRect = false;
            this.btnLess.IsUnGroup = false;
            this.btnLess.Location = new System.Drawing.Point(0, 0);
            this.btnLess.Margin = new System.Windows.Forms.Padding(0);
            this.btnLess.Name = "btnLess";
            this.btnLess.Size = new System.Drawing.Size(75, 60);
            this.btnLess.TabIndex = 31;
            this.btnLess.Text = "Less";
            this.btnLess.TextColor = System.Drawing.Color.Black;
            this.btnLess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLess.UseVisualStyleBackColor = false;
            this.btnLess.Click += new System.EventHandler(this.rjButton7_Click);
            // 
            // trackNumObject
            // 
            this.trackNumObject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.trackNumObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackNumObject.Location = new System.Drawing.Point(225, 0);
            this.trackNumObject.Margin = new System.Windows.Forms.Padding(0);
            this.trackNumObject.Maxnimum = 100;
            this.trackNumObject.Minimum = 0;
            this.trackNumObject.Name = "trackNumObject";
            this.trackNumObject.Size = new System.Drawing.Size(146, 60);
            this.trackNumObject.Step = 1;
            this.trackNumObject.TabIndex = 34;
            this.trackNumObject.Value = 0;
            this.trackNumObject.ValueChanged += new System.EventHandler(this.trackNumObject_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 25);
            this.label2.TabIndex = 43;
            this.label2.Text = "Set Model";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 398);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 15, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 25);
            this.label3.TabIndex = 44;
            this.label3.Text = "Limited Counter";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCheckArea
            // 
            this.btnCheckArea.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckArea.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCheckArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCheckArea.BorderColor = System.Drawing.Color.Transparent;
            this.btnCheckArea.BorderRadius = 10;
            this.btnCheckArea.BorderSize = 1;
            this.btnCheckArea.ButtonImage = null;
            this.btnCheckArea.Corner = BeeCore.Corner.Left;
            this.btnCheckArea.FlatAppearance.BorderSize = 0;
            this.btnCheckArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckArea.ForeColor = System.Drawing.Color.Black;
            this.btnCheckArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckArea.IsCLick = false;
            this.btnCheckArea.IsNotChange = false;
            this.btnCheckArea.IsRect = false;
            this.btnCheckArea.IsUnGroup = false;
            this.btnCheckArea.Location = new System.Drawing.Point(3, 498);
            this.btnCheckArea.Margin = new System.Windows.Forms.Padding(3, 15, 0, 5);
            this.btnCheckArea.Name = "btnCheckArea";
            this.btnCheckArea.Size = new System.Drawing.Size(186, 44);
            this.btnCheckArea.TabIndex = 2;
            this.btnCheckArea.Text = "Check Area";
            this.btnCheckArea.TextColor = System.Drawing.Color.Black;
            this.btnCheckArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCheckArea.UseVisualStyleBackColor = false;
            this.btnCheckArea.Click += new System.EventHandler(this.btnCheckArea_Click);
            // 
            // tmCheckFist
            // 
            this.tmCheckFist.Tick += new System.EventHandler(this.tmCheckFist_Tick);
            // 
            // ToolYolo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ToolYolo";
            this.Size = new System.Drawing.Size(400, 613);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.layScore.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.ComponentModel.BackgroundWorker threadProcess;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel layScore;
        private Common.RJButton rjButton4;
        private Commons.CustomNumeric numScore;
        private Common.RJButton rjButton5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        public TrackBar2 trackScore;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Common.RJButton btnOK;
        private Common.RJButton btnCancel;
        private System.Windows.Forms.Label label3;
        private Common.RJButton btnTest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Common.RJButton btnPathModel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Common.RJButton rjButton10;
        private Common.RJButton btnMore;
        private Common.RJButton btnEqual;
        private Common.RJButton btnLess;
        private Commons.CustomNumeric trackNumObject;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Common.RJButton btnCropMask;
        private Common.RJButton btnCropRect;
        private Common.RJButton btnCropArea;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Common.RJButton btnCropFull;
        private Common.RJButton btnCropHalt;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private Common.RJButton btnSetLabel;
        private System.Windows.Forms.TableLayoutPanel tabLbs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer tmCheckFist;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private Common.RJButton btnCheckArea;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private Common.RJButton rjButton1;
        private Common.RJButton rjButton2;
        private Common.RJButton rjButton3;
        private Common.RJButton rjButton6;
        private Commons.CustomNumeric customNumeric1;
    }
}
