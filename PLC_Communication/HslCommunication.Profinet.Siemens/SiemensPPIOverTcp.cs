using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Siemens.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Siemens;

public class SiemensPPIOverTcp : DeviceTcpNet, ISiemensPPI, IReadWriteNet
{
	private byte station = 2;

	private object communicationLock;

	public byte Station
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

	public SiemensPPIOverTcp()
	{
		base.WordLength = 2;
		base.ByteTransform = new ReverseBytesTransform();
		communicationLock = new object();
	}

	public SiemensPPIOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SiemensPPIMessage();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return SiemensPPIHelper.Read(this, address, length, Station, communicationLock);
	}

	[HslMqttApi("ReadBool", "")]
	public override OperateResult<bool> ReadBool(string address)
	{
		return SiemensPPIHelper.ReadBool(this, address, Station, communicationLock);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return SiemensPPIHelper.ReadBool(this, address, length, Station, communicationLock);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return SiemensPPIHelper.Write(this, address, value, Station, communicationLock);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return SiemensPPIHelper.Write(this, address, value, Station, communicationLock);
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

	public override async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		return await Task.Run(() => ReadBool(address));
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadAsync(address, 1));
	}

	public async Task<OperateResult> WriteAsync(string address, byte value)
	{
		return await WriteAsync(address, new byte[1] { value });
	}

	[HslMqttApi]
	public OperateResult Start(string parameter = "")
	{
		return SiemensPPIHelper.Start(this, parameter, Station, communicationLock);
	}

	[HslMqttApi]
	public OperateResult Stop(string parameter = "")
	{
		return SiemensPPIHelper.Stop(this, parameter, Station, communicationLock);
	}

	[HslMqttApi]
	public OperateResult<string> ReadPlcType(string parameter = "")
	{
		return SiemensPPIHelper.ReadPlcType(this, parameter, Station, communicationLock);
	}

	public async Task<OperateResult> StartAsync(string parameter = "")
	{
		return await Task.Run(() => Start(parameter));
	}

	public async Task<OperateResult> StopAsync(string parameter = "")
	{
		return await Task.Run(() => Stop(parameter));
	}

	public async Task<OperateResult<string>> ReadPlcTypeAsync(string parameter = "")
	{
		return await Task.Run(() => SiemensPPIHelper.ReadPlcType(this, parameter, Station, communicationLock));
	}

	public override string ToString()
	{
		return $"SiemensPPIOverTcp[{IpAddress}:{Port}]";
	}
}
