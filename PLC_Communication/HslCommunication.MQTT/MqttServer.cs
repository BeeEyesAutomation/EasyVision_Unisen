using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Core.Security;
using HslCommunication.LogNet;
using HslCommunication.Reflection;
using Newtonsoft.Json.Linq;

namespace HslCommunication.MQTT;

public class MqttServer : CommunicationTcpServer, IDisposable
{
	public delegate OperateResult FileOperateVerificationDelegate(MqttSession session, byte code, string[] groups, string[] fileNames);

	public delegate void FileChangedDelegate(MqttSession session, MqttFileOperateInfo operateInfo);

	public delegate void OnClientApplicationMessageReceiveDelegate(MqttSession session, MqttClientApplicationMessage message);

	public delegate void OnClientConnectedDelegate(MqttSession session);

	public delegate int ClientVerificationDelegate(MqttSession mqttSession, string clientId, string userName, string password);

	private Dictionary<string, MqttRpcApiInfo> apiTopicServiceDict;

	private object rpcApiLock;

	private readonly Dictionary<string, FileMarkId> dictionaryFilesMarks;

	private readonly object dictHybirdLock;

	private string filesDirectoryPath = null;

	private bool fileServerEnabled = false;

	private Dictionary<string, GroupFileContainer> m_dictionary_group_marks = new Dictionary<string, GroupFileContainer>();

	private SimpleHybirdLock group_marks_lock = new SimpleHybirdLock();

	private MqttFileMonitor fileMonitor = new MqttFileMonitor();

	private readonly Dictionary<string, MqttClientApplicationMessage> retainKeys;

	private readonly object keysLock;

	private readonly List<MqttSession> mqttSessions = new List<MqttSession>();

	private readonly object sessionsLock = new object();

	private Timer timerHeart;

	private LogStatisticsDict statisticsDict;

	private bool disposedValue;

	private RSACryptoServiceProvider providerMqttServer = null;

	private bool topicWildcard = false;

	private LogStatistics topicStatistics;

	private AsyncCallback beginReceiveCallback;

	public Func<MqttSession, string, string, OperateResult<string, bool>> DownloadFileRedirect { get; set; }

	public LogStatisticsDict LogStatistics => statisticsDict;

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

	public int OnlineCount => mqttSessions.Count;

	public MqttSession[] OnlineSessions
	{
		get
		{
			MqttSession[] result = null;
			lock (sessionsLock)
			{
				result = mqttSessions.ToArray();
			}
			return result;
		}
	}

	public MqttSession[] MqttOnlineSessions
	{
		get
		{
			MqttSession[] result = null;
			lock (sessionsLock)
			{
				result = mqttSessions.Where((MqttSession m) => m.Protocol == "MQTT").ToArray();
			}
			return result;
		}
	}

	public MqttSession[] SyncOnlineSessions
	{
		get
		{
			MqttSession[] result = null;
			lock (sessionsLock)
			{
				result = mqttSessions.Where((MqttSession m) => m.Protocol == "HUSL").ToArray();
			}
			return result;
		}
	}

	public LogStatistics TopicStatistics
	{
		get
		{
			return topicStatistics;
		}
		set
		{
			topicStatistics = value;
		}
	}

	public event FileOperateVerificationDelegate FileOperateVerification;

	public event FileChangedDelegate OnFileChangedEvent;

	public event OnClientApplicationMessageReceiveDelegate OnClientApplicationMessageReceive;

	public event OnClientConnectedDelegate OnClientConnected;

	public event OnClientConnectedDelegate OnClientDisConnected;

	public event ClientVerificationDelegate ClientVerification;

	public MqttServer(RSACryptoServiceProvider providerServer = null)
	{
		beginReceiveCallback = SocketReceiveCallback;
		statisticsDict = new LogStatisticsDict(GenerateMode.ByEveryDay, 60);
		retainKeys = new Dictionary<string, MqttClientApplicationMessage>();
		apiTopicServiceDict = new Dictionary<string, MqttRpcApiInfo>();
		keysLock = new object();
		rpcApiLock = new object();
		timerHeart = new Timer(ThreadTimerHeartCheck, null, 2000, 10000);
		dictionaryFilesMarks = new Dictionary<string, FileMarkId>();
		dictHybirdLock = new object();
		providerMqttServer = providerServer;
		topicStatistics = new LogStatistics(GenerateMode.ByEveryDay, 30);
	}

	protected override async void ThreadPoolLogin(PipeTcpNet pipe, IPEndPoint endPoint)
	{
		OperateResult<byte, byte[]> readMqtt = await MqttHelper.ReceiveMqttMessageAsync(pipe, 10000);
		if (!readMqtt.IsSuccess)
		{
			return;
		}
		RSACryptoServiceProvider clientKey = null;
		RSACryptoServiceProvider serverKey = null;
		if (readMqtt.Content1 == byte.MaxValue)
		{
			try
			{
				serverKey = providerMqttServer ?? new RSACryptoServiceProvider();
				clientKey = RSAHelper.CreateRsaProviderFromPublicKey(HslSecurity.ByteDecrypt(readMqtt.Content2));
				OperateResult send = pipe.Send(MqttHelper.BuildMqttCommand(byte.MaxValue, null, HslSecurity.ByteEncrypt(clientKey.EncryptLargeData(serverKey.GetPEMPublicKey()))).Content);
				if (!send.IsSuccess)
				{
					return;
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception ex3 = ex2;
				base.LogNet?.WriteError("创建客户端的公钥发生了异常！" + ex3.Message);
				pipe?.CloseCommunication();
				return;
			}
			readMqtt = await MqttHelper.ReceiveMqttMessageAsync(pipe, 10000);
			if (!readMqtt.IsSuccess)
			{
				return;
			}
		}
		HandleMqttConnection(pipe, endPoint, readMqtt, clientKey, serverKey);
	}

	private async void SocketReceiveCallback(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		MqttSession mqttSession = asyncState as MqttSession;
		if (mqttSession == null)
		{
			return;
		}
		try
		{
			mqttSession.MqttPipe.Socket.EndReceive(ar);
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			RemoveAndCloseSession(mqttSession, "Socket EndReceive -> " + ex3.Message);
			return;
		}
		if (mqttSession.Protocol == "FILE")
		{
			if (fileServerEnabled)
			{
				await HandleFileMessageAsync(mqttSession);
			}
			RemoveAndCloseSession(mqttSession, string.Empty);
		}
		else
		{
			OperateResult<byte, byte[]> operateResult = ((mqttSession.Protocol == "MQTT") ? (await MqttHelper.ReceiveMqttMessageAsync(mqttSession.MqttPipe, 60000)) : (await MqttHelper.ReceiveMqttMessageAsync(mqttSession.MqttPipe, 60000, delegate(long already, long total)
			{
				SyncMqttReceiveProgressBack(mqttSession.MqttPipe, already, total);
			})));
			OperateResult<byte, byte[]> readMqtt = operateResult;
			await HandleWithReceiveMqtt(mqttSession, readMqtt);
		}
	}

	private void SyncMqttReceiveProgressBack(PipeTcpNet pipe, long already, long total)
	{
		string message = ((total > 0) ? (already * 100 / total).ToString() : "100");
		byte[] array = new byte[16];
		BitConverter.GetBytes(already).CopyTo(array, 0);
		BitConverter.GetBytes(total).CopyTo(array, 8);
		pipe.Send(MqttHelper.BuildMqttCommand(15, 0, MqttHelper.BuildSegCommandByString(message), array).Content);
	}

	private void HandleMqttConnection(PipeTcpNet pipe, IPEndPoint endPoint, OperateResult<byte, byte[]> readMqtt, RSACryptoServiceProvider providerClient, RSACryptoServiceProvider providerServer)
	{
		if (!readMqtt.IsSuccess)
		{
			return;
		}
		byte[] array = readMqtt.Content2;
		if (providerClient != null)
		{
			try
			{
				array = providerServer.DecryptLargeData(array);
			}
			catch (Exception ex)
			{
				base.LogNet?.WriteError(ToString(), $"[{endPoint}] Decrypt the client's logon data exception！" + ex.Message);
				pipe?.CloseCommunication();
				return;
			}
		}
		OperateResult<int, MqttSession> operateResult = CheckMqttConnection(readMqtt.Content1, array, pipe, endPoint);
		if (!operateResult.IsSuccess)
		{
			base.LogNet?.WriteInfo(ToString(), $"[{endPoint}] Check client login failure: " + operateResult.Message);
			pipe?.CloseCommunication();
			return;
		}
		if (operateResult.Content1 != 0)
		{
			pipe.Send(MqttHelper.BuildMqttCommand(2, 0, null, new byte[2]
			{
				0,
				(byte)operateResult.Content1
			}).Content);
			pipe?.CloseCommunication();
			return;
		}
		if (providerClient == null)
		{
			pipe.Send(MqttHelper.BuildMqttCommand(2, 0, null, new byte[2]).Content);
		}
		else
		{
			operateResult.Content2.AesCryptography = new AesCryptography(HslHelper.HslRandom.GetBytes(16).ToHexString());
			byte[] payLoad = providerClient.Encrypt(Encoding.UTF8.GetBytes(operateResult.Content2.AesCryptography.Key), fOAEP: false);
			pipe.Send(MqttHelper.BuildMqttCommand(2, 0, new byte[2], payLoad).Content);
		}
		try
		{
			pipe.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, operateResult.Content2);
			AddMqttSession(operateResult.Content2);
		}
		catch (Exception ex2)
		{
			base.LogNet?.WriteDebug(ToString(), "Client Online Exception : " + ex2.Message);
			return;
		}
		if (operateResult.Content2.Protocol == "MQTT")
		{
			this.OnClientConnected?.Invoke(operateResult.Content2);
		}
	}

	private OperateResult<int, MqttSession> CheckMqttConnection(byte mqttCode, byte[] content, PipeTcpNet pipe, IPEndPoint endPoint)
	{
		if (mqttCode >> 4 != 1)
		{
			return new OperateResult<int, MqttSession>("Client Send Faied, And Close!");
		}
		if (content.Length < 10)
		{
			return new OperateResult<int, MqttSession>("Receive Data Too Short:" + SoftBasic.ByteToHexString(content, ' '));
		}
		string text = Encoding.ASCII.GetString(content, 2, 4);
		if (!(text == "MQTT") && !(text == "HUSL") && !(text == "FILE"))
		{
			return new OperateResult<int, MqttSession>("Not Mqtt Client Connection");
		}
		try
		{
			int index = 10;
			string clientId = MqttHelper.ExtraMsgFromBytes(content, ref index);
			string willTopic = (((content[7] & 4) == 4) ? MqttHelper.ExtraMsgFromBytes(content, ref index) : string.Empty);
			string s = (((content[7] & 4) == 4) ? MqttHelper.ExtraMsgFromBytes(content, ref index) : string.Empty);
			string userName = (((content[7] & 0x80) == 128) ? MqttHelper.ExtraMsgFromBytes(content, ref index) : string.Empty);
			string password = (((content[7] & 0x40) == 64) ? MqttHelper.ExtraMsgFromBytes(content, ref index) : string.Empty);
			int num = content[8] * 256 + content[9];
			MqttSession mqttSession = new MqttSession(endPoint, text)
			{
				MqttPipe = pipe,
				ClientId = clientId,
				UserName = userName,
				WillTopic = willTopic,
				WillMessage = Encoding.UTF8.GetBytes(s)
			};
			if (text == "MQTT")
			{
				mqttSession.DeveloperPermissions = false;
			}
			else if (string.Equals(mqttSession.UserName, "admin", StringComparison.OrdinalIgnoreCase))
			{
				mqttSession.DeveloperPermissions = true;
			}
			int value = ((this.ClientVerification != null) ? this.ClientVerification(mqttSession, clientId, userName, password) : 0);
			if (num > 0)
			{
				mqttSession.ActiveTimeSpan = TimeSpan.FromSeconds((double)num * 1.5);
			}
			return OperateResult.CreateSuccessResult(value, mqttSession);
		}
		catch (Exception ex)
		{
			return new OperateResult<int, MqttSession>("Client Online Exception : " + ex.Message);
		}
	}

	private async Task HandleWithReceiveMqtt(MqttSession mqttSession, OperateResult<byte, byte[]> readMqtt)
	{
		if (!readMqtt.IsSuccess)
		{
			RemoveAndCloseSession(mqttSession, readMqtt.Message);
			return;
		}
		byte code = readMqtt.Content1;
		byte[] data = readMqtt.Content2;
		try
		{
			if (code >> 4 == 14)
			{
				RemoveAndCloseSession(mqttSession, string.Empty);
				return;
			}
			mqttSession.MqttPipe.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, mqttSession);
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			RemoveAndCloseSession(mqttSession, "HandleWithReceiveMqtt exception:" + ex3.Message);
			return;
		}
		mqttSession.ActiveTime = DateTime.Now;
		if (mqttSession.Protocol != "MQTT")
		{
			await DealWithPublish(mqttSession, code, data);
		}
		else if (code >> 4 == 3)
		{
			await DealWithPublish(mqttSession, code, data);
		}
		else if (code >> 4 != 4 && code >> 4 != 5)
		{
			if (code >> 4 == 6)
			{
				mqttSession.MqttPipe.Send(MqttHelper.BuildMqttCommand(7, 0, null, data).Content);
			}
			else if (code >> 4 == 8)
			{
				DealWithSubscribe(mqttSession, code, data);
			}
			else if (code >> 4 == 10)
			{
				DealWithUnSubscribe(mqttSession, code, data);
			}
			else if (code >> 4 == 12)
			{
				mqttSession.MqttPipe.Send(MqttHelper.BuildMqttCommand(13, 0, null, null).Content);
			}
		}
	}

	protected override void ExtraOnStart()
	{
		base.ExtraOnStart();
	}

	protected override void ExtraOnClose()
	{
		base.ExtraOnClose();
		lock (sessionsLock)
		{
			for (int i = 0; i < mqttSessions.Count; i++)
			{
				mqttSessions[i].MqttPipe?.CloseCommunication();
			}
			mqttSessions.Clear();
		}
	}

	private void ThreadTimerHeartCheck(object obj)
	{
		MqttSession[] array = null;
		lock (sessionsLock)
		{
			array = mqttSessions.ToArray();
		}
		if (array == null || array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Protocol == "MQTT" && DateTime.Now - array[i].ActiveTime > array[i].ActiveTimeSpan)
			{
				RemoveAndCloseSession(array[i], "Thread Timer Heart Check failed:" + SoftBasic.GetTimeSpanDescription(DateTime.Now - array[i].ActiveTime));
			}
		}
	}

	private async Task DealWithPublish(MqttSession session, byte code, byte[] data)
	{
		OperateResult<MqttClientApplicationMessage> messageResult = MqttHelper.ParseMqttClientApplicationMessage(session, code, data);
		if (!messageResult.IsSuccess)
		{
			RemoveAndCloseSession(session, messageResult.Message);
			return;
		}
		MqttClientApplicationMessage mqttClientApplicationMessage = messageResult.Content;
		if (session.Protocol == "MQTT")
		{
			MqttQualityOfServiceLevel mqttQuality = mqttClientApplicationMessage.QualityOfServiceLevel;
			switch (mqttQuality)
			{
			case MqttQualityOfServiceLevel.AtLeastOnce:
				session.SendMqttCommand(4, 0, null, MqttHelper.BuildIntBytes(mqttClientApplicationMessage.MsgID));
				break;
			case MqttQualityOfServiceLevel.ExactlyOnce:
				session.SendMqttCommand(5, 0, null, MqttHelper.BuildIntBytes(mqttClientApplicationMessage.MsgID));
				break;
			}
			if (session.ForbidPublishTopic)
			{
				return;
			}
			this.OnClientApplicationMessageReceive?.Invoke(session, mqttClientApplicationMessage);
			if (mqttQuality != MqttQualityOfServiceLevel.OnlyTransfer && !mqttClientApplicationMessage.IsCancelPublish)
			{
				PublishTopicPayload(mqttClientApplicationMessage.Topic, mqttClientApplicationMessage.Payload, retain: false);
				if (mqttClientApplicationMessage.Retain)
				{
					RetainTopicPayload(mqttClientApplicationMessage.Topic, mqttClientApplicationMessage);
				}
				topicStatistics.StatisticsAdd(1L);
			}
		}
		else if (code >> 4 == 3)
		{
			string apiName = mqttClientApplicationMessage.Topic.Trim('/');
			MqttRpcApiInfo apiInformation = GetMqttRpcApiInfo(apiName);
			if (apiInformation == null)
			{
				this.OnClientApplicationMessageReceive?.Invoke(session, mqttClientApplicationMessage);
				return;
			}
			DateTime dateTime = DateTime.Now;
			OperateResult<string> result = await MqttHelper.HandleObjectMethod(session, mqttClientApplicationMessage, apiInformation);
			double timeSpend = Math.Round((DateTime.Now - dateTime).TotalSeconds, 5);
			apiInformation.CalledCountAddOne((long)(timeSpend * 100000.0));
			statisticsDict.StatisticsAdd(apiInformation.ApiTopic, 1L);
			base.LogNet?.WriteDebug(ToString(), $"{session} RPC: [{mqttClientApplicationMessage.Topic}] Spend:[{timeSpend * 1000.0:F2} ms] Count:[{apiInformation.CalledCount}] Return:[{result.IsSuccess}]");
			ReportOperateResult(session, result);
		}
		else if (code >> 4 == 8)
		{
			if (session.DeveloperPermissions)
			{
				ReportOperateResult(session, OperateResult.CreateSuccessResult(JArray.FromObject(GetAllMqttRpcApiInfo()).ToString()));
			}
			else
			{
				ReportOperateResult(session, StringResources.Language.DeveloperPrivileges);
			}
		}
		else if (code >> 4 == 4)
		{
			if (session.DeveloperPermissions)
			{
				PublishTopicPayload(session, "", HslProtocol.PackStringArrayToByte(GetAllRetainTopics()));
			}
			else
			{
				ReportOperateResult(session, StringResources.Language.DeveloperPrivileges);
			}
		}
		else if (code >> 4 == 6)
		{
			if (session.DeveloperPermissions)
			{
				long[] logs = (string.IsNullOrEmpty(mqttClientApplicationMessage.Topic) ? LogStatistics.LogStat.GetStatisticsSnapshot() : LogStatistics.GetStatisticsSnapshot(mqttClientApplicationMessage.Topic));
				if (logs == null)
				{
					ReportOperateResult(session, new OperateResult<string>($"{session} RPC:{mqttClientApplicationMessage.Topic} has no data or not exist."));
				}
				else
				{
					ReportOperateResult(session, OperateResult.CreateSuccessResult(logs.ToArrayString()));
				}
			}
			else
			{
				ReportOperateResult(session, StringResources.Language.DeveloperPrivileges);
			}
		}
		else if (code >> 4 == 11)
		{
			if (session.DeveloperPermissions)
			{
				ReportOperateResult(session, OperateResult.CreateSuccessResult(JArray.FromObject(OnlineSessions.Select((MqttSession m) => m.GetSessionInfo())).ToString()));
			}
			else
			{
				ReportOperateResult(session, StringResources.Language.DeveloperPrivileges);
			}
		}
		else
		{
			if (code >> 4 != 5)
			{
				return;
			}
			if (session.DeveloperPermissions)
			{
				lock (keysLock)
				{
					if (retainKeys.ContainsKey(mqttClientApplicationMessage.Topic))
					{
						PublishTopicPayload(payload: Encoding.UTF8.GetBytes(retainKeys[mqttClientApplicationMessage.Topic].ToJsonString()), session: session, topic: mqttClientApplicationMessage.Topic);
						return;
					}
					ReportOperateResult(session, StringResources.Language.KeyIsNotExist);
				}
			}
			else
			{
				ReportOperateResult(session, StringResources.Language.DeveloperPrivileges);
			}
		}
	}

	private void RetainTopicPayload(string topic, byte[] payload)
	{
		MqttClientApplicationMessage value = new MqttClientApplicationMessage
		{
			ClientId = "MqttServer",
			QualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce,
			Retain = true,
			Topic = topic,
			UserName = "MqttServer",
			Payload = payload
		};
		lock (keysLock)
		{
			if (retainKeys.ContainsKey(topic))
			{
				retainKeys[topic] = value;
			}
			else
			{
				retainKeys.Add(topic, value);
			}
		}
	}

	private void RetainTopicPayload(string topic, MqttClientApplicationMessage message)
	{
		lock (keysLock)
		{
			if (retainKeys.ContainsKey(topic))
			{
				retainKeys[topic] = message;
			}
			else
			{
				retainKeys.Add(topic, message);
			}
		}
	}

	private void DealWithSubscribe(MqttSession session, byte code, byte[] data)
	{
		int num = 0;
		int index = 0;
		num = MqttHelper.ExtraIntFromBytes(data, ref index);
		List<string> list = new List<string>();
		List<byte> list2 = new List<byte>();
		try
		{
			while (index < data.Length - 1)
			{
				MqttHelper.ExtraSubscribeMsgFromBytes(data, ref index, list, list2);
			}
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteError(ToString(), $"{session} DealWithSubscribe exception: " + ex.Message + " Source: " + data.ToHexString(' '));
			return;
		}
		session.SendMqttCommand(9, 0, MqttHelper.BuildIntBytes(num), list2.ToArray());
		lock (keysLock)
		{
			if (topicWildcard)
			{
				foreach (KeyValuePair<string, MqttClientApplicationMessage> retainKey in retainKeys)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (MqttHelper.CheckMqttTopicWildcards(retainKey.Key, list[i]))
						{
							session.MqttPipe.Send(MqttHelper.BuildPublishMqttCommand(retainKey.Key, retainKey.Value.Payload, retain: true, session.IsAesCryptography ? session.AesCryptography : null).Content);
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (retainKeys.ContainsKey(list[j]))
					{
						session.MqttPipe.Send(MqttHelper.BuildPublishMqttCommand(list[j], retainKeys[list[j]].Payload, retain: true, session.IsAesCryptography ? session.AesCryptography : null).Content);
					}
				}
			}
		}
		session.AddSubscribe(list.ToArray());
		base.LogNet?.WriteDebug(ToString(), session.ToString() + " Subscribe: " + list.ToArray().ToArrayString());
	}

	private void DealWithUnSubscribe(MqttSession session, byte code, byte[] data)
	{
		int num = 0;
		int index = 0;
		num = MqttHelper.ExtraIntFromBytes(data, ref index);
		List<string> list = new List<string>();
		while (index < data.Length)
		{
			list.Add(MqttHelper.ExtraMsgFromBytes(data, ref index));
		}
		session.SendMqttCommand(11, 0, null, MqttHelper.BuildIntBytes(num));
		session.RemoveSubscribe(list.ToArray());
		base.LogNet?.WriteDebug(ToString(), session.ToString() + " UnSubscribe: " + list.ToArray().ToArrayString());
	}

	public void PublishTopicPayload(MqttSession session, string topic, byte[] payload)
	{
		OperateResult operateResult = session.MqttPipe.Send(MqttHelper.BuildPublishMqttCommand(topic, payload, retain: false, session.IsAesCryptography ? session.AesCryptography : null).Content);
		if (!operateResult.IsSuccess)
		{
			base.LogNet?.WriteError(ToString(), $"{session} PublishTopicPayload Failed:" + operateResult.Message);
		}
		topicStatistics.StatisticsAdd(1L);
	}

	public void PublishTopicPayload(string topic, byte[] payload, bool retain, Func<MqttSession, bool> check)
	{
		lock (sessionsLock)
		{
			byte[] array = null;
			for (int i = 0; i < mqttSessions.Count; i++)
			{
				MqttSession mqttSession = mqttSessions[i];
				byte[] array2 = null;
				if (!(mqttSession.Protocol == "MQTT") || !check(mqttSession))
				{
					continue;
				}
				if (mqttSession.IsAesCryptography)
				{
					if (array2 == null)
					{
						array2 = MqttHelper.BuildPublishMqttCommand(topic, payload, retain: false, mqttSession.AesCryptography).Content;
					}
				}
				else if (array == null)
				{
					array = MqttHelper.BuildPublishMqttCommand(topic, payload).Content;
				}
				OperateResult operateResult = mqttSession.MqttPipe.Send(mqttSession.IsAesCryptography ? array2 : array);
				if (!operateResult.IsSuccess)
				{
					base.LogNet?.WriteError(ToString(), $"{mqttSession} PublishTopicPayload Failed:" + operateResult.Message);
				}
			}
		}
		if (retain)
		{
			RetainTopicPayload(topic, payload);
		}
		topicStatistics.StatisticsAdd(1L);
	}

	public void PublishTopicPayload(string topic, byte[] payload, bool retain = true)
	{
		PublishTopicPayload(topic, payload, retain, (MqttSession session) => session.IsClientSubscribe(topic, topicWildcard));
	}

	public void PublishAllClientTopicPayload(string topic, byte[] payload, bool retain = false)
	{
		PublishTopicPayload(topic, payload, retain, (MqttSession session) => true);
	}

	public void PublishTopicPayload(string clientId, string topic, byte[] payload, bool retain = false)
	{
		PublishTopicPayload(topic, payload, retain, (MqttSession session) => session.ClientId == clientId);
	}

	public void ReportProgress(MqttSession session, string topic, string payload)
	{
		if (session.Protocol == "HUSL")
		{
			payload = payload ?? string.Empty;
			OperateResult operateResult = session.SendMqttCommand(15, 0, MqttHelper.BuildSegCommandByString(topic), Encoding.UTF8.GetBytes(payload));
			if (!operateResult.IsSuccess)
			{
				base.LogNet?.WriteError(ToString(), $"{session} PublishTopicPayload Failed:" + operateResult.Message);
			}
			return;
		}
		throw new Exception("ReportProgress only support sync communication");
	}

	public void ReportOperateResult(MqttSession session, string message)
	{
		ReportOperateResult(session, new OperateResult<string>(message));
	}

	public void ReportOperateResult(MqttSession session, OperateResult<string> result)
	{
		if (session.Protocol == "HUSL")
		{
			if (result.IsSuccess)
			{
				byte[] payload = (string.IsNullOrEmpty(result.Content) ? new byte[0] : Encoding.UTF8.GetBytes(result.Content));
				PublishTopicPayload(session, result.ErrorCode.ToString(), payload);
				return;
			}
			OperateResult operateResult = session.SendMqttCommand(0, 0, MqttHelper.BuildSegCommandByString(result.ErrorCode.ToString()), string.IsNullOrEmpty(result.Message) ? new byte[0] : Encoding.UTF8.GetBytes(result.Message), session.IsAesCryptography ? session.AesCryptography : null);
			if (!operateResult.IsSuccess)
			{
				base.LogNet?.WriteError(ToString(), $"{session} PublishTopicPayload Failed:" + operateResult.Message);
			}
			return;
		}
		throw new Exception("Report Result Message only support sync communication, client is MqttSyncClient");
	}

	public async Task ReportObjectApiMethod(MqttSession session, MqttClientApplicationMessage message, object apiObject)
	{
		if (session.Protocol == "HUSL")
		{
			ReportOperateResult(session, await MqttHelper.HandleObjectMethod(session, message, apiObject));
			return;
		}
		throw new Exception("Report Result Message only support sync communication, client is MqttSyncClient");
	}

	private void MqttRpcAdd(string apiTopic, MqttRpcApiInfo apiInfo)
	{
		if (apiTopicServiceDict.ContainsKey(apiTopic))
		{
			apiTopicServiceDict[apiTopic] = apiInfo;
		}
		else
		{
			apiTopicServiceDict.Add(apiTopic, apiInfo);
		}
	}

	private bool MqttRpcRemove(string apiTopic)
	{
		if (apiTopicServiceDict.ContainsKey(apiTopic))
		{
			return apiTopicServiceDict.Remove(apiTopic);
		}
		return false;
	}

	private MqttRpcApiInfo GetMqttRpcApiInfo(string apiTopic)
	{
		MqttRpcApiInfo result = null;
		lock (rpcApiLock)
		{
			if (apiTopicServiceDict.ContainsKey(apiTopic))
			{
				result = apiTopicServiceDict[apiTopic];
			}
		}
		return result;
	}

	public MqttRpcApiInfo[] GetAllMqttRpcApiInfo()
	{
		MqttRpcApiInfo[] result = null;
		lock (rpcApiLock)
		{
			result = apiTopicServiceDict.Values.ToArray();
		}
		return result;
	}

	public void RegisterMqttRpcApi(string api, object obj, HslMqttPermissionAttribute permissionAttribute)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(api, obj, permissionAttribute))
			{
				MqttRpcAdd(item.ApiTopic, item);
			}
		}
	}

	public void RegisterMqttRpcApi(string api, object obj)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(api, obj))
			{
				MqttRpcAdd(item.ApiTopic, item);
			}
		}
	}

	public void RegisterMqttRpcApi(object obj)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(obj))
			{
				MqttRpcAdd(item.ApiTopic, item);
			}
		}
	}

	public void UnRegisterMqttRpcApi(string api, object obj)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(api, obj))
			{
				MqttRpcRemove(item.ApiTopic);
			}
		}
	}

	public void UnRegisterMqttRpcApi(object obj)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(obj))
			{
				MqttRpcRemove(item.ApiTopic);
			}
		}
	}

	public bool UnRegisterHttpRpcApiSingle(string apiTopic)
	{
		lock (rpcApiLock)
		{
			return MqttRpcRemove(apiTopic);
		}
	}

	public void UseFileServer(string filePath)
	{
		filesDirectoryPath = filePath;
		fileServerEnabled = true;
		CheckFolderAndCreate();
	}

	public void CloseFileServer()
	{
		fileServerEnabled = false;
	}

	[HslMqttApi(Description = "Get the current number of file management containers for the folder")]
	public int GroupFileContainerCount()
	{
		return m_dictionary_group_marks.Count;
	}

	[HslMqttApi(Description = "Obtain current real-time file upload and download monitoring information, operating client information, file classification, file name, upload or download speed, etc.")]
	public MqttFileMonitorItem[] GetMonitorItemsSnapShoot()
	{
		return fileMonitor.GetMonitorItemsSnapShoot();
	}

	private bool CheckPathAndFilenameLegal(string input)
	{
		return Regex.IsMatch(input, "[:?*/\\<>|\"]");
	}

	private async Task HandleFileMessageAsync(MqttSession session)
	{
		OperateResult<byte, byte[]> receiveGroupInfo = await MqttHelper.ReceiveMqttMessageAsync(session.MqttPipe, 60000);
		if (!receiveGroupInfo.IsSuccess)
		{
			return;
		}
		string[] groupInfo = HslProtocol.UnPackStringArrayFromByte(receiveGroupInfo.Content2);
		OperateResult<byte, byte[]> receiveFileNames = await MqttHelper.ReceiveMqttMessageAsync(session.MqttPipe, 60000);
		if (!receiveFileNames.IsSuccess)
		{
			return;
		}
		string[] fileNames = HslProtocol.UnPackStringArrayFromByte(receiveFileNames.Content2);
		for (int i = 0; i < groupInfo.Length; i++)
		{
			if (CheckPathAndFilenameLegal(groupInfo[i]))
			{
				session.SendMqttCommand(0, null, HslHelper.GetUTF8Bytes("Path Invalid, not include '\\/:*?\"<>|'"));
				RemoveAndCloseSession(session, "CheckPathAndFilenameLegal:" + groupInfo[i]);
				return;
			}
		}
		for (int j = 0; j < fileNames.Length; j++)
		{
			if (receiveFileNames.Content1 != 111)
			{
				if (CheckPathAndFilenameLegal(fileNames[j]))
				{
					session.SendMqttCommand(0, null, HslHelper.GetUTF8Bytes("FileName Invalid, not include '\\/:*?\"<>|'"));
					RemoveAndCloseSession(session, "CheckPathAndFilenameLegal:" + fileNames[j]);
					return;
				}
			}
			else if (Regex.IsMatch(fileNames[j], "[:?*<>|\"]"))
			{
				session.SendMqttCommand(0, null, HslHelper.GetUTF8Bytes("FileName Invalid, not include ':*?\"<>|'"));
				RemoveAndCloseSession(session, "CheckPathAndFilenameLegal:" + fileNames[j]);
				return;
			}
		}
		OperateResult opLegal = this.FileOperateVerification?.Invoke(session, receiveFileNames.Content1, groupInfo, fileNames);
		if (opLegal == null)
		{
			opLegal = OperateResult.CreateSuccessResult();
		}
		OperateResult sendLegal = await session.SendMqttCommandAsync((byte)(opLegal.IsSuccess ? 100u : 0u), null, HslHelper.GetUTF8Bytes(opLegal.Message));
		if (!opLegal.IsSuccess)
		{
			RemoveAndCloseSession(session, "FileOperateVerification:" + opLegal.Message);
			return;
		}
		if (!sendLegal.IsSuccess)
		{
			RemoveAndCloseSession(session, "FileOperate SendLegal:" + sendLegal.Message);
			return;
		}
		string relativeName2 = GetRelativeFileName(groupInfo, (fileNames != null && fileNames.Length != 0) ? fileNames[0] : string.Empty);
		if (receiveFileNames.Content1 == 101)
		{
			string fileName2 = fileNames[0];
			string guidName = TransformFactFileName(groupInfo, fileName2);
			FileMarkId fileMarkId = GetFileMarksFromDictionaryWithFileName(guidName);
			fileMarkId.EnterReadOperator();
			DateTime dateTimeStart5 = DateTime.Now;
			MqttFileMonitorItem monitorItem = new MqttFileMonitorItem
			{
				EndPoint = session.EndPoint,
				ClientId = session.ClientId,
				UserName = session.UserName,
				FileName = fileName2,
				MappingName = guidName,
				Operate = "Download",
				Groups = HslHelper.PathCombine(groupInfo)
			};
			fileMonitor.Add(monitorItem);
			string fullDownloadFileName = ReturnAbsoluteFileName(groupInfo, guidName);
			bool deleteFile = false;
			if (DownloadFileRedirect != null)
			{
				OperateResult<string, bool> redirect = DownloadFileRedirect(session, fileName2, fullDownloadFileName);
				if (redirect.IsSuccess)
				{
					fullDownloadFileName = redirect.Content1;
					deleteFile = redirect.Content2;
				}
			}
			OperateResult send = await MqttHelper.SendMqttFileAsync(session.MqttPipe, fullDownloadFileName, fileName2, "", monitorItem.UpdateProgress, session.IsAesCryptography ? session.AesCryptography : null);
			fileMarkId.LeaveReadOperator();
			fileMonitor.Remove(monitorItem.UniqueId);
			this.OnFileChangedEvent?.Invoke(session, new MqttFileOperateInfo
			{
				Groups = HslHelper.PathCombine(groupInfo),
				FileNames = fileNames,
				MappingNames = new string[1] { guidName },
				Operate = "Download",
				TimeCost = DateTime.Now - dateTimeStart5
			});
			if (deleteFile)
			{
				DeleteFileByName(fullDownloadFileName);
			}
			if (!send.IsSuccess)
			{
				base.LogNet?.WriteError(ToString(), $"{session} {StringResources.Language.FileDownloadFailed}[{send.Message}]:{relativeName2} Name:{session.UserName}" + " Spend:" + SoftBasic.GetTimeSpanDescription(DateTime.Now - dateTimeStart5));
			}
			else
			{
				base.LogNet?.WriteInfo(ToString(), $"{session} {StringResources.Language.FileDownloadSuccess}:{relativeName2} Spend:{SoftBasic.GetTimeSpanDescription(DateTime.Now - dateTimeStart5)}");
			}
		}
		else if (receiveFileNames.Content1 == 102)
		{
			string fileName3 = fileNames[0];
			string fullFileName5 = ReturnAbsoluteFileName(groupInfo, fileName3);
			CheckFolderAndCreate();
			FileInfo info5 = new FileInfo(fullFileName5);
			try
			{
				if (!Directory.Exists(info5.DirectoryName))
				{
					Directory.CreateDirectory(info5.DirectoryName);
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception ex3 = ex2;
				base.LogNet?.WriteException(ToString(), StringResources.Language.FilePathCreateFailed + fullFileName5, ex3);
				return;
			}
			string guidName2 = SoftBasic.GetUniqueStringByGuidAndRandom();
			DateTime dateTimeStart6 = DateTime.Now;
			MqttFileMonitorItem monitorItem2 = new MqttFileMonitorItem
			{
				EndPoint = session.EndPoint,
				ClientId = session.ClientId,
				UserName = session.UserName,
				FileName = fileName3,
				MappingName = guidName2,
				Operate = "Upload",
				Groups = HslHelper.PathCombine(groupInfo)
			};
			fileMonitor.Add(monitorItem2);
			OperateResult<FileBaseInfo> receive = await ReceiveMqttFileAndUpdateGroupAsync(session, info5, guidName2, monitorItem2.UpdateProgress);
			fileMonitor.Remove(monitorItem2.UniqueId);
			if (receive.IsSuccess)
			{
				this.OnFileChangedEvent?.Invoke(session, new MqttFileOperateInfo
				{
					Groups = HslHelper.PathCombine(groupInfo),
					FileNames = fileNames,
					MappingNames = new string[1] { guidName2 },
					Operate = "Upload",
					TimeCost = DateTime.Now - dateTimeStart6
				});
				base.LogNet?.WriteInfo(ToString(), $"{session} {StringResources.Language.FileUploadSuccess}:{relativeName2} Spend:{SoftBasic.GetTimeSpanDescription(DateTime.Now - dateTimeStart6)}");
			}
			else
			{
				base.LogNet?.WriteError(ToString(), $"{session} {StringResources.Language.FileUploadFailed}[{receive.Message}]:{relativeName2} Spend:{SoftBasic.GetTimeSpanDescription(DateTime.Now - dateTimeStart6)}");
			}
		}
		else if (receiveFileNames.Content1 == 103)
		{
			DateTime dateTimeStart7 = DateTime.Now;
			List<string> mappings = new List<string>();
			string[] array = fileNames;
			string[] array2 = array;
			foreach (string item in array2)
			{
				string fullFileName6 = ReturnAbsoluteFileName(groupInfo, item);
				FileInfo info6 = new FileInfo(fullFileName6);
				GroupFileContainer fileManagment7 = GetGroupFromFilePath(info6.DirectoryName);
				string guid = fileManagment7.DeleteFile(info6.Name);
				DeleteExsistingFile(info6.DirectoryName, guid);
				mappings.Add(guid);
				relativeName2 = GetRelativeFileName(groupInfo, item);
				base.LogNet?.WriteInfo(ToString(), $"{session} {StringResources.Language.FileDeleteSuccess}:{relativeName2}");
			}
			await session.SendMqttCommandAsync(103, null, null);
			this.OnFileChangedEvent?.Invoke(session, new MqttFileOperateInfo
			{
				Groups = HslHelper.PathCombine(groupInfo),
				FileNames = fileNames,
				MappingNames = mappings.ToArray(),
				Operate = "Delete",
				TimeCost = DateTime.Now - dateTimeStart7
			});
		}
		else if (receiveFileNames.Content1 == 110)
		{
			DateTime dateTimeStart8 = DateTime.Now;
			string fullFileName7 = ReturnAbsoluteFileName(groupInfo, "123.txt");
			FileInfo info7 = new FileInfo(fullFileName7);
			GroupFileContainer fileManagment8 = GetGroupFromFilePath(info7.DirectoryName);
			List<string> file_names2;
			List<string> guid_names2 = fileManagment8.ClearAllFiles(out file_names2);
			DeleteExsistingFile(info7.DirectoryName, guid_names2);
			await session.SendMqttCommandAsync(110, null, null);
			this.OnFileChangedEvent?.Invoke(session, new MqttFileOperateInfo
			{
				Groups = HslHelper.PathCombine(groupInfo),
				FileNames = file_names2.ToArray(),
				MappingNames = guid_names2.ToArray(),
				Operate = "ClearFolder",
				TimeCost = DateTime.Now - dateTimeStart8
			});
			base.LogNet?.WriteInfo(ToString(), session.ToString() + "ClearFolder : " + relativeName2);
		}
		else if (receiveFileNames.Content1 == 104)
		{
			DateTime dateTimeStart9 = DateTime.Now;
			string fullFileName8 = ReturnAbsoluteFileName(groupInfo, "123.txt");
			FileInfo info8 = new FileInfo(fullFileName8);
			GroupFileContainer fileManagment9 = GetGroupFromFilePath(info8.DirectoryName);
			List<string> file_names3;
			List<string> guid_names3 = fileManagment9.GetAllFiles(out file_names3);
			DeleteGroupFileContainer(info8.DirectoryName);
			await session.SendMqttCommandAsync(104, null, null);
			this.OnFileChangedEvent?.Invoke(session, new MqttFileOperateInfo
			{
				Groups = HslHelper.PathCombine(groupInfo),
				FileNames = file_names3.ToArray(),
				MappingNames = guid_names3.ToArray(),
				Operate = "DeleteFolder",
				TimeCost = DateTime.Now - dateTimeStart9
			});
			base.LogNet?.WriteInfo(ToString(), session.ToString() + " FolderDelete : " + relativeName2);
		}
		else if (receiveFileNames.Content1 == 111)
		{
			DateTime dateTimeStart10 = DateTime.Now;
			string fullFileName9 = ReturnAbsoluteFileName(groupInfo, "123.txt");
			FileInfo info9 = new FileInfo(fullFileName9);
			OperateResult rename = RenameGroupFileContainer(newPath: Path.Combine(path2: HslHelper.PathCombine(fileNames[0].Split('/', '\\')), path1: filesDirectoryPath), filePath: info9.DirectoryName);
			if (!rename.IsSuccess)
			{
				base.LogNet?.WriteInfo(ToString(), session.ToString() + " RenameFolder : " + HslHelper.PathCombine(groupInfo) + " -> " + fileNames[0] + " failed: " + rename.Message);
				session.SendMqttCommand(0, null, Encoding.UTF8.GetBytes("File path already exist"));
			}
			else
			{
				await session.SendMqttCommandAsync(111, null, null);
				this.OnFileChangedEvent?.Invoke(session, new MqttFileOperateInfo
				{
					Groups = HslHelper.PathCombine(groupInfo),
					FileNames = null,
					Operate = "RenameFolder",
					TimeCost = DateTime.Now - dateTimeStart10
				});
				base.LogNet?.WriteInfo(ToString(), session.ToString() + " RenameFolder : " + HslHelper.PathCombine(groupInfo) + " -> " + fileNames[0]);
			}
		}
		else if (receiveFileNames.Content1 == 105)
		{
			GroupFileContainer fileManagment10 = GetGroupFromFilePath(ReturnAbsoluteFilePath(groupInfo));
			await session.SendMqttCommandAsync(104, null, Encoding.UTF8.GetBytes(fileManagment10.JsonArrayContent));
		}
		else if (receiveFileNames.Content1 == 108)
		{
			GroupFileContainer fileManagment11 = GetGroupFromFilePath(ReturnAbsoluteFilePath(groupInfo));
			await session.SendMqttCommandAsync(108, null, Encoding.UTF8.GetBytes(fileManagment11.GetGroupFileInfo(withLastFileInfo: true).ToJsonString()));
		}
		else if (receiveFileNames.Content1 == 109)
		{
			bool withLastFileInfo = fileNames != null && fileNames.Length != 0 && fileNames[0] == "1";
			List<GroupFileInfo> folders2 = new List<GroupFileInfo>();
			string[] directories = GetDirectories(groupInfo);
			string[] array3 = directories;
			foreach (string l2 in array3)
			{
				DirectoryInfo directory2 = new DirectoryInfo(l2);
				GroupFileContainer fileManagment12 = GetGroupFromFilePath(ReturnAbsoluteFilePath(new List<string>(groupInfo) { directory2.Name }.ToArray()));
				GroupFileInfo groupFileInfo = fileManagment12.GetGroupFileInfo(withLastFileInfo);
				groupFileInfo.PathName = directory2.Name;
				folders2.Add(groupFileInfo);
			}
			await session.SendMqttCommandAsync(108, null, Encoding.UTF8.GetBytes(folders2.ToJsonString()));
		}
		else if (receiveFileNames.Content1 == 106)
		{
			List<string> folders3 = new List<string>();
			string[] directories2 = GetDirectories(groupInfo);
			string[] array4 = directories2;
			foreach (string k2 in array4)
			{
				DirectoryInfo directory3 = new DirectoryInfo(k2);
				folders3.Add(directory3.Name);
			}
			JArray jArray = JArray.FromObject(folders3.ToArray());
			await session.SendMqttCommandAsync(106, null, Encoding.UTF8.GetBytes(jArray.ToString()));
		}
		else if (receiveFileNames.Content1 == 107)
		{
			string fileName4 = fileNames[0];
			string fullPath = ReturnAbsoluteFilePath(groupInfo);
			GroupFileContainer fileManagment13 = GetGroupFromFilePath(fullPath);
			bool isExists = fileManagment13.FileExists(fileName4);
			await session.SendMqttCommandAsync((byte)(isExists ? 1u : 0u), null, Encoding.UTF8.GetBytes(StringResources.Language.FileNotExist));
		}
	}

	private async Task<OperateResult<FileBaseInfo>> ReceiveMqttFileAndUpdateGroupAsync(MqttSession session, FileInfo info, string guidName, Action<long, long> reportProgress)
	{
		string fileName = Path.Combine(info.DirectoryName, guidName);
		OperateResult<FileBaseInfo> receive = await MqttHelper.ReceiveMqttFileAsync(session.MqttPipe, fileName, reportProgress, session.IsAesCryptography ? session.AesCryptography : null);
		if (!receive.IsSuccess)
		{
			DeleteFileByName(fileName);
			return receive;
		}
		GroupFileContainer fileManagment = GetGroupFromFilePath(info.DirectoryName);
		DeleteExsistingFile(fileName: fileManagment.UpdateFileMappingName(info.Name, receive.Content.Size, guidName, session.UserName, receive.Content.Tag), path: info.DirectoryName);
		OperateResult sendBack = await session.SendMqttCommandAsync(100, null, Encoding.UTF8.GetBytes(StringResources.Language.SuccessText));
		if (!sendBack.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(sendBack);
		}
		return OperateResult.CreateSuccessResult(receive.Content);
	}

	private string GetRelativeFileName(string[] groups, string fileName)
	{
		string path = "";
		for (int i = 0; i < groups.Length; i++)
		{
			if (!string.IsNullOrEmpty(groups[i]))
			{
				path = Path.Combine(path, groups[i]);
			}
		}
		return Path.Combine(path, fileName);
	}

	private string ReturnAbsoluteFilePath(string[] groups)
	{
		return Path.Combine(filesDirectoryPath, Path.Combine(groups));
	}

	protected string ReturnAbsoluteFileName(string[] groups, string fileName)
	{
		return Path.Combine(ReturnAbsoluteFilePath(groups), fileName);
	}

	private string TransformFactFileName(string[] groups, string fileName)
	{
		string filePath = ReturnAbsoluteFilePath(groups);
		GroupFileContainer groupFromFilePath = GetGroupFromFilePath(filePath);
		return groupFromFilePath.GetCurrentFileMappingName(fileName);
	}

	private GroupFileContainer GetGroupFromFilePath(string filePath, bool create = true)
	{
		GroupFileContainer groupFileContainer = null;
		filePath = filePath.ToUpper();
		group_marks_lock.Enter();
		if (m_dictionary_group_marks.ContainsKey(filePath))
		{
			groupFileContainer = m_dictionary_group_marks[filePath];
		}
		else if (create)
		{
			groupFileContainer = new GroupFileContainer(base.LogNet, filePath);
			m_dictionary_group_marks.Add(filePath, groupFileContainer);
		}
		group_marks_lock.Leave();
		return groupFileContainer;
	}

	private void DeleteGroupFileContainer(string filePath)
	{
		filePath = filePath.ToUpper();
		group_marks_lock.Enter();
		if (m_dictionary_group_marks.ContainsKey(filePath))
		{
			GroupFileContainer groupFileContainer = m_dictionary_group_marks[filePath];
			groupFileContainer.DeleteFolder();
			m_dictionary_group_marks.Remove(filePath);
		}
		group_marks_lock.Leave();
	}

	private OperateResult RenameGroupFileContainer(string filePath, string newPath)
	{
		filePath = filePath.ToUpper();
		string text = newPath.ToUpper();
		OperateResult result = OperateResult.CreateSuccessResult();
		group_marks_lock.Enter();
		if (m_dictionary_group_marks.ContainsKey(text) || Directory.Exists(text))
		{
			result = new OperateResult("修改的目录名称已经存在！");
		}
		else if (m_dictionary_group_marks.ContainsKey(filePath))
		{
			GroupFileContainer groupFileContainer = m_dictionary_group_marks[filePath];
			groupFileContainer.RenameFolder(newPath);
			m_dictionary_group_marks.Remove(filePath);
			m_dictionary_group_marks.Add(text, groupFileContainer);
		}
		group_marks_lock.Leave();
		return result;
	}

	public GroupFileContainer GetGroupFromFilePath(string[] groups)
	{
		return GetGroupFromFilePath(ReturnAbsoluteFilePath(groups), create: false);
	}

	private string[] GetDirectories(string[] groups)
	{
		if (string.IsNullOrEmpty(filesDirectoryPath))
		{
			return new string[0];
		}
		string path = ReturnAbsoluteFilePath(groups);
		if (!Directory.Exists(path))
		{
			return new string[0];
		}
		return Directory.GetDirectories(path);
	}

	private FileMarkId GetFileMarksFromDictionaryWithFileName(string fileName)
	{
		FileMarkId fileMarkId;
		lock (dictHybirdLock)
		{
			if (dictionaryFilesMarks.ContainsKey(fileName))
			{
				fileMarkId = dictionaryFilesMarks[fileName];
			}
			else
			{
				fileMarkId = new FileMarkId(base.LogNet, fileName);
				dictionaryFilesMarks.Add(fileName, fileMarkId);
			}
		}
		return fileMarkId;
	}

	private void CheckFolderAndCreate()
	{
		if (!Directory.Exists(filesDirectoryPath))
		{
			Directory.CreateDirectory(filesDirectoryPath);
		}
	}

	private void DeleteExsistingFile(string path, string fileName)
	{
		DeleteExsistingFile(path, new List<string> { fileName });
	}

	private void DeleteExsistingFile(string path, List<string> fileNames)
	{
		foreach (string fileName in fileNames)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				continue;
			}
			string fileUltimatePath = Path.Combine(path, fileName);
			FileMarkId fileMarksFromDictionaryWithFileName = GetFileMarksFromDictionaryWithFileName(fileName);
			fileMarksFromDictionaryWithFileName.AddOperation(delegate
			{
				if (!DeleteFileByName(fileUltimatePath))
				{
					base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteFailed + fileUltimatePath);
				}
				else
				{
					base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteSuccess + fileUltimatePath);
				}
			});
		}
	}

	private void AddMqttSession(MqttSession session)
	{
		lock (sessionsLock)
		{
			mqttSessions.Add(session);
		}
		base.LogNet?.WriteDebug(ToString(), $"{session} Online");
	}

	public void RemoveAndCloseSession(MqttSession session, string reason)
	{
		bool flag = false;
		lock (sessionsLock)
		{
			flag = mqttSessions.Remove(session);
		}
		if (!flag)
		{
			return;
		}
		NetSupport.CloseSocket(session.MqttPipe.Socket);
		base.LogNet?.WriteDebug(ToString(), $"{session} Offline {reason}");
		if (session.Protocol == "MQTT")
		{
			this.OnClientDisConnected?.Invoke(session);
			if (!string.IsNullOrEmpty(reason) && !string.IsNullOrEmpty(session.WillTopic))
			{
				PublishTopicPayload(session.WillTopic, session.WillMessage);
			}
		}
	}

	public void DeleteRetainTopic(string topic)
	{
		lock (keysLock)
		{
			if (retainKeys.ContainsKey(topic))
			{
				retainKeys.Remove(topic);
			}
		}
	}

	public string[] GetAllRetainTopics()
	{
		string[] result = null;
		lock (keysLock)
		{
			result = retainKeys.Select((KeyValuePair<string, MqttClientApplicationMessage> m) => m.Key).ToArray();
		}
		return result;
	}

	public MqttSession[] GetMqttSessionsByTopic(string topic)
	{
		MqttSession[] result = null;
		lock (sessionsLock)
		{
			result = mqttSessions.Where((MqttSession m) => m.Protocol == "MQTT" && m.IsClientSubscribe(topic, topicWildcard)).ToArray();
		}
		return result;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				timerHeart?.Dispose();
				group_marks_lock?.Dispose();
				this.ClientVerification = null;
				this.FileOperateVerification = null;
				this.OnClientApplicationMessageReceive = null;
				this.OnClientConnected = null;
				this.OnClientDisConnected = null;
				this.OnFileChangedEvent = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected bool DeleteFileByName(string fileName)
	{
		try
		{
			if (!File.Exists(fileName))
			{
				return true;
			}
			File.Delete(fileName);
			return true;
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), "delete file [" + fileName + "] failed: ", ex);
			return false;
		}
	}

	public override string ToString()
	{
		return $"MqttServer[{base.Port}]";
	}
}
