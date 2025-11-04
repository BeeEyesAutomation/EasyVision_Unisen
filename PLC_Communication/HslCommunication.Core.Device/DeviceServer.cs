using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using HslCommunication.Core.Net;

namespace HslCommunication.Core.Device;

public class DeviceServer : DeviceCommunication
{
	public delegate void DataReceivedDelegate(object sender, PipeSession session, byte[] data);

	public delegate void DataSendDelegate(object sender, byte[] data);

	private Timer timerHeart;

	private CommunicationServer server;

	private int udpServePort = 0;

	public bool IsStarted { get; private set; }

	public int Port
	{
		get
		{
			return server.Port;
		}
		set
		{
			server.Port = value;
		}
	}

	public int BothModeUdpPort => udpServePort;

	public bool EnableIPv6
	{
		get
		{
			return server.EnableIPv6;
		}
		set
		{
			server.EnableIPv6 = value;
		}
	}

	public int SocketKeepAliveTime
	{
		get
		{
			return server.SocketKeepAliveTime;
		}
		set
		{
			server.SocketKeepAliveTime = value;
		}
	}

	public bool EnableWrite { get; set; } = true;

	public TimeSpan ActiveTimeSpan { get; set; }

	public int SerialReceiveAtleastTime
	{
		get
		{
			return server.SerialReceiveAtleastTime;
		}
		set
		{
			server.SerialReceiveAtleastTime = value;
		}
	}

	public bool ForceSerialReceiveOnce
	{
		get
		{
			return server.ForceSerialReceiveOnce;
		}
		set
		{
			server.ForceSerialReceiveOnce = value;
		}
	}

	public int OnlineCount => server.GetPipeSessions().Length;

	public int ServerMode { get; private set; }

	public event DataReceivedDelegate OnDataReceived;

	public event DataSendDelegate OnDataSend;

	public DeviceServer()
	{
		ActiveTimeSpan = TimeSpan.FromHours(24.0);
		server = new CommunicationServer();
		server.OnPipeMessageReceived += Server_OnPipeMessageReceived;
		server.LogDebugMessage = delegate(string m)
		{
			base.LogNet?.WriteDebug(ToString(), m);
		};
		server.CreateNewMessage = GetNewNetMessage;
		server.ThreadPoolLoginAfterClientCheck = ThreadPoolLoginAfterClientCheck;
		server.CheckSerialDataComplete = CheckSerialReceiveDataComplete;
		timerHeart = new Timer(ThreadTimerHeartCheck, null, 2000, 10000);
	}

	public CommunicationServer GetCommunicationServer()
	{
		return server;
	}

	protected virtual OperateResult SocketAcceptExtraCheck(Socket socket, IPEndPoint endPoint)
	{
		return OperateResult.CreateSuccessResult();
	}

	protected virtual void StartInitialization()
	{
	}

	public virtual void ServerStart(int port, bool modeTcp = true)
	{
		if (!IsStarted)
		{
			IsStarted = true;
			ServerMode = ((!modeTcp) ? 1 : 0);
			StartInitialization();
			server.ServerStart(port, modeTcp);
		}
	}

	public void ServerStart(int tcpPort, int udpPort)
	{
		if (!IsStarted)
		{
			IsStarted = true;
			ServerMode = 2;
			StartInitialization();
			server.ServerStart(tcpPort, udpPort);
			udpServePort = udpPort;
		}
	}

	public void ServerStart()
	{
		ServerStart(Port);
	}

	protected virtual void CloseAction()
	{
	}

	public virtual void ServerClose()
	{
		if (IsStarted)
		{
			IsStarted = false;
			server.ServerClose();
			CloseAction();
		}
	}

	public void CloseSerialSlave()
	{
		server.CloseSerialSlave();
	}

	public void UseSSL(X509Certificate cert)
	{
		server.UseSSL(cert);
	}

	public void UseSSL(string cert, string password = "")
	{
		server.UseSSL(cert, password);
	}

	public void SetTrustedIpAddress(List<string> clients)
	{
		server.SetTrustedIpAddress(clients);
	}

	public string[] GetTrustedClients()
	{
		return server.GetTrustedClients();
	}

	public void SaveDataPool(string path)
	{
		byte[] bytes = SaveToBytes();
		File.WriteAllBytes(path, bytes);
	}

	public void LoadDataPool(string path)
	{
		if (File.Exists(path))
		{
			byte[] content = File.ReadAllBytes(path);
			LoadFromBytes(content);
		}
	}

	protected virtual void LoadFromBytes(byte[] content)
	{
	}

	protected virtual byte[] SaveToBytes()
	{
		return new byte[0];
	}

	protected void RaiseDataReceived(PipeSession session, byte[] receive)
	{
		this.OnDataReceived?.Invoke(this, session, receive);
	}

	protected void RaiseDataSend(byte[] send)
	{
		this.OnDataSend?.Invoke(this, send);
	}

	private void Server_OnPipeMessageReceived(PipeSession session, byte[] buffer)
	{
		LogRevcMessage(buffer, session);
		OperateResult<byte[]> operateResult = null;
		try
		{
			operateResult = ReadFromCoreServer(session, buffer);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException("ReadFromCoreServer", "Source Data: " + buffer.ToHexString(' '), ex);
			return;
		}
		if (!operateResult.IsSuccess)
		{
			if (operateResult.ErrorCode != int.MinValue)
			{
				base.LogNet?.WriteDebug(ToString(), $"<{session.Communication}> {operateResult.Message}");
			}
			if (operateResult.Content != null && operateResult.Content.Length != 0)
			{
				operateResult.IsSuccess = true;
			}
		}
		if (operateResult.IsSuccess && operateResult.Content != null && operateResult.Content.Length != 0)
		{
			session.Communication.Send(operateResult.Content);
			RaiseDataSend(operateResult.Content);
			LogSendMessage(operateResult.Content, session);
		}
		RaiseDataReceived(session, buffer);
	}

	protected virtual OperateResult<byte[]> ReadFromCoreServer(PipeSession session, byte[] receive)
	{
		return new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction);
	}

	protected virtual OperateResult ThreadPoolLoginAfterClientCheck(PipeSession session, IPEndPoint endPoint)
	{
		return OperateResult.CreateSuccessResult();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			this.OnDataSend = null;
			this.OnDataReceived = null;
			ServerClose();
		}
		base.Dispose(disposing);
	}

	public OperateResult StartSerialSlave(string com)
	{
		return server.StartSerialSlave(com);
	}

	public OperateResult StartSerialSlave(string com, int baudRate)
	{
		return server.StartSerialSlave(com, baudRate);
	}

	public OperateResult StartSerialSlave(string com, int baudRate, int dataBits, Parity parity, StopBits stopBits)
	{
		return server.StartSerialSlave(com, baudRate, dataBits, parity, stopBits);
	}

	public OperateResult StartSerialSlave(Action<SerialPort> inni)
	{
		return server.StartSerialSlave(inni);
	}

	protected virtual bool CheckSerialReceiveDataComplete(byte[] buffer, int receivedLength)
	{
		return false;
	}

	private void ThreadTimerHeartCheck(object obj)
	{
		server.RemoveSession(ActiveTimeSpan);
	}
}
