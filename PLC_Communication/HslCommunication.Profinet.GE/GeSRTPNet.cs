using System;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.GE;

public class GeSRTPNet : DeviceTcpNet
{
	private SoftIncrementCount incrementCount = new SoftIncrementCount(65535L, 0L);

	public GeSRTPNet()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 2;
	}

	public GeSRTPNet(string ipAddress, int port = 18245)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new GeSRTPMessage();
	}

	protected override OperateResult InitializationOnConnect()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, new byte[56], hasResponseData: true, usePackAndUnpack: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(CommunicationPipe, new byte[56], hasResponseData: true, usePackAndUnpack: true).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte[]> operateResult = GeHelper.BuildReadCommand(incrementCount.GetCurrentValue(), address, length, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return GeHelper.ExtraResponseContent(operateResult2.Content);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = GeHelper.BuildWriteCommand(incrementCount.GetCurrentValue(), address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return GeHelper.ExtraResponseContent(operateResult2.Content);
	}

	[HslMqttApi("ReadByte", "")]
	public OperateResult<byte> ReadByte(string address)
	{
		OperateResult<GeSRTPAddress> operateResult = GeSRTPAddress.ParseFrom(address, 1, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte>(operateResult);
		}
		if (operateResult.Content.DataCode == 10 || operateResult.Content.DataCode == 12 || operateResult.Content.DataCode == 8)
		{
			return new OperateResult<byte>(StringResources.Language.GeSRTPNotSupportByteReadWrite);
		}
		return ByteTransformHelper.GetResultFromArray(Read(address, 1));
	}

	[HslMqttApi("WriteByte", "")]
	public OperateResult Write(string address, byte value)
	{
		OperateResult<GeSRTPAddress> operateResult = GeSRTPAddress.ParseFrom(address, 1, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte>(operateResult);
		}
		if (operateResult.Content.DataCode == 10 || operateResult.Content.DataCode == 12 || operateResult.Content.DataCode == 8)
		{
			return new OperateResult<byte>(StringResources.Language.GeSRTPNotSupportByteReadWrite);
		}
		return Write(address, new byte[1] { value });
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		OperateResult<GeSRTPAddress> operateResult = GeSRTPAddress.ParseFrom(address, length, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = GeHelper.BuildReadCommand(incrementCount.GetCurrentValue(), operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = ReadFromCoreServer(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult3);
		}
		OperateResult<byte[]> operateResult4 = GeHelper.ExtraResponseContent(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult4);
		}
		return OperateResult.CreateSuccessResult(operateResult4.Content.ToBoolArray().SelectMiddle(operateResult.Content.AddressStart % 8, length));
	}

	[HslMqttApi(ApiTopic = "WriteBoolArray", Description = "In units of bits, write bool arrays in batches to the specified addresses")]
	public override OperateResult Write(string address, bool[] value)
	{
		OperateResult<byte[]> operateResult = GeHelper.BuildWriteCommand(incrementCount.GetCurrentValue(), address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return GeHelper.ExtraResponseContent(operateResult2.Content);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<byte[]> build = GeHelper.BuildReadCommand(incrementCount.GetCurrentValue(), address, length, isBit: false);
		if (!build.IsSuccess)
		{
			return build;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return GeHelper.ExtraResponseContent(read.Content);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> build = GeHelper.BuildWriteCommand(incrementCount.GetCurrentValue(), address, value);
		if (!build.IsSuccess)
		{
			return build;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return GeHelper.ExtraResponseContent(read.Content);
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		OperateResult<GeSRTPAddress> analysis = GeSRTPAddress.ParseFrom(address, 1, isBit: true);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte>(analysis);
		}
		if (analysis.Content.DataCode == 10 || analysis.Content.DataCode == 12 || analysis.Content.DataCode == 8)
		{
			return new OperateResult<byte>(StringResources.Language.GeSRTPNotSupportByteReadWrite);
		}
		return ByteTransformHelper.GetResultFromArray(await ReadAsync(address, 1));
	}

	public async Task<OperateResult> WriteAsync(string address, byte value)
	{
		OperateResult<GeSRTPAddress> analysis = GeSRTPAddress.ParseFrom(address, 1, isBit: true);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte>(analysis);
		}
		if (analysis.Content.DataCode == 10 || analysis.Content.DataCode == 12 || analysis.Content.DataCode == 8)
		{
			return new OperateResult<byte>(StringResources.Language.GeSRTPNotSupportByteReadWrite);
		}
		return await WriteAsync(address, new byte[1] { value });
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		OperateResult<GeSRTPAddress> analysis = GeSRTPAddress.ParseFrom(address, length, isBit: true);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(analysis);
		}
		OperateResult<byte[]> build = GeHelper.BuildReadCommand(incrementCount.GetCurrentValue(), analysis.Content);
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		OperateResult<byte[]> extra = GeHelper.ExtraResponseContent(read.Content);
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(extra);
		}
		return OperateResult.CreateSuccessResult(extra.Content.ToBoolArray().SelectMiddle(analysis.Content.AddressStart % 8, length));
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		OperateResult<byte[]> build = GeHelper.BuildWriteCommand(incrementCount.GetCurrentValue(), address, value);
		if (!build.IsSuccess)
		{
			return build;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return GeHelper.ExtraResponseContent(read.Content);
	}

	[HslMqttApi(Description = "Read the current time of the PLC")]
	public OperateResult<DateTime> ReadPLCTime()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<DateTime>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> operateResult = GeHelper.BuildReadCoreCommand(incrementCount.GetCurrentValue(), 37, new byte[5] { 0, 0, 0, 2, 0 });
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = GeHelper.ExtraResponseContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult3);
		}
		return GeHelper.ExtraDateTime(operateResult3.Content);
	}

	[HslMqttApi(Description = "Read the name of the current program of the PLC")]
	public OperateResult<string> ReadProgramName()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<string>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> operateResult = GeHelper.BuildReadCoreCommand(incrementCount.GetCurrentValue(), 1, new byte[5] { 0, 0, 0, 2, 0 });
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = GeHelper.ExtraResponseContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult3);
		}
		return GeHelper.ExtraProgramName(operateResult3.Content);
	}

	public async Task<OperateResult<DateTime>> ReadPLCTimeAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<DateTime>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> build = GeHelper.BuildReadCoreCommand(incrementCount.GetCurrentValue(), 37, new byte[5] { 0, 0, 0, 2, 0 });
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(read);
		}
		OperateResult<byte[]> extra = GeHelper.ExtraResponseContent(read.Content);
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(extra);
		}
		return GeHelper.ExtraDateTime(extra.Content);
	}

	public async Task<OperateResult<string>> ReadProgramNameAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<string>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> build = GeHelper.BuildReadCoreCommand(incrementCount.GetCurrentValue(), 1, new byte[5] { 0, 0, 0, 2, 0 });
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult<byte[]> extra = GeHelper.ExtraResponseContent(read.Content);
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(extra);
		}
		return GeHelper.ExtraProgramName(extra.Content);
	}

	public override string ToString()
	{
		return $"GeSRTPNet[{IpAddress}:{Port}]";
	}
}
