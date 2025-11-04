using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Profinet.LSIS.Helper;

public class LSCpuHelper
{
	private const string CpuTypes = "PMLKFTCDSQINUZR";

	public static string GetErrorText(int err)
	{
		if (1 == 0)
		{
		}
		string result = err switch
		{
			3 => StringResources.Language.LsisCnet0003, 
			4 => StringResources.Language.LsisCnet0004, 
			7 => StringResources.Language.LsisCnet0007, 
			17 => StringResources.Language.LsisCnet0011, 
			144 => StringResources.Language.LsisCnet0090, 
			400 => StringResources.Language.LsisCnet0190, 
			656 => StringResources.Language.LsisCnet0290, 
			4402 => StringResources.Language.LsisCnet1132, 
			4658 => StringResources.Language.LsisCnet1232, 
			4660 => StringResources.Language.LsisCnet1234, 
			4914 => StringResources.Language.LsisCnet1332, 
			5170 => StringResources.Language.LsisCnet1432, 
			28978 => StringResources.Language.LsisCnet7132, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		try
		{
			if (response[0] == 6)
			{
				if (response[1] == 110 || response[1] == 111 || response[1] == 119)
				{
					return OperateResult.CreateSuccessResult(response);
				}
				string text = Encoding.ASCII.GetString(response);
				string text2 = text.Substring(1, text.Length - 2);
				text2 = text2.Substring(1, text2.Length - 3);
				return OperateResult.CreateSuccessResult(GetBytesFromHex(text2));
			}
			if (response[0] == 21)
			{
				int err = Convert.ToInt32(Encoding.ASCII.GetString(response, 6, 4), 16);
				return new OperateResult<byte[]>(err, GetErrorText(err));
			}
			return new OperateResult<byte[]>(response[0], "Source: " + SoftBasic.GetAsciiStringRender(response));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(1, "Wrong:" + ex.Message + Environment.NewLine + "Source: " + response.ToHexString());
		}
	}

	public static byte[] GetBytesFromHex(string IP)
	{
		byte[] array = new byte[IP.Length / 2];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Convert.ToByte(IP.Substring(i * 2, 2), 16);
		}
		return array;
	}

	public static OperateResult<string> AnalysisAddress(string address, bool transBit = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			if (!"PMLKFTCDSQINUZR".Contains(address[0].ToString()))
			{
				return new OperateResult<string>(StringResources.Language.NotSupportedDataType);
			}
			stringBuilder.Append(address[0]);
			if (address[0] == 'M')
			{
				stringBuilder.Append("X");
				if (transBit & (address.IndexOf('.') > 0))
				{
					int bitIndexInformation = HslHelper.GetBitIndexInformation(ref address);
					stringBuilder.Append(address.Substring(2));
					stringBuilder.Append(bitIndexInformation.ToString("X1"));
				}
				else
				{
					stringBuilder.Append(address.Substring(2, address.Length - 2));
				}
			}
			else
			{
				stringBuilder.Append("W");
				stringBuilder.Append(Convert.ToInt32(address.Substring(2, address.Length - 2)));
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(ex.Message);
		}
		return OperateResult.CreateSuccessResult(stringBuilder.ToString());
	}

	private static void AddBccTail(List<byte> command)
	{
		int num = 0;
		for (int i = 0; i < command.Count; i++)
		{
			num += command[i];
		}
		command.AddRange(SoftBasic.BuildAsciiBytesFrom((byte)num));
	}

	public static OperateResult<byte[]> BuildReadByteCommand(byte station, string address, ushort length)
	{
		List<byte> list = new List<byte>();
		byte b = 0;
		list.Clear();
		int memoryType = GetMemoryType(address);
		int dataType = GetDataType(address);
		int staraddress = HexToOct(address.Substring(1, address.Length - 1));
		list.Add(2);
		list.AddRange(Encoding.ASCII.GetBytes(sprintf("r%C", select_data_code(memoryType))));
		list.AddRange(Encoding.ASCII.GetBytes($"{(byte)0:X4}"));
		list.AddRange(Encoding.ASCII.GetBytes($"{(byte)0:X2}"));
		list.AddRange(Encoding.ASCII.GetBytes($"{(byte)GetDataSize2(staraddress, length, dataType):X2}"));
		for (int i = 1; i <= 10; i++)
		{
			b += list[i];
		}
		list.AddRange(Encoding.ASCII.GetBytes($"{b:X2}"));
		list.Add(3);
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult<byte[]> BuildReadCommand(byte station, string address, ushort length)
	{
		return BuildReadByteCommand(station, address, length);
	}

	public static OperateResult<byte[]> BuildWriteByteCommand(byte station, string address, byte[] value)
	{
		OperateResult<string> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		string text = new string(new char[60]);
		ushort num = 0;
		float[] array = new float[3];
		char[] array2 = new char[4];
		short num2 = BitConverter.ToInt16(value, 0);
		int dataType = GetDataType(operateResult.Content);
		int memoryType = GetMemoryType(operateResult.Content);
		int num3 = int.Parse(operateResult.Content.Substring(2, operateResult.Content.Length - 2));
		if (memoryType == 128)
		{
			Console.WriteLine("Memory Type Input Error. Memory Type = P, M, K, T, C, U, Z, S, L, N, D, R, ZR", num3);
		}
		else
		{
			list.Clear();
			list.Add(2);
			list.AddRange(Encoding.ASCII.GetBytes(sprintf("w%C", select_data_code(memoryType))));
			if (text != null)
			{
				list.AddRange(Encoding.ASCII.GetBytes($"{GetDataSize((ushort)Convert.ToInt32(num3), 2):X2}{(byte)0:X2}"));
			}
			list.AddRange(Encoding.ASCII.GetBytes($"{(byte)0:X2}"));
			list.AddRange(Encoding.ASCII.GetBytes($"{(byte)GetDataSize2(num3, 1, dataType):X2}"));
			int num4;
			switch (dataType)
			{
			case 1:
				list.AddRange(Encoding.ASCII.GetBytes($"{(byte)num2:X2}"));
				num4 = 13;
				break;
			case 3:
			{
				short num5 = (short)((ulong)num2 >> 16);
				list.AddRange(Encoding.ASCII.GetBytes($"{(uint)num2:X2}{(byte)num2:X2}"));
				list.AddRange(Encoding.ASCII.GetBytes($"{(uint)num5:X2}{(ulong)num2 >> 24:X2}"));
				num4 = 18;
				break;
			}
			case 4:
				array[0] = num2;
				Array.Copy(array2, array, 2);
				list.AddRange(Encoding.ASCII.GetBytes($"{(uint)array2[0]:X2}{(uint)array2[1]:X2}"));
				list.AddRange(Encoding.ASCII.GetBytes($"{(uint)array2[2]:X2}{(uint)array2[3]:X2}"));
				num4 = 18;
				break;
			case 5:
				array[0] = num2;
				Array.Copy(array2, array, 2);
				list.AddRange(Encoding.ASCII.GetBytes($"{(uint)array2[3]:X2}{(uint)array2[2]:X2}"));
				list.AddRange(Encoding.ASCII.GetBytes($"{(uint)array2[1]:X2}{(uint)array2[0]:X2}"));
				num4 = 18;
				break;
			default:
				list.AddRange(Encoding.ASCII.GetBytes($"{(sbyte)num2:X2}{num2 >> 8:X2}"));
				num4 = 14;
				break;
			}
			for (int i = 1; i <= num4; i++)
			{
				num += list[i];
			}
			list.AddRange(Encoding.ASCII.GetBytes($"{(byte)num:X2}"));
			list.Add(3);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult<byte[]> BuildWriteOneCommand(byte station, string address, byte[] value)
	{
		OperateResult<string> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		string text = new string(new char[31]);
		ushort num = 0;
		int memoryType = GetMemoryType(operateResult.Content);
		int num2 = HexToOct(operateResult.Content.Substring(2, operateResult.Content.Length - 2));
		string text2 = value[0].ToString("X2");
		if (memoryType == 128)
		{
			Console.WriteLine("Memory Type Input Error. Memory Type = P, M, K, T, C, U, Z, S, L, N, D, R, ZR", address);
		}
		else
		{
			list.Clear();
			list.Add(2);
			if (text2 == "01")
			{
				list.AddRange(Encoding.ASCII.GetBytes(sprintf("o%C", select_data_code(memoryType))));
			}
			else
			{
				list.AddRange(Encoding.ASCII.GetBytes(sprintf("n%C", select_data_code(memoryType))));
			}
			if (memoryType != 1)
			{
				text = $"{num2 >> 4:X4}";
				list.AddRange(Encoding.ASCII.GetBytes($"{GetDataSize((ushort)Convert.ToInt32(text), 2):X2}{(byte)0:X2}"));
			}
			else
			{
				list.AddRange(Encoding.ASCII.GetBytes($"{(uint)(sbyte)((ushort)num2 / 16):X2}{(ushort)((ushort)num2 / 16) >> 16:X2}"));
			}
			list.AddRange(Encoding.ASCII.GetBytes($"{(byte)0:X2}"));
			short num3 = ((!(text2 != "00")) ? ((short)(-1.0 - Math.Pow(2.0, num2 % 16))) : ((short)Math.Pow(2.0, num2 % 16)));
			list.AddRange(Encoding.ASCII.GetBytes($"{(sbyte)num3:X2}{HIBYTE(num3):X2}"));
			for (int i = 1; i <= 12; i++)
			{
				num += list[i];
			}
			list.AddRange(Encoding.ASCII.GetBytes($"{(byte)num:X2}"));
			list.Add(3);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult<byte[]> BuildWriteCommand(byte station, string address, byte[] value)
	{
		OperateResult<string> dataTypeToAddress = LSFastEnet.GetDataTypeToAddress(address);
		if (!dataTypeToAddress.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(dataTypeToAddress);
		}
		switch (dataTypeToAddress.Content)
		{
		case "Bit":
			return BuildWriteOneCommand(station, address, value);
		case "Word":
		case "DWord":
		case "LWord":
		case "Continuous":
			return BuildWriteByteCommand(station, address, value);
		default:
			return new OperateResult<byte[]>(StringResources.Language.NotSupportedDataType);
		}
	}

	public static OperateResult<byte[]> Read(IReadWriteDevice plc, int station, string address, ushort length)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = BuildReadCommand(station2, address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return plc.ReadFromCoreServer(operateResult.Content);
	}

	public static OperateResult Write(IReadWriteDevice plc, int station, string address, byte[] value)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = BuildWriteCommand(station2, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return plc.ReadFromCoreServer(operateResult.Content);
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteDevice plc, int station, string address, ushort length)
	{
		int bitIndexInformation = HslHelper.GetBitIndexInformation(ref address);
		int num = HslHelper.CalculateOccupyLength(bitIndexInformation, length);
		OperateResult<byte[]> operateResult = Read(plc, station, address, (ushort)num);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.ToBoolArray().SelectMiddle(bitIndexInformation, length));
	}

	public static OperateResult Write(IReadWriteDevice plc, int station, string address, bool value)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<string> operateResult = AnalysisAddress(address, transBit: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = BuildWriteOneCommand(station2, operateResult.Content, new byte[1] { (byte)(value ? 1u : 0u) });
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return plc.ReadFromCoreServer(operateResult2.Content);
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteDevice plc, int station, string address, ushort length)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> command = BuildReadCommand(stat, address, length);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await plc.ReadFromCoreServerAsync(command.Content);
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, int station, string address, byte[] value)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> command = BuildWriteCommand(stat, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await plc.ReadFromCoreServerAsync(command.Content);
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteDevice plc, int station, string address, ushort length)
	{
		int bitIndex = HslHelper.GetBitIndexInformation(ref address);
		ushort length2 = (ushort)HslHelper.CalculateOccupyLength(bitIndex, length);
		OperateResult<byte[]> read = await ReadAsync(plc, station, address, length2);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.ToBoolArray().SelectMiddle(bitIndex, length));
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, int station, string address, bool value)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<string> analysis = AnalysisAddress(address, transBit: true);
		if (!analysis.IsSuccess)
		{
			return analysis;
		}
		OperateResult<byte[]> command = BuildWriteOneCommand(stat, analysis.Content.Substring(1), new byte[1] { (byte)(value ? 1u : 0u) });
		if (!command.IsSuccess)
		{
			return command;
		}
		return await plc.ReadFromCoreServerAsync(command.Content);
	}

	private static int GetMemoryType(string address)
	{
		char c = address[0];
		if (1 == 0)
		{
		}
		int result = c switch
		{
			'P' => 0, 
			'M' => 1, 
			'K' => 2, 
			'F' => 3, 
			'T' => 4, 
			'C' => 5, 
			'U' => 6, 
			'Z' => 7, 
			'S' => 8, 
			'L' => 9, 
			'N' => 10, 
			'D' => 11, 
			'R' => 12, 
			_ => 128, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static int GetDataType(string address)
	{
		char c = address[1];
		if (1 == 0)
		{
		}
		int result = c switch
		{
			'X' => 1, 
			'W' => 2, 
			'D' => 4, 
			'F' => 8, 
			_ => 1, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static string sprintf(string input, params object[] inpVars)
	{
		int i = -1;
		input = Regex.Replace(input, "%.", delegate
		{
			int num = ++i;
			return "{" + num + "}";
		});
		return string.Format(input, inpVars);
	}

	private static char select_data_code(int MemoryType)
	{
		if (1 == 0)
		{
		}
		char result = MemoryType switch
		{
			0 => 'h', 
			2 => 'k', 
			3 => 'n', 
			4 => 'd', 
			5 => 'm', 
			6 => 'q', 
			7 => 'z', 
			8 => 'o', 
			9 => 'j', 
			10 => 'p', 
			11 => 'a', 
			12 => 'r', 
			13 => '{', 
			_ => 'i', 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static int GetDataSize(int address, int datatype)
	{
		if (datatype == 2)
		{
			return 2 * address;
		}
		if (datatype > 2 && datatype <= 5)
		{
			return 4 * address;
		}
		return 10 * address / 8;
	}

	private static int GetDataSize2(int Staraddress, int DataSize, int datatype)
	{
		switch (datatype)
		{
		case 2:
			return 2 * DataSize;
		default:
			if (datatype != 5)
			{
				int num = 10 * DataSize / 8;
				int num2 = (8 - 10 * Staraddress % 8) % 8;
				if (num2 + 8 * num < 10 * DataSize)
				{
					num++;
				}
				if (num2 != 0)
				{
					num++;
				}
				return num;
			}
			break;
		case 3:
		case 4:
			break;
		}
		return 4 * DataSize;
	}

	private static byte? HIBYTE(short n)
	{
		byte[] array = new byte[4]
		{
			(byte)((n >> 24) & 0xFF),
			(byte)((n >> 16) & 0xFF),
			(byte)((n >> 8) & 0xFF),
			(byte)(n & 0xFF)
		};
		if ((byte)((n >> 8) & 0xFF) != byte.MaxValue && (byte)((n >> 8) & 0xFF) != 0)
		{
			return (byte)((n >> 8) & 0xFF);
		}
		if ((byte)(n & 0xFF) != byte.MaxValue && (byte)(n & 0xFF) != 0)
		{
			return (byte)(n & 0xFF);
		}
		return array[3];
	}

	private static int HexToOct(string hexNum)
	{
		Dictionary<char, int> dictionary = new Dictionary<char, int>
		{
			{ '0', 0 },
			{ '1', 1 },
			{ '2', 2 },
			{ '3', 3 },
			{ '4', 4 },
			{ '5', 5 },
			{ '6', 6 },
			{ '7', 7 },
			{ '8', 8 },
			{ '9', 9 },
			{ 'A', 10 },
			{ 'B', 11 },
			{ 'C', 12 },
			{ 'D', 13 },
			{ 'E', 14 },
			{ 'F', 15 }
		};
		int num = 0;
		foreach (char key in hexNum)
		{
			num = num * 16 + dictionary[key];
		}
		return num;
	}
}
