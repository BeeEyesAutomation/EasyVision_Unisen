using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using Heroje_Debug_Tool.SubForm;
using HJ_CRC32_n;

namespace 合杰图像算法调试工具
{
	public class CMD_Tool : Form
	{
		public struct SettingMaskAndData
		{
			public byte Type;

			public ushort Mask;

			public byte Data;

			public SettingMaskAndData(byte a, ushort b, byte c)
			{
				Type = a;
				Mask = b;
				Data = c;
			}
		}

		public struct SettingCMD
		{
			public byte Type;

			public byte Mask;

			public ushort Addr;

			public byte Data;

			public SettingCMD(byte a, byte b, ushort c, byte d)
			{
				Type = a;
				Mask = b;
				Addr = c;
				Data = d;
			}
		}

		public struct AddonKeyStu
		{
			public int flag_keycode;

			public int addon_keycode;
		}

		private GetCfgCB GetCfgCBFunc;

		public static AddonKeyStu AddonKey;

		private DecodeSettingCodeXml SettingCodeXml = new DecodeSettingCodeXml();

		private TreeNode LastTreeNode;

		private Dictionary<string, SettingMaskAndData> SettingDictionary = new Dictionary<string, SettingMaskAndData>();

		private SettingCMD SettingCmdData;

		private string ConfigPath = "dpmcmd.xml";

		private string SettingDecriptionString;

		private bool turn_to_cmd = false;

		private bool ack_ok = false;

		private bool IsCtrlKeyPress = false;

		private TreeNode EditSeclectNode;

		private IContainer components = null;

		private TreeView TrvSelectSetting;

		private Label LabSerch;

		private TextBox TxbSearch;

		private CheckBox ChkAllowEdit;

		private Button BtnSendCMD;

		private TextBox TxbSendData;

		private Button BtnSaveConfigXml;

		private GroupBox GrbSettingList;

		private Label LabDispCMD;

		private TextBox TxbCMDDisp;

		private GroupBox GrbSendCMD;

		private GroupBox GrbCrcCalc;

		private TextBox TxtShowCRC;

		private Button BtnCalcCMD;

		private TextBox TxbCrcdInputData;

		private GroupBox GrbExplain;

		private Button BtnAddToRignt;

		private TextBox TxbShowWriteCMD;

		private TextBox TxbShowReadCMD;

		private RadioButton RdbSendHEX;

		private RadioButton RdbSendString;

		private Label LabSendOk;

		private Label LabSendFailed;

		private ContextMenuStrip CmsClickRight;

		private ToolStripComboBox CmbSelectType;

		private ToolStripTextBox TtbInputAddr;

		private ToolStripTextBox TtbInputMask;

		private ToolStripTextBox TtbInputData;

		private ToolStripMenuItem TsmClickOk;

		private ContextMenuStrip CmsClickMid;

		private ToolStripTextBox TtbIDName;

		private ToolStripTextBox TtbDispName;

		private ToolStripMenuItem TsmAddLittleKey;

		private ToolStripMenuItem TsmAddBigKey;

		private ToolStripComboBox TcbSeclectTypeMid;

		private ToolStripTextBox TtbInputAddrMid;

		private ToolStripTextBox TtbInputMaskMid;

		private ToolStripTextBox TtbInputDataMid;

		private Label LabTipsState;

		public CMD_Tool()
		{
			InitializeComponent();
		}

		public void SetCBFunc(GetCfgCB getCfgCB)
		{
			GetCfgCBFunc = getCfgCB;
		}

		private void CMD_Tool_Load(object sender, EventArgs e)
		{
			TrvSelectSetting.LabelEdit = false;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				ConfigPath = "dpmcmd_cn.xml";
			}
			else
			{
				ConfigPath = "dpmcmd_en.xml";
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath);
			try
			{
				SettingCodeXml.XMLToTreeByLinq(ConfigPath, TrvSelectSetting, SettingDictionary, this);
			}
			catch (Exception ex)
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdXmlCfgErrorTips) + "(" + ConfigPath + ")\r\n\r\n" + ex.Message, MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdXmlCfgErrorTips));
			}
		}

		private TreeNode FindTreeNode(string searchtext, TreeNode tnParent)
		{
			if (tnParent == null)
			{
				return null;
			}
			if (tnParent.Text.IndexOf(searchtext, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return tnParent;
			}
			TreeNode treeNode = null;
			foreach (TreeNode node in tnParent.Nodes)
			{
				treeNode = FindTreeNode(searchtext, node);
				if (treeNode != null)
				{
					break;
				}
			}
			return treeNode;
		}

		private TreeNode CallFindNode(string searchtext, TreeView treeView)
		{
			TreeNodeCollection nodes = treeView.Nodes;
			foreach (TreeNode item in nodes)
			{
				TreeNode treeNode = FindTreeNode(searchtext, item);
				if (treeNode != null)
				{
					return treeNode;
				}
			}
			return null;
		}

		private void TxbSearch_TextChanged(object sender, EventArgs e)
		{
			if (TxbSearch.Text.Length > 0)
			{
				TreeNode treeNode = CallFindNode(TxbSearch.Text.Trim(), TrvSelectSetting);
				if (treeNode != null)
				{
					if (LastTreeNode != null)
					{
						LastTreeNode.BackColor = treeNode.BackColor;
						LastTreeNode.ForeColor = SystemColors.MenuText;
					}
					LastTreeNode = treeNode;
					treeNode.BackColor = SystemColors.MenuHighlight;
					treeNode.ForeColor = SystemColors.HighlightText;
					if (treeNode.GetNodeCount(true) > 0)
					{
						treeNode.Expand();
						TrvSelectSetting.SelectedNode = treeNode.FirstNode;
					}
					else
					{
						TrvSelectSetting.SelectedNode = treeNode;
					}
				}
			}
			else
			{
				LastTreeNode.BackColor = Color.Empty;
				LastTreeNode.ForeColor = SystemColors.MenuText;
			}
		}

		private void TrvSelectSetting_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (e.Label != null && e.Label == "")
			{
				if (MessageBox.Show("Do you wang to Remove This Node?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					e.Node.Remove();
				}
				else
				{
					e.CancelEdit = true;
				}
			}
		}

		private void TrvSelectSetting_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selectedNode = TrvSelectSetting.SelectedNode;
			int num = 0;
			string text = null;
			bool flag = true;
			List<byte> list = new List<byte>();
			if (LastTreeNode != null && LastTreeNode != selectedNode)
			{
				LastTreeNode.BackColor = Color.Empty;
				LastTreeNode.ForeColor = SystemColors.MenuText;
			}
			if (selectedNode == null || !SettingDictionary.ContainsKey(selectedNode.Name))
			{
				return;
			}
			byte type = SettingDictionary[selectedNode.Name].Type;
			if (type == 73)
			{
				text = InputBox(type, SettingDictionary[selectedNode.Name].Data);
				if (text == null)
				{
					flag = false;
				}
				else
				{
					flag = true;
					SettingDecriptionString = int.Parse(text).ToString();
					num = int.Parse(text) / 100;
					list.Add(8);
					list.Add(2);
					list.Add(0);
					list.Add((byte)((SettingDictionary[selectedNode.Name].Mask & 0xFF00) >> 8));
					list.Add((byte)((uint)num & 0xFFu));
					list.Add((byte)((num & 0xFF00) >> 8));
					byte[] array = list.ToArray();
					ushort num2 = HJ_CRC32.crc16(array, array.Length);
					list.Add((byte)((num2 & 0xFF00) >> 8));
					list.Add((byte)(num2 & 0xFFu));
					list.Insert(0, 0);
					list.Insert(0, 126);
				}
			}
			else if (type == 84)
			{
				text = InputBox(type, SettingDictionary[selectedNode.Name].Data);
				if (text == null)
				{
					flag = false;
				}
				else
				{
					int num3 = Encoding.Default.GetByteCount(text) + 1;
					byte[] bytes = Encoding.Default.GetBytes(text);
					flag = true;
					list.Add(8);
					list.Add((byte)num3);
					list.Add(0);
					list.Add((byte)((SettingDictionary[selectedNode.Name].Mask & 0xFF00) >> 8));
					byte[] array2 = bytes;
					foreach (byte item in array2)
					{
						list.Add(item);
					}
					list.Add(0);
					byte[] array3 = list.ToArray();
					ushort num2 = HJ_CRC32.crc16(array3, array3.Length);
					list.Add((byte)((num2 & 0xFF00) >> 8));
					list.Add((byte)(num2 & 0xFFu));
					list.Insert(0, 0);
					list.Insert(0, 126);
				}
			}
			else if (type == 75)
			{
				flag = false;
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdCodeTypeKTips), MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdCodeTypeKTips), MessageBoxButtons.OK);
			}
			else if (type == 88)
			{
				text = InputBox(type, SettingDictionary[selectedNode.Name].Data);
				if (text == null)
				{
					flag = false;
				}
				else
				{
					flag = true;
					SettingDecriptionString = int.Parse(text).ToString();
					num = int.Parse(text);
					list.Add(9);
					list.Add((byte)(SettingDictionary[selectedNode.Name].Mask & 0xFFu));
					list.Add(0);
					list.Add((byte)((SettingDictionary[selectedNode.Name].Mask & 0xFF00) >> 8));
					list.Add((byte)((uint)num & 0xFFu));
					byte[] array4 = list.ToArray();
					ushort num2 = HJ_CRC32.crc16(array4, array4.Length);
					list.Add((byte)((num2 & 0xFF00) >> 8));
					list.Add((byte)(num2 & 0xFFu));
					list.Insert(0, 0);
					list.Insert(0, 126);
				}
			}
			else if (type == 89 || type == 77)
			{
				text = InputBox(type, SettingDictionary[selectedNode.Name].Data);
				if (text == null)
				{
					flag = false;
				}
				else
				{
					flag = true;
					SettingDecriptionString = int.Parse(text).ToString();
					num = int.Parse(text) / 10;
					list.Add(9);
					list.Add((byte)(SettingDictionary[selectedNode.Name].Mask & 0xFFu));
					list.Add(0);
					list.Add((byte)((SettingDictionary[selectedNode.Name].Mask & 0xFF00) >> 8));
					list.Add((byte)((uint)num & 0xFFu));
					byte[] array5 = list.ToArray();
					ushort num2 = HJ_CRC32.crc16(array5, array5.Length);
					list.Add((byte)((num2 & 0xFF00) >> 8));
					list.Add((byte)(num2 & 0xFFu));
					list.Insert(0, 0);
					list.Insert(0, 126);
					SettingDecriptionString = int.Parse(text).ToString();
				}
			}
			else
			{
				switch (type)
				{
				case 90:
				{
					text = InputBox(type, SettingDictionary[selectedNode.Name].Data);
					if (text == null)
					{
						flag = false;
						break;
					}
					flag = true;
					SettingDecriptionString = int.Parse(text).ToString();
					num = int.Parse(text) / 100;
					list.Add(9);
					list.Add((byte)(SettingDictionary[selectedNode.Name].Mask & 0xFFu));
					list.Add(0);
					list.Add((byte)((SettingDictionary[selectedNode.Name].Mask & 0xFF00) >> 8));
					list.Add((byte)((uint)num & 0xFFu));
					byte[] array9 = list.ToArray();
					ushort num2 = HJ_CRC32.crc16(array9, array9.Length);
					list.Add((byte)((num2 & 0xFF00) >> 8));
					list.Add((byte)(num2 & 0xFFu));
					list.Insert(0, 0);
					list.Insert(0, 126);
					SettingDecriptionString = int.Parse(text).ToString();
					break;
				}
				case 76:
				{
					text = InputBox(type, SettingDictionary[selectedNode.Name].Data);
					if (text == null)
					{
						flag = false;
						break;
					}
					flag = true;
					SettingDecriptionString = int.Parse(text).ToString();
					num = int.Parse(text);
					list.Add(8);
					list.Add(2);
					list.Add(0);
					list.Add((byte)((SettingDictionary[selectedNode.Name].Mask & 0xFF00) >> 8));
					list.Add((byte)((uint)num & 0xFFu));
					list.Add((byte)((num & 0xFF00) >> 8));
					byte[] array8 = list.ToArray();
					ushort num2 = HJ_CRC32.crc16(array8, array8.Length);
					list.Add((byte)((num2 & 0xFF00) >> 8));
					list.Add((byte)(num2 & 0xFFu));
					list.Insert(0, 0);
					list.Insert(0, 126);
					break;
				}
				case 87:
				{
					flag = true;
					num = (SettingDictionary[selectedNode.Name].Mask & 0xFF) + (SettingDictionary[selectedNode.Name].Data << 8);
					list.Add(8);
					list.Add(2);
					list.Add(0);
					list.Add((byte)((SettingDictionary[selectedNode.Name].Mask & 0xFF00) >> 8));
					list.Add((byte)((uint)num & 0xFFu));
					list.Add((byte)((num & 0xFF00) >> 8));
					byte[] array7 = list.ToArray();
					ushort num2 = HJ_CRC32.crc16(array7, array7.Length);
					list.Add((byte)((num2 & 0xFF00) >> 8));
					list.Add((byte)(num2 & 0xFFu));
					list.Insert(0, 0);
					list.Insert(0, 126);
					break;
				}
				default:
				{
					flag = true;
					SettingDecriptionString = selectedNode.Text;
					list.Add(9);
					list.Add((byte)(SettingDictionary[selectedNode.Name].Mask & 0xFFu));
					list.Add(0);
					list.Add((byte)((SettingDictionary[selectedNode.Name].Mask & 0xFF00) >> 8));
					list.Add(SettingDictionary[selectedNode.Name].Data);
					byte[] array6 = list.ToArray();
					ushort num2 = HJ_CRC32.crc16(array6, array6.Length);
					list.Add((byte)((num2 & 0xFF00) >> 8));
					list.Add((byte)(num2 & 0xFFu));
					list.Insert(0, 0);
					list.Insert(0, 126);
					break;
				}
				}
			}
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				byte[] array10 = list.ToArray();
				byte[] array11 = array10;
				foreach (byte b in array11)
				{
					stringBuilder.Append(b.ToString("X2") + " ");
				}
				TxbCMDDisp.Text = stringBuilder.ToString();
				RdbSendHEX.Checked = true;
				TxbSendData.Text = TxbCMDDisp.Text;
			}
		}

		private string InputBox(byte type, byte data)
		{
			int num = 0;
			Form form = new Form();
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.Width = 240;
			form.Height = 150;
			form.AutoSize = true;
			form.ShowIcon = false;
			Label label = new Label();
			label.Left = 10;
			label.Top = 20;
			label.Parent = form;
			label.AutoSize = true;
			TextBox textBox = new TextBox();
			textBox.Left = 30;
			textBox.Top = 45;
			textBox.Width = 160;
			textBox.Parent = form;
			textBox.Text = "";
			textBox.SelectAll();
			Button button = new Button();
			button.Left = 30;
			button.Top = 80;
			button.Parent = form;
			form.AcceptButton = button;
			button.DialogResult = DialogResult.OK;
			Button button2 = new Button();
			button2.Left = 115;
			button2.Top = 80;
			button2.Parent = form;
			button2.DialogResult = DialogResult.Cancel;
			RadioButton radioButton = new RadioButton();
			RadioButton radioButton2 = new RadioButton();
			RadioButton radioButton3 = new RadioButton();
			Control control2 = (radioButton3.Parent = form);
			Control control4 = (radioButton2.Parent = control2);
			radioButton.Parent = control4;
			int num3 = (radioButton3.Top = 0);
			int top = (radioButton2.Top = num3);
			radioButton.Top = top;
			radioButton.Left = 10;
			radioButton2.Left = 70;
			radioButton3.Left = 140;
			radioButton.Text = "十进制";
			radioButton2.Text = "十六进制";
			radioButton3.Text = "字符输入";
			bool flag2 = (radioButton3.Visible = false);
			bool visible = (radioButton2.Visible = flag2);
			radioButton.Visible = visible;
			form.Text = "自定义数据输入";
			button.Text = "确定";
			button2.Text = "取消";
			string text = "请输入整数数值,并确保数值";
			string text2 = "请输入字符串,并确保长度";
			string text3 = "个字符";
			string text4 = "请输入";
			string text5 = "的整数倍数值,并确保数值";
			string text6 = "请按规则输入值,并确保数值";
			string text7 = "如果数值是0，代表不起作用";
			if (type == 73)
			{
				num = 25700;
				label.Text = text + " <= 6553500";
			}
			switch (type)
			{
			case 88:
				flag2 = (radioButton3.Visible = true);
				visible = (radioButton2.Visible = flag2);
				radioButton.Visible = visible;
				radioButton.Checked = true;
				num = 1;
				label.Text = text6 + " <= " + data;
				break;
			case 84:
				visible = (radioButton3.Visible = true);
				radioButton2.Visible = visible;
				radioButton3.Checked = true;
				num = 0;
				label.Text = text2 + " <= " + data + text3;
				break;
			case 89:
				num = 10;
				label.Text = text4 + "10" + text5 + " <= " + data * 10;
				break;
			case 90:
				num = 100;
				label.Text = text4 + "100" + text5 + " <= " + data * 100;
				break;
			case 77:
				num = -10;
				label.Text = text4 + "10" + text5 + " >= " + data * 10;
				break;
			case 76:
				num = 0;
				label.Text = text + "<= " + 65535 + "," + text7;
				break;
			}
			try
			{
				while (form.ShowDialog() == DialogResult.OK)
				{
					bool flag7 = false;
					try
					{
						if (num == 0)
						{
							if (radioButton2.Checked)
							{
								if (!Regex.IsMatch(textBox.Text, "(?i)[\\da-f]{2}"))
								{
									throw new ArgumentNullException("Illegal values");
								}
								MatchCollection matchCollection = Regex.Matches(textBox.Text, "(?i)[\\da-f]{2}");
								if (matchCollection.Count > data)
								{
									throw new ArgumentNullException("Length Miss Macth");
								}
								byte[] array = new byte[matchCollection.Count];
								for (int i = 0; i < matchCollection.Count; i++)
								{
									array[i] = byte.Parse(matchCollection[i].Value, NumberStyles.HexNumber);
								}
								textBox.Text = Encoding.Default.GetString(array);
								flag7 = true;
							}
							int byteCount = Encoding.Default.GetByteCount(textBox.Text);
							if (byteCount <= data)
							{
								flag7 = true;
							}
						}
						else if (num < 0)
						{
							if (int.Parse(textBox.Text) >= data * -num && int.Parse(textBox.Text) <= 255 * -num)
							{
								flag7 = true;
							}
						}
						else if (num == 1)
						{
							if (radioButton3.Checked)
							{
								if (textBox.TextLength != 1)
								{
									throw new ArgumentNullException("Length Miss Macth");
								}
								textBox.Text = ((byte)textBox.Text[0]).ToString();
								flag7 = true;
							}
							else if (radioButton2.Checked)
							{
								if (!Regex.IsMatch(textBox.Text, "(?i)[\\da-f]{2}"))
								{
									throw new ArgumentNullException("Illegal values");
								}
								MatchCollection matchCollection2 = Regex.Matches(textBox.Text, "(?i)[\\da-f]{2}");
								if (matchCollection2.Count > 1)
								{
									throw new ArgumentNullException("Length Miss Macth");
								}
								textBox.Text = byte.Parse(matchCollection2[0].Value, NumberStyles.HexNumber).ToString();
								flag7 = true;
							}
							else if (int.Parse(textBox.Text) <= data * num && int.Parse(textBox.Text) >= 0)
							{
								flag7 = true;
							}
						}
						else if (int.Parse(textBox.Text) <= data * num && int.Parse(textBox.Text) >= 0)
						{
							flag7 = true;
						}
						if (flag7)
						{
							return textBox.Text;
						}
						MessageBox.Show("Invalid Value!", "Error");
					}
					catch
					{
						MessageBox.Show("Invalid Data Format!", "Error");
					}
				}
				return null;
			}
			finally
			{
				form.Dispose();
			}
		}

		private void BtnAddToRignt_Click(object sender, EventArgs e)
		{
			RdbSendHEX.Checked = true;
			TxbSendData.Text = TxbCMDDisp.Text;
		}

		public void CMD_Ack_CallBack(byte[] dat, int len)
		{
			try
			{
				ack_ok = true;
				Invoke((MethodInvoker)delegate
				{
					LabSendFailed.Visible = false;
					LabSendOk.Visible = true;
				});
			}
			catch (Exception)
			{
			}
		}

		private void WaitCmdAck_Elapsed(object sender, ElapsedEventArgs e)
		{
			((System.Timers.Timer)sender).Stop();
			if (!ack_ok)
			{
				Invoke((MethodInvoker)delegate
				{
					LabSendFailed.Visible = true;
					LabSendOk.Visible = false;
					ToolCfg.UpdateAdjState = true;
				});
			}
			Invoke((MethodInvoker)delegate
			{
				BtnSendCMD.Enabled = true;
			});
			ack_ok = false;
			((System.Timers.Timer)sender).Dispose();
		}

		private void BtnSendCMD_Click(object sender, EventArgs e)
		{
			if (ToolCfg.CurrentDevice != null && ToolCfg.CurrentDevice.IsConnect)
			{
				if (GetCfgCBFunc(3u) == 2 && ToolCfg.is_get_rawimg)
				{
					LabTipsState.Visible = true;
				}
				else
				{
					LabTipsState.Visible = false;
				}
				if (RdbSendString.Checked)
				{
					byte[] bytes = Encoding.Default.GetBytes(TxbSendData.Text);
					ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, bytes, bytes.Length);
					BtnSendCMD.Enabled = false;
					System.Timers.Timer timer = new System.Timers.Timer(800.0);
					timer.Elapsed += WaitCmdAck_Elapsed;
					timer.Start();
				}
				else if (Regex.IsMatch(TxbSendData.Text, "(?i)[\\da-f]{2}"))
				{
					MatchCollection matchCollection = Regex.Matches(TxbSendData.Text, "(?i)[\\da-f]{2}");
					List<byte> list = new List<byte>();
					foreach (Match item in matchCollection)
					{
						list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
					}
					ToolCfg.CurrentDevice.SendData(ToolCfg.CurrentDevice.DeviceHandle, list.ToArray(), list.Count);
					BtnSendCMD.Enabled = false;
					System.Timers.Timer timer2 = new System.Timers.Timer(800.0);
					timer2.Elapsed += WaitCmdAck_Elapsed;
					timer2.Start();
				}
				else
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.EnterRightHexTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.EnterRightHexTips));
				}
			}
			else
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdConnectThenSendTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdConnectThenSendTips));
			}
		}

		private void BtnCalcCMD_Click(object sender, EventArgs e)
		{
			if (Regex.IsMatch(TxbCrcdInputData.Text, "(?i)[\\da-f]{2}"))
			{
				MatchCollection matchCollection = Regex.Matches(TxbCrcdInputData.Text, "(?i)[\\da-f]{2}");
				List<byte> list = new List<byte>();
				bool flag = false;
				foreach (Match item in matchCollection)
				{
					list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
				}
				if (list[0] == 126 && list[1] == 0)
				{
					list.RemoveAt(0);
					list.RemoveAt(0);
					flag = true;
				}
				uint num = HJ_CRC32.crc16(list.ToArray(), list.ToArray().Length);
				string text = num.ToString("X4");
				TxtShowCRC.Text = "";
				if (flag)
				{
					TxtShowCRC.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdRemove7e00) + "\r\n";
				}
				TextBox txtShowCRC = TxtShowCRC;
				txtShowCRC.Text = txtShowCRC.Text + MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdCalcResult) + "\r\n";
				TextBox txtShowCRC2 = TxtShowCRC;
				txtShowCRC2.Text = txtShowCRC2.Text + ((num & 0xFF00) >> 8).ToString("X2") + " " + (num & 0xFFu).ToString("X2") + "\r\n";
			}
			else
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.EnterRightHexTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.EnterRightHexTips));
			}
		}

		private void ChkAllowEdit_CheckedChanged(object sender, EventArgs e)
		{
			TrvSelectSetting.LabelEdit = ChkAllowEdit.Checked;
			BtnSaveConfigXml.Enabled = ChkAllowEdit.Checked;
		}

		private void BtnSaveConfigXml_Click(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdSaveCfgTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdSaveCfgTips), MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					SettingCodeXml.TreeToXmlByLinq(TrvSelectSetting, SettingDictionary, this, ConfigPath);
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdSaveCfgOkTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdSaveCfgOkTips));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error");
			}
		}

		private void TrvSelectSetting_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (TrvSelectSetting.LabelEdit)
			{
				e.Node.BeginEdit();
			}
		}

		private void TrvSelectSetting_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button != MouseButtons.Right || e.Node.Parent == null || e.Node == null || !ChkAllowEdit.Checked)
			{
				return;
			}
			EditSeclectNode = e.Node;
			TreeNode node = e.Node;
			if (SettingDictionary.ContainsKey(node.Name) && !IsCtrlKeyPress)
			{
				byte type = SettingDictionary[node.Name].Type;
				if (type == 73)
				{
					CmbSelectType.SelectedIndex = 6;
				}
				else if (type == 84)
				{
					CmbSelectType.SelectedIndex = 4;
				}
				else if (type == 75)
				{
					CmbSelectType.SelectedIndex = 7;
				}
				else if (type == 88)
				{
					CmbSelectType.SelectedIndex = 1;
				}
				else if (type == 89 || type == 77)
				{
					CmbSelectType.SelectedIndex = 2;
				}
				else
				{
					switch (type)
					{
					case 90:
						CmbSelectType.SelectedIndex = 3;
						break;
					case 76:
						CmbSelectType.SelectedIndex = 5;
						break;
					default:
						CmbSelectType.SelectedIndex = 0;
						break;
					}
				}
				TtbInputAddr.Text = ((SettingDictionary[node.Name].Mask & 0xFF00) >> 8).ToString("X2");
				TtbInputMask.Text = (SettingDictionary[node.Name].Mask & 0xFF).ToString("X2");
				TtbInputData.Text = SettingDictionary[node.Name].Data.ToString("X2");
				CmsClickRight.Show(TrvSelectSetting, e.X + 8, e.Y);
			}
			else if (IsCtrlKeyPress)
			{
				TcbSeclectTypeMid.SelectedIndex = 0;
				CmsClickMid.Show(TrvSelectSetting, e.X + 8, e.Y);
			}
			TrvSelectSetting.SelectedNode = e.Node;
		}

		private void TsmClickOk_Click(object sender, EventArgs e)
		{
			SettingMaskAndData value = default(SettingMaskAndData);
			try
			{
				value.Type = (byte)CmbSelectType.Items[CmbSelectType.SelectedIndex].ToString().Substring(1, 1).ToArray()[0];
				value.Mask = (ushort)(byte.Parse(TtbInputMask.Text, NumberStyles.AllowHexSpecifier) + (byte.Parse(TtbInputAddr.Text, NumberStyles.AllowHexSpecifier) << 8));
				value.Data = byte.Parse(TtbInputData.Text, NumberStyles.AllowHexSpecifier);
				SettingDictionary[EditSeclectNode.Name] = value;
			}
			catch (Exception)
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdSaveErrorTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdSaveErrorTips));
			}
			CmsClickRight.Close();
		}

		private void TsmAddLittleKey_Click(object sender, EventArgs e)
		{
			bool flag = true;
			if (TcbSeclectTypeMid.SelectedIndex < TcbSeclectTypeMid.Items.Count - 1)
			{
				SettingMaskAndData value = default(SettingMaskAndData);
				try
				{
					value.Type = (byte)TcbSeclectTypeMid.Items[TcbSeclectTypeMid.SelectedIndex].ToString().Substring(1, 1).ToArray()[0];
					value.Mask = (ushort)(byte.Parse(TtbInputMaskMid.Text, NumberStyles.AllowHexSpecifier) + (byte.Parse(TtbInputAddrMid.Text, NumberStyles.AllowHexSpecifier) << 8));
					value.Data = byte.Parse(TtbInputDataMid.Text, NumberStyles.AllowHexSpecifier);
					if (!SettingDictionary.ContainsKey(TtbIDName.Text))
					{
						SettingDictionary.Add(TtbIDName.Text, value);
					}
					else
					{
						flag = false;
						MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdSaveKeyErrorTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdSaveKeyErrorTips));
					}
				}
				catch
				{
					flag = false;
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdSaveErrorTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdSaveErrorTips));
				}
			}
			if (flag)
			{
				EditSeclectNode.Nodes.Add(TtbIDName.Text, TtbDispName.Text);
			}
			CmsClickMid.Close();
		}

		private void TsmAddBigKey_Click(object sender, EventArgs e)
		{
			bool flag = true;
			if (TcbSeclectTypeMid.SelectedIndex < TcbSeclectTypeMid.Items.Count - 1)
			{
				SettingMaskAndData value = default(SettingMaskAndData);
				try
				{
					value.Type = (byte)TcbSeclectTypeMid.Items[TcbSeclectTypeMid.SelectedIndex].ToString().Substring(1, 1).ToArray()[0];
					value.Mask = (ushort)(byte.Parse(TtbInputMaskMid.Text, NumberStyles.AllowHexSpecifier) + (byte.Parse(TtbInputAddrMid.Text, NumberStyles.AllowHexSpecifier) << 8));
					value.Data = byte.Parse(TtbInputDataMid.Text, NumberStyles.AllowHexSpecifier);
					if (!SettingDictionary.ContainsKey(TtbIDName.Text))
					{
						SettingDictionary.Add(TtbIDName.Text, value);
					}
					else
					{
						flag = false;
						MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdSaveKeyErrorTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdSaveKeyErrorTips));
					}
				}
				catch
				{
					flag = false;
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdSaveErrorTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdSaveErrorTips));
				}
			}
			if (flag)
			{
				EditSeclectNode.Parent.Nodes.Insert(EditSeclectNode.Index + 1, TtbIDName.Text, TtbDispName.Text);
			}
			CmsClickMid.Close();
		}

		private void TrvSelectSetting_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && ChkAllowEdit.Checked)
			{
				IsCtrlKeyPress = !IsCtrlKeyPress;
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.E)
			{
				ChkAllowEdit.Visible = true;
				BtnSaveConfigXml.Visible = true;
			}
		}

		private void LabTipsState_Click(object sender, EventArgs e)
		{
			MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.CmdContinueModeTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.CmdContinueModeTips));
		}

		private void LabSerch_DoubleClick(object sender, EventArgs e)
		{
			ChkAllowEdit.Visible = true;
			BtnSaveConfigXml.Visible = true;
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
			this.TrvSelectSetting = new System.Windows.Forms.TreeView();
			this.CmsClickRight = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CmbSelectType = new System.Windows.Forms.ToolStripComboBox();
			this.TtbInputAddr = new System.Windows.Forms.ToolStripTextBox();
			this.TtbInputMask = new System.Windows.Forms.ToolStripTextBox();
			this.TtbInputData = new System.Windows.Forms.ToolStripTextBox();
			this.TsmClickOk = new System.Windows.Forms.ToolStripMenuItem();
			this.LabSerch = new System.Windows.Forms.Label();
			this.TxbSearch = new System.Windows.Forms.TextBox();
			this.ChkAllowEdit = new System.Windows.Forms.CheckBox();
			this.BtnSendCMD = new System.Windows.Forms.Button();
			this.TxbSendData = new System.Windows.Forms.TextBox();
			this.BtnSaveConfigXml = new System.Windows.Forms.Button();
			this.GrbSettingList = new System.Windows.Forms.GroupBox();
			this.BtnAddToRignt = new System.Windows.Forms.Button();
			this.LabDispCMD = new System.Windows.Forms.Label();
			this.TxbCMDDisp = new System.Windows.Forms.TextBox();
			this.GrbSendCMD = new System.Windows.Forms.GroupBox();
			this.LabTipsState = new System.Windows.Forms.Label();
			this.LabSendFailed = new System.Windows.Forms.Label();
			this.LabSendOk = new System.Windows.Forms.Label();
			this.RdbSendHEX = new System.Windows.Forms.RadioButton();
			this.RdbSendString = new System.Windows.Forms.RadioButton();
			this.GrbCrcCalc = new System.Windows.Forms.GroupBox();
			this.TxtShowCRC = new System.Windows.Forms.TextBox();
			this.BtnCalcCMD = new System.Windows.Forms.Button();
			this.TxbCrcdInputData = new System.Windows.Forms.TextBox();
			this.GrbExplain = new System.Windows.Forms.GroupBox();
			this.TxbShowWriteCMD = new System.Windows.Forms.TextBox();
			this.TxbShowReadCMD = new System.Windows.Forms.TextBox();
			this.CmsClickMid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.TtbIDName = new System.Windows.Forms.ToolStripTextBox();
			this.TtbDispName = new System.Windows.Forms.ToolStripTextBox();
			this.TcbSeclectTypeMid = new System.Windows.Forms.ToolStripComboBox();
			this.TtbInputAddrMid = new System.Windows.Forms.ToolStripTextBox();
			this.TtbInputMaskMid = new System.Windows.Forms.ToolStripTextBox();
			this.TtbInputDataMid = new System.Windows.Forms.ToolStripTextBox();
			this.TsmAddLittleKey = new System.Windows.Forms.ToolStripMenuItem();
			this.TsmAddBigKey = new System.Windows.Forms.ToolStripMenuItem();
			this.CmsClickRight.SuspendLayout();
			this.GrbSettingList.SuspendLayout();
			this.GrbSendCMD.SuspendLayout();
			this.GrbCrcCalc.SuspendLayout();
			this.GrbExplain.SuspendLayout();
			this.CmsClickMid.SuspendLayout();
			base.SuspendLayout();
			this.TrvSelectSetting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.TrvSelectSetting.Location = new System.Drawing.Point(8, 63);
			this.TrvSelectSetting.Name = "TrvSelectSetting";
			this.TrvSelectSetting.Size = new System.Drawing.Size(262, 413);
			this.TrvSelectSetting.TabIndex = 0;
			this.TrvSelectSetting.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(TrvSelectSetting_AfterLabelEdit);
			this.TrvSelectSetting.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(TrvSelectSetting_AfterSelect);
			this.TrvSelectSetting.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(TrvSelectSetting_NodeMouseClick);
			this.TrvSelectSetting.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(TrvSelectSetting_NodeMouseDoubleClick);
			this.TrvSelectSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(TrvSelectSetting_KeyDown);
			this.CmsClickRight.AutoSize = false;
			this.CmsClickRight.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.CmbSelectType, this.TtbInputAddr, this.TtbInputMask, this.TtbInputData, this.TsmClickOk });
			this.CmsClickRight.Name = "CmsClickRight";
			this.CmsClickRight.ShowImageMargin = false;
			this.CmsClickRight.ShowItemToolTips = false;
			this.CmsClickRight.Size = new System.Drawing.Size(120, 133);
			this.CmbSelectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CmbSelectType.DropDownWidth = 100;
			this.CmbSelectType.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.CmbSelectType.Items.AddRange(new object[9] { "‘R’", "‘X’", "‘Y’", "‘Z’", "‘T’", "‘L’", "‘I’", "‘K’", "'W'" });
			this.CmbSelectType.Margin = new System.Windows.Forms.Padding(1, 2, 2, 1);
			this.CmbSelectType.MaxDropDownItems = 4;
			this.CmbSelectType.Name = "CmbSelectType";
			this.CmbSelectType.Size = new System.Drawing.Size(100, 25);
			this.TtbInputAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TtbInputAddr.MaxLength = 2;
			this.TtbInputAddr.Name = "TtbInputAddr";
			this.TtbInputAddr.Size = new System.Drawing.Size(100, 23);
			this.TtbInputMask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TtbInputMask.MaxLength = 2;
			this.TtbInputMask.Name = "TtbInputMask";
			this.TtbInputMask.Size = new System.Drawing.Size(100, 23);
			this.TtbInputData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TtbInputData.MaxLength = 2;
			this.TtbInputData.Name = "TtbInputData";
			this.TtbInputData.Size = new System.Drawing.Size(100, 23);
			this.TsmClickOk.Name = "TsmClickOk";
			this.TsmClickOk.Size = new System.Drawing.Size(135, 22);
			this.TsmClickOk.Text = "       确  认";
			this.TsmClickOk.Click += new System.EventHandler(TsmClickOk_Click);
			this.LabSerch.AutoSize = true;
			this.LabSerch.Location = new System.Drawing.Point(6, 17);
			this.LabSerch.Name = "LabSerch";
			this.LabSerch.Size = new System.Drawing.Size(35, 12);
			this.LabSerch.TabIndex = 1;
			this.LabSerch.Text = "搜索:";
			this.LabSerch.DoubleClick += new System.EventHandler(LabSerch_DoubleClick);
			this.TxbSearch.Location = new System.Drawing.Point(50, 13);
			this.TxbSearch.Name = "TxbSearch";
			this.TxbSearch.Size = new System.Drawing.Size(80, 21);
			this.TxbSearch.TabIndex = 2;
			this.TxbSearch.TextChanged += new System.EventHandler(TxbSearch_TextChanged);
			this.ChkAllowEdit.AutoSize = true;
			this.ChkAllowEdit.Location = new System.Drawing.Point(132, 16);
			this.ChkAllowEdit.Name = "ChkAllowEdit";
			this.ChkAllowEdit.Size = new System.Drawing.Size(72, 16);
			this.ChkAllowEdit.TabIndex = 3;
			this.ChkAllowEdit.Text = "编辑模式";
			this.ChkAllowEdit.UseVisualStyleBackColor = true;
			this.ChkAllowEdit.Visible = false;
			this.ChkAllowEdit.CheckedChanged += new System.EventHandler(ChkAllowEdit_CheckedChanged);
			this.BtnSendCMD.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			this.BtnSendCMD.Location = new System.Drawing.Point(188, 161);
			this.BtnSendCMD.Name = "BtnSendCMD";
			this.BtnSendCMD.Size = new System.Drawing.Size(82, 31);
			this.BtnSendCMD.TabIndex = 4;
			this.BtnSendCMD.Text = "发送命令";
			this.BtnSendCMD.UseVisualStyleBackColor = true;
			this.BtnSendCMD.Click += new System.EventHandler(BtnSendCMD_Click);
			this.TxbSendData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TxbSendData.Location = new System.Drawing.Point(6, 28);
			this.TxbSendData.Multiline = true;
			this.TxbSendData.Name = "TxbSendData";
			this.TxbSendData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxbSendData.Size = new System.Drawing.Size(281, 128);
			this.TxbSendData.TabIndex = 5;
			this.BtnSaveConfigXml.Enabled = false;
			this.BtnSaveConfigXml.Location = new System.Drawing.Point(203, 12);
			this.BtnSaveConfigXml.Name = "BtnSaveConfigXml";
			this.BtnSaveConfigXml.Size = new System.Drawing.Size(67, 23);
			this.BtnSaveConfigXml.TabIndex = 8;
			this.BtnSaveConfigXml.Text = "保存配置";
			this.BtnSaveConfigXml.UseVisualStyleBackColor = true;
			this.BtnSaveConfigXml.Visible = false;
			this.BtnSaveConfigXml.Click += new System.EventHandler(BtnSaveConfigXml_Click);
			this.GrbSettingList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.GrbSettingList.Controls.Add(this.BtnAddToRignt);
			this.GrbSettingList.Controls.Add(this.TxbCMDDisp);
			this.GrbSettingList.Controls.Add(this.BtnSaveConfigXml);
			this.GrbSettingList.Controls.Add(this.ChkAllowEdit);
			this.GrbSettingList.Controls.Add(this.TxbSearch);
			this.GrbSettingList.Controls.Add(this.TrvSelectSetting);
			this.GrbSettingList.Controls.Add(this.LabDispCMD);
			this.GrbSettingList.Controls.Add(this.LabSerch);
			this.GrbSettingList.Location = new System.Drawing.Point(6, 3);
			this.GrbSettingList.Name = "GrbSettingList";
			this.GrbSettingList.Size = new System.Drawing.Size(274, 487);
			this.GrbSettingList.TabIndex = 9;
			this.GrbSettingList.TabStop = false;
			this.GrbSettingList.Text = "命令列表";
			this.BtnAddToRignt.Location = new System.Drawing.Point(221, 35);
			this.BtnAddToRignt.Name = "BtnAddToRignt";
			this.BtnAddToRignt.Size = new System.Drawing.Size(49, 23);
			this.BtnAddToRignt.TabIndex = 11;
			this.BtnAddToRignt.Text = "->>>";
			this.BtnAddToRignt.UseVisualStyleBackColor = true;
			this.BtnAddToRignt.Click += new System.EventHandler(BtnAddToRignt_Click);
			this.LabDispCMD.AutoSize = true;
			this.LabDispCMD.Location = new System.Drawing.Point(6, 40);
			this.LabDispCMD.Name = "LabDispCMD";
			this.LabDispCMD.Size = new System.Drawing.Size(35, 12);
			this.LabDispCMD.TabIndex = 10;
			this.LabDispCMD.Text = "指令:";
			this.TxbCMDDisp.Location = new System.Drawing.Point(50, 36);
			this.TxbCMDDisp.Name = "TxbCMDDisp";
			this.TxbCMDDisp.Size = new System.Drawing.Size(170, 21);
			this.TxbCMDDisp.TabIndex = 9;
			this.GrbSendCMD.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbSendCMD.Controls.Add(this.LabTipsState);
			this.GrbSendCMD.Controls.Add(this.LabSendFailed);
			this.GrbSendCMD.Controls.Add(this.LabSendOk);
			this.GrbSendCMD.Controls.Add(this.RdbSendHEX);
			this.GrbSendCMD.Controls.Add(this.RdbSendString);
			this.GrbSendCMD.Controls.Add(this.TxbSendData);
			this.GrbSendCMD.Controls.Add(this.BtnSendCMD);
			this.GrbSendCMD.Location = new System.Drawing.Point(294, 3);
			this.GrbSendCMD.Name = "GrbSendCMD";
			this.GrbSendCMD.Size = new System.Drawing.Size(294, 199);
			this.GrbSendCMD.TabIndex = 10;
			this.GrbSendCMD.TabStop = false;
			this.GrbSendCMD.Text = "发送指令";
			this.LabTipsState.AutoSize = true;
			this.LabTipsState.ForeColor = System.Drawing.Color.Blue;
			this.LabTipsState.Location = new System.Drawing.Point(65, 12);
			this.LabTipsState.Name = "LabTipsState";
			this.LabTipsState.Size = new System.Drawing.Size(113, 12);
			this.LabTipsState.TabIndex = 12;
			this.LabTipsState.Text = "->提示可能不准确-<";
			this.LabTipsState.Visible = false;
			this.LabTipsState.Click += new System.EventHandler(LabTipsState_Click);
			this.LabSendFailed.AutoSize = true;
			this.LabSendFailed.ForeColor = System.Drawing.Color.Red;
			this.LabSendFailed.Location = new System.Drawing.Point(217, 13);
			this.LabSendFailed.Name = "LabSendFailed";
			this.LabSendFailed.Size = new System.Drawing.Size(53, 12);
			this.LabSendFailed.TabIndex = 11;
			this.LabSendFailed.Text = "发送失败";
			this.LabSendFailed.Visible = false;
			this.LabSendOk.AutoSize = true;
			this.LabSendOk.ForeColor = System.Drawing.Color.Lime;
			this.LabSendOk.Location = new System.Drawing.Point(221, 13);
			this.LabSendOk.Name = "LabSendOk";
			this.LabSendOk.Size = new System.Drawing.Size(53, 12);
			this.LabSendOk.TabIndex = 10;
			this.LabSendOk.Text = "发送成功";
			this.LabSendOk.Visible = false;
			this.RdbSendHEX.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.RdbSendHEX.AutoSize = true;
			this.RdbSendHEX.Checked = true;
			this.RdbSendHEX.Location = new System.Drawing.Point(84, 169);
			this.RdbSendHEX.Name = "RdbSendHEX";
			this.RdbSendHEX.Size = new System.Drawing.Size(89, 16);
			this.RdbSendHEX.TabIndex = 9;
			this.RdbSendHEX.TabStop = true;
			this.RdbSendHEX.Text = "十六进制HEX";
			this.RdbSendHEX.UseVisualStyleBackColor = true;
			this.RdbSendString.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.RdbSendString.AutoSize = true;
			this.RdbSendString.Location = new System.Drawing.Point(6, 169);
			this.RdbSendString.Name = "RdbSendString";
			this.RdbSendString.Size = new System.Drawing.Size(77, 16);
			this.RdbSendString.TabIndex = 8;
			this.RdbSendString.Text = "字符Ascii";
			this.RdbSendString.UseVisualStyleBackColor = true;
			this.GrbCrcCalc.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbCrcCalc.Controls.Add(this.TxtShowCRC);
			this.GrbCrcCalc.Controls.Add(this.BtnCalcCMD);
			this.GrbCrcCalc.Controls.Add(this.TxbCrcdInputData);
			this.GrbCrcCalc.Location = new System.Drawing.Point(293, 205);
			this.GrbCrcCalc.Name = "GrbCrcCalc";
			this.GrbCrcCalc.Size = new System.Drawing.Size(294, 95);
			this.GrbCrcCalc.TabIndex = 11;
			this.GrbCrcCalc.TabStop = false;
			this.GrbCrcCalc.Text = "CRC计算";
			this.TxtShowCRC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.TxtShowCRC.Location = new System.Drawing.Point(214, 18);
			this.TxtShowCRC.Multiline = true;
			this.TxtShowCRC.Name = "TxtShowCRC";
			this.TxtShowCRC.ReadOnly = true;
			this.TxtShowCRC.Size = new System.Drawing.Size(74, 63);
			this.TxtShowCRC.TabIndex = 9;
			this.BtnCalcCMD.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			this.BtnCalcCMD.Location = new System.Drawing.Point(185, 18);
			this.BtnCalcCMD.Name = "BtnCalcCMD";
			this.BtnCalcCMD.Size = new System.Drawing.Size(23, 63);
			this.BtnCalcCMD.TabIndex = 8;
			this.BtnCalcCMD.Text = "计 算";
			this.BtnCalcCMD.UseVisualStyleBackColor = true;
			this.BtnCalcCMD.Click += new System.EventHandler(BtnCalcCMD_Click);
			this.TxbCrcdInputData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TxbCrcdInputData.Location = new System.Drawing.Point(7, 18);
			this.TxbCrcdInputData.Multiline = true;
			this.TxbCrcdInputData.Name = "TxbCrcdInputData";
			this.TxbCrcdInputData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxbCrcdInputData.Size = new System.Drawing.Size(172, 63);
			this.TxbCrcdInputData.TabIndex = 8;
			this.GrbExplain.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbExplain.Controls.Add(this.TxbShowWriteCMD);
			this.GrbExplain.Controls.Add(this.TxbShowReadCMD);
			this.GrbExplain.Location = new System.Drawing.Point(294, 306);
			this.GrbExplain.Name = "GrbExplain";
			this.GrbExplain.Size = new System.Drawing.Size(292, 183);
			this.GrbExplain.TabIndex = 12;
			this.GrbExplain.TabStop = false;
			this.GrbExplain.Text = "指令说明";
			this.TxbShowWriteCMD.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TxbShowWriteCMD.Location = new System.Drawing.Point(6, 99);
			this.TxbShowWriteCMD.Multiline = true;
			this.TxbShowWriteCMD.Name = "TxbShowWriteCMD";
			this.TxbShowWriteCMD.ReadOnly = true;
			this.TxbShowWriteCMD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxbShowWriteCMD.Size = new System.Drawing.Size(280, 78);
			this.TxbShowWriteCMD.TabIndex = 11;
			this.TxbShowReadCMD.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TxbShowReadCMD.Location = new System.Drawing.Point(6, 15);
			this.TxbShowReadCMD.Multiline = true;
			this.TxbShowReadCMD.Name = "TxbShowReadCMD";
			this.TxbShowReadCMD.ReadOnly = true;
			this.TxbShowReadCMD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxbShowReadCMD.Size = new System.Drawing.Size(280, 81);
			this.TxbShowReadCMD.TabIndex = 10;
			this.CmsClickMid.AutoSize = false;
			this.CmsClickMid.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.TtbIDName, this.TtbDispName, this.TcbSeclectTypeMid, this.TtbInputAddrMid, this.TtbInputMaskMid, this.TtbInputDataMid, this.TsmAddLittleKey, this.TsmAddBigKey });
			this.CmsClickMid.Name = "CmsClickRight";
			this.CmsClickMid.ShowImageMargin = false;
			this.CmsClickMid.ShowItemToolTips = false;
			this.CmsClickMid.Size = new System.Drawing.Size(120, 205);
			this.TtbIDName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TtbIDName.MaxLength = 500;
			this.TtbIDName.Name = "TtbIDName";
			this.TtbIDName.Size = new System.Drawing.Size(100, 23);
			this.TtbDispName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TtbDispName.MaxLength = 50;
			this.TtbDispName.Name = "TtbDispName";
			this.TtbDispName.Size = new System.Drawing.Size(100, 23);
			this.TcbSeclectTypeMid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TcbSeclectTypeMid.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.TcbSeclectTypeMid.Items.AddRange(new object[9] { "‘R’", "‘X’", "‘Y’", "‘Z’", "‘T’", "‘L’", "‘I’", "‘K’", "无" });
			this.TcbSeclectTypeMid.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.TcbSeclectTypeMid.Name = "TcbSeclectTypeMid";
			this.TcbSeclectTypeMid.Size = new System.Drawing.Size(100, 25);
			this.TtbInputAddrMid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TtbInputAddrMid.MaxLength = 2;
			this.TtbInputAddrMid.Name = "TtbInputAddrMid";
			this.TtbInputAddrMid.Size = new System.Drawing.Size(100, 23);
			this.TtbInputMaskMid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TtbInputMaskMid.MaxLength = 2;
			this.TtbInputMaskMid.Name = "TtbInputMaskMid";
			this.TtbInputMaskMid.Size = new System.Drawing.Size(100, 23);
			this.TtbInputDataMid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TtbInputDataMid.MaxLength = 2;
			this.TtbInputDataMid.Name = "TtbInputDataMid";
			this.TtbInputDataMid.Size = new System.Drawing.Size(100, 23);
			this.TsmAddLittleKey.Name = "TsmAddLittleKey";
			this.TsmAddLittleKey.Size = new System.Drawing.Size(135, 22);
			this.TsmAddLittleKey.Text = "    添加到子键";
			this.TsmAddLittleKey.Click += new System.EventHandler(TsmAddLittleKey_Click);
			this.TsmAddBigKey.Name = "TsmAddBigKey";
			this.TsmAddBigKey.Size = new System.Drawing.Size(135, 22);
			this.TsmAddBigKey.Text = "    添加到主键";
			this.TsmAddBigKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.TsmAddBigKey.Click += new System.EventHandler(TsmAddBigKey_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(601, 498);
			base.Controls.Add(this.GrbExplain);
			base.Controls.Add(this.GrbCrcCalc);
			base.Controls.Add(this.GrbSendCMD);
			base.Controls.Add(this.GrbSettingList);
			base.KeyPreview = true;
			base.Name = "CMD_Tool";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "命令配置工具";
			base.Load += new System.EventHandler(CMD_Tool_Load);
			this.CmsClickRight.ResumeLayout(false);
			this.CmsClickRight.PerformLayout();
			this.GrbSettingList.ResumeLayout(false);
			this.GrbSettingList.PerformLayout();
			this.GrbSendCMD.ResumeLayout(false);
			this.GrbSendCMD.PerformLayout();
			this.GrbCrcCalc.ResumeLayout(false);
			this.GrbCrcCalc.PerformLayout();
			this.GrbExplain.ResumeLayout(false);
			this.GrbExplain.PerformLayout();
			this.CmsClickMid.ResumeLayout(false);
			this.CmsClickMid.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
