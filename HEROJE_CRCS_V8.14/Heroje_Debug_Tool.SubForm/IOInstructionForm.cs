using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace Heroje_Debug_Tool.SubForm
{
	public class IOInstructionForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private string ExtriTrigStr = "None";

		private bool IsLabShowStr = true;

		private bool ParseCmdEnable = true;

		private bool IsByteArrInitOk = false;

		private TextBox[] TxbByteArr = new TextBox[7];

		private IContainer components = null;

		private GroupBox GrbOutputPinSetting;

		private Panel PanOut2OutputWay;

		private RadioButton RdbOut2OutputFailed;

		private RadioButton RdbOut2OutputSuccess;

		private Label LabOut2OutputWay;

		private Panel PanOutput2Polarity;

		private RadioButton RdbPolarityOut2Low;

		private RadioButton RdbPolarityOut2Hight;

		private Label LabPolarityOut2;

		private Panel PanOut1OutputWay;

		private RadioButton RdbOut1OutputFailed;

		private RadioButton RdbOut1OutputSuccess;

		private Label LabOut1OutputWay;

		private Panel PanOutput1Polarity;

		private RadioButton RdbPolarityOut1Low;

		private RadioButton RdbPolarityOut1Hight;

		private Label LabPolarityOut1;

		private Button BtnOutputPinTimeSet;

		private TextBox TxbOutputPinTimeSet;

		private Label LabOutputPinTimeSet;

		private GroupBox GrbIuputPinSetting;

		private CheckBox ChbExtraTrig;

		private GroupBox GrbTrigCMDSet;

		private GroupBox GrbFindHexTool;

		private Label LabDispChar;

		private Button BtnChar2Hex;

		private Button BtnHex2Char;

		private TextBox TxbFindHex;

		private Button BtnRfInfo;

		private Label LabTrigCMD;

		private TextBox TxbRfInfo;

		private CheckBox ChbRfInfo;

		private Label LabTrigCMDDisp;

		private Button BtnApply;

		private Label LabCmdParseResult;

		private TextBox TxbByte7;

		private TextBox TxbByte6;

		private TextBox TxbByte5;

		private TextBox TxbByte4;

		private TextBox TxbByte3;

		private TextBox TxbByte2;

		private TextBox TxbByte1;

		private ComboBox CobSelectLength;

		private Button BtnClearCon;

		private Label LabTriggerCode;

		private TextBox TxbTriggerCode;

		private Label LabCodeType;

		private ComboBox CobCodeType;

		private Panel PanCmdCode;

		public IOInstructionForm()
		{
			InitializeComponent();
		}

		private void IOInstructionForm_Load(object sender, EventArgs e)
		{
			ParseCmdEnable = false;
			CobSelectLength.Enabled = false;
			CobCodeType.SelectedIndex = 0;
			ParseCmdEnable = true;
		}

		public void SetCBFunc(SetCfgCB setCfgCB, GetCfgCB getCfgCB, SendCfgDataCB sendCfgDataCB)
		{
			SetCfgCBFuncCB = setCfgCB;
			GetCfgCBFuncCB = getCfgCB;
			SendCfgDataCBFuncCB = sendCfgDataCB;
		}

		public void ShowTriggerStr()
		{
			string text = ((GetCfgCBFuncCB(53360u) == 16) ? ("0x" + GetCfgCBFuncCB(58367u).ToString("X2")) : ((GetCfgCBFuncCB(53360u) == 32) ? ("0x" + GetCfgCBFuncCB(58367u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58623u).ToString("X2")) : ((GetCfgCBFuncCB(53360u) == 48) ? ("0x" + GetCfgCBFuncCB(58367u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58623u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58879u).ToString("X2")) : ((GetCfgCBFuncCB(53360u) == 64) ? ("0x" + GetCfgCBFuncCB(58367u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58623u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58879u).ToString("X2") + " 0x" + GetCfgCBFuncCB(59135u).ToString("X2")) : ((GetCfgCBFuncCB(53360u) == 80) ? ("0x" + GetCfgCBFuncCB(58367u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58623u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58879u).ToString("X2") + " 0x" + GetCfgCBFuncCB(59135u).ToString("X2") + " 0x" + GetCfgCBFuncCB(52735u).ToString("X2")) : ((GetCfgCBFuncCB(53360u) == 96) ? ("0x" + GetCfgCBFuncCB(58367u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58623u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58879u).ToString("X2") + " 0x" + GetCfgCBFuncCB(59135u).ToString("X2") + " 0x" + GetCfgCBFuncCB(52735u).ToString("X2") + " 0x" + GetCfgCBFuncCB(52991u).ToString("X2")) : ((GetCfgCBFuncCB(53360u) == 112) ? ("0x" + GetCfgCBFuncCB(58367u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58623u).ToString("X2") + " 0x" + GetCfgCBFuncCB(58879u).ToString("X2") + " 0x" + GetCfgCBFuncCB(59135u).ToString("X2") + " 0x" + GetCfgCBFuncCB(52735u).ToString("X2") + " 0x" + GetCfgCBFuncCB(52991u).ToString("X2") + " 0x" + GetCfgCBFuncCB(53247u).ToString("X2")) : ((GetCfgCBFuncCB(53250u) != 2) ? "None" : ("0x" + GetCfgCBFuncCB(53759u).ToString("X2"))))))))));
			if (!(text != ExtriTrigStr))
			{
				return;
			}
			ExtriTrigStr = text;
			if (IsLabShowStr && !text.Contains("None"))
			{
				if (Regex.IsMatch(ExtriTrigStr, "(?i)[\\da-f]{2}"))
				{
					MatchCollection matchCollection = Regex.Matches(ExtriTrigStr, "(?i)[\\da-f]{2}");
					List<byte> list = new List<byte>();
					foreach (Match item in matchCollection)
					{
						list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
					}
					LabTrigCMD.Text = Encoding.Default.GetString(list.ToArray());
				}
				else
				{
					LabTrigCMD.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.CharConvertError);
				}
			}
			else
			{
				LabTrigCMD.Text = ExtriTrigStr;
			}
		}

		public void UpdateUiDisplay(UiParaInfoStu para)
		{
			if (GetCfgCBFuncCB(24592u) == 16)
			{
				ChbRfInfo.Checked = true;
			}
			else
			{
				ChbRfInfo.Checked = false;
			}
			ChbExtraTrig.Checked = GetCfgCBFuncCB(51744u) == 32;
			List<byte> list = new List<byte>();
			list.Clear();
			for (int i = 0; i < 15; i++)
			{
				byte b = GetCfgCBFuncCB((uint)(33535uL + (ulong)(i << 8)));
				if (b == 0)
				{
					break;
				}
				list.Add(b);
			}
			TxbRfInfo.Text = Encoding.Default.GetString(list.ToArray());
			TxbOutputPinTimeSet.Text = GetCfgCBFuncCB(6911u).ToString();
			RdbPolarityOut1Hight.Checked = GetCfgCBFuncCB(3200u) == 128;
			RdbPolarityOut1Low.Checked = !RdbPolarityOut1Hight.Checked;
			RdbOut1OutputSuccess.Checked = GetCfgCBFuncCB(51776u) == 64;
			RdbOut1OutputFailed.Checked = !RdbOut1OutputSuccess.Checked;
			RdbPolarityOut2Hight.Checked = GetCfgCBFuncCB(55360u) == 64;
			RdbPolarityOut2Low.Checked = !RdbPolarityOut2Hight.Checked;
			RdbOut2OutputSuccess.Checked = GetCfgCBFuncCB(55328u) == 32;
			RdbOut2OutputFailed.Checked = !RdbOut2OutputSuccess.Checked;
			if (GetCfgCBFuncCB(53360u) == 0)
			{
				CobSelectLength.SelectedIndex = 0;
				TxbByte1.Enabled = false;
				TxbByte2.Enabled = false;
				TxbByte3.Enabled = false;
				TxbByte4.Enabled = false;
				TxbByte5.Enabled = false;
				TxbByte6.Enabled = false;
				TxbByte7.Enabled = false;
			}
			else if (GetCfgCBFuncCB(53360u) == 16)
			{
				CobSelectLength.SelectedIndex = 1;
				TxbByte1.Enabled = true;
				TxbByte2.Enabled = false;
				TxbByte3.Enabled = false;
				TxbByte4.Enabled = false;
				TxbByte5.Enabled = false;
				TxbByte6.Enabled = false;
				TxbByte7.Enabled = false;
				TxbByte1.Text = GetCfgCBFuncCB(58367u).ToString("X2");
			}
			else if (GetCfgCBFuncCB(53360u) == 32)
			{
				CobSelectLength.SelectedIndex = 2;
				TxbByte1.Enabled = true;
				TxbByte2.Enabled = true;
				TxbByte3.Enabled = false;
				TxbByte4.Enabled = false;
				TxbByte5.Enabled = false;
				TxbByte6.Enabled = false;
				TxbByte7.Enabled = false;
				TxbByte1.Text = GetCfgCBFuncCB(58367u).ToString("X2");
				TxbByte2.Text = GetCfgCBFuncCB(58623u).ToString("X2");
			}
			else if (GetCfgCBFuncCB(53360u) == 48)
			{
				CobSelectLength.SelectedIndex = 3;
				TxbByte1.Enabled = true;
				TxbByte2.Enabled = true;
				TxbByte3.Enabled = true;
				TxbByte4.Enabled = false;
				TxbByte5.Enabled = false;
				TxbByte6.Enabled = false;
				TxbByte7.Enabled = false;
				TxbByte1.Text = GetCfgCBFuncCB(58367u).ToString("X2");
				TxbByte2.Text = GetCfgCBFuncCB(58623u).ToString("X2");
				TxbByte3.Text = GetCfgCBFuncCB(58879u).ToString("X2");
			}
			else if (GetCfgCBFuncCB(53360u) == 64)
			{
				CobSelectLength.SelectedIndex = 4;
				TxbByte1.Enabled = true;
				TxbByte2.Enabled = true;
				TxbByte3.Enabled = true;
				TxbByte4.Enabled = true;
				TxbByte5.Enabled = false;
				TxbByte6.Enabled = false;
				TxbByte7.Enabled = false;
				TxbByte1.Text = GetCfgCBFuncCB(58367u).ToString("X2");
				TxbByte2.Text = GetCfgCBFuncCB(58623u).ToString("X2");
				TxbByte3.Text = GetCfgCBFuncCB(58879u).ToString("X2");
				TxbByte4.Text = GetCfgCBFuncCB(59135u).ToString("X2");
			}
			else if (GetCfgCBFuncCB(53360u) == 80)
			{
				CobSelectLength.SelectedIndex = 5;
				TxbByte1.Enabled = true;
				TxbByte2.Enabled = true;
				TxbByte3.Enabled = true;
				TxbByte4.Enabled = true;
				TxbByte5.Enabled = true;
				TxbByte6.Enabled = false;
				TxbByte7.Enabled = false;
				TxbByte1.Text = GetCfgCBFuncCB(58367u).ToString("X2");
				TxbByte2.Text = GetCfgCBFuncCB(58623u).ToString("X2");
				TxbByte3.Text = GetCfgCBFuncCB(58879u).ToString("X2");
				TxbByte4.Text = GetCfgCBFuncCB(59135u).ToString("X2");
				TxbByte5.Text = GetCfgCBFuncCB(52735u).ToString("X2");
			}
			else if (GetCfgCBFuncCB(53360u) == 96)
			{
				CobSelectLength.SelectedIndex = 6;
				TxbByte1.Enabled = true;
				TxbByte2.Enabled = true;
				TxbByte3.Enabled = true;
				TxbByte4.Enabled = true;
				TxbByte5.Enabled = true;
				TxbByte6.Enabled = true;
				TxbByte7.Enabled = false;
				TxbByte1.Text = GetCfgCBFuncCB(58367u).ToString("X2");
				TxbByte2.Text = GetCfgCBFuncCB(58623u).ToString("X2");
				TxbByte3.Text = GetCfgCBFuncCB(58879u).ToString("X2");
				TxbByte4.Text = GetCfgCBFuncCB(59135u).ToString("X2");
				TxbByte5.Text = GetCfgCBFuncCB(52735u).ToString("X2");
				TxbByte6.Text = GetCfgCBFuncCB(52991u).ToString("X2");
			}
			else if (GetCfgCBFuncCB(53360u) == 112)
			{
				CobSelectLength.SelectedIndex = 7;
				TxbByte1.Enabled = true;
				TxbByte2.Enabled = true;
				TxbByte3.Enabled = true;
				TxbByte4.Enabled = true;
				TxbByte5.Enabled = true;
				TxbByte6.Enabled = true;
				TxbByte7.Enabled = true;
				TxbByte1.Text = GetCfgCBFuncCB(58367u).ToString("X2");
				TxbByte2.Text = GetCfgCBFuncCB(58623u).ToString("X2");
				TxbByte3.Text = GetCfgCBFuncCB(58879u).ToString("X2");
				TxbByte4.Text = GetCfgCBFuncCB(59135u).ToString("X2");
				TxbByte5.Text = GetCfgCBFuncCB(52735u).ToString("X2");
				TxbByte6.Text = GetCfgCBFuncCB(52991u).ToString("X2");
				TxbByte7.Text = GetCfgCBFuncCB(53247u).ToString("X2");
			}
			else
			{
				CobSelectLength.SelectedIndex = 0;
				TxbByte1.Enabled = false;
				TxbByte2.Enabled = false;
				TxbByte3.Enabled = false;
				TxbByte4.Enabled = false;
				TxbByte5.Enabled = false;
				TxbByte6.Enabled = false;
				TxbByte7.Enabled = false;
			}
			bool flag = true;
			TxbByte1.Enabled = false;
			TxbByte2.Enabled = false;
			TxbByte3.Enabled = false;
			TxbByte4.Enabled = false;
			TxbByte5.Enabled = false;
			TxbByte6.Enabled = false;
			TxbByte7.Enabled = false;
		}

		public void UpdateLanguageUI()
		{
			int selectedIndex = CobSelectLength.SelectedIndex;
			CobSelectLength.Items.Clear();
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				CobSelectLength.Items.AddRange(new object[8] { "关闭编辑指令功能", "单个字节", "2个字节", "3个字节", "4个字节", "5个字节", "6个字节", "7个字节" });
			}
			else
			{
				CobSelectLength.Items.AddRange(new object[8] { "Close Custom CMD", "1 Byte", "2 Bytes", "3 Bytes", "4 Bytes", "5 Bytes", "6 Bytes", "7 Bytes" });
			}
			CobSelectLength.SelectedIndex = selectedIndex;
		}

		private void CobSelectLength_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void TxbByte1_TextChanged(object sender, EventArgs e)
		{
			if (TxbByte1.TextLength == 2 && TxbByte2.Enabled)
			{
				TxbByte2.Focus();
			}
		}

		private void TxbByte2_TextChanged(object sender, EventArgs e)
		{
			if (TxbByte2.TextLength == 2 && TxbByte3.Enabled)
			{
				TxbByte3.Focus();
			}
		}

		private void TxbByte3_TextChanged(object sender, EventArgs e)
		{
			if (TxbByte3.TextLength == 2 && TxbByte4.Enabled)
			{
				TxbByte4.Focus();
			}
		}

		private void TxbByte4_TextChanged(object sender, EventArgs e)
		{
			if (TxbByte4.TextLength == 2 && TxbByte5.Enabled)
			{
				TxbByte5.Focus();
			}
		}

		private void TxbByte5_TextChanged(object sender, EventArgs e)
		{
			if (TxbByte5.TextLength == 2 && TxbByte6.Enabled)
			{
				TxbByte6.Focus();
			}
		}

		private void TxbByte6_TextChanged(object sender, EventArgs e)
		{
			if (TxbByte6.TextLength == 2 && TxbByte7.Enabled)
			{
				TxbByte7.Focus();
			}
		}

		private void TxbByte7_TextChanged(object sender, EventArgs e)
		{
		}

		private void LabTrigCMDDisp_DoubleClick(object sender, EventArgs e)
		{
			IsLabShowStr = !IsLabShowStr;
			if (IsLabShowStr && !ExtriTrigStr.Contains("None"))
			{
				if (Regex.IsMatch(ExtriTrigStr, "(?i)[\\da-f]{2}"))
				{
					MatchCollection matchCollection = Regex.Matches(ExtriTrigStr, "(?i)[\\da-f]{2}");
					List<byte> list = new List<byte>();
					foreach (Match item in matchCollection)
					{
						list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
					}
					LabTrigCMD.Text = Encoding.Default.GetString(list.ToArray());
				}
				else
				{
					LabTrigCMD.Text = "转换成字符串失败";
					LabTrigCMD.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.CharConvertError);
				}
			}
			else
			{
				LabTrigCMD.Text = ExtriTrigStr;
			}
		}

		private void LabTrigCMD_DoubleClick(object sender, EventArgs e)
		{
			string value = "None";
			if (ExtriTrigStr.Contains(value))
			{
				return;
			}
			IsLabShowStr = !IsLabShowStr;
			if (IsLabShowStr && !ExtriTrigStr.Contains(value))
			{
				if (Regex.IsMatch(ExtriTrigStr, "(?i)[\\da-f]{2}"))
				{
					MatchCollection matchCollection = Regex.Matches(ExtriTrigStr, "(?i)[\\da-f]{2}");
					List<byte> list = new List<byte>();
					foreach (Match item in matchCollection)
					{
						list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
					}
					LabTrigCMD.Text = Encoding.Default.GetString(list.ToArray());
				}
				else
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CharConvertError), MultLanguageText.Get_Title(MultLanguageText.TextDef.CharConvertError));
				}
			}
			else
			{
				LabTrigCMD.Text = ExtriTrigStr;
			}
		}

		private void BtnClearCon_Click(object sender, EventArgs e)
		{
			TxbByte1.Text = "";
			TxbByte2.Text = "";
			TxbByte3.Text = "";
			TxbByte4.Text = "";
			TxbByte5.Text = "";
			TxbByte6.Text = "";
			TxbByte7.Text = "";
		}

		private void BtnApply_Click(object sender, EventArgs e)
		{
			switch (CobSelectLength.SelectedIndex)
			{
			case 0:
				SetCfgCBFuncCB(53360u, 0u);
				break;
			case 1:
				SetCfgCBFuncCB(53360u, 16u);
				break;
			case 2:
				SetCfgCBFuncCB(53360u, 32u);
				break;
			case 3:
				SetCfgCBFuncCB(53360u, 48u);
				break;
			case 4:
				SetCfgCBFuncCB(53360u, 64u);
				break;
			case 5:
				SetCfgCBFuncCB(53360u, 80u);
				break;
			case 6:
				SetCfgCBFuncCB(53360u, 96u);
				break;
			case 7:
				SetCfgCBFuncCB(53360u, 112u);
				break;
			}
			try
			{
				byte paraVal = 0;
				byte paraVal2 = 0;
				byte paraVal3 = 0;
				byte paraVal4 = 0;
				byte paraVal5 = 0;
				byte paraVal6 = 0;
				byte paraVal7 = 0;
				if (TxbByte1.Text != "")
				{
					paraVal = byte.Parse(TxbByte1.Text, NumberStyles.HexNumber);
				}
				if (TxbByte2.Text != "")
				{
					paraVal2 = byte.Parse(TxbByte2.Text, NumberStyles.HexNumber);
				}
				if (TxbByte3.Text != "")
				{
					paraVal3 = byte.Parse(TxbByte3.Text, NumberStyles.HexNumber);
				}
				if (TxbByte4.Text != "")
				{
					paraVal4 = byte.Parse(TxbByte4.Text, NumberStyles.HexNumber);
				}
				if (TxbByte5.Text != "")
				{
					paraVal5 = byte.Parse(TxbByte5.Text, NumberStyles.HexNumber);
				}
				if (TxbByte6.Text != "")
				{
					paraVal6 = byte.Parse(TxbByte6.Text, NumberStyles.HexNumber);
				}
				if (TxbByte7.Text != "")
				{
					paraVal7 = byte.Parse(TxbByte7.Text, NumberStyles.HexNumber);
				}
				SetCfgCBFuncCB(58367u, paraVal);
				SetCfgCBFuncCB(58623u, paraVal2);
				SetCfgCBFuncCB(58879u, paraVal3);
				SetCfgCBFuncCB(59135u, paraVal4);
				SetCfgCBFuncCB(52735u, paraVal5);
				SetCfgCBFuncCB(52991u, paraVal6);
				SetCfgCBFuncCB(53247u, paraVal7);
				SendCfgDataCBFuncCB(1024u);
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("保存当前设置成功。", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Save Setting OK", "Tips", MessageBoxButtons.OK);
				}
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的十六进制数值。", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Please enter the correct hexadecimal value", "Tips", MessageBoxButtons.OK);
				}
			}
		}

		private void ChbRfInfo_CheckedChanged(object sender, EventArgs e)
		{
			if (ChbRfInfo.Checked)
			{
				SetCfgCBFuncCB(24592u, 16u);
			}
			else
			{
				SetCfgCBFuncCB(24592u, 0u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void TxbRfInfo_TextChanged(object sender, EventArgs e)
		{
			BtnRfInfo.Visible = true;
		}

		private void BtnRfInfo_Click(object sender, EventArgs e)
		{
			try
			{
				string s = TxbRfInfo.Text;
				if (Encoding.Default.GetByteCount(s) > 15)
				{
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("请正确输入字符串，最长15", "提示", MessageBoxButtons.OK);
					}
					else
					{
						MessageBox.Show("Please enter the string correctly,max length is 15", "Tips", MessageBoxButtons.OK);
					}
					return;
				}
				byte[] bytes = Encoding.Default.GetBytes(s);
				int num = 0;
				byte[] array = bytes;
				foreach (byte paraVal in array)
				{
					SetCfgCBFuncCB((uint)(33535uL + (ulong)(num << 8)), paraVal);
					num++;
				}
				SetCfgCBFuncCB((uint)(33535uL + (ulong)(num << 8)), 0u);
				SendCfgDataCBFuncCB(0u);
				BtnRfInfo.Visible = false;
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请正确输入字符串，最长15", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Please enter the string correctly,max length is 15", "Tips", MessageBoxButtons.OK);
				}
			}
		}

		private void BtnChar2Hex_Click(object sender, EventArgs e)
		{
			if (TxbFindHex.TextLength == 1)
			{
				byte b = (byte)TxbFindHex.Text.ToArray()[0];
				LabDispChar.Text = b.ToString("X2");
			}
		}

		private void BtnHex2Char_Click(object sender, EventArgs e)
		{
			if (TxbFindHex.TextLength != 1 && TxbFindHex.TextLength != 2)
			{
				return;
			}
			try
			{
				byte[] bytes = new byte[1] { byte.Parse(TxbFindHex.Text, NumberStyles.HexNumber) };
				LabDispChar.Text = Encoding.Default.GetString(bytes);
			}
			catch
			{
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的十六进制数值。", "提示", MessageBoxButtons.OK);
				}
				else
				{
					MessageBox.Show("Please enter the correct hexadecimal value", "Tips", MessageBoxButtons.OK);
				}
			}
		}

		private void ChbExtraTrig_CheckedChanged(object sender, EventArgs e)
		{
			if (ChbExtraTrig.Checked)
			{
				SetCfgCBFuncCB(51744u, 32u);
			}
			else
			{
				SetCfgCBFuncCB(51744u, 0u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void TxbOutputPinTimeSet_TextChanged(object sender, EventArgs e)
		{
			BtnOutputPinTimeSet.Visible = true;
		}

		private void BtnOutputPinTimeSet_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result;
				if (ushort.TryParse(TxbOutputPinTimeSet.Text, out result) && result < 256)
				{
					SetCfgCBFuncCB(6911u, result);
					SetCfgCBFuncCB(82175u, result);
					SetCfgCBFuncCB(82431u, result);
					SendCfgDataCBFuncCB(0u);
					BtnOutputPinTimeSet.Visible = false;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的时间,小于256", "错误");
				}
				else
				{
					MessageBox.Show("Please enter the correct time value,make it < 256", "Error");
				}
			}
		}

		private void RdbPolarityOut1Hight_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbPolarityOut1Hight.Checked)
			{
				SetCfgCBFuncCB(3200u, 128u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbPolarityOut1Low_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbPolarityOut1Low.Checked)
			{
				SetCfgCBFuncCB(3200u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOut1OutputSuccess_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOut1OutputSuccess.Checked)
			{
				SetCfgCBFuncCB(51776u, 64u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOut1OutputFailed_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOut1OutputFailed.Checked)
			{
				SetCfgCBFuncCB(51776u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbPolarityOut2Hight_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbPolarityOut2Hight.Checked)
			{
				SetCfgCBFuncCB(55360u, 64u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbPolarityOut2Low_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbPolarityOut2Low.Checked)
			{
				SetCfgCBFuncCB(55360u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOut2OutputSuccess_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOut2OutputSuccess.Checked)
			{
				SetCfgCBFuncCB(55328u, 32u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOut2OutputFailed_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOut2OutputFailed.Checked)
			{
				SetCfgCBFuncCB(55328u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobCodeType_SelectedIndexChanged(object sender, EventArgs e)
		{
			ParseTriggerCode();
		}

		private void ParseTriggerCode()
		{
			if (!ParseCmdEnable)
			{
				return;
			}
			if (!IsByteArrInitOk)
			{
				TxbByteArr[0] = TxbByte1;
				TxbByteArr[1] = TxbByte2;
				TxbByteArr[2] = TxbByte3;
				TxbByteArr[3] = TxbByte4;
				TxbByteArr[4] = TxbByte5;
				TxbByteArr[5] = TxbByte6;
				TxbByteArr[6] = TxbByte7;
				IsByteArrInitOk = true;
			}
			for (int i = 0; i < TxbByteArr.Length; i++)
			{
				TxbByteArr[i].Text = "";
			}
			CobSelectLength.SelectedIndex = 0;
			if (!(TxbTriggerCode.Text.Trim() != ""))
			{
				return;
			}
			int num = 0;
			byte[] array = new byte[7];
			switch (CobCodeType.SelectedIndex)
			{
			case 0:
			{
				if (!Regex.IsMatch(TxbTriggerCode.Text.Trim(), "(?i)[\\da-f]{2}"))
				{
					return;
				}
				MatchCollection matchCollection = Regex.Matches(TxbTriggerCode.Text.Trim(), "(?i)[\\da-f]{2}");
				List<byte> list = new List<byte>();
				foreach (Match item in matchCollection)
				{
					list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
				}
				array = list.ToArray();
				break;
			}
			case 1:
				array = Encoding.Default.GetBytes(TxbTriggerCode.Text.Trim());
				break;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (j < TxbByteArr.Length)
				{
					TxbByteArr[j].Text = array[j].ToString("X2");
					CobSelectLength.SelectedIndex = j + 1;
				}
			}
		}

		private void TxbTriggerCode_TextChanged(object sender, EventArgs e)
		{
			ParseTriggerCode();
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
			this.GrbOutputPinSetting = new System.Windows.Forms.GroupBox();
			this.PanOut2OutputWay = new System.Windows.Forms.Panel();
			this.RdbOut2OutputFailed = new System.Windows.Forms.RadioButton();
			this.RdbOut2OutputSuccess = new System.Windows.Forms.RadioButton();
			this.LabOut2OutputWay = new System.Windows.Forms.Label();
			this.PanOutput2Polarity = new System.Windows.Forms.Panel();
			this.RdbPolarityOut2Low = new System.Windows.Forms.RadioButton();
			this.RdbPolarityOut2Hight = new System.Windows.Forms.RadioButton();
			this.LabPolarityOut2 = new System.Windows.Forms.Label();
			this.PanOut1OutputWay = new System.Windows.Forms.Panel();
			this.RdbOut1OutputFailed = new System.Windows.Forms.RadioButton();
			this.RdbOut1OutputSuccess = new System.Windows.Forms.RadioButton();
			this.LabOut1OutputWay = new System.Windows.Forms.Label();
			this.PanOutput1Polarity = new System.Windows.Forms.Panel();
			this.RdbPolarityOut1Low = new System.Windows.Forms.RadioButton();
			this.RdbPolarityOut1Hight = new System.Windows.Forms.RadioButton();
			this.LabPolarityOut1 = new System.Windows.Forms.Label();
			this.BtnOutputPinTimeSet = new System.Windows.Forms.Button();
			this.TxbOutputPinTimeSet = new System.Windows.Forms.TextBox();
			this.LabOutputPinTimeSet = new System.Windows.Forms.Label();
			this.GrbIuputPinSetting = new System.Windows.Forms.GroupBox();
			this.ChbExtraTrig = new System.Windows.Forms.CheckBox();
			this.GrbTrigCMDSet = new System.Windows.Forms.GroupBox();
			this.PanCmdCode = new System.Windows.Forms.Panel();
			this.LabCmdParseResult = new System.Windows.Forms.Label();
			this.TxbByte2 = new System.Windows.Forms.TextBox();
			this.TxbByte6 = new System.Windows.Forms.TextBox();
			this.TxbByte1 = new System.Windows.Forms.TextBox();
			this.TxbByte5 = new System.Windows.Forms.TextBox();
			this.CobSelectLength = new System.Windows.Forms.ComboBox();
			this.TxbByte4 = new System.Windows.Forms.TextBox();
			this.TxbByte7 = new System.Windows.Forms.TextBox();
			this.TxbByte3 = new System.Windows.Forms.TextBox();
			this.LabCodeType = new System.Windows.Forms.Label();
			this.BtnApply = new System.Windows.Forms.Button();
			this.BtnClearCon = new System.Windows.Forms.Button();
			this.CobCodeType = new System.Windows.Forms.ComboBox();
			this.GrbFindHexTool = new System.Windows.Forms.GroupBox();
			this.LabDispChar = new System.Windows.Forms.Label();
			this.BtnChar2Hex = new System.Windows.Forms.Button();
			this.BtnHex2Char = new System.Windows.Forms.Button();
			this.TxbFindHex = new System.Windows.Forms.TextBox();
			this.BtnRfInfo = new System.Windows.Forms.Button();
			this.LabTriggerCode = new System.Windows.Forms.Label();
			this.TxbTriggerCode = new System.Windows.Forms.TextBox();
			this.LabTrigCMD = new System.Windows.Forms.Label();
			this.TxbRfInfo = new System.Windows.Forms.TextBox();
			this.ChbRfInfo = new System.Windows.Forms.CheckBox();
			this.LabTrigCMDDisp = new System.Windows.Forms.Label();
			this.GrbOutputPinSetting.SuspendLayout();
			this.PanOut2OutputWay.SuspendLayout();
			this.PanOutput2Polarity.SuspendLayout();
			this.PanOut1OutputWay.SuspendLayout();
			this.PanOutput1Polarity.SuspendLayout();
			this.GrbIuputPinSetting.SuspendLayout();
			this.GrbTrigCMDSet.SuspendLayout();
			this.PanCmdCode.SuspendLayout();
			this.GrbFindHexTool.SuspendLayout();
			base.SuspendLayout();
			this.GrbOutputPinSetting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbOutputPinSetting.Controls.Add(this.PanOut2OutputWay);
			this.GrbOutputPinSetting.Controls.Add(this.PanOutput2Polarity);
			this.GrbOutputPinSetting.Controls.Add(this.PanOut1OutputWay);
			this.GrbOutputPinSetting.Controls.Add(this.PanOutput1Polarity);
			this.GrbOutputPinSetting.Controls.Add(this.BtnOutputPinTimeSet);
			this.GrbOutputPinSetting.Controls.Add(this.TxbOutputPinTimeSet);
			this.GrbOutputPinSetting.Controls.Add(this.LabOutputPinTimeSet);
			this.GrbOutputPinSetting.Location = new System.Drawing.Point(9, 348);
			this.GrbOutputPinSetting.Name = "GrbOutputPinSetting";
			this.GrbOutputPinSetting.Size = new System.Drawing.Size(395, 173);
			this.GrbOutputPinSetting.TabIndex = 56;
			this.GrbOutputPinSetting.TabStop = false;
			this.GrbOutputPinSetting.Text = "输出引脚设置";
			this.PanOut2OutputWay.Controls.Add(this.RdbOut2OutputFailed);
			this.PanOut2OutputWay.Controls.Add(this.RdbOut2OutputSuccess);
			this.PanOut2OutputWay.Controls.Add(this.LabOut2OutputWay);
			this.PanOut2OutputWay.Location = new System.Drawing.Point(33, 137);
			this.PanOut2OutputWay.Name = "PanOut2OutputWay";
			this.PanOut2OutputWay.Size = new System.Drawing.Size(353, 27);
			this.PanOut2OutputWay.TabIndex = 55;
			this.RdbOut2OutputFailed.AutoSize = true;
			this.RdbOut2OutputFailed.Location = new System.Drawing.Point(226, 5);
			this.RdbOut2OutputFailed.Name = "RdbOut2OutputFailed";
			this.RdbOut2OutputFailed.Size = new System.Drawing.Size(109, 18);
			this.RdbOut2OutputFailed.TabIndex = 2;
			this.RdbOut2OutputFailed.TabStop = true;
			this.RdbOut2OutputFailed.Text = "读码失败动作";
			this.RdbOut2OutputFailed.UseVisualStyleBackColor = true;
			this.RdbOut2OutputFailed.CheckedChanged += new System.EventHandler(RdbOut2OutputFailed_CheckedChanged);
			this.RdbOut2OutputSuccess.AutoSize = true;
			this.RdbOut2OutputSuccess.Location = new System.Drawing.Point(97, 5);
			this.RdbOut2OutputSuccess.Name = "RdbOut2OutputSuccess";
			this.RdbOut2OutputSuccess.Size = new System.Drawing.Size(109, 18);
			this.RdbOut2OutputSuccess.TabIndex = 1;
			this.RdbOut2OutputSuccess.TabStop = true;
			this.RdbOut2OutputSuccess.Text = "读码成功动作";
			this.RdbOut2OutputSuccess.UseVisualStyleBackColor = true;
			this.RdbOut2OutputSuccess.CheckedChanged += new System.EventHandler(RdbOut2OutputSuccess_CheckedChanged);
			this.LabOut2OutputWay.AutoSize = true;
			this.LabOut2OutputWay.Location = new System.Drawing.Point(2, 7);
			this.LabOut2OutputWay.Name = "LabOut2OutputWay";
			this.LabOut2OutputWay.Size = new System.Drawing.Size(98, 14);
			this.LabOut2OutputWay.TabIndex = 0;
			this.LabOut2OutputWay.Text = "输出IO2条件：";
			this.PanOutput2Polarity.Controls.Add(this.RdbPolarityOut2Low);
			this.PanOutput2Polarity.Controls.Add(this.RdbPolarityOut2Hight);
			this.PanOutput2Polarity.Controls.Add(this.LabPolarityOut2);
			this.PanOutput2Polarity.Location = new System.Drawing.Point(33, 108);
			this.PanOutput2Polarity.Name = "PanOutput2Polarity";
			this.PanOutput2Polarity.Size = new System.Drawing.Size(353, 27);
			this.PanOutput2Polarity.TabIndex = 55;
			this.RdbPolarityOut2Low.AutoSize = true;
			this.RdbPolarityOut2Low.Location = new System.Drawing.Point(226, 5);
			this.RdbPolarityOut2Low.Name = "RdbPolarityOut2Low";
			this.RdbPolarityOut2Low.Size = new System.Drawing.Size(123, 18);
			this.RdbPolarityOut2Low.TabIndex = 2;
			this.RdbPolarityOut2Low.TabStop = true;
			this.RdbPolarityOut2Low.Text = "动作时极性反相";
			this.RdbPolarityOut2Low.UseVisualStyleBackColor = true;
			this.RdbPolarityOut2Low.CheckedChanged += new System.EventHandler(RdbPolarityOut2Low_CheckedChanged);
			this.RdbPolarityOut2Hight.AutoSize = true;
			this.RdbPolarityOut2Hight.Location = new System.Drawing.Point(97, 5);
			this.RdbPolarityOut2Hight.Name = "RdbPolarityOut2Hight";
			this.RdbPolarityOut2Hight.Size = new System.Drawing.Size(123, 18);
			this.RdbPolarityOut2Hight.TabIndex = 1;
			this.RdbPolarityOut2Hight.TabStop = true;
			this.RdbPolarityOut2Hight.Text = "动作时常规极性";
			this.RdbPolarityOut2Hight.UseVisualStyleBackColor = true;
			this.RdbPolarityOut2Hight.CheckedChanged += new System.EventHandler(RdbPolarityOut2Hight_CheckedChanged);
			this.LabPolarityOut2.AutoSize = true;
			this.LabPolarityOut2.Location = new System.Drawing.Point(2, 7);
			this.LabPolarityOut2.Name = "LabPolarityOut2";
			this.LabPolarityOut2.Size = new System.Drawing.Size(98, 14);
			this.LabPolarityOut2.TabIndex = 0;
			this.LabPolarityOut2.Text = "输出IO2极性：";
			this.PanOut1OutputWay.Controls.Add(this.RdbOut1OutputFailed);
			this.PanOut1OutputWay.Controls.Add(this.RdbOut1OutputSuccess);
			this.PanOut1OutputWay.Controls.Add(this.LabOut1OutputWay);
			this.PanOut1OutputWay.Location = new System.Drawing.Point(33, 79);
			this.PanOut1OutputWay.Name = "PanOut1OutputWay";
			this.PanOut1OutputWay.Size = new System.Drawing.Size(353, 27);
			this.PanOut1OutputWay.TabIndex = 54;
			this.RdbOut1OutputFailed.AutoSize = true;
			this.RdbOut1OutputFailed.Location = new System.Drawing.Point(226, 5);
			this.RdbOut1OutputFailed.Name = "RdbOut1OutputFailed";
			this.RdbOut1OutputFailed.Size = new System.Drawing.Size(109, 18);
			this.RdbOut1OutputFailed.TabIndex = 2;
			this.RdbOut1OutputFailed.TabStop = true;
			this.RdbOut1OutputFailed.Text = "读码失败动作";
			this.RdbOut1OutputFailed.UseVisualStyleBackColor = true;
			this.RdbOut1OutputFailed.CheckedChanged += new System.EventHandler(RdbOut1OutputFailed_CheckedChanged);
			this.RdbOut1OutputSuccess.AutoSize = true;
			this.RdbOut1OutputSuccess.Location = new System.Drawing.Point(97, 5);
			this.RdbOut1OutputSuccess.Name = "RdbOut1OutputSuccess";
			this.RdbOut1OutputSuccess.Size = new System.Drawing.Size(109, 18);
			this.RdbOut1OutputSuccess.TabIndex = 1;
			this.RdbOut1OutputSuccess.TabStop = true;
			this.RdbOut1OutputSuccess.Text = "读码成功动作";
			this.RdbOut1OutputSuccess.UseVisualStyleBackColor = true;
			this.RdbOut1OutputSuccess.CheckedChanged += new System.EventHandler(RdbOut1OutputSuccess_CheckedChanged);
			this.LabOut1OutputWay.AutoSize = true;
			this.LabOut1OutputWay.Location = new System.Drawing.Point(2, 7);
			this.LabOut1OutputWay.Name = "LabOut1OutputWay";
			this.LabOut1OutputWay.Size = new System.Drawing.Size(98, 14);
			this.LabOut1OutputWay.TabIndex = 0;
			this.LabOut1OutputWay.Text = "输出IO1条件：";
			this.PanOutput1Polarity.Controls.Add(this.RdbPolarityOut1Low);
			this.PanOutput1Polarity.Controls.Add(this.RdbPolarityOut1Hight);
			this.PanOutput1Polarity.Controls.Add(this.LabPolarityOut1);
			this.PanOutput1Polarity.Location = new System.Drawing.Point(33, 50);
			this.PanOutput1Polarity.Name = "PanOutput1Polarity";
			this.PanOutput1Polarity.Size = new System.Drawing.Size(353, 27);
			this.PanOutput1Polarity.TabIndex = 53;
			this.RdbPolarityOut1Low.AutoSize = true;
			this.RdbPolarityOut1Low.Location = new System.Drawing.Point(226, 5);
			this.RdbPolarityOut1Low.Name = "RdbPolarityOut1Low";
			this.RdbPolarityOut1Low.Size = new System.Drawing.Size(123, 18);
			this.RdbPolarityOut1Low.TabIndex = 2;
			this.RdbPolarityOut1Low.TabStop = true;
			this.RdbPolarityOut1Low.Text = "动作时极性反相";
			this.RdbPolarityOut1Low.UseVisualStyleBackColor = true;
			this.RdbPolarityOut1Low.CheckedChanged += new System.EventHandler(RdbPolarityOut1Low_CheckedChanged);
			this.RdbPolarityOut1Hight.AutoSize = true;
			this.RdbPolarityOut1Hight.Location = new System.Drawing.Point(97, 5);
			this.RdbPolarityOut1Hight.Name = "RdbPolarityOut1Hight";
			this.RdbPolarityOut1Hight.Size = new System.Drawing.Size(123, 18);
			this.RdbPolarityOut1Hight.TabIndex = 1;
			this.RdbPolarityOut1Hight.TabStop = true;
			this.RdbPolarityOut1Hight.Text = "动作时常规极性";
			this.RdbPolarityOut1Hight.UseVisualStyleBackColor = true;
			this.RdbPolarityOut1Hight.CheckedChanged += new System.EventHandler(RdbPolarityOut1Hight_CheckedChanged);
			this.LabPolarityOut1.AutoSize = true;
			this.LabPolarityOut1.Location = new System.Drawing.Point(2, 7);
			this.LabPolarityOut1.Name = "LabPolarityOut1";
			this.LabPolarityOut1.Size = new System.Drawing.Size(98, 14);
			this.LabPolarityOut1.TabIndex = 0;
			this.LabPolarityOut1.Text = "输出IO1极性：";
			this.BtnOutputPinTimeSet.Location = new System.Drawing.Point(272, 24);
			this.BtnOutputPinTimeSet.Name = "BtnOutputPinTimeSet";
			this.BtnOutputPinTimeSet.Size = new System.Drawing.Size(71, 23);
			this.BtnOutputPinTimeSet.TabIndex = 52;
			this.BtnOutputPinTimeSet.Text = "确认";
			this.BtnOutputPinTimeSet.UseVisualStyleBackColor = true;
			this.BtnOutputPinTimeSet.Visible = false;
			this.BtnOutputPinTimeSet.Click += new System.EventHandler(BtnOutputPinTimeSet_Click);
			this.TxbOutputPinTimeSet.Location = new System.Drawing.Point(219, 24);
			this.TxbOutputPinTimeSet.Name = "TxbOutputPinTimeSet";
			this.TxbOutputPinTimeSet.Size = new System.Drawing.Size(47, 23);
			this.TxbOutputPinTimeSet.TabIndex = 51;
			this.TxbOutputPinTimeSet.Text = "2000";
			this.TxbOutputPinTimeSet.TextChanged += new System.EventHandler(TxbOutputPinTimeSet_TextChanged);
			this.LabOutputPinTimeSet.AutoSize = true;
			this.LabOutputPinTimeSet.Location = new System.Drawing.Point(36, 28);
			this.LabOutputPinTimeSet.Name = "LabOutputPinTimeSet";
			this.LabOutputPinTimeSet.Size = new System.Drawing.Size(175, 14);
			this.LabOutputPinTimeSet.TabIndex = 0;
			this.LabOutputPinTimeSet.Text = "输出信号持续时长(100ms):";
			this.GrbIuputPinSetting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbIuputPinSetting.Controls.Add(this.ChbExtraTrig);
			this.GrbIuputPinSetting.Location = new System.Drawing.Point(9, 286);
			this.GrbIuputPinSetting.Name = "GrbIuputPinSetting";
			this.GrbIuputPinSetting.Size = new System.Drawing.Size(395, 56);
			this.GrbIuputPinSetting.TabIndex = 55;
			this.GrbIuputPinSetting.TabStop = false;
			this.GrbIuputPinSetting.Text = "输入引脚设置";
			this.ChbExtraTrig.AutoSize = true;
			this.ChbExtraTrig.Location = new System.Drawing.Point(39, 32);
			this.ChbExtraTrig.Name = "ChbExtraTrig";
			this.ChbExtraTrig.Size = new System.Drawing.Size(306, 18);
			this.ChbExtraTrig.TabIndex = 47;
			this.ChbExtraTrig.Text = "允许外部IO触发扫码(修改在设备重启后生效)";
			this.ChbExtraTrig.UseVisualStyleBackColor = true;
			this.ChbExtraTrig.CheckedChanged += new System.EventHandler(ChbExtraTrig_CheckedChanged);
			this.GrbTrigCMDSet.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbTrigCMDSet.Controls.Add(this.PanCmdCode);
			this.GrbTrigCMDSet.Controls.Add(this.LabCodeType);
			this.GrbTrigCMDSet.Controls.Add(this.BtnApply);
			this.GrbTrigCMDSet.Controls.Add(this.BtnClearCon);
			this.GrbTrigCMDSet.Controls.Add(this.CobCodeType);
			this.GrbTrigCMDSet.Controls.Add(this.GrbFindHexTool);
			this.GrbTrigCMDSet.Controls.Add(this.BtnRfInfo);
			this.GrbTrigCMDSet.Controls.Add(this.LabTriggerCode);
			this.GrbTrigCMDSet.Controls.Add(this.TxbTriggerCode);
			this.GrbTrigCMDSet.Controls.Add(this.LabTrigCMD);
			this.GrbTrigCMDSet.Controls.Add(this.TxbRfInfo);
			this.GrbTrigCMDSet.Controls.Add(this.ChbRfInfo);
			this.GrbTrigCMDSet.Controls.Add(this.LabTrigCMDDisp);
			this.GrbTrigCMDSet.Location = new System.Drawing.Point(12, 12);
			this.GrbTrigCMDSet.Name = "GrbTrigCMDSet";
			this.GrbTrigCMDSet.Size = new System.Drawing.Size(392, 268);
			this.GrbTrigCMDSet.TabIndex = 54;
			this.GrbTrigCMDSet.TabStop = false;
			this.GrbTrigCMDSet.Text = "触发指令和失败返回";
			this.PanCmdCode.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
			this.PanCmdCode.Controls.Add(this.TxbByte2);
			this.PanCmdCode.Controls.Add(this.TxbByte6);
			this.PanCmdCode.Controls.Add(this.TxbByte1);
			this.PanCmdCode.Controls.Add(this.TxbByte5);
			this.PanCmdCode.Controls.Add(this.CobSelectLength);
			this.PanCmdCode.Controls.Add(this.TxbByte4);
			this.PanCmdCode.Controls.Add(this.TxbByte7);
			this.PanCmdCode.Controls.Add(this.TxbByte3);
			this.PanCmdCode.Controls.Add(this.LabCmdParseResult);
			this.PanCmdCode.Location = new System.Drawing.Point(30, 76);
			this.PanCmdCode.Name = "PanCmdCode";
			this.PanCmdCode.Size = new System.Drawing.Size(266, 61);
			this.PanCmdCode.TabIndex = 61;
			this.LabCmdParseResult.AutoSize = true;
			this.LabCmdParseResult.Location = new System.Drawing.Point(4, 6);
			this.LabCmdParseResult.Name = "LabCmdParseResult";
			this.LabCmdParseResult.Size = new System.Drawing.Size(98, 14);
			this.LabCmdParseResult.TabIndex = 36;
			this.LabCmdParseResult.Text = "指令解析结果:";
			this.TxbByte2.BackColor = System.Drawing.Color.White;
			this.TxbByte2.Font = new System.Drawing.Font("宋体", 10.5f);
			this.TxbByte2.Location = new System.Drawing.Point(45, 31);
			this.TxbByte2.MaxLength = 2;
			this.TxbByte2.Name = "TxbByte2";
			this.TxbByte2.Size = new System.Drawing.Size(33, 23);
			this.TxbByte2.TabIndex = 25;
			this.TxbByte2.TextChanged += new System.EventHandler(TxbByte2_TextChanged);
			this.TxbByte6.BackColor = System.Drawing.Color.White;
			this.TxbByte6.Location = new System.Drawing.Point(185, 31);
			this.TxbByte6.MaxLength = 2;
			this.TxbByte6.Name = "TxbByte6";
			this.TxbByte6.Size = new System.Drawing.Size(33, 23);
			this.TxbByte6.TabIndex = 33;
			this.TxbByte6.TextChanged += new System.EventHandler(TxbByte6_TextChanged);
			this.TxbByte1.BackColor = System.Drawing.Color.White;
			this.TxbByte1.Font = new System.Drawing.Font("宋体", 10.5f);
			this.TxbByte1.Location = new System.Drawing.Point(10, 31);
			this.TxbByte1.MaxLength = 2;
			this.TxbByte1.Name = "TxbByte1";
			this.TxbByte1.Size = new System.Drawing.Size(33, 23);
			this.TxbByte1.TabIndex = 23;
			this.TxbByte1.TextChanged += new System.EventHandler(TxbByte1_TextChanged);
			this.TxbByte5.BackColor = System.Drawing.Color.White;
			this.TxbByte5.Location = new System.Drawing.Point(150, 31);
			this.TxbByte5.MaxLength = 2;
			this.TxbByte5.Name = "TxbByte5";
			this.TxbByte5.Size = new System.Drawing.Size(33, 23);
			this.TxbByte5.TabIndex = 31;
			this.TxbByte5.TextChanged += new System.EventHandler(TxbByte5_TextChanged);
			this.CobSelectLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobSelectLength.FormattingEnabled = true;
			this.CobSelectLength.Items.AddRange(new object[8] { "关闭编辑指令功能", "单个字节", "2个字节", "3个字节", "4个字节", "5个字节", "6个字节", "7个字节" });
			this.CobSelectLength.Location = new System.Drawing.Point(122, 3);
			this.CobSelectLength.Name = "CobSelectLength";
			this.CobSelectLength.Size = new System.Drawing.Size(143, 22);
			this.CobSelectLength.TabIndex = 21;
			this.CobSelectLength.SelectedIndexChanged += new System.EventHandler(CobSelectLength_SelectedIndexChanged);
			this.TxbByte4.BackColor = System.Drawing.Color.White;
			this.TxbByte4.Location = new System.Drawing.Point(115, 31);
			this.TxbByte4.MaxLength = 2;
			this.TxbByte4.Name = "TxbByte4";
			this.TxbByte4.Size = new System.Drawing.Size(33, 23);
			this.TxbByte4.TabIndex = 29;
			this.TxbByte4.TextChanged += new System.EventHandler(TxbByte4_TextChanged);
			this.TxbByte7.BackColor = System.Drawing.Color.White;
			this.TxbByte7.Location = new System.Drawing.Point(220, 31);
			this.TxbByte7.MaxLength = 2;
			this.TxbByte7.Name = "TxbByte7";
			this.TxbByte7.Size = new System.Drawing.Size(33, 23);
			this.TxbByte7.TabIndex = 35;
			this.TxbByte7.TextChanged += new System.EventHandler(TxbByte7_TextChanged);
			this.TxbByte3.BackColor = System.Drawing.Color.White;
			this.TxbByte3.Location = new System.Drawing.Point(80, 31);
			this.TxbByte3.MaxLength = 2;
			this.TxbByte3.Name = "TxbByte3";
			this.TxbByte3.Size = new System.Drawing.Size(33, 23);
			this.TxbByte3.TabIndex = 27;
			this.TxbByte3.TextChanged += new System.EventHandler(TxbByte3_TextChanged);
			this.LabCodeType.AutoSize = true;
			this.LabCodeType.ForeColor = System.Drawing.Color.Blue;
			this.LabCodeType.Location = new System.Drawing.Point(299, 28);
			this.LabCodeType.Name = "LabCodeType";
			this.LabCodeType.Size = new System.Drawing.Size(63, 14);
			this.LabCodeType.TabIndex = 60;
			this.LabCodeType.Text = "指令类型";
			this.BtnApply.Location = new System.Drawing.Point(301, 106);
			this.BtnApply.Name = "BtnApply";
			this.BtnApply.Size = new System.Drawing.Size(75, 23);
			this.BtnApply.TabIndex = 19;
			this.BtnApply.Text = "应用配置";
			this.BtnApply.UseVisualStyleBackColor = true;
			this.BtnApply.Click += new System.EventHandler(BtnApply_Click);
			this.BtnClearCon.Location = new System.Drawing.Point(301, 77);
			this.BtnClearCon.Name = "BtnClearCon";
			this.BtnClearCon.Size = new System.Drawing.Size(75, 23);
			this.BtnClearCon.TabIndex = 20;
			this.BtnClearCon.Text = "清除配置";
			this.BtnClearCon.UseVisualStyleBackColor = true;
			this.BtnClearCon.Click += new System.EventHandler(BtnClearCon_Click);
			this.CobCodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobCodeType.FormattingEnabled = true;
			this.CobCodeType.Items.AddRange(new object[2] { "十六进制", "字符串" });
			this.CobCodeType.Location = new System.Drawing.Point(302, 47);
			this.CobCodeType.Name = "CobCodeType";
			this.CobCodeType.Size = new System.Drawing.Size(83, 22);
			this.CobCodeType.TabIndex = 59;
			this.CobCodeType.SelectedIndexChanged += new System.EventHandler(CobCodeType_SelectedIndexChanged);
			this.GrbFindHexTool.Controls.Add(this.LabDispChar);
			this.GrbFindHexTool.Controls.Add(this.BtnChar2Hex);
			this.GrbFindHexTool.Controls.Add(this.BtnHex2Char);
			this.GrbFindHexTool.Controls.Add(this.TxbFindHex);
			this.GrbFindHexTool.Location = new System.Drawing.Point(30, 213);
			this.GrbFindHexTool.Name = "GrbFindHexTool";
			this.GrbFindHexTool.Size = new System.Drawing.Size(336, 49);
			this.GrbFindHexTool.TabIndex = 57;
			this.GrbFindHexTool.TabStop = false;
			this.GrbFindHexTool.Text = "查询工具";
			this.LabDispChar.AutoSize = true;
			this.LabDispChar.Location = new System.Drawing.Point(64, 22);
			this.LabDispChar.Name = "LabDispChar";
			this.LabDispChar.Size = new System.Drawing.Size(14, 14);
			this.LabDispChar.TabIndex = 21;
			this.LabDispChar.Text = " ";
			this.BtnChar2Hex.Location = new System.Drawing.Point(85, 18);
			this.BtnChar2Hex.Name = "BtnChar2Hex";
			this.BtnChar2Hex.Size = new System.Drawing.Size(118, 23);
			this.BtnChar2Hex.TabIndex = 20;
			this.BtnChar2Hex.Text = "字符转十六进制";
			this.BtnChar2Hex.UseVisualStyleBackColor = true;
			this.BtnChar2Hex.Click += new System.EventHandler(BtnChar2Hex_Click);
			this.BtnHex2Char.Location = new System.Drawing.Point(209, 18);
			this.BtnHex2Char.Name = "BtnHex2Char";
			this.BtnHex2Char.Size = new System.Drawing.Size(123, 23);
			this.BtnHex2Char.TabIndex = 19;
			this.BtnHex2Char.Text = "十六进制转字符";
			this.BtnHex2Char.UseVisualStyleBackColor = true;
			this.BtnHex2Char.Click += new System.EventHandler(BtnHex2Char_Click);
			this.TxbFindHex.Location = new System.Drawing.Point(9, 18);
			this.TxbFindHex.MaxLength = 2;
			this.TxbFindHex.Name = "TxbFindHex";
			this.TxbFindHex.Size = new System.Drawing.Size(47, 23);
			this.TxbFindHex.TabIndex = 0;
			this.BtnRfInfo.Location = new System.Drawing.Point(294, 174);
			this.BtnRfInfo.Name = "BtnRfInfo";
			this.BtnRfInfo.Size = new System.Drawing.Size(72, 23);
			this.BtnRfInfo.TabIndex = 55;
			this.BtnRfInfo.Text = "确认";
			this.BtnRfInfo.UseVisualStyleBackColor = true;
			this.BtnRfInfo.Visible = false;
			this.BtnRfInfo.Click += new System.EventHandler(BtnRfInfo_Click);
			this.LabTriggerCode.AutoSize = true;
			this.LabTriggerCode.ForeColor = System.Drawing.Color.Blue;
			this.LabTriggerCode.Location = new System.Drawing.Point(27, 28);
			this.LabTriggerCode.Name = "LabTriggerCode";
			this.LabTriggerCode.Size = new System.Drawing.Size(224, 14);
			this.LabTriggerCode.TabIndex = 57;
			this.LabTriggerCode.Text = "设置触发指令（最多设置7个字节）";
			this.TxbTriggerCode.BackColor = System.Drawing.Color.White;
			this.TxbTriggerCode.Location = new System.Drawing.Point(30, 47);
			this.TxbTriggerCode.Name = "TxbTriggerCode";
			this.TxbTriggerCode.Size = new System.Drawing.Size(266, 23);
			this.TxbTriggerCode.TabIndex = 58;
			this.TxbTriggerCode.TextChanged += new System.EventHandler(TxbTriggerCode_TextChanged);
			this.LabTrigCMD.AutoSize = true;
			this.LabTrigCMD.Location = new System.Drawing.Point(133, 149);
			this.LabTrigCMD.Name = "LabTrigCMD";
			this.LabTrigCMD.Size = new System.Drawing.Size(35, 14);
			this.LabTrigCMD.TabIndex = 39;
			this.LabTrigCMD.Text = "None";
			this.LabTrigCMD.DoubleClick += new System.EventHandler(LabTrigCMD_DoubleClick);
			this.TxbRfInfo.Location = new System.Drawing.Point(159, 174);
			this.TxbRfInfo.MaxLength = 15;
			this.TxbRfInfo.Name = "TxbRfInfo";
			this.TxbRfInfo.Size = new System.Drawing.Size(133, 23);
			this.TxbRfInfo.TabIndex = 54;
			this.TxbRfInfo.TextChanged += new System.EventHandler(TxbRfInfo_TextChanged);
			this.ChbRfInfo.AutoSize = true;
			this.ChbRfInfo.Location = new System.Drawing.Point(30, 177);
			this.ChbRfInfo.Name = "ChbRfInfo";
			this.ChbRfInfo.Size = new System.Drawing.Size(117, 18);
			this.ChbRfInfo.TabIndex = 56;
			this.ChbRfInfo.Text = "读码失败返回:";
			this.ChbRfInfo.UseVisualStyleBackColor = true;
			this.ChbRfInfo.CheckedChanged += new System.EventHandler(ChbRfInfo_CheckedChanged);
			this.LabTrigCMDDisp.AutoSize = true;
			this.LabTrigCMDDisp.Location = new System.Drawing.Point(27, 149);
			this.LabTrigCMDDisp.Name = "LabTrigCMDDisp";
			this.LabTrigCMDDisp.Size = new System.Drawing.Size(98, 14);
			this.LabTrigCMDDisp.TabIndex = 38;
			this.LabTrigCMDDisp.Text = "触发指令内容:";
			this.LabTrigCMDDisp.DoubleClick += new System.EventHandler(LabTrigCMDDisp_DoubleClick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(735, 689);
			base.Controls.Add(this.GrbOutputPinSetting);
			base.Controls.Add(this.GrbIuputPinSetting);
			base.Controls.Add(this.GrbTrigCMDSet);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "IOInstructionForm";
			this.Text = "IOInstructionForm";
			base.Load += new System.EventHandler(IOInstructionForm_Load);
			this.GrbOutputPinSetting.ResumeLayout(false);
			this.GrbOutputPinSetting.PerformLayout();
			this.PanOut2OutputWay.ResumeLayout(false);
			this.PanOut2OutputWay.PerformLayout();
			this.PanOutput2Polarity.ResumeLayout(false);
			this.PanOutput2Polarity.PerformLayout();
			this.PanOut1OutputWay.ResumeLayout(false);
			this.PanOut1OutputWay.PerformLayout();
			this.PanOutput1Polarity.ResumeLayout(false);
			this.PanOutput1Polarity.PerformLayout();
			this.GrbIuputPinSetting.ResumeLayout(false);
			this.GrbIuputPinSetting.PerformLayout();
			this.GrbTrigCMDSet.ResumeLayout(false);
			this.GrbTrigCMDSet.PerformLayout();
			this.PanCmdCode.ResumeLayout(false);
			this.PanCmdCode.PerformLayout();
			this.GrbFindHexTool.ResumeLayout(false);
			this.GrbFindHexTool.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
