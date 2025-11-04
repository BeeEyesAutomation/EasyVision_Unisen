using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Instrument.DLT.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Instrument.DLT;

public class DLT645With1997OverTcp : DeviceTcpNet, IDlt645, IReadWriteDevice, IReadWriteNet
{
	private string station = "1";

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

	public DLT645Type DLTType { get; } = DLT645Type.DLT1997;

	public string Password { get; set; }

	public string OpCode { get; set; }

	public bool CheckDataId { get; set; } = true;

	public DLT645With1997OverTcp()
	{
		base.ByteTransform = new RegularByteTransform();
	}

	public DLT645With1997OverTcp(string ipAddress, int port = 502, string station = "1")
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
		this.station = station;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new DLT645Message(CheckDataId);
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		if (EnableCodeFE)
		{
			return SoftBasic.SpliceArray<byte>(new byte[4] { 254, 254, 254, 254 }, command);
		}
		return base.PackCommandWithHeader(command);
	}

	public OperateResult ActiveDeveice()
	{
		return ReadFromCoreServer(new byte[4] { 254, 254, 254, 254 }, hasResponseData: false, usePackAndUnpack: true);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return DLT645Helper.Read(this, address, length);
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		return DLT645Helper.ReadDouble(this, address, length);
	}

	public override OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromArray(ReadStringArray(address));
	}

	public OperateResult<string[]> ReadStringArray(string address)
	{
		return DLT645Helper.ReadStringArray(this, address);
	}

	public async Task<OperateResult> ActiveDeveiceAsync()
	{
		return await ReadFromCoreServerAsync(new byte[4] { 254, 254, 254, 254 }, hasResponseData: false, usePackAndUnpack: false);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await DLT645Helper.ReadAsync(this, address, length);
	}

	public override async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		return await DLT645Helper.ReadDoubleAsync(this, address, length);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadStringArrayAsync(address));
	}

	public async Task<OperateResult<string[]>> ReadStringArrayAsync(string address)
	{
		return await DLT645Helper.ReadStringArrayAsync(this, address);
	}

	public override OperateResult Write(string address, byte[] value)
	{
		return DLT645Helper.Write(this, "", "", address, value);
	}

	[HslMqttApi("WriteDoubleArray", "")]
	public override OperateResult Write(string address, double[] values)
	{
		return DLT645Helper.Write(this, "", "", address, values.Select((double m) => m.ToString()).ToArray());
	}

	public OperateResult WriteAddress(string address)
	{
		return DLT645Helper.WriteAddress(this, address);
	}

	public OperateResult BroadcastTime(DateTime dateTime)
	{
		return DLT645Helper.BroadcastTime(this, dateTime);
	}

	public OperateResult ChangeBaudRate(string baudRate)
	{
		return DLT645Helper.ChangeBaudRate(this, baudRate);
	}

	public OperateResult<string> ReadAddress()
	{
		return new OperateResult<string>(StringResources.Language.NotSupportedFunction);
	}

	public OperateResult Trip(DateTime validTime)
	{
		return Trip(Station, validTime);
	}

	public OperateResult Trip(string station, DateTime validTime)
	{
		return DLT645Helper.Function1C(this, "", "", station, 26, validTime);
	}

	public OperateResult SwitchingOn(DateTime validTime)
	{
		return SwitchingOn(Station, validTime);
	}

	public OperateResult SwitchingOn(string station, DateTime validTime)
	{
		return DLT645Helper.Function1C(this, "", "", station, 27, validTime);
	}

	public override Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return DLT645Helper.WriteAsync(this, "", "", address, values.Select((double m) => m.ToString()).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await DLT645Helper.WriteAsync(this, "", "", address, value);
	}

	public async Task<OperateResult> WriteAddressAsync(string address)
	{
		return await DLT645Helper.WriteAddressAsync(this, address);
	}

	public async Task<OperateResult> BroadcastTimeAsync(DateTime dateTime)
	{
		return await DLT645Helper.BroadcastTimeAsync(this, dateTime, ReadFromCoreServerAsync);
	}

	public async Task<OperateResult> ChangeBaudRateAsync(string baudRate)
	{
		return await DLT645Helper.ChangeBaudRateAsync(this, baudRate);
	}

	public override string ToString()
	{
		return $"DLT645With1997OverTcp[{IpAddress}:{Port}]";
	}
}
