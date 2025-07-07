
using BeeGlobal;
using BeeInterface;

namespace BeeUi
{
    partial class IOSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOSetting));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pCany = new System.Windows.Forms.Panel();
            this.btnSmall = new RJButton();
            this.btnNormal = new RJButton();
            this.btnbig = new RJButton();
            this.pArea = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.trackLimitDay = new TrackBar2();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveNG = new RJButton();
            this.btnSaveOK = new RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSaveRS = new RJButton();
            this.btnSaveRaw = new RJButton();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3.SuspendLayout();
            this.pCany.SuspendLayout();
            this.pArea.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.pArea);
            this.tabPage3.Controls.Add(this.pCany);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(423, 432);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "History Data";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pCany
            // 
            this.pCany.Controls.Add(this.btnbig);
            this.pCany.Controls.Add(this.btnNormal);
            this.pCany.Controls.Add(this.btnSmall);
            this.pCany.Location = new System.Drawing.Point(3, 340);
            this.pCany.Name = "pCany";
            this.pCany.Size = new System.Drawing.Size(389, 57);
            this.pCany.TabIndex = 12;
            // 
            // btnSmall
            // 
            this.btnSmall.BackColor = System.Drawing.Color.Transparent;
            this.btnSmall.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnSmall.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSmall.BackgroundImage")));
            this.btnSmall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSmall.BorderColor = System.Drawing.Color.Transparent;
            this.btnSmall.BorderRadius = 5;
            this.btnSmall.BorderSize = 1;
            this.btnSmall.ButtonImage = null;
            this.btnSmall.Corner =Corner.Both;
            this.btnSmall.FlatAppearance.BorderSize = 0;
            this.btnSmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSmall.ForeColor = System.Drawing.Color.Black;
            this.btnSmall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSmall.IsCLick = true;
            this.btnSmall.IsNotChange = false;
            this.btnSmall.IsRect = false;
            this.btnSmall.IsUnGroup = false;
            this.btnSmall.Location = new System.Drawing.Point(1, 8);
            this.btnSmall.Name = "btnSmall";
            this.btnSmall.Size = new System.Drawing.Size(120, 40);
            this.btnSmall.TabIndex = 7;
            this.btnSmall.Text = "Small Size";
            this.btnSmall.TextColor = System.Drawing.Color.Black;
            this.btnSmall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSmall.UseVisualStyleBackColor = false;
            this.btnSmall.Click += new System.EventHandler(this.btnSmall_Click);
            // 
            // btnNormal
            // 
            this.btnNormal.BackColor = System.Drawing.Color.Transparent;
            this.btnNormal.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnNormal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNormal.BackgroundImage")));
            this.btnNormal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNormal.BorderColor = System.Drawing.Color.Transparent;
            this.btnNormal.BorderRadius = 5;
            this.btnNormal.BorderSize = 1;
            this.btnNormal.ButtonImage = null;
            this.btnNormal.Corner =Corner.Both;
            this.btnNormal.FlatAppearance.BorderSize = 0;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormal.ForeColor = System.Drawing.Color.Black;
            this.btnNormal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNormal.IsCLick = false;
            this.btnNormal.IsNotChange = false;
            this.btnNormal.IsRect = false;
            this.btnNormal.IsUnGroup = false;
            this.btnNormal.Location = new System.Drawing.Point(127, 8);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(126, 40);
            this.btnNormal.TabIndex = 10;
            this.btnNormal.Text = "Normal Size";
            this.btnNormal.TextColor = System.Drawing.Color.Black;
            this.btnNormal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNormal.UseVisualStyleBackColor = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnbig
            // 
            this.btnbig.BackColor = System.Drawing.Color.Transparent;
            this.btnbig.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnbig.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnbig.BackgroundImage")));
            this.btnbig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnbig.BorderColor = System.Drawing.Color.Transparent;
            this.btnbig.BorderRadius = 5;
            this.btnbig.BorderSize = 1;
            this.btnbig.ButtonImage = null;
            this.btnbig.Corner =Corner.Both;
            this.btnbig.FlatAppearance.BorderSize = 0;
            this.btnbig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnbig.ForeColor = System.Drawing.Color.Black;
            this.btnbig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnbig.IsCLick = false;
            this.btnbig.IsNotChange = false;
            this.btnbig.IsRect = false;
            this.btnbig.IsUnGroup = false;
            this.btnbig.Location = new System.Drawing.Point(257, 8);
            this.btnbig.Name = "btnbig";
            this.btnbig.Size = new System.Drawing.Size(126, 40);
            this.btnbig.TabIndex = 11;
            this.btnbig.Text = "Big Size";
            this.btnbig.TextColor = System.Drawing.Color.Black;
            this.btnbig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnbig.UseVisualStyleBackColor = false;
            this.btnbig.Click += new System.EventHandler(this.btnbig_Click);
            // 
            // pArea
            // 
            this.pArea.Controls.Add(this.label3);
            this.pArea.Controls.Add(this.trackLimitDay);
            this.pArea.Controls.Add(this.label6);
            this.pArea.Location = new System.Drawing.Point(3, 228);
            this.pArea.Name = "pArea";
            this.pArea.Size = new System.Drawing.Size(389, 76);
            this.pArea.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Limit";
            // 
            // trackLimitDay
            // 
            this.trackLimitDay.ColorTrack = System.Drawing.Color.Gray;
            this.trackLimitDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackLimitDay.Location = new System.Drawing.Point(53, 18);
            this.trackLimitDay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackLimitDay.Max = 60;
            this.trackLimitDay.Min = 1;
            this.trackLimitDay.Name = "trackLimitDay";
            this.trackLimitDay.Size = new System.Drawing.Size(289, 47);
            this.trackLimitDay.Step = 1;
            this.trackLimitDay.TabIndex = 28;
            this.trackLimitDay.Value = 1;
            this.trackLimitDay.ValueScore = 0;
            this.trackLimitDay.ValueChanged +=  new System.Action<float>(this.trackLimitDay_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(346, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 32;
            this.label3.Text = "Day";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(2, 317);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(165, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Compression methord";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Conditions";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveOK);
            this.panel1.Controls.Add(this.btnSaveNG);
            this.panel1.Location = new System.Drawing.Point(6, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 50);
            this.panel1.TabIndex = 31;
            // 
            // btnSaveNG
            // 
            this.btnSaveNG.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveNG.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSaveNG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveNG.BackgroundImage")));
            this.btnSaveNG.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveNG.BorderRadius = 5;
            this.btnSaveNG.BorderSize = 1;
            this.btnSaveNG.ButtonImage = null;
            this.btnSaveNG.Corner =Corner.Both;
            this.btnSaveNG.FlatAppearance.BorderSize = 0;
            this.btnSaveNG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveNG.ForeColor = System.Drawing.Color.Black;
            this.btnSaveNG.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveNG.IsCLick = false;
            this.btnSaveNG.IsNotChange = false;
            this.btnSaveNG.IsRect = false;
            this.btnSaveNG.IsUnGroup = true;
            this.btnSaveNG.Location = new System.Drawing.Point(195, 5);
            this.btnSaveNG.Name = "btnSaveNG";
            this.btnSaveNG.Size = new System.Drawing.Size(188, 40);
            this.btnSaveNG.TabIndex = 9;
            this.btnSaveNG.Text = "NG";
            this.btnSaveNG.TextColor = System.Drawing.Color.Black;
            this.btnSaveNG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveNG.UseVisualStyleBackColor = false;
            this.btnSaveNG.Click += new System.EventHandler(this.btnSaveNG_Click);
            // 
            // btnSaveOK
            // 
            this.btnSaveOK.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveOK.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnSaveOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveOK.BackgroundImage")));
            this.btnSaveOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveOK.BorderColor = System.Drawing.Color.Transparent;
            this.btnSaveOK.BorderRadius = 5;
            this.btnSaveOK.BorderSize = 1;
            this.btnSaveOK.ButtonImage = null;
            this.btnSaveOK.Corner =Corner.Both;
            this.btnSaveOK.FlatAppearance.BorderSize = 0;
            this.btnSaveOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveOK.ForeColor = System.Drawing.Color.Black;
            this.btnSaveOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveOK.IsCLick = true;
            this.btnSaveOK.IsNotChange = false;
            this.btnSaveOK.IsRect = false;
            this.btnSaveOK.IsUnGroup = true;
            this.btnSaveOK.Location = new System.Drawing.Point(3, 5);
            this.btnSaveOK.Name = "btnSaveOK";
            this.btnSaveOK.Size = new System.Drawing.Size(185, 40);
            this.btnSaveOK.TabIndex = 7;
            this.btnSaveOK.Text = "OK";
            this.btnSaveOK.TextColor = System.Drawing.Color.Black;
            this.btnSaveOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveOK.UseVisualStyleBackColor = false;
            this.btnSaveOK.Click += new System.EventHandler(this.btnSaveOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.TabIndex = 32;
            this.label2.Text = "Image";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnSaveRaw);
            this.panel4.Controls.Add(this.btnSaveRS);
            this.panel4.Location = new System.Drawing.Point(6, 131);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(386, 50);
            this.panel4.TabIndex = 33;
            // 
            // btnSaveRS
            // 
            this.btnSaveRS.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveRS.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSaveRS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveRS.BackgroundImage")));
            this.btnSaveRS.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveRS.BorderRadius = 5;
            this.btnSaveRS.BorderSize = 1;
            this.btnSaveRS.ButtonImage = null;
            this.btnSaveRS.Corner =Corner.Both;
            this.btnSaveRS.FlatAppearance.BorderSize = 0;
            this.btnSaveRS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveRS.ForeColor = System.Drawing.Color.Black;
            this.btnSaveRS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveRS.IsCLick = false;
            this.btnSaveRS.IsNotChange = false;
            this.btnSaveRS.IsRect = false;
            this.btnSaveRS.IsUnGroup = true;
            this.btnSaveRS.Location = new System.Drawing.Point(195, 5);
            this.btnSaveRS.Name = "btnSaveRS";
            this.btnSaveRS.Size = new System.Drawing.Size(188, 40);
            this.btnSaveRS.TabIndex = 9;
            this.btnSaveRS.Text = "Result Image";
            this.btnSaveRS.TextColor = System.Drawing.Color.Black;
            this.btnSaveRS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveRS.UseVisualStyleBackColor = false;
            this.btnSaveRS.Click += new System.EventHandler(this.btnSaveRS_Click);
            // 
            // btnSaveRaw
            // 
            this.btnSaveRaw.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveRaw.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnSaveRaw.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveRaw.BackgroundImage")));
            this.btnSaveRaw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveRaw.BorderColor = System.Drawing.Color.Transparent;
            this.btnSaveRaw.BorderRadius = 5;
            this.btnSaveRaw.BorderSize = 1;
            this.btnSaveRaw.ButtonImage = null;
            this.btnSaveRaw.Corner =Corner.Both;
            this.btnSaveRaw.FlatAppearance.BorderSize = 0;
            this.btnSaveRaw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveRaw.ForeColor = System.Drawing.Color.Black;
            this.btnSaveRaw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveRaw.IsCLick = true;
            this.btnSaveRaw.IsNotChange = false;
            this.btnSaveRaw.IsRect = false;
            this.btnSaveRaw.IsUnGroup = true;
            this.btnSaveRaw.Location = new System.Drawing.Point(3, 5);
            this.btnSaveRaw.Name = "btnSaveRaw";
            this.btnSaveRaw.Size = new System.Drawing.Size(185, 40);
            this.btnSaveRaw.TabIndex = 7;
            this.btnSaveRaw.Text = "Raw Image";
            this.btnSaveRaw.TextColor = System.Drawing.Color.Black;
            this.btnSaveRaw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveRaw.UseVisualStyleBackColor = false;
            this.btnSaveRaw.Click += new System.EventHandler(this.btnSaveRaw_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(431, 465);
            this.tabControl2.TabIndex = 49;
            // 
            // IOSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 465);
            this.Controls.Add(this.tabControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "IOSetting";
            this.Text = "Setting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IOSetting_FormClosing);
            this.Load += new System.EventHandler(this.IOSetting_Load);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.pCany.ResumeLayout(false);
            this.pArea.ResumeLayout(false);
            this.pArea.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel4;
        private RJButton btnSaveRaw;
        private RJButton btnSaveRS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private RJButton btnSaveOK;
        private RJButton btnSaveNG;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pArea;
        private System.Windows.Forms.Label label3;
        public TrackBar2 trackLimitDay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pCany;
        private RJButton btnbig;
        private RJButton btnNormal;
        private RJButton btnSmall;
        private System.Windows.Forms.TabControl tabControl2;
    }
}