using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Melsec.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecMcNet : DeviceTcpNet, IReadWriteMc, IReadWriteDevice, IReadWriteNet
{
	public virtual McType McType => McType.McBinary;

	public byte NetworkNumber { get; set; } = 0;

	public byte PLCNumber { get; set; } = byte.MaxValue;

	public byte NetworkStationNumber { get; set; } = 0;

	public bool EnableWriteBitToWordRegister { get; set; }

	public ushort TargetIOStation { get; set; } = 1023;

	public MelsecMcNet()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
	}

	public MelsecMcNet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new MelsecQnA3EBinaryMessage();
	}

	public virtual OperateResult<McAddressData> McAnalysisAddress(string address, ushort length, bool isBit)
	{
		return McAddressData.ParseMelsecFrom(address, length, isBit);
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return McBinaryHelper.PackMcCommand(this, command);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		OperateResult operateResult = McBinaryHelper.CheckResponseContentHelper(response);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(response.RemoveBegin(11));
	}

	public virtual byte[] ExtractActualData(byte[] response, bool isBit)
	{
		return McBinaryHelper.ExtractActualDataHelper(response, isBit);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return McHelper.Read(this, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return McHelper.Write(this, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await McHelper.ReadAsync(this, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await McHelper.WriteAsync(this, address, value);
	}

	[HslMqttApi("随机读取PLC的数据信息，可以跨地址，跨类型组合，但是每个地址只能读取一个word，也就是2个字节的内容。收到结果后，需要自行解析数据")]
	public OperateResult<byte[]> ReadRandom(string[] address)
	{
		return McHelper.ReadRandom(this, address);
	}

	[HslMqttApi(ApiTopic = "ReadRandoms", Description = "随机读取PLC的数据信息，可以跨地址，跨类型组合，每个地址是任意的长度。收到结果后，需要自行解析数据，目前只支持字地址，比如D区，W区，R区，不支持X，Y，M，B，L等等")]
	public OperateResult<byte[]> ReadRandom(string[] address, ushort[] length)
	{
		return McHelper.ReadRandom(this, address, length);
	}

	public OperateResult<short[]> ReadRandomInt16(string[] address)
	{
		return McHelper.ReadRandomInt16(this, address);
	}

	public OperateResult<ushort[]> ReadRandomUInt16(string[] address)
	{
		return McHelper.ReadRandomUInt16(this, address);
	}

	public async Task<OperateResult<byte[]>> ReadRandomAsync(string[] address)
	{
		return await McHelper.ReadRandomAsync(this, address);
	}

	public async Task<OperateResult<byte[]>> ReadRandomAsync(string[] address, ushort[] length)
	{
		return await McHelper.ReadRandomAsync(this, address, length);
	}

	public async Task<OperateResult<short[]>> ReadRandomInt16Async(string[] address)
	{
		return await McHelper.ReadRandomInt16Async(this, address);
	}

	public async Task<OperateResult<ushort[]>> ReadRandomUInt16Async(string[] address)
	{
		return await McHelper.ReadRandomUInt16Async(this, address);
	}

	[HslMqttApi("ReadBool", "")]
	public override OperateResult<bool> ReadBool(string address)
	{
		return base.ReadBool(address);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return McHelper.ReadBool(this, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] values)
	{
		return McHelper.Write(this, address, values);
	}

	public override async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		return await base.ReadBoolAsync(address);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await McHelper.ReadBoolAsync(this, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		return await McHelper.WriteAsync(this, address, values);
	}

	[HslMqttApi(ApiTopic = "ReadTag", Description = "读取PLC的标签信息，需要传入标签的名称，读取的字长度，标签举例：A; label[1]; bbb[10,10,10]")]
	public OperateResult<byte[]> ReadTags(string tag, ushort length)
	{
		return ReadTags(new string[1] { tag }, new ushort[1] { length });
	}

	[HslMqttApi(ApiTopic = "ReadTags", Description = "批量读取PLC的标签信息，需要传入标签的名称，读取的字长度，标签举例：A; label[1]; bbb[10,10,10]")]
	public OperateResult<byte[]> ReadTags(string[] tags, ushort[] length)
	{
		return McBinaryHelper.ReadTags(this, tags, length);
	}

	public async Task<OperateResult<byte[]>> ReadTagsAsync(string tag, ushort length)
	{
		return await ReadTagsAsync(new string[1] { tag }, new ushort[1] { length });
	}

	public async Task<OperateResult<byte[]>> ReadTagsAsync(string[] tags, ushort[] length)
	{
		return await McBinaryHelper.ReadTagsAsync(this, tags, length);
	}

	[HslMqttApi(ApiTopic = "ReadExtend", Description = "读取扩展的数据信息，需要在原有的地址，长度信息之外，输入扩展值信息")]
	public OperateResult<byte[]> ReadExtend(ushort extend, string address, ushort length)
	{
		return McHelper.ReadExtend(this, extend, address, length);
	}

	public async Task<OperateResult<byte[]>> ReadExtendAsync(ushort extend, string address, ushort length)
	{
		return await McHelper.ReadExtendAsync(this, extend, address, length);
	}

	[HslMqttApi(ApiTopic = "ReadMemory", Description = "读取缓冲寄存器的数据信息，地址直接为偏移地址")]
	public OperateResult<byte[]> ReadMemory(string address, ushort length)
	{
		return McHelper.ReadMemory(this, address, length);
	}

	public async Task<OperateResult<byte[]>> ReadMemoryAsync(string address, ushort length)
	{
		return await McHelper.ReadMemoryAsync(this, address, length);
	}

	[HslMqttApi(ApiTopic = "ReadSmartModule", Description = "读取智能模块的数据信息，需要指定模块地址，偏移地址，读取的字节长度")]
	public OperateResult<byte[]> ReadSmartModule(ushort module, string address, ushort length)
	{
		return McHelper.ReadSmartModule(this, module, address, length);
	}

	public async Task<OperateResult<byte[]>> ReadSmartModuleAsync(ushort module, string address, ushort length)
	{
		return await McHelper.ReadSmartModuleAsync(this, module, address, length);
	}

	[HslMqttApi(ApiTopic = "RemoteRun", Description = "远程Run操作")]
	public OperateResult RemoteRun()
	{
		return McHelper.RemoteRun(this);
	}

	[HslMqttApi(ApiTopic = "RemoteStop", Description = "远程Stop操作")]
	public OperateResult RemoteStop()
	{
		return McHelper.RemoteStop(this);
	}

	[HslMqttApi(ApiTopic = "RemoteReset", Description = "LED 熄灭 出错代码初始化")]
	public OperateResult RemoteReset()
	{
		return McHelper.RemoteReset(this);
	}

	[HslMqttApi(ApiTopic = "ReadPlcType", Description = "读取PLC的型号信息，例如 Q02HCPU")]
	public OperateResult<string> ReadPlcType()
	{
		return McHelper.ReadPlcType(this);
	}

	[HslMqttApi(ApiTopic = "ErrorStateReset", Description = "LED 熄灭 出错代码初始化")]
	public OperateResult ErrorStateReset()
	{
		return McHelper.ErrorStateReset(this);
	}

	public async Task<OperateResult> RemoteRunAsync()
	{
		return await McHelper.RemoteRunAsync(this);
	}

	public async Task<OperateResult> RemoteStopAsync()
	{
		return await McHelper.RemoteStopAsync(this);
	}

	public async Task<OperateResult> RemoteResetAsync()
	{
		return await McHelper.RemoteResetAsync(this);
	}

	public async Task<OperateResult<string>> ReadPlcTypeAsync()
	{
		return await McHelper.ReadPlcTypeAsync(this);
	}

	public async Task<OperateResult> ErrorStateResetAsync()
	{
		return await McHelper.ErrorStateResetAsync(this);
	}

	public override string ToString()
	{
		return $"MelsecMcNet[{IpAddress}:{Port}]";
	}
}
