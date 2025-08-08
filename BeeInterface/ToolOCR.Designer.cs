using BeeGlobal;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolOCR));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.trackScore = new BeeInterface.AdjustBar();
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
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new BeeInterface.RJButton();
            this.btnCancel = new BeeInterface.RJButton();
            this.btnTest = new BeeInterface.RJButton();
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
            this.numLimtArea = new BeeInterface.CustomNumeric();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEnLimitArea = new BeeInterface.RJButton();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.numBlur = new BeeInterface.CustomNumeric();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numUnsharp = new BeeInterface.CustomNumeric();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.numCLAHE = new BeeInterface.CustomNumeric();
            this.label1 = new System.Windows.Forms.Label();
            this.tmCheckFist = new System.Windows.Forms.Timer(this.components);
            this.workLoadModel = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabP1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(533, 1030);
            this.tabControl1.TabIndex = 18;
            // 
            // tabP1
            // 
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabP1.Size = new System.Drawing.Size(525, 992);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            this.tabP1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel12, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel11, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tabLabelResult, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(517, 984);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // trackScore
            // 
            this.trackScore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackScore.Location = new System.Drawing.Point(5, 617);
            this.trackScore.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.trackScore.Max = 100F;
            this.trackScore.Min = 0F;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(507, 69);
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 68;
            this.trackScore.Value = 0F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel12.Controls.Add(this.btnApply, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.txtAllow, 0, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(4, 220);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(509, 164);
            this.tableLayoutPanel12.TabIndex = 53;
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.Transparent;
            this.btnApply.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnApply.BorderColor = System.Drawing.Color.Silver;
            this.btnApply.BorderRadius = 10;
            this.btnApply.BorderSize = 1;
            this.btnApply.Image = null;
            this.btnApply.Corner = BeeGlobal.Corner.Right;
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
            this.btnApply.Location = new System.Drawing.Point(389, 12);
            this.btnApply.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(107, 140);
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
            this.txtAllow.Location = new System.Drawing.Point(27, 4);
            this.txtAllow.Margin = new System.Windows.Forms.Padding(27, 4, 4, 4);
            this.txtAllow.Multiline = true;
            this.txtAllow.Name = "txtAllow";
            this.txtAllow.Size = new System.Drawing.Size(345, 156);
            this.txtAllow.TabIndex = 47;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 191);
            this.label10.Margin = new System.Windows.Forms.Padding(7, 6, 4, 0);
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
            this.tableLayoutPanel11.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.btnReadThresh, 1, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(11, 394);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(11, 6, 4, 4);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(502, 60);
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
            this.label4.Size = new System.Drawing.Size(273, 60);
            this.label4.TabIndex = 45;
            this.label4.Text = "Set Temp";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnReadThresh
            // 
            this.btnReadThresh.BackColor = System.Drawing.Color.Transparent;
            this.btnReadThresh.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnReadThresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReadThresh.BorderColor = System.Drawing.Color.Transparent;
            this.btnReadThresh.BorderRadius = 10;
            this.btnReadThresh.BorderSize = 1;
            this.btnReadThresh.Image = null;
            this.btnReadThresh.Corner = BeeGlobal.Corner.None;
            this.btnReadThresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadThresh.FlatAppearance.BorderSize = 0;
            this.btnReadThresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadThresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadThresh.ForeColor = System.Drawing.Color.Black;
            this.btnReadThresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadThresh.IsCLick = false;
            this.btnReadThresh.IsNotChange = false;
            this.btnReadThresh.IsRect = false;
            this.btnReadThresh.IsUnGroup = true;
            this.btnReadThresh.Location = new System.Drawing.Point(277, 4);
            this.btnReadThresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReadThresh.Name = "btnReadThresh";
            this.btnReadThresh.Size = new System.Drawing.Size(221, 52);
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
            this.tabLabelResult.Location = new System.Drawing.Point(4, 462);
            this.tabLabelResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabLabelResult.Name = "tabLabelResult";
            this.tabLabelResult.RowCount = 2;
            this.tabLabelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabLabelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabLabelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tabLabelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tabLabelResult.Size = new System.Drawing.Size(509, 115);
            this.tabLabelResult.TabIndex = 48;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel3.Controls.Add(this.btnSet, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtContent, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 120);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(509, 61);
            this.tableLayoutPanel3.TabIndex = 47;
            // 
            // btnSet
            // 
            this.btnSet.BackColor = System.Drawing.Color.Transparent;
            this.btnSet.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnSet.BorderColor = System.Drawing.Color.Silver;
            this.btnSet.BorderRadius = 10;
            this.btnSet.BorderSize = 1;
            this.btnSet.Image = null;
            this.btnSet.Corner = BeeGlobal.Corner.Right;
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
            this.btnSet.Location = new System.Drawing.Point(376, 0);
            this.btnSet.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(129, 61);
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
            this.txtContent.Location = new System.Drawing.Point(27, 4);
            this.txtContent.Margin = new System.Windows.Forms.Padding(27, 4, 4, 4);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(345, 44);
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
            this.label8.Location = new System.Drawing.Point(7, 587);
            this.label8.Margin = new System.Windows.Forms.Padding(7, 6, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(506, 25);
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
            this.tableLayoutPanel6.Location = new System.Drawing.Point(7, 910);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(503, 68);
            this.tableLayoutPanel6.TabIndex = 45;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnOK.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.BorderRadius = 4;
            this.btnOK.BorderSize = 1;
            this.btnOK.Image = null;
            this.btnOK.Corner = BeeGlobal.Corner.Left;
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
            this.btnOK.Location = new System.Drawing.Point(4, 4);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 0, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(247, 60);
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
            this.btnCancel.Image = null;
            this.btnCancel.Corner = BeeGlobal.Corner.Right;
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
            this.btnCancel.Location = new System.Drawing.Point(251, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(248, 60);
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
            this.btnTest.Image = null;
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
            this.btnTest.Location = new System.Drawing.Point(7, 703);
            this.btnTest.Margin = new System.Windows.Forms.Padding(7, 12, 7, 12);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(503, 189);
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
            this.label5.Location = new System.Drawing.Point(7, 91);
            this.label5.Margin = new System.Windows.Forms.Padding(7, 6, 4, 0);
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
            this.label9.Location = new System.Drawing.Point(7, 6);
            this.label9.Margin = new System.Windows.Forms.Padding(7, 6, 4, 0);
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(7, 31);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(7, 0, 4, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(506, 54);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // btnCropFull
            // 
            this.btnCropFull.BackColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BorderColor = System.Drawing.Color.Silver;
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.Image = null;
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
            this.btnCropFull.Location = new System.Drawing.Point(253, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(249, 54);
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
            this.btnCropHalt.Image = null;
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
            this.btnCropHalt.Location = new System.Drawing.Point(4, 0);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(249, 54);
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
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage4.Size = new System.Drawing.Size(525, 992);
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
            this.tableLayoutPanel8.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(4, 25, 4, 4);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 9;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(517, 984);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // layoutLineLimit
            // 
            this.layoutLineLimit.ColumnCount = 4;
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.layoutLineLimit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.layoutLineLimit.Controls.Add(this.rjButton1, 3, 0);
            this.layoutLineLimit.Controls.Add(this.btnMoreArea, 1, 0);
            this.layoutLineLimit.Controls.Add(this.btnLessArea, 0, 0);
            this.layoutLineLimit.Controls.Add(this.numLimtArea, 2, 0);
            this.layoutLineLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLineLimit.Enabled = false;
            this.layoutLineLimit.Location = new System.Drawing.Point(7, 316);
            this.layoutLineLimit.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.layoutLineLimit.Name = "layoutLineLimit";
            this.layoutLineLimit.RowCount = 1;
            this.layoutLineLimit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLineLimit.Size = new System.Drawing.Size(503, 59);
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
            this.rjButton1.Image = null;
            this.rjButton1.Corner = BeeGlobal.Corner.Right;
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
            this.rjButton1.Location = new System.Drawing.Point(496, 0);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(7, 59);
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
            this.btnMoreArea.Image = null;
            this.btnMoreArea.Corner = BeeGlobal.Corner.None;
            this.btnMoreArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMoreArea.FlatAppearance.BorderSize = 0;
            this.btnMoreArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoreArea.ForeColor = System.Drawing.Color.Black;
            this.btnMoreArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoreArea.IsCLick = true;
            this.btnMoreArea.IsNotChange = false;
            this.btnMoreArea.IsRect = false;
            this.btnMoreArea.IsUnGroup = false;
            this.btnMoreArea.Location = new System.Drawing.Point(100, 0);
            this.btnMoreArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnMoreArea.Name = "btnMoreArea";
            this.btnMoreArea.Size = new System.Drawing.Size(100, 59);
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
            this.btnLessArea.Image = null;
            this.btnLessArea.Corner = BeeGlobal.Corner.Left;
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
            this.btnLessArea.Size = new System.Drawing.Size(100, 59);
            this.btnLessArea.TabIndex = 31;
            this.btnLessArea.Text = "Less";
            this.btnLessArea.TextColor = System.Drawing.Color.Black;
            this.btnLessArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLessArea.UseVisualStyleBackColor = false;
            // 
            // numLimtArea
            // 
            this.numLimtArea.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numLimtArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numLimtArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numLimtArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numLimtArea.Location = new System.Drawing.Point(200, 0);
            this.numLimtArea.Margin = new System.Windows.Forms.Padding(0);
            this.numLimtArea.Maxnimum = 2000F;
            this.numLimtArea.Minimum = 0F;
            this.numLimtArea.Name = "numLimtArea";
            this.numLimtArea.Size = new System.Drawing.Size(296, 59);
            this.numLimtArea.Step = 1F;
            this.numLimtArea.TabIndex = 34;
            this.numLimtArea.Value = 500F;
            this.numLimtArea.ValueChanged += new System.EventHandler(this.numLimtArea_ValueChanged);
            this.numLimtArea.Load += new System.EventHandler(this.numLimtArea_Load);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel5.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnEnLimitArea, 2, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(11, 264);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(11, 18, 4, 4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(502, 48);
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
            this.label3.Size = new System.Drawing.Size(186, 48);
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
            this.btnEnLimitArea.Image = null;
            this.btnEnLimitArea.Corner = BeeGlobal.Corner.None;
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
            this.btnEnLimitArea.Location = new System.Drawing.Point(349, 4);
            this.btnEnLimitArea.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEnLimitArea.Name = "btnEnLimitArea";
            this.btnEnLimitArea.Size = new System.Drawing.Size(149, 40);
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
            this.tableLayoutPanel10.Location = new System.Drawing.Point(13, 178);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(13, 6, 7, 6);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(497, 62);
            this.tableLayoutPanel10.TabIndex = 49;
            // 
            // numBlur
            // 
            this.numBlur.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numBlur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numBlur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numBlur.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBlur.Location = new System.Drawing.Point(248, 0);
            this.numBlur.Margin = new System.Windows.Forms.Padding(0);
            this.numBlur.Maxnimum = 100F;
            this.numBlur.Minimum = 0F;
            this.numBlur.Name = "numBlur";
            this.numBlur.Size = new System.Drawing.Size(249, 62);
            this.numBlur.Step = 1F;
            this.numBlur.TabIndex = 45;
            this.numBlur.Value = 3F;
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
            this.label7.Size = new System.Drawing.Size(248, 62);
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
            this.label6.Size = new System.Drawing.Size(517, 47);
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
            this.tableLayoutPanel4.Location = new System.Drawing.Point(13, 119);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(13, 6, 7, 6);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(497, 47);
            this.tableLayoutPanel4.TabIndex = 47;
            // 
            // numUnsharp
            // 
            this.numUnsharp.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numUnsharp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numUnsharp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numUnsharp.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUnsharp.Location = new System.Drawing.Point(248, 0);
            this.numUnsharp.Margin = new System.Windows.Forms.Padding(0);
            this.numUnsharp.Maxnimum = 100F;
            this.numUnsharp.Minimum = 0F;
            this.numUnsharp.Name = "numUnsharp";
            this.numUnsharp.Size = new System.Drawing.Size(249, 47);
            this.numUnsharp.Step = 1F;
            this.numUnsharp.TabIndex = 45;
            this.numUnsharp.Value = 3F;
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
            this.label2.Size = new System.Drawing.Size(248, 47);
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
            this.tableLayoutPanel9.Location = new System.Drawing.Point(13, 51);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(13, 4, 4, 4);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(500, 58);
            this.tableLayoutPanel9.TabIndex = 45;
            // 
            // numCLAHE
            // 
            this.numCLAHE.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numCLAHE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numCLAHE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numCLAHE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCLAHE.Location = new System.Drawing.Point(250, 0);
            this.numCLAHE.Margin = new System.Windows.Forms.Padding(0);
            this.numCLAHE.Maxnimum = 100F;
            this.numCLAHE.Minimum = 0F;
            this.numCLAHE.Name = "numCLAHE";
            this.numCLAHE.Size = new System.Drawing.Size(250, 58);
            this.numCLAHE.Step = 1F;
            this.numCLAHE.TabIndex = 45;
            this.numCLAHE.Value = 2F;
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
            this.label1.Size = new System.Drawing.Size(250, 58);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ToolOCR";
            this.Size = new System.Drawing.Size(533, 1030);
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
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnOK;
        private RJButton btnCancel;
        private RJButton btnTest;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Timer tmCheckFist;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnSet;
        private System.Windows.Forms.TextBox txtContent;
        private System.ComponentModel.BackgroundWorker workLoadModel;
        private System.Windows.Forms.TableLayoutPanel tabLabelResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private CustomNumeric numUnsharp;
        private System.Windows.Forms.Label label2;
        private CustomNumeric numCLAHE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private CustomNumeric numBlur;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label3;
        private RJButton btnEnLimitArea;
        private System.Windows.Forms.TableLayoutPanel layoutLineLimit;
        private RJButton rjButton1;
        private RJButton btnMoreArea;
        private RJButton btnLessArea;
        private CustomNumeric numLimtArea;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Label label4;
        private RJButton btnReadThresh;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private RJButton btnApply;
        private System.Windows.Forms.TextBox txtAllow;
        private System.Windows.Forms.Label label10;
        private AdjustBar trackScore;
    }
}
