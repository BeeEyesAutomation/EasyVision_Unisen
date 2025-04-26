using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using Heroje_Debug_Tool.Properties;

namespace 合杰图像算法调试工具
{
	public class StartForm : Form
	{
		private IContainer components = null;

		private Label LabStartUpTips;

		public StartForm()
		{
			InitializeComponent();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				LabStartUpTips.Text = "正在启动中，请稍候....";
			}
			else
			{
				LabStartUpTips.Text = "Software starting....";
			}
		}

		private void StartForm_Load(object sender, EventArgs e)
		{
			if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Heroje_Standard)
			{
				BackgroundImage = Resources.SoftwareStartImageA;
			}
			else if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Hide_Heroje_Logo)
			{
				BackgroundImage = Resources.SoftwareStartImageA2;
			}
			int num = Screen.PrimaryScreen.Bounds.Width;
			int num2 = Screen.PrimaryScreen.Bounds.Height;
			base.Width = num / 3;
			base.Height = base.Width * BackgroundImage.Height / BackgroundImage.Width;
			int num3 = (num - base.Width) / 2;
			int num4 = (num2 - base.Height) / 2;
			base.Location = new Point(num3, num4);
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
			this.LabStartUpTips = new System.Windows.Forms.Label();
			base.SuspendLayout();
			this.LabStartUpTips.AutoSize = true;
			this.LabStartUpTips.BackColor = System.Drawing.Color.White;
			this.LabStartUpTips.Font = new System.Drawing.Font("宋体", 10.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.LabStartUpTips.Location = new System.Drawing.Point(12, 20);
			this.LabStartUpTips.Name = "LabStartUpTips";
			this.LabStartUpTips.Size = new System.Drawing.Size(174, 14);
			this.LabStartUpTips.TabIndex = 0;
			this.LabStartUpTips.Text = "正在启动中，请稍候....";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = Heroje_Debug_Tool.Properties.Resources.SoftwareStartImageA;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			base.ClientSize = new System.Drawing.Size(541, 379);
			base.Controls.Add(this.LabStartUpTips);
			this.DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "StartForm";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "StartForm";
			base.Load += new System.EventHandler(StartForm_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
