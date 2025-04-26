using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Heroje_Debug_Tool.BaseClass;
using 合杰图像算法调试工具;

namespace 图像算法调试工具
{
	public class FormNet : Form
	{
		private string[] localIP = new string[2] { "192.168.1.91", "0.0.0.0" };

		public TcpListener listener;

		private IPAddress ipAdd;

		private int port;

		private IPAddress RemoteIpAdd;

		private int RemotePort;

		private TcpClient client;

		public StreamReader STR;

		public StreamWriter STW;

		private bool connectFlag = false;

		public string dataReceive;

		public string dataSend;

		private StringBuilder DataToString = new StringBuilder();

		private byte[] bytDataReceive;

		private byte[] bytDataSend;

		public Thread ListenThread;

		private int ReceiveCount = 0;

		private int SendCount = 0;

		private bool IsServer = false;

		private System.Timers.Timer AutoSend;

		private System.Timers.Timer timer;

		private long t1 = 0L;

		private long t2 = 0L;

		private bool Interval_Flag = false;

		private bool Rx_TxFlag = false;

		private bool DispTimeFlag = false;

		private bool time_flag = false;

		private bool UdpConnect = false;

		private UdpClient Uclient;

		private Thread th_UDPReceive;

		private IContainer components = null;

		private GroupBox GrbNetTool;

		private GroupBox GpbDataSend;

		private TextBox TxtDataSend;

		private Label LabSendCount;

		private CheckBox ChkDispTime;

		private Label LabCycleTimeUnit;

		private NumericUpDown numericUpDown1;

		private Label LabCycleTime;

		private CheckBox ChkTimedSend;

		private RadioButton RdbHexSend;

		private RadioButton RdbStringSend;

		private GroupBox GpbDataReceive;

		private TextBox TxtDataReceive;

		private Label LabReceiveCount;

		private ComboBox CobEncodeSelect;

		private Label LabDispEncode;

		private RadioButton RdbHexDisplay;

		private RadioButton RdbStringDisplay;

		private Button BtnConnect;

		private Label LabPort;

		private ComboBox CobIPAdress;

		private Label LabIPAdress;

		private ComboBox CobProtocolSelect;

		private Label LabProtocolSelect;

		private Label LabState;

		private TextBox TxtPort;

		private Button BtnSend;

		private BackgroundWorker BgwNetTool_1;

		private BackgroundWorker BgwNetTool_2;

		private TextBox TxtRemotePort;

		private Label LabRemotePort;

		private ComboBox CobRemoteIP;

		private Label LabRemoteIP;

		private Label LabReceiveByteCount;

		private Label LabSendByteCount;

		public FormNet()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Heroje_Standard)
			{
				Text = "HEROJE 网口调试工具v1.0";
			}
			else if (ToolCfg.ThisSoftware == ToolCfg.SoftwareDef.Hide_Heroje_Logo)
			{
				Text = "网口调试工具v1.0";
			}
			ControlAndXML controlAndXML = new ControlAndXML();
			controlAndXML.XMLToControlByLinq(ToolCfg.ConfigPath, this);
			IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (iPAddress.AddressFamily.ToString() == "InterNetwork")
				{
					localIP[1] = iPAddress.ToString();
					break;
				}
			}
			ComboBox.ObjectCollection items = CobIPAdress.Items;
			object[] items2 = localIP;
			items.AddRange(items2);
			if (CobIPAdress.Items.Count > 0)
			{
				CobIPAdress.SelectedIndex = 0;
			}
			TxtPort.Text = "1436";
			RdbStringDisplay.Checked = true;
			RdbStringSend.Checked = true;
			CobEncodeSelect.Text = "Default";
			ChkTimedSend.Checked = false;
			numericUpDown1.Value = 1000m;
			CobProtocolSelect.Text = "TCP Client";
			CobProtocolSelect.Enabled = true;
			CobIPAdress.Enabled = true;
			TxtPort.Enabled = true;
			BtnSend.Enabled = true;
			LabRemoteIP.Visible = false;
			CobRemoteIP.Visible = false;
			LabRemotePort.Visible = false;
			TxtRemotePort.Visible = false;
			AutoSend = new System.Timers.Timer(1000.0);
			AutoSend.Elapsed += TimedSendText;
			AutoSend.AutoReset = true;
			AutoSend.Enabled = true;
			AutoSend.Stop();
			timer = new System.Timers.Timer(40.0);
			timer.Elapsed += test;
			timer.AutoReset = true;
			timer.Enabled = true;
			timer.Stop();
		}

		private void CobProtocolSelect_TextChanged(object sender, EventArgs e)
		{
			LabRemoteIP.Visible = false;
			CobRemoteIP.Visible = false;
			LabRemotePort.Visible = false;
			TxtRemotePort.Visible = false;
			object[] items2;
			if (CobProtocolSelect.Text.Contains("TCP Client"))
			{
				CobIPAdress.DropDownStyle = ComboBoxStyle.DropDown;
				CobIPAdress.Items.Clear();
				ComboBox.ObjectCollection items = CobIPAdress.Items;
				items2 = localIP;
				items.AddRange(items2);
				if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
				{
					LabIPAdress.Text = "远程主机地址";
					LabPort.Text = "远程主机端口";
					BtnConnect.Text = "连接";
				}
				else
				{
					LabIPAdress.Text = "Remote IP:";
					LabPort.Text = "Remote Port:";
					BtnConnect.Text = "Connect";
				}
				return;
			}
			CobIPAdress.DropDownStyle = ComboBoxStyle.DropDown;
			CobIPAdress.Items.Clear();
			ComboBox.ObjectCollection items3 = CobIPAdress.Items;
			items2 = localIP;
			items3.AddRange(items2);
			if (ToolCfg.ConfigPath.Contains("ChineseS") || ToolCfg.ConfigPath.Contains("ChineseT"))
			{
				LabIPAdress.Text = "本地主机IP:";
				LabPort.Text = "本地主机端口:";
				BtnConnect.Text = "打开";
			}
			else
			{
				LabIPAdress.Text = "Local IP:";
				LabPort.Text = "Local Port:";
				BtnConnect.Text = "Open";
			}
			if (CobProtocolSelect.Text.Equals("UDP"))
			{
				LabRemoteIP.Visible = true;
				CobRemoteIP.Visible = true;
				LabRemotePort.Visible = true;
				TxtRemotePort.Visible = true;
			}
		}

		private void BtnConnect_Click(object sender, EventArgs e)
		{
			try
			{
				ipAdd = IPAddress.Parse(CobIPAdress.Text);
			}
			catch (Exception)
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.NetRightIpTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.NetRightIpTips), MessageBoxButtons.OK);
				return;
			}
			try
			{
				port = int.Parse(TxtPort.Text);
			}
			catch (Exception)
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.NetRightPortTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.NetRightPortTips), MessageBoxButtons.OK);
				return;
			}
			ChkTimedSend.Checked = false;
			switch (CobProtocolSelect.Text)
			{
			case "UDP":
				try
				{
					RemoteIpAdd = IPAddress.Parse(CobRemoteIP.Text);
				}
				catch (Exception)
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.NetRightRemoteIpTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.NetRightRemoteIpTips), MessageBoxButtons.OK);
					break;
				}
				try
				{
					RemotePort = int.Parse(TxtRemotePort.Text);
				}
				catch (Exception)
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.NetRightRemotePortTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.NetRightRemotePortTips), MessageBoxButtons.OK);
					break;
				}
				if (!UdpConnect)
				{
					UdpStart();
					UdpConnect = true;
					CobProtocolSelect.Enabled = false;
					CobIPAdress.Enabled = false;
					TxtPort.Enabled = false;
					BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetDisconnect);
				}
				else
				{
					Uclient.Close();
					UdpConnect = false;
					LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.UdpCloseTips);
					BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetOpen);
				}
				break;
			case "TCP Client":
				IsServer = false;
				if (!connectFlag)
				{
					ClientConnect();
					break;
				}
				ClientTerminate();
				BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetConnect);
				break;
			case "TCP Server":
				IsServer = true;
				if (!connectFlag)
				{
					try
					{
						if (listener.ToString() != null)
						{
							ServerTerminate();
							BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetOpen);
						}
						break;
					}
					catch (NullReferenceException)
					{
						ServerListen();
						BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetDisconnect);
						break;
					}
				}
				ServerTerminate();
				BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetOpen);
				break;
			}
		}

		private void ServerListen()
		{
			LabState.Invoke((MethodInvoker)delegate
			{
				LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.StateListening);
			});
			listener = new TcpListener(ipAdd, port);
			listener.Start();
			IAsyncResult asyncResult = listener.BeginAcceptTcpClient(DoAcceptTcpClientCallback, listener);
		}

		private void DoAcceptTcpClientCallback(IAsyncResult iar)
		{
			TcpListener tcpListener = (TcpListener)iar.AsyncState;
			try
			{
				client = tcpListener.EndAcceptTcpClient(iar);
				if (client.Connected)
				{
					connectFlag = true;
					LabState.Invoke((MethodInvoker)delegate
					{
						LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.StateClientConnected);
					});
				}
				bytDataReceive = new byte[103];
				client.GetStream().BeginRead(bytDataReceive, 0, bytDataReceive.Length, ReadFormStream, client);
			}
			catch (Exception)
			{
			}
		}

		private void ReadFormStream(IAsyncResult iar)
		{
			try
			{
				TcpClient tcpClient = (TcpClient)iar.AsyncState;
				int nCountBytesReceivedFromServer = tcpClient.GetStream().EndRead(iar);
				if (nCountBytesReceivedFromServer == 0)
				{
					Invoke((MethodInvoker)delegate
					{
						MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.DisconnectTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.DisconnectTips), MessageBoxButtons.OK);
						if (IsServer)
						{
							ServerTerminate();
							ServerListen();
						}
					});
					return;
				}
				if (Rx_TxFlag)
				{
					timer.Start();
					Interval_Flag = true;
				}
				ReceiveCount += nCountBytesReceivedFromServer;
				string strReceived;
				if (RdbStringDisplay.Checked)
				{
					string EncodeSelect = "Default";
					Invoke((MethodInvoker)delegate
					{
						EncodeSelect = CobEncodeSelect.Text;
					});
					switch (EncodeSelect)
					{
					case "Default":
						strReceived = Encoding.Default.GetString(bytDataReceive).Replace("\0", "");
						break;
					case "Unicode(big)":
						strReceived = Encoding.BigEndianUnicode.GetString(bytDataReceive).Replace("\0", "");
						break;
					case "Unicode(little)":
						strReceived = Encoding.Unicode.GetString(bytDataReceive).Replace("\0", "");
						break;
					default:
						strReceived = Encoding.GetEncoding(EncodeSelect).GetString(bytDataReceive).Replace("\0", "");
						break;
					}
					TxtDataReceive.Invoke((MethodInvoker)delegate
					{
						TxtDataReceive.AppendText(strReceived + " ");
					});
				}
				else
				{
					TxtDataReceive.Invoke((MethodInvoker)delegate
					{
						for (int i = 0; i < nCountBytesReceivedFromServer; i++)
						{
							TxtDataReceive.AppendText(bytDataReceive[i].ToString("X2") + " ");
						}
					});
				}
				LabReceiveByteCount.Invoke((MethodInvoker)delegate
				{
					LabReceiveByteCount.Text = ReceiveCount.ToString();
				});
				strReceived = "";
				bytDataReceive = new byte[103];
				tcpClient.GetStream().BeginRead(bytDataReceive, 0, bytDataReceive.Length, ReadFormStream, tcpClient);
			}
			catch (Exception)
			{
			}
		}

		private void WriteToStream(IAsyncResult iar)
		{
			try
			{
				TcpClient tcpClient = (TcpClient)iar.AsyncState;
				tcpClient.GetStream().EndWrite(iar);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception exc = ex2;
				Invoke((MethodInvoker)delegate
				{
					MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK);
				});
			}
		}

		private void ServerTerminate()
		{
			LabState.Invoke((MethodInvoker)delegate
			{
				LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.stateDisconnect);
			});
			try
			{
				client.Close();
			}
			catch (Exception)
			{
			}
			try
			{
				connectFlag = false;
				listener.Stop();
				listener = null;
			}
			catch (Exception)
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.NetServerNotOpenTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.NetServerNotOpenTips), MessageBoxButtons.OK);
			}
		}

		private void BtnSend_Click(object sender, EventArgs e)
		{
			ChkTimedSend.Checked = false;
			if (TxtDataSend.Text != "")
			{
				dataSend = TxtDataSend.Text;
				bytDataSend = Encoding.Default.GetBytes(TxtDataSend.Text);
				if (!CobProtocolSelect.Text.Equals("UDP"))
				{
					try
					{
						if (client.Connected)
						{
							if (DispTimeFlag)
							{
								Rx_TxFlag = true;
								t1 = DateTime.Now.Ticks / 10000;
							}
							if (RdbStringSend.Checked)
							{
								client.GetStream().BeginWrite(bytDataSend, 0, bytDataSend.Length, WriteToStream, client);
								SendCount += bytDataSend.Length;
							}
							else if (Regex.IsMatch(TxtDataSend.Text, "(?i)[\\da-f]{2}"))
							{
								MatchCollection matchCollection = Regex.Matches(TxtDataSend.Text, "(?i)[\\da-f]{2}");
								List<byte> list = new List<byte>();
								foreach (Match item in matchCollection)
								{
									list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
								}
								client.GetStream().BeginWrite(list.ToArray(), 0, list.Count, WriteToStream, client);
								SendCount += list.Count;
							}
							else
							{
								MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.EnterRightHexTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.EnterRightHexTips));
							}
							LabSendByteCount.Invoke((MethodInvoker)delegate
							{
								LabSendByteCount.Text = Convert.ToString(SendCount);
							});
						}
						else
						{
							connectFlag = false;
							if (IsServer)
							{
								LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.StateClientDisconnect);
							}
							else
							{
								LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.stateDisconnect);
								BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetConnect);
							}
						}
						return;
					}
					catch (Exception)
					{
						connectFlag = false;
						LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.StateDisconnectWithServer);
						BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetConnect);
						return;
					}
				}
				if (UdpConnect)
				{
					string text = null;
					byte[] array = null;
					IPEndPoint endPoint = new IPEndPoint(RemoteIpAdd, RemotePort);
					text = TxtDataSend.Text.Trim();
					array = Encoding.Default.GetBytes(text);
					UdpClient udpClient = new UdpClient();
					if (RdbStringSend.Checked)
					{
						udpClient.Send(array, array.Length, endPoint);
						SendCount += bytDataSend.Length;
					}
					else if (Regex.IsMatch(TxtDataSend.Text, "(?i)[\\da-f]{2}"))
					{
						MatchCollection matchCollection2 = Regex.Matches(TxtDataSend.Text, "(?i)[\\da-f]{2}");
						List<byte> list2 = new List<byte>();
						foreach (Match item2 in matchCollection2)
						{
							list2.Add(byte.Parse(item2.Value, NumberStyles.HexNumber));
						}
						udpClient.Send(list2.ToArray(), list2.Count, endPoint);
						SendCount += list2.Count;
					}
					else
					{
						MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.EnterRightHexTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.EnterRightHexTips));
					}
					LabSendByteCount.Invoke((MethodInvoker)delegate
					{
						LabSendByteCount.Text = Convert.ToString(SendCount);
					});
				}
				else
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.OpenUdpTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.OpenUdpTips), MessageBoxButtons.OK);
				}
			}
			else
			{
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.EnterTextTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.EnterTextTips), MessageBoxButtons.OK);
			}
		}

		private void ClientConnect()
		{
			client = new TcpClient();
			try
			{
				client.Connect(ipAdd, port);
				if (client.Connected)
				{
					connectFlag = true;
					LabState.Invoke((MethodInvoker)delegate
					{
						LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.StateConnectToServer);
					});
					bytDataReceive = new byte[103];
					client.GetStream().BeginRead(bytDataReceive, 0, bytDataReceive.Length, ReadFormStream, client);
					BtnConnect.Invoke((MethodInvoker)delegate
					{
						BtnConnect.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.NetDisconnect);
					});
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
			}
		}

		private void ClientTerminate()
		{
			LabState.Invoke((MethodInvoker)delegate
			{
				LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.DisconnectTips);
			});
			client.Close();
			connectFlag = false;
		}

		private void TxtDataReceive_TextChanged(object sender, EventArgs e)
		{
			TxtDataReceive.SelectionStart = TxtDataReceive.Text.Length;
			TxtDataReceive.ScrollToCaret();
		}

		private void TxtDataReceive_DoubleClick(object sender, EventArgs e)
		{
			if (MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.ClearReceiveDataTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.ClearReceiveDataTips), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				TxtDataReceive.Clear();
			}
		}

		private void LabReceiveCount_DoubleClick(object sender, EventArgs e)
		{
			if (MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.ClearReceiveCountTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.ClearReceiveCountTips), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				ReceiveCount = 0;
				LabReceiveByteCount.Text = "0";
			}
		}

		private void LabSendCount_DoubleClick(object sender, EventArgs e)
		{
			if (MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.ClearSendCountTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.ClearSendCountTips), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				SendCount = 0;
				LabSendCount.Text = "0";
			}
		}

		private void TxtDataSend_DoubleClick(object sender, EventArgs e)
		{
			if (MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.ClearSendDataTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.ClearSendDataTips), MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				TxtDataSend.Clear();
			}
		}

		private void TimedSendText(object source, ElapsedEventArgs e)
		{
			if (!ChkTimedSend.Checked)
			{
				return;
			}
			if (connectFlag | UdpConnect)
			{
				bytDataSend = Encoding.Default.GetBytes(TxtDataSend.Text);
				SendCount += bytDataSend.Length;
				if (DispTimeFlag)
				{
					Rx_TxFlag = true;
					t1 = DateTime.Now.Ticks / 10000;
				}
				if (RdbStringSend.Checked)
				{
					if (UdpConnect)
					{
						IPEndPoint endPoint = new IPEndPoint(RemoteIpAdd, RemotePort);
						UdpClient udpClient = new UdpClient();
						udpClient.Send(bytDataSend, bytDataSend.Length, endPoint);
					}
					else
					{
						client.GetStream().BeginWrite(bytDataSend, 0, bytDataSend.Length, WriteToStream, client);
					}
				}
				else if (Regex.IsMatch(TxtDataSend.Text, "(?i)[\\da-f]{2}"))
				{
					MatchCollection matchCollection = Regex.Matches(TxtDataSend.Text, "(?i)[\\da-f]{2}");
					List<byte> list = new List<byte>();
					foreach (Match item in matchCollection)
					{
						list.Add(byte.Parse(item.Value, NumberStyles.HexNumber));
					}
					if (UdpConnect)
					{
						IPEndPoint endPoint2 = new IPEndPoint(RemoteIpAdd, RemotePort);
						UdpClient udpClient2 = new UdpClient();
						udpClient2.Send(list.ToArray(), list.Count, endPoint2);
					}
					else
					{
						client.GetStream().BeginWrite(list.ToArray(), 0, list.Count, WriteToStream, client);
					}
				}
				else
				{
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.EnterRightHexTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.EnterRightHexTips));
				}
				LabSendByteCount.Invoke((MethodInvoker)delegate
				{
					LabSendByteCount.Text = Convert.ToString(SendCount);
				});
			}
			else
			{
				AutoSend.Stop();
				MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.ConnectThenResendTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.ConnectThenResendTips));
			}
		}

		private void ChkTimedSend_CheckedChanged(object sender, EventArgs e)
		{
			if (ChkTimedSend.Checked)
			{
				if (connectFlag | UdpConnect)
				{
					numericUpDown1.Enabled = false;
					AutoSend.Interval = (long)numericUpDown1.Value;
					AutoSend.Start();
				}
				else
				{
					ChkTimedSend.Checked = false;
					MessageBox.Show(MultLanguageText.Get_Content(MultLanguageText.TextDef.ConnectThenResendTips), MultLanguageText.Get_Title(MultLanguageText.TextDef.ConnectThenResendTips));
				}
			}
			else
			{
				numericUpDown1.Enabled = true;
				AutoSend.Stop();
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
				string str = " (" + Convert.ToString(t2 - t1) + "ms) \n";
				TxtDataReceive.Invoke((MethodInvoker)delegate
				{
					TxtDataReceive.AppendText(str);
				});
			}
		}

		private void ChkDispTime_CheckedChanged(object sender, EventArgs e)
		{
			if (ChkDispTime.Checked)
			{
				DispTimeFlag = true;
				return;
			}
			DispTimeFlag = false;
			timer.Stop();
		}

		private void UdpStart()
		{
			LabState.Invoke((MethodInvoker)delegate
			{
				LabState.Text = MultLanguageText.Get_Content(MultLanguageText.TextDef.StateUdpOpened);
			});
			th_UDPReceive = new Thread(UDPReceive);
			th_UDPReceive.IsBackground = true;
			th_UDPReceive.Start();
		}

		private void UDPReceive()
		{
			string text = null;
			byte[] array = null;
			IPEndPoint remoteEP = new IPEndPoint(ipAdd, port);
			Uclient = new UdpClient();
			Uclient.Client.Bind(remoteEP);
			while (UdpConnect)
			{
				try
				{
					if (Uclient.Available > 0)
					{
						array = Uclient.Receive(ref remoteEP);
						text = Encoding.Default.GetString(array);
						ReceiveCount += array.Length;
						DispStringOrHex(array);
					}
				}
				catch (Exception)
				{
					Uclient.Close();
					UdpConnect = false;
				}
			}
		}

		private void FormNet_FormClosing(object sender, FormClosingEventArgs e)
		{
			AutoSend.Stop();
			switch (CobProtocolSelect.Text)
			{
			case "UDP":
				if (UdpConnect)
				{
					Uclient.Close();
				}
				break;
			case "TCP Client":
				IsServer = false;
				if (connectFlag)
				{
					ClientTerminate();
				}
				break;
			case "TCP Server":
				IsServer = true;
				if (connectFlag)
				{
					ServerTerminate();
				}
				break;
			}
		}

		private void CobProtocolSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void DispStringOrHex(byte[] ByteData)
		{
			string StrTemp = "";
			if (RdbStringDisplay.Checked)
			{
				string EncodeSelect = "Default";
				Invoke((MethodInvoker)delegate
				{
					EncodeSelect = CobEncodeSelect.Text;
				});
				switch (EncodeSelect)
				{
				case "Default":
					StrTemp = Encoding.Default.GetString(ByteData).Replace("\0", "");
					break;
				case "Unicode(big)":
					StrTemp = Encoding.BigEndianUnicode.GetString(ByteData).Replace("\0", "");
					break;
				case "Unicode(little)":
					StrTemp = Encoding.Unicode.GetString(ByteData).Replace("\0", "");
					break;
				case "GBK":
					StrTemp = Encoding.GetEncoding("gb2312").GetString(ByteData).Replace("\0", "");
					break;
				case "UTF-8":
					StrTemp = Encoding.GetEncoding("utf-8").GetString(ByteData).Replace("\0", "");
					break;
				case "Big5":
					StrTemp = Encoding.GetEncoding("big5").GetString(ByteData).Replace("\0", "");
					break;
				default:
					StrTemp = Encoding.GetEncoding(EncodeSelect).GetString(ByteData).Replace("\0", "");
					break;
				}
				TxtDataReceive.Invoke((MethodInvoker)delegate
				{
					TxtDataReceive.AppendText(StrTemp + " ");
				});
			}
			else
			{
				TxtDataReceive.Invoke((MethodInvoker)delegate
				{
					for (int i = 0; i < ByteData.Length; i++)
					{
						TxtDataReceive.AppendText(ByteData[i].ToString("X2") + " ");
					}
				});
			}
			LabReceiveByteCount.Invoke((MethodInvoker)delegate
			{
				LabReceiveByteCount.Text = ReceiveCount.ToString();
			});
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
			this.GrbNetTool = new System.Windows.Forms.GroupBox();
			this.TxtRemotePort = new System.Windows.Forms.TextBox();
			this.LabRemotePort = new System.Windows.Forms.Label();
			this.CobRemoteIP = new System.Windows.Forms.ComboBox();
			this.LabRemoteIP = new System.Windows.Forms.Label();
			this.BtnSend = new System.Windows.Forms.Button();
			this.TxtPort = new System.Windows.Forms.TextBox();
			this.LabState = new System.Windows.Forms.Label();
			this.GpbDataSend = new System.Windows.Forms.GroupBox();
			this.LabSendByteCount = new System.Windows.Forms.Label();
			this.TxtDataSend = new System.Windows.Forms.TextBox();
			this.LabSendCount = new System.Windows.Forms.Label();
			this.ChkDispTime = new System.Windows.Forms.CheckBox();
			this.LabCycleTimeUnit = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.LabCycleTime = new System.Windows.Forms.Label();
			this.ChkTimedSend = new System.Windows.Forms.CheckBox();
			this.RdbHexSend = new System.Windows.Forms.RadioButton();
			this.RdbStringSend = new System.Windows.Forms.RadioButton();
			this.GpbDataReceive = new System.Windows.Forms.GroupBox();
			this.LabReceiveByteCount = new System.Windows.Forms.Label();
			this.TxtDataReceive = new System.Windows.Forms.TextBox();
			this.LabReceiveCount = new System.Windows.Forms.Label();
			this.CobEncodeSelect = new System.Windows.Forms.ComboBox();
			this.LabDispEncode = new System.Windows.Forms.Label();
			this.RdbHexDisplay = new System.Windows.Forms.RadioButton();
			this.RdbStringDisplay = new System.Windows.Forms.RadioButton();
			this.BtnConnect = new System.Windows.Forms.Button();
			this.LabPort = new System.Windows.Forms.Label();
			this.CobIPAdress = new System.Windows.Forms.ComboBox();
			this.LabIPAdress = new System.Windows.Forms.Label();
			this.CobProtocolSelect = new System.Windows.Forms.ComboBox();
			this.LabProtocolSelect = new System.Windows.Forms.Label();
			this.BgwNetTool_1 = new System.ComponentModel.BackgroundWorker();
			this.BgwNetTool_2 = new System.ComponentModel.BackgroundWorker();
			this.GrbNetTool.SuspendLayout();
			this.GpbDataSend.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.numericUpDown1).BeginInit();
			this.GpbDataReceive.SuspendLayout();
			base.SuspendLayout();
			this.GrbNetTool.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GrbNetTool.Controls.Add(this.TxtRemotePort);
			this.GrbNetTool.Controls.Add(this.LabRemotePort);
			this.GrbNetTool.Controls.Add(this.CobRemoteIP);
			this.GrbNetTool.Controls.Add(this.LabRemoteIP);
			this.GrbNetTool.Controls.Add(this.BtnSend);
			this.GrbNetTool.Controls.Add(this.TxtPort);
			this.GrbNetTool.Controls.Add(this.LabState);
			this.GrbNetTool.Controls.Add(this.GpbDataSend);
			this.GrbNetTool.Controls.Add(this.GpbDataReceive);
			this.GrbNetTool.Controls.Add(this.BtnConnect);
			this.GrbNetTool.Controls.Add(this.LabPort);
			this.GrbNetTool.Controls.Add(this.CobIPAdress);
			this.GrbNetTool.Controls.Add(this.LabIPAdress);
			this.GrbNetTool.Controls.Add(this.CobProtocolSelect);
			this.GrbNetTool.Controls.Add(this.LabProtocolSelect);
			this.GrbNetTool.Location = new System.Drawing.Point(8, 7);
			this.GrbNetTool.Margin = new System.Windows.Forms.Padding(2);
			this.GrbNetTool.Name = "GrbNetTool";
			this.GrbNetTool.Padding = new System.Windows.Forms.Padding(2);
			this.GrbNetTool.Size = new System.Drawing.Size(637, 425);
			this.GrbNetTool.TabIndex = 0;
			this.GrbNetTool.TabStop = false;
			this.GrbNetTool.Text = "网口调试";
			this.TxtRemotePort.Location = new System.Drawing.Point(460, 42);
			this.TxtRemotePort.Margin = new System.Windows.Forms.Padding(2);
			this.TxtRemotePort.Name = "TxtRemotePort";
			this.TxtRemotePort.Size = new System.Drawing.Size(61, 21);
			this.TxtRemotePort.TabIndex = 15;
			this.LabRemotePort.AutoSize = true;
			this.LabRemotePort.Location = new System.Drawing.Point(382, 47);
			this.LabRemotePort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabRemotePort.Name = "LabRemotePort";
			this.LabRemotePort.Size = new System.Drawing.Size(77, 12);
			this.LabRemotePort.TabIndex = 14;
			this.LabRemotePort.Text = "远程主机端口";
			this.CobRemoteIP.FormattingEnabled = true;
			this.CobRemoteIP.Location = new System.Drawing.Point(239, 43);
			this.CobRemoteIP.Margin = new System.Windows.Forms.Padding(2);
			this.CobRemoteIP.Name = "CobRemoteIP";
			this.CobRemoteIP.Size = new System.Drawing.Size(139, 20);
			this.CobRemoteIP.TabIndex = 13;
			this.LabRemoteIP.AutoSize = true;
			this.LabRemoteIP.Location = new System.Drawing.Point(158, 46);
			this.LabRemoteIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabRemoteIP.Name = "LabRemoteIP";
			this.LabRemoteIP.Size = new System.Drawing.Size(77, 12);
			this.LabRemoteIP.TabIndex = 12;
			this.LabRemoteIP.Text = "远程主机地址";
			this.BtnSend.Location = new System.Drawing.Point(539, 43);
			this.BtnSend.Margin = new System.Windows.Forms.Padding(2);
			this.BtnSend.Name = "BtnSend";
			this.BtnSend.Size = new System.Drawing.Size(90, 23);
			this.BtnSend.TabIndex = 11;
			this.BtnSend.Text = "发送";
			this.BtnSend.UseVisualStyleBackColor = true;
			this.BtnSend.Click += new System.EventHandler(BtnSend_Click);
			this.TxtPort.Location = new System.Drawing.Point(460, 18);
			this.TxtPort.Margin = new System.Windows.Forms.Padding(2);
			this.TxtPort.Name = "TxtPort";
			this.TxtPort.Size = new System.Drawing.Size(61, 21);
			this.TxtPort.TabIndex = 10;
			this.LabState.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			this.LabState.AutoSize = true;
			this.LabState.Location = new System.Drawing.Point(4, 408);
			this.LabState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabState.Name = "LabState";
			this.LabState.Size = new System.Drawing.Size(41, 12);
			this.LabState.TabIndex = 9;
			this.LabState.Text = "状态：";
			this.GpbDataSend.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.GpbDataSend.Controls.Add(this.LabSendByteCount);
			this.GpbDataSend.Controls.Add(this.TxtDataSend);
			this.GpbDataSend.Controls.Add(this.LabSendCount);
			this.GpbDataSend.Controls.Add(this.ChkDispTime);
			this.GpbDataSend.Controls.Add(this.LabCycleTimeUnit);
			this.GpbDataSend.Controls.Add(this.numericUpDown1);
			this.GpbDataSend.Controls.Add(this.LabCycleTime);
			this.GpbDataSend.Controls.Add(this.ChkTimedSend);
			this.GpbDataSend.Controls.Add(this.RdbHexSend);
			this.GpbDataSend.Controls.Add(this.RdbStringSend);
			this.GpbDataSend.Location = new System.Drawing.Point(4, 269);
			this.GpbDataSend.Margin = new System.Windows.Forms.Padding(2);
			this.GpbDataSend.Name = "GpbDataSend";
			this.GpbDataSend.Padding = new System.Windows.Forms.Padding(2);
			this.GpbDataSend.Size = new System.Drawing.Size(629, 137);
			this.GpbDataSend.TabIndex = 8;
			this.GpbDataSend.TabStop = false;
			this.GpbDataSend.Text = "数据发送区";
			this.LabSendByteCount.AutoSize = true;
			this.LabSendByteCount.Location = new System.Drawing.Point(487, 22);
			this.LabSendByteCount.Name = "LabSendByteCount";
			this.LabSendByteCount.Size = new System.Drawing.Size(11, 12);
			this.LabSendByteCount.TabIndex = 9;
			this.LabSendByteCount.Text = "0";
			this.TxtDataSend.Location = new System.Drawing.Point(2, 38);
			this.TxtDataSend.Margin = new System.Windows.Forms.Padding(2);
			this.TxtDataSend.Multiline = true;
			this.TxtDataSend.Name = "TxtDataSend";
			this.TxtDataSend.Size = new System.Drawing.Size(623, 95);
			this.TxtDataSend.TabIndex = 8;
			this.TxtDataSend.DoubleClick += new System.EventHandler(TxtDataSend_DoubleClick);
			this.LabSendCount.AutoSize = true;
			this.LabSendCount.Location = new System.Drawing.Point(417, 22);
			this.LabSendCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabSendCount.Name = "LabSendCount";
			this.LabSendCount.Size = new System.Drawing.Size(65, 12);
			this.LabSendCount.TabIndex = 7;
			this.LabSendCount.Text = "发送字节：";
			this.LabSendCount.DoubleClick += new System.EventHandler(LabSendCount_DoubleClick);
			this.ChkDispTime.AutoSize = true;
			this.ChkDispTime.Location = new System.Drawing.Point(341, 21);
			this.ChkDispTime.Margin = new System.Windows.Forms.Padding(2);
			this.ChkDispTime.Name = "ChkDispTime";
			this.ChkDispTime.Size = new System.Drawing.Size(72, 16);
			this.ChkDispTime.TabIndex = 6;
			this.ChkDispTime.Text = "显示时间";
			this.ChkDispTime.UseVisualStyleBackColor = true;
			this.ChkDispTime.CheckedChanged += new System.EventHandler(ChkDispTime_CheckedChanged);
			this.LabCycleTimeUnit.AutoSize = true;
			this.LabCycleTimeUnit.Location = new System.Drawing.Point(320, 21);
			this.LabCycleTimeUnit.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabCycleTimeUnit.Name = "LabCycleTimeUnit";
			this.LabCycleTimeUnit.Size = new System.Drawing.Size(17, 12);
			this.LabCycleTimeUnit.TabIndex = 5;
			this.LabCycleTimeUnit.Text = "ms";
			this.numericUpDown1.Location = new System.Drawing.Point(237, 16);
			this.numericUpDown1.Margin = new System.Windows.Forms.Padding(2);
			this.numericUpDown1.Maximum = new decimal(new int[4] { 100000, 0, 0, 0 });
			this.numericUpDown1.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(82, 21);
			this.numericUpDown1.TabIndex = 4;
			this.numericUpDown1.Value = new decimal(new int[4] { 1, 0, 0, 0 });
			this.LabCycleTime.AutoSize = true;
			this.LabCycleTime.Location = new System.Drawing.Point(203, 20);
			this.LabCycleTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabCycleTime.Name = "LabCycleTime";
			this.LabCycleTime.Size = new System.Drawing.Size(41, 12);
			this.LabCycleTime.TabIndex = 3;
			this.LabCycleTime.Text = "周期：";
			this.ChkTimedSend.AutoSize = true;
			this.ChkTimedSend.Location = new System.Drawing.Point(137, 19);
			this.ChkTimedSend.Margin = new System.Windows.Forms.Padding(2);
			this.ChkTimedSend.Name = "ChkTimedSend";
			this.ChkTimedSend.Size = new System.Drawing.Size(72, 16);
			this.ChkTimedSend.TabIndex = 2;
			this.ChkTimedSend.Text = "定时发送";
			this.ChkTimedSend.UseVisualStyleBackColor = true;
			this.ChkTimedSend.CheckedChanged += new System.EventHandler(ChkTimedSend_CheckedChanged);
			this.RdbHexSend.AutoSize = true;
			this.RdbHexSend.Location = new System.Drawing.Point(62, 18);
			this.RdbHexSend.Margin = new System.Windows.Forms.Padding(2);
			this.RdbHexSend.Name = "RdbHexSend";
			this.RdbHexSend.Size = new System.Drawing.Size(71, 16);
			this.RdbHexSend.TabIndex = 1;
			this.RdbHexSend.TabStop = true;
			this.RdbHexSend.Text = "十六进制";
			this.RdbHexSend.UseVisualStyleBackColor = true;
			this.RdbStringSend.AutoSize = true;
			this.RdbStringSend.Location = new System.Drawing.Point(5, 18);
			this.RdbStringSend.Margin = new System.Windows.Forms.Padding(2);
			this.RdbStringSend.Name = "RdbStringSend";
			this.RdbStringSend.Size = new System.Drawing.Size(59, 16);
			this.RdbStringSend.TabIndex = 0;
			this.RdbStringSend.TabStop = true;
			this.RdbStringSend.Text = "字符串";
			this.RdbStringSend.UseVisualStyleBackColor = true;
			this.GpbDataReceive.Controls.Add(this.LabReceiveByteCount);
			this.GpbDataReceive.Controls.Add(this.TxtDataReceive);
			this.GpbDataReceive.Controls.Add(this.LabReceiveCount);
			this.GpbDataReceive.Controls.Add(this.CobEncodeSelect);
			this.GpbDataReceive.Controls.Add(this.LabDispEncode);
			this.GpbDataReceive.Controls.Add(this.RdbHexDisplay);
			this.GpbDataReceive.Controls.Add(this.RdbStringDisplay);
			this.GpbDataReceive.Location = new System.Drawing.Point(5, 61);
			this.GpbDataReceive.Margin = new System.Windows.Forms.Padding(2);
			this.GpbDataReceive.Name = "GpbDataReceive";
			this.GpbDataReceive.Padding = new System.Windows.Forms.Padding(2);
			this.GpbDataReceive.Size = new System.Drawing.Size(628, 204);
			this.GpbDataReceive.TabIndex = 7;
			this.GpbDataReceive.TabStop = false;
			this.GpbDataReceive.Text = "数据显示区";
			this.LabReceiveByteCount.AutoSize = true;
			this.LabReceiveByteCount.Location = new System.Drawing.Point(453, 18);
			this.LabReceiveByteCount.Name = "LabReceiveByteCount";
			this.LabReceiveByteCount.Size = new System.Drawing.Size(11, 12);
			this.LabReceiveByteCount.TabIndex = 6;
			this.LabReceiveByteCount.Text = "0";
			this.TxtDataReceive.Location = new System.Drawing.Point(2, 36);
			this.TxtDataReceive.Margin = new System.Windows.Forms.Padding(2);
			this.TxtDataReceive.Multiline = true;
			this.TxtDataReceive.Name = "TxtDataReceive";
			this.TxtDataReceive.Size = new System.Drawing.Size(622, 164);
			this.TxtDataReceive.TabIndex = 5;
			this.TxtDataReceive.TextChanged += new System.EventHandler(TxtDataReceive_TextChanged);
			this.TxtDataReceive.DoubleClick += new System.EventHandler(TxtDataReceive_DoubleClick);
			this.LabReceiveCount.AutoSize = true;
			this.LabReceiveCount.Location = new System.Drawing.Point(377, 18);
			this.LabReceiveCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabReceiveCount.Name = "LabReceiveCount";
			this.LabReceiveCount.Size = new System.Drawing.Size(65, 12);
			this.LabReceiveCount.TabIndex = 4;
			this.LabReceiveCount.Text = "接收字节：";
			this.LabReceiveCount.DoubleClick += new System.EventHandler(LabReceiveCount_DoubleClick);
			this.CobEncodeSelect.FormattingEnabled = true;
			this.CobEncodeSelect.Items.AddRange(new object[6] { "Default", "GBK", "UTF-8", "Unicode(big)", "Unicode(little)", "Big5" });
			this.CobEncodeSelect.Location = new System.Drawing.Point(249, 15);
			this.CobEncodeSelect.Margin = new System.Windows.Forms.Padding(2);
			this.CobEncodeSelect.Name = "CobEncodeSelect";
			this.CobEncodeSelect.Size = new System.Drawing.Size(102, 20);
			this.CobEncodeSelect.TabIndex = 3;
			this.LabDispEncode.AutoSize = true;
			this.LabDispEncode.Location = new System.Drawing.Point(191, 19);
			this.LabDispEncode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabDispEncode.Name = "LabDispEncode";
			this.LabDispEncode.Size = new System.Drawing.Size(53, 12);
			this.LabDispEncode.TabIndex = 2;
			this.LabDispEncode.Text = "显示编码";
			this.RdbHexDisplay.AutoSize = true;
			this.RdbHexDisplay.Location = new System.Drawing.Point(79, 17);
			this.RdbHexDisplay.Margin = new System.Windows.Forms.Padding(2);
			this.RdbHexDisplay.Name = "RdbHexDisplay";
			this.RdbHexDisplay.Size = new System.Drawing.Size(71, 16);
			this.RdbHexDisplay.TabIndex = 1;
			this.RdbHexDisplay.TabStop = true;
			this.RdbHexDisplay.Text = "十六进制";
			this.RdbHexDisplay.UseVisualStyleBackColor = true;
			this.RdbStringDisplay.AutoSize = true;
			this.RdbStringDisplay.Location = new System.Drawing.Point(4, 17);
			this.RdbStringDisplay.Margin = new System.Windows.Forms.Padding(2);
			this.RdbStringDisplay.Name = "RdbStringDisplay";
			this.RdbStringDisplay.Size = new System.Drawing.Size(59, 16);
			this.RdbStringDisplay.TabIndex = 0;
			this.RdbStringDisplay.TabStop = true;
			this.RdbStringDisplay.Text = "字符串";
			this.RdbStringDisplay.UseVisualStyleBackColor = true;
			this.BtnConnect.Location = new System.Drawing.Point(539, 16);
			this.BtnConnect.Margin = new System.Windows.Forms.Padding(2);
			this.BtnConnect.Name = "BtnConnect";
			this.BtnConnect.Size = new System.Drawing.Size(90, 23);
			this.BtnConnect.TabIndex = 6;
			this.BtnConnect.Text = "监听";
			this.BtnConnect.UseVisualStyleBackColor = true;
			this.BtnConnect.Click += new System.EventHandler(BtnConnect_Click);
			this.LabPort.AutoSize = true;
			this.LabPort.Location = new System.Drawing.Point(382, 22);
			this.LabPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabPort.Name = "LabPort";
			this.LabPort.Size = new System.Drawing.Size(77, 12);
			this.LabPort.TabIndex = 4;
			this.LabPort.Text = "本地主机端口";
			this.CobIPAdress.FormattingEnabled = true;
			this.CobIPAdress.Location = new System.Drawing.Point(239, 19);
			this.CobIPAdress.Margin = new System.Windows.Forms.Padding(2);
			this.CobIPAdress.Name = "CobIPAdress";
			this.CobIPAdress.Size = new System.Drawing.Size(139, 20);
			this.CobIPAdress.TabIndex = 3;
			this.LabIPAdress.AutoSize = true;
			this.LabIPAdress.Location = new System.Drawing.Point(158, 23);
			this.LabIPAdress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabIPAdress.Name = "LabIPAdress";
			this.LabIPAdress.Size = new System.Drawing.Size(77, 12);
			this.LabIPAdress.TabIndex = 2;
			this.LabIPAdress.Text = "本地主机地址";
			this.CobProtocolSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CobProtocolSelect.FormattingEnabled = true;
			this.CobProtocolSelect.Items.AddRange(new object[3] { "UDP", "TCP Client", "TCP Server" });
			this.CobProtocolSelect.Location = new System.Drawing.Point(60, 19);
			this.CobProtocolSelect.Margin = new System.Windows.Forms.Padding(2);
			this.CobProtocolSelect.Name = "CobProtocolSelect";
			this.CobProtocolSelect.Size = new System.Drawing.Size(94, 20);
			this.CobProtocolSelect.TabIndex = 1;
			this.CobProtocolSelect.SelectedIndexChanged += new System.EventHandler(CobProtocolSelect_SelectedIndexChanged);
			this.CobProtocolSelect.TextChanged += new System.EventHandler(CobProtocolSelect_TextChanged);
			this.LabProtocolSelect.AutoSize = true;
			this.LabProtocolSelect.Location = new System.Drawing.Point(4, 22);
			this.LabProtocolSelect.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabProtocolSelect.Name = "LabProtocolSelect";
			this.LabProtocolSelect.Size = new System.Drawing.Size(53, 12);
			this.LabProtocolSelect.TabIndex = 0;
			this.LabProtocolSelect.Text = "协议类型";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			base.ClientSize = new System.Drawing.Size(648, 436);
			base.Controls.Add(this.GrbNetTool);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Margin = new System.Windows.Forms.Padding(2);
			base.MaximizeBox = false;
			base.Name = "FormNet";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "合杰网口调试工具v1.0";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FormNet_FormClosing);
			base.Load += new System.EventHandler(Form1_Load);
			this.GrbNetTool.ResumeLayout(false);
			this.GrbNetTool.PerformLayout();
			this.GpbDataSend.ResumeLayout(false);
			this.GpbDataSend.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.numericUpDown1).EndInit();
			this.GpbDataReceive.ResumeLayout(false);
			this.GpbDataReceive.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
