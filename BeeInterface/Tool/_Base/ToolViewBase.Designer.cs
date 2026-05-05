namespace BeeInterface.Tool._Base
{
    partial class ToolViewBase
    {
        private System.ComponentModel.IContainer components = null;

        protected System.Windows.Forms.TabControl tabRoot;
        protected System.Windows.Forms.TabPage tabGeneral;
        protected System.Windows.Forms.TabPage tabRoi;
        protected System.Windows.Forms.TabPage tabParams;
        protected System.Windows.Forms.TabPage tabResult;

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
            this.tabRoot = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tabRoi = new System.Windows.Forms.TabPage();
            this.tabParams = new System.Windows.Forms.TabPage();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.tabRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabRoot
            // 
            this.tabRoot.Controls.Add(this.tabGeneral);
            this.tabRoot.Controls.Add(this.tabRoi);
            this.tabRoot.Controls.Add(this.tabParams);
            this.tabRoot.Controls.Add(this.tabResult);
            this.tabRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRoot.Location = new System.Drawing.Point(0, 0);
            this.tabRoot.Name = "tabRoot";
            this.tabRoot.SelectedIndex = 0;
            this.tabRoot.Size = new System.Drawing.Size(800, 480);
            this.tabRoot.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(792, 454);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Tag = "general";
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tabRoi
            // 
            this.tabRoi.Location = new System.Drawing.Point(4, 22);
            this.tabRoi.Name = "tabRoi";
            this.tabRoi.Padding = new System.Windows.Forms.Padding(3);
            this.tabRoi.Size = new System.Drawing.Size(792, 454);
            this.tabRoi.TabIndex = 1;
            this.tabRoi.Tag = "roi";
            this.tabRoi.Text = "ROI";
            this.tabRoi.UseVisualStyleBackColor = true;
            // 
            // tabParams
            // 
            this.tabParams.Location = new System.Drawing.Point(4, 22);
            this.tabParams.Name = "tabParams";
            this.tabParams.Padding = new System.Windows.Forms.Padding(3);
            this.tabParams.Size = new System.Drawing.Size(792, 454);
            this.tabParams.TabIndex = 2;
            this.tabParams.Tag = "params";
            this.tabParams.Text = "Params";
            this.tabParams.UseVisualStyleBackColor = true;
            // 
            // tabResult
            // 
            this.tabResult.Location = new System.Drawing.Point(4, 22);
            this.tabResult.Name = "tabResult";
            this.tabResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabResult.Size = new System.Drawing.Size(792, 454);
            this.tabResult.TabIndex = 3;
            this.tabResult.Tag = "result";
            this.tabResult.Text = "Result";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // ToolViewBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabRoot);
            this.Name = "ToolViewBase";
            this.Size = new System.Drawing.Size(800, 480);
            this.tabRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
