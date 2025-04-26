using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Timers;
using System.Windows.Forms;
using Communication_N;
using Heroje_Debug_Tool.BaseClass;
using Heroje_Debug_Tool.SubForm;

namespace Heroje_Debug_Tool.SubTool
{
	public class ShowFirmwareVerForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private bool IsContinueMode = false;

		private string TcpTargetIP = "192.168.1.91";

		private string TcpTargetPort = "1436";

		private Tcp_Method TcpClientWay;

		private bool IsTcpClientOpen = false;

		private byte[] DataReceive;

		private System.Timers.Timer DisconnectTimer;

		private IContainer components = null;

		private Label LabVersion;

		private Panel PanHeroJe;

		private Label LabCopyRight;

		public ShowFirmwareVerForm()
		{
			InitializeComponent();
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
		}

		private void PanHeroJe_MouseClick(object sender, MouseEventArgs e)
		{
			Process.Start("http://www.heroje.com");
		}

		private void ShowFirmwareVerForm_Load(object sender, EventArgs e)
		{
			if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Heroje_Standard)
			{
				PanHeroJe.Visible = true;
			}
			else if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Hide_Heroje_Logo)
			{
				PanHeroJe.Visible = false;
			}
			Text = MultLanguageText.Get_Title(MultLanguageText.TextDef.FirmWarePageText);
			LabVersion.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.FirmWarePageText) + "——";
			try
			{
				if (GetCfgCBFuncCB(3u) == 2)
				{
					IsContinueMode = true;
					SetCfgCBFuncCB(3u, 0u);
					SendCfgDataCBFuncCB(64u);
				}
				if (ToolCfg.CurrentDevice != null)
				{
					byte[] byteDataSend = new byte[9] { 126, 0, 9, 255, 0, 217, 6, 44, 166 };
					TcpTargetIP = ToolCfg.CurrentDevice.IpAddrStr;
					ConnectTcpServer();
					if (IsTcpClientOpen)
					{
						DisconnectTimer = new System.Timers.Timer(500.0);
						DisconnectTimer.Elapsed += DisconnectTimer_Elapsed;
						DisconnectTimer.Start();
						int num = TcpClientWay.SendData(byteDataSend);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void ConnectTcpServer()
		{
			try
			{
				if (TcpTargetIP != "" && TcpTargetPort != "")
				{
					IPAddress remoteIp = IPAddress.Parse(TcpTargetIP);
					int remotePort = Convert.ToInt32(TcpTargetPort);
					TcpClientWay = new Tcp_Method(remotePort, remoteIp, CommCallBack);
					if (TcpClientWay.IsOpenSuccess)
					{
						IsTcpClientOpen = true;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void DisconnectTcpServer()
		{
			try
			{
				if (TcpClientWay != null)
				{
					TcpClientWay.close();
				}
				IsTcpClientOpen = false;
			}
			catch (Exception)
			{
			}
		}

		private void CommCallBack(int num, string str, int CommWay)
		{
			try
			{
				if (num != 1 && num == 2)
				{
					string VersionInfo = str.Replace("D_", " ").Replace("_L", " ");
					VersionInfo = VersionInfo.Replace("_L", " ");
					VersionInfo = VersionInfo.Replace("\r\n", " ");
					VersionInfo = VersionInfo.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0];
					LabVersion.Invoke((MethodInvoker)delegate
					{
						Label labVersion = LabVersion;
						string text2 = (LabVersion.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.FirmWarePageText) + VersionInfo);
						labVersion.Text = text2;
					});
				}
			}
			catch (Exception)
			{
			}
		}

		private void DisconnectTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			DisconnectTimer.Stop();
			DisconnectTimer.Dispose();
			DisconnectTcpServer();
			if (IsContinueMode)
			{
				SetCfgCBFuncCB(3u, 2u);
				SendCfgDataCBFuncCB(64u);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Heroje_Debug_Tool.SubTool.ShowFirmwareVerForm));
			this.LabVersion = new System.Windows.Forms.Label();
			this.LabCopyRight = new System.Windows.Forms.Label();
			this.PanHeroJe = new System.Windows.Forms.Panel();
			base.SuspendLayout();
			this.LabVersion.Font = new System.Drawing.Font("宋体", 20f);
			this.LabVersion.Location = new System.Drawing.Point(24, 72);
			this.LabVersion.Name = "LabVersion";
			this.LabVersion.Size = new System.Drawing.Size(522, 39);
			this.LabVersion.TabIndex = 12;
			this.LabVersion.Text = "解码设备固件版本：——";
			this.LabVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LabCopyRight.Font = new System.Drawing.Font("宋体", 20f);
			this.LabCopyRight.Location = new System.Drawing.Point(181, 116);
			this.LabCopyRight.Name = "LabCopyRight";
			this.LabCopyRight.Size = new System.Drawing.Size(208, 27);
			this.LabCopyRight.TabIndex = 13;
			this.LabCopyRight.Text = "CopyRight®2020";
			this.LabCopyRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.PanHeroJe.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.PanHeroJe.BackColor = System.Drawing.Color.Transparent;
			this.PanHeroJe.BackgroundImage = (System.Drawing.Image)resources.GetObject("PanHeroJe.BackgroundImage");
			this.PanHeroJe.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.PanHeroJe.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PanHeroJe.Location = new System.Drawing.Point(198, 12);
			this.PanHeroJe.Name = "PanHeroJe";
			this.PanHeroJe.Size = new System.Drawing.Size(174, 56);
			this.PanHeroJe.TabIndex = 14;
			this.PanHeroJe.MouseClick += new System.Windows.Forms.MouseEventHandler(PanHeroJe_MouseClick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(574, 190);
			base.Controls.Add(this.PanHeroJe);
			base.Controls.Add(this.LabCopyRight);
			base.Controls.Add(this.LabVersion);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ShowFirmwareVerForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "设备固件信息";
			base.Load += new System.EventHandler(ShowFirmwareVerForm_Load);
			base.ResumeLayout(false);
		}
	}
}
