using System;
using System.Net;
using System.Net.Sockets;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Enthernet;

public class ForwardSession : PipeSession
{
	public Socket ServerSocket { get; set; }

	public byte[] ServerBuffer { get; set; }

	public byte[] BytesBuffer { get; set; }

	public IPEndPoint IpEndPoint { get; set; }

	public ForwardSession()
	{
		ServerBuffer = new byte[2048];
		BytesBuffer = new byte[2048];
	}

	public ForwardSession(PipeTcpNet pipe, IPEndPoint endPoint, int cacheSize = 2048)
	{
		base.Communication = pipe;
		IpEndPoint = endPoint;
		ServerBuffer = new byte[cacheSize];
		BytesBuffer = new byte[cacheSize];
	}

	public override void Close()
	{
		base.Close();
		NetSupport.CloseSocket(ServerSocket);
	}

	public override string ToString()
	{
		return $"Server[{ServerSocket.RemoteEndPoint}] Local[{IpEndPoint}] Online:{SoftBasic.GetTimeSpanDescription(DateTime.Now - base.OnlineTime)}";
	}
}
