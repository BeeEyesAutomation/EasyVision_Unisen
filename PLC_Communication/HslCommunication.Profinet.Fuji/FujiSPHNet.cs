using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Fuji;

public class FujiSPHNet : DeviceTcpNet
{
	public byte ConnectionID { get; set; } = 254;

	public FujiSPHNet()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
	}

	public FujiSPHNet(string ipAddress, int port = 18245)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FujiSPHMessage();
	}

	private OperateResult<byte[]> ReadFujiSPHAddress(FujiSPHAddress address, ushort length)
	{
		OperateResult<List<byte[]>> operateResult = BuildReadCommand(ConnectionID, address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<byte[]>();
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult<byte[]> operateResult3 = ExtractActualData(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			list.AddRange(operateResult3.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<FujiSPHAddress> operateResult = FujiSPHAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<byte[]>();
		}
		return ReadFujiSPHAddress(operateResult.Content, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteCommand(ConnectionID, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<byte[]>();
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = ExtractActualData(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		OperateResult<FujiSPHAddress> operateResult = FujiSPHAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<bool[]>();
		}
		int num = operateResult.Content.BitIndex + length;
		int num2 = ((num % 16 == 0) ? (num / 16) : (num / 16 + 1));
		OperateResult<byte[]> operateResult2 = ReadFujiSPHAddress(operateResult.Content, (ushort)num2);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2.ConvertFailed<bool[]>();
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.ToBoolArray().SelectMiddle(operateResult.Content.BitIndex, length));
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		OperateResult<FujiSPHAddress> operateResult = FujiSPHAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<bool[]>();
		}
		int num = operateResult.Content.BitIndex + value.Length;
		int num2 = ((num % 16 == 0) ? (num / 16) : (num / 16 + 1));
		OperateResult<byte[]> operateResult2 = ReadFujiSPHAddress(operateResult.Content, (ushort)num2);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2.ConvertFailed<bool[]>();
		}
		bool[] array = operateResult2.Content.ToBoolArray();
		value.CopyTo(array, operateResult.Content.BitIndex);
		OperateResult<byte[]> operateResult3 = BuildWriteCommand(ConnectionID, address, array.ToByteArray());
		if (!operateResult3.IsSuccess)
		{
			return operateResult3.ConvertFailed<byte[]>();
		}
		OperateResult<byte[]> operateResult4 = ReadFromCoreServer(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return operateResult4;
		}
		OperateResult<byte[]> operateResult5 = ExtractActualData(operateResult4.Content);
		if (!operateResult5.IsSuccess)
		{
			return operateResult5;
		}
		return OperateResult.CreateSuccessResult();
	}

	private async Task<OperateResult<byte[]>> ReadFujiSPHAddressAsync(FujiSPHAddress address, ushort length)
	{
		OperateResult<List<byte[]>> command = BuildReadCommand(ConnectionID, address, length);
		if (!command.IsSuccess)
		{
			return command.ConvertFailed<byte[]>();
		}
		List<byte> array = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return read;
			}
			OperateResult<byte[]> extra = ExtractActualData(read.Content);
			if (!extra.IsSuccess)
			{
				return extra;
			}
			array.AddRange(extra.Content);
		}
		return OperateResult.CreateSuccessResult(array.ToArray());
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<FujiSPHAddress> analysis = FujiSPHAddress.ParseFrom(address);
		if (!analysis.IsSuccess)
		{
			return analysis.ConvertFailed<byte[]>();
		}
		return await ReadFujiSPHAddressAsync(analysis.Content, length);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> command = BuildWriteCommand(ConnectionID, address, value);
		if (!command.IsSuccess)
		{
			return command.ConvertFailed<byte[]>();
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> extra = ExtractActualData(read.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult();
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		OperateResult<FujiSPHAddress> analysis = FujiSPHAddress.ParseFrom(address);
		if (!analysis.IsSuccess)
		{
			return analysis.ConvertFailed<bool[]>();
		}
		int bitCount = analysis.Content.BitIndex + length;
		OperateResult<byte[]> read = await ReadFujiSPHAddressAsync(length: (ushort)((bitCount % 16 == 0) ? (bitCount / 16) : (bitCount / 16 + 1)), address: analysis.Content);
		if (!read.IsSuccess)
		{
			return read.ConvertFailed<bool[]>();
		}
		return OperateResult.CreateSuccessResult(read.Content.ToBoolArray().SelectMiddle(analysis.Content.BitIndex, length));
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		OperateResult<FujiSPHAddress> analysis = FujiSPHAddress.ParseFrom(address);
		if (!analysis.IsSuccess)
		{
			return analysis.ConvertFailed<bool[]>();
		}
		int bitCount = analysis.Content.BitIndex + value.Length;
		OperateResult<byte[]> read = await ReadFujiSPHAddressAsync(length: (ushort)((bitCount % 16 == 0) ? (bitCount / 16) : (bitCount / 16 + 1)), address: analysis.Content);
		if (!read.IsSuccess)
		{
			return read.ConvertFailed<bool[]>();
		}
		bool[] writeBoolArray = read.Content.ToBoolArray();
		value.CopyTo(writeBoolArray, analysis.Content.BitIndex);
		OperateResult<byte[]> command = BuildWriteCommand(ConnectionID, address, writeBoolArray.ToByteArray());
		if (!command.IsSuccess)
		{
			return command.ConvertFailed<byte[]>();
		}
		OperateResult<byte[]> write = await ReadFromCoreServerAsync(command.Content);
		if (!write.IsSuccess)
		{
			return write;
		}
		OperateResult<byte[]> extra = ExtractActualData(write.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi]
	public OperateResult CpuBatchStart()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return ReadFromCoreServer(PackCommand(ConnectionID, 4, 0, null)).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	[HslMqttApi]
	public OperateResult CpuBatchInitializeAndStart()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return ReadFromCoreServer(PackCommand(ConnectionID, 4, 1, null)).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	[HslMqttApi]
	public OperateResult CpuBatchStop()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return ReadFromCoreServer(PackCommand(ConnectionID, 4, 2, null)).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	[HslMqttApi]
	public OperateResult CpuBatchReset()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return ReadFromCoreServer(PackCommand(ConnectionID, 4, 3, null)).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	[HslMqttApi]
	public OperateResult CpuIndividualStart()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return ReadFromCoreServer(PackCommand(ConnectionID, 4, 4, null)).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	[HslMqttApi]
	public OperateResult CpuIndividualInitializeAndStart()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return ReadFromCoreServer(PackCommand(ConnectionID, 4, 5, null)).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	[HslMqttApi]
	public OperateResult CpuIndividualStop()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return ReadFromCoreServer(PackCommand(ConnectionID, 4, 6, null)).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	[HslMqttApi]
	public OperateResult CpuIndividualReset()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return ReadFromCoreServer(PackCommand(ConnectionID, 4, 7, null)).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public async Task<OperateResult> CpuBatchStartAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return (await ReadFromCoreServerAsync(PackCommand(ConnectionID, 4, 0, null))).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public async Task<OperateResult> CpuBatchInitializeAndStartAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return (await ReadFromCoreServerAsync(PackCommand(ConnectionID, 4, 1, null))).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public async Task<OperateResult> CpuBatchStopAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return (await ReadFromCoreServerAsync(PackCommand(ConnectionID, 4, 2, null))).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public async Task<OperateResult> CpuBatchResetAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return (await ReadFromCoreServerAsync(PackCommand(ConnectionID, 4, 3, null))).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public async Task<OperateResult> CpuIndividualStartAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return (await ReadFromCoreServerAsync(PackCommand(ConnectionID, 4, 4, null))).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public async Task<OperateResult> CpuIndividualInitializeAndStartAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return (await ReadFromCoreServerAsync(PackCommand(ConnectionID, 4, 5, null))).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public async Task<OperateResult> CpuIndividualStopAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return (await ReadFromCoreServerAsync(PackCommand(ConnectionID, 4, 6, null))).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public async Task<OperateResult> CpuIndividualResetAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		return (await ReadFromCoreServerAsync(PackCommand(ConnectionID, 4, 7, null))).Check((Func<byte[], OperateResult>)ExtractActualData);
	}

	public override string ToString()
	{
		return $"FujiSPHNet[{IpAddress}:{Port}]";
	}

	public static string GetErrorDescription(byte code)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			16 => "Command cannot be executed because an error occurred in the CPU.", 
			17 => "Command cannot be executed because the CPU is running.", 
			18 => "Command cannot be executed due to the key switch condition of the CPU.", 
			32 => "CPU received undefined command or mode.", 
			34 => "Setting error was found in command header part.", 
			35 => "Transmission is interlocked by a command from another device.", 
			40 => "Requested command cannot be executed because another command is now being executed.", 
			43 => "Requested command cannot be executed because the loader is now performing another processing( including program change).", 
			47 => "Requested command cannot be executed because the system is now being initialized.", 
			64 => "Invalid data type or number was specified.", 
			65 => "Specified data cannot be found.", 
			68 => "Specified address exceeds the valid range.", 
			69 => "Address + the number of read/write words exceed the valid range.", 
			160 => "No module exists at specified destination station No.", 
			162 => "No response data is returned from the destination module.", 
			164 => "Command cannot be communicated because an error occurred in the SX bus.", 
			165 => "Command cannot be communicated because NAK occurred while sending data via the SX bus.", 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static byte[] PackCommand(byte connectionId, byte command, byte mode, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		byte[] array = new byte[20 + data.Length];
		array[0] = 251;
		array[1] = 128;
		array[2] = 128;
		array[3] = 0;
		array[4] = byte.MaxValue;
		array[5] = 123;
		array[6] = connectionId;
		array[7] = 0;
		array[8] = 17;
		array[9] = 0;
		array[10] = 0;
		array[11] = 0;
		array[12] = 0;
		array[13] = 0;
		array[14] = command;
		array[15] = mode;
		array[16] = 0;
		array[17] = 1;
		array[18] = BitConverter.GetBytes(data.Length)[0];
		array[19] = BitConverter.GetBytes(data.Length)[1];
		if (data.Length != 0)
		{
			data.CopyTo(array, 20);
		}
		return array;
	}

	public static OperateResult<List<byte[]>> BuildReadCommand(byte connectionId, string address, ushort length)
	{
		OperateResult<FujiSPHAddress> operateResult = FujiSPHAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<List<byte[]>>();
		}
		return BuildReadCommand(connectionId, operateResult.Content, length);
	}

	public static OperateResult<List<byte[]>> BuildReadCommand(byte connectionId, FujiSPHAddress address, ushort length)
	{
		List<byte[]> list = new List<byte[]>();
		int[] array = SoftBasic.SplitIntegerToArray(length, 230);
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(PackCommand(connectionId, 0, 0, new byte[6]
			{
				address.TypeCode,
				BitConverter.GetBytes(address.AddressStart)[0],
				BitConverter.GetBytes(address.AddressStart)[1],
				BitConverter.GetBytes(address.AddressStart)[2],
				BitConverter.GetBytes(array[i])[0],
				BitConverter.GetBytes(array[i])[1]
			}));
			address.AddressStart += array[i];
		}
		return OperateResult.CreateSuccessResult(list);
	}

	public static OperateResult<byte[]> BuildWriteCommand(byte connectionId, string address, byte[] data)
	{
		OperateResult<FujiSPHAddress> operateResult = FujiSPHAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<byte[]>();
		}
		int value = data.Length / 2;
		byte[] array = new byte[6 + data.Length];
		array[0] = operateResult.Content.TypeCode;
		array[1] = BitConverter.GetBytes(operateResult.Content.AddressStart)[0];
		array[2] = BitConverter.GetBytes(operateResult.Content.AddressStart)[1];
		array[3] = BitConverter.GetBytes(operateResult.Content.AddressStart)[2];
		array[4] = BitConverter.GetBytes(value)[0];
		array[5] = BitConverter.GetBytes(value)[1];
		data.CopyTo(array, 6);
		return OperateResult.CreateSuccessResult(PackCommand(connectionId, 1, 0, array));
	}

	public static OperateResult<byte[]> ExtractActualData(byte[] response)
	{
		try
		{
			if (response[4] != 0)
			{
				return new OperateResult<byte[]>(response[4], GetErrorDescription(response[4]));
			}
			if (response.Length > 26)
			{
				return OperateResult.CreateSuccessResult(response.RemoveBegin(26));
			}
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message + " Source: " + response.ToHexString(' '));
		}
	}
}
