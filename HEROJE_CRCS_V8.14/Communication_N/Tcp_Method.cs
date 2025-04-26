using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using 合杰图像算法调试工具;

namespace Communication_N
{
	public class Tcp_Method
	{
		public delegate void TcpMethodCallBack(int n, string description, int CommWay);

		private TcpMethodCallBack TcpMethodCB;

		private int comm_way = 2;

		private TcpListener listener;

		private TcpClient client;

		private IPAddress TargetIP;

		private int TargetPort;

		public bool IsOpenSuccess = false;

		private byte[] bytDataReceive;

		private int initway = -1;

		public static string[] GetIpAddress()
		{
			List<string> list = new List<string>();
			IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (iPAddress.AddressFamily.ToString() == "InterNetwork")
				{
					list.Add(iPAddress.ToString());
				}
			}
			string[] array = new string[list.Count];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = list[j];
			}
			return array;
		}

		public Tcp_Method(IPAddress LocalIpAdd, int LocalPort, TcpMethodCallBack call)
		{
			try
			{
				initway = 1;
				TcpMethodCB = call;
				listener = new TcpListener(LocalIpAdd, LocalPort);
				try
				{
					listener.Start();
					IsOpenSuccess = true;
				}
				catch (Exception ex)
				{
					IsOpenSuccess = false;
					if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
					{
						TcpMethodCB(1, "Tcp Server 打开失败.", comm_way);
					}
					else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
					{
						TcpMethodCB(1, "Tcp Server 打開失敗.", comm_way);
					}
					else
					{
						TcpMethodCB(1, "Failed to open the Tcp Server.", comm_way);
					}
					LogRecord.WriteError(ex);
					return;
				}
				IAsyncResult asyncResult = listener.BeginAcceptTcpClient(DoAcceptTcpClientCallback, listener);
				if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
				{
					TcpMethodCB(1, "Tcp Server 侦听中.", comm_way);
				}
				else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
				{
					TcpMethodCB(1, "Tcp Server 偵聽中.", comm_way);
				}
				else
				{
					TcpMethodCB(1, "The Tcp Server is listening. ", comm_way);
				}
			}
			catch (Exception ex2)
			{
				LogRecord.WriteError(ex2);
			}
		}

		public Tcp_Method(int RemotePort, IPAddress RemoteIp, TcpMethodCallBack call)
		{
			initway = 2;
			TcpMethodCB = call;
			TargetIP = RemoteIp;
			TargetPort = RemotePort;
			client = new TcpClient();
			try
			{
				IAsyncResult asyncResult = client.BeginConnect(RemoteIp, RemotePort, null, null);
				bool flag = asyncResult.AsyncWaitHandle.WaitOne(100, false);
				if (client.Connected)
				{
					IsOpenSuccess = true;
					if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
					{
						TcpMethodCB(1, "Tcp Client 已连接到服务器.", comm_way);
					}
					else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
					{
						TcpMethodCB(1, "Tcp Client 已連接到伺服器.", comm_way);
					}
					else
					{
						TcpMethodCB(1, "The Tcp Client is connected to the Tcp server.", comm_way);
					}
					bytDataReceive = new byte[103];
					client.GetStream().BeginRead(bytDataReceive, 0, bytDataReceive.Length, ReadFormStream, client);
				}
				else
				{
					IsOpenSuccess = false;
					if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
					{
						TcpMethodCB(1, "Tcp Client 连接失败.", comm_way);
					}
					else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
					{
						TcpMethodCB(1, "Tcp Client 連接失敗.", comm_way);
					}
					else
					{
						TcpMethodCB(1, "The Tcp Client connection failed. ", comm_way);
					}
				}
			}
			catch (Exception ex)
			{
				IsOpenSuccess = false;
				if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
				{
					TcpMethodCB(1, "Tcp Client 连接失败.", comm_way);
				}
				else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
				{
					TcpMethodCB(1, "Tcp Client 連接失敗.", comm_way);
				}
				else
				{
					TcpMethodCB(1, "The Tcp Client connection failed. ", comm_way);
				}
				LogRecord.WriteError(ex);
			}
		}

		private void ReadFormStream(IAsyncResult iar)
		{
			try
			{
				TcpClient tcpClient = (TcpClient)iar.AsyncState;
				if (tcpClient.GetStream().EndRead(iar) == 0)
				{
					if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
					{
						TcpMethodCB(1, "Tcp Client 连接已断开.", comm_way);
					}
					else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
					{
						TcpMethodCB(1, "Tcp Client 連接已斷開.", comm_way);
					}
					else
					{
						TcpMethodCB(1, "The Tcp Client is disconnected.", comm_way);
					}
					client.Close();
					client = new TcpClient();
					client.BeginConnect(TargetIP, TargetPort, ConnectCallback, client);
				}
				else
				{
					string description = Encoding.Default.GetString(bytDataReceive).Replace("\0", "");
					TcpMethodCB(2, description, comm_way);
					bytDataReceive = new byte[103];
					tcpClient.GetStream().BeginRead(bytDataReceive, 0, bytDataReceive.Length, ReadFormStream, tcpClient);
				}
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		private void DoAcceptTcpClientCallback(IAsyncResult iar)
		{
			TcpListener tcpListener = (TcpListener)iar.AsyncState;
			try
			{
				client = tcpListener.EndAcceptTcpClient(iar);
				if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
				{
					TcpMethodCB(1, "Tcp 客户端已连接.", comm_way);
				}
				else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
				{
					TcpMethodCB(1, "Tcp 用戶端已連接.", comm_way);
				}
				else
				{
					TcpMethodCB(1, "The Tcp client is connected. ", comm_way);
				}
				bytDataReceive = new byte[103];
				client.GetStream().BeginRead(bytDataReceive, 0, bytDataReceive.Length, ReadFormStream, client);
				IAsyncResult asyncResult = tcpListener.BeginAcceptTcpClient(DoAcceptTcpClientCallback, tcpListener);
			}
			catch (Exception ex)
			{
				LogRecord.WriteError(ex);
			}
		}

		public int SendData(byte[] byteDataSend)
		{
			if (client != null && client.Connected)
			{
				try
				{
					client.GetStream().BeginWrite(byteDataSend, 0, byteDataSend.Length, WriteToStream, client);
					return 0;
				}
				catch (Exception ex)
				{
					if (initway == 1)
					{
						if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
						{
							TcpMethodCB(1, "Tcp Client 连接已断开.", comm_way);
						}
						else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
						{
							TcpMethodCB(1, "Tcp Client 連接已斷開.", comm_way);
						}
						else
						{
							TcpMethodCB(1, "The Tcp Client is disconnected.", comm_way);
						}
						IAsyncResult asyncResult = listener.BeginAcceptTcpClient(DoAcceptTcpClientCallback, listener);
					}
					else if (initway == 2)
					{
						if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
						{
							TcpMethodCB(1, "Tcp Client 连接已断开.", comm_way);
						}
						else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
						{
							TcpMethodCB(1, "Tcp Client 連接已斷開.", comm_way);
						}
						else
						{
							TcpMethodCB(1, "The Tcp Client is disconnected.", comm_way);
						}
						client.Close();
						client = new TcpClient();
						client.BeginConnect(TargetIP, TargetPort, ConnectCallback, client);
					}
					LogRecord.WriteError(ex);
					return 2;
				}
			}
			return 1;
		}

		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				client = (TcpClient)ar.AsyncState;
				if (client.Connected)
				{
					if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
					{
						TcpMethodCB(1, "Tcp Client重连成功.", comm_way);
					}
					else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
					{
						TcpMethodCB(1, "Tcp Client重連成功.", comm_way);
					}
					else
					{
						TcpMethodCB(1, "The Tcp Client is successfully reconnected.", comm_way);
					}
					client.GetStream().BeginRead(bytDataReceive, 0, bytDataReceive.Length, ReadFormStream, client);
				}
				else
				{
					try
					{
						client.EndConnect(ar);
					}
					catch (Exception ex)
					{
						LogRecord.WriteError(ex);
					}
					client.BeginConnect(TargetIP, TargetPort, ConnectCallback, client);
				}
			}
			catch (Exception ex2)
			{
				LogRecord.WriteError(ex2);
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
				LogRecord.WriteError(ex);
			}
		}

		public void close()
		{
			if (client != null)
			{
				client.Close();
			}
			if (listener != null)
			{
				listener.Stop();
			}
			if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseS)
			{
				TcpMethodCB(1, "Tcp 通信已关闭.", comm_way);
			}
			else if (AppLanguage.ThisLanguage == AppLanguage.Language.ChineseT)
			{
				TcpMethodCB(1, "Tcp 通信已關閉.", comm_way);
			}
			else
			{
				TcpMethodCB(1, "The Tcp communication is disabled", comm_way);
			}
		}
	}
}
