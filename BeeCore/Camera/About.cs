using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace BeeCore
{
	public class About : Form
	{
		private IContainer components = null;

		private Label LabVersion;

		private Label LabCopyRight;

		private Label LabDeclare;

		private Panel PanHeroJe;

		public About()
		{
			InitializeComponent();
		}

		private void About_Load(object sender, EventArgs e)
		{
			string text = "";
			if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Heroje_Standard)
			{
				text = "HEROJE";
				PanHeroJe.Visible = true;
			}
			else if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Hide_Heroje_Logo)
			{
				text = "";
				PanHeroJe.Visible = false;
			}
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				Text = "关于" + text + "-图像算法调试工具";
				LabDeclare.Text = text + "图像算法调试工具";
				LabVersion.Text = "版本:" + ToolCfg.VersionInfo;
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				Text = "關於" + text + "-圖像算法調試工具";
				LabDeclare.Text = text + "圖像算法調試工具";
				LabVersion.Text = "版本:" + ToolCfg.VersionInfo;
			}
			else
			{
				Text = "About " + text + " Barcode And Vision Config Software";
				LabDeclare.Text = "Vision Config Software";
				LabVersion.Text = "Version:" + ToolCfg.VersionInfo;
			}
		}

		private void PanHeroJe_MouseClick(object sender, MouseEventArgs e)
		{
			Process.Start("http://www.heroje.com");
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BeeCore.About));
			this.LabVersion = new System.Windows.Forms.Label();
			this.LabCopyRight = new System.Windows.Forms.Label();
			this.LabDeclare = new System.Windows.Forms.Label();
			this.PanHeroJe = new System.Windows.Forms.Panel();
			base.SuspendLayout();
			this.LabVersion.Font = new System.Drawing.Font("宋体", 20f);
			this.LabVersion.Location = new System.Drawing.Point(58, 112);
			this.LabVersion.Name = "LabVersion";
			this.LabVersion.Size = new System.Drawing.Size(278, 27);
			this.LabVersion.TabIndex = 10;
			this.LabVersion.Text = "版本:V8.08";
			this.LabVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LabCopyRight.Font = new System.Drawing.Font("宋体", 20f);
			this.LabCopyRight.Location = new System.Drawing.Point(53, 154);
			this.LabCopyRight.Name = "LabCopyRight";
			this.LabCopyRight.Size = new System.Drawing.Size(289, 27);
			this.LabCopyRight.TabIndex = 11;
			this.LabCopyRight.Text = "CopyRight®2020";
			this.LabCopyRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LabDeclare.BackColor = System.Drawing.SystemColors.Control;
			this.LabDeclare.Font = new System.Drawing.Font("宋体", 20f, System.Drawing.FontStyle.Bold);
			this.LabDeclare.Location = new System.Drawing.Point(7, 74);
			this.LabDeclare.Name = "LabDeclare";
			this.LabDeclare.Size = new System.Drawing.Size(380, 27);
			this.LabDeclare.TabIndex = 9;
			this.LabDeclare.Text = "HEROJE图像算法调试工具";
			this.LabDeclare.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.PanHeroJe.BackColor = System.Drawing.Color.Transparent;
			this.PanHeroJe.BackgroundImage = (System.Drawing.Image)resources.GetObject("PanHeroJe.BackgroundImage");
			this.PanHeroJe.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.PanHeroJe.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PanHeroJe.Location = new System.Drawing.Point(110, 12);
			this.PanHeroJe.Name = "PanHeroJe";
			this.PanHeroJe.Size = new System.Drawing.Size(174, 56);
			this.PanHeroJe.TabIndex = 12;
			this.PanHeroJe.MouseClick += new System.Windows.Forms.MouseEventHandler(PanHeroJe_MouseClick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(404, 239);
			base.Controls.Add(this.LabVersion);
			base.Controls.Add(this.PanHeroJe);
			base.Controls.Add(this.LabCopyRight);
			base.Controls.Add(this.LabDeclare);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "About";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "关于HEROJE-图像算法调试工具";
			base.Load += new System.EventHandler(About_Load);
			base.ResumeLayout(false);
		}
	}
}
