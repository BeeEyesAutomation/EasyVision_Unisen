using BeeCore;
using BeeGlobal;
using System.Windows.Forms;
namespace BeeInterface
{
    partial class ToolCircle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolCircle));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new  System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel15 = new  System.Windows.Forms.TableLayoutPanel();
            this.btnBinary = new BeeInterface.RJButton();
            this.btnStrongEdge = new BeeInterface.RJButton();
            this.btnCloseEdge = new BeeInterface.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new  System.Windows.Forms.TableLayoutPanel();
            this.btnOutsideIn = new BeeInterface.RJButton();
            this.btnInsideOut = new BeeInterface.RJButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.tableLayoutPanel3 = new  System.Windows.Forms.TableLayoutPanel();
            this.btnMask = new BeeInterface.RJButton();
            this.btnCropRect = new BeeInterface.RJButton();
            this.btnCropArea = new BeeInterface.RJButton();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new  System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.tableLayoutPanel5 = new  System.Windows.Forms.TableLayoutPanel();
            this.btnNone = new BeeInterface.RJButton();
            this.btnElip = new BeeInterface.RJButton();
            this.btnRect = new BeeInterface.RJButton();
            this.trackScore = new BeeInterface.AdjustBar();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new  System.Windows.Forms.TableLayoutPanel();
            this.btnCalib = new BeeInterface.RJButton();
            this.tableLayoutPanel14 = new  System.Windows.Forms.TableLayoutPanel();
            this.rjButton2 = new BeeInterface.RJButton();
            this.numScale = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel13 = new  System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new  System.Windows.Forms.TableLayoutPanel();
            this.numMinRadius = new BeeInterface.CustomNumeric();
            this.numMaxRadius = new BeeInterface.CustomNumeric();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackThreshold = new BeeInterface.AdjustBar();
            this.trackMinInlier = new BeeInterface.AdjustBar();
            this.trackIterations = new BeeInterface.AdjustBar();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
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
            this.tabControl2.Size = new System.Drawing.Size(500, 890);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(492, 852);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            this.tabP1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel15, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(486, 846);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel15.ColumnCount = 3;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.Controls.Add(this.btnBinary, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnStrongEdge, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.btnCloseEdge, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(5, 405);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(478, 60);
            this.tableLayoutPanel15.TabIndex = 62;
            // 
            // btnBinary
            // 
            this.btnBinary.AutoFont = false;
            this.btnBinary.BackColor = System.Drawing.Color.Transparent;
            this.btnBinary.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnBinary.BorderColor = System.Drawing.Color.Silver;
            this.btnBinary.BorderRadius = 10;
            this.btnBinary.BorderSize = 1;
            this.btnBinary.Corner = BeeGlobal.Corner.Right;
            this.btnBinary.DebounceResizeMs = 16;
            this.btnBinary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBinary.FlatAppearance.BorderSize = 0;
            this.btnBinary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBinary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBinary.ForeColor = System.Drawing.Color.Black;
            this.btnBinary.Image = null;
            this.btnBinary.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBinary.IsCLick = false;
            this.btnBinary.IsNotChange = false;
            this.btnBinary.IsRect = false;
            this.btnBinary.IsUnGroup = false;
            this.btnBinary.Location = new System.Drawing.Point(318, 0);
            this.btnBinary.Margin = new System.Windows.Forms.Padding(0);
            this.btnBinary.Name = "btnBinary";
            this.btnBinary.Size = new System.Drawing.Size(160, 60);
            this.btnBinary.TabIndex = 4;
            this.btnBinary.Text = "Binay";
            this.btnBinary.TextColor = System.Drawing.Color.Black;
            this.btnBinary.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBinary.UseVisualStyleBackColor = false;
            this.btnBinary.Click += new System.EventHandler(this.btnBinary_Click);
            // 
            // btnStrongEdge
            // 
            this.btnStrongEdge.AutoFont = false;
            this.btnStrongEdge.BackColor = System.Drawing.Color.Transparent;
            this.btnStrongEdge.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnStrongEdge.BorderColor = System.Drawing.Color.Transparent;
            this.btnStrongEdge.BorderRadius = 10;
            this.btnStrongEdge.BorderSize = 1;
            this.btnStrongEdge.Corner = BeeGlobal.Corner.None;
            this.btnStrongEdge.DebounceResizeMs = 16;
            this.btnStrongEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStrongEdge.FlatAppearance.BorderSize = 0;
            this.btnStrongEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStrongEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStrongEdge.ForeColor = System.Drawing.Color.Black;
            this.btnStrongEdge.Image = null;
            this.btnStrongEdge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStrongEdge.IsCLick = false;
            this.btnStrongEdge.IsNotChange = false;
            this.btnStrongEdge.IsRect = false;
            this.btnStrongEdge.IsUnGroup = false;
            this.btnStrongEdge.Location = new System.Drawing.Point(159, 0);
            this.btnStrongEdge.Margin = new System.Windows.Forms.Padding(0);
            this.btnStrongEdge.Name = "btnStrongEdge";
            this.btnStrongEdge.Size = new System.Drawing.Size(159, 60);
            this.btnStrongEdge.TabIndex = 3;
            this.btnStrongEdge.Text = "Strong Edge";
            this.btnStrongEdge.TextColor = System.Drawing.Color.Black;
            this.btnStrongEdge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStrongEdge.UseVisualStyleBackColor = false;
            this.btnStrongEdge.Click += new System.EventHandler(this.btnStrongEdge_Click);
            // 
            // btnCloseEdge
            // 
            this.btnCloseEdge.AutoFont = false;
            this.btnCloseEdge.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseEdge.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCloseEdge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseEdge.BorderColor = System.Drawing.Color.Transparent;
            this.btnCloseEdge.BorderRadius = 10;
            this.btnCloseEdge.BorderSize = 1;
            this.btnCloseEdge.Corner = BeeGlobal.Corner.Left;
            this.btnCloseEdge.DebounceResizeMs = 16;
            this.btnCloseEdge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseEdge.FlatAppearance.BorderSize = 0;
            this.btnCloseEdge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseEdge.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseEdge.ForeColor = System.Drawing.Color.Black;
            this.btnCloseEdge.Image = null;
            this.btnCloseEdge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseEdge.IsCLick = true;
            this.btnCloseEdge.IsNotChange = false;
            this.btnCloseEdge.IsRect = false;
            this.btnCloseEdge.IsUnGroup = false;
            this.btnCloseEdge.Location = new System.Drawing.Point(3, 0);
            this.btnCloseEdge.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCloseEdge.Name = "btnCloseEdge";
            this.btnCloseEdge.Size = new System.Drawing.Size(156, 60);
            this.btnCloseEdge.TabIndex = 2;
            this.btnCloseEdge.Text = "Close Edge";
            this.btnCloseEdge.TextColor = System.Drawing.Color.Black;
            this.btnCloseEdge.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseEdge.UseVisualStyleBackColor = false;
            this.btnCloseEdge.Click += new System.EventHandler(this.btnCloseEdge_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 377);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(478, 25);
            this.label2.TabIndex = 61;
            this.label2.Text = "Methord Get Edge";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnOutsideIn, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnInsideOut, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 312);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(478, 60);
            this.tableLayoutPanel6.TabIndex = 60;
            // 
            // btnOutsideIn
            // 
            this.btnOutsideIn.AutoFont = false;
            this.btnOutsideIn.BackColor = System.Drawing.Color.Transparent;
            this.btnOutsideIn.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnOutsideIn.BorderColor = System.Drawing.Color.Silver;
            this.btnOutsideIn.BorderRadius = 10;
            this.btnOutsideIn.BorderSize = 1;
            this.btnOutsideIn.Corner = BeeGlobal.Corner.Right;
            this.btnOutsideIn.DebounceResizeMs = 16;
            this.btnOutsideIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOutsideIn.FlatAppearance.BorderSize = 0;
            this.btnOutsideIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutsideIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOutsideIn.ForeColor = System.Drawing.Color.Black;
            this.btnOutsideIn.Image = null;
            this.btnOutsideIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOutsideIn.IsCLick = false;
            this.btnOutsideIn.IsNotChange = false;
            this.btnOutsideIn.IsRect = false;
            this.btnOutsideIn.IsUnGroup = false;
            this.btnOutsideIn.Location = new System.Drawing.Point(239, 0);
            this.btnOutsideIn.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnOutsideIn.Name = "btnOutsideIn";
            this.btnOutsideIn.Size = new System.Drawing.Size(236, 60);
            this.btnOutsideIn.TabIndex = 3;
            this.btnOutsideIn.Text = "Outside In";
            this.btnOutsideIn.TextColor = System.Drawing.Color.Black;
            this.btnOutsideIn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOutsideIn.UseVisualStyleBackColor = false;
            this.btnOutsideIn.Click += new System.EventHandler(this.btnOutsideIn_Click);
            // 
            // btnInsideOut
            // 
            this.btnInsideOut.AutoFont = false;
            this.btnInsideOut.BackColor = System.Drawing.Color.Transparent;
            this.btnInsideOut.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnInsideOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInsideOut.BorderColor = System.Drawing.Color.Transparent;
            this.btnInsideOut.BorderRadius = 10;
            this.btnInsideOut.BorderSize = 1;
            this.btnInsideOut.Corner = BeeGlobal.Corner.Left;
            this.btnInsideOut.DebounceResizeMs = 16;
            this.btnInsideOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInsideOut.FlatAppearance.BorderSize = 0;
            this.btnInsideOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInsideOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsideOut.ForeColor = System.Drawing.Color.Black;
            this.btnInsideOut.Image = null;
            this.btnInsideOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInsideOut.IsCLick = true;
            this.btnInsideOut.IsNotChange = false;
            this.btnInsideOut.IsRect = false;
            this.btnInsideOut.IsUnGroup = false;
            this.btnInsideOut.Location = new System.Drawing.Point(3, 0);
            this.btnInsideOut.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnInsideOut.Name = "btnInsideOut";
            this.btnInsideOut.Size = new System.Drawing.Size(236, 60);
            this.btnInsideOut.TabIndex = 2;
            this.btnInsideOut.Text = "Inside Out";
            this.btnInsideOut.TextColor = System.Drawing.Color.Black;
            this.btnInsideOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInsideOut.UseVisualStyleBackColor = false;
            this.btnInsideOut.Click += new System.EventHandler(this.btnInsideOut_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 284);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(478, 25);
            this.label3.TabIndex = 59;
            this.label3.Text = "Direction";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(5, 98);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(478, 25);
            this.label13.TabIndex = 58;
            this.label13.Text = "Choose Area";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(5, 191);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(478, 25);
            this.label12.TabIndex = 57;
            this.label12.Text = "Shape";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 470);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(478, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTest
            // 
            this.btnTest.AutoFont = false;
            this.btnTest.BackColor = System.Drawing.Color.Transparent;
            this.btnTest.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderRadius = 1;
            this.btnTest.BorderSize = 1;
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.DebounceResizeMs = 16;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.Image = null;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.IsCLick = false;
            this.btnTest.IsNotChange = true;
            this.btnTest.IsRect = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(20, 584);
            this.btnTest.Margin = new System.Windows.Forms.Padding(20);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(446, 242);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.Controls.Add(this.btnMask, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropRect, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropArea, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 126);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(478, 60);
            this.tableLayoutPanel3.TabIndex = 41;
            // 
            // btnMask
            // 
            this.btnMask.AutoFont = false;
            this.btnMask.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMask.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnMask.BorderColor = System.Drawing.Color.Silver;
            this.btnMask.BorderRadius = 10;
            this.btnMask.BorderSize = 1;
            this.btnMask.Corner = BeeGlobal.Corner.Right;
            this.btnMask.DebounceResizeMs = 16;
            this.btnMask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMask.FlatAppearance.BorderSize = 0;
            this.btnMask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMask.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMask.ForeColor = System.Drawing.Color.Black;
            this.btnMask.Image = null;
            this.btnMask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMask.IsCLick = false;
            this.btnMask.IsNotChange = false;
            this.btnMask.IsRect = false;
            this.btnMask.IsUnGroup = false;
            this.btnMask.Location = new System.Drawing.Point(322, 0);
            this.btnMask.Margin = new System.Windows.Forms.Padding(0);
            this.btnMask.Name = "btnMask";
            this.btnMask.Size = new System.Drawing.Size(156, 60);
            this.btnMask.TabIndex = 4;
            this.btnMask.Text = "Area Mask";
            this.btnMask.TextColor = System.Drawing.Color.Black;
            this.btnMask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMask.UseVisualStyleBackColor = false;
            this.btnMask.Click += new System.EventHandler(this.btnMask_Click);
            // 
            // btnCropRect
            // 
            this.btnCropRect.AutoFont = false;
            this.btnCropRect.BackColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropRect.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BorderRadius = 10;
            this.btnCropRect.BorderSize = 1;
            this.btnCropRect.Corner = BeeGlobal.Corner.Left;
            this.btnCropRect.DebounceResizeMs = 16;
            this.btnCropRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropRect.Enabled = false;
            this.btnCropRect.FlatAppearance.BorderSize = 0;
            this.btnCropRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropRect.ForeColor = System.Drawing.Color.Black;
            this.btnCropRect.Image = null;
            this.btnCropRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropRect.IsCLick = false;
            this.btnCropRect.IsNotChange = true;
            this.btnCropRect.IsRect = false;
            this.btnCropRect.IsUnGroup = false;
            this.btnCropRect.Location = new System.Drawing.Point(0, 0);
            this.btnCropRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropRect.Name = "btnCropRect";
            this.btnCropRect.Size = new System.Drawing.Size(154, 60);
            this.btnCropRect.TabIndex = 2;
            this.btnCropRect.Text = "Area Temp";
            this.btnCropRect.TextColor = System.Drawing.Color.Black;
            this.btnCropRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropRect.UseVisualStyleBackColor = false;
            this.btnCropRect.Click += new System.EventHandler(this.btnCropRect_Click);
            // 
            // btnCropArea
            // 
            this.btnCropArea.AutoFont = false;
            this.btnCropArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BorderColor = System.Drawing.Color.Silver;
            this.btnCropArea.BorderRadius = 5;
            this.btnCropArea.BorderSize = 1;
            this.btnCropArea.Corner = BeeGlobal.Corner.None;
            this.btnCropArea.DebounceResizeMs = 16;
            this.btnCropArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropArea.ForeColor = System.Drawing.Color.Black;
            this.btnCropArea.Image = null;
            this.btnCropArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropArea.IsCLick = true;
            this.btnCropArea.IsNotChange = false;
            this.btnCropArea.IsRect = false;
            this.btnCropArea.IsUnGroup = false;
            this.btnCropArea.Location = new System.Drawing.Point(154, 0);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(168, 60);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 25);
            this.label7.TabIndex = 38;
            this.label7.Text = "Search Range";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 33);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(478, 60);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // btnCropFull
            // 
            this.btnCropFull.AutoFont = false;
            this.btnCropFull.BackColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropFull.BorderColor = System.Drawing.Color.Silver;
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.DebounceResizeMs = 16;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropFull.ForeColor = System.Drawing.Color.Black;
            this.btnCropFull.Image = null;
            this.btnCropFull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropFull.IsCLick = false;
            this.btnCropFull.IsNotChange = false;
            this.btnCropFull.IsRect = false;
            this.btnCropFull.IsUnGroup = false;
            this.btnCropFull.Location = new System.Drawing.Point(239, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(236, 60);
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
            this.btnCropHalt.BackColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.DebounceResizeMs = 16;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropHalt.ForeColor = System.Drawing.Color.Black;
            this.btnCropHalt.Image = null;
            this.btnCropHalt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropHalt.IsCLick = true;
            this.btnCropHalt.IsNotChange = false;
            this.btnCropHalt.IsRect = false;
            this.btnCropHalt.IsUnGroup = false;
            this.btnCropHalt.Location = new System.Drawing.Point(3, 0);
            this.btnCropHalt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnCropHalt.Name = "btnCropHalt";
            this.btnCropHalt.Size = new System.Drawing.Size(236, 60);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.btnNone, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnElip, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnRect, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 219);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(476, 60);
            this.tableLayoutPanel5.TabIndex = 54;
            // 
            // btnNone
            // 
            this.btnNone.AutoFont = false;
            this.btnNone.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNone.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnNone.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNone.BackgroundImage")));
            this.btnNone.BorderColor = System.Drawing.Color.Silver;
            this.btnNone.BorderRadius = 10;
            this.btnNone.BorderSize = 1;
            this.btnNone.Corner = BeeGlobal.Corner.Right;
            this.btnNone.DebounceResizeMs = 16;
            this.btnNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNone.FlatAppearance.BorderSize = 0;
            this.btnNone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNone.ForeColor = System.Drawing.Color.Black;
            this.btnNone.Image = null;
            this.btnNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNone.IsCLick = false;
            this.btnNone.IsNotChange = false;
            this.btnNone.IsRect = false;
            this.btnNone.IsUnGroup = false;
            this.btnNone.Location = new System.Drawing.Point(316, 0);
            this.btnNone.Margin = new System.Windows.Forms.Padding(0);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(160, 60);
            this.btnNone.TabIndex = 5;
            this.btnNone.Text = "None";
            this.btnNone.TextColor = System.Drawing.Color.Black;
            this.btnNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNone.UseVisualStyleBackColor = false;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnElip
            // 
            this.btnElip.AutoFont = false;
            this.btnElip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnElip.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnElip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnElip.BackgroundImage")));
            this.btnElip.BorderColor = System.Drawing.Color.Silver;
            this.btnElip.BorderRadius = 10;
            this.btnElip.BorderSize = 1;
            this.btnElip.Corner = BeeGlobal.Corner.None;
            this.btnElip.DebounceResizeMs = 16;
            this.btnElip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnElip.FlatAppearance.BorderSize = 0;
            this.btnElip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnElip.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnElip.ForeColor = System.Drawing.Color.Black;
            this.btnElip.Image = null;
            this.btnElip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnElip.IsCLick = false;
            this.btnElip.IsNotChange = false;
            this.btnElip.IsRect = false;
            this.btnElip.IsUnGroup = false;
            this.btnElip.Location = new System.Drawing.Point(158, 0);
            this.btnElip.Margin = new System.Windows.Forms.Padding(0);
            this.btnElip.Name = "btnElip";
            this.btnElip.Size = new System.Drawing.Size(158, 60);
            this.btnElip.TabIndex = 3;
            this.btnElip.Text = "Elip";
            this.btnElip.TextColor = System.Drawing.Color.Black;
            this.btnElip.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnElip.UseVisualStyleBackColor = false;
            this.btnElip.Click += new System.EventHandler(this.btnElip_Click);
            // 
            // btnRect
            // 
            this.btnRect.AutoFont = false;
            this.btnRect.BackColor = System.Drawing.Color.Transparent;
            this.btnRect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnRect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRect.BackgroundImage")));
            this.btnRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRect.BorderColor = System.Drawing.Color.Transparent;
            this.btnRect.BorderRadius = 10;
            this.btnRect.BorderSize = 1;
            this.btnRect.Corner = BeeGlobal.Corner.Left;
            this.btnRect.DebounceResizeMs = 16;
            this.btnRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRect.FlatAppearance.BorderSize = 0;
            this.btnRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRect.ForeColor = System.Drawing.Color.Black;
            this.btnRect.Image = null;
            this.btnRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRect.IsCLick = true;
            this.btnRect.IsNotChange = false;
            this.btnRect.IsRect = false;
            this.btnRect.IsUnGroup = false;
            this.btnRect.Location = new System.Drawing.Point(0, 0);
            this.btnRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(158, 60);
            this.btnRect.TabIndex = 4;
            this.btnRect.Text = "Rectangle";
            this.btnRect.TextColor = System.Drawing.Color.Black;
            this.btnRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRect.UseVisualStyleBackColor = false;
            this.btnRect.Click += new System.EventHandler(this.btnRect_Click);
            // 
            // trackScore
            // 
            this.trackScore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackScore.Location = new System.Drawing.Point(3, 501);
            this.trackScore.Max = 100F;
            this.trackScore.Min = 0F;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(480, 60);
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 63;
            this.trackScore.Value = 0F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(492, 852);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.btnCalib, 0, 9);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 8);
            this.tableLayoutPanel8.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel4, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.trackThreshold, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.trackMinInlier, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.trackIterations, 0, 5);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 11;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(486, 846);
            this.tableLayoutPanel8.TabIndex = 0;
            this.tableLayoutPanel8.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel8_Paint);
            // 
            // btnCalib
            // 
            this.btnCalib.AutoFont = false;
            this.btnCalib.BackColor = System.Drawing.Color.Transparent;
            this.btnCalib.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCalib.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalib.BorderColor = System.Drawing.Color.Transparent;
            this.btnCalib.BorderRadius = 10;
            this.btnCalib.BorderSize = 1;
            this.btnCalib.Corner = BeeGlobal.Corner.Left;
            this.btnCalib.DebounceResizeMs = 16;
            this.btnCalib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalib.FlatAppearance.BorderSize = 0;
            this.btnCalib.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalib.ForeColor = System.Drawing.Color.Black;
            this.btnCalib.Image = null;
            this.btnCalib.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCalib.IsCLick = false;
            this.btnCalib.IsNotChange = false;
            this.btnCalib.IsRect = false;
            this.btnCalib.IsUnGroup = false;
            this.btnCalib.Location = new System.Drawing.Point(3, 514);
            this.btnCalib.Margin = new System.Windows.Forms.Padding(3, 20, 0, 0);
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(483, 73);
            this.btnCalib.TabIndex = 62;
            this.btnCalib.Text = "Auto Calib";
            this.btnCalib.TextColor = System.Drawing.Color.Black;
            this.btnCalib.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCalib.UseVisualStyleBackColor = false;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel14.ColumnCount = 2;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.Controls.Add(this.rjButton2, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.numScale, 1, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 439);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(5, 15, 3, 0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(478, 55);
            this.tableLayoutPanel14.TabIndex = 61;
            // 
            // rjButton2
            // 
            this.rjButton2.AutoFont = false;
            this.rjButton2.BackColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 10;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.Corner = BeeGlobal.Corner.Left;
            this.rjButton2.DebounceResizeMs = 16;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.Enabled = false;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton2.ForeColor = System.Drawing.Color.White;
            this.rjButton2.Image = null;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = false;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(3, 0);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(236, 55);
            this.rjButton2.TabIndex = 2;
            this.rjButton2.Text = "Scale";
            this.rjButton2.TextColor = System.Drawing.Color.White;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // numScale
            // 
            this.numScale.DecimalPlaces = 3;
            this.numScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numScale.Location = new System.Drawing.Point(242, 3);
            this.numScale.Name = "numScale";
            this.numScale.Size = new System.Drawing.Size(233, 47);
            this.numScale.TabIndex = 47;
            this.numScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numScale.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 220);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 25);
            this.label1.TabIndex = 59;
            this.label1.Text = "Iterations";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.label9, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(5, 320);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 5);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Size = new System.Drawing.Size(478, 46);
            this.tableLayoutPanel13.TabIndex = 56;
            // 
            // label9
            // 
            this.label9.AutoEllipsis = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label9.Location = new System.Drawing.Point(244, 10);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(231, 36);
            this.label9.TabIndex = 52;
            this.label9.Text = "Max Radius";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label5.Location = new System.Drawing.Point(5, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(231, 36);
            this.label5.TabIndex = 51;
            this.label5.Text = "Min Radius";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel4.Controls.Add(this.numMinRadius, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.numMaxRadius, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 376);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(478, 48);
            this.tableLayoutPanel4.TabIndex = 55;
            // 
            // numMinRadius
            // 
            this.numMinRadius.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numMinRadius.BackColor = System.Drawing.Color.Transparent;
            this.numMinRadius.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numMinRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMinRadius.ForeColor = System.Drawing.Color.Red;
            this.numMinRadius.Location = new System.Drawing.Point(10, 0);
            this.numMinRadius.Margin = new System.Windows.Forms.Padding(0);
            this.numMinRadius.Maxnimum = 1000F;
            this.numMinRadius.Minimum = 0F;
            this.numMinRadius.Name = "numMinRadius";
            this.numMinRadius.Size = new System.Drawing.Size(219, 48);
            this.numMinRadius.Step = 1F;
            this.numMinRadius.TabIndex = 38;
            this.numMinRadius.Value = 100F;
            this.numMinRadius.ValueChanged += new System.EventHandler(this.numMinRadius_ValueChanged);
            // 
            // numMaxRadius
            // 
            this.numMaxRadius.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numMaxRadius.BackColor = System.Drawing.Color.Transparent;
            this.numMaxRadius.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numMaxRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMaxRadius.ForeColor = System.Drawing.Color.Red;
            this.numMaxRadius.Location = new System.Drawing.Point(249, 0);
            this.numMaxRadius.Margin = new System.Windows.Forms.Padding(0);
            this.numMaxRadius.Maxnimum = 1000F;
            this.numMaxRadius.Minimum = 0F;
            this.numMaxRadius.Name = "numMaxRadius";
            this.numMaxRadius.Size = new System.Drawing.Size(219, 48);
            this.numMaxRadius.Step = 1F;
            this.numMaxRadius.TabIndex = 35;
            this.numMaxRadius.Value = 100F;
            this.numMaxRadius.ValueChanged += new System.EventHandler(this.numMaxRadius_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 115);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 25);
            this.label6.TabIndex = 48;
            this.label6.Text = "Min Inliers";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(478, 25);
            this.label4.TabIndex = 39;
            this.label4.Text = "Threshold";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackThreshold
            // 
            this.trackThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackThreshold.Location = new System.Drawing.Point(3, 38);
            this.trackThreshold.Max = 4F;
            this.trackThreshold.Min = 0.1F;
            this.trackThreshold.Name = "trackThreshold";
            this.trackThreshold.Size = new System.Drawing.Size(480, 64);
            this.trackThreshold.Step = 0.1F;
            this.trackThreshold.TabIndex = 63;
            this.trackThreshold.Value = 0.1F;
            this.trackThreshold.ValueChanged += new System.Action<float>(this.trackThreshold_ValueChanged);
            // 
            // trackMinInlier
            // 
            this.trackMinInlier.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackMinInlier.Location = new System.Drawing.Point(3, 143);
            this.trackMinInlier.Max = 10000F;
            this.trackMinInlier.Min = 1F;
            this.trackMinInlier.Name = "trackMinInlier";
            this.trackMinInlier.Size = new System.Drawing.Size(480, 64);
            this.trackMinInlier.Step = 1F;
            this.trackMinInlier.TabIndex = 64;
            this.trackMinInlier.Value = 2F;
            this.trackMinInlier.ValueChanged += new System.Action<float>(this.trackMinInlier_ValueChanged);
            // 
            // trackIterations
            // 
            this.trackIterations.Location = new System.Drawing.Point(3, 248);
            this.trackIterations.Max = 5000F;
            this.trackIterations.Min = 1F;
            this.trackIterations.Name = "trackIterations";
            this.trackIterations.Size = new System.Drawing.Size(380, 55);
            this.trackIterations.Step = 1F;
            this.trackIterations.TabIndex = 65;
            this.trackIterations.Value = 1F;
            this.trackIterations.ValueChanged += new System.Action<float>(this.trackIterations_ValueChanged);
            this.trackIterations.Load += new System.EventHandler(this.trackIterations_Load);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.BackColor = System.Drawing.Color.Transparent;
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 877);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(500, 62);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolCircle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.tabControl2);
            this.Name = "ToolCircle";
            this.Size = new System.Drawing.Size(500, 939);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TabPage tabPage4;
        public System.ComponentModel.BackgroundWorker threadProcess;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private CustomNumeric numMinRadius;
        private CustomNumeric numMaxRadius;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private GroupControl.OK_Cancel oK_Cancel1;
        private System.Windows.Forms.NumericUpDown numScale;
        private System.Windows.Forms.Label label1;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private RJButton rjButton2;
        private RJButton btnCalib;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private RJButton btnStrongEdge;
        private RJButton btnCloseEdge;
        private System.Windows.Forms.Label label2;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnOutsideIn;
        private RJButton btnInsideOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private RJButton btnTest;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnMask;
        private RJButton btnCropRect;
        private RJButton btnCropArea;
        private System.Windows.Forms.Label label7;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnNone;
        private RJButton btnElip;
        private RJButton btnRect;
        private RJButton btnBinary;
        private AdjustBar trackScore;
        private AdjustBar trackThreshold;
        private AdjustBar trackMinInlier;
        private AdjustBar trackIterations;
    }
}
