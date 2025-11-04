using HslCommunication.Core;
using HslCommunication.Core.Pipe;
using HslCommunication.Profinet.YASKAWA.Helper;

namespace HslCommunication.Profinet.YASKAWA;

public class MemobusUdpNet : MemobusTcpNet, IMemobus, IReadWriteDevice, IReadWriteNet
{
	public MemobusUdpNet()
	{
		CommunicationPipe = new PipeUdpNet();
	}

	public MemobusUdpNet(string ipAddress, int port = 502)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	public override string ToString()
	{
		return $"MemobusUdpNet[{IpAddress}:{Port}]";
	}
}
