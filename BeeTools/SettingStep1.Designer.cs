
using BeeGlobal;

namespace BeeInterface
{
    partial class SettingStep1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingStep1));
            this.workRead = new System.ComponentModel.BackgroundWorker();
            this.tmDelaySend = new System.Windows.Forms.Timer(this.components);
            this.tableLayout1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDownLoadPara = new BeeInterface.RJButton();
            this.AdjOffSetY = new BeeInterface.AdjustBar();
            this.label13 = new System.Windows.Forms.Label();
            this.AdjOffsetX = new BeeInterface.AdjustBar();
            this.label12 = new System.Windows.Forms.Label();
            this.AdjHeight = new BeeInterface.AdjustBar();
            this.label11 = new System.Windows.Forms.Label();
            this.AdjWidth = new BeeInterface.AdjustBar();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.trackShift = new BeeInterface.AdjustBar();
            this.label3 = new System.Windows.Forms.Label();
            this.trackGain = new BeeInterface.AdjustBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackExposure = new BeeInterface.AdjustBar();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCenterY = new BeeInterface.RJButton();
            this.btnCenterX = new BeeInterface.RJButton();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new BeeInterface.RJButton();
            this.btnNextStep = new BeeInterface.RJButton();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOn = new BeeInterface.RJButton();
            this.btnOFF = new BeeInterface.RJButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLight3 = new BeeInterface.RJButton();
            this.btnLight1 = new BeeInterface.RJButton();
            this.btnLight2 = new BeeInterface.RJButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.AdDelayOutput = new BeeInterface.AdjustBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOutsideIn = new BeeInterface.RJButton();
            this.btnInsideOut = new BeeInterface.RJButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AdDelayTrig = new BeeInterface.AdjustBar();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExternal = new BeeInterface.RJButton();
            this.btnInternal = new BeeInterface.RJButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.workReadPara = new System.ComponentModel.BackgroundWorker();
            this.tableLayout1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayout1
            // 
            this.tableLayout1.AutoScroll = true;
            this.tableLayout1.ColumnCount = 1;
            this.tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout1.Controls.Add(this.btnDownLoadPara, 0, 0);
            this.tableLayout1.Controls.Add(this.AdjOffSetY, 0, 15);
            this.tableLayout1.Controls.Add(this.label13, 0, 14);
            this.tableLayout1.Controls.Add(this.AdjOffsetX, 0, 13);
            this.tableLayout1.Controls.Add(this.label12, 0, 12);
            this.tableLayout1.Controls.Add(this.AdjHeight, 0, 11);
            this.tableLayout1.Controls.Add(this.label11, 0, 10);
            this.tableLayout1.Controls.Add(this.AdjWidth, 0, 9);
            this.tableLayout1.Controls.Add(this.label10, 0, 8);
            this.tableLayout1.Controls.Add(this.label5, 0, 7);
            this.tableLayout1.Controls.Add(this.trackShift, 0, 6);
            this.tableLayout1.Controls.Add(this.label3, 0, 5);
            this.tableLayout1.Controls.Add(this.trackGain, 0, 4);
            this.tableLayout1.Controls.Add(this.label2, 0, 3);
            this.tableLayout1.Controls.Add(this.label4, 0, 1);
            this.tableLayout1.Controls.Add(this.trackExposure, 0, 2);
            this.tableLayout1.Controls.Add(this.tableLayoutPanel5, 0, 16);
            this.tableLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayout1.Location = new System.Drawing.Point(3, 3);
            this.tableLayout1.Name = "tableLayout1";
            this.tableLayout1.RowCount = 18;
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout1.Size = new System.Drawing.Size(380, 794);
            this.tableLayout1.TabIndex = 52;
            // 
            // btnDownLoadPara
            // 
            this.btnDownLoadPara.BackColor = System.Drawing.Color.Transparent;
            this.btnDownLoadPara.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnDownLoadPara.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDownLoadPara.BackgroundImage")));
            this.btnDownLoadPara.BorderColor = System.Drawing.Color.Transparent;
            this.btnDownLoadPara.BorderRadius = 5;
            this.btnDownLoadPara.BorderSize = 1;
            this.btnDownLoadPara.ButtonImage = null;
            this.btnDownLoadPara.Corner = BeeGlobal.Corner.Both;
            this.btnDownLoadPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDownLoadPara.FlatAppearance.BorderSize = 0;
            this.btnDownLoadPara.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownLoadPara.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownLoadPara.ForeColor = System.Drawing.Color.Black;
            this.btnDownLoadPara.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownLoadPara.IsCLick = false;
            this.btnDownLoadPara.IsNotChange = true;
            this.btnDownLoadPara.IsRect = false;
            this.btnDownLoadPara.IsUnGroup = false;
            this.btnDownLoadPara.Location = new System.Drawing.Point(3, 3);
            this.btnDownLoadPara.Name = "btnDownLoadPara";
            this.btnDownLoadPara.Size = new System.Drawing.Size(374, 58);
            this.btnDownLoadPara.TabIndex = 79;
            this.btnDownLoadPara.Text = "DownLoad Parameter Camera";
            this.btnDownLoadPara.TextColor = System.Drawing.Color.Black;
            this.btnDownLoadPara.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownLoadPara.UseVisualStyleBackColor = false;
            this.btnDownLoadPara.Click += new System.EventHandler(this.btnDownLoadPara_Click);
            // 
            // AdjOffSetY
            // 
            this.AdjOffSetY.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjOffSetY.Location = new System.Drawing.Point(20, 643);
            this.AdjOffSetY.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.AdjOffSetY.Max = 100F;
            this.AdjOffSetY.Min = 0F;
            this.AdjOffSetY.Name = "AdjOffSetY";
            this.AdjOffSetY.Size = new System.Drawing.Size(357, 50);
            this.AdjOffSetY.Step = 1F;
            this.AdjOffSetY.TabIndex = 78;
            this.AdjOffSetY.Value = 1F;
            this.AdjOffSetY.ValueChanged += new System.Action<float>(this.AdjOffSetY_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(15, 617);
            this.label13.Margin = new System.Windows.Forms.Padding(15, 5, 3, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(362, 20);
            this.label13.TabIndex = 77;
            this.label13.Text = "OffSet Y";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdjOffsetX
            // 
            this.AdjOffsetX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjOffsetX.Location = new System.Drawing.Point(20, 559);
            this.AdjOffsetX.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.AdjOffsetX.Max = 100F;
            this.AdjOffsetX.Min = 0F;
            this.AdjOffsetX.Name = "AdjOffsetX";
            this.AdjOffsetX.Size = new System.Drawing.Size(357, 50);
            this.AdjOffsetX.Step = 1F;
            this.AdjOffsetX.TabIndex = 76;
            this.AdjOffsetX.Value = 1F;
            this.AdjOffsetX.ValueChanged += new System.Action<float>(this.AdjOffsetX_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(15, 533);
            this.label12.Margin = new System.Windows.Forms.Padding(15, 5, 3, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(362, 20);
            this.label12.TabIndex = 75;
            this.label12.Text = "OffSet X";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdjHeight
            // 
            this.AdjHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjHeight.Location = new System.Drawing.Point(20, 475);
            this.AdjHeight.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.AdjHeight.Max = 100F;
            this.AdjHeight.Min = 0F;
            this.AdjHeight.Name = "AdjHeight";
            this.AdjHeight.Size = new System.Drawing.Size(357, 50);
            this.AdjHeight.Step = 1F;
            this.AdjHeight.TabIndex = 74;
            this.AdjHeight.Value = 1F;
            this.AdjHeight.ValueChanged += new System.Action<float>(this.AdjHeight_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(15, 449);
            this.label11.Margin = new System.Windows.Forms.Padding(15, 5, 3, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(362, 20);
            this.label11.TabIndex = 73;
            this.label11.Text = "Hight";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdjWidth
            // 
            this.AdjWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjWidth.Location = new System.Drawing.Point(20, 391);
            this.AdjWidth.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.AdjWidth.Max = 100F;
            this.AdjWidth.Min = 0F;
            this.AdjWidth.Name = "AdjWidth";
            this.AdjWidth.Size = new System.Drawing.Size(357, 50);
            this.AdjWidth.Step = 1F;
            this.AdjWidth.TabIndex = 72;
            this.AdjWidth.Value = 1F;
            this.AdjWidth.ValueChanged += new System.Action<float>(this.AdjWidth_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(15, 365);
            this.label10.Margin = new System.Windows.Forms.Padding(15, 5, 3, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(362, 20);
            this.label10.TabIndex = 71;
            this.label10.Text = "Width";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 333);
            this.label5.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(377, 24);
            this.label5.TabIndex = 70;
            this.label5.Text = "ReSolution";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackShift
            // 
            this.trackShift.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackShift.Location = new System.Drawing.Point(10, 275);
            this.trackShift.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackShift.Max = 100F;
            this.trackShift.Min = 0F;
            this.trackShift.Name = "trackShift";
            this.trackShift.Size = new System.Drawing.Size(367, 50);
            this.trackShift.Step = 1F;
            this.trackShift.TabIndex = 69;
            this.trackShift.Value = 1F;
            this.trackShift.ValueChanged += new System.Action<float>(this.trackShift_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 245);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(377, 24);
            this.label3.TabIndex = 68;
            this.label3.Text = "Shift";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackGain
            // 
            this.trackGain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackGain.Location = new System.Drawing.Point(10, 187);
            this.trackGain.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackGain.Max = 100F;
            this.trackGain.Min = 0F;
            this.trackGain.Name = "trackGain";
            this.trackGain.Size = new System.Drawing.Size(367, 50);
            this.trackGain.Step = 1F;
            this.trackGain.TabIndex = 67;
            this.trackGain.Value = 1F;
            this.trackGain.ValueChanged += new System.Action<float>(this.trackGain_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 157);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(377, 24);
            this.label2.TabIndex = 66;
            this.label2.Text = "Gain";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 69);
            this.label4.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(377, 24);
            this.label4.TabIndex = 65;
            this.label4.Text = "Exposue Time";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackExposure
            // 
            this.trackExposure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackExposure.Location = new System.Drawing.Point(10, 99);
            this.trackExposure.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackExposure.Max = 100F;
            this.trackExposure.Min = 0F;
            this.trackExposure.Name = "trackExposure";
            this.trackExposure.Size = new System.Drawing.Size(367, 50);
            this.trackExposure.Step = 1F;
            this.trackExposure.TabIndex = 64;
            this.trackExposure.Value = 1F;
            this.trackExposure.ValueChanged += new System.Action<float>(this.trackExposure_ValueChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.btnCenterY, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnCenterX, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(20, 699);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(357, 44);
            this.tableLayoutPanel5.TabIndex = 80;
            // 
            // btnCenterY
            // 
            this.btnCenterY.BackColor = System.Drawing.Color.Transparent;
            this.btnCenterY.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCenterY.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCenterY.BackgroundImage")));
            this.btnCenterY.BorderColor = System.Drawing.Color.Transparent;
            this.btnCenterY.BorderRadius = 5;
            this.btnCenterY.BorderSize = 1;
            this.btnCenterY.ButtonImage = null;
            this.btnCenterY.Corner = BeeGlobal.Corner.Both;
            this.btnCenterY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenterY.FlatAppearance.BorderSize = 0;
            this.btnCenterY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCenterY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCenterY.ForeColor = System.Drawing.Color.Black;
            this.btnCenterY.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCenterY.IsCLick = false;
            this.btnCenterY.IsNotChange = true;
            this.btnCenterY.IsRect = false;
            this.btnCenterY.IsUnGroup = false;
            this.btnCenterY.Location = new System.Drawing.Point(181, 3);
            this.btnCenterY.Name = "btnCenterY";
            this.btnCenterY.Size = new System.Drawing.Size(173, 38);
            this.btnCenterY.TabIndex = 60;
            this.btnCenterY.Text = "CenterY";
            this.btnCenterY.TextColor = System.Drawing.Color.Black;
            this.btnCenterY.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCenterY.UseVisualStyleBackColor = false;
            this.btnCenterY.Click += new System.EventHandler(this.btnCenterY_Click);
            // 
            // btnCenterX
            // 
            this.btnCenterX.BackColor = System.Drawing.Color.Transparent;
            this.btnCenterX.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCenterX.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCenterX.BackgroundImage")));
            this.btnCenterX.BorderColor = System.Drawing.Color.Transparent;
            this.btnCenterX.BorderRadius = 5;
            this.btnCenterX.BorderSize = 1;
            this.btnCenterX.ButtonImage = null;
            this.btnCenterX.Corner = BeeGlobal.Corner.Both;
            this.btnCenterX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenterX.FlatAppearance.BorderSize = 0;
            this.btnCenterX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCenterX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCenterX.ForeColor = System.Drawing.Color.Black;
            this.btnCenterX.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCenterX.IsCLick = false;
            this.btnCenterX.IsNotChange = true;
            this.btnCenterX.IsRect = false;
            this.btnCenterX.IsUnGroup = false;
            this.btnCenterX.Location = new System.Drawing.Point(3, 3);
            this.btnCenterX.Name = "btnCenterX";
            this.btnCenterX.Size = new System.Drawing.Size(172, 38);
            this.btnCenterX.TabIndex = 59;
            this.btnCenterX.Text = "CenterX";
            this.btnCenterX.TextColor = System.Drawing.Color.Black;
            this.btnCenterX.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCenterX.UseVisualStyleBackColor = false;
            this.btnCenterX.Click += new System.EventHandler(this.btnCenterX_Click);
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.2844F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.7156F));
            this.tableLayoutPanel11.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.btnNextStep, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 841);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(394, 74);
            this.tableLayoutPanel11.TabIndex = 59;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 5;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.ButtonImage = null;
            this.btnCancel.Corner = BeeGlobal.Corner.Both;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.IsCLick = false;
            this.btnCancel.IsNotChange = true;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(264, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 68);
            this.btnCancel.TabIndex = 58;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.Black;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNextStep
            // 
            this.btnNextStep.BackColor = System.Drawing.Color.Transparent;
            this.btnNextStep.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnNextStep.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNextStep.BackgroundImage")));
            this.btnNextStep.BorderColor = System.Drawing.Color.Transparent;
            this.btnNextStep.BorderRadius = 5;
            this.btnNextStep.BorderSize = 1;
            this.btnNextStep.ButtonImage = null;
            this.btnNextStep.Corner = BeeGlobal.Corner.Both;
            this.btnNextStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNextStep.FlatAppearance.BorderSize = 0;
            this.btnNextStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextStep.ForeColor = System.Drawing.Color.Black;
            this.btnNextStep.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNextStep.IsCLick = true;
            this.btnNextStep.IsNotChange = true;
            this.btnNextStep.IsRect = false;
            this.btnNextStep.IsUnGroup = false;
            this.btnNextStep.Location = new System.Drawing.Point(3, 3);
            this.btnNextStep.Name = "btnNextStep";
            this.btnNextStep.Size = new System.Drawing.Size(255, 68);
            this.btnNextStep.TabIndex = 57;
            this.btnNextStep.Text = "NextStep";
            this.btnNextStep.TextColor = System.Drawing.Color.Black;
            this.btnNextStep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNextStep.UseVisualStyleBackColor = false;
            this.btnNextStep.Click += new System.EventHandler(this.btnNextStep_Click);
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel4, 0, 9);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel3, 0, 8);
            this.tableLayoutPanel10.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel10.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel2, 0, 6);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel1, 0, 5);
            this.tableLayoutPanel10.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel10.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.AdDelayTrig, 0, 3);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 15;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(380, 794);
            this.tableLayoutPanel10.TabIndex = 53;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.btnOn, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnOFF, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(10, 409);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(360, 48);
            this.tableLayoutPanel4.TabIndex = 74;
            // 
            // btnOn
            // 
            this.btnOn.BackColor = System.Drawing.Color.Transparent;
            this.btnOn.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnOn.BorderColor = System.Drawing.Color.Transparent;
            this.btnOn.BorderRadius = 10;
            this.btnOn.BorderSize = 1;
            this.btnOn.ButtonImage = null;
            this.btnOn.Corner = BeeGlobal.Corner.Right;
            this.btnOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOn.FlatAppearance.BorderSize = 0;
            this.btnOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOn.ForeColor = System.Drawing.Color.Black;
            this.btnOn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOn.IsCLick = false;
            this.btnOn.IsNotChange = false;
            this.btnOn.IsRect = false;
            this.btnOn.IsUnGroup = false;
            this.btnOn.Location = new System.Drawing.Point(180, 0);
            this.btnOn.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(177, 48);
            this.btnOn.TabIndex = 3;
            this.btnOn.Text = "ON";
            this.btnOn.TextColor = System.Drawing.Color.Black;
            this.btnOn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOn.UseVisualStyleBackColor = false;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // btnOFF
            // 
            this.btnOFF.BackColor = System.Drawing.Color.Transparent;
            this.btnOFF.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnOFF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOFF.BorderColor = System.Drawing.Color.Transparent;
            this.btnOFF.BorderRadius = 10;
            this.btnOFF.BorderSize = 1;
            this.btnOFF.ButtonImage = null;
            this.btnOFF.Corner = BeeGlobal.Corner.Left;
            this.btnOFF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOFF.FlatAppearance.BorderSize = 0;
            this.btnOFF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOFF.ForeColor = System.Drawing.Color.Black;
            this.btnOFF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOFF.IsCLick = true;
            this.btnOFF.IsNotChange = false;
            this.btnOFF.IsRect = false;
            this.btnOFF.IsUnGroup = false;
            this.btnOFF.Location = new System.Drawing.Point(3, 0);
            this.btnOFF.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnOFF.Name = "btnOFF";
            this.btnOFF.Size = new System.Drawing.Size(177, 48);
            this.btnOFF.TabIndex = 2;
            this.btnOFF.Text = "OFF";
            this.btnOFF.TextColor = System.Drawing.Color.Black;
            this.btnOFF.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOFF.UseVisualStyleBackColor = false;
            this.btnOFF.Click += new System.EventHandler(this.btnOFF_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.Controls.Add(this.btnLight3, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLight1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLight2, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 364);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(372, 40);
            this.tableLayoutPanel3.TabIndex = 73;
            // 
            // btnLight3
            // 
            this.btnLight3.BackColor = System.Drawing.Color.Transparent;
            this.btnLight3.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnLight3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLight3.BackgroundImage")));
            this.btnLight3.BorderColor = System.Drawing.Color.Silver;
            this.btnLight3.BorderRadius = 10;
            this.btnLight3.BorderSize = 1;
            this.btnLight3.ButtonImage = null;
            this.btnLight3.Corner = BeeGlobal.Corner.Right;
            this.btnLight3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLight3.FlatAppearance.BorderSize = 0;
            this.btnLight3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLight3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLight3.ForeColor = System.Drawing.Color.Black;
            this.btnLight3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLight3.IsCLick = false;
            this.btnLight3.IsNotChange = false;
            this.btnLight3.IsRect = false;
            this.btnLight3.IsUnGroup = true;
            this.btnLight3.Location = new System.Drawing.Point(251, 0);
            this.btnLight3.Margin = new System.Windows.Forms.Padding(0);
            this.btnLight3.Name = "btnLight3";
            this.btnLight3.Size = new System.Drawing.Size(121, 40);
            this.btnLight3.TabIndex = 4;
            this.btnLight3.Text = "Light 3";
            this.btnLight3.TextColor = System.Drawing.Color.Black;
            this.btnLight3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLight3.UseVisualStyleBackColor = false;
            this.btnLight3.Click += new System.EventHandler(this.btnLight3_Click);
            // 
            // btnLight1
            // 
            this.btnLight1.BackColor = System.Drawing.Color.Transparent;
            this.btnLight1.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnLight1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLight1.BorderColor = System.Drawing.Color.Transparent;
            this.btnLight1.BorderRadius = 10;
            this.btnLight1.BorderSize = 1;
            this.btnLight1.ButtonImage = null;
            this.btnLight1.Corner = BeeGlobal.Corner.Left;
            this.btnLight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLight1.FlatAppearance.BorderSize = 0;
            this.btnLight1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLight1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLight1.ForeColor = System.Drawing.Color.Black;
            this.btnLight1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLight1.IsCLick = true;
            this.btnLight1.IsNotChange = false;
            this.btnLight1.IsRect = false;
            this.btnLight1.IsUnGroup = true;
            this.btnLight1.Location = new System.Drawing.Point(0, 0);
            this.btnLight1.Margin = new System.Windows.Forms.Padding(0);
            this.btnLight1.Name = "btnLight1";
            this.btnLight1.Size = new System.Drawing.Size(120, 40);
            this.btnLight1.TabIndex = 2;
            this.btnLight1.Text = "Light 1";
            this.btnLight1.TextColor = System.Drawing.Color.Black;
            this.btnLight1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLight1.UseVisualStyleBackColor = false;
            this.btnLight1.Click += new System.EventHandler(this.btnLight1_Click);
            // 
            // btnLight2
            // 
            this.btnLight2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight2.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLight2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLight2.BackgroundImage")));
            this.btnLight2.BorderColor = System.Drawing.Color.Silver;
            this.btnLight2.BorderRadius = 5;
            this.btnLight2.BorderSize = 1;
            this.btnLight2.ButtonImage = null;
            this.btnLight2.Corner = BeeGlobal.Corner.None;
            this.btnLight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLight2.FlatAppearance.BorderSize = 0;
            this.btnLight2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLight2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLight2.ForeColor = System.Drawing.Color.Black;
            this.btnLight2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLight2.IsCLick = false;
            this.btnLight2.IsNotChange = false;
            this.btnLight2.IsRect = false;
            this.btnLight2.IsUnGroup = true;
            this.btnLight2.Location = new System.Drawing.Point(120, 0);
            this.btnLight2.Margin = new System.Windows.Forms.Padding(0);
            this.btnLight2.Name = "btnLight2";
            this.btnLight2.Size = new System.Drawing.Size(131, 40);
            this.btnLight2.TabIndex = 3;
            this.btnLight2.Text = "Light 2";
            this.btnLight2.TextColor = System.Drawing.Color.Black;
            this.btnLight2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLight2.UseVisualStyleBackColor = false;
            this.btnLight2.Click += new System.EventHandler(this.btnLight2_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(0, 336);
            this.label9.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(377, 25);
            this.label9.TabIndex = 72;
            this.label9.Text = "Light";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(0, 5);
            this.label8.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(377, 25);
            this.label8.TabIndex = 71;
            this.label8.Text = "Methord Trigger";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.26667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.73333F));
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.AdDelayOutput, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 265);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(372, 66);
            this.tableLayoutPanel2.TabIndex = 70;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(8, 5, 3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 58);
            this.label7.TabIndex = 69;
            this.label7.Text = "Delay";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AdDelayOutput
            // 
            this.AdDelayOutput.Location = new System.Drawing.Point(93, 3);
            this.AdDelayOutput.Max = 1000F;
            this.AdDelayOutput.Min = 0F;
            this.AdDelayOutput.Name = "AdDelayOutput";
            this.AdDelayOutput.Size = new System.Drawing.Size(276, 60);
            this.AdDelayOutput.Step = 1F;
            this.AdDelayOutput.TabIndex = 70;
            this.AdDelayOutput.Value = 0F;
            this.AdDelayOutput.ValueChanged += new System.Action<float>(this.AdDelayOutput_ValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnOutsideIn, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnInsideOut, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 227);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(372, 38);
            this.tableLayoutPanel1.TabIndex = 69;
            // 
            // btnOutsideIn
            // 
            this.btnOutsideIn.BackColor = System.Drawing.Color.Transparent;
            this.btnOutsideIn.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnOutsideIn.BorderColor = System.Drawing.Color.Silver;
            this.btnOutsideIn.BorderRadius = 10;
            this.btnOutsideIn.BorderSize = 1;
            this.btnOutsideIn.ButtonImage = null;
            this.btnOutsideIn.Corner = BeeGlobal.Corner.Right;
            this.btnOutsideIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOutsideIn.FlatAppearance.BorderSize = 0;
            this.btnOutsideIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutsideIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOutsideIn.ForeColor = System.Drawing.Color.Black;
            this.btnOutsideIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOutsideIn.IsCLick = false;
            this.btnOutsideIn.IsNotChange = false;
            this.btnOutsideIn.IsRect = false;
            this.btnOutsideIn.IsUnGroup = false;
            this.btnOutsideIn.Location = new System.Drawing.Point(186, 0);
            this.btnOutsideIn.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnOutsideIn.Name = "btnOutsideIn";
            this.btnOutsideIn.Size = new System.Drawing.Size(183, 38);
            this.btnOutsideIn.TabIndex = 3;
            this.btnOutsideIn.Text = "All Time";
            this.btnOutsideIn.TextColor = System.Drawing.Color.Black;
            this.btnOutsideIn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOutsideIn.UseVisualStyleBackColor = false;
            // 
            // btnInsideOut
            // 
            this.btnInsideOut.BackColor = System.Drawing.Color.Transparent;
            this.btnInsideOut.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnInsideOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInsideOut.BorderColor = System.Drawing.Color.Transparent;
            this.btnInsideOut.BorderRadius = 10;
            this.btnInsideOut.BorderSize = 1;
            this.btnInsideOut.ButtonImage = null;
            this.btnInsideOut.Corner = BeeGlobal.Corner.Left;
            this.btnInsideOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInsideOut.FlatAppearance.BorderSize = 0;
            this.btnInsideOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInsideOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsideOut.ForeColor = System.Drawing.Color.Black;
            this.btnInsideOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInsideOut.IsCLick = true;
            this.btnInsideOut.IsNotChange = false;
            this.btnInsideOut.IsRect = false;
            this.btnInsideOut.IsUnGroup = false;
            this.btnInsideOut.Location = new System.Drawing.Point(3, 0);
            this.btnInsideOut.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnInsideOut.Name = "btnInsideOut";
            this.btnInsideOut.Size = new System.Drawing.Size(183, 38);
            this.btnInsideOut.TabIndex = 2;
            this.btnInsideOut.Text = "Blink";
            this.btnInsideOut.TextColor = System.Drawing.Color.Black;
            this.btnInsideOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInsideOut.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 199);
            this.label6.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(377, 25);
            this.label6.TabIndex = 68;
            this.label6.Text = "Methord OutPut";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 96);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 5, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(377, 25);
            this.label1.TabIndex = 66;
            this.label1.Text = "Delay Trigger (ms)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdDelayTrig
            // 
            this.AdDelayTrig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdDelayTrig.Location = new System.Drawing.Point(3, 127);
            this.AdDelayTrig.Max = 2000F;
            this.AdDelayTrig.Min = 1F;
            this.AdDelayTrig.Name = "AdDelayTrig";
            this.AdDelayTrig.Size = new System.Drawing.Size(374, 64);
            this.AdDelayTrig.Step = 1F;
            this.AdDelayTrig.TabIndex = 67;
            this.AdDelayTrig.Value = 1F;
            this.AdDelayTrig.ValueChanged += new System.Action<float>(this.AdDelayTrig_ValueChanged);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnExternal, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnInternal, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 38);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(370, 48);
            this.tableLayoutPanel6.TabIndex = 44;
            // 
            // btnExternal
            // 
            this.btnExternal.BackColor = System.Drawing.Color.Transparent;
            this.btnExternal.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnExternal.BorderColor = System.Drawing.Color.Transparent;
            this.btnExternal.BorderRadius = 10;
            this.btnExternal.BorderSize = 1;
            this.btnExternal.ButtonImage = null;
            this.btnExternal.Corner = BeeGlobal.Corner.Right;
            this.btnExternal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExternal.FlatAppearance.BorderSize = 0;
            this.btnExternal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExternal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExternal.ForeColor = System.Drawing.Color.Black;
            this.btnExternal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExternal.IsCLick = false;
            this.btnExternal.IsNotChange = false;
            this.btnExternal.IsRect = false;
            this.btnExternal.IsUnGroup = false;
            this.btnExternal.Location = new System.Drawing.Point(185, 0);
            this.btnExternal.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnExternal.Name = "btnExternal";
            this.btnExternal.Size = new System.Drawing.Size(182, 48);
            this.btnExternal.TabIndex = 3;
            this.btnExternal.Text = "External";
            this.btnExternal.TextColor = System.Drawing.Color.Black;
            this.btnExternal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExternal.UseVisualStyleBackColor = false;
            this.btnExternal.Click += new System.EventHandler(this.btnExternal_Click);
            // 
            // btnInternal
            // 
            this.btnInternal.BackColor = System.Drawing.Color.Transparent;
            this.btnInternal.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnInternal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInternal.BorderColor = System.Drawing.Color.Transparent;
            this.btnInternal.BorderRadius = 10;
            this.btnInternal.BorderSize = 1;
            this.btnInternal.ButtonImage = null;
            this.btnInternal.Corner = BeeGlobal.Corner.Left;
            this.btnInternal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInternal.FlatAppearance.BorderSize = 0;
            this.btnInternal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInternal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInternal.ForeColor = System.Drawing.Color.Black;
            this.btnInternal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInternal.IsCLick = true;
            this.btnInternal.IsNotChange = false;
            this.btnInternal.IsRect = false;
            this.btnInternal.IsUnGroup = false;
            this.btnInternal.Location = new System.Drawing.Point(3, 0);
            this.btnInternal.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnInternal.Name = "btnInternal";
            this.btnInternal.Size = new System.Drawing.Size(182, 48);
            this.btnInternal.TabIndex = 2;
            this.btnInternal.Text = "Internal";
            this.btnInternal.TextColor = System.Drawing.Color.Black;
            this.btnInternal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInternal.UseVisualStyleBackColor = false;
            this.btnInternal.Click += new System.EventHandler(this.btnInternal_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(394, 838);
            this.tabControl1.TabIndex = 60;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel10);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(386, 800);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Input";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayout1);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(386, 800);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Camera";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // workReadPara
            // 
            this.workReadPara.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workReadPara_DoWork);
            this.workReadPara.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workReadPara_RunWorkerCompleted);
            // 
            // SettingStep1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tableLayoutPanel11);
            this.Name = "SettingStep1";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(400, 918);
            this.Load += new System.EventHandler(this.SettingStep1_Load);
            this.tableLayout1.ResumeLayout(false);
            this.tableLayout1.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker workRead;
        private System.Windows.Forms.TableLayoutPanel tableLayout1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnExternal;
        private RJButton btnInternal;
        private System.Windows.Forms.Timer tmDelaySend;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private RJButton btnCancel;
        private RJButton btnNextStep;
        private AdjustBar trackExposure;
        private AdjustBar trackGain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private AdjustBar trackShift;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private AdjustBar AdDelayTrig;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RJButton btnOutsideIn;
        private RJButton btnInsideOut;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label7;
        private AdjustBar AdDelayOutput;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnLight3;
        private RJButton btnLight1;
        private RJButton btnLight2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private RJButton btnOn;
        private RJButton btnOFF;
        private AdjustBar AdjHeight;
        private System.Windows.Forms.Label label11;
        private AdjustBar AdjWidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private AdjustBar AdjOffSetY;
        private System.Windows.Forms.Label label13;
        private AdjustBar AdjOffsetX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.ComponentModel.BackgroundWorker workReadPara;
        private RJButton btnDownLoadPara;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnCenterY;
        private RJButton btnCenterX;
    }
}
