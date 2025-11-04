using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetPushServer : NetworkServerBase
{
	private Dictionary<string, string> dictSendHistory;

	private Dictionary<string, PushGroupClient> dictPushClients;

	private readonly object dicHybirdLock;

	private readonly object dicSendCacheLock;

	private Action<AppSession, string> sendAction;

	private int onlineCount = 0;

	private List<NetPushClient> pushClients;

	private object pushClientsLock;

	private bool isPushCacheAfterConnect = true;

	public int OnlineCount => onlineCount;

	public bool PushCacheAfterConnect
	{
		get
		{
			return isPushCacheAfterConnect;
		}
		set
		{
			isPushCacheAfterConnect = value;
		}
	}

	public NetPushServer()
	{
		dictPushClients = new Dictionary<string, PushGroupClient>();
		dictSendHistory = new Dictionary<string, string>();
		dicHybirdLock = new object();
		dicSendCacheLock = new object();
		sendAction = SendString;
		pushClientsLock = new object();
		pushClients = new List<NetPushClient>();
	}

	protected override void ThreadPoolLogin(Socket socket, IPEndPoint endPoint)
	{
		OperateResult<int, string> operateResult = ReceiveStringContentFromSocket(socket);
		if (!operateResult.IsSuccess)
		{
			return;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(socket, 0, "");
		if (!operateResult2.IsSuccess)
		{
			return;
		}
		AppSession appSession = new AppSession(socket)
		{
			KeyGroup = operateResult.Content2
		};
		appSession.BytesBuffer = new byte[4];
		try
		{
			socket.BeginReceive(appSession.BytesBuffer, 0, appSession.BytesBuffer.Length, SocketFlags.None, ReceiveCallback, appSession);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.SocketReceiveException, ex);
			return;
		}
		base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOnlineInfo, appSession.IpEndPoint));
		PushGroupClient pushGroupClient = GetPushGroupClient(operateResult.Content2);
		if (pushGroupClient == null)
		{
			return;
		}
		Interlocked.Increment(ref onlineCount);
		pushGroupClient.AddPushClient(appSession);
		lock (dicSendCacheLock)
		{
			if (dictSendHistory.ContainsKey(operateResult.Content2) && isPushCacheAfterConnect)
			{
				SendString(appSession, dictSendHistory[operateResult.Content2]);
			}
		}
	}

	public override void ServerClose()
	{
		base.ServerClose();
	}

	public void PushString(string key, string content)
	{
		lock (dicSendCacheLock)
		{
			if (dictSendHistory.ContainsKey(key))
			{
				dictSendHistory[key] = content;
			}
			else
			{
				dictSendHistory.Add(key, content);
			}
		}
		AddPushKey(key);
		GetPushGroupClient(key)?.PushString(content, sendAction);
	}

	public void RemoveKey(string key)
	{
		lock (dicHybirdLock)
		{
			if (dictPushClients.ContainsKey(key))
			{
				int num = dictPushClients[key].RemoveAllClient();
				for (int i = 0; i < num; i++)
				{
					Interlocked.Decrement(ref onlineCount);
				}
				dictPushClients.Remove(key);
			}
		}
	}

	public OperateResult CreatePushRemote(string ipAddress, int port, string key)
	{
		OperateResult operateResult;
		lock (pushClientsLock)
		{
			if (pushClients.Find((NetPushClient m) => m.KeyWord == key) == null)
			{
				NetPushClient netPushClient = new NetPushClient(ipAddress, port, key);
				operateResult = netPushClient.CreatePush(GetPushFromServer);
				if (operateResult.IsSuccess)
				{
					pushClients.Add(netPushClient);
				}
			}
			else
			{
				operateResult = new OperateResult(StringResources.Language.KeyIsExistAlready);
			}
		}
		return operateResult;
	}

	private void ReceiveCallback(IAsyncResult ar)
	{
		if (!(ar.AsyncState is AppSession appSession))
		{
			return;
		}
		try
		{
			Socket workSocket = appSession.WorkSocket;
			int num = workSocket.EndReceive(ar);
			if (num <= 4)
			{
				base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOfflineInfo, appSession.IpEndPoint));
				RemoveGroupOnline(appSession.KeyGroup, appSession.ClientUniqueID);
			}
			else
			{
				appSession.UpdateHeartTime();
			}
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains(StringResources.Language.SocketRemoteCloseException))
			{
				base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOfflineInfo, appSession.IpEndPoint));
				RemoveGroupOnline(appSession.KeyGroup, appSession.ClientUniqueID);
			}
			else
			{
				base.LogNet?.WriteException(ToString(), string.Format(StringResources.Language.ClientOfflineInfo, appSession.IpEndPoint), ex);
				RemoveGroupOnline(appSession.KeyGroup, appSession.ClientUniqueID);
			}
		}
	}

	private void AddPushKey(string key)
	{
		lock (dicHybirdLock)
		{
			if (!dictPushClients.ContainsKey(key))
			{
				dictPushClients.Add(key, new PushGroupClient());
			}
		}
	}

	private PushGroupClient GetPushGroupClient(string key)
	{
		PushGroupClient pushGroupClient = null;
		lock (dicHybirdLock)
		{
			if (dictPushClients.ContainsKey(key))
			{
				pushGroupClient = dictPushClients[key];
			}
			else
			{
				pushGroupClient = new PushGroupClient();
				dictPushClients.Add(key, pushGroupClient);
			}
		}
		return pushGroupClient;
	}

	private void RemoveGroupOnline(string key, string clientID)
	{
		PushGroupClient pushGroupClient = GetPushGroupClient(key);
		if (pushGroupClient != null && pushGroupClient.RemovePushClient(clientID))
		{
			Interlocked.Decrement(ref onlineCount);
		}
	}

	private void SendString(AppSession appSession, string content)
	{
		if (!Send(appSession.WorkSocket, HslProtocol.CommandBytes(0, base.Token, content)).IsSuccess)
		{
			RemoveGroupOnline(appSession.KeyGroup, appSession.ClientUniqueID);
		}
	}

	private void GetPushFromServer(NetPushClient pushClient, string data)
	{
		PushString(pushClient.KeyWord, data);
	}

	public override string ToString()
	{
		return $"NetPushServer[{base.Port}]";
	}
}
