using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using Heroje_Debug_Tool.Properties;
using Heroje_Debug_Tool.SubForm;
using Heroje_Debug_Tool.SubForm.SmallForm;
using Heroje_Debug_Tool.SubTool;
using IniConfigFile_n;
using 合杰图像算法调试工具;
using 图像算法调试工具;

namespace Heroje_Debug_Tool
{
	public class MainForm : Form
	{
		private SetCfgCB SetCfgCBFunc;

		private GetCfgCB GetCfgCBFunc;

		private SendCfgDataCB SendCfgDataCBFunc;

		private DeviceConnectForm DeviceConnectPage;

		private ReadingForm ReadingPage;

		private DataEditForm DataEditPage;

		private IOInstructionForm IOInstructionPage;

		private AlgorithmProtocolForm AlgorithmProtocolPage;

		private CommunicateSettingForm CommunicateSettingPage;

		private AdvancedSettingForm AdvancedSettingPage;

		private ImageSaveSettingForm ImageSaveSettingPage;

		private OthersSettingForm OthersSettingPage;

		private string[] PageName_C = new string[8] { "读取", "通讯协议", "数据编辑", "IO指令", "算法协议", "图像保存", "其他", "高级功能" };

		private string[] PageName_T = new string[8] { "讀取", "通訊協定", "資料編輯", "IO指令", "演算法協定", "圖像保存", "其他", "高級功能" };

		private string[] PageName_E = new string[8] { "Read", "Communicate", "DataEdit", "IO CMD", "Algorithm", "ImgSave", "Others", "Advance" };

		private List<TabPage> PageList = new List<TabPage>();

		private DeviceCapacity.DeviceCapacityInfoStu DevCapacityArr;

		private string DeviceNameTemp;

		private bool NeedWaiteStart = true;

		private System.Timers.Timer FormClosedTimer;

		private ComDebugTool ComDebugToolForm = new ComDebugTool();

		private FormNet FormNetTools = new FormNet();

		private About AboutForm;

		private SettingForm FormSetting;

		private IContainer components = null;

		private MenuStrip MenuTopMain;

		private ToolStripMenuItem TsmTop_File;

		private ToolStripMenuItem TsmTop_Disp;

		private ToolStripMenuItem TsmTop_Sys;

		private ToolStripMenuItem TsmTop_Tool;

		private ToolStripMenuItem TsmTop_Help;

		private ToolStrip ToolStrip_Icon;

		private ToolStripSeparator TSS_Separator1;

		private ToolStripButton TSB_SearchDevice;

		private ToolStripButton TSB_ConnectDevice;

		private ToolStripButton TSB_DisconnectDevice;

		private ToolStripSeparator TSS_Separator2;

		private SplitContainer MainContainer;

		private TabControl TabSetting;

		private ToolStripMenuItem InstallUSBDrvToolStripMenuItem;

		private Panel PanDecMode;

		private RadioButton RdbDecodeModeC;

		private RadioButton RdbDecodeModeB;

		private RadioButton RdbDecodeModeA;

		private ToolStripMenuItem TsmTop_Language;

		private ToolStripMenuItem TSM_AboutSoftware;

		private ToolStripMenuItem TSM_SoftwareCloseDo;

		private CheckBox ChbUseTrainPara;

		private Button BtnDecodeCTraining;

		private ToolStripButton TSB_OpenCfg;

		private ToolStripButton TSB_SaveCfg;

		private ToolStripMenuItem Tsm_ComDebugTool;

		private ToolStripMenuItem Tsm_NetDebugTool;

		private ToolStripMenuItem Tsm_CmdConfigurationTool;

		private ToolStripMenuItem TSI_InstallGuide;

		private ToolStripMenuItem Tsm_ShowNetGrid;

		private ToolStripMenuItem TSM_UpgradeFirmware;

		private ToolStripMenuItem TSI_OpenCfg;

		private ToolStripMenuItem TSI_SaveCfg;

		private ToolStripMenuItem TSI_SaveCurrentImage;

		private ToolStripMenuItem TSM_FirmwareVersion;

		private Button BtnSaveCurrentCfg;

		private Button BtnRestartDevice;

		private ToolStripMenuItem TsmTop_Setting;

		private ToolStripMenuItem Tsm_Regular_Barcode;

		private ToolStripMenuItem Tsm_Regular_Output;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Heroje_Standard)
			{
				Text = "合杰读码调试工具" + ToolCfg.VersionInfo;
				base.Icon = Resources.logoIcon;
				StartForm startForm = new StartForm();
				startForm.Show();
				DateTime now = DateTime.Now;
				AddSubPages();
				SetPagesCallBackFunction();
				Pages_Init();
				while (!(DateTime.Now.Subtract(now).TotalMilliseconds > 1500.0))
				{
					Thread.Sleep(100);
					Application.DoEvents();
				}
				startForm.Close();
			}
			else if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Hide_Heroje_Logo)
			{
				Text = "读码调试工具" + ToolCfg.VersionInfo;
				AddSubPages();
				SetPagesCallBackFunction();
				Pages_Init();
			}
			SetUiSplitDistance();
			base.WindowState = FormWindowState.Maximized;
			SetUiSplitDistance();
		}

		private void AddSubPages()
		{
			DeviceConnectPage = new DeviceConnectForm();
			DeviceConnectPage.TopLevel = false;
			DeviceConnectPage.FormBorderStyle = FormBorderStyle.None;
			DeviceConnectPage.Height = MainContainer.Panel1.Height;
			DeviceConnectPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			DeviceConnectPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			MainContainer.Panel1.Controls.Add(DeviceConnectPage);
			DeviceConnectPage.Show();
			ReadingPage = new ReadingForm();
			ReadingPage.TopLevel = false;
			ReadingPage.FormBorderStyle = FormBorderStyle.None;
			ReadingPage.Dock = DockStyle.Fill;
			AddPage(ReadingPage, "读取");
			ReadingPage.DeviceSettingPage.Owner = this;
			CommunicateSettingPage = new CommunicateSettingForm();
			CommunicateSettingPage.TopLevel = false;
			CommunicateSettingPage.FormBorderStyle = FormBorderStyle.None;
			AddPage(CommunicateSettingPage, "通讯协议");
			DataEditPage = new DataEditForm();
			DataEditPage.TopLevel = false;
			DataEditPage.FormBorderStyle = FormBorderStyle.None;
			AddPage(DataEditPage, "数据编辑");
			IOInstructionPage = new IOInstructionForm();
			IOInstructionPage.TopLevel = false;
			IOInstructionPage.FormBorderStyle = FormBorderStyle.None;
			AddPage(IOInstructionPage, "IO指令");
			AlgorithmProtocolPage = new AlgorithmProtocolForm();
			AlgorithmProtocolPage.TopLevel = false;
			AlgorithmProtocolPage.FormBorderStyle = FormBorderStyle.None;
			AddPage(AlgorithmProtocolPage, "算法协议");
			ImageSaveSettingPage = new ImageSaveSettingForm();
			ImageSaveSettingPage.TopLevel = false;
			ImageSaveSettingPage.FormBorderStyle = FormBorderStyle.None;
			AddPage(ImageSaveSettingPage, "图像保存");
			OthersSettingPage = new OthersSettingForm();
			OthersSettingPage.TopLevel = false;
			OthersSettingPage.FormBorderStyle = FormBorderStyle.None;
			AddPage(OthersSettingPage, "其他");
			AdvancedSettingPage = new AdvancedSettingForm();
			AdvancedSettingPage.TopLevel = false;
			AdvancedSettingPage.FormBorderStyle = FormBorderStyle.None;
			AddPage(AdvancedSettingPage, "高级功能");
		}

		private void SetPagesCallBackFunction()
		{
			SetCfgCBFunc = DeviceConnectPage.SetPara;
			GetCfgCBFunc = DeviceConnectPage.GetPara;
			SendCfgDataCBFunc = DeviceConnectPage.DeviceConfigDataSend;
			DeviceConnectPage.SetCBFunc(UpdateParaAndDisplayCBFunc, ReadingPage.ReadingPage_Update, AdvancedSettingPage.AdvancedPage_Update);
			DeviceConnectPage.DevStateCallback = DevStateCB_Func;
			ReadingPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc);
			DataEditPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc);
			IOInstructionPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc);
			AlgorithmProtocolPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc, SetDecModeValueFuncCB);
			CommunicateSettingPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc);
			ReadingPage.DeviceSettingPage.TemplateSettingPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, Temp_Setting_Page_SendDataCB, Template_Get_Show, TemplatePage_Draw_ROI_CB);
			AdvancedSettingPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc);
			ImageSaveSettingPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc);
			OthersSettingPage.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc);
			ToolCfg.DeviceIPSettingPage = new FormDeviceIP(GetCfgCBFunc, DeviceConnectPage.ChangeIpByBrocast);
		}

		private bool Temp_Setting_Page_SendDataCB(uint action)
		{
			bool result = SendCfgDataCBFunc(action);
			if (action == 32768)
			{
				ReadingPage.UpdateTemplateRoiList();
			}
			return result;
		}

		private void AddPage(Control page, string pageName)
		{
			TabPage tabPage = new TabPage(pageName);
			tabPage.BorderStyle = BorderStyle.FixedSingle;
			tabPage.BackColor = Color.White;
			tabPage.Controls.Add(page);
			TabSetting.TabPages.Add(tabPage);
			page.Show();
			PageList.Add(tabPage);
		}

		private void UpdateParaAndDisplayCBFunc(UiParaInfoStu para)
		{
			IOInstructionPage.ShowTriggerStr();
			if (para.UpdateAdjState)
			{
				ReadingPage.UpdateUiDisplay(para);
				DataEditPage.UpdateUiDisplay(para);
				IOInstructionPage.UpdateUiDisplay(para);
				AlgorithmProtocolPage.UpdateUiDisplay(para);
				CommunicateSettingPage.UpdateUiDisplay(para);
				ReadingPage.DeviceSettingPage.TemplateSettingPage.UpdateUiDisplay(para);
				AdvancedSettingPage.UpdateUiDisplay(para);
				ImageSaveSettingPage.UpdateUiDisplay(para);
				if (para.ParaDataLen >= 4096)
				{
					RdbDecodeModeA.Visible = true;
					RdbDecodeModeB.Visible = true;
					RdbDecodeModeC.Visible = true;
				}
				else
				{
					RdbDecodeModeA.Visible = false;
					RdbDecodeModeB.Visible = false;
					RdbDecodeModeC.Visible = false;
				}
				DeviceNameTemp = para.DeviceName;
				DevCapacityArr = DeviceCapacity.GetCapacityInfo((byte)para.DeviceTypeRecord, para.ConnectType, DeviceNameTemp);
				TSM_UpgradeFirmware.Enabled = DevCapacityArr.IsNetworkDevice;
				ReadingPage.FunctionOnOff(DevCapacityArr.CapacityArr, DeviceNameTemp);
				CommunicateSettingPage.FunctionOnOff(DevCapacityArr.CapacityArr);
				ImageSaveSettingPage.FunctionOnOff(DevCapacityArr.CapacityArr);
				OthersSettingPage.Enabled = DevCapacityArr.CapacityArr[21];
				AdvancedSettingPage.FunctionOnOff(DevCapacityArr.CapacityArr);
			}
		}

		private void Pages_Init()
		{
			AddLanguageMenuItem();
			UpdateLanguageUI();
			DeviceConnectPage.Page_Init();
			ReadingPage.Page_Init();
		}

		private void AddLanguageMenuItem()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath);
			FileInfo[] files = directoryInfo.GetFiles("*.xml");
			List<FileInfo> list = new List<FileInfo>();
			list.AddRange(files);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Name.Contains("dpmcmd"))
				{
					list.Remove(list[i]);
					i--;
				}
			}
			FileInfo[] array = list.ToArray();
			FileInfo[] array2 = array;
			foreach (FileInfo fileInfo in array2)
			{
				if (fileInfo.Name.Contains("ChineseS"))
				{
					AddContextMenu("简体中文", TsmTop_Language.DropDownItems, MenuClicked);
				}
				else if (fileInfo.Name.Contains("ChineseT"))
				{
					AddContextMenu("繁體中文", TsmTop_Language.DropDownItems, MenuClicked);
				}
				else
				{
					AddContextMenu(fileInfo.Name.Substring(0, fileInfo.Name.Length - 4), TsmTop_Language.DropDownItems, MenuClicked);
				}
			}
			if (array.Length == 0)
			{
				FileStream fileStream = File.Create(ToolCfg.ConfigPath);
				fileStream.Flush();
				fileStream.Close();
				ControlAndXML controlAndXML = new ControlAndXML();
				controlAndXML.MainControlToXmlByLinq(new MainForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new DeviceConnectForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new ReadingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new LightingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new BarcodeTypeForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new DataEditForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new IOInstructionForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new ImageProcessingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new AlgorithmProtocolForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new CommunicateSettingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new TemplateSettingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new AdvancedSettingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new ImageSaveSettingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new OthersSettingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new DeviceSettingForm(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new CMD_Tool(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new ComDebugTool(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new ConfigBarcodeCheck(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new DriverInstall(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new FormNet(), ToolCfg.ConfigPath);
				controlAndXML.SubControlToXmlByLinq(new FormDeviceIP(), ToolCfg.ConfigPath);
				AddContextMenu("简体中文", TsmTop_Language.DropDownItems, MenuClicked);
				controlAndXML.AddTextToXml(MultLanguageText.CurLanguageCfg, ToolCfg.ConfigPath);
				return;
			}
			try
			{
				try
				{
					IniConfigFile iniConfigFile = new IniConfigFile();
					iniConfigFile.Config("config.ini");
					ToolCfg.ConfigPath = iniConfigFile.get("Language") + ".xml";
				}
				catch (Exception)
				{
				}
				SwitchLanguage(ToolCfg.ConfigPath);
			}
			catch (Exception ex2)
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS"))
				{
					MessageBox.Show("程序运行需要的" + ToolCfg.ConfigPath + "配置文件格式不正确，导致程序不能正常运行。\r\n可以删掉程序目录下的" + ToolCfg.ConfigPath + "文件重试\r\n错误信息：\r\n" + ex2.Message, "错误");
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("軟體運行需要的" + ToolCfg.ConfigPath + "配置文件格式不正確，導致軟體不能正常運行。\r\n可以刪掉軟體目錄下的" + ToolCfg.ConfigPath + "文件重試\r\n錯誤信息：\r\n" + ex2.Message, "錯誤");
				}
				else
				{
					MessageBox.Show("Software important file:" + ToolCfg.ConfigPath + " which format is not match ，software will exit after press enter key.\r\nyou can delect " + ToolCfg.ConfigPath + "file in software directory and retryError Message:\r\n" + ex2.Message, "ERROR");
				}
			}
		}

		private void SwitchLanguage(string XmlPath)
		{
			ControlAndXML controlAndXML = new ControlAndXML();
			controlAndXML.XMLToControlByLinq(XmlPath, this);
			Text += ToolCfg.VersionInfo;
			controlAndXML.XMLToControlByLinq(XmlPath, DeviceConnectPage);
			controlAndXML.XMLToControlByLinq(XmlPath, ReadingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, ReadingPage.DeviceSettingPage.LightingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, ReadingPage.DeviceSettingPage.BarcodeTypePage);
			controlAndXML.XMLToControlByLinq(XmlPath, DataEditPage);
			controlAndXML.XMLToControlByLinq(XmlPath, IOInstructionPage);
			controlAndXML.XMLToControlByLinq(XmlPath, ReadingPage.DeviceSettingPage.ImageProcessingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, AlgorithmProtocolPage);
			controlAndXML.XMLToControlByLinq(XmlPath, CommunicateSettingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, ReadingPage.DeviceSettingPage.TemplateSettingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, AdvancedSettingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, ImageSaveSettingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, OthersSettingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, ReadingPage.DeviceSettingPage);
			controlAndXML.XMLToControlByLinq(XmlPath, ToolCfg.FormCMD_Tool);
			controlAndXML.XMLToControlByLinq(XmlPath, ComDebugToolForm);
			controlAndXML.XMLToControlByLinq(XmlPath, ToolCfg.ConfigBarcodeCheckForm);
			controlAndXML.XMLToControlByLinq(XmlPath, DeviceConnectPage.DriverInstallForm);
			controlAndXML.XMLToControlByLinq(XmlPath, FormNetTools);
			controlAndXML.XMLToControlByLinq(XmlPath, ToolCfg.DeviceIPSettingPage);
			MultLanguageText.SetCurLanguageText(controlAndXML.ReadMsgTextInfoFromXml(XmlPath));
			if (DevCapacityArr.CapacityArr == null)
			{
				DevCapacityArr.CapacityArr = new bool[64];
			}
			if (DeviceNameTemp == null)
			{
				DeviceNameTemp = "";
			}
			ReadingPage.DeviceSettingPage.LightingPage.FunctionOnOff(DevCapacityArr.CapacityArr, DeviceNameTemp);
			try
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS"))
				{
					for (int i = 0; i < PageList.Count; i++)
					{
						PageList[i].Text = PageName_C[i];
					}
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					for (int j = 0; j < PageList.Count; j++)
					{
						PageList[j].Text = PageName_T[j];
					}
				}
				else
				{
					for (int k = 0; k < PageList.Count; k++)
					{
						PageList[k].Text = PageName_E[k];
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private ToolStripMenuItem AddContextMenu(string text, ToolStripItemCollection cms, EventHandler callback)
		{
			if (text == "-")
			{
				ToolStripSeparator value = new ToolStripSeparator();
				cms.Add(value);
				return null;
			}
			if (!string.IsNullOrEmpty(text))
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text);
				int num = 169;
				toolStripMenuItem.BackColor = Color.FromArgb(num, num, num);
				toolStripMenuItem.ForeColor = Color.Black;
				if (callback != null)
				{
					toolStripMenuItem.Click += callback;
				}
				cms.Add(toolStripMenuItem);
				return toolStripMenuItem;
			}
			return null;
		}

		private void MenuClicked(object sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem).Text.Contains("简体中文"))
			{
				ToolCfg.ConfigPath = "ChineseS.xml";
			}
			else if ((sender as ToolStripMenuItem).Text.Contains("繁體中文"))
			{
				ToolCfg.ConfigPath = "ChineseT.xml";
			}
			else
			{
				ToolCfg.ConfigPath = (sender as ToolStripMenuItem).Text.Substring(0, (sender as ToolStripMenuItem).Text.Length) + ".xml";
			}
			try
			{
				IniConfigFile iniConfigFile = new IniConfigFile();
				iniConfigFile.Config("config.ini");
				iniConfigFile.set("Language", ToolCfg.ConfigPath.Substring(0, ToolCfg.ConfigPath.Length - 4));
				iniConfigFile.save();
				SwitchLanguage(ToolCfg.ConfigPath);
				UpdateLanguageUI();
			}
			catch (Exception ex)
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS"))
				{
					MessageBox.Show("程序运行需要的" + ToolCfg.ConfigPath + "配置文件格式不正确，导致程序不能正常运行。\r\n可以删掉程序目录下的" + ToolCfg.ConfigPath + "文件重试\r\n错误信息：\r\n" + ex.Message, "错误");
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("軟體運行需要的" + ToolCfg.ConfigPath + "配置文件格式不正確，導致軟體不能正常運行。\r\n可以刪掉軟體目錄下的" + ToolCfg.ConfigPath + "文件重試\r\n錯誤信息：\r\n" + ex.Message, "錯誤");
				}
				else
				{
					MessageBox.Show("Software important file:" + ToolCfg.ConfigPath + " which format is not match ，software will exit after press enter key.\r\nyou can delect " + ToolCfg.ConfigPath + "file in software directory and retryError Message:\r\n" + ex.Message, "ERROR");
				}
			}
		}

		private void UpdateLanguageUI()
		{
			DeviceConnectPage.UpdateLanguageUI();
			ReadingPage.UpdateLanguageUI();
			IOInstructionPage.UpdateLanguageUI();
			AlgorithmProtocolPage.UpdateLanguageUI();
			CommunicateSettingPage.UpdateLanguageUI();
			ReadingPage.DeviceSettingPage.TemplateSettingPage.UpdateLanguageUI();
			AdvancedSettingPage.UpdateLanguageUI();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
			}
			try
			{
				ToolCfg.UpdateAdjState = true;
				ToolCfg.SendReadBackCMD();
			}
			catch
			{
			}
		}

		private void ShowBootAnimation()
		{
			StartForm startForm = new StartForm();
			FormClosedTimer = new System.Timers.Timer(5000.0);
			FormClosedTimer.Elapsed += FormClosedTimer_Elapsed;
			FormClosedTimer.Start();
			startForm.Show();
			while (NeedWaiteStart)
			{
				Thread.Sleep(100);
				Application.DoEvents();
			}
			startForm.Close();
		}

		private void FormClosedTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			NeedWaiteStart = false;
			FormClosedTimer.Stop();
			FormClosedTimer.Dispose();
		}

		private void Template_Get_Show(ref template_config_t tmp, TemplateActionDef act)
		{
			switch (act)
			{
			case TemplateActionDef.Get_New_Template_Value:
				tmp.decode_config.decode_lib = AlgorithmProtocolPage.DecodeMode;
				tmp.decode_config.type_switch = ReadingPage.DeviceSettingPage.BarcodeTypePage.BarcodeSwitch;
				tmp.sensor_config.light_pwm1 = ReadingPage.DeviceSettingPage.LightingPage.Light_Pwm_1;
				tmp.sensor_config.light_pwm2 = ReadingPage.DeviceSettingPage.LightingPage.Light_Pwm_2;
				tmp.sensor_config.light_ctrl = ReadingPage.DeviceSettingPage.LightingPage.FarNearLight;
				tmp.sensor_config.exp_time = ReadingPage.TempExpVal;
				tmp.sensor_config.config_flag |= ReadingPage.ratio_value_cfg;
				tmp.sensor_config.gain_set = ReadingPage.TempGainVal;
				tmp.sensor_config.focus_set = ReadingPage.TempFocusVal;
				break;
			case TemplateActionDef.Show_New_Template_Value:
				if (((ulong)tmp.config_flag & 8uL) == 8)
				{
					bool flag = false;
					if (((ulong)tmp.sensor_config.config_flag & 2uL) == 2)
					{
						flag = true;
						ReadingPage.ratio_value_cfg = 2;
						ReadingPage.TempExpVal = tmp.sensor_config.exp_time;
					}
					else
					{
						ReadingPage.ratio_value_cfg = 32;
						ReadingPage.TempExpVal = tmp.sensor_config.exp_time;
					}
					if (((ulong)tmp.sensor_config.config_flag & 4uL) == 4)
					{
						flag = true;
						ReadingPage.TempGainVal = tmp.sensor_config.gain_set;
					}
					if (((ulong)tmp.sensor_config.config_flag & 1uL) == 1)
					{
						ReadingPage.DeviceSettingPage.LightingPage.Light_Pwm_1 = tmp.sensor_config.light_pwm1;
						ReadingPage.DeviceSettingPage.LightingPage.Light_Pwm_2 = tmp.sensor_config.light_pwm2;
					}
					if (((ulong)tmp.sensor_config.config_flag & 0x10uL) == 16)
					{
						ReadingPage.DeviceSettingPage.LightingPage.FarNearLight = tmp.sensor_config.light_ctrl;
					}
					if (((ulong)tmp.sensor_config.config_flag & 8uL) == 8)
					{
						ReadingPage.TempFocusVal = tmp.sensor_config.focus_set;
					}
				}
				if (((ulong)tmp.config_flag & 4uL) == 4)
				{
					if (tmp.decode_config.decode_lib == 1)
					{
						RdbDecodeModeA.Checked = true;
					}
					if (tmp.decode_config.decode_lib == 0)
					{
						RdbDecodeModeB.Checked = true;
					}
					if (tmp.decode_config.decode_lib == 2)
					{
						RdbDecodeModeC.Checked = true;
					}
					AlgorithmProtocolPage.DecodeMode = tmp.decode_config.decode_lib;
					ReadingPage.DeviceSettingPage.BarcodeTypePage.BarcodeSwitch = tmp.decode_config.type_switch;
				}
				break;
			case TemplateActionDef.Show_Old_Template_Value:
				ReadingPage.ratio_value_cfg = 2;
				ReadingPage.TempExpVal = tmp.sensor_config.exp_time;
				ReadingPage.TempGainVal = tmp.sensor_config.gain_set;
				ReadingPage.DeviceSettingPage.LightingPage.Light_Pwm_1 = tmp.sensor_config.light_pwm1;
				ReadingPage.DeviceSettingPage.LightingPage.Light_Pwm_2 = tmp.sensor_config.light_pwm2;
				break;
			}
		}

		private void SetDecModeValueFuncCB(ReadingForm.SetControlsValDef act, int val_1, string val_2)
		{
			switch (act)
			{
			case ReadingForm.SetControlsValDef.SetDecodeModeA:
				RdbDecodeModeA.Checked = true;
				break;
			case ReadingForm.SetControlsValDef.SetDecodeModeB:
				RdbDecodeModeB.Checked = true;
				break;
			case ReadingForm.SetControlsValDef.SetDecodeModeC:
				RdbDecodeModeC.Checked = true;
				break;
			case ReadingForm.SetControlsValDef.TrainCheck_Y:
				ChbUseTrainPara.Checked = true;
				break;
			case ReadingForm.SetControlsValDef.TrainCheck_N:
				ChbUseTrainPara.Checked = false;
				break;
			case ReadingForm.SetControlsValDef.BtnTrainTextChange:
				BtnDecodeCTraining.Text = val_2;
				break;
			}
		}

		private void RdbDecodeModeA_CheckedChanged(object sender, EventArgs e)
		{
			AlgorithmProtocolPage.SetDecodeMode(ReadingForm.SetControlsValDef.SetDecodeModeA);
			BtnDecodeCTraining.Visible = false;
			ChbUseTrainPara.Visible = false;
		}

		private void RdbDecodeModeB_CheckedChanged(object sender, EventArgs e)
		{
			AlgorithmProtocolPage.SetDecodeMode(ReadingForm.SetControlsValDef.SetDecodeModeB);
			BtnDecodeCTraining.Visible = false;
			ChbUseTrainPara.Visible = false;
		}

		private void RdbDecodeModeC_CheckedChanged(object sender, EventArgs e)
		{
			AlgorithmProtocolPage.SetDecodeMode(ReadingForm.SetControlsValDef.SetDecodeModeC);
			BtnDecodeCTraining.Visible = true;
			ChbUseTrainPara.Visible = true;
		}

		private void ChbUseTrainPara_CheckedChanged(object sender, EventArgs e)
		{
			if (ChbUseTrainPara.Checked)
			{
				AlgorithmProtocolPage.SetDecodeMode(ReadingForm.SetControlsValDef.TrainCheck_Y);
			}
			else
			{
				AlgorithmProtocolPage.SetDecodeMode(ReadingForm.SetControlsValDef.TrainCheck_N);
			}
		}

		private void BtnDecodeCTraining_Click(object sender, EventArgs e)
		{
			BtnDecodeCTraining.Text = AlgorithmProtocolPage.BtnDecodeCTraining_Click_Action();
		}

		private void SetUiSplitDistance()
		{
			try
			{
				IniConfigFile iniConfigFile = new IniConfigFile();
				iniConfigFile.Config("config.ini");
				//if (!iniConfigFile.get("ThisWindowState").Contains(base.WindowState.ToString()))
				//{
				//	return;
				//}
				int num = Convert.ToInt32(iniConfigFile.get("ThisScreenWidth"));
				if (num == Screen.PrimaryScreen.Bounds.Width)
				{
					int num2 = Convert.ToInt32(iniConfigFile.get("ThisScreenHeight"));
					if (num2 == Screen.PrimaryScreen.Bounds.Height)
					{
						MainContainer.SplitterDistance = Convert.ToInt32(iniConfigFile.get("SplitDisConnectPage"));
						ToolCfg.SplitDisReadingPage_h = Convert.ToInt32(iniConfigFile.get("SplitDisReadingPage_h"));
						ToolCfg.SplitDisReadingPage_v = Convert.ToInt32(iniConfigFile.get("SplitDisReadingPage_v"));
						ReadingPage.Set_SplitDistance_h_v(ToolCfg.SplitDisReadingPage_h, ToolCfg.SplitDisReadingPage_v);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void SaveUiSplitDistance()
		{
			try
			{
				ToolCfg.ThisScreenWidth = Screen.PrimaryScreen.Bounds.Width;
				ToolCfg.ThisScreenHeight = Screen.PrimaryScreen.Bounds.Height;
				ToolCfg.SplitDisConnectPage = MainContainer.SplitterDistance;
				ReadingPage.Get_SplitDistance_h_v(ref ToolCfg.SplitDisReadingPage_h, ref ToolCfg.SplitDisReadingPage_v);
				IniConfigFile iniConfigFile = new IniConfigFile();
				iniConfigFile.Config("config.ini");
				iniConfigFile.set("ThisWindowState", base.WindowState.ToString());
				iniConfigFile.set("ThisScreenWidth", ToolCfg.ThisScreenWidth.ToString());
				iniConfigFile.set("ThisScreenHeight", ToolCfg.ThisScreenHeight.ToString());
				iniConfigFile.set("SplitDisConnectPage", ToolCfg.SplitDisConnectPage.ToString());
				iniConfigFile.set("SplitDisReadingPage_h", ToolCfg.SplitDisReadingPage_h.ToString());
				iniConfigFile.set("SplitDisReadingPage_v", ToolCfg.SplitDisReadingPage_v.ToString());
				iniConfigFile.save();
			}
			catch (Exception)
			{
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveUiSplitDistance();
			DeviceConnectPage.FormClosingDo(sender, e);
		}

		private void InstallUSBDrvToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.InstallUSBDrvTool(sender, e);
		}

		private void ComDebugToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ComDebugToolForm == null || ComDebugToolForm.IsDisposed)
			{
				ComDebugToolForm = new ComDebugTool();
				ComDebugToolForm.Show();
			}
			else
			{
				ComDebugToolForm.Show();
				ComDebugToolForm.Focus();
			}
		}

		private void NetworkDebugToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FormNetTools == null || FormNetTools.IsDisposed)
			{
				FormNetTools = new FormNet();
				FormNetTools.Show();
			}
			else
			{
				FormNetTools.Show();
				FormNetTools.Activate();
			}
		}

		private void CmdCfgToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ToolCfg.FormCMD_Tool == null || ToolCfg.FormCMD_Tool.IsDisposed)
			{
				ToolCfg.FormCMD_Tool = new CMD_Tool();
				ToolCfg.FormCMD_Tool.SetCBFunc(GetCfgCBFunc);
				ToolCfg.FormCMD_Tool.Show();
			}
			else
			{
				ToolCfg.FormCMD_Tool.SetCBFunc(GetCfgCBFunc);
				ToolCfg.FormCMD_Tool.Show();
				ToolCfg.FormCMD_Tool.Focus();
			}
		}

		private void TSM_AboutSoftware_Click(object sender, EventArgs e)
		{
			AboutForm = new About();
			AboutForm.ShowDialog();
		}

		private void TSM_SoftwareCloseDo_Click(object sender, EventArgs e)
		{
			FormSetting = new SettingForm();
			FormSetting.ShowDialog();
		}

		private void TSM_FirmwareVersion_Click(object sender, EventArgs e)
		{
			ShowFirmwareVerForm showFirmwareVerForm = new ShowFirmwareVerForm();
			showFirmwareVerForm.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc);
			showFirmwareVerForm.ShowDialog();
		}

		private void TSB_SaveCurrentCfg_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.BtnSaveCurrentCfg_Click(null, null);
		}

		private void TSB_RestartDevice_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.Tsm_RestartDevice_Click(null, null);
		}

		private void TsmTop_Tool_DropDownOpening(object sender, EventArgs e)
		{
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				InstallUSBDrvToolStripMenuItem.Text = "安装USB驱动";
				Tsm_ComDebugTool.Text = "串口调试助手";
				Tsm_NetDebugTool.Text = "网口调试助手";
				Tsm_CmdConfigurationTool.Text = "命令配置工具";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				InstallUSBDrvToolStripMenuItem.Text = "安裝USB驅動";
				Tsm_ComDebugTool.Text = "串口調試助手";
				Tsm_NetDebugTool.Text = "網口調試助手";
				Tsm_CmdConfigurationTool.Text = "命令配置工具";
			}
			else
			{
				InstallUSBDrvToolStripMenuItem.Text = "Install USB Driver";
				Tsm_ComDebugTool.Text = "Serial Assistant";
				Tsm_NetDebugTool.Text = "Network Assistant";
				Tsm_CmdConfigurationTool.Text = "Command Configuration Tool";
			}
		}

		private void TsmTop_Disp_DropDownOpening(object sender, EventArgs e)
		{
			Tsm_ShowNetGrid.Checked = ToolCfg.IsDispNetGrid;
			if (ToolCfg.ConfigPath.Contains("ChineseS"))
			{
				Tsm_ShowNetGrid.Text = "显示标线";
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				Tsm_ShowNetGrid.Text = "顯示標線";
			}
			else
			{
				Tsm_ShowNetGrid.Text = "Show Grid";
			}
		}

		private void Tsm_ShowNetGrid_Click(object sender, EventArgs e)
		{
			Tsm_ShowNetGrid.Checked = !Tsm_ShowNetGrid.Checked;
			ReadingPage.TSI_ShowNetGrid_Click(null, null);
		}

		private void TSB_SearchDevice_Click(object sender, EventArgs e)
		{
			TSB_SearchDevice.Enabled = false;
			DeviceConnectPage.Tsb_SearchDevice_Click(null, null);
			TSB_SearchDevice.Enabled = true;
		}

		private void TSB_ConnectDevice_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.Tsb_ConnectDevice_Click(null, null);
		}

		private void TSB_DisconnectDevice_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.Tsb_DisconnectDevice_Click(null, null);
		}

		private void TSB_OpenCfg_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.Tsb_OpenCfg_Click(null, null);
		}

		private void TSB_SaveCfg_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.Tsb_SaveCfg_Click(null, null);
		}

		private void TSM_UpgradeFirmware_Click(object sender, EventArgs e)
		{
			if (AdvancedSettingPage.CommunicationCheck())
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = ".bin|*.bin";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					AdvancedSettingPage.UploadFileToDevice(openFileDialog.FileName, true);
				}
			}
		}

		private void TSI_OpenCfg_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.Tsb_OpenCfg_Click(null, null);
		}

		private void TSI_SaveCfg_Click(object sender, EventArgs e)
		{
			DeviceConnectPage.Tsb_SaveCfg_Click(null, null);
		}

		private void TSI_SaveCurrentImage_Click(object sender, EventArgs e)
		{
			ReadingPage.TSI_SaveCurrentImage_Click(null, null);
		}

		private void TemplatePage_Draw_ROI_CB(DeviceSettingForm.ImageRoiActionNum Act)
		{
			ReadingPage.ImageRoiActionCBFunc(Act);
		}

		private void BtnSaveCurrentCfg_BtnClick(object sender, EventArgs e)
		{
			DeviceConnectPage.BtnSaveCurrentCfg_Click(null, null);
		}

		private void BtnRestartDevice_BtnClick(object sender, EventArgs e)
		{
			DeviceConnectPage.Tsm_RestartDevice_Click(null, null);
		}

		private void DevStateCB_Func(DevStateDef a)
		{
			switch (a)
			{
			case DevStateDef.DevConnected:
				TSB_ConnectDevice.Enabled = false;
				TSB_DisconnectDevice.Enabled = true;
				break;
			case DevStateDef.DevDisConnnected:
				TSB_ConnectDevice.Enabled = true;
				TSB_DisconnectDevice.Enabled = false;
				break;
			case DevStateDef.BothDisenable:
				TSB_ConnectDevice.Enabled = false;
				TSB_DisconnectDevice.Enabled = false;
				break;
			}
		}

		private void Tsm_Regular_Barcode_Click(object sender, EventArgs e)
		{
			RegularForm regularForm = new RegularForm();
			regularForm.Regex_txt_path = regularForm.REGEX_BARCODE_STRING;
			regularForm.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc, Tsm_Regular_Barcode.Text);
			regularForm.ShowDialog();
		}

		private void Tsm_Regular_Output_Click(object sender, EventArgs e)
		{
			RegularForm regularForm = new RegularForm();
			regularForm.Regex_txt_path = regularForm.REGEX_OUTPUT_STRING;
			regularForm.SetCBFunc(SetCfgCBFunc, GetCfgCBFunc, SendCfgDataCBFunc, Tsm_Regular_Output.Text);
			regularForm.ShowDialog();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Heroje_Debug_Tool.MainForm));
			this.MenuTopMain = new System.Windows.Forms.MenuStrip();
			this.TsmTop_File = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_OpenCfg = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_SaveCfg = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_SaveCurrentImage = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmTop_Disp = new System.Windows.Forms.ToolStripMenuItem();
			this.Tsm_ShowNetGrid = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmTop_Tool = new System.Windows.Forms.ToolStripMenuItem();
			this.InstallUSBDrvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Tsm_ComDebugTool = new System.Windows.Forms.ToolStripMenuItem();
			this.Tsm_NetDebugTool = new System.Windows.Forms.ToolStripMenuItem();
			this.Tsm_CmdConfigurationTool = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmTop_Setting = new System.Windows.Forms.ToolStripMenuItem();
			this.Tsm_Regular_Barcode = new System.Windows.Forms.ToolStripMenuItem();
			this.Tsm_Regular_Output = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmTop_Language = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmTop_Sys = new System.Windows.Forms.ToolStripMenuItem();
			this.TSM_AboutSoftware = new System.Windows.Forms.ToolStripMenuItem();
			this.TSM_FirmwareVersion = new System.Windows.Forms.ToolStripMenuItem();
			this.TSM_SoftwareCloseDo = new System.Windows.Forms.ToolStripMenuItem();
			this.TSM_UpgradeFirmware = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmTop_Help = new System.Windows.Forms.ToolStripMenuItem();
			this.TSI_InstallGuide = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStrip_Icon = new System.Windows.Forms.ToolStrip();
			this.TSB_SearchDevice = new System.Windows.Forms.ToolStripButton();
			this.TSB_ConnectDevice = new System.Windows.Forms.ToolStripButton();
			this.TSB_DisconnectDevice = new System.Windows.Forms.ToolStripButton();
			this.TSS_Separator1 = new System.Windows.Forms.ToolStripSeparator();
			this.TSB_OpenCfg = new System.Windows.Forms.ToolStripButton();
			this.TSB_SaveCfg = new System.Windows.Forms.ToolStripButton();
			this.TSS_Separator2 = new System.Windows.Forms.ToolStripSeparator();
			this.MainContainer = new System.Windows.Forms.SplitContainer();
			this.TabSetting = new System.Windows.Forms.TabControl();
			this.PanDecMode = new System.Windows.Forms.Panel();
			this.ChbUseTrainPara = new System.Windows.Forms.CheckBox();
			this.BtnDecodeCTraining = new System.Windows.Forms.Button();
			this.RdbDecodeModeC = new System.Windows.Forms.RadioButton();
			this.RdbDecodeModeB = new System.Windows.Forms.RadioButton();
			this.RdbDecodeModeA = new System.Windows.Forms.RadioButton();
			this.BtnSaveCurrentCfg = new System.Windows.Forms.Button();
			this.BtnRestartDevice = new System.Windows.Forms.Button();
			this.MenuTopMain.SuspendLayout();
			this.ToolStrip_Icon.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.MainContainer).BeginInit();
			this.MainContainer.Panel2.SuspendLayout();
			this.MainContainer.SuspendLayout();
			this.PanDecMode.SuspendLayout();
			base.SuspendLayout();
			this.MenuTopMain.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5f);
			this.MenuTopMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.TsmTop_File, this.TsmTop_Disp, this.TsmTop_Tool, this.TsmTop_Setting, this.TsmTop_Language, this.TsmTop_Sys, this.TsmTop_Help });
			this.MenuTopMain.Location = new System.Drawing.Point(0, 0);
			this.MenuTopMain.Name = "MenuTopMain";
			this.MenuTopMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			this.MenuTopMain.Size = new System.Drawing.Size(1264, 28);
			this.MenuTopMain.TabIndex = 0;
			this.MenuTopMain.Text = "MenuStripTop";
			this.TsmTop_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.TSI_OpenCfg, this.TSI_SaveCfg, this.TSI_SaveCurrentImage });
			this.TsmTop_File.Name = "TsmTop_File";
			this.TsmTop_File.Size = new System.Drawing.Size(66, 24);
			this.TsmTop_File.Text = "文件(F)";
			this.TSI_OpenCfg.Name = "TSI_OpenCfg";
			this.TSI_OpenCfg.Size = new System.Drawing.Size(168, 24);
			this.TSI_OpenCfg.Text = "打开配置(O)";
			this.TSI_OpenCfg.Click += new System.EventHandler(TSI_OpenCfg_Click);
			this.TSI_SaveCfg.Name = "TSI_SaveCfg";
			this.TSI_SaveCfg.Size = new System.Drawing.Size(168, 24);
			this.TSI_SaveCfg.Text = "保存配置(S)";
			this.TSI_SaveCfg.Click += new System.EventHandler(TSI_SaveCfg_Click);
			this.TSI_SaveCurrentImage.Name = "TSI_SaveCurrentImage";
			this.TSI_SaveCurrentImage.Size = new System.Drawing.Size(168, 24);
			this.TSI_SaveCurrentImage.Text = "图像另存为(A)";
			this.TSI_SaveCurrentImage.Click += new System.EventHandler(TSI_SaveCurrentImage_Click);
			this.TsmTop_Disp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.Tsm_ShowNetGrid });
			this.TsmTop_Disp.Name = "TsmTop_Disp";
			this.TsmTop_Disp.Size = new System.Drawing.Size(68, 24);
			this.TsmTop_Disp.Text = "显示(V)";
			this.TsmTop_Disp.DropDownOpening += new System.EventHandler(TsmTop_Disp_DropDownOpening);
			this.Tsm_ShowNetGrid.Name = "Tsm_ShowNetGrid";
			this.Tsm_ShowNetGrid.Size = new System.Drawing.Size(134, 24);
			this.Tsm_ShowNetGrid.Text = "显示标线";
			this.Tsm_ShowNetGrid.Click += new System.EventHandler(Tsm_ShowNetGrid_Click);
			this.TsmTop_Tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.InstallUSBDrvToolStripMenuItem, this.Tsm_ComDebugTool, this.Tsm_NetDebugTool, this.Tsm_CmdConfigurationTool });
			this.TsmTop_Tool.Name = "TsmTop_Tool";
			this.TsmTop_Tool.Size = new System.Drawing.Size(66, 24);
			this.TsmTop_Tool.Text = "工具(L)";
			this.TsmTop_Tool.DropDownOpening += new System.EventHandler(TsmTop_Tool_DropDownOpening);
			this.InstallUSBDrvToolStripMenuItem.Name = "InstallUSBDrvToolStripMenuItem";
			this.InstallUSBDrvToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
			this.InstallUSBDrvToolStripMenuItem.Text = "安装USB驱动";
			this.InstallUSBDrvToolStripMenuItem.Click += new System.EventHandler(InstallUSBDrvToolStripMenuItem_Click);
			this.Tsm_ComDebugTool.Name = "Tsm_ComDebugTool";
			this.Tsm_ComDebugTool.Size = new System.Drawing.Size(162, 24);
			this.Tsm_ComDebugTool.Text = "串口调试助手";
			this.Tsm_ComDebugTool.Click += new System.EventHandler(ComDebugToolStripMenuItem_Click);
			this.Tsm_NetDebugTool.Name = "Tsm_NetDebugTool";
			this.Tsm_NetDebugTool.Size = new System.Drawing.Size(162, 24);
			this.Tsm_NetDebugTool.Text = "网口调试助手";
			this.Tsm_NetDebugTool.Click += new System.EventHandler(NetworkDebugToolStripMenuItem_Click);
			this.Tsm_CmdConfigurationTool.Name = "Tsm_CmdConfigurationTool";
			this.Tsm_CmdConfigurationTool.Size = new System.Drawing.Size(162, 24);
			this.Tsm_CmdConfigurationTool.Text = "命令配置工具";
			this.Tsm_CmdConfigurationTool.Click += new System.EventHandler(CmdCfgToolStripMenuItem_Click);
			this.TsmTop_Setting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.Tsm_Regular_Barcode, this.Tsm_Regular_Output });
			this.TsmTop_Setting.Name = "TsmTop_Setting";
			this.TsmTop_Setting.Size = new System.Drawing.Size(95, 24);
			this.TsmTop_Setting.Text = "高级设置(S)";
			this.Tsm_Regular_Barcode.Name = "Tsm_Regular_Barcode";
			this.Tsm_Regular_Barcode.Size = new System.Drawing.Size(210, 24);
			this.Tsm_Regular_Barcode.Text = "正则表达式-条码匹配";
			this.Tsm_Regular_Barcode.Click += new System.EventHandler(Tsm_Regular_Barcode_Click);
			this.Tsm_Regular_Output.Name = "Tsm_Regular_Output";
			this.Tsm_Regular_Output.Size = new System.Drawing.Size(210, 24);
			this.Tsm_Regular_Output.Text = "正则表达式-输出过滤";
			this.Tsm_Regular_Output.Click += new System.EventHandler(Tsm_Regular_Output_Click);
			this.TsmTop_Language.Name = "TsmTop_Language";
			this.TsmTop_Language.Size = new System.Drawing.Size(49, 24);
			this.TsmTop_Language.Text = "语言";
			this.TsmTop_Sys.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.TSM_AboutSoftware, this.TSM_FirmwareVersion, this.TSM_SoftwareCloseDo, this.TSM_UpgradeFirmware });
			this.TsmTop_Sys.Name = "TsmTop_Sys";
			this.TsmTop_Sys.Size = new System.Drawing.Size(73, 24);
			this.TsmTop_Sys.Text = "系统(M)";
			this.TSM_AboutSoftware.Name = "TSM_AboutSoftware";
			this.TSM_AboutSoftware.Size = new System.Drawing.Size(204, 24);
			this.TSM_AboutSoftware.Text = "关于软件";
			this.TSM_AboutSoftware.Click += new System.EventHandler(TSM_AboutSoftware_Click);
			this.TSM_FirmwareVersion.Name = "TSM_FirmwareVersion";
			this.TSM_FirmwareVersion.Size = new System.Drawing.Size(204, 24);
			this.TSM_FirmwareVersion.Text = "查看固件版本信息";
			this.TSM_FirmwareVersion.Click += new System.EventHandler(TSM_FirmwareVersion_Click);
			this.TSM_SoftwareCloseDo.Name = "TSM_SoftwareCloseDo";
			this.TSM_SoftwareCloseDo.Size = new System.Drawing.Size(204, 24);
			this.TSM_SoftwareCloseDo.Text = "设置软件退出时动作";
			this.TSM_SoftwareCloseDo.Click += new System.EventHandler(TSM_SoftwareCloseDo_Click);
			this.TSM_UpgradeFirmware.Name = "TSM_UpgradeFirmware";
			this.TSM_UpgradeFirmware.Size = new System.Drawing.Size(204, 24);
			this.TSM_UpgradeFirmware.Text = "固件升级";
			this.TSM_UpgradeFirmware.Click += new System.EventHandler(TSM_UpgradeFirmware_Click);
			this.TsmTop_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.TSI_InstallGuide });
			this.TsmTop_Help.Name = "TsmTop_Help";
			this.TsmTop_Help.Size = new System.Drawing.Size(70, 24);
			this.TsmTop_Help.Text = "帮助(H)";
			this.TSI_InstallGuide.Name = "TSI_InstallGuide";
			this.TSI_InstallGuide.Size = new System.Drawing.Size(180, 24);
			this.TSI_InstallGuide.Text = "安装指南";
			this.ToolStrip_Icon.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.ToolStrip_Icon.AutoSize = false;
			this.ToolStrip_Icon.BackColor = System.Drawing.SystemColors.Control;
			this.ToolStrip_Icon.Dock = System.Windows.Forms.DockStyle.None;
			this.ToolStrip_Icon.Font = new System.Drawing.Font("宋体", 10.5f);
			this.ToolStrip_Icon.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.TSB_SearchDevice, this.TSB_ConnectDevice, this.TSB_DisconnectDevice, this.TSS_Separator1, this.TSB_OpenCfg, this.TSB_SaveCfg, this.TSS_Separator2 });
			this.ToolStrip_Icon.Location = new System.Drawing.Point(0, 31);
			this.ToolStrip_Icon.Name = "ToolStrip_Icon";
			this.ToolStrip_Icon.Size = new System.Drawing.Size(1264, 77);
			this.ToolStrip_Icon.TabIndex = 1;
			this.ToolStrip_Icon.Text = "toolStrip1";
			this.TSB_SearchDevice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.TSB_SearchDevice.Image = (System.Drawing.Image)resources.GetObject("TSB_SearchDevice.Image");
			this.TSB_SearchDevice.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.TSB_SearchDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSB_SearchDevice.Name = "TSB_SearchDevice";
			this.TSB_SearchDevice.Size = new System.Drawing.Size(68, 74);
			this.TSB_SearchDevice.Text = "搜索设备";
			this.TSB_SearchDevice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.TSB_SearchDevice.Click += new System.EventHandler(TSB_SearchDevice_Click);
			this.TSB_ConnectDevice.Enabled = false;
			this.TSB_ConnectDevice.Image = (System.Drawing.Image)resources.GetObject("TSB_ConnectDevice.Image");
			this.TSB_ConnectDevice.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.TSB_ConnectDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSB_ConnectDevice.Name = "TSB_ConnectDevice";
			this.TSB_ConnectDevice.Size = new System.Drawing.Size(68, 74);
			this.TSB_ConnectDevice.Text = "连接设备";
			this.TSB_ConnectDevice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.TSB_ConnectDevice.Click += new System.EventHandler(TSB_ConnectDevice_Click);
			this.TSB_DisconnectDevice.Enabled = false;
			this.TSB_DisconnectDevice.Image = (System.Drawing.Image)resources.GetObject("TSB_DisconnectDevice.Image");
			this.TSB_DisconnectDevice.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.TSB_DisconnectDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSB_DisconnectDevice.Name = "TSB_DisconnectDevice";
			this.TSB_DisconnectDevice.Size = new System.Drawing.Size(68, 74);
			this.TSB_DisconnectDevice.Text = "断开设备";
			this.TSB_DisconnectDevice.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.TSB_DisconnectDevice.Click += new System.EventHandler(TSB_DisconnectDevice_Click);
			this.TSS_Separator1.Name = "TSS_Separator1";
			this.TSS_Separator1.Size = new System.Drawing.Size(6, 77);
			this.TSB_OpenCfg.Image = (System.Drawing.Image)resources.GetObject("TSB_OpenCfg.Image");
			this.TSB_OpenCfg.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.TSB_OpenCfg.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSB_OpenCfg.Name = "TSB_OpenCfg";
			this.TSB_OpenCfg.Size = new System.Drawing.Size(68, 74);
			this.TSB_OpenCfg.Text = "打开配置";
			this.TSB_OpenCfg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.TSB_OpenCfg.Click += new System.EventHandler(TSB_OpenCfg_Click);
			this.TSB_SaveCfg.Image = (System.Drawing.Image)resources.GetObject("TSB_SaveCfg.Image");
			this.TSB_SaveCfg.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.TSB_SaveCfg.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TSB_SaveCfg.Name = "TSB_SaveCfg";
			this.TSB_SaveCfg.Size = new System.Drawing.Size(68, 74);
			this.TSB_SaveCfg.Text = "保存配置";
			this.TSB_SaveCfg.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.TSB_SaveCfg.Click += new System.EventHandler(TSB_SaveCfg_Click);
			this.TSS_Separator2.Name = "TSS_Separator2";
			this.TSS_Separator2.Size = new System.Drawing.Size(6, 77);
			this.MainContainer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.MainContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.MainContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.MainContainer.Location = new System.Drawing.Point(1, 111);
			this.MainContainer.Name = "MainContainer";
			this.MainContainer.Panel2.Controls.Add(this.TabSetting);
			this.MainContainer.Size = new System.Drawing.Size(1260, 646);
			this.MainContainer.SplitterDistance = 340;
			this.MainContainer.SplitterWidth = 5;
			this.MainContainer.TabIndex = 4;
			this.TabSetting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TabSetting.ItemSize = new System.Drawing.Size(60, 29);
			this.TabSetting.Location = new System.Drawing.Point(-1, -1);
			this.TabSetting.Name = "TabSetting";
			this.TabSetting.SelectedIndex = 0;
			this.TabSetting.Size = new System.Drawing.Size(893, 642);
			this.TabSetting.TabIndex = 0;
			this.PanDecMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.PanDecMode.BackColor = System.Drawing.SystemColors.Control;
			this.PanDecMode.Controls.Add(this.ChbUseTrainPara);
			this.PanDecMode.Controls.Add(this.BtnDecodeCTraining);
			this.PanDecMode.Controls.Add(this.RdbDecodeModeC);
			this.PanDecMode.Controls.Add(this.RdbDecodeModeB);
			this.PanDecMode.Controls.Add(this.RdbDecodeModeA);
			this.PanDecMode.Location = new System.Drawing.Point(699, 58);
			this.PanDecMode.Name = "PanDecMode";
			this.PanDecMode.Size = new System.Drawing.Size(345, 27);
			this.PanDecMode.TabIndex = 5;
			this.ChbUseTrainPara.AutoSize = true;
			this.ChbUseTrainPara.Location = new System.Drawing.Point(275, 7);
			this.ChbUseTrainPara.Name = "ChbUseTrainPara";
			this.ChbUseTrainPara.Size = new System.Drawing.Size(15, 14);
			this.ChbUseTrainPara.TabIndex = 55;
			this.ChbUseTrainPara.UseVisualStyleBackColor = true;
			this.ChbUseTrainPara.Visible = false;
			this.ChbUseTrainPara.CheckedChanged += new System.EventHandler(ChbUseTrainPara_CheckedChanged);
			this.BtnDecodeCTraining.Location = new System.Drawing.Point(291, 2);
			this.BtnDecodeCTraining.Name = "BtnDecodeCTraining";
			this.BtnDecodeCTraining.Size = new System.Drawing.Size(48, 23);
			this.BtnDecodeCTraining.TabIndex = 54;
			this.BtnDecodeCTraining.Text = "训练";
			this.BtnDecodeCTraining.UseVisualStyleBackColor = true;
			this.BtnDecodeCTraining.Visible = false;
			this.BtnDecodeCTraining.Click += new System.EventHandler(BtnDecodeCTraining_Click);
			this.RdbDecodeModeC.AutoSize = true;
			this.RdbDecodeModeC.Location = new System.Drawing.Point(187, 4);
			this.RdbDecodeModeC.Name = "RdbDecodeModeC";
			this.RdbDecodeModeC.Size = new System.Drawing.Size(88, 18);
			this.RdbDecodeModeC.TabIndex = 30;
			this.RdbDecodeModeC.Text = "C增强模式";
			this.RdbDecodeModeC.UseVisualStyleBackColor = true;
			this.RdbDecodeModeC.CheckedChanged += new System.EventHandler(RdbDecodeModeC_CheckedChanged);
			this.RdbDecodeModeB.AutoSize = true;
			this.RdbDecodeModeB.Location = new System.Drawing.Point(97, 3);
			this.RdbDecodeModeB.Name = "RdbDecodeModeB";
			this.RdbDecodeModeB.Size = new System.Drawing.Size(88, 18);
			this.RdbDecodeModeB.TabIndex = 29;
			this.RdbDecodeModeB.Text = "B快速模式";
			this.RdbDecodeModeB.UseVisualStyleBackColor = true;
			this.RdbDecodeModeB.CheckedChanged += new System.EventHandler(RdbDecodeModeB_CheckedChanged);
			this.RdbDecodeModeA.AutoSize = true;
			this.RdbDecodeModeA.Location = new System.Drawing.Point(3, 3);
			this.RdbDecodeModeA.Name = "RdbDecodeModeA";
			this.RdbDecodeModeA.Size = new System.Drawing.Size(88, 18);
			this.RdbDecodeModeA.TabIndex = 28;
			this.RdbDecodeModeA.Text = "A精细模式";
			this.RdbDecodeModeA.UseVisualStyleBackColor = true;
			this.RdbDecodeModeA.CheckedChanged += new System.EventHandler(RdbDecodeModeA_CheckedChanged);
			this.BtnSaveCurrentCfg.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.BtnSaveCurrentCfg.Location = new System.Drawing.Point(1056, 54);
			this.BtnSaveCurrentCfg.Name = "BtnSaveCurrentCfg";
			this.BtnSaveCurrentCfg.Size = new System.Drawing.Size(120, 35);
			this.BtnSaveCurrentCfg.TabIndex = 8;
			this.BtnSaveCurrentCfg.Text = "保存配置到设备";
			this.BtnSaveCurrentCfg.UseVisualStyleBackColor = true;
			this.BtnSaveCurrentCfg.Click += new System.EventHandler(BtnSaveCurrentCfg_BtnClick);
			this.BtnRestartDevice.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.BtnRestartDevice.Location = new System.Drawing.Point(1179, 54);
			this.BtnRestartDevice.Name = "BtnRestartDevice";
			this.BtnRestartDevice.Size = new System.Drawing.Size(74, 35);
			this.BtnRestartDevice.TabIndex = 9;
			this.BtnRestartDevice.Text = "重启设备";
			this.BtnRestartDevice.UseVisualStyleBackColor = true;
			this.BtnRestartDevice.Click += new System.EventHandler(BtnRestartDevice_BtnClick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(1264, 761);
			base.Controls.Add(this.BtnRestartDevice);
			base.Controls.Add(this.BtnSaveCurrentCfg);
			base.Controls.Add(this.PanDecMode);
			base.Controls.Add(this.MainContainer);
			base.Controls.Add(this.MenuTopMain);
			base.Controls.Add(this.ToolStrip_Icon);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.MainMenuStrip = this.MenuTopMain;
			base.Name = "MainForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "合杰读码调试工具V8.08";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
			base.Load += new System.EventHandler(MainForm_Load);
			this.MenuTopMain.ResumeLayout(false);
			this.MenuTopMain.PerformLayout();
			this.ToolStrip_Icon.ResumeLayout(false);
			this.ToolStrip_Icon.PerformLayout();
			this.MainContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.MainContainer).EndInit();
			this.MainContainer.ResumeLayout(false);
			this.PanDecMode.ResumeLayout(false);
			this.PanDecMode.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
