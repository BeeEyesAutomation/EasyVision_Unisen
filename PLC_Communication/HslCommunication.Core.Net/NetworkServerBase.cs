using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.Core.IMessage;

namespace HslCommunication.Core.Net;

public class NetworkServerBase : NetworkXBase
{
	public bool IsStarted { get; protected set; }

	public int Port { get; set; }

	public bool EnableIPv6 { get; set; }

	public int SocketKeepAliveTime { get; set; } = -1;

	public NetworkServerBase()
	{
		IsStarted = false;
		Port = 0;
	}

	protected void AsyncAcceptCallback(IAsyncResult iar)
	{
		if (!(iar.AsyncState is Socket socket))
		{
			return;
		}
		Socket socket2 = null;
		try
		{
			socket2 = socket.EndAccept(iar);
			if (SocketKeepAliveTime > 0)
			{
				socket2.SetKeepAlive(SocketKeepAliveTime, SocketKeepAliveTime);
			}
			ThreadPool.QueueUserWorkItem(ThreadPoolLogin, socket2);
		}
		catch (ObjectDisposedException)
		{
			return;
		}
		catch (Exception ex2)
		{
			NetSupport.CloseSocket(socket2);
			base.LogNet?.WriteException(ToString(), StringResources.Language.SocketAcceptCallbackException, ex2);
		}
		int num = 0;
		while (num < 3)
		{
			try
			{
				socket.BeginAccept(AsyncAcceptCallback, socket);
			}
			catch (Exception ex3)
			{
				HslHelper.ThreadSleep(1000);
				base.LogNet?.WriteException(ToString(), StringResources.Language.SocketReAcceptCallbackException, ex3);
				num++;
				continue;
			}
			break;
		}
		if (num < 3)
		{
			return;
		}
		base.LogNet?.WriteError(ToString(), StringResources.Language.SocketReAcceptCallbackException);
		throw new Exception(StringResources.Language.SocketReAcceptCallbackException);
	}

	private void ThreadPoolLogin(object obj)
	{
		if (obj is Socket socket)
		{
			IPEndPoint iPEndPoint = (IPEndPoint)socket.RemoteEndPoint;
			OperateResult operateResult = SocketAcceptExtraCheck(socket, iPEndPoint);
			if (!operateResult.IsSuccess)
			{
				base.LogNet?.WriteDebug(ToString(), $"[{iPEndPoint}] Socket Accept Extra Check Failed : {operateResult.Message}");
				socket?.Close();
			}
			else
			{
				ThreadPoolLogin(socket, iPEndPoint);
			}
		}
	}

	protected virtual void ThreadPoolLogin(Socket socket, IPEndPoint endPoint)
	{
		NetSupport.CloseSocket(socket);
	}

	protected virtual OperateResult SocketAcceptExtraCheck(Socket socket, IPEndPoint endPoint)
	{
		return OperateResult.CreateSuccessResult();
	}

	protected virtual void StartInitialization()
	{
	}

	public virtual void ServerStart(int port)
	{
		if (!IsStarted)
		{
			StartInitialization();
			if (!EnableIPv6)
			{
				CoreSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				CoreSocket.Bind(new IPEndPoint(IPAddress.Any, port));
			}
			else
			{
				CoreSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
				CoreSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, port));
			}
			CoreSocket.Listen(500);
			CoreSocket.BeginAccept(AsyncAcceptCallback, CoreSocket);
			IsStarted = true;
			Port = port;
			base.LogNet?.WriteInfo(ToString(), StringResources.Language.NetEngineStart);
		}
	}

	public void ServerStart()
	{
		ServerStart(Port);
	}

	protected virtual void CloseAction()
	{
	}

	public virtual void ServerClose()
	{
		if (IsStarted)
		{
			IsStarted = false;
			CloseAction();
			NetSupport.CloseSocket(CoreSocket);
			base.LogNet?.WriteInfo(ToString(), StringResources.Language.NetEngineClose);
		}
	}

	private byte[] CreateHslAlienMessage(string dtuId, string password)
	{
		if (dtuId.Length > 11)
		{
			dtuId = dtuId.Substring(11);
		}
		byte[] array = new byte[28]
		{
			72, 115, 110, 0, 23, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0
		};
		if (dtuId.Length > 11)
		{
			dtuId = dtuId.Substring(0, 11);
		}
		Encoding.ASCII.GetBytes(dtuId).CopyTo(array, 5);
		if (!string.IsNullOrEmpty(password))
		{
			if (password.Length > 6)
			{
				password = password.Substring(6);
			}
			Encoding.ASCII.GetBytes(password).CopyTo(array, 16);
		}
		return array;
	}

	public OperateResult ConnectHslAlientClient(string ipAddress, int port, string dtuId, string password = "")
	{
		byte[] data = CreateHslAlienMessage(dtuId, password);
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ipAddress, port, 10000);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = Send(operateResult.Content, data);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = ReceiveByMessage(operateResult.Content, 10000, new AlienMessage());
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		switch (operateResult3.Content[5])
		{
		case 1:
			operateResult.Content?.Close();
			return new OperateResult(StringResources.Language.DeviceCurrentIsLoginRepeat);
		case 2:
			operateResult.Content?.Close();
			return new OperateResult(StringResources.Language.DeviceCurrentIsLoginForbidden);
		case 3:
			operateResult.Content?.Close();
			return new OperateResult(StringResources.Language.PasswordCheckFailed);
		default:
			ThreadPoolLogin(operateResult.Content);
			return OperateResult.CreateSuccessResult();
		}
	}

	public async Task<OperateResult> ConnectHslAlientClientAsync(string ipAddress, int port, string dtuId, string password = "")
	{
		byte[] sendBytes = CreateHslAlienMessage(dtuId, password);
		OperateResult<Socket> connect = await CreateSocketAndConnectAsync(ipAddress, port, 10000);
		if (!connect.IsSuccess)
		{
			return connect;
		}
		OperateResult send = await SendAsync(connect.Content, sendBytes);
		if (!send.IsSuccess)
		{
			return send;
		}
		OperateResult<byte[]> receive = await ReceiveByMessageAsync(connect.Content, 10000, new AlienMessage());
		if (!receive.IsSuccess)
		{
			return receive;
		}
		switch (receive.Content[5])
		{
		case 1:
			connect.Content?.Close();
			return new OperateResult(StringResources.Language.DeviceCurrentIsLoginRepeat);
		case 2:
			connect.Content?.Close();
			return new OperateResult(StringResources.Language.DeviceCurrentIsLoginForbidden);
		case 3:
			connect.Content?.Close();
			return new OperateResult(StringResources.Language.PasswordCheckFailed);
		default:
			ThreadPoolLogin(connect.Content);
			return OperateResult.CreateSuccessResult();
		}
	}

	public override string ToString()
	{
		return $"NetworkServerBase[{Port}]";
	}
}
