using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core;

public class HslHelper
{
	public static Random HslRandom { get; private set; } = new Random();

	public static int LockLimit { get; set; } = 1000;

	public static bool UseAsyncLock { get; set; } = true;

	public static int ExtractParameter(ref string address, string paraName, int defaultValue)
	{
		OperateResult<int> operateResult = ExtractParameter(ref address, paraName);
		return operateResult.IsSuccess ? operateResult.Content : defaultValue;
	}

	public static bool ExtractBooleanParameter(ref string address, string paraName, bool defaultValue)
	{
		OperateResult<bool> operateResult = ExtractBooleanParameter(ref address, paraName);
		return operateResult.IsSuccess ? operateResult.Content : defaultValue;
	}

	public static OperateResult<int> ExtractParameter(ref string address, string paraName)
	{
		try
		{
			Match match = Regex.Match(address, paraName + "=[0-9A-Fa-fxX]+;", RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				return new OperateResult<int>("Address [" + address + "] can't find [" + paraName + "] Parameters. for example : " + paraName + "=1;100");
			}
			string text = match.Value.Substring(paraName.Length + 1, match.Value.Length - paraName.Length - 2);
			int value = ((text.StartsWith("0x") || text.StartsWith("0X")) ? Convert.ToInt32(text.Substring(2), 16) : (text.StartsWith("0") ? Convert.ToInt32(text, 8) : Convert.ToInt32(text)));
			address = address.Replace(match.Value, "");
			return OperateResult.CreateSuccessResult(value);
		}
		catch (Exception ex)
		{
			return new OperateResult<int>("Address [" + address + "] Get [" + paraName + "] Parameters failed: " + ex.Message);
		}
	}

	public static OperateResult<bool> ExtractBooleanParameter(ref string address, string paraName)
	{
		try
		{
			Match match = Regex.Match(address, paraName + "=[0-1A-Za-z]+;");
			if (!match.Success)
			{
				return new OperateResult<bool>("Address [" + address + "] can't find [" + paraName + "] Parameters. for example : " + paraName + "=True;100");
			}
			string text = match.Value.Substring(paraName.Length + 1, match.Value.Length - paraName.Length - 2);
			bool flag = false;
			flag = ((!Regex.IsMatch(text, "^[0-1]+$")) ? Convert.ToBoolean(text) : (Convert.ToInt32(text) != 0));
			address = address.Replace(match.Value, "");
			return OperateResult.CreateSuccessResult(flag);
		}
		catch (Exception ex)
		{
			return new OperateResult<bool>("Address [" + address + "] Get [" + paraName + "] Parameters failed: " + ex.Message);
		}
	}

	public static int ExtractStartIndex(ref string address)
	{
		try
		{
			Match match = Regex.Match(address, "\\[[0-9]+\\]$");
			if (!match.Success)
			{
				return -1;
			}
			string value = match.Value.Substring(1, match.Value.Length - 2);
			int result = Convert.ToInt32(value);
			address = address.Remove(address.Length - match.Value.Length);
			return result;
		}
		catch
		{
			return -1;
		}
	}

	public static IByteTransform ExtractTransformParameter(ref string address, IByteTransform defaultTransform)
	{
		try
		{
			string text = "format";
			Match match = Regex.Match(address, text + "=(ABCD|BADC|DCBA|CDAB);", RegexOptions.IgnoreCase);
			if (!match.Success)
			{
				return defaultTransform;
			}
			string text2 = match.Value.Substring(text.Length + 1, match.Value.Length - text.Length - 2);
			DataFormat dataFormat = defaultTransform.DataFormat;
			switch (text2.ToUpper())
			{
			case "ABCD":
				dataFormat = DataFormat.ABCD;
				break;
			case "BADC":
				dataFormat = DataFormat.BADC;
				break;
			case "DCBA":
				dataFormat = DataFormat.DCBA;
				break;
			case "CDAB":
				dataFormat = DataFormat.CDAB;
				break;
			}
			address = address.Replace(match.Value, "");
			if (dataFormat != defaultTransform.DataFormat)
			{
				return defaultTransform.CreateByDateFormat(dataFormat);
			}
			return defaultTransform;
		}
		catch
		{
			throw;
		}
	}

	public static OperateResult<int[], int[]> SplitReadLength(int address, int length, int segment)
	{
		int[] array = SoftBasic.SplitIntegerToArray(length, segment);
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			if (i == 0)
			{
				array2[i] = address;
			}
			else
			{
				array2[i] = array2[i - 1] + array[i - 1];
			}
		}
		return OperateResult.CreateSuccessResult(array2, array);
	}

	public static OperateResult<int[], List<T[]>> SplitWriteData<T>(int address, T[] value, ushort segment, int addressLength)
	{
		List<T[]> list = SoftBasic.ArraySplitByLength(value, segment * addressLength);
		int[] array = new int[list.Count];
		for (int i = 0; i < array.Length; i++)
		{
			if (i == 0)
			{
				array[i] = address;
			}
			else
			{
				array[i] = array[i - 1] + list[i - 1].Length / addressLength;
			}
		}
		return OperateResult.CreateSuccessResult(array, list);
	}

	public static int GetBitIndexInformation(ref string address)
	{
		int result = 0;
		int num = address.LastIndexOf('.');
		if (num > 0 && num < address.Length - 1)
		{
			string text = address.Substring(num + 1);
			result = ((!text.Contains(new string[6] { "A", "B", "C", "D", "E", "F" })) ? Convert.ToInt32(text) : Convert.ToInt32(text, 16));
			address = address.Substring(0, num);
		}
		return result;
	}

	public static string GetIpAddressFromInput(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (!value.EndsWith(new string[6] { ".com", ".cn", ".net", ".top", ".vip", ".club" }) && IPAddress.TryParse(value, out var _))
			{
				return value;
			}
			IPHostEntry hostEntry = Dns.GetHostEntry(value);
			IPAddress[] addressList = hostEntry.AddressList;
			if (addressList.Length != 0)
			{
				return addressList[0].ToString();
			}
		}
		return "127.0.0.1";
	}

	public static byte[] ReadSpecifiedLengthFromStream(Stream stream, int length)
	{
		byte[] array = new byte[length];
		int num = 0;
		while (num < length)
		{
			int num2 = stream.Read(array, num, array.Length - num);
			num += num2;
			if (num2 == 0)
			{
				break;
			}
		}
		return array;
	}

	public static void WriteStringToStream(Stream stream, string value)
	{
		byte[] value2 = (string.IsNullOrEmpty(value) ? new byte[0] : Encoding.UTF8.GetBytes(value));
		WriteBinaryToStream(stream, value2);
	}

	public static string ReadStringFromStream(Stream stream)
	{
		byte[] bytes = ReadBinaryFromStream(stream);
		return Encoding.UTF8.GetString(bytes);
	}

	public static void WriteBinaryToStream(Stream stream, byte[] value)
	{
		stream.Write(BitConverter.GetBytes(value.Length), 0, 4);
		stream.Write(value, 0, value.Length);
	}

	public static byte[] ReadBinaryFromStream(Stream stream)
	{
		byte[] value = ReadSpecifiedLengthFromStream(stream, 4);
		int num = BitConverter.ToInt32(value, 0);
		if (num <= 0)
		{
			return new byte[0];
		}
		return ReadSpecifiedLengthFromStream(stream, num);
	}

	public static byte[] GetUTF8Bytes(string message)
	{
		return string.IsNullOrEmpty(message) ? new byte[0] : Encoding.UTF8.GetBytes(message);
	}

	public static void ThreadSleep(int millisecondsTimeout)
	{
		try
		{
			Thread.Sleep(millisecondsTimeout);
		}
		catch
		{
		}
	}

	public static string PathCombine(params string[] paths)
	{
		return Path.Combine(paths);
	}

	public static OperateResult<T> ByteArrayToStruct<T>(byte[] content) where T : struct
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<T>(StringResources.Language.InsufficientPrivileges);
		}
		int num = Marshal.SizeOf(typeof(T));
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		try
		{
			Marshal.Copy(content, 0, intPtr, num);
			T value = Marshal.PtrToStructure<T>(intPtr);
			Marshal.FreeHGlobal(intPtr);
			return OperateResult.CreateSuccessResult(value);
		}
		catch (Exception ex)
		{
			Marshal.FreeHGlobal(intPtr);
			return new OperateResult<T>(ex.Message);
		}
	}

	public static void CalculateStartBitIndexAndLength(int addressStart, ushort length, out int newStart, out ushort byteLength, out int offset)
	{
		byteLength = (ushort)((addressStart + length - 1) / 8 - addressStart / 8 + 1);
		offset = addressStart % 8;
		newStart = addressStart - offset;
	}

	public static int CalculateBitStartIndex(string bit)
	{
		if (Regex.IsMatch(bit, "[ABCDEF]", RegexOptions.IgnoreCase))
		{
			return Convert.ToInt32(bit, 16);
		}
		return Convert.ToInt32(bit);
	}

	public static T[,] CreateTwoArrayFromOneArray<T>(T[] array, int row, int col)
	{
		T[,] array2 = new T[row, col];
		int num = 0;
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				array2[i, j] = array[num];
				num++;
			}
		}
		return array2;
	}

	public static bool IsAddressEndWithIndex(string address)
	{
		return Regex.IsMatch(address, "\\[[0-9]+\\]$");
	}

	public static int CalculateOccupyLength(int address, int length, int hex = 8)
	{
		return (address + length - 1) / hex - address / hex + 1;
	}

	public static OperateResult<T[]> ReadCuttingHelper<T>(Func<string, ushort, OperateResult<T[]>> readFunc, List<CuttingAddress> cuttings, string address, ushort length)
	{
		string text = string.Empty;
		OperateResult<int> operateResult = ExtractParameter(ref address, "s");
		if (operateResult.IsSuccess)
		{
			text = $"s={operateResult.Content};";
		}
		foreach (CuttingAddress cutting in cuttings)
		{
			if (address.StartsWith(cutting.DataType, StringComparison.OrdinalIgnoreCase))
			{
				int num = 0;
				try
				{
					num = Convert.ToInt32(address.Substring(cutting.DataType.Length), cutting.FromBase);
				}
				catch
				{
					break;
				}
				if (num < cutting.Address && num + length > cutting.Address)
				{
					ushort num2 = (ushort)(cutting.Address - num);
					ushort arg = (ushort)(length - num2);
					OperateResult<T[]> operateResult2 = readFunc(text + address, num2);
					if (!operateResult2.IsSuccess)
					{
						return operateResult2;
					}
					OperateResult<T[]> operateResult3 = readFunc(text + cutting.DataType + Convert.ToString(cutting.Address, cutting.FromBase), arg);
					if (!operateResult3.IsSuccess)
					{
						return operateResult3;
					}
					return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<T>(operateResult2.Content, operateResult3.Content));
				}
				break;
			}
		}
		return readFunc(address, length);
	}

	public static async Task<OperateResult<T[]>> ReadCuttingAsyncHelper<T>(Func<string, ushort, Task<OperateResult<T[]>>> readFunc, List<CuttingAddress> cuttings, string address, ushort length)
	{
		string station = string.Empty;
		OperateResult<int> stationPara = ExtractParameter(ref address, "s");
		if (stationPara.IsSuccess)
		{
			station = $"s={stationPara.Content};";
		}
		foreach (CuttingAddress item in cuttings)
		{
			if (address.StartsWith(item.DataType, StringComparison.OrdinalIgnoreCase))
			{
				int add;
				try
				{
					add = Convert.ToInt32(address.Substring(item.DataType.Length), item.FromBase);
				}
				catch
				{
					break;
				}
				if (add < item.Address && add + length > item.Address)
				{
					ushort len1 = (ushort)(item.Address - add);
					ushort len2 = (ushort)(length - len1);
					OperateResult<T[]> read1 = await readFunc(station + address, len1);
					if (!read1.IsSuccess)
					{
						return read1;
					}
					OperateResult<T[]> read2 = await readFunc(station + item.DataType + Convert.ToString(item.Address, item.FromBase), len2);
					if (!read2.IsSuccess)
					{
						return read2;
					}
					return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<T>(read1.Content, read2.Content));
				}
				break;
			}
		}
		return await readFunc(address, length);
	}

	private static string[] SplitsAddressDot(string address)
	{
		int num = address.LastIndexOf(".");
		if (num > 0 && num < address.Length - 1)
		{
			return new string[2]
			{
				address.Substring(0, num),
				address.Substring(num + 1)
			};
		}
		return new string[1] { address };
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteNet device, string address, ushort length, int addressLength = 16, bool reverseByWord = false)
	{
		if (address.IndexOf('.') > 0)
		{
			string[] array = SplitsAddressDot(address);
			int num = 0;
			try
			{
				num = CalculateBitStartIndex(array[1]);
			}
			catch (Exception ex)
			{
				return new OperateResult<bool[]>("Bit Index format wrong, " + ex.Message);
			}
			ushort length2 = (ushort)((length + num + addressLength - 1) / addressLength);
			OperateResult<byte[]> operateResult = device.Read(array[0], length2);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult);
			}
			if (reverseByWord)
			{
				return OperateResult.CreateSuccessResult(operateResult.Content.ReverseByWord().ToBoolArray().SelectMiddle(num, length));
			}
			return OperateResult.CreateSuccessResult(operateResult.Content.ToBoolArray().SelectMiddle(num, length));
		}
		return device.ReadBool(address, length);
	}

	public static OperateResult WriteBool(IReadWriteNet device, string address, bool[] value, int addressLength = 16, bool reverseByWord = false, bool insertPoint = false)
	{
		if (address.IndexOf('.') > 0)
		{
			string[] array = SplitsAddressDot(address);
			int num = 0;
			try
			{
				num = CalculateBitStartIndex(array[1]);
			}
			catch (Exception ex)
			{
				return new OperateResult<bool[]>("Bit Index format wrong, " + ex.Message);
			}
			ushort length = (ushort)((value.Length + num + addressLength - 1) / addressLength);
			OperateResult<byte[]> operateResult = device.Read(array[0], length);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult);
			}
			bool[] array2 = (reverseByWord ? operateResult.Content.ReverseByWord().ToBoolArray() : operateResult.Content.ToBoolArray());
			value.CopyTo(array2, num);
			byte[] value2 = (reverseByWord ? array2.ToByteArray().ReverseByWord() : array2.ToByteArray());
			return device.Write(array[0], value2);
		}
		if (insertPoint && address.Length > 1)
		{
			string address2 = AddressAddPoint(address);
			return WriteBool(device, address2, value, addressLength, reverseByWord, insertPoint);
		}
		return device.Write(address, value);
	}

	private static string AddressAddPoint(string address)
	{
		return address.Substring(0, address.Length - 1) + "." + address[address.Length - 1];
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteNet device, string address, ushort length, int addressLength = 16, bool reverseByWord = false)
	{
		if (address.IndexOf('.') > 0)
		{
			string[] addressSplits = SplitsAddressDot(address);
			int bitIndex;
			try
			{
				bitIndex = CalculateBitStartIndex(addressSplits[1]);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception ex3 = ex2;
				return new OperateResult<bool[]>("Bit Index format wrong, " + ex3.Message);
			}
			OperateResult<byte[]> read = await device.ReadAsync(length: (ushort)((length + bitIndex + addressLength - 1) / addressLength), address: addressSplits[0]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			if (reverseByWord)
			{
				return OperateResult.CreateSuccessResult(read.Content.ReverseByWord().ToBoolArray().SelectMiddle(bitIndex, length));
			}
			return OperateResult.CreateSuccessResult(read.Content.ToBoolArray().SelectMiddle(bitIndex, length));
		}
		return await device.ReadBoolAsync(address, length);
	}

	public static async Task<OperateResult> WriteBoolAsync(IReadWriteNet device, string address, bool[] value, int addressLength = 16, bool reverseByWord = false, bool insertPoint = false)
	{
		if (address.IndexOf('.') > 0)
		{
			string[] addressSplits = SplitsAddressDot(address);
			int bitIndex;
			try
			{
				bitIndex = CalculateBitStartIndex(addressSplits[1]);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception ex3 = ex2;
				return new OperateResult<bool[]>("Bit Index format wrong, " + ex3.Message);
			}
			OperateResult<byte[]> read = await device.ReadAsync(length: (ushort)((value.Length + bitIndex + addressLength - 1) / addressLength), address: addressSplits[0]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			bool[] boolArray = (reverseByWord ? read.Content.ReverseByWord().ToBoolArray() : read.Content.ToBoolArray());
			value.CopyTo(boolArray, bitIndex);
			return await device.WriteAsync(value: reverseByWord ? boolArray.ToByteArray().ReverseByWord() : boolArray.ToByteArray(), address: addressSplits[0]);
		}
		if (insertPoint && address.Length > 1)
		{
			string addressNew = AddressAddPoint(address);
			return await WriteBoolAsync(device, addressNew, value, addressLength, reverseByWord, insertPoint);
		}
		return device.Write(address, value);
	}

	public static string ToFormatString(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(portName);
		stringBuilder.Append("-");
		stringBuilder.Append(baudRate.ToString());
		stringBuilder.Append("-");
		stringBuilder.Append(dataBits.ToString());
		stringBuilder.Append("-");
		switch (parity)
		{
		case Parity.None:
			stringBuilder.Append("N");
			break;
		case Parity.Even:
			stringBuilder.Append("E");
			break;
		case Parity.Odd:
			stringBuilder.Append("O");
			break;
		case Parity.Space:
			stringBuilder.Append("S");
			break;
		default:
			stringBuilder.Append("M");
			break;
		}
		stringBuilder.Append("-");
		switch (stopBits)
		{
		case StopBits.None:
			stringBuilder.Append("0");
			break;
		case StopBits.One:
			stringBuilder.Append("1");
			break;
		case StopBits.Two:
			stringBuilder.Append("2");
			break;
		default:
			stringBuilder.Append("1.5");
			break;
		}
		return stringBuilder.ToString();
	}
}
