using HslCommunication.Core;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Profinet.Freedom;

public class FreedomUdpNet : FreedomTcpNet
{
	public FreedomUdpNet()
	{
		base.ByteTransform = new RegularByteTransform();
		CommunicationPipe = new PipeUdpNet();
	}

	public FreedomUdpNet(string ipAddress, int port)
	{
		CommunicationPipe = new PipeUdpNet();
		IpAddress = ipAddress;
		Port = port;
		base.ByteTransform = new RegularByteTransform();
	}

	public override string ToString()
	{
		return $"FreedomUdpNet<{base.ByteTransform.GetType()}>[{IpAddress}:{Port}]";
	}
}
