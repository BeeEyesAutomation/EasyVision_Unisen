using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Profinet.Melsec.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecFxLinks : DeviceSerialPort, IReadWriteFxLinks, IReadWriteDevice, IReadWriteNet, IReadWriteDeviceStation
{
	private byte station = 0;

	private byte watiingTime = 0;

	private bool sumCheck = true;

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

	public byte WaittingTime
	{
		get
		{
			return watiingTime;
		}
		set
		{
			if (watiingTime > 15)
			{
				watiingTime = 15;
			}
			else
			{
				watiingTime = value;
			}
		}
	}

	public bool SumCheck
	{
		get
		{
			return sumCheck;
		}
		set
		{
			sumCheck = value;
		}
	}

	public int Format { get; set; } = 1;

	public MelsecFxLinks()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
		LogMsgFormatBinary = false;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new MelsecFxLinksMessage(Format, SumCheck);
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return MelsecFxLinksHelper.PackCommandWithHeader(this, command);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return MelsecFxLinksHelper.Read(this, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return MelsecFxLinksHelper.Write(this, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return MelsecFxLinksHelper.ReadBool(this, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return MelsecFxLinksHelper.Write(this, address, value);
	}

	[HslMqttApi(Description = "Start the PLC operation, you can carry additional parameter information and specify the station number. Example: s=2; Note: The semicolon is required.")]
	public OperateResult StartPLC(string parameter = "")
	{
		return MelsecFxLinksHelper.StartPLC(this, parameter);
	}

	[HslMqttApi(Description = "Stop PLC operation, you can carry additional parameter information and specify the station number. Example: s=2; Note: The semicolon is required.")]
	public OperateResult StopPLC(string parameter = "")
	{
		return MelsecFxLinksHelper.StopPLC(this, parameter);
	}

	[HslMqttApi(Description = "Read the PLC model information, you can carry additional parameter information, and specify the station number. Example: s=2; Note: The semicolon is required.")]
	public OperateResult<string> ReadPlcType(string parameter = "")
	{
		return MelsecFxLinksHelper.ReadPlcType(this, parameter);
	}

	public override string ToString()
	{
		return $"MelsecFxLinks[{base.PortName}:{base.BaudRate}]";
	}
}
