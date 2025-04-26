using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace Heroje_Debug_Tool.SubForm
{
	public class ImageSaveSettingForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private IContainer components = null;

		private GroupBox GrbImageDataProcess;

		private Button BtnChangeFtpPasswd;

		private Button BtnChangeFtpName;

		private TextBox TxbFtpPasswd;

		private Label LabFtpPw;

		private TextBox TxbFtpName;

		private Label LabFtpName;

		private CheckBox ChbDecodeNGFtpImg;

		private CheckBox ChbDecodeOKFtpImg;

		private CheckBox ChbDecodeNGSendImg;

		private CheckBox ChbDecodeOKSendImg;

		private CheckBox ChbDecodeNGSaveImg;

		private CheckBox ChbDecodeOKSaveImg;

		public ImageSaveSettingForm()
		{
			InitializeComponent();
		}

		private void ImageSaveSettingForm_Load(object sender, EventArgs e)
		{
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			if (para.DeviceTypeRecord >= 6)
			{
				GrbImageDataProcess.Enabled = true;
				ChbDecodeOKSaveImg.Checked = GetCfgCBFuncCB(139528u) == 8;
				ChbDecodeNGSaveImg.Checked = GetCfgCBFuncCB(139524u) == 4;
				ChbDecodeOKSendImg.Checked = GetCfgCBFuncCB(139522u) == 2;
				ChbDecodeNGSendImg.Checked = GetCfgCBFuncCB(139521u) == 1;
				ChbDecodeOKFtpImg.Checked = GetCfgCBFuncCB(139552u) == 32;
				ChbDecodeNGFtpImg.Checked = GetCfgCBFuncCB(139536u) == 16;
				List<byte> list = new List<byte>();
				for (int i = 0; i < 15; i++)
				{
					byte b = GetCfgCBFuncCB((uint)(147711uL + (ulong)(i << 8)));
					if (b == 0)
					{
						break;
					}
					list.Add(b);
				}
				TxbFtpName.Text = Encoding.Default.GetString(list.ToArray());
				list.Clear();
				for (int j = 0; j < 15; j++)
				{
					byte b2 = GetCfgCBFuncCB((uint)(151807uL + (ulong)(j << 8)));
					if (b2 == 0)
					{
						break;
					}
					list.Add(b2);
				}
				TxbFtpPasswd.Text = Encoding.Default.GetString(list.ToArray());
			}
			else if (para.DeviceTypeRecord == 5)
			{
				GrbImageDataProcess.Enabled = true;
				ChbDecodeOKSaveImg.Enabled = false;
				ChbDecodeNGSaveImg.Enabled = false;
				ChbDecodeOKSendImg.Enabled = false;
				ChbDecodeNGSendImg.Enabled = false;
				ChbDecodeOKFtpImg.Checked = GetCfgCBFuncCB(139552u) == 32;
				ChbDecodeNGFtpImg.Checked = GetCfgCBFuncCB(139536u) == 16;
				List<byte> list2 = new List<byte>();
				for (int k = 0; k < 15; k++)
				{
					byte b3 = GetCfgCBFuncCB((uint)(147711uL + (ulong)(k << 8)));
					if (b3 == 0)
					{
						break;
					}
					list2.Add(b3);
				}
				TxbFtpName.Text = Encoding.Default.GetString(list2.ToArray());
				list2.Clear();
				for (int l = 0; l < 15; l++)
				{
					byte b4 = GetCfgCBFuncCB((uint)(151807uL + (ulong)(l << 8)));
					if (b4 == 0)
					{
						break;
					}
					list2.Add(b4);
				}
			}
			else
			{
				GrbImageDataProcess.Enabled = false;
			}
		}

		public void FunctionOnOff(bool[] CapacityArr)
		{
			ChbDecodeOKSaveImg.Enabled = CapacityArr[17];
			ChbDecodeNGSaveImg.Enabled = CapacityArr[18];
			ChbDecodeOKFtpImg.Enabled = CapacityArr[19];
			ChbDecodeNGFtpImg.Enabled = CapacityArr[20];
		}

		private void ChbDecodeOKSaveImg_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDecodeOKSaveImg.Checked)
				{
					SetCfgCBFuncCB(139528u, 8u);
				}
				else
				{
					SetCfgCBFuncCB(139528u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDecodeOKFtpImg_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDecodeOKFtpImg.Checked)
				{
					SetCfgCBFuncCB(139552u, 32u);
				}
				else
				{
					SetCfgCBFuncCB(139552u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDecodeOKSendImg_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDecodeOKSendImg.Checked)
				{
					SetCfgCBFuncCB(139522u, 2u);
				}
				else
				{
					SetCfgCBFuncCB(139522u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDecodeNGSaveImg_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDecodeNGSaveImg.Checked)
				{
					SetCfgCBFuncCB(139524u, 4u);
				}
				else
				{
					SetCfgCBFuncCB(139524u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDecodeNGFtpImg_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDecodeNGFtpImg.Checked)
				{
					SetCfgCBFuncCB(139536u, 16u);
				}
				else
				{
					SetCfgCBFuncCB(139536u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDecodeNGSendImg_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDecodeNGSendImg.Checked)
				{
					SetCfgCBFuncCB(139521u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(139521u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbFtpName_TextChanged(object sender, EventArgs e)
		{
			BtnChangeFtpName.Visible = true;
		}

		private void BtnChangeFtpName_Click(object sender, EventArgs e)
		{
			try
			{
				string s = TxbFtpName.Text;
				if (Encoding.Default.GetByteCount(s) > 15)
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("请正确输入字符串，最长15", "提示", MessageBoxButtons.OK);
					}
					else
					{
						MessageBox.Show("Please enter the string correctly,max length is 15", "Tips", MessageBoxButtons.OK);
					}
					return;
				}
				byte[] bytes = Encoding.Default.GetBytes(s);
				int num = 0;
				byte[] array = bytes;
				foreach (byte paraVal in array)
				{
					SetCfgCBFuncCB((uint)(147711uL + (ulong)(num << 8)), paraVal);
					num++;
				}
				SetCfgCBFuncCB((uint)(147711uL + (ulong)(num << 8)), 0u);
				SendCfgDataCBFuncCB(0u);
				BtnChangeFtpName.Visible = false;
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请正确输入字符串，最长15", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Please enter the string correctly,max length is 15", "Tips", MessageBoxButtons.OK);
				}
			}
		}

		private void TxbFtpPasswd_TextChanged(object sender, EventArgs e)
		{
			BtnChangeFtpPasswd.Visible = true;
		}

		private void BtnChangeFtpPasswd_Click(object sender, EventArgs e)
		{
			try
			{
				string s = TxbFtpPasswd.Text;
				if (Encoding.Default.GetByteCount(s) > 15)
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("请正确输入字符串，最长15", "提示", MessageBoxButtons.OK);
					}
					else
					{
						MessageBox.Show("Please enter the string correctly,max length is 15", "Tips", MessageBoxButtons.OK);
					}
					return;
				}
				byte[] bytes = Encoding.Default.GetBytes(s);
				int num = 0;
				byte[] array = bytes;
				foreach (byte paraVal in array)
				{
					SetCfgCBFuncCB((uint)(151807uL + (ulong)(num << 8)), paraVal);
					num++;
				}
				SetCfgCBFuncCB((uint)(151807uL + (ulong)(num << 8)), 0u);
				SendCfgDataCBFuncCB(0u);
				BtnChangeFtpPasswd.Visible = false;
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请正确输入字符串，最长15", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Please enter the string correctly,max length is 15", "Tips", MessageBoxButtons.OK);
				}
			}
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
			this.GrbImageDataProcess = new System.Windows.Forms.GroupBox();
			this.BtnChangeFtpPasswd = new System.Windows.Forms.Button();
			this.BtnChangeFtpName = new System.Windows.Forms.Button();
			this.TxbFtpPasswd = new System.Windows.Forms.TextBox();
			this.LabFtpPw = new System.Windows.Forms.Label();
			this.TxbFtpName = new System.Windows.Forms.TextBox();
			this.LabFtpName = new System.Windows.Forms.Label();
			this.ChbDecodeNGFtpImg = new System.Windows.Forms.CheckBox();
			this.ChbDecodeOKFtpImg = new System.Windows.Forms.CheckBox();
			this.ChbDecodeNGSendImg = new System.Windows.Forms.CheckBox();
			this.ChbDecodeOKSendImg = new System.Windows.Forms.CheckBox();
			this.ChbDecodeNGSaveImg = new System.Windows.Forms.CheckBox();
			this.ChbDecodeOKSaveImg = new System.Windows.Forms.CheckBox();
			this.GrbImageDataProcess.SuspendLayout();
			base.SuspendLayout();
			this.GrbImageDataProcess.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbImageDataProcess.Controls.Add(this.BtnChangeFtpPasswd);
			this.GrbImageDataProcess.Controls.Add(this.BtnChangeFtpName);
			this.GrbImageDataProcess.Controls.Add(this.TxbFtpPasswd);
			this.GrbImageDataProcess.Controls.Add(this.LabFtpPw);
			this.GrbImageDataProcess.Controls.Add(this.TxbFtpName);
			this.GrbImageDataProcess.Controls.Add(this.LabFtpName);
			this.GrbImageDataProcess.Controls.Add(this.ChbDecodeNGFtpImg);
			this.GrbImageDataProcess.Controls.Add(this.ChbDecodeOKFtpImg);
			this.GrbImageDataProcess.Controls.Add(this.ChbDecodeNGSendImg);
			this.GrbImageDataProcess.Controls.Add(this.ChbDecodeOKSendImg);
			this.GrbImageDataProcess.Controls.Add(this.ChbDecodeNGSaveImg);
			this.GrbImageDataProcess.Controls.Add(this.ChbDecodeOKSaveImg);
			this.GrbImageDataProcess.Location = new System.Drawing.Point(12, 12);
			this.GrbImageDataProcess.Name = "GrbImageDataProcess";
			this.GrbImageDataProcess.Size = new System.Drawing.Size(455, 153);
			this.GrbImageDataProcess.TabIndex = 57;
			this.GrbImageDataProcess.TabStop = false;
			this.GrbImageDataProcess.Text = "图像数据输出保存";
			this.BtnChangeFtpPasswd.Location = new System.Drawing.Point(210, 118);
			this.BtnChangeFtpPasswd.Name = "BtnChangeFtpPasswd";
			this.BtnChangeFtpPasswd.Size = new System.Drawing.Size(35, 27);
			this.BtnChangeFtpPasswd.TabIndex = 56;
			this.BtnChangeFtpPasswd.Text = "OK";
			this.BtnChangeFtpPasswd.UseVisualStyleBackColor = true;
			this.BtnChangeFtpPasswd.Visible = false;
			this.BtnChangeFtpPasswd.Click += new System.EventHandler(BtnChangeFtpPasswd_Click);
			this.BtnChangeFtpName.Location = new System.Drawing.Point(209, 94);
			this.BtnChangeFtpName.Name = "BtnChangeFtpName";
			this.BtnChangeFtpName.Size = new System.Drawing.Size(35, 27);
			this.BtnChangeFtpName.TabIndex = 39;
			this.BtnChangeFtpName.Text = "OK";
			this.BtnChangeFtpName.UseVisualStyleBackColor = true;
			this.BtnChangeFtpName.Visible = false;
			this.BtnChangeFtpName.Click += new System.EventHandler(BtnChangeFtpName_Click);
			this.TxbFtpPasswd.Location = new System.Drawing.Point(133, 120);
			this.TxbFtpPasswd.MaxLength = 15;
			this.TxbFtpPasswd.Name = "TxbFtpPasswd";
			this.TxbFtpPasswd.Size = new System.Drawing.Size(76, 23);
			this.TxbFtpPasswd.TabIndex = 54;
			this.TxbFtpPasswd.Text = "root";
			this.TxbFtpPasswd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbFtpPasswd.TextChanged += new System.EventHandler(TxbFtpPasswd_TextChanged);
			this.LabFtpPw.AutoSize = true;
			this.LabFtpPw.Location = new System.Drawing.Point(31, 123);
			this.LabFtpPw.Name = "LabFtpPw";
			this.LabFtpPw.Size = new System.Drawing.Size(91, 14);
			this.LabFtpPw.TabIndex = 55;
			this.LabFtpPw.Text = "FTP登录密码:";
			this.TxbFtpName.Location = new System.Drawing.Point(133, 96);
			this.TxbFtpName.MaxLength = 15;
			this.TxbFtpName.Name = "TxbFtpName";
			this.TxbFtpName.Size = new System.Drawing.Size(75, 23);
			this.TxbFtpName.TabIndex = 39;
			this.TxbFtpName.Text = "root";
			this.TxbFtpName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbFtpName.TextChanged += new System.EventHandler(TxbFtpName_TextChanged);
			this.LabFtpName.AutoSize = true;
			this.LabFtpName.Location = new System.Drawing.Point(31, 99);
			this.LabFtpName.Name = "LabFtpName";
			this.LabFtpName.Size = new System.Drawing.Size(105, 14);
			this.LabFtpName.TabIndex = 53;
			this.LabFtpName.Text = "FTP用户登录名:";
			this.ChbDecodeNGFtpImg.AutoSize = true;
			this.ChbDecodeNGFtpImg.Location = new System.Drawing.Point(235, 47);
			this.ChbDecodeNGFtpImg.Name = "ChbDecodeNGFtpImg";
			this.ChbDecodeNGFtpImg.Size = new System.Drawing.Size(159, 18);
			this.ChbDecodeNGFtpImg.TabIndex = 52;
			this.ChbDecodeNGFtpImg.Text = "解码失败FTP保存图片";
			this.ChbDecodeNGFtpImg.UseVisualStyleBackColor = true;
			this.ChbDecodeNGFtpImg.CheckedChanged += new System.EventHandler(ChbDecodeNGFtpImg_CheckedChanged);
			this.ChbDecodeOKFtpImg.AutoSize = true;
			this.ChbDecodeOKFtpImg.Location = new System.Drawing.Point(34, 47);
			this.ChbDecodeOKFtpImg.Name = "ChbDecodeOKFtpImg";
			this.ChbDecodeOKFtpImg.Size = new System.Drawing.Size(159, 18);
			this.ChbDecodeOKFtpImg.TabIndex = 51;
			this.ChbDecodeOKFtpImg.Text = "解码成功FTP保存图片";
			this.ChbDecodeOKFtpImg.UseVisualStyleBackColor = true;
			this.ChbDecodeOKFtpImg.CheckedChanged += new System.EventHandler(ChbDecodeOKFtpImg_CheckedChanged);
			this.ChbDecodeNGSendImg.AutoSize = true;
			this.ChbDecodeNGSendImg.Location = new System.Drawing.Point(235, 70);
			this.ChbDecodeNGSendImg.Name = "ChbDecodeNGSendImg";
			this.ChbDecodeNGSendImg.Size = new System.Drawing.Size(138, 18);
			this.ChbDecodeNGSendImg.TabIndex = 50;
			this.ChbDecodeNGSendImg.Text = "解码失败发送图片";
			this.ChbDecodeNGSendImg.UseVisualStyleBackColor = true;
			this.ChbDecodeNGSendImg.CheckedChanged += new System.EventHandler(ChbDecodeNGSendImg_CheckedChanged);
			this.ChbDecodeOKSendImg.AutoSize = true;
			this.ChbDecodeOKSendImg.Location = new System.Drawing.Point(34, 70);
			this.ChbDecodeOKSendImg.Name = "ChbDecodeOKSendImg";
			this.ChbDecodeOKSendImg.Size = new System.Drawing.Size(138, 18);
			this.ChbDecodeOKSendImg.TabIndex = 49;
			this.ChbDecodeOKSendImg.Text = "解码成功发送图片";
			this.ChbDecodeOKSendImg.UseVisualStyleBackColor = true;
			this.ChbDecodeOKSendImg.CheckedChanged += new System.EventHandler(ChbDecodeOKSendImg_CheckedChanged);
			this.ChbDecodeNGSaveImg.AutoSize = true;
			this.ChbDecodeNGSaveImg.Location = new System.Drawing.Point(235, 24);
			this.ChbDecodeNGSaveImg.Name = "ChbDecodeNGSaveImg";
			this.ChbDecodeNGSaveImg.Size = new System.Drawing.Size(166, 18);
			this.ChbDecodeNGSaveImg.TabIndex = 48;
			this.ChbDecodeNGSaveImg.Text = "解码失败设备保存图片";
			this.ChbDecodeNGSaveImg.UseVisualStyleBackColor = true;
			this.ChbDecodeNGSaveImg.CheckedChanged += new System.EventHandler(ChbDecodeNGSaveImg_CheckedChanged);
			this.ChbDecodeOKSaveImg.AutoSize = true;
			this.ChbDecodeOKSaveImg.Location = new System.Drawing.Point(34, 24);
			this.ChbDecodeOKSaveImg.Name = "ChbDecodeOKSaveImg";
			this.ChbDecodeOKSaveImg.Size = new System.Drawing.Size(166, 18);
			this.ChbDecodeOKSaveImg.TabIndex = 47;
			this.ChbDecodeOKSaveImg.Text = "解码成功设备保存图片";
			this.ChbDecodeOKSaveImg.UseVisualStyleBackColor = true;
			this.ChbDecodeOKSaveImg.CheckedChanged += new System.EventHandler(ChbDecodeOKSaveImg_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(735, 689);
			base.Controls.Add(this.GrbImageDataProcess);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "ImageSaveSettingForm";
			this.Text = "图像保存";
			base.Load += new System.EventHandler(ImageSaveSettingForm_Load);
			this.GrbImageDataProcess.ResumeLayout(false);
			this.GrbImageDataProcess.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
