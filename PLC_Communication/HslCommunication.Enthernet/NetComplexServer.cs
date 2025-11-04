using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetComplexServer : NetworkServerBase
{
	private int connectMaxClient = 10000;

	private readonly List<AppSession> appSessions = null;

	private readonly object lockSessions = null;

	public int ConnectMax
	{
		get
		{
			return connectMaxClient;
		}
		set
		{
			connectMaxClient = value;
		}
	}

	public bool IsSaveLogClientLineChange { get; set; } = true;

	public int ClientCount => appSessions.Count;

	private Thread Thread_heart_check { get; set; } = null;

	public event Action<int> AllClientsStatusChange;

	public event Action<AppSession> ClientOnline;

	public event Action<AppSession, string> ClientOffline;

	public event Action<AppSession, NetHandle, string> AcceptString;

	public event Action<AppSession, NetHandle, byte[]> AcceptByte;

	public NetComplexServer()
	{
		appSessions = new List<AppSession>();
		lockSessions = new object();
	}

	protected override void StartInitialization()
	{
		Thread_heart_check = new Thread(ThreadHeartCheck)
		{
			IsBackground = true,
			Priority = ThreadPriority.AboveNormal
		};
		Thread_heart_check.Start();
		base.StartInitialization();
	}

	protected override void CloseAction()
	{
		Thread_heart_check?.Abort();
		this.ClientOffline = null;
		this.ClientOnline = null;
		this.AcceptString = null;
		this.AcceptByte = null;
		lock (lockSessions)
		{
			appSessions.ForEach(delegate(AppSession m)
			{
				m.WorkSocket?.Close();
			});
		}
		base.CloseAction();
	}

	private void TcpStateUpLine(AppSession session)
	{
		lock (lockSessions)
		{
			appSessions.Add(session);
		}
		this.ClientOnline?.Invoke(session);
		this.AllClientsStatusChange?.Invoke(ClientCount);
		if (IsSaveLogClientLineChange)
		{
			base.LogNet?.WriteDebug(ToString(), $"[{session.IpEndPoint}] Name:{session?.LoginAlias} {StringResources.Language.NetClientOnline}");
		}
	}

	private void TcpStateDownLine(AppSession session, bool regular, bool logSave = true)
	{
		lock (lockSessions)
		{
			if (!appSessions.Remove(session))
			{
				return;
			}
		}
		session.WorkSocket?.Close();
		string arg = (regular ? StringResources.Language.NetClientOffline : StringResources.Language.NetClientBreak);
		this.ClientOffline?.Invoke(session, arg);
		this.AllClientsStatusChange?.Invoke(ClientCount);
		if (IsSaveLogClientLineChange && logSave)
		{
			base.LogNet?.WriteInfo(ToString(), $"[{session.IpEndPoint}] Name:{session?.LoginAlias} {arg}");
		}
	}

	public void AppSessionRemoteClose(AppSession session)
	{
		TcpStateDownLine(session, regular: true);
	}

	protected override void ThreadPoolLogin(Socket socket, IPEndPoint endPoint)
	{
		if (appSessions.Count > ConnectMax)
		{
			socket?.Close();
			base.LogNet?.WriteWarn(ToString(), StringResources.Language.NetClientFull);
			return;
		}
		OperateResult<int, string> operateResult = ReceiveStringContentFromSocket(socket);
		if (!operateResult.IsSuccess)
		{
			return;
		}
		AppSession appSession = new AppSession(socket)
		{
			LoginAlias = operateResult.Content2
		};
		try
		{
			appSession.WorkSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallback, appSession);
			TcpStateUpLine(appSession);
			HslHelper.ThreadSleep(20);
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
		try
		{
			appSession.WorkSocket.EndReceive(ar);
		}
		catch
		{
			TcpStateDownLine(appSession, regular: false);
			return;
		}
		OperateResult<int, int, byte[]> read = await ReceiveHslMessageAsync(appSession.WorkSocket);
		if (!read.IsSuccess)
		{
			TcpStateDownLine(appSession, regular: false);
			return;
		}
		try
		{
			appSession.WorkSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallback, appSession);
		}
		catch
		{
			TcpStateDownLine(appSession, regular: false);
			return;
		}
		int protocol = read.Content1;
		int customer = read.Content2;
		byte[] content = read.Content3;
		switch (protocol)
		{
		case 1:
			BitConverter.GetBytes(DateTime.Now.Ticks).CopyTo(content, 8);
			base.LogNet?.WriteDebug(ToString(), $"Heart Check From {appSession.IpEndPoint}");
			if (Send(appSession.WorkSocket, HslProtocol.CommandBytes(1, customer, base.Token, content)).IsSuccess)
			{
				appSession.UpdateHeartTime();
			}
			break;
		case 2:
			TcpStateDownLine(appSession, regular: true);
			break;
		case 1002:
			this.AcceptByte?.Invoke(appSession, customer, content);
			break;
		case 1001:
		{
			string str = Encoding.Unicode.GetString(content);
			this.AcceptString?.Invoke(appSession, customer, str);
			break;
		}
		}
	}

	public void Send(AppSession session, NetHandle customer, string str)
	{
		Send(session.WorkSocket, HslProtocol.CommandBytes(customer, base.Token, str));
	}

	public void Send(AppSession session, NetHandle customer, byte[] bytes)
	{
		Send(session.WorkSocket, HslProtocol.CommandBytes(customer, base.Token, bytes));
	}

	public void SendAllClients(NetHandle customer, string str)
	{
		lock (lockSessions)
		{
			for (int i = 0; i < appSessions.Count; i++)
			{
				Send(appSessions[i], customer, str);
			}
		}
	}

	public void SendAllClients(NetHandle customer, byte[] data)
	{
		lock (lockSessions)
		{
			for (int i = 0; i < appSessions.Count; i++)
			{
				Send(appSessions[i], customer, data);
			}
		}
	}

	public void SendClientByAlias(string Alias, NetHandle customer, string str)
	{
		lock (lockSessions)
		{
			for (int i = 0; i < appSessions.Count; i++)
			{
				if (appSessions[i].LoginAlias == Alias)
				{
					Send(appSessions[i], customer, str);
				}
			}
		}
	}

	public void SendClientByAlias(string Alias, NetHandle customer, byte[] data)
	{
		lock (lockSessions)
		{
			for (int i = 0; i < appSessions.Count; i++)
			{
				if (appSessions[i].LoginAlias == Alias)
				{
					Send(appSessions[i], customer, data);
				}
			}
		}
	}

	private void ThreadHeartCheck()
	{
		do
		{
			HslHelper.ThreadSleep(2000);
			try
			{
				AppSession[] array = null;
				lock (lockSessions)
				{
					array = appSessions.ToArray();
				}
				for (int num = array.Length - 1; num >= 0; num--)
				{
					if (array[num] != null && (DateTime.Now - array[num].HeartTime).TotalSeconds > 30.0)
					{
						base.LogNet?.WriteWarn(ToString(), StringResources.Language.NetHeartCheckTimeout + array[num].IpAddress.ToString());
						TcpStateDownLine(array[num], regular: false, logSave: false);
					}
				}
			}
			catch (Exception ex)
			{
				base.LogNet?.WriteException(ToString(), StringResources.Language.NetHeartCheckFailed, ex);
			}
		}
		while (base.IsStarted);
	}

	public override string ToString()
	{
		return $"NetComplexServer[{base.Port}]";
	}
}
