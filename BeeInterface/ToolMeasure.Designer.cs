using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolMeasure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMeasure));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton10 = new BeeInterface.RJButton();
            this.cbDirect = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton3 = new BeeInterface.RJButton();
            this.cbMethord = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton2 = new BeeInterface.RJButton();
            this.cbMeasure = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton1 = new BeeInterface.RJButton();
            this.cb8 = new System.Windows.Forms.ComboBox();
            this.cb7 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton13 = new BeeInterface.RJButton();
            this.cb6 = new System.Windows.Forms.ComboBox();
            this.cb5 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton12 = new BeeInterface.RJButton();
            this.cb4 = new System.Windows.Forms.ComboBox();
            this.cb3 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton11 = new BeeInterface.RJButton();
            this.cb2 = new System.Windows.Forms.ComboBox();
            this.cb1 = new System.Windows.Forms.ComboBox();
            this.layScore = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton4 = new BeeInterface.RJButton();
            this.numScore = new BeeInterface.CustomNumeric();
            this.rjButton5 = new BeeInterface.RJButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.trackScore = new BeeInterface.TrackBar2();
            this.label8 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.ckBitwiseNot = new BeeInterface.RJButton();
            this.ckSubPixel = new BeeInterface.RJButton();
            this.ckSIMD = new BeeInterface.RJButton();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHighSpeed = new BeeInterface.RJButton();
            this.btnNormal = new BeeInterface.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton8 = new BeeInterface.RJButton();
            this.numOverLap = new BeeInterface.CustomNumeric();
            this.rjButton9 = new BeeInterface.RJButton();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.trackMaxOverLap = new BeeInterface.TrackBar2();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.numScale = new System.Windows.Forms.NumericUpDown();
            this.rjButton6 = new BeeInterface.RJButton();
            this.rjButton7 = new BeeInterface.RJButton();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.trackAngle = new BeeInterface.TrackBar2();
            this.label4 = new System.Windows.Forms.Label();
            this.pCany = new System.Windows.Forms.Panel();
            this.btnAreaBlack = new BeeInterface.RJButton();
            this.btnAreaWhite = new BeeInterface.RJButton();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.layScore.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            this.tableLayoutPanel10.SuspendLayout();
            this.pCany.SuspendLayout();
            this.SuspendLayout();
            // 
            // threadProcess
            // 
            this.threadProcess.WorkerReportsProgress = true;
            this.threadProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.threadProcess_DoWork);
            this.threadProcess.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.threadProcess_RunWorkerCompleted);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabP1);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(462, 881);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(454, 843);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            this.tabP1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel16, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel15, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.layScore, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 837);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 2;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.71429F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.28572F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Controls.Add(this.rjButton10, 0, 0);
            this.tableLayoutPanel16.Controls.Add(this.cbDirect, 1, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(5, 55);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 1;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(440, 45);
            this.tableLayoutPanel16.TabIndex = 54;
            // 
            // rjButton10
            // 
            this.rjButton10.BackColor = System.Drawing.Color.LightGray;
            this.rjButton10.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton10.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton10.BorderRadius = 10;
            this.rjButton10.BorderSize = 1;
            this.rjButton10.ButtonImage = null;
            this.rjButton10.Corner = BeeGlobal.Corner.Left;
            this.rjButton10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton10.FlatAppearance.BorderSize = 0;
            this.rjButton10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton10.ForeColor = System.Drawing.Color.Black;
            this.rjButton10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton10.IsCLick = false;
            this.rjButton10.IsNotChange = false;
            this.rjButton10.IsRect = false;
            this.rjButton10.IsUnGroup = false;
            this.rjButton10.Location = new System.Drawing.Point(0, 0);
            this.rjButton10.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton10.Name = "rjButton10";
            this.rjButton10.Size = new System.Drawing.Size(201, 45);
            this.rjButton10.TabIndex = 33;
            this.rjButton10.Text = "Direct Measure";
            this.rjButton10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rjButton10.TextColor = System.Drawing.Color.Black;
            this.rjButton10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton10.UseVisualStyleBackColor = false;
            // 
            // cbDirect
            // 
            this.cbDirect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDirect.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDirect.FormattingEnabled = true;
            this.cbDirect.Location = new System.Drawing.Point(203, 2);
            this.cbDirect.Margin = new System.Windows.Forms.Padding(2);
            this.cbDirect.Name = "cbDirect";
            this.cbDirect.Size = new System.Drawing.Size(235, 39);
            this.cbDirect.TabIndex = 2;
            this.cbDirect.SelectedIndexChanged += new System.EventHandler(this.cbDirect_SelectedIndexChanged);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.71429F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.28572F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel15.Controls.Add(this.rjButton3, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.cbMethord, 1, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(5, 105);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(440, 45);
            this.tableLayoutPanel15.TabIndex = 53;
            // 
            // rjButton3
            // 
            this.rjButton3.BackColor = System.Drawing.Color.LightGray;
            this.rjButton3.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton3.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton3.BorderRadius = 10;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.ButtonImage = null;
            this.rjButton3.Corner = BeeGlobal.Corner.Left;
            this.rjButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.ForeColor = System.Drawing.Color.Black;
            this.rjButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton3.IsCLick = false;
            this.rjButton3.IsNotChange = false;
            this.rjButton3.IsRect = false;
            this.rjButton3.IsUnGroup = false;
            this.rjButton3.Location = new System.Drawing.Point(0, 0);
            this.rjButton3.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(201, 45);
            this.rjButton3.TabIndex = 33;
            this.rjButton3.Text = "Methord Measure";
            this.rjButton3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rjButton3.TextColor = System.Drawing.Color.Black;
            this.rjButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton3.UseVisualStyleBackColor = false;
            // 
            // cbMethord
            // 
            this.cbMethord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMethord.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMethord.FormattingEnabled = true;
            this.cbMethord.Location = new System.Drawing.Point(203, 2);
            this.cbMethord.Margin = new System.Windows.Forms.Padding(2);
            this.cbMethord.Name = "cbMethord";
            this.cbMethord.Size = new System.Drawing.Size(235, 39);
            this.cbMethord.TabIndex = 2;
            this.cbMethord.SelectedIndexChanged += new System.EventHandler(this.cbMethord_SelectedIndexChanged);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.71429F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.28572F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.rjButton2, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.cbMeasure, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(440, 45);
            this.tableLayoutPanel6.TabIndex = 52;
            // 
            // rjButton2
            // 
            this.rjButton2.BackColor = System.Drawing.Color.LightGray;
            this.rjButton2.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 10;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ButtonImage = null;
            this.rjButton2.Corner = BeeGlobal.Corner.Left;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = false;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(0, 0);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(201, 45);
            this.rjButton2.TabIndex = 33;
            this.rjButton2.Text = "Type Measure";
            this.rjButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // cbMeasure
            // 
            this.cbMeasure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMeasure.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMeasure.FormattingEnabled = true;
            this.cbMeasure.Location = new System.Drawing.Point(203, 2);
            this.cbMeasure.Margin = new System.Windows.Forms.Padding(2);
            this.cbMeasure.Name = "cbMeasure";
            this.cbMeasure.Size = new System.Drawing.Size(235, 39);
            this.cbMeasure.TabIndex = 2;
            this.cbMeasure.SelectedIndexChanged += new System.EventHandler(this.cbMeasure_SelectedIndexChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.09091F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.13636F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.54546F));
            this.tableLayoutPanel5.Controls.Add(this.rjButton1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cb8, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.cb7, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 396);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(440, 51);
            this.tableLayoutPanel5.TabIndex = 51;
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.LightGray;
            this.rjButton1.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton1.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton1.BorderRadius = 10;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.ButtonImage = null;
            this.rjButton1.Corner = BeeGlobal.Corner.Left;
            this.rjButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(0, 0);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(128, 51);
            this.rjButton1.TabIndex = 33;
            this.rjButton1.Text = "Point 2";
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // cb8
            // 
            this.cb8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb8.FormattingEnabled = true;
            this.cb8.Location = new System.Drawing.Point(290, 8);
            this.cb8.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb8.Name = "cb8";
            this.cb8.Size = new System.Drawing.Size(147, 39);
            this.cb8.TabIndex = 2;
            this.cb8.SelectedIndexChanged += new System.EventHandler(this.cb8_SelectedIndexChanged);
            // 
            // cb7
            // 
            this.cb7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb7.FormattingEnabled = true;
            this.cb7.Location = new System.Drawing.Point(131, 8);
            this.cb7.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb7.Name = "cb7";
            this.cb7.Size = new System.Drawing.Size(153, 39);
            this.cb7.TabIndex = 1;
            this.cb7.SelectedIndexChanged += new System.EventHandler(this.cb7_SelectedIndexChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.54545F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.68182F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.54546F));
            this.tableLayoutPanel4.Controls.Add(this.rjButton13, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.cb6, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.cb5, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 344);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(440, 47);
            this.tableLayoutPanel4.TabIndex = 50;
            // 
            // rjButton13
            // 
            this.rjButton13.BackColor = System.Drawing.Color.LightGray;
            this.rjButton13.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton13.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton13.BorderRadius = 10;
            this.rjButton13.BorderSize = 1;
            this.rjButton13.ButtonImage = null;
            this.rjButton13.Corner = BeeGlobal.Corner.Left;
            this.rjButton13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton13.FlatAppearance.BorderSize = 0;
            this.rjButton13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton13.ForeColor = System.Drawing.Color.Black;
            this.rjButton13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton13.IsCLick = true;
            this.rjButton13.IsNotChange = false;
            this.rjButton13.IsRect = false;
            this.rjButton13.IsUnGroup = false;
            this.rjButton13.Location = new System.Drawing.Point(0, 0);
            this.rjButton13.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton13.Name = "rjButton13";
            this.rjButton13.Size = new System.Drawing.Size(130, 47);
            this.rjButton13.TabIndex = 32;
            this.rjButton13.Text = "Point 1";
            this.rjButton13.TextColor = System.Drawing.Color.Black;
            this.rjButton13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton13.UseVisualStyleBackColor = false;
            // 
            // cb6
            // 
            this.cb6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb6.FormattingEnabled = true;
            this.cb6.Location = new System.Drawing.Point(290, 8);
            this.cb6.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb6.Name = "cb6";
            this.cb6.Size = new System.Drawing.Size(147, 39);
            this.cb6.TabIndex = 2;
            this.cb6.SelectedIndexChanged += new System.EventHandler(this.cb6_SelectedIndexChanged);
            // 
            // cb5
            // 
            this.cb5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb5.FormattingEnabled = true;
            this.cb5.Location = new System.Drawing.Point(133, 8);
            this.cb5.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb5.Name = "cb5";
            this.cb5.Size = new System.Drawing.Size(151, 39);
            this.cb5.TabIndex = 1;
            this.cb5.SelectedIndexChanged += new System.EventHandler(this.cb5_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 320);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 24);
            this.label5.TabIndex = 49;
            this.label5.Text = "Line 2";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.09091F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.13636F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.54546F));
            this.tableLayoutPanel3.Controls.Add(this.rjButton12, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cb4, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.cb3, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 247);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(440, 53);
            this.tableLayoutPanel3.TabIndex = 48;
            // 
            // rjButton12
            // 
            this.rjButton12.BackColor = System.Drawing.Color.LightGray;
            this.rjButton12.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton12.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton12.BorderRadius = 10;
            this.rjButton12.BorderSize = 1;
            this.rjButton12.ButtonImage = null;
            this.rjButton12.Corner = BeeGlobal.Corner.Left;
            this.rjButton12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton12.FlatAppearance.BorderSize = 0;
            this.rjButton12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton12.ForeColor = System.Drawing.Color.Black;
            this.rjButton12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton12.IsCLick = false;
            this.rjButton12.IsNotChange = false;
            this.rjButton12.IsRect = false;
            this.rjButton12.IsUnGroup = false;
            this.rjButton12.Location = new System.Drawing.Point(0, 0);
            this.rjButton12.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton12.Name = "rjButton12";
            this.rjButton12.Size = new System.Drawing.Size(128, 53);
            this.rjButton12.TabIndex = 33;
            this.rjButton12.Text = "Point 2";
            this.rjButton12.TextColor = System.Drawing.Color.Black;
            this.rjButton12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton12.UseVisualStyleBackColor = false;
            // 
            // cb4
            // 
            this.cb4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb4.FormattingEnabled = true;
            this.cb4.Location = new System.Drawing.Point(290, 8);
            this.cb4.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb4.Name = "cb4";
            this.cb4.Size = new System.Drawing.Size(147, 39);
            this.cb4.TabIndex = 2;
            this.cb4.SelectedIndexChanged += new System.EventHandler(this.cb4_SelectedIndexChanged);
            // 
            // cb3
            // 
            this.cb3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb3.FormattingEnabled = true;
            this.cb3.Location = new System.Drawing.Point(131, 8);
            this.cb3.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb3.Name = "cb3";
            this.cb3.Size = new System.Drawing.Size(153, 39);
            this.cb3.TabIndex = 1;
            this.cb3.SelectedIndexChanged += new System.EventHandler(this.cb3_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.54545F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.68182F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.54546F));
            this.tableLayoutPanel2.Controls.Add(this.rjButton11, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cb2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.cb1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 195);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(440, 47);
            this.tableLayoutPanel2.TabIndex = 47;
            // 
            // rjButton11
            // 
            this.rjButton11.BackColor = System.Drawing.Color.LightGray;
            this.rjButton11.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton11.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton11.BorderRadius = 10;
            this.rjButton11.BorderSize = 1;
            this.rjButton11.ButtonImage = null;
            this.rjButton11.Corner = BeeGlobal.Corner.Left;
            this.rjButton11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton11.FlatAppearance.BorderSize = 0;
            this.rjButton11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton11.ForeColor = System.Drawing.Color.Black;
            this.rjButton11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton11.IsCLick = true;
            this.rjButton11.IsNotChange = false;
            this.rjButton11.IsRect = false;
            this.rjButton11.IsUnGroup = false;
            this.rjButton11.Location = new System.Drawing.Point(0, 0);
            this.rjButton11.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton11.Name = "rjButton11";
            this.rjButton11.Size = new System.Drawing.Size(130, 47);
            this.rjButton11.TabIndex = 32;
            this.rjButton11.Text = "Point 1";
            this.rjButton11.TextColor = System.Drawing.Color.Black;
            this.rjButton11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton11.UseVisualStyleBackColor = false;
            // 
            // cb2
            // 
            this.cb2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb2.FormattingEnabled = true;
            this.cb2.Location = new System.Drawing.Point(290, 8);
            this.cb2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb2.Name = "cb2";
            this.cb2.Size = new System.Drawing.Size(147, 39);
            this.cb2.TabIndex = 2;
            this.cb2.SelectedIndexChanged += new System.EventHandler(this.cb2_SelectedIndexChanged);
            // 
            // cb1
            // 
            this.cb1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb1.FormattingEnabled = true;
            this.cb1.Location = new System.Drawing.Point(133, 8);
            this.cb1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb1.Name = "cb1";
            this.cb1.Size = new System.Drawing.Size(151, 39);
            this.cb1.TabIndex = 1;
            this.cb1.SelectedIndexChanged += new System.EventHandler(this.cb1_SelectedIndexChanged);
            // 
            // layScore
            // 
            this.layScore.BackColor = System.Drawing.Color.Transparent;
            this.layScore.ColumnCount = 4;
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.layScore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.layScore.Controls.Add(this.rjButton4, 3, 0);
            this.layScore.Controls.Add(this.numScore, 2, 0);
            this.layScore.Controls.Add(this.rjButton5, 0, 0);
            this.layScore.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.layScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layScore.Location = new System.Drawing.Point(5, 492);
            this.layScore.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layScore.Name = "layScore";
            this.layScore.RowCount = 1;
            this.layScore.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layScore.Size = new System.Drawing.Size(438, 58);
            this.layScore.TabIndex = 46;
            // 
            // rjButton4
            // 
            this.rjButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton4.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton4.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton4.BorderRadius = 10;
            this.rjButton4.BorderSize = 1;
            this.rjButton4.ButtonImage = null;
            this.rjButton4.Corner = BeeGlobal.Corner.Right;
            this.rjButton4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton4.Enabled = false;
            this.rjButton4.FlatAppearance.BorderSize = 0;
            this.rjButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton4.ForeColor = System.Drawing.Color.Black;
            this.rjButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton4.IsCLick = false;
            this.rjButton4.IsNotChange = true;
            this.rjButton4.IsRect = false;
            this.rjButton4.IsUnGroup = false;
            this.rjButton4.Location = new System.Drawing.Point(433, 0);
            this.rjButton4.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton4.Name = "rjButton4";
            this.rjButton4.Size = new System.Drawing.Size(5, 58);
            this.rjButton4.TabIndex = 37;
            this.rjButton4.TextColor = System.Drawing.Color.Black;
            this.rjButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton4.UseVisualStyleBackColor = false;
            // 
            // numScore
            // 
            this.numScore.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numScore.Location = new System.Drawing.Point(293, 0);
            this.numScore.Margin = new System.Windows.Forms.Padding(0);
            this.numScore.Maxnimum = 100F;
            this.numScore.Minimum = 0F;
            this.numScore.Name = "numScore";
            this.numScore.Size = new System.Drawing.Size(140, 58);
            this.numScore.Step = 1F;
            this.numScore.TabIndex = 35;
            this.numScore.Value = 100F;
            this.numScore.ValueChanged += new System.EventHandler(this.numScore_ValueChanged);
            // 
            // rjButton5
            // 
            this.rjButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton5.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton5.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton5.BorderRadius = 5;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.ButtonImage = null;
            this.rjButton5.Corner = BeeGlobal.Corner.Left;
            this.rjButton5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton5.Enabled = false;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton5.IsCLick = false;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = false;
            this.rjButton5.IsUnGroup = false;
            this.rjButton5.Location = new System.Drawing.Point(0, 0);
            this.rjButton5.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(5, 58);
            this.rjButton5.TabIndex = 2;
            this.rjButton5.TextColor = System.Drawing.Color.Black;
            this.rjButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton5.UseVisualStyleBackColor = false;
            this.rjButton5.Click += new System.EventHandler(this.rjButton5_Click);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.trackScore, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(5, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.39683F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.60317F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(288, 58);
            this.tableLayoutPanel7.TabIndex = 36;
            // 
            // trackScore
            // 
            this.trackScore.BackColor = System.Drawing.Color.Transparent;
            this.trackScore.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackScore.Location = new System.Drawing.Point(3, 14);
            this.trackScore.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.trackScore.Max = 10F;
            this.trackScore.Min = 0F;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(282, 44);
            this.trackScore.Step = 0.1F;
            this.trackScore.TabIndex = 28;
            this.trackScore.Value = 1F;
            this.trackScore.ValueScore = 0F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 467);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(440, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.Transparent;
            this.btnTest.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderRadius = 1;
            this.btnTest.BorderSize = 1;
            this.btnTest.ButtonImage = null;
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.IsCLick = false;
            this.btnTest.IsNotChange = true;
            this.btnTest.IsRect = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(20, 570);
            this.btnTest.Margin = new System.Windows.Forms.Padding(20);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(408, 247);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 170);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 25);
            this.label7.TabIndex = 38;
            this.label7.Text = "Line 1";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(454, 843);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel11, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 9;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(448, 837);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 3;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel14.Controls.Add(this.ckBitwiseNot, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.ckSubPixel, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.ckSIMD, 0, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 395);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 5, 3, 10);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel14.Size = new System.Drawing.Size(440, 55);
            this.tableLayoutPanel14.TabIndex = 53;
            // 
            // ckBitwiseNot
            // 
            this.ckBitwiseNot.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ckBitwiseNot.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.ckBitwiseNot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckBitwiseNot.BackgroundImage")));
            this.ckBitwiseNot.BorderColor = System.Drawing.Color.Silver;
            this.ckBitwiseNot.BorderRadius = 10;
            this.ckBitwiseNot.BorderSize = 1;
            this.ckBitwiseNot.ButtonImage = null;
            this.ckBitwiseNot.Corner = BeeGlobal.Corner.Both;
            this.ckBitwiseNot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckBitwiseNot.FlatAppearance.BorderSize = 0;
            this.ckBitwiseNot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckBitwiseNot.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckBitwiseNot.ForeColor = System.Drawing.Color.Black;
            this.ckBitwiseNot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckBitwiseNot.IsCLick = false;
            this.ckBitwiseNot.IsNotChange = false;
            this.ckBitwiseNot.IsRect = false;
            this.ckBitwiseNot.IsUnGroup = true;
            this.ckBitwiseNot.Location = new System.Drawing.Point(302, 3);
            this.ckBitwiseNot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckBitwiseNot.Name = "ckBitwiseNot";
            this.ckBitwiseNot.Size = new System.Drawing.Size(133, 49);
            this.ckBitwiseNot.TabIndex = 3;
            this.ckBitwiseNot.Text = "Not";
            this.ckBitwiseNot.TextColor = System.Drawing.Color.Black;
            this.ckBitwiseNot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckBitwiseNot.UseVisualStyleBackColor = false;
            this.ckBitwiseNot.Click += new System.EventHandler(this.ckBitwiseNot_Click);
            // 
            // ckSubPixel
            // 
            this.ckSubPixel.BackColor = System.Drawing.Color.Transparent;
            this.ckSubPixel.BackgroundColor = System.Drawing.Color.Transparent;
            this.ckSubPixel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckSubPixel.BackgroundImage")));
            this.ckSubPixel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ckSubPixel.BorderColor = System.Drawing.Color.Transparent;
            this.ckSubPixel.BorderRadius = 10;
            this.ckSubPixel.BorderSize = 1;
            this.ckSubPixel.ButtonImage = null;
            this.ckSubPixel.Corner = BeeGlobal.Corner.Both;
            this.ckSubPixel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckSubPixel.FlatAppearance.BorderSize = 0;
            this.ckSubPixel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckSubPixel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckSubPixel.ForeColor = System.Drawing.Color.Black;
            this.ckSubPixel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckSubPixel.IsCLick = true;
            this.ckSubPixel.IsNotChange = false;
            this.ckSubPixel.IsRect = false;
            this.ckSubPixel.IsUnGroup = true;
            this.ckSubPixel.Location = new System.Drawing.Point(147, 3);
            this.ckSubPixel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckSubPixel.Name = "ckSubPixel";
            this.ckSubPixel.Size = new System.Drawing.Size(145, 49);
            this.ckSubPixel.TabIndex = 4;
            this.ckSubPixel.Text = "SubPixel";
            this.ckSubPixel.TextColor = System.Drawing.Color.Black;
            this.ckSubPixel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckSubPixel.UseVisualStyleBackColor = false;
            this.ckSubPixel.Click += new System.EventHandler(this.ckSubPixel_Click);
            // 
            // ckSIMD
            // 
            this.ckSIMD.BackColor = System.Drawing.Color.Transparent;
            this.ckSIMD.BackgroundColor = System.Drawing.Color.Transparent;
            this.ckSIMD.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ckSIMD.BackgroundImage")));
            this.ckSIMD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ckSIMD.BorderColor = System.Drawing.Color.Transparent;
            this.ckSIMD.BorderRadius = 10;
            this.ckSIMD.BorderSize = 1;
            this.ckSIMD.ButtonImage = null;
            this.ckSIMD.Corner = BeeGlobal.Corner.Both;
            this.ckSIMD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckSIMD.FlatAppearance.BorderSize = 0;
            this.ckSIMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckSIMD.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckSIMD.ForeColor = System.Drawing.Color.Black;
            this.ckSIMD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ckSIMD.IsCLick = true;
            this.ckSIMD.IsNotChange = false;
            this.ckSIMD.IsRect = false;
            this.ckSIMD.IsUnGroup = true;
            this.ckSIMD.Location = new System.Drawing.Point(5, 3);
            this.ckSIMD.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckSIMD.Name = "ckSIMD";
            this.ckSIMD.Size = new System.Drawing.Size(132, 49);
            this.ckSIMD.TabIndex = 2;
            this.ckSIMD.Text = "SIMD";
            this.ckSIMD.TextColor = System.Drawing.Color.Black;
            this.ckSIMD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckSIMD.UseVisualStyleBackColor = false;
            this.ckSIMD.Click += new System.EventHandler(this.ckSIMD_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 365);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(440, 25);
            this.label3.TabIndex = 52;
            this.label3.Text = "Search Algorithm";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.btnHighSpeed, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.btnNormal, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 280);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 5);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Size = new System.Drawing.Size(440, 60);
            this.tableLayoutPanel13.TabIndex = 51;
            // 
            // btnHighSpeed
            // 
            this.btnHighSpeed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnHighSpeed.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnHighSpeed.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHighSpeed.BackgroundImage")));
            this.btnHighSpeed.BorderColor = System.Drawing.Color.Silver;
            this.btnHighSpeed.BorderRadius = 10;
            this.btnHighSpeed.BorderSize = 1;
            this.btnHighSpeed.ButtonImage = null;
            this.btnHighSpeed.Corner = BeeGlobal.Corner.Right;
            this.btnHighSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHighSpeed.FlatAppearance.BorderSize = 0;
            this.btnHighSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHighSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHighSpeed.ForeColor = System.Drawing.Color.Black;
            this.btnHighSpeed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHighSpeed.IsCLick = false;
            this.btnHighSpeed.IsNotChange = false;
            this.btnHighSpeed.IsRect = false;
            this.btnHighSpeed.IsUnGroup = false;
            this.btnHighSpeed.Location = new System.Drawing.Point(220, 3);
            this.btnHighSpeed.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnHighSpeed.Name = "btnHighSpeed";
            this.btnHighSpeed.Size = new System.Drawing.Size(217, 55);
            this.btnHighSpeed.TabIndex = 3;
            this.btnHighSpeed.Text = "High Speed";
            this.btnHighSpeed.TextColor = System.Drawing.Color.Black;
            this.btnHighSpeed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHighSpeed.UseVisualStyleBackColor = false;
            this.btnHighSpeed.Click += new System.EventHandler(this.btnHighSpeed_Click);
            // 
            // btnNormal
            // 
            this.btnNormal.BackColor = System.Drawing.Color.Transparent;
            this.btnNormal.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnNormal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNormal.BackgroundImage")));
            this.btnNormal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNormal.BorderColor = System.Drawing.Color.Transparent;
            this.btnNormal.BorderRadius = 10;
            this.btnNormal.BorderSize = 1;
            this.btnNormal.ButtonImage = null;
            this.btnNormal.Corner = BeeGlobal.Corner.Left;
            this.btnNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNormal.FlatAppearance.BorderSize = 0;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNormal.ForeColor = System.Drawing.Color.Black;
            this.btnNormal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNormal.IsCLick = true;
            this.btnNormal.IsNotChange = false;
            this.btnNormal.IsRect = false;
            this.btnNormal.IsUnGroup = false;
            this.btnNormal.Location = new System.Drawing.Point(3, 3);
            this.btnNormal.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(217, 55);
            this.btnNormal.TabIndex = 2;
            this.btnNormal.Text = "Normal";
            this.btnNormal.TextColor = System.Drawing.Color.Black;
            this.btnNormal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNormal.UseVisualStyleBackColor = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 250);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(440, 25);
            this.label2.TabIndex = 50;
            this.label2.Text = "Search Algorithm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel11.ColumnCount = 4;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel11.Controls.Add(this.rjButton8, 3, 0);
            this.tableLayoutPanel11.Controls.Add(this.numOverLap, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.rjButton9, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel12, 1, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(5, 165);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(440, 65);
            this.tableLayoutPanel11.TabIndex = 49;
            // 
            // rjButton8
            // 
            this.rjButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton8.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton8.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton8.BorderRadius = 10;
            this.rjButton8.BorderSize = 1;
            this.rjButton8.ButtonImage = null;
            this.rjButton8.Corner = BeeGlobal.Corner.Right;
            this.rjButton8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton8.Enabled = false;
            this.rjButton8.FlatAppearance.BorderSize = 0;
            this.rjButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton8.ForeColor = System.Drawing.Color.Black;
            this.rjButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton8.IsCLick = false;
            this.rjButton8.IsNotChange = true;
            this.rjButton8.IsRect = false;
            this.rjButton8.IsUnGroup = false;
            this.rjButton8.Location = new System.Drawing.Point(435, 0);
            this.rjButton8.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton8.Name = "rjButton8";
            this.rjButton8.Size = new System.Drawing.Size(5, 65);
            this.rjButton8.TabIndex = 37;
            this.rjButton8.TextColor = System.Drawing.Color.Black;
            this.rjButton8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton8.UseVisualStyleBackColor = false;
            // 
            // numOverLap
            // 
            this.numOverLap.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numOverLap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numOverLap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numOverLap.Location = new System.Drawing.Point(295, 0);
            this.numOverLap.Margin = new System.Windows.Forms.Padding(0);
            this.numOverLap.Maxnimum = 100F;
            this.numOverLap.Minimum = 0F;
            this.numOverLap.Name = "numOverLap";
            this.numOverLap.Size = new System.Drawing.Size(140, 65);
            this.numOverLap.Step = 1F;
            this.numOverLap.TabIndex = 35;
            this.numOverLap.Value = 100F;
            // 
            // rjButton9
            // 
            this.rjButton9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton9.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton9.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton9.BorderRadius = 10;
            this.rjButton9.BorderSize = 1;
            this.rjButton9.ButtonImage = null;
            this.rjButton9.Corner = BeeGlobal.Corner.Left;
            this.rjButton9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton9.Enabled = false;
            this.rjButton9.FlatAppearance.BorderSize = 0;
            this.rjButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton9.ForeColor = System.Drawing.Color.Black;
            this.rjButton9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton9.IsCLick = false;
            this.rjButton9.IsNotChange = true;
            this.rjButton9.IsRect = false;
            this.rjButton9.IsUnGroup = false;
            this.rjButton9.Location = new System.Drawing.Point(0, 0);
            this.rjButton9.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton9.Name = "rjButton9";
            this.rjButton9.Size = new System.Drawing.Size(10, 65);
            this.rjButton9.TabIndex = 2;
            this.rjButton9.TextColor = System.Drawing.Color.Black;
            this.rjButton9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton9.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel12.ColumnCount = 1;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Controls.Add(this.trackMaxOverLap, 0, 1);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 2;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.39683F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.60317F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(285, 65);
            this.tableLayoutPanel12.TabIndex = 36;
            // 
            // trackMaxOverLap
            // 
            this.trackMaxOverLap.BackColor = System.Drawing.Color.Transparent;
            this.trackMaxOverLap.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackMaxOverLap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackMaxOverLap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackMaxOverLap.Location = new System.Drawing.Point(3, 16);
            this.trackMaxOverLap.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.trackMaxOverLap.Max = 100F;
            this.trackMaxOverLap.Min = 0F;
            this.trackMaxOverLap.Name = "trackMaxOverLap";
            this.trackMaxOverLap.Size = new System.Drawing.Size(279, 49);
            this.trackMaxOverLap.Step = 1F;
            this.trackMaxOverLap.TabIndex = 28;
            this.trackMaxOverLap.Value = 100F;
            this.trackMaxOverLap.ValueScore = 0F;
            this.trackMaxOverLap.ValueChanged += new System.Action<float>(this.trackMaxOverLap_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 135);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 25);
            this.label6.TabIndex = 48;
            this.label6.Text = "OverLap Range";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel9.ColumnCount = 4;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel9.Controls.Add(this.numScale, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.rjButton6, 3, 0);
            this.tableLayoutPanel9.Controls.Add(this.rjButton7, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel10, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 50);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(440, 65);
            this.tableLayoutPanel9.TabIndex = 47;
            // 
            // numScale
            // 
            this.numScale.DecimalPlaces = 3;
            this.numScale.Location = new System.Drawing.Point(298, 3);
            this.numScale.Name = "numScale";
            this.numScale.Size = new System.Drawing.Size(120, 31);
            this.numScale.TabIndex = 48;
            this.numScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numScale.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
            // 
            // rjButton6
            // 
            this.rjButton6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton6.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton6.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton6.BorderRadius = 10;
            this.rjButton6.BorderSize = 1;
            this.rjButton6.ButtonImage = null;
            this.rjButton6.Corner = BeeGlobal.Corner.Right;
            this.rjButton6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton6.Enabled = false;
            this.rjButton6.FlatAppearance.BorderSize = 0;
            this.rjButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton6.ForeColor = System.Drawing.Color.Black;
            this.rjButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton6.IsCLick = false;
            this.rjButton6.IsNotChange = true;
            this.rjButton6.IsRect = false;
            this.rjButton6.IsUnGroup = false;
            this.rjButton6.Location = new System.Drawing.Point(435, 0);
            this.rjButton6.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton6.Name = "rjButton6";
            this.rjButton6.Size = new System.Drawing.Size(5, 65);
            this.rjButton6.TabIndex = 37;
            this.rjButton6.TextColor = System.Drawing.Color.Black;
            this.rjButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton6.UseVisualStyleBackColor = false;
            // 
            // rjButton7
            // 
            this.rjButton7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton7.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.rjButton7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton7.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton7.BorderRadius = 10;
            this.rjButton7.BorderSize = 1;
            this.rjButton7.ButtonImage = null;
            this.rjButton7.Corner = BeeGlobal.Corner.Left;
            this.rjButton7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton7.Enabled = false;
            this.rjButton7.FlatAppearance.BorderSize = 0;
            this.rjButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton7.ForeColor = System.Drawing.Color.Black;
            this.rjButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton7.IsCLick = false;
            this.rjButton7.IsNotChange = true;
            this.rjButton7.IsRect = false;
            this.rjButton7.IsUnGroup = false;
            this.rjButton7.Location = new System.Drawing.Point(0, 0);
            this.rjButton7.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton7.Name = "rjButton7";
            this.rjButton7.Size = new System.Drawing.Size(10, 65);
            this.rjButton7.TabIndex = 2;
            this.rjButton7.TextColor = System.Drawing.Color.Black;
            this.rjButton7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton7.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Controls.Add(this.trackAngle, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.39683F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.60317F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(285, 65);
            this.tableLayoutPanel10.TabIndex = 36;
            // 
            // trackAngle
            // 
            this.trackAngle.BackColor = System.Drawing.Color.Transparent;
            this.trackAngle.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackAngle.Location = new System.Drawing.Point(3, 16);
            this.trackAngle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.trackAngle.Max = 180F;
            this.trackAngle.Min = 0F;
            this.trackAngle.Name = "trackAngle";
            this.trackAngle.Size = new System.Drawing.Size(279, 49);
            this.trackAngle.Step = 1F;
            this.trackAngle.TabIndex = 28;
            this.trackAngle.Value = 100F;
            this.trackAngle.ValueScore = 0F;
            this.trackAngle.ValueChanged += new System.Action<float>(this.trackAngle_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(440, 25);
            this.label4.TabIndex = 39;
            this.label4.Text = "Angle Range";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pCany
            // 
            this.pCany.Controls.Add(this.btnAreaBlack);
            this.pCany.Controls.Add(this.btnAreaWhite);
            this.pCany.Enabled = false;
            this.pCany.Location = new System.Drawing.Point(529, 529);
            this.pCany.Name = "pCany";
            this.pCany.Size = new System.Drawing.Size(386, 50);
            this.pCany.TabIndex = 12;
            this.pCany.Visible = false;
            // 
            // btnAreaBlack
            // 
            this.btnAreaBlack.BackColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaBlack.BackgroundImage")));
            this.btnAreaBlack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAreaBlack.BorderColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BorderRadius = 5;
            this.btnAreaBlack.BorderSize = 1;
            this.btnAreaBlack.ButtonImage = null;
            this.btnAreaBlack.Corner = BeeGlobal.Corner.Both;
            this.btnAreaBlack.FlatAppearance.BorderSize = 0;
            this.btnAreaBlack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaBlack.ForeColor = System.Drawing.Color.Black;
            this.btnAreaBlack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaBlack.IsCLick = true;
            this.btnAreaBlack.IsNotChange = false;
            this.btnAreaBlack.IsRect = false;
            this.btnAreaBlack.IsUnGroup = false;
            this.btnAreaBlack.Location = new System.Drawing.Point(3, 5);
            this.btnAreaBlack.Name = "btnAreaBlack";
            this.btnAreaBlack.Size = new System.Drawing.Size(185, 40);
            this.btnAreaBlack.TabIndex = 7;
            this.btnAreaBlack.Text = "Vùng Tối";
            this.btnAreaBlack.TextColor = System.Drawing.Color.Black;
            this.btnAreaBlack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAreaBlack.UseVisualStyleBackColor = false;
            // 
            // btnAreaWhite
            // 
            this.btnAreaWhite.BackColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaWhite.BackgroundImage")));
            this.btnAreaWhite.BorderColor = System.Drawing.Color.Silver;
            this.btnAreaWhite.BorderRadius = 5;
            this.btnAreaWhite.BorderSize = 1;
            this.btnAreaWhite.ButtonImage = null;
            this.btnAreaWhite.Corner = BeeGlobal.Corner.Both;
            this.btnAreaWhite.FlatAppearance.BorderSize = 0;
            this.btnAreaWhite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaWhite.ForeColor = System.Drawing.Color.Black;
            this.btnAreaWhite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaWhite.IsCLick = false;
            this.btnAreaWhite.IsNotChange = false;
            this.btnAreaWhite.IsRect = false;
            this.btnAreaWhite.IsUnGroup = false;
            this.btnAreaWhite.Location = new System.Drawing.Point(195, 5);
            this.btnAreaWhite.Name = "btnAreaWhite";
            this.btnAreaWhite.Size = new System.Drawing.Size(188, 40);
            this.btnAreaWhite.TabIndex = 9;
            this.btnAreaWhite.Text = "Vùng sáng";
            this.btnAreaWhite.TextColor = System.Drawing.Color.Black;
            this.btnAreaWhite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAreaWhite.UseVisualStyleBackColor = false;
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 887);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(462, 52);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.pCany);
            this.Controls.Add(this.tabControl2);
            this.Name = "ToolMeasure";
            this.Size = new System.Drawing.Size(462, 939);
            this.Load += new System.EventHandler(this.ToolCalculator_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.layScore.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.pCany.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TabPage tabPage4;
        private RJButton ckSIMD;
        private RJButton ckBitwiseNot;
        private RJButton ckSubPixel;
        public System.ComponentModel.BackgroundWorker threadProcess;
        private System.Windows.Forms.Panel pCany;
        private RJButton btnAreaBlack;
        private RJButton btnAreaWhite;
        private RJButton btnHighSpeed;
        private RJButton btnNormal;
        private RJButton btnTest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel layScore;
        private RJButton rjButton5;
        private CustomNumeric numScore;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private RJButton rjButton4;
        public TrackBar2 trackScore;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private RJButton rjButton6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        public TrackBar2 trackAngle;
        private System.Windows.Forms.Label label4;
        private RJButton rjButton7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private RJButton rjButton8;
        private CustomNumeric numOverLap;
        private RJButton rjButton9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        public TrackBar2 trackMaxOverLap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cb1;
        private System.Windows.Forms.ComboBox cb2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox cb4;
        private System.Windows.Forms.ComboBox cb3;
        private RJButton rjButton12;
        private RJButton rjButton11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private RJButton rjButton13;
        private System.Windows.Forms.ComboBox cb6;
        private System.Windows.Forms.ComboBox cb5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton rjButton1;
        private System.Windows.Forms.ComboBox cb8;
        private System.Windows.Forms.ComboBox cb7;
        private System.Windows.Forms.NumericUpDown numScale;
        private GroupControl.OK_Cancel oK_Cancel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton rjButton2;
        private System.Windows.Forms.ComboBox cbMeasure;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private RJButton rjButton10;
        private System.Windows.Forms.ComboBox cbDirect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private RJButton rjButton3;
        private System.Windows.Forms.ComboBox cbMethord;
    }
}
