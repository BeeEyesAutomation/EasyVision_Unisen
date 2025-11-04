using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;

namespace HslCommunication.WebSocket;

public class WebSocketServer : CommunicationTcpServer, IDisposable
{
	public delegate void OnClientApplicationMessageReceiveDelegate(WebSocketSession session, WebSocketMessage message);

	public delegate void OnClientConnectedDelegate(WebSocketSession session);

	private readonly Dictionary<string, string> retainKeys;

	private readonly object keysLock;

	private bool isRetain = true;

	private readonly List<WebSocketSession> wsSessions = new List<WebSocketSession>();

	private readonly object sessionsLock = new object();

	private Timer timerHeart;

	private bool disposedValue;

	private bool topicWildcard = false;

	private AsyncCallback beginReceiveCallback = null;

	public int OnlineCount => wsSessions.Count;

	public bool TopicWildcard
	{
		get
		{
			return topicWildcard;
		}
		set
		{
			topicWildcard = value;
		}
	}

	public bool IsTopicRetain
	{
		get
		{
			return isRetain;
		}
		set
		{
			isRetain = value;
		}
	}

	public WebSocketSession[] OnlineSessions
	{
		get
		{
			WebSocketSession[] result = null;
			lock (sessionsLock)
			{
				result = wsSessions.ToArray();
			}
			return result;
		}
	}

	public TimeSpan KeepAlivePeriod { get; set; } = TimeSpan.FromSeconds(120.0);

	public TimeSpan KeepAliveSendInterval { get; set; } = TimeSpan.FromSeconds(30.0);

	public event OnClientApplicationMessageReceiveDelegate OnClientApplicationMessageReceive;

	public event OnClientConnectedDelegate OnClientConnected;

	public event OnClientConnectedDelegate OnClientDisConnected;

	public WebSocketServer()
	{
		beginReceiveCallback = ReceiveCallback;
		retainKeys = new Dictionary<string, string>();
		keysLock = new object();
	}

	protected override void ExtraOnStart()
	{
		base.ExtraOnStart();
		if (KeepAliveSendInterval.TotalMilliseconds > 0.0 && timerHeart == null)
		{
			timerHeart = new Timer(ThreadTimerHeartCheck, null, 2000, (int)KeepAliveSendInterval.TotalMilliseconds);
		}
	}

	protected override void ExtraOnClose()
	{
		base.ExtraOnClose();
		CleanWsSession();
	}

	private void ThreadTimerHeartCheck(object obj)
	{
		WebSocketSession[] array = null;
		lock (sessionsLock)
		{
			array = wsSessions.ToArray();
		}
		if (array == null || array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsQASession)
			{
				if (DateTime.Now - array[i].HeartTime > KeepAlivePeriod)
				{
					RemoveAndCloseSession(array[i], "Heart check timeout[" + SoftBasic.GetTimeSpanDescription(DateTime.Now - array[i].HeartTime) + "]");
				}
				else
				{
					SendWebsocket(array[i], WebSocketHelper.WebScoketPackData(9, isMask: false, "Heart Check"));
				}
			}
		}
	}

	protected override void ThreadPoolLogin(PipeTcpNet pipe, IPEndPoint endPoint)
	{
		HandleWebsocketConnection(pipe, endPoint);
	}

	private async void ReceiveCallback(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (asyncState is WebSocketSession session)
		{
			PipeTcpNet pipe = (PipeTcpNet)session.Communication;
			try
			{
				pipe.Socket.EndReceive(ar);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception ex3 = ex2;
				session.Close();
				base.LogNet?.WriteDebug(ToString(), "ReceiveCallback Failed:" + ex3.Message);
				RemoveAndCloseSession(session);
				return;
			}
			WebSocketSession session2 = session;
			HandleWebsocketMessage(session2, await WebSocketHelper.ReceiveWebSocketPayloadAsync(pipe));
		}
	}

	private OperateResult SendWebsocket(WebSocketSession session, byte[] data)
	{
		return session.Communication.Send(data);
	}

	private async Task<OperateResult> SendWebsocketAsync(WebSocketSession session, byte[] data)
	{
		return await session.Communication.SendAsync(data).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void HandleWebsocketConnection(PipeTcpNet pipe, IPEndPoint endPoint)
	{
		WebSocketSession webSocketSession = new WebSocketSession
		{
			HeartTime = DateTime.Now,
			Remote = endPoint,
			Communication = pipe
		};
		OperateResult<byte[]> operateResult = webSocketSession.Communication.Receive(-1, 5000);
		if (!operateResult.IsSuccess)
		{
			return;
		}
		string text = Encoding.UTF8.GetString(operateResult.Content);
		OperateResult operateResult2 = WebSocketHelper.CheckWebSocketLegality(text);
		if (!operateResult2.IsSuccess)
		{
			pipe?.CloseCommunication();
			base.LogNet?.WriteDebug(ToString(), $"[{endPoint}] WebScoket Check Failed:" + operateResult2.Message + Environment.NewLine + text);
			return;
		}
		OperateResult<byte[]> response = WebSocketHelper.GetResponse(text);
		if (!response.IsSuccess)
		{
			pipe?.CloseCommunication();
			base.LogNet?.WriteDebug(ToString(), $"[{endPoint}] GetResponse Failed:" + response.Message);
			return;
		}
		OperateResult operateResult3 = SendWebsocket(webSocketSession, response.Content);
		if (!operateResult3.IsSuccess)
		{
			return;
		}
		webSocketSession.IsQASession = text.Contains("HslRequestAndAnswer: true") || text.Contains("HslRequestAndAnswer:true");
		Match match = Regex.Match(text, "GET [\\S\\s]+ HTTP/1", RegexOptions.IgnoreCase);
		if (match.Success)
		{
			webSocketSession.Url = match.Value.Substring(4, match.Value.Length - 11);
		}
		try
		{
			string[] array = WebSocketHelper.GetWebSocketSubscribes(text);
			if (array == null)
			{
				array = WebSocketHelper.GetWebSocketSubscribesFromUrl(webSocketSession.Url);
			}
			if (array != null)
			{
				webSocketSession.Topics = new List<string>(array);
				if (isRetain)
				{
					lock (keysLock)
					{
						if (TopicWildcard)
						{
							foreach (KeyValuePair<string, string> retainKey in retainKeys)
							{
								if (webSocketSession.IsClientSubscribe(retainKey.Key, TopicWildcard))
								{
									operateResult3 = SendWebsocket(webSocketSession, WebSocketHelper.WebScoketPackData(1, isMask: false, retainKey.Value));
									if (!operateResult3.IsSuccess)
									{
										return;
									}
								}
							}
						}
						else
						{
							for (int i = 0; i < webSocketSession.Topics.Count; i++)
							{
								if (retainKeys.ContainsKey(webSocketSession.Topics[i]))
								{
									operateResult3 = SendWebsocket(webSocketSession, WebSocketHelper.WebScoketPackData(1, isMask: false, retainKeys[webSocketSession.Topics[i]]));
									if (!operateResult3.IsSuccess)
									{
										return;
									}
								}
							}
						}
					}
				}
			}
			pipe.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, webSocketSession);
			AddWsSession(webSocketSession);
		}
		catch (Exception ex)
		{
			pipe?.CloseCommunication();
			base.LogNet?.WriteDebug(ToString(), $"[{webSocketSession.Remote}] BeginReceive Failed: {ex.Message}");
			return;
		}
		this.OnClientConnected?.Invoke(webSocketSession);
	}

	private void HandleWebsocketMessage(WebSocketSession session, OperateResult<WebSocketMessage> read)
	{
		if (!read.IsSuccess)
		{
			RemoveAndCloseSession(session);
			return;
		}
		session.HeartTime = DateTime.Now;
		if (read.Content.OpCode == 8)
		{
			session.Close();
			RemoveAndCloseSession(session, Encoding.UTF8.GetString(read.Content.Payload));
			return;
		}
		if (read.Content.OpCode == 9)
		{
			base.LogNet?.WriteDebug(ToString(), $"[{session.Remote}] PING: {read.Content}");
			OperateResult operateResult = SendWebsocket(session, WebSocketHelper.WebScoketPackData(10, isMask: false, read.Content.Payload));
			if (!operateResult.IsSuccess)
			{
				RemoveAndCloseSession(session, "HandleWebsocketMessage -> 09 opCode send back exception -> " + operateResult.Message);
				return;
			}
		}
		else if (read.Content.OpCode == 10)
		{
			base.LogNet?.WriteDebug(ToString(), $"[{session.Remote}] PONG: {read.Content}");
		}
		else
		{
			this.OnClientApplicationMessageReceive?.Invoke(session, read.Content);
		}
		try
		{
			PipeTcpNet pipeTcpNet = session.Communication as PipeTcpNet;
			pipeTcpNet.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, session);
		}
		catch (Exception ex)
		{
			session.Close();
			RemoveAndCloseSession(session, "BeginReceive Exception -> " + ex.Message);
		}
	}

	private void PublishSessionList(IEnumerable<WebSocketSession> sessions, string payload)
	{
		foreach (WebSocketSession session in sessions)
		{
			OperateResult operateResult = SendWebsocket(session, WebSocketHelper.WebScoketPackData(1, isMask: false, payload));
			if (!operateResult.IsSuccess)
			{
				base.LogNet?.WriteError(ToString(), $"[{session.Remote}] Send Failed: {operateResult.Message}");
			}
		}
	}

	private void PublishSessionList(IEnumerable<WebSocketSession> sessions, byte[] payload)
	{
		foreach (WebSocketSession session in sessions)
		{
			OperateResult operateResult = SendWebsocket(session, WebSocketHelper.WebScoketPackData(2, isMask: false, payload));
			if (!operateResult.IsSuccess)
			{
				base.LogNet?.WriteError(ToString(), $"[{session.Remote}] Send Failed: {operateResult.Message}");
			}
		}
	}

	public void PublishAllClientPayload(string payload)
	{
		List<WebSocketSession> list = new List<WebSocketSession>();
		lock (sessionsLock)
		{
			for (int i = 0; i < wsSessions.Count; i++)
			{
				if (!wsSessions[i].IsQASession)
				{
					list.Add(wsSessions[i]);
				}
			}
		}
		PublishSessionList(list, payload);
	}

	public void PublishClientPayload(string topic, string payload)
	{
		PublishClientPayload(topic, payload, isRetain);
	}

	public void PublishAllClientPayload(byte[] payload)
	{
		List<WebSocketSession> list = new List<WebSocketSession>();
		lock (sessionsLock)
		{
			for (int i = 0; i < wsSessions.Count; i++)
			{
				if (!wsSessions[i].IsQASession)
				{
					list.Add(wsSessions[i]);
				}
			}
		}
		PublishSessionList(list, payload);
	}

	public void PublishClientPayload(string topic, byte[] payload)
	{
		List<WebSocketSession> list = new List<WebSocketSession>();
		lock (sessionsLock)
		{
			for (int i = 0; i < wsSessions.Count; i++)
			{
				if (!wsSessions[i].IsQASession && wsSessions[i].IsClientSubscribe(topic, topicWildcard))
				{
					list.Add(wsSessions[i]);
				}
			}
		}
		PublishSessionList(list, payload);
	}

	public void PublishClientBinary(byte[] binary)
	{
		for (int i = 0; i < wsSessions.Count; i++)
		{
			OperateResult operateResult = SendWebsocket(wsSessions[i], binary);
			if (!operateResult.IsSuccess)
			{
				base.LogNet?.WriteError(ToString(), $"[{wsSessions[i].Remote}] Send Failed: {operateResult.Message}");
			}
		}
	}

	public void PublishClientPayload(string topic, string payload, bool retain)
	{
		List<WebSocketSession> list = new List<WebSocketSession>();
		lock (sessionsLock)
		{
			for (int i = 0; i < wsSessions.Count; i++)
			{
				if (!wsSessions[i].IsQASession && wsSessions[i].IsClientSubscribe(topic, topicWildcard))
				{
					list.Add(wsSessions[i]);
				}
			}
		}
		PublishSessionList(list, payload);
		if (retain)
		{
			AddTopicRetain(topic, payload);
		}
	}

	private async Task PublishSessionListAsync(List<WebSocketSession> sessions, string payload)
	{
		for (int i = 0; i < sessions.Count; i++)
		{
			OperateResult send = await SendWebsocketAsync(sessions[i], WebSocketHelper.WebScoketPackData(1, isMask: false, payload));
			if (!send.IsSuccess)
			{
				base.LogNet?.WriteError(ToString(), $"[{sessions[i].Remote}] Send Failed: {send.Message}");
			}
		}
	}

	public async Task PublishAllClientPayloadAsync(string payload)
	{
		List<WebSocketSession> sessions = new List<WebSocketSession>();
		lock (sessionsLock)
		{
			for (int i = 0; i < wsSessions.Count; i++)
			{
				if (!wsSessions[i].IsQASession)
				{
					sessions.Add(wsSessions[i]);
				}
			}
		}
		await PublishSessionListAsync(sessions, payload);
	}

	public async Task PublishClientPayloadAsync(string topic, string payload)
	{
		await PublishClientPayloadAsync(topic, payload, isRetain);
	}

	public async Task PublishClientPayloadAsync(string topic, string payload, bool retain)
	{
		List<WebSocketSession> sessions = new List<WebSocketSession>();
		lock (sessionsLock)
		{
			for (int i = 0; i < wsSessions.Count; i++)
			{
				if (!wsSessions[i].IsQASession && wsSessions[i].IsClientSubscribe(topic, topicWildcard))
				{
					sessions.Add(wsSessions[i]);
				}
			}
		}
		await PublishSessionListAsync(sessions, payload);
		if (retain)
		{
			AddTopicRetain(topic, payload);
		}
	}

	public void SendClientPayload(WebSocketSession session, string payload)
	{
		SendWebsocket(session, WebSocketHelper.WebScoketPackData(1, isMask: false, payload));
	}

	public void AddSessionTopic(WebSocketSession session, string topic)
	{
		session.AddTopic(topic);
		PublishSessionTopic(session, topic);
	}

	private void CleanWsSession()
	{
		lock (sessionsLock)
		{
			for (int i = 0; i < wsSessions.Count; i++)
			{
				wsSessions[i].Close();
			}
			wsSessions.Clear();
		}
	}

	private void AddWsSession(WebSocketSession session)
	{
		lock (sessionsLock)
		{
			wsSessions.Add(session);
		}
		base.LogNet?.WriteDebug(ToString(), $"Client[{session.Remote}] Online");
	}

	public void RemoveAndCloseSession(WebSocketSession session, string reason = null)
	{
		lock (sessionsLock)
		{
			wsSessions.Remove(session);
		}
		session.Close();
		base.LogNet?.WriteDebug(ToString(), $"Client[{session.Remote}]  Offline {reason}");
		this.OnClientDisConnected?.Invoke(session);
	}

	private void AddTopicRetain(string topic, string payload)
	{
		lock (keysLock)
		{
			if (retainKeys.ContainsKey(topic))
			{
				retainKeys[topic] = payload;
			}
			else
			{
				retainKeys.Add(topic, payload);
			}
		}
	}

	private void PublishSessionTopic(WebSocketSession session, string topic)
	{
		bool flag = false;
		string message = string.Empty;
		lock (keysLock)
		{
			if (retainKeys.ContainsKey(topic))
			{
				flag = true;
				message = retainKeys[topic];
			}
		}
		if (flag)
		{
			SendWebsocket(session, WebSocketHelper.WebScoketPackData(1, isMask: false, message));
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				this.OnClientApplicationMessageReceive = null;
				this.OnClientConnected = null;
				this.OnClientDisConnected = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public override string ToString()
	{
		return $"WebSocketServer[{base.Port}]";
	}
}
