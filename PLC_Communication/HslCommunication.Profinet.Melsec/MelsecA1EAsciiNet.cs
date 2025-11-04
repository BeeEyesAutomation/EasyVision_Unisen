using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecA1EAsciiNet : DeviceTcpNet
{
	public byte PLCNumber { get; set; } = byte.MaxValue;

	public MelsecA1EAsciiNet()
	{
		base.WordLength = 1;
		LogMsgFormatBinary = false;
		base.ByteTransform = new RegularByteTransform();
	}

	public MelsecA1EAsciiNet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new MelsecA1EAsciiMessage();
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
	public override OperateResult Write(string address, bool[] values)
	{
		OperateResult<byte[]> operateResult = BuildWriteBoolCommand(address, values, PLCNumber);
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
		byte value = ((!isBit) ? ((byte)1) : ((byte)0));
		int[] array = SoftBasic.SplitIntegerToArray(length, isBit ? 256 : 64);
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < array.Length; i++)
		{
			byte[] array2 = new byte[24]
			{
				SoftBasic.BuildAsciiBytesFrom(value)[0],
				SoftBasic.BuildAsciiBytesFrom(value)[1],
				SoftBasic.BuildAsciiBytesFrom(plcNumber)[0],
				SoftBasic.BuildAsciiBytesFrom(plcNumber)[1],
				48,
				48,
				48,
				65,
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[1])[0],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[1])[1],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[0])[0],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[0])[1],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[3])[0],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[3])[1],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[2])[0],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[2])[1],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[1])[0],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[1])[1],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[0])[0],
				SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[0])[1],
				0,
				0,
				0,
				0
			};
			int num = array[i];
			if (num == 256)
			{
				num = 0;
			}
			array2[20] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(num % 256)[0])[0];
			array2[21] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(num % 256)[0])[1];
			array2[22] = 48;
			array2[23] = 48;
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
		value = MelsecHelper.TransByteArrayToAsciiByteArray(value);
		byte[] array = new byte[24 + value.Length];
		array[0] = 48;
		array[1] = 51;
		array[2] = SoftBasic.BuildAsciiBytesFrom(plcNumber)[0];
		array[3] = SoftBasic.BuildAsciiBytesFrom(plcNumber)[1];
		array[4] = 48;
		array[5] = 48;
		array[6] = 48;
		array[7] = 65;
		array[8] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[1])[0];
		array[9] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[1])[1];
		array[10] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[0])[0];
		array[11] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[0])[1];
		array[12] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[3])[0];
		array[13] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[3])[1];
		array[14] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[2])[0];
		array[15] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[2])[1];
		array[16] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[1])[0];
		array[17] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[1])[1];
		array[18] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[0])[0];
		array[19] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[0])[1];
		array[20] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(value.Length / 4)[0])[0];
		array[21] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(value.Length / 4)[0])[1];
		array[22] = 48;
		array[23] = 48;
		value.CopyTo(array, 24);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteBoolCommand(string address, bool[] value, byte plcNumber)
	{
		OperateResult<MelsecA1EDataType, int> operateResult = MelsecHelper.McA1EAnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = value.Select((bool m) => (byte)(m ? 49u : 48u)).ToArray();
		if (array.Length % 2 == 1)
		{
			array = SoftBasic.SpliceArray<byte>(array, new byte[1] { 48 });
		}
		byte[] array2 = new byte[24 + array.Length];
		array2[0] = 48;
		array2[1] = 50;
		array2[2] = SoftBasic.BuildAsciiBytesFrom(plcNumber)[0];
		array2[3] = SoftBasic.BuildAsciiBytesFrom(plcNumber)[1];
		array2[4] = 48;
		array2[5] = 48;
		array2[6] = 48;
		array2[7] = 65;
		array2[8] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[1])[0];
		array2[9] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[1])[1];
		array2[10] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[0])[0];
		array2[11] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content1.DataCode)[0])[1];
		array2[12] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[3])[0];
		array2[13] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[3])[1];
		array2[14] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[2])[0];
		array2[15] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[2])[1];
		array2[16] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[1])[0];
		array2[17] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[1])[1];
		array2[18] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[0])[0];
		array2[19] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Content2)[0])[1];
		array2[20] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(value.Length)[0])[0];
		array2[21] = SoftBasic.BuildAsciiBytesFrom(BitConverter.GetBytes(value.Length)[0])[1];
		array2[22] = 48;
		array2[23] = 48;
		array.CopyTo(array2, 24);
		return OperateResult.CreateSuccessResult(array2);
	}

	public static OperateResult CheckResponseLegal(byte[] response)
	{
		if (response.Length < 4)
		{
			return new OperateResult(StringResources.Language.ReceiveDataLengthTooShort);
		}
		if (response[2] == 48 && response[3] == 48)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (response[2] == 53 && response[3] == 66)
		{
			return new OperateResult(Convert.ToInt32(Encoding.ASCII.GetString(response, 4, 2), 16), StringResources.Language.MelsecPleaseReferToManualDocument);
		}
		return new OperateResult(Convert.ToInt32(Encoding.ASCII.GetString(response, 2, 2), 16), StringResources.Language.MelsecPleaseReferToManualDocument);
	}

	public static OperateResult<byte[]> ExtractActualData(byte[] response, bool isBit)
	{
		if (isBit)
		{
			return OperateResult.CreateSuccessResult((from m in response.RemoveBegin(4)
				select (m != 48) ? ((byte)1) : ((byte)0)).ToArray());
		}
		return OperateResult.CreateSuccessResult(MelsecHelper.TransAsciiByteArrayToByteArray(response.RemoveBegin(4)));
	}
}
