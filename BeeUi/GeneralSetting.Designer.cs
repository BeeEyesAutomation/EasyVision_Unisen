
using BeeGlobal;
using BeeInterface;

namespace BeeUi
{
    partial class GeneralSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralSetting));
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTriggerMulti = new BeeInterface.RJButton();
            this.btnTriggerOne = new BeeInterface.RJButton();
            this.label4 = new System.Windows.Forms.Label();
            this.numTrigger = new BeeInterface.AdjustBarEx();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMulti = new BeeInterface.RJButton();
            this.btnSingle = new BeeInterface.RJButton();
            this.btnSave = new BeeInterface.RJButton();
            this.tabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pArea.SuspendLayout();
            this.pCany.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.btnSaveRaw.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSaveRaw.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSaveRaw.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.btnSaveRS.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSaveRS.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSaveRS.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.btnSaveOK.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSaveOK.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSaveOK.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.btnSaveNG.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSaveNG.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSaveNG.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.btnbig.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnbig.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnbig.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.btnNormal.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnNormal.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnNormal.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.btnSmall.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSmall.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSmall.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(423, 609);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "General";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.numTrigger, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(417, 603);
            this.tableLayoutPanel1.TabIndex = 35;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint_1);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnTriggerMulti, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnTriggerOne, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 257);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(407, 55);
            this.tableLayoutPanel3.TabIndex = 72;
            // 
            // btnTriggerMulti
            // 
            this.btnTriggerMulti.AutoFont = true;
            this.btnTriggerMulti.AutoFontHeightRatio = 0.75F;
            this.btnTriggerMulti.AutoFontMax = 100F;
            this.btnTriggerMulti.AutoFontMin = 6F;
            this.btnTriggerMulti.AutoFontWidthRatio = 0.92F;
            this.btnTriggerMulti.AutoImage = true;
            this.btnTriggerMulti.AutoImageMaxRatio = 0.75F;
            this.btnTriggerMulti.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTriggerMulti.AutoImageTint = true;
            this.btnTriggerMulti.BackColor = System.Drawing.SystemColors.Control;
            this.btnTriggerMulti.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnTriggerMulti.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTriggerMulti.BackgroundImage")));
            this.btnTriggerMulti.BorderColor = System.Drawing.Color.Silver;
            this.btnTriggerMulti.BorderRadius = 5;
            this.btnTriggerMulti.BorderSize = 1;
            this.btnTriggerMulti.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnTriggerMulti.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnTriggerMulti.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnTriggerMulti.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTriggerMulti.Corner = BeeGlobal.Corner.Both;
            this.btnTriggerMulti.DebounceResizeMs = 16;
            this.btnTriggerMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTriggerMulti.FlatAppearance.BorderSize = 0;
            this.btnTriggerMulti.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTriggerMulti.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnTriggerMulti.ForeColor = System.Drawing.Color.Black;
            this.btnTriggerMulti.Image = null;
            this.btnTriggerMulti.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTriggerMulti.ImageDisabled = null;
            this.btnTriggerMulti.ImageHover = null;
            this.btnTriggerMulti.ImageNormal = null;
            this.btnTriggerMulti.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTriggerMulti.ImagePressed = null;
            this.btnTriggerMulti.ImageTextSpacing = 6;
            this.btnTriggerMulti.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTriggerMulti.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTriggerMulti.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTriggerMulti.ImageTintOpacity = 0.5F;
            this.btnTriggerMulti.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTriggerMulti.IsCLick = true;
            this.btnTriggerMulti.IsNotChange = false;
            this.btnTriggerMulti.IsRect = false;
            this.btnTriggerMulti.IsUnGroup = false;
            this.btnTriggerMulti.Location = new System.Drawing.Point(206, 8);
            this.btnTriggerMulti.Multiline = false;
            this.btnTriggerMulti.Name = "btnTriggerMulti";
            this.btnTriggerMulti.Size = new System.Drawing.Size(193, 39);
            this.btnTriggerMulti.TabIndex = 9;
            this.btnTriggerMulti.Text = "Multi";
            this.btnTriggerMulti.TextColor = System.Drawing.Color.Black;
            this.btnTriggerMulti.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTriggerMulti.UseVisualStyleBackColor = false;
            this.btnTriggerMulti.Click += new System.EventHandler(this.btnTriggerMulti_Click);
            // 
            // btnTriggerOne
            // 
            this.btnTriggerOne.AutoFont = true;
            this.btnTriggerOne.AutoFontHeightRatio = 0.75F;
            this.btnTriggerOne.AutoFontMax = 100F;
            this.btnTriggerOne.AutoFontMin = 6F;
            this.btnTriggerOne.AutoFontWidthRatio = 0.92F;
            this.btnTriggerOne.AutoImage = true;
            this.btnTriggerOne.AutoImageMaxRatio = 0.75F;
            this.btnTriggerOne.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTriggerOne.AutoImageTint = true;
            this.btnTriggerOne.BackColor = System.Drawing.SystemColors.Control;
            this.btnTriggerOne.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnTriggerOne.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTriggerOne.BackgroundImage")));
            this.btnTriggerOne.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTriggerOne.BorderColor = System.Drawing.Color.Transparent;
            this.btnTriggerOne.BorderRadius = 5;
            this.btnTriggerOne.BorderSize = 1;
            this.btnTriggerOne.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnTriggerOne.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnTriggerOne.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnTriggerOne.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTriggerOne.Corner = BeeGlobal.Corner.Both;
            this.btnTriggerOne.DebounceResizeMs = 16;
            this.btnTriggerOne.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTriggerOne.FlatAppearance.BorderSize = 0;
            this.btnTriggerOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTriggerOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnTriggerOne.ForeColor = System.Drawing.Color.Black;
            this.btnTriggerOne.Image = null;
            this.btnTriggerOne.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTriggerOne.ImageDisabled = null;
            this.btnTriggerOne.ImageHover = null;
            this.btnTriggerOne.ImageNormal = null;
            this.btnTriggerOne.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTriggerOne.ImagePressed = null;
            this.btnTriggerOne.ImageTextSpacing = 6;
            this.btnTriggerOne.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTriggerOne.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTriggerOne.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTriggerOne.ImageTintOpacity = 0.5F;
            this.btnTriggerOne.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTriggerOne.IsCLick = false;
            this.btnTriggerOne.IsNotChange = false;
            this.btnTriggerOne.IsRect = false;
            this.btnTriggerOne.IsUnGroup = false;
            this.btnTriggerOne.Location = new System.Drawing.Point(8, 8);
            this.btnTriggerOne.Multiline = false;
            this.btnTriggerOne.Name = "btnTriggerOne";
            this.btnTriggerOne.Size = new System.Drawing.Size(192, 39);
            this.btnTriggerOne.TabIndex = 7;
            this.btnTriggerOne.Text = "Single";
            this.btnTriggerOne.TextColor = System.Drawing.Color.Black;
            this.btnTriggerOne.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTriggerOne.UseVisualStyleBackColor = false;
            this.btnTriggerOne.Click += new System.EventHandler(this.btnTriggerOne_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(5, 225);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(407, 32);
            this.label4.TabIndex = 71;
            this.label4.Text = "Trigger Mode";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numTrigger
            // 
            this.numTrigger.AutoRepeatAccelDeltaMs = -5;
            this.numTrigger.AutoRepeatAccelerate = true;
            this.numTrigger.AutoRepeatEnabled = true;
            this.numTrigger.AutoRepeatInitialDelay = 400;
            this.numTrigger.AutoRepeatInterval = 60;
            this.numTrigger.AutoRepeatMinInterval = 20;
            this.numTrigger.AutoShowTextbox = true;
            this.numTrigger.AutoSizeTextbox = true;
            this.numTrigger.BackColor = System.Drawing.Color.White;
            this.numTrigger.BarLeftGap = 20;
            this.numTrigger.BarRightGap = 6;
            this.numTrigger.ChromeGap = 1;
            this.numTrigger.ChromeWidthRatio = 0.14F;
            this.numTrigger.ColorBorder = System.Drawing.Color.LightGray;
            this.numTrigger.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.numTrigger.ColorScale = System.Drawing.Color.LightGray;
            this.numTrigger.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.numTrigger.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.numTrigger.ColorTrack = System.Drawing.Color.LightGray;
            this.numTrigger.Decimals = 0;
            this.numTrigger.DisabledDesaturateMix = 0.3F;
            this.numTrigger.DisabledDimFactor = 0.9F;
            this.numTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTrigger.EdgePadding = 2;
            this.numTrigger.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numTrigger.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.numTrigger.KeyboardStep = 1F;
            this.numTrigger.Location = new System.Drawing.Point(5, 152);
            this.numTrigger.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.numTrigger.MatchTextboxFontToThumb = true;
            this.numTrigger.Max = 4F;
            this.numTrigger.MaxTextboxWidth = 0;
            this.numTrigger.MaxThumb = 1000;
            this.numTrigger.MaxTrackHeight = 1000;
            this.numTrigger.Min = 1F;
            this.numTrigger.MinChromeWidth = 64;
            this.numTrigger.MinimumSize = new System.Drawing.Size(140, 36);
            this.numTrigger.MinTextboxWidth = 32;
            this.numTrigger.MinThumb = 30;
            this.numTrigger.MinTrackHeight = 8;
            this.numTrigger.Name = "numTrigger";
            this.numTrigger.Radius = 8;
            this.numTrigger.ShowValueOnThumb = true;
            this.numTrigger.Size = new System.Drawing.Size(407, 53);
            this.numTrigger.SnapToStep = true;
            this.numTrigger.StartWithTextboxHidden = true;
            this.numTrigger.Step = 1F;
            this.numTrigger.TabIndex = 70;
            this.numTrigger.TextboxFontSize = 22F;
            this.numTrigger.TextboxSidePadding = 10;
            this.numTrigger.TextboxWidth = 600;
            this.numTrigger.ThumbDiameterRatio = 1F;
            this.numTrigger.ThumbValueBold = true;
            this.numTrigger.ThumbValueFontScale = 1.5F;
            this.numTrigger.ThumbValuePadding = 0;
            this.numTrigger.TightEdges = true;
            this.numTrigger.TrackHeightRatio = 0.45F;
            this.numTrigger.TrackWidthRatio = 1F;
            this.numTrigger.UnitText = "";
            this.numTrigger.Value = 1F;
            this.numTrigger.WheelStep = 1F;
            this.numTrigger.ValueChanged += new System.Action<float>(this.numTrigger_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(5, 127);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(407, 25);
            this.label1.TabIndex = 59;
            this.label1.Text = "Num Follow Chart";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(5, 20);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(407, 32);
            this.label12.TabIndex = 38;
            this.label12.Text = "Camera Mode";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnMulti, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSingle, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 52);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(407, 55);
            this.tableLayoutPanel2.TabIndex = 39;
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
            this.btnMulti.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMulti.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMulti.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMulti.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMulti.Corner = BeeGlobal.Corner.Both;
            this.btnMulti.DebounceResizeMs = 16;
            this.btnMulti.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.btnMulti.Location = new System.Drawing.Point(206, 8);
            this.btnMulti.Multiline = false;
            this.btnMulti.Name = "btnMulti";
            this.btnMulti.Size = new System.Drawing.Size(193, 39);
            this.btnMulti.TabIndex = 9;
            this.btnMulti.Text = "Multi";
            this.btnMulti.TextColor = System.Drawing.Color.Black;
            this.btnMulti.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMulti.UseVisualStyleBackColor = false;
            this.btnMulti.Click += new System.EventHandler(this.btnMulti_Click);
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
            this.btnSingle.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSingle.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSingle.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnSingle.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSingle.Corner = BeeGlobal.Corner.Both;
            this.btnSingle.DebounceResizeMs = 16;
            this.btnSingle.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.btnSingle.Location = new System.Drawing.Point(8, 8);
            this.btnSingle.Multiline = false;
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(192, 39);
            this.btnSingle.TabIndex = 7;
            this.btnSingle.Text = "Single";
            this.btnSingle.TextColor = System.Drawing.Color.Black;
            this.btnSingle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSingle.UseVisualStyleBackColor = false;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoFont = true;
            this.btnSave.AutoFontHeightRatio = 0.75F;
            this.btnSave.AutoFontMax = 100F;
            this.btnSave.AutoFontMin = 6F;
            this.btnSave.AutoFontWidthRatio = 0.92F;
            this.btnSave.AutoImage = true;
            this.btnSave.AutoImageMaxRatio = 0.75F;
            this.btnSave.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSave.AutoImageTint = true;
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.BorderColor = System.Drawing.Color.Transparent;
            this.btnSave.BorderRadius = 5;
            this.btnSave.BorderSize = 1;
            this.btnSave.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSave.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSave.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnSave.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSave.Corner = BeeGlobal.Corner.Both;
            this.btnSave.DebounceResizeMs = 16;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.92969F);
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = null;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnSave.Location = new System.Drawing.Point(0, 576);
            this.btnSave.Multiline = false;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(431, 66);
            this.btnSave.TabIndex = 71;
            this.btnSave.Text = "Save Config";
            this.btnSave.TextColor = System.Drawing.Color.Black;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // GeneralSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 642);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GeneralSetting";
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
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
        private RJButton btnSingle;
        private RJButton btnMulti;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private AdjustBarEx numTrigger;
        private RJButton btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnTriggerMulti;
        private RJButton btnTriggerOne;
        private System.Windows.Forms.Label label4;
    }
}