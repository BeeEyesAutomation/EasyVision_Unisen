
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
            this.panel7 = new System.Windows.Forms.Panel();
            this.roundedPanel1 = new BeeUi.Commons.RoundedPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.cbReSolution = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnShowList = new System.Windows.Forms.Button();
            this.cbCCD = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCameraTiny = new BeeUi.Common.RJButton();
            this.btnGigE = new BeeUi.Common.RJButton();
            this.btnUSB2_0 = new BeeUi.Common.RJButton();
            this.roundedPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(537, 45);
            this.label2.TabIndex = 7;
            this.label2.Text = "Setup Camera";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(5, 327);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(537, 46);
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
            this.btnClose.Location = new System.Drawing.Point(489, 8);
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
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(5, 50);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(537, 20);
            this.panel7.TabIndex = 51;
            // 
            // roundedPanel1
            // 
            this.roundedPanel1.BackColor = System.Drawing.Color.White;
            this.roundedPanel1.BorderColor = System.Drawing.Color.Transparent;
            this.roundedPanel1.BorderRadius = 30;
            this.roundedPanel1.BorderSize = 2;
            this.roundedPanel1.Controls.Add(this.panel4);
            this.roundedPanel1.Controls.Add(this.panel1);
            this.roundedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roundedPanel1.Location = new System.Drawing.Point(5, 70);
            this.roundedPanel1.Name = "roundedPanel1";
            this.roundedPanel1.Size = new System.Drawing.Size(537, 257);
            this.roundedPanel1.TabIndex = 50;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(126, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.panel4.Size = new System.Drawing.Size(411, 257);
            this.panel4.TabIndex = 49;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button2);
            this.panel6.Controls.Add(this.comboBox1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(10, 173);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.panel6.Size = new System.Drawing.Size(391, 48);
            this.panel6.TabIndex = 51;
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::BeeUi.Properties.Resources.Down_Button;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.Location = new System.Drawing.Point(338, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 38);
            this.button2.TabIndex = 55;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "320x240 (0.08 MP)",
            "640x480 (0.3 MP)",
            "800x600 (0,5 MP)",
            "1024x768 (0.8 MP)",
            "1280x720 (1.3 MP)",
            "1600x1200 (1.9 MP)",
            "1920x1080 (2.1 MP)",
            "2048x1536 (3.1 MP)",
            "2592x1944 (5 MP)",
            "3840x2748 (10MP)"});
            this.comboBox1.Location = new System.Drawing.Point(20, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(366, 37);
            this.comboBox1.TabIndex = 42;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(391, 27);
            this.label4.TabIndex = 50;
            this.label4.Text = "Type Color";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button1);
            this.panel5.Controls.Add(this.cbReSolution);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(10, 98);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.panel5.Size = new System.Drawing.Size(391, 48);
            this.panel5.TabIndex = 49;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::BeeUi.Properties.Resources.Down_Button;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(338, 5);
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
            this.cbReSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.cbReSolution.Size = new System.Drawing.Size(366, 37);
            this.cbReSolution.TabIndex = 42;
            this.cbReSolution.SelectedIndexChanged += new System.EventHandler(this.cbReSolution_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(391, 25);
            this.label1.TabIndex = 41;
            this.label1.Text = "Resolution";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnShowList);
            this.panel3.Controls.Add(this.cbCCD);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 25);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.panel3.Size = new System.Drawing.Size(391, 48);
            this.panel3.TabIndex = 48;
            // 
            // btnShowList
            // 
            this.btnShowList.BackgroundImage = global::BeeUi.Properties.Resources.Down_Button;
            this.btnShowList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnShowList.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShowList.Location = new System.Drawing.Point(338, 5);
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
            this.cbCCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCCD.FormattingEnabled = true;
            this.cbCCD.Location = new System.Drawing.Point(20, 5);
            this.cbCCD.Name = "cbCCD";
            this.cbCCD.Size = new System.Drawing.Size(366, 37);
            this.cbCCD.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(391, 25);
            this.label3.TabIndex = 32;
            this.label3.Text = "Camera";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnCameraTiny);
            this.panel1.Controls.Add(this.btnGigE);
            this.panel1.Controls.Add(this.btnUSB2_0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(126, 257);
            this.panel1.TabIndex = 47;
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
            this.btnCameraTiny.ButtonImage = null;
            this.btnCameraTiny.Corner = BeeCore.Corner.Both;
            this.btnCameraTiny.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCameraTiny.FlatAppearance.BorderSize = 0;
            this.btnCameraTiny.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCameraTiny.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCameraTiny.ForeColor = System.Drawing.Color.Black;
            this.btnCameraTiny.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCameraTiny.IsCLick = false;
            this.btnCameraTiny.IsNotChange = false;
            this.btnCameraTiny.IsRect = true;
            this.btnCameraTiny.IsUnGroup = false;
            this.btnCameraTiny.Location = new System.Drawing.Point(2, 136);
            this.btnCameraTiny.Name = "btnCameraTiny";
            this.btnCameraTiny.Size = new System.Drawing.Size(120, 67);
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
            this.btnGigE.ButtonImage = null;
            this.btnGigE.Corner = BeeCore.Corner.Both;
            this.btnGigE.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGigE.FlatAppearance.BorderSize = 0;
            this.btnGigE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGigE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGigE.ForeColor = System.Drawing.Color.Black;
            this.btnGigE.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGigE.IsCLick = false;
            this.btnGigE.IsNotChange = false;
            this.btnGigE.IsRect = true;
            this.btnGigE.IsUnGroup = false;
            this.btnGigE.Location = new System.Drawing.Point(2, 69);
            this.btnGigE.Name = "btnGigE";
            this.btnGigE.Size = new System.Drawing.Size(120, 67);
            this.btnGigE.TabIndex = 48;
            this.btnGigE.Text = "GigE|3.0";
            this.btnGigE.TextColor = System.Drawing.Color.Black;
            this.btnGigE.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnGigE.UseVisualStyleBackColor = false;
            this.btnGigE.Click += new System.EventHandler(this.btnGigE_Click);
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
            this.btnUSB2_0.ButtonImage = null;
            this.btnUSB2_0.Corner = BeeCore.Corner.Both;
            this.btnUSB2_0.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUSB2_0.FlatAppearance.BorderSize = 0;
            this.btnUSB2_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUSB2_0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUSB2_0.ForeColor = System.Drawing.Color.Black;
            this.btnUSB2_0.Image = global::BeeUi.Properties.Resources.Camera;
            this.btnUSB2_0.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUSB2_0.IsCLick = true;
            this.btnUSB2_0.IsNotChange = false;
            this.btnUSB2_0.IsRect = true;
            this.btnUSB2_0.IsUnGroup = false;
            this.btnUSB2_0.Location = new System.Drawing.Point(2, 2);
            this.btnUSB2_0.Name = "btnUSB2_0";
            this.btnUSB2_0.Size = new System.Drawing.Size(120, 67);
            this.btnUSB2_0.TabIndex = 47;
            this.btnUSB2_0.Text = "USB 2.0";
            this.btnUSB2_0.TextColor = System.Drawing.Color.Black;
            this.btnUSB2_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnUSB2_0.UseVisualStyleBackColor = false;
            this.btnUSB2_0.Click += new System.EventHandler(this.btnUSB2_0_Click);
            // 
            // ScanCCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(547, 378);
            this.Controls.Add(this.roundedPanel1);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScanCCD";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "FormActive";
            this.Load += new System.EventHandler(this.ScanCCD_Load);
            this.roundedPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

            }

            #endregion
            public System.Windows.Forms.Label label2;
            private System.Windows.Forms.Button btnConnect;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.Button btnClose;
            private System.Windows.Forms.OpenFileDialog openFile;
        private System.ComponentModel.BackgroundWorker work;
        private Common.RJButton btnSimulation;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbCCD;
        public System.Windows.Forms.ComboBox cbReSolution;
        private System.Windows.Forms.Panel panel1;
        private Common.RJButton btnGigE;
        private Common.RJButton btnUSB2_0;
        private Common.RJButton btnCameraTiny;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnShowList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private Commons.RoundedPanel roundedPanel1;
        private System.Windows.Forms.Panel panel7;
    }
    }