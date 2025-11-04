using System.Text;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetUdpClient : NetworkUdpBase
{
	public NetUdpClient(string ipAddress, int port)
	{
		IpAddress = ipAddress;
		Port = port;
	}

	public OperateResult<string> ReadFromServer(NetHandle customer, string send = null)
	{
		OperateResult<byte[]> operateResult = ReadFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(operateResult.Content));
	}

	public OperateResult<byte[]> ReadFromServer(NetHandle customer, byte[] send)
	{
		return ReadFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
	}

	public OperateResult<NetHandle, string> ReadCustomerFromServer(NetHandle customer, string send = null)
	{
		OperateResult<NetHandle, byte[]> operateResult = ReadCustomerFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<NetHandle, string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content1, Encoding.Unicode.GetString(operateResult.Content2));
	}

	public OperateResult<NetHandle, byte[]> ReadCustomerFromServer(NetHandle customer, byte[] send)
	{
		return ReadCustomerFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
	}

	private OperateResult<byte[]> ReadFromServerBase(byte[] send)
	{
		OperateResult<NetHandle, byte[]> operateResult = ReadCustomerFromServerBase(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content2);
	}

	private OperateResult<NetHandle, byte[]> ReadCustomerFromServerBase(byte[] send)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<NetHandle, byte[]>(operateResult);
		}
		return HslProtocol.ExtractHslData(operateResult.Content);
	}

	public override string ToString()
	{
		return $"NetUdpClient[{IpAddress}:{Port}]";
	}
}
