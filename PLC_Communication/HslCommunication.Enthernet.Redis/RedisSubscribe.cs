using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using HslCommunication.Core;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet.Redis;

public class RedisSubscribe : NetworkXBase
{
	public delegate void RedisMessageReceiveDelegate(string topic, string message);

	private IPEndPoint endPoint;

	private List<string> keyWords = null;

	private object listLock = new object();

	private int reconnectTime = 10000;

	private int connectTimeOut = 5000;

	public string Password { get; set; }

	public int ConnectTimeOut
	{
		get
		{
			return connectTimeOut;
		}
		set
		{
			connectTimeOut = value;
		}
	}

	public event RedisMessageReceiveDelegate OnRedisMessageReceived;

	public RedisSubscribe(string ipAddress, int port)
	{
		endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
		keyWords = new List<string>();
	}

	public RedisSubscribe(string ipAddress, int port, string[] keys)
	{
		endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
		keyWords = new List<string>(keys);
	}

	public RedisSubscribe(string ipAddress, int port, string key)
	{
		endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
		keyWords = new List<string> { key };
	}

	private OperateResult CreatePush()
	{
		CoreSocket?.Close();
		OperateResult<Socket> operateResult = CreateSocketAndConnect(endPoint, connectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (!string.IsNullOrEmpty(Password))
		{
			OperateResult operateResult2 = Send(operateResult.Content, RedisHelper.PackStringCommand(new string[2] { "AUTH", Password }));
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult<byte[]> operateResult3 = ReceiveRedisCommand(operateResult.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			string text = Encoding.UTF8.GetString(operateResult3.Content);
			if (!text.StartsWith("+OK"))
			{
				return new OperateResult(text);
			}
		}
		List<string> list = keyWords;
		if (list != null && list.Count > 0)
		{
			OperateResult operateResult4 = Send(operateResult.Content, RedisHelper.PackSubscribeCommand(keyWords.ToArray()));
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
		}
		CoreSocket = operateResult.Content;
		try
		{
			operateResult.Content.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallBack, operateResult.Content);
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
		return OperateResult.CreateSuccessResult();
	}

	private void ReceiveCallBack(IAsyncResult ar)
	{
		if (!(ar.AsyncState is Socket socket))
		{
			return;
		}
		try
		{
			int num = socket.EndReceive(ar);
		}
		catch (ObjectDisposedException)
		{
			base.LogNet?.WriteWarn("Socket Disposed!");
			return;
		}
		catch (Exception ex2)
		{
			SocketReceiveException(ex2);
			return;
		}
		OperateResult<byte[]> operateResult = ReceiveRedisCommand(socket);
		if (!operateResult.IsSuccess)
		{
			SocketReceiveException(null);
			return;
		}
		try
		{
			socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveCallBack, socket);
		}
		catch (Exception ex3)
		{
			SocketReceiveException(ex3);
			return;
		}
		OperateResult<string[]> stringsFromCommandLine = RedisHelper.GetStringsFromCommandLine(operateResult.Content);
		if (!stringsFromCommandLine.IsSuccess)
		{
			base.LogNet?.WriteWarn(stringsFromCommandLine.Message);
		}
		else if (!(stringsFromCommandLine.Content[0].ToUpper() == "SUBSCRIBE"))
		{
			if (stringsFromCommandLine.Content[0].ToUpper() == "MESSAGE")
			{
				this.OnRedisMessageReceived?.Invoke(stringsFromCommandLine.Content[1], stringsFromCommandLine.Content[2]);
			}
			else
			{
				base.LogNet?.WriteWarn(stringsFromCommandLine.Content[0]);
			}
		}
	}

	private void SocketReceiveException(Exception ex)
	{
		do
		{
			if (ex != null)
			{
				base.LogNet?.WriteException("Offline", ex);
			}
			Console.WriteLine(StringResources.Language.ReConnectServerAfterTenSeconds);
			HslHelper.ThreadSleep(reconnectTime);
		}
		while (!CreatePush().IsSuccess);
		Console.WriteLine(StringResources.Language.ReConnectServerSuccess);
	}

	private void AddSubTopics(string[] topics)
	{
		lock (listLock)
		{
			for (int i = 0; i < topics.Length; i++)
			{
				if (!keyWords.Contains(topics[i]))
				{
					keyWords.Add(topics[i]);
				}
			}
		}
	}

	private void RemoveSubTopics(string[] topics)
	{
		lock (listLock)
		{
			for (int i = 0; i < topics.Length; i++)
			{
				if (keyWords.Contains(topics[i]))
				{
					keyWords.Remove(topics[i]);
				}
			}
		}
	}

	public OperateResult SubscribeMessage(string topic)
	{
		return SubscribeMessage(new string[1] { topic });
	}

	public OperateResult SubscribeMessage(string[] topics)
	{
		if (topics == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (topics.Length == 0)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (CoreSocket == null)
		{
			OperateResult operateResult = ConnectServer();
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		OperateResult operateResult2 = Send(CoreSocket, RedisHelper.PackSubscribeCommand(topics));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		AddSubTopics(topics);
		return OperateResult.CreateSuccessResult();
	}

	public OperateResult UnSubscribeMessage(string[] topics)
	{
		if (CoreSocket == null)
		{
			OperateResult operateResult = ConnectServer();
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		OperateResult operateResult2 = Send(CoreSocket, RedisHelper.PackUnSubscribeCommand(topics));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		RemoveSubTopics(topics);
		return OperateResult.CreateSuccessResult();
	}

	public OperateResult UnSubscribeMessage(string topic)
	{
		return UnSubscribeMessage(new string[1] { topic });
	}

	public OperateResult ConnectServer()
	{
		return CreatePush();
	}

	public void ConnectClose()
	{
		CoreSocket?.Close();
		lock (listLock)
		{
			keyWords.Clear();
		}
	}

	public override string ToString()
	{
		return $"RedisSubscribe[{endPoint}]";
	}
}
