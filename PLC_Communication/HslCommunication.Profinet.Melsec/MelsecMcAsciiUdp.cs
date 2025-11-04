using HslCommunication.Core;
using HslCommunication.Core.Pipe;
using HslCommunication.Profinet.Melsec.Helper;

namespace HslCommunication.Profinet.Melsec;

public class MelsecMcAsciiUdp : MelsecMcAsciiNet, IReadWriteMc, IReadWriteDevice, IReadWriteNet
{
	public MelsecMcAsciiUdp()
		: this("127.0.0.1", 6000)
	{
	}

	public MelsecMcAsciiUdp(string ipAddress, int port)
	{
		base.WordLength = 1;
		LogMsgFormatBinary = false;
		base.ByteTransform = new RegularByteTransform();
		CommunicationPipe = new PipeUdpNet();
		IpAddress = ipAddress;
		Port = port;
	}

	public override string ToString()
	{
		return $"MelsecMcAsciiUdp[{IpAddress}:{Port}]";
	}
}
