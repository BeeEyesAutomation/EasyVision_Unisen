
using BeeGlobal;


namespace BeeInterface
{
    partial class SettingStep2
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
            this.workRead = new System.ComponentModel.BackgroundWorker();
            this.tmNotPress = new System.Windows.Forms.Timer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNextStep = new BeeInterface.RJButton();
            this.btnCancel = new BeeInterface.RJButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RegisterImg = new BeeInterface.RegisterImgDashboard();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // workRead
            // 
            this.workRead.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workRead_DoWork);
            this.workRead.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workRead_RunWorkerCompleted);
            // 
            // tmNotPress
            // 
            this.tmNotPress.Interval = 200;
            this.tmNotPress.Tick += new System.EventHandler(this.tmNotPress_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNextStep);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 498);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(371, 62);
            this.panel1.TabIndex = 14;
            // 
            // btnNextStep
            // 
            this.btnNextStep.AutoFont = true;
            this.btnNextStep.AutoFontHeightRatio = 0.65F;
            this.btnNextStep.AutoFontMax = 100F;
            this.btnNextStep.AutoFontMin = 6F;
            this.btnNextStep.AutoFontWidthRatio = 0.92F;
            this.btnNextStep.AutoImage = true;
            this.btnNextStep.AutoImageMaxRatio = 0.75F;
            this.btnNextStep.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnNextStep.AutoImageTint = true;
            this.btnNextStep.BackColor = System.Drawing.SystemColors.Control;
            this.btnNextStep.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnNextStep.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnNextStep.BorderRadius = 5;
            this.btnNextStep.BorderSize = 1;
            this.btnNextStep.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnNextStep.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnNextStep.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnNextStep.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnNextStep.Corner = BeeGlobal.Corner.Both;
            this.btnNextStep.DebounceResizeMs = 16;
            this.btnNextStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNextStep.Enabled = false;
            this.btnNextStep.FlatAppearance.BorderSize = 0;
            this.btnNextStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.6875F, System.Drawing.FontStyle.Bold);
            this.btnNextStep.ForeColor = System.Drawing.Color.Black;
            this.btnNextStep.Image = null;
            this.btnNextStep.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNextStep.ImageDisabled = null;
            this.btnNextStep.ImageHover = null;
            this.btnNextStep.ImageNormal = null;
            this.btnNextStep.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnNextStep.ImagePressed = null;
            this.btnNextStep.ImageTextSpacing = 6;
            this.btnNextStep.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnNextStep.ImageTintHover = System.Drawing.Color.Empty;
            this.btnNextStep.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnNextStep.ImageTintOpacity = 0.5F;
            this.btnNextStep.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnNextStep.IsCLick = true;
            this.btnNextStep.IsNotChange = true;
            this.btnNextStep.IsRect = false;
            this.btnNextStep.IsTouch = false;
            this.btnNextStep.IsUnGroup = false;
            this.btnNextStep.Location = new System.Drawing.Point(0, 0);
            this.btnNextStep.Multiline = false;
            this.btnNextStep.Name = "btnNextStep";
            this.btnNextStep.Size = new System.Drawing.Size(200, 62);
            this.btnNextStep.TabIndex = 12;
            this.btnNextStep.Text = "NextStep";
            this.btnNextStep.TextColor = System.Drawing.Color.Black;
            this.btnNextStep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNextStep.UseVisualStyleBackColor = false;
            this.btnNextStep.Click += new System.EventHandler(this.btnNextStep_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoFont = true;
            this.btnCancel.AutoFontHeightRatio = 0.65F;
            this.btnCancel.AutoFontMax = 100F;
            this.btnCancel.AutoFontMin = 6F;
            this.btnCancel.AutoFontWidthRatio = 0.92F;
            this.btnCancel.AutoImage = true;
            this.btnCancel.AutoImageMaxRatio = 0.75F;
            this.btnCancel.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCancel.AutoImageTint = true;
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnCancel.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.BorderRadius = 5;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCancel.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCancel.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCancel.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCancel.Corner = BeeGlobal.Corner.Both;
            this.btnCancel.DebounceResizeMs = 16;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.6875F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = null;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.ImageDisabled = null;
            this.btnCancel.ImageHover = null;
            this.btnCancel.ImageNormal = null;
            this.btnCancel.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCancel.ImagePressed = null;
            this.btnCancel.ImageTextSpacing = 6;
            this.btnCancel.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCancel.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCancel.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCancel.ImageTintOpacity = 0.5F;
            this.btnCancel.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCancel.IsCLick = false;
            this.btnCancel.IsNotChange = true;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsTouch = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(200, 0);
            this.btnCancel.Multiline = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(171, 62);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.Black;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.RegisterImg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(371, 498);
            this.panel2.TabIndex = 15;
            // 
            // RegisterImg
            // 
            this.RegisterImg.AutoNameDigits = 3;
            this.RegisterImg.AutoNamePrefix = "Img";
            this.RegisterImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RegisterImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegisterImg.HeightTopBar1 = 60;
            this.RegisterImg.HeightTopBar2 = 50;
            this.RegisterImg.HeightTopBar3 = 38;
            this.RegisterImg.HideTopBar = false;
            this.RegisterImg.Location = new System.Drawing.Point(0, 0);
            this.RegisterImg.Name = "RegisterImg";
            this.RegisterImg.Size = new System.Drawing.Size(371, 498);
            this.RegisterImg.TabIndex = 11;
            this.RegisterImg.UpdateGlobal = true;
            this.RegisterImg.SelectedItemChanged += new System.EventHandler<BeeInterface.RegisterImgSelectionChangedEventArgs>(this.RegisterImg_SelectedItemChanged);
            this.RegisterImg.ItemsChanged += new System.EventHandler(this.RegisterImg_ItemsChanged);
            // 
            // SettingStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "SettingStep2";
            this.Size = new System.Drawing.Size(371, 560);
            this.Load += new System.EventHandler(this.SettingStep2_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private RJButton btnCancel;
        private RJButton btnNextStep;
        private System.ComponentModel.BackgroundWorker workRead;
        private System.Windows.Forms.Timer tmNotPress;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public RegisterImgDashboard RegisterImg;
    }
}
