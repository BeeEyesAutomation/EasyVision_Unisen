
namespace BeeUi.Tool
{
    partial class AddTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTool));
            this.toolPage1 = new BeeUi.Commons.ToolPage();
            this.SuspendLayout();
            // 
            // toolPage1
            // 
            this.toolPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolPage1.Location = new System.Drawing.Point(0, 0);
            this.toolPage1.Name = "toolPage1";
            this.toolPage1.Size = new System.Drawing.Size(663, 502);
            this.toolPage1.TabIndex = 0;
            this.toolPage1.Load += new System.EventHandler(this.toolPage1_Load);
            // 
            // AddTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 502);
            this.Controls.Add(this.toolPage1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddTool";
            this.Text = "AddTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddTool_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private Commons.ToolPage toolPage1;
    }
}