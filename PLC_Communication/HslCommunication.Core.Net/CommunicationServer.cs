using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Core.Net;

public class CommunicationServer : CommunicationTcpServer
{
	public delegate void OnClientStatusChangeDelegate(object server, PipeSession session);

	public delegate void PipeMessageReceived(PipeSession session, byte[] buffer);

	private Socket udpServer = null;

	private EndPoint udpEndPoint = null;

	private byte[] udpBuffer;

	private List<PipeSession> pipes;

	private object lockSession = new object();

	private AsyncCallback beginReceiveCallback = null;

	private AsyncCallback udpBeginReceiveCallback = null;

	private AsyncCallback dtuSocketAsyncCallBack = null;

	public int UdpBufferSize { get; set; } = 2048;

	public Func<INetMessage> CreateNewMessage { get; set; }

	public Func<byte[], int, bool> CheckSerialDataComplete { get; set; }

	public uint SessionsMax { get; set; } = uint.MaxValue;

	public Func<PipeSession, IPEndPoint, OperateResult> ThreadPoolLoginAfterClientCheck { get; set; }

	public Func<CommunicationPipe, PipeSession> CreatePipeSession { get; set; }

	public int SerialReceiveAtleastTime { get; set; } = 20;

	public bool ForceSerialReceiveOnce { get; set; }

	public event OnClientStatusChangeDelegate OnClientOnline;

	public event OnClientStatusChangeDelegate OnClientOffline;

	public event PipeMessageReceived OnPipeMessageReceived;

	public CommunicationServer()
	{
		beginReceiveCallback = ReceiveCallback;
		udpBeginReceiveCallback = UdpAsyncCallback;
		dtuSocketAsyncCallBack = InitiativeSocketAsyncCallBack;
		CreatePipeSession = (CommunicationPipe m) => new PipeSession
		{
			Communication = m
		};
		pipes = new List<PipeSession>();
	}

	public void ServerStart(int port, bool modeTcp)
	{
		if (modeTcp)
		{
			ServerStart(port);
		}
		else if (!base.IsStarted)
		{
			if (!base.EnableIPv6)
			{
				udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				udpServer.Bind(new IPEndPoint(IPAddress.Any, port));
			}
			else
			{
				udpServer = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
				udpServer.Bind(new IPEndPoint(IPAddress.IPv6Any, port));
			}
			base.IsStarted = true;
			base.Port = port;
			if (udpBuffer == null)
			{
				udpBuffer = new byte[UdpBufferSize];
			}
			udpEndPoint = new IPEndPoint(base.EnableIPv6 ? IPAddress.Parse("::1") : IPAddress.Parse("127.0.0.1"), 0);
			PipeUdpNet arg = new PipeUdpNet
			{
				Socket = udpServer
			};
			PipeSession pipeSession = CreatePipeSession(arg);
			SetUdpIp(pipeSession);
			AddSession(pipeSession);
			UdpRefreshReceive(pipeSession);
			ExtraOnStart();
			LogDebugMsg(StringResources.Language.NetEngineStart);
		}
	}

	public void ServerStart(int tcpPort, int udpPort)
	{
		if (!base.IsStarted)
		{
			ServerStart(tcpPort);
			if (!base.EnableIPv6)
			{
				udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				udpServer.Bind(new IPEndPoint(IPAddress.Any, udpPort));
			}
			else
			{
				udpServer = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
				udpServer.Bind(new IPEndPoint(IPAddress.IPv6Any, udpPort));
			}
			if (udpBuffer == null)
			{
				udpBuffer = new byte[UdpBufferSize];
			}
			udpEndPoint = new IPEndPoint(base.EnableIPv6 ? IPAddress.Parse("::1") : IPAddress.Parse("127.0.0.1"), 0);
			PipeUdpNet arg = new PipeUdpNet
			{
				Socket = udpServer
			};
			PipeSession pipeSession = CreatePipeSession(arg);
			SetUdpIp(pipeSession);
			AddSession(pipeSession);
			UdpRefreshReceive(pipeSession);
		}
	}

	protected override void ExtraOnClose()
	{
		base.ExtraOnClose();
		lock (lockSession)
		{
			for (int i = 0; i < pipes.Count; i++)
			{
				pipes[i].Close();
			}
			pipes.Clear();
		}
		if (udpServer != null)
		{
			NetSupport.CloseSocket(udpServer);
		}
	}

	public void SetNetMessage(INetMessage netMessage)
	{
		CreateNewMessage = () => netMessage;
	}

	public void AddSession(PipeSession session)
	{
		lock (lockSession)
		{
			pipes.Add(session);
		}
		LogDebugMsg(string.Format(StringResources.Language.ClientOnlineInfo, session.Communication));
		this.OnClientOnline?.Invoke(this, session);
	}

	public void RemoveSession(PipeSession session, string reason)
	{
		bool flag = false;
		lock (lockSession)
		{
			flag = pipes.Remove(session);
		}
		if (flag)
		{
			session.Close();
			LogDebugMsg(string.Format(StringResources.Language.ClientOfflineInfo, session.Communication) + " " + reason);
			this.OnClientOffline?.Invoke(this, session);
		}
	}

	public PipeSession[] GetPipeSessions()
	{
		PipeSession[] result = null;
		lock (lockSession)
		{
			result = pipes.ToArray();
		}
		return result;
	}

	public void RemoveSession(TimeSpan timeSpan)
	{
		lock (lockSession)
		{
			for (int num = pipes.Count - 1; num >= 0; num--)
			{
				PipeSession pipeSession = pipes[num];
				if (pipeSession.Communication is PipeTcpNet pipeTcpNet && pipeTcpNet.GetType() == typeof(PipeTcpNet) && DateTime.Now - pipeSession.HeartTime > timeSpan)
				{
					pipeSession.Close();
					pipes.RemoveAt(num);
					LogDebugMsg(string.Format(StringResources.Language.ClientOfflineInfo, pipeSession.Communication) + $" Not communication for times[{timeSpan}]");
				}
			}
		}
	}

	public RemoteConnectInfo ConnectRemoteServer(string ipAddress, int port, byte[] dtu = null)
	{
		RemoteConnectInfo remoteConnectInfo = new RemoteConnectInfo(ipAddress, port, dtu);
		ThreadPool.QueueUserWorkItem(ConnectIpEndPoint, remoteConnectInfo);
		return remoteConnectInfo;
	}

	public RemoteConnectInfo ConnectHslAlientClient(string ipAddress, int port, string dtuId, string password = "", bool needAckResult = true)
	{
		RemoteConnectInfo remoteConnectInfo = new RemoteConnectInfo(ipAddress, port, dtuId, password, needAckResult);
		ThreadPool.QueueUserWorkItem(ConnectIpEndPoint, remoteConnectInfo);
		return remoteConnectInfo;
	}

	private OperateResult CheckConnectRemote(RemoteConnectInfo remoteConnect, PipeSession session)
	{
		PipeTcpNet pipeTcpNet = session.Communication as PipeTcpNet;
		if (remoteConnect.DtuBytes != null)
		{
			OperateResult operateResult = pipeTcpNet.Send(remoteConnect.DtuBytes);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			if (remoteConnect.NeedAckResult)
			{
				OperateResult<byte[]> operateResult2 = pipeTcpNet.ReceiveMessage(new AlienMessage(), null);
				if (!operateResult2.IsSuccess)
				{
					return operateResult2;
				}
				switch (operateResult2.Content[5])
				{
				case 1:
					return new OperateResult(StringResources.Language.DeviceCurrentIsLoginRepeat);
				case 2:
					return new OperateResult(StringResources.Language.DeviceCurrentIsLoginForbidden);
				case 3:
					return new OperateResult(StringResources.Language.PasswordCheckFailed);
				}
			}
		}
		OperateResult operateResult3 = SocketAcceptExtraCheck(pipeTcpNet.Socket, remoteConnect.EndPoint);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		if (ThreadPoolLoginAfterClientCheck != null)
		{
			OperateResult operateResult4 = ThreadPoolLoginAfterClientCheck(session, remoteConnect.EndPoint);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	private void ConnectIpEndPoint(object obj)
	{
		if (!(obj is RemoteConnectInfo remoteConnectInfo))
		{
			return;
		}
		if (remoteConnectInfo.Status == DtuStatus.Closed)
		{
			remoteConnectInfo.Session?.Close();
			return;
		}
		remoteConnectInfo.Status = DtuStatus.Connecting;
		OperateResult<Socket> operateResult = NetSupport.CreateSocketAndConnect(remoteConnectInfo.EndPoint, 10000);
		if (!operateResult.IsSuccess)
		{
			LogDebugMsg($"RemoteConnectInfo[{remoteConnectInfo.EndPoint}] Socket Connected Failed : {operateResult.Message} 10s later retry...");
			HslHelper.ThreadSleep(10000);
			ThreadPool.QueueUserWorkItem(ConnectIpEndPoint, remoteConnectInfo);
			return;
		}
		LogDebugMsg($"RemoteConnectInfo[{remoteConnectInfo.EndPoint}] Socket Connected Success");
		PipeTcpNet pipeTcpNet = new PipeTcpNet(remoteConnectInfo.EndPoint.Address.ToString(), remoteConnectInfo.EndPoint.Port);
		pipeTcpNet.Socket = operateResult.Content;
		PipeSession pipeSession = CreatePipeSession(pipeTcpNet);
		OperateResult operateResult2 = CheckConnectRemote(remoteConnectInfo, pipeSession);
		if (!operateResult2.IsSuccess)
		{
			LogDebugMsg($"RemoteConnectInfo[{remoteConnectInfo.EndPoint}] Socket Check Failed : {operateResult2.Message} 10s later retry...");
			NetSupport.CloseSocket(operateResult.Content);
			HslHelper.ThreadSleep(10000);
			ThreadPool.QueueUserWorkItem(ConnectIpEndPoint, remoteConnectInfo);
			return;
		}
		LogDebugMsg($"RemoteConnectInfo[{remoteConnectInfo.EndPoint}] Socket Check Success");
		remoteConnectInfo.Session = pipeSession;
		remoteConnectInfo.Status = DtuStatus.Connected;
		pipeSession.OnlineTime = DateTime.Now;
		try
		{
			pipeTcpNet.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, dtuSocketAsyncCallBack, remoteConnectInfo);
			AddSession(pipeSession);
		}
		catch (Exception ex)
		{
			LogDebugMsg($"ConnectIpEndPoint[{remoteConnectInfo.EndPoint}] Socket.BeginReceive failed: " + ex.Message);
			NetSupport.CloseSocket(pipeTcpNet.Socket);
			ThreadPool.QueueUserWorkItem(ConnectIpEndPoint, remoteConnectInfo);
		}
	}

	private void InitiativeSocketAsyncCallBack(IAsyncResult ar)
	{
		if (!(ar.AsyncState is RemoteConnectInfo { Session: var session } remoteConnectInfo))
		{
			return;
		}
		PipeTcpNet pipeTcpNet = session.Communication as PipeTcpNet;
		byte[] array = null;
		try
		{
			Socket socket = pipeTcpNet.Socket;
			if (socket == null)
			{
				RemoveSession(session, string.Empty);
				return;
			}
			int num = socket.EndReceive(ar);
			INetMessage newNetMessage = GetNewNetMessage();
			OperateResult<byte[]> operateResult = pipeTcpNet.ReceiveMessage(newNetMessage, null, useActivePush: false);
			if (!operateResult.IsSuccess)
			{
				RemoveSession(session, operateResult.Message);
				if (base.IsStarted)
				{
					LogDebugMsg($"RemoteConnectInfo[{remoteConnectInfo.EndPoint}] Socket Connected 10s later retry...");
					HslHelper.ThreadSleep(10000);
					ConnectIpEndPoint(remoteConnectInfo);
				}
				return;
			}
			session.HeartTime = DateTime.Now;
			array = operateResult.Content;
		}
		catch (Exception ex)
		{
			RemoveSession(session, ex.Message);
			if (base.IsStarted)
			{
				ConnectIpEndPoint(remoteConnectInfo);
			}
			return;
		}
		this.OnPipeMessageReceived?.Invoke(session, array);
		try
		{
			pipeTcpNet.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, dtuSocketAsyncCallBack, remoteConnectInfo);
		}
		catch (Exception ex2)
		{
			RemoveSession(session, ex2.Message);
			HslHelper.ThreadSleep(1000);
			if (base.IsStarted)
			{
				ConnectIpEndPoint(remoteConnectInfo);
			}
		}
	}

	private void SetUdpIp(PipeSession session)
	{
		PipeUdpNet pipeUdpNet = session.Communication as PipeUdpNet;
		if (udpEndPoint is IPEndPoint iPEndPoint)
		{
			pipeUdpNet.IpAddress = iPEndPoint.Address.ToString();
			pipeUdpNet.Port = iPEndPoint.Port;
		}
	}

	private void UdpRefreshReceive(PipeSession session)
	{
		try
		{
			udpServer.BeginReceiveFrom(udpBuffer, 0, udpBuffer.Length, SocketFlags.None, ref udpEndPoint, udpBeginReceiveCallback, session);
		}
		catch (Exception ex)
		{
			RemoveSession(session, "UdpRefreshReceive exception: " + ex.Message);
		}
	}

	private void UdpAsyncCallback(IAsyncResult ar)
	{
		if (!(ar.AsyncState is PipeSession pipeSession))
		{
			return;
		}
		PipeUdpNet pipeUdpNet = pipeSession.Communication as PipeUdpNet;
		byte[] array = null;
		if (pipeUdpNet.Socket == null)
		{
			RemoveSession(pipeSession, "Server closed");
			return;
		}
		try
		{
			int num = pipeUdpNet.Socket.EndReceiveFrom(ar, ref udpEndPoint);
			pipeSession.HeartTime = DateTime.Now;
			SetUdpIp(pipeSession);
			if (!Authorization.nzugaydgwadawdibbas())
			{
				RemoveSession(pipeSession, StringResources.Language.AuthorizationFailed);
				return;
			}
			INetMessage newNetMessage = GetNewNetMessage();
			if (newNetMessage != null)
			{
				OperateResult<byte[]> operateResult = pipeUdpNet.ReceiveMessage(newNetMessage, null, udpBuffer.SelectBegin(num), null, closeOnException: false);
				if (!operateResult.IsSuccess)
				{
					LogDebugMsg($"<{pipeUdpNet}> Udp ReceiveMessage faild: " + operateResult.Message + ((num > 0) ? udpBuffer.SelectBegin(num).ToHexString(' ') : string.Empty));
					UdpRefreshReceive(pipeSession);
					return;
				}
				array = operateResult.Content;
			}
			else
			{
				array = udpBuffer.SelectBegin(num);
			}
		}
		catch (ObjectDisposedException)
		{
			RemoveSession(pipeSession, "Socket ObjectDisposedException");
			return;
		}
		catch (Exception ex2)
		{
			LogDebugMsg($"<{pipeUdpNet}> UdpAsyncCallback faild: " + ex2.Message);
			UdpRefreshReceive(pipeSession);
			return;
		}
		this.OnPipeMessageReceived?.Invoke(pipeSession, array);
		UdpRefreshReceive(pipeSession);
	}

	protected override void ThreadPoolLogin(PipeTcpNet pipeTcpNet, IPEndPoint endPoint)
	{
		PipeSession pipeSession = CreatePipeSession(pipeTcpNet);
		if (ThreadPoolLoginAfterClientCheck != null)
		{
			OperateResult operateResult = ThreadPoolLoginAfterClientCheck(pipeSession, endPoint);
			if (!operateResult.IsSuccess)
			{
				base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientDisableLogin, endPoint) + " LoginAfterClientCheck failed");
				pipeTcpNet?.CloseCommunication();
				return;
			}
		}
		if (GetPipeSessions().Length >= SessionsMax)
		{
			base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientDisableLogin, endPoint) + $" Online count > SessionsMax({SessionsMax})");
			pipeTcpNet?.CloseCommunication();
			return;
		}
		try
		{
			pipeTcpNet.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, pipeSession);
			AddSession(pipeSession);
		}
		catch (Exception ex)
		{
			LogDebugMsg(StringResources.Language.SocketReceiveException + " " + ex.Message);
		}
	}

	private async void ReceiveCallback(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (!(asyncState is PipeSession session))
		{
			return;
		}
		PipeTcpNet pipeTcpNet = session.Communication as PipeTcpNet;
		byte[] buffer;
		try
		{
			Socket client = pipeTcpNet.Socket;
			if (client == null)
			{
				RemoveSession(session, string.Empty);
				return;
			}
			client.EndReceive(ar);
			INetMessage netMessage = GetNewNetMessage();
			OperateResult<byte[]> read = await pipeTcpNet.ReceiveMessageAsync(netMessage, null, useActivePush: false);
			if (!read.IsSuccess)
			{
				RemoveSession(session, read.Message);
				return;
			}
			session.HeartTime = DateTime.Now;
			buffer = read.Content;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			if (ex2.Message.Contains(StringResources.Language.SocketRemoteCloseException))
			{
				RemoveSession(session, string.Empty);
			}
			else
			{
				RemoveSession(session, ex2.Message);
			}
			return;
		}
		this.OnPipeMessageReceived?.Invoke(session, buffer);
		try
		{
			pipeTcpNet.Socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, beginReceiveCallback, session);
		}
		catch (Exception ex3)
		{
			RemoveSession(session, ex3.Message);
		}
	}

	public OperateResult StartSerialSlave(string com)
	{
		if (com.Contains("-") || com.Contains(";"))
		{
			return StartSerialSlave(delegate(SerialPort sp)
			{
				sp.IniSerialByFormatString(com);
			});
		}
		return StartSerialSlave(com, 9600);
	}

	public OperateResult StartSerialSlave(string com, int baudRate)
	{
		return StartSerialSlave(delegate(SerialPort sp)
		{
			sp.PortName = com;
			sp.BaudRate = baudRate;
			sp.DataBits = 8;
			sp.Parity = Parity.None;
			sp.StopBits = StopBits.One;
		});
	}

	public OperateResult StartSerialSlave(string com, int baudRate, int dataBits, Parity parity, StopBits stopBits)
	{
		return StartSerialSlave(delegate(SerialPort sp)
		{
			sp.PortName = com;
			sp.BaudRate = baudRate;
			sp.DataBits = dataBits;
			sp.Parity = parity;
			sp.StopBits = stopBits;
		});
	}

	public OperateResult StartSerialSlave(Action<SerialPort> inni)
	{
		PipeSerialPort pipeSerialPort = new PipeSerialPort();
		pipeSerialPort.SerialPortInni(inni);
		pipeSerialPort.GetPipe().DataReceived += SerialPort_DataReceived;
		OperateResult operateResult = pipeSerialPort.OpenCommunication();
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		AddSession(CreatePipeSession(pipeSerialPort));
		return OperateResult.CreateSuccessResult();
	}

	public void CloseSerialSlave()
	{
		lock (lockSession)
		{
			for (int num = pipes.Count - 1; num >= 0; num--)
			{
				PipeSession pipeSession = pipes[num];
				if (pipeSession.Communication is PipeSerialPort pipeSerialPort)
				{
					pipeSerialPort.CloseCommunication();
					pipes.RemoveAt(num);
				}
			}
		}
	}

	private PipeSession FindSerialPortSession(string portName)
	{
		lock (lockSession)
		{
			for (int num = pipes.Count - 1; num >= 0; num--)
			{
				PipeSession pipeSession = pipes[num];
				if (pipeSession.Communication is PipeSerialPort pipeSerialPort && pipeSerialPort.GetPipe().PortName == portName)
				{
					return pipeSession;
				}
			}
		}
		return null;
	}

	private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		if (!Authorization.nzugaydgwadawdibbas())
		{
			return;
		}
		SerialPort serialPort = sender as SerialPort;
		PipeSession pipeSession = FindSerialPortSession(serialPort.PortName);
		if (pipeSession == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		byte[] array = new byte[2048];
		DateTime now = DateTime.Now;
		while (true)
		{
			try
			{
				int num3 = serialPort.Read(array, num, serialPort.BytesToRead);
				if (num3 == 0 && num2 != 0 && (DateTime.Now - now).TotalMilliseconds >= (double)SerialReceiveAtleastTime)
				{
					break;
				}
				num += num3;
				num2++;
				goto IL_00c7;
			}
			catch (Exception ex)
			{
				LogDebugMsg("SerialPort_DataReceived Error: " + ex.Message);
			}
			break;
			IL_00c7:
			if ((ForceSerialReceiveOnce && num > 0) || CheckSerialReceiveDataComplete(array, num))
			{
				break;
			}
			HslHelper.ThreadSleep(20);
		}
		if (num != 0)
		{
			try
			{
				array = array.SelectBegin(num);
			}
			catch (Exception ex2)
			{
				LogDebugMsg("SerialPort_DataReceived: " + ex2.Message);
			}
			pipeSession.HeartTime = DateTime.Now;
			this.OnPipeMessageReceived?.Invoke(pipeSession, array);
		}
	}

	protected virtual bool CheckSerialReceiveDataComplete(byte[] buffer, int receivedLength)
	{
		if (CheckSerialDataComplete == null)
		{
			return false;
		}
		return CheckSerialDataComplete(buffer, receivedLength);
	}

	protected virtual INetMessage GetNewNetMessage()
	{
		return (CreateNewMessage == null) ? null : CreateNewMessage();
	}

	public override string ToString()
	{
		return $"CommunicationServer[{base.Port}]";
	}
}
