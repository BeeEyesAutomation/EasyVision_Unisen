using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Core.Security;

namespace HslCommunication.MQTT;

public class MqttClient : NetworkXBase, IDisposable
{
	public delegate void MqttMessageReceiveDelegate(MqttClient client, MqttApplicationMessage message);

	public delegate void OnClientConnectedDelegate(MqttClient client);

	private NetworkStream networkStream;

	private SslStream sslStream = null;

	private DateTime activeTime;

	private int isReConnectServer = 0;

	private List<MqttPublishMessage> publishMessages;

	private object listLock;

	private Dictionary<string, SubscribeTopic> subscribeTopics;

	private object connectLock;

	private object subscribeLock;

	private SoftIncrementCount incrementCount;

	private bool closed = false;

	private MqttConnectionOptions connectionOptions;

	private Timer timerCheck;

	private bool disposedValue;

	private RSACryptoServiceProvider cryptoServiceProvider = null;

	private AesCryptography aesCryptography = null;

	private AsyncCallback beginReceiveCallback = null;

	public MqttConnectionOptions ConnectionOptions => connectionOptions;

	public bool UseTimerCheckDropped { get; set; } = true;

	public bool IsConnected { get; private set; } = false;

	public string[] SubcribeTopics
	{
		get
		{
			lock (subscribeLock)
			{
				return subscribeTopics.Keys.ToArray();
			}
		}
	}

	public object Tag { get; set; }

	public event MqttMessageReceiveDelegate OnMqttMessageReceived;

	public event EventHandler OnNetworkError;

	public event OnClientConnectedDelegate OnClientConnected;

	public MqttClient(MqttConnectionOptions options)
	{
		beginReceiveCallback = ReceiveAsyncCallback;
		connectionOptions = options;
		incrementCount = new SoftIncrementCount(65535L, 1L);
		listLock = new object();
		publishMessages = new List<MqttPublishMessage>();
		subscribeTopics = new Dictionary<string, SubscribeTopic>();
		activeTime = DateTime.Now;
		subscribeLock = new object();
		connectLock = new object();
	}

	public OperateResult ConnectServer()
	{
		if (connectionOptions == null)
		{
			return new OperateResult("Optines is null");
		}
		OperateResult<Socket> operateResult = CreateSocketAndConnect(connectionOptions.IpAddress, connectionOptions.Port, connectionOptions.ConnectTimeout);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		RSACryptoServiceProvider rsa = null;
		if (connectionOptions.UseRSAProvider)
		{
			cryptoServiceProvider = new RSACryptoServiceProvider();
			OperateResult operateResult2 = Send(operateResult.Content, MqttHelper.BuildMqttCommand(byte.MaxValue, null, HslSecurity.ByteEncrypt(cryptoServiceProvider.GetPEMPublicKey())).Content);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult<byte, byte[]> operateResult3 = ReceiveMqttMessage(operateResult.Content, 10000);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			try
			{
				byte[] publicKey = cryptoServiceProvider.DecryptLargeData(HslSecurity.ByteDecrypt(operateResult3.Content2));
				rsa = RSAHelper.CreateRsaProviderFromPublicKey(publicKey);
			}
			catch (Exception ex)
			{
				operateResult.Content?.Close();
				return new OperateResult("RSA check failed: " + ex.Message);
			}
		}
		OperateResult<byte[]> operateResult4 = MqttHelper.BuildConnectMqttCommand(connectionOptions, "MQTT", rsa);
		if (!operateResult4.IsSuccess)
		{
			return operateResult4;
		}
		if (!ConnectionOptions.UseSSL)
		{
			OperateResult operateResult5 = Send(operateResult.Content, operateResult4.Content);
			if (!operateResult5.IsSuccess)
			{
				return operateResult5;
			}
			OperateResult<byte, byte[]> operateResult6 = ReceiveMqttMessage(operateResult.Content, 30000);
			if (!operateResult6.IsSuccess)
			{
				return operateResult6;
			}
			OperateResult operateResult7 = MqttHelper.CheckConnectBack(operateResult6.Content1, operateResult6.Content2);
			if (!operateResult7.IsSuccess)
			{
				operateResult.Content?.Close();
				return operateResult7;
			}
			if (connectionOptions.UseRSAProvider)
			{
				string key = Encoding.UTF8.GetString(cryptoServiceProvider.Decrypt(operateResult6.Content2.RemoveBegin(2), fOAEP: false));
				aesCryptography = new AesCryptography(key);
			}
		}
		else
		{
			OperateResult<SslStream> operateResult8 = CreateSslStream(operateResult.Content, createNew: true);
			if (!operateResult8.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult8);
			}
			OperateResult operateResult9 = Send(operateResult8.Content, operateResult4.Content);
			if (!operateResult9.IsSuccess)
			{
				return operateResult9;
			}
			OperateResult<byte, byte[]> operateResult10 = ReceiveMqttMessage(operateResult8.Content, 30000);
			if (!operateResult10.IsSuccess)
			{
				return operateResult10;
			}
			OperateResult operateResult11 = MqttHelper.CheckConnectBack(operateResult10.Content1, operateResult10.Content2);
			if (!operateResult11.IsSuccess)
			{
				operateResult.Content?.Close();
				return operateResult11;
			}
		}
		incrementCount.ResetCurrentValue();
		closed = false;
		try
		{
			operateResult.Content.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, operateResult.Content);
		}
		catch (Exception ex2)
		{
			return new OperateResult(ex2.Message);
		}
		CoreSocket?.Close();
		CoreSocket = operateResult.Content;
		IsConnected = true;
		this.OnClientConnected?.Invoke(this);
		timerCheck?.Dispose();
		activeTime = DateTime.Now;
		if (UseTimerCheckDropped && (int)connectionOptions.KeepAliveSendInterval.TotalMilliseconds > 0)
		{
			timerCheck = new Timer(TimerCheckServer, null, 2000, (int)connectionOptions.KeepAliveSendInterval.TotalMilliseconds);
		}
		return OperateResult.CreateSuccessResult();
	}

	private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		if (sslPolicyErrors == SslPolicyErrors.None)
		{
			return true;
		}
		return !connectionOptions.SSLSecure;
	}

	public void ConnectClose()
	{
		lock (connectLock)
		{
			closed = true;
			IsConnected = false;
		}
		OperateResult<byte[]> operateResult = MqttHelper.BuildMqttCommand(14, 0, null, null);
		if (operateResult.IsSuccess)
		{
			SendMqttBytes(operateResult.Content);
		}
		timerCheck?.Dispose();
		HslHelper.ThreadSleep(20);
		CoreSocket?.Close();
	}

	public async Task<OperateResult> ConnectServerAsync()
	{
		if (connectionOptions == null)
		{
			return new OperateResult("Optines is null");
		}
		OperateResult<Socket> connect = await CreateSocketAndConnectAsync(connectionOptions.IpAddress, connectionOptions.Port, connectionOptions.ConnectTimeout);
		if (!connect.IsSuccess)
		{
			return connect;
		}
		RSACryptoServiceProvider rsa = null;
		if (connectionOptions.UseRSAProvider)
		{
			cryptoServiceProvider = new RSACryptoServiceProvider();
			OperateResult sendKey = await SendAsync(connect.Content, MqttHelper.BuildMqttCommand(byte.MaxValue, null, HslSecurity.ByteEncrypt(cryptoServiceProvider.GetPEMPublicKey())).Content);
			if (!sendKey.IsSuccess)
			{
				return sendKey;
			}
			OperateResult<byte, byte[]> key = await ReceiveMqttMessageAsync(connect.Content, 10000);
			if (!key.IsSuccess)
			{
				return key;
			}
			try
			{
				byte[] serverPublicToken = cryptoServiceProvider.DecryptLargeData(HslSecurity.ByteDecrypt(key.Content2));
				rsa = RSAHelper.CreateRsaProviderFromPublicKey(serverPublicToken);
			}
			catch (Exception ex)
			{
				connect.Content?.Close();
				return new OperateResult("RSA check failed: " + ex.Message);
			}
		}
		OperateResult<byte[]> command = MqttHelper.BuildConnectMqttCommand(connectionOptions, "MQTT", rsa);
		if (!command.IsSuccess)
		{
			return command;
		}
		if (!ConnectionOptions.UseSSL)
		{
			OperateResult send2 = await SendAsync(connect.Content, command.Content);
			if (!send2.IsSuccess)
			{
				return send2;
			}
			OperateResult<byte, byte[]> receive2 = await ReceiveMqttMessageAsync(connect.Content, 30000);
			if (!receive2.IsSuccess)
			{
				return receive2;
			}
			OperateResult check2 = MqttHelper.CheckConnectBack(receive2.Content1, receive2.Content2);
			if (!check2.IsSuccess)
			{
				connect.Content?.Close();
				return check2;
			}
			if (connectionOptions.UseRSAProvider)
			{
				string key2 = Encoding.UTF8.GetString(cryptoServiceProvider.Decrypt(receive2.Content2.RemoveBegin(2), fOAEP: false));
				aesCryptography = new AesCryptography(key2);
			}
		}
		else
		{
			OperateResult<SslStream> ssl = CreateSslStream(connect.Content, createNew: true);
			if (!ssl.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(ssl);
			}
			OperateResult send3 = await SendAsync(ssl.Content, command.Content);
			if (!send3.IsSuccess)
			{
				return send3;
			}
			OperateResult<byte, byte[]> receive3 = await ReceiveMqttMessageAsync(ssl.Content, 30000);
			if (!receive3.IsSuccess)
			{
				return receive3;
			}
			OperateResult check3 = MqttHelper.CheckConnectBack(receive3.Content1, receive3.Content2);
			if (!check3.IsSuccess)
			{
				connect.Content?.Close();
				return check3;
			}
		}
		incrementCount.ResetCurrentValue();
		closed = false;
		try
		{
			connect.Content.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, connect.Content);
		}
		catch (Exception ex2)
		{
			return new OperateResult(ex2.Message);
		}
		CoreSocket?.Close();
		CoreSocket = connect.Content;
		IsConnected = true;
		this.OnClientConnected?.Invoke(this);
		timerCheck?.Dispose();
		activeTime = DateTime.Now;
		if (UseTimerCheckDropped && (int)connectionOptions.KeepAliveSendInterval.TotalMilliseconds > 0)
		{
			timerCheck = new Timer(TimerCheckServer, null, 2000, (int)connectionOptions.KeepAliveSendInterval.TotalMilliseconds);
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task ConnectCloseAsync()
	{
		lock (connectLock)
		{
			closed = true;
			IsConnected = true;
		}
		OperateResult<byte[]> command = MqttHelper.BuildMqttCommand(14, 0, null, null);
		if (command.IsSuccess)
		{
			await SendMqttBytesAsync(command.Content);
		}
		timerCheck?.Dispose();
		HslHelper.ThreadSleep(20);
		CoreSocket?.Close();
	}

	public OperateResult PublishMessage(MqttApplicationMessage message)
	{
		MqttPublishMessage mqttPublishMessage = new MqttPublishMessage
		{
			Identifier = (int)((message.QualityOfServiceLevel != MqttQualityOfServiceLevel.AtMostOnce) ? incrementCount.GetCurrentValue() : 0),
			Message = message
		};
		OperateResult<byte[]> operateResult = MqttHelper.BuildPublishMqttCommand(mqttPublishMessage, aesCryptography);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (message.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtMostOnce)
		{
			return SendMqttBytes(operateResult.Content);
		}
		AddPublishMessage(mqttPublishMessage);
		return SendMqttBytes(operateResult.Content);
	}

	public async Task<OperateResult> PublishMessageAsync(MqttApplicationMessage message)
	{
		MqttPublishMessage publishMessage = new MqttPublishMessage
		{
			Identifier = (int)((message.QualityOfServiceLevel != MqttQualityOfServiceLevel.AtMostOnce) ? incrementCount.GetCurrentValue() : 0),
			Message = message
		};
		OperateResult<byte[]> command = MqttHelper.BuildPublishMqttCommand(publishMessage, aesCryptography);
		if (!command.IsSuccess)
		{
			return command;
		}
		if (message.QualityOfServiceLevel == MqttQualityOfServiceLevel.AtMostOnce)
		{
			return await SendMqttBytesAsync(command.Content);
		}
		AddPublishMessage(publishMessage);
		return await SendMqttBytesAsync(command.Content);
	}

	public OperateResult SubscribeMessage(string topic)
	{
		return SubscribeMessage(new string[1] { topic });
	}

	public OperateResult SubscribeMessage(string[] topics)
	{
		MqttSubscribeMessage subcribeMessage = new MqttSubscribeMessage
		{
			Identifier = (int)incrementCount.GetCurrentValue(),
			Topics = topics
		};
		return SubscribeMessage(subcribeMessage);
	}

	public OperateResult SubscribeMessage(MqttSubscribeMessage subcribeMessage)
	{
		if (subcribeMessage.Topics == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (subcribeMessage.Topics.Length == 0)
		{
			return OperateResult.CreateSuccessResult();
		}
		OperateResult<byte[]> operateResult = MqttHelper.BuildSubscribeMqttCommand(subcribeMessage);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendMqttBytes(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		AddSubTopics(subcribeMessage.Topics);
		return OperateResult.CreateSuccessResult();
	}

	private void AddSubTopics(string[] topics)
	{
		lock (subscribeLock)
		{
			for (int i = 0; i < topics.Length; i++)
			{
				if (!subscribeTopics.ContainsKey(topics[i]))
				{
					subscribeTopics.Add(topics[i], new SubscribeTopic(topics[i]));
				}
				subscribeTopics[topics[i]].AddSubscribeTick();
			}
		}
	}

	public SubscribeTopic GetSubscribeTopic(string topic)
	{
		SubscribeTopic result = null;
		lock (subscribeLock)
		{
			if (subscribeTopics.ContainsKey(topic))
			{
				result = subscribeTopics[topic];
			}
		}
		return result;
	}

	public OperateResult UnSubscribeMessage(string[] topics)
	{
		MqttSubscribeMessage message = new MqttSubscribeMessage
		{
			Identifier = (int)incrementCount.GetCurrentValue(),
			Topics = topics
		};
		OperateResult<byte[]> operateResult = MqttHelper.BuildUnSubscribeMqttCommand(message);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendMqttBytes(operateResult.Content);
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

	private bool RemoveSubTopics(string[] topics)
	{
		bool result = true;
		lock (subscribeLock)
		{
			for (int i = 0; i < topics.Length; i++)
			{
				if (subscribeTopics.ContainsKey(topics[i]))
				{
					subscribeTopics.Remove(topics[i]);
				}
			}
		}
		return result;
	}

	public async Task<OperateResult> SubscribeMessageAsync(string topic)
	{
		return await SubscribeMessageAsync(new string[1] { topic });
	}

	public async Task<OperateResult> SubscribeMessageAsync(string[] topics)
	{
		if (topics == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (topics.Length == 0)
		{
			return OperateResult.CreateSuccessResult();
		}
		MqttSubscribeMessage subcribeMessage = new MqttSubscribeMessage
		{
			Identifier = (int)incrementCount.GetCurrentValue(),
			Topics = topics
		};
		OperateResult<byte[]> command = MqttHelper.BuildSubscribeMqttCommand(subcribeMessage);
		if (!command.IsSuccess)
		{
			return command;
		}
		AddSubTopics(topics);
		return await SendMqttBytesAsync(command.Content);
	}

	public async Task<OperateResult> UnSubscribeMessageAsync(string[] topics)
	{
		MqttSubscribeMessage subcribeMessage = new MqttSubscribeMessage
		{
			Identifier = (int)incrementCount.GetCurrentValue(),
			Topics = topics
		};
		OperateResult<byte[]> command = MqttHelper.BuildUnSubscribeMqttCommand(subcribeMessage);
		RemoveSubTopics(topics);
		return await SendMqttBytesAsync(command.Content);
	}

	public async Task<OperateResult> UnSubscribeMessageAsync(string topic)
	{
		return await UnSubscribeMessageAsync(new string[1] { topic });
	}

	private void OnMqttNetworkError()
	{
		if (closed)
		{
			base.LogNet?.WriteDebug(ToString(), "Closed");
		}
		else
		{
			if (Interlocked.CompareExchange(ref isReConnectServer, 1, 0) != 0)
			{
				return;
			}
			try
			{
				IsConnected = false;
				timerCheck?.Dispose();
				timerCheck = null;
				if (this.OnNetworkError == null)
				{
					base.LogNet?.WriteInfo(ToString(), "The network is abnormal, and the system is ready to automatically reconnect after 10 seconds.");
					while (true)
					{
						for (int i = 0; i < 10; i++)
						{
							HslHelper.ThreadSleep(1000);
							base.LogNet?.WriteInfo(ToString(), $"Wait for {10 - i} second to connect to the server ...");
							if (closed)
							{
								base.LogNet?.WriteDebug(ToString(), "Closed");
								Interlocked.Exchange(ref isReConnectServer, 0);
								return;
							}
						}
						lock (connectLock)
						{
							if (closed)
							{
								base.LogNet?.WriteDebug(ToString(), "Closed");
								Interlocked.Exchange(ref isReConnectServer, 0);
								return;
							}
							OperateResult operateResult = ConnectServer();
							if (operateResult.IsSuccess)
							{
								base.LogNet?.WriteInfo(ToString(), "Successfully connected to the server!");
								break;
							}
							base.LogNet?.WriteInfo(ToString(), "The connection failed. Prepare to reconnect after 10 seconds.");
							if (closed)
							{
								base.LogNet?.WriteDebug(ToString(), "Closed");
								Interlocked.Exchange(ref isReConnectServer, 0);
								return;
							}
						}
					}
				}
				else
				{
					this.OnNetworkError?.Invoke(this, new EventArgs());
				}
				Interlocked.Exchange(ref isReConnectServer, 0);
			}
			catch
			{
				Interlocked.Exchange(ref isReConnectServer, 0);
				throw;
			}
		}
	}

	private async void ReceiveAsyncCallback(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (!(asyncState is Socket socket))
		{
			return;
		}
		try
		{
			socket.EndReceive(ar);
		}
		catch (ObjectDisposedException)
		{
			socket?.Close();
			base.LogNet?.WriteDebug(ToString(), "Closed");
			return;
		}
		catch (Exception ex2)
		{
			Exception ex4 = ex2;
			Exception ex5 = ex4;
			socket?.Close();
			base.LogNet?.WriteDebug(ToString(), "ReceiveCallback Failed:" + ex5.Message);
			OnMqttNetworkError();
			return;
		}
		if (closed)
		{
			base.LogNet?.WriteDebug(ToString(), "Closed");
			return;
		}
		OperateResult<byte, byte[]> read;
		if (string.IsNullOrEmpty(connectionOptions.CertificateFile))
		{
			read = await ReceiveMqttMessageAsync(socket, 30000);
		}
		else
		{
			OperateResult<SslStream> ssl = CreateSslStream(socket);
			if (!ssl.IsSuccess)
			{
				socket?.Close();
				base.LogNet?.WriteDebug(ToString(), "CreateSslStream Failed:" + ssl.Message);
				OnMqttNetworkError();
				return;
			}
			read = await ReceiveMqttMessageAsync(ssl.Content, 30000);
		}
		if (!read.IsSuccess)
		{
			OnMqttNetworkError();
			return;
		}
		byte mqttCode = read.Content1;
		byte[] data = read.Content2;
		switch (mqttCode >> 4)
		{
		case 4:
			base.LogNet?.WriteDebug(ToString(), $"Code[{mqttCode:X2}] Publish Ack: {SoftBasic.ByteToHexString(data, ' ')}");
			break;
		case 5:
			SendMqttBytes(MqttHelper.BuildMqttCommand(6, 2, data, new byte[0]).Content);
			base.LogNet?.WriteDebug(ToString(), $"Code[{mqttCode:X2}] Publish Rec: {SoftBasic.ByteToHexString(data, ' ')}");
			break;
		case 7:
			base.LogNet?.WriteDebug(ToString(), $"Code[{mqttCode:X2}] Publish Complete: {SoftBasic.ByteToHexString(data, ' ')}");
			break;
		case 13:
			activeTime = DateTime.Now;
			base.LogNet?.WriteDebug(ToString(), "Heart Code Check!");
			break;
		case 9:
			base.LogNet?.WriteDebug(ToString(), $"Code[{mqttCode:X2}] Subscribe Ack: {SoftBasic.ByteToHexString(data, ' ')}");
			break;
		case 11:
			base.LogNet?.WriteDebug(ToString(), $"Code[{mqttCode:X2}] UnSubscribe Ack: {SoftBasic.ByteToHexString(data, ' ')}");
			break;
		case 3:
			ExtraPublishData(mqttCode, data);
			break;
		default:
			base.LogNet?.WriteDebug(ToString(), $"Code[{mqttCode:X2}] {SoftBasic.ByteToHexString(data, ' ')}");
			break;
		}
		try
		{
			socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, socket);
		}
		catch (Exception ex6)
		{
			socket?.Close();
			base.LogNet?.WriteDebug(ToString(), "BeginReceive Failed:" + ex6.Message);
			OnMqttNetworkError();
		}
	}

	private void ExtraPublishData(byte mqttCode, byte[] data)
	{
		activeTime = DateTime.Now;
		OperateResult<string, byte[]> operateResult = MqttHelper.ExtraMqttReceiveData(mqttCode, data, aesCryptography);
		if (!operateResult.IsSuccess)
		{
			base.LogNet?.WriteDebug(ToString(), operateResult.Message);
			return;
		}
		int qos = MqttHelper.ExtraQosFromMqttCode(mqttCode);
		MqttApplicationMessage mqttApplicationMessage = new MqttApplicationMessage();
		mqttApplicationMessage.Topic = operateResult.Content1;
		mqttApplicationMessage.Retain = (mqttCode & 1) == 1;
		mqttApplicationMessage.QualityOfServiceLevel = MqttHelper.GetFromQos(qos);
		mqttApplicationMessage.Payload = operateResult.Content2;
		this.OnMqttMessageReceived?.Invoke(this, mqttApplicationMessage);
		GetSubscribeTopic(operateResult.Content1)?.TriggerSubscription(this, mqttApplicationMessage);
	}

	private void TimerCheckServer(object obj)
	{
		if (CoreSocket != null)
		{
			if ((DateTime.Now - activeTime).TotalSeconds > connectionOptions.KeepAliveSendInterval.TotalSeconds * 3.0)
			{
				OnMqttNetworkError();
			}
			else if (!SendMqttBytes(MqttHelper.BuildMqttCommand(12, 0, new byte[0], new byte[0]).Content).IsSuccess)
			{
				OnMqttNetworkError();
			}
		}
	}

	private void AddPublishMessage(MqttPublishMessage publishMessage)
	{
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				incrementCount?.Dispose();
				timerCheck?.Dispose();
				this.OnClientConnected = null;
				this.OnMqttMessageReceived = null;
				this.OnNetworkError = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private OperateResult<SslStream> CreateSslStream(Socket socket, bool createNew = false)
	{
		if (createNew)
		{
			networkStream?.Close();
			sslStream?.Close();
			networkStream = new NetworkStream(socket, ownsSocket: false);
			sslStream = new SslStream(networkStream, leaveInnerStreamOpen: false, ValidateServerCertificate, null);
			try
			{
				if (string.IsNullOrEmpty(ConnectionOptions.CertificateFile))
				{
					sslStream.AuthenticateAsClient(connectionOptions.HostName);
				}
				else
				{
					X509CertificateCollection clientCertificates = new X509CertificateCollection(new X509Certificate[1] { X509Certificate.CreateFromCertFile(ConnectionOptions.CertificateFile) });
					sslStream.AuthenticateAsClient(connectionOptions.HostName, clientCertificates, SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, checkCertificateRevocation: false);
				}
				return OperateResult.CreateSuccessResult(sslStream);
			}
			catch (Exception ex)
			{
				return new OperateResult<SslStream>(ex.Message);
			}
		}
		return OperateResult.CreateSuccessResult(sslStream);
	}

	private OperateResult SendMqttBytes(byte[] data)
	{
		if (string.IsNullOrEmpty(connectionOptions.CertificateFile))
		{
			return Send(CoreSocket, data);
		}
		OperateResult<SslStream> operateResult = CreateSslStream(CoreSocket);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return Send(operateResult.Content, data);
	}

	private async Task<OperateResult> SendMqttBytesAsync(byte[] data)
	{
		if (string.IsNullOrEmpty(connectionOptions.CertificateFile))
		{
			return await SendAsync(CoreSocket, data);
		}
		OperateResult<SslStream> ssl = CreateSslStream(CoreSocket);
		if (!ssl.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(ssl);
		}
		return await SendAsync(ssl.Content, data);
	}

	public override string ToString()
	{
		return $"MqttClient[{connectionOptions.IpAddress}:{connectionOptions.Port}]";
	}
}
