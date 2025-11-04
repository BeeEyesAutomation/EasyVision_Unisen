using System;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Profinet.YASKAWA.Helper;

public class MemobusHelper
{
	internal static byte[] PackCommandWithHeader(byte[] command, long id)
	{
		byte[] array = new byte[12 + command.Length];
		array[0] = 17;
		array[1] = (byte)id;
		array[2] = 0;
		array[3] = 0;
		array[6] = BitConverter.GetBytes(array.Length)[0];
		array[7] = BitConverter.GetBytes(array.Length)[1];
		command.CopyTo(array, 12);
		return array;
	}

	internal static string GetErrorText(byte err)
	{
		if (1 == 0)
		{
		}
		string result = err switch
		{
			1 => StringResources.Language.Memobus01, 
			2 => StringResources.Language.Memobus02, 
			3 => StringResources.Language.Memobus03, 
			64 => StringResources.Language.Memobus40, 
			65 => StringResources.Language.Memobus41, 
			66 => StringResources.Language.Memobus42, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	internal static OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		try
		{
			if (send.Length > 15 && response.Length > 15)
			{
				if (send[15] + 128 == response[15] && response.Length >= 18)
				{
					return new OperateResult<byte[]>(response[17], GetErrorText(response[17]) + " Source: " + response.ToHexString(' '));
				}
				if (send[15] != response[15])
				{
					return new OperateResult<byte[]>(response[15], "Send SFC not same as back SFC:" + response.ToHexString());
				}
			}
			return OperateResult.CreateSuccessResult(response.RemoveBegin(12));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("UnpackResponseContent failed: " + ex.Message + "  Source: " + response.ToHexString(' '));
		}
	}

	private static void SetByteHead(byte[] buffer, byte mfc, byte sfc, byte cpuTo, byte cpuFrom)
	{
		buffer[0] = BitConverter.GetBytes(buffer.Length - 2)[0];
		buffer[1] = BitConverter.GetBytes(buffer.Length - 2)[1];
		buffer[2] = mfc;
		buffer[3] = sfc;
		buffer[4] = (byte)((cpuTo << 4) + cpuFrom);
	}

	internal static byte GetAddressDataType(string address)
	{
		byte result = 77;
		if (address[0] == 'M' || address[0] == 'm')
		{
			result = 77;
		}
		else if (address[0] == 'G' || address[0] == 'g')
		{
			result = 71;
		}
		else if (address[0] == 'I' || address[0] == 'i')
		{
			result = 73;
		}
		else if (address[0] == 'O' || address[0] == 'o')
		{
			result = 79;
		}
		else if (address[0] == 'S' || address[0] == 's')
		{
			result = 83;
		}
		return result;
	}

	private static int CalculateBoolIndex(string address)
	{
		address = ((address[1] != 'B' && address[1] != 'b') ? address.Substring(1) : address.Substring(2));
		int num = address.IndexOf('.');
		if (num > 0)
		{
			return Convert.ToInt32(address.Substring(0, num)) * 16 + HslHelper.CalculateBitStartIndex(address.Substring(num + 1));
		}
		return Convert.ToInt32(address.Substring(0, address.Length - 1)) * 16 + HslHelper.CalculateBitStartIndex(address.Substring(address.Length - 1));
	}

	internal static OperateResult<byte[]> BuildReadCommand(byte mfc, byte sfc, byte cpuTo, byte cpuFrom, string address, ushort length)
	{
		if (address.StartsWith(new string[5] { "M", "G", "I", "O", "S" }))
		{
			byte addressDataType = GetAddressDataType(address);
			if (address[1] == 'B' || address[1] == 'b' || address.IndexOf('.') > 0)
			{
				int value = CalculateBoolIndex(address);
				byte[] array = new byte[16];
				SetByteHead(array, 67, 65, cpuTo, cpuFrom);
				array[6] = addressDataType;
				BitConverter.GetBytes(value).CopyTo(array, 8);
				BitConverter.GetBytes(length).CopyTo(array, 12);
				return OperateResult.CreateSuccessResult(array);
			}
			byte[] array2 = new byte[14];
			SetByteHead(array2, 67, 73, cpuTo, cpuFrom);
			array2[6] = addressDataType;
			BitConverter.GetBytes(Convert.ToUInt32(address.Substring(1))).CopyTo(array2, 8);
			BitConverter.GetBytes(length).CopyTo(array2, 12);
			return OperateResult.CreateSuccessResult(array2);
		}
		if (!ushort.TryParse(address, out var result))
		{
			return new OperateResult<byte[]>("Address[" + address + "] wrong, not supported");
		}
		if (sfc == 1 || sfc == 2 || sfc == 3 || sfc == 4)
		{
			byte[] array3 = new byte[9];
			SetByteHead(array3, mfc, sfc, cpuTo, cpuFrom);
			array3[5] = BitConverter.GetBytes(result)[1];
			array3[6] = BitConverter.GetBytes(result)[0];
			array3[7] = BitConverter.GetBytes(length)[1];
			array3[8] = BitConverter.GetBytes(length)[0];
			return OperateResult.CreateSuccessResult(array3);
		}
		if (sfc == 9 || sfc == 10)
		{
			byte[] array4 = new byte[10];
			SetByteHead(array4, mfc, sfc, cpuTo, cpuFrom);
			array4[6] = BitConverter.GetBytes(result)[0];
			array4[7] = BitConverter.GetBytes(result)[1];
			array4[8] = BitConverter.GetBytes(length)[0];
			array4[9] = BitConverter.GetBytes(length)[1];
			return OperateResult.CreateSuccessResult(array4);
		}
		return new OperateResult<byte[]>($"SFC:{sfc} {StringResources.Language.NotSupportedFunction}");
	}

	internal static OperateResult<byte[]> BuildReadRandomCommand(byte mfc, byte sfc, byte cpuTo, byte cpuFrom, ushort[] address)
	{
		byte[] array = new byte[8 + address.Length * 2];
		SetByteHead(array, mfc, sfc, cpuTo, cpuFrom);
		array[6] = BitConverter.GetBytes(address.Length)[0];
		array[7] = BitConverter.GetBytes(address.Length)[1];
		for (int i = 0; i < address.Length; i++)
		{
			array[8 + i * 2] = BitConverter.GetBytes(address[i])[0];
			array[8 + i * 2 + 1] = BitConverter.GetBytes(address[i])[1];
		}
		return OperateResult.CreateSuccessResult(array);
	}

	internal static OperateResult<byte[]> BuildReadRandomCommand(byte cpuTo, byte cpuFrom, string[] address)
	{
		byte[] array = new byte[8 + address.Length * 6];
		SetByteHead(array, 67, 77, cpuTo, cpuFrom);
		array[6] = BitConverter.GetBytes(address.Length)[0];
		array[7] = BitConverter.GetBytes(address.Length)[1];
		for (int i = 0; i < address.Length; i++)
		{
			byte b = (array[8 + i * 6] = GetAddressDataType(address[i]));
			array[8 + i * 6 + 1] = 2;
			BitConverter.GetBytes(Convert.ToUInt32(address[i].Substring(1))).CopyTo(array, 8 + i * 6 + 2);
		}
		return OperateResult.CreateSuccessResult(array);
	}

	internal static OperateResult<byte[]> BuildWriteCommand(byte mfc, byte sfc, byte cpuTo, byte cpuFrom, ushort address, bool value)
	{
		byte[] array = new byte[9];
		SetByteHead(array, mfc, sfc, cpuTo, cpuFrom);
		array[5] = BitConverter.GetBytes(address)[1];
		array[6] = BitConverter.GetBytes(address)[0];
		array[7] = (byte)(value ? 255u : 0u);
		array[8] = 0;
		return OperateResult.CreateSuccessResult(array);
	}

	internal static OperateResult<byte[]> BuildWriteCommand(byte mfc, byte sfc, byte cpuTo, byte cpuFrom, string address, bool[] value)
	{
		if (address.StartsWith(new string[5] { "M", "G", "I", "O", "S" }))
		{
			byte addressDataType = GetAddressDataType(address);
			if (address[1] == 'B' || address[1] == 'b' || address.IndexOf('.') > 0)
			{
				int value2 = CalculateBoolIndex(address);
				byte[] array = new byte[16 + (value.Length + 7) / 8];
				SetByteHead(array, 67, 79, cpuTo, cpuFrom);
				array[6] = addressDataType;
				BitConverter.GetBytes(value2).CopyTo(array, 8);
				BitConverter.GetBytes(value.Length).CopyTo(array, 12);
				value.ToByteArray().CopyTo(array, 16);
				return OperateResult.CreateSuccessResult(array);
			}
			return new OperateResult<byte[]>(StringResources.Language.NotSupportedDataType);
		}
		if (!ushort.TryParse(address, out var result))
		{
			return new OperateResult<byte[]>("Address[" + address + "] wrong, not supported");
		}
		byte[] array2 = SoftBasic.BoolArrayToByte(value);
		byte[] array3 = new byte[9 + array2.Length];
		SetByteHead(array3, mfc, sfc, cpuTo, cpuFrom);
		array3[5] = BitConverter.GetBytes(result)[1];
		array3[6] = BitConverter.GetBytes(result)[0];
		array3[7] = BitConverter.GetBytes(value.Length)[1];
		array3[8] = BitConverter.GetBytes(value.Length)[0];
		array2.CopyTo(array3, 9);
		return OperateResult.CreateSuccessResult(array3);
	}

	internal static OperateResult<byte[]> BuildWriteCommand(byte mfc, byte sfc, byte cpuTo, byte cpuFrom, ushort address, short value)
	{
		byte[] array = new byte[9];
		SetByteHead(array, mfc, sfc, cpuTo, cpuFrom);
		array[5] = BitConverter.GetBytes(address)[1];
		array[6] = BitConverter.GetBytes(address)[0];
		array[7] = BitConverter.GetBytes(value)[1];
		array[8] = BitConverter.GetBytes(value)[0];
		return OperateResult.CreateSuccessResult(array);
	}

	internal static OperateResult<byte[]> BuildWriteCommand(byte mfc, byte sfc, byte cpuTo, byte cpuFrom, ushort address, ushort value)
	{
		byte[] array = new byte[9];
		SetByteHead(array, mfc, sfc, cpuTo, cpuFrom);
		array[5] = BitConverter.GetBytes(address)[1];
		array[6] = BitConverter.GetBytes(address)[0];
		array[7] = BitConverter.GetBytes(value)[1];
		array[8] = BitConverter.GetBytes(value)[0];
		return OperateResult.CreateSuccessResult(array);
	}

	internal static OperateResult<byte[]> BuildWriteCommand(byte mfc, byte sfc, byte cpuTo, byte cpuFrom, string address, byte[] value)
	{
		if (address.StartsWithAndNumber(new string[5] { "M", "G", "I", "O", "S" }))
		{
			byte addressDataType = GetAddressDataType(address);
			byte[] array = new byte[14 + value.Length];
			SetByteHead(array, 67, 75, cpuTo, cpuFrom);
			array[6] = addressDataType;
			BitConverter.GetBytes(Convert.ToUInt32(address.Substring(1))).CopyTo(array, 8);
			BitConverter.GetBytes(value.Length / 2).CopyTo(array, 12);
			SoftBasic.BytesReverseByWord(value).CopyTo(array, 14);
			return OperateResult.CreateSuccessResult(array);
		}
		if (!ushort.TryParse(address, out var result))
		{
			return new OperateResult<byte[]>("Address[" + address + "] wrong, not supported");
		}
		switch (sfc)
		{
		case 11:
		{
			byte[] array3 = new byte[10 + value.Length];
			SetByteHead(array3, mfc, sfc, cpuTo, cpuFrom);
			array3[6] = BitConverter.GetBytes(result)[0];
			array3[7] = BitConverter.GetBytes(result)[1];
			array3[8] = BitConverter.GetBytes(value.Length / 2)[0];
			array3[9] = BitConverter.GetBytes(value.Length / 2)[1];
			SoftBasic.BytesReverseByWord(value).CopyTo(array3, 10);
			return OperateResult.CreateSuccessResult(array3);
		}
		case 16:
		{
			byte[] array2 = new byte[9 + value.Length];
			SetByteHead(array2, mfc, sfc, cpuTo, cpuFrom);
			array2[5] = BitConverter.GetBytes(result)[1];
			array2[6] = BitConverter.GetBytes(result)[0];
			array2[7] = BitConverter.GetBytes(value.Length / 2)[1];
			array2[8] = BitConverter.GetBytes(value.Length / 2)[0];
			value.CopyTo(array2, 9);
			return OperateResult.CreateSuccessResult(array2);
		}
		default:
			return new OperateResult<byte[]>($"SFC:{sfc} {StringResources.Language.NotSupportedFunction}");
		}
	}

	internal static OperateResult<byte[]> BuildWriteRandomCommand(byte mfc, byte sfc, byte cpuTo, byte cpuFrom, ushort[] address, byte[] value)
	{
		if (value.Length != address.Length * 2)
		{
			return new OperateResult<byte[]>("value.Length must be twice as much as address.Length");
		}
		byte[] array = new byte[8 + address.Length * 4];
		SetByteHead(array, mfc, sfc, cpuTo, cpuFrom);
		array[6] = BitConverter.GetBytes(address.Length)[0];
		array[7] = BitConverter.GetBytes(address.Length)[1];
		for (int i = 0; i < address.Length; i++)
		{
			array[8 + i * 4] = BitConverter.GetBytes(address[i])[0];
			array[8 + i * 4 + 1] = BitConverter.GetBytes(address[i])[1];
			array[8 + i * 4 + 2] = value[i * 2 + 1];
			array[8 + i * 4 + 3] = value[i * 2];
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<bool[]> ReadBool(IMemobus memobus, string address, ushort length)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 1);
		OperateResult<byte[]> operateResult = BuildReadCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		if (operateResult2.Content[3] == 65)
		{
			return OperateResult.CreateSuccessResult(operateResult2.Content.RemoveBegin(8).ToBoolArray().SelectBegin(length));
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.RemoveBegin(5).ToBoolArray().SelectBegin(length));
	}

	public static OperateResult Write(IMemobus memobus, string address, bool value)
	{
		if (address.StartsWith(new string[5] { "M", "G", "I", "O", "S" }))
		{
			return Write(memobus, address, new bool[1] { value });
		}
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 5);
		OperateResult<byte[]> operateResult = BuildWriteCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, ushort.Parse(address), value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult Write(IMemobus memobus, string address, bool[] value)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 15);
		OperateResult<byte[]> operateResult = BuildWriteCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult();
	}

	private static OperateResult<byte[]> ExtraContent(string address, byte[] content)
	{
		if (content[2] == 32)
		{
			if (content[3] == 3 || content[3] == 4)
			{
				return OperateResult.CreateSuccessResult(content.RemoveBegin(5));
			}
			if (content[3] == 9 || content[3] == 10)
			{
				return OperateResult.CreateSuccessResult(SoftBasic.BytesReverseByWord(content.RemoveBegin(8)));
			}
			return OperateResult.CreateSuccessResult(content.RemoveBegin(5));
		}
		if (content[2] == 67)
		{
			if (content[3] == 73 || content[3] == 77)
			{
				return OperateResult.CreateSuccessResult(SoftBasic.BytesReverseByWord(content.RemoveBegin(10)));
			}
			return OperateResult.CreateSuccessResult(content.RemoveBegin(8));
		}
		return new OperateResult<byte[]>($"[{address}], mfc[{content[2]}] is not supported");
	}

	public static OperateResult<byte[]> Read(IMemobus memobus, string address, ushort length)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 3);
		OperateResult<byte[]> operateResult = BuildReadCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return ExtraContent(address, operateResult2.Content);
	}

	public static OperateResult Write(IMemobus memobus, string address, byte[] value)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte b = (byte)HslHelper.ExtractParameter(ref address, "x", 16);
		if (b == 3)
		{
			b = 16;
		}
		if (b == 9)
		{
			b = 11;
		}
		OperateResult<byte[]> operateResult = BuildWriteCommand(mfc, b, memobus.CpuTo, memobus.CpuFrom, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult Write(IMemobus memobus, string address, short value, Func<string, short, OperateResult> writeShort)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte b = (byte)HslHelper.ExtractParameter(ref address, "x", 6);
		if (b == 11 || b == 9)
		{
			return writeShort($"x={b};{address}", value);
		}
		if (b == 3)
		{
			b = 6;
		}
		OperateResult<byte[]> operateResult = BuildWriteCommand(mfc, b, memobus.CpuTo, memobus.CpuFrom, ushort.Parse(address), value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult Write(IMemobus memobus, string address, ushort value, Func<string, ushort, OperateResult> writeUShort)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte b = (byte)HslHelper.ExtractParameter(ref address, "x", 6);
		if (b == 11 || b == 9)
		{
			return writeUShort($"x={b};{address}", value);
		}
		if (b == 3)
		{
			b = 6;
		}
		OperateResult<byte[]> operateResult = BuildWriteCommand(mfc, b, memobus.CpuTo, memobus.CpuFrom, ushort.Parse(address), value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult<byte[]> ReadRandom(IMemobus memobus, ushort[] address)
	{
		OperateResult<byte[]> operateResult = BuildReadRandomCommand(32, 13, memobus.CpuTo, memobus.CpuFrom, address);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.BytesReverseByWord(operateResult2.Content.RemoveBegin(8)));
	}

	public static OperateResult<byte[]> ReadRandom(IMemobus memobus, string[] address)
	{
		OperateResult<byte[]> operateResult = BuildReadRandomCommand(memobus.CpuTo, memobus.CpuFrom, address);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.BytesReverseByWord(operateResult2.Content.RemoveBegin(8)));
	}

	public static OperateResult WriteRandom(IMemobus memobus, ushort[] address, byte[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteRandomCommand(32, 14, memobus.CpuTo, memobus.CpuFrom, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = memobus.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IMemobus memobus, string address, ushort length)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 1);
		OperateResult<byte[]> command = BuildReadCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, address, length);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		if (read.Content[3] == 65)
		{
			return OperateResult.CreateSuccessResult(read.Content.RemoveBegin(8).ToBoolArray().SelectBegin(length));
		}
		return OperateResult.CreateSuccessResult(read.Content.RemoveBegin(5).ToBoolArray().SelectBegin(length));
	}

	public static async Task<OperateResult> WriteAsync(IMemobus memobus, string address, bool value)
	{
		if (address.StartsWith(new string[5] { "M", "G", "I", "O", "S" }))
		{
			return await WriteAsync(memobus, address, new bool[1] { value }).ConfigureAwait(continueOnCapturedContext: false);
		}
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 5);
		OperateResult<byte[]> command = BuildWriteCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, ushort.Parse(address), value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult> WriteAsync(IMemobus memobus, string address, bool[] value)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 15);
		OperateResult<byte[]> command = BuildWriteCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IMemobus memobus, string address, ushort length)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 3);
		OperateResult<byte[]> command = BuildReadCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, address, length);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return ExtraContent(address, read.Content);
	}

	public static async Task<OperateResult> WriteAsync(IMemobus memobus, string address, byte[] value)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 16);
		if (sfc == 3)
		{
			sfc = 16;
		}
		if (sfc == 9)
		{
			sfc = 11;
		}
		OperateResult<byte[]> command = BuildWriteCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult> WriteAsync(IMemobus memobus, string address, short value, Func<string, short, Task<OperateResult>> writeShort)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 6);
		if (sfc == 11 || sfc == 9)
		{
			return await writeShort($"x={sfc};{address}", value);
		}
		if (sfc == 3)
		{
			sfc = 6;
		}
		OperateResult<byte[]> command = BuildWriteCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, ushort.Parse(address), value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult> WriteAsync(IMemobus memobus, string address, ushort value, Func<string, ushort, Task<OperateResult>> writeUShort)
	{
		byte mfc = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
		byte sfc = (byte)HslHelper.ExtractParameter(ref address, "x", 6);
		if (sfc == 11 || sfc == 9)
		{
			return await writeUShort($"x={sfc};{address}", value);
		}
		if (sfc == 3)
		{
			sfc = 6;
		}
		OperateResult<byte[]> command = BuildWriteCommand(mfc, sfc, memobus.CpuTo, memobus.CpuFrom, ushort.Parse(address), value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<byte[]>> ReadRandomAsync(IMemobus memobus, ushort[] address)
	{
		OperateResult<byte[]> command = BuildReadRandomCommand(32, 13, memobus.CpuTo, memobus.CpuFrom, address);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.BytesReverseByWord(read.Content.RemoveBegin(8)));
	}

	public static async Task<OperateResult<byte[]>> ReadRandomAsync(IMemobus memobus, string[] address)
	{
		OperateResult<byte[]> command = BuildReadRandomCommand(memobus.CpuTo, memobus.CpuFrom, address);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.BytesReverseByWord(read.Content.RemoveBegin(8)));
	}

	public static async Task<OperateResult> WriteRandomAsync(IMemobus memobus, ushort[] address, byte[] value)
	{
		OperateResult<byte[]> command = BuildWriteRandomCommand(32, 14, memobus.CpuTo, memobus.CpuFrom, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await memobus.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult();
	}
}
