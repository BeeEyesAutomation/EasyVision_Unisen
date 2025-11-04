using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Profinet.LSIS.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.LSIS;

public class LSCpu : DeviceSerialPort
{
	public byte Station { get; set; } = 5;

	public LSCpu()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 2;
		LogMsgFormatBinary = false;
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return LSCpuHelper.UnpackResponseContent(send, response);
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

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return LSCpuHelper.ReadBool(this, Station, address, length);
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
		return LSCpuHelper.Write(this, Station, address, value);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await LSCpuHelper.ReadBoolAsync(this, Station, address, length);
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
		return await LSCpuHelper.WriteAsync(this, Station, address, value);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return LSCpuHelper.Read(this, Station, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return LSCpuHelper.Write(this, Station, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await LSCpuHelper.ReadAsync(this, Station, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await LSCpuHelper.WriteAsync(this, Station, address, value);
	}

	public override string ToString()
	{
		return $"LSCpu[{base.PortName}:{base.BaudRate}]";
	}
}
