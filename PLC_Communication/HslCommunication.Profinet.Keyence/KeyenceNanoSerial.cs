using System;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Keyence;

public class KeyenceNanoSerial : DeviceSerialPort
{
	public byte Station { get; set; }

	public bool UseStation { get; set; }

	public KeyenceNanoSerial()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
		base.ByteTransform.IsStringReverseByteWord = true;
		LogMsgFormatBinary = false;
		base.ReceiveEmptyDataCount = 5;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new KeyenceNanoSerialMessage();
	}

	protected override OperateResult InitializationOnConnect()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, KeyenceNanoHelper.GetConnectCmd(Station, UseStation), hasResponseData: true, usePackAndUnpack: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content.Length > 2 && operateResult.Content[0] == 67 && operateResult.Content[1] == 67)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult("Check Failed: " + SoftBasic.ByteToHexString(operateResult.Content, ' '));
	}

	protected override OperateResult ExtraOnDisconnect()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, KeyenceNanoHelper.GetDisConnectCmd(Station, UseStation), hasResponseData: true, usePackAndUnpack: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content.Length > 2 && operateResult.Content[0] == 67 && operateResult.Content[1] == 70)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult("Check Failed: " + SoftBasic.ByteToHexString(operateResult.Content, ' '));
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		return await Task.Run(() => InitializationOnConnect()).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected override Task<OperateResult> ExtraOnDisconnectAsync()
	{
		return base.ExtraOnDisconnectAsync();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return KeyenceNanoHelper.Read(this, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return KeyenceNanoHelper.Write(this, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return KeyenceNanoHelper.ReadBool(this, address, length);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return KeyenceNanoHelper.Write(this, address, value);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return KeyenceNanoHelper.Write(this, address, value);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await Task.Run(() => Write(address, value));
	}

	[HslMqttApi("清除CPU单元发生的错误")]
	public OperateResult ClearError()
	{
		return KeyenceNanoHelper.ClearError(this);
	}

	[HslMqttApi("查询PLC的型号信息")]
	public OperateResult<KeyencePLCS> ReadPlcType()
	{
		return KeyenceNanoHelper.ReadPlcType(this);
	}

	[HslMqttApi("读取当前PLC的模式，如果是0，代表 PROG模式或者梯形图未登录，如果为1，代表RUN模式")]
	public OperateResult<int> ReadPlcMode()
	{
		return KeyenceNanoHelper.ReadPlcMode(this);
	}

	[HslMqttApi("设置PLC的时间")]
	public OperateResult SetPlcDateTime(DateTime dateTime)
	{
		return KeyenceNanoHelper.SetPlcDateTime(this, dateTime);
	}

	[HslMqttApi("读取指定软元件的注释信息")]
	public OperateResult<string> ReadAddressAnnotation(string address)
	{
		return KeyenceNanoHelper.ReadAddressAnnotation(this, address);
	}

	[HslMqttApi("从扩展单元缓冲存储器连续读取指定个数的数据，单位为字")]
	public OperateResult<byte[]> ReadExpansionMemory(byte unit, ushort address, ushort length)
	{
		return KeyenceNanoHelper.ReadExpansionMemory(this, unit, address, length);
	}

	[HslMqttApi("将原始字节数据写入到扩展的缓冲存储器，需要指定单元编号，偏移地址，写入的数据")]
	public OperateResult WriteExpansionMemory(byte unit, ushort address, byte[] value)
	{
		return KeyenceNanoHelper.WriteExpansionMemory(this, unit, address, value);
	}

	public override string ToString()
	{
		return $"KeyenceNanoSerial[{base.PortName}:{base.BaudRate}]";
	}
}
