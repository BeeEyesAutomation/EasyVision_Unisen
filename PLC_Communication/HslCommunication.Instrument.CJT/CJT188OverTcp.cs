using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Instrument.CJT.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Instrument.CJT;

public class CJT188OverTcp : DeviceTcpNet, ICjt188, IReadWriteDevice, IReadWriteNet
{
	private string station = "1";

	public byte InstrumentType { get; set; }

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

	public bool EnableCodeFE { get; set; } = true;

	public bool StationMatch { get; set; } = false;

	public CJT188OverTcp(string station)
	{
		base.ByteTransform = new RegularByteTransform();
		this.station = station;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new CJT188Message(StationMatch);
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		if (EnableCodeFE)
		{
			return SoftBasic.SpliceArray<byte>(new byte[2] { 254, 254 }, command);
		}
		return base.PackCommandWithHeader(command);
	}

	public OperateResult ActiveDeveice()
	{
		return ReadFromCoreServer(new byte[2] { 254, 254 }, hasResponseData: false, usePackAndUnpack: false);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return CJT188Helper.Read(this, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return CJT188Helper.Write(this, address, value);
	}

	[HslMqttApi("ReadFloatArray", "")]
	public override OperateResult<float[]> ReadFloat(string address, ushort length)
	{
		return CJT188Helper.ReadValue(this, address, length, float.Parse);
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		return CJT188Helper.ReadValue(this, address, length, double.Parse);
	}

	public override OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromArray(ReadStringArray(address));
	}

	public OperateResult<string[]> ReadStringArray(string address)
	{
		return CJT188Helper.ReadStringArray(this, address);
	}

	public override async Task<OperateResult<float[]>> ReadFloatAsync(string address, ushort length)
	{
		return await Task.Run(() => ReadFloat(address, length));
	}

	public override async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		return await Task.Run(() => ReadDouble(address, length));
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		return await Task.Run(() => ReadString(address, length, encoding));
	}

	public OperateResult WriteAddress(string address)
	{
		return CJT188Helper.WriteAddress(this, address);
	}

	public OperateResult<string> ReadAddress()
	{
		return CJT188Helper.ReadAddress(this);
	}

	public override string ToString()
	{
		return $"CJT188OverTcp[{IpAddress}:{Port}]";
	}
}
