using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Yokogawa;

public class YokogawaLinkTcp : DeviceTcpNet
{
	public byte CpuNumber { get; set; }

	public YokogawaLinkTcp()
	{
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
		CpuNumber = 1;
	}

	public YokogawaLinkTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new YokogawaLinkBinaryMessage();
	}

	[HslMqttApi("ReadByteArray", "Supports X,Y,I,E,M,T,C,L,D,B,F,R,V,Z,W,TN,CN, for example: D100; or cpu=2;D100 or Special:unit=0;slot=1;100")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<List<byte[]>> operateResult;
		if (address.StartsWith("Special:") || address.StartsWith("special:"))
		{
			if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
			{
				return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
			}
			operateResult = BuildReadSpecialModule(CpuNumber, address, length);
		}
		else
		{
			operateResult = BuildReadCommand(CpuNumber, address, length, isBit: false);
		}
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult<byte[]> operateResult3 = CheckContent(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			list.AddRange(operateResult3.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	[HslMqttApi("WriteByteArray", "Supports Y,I,E,M,T,C,L,D,B,F,R,V,Z,W,TN,CN, for example: D100; or cpu=2;D100 or Special:unit=0;slot=1;100")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult;
		if (address.StartsWith("Special:") || address.StartsWith("special:"))
		{
			if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
			{
				return new OperateResult(StringResources.Language.InsufficientPrivileges);
			}
			operateResult = BuildWriteSpecialModule(CpuNumber, address, value);
		}
		else
		{
			operateResult = BuildWriteWordCommand(CpuNumber, address, value);
		}
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckContent(operateResult2.Content);
	}

	[HslMqttApi("ReadBoolArray", "Read coil address supports X, Y, I, E, M, T, C, L, for example: Y100; or cpu=2;Y100")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		OperateResult<List<byte[]>> operateResult = BuildReadCommand(CpuNumber, address, length, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult2);
			}
			OperateResult<byte[]> operateResult3 = CheckContent(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			list.AddRange(operateResult3.Content);
		}
		return OperateResult.CreateSuccessResult((from m in list.ToArray()
			select m != 0).ToArray());
	}

	[HslMqttApi("WriteBoolArray", "The write coil address supports Y, I, E, M, T, C, L, for example: Y100; or cpu=2;Y100")]
	public override OperateResult Write(string address, bool[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteBoolCommand(CpuNumber, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckContent(operateResult2.Content);
	}

	[HslMqttApi(Description = "Read random relay, supports X, Y, I, E, M, T, C, L, for example: Y100;")]
	public OperateResult<bool[]> ReadRandomBool(string[] address)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<bool[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<List<byte[]>> operateResult = BuildReadRandomCommand(CpuNumber, address, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		List<bool> list = new List<bool>();
		foreach (byte[] item in operateResult.Content)
		{
			OperateResult<byte[]> operateResult2 = ReadFromCoreServer(item);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult2);
			}
			OperateResult<byte[]> operateResult3 = CheckContent(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			list.AddRange(operateResult3.Content.Select((byte m) => m != 0));
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	[HslMqttApi(Description = "Write random relay, supports Y, I, E, M, T, C, L, for example: Y100;")]
	public OperateResult WriteRandomBool(string[] address, bool[] value)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<bool[]>(StringResources.Language.InsufficientPrivileges);
		}
		if (address.Length != value.Length)
		{
			return new OperateResult(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		OperateResult<byte[]> operateResult = BuildWriteRandomBoolCommand(CpuNumber, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = CheckContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi(Description = "Read random register, supports D,B,F,R,V,Z,W,TN,CN，example: D100")]
	public OperateResult<byte[]> ReadRandom(string[] address)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<List<byte[]>> operateResult = BuildReadRandomCommand(CpuNumber, address, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		foreach (byte[] item in operateResult.Content)
		{
			OperateResult<byte[]> operateResult2 = ReadFromCoreServer(item);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult2);
			}
			OperateResult<byte[]> operateResult3 = CheckContent(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult3);
			}
			list.AddRange(operateResult3.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	[HslMqttApi(Description = "Read random register, and get short array, supports D, B, F, R, V, Z, W, TN, CN，example: D100")]
	public OperateResult<short[]> ReadRandomInt16(string[] address)
	{
		return ReadRandom(address).Then((byte[] m) => OperateResult.CreateSuccessResult(base.ByteTransform.TransInt16(m, 0, address.Length)));
	}

	[HslMqttApi(Description = "Read random register, and get ushort array, supports D, B, F, R, V, Z, W, TN, CN，example: D100")]
	public OperateResult<ushort[]> ReadRandomUInt16(string[] address)
	{
		return ReadRandom(address).Then((byte[] m) => OperateResult.CreateSuccessResult(base.ByteTransform.TransUInt16(m, 0, address.Length)));
	}

	[HslMqttApi(ApiTopic = "WriteRandom", Description = "Randomly write the byte array information, the main need to pass in the byte array address information")]
	public OperateResult WriteRandom(string[] address, byte[] value)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<bool[]>(StringResources.Language.InsufficientPrivileges);
		}
		if (address.Length * 2 != value.Length)
		{
			return new OperateResult(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		OperateResult<byte[]> operateResult = BuildWriteRandomWordCommand(CpuNumber, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = CheckContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi(ApiTopic = "WriteRandomInt16", Description = "Randomly write the short array information, the main need to pass in the short array address information")]
	public OperateResult WriteRandom(string[] address, short[] value)
	{
		return WriteRandom(address, base.ByteTransform.TransByte(value));
	}

	[HslMqttApi(ApiTopic = "WriteRandomUInt16", Description = "Randomly write the ushort array information, the main need to pass in the ushort array address information")]
	public OperateResult WriteRandom(string[] address, ushort[] value)
	{
		return WriteRandom(address, base.ByteTransform.TransByte(value));
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<List<byte[]>> command;
		if (address.StartsWith("Special:") || address.StartsWith("special:"))
		{
			if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
			{
				return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
			}
			command = BuildReadSpecialModule(CpuNumber, address, length);
		}
		else
		{
			command = BuildReadCommand(CpuNumber, address, length, isBit: false);
		}
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		List<byte> content = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return read;
			}
			OperateResult<byte[]> check = CheckContent(read.Content);
			if (!check.IsSuccess)
			{
				return check;
			}
			content.AddRange(check.Content);
		}
		return OperateResult.CreateSuccessResult(content.ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> command;
		if (address.StartsWith("Special:") || address.StartsWith("special:"))
		{
			if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
			{
				return new OperateResult(StringResources.Language.InsufficientPrivileges);
			}
			command = BuildWriteSpecialModule(CpuNumber, address, value);
		}
		else
		{
			command = BuildWriteWordCommand(CpuNumber, address, value);
		}
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckContent(read.Content);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		OperateResult<List<byte[]>> command = BuildReadCommand(CpuNumber, address, length, isBit: true);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		List<byte> content = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult<byte[]> check = CheckContent(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(check);
			}
			content.AddRange(check.Content);
		}
		return OperateResult.CreateSuccessResult((from m in content.ToArray()
			select m != 0).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		OperateResult<byte[]> command = BuildWriteBoolCommand(CpuNumber, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckContent(read.Content);
	}

	public async Task<OperateResult<bool[]>> ReadRandomBoolAsync(string[] address)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<bool[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<List<byte[]>> command = BuildReadRandomCommand(CpuNumber, address, isBit: true);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		List<bool> lists = new List<bool>();
		foreach (byte[] content in command.Content)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(content);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult<byte[]> check = CheckContent(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(check);
			}
			lists.AddRange(check.Content.Select((byte m) => m != 0));
		}
		return OperateResult.CreateSuccessResult(lists.ToArray());
	}

	public async Task<OperateResult> WriteRandomBoolAsync(string[] address, bool[] value)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<bool[]>(StringResources.Language.InsufficientPrivileges);
		}
		if (address.Length != value.Length)
		{
			return new OperateResult(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		OperateResult<byte[]> command = BuildWriteRandomBoolCommand(CpuNumber, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> check = CheckContent(read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult<byte[]>> ReadRandomAsync(string[] address)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<List<byte[]>> command = BuildReadRandomCommand(CpuNumber, address, isBit: false);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		List<byte> lists = new List<byte>();
		foreach (byte[] content in command.Content)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(content);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(read);
			}
			OperateResult<byte[]> check = CheckContent(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(check);
			}
			lists.AddRange(check.Content);
		}
		return OperateResult.CreateSuccessResult(lists.ToArray());
	}

	public async Task<OperateResult<short[]>> ReadRandomInt16Async(string[] address)
	{
		return (await ReadRandomAsync(address)).Then((byte[] m) => OperateResult.CreateSuccessResult(base.ByteTransform.TransInt16(m, 0, address.Length)));
	}

	public async Task<OperateResult<ushort[]>> ReadRandomUInt16Async(string[] address)
	{
		return (await ReadRandomAsync(address)).Then((byte[] m) => OperateResult.CreateSuccessResult(base.ByteTransform.TransUInt16(m, 0, address.Length)));
	}

	public async Task<OperateResult> WriteRandomAsync(string[] address, byte[] value)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<bool[]>(StringResources.Language.InsufficientPrivileges);
		}
		if (address.Length * 2 != value.Length)
		{
			return new OperateResult(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		OperateResult<byte[]> command = BuildWriteRandomWordCommand(CpuNumber, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> check = CheckContent(read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult> WriteRandomAsync(string[] address, short[] value)
	{
		return await WriteRandomAsync(address, base.ByteTransform.TransByte(value));
	}

	public async Task<OperateResult> WriteRandomAsync(string[] address, ushort[] value)
	{
		return await WriteRandomAsync(address, base.ByteTransform.TransByte(value));
	}

	[HslMqttApi(Description = "Starts executing a program if it is not being executed")]
	public OperateResult Start()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> operateResult = BuildStartCommand(CpuNumber);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = CheckContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi(Description = "Stops the executing program.")]
	public OperateResult Stop()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> operateResult = BuildStopCommand(CpuNumber);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = CheckContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi(Description = "Reset the connection which is currently open is forced to close")]
	public OperateResult ModuleReset()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> operateResult = ReadFromCoreServer(new byte[4] { 97, CpuNumber, 0, 0 }, hasResponseData: false, usePackAndUnpack: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi(Description = "Read the program status. return code 1:RUN; 2:Stop; 3:Debug; 255:ROM writer")]
	public OperateResult<int> ReadProgramStatus()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<int>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] array = new byte[6] { 98, 0, 0, 2, 0, 1 };
		array[1] = CpuNumber;
		OperateResult<byte[]> operateResult = ReadFromCoreServer(array);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = CheckContent(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(operateResult2);
		}
		return OperateResult.CreateSuccessResult((int)operateResult2.Content[1]);
	}

	[HslMqttApi(Description = "Read current PLC system status, system ID, CPU type, program size information")]
	public OperateResult<YokogawaSystemInfo> ReadSystemInfo()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<YokogawaSystemInfo>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] array = new byte[6] { 98, 0, 0, 2, 0, 2 };
		array[1] = CpuNumber;
		OperateResult<byte[]> operateResult = ReadFromCoreServer(array);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<YokogawaSystemInfo>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = CheckContent(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<YokogawaSystemInfo>(operateResult2);
		}
		return YokogawaSystemInfo.Parse(operateResult2.Content);
	}

	[HslMqttApi(Description = "Read current PLC time information, including year, month, day, hour, minute, and second")]
	public OperateResult<DateTime> ReadDateTime()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<DateTime>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> operateResult = ReadFromCoreServer(new byte[4] { 99, CpuNumber, 0, 0 });
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = CheckContent(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(new DateTime(2000 + base.ByteTransform.TransUInt16(operateResult2.Content, 0), base.ByteTransform.TransUInt16(operateResult2.Content, 2), base.ByteTransform.TransUInt16(operateResult2.Content, 4), base.ByteTransform.TransUInt16(operateResult2.Content, 6), base.ByteTransform.TransUInt16(operateResult2.Content, 8), base.ByteTransform.TransUInt16(operateResult2.Content, 10)));
	}

	[HslMqttApi(Description = "Read the data information of a special module, you need to specify the module unit number, module slot number, data address, and length information.")]
	public OperateResult<byte[]> ReadSpecialModule(byte moduleUnit, byte moduleSlot, ushort dataPosition, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		List<byte> list = new List<byte>();
		List<byte[]> list2 = BuildReadSpecialModule(CpuNumber, moduleUnit, moduleSlot, dataPosition, length);
		for (int i = 0; i < list2.Count; i++)
		{
			OperateResult<byte[]> operateResult = ReadFromCoreServer(list2[i]);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			OperateResult<byte[]> operateResult2 = CheckContent(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult2);
			}
			list.AddRange(operateResult2.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public async Task<OperateResult> StartAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> command = BuildStartCommand(CpuNumber);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> check = CheckContent(read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult> StopAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> command = BuildStopCommand(CpuNumber);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> check = CheckContent(read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult> ModuleResetAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(new byte[4] { 97, CpuNumber, 0, 0 }, hasResponseData: false, usePackAndUnpack: true);
		if (!read.IsSuccess)
		{
			return read;
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult<int>> ReadProgramStatusAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<int>(StringResources.Language.InsufficientPrivileges);
		}
		YokogawaLinkTcp yokogawaLinkTcp = this;
		byte[] obj = new byte[6] { 98, 0, 0, 2, 0, 1 };
		obj[1] = CpuNumber;
		OperateResult<byte[]> read = await yokogawaLinkTcp.ReadFromCoreServerAsync(obj);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(read);
		}
		OperateResult<byte[]> check = CheckContent(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(check);
		}
		return OperateResult.CreateSuccessResult((int)check.Content[1]);
	}

	public async Task<OperateResult<YokogawaSystemInfo>> ReadSystemInfoAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<YokogawaSystemInfo>(StringResources.Language.InsufficientPrivileges);
		}
		YokogawaLinkTcp yokogawaLinkTcp = this;
		byte[] obj = new byte[6] { 98, 0, 0, 2, 0, 2 };
		obj[1] = CpuNumber;
		OperateResult<byte[]> read = await yokogawaLinkTcp.ReadFromCoreServerAsync(obj);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<YokogawaSystemInfo>(read);
		}
		OperateResult<byte[]> check = CheckContent(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<YokogawaSystemInfo>(check);
		}
		return YokogawaSystemInfo.Parse(check.Content);
	}

	public async Task<OperateResult<DateTime>> ReadDateTimeAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<DateTime>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(new byte[4] { 99, CpuNumber, 0, 0 });
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(read);
		}
		OperateResult<byte[]> check = CheckContent(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(check);
		}
		return OperateResult.CreateSuccessResult(new DateTime(2000 + base.ByteTransform.TransUInt16(check.Content, 0), base.ByteTransform.TransUInt16(check.Content, 2), base.ByteTransform.TransUInt16(check.Content, 4), base.ByteTransform.TransUInt16(check.Content, 6), base.ByteTransform.TransUInt16(check.Content, 8), base.ByteTransform.TransUInt16(check.Content, 10)));
	}

	public async Task<OperateResult<byte[]>> ReadSpecialModuleAsync(byte moduleUnit, byte moduleSlot, ushort dataPosition, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		List<byte> content = new List<byte>();
		List<byte[]> commands = BuildReadSpecialModule(CpuNumber, moduleUnit, moduleSlot, dataPosition, length);
		for (int i = 0; i < commands.Count; i++)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(commands[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(read);
			}
			OperateResult<byte[]> check = CheckContent(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(check);
			}
			content.AddRange(check.Content);
		}
		return OperateResult.CreateSuccessResult(content.ToArray());
	}

	public static OperateResult<byte[]> CheckContent(byte[] content)
	{
		if (content == null || content.Length < 2)
		{
			return new OperateResult<byte[]>(StringResources.Language.ReceiveDataLengthTooShort + " 2 content: " + content.ToHexString(' '));
		}
		if (content[1] != 0)
		{
			return new OperateResult<byte[]>(YokogawaLinkHelper.GetErrorMsg(content[1]));
		}
		if (content.Length > 4)
		{
			return OperateResult.CreateSuccessResult(content.RemoveBegin(4));
		}
		return OperateResult.CreateSuccessResult(new byte[0]);
	}

	public static OperateResult<List<byte[]>> BuildReadCommand(byte cpu, string address, ushort length, bool isBit)
	{
		cpu = (byte)HslHelper.ExtractParameter(ref address, "cpu", cpu);
		OperateResult<YokogawaLinkAddress> operateResult = YokogawaLinkAddress.ParseFrom(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<List<byte[]>>(operateResult);
		}
		OperateResult<int[], int[]> operateResult2 = ((!Authorization.asdniasnfaksndiqwhawfskhfaiw()) ? HslHelper.SplitReadLength(operateResult.Content.AddressStart, length, 65535) : ((!isBit) ? HslHelper.SplitReadLength(operateResult.Content.AddressStart, length, 64) : HslHelper.SplitReadLength(operateResult.Content.AddressStart, length, 256)));
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < operateResult2.Content1.Length; i++)
		{
			operateResult.Content.AddressStart = operateResult2.Content1[i];
			byte[] array = new byte[12]
			{
				(byte)(isBit ? 1u : 17u),
				cpu,
				0,
				8,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			operateResult.Content.GetAddressBinaryContent().CopyTo(array, 4);
			array[10] = BitConverter.GetBytes(operateResult2.Content2[i])[1];
			array[11] = BitConverter.GetBytes(operateResult2.Content2[i])[0];
			list.Add(array);
		}
		return OperateResult.CreateSuccessResult(list);
	}

	public static OperateResult<List<byte[]>> BuildReadRandomCommand(byte cpu, string[] address, bool isBit)
	{
		List<string[]> list = SoftBasic.ArraySplitByLength(address, 32);
		List<byte[]> list2 = new List<byte[]>();
		foreach (string[] item in list)
		{
			byte[] array = new byte[6 + 6 * item.Length];
			array[0] = (byte)(isBit ? 4u : 20u);
			array[1] = cpu;
			array[2] = BitConverter.GetBytes(array.Length - 4)[1];
			array[3] = BitConverter.GetBytes(array.Length - 4)[0];
			array[4] = BitConverter.GetBytes(item.Length)[1];
			array[5] = BitConverter.GetBytes(item.Length)[0];
			for (int i = 0; i < item.Length; i++)
			{
				array[1] = (byte)HslHelper.ExtractParameter(ref item[i], "cpu", cpu);
				OperateResult<YokogawaLinkAddress> operateResult = YokogawaLinkAddress.ParseFrom(item[i], 1);
				if (!operateResult.IsSuccess)
				{
					return OperateResult.CreateFailedResult<List<byte[]>>(operateResult);
				}
				operateResult.Content.GetAddressBinaryContent().CopyTo(array, 6 * i + 6);
			}
			list2.Add(array);
		}
		return OperateResult.CreateSuccessResult(list2);
	}

	public static OperateResult<byte[]> BuildWriteBoolCommand(byte cpu, string address, bool[] value)
	{
		cpu = (byte)HslHelper.ExtractParameter(ref address, "cpu", cpu);
		OperateResult<YokogawaLinkAddress> operateResult = YokogawaLinkAddress.ParseFrom(address, 0);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[12 + value.Length];
		array[0] = 2;
		array[1] = cpu;
		array[2] = 0;
		array[3] = (byte)(8 + value.Length);
		operateResult.Content.GetAddressBinaryContent().CopyTo(array, 4);
		array[10] = BitConverter.GetBytes(value.Length)[1];
		array[11] = BitConverter.GetBytes(value.Length)[0];
		for (int i = 0; i < value.Length; i++)
		{
			array[12 + i] = (byte)(value[i] ? 1u : 0u);
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteRandomBoolCommand(byte cpu, string[] address, bool[] value)
	{
		if (address.Length != value.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		byte[] array = new byte[6 + address.Length * 8 - 1];
		array[0] = 5;
		array[1] = cpu;
		array[2] = BitConverter.GetBytes(array.Length - 4)[1];
		array[3] = BitConverter.GetBytes(array.Length - 4)[0];
		array[4] = BitConverter.GetBytes(address.Length)[1];
		array[5] = BitConverter.GetBytes(address.Length)[0];
		for (int i = 0; i < address.Length; i++)
		{
			array[1] = (byte)HslHelper.ExtractParameter(ref address[i], "cpu", cpu);
			OperateResult<YokogawaLinkAddress> operateResult = YokogawaLinkAddress.ParseFrom(address[i], 0);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			operateResult.Content.GetAddressBinaryContent().CopyTo(array, 6 + 8 * i);
			array[12 + 8 * i] = (byte)(value[i] ? 1u : 0u);
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteWordCommand(byte cpu, string address, byte[] value)
	{
		cpu = (byte)HslHelper.ExtractParameter(ref address, "cpu", cpu);
		OperateResult<YokogawaLinkAddress> operateResult = YokogawaLinkAddress.ParseFrom(address, 0);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[12 + value.Length];
		array[0] = 18;
		array[1] = cpu;
		array[2] = 0;
		array[3] = (byte)(8 + value.Length);
		operateResult.Content.GetAddressBinaryContent().CopyTo(array, 4);
		array[10] = BitConverter.GetBytes(value.Length / 2)[1];
		array[11] = BitConverter.GetBytes(value.Length / 2)[0];
		value.CopyTo(array, 12);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteRandomWordCommand(byte cpu, string[] address, byte[] value)
	{
		if (address.Length * 2 != value.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		byte[] array = new byte[6 + address.Length * 8];
		array[0] = 21;
		array[1] = cpu;
		array[2] = BitConverter.GetBytes(array.Length - 4)[1];
		array[3] = BitConverter.GetBytes(array.Length - 4)[0];
		array[4] = BitConverter.GetBytes(address.Length)[1];
		array[5] = BitConverter.GetBytes(address.Length)[0];
		for (int i = 0; i < address.Length; i++)
		{
			array[1] = (byte)HslHelper.ExtractParameter(ref address[i], "cpu", cpu);
			OperateResult<YokogawaLinkAddress> operateResult = YokogawaLinkAddress.ParseFrom(address[i], 0);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			operateResult.Content.GetAddressBinaryContent().CopyTo(array, 6 + 8 * i);
			array[12 + 8 * i] = value[i * 2];
			array[13 + 8 * i] = value[i * 2 + 1];
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildStartCommand(byte cpu)
	{
		return OperateResult.CreateSuccessResult(new byte[4] { 69, cpu, 0, 0 });
	}

	public static OperateResult<byte[]> BuildStopCommand(byte cpu)
	{
		return OperateResult.CreateSuccessResult(new byte[4] { 70, cpu, 0, 0 });
	}

	public static List<byte[]> BuildReadSpecialModule(byte cpu, byte moduleUnit, byte moduleSlot, ushort dataPosition, ushort length)
	{
		List<byte[]> list = new List<byte[]>();
		OperateResult<int[], int[]> operateResult = HslHelper.SplitReadLength(dataPosition, length, 64);
		for (int i = 0; i < operateResult.Content1.Length; i++)
		{
			byte[] array = new byte[10];
			array[0] = 49;
			array[1] = cpu;
			array[2] = BitConverter.GetBytes(array.Length - 4)[1];
			array[3] = BitConverter.GetBytes(array.Length - 4)[0];
			array[4] = moduleUnit;
			array[5] = moduleSlot;
			array[6] = BitConverter.GetBytes(operateResult.Content1[i])[1];
			array[7] = BitConverter.GetBytes(operateResult.Content1[i])[0];
			array[8] = BitConverter.GetBytes(operateResult.Content2[i])[1];
			array[9] = BitConverter.GetBytes(operateResult.Content2[i])[0];
			list.Add(array);
		}
		return list;
	}

	public static OperateResult<List<byte[]>> BuildReadSpecialModule(byte cpu, string address, ushort length)
	{
		if (address.StartsWith("Special:") || address.StartsWith("special:"))
		{
			address = address.Substring(8);
			cpu = (byte)HslHelper.ExtractParameter(ref address, "cpu", cpu);
			OperateResult<int> operateResult = HslHelper.ExtractParameter(ref address, "unit");
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<List<byte[]>>(operateResult);
			}
			OperateResult<int> operateResult2 = HslHelper.ExtractParameter(ref address, "slot");
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<List<byte[]>>(operateResult2);
			}
			try
			{
				return OperateResult.CreateSuccessResult(BuildReadSpecialModule(cpu, (byte)operateResult.Content, (byte)operateResult2.Content, ushort.Parse(address), length));
			}
			catch (Exception ex)
			{
				return new OperateResult<List<byte[]>>("Address format wrong: " + ex.Message);
			}
		}
		return new OperateResult<List<byte[]>>("Special module address must start with Special:");
	}

	public static byte[] BuildWriteSpecialModule(byte cpu, byte moduleUnit, byte moduleSlot, ushort dataPosition, byte[] data)
	{
		byte[] array = new byte[10 + data.Length];
		array[0] = 50;
		array[1] = cpu;
		array[2] = BitConverter.GetBytes(array.Length - 4)[1];
		array[3] = BitConverter.GetBytes(array.Length - 4)[0];
		array[4] = moduleUnit;
		array[5] = moduleSlot;
		array[6] = BitConverter.GetBytes(dataPosition)[1];
		array[7] = BitConverter.GetBytes(dataPosition)[0];
		array[8] = BitConverter.GetBytes(data.Length / 2)[1];
		array[9] = BitConverter.GetBytes(data.Length / 2)[0];
		data.CopyTo(array, 10);
		return array;
	}

	public static OperateResult<byte[]> BuildWriteSpecialModule(byte cpu, string address, byte[] data)
	{
		OperateResult<List<byte[]>> operateResult = BuildReadSpecialModule(cpu, address, 0);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[10 + data.Length];
		array[0] = 50;
		array[1] = operateResult.Content[0][1];
		array[2] = BitConverter.GetBytes(array.Length - 4)[1];
		array[3] = BitConverter.GetBytes(array.Length - 4)[0];
		array[4] = operateResult.Content[0][4];
		array[5] = operateResult.Content[0][5];
		array[6] = operateResult.Content[0][6];
		array[7] = operateResult.Content[0][7];
		array[8] = BitConverter.GetBytes(data.Length / 2)[1];
		array[9] = BitConverter.GetBytes(data.Length / 2)[0];
		data.CopyTo(array, 10);
		return OperateResult.CreateSuccessResult(array);
	}

	public override string ToString()
	{
		return $"YokogawaLinkTcp[{IpAddress}:{Port}]";
	}
}
