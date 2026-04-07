namespace BeeInterface.PLC
{
    partial class ucBitOutput
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
            this.layOut = new System.Windows.Forms.TableLayoutPanel();
            this.Blink = new BeeInterface.RJButton();
            this.Type = new System.Windows.Forms.ComboBox();
            this.lbName = new System.Windows.Forms.Label();
            this.Value = new BeeInterface.RJButton();
            this.layOut.SuspendLayout();
            this.SuspendLayout();
            // 
            // layOut
            // 
            this.layOut.AutoScroll = true;
            this.layOut.BackColor = System.Drawing.SystemColors.Control;
            this.layOut.ColumnCount = 4;
            this.layOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.layOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.28326F));
            this.layOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.67265F));
            this.layOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.0441F));
            this.layOut.Controls.Add(this.Blink, 3, 0);
            this.layOut.Controls.Add(this.Type, 1, 0);
            this.layOut.Controls.Add(this.lbName, 0, 0);
            this.layOut.Controls.Add(this.Value, 2, 0);
            this.layOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layOut.Location = new System.Drawing.Point(0, 0);
            this.layOut.Name = "layOut";
            this.layOut.RowCount = 1;
            this.layOut.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layOut.Size = new System.Drawing.Size(460, 46);
            this.layOut.TabIndex = 57;
            // 
            // Blink
            // 
            this.Blink.AutoFont = true;
            this.Blink.AutoFontHeightRatio = 0.75F;
            this.Blink.AutoFontMax = 100F;
            this.Blink.AutoFontMin = 6F;
            this.Blink.AutoFontWidthRatio = 0.92F;
            this.Blink.AutoImage = true;
            this.Blink.AutoImageMaxRatio = 0.75F;
            this.Blink.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.Blink.AutoImageTint = true;
            this.Blink.BackColor = System.Drawing.SystemColors.Control;
            this.Blink.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Blink.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Blink.BorderRadius = 0;
            this.Blink.BorderSize = 0;
            this.Blink.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.Blink.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.Blink.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.Blink.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Blink.Corner = BeeGlobal.Corner.Both;
            this.Blink.DebounceResizeMs = 16;
            this.Blink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Blink.Enabled = false;
            this.Blink.FlatAppearance.BorderSize = 0;
            this.Blink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Blink.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.Blink.ForeColor = System.Drawing.Color.Black;
            this.Blink.Image = null;
            this.Blink.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Blink.ImageDisabled = null;
            this.Blink.ImageHover = null;
            this.Blink.ImageNormal = null;
            this.Blink.ImagePadding = new System.Windows.Forms.Padding(1);
            this.Blink.ImagePressed = null;
            this.Blink.ImageTextSpacing = 6;
            this.Blink.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.Blink.ImageTintHover = System.Drawing.Color.Empty;
            this.Blink.ImageTintNormal = System.Drawing.Color.Empty;
            this.Blink.ImageTintOpacity = 0.5F;
            this.Blink.ImageTintPressed = System.Drawing.Color.Empty;
            this.Blink.IsCLick = false;
            this.Blink.IsNotChange = false;
            this.Blink.IsRect = false;
            this.Blink.IsTouch = false;
            this.Blink.IsUnGroup = true;
            this.Blink.Location = new System.Drawing.Point(375, 3);
            this.Blink.Multiline = false;
            this.Blink.Name = "Blink";
            this.Blink.Size = new System.Drawing.Size(82, 40);
            this.Blink.TabIndex = 58;
            this.Blink.Text = "OFF";
            this.Blink.TextColor = System.Drawing.Color.Black;
            this.Blink.UseVisualStyleBackColor = false;
            this.Blink.Click += new System.EventHandler(this.Blink_Click);
            // 
            // Type
            // 
            this.Type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Type.DropDownHeight = 140;
            this.Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Type.Enabled = false;
            this.Type.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Type.ForeColor = System.Drawing.Color.Blue;
            this.Type.FormattingEnabled = true;
            this.Type.IntegralHeight = false;
            this.Type.Location = new System.Drawing.Point(68, 3);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(141, 37);
            this.Type.TabIndex = 2;
            this.Type.DropDown += new System.EventHandler(this.Type_DropDown);
            this.Type.SelectionChangeCommitted += new System.EventHandler(this.Type_SelectionChangeCommitted);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(3, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(59, 46);
            this.lbName.TabIndex = 4;
            this.lbName.Text = "Bit 00";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Value
            // 
            this.Value.AutoFont = true;
            this.Value.AutoFontHeightRatio = 0.75F;
            this.Value.AutoFontMax = 100F;
            this.Value.AutoFontMin = 6F;
            this.Value.AutoFontWidthRatio = 0.92F;
            this.Value.AutoImage = true;
            this.Value.AutoImageMaxRatio = 0.75F;
            this.Value.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.Value.AutoImageTint = true;
            this.Value.BackColor = System.Drawing.SystemColors.Control;
            this.Value.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Value.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.Value.BorderRadius = 0;
            this.Value.BorderSize = 0;
            this.Value.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.Value.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.Value.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.Value.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Value.Corner = BeeGlobal.Corner.Both;
            this.Value.DebounceResizeMs = 16;
            this.Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Value.FlatAppearance.BorderSize = 0;
            this.Value.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.Value.ForeColor = System.Drawing.Color.Black;
            this.Value.Image = null;
            this.Value.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Value.ImageDisabled = null;
            this.Value.ImageHover = null;
            this.Value.ImageNormal = null;
            this.Value.ImagePadding = new System.Windows.Forms.Padding(1);
            this.Value.ImagePressed = null;
            this.Value.ImageTextSpacing = 6;
            this.Value.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.Value.ImageTintHover = System.Drawing.Color.Empty;
            this.Value.ImageTintNormal = System.Drawing.Color.Empty;
            this.Value.ImageTintOpacity = 0.5F;
            this.Value.ImageTintPressed = System.Drawing.Color.Empty;
            this.Value.IsCLick = false;
            this.Value.IsNotChange = false;
            this.Value.IsRect = false;
            this.Value.IsTouch = false;
            this.Value.IsUnGroup = true;
            this.Value.Location = new System.Drawing.Point(215, 3);
            this.Value.Multiline = false;
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(154, 40);
            this.Value.TabIndex = 26;
            this.Value.Text = "0";
            this.Value.TextColor = System.Drawing.Color.Black;
            this.Value.UseVisualStyleBackColor = false;
            this.Value.Click += new System.EventHandler(this.Value_Click);
            // 
            // ucBitOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layOut);
            this.DoubleBuffered = true;
            this.Name = "ucBitOutput";
            this.Size = new System.Drawing.Size(460, 46);
            this.layOut.ResumeLayout(false);
            this.layOut.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layOut;
        private System.Windows.Forms.Label lbName;
        private RJButton Value;
        public RJButton Blink;
        public System.Windows.Forms.ComboBox Type;
    }
}
