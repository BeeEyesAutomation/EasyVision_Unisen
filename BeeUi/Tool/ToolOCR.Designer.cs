namespace BeeUi.Tool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolOCR));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.btnApply = new BeeUi.Common.RJButton();
            this.txtAllow = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.numMatching = new BeeUi.Commons.CustomNumeric();
            this.label4 = new System.Windows.Forms.Label();
            this.tabLabelResult = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSet = new BeeUi.Common.RJButton();
            this.txtContent = new System.Windows.Forms.TextBox();
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
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeUi.Common.RJButton();
            this.btnCropHalt = new BeeUi.Common.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.layoutLineLimit = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton1 = new BeeUi.Common.RJButton();
            this.btnMoreArea = new BeeUi.Common.RJButton();
            this.btnLessArea = new BeeUi.Common.RJButton();
            this.numLimtArea = new BeeUi.Commons.CustomNumeric();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEnLimitArea = new BeeUi.Common.RJButton();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.numBlur = new BeeUi.Commons.CustomNumeric();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numUnsharp = new BeeUi.Commons.CustomNumeric();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.numCLAHE = new BeeUi.Commons.CustomNumeric();
            this.label1 = new System.Windows.Forms.Label();
            this.tmCheckFist = new System.Windows.Forms.Timer(this.components);
            this.workLoadModel = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.layScore.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.layoutLineLimit.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // threadProcess
            // 
            this.threadProcess.WorkerReportsProgress = true;
            this.threadProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.threadProcess_DoWork);
            this.threadProcess.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.threadProcess_RunWorkerCompleted);
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
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(392, 799);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            this.tabP1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel12, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel11, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tabLabelResult, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.layScore, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 793);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel12
            // 
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
            this.tableLayoutPanel12.Size = new System.Drawing.Size(380, 134);
            this.tableLayoutPanel12.TabIndex = 53;
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.Transparent;
            this.btnApply.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnApply.BorderColor = System.Drawing.Color.Silver;
            this.btnApply.BorderRadius = 10;
            this.btnApply.BorderSize = 1;
            this.btnApply.ButtonImage = null;
            this.btnApply.Corner = BeeCore.Corner.Right;
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.Black;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApply.IsCLick = false;
            this.btnApply.IsNotChange = true;
            this.btnApply.IsRect = false;
            this.btnApply.IsUnGroup = false;
            this.btnApply.Location = new System.Drawing.Point(290, 10);
            this.btnApply.Margin = new System.Windows.Forms.Padding(10);
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
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel11.Controls.Add(this.numMatching, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(8, 335);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(8, 5, 3, 3);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(375, 49);
            this.tableLayoutPanel11.TabIndex = 51;
            // 
            // numMatching
            // 
            this.numMatching.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numMatching.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numMatching.Location = new System.Drawing.Point(204, 0);
            this.numMatching.Margin = new System.Windows.Forms.Padding(0);
            this.numMatching.Maxnimum = 10;
            this.numMatching.Minimum = 0;
            this.numMatching.Name = "numMatching";
            this.numMatching.Size = new System.Drawing.Size(171, 49);
            this.numMatching.Step = 1;
            this.numMatching.TabIndex = 46;
            this.numMatching.Value = 1;
            this.numMatching.ValueChanged += new System.EventHandler(this.numMatching_ValueChanged);
            this.numMatching.Load += new System.EventHandler(this.numMatching_Load);
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
            this.label4.Text = "Number Char Error";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnSet.BackColor = System.Drawing.Color.Transparent;
            this.btnSet.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnSet.BorderColor = System.Drawing.Color.Silver;
            this.btnSet.BorderRadius = 10;
            this.btnSet.BorderSize = 1;
            this.btnSet.ButtonImage = null;
            this.btnSet.Corner = BeeCore.Corner.Right;
            this.btnSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSet.FlatAppearance.BorderSize = 0;
            this.btnSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet.ForeColor = System.Drawing.Color.Black;
            this.btnSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSet.IsCLick = false;
            this.btnSet.IsNotChange = true;
            this.btnSet.IsRect = false;
            this.btnSet.IsUnGroup = false;
            this.btnSet.Location = new System.Drawing.Point(280, 0);
            this.btnSet.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
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
            this.layScore.Location = new System.Drawing.Point(5, 525);
            this.layScore.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layScore.Name = "layScore";
            this.layScore.RowCount = 1;
            this.layScore.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layScore.Size = new System.Drawing.Size(376, 64);
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
            this.rjButton4.Size = new System.Drawing.Size(5, 64);
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
            this.numScore.Size = new System.Drawing.Size(200, 64);
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
            this.rjButton5.Size = new System.Drawing.Size(5, 64);
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
            this.tableLayoutPanel7.Size = new System.Drawing.Size(166, 64);
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
            this.trackScore.Max = 100F;
            this.trackScore.Min = 0F;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(160, 48);
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 28;
            this.trackScore.Value = 100F;
            this.trackScore.ValueScore = 0F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            this.trackScore.Load += new System.EventHandler(this.trackScore_Load_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 500);
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
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 733);
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
            this.btnTest.Location = new System.Drawing.Point(5, 599);
            this.btnTest.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(376, 119);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click_1);
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
            this.rjButton1.Size = new System.Drawing.Size(5, 48);
            this.rjButton1.TabIndex = 38;
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // btnMoreArea
            // 
            this.btnMoreArea.BackColor = System.Drawing.Color.LightGray;
            this.btnMoreArea.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnMoreArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMoreArea.BorderColor = System.Drawing.Color.Transparent;
            this.btnMoreArea.BorderRadius = 5;
            this.btnMoreArea.BorderSize = 1;
            this.btnMoreArea.ButtonImage = null;
            this.btnMoreArea.Corner = BeeCore.Corner.None;
            this.btnMoreArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMoreArea.FlatAppearance.BorderSize = 0;
            this.btnMoreArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoreArea.ForeColor = System.Drawing.Color.Black;
            this.btnMoreArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoreArea.IsCLick = true;
            this.btnMoreArea.IsNotChange = false;
            this.btnMoreArea.IsRect = false;
            this.btnMoreArea.IsUnGroup = false;
            this.btnMoreArea.Location = new System.Drawing.Point(75, 0);
            this.btnMoreArea.Margin = new System.Windows.Forms.Padding(0);
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
            this.btnLessArea.BackColor = System.Drawing.Color.LightGray;
            this.btnLessArea.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnLessArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLessArea.BorderColor = System.Drawing.Color.Transparent;
            this.btnLessArea.BorderRadius = 10;
            this.btnLessArea.BorderSize = 1;
            this.btnLessArea.ButtonImage = null;
            this.btnLessArea.Corner = BeeCore.Corner.Left;
            this.btnLessArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLessArea.FlatAppearance.BorderSize = 0;
            this.btnLessArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLessArea.ForeColor = System.Drawing.Color.Black;
            this.btnLessArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLessArea.IsCLick = false;
            this.btnLessArea.IsNotChange = false;
            this.btnLessArea.IsRect = false;
            this.btnLessArea.IsUnGroup = false;
            this.btnLessArea.Location = new System.Drawing.Point(0, 0);
            this.btnLessArea.Margin = new System.Windows.Forms.Padding(0);
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
            this.numLimtArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numLimtArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numLimtArea.Location = new System.Drawing.Point(150, 0);
            this.numLimtArea.Margin = new System.Windows.Forms.Padding(0);
            this.numLimtArea.Maxnimum = 2000;
            this.numLimtArea.Minimum = 0;
            this.numLimtArea.Name = "numLimtArea";
            this.numLimtArea.Size = new System.Drawing.Size(221, 48);
            this.numLimtArea.Step = 1;
            this.numLimtArea.TabIndex = 34;
            this.numLimtArea.Value = 500;
            this.numLimtArea.ValueChanged += new System.EventHandler(this.numLimtArea_ValueChanged);
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
            this.btnEnLimitArea.BackColor = System.Drawing.Color.Transparent;
            this.btnEnLimitArea.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnEnLimitArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnLimitArea.BorderColor = System.Drawing.Color.Transparent;
            this.btnEnLimitArea.BorderRadius = 10;
            this.btnEnLimitArea.BorderSize = 1;
            this.btnEnLimitArea.ButtonImage = null;
            this.btnEnLimitArea.Corner = BeeCore.Corner.None;
            this.btnEnLimitArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnLimitArea.FlatAppearance.BorderSize = 0;
            this.btnEnLimitArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnLimitArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnLimitArea.ForeColor = System.Drawing.Color.Black;
            this.btnEnLimitArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnLimitArea.IsCLick = false;
            this.btnEnLimitArea.IsNotChange = false;
            this.btnEnLimitArea.IsRect = false;
            this.btnEnLimitArea.IsUnGroup = true;
            this.btnEnLimitArea.Location = new System.Drawing.Point(261, 3);
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
            this.numBlur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numBlur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numBlur.Location = new System.Drawing.Point(185, 0);
            this.numBlur.Margin = new System.Windows.Forms.Padding(0);
            this.numBlur.Maxnimum = 100;
            this.numBlur.Minimum = 0;
            this.numBlur.Name = "numBlur";
            this.numBlur.Size = new System.Drawing.Size(186, 50);
            this.numBlur.Step = 1;
            this.numBlur.TabIndex = 45;
            this.numBlur.Value = 3;
            this.numBlur.ValueChanged += new System.EventHandler(this.numBlur_ValueChanged);
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
            this.numUnsharp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numUnsharp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numUnsharp.Location = new System.Drawing.Point(185, 0);
            this.numUnsharp.Margin = new System.Windows.Forms.Padding(0);
            this.numUnsharp.Maxnimum = 100;
            this.numUnsharp.Minimum = 0;
            this.numUnsharp.Name = "numUnsharp";
            this.numUnsharp.Size = new System.Drawing.Size(186, 38);
            this.numUnsharp.Step = 1;
            this.numUnsharp.TabIndex = 45;
            this.numUnsharp.Value = 3;
            this.numUnsharp.ValueChanged += new System.EventHandler(this.numUnsharp_ValueChanged);
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
            this.numCLAHE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numCLAHE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numCLAHE.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCLAHE.Location = new System.Drawing.Point(186, 0);
            this.numCLAHE.Margin = new System.Windows.Forms.Padding(0);
            this.numCLAHE.Maxnimum = 100;
            this.numCLAHE.Minimum = 0;
            this.numCLAHE.Name = "numCLAHE";
            this.numCLAHE.Size = new System.Drawing.Size(187, 48);
            this.numCLAHE.Step = 1;
            this.numCLAHE.TabIndex = 45;
            this.numCLAHE.Value = 2;
            this.numCLAHE.ValueChanged += new System.EventHandler(this.numCLAHE_ValueChanged);
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
            // ToolOCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
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
            this.layScore.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
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
            this.ResumeLayout(false);

        }

        #endregion
        public System.ComponentModel.BackgroundWorker threadProcess;
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
        private Common.RJButton btnTest;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Common.RJButton btnCropFull;
        private Common.RJButton btnCropHalt;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Timer tmCheckFist;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Common.RJButton btnSet;
        private System.Windows.Forms.TextBox txtContent;
        private System.ComponentModel.BackgroundWorker workLoadModel;
        private System.Windows.Forms.TableLayoutPanel tabLabelResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Commons.CustomNumeric numUnsharp;
        private System.Windows.Forms.Label label2;
        private Commons.CustomNumeric numCLAHE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private Commons.CustomNumeric numBlur;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label3;
        private Common.RJButton btnEnLimitArea;
        private System.Windows.Forms.TableLayoutPanel layoutLineLimit;
        private Common.RJButton rjButton1;
        private Common.RJButton btnMoreArea;
        private Common.RJButton btnLessArea;
        private Commons.CustomNumeric numLimtArea;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private Common.RJButton btnApply;
        private System.Windows.Forms.TextBox txtAllow;
        private System.Windows.Forms.Label label10;
        private Commons.CustomNumeric numMatching;
    }
}
