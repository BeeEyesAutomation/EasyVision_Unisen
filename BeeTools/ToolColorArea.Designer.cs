using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolColorArea
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolColorArea));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRGB = new BeeInterface.RJButton();
            this.btnHSV = new BeeInterface.RJButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.picColor = new System.Windows.Forms.PictureBox();
            this.btnGetColor = new BeeInterface.RJButton();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnInspect = new BeeInterface.RJButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMask = new BeeInterface.RJButton();
            this.btnCropRect = new BeeInterface.RJButton();
            this.btnCropArea = new BeeInterface.RJButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNone = new BeeInterface.RJButton();
            this.btnElip = new BeeInterface.RJButton();
            this.btnRect = new BeeInterface.RJButton();
            this.trackScore = new BeeInterface.AdjustBar();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.trackPixel = new BeeInterface.AdjustBar();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(400, 828);
            this.tabControl2.TabIndex = 17;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel1);
            this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage3.Location = new System.Drawing.Point(4, 38);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(392, 786);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Basic";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.btnInspect, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 9);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.46512F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.46512F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.46512F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.46512F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.13953F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 780);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 277);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 25);
            this.label2.TabIndex = 61;
            this.label2.Text = "Extract color";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.btnRGB, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnHSV, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 237);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(378, 35);
            this.tableLayoutPanel6.TabIndex = 60;
            // 
            // btnRGB
            // 
            this.btnRGB.BackColor = System.Drawing.Color.Transparent;
            this.btnRGB.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnRGB.BorderColor = System.Drawing.Color.Silver;
            this.btnRGB.BorderRadius = 10;
            this.btnRGB.BorderSize = 1;
            this.btnRGB.ButtonImage = null;
            this.btnRGB.Corner = BeeGlobal.Corner.Right;
            this.btnRGB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRGB.FlatAppearance.BorderSize = 0;
            this.btnRGB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRGB.ForeColor = System.Drawing.Color.Black;
            this.btnRGB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRGB.IsCLick = false;
            this.btnRGB.IsNotChange = false;
            this.btnRGB.IsRect = false;
            this.btnRGB.IsUnGroup = false;
            this.btnRGB.Location = new System.Drawing.Point(189, 0);
            this.btnRGB.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnRGB.Name = "btnRGB";
            this.btnRGB.Size = new System.Drawing.Size(186, 35);
            this.btnRGB.TabIndex = 3;
            this.btnRGB.Text = "RGB";
            this.btnRGB.TextColor = System.Drawing.Color.Black;
            this.btnRGB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRGB.UseVisualStyleBackColor = false;
            this.btnRGB.Click += new System.EventHandler(this.btnRGB_Click);
            // 
            // btnHSV
            // 
            this.btnHSV.BackColor = System.Drawing.Color.Transparent;
            this.btnHSV.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnHSV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHSV.BorderColor = System.Drawing.Color.Transparent;
            this.btnHSV.BorderRadius = 10;
            this.btnHSV.BorderSize = 1;
            this.btnHSV.ButtonImage = null;
            this.btnHSV.Corner = BeeGlobal.Corner.Left;
            this.btnHSV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHSV.FlatAppearance.BorderSize = 0;
            this.btnHSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHSV.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHSV.ForeColor = System.Drawing.Color.Black;
            this.btnHSV.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHSV.IsCLick = true;
            this.btnHSV.IsNotChange = false;
            this.btnHSV.IsRect = false;
            this.btnHSV.IsUnGroup = false;
            this.btnHSV.Location = new System.Drawing.Point(3, 0);
            this.btnHSV.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnHSV.Name = "btnHSV";
            this.btnHSV.Size = new System.Drawing.Size(186, 35);
            this.btnHSV.TabIndex = 2;
            this.btnHSV.Text = "HSV";
            this.btnHSV.TextColor = System.Drawing.Color.Black;
            this.btnHSV.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHSV.UseVisualStyleBackColor = false;
            this.btnHSV.Click += new System.EventHandler(this.btnHSV_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 209);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(378, 25);
            this.label3.TabIndex = 59;
            this.label3.Text = "Color Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 20);
            this.label7.TabIndex = 31;
            this.label7.Text = "Extraction";
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Image = global::BeeInterface.Properties.Resources.Delete;
            this.btnDeleteAll.Location = new System.Drawing.Point(329, 3);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(34, 27);
            this.btnDeleteAll.TabIndex = 35;
            this.btnDeleteAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedo.BackgroundImage = global::BeeInterface.Properties.Resources.Redo;
            this.btnRedo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRedo.Location = new System.Drawing.Point(289, 3);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(34, 27);
            this.btnRedo.TabIndex = 34;
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.BackgroundImage = global::BeeInterface.Properties.Resources.Undo_3;
            this.btnUndo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUndo.Location = new System.Drawing.Point(249, 3);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(34, 27);
            this.btnUndo.TabIndex = 33;
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // picColor
            // 
            this.picColor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picColor.Location = new System.Drawing.Point(123, 3);
            this.picColor.Name = "picColor";
            this.picColor.Size = new System.Drawing.Size(246, 54);
            this.picColor.TabIndex = 31;
            this.picColor.TabStop = false;
            this.picColor.Click += new System.EventHandler(this.picColor_Click);
            this.picColor.Paint += new System.Windows.Forms.PaintEventHandler(this.picColor_Paint);
            // 
            // btnGetColor
            // 
            this.btnGetColor.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnGetColor.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnGetColor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGetColor.BackgroundImage")));
            this.btnGetColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGetColor.BorderColor = System.Drawing.Color.Silver;
            this.btnGetColor.BorderRadius = 5;
            this.btnGetColor.BorderSize = 1;
            this.btnGetColor.ButtonImage = null;
            this.btnGetColor.Corner = BeeGlobal.Corner.Both;
            this.btnGetColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetColor.FlatAppearance.BorderSize = 0;
            this.btnGetColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetColor.ForeColor = System.Drawing.Color.Black;
            this.btnGetColor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetColor.IsCLick = false;
            this.btnGetColor.IsNotChange = false;
            this.btnGetColor.IsRect = false;
            this.btnGetColor.IsUnGroup = false;
            this.btnGetColor.Location = new System.Drawing.Point(3, 3);
            this.btnGetColor.Name = "btnGetColor";
            this.btnGetColor.Size = new System.Drawing.Size(114, 54);
            this.btnGetColor.TabIndex = 4;
            this.btnGetColor.Text = "Get Color";
            this.btnGetColor.TextColor = System.Drawing.Color.Black;
            this.btnGetColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnGetColor.UseVisualStyleBackColor = false;
            this.btnGetColor.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(5, 73);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(378, 25);
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
            this.label12.Location = new System.Drawing.Point(5, 141);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(378, 25);
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
            this.label8.Location = new System.Drawing.Point(5, 490);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(378, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnInspect
            // 
            this.btnInspect.BackColor = System.Drawing.Color.Transparent;
            this.btnInspect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnInspect.BorderColor = System.Drawing.Color.Transparent;
            this.btnInspect.BorderRadius = 1;
            this.btnInspect.BorderSize = 1;
            this.btnInspect.ButtonImage = null;
            this.btnInspect.Corner = BeeGlobal.Corner.Both;
            this.btnInspect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInspect.FlatAppearance.BorderSize = 0;
            this.btnInspect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInspect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInspect.ForeColor = System.Drawing.Color.Black;
            this.btnInspect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInspect.IsCLick = false;
            this.btnInspect.IsNotChange = true;
            this.btnInspect.IsRect = false;
            this.btnInspect.IsUnGroup = true;
            this.btnInspect.Location = new System.Drawing.Point(20, 604);
            this.btnInspect.Margin = new System.Windows.Forms.Padding(20);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(346, 156);
            this.btnInspect.TabIndex = 37;
            this.btnInspect.Text = "Inspect";
            this.btnInspect.TextColor = System.Drawing.Color.Black;
            this.btnInspect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInspect.UseVisualStyleBackColor = false;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 101);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(378, 35);
            this.tableLayoutPanel3.TabIndex = 41;
            // 
            // btnMask
            // 
            this.btnMask.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMask.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnMask.BorderColor = System.Drawing.Color.Silver;
            this.btnMask.BorderRadius = 10;
            this.btnMask.BorderSize = 1;
            this.btnMask.ButtonImage = null;
            this.btnMask.Corner = BeeGlobal.Corner.Right;
            this.btnMask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMask.FlatAppearance.BorderSize = 0;
            this.btnMask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMask.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMask.ForeColor = System.Drawing.Color.Black;
            this.btnMask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMask.IsCLick = false;
            this.btnMask.IsNotChange = false;
            this.btnMask.IsRect = false;
            this.btnMask.IsUnGroup = false;
            this.btnMask.Location = new System.Drawing.Point(255, 0);
            this.btnMask.Margin = new System.Windows.Forms.Padding(0);
            this.btnMask.Name = "btnMask";
            this.btnMask.Size = new System.Drawing.Size(123, 35);
            this.btnMask.TabIndex = 4;
            this.btnMask.Text = "Area Mask";
            this.btnMask.TextColor = System.Drawing.Color.Black;
            this.btnMask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMask.UseVisualStyleBackColor = false;
            this.btnMask.Click += new System.EventHandler(this.btnMask_Click);
            // 
            // btnCropRect
            // 
            this.btnCropRect.BackColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropRect.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BorderRadius = 10;
            this.btnCropRect.BorderSize = 1;
            this.btnCropRect.ButtonImage = null;
            this.btnCropRect.Corner = BeeGlobal.Corner.Left;
            this.btnCropRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropRect.Enabled = false;
            this.btnCropRect.FlatAppearance.BorderSize = 0;
            this.btnCropRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropRect.ForeColor = System.Drawing.Color.Black;
            this.btnCropRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropRect.IsCLick = false;
            this.btnCropRect.IsNotChange = true;
            this.btnCropRect.IsRect = false;
            this.btnCropRect.IsUnGroup = false;
            this.btnCropRect.Location = new System.Drawing.Point(0, 0);
            this.btnCropRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropRect.Name = "btnCropRect";
            this.btnCropRect.Size = new System.Drawing.Size(122, 35);
            this.btnCropRect.TabIndex = 2;
            this.btnCropRect.Text = "Area Temp";
            this.btnCropRect.TextColor = System.Drawing.Color.Black;
            this.btnCropRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropRect.UseVisualStyleBackColor = false;
            this.btnCropRect.Click += new System.EventHandler(this.btnCropRect_Click);
            // 
            // btnCropArea
            // 
            this.btnCropArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundColor = System.Drawing.Color.WhiteSmoke;
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
            this.btnCropArea.Location = new System.Drawing.Point(122, 0);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(133, 35);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 25);
            this.label1.TabIndex = 38;
            this.label1.Text = "Search Range";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 35);
            this.tableLayoutPanel2.TabIndex = 39;
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
            this.btnCropFull.Location = new System.Drawing.Point(189, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(186, 35);
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
            this.btnCropHalt.Size = new System.Drawing.Size(186, 35);
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
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 169);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(376, 35);
            this.tableLayoutPanel5.TabIndex = 54;
            // 
            // btnNone
            // 
            this.btnNone.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNone.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnNone.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNone.BackgroundImage")));
            this.btnNone.BorderColor = System.Drawing.Color.Silver;
            this.btnNone.BorderRadius = 10;
            this.btnNone.BorderSize = 1;
            this.btnNone.ButtonImage = null;
            this.btnNone.Corner = BeeGlobal.Corner.Right;
            this.btnNone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNone.FlatAppearance.BorderSize = 0;
            this.btnNone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNone.ForeColor = System.Drawing.Color.Black;
            this.btnNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNone.IsCLick = false;
            this.btnNone.IsNotChange = false;
            this.btnNone.IsRect = false;
            this.btnNone.IsUnGroup = false;
            this.btnNone.Location = new System.Drawing.Point(250, 0);
            this.btnNone.Margin = new System.Windows.Forms.Padding(0);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(126, 35);
            this.btnNone.TabIndex = 5;
            this.btnNone.Text = "None";
            this.btnNone.TextColor = System.Drawing.Color.Black;
            this.btnNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNone.UseVisualStyleBackColor = false;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // btnElip
            // 
            this.btnElip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnElip.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnElip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnElip.BackgroundImage")));
            this.btnElip.BorderColor = System.Drawing.Color.Silver;
            this.btnElip.BorderRadius = 10;
            this.btnElip.BorderSize = 1;
            this.btnElip.ButtonImage = null;
            this.btnElip.Corner = BeeGlobal.Corner.None;
            this.btnElip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnElip.FlatAppearance.BorderSize = 0;
            this.btnElip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnElip.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnElip.ForeColor = System.Drawing.Color.Black;
            this.btnElip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnElip.IsCLick = false;
            this.btnElip.IsNotChange = false;
            this.btnElip.IsRect = false;
            this.btnElip.IsUnGroup = false;
            this.btnElip.Location = new System.Drawing.Point(125, 0);
            this.btnElip.Margin = new System.Windows.Forms.Padding(0);
            this.btnElip.Name = "btnElip";
            this.btnElip.Size = new System.Drawing.Size(125, 35);
            this.btnElip.TabIndex = 3;
            this.btnElip.Text = "Elip";
            this.btnElip.TextColor = System.Drawing.Color.Black;
            this.btnElip.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnElip.UseVisualStyleBackColor = false;
            this.btnElip.Click += new System.EventHandler(this.btnElip_Click);
            // 
            // btnRect
            // 
            this.btnRect.BackColor = System.Drawing.Color.Transparent;
            this.btnRect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnRect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRect.BackgroundImage")));
            this.btnRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRect.BorderColor = System.Drawing.Color.Transparent;
            this.btnRect.BorderRadius = 10;
            this.btnRect.BorderSize = 1;
            this.btnRect.ButtonImage = null;
            this.btnRect.Corner = BeeGlobal.Corner.Left;
            this.btnRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRect.FlatAppearance.BorderSize = 0;
            this.btnRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRect.ForeColor = System.Drawing.Color.Black;
            this.btnRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRect.IsCLick = true;
            this.btnRect.IsNotChange = false;
            this.btnRect.IsRect = false;
            this.btnRect.IsUnGroup = false;
            this.btnRect.Location = new System.Drawing.Point(0, 0);
            this.btnRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(125, 35);
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
            this.trackScore.Location = new System.Drawing.Point(3, 521);
            this.trackScore.Max = 100F;
            this.trackScore.Min = 0F;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(380, 60);
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 63;
            this.trackScore.Value = 0F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel6);
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 786);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Location = new System.Drawing.Point(6, 6);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(364, 48);
            this.panel6.TabIndex = 27;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 20);
            this.label10.TabIndex = 25;
            this.label10.Text = "Add Area Mask";
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 827);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(400, 52);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel8, 0, 2);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 308);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(380, 174);
            this.tableLayoutPanel4.TabIndex = 64;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.btnGetColor, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.picColor, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(5, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(372, 60);
            this.tableLayoutPanel7.TabIndex = 42;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.trackPixel, 0, 1);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(374, 108);
            this.tableLayoutPanel8.TabIndex = 43;
            // 
            // trackPixel
            // 
            this.trackPixel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackPixel.Location = new System.Drawing.Point(3, 36);
            this.trackPixel.Max = 100F;
            this.trackPixel.Min = 1F;
            this.trackPixel.Name = "trackPixel";
            this.trackPixel.Size = new System.Drawing.Size(368, 69);
            this.trackPixel.Step = 1F;
            this.trackPixel.TabIndex = 64;
            this.trackPixel.Value = 1F;
            this.trackPixel.ValueChanged += new System.Action<float>(this.trackPixel_ValueChanged);
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 4;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.Controls.Add(this.btnDeleteAll, 3, 0);
            this.tableLayoutPanel9.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.btnRedo, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.btnUndo, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 0);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(366, 33);
            this.tableLayoutPanel9.TabIndex = 65;
            // 
            // ToolColorArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.tabControl2);
            this.Name = "ToolColorArea";
            this.Size = new System.Drawing.Size(400, 879);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker threadProcess;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label10;
   
        public RJButton btnGetColor;
        public System.Windows.Forms.PictureBox picColor;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.Label label7;
        private GroupControl.OK_Cancel oK_Cancel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton btnRGB;
        private RJButton btnHSV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private RJButton btnInspect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnMask;
        private RJButton btnCropRect;
        private RJButton btnCropArea;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnNone;
        private RJButton btnElip;
        private RJButton btnRect;
        private AdjustBar trackScore;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private AdjustBar trackPixel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
    }
}
