
using BeeGlobal;
using BeeInterface;

namespace BeeUi
{
    partial class ScanCCD
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

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanCCD));
            this.label2 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.work = new System.ComponentModel.BackgroundWorker();
            this.pCamera = new DbTableLayoutPanel();
            this.btnCamera4 = new BeeInterface.RJButton();
            this.btnCamera1 = new BeeInterface.RJButton();
            this.btnCamera2 = new BeeInterface.RJButton();
            this.btnCamera3 = new BeeInterface.RJButton();
            this.workConAll = new System.ComponentModel.BackgroundWorker();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.cbReSolution = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnShowList = new System.Windows.Forms.Button();
            this.cbCCD = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new DbTableLayoutPanel();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new DbTableLayoutPanel();
            this.btnUSB2_0 = new BeeInterface.RJButton();
            this.btnCameraTiny = new BeeInterface.RJButton();
            this.btnGigE = new BeeInterface.RJButton();
            this.tableLayoutPanel3 = new DbTableLayoutPanel();
            this.tableLayoutPanel4 = new DbTableLayoutPanel();
            this.tableLayoutPanel5 = new DbTableLayoutPanel();
            this.pCamera.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(394, 44);
            this.label2.TabIndex = 7;
            this.label2.Text = "Setup Camera";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(176, 5);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(161, 32);
            this.btnConnect.TabIndex = 33;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnAreaBlack_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnClose.BackgroundImage = global::BeeUi.Properties.Resources.Delete;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnClose.Location = new System.Drawing.Point(403, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(53, 36);
            this.btnClose.TabIndex = 38;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            this.openFile.Filter = "KeyAcitve|*.bee";
            // 
            // work
            // 
            this.work.DoWork += new System.ComponentModel.DoWorkEventHandler(this.work_DoWork);
            this.work.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.work_RunWorkerCompleted);
            // 
            // pCamera
            // 
            this.pCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.pCamera.ColumnCount = 5;
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pCamera.Controls.Add(this.btnCamera4, 4, 0);
            this.pCamera.Controls.Add(this.btnCamera1, 1, 0);
            this.pCamera.Controls.Add(this.btnCamera2, 2, 0);
            this.pCamera.Controls.Add(this.btnCamera3, 3, 0);
            this.pCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pCamera.Location = new System.Drawing.Point(5, 55);
            this.pCamera.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.pCamera.Name = "pCamera";
            this.pCamera.RowCount = 1;
            this.pCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pCamera.Size = new System.Drawing.Size(460, 53);
            this.pCamera.TabIndex = 32;
            // 
            // btnCamera4
            // 
            this.btnCamera4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera4.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera4.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCamera4.BorderRadius = 10;
            this.btnCamera4.BorderSize = 0;
            this.btnCamera4.Corner = BeeGlobal.Corner.Right;
            this.btnCamera4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera4.FlatAppearance.BorderSize = 0;
            this.btnCamera4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera4.ForeColor = System.Drawing.Color.Black;
            this.btnCamera4.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera4.Image")));
            this.btnCamera4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera4.IsCLick = false;
            this.btnCamera4.IsNotChange = false;
            this.btnCamera4.IsRect = true;
            this.btnCamera4.IsUnGroup = false;
            this.btnCamera4.Location = new System.Drawing.Point(371, 0);
            this.btnCamera4.Margin = new System.Windows.Forms.Padding(0);
            this.btnCamera4.Name = "btnCamera4";
            this.btnCamera4.Size = new System.Drawing.Size(89, 53);
            this.btnCamera4.TabIndex = 31;
            this.btnCamera4.Text = "Camera 4";
            this.btnCamera4.TextColor = System.Drawing.Color.Black;
            this.btnCamera4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera4.UseVisualStyleBackColor = false;
            this.btnCamera4.Click += new System.EventHandler(this.btnCamera4_Click);
            // 
            // btnCamera1
            // 
            this.btnCamera1.BackColor = System.Drawing.Color.Transparent;
            this.btnCamera1.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCamera1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera1.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCamera1.BorderRadius = 10;
            this.btnCamera1.BorderSize = 0;
            this.btnCamera1.Corner = BeeGlobal.Corner.Left;
            this.btnCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera1.FlatAppearance.BorderSize = 0;
            this.btnCamera1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera1.ForeColor = System.Drawing.Color.Black;
            this.btnCamera1.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera1.Image")));
            this.btnCamera1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera1.IsCLick = true;
            this.btnCamera1.IsNotChange = false;
            this.btnCamera1.IsRect = true;
            this.btnCamera1.IsUnGroup = false;
            this.btnCamera1.Location = new System.Drawing.Point(110, 0);
            this.btnCamera1.Margin = new System.Windows.Forms.Padding(0);
            this.btnCamera1.Name = "btnCamera1";
            this.btnCamera1.Size = new System.Drawing.Size(87, 53);
            this.btnCamera1.TabIndex = 20;
            this.btnCamera1.Text = "Camera 1";
            this.btnCamera1.TextColor = System.Drawing.Color.Black;
            this.btnCamera1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera1.UseVisualStyleBackColor = false;
            this.btnCamera1.Click += new System.EventHandler(this.btnCamera1_Click);
            // 
            // btnCamera2
            // 
            this.btnCamera2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera2.BorderColor = System.Drawing.Color.Transparent;
            this.btnCamera2.BorderRadius = 5;
            this.btnCamera2.BorderSize = 0;
            this.btnCamera2.Corner = BeeGlobal.Corner.None;
            this.btnCamera2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera2.FlatAppearance.BorderSize = 0;
            this.btnCamera2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera2.ForeColor = System.Drawing.Color.Black;
            this.btnCamera2.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera2.Image")));
            this.btnCamera2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera2.IsCLick = false;
            this.btnCamera2.IsNotChange = false;
            this.btnCamera2.IsRect = true;
            this.btnCamera2.IsUnGroup = false;
            this.btnCamera2.Location = new System.Drawing.Point(197, 0);
            this.btnCamera2.Margin = new System.Windows.Forms.Padding(0);
            this.btnCamera2.Name = "btnCamera2";
            this.btnCamera2.Size = new System.Drawing.Size(87, 53);
            this.btnCamera2.TabIndex = 19;
            this.btnCamera2.Text = "Camera 2";
            this.btnCamera2.TextColor = System.Drawing.Color.Black;
            this.btnCamera2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera2.UseVisualStyleBackColor = false;
            this.btnCamera2.Click += new System.EventHandler(this.btnCamera2_Click);
            // 
            // btnCamera3
            // 
            this.btnCamera3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera3.BorderColor = System.Drawing.Color.Transparent;
            this.btnCamera3.BorderRadius = 5;
            this.btnCamera3.BorderSize = 0;
            this.btnCamera3.Corner = BeeGlobal.Corner.None;
            this.btnCamera3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera3.FlatAppearance.BorderSize = 0;
            this.btnCamera3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera3.ForeColor = System.Drawing.Color.Black;
            this.btnCamera3.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera3.Image")));
            this.btnCamera3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera3.IsCLick = false;
            this.btnCamera3.IsNotChange = false;
            this.btnCamera3.IsRect = true;
            this.btnCamera3.IsUnGroup = false;
            this.btnCamera3.Location = new System.Drawing.Point(284, 0);
            this.btnCamera3.Margin = new System.Windows.Forms.Padding(0);
            this.btnCamera3.Name = "btnCamera3";
            this.btnCamera3.Size = new System.Drawing.Size(87, 53);
            this.btnCamera3.TabIndex = 29;
            this.btnCamera3.Text = "Camera 3";
            this.btnCamera3.TextColor = System.Drawing.Color.Black;
            this.btnCamera3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera3.UseVisualStyleBackColor = false;
            this.btnCamera3.Click += new System.EventHandler(this.btnCamera3_Click);
            // 
            // workConAll
            // 
            this.workConAll.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workConAll_DoWork);
            this.workConAll.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workConAll_RunWorkerCompleted);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(114, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 195);
            this.panel4.TabIndex = 49;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button1);
            this.panel5.Controls.Add(this.cbReSolution);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 100);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.panel5.Size = new System.Drawing.Size(342, 48);
            this.panel5.TabIndex = 49;
            this.panel5.Visible = false;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::BeeUi.Properties.Resources.Down_Button;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(289, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 38);
            this.button1.TabIndex = 55;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbReSolution
            // 
            this.cbReSolution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbReSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbReSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbReSolution.FormattingEnabled = true;
            this.cbReSolution.Items.AddRange(new object[] {
            "320x240 (0.08 MP)",
            "640x480 (0.3 MP)",
            "800x600 (0,5 MP)",
            "1024x768 (0.8 MP)",
            "1280x720 (1.3 MP)",
            "1600x1200 (1.9 MP)",
            "1920x1080 (2.1 MP)",
            "2048x1536 (3.1 MP)",
            "2592x1944 (5 MP)",
            "3840x2748 (10MP)",
            "7300 x5475 (40MP)"});
            this.cbReSolution.Location = new System.Drawing.Point(20, 5);
            this.cbReSolution.Name = "cbReSolution";
            this.cbReSolution.Size = new System.Drawing.Size(317, 32);
            this.cbReSolution.TabIndex = 42;
            this.cbReSolution.SelectedIndexChanged += new System.EventHandler(this.cbReSolution_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 25);
            this.label1.TabIndex = 41;
            this.label1.Text = "Card";
            this.label1.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnShowList);
            this.panel3.Controls.Add(this.cbCCD);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 27);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.panel3.Size = new System.Drawing.Size(342, 48);
            this.panel3.TabIndex = 48;
            // 
            // btnShowList
            // 
            this.btnShowList.BackgroundImage = global::BeeUi.Properties.Resources.Down_Button;
            this.btnShowList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnShowList.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShowList.Location = new System.Drawing.Point(289, 5);
            this.btnShowList.Name = "btnShowList";
            this.btnShowList.Size = new System.Drawing.Size(48, 38);
            this.btnShowList.TabIndex = 54;
            this.btnShowList.UseVisualStyleBackColor = true;
            this.btnShowList.Click += new System.EventHandler(this.btnShowList_Click);
            // 
            // cbCCD
            // 
            this.cbCCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbCCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCCD.FormattingEnabled = true;
            this.cbCCD.Location = new System.Drawing.Point(20, 5);
            this.cbCCD.Name = "cbCCD";
            this.cbCCD.Size = new System.Drawing.Size(317, 32);
            this.cbCCD.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(342, 27);
            this.label3.TabIndex = 32;
            this.label3.Text = "Camera";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnDisConnect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnConnect, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 153);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(342, 42);
            this.tableLayoutPanel1.TabIndex = 52;
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.BackColor = System.Drawing.Color.Gray;
            this.btnDisConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDisConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDisConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisConnect.ForeColor = System.Drawing.Color.White;
            this.btnDisConnect.Location = new System.Drawing.Point(5, 5);
            this.btnDisConnect.Margin = new System.Windows.Forms.Padding(5);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(161, 32);
            this.btnDisConnect.TabIndex = 34;
            this.btnDisConnect.Text = "DisConnect";
            this.btnDisConnect.UseVisualStyleBackColor = false;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnUSB2_0, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCameraTiny, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnGigE, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(105, 195);
            this.tableLayoutPanel2.TabIndex = 50;
            // 
            // btnUSB2_0
            // 
            this.btnUSB2_0.BackColor = System.Drawing.Color.Transparent;
            this.btnUSB2_0.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnUSB2_0.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUSB2_0.BackgroundImage")));
            this.btnUSB2_0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUSB2_0.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUSB2_0.BorderRadius = 5;
            this.btnUSB2_0.BorderSize = 0;
            this.btnUSB2_0.Corner = BeeGlobal.Corner.Both;
            this.btnUSB2_0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUSB2_0.FlatAppearance.BorderSize = 0;
            this.btnUSB2_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUSB2_0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUSB2_0.ForeColor = System.Drawing.Color.Black;
            this.btnUSB2_0.Image = global::BeeUi.Properties.Resources.CameraNotConnect;
            this.btnUSB2_0.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUSB2_0.IsCLick = true;
            this.btnUSB2_0.IsNotChange = false;
            this.btnUSB2_0.IsRect = true;
            this.btnUSB2_0.IsUnGroup = false;
            this.btnUSB2_0.Location = new System.Drawing.Point(5, 5);
            this.btnUSB2_0.Margin = new System.Windows.Forms.Padding(5);
            this.btnUSB2_0.Name = "btnUSB2_0";
            this.btnUSB2_0.Size = new System.Drawing.Size(95, 55);
            this.btnUSB2_0.TabIndex = 47;
            this.btnUSB2_0.Text = "USB 2.0";
            this.btnUSB2_0.TextColor = System.Drawing.Color.Black;
            this.btnUSB2_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnUSB2_0.UseVisualStyleBackColor = false;
            this.btnUSB2_0.Click += new System.EventHandler(this.btnUSB2_0_Click);
            // 
            // btnCameraTiny
            // 
            this.btnCameraTiny.BackColor = System.Drawing.Color.Transparent;
            this.btnCameraTiny.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCameraTiny.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCameraTiny.BackgroundImage")));
            this.btnCameraTiny.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCameraTiny.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCameraTiny.BorderRadius = 5;
            this.btnCameraTiny.BorderSize = 0;
            this.btnCameraTiny.Corner = BeeGlobal.Corner.Both;
            this.btnCameraTiny.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCameraTiny.FlatAppearance.BorderSize = 0;
            this.btnCameraTiny.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCameraTiny.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCameraTiny.ForeColor = System.Drawing.Color.Black;
            this.btnCameraTiny.Image = global::BeeUi.Properties.Resources.CameraConnected;
            this.btnCameraTiny.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCameraTiny.IsCLick = false;
            this.btnCameraTiny.IsNotChange = false;
            this.btnCameraTiny.IsRect = true;
            this.btnCameraTiny.IsUnGroup = false;
            this.btnCameraTiny.Location = new System.Drawing.Point(5, 135);
            this.btnCameraTiny.Margin = new System.Windows.Forms.Padding(5);
            this.btnCameraTiny.Name = "btnCameraTiny";
            this.btnCameraTiny.Size = new System.Drawing.Size(95, 55);
            this.btnCameraTiny.TabIndex = 49;
            this.btnCameraTiny.Text = "Tiny";
            this.btnCameraTiny.TextColor = System.Drawing.Color.Black;
            this.btnCameraTiny.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCameraTiny.UseVisualStyleBackColor = false;
            this.btnCameraTiny.Click += new System.EventHandler(this.btnCameraTiny_Click);
            // 
            // btnGigE
            // 
            this.btnGigE.BackColor = System.Drawing.Color.Transparent;
            this.btnGigE.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnGigE.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGigE.BackgroundImage")));
            this.btnGigE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGigE.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGigE.BorderRadius = 5;
            this.btnGigE.BorderSize = 0;
            this.btnGigE.Corner = BeeGlobal.Corner.Both;
            this.btnGigE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGigE.FlatAppearance.BorderSize = 0;
            this.btnGigE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGigE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGigE.ForeColor = System.Drawing.Color.Black;
            this.btnGigE.Image = global::BeeUi.Properties.Resources.Camera1;
            this.btnGigE.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGigE.IsCLick = false;
            this.btnGigE.IsNotChange = false;
            this.btnGigE.IsRect = true;
            this.btnGigE.IsUnGroup = false;
            this.btnGigE.Location = new System.Drawing.Point(5, 70);
            this.btnGigE.Margin = new System.Windows.Forms.Padding(5);
            this.btnGigE.Name = "btnGigE";
            this.btnGigE.Size = new System.Drawing.Size(95, 55);
            this.btnGigE.TabIndex = 48;
            this.btnGigE.Text = "GigE|3.0";
            this.btnGigE.TextColor = System.Drawing.Color.Black;
            this.btnGigE.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnGigE.UseVisualStyleBackColor = false;
            this.btnGigE.Click += new System.EventHandler(this.btnGigE_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.pCamera, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(465, 320);
            this.tableLayoutPanel3.TabIndex = 51;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(459, 44);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.35597F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.64403F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 116);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(459, 201);
            this.tableLayoutPanel5.TabIndex = 33;
            // 
            // ScanCCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.ClientSize = new System.Drawing.Size(475, 330);
            this.Controls.Add(this.tableLayoutPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScanCCD";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "FormActive";
            this.Load += new System.EventHandler(this.ScanCCD_Load);
            this.pCamera.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

            }

            #endregion
            public System.Windows.Forms.Label label2;
            private System.Windows.Forms.Button btnConnect;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.Button btnClose;
            private System.Windows.Forms.OpenFileDialog openFile;
        private System.ComponentModel.BackgroundWorker work;
        private RJButton btnSimulation;
        public System.Windows.Forms.ComboBox cbCCD;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnShowList;
        private DbTableLayoutPanel pCamera;
        public RJButton btnGigE;
        public RJButton btnUSB2_0;
        public RJButton btnCameraTiny;
        public RJButton btnCamera4;
        public RJButton btnCamera1;
        public RJButton btnCamera2;
        public RJButton btnCamera3;
        private DbTableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ComboBox cbReSolution;
        private DbTableLayoutPanel tableLayoutPanel2;
        public System.ComponentModel.BackgroundWorker workConAll;
        private DbTableLayoutPanel tableLayoutPanel3;
        private DbTableLayoutPanel tableLayoutPanel4;
        private DbTableLayoutPanel tableLayoutPanel5;
    }
    }