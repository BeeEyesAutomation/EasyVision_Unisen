using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Pipe;
using HslCommunication.Instrument.DLT.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Instrument.DLT;

public class DLT698OverTcp : DeviceTcpNet, IDlt698, IReadWriteDevice, IReadWriteNet
{
	public delegate void DataReportingDelegate(CommunicationPipe pipe, OperateResult<byte[]> receive);

	private string station = "1";

	public bool UseSecurityResquest { get; set; } = true;

	public byte CA { get; set; } = 0;

	public bool IsServerActivePush { get; set; } = false;

	public string Station
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

	public bool EnableCodeFE { get; set; }

	public event DataReportingDelegate OnDataReporting;

	public DLT698OverTcp()
	{
		base.ByteTransform = new ReverseBytesTransform();
	}

	public DLT698OverTcp(string station)
		: this()
	{
		this.station = station;
	}

	public DLT698OverTcp(string ipAddress, int port, string station)
		: this(station)
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new DLT698Message();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return DLT698Helper.PackCommandWithHeader(this, command);
	}

	protected override OperateResult InitializationOnConnect()
	{
		if (IsServerActivePush)
		{
			CommunicationPipe.UseServerActivePush = true;
		}
		return base.InitializationOnConnect();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		if (IsServerActivePush)
		{
			CommunicationPipe.UseServerActivePush = true;
		}
		return await base.InitializationOnConnectAsync();
	}

	protected override bool DecideWhetherQAMessage(CommunicationPipe pipe, OperateResult<byte[]> receive)
	{
		if (IsServerActivePush)
		{
			if (!receive.IsSuccess)
			{
				return base.DecideWhetherQAMessage(pipe, receive);
			}
			try
			{
				byte[] content = receive.Content;
				string text = content.SelectMiddle(5, (content[4] & 0xF) + 1).AsEnumerable().Reverse()
					.ToArray()
					.ToHexString();
				int num = 5 + (content[4] & 0xF) + 1 + 3;
				byte ca = 16;
				if (content[3] == 129)
				{
					if (content[num] == 1 && (content[num + 2] == 0 || content[num + 2] == 1))
					{
						LogRevcMessage(receive.Content);
						byte[] content2 = DLT698Helper.BuildEntireCommand(1, Station, ca, DLT698Helper.CreatePreLogin(content.SelectMiddle(num + 5, 10))).Content;
						LogSendMessage(content2);
						pipe.Send(content2);
						return false;
					}
				}
				else if (content[3] == 67)
				{
					byte[] array = content.RemoveDouble(num, 2);
					if (array[0] == 16)
					{
						array = array.SelectMiddle(3, array[2]);
					}
					if (array[0] == 136)
					{
						LogRevcMessage(receive.Content);
						this.OnDataReporting?.Invoke(pipe, receive);
						return false;
					}
				}
			}
			catch
			{
				return base.DecideWhetherQAMessage(pipe, receive);
			}
		}
		return base.DecideWhetherQAMessage(pipe, receive);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return DLT698Helper.Read(this, address, length);
	}

	public override OperateResult Write(string address, byte[] value)
	{
		return DLT698Helper.Write(this, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return DLT698Helper.ReadBool(ReadStringArray(address), length);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<byte[]> command = DLT698Helper.BuildReadSingleObject(address, station, this);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		return DLT698Helper.CheckResponse(read.Content);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return DLT698Helper.ReadBool(await ReadStringArrayAsync(address), length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> build = DLT698Helper.BuildWriteSingleObject(address, station, value, this);
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		return DLT698Helper.CheckResponse(read.Content);
	}

	public OperateResult<byte[]> ReadByApdu(byte[] apdu)
	{
		return DLT698Helper.ReadByApdu(this, apdu);
	}

	public OperateResult ActiveDeveice()
	{
		return DLT698Helper.ActiveDeveice(this);
	}

	public OperateResult<string[]> ReadStringArray(string address)
	{
		return DLT698Helper.ReadStringArray(this, address);
	}

	public OperateResult<string[]> ReadStringArray(string[] address)
	{
		return DLT698Helper.ReadStringArray(this, address);
	}

	private OperateResult<T[]> ReadDataAndParse<T>(string address, ushort length, Func<string, T> trans)
	{
		return DLT698Helper.ReadDataAndParse(ReadStringArray(address), length, trans);
	}

	public OperateResult<string> ReadAddress()
	{
		return DLT698Helper.ReadAddress(this);
	}

	public OperateResult WriteAddress(string address)
	{
		return DLT698Helper.WriteAddress(this, address);
	}

	public OperateResult WriteDateTime(string address, DateTime time)
	{
		return DLT698Helper.WriteDateTime(this, address, time);
	}

	private async Task<OperateResult<T[]>> ReadDataAndParseAsync<T>(string address, ushort length, Func<string, T> trans)
	{
		return DLT698Helper.ReadDataAndParse(await ReadStringArrayAsync(address), length, trans);
	}

	public async Task<OperateResult<byte[]>> ReadByApduAsync(byte[] apdu)
	{
		return await DLT698Helper.ReadByApduAsync(this, apdu);
	}

	public async Task<OperateResult> ActiveDeveiceAsync()
	{
		return await ReadFromCoreServerAsync(new byte[4] { 254, 254, 254, 254 }, hasResponseData: false, usePackAndUnpack: true);
	}

	public async Task<OperateResult<string[]>> ReadStringArrayAsync(string address)
	{
		OperateResult<byte[]> read = await ReadAsync(address, 1).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(read);
		}
		int index = 8;
		return OperateResult.CreateSuccessResult(DLT698Helper.ExtraStringsValues(base.ByteTransform, read.Content, ref index));
	}

	public async Task<OperateResult<string>> ReadAddressAsync()
	{
		OperateResult<byte[]> build = DLT698Helper.BuildReadSingleObject("40-01-02-00", "AAAAAAAAAAAA", this);
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult<byte[]> extra = DLT698Helper.CheckResponse(read.Content);
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(extra);
		}
		station = extra.Content.SelectMiddle(10, extra.Content[9]).ToHexString();
		return OperateResult.CreateSuccessResult(station);
	}

	public async Task<OperateResult> WriteAddressAsync(string address)
	{
		OperateResult<byte[]> build = DLT698Helper.BuildWriteSingleObject("40-01-02-00", "AAAAAAAAAAAA", DLT698Helper.CreateStringValueBuffer(address), this);
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		return DLT698Helper.CheckResponse(read.Content);
	}

	[HslMqttApi("ReadInt16Array", "")]
	public override OperateResult<short[]> ReadInt16(string address, ushort length)
	{
		return ReadDataAndParse(address, length, short.Parse);
	}

	[HslMqttApi("ReadUInt16Array", "")]
	public override OperateResult<ushort[]> ReadUInt16(string address, ushort length)
	{
		return ReadDataAndParse(address, length, ushort.Parse);
	}

	[HslMqttApi("ReadInt32Array", "")]
	public override OperateResult<int[]> ReadInt32(string address, ushort length)
	{
		return ReadDataAndParse(address, length, int.Parse);
	}

	[HslMqttApi("ReadUInt32Array", "")]
	public override OperateResult<uint[]> ReadUInt32(string address, ushort length)
	{
		return ReadDataAndParse(address, length, uint.Parse);
	}

	[HslMqttApi("ReadInt64Array", "")]
	public override OperateResult<long[]> ReadInt64(string address, ushort length)
	{
		return ReadDataAndParse(address, length, long.Parse);
	}

	[HslMqttApi("ReadUInt64Array", "")]
	public override OperateResult<ulong[]> ReadUInt64(string address, ushort length)
	{
		return ReadDataAndParse(address, length, ulong.Parse);
	}

	[HslMqttApi("ReadFloatArray", "")]
	public override OperateResult<float[]> ReadFloat(string address, ushort length)
	{
		return ReadDataAndParse(address, length, float.Parse);
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		return ReadDataAndParse(address, length, double.Parse);
	}

	public override OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromArray(ReadStringArray(address));
	}

	public override async Task<OperateResult<short[]>> ReadInt16Async(string address, ushort length)
	{
		return await ReadDataAndParseAsync(address, length, short.Parse);
	}

	public override async Task<OperateResult<ushort[]>> ReadUInt16Async(string address, ushort length)
	{
		return await ReadDataAndParseAsync(address, length, ushort.Parse);
	}

	public override async Task<OperateResult<int[]>> ReadInt32Async(string address, ushort length)
	{
		return await ReadDataAndParseAsync(address, length, int.Parse);
	}

	public override async Task<OperateResult<uint[]>> ReadUInt32Async(string address, ushort length)
	{
		return await ReadDataAndParseAsync(address, length, uint.Parse);
	}

	public override async Task<OperateResult<long[]>> ReadInt64Async(string address, ushort length)
	{
		return await ReadDataAndParseAsync(address, length, long.Parse);
	}

	public override async Task<OperateResult<ulong[]>> ReadUInt64Async(string address, ushort length)
	{
		return await ReadDataAndParseAsync(address, length, ulong.Parse);
	}

	public override async Task<OperateResult<float[]>> ReadFloatAsync(string address, ushort length)
	{
		return await ReadDataAndParseAsync(address, length, float.Parse);
	}

	public override async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		return await ReadDataAndParseAsync(address, length, double.Parse);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadStringArrayAsync(address));
	}

	public override string ToString()
	{
		return $"DLT698OverTcp[{IpAddress}:{Port}]";
	}
}
