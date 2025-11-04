using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Omron.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Omron;

public class OmronHostLinkCModeOverTcp : DeviceTcpNet, IHostLinkCMode, IReadWriteNet
{
	public byte UnitNumber { get; set; }

	public OmronHostLinkCModeOverTcp()
	{
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
		base.WordLength = 1;
		base.ByteTransform.IsStringReverseByteWord = true;
		LogMsgFormatBinary = false;
	}

	public OmronHostLinkCModeOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(13);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return HslHelper.ReadBool(this, address, length, 16, reverseByWord: true);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return HslHelper.WriteBool(this, address, value, 16, reverseByWord: true);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return OmronHostLinkCModeHelper.Read(this, UnitNumber, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return OmronHostLinkCModeHelper.Write(this, UnitNumber, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await OmronHostLinkCModeHelper.ReadAsync(this, UnitNumber, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await OmronHostLinkCModeHelper.WriteAsync(this, UnitNumber, address, value);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await HslHelper.ReadBoolAsync(this, address, length, 16, reverseByWord: true);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await HslHelper.WriteBoolAsync(this, address, value, 16, reverseByWord: true);
	}

	[HslMqttApi("读取PLC的当前的型号信息")]
	public OperateResult<string> ReadPlcType()
	{
		return ReadPlcType(UnitNumber);
	}

	public OperateResult<string> ReadPlcType(byte unitNumber)
	{
		return OmronHostLinkCModeHelper.ReadPlcType(this, unitNumber);
	}

	[HslMqttApi("读取PLC当前的操作模式，0: 编程模式  1: 运行模式  2: 监视模式")]
	public OperateResult<int> ReadPlcMode()
	{
		return ReadPlcMode(UnitNumber);
	}

	public OperateResult<int> ReadPlcMode(byte unitNumber)
	{
		return OmronHostLinkCModeHelper.ReadPlcMode(this, unitNumber);
	}

	[HslMqttApi("将当前PLC的模式变更为指定的模式，0: 编程模式  1: 运行模式  2: 监视模式")]
	public OperateResult ChangePlcMode(byte mode)
	{
		return ChangePlcMode(UnitNumber, mode);
	}

	public OperateResult ChangePlcMode(byte unitNumber, byte mode)
	{
		return OmronHostLinkCModeHelper.ChangePlcMode(this, unitNumber, mode);
	}

	public override string ToString()
	{
		return $"OmronHostLinkCModeOverTcp[{IpAddress}:{Port}]";
	}
}
