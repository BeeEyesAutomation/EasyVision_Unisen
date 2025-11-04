using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;

namespace HslCommunication.Profinet.Siemens.Helper;

internal class SiemensS7Helper
{
	internal static OperateResult<byte[]> AnalysisReadBit(byte[] content)
	{
		try
		{
			int num = 1;
			if (content.Length >= 21 && content[20] == 1)
			{
				byte[] array = new byte[num];
				if (22 < content.Length)
				{
					if (content[21] != byte.MaxValue || content[22] != 3)
					{
						if (content[21] == 5 && content[22] == 0)
						{
							return new OperateResult<byte[]>(content[21], StringResources.Language.SiemensReadLengthOverPlcAssign);
						}
						if (content[21] == 6 && content[22] == 0)
						{
							return new OperateResult<byte[]>(content[21], StringResources.Language.SiemensError0006);
						}
						if (content[21] == 10 && content[22] == 0)
						{
							return new OperateResult<byte[]>(content[21], StringResources.Language.SiemensError000A);
						}
						return new OperateResult<byte[]>(content[21], StringResources.Language.UnknownError + " Source: " + content.ToHexString(' '));
					}
					array[0] = content[25];
				}
				return OperateResult.CreateSuccessResult(array);
			}
			return new OperateResult<byte[]>(StringResources.Language.SiemensDataLengthCheckFailed);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("AnalysisReadBit failed: " + ex.Message + Environment.NewLine + " Msg:" + SoftBasic.ByteToHexString(content, ' '));
		}
	}

	internal static List<S7AddressData[]> ArraySplitByLength(S7AddressData[] s7Addresses, int pduLength)
	{
		List<S7AddressData[]> list = new List<S7AddressData[]>();
		List<S7AddressData> list2 = new List<S7AddressData>();
		int num = 0;
		for (int i = 0; i < s7Addresses.Length; i++)
		{
			if (list2.Count >= 19 || num + s7Addresses[i].Length + list2.Count * 4 >= pduLength)
			{
				if (list2.Count > 0)
				{
					list.Add(list2.ToArray());
					list2.Clear();
				}
				num = 0;
			}
			list2.Add(s7Addresses[i]);
			num += s7Addresses[i].Length;
		}
		if (list2.Count > 0)
		{
			list.Add(list2.ToArray());
		}
		return list;
	}

	internal static S7AddressData[] SplitS7Address(S7AddressData s7Address, int pduLength)
	{
		List<S7AddressData> list = new List<S7AddressData>();
		int i = 0;
		ushort num;
		for (int length = s7Address.Length; i < length; i += num)
		{
			num = (ushort)Math.Min(length - i, pduLength);
			S7AddressData s7AddressData = new S7AddressData(s7Address);
			if (s7Address.DataCode == 31 || s7Address.DataCode == 30)
			{
				s7AddressData.AddressStart = s7Address.AddressStart + i / 2;
			}
			else
			{
				s7AddressData.AddressStart = s7Address.AddressStart + i * 8;
			}
			s7AddressData.Length = num;
			list.Add(s7AddressData);
		}
		return list.ToArray();
	}

	internal static OperateResult<byte[]> AnalysisReadByte(byte[] content)
	{
		try
		{
			List<byte> list = new List<byte>();
			if (content.Length >= 21)
			{
				for (int i = 21; i < content.Length - 1; i++)
				{
					if (content[i] == byte.MaxValue && content[i + 1] == 4)
					{
						int num = (content[i + 2] * 256 + content[i + 3]) / 8;
						list.AddRange(content.SelectMiddle(i + 4, num));
						i += num + 3;
					}
					else if (content[i] == byte.MaxValue && content[i + 1] == 9)
					{
						int num2 = content[i + 2] * 256 + content[i + 3];
						if (num2 % 3 == 0)
						{
							for (int j = 0; j < num2 / 3; j++)
							{
								list.AddRange(content.SelectMiddle(i + 5 + 3 * j, 2));
							}
						}
						else
						{
							for (int k = 0; k < num2 / 5; k++)
							{
								list.AddRange(content.SelectMiddle(i + 7 + 5 * k, 2));
							}
						}
						i += num2 + 4;
					}
					else
					{
						if (content[i] == 5 && content[i + 1] == 0)
						{
							return new OperateResult<byte[]>(content[i], StringResources.Language.SiemensReadLengthOverPlcAssign);
						}
						if (content[i] == 6 && content[i + 1] == 0)
						{
							return new OperateResult<byte[]>(content[i], StringResources.Language.SiemensError0006);
						}
						if (content[i] == 10 && content[i + 1] == 0)
						{
							return new OperateResult<byte[]>(content[i], StringResources.Language.SiemensError000A);
						}
					}
				}
				return OperateResult.CreateSuccessResult(list.ToArray());
			}
			return new OperateResult<byte[]>(StringResources.Language.SiemensDataLengthCheckFailed + " Msg: " + SoftBasic.ByteToHexString(content, ' '));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("AnalysisReadByte failed: " + ex.Message + Environment.NewLine + " Msg:" + SoftBasic.ByteToHexString(content, ' '));
		}
	}

	public static OperateResult<string> ReadString(IReadWriteNet plc, SiemensPLCS currentPlc, string address, Encoding encoding)
	{
		if (currentPlc != SiemensPLCS.S200Smart)
		{
			OperateResult<byte[]> operateResult = plc.Read(address, 2);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult);
			}
			if (operateResult.Content[0] == 0 || operateResult.Content[0] == byte.MaxValue)
			{
				return new OperateResult<string>("Value in plc is not string type");
			}
			OperateResult<byte[]> operateResult2 = plc.Read(address, (ushort)(2 + operateResult.Content[1]));
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult2);
			}
			return OperateResult.CreateSuccessResult(encoding.GetString(operateResult2.Content, 2, operateResult2.Content.Length - 2));
		}
		OperateResult<byte[]> operateResult3 = plc.Read(address, 1);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult3);
		}
		OperateResult<byte[]> operateResult4 = plc.Read(address, (ushort)(1 + operateResult3.Content[0]));
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult4);
		}
		return OperateResult.CreateSuccessResult(encoding.GetString(operateResult4.Content, 1, operateResult4.Content.Length - 1));
	}

	public static OperateResult Write(IReadWriteNet plc, SiemensPLCS currentPlc, string address, string value, Encoding encoding)
	{
		if (value == null)
		{
			value = string.Empty;
		}
		byte[] array = encoding.GetBytes(value);
		if (encoding == Encoding.Unicode)
		{
			array = SoftBasic.BytesReverseByWord(array);
		}
		if (currentPlc != SiemensPLCS.S200Smart)
		{
			OperateResult<byte[]> operateResult = plc.Read(address, 2);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			if (operateResult.Content[0] == byte.MaxValue)
			{
				return new OperateResult<string>("Value in plc is not string type");
			}
			if (operateResult.Content[0] == 0)
			{
				operateResult.Content[0] = 254;
			}
			if (array.Length > operateResult.Content[0])
			{
				return new OperateResult<string>("String length is too long than plc defined");
			}
			return plc.Write(address, SoftBasic.SpliceArray<byte>(new byte[2]
			{
				operateResult.Content[0],
				(byte)array.Length
			}, array));
		}
		return plc.Write(address, SoftBasic.SpliceArray<byte>(new byte[1] { (byte)array.Length }, array));
	}

	public static OperateResult<string> ReadWString(IReadWriteNet plc, SiemensPLCS currentPlc, string address)
	{
		if (currentPlc != SiemensPLCS.S200Smart)
		{
			OperateResult<byte[]> operateResult = plc.Read(address, 4);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult);
			}
			OperateResult<byte[]> operateResult2 = plc.Read(address, (ushort)(4 + (operateResult.Content[2] * 256 + operateResult.Content[3]) * 2));
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult2);
			}
			return OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(SoftBasic.BytesReverseByWord(operateResult2.Content.RemoveBegin(4))));
		}
		OperateResult<byte[]> operateResult3 = plc.Read(address, 1);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult3);
		}
		OperateResult<byte[]> operateResult4 = plc.Read(address, (ushort)(1 + operateResult3.Content[0] * 2));
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult4);
		}
		return OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(operateResult4.Content, 1, operateResult4.Content.Length - 1));
	}

	public static OperateResult WriteWString(IReadWriteNet plc, SiemensPLCS currentPlc, string address, string value)
	{
		if (currentPlc != SiemensPLCS.S200Smart)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			byte[] array = Encoding.Unicode.GetBytes(value).ReverseByWord();
			OperateResult<byte[]> operateResult = plc.Read(address, 4);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			int num = operateResult.Content[0] * 256 + operateResult.Content[1];
			if (num == 0)
			{
				num = 254;
				operateResult.Content[1] = 254;
			}
			if (value.Length > num)
			{
				return new OperateResult<string>("String length is too long than plc defined");
			}
			byte[] array2 = new byte[array.Length + 4];
			array2[0] = operateResult.Content[0];
			array2[1] = operateResult.Content[1];
			array2[2] = BitConverter.GetBytes(value.Length)[1];
			array2[3] = BitConverter.GetBytes(value.Length)[0];
			array.CopyTo(array2, 4);
			return plc.Write(address, array2);
		}
		return plc.Write(address, value, Encoding.Unicode);
	}

	public static async Task<OperateResult<string>> ReadStringAsync(IReadWriteNet plc, SiemensPLCS currentPlc, string address, Encoding encoding)
	{
		if (currentPlc != SiemensPLCS.S200Smart)
		{
			OperateResult<byte[]> read2 = await plc.ReadAsync(address, 2);
			if (!read2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(read2);
			}
			if (read2.Content[0] == 0 || read2.Content[0] == byte.MaxValue)
			{
				return new OperateResult<string>("Value in plc is not string type");
			}
			OperateResult<byte[]> readString2 = await plc.ReadAsync(address, (ushort)(2 + read2.Content[1]));
			if (!readString2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(readString2);
			}
			return OperateResult.CreateSuccessResult(encoding.GetString(readString2.Content, 2, readString2.Content.Length - 2));
		}
		OperateResult<byte[]> read3 = await plc.ReadAsync(address, 1);
		if (!read3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read3);
		}
		OperateResult<byte[]> readString3 = await plc.ReadAsync(address, (ushort)(1 + read3.Content[0]));
		if (!readString3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(readString3);
		}
		return OperateResult.CreateSuccessResult(encoding.GetString(readString3.Content, 1, readString3.Content.Length - 1));
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteNet plc, SiemensPLCS currentPlc, string address, string value, Encoding encoding)
	{
		if (value == null)
		{
			value = string.Empty;
		}
		byte[] buffer = encoding.GetBytes(value);
		if (encoding == Encoding.Unicode)
		{
			buffer = SoftBasic.BytesReverseByWord(buffer);
		}
		if (currentPlc != SiemensPLCS.S200Smart)
		{
			OperateResult<byte[]> readLength = await plc.ReadAsync(address, 2);
			if (!readLength.IsSuccess)
			{
				return readLength;
			}
			if (readLength.Content[0] == byte.MaxValue)
			{
				return new OperateResult<string>("Value in plc is not string type");
			}
			if (readLength.Content[0] == 0)
			{
				readLength.Content[0] = 254;
			}
			if (buffer.Length > readLength.Content[0])
			{
				return new OperateResult<string>("String length is too long than plc defined");
			}
			return await plc.WriteAsync(address, SoftBasic.SpliceArray<byte>(new byte[2]
			{
				readLength.Content[0],
				(byte)buffer.Length
			}, buffer));
		}
		return await plc.WriteAsync(address, SoftBasic.SpliceArray<byte>(new byte[1] { (byte)buffer.Length }, buffer));
	}

	public static async Task<OperateResult<string>> ReadWStringAsync(IReadWriteNet plc, SiemensPLCS currentPlc, string address)
	{
		if (currentPlc != SiemensPLCS.S200Smart)
		{
			OperateResult<byte[]> read2 = await plc.ReadAsync(address, 4);
			if (!read2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(read2);
			}
			OperateResult<byte[]> readString2 = await plc.ReadAsync(address, (ushort)(4 + (read2.Content[2] * 256 + read2.Content[3]) * 2));
			if (!readString2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(readString2);
			}
			return OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(SoftBasic.BytesReverseByWord(readString2.Content.RemoveBegin(4))));
		}
		OperateResult<byte[]> read3 = await plc.ReadAsync(address, 1);
		if (!read3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read3);
		}
		OperateResult<byte[]> readString3 = await plc.ReadAsync(address, (ushort)(1 + read3.Content[0] * 2));
		if (!readString3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(readString3);
		}
		return OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(readString3.Content, 1, readString3.Content.Length - 1));
	}

	public static async Task<OperateResult> WriteWStringAsync(IReadWriteNet plc, SiemensPLCS currentPlc, string address, string value)
	{
		if (currentPlc != SiemensPLCS.S200Smart)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			byte[] buffer2 = Encoding.Unicode.GetBytes(value);
			buffer2 = SoftBasic.BytesReverseByWord(buffer2);
			OperateResult<byte[]> readLength = await plc.ReadAsync(address, 4);
			if (!readLength.IsSuccess)
			{
				return readLength;
			}
			int defineLength = readLength.Content[0] * 256 + readLength.Content[1];
			if (defineLength == 0)
			{
				defineLength = 254;
				readLength.Content[1] = 254;
			}
			if (value.Length > defineLength)
			{
				return new OperateResult<string>("String length is too long than plc defined");
			}
			byte[] write = new byte[buffer2.Length + 4];
			write[0] = readLength.Content[0];
			write[1] = readLength.Content[1];
			write[2] = BitConverter.GetBytes(value.Length)[1];
			write[3] = BitConverter.GetBytes(value.Length)[0];
			buffer2.CopyTo(write, 4);
			return await plc.WriteAsync(address, write);
		}
		return await plc.WriteAsync(address, value, Encoding.Unicode);
	}
}
