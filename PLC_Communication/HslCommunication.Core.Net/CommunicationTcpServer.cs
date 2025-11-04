using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using HslCommunication.Core.Pipe;
using HslCommunication.LogNet;

namespace HslCommunication.Core.Net;

public class CommunicationTcpServer
{
	private List<string> TrustedClients = null;

	private bool IsTrustedClientsOnly = false;

	private object lock_trusted_clients = new object();

	protected Socket socketServer = null;

	private bool useSSL = false;

	private X509Certificate certificate;

	private Action<PipeSslNet> pipeSslNetAction = null;

	private AsyncCallback beginAcceptCallback = null;

	public bool IsStarted { get; protected set; }

	public int Port { get; set; } = 10000;

	public bool EnableIPv6 { get; set; }

	public int SocketKeepAliveTime { get; set; } = -1;

	public ILogNet LogNet { get; set; }

	public Action<string> LogDebugMessage { get; set; }

	public CommunicationTcpServer()
	{
		beginAcceptCallback = AsyncAcceptCallback;
	}

	public void UseSSL(X509Certificate cert)
	{
		useSSL = true;
		certificate = cert;
	}

	public void UseSSL(string cert, string password = "")
	{
		useSSL = true;
		certificate = (string.IsNullOrEmpty(password) ? new X509Certificate(cert) : new X509Certificate(cert, password));
	}

	public void SetSslPipeAction(Action<PipeSslNet> action)
	{
		pipeSslNetAction = action;
	}

	public void ServerStart(int port)
	{
		if (!IsStarted)
		{
			if (!EnableIPv6)
			{
				socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socketServer.Bind(new IPEndPoint(IPAddress.Any, port));
			}
			else
			{
				socketServer = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
				socketServer.Bind(new IPEndPoint(IPAddress.IPv6Any, port));
			}
			socketServer.Listen(500);
			socketServer.BeginAccept(beginAcceptCallback, socketServer);
			IsStarted = true;
			Port = port;
			ExtraOnStart();
			LogDebugMsg(StringResources.Language.NetEngineStart);
		}
	}

	public void ServerStart()
	{
		ServerStart(Port);
	}

	public void ServerClose()
	{
		if (IsStarted)
		{
			IsStarted = false;
			ExtraOnClose();
			NetSupport.CloseSocket(socketServer);
			LogDebugMsg(StringResources.Language.NetEngineClose);
		}
	}

	public void SetTrustedIpAddress(List<string> clients)
	{
		lock (lock_trusted_clients)
		{
			if (clients != null && clients.Count > 0)
			{
				TrustedClients = clients.Select((string m) => HslHelper.GetIpAddressFromInput(m)).ToList();
				IsTrustedClientsOnly = true;
			}
			else
			{
				TrustedClients = new List<string>();
				IsTrustedClientsOnly = false;
			}
		}
	}

	private bool CheckIpAddressTrusted(string ipAddress)
	{
		if (IsTrustedClientsOnly)
		{
			bool result = false;
			lock (lock_trusted_clients)
			{
				for (int i = 0; i < TrustedClients.Count; i++)
				{
					if (TrustedClients[i] == ipAddress)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
		return false;
	}

	public string[] GetTrustedClients()
	{
		string[] result = new string[0];
		lock (lock_trusted_clients)
		{
			if (TrustedClients != null)
			{
				result = TrustedClients.ToArray();
			}
		}
		return result;
	}

	private void AsyncAcceptCallback(IAsyncResult iar)
	{
		if (!(iar.AsyncState is Socket socket))
		{
			return;
		}
		Socket socket2 = null;
		try
		{
			socket2 = socket.EndAccept(iar);
			if (SocketKeepAliveTime > 0)
			{
				socket2.SetKeepAlive(SocketKeepAliveTime, SocketKeepAliveTime);
			}
			ThreadPool.QueueUserWorkItem(ThreadPoolLogin, socket2);
		}
		catch (ObjectDisposedException)
		{
			return;
		}
		catch (SocketException ex2)
		{
			if (!IsStarted && ex2.SocketErrorCode == SocketError.OperationAborted)
			{
				return;
			}
			NetSupport.CloseSocket(socket2);
			LogDebugMsg(StringResources.Language.SocketAcceptCallbackException + " " + ex2.Message);
		}
		catch (Exception ex3)
		{
			NetSupport.CloseSocket(socket2);
			LogDebugMsg(StringResources.Language.SocketAcceptCallbackException + " " + ex3.Message);
		}
		int num = 0;
		while (num < 3)
		{
			if (!IsStarted)
			{
				NetSupport.CloseSocket(socket2);
				return;
			}
			try
			{
				socket.BeginAccept(beginAcceptCallback, socket);
			}
			catch (ObjectDisposedException)
			{
				if (!IsStarted)
				{
					return;
				}
				continue;
			}
			catch (Exception ex5)
			{
				HslHelper.ThreadSleep(100);
				LogDebugMsg(StringResources.Language.SocketReAcceptCallbackException + " " + ex5.Message);
				num++;
				continue;
			}
			break;
		}
		if (num < 3)
		{
			return;
		}
		LogDebugMsg(StringResources.Language.SocketReAcceptCallbackException);
		throw new Exception(StringResources.Language.SocketReAcceptCallbackException);
	}

	private void ThreadPoolLogin(object obj)
	{
		if (!(obj is Socket socket))
		{
			return;
		}
		IPEndPoint iPEndPoint = (IPEndPoint)socket.RemoteEndPoint;
		if (IsTrustedClientsOnly)
		{
			string ipAddress = ((iPEndPoint == null) ? string.Empty : iPEndPoint.Address.ToString());
			if (!CheckIpAddressTrusted(ipAddress))
			{
				LogDebugMsg(string.Format(StringResources.Language.ClientDisableLogin, iPEndPoint));
				NetSupport.CloseSocket(socket);
				return;
			}
		}
		OperateResult operateResult = SocketAcceptExtraCheck(socket, iPEndPoint);
		if (!operateResult.IsSuccess)
		{
			LogDebugMsg($"Client <{iPEndPoint}> SocketAcceptExtraCheck failed: {operateResult.Message}");
			NetSupport.CloseSocket(socket);
			return;
		}
		PipeTcpNet pipeTcpNet = null;
		if (useSSL)
		{
			PipeSslNet pipeSslNet = new PipeSslNet(socket, iPEndPoint, serverMode: true);
			pipeSslNet.Certificate = certificate;
			pipeSslNetAction?.Invoke(pipeSslNet);
			OperateResult<SslStream> operateResult2 = pipeSslNet.CreateSslStream(socket, createNew: true);
			if (!operateResult2.IsSuccess)
			{
				pipeSslNet.CloseCommunication();
				LogNet?.WriteDebug(ToString(), $"[{iPEndPoint}] WebScoket SSL Check Failed:" + operateResult2.Message);
				return;
			}
			pipeTcpNet = pipeSslNet;
		}
		else
		{
			pipeTcpNet = new PipeTcpNet(socket, iPEndPoint);
		}
		ThreadPoolLogin(pipeTcpNet, iPEndPoint);
	}

	protected virtual void ThreadPoolLogin(PipeTcpNet pipe, IPEndPoint endPoint)
	{
	}

	protected virtual void ExtraOnClose()
	{
	}

	protected virtual void ExtraOnStart()
	{
	}

	protected virtual OperateResult SocketAcceptExtraCheck(Socket socket, IPEndPoint endPoint)
	{
		return OperateResult.CreateSuccessResult();
	}

	protected void LogDebugMsg(string message)
	{
		LogNet?.WriteDebug(ToString(), message);
		LogDebugMessage?.Invoke(message);
	}
}
