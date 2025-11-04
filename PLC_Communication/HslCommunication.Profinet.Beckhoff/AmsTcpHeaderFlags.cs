namespace HslCommunication.Profinet.Beckhoff;

public enum AmsTcpHeaderFlags : ushort
{
	Command = 0,
	PortClose = 1,
	PortConnect = 4096,
	RouterNotification = 4097,
	GetLocalNetId = 4098
}
