using BeeGlobal;
using BeeInterface;

namespace BeeUi.Unit
{
    partial class BtnHeaderBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BtnHeaderBar));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btncheck = new BeeInterface.RJButton();
            this.btnUser = new BeeInterface.RJButton();
            this.btnSettingPLC = new BeeInterface.RJButton();
            this.btnCamera = new BeeInterface.RJButton();
            this.btnfull = new BeeInterface.RJButton();
            this.btnSetting = new BeeInterface.RJButton();
            this.btnReport = new BeeInterface.RJButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btncheck, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnUser, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSettingPLC, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCamera, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnfull, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSetting, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnReport, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(540, 41);
            this.tableLayoutPanel1.TabIndex = 36;
            // 
            // btncheck
            // 
            this.btncheck.BackColor = System.Drawing.Color.Transparent;
            this.btncheck.BackgroundColor = System.Drawing.Color.Transparent;
            this.btncheck.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btncheck.BackgroundImage")));
            this.btncheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btncheck.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(108)))), ((int)(((byte)(182)))));
            this.btncheck.BorderRadius = 5;
            this.btncheck.BorderSize = 1;
            this.btncheck.Corner = BeeGlobal.Corner.Both;
            this.btncheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btncheck.FlatAppearance.BorderSize = 0;
            this.btncheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncheck.ForeColor = System.Drawing.Color.Black;
            this.btncheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncheck.IsCLick = false;
            this.btncheck.IsNotChange = true;
            this.btncheck.IsRect = true;
            this.btncheck.IsUnGroup = true;
            this.btncheck.Location = new System.Drawing.Point(467, 3);
            this.btncheck.Name = "btncheck";
            this.btncheck.Size = new System.Drawing.Size(70, 35);
            this.btncheck.TabIndex = 39;
            this.btncheck.Text = "Update";
            this.btncheck.TextColor = System.Drawing.Color.Black;
            this.btncheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btncheck.UseVisualStyleBackColor = false;
            this.btncheck.Click += new System.EventHandler(this.btncheck_Click);
            // 
            // btnUser
            // 
            this.btnUser.BackColor = System.Drawing.Color.Transparent;
            this.btnUser.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUser.BackgroundImage")));
            this.btnUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(108)))), ((int)(((byte)(182)))));
            this.btnUser.BorderRadius = 5;
            this.btnUser.BorderSize = 1;
            this.btnUser.Corner = BeeGlobal.Corner.Both;
            this.btnUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUser.FlatAppearance.BorderSize = 0;
            this.btnUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUser.ForeColor = System.Drawing.Color.Black;
            this.btnUser.Image = global::BeeUi.Properties.Resources.User;
            this.btnUser.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUser.IsCLick = false;
            this.btnUser.IsNotChange = true;
            this.btnUser.IsRect = true;
            this.btnUser.IsUnGroup = true;
            this.btnUser.Location = new System.Drawing.Point(383, 3);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(78, 35);
            this.btnUser.TabIndex = 32;
            this.btnUser.Text = "Account";
            this.btnUser.TextColor = System.Drawing.Color.Black;
            this.btnUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUser.UseVisualStyleBackColor = false;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnSettingPLC
            // 
            this.btnSettingPLC.BackColor = System.Drawing.Color.Transparent;
            this.btnSettingPLC.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnSettingPLC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSettingPLC.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(108)))), ((int)(((byte)(182)))));
            this.btnSettingPLC.BorderRadius = 5;
            this.btnSettingPLC.BorderSize = 1;
            this.btnSettingPLC.Corner = BeeGlobal.Corner.Both;
            this.btnSettingPLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSettingPLC.FlatAppearance.BorderSize = 0;
            this.btnSettingPLC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettingPLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettingPLC.ForeColor = System.Drawing.Color.Black;
            this.btnSettingPLC.Image = ((System.Drawing.Image)(resources.GetObject("btnSettingPLC.Image")));
            this.btnSettingPLC.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSettingPLC.IsCLick = false;
            this.btnSettingPLC.IsNotChange = false;
            this.btnSettingPLC.IsRect = false;
            this.btnSettingPLC.IsUnGroup = true;
            this.btnSettingPLC.Location = new System.Drawing.Point(83, 3);
            this.btnSettingPLC.Name = "btnSettingPLC";
            this.btnSettingPLC.Size = new System.Drawing.Size(74, 35);
            this.btnSettingPLC.TabIndex = 37;
            this.btnSettingPLC.Text = "PLC";
            this.btnSettingPLC.TextColor = System.Drawing.Color.Black;
            this.btnSettingPLC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSettingPLC.UseVisualStyleBackColor = false;
            this.btnSettingPLC.Click += new System.EventHandler(this.btnSettingPLC_Click);
            // 
            // btnCamera
            // 
            this.btnCamera.BackColor = System.Drawing.Color.Transparent;
            this.btnCamera.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCamera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCamera.BackgroundImage")));
            this.btnCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(108)))), ((int)(((byte)(182)))));
            this.btnCamera.BorderRadius = 5;
            this.btnCamera.BorderSize = 1;
            this.btnCamera.Corner = BeeGlobal.Corner.Both;
            this.btnCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera.FlatAppearance.BorderSize = 0;
            this.btnCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera.ForeColor = System.Drawing.Color.Black;
            this.btnCamera.Image = global::BeeUi.Properties.Resources.Camera1;
            this.btnCamera.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCamera.IsCLick = false;
            this.btnCamera.IsNotChange = true;
            this.btnCamera.IsRect = true;
            this.btnCamera.IsUnGroup = true;
            this.btnCamera.Location = new System.Drawing.Point(323, 3);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(54, 35);
            this.btnCamera.TabIndex = 33;
            this.btnCamera.Text = "Camera";
            this.btnCamera.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCamera.TextColor = System.Drawing.Color.Black;
            this.btnCamera.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera.UseVisualStyleBackColor = false;
            this.btnCamera.Visible = false;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // btnfull
            // 
            this.btnfull.BackColor = System.Drawing.Color.Transparent;
            this.btnfull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnfull.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnfull.BorderColor = System.Drawing.Color.Transparent;
            this.btnfull.BorderRadius = 5;
            this.btnfull.BorderSize = 1;
            this.btnfull.Corner = BeeGlobal.Corner.Both;
            this.btnfull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnfull.FlatAppearance.BorderSize = 0;
            this.btnfull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnfull.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnfull.ForeColor = System.Drawing.Color.Black;
            this.btnfull.Image = global::BeeUi.Properties.Resources.Full_Screen;
            this.btnfull.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnfull.IsCLick = false;
            this.btnfull.IsNotChange = false;
            this.btnfull.IsRect = false;
            this.btnfull.IsUnGroup = true;
            this.btnfull.Location = new System.Drawing.Point(3, 3);
            this.btnfull.Name = "btnfull";
            this.btnfull.Size = new System.Drawing.Size(74, 35);
            this.btnfull.TabIndex = 38;
            this.btnfull.Text = "Full";
            this.btnfull.TextColor = System.Drawing.Color.Black;
            this.btnfull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnfull.UseVisualStyleBackColor = false;
            this.btnfull.Click += new System.EventHandler(this.btnfull_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.BackColor = System.Drawing.Color.Transparent;
            this.btnSetting.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnSetting.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSetting.BackgroundImage")));
            this.btnSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSetting.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(108)))), ((int)(((byte)(182)))));
            this.btnSetting.BorderRadius = 5;
            this.btnSetting.BorderSize = 1;
            this.btnSetting.Corner = BeeGlobal.Corner.Both;
            this.btnSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetting.ForeColor = System.Drawing.Color.Black;
            this.btnSetting.Image = global::BeeUi.Properties.Resources.Support;
            this.btnSetting.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSetting.IsCLick = false;
            this.btnSetting.IsNotChange = true;
            this.btnSetting.IsRect = true;
            this.btnSetting.IsUnGroup = true;
            this.btnSetting.Location = new System.Drawing.Point(243, 3);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(74, 35);
            this.btnSetting.TabIndex = 35;
            this.btnSetting.Text = "Settings";
            this.btnSetting.TextColor = System.Drawing.Color.Black;
            this.btnSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnReport
            // 
            this.btnReport.BackColor = System.Drawing.Color.Transparent;
            this.btnReport.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnReport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReport.BackgroundImage")));
            this.btnReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnReport.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(108)))), ((int)(((byte)(182)))));
            this.btnReport.BorderRadius = 5;
            this.btnReport.BorderSize = 1;
            this.btnReport.Corner = BeeGlobal.Corner.Both;
            this.btnReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReport.FlatAppearance.BorderSize = 0;
            this.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.ForeColor = System.Drawing.Color.Black;
            this.btnReport.Image = global::BeeUi.Properties.Resources.Report;
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReport.IsCLick = false;
            this.btnReport.IsNotChange = true;
            this.btnReport.IsRect = true;
            this.btnReport.IsUnGroup = true;
            this.btnReport.Location = new System.Drawing.Point(163, 3);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(74, 35);
            this.btnReport.TabIndex = 34;
            this.btnReport.Text = "Report";
            this.btnReport.TextColor = System.Drawing.Color.Black;
            this.btnReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // BtnHeaderBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BtnHeaderBar";
            this.Size = new System.Drawing.Size(540, 41);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RJButton btnSetting;
        private RJButton btnReport;
        public RJButton btnCamera;
        public RJButton btnUser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public RJButton btnSettingPLC;
        public RJButton btnfull;
        private RJButton btncheck;
    }
}
