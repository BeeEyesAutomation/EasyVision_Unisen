using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.Net;
using HslCommunication.Profinet.LSIS.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.LSIS;

public class LSCnetOverTcp : DeviceTcpNet, IReadWriteDeviceStation, IReadWriteDevice, IReadWriteNet
{
	public byte Station { get; set; } = 5;

	public LSCnetOverTcp()
	{
		base.WordLength = 2;
		base.ByteTransform = new RegularByteTransform();
		base.SleepTime = 20;
	}

	public LSCnetOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return LSCnetHelper.UnpackResponseContent(send, response);
	}

	[HslMqttApi("ReadByte", "")]
	public OperateResult<byte> ReadByte(string address)
	{
		return ByteTransformHelper.GetResultFromArray(Read(address, 1));
	}

	[HslMqttApi("WriteByte", "")]
	public OperateResult Write(string address, byte value)
	{
		return Write(address, new byte[1] { value });
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadAsync(address, 2));
	}

	public async Task<OperateResult> WriteAsync(string address, byte value)
	{
		return await WriteAsync(address, new byte[1] { value });
	}

	[HslMqttApi("ReadBool", "")]
	public override OperateResult<bool> ReadBool(string address)
	{
		return LSCnetHelper.ReadBool(this, Station, address);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return LSCnetHelper.ReadBool(this, address, length);
	}

	public OperateResult<bool> ReadCoil(string address)
	{
		return ReadBool(address);
	}

	public OperateResult<bool[]> ReadCoil(string address, ushort length)
	{
		return ReadBool(address, length);
	}

	public OperateResult WriteCoil(string address, bool value)
	{
		return Write(address, value);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return LSCnetHelper.Write(this, Station, address, value);
	}

	public override async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		return await LSCnetHelper.ReadBoolAsync(this, Station, address);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await LSCnetHelper.ReadBoolAsync(this, address, length);
	}

	public async Task<OperateResult<bool>> ReadCoilAsync(string address)
	{
		return await ReadBoolAsync(address);
	}

	public async Task<OperateResult<bool[]>> ReadCoilAsync(string address, ushort length)
	{
		return await ReadBoolAsync(address, length);
	}

	public async Task<OperateResult> WriteCoilAsync(string address, bool value)
	{
		return await WriteAsync(address, value);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await LSCnetHelper.WriteAsync(this, Station, address, value);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return LSCnetHelper.Read(this, address, length);
	}

	public OperateResult<byte[]> Read(string[] address)
	{
		return LSCnetHelper.Read(this, Station, address);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return LSCnetHelper.Write(this, Station, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await LSCnetHelper.ReadAsync(this, address, length);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string[] address)
	{
		return await LSCnetHelper.ReadAsync(this, Station, address);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await LSCnetHelper.WriteAsync(this, Station, address, value);
	}

	public override string ToString()
	{
		return $"LsCnetOverTcp[{IpAddress}:{Port}]";
	}
}
