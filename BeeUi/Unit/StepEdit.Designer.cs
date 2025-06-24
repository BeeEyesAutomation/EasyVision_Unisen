using BeeCore;

namespace BeeUi.Common
{
    partial class StepEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StepEdit));
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.workConnect = new System.ComponentModel.BackgroundWorker();
            this.btnSaveProgram = new BeeUi.Common.RJButton();
            this.btnStep4 = new BeeUi.Common.RJButton();
            this.btnStep3 = new BeeUi.Common.RJButton();
            this.btnStep2 = new BeeUi.Common.RJButton();
            this.btnStep1 = new BeeUi.Common.RJButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Image = global::BeeUi.Properties.Resources.Hide;
            this.pictureBox3.Location = new System.Drawing.Point(608, 31);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(29, 37);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::BeeUi.Properties.Resources.Hide;
            this.pictureBox2.Location = new System.Drawing.Point(396, 31);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(29, 37);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::BeeUi.Properties.Resources.Hide;
            this.pictureBox1.Location = new System.Drawing.Point(193, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(29, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // workConnect
            // 
            this.workConnect.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workConnect_DoWork);
            this.workConnect.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workConnect_RunWorkerCompleted);
            // 
            // btnSaveProgram
            // 
            this.btnSaveProgram.BackColor = System.Drawing.Color.White;
            this.btnSaveProgram.BackgroundColor = System.Drawing.Color.White;
            this.btnSaveProgram.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveProgram.BackgroundImage")));
            this.btnSaveProgram.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveProgram.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveProgram.BorderRadius = 20;
            this.btnSaveProgram.BorderSize = 2;
            this.btnSaveProgram.ButtonImage = null;
            this.btnSaveProgram.Corner = BeeCore.Corner.Both;
            this.btnSaveProgram.FlatAppearance.BorderSize = 0;
            this.btnSaveProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveProgram.ForeColor = System.Drawing.Color.Black;
            this.btnSaveProgram.Image = global::BeeUi.Properties.Resources.Save_1;
            this.btnSaveProgram.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveProgram.IsCLick = false;
            this.btnSaveProgram.IsNotChange = false;
            this.btnSaveProgram.IsRect = true;
            this.btnSaveProgram.IsUnGroup = false;
            this.btnSaveProgram.Location = new System.Drawing.Point(1096, 17);
            this.btnSaveProgram.Name = "btnSaveProgram";
            this.btnSaveProgram.Size = new System.Drawing.Size(207, 88);
            this.btnSaveProgram.TabIndex = 10;
            this.btnSaveProgram.Text = "Save Program";
            this.btnSaveProgram.TextColor = System.Drawing.Color.Black;
            this.btnSaveProgram.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveProgram.UseVisualStyleBackColor = false;
            this.btnSaveProgram.Visible = false;
            this.btnSaveProgram.Click += new System.EventHandler(this.btnSaveProgram_Click);
            // 
            // btnStep4
            // 
            this.btnStep4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStep4.BackColor = System.Drawing.Color.White;
            this.btnStep4.BackgroundColor = System.Drawing.Color.White;
            this.btnStep4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStep4.BackgroundImage")));
            this.btnStep4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStep4.BorderColor = System.Drawing.Color.Silver;
            this.btnStep4.BorderRadius = 20;
            this.btnStep4.BorderSize = 2;
            this.btnStep4.ButtonImage = null;
            this.btnStep4.Corner = BeeCore.Corner.Both;
            this.btnStep4.FlatAppearance.BorderSize = 0;
            this.btnStep4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.btnStep4.ForeColor = System.Drawing.Color.Black;
            this.btnStep4.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStep4.IsCLick = false;
            this.btnStep4.IsNotChange = false;
            this.btnStep4.IsRect = true;
            this.btnStep4.IsUnGroup = false;
            this.btnStep4.Location = new System.Drawing.Point(643, 7);
            this.btnStep4.Name = "btnStep4";
            this.btnStep4.Size = new System.Drawing.Size(162, 92);
            this.btnStep4.TabIndex = 9;
            this.btnStep4.Text = "4.Output\r\nAssignment";
            this.btnStep4.TextColor = System.Drawing.Color.Black;
            this.btnStep4.UseVisualStyleBackColor = false;
            this.btnStep4.Click += new System.EventHandler(this.btnStep4_Click);
            // 
            // btnStep3
            // 
            this.btnStep3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStep3.BackColor = System.Drawing.Color.White;
            this.btnStep3.BackgroundColor = System.Drawing.Color.White;
            this.btnStep3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStep3.BackgroundImage")));
            this.btnStep3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStep3.BorderColor = System.Drawing.Color.Silver;
            this.btnStep3.BorderRadius = 20;
            this.btnStep3.BorderSize = 2;
            this.btnStep3.ButtonImage = null;
            this.btnStep3.Corner = BeeCore.Corner.Both;
            this.btnStep3.FlatAppearance.BorderSize = 0;
            this.btnStep3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.btnStep3.ForeColor = System.Drawing.Color.Black;
            this.btnStep3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStep3.IsCLick = false;
            this.btnStep3.IsNotChange = false;
            this.btnStep3.IsRect = true;
            this.btnStep3.IsUnGroup = false;
            this.btnStep3.Location = new System.Drawing.Point(431, 7);
            this.btnStep3.Name = "btnStep3";
            this.btnStep3.Size = new System.Drawing.Size(162, 92);
            this.btnStep3.TabIndex = 8;
            this.btnStep3.Text = "3.Tool\r\nSettings";
            this.btnStep3.TextColor = System.Drawing.Color.Black;
            this.btnStep3.UseVisualStyleBackColor = false;
            this.btnStep3.Click += new System.EventHandler(this.btnStep3_Click);
            // 
            // btnStep2
            // 
            this.btnStep2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStep2.BackColor = System.Drawing.Color.White;
            this.btnStep2.BackgroundColor = System.Drawing.Color.White;
            this.btnStep2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStep2.BackgroundImage")));
            this.btnStep2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStep2.BorderColor = System.Drawing.Color.Silver;
            this.btnStep2.BorderRadius = 20;
            this.btnStep2.BorderSize = 2;
            this.btnStep2.ButtonImage = null;
            this.btnStep2.Corner = BeeCore.Corner.Both;
            this.btnStep2.FlatAppearance.BorderSize = 0;
            this.btnStep2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.btnStep2.ForeColor = System.Drawing.Color.Black;
            this.btnStep2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStep2.IsCLick = false;
            this.btnStep2.IsNotChange = false;
            this.btnStep2.IsRect = true;
            this.btnStep2.IsUnGroup = false;
            this.btnStep2.Location = new System.Drawing.Point(228, 7);
            this.btnStep2.Name = "btnStep2";
            this.btnStep2.Size = new System.Drawing.Size(162, 92);
            this.btnStep2.TabIndex = 7;
            this.btnStep2.Text = "2.Master \r\nResgistration";
            this.btnStep2.TextColor = System.Drawing.Color.Black;
            this.btnStep2.UseVisualStyleBackColor = false;
            this.btnStep2.Click += new System.EventHandler(this.btnStep2_Click);
            // 
            // btnStep1
            // 
            this.btnStep1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStep1.BackColor = System.Drawing.Color.White;
            this.btnStep1.BackgroundColor = System.Drawing.Color.White;
            this.btnStep1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStep1.BackgroundImage")));
            this.btnStep1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStep1.BorderColor = System.Drawing.Color.Silver;
            this.btnStep1.BorderRadius = 20;
            this.btnStep1.BorderSize = 2;
            this.btnStep1.ButtonImage = null;
            this.btnStep1.Corner = BeeCore.Corner.Both;
            this.btnStep1.FlatAppearance.BorderSize = 0;
            this.btnStep1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.btnStep1.ForeColor = System.Drawing.Color.Black;
            this.btnStep1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStep1.IsCLick = false;
            this.btnStep1.IsNotChange = false;
            this.btnStep1.IsRect = true;
            this.btnStep1.IsUnGroup = false;
            this.btnStep1.Location = new System.Drawing.Point(25, 7);
            this.btnStep1.Name = "btnStep1";
            this.btnStep1.Size = new System.Drawing.Size(162, 92);
            this.btnStep1.TabIndex = 6;
            this.btnStep1.Text = "1.Image \r\nOptimization";
            this.btnStep1.TextColor = System.Drawing.Color.Black;
            this.btnStep1.UseVisualStyleBackColor = false;
            this.btnStep1.Click += new System.EventHandler(this.btnStep1_Click);
            // 
            // StepEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnSaveProgram);
            this.Controls.Add(this.btnStep4);
            this.Controls.Add(this.btnStep3);
            this.Controls.Add(this.btnStep2);
            this.Controls.Add(this.btnStep1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "StepEdit";
            this.Size = new System.Drawing.Size(950, 108);
            this.Load += new System.EventHandler(this.StepEdit_Load);
            this.SizeChanged += new System.EventHandler(this.StepEdit_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.StepEdit_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        public RJButton btnStep1;
        public RJButton btnStep2;
        public RJButton btnStep3;
        public RJButton btnStep4;
        private System.ComponentModel.BackgroundWorker workConnect;
        public RJButton btnSaveProgram;
    }
}
