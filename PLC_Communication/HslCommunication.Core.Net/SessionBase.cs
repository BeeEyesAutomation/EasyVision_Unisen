using System;
using System.Net;
using System.Net.Sockets;

namespace HslCommunication.Core.Net;

public abstract class SessionBase
{
	public Socket WorkSocket { get; private set; }

	public DateTime OnlineTime { get; protected set; }

	public string IpAddress { get; protected set; }

	public IPEndPoint IpEndPoint { get; protected set; }

	public DateTime HeartTime { get; private set; } = DateTime.Now;

	public SessionBase()
	{
		OnlineTime = DateTime.Now;
	}

	public SessionBase(Socket socket)
		: this()
	{
		UpdateSocket(socket);
	}

	public void UpdateHeartTime()
	{
		HeartTime = DateTime.Now;
	}

	public void UpdateSocket(Socket socket)
	{
		if (socket == null)
		{
			return;
		}
		WorkSocket = socket;
		try
		{
			IpEndPoint = WorkSocket.RemoteEndPoint as IPEndPoint;
			IpAddress = ((IpEndPoint == null) ? string.Empty : IpEndPoint.Address.ToString());
		}
		catch
		{
		}
	}
}
