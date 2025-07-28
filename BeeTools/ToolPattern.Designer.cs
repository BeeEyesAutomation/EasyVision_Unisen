using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolPattern
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolPattern));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.btnLearning = new BeeInterface.RJButton();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.btnModeEdge = new BeeInterface.RJButton();
            this.btnModePattern = new BeeInterface.RJButton();
            this.btnModeCany = new BeeInterface.RJButton();
            this.label8 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.imgTemp = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClear = new BeeInterface.RJButton();
            this.btnCropRect = new BeeInterface.RJButton();
            this.btnCropArea = new BeeInterface.RJButton();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNone = new BeeInterface.RJButton();
            this.btnElip = new BeeInterface.RJButton();
            this.btnRect = new BeeInterface.RJButton();
            this.trackScore = new BeeInterface.AdjustBar();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.ckBitwiseNot = new BeeInterface.RJButton();
            this.ckSubPixel = new BeeInterface.RJButton();
            this.ckSIMD = new BeeInterface.RJButton();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHighSpeed = new BeeInterface.RJButton();
            this.btnNormal = new BeeInterface.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackAngle = new BeeInterface.AdjustBar();
            this.trackMaxOverLap = new BeeInterface.AdjustBar();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.layMaximumObj = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackNumObject = new BeeInterface.CustomNumeric();
            this.layLimitCouter = new System.Windows.Forms.TableLayoutPanel();
            this.btnMore = new BeeInterface.RJButton();
            this.btnEqual = new BeeInterface.RJButton();
            this.btnLess = new BeeInterface.RJButton();
            this.numLimitCounter = new BeeInterface.CustomNumeric();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.btnLimitCounter = new BeeInterface.RJButton();
            this.label22 = new System.Windows.Forms.Label();
            this.pCany = new System.Windows.Forms.Panel();
            this.btnAreaBlack = new BeeInterface.RJButton();
            this.btnAreaWhite = new BeeInterface.RJButton();
            this.workLoadModel = new System.ComponentModel.BackgroundWorker();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.layMaximumObj.SuspendLayout();
            this.layLimitCouter.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
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
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(400, 881);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(392, 843);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            this.tabP1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnLearning, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel15, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 837);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(5, 77);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(378, 24);
            this.label13.TabIndex = 58;
            this.label13.Text = "Choose Area";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLearning
            // 
            this.btnLearning.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLearning.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLearning.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLearning.BackgroundImage")));
            this.btnLearning.BorderColor = System.Drawing.Color.Transparent;
            this.btnLearning.BorderRadius = 10;
            this.btnLearning.BorderSize = 1;
            this.btnLearning.ButtonImage = null;
            this.btnLearning.Corner = BeeGlobal.Corner.Both;
            this.btnLearning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLearning.FlatAppearance.BorderSize = 0;
            this.btnLearning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLearning.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLearning.ForeColor = System.Drawing.Color.Black;
            this.btnLearning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLearning.IsCLick = false;
            this.btnLearning.IsNotChange = true;
            this.btnLearning.IsRect = false;
            this.btnLearning.IsUnGroup = true;
            this.btnLearning.Location = new System.Drawing.Point(20, 581);
            this.btnLearning.Margin = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.btnLearning.Name = "btnLearning";
            this.btnLearning.Size = new System.Drawing.Size(361, 35);
            this.btnLearning.TabIndex = 5;
            this.btnLearning.Text = "Train";
            this.btnLearning.TextColor = System.Drawing.Color.Black;
            this.btnLearning.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLearning.UseVisualStyleBackColor = false;
            this.btnLearning.Click += new System.EventHandler(this.btnLearning_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(20, 144);
            this.label12.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(363, 24);
            this.label12.TabIndex = 57;
            this.label12.Text = "Shape";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(5, 216);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(378, 24);
            this.label9.TabIndex = 56;
            this.label9.Text = "Methord";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 3;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel15.Controls.Add(this.btnModeEdge, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnModePattern, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnModeCany, 1, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(5, 240);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(378, 43);
            this.tableLayoutPanel15.TabIndex = 55;
            // 
            // btnModeEdge
            // 
            this.btnModeEdge.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnModeEdge.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnModeEdge.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnModeEdge.BackgroundImage")));
            this.btnModeEdge.BorderColor = System.Drawing.Color.Silver;
            this.btnModeEdge.BorderRadius = 10;
            this.btnModeEdge.BorderSize = 1;
            this.btnModeEdge.ButtonImage = null;
            this.btnModeEdge.Corner = BeeGlobal.Corner.Right;
            this.btnModeEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnModeEdge.FlatAppearance.BorderSize = 0;
            this.btnModeEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModeEdge.ForeColor = System.Drawing.Color.Black;
            this.btnModeEdge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModeEdge.IsCLick = false;
            this.btnModeEdge.IsNotChange = false;
            this.btnModeEdge.IsRect = false;
            this.btnModeEdge.IsUnGroup = false;
            this.btnModeEdge.Location = new System.Drawing.Point(255, 0);
            this.btnModeEdge.Margin = new System.Windows.Forms.Padding(0);
            this.btnModeEdge.Name = "btnModeEdge";
            this.btnModeEdge.Size = new System.Drawing.Size(123, 43);
            this.btnModeEdge.TabIndex = 4;
            this.btnModeEdge.Text = "Edge";
            this.btnModeEdge.TextColor = System.Drawing.Color.Black;
            this.btnModeEdge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModeEdge.UseVisualStyleBackColor = false;
            this.btnModeEdge.Click += new System.EventHandler(this.btnModeEdge_Click);
            // 
            // btnModePattern
            // 
            this.btnModePattern.BackColor = System.Drawing.Color.Transparent;
            this.btnModePattern.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnModePattern.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnModePattern.BackgroundImage")));
            this.btnModePattern.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnModePattern.BorderColor = System.Drawing.Color.Transparent;
            this.btnModePattern.BorderRadius = 10;
            this.btnModePattern.BorderSize = 1;
            this.btnModePattern.ButtonImage = null;
            this.btnModePattern.Corner = BeeGlobal.Corner.Left;
            this.btnModePattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnModePattern.FlatAppearance.BorderSize = 0;
            this.btnModePattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModePattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModePattern.ForeColor = System.Drawing.Color.Black;
            this.btnModePattern.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModePattern.IsCLick = true;
            this.btnModePattern.IsNotChange = false;
            this.btnModePattern.IsRect = false;
            this.btnModePattern.IsUnGroup = false;
            this.btnModePattern.Location = new System.Drawing.Point(0, 0);
            this.btnModePattern.Margin = new System.Windows.Forms.Padding(0);
            this.btnModePattern.Name = "btnModePattern";
            this.btnModePattern.Size = new System.Drawing.Size(122, 43);
            this.btnModePattern.TabIndex = 2;
            this.btnModePattern.Text = "Pattern";
            this.btnModePattern.TextColor = System.Drawing.Color.Black;
            this.btnModePattern.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModePattern.UseVisualStyleBackColor = false;
            this.btnModePattern.Click += new System.EventHandler(this.btnModePattern_Click);
            // 
            // btnModeCany
            // 
            this.btnModeCany.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnModeCany.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnModeCany.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnModeCany.BackgroundImage")));
            this.btnModeCany.BorderColor = System.Drawing.Color.Silver;
            this.btnModeCany.BorderRadius = 5;
            this.btnModeCany.BorderSize = 1;
            this.btnModeCany.ButtonImage = null;
            this.btnModeCany.Corner = BeeGlobal.Corner.None;
            this.btnModeCany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnModeCany.FlatAppearance.BorderSize = 0;
            this.btnModeCany.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeCany.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModeCany.ForeColor = System.Drawing.Color.Black;
            this.btnModeCany.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModeCany.IsCLick = false;
            this.btnModeCany.IsNotChange = false;
            this.btnModeCany.IsRect = false;
            this.btnModeCany.IsUnGroup = false;
            this.btnModeCany.Location = new System.Drawing.Point(122, 0);
            this.btnModeCany.Margin = new System.Windows.Forms.Padding(0);
            this.btnModeCany.Name = "btnModeCany";
            this.btnModeCany.Size = new System.Drawing.Size(133, 43);
            this.btnModeCany.TabIndex = 3;
            this.btnModeCany.Text = "Cany";
            this.btnModeCany.TextColor = System.Drawing.Color.Black;
            this.btnModeCany.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModeCany.UseVisualStyleBackColor = false;
            this.btnModeCany.Click += new System.EventHandler(this.btnModeCany_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 626);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
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
            this.btnTest.Location = new System.Drawing.Point(10, 726);
            this.btnTest.Margin = new System.Windows.Forms.Padding(10);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(366, 100);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.imgTemp, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(1, 322);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(1, 5, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(382, 254);
            this.tableLayoutPanel4.TabIndex = 43;
            // 
            // imgTemp
            // 
            this.imgTemp.BackColor = System.Drawing.Color.White;
            this.imgTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgTemp.Location = new System.Drawing.Point(20, 5);
            this.imgTemp.Margin = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.imgTemp.Name = "imgTemp";
            this.imgTemp.Size = new System.Drawing.Size(357, 244);
            this.imgTemp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgTemp.TabIndex = 31;
            this.imgTemp.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 293);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(378, 24);
            this.label10.TabIndex = 42;
            this.label10.Text = "Training Sample";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.Controls.Add(this.btnClear, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropRect, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropArea, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 101);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(378, 43);
            this.tableLayoutPanel3.TabIndex = 41;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClear.BackgroundImage")));
            this.btnClear.BorderColor = System.Drawing.Color.Silver;
            this.btnClear.BorderRadius = 10;
            this.btnClear.BorderSize = 1;
            this.btnClear.ButtonImage = null;
            this.btnClear.Corner = BeeGlobal.Corner.Right;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.IsCLick = false;
            this.btnClear.IsNotChange = false;
            this.btnClear.IsRect = false;
            this.btnClear.IsUnGroup = false;
            this.btnClear.Location = new System.Drawing.Point(255, 0);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(123, 43);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Area Mask";
            this.btnClear.TextColor = System.Drawing.Color.Black;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCropRect
            // 
            this.btnCropRect.BackColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropRect.BackgroundImage")));
            this.btnCropRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropRect.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BorderRadius = 10;
            this.btnCropRect.BorderSize = 1;
            this.btnCropRect.ButtonImage = null;
            this.btnCropRect.Corner = BeeGlobal.Corner.Left;
            this.btnCropRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropRect.FlatAppearance.BorderSize = 0;
            this.btnCropRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropRect.ForeColor = System.Drawing.Color.Black;
            this.btnCropRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropRect.IsCLick = false;
            this.btnCropRect.IsNotChange = false;
            this.btnCropRect.IsRect = false;
            this.btnCropRect.IsUnGroup = false;
            this.btnCropRect.Location = new System.Drawing.Point(0, 0);
            this.btnCropRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropRect.Name = "btnCropRect";
            this.btnCropRect.Size = new System.Drawing.Size(122, 43);
            this.btnCropRect.TabIndex = 2;
            this.btnCropRect.Text = "Area Temp";
            this.btnCropRect.TextColor = System.Drawing.Color.Black;
            this.btnCropRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropRect.UseVisualStyleBackColor = false;
            this.btnCropRect.Click += new System.EventHandler(this.btnCropRect_Click);
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
            this.btnCropArea.Corner = BeeGlobal.Corner.None;
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
            this.btnCropArea.Location = new System.Drawing.Point(122, 0);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(133, 43);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 24);
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 29);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 43);
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
            this.btnCropFull.Size = new System.Drawing.Size(186, 43);
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
            this.btnCropHalt.Size = new System.Drawing.Size(186, 43);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.btnNone, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnElip, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnRect, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(20, 168);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(20, 0, 5, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(361, 43);
            this.tableLayoutPanel5.TabIndex = 54;
            // 
            // btnNone
            // 
            this.btnNone.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNone.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnNone.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNone.BackgroundImage")));
            this.btnNone.BorderColor = System.Drawing.Color.Silver;
            this.btnNone.BorderRadius = 10;
            this.btnNone.BorderSize = 1;
            this.btnNone.ButtonImage = null;
            this.btnNone.Corner = BeeGlobal.Corner.Right;
            this.btnNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNone.FlatAppearance.BorderSize = 0;
            this.btnNone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNone.ForeColor = System.Drawing.Color.Black;
            this.btnNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNone.IsCLick = false;
            this.btnNone.IsNotChange = false;
            this.btnNone.IsRect = false;
            this.btnNone.IsUnGroup = false;
            this.btnNone.Location = new System.Drawing.Point(240, 0);
            this.btnNone.Margin = new System.Windows.Forms.Padding(0);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(121, 43);
            this.btnNone.TabIndex = 5;
            this.btnNone.Text = "None";
            this.btnNone.TextColor = System.Drawing.Color.Black;
            this.btnNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNone.UseVisualStyleBackColor = false;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnElip
            // 
            this.btnElip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnElip.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnElip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnElip.BackgroundImage")));
            this.btnElip.BorderColor = System.Drawing.Color.Silver;
            this.btnElip.BorderRadius = 10;
            this.btnElip.BorderSize = 1;
            this.btnElip.ButtonImage = null;
            this.btnElip.Corner = BeeGlobal.Corner.None;
            this.btnElip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnElip.FlatAppearance.BorderSize = 0;
            this.btnElip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnElip.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnElip.ForeColor = System.Drawing.Color.Black;
            this.btnElip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnElip.IsCLick = false;
            this.btnElip.IsNotChange = false;
            this.btnElip.IsRect = false;
            this.btnElip.IsUnGroup = false;
            this.btnElip.Location = new System.Drawing.Point(120, 0);
            this.btnElip.Margin = new System.Windows.Forms.Padding(0);
            this.btnElip.Name = "btnElip";
            this.btnElip.Size = new System.Drawing.Size(120, 43);
            this.btnElip.TabIndex = 3;
            this.btnElip.Text = "Elip";
            this.btnElip.TextColor = System.Drawing.Color.Black;
            this.btnElip.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnElip.UseVisualStyleBackColor = false;
            this.btnElip.Click += new System.EventHandler(this.btnElip_Click);
            // 
            // btnRect
            // 
            this.btnRect.BackColor = System.Drawing.Color.Transparent;
            this.btnRect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnRect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRect.BackgroundImage")));
            this.btnRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRect.BorderColor = System.Drawing.Color.Transparent;
            this.btnRect.BorderRadius = 10;
            this.btnRect.BorderSize = 1;
            this.btnRect.ButtonImage = null;
            this.btnRect.Corner = BeeGlobal.Corner.Left;
            this.btnRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRect.FlatAppearance.BorderSize = 0;
            this.btnRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRect.ForeColor = System.Drawing.Color.Black;
            this.btnRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRect.IsCLick = true;
            this.btnRect.IsNotChange = false;
            this.btnRect.IsRect = false;
            this.btnRect.IsUnGroup = false;
            this.btnRect.Location = new System.Drawing.Point(0, 0);
            this.btnRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(120, 43);
            this.btnRect.TabIndex = 4;
            this.btnRect.Text = "Rectangle";
            this.btnRect.TextColor = System.Drawing.Color.Black;
            this.btnRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRect.UseVisualStyleBackColor = false;
            this.btnRect.Click += new System.EventHandler(this.btnRect_Click);
            // 
            // trackScore
            // 
            this.trackScore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackScore.Location = new System.Drawing.Point(3, 654);
            this.trackScore.Max = 100F;
            this.trackScore.Min = 0F;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(380, 59);
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 59;
            this.trackScore.Value = 0F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 843);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.trackAngle, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.trackMaxOverLap, 0, 3);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 9;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(386, 837);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 3;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel14.Controls.Add(this.ckBitwiseNot, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.ckSubPixel, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.ckSIMD, 0, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 380);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 5, 3, 10);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel14.Size = new System.Drawing.Size(378, 55);
            this.tableLayoutPanel14.TabIndex = 53;
            // 
            // ckBitwiseNot
            // 
            this.ckBitwiseNot.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ckBitwiseNot.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.ckBitwiseNot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckBitwiseNot.BackgroundImage")));
            this.ckBitwiseNot.BorderColor = System.Drawing.Color.Silver;
            this.ckBitwiseNot.BorderRadius = 10;
            this.ckBitwiseNot.BorderSize = 1;
            this.ckBitwiseNot.ButtonImage = null;
            this.ckBitwiseNot.Corner = BeeGlobal.Corner.Both;
            this.ckBitwiseNot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckBitwiseNot.FlatAppearance.BorderSize = 0;
            this.ckBitwiseNot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckBitwiseNot.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckBitwiseNot.ForeColor = System.Drawing.Color.Black;
            this.ckBitwiseNot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckBitwiseNot.IsCLick = false;
            this.ckBitwiseNot.IsNotChange = false;
            this.ckBitwiseNot.IsRect = false;
            this.ckBitwiseNot.IsUnGroup = true;
            this.ckBitwiseNot.Location = new System.Drawing.Point(260, 3);
            this.ckBitwiseNot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckBitwiseNot.Name = "ckBitwiseNot";
            this.ckBitwiseNot.Size = new System.Drawing.Size(113, 49);
            this.ckBitwiseNot.TabIndex = 3;
            this.ckBitwiseNot.Text = "Not";
            this.ckBitwiseNot.TextColor = System.Drawing.Color.Black;
            this.ckBitwiseNot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckBitwiseNot.UseVisualStyleBackColor = false;
            this.ckBitwiseNot.Click += new System.EventHandler(this.ckBitwiseNot_Click);
            // 
            // ckSubPixel
            // 
            this.ckSubPixel.BackColor = System.Drawing.Color.Transparent;
            this.ckSubPixel.BackgroundColor = System.Drawing.Color.Transparent;
            this.ckSubPixel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckSubPixel.BackgroundImage")));
            this.ckSubPixel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ckSubPixel.BorderColor = System.Drawing.Color.Transparent;
            this.ckSubPixel.BorderRadius = 10;
            this.ckSubPixel.BorderSize = 1;
            this.ckSubPixel.ButtonImage = null;
            this.ckSubPixel.Corner = BeeGlobal.Corner.Both;
            this.ckSubPixel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckSubPixel.FlatAppearance.BorderSize = 0;
            this.ckSubPixel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckSubPixel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckSubPixel.ForeColor = System.Drawing.Color.Black;
            this.ckSubPixel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckSubPixel.IsCLick = true;
            this.ckSubPixel.IsNotChange = false;
            this.ckSubPixel.IsRect = false;
            this.ckSubPixel.IsUnGroup = true;
            this.ckSubPixel.Location = new System.Drawing.Point(127, 3);
            this.ckSubPixel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckSubPixel.Name = "ckSubPixel";
            this.ckSubPixel.Size = new System.Drawing.Size(123, 49);
            this.ckSubPixel.TabIndex = 4;
            this.ckSubPixel.Text = "SubPixel";
            this.ckSubPixel.TextColor = System.Drawing.Color.Black;
            this.ckSubPixel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckSubPixel.UseVisualStyleBackColor = false;
            this.ckSubPixel.Click += new System.EventHandler(this.ckSubPixel_Click);
            // 
            // ckSIMD
            // 
            this.ckSIMD.BackColor = System.Drawing.Color.Transparent;
            this.ckSIMD.BackgroundColor = System.Drawing.Color.Transparent;
            this.ckSIMD.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckSIMD.BackgroundImage")));
            this.ckSIMD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ckSIMD.BorderColor = System.Drawing.Color.Transparent;
            this.ckSIMD.BorderRadius = 10;
            this.ckSIMD.BorderSize = 1;
            this.ckSIMD.ButtonImage = null;
            this.ckSIMD.Corner = BeeGlobal.Corner.Both;
            this.ckSIMD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckSIMD.FlatAppearance.BorderSize = 0;
            this.ckSIMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckSIMD.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckSIMD.ForeColor = System.Drawing.Color.Black;
            this.ckSIMD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckSIMD.IsCLick = true;
            this.ckSIMD.IsNotChange = false;
            this.ckSIMD.IsRect = false;
            this.ckSIMD.IsUnGroup = true;
            this.ckSIMD.Location = new System.Drawing.Point(5, 3);
            this.ckSIMD.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckSIMD.Name = "ckSIMD";
            this.ckSIMD.Size = new System.Drawing.Size(112, 49);
            this.ckSIMD.TabIndex = 2;
            this.ckSIMD.Text = "SIMD";
            this.ckSIMD.TextColor = System.Drawing.Color.Black;
            this.ckSIMD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckSIMD.UseVisualStyleBackColor = false;
            this.ckSIMD.Click += new System.EventHandler(this.ckSIMD_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 350);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(378, 25);
            this.label3.TabIndex = 52;
            this.label3.Text = "Option";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.btnHighSpeed, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.btnNormal, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 265);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 5);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Size = new System.Drawing.Size(378, 60);
            this.tableLayoutPanel13.TabIndex = 51;
            // 
            // btnHighSpeed
            // 
            this.btnHighSpeed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnHighSpeed.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnHighSpeed.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHighSpeed.BackgroundImage")));
            this.btnHighSpeed.BorderColor = System.Drawing.Color.Silver;
            this.btnHighSpeed.BorderRadius = 10;
            this.btnHighSpeed.BorderSize = 1;
            this.btnHighSpeed.ButtonImage = null;
            this.btnHighSpeed.Corner = BeeGlobal.Corner.Right;
            this.btnHighSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHighSpeed.FlatAppearance.BorderSize = 0;
            this.btnHighSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHighSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHighSpeed.ForeColor = System.Drawing.Color.Black;
            this.btnHighSpeed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHighSpeed.IsCLick = false;
            this.btnHighSpeed.IsNotChange = false;
            this.btnHighSpeed.IsRect = false;
            this.btnHighSpeed.IsUnGroup = false;
            this.btnHighSpeed.Location = new System.Drawing.Point(189, 3);
            this.btnHighSpeed.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnHighSpeed.Name = "btnHighSpeed";
            this.btnHighSpeed.Size = new System.Drawing.Size(186, 55);
            this.btnHighSpeed.TabIndex = 3;
            this.btnHighSpeed.Text = "High Speed";
            this.btnHighSpeed.TextColor = System.Drawing.Color.Black;
            this.btnHighSpeed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHighSpeed.UseVisualStyleBackColor = false;
            this.btnHighSpeed.Click += new System.EventHandler(this.btnHighSpeed_Click);
            // 
            // btnNormal
            // 
            this.btnNormal.BackColor = System.Drawing.Color.Transparent;
            this.btnNormal.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnNormal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNormal.BackgroundImage")));
            this.btnNormal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNormal.BorderColor = System.Drawing.Color.Transparent;
            this.btnNormal.BorderRadius = 10;
            this.btnNormal.BorderSize = 1;
            this.btnNormal.ButtonImage = null;
            this.btnNormal.Corner = BeeGlobal.Corner.Left;
            this.btnNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNormal.FlatAppearance.BorderSize = 0;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNormal.ForeColor = System.Drawing.Color.Black;
            this.btnNormal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNormal.IsCLick = true;
            this.btnNormal.IsNotChange = false;
            this.btnNormal.IsRect = false;
            this.btnNormal.IsUnGroup = false;
            this.btnNormal.Location = new System.Drawing.Point(3, 3);
            this.btnNormal.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(186, 55);
            this.btnNormal.TabIndex = 2;
            this.btnNormal.Text = "Normal";
            this.btnNormal.TextColor = System.Drawing.Color.Black;
            this.btnNormal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNormal.UseVisualStyleBackColor = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 235);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 25);
            this.label2.TabIndex = 50;
            this.label2.Text = "Search Algorithm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 116);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 25);
            this.label6.TabIndex = 48;
            this.label6.Text = "OverLap Range";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(378, 25);
            this.label4.TabIndex = 39;
            this.label4.Text = "Angle Range";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackAngle
            // 
            this.trackAngle.Location = new System.Drawing.Point(3, 33);
            this.trackAngle.Max = 100F;
            this.trackAngle.Min = 1F;
            this.trackAngle.Name = "trackAngle";
            this.trackAngle.Size = new System.Drawing.Size(380, 60);
            this.trackAngle.Step = 1F;
            this.trackAngle.TabIndex = 54;
            this.trackAngle.Value = 1F;
            this.trackAngle.ValueChanged += new System.Action<float>(this.trackAngle_ValueChanged);
            // 
            // trackMaxOverLap
            // 
            this.trackMaxOverLap.Location = new System.Drawing.Point(3, 144);
            this.trackMaxOverLap.Max = 100F;
            this.trackMaxOverLap.Min = 0F;
            this.trackMaxOverLap.Name = "trackMaxOverLap";
            this.trackMaxOverLap.Size = new System.Drawing.Size(380, 64);
            this.trackMaxOverLap.Step = 1F;
            this.trackMaxOverLap.TabIndex = 0;
            this.trackMaxOverLap.Value = 1F;
            this.trackMaxOverLap.ValueChanged += new System.Action<float>(this.trackMaxOverLap_ValueChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel16);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(392, 843);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Limit";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 1;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Controls.Add(this.layMaximumObj, 0, 1);
            this.tableLayoutPanel16.Controls.Add(this.layLimitCouter, 0, 3);
            this.tableLayoutPanel16.Controls.Add(this.tableLayoutPanel21, 0, 2);
            this.tableLayoutPanel16.Controls.Add(this.label22, 0, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 17;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel16.Size = new System.Drawing.Size(386, 837);
            this.tableLayoutPanel16.TabIndex = 1;
            // 
            // layMaximumObj
            // 
            this.layMaximumObj.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layMaximumObj.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.layMaximumObj.ColumnCount = 3;
            this.layMaximumObj.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 188F));
            this.layMaximumObj.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.layMaximumObj.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layMaximumObj.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layMaximumObj.Controls.Add(this.label11, 1, 0);
            this.layMaximumObj.Controls.Add(this.label1, 0, 0);
            this.layMaximumObj.Controls.Add(this.trackNumObject, 2, 0);
            this.layMaximumObj.Location = new System.Drawing.Point(10, 34);
            this.layMaximumObj.Margin = new System.Windows.Forms.Padding(10, 5, 5, 0);
            this.layMaximumObj.Name = "layMaximumObj";
            this.layMaximumObj.RowCount = 1;
            this.layMaximumObj.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layMaximumObj.Size = new System.Drawing.Size(371, 42);
            this.layMaximumObj.TabIndex = 44;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(188, 0);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 42);
            this.label11.TabIndex = 46;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 42);
            this.label1.TabIndex = 45;
            this.label1.Text = "Maximum Object";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackNumObject
            // 
            this.trackNumObject.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.trackNumObject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.trackNumObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackNumObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackNumObject.Location = new System.Drawing.Point(214, 0);
            this.trackNumObject.Margin = new System.Windows.Forms.Padding(0);
            this.trackNumObject.Maxnimum = 100F;
            this.trackNumObject.Minimum = 0F;
            this.trackNumObject.Name = "trackNumObject";
            this.trackNumObject.Size = new System.Drawing.Size(157, 42);
            this.trackNumObject.Step = 1F;
            this.trackNumObject.TabIndex = 34;
            this.trackNumObject.Value = 100F;
            this.trackNumObject.ValueChanged += new System.EventHandler(this.trackNumObject_ValueChanged);
            this.trackNumObject.Load += new System.EventHandler(this.trackNumObject_Load);
            // 
            // layLimitCouter
            // 
            this.layLimitCouter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.layLimitCouter.ColumnCount = 4;
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layLimitCouter.Controls.Add(this.btnMore, 2, 0);
            this.layLimitCouter.Controls.Add(this.btnEqual, 1, 0);
            this.layLimitCouter.Controls.Add(this.btnLess, 0, 0);
            this.layLimitCouter.Controls.Add(this.numLimitCounter, 3, 0);
            this.layLimitCouter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layLimitCouter.Location = new System.Drawing.Point(10, 126);
            this.layLimitCouter.Margin = new System.Windows.Forms.Padding(10, 0, 5, 0);
            this.layLimitCouter.Name = "layLimitCouter";
            this.layLimitCouter.Padding = new System.Windows.Forms.Padding(5, 5, 5, 8);
            this.layLimitCouter.RowCount = 1;
            this.layLimitCouter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layLimitCouter.Size = new System.Drawing.Size(371, 73);
            this.layLimitCouter.TabIndex = 48;
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
            this.btnMore.Corner = BeeGlobal.Corner.None;
            this.btnMore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMore.FlatAppearance.BorderSize = 0;
            this.btnMore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMore.ForeColor = System.Drawing.Color.Black;
            this.btnMore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMore.IsCLick = false;
            this.btnMore.IsNotChange = false;
            this.btnMore.IsRect = false;
            this.btnMore.IsUnGroup = false;
            this.btnMore.Location = new System.Drawing.Point(155, 5);
            this.btnMore.Margin = new System.Windows.Forms.Padding(0);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(75, 60);
            this.btnMore.TabIndex = 33;
            this.btnMore.Text = "More";
            this.btnMore.TextColor = System.Drawing.Color.Black;
            this.btnMore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMore.UseVisualStyleBackColor = false;
            this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
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
            this.btnEqual.Corner = BeeGlobal.Corner.None;
            this.btnEqual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEqual.FlatAppearance.BorderSize = 0;
            this.btnEqual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEqual.ForeColor = System.Drawing.Color.Black;
            this.btnEqual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEqual.IsCLick = true;
            this.btnEqual.IsNotChange = false;
            this.btnEqual.IsRect = false;
            this.btnEqual.IsUnGroup = false;
            this.btnEqual.Location = new System.Drawing.Point(80, 5);
            this.btnEqual.Margin = new System.Windows.Forms.Padding(0);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(75, 60);
            this.btnEqual.TabIndex = 32;
            this.btnEqual.Text = "Equal";
            this.btnEqual.TextColor = System.Drawing.Color.Black;
            this.btnEqual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEqual.UseVisualStyleBackColor = false;
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
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
            this.btnLess.Corner = BeeGlobal.Corner.Left;
            this.btnLess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLess.FlatAppearance.BorderSize = 0;
            this.btnLess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLess.ForeColor = System.Drawing.Color.Black;
            this.btnLess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLess.IsCLick = false;
            this.btnLess.IsNotChange = false;
            this.btnLess.IsRect = false;
            this.btnLess.IsUnGroup = false;
            this.btnLess.Location = new System.Drawing.Point(5, 5);
            this.btnLess.Margin = new System.Windows.Forms.Padding(0);
            this.btnLess.Name = "btnLess";
            this.btnLess.Size = new System.Drawing.Size(75, 60);
            this.btnLess.TabIndex = 31;
            this.btnLess.Text = "Less";
            this.btnLess.TextColor = System.Drawing.Color.Black;
            this.btnLess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLess.UseVisualStyleBackColor = false;
            this.btnLess.Click += new System.EventHandler(this.btnLess_Click);
            // 
            // numLimitCounter
            // 
            this.numLimitCounter.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numLimitCounter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numLimitCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numLimitCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numLimitCounter.Location = new System.Drawing.Point(230, 5);
            this.numLimitCounter.Margin = new System.Windows.Forms.Padding(0);
            this.numLimitCounter.Maxnimum = 100F;
            this.numLimitCounter.Minimum = 0F;
            this.numLimitCounter.Name = "numLimitCounter";
            this.numLimitCounter.Size = new System.Drawing.Size(136, 60);
            this.numLimitCounter.Step = 1F;
            this.numLimitCounter.TabIndex = 34;
            this.numLimitCounter.Value = 1F;
            this.numLimitCounter.ValueChanged += new System.EventHandler(this.numLimitCounter_ValueChanged);
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.ColumnCount = 2;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel21.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanel21.Controls.Add(this.btnLimitCounter, 1, 0);
            this.tableLayoutPanel21.Location = new System.Drawing.Point(10, 86);
            this.tableLayoutPanel21.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 1;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(373, 40);
            this.tableLayoutPanel21.TabIndex = 53;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(203, 40);
            this.label14.TabIndex = 45;
            this.label14.Text = "Limit Couter";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLimitCounter
            // 
            this.btnLimitCounter.BackColor = System.Drawing.Color.Transparent;
            this.btnLimitCounter.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnLimitCounter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLimitCounter.BorderColor = System.Drawing.Color.Transparent;
            this.btnLimitCounter.BorderRadius = 1;
            this.btnLimitCounter.BorderSize = 1;
            this.btnLimitCounter.ButtonImage = null;
            this.btnLimitCounter.Corner = BeeGlobal.Corner.None;
            this.btnLimitCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLimitCounter.FlatAppearance.BorderSize = 0;
            this.btnLimitCounter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimitCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimitCounter.ForeColor = System.Drawing.Color.Black;
            this.btnLimitCounter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLimitCounter.IsCLick = false;
            this.btnLimitCounter.IsNotChange = false;
            this.btnLimitCounter.IsRect = false;
            this.btnLimitCounter.IsUnGroup = true;
            this.btnLimitCounter.Location = new System.Drawing.Point(206, 3);
            this.btnLimitCounter.Name = "btnLimitCounter";
            this.btnLimitCounter.Size = new System.Drawing.Size(164, 34);
            this.btnLimitCounter.TabIndex = 2;
            this.btnLimitCounter.Text = "Enable";
            this.btnLimitCounter.TextColor = System.Drawing.Color.Black;
            this.btnLimitCounter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLimitCounter.UseVisualStyleBackColor = false;
            this.btnLimitCounter.Click += new System.EventHandler(this.btnLimitCounter_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(5, 5);
            this.label22.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(131, 24);
            this.label22.TabIndex = 38;
            this.label22.Text = "Search Range";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.btnAreaBlack.Click += new System.EventHandler(this.btnAreaBlack_Click);
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
            this.btnAreaWhite.Click += new System.EventHandler(this.btnAreaWhite_Click);
            // 
            // workLoadModel
            // 
            this.workLoadModel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadModel_DoWork);
            this.workLoadModel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadModel_RunWorkerCompleted);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 887);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(400, 52);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolPattern
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.pCany);
            this.Controls.Add(this.tabControl2);
            this.Name = "ToolPattern";
            this.Size = new System.Drawing.Size(400, 939);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel16.PerformLayout();
            this.layMaximumObj.ResumeLayout(false);
            this.layMaximumObj.PerformLayout();
            this.layLimitCouter.ResumeLayout(false);
            this.tableLayoutPanel21.ResumeLayout(false);
            this.pCany.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private RJButton btnCropArea;
        private RJButton btnCropRect;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TabPage tabPage4;
        private RJButton ckSIMD;
        private RJButton ckBitwiseNot;
        private RJButton ckSubPixel;
        private RJButton btnClear;
        public System.ComponentModel.BackgroundWorker threadProcess;
        private System.Windows.Forms.Panel pCany;
        private RJButton btnAreaBlack;
        private RJButton btnAreaWhite;
        private RJButton btnHighSpeed;
        private RJButton btnNormal;
        private System.Windows.Forms.PictureBox imgTemp;
        private RJButton btnLearning;
        private RJButton btnTest;
        private System.Windows.Forms.Label label7;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel layMaximumObj;
        private CustomNumeric trackNumObject;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel layLimitCouter;
        private RJButton btnMore;
        private RJButton btnEqual;
        private RJButton btnLess;
        private CustomNumeric numLimitCounter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel21;
        private System.Windows.Forms.Label label14;
        private RJButton btnLimitCounter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnElip;
        private RJButton btnRect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private RJButton btnModeEdge;
        private RJButton btnModePattern;
        private RJButton btnModeCany;
        private System.ComponentModel.BackgroundWorker workLoadModel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private RJButton btnNone;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.Label label22;
        private GroupControl.OK_Cancel oK_Cancel1;
        private AdjustBar trackScore;
        private AdjustBar trackAngle;
        private AdjustBar trackMaxOverLap;
    }
}
