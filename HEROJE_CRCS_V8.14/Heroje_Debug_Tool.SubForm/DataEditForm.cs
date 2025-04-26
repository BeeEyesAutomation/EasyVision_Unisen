using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;

namespace Heroje_Debug_Tool.SubForm
{
	public class DataEditForm : Form
	{
		private SetCfgCB SetCfgCBFuncCB;

		private GetCfgCB GetCfgCBFuncCB;

		private SendCfgDataCB SendCfgDataCBFuncCB;

		private IContainer components = null;

		private GroupBox GrbExtraDataOutput;

		private ComboBox CobOutputBarcodeRect;

		private Label LabOutputBarcodeRect;

		private GroupBox GrbMuiltCodeSplit;

		private Button BtnMuiltCodeSplit;

		private TextBox TxbMuiltCodeSplit;

		private CheckBox ChkMuiltCodeSplitSameEof;

		private Label LabMuiltCodeSplit;

		private GroupBox GrbBarcodeIDAndEof;

		private CheckBox ChbBarcodeIDOut;

		private ComboBox CobStringEof;

		private Label LabStringEof;

		private GroupBox GrbDataCropSetting;

		private RadioButton RdbOnlyEndOutput;

		private RadioButton RdbOnlyCenterOutput;

		private RadioButton RdbOnlyFrontOutput;

		private RadioButton RdbAllDataOutput;

		private Button BtnEndSessionSet;

		private TextBox TxbEndSessionSet;

		private Button BtnFrontSessionSet;

		private TextBox TxbFrontSessionSet;

		private Label LabEndSessionSet;

		private Label LabFrontSessionSet;

		private GroupBox GrbPrefixAndSuffix;

		private CheckBox ChbDataSuffix;

		private CheckBox ChbDataPrefix;

		private Button BtnDataSuffix;

		private TextBox TxbDataSuffix;

		private Button BtnDataPrefix;

		private TextBox TxbDataPrefix;

		private Button BtnTemplateIdxSplit;

		private TextBox TxbTemplateIdxSplit;

		private Label LabTemplateIdxSplit;

		private RadioButton RdbTemplateIdxSuffix;

		private RadioButton RdbTemplateIdxPrefix;

		private RadioButton RdbTemplateIdxNoOut;

		private Label LabTemplateIdxOut;

		public DataEditForm()
		{
			InitializeComponent();
		}

		private void DataEditForm_Load(object sender, EventArgs e)
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
			if (GetCfgCBFuncCB(24584u) == 8)
			{
				ChbDataPrefix.Checked = true;
			}
			else
			{
				ChbDataPrefix.Checked = false;
			}
			if (GetCfgCBFuncCB(24578u) == 2)
			{
				ChbDataSuffix.Checked = true;
			}
			else
			{
				ChbDataSuffix.Checked = false;
			}
			if (GetCfgCBFuncCB(24672u) == 0)
			{
				CobStringEof.SelectedIndex = 1;
			}
			else if (GetCfgCBFuncCB(24672u) == 64)
			{
				CobStringEof.SelectedIndex = 2;
			}
			else if (GetCfgCBFuncCB(24672u) == 32)
			{
				CobStringEof.SelectedIndex = 3;
			}
			else
			{
				CobStringEof.SelectedIndex = 0;
			}
			List<byte> list = new List<byte>();
			for (int i = 0; i < 15; i++)
			{
				byte b = GetCfgCBFuncCB((uint)(25599uL + (ulong)(i << 8)));
				if (b == 0)
				{
					break;
				}
				list.Add(b);
			}
			TxbDataPrefix.Text = Encoding.Default.GetString(list.ToArray());
			list.Clear();
			for (int j = 0; j < 15; j++)
			{
				byte b2 = GetCfgCBFuncCB((uint)(29439uL + (ulong)(j << 8)));
				if (b2 == 0)
				{
					break;
				}
				list.Add(b2);
			}
			TxbDataSuffix.Text = Encoding.Default.GetString(list.ToArray());
			TxbFrontSessionSet.Text = GetCfgCBFuncCB(45567u).ToString();
			TxbEndSessionSet.Text = GetCfgCBFuncCB(45823u).ToString();
			if (GetCfgCBFuncCB(45059u) == 0)
			{
				RdbAllDataOutput.Checked = true;
			}
			else if (GetCfgCBFuncCB(45059u) == 1)
			{
				RdbOnlyFrontOutput.Checked = true;
			}
			else if (GetCfgCBFuncCB(45059u) == 3)
			{
				RdbOnlyCenterOutput.Checked = true;
			}
			else
			{
				RdbOnlyEndOutput.Checked = true;
			}
			ChbBarcodeIDOut.Checked = GetCfgCBFuncCB(24580u) == 4;
			if (para.ParaDataLen >= 4096)
			{
				if (GetCfgCBFuncCB(140287u) == 13)
				{
					ChkMuiltCodeSplitSameEof.Checked = true;
					TxbMuiltCodeSplit.Text = "";
				}
				else
				{
					ChkMuiltCodeSplitSameEof.Checked = false;
					TxbMuiltCodeSplit.Text = GetCfgCBFuncCB(140287u).ToString("X2");
				}
				if (GetCfgCBFuncCB(201731u) == 1)
				{
					RdbTemplateIdxPrefix.Checked = true;
				}
				else if (GetCfgCBFuncCB(201731u) == 2)
				{
					RdbTemplateIdxSuffix.Checked = true;
				}
				else
				{
					RdbTemplateIdxNoOut.Checked = true;
				}
				TxbTemplateIdxSplit.Text = GetCfgCBFuncCB(202239u).ToString("X2");
			}
			if (para.DeviceTypeRecord == 6 || para.DeviceTypeRecord == 9 || para.DeviceTypeRecord == 16 || para.DeviceTypeRecord == 10 || para.DeviceTypeRecord == 19 || para.DeviceTypeRecord == 13)
			{
				GrbExtraDataOutput.Visible = true;
				if (GetCfgCBFuncCB(204803u) == 1)
				{
					CobOutputBarcodeRect.SelectedIndex = 1;
				}
				else if (GetCfgCBFuncCB(204803u) == 2)
				{
					CobOutputBarcodeRect.SelectedIndex = 2;
				}
				else if (GetCfgCBFuncCB(204803u) == 3)
				{
					CobOutputBarcodeRect.SelectedIndex = 3;
				}
				else
				{
					CobOutputBarcodeRect.SelectedIndex = 0;
				}
			}
		}

		private void ChbDataPrefix_CheckedChanged(object sender, EventArgs e)
		{
			if (ChbDataPrefix.Checked)
			{
				SetCfgCBFuncCB(24584u, 8u);
			}
			else
			{
				SetCfgCBFuncCB(24584u, 0u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void TxbDataPrefix_TextChanged(object sender, EventArgs e)
		{
			BtnDataPrefix.Visible = true;
		}

		private void BtnDataPrefix_Click(object sender, EventArgs e)
		{
			try
			{
				string s = TxbDataPrefix.Text;
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
					SetCfgCBFuncCB((uint)(25599uL + (ulong)(num << 8)), paraVal);
					num++;
				}
				SetCfgCBFuncCB((uint)(25599uL + (ulong)(num << 8)), 0u);
				SendCfgDataCBFuncCB(0u);
				BtnDataPrefix.Visible = false;
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

		private void ChbDataSuffix_CheckedChanged(object sender, EventArgs e)
		{
			if (ChbDataSuffix.Checked)
			{
				SetCfgCBFuncCB(24578u, 2u);
			}
			else
			{
				SetCfgCBFuncCB(24578u, 0u);
			}
			SendCfgDataCBFuncCB(0u);
		}

		private void TxbDataSuffix_TextChanged(object sender, EventArgs e)
		{
			BtnDataSuffix.Visible = true;
		}

		private void BtnDataSuffix_Click(object sender, EventArgs e)
		{
			try
			{
				string s = TxbDataSuffix.Text;
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
					SetCfgCBFuncCB((uint)(29439uL + (ulong)(num << 8)), paraVal);
					num++;
				}
				SetCfgCBFuncCB((uint)(29439uL + (ulong)(num << 8)), 0u);
				SendCfgDataCBFuncCB(0u);
				BtnDataSuffix.Visible = false;
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

		private void TxbFrontSessionSet_TextChanged(object sender, EventArgs e)
		{
			BtnFrontSessionSet.Visible = true;
		}

		private void BtnFrontSessionSet_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbFrontSessionSet.Text, out result) && result < 256)
				{
					SetCfgCBFuncCB(45567u, (byte)result);
					SendCfgDataCBFuncCB(0u);
					BtnFrontSessionSet.Visible = false;
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

		private void TxbEndSessionSet_TextChanged(object sender, EventArgs e)
		{
			BtnEndSessionSet.Visible = true;
		}

		private void BtnEndSessionSet_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				ushort result = 0;
				if (ushort.TryParse(TxbEndSessionSet.Text, out result) && result < 256)
				{
					SetCfgCBFuncCB(45823u, (byte)result);
					SendCfgDataCBFuncCB(0u);
					BtnEndSessionSet.Visible = false;
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

		private void RdbAllDataOutput_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbAllDataOutput.Checked)
			{
				SetCfgCBFuncCB(45059u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOnlyFrontOutput_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOnlyFrontOutput.Checked)
			{
				SetCfgCBFuncCB(45059u, 1u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOnlyCenterOutput_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOnlyCenterOutput.Checked)
			{
				SetCfgCBFuncCB(45059u, 3u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbOnlyEndOutput_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbOnlyEndOutput.Checked)
			{
				SetCfgCBFuncCB(45059u, 2u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChbBarcodeIDOut_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChbBarcodeIDOut.Checked)
				{
					SetCfgCBFuncCB(24580u, 4u);
				}
				else
				{
					SetCfgCBFuncCB(24580u, 0u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void CobStringEof_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobStringEof.SelectedIndex == 0)
				{
					SetCfgCBFuncCB(24672u, 96u);
				}
				else if (CobStringEof.SelectedIndex == 1)
				{
					SetCfgCBFuncCB(24672u, 0u);
				}
				else if (CobStringEof.SelectedIndex == 2)
				{
					SetCfgCBFuncCB(24672u, 64u);
				}
				else if (CobStringEof.SelectedIndex == 3)
				{
					SetCfgCBFuncCB(24672u, 32u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void ChkMuiltCodeSplitSameEof_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (ChkMuiltCodeSplitSameEof.Checked)
				{
					SetCfgCBFuncCB(140287u, 13u);
				}
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbMuiltCodeSplit_TextChanged(object sender, EventArgs e)
		{
			BtnMuiltCodeSplit.Visible = true;
		}

		private void BtnMuiltCodeSplit_Click(object sender, EventArgs e)
		{
			if (ToolCfg.UpdateAdjState)
			{
				return;
			}
			byte result = 0;
			if (byte.TryParse(TxbMuiltCodeSplit.Text, NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-GB"), out result))
			{
				if (result == 13)
				{
					ChkMuiltCodeSplitSameEof.Checked = true;
				}
				else
				{
					SetCfgCBFuncCB(140287u, result);
					SendCfgDataCBFuncCB(0u);
				}
				BtnMuiltCodeSplit.Visible = false;
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

		private void RdbTemplateIdxNoOut_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbTemplateIdxNoOut.Checked)
			{
				SetCfgCBFuncCB(201731u, 0u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbTemplateIdxPrefix_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbTemplateIdxPrefix.Checked)
			{
				SetCfgCBFuncCB(201731u, 1u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void RdbTemplateIdxSuffix_CheckedChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState && RdbTemplateIdxSuffix.Checked)
			{
				SetCfgCBFuncCB(201731u, 2u);
				SendCfgDataCBFuncCB(0u);
			}
		}

		private void TxbTemplateIdxSplit_TextChanged(object sender, EventArgs e)
		{
			BtnTemplateIdxSplit.Visible = true;
		}

		private void BtnTemplateIdxSplit_Click(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				byte result = 0;
				if (byte.TryParse(TxbTemplateIdxSplit.Text, NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-GB"), out result))
				{
					SetCfgCBFuncCB(202239u, result);
					SendCfgDataCBFuncCB(0u);
					BtnTemplateIdxSplit.Visible = false;
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

		private void CobOutputBarcodeRect_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!ToolCfg.UpdateAdjState)
			{
				if (CobOutputBarcodeRect.SelectedIndex < 0)
				{
					CobOutputBarcodeRect.SelectedIndex = 0;
				}
				switch (CobOutputBarcodeRect.SelectedIndex)
				{
				case 0:
					SetCfgCBFuncCB(204803u, 0u);
					break;
				case 1:
					SetCfgCBFuncCB(204803u, 1u);
					break;
				case 2:
					SetCfgCBFuncCB(204803u, 2u);
					break;
				case 3:
					SetCfgCBFuncCB(204803u, 3u);
					break;
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
			this.GrbExtraDataOutput = new System.Windows.Forms.GroupBox();
			this.CobOutputBarcodeRect = new System.Windows.Forms.ComboBox();
			this.LabOutputBarcodeRect = new System.Windows.Forms.Label();
			this.GrbMuiltCodeSplit = new System.Windows.Forms.GroupBox();
			this.BtnTemplateIdxSplit = new System.Windows.Forms.Button();
			this.TxbTemplateIdxSplit = new System.Windows.Forms.TextBox();
			this.LabTemplateIdxSplit = new System.Windows.Forms.Label();
			this.RdbTemplateIdxSuffix = new System.Windows.Forms.RadioButton();
			this.RdbTemplateIdxPrefix = new System.Windows.Forms.RadioButton();
			this.RdbTemplateIdxNoOut = new System.Windows.Forms.RadioButton();
			this.LabTemplateIdxOut = new System.Windows.Forms.Label();
			this.BtnMuiltCodeSplit = new System.Windows.Forms.Button();
			this.TxbMuiltCodeSplit = new System.Windows.Forms.TextBox();
			this.ChkMuiltCodeSplitSameEof = new System.Windows.Forms.CheckBox();
			this.LabMuiltCodeSplit = new System.Windows.Forms.Label();
			this.GrbBarcodeIDAndEof = new System.Windows.Forms.GroupBox();
			this.ChbBarcodeIDOut = new System.Windows.Forms.CheckBox();
			this.CobStringEof = new System.Windows.Forms.ComboBox();
			this.LabStringEof = new System.Windows.Forms.Label();
			this.GrbDataCropSetting = new System.Windows.Forms.GroupBox();
			this.RdbOnlyEndOutput = new System.Windows.Forms.RadioButton();
			this.RdbOnlyCenterOutput = new System.Windows.Forms.RadioButton();
			this.RdbOnlyFrontOutput = new System.Windows.Forms.RadioButton();
			this.RdbAllDataOutput = new System.Windows.Forms.RadioButton();
			this.BtnEndSessionSet = new System.Windows.Forms.Button();
			this.TxbEndSessionSet = new System.Windows.Forms.TextBox();
			this.BtnFrontSessionSet = new System.Windows.Forms.Button();
			this.TxbFrontSessionSet = new System.Windows.Forms.TextBox();
			this.LabEndSessionSet = new System.Windows.Forms.Label();
			this.LabFrontSessionSet = new System.Windows.Forms.Label();
			this.GrbPrefixAndSuffix = new System.Windows.Forms.GroupBox();
			this.BtnDataSuffix = new System.Windows.Forms.Button();
			this.TxbDataSuffix = new System.Windows.Forms.TextBox();
			this.BtnDataPrefix = new System.Windows.Forms.Button();
			this.TxbDataPrefix = new System.Windows.Forms.TextBox();
			this.ChbDataPrefix = new System.Windows.Forms.CheckBox();
			this.ChbDataSuffix = new System.Windows.Forms.CheckBox();
			this.GrbExtraDataOutput.SuspendLayout();
			this.GrbMuiltCodeSplit.SuspendLayout();
			this.GrbBarcodeIDAndEof.SuspendLayout();
			this.GrbDataCropSetting.SuspendLayout();
			this.GrbPrefixAndSuffix.SuspendLayout();
			base.SuspendLayout();
			this.GrbExtraDataOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbExtraDataOutput.Controls.Add(this.CobOutputBarcodeRect);
			this.GrbExtraDataOutput.Controls.Add(this.LabOutputBarcodeRect);
			this.GrbExtraDataOutput.Location = new System.Drawing.Point(12, 463);
			this.GrbExtraDataOutput.Name = "GrbExtraDataOutput";
			this.GrbExtraDataOutput.Size = new System.Drawing.Size(400, 63);
			this.GrbExtraDataOutput.TabIndex = 53;
			this.GrbExtraDataOutput.TabStop = false;
			this.GrbExtraDataOutput.Text = "额外数据输出";
			this.GrbExtraDataOutput.Visible = false;
			this.CobOutputBarcodeRect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobOutputBarcodeRect.FormattingEnabled = true;
			this.CobOutputBarcodeRect.Items.AddRange(new object[4] { "不输出坐标", "输出四个角点", "输出中心点", "输出四个角点+中心点" });
			this.CobOutputBarcodeRect.Location = new System.Drawing.Point(138, 24);
			this.CobOutputBarcodeRect.Name = "CobOutputBarcodeRect";
			this.CobOutputBarcodeRect.Size = new System.Drawing.Size(177, 22);
			this.CobOutputBarcodeRect.TabIndex = 45;
			this.CobOutputBarcodeRect.SelectedIndexChanged += new System.EventHandler(CobOutputBarcodeRect_SelectedIndexChanged);
			this.LabOutputBarcodeRect.AutoSize = true;
			this.LabOutputBarcodeRect.Location = new System.Drawing.Point(34, 27);
			this.LabOutputBarcodeRect.Name = "LabOutputBarcodeRect";
			this.LabOutputBarcodeRect.Size = new System.Drawing.Size(98, 14);
			this.LabOutputBarcodeRect.TabIndex = 44;
			this.LabOutputBarcodeRect.Text = "输出条码位置:";
			this.GrbMuiltCodeSplit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbMuiltCodeSplit.Controls.Add(this.BtnTemplateIdxSplit);
			this.GrbMuiltCodeSplit.Controls.Add(this.TxbTemplateIdxSplit);
			this.GrbMuiltCodeSplit.Controls.Add(this.LabTemplateIdxSplit);
			this.GrbMuiltCodeSplit.Controls.Add(this.RdbTemplateIdxSuffix);
			this.GrbMuiltCodeSplit.Controls.Add(this.RdbTemplateIdxPrefix);
			this.GrbMuiltCodeSplit.Controls.Add(this.RdbTemplateIdxNoOut);
			this.GrbMuiltCodeSplit.Controls.Add(this.LabTemplateIdxOut);
			this.GrbMuiltCodeSplit.Controls.Add(this.BtnMuiltCodeSplit);
			this.GrbMuiltCodeSplit.Controls.Add(this.TxbMuiltCodeSplit);
			this.GrbMuiltCodeSplit.Controls.Add(this.ChkMuiltCodeSplitSameEof);
			this.GrbMuiltCodeSplit.Controls.Add(this.LabMuiltCodeSplit);
			this.GrbMuiltCodeSplit.Location = new System.Drawing.Point(12, 327);
			this.GrbMuiltCodeSplit.Name = "GrbMuiltCodeSplit";
			this.GrbMuiltCodeSplit.Size = new System.Drawing.Size(400, 130);
			this.GrbMuiltCodeSplit.TabIndex = 54;
			this.GrbMuiltCodeSplit.TabStop = false;
			this.GrbMuiltCodeSplit.Text = "多条码多模板分隔符";
			this.BtnTemplateIdxSplit.Location = new System.Drawing.Point(260, 94);
			this.BtnTemplateIdxSplit.Name = "BtnTemplateIdxSplit";
			this.BtnTemplateIdxSplit.Size = new System.Drawing.Size(54, 23);
			this.BtnTemplateIdxSplit.TabIndex = 58;
			this.BtnTemplateIdxSplit.Text = "确认";
			this.BtnTemplateIdxSplit.UseVisualStyleBackColor = true;
			this.BtnTemplateIdxSplit.Visible = false;
			this.BtnTemplateIdxSplit.Click += new System.EventHandler(BtnTemplateIdxSplit_Click);
			this.TxbTemplateIdxSplit.Location = new System.Drawing.Point(219, 94);
			this.TxbTemplateIdxSplit.MaxLength = 2;
			this.TxbTemplateIdxSplit.Name = "TxbTemplateIdxSplit";
			this.TxbTemplateIdxSplit.Size = new System.Drawing.Size(35, 23);
			this.TxbTemplateIdxSplit.TabIndex = 59;
			this.TxbTemplateIdxSplit.TextChanged += new System.EventHandler(TxbTemplateIdxSplit_TextChanged);
			this.LabTemplateIdxSplit.AutoSize = true;
			this.LabTemplateIdxSplit.Location = new System.Drawing.Point(36, 98);
			this.LabTemplateIdxSplit.Name = "LabTemplateIdxSplit";
			this.LabTemplateIdxSplit.Size = new System.Drawing.Size(182, 14);
			this.LabTemplateIdxSplit.TabIndex = 60;
			this.LabTemplateIdxSplit.Text = "模板号分隔符(十六进制):0x";
			this.RdbTemplateIdxSuffix.AutoSize = true;
			this.RdbTemplateIdxSuffix.Location = new System.Drawing.Point(276, 73);
			this.RdbTemplateIdxSuffix.Name = "RdbTemplateIdxSuffix";
			this.RdbTemplateIdxSuffix.Size = new System.Drawing.Size(81, 18);
			this.RdbTemplateIdxSuffix.TabIndex = 57;
			this.RdbTemplateIdxSuffix.TabStop = true;
			this.RdbTemplateIdxSuffix.Text = "在条码后";
			this.RdbTemplateIdxSuffix.UseVisualStyleBackColor = true;
			this.RdbTemplateIdxSuffix.CheckedChanged += new System.EventHandler(RdbTemplateIdxSuffix_CheckedChanged);
			this.RdbTemplateIdxPrefix.AutoSize = true;
			this.RdbTemplateIdxPrefix.Location = new System.Drawing.Point(194, 73);
			this.RdbTemplateIdxPrefix.Name = "RdbTemplateIdxPrefix";
			this.RdbTemplateIdxPrefix.Size = new System.Drawing.Size(81, 18);
			this.RdbTemplateIdxPrefix.TabIndex = 56;
			this.RdbTemplateIdxPrefix.TabStop = true;
			this.RdbTemplateIdxPrefix.Text = "在条码前";
			this.RdbTemplateIdxPrefix.UseVisualStyleBackColor = true;
			this.RdbTemplateIdxPrefix.CheckedChanged += new System.EventHandler(RdbTemplateIdxPrefix_CheckedChanged);
			this.RdbTemplateIdxNoOut.AutoSize = true;
			this.RdbTemplateIdxNoOut.Location = new System.Drawing.Point(124, 73);
			this.RdbTemplateIdxNoOut.Name = "RdbTemplateIdxNoOut";
			this.RdbTemplateIdxNoOut.Size = new System.Drawing.Size(67, 18);
			this.RdbTemplateIdxNoOut.TabIndex = 55;
			this.RdbTemplateIdxNoOut.TabStop = true;
			this.RdbTemplateIdxNoOut.Text = "不输出";
			this.RdbTemplateIdxNoOut.UseVisualStyleBackColor = true;
			this.RdbTemplateIdxNoOut.CheckedChanged += new System.EventHandler(RdbTemplateIdxNoOut_CheckedChanged);
			this.LabTemplateIdxOut.AutoSize = true;
			this.LabTemplateIdxOut.Location = new System.Drawing.Point(34, 75);
			this.LabTemplateIdxOut.Name = "LabTemplateIdxOut";
			this.LabTemplateIdxOut.Size = new System.Drawing.Size(84, 14);
			this.LabTemplateIdxOut.TabIndex = 54;
			this.LabTemplateIdxOut.Text = "模板号输出:";
			this.BtnMuiltCodeSplit.Location = new System.Drawing.Point(257, 48);
			this.BtnMuiltCodeSplit.Name = "BtnMuiltCodeSplit";
			this.BtnMuiltCodeSplit.Size = new System.Drawing.Size(54, 23);
			this.BtnMuiltCodeSplit.TabIndex = 35;
			this.BtnMuiltCodeSplit.Text = "确认";
			this.BtnMuiltCodeSplit.UseVisualStyleBackColor = true;
			this.BtnMuiltCodeSplit.Visible = false;
			this.BtnMuiltCodeSplit.Click += new System.EventHandler(BtnMuiltCodeSplit_Click);
			this.TxbMuiltCodeSplit.Location = new System.Drawing.Point(216, 48);
			this.TxbMuiltCodeSplit.MaxLength = 2;
			this.TxbMuiltCodeSplit.Name = "TxbMuiltCodeSplit";
			this.TxbMuiltCodeSplit.Size = new System.Drawing.Size(35, 23);
			this.TxbMuiltCodeSplit.TabIndex = 35;
			this.TxbMuiltCodeSplit.TextChanged += new System.EventHandler(TxbMuiltCodeSplit_TextChanged);
			this.ChkMuiltCodeSplitSameEof.AutoSize = true;
			this.ChkMuiltCodeSplitSameEof.Location = new System.Drawing.Point(37, 28);
			this.ChkMuiltCodeSplitSameEof.Name = "ChkMuiltCodeSplitSameEof";
			this.ChkMuiltCodeSplitSameEof.Size = new System.Drawing.Size(138, 18);
			this.ChkMuiltCodeSplitSameEof.TabIndex = 46;
			this.ChkMuiltCodeSplitSameEof.Text = "分隔符跟随结束符";
			this.ChkMuiltCodeSplitSameEof.UseVisualStyleBackColor = true;
			this.ChkMuiltCodeSplitSameEof.CheckedChanged += new System.EventHandler(ChkMuiltCodeSplitSameEof_CheckedChanged);
			this.LabMuiltCodeSplit.AutoSize = true;
			this.LabMuiltCodeSplit.Location = new System.Drawing.Point(34, 52);
			this.LabMuiltCodeSplit.Name = "LabMuiltCodeSplit";
			this.LabMuiltCodeSplit.Size = new System.Drawing.Size(182, 14);
			this.LabMuiltCodeSplit.TabIndex = 44;
			this.LabMuiltCodeSplit.Text = "多条码分隔符(十六进制):0x";
			this.GrbBarcodeIDAndEof.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbBarcodeIDAndEof.Controls.Add(this.ChbBarcodeIDOut);
			this.GrbBarcodeIDAndEof.Controls.Add(this.CobStringEof);
			this.GrbBarcodeIDAndEof.Controls.Add(this.LabStringEof);
			this.GrbBarcodeIDAndEof.Location = new System.Drawing.Point(12, 235);
			this.GrbBarcodeIDAndEof.Name = "GrbBarcodeIDAndEof";
			this.GrbBarcodeIDAndEof.Size = new System.Drawing.Size(400, 86);
			this.GrbBarcodeIDAndEof.TabIndex = 52;
			this.GrbBarcodeIDAndEof.TabStop = false;
			this.GrbBarcodeIDAndEof.Text = "条码ID与结束符";
			this.ChbBarcodeIDOut.AutoSize = true;
			this.ChbBarcodeIDOut.Location = new System.Drawing.Point(37, 28);
			this.ChbBarcodeIDOut.Name = "ChbBarcodeIDOut";
			this.ChbBarcodeIDOut.Size = new System.Drawing.Size(152, 18);
			this.ChbBarcodeIDOut.TabIndex = 46;
			this.ChbBarcodeIDOut.Text = "允许条码类型ID输出";
			this.ChbBarcodeIDOut.UseVisualStyleBackColor = true;
			this.ChbBarcodeIDOut.CheckedChanged += new System.EventHandler(ChbBarcodeIDOut_CheckedChanged);
			this.CobStringEof.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobStringEof.FormattingEnabled = true;
			this.CobStringEof.Items.AddRange(new object[4] { "None", "CR", "TAB", "CRLF" });
			this.CobStringEof.Location = new System.Drawing.Point(125, 50);
			this.CobStringEof.Name = "CobStringEof";
			this.CobStringEof.Size = new System.Drawing.Size(79, 22);
			this.CobStringEof.TabIndex = 45;
			this.CobStringEof.SelectedIndexChanged += new System.EventHandler(CobStringEof_SelectedIndexChanged);
			this.LabStringEof.AutoSize = true;
			this.LabStringEof.Location = new System.Drawing.Point(34, 53);
			this.LabStringEof.Name = "LabStringEof";
			this.LabStringEof.Size = new System.Drawing.Size(84, 14);
			this.LabStringEof.TabIndex = 44;
			this.LabStringEof.Text = "条码结束符:";
			this.GrbDataCropSetting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbDataCropSetting.Controls.Add(this.RdbOnlyEndOutput);
			this.GrbDataCropSetting.Controls.Add(this.RdbOnlyCenterOutput);
			this.GrbDataCropSetting.Controls.Add(this.RdbOnlyFrontOutput);
			this.GrbDataCropSetting.Controls.Add(this.RdbAllDataOutput);
			this.GrbDataCropSetting.Controls.Add(this.BtnEndSessionSet);
			this.GrbDataCropSetting.Controls.Add(this.TxbEndSessionSet);
			this.GrbDataCropSetting.Controls.Add(this.BtnFrontSessionSet);
			this.GrbDataCropSetting.Controls.Add(this.TxbFrontSessionSet);
			this.GrbDataCropSetting.Controls.Add(this.LabEndSessionSet);
			this.GrbDataCropSetting.Controls.Add(this.LabFrontSessionSet);
			this.GrbDataCropSetting.Location = new System.Drawing.Point(12, 101);
			this.GrbDataCropSetting.Name = "GrbDataCropSetting";
			this.GrbDataCropSetting.Size = new System.Drawing.Size(400, 128);
			this.GrbDataCropSetting.TabIndex = 51;
			this.GrbDataCropSetting.TabStop = false;
			this.GrbDataCropSetting.Text = "数据裁剪";
			this.RdbOnlyEndOutput.AutoSize = true;
			this.RdbOnlyEndOutput.Location = new System.Drawing.Point(186, 97);
			this.RdbOnlyEndOutput.Name = "RdbOnlyEndOutput";
			this.RdbOnlyEndOutput.Size = new System.Drawing.Size(123, 18);
			this.RdbOnlyEndOutput.TabIndex = 34;
			this.RdbOnlyEndOutput.TabStop = true;
			this.RdbOnlyEndOutput.Text = "只传输数据后段";
			this.RdbOnlyEndOutput.UseVisualStyleBackColor = true;
			this.RdbOnlyEndOutput.CheckedChanged += new System.EventHandler(RdbOnlyEndOutput_CheckedChanged);
			this.RdbOnlyCenterOutput.AutoSize = true;
			this.RdbOnlyCenterOutput.Location = new System.Drawing.Point(37, 97);
			this.RdbOnlyCenterOutput.Name = "RdbOnlyCenterOutput";
			this.RdbOnlyCenterOutput.Size = new System.Drawing.Size(137, 18);
			this.RdbOnlyCenterOutput.TabIndex = 33;
			this.RdbOnlyCenterOutput.TabStop = true;
			this.RdbOnlyCenterOutput.Text = "只传输数据中间段";
			this.RdbOnlyCenterOutput.UseVisualStyleBackColor = true;
			this.RdbOnlyCenterOutput.CheckedChanged += new System.EventHandler(RdbOnlyCenterOutput_CheckedChanged);
			this.RdbOnlyFrontOutput.AutoSize = true;
			this.RdbOnlyFrontOutput.Location = new System.Drawing.Point(186, 76);
			this.RdbOnlyFrontOutput.Name = "RdbOnlyFrontOutput";
			this.RdbOnlyFrontOutput.Size = new System.Drawing.Size(123, 18);
			this.RdbOnlyFrontOutput.TabIndex = 32;
			this.RdbOnlyFrontOutput.TabStop = true;
			this.RdbOnlyFrontOutput.Text = "只传输数据前段";
			this.RdbOnlyFrontOutput.UseVisualStyleBackColor = true;
			this.RdbOnlyFrontOutput.CheckedChanged += new System.EventHandler(RdbOnlyFrontOutput_CheckedChanged);
			this.RdbAllDataOutput.AutoSize = true;
			this.RdbAllDataOutput.Location = new System.Drawing.Point(36, 76);
			this.RdbAllDataOutput.Name = "RdbAllDataOutput";
			this.RdbAllDataOutput.Size = new System.Drawing.Size(123, 18);
			this.RdbAllDataOutput.TabIndex = 31;
			this.RdbAllDataOutput.TabStop = true;
			this.RdbAllDataOutput.Text = "传输整个数据段";
			this.RdbAllDataOutput.UseVisualStyleBackColor = true;
			this.RdbAllDataOutput.CheckedChanged += new System.EventHandler(RdbAllDataOutput_CheckedChanged);
			this.BtnEndSessionSet.Location = new System.Drawing.Point(232, 48);
			this.BtnEndSessionSet.Name = "BtnEndSessionSet";
			this.BtnEndSessionSet.Size = new System.Drawing.Size(75, 23);
			this.BtnEndSessionSet.TabIndex = 29;
			this.BtnEndSessionSet.Text = "确认";
			this.BtnEndSessionSet.UseVisualStyleBackColor = true;
			this.BtnEndSessionSet.Visible = false;
			this.BtnEndSessionSet.Click += new System.EventHandler(BtnEndSessionSet_Click);
			this.TxbEndSessionSet.Location = new System.Drawing.Point(148, 47);
			this.TxbEndSessionSet.Name = "TxbEndSessionSet";
			this.TxbEndSessionSet.Size = new System.Drawing.Size(75, 23);
			this.TxbEndSessionSet.TabIndex = 30;
			this.TxbEndSessionSet.TextChanged += new System.EventHandler(TxbEndSessionSet_TextChanged);
			this.BtnFrontSessionSet.Location = new System.Drawing.Point(232, 21);
			this.BtnFrontSessionSet.Name = "BtnFrontSessionSet";
			this.BtnFrontSessionSet.Size = new System.Drawing.Size(76, 23);
			this.BtnFrontSessionSet.TabIndex = 25;
			this.BtnFrontSessionSet.Text = "确认";
			this.BtnFrontSessionSet.UseVisualStyleBackColor = true;
			this.BtnFrontSessionSet.Visible = false;
			this.BtnFrontSessionSet.Click += new System.EventHandler(BtnFrontSessionSet_Click);
			this.TxbFrontSessionSet.Location = new System.Drawing.Point(148, 21);
			this.TxbFrontSessionSet.Name = "TxbFrontSessionSet";
			this.TxbFrontSessionSet.Size = new System.Drawing.Size(75, 23);
			this.TxbFrontSessionSet.TabIndex = 26;
			this.TxbFrontSessionSet.TextChanged += new System.EventHandler(TxbFrontSessionSet_TextChanged);
			this.LabEndSessionSet.AutoSize = true;
			this.LabEndSessionSet.Location = new System.Drawing.Point(34, 50);
			this.LabEndSessionSet.Name = "LabEndSessionSet";
			this.LabEndSessionSet.Size = new System.Drawing.Size(98, 14);
			this.LabEndSessionSet.TabIndex = 28;
			this.LabEndSessionSet.Text = "后段长度设置:";
			this.LabFrontSessionSet.AutoSize = true;
			this.LabFrontSessionSet.Location = new System.Drawing.Point(34, 25);
			this.LabFrontSessionSet.Name = "LabFrontSessionSet";
			this.LabFrontSessionSet.Size = new System.Drawing.Size(98, 14);
			this.LabFrontSessionSet.TabIndex = 27;
			this.LabFrontSessionSet.Text = "前段长度设置:";
			this.GrbPrefixAndSuffix.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbPrefixAndSuffix.Controls.Add(this.BtnDataSuffix);
			this.GrbPrefixAndSuffix.Controls.Add(this.TxbDataSuffix);
			this.GrbPrefixAndSuffix.Controls.Add(this.BtnDataPrefix);
			this.GrbPrefixAndSuffix.Controls.Add(this.TxbDataPrefix);
			this.GrbPrefixAndSuffix.Controls.Add(this.ChbDataPrefix);
			this.GrbPrefixAndSuffix.Controls.Add(this.ChbDataSuffix);
			this.GrbPrefixAndSuffix.Location = new System.Drawing.Point(12, 12);
			this.GrbPrefixAndSuffix.Name = "GrbPrefixAndSuffix";
			this.GrbPrefixAndSuffix.Size = new System.Drawing.Size(400, 83);
			this.GrbPrefixAndSuffix.TabIndex = 50;
			this.GrbPrefixAndSuffix.TabStop = false;
			this.GrbPrefixAndSuffix.Text = "前后缀设置";
			this.BtnDataSuffix.Location = new System.Drawing.Point(303, 52);
			this.BtnDataSuffix.Name = "BtnDataSuffix";
			this.BtnDataSuffix.Size = new System.Drawing.Size(66, 23);
			this.BtnDataSuffix.TabIndex = 40;
			this.BtnDataSuffix.Text = "确认";
			this.BtnDataSuffix.UseVisualStyleBackColor = true;
			this.BtnDataSuffix.Visible = false;
			this.BtnDataSuffix.Click += new System.EventHandler(BtnDataSuffix_Click);
			this.TxbDataSuffix.Location = new System.Drawing.Point(130, 52);
			this.TxbDataSuffix.MaxLength = 15;
			this.TxbDataSuffix.Name = "TxbDataSuffix";
			this.TxbDataSuffix.Size = new System.Drawing.Size(171, 23);
			this.TxbDataSuffix.TabIndex = 39;
			this.TxbDataSuffix.TextChanged += new System.EventHandler(TxbDataSuffix_TextChanged);
			this.BtnDataPrefix.Location = new System.Drawing.Point(303, 26);
			this.BtnDataPrefix.Name = "BtnDataPrefix";
			this.BtnDataPrefix.Size = new System.Drawing.Size(66, 23);
			this.BtnDataPrefix.TabIndex = 37;
			this.BtnDataPrefix.Text = "确认";
			this.BtnDataPrefix.UseVisualStyleBackColor = true;
			this.BtnDataPrefix.Visible = false;
			this.BtnDataPrefix.Click += new System.EventHandler(BtnDataPrefix_Click);
			this.TxbDataPrefix.Location = new System.Drawing.Point(130, 26);
			this.TxbDataPrefix.MaxLength = 15;
			this.TxbDataPrefix.Name = "TxbDataPrefix";
			this.TxbDataPrefix.Size = new System.Drawing.Size(171, 23);
			this.TxbDataPrefix.TabIndex = 36;
			this.TxbDataPrefix.TextChanged += new System.EventHandler(TxbDataPrefix_TextChanged);
			this.ChbDataPrefix.AutoSize = true;
			this.ChbDataPrefix.Location = new System.Drawing.Point(36, 28);
			this.ChbDataPrefix.Name = "ChbDataPrefix";
			this.ChbDataPrefix.Size = new System.Drawing.Size(89, 18);
			this.ChbDataPrefix.TabIndex = 47;
			this.ChbDataPrefix.Text = "条码前缀:";
			this.ChbDataPrefix.UseVisualStyleBackColor = true;
			this.ChbDataPrefix.CheckedChanged += new System.EventHandler(ChbDataPrefix_CheckedChanged);
			this.ChbDataSuffix.AutoSize = true;
			this.ChbDataSuffix.Location = new System.Drawing.Point(36, 54);
			this.ChbDataSuffix.Name = "ChbDataSuffix";
			this.ChbDataSuffix.Size = new System.Drawing.Size(89, 18);
			this.ChbDataSuffix.TabIndex = 48;
			this.ChbDataSuffix.Text = "条码后缀:";
			this.ChbDataSuffix.UseVisualStyleBackColor = true;
			this.ChbDataSuffix.CheckedChanged += new System.EventHandler(ChbDataSuffix_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(735, 689);
			base.Controls.Add(this.GrbExtraDataOutput);
			base.Controls.Add(this.GrbMuiltCodeSplit);
			base.Controls.Add(this.GrbBarcodeIDAndEof);
			base.Controls.Add(this.GrbDataCropSetting);
			base.Controls.Add(this.GrbPrefixAndSuffix);
			this.Font = new System.Drawing.Font("宋体", 10.5f);
			base.Name = "DataEditForm";
			this.Text = "数据编辑";
			base.Load += new System.EventHandler(DataEditForm_Load);
			this.GrbExtraDataOutput.ResumeLayout(false);
			this.GrbExtraDataOutput.PerformLayout();
			this.GrbMuiltCodeSplit.ResumeLayout(false);
			this.GrbMuiltCodeSplit.PerformLayout();
			this.GrbBarcodeIDAndEof.ResumeLayout(false);
			this.GrbBarcodeIDAndEof.PerformLayout();
			this.GrbDataCropSetting.ResumeLayout(false);
			this.GrbDataCropSetting.PerformLayout();
			this.GrbPrefixAndSuffix.ResumeLayout(false);
			this.GrbPrefixAndSuffix.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
