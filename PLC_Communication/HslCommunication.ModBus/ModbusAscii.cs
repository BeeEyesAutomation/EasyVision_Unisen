using HslCommunication.Core.IMessage;

namespace HslCommunication.ModBus;

public class ModbusAscii : ModbusRtu
{
	public ModbusAscii()
	{
		LogMsgFormatBinary = false;
		base.ReceiveEmptyDataCount = 5;
	}

	public ModbusAscii(byte station = 1)
		: base(station)
	{
		LogMsgFormatBinary = false;
		base.ReceiveEmptyDataCount = 5;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new ModbusAsciiMessage();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return ModbusInfo.TransModbusCoreToAsciiPackCommand(command);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return ModbusHelper.ExtraAsciiResponseContent(send, response, base.BroadcastStation);
	}

	public override string ToString()
	{
		return $"ModbusAscii[{base.PortName}:{base.BaudRate}]";
	}
}
