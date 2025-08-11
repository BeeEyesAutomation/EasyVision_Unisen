using BeeCore;
using BeeGlobal;
using BeeInterface;

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
            this.workConnect = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1 = new  System.Windows.Forms.TableLayoutPanel();
            this.btnStep4 = new BeeInterface.RJButton();
            this.btnStep3 = new BeeInterface.RJButton();
            this.btnStep2 = new BeeInterface.RJButton();
            this.btnSave = new BeeInterface.RJButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.btnStep1 = new BeeInterface.RJButton();
            this.btnSaveProgram = new BeeInterface.RJButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // workConnect
            // 
            this.workConnect.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workConnect_DoWork);
            this.workConnect.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workConnect_RunWorkerCompleted);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Silver;
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.Controls.Add(this.btnStep4, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStep3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStep2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox4, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox3, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStep1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(983, 110);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // btnStep4
            // 
            this.btnStep4.AutoFont = true;
            this.btnStep4.AutoFontHeightRatio = 0.75F;
            this.btnStep4.AutoFontMax = 100F;
            this.btnStep4.AutoFontMin = 6F;
            this.btnStep4.AutoFontWidthRatio = 0.92F;
            this.btnStep4.AutoImage = true;
            this.btnStep4.AutoImageMaxRatio = 0.75F;
            this.btnStep4.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnStep4.AutoImageTint = true;
            this.btnStep4.BackColor = System.Drawing.Color.Silver;
            this.btnStep4.BackgroundColor = System.Drawing.Color.Silver;
            this.btnStep4.BorderColor = System.Drawing.Color.Silver;
            this.btnStep4.BorderRadius = 14;
            this.btnStep4.BorderSize = 1;
            this.btnStep4.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnStep4.Corner = BeeGlobal.Corner.Both;
            this.btnStep4.DebounceResizeMs = 16;
            this.btnStep4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStep4.FlatAppearance.BorderSize = 0;
            this.btnStep4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnStep4.ForeColor = System.Drawing.Color.Black;
            this.btnStep4.Image = null;
            this.btnStep4.ImageDisabled = null;
            this.btnStep4.ImageHover = null;
            this.btnStep4.ImageNormal = null;
            this.btnStep4.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnStep4.ImagePressed = null;
            this.btnStep4.ImageTextSpacing = 6;
            this.btnStep4.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnStep4.ImageTintHover = System.Drawing.Color.Empty;
            this.btnStep4.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnStep4.ImageTintOpacity = 0.5F;
            this.btnStep4.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnStep4.IsCLick = false;
            this.btnStep4.IsNotChange = false;
            this.btnStep4.IsRect = false;
            this.btnStep4.IsUnGroup = false;
            this.btnStep4.Location = new System.Drawing.Point(628, 2);
            this.btnStep4.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.btnStep4.Multiline = true;
            this.btnStep4.Name = "btnStep4";
            this.btnStep4.Size = new System.Drawing.Size(137, 106);
            this.btnStep4.TabIndex = 16;
            this.btnStep4.Text = "4.Output\r\nAssignment";
            this.btnStep4.TextColor = System.Drawing.Color.Black;
            this.btnStep4.UseVisualStyleBackColor = false;
            this.btnStep4.Click += new System.EventHandler(this.btnStep4_Click);
            // 
            // btnStep3
            // 
            this.btnStep3.AutoFont = true;
            this.btnStep3.AutoFontHeightRatio = 0.75F;
            this.btnStep3.AutoFontMax = 100F;
            this.btnStep3.AutoFontMin = 6F;
            this.btnStep3.AutoFontWidthRatio = 0.92F;
            this.btnStep3.AutoImage = true;
            this.btnStep3.AutoImageMaxRatio = 0.75F;
            this.btnStep3.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnStep3.AutoImageTint = true;
            this.btnStep3.BackColor = System.Drawing.Color.Silver;
            this.btnStep3.BackgroundColor = System.Drawing.Color.Silver;
            this.btnStep3.BorderColor = System.Drawing.Color.Silver;
            this.btnStep3.BorderRadius = 14;
            this.btnStep3.BorderSize = 1;
            this.btnStep3.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnStep3.Corner = BeeGlobal.Corner.Both;
            this.btnStep3.DebounceResizeMs = 16;
            this.btnStep3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStep3.FlatAppearance.BorderSize = 0;
            this.btnStep3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep3.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.21875F);
            this.btnStep3.ForeColor = System.Drawing.Color.Black;
            this.btnStep3.Image = null;
            this.btnStep3.ImageDisabled = null;
            this.btnStep3.ImageHover = null;
            this.btnStep3.ImageNormal = null;
            this.btnStep3.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnStep3.ImagePressed = null;
            this.btnStep3.ImageTextSpacing = 6;
            this.btnStep3.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnStep3.ImageTintHover = System.Drawing.Color.Empty;
            this.btnStep3.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnStep3.ImageTintOpacity = 0.5F;
            this.btnStep3.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnStep3.IsCLick = false;
            this.btnStep3.IsNotChange = false;
            this.btnStep3.IsRect = false;
            this.btnStep3.IsUnGroup = false;
            this.btnStep3.Location = new System.Drawing.Point(422, 2);
            this.btnStep3.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.btnStep3.Multiline = true;
            this.btnStep3.Name = "btnStep3";
            this.btnStep3.Size = new System.Drawing.Size(137, 106);
            this.btnStep3.TabIndex = 15;
            this.btnStep3.Text = "3.Tool\r\nSettings";
            this.btnStep3.TextColor = System.Drawing.Color.Black;
            this.btnStep3.UseVisualStyleBackColor = false;
            this.btnStep3.Click += new System.EventHandler(this.btnStep3_Click);
            // 
            // btnStep2
            // 
            this.btnStep2.AutoFont = true;
            this.btnStep2.AutoFontHeightRatio = 0.75F;
            this.btnStep2.AutoFontMax = 100F;
            this.btnStep2.AutoFontMin = 6F;
            this.btnStep2.AutoFontWidthRatio = 0.92F;
            this.btnStep2.AutoImage = true;
            this.btnStep2.AutoImageMaxRatio = 0.75F;
            this.btnStep2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnStep2.AutoImageTint = true;
            this.btnStep2.BackColor = System.Drawing.Color.Silver;
            this.btnStep2.BackgroundColor = System.Drawing.Color.Silver;
            this.btnStep2.BorderColor = System.Drawing.Color.Silver;
            this.btnStep2.BorderRadius = 14;
            this.btnStep2.BorderSize = 1;
            this.btnStep2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnStep2.Corner = BeeGlobal.Corner.Both;
            this.btnStep2.DebounceResizeMs = 16;
            this.btnStep2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStep2.FlatAppearance.BorderSize = 0;
            this.btnStep2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.34375F);
            this.btnStep2.ForeColor = System.Drawing.Color.Black;
            this.btnStep2.Image = null;
            this.btnStep2.ImageDisabled = null;
            this.btnStep2.ImageHover = null;
            this.btnStep2.ImageNormal = null;
            this.btnStep2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnStep2.ImagePressed = null;
            this.btnStep2.ImageTextSpacing = 6;
            this.btnStep2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnStep2.ImageTintHover = System.Drawing.Color.Empty;
            this.btnStep2.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnStep2.ImageTintOpacity = 0.5F;
            this.btnStep2.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnStep2.IsCLick = false;
            this.btnStep2.IsNotChange = false;
            this.btnStep2.IsRect = false;
            this.btnStep2.IsUnGroup = false;
            this.btnStep2.Location = new System.Drawing.Point(216, 2);
            this.btnStep2.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.btnStep2.Multiline = true;
            this.btnStep2.Name = "btnStep2";
            this.btnStep2.Size = new System.Drawing.Size(137, 106);
            this.btnStep2.TabIndex = 14;
            this.btnStep2.Text = "2.Master \r\nResgistration";
            this.btnStep2.TextColor = System.Drawing.Color.Black;
            this.btnStep2.UseVisualStyleBackColor = false;
            this.btnStep2.Click += new System.EventHandler(this.btnStep2_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoEllipsis = true;
            this.btnSave.AutoFont = true;
            this.btnSave.AutoFontHeightRatio = 0.75F;
            this.btnSave.AutoFontMax = 100F;
            this.btnSave.AutoFontMin = 6F;
            this.btnSave.AutoFontWidthRatio = 0.92F;
            this.btnSave.AutoImage = true;
            this.btnSave.AutoImageMaxRatio = 0.75F;
            this.btnSave.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSave.AutoImageTint = true;
            this.btnSave.BackColor = System.Drawing.Color.Silver;
            this.btnSave.BackgroundColor = System.Drawing.Color.Silver;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.BorderColor = System.Drawing.Color.Silver;
            this.btnSave.BorderRadius = 20;
            this.btnSave.BorderSize = 1;
            this.btnSave.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSave.Corner = BeeGlobal.Corner.Both;
            this.btnSave.DebounceResizeMs = 16;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = null;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.ImageDisabled = null;
            this.btnSave.ImageHover = null;
            this.btnSave.ImageNormal = null;
            this.btnSave.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSave.ImagePressed = null;
            this.btnSave.ImageTextSpacing = 6;
            this.btnSave.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSave.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSave.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSave.ImageTintOpacity = 0.5F;
            this.btnSave.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSave.IsCLick = false;
            this.btnSave.IsNotChange = false;
            this.btnSave.IsRect = false;
            this.btnSave.IsUnGroup = false;
            this.btnSave.Location = new System.Drawing.Point(834, 2);
            this.btnSave.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.btnSave.Multiline = false;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(139, 106);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Run Mode";
            this.btnSave.TextColor = System.Drawing.Color.Black;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::BeeUi.Properties.Resources.Hide;
            this.pictureBox1.Location = new System.Drawing.Point(160, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 104);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Image = global::BeeUi.Properties.Resources.Hide;
            this.pictureBox4.Location = new System.Drawing.Point(778, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(43, 104);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 11;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::BeeUi.Properties.Resources.Hide;
            this.pictureBox2.Location = new System.Drawing.Point(366, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(43, 104);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Image = global::BeeUi.Properties.Resources.Hide;
            this.pictureBox3.Location = new System.Drawing.Point(572, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(43, 104);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // btnStep1
            // 
            this.btnStep1.AutoFont = true;
            this.btnStep1.AutoFontHeightRatio = 0.75F;
            this.btnStep1.AutoFontMax = 100F;
            this.btnStep1.AutoFontMin = 6F;
            this.btnStep1.AutoFontWidthRatio = 0.92F;
            this.btnStep1.AutoImage = true;
            this.btnStep1.AutoImageMaxRatio = 0.75F;
            this.btnStep1.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnStep1.AutoImageTint = true;
            this.btnStep1.BackColor = System.Drawing.Color.Silver;
            this.btnStep1.BackgroundColor = System.Drawing.Color.Silver;
            this.btnStep1.BorderColor = System.Drawing.Color.Silver;
            this.btnStep1.BorderRadius = 14;
            this.btnStep1.BorderSize = 1;
            this.btnStep1.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnStep1.Corner = BeeGlobal.Corner.Both;
            this.btnStep1.DebounceResizeMs = 16;
            this.btnStep1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStep1.FlatAppearance.BorderSize = 0;
            this.btnStep1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.34375F);
            this.btnStep1.ForeColor = System.Drawing.Color.Black;
            this.btnStep1.Image = null;
            this.btnStep1.ImageDisabled = null;
            this.btnStep1.ImageHover = null;
            this.btnStep1.ImageNormal = null;
            this.btnStep1.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnStep1.ImagePressed = null;
            this.btnStep1.ImageTextSpacing = 6;
            this.btnStep1.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnStep1.ImageTintHover = System.Drawing.Color.Empty;
            this.btnStep1.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnStep1.ImageTintOpacity = 0.5F;
            this.btnStep1.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnStep1.IsCLick = false;
            this.btnStep1.IsNotChange = false;
            this.btnStep1.IsRect = false;
            this.btnStep1.IsUnGroup = false;
            this.btnStep1.Location = new System.Drawing.Point(10, 2);
            this.btnStep1.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.btnStep1.Multiline = true;
            this.btnStep1.Name = "btnStep1";
            this.btnStep1.Size = new System.Drawing.Size(137, 106);
            this.btnStep1.TabIndex = 13;
            this.btnStep1.Text = "1.Image Optimization";
            this.btnStep1.TextColor = System.Drawing.Color.Black;
            this.btnStep1.UseVisualStyleBackColor = false;
            this.btnStep1.Click += new System.EventHandler(this.btnStep1_Click);
            // 
            // btnSaveProgram
            // 
            this.btnSaveProgram.AutoFont = false;
            this.btnSaveProgram.AutoFontHeightRatio = 0.75F;
            this.btnSaveProgram.AutoFontMax = 100F;
            this.btnSaveProgram.AutoFontMin = 6F;
            this.btnSaveProgram.AutoFontWidthRatio = 0.92F;
            this.btnSaveProgram.AutoImage = true;
            this.btnSaveProgram.AutoImageMaxRatio = 0.75F;
            this.btnSaveProgram.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSaveProgram.AutoImageTint = true;
            this.btnSaveProgram.BackColor = System.Drawing.Color.White;
            this.btnSaveProgram.BackgroundColor = System.Drawing.Color.White;
            this.btnSaveProgram.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveProgram.BackgroundImage")));
            this.btnSaveProgram.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveProgram.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveProgram.BorderRadius = 20;
            this.btnSaveProgram.BorderSize = 2;
            this.btnSaveProgram.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSaveProgram.Corner = BeeGlobal.Corner.Both;
            this.btnSaveProgram.DebounceResizeMs = 16;
            this.btnSaveProgram.FlatAppearance.BorderSize = 0;
            this.btnSaveProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveProgram.ForeColor = System.Drawing.Color.Black;
            this.btnSaveProgram.Image = global::BeeUi.Properties.Resources.Save_1;
            this.btnSaveProgram.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveProgram.ImageDisabled = null;
            this.btnSaveProgram.ImageHover = null;
            this.btnSaveProgram.ImageNormal = null;
            this.btnSaveProgram.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSaveProgram.ImagePressed = null;
            this.btnSaveProgram.ImageTextSpacing = 6;
            this.btnSaveProgram.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSaveProgram.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSaveProgram.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSaveProgram.ImageTintOpacity = 0.5F;
            this.btnSaveProgram.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSaveProgram.IsCLick = false;
            this.btnSaveProgram.IsNotChange = false;
            this.btnSaveProgram.IsRect = true;
            this.btnSaveProgram.IsUnGroup = false;
            this.btnSaveProgram.Location = new System.Drawing.Point(1096, 17);
            this.btnSaveProgram.Multiline = false;
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
            // StepEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnSaveProgram);
            this.DoubleBuffered = true;
            this.Name = "StepEdit";
            this.Size = new System.Drawing.Size(983, 110);
            this.Load += new System.EventHandler(this.StepEdit_Load);
            this.SizeChanged += new System.EventHandler(this.StepEdit_SizeChanged);
            this.VisibleChanged += new System.EventHandler(this.StepEdit_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.ComponentModel.BackgroundWorker workConnect;
        public RJButton btnSaveProgram;
        public RJButton btnSave;
        private System.Windows.Forms.PictureBox pictureBox4;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public RJButton btnStep1;
        public RJButton btnStep2;
        public RJButton btnStep3;
        public RJButton btnStep4;
    }
}
