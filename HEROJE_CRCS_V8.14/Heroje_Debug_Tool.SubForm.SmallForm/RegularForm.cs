using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using HJ_Controls_Lib;

namespace Heroje_Debug_Tool.SubForm.SmallForm
{
	public class RegularForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		public string REGEX_OUTPUT_STRING = "output_regex.txt";

		public string REGEX_BARCODE_STRING = "barcode_regex.txt";

		public string Regex_txt_path = "";

		private string remote_ftp_path = "";

		private IContainer components = null;

		private RichTextBox TxbRegularText;

		private Button BtnOk;

		private Button BtnCancel;

		private Panel panel1;

		private UCRadioButton RdbCloseRegular;

		private UCRadioButton RdbOpenRegular;

		public RegularForm()
		{
			InitializeComponent();
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB, string FormName)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				Text = FormName + " 编辑规则";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				Text = FormName + " 編輯規則";
			}
			else
			{
				Text = FormName + " Rules Editing";
			}
		}

		private void RegularForm_Load(object sender, EventArgs e)
		{
			try
			{
				RdbCloseRegular.Checked = true;
				remote_ftp_path = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/" + Regex_txt_path;
				byte[] array = ToolCfg.ftp.DownloadFile(remote_ftp_path);
				if (array != null)
				{
					TxbRegularText.Text = Encoding.Default.GetString(array);
					RdbOpenRegular.Checked = true;
				}
				TxbRegularText.Enabled = RdbOpenRegular.Checked;
			}
			catch (Exception)
			{
			}
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				RdbCloseRegular.TextValue = "关闭规则";
				RdbOpenRegular.TextValue = "开启规则";
				BtnOk.Text = "确定";
				BtnCancel.Text = "取消";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				RdbCloseRegular.TextValue = "關閉規則";
				RdbOpenRegular.TextValue = "開啟規則";
				BtnOk.Text = "確定";
				BtnCancel.Text = "取消";
			}
			else
			{
				RdbCloseRegular.TextValue = "Disable Rules";
				RdbOpenRegular.TextValue = "Enable Rules ";
				BtnOk.Text = "OK";
				BtnCancel.Text = "Cancel ";
			}
		}

		private bool IsValidRegex(string pattern)
		{
			try
			{
				Regex.Match("", pattern);
				return true;
			}
			catch (ArgumentException ex)
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS"))
				{
					MessageBox.Show(ex.Message, "提示（正则表达式语法错误）", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show(ex.Message, "提示（規則運算式語法錯誤）", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					MessageBox.Show(ex.Message, "Tips（The regular expression syntax is incorrect）", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				return false;
			}
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			try
			{
				string text = TxbRegularText.Text.Trim();
				if (text != "" && RdbOpenRegular.Checked)
				{
					if (IsValidRegex(text) && Regex_txt_path != "")
					{
						byte[] bytes = Encoding.Default.GetBytes(text);
						string regex_txt_path = Regex_txt_path;
						FileStream fileStream = new FileStream(regex_txt_path, FileMode.Create);
						fileStream.Write(bytes, 0, bytes.Length);
						fileStream.Close();
						ToolCfg.ftp.UploadFile(regex_txt_path, remote_ftp_path);
						SendCfgDataCBFuncCB(16777216u);
						Close();
					}
				}
				else
				{
					RdbCloseRegular.Checked = true;
					ToolCfg.ftp.DeleteFile("ftp://" + ToolCfg.CurrentDevice.IpAddrStr, Regex_txt_path);
					SendCfgDataCBFuncCB(16777216u);
					Close();
				}
			}
			catch (Exception)
			{
				Close();
			}
		}

		private void RdbCloseRegular_CheckedChangeEvent(object sender, EventArgs e)
		{
		}

		private void RdbOpenRegular_CheckedChangeEvent(object sender, EventArgs e)
		{
			TxbRegularText.Enabled = RdbOpenRegular.Checked;
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
			this.TxbRegularText = new System.Windows.Forms.RichTextBox();
			this.BtnOk = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.RdbCloseRegular = new HJ_Controls_Lib.UCRadioButton();
			this.RdbOpenRegular = new HJ_Controls_Lib.UCRadioButton();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.TxbRegularText.Font = new System.Drawing.Font("宋体", 10.5f);
			this.TxbRegularText.Location = new System.Drawing.Point(3, 48);
			this.TxbRegularText.Name = "TxbRegularText";
			this.TxbRegularText.Size = new System.Drawing.Size(389, 236);
			this.TxbRegularText.TabIndex = 0;
			this.TxbRegularText.Text = "";
			this.BtnOk.BackColor = System.Drawing.Color.FromArgb(17, 119, 197);
			this.BtnOk.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnOk.Font = new System.Drawing.Font("宋体", 10.5f);
			this.BtnOk.ForeColor = System.Drawing.Color.White;
			this.BtnOk.Location = new System.Drawing.Point(151, 3);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.Size = new System.Drawing.Size(87, 32);
			this.BtnOk.TabIndex = 1;
			this.BtnOk.Text = "确定";
			this.BtnOk.UseVisualStyleBackColor = false;
			this.BtnOk.Click += new System.EventHandler(BtnOk_Click);
			this.BtnCancel.Font = new System.Drawing.Font("宋体", 10.5f);
			this.BtnCancel.Location = new System.Drawing.Point(267, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(87, 32);
			this.BtnCancel.TabIndex = 2;
			this.BtnCancel.Text = "取消";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.BtnOk);
			this.panel1.Controls.Add(this.BtnCancel);
			this.panel1.Location = new System.Drawing.Point(3, 287);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(389, 40);
			this.panel1.TabIndex = 3;
			this.RdbCloseRegular.Checked = true;
			this.RdbCloseRegular.GroupName = null;
			this.RdbCloseRegular.Location = new System.Drawing.Point(12, 12);
			this.RdbCloseRegular.Name = "RdbCloseRegular";
			this.RdbCloseRegular.Size = new System.Drawing.Size(98, 30);
			this.RdbCloseRegular.TabIndex = 4;
			this.RdbCloseRegular.TextValue = "关闭规则";
			this.RdbCloseRegular.CheckedChangeEvent += new System.EventHandler(RdbCloseRegular_CheckedChangeEvent);
			this.RdbOpenRegular.Checked = false;
			this.RdbOpenRegular.GroupName = null;
			this.RdbOpenRegular.Location = new System.Drawing.Point(122, 12);
			this.RdbOpenRegular.Name = "RdbOpenRegular";
			this.RdbOpenRegular.Size = new System.Drawing.Size(98, 30);
			this.RdbOpenRegular.TabIndex = 5;
			this.RdbOpenRegular.TextValue = "开启规则";
			this.RdbOpenRegular.CheckedChangeEvent += new System.EventHandler(RdbOpenRegular_CheckedChangeEvent);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(400, 333);
			base.Controls.Add(this.RdbOpenRegular);
			base.Controls.Add(this.RdbCloseRegular);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.TxbRegularText);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RegularForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "编辑规则";
			base.Load += new System.EventHandler(RegularForm_Load);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
