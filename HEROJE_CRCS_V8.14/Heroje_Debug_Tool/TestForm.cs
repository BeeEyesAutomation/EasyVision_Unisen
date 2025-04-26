using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Heroje_Debug_Tool
{
	public class TestForm : Form
	{
		private IContainer components = null;

		public TestForm()
		{
			InitializeComponent();
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(800, 534);
			base.Name = "TestForm";
			this.Text = "TestForm";
			base.ResumeLayout(false);
		}
	}
}
