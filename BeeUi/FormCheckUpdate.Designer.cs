namespace BeeUi
{
    partial class FormCheckUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCheckUpdate));
            this.btnCheckUpdate = new BeeInterface.RJButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnUpdate = new BeeInterface.RJButton();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnCheckUpdate
            // 
            this.btnCheckUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnCheckUpdate.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCheckUpdate.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnCheckUpdate.BorderRadius = 0;
            this.btnCheckUpdate.BorderSize = 0;
            this.btnCheckUpdate.Image = null;
            this.btnCheckUpdate.Corner = BeeGlobal.Corner.Both;
            this.btnCheckUpdate.FlatAppearance.BorderSize = 0;
            this.btnCheckUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckUpdate.ForeColor = System.Drawing.Color.Black;
            this.btnCheckUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckUpdate.IsCLick = false;
            this.btnCheckUpdate.IsNotChange = false;
            this.btnCheckUpdate.IsRect = false;
            this.btnCheckUpdate.IsUnGroup = false;
            this.btnCheckUpdate.Location = new System.Drawing.Point(12, 250);
            this.btnCheckUpdate.Name = "btnCheckUpdate";
            this.btnCheckUpdate.Size = new System.Drawing.Size(169, 47);
            this.btnCheckUpdate.TabIndex = 0;
            this.btnCheckUpdate.Text = "Check Update";
            this.btnCheckUpdate.TextColor = System.Drawing.Color.Black;
            this.btnCheckUpdate.UseVisualStyleBackColor = false;
            this.btnCheckUpdate.Click += new System.EventHandler(this.btnCheckUpdate_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 24);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(342, 23);
            this.progressBar.TabIndex = 1;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btnUpdate.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnUpdate.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnUpdate.BorderRadius = 0;
            this.btnUpdate.BorderSize = 0;
            this.btnUpdate.Image = null;
            this.btnUpdate.Corner = BeeGlobal.Corner.Both;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.Black;
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdate.IsCLick = false;
            this.btnUpdate.IsNotChange = false;
            this.btnUpdate.IsRect = false;
            this.btnUpdate.IsUnGroup = false;
            this.btnUpdate.Location = new System.Drawing.Point(196, 250);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(158, 47);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.TextColor = System.Drawing.Color.Black;
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(12, 61);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(342, 21);
            this.lbStatus.TabIndex = 3;
            this.lbStatus.Text = "----";
            // 
            // lbList
            // 
            this.lbList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbList.FormattingEnabled = true;
            this.lbList.ItemHeight = 24;
            this.lbList.Location = new System.Drawing.Point(16, 96);
            this.lbList.Name = "lbList";
            this.lbList.Size = new System.Drawing.Size(338, 148);
            this.lbList.TabIndex = 4;
            // 
            // FormCheckUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 309);
            this.Controls.Add(this.lbList);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnCheckUpdate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCheckUpdate";
            this.Text = "Check Update";
            this.Load += new System.EventHandler(this.FormCheckUpdate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private BeeInterface.RJButton btnCheckUpdate;
        private System.Windows.Forms.ProgressBar progressBar;
        private BeeInterface.RJButton btnUpdate;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.ListBox lbList;
    }
}