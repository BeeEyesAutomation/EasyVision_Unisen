﻿
using BeeGlobal;
using BeeInterface;

namespace BeeUi.Tool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingStep1));
            this.workRead = new System.ComponentModel.BackgroundWorker();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayout1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.trackShift = new BeeInterface.TrackBar2();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.trackGain = new BeeInterface.TrackBar2();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.trackExposure = new BeeInterface.TrackBar2();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEnhance = new BeeInterface.RJButton();
            this.btnMirror = new BeeInterface.RJButton();
            this.btnEqualization = new BeeInterface.RJButton();
            this.btnRevese = new BeeInterface.RJButton();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.numTrigger = new BeeInterface.CustomNumeric();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExternal = new BeeInterface.RJButton();
            this.btnInternal = new BeeInterface.RJButton();
            this.pCany = new System.Windows.Forms.Panel();
            this.pArea = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numDelay = new BeeInterface.CustomNumeric();
            this.label5 = new System.Windows.Forms.Label();
            this.tmDelaySend = new System.Windows.Forms.Timer();
            this.rjButton4 = new BeeInterface.RJButton();
            this.rjButton7 = new BeeInterface.RJButton();
            this.rjButton1 = new BeeInterface.RJButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnAuto = new BeeInterface.RJButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnAutoExposure = new BeeInterface.RJButton();
            this.btnCancel = new BeeInterface.RJButton();
            this.btnNextStep = new BeeInterface.RJButton();
            this.btnHD = new BeeInterface.RJButton();
            this.btnFull = new BeeInterface.RJButton();
            this.rjButton8 = new BeeInterface.RJButton();
            this.btn480 = new BeeInterface.RJButton();
            this.rjButton9 = new BeeInterface.RJButton();
            this.rjButton6 = new BeeInterface.RJButton();
            this.btnTopLight = new BeeInterface.RJButton();
            this.btnBoth = new BeeInterface.RJButton();
            this.btnBackLight = new BeeInterface.RJButton();
            this.btnONLight = new BeeInterface.RJButton();
            this.rjButton2 = new BeeInterface.RJButton();
            this.rjButton5 = new BeeInterface.RJButton();
            this.rjButton3 = new BeeInterface.RJButton();
            this.panel9.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayout1.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.pCany.SuspendLayout();
            this.pArea.SuspendLayout();
            this.panel10.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // workRead
            // 
            this.workRead.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workRead_DoWork);
            this.workRead.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workRead_RunWorkerCompleted);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Gainsboro;
            this.panel9.Controls.Add(this.btnHD);
            this.panel9.Controls.Add(this.btnFull);
            this.panel9.Controls.Add(this.rjButton8);
            this.panel9.Controls.Add(this.btn480);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(5, 5);
            this.panel9.Margin = new System.Windows.Forms.Padding(3, 40, 3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new System.Windows.Forms.Padding(3);
            this.panel9.Size = new System.Drawing.Size(437, 65);
            this.panel9.TabIndex = 53;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.numericUpDown1);
            this.panel3.Location = new System.Drawing.Point(655, 221);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(233, 48);
            this.panel3.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(153, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 24);
            this.label3.TabIndex = 46;
            this.label3.Text = "pixel/mm";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Location = new System.Drawing.Point(40, 4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(114, 40);
            this.numericUpDown1.TabIndex = 42;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(455, 694);
            this.tabControl2.TabIndex = 53;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayout1);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(447, 656);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Basic";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayout1
            // 
            this.tableLayout1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayout1.ColumnCount = 1;
            this.tableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout1.Controls.Add(this.tableLayoutPanel12, 0, 3);
            this.tableLayout1.Controls.Add(this.tableLayoutPanel7, 0, 2);
            this.tableLayout1.Controls.Add(this.tableLayoutPanel1, 0, 5);
            this.tableLayout1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayout1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayout1.Controls.Add(this.tableLayoutPanel11, 0, 9);
            this.tableLayout1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayout1.Location = new System.Drawing.Point(3, 3);
            this.tableLayout1.Name = "tableLayout1";
            this.tableLayout1.RowCount = 10;
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayout1.Size = new System.Drawing.Size(442, 650);
            this.tableLayout1.TabIndex = 52;
            this.tableLayout1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel12.ColumnCount = 4;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.Controls.Add(this.rjButton4, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.label8, 3, 0);
            this.tableLayoutPanel12.Controls.Add(this.trackShift, 1, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 173);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(436, 74);
            this.tableLayoutPanel12.TabIndex = 60;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(398, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 74);
            this.label8.TabIndex = 48;
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackShift
            // 
            this.trackShift.ColorTrack = System.Drawing.Color.Gray;
            this.trackShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackShift.Location = new System.Drawing.Point(92, 20);
            this.trackShift.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.trackShift.Max = 100F;
            this.trackShift.Min = 1F;
            this.trackShift.Name = "trackShift";
            this.trackShift.Size = new System.Drawing.Size(300, 51);
            this.trackShift.Step = 1F;
            this.trackShift.TabIndex = 44;
            this.trackShift.Value = 100F;
            this.trackShift.ValueScore = 0F;
            this.trackShift.ValueChanged += new System.Action<float>(this.trackShift_ValueChanged);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.Controls.Add(this.rjButton7, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.trackGain, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 88);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(436, 74);
            this.tableLayoutPanel7.TabIndex = 55;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(398, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 74);
            this.label6.TabIndex = 48;
            this.label6.Text = "us";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackGain
            // 
            this.trackGain.ColorTrack = System.Drawing.Color.Gray;
            this.trackGain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackGain.Location = new System.Drawing.Point(92, 20);
            this.trackGain.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.trackGain.Max = 100F;
            this.trackGain.Min = 1F;
            this.trackGain.Name = "trackGain";
            this.trackGain.Size = new System.Drawing.Size(300, 51);
            this.trackGain.Step = 1F;
            this.trackGain.TabIndex = 44;
            this.trackGain.Value = 100F;
            this.trackGain.ValueScore = 0F;
            this.trackGain.ValueChanged += new System.Action<float>(this.trackGain_ValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.rjButton1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Enabled = false;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 330);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(436, 1);
            this.tableLayoutPanel1.TabIndex = 52;
            this.tableLayoutPanel1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 1);
            this.panel1.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(100, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 1);
            this.label1.TabIndex = 1;
            this.label1.Text = "Auto Check Level  Briness";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnAuto, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 15);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(436, 1);
            this.tableLayoutPanel3.TabIndex = 51;
            this.tableLayoutPanel3.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(265, 1);
            this.panel2.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(100, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 1);
            this.label4.TabIndex = 1;
            this.label4.Text = "Auto Check Level  Briness";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnAutoExposure, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.trackExposure, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(436, 74);
            this.tableLayoutPanel2.TabIndex = 49;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(398, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 74);
            this.label2.TabIndex = 48;
            this.label2.Text = "us";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackExposure
            // 
            this.trackExposure.ColorTrack = System.Drawing.Color.Gray;
            this.trackExposure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackExposure.Location = new System.Drawing.Point(92, 20);
            this.trackExposure.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.trackExposure.Max = 100F;
            this.trackExposure.Min = 1F;
            this.trackExposure.Name = "trackExposure";
            this.trackExposure.Size = new System.Drawing.Size(300, 51);
            this.trackExposure.Step = 1F;
            this.trackExposure.TabIndex = 44;
            this.trackExposure.Value = 100F;
            this.trackExposure.ValueScore = 0F;
            this.trackExposure.ValueChanged += new System.Action<float>(this.trackExposure_ValueChanged);
            this.trackExposure.Load += new System.EventHandler(this.trackExposure_Load);
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.2844F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.7156F));
            this.tableLayoutPanel11.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.btnNextStep, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 573);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(436, 74);
            this.tableLayoutPanel11.TabIndex = 59;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel9);
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage4.Size = new System.Drawing.Size(447, 656);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel8.ColumnCount = 4;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel8.Controls.Add(this.btnEnhance, 3, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnMirror, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnEqualization, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnRevese, 0, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(15, 87);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(294, 74);
            this.tableLayoutPanel8.TabIndex = 56;
            // 
            // btnEnhance
            // 
            this.btnEnhance.BackColor = System.Drawing.Color.Transparent;
            this.btnEnhance.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnEnhance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnhance.BorderColor = System.Drawing.Color.Transparent;
            this.btnEnhance.BorderRadius = 10;
            this.btnEnhance.BorderSize = 1;
            this.btnEnhance.ButtonImage = null;
            this.btnEnhance.Corner = BeeGlobal.Corner.Both;
            this.btnEnhance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnhance.FlatAppearance.BorderSize = 0;
            this.btnEnhance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnhance.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnhance.ForeColor = System.Drawing.Color.Black;
            this.btnEnhance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnhance.IsCLick = false;
            this.btnEnhance.IsNotChange = false;
            this.btnEnhance.IsRect = false;
            this.btnEnhance.IsUnGroup = false;
            this.btnEnhance.Location = new System.Drawing.Point(222, 3);
            this.btnEnhance.Name = "btnEnhance";
            this.btnEnhance.Size = new System.Drawing.Size(69, 68);
            this.btnEnhance.TabIndex = 6;
            this.btnEnhance.Text = "Gray Enhance";
            this.btnEnhance.TextColor = System.Drawing.Color.Black;
            this.btnEnhance.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnhance.UseVisualStyleBackColor = false;
            this.btnEnhance.Click += new System.EventHandler(this.btnEnhance_Click);
            // 
            // btnMirror
            // 
            this.btnMirror.BackColor = System.Drawing.Color.Transparent;
            this.btnMirror.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnMirror.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMirror.BorderColor = System.Drawing.Color.Transparent;
            this.btnMirror.BorderRadius = 10;
            this.btnMirror.BorderSize = 1;
            this.btnMirror.ButtonImage = null;
            this.btnMirror.Corner = BeeGlobal.Corner.Both;
            this.btnMirror.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMirror.FlatAppearance.BorderSize = 0;
            this.btnMirror.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMirror.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMirror.ForeColor = System.Drawing.Color.Black;
            this.btnMirror.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMirror.IsCLick = false;
            this.btnMirror.IsNotChange = false;
            this.btnMirror.IsRect = false;
            this.btnMirror.IsUnGroup = false;
            this.btnMirror.Location = new System.Drawing.Point(149, 3);
            this.btnMirror.Name = "btnMirror";
            this.btnMirror.Size = new System.Drawing.Size(67, 68);
            this.btnMirror.TabIndex = 5;
            this.btnMirror.Text = "Mirror Image";
            this.btnMirror.TextColor = System.Drawing.Color.Black;
            this.btnMirror.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMirror.UseVisualStyleBackColor = false;
            this.btnMirror.Click += new System.EventHandler(this.btnMirror_Click);
            // 
            // btnEqualization
            // 
            this.btnEqualization.BackColor = System.Drawing.Color.Transparent;
            this.btnEqualization.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnEqualization.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEqualization.BorderColor = System.Drawing.Color.Transparent;
            this.btnEqualization.BorderRadius = 10;
            this.btnEqualization.BorderSize = 1;
            this.btnEqualization.ButtonImage = null;
            this.btnEqualization.Corner = BeeGlobal.Corner.Both;
            this.btnEqualization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEqualization.FlatAppearance.BorderSize = 0;
            this.btnEqualization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEqualization.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEqualization.ForeColor = System.Drawing.Color.Black;
            this.btnEqualization.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEqualization.IsCLick = false;
            this.btnEqualization.IsNotChange = false;
            this.btnEqualization.IsRect = false;
            this.btnEqualization.IsUnGroup = false;
            this.btnEqualization.Location = new System.Drawing.Point(76, 3);
            this.btnEqualization.Name = "btnEqualization";
            this.btnEqualization.Size = new System.Drawing.Size(67, 68);
            this.btnEqualization.TabIndex = 4;
            this.btnEqualization.Text = "Equalization";
            this.btnEqualization.TextColor = System.Drawing.Color.Black;
            this.btnEqualization.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEqualization.UseVisualStyleBackColor = false;
            this.btnEqualization.Click += new System.EventHandler(this.btnEqualization_Click);
            // 
            // btnRevese
            // 
            this.btnRevese.BackColor = System.Drawing.Color.Transparent;
            this.btnRevese.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnRevese.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRevese.BorderColor = System.Drawing.Color.Transparent;
            this.btnRevese.BorderRadius = 10;
            this.btnRevese.BorderSize = 1;
            this.btnRevese.ButtonImage = null;
            this.btnRevese.Corner = BeeGlobal.Corner.Both;
            this.btnRevese.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRevese.FlatAppearance.BorderSize = 0;
            this.btnRevese.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevese.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevese.ForeColor = System.Drawing.Color.Black;
            this.btnRevese.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRevese.IsCLick = false;
            this.btnRevese.IsNotChange = false;
            this.btnRevese.IsRect = false;
            this.btnRevese.IsUnGroup = false;
            this.btnRevese.Location = new System.Drawing.Point(3, 3);
            this.btnRevese.Name = "btnRevese";
            this.btnRevese.Size = new System.Drawing.Size(67, 68);
            this.btnRevese.TabIndex = 3;
            this.btnRevese.Text = "Revese Color";
            this.btnRevese.TextColor = System.Drawing.Color.Black;
            this.btnRevese.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRevese.UseVisualStyleBackColor = false;
            this.btnRevese.Click += new System.EventHandler(this.btnRevese_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel10);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(447, 656);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Output";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel5, 0, 3);
            this.tableLayoutPanel10.Controls.Add(this.pCany, 0, 4);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 10;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(441, 650);
            this.tableLayoutPanel10.TabIndex = 53;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel9.Controls.Add(this.rjButton9, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.numTrigger, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(294, 61);
            this.tableLayoutPanel9.TabIndex = 57;
            // 
            // numTrigger
            // 
            this.numTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numTrigger.Location = new System.Drawing.Point(103, 3);
            this.numTrigger.Maxnimum = 1000;
            this.numTrigger.Minimum = 1;
            this.numTrigger.Name = "numTrigger";
            this.numTrigger.Size = new System.Drawing.Size(138, 55);
            this.numTrigger.Step = 10;
            this.numTrigger.TabIndex = 44;
            this.numTrigger.Value = 1;
            this.numTrigger.ValueChanged += new System.EventHandler(this.numTrigger_ValueChanged);
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(247, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 61);
            this.label7.TabIndex = 45;
            this.label7.Text = "ms";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.rjButton6, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 147);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(294, 56);
            this.tableLayoutPanel5.TabIndex = 54;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnExternal, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnInternal, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(105, 5);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(184, 46);
            this.tableLayoutPanel6.TabIndex = 44;
            // 
            // btnExternal
            // 
            this.btnExternal.BackColor = System.Drawing.Color.Transparent;
            this.btnExternal.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnExternal.BorderColor = System.Drawing.Color.Silver;
            this.btnExternal.BorderRadius = 1;
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
            this.btnExternal.Location = new System.Drawing.Point(92, 0);
            this.btnExternal.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnExternal.Name = "btnExternal";
            this.btnExternal.Size = new System.Drawing.Size(89, 46);
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
            this.btnInternal.BorderRadius = 1;
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
            this.btnInternal.Size = new System.Drawing.Size(89, 46);
            this.btnInternal.TabIndex = 2;
            this.btnInternal.Text = "Internal";
            this.btnInternal.TextColor = System.Drawing.Color.Black;
            this.btnInternal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInternal.UseVisualStyleBackColor = false;
            this.btnInternal.Click += new System.EventHandler(this.btnInternal_Click);
            // 
            // pCany
            // 
            this.pCany.BackColor = System.Drawing.Color.Gainsboro;
            this.pCany.Controls.Add(this.btnTopLight);
            this.pCany.Controls.Add(this.btnBoth);
            this.pCany.Controls.Add(this.btnBackLight);
            this.pCany.Controls.Add(this.pArea);
            this.pCany.Controls.Add(this.panel10);
            this.pCany.Location = new System.Drawing.Point(3, 232);
            this.pCany.Name = "pCany";
            this.pCany.Padding = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.pCany.Size = new System.Drawing.Size(294, 104);
            this.pCany.TabIndex = 47;
            // 
            // pArea
            // 
            this.pArea.BackColor = System.Drawing.Color.Transparent;
            this.pArea.Controls.Add(this.btnONLight);
            this.pArea.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pArea.Location = new System.Drawing.Point(85, 52);
            this.pArea.Name = "pArea";
            this.pArea.Padding = new System.Windows.Forms.Padding(2, 5, 2, 2);
            this.pArea.Size = new System.Drawing.Size(207, 47);
            this.pArea.TabIndex = 48;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.rjButton2);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel10.Location = new System.Drawing.Point(2, 5);
            this.panel10.Name = "panel10";
            this.panel10.Padding = new System.Windows.Forms.Padding(2, 2, 5, 2);
            this.panel10.Size = new System.Drawing.Size(83, 94);
            this.panel10.TabIndex = 51;
            this.panel10.Paint += new System.Windows.Forms.PaintEventHandler(this.panel10_Paint);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4.Controls.Add(this.rjButton5, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.numDelay, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 75);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(294, 61);
            this.tableLayoutPanel4.TabIndex = 53;
            this.tableLayoutPanel4.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel4_Paint_1);
            // 
            // numDelay
            // 
            this.numDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numDelay.Location = new System.Drawing.Point(103, 3);
            this.numDelay.Maxnimum = 1000;
            this.numDelay.Minimum = 1;
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(138, 55);
            this.numDelay.Step = 10;
            this.numDelay.TabIndex = 44;
            this.numDelay.Value = 1;
            this.numDelay.ValueChanged += new System.EventHandler(this.numDelay_ValueChanged_1);
            this.numDelay.Load += new System.EventHandler(this.numDelay_Load);
            this.numDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numDelay_KeyDown);
            this.numDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numDelay_KeyPress);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(247, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 61);
            this.label5.TabIndex = 45;
            this.label5.Text = "ms";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmDelaySend
            // 
            this.tmDelaySend.Tick += new System.EventHandler(this.tmDelaySend_Tick);
            // 
            // rjButton4
            // 
            this.rjButton4.BackColor = System.Drawing.Color.Transparent;
            this.rjButton4.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton4.BackgroundImage")));
            this.rjButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton4.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton4.BorderRadius = 1;
            this.rjButton4.BorderSize = 1;
            this.rjButton4.ButtonImage = null;
            this.rjButton4.Corner = BeeGlobal.Corner.Both;
            this.rjButton4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton4.FlatAppearance.BorderSize = 0;
            this.rjButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton4.ForeColor = System.Drawing.Color.Black;
            this.rjButton4.Image = global::BeeUi.Properties.Resources.Aperture;
            this.rjButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton4.IsCLick = false;
            this.rjButton4.IsNotChange = true;
            this.rjButton4.IsRect = true;
            this.rjButton4.IsUnGroup = true;
            this.rjButton4.Location = new System.Drawing.Point(3, 3);
            this.rjButton4.Name = "rjButton4";
            this.rjButton4.Size = new System.Drawing.Size(83, 68);
            this.rjButton4.TabIndex = 43;
            this.rjButton4.Text = "Shift";
            this.rjButton4.TextColor = System.Drawing.Color.Black;
            this.rjButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rjButton4.UseVisualStyleBackColor = false;
            // 
            // rjButton7
            // 
            this.rjButton7.BackColor = System.Drawing.Color.Transparent;
            this.rjButton7.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton7.BackgroundImage")));
            this.rjButton7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton7.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton7.BorderRadius = 1;
            this.rjButton7.BorderSize = 1;
            this.rjButton7.ButtonImage = null;
            this.rjButton7.Corner = BeeGlobal.Corner.Both;
            this.rjButton7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton7.FlatAppearance.BorderSize = 0;
            this.rjButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton7.ForeColor = System.Drawing.Color.Black;
            this.rjButton7.Image = global::BeeUi.Properties.Resources.Aperture;
            this.rjButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton7.IsCLick = false;
            this.rjButton7.IsNotChange = true;
            this.rjButton7.IsRect = true;
            this.rjButton7.IsUnGroup = true;
            this.rjButton7.Location = new System.Drawing.Point(3, 3);
            this.rjButton7.Name = "rjButton7";
            this.rjButton7.Size = new System.Drawing.Size(83, 68);
            this.rjButton7.TabIndex = 43;
            this.rjButton7.Text = "Gain";
            this.rjButton7.TextColor = System.Drawing.Color.Black;
            this.rjButton7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rjButton7.UseVisualStyleBackColor = false;
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.Transparent;
            this.rjButton1.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton1.BackgroundImage")));
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton1.BorderColor = System.Drawing.Color.Silver;
            this.rjButton1.BorderRadius = 1;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.ButtonImage = null;
            this.rjButton1.Corner = BeeGlobal.Corner.Both;
            this.rjButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton1.Enabled = false;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.IsCLick = true;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(283, 11);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(10);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(142, 1);
            this.rjButton1.TabIndex = 35;
            this.rjButton1.Text = "Auto Focus";
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 1);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // btnAuto
            // 
            this.btnAuto.BackColor = System.Drawing.Color.Transparent;
            this.btnAuto.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnAuto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAuto.BackgroundImage")));
            this.btnAuto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAuto.BorderColor = System.Drawing.Color.Silver;
            this.btnAuto.BorderRadius = 1;
            this.btnAuto.BorderSize = 1;
            this.btnAuto.ButtonImage = null;
            this.btnAuto.Corner = BeeGlobal.Corner.Both;
            this.btnAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAuto.Enabled = false;
            this.btnAuto.FlatAppearance.BorderSize = 0;
            this.btnAuto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuto.ForeColor = System.Drawing.Color.Black;
            this.btnAuto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAuto.IsCLick = true;
            this.btnAuto.IsNotChange = false;
            this.btnAuto.IsRect = false;
            this.btnAuto.IsUnGroup = false;
            this.btnAuto.Location = new System.Drawing.Point(283, 11);
            this.btnAuto.Margin = new System.Windows.Forms.Padding(10);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(142, 1);
            this.btnAuto.TabIndex = 35;
            this.btnAuto.Text = "Auto Exposure";
            this.btnAuto.TextColor = System.Drawing.Color.Black;
            this.btnAuto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAuto.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 1);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnAutoExposure
            // 
            this.btnAutoExposure.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoExposure.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnAutoExposure.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAutoExposure.BackgroundImage")));
            this.btnAutoExposure.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAutoExposure.BorderColor = System.Drawing.Color.Transparent;
            this.btnAutoExposure.BorderRadius = 1;
            this.btnAutoExposure.BorderSize = 1;
            this.btnAutoExposure.ButtonImage = null;
            this.btnAutoExposure.Corner = BeeGlobal.Corner.Both;
            this.btnAutoExposure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoExposure.FlatAppearance.BorderSize = 0;
            this.btnAutoExposure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoExposure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoExposure.ForeColor = System.Drawing.Color.Black;
            this.btnAutoExposure.Image = global::BeeUi.Properties.Resources.Aperture;
            this.btnAutoExposure.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAutoExposure.IsCLick = false;
            this.btnAutoExposure.IsNotChange = true;
            this.btnAutoExposure.IsRect = true;
            this.btnAutoExposure.IsUnGroup = true;
            this.btnAutoExposure.Location = new System.Drawing.Point(3, 3);
            this.btnAutoExposure.Name = "btnAutoExposure";
            this.btnAutoExposure.Size = new System.Drawing.Size(83, 68);
            this.btnAutoExposure.TabIndex = 43;
            this.btnAutoExposure.Text = "Exposure";
            this.btnAutoExposure.TextColor = System.Drawing.Color.Black;
            this.btnAutoExposure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAutoExposure.UseVisualStyleBackColor = false;
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
            this.btnCancel.Location = new System.Drawing.Point(291, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(142, 68);
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
            this.btnNextStep.Enabled = false;
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
            this.btnNextStep.Size = new System.Drawing.Size(282, 68);
            this.btnNextStep.TabIndex = 57;
            this.btnNextStep.Text = "NextStep";
            this.btnNextStep.TextColor = System.Drawing.Color.Black;
            this.btnNextStep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNextStep.UseVisualStyleBackColor = false;
            this.btnNextStep.Click += new System.EventHandler(this.btnNextStep_Click);
            // 
            // btnHD
            // 
            this.btnHD.BackColor = System.Drawing.SystemColors.Control;
            this.btnHD.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnHD.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHD.BackgroundImage")));
            this.btnHD.BorderColor = System.Drawing.Color.Silver;
            this.btnHD.BorderRadius = 5;
            this.btnHD.BorderSize = 1;
            this.btnHD.ButtonImage = null;
            this.btnHD.Corner = BeeGlobal.Corner.Both;
            this.btnHD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHD.FlatAppearance.BorderSize = 0;
            this.btnHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHD.ForeColor = System.Drawing.Color.Black;
            this.btnHD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHD.IsCLick = false;
            this.btnHD.IsNotChange = false;
            this.btnHD.IsRect = false;
            this.btnHD.IsUnGroup = false;
            this.btnHD.Location = new System.Drawing.Point(216, 3);
            this.btnHD.Name = "btnHD";
            this.btnHD.Size = new System.Drawing.Size(138, 59);
            this.btnHD.TabIndex = 8;
            this.btnHD.Text = "HD";
            this.btnHD.TextColor = System.Drawing.Color.Black;
            this.btnHD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHD.UseVisualStyleBackColor = false;
            this.btnHD.Click += new System.EventHandler(this.btnHD_Click);
            // 
            // btnFull
            // 
            this.btnFull.BackColor = System.Drawing.Color.Transparent;
            this.btnFull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnFull.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFull.BackgroundImage")));
            this.btnFull.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFull.BorderColor = System.Drawing.Color.Transparent;
            this.btnFull.BorderRadius = 5;
            this.btnFull.BorderSize = 1;
            this.btnFull.ButtonImage = null;
            this.btnFull.Corner = BeeGlobal.Corner.Both;
            this.btnFull.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnFull.FlatAppearance.BorderSize = 0;
            this.btnFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFull.ForeColor = System.Drawing.Color.Black;
            this.btnFull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFull.IsCLick = true;
            this.btnFull.IsNotChange = false;
            this.btnFull.IsRect = false;
            this.btnFull.IsUnGroup = false;
            this.btnFull.Location = new System.Drawing.Point(136, 3);
            this.btnFull.Name = "btnFull";
            this.btnFull.Size = new System.Drawing.Size(80, 59);
            this.btnFull.TabIndex = 7;
            this.btnFull.Text = "FullHD";
            this.btnFull.TextColor = System.Drawing.Color.Black;
            this.btnFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFull.UseVisualStyleBackColor = false;
            this.btnFull.Click += new System.EventHandler(this.btnFull_Click);
            // 
            // rjButton8
            // 
            this.rjButton8.BackColor = System.Drawing.Color.Transparent;
            this.rjButton8.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton8.BackgroundImage")));
            this.rjButton8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton8.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton8.BorderRadius = 5;
            this.rjButton8.BorderSize = 1;
            this.rjButton8.ButtonImage = null;
            this.rjButton8.Corner = BeeGlobal.Corner.Both;
            this.rjButton8.Dock = System.Windows.Forms.DockStyle.Left;
            this.rjButton8.FlatAppearance.BorderSize = 0;
            this.rjButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton8.ForeColor = System.Drawing.Color.Black;
            this.rjButton8.Image = global::BeeUi.Properties.Resources.Aperture;
            this.rjButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton8.IsCLick = false;
            this.rjButton8.IsNotChange = true;
            this.rjButton8.IsRect = true;
            this.rjButton8.IsUnGroup = true;
            this.rjButton8.Location = new System.Drawing.Point(3, 3);
            this.rjButton8.Name = "rjButton8";
            this.rjButton8.Size = new System.Drawing.Size(133, 59);
            this.rjButton8.TabIndex = 44;
            this.rjButton8.Text = "ReSolution";
            this.rjButton8.TextColor = System.Drawing.Color.Black;
            this.rjButton8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rjButton8.UseVisualStyleBackColor = false;
            // 
            // btn480
            // 
            this.btn480.BackColor = System.Drawing.SystemColors.Control;
            this.btn480.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btn480.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn480.BackgroundImage")));
            this.btn480.BorderColor = System.Drawing.Color.Silver;
            this.btn480.BorderRadius = 5;
            this.btn480.BorderSize = 1;
            this.btn480.ButtonImage = null;
            this.btn480.Corner = BeeGlobal.Corner.Both;
            this.btn480.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn480.FlatAppearance.BorderSize = 0;
            this.btn480.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn480.ForeColor = System.Drawing.Color.Black;
            this.btn480.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn480.IsCLick = false;
            this.btn480.IsNotChange = false;
            this.btn480.IsRect = false;
            this.btn480.IsUnGroup = false;
            this.btn480.Location = new System.Drawing.Point(354, 3);
            this.btn480.Name = "btn480";
            this.btn480.Size = new System.Drawing.Size(80, 59);
            this.btn480.TabIndex = 9;
            this.btn480.Text = "480p";
            this.btn480.TextColor = System.Drawing.Color.Black;
            this.btn480.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn480.UseVisualStyleBackColor = false;
            this.btn480.Click += new System.EventHandler(this.btn480_Click);
            // 
            // rjButton9
            // 
            this.rjButton9.BackColor = System.Drawing.Color.Transparent;
            this.rjButton9.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton9.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton9.BorderRadius = 1;
            this.rjButton9.BorderSize = 1;
            this.rjButton9.ButtonImage = null;
            this.rjButton9.Corner = BeeGlobal.Corner.Both;
            this.rjButton9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton9.FlatAppearance.BorderSize = 0;
            this.rjButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton9.ForeColor = System.Drawing.Color.Black;
            this.rjButton9.Image = global::BeeUi.Properties.Resources.BID_ICON_WIDTH_TOOL_LARGE_E_32BIT;
            this.rjButton9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton9.IsCLick = false;
            this.rjButton9.IsNotChange = true;
            this.rjButton9.IsRect = true;
            this.rjButton9.IsUnGroup = true;
            this.rjButton9.Location = new System.Drawing.Point(3, 3);
            this.rjButton9.Name = "rjButton9";
            this.rjButton9.Size = new System.Drawing.Size(94, 55);
            this.rjButton9.TabIndex = 43;
            this.rjButton9.Text = "Delay Trigger";
            this.rjButton9.TextColor = System.Drawing.Color.Black;
            this.rjButton9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rjButton9.UseVisualStyleBackColor = false;
            // 
            // rjButton6
            // 
            this.rjButton6.BackColor = System.Drawing.Color.Transparent;
            this.rjButton6.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton6.BackgroundImage")));
            this.rjButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton6.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton6.BorderRadius = 1;
            this.rjButton6.BorderSize = 1;
            this.rjButton6.ButtonImage = null;
            this.rjButton6.Corner = BeeGlobal.Corner.Both;
            this.rjButton6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton6.FlatAppearance.BorderSize = 0;
            this.rjButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton6.ForeColor = System.Drawing.Color.Black;
            this.rjButton6.Image = ((System.Drawing.Image)(resources.GetObject("rjButton6.Image")));
            this.rjButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton6.IsCLick = false;
            this.rjButton6.IsNotChange = true;
            this.rjButton6.IsRect = true;
            this.rjButton6.IsUnGroup = true;
            this.rjButton6.Location = new System.Drawing.Point(3, 3);
            this.rjButton6.Name = "rjButton6";
            this.rjButton6.Size = new System.Drawing.Size(94, 50);
            this.rjButton6.TabIndex = 43;
            this.rjButton6.Text = "Trigger Input";
            this.rjButton6.TextColor = System.Drawing.Color.Black;
            this.rjButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rjButton6.UseVisualStyleBackColor = false;
            // 
            // btnTopLight
            // 
            this.btnTopLight.BackColor = System.Drawing.Color.Transparent;
            this.btnTopLight.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnTopLight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTopLight.BackgroundImage")));
            this.btnTopLight.BorderColor = System.Drawing.Color.Transparent;
            this.btnTopLight.BorderRadius = 0;
            this.btnTopLight.BorderSize = 1;
            this.btnTopLight.ButtonImage = null;
            this.btnTopLight.Corner = BeeGlobal.Corner.Both;
            this.btnTopLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTopLight.Enabled = false;
            this.btnTopLight.FlatAppearance.BorderSize = 0;
            this.btnTopLight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTopLight.ForeColor = System.Drawing.Color.Black;
            this.btnTopLight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTopLight.IsCLick = false;
            this.btnTopLight.IsNotChange = false;
            this.btnTopLight.IsRect = false;
            this.btnTopLight.IsUnGroup = false;
            this.btnTopLight.Location = new System.Drawing.Point(188, 5);
            this.btnTopLight.Name = "btnTopLight";
            this.btnTopLight.Size = new System.Drawing.Size(0, 47);
            this.btnTopLight.TabIndex = 8;
            this.btnTopLight.Text = "Red Light";
            this.btnTopLight.TextColor = System.Drawing.Color.Black;
            this.btnTopLight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTopLight.UseVisualStyleBackColor = false;
            this.btnTopLight.Click += new System.EventHandler(this.btnTopLight_Click);
            // 
            // btnBoth
            // 
            this.btnBoth.BackColor = System.Drawing.Color.Transparent;
            this.btnBoth.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnBoth.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBoth.BackgroundImage")));
            this.btnBoth.BorderColor = System.Drawing.Color.Transparent;
            this.btnBoth.BorderRadius = 0;
            this.btnBoth.BorderSize = 1;
            this.btnBoth.ButtonImage = null;
            this.btnBoth.Corner = BeeGlobal.Corner.Both;
            this.btnBoth.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBoth.Enabled = false;
            this.btnBoth.FlatAppearance.BorderSize = 0;
            this.btnBoth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBoth.ForeColor = System.Drawing.Color.Black;
            this.btnBoth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBoth.IsCLick = false;
            this.btnBoth.IsNotChange = false;
            this.btnBoth.IsRect = false;
            this.btnBoth.IsUnGroup = false;
            this.btnBoth.Location = new System.Drawing.Point(176, 5);
            this.btnBoth.Name = "btnBoth";
            this.btnBoth.Size = new System.Drawing.Size(116, 47);
            this.btnBoth.TabIndex = 9;
            this.btnBoth.Text = "Both";
            this.btnBoth.TextColor = System.Drawing.Color.Black;
            this.btnBoth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBoth.UseVisualStyleBackColor = false;
            this.btnBoth.Click += new System.EventHandler(this.btnBoth_Click);
            // 
            // btnBackLight
            // 
            this.btnBackLight.BackColor = System.Drawing.Color.Transparent;
            this.btnBackLight.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnBackLight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBackLight.BackgroundImage")));
            this.btnBackLight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBackLight.BorderColor = System.Drawing.Color.Transparent;
            this.btnBackLight.BorderRadius = 0;
            this.btnBackLight.BorderSize = 1;
            this.btnBackLight.ButtonImage = null;
            this.btnBackLight.Corner = BeeGlobal.Corner.Both;
            this.btnBackLight.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBackLight.FlatAppearance.BorderSize = 0;
            this.btnBackLight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackLight.ForeColor = System.Drawing.Color.Black;
            this.btnBackLight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBackLight.IsCLick = true;
            this.btnBackLight.IsNotChange = false;
            this.btnBackLight.IsRect = false;
            this.btnBackLight.IsUnGroup = false;
            this.btnBackLight.Location = new System.Drawing.Point(85, 5);
            this.btnBackLight.Name = "btnBackLight";
            this.btnBackLight.Size = new System.Drawing.Size(103, 47);
            this.btnBackLight.TabIndex = 7;
            this.btnBackLight.Text = "White Light";
            this.btnBackLight.TextColor = System.Drawing.Color.Black;
            this.btnBackLight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBackLight.UseVisualStyleBackColor = false;
            this.btnBackLight.Click += new System.EventHandler(this.btnBackLight_Click);
            // 
            // btnONLight
            // 
            this.btnONLight.BackColor = System.Drawing.Color.Transparent;
            this.btnONLight.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnONLight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnONLight.BackgroundImage")));
            this.btnONLight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnONLight.BorderColor = System.Drawing.Color.Transparent;
            this.btnONLight.BorderRadius = 5;
            this.btnONLight.BorderSize = 1;
            this.btnONLight.ButtonImage = null;
            this.btnONLight.Corner = BeeGlobal.Corner.Both;
            this.btnONLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnONLight.FlatAppearance.BorderSize = 0;
            this.btnONLight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnONLight.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnONLight.ForeColor = System.Drawing.Color.Black;
            this.btnONLight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnONLight.IsCLick = false;
            this.btnONLight.IsNotChange = false;
            this.btnONLight.IsRect = false;
            this.btnONLight.IsUnGroup = true;
            this.btnONLight.Location = new System.Drawing.Point(2, 5);
            this.btnONLight.Name = "btnONLight";
            this.btnONLight.Size = new System.Drawing.Size(203, 40);
            this.btnONLight.TabIndex = 2;
            this.btnONLight.Text = "ON Light";
            this.btnONLight.TextColor = System.Drawing.Color.Black;
            this.btnONLight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnONLight.UseVisualStyleBackColor = false;
            this.btnONLight.Click += new System.EventHandler(this.btnONLight_Click);
            // 
            // rjButton2
            // 
            this.rjButton2.BackColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton2.BackgroundImage")));
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 0;
            this.rjButton2.BorderSize = 3;
            this.rjButton2.ButtonImage = null;
            this.rjButton2.Corner = BeeGlobal.Corner.Both;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.Image = global::BeeUi.Properties.Resources.Brightness_1;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = true;
            this.rjButton2.IsRect = true;
            this.rjButton2.IsUnGroup = true;
            this.rjButton2.Location = new System.Drawing.Point(2, 2);
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(76, 90);
            this.rjButton2.TabIndex = 50;
            this.rjButton2.Text = "Light";
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // rjButton5
            // 
            this.rjButton5.BackColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton5.BackgroundImage")));
            this.rjButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton5.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton5.BorderRadius = 1;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.ButtonImage = null;
            this.rjButton5.Corner = BeeGlobal.Corner.Both;
            this.rjButton5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.Image = global::BeeUi.Properties.Resources.BID_ICON_WIDTH_TOOL_LARGE_E_32BIT;
            this.rjButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton5.IsCLick = false;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = true;
            this.rjButton5.IsUnGroup = true;
            this.rjButton5.Location = new System.Drawing.Point(3, 3);
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(94, 55);
            this.rjButton5.TabIndex = 43;
            this.rjButton5.Text = "Delay Output";
            this.rjButton5.TextColor = System.Drawing.Color.Black;
            this.rjButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rjButton5.UseVisualStyleBackColor = false;
            // 
            // rjButton3
            // 
            this.rjButton3.BackColor = System.Drawing.Color.Transparent;
            this.rjButton3.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton3.BackgroundImage")));
            this.rjButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton3.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton3.BorderRadius = 5;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.ButtonImage = null;
            this.rjButton3.Corner = BeeGlobal.Corner.Both;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton3.ForeColor = System.Drawing.Color.Black;
            this.rjButton3.Image = global::BeeUi.Properties.Resources.Color_Dropper_2;
            this.rjButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton3.IsCLick = false;
            this.rjButton3.IsNotChange = true;
            this.rjButton3.IsRect = true;
            this.rjButton3.IsUnGroup = true;
            this.rjButton3.Location = new System.Drawing.Point(558, 221);
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(82, 50);
            this.rjButton3.TabIndex = 51;
            this.rjButton3.Text = "Scale pixel";
            this.rjButton3.TextColor = System.Drawing.Color.Black;
            this.rjButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton3.UseVisualStyleBackColor = false;
            // 
            // SettingStep1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.rjButton3);
            this.Name = "SettingStep1";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(458, 714);
            this.Load += new System.EventHandler(this.SettingStep1_Load);
            this.VisibleChanged += new System.EventHandler(this.SettingStep1_VisibleChanged);
            this.panel9.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayout1.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.pCany.ResumeLayout(false);
            this.pArea.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker workRead;
        private RJButton rjButton3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Panel panel9;
        private RJButton btnFull;
        private RJButton btnHD;
        private RJButton btn480;
        private RJButton rjButton8;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayout1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public RJButton btnAuto;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnAutoExposure;
        private System.Windows.Forms.Label label2;
        public TrackBar2 trackExposure;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public RJButton rjButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private RJButton rjButton5;
        public  CustomNumeric numDelay;
        private System.Windows.Forms.Panel pCany;
        private RJButton btnTopLight;
        private RJButton btnBoth;
        private RJButton btnBackLight;
        private System.Windows.Forms.Panel pArea;
        private RJButton btnONLight;
        private System.Windows.Forms.Panel panel10;
        private RJButton rjButton2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton rjButton6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnExternal;
        private RJButton btnInternal;
        private System.Windows.Forms.Timer tmDelaySend;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private RJButton rjButton7;
        private System.Windows.Forms.Label label6;
        public TrackBar2 trackGain;
        private RJButton btnEnhance;
        private RJButton btnMirror;
        private RJButton btnEqualization;
        private RJButton btnRevese;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private RJButton rjButton9;
        public  CustomNumeric numTrigger;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private RJButton btnCancel;
        private RJButton btnNextStep;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private RJButton rjButton4;
        private System.Windows.Forms.Label label8;
        public TrackBar2 trackShift;
    }
}
