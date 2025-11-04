using HslCommunication.ModBus;

namespace HslCommunication.Profinet.XINJE;

public class XinJESerial : ModbusRtu
{
	public XinJESeries Series { get; set; }

	public XinJESerial()
	{
		Series = XinJESeries.XC;
	}

	public XinJESerial(byte station = 1)
		: base(station)
	{
		Series = XinJESeries.XC;
	}

	public XinJESerial(XinJESeries series, byte station = 1)
		: base(station)
	{
		Series = series;
	}

	public override OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode)
	{
		return XinJEHelper.PraseXinJEAddress(Series, address, modbusCode);
	}

	public override string ToString()
	{
		return $"XinJESerial<{Series}>[{base.PortName}:{base.BaudRate}]";
	}
}
