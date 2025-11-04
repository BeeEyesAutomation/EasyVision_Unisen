using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Core.Net;

public class NetworkUdpBase : NetworkBase
{
	protected bool LogMsgFormatBinary = true;

	private int connectErrorCount = 0;

	private PipeSocket pipeSocket;

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

	public int ReceiveTimeout { get; set; }

	public string ConnectionId { get; set; }

	public int ReceiveCacheLength { get; set; } = 2048;

	public IPEndPoint LocalBinding { get; set; }

	public NetworkUdpBase()
	{
		ReceiveTimeout = 5000;
		ConnectionId = SoftBasic.GetUniqueStringByGuidAndRandom();
		pipeSocket = new PipeSocket();
	}

	public virtual byte[] PackCommandWithHeader(byte[] command)
	{
		return command;
	}

	public virtual OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return OperateResult.CreateSuccessResult(response);
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(byte[] send)
	{
		return ReadFromCoreServer(send, hasResponseData: true, usePackAndUnpack: true);
	}

	public OperateResult<byte[]> ReadFromCoreServer(IEnumerable<byte[]> send)
	{
		return NetSupport.ReadFromCoreServer(send, ReadFromCoreServer);
	}

	protected virtual byte[] ReceiveFromUdpSocket(Socket socket, int timeOut, byte[] send)
	{
		socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, ReceiveTimeout);
		IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(IpAddress), Port);
		IPEndPoint iPEndPoint2 = new IPEndPoint((iPEndPoint.AddressFamily == AddressFamily.InterNetworkV6) ? IPAddress.IPv6Any : IPAddress.Any, 0);
		EndPoint remoteEP = iPEndPoint2;
		byte[] array = new byte[ReceiveCacheLength];
		int length = socket.ReceiveFrom(array, ref remoteEP);
		return array.SelectBegin(length);
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(byte[] send, bool hasResponseData, bool usePackAndUnpack)
	{
		if (!Authorization.nzugaydgwadawdibbas())
		{
			return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		}
		byte[] array = (usePackAndUnpack ? PackCommandWithHeader(send) : send);
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + (LogMsgFormatBinary ? SoftBasic.ByteToHexString(array) : Encoding.ASCII.GetString(array)));
		if (pipeSocket.LockingTick > HslHelper.LockLimit)
		{
			return new OperateResult<byte[]>(StringResources.Language.TooManyLock);
		}
		pipeSocket.PipeLockEnter();
		try
		{
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(IpAddress), Port);
			OperateResult<Socket> availableSocketAsync = GetAvailableSocketAsync(iPEndPoint);
			if (!availableSocketAsync.IsSuccess)
			{
				pipeSocket.PipeLockLeave();
				return OperateResult.CreateFailedResult<byte[]>(availableSocketAsync);
			}
			availableSocketAsync.Content.SendTo(array, array.Length, SocketFlags.None, iPEndPoint);
			if (ReceiveTimeout < 0)
			{
				pipeSocket.PipeLockLeave();
				return OperateResult.CreateSuccessResult(new byte[0]);
			}
			if (!hasResponseData)
			{
				pipeSocket.PipeLockLeave();
				return OperateResult.CreateSuccessResult(new byte[0]);
			}
			byte[] array2 = ReceiveFromUdpSocket(availableSocketAsync.Content, ReceiveTimeout, array);
			pipeSocket.PipeLockLeave();
			base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? SoftBasic.ByteToHexString(array2) : Encoding.ASCII.GetString(array2)));
			connectErrorCount = 0;
			pipeSocket.IsSocketError = false;
			try
			{
				return usePackAndUnpack ? UnpackResponseContent(array, array2) : OperateResult.CreateSuccessResult(array2);
			}
			catch (Exception ex)
			{
				return new OperateResult<byte[]>("UnpackResponseContent failed: " + ex.Message);
			}
		}
		catch (Exception ex2)
		{
			pipeSocket.ChangePorts();
			pipeSocket.IsSocketError = true;
			if (connectErrorCount < 100000000)
			{
				connectErrorCount++;
			}
			pipeSocket.PipeLockLeave();
			return new OperateResult<byte[]>(-connectErrorCount, ex2.Message);
		}
	}

	public async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(byte[] value)
	{
		return await Task.Run(() => ReadFromCoreServer(value));
	}

	public async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(IEnumerable<byte[]> send)
	{
		return await NetSupport.ReadFromCoreServerAsync(send, ReadFromCoreServerAsync);
	}

	public IPStatus IpAddressPing()
	{
		Ping ping = new Ping();
		return ping.Send(IpAddress).Status;
	}

	public void SetPipeSocket(PipeSocket pipeSocket)
	{
		if (this.pipeSocket != null)
		{
			this.pipeSocket = pipeSocket;
		}
	}

	public PipeSocket GetPipeSocket()
	{
		return pipeSocket;
	}

	private OperateResult<Socket> GetAvailableSocketAsync(IPEndPoint endPoint)
	{
		if (pipeSocket.IsConnectitonError())
		{
			OperateResult operateResult = null;
			try
			{
				pipeSocket.Socket?.Close();
				Socket socket = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
				if (LocalBinding != null)
				{
					socket.Bind(LocalBinding);
				}
				pipeSocket.Socket = socket;
				operateResult = OperateResult.CreateSuccessResult();
			}
			catch (Exception ex)
			{
				pipeSocket.IsSocketError = true;
				operateResult = new OperateResult(ex.Message);
			}
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

	public override string ToString()
	{
		return $"NetworkUdpBase[{IpAddress}:{Port}]";
	}
}
