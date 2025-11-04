using System;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Freedom;

public class FreedomTcpNet : DeviceTcpNet
{
	public Func<byte[], byte[], OperateResult> CheckResponseStatus { get; set; }

	public INetMessage NetMessage { get; set; }

	public FreedomTcpNet()
	{
		base.ByteTransform = new RegularByteTransform();
	}

	public FreedomTcpNet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return NetMessage;
	}

	[HslMqttApi("ReadByteArray", "特殊的地址格式，需要采用解析包起始地址的报文，例如 modbus 协议为 stx=9;00 00 00 00 00 06 01 03 00 64 00 01")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		int num = HslHelper.ExtractParameter(ref address, "stx", 0);
		byte[] array = address.ToHexBytes();
		OperateResult<byte[]> operateResult = ReadFromCoreServer(array);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (CheckResponseStatus != null)
		{
			OperateResult operateResult2 = CheckResponseStatus(array, operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult2);
			}
		}
		if (num >= operateResult.Content.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.ReceiveDataLengthTooShort);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.RemoveBegin(num));
	}

	public override OperateResult Write(string address, byte[] value)
	{
		return Read(address, 0);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		int startIndex = HslHelper.ExtractParameter(ref address, "stx", 0);
		byte[] send = address.ToHexBytes();
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(send);
		if (!read.IsSuccess)
		{
			return read;
		}
		if (CheckResponseStatus != null)
		{
			OperateResult check = CheckResponseStatus(send, read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(check);
			}
		}
		if (startIndex >= read.Content.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.ReceiveDataLengthTooShort);
		}
		return OperateResult.CreateSuccessResult(read.Content.RemoveBegin(startIndex));
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await ReadAsync(address, 0);
	}

	public override string ToString()
	{
		return $"FreedomTcpNet<{base.ByteTransform.GetType()}>[{IpAddress}:{Port}]";
	}
}
