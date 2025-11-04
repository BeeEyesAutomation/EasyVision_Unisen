using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetPlainSocket : NetworkXBase
{
	private Encoding encoding;

	private object connectLock = new object();

	private string ipAddress = "127.0.0.1";

	private int port = 10000;

	private int bufferLength = 2048;

	private byte[] buffer = null;

	public Encoding Encoding
	{
		get
		{
			return encoding;
		}
		set
		{
			encoding = value;
		}
	}

	public event Action<string> ReceivedString;

	public NetPlainSocket()
	{
		buffer = new byte[bufferLength];
		encoding = Encoding.UTF8;
	}

	public NetPlainSocket(string ipAddress, int port)
	{
		buffer = new byte[bufferLength];
		encoding = Encoding.UTF8;
		this.ipAddress = ipAddress;
		this.port = port;
	}

	public OperateResult ConnectServer()
	{
		CoreSocket?.Close();
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ipAddress, port, 5000);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		try
		{
			CoreSocket = operateResult.Content;
			CoreSocket.BeginReceive(buffer, 0, bufferLength, SocketFlags.None, ReceiveCallBack, CoreSocket);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
	}

	public OperateResult ConnectClose()
	{
		try
		{
			CoreSocket?.Close();
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
	}

	public OperateResult SendString(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return OperateResult.CreateSuccessResult();
		}
		return Send(CoreSocket, encoding.GetBytes(text));
	}

	private void ReceiveCallBack(IAsyncResult ar)
	{
		if (!(ar.AsyncState is Socket socket))
		{
			return;
		}
		byte[] array = null;
		try
		{
			int num = socket.EndReceive(ar);
			socket.BeginReceive(buffer, 0, bufferLength, SocketFlags.None, ReceiveCallBack, socket);
			if (num == 0)
			{
				CoreSocket?.Close();
				return;
			}
			array = new byte[num];
			Array.Copy(buffer, 0, array, 0, num);
		}
		catch (ObjectDisposedException)
		{
		}
		catch (Exception ex2)
		{
			base.LogNet?.WriteWarn(StringResources.Language.SocketContentReceiveException + ":" + ex2.Message);
			ThreadPool.QueueUserWorkItem(ReConnectServer, null);
		}
		if (array != null)
		{
			this.ReceivedString?.Invoke(encoding.GetString(array));
		}
	}

	private void ReConnectServer(object obj)
	{
		base.LogNet?.WriteWarn(StringResources.Language.ReConnectServerAfterTenSeconds);
		for (int i = 0; i < 10; i++)
		{
			HslHelper.ThreadSleep(1000);
			base.LogNet?.WriteWarn($"Wait for connecting server after {9 - i} seconds");
		}
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ipAddress, port, 5000);
		if (!operateResult.IsSuccess)
		{
			ThreadPool.QueueUserWorkItem(ReConnectServer, obj);
			return;
		}
		lock (connectLock)
		{
			try
			{
				CoreSocket?.Close();
				CoreSocket = operateResult.Content;
				CoreSocket.BeginReceive(buffer, 0, bufferLength, SocketFlags.None, ReceiveCallBack, CoreSocket);
				base.LogNet?.WriteWarn(StringResources.Language.ReConnectServerSuccess);
			}
			catch (Exception ex)
			{
				base.LogNet?.WriteWarn(StringResources.Language.RemoteClosedConnection + ":" + ex.Message);
				ThreadPool.QueueUserWorkItem(ReConnectServer, obj);
			}
		}
	}

	public override string ToString()
	{
		return $"NetPlainSocket[{ipAddress}:{port}]";
	}
}
