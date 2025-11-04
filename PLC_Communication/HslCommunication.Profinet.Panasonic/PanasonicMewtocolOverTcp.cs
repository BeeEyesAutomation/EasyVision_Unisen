using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Panasonic.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Panasonic;

public class PanasonicMewtocolOverTcp : DeviceTcpNet
{
	public byte Station { get; set; }

	public PanasonicMewtocolOverTcp(byte station = 238)
	{
		base.ByteTransform = new RegularByteTransform();
		Station = station;
		base.ByteTransform.DataFormat = DataFormat.DCBA;
		base.WordLength = 1;
		LogMsgFormatBinary = false;
	}

	public PanasonicMewtocolOverTcp(string ipAddress, int port, byte station = 238)
		: this(station)
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(13);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return MewtocolHelper.Read(this, Station, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return MewtocolHelper.Write(this, Station, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await MewtocolHelper.ReadAsync(this, Station, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await MewtocolHelper.WriteAsync(this, Station, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return MewtocolHelper.ReadBool(this, Station, address, length);
	}

	[HslMqttApi("ReadBool", "")]
	public override OperateResult<bool> ReadBool(string address)
	{
		return MewtocolHelper.ReadBool(this, Station, address);
	}

	public OperateResult<bool[]> ReadBool(string[] address)
	{
		return MewtocolHelper.ReadBool(this, Station, address);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] values)
	{
		return MewtocolHelper.Write(this, Station, address, values);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return MewtocolHelper.Write(this, Station, address, value);
	}

	public OperateResult Write(string[] address, bool[] value)
	{
		return MewtocolHelper.Write(this, Station, address, value);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await MewtocolHelper.ReadBoolAsync(this, Station, address, length);
	}

	public override async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		return await MewtocolHelper.ReadBoolAsync(this, Station, address);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		return await MewtocolHelper.WriteAsync(this, Station, address, values);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await MewtocolHelper.WriteAsync(this, Station, address, value);
	}

	public async Task<OperateResult<bool[]>> ReadBoolAsync(string[] address)
	{
		return await MewtocolHelper.ReadBoolAsync(this, Station, address);
	}

	public async Task<OperateResult> WriteAsync(string[] address, bool[] value)
	{
		return await MewtocolHelper.WriteAsync(this, Station, address, value);
	}

	public OperateResult<string> ReadPlcType()
	{
		return MewtocolHelper.ReadPlcType(this, Station);
	}

	public override string ToString()
	{
		return $"PanasonicMewtocolOverTcp[{IpAddress}:{Port}]";
	}
}
