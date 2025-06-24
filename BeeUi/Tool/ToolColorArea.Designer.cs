using BeeCore;

namespace BeeUi.Tool
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
            this.label4 = new System.Windows.Forms.Label();
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.trackScore = new BeeUi.TrackBar2();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnClWhite = new BeeUi.Common.RJButton();
            this.btnColor = new BeeUi.Common.RJButton();
            this.label6 = new System.Windows.Forms.Label();
            this.pArea = new System.Windows.Forms.Panel();
            this.btnCropArea = new BeeUi.Common.RJButton();
            this.btnCropRect = new BeeUi.Common.RJButton();
            this.pMode = new System.Windows.Forms.Panel();
            this.trackPixel = new BeeUi.TrackBar2();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.picColor = new System.Windows.Forms.PictureBox();
            this.btnGetColor = new BeeUi.Common.RJButton();
            this.rjButton3 = new BeeUi.Common.RJButton();
            this.rjButton1 = new BeeUi.Common.RJButton();
            this.btnCancel = new BeeUi.Common.RJButton();
            this.rjButton5 = new BeeUi.Common.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.btnMask = new BeeUi.Common.RJButton();
            this.btnTest = new BeeUi.Common.RJButton();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pArea.SuspendLayout();
            this.pMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 368);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Score";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Chọn biên dạng ";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(400, 646);
            this.tabControl2.TabIndex = 17;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnTest);
            this.tabPage3.Controls.Add(this.trackScore);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.pArea);
            this.tabPage3.Controls.Add(this.pMode);
            this.tabPage3.Controls.Add(this.rjButton3);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.rjButton1);
            this.tabPage3.Controls.Add(this.btnCancel);
            this.tabPage3.Controls.Add(this.rjButton5);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(392, 613);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Căn bản";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // trackScore
            // 
            this.trackScore.ColorTrack = System.Drawing.Color.Gray;
            this.trackScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackScore.Location = new System.Drawing.Point(16, 393);
            this.trackScore.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackScore.Max = 100;
            this.trackScore.Min = 0;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(360, 45);
            this.trackScore.Step = 1;
            this.trackScore.TabIndex = 30;
            this.trackScore.Value = 0;
            this.trackScore.ValueScore = 0;
            this.trackScore.ValueChanged +=  new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 29;
            this.label1.Text = "Color Type";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClWhite);
            this.panel4.Controls.Add(this.btnColor);
            this.panel4.Location = new System.Drawing.Point(12, 183);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(364, 48);
            this.panel4.TabIndex = 28;
            // 
            // btnClWhite
            // 
            this.btnClWhite.BackColor = System.Drawing.Color.Transparent;
            this.btnClWhite.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnClWhite.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClWhite.BackgroundImage")));
            this.btnClWhite.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClWhite.BorderColor = System.Drawing.Color.Transparent;
            this.btnClWhite.BorderRadius = 5;
            this.btnClWhite.BorderSize = 1;
            this.btnClWhite.ButtonImage = null;
            this.btnClWhite.Corner = BeeCore.Corner.Both;
            this.btnClWhite.FlatAppearance.BorderSize = 0;
            this.btnClWhite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClWhite.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClWhite.ForeColor = System.Drawing.Color.Black;
            this.btnClWhite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClWhite.IsCLick = false;
            this.btnClWhite.IsNotChange = false;
            this.btnClWhite.IsRect = false;
            this.btnClWhite.IsUnGroup = false;
            this.btnClWhite.Location = new System.Drawing.Point(179, 3);
            this.btnClWhite.Name = "btnClWhite";
            this.btnClWhite.Size = new System.Drawing.Size(176, 40);
            this.btnClWhite.TabIndex = 4;
            this.btnClWhite.Text = "RGB";
            this.btnClWhite.TextColor = System.Drawing.Color.Black;
            this.btnClWhite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClWhite.UseVisualStyleBackColor = false;
            this.btnClWhite.Click += new System.EventHandler(this.btnClWhite_Click);
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.Transparent;
            this.btnColor.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnColor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnColor.BackgroundImage")));
            this.btnColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnColor.BorderColor = System.Drawing.Color.Transparent;
            this.btnColor.BorderRadius = 5;
            this.btnColor.BorderSize = 1;
            this.btnColor.ButtonImage = null;
            this.btnColor.Corner = BeeCore.Corner.Both;
            this.btnColor.FlatAppearance.BorderSize = 0;
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColor.ForeColor = System.Drawing.Color.Black;
            this.btnColor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnColor.IsCLick = true;
            this.btnColor.IsNotChange = false;
            this.btnColor.IsRect = false;
            this.btnColor.IsUnGroup = false;
            this.btnColor.Location = new System.Drawing.Point(2, 3);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(177, 40);
            this.btnColor.TabIndex = 2;
            this.btnColor.Text = "HSV";
            this.btnColor.TextColor = System.Drawing.Color.Black;
            this.btnColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 20);
            this.label6.TabIndex = 25;
            this.label6.Text = "Search Range";
            // 
            // pArea
            // 
            this.pArea.Controls.Add(this.btnCropArea);
            this.pArea.Controls.Add(this.btnCropRect);
            this.pArea.Enabled = false;
            this.pArea.Location = new System.Drawing.Point(12, 105);
            this.pArea.Name = "pArea";
            this.pArea.Size = new System.Drawing.Size(364, 48);
            this.pArea.TabIndex = 26;
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
            this.btnCropArea.Corner = BeeCore.Corner.Both;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropArea.ForeColor = System.Drawing.Color.Black;
            this.btnCropArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropArea.IsCLick = false;
            this.btnCropArea.IsNotChange = false;
            this.btnCropArea.IsRect = false;
            this.btnCropArea.IsUnGroup = false;
            this.btnCropArea.Location = new System.Drawing.Point(179, 3);
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(175, 40);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Partial";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click_1);
            // 
            // btnCropRect
            // 
            this.btnCropRect.BackColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropRect.BackgroundImage")));
            this.btnCropRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropRect.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropRect.BorderRadius = 5;
            this.btnCropRect.BorderSize = 1;
            this.btnCropRect.ButtonImage = null;
            this.btnCropRect.Corner = BeeCore.Corner.Both;
            this.btnCropRect.FlatAppearance.BorderSize = 0;
            this.btnCropRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropRect.ForeColor = System.Drawing.Color.Black;
            this.btnCropRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropRect.IsCLick = true;
            this.btnCropRect.IsNotChange = false;
            this.btnCropRect.IsRect = false;
            this.btnCropRect.IsUnGroup = false;
            this.btnCropRect.Location = new System.Drawing.Point(2, 3);
            this.btnCropRect.Name = "btnCropRect";
            this.btnCropRect.Size = new System.Drawing.Size(176, 40);
            this.btnCropRect.TabIndex = 2;
            this.btnCropRect.Text = "Entire";
            this.btnCropRect.TextColor = System.Drawing.Color.Black;
            this.btnCropRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropRect.UseVisualStyleBackColor = false;
            this.btnCropRect.Click += new System.EventHandler(this.btnCropRect_Click_1);
            // 
            // pMode
            // 
            this.pMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pMode.Controls.Add(this.trackPixel);
            this.pMode.Controls.Add(this.label7);
            this.pMode.Controls.Add(this.btnDeleteAll);
            this.pMode.Controls.Add(this.btnRedo);
            this.pMode.Controls.Add(this.btnUndo);
            this.pMode.Controls.Add(this.picColor);
            this.pMode.Controls.Add(this.btnGetColor);
            this.pMode.Location = new System.Drawing.Point(12, 249);
            this.pMode.Name = "pMode";
            this.pMode.Size = new System.Drawing.Size(364, 101);
            this.pMode.TabIndex = 18;
            this.pMode.Paint += new System.Windows.Forms.PaintEventHandler(this.pMode_Paint);
            // 
            // trackPixel
            // 
            this.trackPixel.ColorTrack = System.Drawing.Color.Gray;
            this.trackPixel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackPixel.Location = new System.Drawing.Point(64, 55);
            this.trackPixel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackPixel.Max = 50;
            this.trackPixel.Min = 1;
            this.trackPixel.Name = "trackPixel";
            this.trackPixel.Size = new System.Drawing.Size(293, 45);
            this.trackPixel.Step = 1;
            this.trackPixel.TabIndex = 32;
            this.trackPixel.Value = 1;
            this.trackPixel.ValueScore = 0;
            this.trackPixel.ValueChanged +=  new System.Action<float>(this.trackPixel_ValueChanged);
            this.trackPixel.Validating += new System.ComponentModel.CancelEventHandler(this.trackPixel_Validating);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(0, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 16);
            this.label7.TabIndex = 31;
            this.label7.Text = "Extraction";
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.Image = global::BeeUi.Properties.Resources.Delete;
            this.btnDeleteAll.Location = new System.Drawing.Point(319, 10);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(34, 30);
            this.btnDeleteAll.TabIndex = 35;
            this.btnDeleteAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedo.BackgroundImage = global::BeeUi.Properties.Resources.Redo;
            this.btnRedo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRedo.Location = new System.Drawing.Point(279, 10);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(34, 30);
            this.btnRedo.TabIndex = 34;
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.BackgroundImage = global::BeeUi.Properties.Resources.Undo_3;
            this.btnUndo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUndo.Location = new System.Drawing.Point(239, 10);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(34, 30);
            this.btnUndo.TabIndex = 33;
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // picColor
            // 
            this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picColor.Location = new System.Drawing.Point(64, 9);
            this.picColor.Name = "picColor";
            this.picColor.Size = new System.Drawing.Size(169, 30);
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
            this.btnGetColor.Corner = BeeCore.Corner.Both;
            this.btnGetColor.FlatAppearance.BorderSize = 0;
            this.btnGetColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetColor.ForeColor = System.Drawing.Color.Black;
            this.btnGetColor.Image = global::BeeUi.Properties.Resources.BID_ICON_EXTRACTION_ADD_E_32BIT;
            this.btnGetColor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetColor.IsCLick = false;
            this.btnGetColor.IsNotChange = false;
            this.btnGetColor.IsRect = false;
            this.btnGetColor.IsUnGroup = false;
            this.btnGetColor.Location = new System.Drawing.Point(4, 4);
            this.btnGetColor.Name = "btnGetColor";
            this.btnGetColor.Size = new System.Drawing.Size(54, 36);
            this.btnGetColor.TabIndex = 4;
            this.btnGetColor.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGetColor.TextColor = System.Drawing.Color.Black;
            this.btnGetColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnGetColor.UseVisualStyleBackColor = false;
            this.btnGetColor.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // rjButton3
            // 
            this.rjButton3.BackColor = System.Drawing.SystemColors.Control;
            this.rjButton3.BackgroundColor = System.Drawing.SystemColors.Control;
            this.rjButton3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton3.BackgroundImage")));
            this.rjButton3.BorderColor = System.Drawing.Color.Silver;
            this.rjButton3.BorderRadius = 5;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.ButtonImage = null;
            this.rjButton3.Corner = BeeCore.Corner.Both;
            this.rjButton3.Enabled = false;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.ForeColor = System.Drawing.Color.Black;
            this.rjButton3.Image = global::BeeUi.Properties.Resources.Circle_1;
            this.rjButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton3.IsCLick = false;
            this.rjButton3.IsNotChange = false;
            this.rjButton3.IsRect = false;
            this.rjButton3.IsUnGroup = false;
            this.rjButton3.Location = new System.Drawing.Point(192, 36);
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(175, 40);
            this.rjButton3.TabIndex = 1;
            this.rjButton3.Text = "Hình tròn";
            this.rjButton3.TextColor = System.Drawing.Color.Black;
            this.rjButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton3.UseVisualStyleBackColor = false;
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.Transparent;
            this.rjButton1.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton1.BackgroundImage")));
            this.rjButton1.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton1.BorderRadius = 5;
            this.rjButton1.BorderSize = 0;
            this.rjButton1.ButtonImage = null;
            this.rjButton1.Corner = BeeCore.Corner.Both;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.Image = global::BeeUi.Properties.Resources.Rectangle;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.IsCLick = true;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(15, 36);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(176, 40);
            this.rjButton1.TabIndex = 0;
            this.rjButton1.Text = "Hình Vuông";
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            this.rjButton1.Click += new System.EventHandler(this.rjButton1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnCancel.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 5;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.ButtonImage = null;
            this.btnCancel.Corner = BeeCore.Corner.Both;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.IsCLick = false;
            this.btnCancel.IsNotChange = true;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(196, 567);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(175, 40);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.Black;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rjButton5
            // 
            this.rjButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rjButton5.BackColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton5.BackgroundImage")));
            this.rjButton5.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton5.BorderRadius = 5;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.ButtonImage = null;
            this.rjButton5.Corner = BeeCore.Corner.Both;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton5.IsCLick = true;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = false;
            this.rjButton5.IsUnGroup = true;
            this.rjButton5.Location = new System.Drawing.Point(8, 567);
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(178, 40);
            this.rjButton5.TabIndex = 10;
            this.rjButton5.Text = "OK";
            this.rjButton5.TextColor = System.Drawing.Color.Black;
            this.rjButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton5.UseVisualStyleBackColor = false;
            this.rjButton5.Click += new System.EventHandler(this.rjButton8_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel6);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 492);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Chức năng mở rộng";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.btnMask);
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
            // btnMask
            // 
            this.btnMask.BackColor = System.Drawing.Color.Transparent;
            this.btnMask.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnMask.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMask.BackgroundImage")));
            this.btnMask.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMask.BorderColor = System.Drawing.Color.Transparent;
            this.btnMask.BorderRadius = 5;
            this.btnMask.BorderSize = 1;
            this.btnMask.ButtonImage = null;
            this.btnMask.Corner = BeeCore.Corner.Both;
            this.btnMask.FlatAppearance.BorderSize = 0;
            this.btnMask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMask.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMask.ForeColor = System.Drawing.Color.Black;
            this.btnMask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMask.IsCLick = false;
            this.btnMask.IsNotChange = false;
            this.btnMask.IsRect = false;
            this.btnMask.IsUnGroup = true;
            this.btnMask.Location = new System.Drawing.Point(179, 5);
            this.btnMask.Name = "btnMask";
            this.btnMask.Size = new System.Drawing.Size(175, 40);
            this.btnMask.TabIndex = 21;
            this.btnMask.Text = "Mask";
            this.btnMask.TextColor = System.Drawing.Color.Black;
            this.btnMask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMask.UseVisualStyleBackColor = false;
            this.btnMask.Click += new System.EventHandler(this.btnMask_Click);
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.Transparent;
            this.btnTest.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderRadius = 1;
            this.btnTest.BorderSize = 1;
            this.btnTest.ButtonImage = null;
            this.btnTest.Corner = BeeCore.Corner.Both;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.IsCLick = false;
            this.btnTest.IsNotChange = true;
            this.btnTest.IsRect = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(8, 453);
            this.btnTest.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(376, 71);
            this.btnTest.TabIndex = 38;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // ToolColorArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl2);
            this.Name = "ToolColorArea";
            this.Size = new System.Drawing.Size(400, 646);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.pArea.ResumeLayout(false);
            this.pMode.ResumeLayout(false);
            this.pMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Common.RJButton btnCancel;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker threadProcess;
        private Common.RJButton rjButton3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Common.RJButton rjButton1;
        private Common.RJButton rjButton5;
        private System.Windows.Forms.Panel pMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pArea;
        private Common.RJButton btnCropArea;
        private Common.RJButton btnCropRect;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label10;
        private Common.RJButton btnMask;
        public Common.RJButton btnGetColor;
        private System.Windows.Forms.Panel panel4;
        private Common.RJButton btnClWhite;
        private Common.RJButton btnColor;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.PictureBox picColor;
        private TrackBar2 trackPixel;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.Label label7;
        public TrackBar2 trackScore;
        private Common.RJButton btnTest;
    }
}
