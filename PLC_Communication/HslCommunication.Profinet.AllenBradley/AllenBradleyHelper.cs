using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Profinet.AllenBradley;

public class AllenBradleyHelper
{
	public const byte CIP_Execute_PCCC = 75;

	public const byte CIP_READ_DATA = 76;

	public const int CIP_WRITE_DATA = 77;

	public const int CIP_READ_WRITE_DATA = 78;

	public const int CIP_READ_FRAGMENT = 82;

	public const int CIP_WRITE_FRAGMENT = 83;

	public const byte CIP_READ_LIST = 85;

	public const int CIP_MULTIREAD_DATA = 4096;

	public const ushort CIP_Type_DATE = 8;

	public const ushort CIP_Type_TIME = 9;

	public const ushort CIP_Type_TimeAndDate = 10;

	public const ushort CIP_Type_TimeOfDate = 11;

	public const ushort CIP_Type_Bool = 193;

	public const ushort CIP_Type_Byte = 194;

	public const ushort CIP_Type_Word = 195;

	public const ushort CIP_Type_DWord = 196;

	public const ushort CIP_Type_LInt = 197;

	public const ushort CIP_Type_USInt = 198;

	public const ushort CIP_Type_UInt = 199;

	public const ushort CIP_Type_UDint = 200;

	public const ushort CIP_Type_ULint = 201;

	public const ushort CIP_Type_Real = 202;

	public const ushort CIP_Type_Double = 203;

	public const ushort CIP_Type_Struct = 204;

	public const ushort CIP_Type_String = 208;

	public const ushort CIP_Type_D1 = 209;

	public const ushort CIP_Type_D2 = 210;

	public const ushort CIP_Type_D3 = 211;

	public const ushort CIP_Type_D4 = 212;

	public const ushort CIP_Type_BitArray = 211;

	public const ushort OriginatorVendorID = 4105;

	public const uint OriginatorSerialNumber = 3248834059u;

	private static byte[] BuildRequestPathCommand(string address, bool isConnectedAddress = false)
	{
		using MemoryStream memoryStream = new MemoryStream();
		int num = HslHelper.ExtractParameter(ref address, "class", -1);
		if (num != -1)
		{
			int num2 = (address.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(address.Substring(2), 16) : Convert.ToInt32(address));
			if (num < 256)
			{
				memoryStream.WriteByte(32);
				memoryStream.WriteByte((byte)num);
			}
			else
			{
				memoryStream.WriteByte(33);
				memoryStream.WriteByte(0);
				memoryStream.WriteByte(BitConverter.GetBytes(num)[0]);
				memoryStream.WriteByte(BitConverter.GetBytes(num)[1]);
			}
			if (num2 < 256)
			{
				memoryStream.WriteByte(36);
				memoryStream.WriteByte((byte)num2);
			}
			else
			{
				memoryStream.WriteByte(37);
				memoryStream.WriteByte(0);
				memoryStream.WriteByte(BitConverter.GetBytes(num2)[0]);
				memoryStream.WriteByte(BitConverter.GetBytes(num2)[1]);
			}
		}
		else
		{
			string[] array = address.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string text = string.Empty;
				int num3 = array[i].LastIndexOf('[');
				int num4 = array[i].LastIndexOf(']');
				if (num3 > 0 && num4 > 0 && num4 > num3)
				{
					text = array[i].Substring(num3 + 1, num4 - num3 - 1);
					array[i] = array[i].Substring(0, num3);
				}
				memoryStream.WriteByte(145);
				byte[] bytes = Encoding.UTF8.GetBytes(array[i]);
				memoryStream.WriteByte((byte)bytes.Length);
				memoryStream.Write(bytes, 0, bytes.Length);
				if (bytes.Length % 2 == 1)
				{
					memoryStream.WriteByte(0);
				}
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				string[] array2 = text.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < array2.Length; j++)
				{
					int num5 = Convert.ToInt32(array2[j]);
					if (num5 < 256 && !isConnectedAddress)
					{
						memoryStream.WriteByte(40);
						memoryStream.WriteByte((byte)num5);
					}
					else if (num5 < 65536)
					{
						memoryStream.WriteByte(41);
						memoryStream.WriteByte(0);
						memoryStream.WriteByte(BitConverter.GetBytes(num5)[0]);
						memoryStream.WriteByte(BitConverter.GetBytes(num5)[1]);
					}
					else
					{
						memoryStream.WriteByte(42);
						memoryStream.WriteByte(0);
						memoryStream.Write(BitConverter.GetBytes(num5));
					}
				}
			}
		}
		return memoryStream.ToArray();
	}

	public static string ParseRequestPathCommand(byte[] pathCommand)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < pathCommand.Length; i++)
		{
			if (pathCommand[i] != 145)
			{
				continue;
			}
			string text = Encoding.UTF8.GetString(pathCommand, i + 2, pathCommand[i + 1]).TrimEnd(default(char));
			stringBuilder.Append(text);
			int num = 2 + text.Length;
			if (text.Length % 2 == 1)
			{
				num++;
			}
			if (pathCommand.Length > num + i)
			{
				if (pathCommand[i + num] == 40)
				{
					stringBuilder.Append($"[{pathCommand[i + num + 1]}]");
				}
				else if (pathCommand[i + num] == 41)
				{
					stringBuilder.Append($"[{BitConverter.ToUInt16(pathCommand, i + num + 2)}]");
				}
			}
			stringBuilder.Append(".");
		}
		if (stringBuilder[stringBuilder.Length - 1] == '.')
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		return stringBuilder.ToString();
	}

	public static OperateResult<int, int> ParseRequestPathSymbolInstanceAddressing(byte[] pathCommand)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (pathCommand[num3] == 32)
		{
			num3++;
			num = pathCommand[num3++];
		}
		else
		{
			if (pathCommand[num3] != 33)
			{
				return new OperateResult<int, int>();
			}
			num3 += 2;
			num = BitConverter.ToUInt16(pathCommand, num3);
			num3 += 2;
		}
		if (pathCommand[num3] == 36)
		{
			num3++;
			num2 = pathCommand[num3++];
		}
		else
		{
			if (pathCommand[num3] != 37)
			{
				return new OperateResult<int, int>();
			}
			num3 += 2;
			num2 = BitConverter.ToUInt16(pathCommand, num3);
			num3 += 2;
		}
		return OperateResult.CreateSuccessResult(num, num2);
	}

	public static byte[] BuildEnumeratorCommand(uint startInstance)
	{
		byte[] obj = new byte[16]
		{
			85, 3, 32, 107, 37, 0, 0, 0, 3, 0,
			1, 0, 2, 0, 8, 0
		};
		obj[6] = BitConverter.GetBytes(startInstance)[0];
		obj[7] = BitConverter.GetBytes(startInstance)[1];
		return obj;
	}

	public static byte[] BuildEnumeratorProgrameMainCommand(uint startInstance)
	{
		byte[] array = new byte[38];
		array[0] = 85;
		array[1] = 14;
		array[2] = 145;
		array[3] = 19;
		Encoding.ASCII.GetBytes("Program:MainProgram").CopyTo(array, 4);
		array[23] = 0;
		array[24] = 32;
		array[25] = 107;
		array[26] = 37;
		array[27] = 0;
		array[28] = BitConverter.GetBytes(startInstance)[0];
		array[29] = BitConverter.GetBytes(startInstance)[1];
		array[30] = 3;
		array[31] = 0;
		array[32] = 1;
		array[33] = 0;
		array[34] = 2;
		array[35] = 0;
		array[36] = 8;
		array[37] = 0;
		return array;
	}

	public static byte[] GetStructHandleCommand(ushort symbolType)
	{
		byte[] array = new byte[18];
		symbolType &= 0xFFF;
		array[0] = 3;
		array[1] = 3;
		array[2] = 32;
		array[3] = 108;
		array[4] = 37;
		array[5] = 0;
		array[6] = BitConverter.GetBytes(symbolType)[0];
		array[7] = BitConverter.GetBytes(symbolType)[1];
		array[8] = 4;
		array[9] = 0;
		array[10] = 4;
		array[11] = 0;
		array[12] = 5;
		array[13] = 0;
		array[14] = 2;
		array[15] = 0;
		array[16] = 1;
		array[17] = 0;
		return array;
	}

	public static byte[] GetStructItemNameType(ushort symbolType, AbStructHandle structHandle, int offset)
	{
		byte[] array = new byte[14];
		symbolType &= 0xFFF;
		byte[] bytes = BitConverter.GetBytes(structHandle.TemplateObjectDefinitionSize * 4 - 21);
		array[0] = 76;
		array[1] = 3;
		array[2] = 32;
		array[3] = 108;
		array[4] = 37;
		array[5] = 0;
		array[6] = BitConverter.GetBytes(symbolType)[0];
		array[7] = BitConverter.GetBytes(symbolType)[1];
		array[8] = BitConverter.GetBytes(offset)[0];
		array[9] = BitConverter.GetBytes(offset)[1];
		array[10] = BitConverter.GetBytes(offset)[2];
		array[11] = BitConverter.GetBytes(offset)[3];
		array[12] = bytes[0];
		array[13] = bytes[1];
		return array;
	}

	public static byte[] PackRequestHeader(ushort command, uint session, byte[] commandSpecificData, byte[] senderContext = null)
	{
		if (commandSpecificData == null)
		{
			commandSpecificData = new byte[0];
		}
		byte[] array = new byte[commandSpecificData.Length + 24];
		Array.Copy(commandSpecificData, 0, array, 24, commandSpecificData.Length);
		BitConverter.GetBytes(command).CopyTo(array, 0);
		BitConverter.GetBytes(session).CopyTo(array, 4);
		if (senderContext != null && senderContext.Length <= 8)
		{
			senderContext.CopyTo(array, 12);
		}
		BitConverter.GetBytes((ushort)commandSpecificData.Length).CopyTo(array, 2);
		return array;
	}

	public static byte[] PackRequestHeader(ushort command, uint error, uint session, byte[] commandSpecificData, byte[] senderContext = null)
	{
		byte[] array = PackRequestHeader(command, session, commandSpecificData, senderContext);
		BitConverter.GetBytes(error).CopyTo(array, 8);
		return array;
	}

	private static byte[] PackExecutePCCC(byte[] pccc)
	{
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(75);
		memoryStream.WriteByte(2);
		memoryStream.WriteByte(32);
		memoryStream.WriteByte(103);
		memoryStream.WriteByte(36);
		memoryStream.WriteByte(1);
		memoryStream.WriteByte(7);
		memoryStream.WriteByte(9);
		memoryStream.WriteByte(16);
		memoryStream.WriteByte(11);
		memoryStream.WriteByte(70);
		memoryStream.WriteByte(165);
		memoryStream.WriteByte(193);
		memoryStream.Write(pccc);
		byte[] array = memoryStream.ToArray();
		BitConverter.GetBytes((ushort)4105).CopyTo(array, 7);
		BitConverter.GetBytes(3248834059u).CopyTo(array, 9);
		return array;
	}

	public static OperateResult<byte[]> PackExecutePCCCRead(int tns, string address, int length)
	{
		OperateResult<byte[]> operateResult = AllenBradleyDF1Serial.BuildProtectedTypedLogicalReadWithThreeAddressFields(tns, address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return OperateResult.CreateSuccessResult(PackExecutePCCC(operateResult.Content));
	}

	public static OperateResult<byte[]> PackExecutePCCCWrite(int tns, string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = AllenBradleyDF1Serial.BuildProtectedTypedLogicalWriteWithThreeAddressFields(tns, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return OperateResult.CreateSuccessResult(PackExecutePCCC(operateResult.Content));
	}

	internal static OperateResult<byte[]> PackExecutePCCCWrite(int tns, string address, int bitIndex, bool value)
	{
		OperateResult<byte[]> operateResult = AllenBradleyDF1Serial.BuildProtectedTypedLogicalMaskWithThreeAddressFields(tns, address, bitIndex, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return OperateResult.CreateSuccessResult(PackExecutePCCC(operateResult.Content));
	}

	public static byte[] PackRequsetRead(string address, int length, bool isConnectedAddress = false)
	{
		byte[] array = new byte[1024];
		int num = 0;
		array[num++] = 76;
		num++;
		byte[] array2 = BuildRequestPathCommand(address, isConnectedAddress);
		array2.CopyTo(array, num);
		num += array2.Length;
		array[1] = (byte)((num - 2) / 2);
		array[num++] = BitConverter.GetBytes(length)[0];
		array[num++] = BitConverter.GetBytes(length)[1];
		byte[] array3 = new byte[num];
		Array.Copy(array, 0, array3, 0, num);
		return array3;
	}

	public static byte[] PackRequestReadSegment(string address, int startIndex, int length)
	{
		byte[] array = new byte[1024];
		int num = 0;
		array[num++] = 82;
		num++;
		byte[] array2 = BuildRequestPathCommand(address);
		array2.CopyTo(array, num);
		num += array2.Length;
		array[1] = (byte)((num - 2) / 2);
		array[num++] = BitConverter.GetBytes(length)[0];
		array[num++] = BitConverter.GetBytes(length)[1];
		array[num++] = BitConverter.GetBytes(startIndex)[0];
		array[num++] = BitConverter.GetBytes(startIndex)[1];
		array[num++] = BitConverter.GetBytes(startIndex)[2];
		array[num++] = BitConverter.GetBytes(startIndex)[3];
		byte[] array3 = new byte[num];
		Array.Copy(array, 0, array3, 0, num);
		return array3;
	}

	public static byte[] PackRequestWrite(string address, ushort typeCode, byte[] value, int length = 1, bool isConnectedAddress = false, bool paddingTail = false)
	{
		byte[] array = new byte[1024];
		int num = 0;
		array[num++] = 77;
		num++;
		byte[] array2 = BuildRequestPathCommand(address, isConnectedAddress);
		array2.CopyTo(array, num);
		num += array2.Length;
		array[1] = (byte)((num - 2) / 2);
		array[num++] = BitConverter.GetBytes(typeCode)[0];
		array[num++] = BitConverter.GetBytes(typeCode)[1];
		array[num++] = BitConverter.GetBytes(length)[0];
		array[num++] = BitConverter.GetBytes(length)[1];
		if (value == null)
		{
			value = new byte[0];
		}
		int num2 = 0;
		if (paddingTail && typeCode == 193 && length == 1 && value.Length % 2 > 0)
		{
			num2 = 1;
		}
		byte[] array3 = new byte[value.Length + num + num2];
		Array.Copy(array, 0, array3, 0, num);
		value.CopyTo(array3, num);
		return array3;
	}

	public static byte[] PackRequestWriteSegment(string address, ushort typeCode, byte[] value, int startIndex, int length = 1, bool isConnectedAddress = false)
	{
		byte[] array = new byte[1024];
		int num = 0;
		array[num++] = 83;
		num++;
		byte[] array2 = BuildRequestPathCommand(address, isConnectedAddress);
		array2.CopyTo(array, num);
		num += array2.Length;
		array[1] = (byte)((num - 2) / 2);
		array[num++] = BitConverter.GetBytes(typeCode)[0];
		array[num++] = BitConverter.GetBytes(typeCode)[1];
		array[num++] = BitConverter.GetBytes(length)[0];
		array[num++] = BitConverter.GetBytes(length)[1];
		array[num++] = BitConverter.GetBytes(startIndex)[0];
		array[num++] = BitConverter.GetBytes(startIndex)[1];
		array[num++] = BitConverter.GetBytes(startIndex)[2];
		array[num++] = BitConverter.GetBytes(startIndex)[3];
		if (value == null)
		{
			value = new byte[0];
		}
		byte[] array3 = new byte[value.Length + num];
		Array.Copy(array, 0, array3, 0, num);
		value.CopyTo(array3, num);
		return array3;
	}

	public static byte[] PackRequestReadModifyWrite(string address, uint orMask, uint andMask, bool isConnectedAddress = false)
	{
		byte[] array = new byte[1024];
		int num = 0;
		array[num++] = 78;
		num++;
		byte[] array2 = BuildRequestPathCommand(address, isConnectedAddress);
		array2.CopyTo(array, num);
		num += array2.Length;
		array[1] = (byte)((num - 2) / 2);
		array[num++] = 4;
		array[num++] = 0;
		BitConverter.GetBytes(orMask).CopyTo(array, num);
		num += 4;
		BitConverter.GetBytes(andMask).CopyTo(array, num);
		num += 4;
		return array.SelectBegin(num);
	}

	public static byte[] PackRequestReadModifyWrite(string address, int index, bool value, bool isConnectedAddress = false)
	{
		if (index / 32 != 0)
		{
			address += $"[{index / 32}]";
		}
		index %= 32;
		if (value)
		{
			uint num = 1u;
			num <<= index;
			return PackRequestReadModifyWrite(address, num, uint.MaxValue, isConnectedAddress);
		}
		uint num2 = 1u;
		num2 <<= index;
		num2 = ~num2;
		return PackRequestReadModifyWrite(address, 0u, num2, isConnectedAddress);
	}

	public static string AnalysisArrayIndex(string address, out int arrayIndex)
	{
		arrayIndex = 0;
		if (!address.EndsWith("]"))
		{
			return address;
		}
		int num = address.LastIndexOf('[');
		if (num < 0)
		{
			return address;
		}
		address = address.Remove(address.Length - 1);
		try
		{
			arrayIndex = int.Parse(address.Substring(num + 1));
			address = address.Substring(0, num);
			return address;
		}
		catch
		{
			return address;
		}
	}

	public static byte[] PackRequestWrite(string address, bool value)
	{
		address = AnalysisArrayIndex(address, out var arrayIndex);
		return PackRequestReadModifyWrite(address, arrayIndex, value);
	}

	public static byte[] PackCommandService(byte[] portSlot, params byte[][] cips)
	{
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(178);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(82);
		memoryStream.WriteByte(2);
		memoryStream.WriteByte(32);
		memoryStream.WriteByte(6);
		memoryStream.WriteByte(36);
		memoryStream.WriteByte(1);
		memoryStream.WriteByte(10);
		memoryStream.WriteByte(240);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		int num = 0;
		if (cips.Length == 1)
		{
			memoryStream.Write(cips[0], 0, cips[0].Length);
			num += cips[0].Length;
			if (cips[0].Length % 2 == 1)
			{
				memoryStream.WriteByte(0);
			}
		}
		else
		{
			memoryStream.WriteByte(10);
			memoryStream.WriteByte(2);
			memoryStream.WriteByte(32);
			memoryStream.WriteByte(2);
			memoryStream.WriteByte(36);
			memoryStream.WriteByte(1);
			num += 8;
			memoryStream.Write(BitConverter.GetBytes((ushort)cips.Length), 0, 2);
			ushort num2 = (ushort)(2 + 2 * cips.Length);
			num += 2 * cips.Length;
			for (int i = 0; i < cips.Length; i++)
			{
				memoryStream.Write(BitConverter.GetBytes(num2), 0, 2);
				num2 = (ushort)(num2 + cips[i].Length);
			}
			for (int j = 0; j < cips.Length; j++)
			{
				memoryStream.Write(cips[j], 0, cips[j].Length);
				num += cips[j].Length;
			}
		}
		if (portSlot != null)
		{
			memoryStream.WriteByte((byte)((portSlot.Length + 1) / 2));
			memoryStream.WriteByte(0);
			memoryStream.Write(portSlot, 0, portSlot.Length);
			if (portSlot.Length % 2 == 1)
			{
				memoryStream.WriteByte(0);
			}
		}
		byte[] array = memoryStream.ToArray();
		BitConverter.GetBytes((short)num).CopyTo(array, 12);
		BitConverter.GetBytes((short)(array.Length - 4)).CopyTo(array, 2);
		return array;
	}

	public static byte[] PackCleanCommandService(byte[] portSlot, params byte[][] cips)
	{
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(178);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		if (cips.Length == 1)
		{
			memoryStream.Write(cips[0], 0, cips[0].Length);
		}
		else
		{
			memoryStream.WriteByte(10);
			memoryStream.WriteByte(2);
			memoryStream.WriteByte(32);
			memoryStream.WriteByte(2);
			memoryStream.WriteByte(36);
			memoryStream.WriteByte(1);
			memoryStream.Write(BitConverter.GetBytes((ushort)cips.Length), 0, 2);
			ushort num = (ushort)(2 + 2 * cips.Length);
			for (int i = 0; i < cips.Length; i++)
			{
				memoryStream.Write(BitConverter.GetBytes(num), 0, 2);
				num = (ushort)(num + cips[i].Length);
			}
			for (int j = 0; j < cips.Length; j++)
			{
				memoryStream.Write(cips[j], 0, cips[j].Length);
			}
		}
		memoryStream.WriteByte((byte)((portSlot.Length + 1) / 2));
		memoryStream.WriteByte(0);
		memoryStream.Write(portSlot, 0, portSlot.Length);
		if (portSlot.Length % 2 == 1)
		{
			memoryStream.WriteByte(0);
		}
		byte[] array = memoryStream.ToArray();
		BitConverter.GetBytes((short)(array.Length - 4)).CopyTo(array, 2);
		return array;
	}

	public static byte[] PackCommandGetAttributesAll(byte[] portSlot, uint sessionHandle)
	{
		byte[] commandSpecificData = PackCommandSpecificData(new byte[4], PackCommandService(portSlot, new byte[6] { 1, 2, 32, 1, 36, 1 }));
		return PackRequestHeader(111, sessionHandle, commandSpecificData);
	}

	public static byte[] PackCommandResponse(byte[] data, bool isRead)
	{
		if (data == null)
		{
			return new byte[6] { 0, 0, 4, 0, 0, 0 };
		}
		return SoftBasic.SpliceArray<byte>(new byte[6]
		{
			(byte)(isRead ? 204u : 205u),
			0,
			0,
			0,
			0,
			0
		}, data);
	}

	public static byte[] PackCommandSpecificData(params byte[][] service)
	{
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(10);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(BitConverter.GetBytes(service.Length)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(service.Length)[1]);
		for (int i = 0; i < service.Length; i++)
		{
			memoryStream.Write(service[i], 0, service[i].Length);
		}
		return memoryStream.ToArray();
	}

	public static byte[] PackCommandSingleService(byte[] command, ushort code = 178, bool isConnected = false, ushort sequence = 0)
	{
		if (command == null)
		{
			command = new byte[0];
		}
		byte[] array = (isConnected ? new byte[6 + command.Length] : new byte[4 + command.Length]);
		array[0] = BitConverter.GetBytes(code)[0];
		array[1] = BitConverter.GetBytes(code)[1];
		array[2] = BitConverter.GetBytes(array.Length - 4)[0];
		array[3] = BitConverter.GetBytes(array.Length - 4)[1];
		command.CopyTo(array, isConnected ? 6 : 4);
		if (isConnected)
		{
			BitConverter.GetBytes(sequence).CopyTo(array, 4);
		}
		return array;
	}

	public static byte[] RegisterSessionHandle(byte[] senderContext = null)
	{
		byte[] commandSpecificData = new byte[4] { 1, 0, 0, 0 };
		return PackRequestHeader(101, 0u, commandSpecificData, senderContext);
	}

	public static byte[] UnRegisterSessionHandle(uint sessionHandle)
	{
		return PackRequestHeader(102, sessionHandle, new byte[0]);
	}

	public static OperateResult CheckResponse(byte[] response)
	{
		try
		{
			int num = BitConverter.ToInt32(response, 8);
			if (num == 0)
			{
				return OperateResult.CreateSuccessResult();
			}
			string empty = string.Empty;
			int err = num;
			if (1 == 0)
			{
			}
			string msg = num switch
			{
				1 => StringResources.Language.AllenBradleySessionStatus01, 
				2 => StringResources.Language.AllenBradleySessionStatus02, 
				3 => StringResources.Language.AllenBradleySessionStatus03, 
				100 => StringResources.Language.AllenBradleySessionStatus64, 
				101 => StringResources.Language.AllenBradleySessionStatus65, 
				105 => StringResources.Language.AllenBradleySessionStatus69, 
				_ => StringResources.Language.UnknownError, 
			};
			if (1 == 0)
			{
			}
			return new OperateResult(err, msg);
		}
		catch (Exception ex)
		{
			return new OperateResult("CheckResponse failed: " + ex.Message + Environment.NewLine + "Source: " + response.ToHexString(' '));
		}
	}

	public static OperateResult<byte[], ushort, bool> ExtractActualData(byte[] response, bool isRead)
	{
		List<byte> list = new List<byte>();
		try
		{
			int num = 38;
			bool value = false;
			ushort value2 = 0;
			ushort num2 = BitConverter.ToUInt16(response, 38);
			if (BitConverter.ToInt32(response, 40) == 138)
			{
				num = 44;
				int num3 = BitConverter.ToUInt16(response, num);
				for (int i = 0; i < num3; i++)
				{
					int num4 = BitConverter.ToUInt16(response, num + 2 + i * 2) + num;
					int num5 = ((i == num3 - 1) ? response.Length : (BitConverter.ToUInt16(response, num + 4 + i * 2) + num));
					ushort num6 = BitConverter.ToUInt16(response, num4 + 2);
					switch (num6)
					{
					case 4:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.AllenBradley04
						};
					case 5:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.AllenBradley05
						};
					case 6:
						if (response[num + 2] == 210 || response[num + 2] == 204)
						{
							return new OperateResult<byte[], ushort, bool>
							{
								ErrorCode = num6,
								Message = StringResources.Language.AllenBradley06
							};
						}
						break;
					case 10:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.AllenBradley0A
						};
					case 12:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.AllenBradley0C
						};
					case 19:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.AllenBradley13
						};
					case 28:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.AllenBradley1C
						};
					case 30:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.AllenBradley1E
						};
					case 38:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.AllenBradley26
						};
					default:
						return new OperateResult<byte[], ushort, bool>
						{
							ErrorCode = num6,
							Message = StringResources.Language.UnknownError
						};
					case 0:
						break;
					}
					if (isRead)
					{
						for (int j = num4 + 6; j < num5; j++)
						{
							list.Add(response[j]);
						}
					}
				}
			}
			else
			{
				byte b = response[num + 4];
				switch (b)
				{
				case 4:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley04
					};
				case 5:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley05
					};
				case 6:
					value = true;
					break;
				case 10:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley0A
					};
				case 12:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley0C
					};
				case 19:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley13
					};
				case 28:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley1C
					};
				case 30:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley1E
					};
				case 32:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley20
					};
				case 38:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.AllenBradley26
					};
				default:
					return new OperateResult<byte[], ushort, bool>
					{
						ErrorCode = b,
						Message = StringResources.Language.UnknownError
					};
				case 0:
					break;
				}
				if (response[num + 2] == 205 || response[num + 2] == 211)
				{
					return OperateResult.CreateSuccessResult(list.ToArray(), value2, value);
				}
				if (response[num + 2] == 204 || response[num + 2] == 210)
				{
					for (int k = num + 8; k < num + 2 + num2; k++)
					{
						list.Add(response[k]);
					}
					value2 = BitConverter.ToUInt16(response, num + 6);
				}
				else if (response[num + 2] == 213)
				{
					for (int l = num + 6; l < num + 2 + num2; l++)
					{
						list.Add(response[l]);
					}
				}
			}
			return OperateResult.CreateSuccessResult(list.ToArray(), value2, value);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[], ushort, bool>("ExtractActualData failed: " + ex.Message + Environment.NewLine + response.ToHexString(' '));
		}
	}

	internal static OperateResult<string> ExtractActualString(OperateResult<byte[], ushort, bool> read, IByteTransform byteTransform, Encoding encoding)
	{
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		try
		{
			if (read.Content2 == 218)
			{
				if (read.Content1.Length >= 1)
				{
					if (read.Content1[0] == 0)
					{
						return OperateResult.CreateSuccessResult(string.Empty);
					}
					if (read.Content1[0] >= read.Content1.Length)
					{
						return OperateResult.CreateSuccessResult(encoding.GetString(read.Content1));
					}
					return OperateResult.CreateSuccessResult(encoding.GetString(read.Content1, 1, read.Content1[0]));
				}
				return OperateResult.CreateSuccessResult(encoding.GetString(read.Content1));
			}
			if (read.Content1.Length >= 6)
			{
				int num = byteTransform.TransInt32(read.Content1, 2);
				if (num == 0)
				{
					return OperateResult.CreateSuccessResult(string.Empty);
				}
				return OperateResult.CreateSuccessResult(encoding.GetString(read.Content1, 6, num));
			}
			return OperateResult.CreateSuccessResult(encoding.GetString(read.Content1));
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(ex.Message + " Source: " + read.Content1.ToHexString(' '));
		}
	}

	public static OperateResult<string> ReadPlcType(IReadWriteDevice plc)
	{
		byte[] send = "00 00 00 00 00 00 02 00 00 00 00 00 b2 00 06 00 01 02 20 01 24 01".ToHexBytes();
		OperateResult<byte[]> operateResult = plc.ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		if (operateResult.Content.Length > 59 && operateResult.Content.Length >= 59 + operateResult.Content[58])
		{
			return OperateResult.CreateSuccessResult(Encoding.UTF8.GetString(operateResult.Content, 59, operateResult.Content[58]));
		}
		return new OperateResult<string>("Data is too short: " + operateResult.Content.ToHexString(' '));
	}

	public static async Task<OperateResult<string>> ReadPlcTypeAsync(IReadWriteDevice plc)
	{
		byte[] buffer = "00 00 00 00 00 00 02 00 00 00 00 00 b2 00 06 00 01 02 20 01 24 01".ToHexBytes();
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(buffer);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		if (read.Content.Length > 59 && read.Content.Length >= 59 + read.Content[58])
		{
			return OperateResult.CreateSuccessResult(Encoding.UTF8.GetString(read.Content, 59, read.Content[58]));
		}
		return new OperateResult<string>("Data is too short: " + read.Content.ToHexString(' '));
	}

	public static OperateResult<DateTime> ReadDate(IReadWriteCip plc, string address)
	{
		OperateResult<long> operateResult = plc.ReadInt64(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult);
		}
		long value = operateResult.Content / 100;
		return OperateResult.CreateSuccessResult(new DateTime(1970, 1, 1).AddTicks(value));
	}

	public static OperateResult WriteDate(IReadWriteCip plc, string address, DateTime date)
	{
		long value = (date.Date - new DateTime(1970, 1, 1)).Ticks * 100;
		return plc.WriteTag(address, 8, plc.ByteTransform.TransByte(value));
	}

	public static OperateResult WriteTimeAndDate(IReadWriteCip plc, string address, DateTime date)
	{
		long value = (date - new DateTime(1970, 1, 1)).Ticks * 100;
		return plc.WriteTag(address, 10, plc.ByteTransform.TransByte(value));
	}

	public static OperateResult<TimeSpan> ReadTime(IReadWriteCip plc, string address)
	{
		OperateResult<long> operateResult = plc.ReadInt64(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TimeSpan>(operateResult);
		}
		long value = operateResult.Content / 100;
		return OperateResult.CreateSuccessResult(TimeSpan.FromTicks(value));
	}

	public static OperateResult WriteTime(IReadWriteCip plc, string address, TimeSpan time)
	{
		return plc.WriteTag(address, 9, plc.ByteTransform.TransByte(time.Ticks * 100));
	}

	public static OperateResult WriteTimeOfDate(IReadWriteCip plc, string address, TimeSpan timeOfDate)
	{
		return plc.WriteTag(address, 11, plc.ByteTransform.TransByte(timeOfDate.Ticks * 100));
	}

	public static async Task<OperateResult<DateTime>> ReadDateAsync(IReadWriteCip plc, string address)
	{
		OperateResult<long> read = await plc.ReadInt64Async(address);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(read);
		}
		long tick = read.Content / 100;
		return OperateResult.CreateSuccessResult(new DateTime(1970, 1, 1).AddTicks(tick));
	}

	public static async Task<OperateResult> WriteDateAsync(IReadWriteCip plc, string address, DateTime date)
	{
		long tick = (date.Date - new DateTime(1970, 1, 1)).Ticks * 100;
		return await plc.WriteTagAsync(address, 8, plc.ByteTransform.TransByte(tick));
	}

	public static async Task<OperateResult> WriteTimeAndDateAsync(IReadWriteCip plc, string address, DateTime date)
	{
		long tick = (date - new DateTime(1970, 1, 1)).Ticks * 100;
		return await plc.WriteTagAsync(address, 10, plc.ByteTransform.TransByte(tick));
	}

	public static async Task<OperateResult<TimeSpan>> ReadTimeAsync(IReadWriteCip plc, string address)
	{
		OperateResult<long> read = await plc.ReadInt64Async(address);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<TimeSpan>(read);
		}
		long tick = read.Content / 100;
		return OperateResult.CreateSuccessResult(TimeSpan.FromTicks(tick));
	}

	public static async Task<OperateResult> WriteTimeAsync(IReadWriteCip plc, string address, TimeSpan time)
	{
		return await plc.WriteTagAsync(address, 9, plc.ByteTransform.TransByte(time.Ticks * 100));
	}

	public static async Task<OperateResult> WriteTimeOfDateAsync(IReadWriteCip plc, string address, TimeSpan timeOfDate)
	{
		return await plc.WriteTagAsync(address, 11, plc.ByteTransform.TransByte(timeOfDate.Ticks * 100));
	}
}
