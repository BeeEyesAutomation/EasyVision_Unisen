
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSaveRaw = new BeeInterface.RJButton();
            this.btnSaveRS = new BeeInterface.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveOK = new BeeInterface.RJButton();
            this.btnSaveNG = new BeeInterface.RJButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pArea = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.trackLimitDay = new BeeInterface.TrackBar2();
            this.label6 = new System.Windows.Forms.Label();
            this.pCany = new System.Windows.Forms.Panel();
            this.btnbig = new BeeInterface.RJButton();
            this.btnNormal = new BeeInterface.RJButton();
            this.btnSmall = new BeeInterface.RJButton();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnIsOneTrig = new BeeInterface.RJButton();
            this.btnIsMultiTrig = new BeeInterface.RJButton();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSingle = new BeeInterface.RJButton();
            this.btnMulti = new BeeInterface.RJButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pArea.SuspendLayout();
            this.pCany.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
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
            this.tabPage3.Size = new System.Drawing.Size(423, 609);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "History Data";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.btnSaveRaw);
            this.panel4.Controls.Add(this.btnSaveRS);
            this.panel4.Location = new System.Drawing.Point(6, 131);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(386, 50);
            this.panel4.TabIndex = 33;
            // 
            // btnSaveRaw
            // 
            this.btnSaveRaw.AutoFont = true;
            this.btnSaveRaw.AutoFontHeightRatio = 0.75F;
            this.btnSaveRaw.AutoFontMax = 100F;
            this.btnSaveRaw.AutoFontMin = 6F;
            this.btnSaveRaw.AutoFontWidthRatio = 0.92F;
            this.btnSaveRaw.AutoImage = true;
            this.btnSaveRaw.AutoImageMaxRatio = 0.75F;
            this.btnSaveRaw.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSaveRaw.AutoImageTint = true;
            this.btnSaveRaw.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveRaw.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSaveRaw.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveRaw.BackgroundImage")));
            this.btnSaveRaw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveRaw.BorderColor = System.Drawing.Color.Transparent;
            this.btnSaveRaw.BorderRadius = 5;
            this.btnSaveRaw.BorderSize = 1;
            this.btnSaveRaw.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSaveRaw.Corner = BeeGlobal.Corner.Both;
            this.btnSaveRaw.DebounceResizeMs = 16;
            this.btnSaveRaw.FlatAppearance.BorderSize = 0;
            this.btnSaveRaw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveRaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnSaveRaw.ForeColor = System.Drawing.Color.Black;
            this.btnSaveRaw.Image = null;
            this.btnSaveRaw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveRaw.ImageDisabled = null;
            this.btnSaveRaw.ImageHover = null;
            this.btnSaveRaw.ImageNormal = null;
            this.btnSaveRaw.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSaveRaw.ImagePressed = null;
            this.btnSaveRaw.ImageTextSpacing = 6;
            this.btnSaveRaw.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSaveRaw.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSaveRaw.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSaveRaw.ImageTintOpacity = 0.5F;
            this.btnSaveRaw.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSaveRaw.IsCLick = true;
            this.btnSaveRaw.IsNotChange = false;
            this.btnSaveRaw.IsRect = false;
            this.btnSaveRaw.IsUnGroup = true;
            this.btnSaveRaw.Location = new System.Drawing.Point(3, 5);
            this.btnSaveRaw.Multiline = false;
            this.btnSaveRaw.Name = "btnSaveRaw";
            this.btnSaveRaw.Size = new System.Drawing.Size(185, 40);
            this.btnSaveRaw.TabIndex = 7;
            this.btnSaveRaw.Text = "Raw Image";
            this.btnSaveRaw.TextColor = System.Drawing.Color.Black;
            this.btnSaveRaw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveRaw.UseVisualStyleBackColor = false;
            this.btnSaveRaw.Click += new System.EventHandler(this.btnSaveRaw_Click);
            // 
            // btnSaveRS
            // 
            this.btnSaveRS.AutoFont = true;
            this.btnSaveRS.AutoFontHeightRatio = 0.75F;
            this.btnSaveRS.AutoFontMax = 100F;
            this.btnSaveRS.AutoFontMin = 6F;
            this.btnSaveRS.AutoFontWidthRatio = 0.92F;
            this.btnSaveRS.AutoImage = true;
            this.btnSaveRS.AutoImageMaxRatio = 0.75F;
            this.btnSaveRS.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSaveRS.AutoImageTint = true;
            this.btnSaveRS.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveRS.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSaveRS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveRS.BackgroundImage")));
            this.btnSaveRS.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveRS.BorderRadius = 5;
            this.btnSaveRS.BorderSize = 1;
            this.btnSaveRS.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSaveRS.Corner = BeeGlobal.Corner.Both;
            this.btnSaveRS.DebounceResizeMs = 16;
            this.btnSaveRS.FlatAppearance.BorderSize = 0;
            this.btnSaveRS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveRS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnSaveRS.ForeColor = System.Drawing.Color.Black;
            this.btnSaveRS.Image = null;
            this.btnSaveRS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveRS.ImageDisabled = null;
            this.btnSaveRS.ImageHover = null;
            this.btnSaveRS.ImageNormal = null;
            this.btnSaveRS.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSaveRS.ImagePressed = null;
            this.btnSaveRS.ImageTextSpacing = 6;
            this.btnSaveRS.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSaveRS.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSaveRS.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSaveRS.ImageTintOpacity = 0.5F;
            this.btnSaveRS.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSaveRS.IsCLick = false;
            this.btnSaveRS.IsNotChange = false;
            this.btnSaveRS.IsRect = false;
            this.btnSaveRS.IsUnGroup = true;
            this.btnSaveRS.Location = new System.Drawing.Point(195, 5);
            this.btnSaveRS.Multiline = false;
            this.btnSaveRS.Name = "btnSaveRS";
            this.btnSaveRS.Size = new System.Drawing.Size(188, 40);
            this.btnSaveRS.TabIndex = 9;
            this.btnSaveRS.Text = "Result Image";
            this.btnSaveRS.TextColor = System.Drawing.Color.Black;
            this.btnSaveRS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveRS.UseVisualStyleBackColor = false;
            this.btnSaveRS.Click += new System.EventHandler(this.btnSaveRS_Click);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveOK);
            this.panel1.Controls.Add(this.btnSaveNG);
            this.panel1.Location = new System.Drawing.Point(6, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 50);
            this.panel1.TabIndex = 31;
            // 
            // btnSaveOK
            // 
            this.btnSaveOK.AutoFont = true;
            this.btnSaveOK.AutoFontHeightRatio = 0.75F;
            this.btnSaveOK.AutoFontMax = 100F;
            this.btnSaveOK.AutoFontMin = 6F;
            this.btnSaveOK.AutoFontWidthRatio = 0.92F;
            this.btnSaveOK.AutoImage = true;
            this.btnSaveOK.AutoImageMaxRatio = 0.75F;
            this.btnSaveOK.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSaveOK.AutoImageTint = true;
            this.btnSaveOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveOK.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSaveOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveOK.BackgroundImage")));
            this.btnSaveOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveOK.BorderColor = System.Drawing.Color.Transparent;
            this.btnSaveOK.BorderRadius = 5;
            this.btnSaveOK.BorderSize = 1;
            this.btnSaveOK.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSaveOK.Corner = BeeGlobal.Corner.Both;
            this.btnSaveOK.DebounceResizeMs = 16;
            this.btnSaveOK.FlatAppearance.BorderSize = 0;
            this.btnSaveOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnSaveOK.ForeColor = System.Drawing.Color.Black;
            this.btnSaveOK.Image = null;
            this.btnSaveOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveOK.ImageDisabled = null;
            this.btnSaveOK.ImageHover = null;
            this.btnSaveOK.ImageNormal = null;
            this.btnSaveOK.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSaveOK.ImagePressed = null;
            this.btnSaveOK.ImageTextSpacing = 6;
            this.btnSaveOK.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSaveOK.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSaveOK.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSaveOK.ImageTintOpacity = 0.5F;
            this.btnSaveOK.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSaveOK.IsCLick = true;
            this.btnSaveOK.IsNotChange = false;
            this.btnSaveOK.IsRect = false;
            this.btnSaveOK.IsUnGroup = true;
            this.btnSaveOK.Location = new System.Drawing.Point(3, 5);
            this.btnSaveOK.Multiline = false;
            this.btnSaveOK.Name = "btnSaveOK";
            this.btnSaveOK.Size = new System.Drawing.Size(185, 40);
            this.btnSaveOK.TabIndex = 7;
            this.btnSaveOK.Text = "OK";
            this.btnSaveOK.TextColor = System.Drawing.Color.Black;
            this.btnSaveOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveOK.UseVisualStyleBackColor = false;
            this.btnSaveOK.Click += new System.EventHandler(this.btnSaveOK_Click);
            // 
            // btnSaveNG
            // 
            this.btnSaveNG.AutoFont = true;
            this.btnSaveNG.AutoFontHeightRatio = 0.75F;
            this.btnSaveNG.AutoFontMax = 100F;
            this.btnSaveNG.AutoFontMin = 6F;
            this.btnSaveNG.AutoFontWidthRatio = 0.92F;
            this.btnSaveNG.AutoImage = true;
            this.btnSaveNG.AutoImageMaxRatio = 0.75F;
            this.btnSaveNG.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSaveNG.AutoImageTint = true;
            this.btnSaveNG.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveNG.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSaveNG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveNG.BackgroundImage")));
            this.btnSaveNG.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveNG.BorderRadius = 5;
            this.btnSaveNG.BorderSize = 1;
            this.btnSaveNG.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSaveNG.Corner = BeeGlobal.Corner.Both;
            this.btnSaveNG.DebounceResizeMs = 16;
            this.btnSaveNG.FlatAppearance.BorderSize = 0;
            this.btnSaveNG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnSaveNG.ForeColor = System.Drawing.Color.Black;
            this.btnSaveNG.Image = null;
            this.btnSaveNG.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveNG.ImageDisabled = null;
            this.btnSaveNG.ImageHover = null;
            this.btnSaveNG.ImageNormal = null;
            this.btnSaveNG.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSaveNG.ImagePressed = null;
            this.btnSaveNG.ImageTextSpacing = 6;
            this.btnSaveNG.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSaveNG.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSaveNG.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSaveNG.ImageTintOpacity = 0.5F;
            this.btnSaveNG.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSaveNG.IsCLick = false;
            this.btnSaveNG.IsNotChange = false;
            this.btnSaveNG.IsRect = false;
            this.btnSaveNG.IsUnGroup = true;
            this.btnSaveNG.Location = new System.Drawing.Point(195, 5);
            this.btnSaveNG.Multiline = false;
            this.btnSaveNG.Name = "btnSaveNG";
            this.btnSaveNG.Size = new System.Drawing.Size(188, 40);
            this.btnSaveNG.TabIndex = 9;
            this.btnSaveNG.Text = "NG";
            this.btnSaveNG.TextColor = System.Drawing.Color.Black;
            this.btnSaveNG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveNG.UseVisualStyleBackColor = false;
            this.btnSaveNG.Click += new System.EventHandler(this.btnSaveNG_Click);
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
            // trackLimitDay
            // 
            this.trackLimitDay.ColorTrack = System.Drawing.Color.Gray;
            this.trackLimitDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackLimitDay.Location = new System.Drawing.Point(53, 18);
            this.trackLimitDay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackLimitDay.Max = 60F;
            this.trackLimitDay.Min = 1F;
            this.trackLimitDay.Name = "trackLimitDay";
            this.trackLimitDay.Size = new System.Drawing.Size(289, 47);
            this.trackLimitDay.Step = 1F;
            this.trackLimitDay.TabIndex = 28;
            this.trackLimitDay.Value = 1F;
            this.trackLimitDay.ValueScore = 0F;
            this.trackLimitDay.ValueChanged += new System.Action<float>(this.trackLimitDay_ValueChanged);
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
            // btnbig
            // 
            this.btnbig.AutoFont = true;
            this.btnbig.AutoFontHeightRatio = 0.75F;
            this.btnbig.AutoFontMax = 100F;
            this.btnbig.AutoFontMin = 6F;
            this.btnbig.AutoFontWidthRatio = 0.92F;
            this.btnbig.AutoImage = true;
            this.btnbig.AutoImageMaxRatio = 0.75F;
            this.btnbig.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnbig.AutoImageTint = true;
            this.btnbig.BackColor = System.Drawing.SystemColors.Control;
            this.btnbig.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnbig.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnbig.BackgroundImage")));
            this.btnbig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnbig.BorderColor = System.Drawing.Color.Transparent;
            this.btnbig.BorderRadius = 5;
            this.btnbig.BorderSize = 1;
            this.btnbig.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnbig.Corner = BeeGlobal.Corner.Both;
            this.btnbig.DebounceResizeMs = 16;
            this.btnbig.FlatAppearance.BorderSize = 0;
            this.btnbig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnbig.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnbig.ForeColor = System.Drawing.Color.Black;
            this.btnbig.Image = null;
            this.btnbig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnbig.ImageDisabled = null;
            this.btnbig.ImageHover = null;
            this.btnbig.ImageNormal = null;
            this.btnbig.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnbig.ImagePressed = null;
            this.btnbig.ImageTextSpacing = 6;
            this.btnbig.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnbig.ImageTintHover = System.Drawing.Color.Empty;
            this.btnbig.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnbig.ImageTintOpacity = 0.5F;
            this.btnbig.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnbig.IsCLick = false;
            this.btnbig.IsNotChange = false;
            this.btnbig.IsRect = false;
            this.btnbig.IsUnGroup = false;
            this.btnbig.Location = new System.Drawing.Point(257, 8);
            this.btnbig.Multiline = false;
            this.btnbig.Name = "btnbig";
            this.btnbig.Size = new System.Drawing.Size(126, 40);
            this.btnbig.TabIndex = 11;
            this.btnbig.Text = "Big Size";
            this.btnbig.TextColor = System.Drawing.Color.Black;
            this.btnbig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnbig.UseVisualStyleBackColor = false;
            this.btnbig.Click += new System.EventHandler(this.btnbig_Click);
            // 
            // btnNormal
            // 
            this.btnNormal.AutoFont = true;
            this.btnNormal.AutoFontHeightRatio = 0.75F;
            this.btnNormal.AutoFontMax = 100F;
            this.btnNormal.AutoFontMin = 6F;
            this.btnNormal.AutoFontWidthRatio = 0.92F;
            this.btnNormal.AutoImage = true;
            this.btnNormal.AutoImageMaxRatio = 0.75F;
            this.btnNormal.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnNormal.AutoImageTint = true;
            this.btnNormal.BackColor = System.Drawing.SystemColors.Control;
            this.btnNormal.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnNormal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNormal.BackgroundImage")));
            this.btnNormal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNormal.BorderColor = System.Drawing.Color.Transparent;
            this.btnNormal.BorderRadius = 5;
            this.btnNormal.BorderSize = 1;
            this.btnNormal.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnNormal.Corner = BeeGlobal.Corner.Both;
            this.btnNormal.DebounceResizeMs = 16;
            this.btnNormal.FlatAppearance.BorderSize = 0;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnNormal.ForeColor = System.Drawing.Color.Black;
            this.btnNormal.Image = null;
            this.btnNormal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNormal.ImageDisabled = null;
            this.btnNormal.ImageHover = null;
            this.btnNormal.ImageNormal = null;
            this.btnNormal.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnNormal.ImagePressed = null;
            this.btnNormal.ImageTextSpacing = 6;
            this.btnNormal.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnNormal.ImageTintHover = System.Drawing.Color.Empty;
            this.btnNormal.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnNormal.ImageTintOpacity = 0.5F;
            this.btnNormal.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnNormal.IsCLick = false;
            this.btnNormal.IsNotChange = false;
            this.btnNormal.IsRect = false;
            this.btnNormal.IsUnGroup = false;
            this.btnNormal.Location = new System.Drawing.Point(127, 8);
            this.btnNormal.Multiline = false;
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(126, 40);
            this.btnNormal.TabIndex = 10;
            this.btnNormal.Text = "Normal Size";
            this.btnNormal.TextColor = System.Drawing.Color.Black;
            this.btnNormal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNormal.UseVisualStyleBackColor = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnSmall
            // 
            this.btnSmall.AutoFont = true;
            this.btnSmall.AutoFontHeightRatio = 0.75F;
            this.btnSmall.AutoFontMax = 100F;
            this.btnSmall.AutoFontMin = 6F;
            this.btnSmall.AutoFontWidthRatio = 0.92F;
            this.btnSmall.AutoImage = true;
            this.btnSmall.AutoImageMaxRatio = 0.75F;
            this.btnSmall.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSmall.AutoImageTint = true;
            this.btnSmall.BackColor = System.Drawing.SystemColors.Control;
            this.btnSmall.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSmall.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSmall.BackgroundImage")));
            this.btnSmall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSmall.BorderColor = System.Drawing.Color.Transparent;
            this.btnSmall.BorderRadius = 5;
            this.btnSmall.BorderSize = 1;
            this.btnSmall.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSmall.Corner = BeeGlobal.Corner.Both;
            this.btnSmall.DebounceResizeMs = 16;
            this.btnSmall.FlatAppearance.BorderSize = 0;
            this.btnSmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSmall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnSmall.ForeColor = System.Drawing.Color.Black;
            this.btnSmall.Image = null;
            this.btnSmall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSmall.ImageDisabled = null;
            this.btnSmall.ImageHover = null;
            this.btnSmall.ImageNormal = null;
            this.btnSmall.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSmall.ImagePressed = null;
            this.btnSmall.ImageTextSpacing = 6;
            this.btnSmall.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSmall.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSmall.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSmall.ImageTintOpacity = 0.5F;
            this.btnSmall.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSmall.IsCLick = true;
            this.btnSmall.IsNotChange = false;
            this.btnSmall.IsRect = false;
            this.btnSmall.IsUnGroup = false;
            this.btnSmall.Location = new System.Drawing.Point(1, 8);
            this.btnSmall.Multiline = false;
            this.btnSmall.Name = "btnSmall";
            this.btnSmall.Size = new System.Drawing.Size(120, 40);
            this.btnSmall.TabIndex = 7;
            this.btnSmall.Text = "Small Size";
            this.btnSmall.TextColor = System.Drawing.Color.Black;
            this.btnSmall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSmall.UseVisualStyleBackColor = false;
            this.btnSmall.Click += new System.EventHandler(this.btnSmall_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(431, 642);
            this.tabControl2.TabIndex = 49;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(423, 609);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "General";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.btnIsOneTrig);
            this.panel3.Controls.Add(this.btnIsMultiTrig);
            this.panel3.Location = new System.Drawing.Point(15, 125);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(386, 50);
            this.panel3.TabIndex = 34;
            // 
            // btnIsOneTrig
            // 
            this.btnIsOneTrig.AutoFont = true;
            this.btnIsOneTrig.AutoFontHeightRatio = 0.75F;
            this.btnIsOneTrig.AutoFontMax = 100F;
            this.btnIsOneTrig.AutoFontMin = 6F;
            this.btnIsOneTrig.AutoFontWidthRatio = 0.92F;
            this.btnIsOneTrig.AutoImage = true;
            this.btnIsOneTrig.AutoImageMaxRatio = 0.75F;
            this.btnIsOneTrig.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnIsOneTrig.AutoImageTint = true;
            this.btnIsOneTrig.BackColor = System.Drawing.SystemColors.Control;
            this.btnIsOneTrig.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnIsOneTrig.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnIsOneTrig.BackgroundImage")));
            this.btnIsOneTrig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIsOneTrig.BorderColor = System.Drawing.Color.Transparent;
            this.btnIsOneTrig.BorderRadius = 5;
            this.btnIsOneTrig.BorderSize = 1;
            this.btnIsOneTrig.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnIsOneTrig.Corner = BeeGlobal.Corner.Both;
            this.btnIsOneTrig.DebounceResizeMs = 16;
            this.btnIsOneTrig.FlatAppearance.BorderSize = 0;
            this.btnIsOneTrig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsOneTrig.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnIsOneTrig.ForeColor = System.Drawing.Color.Black;
            this.btnIsOneTrig.Image = null;
            this.btnIsOneTrig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIsOneTrig.ImageDisabled = null;
            this.btnIsOneTrig.ImageHover = null;
            this.btnIsOneTrig.ImageNormal = null;
            this.btnIsOneTrig.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnIsOneTrig.ImagePressed = null;
            this.btnIsOneTrig.ImageTextSpacing = 6;
            this.btnIsOneTrig.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnIsOneTrig.ImageTintHover = System.Drawing.Color.Empty;
            this.btnIsOneTrig.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnIsOneTrig.ImageTintOpacity = 0.5F;
            this.btnIsOneTrig.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnIsOneTrig.IsCLick = true;
            this.btnIsOneTrig.IsNotChange = false;
            this.btnIsOneTrig.IsRect = false;
            this.btnIsOneTrig.IsUnGroup = false;
            this.btnIsOneTrig.Location = new System.Drawing.Point(3, 5);
            this.btnIsOneTrig.Multiline = false;
            this.btnIsOneTrig.Name = "btnIsOneTrig";
            this.btnIsOneTrig.Size = new System.Drawing.Size(185, 40);
            this.btnIsOneTrig.TabIndex = 7;
            this.btnIsOneTrig.Text = "One Trigger";
            this.btnIsOneTrig.TextColor = System.Drawing.Color.Black;
            this.btnIsOneTrig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIsOneTrig.UseVisualStyleBackColor = false;
            this.btnIsOneTrig.Click += new System.EventHandler(this.btnIsOneTrig_Click);
            // 
            // btnIsMultiTrig
            // 
            this.btnIsMultiTrig.AutoFont = true;
            this.btnIsMultiTrig.AutoFontHeightRatio = 0.75F;
            this.btnIsMultiTrig.AutoFontMax = 100F;
            this.btnIsMultiTrig.AutoFontMin = 6F;
            this.btnIsMultiTrig.AutoFontWidthRatio = 0.92F;
            this.btnIsMultiTrig.AutoImage = true;
            this.btnIsMultiTrig.AutoImageMaxRatio = 0.75F;
            this.btnIsMultiTrig.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnIsMultiTrig.AutoImageTint = true;
            this.btnIsMultiTrig.BackColor = System.Drawing.SystemColors.Control;
            this.btnIsMultiTrig.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnIsMultiTrig.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnIsMultiTrig.BackgroundImage")));
            this.btnIsMultiTrig.BorderColor = System.Drawing.Color.Silver;
            this.btnIsMultiTrig.BorderRadius = 5;
            this.btnIsMultiTrig.BorderSize = 1;
            this.btnIsMultiTrig.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnIsMultiTrig.Corner = BeeGlobal.Corner.Both;
            this.btnIsMultiTrig.DebounceResizeMs = 16;
            this.btnIsMultiTrig.FlatAppearance.BorderSize = 0;
            this.btnIsMultiTrig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsMultiTrig.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnIsMultiTrig.ForeColor = System.Drawing.Color.Black;
            this.btnIsMultiTrig.Image = null;
            this.btnIsMultiTrig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIsMultiTrig.ImageDisabled = null;
            this.btnIsMultiTrig.ImageHover = null;
            this.btnIsMultiTrig.ImageNormal = null;
            this.btnIsMultiTrig.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnIsMultiTrig.ImagePressed = null;
            this.btnIsMultiTrig.ImageTextSpacing = 6;
            this.btnIsMultiTrig.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnIsMultiTrig.ImageTintHover = System.Drawing.Color.Empty;
            this.btnIsMultiTrig.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnIsMultiTrig.ImageTintOpacity = 0.5F;
            this.btnIsMultiTrig.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnIsMultiTrig.IsCLick = false;
            this.btnIsMultiTrig.IsNotChange = false;
            this.btnIsMultiTrig.IsRect = false;
            this.btnIsMultiTrig.IsUnGroup = false;
            this.btnIsMultiTrig.Location = new System.Drawing.Point(195, 5);
            this.btnIsMultiTrig.Multiline = false;
            this.btnIsMultiTrig.Name = "btnIsMultiTrig";
            this.btnIsMultiTrig.Size = new System.Drawing.Size(188, 40);
            this.btnIsMultiTrig.TabIndex = 9;
            this.btnIsMultiTrig.Text = "Multi Trigger";
            this.btnIsMultiTrig.TextColor = System.Drawing.Color.Black;
            this.btnIsMultiTrig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIsMultiTrig.UseVisualStyleBackColor = false;
            this.btnIsMultiTrig.Click += new System.EventHandler(this.btnIsMultiTrig_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 20);
            this.label4.TabIndex = 33;
            this.label4.Text = "Process";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.btnSingle);
            this.panel2.Controls.Add(this.btnMulti);
            this.panel2.Location = new System.Drawing.Point(12, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(386, 50);
            this.panel2.TabIndex = 32;
            // 
            // btnSingle
            // 
            this.btnSingle.AutoFont = true;
            this.btnSingle.AutoFontHeightRatio = 0.75F;
            this.btnSingle.AutoFontMax = 100F;
            this.btnSingle.AutoFontMin = 6F;
            this.btnSingle.AutoFontWidthRatio = 0.92F;
            this.btnSingle.AutoImage = true;
            this.btnSingle.AutoImageMaxRatio = 0.75F;
            this.btnSingle.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSingle.AutoImageTint = true;
            this.btnSingle.BackColor = System.Drawing.SystemColors.Control;
            this.btnSingle.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSingle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSingle.BackgroundImage")));
            this.btnSingle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSingle.BorderColor = System.Drawing.Color.Transparent;
            this.btnSingle.BorderRadius = 5;
            this.btnSingle.BorderSize = 1;
            this.btnSingle.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSingle.Corner = BeeGlobal.Corner.Both;
            this.btnSingle.DebounceResizeMs = 16;
            this.btnSingle.FlatAppearance.BorderSize = 0;
            this.btnSingle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnSingle.ForeColor = System.Drawing.Color.Black;
            this.btnSingle.Image = null;
            this.btnSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSingle.ImageDisabled = null;
            this.btnSingle.ImageHover = null;
            this.btnSingle.ImageNormal = null;
            this.btnSingle.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSingle.ImagePressed = null;
            this.btnSingle.ImageTextSpacing = 6;
            this.btnSingle.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSingle.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSingle.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSingle.ImageTintOpacity = 0.5F;
            this.btnSingle.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSingle.IsCLick = false;
            this.btnSingle.IsNotChange = false;
            this.btnSingle.IsRect = false;
            this.btnSingle.IsUnGroup = false;
            this.btnSingle.Location = new System.Drawing.Point(3, 5);
            this.btnSingle.Multiline = false;
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(185, 40);
            this.btnSingle.TabIndex = 7;
            this.btnSingle.Text = "Single";
            this.btnSingle.TextColor = System.Drawing.Color.Black;
            this.btnSingle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSingle.UseVisualStyleBackColor = false;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            // 
            // btnMulti
            // 
            this.btnMulti.AutoFont = true;
            this.btnMulti.AutoFontHeightRatio = 0.75F;
            this.btnMulti.AutoFontMax = 100F;
            this.btnMulti.AutoFontMin = 6F;
            this.btnMulti.AutoFontWidthRatio = 0.92F;
            this.btnMulti.AutoImage = true;
            this.btnMulti.AutoImageMaxRatio = 0.75F;
            this.btnMulti.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMulti.AutoImageTint = true;
            this.btnMulti.BackColor = System.Drawing.SystemColors.Control;
            this.btnMulti.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnMulti.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMulti.BackgroundImage")));
            this.btnMulti.BorderColor = System.Drawing.Color.Silver;
            this.btnMulti.BorderRadius = 5;
            this.btnMulti.BorderSize = 1;
            this.btnMulti.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMulti.Corner = BeeGlobal.Corner.Both;
            this.btnMulti.DebounceResizeMs = 16;
            this.btnMulti.FlatAppearance.BorderSize = 0;
            this.btnMulti.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMulti.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnMulti.ForeColor = System.Drawing.Color.Black;
            this.btnMulti.Image = null;
            this.btnMulti.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMulti.ImageDisabled = null;
            this.btnMulti.ImageHover = null;
            this.btnMulti.ImageNormal = null;
            this.btnMulti.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMulti.ImagePressed = null;
            this.btnMulti.ImageTextSpacing = 6;
            this.btnMulti.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMulti.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMulti.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMulti.ImageTintOpacity = 0.5F;
            this.btnMulti.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMulti.IsCLick = true;
            this.btnMulti.IsNotChange = false;
            this.btnMulti.IsRect = false;
            this.btnMulti.IsUnGroup = false;
            this.btnMulti.Location = new System.Drawing.Point(195, 5);
            this.btnMulti.Multiline = false;
            this.btnMulti.Name = "btnMulti";
            this.btnMulti.Size = new System.Drawing.Size(188, 40);
            this.btnMulti.TabIndex = 9;
            this.btnMulti.Text = "Multi";
            this.btnMulti.TextColor = System.Drawing.Color.Black;
            this.btnMulti.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMulti.UseVisualStyleBackColor = false;
            this.btnMulti.Click += new System.EventHandler(this.btnMulti_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Camera";
            // 
            // IOSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 642);
            this.Controls.Add(this.tabControl2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IOSetting";
            this.Text = "Setting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IOSetting_FormClosing);
            this.Load += new System.EventHandler(this.IOSetting_Load);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pArea.ResumeLayout(false);
            this.pArea.PerformLayout();
            this.pCany.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
        private RJButton btnSingle;
        private RJButton btnMulti;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private RJButton btnIsOneTrig;
        private RJButton btnIsMultiTrig;
        private System.Windows.Forms.Label label4;
    }
}