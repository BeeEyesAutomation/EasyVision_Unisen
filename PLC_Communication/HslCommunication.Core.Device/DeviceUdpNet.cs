using System.Net;
using System.Net.NetworkInformation;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Core.Device;

public class DeviceUdpNet : DeviceCommunication
{
	private PipeUdpNet pipe;

	public override CommunicationPipe CommunicationPipe
	{
		get
		{
			return base.CommunicationPipe;
		}
		set
		{
			base.CommunicationPipe = value;
			if (value is PipeUdpNet pipeUdpNet)
			{
				pipe = pipeUdpNet;
			}
		}
	}

	public virtual string IpAddress
	{
		get
		{
			return pipe.IpAddress;
		}
		set
		{
			pipe.IpAddress = value;
		}
	}

	public virtual int Port
	{
		get
		{
			return pipe.Port;
		}
		set
		{
			pipe.Port = value;
		}
	}

	public int ReceiveCacheLength
	{
		get
		{
			return pipe.ReceiveCacheLength;
		}
		set
		{
			pipe.ReceiveCacheLength = value;
		}
	}

	public IPEndPoint LocalBinding
	{
		get
		{
			return pipe.LocalBinding;
		}
		set
		{
			pipe.LocalBinding = value;
		}
	}

	public DeviceUdpNet()
		: this("127.0.0.1", 5000)
	{
	}

	public DeviceUdpNet(string ipAddress, int port)
	{
		CommunicationPipe = new PipeUdpNet
		{
			IpAddress = ipAddress,
			Port = port
		};
	}

	public IPStatus IpAddressPing()
	{
		Ping ping = new Ping();
		return ping.Send(IpAddress).Status;
	}

	public override string ToString()
	{
		return $"DeviceUdpNet<{base.ByteTransform}>{{{CommunicationPipe}}}";
	}
}
