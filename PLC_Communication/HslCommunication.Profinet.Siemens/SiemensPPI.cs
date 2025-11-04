using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Siemens.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Siemens;

public class SiemensPPI : DeviceSerialPort, ISiemensPPI, IReadWriteNet
{
	private byte station = 2;

	private object communicationLock;

	[HslMqttApi]
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

	public SiemensPPI()
	{
		base.ByteTransform = new ReverseBytesTransform();
		base.WordLength = 2;
		communicationLock = new object();
		base.ReceiveEmptyDataCount = 5;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SiemensPPIMessage();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return SiemensPPIHelper.Read(this, address, length, Station, communicationLock);
	}

	[HslMqttApi("ReadBool", "")]
	public override OperateResult<bool> ReadBool(string address)
	{
		return SiemensPPIHelper.ReadBool(this, address, Station, communicationLock);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return SiemensPPIHelper.ReadBool(this, address, length, Station, communicationLock);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return SiemensPPIHelper.Write(this, address, value, Station, communicationLock);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return SiemensPPIHelper.Write(this, address, value, Station, communicationLock);
	}

	[HslMqttApi("ReadByte", "")]
	public OperateResult<byte> ReadByte(string address)
	{
		return ByteTransformHelper.GetResultFromArray(Read(address, 1));
	}

	[HslMqttApi("WriteByte", "向西门子的PLC中写入byte数据，地址为\"M100\",\"AI100\",\"I0\",\"Q0\",\"V100\",\"S100\"等，详细请参照API文档")]
	public OperateResult Write(string address, byte value)
	{
		return Write(address, new byte[1] { value });
	}

	public override async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		return await Task.Run(() => ReadBool(address));
	}

	[HslMqttApi]
	public OperateResult Start(string parameter = "")
	{
		return SiemensPPIHelper.Start(this, parameter, Station, communicationLock);
	}

	[HslMqttApi]
	public OperateResult Stop(string parameter = "")
	{
		return SiemensPPIHelper.Stop(this, parameter, Station, communicationLock);
	}

	[HslMqttApi]
	public OperateResult<string> ReadPlcType(string parameter = "")
	{
		return SiemensPPIHelper.ReadPlcType(this, parameter, Station, communicationLock);
	}

	public override string ToString()
	{
		return $"SiemensPPI[{base.PortName}:{base.BaudRate}]";
	}
}
