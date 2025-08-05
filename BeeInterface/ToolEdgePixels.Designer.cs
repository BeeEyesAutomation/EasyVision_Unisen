namespace BeeInterface
{
    partial class ToolEdgePixels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolEdgePixels));
            this.pCany = new System.Windows.Forms.Panel();
            this.btnCannyMin = new BeeInterface.RJButton();
            this.btnCannyMedium = new BeeInterface.RJButton();
            this.btnCannyMax = new BeeInterface.RJButton();
            this.label4 = new System.Windows.Forms.Label();
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pArea = new System.Windows.Forms.Panel();
            this.btnCropArea = new BeeInterface.RJButton();
            this.btnCropRect = new BeeInterface.RJButton();
            this.rjButton3 = new BeeInterface.RJButton();
            this.rjButton1 = new BeeInterface.RJButton();
            this.btnCancel = new BeeInterface.RJButton();
            this.rjButton5 = new BeeInterface.RJButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMask = new BeeInterface.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnNot = new BeeInterface.RJButton();
            this.label1 = new System.Windows.Forms.Label();
            this.trackScore = new BeeInterface.TrackBar2();
            this.pCany.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.pArea.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pCany
            // 
            this.pCany.Controls.Add(this.btnCannyMin);
            this.pCany.Controls.Add(this.btnCannyMedium);
            this.pCany.Controls.Add(this.btnCannyMax);
            this.pCany.Location = new System.Drawing.Point(5, 278);
            this.pCany.Name = "pCany";
            this.pCany.Size = new System.Drawing.Size(386, 48);
            this.pCany.TabIndex = 12;
            // 
            // btnCannyMin
            // 
            this.btnCannyMin.BackColor = System.Drawing.Color.Transparent;
            this.btnCannyMin.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCannyMin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCannyMin.BackgroundImage")));
            this.btnCannyMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCannyMin.BorderColor = System.Drawing.Color.Transparent;
            this.btnCannyMin.BorderRadius = 5;
            this.btnCannyMin.BorderSize = 1;
            this.btnCannyMin.FlatAppearance.BorderSize = 0;
            this.btnCannyMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCannyMin.ForeColor = System.Drawing.Color.Black;
            this.btnCannyMin.IsCLick = true;
            this.btnCannyMin.IsNotChange = false;
            this.btnCannyMin.IsRect = false;
            this.btnCannyMin.IsUnGroup = false;
            this.btnCannyMin.Location = new System.Drawing.Point(3, 5);
            this.btnCannyMin.Name = "btnCannyMin";
            this.btnCannyMin.Size = new System.Drawing.Size(123, 40);
            this.btnCannyMin.TabIndex = 7;
            this.btnCannyMin.Text = "Thấp";
            this.btnCannyMin.TextColor = System.Drawing.Color.Black;
            this.btnCannyMin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCannyMin.UseVisualStyleBackColor = false;
            this.btnCannyMin.Click += new System.EventHandler(this.btnCannyMin_Click);
            // 
            // btnCannyMedium
            // 
            this.btnCannyMedium.BackColor = System.Drawing.SystemColors.Control;
            this.btnCannyMedium.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCannyMedium.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCannyMedium.BackgroundImage")));
            this.btnCannyMedium.BorderColor = System.Drawing.Color.Silver;
            this.btnCannyMedium.BorderRadius = 5;
            this.btnCannyMedium.BorderSize = 1;
            this.btnCannyMedium.FlatAppearance.BorderSize = 0;
            this.btnCannyMedium.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCannyMedium.ForeColor = System.Drawing.Color.Black;
            this.btnCannyMedium.IsCLick = false;
            this.btnCannyMedium.IsNotChange = false;
            this.btnCannyMedium.IsRect = false;
            this.btnCannyMedium.IsUnGroup = false;
            this.btnCannyMedium.Location = new System.Drawing.Point(133, 5);
            this.btnCannyMedium.Name = "btnCannyMedium";
            this.btnCannyMedium.Size = new System.Drawing.Size(123, 40);
            this.btnCannyMedium.TabIndex = 8;
            this.btnCannyMedium.Text = "Trung bình";
            this.btnCannyMedium.TextColor = System.Drawing.Color.Black;
            this.btnCannyMedium.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCannyMedium.UseVisualStyleBackColor = false;
            this.btnCannyMedium.Click += new System.EventHandler(this.btnCannyMedium_Click);
            // 
            // btnCannyMax
            // 
            this.btnCannyMax.BackColor = System.Drawing.SystemColors.Control;
            this.btnCannyMax.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCannyMax.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCannyMax.BackgroundImage")));
            this.btnCannyMax.BorderColor = System.Drawing.Color.Silver;
            this.btnCannyMax.BorderRadius = 5;
            this.btnCannyMax.BorderSize = 1;
            this.btnCannyMax.FlatAppearance.BorderSize = 0;
            this.btnCannyMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCannyMax.ForeColor = System.Drawing.Color.Black;
            this.btnCannyMax.IsCLick = false;
            this.btnCannyMax.IsNotChange = false;
            this.btnCannyMax.IsRect = false;
            this.btnCannyMax.IsUnGroup = false;
            this.btnCannyMax.Location = new System.Drawing.Point(263, 5);
            this.btnCannyMax.Name = "btnCannyMax";
            this.btnCannyMax.Size = new System.Drawing.Size(123, 40);
            this.btnCannyMax.TabIndex = 9;
            this.btnCannyMax.Text = "Cao";
            this.btnCannyMax.TextColor = System.Drawing.Color.Black;
            this.btnCannyMax.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCannyMax.UseVisualStyleBackColor = false;
            this.btnCannyMax.Click += new System.EventHandler(this.btnCannyMax_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(19, 342);
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
            this.tabControl2.Size = new System.Drawing.Size(400, 525);
            this.tabControl2.TabIndex = 17;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.trackScore);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.pArea);
            this.tabPage3.Controls.Add(this.rjButton3);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.rjButton1);
            this.tabPage3.Controls.Add(this.btnCancel);
            this.tabPage3.Controls.Add(this.pCany);
            this.tabPage3.Controls.Add(this.rjButton5);
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(392, 492);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Căn bản";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 255);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 20);
            this.label7.TabIndex = 24;
            this.label7.Text = "Fine tune outline";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 20);
            this.label6.TabIndex = 22;
            this.label6.Text = "Search Range";
            // 
            // pArea
            // 
            this.pArea.Controls.Add(this.btnCropArea);
            this.pArea.Controls.Add(this.btnCropRect);
            this.pArea.Location = new System.Drawing.Point(22, 110);
            this.pArea.Name = "pArea";
            this.pArea.Size = new System.Drawing.Size(364, 48);
            this.pArea.TabIndex = 23;
            // 
            // btnCropArea
            // 
            this.btnCropArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCropArea.BackgroundImage")));
            this.btnCropArea.BorderColor = System.Drawing.Color.Silver;
            this.btnCropArea.BorderRadius = 5;
            this.btnCropArea.BorderSize = 1;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCropArea.ForeColor = System.Drawing.Color.Black;
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
            // rjButton3
            // 
            this.rjButton3.BackColor = System.Drawing.SystemColors.Control;
            this.rjButton3.BackgroundColor = System.Drawing.SystemColors.Control;
            this.rjButton3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton3.BackgroundImage")));
            this.rjButton3.BorderColor = System.Drawing.Color.Silver;
            this.rjButton3.BorderRadius = 5;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.ForeColor = System.Drawing.Color.Black;
            this.rjButton3.Image = global::BeeInterface.Properties.Resources.Circle_1;
            this.rjButton3.IsCLick = false;
            this.rjButton3.IsNotChange = false;
            this.rjButton3.IsRect = false;
            this.rjButton3.IsUnGroup = false;
            this.rjButton3.Location = new System.Drawing.Point(201, 34);
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
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton1.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton1.BorderRadius = 5;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.Image = global::BeeInterface.Properties.Resources.Rectangle;
            this.rjButton1.IsCLick = true;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(33, 34);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(161, 40);
            this.rjButton1.TabIndex = 0;
            this.rjButton1.Text = "Hình Vuông";
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 5;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.IsCLick = false;
            this.btnCancel.IsNotChange = true;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(215, 446);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(171, 40);
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
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.IsCLick = true;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = false;
            this.rjButton5.IsUnGroup = false;
            this.rjButton5.Location = new System.Drawing.Point(8, 446);
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(137, 40);
            this.rjButton5.TabIndex = 10;
            this.rjButton5.Text = "OK";
            this.rjButton5.TextColor = System.Drawing.Color.Black;
            this.rjButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton5.UseVisualStyleBackColor = false;
            this.rjButton5.Click += new System.EventHandler(this.rjButton8_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnMask);
            this.panel1.Location = new System.Drawing.Point(22, 173);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 48);
            this.panel1.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-2, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 25;
            this.label2.Text = "Add Area Mask";
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
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel4);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(392, 492);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Chức năng mở rộng";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnNot);
            this.panel4.Location = new System.Drawing.Point(180, 27);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(192, 48);
            this.panel4.TabIndex = 22;
            // 
            // btnNot
            // 
            this.btnNot.BackColor = System.Drawing.Color.Transparent;
            this.btnNot.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnNot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNot.BackgroundImage")));
            this.btnNot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNot.BorderColor = System.Drawing.Color.Transparent;
            this.btnNot.BorderRadius = 5;
            this.btnNot.BorderSize = 1;
            this.btnNot.FlatAppearance.BorderSize = 0;
            this.btnNot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNot.ForeColor = System.Drawing.Color.Black;
            this.btnNot.IsCLick = true;
            this.btnNot.IsNotChange = false;
            this.btnNot.IsRect = false;
            this.btnNot.IsUnGroup = true;
            this.btnNot.Location = new System.Drawing.Point(3, 4);
            this.btnNot.Name = "btnNot";
            this.btnNot.Size = new System.Drawing.Size(183, 40);
            this.btnNot.TabIndex = 17;
            this.btnNot.Text = "Đảo ngược màu";
            this.btnNot.TextColor = System.Drawing.Color.Black;
            this.btnNot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNot.UseVisualStyleBackColor = false;
            this.btnNot.Click += new System.EventHandler(this.btnNot_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Đảo ngược màu";
            // 
            // trackScore
            // 
            this.trackScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackScore.Location = new System.Drawing.Point(22, 380);
            this.trackScore.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackScore.Max = 100;
            this.trackScore.Min = 0;
            this.trackScore.Name = "trackScore";
            this.trackScore.Size = new System.Drawing.Size(360, 45);
            this.trackScore.Step = 1;
            this.trackScore.TabIndex = 29;
            this.trackScore.Value = 0;
            this.trackScore.ValueChanged +=  new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // ToolEdgePixels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl2);
            this.Name = "ToolEdgePixels";
            this.Size = new System.Drawing.Size(400, 525);
            this.Load += new System.EventHandler(this.ToolOutLine_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.pCany.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.pArea.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private RJButton btnCannyMedium;
        private RJButton btnCannyMin;
        private RJButton btnCannyMax;
        private RJButton btnCancel;
        private System.Windows.Forms.Panel pCany;
        private System.Windows.Forms.Label label4;
        private RJButton rjButton3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private RJButton rjButton1;
        private RJButton rjButton5;
        public System.ComponentModel.BackgroundWorker threadProcess;
        private RJButton btnMask;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pArea;
        private RJButton btnCropArea;
        private RJButton btnCropRect;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel4;
        private RJButton btnNot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        public TrackBar2 trackScore;
    }
}
