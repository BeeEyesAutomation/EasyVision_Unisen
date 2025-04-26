using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using 合杰图像算法调试工具;

namespace Heroje_Debug_Tool.SubForm
{
	public class AlgorithmProtocolForm : Form
	{
		private ReadingForm.SetControlsValueCB SetControlsValueFuncCB;

		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private int decode_mode;

		private bool CB_Flag = true;

		private bool is_train_press = false;

		private string btn_string_train;

		private IContainer components = null;

		private GroupBox GrbLimitBarcodeLen;

		private CheckBox ChbAztecCheckDisable;

		private CheckBox ChbIL25CheckDisable;

		private CheckBox ChbCode93CheckDisable;

		private CheckBox ChbCodabarCheckDisable;

		private CheckBox ChbCode39CheckDisable;

		private CheckBox ChbCode128CheckDisable;

		private CheckBox ChbPDF417CheckDisable;

		private CheckBox ChbDMCheckDisable;

		private CheckBox ChbQRCheckDisable;

		private Label LabSetLimitException;

		private Button BtnBarcodeLenLimitMax;

		private TextBox TxbBarcodeLenLimitMax;

		private Button BtnBarcodeLenLimitMin;

		private TextBox TxbBarcodeLenLimitMin;

		private Label LabBarcodeLenLimitMax;

		private Label LabBarcodeLenLimitMin;

		private GroupBox GrbAlgorithmSetting;

		private CheckBox ChbUseTrainPara;

		private Button BtnDecodeCTraining;

		private Button BtnSaveAndRestart;

		private RadioButton RdbDecodeModeC;

		private RadioButton RdbDecodeModeB;

		private RadioButton RdbDecodeModeA;

		private CheckBox ChkSameCodeDlyInfinite;

		private Button BtnSameCodeDelay;

		private TextBox TxbSameCodeDelay;

		private Label LabSameCodeDly;

		private Button BtnSigleImageTimeout;

		private ComboBox CobSerchLevel;

		private Label LabSerchLevel;

		private TextBox TxbSigleImageTimeout;

		private Label LabSigleDecodeTime;

		private Panel PanExpectBarcodeSetting;

		private Label LabCodeRuleDesc;

		private Panel PanDecodeMode;

		public int DecodeMode
		{
			get
			{
				if (RdbDecodeModeA.Checked)
				{
					decode_mode = 1;
				}
				else if (RdbDecodeModeB.Checked)
				{
					decode_mode = 0;
				}
				else if (RdbDecodeModeC.Checked)
				{
					decode_mode = 2;
				}
				return decode_mode;
			}
			set
			{
				decode_mode = value;
				switch (decode_mode)
				{
				case 1:
					RdbDecodeModeA.Checked = true;
					break;
				case 0:
					RdbDecodeModeB.Checked = true;
					break;
				case 2:
					RdbDecodeModeC.Checked = true;
					break;
				}
			}
		}

		public AlgorithmProtocolForm()
		{
			InitializeComponent();
		}

		private void AlgorithmProtocolForm_Load(object sender, EventArgs e)
		{
			if (ToolCfg.ConfigBarcodeCheckForm == null || ToolCfg.ConfigBarcodeCheckForm.IsDisposed)
			{
				ToolCfg.ConfigBarcodeCheckForm = new ConfigBarcodeCheck();
				ToolCfg.ConfigBarcodeCheckForm.SetCBFunc(GetCfgCBFuncCB);
			}
			else
			{
				ToolCfg.ConfigBarcodeCheckForm.SetCBFunc(GetCfgCBFuncCB);
				ToolCfg.ConfigBarcodeCheckForm.Focus();
			}
			ToolCfg.ConfigBarcodeCheckForm.TopLevel = false;
			ToolCfg.ConfigBarcodeCheckForm.FormBorderStyle = FormBorderStyle.None;
			PanExpectBarcodeSetting.Controls.Add(ToolCfg.ConfigBarcodeCheckForm);
			ToolCfg.ConfigBarcodeCheckForm.Show();
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB, ReadingForm.SetControlsValueCB setControlsValueCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
			SetControlsValueFuncCB = setControlsValueCB;
			ToolCfg.ConfigBarcodeCheckForm.SetCBFunc(GetCfgCBFuncCB);
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			if (GetCfgCBFuncCB(4992u) == 128)
			{
				if (GetCfgCBFuncCB(4991u) == 0)
				{
					ChkSameCodeDlyInfinite.Checked = true;
					TxbSameCodeDelay.Text = "∞";
				}
				else
				{
					ChkSameCodeDlyInfinite.Checked = false;
				}
			}
			else
			{
				int num = 0;
				TxbSameCodeDelay.Text = "0";
			}
			TxbSigleImageTimeout.Text = GetCfgCBFuncCB(49663u).ToString();
			if (GetCfgCBFuncCB(51504u) == 16)
			{
				CobSerchLevel.SelectedIndex = 1;
			}
			else if (GetCfgCBFuncCB(51504u) == 32)
			{
				CobSerchLevel.SelectedIndex = 2;
			}
			else
			{
				CobSerchLevel.SelectedIndex = 0;
			}
			if (GetCfgCBFuncCB(864u) == 0)
			{
				RdbDecodeModeA.Checked = true;
			}
			else if (GetCfgCBFuncCB(864u) == 64)
			{
				RdbDecodeModeC.Checked = true;
			}
			else
			{
				RdbDecodeModeB.Checked = true;
			}
			int num2 = GetCfgCBFuncCB(48127u) + GetCfgCBFuncCB(48383u) * 256;
			TxbBarcodeLenLimitMin.Text = num2.ToString();
			num2 = GetCfgCBFuncCB(48639u) + GetCfgCBFuncCB(48895u) * 256;
			TxbBarcodeLenLimitMax.Text = num2.ToString();
			ChbCode128CheckDisable.Checked = GetCfgCBFuncCB(13184u) == 0;
			ChbCode39CheckDisable.Checked = GetCfgCBFuncCB(13952u) == 0;
			ChbCode93CheckDisable.Checked = GetCfgCBFuncCB(14720u) == 0;
			ChbQRCheckDisable.Checked = GetCfgCBFuncCB(16256u) == 0;
			ChbDMCheckDisable.Checked = GetCfgCBFuncCB(21632u) == 0;
			ChbAztecCheckDisable.Checked = GetCfgCBFuncCB(22144u) == 0;
			ChbIL25CheckDisable.Checked = GetCfgCBFuncCB(16512u) == 0;
			ChbCodabarCheckDisable.Checked = GetCfgCBFuncCB(15488u) == 0;
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
			ChbUseTrainPara.Checked = GetCfgCBFuncCB(200449u) == 1;
		}

		public void UpdateLanguageUI()
		{
			int selectedIndex = CobSerchLevel.SelectedIndex;
			CobSerchLevel.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobSerchLevel.Items.AddRange(new object[3] { "正常等级", "一级精度", "二级精度" });
			}
			else
			{
				CobSerchLevel.Items.AddRange(new object[3] { "Low", "Mid", "High" });
			}
			CobSerchLevel.SelectedIndex = selectedIndex;
		}

		private void TxbSigleImageTimeout_TextChanged(object sender, EventArgs e)
		{
			BtnSigleImageTimeout.Visible = true;
		}

		private void BtnSigleImageTimeout_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbSigleImageTimeout.Text, out result) && result < 256)
				{
					SetCfgCBFuncCB(49663u, result);
					SendCfgDataCBFuncCB(0u);
					BtnSigleImageTimeout.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入小于256的数", "错误");
				}
				else
				{
					MessageBox.Show("Please enter a number less than 256", "Error");
				}
			}
		}

		private void TxbSameCodeDelay_TextChanged(object sender, EventArgs e)
		{
			BtnSameCodeDelay.Visible = true;
		}

		private void BtnSameCodeDelay_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			ushort result = 0;
			if (ushort.TryParse(TxbSameCodeDelay.Text, out result) && result < 128)
			{
				if (result == 0)
				{
					SetCfgCBFuncCB(4992u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(4992u, 128u);
					SetCfgCBFuncCB(4991u, result);
				}
				SendCfgDataCBFuncCB(0u);
				BtnSameCodeDelay.Visible = false;
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("请输入小于128的数", "错误");
			}
			else
			{
				MessageBox.Show("Please enter a number less than 128", "Error");
			}
		}

		private void ChkSameCodeDlyInfinite_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChkSameCodeDlyInfinite.Checked)
				{
					SetCfgCBFuncCB(4992u, 128u);
					SetCfgCBFuncCB(4991u, 0u);
					SendCfgDataCBFuncCB(0u);
				}
				else
				{
					SetCfgCBFuncCB(4992u, 0u);
					TxbSameCodeDelay.Text = "0";
					SendCfgDataCBFuncCB(0u);
				}
			}
		}

		private void CobSerchLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobSerchLevel.SelectedIndex == 0)
				{
					SetCfgCBFuncCB(51504u, 0u);
				}
				else if (CobSerchLevel.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(51504u, 16u);
				}
				else if (CobSerchLevel.SelectedIndex == 2)
				{
					SetCfgCBFuncCB(51504u, 32u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void BtnSaveAndRestart_Click(object sender, EventArgs e)
		{
			SendCfgDataCBFuncCB(1026u);
		}

		public void SetDecodeMode(ReadingForm.SetControlsValDef act)
		{
			CB_Flag = false;
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
			}
			CB_Flag = true;
		}

		private void RdbDecodeModeA_CheckedChanged(object sender, EventArgs e)
		{
			if (RdbDecodeModeA.Checked && !ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(864u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
			BtnDecodeCTraining.Visible = false;
			ChbUseTrainPara.Visible = false;
			if (CB_Flag)
			{
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetDecodeModeA, 1, "");
			}
		}

		private void RdbDecodeModeB_CheckedChanged(object sender, EventArgs e)
		{
			if (RdbDecodeModeB.Checked && !ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(864u, 32u);
				SendCfgDataCBFuncCB(0u);
			}
			BtnDecodeCTraining.Visible = false;
			ChbUseTrainPara.Visible = false;
			if (CB_Flag)
			{
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetDecodeModeB, 1, "");
			}
		}

		private void RdbDecodeModeC_CheckedChanged(object sender, EventArgs e)
		{
			if (RdbDecodeModeC.Checked && !ToolCfg.UpdateAdjState)
			{
				SetCfgCBFuncCB(864u, 64u);
				SendCfgDataCBFuncCB(0u);
			}
			BtnDecodeCTraining.Visible = true;
			ChbUseTrainPara.Visible = true;
			if (CB_Flag)
			{
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.SetDecodeModeC, 1, "");
			}
		}

		public void ChbUseTrainPara_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbUseTrainPara.Checked)
				{
					SetCfgCBFuncCB(200449u, 1u);
				}
				else
				{
					SetCfgCBFuncCB(200449u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
			if (CB_Flag)
			{
				if (ChbUseTrainPara.Checked)
				{
					SetControlsValueFuncCB(ReadingForm.SetControlsValDef.TrainCheck_Y, 1, "");
				}
				else
				{
					SetControlsValueFuncCB(ReadingForm.SetControlsValDef.TrainCheck_N, 1, "");
				}
			}
		}

		public string BtnDecodeCTraining_Click_Action()
		{
			if (is_train_press)
			{
				SendCfgDataCBFuncCB(2097152u);
				BtnDecodeCTraining.Text = btn_string_train;
				ChbUseTrainPara.Checked = true;
			}
			else
			{
				SendCfgDataCBFuncCB(1048576u);
				btn_string_train = BtnDecodeCTraining.Text;
				BtnDecodeCTraining.Text = "OK";
			}
			is_train_press = !is_train_press;
			return BtnDecodeCTraining.Text;
		}

		public void BtnDecodeCTraining_Click(object sender, EventArgs e)
		{
			if (is_train_press)
			{
				SendCfgDataCBFuncCB(2097152u);
				BtnDecodeCTraining.Text = btn_string_train;
				ChbUseTrainPara.Checked = true;
			}
			else
			{
				SendCfgDataCBFuncCB(1048576u);
				btn_string_train = BtnDecodeCTraining.Text;
				BtnDecodeCTraining.Text = "OK";
			}
			is_train_press = !is_train_press;
			if (CB_Flag)
			{
				SetControlsValueFuncCB(ReadingForm.SetControlsValDef.BtnTrainTextChange, 1, BtnDecodeCTraining.Text);
			}
		}

		private void TxbBarcodeLenLimitMin_TextChanged(object sender, EventArgs e)
		{
			BtnBarcodeLenLimitMin.Visible = true;
		}

		private void BtnBarcodeLenLimitMin_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbBarcodeLenLimitMin.Text, out result) && result < 4096)
				{
					SetCfgCBFuncCB(48127u, (byte)(result & 0xFFu));
					SetCfgCBFuncCB(48383u, (byte)((result & 0xFF00) >> 8));
					SendCfgDataCBFuncCB(0u);
					BtnBarcodeLenLimitMin.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入小于4096的数", "错误");
				}
				else
				{
					MessageBox.Show("Please enter a number less than 4096", "Error");
				}
			}
		}

		private void TxbBarcodeLenLimitMax_TextChanged(object sender, EventArgs e)
		{
			BtnBarcodeLenLimitMax.Visible = true;
		}

		private void BtnBarcodeLenLimitMax_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbBarcodeLenLimitMax.Text, out result) && result < 4096)
				{
					SetCfgCBFuncCB(48639u, (byte)(result & 0xFFu));
					SetCfgCBFuncCB(48895u, (byte)((result & 0xFF00) >> 8));
					SendCfgDataCBFuncCB(0u);
					BtnBarcodeLenLimitMax.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入小于4096的数", "错误");
				}
				else
				{
					MessageBox.Show("Please enter a number less than 4096", "Error");
				}
			}
		}

		private void ChbCode128CheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbCode128CheckDisable.Checked)
				{
					SetCfgCBFuncCB(13184u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(13184u, 128u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbCode39CheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbCode39CheckDisable.Checked)
				{
					SetCfgCBFuncCB(13952u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(13952u, 128u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbCode93CheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbCode93CheckDisable.Checked)
				{
					SetCfgCBFuncCB(14720u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(14720u, 128u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbIL25CheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbIL25CheckDisable.Checked)
				{
					SetCfgCBFuncCB(16512u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(16512u, 128u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbCodabarCheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbCodabarCheckDisable.Checked)
				{
					SetCfgCBFuncCB(15488u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(15488u, 128u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbAztecCheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbAztecCheckDisable.Checked)
				{
					SetCfgCBFuncCB(22144u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(22144u, 128u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbQRCheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbQRCheckDisable.Checked)
				{
					SetCfgCBFuncCB(16256u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(16256u, 128u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbDMCheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbDMCheckDisable.Checked)
				{
					SetCfgCBFuncCB(21632u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(21632u, 128u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbPDF417CheckDisable_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbPDF417CheckDisable.Checked)
				{
					SetCfgCBFuncCB(21888u, 0u);
				}
				else
				{
					SetCfgCBFuncCB(21888u, 128u);
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
			this.GrbLimitBarcodeLen = new System.Windows.Forms.GroupBox();
			this.ChbAztecCheckDisable = new System.Windows.Forms.CheckBox();
			this.ChbIL25CheckDisable = new System.Windows.Forms.CheckBox();
			this.ChbCode93CheckDisable = new System.Windows.Forms.CheckBox();
			this.ChbCodabarCheckDisable = new System.Windows.Forms.CheckBox();
			this.ChbCode39CheckDisable = new System.Windows.Forms.CheckBox();
			this.ChbCode128CheckDisable = new System.Windows.Forms.CheckBox();
			this.ChbPDF417CheckDisable = new System.Windows.Forms.CheckBox();
			this.ChbDMCheckDisable = new System.Windows.Forms.CheckBox();
			this.ChbQRCheckDisable = new System.Windows.Forms.CheckBox();
			this.LabSetLimitException = new System.Windows.Forms.Label();
			this.BtnBarcodeLenLimitMax = new System.Windows.Forms.Button();
			this.TxbBarcodeLenLimitMax = new System.Windows.Forms.TextBox();
			this.BtnBarcodeLenLimitMin = new System.Windows.Forms.Button();
			this.TxbBarcodeLenLimitMin = new System.Windows.Forms.TextBox();
			this.LabBarcodeLenLimitMax = new System.Windows.Forms.Label();
			this.LabBarcodeLenLimitMin = new System.Windows.Forms.Label();
			this.GrbAlgorithmSetting = new System.Windows.Forms.GroupBox();
			this.PanDecodeMode = new System.Windows.Forms.Panel();
			this.ChbUseTrainPara = new System.Windows.Forms.CheckBox();
			this.RdbDecodeModeA = new System.Windows.Forms.RadioButton();
			this.BtnDecodeCTraining = new System.Windows.Forms.Button();
			this.RdbDecodeModeB = new System.Windows.Forms.RadioButton();
			this.RdbDecodeModeC = new System.Windows.Forms.RadioButton();
			this.BtnSaveAndRestart = new System.Windows.Forms.Button();
			this.ChkSameCodeDlyInfinite = new System.Windows.Forms.CheckBox();
			this.BtnSameCodeDelay = new System.Windows.Forms.Button();
			this.TxbSameCodeDelay = new System.Windows.Forms.TextBox();
			this.LabSameCodeDly = new System.Windows.Forms.Label();
			this.BtnSigleImageTimeout = new System.Windows.Forms.Button();
			this.CobSerchLevel = new System.Windows.Forms.ComboBox();
			this.LabSerchLevel = new System.Windows.Forms.Label();
			this.TxbSigleImageTimeout = new System.Windows.Forms.TextBox();
			this.LabSigleDecodeTime = new System.Windows.Forms.Label();
			this.PanExpectBarcodeSetting = new System.Windows.Forms.Panel();
			this.LabCodeRuleDesc = new System.Windows.Forms.Label();
			this.GrbLimitBarcodeLen.SuspendLayout();
			this.GrbAlgorithmSetting.SuspendLayout();
			this.PanDecodeMode.SuspendLayout();
			this.PanExpectBarcodeSetting.SuspendLayout();
			base.SuspendLayout();
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbAztecCheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbIL25CheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbCode93CheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbCodabarCheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbCode39CheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbCode128CheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbPDF417CheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbDMCheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.ChbQRCheckDisable);
			this.GrbLimitBarcodeLen.Controls.Add(this.LabSetLimitException);
			this.GrbLimitBarcodeLen.Controls.Add(this.BtnBarcodeLenLimitMax);
			this.GrbLimitBarcodeLen.Controls.Add(this.TxbBarcodeLenLimitMax);
			this.GrbLimitBarcodeLen.Controls.Add(this.BtnBarcodeLenLimitMin);
			this.GrbLimitBarcodeLen.Controls.Add(this.TxbBarcodeLenLimitMin);
			this.GrbLimitBarcodeLen.Controls.Add(this.LabBarcodeLenLimitMax);
			this.GrbLimitBarcodeLen.Controls.Add(this.LabBarcodeLenLimitMin);
			this.GrbLimitBarcodeLen.Location = new System.Drawing.Point(407, 12);
			this.GrbLimitBarcodeLen.Name = "GrbLimitBarcodeLen";
			this.GrbLimitBarcodeLen.Size = new System.Drawing.Size(376, 147);
			this.GrbLimitBarcodeLen.TabIndex = 10;
			this.GrbLimitBarcodeLen.TabStop = false;
			this.GrbLimitBarcodeLen.Text = "限制条码长度";
			this.ChbAztecCheckDisable.AutoSize = true;
			this.ChbAztecCheckDisable.Location = new System.Drawing.Point(276, 92);
			this.ChbAztecCheckDisable.Name = "ChbAztecCheckDisable";
			this.ChbAztecCheckDisable.Size = new System.Drawing.Size(61, 18);
			this.ChbAztecCheckDisable.TabIndex = 42;
			this.ChbAztecCheckDisable.Text = "Aztec";
			this.ChbAztecCheckDisable.UseVisualStyleBackColor = true;
			this.ChbAztecCheckDisable.CheckedChanged += new System.EventHandler(ChbAztecCheckDisable_CheckedChanged);
			this.ChbIL25CheckDisable.AutoSize = true;
			this.ChbIL25CheckDisable.Checked = true;
			this.ChbIL25CheckDisable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbIL25CheckDisable.Location = new System.Drawing.Point(69, 92);
			this.ChbIL25CheckDisable.Name = "ChbIL25CheckDisable";
			this.ChbIL25CheckDisable.Size = new System.Drawing.Size(54, 18);
			this.ChbIL25CheckDisable.TabIndex = 41;
			this.ChbIL25CheckDisable.Text = "IL25";
			this.ChbIL25CheckDisable.UseVisualStyleBackColor = true;
			this.ChbIL25CheckDisable.CheckedChanged += new System.EventHandler(ChbIL25CheckDisable_CheckedChanged);
			this.ChbCode93CheckDisable.AutoSize = true;
			this.ChbCode93CheckDisable.Checked = true;
			this.ChbCode93CheckDisable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCode93CheckDisable.Location = new System.Drawing.Point(276, 73);
			this.ChbCode93CheckDisable.Name = "ChbCode93CheckDisable";
			this.ChbCode93CheckDisable.Size = new System.Drawing.Size(68, 18);
			this.ChbCode93CheckDisable.TabIndex = 40;
			this.ChbCode93CheckDisable.Text = "Code93";
			this.ChbCode93CheckDisable.UseVisualStyleBackColor = true;
			this.ChbCode93CheckDisable.CheckedChanged += new System.EventHandler(ChbCode93CheckDisable_CheckedChanged);
			this.ChbCodabarCheckDisable.AutoSize = true;
			this.ChbCodabarCheckDisable.Checked = true;
			this.ChbCodabarCheckDisable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCodabarCheckDisable.Location = new System.Drawing.Point(166, 92);
			this.ChbCodabarCheckDisable.Name = "ChbCodabarCheckDisable";
			this.ChbCodabarCheckDisable.Size = new System.Drawing.Size(75, 18);
			this.ChbCodabarCheckDisable.TabIndex = 39;
			this.ChbCodabarCheckDisable.Text = "CodaBar";
			this.ChbCodabarCheckDisable.UseVisualStyleBackColor = true;
			this.ChbCodabarCheckDisable.CheckedChanged += new System.EventHandler(ChbCodabarCheckDisable_CheckedChanged);
			this.ChbCode39CheckDisable.AutoSize = true;
			this.ChbCode39CheckDisable.Checked = true;
			this.ChbCode39CheckDisable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCode39CheckDisable.Location = new System.Drawing.Point(166, 74);
			this.ChbCode39CheckDisable.Name = "ChbCode39CheckDisable";
			this.ChbCode39CheckDisable.Size = new System.Drawing.Size(68, 18);
			this.ChbCode39CheckDisable.TabIndex = 38;
			this.ChbCode39CheckDisable.Text = "Code39";
			this.ChbCode39CheckDisable.UseVisualStyleBackColor = true;
			this.ChbCode39CheckDisable.CheckedChanged += new System.EventHandler(ChbCode39CheckDisable_CheckedChanged);
			this.ChbCode128CheckDisable.AutoSize = true;
			this.ChbCode128CheckDisable.Checked = true;
			this.ChbCode128CheckDisable.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChbCode128CheckDisable.Location = new System.Drawing.Point(69, 74);
			this.ChbCode128CheckDisable.Name = "ChbCode128CheckDisable";
			this.ChbCode128CheckDisable.Size = new System.Drawing.Size(75, 18);
			this.ChbCode128CheckDisable.TabIndex = 37;
			this.ChbCode128CheckDisable.Text = "Code128";
			this.ChbCode128CheckDisable.UseVisualStyleBackColor = true;
			this.ChbCode128CheckDisable.CheckedChanged += new System.EventHandler(ChbCode128CheckDisable_CheckedChanged);
			this.ChbPDF417CheckDisable.AutoSize = true;
			this.ChbPDF417CheckDisable.Location = new System.Drawing.Point(276, 109);
			this.ChbPDF417CheckDisable.Name = "ChbPDF417CheckDisable";
			this.ChbPDF417CheckDisable.Size = new System.Drawing.Size(68, 18);
			this.ChbPDF417CheckDisable.TabIndex = 36;
			this.ChbPDF417CheckDisable.Text = "PDF417";
			this.ChbPDF417CheckDisable.UseVisualStyleBackColor = true;
			this.ChbPDF417CheckDisable.CheckedChanged += new System.EventHandler(ChbPDF417CheckDisable_CheckedChanged);
			this.ChbDMCheckDisable.AutoSize = true;
			this.ChbDMCheckDisable.Location = new System.Drawing.Point(166, 109);
			this.ChbDMCheckDisable.Name = "ChbDMCheckDisable";
			this.ChbDMCheckDisable.Size = new System.Drawing.Size(96, 18);
			this.ChbDMCheckDisable.TabIndex = 35;
			this.ChbDMCheckDisable.Text = "DataMatrix";
			this.ChbDMCheckDisable.UseVisualStyleBackColor = true;
			this.ChbDMCheckDisable.CheckedChanged += new System.EventHandler(ChbDMCheckDisable_CheckedChanged);
			this.ChbQRCheckDisable.AutoSize = true;
			this.ChbQRCheckDisable.Location = new System.Drawing.Point(69, 109);
			this.ChbQRCheckDisable.Name = "ChbQRCheckDisable";
			this.ChbQRCheckDisable.Size = new System.Drawing.Size(68, 18);
			this.ChbQRCheckDisable.TabIndex = 34;
			this.ChbQRCheckDisable.Text = "QRCode";
			this.ChbQRCheckDisable.UseVisualStyleBackColor = true;
			this.ChbQRCheckDisable.CheckedChanged += new System.EventHandler(ChbQRCheckDisable_CheckedChanged);
			this.LabSetLimitException.AutoSize = true;
			this.LabSetLimitException.Location = new System.Drawing.Point(33, 52);
			this.LabSetLimitException.Name = "LabSetLimitException";
			this.LabSetLimitException.Size = new System.Drawing.Size(210, 14);
			this.LabSetLimitException.TabIndex = 25;
			this.LabSetLimitException.Text = "允许例外的码制(不受长度限制):";
			this.BtnBarcodeLenLimitMax.Location = new System.Drawing.Point(319, 24);
			this.BtnBarcodeLenLimitMax.Name = "BtnBarcodeLenLimitMax";
			this.BtnBarcodeLenLimitMax.Size = new System.Drawing.Size(49, 23);
			this.BtnBarcodeLenLimitMax.TabIndex = 23;
			this.BtnBarcodeLenLimitMax.Text = "确认";
			this.BtnBarcodeLenLimitMax.UseVisualStyleBackColor = true;
			this.BtnBarcodeLenLimitMax.Visible = false;
			this.BtnBarcodeLenLimitMax.Click += new System.EventHandler(BtnBarcodeLenLimitMax_Click);
			this.TxbBarcodeLenLimitMax.Location = new System.Drawing.Point(269, 24);
			this.TxbBarcodeLenLimitMax.Name = "TxbBarcodeLenLimitMax";
			this.TxbBarcodeLenLimitMax.Size = new System.Drawing.Size(49, 23);
			this.TxbBarcodeLenLimitMax.TabIndex = 24;
			this.TxbBarcodeLenLimitMax.TextChanged += new System.EventHandler(TxbBarcodeLenLimitMax_TextChanged);
			this.BtnBarcodeLenLimitMin.Location = new System.Drawing.Point(153, 24);
			this.BtnBarcodeLenLimitMin.Name = "BtnBarcodeLenLimitMin";
			this.BtnBarcodeLenLimitMin.Size = new System.Drawing.Size(46, 23);
			this.BtnBarcodeLenLimitMin.TabIndex = 21;
			this.BtnBarcodeLenLimitMin.Text = "确认";
			this.BtnBarcodeLenLimitMin.UseVisualStyleBackColor = true;
			this.BtnBarcodeLenLimitMin.Visible = false;
			this.BtnBarcodeLenLimitMin.Click += new System.EventHandler(BtnBarcodeLenLimitMin_Click);
			this.TxbBarcodeLenLimitMin.Location = new System.Drawing.Point(101, 24);
			this.TxbBarcodeLenLimitMin.Name = "TxbBarcodeLenLimitMin";
			this.TxbBarcodeLenLimitMin.Size = new System.Drawing.Size(51, 23);
			this.TxbBarcodeLenLimitMin.TabIndex = 21;
			this.TxbBarcodeLenLimitMin.TextChanged += new System.EventHandler(TxbBarcodeLenLimitMin_TextChanged);
			this.LabBarcodeLenLimitMax.AutoSize = true;
			this.LabBarcodeLenLimitMax.Location = new System.Drawing.Point(200, 28);
			this.LabBarcodeLenLimitMax.Name = "LabBarcodeLenLimitMax";
			this.LabBarcodeLenLimitMax.Size = new System.Drawing.Size(70, 14);
			this.LabBarcodeLenLimitMax.TabIndex = 22;
			this.LabBarcodeLenLimitMax.Text = "最大长度:";
			this.LabBarcodeLenLimitMin.AutoSize = true;
			this.LabBarcodeLenLimitMin.Location = new System.Drawing.Point(33, 28);
			this.LabBarcodeLenLimitMin.Name = "LabBarcodeLenLimitMin";
			this.LabBarcodeLenLimitMin.Size = new System.Drawing.Size(70, 14);
			this.LabBarcodeLenLimitMin.TabIndex = 21;
			this.LabBarcodeLenLimitMin.Text = "最小长度:";
			this.GrbAlgorithmSetting.Controls.Add(this.PanDecodeMode);
			this.GrbAlgorithmSetting.Controls.Add(this.ChkSameCodeDlyInfinite);
			this.GrbAlgorithmSetting.Controls.Add(this.BtnSameCodeDelay);
			this.GrbAlgorithmSetting.Controls.Add(this.TxbSameCodeDelay);
			this.GrbAlgorithmSetting.Controls.Add(this.LabSameCodeDly);
			this.GrbAlgorithmSetting.Controls.Add(this.BtnSigleImageTimeout);
			this.GrbAlgorithmSetting.Controls.Add(this.TxbSigleImageTimeout);
			this.GrbAlgorithmSetting.Controls.Add(this.LabSigleDecodeTime);
			this.GrbAlgorithmSetting.Location = new System.Drawing.Point(12, 12);
			this.GrbAlgorithmSetting.Name = "GrbAlgorithmSetting";
			this.GrbAlgorithmSetting.Size = new System.Drawing.Size(389, 147);
			this.GrbAlgorithmSetting.TabIndex = 9;
			this.GrbAlgorithmSetting.TabStop = false;
			this.GrbAlgorithmSetting.Text = "算法相关设置";
			this.PanDecodeMode.Controls.Add(this.BtnSaveAndRestart);
			this.PanDecodeMode.Controls.Add(this.ChbUseTrainPara);
			this.PanDecodeMode.Controls.Add(this.RdbDecodeModeA);
			this.PanDecodeMode.Controls.Add(this.BtnDecodeCTraining);
			this.PanDecodeMode.Controls.Add(this.RdbDecodeModeB);
			this.PanDecodeMode.Controls.Add(this.RdbDecodeModeC);
			this.PanDecodeMode.Controls.Add(this.LabSerchLevel);
			this.PanDecodeMode.Controls.Add(this.CobSerchLevel);
			this.PanDecodeMode.Location = new System.Drawing.Point(21, 88);
			this.PanDecodeMode.Name = "PanDecodeMode";
			this.PanDecodeMode.Size = new System.Drawing.Size(356, 55);
			this.PanDecodeMode.TabIndex = 13;
			this.PanDecodeMode.Visible = false;
			this.ChbUseTrainPara.AutoSize = true;
			this.ChbUseTrainPara.Location = new System.Drawing.Point(276, 34);
			this.ChbUseTrainPara.Name = "ChbUseTrainPara";
			this.ChbUseTrainPara.Size = new System.Drawing.Size(15, 14);
			this.ChbUseTrainPara.TabIndex = 53;
			this.ChbUseTrainPara.UseVisualStyleBackColor = true;
			this.ChbUseTrainPara.Visible = false;
			this.ChbUseTrainPara.CheckedChanged += new System.EventHandler(ChbUseTrainPara_CheckedChanged);
			this.RdbDecodeModeA.AutoSize = true;
			this.RdbDecodeModeA.Location = new System.Drawing.Point(7, 31);
			this.RdbDecodeModeA.Name = "RdbDecodeModeA";
			this.RdbDecodeModeA.Size = new System.Drawing.Size(88, 18);
			this.RdbDecodeModeA.TabIndex = 25;
			this.RdbDecodeModeA.Text = "A精细模式";
			this.RdbDecodeModeA.UseVisualStyleBackColor = true;
			this.RdbDecodeModeA.Visible = false;
			this.RdbDecodeModeA.CheckedChanged += new System.EventHandler(RdbDecodeModeA_CheckedChanged);
			this.BtnDecodeCTraining.Location = new System.Drawing.Point(292, 29);
			this.BtnDecodeCTraining.Name = "BtnDecodeCTraining";
			this.BtnDecodeCTraining.Size = new System.Drawing.Size(48, 23);
			this.BtnDecodeCTraining.TabIndex = 38;
			this.BtnDecodeCTraining.Text = "训练";
			this.BtnDecodeCTraining.UseVisualStyleBackColor = true;
			this.BtnDecodeCTraining.Visible = false;
			this.BtnDecodeCTraining.Click += new System.EventHandler(BtnDecodeCTraining_Click);
			this.RdbDecodeModeB.AutoSize = true;
			this.RdbDecodeModeB.Location = new System.Drawing.Point(101, 31);
			this.RdbDecodeModeB.Name = "RdbDecodeModeB";
			this.RdbDecodeModeB.Size = new System.Drawing.Size(88, 18);
			this.RdbDecodeModeB.TabIndex = 26;
			this.RdbDecodeModeB.Text = "B快速模式";
			this.RdbDecodeModeB.UseVisualStyleBackColor = true;
			this.RdbDecodeModeB.Visible = false;
			this.RdbDecodeModeB.CheckedChanged += new System.EventHandler(RdbDecodeModeB_CheckedChanged);
			this.RdbDecodeModeC.AutoSize = true;
			this.RdbDecodeModeC.Location = new System.Drawing.Point(191, 32);
			this.RdbDecodeModeC.Name = "RdbDecodeModeC";
			this.RdbDecodeModeC.Size = new System.Drawing.Size(88, 18);
			this.RdbDecodeModeC.TabIndex = 27;
			this.RdbDecodeModeC.Text = "C增强模式";
			this.RdbDecodeModeC.UseVisualStyleBackColor = true;
			this.RdbDecodeModeC.Visible = false;
			this.RdbDecodeModeC.CheckedChanged += new System.EventHandler(RdbDecodeModeC_CheckedChanged);
			this.BtnSaveAndRestart.Location = new System.Drawing.Point(192, 1);
			this.BtnSaveAndRestart.Name = "BtnSaveAndRestart";
			this.BtnSaveAndRestart.Size = new System.Drawing.Size(138, 26);
			this.BtnSaveAndRestart.TabIndex = 36;
			this.BtnSaveAndRestart.Text = "保存设置重启设备";
			this.BtnSaveAndRestart.UseVisualStyleBackColor = true;
			this.BtnSaveAndRestart.Click += new System.EventHandler(BtnSaveAndRestart_Click);
			this.ChkSameCodeDlyInfinite.AutoSize = true;
			this.ChkSameCodeDlyInfinite.Location = new System.Drawing.Point(36, 71);
			this.ChkSameCodeDlyInfinite.Name = "ChkSameCodeDlyInfinite";
			this.ChkSameCodeDlyInfinite.Size = new System.Drawing.Size(138, 18);
			this.ChkSameCodeDlyInfinite.TabIndex = 24;
			this.ChkSameCodeDlyInfinite.Text = "相同条码无限延迟";
			this.ChkSameCodeDlyInfinite.UseVisualStyleBackColor = true;
			this.ChkSameCodeDlyInfinite.CheckedChanged += new System.EventHandler(ChkSameCodeDlyInfinite_CheckedChanged);
			this.BtnSameCodeDelay.Location = new System.Drawing.Point(294, 48);
			this.BtnSameCodeDelay.Name = "BtnSameCodeDelay";
			this.BtnSameCodeDelay.Size = new System.Drawing.Size(67, 23);
			this.BtnSameCodeDelay.TabIndex = 23;
			this.BtnSameCodeDelay.Text = "确认";
			this.BtnSameCodeDelay.UseVisualStyleBackColor = true;
			this.BtnSameCodeDelay.Visible = false;
			this.BtnSameCodeDelay.Click += new System.EventHandler(BtnSameCodeDelay_Click);
			this.TxbSameCodeDelay.Location = new System.Drawing.Point(210, 48);
			this.TxbSameCodeDelay.Name = "TxbSameCodeDelay";
			this.TxbSameCodeDelay.Size = new System.Drawing.Size(78, 23);
			this.TxbSameCodeDelay.TabIndex = 22;
			this.TxbSameCodeDelay.TextChanged += new System.EventHandler(TxbSameCodeDelay_TextChanged);
			this.LabSameCodeDly.AutoSize = true;
			this.LabSameCodeDly.Location = new System.Drawing.Point(33, 51);
			this.LabSameCodeDly.Name = "LabSameCodeDly";
			this.LabSameCodeDly.Size = new System.Drawing.Size(154, 14);
			this.LabSameCodeDly.TabIndex = 21;
			this.LabSameCodeDly.Text = "相同条码延迟(100ms)：";
			this.BtnSigleImageTimeout.Location = new System.Drawing.Point(294, 24);
			this.BtnSigleImageTimeout.Name = "BtnSigleImageTimeout";
			this.BtnSigleImageTimeout.Size = new System.Drawing.Size(67, 23);
			this.BtnSigleImageTimeout.TabIndex = 20;
			this.BtnSigleImageTimeout.Text = "确认";
			this.BtnSigleImageTimeout.UseVisualStyleBackColor = true;
			this.BtnSigleImageTimeout.Visible = false;
			this.BtnSigleImageTimeout.Click += new System.EventHandler(BtnSigleImageTimeout_Click);
			this.CobSerchLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobSerchLevel.FormattingEnabled = true;
			this.CobSerchLevel.Items.AddRange(new object[3] { "正常等级", "一级精度", "二级精度" });
			this.CobSerchLevel.Location = new System.Drawing.Point(94, 3);
			this.CobSerchLevel.Name = "CobSerchLevel";
			this.CobSerchLevel.Size = new System.Drawing.Size(85, 22);
			this.CobSerchLevel.TabIndex = 19;
			this.CobSerchLevel.SelectedIndexChanged += new System.EventHandler(CobSerchLevel_SelectedIndexChanged);
			this.LabSerchLevel.AutoSize = true;
			this.LabSerchLevel.Location = new System.Drawing.Point(4, 6);
			this.LabSerchLevel.Name = "LabSerchLevel";
			this.LabSerchLevel.Size = new System.Drawing.Size(84, 14);
			this.LabSerchLevel.TabIndex = 18;
			this.LabSerchLevel.Text = "DM搜索精度:";
			this.TxbSigleImageTimeout.Location = new System.Drawing.Point(210, 24);
			this.TxbSigleImageTimeout.Name = "TxbSigleImageTimeout";
			this.TxbSigleImageTimeout.Size = new System.Drawing.Size(78, 23);
			this.TxbSigleImageTimeout.TabIndex = 1;
			this.TxbSigleImageTimeout.TextChanged += new System.EventHandler(TxbSigleImageTimeout_TextChanged);
			this.LabSigleDecodeTime.AutoSize = true;
			this.LabSigleDecodeTime.Location = new System.Drawing.Point(33, 28);
			this.LabSigleDecodeTime.Name = "LabSigleDecodeTime";
			this.LabSigleDecodeTime.Size = new System.Drawing.Size(175, 14);
			this.LabSigleDecodeTime.TabIndex = 0;
			this.LabSigleDecodeTime.Text = "单张图片解码超时(10ms)：";
			this.PanExpectBarcodeSetting.Controls.Add(this.LabCodeRuleDesc);
			this.PanExpectBarcodeSetting.Location = new System.Drawing.Point(12, 165);
			this.PanExpectBarcodeSetting.Name = "PanExpectBarcodeSetting";
			this.PanExpectBarcodeSetting.Size = new System.Drawing.Size(569, 342);
			this.PanExpectBarcodeSetting.TabIndex = 12;
			this.LabCodeRuleDesc.AutoSize = true;
			this.LabCodeRuleDesc.Location = new System.Drawing.Point(3, 11);
			this.LabCodeRuleDesc.Name = "LabCodeRuleDesc";
			this.LabCodeRuleDesc.Size = new System.Drawing.Size(245, 14);
			this.LabCodeRuleDesc.TabIndex = 0;
			this.LabCodeRuleDesc.Text = "条码匹配（页面加载时添加设置页面）";
			this.LabCodeRuleDesc.Visible = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(790, 689);
			base.Controls.Add(this.PanExpectBarcodeSetting);
			base.Controls.Add(this.GrbLimitBarcodeLen);
			base.Controls.Add(this.GrbAlgorithmSetting);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "AlgorithmProtocolForm";
			this.Text = "算法协议";
			base.Load += new System.EventHandler(AlgorithmProtocolForm_Load);
			this.GrbLimitBarcodeLen.ResumeLayout(false);
			this.GrbLimitBarcodeLen.PerformLayout();
			this.GrbAlgorithmSetting.ResumeLayout(false);
			this.GrbAlgorithmSetting.PerformLayout();
			this.PanDecodeMode.ResumeLayout(false);
			this.PanDecodeMode.PerformLayout();
			this.PanExpectBarcodeSetting.ResumeLayout(false);
			this.PanExpectBarcodeSetting.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
