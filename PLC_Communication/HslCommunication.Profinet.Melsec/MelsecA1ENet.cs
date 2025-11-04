using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecA1ENet : DeviceTcpNet
{
	public byte PLCNumber { get; set; } = byte.MaxValue;

	public MelsecA1ENet()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
	}

	public MelsecA1ENet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new MelsecA1EBinaryMessage();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<List<byte[]>> operateResult = BuildReadCommand(address, length, isBit: false, PLCNumber);
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
				return OperateResult.CreateFailedResult<byte[]>(operateResult2);
			}
			OperateResult operateResult3 = CheckResponseLegal(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult3);
			}
			OperateResult<byte[]> operateResult4 = ExtractActualData(operateResult2.Content, isBit: false);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			list.AddRange(operateResult4.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteWordCommand(address, value, PLCNumber);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = CheckResponseLegal(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult3);
		}
		return OperateResult.CreateSuccessResult();
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<List<byte[]>> command = BuildReadCommand(address, length, isBit: false, PLCNumber);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		List<byte> array = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(read);
			}
			OperateResult check = CheckResponseLegal(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(check);
			}
			OperateResult<byte[]> extra = ExtractActualData(read.Content, isBit: false);
			if (!extra.IsSuccess)
			{
				return extra;
			}
			array.AddRange(extra.Content);
		}
		return OperateResult.CreateSuccessResult(array.ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> command = BuildWriteWordCommand(address, value, PLCNumber);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = CheckResponseLegal(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		if (address.IndexOf('.') > 0)
		{
			return HslHelper.ReadBool(this, address, length);
		}
		OperateResult<List<byte[]>> operateResult = BuildReadCommand(address, length, isBit: true, PLCNumber);
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
			OperateResult operateResult3 = CheckResponseLegal(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			OperateResult<byte[]> operateResult4 = ExtractActualData(operateResult2.Content, isBit: true);
			if (!operateResult4.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult4);
			}
			list.AddRange(operateResult4.Content);
		}
		return OperateResult.CreateSuccessResult(list.Select((byte m) => m == 1).Take(length).ToArray());
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteBoolCommand(address, value, PLCNumber);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckResponseLegal(operateResult2.Content);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		if (address.IndexOf('.') > 0)
		{
			return await HslHelper.ReadBoolAsync(this, address, length);
		}
		OperateResult<List<byte[]>> command = BuildReadCommand(address, length, isBit: true, PLCNumber);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		List<byte> array = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult check = CheckResponseLegal(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(check);
			}
			OperateResult<byte[]> extract = ExtractActualData(read.Content, isBit: true);
			if (!extract.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(extract);
			}
			array.AddRange(extract.Content);
		}
		return OperateResult.CreateSuccessResult(array.Select((byte m) => m == 1).Take(length).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		OperateResult<byte[]> command = BuildWriteBoolCommand(address, values, PLCNumber);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckResponseLegal(read.Content);
	}

	public override string ToString()
	{
		return $"MelsecA1ENet[{IpAddress}:{Port}]";
	}

	public static OperateResult<List<byte[]>> BuildReadCommand(string address, ushort length, bool isBit, byte plcNumber)
	{
		OperateResult<MelsecA1EDataType, int> operateResult = MelsecHelper.McA1EAnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<List<byte[]>>(operateResult);
		}
		byte b = ((!isBit) ? ((byte)1) : ((byte)0));
		int[] array = SoftBasic.SplitIntegerToArray(length, isBit ? 256 : 64);
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < array.Length; i++)
		{
			byte[] array2 = new byte[12]
			{
				b,
				plcNumber,
				10,
				0,
				BitConverter.GetBytes(operateResult.Content2)[0],
				BitConverter.GetBytes(operateResult.Content2)[1],
				BitConverter.GetBytes(operateResult.Content2)[2],
				BitConverter.GetBytes(operateResult.Content2)[3],
				BitConverter.GetBytes(operateResult.Content1.DataCode)[0],
				BitConverter.GetBytes(operateResult.Content1.DataCode)[1],
				0,
				0
			};
			int num = array[i];
			if (num == 256)
			{
				num = 0;
			}
			array2[10] = BitConverter.GetBytes(num)[0];
			array2[11] = BitConverter.GetBytes(num)[1];
			list.Add(array2);
			operateResult.Content2 += array[i];
		}
		return OperateResult.CreateSuccessResult(list);
	}

	public static OperateResult<byte[]> BuildWriteWordCommand(string address, byte[] value, byte plcNumber)
	{
		OperateResult<MelsecA1EDataType, int> operateResult = MelsecHelper.McA1EAnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[12 + value.Length];
		array[0] = 3;
		array[1] = plcNumber;
		array[2] = 10;
		array[3] = 0;
		array[4] = BitConverter.GetBytes(operateResult.Content2)[0];
		array[5] = BitConverter.GetBytes(operateResult.Content2)[1];
		array[6] = BitConverter.GetBytes(operateResult.Content2)[2];
		array[7] = BitConverter.GetBytes(operateResult.Content2)[3];
		array[8] = BitConverter.GetBytes(operateResult.Content1.DataCode)[0];
		array[9] = BitConverter.GetBytes(operateResult.Content1.DataCode)[1];
		array[10] = BitConverter.GetBytes(value.Length / 2)[0];
		array[11] = BitConverter.GetBytes(value.Length / 2)[1];
		Array.Copy(value, 0, array, 12, value.Length);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteBoolCommand(string address, bool[] value, byte plcNumber)
	{
		OperateResult<MelsecA1EDataType, int> operateResult = MelsecHelper.McA1EAnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = MelsecHelper.TransBoolArrayToByteData(value);
		byte[] array2 = new byte[12 + array.Length];
		array2[0] = 2;
		array2[1] = plcNumber;
		array2[2] = 10;
		array2[3] = 0;
		array2[4] = BitConverter.GetBytes(operateResult.Content2)[0];
		array2[5] = BitConverter.GetBytes(operateResult.Content2)[1];
		array2[6] = BitConverter.GetBytes(operateResult.Content2)[2];
		array2[7] = BitConverter.GetBytes(operateResult.Content2)[3];
		array2[8] = BitConverter.GetBytes(operateResult.Content1.DataCode)[0];
		array2[9] = BitConverter.GetBytes(operateResult.Content1.DataCode)[1];
		array2[10] = BitConverter.GetBytes(value.Length)[0];
		array2[11] = BitConverter.GetBytes(value.Length)[1];
		Array.Copy(array, 0, array2, 12, array.Length);
		return OperateResult.CreateSuccessResult(array2);
	}

	public static OperateResult CheckResponseLegal(byte[] response)
	{
		if (response.Length < 2)
		{
			return new OperateResult(StringResources.Language.ReceiveDataLengthTooShort);
		}
		if (response[1] == 0)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (response[1] == 91)
		{
			return new OperateResult(response[2], StringResources.Language.MelsecPleaseReferToManualDocument);
		}
		return new OperateResult(response[1], StringResources.Language.MelsecPleaseReferToManualDocument);
	}

	public static OperateResult<byte[]> ExtractActualData(byte[] response, bool isBit)
	{
		if (isBit)
		{
			byte[] array = new byte[(response.Length - 2) * 2];
			for (int i = 2; i < response.Length; i++)
			{
				if ((response[i] & 0x10) == 16)
				{
					array[(i - 2) * 2] = 1;
				}
				if ((response[i] & 1) == 1)
				{
					array[(i - 2) * 2 + 1] = 1;
				}
			}
			return OperateResult.CreateSuccessResult(array);
		}
		return OperateResult.CreateSuccessResult(response.RemoveBegin(2));
	}
}
