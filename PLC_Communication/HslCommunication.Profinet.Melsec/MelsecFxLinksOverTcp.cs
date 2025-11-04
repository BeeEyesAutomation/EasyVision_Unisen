using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Profinet.Melsec.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecFxLinksOverTcp : DeviceTcpNet, IReadWriteFxLinks, IReadWriteDevice, IReadWriteNet, IReadWriteDeviceStation
{
	private byte station = 0;

	private byte waittingTime = 0;

	private bool sumCheck = true;

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

	public byte WaittingTime
	{
		get
		{
			return waittingTime;
		}
		set
		{
			if (value > 15)
			{
				waittingTime = 15;
			}
			else
			{
				waittingTime = value;
			}
		}
	}

	public bool SumCheck
	{
		get
		{
			return sumCheck;
		}
		set
		{
			sumCheck = value;
		}
	}

	public int Format { get; set; } = 1;

	public MelsecFxLinksOverTcp()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		LogMsgFormatBinary = false;
	}

	public MelsecFxLinksOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new MelsecFxLinksMessage(Format, SumCheck);
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return MelsecFxLinksHelper.PackCommandWithHeader(this, command);
	}

	[HslMqttApi("ReadByteArray", "Read PLC data in batches, in units of words, supports reading X, Y, M, S, D, T, C.")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return MelsecFxLinksHelper.Read(this, address, length);
	}

	[HslMqttApi("WriteByteArray", "The data written to the PLC in batches is in units of words, that is, at least 2 bytes of information. It supports X, Y, M, S, D, T, and C. ")]
	public override OperateResult Write(string address, byte[] value)
	{
		return MelsecFxLinksHelper.Write(this, address, value);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await MelsecFxLinksHelper.ReadAsync(this, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await MelsecFxLinksHelper.WriteAsync(this, address, value);
	}

	[HslMqttApi("ReadBoolArray", "Read bool data in batches. The supported types are X, Y, S, T, C.")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return MelsecFxLinksHelper.ReadBool(this, address, length);
	}

	[HslMqttApi("WriteBoolArray", "Write arrays of type bool in batches. The supported types are X, Y, S, T, C.")]
	public override OperateResult Write(string address, bool[] value)
	{
		return MelsecFxLinksHelper.Write(this, address, value);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await MelsecFxLinksHelper.ReadBoolAsync(this, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await MelsecFxLinksHelper.WriteAsync(this, address, value);
	}

	[HslMqttApi(Description = "Start the PLC operation, you can carry additional parameter information and specify the station number. Example: s=2; Note: The semicolon is required.")]
	public OperateResult StartPLC(string parameter = "")
	{
		return MelsecFxLinksHelper.StartPLC(this, parameter);
	}

	[HslMqttApi(Description = "Stop PLC operation, you can carry additional parameter information and specify the station number. Example: s=2; Note: The semicolon is required.")]
	public OperateResult StopPLC(string parameter = "")
	{
		return MelsecFxLinksHelper.StopPLC(this, parameter);
	}

	[HslMqttApi(Description = "Read the PLC model information, you can carry additional parameter information, and specify the station number. Example: s=2; Note: The semicolon is required.")]
	public OperateResult<string> ReadPlcType(string parameter = "")
	{
		return MelsecFxLinksHelper.ReadPlcType(this, parameter);
	}

	public async Task<OperateResult> StartPLCAsync(string parameter = "")
	{
		return await MelsecFxLinksHelper.StartPLCAsync(this, parameter);
	}

	public async Task<OperateResult> StopPLCAsync(string parameter = "")
	{
		return await MelsecFxLinksHelper.StopPLCAsync(this, parameter);
	}

	public async Task<OperateResult<string>> ReadPlcTypeAsync(string parameter = "")
	{
		return await MelsecFxLinksHelper.ReadPlcTypeAsync(this, parameter);
	}

	public override string ToString()
	{
		return $"MelsecFxLinksOverTcp[{IpAddress}:{Port}]";
	}
}
