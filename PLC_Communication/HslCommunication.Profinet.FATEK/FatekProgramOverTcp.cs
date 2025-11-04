using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.FATEK.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.FATEK;

public class FatekProgramOverTcp : DeviceTcpNet, IFatekProgram, IReadWriteNet
{
	private byte station = 1;

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

	public FatekProgramOverTcp()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		LogMsgFormatBinary = false;
	}

	public FatekProgramOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(3);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return FatekProgramHelper.Read(this, station, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return FatekProgramHelper.Write(this, station, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await FatekProgramHelper.ReadAsync(this, station, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await FatekProgramHelper.WriteAsync(this, station, address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return FatekProgramHelper.ReadBool(this, station, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return FatekProgramHelper.Write(this, station, address, value);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await FatekProgramHelper.ReadBoolAsync(this, station, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await FatekProgramHelper.WriteAsync(this, station, address, value);
	}

	public OperateResult Run(byte station)
	{
		return FatekProgramHelper.Run(this, station);
	}

	[HslMqttApi("Run", "使PLC处于RUN状态")]
	public OperateResult Run()
	{
		return Run(Station);
	}

	public OperateResult Stop(byte station)
	{
		return FatekProgramHelper.Stop(this, station);
	}

	[HslMqttApi("Stop", "使PLC处于STOP状态")]
	public OperateResult Stop()
	{
		return Stop(Station);
	}

	public OperateResult<bool[]> ReadStatus(byte station)
	{
		return FatekProgramHelper.ReadStatus(this, station);
	}

	[HslMqttApi("ReadStatus", "读取PLC基本的状态信息")]
	public OperateResult<bool[]> ReadStatus()
	{
		return ReadStatus(Station);
	}

	public async Task<OperateResult> RunAsync(byte station)
	{
		return await FatekProgramHelper.RunAsync(this, station);
	}

	public async Task<OperateResult> RunAsync()
	{
		return await RunAsync(Station);
	}

	public async Task<OperateResult> StopAsync(byte station)
	{
		return await FatekProgramHelper.StopAsync(this, station);
	}

	public async Task<OperateResult> StopAsync()
	{
		return await StopAsync(Station);
	}

	public async Task<OperateResult<bool[]>> ReadStatusAsync(byte station)
	{
		return await FatekProgramHelper.ReadStatusAsync(this, station);
	}

	public async Task<OperateResult<bool[]>> ReadStatusAsync()
	{
		return await ReadStatusAsync(Station);
	}

	public override string ToString()
	{
		return $"FatekProgramOverTcp[{IpAddress}:{Port}]";
	}
}
