using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.FATEK.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.FATEK;

public class FatekProgram : DeviceSerialPort, IFatekProgram, IReadWriteNet
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

	public FatekProgram()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
		LogMsgFormatBinary = false;
		base.ReceiveEmptyDataCount = 5;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FatekProgramMessage();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return FatekProgramHelper.Read(this, station, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return FatekProgramHelper.Write(this, station, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return FatekProgramHelper.ReadBool(this, station, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return FatekProgramHelper.Write(this, station, address, value);
	}

	public OperateResult Run(byte station)
	{
		return FatekProgramHelper.Run(this, station);
	}

	[HslMqttApi("Run", "使PLC处于RUN状态")]
	public OperateResult Run()
	{
		return Run(Station);
	}

	public OperateResult Stop(byte station)
	{
		return FatekProgramHelper.Stop(this, station);
	}

	[HslMqttApi("Stop", "使PLC处于STOP状态")]
	public OperateResult Stop()
	{
		return Stop(Station);
	}

	public OperateResult<bool[]> ReadStatus(byte station)
	{
		return FatekProgramHelper.ReadStatus(this, station);
	}

	[HslMqttApi("ReadStatus", "读取PLC基本的状态信息")]
	public OperateResult<bool[]> ReadStatus()
	{
		return ReadStatus(Station);
	}

	public override string ToString()
	{
		return $"FatekProgram[{base.PortName}:{base.BaudRate}]";
	}
}
