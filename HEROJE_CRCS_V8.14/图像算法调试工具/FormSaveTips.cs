using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace 图像算法调试工具
{
	public class FormSaveTips : Form
	{
		private Form parent;

		public int ac_time = 3000;

		private IContainer components = null;

		private Label LabSaveTips_2;

		private PictureBox PtbShowGif;

		private Label LabSaveTips_1;

		public FormSaveTips()
		{
			InitializeComponent();
		}

		private void AutoClose_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				Invoke((MethodInvoker)delegate
				{
					parent.Close();
				});
			}
			catch (Exception)
			{
			}
		}

		private void FormSaveTips_Load(object sender, EventArgs e)
		{
			System.Timers.Timer timer = new System.Timers.Timer(ac_time);
			timer.Elapsed += AutoClose_Elapsed;
			timer.Start();
			parent = this;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				LabSaveTips_2.Text = "设备配置正在保存中...\r\n请勿断电，以免丢失数据";
			}
			else
			{
				LabSaveTips_2.Text = "Device configuration is being saved... Do not power off to avoid loss of data ";
			}
			LabSaveTips_1.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.SaveTips_1);
			LabSaveTips_2.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.SaveTips_2);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(图像算法调试工具.FormSaveTips));
			this.LabSaveTips_2 = new System.Windows.Forms.Label();
			this.PtbShowGif = new System.Windows.Forms.PictureBox();
			this.LabSaveTips_1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)this.PtbShowGif).BeginInit();
			base.SuspendLayout();
			this.LabSaveTips_2.AutoSize = true;
			this.LabSaveTips_2.BackColor = System.Drawing.Color.Transparent;
			this.LabSaveTips_2.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.LabSaveTips_2.Location = new System.Drawing.Point(4, 179);
			this.LabSaveTips_2.Name = "LabSaveTips_2";
			this.LabSaveTips_2.Size = new System.Drawing.Size(200, 16);
			this.LabSaveTips_2.TabIndex = 0;
			this.LabSaveTips_2.Text = "请勿断电，以免丢失数据！";
			this.PtbShowGif.Image = (System.Drawing.Image)resources.GetObject("PtbShowGif.Image");
			this.PtbShowGif.InitialImage = null;
			this.PtbShowGif.Location = new System.Drawing.Point(-1, 2);
			this.PtbShowGif.Name = "PtbShowGif";
			this.PtbShowGif.Size = new System.Drawing.Size(388, 154);
			this.PtbShowGif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PtbShowGif.TabIndex = 1;
			this.PtbShowGif.TabStop = false;
			this.LabSaveTips_1.AutoSize = true;
			this.LabSaveTips_1.BackColor = System.Drawing.Color.Transparent;
			this.LabSaveTips_1.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.LabSaveTips_1.Location = new System.Drawing.Point(4, 159);
			this.LabSaveTips_1.Name = "LabSaveTips_1";
			this.LabSaveTips_1.Size = new System.Drawing.Size(176, 16);
			this.LabSaveTips_1.TabIndex = 2;
			this.LabSaveTips_1.Text = "设备配置正在保存中...";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			base.ClientSize = new System.Drawing.Size(388, 200);
			base.Controls.Add(this.LabSaveTips_1);
			base.Controls.Add(this.LabSaveTips_2);
			base.Controls.Add(this.PtbShowGif);
			this.DoubleBuffered = true;
			base.Name = "FormSaveTips";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "！！！！";
			base.Load += new System.EventHandler(FormSaveTips_Load);
			((System.ComponentModel.ISupportInitialize)this.PtbShowGif).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
