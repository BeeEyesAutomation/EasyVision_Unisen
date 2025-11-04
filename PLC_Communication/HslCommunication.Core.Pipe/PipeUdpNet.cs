using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using HslCommunication.Core.IMessage;

namespace HslCommunication.Core.Pipe;

public class PipeUdpNet : PipeTcpNet
{
	public int ReceiveCacheLength { get; set; } = 2048;

	public PipeUdpNet()
	{
	}

	public PipeUdpNet(string ipAddress, int port)
	{
		base.IpAddress = ipAddress;
		base.Port = port;
	}

	public override OperateResult<bool> OpenCommunication()
	{
		if (IsConnectError())
		{
			try
			{
				IPEndPoint connectIPEndPoint = GetConnectIPEndPoint();
				Socket socket = new Socket(connectIPEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
				if (base.LocalBinding != null)
				{
					socket.Bind(base.LocalBinding);
				}
				base.Socket = socket;
				ResetConnectErrorCount();
				return OperateResult.CreateSuccessResult(value: true);
			}
			catch (Exception ex)
			{
				CloseCommunication();
				return new OperateResult<bool>(-IncrConnectErrorCount(), ex.Message);
			}
		}
		return OperateResult.CreateSuccessResult(value: false);
	}

	public override async Task<OperateResult<bool>> OpenCommunicationAsync()
	{
		return await Task.Run(() => OpenCommunication()).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override OperateResult Send(byte[] data, int offset, int size)
	{
		IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(base.IpAddress), base.Port);
		try
		{
			base.Socket.SendTo(data, offset, size, SocketFlags.None, remoteEP);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			CloseCommunication();
			return new OperateResult<byte[]>(-IncrConnectErrorCount(), ex.Message);
		}
	}

	public override async Task<OperateResult> SendAsync(byte[] data, int offset, int size)
	{
		return await Task.Run(() => Send(data, offset, size)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override OperateResult<int> Receive(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		try
		{
			base.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, base.ReceiveTimeOut);
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(base.IpAddress), base.Port);
			IPEndPoint iPEndPoint2 = new IPEndPoint((iPEndPoint.AddressFamily == AddressFamily.InterNetworkV6) ? IPAddress.IPv6Any : IPAddress.Any, 0);
			EndPoint remoteEP = iPEndPoint2;
			if (length > 0)
			{
				int value = base.Socket.ReceiveFrom(buffer, offset, length, SocketFlags.None, ref remoteEP);
				return OperateResult.CreateSuccessResult(value);
			}
			int value2 = base.Socket.ReceiveFrom(buffer, offset, buffer.Length - offset, SocketFlags.None, ref remoteEP);
			return OperateResult.CreateSuccessResult(value2);
		}
		catch (Exception ex)
		{
			CloseCommunication();
			return new OperateResult<int>(-IncrConnectErrorCount(), "Socket Exception -> " + ex.Message);
		}
	}

	public override async Task<OperateResult<int>> ReceiveAsync(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		return await Task.Run(() => Receive(buffer, offset, length, timeOut, reportProgress)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override OperateResult<byte[]> ReceiveMessage(INetMessage netMessage, byte[] sendValue, bool useActivePush = true, Action<long, long> reportProgress = null, Action<byte[]> logMessage = null)
	{
		if (base.UseServerActivePush)
		{
			return base.ReceiveMessage(netMessage, sendValue, useActivePush, reportProgress, logMessage);
		}
		return ReceiveMessage(netMessage, sendValue, null, logMessage);
	}

	public OperateResult<byte[]> ReceiveMessage(INetMessage netMessage, byte[] sendValue, byte[] alreadyReceive, Action<byte[]> logMessage = null, bool closeOnException = true)
	{
		try
		{
			MemoryStream ms = new MemoryStream();
			if (alreadyReceive != null && alreadyReceive.Length != 0)
			{
				ms.Write(alreadyReceive);
				if (CheckMessageComplete(netMessage, sendValue, ref ms))
				{
					return OperateResult.CreateSuccessResult(ms.ToArray());
				}
			}
			base.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, base.ReceiveTimeOut);
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(base.IpAddress), base.Port);
			IPEndPoint iPEndPoint2 = new IPEndPoint((iPEndPoint.AddressFamily == AddressFamily.InterNetworkV6) ? IPAddress.IPv6Any : IPAddress.Any, 0);
			EndPoint remoteEP = iPEndPoint2;
			OperateResult<byte[]> operateResult = NetSupport.CreateReceiveBuffer(ReceiveCacheLength);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			DateTime now = DateTime.Now;
			while (true)
			{
				int length = base.Socket.ReceiveFrom(operateResult.Content, ref remoteEP);
				byte[] array = operateResult.Content.SelectBegin(length);
				ms.Write(array);
				logMessage?.Invoke(array);
				if (netMessage == null || CheckMessageComplete(netMessage, sendValue, ref ms))
				{
					break;
				}
				if (base.ReceiveTimeOut >= 0 && (DateTime.Now - now).TotalMilliseconds > (double)base.ReceiveTimeOut)
				{
					return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + base.ReceiveTimeOut + " Received: " + ms.ToArray().ToHexString(' '));
				}
			}
			return OperateResult.CreateSuccessResult(ms.ToArray());
		}
		catch (Exception ex)
		{
			if (closeOnException)
			{
				CloseCommunication();
			}
			return new OperateResult<byte[]>(-IncrConnectErrorCount(), "Socket Exception -> " + ex.Message);
		}
	}

	public override async Task<OperateResult<byte[]>> ReceiveMessageAsync(INetMessage netMessage, byte[] sendValue, bool useActivePush = true, Action<long, long> reportProgress = null, Action<byte[]> logMessage = null)
	{
		return await Task.Run(() => ReceiveMessage(netMessage, sendValue, useActivePush, reportProgress, logMessage)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override string ToString()
	{
		return $"PipeUdpNet[{base.IpAddress}:{base.Port}]";
	}
}
