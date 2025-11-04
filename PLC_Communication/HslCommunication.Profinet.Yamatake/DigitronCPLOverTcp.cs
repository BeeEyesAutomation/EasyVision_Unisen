using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Yamatake.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Yamatake;

public class DigitronCPLOverTcp : DeviceTcpNet
{
	public byte Station { get; set; }

	public DigitronCPLOverTcp()
	{
		Station = 1;
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		LogMsgFormatBinary = false;
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

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", Station);
		OperateResult<byte[]> command = DigitronCPLHelper.BuildReadCommand(station, address, length);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return DigitronCPLHelper.ExtraActualResponse(read.Content);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", Station);
		OperateResult<byte[]> command = DigitronCPLHelper.BuildWriteCommand(station, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return DigitronCPLHelper.ExtraActualResponse(read.Content);
	}

	public override string ToString()
	{
		return $"DigitronCPLOverTcp[{IpAddress}:{Port}]";
	}
}
