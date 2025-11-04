using System;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Freedom;

public class FreedomSerial : DeviceSerialPort
{
	public Func<byte[], byte[], OperateResult> CheckResponseStatus { get; set; }

	public FreedomSerial()
	{
		base.ByteTransform = new RegularByteTransform();
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

	public override string ToString()
	{
		return $"FreedomSerial<{base.ByteTransform.GetType()}>[{base.PortName}:{base.BaudRate}]";
	}
}
