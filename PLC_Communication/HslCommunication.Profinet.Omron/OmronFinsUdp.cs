using System;
using System.Text;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Omron.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Omron;

public class OmronFinsUdp : DeviceUdpNet, IOmronFins, IReadWriteDevice, IReadWriteNet
{
	public byte ICF { get; set; } = 128;

	public byte RSV { get; private set; } = 0;

	public byte GCT { get; set; } = 2;

	public byte DNA { get; set; } = 0;

	public byte DA1 { get; set; } = 0;

	public byte DA2 { get; set; } = 0;

	public byte SNA { get; set; } = 0;

	public byte SA1 { get; set; } = 13;

	public byte SA2 { get; set; }

	public byte SID { get; set; } = 0;

	public int ReadSplits { get; set; } = 500;

	public OmronPlcType PlcType { get; set; } = OmronPlcType.CSCJ;

	public OmronFinsUdp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	public OmronFinsUdp()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
		base.ByteTransform.IsStringReverseByteWord = true;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FinsUdpMessage();
	}

	private byte[] PackCommand(byte[] cmd)
	{
		byte[] array = new byte[10 + cmd.Length];
		array[0] = ICF;
		array[1] = RSV;
		array[2] = GCT;
		array[3] = DNA;
		array[4] = DA1;
		array[5] = DA2;
		array[6] = SNA;
		array[7] = SA1;
		array[8] = SA2;
		array[9] = SID;
		cmd.CopyTo(array, 10);
		return array;
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return PackCommand(command);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return OmronFinsNetHelper.UdpResponseValidAnalysis(response);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return OmronFinsNetHelper.Read(this, address, length, ReadSplits);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return OmronFinsNetHelper.Write(this, address, value);
	}

	[HslMqttApi("ReadString", "")]
	public override OperateResult<string> ReadString(string address, ushort length)
	{
		return base.ReadString(address, length, Encoding.UTF8);
	}

	[HslMqttApi("WriteString", "")]
	public override OperateResult Write(string address, string value)
	{
		return base.Write(address, value, Encoding.UTF8);
	}

	public OperateResult<byte[]> Read(string[] address)
	{
		return OmronFinsNetHelper.Read(this, address);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return OmronFinsNetHelper.ReadBool(this, address, length, ReadSplits);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] values)
	{
		return OmronFinsNetHelper.Write(this, address, values);
	}

	[HslMqttApi(ApiTopic = "Run", Description = "将CPU单元的操作模式更改为RUN，从而使PLC能够执行其程序。")]
	public OperateResult Run()
	{
		return OmronFinsNetHelper.Run(this);
	}

	[HslMqttApi(ApiTopic = "Stop", Description = "将CPU单元的操作模式更改为PROGRAM，停止程序执行。")]
	public OperateResult Stop()
	{
		return OmronFinsNetHelper.Stop(this);
	}

	[HslMqttApi(ApiTopic = "ReadCpuUnitData", Description = "读取CPU的一些数据信息，主要包含型号，版本，一些数据块的大小。")]
	public OperateResult<OmronCpuUnitData> ReadCpuUnitData()
	{
		return OmronFinsNetHelper.ReadCpuUnitData(this);
	}

	[HslMqttApi(ApiTopic = "ReadCpuUnitStatus", Description = "读取CPU单元的一些操作状态数据，主要包含运行状态，工作模式，错误信息等。")]
	public OperateResult<OmronCpuUnitStatus> ReadCpuUnitStatus()
	{
		return OmronFinsNetHelper.ReadCpuUnitStatus(this);
	}

	[HslMqttApi(ApiTopic = "ReadCpuTime", Description = "读取CPU单元的时间信息。")]
	public OperateResult<DateTime> ReadCpuTime()
	{
		return OmronFinsNetHelper.ReadCpuTime(this);
	}

	public override string ToString()
	{
		return $"OmronFinsUdp[{IpAddress}:{Port}]";
	}
}
