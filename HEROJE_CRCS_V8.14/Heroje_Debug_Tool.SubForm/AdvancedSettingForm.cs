using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FTPManager_N;
using Heroje_Debug_Tool.BaseClass;
using 合杰图像算法调试工具;
using 图像算法调试工具;

namespace Heroje_Debug_Tool.SubForm
{
	public class AdvancedSettingForm : Form
	{
		private class IconsIndexes
		{
			public const int FixedDrive = 0;

			public const int CDRom = 1;

			public const int RemovableDisk = 2;

			public const int Folder = 3;

			public const int RecentFiles = 4;

			public const int ReturnLast = 5;
		}

		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private int record_click_for_log = 0;

		private string curFilePath = "";

		private string CurrentDir = "";

		private ProcessBarForm FormProcessBar;

		private int foldercnt = 0;

		private bool IsUpgradeFirmware = false;

		private IContainer components = null;

		private GroupBox GrbFtpSettomg;

		private TextBox TxbFTPAccount;

		private TextBox TxbFtpIP;

		private Label LabFtpPassword;

		private Label LabAccount;

		private Label LabFtpIPADDR;

		private ListView lvwFiles;

		private ColumnHeader ColFileName;

		private ColumnHeader ColFileType;

		private ColumnHeader ColFileSize;

		private Button BtnFreshFtp;

		private Button BtnDisconnectFTP;

		private Button BtnConnectFTP;

		private TextBox TxbFtpPassword;

		private GroupBox GrbDeviceADSetting;

		private ComboBox CobMuiltCodeSet;

		private Label LabMuiltCode;

		private Label LabResolution;

		private ComboBox CobResolutionSet;

		private Panel PanResultOutputWay;

		private RadioButton RdbQualityResultOverall;

		private RadioButton RdbQualityResultOrderAll;

		private Label LabResultOutputWay;

		private RadioButton RdbQualityOutputDescAndData;

		private Label LabForceLowerLevel;

		private ComboBox CobForceLowerLevel;

		private TextBox TxbQualityAllSeparator;

		private Label LabQualityAllSeparator;

		private TextBox TxbQualityBarcodeSeparator;

		private Label LabQualityBarcodeSeparator;

		private Label LabBarcodeQualityDisp;

		private ComboBox CobBarcodeQualityMode;

		private Panel PanOnlyForServer;

		private ComboBox CobHostUploadMode;

		private Label LabHostUploadMode;

		private Label LabDecodeWaitTimeUnit;

		private Label LabDecodeWaitTime;

		private Button BtnDecodeWaitTimeSet;

		private TextBox TxbDecodeWaitTime;

		private CheckBox ChkNetworkingSplitCh;

		private Button BtnNetWorkingSplitCh;

		private TextBox TxbNetWorkingSplitCh;

		private CheckBox ChkUseSameTrig;

		private Panel PanOnlyForClient;

		private Button BtnHostSIP_Set;

		private TextBox TxbHostSIP_Seg4;

		private TextBox TxbHostSIP_Seg3;

		private TextBox TxbHostSIP_Seg2;

		private TextBox TxbHostSIP_Seg1;

		private Label LabNetworkingHostIP;

		private RadioButton RdbClientMode;

		private RadioButton RdbNetWorkingServer;

		private RadioButton RdbNetWorkingClose;

		private ComboBox CobLogState;

		private Label LabLogState;

		private ListView LvwClientDisp;

		private Label LabClientCount;

		private Label LabClientCountDisp;

		private GroupBox GrbMuiltCoreAls;

		private ComboBox CobMuiltCoreMode;

		private Label LabMuiltCoreMode;

		private ImageList ilstIcons;

		private ContextMenuStrip cmsMain;

		private ToolStripMenuItem tsmiDownload;

		private ToolStripMenuItem tsmiUpload;

		private ToolStripMenuItem tsmiDelete;

		private ToolStripMenuItem tsmiReflesh;

		private ToolStripMenuItem RenameToolStripMenuItem;

		private ToolStripMenuItem MkdirToolStripMenuItem;

		private GroupBox GrpCodeQuality;

		private GroupBox GrpGroupNet;

		private GroupBox GrpState;

		private TabControl TabMultSettings;

		private TabPage TpIso;

		private CheckBox ChkSwitchDec;

		private CheckBox ChkSwitchFPD;

		private CheckBox ChkSwitchMD;

		private CheckBox ChkSwitchSC;

		private CheckBox ChkSwitchPG;

		private CheckBox ChkSwitchRM;

		private CheckBox ChkSwitchUEC;

		private CheckBox ChkSwitchGN;

		private CheckBox ChkSwitchAN;

		private CheckBox ChkSwitchVID;

		private CheckBox ChkSwitchFID;

		private Panel PanChkSwitch;

		private TabPage TpTheOthers;

		private ComboBox CobCauseNG_TH_Set;

		private Label LabCauseNG_TH_Set;

		private ComboBox CobCauseNGOut;

		private Label LabCauseNGOut;

		private ComboBox CobTryMultCode;

		private Label LabTryMultCode;

		public AdvancedSettingForm()
		{
			InitializeComponent();
		}

		private void AdvancedSettingForm_Load(object sender, EventArgs e)
		{
			CobResolutionSet.Enabled = false;
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			if (para.ParaDataLen >= 4096)
			{
				CobResolutionSet.Enabled = false;
			}
			else
			{
				CobResolutionSet.Enabled = true;
			}
			if (para.DeviceTypeRecord >= 6)
			{
				GrbMuiltCoreAls.Enabled = true;
				if (GetCfgCBFuncCB(139783u) == 2)
				{
					CobMuiltCoreMode.SelectedIndex = 2;
				}
				else if (GetCfgCBFuncCB(139783u) == 4)
				{
					CobMuiltCoreMode.SelectedIndex = 3;
				}
				else if (GetCfgCBFuncCB(139783u) == 1)
				{
					CobMuiltCoreMode.SelectedIndex = 1;
				}
				else
				{
					CobMuiltCoreMode.SelectedIndex = 0;
				}
				GrbDeviceADSetting.Enabled = true;
				if (GetCfgCBFuncCB(200711u) == 0)
				{
					CobBarcodeQualityMode.SelectedIndex = 0;
				}
				else if (GetCfgCBFuncCB(200711u) == 1)
				{
					CobBarcodeQualityMode.SelectedIndex = 1;
				}
				else if (GetCfgCBFuncCB(200711u) == 2)
				{
					CobBarcodeQualityMode.SelectedIndex = 2;
				}
				get_show_iso_item_switch();
				if (GetCfgCBFuncCB(201479u) == 4)
				{
					CobForceLowerLevel.SelectedIndex = 1;
				}
				else if (GetCfgCBFuncCB(201479u) == 3)
				{
					CobForceLowerLevel.SelectedIndex = 2;
				}
				else if (GetCfgCBFuncCB(201479u) == 2)
				{
					CobForceLowerLevel.SelectedIndex = 3;
				}
				else if (GetCfgCBFuncCB(201479u) == 1)
				{
					CobForceLowerLevel.SelectedIndex = 4;
				}
				else
				{
					CobForceLowerLevel.SelectedIndex = 0;
				}
				if (GetCfgCBFuncCB(201496u) == 8)
				{
					CobCauseNGOut.SelectedIndex = 1;
				}
				else if (GetCfgCBFuncCB(201496u) == 16)
				{
					CobCauseNGOut.SelectedIndex = 2;
				}
				else
				{
					CobCauseNGOut.SelectedIndex = 0;
				}
				if (GetCfgCBFuncCB(201696u) == 32)
				{
					CobCauseNG_TH_Set.SelectedIndex = 1;
				}
				else if (GetCfgCBFuncCB(201696u) == 64)
				{
					CobCauseNG_TH_Set.SelectedIndex = 2;
				}
				else if (GetCfgCBFuncCB(201696u) == 96)
				{
					CobCauseNG_TH_Set.SelectedIndex = 3;
				}
				else if (GetCfgCBFuncCB(201696u) == 128)
				{
					CobCauseNG_TH_Set.SelectedIndex = 4;
				}
				else
				{
					CobCauseNG_TH_Set.SelectedIndex = 0;
				}
				if (GetCfgCBFuncCB(68355u) == 0)
				{
					CobTryMultCode.SelectedIndex = 0;
				}
				else
				{
					CobTryMultCode.SelectedIndex = 1;
				}
				if (GetCfgCBFuncCB(200728u) == 0)
				{
					RdbQualityResultOverall.Checked = true;
				}
				else if (GetCfgCBFuncCB(200728u) == 8)
				{
					RdbQualityResultOrderAll.Checked = true;
				}
				else if (GetCfgCBFuncCB(200711u) == 0)
				{
					RdbQualityOutputDescAndData.Checked = true;
				}
				if (GetCfgCBFuncCB(201215u) == 0)
				{
					TxbQualityBarcodeSeparator.Text = "0";
				}
				else
				{
					byte[] bytes = new byte[2]
					{
						GetCfgCBFuncCB(201215u),
						0
					};
					TxbQualityBarcodeSeparator.Text = Encoding.Default.GetString(bytes);
				}
				if (GetCfgCBFuncCB(201471u) == 0)
				{
					TxbQualityAllSeparator.Text = "0";
				}
				else
				{
					byte[] bytes2 = new byte[2]
					{
						GetCfgCBFuncCB(201471u),
						0
					};
					TxbQualityAllSeparator.Text = Encoding.Default.GetString(bytes2);
				}
				if (GetCfgCBFuncCB(155651u) == 2)
				{
					RdbNetWorkingServer.Checked = true;
				}
				else if (GetCfgCBFuncCB(155651u) == 1)
				{
					RdbClientMode.Checked = true;
				}
				else
				{
					RdbNetWorkingClose.Checked = true;
				}
				ChkUseSameTrig.Checked = GetCfgCBFuncCB(155696u) == 0;
				if (GetCfgCBFuncCB(158207u) == 0)
				{
					ChkNetworkingSplitCh.Checked = false;
					TxbNetWorkingSplitCh.Text = "";
					TxbNetWorkingSplitCh.Enabled = false;
				}
				else
				{
					ChkNetworkingSplitCh.Checked = true;
					TxbNetWorkingSplitCh.Text = GetCfgCBFuncCB(158207u).ToString("X2");
					TxbNetWorkingSplitCh.Enabled = true;
				}
				TxbDecodeWaitTime.Text = (GetCfgCBFuncCB(156415u) * 256 + GetCfgCBFuncCB(156159u)).ToString();
				TxbHostSIP_Seg1.Text = GetCfgCBFuncCB(156671u).ToString();
				TxbHostSIP_Seg2.Text = GetCfgCBFuncCB(156927u).ToString();
				TxbHostSIP_Seg3.Text = GetCfgCBFuncCB(157183u).ToString();
				TxbHostSIP_Seg4.Text = GetCfgCBFuncCB(157439u).ToString();
				if (GetCfgCBFuncCB(155660u) == 0)
				{
					CobHostUploadMode.SelectedIndex = 0;
				}
				else if (GetCfgCBFuncCB(155660u) == 8)
				{
					CobHostUploadMode.SelectedIndex = 1;
				}
				if (GetCfgCBFuncCB(77831u) == 0)
				{
					CobLogState.SelectedIndex = 0;
				}
				else if (GetCfgCBFuncCB(77831u) == 4)
				{
					CobLogState.SelectedIndex = 2;
				}
				else if (GetCfgCBFuncCB(77831u) == 5)
				{
					CobLogState.SelectedIndex = 3;
				}
				else if (GetCfgCBFuncCB(77831u) == 6)
				{
					CobLogState.SelectedIndex = 4;
				}
				else if (GetCfgCBFuncCB(77831u) == 7)
				{
					CobLogState.SelectedIndex = 5;
				}
				else
				{
					CobLogState.SelectedIndex = 1;
				}
			}
			else
			{
				GrbMuiltCoreAls.Enabled = false;
			}
			int selectedIndex = GetCfgCBFuncCB(51715u) & 3;
			CobResolutionSet.SelectedIndex = selectedIndex;
			if (GetCfgCBFuncCB(11280u) == 16)
			{
				selectedIndex = GetCfgCBFuncCB(48911u);
				CobMuiltCodeSet.SelectedIndex = selectedIndex;
			}
			else
			{
				CobMuiltCodeSet.SelectedIndex = 0;
			}
		}

		private void get_show_iso_item_switch()
		{
			ChkSwitchSC.Checked = false;
			ChkSwitchFPD.Checked = false;
			ChkSwitchDec.Checked = false;
			ChkSwitchMD.Checked = false;
			ChkSwitchAN.Checked = false;
			ChkSwitchGN.Checked = false;
			ChkSwitchUEC.Checked = false;
			ChkSwitchPG.Checked = false;
			ChkSwitchRM.Checked = false;
			ChkSwitchFID.Checked = false;
			ChkSwitchVID.Checked = false;
			if (GetCfgCBFuncCB(202241u) == 0)
			{
				ChkSwitchSC.Checked = true;
			}
			if (GetCfgCBFuncCB(202244u) == 0)
			{
				ChkSwitchFPD.Checked = true;
			}
			if (GetCfgCBFuncCB(202248u) == 0)
			{
				ChkSwitchDec.Checked = true;
			}
			if (GetCfgCBFuncCB(202242u) == 0)
			{
				ChkSwitchMD.Checked = true;
			}
			if (GetCfgCBFuncCB(202256u) == 0)
			{
				ChkSwitchAN.Checked = true;
			}
			if (GetCfgCBFuncCB(202272u) == 0)
			{
				ChkSwitchGN.Checked = true;
			}
			if (GetCfgCBFuncCB(202304u) == 0)
			{
				ChkSwitchUEC.Checked = true;
			}
			if (GetCfgCBFuncCB(202497u) == 0)
			{
				ChkSwitchPG.Checked = true;
			}
			if (GetCfgCBFuncCB(202368u) == 0)
			{
				ChkSwitchRM.Checked = true;
			}
			if (GetCfgCBFuncCB(202498u) == 0)
			{
				ChkSwitchFID.Checked = true;
			}
			if (GetCfgCBFuncCB(202500u) == 0)
			{
				ChkSwitchVID.Checked = true;
			}
		}

		public void FunctionOnOff(bool[] CapacityArr)
		{
			GrpCodeQuality.Enabled = CapacityArr[22];
			GrpGroupNet.Enabled = CapacityArr[23];
			GrbFtpSettomg.Enabled = CapacityArr[24];
		}

		public void UpdateLanguageUI()
		{
			int selectedIndex = CobMuiltCodeSet.SelectedIndex;
			CobMuiltCodeSet.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobMuiltCodeSet.Items.AddRange(new object[10] { "关闭多码识别", "识别多个条码", "只识别2个条码", "只识别3个条码", "只识别4个条码", "只识别5个条码", "只识别6个条码", "只识别7个条码", "只识别8个条码", "只识别9个条码" });
			}
			else
			{
				CobMuiltCodeSet.Items.AddRange(new object[10] { "MuiltCodeDisable", "Enable-Any", "2 Barcodes", "3 Barcodes", "4 Barcodes", "5 Barcodes", "6 Barcodes", "7 Barcodes", "8 Barcodes", "9 Barcodes" });
			}
			CobMuiltCodeSet.SelectedIndex = selectedIndex;
			CobMuiltCoreMode.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobMuiltCoreMode.Items.AddRange(new object[4] { "关闭加速", "运动流水线狂扫模式", "静态图像快读模式", "解码疯狂探索模式" });
			}
			else
			{
				CobMuiltCoreMode.Items.AddRange(new object[4] { "None", "MotionMode", "StaticMode", "EnhanceDecode" });
			}
			CobHostUploadMode.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobHostUploadMode.Items.AddRange(new object[2] { "主机从机读完上传才结束", "任意设备读完上传就结束" });
			}
			else
			{
				CobHostUploadMode.Items.AddRange(new object[2] { "EndAfterAllDeviceRead&Uploaded", "EndAfterOneOfDevicesRead&Uploaded" });
			}
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				ColFileName.Text = "文件名字";
				ColFileType.Text = "类型";
				ColFileSize.Text = "大小";
			}
			else
			{
				ColFileName.Text = "FileName";
				ColFileType.Text = "Type";
				ColFileSize.Text = "Size";
			}
		}

		public void AdvancedPage_Update(ProtocolExtraDataStu ProtocolExtraData)
		{
			uint num = ProtocolExtraData.ClientCount;
			if (num > ProtocolExtraData.ClientIPs.Length)
			{
				num = 0u;
			}
			LabClientCount.Text = num.ToString();
			LvwClientDisp.BeginUpdate();
			LvwClientDisp.Items.Clear();
			IPAddress iPAddress = IPAddress.Parse(ToolCfg.CurrentDevice.IpAddrStr);
			byte[] addressBytes = iPAddress.GetAddressBytes();
			try
			{
				for (int i = 0; i < num; i++)
				{
					ListViewItem listViewItem = LvwClientDisp.Items.Add(addressBytes[0] + "." + addressBytes[1] + "." + addressBytes[2] + "." + ProtocolExtraData.ClientIPs[i]);
				}
			}
			catch (Exception)
			{
			}
			LvwClientDisp.EndUpdate();
		}

		private void CobResolutionSet_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(51715u, (ushort)CobResolutionSet.SelectedIndex);
				SendCfgDataCBFuncCB(1026u);
			}
		}

		private void CobMuiltCodeSet_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobMuiltCodeSet.SelectedIndex != 0)
				{
					SetCfgCBFuncCB(48911u, (ushort)CobMuiltCodeSet.SelectedIndex);
					SetCfgCBFuncCB(11280u, 16u);
				}
				else
				{
					SetCfgCBFuncCB(11280u, 0u);
				}
				if (ToolCfg.DeviceTypeRecord >= 5)
				{
					SendCfgDataCBFuncCB(1024u);
				}
				else
				{
					SendCfgDataCBFuncCB(1026u);
				}
			}
		}

		private void CobBarcodeQualityMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobBarcodeQualityMode.SelectedIndex == 0)
				{
					SetCfgCBFuncCB(200711u, 0u);
				}
				else if (CobBarcodeQualityMode.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(200711u, 1u);
				}
				else if (CobBarcodeQualityMode.SelectedIndex == 2)
				{
					SetCfgCBFuncCB(200711u, 2u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbQualityResultOverall_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbQualityResultOverall.Checked)
			{
				SetCfgCBFuncCB(200728u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbQualityResultOrderAll_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbQualityResultOrderAll.Checked)
			{
				SetCfgCBFuncCB(200728u, 8u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbQualityOutputDescAndData_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbQualityOutputDescAndData.Checked)
			{
				SetCfgCBFuncCB(200728u, 16u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbQualityBarcodeSeparator_TextChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && TxbQualityBarcodeSeparator.TextLength > 0)
			{
				if (TxbQualityBarcodeSeparator.Text == "0")
				{
					SetCfgCBFuncCB(201215u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(201215u, (byte)TxbQualityBarcodeSeparator.Text[0]);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbQualityAllSeparator_TextChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && TxbQualityAllSeparator.TextLength > 0)
			{
				if (TxbQualityAllSeparator.Text == "0")
				{
					SetCfgCBFuncCB(201471u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(201471u, (byte)TxbQualityAllSeparator.Text[0]);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobForceLowerLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobForceLowerLevel.SelectedIndex < 0)
				{
					CobForceLowerLevel.SelectedIndex = 0;
				}
				switch (CobForceLowerLevel.SelectedIndex)
				{
				case 0:
					SetCfgCBFuncCB(201479u, 0u);
					break;
				case 1:
					SetCfgCBFuncCB(201479u, 4u);
					break;
				case 2:
					SetCfgCBFuncCB(201479u, 3u);
					break;
				case 3:
					SetCfgCBFuncCB(201479u, 2u);
					break;
				case 4:
					SetCfgCBFuncCB(201479u, 1u);
					break;
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbNetWorkingClose_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbNetWorkingClose.Checked)
			{
				SetCfgCBFuncCB(155651u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
			if (RdbNetWorkingClose.Checked)
			{
				PanOnlyForClient.Enabled = false;
				PanOnlyForServer.Enabled = false;
			}
		}

		private void RdbNetWorkingServer_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbNetWorkingServer.Checked)
			{
				SetCfgCBFuncCB(155651u, 2u);
				SendCfgDataCBFuncCB(0u);
			}
			if (RdbNetWorkingServer.Checked)
			{
				PanOnlyForClient.Enabled = false;
				PanOnlyForServer.Enabled = true;
			}
		}

		private void RdbClientMode_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbClientMode.Checked)
			{
				SetCfgCBFuncCB(155651u, 1u);
				SendCfgDataCBFuncCB(0u);
			}
			if (RdbClientMode.Checked)
			{
				PanOnlyForClient.Enabled = true;
				PanOnlyForServer.Enabled = false;
			}
		}

		private void ChkUseSameTrig_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChkUseSameTrig.Checked)
				{
					SetCfgCBFuncCB(155696u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(155696u, 48u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChkNetworkingSplitCh_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChkNetworkingSplitCh.Checked)
				{
					SetCfgCBFuncCB(158207u, 13u);
					TxbNetWorkingSplitCh.Enabled = true;
					TxbNetWorkingSplitCh.Text = "0D";
				}
				else
				{
					SetCfgCBFuncCB(158207u, 0u);
					TxbNetWorkingSplitCh.Enabled = false;
					TxbNetWorkingSplitCh.Text = "0D";
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbNetWorkingSplitCh_TextChanged(object sender, EventArgs e)
		{
			BtnNetWorkingSplitCh.Visible = true;
		}

		private void BtnNetWorkingSplitCh_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				byte result = 0;
				if (byte.TryParse(TxbNetWorkingSplitCh.Text, NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-GB"), out result))
				{
					SetCfgCBFuncCB(158207u, result);
					SendCfgDataCBFuncCB(0u);
					BtnNetWorkingSplitCh.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的十六进制", "错误");
				}
				else
				{
					MessageBox.Show("Please enter a Hex number ", "Error");
				}
			}
		}

		private void TxbDecodeWaitTime_TextChanged(object sender, EventArgs e)
		{
			BtnDecodeWaitTimeSet.Visible = true;
		}

		private void BtnDecodeWaitTimeSet_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result;
				if (ushort.TryParse(TxbDecodeWaitTime.Text, out result))
				{
					SetCfgCBFuncCB(156415u, (uint)(result >> 8) & 0xFFu);
					SetCfgCBFuncCB(156159u, result & 0xFFu);
					SendCfgDataCBFuncCB(0u);
					BtnDecodeWaitTimeSet.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的时间大于1 小于65535", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct time,1 < value < 65535 ", "Error");
				}
			}
		}

		private void CobHostUploadMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobHostUploadMode.SelectedIndex == 0)
				{
					SetCfgCBFuncCB(155660u, 0u);
				}
				else if (CobHostUploadMode.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(155660u, 8u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbHostSIP_Seg1_TextChanged(object sender, EventArgs e)
		{
			BtnHostSIP_Set.Visible = true;
		}

		private void TxbHostSIP_Seg2_TextChanged(object sender, EventArgs e)
		{
			BtnHostSIP_Set.Visible = true;
		}

		private void TxbHostSIP_Seg3_TextChanged(object sender, EventArgs e)
		{
			BtnHostSIP_Set.Visible = true;
		}

		private void TxbHostSIP_Seg4_TextChanged(object sender, EventArgs e)
		{
			BtnHostSIP_Set.Visible = true;
		}

		private void BtnHostSIP_Set_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			try
			{
				SetCfgCBFuncCB(156671u, uint.Parse(TxbHostSIP_Seg1.Text));
				SetCfgCBFuncCB(156927u, uint.Parse(TxbHostSIP_Seg2.Text));
				SetCfgCBFuncCB(157183u, uint.Parse(TxbHostSIP_Seg3.Text));
				SetCfgCBFuncCB(157439u, uint.Parse(TxbHostSIP_Seg4.Text));
				SendCfgDataCBFuncCB(0u);
				BtnHostSIP_Set.Visible = false;
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

		private void LabLogState_DoubleClick(object sender, EventArgs e)
		{
			record_click_for_log++;
			if (record_click_for_log > 4)
			{
				CobLogState.Enabled = true;
			}
		}

		private void CobLogState_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobLogState.SelectedIndex == 0)
				{
					SetCfgCBFuncCB(77831u, 0u);
				}
				else if (CobLogState.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(77831u, 3u);
				}
				else if (CobLogState.SelectedIndex == 2)
				{
					SetCfgCBFuncCB(77831u, 4u);
				}
				else if (CobLogState.SelectedIndex == 3)
				{
					SetCfgCBFuncCB(77831u, 5u);
				}
				else if (CobLogState.SelectedIndex == 4)
				{
					SetCfgCBFuncCB(77831u, 6u);
				}
				else if (CobLogState.SelectedIndex == 5)
				{
					SetCfgCBFuncCB(77831u, 7u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private bool IsValidFileName(string fileName)
		{
			bool result = true;
			string text = "\\/:*?\"<>|";
			for (int i = 0; i < text.Length; i++)
			{
				if (fileName.Contains(text[i].ToString()))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public static string ShowFileSize(long fileSize)
		{
			string result = "";
			if (fileSize < 1024)
			{
				result = fileSize + " Bytes";
			}
			else if (fileSize >= 1024 && fileSize < 1048576)
			{
				result = Math.Round((double)fileSize * 1.0 / 1024.0, 2, MidpointRounding.AwayFromZero) + " KB(" + fileSize + "Bytes)";
			}
			else if (fileSize >= 1048576 && fileSize < 1073741824)
			{
				result = Math.Round((double)fileSize * 1.0 / 1048576.0, 2, MidpointRounding.AwayFromZero) + " MB(" + fileSize + "Bytes)";
			}
			else if (fileSize >= 1073741824)
			{
				result = Math.Round((double)fileSize * 1.0 / 1073741824.0, 2, MidpointRounding.AwayFromZero) + " GB(" + fileSize + "Bytes)";
			}
			return result;
		}

		private void OpenFiles()
		{
			if (lvwFiles.SelectedItems[0].Text == "..")
			{
				if (CurrentDir != "")
				{
					int num = CurrentDir.LastIndexOf('/');
					if (num > 10)
					{
						CurrentDir = CurrentDir.Substring(0, num);
					}
					ShowFilesList(CurrentDir);
				}
			}
			else
			{
				if (lvwFiles.SelectedItems.Count <= 0 || !(lvwFiles.SelectedItems[0].Text != ".."))
				{
					return;
				}
				DirItemInfo dirItemInfo = (DirItemInfo)lvwFiles.SelectedItems[0].Tag;
				string text = dirItemInfo.FullFileName.ToString();
				try
				{
					if (dirItemInfo.FileType == 0)
					{
						ShowFilesList(text);
						CurrentDir = text;
						return;
					}
					string text2 = Path.GetTempPath() + "\\" + DateTime.UtcNow.GetHashCode().ToString("X4") + "_" + dirItemInfo.Name;
					byte[] array = ToolCfg.ftp.DownloadFile(text);
					if (array != null)
					{
						FileStream fileStream = new FileStream(text2, FileMode.OpenOrCreate);
						fileStream.Write(array, 0, array.Length);
						fileStream.Flush();
						fileStream.Close();
						Process.Start(text2);
					}
					else
					{
						MessageBox.Show("File Open Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		public void ShowFilesList(string ftpPath)
		{
			lvwFiles.BeginUpdate();
			lvwFiles.Items.Clear();
			try
			{
				List<DirItemInfo> ftpFileInfos = ToolCfg.ftp.GetFtpFileInfos(ftpPath);
				if (ftpFileInfos != null)
				{
					DirItemInfo[] array = ftpFileInfos.ToArray();
					ListViewItem listViewItem = lvwFiles.Items.Add("..", 5);
					listViewItem.SubItems.Add(MultLanguageText.Get_Content(MultLanguageText.TextDef.UpOneLevelText));
					listViewItem.SubItems.Add("");
					foreach (ListViewItem item in lvwFiles.Items)
					{
						if (item.Text.EndsWith(".exe"))
						{
							ilstIcons.Images.RemoveByKey(item.Text);
						}
					}
					DirItemInfo[] array2 = array;
					foreach (DirItemInfo dirItemInfo in array2)
					{
						if (dirItemInfo.FileType == 0)
						{
							ListViewItem listViewItem3 = lvwFiles.Items.Add(dirItemInfo.Name, 3);
							listViewItem3.Tag = dirItemInfo;
							listViewItem3.SubItems.Add(MultLanguageText.Get_Content(MultLanguageText.TextDef.FolderText));
							listViewItem3.SubItems.Add("");
							continue;
						}
						ListViewItem listViewItem4 = lvwFiles.Items.Add(dirItemInfo.Name);
						if (!ilstIcons.Images.ContainsKey(Path.GetExtension(dirItemInfo.FullFileName)))
						{
							Icon iconByFileName = GetSystemIcon.GetIconByFileName(Path.GetExtension(dirItemInfo.FullFileName));
							if (iconByFileName != null)
							{
								ilstIcons.Images.Add(Path.GetExtension(dirItemInfo.FullFileName), iconByFileName);
								listViewItem4.ImageKey = Path.GetExtension(dirItemInfo.FullFileName);
							}
							else
							{
								listViewItem4.ImageIndex = 1;
							}
						}
						else
						{
							listViewItem4.ImageKey = Path.GetExtension(dirItemInfo.FullFileName);
						}
						listViewItem4.Tag = dirItemInfo;
						listViewItem4.SubItems.Add(Path.GetExtension(dirItemInfo.FullFileName) + " file");
						listViewItem4.SubItems.Add(ShowFileSize(dirItemInfo.Size).Split('(')[0]);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			curFilePath = ftpPath;
			lvwFiles.EndUpdate();
		}

		private void ShowDeviceIpSettingForm()
		{
			ToolCfg.DeviceIPSettingPage.ShowDialog();
		}

		private void BtnConnectFTP_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice == null)
			{
				return;
			}
			DeviceFindAndCom.DeviceFound currentDevice = ToolCfg.CurrentDevice;
			if (currentDevice == null || currentDevice.ConnectType != DeviceFindAndCom.DeviceType.NETWORK)
			{
				return;
			}
			if (currentDevice.IsCommunicate)
			{
				ShowFilesList("ftp://" + currentDevice.IpAddrStr);
				TxbFtpIP.Text = currentDevice.IpAddrStr;
				return;
			}
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				text = "当前设备的ip:" + currentDevice.IpAddrStr + "，与选择的主机ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一个网段上\r\n请点击确定或取消按钮去选择或者修改主机ip到对应设备ip段上,\r\n也可以点击否按钮,把设备ip改成对应主机的网段上";
				caption = "注意";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "當前設備的ip:" + currentDevice.IpAddrStr + "，與選擇的主機ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一個網段上\r\n請點擊確定或取消按鈕去選擇或者修改主機ip到對應設備ip段上,\r\n也可以點擊否按鈕,把設備ip改成對應主機的網段上";
				caption = "注意";
			}
			else
			{
				text = "Current device ip:" + currentDevice.IpAddrStr + ",is not the same segment with host ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
			{
				ShowDeviceIpSettingForm();
			}
		}

		private void BtnDisconnectFTP_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice == null)
			{
				return;
			}
			DeviceFindAndCom.DeviceFound currentDevice = ToolCfg.CurrentDevice;
			if (currentDevice == null || currentDevice.ConnectType != DeviceFindAndCom.DeviceType.NETWORK)
			{
				return;
			}
			if (currentDevice.IsCommunicate)
			{
				ToolCfg.ftp.DisconnectFtp("ftp://" + currentDevice.IpAddrStr);
				lvwFiles.Items.Clear();
				return;
			}
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				text = "当前设备的ip:" + currentDevice.IpAddrStr + "，与选择的主机ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一个网段上\r\n请点击确定或取消按钮去选择或者修改主机ip到对应设备ip段上,\r\n也可以点击否按钮,把设备ip改成对应主机的网段上";
				caption = "注意";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "當前設備的ip:" + currentDevice.IpAddrStr + "，與選擇的主機ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一個網段上\r\n請點擊確定或取消按鈕去選擇或者修改主機ip到對應設備ip段上,\r\n也可以點擊否按鈕,把設備ip改成對應主機的網段上";
				caption = "注意";
			}
			else
			{
				text = "Current device ip:" + currentDevice.IpAddrStr + ",is not the same segment with host ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
			{
				ShowDeviceIpSettingForm();
			}
		}

		private void BtnFreshFtp_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice == null)
			{
				return;
			}
			DeviceFindAndCom.DeviceFound currentDevice = ToolCfg.CurrentDevice;
			if (currentDevice == null || currentDevice.ConnectType != DeviceFindAndCom.DeviceType.NETWORK)
			{
				return;
			}
			if (currentDevice.IsCommunicate)
			{
				string ftpPath = ((!(CurrentDir == "")) ? CurrentDir : ("ftp://" + currentDevice.IpAddrStr));
				ShowFilesList(ftpPath);
				return;
			}
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				text = "当前设备的ip:" + currentDevice.IpAddrStr + "，与选择的主机ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一个网段上\r\n请点击确定或取消按钮去选择或者修改主机ip到对应设备ip段上,\r\n也可以点击否按钮,把设备ip改成对应主机的网段上";
				caption = "注意";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "當前設備的ip:" + currentDevice.IpAddrStr + "，與選擇的主機ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一個網段上\r\n請點擊確定或取消按鈕去選擇或者修改主機ip到對應設備ip段上,\r\n也可以點擊否按鈕,把設備ip改成對應主機的網段上";
				caption = "注意";
			}
			else
			{
				text = "Current device ip:" + currentDevice.IpAddrStr + ",is not the same segment with host ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
			{
				ShowDeviceIpSettingForm();
			}
		}

		private void lvwFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			string label = e.Label;
			lvwFiles.LabelEdit = false;
			ListViewItem listViewItem = lvwFiles.SelectedItems[0];
			if (string.IsNullOrEmpty(label))
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("文件名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show("File name can not be emty！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				e.CancelEdit = true;
			}
			else
			{
				if (label == null || label == listViewItem.Text)
				{
					return;
				}
				if (!IsValidFileName(label))
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("文件名不能包含下列任何字符:\r\n\t\\/:*?\"<>|", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					else
					{
						MessageBox.Show("File can not include character:\r\n\t\\/:*?\"<>|", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					e.CancelEdit = true;
					return;
				}
				DirItemInfo dirItemInfo = (DirItemInfo)lvwFiles.SelectedItems[0].Tag;
				string fileName = dirItemInfo.Name.ToString();
				if (CurrentDir == "")
				{
					CurrentDir = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr;
				}
				ToolCfg.ftp.RenameFile(CurrentDir, fileName, label);
				ShowFilesList(CurrentDir);
			}
		}

		private void FileUploadProc(object state)
		{
			string text = (string)state;
			string ftpFile = ((!(CurrentDir == "")) ? (CurrentDir + "/" + Path.GetFileName(text)) : ("ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/" + Path.GetFileName(text)));
			if (IsUpgradeFirmware)
			{
				ftpFile = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr + "/upgrade/" + Path.GetFileName(text);
			}
			ToolCfg.ftp.UploadFile(text, ftpFile);
			if (!Path.GetFileName(text).ToLower().Contains(".bin"))
			{
				Invoke((MethodInvoker)delegate
				{
					ShowFilesList(CurrentDir);
				});
				return;
			}
			Invoke((MethodInvoker)delegate
			{
				base.Enabled = true;
				FormProcessBar.Close();
			});
		}

		private void lvwFiles_DragDrop(object sender, DragEventArgs e)
		{
			DeviceFindAndCom.DeviceFound currentDevice = ToolCfg.CurrentDevice;
			if (currentDevice == null || currentDevice.ConnectType != DeviceFindAndCom.DeviceType.NETWORK)
			{
				return;
			}
			if (currentDevice.IsCommunicate)
			{
				string filePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
				UploadFileToDevice(filePath);
				return;
			}
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				text = "当前设备的ip:" + currentDevice.IpAddrStr + "，与选择的主机ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一个网段上\r\n请点击确定或取消按钮去选择或者修改主机ip到对应设备ip段上,\r\n也可以点击否按钮,把设备ip改成对应主机的网段上";
				caption = "注意";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "當前設備的ip:" + currentDevice.IpAddrStr + "，與選擇的主機ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一個網段上\r\n請點擊確定或取消按鈕去選擇或者修改主機ip到對應設備ip段上,\r\n也可以點擊否按鈕,把設備ip改成對應主機的網段上";
				caption = "注意";
			}
			else
			{
				text = "Current device ip:" + currentDevice.IpAddrStr + ",is not the same segment with host ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
			{
				ShowDeviceIpSettingForm();
			}
		}

		private void lvwFiles_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void lvwFiles_ItemActivate(object sender, EventArgs e)
		{
			OpenFiles();
		}

		private void lvwFiles_MouseDown(object sender, MouseEventArgs e)
		{
			ListView control = (ListView)sender;
			if (e.Button == MouseButtons.Right)
			{
				cmsMain.Show(control, e.X, e.Y);
			}
		}

		private void CobMuiltCoreMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobMuiltCoreMode.SelectedIndex < 0)
				{
					CobMuiltCoreMode.SelectedIndex = 0;
				}
				switch (CobMuiltCoreMode.SelectedIndex)
				{
				case 0:
					SetCfgCBFuncCB(139783u, 0u);
					break;
				case 1:
					SetCfgCBFuncCB(139783u, 1u);
					break;
				case 2:
					SetCfgCBFuncCB(139783u, 2u);
					break;
				case 3:
					SetCfgCBFuncCB(139783u, 4u);
					break;
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void tsmiDownload_Click(object sender, EventArgs e)
		{
			if (lvwFiles.SelectedItems.Count <= 0 || !(lvwFiles.SelectedItems[0].Text != "..") || ToolCfg.CurrentDevice == null || !ToolCfg.CurrentDevice.IsConnect)
			{
				return;
			}
			DirItemInfo dirItemInfo = (DirItemInfo)lvwFiles.SelectedItems[0].Tag;
			string fileName = dirItemInfo.FullFileName.ToString();
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.FileName = dirItemInfo.Name;
			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			string fileName2 = saveFileDialog.FileName;
			if (dirItemInfo.FileType == 0)
			{
				if (!ToolCfg.ftp.DownloadDirectory(dirItemInfo.FullFileName.Replace(dirItemInfo.Name, ""), fileName2, dirItemInfo.Name))
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("文件夹保存失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					else
					{
						MessageBox.Show("Folder save failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				return;
			}
			byte[] array = ToolCfg.ftp.DownloadFile(fileName);
			if (array != null)
			{
				FileStream fileStream = new FileStream(fileName2, FileMode.OpenOrCreate);
				fileStream.Write(array, 0, array.Length);
				fileStream.Flush();
				fileStream.Close();
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("文件保存失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				MessageBox.Show("File save failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void tsmiUpload_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == DialogResult.OK && ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
			{
				string fileName = openFileDialog.FileName;
				UploadFileToDevice(fileName);
			}
		}

		private void DeleteFiles()
		{
			if (lvwFiles.SelectedItems.Count <= 0)
			{
				return;
			}
			DialogResult dialogResult = ((!ToolCfg.ConfigPath.Contains("ChineseS") && !ToolCfg.ConfigPath.Contains("ChineseT")) ? MessageBox.Show("Are you sure you want to delete it?", "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) : MessageBox.Show("确定要删除吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation));
			if (dialogResult == DialogResult.No)
			{
				return;
			}
			try
			{
				foreach (ListViewItem selectedItem in lvwFiles.SelectedItems)
				{
					DirItemInfo dirItemInfo = (DirItemInfo)lvwFiles.SelectedItems[0].Tag;
					string text = dirItemInfo.FullFileName.ToString();
					if (dirItemInfo.FileType == 1)
					{
						ToolCfg.ftp.DeleteFile(dirItemInfo.FullFileName.Replace(dirItemInfo.Name, ""), dirItemInfo.Name);
					}
					else
					{
						ToolCfg.ftp.DeleteDir(dirItemInfo.FullFileName.Replace(dirItemInfo.Name, ""), dirItemInfo.Name);
					}
					lvwFiles.Items.Remove(selectedItem);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void tsmiDelete_Click(object sender, EventArgs e)
		{
			if (lvwFiles.SelectedItems.Count > 0 && lvwFiles.SelectedItems[0].Text != ".." && ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
			{
				DeleteFiles();
			}
		}

		private void tsmiReflesh_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice == null || ToolCfg.CurrentDevice.ConnectType != DeviceFindAndCom.DeviceType.NETWORK)
			{
				return;
			}
			if (ToolCfg.CurrentDevice.IsCommunicate)
			{
				string ftpPath = ((!(CurrentDir == "")) ? CurrentDir : ("ftp://" + ToolCfg.CurrentDevice.IpAddrStr));
				ShowFilesList(ftpPath);
				return;
			}
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				text = "当前设备的ip:" + ToolCfg.CurrentDevice.IpAddrStr + "，与选择的主机ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一个网段上\r\n请点击确定或取消按钮去选择或者修改主机ip到对应设备ip段上,\r\n也可以点击否按钮,把设备ip改成对应主机的网段上";
				caption = "注意";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "當前設備的ip:" + ToolCfg.CurrentDevice.IpAddrStr + "，與選擇的主機ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一個網段上\r\n請點擊確定或取消按鈕去選擇或者修改主機ip到對應設備ip段上,\r\n也可以點擊否按鈕,把設備ip改成對應主機的網段上";
				caption = "注意";
			}
			else
			{
				text = "Current device ip:" + ToolCfg.CurrentDevice.IpAddrStr + ",is not the same segment with host ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
			{
				ShowDeviceIpSettingForm();
			}
		}

		private void RenameFile()
		{
			if (lvwFiles.SelectedItems.Count > 0 && lvwFiles.SelectedItems[0].Text != "..")
			{
				lvwFiles.LabelEdit = true;
				lvwFiles.SelectedItems[0].BeginEdit();
			}
		}

		private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RenameFile();
		}

		private void MkdirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice == null || ToolCfg.CurrentDevice.ConnectType != DeviceFindAndCom.DeviceType.NETWORK)
			{
				return;
			}
			if (ToolCfg.CurrentDevice.IsCommunicate)
			{
				if (CurrentDir == "")
				{
					CurrentDir = "ftp://" + ToolCfg.CurrentDevice.IpAddrStr;
					foldercnt++;
				}
				ToolCfg.ftp.MakeDir(CurrentDir, "NewFolder" + foldercnt);
				foldercnt++;
				ShowFilesList(CurrentDir);
				return;
			}
			string text;
			string caption;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				text = "当前设备的ip:" + ToolCfg.CurrentDevice.IpAddrStr + "，与选择的主机ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一个网段上\r\n请点击确定或取消按钮去选择或者修改主机ip到对应设备ip段上,\r\n也可以点击否按钮,把设备ip改成对应主机的网段上";
				caption = "注意";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				text = "當前設備的ip:" + ToolCfg.CurrentDevice.IpAddrStr + "，與選擇的主機ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一個網段上\r\n請點擊確定或取消按鈕去選擇或者修改主機ip到對應設備ip段上,\r\n也可以點擊否按鈕,把設備ip改成對應主機的網段上";
				caption = "注意";
			}
			else
			{
				text = "Current device ip:" + ToolCfg.CurrentDevice.IpAddrStr + ",is not the same segment with host ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
				caption = "Pay Attention";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
			{
				ShowDeviceIpSettingForm();
			}
		}

		private void cmsMain_Opening(object sender, CancelEventArgs e)
		{
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				tsmiDownload.Text = "下载";
				tsmiUpload.Text = "上传";
				tsmiDelete.Text = "删除";
				tsmiReflesh.Text = "刷新";
				RenameToolStripMenuItem.Text = "重命名";
				MkdirToolStripMenuItem.Text = "新建文件夹";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				tsmiDownload.Text = "下載";
				tsmiUpload.Text = "上傳";
				tsmiDelete.Text = "刪除";
				tsmiReflesh.Text = "刷新";
				RenameToolStripMenuItem.Text = "重命名";
				MkdirToolStripMenuItem.Text = "新建資料夾";
			}
			else
			{
				tsmiDownload.Text = "Download ";
				tsmiUpload.Text = "Upload ";
				tsmiDelete.Text = "Delete ";
				tsmiReflesh.Text = "Refresh ";
				RenameToolStripMenuItem.Text = "Renamed";
				MkdirToolStripMenuItem.Text = "New Folder";
			}
		}

		public bool CommunicationCheck()
		{
			DeviceFindAndCom.DeviceFound currentDevice = ToolCfg.CurrentDevice;
			if (currentDevice != null && currentDevice.ConnectType == DeviceFindAndCom.DeviceType.NETWORK)
			{
				if (currentDevice.IsCommunicate)
				{
					return ToolCfg.ftp.FtpConnectCheck();
				}
				string text;
				string caption;
				if (ToolCfg.ConfigPath.Contains("ChineseS"))
				{
					text = "当前设备的ip:" + currentDevice.IpAddrStr + "，与选择的主机ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一个网段上\r\n请点击确定或取消按钮去选择或者修改主机ip到对应设备ip段上,\r\n也可以点击否按钮,把设备ip改成对应主机的网段上";
					caption = "注意";
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					text = "當前設備的ip:" + currentDevice.IpAddrStr + "，與選擇的主機ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "不在同一個網段上\r\n請點擊確定或取消按鈕去選擇或者修改主機ip到對應設備ip段上,\r\n也可以點擊否按鈕,把設備ip改成對應主機的網段上";
					caption = "注意";
				}
				else
				{
					text = "Current device ip:" + currentDevice.IpAddrStr + ",is not the same segment with host ip:" + ToolCfg.IpAddrSeg0 + "." + ToolCfg.IpAddrSeg1 + "." + ToolCfg.IpAddrSeg2 + "." + ToolCfg.IpAddrSeg3 + "\r\nPlease select or modify the host IP to the corresponding device IP segment,\r\nor click No to change the device IP to same segment of the corresponding host";
					caption = "Pay Attention";
				}
				if (MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel) == DialogResult.No)
				{
					ShowDeviceIpSettingForm();
				}
			}
			return false;
		}

		public void UploadFileToDevice(string FilePath, bool UpgradeFirmware = false)
		{
			IsUpgradeFirmware = UpgradeFirmware;
			if (FilePath.ToLower().Contains(".bin"))
			{
				string text;
				string caption;
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					text = "检测到当前您上传的文件是设备固件，\r\n点击确定进行上传，上传完成后设备会进行自动升级并重启设备。\r\n上传和升级需要时间较长，估计不会超过5分钟，请耐心等待";
					caption = "提示";
				}
				else
				{
					text = "It is detected that the file you are uploading is the device firmware,\r\nClick OK to upload. After uploading, the device will automatically upgrade and restart the device.\r\nIt take some time to upload and upgrade. Estimated that it will not exceed 5 minutes. Please wait patiently";
					caption = "提示";
				}
				if (MessageBox.Show(text, caption, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					ThreadPool.QueueUserWorkItem(FileUploadProc, FilePath);
					base.Enabled = false;
					FormProcessBar = new ProcessBarForm();
					FormProcessBar.Show();
				}
			}
			else
			{
				ThreadPool.QueueUserWorkItem(FileUploadProc, FilePath);
			}
		}

		private void ChkSwitchSC_Click(object sender, EventArgs e)
		{
			if (ChkSwitchSC.Checked)
			{
				SetCfgCBFuncCB(202241u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202241u, 1u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchFPD_Click(object sender, EventArgs e)
		{
			if (ChkSwitchFPD.Checked)
			{
				SetCfgCBFuncCB(202244u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202244u, 4u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchDec_Click(object sender, EventArgs e)
		{
			if (ChkSwitchDec.Checked)
			{
				SetCfgCBFuncCB(202248u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202248u, 8u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchMD_Click(object sender, EventArgs e)
		{
			if (ChkSwitchMD.Checked)
			{
				SetCfgCBFuncCB(202242u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202242u, 2u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchAN_Click(object sender, EventArgs e)
		{
			if (ChkSwitchAN.Checked)
			{
				SetCfgCBFuncCB(202256u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202256u, 16u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchGN_Click(object sender, EventArgs e)
		{
			if (ChkSwitchGN.Checked)
			{
				SetCfgCBFuncCB(202272u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202272u, 32u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchUEC_Click(object sender, EventArgs e)
		{
			if (ChkSwitchUEC.Checked)
			{
				SetCfgCBFuncCB(202304u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202304u, 64u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchPG_Click(object sender, EventArgs e)
		{
			if (ChkSwitchPG.Checked)
			{
				SetCfgCBFuncCB(202497u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202497u, 1u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchRM_Click(object sender, EventArgs e)
		{
			if (ChkSwitchRM.Checked)
			{
				SetCfgCBFuncCB(202368u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202368u, 128u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchFID_Click(object sender, EventArgs e)
		{
			if (ChkSwitchFID.Checked)
			{
				SetCfgCBFuncCB(202498u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202498u, 2u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void ChkSwitchVID_Click(object sender, EventArgs e)
		{
			if (ChkSwitchVID.Checked)
			{
				SetCfgCBFuncCB(202500u, 0u);
			}
			else
			{
				SetCfgCBFuncCB(202500u, 4u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void CobCauseNGOut_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobCauseNGOut.SelectedIndex < 0)
				{
					CobCauseNGOut.SelectedIndex = 0;
				}
				switch (CobCauseNGOut.SelectedIndex)
				{
				case 0:
					SetCfgCBFuncCB(201496u, 0u);
					break;
				case 1:
					SetCfgCBFuncCB(201496u, 8u);
					break;
				case 2:
					SetCfgCBFuncCB(201496u, 16u);
					break;
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobCauseNG_TH_Set_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobCauseNG_TH_Set.SelectedIndex < 0)
				{
					CobCauseNG_TH_Set.SelectedIndex = 0;
				}
				switch (CobCauseNG_TH_Set.SelectedIndex)
				{
				case 0:
					SetCfgCBFuncCB(201696u, 0u);
					break;
				case 1:
					SetCfgCBFuncCB(201696u, 32u);
					break;
				case 2:
					SetCfgCBFuncCB(201696u, 64u);
					break;
				case 3:
					SetCfgCBFuncCB(201696u, 96u);
					break;
				case 4:
					SetCfgCBFuncCB(201696u, 128u);
					break;
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobTryMultCode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobTryMultCode.SelectedIndex < 0)
				{
					CobTryMultCode.SelectedIndex = 0;
				}
				if (CobTryMultCode.SelectedIndex > 0)
				{
					SetCfgCBFuncCB(68355u, 2u);
				}
				else
				{
					SetCfgCBFuncCB(68355u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Heroje_Debug_Tool.SubForm.AdvancedSettingForm));
			this.GrbFtpSettomg = new System.Windows.Forms.GroupBox();
			this.TxbFTPAccount = new System.Windows.Forms.TextBox();
			this.TxbFtpIP = new System.Windows.Forms.TextBox();
			this.LabFtpPassword = new System.Windows.Forms.Label();
			this.LabAccount = new System.Windows.Forms.Label();
			this.LabFtpIPADDR = new System.Windows.Forms.Label();
			this.lvwFiles = new System.Windows.Forms.ListView();
			this.ColFileName = new System.Windows.Forms.ColumnHeader();
			this.ColFileType = new System.Windows.Forms.ColumnHeader();
			this.ColFileSize = new System.Windows.Forms.ColumnHeader();
			this.ilstIcons = new System.Windows.Forms.ImageList(this.components);
			this.BtnFreshFtp = new System.Windows.Forms.Button();
			this.BtnDisconnectFTP = new System.Windows.Forms.Button();
			this.BtnConnectFTP = new System.Windows.Forms.Button();
			this.TxbFtpPassword = new System.Windows.Forms.TextBox();
			this.GrbDeviceADSetting = new System.Windows.Forms.GroupBox();
			this.CobMuiltCodeSet = new System.Windows.Forms.ComboBox();
			this.LabMuiltCode = new System.Windows.Forms.Label();
			this.LabResolution = new System.Windows.Forms.Label();
			this.CobResolutionSet = new System.Windows.Forms.ComboBox();
			this.PanResultOutputWay = new System.Windows.Forms.Panel();
			this.RdbQualityResultOverall = new System.Windows.Forms.RadioButton();
			this.RdbQualityResultOrderAll = new System.Windows.Forms.RadioButton();
			this.LabResultOutputWay = new System.Windows.Forms.Label();
			this.RdbQualityOutputDescAndData = new System.Windows.Forms.RadioButton();
			this.CobForceLowerLevel = new System.Windows.Forms.ComboBox();
			this.TxbQualityAllSeparator = new System.Windows.Forms.TextBox();
			this.LabQualityAllSeparator = new System.Windows.Forms.Label();
			this.TxbQualityBarcodeSeparator = new System.Windows.Forms.TextBox();
			this.LabQualityBarcodeSeparator = new System.Windows.Forms.Label();
			this.LabBarcodeQualityDisp = new System.Windows.Forms.Label();
			this.CobBarcodeQualityMode = new System.Windows.Forms.ComboBox();
			this.LabForceLowerLevel = new System.Windows.Forms.Label();
			this.PanOnlyForServer = new System.Windows.Forms.Panel();
			this.CobHostUploadMode = new System.Windows.Forms.ComboBox();
			this.LabHostUploadMode = new System.Windows.Forms.Label();
			this.LabDecodeWaitTimeUnit = new System.Windows.Forms.Label();
			this.LabDecodeWaitTime = new System.Windows.Forms.Label();
			this.BtnDecodeWaitTimeSet = new System.Windows.Forms.Button();
			this.TxbDecodeWaitTime = new System.Windows.Forms.TextBox();
			this.BtnNetWorkingSplitCh = new System.Windows.Forms.Button();
			this.TxbNetWorkingSplitCh = new System.Windows.Forms.TextBox();
			this.ChkUseSameTrig = new System.Windows.Forms.CheckBox();
			this.ChkNetworkingSplitCh = new System.Windows.Forms.CheckBox();
			this.PanOnlyForClient = new System.Windows.Forms.Panel();
			this.BtnHostSIP_Set = new System.Windows.Forms.Button();
			this.TxbHostSIP_Seg4 = new System.Windows.Forms.TextBox();
			this.TxbHostSIP_Seg3 = new System.Windows.Forms.TextBox();
			this.TxbHostSIP_Seg2 = new System.Windows.Forms.TextBox();
			this.TxbHostSIP_Seg1 = new System.Windows.Forms.TextBox();
			this.LabNetworkingHostIP = new System.Windows.Forms.Label();
			this.RdbClientMode = new System.Windows.Forms.RadioButton();
			this.RdbNetWorkingServer = new System.Windows.Forms.RadioButton();
			this.RdbNetWorkingClose = new System.Windows.Forms.RadioButton();
			this.CobLogState = new System.Windows.Forms.ComboBox();
			this.LabLogState = new System.Windows.Forms.Label();
			this.LvwClientDisp = new System.Windows.Forms.ListView();
			this.LabClientCount = new System.Windows.Forms.Label();
			this.LabClientCountDisp = new System.Windows.Forms.Label();
			this.GrbMuiltCoreAls = new System.Windows.Forms.GroupBox();
			this.CobMuiltCoreMode = new System.Windows.Forms.ComboBox();
			this.LabMuiltCoreMode = new System.Windows.Forms.Label();
			this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiDownload = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiUpload = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiReflesh = new System.Windows.Forms.ToolStripMenuItem();
			this.RenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MkdirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GrpCodeQuality = new System.Windows.Forms.GroupBox();
			this.CobCauseNG_TH_Set = new System.Windows.Forms.ComboBox();
			this.LabCauseNG_TH_Set = new System.Windows.Forms.Label();
			this.CobCauseNGOut = new System.Windows.Forms.ComboBox();
			this.LabCauseNGOut = new System.Windows.Forms.Label();
			this.PanChkSwitch = new System.Windows.Forms.Panel();
			this.ChkSwitchVID = new System.Windows.Forms.CheckBox();
			this.ChkSwitchDec = new System.Windows.Forms.CheckBox();
			this.ChkSwitchFID = new System.Windows.Forms.CheckBox();
			this.ChkSwitchSC = new System.Windows.Forms.CheckBox();
			this.ChkSwitchPG = new System.Windows.Forms.CheckBox();
			this.ChkSwitchMD = new System.Windows.Forms.CheckBox();
			this.ChkSwitchUEC = new System.Windows.Forms.CheckBox();
			this.ChkSwitchFPD = new System.Windows.Forms.CheckBox();
			this.ChkSwitchGN = new System.Windows.Forms.CheckBox();
			this.ChkSwitchAN = new System.Windows.Forms.CheckBox();
			this.ChkSwitchRM = new System.Windows.Forms.CheckBox();
			this.GrpGroupNet = new System.Windows.Forms.GroupBox();
			this.GrpState = new System.Windows.Forms.GroupBox();
			this.TabMultSettings = new System.Windows.Forms.TabControl();
			this.TpIso = new System.Windows.Forms.TabPage();
			this.TpTheOthers = new System.Windows.Forms.TabPage();
			this.CobTryMultCode = new System.Windows.Forms.ComboBox();
			this.LabTryMultCode = new System.Windows.Forms.Label();
			this.GrbFtpSettomg.SuspendLayout();
			this.GrbDeviceADSetting.SuspendLayout();
			this.PanResultOutputWay.SuspendLayout();
			this.PanOnlyForServer.SuspendLayout();
			this.PanOnlyForClient.SuspendLayout();
			this.GrbMuiltCoreAls.SuspendLayout();
			this.cmsMain.SuspendLayout();
			this.GrpCodeQuality.SuspendLayout();
			this.PanChkSwitch.SuspendLayout();
			this.GrpGroupNet.SuspendLayout();
			this.GrpState.SuspendLayout();
			this.TabMultSettings.SuspendLayout();
			this.TpIso.SuspendLayout();
			this.TpTheOthers.SuspendLayout();
			base.SuspendLayout();
			this.GrbFtpSettomg.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.GrbFtpSettomg.Controls.Add(this.TxbFTPAccount);
			this.GrbFtpSettomg.Controls.Add(this.TxbFtpIP);
			this.GrbFtpSettomg.Controls.Add(this.LabFtpPassword);
			this.GrbFtpSettomg.Controls.Add(this.LabAccount);
			this.GrbFtpSettomg.Controls.Add(this.LabFtpIPADDR);
			this.GrbFtpSettomg.Controls.Add(this.lvwFiles);
			this.GrbFtpSettomg.Controls.Add(this.BtnFreshFtp);
			this.GrbFtpSettomg.Controls.Add(this.BtnDisconnectFTP);
			this.GrbFtpSettomg.Controls.Add(this.BtnConnectFTP);
			this.GrbFtpSettomg.Controls.Add(this.TxbFtpPassword);
			this.GrbFtpSettomg.Location = new System.Drawing.Point(477, 12);
			this.GrbFtpSettomg.Name = "GrbFtpSettomg";
			this.GrbFtpSettomg.Size = new System.Drawing.Size(439, 617);
			this.GrbFtpSettomg.TabIndex = 12;
			this.GrbFtpSettomg.TabStop = false;
			this.GrbFtpSettomg.Text = "设备FTP文件服务器";
			this.TxbFTPAccount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.TxbFTPAccount.Location = new System.Drawing.Point(186, 590);
			this.TxbFTPAccount.Name = "TxbFTPAccount";
			this.TxbFTPAccount.Size = new System.Drawing.Size(39, 23);
			this.TxbFTPAccount.TabIndex = 16;
			this.TxbFTPAccount.Text = "root";
			this.TxbFTPAccount.Visible = false;
			this.TxbFtpIP.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.TxbFtpIP.Location = new System.Drawing.Point(25, 591);
			this.TxbFtpIP.Name = "TxbFtpIP";
			this.TxbFtpIP.Size = new System.Drawing.Size(99, 23);
			this.TxbFtpIP.TabIndex = 14;
			this.TxbFtpIP.Text = "192.168.1.91";
			this.TxbFtpIP.Visible = false;
			this.LabFtpPassword.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabFtpPassword.AutoSize = true;
			this.LabFtpPassword.Location = new System.Drawing.Point(224, 594);
			this.LabFtpPassword.Name = "LabFtpPassword";
			this.LabFtpPassword.Size = new System.Drawing.Size(70, 14);
			this.LabFtpPassword.TabIndex = 17;
			this.LabFtpPassword.Text = "Password:";
			this.LabFtpPassword.Visible = false;
			this.LabAccount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabAccount.AutoSize = true;
			this.LabAccount.Location = new System.Drawing.Point(124, 594);
			this.LabAccount.Name = "LabAccount";
			this.LabAccount.Size = new System.Drawing.Size(63, 14);
			this.LabAccount.TabIndex = 15;
			this.LabAccount.Text = "Account:";
			this.LabAccount.Visible = false;
			this.LabFtpIPADDR.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabFtpIPADDR.AutoSize = true;
			this.LabFtpIPADDR.Location = new System.Drawing.Point(0, 594);
			this.LabFtpIPADDR.Name = "LabFtpIPADDR";
			this.LabFtpIPADDR.Size = new System.Drawing.Size(28, 14);
			this.LabFtpIPADDR.TabIndex = 13;
			this.LabFtpIPADDR.Text = "IP:";
			this.LabFtpIPADDR.Visible = false;
			this.lvwFiles.AllowDrop = true;
			this.lvwFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.lvwFiles.AutoArrange = false;
			this.lvwFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3] { this.ColFileName, this.ColFileType, this.ColFileSize });
			this.lvwFiles.Font = new System.Drawing.Font("微软雅黑", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			this.lvwFiles.FullRowSelect = true;
			this.lvwFiles.HideSelection = false;
			this.lvwFiles.Location = new System.Drawing.Point(3, 60);
			this.lvwFiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.lvwFiles.Name = "lvwFiles";
			this.lvwFiles.Size = new System.Drawing.Size(430, 524);
			this.lvwFiles.SmallImageList = this.ilstIcons;
			this.lvwFiles.TabIndex = 12;
			this.lvwFiles.UseCompatibleStateImageBehavior = false;
			this.lvwFiles.View = System.Windows.Forms.View.Details;
			this.lvwFiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(lvwFiles_AfterLabelEdit);
			this.lvwFiles.ItemActivate += new System.EventHandler(lvwFiles_ItemActivate);
			this.lvwFiles.DragDrop += new System.Windows.Forms.DragEventHandler(lvwFiles_DragDrop);
			this.lvwFiles.DragEnter += new System.Windows.Forms.DragEventHandler(lvwFiles_DragEnter);
			this.lvwFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(lvwFiles_MouseDown);
			this.ColFileName.Text = "文件名字";
			this.ColFileName.Width = 220;
			this.ColFileType.Text = "类型";
			this.ColFileType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ColFileType.Width = 100;
			this.ColFileSize.Text = "大小";
			this.ColFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ColFileSize.Width = 100;
			this.ilstIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ilstIcons.ImageStream");
			this.ilstIcons.TransparentColor = System.Drawing.SystemColors.Control;
			this.ilstIcons.Images.SetKeyName(0, "disk.png");
			this.ilstIcons.Images.SetKeyName(1, "CD_ROM.png");
			this.ilstIcons.Images.SetKeyName(2, "u_pan.png");
			this.ilstIcons.Images.SetKeyName(3, "folder.png");
			this.ilstIcons.Images.SetKeyName(4, "recent.png");
			this.ilstIcons.Images.SetKeyName(5, "return.png");
			this.BtnFreshFtp.Location = new System.Drawing.Point(224, 22);
			this.BtnFreshFtp.Name = "BtnFreshFtp";
			this.BtnFreshFtp.Size = new System.Drawing.Size(105, 31);
			this.BtnFreshFtp.TabIndex = 11;
			this.BtnFreshFtp.Text = "刷新列表";
			this.BtnFreshFtp.UseVisualStyleBackColor = true;
			this.BtnFreshFtp.Click += new System.EventHandler(BtnFreshFtp_Click);
			this.BtnDisconnectFTP.Location = new System.Drawing.Point(115, 22);
			this.BtnDisconnectFTP.Name = "BtnDisconnectFTP";
			this.BtnDisconnectFTP.Size = new System.Drawing.Size(106, 31);
			this.BtnDisconnectFTP.TabIndex = 10;
			this.BtnDisconnectFTP.Text = "断开FTP";
			this.BtnDisconnectFTP.UseVisualStyleBackColor = true;
			this.BtnDisconnectFTP.Click += new System.EventHandler(BtnDisconnectFTP_Click);
			this.BtnConnectFTP.Location = new System.Drawing.Point(6, 22);
			this.BtnConnectFTP.Name = "BtnConnectFTP";
			this.BtnConnectFTP.Size = new System.Drawing.Size(106, 31);
			this.BtnConnectFTP.TabIndex = 9;
			this.BtnConnectFTP.Text = "连接FTP";
			this.BtnConnectFTP.UseVisualStyleBackColor = true;
			this.BtnConnectFTP.Click += new System.EventHandler(BtnConnectFTP_Click);
			this.TxbFtpPassword.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.TxbFtpPassword.Location = new System.Drawing.Point(290, 590);
			this.TxbFtpPassword.Name = "TxbFtpPassword";
			this.TxbFtpPassword.Size = new System.Drawing.Size(41, 23);
			this.TxbFtpPassword.TabIndex = 18;
			this.TxbFtpPassword.Text = "hj168";
			this.TxbFtpPassword.Visible = false;
			this.GrbDeviceADSetting.Controls.Add(this.CobMuiltCodeSet);
			this.GrbDeviceADSetting.Controls.Add(this.LabMuiltCode);
			this.GrbDeviceADSetting.Controls.Add(this.LabResolution);
			this.GrbDeviceADSetting.Controls.Add(this.CobResolutionSet);
			this.GrbDeviceADSetting.Font = new System.Drawing.Font("宋体", 10.5f);
			this.GrbDeviceADSetting.Location = new System.Drawing.Point(12, 12);
			this.GrbDeviceADSetting.Name = "GrbDeviceADSetting";
			this.GrbDeviceADSetting.Size = new System.Drawing.Size(451, 86);
			this.GrbDeviceADSetting.TabIndex = 9;
			this.GrbDeviceADSetting.TabStop = false;
			this.GrbDeviceADSetting.Text = "设备高级设置";
			this.CobMuiltCodeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobMuiltCodeSet.Font = new System.Drawing.Font("宋体", 10.5f);
			this.CobMuiltCodeSet.FormattingEnabled = true;
			this.CobMuiltCodeSet.Items.AddRange(new object[10] { "关闭多码识别", "识别多个条码", "只识别2个条码", "只识别3个条码", "只识别4个条码", "只识别5个条码", "只识别6个条码", "只识别7个条码", "只识别8个条码", "只识别9个条码" });
			this.CobMuiltCodeSet.Location = new System.Drawing.Point(219, 51);
			this.CobMuiltCodeSet.Name = "CobMuiltCodeSet";
			this.CobMuiltCodeSet.Size = new System.Drawing.Size(148, 22);
			this.CobMuiltCodeSet.TabIndex = 7;
			this.CobMuiltCodeSet.SelectedIndexChanged += new System.EventHandler(CobMuiltCodeSet_SelectedIndexChanged);
			this.LabMuiltCode.AutoSize = true;
			this.LabMuiltCode.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabMuiltCode.Location = new System.Drawing.Point(19, 54);
			this.LabMuiltCode.Name = "LabMuiltCode";
			this.LabMuiltCode.Size = new System.Drawing.Size(196, 14);
			this.LabMuiltCode.TabIndex = 6;
			this.LabMuiltCode.Text = "多码识别(设备自动重启生效):";
			this.LabResolution.AutoSize = true;
			this.LabResolution.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabResolution.Location = new System.Drawing.Point(21, 30);
			this.LabResolution.Name = "LabResolution";
			this.LabResolution.Size = new System.Drawing.Size(182, 14);
			this.LabResolution.TabIndex = 5;
			this.LabResolution.Text = "分辨率(设备自动重启生效):";
			this.CobResolutionSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobResolutionSet.Font = new System.Drawing.Font("宋体", 10.5f);
			this.CobResolutionSet.FormattingEnabled = true;
			this.CobResolutionSet.Items.AddRange(new object[4] { "1280x800", "1280x320", "960x640", "752x480" });
			this.CobResolutionSet.Location = new System.Drawing.Point(219, 27);
			this.CobResolutionSet.Name = "CobResolutionSet";
			this.CobResolutionSet.Size = new System.Drawing.Size(112, 22);
			this.CobResolutionSet.TabIndex = 4;
			this.CobResolutionSet.SelectedIndexChanged += new System.EventHandler(CobResolutionSet_SelectedIndexChanged);
			this.PanResultOutputWay.Controls.Add(this.RdbQualityResultOverall);
			this.PanResultOutputWay.Controls.Add(this.RdbQualityResultOrderAll);
			this.PanResultOutputWay.Controls.Add(this.LabResultOutputWay);
			this.PanResultOutputWay.Controls.Add(this.RdbQualityOutputDescAndData);
			this.PanResultOutputWay.Font = new System.Drawing.Font("宋体", 10.5f);
			this.PanResultOutputWay.Location = new System.Drawing.Point(6, 117);
			this.PanResultOutputWay.Name = "PanResultOutputWay";
			this.PanResultOutputWay.Size = new System.Drawing.Size(430, 26);
			this.PanResultOutputWay.TabIndex = 20;
			this.RdbQualityResultOverall.AutoSize = true;
			this.RdbQualityResultOverall.Location = new System.Drawing.Point(101, 4);
			this.RdbQualityResultOverall.Name = "RdbQualityResultOverall";
			this.RdbQualityResultOverall.Size = new System.Drawing.Size(123, 18);
			this.RdbQualityResultOverall.TabIndex = 9;
			this.RdbQualityResultOverall.TabStop = true;
			this.RdbQualityResultOverall.Text = "只输出综合评价";
			this.RdbQualityResultOverall.UseVisualStyleBackColor = true;
			this.RdbQualityResultOverall.CheckedChanged += new System.EventHandler(RdbQualityResultOverall_CheckedChanged);
			this.RdbQualityResultOrderAll.AutoSize = true;
			this.RdbQualityResultOrderAll.Location = new System.Drawing.Point(225, 4);
			this.RdbQualityResultOrderAll.Name = "RdbQualityResultOrderAll";
			this.RdbQualityResultOrderAll.Size = new System.Drawing.Size(109, 18);
			this.RdbQualityResultOrderAll.TabIndex = 10;
			this.RdbQualityResultOrderAll.TabStop = true;
			this.RdbQualityResultOrderAll.Text = "顺序输出评价";
			this.RdbQualityResultOrderAll.UseVisualStyleBackColor = true;
			this.RdbQualityResultOrderAll.CheckedChanged += new System.EventHandler(RdbQualityResultOrderAll_CheckedChanged);
			this.LabResultOutputWay.AutoSize = true;
			this.LabResultOutputWay.Location = new System.Drawing.Point(4, 6);
			this.LabResultOutputWay.Name = "LabResultOutputWay";
			this.LabResultOutputWay.Size = new System.Drawing.Size(105, 14);
			this.LabResultOutputWay.TabIndex = 19;
			this.LabResultOutputWay.Text = "结果输出方式：";
			this.RdbQualityOutputDescAndData.AutoSize = true;
			this.RdbQualityOutputDescAndData.Location = new System.Drawing.Point(335, 4);
			this.RdbQualityOutputDescAndData.Name = "RdbQualityOutputDescAndData";
			this.RdbQualityOutputDescAndData.Size = new System.Drawing.Size(88, 18);
			this.RdbQualityOutputDescAndData.TabIndex = 11;
			this.RdbQualityOutputDescAndData.TabStop = true;
			this.RdbQualityOutputDescAndData.Text = "标记+数据";
			this.RdbQualityOutputDescAndData.UseVisualStyleBackColor = true;
			this.RdbQualityOutputDescAndData.Visible = false;
			this.RdbQualityOutputDescAndData.CheckedChanged += new System.EventHandler(RdbQualityOutputDescAndData_CheckedChanged);
			this.CobForceLowerLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobForceLowerLevel.Font = new System.Drawing.Font("宋体", 10.5f);
			this.CobForceLowerLevel.FormattingEnabled = true;
			this.CobForceLowerLevel.Items.AddRange(new object[5] { "不限制", "限制D级", "限制C级", "限制B级", "限制A级" });
			this.CobForceLowerLevel.Location = new System.Drawing.Point(114, 171);
			this.CobForceLowerLevel.Name = "CobForceLowerLevel";
			this.CobForceLowerLevel.Size = new System.Drawing.Size(137, 22);
			this.CobForceLowerLevel.TabIndex = 17;
			this.CobForceLowerLevel.SelectedIndexChanged += new System.EventHandler(CobForceLowerLevel_SelectedIndexChanged);
			this.TxbQualityAllSeparator.Font = new System.Drawing.Font("宋体", 10.5f);
			this.TxbQualityAllSeparator.Location = new System.Drawing.Point(346, 144);
			this.TxbQualityAllSeparator.MaxLength = 1;
			this.TxbQualityAllSeparator.Name = "TxbQualityAllSeparator";
			this.TxbQualityAllSeparator.Size = new System.Drawing.Size(31, 23);
			this.TxbQualityAllSeparator.TabIndex = 16;
			this.TxbQualityAllSeparator.TextChanged += new System.EventHandler(TxbQualityAllSeparator_TextChanged);
			this.LabQualityAllSeparator.AutoSize = true;
			this.LabQualityAllSeparator.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabQualityAllSeparator.Location = new System.Drawing.Point(219, 147);
			this.LabQualityAllSeparator.Name = "LabQualityAllSeparator";
			this.LabQualityAllSeparator.Size = new System.Drawing.Size(112, 14);
			this.LabQualityAllSeparator.TabIndex = 15;
			this.LabQualityAllSeparator.Text = "评价之间间隔符:";
			this.TxbQualityBarcodeSeparator.Font = new System.Drawing.Font("宋体", 10.5f);
			this.TxbQualityBarcodeSeparator.Location = new System.Drawing.Point(146, 144);
			this.TxbQualityBarcodeSeparator.MaxLength = 1;
			this.TxbQualityBarcodeSeparator.Name = "TxbQualityBarcodeSeparator";
			this.TxbQualityBarcodeSeparator.Size = new System.Drawing.Size(31, 23);
			this.TxbQualityBarcodeSeparator.TabIndex = 14;
			this.TxbQualityBarcodeSeparator.TextChanged += new System.EventHandler(TxbQualityBarcodeSeparator_TextChanged);
			this.LabQualityBarcodeSeparator.AutoSize = true;
			this.LabQualityBarcodeSeparator.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabQualityBarcodeSeparator.Location = new System.Drawing.Point(10, 147);
			this.LabQualityBarcodeSeparator.Name = "LabQualityBarcodeSeparator";
			this.LabQualityBarcodeSeparator.Size = new System.Drawing.Size(126, 14);
			this.LabQualityBarcodeSeparator.TabIndex = 12;
			this.LabQualityBarcodeSeparator.Text = "条码到评价间隔符:";
			this.LabBarcodeQualityDisp.AutoSize = true;
			this.LabBarcodeQualityDisp.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabBarcodeQualityDisp.Location = new System.Drawing.Point(15, 23);
			this.LabBarcodeQualityDisp.Name = "LabBarcodeQualityDisp";
			this.LabBarcodeQualityDisp.Size = new System.Drawing.Size(98, 14);
			this.LabBarcodeQualityDisp.TabIndex = 7;
			this.LabBarcodeQualityDisp.Text = "条码打印评价:";
			this.CobBarcodeQualityMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobBarcodeQualityMode.Font = new System.Drawing.Font("宋体", 10.5f);
			this.CobBarcodeQualityMode.FormattingEnabled = true;
			this.CobBarcodeQualityMode.Items.AddRange(new object[3] { "Disable", "ISO/ICE-15415", "ISO/ICE-29158(AIM-DPM-2006)" });
			this.CobBarcodeQualityMode.Location = new System.Drawing.Point(119, 20);
			this.CobBarcodeQualityMode.Name = "CobBarcodeQualityMode";
			this.CobBarcodeQualityMode.Size = new System.Drawing.Size(208, 22);
			this.CobBarcodeQualityMode.TabIndex = 6;
			this.CobBarcodeQualityMode.SelectedIndexChanged += new System.EventHandler(CobBarcodeQualityMode_SelectedIndexChanged);
			this.LabForceLowerLevel.AutoSize = true;
			this.LabForceLowerLevel.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabForceLowerLevel.Location = new System.Drawing.Point(10, 174);
			this.LabForceLowerLevel.Name = "LabForceLowerLevel";
			this.LabForceLowerLevel.Size = new System.Drawing.Size(98, 14);
			this.LabForceLowerLevel.TabIndex = 18;
			this.LabForceLowerLevel.Text = "最低等级限制:";
			this.PanOnlyForServer.Controls.Add(this.CobHostUploadMode);
			this.PanOnlyForServer.Controls.Add(this.LabHostUploadMode);
			this.PanOnlyForServer.Controls.Add(this.LabDecodeWaitTimeUnit);
			this.PanOnlyForServer.Controls.Add(this.LabDecodeWaitTime);
			this.PanOnlyForServer.Controls.Add(this.BtnDecodeWaitTimeSet);
			this.PanOnlyForServer.Controls.Add(this.TxbDecodeWaitTime);
			this.PanOnlyForServer.Controls.Add(this.BtnNetWorkingSplitCh);
			this.PanOnlyForServer.Controls.Add(this.TxbNetWorkingSplitCh);
			this.PanOnlyForServer.Controls.Add(this.ChkUseSameTrig);
			this.PanOnlyForServer.Controls.Add(this.ChkNetworkingSplitCh);
			this.PanOnlyForServer.Font = new System.Drawing.Font("宋体", 10.5f);
			this.PanOnlyForServer.Location = new System.Drawing.Point(13, 43);
			this.PanOnlyForServer.Name = "PanOnlyForServer";
			this.PanOnlyForServer.Size = new System.Drawing.Size(428, 93);
			this.PanOnlyForServer.TabIndex = 55;
			this.CobHostUploadMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobHostUploadMode.FormattingEnabled = true;
			this.CobHostUploadMode.Items.AddRange(new object[2] { "主机从机读完上传才结束", "任意设备读完上传就结束" });
			this.CobHostUploadMode.Location = new System.Drawing.Point(160, 68);
			this.CobHostUploadMode.Name = "CobHostUploadMode";
			this.CobHostUploadMode.Size = new System.Drawing.Size(265, 22);
			this.CobHostUploadMode.TabIndex = 54;
			this.CobHostUploadMode.SelectedIndexChanged += new System.EventHandler(CobHostUploadMode_SelectedIndexChanged);
			this.LabHostUploadMode.AutoSize = true;
			this.LabHostUploadMode.Location = new System.Drawing.Point(6, 72);
			this.LabHostUploadMode.Name = "LabHostUploadMode";
			this.LabHostUploadMode.Size = new System.Drawing.Size(154, 14);
			this.LabHostUploadMode.TabIndex = 53;
			this.LabHostUploadMode.Text = "主机读完成后等待时间:";
			this.LabDecodeWaitTimeUnit.AutoSize = true;
			this.LabDecodeWaitTimeUnit.Location = new System.Drawing.Point(235, 47);
			this.LabDecodeWaitTimeUnit.Name = "LabDecodeWaitTimeUnit";
			this.LabDecodeWaitTimeUnit.Size = new System.Drawing.Size(28, 14);
			this.LabDecodeWaitTimeUnit.TabIndex = 52;
			this.LabDecodeWaitTimeUnit.Text = "0ms";
			this.LabDecodeWaitTime.AutoSize = true;
			this.LabDecodeWaitTime.Location = new System.Drawing.Point(7, 47);
			this.LabDecodeWaitTime.Name = "LabDecodeWaitTime";
			this.LabDecodeWaitTime.Size = new System.Drawing.Size(154, 14);
			this.LabDecodeWaitTime.TabIndex = 51;
			this.LabDecodeWaitTime.Text = "主机读完成后等待时间:";
			this.BtnDecodeWaitTimeSet.Location = new System.Drawing.Point(270, 43);
			this.BtnDecodeWaitTimeSet.Name = "BtnDecodeWaitTimeSet";
			this.BtnDecodeWaitTimeSet.Size = new System.Drawing.Size(47, 23);
			this.BtnDecodeWaitTimeSet.TabIndex = 49;
			this.BtnDecodeWaitTimeSet.Text = "确认";
			this.BtnDecodeWaitTimeSet.UseVisualStyleBackColor = true;
			this.BtnDecodeWaitTimeSet.Visible = false;
			this.BtnDecodeWaitTimeSet.Click += new System.EventHandler(BtnDecodeWaitTimeSet_Click);
			this.TxbDecodeWaitTime.Location = new System.Drawing.Point(164, 43);
			this.TxbDecodeWaitTime.MaxLength = 5;
			this.TxbDecodeWaitTime.Name = "TxbDecodeWaitTime";
			this.TxbDecodeWaitTime.Size = new System.Drawing.Size(69, 23);
			this.TxbDecodeWaitTime.TabIndex = 50;
			this.TxbDecodeWaitTime.Text = "500";
			this.TxbDecodeWaitTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.TxbDecodeWaitTime.TextChanged += new System.EventHandler(TxbDecodeWaitTime_TextChanged);
			this.BtnNetWorkingSplitCh.Location = new System.Drawing.Point(150, 21);
			this.BtnNetWorkingSplitCh.Name = "BtnNetWorkingSplitCh";
			this.BtnNetWorkingSplitCh.Size = new System.Drawing.Size(47, 23);
			this.BtnNetWorkingSplitCh.TabIndex = 45;
			this.BtnNetWorkingSplitCh.Text = "确认";
			this.BtnNetWorkingSplitCh.UseVisualStyleBackColor = true;
			this.BtnNetWorkingSplitCh.Visible = false;
			this.BtnNetWorkingSplitCh.Click += new System.EventHandler(BtnNetWorkingSplitCh_Click);
			this.TxbNetWorkingSplitCh.Location = new System.Drawing.Point(114, 21);
			this.TxbNetWorkingSplitCh.MaxLength = 2;
			this.TxbNetWorkingSplitCh.Name = "TxbNetWorkingSplitCh";
			this.TxbNetWorkingSplitCh.Size = new System.Drawing.Size(35, 23);
			this.TxbNetWorkingSplitCh.TabIndex = 46;
			this.TxbNetWorkingSplitCh.TextChanged += new System.EventHandler(TxbNetWorkingSplitCh_TextChanged);
			this.ChkUseSameTrig.AutoSize = true;
			this.ChkUseSameTrig.Location = new System.Drawing.Point(9, 3);
			this.ChkUseSameTrig.Name = "ChkUseSameTrig";
			this.ChkUseSameTrig.Size = new System.Drawing.Size(152, 18);
			this.ChkUseSameTrig.TabIndex = 3;
			this.ChkUseSameTrig.Text = "同步触发全部读码器";
			this.ChkUseSameTrig.UseVisualStyleBackColor = true;
			this.ChkUseSameTrig.CheckedChanged += new System.EventHandler(ChkUseSameTrig_CheckedChanged);
			this.ChkNetworkingSplitCh.AutoSize = true;
			this.ChkNetworkingSplitCh.Location = new System.Drawing.Point(9, 24);
			this.ChkNetworkingSplitCh.Name = "ChkNetworkingSplitCh";
			this.ChkNetworkingSplitCh.Size = new System.Drawing.Size(103, 18);
			this.ChkNetworkingSplitCh.TabIndex = 47;
			this.ChkNetworkingSplitCh.Text = "分隔符:  0x";
			this.ChkNetworkingSplitCh.UseVisualStyleBackColor = true;
			this.ChkNetworkingSplitCh.CheckedChanged += new System.EventHandler(ChkNetworkingSplitCh_CheckedChanged);
			this.PanOnlyForClient.Controls.Add(this.BtnHostSIP_Set);
			this.PanOnlyForClient.Controls.Add(this.TxbHostSIP_Seg4);
			this.PanOnlyForClient.Controls.Add(this.TxbHostSIP_Seg3);
			this.PanOnlyForClient.Controls.Add(this.TxbHostSIP_Seg2);
			this.PanOnlyForClient.Controls.Add(this.TxbHostSIP_Seg1);
			this.PanOnlyForClient.Controls.Add(this.LabNetworkingHostIP);
			this.PanOnlyForClient.Font = new System.Drawing.Font("宋体", 10.5f);
			this.PanOnlyForClient.Location = new System.Drawing.Point(6, 138);
			this.PanOnlyForClient.Name = "PanOnlyForClient";
			this.PanOnlyForClient.Size = new System.Drawing.Size(435, 29);
			this.PanOnlyForClient.TabIndex = 48;
			this.BtnHostSIP_Set.Location = new System.Drawing.Point(286, 1);
			this.BtnHostSIP_Set.Name = "BtnHostSIP_Set";
			this.BtnHostSIP_Set.Size = new System.Drawing.Size(47, 23);
			this.BtnHostSIP_Set.TabIndex = 39;
			this.BtnHostSIP_Set.Text = "确认";
			this.BtnHostSIP_Set.UseVisualStyleBackColor = true;
			this.BtnHostSIP_Set.Visible = false;
			this.BtnHostSIP_Set.Click += new System.EventHandler(BtnHostSIP_Set_Click);
			this.TxbHostSIP_Seg4.Location = new System.Drawing.Point(252, 2);
			this.TxbHostSIP_Seg4.MaxLength = 3;
			this.TxbHostSIP_Seg4.Name = "TxbHostSIP_Seg4";
			this.TxbHostSIP_Seg4.Size = new System.Drawing.Size(32, 23);
			this.TxbHostSIP_Seg4.TabIndex = 12;
			this.TxbHostSIP_Seg4.Text = "100";
			this.TxbHostSIP_Seg4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbHostSIP_Seg4.TextChanged += new System.EventHandler(TxbHostSIP_Seg4_TextChanged);
			this.TxbHostSIP_Seg3.Location = new System.Drawing.Point(214, 2);
			this.TxbHostSIP_Seg3.MaxLength = 3;
			this.TxbHostSIP_Seg3.Name = "TxbHostSIP_Seg3";
			this.TxbHostSIP_Seg3.Size = new System.Drawing.Size(32, 23);
			this.TxbHostSIP_Seg3.TabIndex = 11;
			this.TxbHostSIP_Seg3.Text = "0";
			this.TxbHostSIP_Seg3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbHostSIP_Seg3.TextChanged += new System.EventHandler(TxbHostSIP_Seg3_TextChanged);
			this.TxbHostSIP_Seg2.Location = new System.Drawing.Point(176, 2);
			this.TxbHostSIP_Seg2.MaxLength = 3;
			this.TxbHostSIP_Seg2.Name = "TxbHostSIP_Seg2";
			this.TxbHostSIP_Seg2.Size = new System.Drawing.Size(32, 23);
			this.TxbHostSIP_Seg2.TabIndex = 10;
			this.TxbHostSIP_Seg2.Text = "168";
			this.TxbHostSIP_Seg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbHostSIP_Seg2.TextChanged += new System.EventHandler(TxbHostSIP_Seg2_TextChanged);
			this.TxbHostSIP_Seg1.Location = new System.Drawing.Point(138, 2);
			this.TxbHostSIP_Seg1.MaxLength = 3;
			this.TxbHostSIP_Seg1.Name = "TxbHostSIP_Seg1";
			this.TxbHostSIP_Seg1.Size = new System.Drawing.Size(32, 23);
			this.TxbHostSIP_Seg1.TabIndex = 9;
			this.TxbHostSIP_Seg1.Text = "192";
			this.TxbHostSIP_Seg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TxbHostSIP_Seg1.TextChanged += new System.EventHandler(TxbHostSIP_Seg1_TextChanged);
			this.LabNetworkingHostIP.AutoSize = true;
			this.LabNetworkingHostIP.Location = new System.Drawing.Point(13, 6);
			this.LabNetworkingHostIP.Name = "LabNetworkingHostIP";
			this.LabNetworkingHostIP.Size = new System.Drawing.Size(119, 14);
			this.LabNetworkingHostIP.TabIndex = 6;
			this.LabNetworkingHostIP.Text = "组网主机设备IP：";
			this.RdbClientMode.AutoSize = true;
			this.RdbClientMode.Font = new System.Drawing.Font("宋体", 10.5f);
			this.RdbClientMode.Location = new System.Drawing.Point(235, 22);
			this.RdbClientMode.Name = "RdbClientMode";
			this.RdbClientMode.Size = new System.Drawing.Size(81, 18);
			this.RdbClientMode.TabIndex = 2;
			this.RdbClientMode.Text = "从机模式";
			this.RdbClientMode.UseVisualStyleBackColor = true;
			this.RdbClientMode.CheckedChanged += new System.EventHandler(RdbClientMode_CheckedChanged);
			this.RdbNetWorkingServer.AutoSize = true;
			this.RdbNetWorkingServer.Font = new System.Drawing.Font("宋体", 10.5f);
			this.RdbNetWorkingServer.Location = new System.Drawing.Point(133, 22);
			this.RdbNetWorkingServer.Name = "RdbNetWorkingServer";
			this.RdbNetWorkingServer.Size = new System.Drawing.Size(81, 18);
			this.RdbNetWorkingServer.TabIndex = 1;
			this.RdbNetWorkingServer.Text = "主机模式";
			this.RdbNetWorkingServer.UseVisualStyleBackColor = true;
			this.RdbNetWorkingServer.CheckedChanged += new System.EventHandler(RdbNetWorkingServer_CheckedChanged);
			this.RdbNetWorkingClose.AutoSize = true;
			this.RdbNetWorkingClose.Checked = true;
			this.RdbNetWorkingClose.Font = new System.Drawing.Font("宋体", 10.5f);
			this.RdbNetWorkingClose.Location = new System.Drawing.Point(22, 22);
			this.RdbNetWorkingClose.Name = "RdbNetWorkingClose";
			this.RdbNetWorkingClose.Size = new System.Drawing.Size(109, 18);
			this.RdbNetWorkingClose.TabIndex = 0;
			this.RdbNetWorkingClose.TabStop = true;
			this.RdbNetWorkingClose.Text = "关闭组网功能";
			this.RdbNetWorkingClose.UseVisualStyleBackColor = true;
			this.RdbNetWorkingClose.CheckedChanged += new System.EventHandler(RdbNetWorkingClose_CheckedChanged);
			this.CobLogState.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.CobLogState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobLogState.Enabled = false;
			this.CobLogState.Font = new System.Drawing.Font("宋体", 10.5f);
			this.CobLogState.FormattingEnabled = true;
			this.CobLogState.Items.AddRange(new object[6] { "致命错误才记录", "出现错误才记录", "出现警告才记录", "统计解码记录", "一般信息记录", "调试信息记录" });
			this.CobLogState.Location = new System.Drawing.Point(80, 36);
			this.CobLogState.Name = "CobLogState";
			this.CobLogState.Size = new System.Drawing.Size(217, 22);
			this.CobLogState.TabIndex = 56;
			this.CobLogState.SelectedIndexChanged += new System.EventHandler(CobLogState_SelectedIndexChanged);
			this.LabLogState.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.LabLogState.AutoSize = true;
			this.LabLogState.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabLogState.Location = new System.Drawing.Point(19, 40);
			this.LabLogState.Name = "LabLogState";
			this.LabLogState.Size = new System.Drawing.Size(63, 14);
			this.LabLogState.TabIndex = 55;
			this.LabLogState.Text = "Log状态:";
			this.LabLogState.DoubleClick += new System.EventHandler(LabLogState_DoubleClick);
			this.LvwClientDisp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.LvwClientDisp.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LvwClientDisp.HideSelection = false;
			this.LvwClientDisp.Location = new System.Drawing.Point(24, 60);
			this.LvwClientDisp.Name = "LvwClientDisp";
			this.LvwClientDisp.Size = new System.Drawing.Size(407, 127);
			this.LvwClientDisp.TabIndex = 54;
			this.LvwClientDisp.UseCompatibleStateImageBehavior = false;
			this.LvwClientDisp.View = System.Windows.Forms.View.SmallIcon;
			this.LabClientCount.AutoSize = true;
			this.LabClientCount.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabClientCount.Location = new System.Drawing.Point(143, 19);
			this.LabClientCount.Name = "LabClientCount";
			this.LabClientCount.Size = new System.Drawing.Size(14, 14);
			this.LabClientCount.TabIndex = 53;
			this.LabClientCount.Text = "0";
			this.LabClientCountDisp.AutoSize = true;
			this.LabClientCountDisp.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabClientCountDisp.Location = new System.Drawing.Point(21, 19);
			this.LabClientCountDisp.Name = "LabClientCountDisp";
			this.LabClientCountDisp.Size = new System.Drawing.Size(126, 14);
			this.LabClientCountDisp.TabIndex = 52;
			this.LabClientCountDisp.Text = "已连接的设备个数:";
			this.GrbMuiltCoreAls.Controls.Add(this.CobMuiltCoreMode);
			this.GrbMuiltCoreAls.Controls.Add(this.LabMuiltCoreMode);
			this.GrbMuiltCoreAls.Location = new System.Drawing.Point(12, 618);
			this.GrbMuiltCoreAls.Name = "GrbMuiltCoreAls";
			this.GrbMuiltCoreAls.Size = new System.Drawing.Size(395, 42);
			this.GrbMuiltCoreAls.TabIndex = 16;
			this.GrbMuiltCoreAls.TabStop = false;
			this.GrbMuiltCoreAls.Text = "算法加速配置";
			this.GrbMuiltCoreAls.Visible = false;
			this.CobMuiltCoreMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobMuiltCoreMode.FormattingEnabled = true;
			this.CobMuiltCoreMode.Items.AddRange(new object[4] { "关闭加速", "运动流水线狂扫模式", "静态图像快读模式", "解码疯狂探索模式" });
			this.CobMuiltCoreMode.Location = new System.Drawing.Point(182, 17);
			this.CobMuiltCoreMode.Name = "CobMuiltCoreMode";
			this.CobMuiltCoreMode.Size = new System.Drawing.Size(181, 22);
			this.CobMuiltCoreMode.TabIndex = 39;
			this.CobMuiltCoreMode.SelectedIndexChanged += new System.EventHandler(CobMuiltCoreMode_SelectedIndexChanged);
			this.LabMuiltCoreMode.AutoSize = true;
			this.LabMuiltCoreMode.Location = new System.Drawing.Point(33, 20);
			this.LabMuiltCoreMode.Name = "LabMuiltCoreMode";
			this.LabMuiltCoreMode.Size = new System.Drawing.Size(147, 14);
			this.LabMuiltCoreMode.TabIndex = 38;
			this.LabMuiltCoreMode.Text = "多核CPU处理加速模式:";
			this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.tsmiDownload, this.tsmiUpload, this.tsmiDelete, this.tsmiReflesh, this.RenameToolStripMenuItem, this.MkdirToolStripMenuItem });
			this.cmsMain.Name = "contextMenuStrip1";
			this.cmsMain.Size = new System.Drawing.Size(137, 136);
			this.cmsMain.Opening += new System.ComponentModel.CancelEventHandler(cmsMain_Opening);
			this.tsmiDownload.Name = "tsmiDownload";
			this.tsmiDownload.Size = new System.Drawing.Size(136, 22);
			this.tsmiDownload.Text = "下载";
			this.tsmiDownload.Click += new System.EventHandler(tsmiDownload_Click);
			this.tsmiUpload.Name = "tsmiUpload";
			this.tsmiUpload.Size = new System.Drawing.Size(136, 22);
			this.tsmiUpload.Text = "上传";
			this.tsmiUpload.Click += new System.EventHandler(tsmiUpload_Click);
			this.tsmiDelete.Name = "tsmiDelete";
			this.tsmiDelete.Size = new System.Drawing.Size(136, 22);
			this.tsmiDelete.Text = "删除";
			this.tsmiDelete.Click += new System.EventHandler(tsmiDelete_Click);
			this.tsmiReflesh.Name = "tsmiReflesh";
			this.tsmiReflesh.Size = new System.Drawing.Size(136, 22);
			this.tsmiReflesh.Text = "刷新";
			this.tsmiReflesh.Click += new System.EventHandler(tsmiReflesh_Click);
			this.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem";
			this.RenameToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.RenameToolStripMenuItem.Text = "重命名";
			this.RenameToolStripMenuItem.Click += new System.EventHandler(RenameToolStripMenuItem_Click);
			this.MkdirToolStripMenuItem.Name = "MkdirToolStripMenuItem";
			this.MkdirToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.MkdirToolStripMenuItem.Text = "新建文件夹";
			this.MkdirToolStripMenuItem.Click += new System.EventHandler(MkdirToolStripMenuItem_Click);
			this.GrpCodeQuality.Controls.Add(this.CobCauseNG_TH_Set);
			this.GrpCodeQuality.Controls.Add(this.LabCauseNG_TH_Set);
			this.GrpCodeQuality.Controls.Add(this.CobCauseNGOut);
			this.GrpCodeQuality.Controls.Add(this.LabCauseNGOut);
			this.GrpCodeQuality.Controls.Add(this.PanChkSwitch);
			this.GrpCodeQuality.Controls.Add(this.LabBarcodeQualityDisp);
			this.GrpCodeQuality.Controls.Add(this.CobBarcodeQualityMode);
			this.GrpCodeQuality.Controls.Add(this.CobForceLowerLevel);
			this.GrpCodeQuality.Controls.Add(this.LabForceLowerLevel);
			this.GrpCodeQuality.Controls.Add(this.TxbQualityAllSeparator);
			this.GrpCodeQuality.Controls.Add(this.LabQualityAllSeparator);
			this.GrpCodeQuality.Controls.Add(this.LabQualityBarcodeSeparator);
			this.GrpCodeQuality.Controls.Add(this.TxbQualityBarcodeSeparator);
			this.GrpCodeQuality.Controls.Add(this.PanResultOutputWay);
			this.GrpCodeQuality.Font = new System.Drawing.Font("宋体", 10.5f);
			this.GrpCodeQuality.Location = new System.Drawing.Point(6, 6);
			this.GrpCodeQuality.Name = "GrpCodeQuality";
			this.GrpCodeQuality.Size = new System.Drawing.Size(434, 246);
			this.GrpCodeQuality.TabIndex = 17;
			this.GrpCodeQuality.TabStop = false;
			this.GrpCodeQuality.Text = "条码打印质量检测及结果输出";
			this.CobCauseNG_TH_Set.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobCauseNG_TH_Set.Font = new System.Drawing.Font("宋体", 10.5f);
			this.CobCauseNG_TH_Set.FormattingEnabled = true;
			this.CobCauseNG_TH_Set.Items.AddRange(new object[5] { "默认不设置", "A级", "B级", "C级", "D级" });
			this.CobCauseNG_TH_Set.Location = new System.Drawing.Point(157, 219);
			this.CobCauseNG_TH_Set.Name = "CobCauseNG_TH_Set";
			this.CobCauseNG_TH_Set.Size = new System.Drawing.Size(137, 22);
			this.CobCauseNG_TH_Set.TabIndex = 24;
			this.CobCauseNG_TH_Set.SelectedIndexChanged += new System.EventHandler(CobCauseNG_TH_Set_SelectedIndexChanged);
			this.LabCauseNG_TH_Set.AutoSize = true;
			this.LabCauseNG_TH_Set.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabCauseNG_TH_Set.Location = new System.Drawing.Point(10, 222);
			this.LabCauseNG_TH_Set.Name = "LabCauseNG_TH_Set";
			this.LabCauseNG_TH_Set.Size = new System.Drawing.Size(140, 14);
			this.LabCauseNG_TH_Set.TabIndex = 25;
			this.LabCauseNG_TH_Set.Text = "判断达标的等级设置:";
			this.CobCauseNGOut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobCauseNGOut.Font = new System.Drawing.Font("宋体", 10.5f);
			this.CobCauseNGOut.FormattingEnabled = true;
			this.CobCauseNGOut.Items.AddRange(new object[3] { "默认等级达不到也不输出信号", "等级达不到输出OUT1信号", "等级达不到输出OUT2信号" });
			this.CobCauseNGOut.Location = new System.Drawing.Point(190, 195);
			this.CobCauseNGOut.Name = "CobCauseNGOut";
			this.CobCauseNGOut.Size = new System.Drawing.Size(219, 22);
			this.CobCauseNGOut.TabIndex = 22;
			this.CobCauseNGOut.SelectedIndexChanged += new System.EventHandler(CobCauseNGOut_SelectedIndexChanged);
			this.LabCauseNGOut.AutoSize = true;
			this.LabCauseNGOut.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabCauseNGOut.Location = new System.Drawing.Point(10, 198);
			this.LabCauseNGOut.Name = "LabCauseNGOut";
			this.LabCauseNGOut.Size = new System.Drawing.Size(168, 14);
			this.LabCauseNGOut.TabIndex = 23;
			this.LabCauseNGOut.Text = "条码等级不满足信号输出:";
			this.PanChkSwitch.Controls.Add(this.ChkSwitchVID);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchDec);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchFID);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchSC);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchPG);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchMD);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchUEC);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchFPD);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchGN);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchAN);
			this.PanChkSwitch.Controls.Add(this.ChkSwitchRM);
			this.PanChkSwitch.Location = new System.Drawing.Point(9, 45);
			this.PanChkSwitch.Name = "PanChkSwitch";
			this.PanChkSwitch.Size = new System.Drawing.Size(420, 70);
			this.PanChkSwitch.TabIndex = 21;
			this.ChkSwitchVID.AutoSize = true;
			this.ChkSwitchVID.Location = new System.Drawing.Point(280, 79);
			this.ChkSwitchVID.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchVID.Name = "ChkSwitchVID";
			this.ChkSwitchVID.Size = new System.Drawing.Size(110, 18);
			this.ChkSwitchVID.TabIndex = 31;
			this.ChkSwitchVID.Text = "版本信息损坏";
			this.ChkSwitchVID.UseVisualStyleBackColor = true;
			this.ChkSwitchVID.Visible = false;
			this.ChkSwitchVID.Click += new System.EventHandler(ChkSwitchVID_Click);
			this.ChkSwitchDec.AutoSize = true;
			this.ChkSwitchDec.Location = new System.Drawing.Point(17, 5);
			this.ChkSwitchDec.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchDec.Name = "ChkSwitchDec";
			this.ChkSwitchDec.Size = new System.Drawing.Size(82, 18);
			this.ChkSwitchDec.TabIndex = 24;
			this.ChkSwitchDec.Text = "可解码性";
			this.ChkSwitchDec.UseVisualStyleBackColor = true;
			this.ChkSwitchDec.Click += new System.EventHandler(ChkSwitchDec_Click);
			this.ChkSwitchFID.AutoSize = true;
			this.ChkSwitchFID.Location = new System.Drawing.Point(202, 79);
			this.ChkSwitchFID.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchFID.Name = "ChkSwitchFID";
			this.ChkSwitchFID.Size = new System.Drawing.Size(110, 18);
			this.ChkSwitchFID.TabIndex = 30;
			this.ChkSwitchFID.Text = "格式信息损坏";
			this.ChkSwitchFID.UseVisualStyleBackColor = true;
			this.ChkSwitchFID.Visible = false;
			this.ChkSwitchFID.Click += new System.EventHandler(ChkSwitchFID_Click);
			this.ChkSwitchSC.AutoSize = true;
			this.ChkSwitchSC.Location = new System.Drawing.Point(146, 5);
			this.ChkSwitchSC.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchSC.Name = "ChkSwitchSC";
			this.ChkSwitchSC.Size = new System.Drawing.Size(96, 18);
			this.ChkSwitchSC.TabIndex = 21;
			this.ChkSwitchSC.Text = "符号对比度";
			this.ChkSwitchSC.UseVisualStyleBackColor = true;
			this.ChkSwitchSC.Click += new System.EventHandler(ChkSwitchSC_Click);
			this.ChkSwitchPG.AutoSize = true;
			this.ChkSwitchPG.Location = new System.Drawing.Point(286, 49);
			this.ChkSwitchPG.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchPG.Name = "ChkSwitchPG";
			this.ChkSwitchPG.Size = new System.Drawing.Size(82, 18);
			this.ChkSwitchPG.TabIndex = 29;
			this.ChkSwitchPG.Text = "打印增长";
			this.ChkSwitchPG.UseVisualStyleBackColor = true;
			this.ChkSwitchPG.Click += new System.EventHandler(ChkSwitchPG_Click);
			this.ChkSwitchMD.AutoSize = true;
			this.ChkSwitchMD.Location = new System.Drawing.Point(286, 5);
			this.ChkSwitchMD.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchMD.Name = "ChkSwitchMD";
			this.ChkSwitchMD.Size = new System.Drawing.Size(54, 18);
			this.ChkSwitchMD.TabIndex = 22;
			this.ChkSwitchMD.Text = "调制";
			this.ChkSwitchMD.UseVisualStyleBackColor = true;
			this.ChkSwitchMD.Click += new System.EventHandler(ChkSwitchMD_Click);
			this.ChkSwitchUEC.AutoSize = true;
			this.ChkSwitchUEC.Location = new System.Drawing.Point(146, 49);
			this.ChkSwitchUEC.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchUEC.Name = "ChkSwitchUEC";
			this.ChkSwitchUEC.Size = new System.Drawing.Size(96, 18);
			this.ChkSwitchUEC.TabIndex = 27;
			this.ChkSwitchUEC.Text = "未使用纠错";
			this.ChkSwitchUEC.UseVisualStyleBackColor = true;
			this.ChkSwitchUEC.Click += new System.EventHandler(ChkSwitchUEC_Click);
			this.ChkSwitchFPD.AutoSize = true;
			this.ChkSwitchFPD.Location = new System.Drawing.Point(146, 27);
			this.ChkSwitchFPD.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchFPD.Name = "ChkSwitchFPD";
			this.ChkSwitchFPD.Size = new System.Drawing.Size(110, 18);
			this.ChkSwitchFPD.TabIndex = 23;
			this.ChkSwitchFPD.Text = "固定图形损坏";
			this.ChkSwitchFPD.UseVisualStyleBackColor = true;
			this.ChkSwitchFPD.Click += new System.EventHandler(ChkSwitchFPD_Click);
			this.ChkSwitchGN.AutoSize = true;
			this.ChkSwitchGN.Location = new System.Drawing.Point(17, 49);
			this.ChkSwitchGN.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchGN.Name = "ChkSwitchGN";
			this.ChkSwitchGN.Size = new System.Drawing.Size(110, 18);
			this.ChkSwitchGN.TabIndex = 26;
			this.ChkSwitchGN.Text = "网格非均匀性";
			this.ChkSwitchGN.UseVisualStyleBackColor = true;
			this.ChkSwitchGN.Click += new System.EventHandler(ChkSwitchGN_Click);
			this.ChkSwitchAN.AutoSize = true;
			this.ChkSwitchAN.Location = new System.Drawing.Point(286, 27);
			this.ChkSwitchAN.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchAN.Name = "ChkSwitchAN";
			this.ChkSwitchAN.Size = new System.Drawing.Size(110, 18);
			this.ChkSwitchAN.TabIndex = 25;
			this.ChkSwitchAN.Text = "轴向非均匀性";
			this.ChkSwitchAN.UseVisualStyleBackColor = true;
			this.ChkSwitchAN.Click += new System.EventHandler(ChkSwitchAN_Click);
			this.ChkSwitchRM.AutoSize = true;
			this.ChkSwitchRM.Location = new System.Drawing.Point(17, 27);
			this.ChkSwitchRM.Margin = new System.Windows.Forms.Padding(0);
			this.ChkSwitchRM.Name = "ChkSwitchRM";
			this.ChkSwitchRM.Size = new System.Drawing.Size(96, 18);
			this.ChkSwitchRM.TabIndex = 28;
			this.ChkSwitchRM.Text = "反射率余量";
			this.ChkSwitchRM.UseVisualStyleBackColor = true;
			this.ChkSwitchRM.Click += new System.EventHandler(ChkSwitchRM_Click);
			this.GrpGroupNet.Controls.Add(this.PanOnlyForServer);
			this.GrpGroupNet.Controls.Add(this.RdbNetWorkingClose);
			this.GrpGroupNet.Controls.Add(this.PanOnlyForClient);
			this.GrpGroupNet.Controls.Add(this.RdbNetWorkingServer);
			this.GrpGroupNet.Controls.Add(this.RdbClientMode);
			this.GrpGroupNet.Font = new System.Drawing.Font("宋体", 10.5f);
			this.GrpGroupNet.Location = new System.Drawing.Point(12, 396);
			this.GrpGroupNet.Name = "GrpGroupNet";
			this.GrpGroupNet.Size = new System.Drawing.Size(447, 175);
			this.GrpGroupNet.TabIndex = 18;
			this.GrpGroupNet.TabStop = false;
			this.GrpGroupNet.Text = "组网模式配置";
			this.GrpState.Controls.Add(this.CobLogState);
			this.GrpState.Controls.Add(this.LabLogState);
			this.GrpState.Controls.Add(this.LabClientCount);
			this.GrpState.Controls.Add(this.LvwClientDisp);
			this.GrpState.Controls.Add(this.LabClientCountDisp);
			this.GrpState.Font = new System.Drawing.Font("宋体", 10.5f);
			this.GrpState.Location = new System.Drawing.Point(6, 6);
			this.GrpState.Name = "GrpState";
			this.GrpState.Size = new System.Drawing.Size(434, 193);
			this.GrpState.TabIndex = 19;
			this.GrpState.TabStop = false;
			this.GrpState.Text = "状态";
			this.TabMultSettings.Controls.Add(this.TpIso);
			this.TabMultSettings.Controls.Add(this.TpTheOthers);
			this.TabMultSettings.Location = new System.Drawing.Point(12, 104);
			this.TabMultSettings.Name = "TabMultSettings";
			this.TabMultSettings.SelectedIndex = 0;
			this.TabMultSettings.Size = new System.Drawing.Size(451, 286);
			this.TabMultSettings.TabIndex = 20;
			this.TpIso.Controls.Add(this.GrpCodeQuality);
			this.TpIso.Location = new System.Drawing.Point(4, 24);
			this.TpIso.Name = "TpIso";
			this.TpIso.Padding = new System.Windows.Forms.Padding(3);
			this.TpIso.Size = new System.Drawing.Size(443, 258);
			this.TpIso.TabIndex = 1;
			this.TpIso.Text = "条码等级";
			this.TpIso.UseVisualStyleBackColor = true;
			this.TpTheOthers.Controls.Add(this.CobTryMultCode);
			this.TpTheOthers.Controls.Add(this.LabTryMultCode);
			this.TpTheOthers.Controls.Add(this.GrpState);
			this.TpTheOthers.Location = new System.Drawing.Point(4, 24);
			this.TpTheOthers.Name = "TpTheOthers";
			this.TpTheOthers.Padding = new System.Windows.Forms.Padding(3);
			this.TpTheOthers.Size = new System.Drawing.Size(443, 258);
			this.TpTheOthers.TabIndex = 2;
			this.TpTheOthers.Text = "其他";
			this.TpTheOthers.UseVisualStyleBackColor = true;
			this.CobTryMultCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobTryMultCode.Font = new System.Drawing.Font("宋体", 10.5f);
			this.CobTryMultCode.FormattingEnabled = true;
			this.CobTryMultCode.Items.AddRange(new object[2] { "关闭", "开启" });
			this.CobTryMultCode.Location = new System.Drawing.Point(131, 209);
			this.CobTryMultCode.Name = "CobTryMultCode";
			this.CobTryMultCode.Size = new System.Drawing.Size(87, 22);
			this.CobTryMultCode.TabIndex = 20;
			this.CobTryMultCode.SelectedIndexChanged += new System.EventHandler(CobTryMultCode_SelectedIndexChanged);
			this.LabTryMultCode.AutoSize = true;
			this.LabTryMultCode.Font = new System.Drawing.Font("宋体", 10.5f);
			this.LabTryMultCode.Location = new System.Drawing.Point(27, 212);
			this.LabTryMultCode.Name = "LabTryMultCode";
			this.LabTryMultCode.Size = new System.Drawing.Size(98, 14);
			this.LabTryMultCode.TabIndex = 21;
			this.LabTryMultCode.Text = "多码尝试模式:";
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(927, 689);
			base.Controls.Add(this.GrbDeviceADSetting);
			base.Controls.Add(this.TabMultSettings);
			base.Controls.Add(this.GrpGroupNet);
			base.Controls.Add(this.GrbMuiltCoreAls);
			base.Controls.Add(this.GrbFtpSettomg);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "AdvancedSettingForm";
			this.Text = "高级功能";
			base.Load += new System.EventHandler(AdvancedSettingForm_Load);
			this.GrbFtpSettomg.ResumeLayout(false);
			this.GrbFtpSettomg.PerformLayout();
			this.GrbDeviceADSetting.ResumeLayout(false);
			this.GrbDeviceADSetting.PerformLayout();
			this.PanResultOutputWay.ResumeLayout(false);
			this.PanResultOutputWay.PerformLayout();
			this.PanOnlyForServer.ResumeLayout(false);
			this.PanOnlyForServer.PerformLayout();
			this.PanOnlyForClient.ResumeLayout(false);
			this.PanOnlyForClient.PerformLayout();
			this.GrbMuiltCoreAls.ResumeLayout(false);
			this.GrbMuiltCoreAls.PerformLayout();
			this.cmsMain.ResumeLayout(false);
			this.GrpCodeQuality.ResumeLayout(false);
			this.GrpCodeQuality.PerformLayout();
			this.PanChkSwitch.ResumeLayout(false);
			this.PanChkSwitch.PerformLayout();
			this.GrpGroupNet.ResumeLayout(false);
			this.GrpGroupNet.PerformLayout();
			this.GrpState.ResumeLayout(false);
			this.GrpState.PerformLayout();
			this.TabMultSettings.ResumeLayout(false);
			this.TpIso.ResumeLayout(false);
			this.TpTheOthers.ResumeLayout(false);
			this.TpTheOthers.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
