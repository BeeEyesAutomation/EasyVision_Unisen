using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.ModBus;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Inovance;

public class InovanceTcpNet : ModbusTcpNet
{
	public InovanceSeries Series { get; set; }

	public InovanceTcpNet()
	{
		Series = InovanceSeries.AM;
		base.DataFormat = DataFormat.CDAB;
	}

	public InovanceTcpNet(string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
	{
		Series = InovanceSeries.AM;
		base.DataFormat = DataFormat.CDAB;
	}

	public InovanceTcpNet(InovanceSeries series, string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
	{
		Series = series;
		base.DataFormat = DataFormat.CDAB;
	}

	[HslMqttApi("ReadByte", "")]
	public OperateResult<byte> ReadByte(string address)
	{
		return InovanceHelper.ReadByte(this, address);
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		return await InovanceHelper.ReadByteAsync(this, address);
	}

	public override OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode)
	{
		return InovanceHelper.PraseInovanceAddress(Series, address, modbusCode);
	}

	public override OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		if (Series == InovanceSeries.AM && Regex.IsMatch(address, "MB[0-9]*[13579]$", RegexOptions.IgnoreCase))
		{
			return InovanceHelper.ReadAMString(this, address, length, encoding);
		}
		return base.ReadString(address, length, encoding);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		if (Series == InovanceSeries.AM && Regex.IsMatch(address, "MB[0-9]*[13579]$", RegexOptions.IgnoreCase))
		{
			return await InovanceHelper.ReadAMStringAsync(this, address, length, encoding);
		}
		return await base.ReadStringAsync(address, length, encoding);
	}

	public override string ToString()
	{
		return $"InovanceTcpNet<{Series}>[{IpAddress}:{Port}]";
	}
}
