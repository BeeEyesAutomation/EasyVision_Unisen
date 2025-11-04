using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetPushClient : NetworkXBase
{
	private readonly IPEndPoint endPoint;

	private readonly string keyWord = string.Empty;

	private Action<NetPushClient, string> action;

	private int reconnectTime = 10000;

	private bool closed = false;

	public string KeyWord => keyWord;

	public int ReConnectTime
	{
		get
		{
			return reconnectTime;
		}
		set
		{
			reconnectTime = value;
		}
	}

	public event Action<NetPushClient, string> OnReceived;

	public NetPushClient(string ipAddress, int port, string key)
	{
		endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
		keyWord = key;
		if (string.IsNullOrEmpty(key))
		{
			throw new Exception(StringResources.Language.KeyIsNotAllowedNull);
		}
	}

	public OperateResult CreatePush(Action<NetPushClient, string> pushCallBack)
	{
		action = pushCallBack;
		return CreatePush();
	}

	public OperateResult CreatePush()
	{
		CoreSocket?.Close();
		OperateResult<Socket> operateResult = CreateSocketAndConnect(endPoint, 5000);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 0, keyWord);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<int, string> operateResult3 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		if (operateResult3.Content1 != 0)
		{
			operateResult.Content?.Close();
			return new OperateResult(operateResult3.Content2);
		}
		AppSession appSession = new AppSession(operateResult.Content);
		CoreSocket = operateResult.Content;
		try
		{
			appSession.WorkSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallback, appSession);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.SocketReceiveException, ex);
			return new OperateResult(ex.Message);
		}
		closed = false;
		return OperateResult.CreateSuccessResult();
	}

	public void ClosePush()
	{
		action = null;
		closed = true;
		if (CoreSocket != null && CoreSocket.Connected)
		{
			CoreSocket?.Send(BitConverter.GetBytes(100));
		}
		HslHelper.ThreadSleep(20);
		CoreSocket?.Close();
	}

	private void ReconnectServer(object obj)
	{
		do
		{
			if (closed)
			{
				return;
			}
			Console.WriteLine(StringResources.Language.ReConnectServerAfterTenSeconds);
			HslHelper.ThreadSleep(reconnectTime);
			if (closed)
			{
				return;
			}
		}
		while (!CreatePush().IsSuccess);
		Console.WriteLine(StringResources.Language.ReConnectServerSuccess);
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
		int protocol = read.Content1;
		_ = read.Content2;
		byte[] content = read.Content3;
		switch (protocol)
		{
		case 1001:
			action?.Invoke(this, Encoding.Unicode.GetString(content));
			this.OnReceived?.Invoke(this, Encoding.Unicode.GetString(content));
			break;
		case 1:
			Send(appSession.WorkSocket, HslProtocol.CommandBytes(1, 0, base.Token, new byte[0]));
			break;
		}
		try
		{
			appSession.WorkSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallback, appSession);
		}
		catch
		{
			ThreadPool.QueueUserWorkItem(ReconnectServer, null);
		}
	}

	public override string ToString()
	{
		return $"NetPushClient[{endPoint}]";
	}
}
