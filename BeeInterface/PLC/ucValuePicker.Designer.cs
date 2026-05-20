namespace BeeInterface.PLC
{
    partial class ucValuePicker
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lbCaption = new System.Windows.Forms.Label();
            this.cbName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            //
            // lbCaption
            //
            this.lbCaption.AutoSize = true;
            this.lbCaption.Location = new System.Drawing.Point(3, 8);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(80, 13);
            this.lbCaption.TabIndex = 0;
            this.lbCaption.Text = "PLC Variable:";
            //
            // cbName
            //
            this.cbName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(90, 4);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(140, 21);
            this.cbName.TabIndex = 1;
            this.cbName.SelectionChangeCommitted += new System.EventHandler(this.cbName_SelectionChangeCommitted);
            //
            // ucValuePicker
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbName);
            this.Controls.Add(this.lbCaption);
            this.Name = "ucValuePicker";
            this.Size = new System.Drawing.Size(240, 30);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbCaption;
        private System.Windows.Forms.ComboBox cbName;
    }
}
