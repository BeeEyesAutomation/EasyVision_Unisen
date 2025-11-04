using System.Threading.Tasks;
using HslCommunication.ModBus;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.XINJE;

public class XinJETcpNet : ModbusTcpNet
{
	public XinJESeries Series { get; set; }

	public XinJETcpNet()
	{
		Series = XinJESeries.XC;
	}

	public XinJETcpNet(string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
	{
		Series = XinJESeries.XC;
	}

	public XinJETcpNet(XinJESeries series, string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
	{
		Series = series;
	}

	public override OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode)
	{
		return XinJEHelper.PraseXinJEAddress(Series, address, modbusCode);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return XinJEHelper.Read(this, address, length, base.Read);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return XinJEHelper.Write(this, address, value, base.Write);
	}

	[HslMqttApi("WriteInt16", "")]
	public override OperateResult Write(string address, short value)
	{
		return XinJEHelper.Write(this, address, value, base.Write);
	}

	[HslMqttApi("WriteUInt16", "")]
	public override OperateResult Write(string address, ushort value)
	{
		return XinJEHelper.Write(this, address, value, base.Write);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return XinJEHelper.ReadBool(this, address, length, base.ReadBool);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] values)
	{
		return XinJEHelper.Write(this, address, values, base.Write);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return XinJEHelper.Write(this, address, value, base.Write);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await XinJEHelper.ReadAsync(this, address, length, (string address2, ushort length2) => base.ReadAsync(address2, length2));
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await XinJEHelper.WriteAsync(this, address, value, (string address2, byte[] value2) => base.WriteAsync(address2, value2));
	}

	public override async Task<OperateResult> WriteAsync(string address, short value)
	{
		return await XinJEHelper.WriteAsync(this, address, value, (string address2, short value2) => base.WriteAsync(address2, value2));
	}

	public override async Task<OperateResult> WriteAsync(string address, ushort value)
	{
		return await XinJEHelper.WriteAsync(this, address, value, (string address2, ushort value2) => base.WriteAsync(address2, value2));
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await XinJEHelper.ReadBoolAsync(this, address, length, (string address2, ushort length2) => base.ReadBoolAsync(address2, length2));
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		return await XinJEHelper.WriteAsync(this, address, values, (string address2, bool[] values2) => base.WriteAsync(address2, values2));
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await XinJEHelper.WriteAsync(this, address, value, (string address2, bool value2) => base.WriteAsync(address2, value2));
	}

	public override string ToString()
	{
		return $"XinJETcpNet<{Series}>[{IpAddress}:{Port}]";
	}
}
