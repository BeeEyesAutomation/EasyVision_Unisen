using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Profinet.Melsec.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecA3CNetOverTcp : DeviceTcpNet, IReadWriteA3C, IReadWriteDevice, IReadWriteNet
{
	private byte station = 0;

	public byte Station
	{
		get
		{
			return station;
		}
		set
		{
			station = value;
		}
	}

	public bool SumCheck { get; set; } = true;

	public int Format { get; set; } = 1;

	public bool EnableWriteBitToWordRegister { get; set; }

	public MelsecA3CNetOverTcp()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		base.SleepTime = 20;
	}

	public MelsecA3CNetOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return MelsecA3CNetHelper.Read(this, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return MelsecA3CNetHelper.Write(this, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await MelsecA3CNetHelper.ReadAsync(this, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await MelsecA3CNetHelper.WriteAsync(this, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return MelsecA3CNetHelper.ReadBool(this, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return MelsecA3CNetHelper.Write(this, address, value);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await MelsecA3CNetHelper.ReadBoolAsync(this, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await MelsecA3CNetHelper.WriteAsync(this, address, value);
	}

	[HslMqttApi]
	public OperateResult RemoteRun()
	{
		return MelsecA3CNetHelper.RemoteRun(this);
	}

	[HslMqttApi]
	public OperateResult RemoteStop()
	{
		return MelsecA3CNetHelper.RemoteStop(this);
	}

	[HslMqttApi]
	public OperateResult<string> ReadPlcType()
	{
		return MelsecA3CNetHelper.ReadPlcType(this);
	}

	public async Task<OperateResult> RemoteRunAsync()
	{
		return await MelsecA3CNetHelper.RemoteRunAsync(this);
	}

	public async Task<OperateResult> RemoteStopAsync()
	{
		return await MelsecA3CNetHelper.RemoteStopAsync(this);
	}

	public async Task<OperateResult<string>> ReadPlcTypeAsync()
	{
		return await MelsecA3CNetHelper.ReadPlcTypeAsync(this);
	}

	public override string ToString()
	{
		return $"MelsecA3CNetOverTcp[{IpAddress}:{Port}]";
	}
}
