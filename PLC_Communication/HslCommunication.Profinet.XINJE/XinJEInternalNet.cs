using System.Collections.Generic;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.ModBus;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.XINJE;

public class XinJEInternalNet : DeviceTcpNet
{
	private byte station = 1;

	private readonly SoftIncrementCount softIncrementCount;

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

	public DataFormat DataFormat
	{
		get
		{
			return base.ByteTransform.DataFormat;
		}
		set
		{
			base.ByteTransform.DataFormat = value;
		}
	}

	public bool IsStringReverse
	{
		get
		{
			return base.ByteTransform.IsStringReverseByteWord;
		}
		set
		{
			base.ByteTransform.IsStringReverseByteWord = value;
		}
	}

	public SoftIncrementCount MessageId => softIncrementCount;

	public XinJEInternalNet()
	{
		softIncrementCount = new SoftIncrementCount(65535L, 0L);
		base.WordLength = 1;
		station = 1;
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
	}

	public XinJEInternalNet(string ipAddress, int port = 502, byte station = 1)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
		this.station = station;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new ModbusTcpMessage();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return ModbusInfo.PackCommandToTcp(command, (ushort)softIncrementCount.GetCurrentValue());
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return ModbusInfo.ExtractActualData(ModbusInfo.ExplodeTcpCommandToCore(response));
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<List<byte[]>> operateResult = XinJEHelper.BuildReadCommand(Station, address, length, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		OperateResult<List<byte[]>> operateResult = XinJEHelper.BuildReadCommand(Station, address, length, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.ToBoolArray().SelectBegin(length));
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = XinJEHelper.BuildWriteWordCommand(Station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		OperateResult<byte[]> operateResult = XinJEHelper.BuildWriteBoolCommand(Station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<List<byte[]>> command = XinJEHelper.BuildReadCommand(Station, address, length, isBit: false);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		OperateResult<List<byte[]>> command = XinJEHelper.BuildReadCommand(Station, address, length, isBit: true);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.ToBoolArray().SelectBegin(length));
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> command = XinJEHelper.BuildWriteWordCommand(Station, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		OperateResult<byte[]> command = XinJEHelper.BuildWriteBoolCommand(Station, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override string ToString()
	{
		return $"XinJEInternalNet[{IpAddress}:{Port}]";
	}
}
