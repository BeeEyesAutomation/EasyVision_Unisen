using HslCommunication.Core.IMessage;

namespace HslCommunication.ModBus;

public class ModbusAsciiOverTcp : ModbusRtuOverTcp
{
	public ModbusAsciiOverTcp()
	{
		LogMsgFormatBinary = false;
	}

	public ModbusAsciiOverTcp(string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
	{
		LogMsgFormatBinary = false;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(13, 10);
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
		return $"ModbusAsciiOverTcp[{IpAddress}:{Port}]";
	}
}
