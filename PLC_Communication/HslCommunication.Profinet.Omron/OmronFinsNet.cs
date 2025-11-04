using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Omron.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Omron;

public class OmronFinsNet : DeviceTcpNet, IOmronFins, IReadWriteDevice, IReadWriteNet
{
	private readonly byte[] handSingle = new byte[20]
	{
		70, 73, 78, 83, 0, 0, 0, 12, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	};

	private readonly SoftIncrementCount incrementSID = new SoftIncrementCount(255L, 0L);

	public byte ICF { get; set; } = 128;

	public byte RSV { get; private set; } = 0;

	public byte GCT { get; set; } = 2;

	public byte DNA { get; set; } = 0;

	[HslMqttApi(HttpMethod = "GET", Description = "The node address of the PLC is 0 by default. During the process of connecting with the PLC, the value of DA1 is automatically obtained from the PLC.")]
	public byte DA1 { get; set; } = 0;

	public byte DA2 { get; set; } = 0;

	public byte SNA { get; set; } = 0;

	[HslMqttApi(HttpMethod = "GET", Description = "The node address of the host computer is 0x01 by default. After connecting to the PLC, the PLC will set the current value.")]
	public byte SA1 { get; set; } = 1;

	public byte SA2 { get; set; }

	public byte SID { get; set; } = 0;

	public int ReadSplits { get; set; } = 500;

	public bool ReceiveUntilEmpty { get; set; } = false;

	public OmronPlcType PlcType { get; set; } = OmronPlcType.CSCJ;

	public OmronFinsNet()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
		base.ByteTransform.IsStringReverseByteWord = true;
	}

	public OmronFinsNet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FinsMessage();
	}

	private byte[] PackCommand(byte[] cmd)
	{
		byte[] array = new byte[26 + cmd.Length];
		Array.Copy(handSingle, 0, array, 0, 4);
		byte[] bytes = BitConverter.GetBytes(array.Length - 8);
		Array.Reverse(bytes);
		bytes.CopyTo(array, 4);
		array[11] = 2;
		array[16] = ICF;
		array[17] = RSV;
		array[18] = GCT;
		array[19] = DNA;
		array[20] = DA1;
		array[21] = DA2;
		array[22] = SNA;
		array[23] = SA1;
		array[24] = SA2;
		array[25] = (byte)incrementSID.GetCurrentValue();
		cmd.CopyTo(array, 26);
		SID = array[25];
		return array;
	}

	protected override OperateResult InitializationOnConnect()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, handSingle, hasResponseData: true, usePackAndUnpack: false);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		int num = BitConverter.ToInt32(new byte[4]
		{
			operateResult.Content[15],
			operateResult.Content[14],
			operateResult.Content[13],
			operateResult.Content[12]
		}, 0);
		if (num != 0)
		{
			return new OperateResult(num, OmronFinsNetHelper.GetStatusDescription(num));
		}
		if (operateResult.Content.Length >= 20)
		{
			SA1 = operateResult.Content[19];
		}
		if (operateResult.Content.Length >= 24)
		{
			DA1 = operateResult.Content[23];
		}
		incrementSID.ResetStartValue(0L);
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(CommunicationPipe, handSingle, hasResponseData: true, usePackAndUnpack: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		int status = BitConverter.ToInt32(new byte[4]
		{
			read.Content[15],
			read.Content[14],
			read.Content[13],
			read.Content[12]
		}, 0);
		if (status != 0)
		{
			return new OperateResult(status, OmronFinsNetHelper.GetStatusDescription(status));
		}
		if (read.Content.Length >= 20)
		{
			SA1 = read.Content[19];
		}
		if (read.Content.Length >= 24)
		{
			DA1 = read.Content[23];
		}
		incrementSID.ResetStartValue(0L);
		return OperateResult.CreateSuccessResult();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return PackCommand(command);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return OmronFinsNetHelper.ResponseValidAnalysis(response);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return OmronFinsNetHelper.Read(this, address, length, ReadSplits);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return OmronFinsNetHelper.Write(this, address, value);
	}

	[HslMqttApi("ReadString", "")]
	public override OperateResult<string> ReadString(string address, ushort length)
	{
		return base.ReadString(address, length, Encoding.UTF8);
	}

	[HslMqttApi("WriteString", "")]
	public override OperateResult Write(string address, string value)
	{
		return base.Write(address, value, Encoding.UTF8);
	}

	public OperateResult<byte[]> Read(string[] address)
	{
		return OmronFinsNetHelper.Read(this, address);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await OmronFinsNetHelper.ReadAsync(this, address, length, ReadSplits);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await OmronFinsNetHelper.WriteAsync(this, address, value);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length)
	{
		return await base.ReadStringAsync(address, length, Encoding.UTF8);
	}

	public override async Task<OperateResult> WriteAsync(string address, string value)
	{
		return await base.WriteAsync(address, value, Encoding.UTF8);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string[] address)
	{
		return await OmronFinsNetHelper.ReadAsync(this, address);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return OmronFinsNetHelper.ReadBool(this, address, length, ReadSplits);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] values)
	{
		return OmronFinsNetHelper.Write(this, address, values);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await OmronFinsNetHelper.ReadBoolAsync(this, address, length, ReadSplits);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		return await OmronFinsNetHelper.WriteAsync(this, address, values);
	}

	[HslMqttApi(ApiTopic = "Run", Description = "将CPU单元的操作模式更改为RUN，从而使PLC能够执行其程序。")]
	public OperateResult Run()
	{
		return OmronFinsNetHelper.Run(this);
	}

	[HslMqttApi(ApiTopic = "Stop", Description = "将CPU单元的操作模式更改为PROGRAM，停止程序执行。")]
	public OperateResult Stop()
	{
		return OmronFinsNetHelper.Stop(this);
	}

	[HslMqttApi(ApiTopic = "ReadCpuUnitData", Description = "读取CPU的一些数据信息，主要包含型号，版本，一些数据块的大小。")]
	public OperateResult<OmronCpuUnitData> ReadCpuUnitData()
	{
		return OmronFinsNetHelper.ReadCpuUnitData(this);
	}

	[HslMqttApi(ApiTopic = "ReadCpuUnitStatus", Description = "读取CPU单元的一些操作状态数据，主要包含运行状态，工作模式，错误信息等。")]
	public OperateResult<OmronCpuUnitStatus> ReadCpuUnitStatus()
	{
		return OmronFinsNetHelper.ReadCpuUnitStatus(this);
	}

	[HslMqttApi(ApiTopic = "ReadCpuTime", Description = "读取CPU单元的时间信息。")]
	public OperateResult<DateTime> ReadCpuTime()
	{
		return OmronFinsNetHelper.ReadCpuTime(this);
	}

	public async Task<OperateResult> RunAsync()
	{
		return await OmronFinsNetHelper.RunAsync(this).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult> StopAsync()
	{
		return await OmronFinsNetHelper.StopAsync(this).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<OmronCpuUnitData>> ReadCpuUnitDataAsync()
	{
		return await OmronFinsNetHelper.ReadCpuUnitDataAsync(this).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<OmronCpuUnitStatus>> ReadCpuUnitStatusAsync()
	{
		return await OmronFinsNetHelper.ReadCpuUnitStatusAsync(this).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<DateTime>> ReadCpuTimeAsync()
	{
		return await OmronFinsNetHelper.ReadCpuTimeAsync(this).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override string ToString()
	{
		return $"OmronFinsNet[{IpAddress}:{Port}]";
	}
}
