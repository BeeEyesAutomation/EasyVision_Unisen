using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Delta.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Delta;

public class DeltaSerialAsciiOverTcp : ModbusAsciiOverTcp, IDelta, IReadWriteDevice, IReadWriteNet
{
	public DeltaSeries Series { get; set; } = DeltaSeries.Dvp;

	public DeltaSerialAsciiOverTcp()
	{
		base.ByteTransform.DataFormat = DataFormat.CDAB;
	}

	public DeltaSerialAsciiOverTcp(string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
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

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await DeltaHelper.ReadBoolAsync(this, (string address2, ushort length2) => base.ReadBoolAsync(address2, length2), address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		return await DeltaHelper.WriteAsync(this, (string address2, bool[] values2) => base.WriteAsync(address2, values2), address, values);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await DeltaHelper.ReadAsync(this, (string address2, ushort length2) => base.ReadAsync(address2, length2), address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await DeltaHelper.WriteAsync(this, (string address2, byte[] value2) => base.WriteAsync(address2, value2), address, value);
	}

	public override string ToString()
	{
		return $"DeltaSerialAsciiOverTcp[{IpAddress}:{Port}]";
	}
}
