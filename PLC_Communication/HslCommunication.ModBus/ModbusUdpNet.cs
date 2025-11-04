using HslCommunication.Core.Pipe;

namespace HslCommunication.ModBus;

public class ModbusUdpNet : ModbusTcpNet
{
	public ModbusUdpNet()
	{
		CommunicationPipe = new PipeUdpNet();
	}

	public ModbusUdpNet(string ipAddress, int port = 502, byte station = 1)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
		base.Station = station;
	}

	public override string ToString()
	{
		return $"ModbusUdpNet[{IpAddress}:{Port}]";
	}
}
