﻿using BeeGlobal;

namespace BeeInterface
{
    partial class ToolYolo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolYolo));
            this.tmCheckFist = new System.Windows.Forms.Timer(this.components);
            this.workLoadModel = new System.ComponentModel.BackgroundWorker();
            this.workTrain = new System.ComponentModel.BackgroundWorker();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTraining = new BeeInterface.RJButton();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtPercent = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton13 = new BeeInterface.RJButton();
            this.rjButton14 = new BeeInterface.RJButton();
            this.rjButton15 = new BeeInterface.RJButton();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.numEpoch = new BeeInterface.CustomNumericEx();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCheck = new BeeInterface.RJButton();
            this.btnPathDataSet = new BeeInterface.RJButton();
            this.tbDataSet = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel19 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveDataSet = new BeeInterface.RJButton();
            this.tableLayoutPanel20 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoadImg = new BeeInterface.RJButton();
            this.btnCropTemp = new BeeInterface.RJButton();
            this.cbLabels = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.imgCrop = new Cyotek.Windows.Forms.ImageBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton2 = new BeeInterface.RJButton();
            this.btnString = new BeeInterface.RJButton();
            this.btnBits = new BeeInterface.RJButton();
            this.txtAddPLC = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEnable = new BeeInterface.RJButton();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.btnArrangeBox = new BeeInterface.RJButton();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.btnY_R_L = new BeeInterface.RJButton();
            this.btnY_L_R = new BeeInterface.RJButton();
            this.btnX_L_R = new BeeInterface.RJButton();
            this.btnX_R_L = new BeeInterface.RJButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton10 = new BeeInterface.RJButton();
            this.btnMore = new BeeInterface.RJButton();
            this.btnEqual = new BeeInterface.RJButton();
            this.btnLess = new BeeInterface.RJButton();
            this.trackNumObject = new BeeInterface.CustomNumericEx();
            this.label3 = new System.Windows.Forms.Label();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.label15 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dashboardLabel = new BeeInterface.DashboardListCompact();
            this.layoutSetLearning = new System.Windows.Forms.TableLayoutPanel();
            this.btnReload = new BeeInterface.RJButton();
            this.btnRemoveModel = new BeeInterface.RJButton();
            this.btnAddModel = new BeeInterface.RJButton();
            this.cbListModel = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.tabYolo = new System.Windows.Forms.TabControl();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            this.tableLayoutPanel19.SuspendLayout();
            this.tableLayoutPanel20.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.layoutSetLearning.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabYolo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmCheckFist
            // 
            this.tmCheckFist.Tick += new System.EventHandler(this.tmCheckFist_Tick);
            // 
            // workLoadModel
            // 
            this.workLoadModel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadModel_DoWork);
            this.workLoadModel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadModel_RunWorkerCompleted);
            // 
            // workTrain
            // 
            this.workTrain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workTrain_DoWork);
            this.workTrain.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.workTrain_ProgressChanged);
            this.workTrain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workTrain_RunWorkerCompleted);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 768);
            this.oK_Cancel1.Margin = new System.Windows.Forms.Padding(2);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(423, 58);
            this.oK_Cancel1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel10);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(415, 788);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Re-Train";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.btnTraining, 0, 8);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel14, 0, 7);
            this.tableLayoutPanel10.Controls.Add(this.label11, 0, 6);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel16, 0, 4);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel15, 0, 5);
            this.tableLayoutPanel10.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel18, 0, 3);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel17, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel19, 0, 2);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(2, 16, 2, 2);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 9;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(411, 784);
            this.tableLayoutPanel10.TabIndex = 1;
            // 
            // btnTraining
            // 
            this.btnTraining.AutoFont = true;
            this.btnTraining.AutoFontHeightRatio = 0.75F;
            this.btnTraining.AutoFontMax = 100F;
            this.btnTraining.AutoFontMin = 6F;
            this.btnTraining.AutoFontWidthRatio = 0.92F;
            this.btnTraining.AutoImage = true;
            this.btnTraining.AutoImageMaxRatio = 0.75F;
            this.btnTraining.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTraining.AutoImageTint = true;
            this.btnTraining.BackColor = System.Drawing.SystemColors.Control;
            this.btnTraining.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnTraining.BorderColor = System.Drawing.Color.Transparent;
            this.btnTraining.BorderRadius = 1;
            this.btnTraining.BorderSize = 1;
            this.btnTraining.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTraining.Corner = BeeGlobal.Corner.Both;
            this.btnTraining.DebounceResizeMs = 16;
            this.btnTraining.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTraining.FlatAppearance.BorderSize = 0;
            this.btnTraining.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTraining.Font = new System.Drawing.Font("Microsoft Sans Serif", 38.67969F, System.Drawing.FontStyle.Bold);
            this.btnTraining.ForeColor = System.Drawing.Color.Black;
            this.btnTraining.Image = null;
            this.btnTraining.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTraining.ImageDisabled = null;
            this.btnTraining.ImageHover = null;
            this.btnTraining.ImageNormal = null;
            this.btnTraining.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTraining.ImagePressed = null;
            this.btnTraining.ImageTextSpacing = 6;
            this.btnTraining.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTraining.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTraining.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTraining.ImageTintOpacity = 0.5F;
            this.btnTraining.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTraining.IsCLick = false;
            this.btnTraining.IsNotChange = true;
            this.btnTraining.IsRect = false;
            this.btnTraining.IsUnGroup = true;
            this.btnTraining.Location = new System.Drawing.Point(5, 548);
            this.btnTraining.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.btnTraining.Multiline = false;
            this.btnTraining.Name = "btnTraining";
            this.btnTraining.Size = new System.Drawing.Size(401, 226);
            this.btnTraining.TabIndex = 54;
            this.btnTraining.Text = "Start Training";
            this.btnTraining.TextColor = System.Drawing.Color.Black;
            this.btnTraining.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTraining.UseVisualStyleBackColor = false;
            this.btnTraining.Click += new System.EventHandler(this.btnTraining_Click);
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 2;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.2F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.8F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.2F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.8F));
            this.tableLayoutPanel14.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.txtPercent, 1, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(6, 494);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(6, 12, 2, 2);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(403, 42);
            this.tableLayoutPanel14.TabIndex = 53;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(2, 2);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(299, 38);
            this.progressBar1.TabIndex = 0;
            // 
            // txtPercent
            // 
            this.txtPercent.AutoSize = true;
            this.txtPercent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPercent.Location = new System.Drawing.Point(305, 0);
            this.txtPercent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtPercent.Name = "txtPercent";
            this.txtPercent.Size = new System.Drawing.Size(96, 42);
            this.txtPercent.TabIndex = 1;
            this.txtPercent.Text = "0%";
            this.txtPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(4, 456);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 12, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(203, 25);
            this.label11.TabIndex = 52;
            this.label11.Text = "Processing Training";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel16.ColumnCount = 3;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel16.Controls.Add(this.rjButton13, 2, 0);
            this.tableLayoutPanel16.Controls.Add(this.rjButton14, 0, 0);
            this.tableLayoutPanel16.Controls.Add(this.rjButton15, 1, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(4, 341);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(4, 0, 2, 0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 1;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(405, 41);
            this.tableLayoutPanel16.TabIndex = 51;
            // 
            // rjButton13
            // 
            this.rjButton13.AutoFont = true;
            this.rjButton13.AutoFontHeightRatio = 0.75F;
            this.rjButton13.AutoFontMax = 100F;
            this.rjButton13.AutoFontMin = 6F;
            this.rjButton13.AutoFontWidthRatio = 0.92F;
            this.rjButton13.AutoImage = true;
            this.rjButton13.AutoImageMaxRatio = 0.75F;
            this.rjButton13.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton13.AutoImageTint = true;
            this.rjButton13.BackColor = System.Drawing.SystemColors.Control;
            this.rjButton13.BackgroundColor = System.Drawing.SystemColors.Control;
            this.rjButton13.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton13.BackgroundImage")));
            this.rjButton13.BorderColor = System.Drawing.Color.Silver;
            this.rjButton13.BorderRadius = 10;
            this.rjButton13.BorderSize = 1;
            this.rjButton13.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton13.Corner = BeeGlobal.Corner.Right;
            this.rjButton13.DebounceResizeMs = 16;
            this.rjButton13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton13.Enabled = false;
            this.rjButton13.FlatAppearance.BorderSize = 0;
            this.rjButton13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.40625F);
            this.rjButton13.ForeColor = System.Drawing.Color.Black;
            this.rjButton13.Image = null;
            this.rjButton13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton13.ImageDisabled = null;
            this.rjButton13.ImageHover = null;
            this.rjButton13.ImageNormal = null;
            this.rjButton13.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton13.ImagePressed = null;
            this.rjButton13.ImageTextSpacing = 6;
            this.rjButton13.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton13.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton13.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton13.ImageTintOpacity = 0.5F;
            this.rjButton13.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton13.IsCLick = false;
            this.rjButton13.IsNotChange = false;
            this.rjButton13.IsRect = false;
            this.rjButton13.IsUnGroup = false;
            this.rjButton13.Location = new System.Drawing.Point(273, 3);
            this.rjButton13.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.rjButton13.Multiline = false;
            this.rjButton13.Name = "rjButton13";
            this.rjButton13.Size = new System.Drawing.Size(129, 35);
            this.rjButton13.TabIndex = 4;
            this.rjButton13.Text = "New Model";
            this.rjButton13.TextColor = System.Drawing.Color.Black;
            this.rjButton13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton13.UseVisualStyleBackColor = false;
            // 
            // rjButton14
            // 
            this.rjButton14.AutoFont = true;
            this.rjButton14.AutoFontHeightRatio = 0.75F;
            this.rjButton14.AutoFontMax = 100F;
            this.rjButton14.AutoFontMin = 6F;
            this.rjButton14.AutoFontWidthRatio = 0.92F;
            this.rjButton14.AutoImage = true;
            this.rjButton14.AutoImageMaxRatio = 0.75F;
            this.rjButton14.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton14.AutoImageTint = true;
            this.rjButton14.BackColor = System.Drawing.SystemColors.Control;
            this.rjButton14.BackgroundColor = System.Drawing.SystemColors.Control;
            this.rjButton14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton14.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton14.BorderRadius = 10;
            this.rjButton14.BorderSize = 1;
            this.rjButton14.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton14.Corner = BeeGlobal.Corner.Left;
            this.rjButton14.DebounceResizeMs = 16;
            this.rjButton14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton14.Enabled = false;
            this.rjButton14.FlatAppearance.BorderSize = 0;
            this.rjButton14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.40625F);
            this.rjButton14.ForeColor = System.Drawing.Color.Black;
            this.rjButton14.Image = null;
            this.rjButton14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton14.ImageDisabled = null;
            this.rjButton14.ImageHover = null;
            this.rjButton14.ImageNormal = null;
            this.rjButton14.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton14.ImagePressed = null;
            this.rjButton14.ImageTextSpacing = 6;
            this.rjButton14.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton14.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton14.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton14.ImageTintOpacity = 0.5F;
            this.rjButton14.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton14.IsCLick = true;
            this.rjButton14.IsNotChange = false;
            this.rjButton14.IsRect = false;
            this.rjButton14.IsUnGroup = false;
            this.rjButton14.Location = new System.Drawing.Point(3, 3);
            this.rjButton14.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.rjButton14.Multiline = false;
            this.rjButton14.Name = "rjButton14";
            this.rjButton14.Size = new System.Drawing.Size(128, 35);
            this.rjButton14.TabIndex = 2;
            this.rjButton14.Text = "Re-Train";
            this.rjButton14.TextColor = System.Drawing.Color.Black;
            this.rjButton14.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton14.UseVisualStyleBackColor = false;
            // 
            // rjButton15
            // 
            this.rjButton15.AutoFont = true;
            this.rjButton15.AutoFontHeightRatio = 0.75F;
            this.rjButton15.AutoFontMax = 100F;
            this.rjButton15.AutoFontMin = 6F;
            this.rjButton15.AutoFontWidthRatio = 0.92F;
            this.rjButton15.AutoImage = true;
            this.rjButton15.AutoImageMaxRatio = 0.75F;
            this.rjButton15.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton15.AutoImageTint = true;
            this.rjButton15.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton15.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton15.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton15.BackgroundImage")));
            this.rjButton15.BorderColor = System.Drawing.Color.Silver;
            this.rjButton15.BorderRadius = 5;
            this.rjButton15.BorderSize = 1;
            this.rjButton15.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton15.Corner = BeeGlobal.Corner.None;
            this.rjButton15.DebounceResizeMs = 16;
            this.rjButton15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton15.FlatAppearance.BorderSize = 0;
            this.rjButton15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton15.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.14063F);
            this.rjButton15.ForeColor = System.Drawing.Color.Black;
            this.rjButton15.Image = null;
            this.rjButton15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton15.ImageDisabled = null;
            this.rjButton15.ImageHover = null;
            this.rjButton15.ImageNormal = null;
            this.rjButton15.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton15.ImagePressed = null;
            this.rjButton15.ImageTextSpacing = 6;
            this.rjButton15.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton15.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton15.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton15.ImageTintOpacity = 0.5F;
            this.rjButton15.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton15.IsCLick = false;
            this.rjButton15.IsNotChange = false;
            this.rjButton15.IsRect = false;
            this.rjButton15.IsUnGroup = false;
            this.rjButton15.Location = new System.Drawing.Point(131, 0);
            this.rjButton15.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.rjButton15.Multiline = false;
            this.rjButton15.Name = "rjButton15";
            this.rjButton15.Size = new System.Drawing.Size(142, 38);
            this.rjButton15.TabIndex = 3;
            this.rjButton15.Text = "New Class";
            this.rjButton15.TextColor = System.Drawing.Color.Black;
            this.rjButton15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton15.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.4F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.6F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.4F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.6F));
            this.tableLayoutPanel15.Controls.Add(this.numEpoch, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(6, 394);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(6, 12, 2, 2);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(403, 48);
            this.tableLayoutPanel15.TabIndex = 48;
            // 
            // numEpoch
            // 
            this.numEpoch.AutoShowTextbox = false;
            this.numEpoch.AutoSizeTextbox = true;
            this.numEpoch.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numEpoch.BackColor = System.Drawing.SystemColors.Control;
            this.numEpoch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numEpoch.BorderRadius = 6;
            this.numEpoch.ButtonMaxSize = 64;
            this.numEpoch.ButtonMinSize = 24;
            this.numEpoch.Decimals = 0;
            this.numEpoch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numEpoch.ElementGap = 6;
            this.numEpoch.FillTextboxToAvailable = true;
            this.numEpoch.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numEpoch.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numEpoch.KeyboardStep = 1F;
            this.numEpoch.Location = new System.Drawing.Point(122, 0);
            this.numEpoch.Margin = new System.Windows.Forms.Padding(0);
            this.numEpoch.Max = 1000F;
            this.numEpoch.MaxTextboxWidth = 0;
            this.numEpoch.Min = 0F;
            this.numEpoch.MinimumSize = new System.Drawing.Size(120, 32);
            this.numEpoch.MinTextboxWidth = 16;
            this.numEpoch.Name = "numEpoch";
            this.numEpoch.Size = new System.Drawing.Size(281, 48);
            this.numEpoch.SnapToStep = true;
            this.numEpoch.StartWithTextboxHidden = false;
            this.numEpoch.Step = 1F;
            this.numEpoch.TabIndex = 46;
            this.numEpoch.TextboxFontSize = 24F;
            this.numEpoch.TextboxSidePadding = 12;
            this.numEpoch.TextboxWidth = 56;
            this.numEpoch.UnitText = "";
            this.numEpoch.Value = 0F;
            this.numEpoch.WheelStep = 1F;
            this.numEpoch.ValueChanged += new System.Action<float>(this.numEpoch_ValueChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 48);
            this.label4.TabIndex = 45;
            this.label4.Text = "Epoch";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 12);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 12, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 25);
            this.label7.TabIndex = 44;
            this.label7.Text = "Data Set";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 2;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel18.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(6, 307);
            this.tableLayoutPanel18.Margin = new System.Windows.Forms.Padding(6, 12, 2, 2);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 1;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(403, 32);
            this.tableLayoutPanel18.TabIndex = 47;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(219, 32);
            this.label10.TabIndex = 45;
            this.label10.Text = "Choose Type Train";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.ColumnCount = 3;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.16042F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.36899F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.47059F));
            this.tableLayoutPanel17.Controls.Add(this.btnCheck, 2, 0);
            this.tableLayoutPanel17.Controls.Add(this.btnPathDataSet, 1, 0);
            this.tableLayoutPanel17.Controls.Add(this.tbDataSet, 0, 0);
            this.tableLayoutPanel17.Location = new System.Drawing.Point(2, 39);
            this.tableLayoutPanel17.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.RowCount = 1;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel17.Size = new System.Drawing.Size(407, 36);
            this.tableLayoutPanel17.TabIndex = 50;
            // 
            // btnCheck
            // 
            this.btnCheck.AutoFont = true;
            this.btnCheck.AutoFontHeightRatio = 0.75F;
            this.btnCheck.AutoFontMax = 100F;
            this.btnCheck.AutoFontMin = 6F;
            this.btnCheck.AutoFontWidthRatio = 0.92F;
            this.btnCheck.AutoImage = true;
            this.btnCheck.AutoImageMaxRatio = 0.75F;
            this.btnCheck.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCheck.AutoImageTint = true;
            this.btnCheck.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheck.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCheck.BorderColor = System.Drawing.Color.Transparent;
            this.btnCheck.BorderRadius = 10;
            this.btnCheck.BorderSize = 1;
            this.btnCheck.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCheck.Corner = BeeGlobal.Corner.None;
            this.btnCheck.DebounceResizeMs = 16;
            this.btnCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCheck.FlatAppearance.BorderSize = 0;
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.9375F, System.Drawing.FontStyle.Bold);
            this.btnCheck.ForeColor = System.Drawing.Color.Black;
            this.btnCheck.Image = null;
            this.btnCheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheck.ImageDisabled = null;
            this.btnCheck.ImageHover = null;
            this.btnCheck.ImageNormal = null;
            this.btnCheck.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCheck.ImagePressed = null;
            this.btnCheck.ImageTextSpacing = 6;
            this.btnCheck.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCheck.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCheck.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCheck.ImageTintOpacity = 0.5F;
            this.btnCheck.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCheck.IsCLick = false;
            this.btnCheck.IsNotChange = true;
            this.btnCheck.IsRect = false;
            this.btnCheck.IsUnGroup = true;
            this.btnCheck.Location = new System.Drawing.Point(300, 2);
            this.btnCheck.Margin = new System.Windows.Forms.Padding(2);
            this.btnCheck.Multiline = false;
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(105, 32);
            this.btnCheck.TabIndex = 49;
            this.btnCheck.Text = "Inspect";
            this.btnCheck.TextColor = System.Drawing.Color.Black;
            this.btnCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnPathDataSet
            // 
            this.btnPathDataSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPathDataSet.AutoFont = true;
            this.btnPathDataSet.AutoFontHeightRatio = 0.75F;
            this.btnPathDataSet.AutoFontMax = 100F;
            this.btnPathDataSet.AutoFontMin = 6F;
            this.btnPathDataSet.AutoFontWidthRatio = 0.92F;
            this.btnPathDataSet.AutoImage = true;
            this.btnPathDataSet.AutoImageMaxRatio = 0.75F;
            this.btnPathDataSet.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnPathDataSet.AutoImageTint = true;
            this.btnPathDataSet.BackColor = System.Drawing.SystemColors.Control;
            this.btnPathDataSet.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnPathDataSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPathDataSet.BorderColor = System.Drawing.Color.Transparent;
            this.btnPathDataSet.BorderRadius = 10;
            this.btnPathDataSet.BorderSize = 1;
            this.btnPathDataSet.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnPathDataSet.Corner = BeeGlobal.Corner.None;
            this.btnPathDataSet.DebounceResizeMs = 16;
            this.btnPathDataSet.FlatAppearance.BorderSize = 0;
            this.btnPathDataSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPathDataSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnPathDataSet.ForeColor = System.Drawing.Color.Black;
            this.btnPathDataSet.Image = null;
            this.btnPathDataSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPathDataSet.ImageDisabled = null;
            this.btnPathDataSet.ImageHover = null;
            this.btnPathDataSet.ImageNormal = null;
            this.btnPathDataSet.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnPathDataSet.ImagePressed = null;
            this.btnPathDataSet.ImageTextSpacing = 6;
            this.btnPathDataSet.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnPathDataSet.ImageTintHover = System.Drawing.Color.Empty;
            this.btnPathDataSet.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnPathDataSet.ImageTintOpacity = 0.5F;
            this.btnPathDataSet.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnPathDataSet.IsCLick = false;
            this.btnPathDataSet.IsNotChange = false;
            this.btnPathDataSet.IsRect = false;
            this.btnPathDataSet.IsUnGroup = true;
            this.btnPathDataSet.Location = new System.Drawing.Point(247, 3);
            this.btnPathDataSet.Multiline = false;
            this.btnPathDataSet.Name = "btnPathDataSet";
            this.btnPathDataSet.Size = new System.Drawing.Size(48, 30);
            this.btnPathDataSet.TabIndex = 3;
            this.btnPathDataSet.Text = "..";
            this.btnPathDataSet.TextColor = System.Drawing.Color.Black;
            this.btnPathDataSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPathDataSet.UseVisualStyleBackColor = false;
            this.btnPathDataSet.Click += new System.EventHandler(this.btnPathDataSet_Click);
            // 
            // tbDataSet
            // 
            this.tbDataSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDataSet.Location = new System.Drawing.Point(2, 2);
            this.tbDataSet.Margin = new System.Windows.Forms.Padding(2);
            this.tbDataSet.Multiline = true;
            this.tbDataSet.Name = "tbDataSet";
            this.tbDataSet.Size = new System.Drawing.Size(240, 32);
            this.tbDataSet.TabIndex = 0;
            // 
            // tableLayoutPanel19
            // 
            this.tableLayoutPanel19.ColumnCount = 1;
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel19.Controls.Add(this.btnSaveDataSet, 0, 2);
            this.tableLayoutPanel19.Controls.Add(this.tableLayoutPanel20, 0, 0);
            this.tableLayoutPanel19.Controls.Add(this.imgCrop, 0, 1);
            this.tableLayoutPanel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel19.Location = new System.Drawing.Point(2, 79);
            this.tableLayoutPanel19.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel19.Name = "tableLayoutPanel19";
            this.tableLayoutPanel19.RowCount = 4;
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel19.Size = new System.Drawing.Size(407, 214);
            this.tableLayoutPanel19.TabIndex = 55;
            // 
            // btnSaveDataSet
            // 
            this.btnSaveDataSet.AutoFont = true;
            this.btnSaveDataSet.AutoFontHeightRatio = 0.75F;
            this.btnSaveDataSet.AutoFontMax = 100F;
            this.btnSaveDataSet.AutoFontMin = 6F;
            this.btnSaveDataSet.AutoFontWidthRatio = 0.92F;
            this.btnSaveDataSet.AutoImage = true;
            this.btnSaveDataSet.AutoImageMaxRatio = 0.75F;
            this.btnSaveDataSet.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSaveDataSet.AutoImageTint = true;
            this.btnSaveDataSet.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSaveDataSet.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnSaveDataSet.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveDataSet.BackgroundImage")));
            this.btnSaveDataSet.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveDataSet.BorderRadius = 5;
            this.btnSaveDataSet.BorderSize = 1;
            this.btnSaveDataSet.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSaveDataSet.Corner = BeeGlobal.Corner.None;
            this.btnSaveDataSet.DebounceResizeMs = 16;
            this.btnSaveDataSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveDataSet.FlatAppearance.BorderSize = 0;
            this.btnSaveDataSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDataSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.34375F);
            this.btnSaveDataSet.ForeColor = System.Drawing.Color.Black;
            this.btnSaveDataSet.Image = null;
            this.btnSaveDataSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveDataSet.ImageDisabled = null;
            this.btnSaveDataSet.ImageHover = null;
            this.btnSaveDataSet.ImageNormal = null;
            this.btnSaveDataSet.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSaveDataSet.ImagePressed = null;
            this.btnSaveDataSet.ImageTextSpacing = 6;
            this.btnSaveDataSet.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSaveDataSet.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSaveDataSet.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSaveDataSet.ImageTintOpacity = 0.5F;
            this.btnSaveDataSet.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSaveDataSet.IsCLick = false;
            this.btnSaveDataSet.IsNotChange = true;
            this.btnSaveDataSet.IsRect = false;
            this.btnSaveDataSet.IsUnGroup = false;
            this.btnSaveDataSet.Location = new System.Drawing.Point(0, 149);
            this.btnSaveDataSet.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.btnSaveDataSet.Multiline = false;
            this.btnSaveDataSet.Name = "btnSaveDataSet";
            this.btnSaveDataSet.Size = new System.Drawing.Size(407, 42);
            this.btnSaveDataSet.TabIndex = 50;
            this.btnSaveDataSet.Text = "Save Data Set";
            this.btnSaveDataSet.TextColor = System.Drawing.Color.Black;
            this.btnSaveDataSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveDataSet.UseVisualStyleBackColor = false;
            this.btnSaveDataSet.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel20
            // 
            this.tableLayoutPanel20.ColumnCount = 4;
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.63218F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.36782F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel20.Controls.Add(this.btnLoadImg, 3, 0);
            this.tableLayoutPanel20.Controls.Add(this.btnCropTemp, 2, 0);
            this.tableLayoutPanel20.Controls.Add(this.cbLabels, 1, 0);
            this.tableLayoutPanel20.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel20.Location = new System.Drawing.Point(6, 5);
            this.tableLayoutPanel20.Margin = new System.Windows.Forms.Padding(6, 5, 2, 2);
            this.tableLayoutPanel20.Name = "tableLayoutPanel20";
            this.tableLayoutPanel20.RowCount = 1;
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel20.Size = new System.Drawing.Size(399, 43);
            this.tableLayoutPanel20.TabIndex = 48;
            // 
            // btnLoadImg
            // 
            this.btnLoadImg.AutoFont = true;
            this.btnLoadImg.AutoFontHeightRatio = 0.75F;
            this.btnLoadImg.AutoFontMax = 100F;
            this.btnLoadImg.AutoFontMin = 6F;
            this.btnLoadImg.AutoFontWidthRatio = 0.92F;
            this.btnLoadImg.AutoImage = true;
            this.btnLoadImg.AutoImageMaxRatio = 0.75F;
            this.btnLoadImg.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLoadImg.AutoImageTint = true;
            this.btnLoadImg.BackColor = System.Drawing.SystemColors.Control;
            this.btnLoadImg.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnLoadImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoadImg.BorderColor = System.Drawing.Color.Transparent;
            this.btnLoadImg.BorderRadius = 10;
            this.btnLoadImg.BorderSize = 1;
            this.btnLoadImg.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLoadImg.Corner = BeeGlobal.Corner.None;
            this.btnLoadImg.DebounceResizeMs = 16;
            this.btnLoadImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadImg.FlatAppearance.BorderSize = 0;
            this.btnLoadImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnLoadImg.ForeColor = System.Drawing.Color.Black;
            this.btnLoadImg.Image = null;
            this.btnLoadImg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadImg.ImageDisabled = null;
            this.btnLoadImg.ImageHover = null;
            this.btnLoadImg.ImageNormal = null;
            this.btnLoadImg.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLoadImg.ImagePressed = null;
            this.btnLoadImg.ImageTextSpacing = 6;
            this.btnLoadImg.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLoadImg.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLoadImg.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLoadImg.ImageTintOpacity = 0.5F;
            this.btnLoadImg.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLoadImg.IsCLick = false;
            this.btnLoadImg.IsNotChange = true;
            this.btnLoadImg.IsRect = false;
            this.btnLoadImg.IsUnGroup = true;
            this.btnLoadImg.Location = new System.Drawing.Point(260, 2);
            this.btnLoadImg.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadImg.Multiline = false;
            this.btnLoadImg.Name = "btnLoadImg";
            this.btnLoadImg.Size = new System.Drawing.Size(137, 39);
            this.btnLoadImg.TabIndex = 48;
            this.btnLoadImg.Text = "Add Image...";
            this.btnLoadImg.TextColor = System.Drawing.Color.Black;
            this.btnLoadImg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLoadImg.UseVisualStyleBackColor = false;
            this.btnLoadImg.Click += new System.EventHandler(this.btnLoadImg_Click);
            // 
            // btnCropTemp
            // 
            this.btnCropTemp.AutoFont = true;
            this.btnCropTemp.AutoFontHeightRatio = 0.75F;
            this.btnCropTemp.AutoFontMax = 100F;
            this.btnCropTemp.AutoFontMin = 6F;
            this.btnCropTemp.AutoFontWidthRatio = 0.92F;
            this.btnCropTemp.AutoImage = true;
            this.btnCropTemp.AutoImageMaxRatio = 0.75F;
            this.btnCropTemp.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropTemp.AutoImageTint = true;
            this.btnCropTemp.BackColor = System.Drawing.SystemColors.Control;
            this.btnCropTemp.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCropTemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropTemp.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropTemp.BorderRadius = 10;
            this.btnCropTemp.BorderSize = 1;
            this.btnCropTemp.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropTemp.Corner = BeeGlobal.Corner.None;
            this.btnCropTemp.DebounceResizeMs = 16;
            this.btnCropTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropTemp.FlatAppearance.BorderSize = 0;
            this.btnCropTemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnCropTemp.ForeColor = System.Drawing.Color.Black;
            this.btnCropTemp.Image = null;
            this.btnCropTemp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropTemp.ImageDisabled = null;
            this.btnCropTemp.ImageHover = null;
            this.btnCropTemp.ImageNormal = null;
            this.btnCropTemp.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropTemp.ImagePressed = null;
            this.btnCropTemp.ImageTextSpacing = 6;
            this.btnCropTemp.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropTemp.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropTemp.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropTemp.ImageTintOpacity = 0.5F;
            this.btnCropTemp.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropTemp.IsCLick = false;
            this.btnCropTemp.IsNotChange = true;
            this.btnCropTemp.IsRect = false;
            this.btnCropTemp.IsUnGroup = true;
            this.btnCropTemp.Location = new System.Drawing.Point(106, 2);
            this.btnCropTemp.Margin = new System.Windows.Forms.Padding(2);
            this.btnCropTemp.Multiline = false;
            this.btnCropTemp.Name = "btnCropTemp";
            this.btnCropTemp.Size = new System.Drawing.Size(150, 39);
            this.btnCropTemp.TabIndex = 47;
            this.btnCropTemp.Text = "Crop";
            this.btnCropTemp.TextColor = System.Drawing.Color.Black;
            this.btnCropTemp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropTemp.UseVisualStyleBackColor = false;
            this.btnCropTemp.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // cbLabels
            // 
            this.cbLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLabels.FormattingEnabled = true;
            this.cbLabels.Location = new System.Drawing.Point(39, 2);
            this.cbLabels.Margin = new System.Windows.Forms.Padding(2);
            this.cbLabels.Name = "cbLabels";
            this.cbLabels.Size = new System.Drawing.Size(63, 33);
            this.cbLabels.TabIndex = 46;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 43);
            this.label13.TabIndex = 45;
            this.label13.Text = "Draw Label";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imgCrop
            // 
            this.imgCrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.imgCrop.Location = new System.Drawing.Point(2, 52);
            this.imgCrop.Margin = new System.Windows.Forms.Padding(2);
            this.imgCrop.Name = "imgCrop";
            this.imgCrop.Size = new System.Drawing.Size(282, 95);
            this.imgCrop.TabIndex = 49;
            this.imgCrop.Text = "imageBox1";
            this.imgCrop.Paint += new System.Windows.Forms.PaintEventHandler(this.imgCrop_Paint);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage4.Size = new System.Drawing.Size(415, 788);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.AutoScroll = true;
            this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel4, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel3, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel21, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(2, 16, 2, 2);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 14;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(411, 784);
            this.tableLayoutPanel8.TabIndex = 0;
            this.tableLayoutPanel8.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel8_Paint);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel4.Controls.Add(this.rjButton2, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnString, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnBits, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtAddPLC, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(20, 330);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(20, 0, 4, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(387, 60);
            this.tableLayoutPanel4.TabIndex = 55;
            // 
            // rjButton2
            // 
            this.rjButton2.AutoFont = true;
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
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 10;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton2.Corner = BeeGlobal.Corner.Right;
            this.rjButton2.DebounceResizeMs = 16;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.Enabled = false;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.rjButton2.Location = new System.Drawing.Point(383, 0);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton2.Multiline = false;
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(4, 60);
            this.rjButton2.TabIndex = 38;
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // btnString
            // 
            this.btnString.AutoFont = true;
            this.btnString.AutoFontHeightRatio = 0.9F;
            this.btnString.AutoFontMax = 100F;
            this.btnString.AutoFontMin = 8F;
            this.btnString.AutoFontWidthRatio = 0.92F;
            this.btnString.AutoImage = true;
            this.btnString.AutoImageMaxRatio = 0.75F;
            this.btnString.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnString.AutoImageTint = true;
            this.btnString.BackColor = System.Drawing.Color.Gray;
            this.btnString.BackgroundColor = System.Drawing.Color.Gray;
            this.btnString.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnString.BorderColor = System.Drawing.Color.LightGray;
            this.btnString.BorderRadius = 5;
            this.btnString.BorderSize = 1;
            this.btnString.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnString.Corner = BeeGlobal.Corner.None;
            this.btnString.DebounceResizeMs = 16;
            this.btnString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnString.FlatAppearance.BorderSize = 0;
            this.btnString.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnString.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.39063F);
            this.btnString.ForeColor = System.Drawing.Color.Black;
            this.btnString.Image = null;
            this.btnString.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnString.ImageDisabled = null;
            this.btnString.ImageHover = null;
            this.btnString.ImageNormal = null;
            this.btnString.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnString.ImagePressed = null;
            this.btnString.ImageTextSpacing = 6;
            this.btnString.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnString.ImageTintHover = System.Drawing.Color.Empty;
            this.btnString.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnString.ImageTintOpacity = 0.5F;
            this.btnString.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnString.IsCLick = false;
            this.btnString.IsNotChange = false;
            this.btnString.IsRect = false;
            this.btnString.IsUnGroup = false;
            this.btnString.Location = new System.Drawing.Point(81, 0);
            this.btnString.Margin = new System.Windows.Forms.Padding(0);
            this.btnString.Multiline = false;
            this.btnString.Name = "btnString";
            this.btnString.Size = new System.Drawing.Size(78, 60);
            this.btnString.TabIndex = 32;
            this.btnString.Text = "String";
            this.btnString.TextColor = System.Drawing.Color.Black;
            this.btnString.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnString.UseVisualStyleBackColor = false;
            this.btnString.Click += new System.EventHandler(this.btnString_Click);
            // 
            // btnBits
            // 
            this.btnBits.AutoFont = true;
            this.btnBits.AutoFontHeightRatio = 0.75F;
            this.btnBits.AutoFontMax = 100F;
            this.btnBits.AutoFontMin = 6F;
            this.btnBits.AutoFontWidthRatio = 0.92F;
            this.btnBits.AutoImage = true;
            this.btnBits.AutoImageMaxRatio = 0.75F;
            this.btnBits.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnBits.AutoImageTint = true;
            this.btnBits.BackColor = System.Drawing.Color.LightGray;
            this.btnBits.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnBits.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBits.BorderColor = System.Drawing.Color.Transparent;
            this.btnBits.BorderRadius = 10;
            this.btnBits.BorderSize = 1;
            this.btnBits.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnBits.Corner = BeeGlobal.Corner.Left;
            this.btnBits.DebounceResizeMs = 16;
            this.btnBits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBits.FlatAppearance.BorderSize = 0;
            this.btnBits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBits.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.21875F);
            this.btnBits.ForeColor = System.Drawing.Color.Black;
            this.btnBits.Image = null;
            this.btnBits.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBits.ImageDisabled = null;
            this.btnBits.ImageHover = null;
            this.btnBits.ImageNormal = null;
            this.btnBits.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnBits.ImagePressed = null;
            this.btnBits.ImageTextSpacing = 6;
            this.btnBits.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnBits.ImageTintHover = System.Drawing.Color.Empty;
            this.btnBits.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnBits.ImageTintOpacity = 0.5F;
            this.btnBits.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnBits.IsCLick = true;
            this.btnBits.IsNotChange = false;
            this.btnBits.IsRect = false;
            this.btnBits.IsUnGroup = false;
            this.btnBits.Location = new System.Drawing.Point(0, 0);
            this.btnBits.Margin = new System.Windows.Forms.Padding(0);
            this.btnBits.Multiline = false;
            this.btnBits.Name = "btnBits";
            this.btnBits.Size = new System.Drawing.Size(81, 60);
            this.btnBits.TabIndex = 31;
            this.btnBits.Text = "Bits";
            this.btnBits.TextColor = System.Drawing.Color.Black;
            this.btnBits.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBits.UseVisualStyleBackColor = false;
            this.btnBits.Click += new System.EventHandler(this.btnBits_Click);
            // 
            // txtAddPLC
            // 
            this.txtAddPLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddPLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddPLC.Location = new System.Drawing.Point(254, 3);
            this.txtAddPLC.Name = "txtAddPLC";
            this.txtAddPLC.Size = new System.Drawing.Size(126, 56);
            this.txtAddPLC.TabIndex = 39;
            this.txtAddPLC.Text = "D104";
            this.txtAddPLC.TextChanged += new System.EventHandler(this.txtAddPLC_TextChanged);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(162, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 60);
            this.label5.TabIndex = 40;
            this.label5.Text = "Address";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 305);
            this.label2.Margin = new System.Windows.Forms.Padding(20, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 25);
            this.label2.TabIndex = 54;
            this.label2.Text = "Type  Value Send";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnEnable, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 243);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(6, 12, 2, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(403, 60);
            this.tableLayoutPanel3.TabIndex = 53;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 60);
            this.label1.TabIndex = 45;
            this.label1.Text = "Send Result";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnEnable
            // 
            this.btnEnable.AutoFont = true;
            this.btnEnable.AutoFontHeightRatio = 0.75F;
            this.btnEnable.AutoFontMax = 100F;
            this.btnEnable.AutoFontMin = 6F;
            this.btnEnable.AutoFontWidthRatio = 0.92F;
            this.btnEnable.AutoImage = true;
            this.btnEnable.AutoImageMaxRatio = 0.75F;
            this.btnEnable.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnable.AutoImageTint = true;
            this.btnEnable.BackColor = System.Drawing.SystemColors.Control;
            this.btnEnable.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnEnable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnable.BorderColor = System.Drawing.Color.Transparent;
            this.btnEnable.BorderRadius = 10;
            this.btnEnable.BorderSize = 1;
            this.btnEnable.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnable.Corner = BeeGlobal.Corner.None;
            this.btnEnable.DebounceResizeMs = 16;
            this.btnEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnable.FlatAppearance.BorderSize = 0;
            this.btnEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnEnable.ForeColor = System.Drawing.Color.Black;
            this.btnEnable.Image = null;
            this.btnEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnable.ImageDisabled = null;
            this.btnEnable.ImageHover = null;
            this.btnEnable.ImageNormal = null;
            this.btnEnable.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnable.ImagePressed = null;
            this.btnEnable.ImageTextSpacing = 6;
            this.btnEnable.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnable.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnable.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnable.ImageTintOpacity = 0.5F;
            this.btnEnable.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnable.IsCLick = false;
            this.btnEnable.IsNotChange = false;
            this.btnEnable.IsRect = false;
            this.btnEnable.IsUnGroup = true;
            this.btnEnable.Location = new System.Drawing.Point(222, 3);
            this.btnEnable.Multiline = false;
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(178, 54);
            this.btnEnable.TabIndex = 2;
            this.btnEnable.Text = "Enable";
            this.btnEnable.TextColor = System.Drawing.Color.Black;
            this.btnEnable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnable.UseVisualStyleBackColor = false;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel21.ColumnCount = 2;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel21.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanel21.Controls.Add(this.btnArrangeBox, 1, 0);
            this.tableLayoutPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel21.Location = new System.Drawing.Point(6, 109);
            this.tableLayoutPanel21.Margin = new System.Windows.Forms.Padding(6, 12, 2, 2);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 1;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(403, 60);
            this.tableLayoutPanel21.TabIndex = 52;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(219, 60);
            this.label14.TabIndex = 45;
            this.label14.Text = "Arrange Box";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnArrangeBox
            // 
            this.btnArrangeBox.AutoFont = true;
            this.btnArrangeBox.AutoFontHeightRatio = 0.75F;
            this.btnArrangeBox.AutoFontMax = 100F;
            this.btnArrangeBox.AutoFontMin = 6F;
            this.btnArrangeBox.AutoFontWidthRatio = 0.92F;
            this.btnArrangeBox.AutoImage = true;
            this.btnArrangeBox.AutoImageMaxRatio = 0.75F;
            this.btnArrangeBox.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnArrangeBox.AutoImageTint = true;
            this.btnArrangeBox.BackColor = System.Drawing.SystemColors.Control;
            this.btnArrangeBox.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnArrangeBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnArrangeBox.BorderColor = System.Drawing.Color.Transparent;
            this.btnArrangeBox.BorderRadius = 10;
            this.btnArrangeBox.BorderSize = 1;
            this.btnArrangeBox.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnArrangeBox.Corner = BeeGlobal.Corner.None;
            this.btnArrangeBox.DebounceResizeMs = 16;
            this.btnArrangeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnArrangeBox.FlatAppearance.BorderSize = 0;
            this.btnArrangeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArrangeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnArrangeBox.ForeColor = System.Drawing.Color.Black;
            this.btnArrangeBox.Image = null;
            this.btnArrangeBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnArrangeBox.ImageDisabled = null;
            this.btnArrangeBox.ImageHover = null;
            this.btnArrangeBox.ImageNormal = null;
            this.btnArrangeBox.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnArrangeBox.ImagePressed = null;
            this.btnArrangeBox.ImageTextSpacing = 6;
            this.btnArrangeBox.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnArrangeBox.ImageTintHover = System.Drawing.Color.Empty;
            this.btnArrangeBox.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnArrangeBox.ImageTintOpacity = 0.5F;
            this.btnArrangeBox.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnArrangeBox.IsCLick = false;
            this.btnArrangeBox.IsNotChange = false;
            this.btnArrangeBox.IsRect = false;
            this.btnArrangeBox.IsUnGroup = true;
            this.btnArrangeBox.Location = new System.Drawing.Point(222, 3);
            this.btnArrangeBox.Multiline = false;
            this.btnArrangeBox.Name = "btnArrangeBox";
            this.btnArrangeBox.Size = new System.Drawing.Size(178, 54);
            this.btnArrangeBox.TabIndex = 2;
            this.btnArrangeBox.Text = "Enable";
            this.btnArrangeBox.TextColor = System.Drawing.Color.Black;
            this.btnArrangeBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnArrangeBox.UseVisualStyleBackColor = false;
            this.btnArrangeBox.Click += new System.EventHandler(this.btnArrangeBox_Click);
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel13.ColumnCount = 4;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel13.Controls.Add(this.btnY_R_L, 3, 0);
            this.tableLayoutPanel13.Controls.Add(this.btnY_L_R, 2, 0);
            this.tableLayoutPanel13.Controls.Add(this.btnX_L_R, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.btnX_R_L, 1, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(4, 171);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(4, 0, 2, 0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(405, 60);
            this.tableLayoutPanel13.TabIndex = 51;
            // 
            // btnY_R_L
            // 
            this.btnY_R_L.AutoFont = true;
            this.btnY_R_L.AutoFontHeightRatio = 0.75F;
            this.btnY_R_L.AutoFontMax = 100F;
            this.btnY_R_L.AutoFontMin = 6F;
            this.btnY_R_L.AutoFontWidthRatio = 0.92F;
            this.btnY_R_L.AutoImage = true;
            this.btnY_R_L.AutoImageMaxRatio = 0.75F;
            this.btnY_R_L.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnY_R_L.AutoImageTint = true;
            this.btnY_R_L.BackColor = System.Drawing.SystemColors.Control;
            this.btnY_R_L.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnY_R_L.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnY_R_L.BackgroundImage")));
            this.btnY_R_L.BorderColor = System.Drawing.Color.Silver;
            this.btnY_R_L.BorderRadius = 10;
            this.btnY_R_L.BorderSize = 1;
            this.btnY_R_L.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnY_R_L.Corner = BeeGlobal.Corner.Right;
            this.btnY_R_L.DebounceResizeMs = 16;
            this.btnY_R_L.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnY_R_L.Enabled = false;
            this.btnY_R_L.FlatAppearance.BorderSize = 0;
            this.btnY_R_L.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnY_R_L.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnY_R_L.ForeColor = System.Drawing.Color.Black;
            this.btnY_R_L.Image = null;
            this.btnY_R_L.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnY_R_L.ImageDisabled = null;
            this.btnY_R_L.ImageHover = null;
            this.btnY_R_L.ImageNormal = null;
            this.btnY_R_L.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnY_R_L.ImagePressed = null;
            this.btnY_R_L.ImageTextSpacing = 6;
            this.btnY_R_L.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnY_R_L.ImageTintHover = System.Drawing.Color.Empty;
            this.btnY_R_L.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnY_R_L.ImageTintOpacity = 0.5F;
            this.btnY_R_L.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnY_R_L.IsCLick = false;
            this.btnY_R_L.IsNotChange = false;
            this.btnY_R_L.IsRect = false;
            this.btnY_R_L.IsUnGroup = false;
            this.btnY_R_L.Location = new System.Drawing.Point(303, 3);
            this.btnY_R_L.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnY_R_L.Multiline = false;
            this.btnY_R_L.Name = "btnY_R_L";
            this.btnY_R_L.Size = new System.Drawing.Size(99, 54);
            this.btnY_R_L.TabIndex = 5;
            this.btnY_R_L.Text = "Y ->";
            this.btnY_R_L.TextColor = System.Drawing.Color.Black;
            this.btnY_R_L.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnY_R_L.UseVisualStyleBackColor = false;
            this.btnY_R_L.Click += new System.EventHandler(this.btnY_R_L_Click);
            // 
            // btnY_L_R
            // 
            this.btnY_L_R.AutoFont = true;
            this.btnY_L_R.AutoFontHeightRatio = 0.75F;
            this.btnY_L_R.AutoFontMax = 100F;
            this.btnY_L_R.AutoFontMin = 6F;
            this.btnY_L_R.AutoFontWidthRatio = 0.92F;
            this.btnY_L_R.AutoImage = true;
            this.btnY_L_R.AutoImageMaxRatio = 0.75F;
            this.btnY_L_R.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnY_L_R.AutoImageTint = true;
            this.btnY_L_R.BackColor = System.Drawing.SystemColors.Control;
            this.btnY_L_R.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnY_L_R.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnY_L_R.BackgroundImage")));
            this.btnY_L_R.BorderColor = System.Drawing.Color.Silver;
            this.btnY_L_R.BorderRadius = 10;
            this.btnY_L_R.BorderSize = 1;
            this.btnY_L_R.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnY_L_R.Corner = BeeGlobal.Corner.None;
            this.btnY_L_R.DebounceResizeMs = 16;
            this.btnY_L_R.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnY_L_R.Enabled = false;
            this.btnY_L_R.FlatAppearance.BorderSize = 0;
            this.btnY_L_R.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnY_L_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnY_L_R.ForeColor = System.Drawing.Color.Black;
            this.btnY_L_R.Image = null;
            this.btnY_L_R.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnY_L_R.ImageDisabled = null;
            this.btnY_L_R.ImageHover = null;
            this.btnY_L_R.ImageNormal = null;
            this.btnY_L_R.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnY_L_R.ImagePressed = null;
            this.btnY_L_R.ImageTextSpacing = 6;
            this.btnY_L_R.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnY_L_R.ImageTintHover = System.Drawing.Color.Empty;
            this.btnY_L_R.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnY_L_R.ImageTintOpacity = 0.5F;
            this.btnY_L_R.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnY_L_R.IsCLick = false;
            this.btnY_L_R.IsNotChange = false;
            this.btnY_L_R.IsRect = false;
            this.btnY_L_R.IsUnGroup = false;
            this.btnY_L_R.Location = new System.Drawing.Point(202, 3);
            this.btnY_L_R.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.btnY_L_R.Multiline = false;
            this.btnY_L_R.Name = "btnY_L_R";
            this.btnY_L_R.Size = new System.Drawing.Size(101, 54);
            this.btnY_L_R.TabIndex = 4;
            this.btnY_L_R.Text = "Y ->";
            this.btnY_L_R.TextColor = System.Drawing.Color.Black;
            this.btnY_L_R.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnY_L_R.UseVisualStyleBackColor = false;
            this.btnY_L_R.Click += new System.EventHandler(this.btnY_L_R_Click);
            // 
            // btnX_L_R
            // 
            this.btnX_L_R.AutoFont = true;
            this.btnX_L_R.AutoFontHeightRatio = 0.75F;
            this.btnX_L_R.AutoFontMax = 100F;
            this.btnX_L_R.AutoFontMin = 6F;
            this.btnX_L_R.AutoFontWidthRatio = 0.92F;
            this.btnX_L_R.AutoImage = true;
            this.btnX_L_R.AutoImageMaxRatio = 0.75F;
            this.btnX_L_R.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnX_L_R.AutoImageTint = true;
            this.btnX_L_R.BackColor = System.Drawing.SystemColors.Control;
            this.btnX_L_R.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnX_L_R.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnX_L_R.BorderColor = System.Drawing.Color.Transparent;
            this.btnX_L_R.BorderRadius = 10;
            this.btnX_L_R.BorderSize = 1;
            this.btnX_L_R.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnX_L_R.Corner = BeeGlobal.Corner.Left;
            this.btnX_L_R.DebounceResizeMs = 16;
            this.btnX_L_R.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnX_L_R.FlatAppearance.BorderSize = 0;
            this.btnX_L_R.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnX_L_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.95313F);
            this.btnX_L_R.ForeColor = System.Drawing.Color.Black;
            this.btnX_L_R.Image = null;
            this.btnX_L_R.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnX_L_R.ImageDisabled = null;
            this.btnX_L_R.ImageHover = null;
            this.btnX_L_R.ImageNormal = null;
            this.btnX_L_R.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnX_L_R.ImagePressed = null;
            this.btnX_L_R.ImageTextSpacing = 6;
            this.btnX_L_R.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnX_L_R.ImageTintHover = System.Drawing.Color.Empty;
            this.btnX_L_R.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnX_L_R.ImageTintOpacity = 0.5F;
            this.btnX_L_R.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnX_L_R.IsCLick = true;
            this.btnX_L_R.IsNotChange = false;
            this.btnX_L_R.IsRect = false;
            this.btnX_L_R.IsUnGroup = false;
            this.btnX_L_R.Location = new System.Drawing.Point(3, 3);
            this.btnX_L_R.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnX_L_R.Multiline = false;
            this.btnX_L_R.Name = "btnX_L_R";
            this.btnX_L_R.Size = new System.Drawing.Size(98, 54);
            this.btnX_L_R.TabIndex = 2;
            this.btnX_L_R.Text = "X ->";
            this.btnX_L_R.TextColor = System.Drawing.Color.Black;
            this.btnX_L_R.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnX_L_R.UseVisualStyleBackColor = false;
            this.btnX_L_R.Click += new System.EventHandler(this.btnX_L_R_Click);
            // 
            // btnX_R_L
            // 
            this.btnX_R_L.AutoFont = true;
            this.btnX_R_L.AutoFontHeightRatio = 0.75F;
            this.btnX_R_L.AutoFontMax = 100F;
            this.btnX_R_L.AutoFontMin = 6F;
            this.btnX_R_L.AutoFontWidthRatio = 0.92F;
            this.btnX_R_L.AutoImage = true;
            this.btnX_R_L.AutoImageMaxRatio = 0.75F;
            this.btnX_R_L.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnX_R_L.AutoImageTint = true;
            this.btnX_R_L.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnX_R_L.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnX_R_L.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnX_R_L.BackgroundImage")));
            this.btnX_R_L.BorderColor = System.Drawing.Color.Silver;
            this.btnX_R_L.BorderRadius = 5;
            this.btnX_R_L.BorderSize = 1;
            this.btnX_R_L.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnX_R_L.Corner = BeeGlobal.Corner.None;
            this.btnX_R_L.DebounceResizeMs = 16;
            this.btnX_R_L.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnX_R_L.FlatAppearance.BorderSize = 0;
            this.btnX_R_L.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnX_R_L.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.42188F);
            this.btnX_R_L.ForeColor = System.Drawing.Color.Black;
            this.btnX_R_L.Image = null;
            this.btnX_R_L.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnX_R_L.ImageDisabled = null;
            this.btnX_R_L.ImageHover = null;
            this.btnX_R_L.ImageNormal = null;
            this.btnX_R_L.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnX_R_L.ImagePressed = null;
            this.btnX_R_L.ImageTextSpacing = 6;
            this.btnX_R_L.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnX_R_L.ImageTintHover = System.Drawing.Color.Empty;
            this.btnX_R_L.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnX_R_L.ImageTintOpacity = 0.5F;
            this.btnX_R_L.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnX_R_L.IsCLick = false;
            this.btnX_R_L.IsNotChange = false;
            this.btnX_R_L.IsRect = false;
            this.btnX_R_L.IsUnGroup = false;
            this.btnX_R_L.Location = new System.Drawing.Point(101, 0);
            this.btnX_R_L.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.btnX_R_L.Multiline = false;
            this.btnX_R_L.Name = "btnX_R_L";
            this.btnX_R_L.Size = new System.Drawing.Size(101, 57);
            this.btnX_R_L.TabIndex = 3;
            this.btnX_R_L.Text = "X <-";
            this.btnX_R_L.TextColor = System.Drawing.Color.Black;
            this.btnX_R_L.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnX_R_L.UseVisualStyleBackColor = false;
            this.btnX_R_L.Click += new System.EventHandler(this.btnX_R_L_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 5;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel5.Controls.Add(this.rjButton10, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnMore, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnEqual, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnLess, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.trackNumObject, 3, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(4, 37);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(403, 60);
            this.tableLayoutPanel5.TabIndex = 44;
            // 
            // rjButton10
            // 
            this.rjButton10.AutoFont = true;
            this.rjButton10.AutoFontHeightRatio = 0.75F;
            this.rjButton10.AutoFontMax = 100F;
            this.rjButton10.AutoFontMin = 6F;
            this.rjButton10.AutoFontWidthRatio = 0.92F;
            this.rjButton10.AutoImage = true;
            this.rjButton10.AutoImageMaxRatio = 0.75F;
            this.rjButton10.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton10.AutoImageTint = true;
            this.rjButton10.BackColor = System.Drawing.SystemColors.Control;
            this.rjButton10.BackgroundColor = System.Drawing.SystemColors.Control;
            this.rjButton10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton10.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton10.BorderRadius = 10;
            this.rjButton10.BorderSize = 1;
            this.rjButton10.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton10.Corner = BeeGlobal.Corner.Right;
            this.rjButton10.DebounceResizeMs = 16;
            this.rjButton10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton10.Enabled = false;
            this.rjButton10.FlatAppearance.BorderSize = 0;
            this.rjButton10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton10.ForeColor = System.Drawing.Color.Black;
            this.rjButton10.Image = null;
            this.rjButton10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton10.ImageDisabled = null;
            this.rjButton10.ImageHover = null;
            this.rjButton10.ImageNormal = null;
            this.rjButton10.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton10.ImagePressed = null;
            this.rjButton10.ImageTextSpacing = 6;
            this.rjButton10.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton10.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton10.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton10.ImageTintOpacity = 0.5F;
            this.rjButton10.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton10.IsCLick = false;
            this.rjButton10.IsNotChange = true;
            this.rjButton10.IsRect = false;
            this.rjButton10.IsUnGroup = false;
            this.rjButton10.Location = new System.Drawing.Point(399, 0);
            this.rjButton10.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton10.Multiline = false;
            this.rjButton10.Name = "rjButton10";
            this.rjButton10.Size = new System.Drawing.Size(4, 60);
            this.rjButton10.TabIndex = 38;
            this.rjButton10.TextColor = System.Drawing.Color.Black;
            this.rjButton10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton10.UseVisualStyleBackColor = false;
            // 
            // btnMore
            // 
            this.btnMore.AutoFont = true;
            this.btnMore.AutoFontHeightRatio = 0.75F;
            this.btnMore.AutoFontMax = 100F;
            this.btnMore.AutoFontMin = 6F;
            this.btnMore.AutoFontWidthRatio = 0.92F;
            this.btnMore.AutoImage = true;
            this.btnMore.AutoImageMaxRatio = 0.75F;
            this.btnMore.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMore.AutoImageTint = true;
            this.btnMore.BackColor = System.Drawing.Color.LightGray;
            this.btnMore.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnMore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMore.BorderColor = System.Drawing.Color.Transparent;
            this.btnMore.BorderRadius = 5;
            this.btnMore.BorderSize = 1;
            this.btnMore.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMore.Corner = BeeGlobal.Corner.None;
            this.btnMore.DebounceResizeMs = 16;
            this.btnMore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMore.FlatAppearance.BorderSize = 0;
            this.btnMore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.40625F);
            this.btnMore.ForeColor = System.Drawing.Color.Black;
            this.btnMore.Image = null;
            this.btnMore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMore.ImageDisabled = null;
            this.btnMore.ImageHover = null;
            this.btnMore.ImageNormal = null;
            this.btnMore.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMore.ImagePressed = null;
            this.btnMore.ImageTextSpacing = 6;
            this.btnMore.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMore.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMore.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMore.ImageTintOpacity = 0.5F;
            this.btnMore.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMore.IsCLick = false;
            this.btnMore.IsNotChange = false;
            this.btnMore.IsRect = false;
            this.btnMore.IsUnGroup = false;
            this.btnMore.Location = new System.Drawing.Point(120, 0);
            this.btnMore.Margin = new System.Windows.Forms.Padding(0);
            this.btnMore.Multiline = false;
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(60, 60);
            this.btnMore.TabIndex = 33;
            this.btnMore.Text = "More";
            this.btnMore.TextColor = System.Drawing.Color.Black;
            this.btnMore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMore.UseVisualStyleBackColor = false;
            this.btnMore.Click += new System.EventHandler(this.rjButton3_Click_2);
            // 
            // btnEqual
            // 
            this.btnEqual.AutoFont = true;
            this.btnEqual.AutoFontHeightRatio = 0.75F;
            this.btnEqual.AutoFontMax = 100F;
            this.btnEqual.AutoFontMin = 6F;
            this.btnEqual.AutoFontWidthRatio = 0.92F;
            this.btnEqual.AutoImage = true;
            this.btnEqual.AutoImageMaxRatio = 0.75F;
            this.btnEqual.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEqual.AutoImageTint = true;
            this.btnEqual.BackColor = System.Drawing.Color.Gray;
            this.btnEqual.BackgroundColor = System.Drawing.Color.Gray;
            this.btnEqual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEqual.BorderColor = System.Drawing.Color.LightGray;
            this.btnEqual.BorderRadius = 5;
            this.btnEqual.BorderSize = 1;
            this.btnEqual.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEqual.Corner = BeeGlobal.Corner.None;
            this.btnEqual.DebounceResizeMs = 16;
            this.btnEqual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEqual.FlatAppearance.BorderSize = 0;
            this.btnEqual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.9375F);
            this.btnEqual.ForeColor = System.Drawing.Color.Black;
            this.btnEqual.Image = null;
            this.btnEqual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEqual.ImageDisabled = null;
            this.btnEqual.ImageHover = null;
            this.btnEqual.ImageNormal = null;
            this.btnEqual.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEqual.ImagePressed = null;
            this.btnEqual.ImageTextSpacing = 6;
            this.btnEqual.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEqual.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEqual.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEqual.ImageTintOpacity = 0.5F;
            this.btnEqual.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEqual.IsCLick = true;
            this.btnEqual.IsNotChange = false;
            this.btnEqual.IsRect = false;
            this.btnEqual.IsUnGroup = false;
            this.btnEqual.Location = new System.Drawing.Point(60, 0);
            this.btnEqual.Margin = new System.Windows.Forms.Padding(0);
            this.btnEqual.Multiline = false;
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(60, 60);
            this.btnEqual.TabIndex = 32;
            this.btnEqual.Text = "Equal";
            this.btnEqual.TextColor = System.Drawing.Color.Black;
            this.btnEqual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEqual.UseVisualStyleBackColor = false;
            this.btnEqual.Click += new System.EventHandler(this.rjButton6_Click);
            // 
            // btnLess
            // 
            this.btnLess.AutoFont = true;
            this.btnLess.AutoFontHeightRatio = 0.75F;
            this.btnLess.AutoFontMax = 100F;
            this.btnLess.AutoFontMin = 6F;
            this.btnLess.AutoFontWidthRatio = 0.92F;
            this.btnLess.AutoImage = true;
            this.btnLess.AutoImageMaxRatio = 0.75F;
            this.btnLess.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLess.AutoImageTint = true;
            this.btnLess.BackColor = System.Drawing.Color.LightGray;
            this.btnLess.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnLess.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLess.BorderColor = System.Drawing.Color.Transparent;
            this.btnLess.BorderRadius = 10;
            this.btnLess.BorderSize = 1;
            this.btnLess.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLess.Corner = BeeGlobal.Corner.Left;
            this.btnLess.DebounceResizeMs = 16;
            this.btnLess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLess.FlatAppearance.BorderSize = 0;
            this.btnLess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLess.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.14063F);
            this.btnLess.ForeColor = System.Drawing.Color.Black;
            this.btnLess.Image = null;
            this.btnLess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLess.ImageDisabled = null;
            this.btnLess.ImageHover = null;
            this.btnLess.ImageNormal = null;
            this.btnLess.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLess.ImagePressed = null;
            this.btnLess.ImageTextSpacing = 6;
            this.btnLess.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLess.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLess.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLess.ImageTintOpacity = 0.5F;
            this.btnLess.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLess.IsCLick = false;
            this.btnLess.IsNotChange = false;
            this.btnLess.IsRect = false;
            this.btnLess.IsUnGroup = false;
            this.btnLess.Location = new System.Drawing.Point(0, 0);
            this.btnLess.Margin = new System.Windows.Forms.Padding(0);
            this.btnLess.Multiline = false;
            this.btnLess.Name = "btnLess";
            this.btnLess.Size = new System.Drawing.Size(60, 60);
            this.btnLess.TabIndex = 31;
            this.btnLess.Text = "Less";
            this.btnLess.TextColor = System.Drawing.Color.Black;
            this.btnLess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLess.UseVisualStyleBackColor = false;
            this.btnLess.Click += new System.EventHandler(this.rjButton7_Click);
            // 
            // trackNumObject
            // 
            this.trackNumObject.AutoShowTextbox = false;
            this.trackNumObject.AutoSizeTextbox = true;
            this.trackNumObject.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.trackNumObject.BackColor = System.Drawing.SystemColors.Control;
            this.trackNumObject.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.trackNumObject.BorderRadius = 6;
            this.trackNumObject.ButtonMaxSize = 64;
            this.trackNumObject.ButtonMinSize = 24;
            this.trackNumObject.Decimals = 0;
            this.trackNumObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackNumObject.ElementGap = 6;
            this.trackNumObject.FillTextboxToAvailable = true;
            this.trackNumObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackNumObject.InnerPadding = new System.Windows.Forms.Padding(6);
            this.trackNumObject.KeyboardStep = 1F;
            this.trackNumObject.Location = new System.Drawing.Point(180, 0);
            this.trackNumObject.Margin = new System.Windows.Forms.Padding(0);
            this.trackNumObject.Max = 100F;
            this.trackNumObject.MaxTextboxWidth = 0;
            this.trackNumObject.Min = 0F;
            this.trackNumObject.MinimumSize = new System.Drawing.Size(120, 32);
            this.trackNumObject.MinTextboxWidth = 16;
            this.trackNumObject.Name = "trackNumObject";
            this.trackNumObject.Size = new System.Drawing.Size(219, 60);
            this.trackNumObject.SnapToStep = true;
            this.trackNumObject.StartWithTextboxHidden = false;
            this.trackNumObject.Step = 1F;
            this.trackNumObject.TabIndex = 34;
            this.trackNumObject.TextboxFontSize = 30F;
            this.trackNumObject.TextboxSidePadding = 12;
            this.trackNumObject.TextboxWidth = 56;
            this.trackNumObject.UnitText = "";
            this.trackNumObject.Value = 0F;
            this.trackNumObject.WheelStep = 1F;
            this.trackNumObject.ValueChanged += new System.Action<float>(this.trackNumObject_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 12, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 25);
            this.label3.TabIndex = 44;
            this.label3.Text = "Limited Counter";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabP1
            // 
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(415, 788);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.dashboardLabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.layoutSetLearning, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(409, 782);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // trackScore
            // 
            this.trackScore.AutoShowTextbox = true;
            this.trackScore.AutoSizeTextbox = true;
            this.trackScore.BackColor = System.Drawing.SystemColors.Control;
            this.trackScore.BarLeftGap = 20;
            this.trackScore.BarRightGap = 6;
            this.trackScore.ChromeGap = 8;
            this.trackScore.ChromeWidthRatio = 0.14F;
            this.trackScore.ColorBorder = System.Drawing.Color.DarkGray;
            this.trackScore.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackScore.ColorScale = System.Drawing.Color.DarkGray;
            this.trackScore.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorTrack = System.Drawing.Color.DarkGray;
            this.trackScore.Decimals = 0;
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(3, 517);
            this.trackScore.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
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
            this.trackScore.Size = new System.Drawing.Size(393, 49);
            this.trackScore.SnapToStep = true;
            this.trackScore.StartWithTextboxHidden = true;
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 68;
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
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(4, 73);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 4, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(109, 25);
            this.label15.TabIndex = 66;
            this.label15.Text = "Set Model";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(10, 148);
            this.label6.Margin = new System.Windows.Forms.Padding(10, 5, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(386, 25);
            this.label6.TabIndex = 65;
            this.label6.Text = "List labels";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dashboardLabel
            // 
            this.dashboardLabel.BackColor = System.Drawing.Color.White;
            this.dashboardLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboardLabel.ForeColor = System.Drawing.Color.Black;
            this.dashboardLabel.LabelColumnRatio = 0.38F;
            this.dashboardLabel.Location = new System.Drawing.Point(3, 176);
            this.dashboardLabel.Name = "dashboardLabel";
            this.dashboardLabel.Size = new System.Drawing.Size(393, 300);
            this.dashboardLabel.TabIndex = 0;
            this.dashboardLabel.ValueMax = 100000;
            this.dashboardLabel.ValueMin = 0;
            this.dashboardLabel.ValueWidthRatio = 0.6F;
            // 
            // layoutSetLearning
            // 
            this.layoutSetLearning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.layoutSetLearning.ColumnCount = 4;
            this.layoutSetLearning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSetLearning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSetLearning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSetLearning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSetLearning.Controls.Add(this.btnReload, 3, 0);
            this.layoutSetLearning.Controls.Add(this.btnRemoveModel, 2, 0);
            this.layoutSetLearning.Controls.Add(this.btnAddModel, 1, 0);
            this.layoutSetLearning.Controls.Add(this.cbListModel, 0, 0);
            this.layoutSetLearning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSetLearning.Location = new System.Drawing.Point(4, 98);
            this.layoutSetLearning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.layoutSetLearning.Name = "layoutSetLearning";
            this.layoutSetLearning.RowCount = 1;
            this.layoutSetLearning.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSetLearning.Size = new System.Drawing.Size(391, 45);
            this.layoutSetLearning.TabIndex = 50;
            // 
            // btnReload
            // 
            this.btnReload.AutoFont = true;
            this.btnReload.AutoFontHeightRatio = 0.9F;
            this.btnReload.AutoFontMax = 100F;
            this.btnReload.AutoFontMin = 7F;
            this.btnReload.AutoFontWidthRatio = 0.92F;
            this.btnReload.AutoImage = true;
            this.btnReload.AutoImageMaxRatio = 0.75F;
            this.btnReload.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReload.AutoImageTint = true;
            this.btnReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnReload.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnReload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReload.BackgroundImage")));
            this.btnReload.BorderColor = System.Drawing.Color.Silver;
            this.btnReload.BorderRadius = 4;
            this.btnReload.BorderSize = 1;
            this.btnReload.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReload.Corner = BeeGlobal.Corner.Both;
            this.btnReload.DebounceResizeMs = 16;
            this.btnReload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnReload.ForeColor = System.Drawing.Color.Black;
            this.btnReload.Image = null;
            this.btnReload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReload.ImageDisabled = null;
            this.btnReload.ImageHover = null;
            this.btnReload.ImageNormal = null;
            this.btnReload.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReload.ImagePressed = null;
            this.btnReload.ImageTextSpacing = 6;
            this.btnReload.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReload.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReload.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReload.ImageTintOpacity = 0.5F;
            this.btnReload.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReload.IsCLick = false;
            this.btnReload.IsNotChange = true;
            this.btnReload.IsRect = false;
            this.btnReload.IsUnGroup = true;
            this.btnReload.Location = new System.Drawing.Point(333, 2);
            this.btnReload.Margin = new System.Windows.Forms.Padding(2);
            this.btnReload.Multiline = false;
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(56, 41);
            this.btnReload.TabIndex = 39;
            this.btnReload.Text = "Re.Load";
            this.btnReload.TextColor = System.Drawing.Color.Black;
            this.btnReload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnRemoveModel
            // 
            this.btnRemoveModel.AutoFont = true;
            this.btnRemoveModel.AutoFontHeightRatio = 0.9F;
            this.btnRemoveModel.AutoFontMax = 100F;
            this.btnRemoveModel.AutoFontMin = 7F;
            this.btnRemoveModel.AutoFontWidthRatio = 0.92F;
            this.btnRemoveModel.AutoImage = true;
            this.btnRemoveModel.AutoImageMaxRatio = 0.75F;
            this.btnRemoveModel.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRemoveModel.AutoImageTint = true;
            this.btnRemoveModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnRemoveModel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnRemoveModel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemoveModel.BackgroundImage")));
            this.btnRemoveModel.BorderColor = System.Drawing.Color.Silver;
            this.btnRemoveModel.BorderRadius = 4;
            this.btnRemoveModel.BorderSize = 1;
            this.btnRemoveModel.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRemoveModel.Corner = BeeGlobal.Corner.Both;
            this.btnRemoveModel.DebounceResizeMs = 16;
            this.btnRemoveModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveModel.FlatAppearance.BorderSize = 0;
            this.btnRemoveModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.734375F);
            this.btnRemoveModel.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveModel.Image = null;
            this.btnRemoveModel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveModel.ImageDisabled = null;
            this.btnRemoveModel.ImageHover = null;
            this.btnRemoveModel.ImageNormal = null;
            this.btnRemoveModel.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRemoveModel.ImagePressed = null;
            this.btnRemoveModel.ImageTextSpacing = 6;
            this.btnRemoveModel.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRemoveModel.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRemoveModel.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRemoveModel.ImageTintOpacity = 0.5F;
            this.btnRemoveModel.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRemoveModel.IsCLick = false;
            this.btnRemoveModel.IsNotChange = true;
            this.btnRemoveModel.IsRect = false;
            this.btnRemoveModel.IsUnGroup = true;
            this.btnRemoveModel.Location = new System.Drawing.Point(273, 2);
            this.btnRemoveModel.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveModel.Multiline = false;
            this.btnRemoveModel.Name = "btnRemoveModel";
            this.btnRemoveModel.Size = new System.Drawing.Size(56, 41);
            this.btnRemoveModel.TabIndex = 38;
            this.btnRemoveModel.Text = "Delete";
            this.btnRemoveModel.TextColor = System.Drawing.Color.Black;
            this.btnRemoveModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveModel.UseVisualStyleBackColor = false;
            this.btnRemoveModel.Click += new System.EventHandler(this.btnRemoveModel_Click);
            // 
            // btnAddModel
            // 
            this.btnAddModel.AutoFont = true;
            this.btnAddModel.AutoFontHeightRatio = 0.9F;
            this.btnAddModel.AutoFontMax = 100F;
            this.btnAddModel.AutoFontMin = 7F;
            this.btnAddModel.AutoFontWidthRatio = 0.92F;
            this.btnAddModel.AutoImage = true;
            this.btnAddModel.AutoImageMaxRatio = 0.75F;
            this.btnAddModel.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAddModel.AutoImageTint = true;
            this.btnAddModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnAddModel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnAddModel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddModel.BackgroundImage")));
            this.btnAddModel.BorderColor = System.Drawing.Color.Silver;
            this.btnAddModel.BorderRadius = 4;
            this.btnAddModel.BorderSize = 1;
            this.btnAddModel.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAddModel.Corner = BeeGlobal.Corner.Both;
            this.btnAddModel.DebounceResizeMs = 16;
            this.btnAddModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddModel.FlatAppearance.BorderSize = 0;
            this.btnAddModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnAddModel.ForeColor = System.Drawing.Color.Black;
            this.btnAddModel.Image = null;
            this.btnAddModel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddModel.ImageDisabled = null;
            this.btnAddModel.ImageHover = null;
            this.btnAddModel.ImageNormal = null;
            this.btnAddModel.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAddModel.ImagePressed = null;
            this.btnAddModel.ImageTextSpacing = 6;
            this.btnAddModel.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAddModel.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAddModel.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAddModel.ImageTintOpacity = 0.5F;
            this.btnAddModel.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAddModel.IsCLick = false;
            this.btnAddModel.IsNotChange = true;
            this.btnAddModel.IsRect = false;
            this.btnAddModel.IsUnGroup = true;
            this.btnAddModel.Location = new System.Drawing.Point(213, 2);
            this.btnAddModel.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddModel.Multiline = false;
            this.btnAddModel.Name = "btnAddModel";
            this.btnAddModel.Size = new System.Drawing.Size(56, 41);
            this.btnAddModel.TabIndex = 37;
            this.btnAddModel.Text = "Import";
            this.btnAddModel.TextColor = System.Drawing.Color.Black;
            this.btnAddModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddModel.UseVisualStyleBackColor = false;
            this.btnAddModel.Click += new System.EventHandler(this.btnAddModel_Click);
            // 
            // cbListModel
            // 
            this.cbListModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbListModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbListModel.FormattingEnabled = true;
            this.cbListModel.Location = new System.Drawing.Point(2, 2);
            this.cbListModel.Margin = new System.Windows.Forms.Padding(2);
            this.cbListModel.Name = "cbListModel";
            this.cbListModel.Size = new System.Drawing.Size(207, 41);
            this.cbListModel.TabIndex = 36;
            this.cbListModel.SelectionChangeCommitted += new System.EventHandler(this.cbListModel_SelectionChangeCommitted);
            this.cbListModel.SelectedValueChanged += new System.EventHandler(this.cbListModel_SelectedValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 489);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 10, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(393, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTest
            // 
            this.btnTest.AutoFont = true;
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
            this.btnTest.BorderRadius = 1;
            this.btnTest.BorderSize = 1;
            this.btnTest.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.DebounceResizeMs = 16;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 42.71875F, System.Drawing.FontStyle.Bold);
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
            this.btnTest.Location = new System.Drawing.Point(4, 584);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(391, 100);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click_1);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(4, 4);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 4, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(149, 25);
            this.label9.TabIndex = 38;
            this.label9.Text = "Search Range";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnCropFull, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCropHalt, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 29);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 0, 2, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(393, 40);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // btnCropFull
            // 
            this.btnCropFull.AutoFont = true;
            this.btnCropFull.AutoFontHeightRatio = 0.75F;
            this.btnCropFull.AutoFontMax = 100F;
            this.btnCropFull.AutoFontMin = 6F;
            this.btnCropFull.AutoFontWidthRatio = 0.92F;
            this.btnCropFull.AutoImage = true;
            this.btnCropFull.AutoImageMaxRatio = 0.75F;
            this.btnCropFull.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropFull.AutoImageTint = true;
            this.btnCropFull.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropFull.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropFull.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.DebounceResizeMs = 16;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
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
            this.btnCropFull.Location = new System.Drawing.Point(196, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Multiline = false;
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(194, 40);
            this.btnCropFull.TabIndex = 3;
            this.btnCropFull.Text = "Partial";
            this.btnCropFull.TextColor = System.Drawing.Color.Black;
            this.btnCropFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropFull.UseVisualStyleBackColor = false;
            this.btnCropFull.Click += new System.EventHandler(this.btnCropFull_Click_1);
            // 
            // btnCropHalt
            // 
            this.btnCropHalt.AutoFont = true;
            this.btnCropHalt.AutoFontHeightRatio = 0.75F;
            this.btnCropHalt.AutoFontMax = 100F;
            this.btnCropHalt.AutoFontMin = 6F;
            this.btnCropHalt.AutoFontWidthRatio = 0.92F;
            this.btnCropHalt.AutoImage = true;
            this.btnCropHalt.AutoImageMaxRatio = 0.75F;
            this.btnCropHalt.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropHalt.AutoImageTint = true;
            this.btnCropHalt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropHalt.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.DebounceResizeMs = 16;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
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
            this.btnCropHalt.Location = new System.Drawing.Point(3, 0);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCropHalt.Multiline = false;
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(193, 40);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click_1);
            // 
            // tabYolo
            // 
            this.tabYolo.Controls.Add(this.tabP1);
            this.tabYolo.Controls.Add(this.tabPage4);
            this.tabYolo.Controls.Add(this.tabPage1);
            this.tabYolo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabYolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabYolo.Location = new System.Drawing.Point(0, 0);
            this.tabYolo.Name = "tabYolo";
            this.tabYolo.SelectedIndex = 0;
            this.tabYolo.Size = new System.Drawing.Size(423, 826);
            this.tabYolo.TabIndex = 18;
            this.tabYolo.SelectedIndexChanged += new System.EventHandler(this.tabYolo_SelectedIndexChanged);
            // 
            // ToolYolo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.tabYolo);
            this.DoubleBuffered = true;
            this.Name = "ToolYolo";
            this.Size = new System.Drawing.Size(423, 826);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel17.ResumeLayout(false);
            this.tableLayoutPanel17.PerformLayout();
            this.tableLayoutPanel19.ResumeLayout(false);
            this.tableLayoutPanel20.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel21.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.layoutSetLearning.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabYolo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmCheckFist;
        private System.ComponentModel.BackgroundWorker workLoadModel;
        private System.ComponentModel.BackgroundWorker workTrain;
        private GroupControl.OK_Cancel oK_Cancel1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private RJButton btnTraining;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label txtPercent;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private RJButton rjButton13;
        private RJButton rjButton14;
        private RJButton rjButton15;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private CustomNumericEx numEpoch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel17;
        private RJButton btnCheck;
        private RJButton btnPathDataSet;
        private System.Windows.Forms.TextBox tbDataSet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel19;
        private RJButton btnSaveDataSet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel20;
        private RJButton btnLoadImg;
        private RJButton btnCropTemp;
        private System.Windows.Forms.ComboBox cbLabels;
        private System.Windows.Forms.Label label13;
        private Cyotek.Windows.Forms.ImageBox imgCrop;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel21;
        private System.Windows.Forms.Label label14;
        private RJButton btnArrangeBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private RJButton btnY_R_L;
        private RJButton btnY_L_R;
        private RJButton btnX_L_R;
        private RJButton btnX_R_L;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton rjButton10;
        private RJButton btnMore;
        private RJButton btnEqual;
        private RJButton btnLess;
        private CustomNumericEx trackNumObject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AdjustBarEx trackScore;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label6;
        private DashboardListCompact dashboardLabel;
        private System.Windows.Forms.TableLayoutPanel layoutSetLearning;
        private RJButton btnReload;
        private RJButton btnRemoveModel;
        private RJButton btnAddModel;
        public System.Windows.Forms.ComboBox cbListModel;
        private System.Windows.Forms.Label label8;
        private RJButton btnTest;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TabControl tabYolo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private RJButton btnEnable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private RJButton rjButton2;
        private RJButton btnString;
        private RJButton btnBits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAddPLC;
        private System.Windows.Forms.Label label5;
    }
}
