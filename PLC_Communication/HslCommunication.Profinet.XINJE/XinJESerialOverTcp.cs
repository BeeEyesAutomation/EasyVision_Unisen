using HslCommunication.ModBus;

namespace HslCommunication.Profinet.XINJE;

public class XinJESerialOverTcp : ModbusRtuOverTcp
{
	public XinJESeries Series { get; set; }

	public XinJESerialOverTcp()
	{
		Series = XinJESeries.XC;
	}

	public XinJESerialOverTcp(string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
	{
		Series = XinJESeries.XC;
	}

	public XinJESerialOverTcp(XinJESeries series, string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
	{
		Series = series;
	}

	public override OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode)
	{
		return XinJEHelper.PraseXinJEAddress(Series, address, modbusCode);
	}

	public override string ToString()
	{
		return $"XinJESerialOverTcp<{Series}>[{IpAddress}:{Port}]";
	}
}
