using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Vigor.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Vigor;

public class VigorSerialOverTcp : DeviceTcpNet
{
	public byte Station { get; set; }

	public VigorSerialOverTcp()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
	}

	public VigorSerialOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new VigorSerialMessage();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return VigorHelper.Read(this, Station, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return VigorHelper.Write(this, Station, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return VigorHelper.ReadBool(this, Station, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return VigorHelper.Write(this, Station, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await VigorHelper.ReadAsync(this, Station, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await VigorHelper.WriteAsync(this, Station, address, value);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await VigorHelper.ReadBoolAsync(this, Station, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await VigorHelper.WriteAsync(this, Station, address, value);
	}

	public override string ToString()
	{
		return $"VigorSerialOverTcp[{IpAddress}:{Port}]";
	}
}
