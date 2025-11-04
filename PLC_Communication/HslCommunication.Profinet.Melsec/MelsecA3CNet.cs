using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Profinet.Melsec.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecA3CNet : DeviceSerialPort, IReadWriteA3C, IReadWriteDevice, IReadWriteNet
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

	public MelsecA3CNet()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
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

	public override string ToString()
	{
		return $"MelsecA3CNet[{base.PortName}:{base.BaudRate}]";
	}
}
