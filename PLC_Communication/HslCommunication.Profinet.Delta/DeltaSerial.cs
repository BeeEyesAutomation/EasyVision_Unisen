using HslCommunication.Core;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Delta.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Delta;

public class DeltaSerial : ModbusRtu, IDelta, IReadWriteDevice, IReadWriteNet
{
	public DeltaSeries Series { get; set; } = DeltaSeries.Dvp;

	public DeltaSerial()
	{
		base.ByteTransform.DataFormat = DataFormat.CDAB;
	}

	public DeltaSerial(byte station = 1)
		: base(station)
	{
		base.ByteTransform.DataFormat = DataFormat.CDAB;
	}

	public override OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode)
	{
		return DeltaHelper.TranslateToModbusAddress(this, address, modbusCode);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return DeltaHelper.ReadBool(this, base.ReadBool, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] values)
	{
		return DeltaHelper.Write(this, base.Write, address, values);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return DeltaHelper.Read(this, base.Read, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return DeltaHelper.Write(this, base.Write, address, value);
	}

	public override string ToString()
	{
		return $"DeltaSerial[{base.PortName}:{base.BaudRate}]";
	}
}
