namespace HslCommunication.Profinet.AllenBradley;

public class AllenBradleyMicroCip : AllenBradleyNet
{
	public AllenBradleyMicroCip()
	{
	}

	public AllenBradleyMicroCip(string ipAddress, int port = 44818)
		: base(ipAddress, port)
	{
	}

	protected override byte[] PackCommandService(byte[] portSlot, params byte[][] cips)
	{
		return AllenBradleyHelper.PackCleanCommandService(portSlot, cips);
	}

	public override string ToString()
	{
		return $"AllenBradleyMicroCip[{IpAddress}:{Port}]";
	}
}
