using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace Heroje_Debug_Tool.SubForm
{
	public class CommunicateSettingForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private IContainer components = null;

		private GroupBox GrbOutputUSB;

		private RadioButton RdbOuputByUart;

		private RadioButton RdbOuputByCOM;

		private RadioButton RdbOuputByHID;

		private GroupBox GrbOutputInternet;

		private CheckBox ChbTcpOnlyConnectNew;

		private CheckBox ChbTcpUseNewVer;

		private CheckBox ChbNetworkLossDetect;

		private Label LabNetWorkSettingTips;

		private Button BtnTcpIPAddrSet;

		private Button BtnNetPortSet;

		private TextBox TxbSIPAddrSeg3;

		private TextBox TxbSIPAddrSeg2;

		private TextBox TxbSIPAddrSeg1;

		private TextBox TxbSIPAddrSeg0;

		private Label LabTCPServerIP;

		private TextBox TxbComPort;

		private Label LabComPort;

		private ComboBox CobComSel;

		private Label LabComSel;

		private Panel PanMaskCode;

		private Button BtnMaskAddrSet;

		private TextBox TxbMaskCodeSeg3;

		private TextBox TxbMaskCodeSeg2;

		private TextBox TxbMaskCodeSeg1;

		private TextBox TxbMaskCodeSeg0;

		private Label LabMaskCode;

		private Panel PanGateway;

		private Button BtnGWAddrSet;

		private TextBox TxbGwAddrSeg3;

		private TextBox TxbGwAddrSeg2;

		private TextBox TxbGwAddrSeg1;

		private TextBox TxbGwAddrSeg0;

		private Label LabGatewayAddr;

		private Panel PanIpAddr;

		private Button BtnIPAddrSet;

		private TextBox TxbIpAddrSeg3;

		private TextBox TxbIpAddrSeg2;

		private TextBox TxbIpAddrSeg1;

		private TextBox TxbIpAddrSeg0;

		private Label LabIPADDR;

		private Panel PanDhcpAutoIp;

		private RadioButton RdbDhcpDisable;

		private RadioButton RdbDhcpEnable;

		private Label LabDhcpState;

		private GroupBox GrbEncoderSetting;

		private RadioButton RdbOutputUTF8;

		private RadioButton RdbOutputUnicode;

		private RadioButton RdbOutputGBK;

		private GroupBox GrbOutputRS232;

		private ComboBox CobUartStopBit;

		private Label LabUartStopBit;

		private ComboBox CobUartDataBit;

		private Label LabUartDataBit;

		private ComboBox CobUartPolarity;

		private Label LabUartCheckPolarity;

		private ComboBox CobBaudrateSet;

		private Label LabBaudrateSet;

		private TabControl TabProtocol;

		private TabPage TapModbus;

		private Label LabMB_Tips;

		private Button BtnMB_TcpPort;

		private TextBox TxbMB_TcpPort;

		private Label LabMB_TcpPort;

		private Button BtnMB_DeviceAddr;

		private TextBox TxbMB_DeviceAddr;

		private Label LabMBAddr;

		private Label LabMB_Protocol;

		private ComboBox CobMB_Protocol;

		private TabPage TapEthernetIP;

		private Panel PanIdsProtocol;

		private CheckBox ChbEnableProfiNet;

		private CheckBox ChbEthernetIPEnable;

		private Label LabEtherNetTips_1;

		private TabPage TapMcPLC;

		private Label LabMcPLCTips;

		private Button BtnMC_Port;

		private TextBox TxbMC_Port;

		private Label LabMcPLCPort;

		private CheckBox ChbEnableMcPLC;

		private Label LabEtherNetTips_2;

		public CommunicateSettingForm()
		{
			InitializeComponent();
		}

		private void DataOutputSettingForm_Load(object sender, EventArgs e)
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
			int num = ((GetCfgCBFuncCB(11263u) << 8) + GetCfgCBFuncCB(11007u)) * 100;
			int num2 = CobBaudrateSet.Items.IndexOf(num.ToString());
			if (num2 > 0)
			{
				CobBaudrateSet.SelectedIndex = num2;
			}
			else
			{
				CobBaudrateSet.Text = num.ToString();
			}
			if (GetCfgCBFuncCB(3340u) == 4)
			{
				RdbOutputUnicode.Checked = true;
			}
			else if (GetCfgCBFuncCB(3340u) == 12)
			{
				RdbOutputUTF8.Checked = true;
			}
			else
			{
				RdbOutputGBK.Checked = true;
			}
			if (GetCfgCBFuncCB(3331u) == 1)
			{
				if (GetCfgCBFuncCB(55312u) == 16)
				{
					RdbOuputByUart.Checked = true;
				}
				else
				{
					RdbOuputByHID.Checked = true;
				}
			}
			else if (GetCfgCBFuncCB(3331u) == 3)
			{
				RdbOuputByCOM.Checked = true;
			}
			ToolCfg.is_RdbOuputByCOM_checked = RdbOuputByCOM.Checked;
			if (GetCfgCBFuncCB(59649u) == 0)
			{
				CobUartPolarity.SelectedIndex = 0;
			}
			else if (GetCfgCBFuncCB(59654u) == 2)
			{
				CobUartPolarity.SelectedIndex = 1;
			}
			else if (GetCfgCBFuncCB(59654u) == 0)
			{
				CobUartPolarity.SelectedIndex = 2;
			}
			if (GetCfgCBFuncCB(59744u) == 96)
			{
				CobUartDataBit.SelectedIndex = 0;
			}
			else if (GetCfgCBFuncCB(59744u) == 64)
			{
				CobUartDataBit.SelectedIndex = 1;
			}
			else if (GetCfgCBFuncCB(59744u) == 32)
			{
				CobUartDataBit.SelectedIndex = 2;
			}
			else if (GetCfgCBFuncCB(59744u) == 0)
			{
				CobUartDataBit.SelectedIndex = 3;
			}
			if (GetCfgCBFuncCB(59672u) == 0)
			{
				CobUartStopBit.SelectedIndex = 0;
			}
			else if (GetCfgCBFuncCB(59672u) == 8)
			{
				CobUartStopBit.SelectedIndex = 1;
			}
			else if (GetCfgCBFuncCB(59672u) == 16)
			{
				CobUartStopBit.SelectedIndex = 2;
			}
			TxbMB_DeviceAddr.Text = GetCfgCBFuncCB(86527u).ToString();
			int num3 = GetCfgCBFuncCB(88831u) + GetCfgCBFuncCB(89087u) * 256;
			TxbMB_TcpPort.Text = num3.ToString();
			if (GetCfgCBFuncCB(86019u) == 1)
			{
				CobMB_Protocol.SelectedIndex = 1;
			}
			else if (GetCfgCBFuncCB(86019u) == 3)
			{
				CobMB_Protocol.SelectedIndex = 2;
			}
			else
			{
				CobMB_Protocol.SelectedIndex = 0;
			}
			ChbTcpOnlyConnectNew.Visible = false;
			if (para.ParaDataLen >= 4096)
			{
				GrbOutputInternet.Enabled = true;
				RdbDhcpEnable.Checked = GetCfgCBFuncCB(131120u) == 16;
				TxbIpAddrSeg0.Text = GetCfgCBFuncCB(131839u).ToString();
				TxbIpAddrSeg1.Text = GetCfgCBFuncCB(132095u).ToString();
				TxbIpAddrSeg2.Text = GetCfgCBFuncCB(132351u).ToString();
				TxbIpAddrSeg3.Text = GetCfgCBFuncCB(132607u).ToString();
				TxbGwAddrSeg0.Text = GetCfgCBFuncCB(132863u).ToString();
				TxbGwAddrSeg1.Text = GetCfgCBFuncCB(133119u).ToString();
				TxbGwAddrSeg2.Text = GetCfgCBFuncCB(133375u).ToString();
				TxbGwAddrSeg3.Text = GetCfgCBFuncCB(133631u).ToString();
				TxbMaskCodeSeg0.Text = GetCfgCBFuncCB(133887u).ToString();
				TxbMaskCodeSeg1.Text = GetCfgCBFuncCB(134143u).ToString();
				TxbMaskCodeSeg2.Text = GetCfgCBFuncCB(134399u).ToString();
				TxbMaskCodeSeg3.Text = GetCfgCBFuncCB(134655u).ToString();
				if (GetCfgCBFuncCB(131079u) == 0)
				{
					CobComSel.SelectedIndex = 1;
				}
				else if (GetCfgCBFuncCB(131079u) == 2)
				{
					CobComSel.SelectedIndex = 2;
				}
				else
				{
					CobComSel.SelectedIndex = 0;
				}
				TxbComPort.Text = (GetCfgCBFuncCB(138239u) * 256 + GetCfgCBFuncCB(137983u)).ToString();
				TxbMC_Port.Text = (GetCfgCBFuncCB(89599u) * 256 + GetCfgCBFuncCB(89343u)).ToString();
				TxbSIPAddrSeg0.Text = GetCfgCBFuncCB(136959u).ToString();
				TxbSIPAddrSeg1.Text = GetCfgCBFuncCB(137215u).ToString();
				TxbSIPAddrSeg2.Text = GetCfgCBFuncCB(137471u).ToString();
				TxbSIPAddrSeg3.Text = GetCfgCBFuncCB(137727u).ToString();
				ChbNetworkLossDetect.Checked = GetCfgCBFuncCB(131200u) == 128;
				ChbEthernetIPEnable.Checked = GetCfgCBFuncCB(86048u) == 32;
				ChbEnableProfiNet.Checked = GetCfgCBFuncCB(86080u) == 64;
				ChbEnableMcPLC.Checked = GetCfgCBFuncCB(86144u) == 128;
				ChbTcpUseNewVer.Checked = GetCfgCBFuncCB(131344u) == 16;
				if (para.DeviceTypeRecord != 5 && para.DeviceTypeRecord != 13)
				{
					ChbNetworkLossDetect.Visible = false;
					ChbTcpOnlyConnectNew.Visible = true;
					ChbTcpOnlyConnectNew.Checked = GetCfgCBFuncCB(131360u) == 32;
				}
				else
				{
					ChbNetworkLossDetect.Visible = true;
				}
			}
			else
			{
				GrbOutputInternet.Enabled = false;
			}
			if (para.DeviceTypeRecord == 6 || para.DeviceTypeRecord == 9 || para.DeviceTypeRecord == 16 || para.DeviceTypeRecord == 10 || para.DeviceTypeRecord == 19 || para.DeviceTypeRecord == 13)
			{
				PanIdsProtocol.Enabled = true;
			}
			else
			{
				PanIdsProtocol.Enabled = false;
			}
			bool flag = true;
		}

		public void FunctionOnOff(bool[] CapacityArr)
		{
			GrbOutputInternet.Enabled = CapacityArr[16];
		}

		public void UpdateLanguageUI()
		{
			int selectedIndex = CobComSel.SelectedIndex;
			CobComSel.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobComSel.Items.AddRange(new object[3] { "TCP服务器", "TCP客户端", "UDP通信" });
			}
			else
			{
				CobComSel.Items.AddRange(new object[3] { "TCP Server", "TCP Client", "UDP" });
			}
			CobComSel.SelectedIndex = selectedIndex;
			int selectedIndex2 = CobUartPolarity.SelectedIndex;
			CobUartPolarity.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobUartPolarity.Items.AddRange(new object[3] { "无校验", "奇校验", "偶校验" });
			}
			else
			{
				CobUartPolarity.Items.AddRange(new object[3] { "None", "Odd", "Even" });
			}
			CobUartPolarity.SelectedIndex = selectedIndex2;
			CobMB_Protocol.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobMB_Protocol.Items.AddRange(new object[3] { "关闭Modbus协议输出", "Modbus-RTU", "Modbus-TCP" });
			}
			else
			{
				CobMB_Protocol.Items.AddRange(new object[3] { "Disable Modbus", "Modbus-RTU", "Modbus-TCP" });
			}
		}

		private void RdbOutputGBK_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOutputGBK.Checked)
			{
				SetCfgCBFuncCB(3340u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOutputUnicode_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOutputUnicode.Checked)
			{
				SetCfgCBFuncCB(3340u, 4u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOutputUTF8_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOutputUTF8.Checked)
			{
				SetCfgCBFuncCB(3340u, 12u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOuputByHID_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOutputGBK.Checked)
			{
				SetCfgCBFuncCB(55312u, 0u);
				SetCfgCBFuncCB(3331u, 1u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOuputByCOM_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOutputGBK.Checked)
			{
				SetCfgCBFuncCB(3331u, 3u);
				SendCfgDataCBFuncCB(0u);
			}
			ToolCfg.is_RdbOuputByCOM_checked = RdbOuputByCOM.Checked;
		}

		private void RdbOuputByUart_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOutputGBK.Checked)
			{
				SetCfgCBFuncCB(55312u, 16u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobBaudrateSet_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				int num = int.Parse(CobBaudrateSet.Text) / 100;
				SetCfgCBFuncCB(11007u, (ushort)((uint)num & 0xFFu));
				SetCfgCBFuncCB(11263u, (ushort)((num & 0xFF00) >> 8));
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobUartDataBit_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && CobUartDataBit.SelectedIndex >= 0)
			{
				if (CobUartDataBit.SelectedIndex == 0)
				{
					SetCfgCBFuncCB(59744u, 96u);
				}
				else if (CobUartDataBit.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(59744u, 64u);
				}
				else if (CobUartDataBit.SelectedIndex == 2)
				{
					SetCfgCBFuncCB(59744u, 32u);
				}
				else if (CobUartDataBit.SelectedIndex == 3)
				{
					SetCfgCBFuncCB(59744u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobUartPolarity_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && CobUartPolarity.SelectedIndex >= 0)
			{
				if (CobUartPolarity.SelectedIndex == 0)
				{
					SetCfgCBFuncCB(59649u, 0u);
				}
				else if (CobUartPolarity.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(59649u, 1u);
					SetCfgCBFuncCB(59654u, 2u);
				}
				else if (CobUartPolarity.SelectedIndex == 2)
				{
					SetCfgCBFuncCB(59649u, 1u);
					SetCfgCBFuncCB(59654u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobUartStopBit_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && CobUartStopBit.SelectedIndex >= 0)
			{
				if (CobUartStopBit.SelectedIndex == 0)
				{
					SetCfgCBFuncCB(59672u, 0u);
				}
				else if (CobUartStopBit.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(59672u, 8u);
				}
				else if (CobUartStopBit.SelectedIndex == 2)
				{
					SetCfgCBFuncCB(59672u, 16u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDhcpEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && ToolCfg.ParaDataLen >= 4096)
			{
				SetCfgCBFuncCB(131120u, 16u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbDhcpDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && ToolCfg.ParaDataLen >= 4096)
			{
				SetCfgCBFuncCB(131120u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbIpAddrSeg0_TextChanged(object sender, EventArgs e)
		{
			BtnIPAddrSet.Visible = true;
		}

		private void TxbIpAddrSeg1_TextChanged(object sender, EventArgs e)
		{
			BtnIPAddrSet.Visible = true;
		}

		private void TxbIpAddrSeg2_TextChanged(object sender, EventArgs e)
		{
			BtnIPAddrSet.Visible = true;
		}

		private void TxbIpAddrSeg3_TextChanged(object sender, EventArgs e)
		{
			BtnIPAddrSet.Visible = true;
		}

		private void BtnIPAddrSet_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			try
			{
				SetCfgCBFuncCB(131839u, uint.Parse(TxbIpAddrSeg0.Text));
				SetCfgCBFuncCB(132095u, uint.Parse(TxbIpAddrSeg1.Text));
				SetCfgCBFuncCB(132351u, uint.Parse(TxbIpAddrSeg2.Text));
				SetCfgCBFuncCB(132607u, uint.Parse(TxbIpAddrSeg3.Text));
				SendCfgDataCBFuncCB(16384u);
				BtnIPAddrSet.Visible = false;
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的IP地址", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct IP address", "Error");
				}
			}
		}

		private void TxbGwAddrSeg0_TextChanged(object sender, EventArgs e)
		{
			BtnGWAddrSet.Visible = true;
		}

		private void TxbGwAddrSeg1_TextChanged(object sender, EventArgs e)
		{
			BtnGWAddrSet.Visible = true;
		}

		private void TxbGwAddrSeg2_TextChanged(object sender, EventArgs e)
		{
			BtnGWAddrSet.Visible = true;
		}

		private void TxbGwAddrSeg3_TextChanged(object sender, EventArgs e)
		{
			BtnGWAddrSet.Visible = true;
		}

		private void BtnGWAddrSet_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			try
			{
				SetCfgCBFuncCB(132863u, uint.Parse(TxbGwAddrSeg0.Text));
				SetCfgCBFuncCB(133119u, uint.Parse(TxbGwAddrSeg1.Text));
				SetCfgCBFuncCB(133375u, uint.Parse(TxbGwAddrSeg2.Text));
				SetCfgCBFuncCB(133631u, uint.Parse(TxbGwAddrSeg3.Text));
				SendCfgDataCBFuncCB(16384u);
				BtnGWAddrSet.Visible = false;
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的网关地址", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct GateWay address", "Error");
				}
			}
		}

		private void TxbMaskCodeSeg0_TextChanged(object sender, EventArgs e)
		{
			BtnMaskAddrSet.Visible = true;
		}

		private void TxbMaskCodeSeg1_TextChanged(object sender, EventArgs e)
		{
			BtnMaskAddrSet.Visible = true;
		}

		private void TxbMaskCodeSeg2_TextChanged(object sender, EventArgs e)
		{
			BtnMaskAddrSet.Visible = true;
		}

		private void TxbMaskCodeSeg3_TextChanged(object sender, EventArgs e)
		{
			BtnMaskAddrSet.Visible = true;
		}

		private void BtnMaskAddrSet_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			try
			{
				SetCfgCBFuncCB(133887u, uint.Parse(TxbMaskCodeSeg0.Text));
				SetCfgCBFuncCB(134143u, uint.Parse(TxbMaskCodeSeg1.Text));
				SetCfgCBFuncCB(134399u, uint.Parse(TxbMaskCodeSeg2.Text));
				SetCfgCBFuncCB(134655u, uint.Parse(TxbMaskCodeSeg3.Text));
				SendCfgDataCBFuncCB(16384u);
				BtnMaskAddrSet.Visible = false;
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的子网掩码地址", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct subnet mask address", "Error");
				}
			}
		}

		private void CobComSel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (CobComSel.SelectedIndex == 0)
			{
				ChbTcpUseNewVer.Visible = true;
			}
			else
			{
				ChbTcpUseNewVer.Visible = false;
			}
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobComSel.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(131079u, 0u);
				}
				else if (CobComSel.SelectedIndex == 2)
				{
					SetCfgCBFuncCB(131079u, 2u);
				}
				else
				{
					SetCfgCBFuncCB(131079u, 1u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbComPort_TextChanged(object sender, EventArgs e)
		{
			BtnNetPortSet.Visible = true;
		}

		private void BtnNetPortSet_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result;
				if (ushort.TryParse(TxbComPort.Text, out result))
				{
					SetCfgCBFuncCB(138239u, (uint)(result >> 8) & 0xFFu);
					SetCfgCBFuncCB(137983u, result & 0xFFu);
					SendCfgDataCBFuncCB(0u);
					BtnNetPortSet.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的网络端口", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct net port", "Error");
				}
			}
		}

		private void TxbSIPAddrSeg0_TextChanged(object sender, EventArgs e)
		{
			BtnTcpIPAddrSet.Visible = true;
		}

		private void TxbSIPAddrSeg1_TextChanged(object sender, EventArgs e)
		{
			BtnTcpIPAddrSet.Visible = true;
		}

		private void TxbSIPAddrSeg2_TextChanged(object sender, EventArgs e)
		{
			BtnTcpIPAddrSet.Visible = true;
		}

		private void TxbSIPAddrSeg3_TextChanged(object sender, EventArgs e)
		{
			BtnTcpIPAddrSet.Visible = true;
		}

		private void BtnTcpIPAddrSet_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			try
			{
				SetCfgCBFuncCB(136959u, uint.Parse(TxbSIPAddrSeg0.Text));
				SetCfgCBFuncCB(137215u, uint.Parse(TxbSIPAddrSeg1.Text));
				SetCfgCBFuncCB(137471u, uint.Parse(TxbSIPAddrSeg2.Text));
				SetCfgCBFuncCB(137727u, uint.Parse(TxbSIPAddrSeg3.Text));
				SendCfgDataCBFuncCB(0u);
				BtnTcpIPAddrSet.Visible = false;
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的子网掩码地址", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct subnet mask address", "Error");
				}
			}
		}

		private void ChbTcpUseNewVer_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbTcpUseNewVer.Checked)
				{
					SetCfgCBFuncCB(131344u, 16u);
				}
				else
				{
					SetCfgCBFuncCB(131344u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbNetworkLossDetect_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbNetworkLossDetect.Checked)
				{
					SetCfgCBFuncCB(131200u, 128u);
				}
				else
				{
					SetCfgCBFuncCB(131200u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbTcpOnlyConnectNew_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbTcpOnlyConnectNew.Checked)
				{
					SetCfgCBFuncCB(131360u, 32u);
				}
				else
				{
					SetCfgCBFuncCB(131360u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobMB_Protocol_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				switch (CobMB_Protocol.SelectedIndex)
				{
				case 0:
					SetCfgCBFuncCB(86019u, 0u);
					break;
				case 1:
					SetCfgCBFuncCB(86019u, 1u);
					break;
				case 2:
					SetCfgCBFuncCB(86019u, 3u);
					break;
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbMB_DeviceAddr_TextChanged(object sender, EventArgs e)
		{
			BtnMB_DeviceAddr.Visible = true;
		}

		private void BtnMB_DeviceAddr_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbMB_DeviceAddr.Text, out result) && result < 255)
				{
					SetCfgCBFuncCB(86527u, (byte)(result & 0xFFu));
					SendCfgDataCBFuncCB(0u);
					BtnMB_DeviceAddr.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入小于255的数", "错误");
				}
				else
				{
					MessageBox.Show("Please enter a number less than 255", "Error");
				}
			}
		}

		private void TxbMB_TcpPort_TextChanged(object sender, EventArgs e)
		{
			BtnMB_TcpPort.Visible = true;
		}

		private void BtnMB_TcpPort_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbMB_TcpPort.Text, out result) && result < ushort.MaxValue)
				{
					SetCfgCBFuncCB(88831u, (byte)(result & 0xFFu));
					SetCfgCBFuncCB(89087u, (byte)((result & 0xFF00) >> 8));
					SendCfgDataCBFuncCB(0u);
					BtnMB_TcpPort.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入小于65535的数", "错误");
				}
				else
				{
					MessageBox.Show("Please enter a number less than 65535", "Error");
				}
			}
		}

		private void ChbEthernetIPEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbEthernetIPEnable.Checked)
				{
					SetCfgCBFuncCB(86048u, 32u);
				}
				else
				{
					SetCfgCBFuncCB(86048u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbEnableProfiNet_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbEnableProfiNet.Checked)
				{
					SetCfgCBFuncCB(86080u, 64u);
				}
				else
				{
					SetCfgCBFuncCB(86080u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbEnableMcPLC_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbEnableMcPLC.Checked)
				{
					SetCfgCBFuncCB(86144u, 128u);
				}
				else
				{
					SetCfgCBFuncCB(86144u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbMC_Port_TextChanged(object sender, EventArgs e)
		{
			BtnMC_Port.Visible = true;
		}

		private void BtnMC_Port_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result;
				if (ushort.TryParse(TxbMC_Port.Text, out result))
				{
					SetCfgCBFuncCB(89599u, (uint)(result >> 8) & 0xFFu);
					SetCfgCBFuncCB(89343u, result & 0xFFu);
					SendCfgDataCBFuncCB(0u);
					BtnMC_Port.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的网络端口", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct net port", "Error");
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
			this.GrbOutputUSB = new System.Windows.Forms.GroupBox();
			this.RdbOuputByUart = new System.Windows.Forms.RadioButton();
			this.RdbOuputByCOM = new System.Windows.Forms.RadioButton();
			this.RdbOuputByHID = new System.Windows.Forms.RadioButton();
			this.GrbOutputInternet = new System.Windows.Forms.GroupBox();
			this.ChbTcpOnlyConnectNew = new System.Windows.Forms.CheckBox();
			this.ChbNetworkLossDetect = new System.Windows.Forms.CheckBox();
			this.LabNetWorkSettingTips = new System.Windows.Forms.Label();
			this.ChbTcpUseNewVer = new System.Windows.Forms.CheckBox();
			this.BtnTcpIPAddrSet = new System.Windows.Forms.Button();
			this.BtnNetPortSet = new System.Windows.Forms.Button();
			this.TxbSIPAddrSeg3 = new System.Windows.Forms.TextBox();
			this.TxbSIPAddrSeg2 = new System.Windows.Forms.TextBox();
			this.TxbSIPAddrSeg1 = new System.Windows.Forms.TextBox();
			this.TxbSIPAddrSeg0 = new System.Windows.Forms.TextBox();
			this.LabTCPServerIP = new System.Windows.Forms.Label();
			this.TxbComPort = new System.Windows.Forms.TextBox();
			this.LabComPort = new System.Windows.Forms.Label();
			this.CobComSel = new System.Windows.Forms.ComboBox();
			this.LabComSel = new System.Windows.Forms.Label();
			this.PanMaskCode = new System.Windows.Forms.Panel();
			this.BtnMaskAddrSet = new System.Windows.Forms.Button();
			this.TxbMaskCodeSeg3 = new System.Windows.Forms.TextBox();
			this.TxbMaskCodeSeg2 = new System.Windows.Forms.TextBox();
			this.TxbMaskCodeSeg1 = new System.Windows.Forms.TextBox();
			this.TxbMaskCodeSeg0 = new System.Windows.Forms.TextBox();
			this.LabMaskCode = new System.Windows.Forms.Label();
			this.PanGateway = new System.Windows.Forms.Panel();
			this.BtnGWAddrSet = new System.Windows.Forms.Button();
			this.TxbGwAddrSeg3 = new System.Windows.Forms.TextBox();
			this.TxbGwAddrSeg2 = new System.Windows.Forms.TextBox();
			this.TxbGwAddrSeg1 = new System.Windows.Forms.TextBox();
			this.TxbGwAddrSeg0 = new System.Windows.Forms.TextBox();
			this.LabGatewayAddr = new System.Windows.Forms.Label();
			this.PanIpAddr = new System.Windows.Forms.Panel();
			this.BtnIPAddrSet = new System.Windows.Forms.Button();
			this.TxbIpAddrSeg3 = new System.Windows.Forms.TextBox();
			this.TxbIpAddrSeg2 = new System.Windows.Forms.TextBox();
			this.TxbIpAddrSeg1 = new System.Windows.Forms.TextBox();
			this.TxbIpAddrSeg0 = new System.Windows.Forms.TextBox();
			this.LabIPADDR = new System.Windows.Forms.Label();
			this.PanDhcpAutoIp = new System.Windows.Forms.Panel();
			this.RdbDhcpDisable = new System.Windows.Forms.RadioButton();
			this.RdbDhcpEnable = new System.Windows.Forms.RadioButton();
			this.LabDhcpState = new System.Windows.Forms.Label();
			this.GrbEncoderSetting = new System.Windows.Forms.GroupBox();
			this.RdbOutputUTF8 = new System.Windows.Forms.RadioButton();
			this.RdbOutputUnicode = new System.Windows.Forms.RadioButton();
			this.RdbOutputGBK = new System.Windows.Forms.RadioButton();
			this.GrbOutputRS232 = new System.Windows.Forms.GroupBox();
			this.CobUartStopBit = new System.Windows.Forms.ComboBox();
			this.LabUartStopBit = new System.Windows.Forms.Label();
			this.CobUartDataBit = new System.Windows.Forms.ComboBox();
			this.LabUartDataBit = new System.Windows.Forms.Label();
			this.CobUartPolarity = new System.Windows.Forms.ComboBox();
			this.LabUartCheckPolarity = new System.Windows.Forms.Label();
			this.CobBaudrateSet = new System.Windows.Forms.ComboBox();
			this.LabBaudrateSet = new System.Windows.Forms.Label();
			this.TabProtocol = new System.Windows.Forms.TabControl();
			this.TapModbus = new System.Windows.Forms.TabPage();
			this.LabMB_Tips = new System.Windows.Forms.Label();
			this.BtnMB_TcpPort = new System.Windows.Forms.Button();
			this.TxbMB_TcpPort = new System.Windows.Forms.TextBox();
			this.LabMB_TcpPort = new System.Windows.Forms.Label();
			this.BtnMB_DeviceAddr = new System.Windows.Forms.Button();
			this.TxbMB_DeviceAddr = new System.Windows.Forms.TextBox();
			this.LabMBAddr = new System.Windows.Forms.Label();
			this.LabMB_Protocol = new System.Windows.Forms.Label();
			this.CobMB_Protocol = new System.Windows.Forms.ComboBox();
			this.TapEthernetIP = new System.Windows.Forms.TabPage();
			this.PanIdsProtocol = new System.Windows.Forms.Panel();
			this.LabEtherNetTips_2 = new System.Windows.Forms.Label();
			this.ChbEnableProfiNet = new System.Windows.Forms.CheckBox();
			this.ChbEthernetIPEnable = new System.Windows.Forms.CheckBox();
			this.LabEtherNetTips_1 = new System.Windows.Forms.Label();
			this.TapMcPLC = new System.Windows.Forms.TabPage();
			this.LabMcPLCTips = new System.Windows.Forms.Label();
			this.BtnMC_Port = new System.Windows.Forms.Button();
			this.TxbMC_Port = new System.Windows.Forms.TextBox();
			this.LabMcPLCPort = new System.Windows.Forms.Label();
			this.ChbEnableMcPLC = new System.Windows.Forms.CheckBox();
			this.GrbOutputUSB.SuspendLayout();
			this.GrbOutputInternet.SuspendLayout();
			this.PanMaskCode.SuspendLayout();
			this.PanGateway.SuspendLayout();
			this.PanIpAddr.SuspendLayout();
			this.PanDhcpAutoIp.SuspendLayout();
			this.GrbEncoderSetting.SuspendLayout();
			this.GrbOutputRS232.SuspendLayout();
			this.TabProtocol.SuspendLayout();
			this.TapModbus.SuspendLayout();
			this.TapEthernetIP.SuspendLayout();
			this.PanIdsProtocol.SuspendLayout();
			this.TapMcPLC.SuspendLayout();
			base.SuspendLayout();
			this.GrbOutputUSB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbOutputUSB.Controls.Add(this.RdbOuputByUart);
			this.GrbOutputUSB.Controls.Add(this.RdbOuputByCOM);
			this.GrbOutputUSB.Controls.Add(this.RdbOuputByHID);
			this.GrbOutputUSB.Location = new System.Drawing.Point(420, 12);
			this.GrbOutputUSB.Name = "GrbOutputUSB";
			this.GrbOutputUSB.Size = new System.Drawing.Size(162, 104);
			this.GrbOutputUSB.TabIndex = 55;
			this.GrbOutputUSB.TabStop = false;
			this.GrbOutputUSB.Text = "USB接口设置";
			this.RdbOuputByUart.AutoSize = true;
			this.RdbOuputByUart.Location = new System.Drawing.Point(28, 74);
			this.RdbOuputByUart.Name = "RdbOuputByUart";
			this.RdbOuputByUart.Size = new System.Drawing.Size(88, 18);
			this.RdbOuputByUart.TabIndex = 8;
			this.RdbOuputByUart.Text = "USB不输出";
			this.RdbOuputByUart.UseVisualStyleBackColor = true;
			this.RdbOuputByUart.CheckedChanged += new System.EventHandler(RdbOuputByUart_CheckedChanged);
			this.RdbOuputByCOM.AutoSize = true;
			this.RdbOuputByCOM.Location = new System.Drawing.Point(28, 50);
			this.RdbOuputByCOM.Name = "RdbOuputByCOM";
			this.RdbOuputByCOM.Size = new System.Drawing.Size(102, 18);
			this.RdbOuputByCOM.TabIndex = 7;
			this.RdbOuputByCOM.Text = "USB虚拟串口";
			this.RdbOuputByCOM.UseVisualStyleBackColor = true;
			this.RdbOuputByCOM.CheckedChanged += new System.EventHandler(RdbOuputByCOM_CheckedChanged);
			this.RdbOuputByHID.AutoSize = true;
			this.RdbOuputByHID.Checked = true;
			this.RdbOuputByHID.Location = new System.Drawing.Point(28, 26);
			this.RdbOuputByHID.Name = "RdbOuputByHID";
			this.RdbOuputByHID.Size = new System.Drawing.Size(102, 18);
			this.RdbOuputByHID.TabIndex = 6;
			this.RdbOuputByHID.TabStop = true;
			this.RdbOuputByHID.Text = "USB键盘输出";
			this.RdbOuputByHID.UseVisualStyleBackColor = true;
			this.RdbOuputByHID.CheckedChanged += new System.EventHandler(RdbOuputByHID_CheckedChanged);
			this.GrbOutputInternet.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbOutputInternet.Controls.Add(this.ChbTcpOnlyConnectNew);
			this.GrbOutputInternet.Controls.Add(this.ChbNetworkLossDetect);
			this.GrbOutputInternet.Controls.Add(this.LabNetWorkSettingTips);
			this.GrbOutputInternet.Controls.Add(this.ChbTcpUseNewVer);
			this.GrbOutputInternet.Controls.Add(this.BtnTcpIPAddrSet);
			this.GrbOutputInternet.Controls.Add(this.BtnNetPortSet);
			this.GrbOutputInternet.Controls.Add(this.TxbSIPAddrSeg3);
			this.GrbOutputInternet.Controls.Add(this.TxbSIPAddrSeg2);
			this.GrbOutputInternet.Controls.Add(this.TxbSIPAddrSeg1);
			this.GrbOutputInternet.Controls.Add(this.TxbSIPAddrSeg0);
			this.GrbOutputInternet.Controls.Add(this.LabTCPServerIP);
			this.GrbOutputInternet.Controls.Add(this.TxbComPort);
			this.GrbOutputInternet.Controls.Add(this.LabComPort);
			this.GrbOutputInternet.Controls.Add(this.CobComSel);
			this.GrbOutputInternet.Controls.Add(this.LabComSel);
			this.GrbOutputInternet.Controls.Add(this.PanMaskCode);
			this.GrbOutputInternet.Controls.Add(this.PanGateway);
			this.GrbOutputInternet.Controls.Add(this.PanIpAddr);
			this.GrbOutputInternet.Controls.Add(this.PanDhcpAutoIp);
			this.GrbOutputInternet.Location = new System.Drawing.Point(8, 122);
			this.GrbOutputInternet.Name = "GrbOutputInternet";
			this.GrbOutputInternet.Size = new System.Drawing.Size(396, 289);
			this.GrbOutputInternet.TabIndex = 53;
			this.GrbOutputInternet.TabStop = false;
			this.GrbOutputInternet.Text = "网络接口设置";
			this.ChbTcpOnlyConnectNew.AutoSize = true;
			this.ChbTcpOnlyConnectNew.Location = new System.Drawing.Point(272, 238);
			this.ChbTcpOnlyConnectNew.Name = "ChbTcpOnlyConnectNew";
			this.ChbTcpOnlyConnectNew.Size = new System.Drawing.Size(110, 18);
			this.ChbTcpOnlyConnectNew.TabIndex = 59;
			this.ChbTcpOnlyConnectNew.Text = "强制新客户端";
			this.ChbTcpOnlyConnectNew.UseVisualStyleBackColor = true;
			this.ChbTcpOnlyConnectNew.Visible = false;
			this.ChbTcpOnlyConnectNew.CheckedChanged += new System.EventHandler(ChbTcpOnlyConnectNew_CheckedChanged);
			this.ChbNetworkLossDetect.AutoSize = true;
			this.ChbNetworkLossDetect.Location = new System.Drawing.Point(146, 238);
			this.ChbNetworkLossDetect.Name = "ChbNetworkLossDetect";
			this.ChbNetworkLossDetect.Size = new System.Drawing.Size(110, 18);
			this.ChbNetworkLossDetect.TabIndex = 57;
			this.ChbNetworkLossDetect.Text = "网络断联检测";
			this.ChbNetworkLossDetect.UseVisualStyleBackColor = true;
			this.ChbNetworkLossDetect.Visible = false;
			this.ChbNetworkLossDetect.CheckedChanged += new System.EventHandler(ChbNetworkLossDetect_CheckedChanged);
			this.LabNetWorkSettingTips.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabNetWorkSettingTips.AutoSize = true;
			this.LabNetWorkSettingTips.ForeColor = System.Drawing.Color.Blue;
			this.LabNetWorkSettingTips.Location = new System.Drawing.Point(10, 263);
			this.LabNetWorkSettingTips.Name = "LabNetWorkSettingTips";
			this.LabNetWorkSettingTips.Size = new System.Drawing.Size(238, 14);
			this.LabNetWorkSettingTips.TabIndex = 42;
			this.LabNetWorkSettingTips.Text = "注意:网络设备配置在设备重启后生效";
			this.ChbTcpUseNewVer.AutoSize = true;
			this.ChbTcpUseNewVer.Location = new System.Drawing.Point(20, 238);
			this.ChbTcpUseNewVer.Name = "ChbTcpUseNewVer";
			this.ChbTcpUseNewVer.Size = new System.Drawing.Size(110, 18);
			this.ChbTcpUseNewVer.TabIndex = 58;
			this.ChbTcpUseNewVer.Text = "支持多客户端";
			this.ChbTcpUseNewVer.UseVisualStyleBackColor = true;
			this.ChbTcpUseNewVer.Visible = false;
			this.ChbTcpUseNewVer.CheckedChanged += new System.EventHandler(ChbTcpUseNewVer_CheckedChanged);
			this.BtnTcpIPAddrSet.Location = new System.Drawing.Point(285, 203);
			this.BtnTcpIPAddrSet.Name = "BtnTcpIPAddrSet";
			this.BtnTcpIPAddrSet.Size = new System.Drawing.Size(69, 27);
			this.BtnTcpIPAddrSet.TabIndex = 41;
			this.BtnTcpIPAddrSet.Text = "确认";
			this.BtnTcpIPAddrSet.UseVisualStyleBackColor = true;
			this.BtnTcpIPAddrSet.Visible = false;
			this.BtnTcpIPAddrSet.Click += new System.EventHandler(BtnTcpIPAddrSet_Click);
			this.BtnNetPortSet.Location = new System.Drawing.Point(171, 176);
			this.BtnNetPortSet.Name = "BtnNetPortSet";
			this.BtnNetPortSet.Size = new System.Drawing.Size(58, 27);
			this.BtnNetPortSet.TabIndex = 40;
			this.BtnNetPortSet.Text = "确认";
			this.BtnNetPortSet.UseVisualStyleBackColor = true;
			this.BtnNetPortSet.Visible = false;
			this.BtnNetPortSet.Click += new System.EventHandler(BtnNetPortSet_Click);
			this.TxbSIPAddrSeg3.Location = new System.Drawing.Point(244, 205);
			this.TxbSIPAddrSeg3.MaxLength = 3;
			this.TxbSIPAddrSeg3.Name = "TxbSIPAddrSeg3";
			this.TxbSIPAddrSeg3.Size = new System.Drawing.Size(37, 23);
			this.TxbSIPAddrSeg3.TabIndex = 28;
			this.TxbSIPAddrSeg3.Text = "1";
			this.TxbSIPAddrSeg3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbSIPAddrSeg3.TextChanged += new System.EventHandler(TxbSIPAddrSeg3_TextChanged);
			this.TxbSIPAddrSeg2.Location = new System.Drawing.Point(204, 205);
			this.TxbSIPAddrSeg2.MaxLength = 3;
			this.TxbSIPAddrSeg2.Name = "TxbSIPAddrSeg2";
			this.TxbSIPAddrSeg2.Size = new System.Drawing.Size(37, 23);
			this.TxbSIPAddrSeg2.TabIndex = 27;
			this.TxbSIPAddrSeg2.Text = "0";
			this.TxbSIPAddrSeg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbSIPAddrSeg2.TextChanged += new System.EventHandler(TxbSIPAddrSeg2_TextChanged);
			this.TxbSIPAddrSeg1.Location = new System.Drawing.Point(163, 205);
			this.TxbSIPAddrSeg1.MaxLength = 3;
			this.TxbSIPAddrSeg1.Name = "TxbSIPAddrSeg1";
			this.TxbSIPAddrSeg1.Size = new System.Drawing.Size(37, 23);
			this.TxbSIPAddrSeg1.TabIndex = 26;
			this.TxbSIPAddrSeg1.Text = "168";
			this.TxbSIPAddrSeg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbSIPAddrSeg1.TextChanged += new System.EventHandler(TxbSIPAddrSeg1_TextChanged);
			this.TxbSIPAddrSeg0.Location = new System.Drawing.Point(123, 205);
			this.TxbSIPAddrSeg0.MaxLength = 3;
			this.TxbSIPAddrSeg0.Name = "TxbSIPAddrSeg0";
			this.TxbSIPAddrSeg0.Size = new System.Drawing.Size(37, 23);
			this.TxbSIPAddrSeg0.TabIndex = 25;
			this.TxbSIPAddrSeg0.Text = "192";
			this.TxbSIPAddrSeg0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbSIPAddrSeg0.TextChanged += new System.EventHandler(TxbSIPAddrSeg0_TextChanged);
			this.LabTCPServerIP.AutoSize = true;
			this.LabTCPServerIP.Location = new System.Drawing.Point(10, 209);
			this.LabTCPServerIP.Name = "LabTCPServerIP";
			this.LabTCPServerIP.Size = new System.Drawing.Size(119, 14);
			this.LabTCPServerIP.TabIndex = 24;
			this.LabTCPServerIP.Text = "通信目标IP设置：";
			this.TxbComPort.Location = new System.Drawing.Point(108, 178);
			this.TxbComPort.MaxLength = 5;
			this.TxbComPort.Name = "TxbComPort";
			this.TxbComPort.Size = new System.Drawing.Size(52, 23);
			this.TxbComPort.TabIndex = 23;
			this.TxbComPort.Text = "13242";
			this.TxbComPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbComPort.TextChanged += new System.EventHandler(TxbComPort_TextChanged);
			this.LabComPort.AutoSize = true;
			this.LabComPort.Location = new System.Drawing.Point(10, 182);
			this.LabComPort.Name = "LabComPort";
			this.LabComPort.Size = new System.Drawing.Size(105, 14);
			this.LabComPort.TabIndex = 22;
			this.LabComPort.Text = "通讯端口设置：";
			this.CobComSel.FormattingEnabled = true;
			this.CobComSel.Location = new System.Drawing.Point(108, 152);
			this.CobComSel.Name = "CobComSel";
			this.CobComSel.Size = new System.Drawing.Size(121, 22);
			this.CobComSel.TabIndex = 21;
			this.CobComSel.SelectedIndexChanged += new System.EventHandler(CobComSel_SelectedIndexChanged);
			this.LabComSel.AutoSize = true;
			this.LabComSel.Location = new System.Drawing.Point(10, 156);
			this.LabComSel.Name = "LabComSel";
			this.LabComSel.Size = new System.Drawing.Size(105, 14);
			this.LabComSel.TabIndex = 20;
			this.LabComSel.Text = "网络通信协议：";
			this.PanMaskCode.Controls.Add(this.BtnMaskAddrSet);
			this.PanMaskCode.Controls.Add(this.TxbMaskCodeSeg3);
			this.PanMaskCode.Controls.Add(this.TxbMaskCodeSeg2);
			this.PanMaskCode.Controls.Add(this.TxbMaskCodeSeg1);
			this.PanMaskCode.Controls.Add(this.TxbMaskCodeSeg0);
			this.PanMaskCode.Controls.Add(this.LabMaskCode);
			this.PanMaskCode.Location = new System.Drawing.Point(6, 115);
			this.PanMaskCode.Name = "PanMaskCode";
			this.PanMaskCode.Size = new System.Drawing.Size(351, 29);
			this.PanMaskCode.TabIndex = 17;
			this.BtnMaskAddrSet.Location = new System.Drawing.Point(269, 1);
			this.BtnMaskAddrSet.Name = "BtnMaskAddrSet";
			this.BtnMaskAddrSet.Size = new System.Drawing.Size(70, 27);
			this.BtnMaskAddrSet.TabIndex = 39;
			this.BtnMaskAddrSet.Text = "确认";
			this.BtnMaskAddrSet.UseVisualStyleBackColor = true;
			this.BtnMaskAddrSet.Visible = false;
			this.BtnMaskAddrSet.Click += new System.EventHandler(BtnMaskAddrSet_Click);
			this.TxbMaskCodeSeg3.Location = new System.Drawing.Point(227, 2);
			this.TxbMaskCodeSeg3.MaxLength = 3;
			this.TxbMaskCodeSeg3.Name = "TxbMaskCodeSeg3";
			this.TxbMaskCodeSeg3.Size = new System.Drawing.Size(37, 23);
			this.TxbMaskCodeSeg3.TabIndex = 12;
			this.TxbMaskCodeSeg3.Text = "0";
			this.TxbMaskCodeSeg3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbMaskCodeSeg3.TextChanged += new System.EventHandler(TxbMaskCodeSeg3_TextChanged);
			this.TxbMaskCodeSeg2.Location = new System.Drawing.Point(187, 2);
			this.TxbMaskCodeSeg2.MaxLength = 3;
			this.TxbMaskCodeSeg2.Name = "TxbMaskCodeSeg2";
			this.TxbMaskCodeSeg2.Size = new System.Drawing.Size(37, 23);
			this.TxbMaskCodeSeg2.TabIndex = 11;
			this.TxbMaskCodeSeg2.Text = "255";
			this.TxbMaskCodeSeg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbMaskCodeSeg2.TextChanged += new System.EventHandler(TxbMaskCodeSeg2_TextChanged);
			this.TxbMaskCodeSeg1.Location = new System.Drawing.Point(146, 2);
			this.TxbMaskCodeSeg1.MaxLength = 3;
			this.TxbMaskCodeSeg1.Name = "TxbMaskCodeSeg1";
			this.TxbMaskCodeSeg1.Size = new System.Drawing.Size(37, 23);
			this.TxbMaskCodeSeg1.TabIndex = 10;
			this.TxbMaskCodeSeg1.Text = "255";
			this.TxbMaskCodeSeg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbMaskCodeSeg1.TextChanged += new System.EventHandler(TxbMaskCodeSeg1_TextChanged);
			this.TxbMaskCodeSeg0.Location = new System.Drawing.Point(106, 2);
			this.TxbMaskCodeSeg0.MaxLength = 3;
			this.TxbMaskCodeSeg0.Name = "TxbMaskCodeSeg0";
			this.TxbMaskCodeSeg0.Size = new System.Drawing.Size(37, 23);
			this.TxbMaskCodeSeg0.TabIndex = 9;
			this.TxbMaskCodeSeg0.Text = "255";
			this.TxbMaskCodeSeg0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbMaskCodeSeg0.TextChanged += new System.EventHandler(TxbMaskCodeSeg0_TextChanged);
			this.LabMaskCode.AutoSize = true;
			this.LabMaskCode.Location = new System.Drawing.Point(3, 7);
			this.LabMaskCode.Name = "LabMaskCode";
			this.LabMaskCode.Size = new System.Drawing.Size(105, 14);
			this.LabMaskCode.TabIndex = 6;
			this.LabMaskCode.Text = "设备子网掩码：";
			this.PanGateway.Controls.Add(this.BtnGWAddrSet);
			this.PanGateway.Controls.Add(this.TxbGwAddrSeg3);
			this.PanGateway.Controls.Add(this.TxbGwAddrSeg2);
			this.PanGateway.Controls.Add(this.TxbGwAddrSeg1);
			this.PanGateway.Controls.Add(this.TxbGwAddrSeg0);
			this.PanGateway.Controls.Add(this.LabGatewayAddr);
			this.PanGateway.Location = new System.Drawing.Point(6, 83);
			this.PanGateway.Name = "PanGateway";
			this.PanGateway.Size = new System.Drawing.Size(351, 29);
			this.PanGateway.TabIndex = 18;
			this.BtnGWAddrSet.Location = new System.Drawing.Point(269, 1);
			this.BtnGWAddrSet.Name = "BtnGWAddrSet";
			this.BtnGWAddrSet.Size = new System.Drawing.Size(70, 27);
			this.BtnGWAddrSet.TabIndex = 39;
			this.BtnGWAddrSet.Text = "确认";
			this.BtnGWAddrSet.UseVisualStyleBackColor = true;
			this.BtnGWAddrSet.Visible = false;
			this.BtnGWAddrSet.Click += new System.EventHandler(BtnGWAddrSet_Click);
			this.TxbGwAddrSeg3.Location = new System.Drawing.Point(227, 2);
			this.TxbGwAddrSeg3.MaxLength = 3;
			this.TxbGwAddrSeg3.Name = "TxbGwAddrSeg3";
			this.TxbGwAddrSeg3.Size = new System.Drawing.Size(37, 23);
			this.TxbGwAddrSeg3.TabIndex = 12;
			this.TxbGwAddrSeg3.Text = "1";
			this.TxbGwAddrSeg3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbGwAddrSeg3.TextChanged += new System.EventHandler(TxbGwAddrSeg3_TextChanged);
			this.TxbGwAddrSeg2.Location = new System.Drawing.Point(187, 2);
			this.TxbGwAddrSeg2.MaxLength = 3;
			this.TxbGwAddrSeg2.Name = "TxbGwAddrSeg2";
			this.TxbGwAddrSeg2.Size = new System.Drawing.Size(37, 23);
			this.TxbGwAddrSeg2.TabIndex = 11;
			this.TxbGwAddrSeg2.Text = "0";
			this.TxbGwAddrSeg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbGwAddrSeg2.TextChanged += new System.EventHandler(TxbGwAddrSeg2_TextChanged);
			this.TxbGwAddrSeg1.Location = new System.Drawing.Point(146, 2);
			this.TxbGwAddrSeg1.MaxLength = 3;
			this.TxbGwAddrSeg1.Name = "TxbGwAddrSeg1";
			this.TxbGwAddrSeg1.Size = new System.Drawing.Size(37, 23);
			this.TxbGwAddrSeg1.TabIndex = 10;
			this.TxbGwAddrSeg1.Text = "168";
			this.TxbGwAddrSeg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbGwAddrSeg1.TextChanged += new System.EventHandler(TxbGwAddrSeg1_TextChanged);
			this.TxbGwAddrSeg0.Location = new System.Drawing.Point(106, 2);
			this.TxbGwAddrSeg0.MaxLength = 3;
			this.TxbGwAddrSeg0.Name = "TxbGwAddrSeg0";
			this.TxbGwAddrSeg0.Size = new System.Drawing.Size(37, 23);
			this.TxbGwAddrSeg0.TabIndex = 9;
			this.TxbGwAddrSeg0.Text = "192";
			this.TxbGwAddrSeg0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbGwAddrSeg0.TextChanged += new System.EventHandler(TxbGwAddrSeg0_TextChanged);
			this.LabGatewayAddr.AutoSize = true;
			this.LabGatewayAddr.Location = new System.Drawing.Point(3, 7);
			this.LabGatewayAddr.Name = "LabGatewayAddr";
			this.LabGatewayAddr.Size = new System.Drawing.Size(91, 14);
			this.LabGatewayAddr.TabIndex = 6;
			this.LabGatewayAddr.Text = "设备网关IP：";
			this.PanIpAddr.Controls.Add(this.BtnIPAddrSet);
			this.PanIpAddr.Controls.Add(this.TxbIpAddrSeg3);
			this.PanIpAddr.Controls.Add(this.TxbIpAddrSeg2);
			this.PanIpAddr.Controls.Add(this.TxbIpAddrSeg1);
			this.PanIpAddr.Controls.Add(this.TxbIpAddrSeg0);
			this.PanIpAddr.Controls.Add(this.LabIPADDR);
			this.PanIpAddr.Location = new System.Drawing.Point(6, 52);
			this.PanIpAddr.Name = "PanIpAddr";
			this.PanIpAddr.Size = new System.Drawing.Size(351, 29);
			this.PanIpAddr.TabIndex = 16;
			this.BtnIPAddrSet.Location = new System.Drawing.Point(269, 1);
			this.BtnIPAddrSet.Name = "BtnIPAddrSet";
			this.BtnIPAddrSet.Size = new System.Drawing.Size(70, 27);
			this.BtnIPAddrSet.TabIndex = 38;
			this.BtnIPAddrSet.Text = "确认";
			this.BtnIPAddrSet.UseVisualStyleBackColor = true;
			this.BtnIPAddrSet.Visible = false;
			this.BtnIPAddrSet.Click += new System.EventHandler(BtnIPAddrSet_Click);
			this.TxbIpAddrSeg3.Location = new System.Drawing.Point(227, 2);
			this.TxbIpAddrSeg3.MaxLength = 3;
			this.TxbIpAddrSeg3.Name = "TxbIpAddrSeg3";
			this.TxbIpAddrSeg3.Size = new System.Drawing.Size(37, 23);
			this.TxbIpAddrSeg3.TabIndex = 12;
			this.TxbIpAddrSeg3.Text = "135";
			this.TxbIpAddrSeg3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbIpAddrSeg3.TextChanged += new System.EventHandler(TxbIpAddrSeg3_TextChanged);
			this.TxbIpAddrSeg2.Location = new System.Drawing.Point(187, 2);
			this.TxbIpAddrSeg2.MaxLength = 3;
			this.TxbIpAddrSeg2.Name = "TxbIpAddrSeg2";
			this.TxbIpAddrSeg2.Size = new System.Drawing.Size(37, 23);
			this.TxbIpAddrSeg2.TabIndex = 11;
			this.TxbIpAddrSeg2.Text = "0";
			this.TxbIpAddrSeg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbIpAddrSeg2.TextChanged += new System.EventHandler(TxbIpAddrSeg2_TextChanged);
			this.TxbIpAddrSeg1.Location = new System.Drawing.Point(146, 2);
			this.TxbIpAddrSeg1.MaxLength = 3;
			this.TxbIpAddrSeg1.Name = "TxbIpAddrSeg1";
			this.TxbIpAddrSeg1.Size = new System.Drawing.Size(37, 23);
			this.TxbIpAddrSeg1.TabIndex = 10;
			this.TxbIpAddrSeg1.Text = "168";
			this.TxbIpAddrSeg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbIpAddrSeg1.TextChanged += new System.EventHandler(TxbIpAddrSeg1_TextChanged);
			this.TxbIpAddrSeg0.Location = new System.Drawing.Point(106, 2);
			this.TxbIpAddrSeg0.MaxLength = 3;
			this.TxbIpAddrSeg0.Name = "TxbIpAddrSeg0";
			this.TxbIpAddrSeg0.Size = new System.Drawing.Size(37, 23);
			this.TxbIpAddrSeg0.TabIndex = 9;
			this.TxbIpAddrSeg0.Text = "192";
			this.TxbIpAddrSeg0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbIpAddrSeg0.TextChanged += new System.EventHandler(TxbIpAddrSeg0_TextChanged);
			this.LabIPADDR.AutoSize = true;
			this.LabIPADDR.Location = new System.Drawing.Point(3, 7);
			this.LabIPADDR.Name = "LabIPADDR";
			this.LabIPADDR.Size = new System.Drawing.Size(91, 14);
			this.LabIPADDR.TabIndex = 6;
			this.LabIPADDR.Text = "设备IP地址：";
			this.PanDhcpAutoIp.Controls.Add(this.RdbDhcpDisable);
			this.PanDhcpAutoIp.Controls.Add(this.RdbDhcpEnable);
			this.PanDhcpAutoIp.Controls.Add(this.LabDhcpState);
			this.PanDhcpAutoIp.Enabled = false;
			this.PanDhcpAutoIp.Location = new System.Drawing.Point(6, 23);
			this.PanDhcpAutoIp.Name = "PanDhcpAutoIp";
			this.PanDhcpAutoIp.Size = new System.Drawing.Size(351, 27);
			this.PanDhcpAutoIp.TabIndex = 15;
			this.RdbDhcpDisable.AutoSize = true;
			this.RdbDhcpDisable.Location = new System.Drawing.Point(251, 3);
			this.RdbDhcpDisable.Name = "RdbDhcpDisable";
			this.RdbDhcpDisable.Size = new System.Drawing.Size(53, 18);
			this.RdbDhcpDisable.TabIndex = 2;
			this.RdbDhcpDisable.TabStop = true;
			this.RdbDhcpDisable.Text = "关闭";
			this.RdbDhcpDisable.UseVisualStyleBackColor = true;
			this.RdbDhcpDisable.CheckedChanged += new System.EventHandler(RdbDhcpDisable_CheckedChanged);
			this.RdbDhcpEnable.AutoSize = true;
			this.RdbDhcpEnable.Location = new System.Drawing.Point(180, 3);
			this.RdbDhcpEnable.Name = "RdbDhcpEnable";
			this.RdbDhcpEnable.Size = new System.Drawing.Size(53, 18);
			this.RdbDhcpEnable.TabIndex = 1;
			this.RdbDhcpEnable.TabStop = true;
			this.RdbDhcpEnable.Text = "开启";
			this.RdbDhcpEnable.UseVisualStyleBackColor = true;
			this.RdbDhcpEnable.CheckedChanged += new System.EventHandler(RdbDhcpEnable_CheckedChanged);
			this.LabDhcpState.AutoSize = true;
			this.LabDhcpState.Location = new System.Drawing.Point(3, 6);
			this.LabDhcpState.Name = "LabDhcpState";
			this.LabDhcpState.Size = new System.Drawing.Size(140, 14);
			this.LabDhcpState.TabIndex = 0;
			this.LabDhcpState.Text = "DHCP自动获取IP开关:";
			this.GrbEncoderSetting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbEncoderSetting.Controls.Add(this.RdbOutputUTF8);
			this.GrbEncoderSetting.Controls.Add(this.RdbOutputUnicode);
			this.GrbEncoderSetting.Controls.Add(this.RdbOutputGBK);
			this.GrbEncoderSetting.Location = new System.Drawing.Point(588, 12);
			this.GrbEncoderSetting.Name = "GrbEncoderSetting";
			this.GrbEncoderSetting.Size = new System.Drawing.Size(143, 104);
			this.GrbEncoderSetting.TabIndex = 54;
			this.GrbEncoderSetting.TabStop = false;
			this.GrbEncoderSetting.Text = "编码设置";
			this.RdbOutputUTF8.AutoSize = true;
			this.RdbOutputUTF8.Location = new System.Drawing.Point(28, 74);
			this.RdbOutputUTF8.Name = "RdbOutputUTF8";
			this.RdbOutputUTF8.Size = new System.Drawing.Size(60, 18);
			this.RdbOutputUTF8.TabIndex = 5;
			this.RdbOutputUTF8.Text = "UTF-8";
			this.RdbOutputUTF8.UseVisualStyleBackColor = true;
			this.RdbOutputUTF8.CheckedChanged += new System.EventHandler(RdbOutputUTF8_CheckedChanged);
			this.RdbOutputUnicode.AutoSize = true;
			this.RdbOutputUnicode.Location = new System.Drawing.Point(28, 50);
			this.RdbOutputUnicode.Name = "RdbOutputUnicode";
			this.RdbOutputUnicode.Size = new System.Drawing.Size(74, 18);
			this.RdbOutputUnicode.TabIndex = 4;
			this.RdbOutputUnicode.Text = "Unicode";
			this.RdbOutputUnicode.UseVisualStyleBackColor = true;
			this.RdbOutputUnicode.CheckedChanged += new System.EventHandler(RdbOutputUnicode_CheckedChanged);
			this.RdbOutputGBK.AutoSize = true;
			this.RdbOutputGBK.Checked = true;
			this.RdbOutputGBK.Location = new System.Drawing.Point(28, 26);
			this.RdbOutputGBK.Name = "RdbOutputGBK";
			this.RdbOutputGBK.Size = new System.Drawing.Size(46, 18);
			this.RdbOutputGBK.TabIndex = 3;
			this.RdbOutputGBK.TabStop = true;
			this.RdbOutputGBK.Text = "GBK";
			this.RdbOutputGBK.UseVisualStyleBackColor = true;
			this.RdbOutputGBK.CheckedChanged += new System.EventHandler(RdbOutputGBK_CheckedChanged);
			this.GrbOutputRS232.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbOutputRS232.Controls.Add(this.CobUartStopBit);
			this.GrbOutputRS232.Controls.Add(this.LabUartStopBit);
			this.GrbOutputRS232.Controls.Add(this.CobUartDataBit);
			this.GrbOutputRS232.Controls.Add(this.LabUartDataBit);
			this.GrbOutputRS232.Controls.Add(this.CobUartPolarity);
			this.GrbOutputRS232.Controls.Add(this.LabUartCheckPolarity);
			this.GrbOutputRS232.Controls.Add(this.CobBaudrateSet);
			this.GrbOutputRS232.Controls.Add(this.LabBaudrateSet);
			this.GrbOutputRS232.Location = new System.Drawing.Point(8, 12);
			this.GrbOutputRS232.Name = "GrbOutputRS232";
			this.GrbOutputRS232.Size = new System.Drawing.Size(396, 104);
			this.GrbOutputRS232.TabIndex = 52;
			this.GrbOutputRS232.TabStop = false;
			this.GrbOutputRS232.Text = "RS232接口设置";
			this.CobUartStopBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobUartStopBit.FormattingEnabled = true;
			this.CobUartStopBit.Items.AddRange(new object[3] { "1", "1.5", "2" });
			this.CobUartStopBit.Location = new System.Drawing.Point(307, 61);
			this.CobUartStopBit.Name = "CobUartStopBit";
			this.CobUartStopBit.Size = new System.Drawing.Size(68, 22);
			this.CobUartStopBit.TabIndex = 41;
			this.CobUartStopBit.SelectedIndexChanged += new System.EventHandler(CobUartStopBit_SelectedIndexChanged);
			this.LabUartStopBit.AutoSize = true;
			this.LabUartStopBit.Location = new System.Drawing.Point(217, 64);
			this.LabUartStopBit.Name = "LabUartStopBit";
			this.LabUartStopBit.Size = new System.Drawing.Size(84, 14);
			this.LabUartStopBit.TabIndex = 40;
			this.LabUartStopBit.Text = "停止位设置:";
			this.CobUartDataBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobUartDataBit.FormattingEnabled = true;
			this.CobUartDataBit.Items.AddRange(new object[4] { "8", "7", "6", "5" });
			this.CobUartDataBit.Location = new System.Drawing.Point(307, 27);
			this.CobUartDataBit.Name = "CobUartDataBit";
			this.CobUartDataBit.Size = new System.Drawing.Size(68, 22);
			this.CobUartDataBit.TabIndex = 39;
			this.CobUartDataBit.SelectedIndexChanged += new System.EventHandler(CobUartDataBit_SelectedIndexChanged);
			this.LabUartDataBit.AutoSize = true;
			this.LabUartDataBit.Location = new System.Drawing.Point(217, 30);
			this.LabUartDataBit.Name = "LabUartDataBit";
			this.LabUartDataBit.Size = new System.Drawing.Size(84, 14);
			this.LabUartDataBit.TabIndex = 38;
			this.LabUartDataBit.Text = "数据位设置:";
			this.CobUartPolarity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobUartPolarity.FormattingEnabled = true;
			this.CobUartPolarity.Items.AddRange(new object[3] { "无校验", "奇校验", "偶校验" });
			this.CobUartPolarity.Location = new System.Drawing.Point(97, 63);
			this.CobUartPolarity.Name = "CobUartPolarity";
			this.CobUartPolarity.Size = new System.Drawing.Size(97, 22);
			this.CobUartPolarity.TabIndex = 37;
			this.CobUartPolarity.SelectedIndexChanged += new System.EventHandler(CobUartPolarity_SelectedIndexChanged);
			this.LabUartCheckPolarity.AutoSize = true;
			this.LabUartCheckPolarity.Location = new System.Drawing.Point(7, 64);
			this.LabUartCheckPolarity.Name = "LabUartCheckPolarity";
			this.LabUartCheckPolarity.Size = new System.Drawing.Size(70, 14);
			this.LabUartCheckPolarity.TabIndex = 36;
			this.LabUartCheckPolarity.Text = "奇偶校验:";
			this.CobBaudrateSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobBaudrateSet.FormattingEnabled = true;
			this.CobBaudrateSet.Items.AddRange(new object[7] { "2400", "4800", "9600", "19200", "38400", "57600", "115200" });
			this.CobBaudrateSet.Location = new System.Drawing.Point(97, 27);
			this.CobBaudrateSet.Name = "CobBaudrateSet";
			this.CobBaudrateSet.Size = new System.Drawing.Size(97, 22);
			this.CobBaudrateSet.TabIndex = 35;
			this.CobBaudrateSet.SelectedIndexChanged += new System.EventHandler(CobBaudrateSet_SelectedIndexChanged);
			this.LabBaudrateSet.AutoSize = true;
			this.LabBaudrateSet.Location = new System.Drawing.Point(7, 30);
			this.LabBaudrateSet.Name = "LabBaudrateSet";
			this.LabBaudrateSet.Size = new System.Drawing.Size(84, 14);
			this.LabBaudrateSet.TabIndex = 34;
			this.LabBaudrateSet.Text = "波特率设置:";
			this.TabProtocol.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TabProtocol.Controls.Add(this.TapModbus);
			this.TabProtocol.Controls.Add(this.TapEthernetIP);
			this.TabProtocol.Controls.Add(this.TapMcPLC);
			this.TabProtocol.Location = new System.Drawing.Point(8, 417);
			this.TabProtocol.Name = "TabProtocol";
			this.TabProtocol.SelectedIndex = 0;
			this.TabProtocol.Size = new System.Drawing.Size(396, 168);
			this.TabProtocol.TabIndex = 57;
			this.TapModbus.Controls.Add(this.LabMB_Tips);
			this.TapModbus.Controls.Add(this.BtnMB_TcpPort);
			this.TapModbus.Controls.Add(this.TxbMB_TcpPort);
			this.TapModbus.Controls.Add(this.LabMB_TcpPort);
			this.TapModbus.Controls.Add(this.BtnMB_DeviceAddr);
			this.TapModbus.Controls.Add(this.TxbMB_DeviceAddr);
			this.TapModbus.Controls.Add(this.LabMBAddr);
			this.TapModbus.Controls.Add(this.LabMB_Protocol);
			this.TapModbus.Controls.Add(this.CobMB_Protocol);
			this.TapModbus.Location = new System.Drawing.Point(4, 24);
			this.TapModbus.Name = "TapModbus";
			this.TapModbus.Padding = new System.Windows.Forms.Padding(3);
			this.TapModbus.Size = new System.Drawing.Size(388, 140);
			this.TapModbus.TabIndex = 0;
			this.TapModbus.Text = "Modbus协议";
			this.TapModbus.UseVisualStyleBackColor = true;
			this.LabMB_Tips.AutoSize = true;
			this.LabMB_Tips.Location = new System.Drawing.Point(14, 100);
			this.LabMB_Tips.Name = "LabMB_Tips";
			this.LabMB_Tips.Size = new System.Drawing.Size(210, 14);
			this.LabMB_Tips.TabIndex = 60;
			this.LabMB_Tips.Text = "注意:协议配置在设备重启后生效";
			this.BtnMB_TcpPort.Location = new System.Drawing.Point(190, 69);
			this.BtnMB_TcpPort.Name = "BtnMB_TcpPort";
			this.BtnMB_TcpPort.Size = new System.Drawing.Size(46, 23);
			this.BtnMB_TcpPort.TabIndex = 58;
			this.BtnMB_TcpPort.Text = "确认";
			this.BtnMB_TcpPort.UseVisualStyleBackColor = true;
			this.BtnMB_TcpPort.Visible = false;
			this.BtnMB_TcpPort.Click += new System.EventHandler(BtnMB_TcpPort_Click);
			this.TxbMB_TcpPort.Location = new System.Drawing.Point(132, 69);
			this.TxbMB_TcpPort.Name = "TxbMB_TcpPort";
			this.TxbMB_TcpPort.Size = new System.Drawing.Size(51, 23);
			this.TxbMB_TcpPort.TabIndex = 59;
			this.TxbMB_TcpPort.Text = "502";
			this.TxbMB_TcpPort.TextChanged += new System.EventHandler(TxbMB_TcpPort_TextChanged);
			this.LabMB_TcpPort.AutoSize = true;
			this.LabMB_TcpPort.Location = new System.Drawing.Point(14, 73);
			this.LabMB_TcpPort.Name = "LabMB_TcpPort";
			this.LabMB_TcpPort.Size = new System.Drawing.Size(105, 14);
			this.LabMB_TcpPort.TabIndex = 57;
			this.LabMB_TcpPort.Text = "ModbusTCP端口:";
			this.BtnMB_DeviceAddr.Location = new System.Drawing.Point(190, 41);
			this.BtnMB_DeviceAddr.Name = "BtnMB_DeviceAddr";
			this.BtnMB_DeviceAddr.Size = new System.Drawing.Size(46, 23);
			this.BtnMB_DeviceAddr.TabIndex = 55;
			this.BtnMB_DeviceAddr.Text = "确认";
			this.BtnMB_DeviceAddr.UseVisualStyleBackColor = true;
			this.BtnMB_DeviceAddr.Visible = false;
			this.BtnMB_DeviceAddr.Click += new System.EventHandler(BtnMB_DeviceAddr_Click);
			this.TxbMB_DeviceAddr.Location = new System.Drawing.Point(132, 41);
			this.TxbMB_DeviceAddr.Name = "TxbMB_DeviceAddr";
			this.TxbMB_DeviceAddr.Size = new System.Drawing.Size(51, 23);
			this.TxbMB_DeviceAddr.TabIndex = 56;
			this.TxbMB_DeviceAddr.Text = "1";
			this.TxbMB_DeviceAddr.TextChanged += new System.EventHandler(TxbMB_DeviceAddr_TextChanged);
			this.LabMBAddr.AutoSize = true;
			this.LabMBAddr.Location = new System.Drawing.Point(14, 45);
			this.LabMBAddr.Name = "LabMBAddr";
			this.LabMBAddr.Size = new System.Drawing.Size(112, 14);
			this.LabMBAddr.TabIndex = 54;
			this.LabMBAddr.Text = "Modbus设备地址:";
			this.LabMB_Protocol.AutoSize = true;
			this.LabMB_Protocol.Location = new System.Drawing.Point(14, 16);
			this.LabMB_Protocol.Name = "LabMB_Protocol";
			this.LabMB_Protocol.Size = new System.Drawing.Size(112, 14);
			this.LabMB_Protocol.TabIndex = 53;
			this.LabMB_Protocol.Text = "Modbus协议类型:";
			this.CobMB_Protocol.FormattingEnabled = true;
			this.CobMB_Protocol.Items.AddRange(new object[3] { "关闭Modbus协议输出", "Modbus-RTU", "Modbus-TCP" });
			this.CobMB_Protocol.Location = new System.Drawing.Point(132, 13);
			this.CobMB_Protocol.Name = "CobMB_Protocol";
			this.CobMB_Protocol.Size = new System.Drawing.Size(121, 22);
			this.CobMB_Protocol.TabIndex = 52;
			this.CobMB_Protocol.SelectedIndexChanged += new System.EventHandler(CobMB_Protocol_SelectedIndexChanged);
			this.TapEthernetIP.Controls.Add(this.PanIdsProtocol);
			this.TapEthernetIP.Location = new System.Drawing.Point(4, 24);
			this.TapEthernetIP.Name = "TapEthernetIP";
			this.TapEthernetIP.Padding = new System.Windows.Forms.Padding(3);
			this.TapEthernetIP.Size = new System.Drawing.Size(388, 140);
			this.TapEthernetIP.TabIndex = 1;
			this.TapEthernetIP.Text = "EthernetIP/ProfiNet";
			this.TapEthernetIP.UseVisualStyleBackColor = true;
			this.PanIdsProtocol.Controls.Add(this.LabEtherNetTips_2);
			this.PanIdsProtocol.Controls.Add(this.ChbEnableProfiNet);
			this.PanIdsProtocol.Controls.Add(this.ChbEthernetIPEnable);
			this.PanIdsProtocol.Controls.Add(this.LabEtherNetTips_1);
			this.PanIdsProtocol.Enabled = false;
			this.PanIdsProtocol.Location = new System.Drawing.Point(5, 3);
			this.PanIdsProtocol.Name = "PanIdsProtocol";
			this.PanIdsProtocol.Size = new System.Drawing.Size(377, 126);
			this.PanIdsProtocol.TabIndex = 64;
			this.LabEtherNetTips_2.AutoSize = true;
			this.LabEtherNetTips_2.Location = new System.Drawing.Point(18, 95);
			this.LabEtherNetTips_2.Name = "LabEtherNetTips_2";
			this.LabEtherNetTips_2.Size = new System.Drawing.Size(217, 14);
			this.LabEtherNetTips_2.TabIndex = 64;
			this.LabEtherNetTips_2.Text = "但是有的设备不支持这两个协议。";
			this.ChbEnableProfiNet.AutoSize = true;
			this.ChbEnableProfiNet.Location = new System.Drawing.Point(14, 31);
			this.ChbEnableProfiNet.Name = "ChbEnableProfiNet";
			this.ChbEnableProfiNet.Size = new System.Drawing.Size(138, 18);
			this.ChbEnableProfiNet.TabIndex = 63;
			this.ChbEnableProfiNet.Text = "启用ProfiNet协议";
			this.ChbEnableProfiNet.UseVisualStyleBackColor = true;
			this.ChbEnableProfiNet.CheckedChanged += new System.EventHandler(ChbEnableProfiNet_CheckedChanged);
			this.ChbEthernetIPEnable.AutoSize = true;
			this.ChbEthernetIPEnable.Location = new System.Drawing.Point(14, 9);
			this.ChbEthernetIPEnable.Name = "ChbEthernetIPEnable";
			this.ChbEthernetIPEnable.Size = new System.Drawing.Size(152, 18);
			this.ChbEthernetIPEnable.TabIndex = 62;
			this.ChbEthernetIPEnable.Text = "启用EthernetIP协议";
			this.ChbEthernetIPEnable.UseVisualStyleBackColor = true;
			this.ChbEthernetIPEnable.CheckedChanged += new System.EventHandler(ChbEthernetIPEnable_CheckedChanged);
			this.LabEtherNetTips_1.AutoSize = true;
			this.LabEtherNetTips_1.Location = new System.Drawing.Point(18, 72);
			this.LabEtherNetTips_1.Name = "LabEtherNetTips_1";
			this.LabEtherNetTips_1.Size = new System.Drawing.Size(147, 14);
			this.LabEtherNetTips_1.TabIndex = 61;
			this.LabEtherNetTips_1.Text = "注意:设置后重启生效,";
			this.TapMcPLC.Controls.Add(this.LabMcPLCTips);
			this.TapMcPLC.Controls.Add(this.BtnMC_Port);
			this.TapMcPLC.Controls.Add(this.TxbMC_Port);
			this.TapMcPLC.Controls.Add(this.LabMcPLCPort);
			this.TapMcPLC.Controls.Add(this.ChbEnableMcPLC);
			this.TapMcPLC.Location = new System.Drawing.Point(4, 24);
			this.TapMcPLC.Name = "TapMcPLC";
			this.TapMcPLC.Padding = new System.Windows.Forms.Padding(3);
			this.TapMcPLC.Size = new System.Drawing.Size(388, 140);
			this.TapMcPLC.TabIndex = 3;
			this.TapMcPLC.Text = "三菱MC";
			this.TapMcPLC.UseVisualStyleBackColor = true;
			this.LabMcPLCTips.AutoSize = true;
			this.LabMcPLCTips.Location = new System.Drawing.Point(10, 77);
			this.LabMcPLCTips.Name = "LabMcPLCTips";
			this.LabMcPLCTips.Size = new System.Drawing.Size(210, 14);
			this.LabMcPLCTips.TabIndex = 68;
			this.LabMcPLCTips.Text = "注意:协议配置在设备重启后生效";
			this.BtnMC_Port.Location = new System.Drawing.Point(188, 47);
			this.BtnMC_Port.Name = "BtnMC_Port";
			this.BtnMC_Port.Size = new System.Drawing.Size(46, 23);
			this.BtnMC_Port.TabIndex = 66;
			this.BtnMC_Port.Text = "确认";
			this.BtnMC_Port.UseVisualStyleBackColor = true;
			this.BtnMC_Port.Visible = false;
			this.BtnMC_Port.Click += new System.EventHandler(BtnMC_Port_Click);
			this.TxbMC_Port.Location = new System.Drawing.Point(130, 47);
			this.TxbMC_Port.Name = "TxbMC_Port";
			this.TxbMC_Port.Size = new System.Drawing.Size(51, 23);
			this.TxbMC_Port.TabIndex = 67;
			this.TxbMC_Port.Text = "9002";
			this.TxbMC_Port.TextChanged += new System.EventHandler(TxbMC_Port_TextChanged);
			this.LabMcPLCPort.AutoSize = true;
			this.LabMcPLCPort.Location = new System.Drawing.Point(10, 50);
			this.LabMcPLCPort.Name = "LabMcPLCPort";
			this.LabMcPLCPort.Size = new System.Drawing.Size(112, 14);
			this.LabMcPLCPort.TabIndex = 65;
			this.LabMcPLCPort.Text = "三菱MC协议端口:";
			this.ChbEnableMcPLC.AutoSize = true;
			this.ChbEnableMcPLC.Location = new System.Drawing.Point(11, 18);
			this.ChbEnableMcPLC.Name = "ChbEnableMcPLC";
			this.ChbEnableMcPLC.Size = new System.Drawing.Size(124, 18);
			this.ChbEnableMcPLC.TabIndex = 64;
			this.ChbEnableMcPLC.Text = "启用三菱MC协议";
			this.ChbEnableMcPLC.UseVisualStyleBackColor = true;
			this.ChbEnableMcPLC.CheckedChanged += new System.EventHandler(ChbEnableMcPLC_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(735, 689);
			base.Controls.Add(this.TabProtocol);
			base.Controls.Add(this.GrbOutputUSB);
			base.Controls.Add(this.GrbOutputInternet);
			base.Controls.Add(this.GrbEncoderSetting);
			base.Controls.Add(this.GrbOutputRS232);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "CommunicateSettingForm";
			this.Text = "通讯协议";
			base.Load += new System.EventHandler(DataOutputSettingForm_Load);
			this.GrbOutputUSB.ResumeLayout(false);
			this.GrbOutputUSB.PerformLayout();
			this.GrbOutputInternet.ResumeLayout(false);
			this.GrbOutputInternet.PerformLayout();
			this.PanMaskCode.ResumeLayout(false);
			this.PanMaskCode.PerformLayout();
			this.PanGateway.ResumeLayout(false);
			this.PanGateway.PerformLayout();
			this.PanIpAddr.ResumeLayout(false);
			this.PanIpAddr.PerformLayout();
			this.PanDhcpAutoIp.ResumeLayout(false);
			this.PanDhcpAutoIp.PerformLayout();
			this.GrbEncoderSetting.ResumeLayout(false);
			this.GrbEncoderSetting.PerformLayout();
			this.GrbOutputRS232.ResumeLayout(false);
			this.GrbOutputRS232.PerformLayout();
			this.TabProtocol.ResumeLayout(false);
			this.TapModbus.ResumeLayout(false);
			this.TapModbus.PerformLayout();
			this.TapEthernetIP.ResumeLayout(false);
			this.PanIdsProtocol.ResumeLayout(false);
			this.PanIdsProtocol.PerformLayout();
			this.TapMcPLC.ResumeLayout(false);
			this.TapMcPLC.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
