using HslCommunication.Core;
using HslCommunication.Core.Pipe;
using HslCommunication.Profinet.Melsec.Helper;

namespace HslCommunication.Profinet.Melsec;

public class MelsecMcUdp : MelsecMcNet, IReadWriteMc, IReadWriteDevice, IReadWriteNet
{
	public MelsecMcUdp()
		: this("127.0.0.1", 6000)
	{
	}

	public MelsecMcUdp(string ipAddress, int port)
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		CommunicationPipe = new PipeUdpNet();
		IpAddress = ipAddress;
		Port = port;
	}

	public override string ToString()
	{
		return $"MelsecMcUdp[{IpAddress}:{Port}]";
	}
}
