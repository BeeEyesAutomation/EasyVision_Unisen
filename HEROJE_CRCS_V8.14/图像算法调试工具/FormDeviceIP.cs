using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using Heroje_Debug_Tool.SubForm;
using 合杰图像算法调试工具;

namespace 图像算法调试工具
{
	public class FormDeviceIP : Form
	{
		public delegate void ChangeIpByBrocastCB(byte[] dat, int num);

		private GetCfgCB GetCfgCBFuncCB;

		private ChangeIpByBrocastCB ChangeIpByBrocastCBFunc;

		public string SelectedDevIp;

		public string SelectedDevMac;

		private bool is_chagne_en = false;

		private IContainer components = null;

		private Button BtnMaskAddrSet;

		private Panel PanGateway;

		private TextBox TxbGwAddrSeg3;

		private TextBox TxbGwAddrSeg2;

		private TextBox TxbGwAddrSeg1;

		private TextBox TxbGwAddrSeg0;

		private Label LabGatewayAddr;

		private Panel PanIpAddr;

		private TextBox TxbIpAddrSeg3;

		private TextBox TxbIpAddrSeg2;

		private TextBox TxbIpAddrSeg1;

		private TextBox TxbIpAddrSeg0;

		private Label LabIPADDR;

		private Panel PanDhcpGetIp;

		private RadioButton RdbDhcpDisable;

		private RadioButton RdbDhcpEnable;

		private Label LabDhcpState;

		public FormDeviceIP()
		{
			InitializeComponent();
		}

		public FormDeviceIP(GetCfgCB getCfgCB, ChangeIpByBrocastCB call)
		{
			GetCfgCBFuncCB = getCfgCB;
			ChangeIpByBrocastCBFunc = call;
			InitializeComponent();
		}

		private void FormDeviceIP_Load(object sender, EventArgs e)
		{
			ControlAndXML controlAndXML = new ControlAndXML();
			controlAndXML.XMLToControlByLinq(ToolCfg.ConfigPath, this);
			RdbDhcpEnable.Checked = GetCfgCBFuncCB(131120u) == 16;
			IPAddress address;
			if (SelectedDevIp != null && SelectedDevIp != "" && IPAddress.TryParse(SelectedDevIp, out address))
			{
				byte[] addressBytes = address.GetAddressBytes();
				TxbIpAddrSeg0.Text = addressBytes[0].ToString();
				TxbIpAddrSeg1.Text = addressBytes[1].ToString();
				TxbIpAddrSeg2.Text = addressBytes[2].ToString();
				TxbIpAddrSeg3.Text = addressBytes[3].ToString();
				TxbGwAddrSeg0.Text = addressBytes[0].ToString();
				TxbGwAddrSeg1.Text = addressBytes[1].ToString();
				TxbGwAddrSeg2.Text = addressBytes[2].ToString();
				TxbGwAddrSeg3.Text = "1";
			}
			else
			{
				TxbIpAddrSeg0.Text = GetCfgCBFuncCB(131839u).ToString();
				TxbIpAddrSeg1.Text = GetCfgCBFuncCB(132095u).ToString();
				TxbIpAddrSeg2.Text = GetCfgCBFuncCB(132351u).ToString();
				TxbIpAddrSeg3.Text = GetCfgCBFuncCB(132607u).ToString();
				TxbGwAddrSeg0.Text = GetCfgCBFuncCB(132863u).ToString();
				TxbGwAddrSeg1.Text = GetCfgCBFuncCB(133119u).ToString();
				TxbGwAddrSeg2.Text = GetCfgCBFuncCB(133375u).ToString();
				TxbGwAddrSeg3.Text = GetCfgCBFuncCB(133631u).ToString();
			}
			is_chagne_en = true;
		}

		private void BtnMaskAddrSet_Click(object sender, EventArgs e)
		{
			byte[] array = new byte[16]
			{
				128, 46, 46, 128, 4, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0
			};
			bool flag = true;
			if (RdbDhcpEnable.Checked)
			{
				array[5] |= 1;
			}
			else
			{
				array[5] &= 254;
			}
			try
			{
				array[6] = byte.Parse(SelectedDevMac.Substring(12, 2), NumberStyles.HexNumber);
				array[7] = byte.Parse(SelectedDevMac.Substring(15, 2), NumberStyles.HexNumber);
			}
			catch
			{
				array[6] = 0;
				array[7] = 0;
			}
			try
			{
				array[8] = byte.Parse(TxbIpAddrSeg0.Text);
				array[9] = byte.Parse(TxbIpAddrSeg1.Text);
				array[10] = byte.Parse(TxbIpAddrSeg2.Text);
				array[11] = byte.Parse(TxbIpAddrSeg3.Text);
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的IP地址", "错误");
					flag = false;
				}
				else
				{
					MessageBox.Show("Please enter the correct IP address", "Error");
				}
			}
			try
			{
				array[12] = byte.Parse(TxbGwAddrSeg0.Text);
				array[13] = byte.Parse(TxbGwAddrSeg1.Text);
				array[14] = byte.Parse(TxbGwAddrSeg2.Text);
				array[15] = byte.Parse(TxbGwAddrSeg3.Text);
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的网关地址", "错误");
					flag = false;
				}
				else
				{
					MessageBox.Show("Please enter the correct GateWay address", "Error");
				}
			}
			if (flag)
			{
				string text = TxbIpAddrSeg0.Text.Trim() + "." + TxbIpAddrSeg1.Text.Trim() + "." + TxbIpAddrSeg2.Text.Trim() + "." + TxbIpAddrSeg3.Text.Trim();
				ChangeIpByBrocastCBFunc(array, 1);
				Thread.Sleep(300);
				ChangeIpByBrocastCBFunc(array, 2);
				Thread.Sleep(300);
				ChangeIpByBrocastCBFunc(array, 3);
				Close();
			}
		}

		private void RdbDhcpEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (RdbDhcpEnable.Checked)
			{
				PanIpAddr.Enabled = false;
				PanGateway.Enabled = false;
			}
			else
			{
				PanIpAddr.Enabled = true;
				PanGateway.Enabled = true;
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
			this.BtnMaskAddrSet = new System.Windows.Forms.Button();
			this.PanGateway = new System.Windows.Forms.Panel();
			this.TxbGwAddrSeg3 = new System.Windows.Forms.TextBox();
			this.TxbGwAddrSeg2 = new System.Windows.Forms.TextBox();
			this.TxbGwAddrSeg1 = new System.Windows.Forms.TextBox();
			this.TxbGwAddrSeg0 = new System.Windows.Forms.TextBox();
			this.LabGatewayAddr = new System.Windows.Forms.Label();
			this.PanIpAddr = new System.Windows.Forms.Panel();
			this.TxbIpAddrSeg3 = new System.Windows.Forms.TextBox();
			this.TxbIpAddrSeg2 = new System.Windows.Forms.TextBox();
			this.TxbIpAddrSeg1 = new System.Windows.Forms.TextBox();
			this.TxbIpAddrSeg0 = new System.Windows.Forms.TextBox();
			this.LabIPADDR = new System.Windows.Forms.Label();
			this.PanDhcpGetIp = new System.Windows.Forms.Panel();
			this.RdbDhcpDisable = new System.Windows.Forms.RadioButton();
			this.RdbDhcpEnable = new System.Windows.Forms.RadioButton();
			this.LabDhcpState = new System.Windows.Forms.Label();
			this.PanGateway.SuspendLayout();
			this.PanIpAddr.SuspendLayout();
			this.PanDhcpGetIp.SuspendLayout();
			base.SuspendLayout();
			this.BtnMaskAddrSet.Location = new System.Drawing.Point(102, 115);
			this.BtnMaskAddrSet.Name = "BtnMaskAddrSet";
			this.BtnMaskAddrSet.Size = new System.Drawing.Size(100, 38);
			this.BtnMaskAddrSet.TabIndex = 39;
			this.BtnMaskAddrSet.Text = "确认";
			this.BtnMaskAddrSet.UseVisualStyleBackColor = true;
			this.BtnMaskAddrSet.Click += new System.EventHandler(BtnMaskAddrSet_Click);
			this.PanGateway.Controls.Add(this.TxbGwAddrSeg3);
			this.PanGateway.Controls.Add(this.TxbGwAddrSeg2);
			this.PanGateway.Controls.Add(this.TxbGwAddrSeg1);
			this.PanGateway.Controls.Add(this.TxbGwAddrSeg0);
			this.PanGateway.Controls.Add(this.LabGatewayAddr);
			this.PanGateway.Location = new System.Drawing.Point(15, 51);
			this.PanGateway.Name = "PanGateway";
			this.PanGateway.Size = new System.Drawing.Size(287, 29);
			this.PanGateway.TabIndex = 21;
			this.TxbGwAddrSeg3.Location = new System.Drawing.Point(239, 2);
			this.TxbGwAddrSeg3.MaxLength = 3;
			this.TxbGwAddrSeg3.Name = "TxbGwAddrSeg3";
			this.TxbGwAddrSeg3.Size = new System.Drawing.Size(37, 23);
			this.TxbGwAddrSeg3.TabIndex = 12;
			this.TxbGwAddrSeg3.Text = "1";
			this.TxbGwAddrSeg3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbGwAddrSeg2.Location = new System.Drawing.Point(195, 2);
			this.TxbGwAddrSeg2.MaxLength = 3;
			this.TxbGwAddrSeg2.Name = "TxbGwAddrSeg2";
			this.TxbGwAddrSeg2.Size = new System.Drawing.Size(37, 23);
			this.TxbGwAddrSeg2.TabIndex = 11;
			this.TxbGwAddrSeg2.Text = "1";
			this.TxbGwAddrSeg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbGwAddrSeg1.Location = new System.Drawing.Point(150, 2);
			this.TxbGwAddrSeg1.MaxLength = 3;
			this.TxbGwAddrSeg1.Name = "TxbGwAddrSeg1";
			this.TxbGwAddrSeg1.Size = new System.Drawing.Size(37, 23);
			this.TxbGwAddrSeg1.TabIndex = 10;
			this.TxbGwAddrSeg1.Text = "168";
			this.TxbGwAddrSeg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbGwAddrSeg0.Location = new System.Drawing.Point(106, 2);
			this.TxbGwAddrSeg0.MaxLength = 3;
			this.TxbGwAddrSeg0.Name = "TxbGwAddrSeg0";
			this.TxbGwAddrSeg0.Size = new System.Drawing.Size(37, 23);
			this.TxbGwAddrSeg0.TabIndex = 9;
			this.TxbGwAddrSeg0.Text = "192";
			this.TxbGwAddrSeg0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.LabGatewayAddr.AutoSize = true;
			this.LabGatewayAddr.Location = new System.Drawing.Point(3, 7);
			this.LabGatewayAddr.Name = "LabGatewayAddr";
			this.LabGatewayAddr.Size = new System.Drawing.Size(105, 14);
			this.LabGatewayAddr.TabIndex = 6;
			this.LabGatewayAddr.Text = "网关地址设置：";
			this.PanIpAddr.Controls.Add(this.TxbIpAddrSeg3);
			this.PanIpAddr.Controls.Add(this.TxbIpAddrSeg2);
			this.PanIpAddr.Controls.Add(this.TxbIpAddrSeg1);
			this.PanIpAddr.Controls.Add(this.TxbIpAddrSeg0);
			this.PanIpAddr.Controls.Add(this.LabIPADDR);
			this.PanIpAddr.Location = new System.Drawing.Point(15, 17);
			this.PanIpAddr.Name = "PanIpAddr";
			this.PanIpAddr.Size = new System.Drawing.Size(287, 29);
			this.PanIpAddr.TabIndex = 19;
			this.TxbIpAddrSeg3.Location = new System.Drawing.Point(239, 2);
			this.TxbIpAddrSeg3.MaxLength = 3;
			this.TxbIpAddrSeg3.Name = "TxbIpAddrSeg3";
			this.TxbIpAddrSeg3.Size = new System.Drawing.Size(37, 23);
			this.TxbIpAddrSeg3.TabIndex = 12;
			this.TxbIpAddrSeg3.Text = "100";
			this.TxbIpAddrSeg3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbIpAddrSeg2.Location = new System.Drawing.Point(195, 2);
			this.TxbIpAddrSeg2.MaxLength = 3;
			this.TxbIpAddrSeg2.Name = "TxbIpAddrSeg2";
			this.TxbIpAddrSeg2.Size = new System.Drawing.Size(37, 23);
			this.TxbIpAddrSeg2.TabIndex = 11;
			this.TxbIpAddrSeg2.Text = "1";
			this.TxbIpAddrSeg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbIpAddrSeg1.Location = new System.Drawing.Point(150, 2);
			this.TxbIpAddrSeg1.MaxLength = 3;
			this.TxbIpAddrSeg1.Name = "TxbIpAddrSeg1";
			this.TxbIpAddrSeg1.Size = new System.Drawing.Size(37, 23);
			this.TxbIpAddrSeg1.TabIndex = 10;
			this.TxbIpAddrSeg1.Text = "168";
			this.TxbIpAddrSeg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbIpAddrSeg0.Location = new System.Drawing.Point(106, 2);
			this.TxbIpAddrSeg0.MaxLength = 3;
			this.TxbIpAddrSeg0.Name = "TxbIpAddrSeg0";
			this.TxbIpAddrSeg0.Size = new System.Drawing.Size(37, 23);
			this.TxbIpAddrSeg0.TabIndex = 9;
			this.TxbIpAddrSeg0.Text = "192";
			this.TxbIpAddrSeg0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.LabIPADDR.AutoSize = true;
			this.LabIPADDR.Location = new System.Drawing.Point(3, 7);
			this.LabIPADDR.Name = "LabIPADDR";
			this.LabIPADDR.Size = new System.Drawing.Size(91, 14);
			this.LabIPADDR.TabIndex = 6;
			this.LabIPADDR.Text = "IP地址设置：";
			this.PanDhcpGetIp.Controls.Add(this.RdbDhcpDisable);
			this.PanDhcpGetIp.Controls.Add(this.RdbDhcpEnable);
			this.PanDhcpGetIp.Controls.Add(this.LabDhcpState);
			this.PanDhcpGetIp.Enabled = false;
			this.PanDhcpGetIp.Location = new System.Drawing.Point(15, 86);
			this.PanDhcpGetIp.Name = "PanDhcpGetIp";
			this.PanDhcpGetIp.Size = new System.Drawing.Size(287, 23);
			this.PanDhcpGetIp.TabIndex = 40;
			this.RdbDhcpDisable.AutoSize = true;
			this.RdbDhcpDisable.Checked = true;
			this.RdbDhcpDisable.Location = new System.Drawing.Point(215, 3);
			this.RdbDhcpDisable.Name = "RdbDhcpDisable";
			this.RdbDhcpDisable.Size = new System.Drawing.Size(53, 18);
			this.RdbDhcpDisable.TabIndex = 2;
			this.RdbDhcpDisable.TabStop = true;
			this.RdbDhcpDisable.Text = "关闭";
			this.RdbDhcpDisable.UseVisualStyleBackColor = true;
			this.RdbDhcpEnable.AutoSize = true;
			this.RdbDhcpEnable.Location = new System.Drawing.Point(154, 3);
			this.RdbDhcpEnable.Name = "RdbDhcpEnable";
			this.RdbDhcpEnable.Size = new System.Drawing.Size(53, 18);
			this.RdbDhcpEnable.TabIndex = 1;
			this.RdbDhcpEnable.Text = "开启";
			this.RdbDhcpEnable.UseVisualStyleBackColor = true;
			this.RdbDhcpEnable.CheckedChanged += new System.EventHandler(RdbDhcpEnable_CheckedChanged);
			this.LabDhcpState.AutoSize = true;
			this.LabDhcpState.Location = new System.Drawing.Point(3, 5);
			this.LabDhcpState.Name = "LabDhcpState";
			this.LabDhcpState.Size = new System.Drawing.Size(140, 14);
			this.LabDhcpState.TabIndex = 0;
			this.LabDhcpState.Text = "DHCP自动获取IP开关:";
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(321, 162);
			base.Controls.Add(this.PanDhcpGetIp);
			base.Controls.Add(this.BtnMaskAddrSet);
			base.Controls.Add(this.PanGateway);
			base.Controls.Add(this.PanIpAddr);
			this.Font = new System.Drawing.Font("宋体", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FormDeviceIP";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "IP设置";
			base.Load += new System.EventHandler(FormDeviceIP_Load);
			this.PanGateway.ResumeLayout(false);
			this.PanGateway.PerformLayout();
			this.PanIpAddr.ResumeLayout(false);
			this.PanIpAddr.PerformLayout();
			this.PanDhcpGetIp.ResumeLayout(false);
			this.PanDhcpGetIp.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
