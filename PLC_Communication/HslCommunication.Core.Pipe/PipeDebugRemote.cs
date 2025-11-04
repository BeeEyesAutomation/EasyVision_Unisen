using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using HslCommunication.Core.IMessage;

namespace HslCommunication.Core.Pipe;

public class PipeDebugRemote : PipeTcpNet
{
	public ushort ClientKey { get; set; } = 0;

	public PipeDebugRemote()
	{
	}

	public PipeDebugRemote(string ipAddress, int port)
	{
		base.IpAddress = ipAddress;
		base.Port = port;
	}

	public PipeDebugRemote(Socket socket, IPEndPoint iPEndPoint)
	{
		base.Socket = socket;
		base.IpAddress = iPEndPoint.Address.ToString();
		base.Port = iPEndPoint.Port;
	}

	private byte[] CreateHeaderBytes(byte[] sendValue)
	{
		byte[] array = new byte[6];
		BitConverter.GetBytes(ClientKey).CopyTo(array, 0);
		BitConverter.GetBytes(sendValue.Length).CopyTo(array, 2);
		return array;
	}

	public override OperateResult<byte[]> ReadFromCoreServer(INetMessage netMessage, byte[] sendValue, bool hasResponseData, Action<byte[]> logMessage = null)
	{
		Send(CreateHeaderBytes(sendValue));
		return base.ReadFromCoreServer(netMessage, sendValue, hasResponseData, logMessage);
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(INetMessage netMessage, byte[] sendValue, bool hasResponseData, Action<byte[]> logMessage = null)
	{
		await SendAsync(CreateHeaderBytes(sendValue));
		return await base.ReadFromCoreServerAsync(netMessage, sendValue, hasResponseData, logMessage);
	}

	public override string ToString()
	{
		return $"PipeTcpNet[{base.IpAddress}:{base.Port}]";
	}
}
