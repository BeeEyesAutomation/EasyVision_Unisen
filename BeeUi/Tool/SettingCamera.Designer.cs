namespace BeeUi
{
    partial class SettingCamera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingCamera));
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numExplosure = new System.Windows.Forms.NumericUpDown();
            this.trackExplosure = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.numGain = new System.Windows.Forms.NumericUpDown();
            this.trackGain = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.numGamma = new System.Windows.Forms.NumericUpDown();
            this.trackGamma = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.numSaturation = new System.Windows.Forms.NumericUpDown();
            this.trackSaturation = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.numContrast = new System.Windows.Forms.NumericUpDown();
            this.trackContrast = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.numBright = new System.Windows.Forms.NumericUpDown();
            this.trackBright = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.picMov = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numExplosure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackExplosure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackGamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBright)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBright)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMov)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.White;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnApply.Location = new System.Drawing.Point(22, 556);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(683, 86);
            this.btnApply.TabIndex = 29;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.DarkGray;
            this.btnCancel.Location = new System.Drawing.Point(730, 556);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(129, 86);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // numExplosure
            // 
            this.numExplosure.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numExplosure.ForeColor = System.Drawing.Color.Black;
            this.numExplosure.Location = new System.Drawing.Point(140, 476);
            this.numExplosure.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numExplosure.Minimum = new decimal(new int[] {
            13,
            0,
            0,
            -2147483648});
            this.numExplosure.Name = "numExplosure";
            this.numExplosure.Size = new System.Drawing.Size(79, 31);
            this.numExplosure.TabIndex = 27;
            this.numExplosure.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numExplosure.Value = new decimal(new int[] {
            3,
            0,
            0,
            -2147483648});
            this.numExplosure.ValueChanged += new System.EventHandler(this.numExplosure_ValueChanged);
            // 
            // trackExplosure
            // 
            this.trackExplosure.AutoSize = false;
            this.trackExplosure.LargeChange = 1;
            this.trackExplosure.Location = new System.Drawing.Point(2, 5);
            this.trackExplosure.Maximum = -1;
            this.trackExplosure.Minimum = -13;
            this.trackExplosure.Name = "trackExplosure";
            this.trackExplosure.Size = new System.Drawing.Size(594, 26);
            this.trackExplosure.TabIndex = 26;
            this.trackExplosure.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackExplosure.Value = -3;
            this.trackExplosure.Scroll += new System.EventHandler(this.trackExplosure_Scroll);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(18, 479);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 24);
            this.label10.TabIndex = 25;
            this.label10.Text = "Explosure";
            // 
            // numGain
            // 
            this.numGain.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numGain.ForeColor = System.Drawing.Color.Gray;
            this.numGain.Location = new System.Drawing.Point(140, 396);
            this.numGain.Name = "numGain";
            this.numGain.Size = new System.Drawing.Size(79, 31);
            this.numGain.TabIndex = 21;
            this.numGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numGain.ValueChanged += new System.EventHandler(this.numGain_ValueChanged);
            // 
            // trackGain
            // 
            this.trackGain.AutoSize = false;
            this.trackGain.LargeChange = 1;
            this.trackGain.Location = new System.Drawing.Point(255, 399);
            this.trackGain.Maximum = 100;
            this.trackGain.Name = "trackGain";
            this.trackGain.Size = new System.Drawing.Size(594, 26);
            this.trackGain.TabIndex = 20;
            this.trackGain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackGain.Scroll += new System.EventHandler(this.trackGain_Scroll);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(18, 399);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 24);
            this.label8.TabIndex = 19;
            this.label8.Text = "Gain";
            // 
            // numGamma
            // 
            this.numGamma.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numGamma.ForeColor = System.Drawing.Color.Black;
            this.numGamma.Location = new System.Drawing.Point(140, 316);
            this.numGamma.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numGamma.Minimum = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.numGamma.Name = "numGamma";
            this.numGamma.Size = new System.Drawing.Size(79, 31);
            this.numGamma.TabIndex = 18;
            this.numGamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numGamma.Value = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.numGamma.ValueChanged += new System.EventHandler(this.numGamma_ValueChanged);
            // 
            // trackGamma
            // 
            this.trackGamma.AutoSize = false;
            this.trackGamma.BackColor = System.Drawing.Color.White;
            this.trackGamma.LargeChange = 1;
            this.trackGamma.Location = new System.Drawing.Point(255, 319);
            this.trackGamma.Maximum = 500;
            this.trackGamma.Minimum = 72;
            this.trackGamma.Name = "trackGamma";
            this.trackGamma.Size = new System.Drawing.Size(594, 26);
            this.trackGamma.TabIndex = 17;
            this.trackGamma.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackGamma.Value = 72;
            this.trackGamma.Scroll += new System.EventHandler(this.trackGamma_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(18, 319);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 24);
            this.label7.TabIndex = 16;
            this.label7.Text = "Gamma";
            // 
            // numSaturation
            // 
            this.numSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSaturation.ForeColor = System.Drawing.Color.Gray;
            this.numSaturation.Location = new System.Drawing.Point(140, 236);
            this.numSaturation.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numSaturation.Name = "numSaturation";
            this.numSaturation.Size = new System.Drawing.Size(79, 31);
            this.numSaturation.TabIndex = 12;
            this.numSaturation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSaturation.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numSaturation.ValueChanged += new System.EventHandler(this.numSaturation_ValueChanged);
            // 
            // trackSaturation
            // 
            this.trackSaturation.AutoSize = false;
            this.trackSaturation.BackColor = System.Drawing.Color.White;
            this.trackSaturation.LargeChange = 1;
            this.trackSaturation.Location = new System.Drawing.Point(255, 239);
            this.trackSaturation.Maximum = 128;
            this.trackSaturation.Name = "trackSaturation";
            this.trackSaturation.Size = new System.Drawing.Size(594, 26);
            this.trackSaturation.TabIndex = 11;
            this.trackSaturation.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackSaturation.Value = 128;
            this.trackSaturation.Scroll += new System.EventHandler(this.trackSaturation_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(18, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "Saturation";
            // 
            // numContrast
            // 
            this.numContrast.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numContrast.ForeColor = System.Drawing.Color.Black;
            this.numContrast.Location = new System.Drawing.Point(140, 156);
            this.numContrast.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numContrast.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            -2147483648});
            this.numContrast.Name = "numContrast";
            this.numContrast.Size = new System.Drawing.Size(79, 31);
            this.numContrast.TabIndex = 6;
            this.numContrast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numContrast.ValueChanged += new System.EventHandler(this.numContrast_ValueChanged);
            // 
            // trackContrast
            // 
            this.trackContrast.AutoSize = false;
            this.trackContrast.BackColor = System.Drawing.Color.White;
            this.trackContrast.LargeChange = 1;
            this.trackContrast.Location = new System.Drawing.Point(255, 160);
            this.trackContrast.Maximum = 64;
            this.trackContrast.Minimum = -64;
            this.trackContrast.Name = "trackContrast";
            this.trackContrast.Size = new System.Drawing.Size(594, 26);
            this.trackContrast.TabIndex = 5;
            this.trackContrast.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackContrast.Scroll += new System.EventHandler(this.trackContrast_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(18, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "Contrast";
            // 
            // numBright
            // 
            this.numBright.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBright.ForeColor = System.Drawing.Color.Gray;
            this.numBright.Location = new System.Drawing.Point(140, 76);
            this.numBright.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numBright.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            -2147483648});
            this.numBright.Name = "numBright";
            this.numBright.Size = new System.Drawing.Size(79, 31);
            this.numBright.TabIndex = 2;
            this.numBright.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numBright.ValueChanged += new System.EventHandler(this.numBright_ValueChanged);
            // 
            // trackBright
            // 
            this.trackBright.AutoSize = false;
            this.trackBright.LargeChange = 1;
            this.trackBright.Location = new System.Drawing.Point(255, 79);
            this.trackBright.Maximum = 64;
            this.trackBright.Minimum = -64;
            this.trackBright.Name = "trackBright";
            this.trackBright.Size = new System.Drawing.Size(594, 26);
            this.trackBright.TabIndex = 1;
            this.trackBright.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBright.Scroll += new System.EventHandler(this.trackBright_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(18, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brightness";
            // 
            // picMov
            // 
            this.picMov.BackColor = System.Drawing.Color.Transparent;
            this.picMov.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picMov.BackgroundImage")));
            this.picMov.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picMov.Location = new System.Drawing.Point(1, 2);
            this.picMov.Name = "picMov";
            this.picMov.Size = new System.Drawing.Size(27, 25);
            this.picMov.TabIndex = 32;
            this.picMov.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(822, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(51, 51);
            this.btnClose.TabIndex = 31;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label2.Location = new System.Drawing.Point(348, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(273, 39);
            this.label2.TabIndex = 30;
            this.label2.Text = "Cài đặt Camera";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(252, 156);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 31);
            this.panel1.TabIndex = 33;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(252, 316);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(600, 31);
            this.panel3.TabIndex = 35;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.trackExplosure);
            this.panel4.Location = new System.Drawing.Point(252, 476);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(600, 31);
            this.panel4.TabIndex = 36;
            // 
            // SettingCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.trackContrast);
            this.Controls.Add(this.picMov);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numExplosure);
            this.Controls.Add(this.trackBright);
            this.Controls.Add(this.numBright);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numContrast);
            this.Controls.Add(this.numGain);
            this.Controls.Add(this.trackGain);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.trackSaturation);
            this.Controls.Add(this.numGamma);
            this.Controls.Add(this.numSaturation);
            this.Controls.Add(this.trackGamma);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Name = "SettingCamera";
            this.Size = new System.Drawing.Size(876, 661);
            this.Load += new System.EventHandler(this.SettingCamera_Load);
            this.VisibleChanged += new System.EventHandler(this.SettingCamera_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numExplosure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackExplosure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackGamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBright)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBright)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMov)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numGain;
        private System.Windows.Forms.TrackBar trackGain;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numGamma;
        private System.Windows.Forms.TrackBar trackGamma;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numSaturation;
        private System.Windows.Forms.TrackBar trackSaturation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numContrast;
        private System.Windows.Forms.TrackBar trackContrast;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numBright;
        private System.Windows.Forms.TrackBar trackBright;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numExplosure;
        private System.Windows.Forms.TrackBar trackExplosure;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox picMov;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
    }
}
