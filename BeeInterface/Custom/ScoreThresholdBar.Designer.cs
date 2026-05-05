namespace BeeInterface
{
    partial class ScoreThresholdBar
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel layoutRoot;
        private System.Windows.Forms.Label lblCaption;
        private AdjustBarEx bar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.layoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.lblCaption = new System.Windows.Forms.Label();
            this.bar = new BeeInterface.AdjustBarEx();
            this.layoutRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutRoot
            // 
            this.layoutRoot.ColumnCount = 2;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Controls.Add(this.lblCaption, 0, 0);
            this.layoutRoot.Controls.Add(this.bar, 1, 0);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.RowCount = 1;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Size = new System.Drawing.Size(520, 58);
            this.layoutRoot.TabIndex = 0;
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblCaption.Location = new System.Drawing.Point(3, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(80, 58);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "Score";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bar
            // 
            this.bar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bar.Location = new System.Drawing.Point(89, 3);
            this.bar.Max = 100F;
            this.bar.Min = 0F;
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(428, 52);
            this.bar.Step = 1F;
            this.bar.TabIndex = 1;
            this.bar.Value = 0F;
            // 
            // ScoreThresholdBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutRoot);
            this.Name = "ScoreThresholdBar";
            this.Size = new System.Drawing.Size(520, 58);
            this.layoutRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
