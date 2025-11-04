using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Pipe;
using HslCommunication.LogNet;

namespace HslCommunication.WebSocket;

public class WebSocketClient : IDisposable
{
	public delegate void OnClientApplicationMessageReceiveDelegate(WebSocketMessage message);

	public delegate void OnClientConnectedDelegate();

	private int isReConnectServer = 0;

	private string[] subcribeTopics;

	private bool closed = false;

	private string ipAddress = string.Empty;

	private int port = 1883;

	private int connectTimeOut = 10000;

	private Timer timerCheck;

	private string url = string.Empty;

	private bool disposedValue;

	private string host = string.Empty;

	private bool useSSL = false;

	private X509Certificate certificate = null;

	private PipeTcpNet communicationPipe;

	private AsyncCallback beginReceiveCallback = null;

	public string IpAddress
	{
		get
		{
			return ipAddress;
		}
		set
		{
			ipAddress = HslHelper.GetIpAddressFromInput(value);
		}
	}

	public int Port
	{
		get
		{
			return port;
		}
		set
		{
			port = value;
		}
	}

	public bool GetCarryHostAndPort { get; set; } = false;

	public ILogNet LogNet { get; set; }

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

	public bool IsClosed => closed;

	public event OnClientApplicationMessageReceiveDelegate OnClientApplicationMessageReceive;

	public event OnClientConnectedDelegate OnClientConnected;

	public event EventHandler OnNetworkError;

	public WebSocketClient(string ipAddress, int port)
	{
		beginReceiveCallback = ReceiveAsyncCallback;
		host = ipAddress;
		IpAddress = ipAddress;
		Port = port;
	}

	public WebSocketClient(string ipAddress, int port, string url)
	{
		beginReceiveCallback = ReceiveAsyncCallback;
		host = ipAddress;
		IpAddress = ipAddress;
		Port = port;
		this.url = url;
	}

	public WebSocketClient(string url)
	{
		beginReceiveCallback = ReceiveAsyncCallback;
		if (url.StartsWith("ws://", StringComparison.OrdinalIgnoreCase))
		{
			port = 80;
			PraseHost(url.Substring(5));
			return;
		}
		if (url.StartsWith("wss://", StringComparison.OrdinalIgnoreCase))
		{
			port = 443;
			PraseHost(url.Substring(6));
			UseSSL(string.Empty);
			return;
		}
		throw new Exception("Url Must start with ws:// or wss://");
	}

	private void PraseHost(string url)
	{
		if (url.IndexOf('/') < 0)
		{
			PraseIPHost(url);
			return;
		}
		PraseIPHost(url.Substring(0, url.IndexOf('/')));
		this.url = url.Substring(url.IndexOf('/'));
	}

	private void PraseIPHost(string url)
	{
		if (url.IndexOf(':') < 0)
		{
			host = url;
			IpAddress = url;
		}
		else
		{
			host = url.Substring(0, url.IndexOf(':'));
			IpAddress = host;
			Port = int.Parse(url.Substring(url.IndexOf(':') + 1));
		}
	}

	public OperateResult ConnectServer()
	{
		return ConnectServer(subcribeTopics);
	}

	public OperateResult ConnectServer(string[] subscribes)
	{
		subcribeTopics = subscribes;
		communicationPipe?.CloseCommunication();
		if (!useSSL)
		{
			communicationPipe = new PipeTcpNet(ipAddress, port)
			{
				ConnectTimeOut = connectTimeOut
			};
		}
		else
		{
			communicationPipe = new PipeSslNet(host, port, serverMode: false)
			{
				ConnectTimeOut = connectTimeOut,
				Certificate = certificate
			};
		}
		OperateResult operateResult = communicationPipe.OpenCommunication();
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		byte[] data = WebSocketHelper.BuildWsSubRequest(ipAddress, port, url, subcribeTopics, GetCarryHostAndPort);
		OperateResult operateResult2 = communicationPipe.Send(data);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = communicationPipe.Receive(-1, 10000);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		try
		{
			communicationPipe.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, communicationPipe);
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
		closed = false;
		this.OnClientConnected?.Invoke();
		timerCheck?.Dispose();
		timerCheck = new Timer(TimerCheckServer, null, 2000, 30000);
		return OperateResult.CreateSuccessResult();
	}

	public void ConnectClose()
	{
		if (!closed)
		{
			SendWebsocketBytes(WebSocketHelper.WebScoketPackData(8, isMask: true, "Closed"));
			closed = true;
			HslHelper.ThreadSleep(20);
			communicationPipe?.CloseCommunication();
		}
	}

	public async Task<OperateResult> ConnectServerAsync()
	{
		return await ConnectServerAsync(subcribeTopics);
	}

	public async Task<OperateResult> ConnectServerAsync(string[] subscribes)
	{
		subcribeTopics = subscribes;
		communicationPipe?.CloseCommunication();
		if (!useSSL)
		{
			communicationPipe = new PipeTcpNet(ipAddress, port)
			{
				ConnectTimeOut = connectTimeOut
			};
		}
		else
		{
			communicationPipe = new PipeSslNet(ipAddress, port, serverMode: false)
			{
				ConnectTimeOut = connectTimeOut,
				Certificate = certificate
			};
		}
		OperateResult open = await communicationPipe.OpenCommunicationAsync();
		if (!open.IsSuccess)
		{
			return open;
		}
		byte[] command = WebSocketHelper.BuildWsSubRequest(ipAddress, port, url, subcribeTopics, GetCarryHostAndPort);
		OperateResult send = await communicationPipe.SendAsync(command).ConfigureAwait(continueOnCapturedContext: false);
		if (!send.IsSuccess)
		{
			return send;
		}
		OperateResult<byte[]> rece = await communicationPipe.ReceiveAsync(-1, 10000).ConfigureAwait(continueOnCapturedContext: false);
		if (!rece.IsSuccess)
		{
			return rece;
		}
		try
		{
			communicationPipe.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, communicationPipe);
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
		closed = false;
		this.OnClientConnected?.Invoke();
		timerCheck?.Dispose();
		timerCheck = new Timer(TimerCheckServer, null, 2000, 30000);
		return OperateResult.CreateSuccessResult();
	}

	public async Task ConnectCloseAsync()
	{
		if (!closed)
		{
			await communicationPipe.SendAsync(WebSocketHelper.WebScoketPackData(8, isMask: true, "Closed")).ConfigureAwait(continueOnCapturedContext: false);
			closed = true;
			HslHelper.ThreadSleep(20);
			communicationPipe?.CloseCommunication();
		}
	}

	private void OnWebsocketNetworkError()
	{
		if (closed || Interlocked.CompareExchange(ref isReConnectServer, 1, 0) != 0)
		{
			return;
		}
		try
		{
			if (this.OnNetworkError == null)
			{
				LogNet?.WriteInfo(ToString(), "The network is abnormal, and the system is ready to automatically reconnect after 10 seconds.");
				while (true)
				{
					for (int i = 0; i < 10; i++)
					{
						if (closed)
						{
							Interlocked.Exchange(ref isReConnectServer, 0);
							return;
						}
						HslHelper.ThreadSleep(1000);
						LogNet?.WriteInfo(ToString(), $"Wait for {10 - i} second to connect to the server ...");
					}
					if (closed)
					{
						Interlocked.Exchange(ref isReConnectServer, 0);
						return;
					}
					OperateResult operateResult = ConnectServer();
					if (operateResult.IsSuccess)
					{
						break;
					}
					LogNet?.WriteInfo(ToString(), "The connection failed. Prepare to reconnect after 10 seconds.");
				}
				LogNet?.WriteInfo(ToString(), "Successfully connected to the server!");
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

	private async void ReceiveAsyncCallback(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (!(asyncState is PipeTcpNet { Socket: var socket } pipeTcpNet))
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
			LogNet?.WriteDebug(ToString(), "Closed");
			return;
		}
		catch (Exception ex2)
		{
			Exception ex4 = ex2;
			Exception ex5 = ex4;
			socket?.Close();
			LogNet?.WriteDebug(ToString(), "ReceiveCallback Failed:" + ex5.Message);
			OnWebsocketNetworkError();
			return;
		}
		if (closed)
		{
			LogNet?.WriteDebug(ToString(), "Closed");
			return;
		}
		OperateResult<WebSocketMessage> read = await WebSocketHelper.ReceiveWebSocketPayloadAsync(pipeTcpNet).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			OnWebsocketNetworkError();
			return;
		}
		if (read.Content.OpCode == 9)
		{
			SendWebsocketBytes(WebSocketHelper.WebScoketPackData(10, isMask: true, read.Content.Payload));
			LogNet?.WriteDebug(ToString(), read.Content.ToString());
		}
		else if (read.Content.OpCode == 10)
		{
			LogNet?.WriteDebug(ToString(), read.Content.ToString());
		}
		else
		{
			this.OnClientApplicationMessageReceive?.Invoke(read.Content);
		}
		try
		{
			socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, pipeTcpNet);
		}
		catch (Exception ex6)
		{
			socket?.Close();
			LogNet?.WriteDebug(ToString(), "BeginReceive Failed:" + ex6.Message);
			OnWebsocketNetworkError();
		}
	}

	private void TimerCheckServer(object obj)
	{
		if (communicationPipe != null)
		{
		}
	}

	public void UseSSL(string certificateFile)
	{
		useSSL = true;
		if (!string.IsNullOrEmpty(certificateFile))
		{
			certificate = X509Certificate.CreateFromCertFile(certificateFile);
		}
	}

	public void UseSSL(X509Certificate certificate)
	{
		useSSL = true;
		this.certificate = certificate;
	}

	public OperateResult SendServer(string message)
	{
		return SendWebsocketBytes(WebSocketHelper.WebScoketPackData(1, isMask: true, message));
	}

	public OperateResult SendServer(bool mask, string message)
	{
		return SendWebsocketBytes(WebSocketHelper.WebScoketPackData(1, mask, message));
	}

	public OperateResult SendServer(int opCode, bool mask, byte[] payload)
	{
		return SendWebsocketBytes(WebSocketHelper.WebScoketPackData(opCode, mask, payload));
	}

	private OperateResult SendWebsocketBytes(byte[] data)
	{
		return communicationPipe.Send(data);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				this.OnClientApplicationMessageReceive = null;
				this.OnClientConnected = null;
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

	public override string ToString()
	{
		return $"WebSocketClient[{ipAddress}:{port}]";
	}
}
