using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.YASKAWA.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.YASKAWA;

public class MemobusTcpNet : DeviceTcpNet, IMemobus, IReadWriteDevice, IReadWriteNet
{
	private byte cpuTo = 2;

	private byte cpuFrom = 1;

	private readonly SoftIncrementCount softIncrementCount;

	public byte CpuTo
	{
		get
		{
			return cpuTo;
		}
		set
		{
			cpuTo = value;
		}
	}

	public byte CpuFrom
	{
		get
		{
			return cpuFrom;
		}
		set
		{
			cpuFrom = value;
		}
	}

	public MemobusTcpNet()
	{
		softIncrementCount = new SoftIncrementCount(255L, 0L);
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
	}

	public MemobusTcpNet(string ipAddress, int port = 502)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new MemobusMessage();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return MemobusHelper.PackCommandWithHeader(command, softIncrementCount.GetCurrentValue());
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return MemobusHelper.UnpackResponseContent(send, response);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return MemobusHelper.ReadBool(this, address, length);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return MemobusHelper.Write(this, address, value);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return MemobusHelper.Write(this, address, value);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return MemobusHelper.Read(this, address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return MemobusHelper.Write(this, address, value);
	}

	[HslMqttApi("WriteInt16", "")]
	public override OperateResult Write(string address, short value)
	{
		if (ushort.TryParse(address, out var _))
		{
			return MemobusHelper.Write(this, address, value, base.Write);
		}
		return base.Write(address, value);
	}

	[HslMqttApi("WriteUInt16", "")]
	public override OperateResult Write(string address, ushort value)
	{
		if (ushort.TryParse(address, out var _))
		{
			return MemobusHelper.Write(this, address, value, base.Write);
		}
		return base.Write(address, value);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await MemobusHelper.ReadBoolAsync(this, address, length).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await MemobusHelper.WriteAsync(this, address, value).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await MemobusHelper.WriteAsync(this, address, value).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await MemobusHelper.ReadAsync(this, address, length).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await MemobusHelper.WriteAsync(this, address, value).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, short value)
	{
		if (ushort.TryParse(address, out var _))
		{
			return await MemobusHelper.WriteAsync(this, address, value, (string address2, short value2) => base.WriteAsync(address2, value2)).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await base.WriteAsync(address, value).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, ushort value)
	{
		if (ushort.TryParse(address, out var _))
		{
			return await MemobusHelper.WriteAsync(this, address, value, (string address2, ushort value2) => base.WriteAsync(address2, value2)).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await base.WriteAsync(address, value).ConfigureAwait(continueOnCapturedContext: false);
	}

	public OperateResult<byte[]> ReadRandom(string[] address)
	{
		return MemobusHelper.ReadRandom(this, address);
	}

	public OperateResult<byte[]> ReadRandom(ushort[] address)
	{
		return MemobusHelper.ReadRandom(this, address);
	}

	public OperateResult WriteRandom(ushort[] address, byte[] value)
	{
		return MemobusHelper.WriteRandom(this, address, value);
	}

	public async Task<OperateResult<byte[]>> ReadRandomAsync(string[] address)
	{
		return await MemobusHelper.ReadRandomAsync(this, address).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<byte[]>> ReadRandomAsync(ushort[] address)
	{
		return await MemobusHelper.ReadRandomAsync(this, address).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult> WriteRandomAsync(ushort[] address, byte[] value)
	{
		return await MemobusHelper.WriteRandomAsync(this, address, value).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override string ToString()
	{
		return $"MemobusTcpNet[{IpAddress}:{Port}]";
	}
}
