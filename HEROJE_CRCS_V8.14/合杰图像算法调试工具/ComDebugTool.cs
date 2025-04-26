using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using UsbRequestFuntion;

namespace 合杰图像算法调试工具
{
	public class ComDebugTool : Form
	{
		private SerialPort SP_Instance = new SerialPort();

		private StringBuilder DataToString = new StringBuilder();

		private int ReceiveCount = 0;

		private int SendCount = 0;

		private long t1 = 0L;

		private long t2 = 0L;

		private bool DispTimeFlag = false;

		private bool Rx_TxFlag = false;

		private bool time_flag = false;

		private bool Interval_Flag = false;

		private ushort[] ReceiveDebugData = new ushort[400];

		private static uint[] crc32_tab = new uint[256]
		{
			0u, 1996959894u, 3993919788u, 2567524794u, 124634137u, 1886057615u, 3915621685u, 2657392035u, 249268274u, 2044508324u,
			3772115230u, 2547177864u, 162941995u, 2125561021u, 3887607047u, 2428444049u, 498536548u, 1789927666u, 4089016648u, 2227061214u,
			450548861u, 1843258603u, 4107580753u, 2211677639u, 325883990u, 1684777152u, 4251122042u, 2321926636u, 335633487u, 1661365465u,
			4195302755u, 2366115317u, 997073096u, 1281953886u, 3579855332u, 2724688242u, 1006888145u, 1258607687u, 3524101629u, 2768942443u,
			901097722u, 1119000684u, 3686517206u, 2898065728u, 853044451u, 1172266101u, 3705015759u, 2882616665u, 651767980u, 1373503546u,
			3369554304u, 3218104598u, 565507253u, 1454621731u, 3485111705u, 3099436303u, 671266974u, 1594198024u, 3322730930u, 2970347812u,
			795835527u, 1483230225u, 3244367275u, 3060149565u, 1994146192u, 31158534u, 2563907772u, 4023717930u, 1907459465u, 112637215u,
			2680153253u, 3904427059u, 2013776290u, 251722036u, 2517215374u, 3775830040u, 2137656763u, 141376813u, 2439277719u, 3865271297u,
			1802195444u, 476864866u, 2238001368u, 4066508878u, 1812370925u, 453092731u, 2181625025u, 4111451223u, 1706088902u, 314042704u,
			2344532202u, 4240017532u, 1658658271u, 366619977u, 2362670323u, 4224994405u, 1303535960u, 984961486u, 2747007092u, 3569037538u,
			1256170817u, 1037604311u, 2765210733u, 3554079995u, 1131014506u, 879679996u, 2909243462u, 3663771856u, 1141124467u, 855842277u,
			2852801631u, 3708648649u, 1342533948u, 654459306u, 3188396048u, 3373015174u, 1466479909u, 544179635u, 3110523913u, 3462522015u,
			1591671054u, 702138776u, 2966460450u, 3352799412u, 1504918807u, 783551873u, 3082640443u, 3233442989u, 3988292384u, 2596254646u,
			62317068u, 1957810842u, 3939845945u, 2647816111u, 81470997u, 1943803523u, 3814918930u, 2489596804u, 225274430u, 2053790376u,
			3826175755u, 2466906013u, 167816743u, 2097651377u, 4027552580u, 2265490386u, 503444072u, 1762050814u, 4150417245u, 2154129355u,
			426522225u, 1852507879u, 4275313526u, 2312317920u, 282753626u, 1742555852u, 4189708143u, 2394877945u, 397917763u, 1622183637u,
			3604390888u, 2714866558u, 953729732u, 1340076626u, 3518719985u, 2797360999u, 1068828381u, 1219638859u, 3624741850u, 2936675148u,
			906185462u, 1090812512u, 3747672003u, 2825379669u, 829329135u, 1181335161u, 3412177804u, 3160834842u, 628085408u, 1382605366u,
			3423369109u, 3138078467u, 570562233u, 1426400815u, 3317316542u, 2998733608u, 733239954u, 1555261956u, 3268935591u, 3050360625u,
			752459403u, 1541320221u, 2607071920u, 3965973030u, 1969922972u, 40735498u, 2617837225u, 3943577151u, 1913087877u, 83908371u,
			2512341634u, 3803740692u, 2075208622u, 213261112u, 2463272603u, 3855990285u, 2094854071u, 198958881u, 2262029012u, 4057260610u,
			1759359992u, 534414190u, 2176718541u, 4139329115u, 1873836001u, 414664567u, 2282248934u, 4279200368u, 1711684554u, 285281116u,
			2405801727u, 4167216745u, 1634467795u, 376229701u, 2685067896u, 3608007406u, 1308918612u, 956543938u, 2808555105u, 3495958263u,
			1231636301u, 1047427035u, 2932959818u, 3654703836u, 1088359270u, 936918000u, 2847714899u, 3736837829u, 1202900863u, 817233897u,
			3183342108u, 3401237130u, 1404277552u, 615818150u, 3134207493u, 3453421203u, 1423857449u, 601450431u, 3009837614u, 3294710456u,
			1567103746u, 711928724u, 3020668471u, 3272380065u, 1510334235u, 755167117u
		};

		private System.Timers.Timer UartAutoConnectTimer;

		private System.Timers.Timer timer;

		private System.Timers.Timer Sendtime;

		private IContainer components = null;

		private Label TxtUartState;

		private ComboBox CobUartSelect;

		private Label TxtUartSelect;

		private Label TxtBaudRateSelect;

		private ComboBox CobBaudRateSelect;

		private Button ButOpenUart;

		private TextBox TxbDataReceive;

		private Label TxtSendCount;

		private Label TxtReceiveCount;

		private Button ButSendData;

		private RadioButton RdbStringDisplay;

		private RadioButton RdbHexDisplay;

		private TextBox TxbDataSend;

		private RadioButton RdbSendString;

		private RadioButton RdbSendHex;

		private GroupBox GdbDataSend;

		private GroupBox GdbDataReceive;

		private OpenFileDialog OpenFileDlg;

		private GroupBox GrpComDebug;

		private ComboBox CobDispEncode;

		private Label LabDispEncode;

		private CheckBox ChkDispTime;

		private CheckBox ChkSendByTime;

		private Label LabCycleTimeUint;

		private Label LabCycleTime;

		private NumericUpDown numericUpDown;

		private uint crc32(byte[] buf, int len)
		{
			uint num = 0u;
			num = ~num;
			for (int i = 4; i < len; i++)
			{
				num = crc32_tab[(num ^ buf[i]) & 0xFF] ^ (num >> 8);
			}
			return ~num;
		}

		public ComDebugTool()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Heroje_Standard)
			{
				Text = "HEROJE 串口调试工具V1.4";
			}
			else if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Hide_Heroje_Logo)
			{
				Text = "串口调试工具V1.4";
			}
			ControlAndXML controlAndXML = new ControlAndXML();
			controlAndXML.XMLToControlByLinq(ToolCfg.ConfigPath, this);
			CobDispEncode.SelectedIndex = 0;
			Control.CheckForIllegalCrossThreadCalls = false;
			SP_Instance.DataReceived += SP1_DataReceived;
			UartAutoConnectTimer = new System.Timers.Timer(1000.0);
			UartAutoConnectTimer.Elapsed += UartAutoConnectTimer_Elapsed;
			UartAutoConnectTimer.Start();
			string[] array = USB.MulGetHardwareInfo(HardwareEnum.Win32_SerialPort, "Name");
			timer = new System.Timers.Timer(40.0);
			timer.Elapsed += test;
			timer.AutoReset = true;
			timer.Enabled = true;
			timer.Stop();
			Sendtime = new System.Timers.Timer(1000.0);
			Sendtime.Elapsed += Send_Text;
			Sendtime.AutoReset = true;
			Sendtime.Enabled = true;
			Sendtime.Stop();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length > 25)
				{
					string text = array[i].Substring(0, 10);
					array[i] = text + "..." + array[i].Substring(array[i].Length - 12, 12);
				}
			}
			ComboBox.ObjectCollection items = CobUartSelect.Items;
			object[] items2 = array;
			items.AddRange(items2);
			try
			{
				CobUartSelect.SelectedIndex = 0;
				SP_Instance.PortName = CobUartSelect.Text;
			}
			catch
			{
			}
			CobBaudRateSelect.Text = "9600";
			SP_Instance.BaudRate = 9600;
			SP_Instance.Parity = Parity.None;
			SP_Instance.NewLine = "\r\n";
			SP_Instance.Close();
			ButSendData.Enabled = false;
			RdbSendString.Checked = true;
			RdbStringDisplay.Checked = true;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				TxtUartState.Text = "状态：关闭(" + CobUartSelect.Text + ")";
			}
			else
			{
				TxtUartState.Text = "State：Close(" + CobUartSelect.Text + ")";
			}
		}

		private void ButOpenUart_Click(object sender, EventArgs e)
		{
			if (SP_Instance.IsOpen)
			{
				try
				{
					SP_Instance.Close();
				}
				catch
				{
				}
				CobUartSelect.Enabled = true;
				CobBaudRateSelect.Enabled = true;
				ButSendData.Enabled = false;
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					ButOpenUart.Text = "打开串口";
					TxtUartState.Text = "状态：关闭(" + CobUartSelect.Text + ")";
				}
				else
				{
					ButOpenUart.Text = "OpenCom";
					TxtUartState.Text = "State：Close(" + CobUartSelect.Text + ")";
				}
				return;
			}
			if (CobUartSelect.Text == "")
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.SelectComTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.SelectComTips), MessageBoxButtons.OK);
				return;
			}
			try
			{
				int num = Convert.ToInt32(CobBaudRateSelect.Text);
				SP_Instance.BaudRate = num;
				OpenUSB_COM(CobUartSelect.Text, num);
				CobUartSelect.Enabled = false;
				CobBaudRateSelect.Enabled = false;
				ButSendData.Enabled = true;
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					ButOpenUart.Text = "关闭串口";
					TxtUartState.Text = "状态：打开(" + CobUartSelect.Text + ")";
				}
				else
				{
					ButOpenUart.Text = "OpenCom";
					TxtUartState.Text = "State：Open(" + CobUartSelect.Text + ")";
				}
			}
			catch (Exception ex)
			{
				CobUartSelect.Enabled = true;
				CobBaudRateSelect.Enabled = true;
				ButSendData.Enabled = false;
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					ButOpenUart.Text = "打开串口";
					TxtUartState.Text = "状态：关闭(" + CobUartSelect.Text + ")";
					MessageBox.Show(ex.Message, "打开串口失败");
				}
				else
				{
					ButOpenUart.Text = "OpenCom";
					TxtUartState.Text = "State：Close(" + CobUartSelect.Text + ")";
					MessageBox.Show(ex.Message, "OpenFailed");
				}
			}
		}

		private void ButSendData_Click(object sender, EventArgs e)
		{
			if (SP_Instance.IsOpen)
			{
				if (DispTimeFlag)
				{
					Rx_TxFlag = true;
					t1 = DateTime.Now.Ticks / 10000;
				}
				if (RdbSendString.Checked)
				{
					SP_Instance.Write(TxbDataSend.Text);
					SendCount += TxbDataSend.Text.Length;
				}
				else if (Regex.IsMatch(TxbDataSend.Text, "(?i)[\\da-f]{2}"))
				{
					MatchCollection matchCollection = Regex.Matches(TxbDataSend.Text, "(?i)[\\da-f]{2}");
					List<byte> list = new List<byte>();
					foreach (Match item in matchCollection)
					{
						list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
					}
					SP_Instance.Write(list.ToArray(), 0, list.Count);
					SendCount += list.Count;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的十六进制字符，如“0A”“0x0a”", "提示");
				}
				else
				{
					MessageBox.Show("Please input hex Value，Like \"0A\"\"0x0a\"", "Note");
				}
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					TxtSendCount.Text = "发送字节：" + Convert.ToString(SendCount);
				}
				else
				{
					TxtSendCount.Text = "TxCount:" + Convert.ToString(SendCount);
				}
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("请打开串口后再发送。", "提示");
			}
			else
			{
				MessageBox.Show("Please send before open com", "Warning");
			}
		}

		private void SP1_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			try
			{
				Thread.Sleep(8);
				if (Rx_TxFlag)
				{
					timer.Start();
					Interval_Flag = true;
				}
				int bytesToRead = SP_Instance.BytesToRead;
				byte[] array = new byte[bytesToRead];
				ReceiveCount += SP_Instance.Read(array, 0, bytesToRead);
				DataToString.Clear();
				if (RdbStringDisplay.Checked)
				{
					bool flag = false;
					if (array[bytesToRead - 1] == 13 || array[bytesToRead - 1] == 10)
					{
						array[bytesToRead - 1] = 0;
						flag = true;
					}
					if (array[bytesToRead - 2] == 13 && array[bytesToRead - 2] == 10)
					{
						array[bytesToRead - 1] = 0;
						array[bytesToRead - 2] = 0;
						flag = true;
					}
					string text = CobDispEncode.Items[CobDispEncode.SelectedIndex].ToString();
					string text2;
					switch (text)
					{
					case "Default":
						text2 = Encoding.Default.GetString(array).Replace("\0", "");
						break;
					case "Unicode(big)":
						text2 = Encoding.BigEndianUnicode.GetString(array).Replace("\0", "");
						break;
					case "Unicode(little)":
						text2 = Encoding.Unicode.GetString(array).Replace("\0", "");
						break;
					default:
						text2 = Encoding.GetEncoding(text).GetString(array).Replace("\0", "");
						break;
					}
					if (flag)
					{
						text2 += Environment.NewLine;
					}
					DataToString.Append(text2);
				}
				else
				{
					byte[] array2 = array;
					foreach (byte b in array2)
					{
						DataToString.Append(b.ToString("X2") + " ");
					}
				}
				TxbDataReceive.AppendText(DataToString.ToString());
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					TxtReceiveCount.Text = "接收字节：" + Convert.ToString(ReceiveCount);
				}
				else
				{
					TxtReceiveCount.Text = "RxCount:" + Convert.ToString(ReceiveCount);
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("端口已关闭") || ex.Message.Contains("Close"))
				{
					CobUartSelect.Enabled = true;
					CobBaudRateSelect.Enabled = true;
					ButSendData.Enabled = false;
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						ButOpenUart.Text = "打开串口";
						TxtUartState.Text = "状态：关闭(" + CobUartSelect.Text + ")";
					}
					else
					{
						ButOpenUart.Text = "OpenCom";
						TxtUartState.Text = "State：Close(" + CobUartSelect.Text + ")";
					}
				}
			}
		}

		private void TxbDataReceive_TextChanged(object sender, EventArgs e)
		{
			TxbDataReceive.SelectionStart = TxbDataReceive.Text.Length;
			TxbDataReceive.ScrollToCaret();
		}

		private void CobUartSelect_DropDown(object sender, EventArgs e)
		{
			string[] array = USB.MulGetHardwareInfo(HardwareEnum.Win32_SerialPort, "Name");
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length > 25)
				{
					string text = array[i].Substring(0, 10);
					array[i] = text + "..." + array[i].Substring(array[i].Length - 12, 12);
				}
			}
			if (array.GetLength(0) != CobUartSelect.Items.Count)
			{
				CobUartSelect.Items.Clear();
				ComboBox.ObjectCollection items = CobUartSelect.Items;
				object[] items2 = array;
				items.AddRange(items2);
			}
		}

		private void TxtSendCount_DoubleClick(object sender, EventArgs e)
		{
			string caption;
			string text;
			string text2;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				caption = "提示";
				text = "是否清零发送字节计数？";
				text2 = "发送字节：0";
			}
			else
			{
				caption = "Tips";
				text = "Clear Tx Count ?";
				text2 = "TxCount:0";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				SendCount = 0;
				TxtSendCount.Text = text2;
			}
		}

		private void TxtReceiveCount_DoubleClick(object sender, EventArgs e)
		{
			string caption;
			string text;
			string text2;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				caption = "提示";
				text = "是否清零接收字节计数？";
				text2 = "接收字节：0";
			}
			else
			{
				caption = "Tips";
				text = "Clear Rx Count ?";
				text2 = "RxCount:0";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				ReceiveCount = 0;
				TxtSendCount.Text = text2;
			}
		}

		private void TxbDataReceive_DoubleClick(object sender, EventArgs e)
		{
			string caption;
			string text;
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				caption = "提示";
				text = "是否清除接收数据？";
			}
			else
			{
				caption = "Tips";
				text = "Clear Rx Text ?";
			}
			if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				TxbDataReceive.Clear();
			}
		}

		private void CobUartSelect_TextChanged(object sender, EventArgs e)
		{
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				TxtUartState.Text = "状态：关闭(" + CobUartSelect.Text + ")";
			}
			else
			{
				TxtUartState.Text = "State：Close(" + CobUartSelect.Text + ")";
			}
		}

		private void OpenUSB_COM(string com, int baudrate)
		{
			int num = com.IndexOf("(COM");
			string text = com.Substring(num + 1, 5);
			if (text[4] == ')')
			{
				text = text.Substring(0, 4);
			}
			SP_Instance.PortName = text;
			SP_Instance.BaudRate = baudrate;
			SP_Instance.Parity = Parity.None;
			SP_Instance.NewLine = "\r\n";
			SP_Instance.Open();
		}

		private void UartAutoConnectTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (!SP_Instance.IsOpen)
			{
				CobUartSelect.Enabled = true;
				CobBaudRateSelect.Enabled = true;
				ButSendData.Enabled = false;
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					ButOpenUart.Text = "打开串口";
					TxtUartState.Text = "状态：关闭(" + CobUartSelect.Text + ")";
				}
				else
				{
					ButOpenUart.Text = "OpenCom";
					TxtUartState.Text = "State：Close(" + CobUartSelect.Text + ")";
				}
			}
		}

		private void test(object source, ElapsedEventArgs e)
		{
			if (Interval_Flag)
			{
				Interval_Flag = false;
				time_flag = true;
			}
			else if (Rx_TxFlag && time_flag)
			{
				timer.Stop();
				time_flag = false;
				Rx_TxFlag = false;
				t2 = DateTime.Now.Ticks / 10000;
				string text = ((t2 - t1 <= 80) ? (" (" + Convert.ToString(t2 - t1 - 80) + "ms) \n") : (" (" + Convert.ToString(t2 - t1) + "ms) \n"));
				TxbDataReceive.AppendText(text);
			}
		}

		private void Send_Text(object source, ElapsedEventArgs e)
		{
			if (!ChkSendByTime.Checked)
			{
				return;
			}
			if (SP_Instance.IsOpen)
			{
				if (DispTimeFlag)
				{
					Rx_TxFlag = true;
					t1 = DateTime.Now.Ticks / 10000;
				}
				if (RdbSendString.Checked)
				{
					SP_Instance.Write(TxbDataSend.Text);
					SendCount += TxbDataSend.Text.Length;
				}
				else if (Regex.IsMatch(TxbDataSend.Text, "(?i)[\\da-f]{2}"))
				{
					MatchCollection matchCollection = Regex.Matches(TxbDataSend.Text, "(?i)[\\da-f]{2}");
					List<byte> list = new List<byte>();
					foreach (Match item in matchCollection)
					{
						list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
					}
					SP_Instance.Write(list.ToArray(), 0, list.Count);
					SendCount += list.Count;
				}
				else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					MessageBox.Show("请输入正确的十六进制字符，如“0A”“0x0a”", "提示");
				}
				else
				{
					MessageBox.Show("Please input hex Value，Like \"0A\"\"0x0a\"", "Note");
				}
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					TxtSendCount.Text = "发送字节：" + Convert.ToString(SendCount);
				}
				else
				{
					TxtSendCount.Text = "TxCount:" + Convert.ToString(SendCount);
				}
			}
			else if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				MessageBox.Show("请打开串口后再发送。", "提示");
			}
			else
			{
				MessageBox.Show("Please send before open com", "Warning");
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				SP_Instance.Close();
				SP_Instance.Dispose();
				UartAutoConnectTimer.Close();
				UartAutoConnectTimer.Dispose();
			}
			catch
			{
			}
		}

		private void CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (ChkDispTime.Checked)
			{
				DispTimeFlag = true;
				return;
			}
			DispTimeFlag = false;
			timer.Stop();
		}

		private void TxbDataSend_TextChanged(object sender, EventArgs e)
		{
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
		}

		private void GdbDataReceive_Enter(object sender, EventArgs e)
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{
		}

		private void timer_Tick(object sender, EventArgs e)
		{
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (ChkSendByTime.Checked)
			{
				if (!SP_Instance.IsOpen)
				{
					ChkSendByTime.Checked = false;
					if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
					{
						MessageBox.Show("请打开串口后再发送。", "提示");
					}
					else
					{
						MessageBox.Show("Please send after open com", "Warning");
					}
				}
				else
				{
					numericUpDown.Enabled = false;
					Sendtime.Interval = (long)numericUpDown.Value;
					Sendtime.Start();
				}
			}
			else
			{
				numericUpDown.Enabled = true;
				Sendtime.Stop();
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
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
			this.SP_Instance = new System.IO.Ports.SerialPort(this.components);
			this.TxtUartState = new System.Windows.Forms.Label();
			this.CobUartSelect = new System.Windows.Forms.ComboBox();
			this.TxtUartSelect = new System.Windows.Forms.Label();
			this.TxtBaudRateSelect = new System.Windows.Forms.Label();
			this.CobBaudRateSelect = new System.Windows.Forms.ComboBox();
			this.ButOpenUart = new System.Windows.Forms.Button();
			this.TxbDataReceive = new System.Windows.Forms.TextBox();
			this.TxtSendCount = new System.Windows.Forms.Label();
			this.TxtReceiveCount = new System.Windows.Forms.Label();
			this.ButSendData = new System.Windows.Forms.Button();
			this.RdbStringDisplay = new System.Windows.Forms.RadioButton();
			this.RdbHexDisplay = new System.Windows.Forms.RadioButton();
			this.TxbDataSend = new System.Windows.Forms.TextBox();
			this.RdbSendString = new System.Windows.Forms.RadioButton();
			this.RdbSendHex = new System.Windows.Forms.RadioButton();
			this.GdbDataSend = new System.Windows.Forms.GroupBox();
			this.numericUpDown = new System.Windows.Forms.NumericUpDown();
			this.ChkSendByTime = new System.Windows.Forms.CheckBox();
			this.LabCycleTimeUint = new System.Windows.Forms.Label();
			this.LabCycleTime = new System.Windows.Forms.Label();
			this.ChkDispTime = new System.Windows.Forms.CheckBox();
			this.GdbDataReceive = new System.Windows.Forms.GroupBox();
			this.CobDispEncode = new System.Windows.Forms.ComboBox();
			this.LabDispEncode = new System.Windows.Forms.Label();
			this.OpenFileDlg = new System.Windows.Forms.OpenFileDialog();
			this.GrpComDebug = new System.Windows.Forms.GroupBox();
			this.GdbDataSend.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.numericUpDown).BeginInit();
			this.GdbDataReceive.SuspendLayout();
			this.GrpComDebug.SuspendLayout();
			base.SuspendLayout();
			this.TxtUartState.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.TxtUartState.AutoSize = true;
			this.TxtUartState.Location = new System.Drawing.Point(7, 426);
			this.TxtUartState.Name = "TxtUartState";
			this.TxtUartState.Size = new System.Drawing.Size(41, 12);
			this.TxtUartState.TabIndex = 0;
			this.TxtUartState.Text = "状态：";
			this.CobUartSelect.BackColor = System.Drawing.SystemColors.Window;
			this.CobUartSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobUartSelect.FormattingEnabled = true;
			this.CobUartSelect.Location = new System.Drawing.Point(76, 14);
			this.CobUartSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.CobUartSelect.Name = "CobUartSelect";
			this.CobUartSelect.Size = new System.Drawing.Size(175, 20);
			this.CobUartSelect.TabIndex = 1;
			this.CobUartSelect.DropDown += new System.EventHandler(CobUartSelect_DropDown);
			this.CobUartSelect.TextChanged += new System.EventHandler(CobUartSelect_TextChanged);
			this.TxtUartSelect.AutoSize = true;
			this.TxtUartSelect.Location = new System.Drawing.Point(8, 18);
			this.TxtUartSelect.Name = "TxtUartSelect";
			this.TxtUartSelect.Size = new System.Drawing.Size(65, 12);
			this.TxtUartSelect.TabIndex = 2;
			this.TxtUartSelect.Text = "选择串口：";
			this.TxtBaudRateSelect.AutoSize = true;
			this.TxtBaudRateSelect.Location = new System.Drawing.Point(257, 18);
			this.TxtBaudRateSelect.Name = "TxtBaudRateSelect";
			this.TxtBaudRateSelect.Size = new System.Drawing.Size(53, 12);
			this.TxtBaudRateSelect.TabIndex = 3;
			this.TxtBaudRateSelect.Text = "波特率：";
			this.CobBaudRateSelect.Items.AddRange(new object[7] { "2400", "4800", "9600", "19200", "38400", "57600", "115200" });
			this.CobBaudRateSelect.Location = new System.Drawing.Point(306, 14);
			this.CobBaudRateSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.CobBaudRateSelect.Name = "CobBaudRateSelect";
			this.CobBaudRateSelect.Size = new System.Drawing.Size(76, 20);
			this.CobBaudRateSelect.TabIndex = 4;
			this.ButOpenUart.Location = new System.Drawing.Point(388, 12);
			this.ButOpenUart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.ButOpenUart.Name = "ButOpenUart";
			this.ButOpenUart.Size = new System.Drawing.Size(68, 22);
			this.ButOpenUart.TabIndex = 7;
			this.ButOpenUart.Text = "打开串口";
			this.ButOpenUart.UseVisualStyleBackColor = true;
			this.ButOpenUart.Click += new System.EventHandler(ButOpenUart_Click);
			this.TxbDataReceive.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TxbDataReceive.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.TxbDataReceive.Location = new System.Drawing.Point(5, 34);
			this.TxbDataReceive.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.TxbDataReceive.Multiline = true;
			this.TxbDataReceive.Name = "TxbDataReceive";
			this.TxbDataReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxbDataReceive.Size = new System.Drawing.Size(506, 222);
			this.TxbDataReceive.TabIndex = 9;
			this.TxbDataReceive.TextChanged += new System.EventHandler(TxbDataReceive_TextChanged);
			this.TxbDataReceive.DoubleClick += new System.EventHandler(TxbDataReceive_DoubleClick);
			this.TxtSendCount.AutoSize = true;
			this.TxtSendCount.Location = new System.Drawing.Point(399, 15);
			this.TxtSendCount.Name = "TxtSendCount";
			this.TxtSendCount.Size = new System.Drawing.Size(71, 12);
			this.TxtSendCount.TabIndex = 12;
			this.TxtSendCount.Text = "发送字节：0";
			this.TxtSendCount.DoubleClick += new System.EventHandler(TxtSendCount_DoubleClick);
			this.TxtReceiveCount.AutoSize = true;
			this.TxtReceiveCount.Location = new System.Drawing.Point(399, 18);
			this.TxtReceiveCount.Name = "TxtReceiveCount";
			this.TxtReceiveCount.Size = new System.Drawing.Size(71, 12);
			this.TxtReceiveCount.TabIndex = 13;
			this.TxtReceiveCount.Text = "接收字节：0";
			this.TxtReceiveCount.DoubleClick += new System.EventHandler(TxtReceiveCount_DoubleClick);
			this.ButSendData.Location = new System.Drawing.Point(462, 12);
			this.ButSendData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.ButSendData.Name = "ButSendData";
			this.ButSendData.Size = new System.Drawing.Size(63, 22);
			this.ButSendData.TabIndex = 14;
			this.ButSendData.Text = "发送数据";
			this.ButSendData.UseVisualStyleBackColor = true;
			this.ButSendData.Click += new System.EventHandler(ButSendData_Click);
			this.RdbStringDisplay.AutoSize = true;
			this.RdbStringDisplay.Location = new System.Drawing.Point(8, 14);
			this.RdbStringDisplay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.RdbStringDisplay.Name = "RdbStringDisplay";
			this.RdbStringDisplay.Size = new System.Drawing.Size(59, 16);
			this.RdbStringDisplay.TabIndex = 17;
			this.RdbStringDisplay.Text = "字符串";
			this.RdbStringDisplay.UseVisualStyleBackColor = true;
			this.RdbHexDisplay.AutoSize = true;
			this.RdbHexDisplay.Location = new System.Drawing.Point(86, 14);
			this.RdbHexDisplay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.RdbHexDisplay.Name = "RdbHexDisplay";
			this.RdbHexDisplay.Size = new System.Drawing.Size(71, 16);
			this.RdbHexDisplay.TabIndex = 18;
			this.RdbHexDisplay.Text = "十六进制";
			this.RdbHexDisplay.UseVisualStyleBackColor = true;
			this.TxbDataSend.AcceptsReturn = true;
			this.TxbDataSend.AllowDrop = true;
			this.TxbDataSend.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.TxbDataSend.Location = new System.Drawing.Point(5, 34);
			this.TxbDataSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.TxbDataSend.Multiline = true;
			this.TxbDataSend.Name = "TxbDataSend";
			this.TxbDataSend.Size = new System.Drawing.Size(508, 84);
			this.TxbDataSend.TabIndex = 8;
			this.TxbDataSend.TextChanged += new System.EventHandler(TxbDataSend_TextChanged);
			this.RdbSendString.AutoSize = true;
			this.RdbSendString.Location = new System.Drawing.Point(6, 14);
			this.RdbSendString.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.RdbSendString.Name = "RdbSendString";
			this.RdbSendString.Size = new System.Drawing.Size(59, 16);
			this.RdbSendString.TabIndex = 15;
			this.RdbSendString.Text = "字符串";
			this.RdbSendString.UseVisualStyleBackColor = true;
			this.RdbSendHex.AutoSize = true;
			this.RdbSendHex.Location = new System.Drawing.Point(58, 14);
			this.RdbSendHex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.RdbSendHex.Name = "RdbSendHex";
			this.RdbSendHex.Size = new System.Drawing.Size(71, 16);
			this.RdbSendHex.TabIndex = 16;
			this.RdbSendHex.Text = "十六进制";
			this.RdbSendHex.UseVisualStyleBackColor = true;
			this.GdbDataSend.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GdbDataSend.Controls.Add(this.numericUpDown);
			this.GdbDataSend.Controls.Add(this.ChkSendByTime);
			this.GdbDataSend.Controls.Add(this.LabCycleTimeUint);
			this.GdbDataSend.Controls.Add(this.LabCycleTime);
			this.GdbDataSend.Controls.Add(this.ChkDispTime);
			this.GdbDataSend.Controls.Add(this.RdbSendHex);
			this.GdbDataSend.Controls.Add(this.RdbSendString);
			this.GdbDataSend.Controls.Add(this.TxbDataSend);
			this.GdbDataSend.Controls.Add(this.TxtSendCount);
			this.GdbDataSend.Location = new System.Drawing.Point(9, 300);
			this.GdbDataSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.GdbDataSend.Name = "GdbDataSend";
			this.GdbDataSend.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.GdbDataSend.Size = new System.Drawing.Size(519, 124);
			this.GdbDataSend.TabIndex = 19;
			this.GdbDataSend.TabStop = false;
			this.GdbDataSend.Text = "数据发送区";
			this.numericUpDown.Location = new System.Drawing.Point(245, 12);
			this.numericUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDown.Maximum = new decimal(new int[4] { 100000, 0, 0, 0 });
			this.numericUpDown.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
			this.numericUpDown.Name = "numericUpDown";
			this.numericUpDown.Size = new System.Drawing.Size(59, 21);
			this.numericUpDown.TabIndex = 22;
			this.numericUpDown.Value = new decimal(new int[4] { 1000, 0, 0, 0 });
			this.ChkSendByTime.AutoSize = true;
			this.ChkSendByTime.Location = new System.Drawing.Point(126, 14);
			this.ChkSendByTime.Margin = new System.Windows.Forms.Padding(2);
			this.ChkSendByTime.Name = "ChkSendByTime";
			this.ChkSendByTime.Size = new System.Drawing.Size(72, 16);
			this.ChkSendByTime.TabIndex = 21;
			this.ChkSendByTime.Text = "定时发送";
			this.ChkSendByTime.UseVisualStyleBackColor = true;
			this.ChkSendByTime.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
			this.LabCycleTimeUint.AutoSize = true;
			this.LabCycleTimeUint.Font = new System.Drawing.Font("宋体", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.LabCycleTimeUint.Location = new System.Drawing.Point(305, 13);
			this.LabCycleTimeUint.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabCycleTimeUint.Name = "LabCycleTimeUint";
			this.LabCycleTimeUint.Size = new System.Drawing.Size(23, 15);
			this.LabCycleTimeUint.TabIndex = 19;
			this.LabCycleTimeUint.Text = "ms";
			this.LabCycleTime.AutoSize = true;
			this.LabCycleTime.Location = new System.Drawing.Point(199, 15);
			this.LabCycleTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabCycleTime.Name = "LabCycleTime";
			this.LabCycleTime.Size = new System.Drawing.Size(41, 12);
			this.LabCycleTime.TabIndex = 18;
			this.LabCycleTime.Text = "周期：";
			this.LabCycleTime.Click += new System.EventHandler(label1_Click);
			this.ChkDispTime.AutoSize = true;
			this.ChkDispTime.Location = new System.Drawing.Point(328, 14);
			this.ChkDispTime.Margin = new System.Windows.Forms.Padding(2);
			this.ChkDispTime.Name = "ChkDispTime";
			this.ChkDispTime.Size = new System.Drawing.Size(72, 16);
			this.ChkDispTime.TabIndex = 17;
			this.ChkDispTime.Text = "显示时间";
			this.ChkDispTime.UseVisualStyleBackColor = true;
			this.ChkDispTime.CheckedChanged += new System.EventHandler(CheckBox_CheckedChanged);
			this.GdbDataReceive.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GdbDataReceive.Controls.Add(this.CobDispEncode);
			this.GdbDataReceive.Controls.Add(this.LabDispEncode);
			this.GdbDataReceive.Controls.Add(this.RdbHexDisplay);
			this.GdbDataReceive.Controls.Add(this.TxtReceiveCount);
			this.GdbDataReceive.Controls.Add(this.RdbStringDisplay);
			this.GdbDataReceive.Controls.Add(this.TxbDataReceive);
			this.GdbDataReceive.Location = new System.Drawing.Point(9, 34);
			this.GdbDataReceive.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.GdbDataReceive.Name = "GdbDataReceive";
			this.GdbDataReceive.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.GdbDataReceive.Size = new System.Drawing.Size(519, 262);
			this.GdbDataReceive.TabIndex = 20;
			this.GdbDataReceive.TabStop = false;
			this.GdbDataReceive.Text = "数据显示区";
			this.GdbDataReceive.Enter += new System.EventHandler(GdbDataReceive_Enter);
			this.CobDispEncode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobDispEncode.Items.AddRange(new object[6] { "Default", "GBK", "UTF-8", "Unicode(big)", "Unicode(little)", "Big5" });
			this.CobDispEncode.Location = new System.Drawing.Point(250, 12);
			this.CobDispEncode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.CobDispEncode.Name = "CobDispEncode";
			this.CobDispEncode.Size = new System.Drawing.Size(122, 20);
			this.CobDispEncode.TabIndex = 21;
			this.LabDispEncode.AutoSize = true;
			this.LabDispEncode.Location = new System.Drawing.Point(177, 16);
			this.LabDispEncode.Name = "LabDispEncode";
			this.LabDispEncode.Size = new System.Drawing.Size(65, 12);
			this.LabDispEncode.TabIndex = 19;
			this.LabDispEncode.Text = "显示编码：";
			this.OpenFileDlg.FileName = "update.bin";
			this.GrpComDebug.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrpComDebug.Controls.Add(this.CobUartSelect);
			this.GrpComDebug.Controls.Add(this.ButOpenUart);
			this.GrpComDebug.Controls.Add(this.ButSendData);
			this.GrpComDebug.Controls.Add(this.GdbDataSend);
			this.GrpComDebug.Controls.Add(this.TxtUartState);
			this.GrpComDebug.Controls.Add(this.GdbDataReceive);
			this.GrpComDebug.Controls.Add(this.CobBaudRateSelect);
			this.GrpComDebug.Controls.Add(this.TxtBaudRateSelect);
			this.GrpComDebug.Controls.Add(this.TxtUartSelect);
			this.GrpComDebug.Location = new System.Drawing.Point(4, 12);
			this.GrpComDebug.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.GrpComDebug.Name = "GrpComDebug";
			this.GrpComDebug.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.GrpComDebug.Size = new System.Drawing.Size(535, 444);
			this.GrpComDebug.TabIndex = 29;
			this.GrpComDebug.TabStop = false;
			this.GrpComDebug.Text = "串口调试";
			this.AllowDrop = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(542, 458);
			base.Controls.Add(this.GrpComDebug);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.KeyPreview = true;
			base.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			base.Name = "ComDebugTool";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "合杰串口调试工具V1.4";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
			base.Load += new System.EventHandler(Form1_Load);
			this.GdbDataSend.ResumeLayout(false);
			this.GdbDataSend.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.numericUpDown).EndInit();
			this.GdbDataReceive.ResumeLayout(false);
			this.GdbDataReceive.PerformLayout();
			this.GrpComDebug.ResumeLayout(false);
			this.GrpComDebug.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
