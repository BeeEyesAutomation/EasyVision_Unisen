using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HJ_Controls_Lib;

namespace Heroje_Debug_Tool.SubForm.SmallForm
{
	public class MsgTipsYesCancelForm : Form
	{
		private IContainer components = null;

		private BtnExt btnExt1;

		private BtnExt btnExt2;

		private BtnExt btnExt3;

		public MsgTipsYesCancelForm()
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
			this.btnExt1 = new HJ_Controls_Lib.BtnExt();
			this.btnExt2 = new HJ_Controls_Lib.BtnExt();
			this.btnExt3 = new HJ_Controls_Lib.BtnExt();
			base.SuspendLayout();
			this.btnExt1.BackColor = System.Drawing.Color.Transparent;
			this.btnExt1.BtnBackColor = System.Drawing.Color.White;
			this.btnExt1.BtnFont = new System.Drawing.Font("宋体", 10.5f);
			this.btnExt1.BtnForeColor = System.Drawing.Color.White;
			this.btnExt1.BtnText = "确认";
			this.btnExt1.ConerRadius = 5;
			this.btnExt1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnExt1.EnabledMouseEffect = false;
			this.btnExt1.FillColor = System.Drawing.Color.FromArgb(255, 77, 59);
			this.btnExt1.Font = new System.Drawing.Font("微软雅黑", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnExt1.IsRadius = true;
			this.btnExt1.IsShowRect = true;
			this.btnExt1.IsShowTips = false;
			this.btnExt1.Location = new System.Drawing.Point(66, 78);
			this.btnExt1.Margin = new System.Windows.Forms.Padding(0);
			this.btnExt1.Name = "btnExt1";
			this.btnExt1.RectColor = System.Drawing.Color.FromArgb(255, 77, 58);
			this.btnExt1.RectWidth = 1;
			this.btnExt1.Size = new System.Drawing.Size(64, 29);
			this.btnExt1.TabIndex = 1;
			this.btnExt1.TabStop = false;
			this.btnExt1.TipsColor = System.Drawing.Color.FromArgb(232, 30, 99);
			this.btnExt1.TipsText = "";
			this.btnExt2.BackColor = System.Drawing.Color.White;
			this.btnExt2.BtnBackColor = System.Drawing.Color.White;
			this.btnExt2.BtnFont = new System.Drawing.Font("宋体", 10.5f);
			this.btnExt2.BtnForeColor = System.Drawing.Color.White;
			this.btnExt2.BtnText = "取消";
			this.btnExt2.ConerRadius = 5;
			this.btnExt2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnExt2.EnabledMouseEffect = false;
			this.btnExt2.FillColor = System.Drawing.Color.FromArgb(255, 77, 59);
			this.btnExt2.Font = new System.Drawing.Font("微软雅黑", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnExt2.IsRadius = true;
			this.btnExt2.IsShowRect = true;
			this.btnExt2.IsShowTips = false;
			this.btnExt2.Location = new System.Drawing.Point(154, 78);
			this.btnExt2.Margin = new System.Windows.Forms.Padding(0);
			this.btnExt2.Name = "btnExt2";
			this.btnExt2.RectColor = System.Drawing.Color.FromArgb(255, 77, 58);
			this.btnExt2.RectWidth = 1;
			this.btnExt2.Size = new System.Drawing.Size(64, 29);
			this.btnExt2.TabIndex = 2;
			this.btnExt2.TabStop = false;
			this.btnExt2.TipsColor = System.Drawing.Color.FromArgb(232, 30, 99);
			this.btnExt2.TipsText = "";
			this.btnExt3.BackColor = System.Drawing.Color.White;
			this.btnExt3.BtnBackColor = System.Drawing.Color.White;
			this.btnExt3.BtnFont = new System.Drawing.Font("宋体", 10.5f);
			this.btnExt3.BtnForeColor = System.Drawing.Color.White;
			this.btnExt3.BtnText = "取消";
			this.btnExt3.ConerRadius = 5;
			this.btnExt3.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnExt3.EnabledMouseEffect = false;
			this.btnExt3.FillColor = System.Drawing.Color.FromArgb(255, 77, 59);
			this.btnExt3.Font = new System.Drawing.Font("微软雅黑", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnExt3.IsRadius = true;
			this.btnExt3.IsShowRect = true;
			this.btnExt3.IsShowTips = false;
			this.btnExt3.Location = new System.Drawing.Point(141, 21);
			this.btnExt3.Margin = new System.Windows.Forms.Padding(0);
			this.btnExt3.Name = "btnExt3";
			this.btnExt3.RectColor = System.Drawing.Color.FromArgb(255, 77, 58);
			this.btnExt3.RectWidth = 1;
			this.btnExt3.Size = new System.Drawing.Size(64, 29);
			this.btnExt3.TabIndex = 3;
			this.btnExt3.TabStop = false;
			this.btnExt3.TipsColor = System.Drawing.Color.FromArgb(232, 30, 99);
			this.btnExt3.TipsText = "";
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(319, 123);
			base.Controls.Add(this.btnExt3);
			base.Controls.Add(this.btnExt2);
			base.Controls.Add(this.btnExt1);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "MsgTipsYesCancelForm";
			this.Text = "MessageTipsForm";
			base.ResumeLayout(false);
		}
	}
}
