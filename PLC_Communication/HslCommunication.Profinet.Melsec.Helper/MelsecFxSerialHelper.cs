using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Profinet.Melsec.Helper;

public class MelsecFxSerialHelper
{
	public static bool CheckReceiveDataComplete(byte[] buffer)
	{
		if (buffer.Length == 0)
		{
			return false;
		}
		if (buffer.Length == 1)
		{
			if (buffer[0] == 21)
			{
				return true;
			}
			if (buffer[0] == 6)
			{
				return true;
			}
		}
		if (buffer.Length == 2 && buffer[0] == 6 && buffer[1] == 3)
		{
			return true;
		}
		int num = FindSTXIndex(buffer);
		if (num > 0)
		{
			return CheckReceiveDataCompleteHelper(buffer.RemoveBegin(num));
		}
		return CheckReceiveDataCompleteHelper(buffer);
	}

	private static bool CheckReceiveDataCompleteHelper(byte[] buffer)
	{
		if (buffer[0] == 2 && buffer.Length >= 5 && buffer[buffer.Length - 3] == 3 && MelsecHelper.CheckCRC(buffer))
		{
			return true;
		}
		return false;
	}

	internal static int FindSTXIndex(byte[] buffer)
	{
		int result = 0;
		for (int i = 0; i < buffer.Length; i++)
		{
			if (buffer[i] != 63)
			{
				result = ((buffer[i] == 2) ? i : 0);
				break;
			}
		}
		return result;
	}

	public static OperateResult<byte[]> Read(IReadWriteDevice plc, string address, ushort length, bool isNewVersion)
	{
		OperateResult<List<byte[]>> operateResult = BuildReadWordCommand(address, length, isNewVersion);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult2);
			}
			OperateResult operateResult3 = CheckPlcReadResponse(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult3);
			}
			OperateResult<byte[]> operateResult4 = ExtractActualData(operateResult2.Content);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			list.AddRange(operateResult4.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteDevice plc, string address, ushort length, bool isNewVersion)
	{
		OperateResult<List<byte[]>, int> operateResult = BuildReadBoolCommand(address, length, isNewVersion);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < operateResult.Content1.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content1[i]);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult2);
			}
			OperateResult operateResult3 = CheckPlcReadResponse(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			OperateResult<byte[]> operateResult4 = ExtractActualData(operateResult2.Content);
			if (!operateResult4.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult4);
			}
			list.AddRange(operateResult4.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray().ToBoolArray().SelectMiddle(operateResult.Content2, length));
	}

	public static OperateResult Write(IReadWriteDevice plc, string address, byte[] value, bool isNewVersion)
	{
		OperateResult<byte[]> operateResult = BuildWriteWordCommand(address, value, isNewVersion);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckPlcWriteResponse(operateResult2.Content);
	}

	public static OperateResult Write(IReadWriteDevice plc, string address, bool[] value, bool isNewVersion)
	{
		if (value == null)
		{
			value = new bool[0];
		}
		if (value.Length % 16 != 0)
		{
			return new OperateResult(StringResources.Language.MelsecFxBoolLength16);
		}
		return Write(plc, address, value.ToByteArray(), isNewVersion);
	}

	public static OperateResult Write(IReadWriteDevice plc, string address, bool value)
	{
		OperateResult<byte[]> operateResult = BuildWriteBoolPacket(address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckPlcWriteResponse(operateResult2.Content);
	}

	public static OperateResult ActivePlc(IReadWriteDevice plc)
	{
		OperateResult<byte[]> operateResult = plc.ReadFromCoreServer(new byte[1] { 5 });
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content[0] != 6)
		{
			return new OperateResult("Send ENQ(0x05), Check Receive ACK(0x06) failed");
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(new byte[11]
		{
			2, 48, 48, 69, 48, 50, 48, 50, 3, 54,
			67
		});
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return plc.ReadFromCoreServer(new byte[11]
		{
			2, 48, 48, 69, 48, 50, 48, 50, 3, 54,
			67
		});
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteDevice plc, string address, ushort length, bool isNewVersion)
	{
		OperateResult<List<byte[]>> command = BuildReadWordCommand(address, length, isNewVersion);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		List<byte> array = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(read);
			}
			OperateResult ackResult = CheckPlcReadResponse(read.Content);
			if (!ackResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(ackResult);
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

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteDevice plc, string address, ushort length, bool isNewVersion)
	{
		OperateResult<List<byte[]>, int> command = BuildReadBoolCommand(address, length, isNewVersion);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		List<byte> array = new List<byte>();
		for (int i = 0; i < command.Content1.Count; i++)
		{
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content1[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult ackResult = CheckPlcReadResponse(read.Content);
			if (!ackResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(ackResult);
			}
			OperateResult<byte[]> extra = ExtractActualData(read.Content);
			if (!extra.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(extra);
			}
			array.AddRange(extra.Content);
		}
		return OperateResult.CreateSuccessResult(array.ToArray().ToBoolArray().SelectMiddle(command.Content2, length));
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, string address, byte[] value, bool isNewVersion)
	{
		OperateResult<byte[]> command = BuildWriteWordCommand(address, value, isNewVersion);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckPlcWriteResponse(read.Content);
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, string address, bool value)
	{
		OperateResult<byte[]> command = BuildWriteBoolPacket(address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckPlcWriteResponse(read.Content);
	}

	public static async Task<OperateResult> ActivePlcAsync(IReadWriteDevice plc)
	{
		OperateResult<byte[]> read1 = await plc.ReadFromCoreServerAsync(new byte[1] { 5 });
		if (!read1.IsSuccess)
		{
			return read1;
		}
		if (read1.Content[0] != 6)
		{
			return new OperateResult("Send ENQ(0x05), Check Receive ACK(0x06) failed");
		}
		OperateResult<byte[]> read2 = await plc.ReadFromCoreServerAsync(new byte[11]
		{
			2, 48, 48, 69, 48, 50, 48, 50, 3, 54,
			67
		});
		if (!read2.IsSuccess)
		{
			return read2;
		}
		return plc.ReadFromCoreServer(new byte[11]
		{
			2, 48, 48, 69, 48, 50, 48, 50, 3, 54,
			67
		});
	}

	public static OperateResult CheckPlcReadResponse(byte[] ack)
	{
		if (ack.Length == 0)
		{
			return new OperateResult(StringResources.Language.MelsecFxReceiveZero);
		}
		if (ack[0] == 21)
		{
			return new OperateResult(StringResources.Language.MelsecFxAckNagative + " Actual: " + SoftBasic.ByteToHexString(ack, ' '));
		}
		if (ack[0] != 2)
		{
			return new OperateResult(StringResources.Language.MelsecFxAckWrong + ack[0] + " Actual: " + SoftBasic.ByteToHexString(ack, ' '));
		}
		try
		{
			if (!MelsecHelper.CheckCRC(ack))
			{
				return new OperateResult(StringResources.Language.MelsecFxCrcCheckFailed + " Actual: " + SoftBasic.ByteToHexString(ack, ' '));
			}
		}
		catch (Exception ex)
		{
			return new OperateResult(StringResources.Language.MelsecFxCrcCheckFailed + ex.Message + Environment.NewLine + "Actual: " + SoftBasic.ByteToHexString(ack, ' '));
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult CheckPlcWriteResponse(byte[] ack)
	{
		if (ack.Length == 0)
		{
			return new OperateResult(StringResources.Language.MelsecFxReceiveZero);
		}
		if (ack[0] == 21)
		{
			return new OperateResult(StringResources.Language.MelsecFxAckNagative + " Actual: " + SoftBasic.ByteToHexString(ack, ' '));
		}
		if (ack[0] != 6)
		{
			return new OperateResult(StringResources.Language.MelsecFxAckWrong + ack[0] + " Actual: " + SoftBasic.ByteToHexString(ack, ' '));
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult<byte[]> BuildWriteBoolPacket(string address, bool value)
	{
		OperateResult<MelsecMcDataType, ushort> operateResult = FxAnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		ushort content = operateResult.Content2;
		if (operateResult.Content1 == MelsecMcDataType.M)
		{
			content = ((content < 8000) ? ((ushort)(content + 2048)) : ((ushort)(content - 8000 + 3840)));
		}
		else if (operateResult.Content1 == MelsecMcDataType.S)
		{
			content = content;
		}
		else if (operateResult.Content1 == MelsecMcDataType.X)
		{
			content += 1024;
		}
		else if (operateResult.Content1 == MelsecMcDataType.Y)
		{
			content += 1280;
		}
		else if (operateResult.Content1 == MelsecMcDataType.CS)
		{
			content += 448;
		}
		else if (operateResult.Content1 == MelsecMcDataType.CC)
		{
			content += 960;
		}
		else if (operateResult.Content1 == MelsecMcDataType.CN)
		{
			content += 3584;
		}
		else if (operateResult.Content1 == MelsecMcDataType.TS)
		{
			content += 192;
		}
		else if (operateResult.Content1 == MelsecMcDataType.TC)
		{
			content += 704;
		}
		else
		{
			if (operateResult.Content1 != MelsecMcDataType.TN)
			{
				return new OperateResult<byte[]>(StringResources.Language.MelsecCurrentTypeNotSupportedBitOperate);
			}
			content += 1536;
		}
		byte[] array = new byte[9]
		{
			2,
			(byte)(value ? 55u : 56u),
			SoftBasic.BuildAsciiBytesFrom(content)[2],
			SoftBasic.BuildAsciiBytesFrom(content)[3],
			SoftBasic.BuildAsciiBytesFrom(content)[0],
			SoftBasic.BuildAsciiBytesFrom(content)[1],
			3,
			0,
			0
		};
		MelsecHelper.FxCalculateCRC(array).CopyTo(array, 7);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<List<byte[]>> BuildReadWordCommand(string address, ushort length, bool isNewVersion)
	{
		OperateResult<ushort> operateResult = FxCalculateWordStartAddress(address, isNewVersion);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<List<byte[]>>(operateResult);
		}
		length *= 2;
		ushort num = operateResult.Content;
		int[] array = SoftBasic.SplitIntegerToArray(length, 254);
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < array.Length; i++)
		{
			if (isNewVersion)
			{
				byte[] obj = new byte[13]
				{
					2, 69, 48, 48, 0, 0, 0, 0, 0, 0,
					3, 0, 0
				};
				obj[4] = SoftBasic.BuildAsciiBytesFrom(num)[0];
				obj[5] = SoftBasic.BuildAsciiBytesFrom(num)[1];
				obj[6] = SoftBasic.BuildAsciiBytesFrom(num)[2];
				obj[7] = SoftBasic.BuildAsciiBytesFrom(num)[3];
				obj[8] = SoftBasic.BuildAsciiBytesFrom((byte)array[i])[0];
				obj[9] = SoftBasic.BuildAsciiBytesFrom((byte)array[i])[1];
				byte[] array2 = obj;
				MelsecHelper.FxCalculateCRC(array2).CopyTo(array2, 11);
				list.Add(array2);
				num = (ushort)(num + array[i]);
			}
			else
			{
				byte[] obj2 = new byte[11]
				{
					2, 48, 0, 0, 0, 0, 0, 0, 3, 0,
					0
				};
				obj2[2] = SoftBasic.BuildAsciiBytesFrom(num)[0];
				obj2[3] = SoftBasic.BuildAsciiBytesFrom(num)[1];
				obj2[4] = SoftBasic.BuildAsciiBytesFrom(num)[2];
				obj2[5] = SoftBasic.BuildAsciiBytesFrom(num)[3];
				obj2[6] = SoftBasic.BuildAsciiBytesFrom((byte)array[i])[0];
				obj2[7] = SoftBasic.BuildAsciiBytesFrom((byte)array[i])[1];
				byte[] array3 = obj2;
				MelsecHelper.FxCalculateCRC(array3).CopyTo(array3, 9);
				list.Add(array3);
				num = (ushort)(num + array[i]);
			}
		}
		return OperateResult.CreateSuccessResult(list);
	}

	public static OperateResult<List<byte[]>, int> BuildReadBoolCommand(string address, ushort length, bool isNewVersion)
	{
		OperateResult<ushort, ushort, ushort> operateResult = FxCalculateBoolStartAddress(address, isNewVersion);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<List<byte[]>, int>(operateResult);
		}
		ushort integer = (ushort)HslHelper.CalculateOccupyLength(operateResult.Content2, length);
		ushort num = operateResult.Content1;
		int[] array = SoftBasic.SplitIntegerToArray(integer, 254);
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < array.Length; i++)
		{
			if (isNewVersion)
			{
				byte[] obj = new byte[13]
				{
					2, 69, 48, 48, 0, 0, 0, 0, 0, 0,
					3, 0, 0
				};
				obj[4] = SoftBasic.BuildAsciiBytesFrom(num)[0];
				obj[5] = SoftBasic.BuildAsciiBytesFrom(num)[1];
				obj[6] = SoftBasic.BuildAsciiBytesFrom(num)[2];
				obj[7] = SoftBasic.BuildAsciiBytesFrom(num)[3];
				obj[8] = SoftBasic.BuildAsciiBytesFrom((byte)array[i])[0];
				obj[9] = SoftBasic.BuildAsciiBytesFrom((byte)array[i])[1];
				byte[] array2 = obj;
				MelsecHelper.FxCalculateCRC(array2).CopyTo(array2, 11);
				list.Add(array2);
			}
			else
			{
				byte[] obj2 = new byte[11]
				{
					2, 48, 0, 0, 0, 0, 0, 0, 3, 0,
					0
				};
				obj2[2] = SoftBasic.BuildAsciiBytesFrom(num)[0];
				obj2[3] = SoftBasic.BuildAsciiBytesFrom(num)[1];
				obj2[4] = SoftBasic.BuildAsciiBytesFrom(num)[2];
				obj2[5] = SoftBasic.BuildAsciiBytesFrom(num)[3];
				obj2[6] = SoftBasic.BuildAsciiBytesFrom((byte)array[i])[0];
				obj2[7] = SoftBasic.BuildAsciiBytesFrom((byte)array[i])[1];
				byte[] array3 = obj2;
				MelsecHelper.FxCalculateCRC(array3).CopyTo(array3, 9);
				list.Add(array3);
			}
			num = (ushort)(num + array[i]);
		}
		return OperateResult.CreateSuccessResult(list, (int)operateResult.Content3);
	}

	public static OperateResult<byte[]> BuildWriteWordCommand(string address, byte[] value, bool isNewVersion)
	{
		OperateResult<ushort> operateResult = FxCalculateWordStartAddress(address, isNewVersion);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (value != null)
		{
			value = SoftBasic.BuildAsciiBytesFrom(value);
		}
		ushort content = operateResult.Content;
		if (isNewVersion)
		{
			byte[] array = new byte[13 + value.Length];
			array[0] = 2;
			array[1] = 69;
			array[2] = 49;
			array[3] = 48;
			array[4] = SoftBasic.BuildAsciiBytesFrom(content)[0];
			array[5] = SoftBasic.BuildAsciiBytesFrom(content)[1];
			array[6] = SoftBasic.BuildAsciiBytesFrom(content)[2];
			array[7] = SoftBasic.BuildAsciiBytesFrom(content)[3];
			array[8] = SoftBasic.BuildAsciiBytesFrom((byte)(value.Length / 2))[0];
			array[9] = SoftBasic.BuildAsciiBytesFrom((byte)(value.Length / 2))[1];
			Array.Copy(value, 0, array, 10, value.Length);
			array[array.Length - 3] = 3;
			MelsecHelper.FxCalculateCRC(array).CopyTo(array, array.Length - 2);
			return OperateResult.CreateSuccessResult(array);
		}
		byte[] array2 = new byte[11 + value.Length];
		array2[0] = 2;
		array2[1] = 49;
		array2[2] = SoftBasic.BuildAsciiBytesFrom(content)[0];
		array2[3] = SoftBasic.BuildAsciiBytesFrom(content)[1];
		array2[4] = SoftBasic.BuildAsciiBytesFrom(content)[2];
		array2[5] = SoftBasic.BuildAsciiBytesFrom(content)[3];
		array2[6] = SoftBasic.BuildAsciiBytesFrom((byte)(value.Length / 2))[0];
		array2[7] = SoftBasic.BuildAsciiBytesFrom((byte)(value.Length / 2))[1];
		Array.Copy(value, 0, array2, 8, value.Length);
		array2[array2.Length - 3] = 3;
		MelsecHelper.FxCalculateCRC(array2).CopyTo(array2, array2.Length - 2);
		return OperateResult.CreateSuccessResult(array2);
	}

	public static OperateResult<byte[]> ExtractActualData(byte[] response)
	{
		try
		{
			byte[] array = new byte[(response.Length - 4) / 2];
			for (int i = 0; i < array.Length; i++)
			{
				byte[] bytes = new byte[2]
				{
					response[i * 2 + 1],
					response[i * 2 + 2]
				};
				array[i] = Convert.ToByte(Encoding.ASCII.GetString(bytes), 16);
			}
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			OperateResult<byte[]> operateResult = new OperateResult<byte[]>();
			operateResult.Message = "Extract Msg：" + ex.Message + Environment.NewLine + "Data: " + SoftBasic.ByteToHexString(response);
			return operateResult;
		}
	}

	public static OperateResult<bool[]> ExtractActualBoolData(byte[] response, int start, int length)
	{
		OperateResult<byte[]> operateResult = ExtractActualData(response);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		try
		{
			bool[] array = new bool[length];
			bool[] array2 = SoftBasic.ByteToBoolArray(operateResult.Content, operateResult.Content.Length * 8);
			for (int i = 0; i < length; i++)
			{
				array[i] = array2[i + start];
			}
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			OperateResult<bool[]> operateResult2 = new OperateResult<bool[]>();
			operateResult2.Message = "Extract Msg：" + ex.Message + Environment.NewLine + "Data: " + SoftBasic.ByteToHexString(response);
			return operateResult2;
		}
	}

	public static OperateResult<MelsecMcDataType, ushort> FxAnalysisAddress(string address)
	{
		OperateResult<MelsecMcDataType, ushort> operateResult = new OperateResult<MelsecMcDataType, ushort>();
		try
		{
			switch (address[0])
			{
			case 'M':
			case 'm':
				operateResult.Content1 = MelsecMcDataType.M;
				operateResult.Content2 = Convert.ToUInt16(address.Substring(1), MelsecMcDataType.M.FromBase);
				break;
			case 'X':
			case 'x':
				operateResult.Content1 = MelsecMcDataType.X;
				operateResult.Content2 = Convert.ToUInt16(address.Substring(1), 8);
				break;
			case 'Y':
			case 'y':
				operateResult.Content1 = MelsecMcDataType.Y;
				operateResult.Content2 = Convert.ToUInt16(address.Substring(1), 8);
				break;
			case 'D':
			case 'd':
				operateResult.Content1 = MelsecMcDataType.D;
				operateResult.Content2 = Convert.ToUInt16(address.Substring(1), MelsecMcDataType.D.FromBase);
				break;
			case 'S':
			case 's':
				operateResult.Content1 = MelsecMcDataType.S;
				operateResult.Content2 = Convert.ToUInt16(address.Substring(1), MelsecMcDataType.S.FromBase);
				break;
			case 'T':
			case 't':
				if (address[1] == 'N' || address[1] == 'n')
				{
					operateResult.Content1 = MelsecMcDataType.TN;
					operateResult.Content2 = Convert.ToUInt16(address.Substring(2), MelsecMcDataType.TN.FromBase);
					break;
				}
				if (address[1] == 'S' || address[1] == 's')
				{
					operateResult.Content1 = MelsecMcDataType.TS;
					operateResult.Content2 = Convert.ToUInt16(address.Substring(2), MelsecMcDataType.TS.FromBase);
					break;
				}
				if (address[1] == 'C' || address[1] == 'c')
				{
					operateResult.Content1 = MelsecMcDataType.TC;
					operateResult.Content2 = Convert.ToUInt16(address.Substring(2), MelsecMcDataType.TC.FromBase);
					break;
				}
				throw new Exception(StringResources.Language.NotSupportedDataType);
			case 'C':
			case 'c':
				if (address[1] == 'N' || address[1] == 'n')
				{
					operateResult.Content1 = MelsecMcDataType.CN;
					operateResult.Content2 = Convert.ToUInt16(address.Substring(2), MelsecMcDataType.CN.FromBase);
					break;
				}
				if (address[1] == 'S' || address[1] == 's')
				{
					operateResult.Content1 = MelsecMcDataType.CS;
					operateResult.Content2 = Convert.ToUInt16(address.Substring(2), MelsecMcDataType.CS.FromBase);
					break;
				}
				if (address[1] == 'C' || address[1] == 'c')
				{
					operateResult.Content1 = MelsecMcDataType.CC;
					operateResult.Content2 = Convert.ToUInt16(address.Substring(2), MelsecMcDataType.CC.FromBase);
					break;
				}
				throw new Exception(StringResources.Language.NotSupportedDataType);
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
		}
		catch (Exception ex)
		{
			operateResult.Message = ex.Message;
			return operateResult;
		}
		operateResult.IsSuccess = true;
		return operateResult;
	}

	private static bool CheckAddressBool(MelsecMcDataType dataType)
	{
		return dataType == MelsecMcDataType.X || dataType == MelsecMcDataType.Y || dataType == MelsecMcDataType.M || dataType == MelsecMcDataType.S || dataType == MelsecMcDataType.CS || dataType == MelsecMcDataType.CC || dataType == MelsecMcDataType.TS || dataType == MelsecMcDataType.TC;
	}

	private static ushort CalculateBoolStartAddress(MelsecMcDataType dataType, ushort startAddress, bool isNewVersion)
	{
		if (dataType == MelsecMcDataType.M)
		{
			startAddress = ((!isNewVersion) ? ((startAddress < 8000) ? ((ushort)(startAddress / 8 + 256)) : ((ushort)((startAddress - 8000) / 8 + 480))) : ((startAddress < 8000) ? ((ushort)(startAddress / 8 + 34816)) : ((ushort)((startAddress - 8000) / 8 + 35840))));
		}
		else if (dataType == MelsecMcDataType.X)
		{
			startAddress = (ushort)(startAddress / 8 + (isNewVersion ? 36000 : 128));
		}
		else if (dataType == MelsecMcDataType.Y)
		{
			startAddress = (ushort)(startAddress / 8 + (isNewVersion ? 35776 : 160));
		}
		else if (dataType == MelsecMcDataType.S)
		{
			startAddress = (ushort)(startAddress / 8 + (isNewVersion ? 36064 : 0));
		}
		else if (dataType == MelsecMcDataType.CS)
		{
			startAddress = (ushort)(startAddress / 8 + (isNewVersion ? 37696 : 448));
		}
		else if (dataType == MelsecMcDataType.CC)
		{
			startAddress = (ushort)(startAddress / 8 + (isNewVersion ? 37600 : 960));
		}
		else if (dataType == MelsecMcDataType.TS)
		{
			startAddress = (ushort)(startAddress / 8 + (isNewVersion ? 37728 : 192));
		}
		else if (dataType == MelsecMcDataType.TC)
		{
			startAddress = (ushort)(startAddress / 8 + (isNewVersion ? 37632 : 704));
		}
		return startAddress;
	}

	internal static OperateResult<ushort> FxCalculateWordStartAddress(string address, bool isNewVersion)
	{
		OperateResult<MelsecMcDataType, ushort> operateResult = FxAnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort>(operateResult);
		}
		ushort content = operateResult.Content2;
		if (operateResult.Content1 == MelsecMcDataType.D)
		{
			content = ((content >= 8000) ? ((ushort)((content - 8000) * 2 + (isNewVersion ? 32768 : 3584))) : (isNewVersion ? ((ushort)(content * 2 + 16384)) : ((ushort)(content * 2 + 4096))));
		}
		else if (operateResult.Content1 == MelsecMcDataType.CN)
		{
			content = ((content < 200) ? ((ushort)(content * 2 + 2560)) : ((ushort)((content - 200) * 4 + 3072)));
		}
		else
		{
			if (operateResult.Content1 != MelsecMcDataType.TN)
			{
				if (CheckAddressBool(operateResult.Content1))
				{
					if (operateResult.Content2 % 16 != 0)
					{
						return new OperateResult<ushort>(StringResources.Language.MelsecFxAddressStartWith16);
					}
					return OperateResult.CreateSuccessResult(CalculateBoolStartAddress(operateResult.Content1, content, isNewVersion));
				}
				return new OperateResult<ushort>(StringResources.Language.MelsecCurrentTypeNotSupportedWordOperate);
			}
			content = ((!isNewVersion) ? ((ushort)(content * 2 + 2048)) : ((ushort)(content * 2 + 4096)));
		}
		return OperateResult.CreateSuccessResult(content);
	}

	public static OperateResult<ushort, ushort, ushort> FxCalculateBoolStartAddress(string address, bool isNewVersion)
	{
		OperateResult<MelsecMcDataType, ushort> operateResult = FxAnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort, ushort, ushort>(operateResult);
		}
		ushort content = operateResult.Content2;
		if (CheckAddressBool(operateResult.Content1))
		{
			content = CalculateBoolStartAddress(operateResult.Content1, content, isNewVersion);
			return OperateResult.CreateSuccessResult(content, operateResult.Content2, (ushort)(operateResult.Content2 % 8));
		}
		return new OperateResult<ushort, ushort, ushort>(StringResources.Language.MelsecCurrentTypeNotSupportedBitOperate);
	}
}
