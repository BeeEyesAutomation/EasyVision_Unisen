using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolCrop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolCrop));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFolder = new BeeInterface.RJButton();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.imgTemp = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.pCany = new System.Windows.Forms.Panel();
            this.btnAreaBlack = new BeeInterface.RJButton();
            this.btnAreaWhite = new BeeInterface.RJButton();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.pCany.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabP1);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(400, 939);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(392, 901);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(386, 895);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.92593F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.07407F));
            this.tableLayoutPanel3.Controls.Add(this.btnFolder, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtFolder, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 392);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(378, 49);
            this.tableLayoutPanel3.TabIndex = 45;
            // 
            // btnFolder
            // 
            this.btnFolder.AutoFont = true;
            this.btnFolder.AutoFontHeightRatio = 0.75F;
            this.btnFolder.AutoFontMax = 100F;
            this.btnFolder.AutoFontMin = 6F;
            this.btnFolder.AutoFontWidthRatio = 0.92F;
            this.btnFolder.AutoImage = true;
            this.btnFolder.AutoImageMaxRatio = 0.75F;
            this.btnFolder.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnFolder.AutoImageTint = true;
            this.btnFolder.BackColor = System.Drawing.SystemColors.Control;
            this.btnFolder.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnFolder.BorderColor = System.Drawing.Color.Silver;
            this.btnFolder.BorderRadius = 10;
            this.btnFolder.BorderSize = 1;
            this.btnFolder.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnFolder.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnFolder.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnFolder.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnFolder.Corner = BeeGlobal.Corner.Right;
            this.btnFolder.DebounceResizeMs = 16;
            this.btnFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFolder.FlatAppearance.BorderSize = 0;
            this.btnFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnFolder.ForeColor = System.Drawing.Color.Black;
            this.btnFolder.Image = null;
            this.btnFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFolder.ImageDisabled = null;
            this.btnFolder.ImageHover = null;
            this.btnFolder.ImageNormal = null;
            this.btnFolder.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnFolder.ImagePressed = null;
            this.btnFolder.ImageTextSpacing = 6;
            this.btnFolder.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnFolder.ImageTintHover = System.Drawing.Color.Empty;
            this.btnFolder.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnFolder.ImageTintOpacity = 0.5F;
            this.btnFolder.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnFolder.IsCLick = false;
            this.btnFolder.IsNotChange = false;
            this.btnFolder.IsRect = false;
            this.btnFolder.IsUnGroup = false;
            this.btnFolder.Location = new System.Drawing.Point(287, 0);
            this.btnFolder.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnFolder.Multiline = false;
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(88, 49);
            this.btnFolder.TabIndex = 3;
            this.btnFolder.Text = "..";
            this.btnFolder.TextColor = System.Drawing.Color.Black;
            this.btnFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFolder.UseVisualStyleBackColor = false;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFolder.Location = new System.Drawing.Point(10, 3);
            this.txtFolder.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(274, 44);
            this.txtFolder.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 368);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 24);
            this.label1.TabIndex = 44;
            this.label1.Text = "Path Save Image";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnTest.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTest.BackgroundImage")));
            this.btnTest.BorderColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderRadius = 10;
            this.btnTest.BorderSize = 1;
            this.btnTest.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnTest.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnTest.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnTest.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.DebounceResizeMs = 16;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.6875F);
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
            this.btnTest.Location = new System.Drawing.Point(20, 451);
            this.btnTest.Margin = new System.Windows.Forms.Padding(20, 10, 5, 5);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(361, 55);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Crop";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.imgTemp, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(1, 118);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(1, 5, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(382, 240);
            this.tableLayoutPanel4.TabIndex = 43;
            // 
            // imgTemp
            // 
            this.imgTemp.BackColor = System.Drawing.Color.White;
            this.imgTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgTemp.Location = new System.Drawing.Point(20, 5);
            this.imgTemp.Margin = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.imgTemp.Name = "imgTemp";
            this.imgTemp.Size = new System.Drawing.Size(357, 230);
            this.imgTemp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgTemp.TabIndex = 31;
            this.imgTemp.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 89);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(378, 24);
            this.label10.TabIndex = 42;
            this.label10.Text = "Image Crop";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 24);
            this.label7.TabIndex = 38;
            this.label7.Text = "Search Range";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnCropFull, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCropHalt, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 29);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 50);
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
            this.btnCropFull.BackColor = System.Drawing.SystemColors.Control;
            this.btnCropFull.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCropFull.BorderColor = System.Drawing.Color.Silver;
            this.btnCropFull.BorderRadius = 10;
            this.btnCropFull.BorderSize = 1;
            this.btnCropFull.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCropFull.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCropFull.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCropFull.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropFull.Corner = BeeGlobal.Corner.Right;
            this.btnCropFull.DebounceResizeMs = 16;
            this.btnCropFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropFull.FlatAppearance.BorderSize = 0;
            this.btnCropFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
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
            this.btnCropFull.Location = new System.Drawing.Point(189, 0);
            this.btnCropFull.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCropFull.Multiline = false;
            this.btnCropFull.Name = "btnCropFull";
            this.btnCropFull.Size = new System.Drawing.Size(186, 50);
            this.btnCropFull.TabIndex = 3;
            this.btnCropFull.Text = "Partial";
            this.btnCropFull.TextColor = System.Drawing.Color.Black;
            this.btnCropFull.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropFull.UseVisualStyleBackColor = false;
            this.btnCropFull.Click += new System.EventHandler(this.btnCropFull_Click);
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
            this.btnCropHalt.BackColor = System.Drawing.SystemColors.Control;
            this.btnCropHalt.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCropHalt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropHalt.BorderColor = System.Drawing.Color.Transparent;
            this.btnCropHalt.BorderRadius = 10;
            this.btnCropHalt.BorderSize = 1;
            this.btnCropHalt.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCropHalt.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCropHalt.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCropHalt.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropHalt.Corner = BeeGlobal.Corner.Left;
            this.btnCropHalt.DebounceResizeMs = 16;
            this.btnCropHalt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropHalt.FlatAppearance.BorderSize = 0;
            this.btnCropHalt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropHalt.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
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
            this.btnCropHalt.Size = new System.Drawing.Size(186, 50);
            this.btnCropHalt.TabIndex = 2;
            this.btnCropHalt.Text = "Entire";
            this.btnCropHalt.TextColor = System.Drawing.Color.Black;
            this.btnCropHalt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropHalt.UseVisualStyleBackColor = false;
            this.btnCropHalt.Click += new System.EventHandler(this.btnCropHalt_Click);
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
            this.btnAreaBlack.AutoFont = true;
            this.btnAreaBlack.AutoFontHeightRatio = 0.75F;
            this.btnAreaBlack.AutoFontMax = 100F;
            this.btnAreaBlack.AutoFontMin = 6F;
            this.btnAreaBlack.AutoFontWidthRatio = 0.92F;
            this.btnAreaBlack.AutoImage = true;
            this.btnAreaBlack.AutoImageMaxRatio = 0.75F;
            this.btnAreaBlack.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAreaBlack.AutoImageTint = true;
            this.btnAreaBlack.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAreaBlack.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAreaBlack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaBlack.BackgroundImage")));
            this.btnAreaBlack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAreaBlack.BorderColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BorderRadius = 5;
            this.btnAreaBlack.BorderSize = 1;
            this.btnAreaBlack.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnAreaBlack.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnAreaBlack.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnAreaBlack.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAreaBlack.Corner = BeeGlobal.Corner.Both;
            this.btnAreaBlack.DebounceResizeMs = 16;
            this.btnAreaBlack.FlatAppearance.BorderSize = 0;
            this.btnAreaBlack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaBlack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnAreaBlack.ForeColor = System.Drawing.Color.Black;
            this.btnAreaBlack.Image = null;
            this.btnAreaBlack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaBlack.ImageDisabled = null;
            this.btnAreaBlack.ImageHover = null;
            this.btnAreaBlack.ImageNormal = null;
            this.btnAreaBlack.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAreaBlack.ImagePressed = null;
            this.btnAreaBlack.ImageTextSpacing = 6;
            this.btnAreaBlack.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAreaBlack.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAreaBlack.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAreaBlack.ImageTintOpacity = 0.5F;
            this.btnAreaBlack.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAreaBlack.IsCLick = true;
            this.btnAreaBlack.IsNotChange = false;
            this.btnAreaBlack.IsRect = false;
            this.btnAreaBlack.IsUnGroup = false;
            this.btnAreaBlack.Location = new System.Drawing.Point(3, 5);
            this.btnAreaBlack.Multiline = false;
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
            this.btnAreaWhite.AutoFont = true;
            this.btnAreaWhite.AutoFontHeightRatio = 0.75F;
            this.btnAreaWhite.AutoFontMax = 100F;
            this.btnAreaWhite.AutoFontMin = 6F;
            this.btnAreaWhite.AutoFontWidthRatio = 0.92F;
            this.btnAreaWhite.AutoImage = true;
            this.btnAreaWhite.AutoImageMaxRatio = 0.75F;
            this.btnAreaWhite.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAreaWhite.AutoImageTint = true;
            this.btnAreaWhite.BackColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaWhite.BackgroundImage")));
            this.btnAreaWhite.BorderColor = System.Drawing.Color.Silver;
            this.btnAreaWhite.BorderRadius = 5;
            this.btnAreaWhite.BorderSize = 1;
            this.btnAreaWhite.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnAreaWhite.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnAreaWhite.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnAreaWhite.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAreaWhite.Corner = BeeGlobal.Corner.Both;
            this.btnAreaWhite.DebounceResizeMs = 16;
            this.btnAreaWhite.FlatAppearance.BorderSize = 0;
            this.btnAreaWhite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaWhite.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnAreaWhite.ForeColor = System.Drawing.Color.Black;
            this.btnAreaWhite.Image = null;
            this.btnAreaWhite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaWhite.ImageDisabled = null;
            this.btnAreaWhite.ImageHover = null;
            this.btnAreaWhite.ImageNormal = null;
            this.btnAreaWhite.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAreaWhite.ImagePressed = null;
            this.btnAreaWhite.ImageTextSpacing = 6;
            this.btnAreaWhite.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAreaWhite.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAreaWhite.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAreaWhite.ImageTintOpacity = 0.5F;
            this.btnAreaWhite.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAreaWhite.IsCLick = false;
            this.btnAreaWhite.IsNotChange = false;
            this.btnAreaWhite.IsRect = false;
            this.btnAreaWhite.IsUnGroup = false;
            this.btnAreaWhite.Location = new System.Drawing.Point(195, 5);
            this.btnAreaWhite.Multiline = false;
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
            this.oK_Cancel1.Size = new System.Drawing.Size(400, 52);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolCrop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.pCany);
            this.Controls.Add(this.tabControl2);
            this.DoubleBuffered = true;
            this.Name = "ToolCrop";
            this.Size = new System.Drawing.Size(400, 939);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgTemp)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pCany.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.Panel pCany;
        private RJButton btnAreaBlack;
        private RJButton btnAreaWhite;
        private System.Windows.Forms.PictureBox imgTemp;
        private System.Windows.Forms.Label label7;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private GroupControl.OK_Cancel oK_Cancel1;
        private RJButton btnTest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RJButton btnFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
    }
}
