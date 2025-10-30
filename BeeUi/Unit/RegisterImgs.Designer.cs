namespace BeeUi.Unit
{
    partial class RegisterImgs
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
            this.btnClose = new BeeInterface.RJButton();
            this.registerImgDashboard1 = new BeeInterface.RegisterImgDashboard();
            this.registerImg = new BeeInterface.RegisterImgDashboard();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 446);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(350, 51);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.AutoFont = true;
            this.btnClose.AutoFontHeightRatio = 0.75F;
            this.btnClose.AutoFontMax = 100F;
            this.btnClose.AutoFontMin = 6F;
            this.btnClose.AutoFontWidthRatio = 0.92F;
            this.btnClose.AutoImage = true;
            this.btnClose.AutoImageMaxRatio = 0.75F;
            this.btnClose.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnClose.AutoImageTint = true;
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnClose.BorderColor = System.Drawing.SystemColors.Control;
            this.btnClose.BorderRadius = 14;
            this.btnClose.BorderSize = 1;
            this.btnClose.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnClose.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnClose.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnClose.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnClose.Corner = BeeGlobal.Corner.Both;
            this.btnClose.DebounceResizeMs = 16;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnClose.ForeColor = System.Drawing.Color.IndianRed;
            this.btnClose.Image = null;
            this.btnClose.ImageDisabled = null;
            this.btnClose.ImageHover = null;
            this.btnClose.ImageNormal = null;
            this.btnClose.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnClose.ImagePressed = null;
            this.btnClose.ImageTextSpacing = 6;
            this.btnClose.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnClose.ImageTintHover = System.Drawing.Color.Empty;
            this.btnClose.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnClose.ImageTintOpacity = 0.5F;
            this.btnClose.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnClose.IsCLick = false;
            this.btnClose.IsNotChange = true;
            this.btnClose.IsRect = false;
            this.btnClose.IsUnGroup = false;
            this.btnClose.Location = new System.Drawing.Point(3, 3);
            this.btnClose.Multiline = false;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(344, 45);
            this.btnClose.TabIndex = 73;
            this.btnClose.Text = "Close";
            this.btnClose.TextColor = System.Drawing.Color.IndianRed;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // registerImgDashboard1
            // 
            this.registerImgDashboard1.AutoNameDigits = 3;
            this.registerImgDashboard1.AutoNamePrefix = "Img";
            this.registerImgDashboard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerImgDashboard1.HideTopBar = true;
            this.registerImgDashboard1.Location = new System.Drawing.Point(0, 0);
            this.registerImgDashboard1.Name = "registerImgDashboard1";
            this.registerImgDashboard1.ShowCameraButton = false;
            this.registerImgDashboard1.Size = new System.Drawing.Size(350, 446);
            this.registerImgDashboard1.TabIndex = 1;
            // 
            // registerImg
            // 
            this.registerImg.AutoNameDigits = 3;
            this.registerImg.AutoNamePrefix = "Img";
            this.registerImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerImg.HideTopBar = true;
            this.registerImg.Location = new System.Drawing.Point(0, 0);
            this.registerImg.Name = "registerImg";
            this.registerImg.ShowCameraButton = false;
            this.registerImg.Size = new System.Drawing.Size(350, 446);
            this.registerImg.TabIndex = 2;
            this.registerImg.SelectedItemChanged += new System.EventHandler<BeeInterface.RegisterImgSelectionChangedEventArgs>(this.registerImg_SelectedItemChanged);
            // 
            // RegisterImgs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.registerImg);
            this.Controls.Add(this.registerImgDashboard1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "RegisterImgs";
            this.Size = new System.Drawing.Size(350, 497);
            this.Load += new System.EventHandler(this.RegisterImgs_Load);
            this.VisibleChanged += new System.EventHandler(this.RegisterImgs_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BeeInterface.RJButton btnClose;
        private BeeInterface.RegisterImgDashboard registerImgDashboard1;
        private BeeInterface.RegisterImgDashboard registerImg;
    }
}
