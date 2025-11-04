#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HslCommunication.Core;
using Newtonsoft.Json.Linq;

namespace HslCommunication.BasicFramework;

public class SoftBasic
{
	public static SystemVersion FrameworkVersion => new SystemVersion("12.3.3");

	public static string CalculateFileMD5(string filePath)
	{
		string result = string.Empty;
		using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
		{
			result = CalculateStreamMD5(stream);
		}
		return result;
	}

	public static string CalculateStreamMD5(Stream stream)
	{
		byte[] array = null;
		using (MD5 mD = new MD5CryptoServiceProvider())
		{
			array = mD.ComputeHash(stream);
		}
		return BitConverter.ToString(array).Replace("-", "");
	}

	public static string CalculateStreamMD5(string data)
	{
		return CalculateStreamMD5(data, Encoding.UTF8);
	}

	public static string CalculateStreamMD5(string data, Encoding encode)
	{
		string result = string.Empty;
		using (MD5 mD = new MD5CryptoServiceProvider())
		{
			byte[] array = mD.ComputeHash(encode.GetBytes(data));
			result = BitConverter.ToString(array).Replace("-", "");
		}
		return result;
	}

	public static string GetSizeDescription(long size)
	{
		if (size < 1000)
		{
			return size + " B";
		}
		if (size < 1000000)
		{
			return ((float)size / 1024f).ToString("F2") + " Kb";
		}
		if (size < 1000000000)
		{
			return ((float)size / 1024f / 1024f).ToString("F2") + " Mb";
		}
		return ((float)size / 1024f / 1024f / 1024f).ToString("F2") + " Gb";
	}

	public static string GetTimeSpanDescription(TimeSpan ts)
	{
		if (ts.TotalSeconds <= 60.0)
		{
			return (int)ts.TotalSeconds + StringResources.Language.TimeDescriptionSecond;
		}
		if (ts.TotalMinutes <= 60.0)
		{
			return ts.TotalMinutes.ToString("F1") + StringResources.Language.TimeDescriptionMinute;
		}
		if (ts.TotalHours <= 24.0)
		{
			return ts.TotalHours.ToString("F2") + StringResources.Language.TimeDescriptionHour;
		}
		return ts.TotalDays.ToString("F2") + StringResources.Language.TimeDescriptionDay;
	}

	public static string ArrayFormat<T>(T[] array)
	{
		return ArrayFormat(array, string.Empty);
	}

	public static string ArrayFormat<T>(T[] array, string format)
	{
		if (array == null)
		{
			return "NULL";
		}
		StringBuilder stringBuilder = new StringBuilder("[");
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(string.IsNullOrEmpty(format) ? array[i].ToString() : string.Format(format, array[i]));
			if (i != array.Length - 1)
			{
				stringBuilder.Append(",");
			}
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}

	public static string ArrayFormat<T>(T array)
	{
		return ArrayFormat(array, string.Empty);
	}

	public static string ArrayFormat<T>(T array, string format)
	{
		StringBuilder stringBuilder = new StringBuilder("[");
		if (array is Array array2)
		{
			foreach (object item in array2)
			{
				stringBuilder.Append(string.IsNullOrEmpty(format) ? item.ToString() : string.Format(format, item));
				stringBuilder.Append(",");
			}
			if (array2.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ',')
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
		}
		else
		{
			stringBuilder.Append(string.IsNullOrEmpty(format) ? array.ToString() : string.Format(format, array));
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}

	public static void AddArrayData<T>(ref T[] array, T[] data, int max)
	{
		if (data == null || data.Length == 0)
		{
			return;
		}
		if (array.Length == max)
		{
			Array.Copy(array, data.Length, array, 0, array.Length - data.Length);
			Array.Copy(data, 0, array, array.Length - data.Length, data.Length);
		}
		else if (array.Length + data.Length > max)
		{
			T[] array2 = new T[max];
			for (int i = 0; i < max - data.Length; i++)
			{
				array2[i] = array[i + (array.Length - max + data.Length)];
			}
			for (int j = 0; j < data.Length; j++)
			{
				array2[array2.Length - data.Length + j] = data[j];
			}
			array = array2;
		}
		else
		{
			T[] array3 = new T[array.Length + data.Length];
			for (int k = 0; k < array.Length; k++)
			{
				array3[k] = array[k];
			}
			for (int l = 0; l < data.Length; l++)
			{
				array3[array3.Length - data.Length + l] = data[l];
			}
			array = array3;
		}
	}

	public static T[] ArrayExpandToLength<T>(T[] data, int length)
	{
		if (data == null)
		{
			return new T[length];
		}
		if (data.Length == length)
		{
			return data;
		}
		T[] array = new T[length];
		Array.Copy(data, array, Math.Min(data.Length, array.Length));
		return array;
	}

	public static T[] ArrayExpandToLengthEven<T>(T[] data)
	{
		if (data == null)
		{
			return new T[0];
		}
		if (data.Length % 2 == 1)
		{
			return ArrayExpandToLength(data, data.Length + 1);
		}
		return data;
	}

	public static List<T[]> ArraySplitByLength<T>(T[] array, int length)
	{
		if (array == null)
		{
			return new List<T[]>();
		}
		List<T[]> list = new List<T[]>();
		int num = 0;
		while (num < array.Length)
		{
			if (num + length < array.Length)
			{
				T[] array2 = new T[length];
				Array.Copy(array, num, array2, 0, length);
				num += length;
				list.Add(array2);
			}
			else
			{
				T[] array3 = new T[array.Length - num];
				Array.Copy(array, num, array3, 0, array3.Length);
				num += length;
				list.Add(array3);
			}
		}
		return list;
	}

	public static int[] SplitIntegerToArray(int integer, int everyLength)
	{
		int[] array = new int[integer / everyLength + ((integer % everyLength != 0) ? 1 : 0)];
		for (int i = 0; i < array.Length; i++)
		{
			if (i == array.Length - 1)
			{
				array[i] = ((integer % everyLength == 0) ? everyLength : (integer % everyLength));
			}
			else
			{
				array[i] = everyLength;
			}
		}
		return array;
	}

	public static bool IsTwoBytesEquel(byte[] b1, int start1, byte[] b2, int start2, int length)
	{
		if (b1 == null || b2 == null)
		{
			return false;
		}
		for (int i = 0; i < length; i++)
		{
			if (b1[i + start1] != b2[i + start2])
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsTwoBytesEquel(byte[] b1, byte[] b2)
	{
		if (b1 == null || b2 == null)
		{
			return false;
		}
		if (b1.Length != b2.Length)
		{
			return false;
		}
		return IsTwoBytesEquel(b1, 0, b2, 0, b1.Length);
	}

	public static bool IsByteTokenEquel(byte[] head, Guid token)
	{
		return IsTwoBytesEquel(head, 12, token.ToByteArray(), 0, 16);
	}

	public static bool IsTwoTokenEquel(Guid token1, Guid token2)
	{
		return IsTwoBytesEquel(token1.ToByteArray(), 0, token2.ToByteArray(), 0, 16);
	}

	public static TEnum[] GetEnumValues<TEnum>() where TEnum : struct
	{
		return (TEnum[])Enum.GetValues(typeof(TEnum));
	}

	public static TEnum GetEnumFromString<TEnum>(string value) where TEnum : struct
	{
		return (TEnum)Enum.Parse(typeof(TEnum), value);
	}

	public static T GetValueFromJsonObject<T>(JObject json, string name, T defaultValue)
	{
		if (json.Property(name) != null)
		{
			return json.Property(name).Value.Value<T>();
		}
		return defaultValue;
	}

	public static void JsonSetValue<T>(JObject json, string property, T value)
	{
		if (json.Property(property) != null)
		{
			json.Property(property).Value = new JValue(value);
		}
		else
		{
			json.Add(property, new JValue(value));
		}
	}

	public static T GetXmlValue<T>(XElement element, string name, T defaultValue)
	{
		if (element.Attribute(name) == null)
		{
			return defaultValue;
		}
		Type typeFromHandle = typeof(T);
		if (typeFromHandle == typeof(bool))
		{
			return (T)(object)bool.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(bool[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<bool>();
		}
		if (typeFromHandle == typeof(byte))
		{
			return (T)(object)byte.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(byte[]))
		{
			return (T)(object)element.Attribute(name).Value.ToHexBytes();
		}
		if (typeFromHandle == typeof(sbyte))
		{
			return (T)(object)sbyte.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(sbyte[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<sbyte>();
		}
		if (typeFromHandle == typeof(short))
		{
			return (T)(object)short.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(short[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<short>();
		}
		if (typeFromHandle == typeof(ushort))
		{
			return (T)(object)ushort.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(ushort[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<ushort>();
		}
		if (typeFromHandle == typeof(int))
		{
			return (T)(object)int.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(int[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<int>();
		}
		if (typeFromHandle == typeof(uint))
		{
			return (T)(object)uint.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(uint[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<uint>();
		}
		if (typeFromHandle == typeof(long))
		{
			return (T)(object)long.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(long[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<long>();
		}
		if (typeFromHandle == typeof(ulong))
		{
			return (T)(object)ulong.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(ulong[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<ulong>();
		}
		if (typeFromHandle == typeof(float))
		{
			return (T)(object)float.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(float[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<float>();
		}
		if (typeFromHandle == typeof(double))
		{
			return (T)(object)double.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(double[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<double>();
		}
		if (typeFromHandle == typeof(DateTime))
		{
			return (T)(object)DateTime.Parse(element.Attribute(name).Value);
		}
		if (typeFromHandle == typeof(DateTime[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<DateTime>();
		}
		if (typeFromHandle == typeof(string))
		{
			return (T)(object)element.Attribute(name).Value;
		}
		if (typeFromHandle == typeof(string[]))
		{
			return (T)(object)element.Attribute(name).Value.ToStringArray<string>();
		}
		throw new Exception("not supported type:" + typeFromHandle.Name);
	}

	public static string GetExceptionMessage(Exception ex)
	{
		return StringResources.Language.ExceptionMessage + ex.Message + Environment.NewLine + StringResources.Language.ExceptionStackTrace + ex.StackTrace + Environment.NewLine + StringResources.Language.ExceptionTargetSite + ex.TargetSite;
	}

	public static string GetExceptionMessage(string extraMsg, Exception ex)
	{
		if (string.IsNullOrEmpty(extraMsg))
		{
			return GetExceptionMessage(ex);
		}
		return extraMsg + Environment.NewLine + GetExceptionMessage(ex);
	}

	public static string ByteToHexString(byte[] InBytes)
	{
		return ByteToHexString(InBytes, '\0');
	}

	public static string ByteToHexString(byte[] InBytes, char segment)
	{
		return ByteToHexString(InBytes, segment, 0);
	}

	public static string ByteToHexString(byte[] InBytes, char segment, int newLineCount, string format = "{0:X2}")
	{
		if (InBytes == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		long num = 0L;
		foreach (byte b in InBytes)
		{
			if (segment == '\0')
			{
				stringBuilder.Append(string.Format(format, b));
			}
			else
			{
				stringBuilder.Append(string.Format(format + "{1}", b, segment));
			}
			num++;
			if (newLineCount > 0 && num >= newLineCount)
			{
				stringBuilder.Append(Environment.NewLine);
				num = 0L;
			}
		}
		if (segment != 0 && stringBuilder.Length > 1 && stringBuilder[stringBuilder.Length - 1] == segment)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		return stringBuilder.ToString();
	}

	public static string ByteToHexString(string InString)
	{
		return ByteToHexString(Encoding.Unicode.GetBytes(InString));
	}

	private static int GetHexCharIndex(char ch)
	{
		switch (ch)
		{
		case '0':
			return 0;
		case '1':
			return 1;
		case '2':
			return 2;
		case '3':
			return 3;
		case '4':
			return 4;
		case '5':
			return 5;
		case '6':
			return 6;
		case '7':
			return 7;
		case '8':
			return 8;
		case '9':
			return 9;
		case 'A':
		case 'a':
			return 10;
		case 'B':
		case 'b':
			return 11;
		case 'C':
		case 'c':
			return 12;
		case 'D':
		case 'd':
			return 13;
		case 'E':
		case 'e':
			return 14;
		case 'F':
		case 'f':
			return 15;
		default:
			return -1;
		}
	}

	public static byte[] HexStringToBytes(string hex)
	{
		MemoryStream memoryStream = new MemoryStream();
		for (int i = 0; i < hex.Length; i++)
		{
			if (i + 1 < hex.Length && GetHexCharIndex(hex[i]) >= 0 && GetHexCharIndex(hex[i + 1]) >= 0)
			{
				memoryStream.WriteByte((byte)(GetHexCharIndex(hex[i]) * 16 + GetHexCharIndex(hex[i + 1])));
				i++;
			}
		}
		byte[] result = memoryStream.ToArray();
		memoryStream.Dispose();
		return result;
	}

	public static byte[] BytesReverseByWord(byte[] inBytes)
	{
		if (inBytes == null)
		{
			return null;
		}
		if (inBytes.Length == 0)
		{
			return new byte[0];
		}
		byte[] array = ArrayExpandToLengthEven(inBytes.CopyArray());
		for (int i = 0; i < array.Length / 2; i++)
		{
			byte b = array[i * 2];
			array[i * 2] = array[i * 2 + 1];
			array[i * 2 + 1] = b;
		}
		return array;
	}

	public static string GetAsciiStringRender(byte[] content)
	{
		if (content == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < content.Length; i++)
		{
			if (content[i] < 32 || content[i] > 126)
			{
				stringBuilder.Append($"\\{content[i]:X2}");
			}
			else
			{
				stringBuilder.Append((char)content[i]);
			}
		}
		return stringBuilder.ToString();
	}

	public static byte[] GetFromAsciiStringRender(string render)
	{
		if (string.IsNullOrEmpty(render))
		{
			return new byte[0];
		}
		MatchEvaluator evaluator = (Match m) => $"{(char)Convert.ToByte(m.Value.Substring(1), 16)}";
		return Encoding.ASCII.GetBytes(Regex.Replace(render.Replace("\\r", "\r").Replace("\\n", "\n"), "\\\\[0-9A-Fa-f]{2}", evaluator));
	}

	public static byte[] BytesToAsciiBytes(byte[] inBytes)
	{
		return Encoding.ASCII.GetBytes(ByteToHexString(inBytes));
	}

	public static byte[] AsciiBytesToBytes(byte[] inBytes)
	{
		return HexStringToBytes(Encoding.ASCII.GetString(inBytes));
	}

	public static byte[] BuildAsciiBytesFrom(byte value)
	{
		return Encoding.ASCII.GetBytes(value.ToString("X2"));
	}

	public static byte[] BuildAsciiBytesFrom(short value)
	{
		return Encoding.ASCII.GetBytes(value.ToString("X4"));
	}

	public static byte[] BuildAsciiBytesFrom(ushort value)
	{
		return Encoding.ASCII.GetBytes(value.ToString("X4"));
	}

	public static byte[] BuildAsciiBytesFrom(uint value)
	{
		return Encoding.ASCII.GetBytes(value.ToString("X8"));
	}

	public static byte[] BuildAsciiBytesFrom(byte[] value)
	{
		byte[] array = new byte[value.Length * 2];
		for (int i = 0; i < value.Length; i++)
		{
			BuildAsciiBytesFrom(value[i]).CopyTo(array, 2 * i);
		}
		return array;
	}

	private static byte GetDataByBitIndex(int offset)
	{
		if (1 == 0)
		{
		}
		byte result = offset switch
		{
			0 => 1, 
			1 => 2, 
			2 => 4, 
			3 => 8, 
			4 => 16, 
			5 => 32, 
			6 => 64, 
			7 => 128, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static bool BoolOnByteIndex(byte value, int offset)
	{
		byte dataByBitIndex = GetDataByBitIndex(offset);
		return (value & dataByBitIndex) == dataByBitIndex;
	}

	public static byte SetBoolOnByteIndex(byte byt, int offset, bool value)
	{
		byte dataByBitIndex = GetDataByBitIndex(offset);
		if (value)
		{
			return (byte)(byt | dataByBitIndex);
		}
		return (byte)(byt & ~dataByBitIndex);
	}

	public static byte[] BoolArrayToByte(bool[] array)
	{
		if (array == null)
		{
			return null;
		}
		int num = ((array.Length % 8 == 0) ? (array.Length / 8) : (array.Length / 8 + 1));
		byte[] array2 = new byte[num];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i])
			{
				array2[i / 8] += GetDataByBitIndex(i % 8);
			}
		}
		return array2;
	}

	public static string BoolArrayToString(bool[] array)
	{
		if (array == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i] ? "1" : "0");
		}
		return stringBuilder.ToString();
	}

	public static bool[] ByteToBoolArray(byte[] inBytes, int length)
	{
		if (inBytes == null)
		{
			return null;
		}
		if (length > inBytes.Length * 8)
		{
			length = inBytes.Length * 8;
		}
		bool[] array = new bool[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = BoolOnByteIndex(inBytes[i / 8], i % 8);
		}
		return array;
	}

	public static bool[] ByteToBoolArray(byte[] inBytes, int length, byte trueValue)
	{
		if (inBytes == null)
		{
			return null;
		}
		if (length > inBytes.Length)
		{
			length = inBytes.Length;
		}
		bool[] array = new bool[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = inBytes[i] == trueValue;
		}
		return array;
	}

	public static bool[] ByteToBoolArray(byte[] InBytes)
	{
		return (InBytes == null) ? null : ByteToBoolArray(InBytes, InBytes.Length * 8);
	}

	public static T[] ArrayRemoveDouble<T>(T[] value, int leftLength, int rightLength)
	{
		if (value == null)
		{
			return null;
		}
		if (value.Length <= leftLength + rightLength)
		{
			return new T[0];
		}
		T[] array = new T[value.Length - leftLength - rightLength];
		Array.Copy(value, leftLength, array, 0, array.Length);
		return array;
	}

	public static T[] ArrayRemoveBegin<T>(T[] value, int length)
	{
		return ArrayRemoveDouble(value, length, 0);
	}

	public static T[] ArrayRemoveLast<T>(T[] value, int length)
	{
		return ArrayRemoveDouble(value, 0, length);
	}

	public static T[] ArraySelectMiddle<T>(T[] value, int index, int length)
	{
		if (value == null)
		{
			return null;
		}
		if (length == 0)
		{
			return new T[0];
		}
		T[] array = new T[Math.Min(value.Length, length)];
		Array.Copy(value, index, array, 0, array.Length);
		return array;
	}

	public static T[] ArraySelectBegin<T>(T[] value, int length)
	{
		if (length == 0)
		{
			return new T[0];
		}
		T[] array = new T[Math.Min(value.Length, length)];
		if (array.Length != 0)
		{
			Array.Copy(value, 0, array, 0, array.Length);
		}
		return array;
	}

	public static T[] ArraySelectLast<T>(T[] value, int length)
	{
		if (length == 0)
		{
			return new T[0];
		}
		T[] array = new T[Math.Min(value.Length, length)];
		Array.Copy(value, value.Length - length, array, 0, array.Length);
		return array;
	}

	public static T[] SpliceArray<T>(params T[][] arrays)
	{
		int num = 0;
		for (int i = 0; i < arrays.Length; i++)
		{
			T[] array = arrays[i];
			if (array != null && array.Length != 0)
			{
				num += arrays[i].Length;
			}
		}
		int num2 = 0;
		T[] array2 = new T[num];
		for (int j = 0; j < arrays.Length; j++)
		{
			T[] array3 = arrays[j];
			if (array3 != null && array3.Length != 0)
			{
				arrays[j].CopyTo(array2, num2);
				num2 += arrays[j].Length;
			}
		}
		return array2;
	}

	public static string[] SpliceStringArray(string first, string[] array)
	{
		List<string> list = new List<string>();
		list.Add(first);
		list.AddRange(array);
		return list.ToArray();
	}

	public static string[] SpliceStringArray(string first, string second, string[] array)
	{
		List<string> list = new List<string>();
		list.Add(first);
		list.Add(second);
		list.AddRange(array);
		return list.ToArray();
	}

	public static string[] SpliceStringArray(string first, string second, string third, string[] array)
	{
		List<string> list = new List<string>();
		list.Add(first);
		list.Add(second);
		list.Add(third);
		list.AddRange(array);
		return list.ToArray();
	}

	private static int HexToInt(char h)
	{
		return (h >= '0' && h <= '9') ? (h - 48) : ((h >= 'a' && h <= 'f') ? (h - 97 + 10) : ((h >= 'A' && h <= 'F') ? (h - 65 + 10) : (-1)));
	}

	private static string ValidateString(string input, bool skipUtf16Validation)
	{
		if (skipUtf16Validation || string.IsNullOrEmpty(input))
		{
			return input;
		}
		int num = -1;
		for (int i = 0; i < input.Length; i++)
		{
			if (char.IsSurrogate(input[i]))
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return input;
		}
		char[] array = input.ToCharArray();
		for (int j = num; j < array.Length; j++)
		{
			char c = array[j];
			if (char.IsLowSurrogate(c))
			{
				array[j] = '\ufffd';
			}
			else if (char.IsHighSurrogate(c))
			{
				if (j + 1 < array.Length && char.IsLowSurrogate(array[j + 1]))
				{
					j++;
				}
				else
				{
					array[j] = '\ufffd';
				}
			}
		}
		return new string(array);
	}

	private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
	{
		if (bytes == null && count == 0)
		{
			return false;
		}
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		if (offset < 0 || offset > bytes.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || offset + count > bytes.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		return true;
	}

	private static bool IsUrlSafeChar(char ch)
	{
		if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
		{
			return true;
		}
		switch (ch)
		{
		case '!':
		case '(':
		case ')':
		case '*':
		case '-':
		case '.':
		case '_':
			return true;
		default:
			return false;
		}
	}

	private static string UrlEncodeSpaces(string str)
	{
		if (str != null && str.IndexOf(' ') >= 0)
		{
			str = str.Replace(" ", "%20");
		}
		return str;
	}

	private static char IntToHex(int n)
	{
		Debug.Assert(n < 16);
		if (n <= 9)
		{
			return (char)(n + 48);
		}
		return (char)(n - 10 + 65);
	}

	private static byte[] UrlEncodeToBytes(byte[] bytes)
	{
		int num = 0;
		int num2 = bytes.Length;
		if (!ValidateUrlEncodingParameters(bytes, num, num2))
		{
			return null;
		}
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < num2; i++)
		{
			char c = (char)bytes[num + i];
			if (c == ' ')
			{
				num3++;
			}
			else if (!IsUrlSafeChar(c))
			{
				num4++;
			}
		}
		if (num3 == 0 && num4 == 0)
		{
			if (num == 0 && bytes.Length == num2)
			{
				return bytes;
			}
			byte[] array = new byte[num2];
			Buffer.BlockCopy(bytes, num, array, 0, num2);
			return array;
		}
		byte[] array2 = new byte[num2 + num4 * 2];
		int num5 = 0;
		for (int j = 0; j < num2; j++)
		{
			byte b = bytes[num + j];
			char c2 = (char)b;
			if (IsUrlSafeChar(c2))
			{
				array2[num5++] = b;
				continue;
			}
			if (c2 == ' ')
			{
				array2[num5++] = 43;
				continue;
			}
			array2[num5++] = 37;
			array2[num5++] = (byte)IntToHex((b >> 4) & 0xF);
			array2[num5++] = (byte)IntToHex(b & 0xF);
		}
		return array2;
	}

	private static byte[] UrlEncode(byte[] bytes, bool alwaysCreateNewReturnValue)
	{
		byte[] array = UrlEncodeToBytes(bytes);
		return (alwaysCreateNewReturnValue && array != null && array == bytes) ? ((byte[])array.Clone()) : array;
	}

	public static string UrlEncode(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		byte[] bytes = e.GetBytes(str);
		return Encoding.ASCII.GetString(UrlEncode(bytes, alwaysCreateNewReturnValue: true));
	}

	public static string UrlDecode(string value, Encoding encoding)
	{
		int length = value.Length;
		UrlDecoder urlDecoder = new UrlDecoder(length, encoding);
		for (int i = 0; i < length; i++)
		{
			char c = value[i];
			switch (c)
			{
			case '+':
				c = ' ';
				break;
			case '%':
				if (i >= length - 2)
				{
					break;
				}
				if (value[i + 1] == 'u' && i < length - 5)
				{
					int num = HexToInt(value[i + 2]);
					int num2 = HexToInt(value[i + 3]);
					int num3 = HexToInt(value[i + 4]);
					int num4 = HexToInt(value[i + 5]);
					if (num >= 0 && num2 >= 0 && num3 >= 0 && num4 >= 0)
					{
						c = (char)((num << 12) | (num2 << 8) | (num3 << 4) | num4);
						i += 5;
						urlDecoder.AddChar(c);
						continue;
					}
				}
				else
				{
					int num5 = HexToInt(value[i + 1]);
					int num6 = HexToInt(value[i + 2]);
					if (num5 >= 0 && num6 >= 0)
					{
						byte b = (byte)((num5 << 4) | num6);
						i += 2;
						urlDecoder.AddByte(b);
						continue;
					}
				}
				break;
			}
			if ((c & 0xFF80) == 0)
			{
				urlDecoder.AddByte((byte)c);
			}
			else
			{
				urlDecoder.AddChar(c);
			}
		}
		return ValidateString(urlDecoder.GetString(), skipUtf16Validation: true);
	}

	public static object DeepClone(object oringinal)
	{
		using MemoryStream memoryStream = new MemoryStream();
		BinaryFormatter binaryFormatter = new BinaryFormatter
		{
			Context = new StreamingContext(StreamingContextStates.Clone)
		};
		binaryFormatter.Serialize(memoryStream, oringinal);
		memoryStream.Position = 0L;
		return binaryFormatter.Deserialize(memoryStream);
	}

	public static string GetUniqueStringByGuidAndRandom()
	{
		return Guid.NewGuid().ToString("N") + HslHelper.HslRandom.Next(1000, 10000);
	}
}
