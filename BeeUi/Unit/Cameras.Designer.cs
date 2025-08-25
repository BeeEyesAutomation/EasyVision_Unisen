using BeeGlobal;
using BeeInterface;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    partial class Cameras
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cameras));
            this.pCamera = new System.Windows.Forms.TableLayoutPanel();
            this.btnLog = new BeeInterface.RJButton();
            this.btnHistory = new BeeInterface.RJButton();
            this.btnCamera4 = new BeeInterface.RJButton();
            this.btnCamera3 = new BeeInterface.RJButton();
            this.btnCamera1 = new BeeInterface.RJButton();
            this.btnCamera2 = new BeeInterface.RJButton();
            this.btnSettingPLC = new BeeInterface.RJButton();
            this.pCamera.SuspendLayout();
            this.SuspendLayout();
            // 
            // pCamera
            // 
            this.pCamera.AutoScroll = true;
            this.pCamera.AutoSize = true;
            this.pCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pCamera.ColumnCount = 8;
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pCamera.Controls.Add(this.btnCamera4, 3, 0);
            this.pCamera.Controls.Add(this.btnCamera3, 2, 0);
            this.pCamera.Controls.Add(this.btnCamera1, 0, 0);
            this.pCamera.Controls.Add(this.btnCamera2, 1, 0);
            this.pCamera.Controls.Add(this.btnSettingPLC, 4, 0);
            this.pCamera.Controls.Add(this.btnLog, 6, 0);
            this.pCamera.Controls.Add(this.btnHistory, 5, 0);
            this.pCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pCamera.Location = new System.Drawing.Point(0, 0);
            this.pCamera.Margin = new System.Windows.Forms.Padding(1);
            this.pCamera.Name = "pCamera";
            this.pCamera.RowCount = 1;
            this.pCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pCamera.Size = new System.Drawing.Size(578, 60);
            this.pCamera.TabIndex = 31;
            // 
            // btnLog
            // 
            this.btnLog.AutoFont = true;
            this.btnLog.AutoFontHeightRatio = 0.75F;
            this.btnLog.AutoFontMax = 100F;
            this.btnLog.AutoFontMin = 7F;
            this.btnLog.AutoFontWidthRatio = 0.92F;
            this.btnLog.AutoImage = true;
            this.btnLog.AutoImageMaxRatio = 0.8F;
            this.btnLog.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLog.AutoImageTint = true;
            this.btnLog.BackColor = System.Drawing.Color.LightGray;
            this.btnLog.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLog.BorderColor = System.Drawing.Color.LightGray;
            this.btnLog.BorderRadius = 10;
            this.btnLog.BorderSize = 1;
            this.btnLog.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLog.Corner = BeeGlobal.Corner.Right;
            this.btnLog.DebounceResizeMs = 16;
            this.btnLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLog.FlatAppearance.BorderSize = 0;
            this.btnLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnLog.ForeColor = System.Drawing.Color.Black;
            this.btnLog.Image = global::BeeUi.Properties.Resources.LOG;
            this.btnLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLog.ImageDisabled = null;
            this.btnLog.ImageHover = null;
            this.btnLog.ImageNormal = null;
            this.btnLog.ImagePadding = new System.Windows.Forms.Padding(3);
            this.btnLog.ImagePressed = null;
            this.btnLog.ImageTextSpacing = 3;
            this.btnLog.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLog.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLog.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLog.ImageTintOpacity = 1F;
            this.btnLog.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLog.IsCLick = false;
            this.btnLog.IsNotChange = false;
            this.btnLog.IsRect = true;
            this.btnLog.IsUnGroup = false;
            this.btnLog.Location = new System.Drawing.Point(460, 5);
            this.btnLog.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnLog.Multiline = false;
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(74, 50);
            this.btnLog.TabIndex = 36;
            this.btnLog.Text = "Logs";
            this.btnLog.TextColor = System.Drawing.Color.Black;
            this.btnLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLog.UseVisualStyleBackColor = false;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.AutoFont = true;
            this.btnHistory.AutoFontHeightRatio = 0.75F;
            this.btnHistory.AutoFontMax = 100F;
            this.btnHistory.AutoFontMin = 7F;
            this.btnHistory.AutoFontWidthRatio = 0.92F;
            this.btnHistory.AutoImage = true;
            this.btnHistory.AutoImageMaxRatio = 0.8F;
            this.btnHistory.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnHistory.AutoImageTint = true;
            this.btnHistory.BackColor = System.Drawing.Color.LightGray;
            this.btnHistory.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHistory.BorderColor = System.Drawing.Color.LightGray;
            this.btnHistory.BorderRadius = 0;
            this.btnHistory.BorderSize = 1;
            this.btnHistory.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnHistory.Corner = BeeGlobal.Corner.None;
            this.btnHistory.DebounceResizeMs = 16;
            this.btnHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnHistory.ForeColor = System.Drawing.Color.Black;
            this.btnHistory.Image = global::BeeUi.Properties.Resources.Image;
            this.btnHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHistory.ImageDisabled = null;
            this.btnHistory.ImageHover = null;
            this.btnHistory.ImageNormal = null;
            this.btnHistory.ImagePadding = new System.Windows.Forms.Padding(3);
            this.btnHistory.ImagePressed = null;
            this.btnHistory.ImageTextSpacing = 3;
            this.btnHistory.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnHistory.ImageTintHover = System.Drawing.Color.Empty;
            this.btnHistory.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnHistory.ImageTintOpacity = 1F;
            this.btnHistory.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnHistory.IsCLick = false;
            this.btnHistory.IsNotChange = false;
            this.btnHistory.IsRect = true;
            this.btnHistory.IsUnGroup = false;
            this.btnHistory.Location = new System.Drawing.Point(390, 5);
            this.btnHistory.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnHistory.Multiline = false;
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(70, 50);
            this.btnHistory.TabIndex = 35;
            this.btnHistory.Text = "History";
            this.btnHistory.TextColor = System.Drawing.Color.Black;
            this.btnHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnCamera4
            // 
            this.btnCamera4.AutoFont = true;
            this.btnCamera4.AutoFontHeightRatio = 0.75F;
            this.btnCamera4.AutoFontMax = 100F;
            this.btnCamera4.AutoFontMin = 7F;
            this.btnCamera4.AutoFontWidthRatio = 0.92F;
            this.btnCamera4.AutoImage = true;
            this.btnCamera4.AutoImageMaxRatio = 0.8F;
            this.btnCamera4.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCamera4.AutoImageTint = true;
            this.btnCamera4.BackColor = System.Drawing.Color.LightGray;
            this.btnCamera4.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnCamera4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera4.BorderColor = System.Drawing.Color.LightGray;
            this.btnCamera4.BorderRadius = 6;
            this.btnCamera4.BorderSize = 1;
            this.btnCamera4.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCamera4.Corner = BeeGlobal.Corner.None;
            this.btnCamera4.DebounceResizeMs = 16;
            this.btnCamera4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera4.FlatAppearance.BorderSize = 0;
            this.btnCamera4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnCamera4.ForeColor = System.Drawing.Color.Black;
            this.btnCamera4.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera4.Image")));
            this.btnCamera4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera4.ImageDisabled = null;
            this.btnCamera4.ImageHover = null;
            this.btnCamera4.ImageNormal = null;
            this.btnCamera4.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCamera4.ImagePressed = null;
            this.btnCamera4.ImageTextSpacing = 0;
            this.btnCamera4.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCamera4.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCamera4.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCamera4.ImageTintOpacity = 1F;
            this.btnCamera4.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCamera4.IsCLick = false;
            this.btnCamera4.IsNotChange = false;
            this.btnCamera4.IsRect = true;
            this.btnCamera4.IsUnGroup = false;
            this.btnCamera4.Location = new System.Drawing.Point(240, 5);
            this.btnCamera4.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnCamera4.Multiline = false;
            this.btnCamera4.Name = "btnCamera4";
            this.btnCamera4.Size = new System.Drawing.Size(80, 50);
            this.btnCamera4.TabIndex = 34;
            this.btnCamera4.Text = "Camera 4";
            this.btnCamera4.TextColor = System.Drawing.Color.Black;
            this.btnCamera4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera4.UseVisualStyleBackColor = false;
            this.btnCamera4.Visible = false;
            this.btnCamera4.Click += new System.EventHandler(this.btnCamera4_Click);
            // 
            // btnCamera3
            // 
            this.btnCamera3.AutoFont = true;
            this.btnCamera3.AutoFontHeightRatio = 0.75F;
            this.btnCamera3.AutoFontMax = 100F;
            this.btnCamera3.AutoFontMin = 7F;
            this.btnCamera3.AutoFontWidthRatio = 0.92F;
            this.btnCamera3.AutoImage = true;
            this.btnCamera3.AutoImageMaxRatio = 0.8F;
            this.btnCamera3.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCamera3.AutoImageTint = true;
            this.btnCamera3.BackColor = System.Drawing.Color.LightGray;
            this.btnCamera3.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnCamera3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera3.BorderColor = System.Drawing.Color.LightGray;
            this.btnCamera3.BorderRadius = 6;
            this.btnCamera3.BorderSize = 1;
            this.btnCamera3.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCamera3.Corner = BeeGlobal.Corner.None;
            this.btnCamera3.DebounceResizeMs = 16;
            this.btnCamera3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera3.FlatAppearance.BorderSize = 0;
            this.btnCamera3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnCamera3.ForeColor = System.Drawing.Color.Black;
            this.btnCamera3.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera3.Image")));
            this.btnCamera3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera3.ImageDisabled = null;
            this.btnCamera3.ImageHover = null;
            this.btnCamera3.ImageNormal = null;
            this.btnCamera3.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCamera3.ImagePressed = null;
            this.btnCamera3.ImageTextSpacing = 0;
            this.btnCamera3.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCamera3.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCamera3.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCamera3.ImageTintOpacity = 1F;
            this.btnCamera3.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCamera3.IsCLick = false;
            this.btnCamera3.IsNotChange = false;
            this.btnCamera3.IsRect = true;
            this.btnCamera3.IsUnGroup = false;
            this.btnCamera3.Location = new System.Drawing.Point(160, 5);
            this.btnCamera3.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnCamera3.Multiline = false;
            this.btnCamera3.Name = "btnCamera3";
            this.btnCamera3.Size = new System.Drawing.Size(80, 50);
            this.btnCamera3.TabIndex = 33;
            this.btnCamera3.Text = "Camera 3";
            this.btnCamera3.TextColor = System.Drawing.Color.Black;
            this.btnCamera3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera3.UseVisualStyleBackColor = false;
            this.btnCamera3.Visible = false;
            this.btnCamera3.Click += new System.EventHandler(this.btnCamera3_Click);
            // 
            // btnCamera1
            // 
            this.btnCamera1.AutoFont = true;
            this.btnCamera1.AutoFontHeightRatio = 0.75F;
            this.btnCamera1.AutoFontMax = 100F;
            this.btnCamera1.AutoFontMin = 7F;
            this.btnCamera1.AutoFontWidthRatio = 0.92F;
            this.btnCamera1.AutoImage = true;
            this.btnCamera1.AutoImageMaxRatio = 0.8F;
            this.btnCamera1.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCamera1.AutoImageTint = true;
            this.btnCamera1.BackColor = System.Drawing.Color.LightGray;
            this.btnCamera1.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnCamera1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera1.BorderColor = System.Drawing.Color.LightGray;
            this.btnCamera1.BorderRadius = 6;
            this.btnCamera1.BorderSize = 1;
            this.btnCamera1.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCamera1.Corner = BeeGlobal.Corner.Left;
            this.btnCamera1.DebounceResizeMs = 16;
            this.btnCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera1.FlatAppearance.BorderSize = 0;
            this.btnCamera1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnCamera1.ForeColor = System.Drawing.Color.Black;
            this.btnCamera1.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera1.Image")));
            this.btnCamera1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera1.ImageDisabled = null;
            this.btnCamera1.ImageHover = null;
            this.btnCamera1.ImageNormal = null;
            this.btnCamera1.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCamera1.ImagePressed = null;
            this.btnCamera1.ImageTextSpacing = 0;
            this.btnCamera1.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCamera1.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCamera1.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCamera1.ImageTintOpacity = 1F;
            this.btnCamera1.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCamera1.IsCLick = true;
            this.btnCamera1.IsNotChange = false;
            this.btnCamera1.IsRect = true;
            this.btnCamera1.IsUnGroup = false;
            this.btnCamera1.Location = new System.Drawing.Point(5, 5);
            this.btnCamera1.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.btnCamera1.Multiline = false;
            this.btnCamera1.Name = "btnCamera1";
            this.btnCamera1.Size = new System.Drawing.Size(75, 50);
            this.btnCamera1.TabIndex = 32;
            this.btnCamera1.Text = "Camera 1";
            this.btnCamera1.TextColor = System.Drawing.Color.Black;
            this.btnCamera1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera1.UseVisualStyleBackColor = false;
            this.btnCamera1.Click += new System.EventHandler(this.btnCamera1_Click);
            // 
            // btnCamera2
            // 
            this.btnCamera2.AutoFont = true;
            this.btnCamera2.AutoFontHeightRatio = 0.75F;
            this.btnCamera2.AutoFontMax = 100F;
            this.btnCamera2.AutoFontMin = 7F;
            this.btnCamera2.AutoFontWidthRatio = 0.92F;
            this.btnCamera2.AutoImage = true;
            this.btnCamera2.AutoImageMaxRatio = 0.8F;
            this.btnCamera2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCamera2.AutoImageTint = true;
            this.btnCamera2.BackColor = System.Drawing.Color.LightGray;
            this.btnCamera2.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnCamera2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera2.BorderColor = System.Drawing.Color.LightGray;
            this.btnCamera2.BorderRadius = 6;
            this.btnCamera2.BorderSize = 1;
            this.btnCamera2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCamera2.Corner = BeeGlobal.Corner.None;
            this.btnCamera2.DebounceResizeMs = 16;
            this.btnCamera2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera2.FlatAppearance.BorderSize = 0;
            this.btnCamera2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnCamera2.ForeColor = System.Drawing.Color.Black;
            this.btnCamera2.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera2.Image")));
            this.btnCamera2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera2.ImageDisabled = null;
            this.btnCamera2.ImageHover = null;
            this.btnCamera2.ImageNormal = null;
            this.btnCamera2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCamera2.ImagePressed = null;
            this.btnCamera2.ImageTextSpacing = 0;
            this.btnCamera2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCamera2.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCamera2.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCamera2.ImageTintOpacity = 1F;
            this.btnCamera2.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCamera2.IsCLick = false;
            this.btnCamera2.IsNotChange = false;
            this.btnCamera2.IsRect = true;
            this.btnCamera2.IsUnGroup = false;
            this.btnCamera2.Location = new System.Drawing.Point(80, 5);
            this.btnCamera2.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnCamera2.Multiline = false;
            this.btnCamera2.Name = "btnCamera2";
            this.btnCamera2.Size = new System.Drawing.Size(80, 50);
            this.btnCamera2.TabIndex = 19;
            this.btnCamera2.Text = "Camera 2";
            this.btnCamera2.TextColor = System.Drawing.Color.Black;
            this.btnCamera2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera2.UseVisualStyleBackColor = false;
            this.btnCamera2.Click += new System.EventHandler(this.btnCamera2_Click);
            // 
            // btnSettingPLC
            // 
            this.btnSettingPLC.AutoFont = true;
            this.btnSettingPLC.AutoFontHeightRatio = 0.85F;
            this.btnSettingPLC.AutoFontMax = 100F;
            this.btnSettingPLC.AutoFontMin = 7F;
            this.btnSettingPLC.AutoFontWidthRatio = 1F;
            this.btnSettingPLC.AutoImage = true;
            this.btnSettingPLC.AutoImageMaxRatio = 0.65F;
            this.btnSettingPLC.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSettingPLC.AutoImageTint = true;
            this.btnSettingPLC.BackColor = System.Drawing.Color.LightGray;
            this.btnSettingPLC.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnSettingPLC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSettingPLC.BorderColor = System.Drawing.Color.LightGray;
            this.btnSettingPLC.BorderRadius = 0;
            this.btnSettingPLC.BorderSize = 1;
            this.btnSettingPLC.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSettingPLC.Corner = BeeGlobal.Corner.None;
            this.btnSettingPLC.DebounceResizeMs = 16;
            this.btnSettingPLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSettingPLC.FlatAppearance.BorderSize = 0;
            this.btnSettingPLC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettingPLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnSettingPLC.ForeColor = System.Drawing.Color.Black;
            this.btnSettingPLC.Image = ((System.Drawing.Image)(resources.GetObject("btnSettingPLC.Image")));
            this.btnSettingPLC.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSettingPLC.ImageDisabled = null;
            this.btnSettingPLC.ImageHover = null;
            this.btnSettingPLC.ImageNormal = null;
            this.btnSettingPLC.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSettingPLC.ImagePressed = null;
            this.btnSettingPLC.ImageTextSpacing = 2;
            this.btnSettingPLC.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSettingPLC.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSettingPLC.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSettingPLC.ImageTintOpacity = 0.5F;
            this.btnSettingPLC.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSettingPLC.IsCLick = false;
            this.btnSettingPLC.IsNotChange = false;
            this.btnSettingPLC.IsRect = false;
            this.btnSettingPLC.IsUnGroup = false;
            this.btnSettingPLC.Location = new System.Drawing.Point(320, 5);
            this.btnSettingPLC.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnSettingPLC.Multiline = false;
            this.btnSettingPLC.Name = "btnSettingPLC";
            this.btnSettingPLC.Size = new System.Drawing.Size(70, 50);
            this.btnSettingPLC.TabIndex = 38;
            this.btnSettingPLC.Text = "Hardware";
            this.btnSettingPLC.TextColor = System.Drawing.Color.Black;
            this.btnSettingPLC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSettingPLC.UseVisualStyleBackColor = false;
            this.btnSettingPLC.Click += new System.EventHandler(this.btnSettingPLC_Click);
            // 
            // Cameras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.pCamera);
            this.DoubleBuffered = true;
            this.Name = "Cameras";
            this.Size = new System.Drawing.Size(578, 60);
            this.pCamera.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private  System.Windows.Forms.TableLayoutPanel pCamera;
        public RJButton btnCamera2;
        public RJButton btnCamera1;
        public RJButton btnCamera4;
        public RJButton btnCamera3;
        public RJButton btnHistory;
        public RJButton btnLog;
        public RJButton btnSettingPLC;
    }
}
