using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetComplexClient : NetworkXBase
{
	private AppSession session;

	private int isConnecting = 0;

	private bool closed = false;

	private Thread thread_heart_check = null;

	public bool IsClientStart { get; set; }

	public int ConnectFailedCount { get; private set; }

	public string ClientAlias { get; set; } = string.Empty;

	public IPEndPoint EndPointServer { get; set; }

	public DateTime ServerTime { get; private set; }

	public int DelayTime { get; private set; }

	public event Action LoginSuccess;

	public event Action<int> LoginFailed;

	public event Action<string> MessageAlerts;

	public event Action BeforReConnected;

	public event Action<AppSession, NetHandle, string> AcceptString;

	public event Action<AppSession, NetHandle, byte[]> AcceptByte;

	public NetComplexClient()
	{
		session = new AppSession();
		ServerTime = DateTime.Now;
		EndPointServer = new IPEndPoint(IPAddress.Any, 0);
	}

	public void ClientClose()
	{
		closed = true;
		if (IsClientStart)
		{
			Send(session.WorkSocket, HslProtocol.CommandBytes(2, 0, base.Token, null));
		}
		IsClientStart = false;
		thread_heart_check = null;
		this.LoginSuccess = null;
		this.LoginFailed = null;
		this.MessageAlerts = null;
		this.AcceptByte = null;
		this.AcceptString = null;
		HslHelper.ThreadSleep(20);
		session.WorkSocket?.Close();
		base.LogNet?.WriteDebug(ToString(), "Client Close.");
	}

	public void ClientStart()
	{
		if (Interlocked.CompareExchange(ref isConnecting, 1, 0) == 0)
		{
			Thread thread = new Thread(ThreadLogin);
			thread.IsBackground = true;
			thread.Start();
			if (thread_heart_check == null)
			{
				thread_heart_check = new Thread(ThreadHeartCheck)
				{
					Priority = ThreadPriority.AboveNormal,
					IsBackground = true
				};
				thread_heart_check.Start();
			}
		}
	}

	private void AwaitToConnect()
	{
		if (ConnectFailedCount == 0)
		{
			this.MessageAlerts?.Invoke(StringResources.Language.ConnectingServer);
			return;
		}
		int num = 10;
		while (num > 0)
		{
			if (closed)
			{
				return;
			}
			num--;
			this.MessageAlerts?.Invoke(string.Format(StringResources.Language.ConnectFailedAndWait, num));
			HslHelper.ThreadSleep(1000);
		}
		this.MessageAlerts?.Invoke(string.Format(StringResources.Language.AttemptConnectServer, ConnectFailedCount));
	}

	private void ConnectFailed()
	{
		ConnectFailedCount++;
		Interlocked.Exchange(ref isConnecting, 0);
		this.LoginFailed?.Invoke(ConnectFailedCount);
		base.LogNet?.WriteDebug(ToString(), "Connected Failed, Times: " + ConnectFailedCount);
	}

	private OperateResult<Socket> ConnectServer()
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(EndPointServer, 10000);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 1, ClientAlias);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<Socket>(operateResult2);
		}
		this.MessageAlerts?.Invoke(StringResources.Language.ConnectServerSuccess);
		return operateResult;
	}

	private void LoginSuccessMethod(Socket socket)
	{
		ConnectFailedCount = 0;
		try
		{
			session.UpdateSocket(socket);
			session.LoginAlias = ClientAlias;
			session.UpdateHeartTime();
			IsClientStart = true;
			session.WorkSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallback, session);
		}
		catch
		{
			ThreadPool.QueueUserWorkItem(ReconnectServer, null);
		}
	}

	private void ThreadLogin()
	{
		AwaitToConnect();
		OperateResult<Socket> operateResult = ConnectServer();
		if (!operateResult.IsSuccess)
		{
			ConnectFailed();
			ThreadPool.QueueUserWorkItem(ReconnectServer, null);
			return;
		}
		LoginSuccessMethod(operateResult.Content);
		this.LoginSuccess?.Invoke();
		Interlocked.Exchange(ref isConnecting, 0);
		HslHelper.ThreadSleep(200);
	}

	private void ReconnectServer(object obj = null)
	{
		if (isConnecting != 1 && !closed)
		{
			this.BeforReConnected?.Invoke();
			session?.WorkSocket?.Close();
			ClientStart();
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
			ThreadPool.QueueUserWorkItem(ReconnectServer, null);
			return;
		}
		OperateResult<int, int, byte[]> read = await ReceiveHslMessageAsync(appSession.WorkSocket);
		if (!read.IsSuccess)
		{
			ThreadPool.QueueUserWorkItem(ReconnectServer, null);
			return;
		}
		try
		{
			appSession.WorkSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallback, appSession);
		}
		catch
		{
			ThreadPool.QueueUserWorkItem(ReconnectServer, null);
			return;
		}
		int protocol = read.Content1;
		int customer = read.Content2;
		byte[] content = read.Content3;
		switch (protocol)
		{
		case 1:
		{
			DateTime dt = new DateTime(BitConverter.ToInt64(content, 0));
			ServerTime = new DateTime(BitConverter.ToInt64(content, 8));
			DelayTime = (int)(DateTime.Now - dt).TotalMilliseconds;
			session.UpdateHeartTime();
			break;
		}
		case 1002:
			this.AcceptByte?.Invoke(session, customer, content);
			break;
		case 1001:
		{
			string str = Encoding.Unicode.GetString(content);
			this.AcceptString?.Invoke(session, customer, str);
			break;
		}
		}
	}

	public void Send(NetHandle customer, string str)
	{
		if (IsClientStart)
		{
			Send(session.WorkSocket, HslProtocol.CommandBytes(customer, base.Token, str));
		}
	}

	public void Send(NetHandle customer, byte[] bytes)
	{
		if (IsClientStart)
		{
			Send(session.WorkSocket, HslProtocol.CommandBytes(customer, base.Token, bytes));
		}
	}

	private void ThreadHeartCheck()
	{
		HslHelper.ThreadSleep(2000);
		while (true)
		{
			HslHelper.ThreadSleep(10000);
			if (closed)
			{
				break;
			}
			byte[] array = new byte[16];
			BitConverter.GetBytes(DateTime.Now.Ticks).CopyTo(array, 0);
			Send(session.WorkSocket, HslProtocol.CommandBytes(1, 0, base.Token, array));
			double totalSeconds = (DateTime.Now - session.HeartTime).TotalSeconds;
			if (totalSeconds > 30.0)
			{
				if (isConnecting == 0)
				{
					base.LogNet?.WriteDebug(ToString(), $"Heart Check Failed int {totalSeconds} Seconds.");
					ReconnectServer();
				}
				if (!closed)
				{
					HslHelper.ThreadSleep(1000);
				}
			}
		}
	}

	public override string ToString()
	{
		return $"NetComplexClient[{EndPointServer}]";
	}
}
