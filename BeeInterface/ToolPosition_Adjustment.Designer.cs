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
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.imgTemp = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.btnLearning = new BeeInterface.RJButton();
            this.rjButton15 = new BeeInterface.RJButton();
            this.btnCrop = new BeeInterface.RJButton();
            this.btnCropArea = new BeeInterface.RJButton();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.trackScore = new BeeInterface.AdjustBar();
            this.rjButton2 = new BeeInterface.RJButton();
            this.rjButton5 = new BeeInterface.RJButton();
            this.trackBar21 = new BeeInterface.TrackBar2();
            this.ckBitwiseNot = new BeeInterface.RJButton();
            this.ckSubPixel = new BeeInterface.RJButton();
            this.ckSIMD = new BeeInterface.RJButton();
            this.numOK = new BeeInterface.CustomNumeric();
            this.numDelay = new BeeInterface.CustomNumeric();
            this.btnHighSpeed = new BeeInterface.RJButton();
            this.btnNormal = new BeeInterface.RJButton();
            this.btnOnTrig = new BeeInterface.RJButton();
            this.btnOffTrig = new BeeInterface.RJButton();
            this.trackAngle = new BeeInterface.AdjustBar();
            this.trackMaxOverLap = new BeeInterface.AdjustBar();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.tabPage4.Controls.Add(this.tableLayoutPanel9);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 854);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel14, 0, 7);
            this.tableLayoutPanel9.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel5, 0, 9);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel13, 0, 5);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel8, 0, 8);
            this.tableLayoutPanel9.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel9.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel9.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.trackAngle, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.trackMaxOverLap, 0, 3);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 11;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 248F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(386, 848);
            this.tableLayoutPanel9.TabIndex = 1;
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
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 335);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 5, 3, 10);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(378, 57);
            this.tableLayoutPanel14.TabIndex = 53;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 305);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(378, 25);
            this.label3.TabIndex = 52;
            this.label3.Text = "Option more";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel5.Controls.Add(this.label6, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.numOK, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.numDelay, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 505);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(386, 114);
            this.tableLayoutPanel5.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(272, 59);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 55);
            this.label6.TabIndex = 41;
            this.label6.Text = "ms";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(272, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 59);
            this.label5.TabIndex = 40;
            this.label5.Text = "unit";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 0);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 59);
            this.label10.TabIndex = 28;
            this.label10.Text = "Count Check";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 59);
            this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 55);
            this.label11.TabIndex = 29;
            this.label11.Text = "Delay After";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 230);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 5);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Size = new System.Drawing.Size(378, 60);
            this.tableLayoutPanel13.TabIndex = 51;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel8.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnOnTrig, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnOffTrig, 2, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 422);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(380, 66);
            this.tableLayoutPanel8.TabIndex = 47;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Silver;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 66);
            this.label14.TabIndex = 44;
            this.label14.Text = "Auto Trigger";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 200);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 25);
            this.label2.TabIndex = 50;
            this.label2.Text = "Search Algorithm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 105);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 25);
            this.label1.TabIndex = 48;
            this.label1.Text = "OverLap Range";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 10);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(378, 25);
            this.label7.TabIndex = 39;
            this.label7.Text = "Angle Range";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.tabControl2.Size = new System.Drawing.Size(400, 892);
            this.tabControl2.TabIndex = 17;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Controls.Add(this.rjButton2);
            this.tabPage3.Controls.Add(this.rjButton5);
            this.tabPage3.Controls.Add(this.trackBar21);
            this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(392, 854);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Basic";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label16, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 848);
            this.tableLayoutPanel1.TabIndex = 25;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.98324F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.01676F));
            this.tableLayoutPanel4.Controls.Add(this.imgTemp, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnLearning, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(1, 169);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(1, 5, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(382, 95);
            this.tableLayoutPanel4.TabIndex = 43;
            // 
            // imgTemp
            // 
            this.imgTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgTemp.Location = new System.Drawing.Point(5, 5);
            this.imgTemp.Margin = new System.Windows.Forms.Padding(5);
            this.imgTemp.Name = "imgTemp";
            this.imgTemp.Size = new System.Drawing.Size(207, 85);
            this.imgTemp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgTemp.TabIndex = 31;
            this.imgTemp.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(5, 139);
            this.label15.Margin = new System.Windows.Forms.Padding(5, 2, 3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(378, 25);
            this.label15.TabIndex = 42;
            this.label15.Text = "Training Sample";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.Controls.Add(this.rjButton15, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCrop, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropArea, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 97);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(386, 40);
            this.tableLayoutPanel3.TabIndex = 41;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(5, 72);
            this.label16.Margin = new System.Windows.Forms.Padding(5, 2, 3, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(137, 25);
            this.label16.TabIndex = 40;
            this.label16.Text = "Choose Area";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(5, 5);
            this.label17.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(149, 25);
            this.label17.TabIndex = 38;
            this.label17.Text = "Search Range";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnCropFull, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCropHalt, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(386, 40);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(5, 266);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 2, 3, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 25);
            this.label13.TabIndex = 45;
            this.label13.Text = "Score";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnTest.Location = new System.Drawing.Point(2, 348);
            this.btnTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(382, 498);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnLearning
            // 
            this.btnLearning.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLearning.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLearning.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLearning.BackgroundImage")));
            this.btnLearning.BorderColor = System.Drawing.Color.Transparent;
            this.btnLearning.BorderRadius = 10;
            this.btnLearning.BorderSize = 1;
            this.btnLearning.ButtonImage = null;
            this.btnLearning.Corner = BeeGlobal.Corner.Both;
            this.btnLearning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLearning.FlatAppearance.BorderSize = 0;
            this.btnLearning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLearning.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLearning.ForeColor = System.Drawing.Color.Black;
            this.btnLearning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLearning.IsCLick = false;
            this.btnLearning.IsNotChange = true;
            this.btnLearning.IsRect = false;
            this.btnLearning.IsUnGroup = true;
            this.btnLearning.Location = new System.Drawing.Point(227, 10);
            this.btnLearning.Margin = new System.Windows.Forms.Padding(10);
            this.btnLearning.Name = "btnLearning";
            this.btnLearning.Size = new System.Drawing.Size(145, 75);
            this.btnLearning.TabIndex = 5;
            this.btnLearning.Text = "Teach Sample";
            this.btnLearning.TextColor = System.Drawing.Color.Black;
            this.btnLearning.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLearning.UseVisualStyleBackColor = false;
            this.btnLearning.Click += new System.EventHandler(this.btnLearning_Click);
            // 
            // rjButton15
            // 
            this.rjButton15.BackColor = System.Drawing.Color.Transparent;
            this.rjButton15.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton15.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton15.BackgroundImage")));
            this.rjButton15.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton15.BorderRadius = 10;
            this.rjButton15.BorderSize = 1;
            this.rjButton15.ButtonImage = null;
            this.rjButton15.Corner = BeeGlobal.Corner.Right;
            this.rjButton15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton15.Enabled = false;
            this.rjButton15.FlatAppearance.BorderSize = 0;
            this.rjButton15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton15.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton15.ForeColor = System.Drawing.Color.Black;
            this.rjButton15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton15.IsCLick = false;
            this.rjButton15.IsNotChange = false;
            this.rjButton15.IsRect = false;
            this.rjButton15.IsUnGroup = false;
            this.rjButton15.Location = new System.Drawing.Point(260, 3);
            this.rjButton15.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.rjButton15.Name = "rjButton15";
            this.rjButton15.Size = new System.Drawing.Size(123, 34);
            this.rjButton15.TabIndex = 4;
            this.rjButton15.Text = "Area Mask";
            this.rjButton15.TextColor = System.Drawing.Color.Black;
            this.rjButton15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton15.UseVisualStyleBackColor = false;
            // 
            // btnCrop
            // 
            this.btnCrop.BackColor = System.Drawing.Color.Transparent;
            this.btnCrop.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCrop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCrop.BackgroundImage")));
            this.btnCrop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCrop.BorderColor = System.Drawing.Color.Transparent;
            this.btnCrop.BorderRadius = 10;
            this.btnCrop.BorderSize = 1;
            this.btnCrop.ButtonImage = null;
            this.btnCrop.Corner = BeeGlobal.Corner.Left;
            this.btnCrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCrop.FlatAppearance.BorderSize = 0;
            this.btnCrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrop.ForeColor = System.Drawing.Color.Black;
            this.btnCrop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCrop.IsCLick = false;
            this.btnCrop.IsNotChange = false;
            this.btnCrop.IsRect = false;
            this.btnCrop.IsUnGroup = false;
            this.btnCrop.Location = new System.Drawing.Point(3, 3);
            this.btnCrop.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnCrop.Name = "btnCrop";
            this.btnCrop.Size = new System.Drawing.Size(121, 34);
            this.btnCrop.TabIndex = 2;
            this.btnCrop.Text = "Area Temp";
            this.btnCrop.TextColor = System.Drawing.Color.Black;
            this.btnCrop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCrop.UseVisualStyleBackColor = false;
            this.btnCrop.Click += new System.EventHandler(this.btnCropRect_Click);
            // 
            // btnCropArea
            // 
            this.btnCropArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropArea.BackgroundImage")));
            this.btnCropArea.BorderColor = System.Drawing.Color.Silver;
            this.btnCropArea.BorderRadius = 5;
            this.btnCropArea.BorderSize = 1;
            this.btnCropArea.ButtonImage = null;
            this.btnCropArea.Corner = BeeGlobal.Corner.None;
            this.btnCropArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropArea.ForeColor = System.Drawing.Color.Black;
            this.btnCropArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropArea.IsCLick = true;
            this.btnCropArea.IsNotChange = false;
            this.btnCropArea.IsRect = false;
            this.btnCropArea.IsUnGroup = false;
            this.btnCropArea.Location = new System.Drawing.Point(124, 3);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(136, 34);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
            // 
            // btnCropFull
            // 
            this.btnCropFull.BackColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BorderColor = System.Drawing.Color.Silver;
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.ButtonImage = null;
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropFull.ForeColor = System.Drawing.Color.Black;
            this.btnCropFull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropFull.IsCLick = false;
            this.btnCropFull.IsNotChange = false;
            this.btnCropFull.IsRect = false;
            this.btnCropFull.IsUnGroup = false;
            this.btnCropFull.Location = new System.Drawing.Point(193, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(190, 40);
            this.btnCropFull.TabIndex = 3;
            this.btnCropFull.Text = "Partial";
            this.btnCropFull.TextColor = System.Drawing.Color.Black;
            this.btnCropFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropFull.UseVisualStyleBackColor = false;
            this.btnCropFull.Click += new System.EventHandler(this.btnCropFull_Click);
            // 
            // btnCropHalt
            // 
            this.btnCropHalt.BackColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.ButtonImage = null;
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropHalt.ForeColor = System.Drawing.Color.Black;
            this.btnCropHalt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropHalt.IsCLick = true;
            this.btnCropHalt.IsNotChange = false;
            this.btnCropHalt.IsRect = false;
            this.btnCropHalt.IsUnGroup = false;
            this.btnCropHalt.Location = new System.Drawing.Point(3, 0);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(190, 40);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // trackScore
            // 
            this.trackScore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackScore.Location = new System.Drawing.Point(10, 294);
            this.trackScore.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackScore.Max = 100F;
            this.trackScore.Min = 0F;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(373, 49);
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 46;
            this.trackScore.Value = 0F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // rjButton2
            // 
            this.rjButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rjButton2.BackColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton2.BackgroundImage")));
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 5;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ButtonImage = null;
            this.rjButton2.Corner = BeeGlobal.Corner.Both;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = true;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(213, 2277);
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
            this.rjButton5.BackColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton5.BackgroundImage")));
            this.rjButton5.BorderColor = System.Drawing.Color.Silver;
            this.rjButton5.BorderRadius = 5;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.ButtonImage = null;
            this.rjButton5.Corner = BeeGlobal.Corner.Both;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton5.IsCLick = true;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = false;
            this.rjButton5.IsUnGroup = false;
            this.rjButton5.Location = new System.Drawing.Point(6, 2277);
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
            this.ckBitwiseNot.Location = new System.Drawing.Point(260, 3);
            this.ckBitwiseNot.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckBitwiseNot.Name = "ckBitwiseNot";
            this.ckBitwiseNot.Size = new System.Drawing.Size(113, 51);
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
            this.ckSubPixel.Location = new System.Drawing.Point(127, 3);
            this.ckSubPixel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.ckSubPixel.Name = "ckSubPixel";
            this.ckSubPixel.Size = new System.Drawing.Size(123, 51);
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
            this.ckSIMD.Size = new System.Drawing.Size(112, 51);
            this.ckSIMD.TabIndex = 2;
            this.ckSIMD.Text = "SIMD";
            this.ckSIMD.TextColor = System.Drawing.Color.Black;
            this.ckSIMD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckSIMD.UseVisualStyleBackColor = false;
            this.ckSIMD.Click += new System.EventHandler(this.ckSIMD_Click);
            // 
            // numOK
            // 
            this.numOK.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numOK.Location = new System.Drawing.Point(115, 0);
            this.numOK.Margin = new System.Windows.Forms.Padding(0);
            this.numOK.Maxnimum = 100F;
            this.numOK.Minimum = 0F;
            this.numOK.Name = "numOK";
            this.numOK.Size = new System.Drawing.Size(154, 59);
            this.numOK.Step = 1F;
            this.numOK.TabIndex = 39;
            this.numOK.Value = 0F;
            // 
            // numDelay
            // 
            this.numDelay.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.numDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numDelay.Location = new System.Drawing.Point(115, 59);
            this.numDelay.Margin = new System.Windows.Forms.Padding(0);
            this.numDelay.Maxnimum = 100F;
            this.numDelay.Minimum = 0F;
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(154, 55);
            this.numDelay.Step = 1F;
            this.numDelay.TabIndex = 34;
            this.numDelay.Value = 0F;
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
            this.btnHighSpeed.Location = new System.Drawing.Point(189, 3);
            this.btnHighSpeed.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnHighSpeed.Name = "btnHighSpeed";
            this.btnHighSpeed.Size = new System.Drawing.Size(186, 55);
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
            this.btnNormal.Size = new System.Drawing.Size(186, 55);
            this.btnNormal.TabIndex = 2;
            this.btnNormal.Text = "Normal";
            this.btnNormal.TextColor = System.Drawing.Color.Black;
            this.btnNormal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNormal.UseVisualStyleBackColor = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnOnTrig
            // 
            this.btnOnTrig.BackColor = System.Drawing.Color.LightGray;
            this.btnOnTrig.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnOnTrig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOnTrig.BorderColor = System.Drawing.Color.Transparent;
            this.btnOnTrig.BorderRadius = 10;
            this.btnOnTrig.BorderSize = 1;
            this.btnOnTrig.ButtonImage = null;
            this.btnOnTrig.Corner = BeeGlobal.Corner.Left;
            this.btnOnTrig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOnTrig.FlatAppearance.BorderSize = 0;
            this.btnOnTrig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnTrig.ForeColor = System.Drawing.Color.Black;
            this.btnOnTrig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOnTrig.IsCLick = true;
            this.btnOnTrig.IsNotChange = false;
            this.btnOnTrig.IsRect = false;
            this.btnOnTrig.IsUnGroup = false;
            this.btnOnTrig.Location = new System.Drawing.Point(114, 0);
            this.btnOnTrig.Margin = new System.Windows.Forms.Padding(0);
            this.btnOnTrig.Name = "btnOnTrig";
            this.btnOnTrig.Size = new System.Drawing.Size(152, 66);
            this.btnOnTrig.TabIndex = 31;
            this.btnOnTrig.Text = "ON";
            this.btnOnTrig.TextColor = System.Drawing.Color.Black;
            this.btnOnTrig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOnTrig.UseVisualStyleBackColor = false;
            this.btnOnTrig.Click += new System.EventHandler(this.btnOnTrig_Click);
            // 
            // btnOffTrig
            // 
            this.btnOffTrig.BackColor = System.Drawing.Color.Gray;
            this.btnOffTrig.BackgroundColor = System.Drawing.Color.Gray;
            this.btnOffTrig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOffTrig.BorderColor = System.Drawing.Color.LightGray;
            this.btnOffTrig.BorderRadius = 5;
            this.btnOffTrig.BorderSize = 1;
            this.btnOffTrig.ButtonImage = null;
            this.btnOffTrig.Corner = BeeGlobal.Corner.None;
            this.btnOffTrig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOffTrig.FlatAppearance.BorderSize = 0;
            this.btnOffTrig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOffTrig.ForeColor = System.Drawing.Color.Black;
            this.btnOffTrig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOffTrig.IsCLick = false;
            this.btnOffTrig.IsNotChange = false;
            this.btnOffTrig.IsRect = false;
            this.btnOffTrig.IsUnGroup = false;
            this.btnOffTrig.Location = new System.Drawing.Point(266, 0);
            this.btnOffTrig.Margin = new System.Windows.Forms.Padding(0);
            this.btnOffTrig.Name = "btnOffTrig";
            this.btnOffTrig.Size = new System.Drawing.Size(114, 66);
            this.btnOffTrig.TabIndex = 32;
            this.btnOffTrig.Text = "OFF";
            this.btnOffTrig.TextColor = System.Drawing.Color.Black;
            this.btnOffTrig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOffTrig.UseVisualStyleBackColor = false;
            this.btnOffTrig.Click += new System.EventHandler(this.btnOffTrig_Click);
            // 
            // trackAngle
            // 
            this.trackAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackAngle.Location = new System.Drawing.Point(10, 38);
            this.trackAngle.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackAngle.Max = 100F;
            this.trackAngle.Min = 1F;
            this.trackAngle.Name = "trackAngle";
            this.trackAngle.Size = new System.Drawing.Size(373, 54);
            this.trackAngle.Step = 1F;
            this.trackAngle.TabIndex = 54;
            this.trackAngle.Value = 1F;
            this.trackAngle.ValueChanged += new System.Action<float>(this.trackAngle_ValueChanged);
            // 
            // trackMaxOverLap
            // 
            this.trackMaxOverLap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackMaxOverLap.Location = new System.Drawing.Point(10, 133);
            this.trackMaxOverLap.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.trackMaxOverLap.Max = 100F;
            this.trackMaxOverLap.Min = 0F;
            this.trackMaxOverLap.Name = "trackMaxOverLap";
            this.trackMaxOverLap.Size = new System.Drawing.Size(373, 54);
            this.trackMaxOverLap.Step = 1F;
            this.trackMaxOverLap.TabIndex = 55;
            this.trackMaxOverLap.Value = 0F;
            this.trackMaxOverLap.ValueChanged += new System.Action<float>(this.trackMaxOverLap_ValueChanged);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 892);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(400, 57);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolPosition_Adjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.label4);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ToolPosition_Adjustment";
            this.Size = new System.Drawing.Size(400, 949);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
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
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private RJButton btnTest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.PictureBox imgTemp;
        private RJButton btnLearning;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnOffTrig;
        private RJButton btnOnTrig;
        private CustomNumeric numDelay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton rjButton15;
        private RJButton btnCrop;
        private RJButton btnCropArea;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private CustomNumeric numOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private RJButton ckBitwiseNot;
        private RJButton ckSubPixel;
        private RJButton ckSIMD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private RJButton btnHighSpeed;
        private RJButton btnNormal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private GroupControl.OK_Cancel oK_Cancel1;
        private AdjustBar trackScore;
        private AdjustBar trackAngle;
        private AdjustBar trackMaxOverLap;
    }
}
