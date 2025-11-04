using System;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.ModBus;

public class ModbusTcpNet : DeviceTcpNet, IModbus, IReadWriteDevice, IReadWriteNet
{
	private byte station = 1;

	private readonly SoftIncrementCount softIncrementCount;

	private bool isAddressStartWithZero = true;

	private Func<string, byte, OperateResult<string>> addressMapping = (string address, byte modbusCode) => OperateResult.CreateSuccessResult(address);

	public bool AddressStartWithZero
	{
		get
		{
			return isAddressStartWithZero;
		}
		set
		{
			isAddressStartWithZero = value;
		}
	}

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

	public bool EnableWriteMaskCode { get; set; } = true;

	public bool IsCheckMessageId { get; set; } = true;

	public bool StationCheckMacth { get; set; } = true;

	public int BroadcastStation { get; set; } = -1;

	public int WordReadBatchLength { get; set; } = 120;

	public SoftIncrementCount MessageId => softIncrementCount;

	public ModbusTcpNet()
	{
		softIncrementCount = new SoftIncrementCount(65535L, 0L);
		base.WordLength = 1;
		station = 1;
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
	}

	public ModbusTcpNet(string ipAddress, int port = 502, byte station = 1)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
		this.station = station;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new ModbusTcpMessage
		{
			IsCheckMessageId = IsCheckMessageId,
			StationCheckMatch = StationCheckMacth
		};
	}

	public override OperateResult<byte[]> ReadFromCoreServer(byte[] send)
	{
		if (BroadcastStation >= 0 && send[0] == BroadcastStation)
		{
			return ReadFromCoreServer(send, hasResponseData: false, usePackAndUnpack: true);
		}
		return base.ReadFromCoreServer(send);
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(byte[] send)
	{
		if (BroadcastStation >= 0 && send[0] == BroadcastStation)
		{
			return ReadFromCoreServer(send, hasResponseData: false, usePackAndUnpack: true);
		}
		return await base.ReadFromCoreServerAsync(send);
	}

	[HslMqttApi("ReadInt32Array", "")]
	public override OperateResult<int[]> ReadInt32(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 2)), (byte[] m) => transform.TransInt32(m, 0, length));
	}

	[HslMqttApi("ReadUInt32Array", "")]
	public override OperateResult<uint[]> ReadUInt32(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 2)), (byte[] m) => transform.TransUInt32(m, 0, length));
	}

	[HslMqttApi("ReadFloatArray", "")]
	public override OperateResult<float[]> ReadFloat(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 2)), (byte[] m) => transform.TransSingle(m, 0, length));
	}

	[HslMqttApi("ReadInt64Array", "")]
	public override OperateResult<long[]> ReadInt64(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 4)), (byte[] m) => transform.TransInt64(m, 0, length));
	}

	[HslMqttApi("ReadUInt64Array", "")]
	public override OperateResult<ulong[]> ReadUInt64(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 4)), (byte[] m) => transform.TransUInt64(m, 0, length));
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 4)), (byte[] m) => transform.TransDouble(m, 0, length));
	}

	[HslMqttApi("WriteInt32Array", "")]
	public override OperateResult Write(string address, int[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteUInt32Array", "")]
	public override OperateResult Write(string address, uint[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteFloatArray", "")]
	public override OperateResult Write(string address, float[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteInt64Array", "")]
	public override OperateResult Write(string address, long[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteUInt64Array", "")]
	public override OperateResult Write(string address, ulong[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteDoubleArray", "")]
	public override OperateResult Write(string address, double[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	public override async Task<OperateResult<int[]>> ReadInt32Async(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 2)), (byte[] m) => transform.TransInt32(m, 0, length));
	}

	public override async Task<OperateResult<uint[]>> ReadUInt32Async(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 2)), (byte[] m) => transform.TransUInt32(m, 0, length));
	}

	public override async Task<OperateResult<float[]>> ReadFloatAsync(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 2)), (byte[] m) => transform.TransSingle(m, 0, length));
	}

	public override async Task<OperateResult<long[]>> ReadInt64Async(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 4)), (byte[] m) => transform.TransInt64(m, 0, length));
	}

	public override async Task<OperateResult<ulong[]>> ReadUInt64Async(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 4)), (byte[] m) => transform.TransUInt64(m, 0, length));
	}

	public override async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 4)), (byte[] m) => transform.TransDouble(m, 0, length));
	}

	public override async Task<OperateResult> WriteAsync(string address, int[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, uint[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, float[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, long[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, ulong[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public virtual OperateResult<string> TranslateToModbusAddress(string address, byte modbusCode)
	{
		if (addressMapping != null)
		{
			return addressMapping(address, modbusCode);
		}
		return OperateResult.CreateSuccessResult(address);
	}

	public void RegisteredAddressMapping(Func<string, byte, OperateResult<string>> mapping)
	{
		addressMapping = mapping;
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return ModbusInfo.PackCommandToTcp(command, (ushort)softIncrementCount.GetCurrentValue());
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		if (BroadcastStation >= 0 && send[6] == BroadcastStation)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (response == null || response.Length < 6)
		{
			return new OperateResult<byte[]>(StringResources.Language.ReceiveDataLengthTooShort + " Content: " + response.ToHexString(' '));
		}
		return ModbusInfo.ExtractActualData(ModbusInfo.ExplodeTcpCommandToCore(response));
	}

	public OperateResult<bool> ReadCoil(string address)
	{
		return ReadBool(address);
	}

	public OperateResult<bool[]> ReadCoil(string address, ushort length)
	{
		return ReadBool(address, length);
	}

	public OperateResult<bool> ReadDiscrete(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadDiscrete(address, 1));
	}

	public OperateResult<bool[]> ReadDiscrete(string address, ushort length)
	{
		return ModbusHelper.ReadBoolHelper(this, address, length, 2);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return ModbusHelper.Read(this, address, length);
	}

	[HslMqttApi("ReadWrite", "Use 0x17 function code to write and read data at the same time, and use a message to implement it")]
	public OperateResult<byte[]> ReadWrite(string readAddress, ushort length, string writeAddress, byte[] value)
	{
		return ModbusHelper.ReadWrite(this, readAddress, length, writeAddress, value);
	}

	[HslMqttApi("ReadFile", "")]
	public OperateResult<byte[]> ReadFile(ushort fileNumber, ushort address, ushort length)
	{
		return ModbusHelper.ReadFile(this, Station, fileNumber, address, length);
	}

	[HslMqttApi("WriteFile", "")]
	public OperateResult WriteFile(ushort fileNumber, ushort address, byte[] data)
	{
		return ModbusHelper.WriteFile(this, Station, fileNumber, address, data);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return ModbusHelper.Write(this, address, value);
	}

	[HslMqttApi("WriteInt16", "")]
	public override OperateResult Write(string address, short value)
	{
		return ModbusHelper.Write(this, address, value);
	}

	[HslMqttApi("WriteUInt16", "")]
	public override OperateResult Write(string address, ushort value)
	{
		return ModbusHelper.Write(this, address, value);
	}

	[HslMqttApi("WriteMask", "")]
	public OperateResult WriteMask(string address, ushort andMask, ushort orMask)
	{
		return ModbusHelper.WriteMask(this, address, andMask, orMask);
	}

	public OperateResult WriteOneRegister(string address, short value)
	{
		return Write(address, value);
	}

	public OperateResult WriteOneRegister(string address, ushort value)
	{
		return Write(address, value);
	}

	public async Task<OperateResult<bool>> ReadCoilAsync(string address)
	{
		return await ReadBoolAsync(address);
	}

	public async Task<OperateResult<bool[]>> ReadCoilAsync(string address, ushort length)
	{
		return await ReadBoolAsync(address, length);
	}

	public async Task<OperateResult<bool>> ReadDiscreteAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadDiscreteAsync(address, 1));
	}

	public async Task<OperateResult<bool[]>> ReadDiscreteAsync(string address, ushort length)
	{
		return await ReadBoolHelperAsync(address, length, 2);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await ModbusHelper.ReadAsync(this, address, length);
	}

	public async Task<OperateResult<byte[]>> ReadWriteAsync(string readAddress, ushort length, string writeAddress, byte[] value)
	{
		return await ModbusHelper.ReadWriteAsync(this, readAddress, length, writeAddress, value);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await ModbusHelper.WriteAsync(this, address, value);
	}

	public override async Task<OperateResult> WriteAsync(string address, short value)
	{
		return await ModbusHelper.WriteAsync(this, address, value);
	}

	public override async Task<OperateResult> WriteAsync(string address, ushort value)
	{
		return await ModbusHelper.WriteAsync(this, address, value);
	}

	public async Task<OperateResult> WriteMaskAsync(string address, ushort andMask, ushort orMask)
	{
		return await ModbusHelper.WriteMaskAsync(this, address, andMask, orMask);
	}

	public async Task<OperateResult<byte[]>> ReadFileAsync(ushort fileNumber, ushort address, ushort length)
	{
		return await ModbusHelper.ReadFileAsync(this, Station, fileNumber, address, length);
	}

	public virtual async Task<OperateResult> WriteOneRegisterAsync(string address, short value)
	{
		return await WriteAsync(address, value);
	}

	public virtual async Task<OperateResult> WriteOneRegisterAsync(string address, ushort value)
	{
		return await WriteAsync(address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return ModbusHelper.ReadBoolHelper(this, address, length, 1);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] values)
	{
		return ModbusHelper.Write(this, address, values);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return ModbusHelper.Write(this, address, value);
	}

	private async Task<OperateResult<bool[]>> ReadBoolHelperAsync(string address, ushort length, byte function)
	{
		return await ModbusHelper.ReadBoolHelperAsync(this, address, length, function);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await ReadBoolHelperAsync(address, length, 1);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		return await ModbusHelper.WriteAsync(this, address, values);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await ModbusHelper.WriteAsync(this, address, value);
	}

	public override string ToString()
	{
		return $"ModbusTcpNet[{IpAddress}:{Port}]";
	}
}
