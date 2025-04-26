using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace Heroje_Debug_Tool.SubForm
{
	public class DeviceSettingForm : Form
	{
		public enum ImageRoiActionNum
		{
			DrawRoiStart,
			DrawRoiOk,
			DrawRoiCancel,
			DrawTemplateRoi,
			DrawTemplateRoiOk
		}

		public delegate void CameraSetFormActionCB(ImageRoiActionNum num);

		private CameraSetFormActionCB CameraSetFormActionCBFunc;

		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private ReadingForm.SetControlsValueCB SetControlsValueFuncCB;

		public LightingForm LightingPage = new LightingForm();

		public ImageProcessingForm ImageProcessingPage = new ImageProcessingForm();

		public BarcodeTypeForm BarcodeTypePage = new BarcodeTypeForm();

		public TemplateSettingForm TemplateSettingPage = new TemplateSettingForm();

		private bool LastTrigIsTrigMode = false;

		private int ExpRatioVal = 0;

		public bool anti_re_in = false;

		private int exp_value_val;

		private int SenGainVal = 0;

		private int FocusVal = 0;

		private IContainer components = null;

		private GroupBox GrbWorkMode;

		private Button BtnTrigTest;

		private RadioButton RdbTrigExtern;

		private RadioButton RdbContinuousTrig;

		private GroupBox GrbExpSetting;

		private Button BtnExpSetValue;

		private TextBox TxbExpSetValue;

		private Button BtnExpSetRatioAdd;

		private Button BtnExpSetRatioDec;

		private CheckBox CheckAutoExp;

		private TextBox TxbExpSetRatio;

		private RadioButton RdbExpSetValue;

		private RadioButton RdbExpSetRatio;

		private GroupBox GrbSensorGainSetting;

		private Label LabSensorGainSet;

		private Button BtnSensorGainSetAdd;

		private Button BtnSensorGainSetDec;

		private TextBox TxbSensorGainSet;

		private TabControl TabSettings;

		private TabPage TpParaAdjust;

		private GroupBox GrbSensorLensFocus;

		private Button BtnSensorLensFocusAdd;

		private Button BtnSensorLensFocusDec;

		private TextBox TxbSensorLensFocusSet;

		private Label LabSensorLensFocusSet;

		private RadioButton RdbAutoFocusEnbale;

		private RadioButton RdbAutoFocusDisabale;

		private GroupBox GrbSoundSetting;

		private RadioButton RdbSoundOff;

		private RadioButton RdbSoundOn;

		private GroupBox GrbTrigTime;

		private RadioButton RdbKeepImage;

		private RadioButton RdbKeepTime;

		private Button BtnKeepImage;

		private Label LabKeepImage;

		private TextBox TxbKeepImage;

		private Label LabKeepImageUnit;

		private Button BtnTrigKeepTime;

		private Label LabTrigKeepTimeName;

		private TextBox TxbTrigKeepTime;

		private Label LabMsSShow;

		private Panel PanHdr;

		private CheckBox ChbHDR_ModeEnable;

		private TrackBar TrbHDR_LightExp;

		private TabPage Tp_Lighting;

		private TabPage Tp_ImageProcess;

		private TabPage Tp_BarcodeType;

		private TabPage Tp_Template;

		public int ExposureRatioVal
		{
			get
			{
				return ExpRatioVal;
			}
			set
			{
				ExpRatioVal = value;
				TxbExpSetRatio.Text = ExpRatioVal.ToString();
				if (!anti_re_in)
				{
					if (ExpRatioVal == 0)
					{
						CheckAutoExp.Checked = true;
					}
					else
					{
						CheckAutoExp.Checked = false;
					}
				}
				anti_re_in = false;
			}
		}

		public int ExpValueVal
		{
			get
			{
				int result;
				if (int.TryParse(TxbExpSetValue.Text, out result))
				{
					exp_value_val = result;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("模板曝光参数非法!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show("Template exp time value illegal!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				return exp_value_val;
			}
			set
			{
				exp_value_val = value;
				TxbExpSetValue.Text = exp_value_val.ToString();
			}
		}

		public bool IsRatioExp
		{
			get
			{
				return RdbExpSetRatio.Checked;
			}
			set
			{
				RdbExpSetRatio.Checked = value;
				RdbExpSetValue.Checked = !RdbExpSetRatio.Checked;
			}
		}

		public int SensorGainVal
		{
			get
			{
				return SenGainVal;
			}
			set
			{
				SenGainVal = value;
				TxbSensorGainSet.Text = SenGainVal.ToString();
			}
		}

		public int LenFocusVal
		{
			get
			{
				return FocusVal;
			}
			set
			{
				FocusVal = value;
				TxbSensorLensFocusSet.Text = FocusVal.ToString();
			}
		}

		public DeviceSettingForm()
		{
			InitializeComponent();
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB, ReadingForm.SetControlsValueCB setControlsValueCB, CameraSetFormActionCB imageRoiActionCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
			SetControlsValueFuncCB = setControlsValueCB;
			CameraSetFormActionCBFunc = imageRoiActionCB;
		}

		private void AddPage(Control page, string pageName)
		{
			page.Show();
			TabPage tabPage = new TabPage(pageName);
			tabPage.BorderStyle = BorderStyle.FixedSingle;
			tabPage.BackColor = Color.White;
			tabPage.Controls.Add(page);
			TabSettings.TabPages.Add(tabPage);
		}

		public void Page_Init()
		{
			LightingPage.TopLevel = false;
			LightingPage.FormBorderStyle = FormBorderStyle.None;
			LightingPage.SetCBFunc(SetCfgCBFuncCB, GetCfgCBFuncCB, SendCfgDataCBFuncCB);
			Tp_Lighting.Controls.Add(LightingPage);
			LightingPage.Show();
			ImageProcessingPage.TopLevel = false;
			ImageProcessingPage.FormBorderStyle = FormBorderStyle.None;
			ImageProcessingPage.SetCBFunc(SetCfgCBFuncCB, GetCfgCBFuncCB, SendCfgDataCBFuncCB);
			Tp_ImageProcess.Controls.Add(ImageProcessingPage);
			ImageProcessingPage.Show();
			BarcodeTypePage.TopLevel = false;
			BarcodeTypePage.FormBorderStyle = FormBorderStyle.None;
			BarcodeTypePage.SetCBFunc(SetCfgCBFuncCB, GetCfgCBFuncCB, SendCfgDataCBFuncCB);
			Tp_BarcodeType.Controls.Add(BarcodeTypePage);
			BarcodeTypePage.Page_Init();
			BarcodeTypePage.Show();
			TemplateSettingPage.TopLevel = false;
			TemplateSettingPage.FormBorderStyle = FormBorderStyle.None;
			Tp_Template.Controls.Add(TemplateSettingPage);
			TemplateSettingPage.Show();
		}

		private void DeviceSettingForm_Load(object sender, EventArgs e)
		{
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			if (GetCfgCBFuncCB(3u) == 2)
			{
				RdbContinuousTrig.Checked = true;
				GrbTrigTime.Enabled = false;
			}
			else
			{
				RdbTrigExtern.Checked = true;
				GrbTrigTime.Enabled = true;
			}
			ToolCfg.is_RdbTrigExtern_checked = RdbTrigExtern.Checked;
			if (GetCfgCBFuncCB(3588u) == 4)
			{
				RdbSoundOn.Checked = true;
			}
			else
			{
				RdbSoundOff.Checked = true;
			}
			TxbTrigKeepTime.Text = (GetCfgCBFuncCB(1791u) * 100).ToString();
			if (para.ParaDataLen >= 4096)
			{
				BtnExpSetValue.Enabled = true;
				TxbExpSetValue.Enabled = true;
				RdbExpSetValue.Enabled = true;
				GrbSensorLensFocus.Enabled = true;
			}
			else
			{
				BtnExpSetValue.Enabled = false;
				TxbExpSetValue.Enabled = false;
				RdbExpSetValue.Enabled = false;
				GrbSensorLensFocus.Enabled = false;
			}
			if (para.DeviceTypeRecord >= 6)
			{
				ChbHDR_ModeEnable.Checked = GetCfgCBFuncCB(75523u) == 2;
			}
			else
			{
				ChbHDR_ModeEnable.Visible = false;
				TrbHDR_LightExp.Visible = false;
			}
			if (GetCfgCBFuncCB(4607u) == 0)
			{
				CheckAutoExp.Checked = true;
			}
			if (GetCfgCBFuncCB(7296u) == 128 && para.ParaDataLen >= 4096)
			{
				RdbExpSetValue.Checked = true;
				TxbExpSetValue.Text = (GetCfgCBFuncCB(7295u) * 256 + GetCfgCBFuncCB(7167u)).ToString();
			}
			else
			{
				RdbExpSetRatio.Checked = true;
			}
			TxbExpSetRatio.Text = GetCfgCBFuncCB(4607u).ToString();
			TxbSensorGainSet.Text = GetCfgCBFuncCB(51455u).ToString();
			LightingPage.UpdateUiDisplay(para);
			ImageProcessingPage.UpdateUiDisplay(para);
			BarcodeTypePage.UpdateUiDisplay(para);
		}

		public void FunctionOnOff(bool[] CapacityArr, string DeviceName)
		{
			GrbSoundSetting.Enabled = CapacityArr[1];
			LightingPage.FunctionOnOff(CapacityArr, DeviceName);
			BarcodeTypePage.FunctionOnOff(CapacityArr);
			TemplateSettingPage.FunctionOnOff(CapacityArr);
		}

		public void RdbContinuousTrig_Click(object sender, EventArgs e)
		{
			RdbContinuousTrig.Checked = true;
			SetCfgCBFuncCB(3u, 2u);
			SendCfgDataCBFuncCB(64u);
			GrbTrigTime.Enabled = false;
			LastTrigIsTrigMode = false;
		}

		public void RdbTrigExtern_Click(object sender, EventArgs e)
		{
			RdbTrigExtern.Checked = true;
			if (!LastTrigIsTrigMode)
			{
				LastTrigIsTrigMode = true;
				SetCfgCBFuncCB(3u, 0u);
				SendCfgDataCBFuncCB(64u);
				GrbTrigTime.Enabled = true;
			}
		}

		private void RdbContinuousTrig_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbContinuousTrig.Checked)
			{
				RdbContinuousTrig_Click(null, null);
			}
		}

		private void RdbTrigExtern_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbTrigExtern.Checked)
			{
				RdbTrigExtern_Click(null, null);
			}
			ToolCfg.is_RdbTrigExtern_checked = RdbTrigExtern.Checked;
		}

		public void BtnTrigTest_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
			{
				byte[] dat = new byte[9] { 126, 0, 8, 1, 0, 2, 1, 171, 205 };
				ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, dat, 9);
			}
		}

		private void RdbKeepTime_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbKeepTime.Checked)
			{
				TxbKeepImage.Text = "0";
				SetCfgCBFuncCB(6655u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbTrigKeepTime_TextChanged(object sender, EventArgs e)
		{
			BtnTrigKeepTime.Visible = true;
		}

		private void BtnTrigKeepTime_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result;
				if (ushort.TryParse(TxbTrigKeepTime.Text, out result) && result < 25600)
				{
					result /= 100;
					SetCfgCBFuncCB(1791u, result);
					SendCfgDataCBFuncCB(0u);
					BtnTrigKeepTime.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的时间,小于25600", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct time value,make it < 25600", "Error");
				}
			}
		}

		private void RdbKeepImage_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbKeepImage.Checked)
			{
				TxbKeepImage.Text = "2";
				SetCfgCBFuncCB(6655u, 2u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbKeepImage_TextChanged(object sender, EventArgs e)
		{
			BtnKeepImage.Visible = true;
		}

		private void BtnKeepImage_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbKeepImage.Text, out result) && result < 255)
				{
					SetCfgCBFuncCB(6655u, (byte)(result & 0xFFu));
					SendCfgDataCBFuncCB(0u);
					BtnKeepImage.Visible = false;
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

		private void CheckAutoExp_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				anti_re_in = true;
				if (CheckAutoExp.Checked)
				{
					SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetExposure, 0, "");
					ExpRatioVal = 0;
				}
				else
				{
					ExpRatioVal = 1;
				}
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetExposure, ExpRatioVal, "");
				SetCfgCBFuncCB(4607u, (ushort)ExpRatioVal);
				SendCfgDataCBFuncCB(128u);
			}
		}

		private void RdbExpSetRatio_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbExpSetRatio.Checked)
			{
				SetCfgCBFuncCB(7296u, 0u);
				SendCfgDataCBFuncCB(128u);
			}
		}

		private void BtnExpSetRatioDec_Click(object sender, EventArgs e)
		{
			if (ExpRatioVal > 0)
			{
				ExpRatioVal--;
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetExposure, ExpRatioVal, "");
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(4607u, (ushort)ExpRatioVal);
					SendCfgDataCBFuncCB(128u);
				}
			}
		}

		private void BtnExpSetRatioAdd_Click(object sender, EventArgs e)
		{
			if (ExpRatioVal < 255)
			{
				ExpRatioVal++;
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetExposure, ExpRatioVal, "");
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(4607u, (ushort)ExpRatioVal);
					SendCfgDataCBFuncCB(128u);
				}
			}
		}

		private void RdbExpSetValue_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbExpSetValue.Checked)
			{
				SetCfgCBFuncCB(7296u, 128u);
				SendCfgDataCBFuncCB(128u);
			}
		}

		private void TxbExpSetValue_TextChanged(object sender, EventArgs e)
		{
			BtnExpSetValue.Visible = true;
		}

		private void BtnExpSetValue_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbExpSetValue.Checked)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbExpSetValue.Text, out result) && result < ushort.MaxValue)
				{
					SetCfgCBFuncCB(7295u, (byte)((result & 0xFF00) >> 8));
					SetCfgCBFuncCB(7167u, (byte)(result & 0xFFu));
					SendCfgDataCBFuncCB(0u);
					BtnExpSetValue.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入小于65536的数", "错误");
				}
				else
				{
					MessageBox.Show("Please enter a number less than 65536", "Error");
				}
				SendCfgDataCBFuncCB(128u);
			}
		}

		private void BtnSensorGainSetDec_Click(object sender, EventArgs e)
		{
			if (SenGainVal > 0)
			{
				SenGainVal--;
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetGain, SenGainVal, "");
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(51455u, (ushort)SenGainVal);
					SendCfgDataCBFuncCB(128u);
				}
			}
		}

		private void BtnSensorGainSetAdd_Click(object sender, EventArgs e)
		{
			if (SenGainVal < 255)
			{
				SenGainVal++;
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetGain, SenGainVal, "");
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(51455u, (ushort)SenGainVal);
					SendCfgDataCBFuncCB(128u);
				}
			}
		}

		private void BtnSensorLensFocusDec_Click(object sender, EventArgs e)
		{
			if (FocusVal > 0)
			{
				FocusVal--;
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetFocus, FocusVal, "");
				if (!ToolCfg.UpdateAdjState)
				{
					int focusVal = FocusVal;
					SetCfgCBFuncCB(74495u, (byte)((uint)focusVal & 0xFFu));
					SetCfgCBFuncCB(74751u, (byte)((focusVal & 0xFF00) >> 8));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void BtnSensorLensFocusAdd_Click(object sender, EventArgs e)
		{
			if (FocusVal < 1008)
			{
				FocusVal++;
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetFocus, FocusVal, "");
				if (!ToolCfg.UpdateAdjState)
				{
					int focusVal = FocusVal;
					SetCfgCBFuncCB(74495u, (byte)((uint)focusVal & 0xFFu));
					SetCfgCBFuncCB(74751u, (byte)((focusVal & 0xFF00) >> 8));
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void RdbSoundOn_CheckedChanged(object sender, EventArgs e)
		{
			if (RdbSoundOn.Checked && !ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(3588u, 4u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbSoundOff_CheckedChanged(object sender, EventArgs e)
		{
			if (RdbSoundOff.Checked && !ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(3588u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbHDR_ModeEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbHDR_ModeEnable.Checked)
				{
					SetCfgCBFuncCB(75523u, 2u);
				}
				else
				{
					SetCfgCBFuncCB(75523u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TrbHDR_LightExp_MouseUp(object sender, MouseEventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(77055u, (uint)TrbHDR_LightExp.Value);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TrbHDR_LightExp_Scroll(object sender, EventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			Hide();
		}

		private void InitializeComponent()
		{
			this.GrbWorkMode = new System.Windows.Forms.GroupBox();
			this.GrbTrigTime = new System.Windows.Forms.GroupBox();
			this.RdbKeepImage = new System.Windows.Forms.RadioButton();
			this.RdbKeepTime = new System.Windows.Forms.RadioButton();
			this.BtnKeepImage = new System.Windows.Forms.Button();
			this.LabKeepImage = new System.Windows.Forms.Label();
			this.TxbKeepImage = new System.Windows.Forms.TextBox();
			this.LabKeepImageUnit = new System.Windows.Forms.Label();
			this.BtnTrigKeepTime = new System.Windows.Forms.Button();
			this.LabTrigKeepTimeName = new System.Windows.Forms.Label();
			this.TxbTrigKeepTime = new System.Windows.Forms.TextBox();
			this.LabMsSShow = new System.Windows.Forms.Label();
			this.BtnTrigTest = new System.Windows.Forms.Button();
			this.RdbTrigExtern = new System.Windows.Forms.RadioButton();
			this.RdbContinuousTrig = new System.Windows.Forms.RadioButton();
			this.GrbExpSetting = new System.Windows.Forms.GroupBox();
			this.BtnExpSetValue = new System.Windows.Forms.Button();
			this.TxbExpSetValue = new System.Windows.Forms.TextBox();
			this.BtnExpSetRatioAdd = new System.Windows.Forms.Button();
			this.BtnExpSetRatioDec = new System.Windows.Forms.Button();
			this.CheckAutoExp = new System.Windows.Forms.CheckBox();
			this.TxbExpSetRatio = new System.Windows.Forms.TextBox();
			this.RdbExpSetValue = new System.Windows.Forms.RadioButton();
			this.RdbExpSetRatio = new System.Windows.Forms.RadioButton();
			this.GrbSensorGainSetting = new System.Windows.Forms.GroupBox();
			this.LabSensorGainSet = new System.Windows.Forms.Label();
			this.BtnSensorGainSetAdd = new System.Windows.Forms.Button();
			this.BtnSensorGainSetDec = new System.Windows.Forms.Button();
			this.TxbSensorGainSet = new System.Windows.Forms.TextBox();
			this.TabSettings = new System.Windows.Forms.TabControl();
			this.TpParaAdjust = new System.Windows.Forms.TabPage();
			this.PanHdr = new System.Windows.Forms.Panel();
			this.ChbHDR_ModeEnable = new System.Windows.Forms.CheckBox();
			this.TrbHDR_LightExp = new System.Windows.Forms.TrackBar();
			this.GrbSoundSetting = new System.Windows.Forms.GroupBox();
			this.RdbSoundOff = new System.Windows.Forms.RadioButton();
			this.RdbSoundOn = new System.Windows.Forms.RadioButton();
			this.GrbSensorLensFocus = new System.Windows.Forms.GroupBox();
			this.BtnSensorLensFocusAdd = new System.Windows.Forms.Button();
			this.BtnSensorLensFocusDec = new System.Windows.Forms.Button();
			this.TxbSensorLensFocusSet = new System.Windows.Forms.TextBox();
			this.LabSensorLensFocusSet = new System.Windows.Forms.Label();
			this.RdbAutoFocusEnbale = new System.Windows.Forms.RadioButton();
			this.RdbAutoFocusDisabale = new System.Windows.Forms.RadioButton();
			this.Tp_Lighting = new System.Windows.Forms.TabPage();
			this.Tp_ImageProcess = new System.Windows.Forms.TabPage();
			this.Tp_BarcodeType = new System.Windows.Forms.TabPage();
			this.Tp_Template = new System.Windows.Forms.TabPage();
			this.GrbWorkMode.SuspendLayout();
			this.GrbTrigTime.SuspendLayout();
			this.GrbExpSetting.SuspendLayout();
			this.GrbSensorGainSetting.SuspendLayout();
			this.TabSettings.SuspendLayout();
			this.TpParaAdjust.SuspendLayout();
			this.PanHdr.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbHDR_LightExp).BeginInit();
			this.GrbSoundSetting.SuspendLayout();
			this.GrbSensorLensFocus.SuspendLayout();
			base.SuspendLayout();
			this.GrbWorkMode.BackColor = System.Drawing.Color.Transparent;
			this.GrbWorkMode.Controls.Add(this.GrbTrigTime);
			this.GrbWorkMode.Controls.Add(this.BtnTrigTest);
			this.GrbWorkMode.Controls.Add(this.RdbTrigExtern);
			this.GrbWorkMode.Controls.Add(this.RdbContinuousTrig);
			this.GrbWorkMode.Location = new System.Drawing.Point(3, 7);
			this.GrbWorkMode.Name = "GrbWorkMode";
			this.GrbWorkMode.Size = new System.Drawing.Size(400, 136);
			this.GrbWorkMode.TabIndex = 11;
			this.GrbWorkMode.TabStop = false;
			this.GrbWorkMode.Text = "工作模式";
			this.GrbTrigTime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbTrigTime.Controls.Add(this.RdbKeepImage);
			this.GrbTrigTime.Controls.Add(this.RdbKeepTime);
			this.GrbTrigTime.Controls.Add(this.BtnKeepImage);
			this.GrbTrigTime.Controls.Add(this.LabKeepImage);
			this.GrbTrigTime.Controls.Add(this.TxbKeepImage);
			this.GrbTrigTime.Controls.Add(this.LabKeepImageUnit);
			this.GrbTrigTime.Controls.Add(this.BtnTrigKeepTime);
			this.GrbTrigTime.Controls.Add(this.LabTrigKeepTimeName);
			this.GrbTrigTime.Controls.Add(this.TxbTrigKeepTime);
			this.GrbTrigTime.Controls.Add(this.LabMsSShow);
			this.GrbTrigTime.Location = new System.Drawing.Point(11, 47);
			this.GrbTrigTime.Name = "GrbTrigTime";
			this.GrbTrigTime.Size = new System.Drawing.Size(383, 76);
			this.GrbTrigTime.TabIndex = 58;
			this.GrbTrigTime.TabStop = false;
			this.GrbTrigTime.Text = "触发读码设置";
			this.RdbKeepImage.AutoSize = true;
			this.RdbKeepImage.Location = new System.Drawing.Point(7, 49);
			this.RdbKeepImage.Name = "RdbKeepImage";
			this.RdbKeepImage.Size = new System.Drawing.Size(81, 18);
			this.RdbKeepImage.TabIndex = 55;
			this.RdbKeepImage.Text = "按图个数";
			this.RdbKeepImage.UseVisualStyleBackColor = true;
			this.RdbKeepImage.CheckedChanged += new System.EventHandler(RdbKeepImage_CheckedChanged);
			this.RdbKeepTime.AutoSize = true;
			this.RdbKeepTime.Checked = true;
			this.RdbKeepTime.Location = new System.Drawing.Point(7, 23);
			this.RdbKeepTime.Name = "RdbKeepTime";
			this.RdbKeepTime.Size = new System.Drawing.Size(67, 18);
			this.RdbKeepTime.TabIndex = 13;
			this.RdbKeepTime.TabStop = true;
			this.RdbKeepTime.Text = "按时长";
			this.RdbKeepTime.UseVisualStyleBackColor = true;
			this.RdbKeepTime.CheckedChanged += new System.EventHandler(RdbKeepTime_CheckedChanged);
			this.BtnKeepImage.Location = new System.Drawing.Point(307, 48);
			this.BtnKeepImage.Name = "BtnKeepImage";
			this.BtnKeepImage.Size = new System.Drawing.Size(70, 23);
			this.BtnKeepImage.TabIndex = 53;
			this.BtnKeepImage.Text = "确认";
			this.BtnKeepImage.UseVisualStyleBackColor = true;
			this.BtnKeepImage.Visible = false;
			this.BtnKeepImage.Click += new System.EventHandler(BtnKeepImage_Click);
			this.LabKeepImage.AutoSize = true;
			this.LabKeepImage.Location = new System.Drawing.Point(128, 51);
			this.LabKeepImage.Name = "LabKeepImage";
			this.LabKeepImage.Size = new System.Drawing.Size(98, 14);
			this.LabKeepImage.TabIndex = 51;
			this.LabKeepImage.Text = "触发图片张数:";
			this.TxbKeepImage.Location = new System.Drawing.Point(235, 48);
			this.TxbKeepImage.Name = "TxbKeepImage";
			this.TxbKeepImage.Size = new System.Drawing.Size(47, 23);
			this.TxbKeepImage.TabIndex = 52;
			this.TxbKeepImage.Text = "2";
			this.TxbKeepImage.TextChanged += new System.EventHandler(TxbKeepImage_TextChanged);
			this.LabKeepImageUnit.AutoSize = true;
			this.LabKeepImageUnit.Location = new System.Drawing.Point(283, 51);
			this.LabKeepImageUnit.Name = "LabKeepImageUnit";
			this.LabKeepImageUnit.Size = new System.Drawing.Size(21, 14);
			this.LabKeepImageUnit.TabIndex = 54;
			this.LabKeepImageUnit.Text = "张";
			this.BtnTrigKeepTime.Location = new System.Drawing.Point(307, 22);
			this.BtnTrigKeepTime.Name = "BtnTrigKeepTime";
			this.BtnTrigKeepTime.Size = new System.Drawing.Size(70, 23);
			this.BtnTrigKeepTime.TabIndex = 49;
			this.BtnTrigKeepTime.Text = "确认";
			this.BtnTrigKeepTime.UseVisualStyleBackColor = true;
			this.BtnTrigKeepTime.Visible = false;
			this.BtnTrigKeepTime.Click += new System.EventHandler(BtnTrigKeepTime_Click);
			this.LabTrigKeepTimeName.AutoSize = true;
			this.LabTrigKeepTimeName.Location = new System.Drawing.Point(128, 25);
			this.LabTrigKeepTimeName.Name = "LabTrigKeepTimeName";
			this.LabTrigKeepTimeName.Size = new System.Drawing.Size(98, 14);
			this.LabTrigKeepTimeName.TabIndex = 47;
			this.LabTrigKeepTimeName.Text = "单次读码时长:";
			this.TxbTrigKeepTime.Location = new System.Drawing.Point(235, 22);
			this.TxbTrigKeepTime.Name = "TxbTrigKeepTime";
			this.TxbTrigKeepTime.Size = new System.Drawing.Size(47, 23);
			this.TxbTrigKeepTime.TabIndex = 48;
			this.TxbTrigKeepTime.Text = "2000";
			this.TxbTrigKeepTime.TextChanged += new System.EventHandler(TxbTrigKeepTime_TextChanged);
			this.LabMsSShow.AutoSize = true;
			this.LabMsSShow.Location = new System.Drawing.Point(283, 25);
			this.LabMsSShow.Name = "LabMsSShow";
			this.LabMsSShow.Size = new System.Drawing.Size(21, 14);
			this.LabMsSShow.TabIndex = 50;
			this.LabMsSShow.Text = "ms";
			this.BtnTrigTest.FlatAppearance.BorderSize = 0;
			this.BtnTrigTest.Location = new System.Drawing.Point(287, 20);
			this.BtnTrigTest.Name = "BtnTrigTest";
			this.BtnTrigTest.Size = new System.Drawing.Size(92, 27);
			this.BtnTrigTest.TabIndex = 8;
			this.BtnTrigTest.Text = "单次触发";
			this.BtnTrigTest.UseVisualStyleBackColor = true;
			this.BtnTrigTest.Click += new System.EventHandler(BtnTrigTest_Click);
			this.RdbTrigExtern.AutoSize = true;
			this.RdbTrigExtern.Checked = true;
			this.RdbTrigExtern.Location = new System.Drawing.Point(134, 23);
			this.RdbTrigExtern.Name = "RdbTrigExtern";
			this.RdbTrigExtern.Size = new System.Drawing.Size(144, 18);
			this.RdbTrigExtern.TabIndex = 7;
			this.RdbTrigExtern.TabStop = true;
			this.RdbTrigExtern.Text = "外部触发/命令触发";
			this.RdbTrigExtern.UseVisualStyleBackColor = true;
			this.RdbTrigExtern.CheckedChanged += new System.EventHandler(RdbTrigExtern_CheckedChanged);
			this.RdbTrigExtern.Click += new System.EventHandler(RdbTrigExtern_Click);
			this.RdbContinuousTrig.AutoSize = true;
			this.RdbContinuousTrig.Location = new System.Drawing.Point(33, 23);
			this.RdbContinuousTrig.Name = "RdbContinuousTrig";
			this.RdbContinuousTrig.Size = new System.Drawing.Size(81, 18);
			this.RdbContinuousTrig.TabIndex = 6;
			this.RdbContinuousTrig.Text = "连续触发";
			this.RdbContinuousTrig.UseVisualStyleBackColor = true;
			this.RdbContinuousTrig.CheckedChanged += new System.EventHandler(RdbContinuousTrig_CheckedChanged);
			this.RdbContinuousTrig.Click += new System.EventHandler(RdbContinuousTrig_Click);
			this.GrbExpSetting.BackColor = System.Drawing.Color.Transparent;
			this.GrbExpSetting.Controls.Add(this.BtnExpSetValue);
			this.GrbExpSetting.Controls.Add(this.TxbExpSetValue);
			this.GrbExpSetting.Controls.Add(this.BtnExpSetRatioAdd);
			this.GrbExpSetting.Controls.Add(this.BtnExpSetRatioDec);
			this.GrbExpSetting.Controls.Add(this.CheckAutoExp);
			this.GrbExpSetting.Controls.Add(this.TxbExpSetRatio);
			this.GrbExpSetting.Controls.Add(this.RdbExpSetValue);
			this.GrbExpSetting.Controls.Add(this.RdbExpSetRatio);
			this.GrbExpSetting.Location = new System.Drawing.Point(3, 149);
			this.GrbExpSetting.Name = "GrbExpSetting";
			this.GrbExpSetting.Size = new System.Drawing.Size(400, 118);
			this.GrbExpSetting.TabIndex = 13;
			this.GrbExpSetting.TabStop = false;
			this.GrbExpSetting.Text = "曝光控制";
			this.BtnExpSetValue.Location = new System.Drawing.Point(274, 80);
			this.BtnExpSetValue.Name = "BtnExpSetValue";
			this.BtnExpSetValue.Size = new System.Drawing.Size(83, 27);
			this.BtnExpSetValue.TabIndex = 33;
			this.BtnExpSetValue.Text = "设置曝光";
			this.BtnExpSetValue.UseVisualStyleBackColor = true;
			this.BtnExpSetValue.Click += new System.EventHandler(BtnExpSetValue_Click);
			this.TxbExpSetValue.Location = new System.Drawing.Point(177, 80);
			this.TxbExpSetValue.Name = "TxbExpSetValue";
			this.TxbExpSetValue.Size = new System.Drawing.Size(88, 23);
			this.TxbExpSetValue.TabIndex = 32;
			this.TxbExpSetValue.TextChanged += new System.EventHandler(TxbExpSetValue_TextChanged);
			this.BtnExpSetRatioAdd.Location = new System.Drawing.Point(316, 49);
			this.BtnExpSetRatioAdd.Name = "BtnExpSetRatioAdd";
			this.BtnExpSetRatioAdd.Size = new System.Drawing.Size(30, 27);
			this.BtnExpSetRatioAdd.TabIndex = 31;
			this.BtnExpSetRatioAdd.TabStop = false;
			this.BtnExpSetRatioAdd.Text = "▶";
			this.BtnExpSetRatioAdd.UseVisualStyleBackColor = true;
			this.BtnExpSetRatioAdd.Click += new System.EventHandler(BtnExpSetRatioAdd_Click);
			this.BtnExpSetRatioDec.Location = new System.Drawing.Point(275, 49);
			this.BtnExpSetRatioDec.Name = "BtnExpSetRatioDec";
			this.BtnExpSetRatioDec.Size = new System.Drawing.Size(31, 27);
			this.BtnExpSetRatioDec.TabIndex = 30;
			this.BtnExpSetRatioDec.TabStop = false;
			this.BtnExpSetRatioDec.Text = "◀";
			this.BtnExpSetRatioDec.UseVisualStyleBackColor = true;
			this.BtnExpSetRatioDec.Click += new System.EventHandler(BtnExpSetRatioDec_Click);
			this.CheckAutoExp.AutoSize = true;
			this.CheckAutoExp.Location = new System.Drawing.Point(33, 23);
			this.CheckAutoExp.Name = "CheckAutoExp";
			this.CheckAutoExp.Size = new System.Drawing.Size(82, 18);
			this.CheckAutoExp.TabIndex = 7;
			this.CheckAutoExp.Text = "自动曝光";
			this.CheckAutoExp.UseVisualStyleBackColor = true;
			this.CheckAutoExp.CheckedChanged += new System.EventHandler(CheckAutoExp_CheckedChanged);
			this.TxbExpSetRatio.Location = new System.Drawing.Point(177, 49);
			this.TxbExpSetRatio.Name = "TxbExpSetRatio";
			this.TxbExpSetRatio.ReadOnly = true;
			this.TxbExpSetRatio.Size = new System.Drawing.Size(88, 23);
			this.TxbExpSetRatio.TabIndex = 6;
			this.RdbExpSetValue.AutoSize = true;
			this.RdbExpSetValue.Location = new System.Drawing.Point(31, 83);
			this.RdbExpSetValue.Name = "RdbExpSetValue";
			this.RdbExpSetValue.Size = new System.Drawing.Size(144, 18);
			this.RdbExpSetValue.TabIndex = 1;
			this.RdbExpSetValue.TabStop = true;
			this.RdbExpSetValue.Text = "数值调节(单位us):";
			this.RdbExpSetValue.UseVisualStyleBackColor = true;
			this.RdbExpSetValue.CheckedChanged += new System.EventHandler(RdbExpSetValue_CheckedChanged);
			this.RdbExpSetRatio.AutoSize = true;
			this.RdbExpSetRatio.Location = new System.Drawing.Point(31, 51);
			this.RdbExpSetRatio.Name = "RdbExpSetRatio";
			this.RdbExpSetRatio.Size = new System.Drawing.Size(137, 18);
			this.RdbExpSetRatio.TabIndex = 0;
			this.RdbExpSetRatio.TabStop = true;
			this.RdbExpSetRatio.Text = "比例调节(1-255):";
			this.RdbExpSetRatio.UseVisualStyleBackColor = true;
			this.RdbExpSetRatio.CheckedChanged += new System.EventHandler(RdbExpSetRatio_CheckedChanged);
			this.GrbSensorGainSetting.BackColor = System.Drawing.Color.Transparent;
			this.GrbSensorGainSetting.Controls.Add(this.LabSensorGainSet);
			this.GrbSensorGainSetting.Controls.Add(this.BtnSensorGainSetAdd);
			this.GrbSensorGainSetting.Controls.Add(this.BtnSensorGainSetDec);
			this.GrbSensorGainSetting.Controls.Add(this.TxbSensorGainSet);
			this.GrbSensorGainSetting.Location = new System.Drawing.Point(3, 274);
			this.GrbSensorGainSetting.Name = "GrbSensorGainSetting";
			this.GrbSensorGainSetting.Size = new System.Drawing.Size(400, 62);
			this.GrbSensorGainSetting.TabIndex = 35;
			this.GrbSensorGainSetting.TabStop = false;
			this.GrbSensorGainSetting.Text = "增益控制";
			this.LabSensorGainSet.AutoSize = true;
			this.LabSensorGainSet.Location = new System.Drawing.Point(30, 30);
			this.LabSensorGainSet.Name = "LabSensorGainSet";
			this.LabSensorGainSet.Size = new System.Drawing.Size(133, 14);
			this.LabSensorGainSet.TabIndex = 32;
			this.LabSensorGainSet.Text = "传感器增益(0-255):";
			this.BtnSensorGainSetAdd.Location = new System.Drawing.Point(320, 24);
			this.BtnSensorGainSetAdd.Name = "BtnSensorGainSetAdd";
			this.BtnSensorGainSetAdd.Size = new System.Drawing.Size(30, 27);
			this.BtnSensorGainSetAdd.TabIndex = 31;
			this.BtnSensorGainSetAdd.TabStop = false;
			this.BtnSensorGainSetAdd.Text = "▶";
			this.BtnSensorGainSetAdd.UseVisualStyleBackColor = true;
			this.BtnSensorGainSetAdd.Click += new System.EventHandler(BtnSensorGainSetAdd_Click);
			this.BtnSensorGainSetDec.Location = new System.Drawing.Point(276, 24);
			this.BtnSensorGainSetDec.Name = "BtnSensorGainSetDec";
			this.BtnSensorGainSetDec.Size = new System.Drawing.Size(31, 27);
			this.BtnSensorGainSetDec.TabIndex = 30;
			this.BtnSensorGainSetDec.TabStop = false;
			this.BtnSensorGainSetDec.Text = "◀";
			this.BtnSensorGainSetDec.UseVisualStyleBackColor = true;
			this.BtnSensorGainSetDec.Click += new System.EventHandler(BtnSensorGainSetDec_Click);
			this.TxbSensorGainSet.Location = new System.Drawing.Point(169, 26);
			this.TxbSensorGainSet.Name = "TxbSensorGainSet";
			this.TxbSensorGainSet.ReadOnly = true;
			this.TxbSensorGainSet.Size = new System.Drawing.Size(81, 23);
			this.TxbSensorGainSet.TabIndex = 6;
			this.TxbSensorGainSet.Text = "0";
			this.TabSettings.Controls.Add(this.TpParaAdjust);
			this.TabSettings.Controls.Add(this.Tp_Lighting);
			this.TabSettings.Controls.Add(this.Tp_ImageProcess);
			this.TabSettings.Controls.Add(this.Tp_BarcodeType);
			this.TabSettings.Controls.Add(this.Tp_Template);
			this.TabSettings.ItemSize = new System.Drawing.Size(54, 30);
			this.TabSettings.Location = new System.Drawing.Point(1, 0);
			this.TabSettings.Name = "TabSettings";
			this.TabSettings.SelectedIndex = 0;
			this.TabSettings.Size = new System.Drawing.Size(470, 691);
			this.TabSettings.TabIndex = 43;
			this.TpParaAdjust.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TpParaAdjust.Controls.Add(this.PanHdr);
			this.TpParaAdjust.Controls.Add(this.GrbSoundSetting);
			this.TpParaAdjust.Controls.Add(this.GrbSensorLensFocus);
			this.TpParaAdjust.Controls.Add(this.GrbWorkMode);
			this.TpParaAdjust.Controls.Add(this.GrbExpSetting);
			this.TpParaAdjust.Controls.Add(this.GrbSensorGainSetting);
			this.TpParaAdjust.Location = new System.Drawing.Point(4, 34);
			this.TpParaAdjust.Name = "TpParaAdjust";
			this.TpParaAdjust.Padding = new System.Windows.Forms.Padding(3);
			this.TpParaAdjust.Size = new System.Drawing.Size(462, 653);
			this.TpParaAdjust.TabIndex = 1;
			this.TpParaAdjust.Text = "调整";
			this.TpParaAdjust.UseVisualStyleBackColor = true;
			this.PanHdr.Controls.Add(this.ChbHDR_ModeEnable);
			this.PanHdr.Controls.Add(this.TrbHDR_LightExp);
			this.PanHdr.Location = new System.Drawing.Point(3, 406);
			this.PanHdr.Name = "PanHdr";
			this.PanHdr.Size = new System.Drawing.Size(400, 38);
			this.PanHdr.TabIndex = 45;
			this.ChbHDR_ModeEnable.AutoSize = true;
			this.ChbHDR_ModeEnable.Location = new System.Drawing.Point(11, 10);
			this.ChbHDR_ModeEnable.Name = "ChbHDR_ModeEnable";
			this.ChbHDR_ModeEnable.Size = new System.Drawing.Size(75, 18);
			this.ChbHDR_ModeEnable.TabIndex = 39;
			this.ChbHDR_ModeEnable.Text = "HDR开启";
			this.ChbHDR_ModeEnable.UseVisualStyleBackColor = true;
			this.ChbHDR_ModeEnable.Visible = false;
			this.ChbHDR_ModeEnable.CheckedChanged += new System.EventHandler(ChbHDR_ModeEnable_CheckedChanged);
			this.TrbHDR_LightExp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TrbHDR_LightExp.AutoSize = false;
			this.TrbHDR_LightExp.BackColor = System.Drawing.Color.White;
			this.TrbHDR_LightExp.Location = new System.Drawing.Point(101, 7);
			this.TrbHDR_LightExp.Maximum = 255;
			this.TrbHDR_LightExp.Name = "TrbHDR_LightExp";
			this.TrbHDR_LightExp.Size = new System.Drawing.Size(266, 28);
			this.TrbHDR_LightExp.TabIndex = 38;
			this.TrbHDR_LightExp.TickStyle = System.Windows.Forms.TickStyle.None;
			this.TrbHDR_LightExp.Visible = false;
			this.TrbHDR_LightExp.Scroll += new System.EventHandler(TrbHDR_LightExp_Scroll);
			this.TrbHDR_LightExp.MouseUp += new System.Windows.Forms.MouseEventHandler(TrbHDR_LightExp_MouseUp);
			this.GrbSoundSetting.Controls.Add(this.RdbSoundOff);
			this.GrbSoundSetting.Controls.Add(this.RdbSoundOn);
			this.GrbSoundSetting.Location = new System.Drawing.Point(3, 342);
			this.GrbSoundSetting.Name = "GrbSoundSetting";
			this.GrbSoundSetting.Size = new System.Drawing.Size(400, 58);
			this.GrbSoundSetting.TabIndex = 38;
			this.GrbSoundSetting.TabStop = false;
			this.GrbSoundSetting.Text = "读码成功声音提示";
			this.RdbSoundOff.AutoSize = true;
			this.RdbSoundOff.Location = new System.Drawing.Point(100, 28);
			this.RdbSoundOff.Name = "RdbSoundOff";
			this.RdbSoundOff.Size = new System.Drawing.Size(53, 18);
			this.RdbSoundOff.TabIndex = 3;
			this.RdbSoundOff.TabStop = true;
			this.RdbSoundOff.Text = "关闭";
			this.RdbSoundOff.UseVisualStyleBackColor = true;
			this.RdbSoundOff.CheckedChanged += new System.EventHandler(RdbSoundOff_CheckedChanged);
			this.RdbSoundOn.AutoSize = true;
			this.RdbSoundOn.Location = new System.Drawing.Point(33, 28);
			this.RdbSoundOn.Name = "RdbSoundOn";
			this.RdbSoundOn.Size = new System.Drawing.Size(53, 18);
			this.RdbSoundOn.TabIndex = 0;
			this.RdbSoundOn.TabStop = true;
			this.RdbSoundOn.Text = "开启";
			this.RdbSoundOn.UseVisualStyleBackColor = true;
			this.RdbSoundOn.CheckedChanged += new System.EventHandler(RdbSoundOn_CheckedChanged);
			this.GrbSensorLensFocus.Controls.Add(this.BtnSensorLensFocusAdd);
			this.GrbSensorLensFocus.Controls.Add(this.BtnSensorLensFocusDec);
			this.GrbSensorLensFocus.Controls.Add(this.TxbSensorLensFocusSet);
			this.GrbSensorLensFocus.Controls.Add(this.LabSensorLensFocusSet);
			this.GrbSensorLensFocus.Controls.Add(this.RdbAutoFocusEnbale);
			this.GrbSensorLensFocus.Controls.Add(this.RdbAutoFocusDisabale);
			this.GrbSensorLensFocus.Enabled = false;
			this.GrbSensorLensFocus.Location = new System.Drawing.Point(3, 450);
			this.GrbSensorLensFocus.Name = "GrbSensorLensFocus";
			this.GrbSensorLensFocus.Size = new System.Drawing.Size(400, 65);
			this.GrbSensorLensFocus.TabIndex = 37;
			this.GrbSensorLensFocus.TabStop = false;
			this.GrbSensorLensFocus.Text = "镜头对焦设置";
			this.GrbSensorLensFocus.Visible = false;
			this.BtnSensorLensFocusAdd.Location = new System.Drawing.Point(332, 26);
			this.BtnSensorLensFocusAdd.Name = "BtnSensorLensFocusAdd";
			this.BtnSensorLensFocusAdd.Size = new System.Drawing.Size(28, 27);
			this.BtnSensorLensFocusAdd.TabIndex = 29;
			this.BtnSensorLensFocusAdd.TabStop = false;
			this.BtnSensorLensFocusAdd.Text = "▶";
			this.BtnSensorLensFocusAdd.UseVisualStyleBackColor = true;
			this.BtnSensorLensFocusAdd.Click += new System.EventHandler(BtnSensorLensFocusAdd_Click);
			this.BtnSensorLensFocusDec.Location = new System.Drawing.Point(295, 26);
			this.BtnSensorLensFocusDec.Name = "BtnSensorLensFocusDec";
			this.BtnSensorLensFocusDec.Size = new System.Drawing.Size(28, 27);
			this.BtnSensorLensFocusDec.TabIndex = 28;
			this.BtnSensorLensFocusDec.TabStop = false;
			this.BtnSensorLensFocusDec.Text = "◀";
			this.BtnSensorLensFocusDec.UseVisualStyleBackColor = true;
			this.BtnSensorLensFocusDec.Click += new System.EventHandler(BtnSensorLensFocusDec_Click);
			this.TxbSensorLensFocusSet.Location = new System.Drawing.Point(222, 27);
			this.TxbSensorLensFocusSet.Name = "TxbSensorLensFocusSet";
			this.TxbSensorLensFocusSet.ReadOnly = true;
			this.TxbSensorLensFocusSet.Size = new System.Drawing.Size(58, 23);
			this.TxbSensorLensFocusSet.TabIndex = 5;
			this.TxbSensorLensFocusSet.Text = "0";
			this.LabSensorLensFocusSet.AutoSize = true;
			this.LabSensorLensFocusSet.Location = new System.Drawing.Point(174, 31);
			this.LabSensorLensFocusSet.Name = "LabSensorLensFocusSet";
			this.LabSensorLensFocusSet.Size = new System.Drawing.Size(42, 14);
			this.LabSensorLensFocusSet.TabIndex = 4;
			this.LabSensorLensFocusSet.Text = "焦距:";
			this.RdbAutoFocusEnbale.AutoSize = true;
			this.RdbAutoFocusEnbale.Location = new System.Drawing.Point(33, 29);
			this.RdbAutoFocusEnbale.Name = "RdbAutoFocusEnbale";
			this.RdbAutoFocusEnbale.Size = new System.Drawing.Size(53, 18);
			this.RdbAutoFocusEnbale.TabIndex = 2;
			this.RdbAutoFocusEnbale.TabStop = true;
			this.RdbAutoFocusEnbale.Text = "自动";
			this.RdbAutoFocusEnbale.UseVisualStyleBackColor = true;
			this.RdbAutoFocusEnbale.Visible = false;
			this.RdbAutoFocusDisabale.AutoSize = true;
			this.RdbAutoFocusDisabale.Location = new System.Drawing.Point(100, 29);
			this.RdbAutoFocusDisabale.Name = "RdbAutoFocusDisabale";
			this.RdbAutoFocusDisabale.Size = new System.Drawing.Size(53, 18);
			this.RdbAutoFocusDisabale.TabIndex = 0;
			this.RdbAutoFocusDisabale.TabStop = true;
			this.RdbAutoFocusDisabale.Text = "手动";
			this.RdbAutoFocusDisabale.UseVisualStyleBackColor = true;
			this.RdbAutoFocusDisabale.Visible = false;
			this.Tp_Lighting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Tp_Lighting.Location = new System.Drawing.Point(4, 34);
			this.Tp_Lighting.Name = "Tp_Lighting";
			this.Tp_Lighting.Padding = new System.Windows.Forms.Padding(3);
			this.Tp_Lighting.Size = new System.Drawing.Size(462, 653);
			this.Tp_Lighting.TabIndex = 2;
			this.Tp_Lighting.Text = "灯光照明";
			this.Tp_Lighting.UseVisualStyleBackColor = true;
			this.Tp_ImageProcess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Tp_ImageProcess.Location = new System.Drawing.Point(4, 34);
			this.Tp_ImageProcess.Name = "Tp_ImageProcess";
			this.Tp_ImageProcess.Padding = new System.Windows.Forms.Padding(3);
			this.Tp_ImageProcess.Size = new System.Drawing.Size(462, 653);
			this.Tp_ImageProcess.TabIndex = 3;
			this.Tp_ImageProcess.Text = "图像处理";
			this.Tp_ImageProcess.UseVisualStyleBackColor = true;
			this.Tp_BarcodeType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Tp_BarcodeType.Location = new System.Drawing.Point(4, 34);
			this.Tp_BarcodeType.Name = "Tp_BarcodeType";
			this.Tp_BarcodeType.Padding = new System.Windows.Forms.Padding(3);
			this.Tp_BarcodeType.Size = new System.Drawing.Size(462, 653);
			this.Tp_BarcodeType.TabIndex = 4;
			this.Tp_BarcodeType.Text = "码制设置";
			this.Tp_BarcodeType.UseVisualStyleBackColor = true;
			this.Tp_Template.Location = new System.Drawing.Point(4, 34);
			this.Tp_Template.Name = "Tp_Template";
			this.Tp_Template.Padding = new System.Windows.Forms.Padding(3);
			this.Tp_Template.Size = new System.Drawing.Size(462, 653);
			this.Tp_Template.TabIndex = 5;
			this.Tp_Template.Text = "模板配置";
			this.Tp_Template.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(473, 691);
			base.Controls.Add(this.TabSettings);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DeviceSettingForm";
			this.Text = "设置窗口";
			base.Load += new System.EventHandler(DeviceSettingForm_Load);
			this.GrbWorkMode.ResumeLayout(false);
			this.GrbWorkMode.PerformLayout();
			this.GrbTrigTime.ResumeLayout(false);
			this.GrbTrigTime.PerformLayout();
			this.GrbExpSetting.ResumeLayout(false);
			this.GrbExpSetting.PerformLayout();
			this.GrbSensorGainSetting.ResumeLayout(false);
			this.GrbSensorGainSetting.PerformLayout();
			this.TabSettings.ResumeLayout(false);
			this.TpParaAdjust.ResumeLayout(false);
			this.PanHdr.ResumeLayout(false);
			this.PanHdr.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbHDR_LightExp).EndInit();
			this.GrbSoundSetting.ResumeLayout(false);
			this.GrbSoundSetting.PerformLayout();
			this.GrbSensorLensFocus.ResumeLayout(false);
			this.GrbSensorLensFocus.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
