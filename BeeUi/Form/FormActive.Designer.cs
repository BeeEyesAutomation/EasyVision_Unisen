
namespace BeeUi
{
    partial class FormActive
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormActive));
            this.label2 = new System.Windows.Forms.Label();
            this.btnCoppy = new System.Windows.Forms.Button();
            this.btnKeys = new System.Windows.Forms.Button();
            this.btnActive = new System.Windows.Forms.Button();
            this.txtLicence = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMac = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(149, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(283, 31);
            this.label2.TabIndex = 7;
            this.label2.Text = "Active Vision Sensor";
            // 
            // btnCoppy
            // 
            this.btnCoppy.BackColor = System.Drawing.Color.White;
            this.btnCoppy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCoppy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCoppy.ForeColor = System.Drawing.Color.Black;
            this.btnCoppy.Image = global::BeeUi.Properties.Resources.Download;
            this.btnCoppy.Location = new System.Drawing.Point(93, 127);
            this.btnCoppy.Name = "btnCoppy";
            this.btnCoppy.Size = new System.Drawing.Size(179, 35);
            this.btnCoppy.TabIndex = 36;
            this.btnCoppy.Text = "Get  MacID";
            this.btnCoppy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCoppy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCoppy.UseVisualStyleBackColor = false;
            this.btnCoppy.Click += new System.EventHandler(this.btnCoppy_Click);
            // 
            // btnKeys
            // 
            this.btnKeys.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnKeys.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnKeys.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeys.ForeColor = System.Drawing.Color.Black;
            this.btnKeys.Image = global::BeeUi.Properties.Resources.Key_1;
            this.btnKeys.Location = new System.Drawing.Point(278, 127);
            this.btnKeys.Name = "btnKeys";
            this.btnKeys.Size = new System.Drawing.Size(172, 34);
            this.btnKeys.TabIndex = 34;
            this.btnKeys.Text = "Set Key";
            this.btnKeys.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnKeys.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnKeys.UseVisualStyleBackColor = false;
            this.btnKeys.Click += new System.EventHandler(this.btnKeys_Click);
            // 
            // btnActive
            // 
            this.btnActive.BackColor = System.Drawing.Color.Brown;
            this.btnActive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActive.ForeColor = System.Drawing.Color.White;
            this.btnActive.Location = new System.Drawing.Point(93, 171);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(357, 42);
            this.btnActive.TabIndex = 33;
            this.btnActive.Text = "Active now";
            this.btnActive.UseVisualStyleBackColor = false;
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // txtLicence
            // 
            this.txtLicence.BackColor = System.Drawing.Color.Wheat;
            this.txtLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLicence.Location = new System.Drawing.Point(93, 90);
            this.txtLicence.Name = "txtLicence";
            this.txtLicence.Size = new System.Drawing.Size(357, 31);
            this.txtLicence.TabIndex = 31;
            this.txtLicence.Text = "No Licence";
            this.txtLicence.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 25);
            this.label3.TabIndex = 32;
            this.label3.Text = "Licence";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVersion.ForeColor = System.Drawing.Color.Black;
            this.lbVersion.Location = new System.Drawing.Point(456, 213);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(75, 13);
            this.lbVersion.TabIndex = 37;
            this.lbVersion.Text = "2.1.0.10000";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::BeeUi.Properties.Resources.Delete;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(205)))), ((int)(((byte)(35)))));
            this.btnClose.Location = new System.Drawing.Point(517, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(20, 20);
            this.btnClose.TabIndex = 38;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            this.openFile.Filter = "KeyAcitve|*.bee";
            // 
            // saveFile
            // 
            this.saveFile.Filter = "ID File| *.mac";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 25);
            this.label1.TabIndex = 39;
            this.label1.Text = "MAC";
            // 
            // txtMac
            // 
            this.txtMac.BackColor = System.Drawing.Color.Wheat;
            this.txtMac.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMac.Location = new System.Drawing.Point(93, 47);
            this.txtMac.Name = "txtMac";
            this.txtMac.Size = new System.Drawing.Size(357, 31);
            this.txtMac.TabIndex = 40;
            this.txtMac.Text = "No MAC";
            this.txtMac.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormActive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(538, 234);
            this.Controls.Add(this.txtMac);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.btnCoppy);
            this.Controls.Add(this.btnKeys);
            this.Controls.Add(this.btnActive);
            this.Controls.Add(this.txtLicence);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormActive";
            this.Text = "FormActive";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormActive_FormClosing);
            this.Load += new System.EventHandler(this.FormLoad_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCoppy;
        private System.Windows.Forms.Button btnKeys;
        private System.Windows.Forms.Button btnActive;
        public System.Windows.Forms.TextBox txtLicence;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtMac;
    }
}