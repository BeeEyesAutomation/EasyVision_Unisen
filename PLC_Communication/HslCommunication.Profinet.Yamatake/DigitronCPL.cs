using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Yamatake.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Yamatake;

public class DigitronCPL : DeviceSerialPort
{
	public byte Station { get; set; }

	public DigitronCPL()
	{
		Station = 1;
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		LogMsgFormatBinary = false;
		base.ReceiveEmptyDataCount = 5;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(13, 10);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", Station);
		OperateResult<byte[]> operateResult = DigitronCPLHelper.BuildReadCommand(station, address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return DigitronCPLHelper.ExtraActualResponse(operateResult2.Content);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", Station);
		OperateResult<byte[]> operateResult = DigitronCPLHelper.BuildWriteCommand(station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return DigitronCPLHelper.ExtraActualResponse(operateResult2.Content);
	}

	public override string ToString()
	{
		return $"DigitronCPL[{base.PortName}:{base.BaudRate}]";
	}
}
