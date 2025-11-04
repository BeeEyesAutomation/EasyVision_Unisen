using System.Text.RegularExpressions;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Vigor.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Vigor;

public class VigorSerial : DeviceSerialPort
{
	public byte Station { get; set; }

	public VigorSerial()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
		base.ReceiveEmptyDataCount = 5;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new VigorSerialMessage();
	}

	protected override ushort GetWordLength(string address, int length, int dataTypeLength)
	{
		if (Regex.IsMatch(address, "^C2[0-9][0-9]$"))
		{
			int num = length * dataTypeLength * 2 / 4;
			return (ushort)((num == 0) ? 1 : ((ushort)num));
		}
		return base.GetWordLength(address, length, dataTypeLength);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return VigorHelper.Read(this, Station, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return VigorHelper.Write(this, Station, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return VigorHelper.ReadBool(this, Station, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return VigorHelper.Write(this, Station, address, value);
	}

	public override string ToString()
	{
		return $"VigorSerial[{base.PortName}:{base.BaudRate}]";
	}
}
