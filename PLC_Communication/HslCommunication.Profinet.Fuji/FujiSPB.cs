using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Fuji;

public class FujiSPB : DeviceSerialPort
{
	private byte station = 1;

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

	public FujiSPB()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
		LogMsgFormatBinary = false;
		base.ReceiveEmptyDataCount = 5;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FujiSPBMessage();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return FujiSPBHelper.Read(this, station, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return FujiSPBHelper.Write(this, station, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return FujiSPBHelper.ReadBool(this, station, address, length);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return FujiSPBHelper.Write(this, station, address, value);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await FujiSPBHelper.WriteAsync(this, station, address, value);
	}

	public override string ToString()
	{
		return $"FujiSPB[{base.PortName}:{base.BaudRate}]";
	}
}
