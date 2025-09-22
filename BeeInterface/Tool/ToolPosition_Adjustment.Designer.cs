using BeeGlobal;

namespace BeeInterface
{
    partial class ToolPosition_Adjustment
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolPosition_Adjustment));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tmClear = new System.Windows.Forms.Timer(this.components);
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.btnTest = new BeeInterface.RJButton();
            this.lay3 = new System.Windows.Forms.TableLayoutPanel();
            this.imgTemp = new System.Windows.Forms.PictureBox();
            this.btnLearning = new BeeInterface.RJButton();
            this.lay2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCrop = new BeeInterface.RJButton();
            this.btnCropArea = new BeeInterface.RJButton();
            this.lay1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.rjButton2 = new BeeInterface.RJButton();
            this.rjButton5 = new BeeInterface.RJButton();
            this.trackBar21 = new BeeInterface.TrackBar2();
            this.label4 = new System.Windows.Forms.Label();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.trackAngle = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNormal = new BeeInterface.RJButton();
            this.btnHighSpeed = new BeeInterface.RJButton();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.ckSIMD = new BeeInterface.RJButton();
            this.ckSubPixel = new BeeInterface.RJButton();
            this.ckBitwiseNot = new BeeInterface.RJButton();
            this.trackMaxOverLap = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage4.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.lay3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).BeginInit();
            this.lay2.SuspendLayout();
            this.lay1.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // threadProcess
            // 
            this.threadProcess.WorkerReportsProgress = true;
            this.threadProcess.WorkerSupportsCancellation = true;
            // 
            // tmClear
            // 
            this.tmClear.Tick += new System.EventHandler(this.tmClear_Tick);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel9);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(492, 854);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(500, 892);
            this.tabControl2.TabIndex = 17;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Controls.Add(this.rjButton2);
            this.tabPage3.Controls.Add(this.rjButton5);
            this.tabPage3.Controls.Add(this.trackBar21);
            this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(492, 854);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.trackAngle, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.lay3, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lay2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lay1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(486, 848);
            this.tableLayoutPanel1.TabIndex = 25;
            // 
            // trackScore
            // 
            this.trackScore.AutoShowTextbox = true;
            this.trackScore.AutoSizeTextbox = true;
            this.trackScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.trackScore.BarLeftGap = 20;
            this.trackScore.BarRightGap = 6;
            this.trackScore.ChromeGap = 8;
            this.trackScore.ChromeWidthRatio = 0.14F;
            this.trackScore.ColorBorder = System.Drawing.Color.LightGray;
            this.trackScore.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackScore.ColorScale = System.Drawing.Color.LightGray;
            this.trackScore.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorTrack = System.Drawing.Color.LightGray;
            this.trackScore.Decimals = 0;
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(5, 620);
            this.trackScore.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.trackScore.MatchTextboxFontToThumb = true;
            this.trackScore.Max = 100F;
            this.trackScore.MaxTextboxWidth = 0;
            this.trackScore.MaxThumb = 1000;
            this.trackScore.MaxTrackHeight = 1000;
            this.trackScore.Min = 0F;
            this.trackScore.MinChromeWidth = 64;
            this.trackScore.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackScore.MinTextboxWidth = 32;
            this.trackScore.MinThumb = 30;
            this.trackScore.MinTrackHeight = 8;
            this.trackScore.Name = "trackScore";
            this.trackScore.Radius = 8;
            this.trackScore.ShowValueOnThumb = true;
            this.trackScore.Size = new System.Drawing.Size(476, 49);
            this.trackScore.SnapToStep = true;
            this.trackScore.StartWithTextboxHidden = true;
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 69;
            this.trackScore.TextboxFontSize = 20F;
            this.trackScore.TextboxSidePadding = 10;
            this.trackScore.TextboxWidth = 600;
            this.trackScore.ThumbDiameterRatio = 2F;
            this.trackScore.ThumbValueBold = true;
            this.trackScore.ThumbValueFontScale = 1.5F;
            this.trackScore.ThumbValuePadding = 0;
            this.trackScore.TightEdges = true;
            this.trackScore.TrackHeightRatio = 0.45F;
            this.trackScore.TrackWidthRatio = 1F;
            this.trackScore.UnitText = "";
            this.trackScore.Value = 0F;
            this.trackScore.WheelStep = 1F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // btnTest
            // 
            this.btnTest.AutoFont = false;
            this.btnTest.AutoFontHeightRatio = 0.75F;
            this.btnTest.AutoFontMax = 100F;
            this.btnTest.AutoFontMin = 6F;
            this.btnTest.AutoFontWidthRatio = 0.92F;
            this.btnTest.AutoImage = true;
            this.btnTest.AutoImageMaxRatio = 0.75F;
            this.btnTest.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTest.AutoImageTint = true;
            this.btnTest.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnTest.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnTest.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnTest.BorderRadius = 10;
            this.btnTest.BorderSize = 1;
            this.btnTest.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.DebounceResizeMs = 16;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.Image = null;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.ImageDisabled = null;
            this.btnTest.ImageHover = null;
            this.btnTest.ImageNormal = null;
            this.btnTest.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTest.ImagePressed = null;
            this.btnTest.ImageTextSpacing = 6;
            this.btnTest.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTest.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTest.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTest.ImageTintOpacity = 0.5F;
            this.btnTest.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTest.IsCLick = false;
            this.btnTest.IsNotChange = true;
            this.btnTest.IsRect = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(10, 679);
            this.btnTest.Margin = new System.Windows.Forms.Padding(10, 10, 10, 2);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(466, 88);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lay3
            // 
            this.lay3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.lay3.ColumnCount = 2;
            this.lay3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.31535F));
            this.lay3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.68465F));
            this.lay3.Controls.Add(this.imgTemp, 0, 0);
            this.lay3.Controls.Add(this.btnLearning, 1, 0);
            this.lay3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay3.Location = new System.Drawing.Point(5, 368);
            this.lay3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lay3.Name = "lay3";
            this.lay3.Padding = new System.Windows.Forms.Padding(5);
            this.lay3.RowCount = 1;
            this.lay3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay3.Size = new System.Drawing.Size(476, 200);
            this.lay3.TabIndex = 43;
            // 
            // imgTemp
            // 
            this.imgTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.imgTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgTemp.Location = new System.Drawing.Point(10, 10);
            this.imgTemp.Margin = new System.Windows.Forms.Padding(5);
            this.imgTemp.Name = "imgTemp";
            this.imgTemp.Size = new System.Drawing.Size(289, 180);
            this.imgTemp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgTemp.TabIndex = 31;
            this.imgTemp.TabStop = false;
            // 
            // btnLearning
            // 
            this.btnLearning.AutoFont = false;
            this.btnLearning.AutoFontHeightRatio = 0.75F;
            this.btnLearning.AutoFontMax = 100F;
            this.btnLearning.AutoFontMin = 6F;
            this.btnLearning.AutoFontWidthRatio = 0.92F;
            this.btnLearning.AutoImage = true;
            this.btnLearning.AutoImageMaxRatio = 0.75F;
            this.btnLearning.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLearning.AutoImageTint = true;
            this.btnLearning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLearning.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLearning.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnLearning.BorderRadius = 10;
            this.btnLearning.BorderSize = 1;
            this.btnLearning.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLearning.Corner = BeeGlobal.Corner.Both;
            this.btnLearning.DebounceResizeMs = 16;
            this.btnLearning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLearning.FlatAppearance.BorderSize = 0;
            this.btnLearning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLearning.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLearning.ForeColor = System.Drawing.Color.Black;
            this.btnLearning.Image = null;
            this.btnLearning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLearning.ImageDisabled = null;
            this.btnLearning.ImageHover = null;
            this.btnLearning.ImageNormal = null;
            this.btnLearning.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLearning.ImagePressed = null;
            this.btnLearning.ImageTextSpacing = 6;
            this.btnLearning.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLearning.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLearning.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLearning.ImageTintOpacity = 0.5F;
            this.btnLearning.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLearning.IsCLick = false;
            this.btnLearning.IsNotChange = true;
            this.btnLearning.IsRect = false;
            this.btnLearning.IsUnGroup = true;
            this.btnLearning.Location = new System.Drawing.Point(314, 15);
            this.btnLearning.Margin = new System.Windows.Forms.Padding(10);
            this.btnLearning.Multiline = false;
            this.btnLearning.Name = "btnLearning";
            this.btnLearning.Size = new System.Drawing.Size(147, 170);
            this.btnLearning.TabIndex = 5;
            this.btnLearning.Text = "Teach Sample";
            this.btnLearning.TextColor = System.Drawing.Color.Black;
            this.btnLearning.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLearning.UseVisualStyleBackColor = false;
            this.btnLearning.Click += new System.EventHandler(this.btnLearning_Click);
            // 
            // lay2
            // 
            this.lay2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.lay2.ColumnCount = 2;
            this.lay2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.lay2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.lay2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.lay2.Controls.Add(this.btnCrop, 0, 0);
            this.lay2.Controls.Add(this.btnCropArea, 1, 0);
            this.lay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay2.Location = new System.Drawing.Point(5, 159);
            this.lay2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lay2.Name = "lay2";
            this.lay2.Padding = new System.Windows.Forms.Padding(5);
            this.lay2.RowCount = 1;
            this.lay2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay2.Size = new System.Drawing.Size(476, 55);
            this.lay2.TabIndex = 41;
            // 
            // btnCrop
            // 
            this.btnCrop.AutoFont = false;
            this.btnCrop.AutoFontHeightRatio = 0.75F;
            this.btnCrop.AutoFontMax = 100F;
            this.btnCrop.AutoFontMin = 6F;
            this.btnCrop.AutoFontWidthRatio = 0.92F;
            this.btnCrop.AutoImage = true;
            this.btnCrop.AutoImageMaxRatio = 0.75F;
            this.btnCrop.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCrop.AutoImageTint = true;
            this.btnCrop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCrop.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCrop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCrop.BackgroundImage")));
            this.btnCrop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCrop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCrop.BorderRadius = 10;
            this.btnCrop.BorderSize = 1;
            this.btnCrop.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCrop.Corner = BeeGlobal.Corner.Left;
            this.btnCrop.DebounceResizeMs = 16;
            this.btnCrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCrop.FlatAppearance.BorderSize = 0;
            this.btnCrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrop.ForeColor = System.Drawing.Color.Black;
            this.btnCrop.Image = null;
            this.btnCrop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCrop.ImageDisabled = null;
            this.btnCrop.ImageHover = null;
            this.btnCrop.ImageNormal = null;
            this.btnCrop.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCrop.ImagePressed = null;
            this.btnCrop.ImageTextSpacing = 6;
            this.btnCrop.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCrop.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCrop.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCrop.ImageTintOpacity = 0.5F;
            this.btnCrop.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCrop.IsCLick = false;
            this.btnCrop.IsNotChange = false;
            this.btnCrop.IsRect = false;
            this.btnCrop.IsUnGroup = false;
            this.btnCrop.Location = new System.Drawing.Point(5, 5);
            this.btnCrop.Margin = new System.Windows.Forms.Padding(0);
            this.btnCrop.Multiline = false;
            this.btnCrop.Name = "btnCrop";
            this.btnCrop.Size = new System.Drawing.Size(233, 45);
            this.btnCrop.TabIndex = 2;
            this.btnCrop.Text = "Area Temp";
            this.btnCrop.TextColor = System.Drawing.Color.Black;
            this.btnCrop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCrop.UseVisualStyleBackColor = false;
            this.btnCrop.Click += new System.EventHandler(this.btnCropRect_Click);
            // 
            // btnCropArea
            // 
            this.btnCropArea.AutoFont = false;
            this.btnCropArea.AutoFontHeightRatio = 0.75F;
            this.btnCropArea.AutoFontMax = 100F;
            this.btnCropArea.AutoFontMin = 6F;
            this.btnCropArea.AutoFontWidthRatio = 0.92F;
            this.btnCropArea.AutoImage = true;
            this.btnCropArea.AutoImageMaxRatio = 0.75F;
            this.btnCropArea.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropArea.AutoImageTint = true;
            this.btnCropArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropArea.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropArea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropArea.BackgroundImage")));
            this.btnCropArea.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropArea.BorderRadius = 10;
            this.btnCropArea.BorderSize = 1;
            this.btnCropArea.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropArea.Corner = BeeGlobal.Corner.Right;
            this.btnCropArea.DebounceResizeMs = 16;
            this.btnCropArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropArea.ForeColor = System.Drawing.Color.Black;
            this.btnCropArea.Image = null;
            this.btnCropArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropArea.ImageDisabled = null;
            this.btnCropArea.ImageHover = null;
            this.btnCropArea.ImageNormal = null;
            this.btnCropArea.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropArea.ImagePressed = null;
            this.btnCropArea.ImageTextSpacing = 6;
            this.btnCropArea.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropArea.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropArea.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropArea.ImageTintOpacity = 0.5F;
            this.btnCropArea.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropArea.IsCLick = true;
            this.btnCropArea.IsNotChange = false;
            this.btnCropArea.IsRect = false;
            this.btnCropArea.IsUnGroup = false;
            this.btnCropArea.Location = new System.Drawing.Point(238, 5);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropArea.Multiline = false;
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(233, 45);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
            // 
            // lay1
            // 
            this.lay1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lay1.ColumnCount = 2;
            this.lay1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.lay1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.lay1.Controls.Add(this.btnCropFull, 1, 0);
            this.lay1.Controls.Add(this.btnCropHalt, 0, 0);
            this.lay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay1.Location = new System.Drawing.Point(5, 52);
            this.lay1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lay1.Name = "lay1";
            this.lay1.RowCount = 1;
            this.lay1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay1.Size = new System.Drawing.Size(476, 55);
            this.lay1.TabIndex = 39;
            // 
            // btnCropFull
            // 
            this.btnCropFull.AutoFont = false;
            this.btnCropFull.AutoFontHeightRatio = 0.75F;
            this.btnCropFull.AutoFontMax = 100F;
            this.btnCropFull.AutoFontMin = 6F;
            this.btnCropFull.AutoFontWidthRatio = 0.92F;
            this.btnCropFull.AutoImage = true;
            this.btnCropFull.AutoImageMaxRatio = 0.75F;
            this.btnCropFull.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropFull.AutoImageTint = true;
            this.btnCropFull.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropFull.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropFull.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.DebounceResizeMs = 16;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropFull.ForeColor = System.Drawing.Color.Black;
            this.btnCropFull.Image = null;
            this.btnCropFull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropFull.ImageDisabled = null;
            this.btnCropFull.ImageHover = null;
            this.btnCropFull.ImageNormal = null;
            this.btnCropFull.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropFull.ImagePressed = null;
            this.btnCropFull.ImageTextSpacing = 6;
            this.btnCropFull.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropFull.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropFull.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropFull.ImageTintOpacity = 0.5F;
            this.btnCropFull.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropFull.IsCLick = false;
            this.btnCropFull.IsNotChange = false;
            this.btnCropFull.IsRect = false;
            this.btnCropFull.IsUnGroup = false;
            this.btnCropFull.Location = new System.Drawing.Point(238, 5);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.btnCropFull.Multiline = false;
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(233, 45);
            this.btnCropFull.TabIndex = 3;
            this.btnCropFull.Text = "Partial";
            this.btnCropFull.TextColor = System.Drawing.Color.Black;
            this.btnCropFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropFull.UseVisualStyleBackColor = false;
            this.btnCropFull.Click += new System.EventHandler(this.btnCropFull_Click);
            // 
            // btnCropHalt
            // 
            this.btnCropHalt.AutoFont = false;
            this.btnCropHalt.AutoFontHeightRatio = 0.75F;
            this.btnCropHalt.AutoFontMax = 100F;
            this.btnCropHalt.AutoFontMin = 6F;
            this.btnCropHalt.AutoFontWidthRatio = 0.92F;
            this.btnCropHalt.AutoImage = true;
            this.btnCropHalt.AutoImageMaxRatio = 0.75F;
            this.btnCropHalt.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropHalt.AutoImageTint = true;
            this.btnCropHalt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropHalt.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.DebounceResizeMs = 16;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropHalt.ForeColor = System.Drawing.Color.Black;
            this.btnCropHalt.Image = null;
            this.btnCropHalt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropHalt.ImageDisabled = null;
            this.btnCropHalt.ImageHover = null;
            this.btnCropHalt.ImageNormal = null;
            this.btnCropHalt.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropHalt.ImagePressed = null;
            this.btnCropHalt.ImageTextSpacing = 6;
            this.btnCropHalt.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropHalt.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropHalt.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropHalt.ImageTintOpacity = 0.5F;
            this.btnCropHalt.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropHalt.IsCLick = true;
            this.btnCropHalt.IsNotChange = false;
            this.btnCropHalt.IsRect = false;
            this.btnCropHalt.IsUnGroup = false;
            this.btnCropHalt.Location = new System.Drawing.Point(5, 5);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.btnCropHalt.Multiline = false;
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(233, 45);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // rjButton2
            // 
            this.rjButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rjButton2.AutoFont = false;
            this.rjButton2.AutoFontHeightRatio = 0.75F;
            this.rjButton2.AutoFontMax = 100F;
            this.rjButton2.AutoFontMin = 6F;
            this.rjButton2.AutoFontWidthRatio = 0.92F;
            this.rjButton2.AutoImage = true;
            this.rjButton2.AutoImageMaxRatio = 0.75F;
            this.rjButton2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton2.AutoImageTint = true;
            this.rjButton2.BackColor = System.Drawing.SystemColors.Control;
            this.rjButton2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.rjButton2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton2.BackgroundImage")));
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 5;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton2.Corner = BeeGlobal.Corner.Both;
            this.rjButton2.DebounceResizeMs = 16;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.Image = null;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.ImageDisabled = null;
            this.rjButton2.ImageHover = null;
            this.rjButton2.ImageNormal = null;
            this.rjButton2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton2.ImagePressed = null;
            this.rjButton2.ImageTextSpacing = 6;
            this.rjButton2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton2.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton2.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton2.ImageTintOpacity = 0.5F;
            this.rjButton2.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = true;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(213, 3757);
            this.rjButton2.Multiline = false;
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(171, 40);
            this.rjButton2.TabIndex = 24;
            this.rjButton2.Text = "Cancel";
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // rjButton5
            // 
            this.rjButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rjButton5.AutoFont = false;
            this.rjButton5.AutoFontHeightRatio = 0.75F;
            this.rjButton5.AutoFontMax = 100F;
            this.rjButton5.AutoFontMin = 6F;
            this.rjButton5.AutoFontWidthRatio = 0.92F;
            this.rjButton5.AutoImage = true;
            this.rjButton5.AutoImageMaxRatio = 0.75F;
            this.rjButton5.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton5.AutoImageTint = true;
            this.rjButton5.BackColor = System.Drawing.SystemColors.Control;
            this.rjButton5.BackgroundColor = System.Drawing.SystemColors.Control;
            this.rjButton5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton5.BackgroundImage")));
            this.rjButton5.BorderColor = System.Drawing.Color.Silver;
            this.rjButton5.BorderRadius = 5;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton5.Corner = BeeGlobal.Corner.Both;
            this.rjButton5.DebounceResizeMs = 16;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.Image = null;
            this.rjButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton5.ImageDisabled = null;
            this.rjButton5.ImageHover = null;
            this.rjButton5.ImageNormal = null;
            this.rjButton5.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton5.ImagePressed = null;
            this.rjButton5.ImageTextSpacing = 6;
            this.rjButton5.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton5.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton5.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton5.ImageTintOpacity = 0.5F;
            this.rjButton5.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton5.IsCLick = true;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = false;
            this.rjButton5.IsUnGroup = false;
            this.rjButton5.Location = new System.Drawing.Point(6, 3757);
            this.rjButton5.Multiline = false;
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(137, 40);
            this.rjButton5.TabIndex = 23;
            this.rjButton5.Text = "OK";
            this.rjButton5.TextColor = System.Drawing.Color.Black;
            this.rjButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton5.UseVisualStyleBackColor = false;
            // 
            // trackBar21
            // 
            this.trackBar21.ColorTrack = System.Drawing.Color.Gray;
            this.trackBar21.Location = new System.Drawing.Point(129, 1934);
            this.trackBar21.Margin = new System.Windows.Forms.Padding(21, 28, 21, 28);
            this.trackBar21.Max = 100F;
            this.trackBar21.Min = 0F;
            this.trackBar21.Name = "trackBar21";
            this.trackBar21.Size = new System.Drawing.Size(1647, 397);
            this.trackBar21.Step = 0F;
            this.trackBar21.TabIndex = 22;
            this.trackBar21.Value = 10F;
            this.trackBar21.ValueScore = 0F;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(532, 312);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 20);
            this.label4.TabIndex = 15;
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 892);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(500, 57);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(5, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(476, 32);
            this.label8.TabIndex = 70;
            this.label8.Text = "Search Range";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(5, 127);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(476, 32);
            this.label9.TabIndex = 71;
            this.label9.Text = "Choose Area";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(5, 336);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(476, 32);
            this.label12.TabIndex = 72;
            this.label12.Text = "Training Sample";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(5, 588);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(476, 32);
            this.label13.TabIndex = 73;
            this.label13.Text = "Score";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(5, 234);
            this.label15.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(476, 32);
            this.label15.TabIndex = 74;
            this.label15.Text = "Angle Range";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackAngle
            // 
            this.trackAngle.AutoShowTextbox = true;
            this.trackAngle.AutoSizeTextbox = true;
            this.trackAngle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.trackAngle.BarLeftGap = 20;
            this.trackAngle.BarRightGap = 6;
            this.trackAngle.ChromeGap = 8;
            this.trackAngle.ChromeWidthRatio = 0.14F;
            this.trackAngle.ColorBorder = System.Drawing.Color.LightGray;
            this.trackAngle.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackAngle.ColorScale = System.Drawing.Color.LightGray;
            this.trackAngle.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackAngle.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackAngle.ColorTrack = System.Drawing.Color.LightGray;
            this.trackAngle.Decimals = 0;
            this.trackAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackAngle.EdgePadding = 2;
            this.trackAngle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackAngle.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackAngle.KeyboardStep = 1F;
            this.trackAngle.Location = new System.Drawing.Point(5, 266);
            this.trackAngle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.trackAngle.MatchTextboxFontToThumb = true;
            this.trackAngle.Max = 100F;
            this.trackAngle.MaxTextboxWidth = 1;
            this.trackAngle.MaxThumb = 1000;
            this.trackAngle.MaxTrackHeight = 1000;
            this.trackAngle.Min = 0F;
            this.trackAngle.MinChromeWidth = 64;
            this.trackAngle.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackAngle.MinTextboxWidth = 32;
            this.trackAngle.MinThumb = 30;
            this.trackAngle.MinTrackHeight = 8;
            this.trackAngle.Name = "trackAngle";
            this.trackAngle.Radius = 8;
            this.trackAngle.ShowValueOnThumb = true;
            this.trackAngle.Size = new System.Drawing.Size(476, 50);
            this.trackAngle.SnapToStep = true;
            this.trackAngle.StartWithTextboxHidden = true;
            this.trackAngle.Step = 1F;
            this.trackAngle.TabIndex = 75;
            this.trackAngle.TextboxFontSize = 20F;
            this.trackAngle.TextboxSidePadding = 10;
            this.trackAngle.TextboxWidth = 600;
            this.trackAngle.ThumbDiameterRatio = 2F;
            this.trackAngle.ThumbValueBold = true;
            this.trackAngle.ThumbValueFontScale = 1.5F;
            this.trackAngle.ThumbValuePadding = 0;
            this.trackAngle.TightEdges = true;
            this.trackAngle.TrackHeightRatio = 0.45F;
            this.trackAngle.TrackWidthRatio = 1F;
            this.trackAngle.UnitText = "";
            this.trackAngle.Value = 0F;
            this.trackAngle.WheelStep = 1F;
            this.trackAngle.ValueChanged += new System.Action<float>(this.trackAngle_ValueChanged);
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.btnHighSpeed, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.btnNormal, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 151);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(478, 55);
            this.tableLayoutPanel13.TabIndex = 51;
            // 
            // btnNormal
            // 
            this.btnNormal.AutoFont = false;
            this.btnNormal.AutoFontHeightRatio = 0.75F;
            this.btnNormal.AutoFontMax = 100F;
            this.btnNormal.AutoFontMin = 6F;
            this.btnNormal.AutoFontWidthRatio = 0.92F;
            this.btnNormal.AutoImage = true;
            this.btnNormal.AutoImageMaxRatio = 0.75F;
            this.btnNormal.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnNormal.AutoImageTint = true;
            this.btnNormal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnNormal.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnNormal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNormal.BackgroundImage")));
            this.btnNormal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNormal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnNormal.BorderRadius = 10;
            this.btnNormal.BorderSize = 1;
            this.btnNormal.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnNormal.Corner = BeeGlobal.Corner.Left;
            this.btnNormal.DebounceResizeMs = 16;
            this.btnNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNormal.FlatAppearance.BorderSize = 0;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.btnNormal.IsCLick = true;
            this.btnNormal.IsNotChange = false;
            this.btnNormal.IsRect = false;
            this.btnNormal.IsUnGroup = false;
            this.btnNormal.Location = new System.Drawing.Point(8, 8);
            this.btnNormal.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnNormal.Multiline = false;
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(231, 39);
            this.btnNormal.TabIndex = 2;
            this.btnNormal.Text = "Normal";
            this.btnNormal.TextColor = System.Drawing.Color.Black;
            this.btnNormal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNormal.UseVisualStyleBackColor = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnHighSpeed
            // 
            this.btnHighSpeed.AutoFont = false;
            this.btnHighSpeed.AutoFontHeightRatio = 0.75F;
            this.btnHighSpeed.AutoFontMax = 100F;
            this.btnHighSpeed.AutoFontMin = 6F;
            this.btnHighSpeed.AutoFontWidthRatio = 0.92F;
            this.btnHighSpeed.AutoImage = true;
            this.btnHighSpeed.AutoImageMaxRatio = 0.75F;
            this.btnHighSpeed.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnHighSpeed.AutoImageTint = true;
            this.btnHighSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnHighSpeed.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnHighSpeed.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHighSpeed.BackgroundImage")));
            this.btnHighSpeed.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnHighSpeed.BorderRadius = 10;
            this.btnHighSpeed.BorderSize = 1;
            this.btnHighSpeed.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnHighSpeed.Corner = BeeGlobal.Corner.Right;
            this.btnHighSpeed.DebounceResizeMs = 16;
            this.btnHighSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHighSpeed.FlatAppearance.BorderSize = 0;
            this.btnHighSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHighSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHighSpeed.ForeColor = System.Drawing.Color.Black;
            this.btnHighSpeed.Image = null;
            this.btnHighSpeed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHighSpeed.ImageDisabled = null;
            this.btnHighSpeed.ImageHover = null;
            this.btnHighSpeed.ImageNormal = null;
            this.btnHighSpeed.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnHighSpeed.ImagePressed = null;
            this.btnHighSpeed.ImageTextSpacing = 6;
            this.btnHighSpeed.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnHighSpeed.ImageTintHover = System.Drawing.Color.Empty;
            this.btnHighSpeed.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnHighSpeed.ImageTintOpacity = 0.5F;
            this.btnHighSpeed.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnHighSpeed.IsCLick = false;
            this.btnHighSpeed.IsNotChange = false;
            this.btnHighSpeed.IsRect = false;
            this.btnHighSpeed.IsUnGroup = false;
            this.btnHighSpeed.Location = new System.Drawing.Point(239, 8);
            this.btnHighSpeed.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnHighSpeed.Multiline = false;
            this.btnHighSpeed.Name = "btnHighSpeed";
            this.btnHighSpeed.Size = new System.Drawing.Size(231, 39);
            this.btnHighSpeed.TabIndex = 3;
            this.btnHighSpeed.Text = "High Speed";
            this.btnHighSpeed.TextColor = System.Drawing.Color.Black;
            this.btnHighSpeed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHighSpeed.UseVisualStyleBackColor = false;
            this.btnHighSpeed.Click += new System.EventHandler(this.btnHighSpeed_Click);
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tableLayoutPanel14.ColumnCount = 3;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel14.Controls.Add(this.ckBitwiseNot, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.ckSubPixel, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.ckSIMD, 0, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 258);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(476, 55);
            this.tableLayoutPanel14.TabIndex = 53;
            // 
            // ckSIMD
            // 
            this.ckSIMD.AutoFont = false;
            this.ckSIMD.AutoFontHeightRatio = 0.75F;
            this.ckSIMD.AutoFontMax = 100F;
            this.ckSIMD.AutoFontMin = 6F;
            this.ckSIMD.AutoFontWidthRatio = 0.92F;
            this.ckSIMD.AutoImage = true;
            this.ckSIMD.AutoImageMaxRatio = 0.75F;
            this.ckSIMD.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.ckSIMD.AutoImageTint = true;
            this.ckSIMD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckSIMD.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckSIMD.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckSIMD.BackgroundImage")));
            this.ckSIMD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ckSIMD.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckSIMD.BorderRadius = 10;
            this.ckSIMD.BorderSize = 1;
            this.ckSIMD.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.ckSIMD.Corner = BeeGlobal.Corner.Both;
            this.ckSIMD.DebounceResizeMs = 16;
            this.ckSIMD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckSIMD.FlatAppearance.BorderSize = 0;
            this.ckSIMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckSIMD.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckSIMD.ForeColor = System.Drawing.Color.Black;
            this.ckSIMD.Image = null;
            this.ckSIMD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckSIMD.ImageDisabled = null;
            this.ckSIMD.ImageHover = null;
            this.ckSIMD.ImageNormal = null;
            this.ckSIMD.ImagePadding = new System.Windows.Forms.Padding(1);
            this.ckSIMD.ImagePressed = null;
            this.ckSIMD.ImageTextSpacing = 6;
            this.ckSIMD.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.ckSIMD.ImageTintHover = System.Drawing.Color.Empty;
            this.ckSIMD.ImageTintNormal = System.Drawing.Color.Empty;
            this.ckSIMD.ImageTintOpacity = 0.5F;
            this.ckSIMD.ImageTintPressed = System.Drawing.Color.Empty;
            this.ckSIMD.IsCLick = true;
            this.ckSIMD.IsNotChange = false;
            this.ckSIMD.IsRect = false;
            this.ckSIMD.IsUnGroup = true;
            this.ckSIMD.Location = new System.Drawing.Point(10, 8);
            this.ckSIMD.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckSIMD.Multiline = false;
            this.ckSIMD.Name = "ckSIMD";
            this.ckSIMD.Size = new System.Drawing.Size(140, 39);
            this.ckSIMD.TabIndex = 2;
            this.ckSIMD.Text = "SIMD";
            this.ckSIMD.TextColor = System.Drawing.Color.Black;
            this.ckSIMD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckSIMD.UseVisualStyleBackColor = false;
            this.ckSIMD.Click += new System.EventHandler(this.ckSIMD_Click);
            // 
            // ckSubPixel
            // 
            this.ckSubPixel.AutoFont = false;
            this.ckSubPixel.AutoFontHeightRatio = 0.75F;
            this.ckSubPixel.AutoFontMax = 100F;
            this.ckSubPixel.AutoFontMin = 6F;
            this.ckSubPixel.AutoFontWidthRatio = 0.92F;
            this.ckSubPixel.AutoImage = true;
            this.ckSubPixel.AutoImageMaxRatio = 0.75F;
            this.ckSubPixel.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.ckSubPixel.AutoImageTint = true;
            this.ckSubPixel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckSubPixel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckSubPixel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckSubPixel.BackgroundImage")));
            this.ckSubPixel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ckSubPixel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckSubPixel.BorderRadius = 10;
            this.ckSubPixel.BorderSize = 1;
            this.ckSubPixel.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.ckSubPixel.Corner = BeeGlobal.Corner.Both;
            this.ckSubPixel.DebounceResizeMs = 16;
            this.ckSubPixel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckSubPixel.FlatAppearance.BorderSize = 0;
            this.ckSubPixel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckSubPixel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckSubPixel.ForeColor = System.Drawing.Color.Black;
            this.ckSubPixel.Image = null;
            this.ckSubPixel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckSubPixel.ImageDisabled = null;
            this.ckSubPixel.ImageHover = null;
            this.ckSubPixel.ImageNormal = null;
            this.ckSubPixel.ImagePadding = new System.Windows.Forms.Padding(1);
            this.ckSubPixel.ImagePressed = null;
            this.ckSubPixel.ImageTextSpacing = 6;
            this.ckSubPixel.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.ckSubPixel.ImageTintHover = System.Drawing.Color.Empty;
            this.ckSubPixel.ImageTintNormal = System.Drawing.Color.Empty;
            this.ckSubPixel.ImageTintOpacity = 0.5F;
            this.ckSubPixel.ImageTintPressed = System.Drawing.Color.Empty;
            this.ckSubPixel.IsCLick = true;
            this.ckSubPixel.IsNotChange = false;
            this.ckSubPixel.IsRect = false;
            this.ckSubPixel.IsUnGroup = true;
            this.ckSubPixel.Location = new System.Drawing.Point(160, 8);
            this.ckSubPixel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckSubPixel.Multiline = false;
            this.ckSubPixel.Name = "ckSubPixel";
            this.ckSubPixel.Size = new System.Drawing.Size(154, 39);
            this.ckSubPixel.TabIndex = 4;
            this.ckSubPixel.Text = "SubPixel";
            this.ckSubPixel.TextColor = System.Drawing.Color.Black;
            this.ckSubPixel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckSubPixel.UseVisualStyleBackColor = false;
            this.ckSubPixel.Click += new System.EventHandler(this.ckSubPixel_Click);
            // 
            // ckBitwiseNot
            // 
            this.ckBitwiseNot.AutoFont = false;
            this.ckBitwiseNot.AutoFontHeightRatio = 0.75F;
            this.ckBitwiseNot.AutoFontMax = 100F;
            this.ckBitwiseNot.AutoFontMin = 6F;
            this.ckBitwiseNot.AutoFontWidthRatio = 0.92F;
            this.ckBitwiseNot.AutoImage = true;
            this.ckBitwiseNot.AutoImageMaxRatio = 0.75F;
            this.ckBitwiseNot.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.ckBitwiseNot.AutoImageTint = true;
            this.ckBitwiseNot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckBitwiseNot.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckBitwiseNot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckBitwiseNot.BackgroundImage")));
            this.ckBitwiseNot.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ckBitwiseNot.BorderRadius = 10;
            this.ckBitwiseNot.BorderSize = 1;
            this.ckBitwiseNot.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.ckBitwiseNot.Corner = BeeGlobal.Corner.Both;
            this.ckBitwiseNot.DebounceResizeMs = 16;
            this.ckBitwiseNot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckBitwiseNot.FlatAppearance.BorderSize = 0;
            this.ckBitwiseNot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckBitwiseNot.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckBitwiseNot.ForeColor = System.Drawing.Color.Black;
            this.ckBitwiseNot.Image = null;
            this.ckBitwiseNot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckBitwiseNot.ImageDisabled = null;
            this.ckBitwiseNot.ImageHover = null;
            this.ckBitwiseNot.ImageNormal = null;
            this.ckBitwiseNot.ImagePadding = new System.Windows.Forms.Padding(1);
            this.ckBitwiseNot.ImagePressed = null;
            this.ckBitwiseNot.ImageTextSpacing = 6;
            this.ckBitwiseNot.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.ckBitwiseNot.ImageTintHover = System.Drawing.Color.Empty;
            this.ckBitwiseNot.ImageTintNormal = System.Drawing.Color.Empty;
            this.ckBitwiseNot.ImageTintOpacity = 0.5F;
            this.ckBitwiseNot.ImageTintPressed = System.Drawing.Color.Empty;
            this.ckBitwiseNot.IsCLick = false;
            this.ckBitwiseNot.IsNotChange = false;
            this.ckBitwiseNot.IsRect = false;
            this.ckBitwiseNot.IsUnGroup = true;
            this.ckBitwiseNot.Location = new System.Drawing.Point(324, 8);
            this.ckBitwiseNot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckBitwiseNot.Multiline = false;
            this.ckBitwiseNot.Name = "ckBitwiseNot";
            this.ckBitwiseNot.Size = new System.Drawing.Size(142, 39);
            this.ckBitwiseNot.TabIndex = 3;
            this.ckBitwiseNot.Text = "Not";
            this.ckBitwiseNot.TextColor = System.Drawing.Color.Black;
            this.ckBitwiseNot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckBitwiseNot.UseVisualStyleBackColor = false;
            this.ckBitwiseNot.Click += new System.EventHandler(this.ckBitwiseNot_Click);
            // 
            // trackMaxOverLap
            // 
            this.trackMaxOverLap.AutoShowTextbox = true;
            this.trackMaxOverLap.AutoSizeTextbox = true;
            this.trackMaxOverLap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.trackMaxOverLap.BarLeftGap = 20;
            this.trackMaxOverLap.BarRightGap = 6;
            this.trackMaxOverLap.ChromeGap = 8;
            this.trackMaxOverLap.ChromeWidthRatio = 0.14F;
            this.trackMaxOverLap.ColorBorder = System.Drawing.Color.LightGray;
            this.trackMaxOverLap.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackMaxOverLap.ColorScale = System.Drawing.Color.LightGray;
            this.trackMaxOverLap.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackMaxOverLap.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackMaxOverLap.ColorTrack = System.Drawing.Color.LightGray;
            this.trackMaxOverLap.Decimals = 0;
            this.trackMaxOverLap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackMaxOverLap.EdgePadding = 2;
            this.trackMaxOverLap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackMaxOverLap.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackMaxOverLap.KeyboardStep = 1F;
            this.trackMaxOverLap.Location = new System.Drawing.Point(5, 52);
            this.trackMaxOverLap.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.trackMaxOverLap.MatchTextboxFontToThumb = true;
            this.trackMaxOverLap.Max = 100F;
            this.trackMaxOverLap.MaxTextboxWidth = 1;
            this.trackMaxOverLap.MaxThumb = 1000;
            this.trackMaxOverLap.MaxTrackHeight = 1000;
            this.trackMaxOverLap.Min = 0F;
            this.trackMaxOverLap.MinChromeWidth = 64;
            this.trackMaxOverLap.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackMaxOverLap.MinTextboxWidth = 32;
            this.trackMaxOverLap.MinThumb = 30;
            this.trackMaxOverLap.MinTrackHeight = 8;
            this.trackMaxOverLap.Name = "trackMaxOverLap";
            this.trackMaxOverLap.Radius = 8;
            this.trackMaxOverLap.ShowValueOnThumb = true;
            this.trackMaxOverLap.Size = new System.Drawing.Size(476, 47);
            this.trackMaxOverLap.SnapToStep = true;
            this.trackMaxOverLap.StartWithTextboxHidden = true;
            this.trackMaxOverLap.Step = 1F;
            this.trackMaxOverLap.TabIndex = 71;
            this.trackMaxOverLap.TextboxFontSize = 20F;
            this.trackMaxOverLap.TextboxSidePadding = 10;
            this.trackMaxOverLap.TextboxWidth = 600;
            this.trackMaxOverLap.ThumbDiameterRatio = 2F;
            this.trackMaxOverLap.ThumbValueBold = true;
            this.trackMaxOverLap.ThumbValueFontScale = 1.5F;
            this.trackMaxOverLap.ThumbValuePadding = 0;
            this.trackMaxOverLap.TightEdges = true;
            this.trackMaxOverLap.TrackHeightRatio = 0.45F;
            this.trackMaxOverLap.TrackWidthRatio = 1F;
            this.trackMaxOverLap.UnitText = "";
            this.trackMaxOverLap.Value = 0F;
            this.trackMaxOverLap.WheelStep = 1F;
            this.trackMaxOverLap.ValueChanged += new System.Action<float>(this.trackMaxOverLap_ValueChanged);
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.AutoScroll = true;
            this.tableLayoutPanel9.AutoSize = true;
            this.tableLayoutPanel9.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel9.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel9.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.trackMaxOverLap, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel14, 0, 5);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel13, 0, 3);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 8;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(486, 848);
            this.tableLayoutPanel9.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(5, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(476, 32);
            this.label1.TabIndex = 72;
            this.label1.Text = "OverLap Range";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(5, 119);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(476, 32);
            this.label2.TabIndex = 73;
            this.label2.Text = "Search Algorithm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(5, 226);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 20, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(476, 32);
            this.label3.TabIndex = 74;
            this.label3.Text = "Option More";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToolPosition_Adjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.label4);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ToolPosition_Adjustment";
            this.Size = new System.Drawing.Size(500, 949);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.lay3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).EndInit();
            this.lay2.ResumeLayout(false);
            this.lay1.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.ComponentModel.BackgroundWorker threadProcess;
        private System.Windows.Forms.Timer tmClear;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private RJButton rjButton2;
        private RJButton rjButton5;
        private TrackBar2 trackBar21;
        private System.Windows.Forms.Label label4;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RJButton btnTest;
        private  System.Windows.Forms.TableLayoutPanel lay3;
        private System.Windows.Forms.PictureBox imgTemp;
        private RJButton btnLearning;
        private  System.Windows.Forms.TableLayoutPanel lay2;
        private RJButton btnCrop;
        private RJButton btnCropArea;
        private  System.Windows.Forms.TableLayoutPanel lay1;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private GroupControl.OK_Cancel oK_Cancel1;
        private AdjustBarEx trackScore;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private AdjustBarEx trackAngle;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private AdjustBarEx trackMaxOverLap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private RJButton ckBitwiseNot;
        private RJButton ckSubPixel;
        private RJButton ckSIMD;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private RJButton btnHighSpeed;
        private RJButton btnNormal;
        private System.Windows.Forms.Label label3;
    }
}
