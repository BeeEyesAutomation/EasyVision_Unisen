using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetUdpServer : NetworkServerBase
{
	public int ReceiveCacheLength { get; set; } = 2048;

	public event Action<AppSession, NetHandle, string> AcceptString;

	public event Action<AppSession, NetHandle, byte[]> AcceptByte;

	public override void ServerStart(int port)
	{
		if (!base.IsStarted)
		{
			CoreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			CoreSocket.Bind(new IPEndPoint(IPAddress.Any, port));
			RefreshReceive();
			base.LogNet?.WriteInfo(ToString(), StringResources.Language.NetEngineStart);
			base.IsStarted = true;
		}
	}

	protected override void CloseAction()
	{
		this.AcceptString = null;
		this.AcceptByte = null;
		base.CloseAction();
	}

	private void RefreshReceive()
	{
		AppSession appSession = new AppSession(CoreSocket);
		appSession.UdpEndPoint = new IPEndPoint(IPAddress.Any, 0);
		appSession.BytesBuffer = new byte[ReceiveCacheLength];
		CoreSocket.BeginReceiveFrom(appSession.BytesBuffer, 0, ReceiveCacheLength, SocketFlags.None, ref appSession.UdpEndPoint, AsyncCallback, appSession);
	}

	private void AsyncCallback(IAsyncResult ar)
	{
		if (!(ar.AsyncState is AppSession appSession))
		{
			return;
		}
		try
		{
			int num = appSession.WorkSocket.EndReceiveFrom(ar, ref appSession.UdpEndPoint);
			RefreshReceive();
			if (num >= 32)
			{
				if (CheckRemoteToken(appSession.BytesBuffer))
				{
					int num2 = BitConverter.ToInt32(appSession.BytesBuffer, 28);
					if (num2 == num - 32)
					{
						byte[] array = new byte[32];
						byte[] array2 = new byte[num2];
						Array.Copy(appSession.BytesBuffer, 0, array, 0, 32);
						if (num2 > 0)
						{
							Array.Copy(appSession.BytesBuffer, 32, array2, 0, num2);
						}
						array2 = HslProtocol.CommandAnalysis(array, array2);
						int protocol = BitConverter.ToInt32(array, 0);
						int customer = BitConverter.ToInt32(array, 4);
						DataProcessingCenter(appSession, protocol, customer, array2);
					}
					else
					{
						base.LogNet?.WriteWarn(ToString(), $"Should Rece：{BitConverter.ToInt32(appSession.BytesBuffer, 4) + 8} Actual：{num}");
					}
				}
				else
				{
					base.LogNet?.WriteWarn(ToString(), StringResources.Language.TokenCheckFailed);
				}
			}
			else
			{
				base.LogNet?.WriteWarn(ToString(), $"Receive error, Actual：{num}");
			}
		}
		catch (ObjectDisposedException)
		{
		}
		catch (Exception ex2)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.SocketEndReceiveException, ex2);
			RefreshReceive();
		}
		finally
		{
		}
	}

	private void DataProcessingCenter(AppSession session, int protocol, int customer, byte[] content)
	{
		switch (protocol)
		{
		case 1002:
			this.AcceptByte?.Invoke(session, customer, content);
			break;
		case 1001:
		{
			string arg = Encoding.Unicode.GetString(content);
			this.AcceptString?.Invoke(session, customer, arg);
			break;
		}
		}
	}

	public void SendMessage(AppSession session, int customer, string str)
	{
		SendBytesAsync(session, HslProtocol.CommandBytes(customer, base.Token, str));
	}

	public void SendMessage(AppSession session, int customer, byte[] bytes)
	{
		SendBytesAsync(session, HslProtocol.CommandBytes(customer, base.Token, bytes));
	}

	private void SendBytesAsync(AppSession session, byte[] data)
	{
		try
		{
			session.WorkSocket.SendTo(data, data.Length, SocketFlags.None, session.UdpEndPoint);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException("SendMessage", ex);
		}
	}

	public override string ToString()
	{
		return $"NetUdpServer[{base.Port}]";
	}
}
