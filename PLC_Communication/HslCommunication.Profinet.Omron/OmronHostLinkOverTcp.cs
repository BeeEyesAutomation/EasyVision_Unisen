using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Omron.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Omron;

public class OmronHostLinkOverTcp : DeviceTcpNet, IHostLink, IReadWriteDevice, IReadWriteNet
{
	public byte ICF { get; set; } = 0;

	public byte DA2 { get; set; } = 0;

	public byte SA2 { get; set; }

	public byte SID { get; set; } = 0;

	public byte ResponseWaitTime { get; set; } = 48;

	public byte UnitNumber { get; set; }

	public int ReadSplits { get; set; } = 260;

	public OmronPlcType PlcType { get; set; } = OmronPlcType.CSCJ;

	public OmronHostLinkOverTcp()
	{
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
		base.WordLength = 1;
		LogMsgFormatBinary = false;
	}

	public OmronHostLinkOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(13);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return OmronHostLinkHelper.ResponseValidAnalysis(send, response);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return OmronHostLinkHelper.Read(this, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return OmronHostLinkHelper.Write(this, address, value);
	}

	public OperateResult<byte[]> Read(string[] address)
	{
		return OmronHostLinkHelper.Read(this, address);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await OmronHostLinkHelper.ReadAsync(this, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await OmronHostLinkHelper.WriteAsync(this, address, value);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string[] address)
	{
		return await OmronHostLinkHelper.ReadAsync(this, address);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return OmronHostLinkHelper.ReadBool(this, address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] values)
	{
		return OmronHostLinkHelper.Write(this, address, values);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await OmronHostLinkHelper.ReadBoolAsync(this, address, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		return await OmronHostLinkHelper.WriteAsync(this, address, values);
	}

	public override string ToString()
	{
		return $"OmronHostLinkOverTcp[{IpAddress}:{Port}]";
	}
}
