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

public class DLT645OverTcp : DeviceTcpNet, IDlt645, IReadWriteDevice, IReadWriteNet
{
	private string station = "1";

	private string password = "00000000";

	private string opCode = "00000000";

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

	public DLT645Type DLTType { get; } = DLT645Type.DLT2007;

	public string Password
	{
		get
		{
			return password;
		}
		set
		{
			password = value;
		}
	}

	public string OpCode
	{
		get
		{
			return opCode;
		}
		set
		{
			opCode = value;
		}
	}

	public bool CheckDataId { get; set; } = true;

	public DLT645OverTcp()
	{
		base.ByteTransform = new RegularByteTransform();
		password = "00000000";
		opCode = "00000000";
		base.WordLength = 1;
	}

	public DLT645OverTcp(string ipAddress, int port = 502, string station = "1", string password = "", string opCode = "")
	{
		IpAddress = ipAddress;
		Port = port;
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		this.station = station;
		this.password = (string.IsNullOrEmpty(password) ? "00000000" : password);
		this.opCode = (string.IsNullOrEmpty(opCode) ? "00000000" : opCode);
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

	public OperateResult Trip(DateTime validTime)
	{
		return Trip(Station, validTime);
	}

	public OperateResult Trip(string station, DateTime validTime)
	{
		return DLT645Helper.Function1C(this, password, opCode, station, 26, validTime);
	}

	public OperateResult SwitchingOn(DateTime validTime)
	{
		return SwitchingOn(Station, validTime);
	}

	public OperateResult SwitchingOn(string station, DateTime validTime)
	{
		return DLT645Helper.Function1C(this, password, opCode, station, 27, validTime);
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
		return DLT645Helper.Write(this, password, opCode, address, value);
	}

	public override OperateResult Write(string address, short[] values)
	{
		return DLT645Helper.Write(this, password, opCode, address, values.Select((short m) => m.ToString()).ToArray());
	}

	public override OperateResult Write(string address, ushort[] values)
	{
		return DLT645Helper.Write(this, password, opCode, address, values.Select((ushort m) => m.ToString()).ToArray());
	}

	public override OperateResult Write(string address, int[] values)
	{
		return DLT645Helper.Write(this, password, opCode, address, values.Select((int m) => m.ToString()).ToArray());
	}

	public override OperateResult Write(string address, uint[] values)
	{
		return DLT645Helper.Write(this, password, opCode, address, values.Select((uint m) => m.ToString()).ToArray());
	}

	public override OperateResult Write(string address, float[] values)
	{
		return DLT645Helper.Write(this, password, opCode, address, values.Select((float m) => m.ToString()).ToArray());
	}

	public override OperateResult Write(string address, double[] values)
	{
		return DLT645Helper.Write(this, password, opCode, address, values.Select((double m) => m.ToString()).ToArray());
	}

	public override OperateResult Write(string address, string value, Encoding encoding)
	{
		return DLT645Helper.Write(this, password, opCode, address, new string[1] { value });
	}

	public OperateResult<string> ReadAddress()
	{
		return DLT645Helper.ReadAddress(this);
	}

	public OperateResult WriteAddress(string address)
	{
		return DLT645Helper.WriteAddress(this, address);
	}

	public OperateResult BroadcastTime(DateTime dateTime)
	{
		return DLT645Helper.BroadcastTime(this, dateTime);
	}

	public OperateResult FreezeCommand(string dataArea)
	{
		return DLT645Helper.FreezeCommand(this, dataArea);
	}

	public OperateResult ChangeBaudRate(string baudRate)
	{
		return DLT645Helper.ChangeBaudRate(this, baudRate);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await DLT645Helper.WriteAsync(this, password, opCode, address, value);
	}

	public override async Task<OperateResult> WriteAsync(string address, short[] values)
	{
		return await DLT645Helper.WriteAsync(this, password, opCode, address, values.Select((short m) => m.ToString()).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, ushort[] values)
	{
		return await DLT645Helper.WriteAsync(this, password, opCode, address, values.Select((ushort m) => m.ToString()).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, int[] values)
	{
		return await DLT645Helper.WriteAsync(this, password, opCode, address, values.Select((int m) => m.ToString()).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, uint[] values)
	{
		return await DLT645Helper.WriteAsync(this, password, opCode, address, values.Select((uint m) => m.ToString()).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, float[] values)
	{
		return await DLT645Helper.WriteAsync(this, password, opCode, address, values.Select((float m) => m.ToString()).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await DLT645Helper.WriteAsync(this, password, opCode, address, values.Select((double m) => m.ToString()).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, string value, Encoding encoding)
	{
		return await DLT645Helper.WriteAsync(this, password, opCode, address, new string[1] { value });
	}

	public async Task<OperateResult<string>> ReadAddressAsync()
	{
		return await DLT645Helper.ReadAddressAsync(this);
	}

	public async Task<OperateResult> WriteAddressAsync(string address)
	{
		return await DLT645Helper.WriteAddressAsync(this, address);
	}

	public async Task<OperateResult> BroadcastTimeAsync(DateTime dateTime)
	{
		return await DLT645Helper.BroadcastTimeAsync(this, dateTime, ReadFromCoreServerAsync);
	}

	public async Task<OperateResult> FreezeCommandAsync(string dataArea)
	{
		return await DLT645Helper.FreezeCommandAsync(this, dataArea);
	}

	public async Task<OperateResult> ChangeBaudRateAsync(string baudRate)
	{
		return await DLT645Helper.ChangeBaudRateAsync(this, baudRate);
	}

	public override string ToString()
	{
		return $"DLT645OverTcp[{IpAddress}:{Port}]";
	}
}
