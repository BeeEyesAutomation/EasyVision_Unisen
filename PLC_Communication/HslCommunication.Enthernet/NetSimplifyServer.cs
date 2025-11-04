using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetSimplifyServer : NetworkAuthenticationServerBase
{
	private int clientCount = 0;

	public int ClientCount => clientCount;

	public event Action<AppSession, NetHandle, string> ReceiveStringEvent;

	public event Action<AppSession, NetHandle, string[]> ReceiveStringArrayEvent;

	public event Action<AppSession, NetHandle, byte[]> ReceivedBytesEvent;

	public void SendMessage(AppSession session, int customer, string str)
	{
		Send(session.WorkSocket, HslProtocol.CommandBytes(customer, base.Token, str));
	}

	public void SendMessage(AppSession session, int customer, string[] str)
	{
		Send(session.WorkSocket, HslProtocol.CommandBytes(customer, base.Token, str));
	}

	public void SendMessage(AppSession session, int customer, byte[] bytes)
	{
		Send(session.WorkSocket, HslProtocol.CommandBytes(customer, base.Token, bytes));
	}

	protected override void CloseAction()
	{
		this.ReceivedBytesEvent = null;
		this.ReceiveStringEvent = null;
		base.CloseAction();
	}

	protected override void ThreadPoolLogin(Socket socket, IPEndPoint endPoint)
	{
		AppSession appSession = new AppSession(socket);
		base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOnlineInfo, appSession.IpEndPoint));
		try
		{
			appSession.WorkSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallback, appSession);
			Interlocked.Increment(ref clientCount);
		}
		catch (Exception ex)
		{
			appSession.WorkSocket?.Close();
			base.LogNet?.WriteException(ToString(), StringResources.Language.NetClientLoginFailed, ex);
		}
	}

	private async void ReceiveCallback(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (!(asyncState is AppSession appSession))
		{
			return;
		}
		if (!appSession.WorkSocket.EndReceiveResult(ar).IsSuccess)
		{
			AppSessionRemoteClose(appSession);
			return;
		}
		OperateResult<int, int, byte[]> read = await ReceiveHslMessageAsync(appSession.WorkSocket);
		if (!read.IsSuccess)
		{
			AppSessionRemoteClose(appSession);
			return;
		}
		int protocol = read.Content1;
		int customer = read.Content2;
		byte[] content = read.Content3;
		switch (protocol)
		{
		case 1:
			appSession.UpdateHeartTime();
			SendMessage(appSession, customer, content);
			base.LogNet?.WriteDebug(ToString(), $"Heart Check From {appSession.IpEndPoint}");
			break;
		case 1002:
			this.ReceivedBytesEvent?.Invoke(appSession, customer, content);
			break;
		case 1001:
			this.ReceiveStringEvent?.Invoke(appSession, customer, Encoding.Unicode.GetString(content));
			break;
		case 1005:
			this.ReceiveStringArrayEvent?.Invoke(appSession, customer, HslProtocol.UnPackStringArrayFromByte(content));
			break;
		case 2:
			AppSessionRemoteClose(appSession);
			return;
		default:
			AppSessionRemoteClose(appSession);
			return;
		}
		if (!appSession.WorkSocket.BeginReceiveResult(ReceiveCallback, appSession).IsSuccess)
		{
			AppSessionRemoteClose(appSession);
		}
	}

	public void AppSessionRemoteClose(AppSession session)
	{
		session.WorkSocket?.Close();
		Interlocked.Decrement(ref clientCount);
		base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOfflineInfo, session.IpEndPoint));
	}

	public override string ToString()
	{
		return $"NetSimplifyServer[{base.Port}]";
	}
}
