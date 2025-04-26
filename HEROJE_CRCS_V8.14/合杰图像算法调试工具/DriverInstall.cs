using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace 合杰图像算法调试工具
{
	public class DriverInstall : Form
	{
		private delegate void ShowMessageHandler(string msg);

		private bool waite_install = false;

		private IContainer components = null;

		private TextBox TxtInfoDisp;

		private Button BtnClose;

		public DriverInstall()
		{
			InitializeComponent();
		}

		private void DriverInstall_Load(object sender, EventArgs e)
		{
			Process process = new Process();
			process.StartInfo.FileName = "DriverForWinUSB.exe";
			process.StartInfo.Arguments = "\"USB DPM Debug Interface\" \"heroje inc\" 47823 55287";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.CreateNoWindow = true;
			process.OutputDataReceived += p_OutputDataReceived;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			ThreadPool.QueueUserWorkItem(ExeThread, process);
		}

		private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			ShowMessage(e.Data);
		}

		private void ShowMessage(string msg)
		{
			if (base.InvokeRequired)
			{
				BeginInvoke(new ShowMessageHandler(ShowMessage), msg);
			}
			else if (msg != null)
			{
				TxtInfoDisp.AppendText(msg + Environment.NewLine);
				Application.DoEvents();
			}
		}

		private void ExeThread(object obj)
		{
			Process process = obj as Process;
			try
			{
				waite_install = true;
				process.Start();
				process.BeginOutputReadLine();
				Application.DoEvents();
				process.WaitForExit();
				process.Close();
				waite_install = false;
			}
			catch (Exception ex2)
			{
				Exception ex3 = ex2;
				Exception ex = ex3;
				Invoke((MethodInvoker)delegate
				{
					bool flag = true;
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("当前用户貌似获取不到管理员权限，请尝试用zidag.exe手动安装。", "提示", MessageBoxButtons.OK);
					}
					else
					{
						MessageBox.Show("Administrator permission is required to install the driver,\r\nPlease try zidag.exe", "Tips", MessageBoxButtons.OK);
					}
					Close();
				});
			}
		}

		private void BtnClose_Click(object sender, EventArgs e)
		{
			if (waite_install)
			{
				if (MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.DriverInstallExitTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.DriverInstallExitTips), MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					Close();
				}
			}
			else
			{
				Close();
			}
		}

		private void DriverInstall_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (waite_install && MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.DriverInstallExitTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.DriverInstallExitTips), MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				e.Cancel = true;
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
			this.TxtInfoDisp = new System.Windows.Forms.TextBox();
			this.BtnClose = new System.Windows.Forms.Button();
			base.SuspendLayout();
			this.TxtInfoDisp.Location = new System.Drawing.Point(12, 12);
			this.TxtInfoDisp.Multiline = true;
			this.TxtInfoDisp.Name = "TxtInfoDisp";
			this.TxtInfoDisp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxtInfoDisp.Size = new System.Drawing.Size(646, 338);
			this.TxtInfoDisp.TabIndex = 0;
			this.BtnClose.Location = new System.Drawing.Point(276, 356);
			this.BtnClose.Name = "BtnClose";
			this.BtnClose.Size = new System.Drawing.Size(89, 31);
			this.BtnClose.TabIndex = 1;
			this.BtnClose.Text = "Close";
			this.BtnClose.UseVisualStyleBackColor = true;
			this.BtnClose.Click += new System.EventHandler(BtnClose_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(670, 399);
			base.Controls.Add(this.BtnClose);
			base.Controls.Add(this.TxtInfoDisp);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DriverInstall";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DriverInstall";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DriverInstall_FormClosing);
			base.Load += new System.EventHandler(DriverInstall_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
