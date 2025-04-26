using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace Heroje_Debug_Tool.SubForm
{
	public class LightingForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private int MainLightVal;

		private int SecondLightVal;

		private int light_ctrl;

		private IContainer components = null;

		private GroupBox GrbSecondLight;

		private Button BtnSecondLightValAll;

		private Button BtnSecondLightValDec;

		private TextBox TxbSecondLightVal;

		private Label LabSecondLight;

		private RadioButton RdbSecondLightClose;

		private RadioButton RdbSecondLightOpen;

		private TrackBar TrbSecondLightDuty;

		private GroupBox GdbMainLight;

		private Button BtnMainLightValAll;

		private Button BtnMainLightValDec;

		private TextBox TxbMainLightVal;

		private Label LabMainLight;

		private RadioButton RdbMainLightClose;

		private RadioButton RdbMainLightOpen;

		private TrackBar TrbMainLightDuty;

		private GroupBox GrbFocusLight;

		private RadioButton RdbFocusLEDNormal;

		private RadioButton RdbFocusLEDClose;

		private RadioButton RdbFocusLEDAlways;

		private RadioButton RdbFocusLEDBlink;

		private GroupBox GrbOtherLightSetting;

		private CheckBox ChbNearLightRB;

		private CheckBox ChbNearLightLB;

		private CheckBox ChbNearLightRT;

		private CheckBox ChbNearLightLT;

		private Panel PanFarNearLightCtrl;

		private CheckBox ChbFarLightRB;

		private CheckBox ChbFarLightLB;

		private CheckBox ChbFarLightRT;

		private CheckBox ChbFarLightLT;

		private Label LabFarNearLightCtrl;

		private Panel PanPolarizationCfg;

		private RadioButton RdbDeafultP;

		private Label LabPolarizationSetting;

		private RadioButton RdbPolarizationDisable;

		private RadioButton RdbPolarizationEnable;

		public int Light_Pwm_1
		{
			get
			{
				if (RdbMainLightOpen.Checked)
				{
					MainLightVal = TrbMainLightDuty.Value;
				}
				else
				{
					MainLightVal = 0;
				}
				return MainLightVal;
			}
			set
			{
				MainLightVal = value;
				TrbMainLightDuty.Value = MainLightVal;
			}
		}

		public int Light_Pwm_2
		{
			get
			{
				if (RdbSecondLightOpen.Checked)
				{
					SecondLightVal = TrbSecondLightDuty.Value;
				}
				else
				{
					SecondLightVal = 0;
				}
				return SecondLightVal;
			}
			set
			{
				SecondLightVal = value;
				TrbSecondLightDuty.Value = SecondLightVal;
			}
		}

		public int FarNearLight
		{
			get
			{
				light_ctrl = 0;
				if (ChbFarLightLT.Checked)
				{
					light_ctrl |= 8;
				}
				if (ChbFarLightLB.Checked)
				{
					light_ctrl |= 16;
				}
				if (ChbFarLightRB.Checked)
				{
					light_ctrl |= 32;
				}
				if (ChbFarLightRT.Checked)
				{
					light_ctrl |= 64;
				}
				if (ChbNearLightLT.Checked)
				{
					light_ctrl |= 128;
				}
				if (ChbNearLightLB.Checked)
				{
					light_ctrl |= 256;
				}
				if (ChbNearLightRB.Checked)
				{
					light_ctrl |= 512;
				}
				if (ChbNearLightRT.Checked)
				{
					light_ctrl |= 1024;
				}
				return light_ctrl;
			}
			set
			{
				light_ctrl = value;
				ChbFarLightLT.Checked = (light_ctrl & 8) == 8;
				ChbFarLightLB.Checked = (light_ctrl & 0x10) == 16;
				ChbFarLightRB.Checked = (light_ctrl & 0x20) == 32;
				ChbFarLightRT.Checked = (light_ctrl & 0x40) == 64;
				ChbNearLightLT.Checked = (light_ctrl & 0x80) == 128;
				ChbNearLightLB.Checked = (light_ctrl & 0x100) == 256;
				ChbNearLightRB.Checked = (light_ctrl & 0x200) == 512;
				ChbNearLightRT.Checked = (light_ctrl & 0x400) == 1024;
			}
		}

		public LightingForm()
		{
			InitializeComponent();
		}

		private void LightingForm_Load(object sender, EventArgs e)
		{
			TrbMainLightDuty.MouseWheel += delegate(object _sender, MouseEventArgs _e)
			{
				((HandledMouseEventArgs)_e).Handled = true;
			};
			TrbSecondLightDuty.MouseWheel += delegate(object _sender, MouseEventArgs _e)
			{
				((HandledMouseEventArgs)_e).Handled = true;
			};
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			if (GetCfgCBFuncCB(12u) == 0)
			{
				RdbMainLightClose.Checked = true;
				TrbMainLightDuty.Value = 0;
			}
			else
			{
				RdbMainLightOpen.Checked = true;
				TrbMainLightDuty.Value = GetCfgCBFuncCB(1279u);
			}
			if (GetCfgCBFuncCB(3585u) == 0)
			{
				RdbSecondLightClose.Checked = true;
				TrbSecondLightDuty.Value = 0;
			}
			else
			{
				RdbSecondLightOpen.Checked = true;
				TrbSecondLightDuty.Value = GetCfgCBFuncCB(5631u);
			}
			if (GetCfgCBFuncCB(48u) == 48)
			{
				RdbFocusLEDAlways.Checked = true;
			}
			else if (GetCfgCBFuncCB(48u) == 0)
			{
				RdbFocusLEDClose.Checked = true;
			}
			else if (GetCfgCBFuncCB(48u) == 32)
			{
				RdbFocusLEDBlink.Checked = true;
			}
			else
			{
				RdbFocusLEDNormal.Checked = true;
			}
			if (para.ParaDataLen >= 4096)
			{
				GrbSecondLight.Enabled = false;
				PanPolarizationCfg.Enabled = true;
			}
			else
			{
				GrbSecondLight.Enabled = true;
				PanPolarizationCfg.Enabled = false;
			}
			if (para.DeviceTypeRecord >= 6)
			{
				PanFarNearLightCtrl.Enabled = true;
				ChbFarLightLT.Checked = GetCfgCBFuncCB(74753u) == 1;
				ChbFarLightLB.Checked = GetCfgCBFuncCB(74754u) == 2;
				ChbFarLightRB.Checked = GetCfgCBFuncCB(74756u) == 4;
				ChbFarLightRT.Checked = GetCfgCBFuncCB(74760u) == 8;
				ChbNearLightLT.Checked = GetCfgCBFuncCB(75009u) == 1;
				ChbNearLightLB.Checked = GetCfgCBFuncCB(75010u) == 2;
				ChbNearLightRB.Checked = GetCfgCBFuncCB(75012u) == 4;
				ChbNearLightRT.Checked = GetCfgCBFuncCB(75016u) == 8;
				if (para.DeviceTypeRecord == 5)
				{
					if (GetCfgCBFuncCB(73731u) == 1)
					{
						RdbPolarizationDisable.Checked = true;
					}
					else if (GetCfgCBFuncCB(73731u) == 2)
					{
						RdbPolarizationEnable.Checked = true;
					}
					else
					{
						RdbDeafultP.Checked = true;
					}
				}
				else if (ChbFarLightLT.Checked && ChbFarLightLB.Checked && !ChbFarLightRT.Checked && !ChbFarLightRB.Checked)
				{
					RdbPolarizationDisable.Checked = true;
				}
				else if (!ChbFarLightLT.Checked && !ChbFarLightLB.Checked && ChbFarLightRT.Checked && ChbFarLightRB.Checked)
				{
					RdbPolarizationEnable.Checked = true;
				}
				else
				{
					RdbDeafultP.Checked = true;
				}
			}
			else
			{
				PanFarNearLightCtrl.Enabled = false;
			}
			GrbSecondLight.Enabled = false;
			if (para.DeviceTypeRecord == 5 && para.ParaDataLen < 4096)
			{
				GrbSecondLight.Enabled = true;
			}
		}

		public void FunctionOnOff(bool[] CapacityArr, string DeviceName)
		{
			GrbFocusLight.Enabled = true;// CapacityArr[2];
			GrbSecondLight.Enabled = CapacityArr[3];
			GdbMainLight.Enabled = CapacityArr[4];
			ChbFarLightLT.Enabled = CapacityArr[5];
			ChbFarLightLB.Enabled = CapacityArr[6];
			ChbFarLightRT.Enabled = CapacityArr[7];
			ChbFarLightRB.Enabled = CapacityArr[8];
			ChbNearLightRT.Enabled = CapacityArr[9];
			ChbNearLightRB.Enabled = CapacityArr[10];
			ChbNearLightLB.Enabled = CapacityArr[11];
			ChbNearLightLT.Enabled = CapacityArr[12];
			PanPolarizationCfg.Enabled = CapacityArr[13];
			ChbFarLightLT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextUpperLeft);
			ChbFarLightLB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextLowerLeft);
			ChbFarLightRT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextUpperRight);
			ChbFarLightRB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextLowerRight);
			ChbNearLightRT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextUpperPart);
			ChbNearLightRB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextLowerPart);
			ChbNearLightLB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextLeftSide);
			ChbNearLightLT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRightSide);
			if (DeviceName.Contains("Tiny"))
			{
				ChbNearLightLB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedFillLight);
				ChbNearLightLT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextWhiteFillLight);
			}
			else if (DeviceName.Contains("HM270") || DeviceName.Contains("HM271"))
			{
				ChbFarLightLT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextWhiteFillLight);
				ChbFarLightRT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedFillLight1);
				ChbFarLightRB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedFillLight2);
			}
			else if (DeviceName.Contains("HRX-"))
			{
				ChbFarLightLT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedFillLight1);
				ChbFarLightLB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedFillLight2);
				ChbFarLightRT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedFillLight3);
				ChbFarLightRB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedFillLight4);
				if (DeviceName.Contains("HRX-013") && !DeviceName.Contains("HRX-013F"))
				{
					ChbNearLightLT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextWhiteFillLight);
				}
			}
			else if (DeviceName.Contains("HM270"))
			{
				ChbFarLightLT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedshadeLight);
				ChbFarLightLB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextWhiteFillLight);
				ChbFarLightRT.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextRedFillLight);
				ChbFarLightRB.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.TextBlueFillLight);
			}
		}

		private void RdbFocusLEDBlink_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbFocusLEDBlink.Checked)
			{
				SetCfgCBFuncCB(48u, 32u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbFocusLEDNormal_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbFocusLEDNormal.Checked)
			{
				SetCfgCBFuncCB(48u, 16u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbFocusLEDAlways_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbFocusLEDAlways.Checked)
			{
				SetCfgCBFuncCB(48u, 48u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbFocusLEDClose_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbFocusLEDClose.Checked)
			{
				SetCfgCBFuncCB(48u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbMainLightClose_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbMainLightClose.Checked)
			{
				SetCfgCBFuncCB(12u, 0u);
				SetCfgCBFuncCB(1279u, 0u);
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void RdbMainLightOpen_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbMainLightOpen.Checked)
			{
				SetCfgCBFuncCB(12u, 4u);
				SetCfgCBFuncCB(1279u, (ushort)TrbMainLightDuty.Value);
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void BtnMainLightValDec_Click(object sender, EventArgs e)
		{
			if (TrbMainLightDuty.Value > 0)
			{
				TrbMainLightDuty.Value--;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(1279u, (ushort)TrbMainLightDuty.Value);
					SendCfgDataCBFuncCB(512u);
				}
			}
		}

		private void BtnMainLightValAll_Click(object sender, EventArgs e)
		{
			if (TrbMainLightDuty.Value < 255)
			{
				TrbMainLightDuty.Value++;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(1279u, (ushort)TrbMainLightDuty.Value);
					SendCfgDataCBFuncCB(512u);
				}
			}
		}

		private void TrbMainLightDuty_MouseUp(object sender, MouseEventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(1279u, (ushort)TrbMainLightDuty.Value);
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void TrbMainLightDuty_ValueChanged(object sender, EventArgs e)
		{
			TxbMainLightVal.Text = TrbMainLightDuty.Value.ToString();
			ToolCfg.Temp_MainLight = TrbMainLightDuty.Value;
		}

		private void RdbSecondLightClose_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbSecondLightClose.Checked)
			{
				SetCfgCBFuncCB(3585u, 0u);
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void RdbSecondLightOpen_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbSecondLightOpen.Checked)
			{
				SetCfgCBFuncCB(3585u, 1u);
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void BtnSecondLightValDec_Click(object sender, EventArgs e)
		{
			if (TrbSecondLightDuty.Value > 0)
			{
				TrbSecondLightDuty.Value--;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(5631u, (ushort)TrbSecondLightDuty.Value);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void BtnSecondLightValAll_Click(object sender, EventArgs e)
		{
			if (TrbSecondLightDuty.Value < 255)
			{
				TrbSecondLightDuty.Value++;
				if (!ToolCfg.UpdateAdjState)
				{
					SetCfgCBFuncCB(5631u, (ushort)TrbSecondLightDuty.Value);
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void TrbSecondLightDuty_MouseUp(object sender, MouseEventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(5631u, (ushort)TrbSecondLightDuty.Value);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TrbSecondLightDuty_ValueChanged(object sender, EventArgs e)
		{
			TxbSecondLightVal.Text = TrbSecondLightDuty.Value.ToString();
			ToolCfg.Temp_SecondLight = TrbSecondLightDuty.Value;
		}

		private void RdbPolarizationEnable_CheckedChanged(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			if (ToolCfg.DeviceTypeRecord == 5)
			{
				if (RdbPolarizationEnable.Checked)
				{
					SetCfgCBFuncCB(73731u, 2u);
					SendCfgDataCBFuncCB(0u);
				}
			}
			else
			{
				ChbFarLightLT.Checked = false;
				Thread.Sleep(100);
				ChbFarLightLB.Checked = false;
				Thread.Sleep(100);
				ChbFarLightRT.Checked = true;
				Thread.Sleep(100);
				ChbFarLightRB.Checked = true;
			}
		}

		private void RdbPolarizationDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			if (ToolCfg.DeviceTypeRecord == 5)
			{
				if (RdbPolarizationDisable.Checked)
				{
					SetCfgCBFuncCB(73731u, 1u);
					SendCfgDataCBFuncCB(0u);
				}
			}
			else
			{
				ChbFarLightLT.Checked = true;
				Thread.Sleep(100);
				ChbFarLightLB.Checked = true;
				Thread.Sleep(100);
				ChbFarLightRT.Checked = false;
				Thread.Sleep(100);
				ChbFarLightRB.Checked = false;
			}
		}

		private void RdbDeafultP_CheckedChanged(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			if (ToolCfg.DeviceTypeRecord == 5)
			{
				if (RdbDeafultP.Checked)
				{
					SetCfgCBFuncCB(73731u, 0u);
					SendCfgDataCBFuncCB(0u);
				}
			}
			else
			{
				ChbFarLightLT.Checked = true;
				Thread.Sleep(100);
				ChbFarLightLB.Checked = true;
				Thread.Sleep(100);
				ChbFarLightRT.Checked = true;
				Thread.Sleep(100);
				ChbFarLightRB.Checked = true;
			}
		}

		private void ChbFarLightLT_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbFarLightLT.Checked)
				{
					SetCfgCBFuncCB(74753u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(74753u, 0u);
				}
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void ChbFarLightRT_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbFarLightRT.Checked)
				{
					SetCfgCBFuncCB(74760u, 8u);
				}
				else
				{
					SetCfgCBFuncCB(74760u, 0u);
				}
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void ChbFarLightLB_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbFarLightLB.Checked)
				{
					SetCfgCBFuncCB(74754u, 2u);
				}
				else
				{
					SetCfgCBFuncCB(74754u, 0u);
				}
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void ChbFarLightRB_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbFarLightRB.Checked)
				{
					SetCfgCBFuncCB(74756u, 4u);
				}
				else
				{
					SetCfgCBFuncCB(74756u, 0u);
				}
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void ChbNearLightLT_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbNearLightLT.Checked)
				{
					SetCfgCBFuncCB(75009u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(75009u, 0u);
				}
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void ChbNearLightRT_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbNearLightRT.Checked)
				{
					SetCfgCBFuncCB(75016u, 8u);
				}
				else
				{
					SetCfgCBFuncCB(75016u, 0u);
				}
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void ChbNearLightLB_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbNearLightLB.Checked)
				{
					SetCfgCBFuncCB(75010u, 2u);
				}
				else
				{
					SetCfgCBFuncCB(75010u, 0u);
				}
				SendCfgDataCBFuncCB(512u);
			}
		}

		private void ChbNearLightRB_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbNearLightRB.Checked)
				{
					SetCfgCBFuncCB(75012u, 4u);
				}
				else
				{
					SetCfgCBFuncCB(75012u, 0u);
				}
				SendCfgDataCBFuncCB(512u);
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
			this.GrbSecondLight = new System.Windows.Forms.GroupBox();
			this.BtnSecondLightValAll = new System.Windows.Forms.Button();
			this.BtnSecondLightValDec = new System.Windows.Forms.Button();
			this.TxbSecondLightVal = new System.Windows.Forms.TextBox();
			this.LabSecondLight = new System.Windows.Forms.Label();
			this.RdbSecondLightClose = new System.Windows.Forms.RadioButton();
			this.RdbSecondLightOpen = new System.Windows.Forms.RadioButton();
			this.TrbSecondLightDuty = new System.Windows.Forms.TrackBar();
			this.GdbMainLight = new System.Windows.Forms.GroupBox();
			this.BtnMainLightValAll = new System.Windows.Forms.Button();
			this.BtnMainLightValDec = new System.Windows.Forms.Button();
			this.TxbMainLightVal = new System.Windows.Forms.TextBox();
			this.LabMainLight = new System.Windows.Forms.Label();
			this.RdbMainLightClose = new System.Windows.Forms.RadioButton();
			this.RdbMainLightOpen = new System.Windows.Forms.RadioButton();
			this.TrbMainLightDuty = new System.Windows.Forms.TrackBar();
			this.GrbFocusLight = new System.Windows.Forms.GroupBox();
			this.RdbFocusLEDNormal = new System.Windows.Forms.RadioButton();
			this.RdbFocusLEDClose = new System.Windows.Forms.RadioButton();
			this.RdbFocusLEDAlways = new System.Windows.Forms.RadioButton();
			this.RdbFocusLEDBlink = new System.Windows.Forms.RadioButton();
			this.GrbOtherLightSetting = new System.Windows.Forms.GroupBox();
			this.ChbNearLightRB = new System.Windows.Forms.CheckBox();
			this.ChbNearLightLB = new System.Windows.Forms.CheckBox();
			this.ChbNearLightRT = new System.Windows.Forms.CheckBox();
			this.ChbNearLightLT = new System.Windows.Forms.CheckBox();
			this.PanFarNearLightCtrl = new System.Windows.Forms.Panel();
			this.ChbFarLightRB = new System.Windows.Forms.CheckBox();
			this.ChbFarLightLB = new System.Windows.Forms.CheckBox();
			this.ChbFarLightRT = new System.Windows.Forms.CheckBox();
			this.ChbFarLightLT = new System.Windows.Forms.CheckBox();
			this.LabFarNearLightCtrl = new System.Windows.Forms.Label();
			this.PanPolarizationCfg = new System.Windows.Forms.Panel();
			this.RdbDeafultP = new System.Windows.Forms.RadioButton();
			this.LabPolarizationSetting = new System.Windows.Forms.Label();
			this.RdbPolarizationDisable = new System.Windows.Forms.RadioButton();
			this.RdbPolarizationEnable = new System.Windows.Forms.RadioButton();
			this.GrbSecondLight.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbSecondLightDuty).BeginInit();
			this.GdbMainLight.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbMainLightDuty).BeginInit();
			this.GrbFocusLight.SuspendLayout();
			this.GrbOtherLightSetting.SuspendLayout();
			this.PanFarNearLightCtrl.SuspendLayout();
			this.PanPolarizationCfg.SuspendLayout();
			base.SuspendLayout();
			this.GrbSecondLight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbSecondLight.Controls.Add(this.BtnSecondLightValAll);
			this.GrbSecondLight.Controls.Add(this.BtnSecondLightValDec);
			this.GrbSecondLight.Controls.Add(this.TxbSecondLightVal);
			this.GrbSecondLight.Controls.Add(this.LabSecondLight);
			this.GrbSecondLight.Controls.Add(this.RdbSecondLightClose);
			this.GrbSecondLight.Controls.Add(this.RdbSecondLightOpen);
			this.GrbSecondLight.Controls.Add(this.TrbSecondLightDuty);
			this.GrbSecondLight.Location = new System.Drawing.Point(3, 156);
			this.GrbSecondLight.Name = "GrbSecondLight";
			this.GrbSecondLight.Size = new System.Drawing.Size(403, 78);
			this.GrbSecondLight.TabIndex = 35;
			this.GrbSecondLight.TabStop = false;
			this.GrbSecondLight.Text = "辅助照明设置";
			this.BtnSecondLightValAll.Location = new System.Drawing.Point(282, 49);
			this.BtnSecondLightValAll.Name = "BtnSecondLightValAll";
			this.BtnSecondLightValAll.Size = new System.Drawing.Size(28, 27);
			this.BtnSecondLightValAll.TabIndex = 29;
			this.BtnSecondLightValAll.TabStop = false;
			this.BtnSecondLightValAll.Text = "▶";
			this.BtnSecondLightValAll.UseVisualStyleBackColor = true;
			this.BtnSecondLightValAll.Click += new System.EventHandler(BtnSecondLightValAll_Click);
			this.BtnSecondLightValDec.Location = new System.Drawing.Point(245, 49);
			this.BtnSecondLightValDec.Name = "BtnSecondLightValDec";
			this.BtnSecondLightValDec.Size = new System.Drawing.Size(28, 27);
			this.BtnSecondLightValDec.TabIndex = 28;
			this.BtnSecondLightValDec.TabStop = false;
			this.BtnSecondLightValDec.Text = "◀";
			this.BtnSecondLightValDec.UseVisualStyleBackColor = true;
			this.BtnSecondLightValDec.Click += new System.EventHandler(BtnSecondLightValDec_Click);
			this.TxbSecondLightVal.Location = new System.Drawing.Point(172, 51);
			this.TxbSecondLightVal.Name = "TxbSecondLightVal";
			this.TxbSecondLightVal.ReadOnly = true;
			this.TxbSecondLightVal.Size = new System.Drawing.Size(58, 23);
			this.TxbSecondLightVal.TabIndex = 5;
			this.TxbSecondLightVal.Text = "0";
			this.LabSecondLight.AutoSize = true;
			this.LabSecondLight.Location = new System.Drawing.Point(103, 55);
			this.LabSecondLight.Name = "LabSecondLight";
			this.LabSecondLight.Size = new System.Drawing.Size(56, 14);
			this.LabSecondLight.TabIndex = 4;
			this.LabSecondLight.Text = "亮度值:";
			this.RdbSecondLightClose.AutoSize = true;
			this.RdbSecondLightClose.Location = new System.Drawing.Point(33, 23);
			this.RdbSecondLightClose.Name = "RdbSecondLightClose";
			this.RdbSecondLightClose.Size = new System.Drawing.Size(53, 18);
			this.RdbSecondLightClose.TabIndex = 2;
			this.RdbSecondLightClose.TabStop = true;
			this.RdbSecondLightClose.Text = "关闭";
			this.RdbSecondLightClose.UseVisualStyleBackColor = true;
			this.RdbSecondLightClose.CheckedChanged += new System.EventHandler(RdbSecondLightClose_CheckedChanged);
			this.RdbSecondLightOpen.AutoSize = true;
			this.RdbSecondLightOpen.Location = new System.Drawing.Point(33, 52);
			this.RdbSecondLightOpen.Name = "RdbSecondLightOpen";
			this.RdbSecondLightOpen.Size = new System.Drawing.Size(53, 18);
			this.RdbSecondLightOpen.TabIndex = 0;
			this.RdbSecondLightOpen.TabStop = true;
			this.RdbSecondLightOpen.Text = "打开";
			this.RdbSecondLightOpen.UseVisualStyleBackColor = true;
			this.RdbSecondLightOpen.CheckedChanged += new System.EventHandler(RdbSecondLightOpen_CheckedChanged);
			this.TrbSecondLightDuty.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TrbSecondLightDuty.AutoSize = false;
			this.TrbSecondLightDuty.BackColor = System.Drawing.Color.White;
			this.TrbSecondLightDuty.Location = new System.Drawing.Point(92, 23);
			this.TrbSecondLightDuty.Maximum = 255;
			this.TrbSecondLightDuty.Name = "TrbSecondLightDuty";
			this.TrbSecondLightDuty.Size = new System.Drawing.Size(308, 26);
			this.TrbSecondLightDuty.TabIndex = 3;
			this.TrbSecondLightDuty.TickStyle = System.Windows.Forms.TickStyle.None;
			this.TrbSecondLightDuty.ValueChanged += new System.EventHandler(TrbSecondLightDuty_ValueChanged);
			this.TrbSecondLightDuty.MouseUp += new System.Windows.Forms.MouseEventHandler(TrbSecondLightDuty_MouseUp);
			this.GdbMainLight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GdbMainLight.Controls.Add(this.BtnMainLightValAll);
			this.GdbMainLight.Controls.Add(this.BtnMainLightValDec);
			this.GdbMainLight.Controls.Add(this.TxbMainLightVal);
			this.GdbMainLight.Controls.Add(this.LabMainLight);
			this.GdbMainLight.Controls.Add(this.RdbMainLightClose);
			this.GdbMainLight.Controls.Add(this.RdbMainLightOpen);
			this.GdbMainLight.Controls.Add(this.TrbMainLightDuty);
			this.GdbMainLight.Location = new System.Drawing.Point(3, 71);
			this.GdbMainLight.Name = "GdbMainLight";
			this.GdbMainLight.Size = new System.Drawing.Size(403, 78);
			this.GdbMainLight.TabIndex = 34;
			this.GdbMainLight.TabStop = false;
			this.GdbMainLight.Text = "主要照明设置";
			this.BtnMainLightValAll.Location = new System.Drawing.Point(281, 48);
			this.BtnMainLightValAll.Name = "BtnMainLightValAll";
			this.BtnMainLightValAll.Size = new System.Drawing.Size(28, 27);
			this.BtnMainLightValAll.TabIndex = 29;
			this.BtnMainLightValAll.TabStop = false;
			this.BtnMainLightValAll.Text = "▶";
			this.BtnMainLightValAll.UseVisualStyleBackColor = true;
			this.BtnMainLightValAll.Click += new System.EventHandler(BtnMainLightValAll_Click);
			this.BtnMainLightValDec.Location = new System.Drawing.Point(244, 48);
			this.BtnMainLightValDec.Name = "BtnMainLightValDec";
			this.BtnMainLightValDec.Size = new System.Drawing.Size(28, 27);
			this.BtnMainLightValDec.TabIndex = 28;
			this.BtnMainLightValDec.TabStop = false;
			this.BtnMainLightValDec.Text = "◀";
			this.BtnMainLightValDec.UseVisualStyleBackColor = true;
			this.BtnMainLightValDec.Click += new System.EventHandler(BtnMainLightValDec_Click);
			this.TxbMainLightVal.Location = new System.Drawing.Point(170, 48);
			this.TxbMainLightVal.Name = "TxbMainLightVal";
			this.TxbMainLightVal.ReadOnly = true;
			this.TxbMainLightVal.Size = new System.Drawing.Size(58, 23);
			this.TxbMainLightVal.TabIndex = 5;
			this.TxbMainLightVal.Text = "0";
			this.LabMainLight.AutoSize = true;
			this.LabMainLight.Location = new System.Drawing.Point(103, 51);
			this.LabMainLight.Name = "LabMainLight";
			this.LabMainLight.Size = new System.Drawing.Size(56, 14);
			this.LabMainLight.TabIndex = 4;
			this.LabMainLight.Text = "亮度值:";
			this.RdbMainLightClose.AutoSize = true;
			this.RdbMainLightClose.Location = new System.Drawing.Point(33, 23);
			this.RdbMainLightClose.Name = "RdbMainLightClose";
			this.RdbMainLightClose.Size = new System.Drawing.Size(53, 18);
			this.RdbMainLightClose.TabIndex = 2;
			this.RdbMainLightClose.TabStop = true;
			this.RdbMainLightClose.Text = "关闭";
			this.RdbMainLightClose.UseVisualStyleBackColor = true;
			this.RdbMainLightClose.CheckedChanged += new System.EventHandler(RdbMainLightClose_CheckedChanged);
			this.RdbMainLightOpen.AutoSize = true;
			this.RdbMainLightOpen.Location = new System.Drawing.Point(33, 49);
			this.RdbMainLightOpen.Name = "RdbMainLightOpen";
			this.RdbMainLightOpen.Size = new System.Drawing.Size(53, 18);
			this.RdbMainLightOpen.TabIndex = 0;
			this.RdbMainLightOpen.TabStop = true;
			this.RdbMainLightOpen.Text = "打开";
			this.RdbMainLightOpen.UseVisualStyleBackColor = true;
			this.RdbMainLightOpen.CheckedChanged += new System.EventHandler(RdbMainLightOpen_CheckedChanged);
			this.TrbMainLightDuty.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TrbMainLightDuty.AutoSize = false;
			this.TrbMainLightDuty.BackColor = System.Drawing.Color.White;
			this.TrbMainLightDuty.Location = new System.Drawing.Point(92, 16);
			this.TrbMainLightDuty.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.TrbMainLightDuty.Maximum = 255;
			this.TrbMainLightDuty.Name = "TrbMainLightDuty";
			this.TrbMainLightDuty.Size = new System.Drawing.Size(308, 26);
			this.TrbMainLightDuty.TabIndex = 3;
			this.TrbMainLightDuty.TickStyle = System.Windows.Forms.TickStyle.None;
			this.TrbMainLightDuty.ValueChanged += new System.EventHandler(TrbMainLightDuty_ValueChanged);
			this.TrbMainLightDuty.MouseUp += new System.Windows.Forms.MouseEventHandler(TrbMainLightDuty_MouseUp);
			this.GrbFocusLight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbFocusLight.Controls.Add(this.RdbFocusLEDNormal);
			this.GrbFocusLight.Controls.Add(this.RdbFocusLEDClose);
			this.GrbFocusLight.Controls.Add(this.RdbFocusLEDAlways);
			this.GrbFocusLight.Controls.Add(this.RdbFocusLEDBlink);
			this.GrbFocusLight.Location = new System.Drawing.Point(3, 5);
			this.GrbFocusLight.Name = "GrbFocusLight";
			this.GrbFocusLight.Size = new System.Drawing.Size(403, 59);
			this.GrbFocusLight.TabIndex = 32;
			this.GrbFocusLight.TabStop = false;
			this.GrbFocusLight.Text = "瞄准指示设置";
			this.RdbFocusLEDNormal.AutoSize = true;
			this.RdbFocusLEDNormal.Location = new System.Drawing.Point(127, 23);
			this.RdbFocusLEDNormal.Name = "RdbFocusLEDNormal";
			this.RdbFocusLEDNormal.Size = new System.Drawing.Size(53, 18);
			this.RdbFocusLEDNormal.TabIndex = 3;
			this.RdbFocusLEDNormal.TabStop = true;
			this.RdbFocusLEDNormal.Text = "正常";
			this.RdbFocusLEDNormal.UseVisualStyleBackColor = true;
			this.RdbFocusLEDNormal.CheckedChanged += new System.EventHandler(RdbFocusLEDNormal_CheckedChanged);
			this.RdbFocusLEDClose.AutoSize = true;
			this.RdbFocusLEDClose.Location = new System.Drawing.Point(316, 23);
			this.RdbFocusLEDClose.Name = "RdbFocusLEDClose";
			this.RdbFocusLEDClose.Size = new System.Drawing.Size(53, 18);
			this.RdbFocusLEDClose.TabIndex = 2;
			this.RdbFocusLEDClose.TabStop = true;
			this.RdbFocusLEDClose.Text = "关闭";
			this.RdbFocusLEDClose.UseVisualStyleBackColor = true;
			this.RdbFocusLEDClose.CheckedChanged += new System.EventHandler(RdbFocusLEDClose_CheckedChanged);
			this.RdbFocusLEDAlways.AutoSize = true;
			this.RdbFocusLEDAlways.Location = new System.Drawing.Point(222, 23);
			this.RdbFocusLEDAlways.Name = "RdbFocusLEDAlways";
			this.RdbFocusLEDAlways.Size = new System.Drawing.Size(53, 18);
			this.RdbFocusLEDAlways.TabIndex = 1;
			this.RdbFocusLEDAlways.TabStop = true;
			this.RdbFocusLEDAlways.Text = "常亮";
			this.RdbFocusLEDAlways.UseVisualStyleBackColor = true;
			this.RdbFocusLEDAlways.CheckedChanged += new System.EventHandler(RdbFocusLEDAlways_CheckedChanged);
			this.RdbFocusLEDBlink.AutoSize = true;
			this.RdbFocusLEDBlink.Location = new System.Drawing.Point(33, 23);
			this.RdbFocusLEDBlink.Name = "RdbFocusLEDBlink";
			this.RdbFocusLEDBlink.Size = new System.Drawing.Size(53, 18);
			this.RdbFocusLEDBlink.TabIndex = 0;
			this.RdbFocusLEDBlink.TabStop = true;
			this.RdbFocusLEDBlink.Text = "闪烁";
			this.RdbFocusLEDBlink.UseVisualStyleBackColor = true;
			this.RdbFocusLEDBlink.CheckedChanged += new System.EventHandler(RdbFocusLEDBlink_CheckedChanged);
			this.GrbOtherLightSetting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbOtherLightSetting.Controls.Add(this.PanFarNearLightCtrl);
			this.GrbOtherLightSetting.Controls.Add(this.PanPolarizationCfg);
			this.GrbOtherLightSetting.Location = new System.Drawing.Point(3, 241);
			this.GrbOtherLightSetting.Name = "GrbOtherLightSetting";
			this.GrbOtherLightSetting.Size = new System.Drawing.Size(403, 266);
			this.GrbOtherLightSetting.TabIndex = 37;
			this.GrbOtherLightSetting.TabStop = false;
			this.GrbOtherLightSetting.Text = "其它相关设置";
			this.ChbNearLightRB.AutoSize = true;
			this.ChbNearLightRB.Location = new System.Drawing.Point(187, 153);
			this.ChbNearLightRB.Name = "ChbNearLightRB";
			this.ChbNearLightRB.Size = new System.Drawing.Size(54, 18);
			this.ChbNearLightRB.TabIndex = 7;
			this.ChbNearLightRB.Text = "下部";
			this.ChbNearLightRB.UseVisualStyleBackColor = true;
			this.ChbNearLightRB.CheckedChanged += new System.EventHandler(ChbNearLightRB_CheckedChanged);
			this.ChbNearLightLB.AutoSize = true;
			this.ChbNearLightLB.Location = new System.Drawing.Point(108, 132);
			this.ChbNearLightLB.Name = "ChbNearLightLB";
			this.ChbNearLightLB.Size = new System.Drawing.Size(54, 18);
			this.ChbNearLightLB.TabIndex = 6;
			this.ChbNearLightLB.Text = "左侧";
			this.ChbNearLightLB.UseVisualStyleBackColor = true;
			this.ChbNearLightLB.CheckedChanged += new System.EventHandler(ChbNearLightLB_CheckedChanged);
			this.ChbNearLightRT.AutoSize = true;
			this.ChbNearLightRT.Location = new System.Drawing.Point(185, 108);
			this.ChbNearLightRT.Name = "ChbNearLightRT";
			this.ChbNearLightRT.Size = new System.Drawing.Size(54, 18);
			this.ChbNearLightRT.TabIndex = 5;
			this.ChbNearLightRT.Text = "上部";
			this.ChbNearLightRT.UseVisualStyleBackColor = true;
			this.ChbNearLightRT.CheckedChanged += new System.EventHandler(ChbNearLightRT_CheckedChanged);
			this.ChbNearLightLT.AutoSize = true;
			this.ChbNearLightLT.Location = new System.Drawing.Point(264, 132);
			this.ChbNearLightLT.Name = "ChbNearLightLT";
			this.ChbNearLightLT.Size = new System.Drawing.Size(54, 18);
			this.ChbNearLightLT.TabIndex = 4;
			this.ChbNearLightLT.Text = "右侧";
			this.ChbNearLightLT.UseVisualStyleBackColor = true;
			this.ChbNearLightLT.CheckedChanged += new System.EventHandler(ChbNearLightLT_CheckedChanged);
			this.PanFarNearLightCtrl.Controls.Add(this.ChbNearLightRB);
			this.PanFarNearLightCtrl.Controls.Add(this.ChbFarLightRB);
			this.PanFarNearLightCtrl.Controls.Add(this.ChbNearLightLB);
			this.PanFarNearLightCtrl.Controls.Add(this.ChbNearLightRT);
			this.PanFarNearLightCtrl.Controls.Add(this.ChbFarLightLB);
			this.PanFarNearLightCtrl.Controls.Add(this.ChbNearLightLT);
			this.PanFarNearLightCtrl.Controls.Add(this.ChbFarLightRT);
			this.PanFarNearLightCtrl.Controls.Add(this.ChbFarLightLT);
			this.PanFarNearLightCtrl.Controls.Add(this.LabFarNearLightCtrl);
			this.PanFarNearLightCtrl.Location = new System.Drawing.Point(20, 76);
			this.PanFarNearLightCtrl.Name = "PanFarNearLightCtrl";
			this.PanFarNearLightCtrl.Size = new System.Drawing.Size(374, 184);
			this.PanFarNearLightCtrl.TabIndex = 5;
			this.ChbFarLightRB.AutoSize = true;
			this.ChbFarLightRB.Location = new System.Drawing.Point(256, 60);
			this.ChbFarLightRB.Name = "ChbFarLightRB";
			this.ChbFarLightRB.Size = new System.Drawing.Size(54, 18);
			this.ChbFarLightRB.TabIndex = 7;
			this.ChbFarLightRB.Text = "右下";
			this.ChbFarLightRB.UseVisualStyleBackColor = true;
			this.ChbFarLightRB.CheckedChanged += new System.EventHandler(ChbFarLightRB_CheckedChanged);
			this.ChbFarLightLB.AutoSize = true;
			this.ChbFarLightLB.Location = new System.Drawing.Point(128, 59);
			this.ChbFarLightLB.Name = "ChbFarLightLB";
			this.ChbFarLightLB.Size = new System.Drawing.Size(54, 18);
			this.ChbFarLightLB.TabIndex = 6;
			this.ChbFarLightLB.Text = "左下";
			this.ChbFarLightLB.UseVisualStyleBackColor = true;
			this.ChbFarLightLB.CheckedChanged += new System.EventHandler(ChbFarLightLB_CheckedChanged);
			this.ChbFarLightRT.AutoSize = true;
			this.ChbFarLightRT.Location = new System.Drawing.Point(256, 29);
			this.ChbFarLightRT.Name = "ChbFarLightRT";
			this.ChbFarLightRT.Size = new System.Drawing.Size(54, 18);
			this.ChbFarLightRT.TabIndex = 5;
			this.ChbFarLightRT.Text = "右上";
			this.ChbFarLightRT.UseVisualStyleBackColor = true;
			this.ChbFarLightRT.CheckedChanged += new System.EventHandler(ChbFarLightRT_CheckedChanged);
			this.ChbFarLightLT.AutoSize = true;
			this.ChbFarLightLT.Location = new System.Drawing.Point(128, 29);
			this.ChbFarLightLT.Name = "ChbFarLightLT";
			this.ChbFarLightLT.Size = new System.Drawing.Size(54, 18);
			this.ChbFarLightLT.TabIndex = 4;
			this.ChbFarLightLT.Text = "左上";
			this.ChbFarLightLT.UseVisualStyleBackColor = true;
			this.ChbFarLightLT.CheckedChanged += new System.EventHandler(ChbFarLightLT_CheckedChanged);
			this.LabFarNearLightCtrl.AutoSize = true;
			this.LabFarNearLightCtrl.Location = new System.Drawing.Point(7, 9);
			this.LabFarNearLightCtrl.Name = "LabFarNearLightCtrl";
			this.LabFarNearLightCtrl.Size = new System.Drawing.Size(98, 14);
			this.LabFarNearLightCtrl.TabIndex = 3;
			this.LabFarNearLightCtrl.Text = "照明分组控制:";
			this.PanPolarizationCfg.Controls.Add(this.RdbDeafultP);
			this.PanPolarizationCfg.Controls.Add(this.LabPolarizationSetting);
			this.PanPolarizationCfg.Controls.Add(this.RdbPolarizationDisable);
			this.PanPolarizationCfg.Controls.Add(this.RdbPolarizationEnable);
			this.PanPolarizationCfg.Location = new System.Drawing.Point(20, 26);
			this.PanPolarizationCfg.Name = "PanPolarizationCfg";
			this.PanPolarizationCfg.Size = new System.Drawing.Size(374, 43);
			this.PanPolarizationCfg.TabIndex = 3;
			this.RdbDeafultP.AutoSize = true;
			this.RdbDeafultP.Location = new System.Drawing.Point(289, 12);
			this.RdbDeafultP.Name = "RdbDeafultP";
			this.RdbDeafultP.Size = new System.Drawing.Size(53, 18);
			this.RdbDeafultP.TabIndex = 4;
			this.RdbDeafultP.TabStop = true;
			this.RdbDeafultP.Text = "默认";
			this.RdbDeafultP.UseVisualStyleBackColor = true;
			this.RdbDeafultP.CheckedChanged += new System.EventHandler(RdbDeafultP_CheckedChanged);
			this.LabPolarizationSetting.AutoSize = true;
			this.LabPolarizationSetting.Location = new System.Drawing.Point(7, 14);
			this.LabPolarizationSetting.Name = "LabPolarizationSetting";
			this.LabPolarizationSetting.Size = new System.Drawing.Size(98, 14);
			this.LabPolarizationSetting.TabIndex = 3;
			this.LabPolarizationSetting.Text = "偏振照明设置:";
			this.RdbPolarizationDisable.AutoSize = true;
			this.RdbPolarizationDisable.Location = new System.Drawing.Point(209, 12);
			this.RdbPolarizationDisable.Name = "RdbPolarizationDisable";
			this.RdbPolarizationDisable.Size = new System.Drawing.Size(53, 18);
			this.RdbPolarizationDisable.TabIndex = 2;
			this.RdbPolarizationDisable.TabStop = true;
			this.RdbPolarizationDisable.Text = "关闭";
			this.RdbPolarizationDisable.UseVisualStyleBackColor = true;
			this.RdbPolarizationDisable.CheckedChanged += new System.EventHandler(RdbPolarizationDisable_CheckedChanged);
			this.RdbPolarizationEnable.AutoSize = true;
			this.RdbPolarizationEnable.Location = new System.Drawing.Point(140, 12);
			this.RdbPolarizationEnable.Name = "RdbPolarizationEnable";
			this.RdbPolarizationEnable.Size = new System.Drawing.Size(53, 18);
			this.RdbPolarizationEnable.TabIndex = 1;
			this.RdbPolarizationEnable.TabStop = true;
			this.RdbPolarizationEnable.Text = "开启";
			this.RdbPolarizationEnable.UseVisualStyleBackColor = true;
			this.RdbPolarizationEnable.CheckedChanged += new System.EventHandler(RdbPolarizationEnable_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(414, 689);
			base.Controls.Add(this.GdbMainLight);
			base.Controls.Add(this.GrbOtherLightSetting);
			base.Controls.Add(this.GrbSecondLight);
			base.Controls.Add(this.GrbFocusLight);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "LightingForm";
			this.Text = "LightingForm";
			base.Load += new System.EventHandler(LightingForm_Load);
			this.GrbSecondLight.ResumeLayout(false);
			this.GrbSecondLight.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbSecondLightDuty).EndInit();
			this.GdbMainLight.ResumeLayout(false);
			this.GdbMainLight.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.TrbMainLightDuty).EndInit();
			this.GrbFocusLight.ResumeLayout(false);
			this.GrbFocusLight.PerformLayout();
			this.GrbOtherLightSetting.ResumeLayout(false);
			this.PanFarNearLightCtrl.ResumeLayout(false);
			this.PanFarNearLightCtrl.PerformLayout();
			this.PanPolarizationCfg.ResumeLayout(false);
			this.PanPolarizationCfg.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
