using System.Threading.Tasks;
using HslCommunication.ModBus;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.MegMeet;

public class MegMeetSerialOverTcp : ModbusRtuOverTcp
{
	public MegMeetSerialOverTcp()
	{
	}

	public MegMeetSerialOverTcp(string ipAddress, int port = 502, byte station = 1)
		: base(ipAddress, port, station)
	{
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return MegMeetHelper.ReadBool(base.ReadBool, address, length);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return MegMeetHelper.Read(base.Read, address, length);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await MegMeetHelper.ReadBoolAsync((string address2, ushort length2) => base.ReadBoolAsync(address2, length2), address, length);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await MegMeetHelper.ReadAsync((string address2, ushort length2) => base.ReadAsync(address2, length2), address, length);
	}

	public override OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode)
	{
		return MegMeetHelper.PraseMegMeetAddress(address, modbusCode);
	}

	public override string ToString()
	{
		return $"MegMeetSerialOverTcp[{IpAddress}:{Port}]";
	}
}
