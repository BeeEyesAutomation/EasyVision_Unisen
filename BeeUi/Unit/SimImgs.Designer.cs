namespace BeeUi.Unit
{
    partial class SimImgs
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.registerImg = new BeeInterface.RegisterImgDashboard();
            this.btnContinuous = new BeeInterface.RJButton();
            this.btnCap = new BeeInterface.RJButton();
            this.registerImgDashboard1 = new BeeInterface.RegisterImgDashboard();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnContinuous, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCap, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 584);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(397, 50);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // registerImg
            // 
            this.registerImg.AutoNameDigits = 3;
            this.registerImg.AutoNamePrefix = "Img";
            this.registerImg.BackColor = System.Drawing.Color.WhiteSmoke;
            this.registerImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerImg.HeightTopBar1 = 50;
            this.registerImg.HeightTopBar2 = 40;
            this.registerImg.HeightTopBar3 = 38;
            this.registerImg.HideTopBar = false;
            this.registerImg.Location = new System.Drawing.Point(0, 0);
            this.registerImg.Name = "registerImg";
            this.registerImg.Size = new System.Drawing.Size(397, 584);
            this.registerImg.TabIndex = 2;
            this.registerImg.UpdateGlobal = true;
            this.registerImg.SelectedItemChanged += new System.EventHandler<BeeInterface.RegisterImgSelectionChangedEventArgs>(this.registerImg_SelectedItemChanged);
            // 
            // btnContinuous
            // 
            this.btnContinuous.AutoFont = true;
            this.btnContinuous.AutoFontHeightRatio = 0.75F;
            this.btnContinuous.AutoFontMax = 100F;
            this.btnContinuous.AutoFontMin = 8F;
            this.btnContinuous.AutoFontWidthRatio = 0.92F;
            this.btnContinuous.AutoImage = true;
            this.btnContinuous.AutoImageMaxRatio = 0.75F;
            this.btnContinuous.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnContinuous.AutoImageTint = true;
            this.btnContinuous.BackColor = System.Drawing.Color.Silver;
            this.btnContinuous.BackgroundColor = System.Drawing.Color.Silver;
            this.btnContinuous.BorderColor = System.Drawing.Color.Silver;
            this.btnContinuous.BorderRadius = 5;
            this.btnContinuous.BorderSize = 1;
            this.btnContinuous.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnContinuous.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnContinuous.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnContinuous.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnContinuous.Corner = BeeGlobal.Corner.Both;
            this.btnContinuous.DebounceResizeMs = 16;
            this.btnContinuous.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnContinuous.FlatAppearance.BorderSize = 0;
            this.btnContinuous.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinuous.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnContinuous.ForeColor = System.Drawing.Color.Black;
            this.btnContinuous.Image = global::BeeUi.Properties.Resources.Play_2;
            this.btnContinuous.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnContinuous.ImageDisabled = null;
            this.btnContinuous.ImageHover = null;
            this.btnContinuous.ImageNormal = null;
            this.btnContinuous.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnContinuous.ImagePressed = null;
            this.btnContinuous.ImageTextSpacing = 0;
            this.btnContinuous.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnContinuous.ImageTintHover = System.Drawing.Color.Empty;
            this.btnContinuous.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnContinuous.ImageTintOpacity = 0.5F;
            this.btnContinuous.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnContinuous.IsCLick = false;
            this.btnContinuous.IsNotChange = false;
            this.btnContinuous.IsRect = false;
            this.btnContinuous.IsTouch = false;
            this.btnContinuous.IsUnGroup = true;
            this.btnContinuous.Location = new System.Drawing.Point(203, 5);
            this.btnContinuous.Margin = new System.Windows.Forms.Padding(5);
            this.btnContinuous.Multiline = false;
            this.btnContinuous.Name = "btnContinuous";
            this.btnContinuous.Size = new System.Drawing.Size(189, 40);
            this.btnContinuous.TabIndex = 11;
            this.btnContinuous.Text = "Continuous";
            this.btnContinuous.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnContinuous.TextColor = System.Drawing.Color.Black;
            this.btnContinuous.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnContinuous.UseVisualStyleBackColor = false;
            this.btnContinuous.Click += new System.EventHandler(this.btnContinuous_Click);
            // 
            // btnCap
            // 
            this.btnCap.AutoFont = true;
            this.btnCap.AutoFontHeightRatio = 0.75F;
            this.btnCap.AutoFontMax = 100F;
            this.btnCap.AutoFontMin = 8F;
            this.btnCap.AutoFontWidthRatio = 0.92F;
            this.btnCap.AutoImage = true;
            this.btnCap.AutoImageMaxRatio = 0.75F;
            this.btnCap.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCap.AutoImageTint = true;
            this.btnCap.BackColor = System.Drawing.Color.Silver;
            this.btnCap.BackgroundColor = System.Drawing.Color.Silver;
            this.btnCap.BorderColor = System.Drawing.Color.Silver;
            this.btnCap.BorderRadius = 5;
            this.btnCap.BorderSize = 1;
            this.btnCap.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCap.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCap.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCap.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCap.Corner = BeeGlobal.Corner.Both;
            this.btnCap.DebounceResizeMs = 16;
            this.btnCap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCap.FlatAppearance.BorderSize = 0;
            this.btnCap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnCap.ForeColor = System.Drawing.Color.Black;
            this.btnCap.Image = global::BeeUi.Properties.Resources.Natural_User_Interface_2;
            this.btnCap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCap.ImageDisabled = null;
            this.btnCap.ImageHover = null;
            this.btnCap.ImageNormal = null;
            this.btnCap.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCap.ImagePressed = null;
            this.btnCap.ImageTextSpacing = 0;
            this.btnCap.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCap.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCap.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCap.ImageTintOpacity = 0.5F;
            this.btnCap.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCap.IsCLick = false;
            this.btnCap.IsNotChange = true;
            this.btnCap.IsRect = false;
            this.btnCap.IsTouch = false;
            this.btnCap.IsUnGroup = true;
            this.btnCap.Location = new System.Drawing.Point(5, 5);
            this.btnCap.Margin = new System.Windows.Forms.Padding(5);
            this.btnCap.Multiline = false;
            this.btnCap.Name = "btnCap";
            this.btnCap.Size = new System.Drawing.Size(188, 40);
            this.btnCap.TabIndex = 10;
            this.btnCap.Text = "Single Trigger";
            this.btnCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCap.TextColor = System.Drawing.Color.Black;
            this.btnCap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCap.UseVisualStyleBackColor = false;
            this.btnCap.Click += new System.EventHandler(this.btnCap_Click);
            // 
            // registerImgDashboard1
            // 
            this.registerImgDashboard1.AutoNameDigits = 3;
            this.registerImgDashboard1.AutoNamePrefix = "Img";
            this.registerImgDashboard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerImgDashboard1.HeightTopBar1 = 50;
            this.registerImgDashboard1.HeightTopBar2 = 70;
            this.registerImgDashboard1.HeightTopBar3 = 40;
            this.registerImgDashboard1.HideTopBar = true;
            this.registerImgDashboard1.Location = new System.Drawing.Point(0, 0);
            this.registerImgDashboard1.Name = "registerImgDashboard1";
            this.registerImgDashboard1.Size = new System.Drawing.Size(397, 634);
            this.registerImgDashboard1.TabIndex = 1;
            this.registerImgDashboard1.UpdateGlobal = true;
            // 
            // SimImgs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.registerImg);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.registerImgDashboard1);
            this.DoubleBuffered = true;
            this.Name = "SimImgs";
            this.Size = new System.Drawing.Size(397, 634);
            this.Load += new System.EventHandler(this.RegisterImgs_Load);
            this.VisibleChanged += new System.EventHandler(this.RegisterImgs_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private BeeInterface.RegisterImgDashboard registerImgDashboard1;
        private BeeInterface.RegisterImgDashboard registerImg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public BeeInterface.RJButton btnCap;
        public BeeInterface.RJButton btnContinuous;
    }
}
