using System.Threading.Tasks;
using HslCommunication.Profinet.AllenBradley;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecCipNet : AllenBradleyNet
{
	public MelsecCipNet()
	{
	}

	public MelsecCipNet(string ipAddress, int port = 44818)
		: base(ipAddress, port)
	{
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return Read(new string[1] { address }, new ushort[1] { length });
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await ReadAsync(new string[1] { address }, new ushort[1] { length });
	}
}
