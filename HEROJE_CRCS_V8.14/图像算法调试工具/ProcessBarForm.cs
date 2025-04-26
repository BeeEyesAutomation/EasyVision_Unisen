using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace 图像算法调试工具
{
	public class ProcessBarForm : Form
	{
		private IContainer components = null;

		private ProgressBar PgbProcessBar;

		public ProcessBarForm()
		{
			InitializeComponent();
		}

		private void ProcessBarForm_Load(object sender, EventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.PgbProcessBar = new System.Windows.Forms.ProgressBar();
			base.SuspendLayout();
			this.PgbProcessBar.Location = new System.Drawing.Point(12, 20);
			this.PgbProcessBar.Name = "PgbProcessBar";
			this.PgbProcessBar.Size = new System.Drawing.Size(352, 23);
			this.PgbProcessBar.Step = 20;
			this.PgbProcessBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.PgbProcessBar.TabIndex = 0;
			this.PgbProcessBar.Value = 20;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(376, 62);
			base.Controls.Add(this.PgbProcessBar);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProcessBarForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Uploading...";
			base.Load += new System.EventHandler(ProcessBarForm_Load);
			base.ResumeLayout(false);
		}
	}
}
