using System;
using System.Net;
using System.Net.Sockets;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Reflection;

namespace HslCommunication.Enthernet;

public class TcpForward : CommunicationServer
{
	public delegate void OnMessageReceivedDelegate(ForwardSession session, byte[] data);

	private string _hostIp = string.Empty;

	private int _port = 0;

	[HslMqttApi(HttpMethod = "GET", Description = "Gets or sets the timeout for the connection, in milliseconds")]
	public virtual int ConnectTimeOut { get; set; }

	public int CacheSize { get; set; } = 2048;

	public int OnlineSessionsCount => GetPipeSessions().Length;

	public bool LogMsgFormatBinary { get; set; } = true;

	public event OnMessageReceivedDelegate OnRemoteMessageReceived;

	public event OnMessageReceivedDelegate OnClientMessageReceive;

	public TcpForward(int localPort, string host, int hostPort)
	{
		base.Port = localPort;
		_hostIp = host;
		_port = hostPort;
		ConnectTimeOut = 5000;
		base.CreatePipeSession = (CommunicationPipe m) => new ForwardSession
		{
			Communication = m
		};
	}

	protected override void ThreadPoolLogin(PipeTcpNet pipe, IPEndPoint endPoint)
	{
		base.LogNet?.WriteInfo(ToString(), $"Local client[{endPoint}] connected");
		OperateResult<Socket> operateResult = NetSupport.CreateSocketAndConnect(_hostIp, _port, ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			base.LogNet?.WriteError(ToString(), "Connect server failed, local client close: " + operateResult.Message);
			pipe?.CloseCommunication();
			return;
		}
		base.LogNet?.WriteInfo(ToString(), $"Connect [{_hostIp}:{_port}] success");
		ForwardSession forwardSession = new ForwardSession(pipe, endPoint, CacheSize);
		forwardSession.ServerSocket = operateResult.Content;
		try
		{
			forwardSession.ServerSocket.BeginReceive(forwardSession.ServerBuffer, 0, forwardSession.ServerBuffer.Length, SocketFlags.None, ServerReceiveAsync, forwardSession);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteError(ToString(), "Server begin receive failed, local client close. " + ex.Message);
			forwardSession.Close();
			return;
		}
		try
		{
			PipeTcpNet pipeTcpNet = forwardSession.Communication as PipeTcpNet;
			pipeTcpNet.Socket.BeginReceive(forwardSession.BytesBuffer, 0, forwardSession.BytesBuffer.Length, SocketFlags.None, LocalReceiveAsync, forwardSession);
		}
		catch (Exception ex2)
		{
			base.LogNet?.WriteError(ToString(), "Local begin receive failed, server close. " + ex2.Message);
			forwardSession.Close();
			return;
		}
		AddSession(forwardSession);
	}

	private void ServerReceiveAsync(IAsyncResult ar)
	{
		if (!(ar.AsyncState is ForwardSession forwardSession))
		{
			return;
		}
		forwardSession.HeartTime = DateTime.Now;
		int num = 0;
		try
		{
			num = forwardSession.ServerSocket.EndReceive(ar);
		}
		catch (ObjectDisposedException)
		{
			RemoveSession(forwardSession, string.Empty);
		}
		catch (Exception ex2)
		{
			RemoveSession(forwardSession, "Server socket endreceive failed: " + ex2.Message);
			return;
		}
		if (num == 0)
		{
			RemoveSession(forwardSession, $"Server socket [{_hostIp}:{_port}], local closed");
			return;
		}
		byte[] array = forwardSession.ServerBuffer.SelectBegin(num);
		LogBuffer("Remote->Client", array);
		this.OnRemoteMessageReceived?.Invoke(forwardSession, array);
		try
		{
			forwardSession.Communication.Send(array);
		}
		catch (Exception ex3)
		{
			RemoveSession(forwardSession, "Local send failed, server closed: " + ex3.Message);
			return;
		}
		try
		{
			forwardSession.ServerSocket.BeginReceive(forwardSession.ServerBuffer, 0, forwardSession.ServerBuffer.Length, SocketFlags.None, ServerReceiveAsync, forwardSession);
		}
		catch (Exception ex4)
		{
			RemoveSession(forwardSession, "Server socket beginReceive failed, local client close. " + ex4.Message);
		}
	}

	private void LocalReceiveAsync(IAsyncResult ar)
	{
		if (!(ar.AsyncState is ForwardSession forwardSession))
		{
			return;
		}
		forwardSession.HeartTime = DateTime.Now;
		PipeTcpNet pipeTcpNet = forwardSession.Communication as PipeTcpNet;
		int num = 0;
		try
		{
			num = pipeTcpNet.Socket.EndReceive(ar);
		}
		catch (Exception ex)
		{
			RemoveSession(forwardSession, "local socket endreceive failed: " + ex.Message);
			return;
		}
		if (num == 0)
		{
			RemoveSession(forwardSession, $"local socket closed[{forwardSession.IpEndPoint}], server[{_hostIp}:{_port}] closed");
			return;
		}
		byte[] array = forwardSession.BytesBuffer.SelectBegin(num);
		LogBuffer("Client->Remote", array);
		this.OnClientMessageReceive?.Invoke(forwardSession, array);
		try
		{
			forwardSession.ServerSocket.Send(array);
		}
		catch (Exception ex2)
		{
			RemoveSession(forwardSession, "Server send failed, local closed: " + ex2.Message);
			return;
		}
		try
		{
			pipeTcpNet.Socket.BeginReceive(forwardSession.BytesBuffer, 0, forwardSession.BytesBuffer.Length, SocketFlags.None, LocalReceiveAsync, forwardSession);
		}
		catch (Exception ex3)
		{
			RemoveSession(forwardSession, "local socket beginReceive failed, server socket close. " + ex3.Message);
		}
	}

	private void LogBuffer(string info, byte[] buffer)
	{
		base.LogNet?.WriteInfo(ToString(), "[" + info + "] " + (LogMsgFormatBinary ? SoftBasic.ByteToHexString(buffer, ' ') : SoftBasic.GetAsciiStringRender(buffer)));
	}

	public override string ToString()
	{
		return $"TcpForward[{base.Port}->{_hostIp}:{_port}]";
	}
}
