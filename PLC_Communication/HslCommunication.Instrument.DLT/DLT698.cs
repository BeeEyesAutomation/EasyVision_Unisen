using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Instrument.DLT.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Instrument.DLT;

public class DLT698 : DeviceSerialPort, IDlt698, IReadWriteDevice, IReadWriteNet
{
	private string station = "1";

	public bool UseSecurityResquest { get; set; } = true;

	public byte CA { get; set; } = 0;

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

	public DLT698()
	{
		base.ByteTransform = new ReverseBytesTransform();
		base.ReceiveEmptyDataCount = 20;
	}

	public DLT698(string station)
		: this()
	{
		this.station = station;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new DLT698Message();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return DLT698Helper.PackCommandWithHeader(this, command);
	}

	public OperateResult<byte[]> ReadByApdu(byte[] apdu)
	{
		return DLT698Helper.ReadByApdu(this, apdu);
	}

	public OperateResult ActiveDeveice()
	{
		return DLT698Helper.ActiveDeveice(this);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return DLT698Helper.Read(this, address, length);
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

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return DLT698Helper.ReadBool(ReadStringArray(address), length);
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

	public override OperateResult Write(string address, byte[] value)
	{
		return DLT698Helper.Write(this, address, value);
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

	public override async Task<OperateResult<short[]>> ReadInt16Async(string address, ushort length)
	{
		return await Task.Run(() => ReadInt16(address, length));
	}

	public override async Task<OperateResult<ushort[]>> ReadUInt16Async(string address, ushort length)
	{
		return await Task.Run(() => ReadUInt16(address, length));
	}

	public override async Task<OperateResult<int[]>> ReadInt32Async(string address, ushort length)
	{
		return await Task.Run(() => ReadInt32(address, length));
	}

	public override async Task<OperateResult<uint[]>> ReadUInt32Async(string address, ushort length)
	{
		return await Task.Run(() => ReadUInt32(address, length));
	}

	public override async Task<OperateResult<long[]>> ReadInt64Async(string address, ushort length)
	{
		return await Task.Run(() => ReadInt64(address, length));
	}

	public override async Task<OperateResult<ulong[]>> ReadUInt64Async(string address, ushort length)
	{
		return await Task.Run(() => ReadUInt64(address, length));
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

	public override string ToString()
	{
		return $"DLT698[{base.PortName}:{base.BaudRate}]";
	}
}
