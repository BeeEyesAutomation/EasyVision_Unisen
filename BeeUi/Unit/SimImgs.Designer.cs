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
            this.btnContinuous = new BeeInterface.RJButton();
            this.btnCap = new BeeInterface.RJButton();
            this.numDelay = new BeeInterface.CustomNumericEx();
            this.autoFontLabel1 = new BeeInterface.AutoFontLabel();
            this.registerImg = new BeeInterface.RegisterImgDashboard();
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
            this.tableLayoutPanel1.Controls.Add(this.numDelay, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 558);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(397, 76);
            this.tableLayoutPanel1.TabIndex = 3;
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
            this.btnContinuous.Size = new System.Drawing.Size(189, 28);
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
            this.btnCap.Size = new System.Drawing.Size(188, 28);
            this.btnCap.TabIndex = 10;
            this.btnCap.Text = "Single Trigger";
            this.btnCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCap.TextColor = System.Drawing.Color.Black;
            this.btnCap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCap.UseVisualStyleBackColor = false;
            this.btnCap.Click += new System.EventHandler(this.btnCap_Click);
            // 
            // numDelay
            // 
            this.numDelay.AutoShowTextbox = false;
            this.numDelay.AutoSizeTextbox = true;
            this.numDelay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numDelay.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numDelay.BorderRadius = 6;
            this.numDelay.ButtonMaxSize = 64;
            this.numDelay.ButtonMinSize = 24;
            this.numDelay.Decimals = 0;
            this.numDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numDelay.ElementGap = 6;
            this.numDelay.FillTextboxToAvailable = true;
            this.numDelay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numDelay.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numDelay.KeyboardStep = 1F;
            this.numDelay.Location = new System.Drawing.Point(201, 41);
            this.numDelay.Max = 10000F;
            this.numDelay.MaxTextboxWidth = 0;
            this.numDelay.Min = 100F;
            this.numDelay.MinimumSize = new System.Drawing.Size(120, 32);
            this.numDelay.MinTextboxWidth = 16;
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(193, 32);
            this.numDelay.SnapToStep = true;
            this.numDelay.StartWithTextboxHidden = false;
            this.numDelay.Step = 1F;
            this.numDelay.TabIndex = 12;
            this.numDelay.TextboxFontSize = 12F;
            this.numDelay.TextboxSidePadding = 12;
            this.numDelay.TextboxWidth = 56;
            this.numDelay.UnitText = "";
            this.numDelay.Value = 100F;
            this.numDelay.WheelStep = 1F;
            this.numDelay.ValueChanged += new System.Action<float>(this.numDelay_ValueChanged);
            // 
            // autoFontLabel1
            // 
            this.autoFontLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoFontLabel1.AutoFont = true;
            this.autoFontLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.47559F);
            this.autoFontLabel1.Location = new System.Drawing.Point(5, 43);
            this.autoFontLabel1.Margin = new System.Windows.Forms.Padding(5);
            this.autoFontLabel1.Name = "autoFontLabel1";
            this.autoFontLabel1.Size = new System.Drawing.Size(188, 28);
            this.autoFontLabel1.TabIndex = 13;
            this.autoFontLabel1.Text = "Delay Check";
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
            this.registerImg.Size = new System.Drawing.Size(397, 558);
            this.registerImg.TabIndex = 2;
            this.registerImg.UpdateGlobal = false;
            this.registerImg.SelectedItemChanged += new System.EventHandler<BeeInterface.RegisterImgSelectionChangedEventArgs>(this.registerImg_SelectedItemChanged);
            // 
            // SimImgs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.registerImg);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "SimImgs";
            this.Size = new System.Drawing.Size(397, 634);
            this.Load += new System.EventHandler(this.RegisterImgs_Load);
            this.VisibleChanged += new System.EventHandler(this.RegisterImgs_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
  
        private BeeInterface.RegisterImgDashboard registerImg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public BeeInterface.RJButton btnCap;
        public BeeInterface.RJButton btnContinuous;
        private BeeInterface.CustomNumericEx numDelay;
        private BeeInterface.AutoFontLabel autoFontLabel1;
    }
}
