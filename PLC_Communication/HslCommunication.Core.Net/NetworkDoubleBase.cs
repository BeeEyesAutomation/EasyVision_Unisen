using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Pipe;
using HslCommunication.MQTT;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Net;

public class NetworkDoubleBase : NetworkBase, IDisposable
{
	protected PipeSocket pipeSocket;

	private IByteTransform byteTransform;

	private string connectionId = string.Empty;

	private bool isUseSpecifiedSocket = false;

	private bool useServerActivePush = false;

	private AutoResetEvent autoResetEvent;

	private byte[] bufferQA = null;

	protected bool isPersistentConn = false;

	protected bool LogMsgFormatBinary = true;

	protected bool isUseAccountCertificate = false;

	private string userName = string.Empty;

	private string password = string.Empty;

	private bool disposedValue = false;

	private MqttClient mqttClient = null;

	private string writeTopic = string.Empty;

	private string readTopic = string.Empty;

	private byte[] sendbyteBefore = null;

	private string sendBefore = string.Empty;

	private Lazy<Ping> ping = new Lazy<Ping>(() => new Ping());

	protected bool UseServerActivePush
	{
		get
		{
			return useServerActivePush;
		}
		set
		{
			if (value)
			{
				if (autoResetEvent == null)
				{
					autoResetEvent = new AutoResetEvent(initialState: false);
				}
				isPersistentConn = true;
			}
			useServerActivePush = value;
		}
	}

	public IByteTransform ByteTransform
	{
		get
		{
			return byteTransform;
		}
		set
		{
			byteTransform = value;
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Gets or sets the timeout for the connection, in milliseconds")]
	public virtual int ConnectTimeOut
	{
		get
		{
			return pipeSocket.ConnectTimeOut;
		}
		set
		{
			if (value >= 0)
			{
				pipeSocket.ConnectTimeOut = value;
			}
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Gets or sets the time to receive server feedback, and if it is a negative number, does not receive feedback")]
	public int ReceiveTimeOut
	{
		get
		{
			return pipeSocket.ReceiveTimeOut;
		}
		set
		{
			pipeSocket.ReceiveTimeOut = value;
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Get or set the IP address of the remote server. If it is a local test, then it needs to be set to 127.0.0.1")]
	public virtual string IpAddress
	{
		get
		{
			return pipeSocket.IpAddress;
		}
		set
		{
			pipeSocket.IpAddress = value;
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Gets or sets the port number of the server. The specific value depends on the configuration of the other party.")]
	public virtual int Port
	{
		get
		{
			return pipeSocket.Port;
		}
		set
		{
			pipeSocket.Port = value;
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "The unique ID number of the current connection. The default is a 20-digit guid code plus a random number.")]
	public string ConnectionId
	{
		get
		{
			return connectionId;
		}
		set
		{
			connectionId = value;
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Get or set the time required to rest before officially receiving the data from the other party. When it is set to 0, no rest is required.")]
	public int SleepTime
	{
		get
		{
			return pipeSocket.SleepTime;
		}
		set
		{
			pipeSocket.SleepTime = value;
		}
	}

	public IPEndPoint LocalBinding
	{
		get
		{
			return pipeSocket.LocalBinding;
		}
		set
		{
			pipeSocket.LocalBinding = value;
		}
	}

	public AlienSession AlienSession { get; set; }

	public int SocketKeepAliveTime { get; set; } = -1;

	public string SendBeforeHex
	{
		get
		{
			return sendBefore;
		}
		set
		{
			sendBefore = value;
			if (string.IsNullOrEmpty(value))
			{
				sendbyteBefore = null;
			}
			else
			{
				sendbyteBefore = value.ToHexBytes();
			}
		}
	}

	public NetworkDoubleBase()
	{
		pipeSocket = new PipeSocket();
		connectionId = SoftBasic.GetUniqueStringByGuidAndRandom();
	}

	protected virtual INetMessage GetNewNetMessage()
	{
		return null;
	}

	public void SetPipeSocket(PipeSocket pipeSocket)
	{
		if (this.pipeSocket != null)
		{
			this.pipeSocket = pipeSocket;
			SetPersistentConnection();
		}
	}

	public PipeSocket GetPipeSocket()
	{
		return pipeSocket;
	}

	public void SetPersistentConnection()
	{
		isPersistentConn = true;
	}

	public IPStatus IpAddressPing()
	{
		return ping.Value.Send(IpAddress).Status;
	}

	public OperateResult ConnectServer()
	{
		isPersistentConn = true;
		pipeSocket.Socket?.Close();
		OperateResult<Socket> operateResult = CreateSocketAndInitialication();
		if (!operateResult.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			operateResult.Content = null;
		}
		else
		{
			pipeSocket.Socket = operateResult.Content;
			if (SocketKeepAliveTime > 0)
			{
				operateResult.Content.SetKeepAlive(SocketKeepAliveTime, SocketKeepAliveTime);
			}
			base.LogNet?.WriteDebug(ToString(), StringResources.Language.NetEngineStart);
		}
		return operateResult;
	}

	public OperateResult ConnectServer(AlienSession session)
	{
		isPersistentConn = true;
		isUseSpecifiedSocket = true;
		if (session != null)
		{
			AlienSession?.Pipe?.CloseCommunication();
			if (string.IsNullOrEmpty(ConnectionId))
			{
				ConnectionId = session.DTU;
			}
			if (ConnectionId == session.DTU)
			{
				if (session.IsStatusOk)
				{
					OperateResult operateResult = InitializationOnConnect(session.Pipe.Socket);
					if (operateResult.IsSuccess)
					{
						pipeSocket.Socket = session.Pipe.Socket;
						pipeSocket.IsSocketError = !session.IsStatusOk;
						AlienSession = session;
					}
					else
					{
						pipeSocket.IsSocketError = true;
					}
					return operateResult;
				}
				return new OperateResult();
			}
			pipeSocket.IsSocketError = true;
			return new OperateResult();
		}
		pipeSocket.IsSocketError = true;
		return new OperateResult();
	}

	public OperateResult ConnectServer(MqttClient mqttClient, string readTopic, string writeTopic)
	{
		isPersistentConn = true;
		this.writeTopic = writeTopic;
		this.readTopic = readTopic;
		pipeSocket.IsSocketError = true;
		if (mqttClient != null)
		{
			SubscribeTopic subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
			if (subscribeTopic == null)
			{
				mqttClient.OnClientConnected += MqttClient_OnClientConnected;
				mqttClient.SubscribeMessage(readTopic);
				subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
			}
			subscribeTopic.OnMqttMessageReceived += SubscribeTopic_OnMqttMessageReceived;
		}
		if (autoResetEvent == null)
		{
			autoResetEvent = new AutoResetEvent(initialState: false);
		}
		this.mqttClient = mqttClient;
		return OperateResult.CreateSuccessResult();
	}

	private void MqttClient_OnClientConnected(MqttClient client)
	{
		client.SubscribeMessage(readTopic);
	}

	private void SubscribeTopic_OnMqttMessageReceived(MqttClient client, MqttApplicationMessage message)
	{
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? message.Payload.ToHexString(' ') : SoftBasic.GetAsciiStringRender(message.Payload)));
		bufferQA = message.Payload;
		autoResetEvent.Set();
	}

	public OperateResult ConnectClose()
	{
		OperateResult operateResult = new OperateResult();
		isPersistentConn = false;
		try
		{
			operateResult = ((pipeSocket.Socket != null) ? ExtraOnDisconnect(pipeSocket.Socket) : OperateResult.CreateSuccessResult());
			if (mqttClient == null)
			{
				pipeSocket.Socket?.Close();
				pipeSocket.Socket = null;
			}
			else
			{
				SubscribeTopic subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
				if (subscribeTopic != null)
				{
					subscribeTopic.OnMqttMessageReceived -= SubscribeTopic_OnMqttMessageReceived;
				}
				mqttClient.OnClientConnected -= MqttClient_OnClientConnected;
				mqttClient.UnSubscribeMessage(readTopic);
				mqttClient = null;
			}
		}
		catch
		{
			throw;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Close);
		return operateResult;
	}

	private async void ServerSocketActivePushAsync(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (!(asyncState is Socket socket))
		{
			return;
		}
		OperateResult<int> endResult = socket.EndReceiveResult(ar);
		if (!endResult.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			return;
		}
		OperateResult<byte[]> receive = await base.ReceiveByMessageAsync(socket, ReceiveTimeOut, GetNewNetMessage()).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			return;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? receive.Content.ToHexString(' ') : SoftBasic.GetAsciiStringRender(receive.Content)));
		OperateResult receiveAgain = socket.BeginReceiveResult(ServerSocketActivePushAsync);
		if (!receiveAgain.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
		}
		if (DecideWhetherQAMessage(socket, receive))
		{
			bufferQA = receive.Content;
			autoResetEvent.Set();
		}
	}

	protected virtual bool DecideWhetherQAMessage(Socket socket, OperateResult<byte[]> receive)
	{
		return true;
	}

	protected virtual OperateResult InitializationOnConnect(Socket socket)
	{
		if (useServerActivePush)
		{
			OperateResult operateResult = socket.BeginReceiveResult(ServerSocketActivePushAsync);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected virtual OperateResult ExtraOnDisconnect(Socket socket)
	{
		return OperateResult.CreateSuccessResult();
	}

	protected virtual void ExtraAfterReadFromCoreServer(OperateResult read)
	{
	}

	public void SetLoginAccount(string userName, string password)
	{
		if (!string.IsNullOrEmpty(userName.Trim()))
		{
			isUseAccountCertificate = true;
			this.userName = userName;
			this.password = password;
		}
		else
		{
			isUseAccountCertificate = false;
		}
	}

	protected OperateResult AccountCertificate(Socket socket)
	{
		OperateResult operateResult = SendAccountAndCheckReceive(socket, 1, userName, password);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<int, string[]> operateResult2 = ReceiveStringArrayContentFromSocket(socket);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if (operateResult2.Content1 == 0)
		{
			return new OperateResult(operateResult2.Content2[0]);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected async Task<OperateResult> AccountCertificateAsync(Socket socket)
	{
		OperateResult send = await SendAccountAndCheckReceiveAsync(socket, 1, userName, password).ConfigureAwait(continueOnCapturedContext: false);
		if (!send.IsSuccess)
		{
			return send;
		}
		OperateResult<int, string[]> read = await ReceiveStringArrayContentFromSocketAsync(socket).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		if (read.Content1 == 0)
		{
			return new OperateResult(read.Content2[0]);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected virtual async Task<OperateResult> InitializationOnConnectAsync(Socket socket)
	{
		if (useServerActivePush)
		{
			OperateResult receive = socket.BeginReceiveResult(ServerSocketActivePushAsync);
			if (!receive.IsSuccess)
			{
				return receive;
			}
		}
		return await Task.FromResult(OperateResult.CreateSuccessResult()).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected virtual async Task<OperateResult> ExtraOnDisconnectAsync(Socket socket)
	{
		return await Task.FromResult(OperateResult.CreateSuccessResult());
	}

	private async Task<OperateResult<Socket>> CreateSocketAndInitialicationAsync()
	{
		OperateResult<Socket> operateResult = ((mqttClient == null) ? (await CreateSocketAndConnectAsync(pipeSocket.GetConnectIPEndPoint(), ConnectTimeOut, LocalBinding).ConfigureAwait(continueOnCapturedContext: false)) : OperateResult.CreateSuccessResult(pipeSocket.Socket));
		OperateResult<Socket> operateResult2 = operateResult;
		OperateResult<Socket> result = operateResult2;
		if (result.IsSuccess)
		{
			OperateResult initi = await InitializationOnConnectAsync(result.Content).ConfigureAwait(continueOnCapturedContext: false);
			if (!initi.IsSuccess)
			{
				result.Content?.Close();
				result.IsSuccess = initi.IsSuccess;
				result.CopyErrorFromOther(initi);
			}
		}
		return result;
	}

	protected async Task<OperateResult<Socket>> GetAvailableSocketAsync()
	{
		if (isPersistentConn)
		{
			if (isUseSpecifiedSocket)
			{
				if (pipeSocket.IsSocketError)
				{
					return new OperateResult<Socket>(StringResources.Language.ConnectionIsNotAvailable);
				}
				return OperateResult.CreateSuccessResult(pipeSocket.Socket);
			}
			if ((mqttClient == null) ? pipeSocket.IsConnectitonError() : pipeSocket.IsSocketError)
			{
				OperateResult connect = await ConnectServerAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (!connect.IsSuccess)
				{
					pipeSocket.IsSocketError = true;
					return OperateResult.CreateFailedResult<Socket>(connect);
				}
				pipeSocket.IsSocketError = false;
				return OperateResult.CreateSuccessResult(pipeSocket.Socket);
			}
			return OperateResult.CreateSuccessResult(pipeSocket.Socket);
		}
		return await CreateSocketAndInitialicationAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult> ConnectServerAsync()
	{
		isPersistentConn = true;
		pipeSocket.Socket?.Close();
		OperateResult<Socket> rSocket = await CreateSocketAndInitialicationAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (!rSocket.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			rSocket.Content = null;
			return rSocket;
		}
		pipeSocket.Socket = rSocket.Content;
		if (SocketKeepAliveTime > 0)
		{
			rSocket.Content.SetKeepAlive(SocketKeepAliveTime, SocketKeepAliveTime);
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.ConnectedSuccess);
		return rSocket;
	}

	public async Task<OperateResult> ConnectCloseAsync()
	{
		new OperateResult();
		isPersistentConn = false;
		OperateResult result;
		try
		{
			result = await ExtraOnDisconnectAsync(pipeSocket.Socket).ConfigureAwait(continueOnCapturedContext: false);
			if (mqttClient == null)
			{
				pipeSocket.Socket?.Close();
				pipeSocket.Socket = null;
			}
			else
			{
				SubscribeTopic subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
				if (subscribeTopic != null)
				{
					subscribeTopic.OnMqttMessageReceived -= SubscribeTopic_OnMqttMessageReceived;
				}
				mqttClient.OnClientConnected -= MqttClient_OnClientConnected;
				mqttClient.UnSubscribeMessage(readTopic);
				mqttClient = null;
			}
		}
		catch
		{
			throw;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Close);
		return result;
	}

	public virtual async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(Socket socket, byte[] send, bool hasResponseData = true, bool usePackAndUnpack = true)
	{
		byte[] sendValue = (usePackAndUnpack ? PackCommandWithHeader(send) : send);
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + (LogMsgFormatBinary ? sendValue.ToHexString(' ') : SoftBasic.GetAsciiStringRender(sendValue)));
		INetMessage netMessage = GetNewNetMessage();
		if (netMessage != null)
		{
			netMessage.SendBytes = sendValue;
		}
		if (sendbyteBefore != null)
		{
			await SendAsync(socket, sendbyteBefore).ConfigureAwait(continueOnCapturedContext: false);
		}
		OperateResult operateResult = ((mqttClient == null) ? (await SendAsync(socket, sendValue).ConfigureAwait(continueOnCapturedContext: false)) : mqttClient.PublishMessage(new MqttApplicationMessage
		{
			Topic = writeTopic,
			Payload = sendValue
		}));
		OperateResult operateResult2 = operateResult;
		OperateResult sendResult = operateResult2;
		if (!sendResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(sendResult);
		}
		if (ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (!hasResponseData)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (SleepTime > 0)
		{
			HslHelper.ThreadSleep(SleepTime);
		}
		OperateResult<byte[]> resultReceive;
		if (useServerActivePush)
		{
			if (!(await Task.Run(() => autoResetEvent.WaitOne(ReceiveTimeOut)).ConfigureAwait(continueOnCapturedContext: false)))
			{
				NetSupport.CloseSocket(socket);
				pipeSocket.IsSocketError = true;
				return new OperateResult<byte[]>(-10000, StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut);
			}
			netMessage.HeadBytes = bufferQA;
			resultReceive = OperateResult.CreateSuccessResult(bufferQA);
		}
		else if (mqttClient != null)
		{
			MemoryStream ms = new MemoryStream();
			while (true)
			{
				if (await Task.Run(() => autoResetEvent.WaitOne(ReceiveTimeOut)).ConfigureAwait(continueOnCapturedContext: false))
				{
					byte[] array = bufferQA;
					if (array != null && array.Length != 0)
					{
						ms.Write(bufferQA);
					}
					if (netMessage == null)
					{
						break;
					}
					if (ms.Length < netMessage.ProtocolHeadBytesLength)
					{
						continue;
					}
					if (netMessage.HeadBytes == null)
					{
						byte[] head = ms.ToArray();
						int start = netMessage.PependedUselesByteLength(head);
						if (start > 0)
						{
							ms = new MemoryStream();
							ms.Write(head.RemoveBegin(start));
							if (ms.Length < netMessage.ProtocolHeadBytesLength)
							{
								continue;
							}
						}
						netMessage.HeadBytes = ms.ToArray().SelectBegin(netMessage.ProtocolHeadBytesLength);
					}
					int contentLength = netMessage.GetContentLengthByHeadBytes();
					if (ms.Length < netMessage.ProtocolHeadBytesLength + contentLength)
					{
						continue;
					}
					break;
				}
				return new OperateResult<byte[]>(-10000, StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut);
			}
			resultReceive = OperateResult.CreateSuccessResult(ms.ToArray());
		}
		else if (netMessage == null)
		{
			DateTime startTime = DateTime.Now;
			MemoryStream ms2 = new MemoryStream();
			while (true)
			{
				OperateResult<byte[]> read = await ReceiveByMessageAsync(socket, ReceiveTimeOut, netMessage).ConfigureAwait(continueOnCapturedContext: false);
				if (!read.IsSuccess)
				{
					return read;
				}
				ms2.Write(read.Content);
				if (CheckReceiveDataComplete(sendValue, ms2))
				{
					resultReceive = OperateResult.CreateSuccessResult(ms2.ToArray());
					break;
				}
				if (ReceiveTimeOut > 0 && (DateTime.Now - startTime).TotalMilliseconds > (double)ReceiveTimeOut)
				{
					resultReceive = new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut);
					break;
				}
			}
		}
		else
		{
			resultReceive = await ReceiveByMessageAsync(socket, ReceiveTimeOut, netMessage).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (!resultReceive.IsSuccess)
		{
			return resultReceive;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? resultReceive.Content.ToHexString(' ') : SoftBasic.GetAsciiStringRender(resultReceive.Content)));
		if (netMessage != null && !netMessage.CheckHeadBytesLegal(base.Token.ToByteArray()))
		{
			if (mqttClient == null)
			{
				NetSupport.CloseSocket(socket);
			}
			return new OperateResult<byte[]>(StringResources.Language.CommandHeadCodeCheckFailed + Environment.NewLine + StringResources.Language.Send + ": " + SoftBasic.ByteToHexString(sendValue, ' ') + Environment.NewLine + StringResources.Language.Receive + ": " + SoftBasic.ByteToHexString(resultReceive.Content, ' '));
		}
		return usePackAndUnpack ? UnpackResponseContent(sendValue, resultReceive.Content) : resultReceive;
	}

	public virtual async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(byte[] send)
	{
		return await ReadFromCoreServerAsync(send, hasResponseData: true).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(IEnumerable<byte[]> send)
	{
		return await NetSupport.ReadFromCoreServerAsync(send, ReadFromCoreServerAsync).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(byte[] send, bool hasResponseData, bool usePackAndUnpack = true)
	{
		if (pipeSocket.LockingTick > HslHelper.LockLimit)
		{
			return new OperateResult<byte[]>(StringResources.Language.TooManyLock);
		}
		OperateResult<byte[]> result = new OperateResult<byte[]>();
		if (HslHelper.UseAsyncLock)
		{
			await Task.Run(delegate
			{
				pipeSocket.PipeLockEnter();
			}).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			pipeSocket.PipeLockEnter();
		}
		OperateResult<Socket> resultSocket;
		try
		{
			resultSocket = await GetAvailableSocketAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (!resultSocket.IsSuccess)
			{
				pipeSocket.IsSocketError = true;
				AlienSession?.Offline();
				pipeSocket.PipeLockLeave();
				result.CopyErrorFromOther(resultSocket);
				return result;
			}
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(resultSocket.Content, send, hasResponseData, usePackAndUnpack).ConfigureAwait(continueOnCapturedContext: false);
			if (read.IsSuccess)
			{
				pipeSocket.IsSocketError = false;
				result.IsSuccess = read.IsSuccess;
				result.Content = read.Content;
				result.Message = StringResources.Language.SuccessText;
			}
			else
			{
				if (read.ErrorCode != int.MinValue)
				{
					pipeSocket.IsSocketError = true;
					AlienSession?.Offline();
				}
				else
				{
					read.ErrorCode = 10000;
				}
				result.CopyErrorFromOther(read);
			}
			ExtraAfterReadFromCoreServer(read);
			pipeSocket.PipeLockLeave();
		}
		catch
		{
			pipeSocket.PipeLockLeave();
			throw;
		}
		if (!isPersistentConn)
		{
			resultSocket?.Content?.Close();
		}
		return result;
	}

	protected virtual bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return true;
	}

	public virtual byte[] PackCommandWithHeader(byte[] command)
	{
		return command;
	}

	public virtual OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return OperateResult.CreateSuccessResult(response);
	}

	protected OperateResult<Socket> GetAvailableSocket()
	{
		if (isPersistentConn)
		{
			if (isUseSpecifiedSocket)
			{
				if (pipeSocket.IsSocketError)
				{
					return new OperateResult<Socket>(StringResources.Language.ConnectionIsNotAvailable);
				}
				return OperateResult.CreateSuccessResult(pipeSocket.Socket);
			}
			if ((mqttClient == null) ? pipeSocket.IsConnectitonError() : pipeSocket.IsSocketError)
			{
				OperateResult operateResult = ConnectServer();
				if (!operateResult.IsSuccess)
				{
					pipeSocket.IsSocketError = true;
					return OperateResult.CreateFailedResult<Socket>(operateResult);
				}
				pipeSocket.IsSocketError = false;
				return OperateResult.CreateSuccessResult(pipeSocket.Socket);
			}
			return OperateResult.CreateSuccessResult(pipeSocket.Socket);
		}
		return CreateSocketAndInitialication();
	}

	private OperateResult<Socket> CreateSocketAndInitialication()
	{
		OperateResult<Socket> operateResult = ((mqttClient == null) ? CreateSocketAndConnect(pipeSocket.GetConnectIPEndPoint(), ConnectTimeOut, LocalBinding) : OperateResult.CreateSuccessResult(pipeSocket.Socket));
		if (operateResult.IsSuccess)
		{
			OperateResult operateResult2 = InitializationOnConnect(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				operateResult.Content?.Close();
				operateResult.IsSuccess = operateResult2.IsSuccess;
				operateResult.CopyErrorFromOther(operateResult2);
			}
		}
		return operateResult;
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(Socket socket, byte[] send, bool hasResponseData = true, bool usePackAndUnpack = true)
	{
		byte[] array = (usePackAndUnpack ? PackCommandWithHeader(send) : send);
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + (LogMsgFormatBinary ? array.ToHexString(' ') : SoftBasic.GetAsciiStringRender(array)));
		INetMessage newNetMessage = GetNewNetMessage();
		if (newNetMessage != null)
		{
			newNetMessage.SendBytes = array;
		}
		if (sendbyteBefore != null)
		{
			Send(socket, sendbyteBefore);
		}
		OperateResult operateResult = ((mqttClient == null) ? Send(socket, array) : mqttClient.PublishMessage(new MqttApplicationMessage
		{
			Topic = writeTopic,
			Payload = array
		}));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (!hasResponseData)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (SleepTime > 0)
		{
			HslHelper.ThreadSleep(SleepTime);
		}
		OperateResult<byte[]> operateResult2 = null;
		if (useServerActivePush)
		{
			if (!autoResetEvent.WaitOne(ReceiveTimeOut))
			{
				NetSupport.CloseSocket(socket);
				pipeSocket.IsSocketError = true;
				return new OperateResult<byte[]>(-10000, StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut);
			}
			newNetMessage.HeadBytes = bufferQA;
			operateResult2 = OperateResult.CreateSuccessResult(bufferQA);
		}
		else if (mqttClient != null)
		{
			MemoryStream memoryStream = new MemoryStream();
			while (true)
			{
				if (autoResetEvent.WaitOne(ReceiveTimeOut))
				{
					byte[] array2 = bufferQA;
					if (array2 != null && array2.Length != 0)
					{
						memoryStream.Write(bufferQA);
					}
					if (newNetMessage == null)
					{
						break;
					}
					if (memoryStream.Length < newNetMessage.ProtocolHeadBytesLength)
					{
						continue;
					}
					if (newNetMessage.HeadBytes == null)
					{
						byte[] array3 = memoryStream.ToArray();
						int num = newNetMessage.PependedUselesByteLength(array3);
						if (num > 0)
						{
							memoryStream = new MemoryStream();
							memoryStream.Write(array3.RemoveBegin(num));
							if (memoryStream.Length < newNetMessage.ProtocolHeadBytesLength)
							{
								continue;
							}
						}
						newNetMessage.HeadBytes = memoryStream.ToArray().SelectBegin(newNetMessage.ProtocolHeadBytesLength);
					}
					int contentLengthByHeadBytes = newNetMessage.GetContentLengthByHeadBytes();
					if (memoryStream.Length < newNetMessage.ProtocolHeadBytesLength + contentLengthByHeadBytes)
					{
						continue;
					}
					break;
				}
				return new OperateResult<byte[]>(-10000, StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut);
			}
			operateResult2 = OperateResult.CreateSuccessResult(memoryStream.ToArray());
		}
		else if (newNetMessage == null)
		{
			DateTime now = DateTime.Now;
			MemoryStream memoryStream2 = new MemoryStream();
			while (true)
			{
				OperateResult<byte[]> operateResult3 = ReceiveByMessage(socket, ReceiveTimeOut, newNetMessage);
				if (!operateResult3.IsSuccess)
				{
					return operateResult3;
				}
				memoryStream2.Write(operateResult3.Content);
				if (CheckReceiveDataComplete(array, memoryStream2))
				{
					operateResult2 = OperateResult.CreateSuccessResult(memoryStream2.ToArray());
					break;
				}
				if (ReceiveTimeOut > 0 && (DateTime.Now - now).TotalMilliseconds > (double)ReceiveTimeOut)
				{
					operateResult2 = new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut);
					break;
				}
			}
		}
		else
		{
			operateResult2 = ReceiveByMessage(socket, ReceiveTimeOut, newNetMessage);
		}
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? operateResult2.Content.ToHexString(' ') : SoftBasic.GetAsciiStringRender(operateResult2.Content)));
		if (newNetMessage != null && !newNetMessage.CheckHeadBytesLegal(base.Token.ToByteArray()))
		{
			if (mqttClient == null)
			{
				NetSupport.CloseSocket(socket);
			}
			return new OperateResult<byte[]>(StringResources.Language.CommandHeadCodeCheckFailed + Environment.NewLine + StringResources.Language.Send + ": " + SoftBasic.ByteToHexString(array, ' ') + Environment.NewLine + StringResources.Language.Receive + ": " + SoftBasic.ByteToHexString(operateResult2.Content, ' '));
		}
		return usePackAndUnpack ? UnpackResponseContent(array, operateResult2.Content) : OperateResult.CreateSuccessResult(operateResult2.Content);
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(byte[] send)
	{
		return ReadFromCoreServer(send, hasResponseData: true);
	}

	public OperateResult<byte[]> ReadFromCoreServer(IEnumerable<byte[]> send)
	{
		return NetSupport.ReadFromCoreServer(send, ReadFromCoreServer);
	}

	public OperateResult<byte[]> ReadFromCoreServer(byte[] send, bool hasResponseData, bool usePackAndUnpack = true)
	{
		if (pipeSocket.LockingTick > HslHelper.LockLimit)
		{
			return new OperateResult<byte[]>(StringResources.Language.TooManyLock);
		}
		OperateResult<byte[]> operateResult = new OperateResult<byte[]>();
		OperateResult<Socket> operateResult2 = null;
		pipeSocket.PipeLockEnter();
		try
		{
			operateResult2 = GetAvailableSocket();
			if (!operateResult2.IsSuccess)
			{
				pipeSocket.IsSocketError = true;
				AlienSession?.Offline();
				pipeSocket.PipeLockLeave();
				operateResult.CopyErrorFromOther(operateResult2);
				return operateResult;
			}
			OperateResult<byte[]> operateResult3 = ReadFromCoreServer(operateResult2.Content, send, hasResponseData, usePackAndUnpack);
			if (operateResult3.IsSuccess)
			{
				pipeSocket.IsSocketError = false;
				operateResult.IsSuccess = operateResult3.IsSuccess;
				operateResult.Content = operateResult3.Content;
				operateResult.Message = StringResources.Language.SuccessText;
			}
			else
			{
				if (operateResult3.ErrorCode != int.MinValue)
				{
					pipeSocket.IsSocketError = true;
					AlienSession?.Offline();
				}
				else
				{
					operateResult3.ErrorCode = 10000;
				}
				operateResult.CopyErrorFromOther(operateResult3);
			}
			ExtraAfterReadFromCoreServer(operateResult3);
			pipeSocket.PipeLockLeave();
		}
		catch
		{
			pipeSocket.PipeLockLeave();
			throw;
		}
		if (!isPersistentConn)
		{
			operateResult2?.Content?.Close();
		}
		return operateResult;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				ConnectClose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public override string ToString()
	{
		INetMessage newNetMessage = GetNewNetMessage();
		string text = ((newNetMessage == null) ? "INetMessage" : newNetMessage.GetType().ToString());
		string text2 = ((ByteTransform == null) ? "IByteTransform" : ByteTransform.GetType().ToString());
		return $"NetworkDoubleBase<{newNetMessage}, {text2}>[{IpAddress}:{Port}]";
	}
}
