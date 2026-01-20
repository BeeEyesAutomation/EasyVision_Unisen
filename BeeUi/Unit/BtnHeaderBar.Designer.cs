using BeeGlobal;
using BeeInterface;
using System.Windows.Forms;

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
            this.btnSetting = new BeeInterface.RJButton();
            this.btnReport = new BeeInterface.RJButton();
            this.btnUser = new BeeInterface.RJButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btncheck, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSetting, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnReport, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnUser, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(391, 53);
            this.tableLayoutPanel1.TabIndex = 36;
            // 
            // btncheck
            // 
            this.btncheck.AutoFont = true;
            this.btncheck.AutoFontHeightRatio = 0.85F;
            this.btncheck.AutoFontMax = 100F;
            this.btncheck.AutoFontMin = 10F;
            this.btncheck.AutoFontWidthRatio = 1F;
            this.btncheck.AutoImage = true;
            this.btncheck.AutoImageMaxRatio = 0.65F;
            this.btncheck.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btncheck.AutoImageTint = true;
            this.btncheck.BackColor = System.Drawing.Color.White;
            this.btncheck.BackgroundColor = System.Drawing.Color.White;
            this.btncheck.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btncheck.BackgroundImage")));
            this.btncheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btncheck.BorderColor = System.Drawing.Color.White;
            this.btncheck.BorderRadius = 5;
            this.btncheck.BorderSize = 1;
            this.btncheck.ClickBotColor = System.Drawing.Color.White;
            this.btncheck.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btncheck.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btncheck.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btncheck.Corner = BeeGlobal.Corner.None;
            this.btncheck.DebounceResizeMs = 16;
            this.btncheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btncheck.FlatAppearance.BorderSize = 0;
            this.btncheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.35156F);
            this.btncheck.ForeColor = System.Drawing.Color.Black;
            this.btncheck.Image = ((System.Drawing.Image)(resources.GetObject("btncheck.Image")));
            this.btncheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncheck.ImageDisabled = null;
            this.btncheck.ImageHover = null;
            this.btncheck.ImageNormal = null;
            this.btncheck.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btncheck.ImagePressed = null;
            this.btncheck.ImageTextSpacing = 2;
            this.btncheck.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btncheck.ImageTintHover = System.Drawing.Color.Empty;
            this.btncheck.ImageTintNormal = System.Drawing.Color.Empty;
            this.btncheck.ImageTintOpacity = 0.5F;
            this.btncheck.ImageTintPressed = System.Drawing.Color.Empty;
            this.btncheck.IsCLick = false;
            this.btncheck.IsNotChange = true;
            this.btncheck.IsRect = false;
            this.btncheck.IsTouch = false;
            this.btncheck.IsUnGroup = true;
            this.btncheck.Location = new System.Drawing.Point(97, 0);
            this.btncheck.Margin = new System.Windows.Forms.Padding(0);
            this.btncheck.Multiline = false;
            this.btncheck.Name = "btncheck";
            this.btncheck.Size = new System.Drawing.Size(97, 53);
            this.btncheck.TabIndex = 39;
            this.btncheck.Text = "Update";
            this.btncheck.TextColor = System.Drawing.Color.Black;
            this.btncheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btncheck.UseVisualStyleBackColor = false;
            this.btncheck.Click += new System.EventHandler(this.btncheck_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.AutoFont = true;
            this.btnSetting.AutoFontHeightRatio = 0.85F;
            this.btnSetting.AutoFontMax = 100F;
            this.btnSetting.AutoFontMin = 10F;
            this.btnSetting.AutoFontWidthRatio = 1F;
            this.btnSetting.AutoImage = true;
            this.btnSetting.AutoImageMaxRatio = 0.65F;
            this.btnSetting.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSetting.AutoImageTint = true;
            this.btnSetting.BackColor = System.Drawing.Color.White;
            this.btnSetting.BackgroundColor = System.Drawing.Color.White;
            this.btnSetting.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSetting.BackgroundImage")));
            this.btnSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSetting.BorderColor = System.Drawing.Color.White;
            this.btnSetting.BorderRadius = 5;
            this.btnSetting.BorderSize = 1;
            this.btnSetting.ClickBotColor = System.Drawing.Color.White;
            this.btnSetting.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSetting.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSetting.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSetting.Corner = BeeGlobal.Corner.None;
            this.btnSetting.DebounceResizeMs = 16;
            this.btnSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.35156F);
            this.btnSetting.ForeColor = System.Drawing.Color.Black;
            this.btnSetting.Image = global::BeeUi.Properties.Resources.Support;
            this.btnSetting.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSetting.ImageDisabled = null;
            this.btnSetting.ImageHover = null;
            this.btnSetting.ImageNormal = null;
            this.btnSetting.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSetting.ImagePressed = null;
            this.btnSetting.ImageTextSpacing = 2;
            this.btnSetting.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSetting.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSetting.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSetting.ImageTintOpacity = 0.5F;
            this.btnSetting.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSetting.IsCLick = false;
            this.btnSetting.IsNotChange = false;
            this.btnSetting.IsRect = false;
            this.btnSetting.IsTouch = false;
            this.btnSetting.IsUnGroup = true;
            this.btnSetting.Location = new System.Drawing.Point(291, 0);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetting.Multiline = false;
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(100, 53);
            this.btnSetting.TabIndex = 35;
            this.btnSetting.Text = "Settings";
            this.btnSetting.TextColor = System.Drawing.Color.Black;
            this.btnSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnReport
            // 
            this.btnReport.AutoFont = true;
            this.btnReport.AutoFontHeightRatio = 0.85F;
            this.btnReport.AutoFontMax = 100F;
            this.btnReport.AutoFontMin = 10F;
            this.btnReport.AutoFontWidthRatio = 1F;
            this.btnReport.AutoImage = true;
            this.btnReport.AutoImageMaxRatio = 0.65F;
            this.btnReport.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReport.AutoImageTint = true;
            this.btnReport.BackColor = System.Drawing.Color.White;
            this.btnReport.BackgroundColor = System.Drawing.Color.White;
            this.btnReport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReport.BackgroundImage")));
            this.btnReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnReport.BorderColor = System.Drawing.Color.White;
            this.btnReport.BorderRadius = 5;
            this.btnReport.BorderSize = 1;
            this.btnReport.ClickBotColor = System.Drawing.Color.White;
            this.btnReport.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnReport.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnReport.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReport.Corner = BeeGlobal.Corner.None;
            this.btnReport.DebounceResizeMs = 16;
            this.btnReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReport.FlatAppearance.BorderSize = 0;
            this.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnReport.ForeColor = System.Drawing.Color.Black;
            this.btnReport.Image = global::BeeUi.Properties.Resources.Report;
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReport.ImageDisabled = null;
            this.btnReport.ImageHover = null;
            this.btnReport.ImageNormal = null;
            this.btnReport.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReport.ImagePressed = null;
            this.btnReport.ImageTextSpacing = 2;
            this.btnReport.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReport.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReport.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReport.ImageTintOpacity = 0.5F;
            this.btnReport.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReport.IsCLick = false;
            this.btnReport.IsNotChange = true;
            this.btnReport.IsRect = false;
            this.btnReport.IsTouch = false;
            this.btnReport.IsUnGroup = true;
            this.btnReport.Location = new System.Drawing.Point(194, 0);
            this.btnReport.Margin = new System.Windows.Forms.Padding(0);
            this.btnReport.Multiline = false;
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(97, 53);
            this.btnReport.TabIndex = 34;
            this.btnReport.Text = "Report";
            this.btnReport.TextColor = System.Drawing.Color.Black;
            this.btnReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnUser
            // 
            this.btnUser.AutoFont = true;
            this.btnUser.AutoFontHeightRatio = 0.85F;
            this.btnUser.AutoFontMax = 100F;
            this.btnUser.AutoFontMin = 10F;
            this.btnUser.AutoFontWidthRatio = 1F;
            this.btnUser.AutoImage = true;
            this.btnUser.AutoImageMaxRatio = 0.65F;
            this.btnUser.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnUser.AutoImageTint = true;
            this.btnUser.BackColor = System.Drawing.Color.White;
            this.btnUser.BackgroundColor = System.Drawing.Color.White;
            this.btnUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUser.BackgroundImage")));
            this.btnUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUser.BorderColor = System.Drawing.Color.White;
            this.btnUser.BorderRadius = 5;
            this.btnUser.BorderSize = 1;
            this.btnUser.ClickBotColor = System.Drawing.Color.White;
            this.btnUser.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnUser.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnUser.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnUser.Corner = BeeGlobal.Corner.None;
            this.btnUser.DebounceResizeMs = 16;
            this.btnUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUser.FlatAppearance.BorderSize = 0;
            this.btnUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.35156F);
            this.btnUser.ForeColor = System.Drawing.Color.Black;
            this.btnUser.Image = global::BeeUi.Properties.Resources.User;
            this.btnUser.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUser.ImageDisabled = null;
            this.btnUser.ImageHover = null;
            this.btnUser.ImageNormal = null;
            this.btnUser.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnUser.ImagePressed = null;
            this.btnUser.ImageTextSpacing = 2;
            this.btnUser.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnUser.ImageTintHover = System.Drawing.Color.Empty;
            this.btnUser.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnUser.ImageTintOpacity = 0.5F;
            this.btnUser.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnUser.IsCLick = false;
            this.btnUser.IsNotChange = true;
            this.btnUser.IsRect = false;
            this.btnUser.IsTouch = false;
            this.btnUser.IsUnGroup = true;
            this.btnUser.Location = new System.Drawing.Point(0, 0);
            this.btnUser.Margin = new System.Windows.Forms.Padding(0);
            this.btnUser.Multiline = false;
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(97, 53);
            this.btnUser.TabIndex = 32;
            this.btnUser.Text = "Account";
            this.btnUser.TextColor = System.Drawing.Color.Black;
            this.btnUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUser.UseVisualStyleBackColor = false;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // BtnHeaderBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "BtnHeaderBar";
            this.Size = new System.Drawing.Size(391, 53);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RJButton btnSetting;
        private RJButton btnReport;
        public RJButton btnUser;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RJButton btncheck;
    }
}
