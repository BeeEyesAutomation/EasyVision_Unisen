
namespace BeeUi
{
    partial class SettingDevice
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingDevice));
            this.cbReSolution = new System.Windows.Forms.ComboBox();
            this.btnSave = new BeeUi.Common.RJButton();
            this.rjButton2 = new BeeUi.Common.RJButton();
            this.btnEqualize = new BeeUi.Common.RJButton();
            this.btnInverse = new BeeUi.Common.RJButton();
            this.btnIamgeImgMirror = new BeeUi.Common.RJButton();
            this.rjButton1 = new BeeUi.Common.RJButton();
            this.trackGain = new BeeUi.TrackBar2();
            this.trackExpo = new BeeUi.TrackBar2();
            this.btnAutoExposure = new BeeUi.Common.RJButton();
            this.SuspendLayout();
            // 
            // cbReSolution
            // 
            this.cbReSolution.BackColor = System.Drawing.Color.Wheat;
            this.cbReSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbReSolution.FormattingEnabled = true;
            this.cbReSolution.Items.AddRange(new object[] {
            "320x240 (0.08 MP)",
            "640x480 (0.3 MP)",
            "800x600 (0,5 MP)",
            "1024x768 (0.8 MP)",
            "1280x720 (1.3 MP)",
            "1280x1080 (1.4 MP)",
            "1600x1200 (1.9 MP)",
            "1920x1080 (2.1 MP)",
            "2048x1536 (3.1 MP)",
            "2592x1944 (5 MP)"});
            this.cbReSolution.Location = new System.Drawing.Point(143, 19);
            this.cbReSolution.Name = "cbReSolution";
            this.cbReSolution.Size = new System.Drawing.Size(276, 32);
            this.cbReSolution.TabIndex = 43;
            this.cbReSolution.SelectedIndexChanged += new System.EventHandler(this.cbReSolution_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.BorderColor = System.Drawing.Color.Silver;
            this.btnSave.BorderRadius = 5;
            this.btnSave.BorderSize = 1;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = global::BeeUi.Properties.Resources.Save;
            this.btnSave.IsCLick = false;
            this.btnSave.IsNotChange = true;
            this.btnSave.IsRect = false;
            this.btnSave.IsUnGroup = true;
            this.btnSave.Location = new System.Drawing.Point(12, 284);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(407, 45);
            this.btnSave.TabIndex = 45;
            this.btnSave.Text = "Save to Device";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextColor = System.Drawing.Color.Black;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // rjButton2
            // 
            this.rjButton2.BackColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton2.BackgroundImage")));
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 5;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.Image = global::BeeUi.Properties.Resources.HD_1080p;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = true;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = true;
            this.rjButton2.Location = new System.Drawing.Point(12, 17);
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(113, 39);
            this.rjButton2.TabIndex = 44;
            this.rjButton2.Text = "Resolution";
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // btnEqualize
            // 
            this.btnEqualize.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnEqualize.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnEqualize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEqualize.BackgroundImage")));
            this.btnEqualize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEqualize.BorderColor = System.Drawing.Color.Silver;
            this.btnEqualize.BorderRadius = 5;
            this.btnEqualize.BorderSize = 1;
            this.btnEqualize.Enabled = false;
            this.btnEqualize.FlatAppearance.BorderSize = 0;
            this.btnEqualize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEqualize.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEqualize.ForeColor = System.Drawing.Color.Black;
            this.btnEqualize.Image = global::BeeUi.Properties.Resources.Image;
            this.btnEqualize.IsCLick = false;
            this.btnEqualize.IsNotChange = false;
            this.btnEqualize.IsRect = false;
            this.btnEqualize.IsUnGroup = true;
            this.btnEqualize.Location = new System.Drawing.Point(287, 217);
            this.btnEqualize.Name = "btnEqualize";
            this.btnEqualize.Size = new System.Drawing.Size(132, 48);
            this.btnEqualize.TabIndex = 34;
            this.btnEqualize.Text = "Equalize";
            this.btnEqualize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEqualize.TextColor = System.Drawing.Color.Black;
            this.btnEqualize.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEqualize.UseVisualStyleBackColor = false;
            // 
            // btnInverse
            // 
            this.btnInverse.BackColor = System.Drawing.Color.Transparent;
            this.btnInverse.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnInverse.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnInverse.BackgroundImage")));
            this.btnInverse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInverse.BorderColor = System.Drawing.Color.Transparent;
            this.btnInverse.BorderRadius = 5;
            this.btnInverse.BorderSize = 1;
            this.btnInverse.FlatAppearance.BorderSize = 0;
            this.btnInverse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInverse.ForeColor = System.Drawing.Color.Black;
            this.btnInverse.Image = global::BeeUi.Properties.Resources.Invert_Colors;
            this.btnInverse.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnInverse.IsCLick = false;
            this.btnInverse.IsNotChange = false;
            this.btnInverse.IsRect = false;
            this.btnInverse.IsUnGroup = true;
            this.btnInverse.Location = new System.Drawing.Point(143, 217);
            this.btnInverse.Name = "btnInverse";
            this.btnInverse.Size = new System.Drawing.Size(113, 48);
            this.btnInverse.TabIndex = 7;
            this.btnInverse.Text = "Inverse";
            this.btnInverse.TextColor = System.Drawing.Color.Black;
            this.btnInverse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInverse.UseVisualStyleBackColor = false;
            this.btnInverse.Click += new System.EventHandler(this.btnInverse_Click);
            // 
            // btnIamgeImgMirror
            // 
            this.btnIamgeImgMirror.BackColor = System.Drawing.Color.Transparent;
            this.btnIamgeImgMirror.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnIamgeImgMirror.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnIamgeImgMirror.BackgroundImage")));
            this.btnIamgeImgMirror.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIamgeImgMirror.BorderColor = System.Drawing.Color.Transparent;
            this.btnIamgeImgMirror.BorderRadius = 5;
            this.btnIamgeImgMirror.BorderSize = 1;
            this.btnIamgeImgMirror.FlatAppearance.BorderSize = 0;
            this.btnIamgeImgMirror.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIamgeImgMirror.ForeColor = System.Drawing.Color.Black;
            this.btnIamgeImgMirror.Image = global::BeeUi.Properties.Resources.Flip_Vertical;
            this.btnIamgeImgMirror.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIamgeImgMirror.IsCLick = false;
            this.btnIamgeImgMirror.IsNotChange = false;
            this.btnIamgeImgMirror.IsRect = false;
            this.btnIamgeImgMirror.IsUnGroup = true;
            this.btnIamgeImgMirror.Location = new System.Drawing.Point(12, 217);
            this.btnIamgeImgMirror.Name = "btnIamgeImgMirror";
            this.btnIamgeImgMirror.Size = new System.Drawing.Size(113, 48);
            this.btnIamgeImgMirror.TabIndex = 6;
            this.btnIamgeImgMirror.Text = "Mirroring";
            this.btnIamgeImgMirror.TextColor = System.Drawing.Color.Black;
            this.btnIamgeImgMirror.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIamgeImgMirror.UseVisualStyleBackColor = false;
            this.btnIamgeImgMirror.Click += new System.EventHandler(this.btnIamgeImgMirror_Click);
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
            this.rjButton1.Enabled = false;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.Image = global::BeeUi.Properties.Resources.Exposure;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = true;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = true;
            this.rjButton1.Location = new System.Drawing.Point(12, 149);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(113, 39);
            this.rjButton1.TabIndex = 5;
            this.rjButton1.Text = "Gain";
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // trackGain
            // 
            this.trackGain.ColorTrack = System.Drawing.Color.Gray;
            this.trackGain.Location = new System.Drawing.Point(143, 149);
            this.trackGain.Max = 255;
            this.trackGain.Min = 1;
            this.trackGain.Name = "trackGain";
            this.trackGain.Size = new System.Drawing.Size(276, 53);
            this.trackGain.Step = 1;
            this.trackGain.TabIndex = 3;
            this.trackGain.Value = 1;
            this.trackGain.ValueScore = 0;
            this.trackGain.ValueChanged += new System.Action<float>(this.trackGain_ValueChanged);
            // 
            // trackExpo
            // 
            this.trackExpo.ColorTrack = System.Drawing.Color.Gray;
            this.trackExpo.Location = new System.Drawing.Point(143, 83);
            this.trackExpo.Max = 255;
            this.trackExpo.Min = 1;
            this.trackExpo.Name = "trackExpo";
            this.trackExpo.Size = new System.Drawing.Size(276, 50);
            this.trackExpo.Step = 1;
            this.trackExpo.TabIndex = 2;
            this.trackExpo.Value = 1;
            this.trackExpo.ValueScore = 0;
            this.trackExpo.ValueChanged += new System.Action<float>(this.trackExpo_ValueChanged);
            // 
            // btnAutoExposure
            // 
            this.btnAutoExposure.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoExposure.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnAutoExposure.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAutoExposure.BackgroundImage")));
            this.btnAutoExposure.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAutoExposure.BorderColor = System.Drawing.Color.Transparent;
            this.btnAutoExposure.BorderRadius = 5;
            this.btnAutoExposure.BorderSize = 1;
            this.btnAutoExposure.FlatAppearance.BorderSize = 0;
            this.btnAutoExposure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoExposure.ForeColor = System.Drawing.Color.Black;
            this.btnAutoExposure.Image = global::BeeUi.Properties.Resources.Aperture;
            this.btnAutoExposure.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAutoExposure.IsCLick = false;
            this.btnAutoExposure.IsNotChange = false;
            this.btnAutoExposure.IsRect = false;
            this.btnAutoExposure.IsUnGroup = true;
            this.btnAutoExposure.Location = new System.Drawing.Point(12, 83);
            this.btnAutoExposure.Name = "btnAutoExposure";
            this.btnAutoExposure.Size = new System.Drawing.Size(113, 39);
            this.btnAutoExposure.TabIndex = 1;
            this.btnAutoExposure.Text = "Auto Exposure";
            this.btnAutoExposure.TextColor = System.Drawing.Color.Black;
            this.btnAutoExposure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAutoExposure.UseVisualStyleBackColor = false;
            this.btnAutoExposure.Click += new System.EventHandler(this.btnAutoExposure_Click);
            // 
            // SettingDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 334);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.rjButton2);
            this.Controls.Add(this.cbReSolution);
            this.Controls.Add(this.btnEqualize);
            this.Controls.Add(this.btnInverse);
            this.Controls.Add(this.btnIamgeImgMirror);
            this.Controls.Add(this.rjButton1);
            this.Controls.Add(this.trackGain);
            this.Controls.Add(this.trackExpo);
            this.Controls.Add(this.btnAutoExposure);
            this.Name = "SettingDevice";
            this.Text = "SettingDevice";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingDevice_FormClosing);
            this.Load += new System.EventHandler(this.SettingDevice_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Common.RJButton btnAutoExposure;
        private TrackBar2 trackExpo;
        private TrackBar2 trackGain;
        private Common.RJButton rjButton1;
        private Common.RJButton btnIamgeImgMirror;
        private Common.RJButton btnInverse;
        public Common.RJButton btnEqualize;
        public System.Windows.Forms.ComboBox cbReSolution;
        private Common.RJButton rjButton2;
        public Common.RJButton btnSave;
    }
}