using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HslCommunication;

public static class HslExtension
{
	public static string ToHexString(this byte[] InBytes)
	{
		return SoftBasic.ByteToHexString(InBytes);
	}

	public static string ToHexString(this byte[] InBytes, char segment)
	{
		return SoftBasic.ByteToHexString(InBytes, segment);
	}

	public static string ToHexString(this byte[] InBytes, char segment, int newLineCount, string format = "{0:X2}")
	{
		return SoftBasic.ByteToHexString(InBytes, segment, newLineCount, format);
	}

	public static byte[] ToHexBytes(this string value)
	{
		return SoftBasic.HexStringToBytes(value);
	}

	public static byte[] ToByteArray(this bool[] array)
	{
		return SoftBasic.BoolArrayToByte(array);
	}

	public static bool[] ToBoolArray(this byte[] InBytes, int length)
	{
		return SoftBasic.ByteToBoolArray(InBytes, length);
	}

	public static T[] ReverseNew<T>(this T[] value)
	{
		T[] array = value.CopyArray();
		Array.Reverse(array);
		return array;
	}

	public static bool[] ToBoolArray(this byte[] InBytes)
	{
		return SoftBasic.ByteToBoolArray(InBytes);
	}

	public static bool GetBoolValue(this byte[] bytes, int bytIndex, int boolIndex)
	{
		return SoftBasic.BoolOnByteIndex(bytes[bytIndex], boolIndex);
	}

	public static bool GetBoolByIndex(this byte[] bytes, int boolIndex)
	{
		return SoftBasic.BoolOnByteIndex(bytes[boolIndex / 8], boolIndex % 8);
	}

	public static bool GetBoolByIndex(this byte byt, int boolIndex)
	{
		return SoftBasic.BoolOnByteIndex(byt, boolIndex % 8);
	}

	public static bool GetBoolByIndex(this short value, int boolIndex)
	{
		return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
	}

	public static bool GetBoolByIndex(this ushort value, int boolIndex)
	{
		return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
	}

	public static bool GetBoolByIndex(this int value, int boolIndex)
	{
		return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
	}

	public static bool GetBoolByIndex(this uint value, int boolIndex)
	{
		return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
	}

	public static bool GetBoolByIndex(this long value, int boolIndex)
	{
		return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
	}

	public static bool GetBoolByIndex(this ulong value, int boolIndex)
	{
		return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
	}

	public static string GetStringOrEndChar(this byte[] buffer, int index, int length, Encoding encoding)
	{
		for (int i = index; i < index + length; i++)
		{
			if (buffer[i] == 0)
			{
				length = i - index;
				break;
			}
		}
		return Encoding.UTF8.GetString(buffer, index, length);
	}

	public static byte SetBoolByIndex(this byte byt, int boolIndex, bool value)
	{
		return SoftBasic.SetBoolOnByteIndex(byt, boolIndex, value);
	}

	public static void SetBoolByIndex(this byte[] buffer, int boolIndex, bool value)
	{
		buffer[boolIndex / 8] = buffer[boolIndex / 8].SetBoolByIndex(boolIndex % 8, value);
	}

	public static short SetBoolByIndex(this short shortValue, int boolIndex, bool value)
	{
		byte[] bytes = BitConverter.GetBytes(shortValue);
		bytes.SetBoolByIndex(boolIndex, value);
		return BitConverter.ToInt16(bytes, 0);
	}

	public static ushort SetBoolByIndex(this ushort ushortValue, int boolIndex, bool value)
	{
		byte[] bytes = BitConverter.GetBytes(ushortValue);
		bytes.SetBoolByIndex(boolIndex, value);
		return BitConverter.ToUInt16(bytes, 0);
	}

	public static int SetBoolByIndex(this int intValue, int boolIndex, bool value)
	{
		byte[] bytes = BitConverter.GetBytes(intValue);
		bytes.SetBoolByIndex(boolIndex, value);
		return BitConverter.ToInt32(bytes, 0);
	}

	public static uint SetBoolByIndex(this uint uintValue, int boolIndex, bool value)
	{
		byte[] bytes = BitConverter.GetBytes(uintValue);
		bytes.SetBoolByIndex(boolIndex, value);
		return BitConverter.ToUInt32(bytes, 0);
	}

	public static long SetBoolByIndex(this long longValue, int boolIndex, bool value)
	{
		byte[] bytes = BitConverter.GetBytes(longValue);
		bytes.SetBoolByIndex(boolIndex, value);
		return BitConverter.ToInt64(bytes, 0);
	}

	public static ulong SetBoolByIndex(this ulong ulongValue, int boolIndex, bool value)
	{
		byte[] bytes = BitConverter.GetBytes(ulongValue);
		bytes.SetBoolByIndex(boolIndex, value);
		return BitConverter.ToUInt64(bytes, 0);
	}

	public static T[] RemoveDouble<T>(this T[] value, int leftLength, int rightLength)
	{
		return SoftBasic.ArrayRemoveDouble(value, leftLength, rightLength);
	}

	public static T[] RemoveBegin<T>(this T[] value, int length)
	{
		return SoftBasic.ArrayRemoveBegin(value, length);
	}

	public static T[] RemoveLast<T>(this T[] value, int length)
	{
		return SoftBasic.ArrayRemoveLast(value, length);
	}

	public static T[] SelectMiddle<T>(this T[] value, int index, int length)
	{
		return SoftBasic.ArraySelectMiddle(value, index, length);
	}

	public static T[] SelectBegin<T>(this T[] value, int length)
	{
		return SoftBasic.ArraySelectBegin(value, length);
	}

	public static T[] SelectLast<T>(this T[] value, int length)
	{
		return SoftBasic.ArraySelectLast(value, length);
	}

	public static T GetValueOrDefault<T>(JObject jObject, string name, T defaultValue)
	{
		return SoftBasic.GetValueFromJsonObject(jObject, name, defaultValue);
	}

	public static T[] SpliceArray<T>(this T[] value, params T[][] arrays)
	{
		List<T[]> list = new List<T[]>(arrays.Length + 1);
		list.Add(value);
		list.AddRange(arrays);
		return SoftBasic.SpliceArray(list.ToArray());
	}

	public static string RemoveLast(this string value, int length)
	{
		if (value == null)
		{
			return null;
		}
		if (value.Length < length)
		{
			return string.Empty;
		}
		return value.Remove(value.Length - length);
	}

	public static byte[] EveryByteAdd(this byte[] array, int value)
	{
		if (array == null)
		{
			return null;
		}
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)(array[i] + value);
		}
		return array;
	}

	public static T[] IncreaseBy<T>(this T[] array, T value)
	{
		if (typeof(T) == typeof(byte))
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(int), "first");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(int), "second");
			Expression body = Expression.Add(parameterExpression, parameterExpression2);
			Expression<Func<int, int, int>> expression = Expression.Lambda<Func<int, int, int>>(body, new ParameterExpression[2] { parameterExpression, parameterExpression2 });
			Func<int, int, int> func = expression.Compile();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (T)(object)(byte)func(Convert.ToInt32(array[i]), Convert.ToInt32(value));
			}
		}
		else
		{
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(T), "first");
			ParameterExpression parameterExpression4 = Expression.Parameter(typeof(T), "second");
			Expression body2 = Expression.Add(parameterExpression3, parameterExpression4);
			Expression<Func<T, T, T>> expression2 = Expression.Lambda<Func<T, T, T>>(body2, new ParameterExpression[2] { parameterExpression3, parameterExpression4 });
			Func<T, T, T> func2 = expression2.Compile();
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = func2(array[j], value);
			}
		}
		return array;
	}

	public static T[] CopyArray<T>(this T[] value)
	{
		if (value == null)
		{
			return null;
		}
		T[] array = new T[value.Length];
		Array.Copy(value, array, value.Length);
		return array;
	}

	public static string ToArrayString<T>(this T[] value)
	{
		return SoftBasic.ArrayFormat(value);
	}

	public static string ToArrayString<T>(this T[] value, string format)
	{
		return SoftBasic.ArrayFormat(value, format);
	}

	public static T[] ToStringArray<T>(this string value, Func<string, T> selector)
	{
		if (value.IndexOf('[') >= 0)
		{
			value = value.Replace("[", "");
		}
		if (value.IndexOf(']') >= 0)
		{
			value = value.Replace("]", "");
		}
		string[] source = value.Split(new char[2] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
		return source.Select(selector).ToArray();
	}

	public static T[] ToStringArray<T>(this string value)
	{
		Type typeFromHandle = typeof(T);
		if (typeFromHandle == typeof(byte))
		{
			return (T[])(object)value.ToStringArray(byte.Parse);
		}
		if (typeFromHandle == typeof(sbyte))
		{
			return (T[])(object)value.ToStringArray(sbyte.Parse);
		}
		if (typeFromHandle == typeof(bool))
		{
			return (T[])(object)value.ToStringArray(bool.Parse);
		}
		if (typeFromHandle == typeof(short))
		{
			return (T[])(object)value.ToStringArray(short.Parse);
		}
		if (typeFromHandle == typeof(ushort))
		{
			return (T[])(object)value.ToStringArray(ushort.Parse);
		}
		if (typeFromHandle == typeof(int))
		{
			return (T[])(object)value.ToStringArray(int.Parse);
		}
		if (typeFromHandle == typeof(uint))
		{
			return (T[])(object)value.ToStringArray(uint.Parse);
		}
		if (typeFromHandle == typeof(long))
		{
			return (T[])(object)value.ToStringArray(long.Parse);
		}
		if (typeFromHandle == typeof(ulong))
		{
			return (T[])(object)value.ToStringArray(ulong.Parse);
		}
		if (typeFromHandle == typeof(float))
		{
			return (T[])(object)value.ToStringArray(float.Parse);
		}
		if (typeFromHandle == typeof(double))
		{
			return (T[])(object)value.ToStringArray(double.Parse);
		}
		if (typeFromHandle == typeof(DateTime))
		{
			return (T[])(object)value.ToStringArray(DateTime.Parse);
		}
		if (typeFromHandle == typeof(Guid))
		{
			return (T[])(object)value.ToStringArray(Guid.Parse);
		}
		if (typeFromHandle == typeof(string))
		{
			return (T[])(object)value.ToStringArray((string m) => m);
		}
		throw new Exception("use ToArray<T>(Func<string,T>) method instead");
	}

	public static OperateResult BeginReceiveResult(this Socket socket, AsyncCallback callback, object obj)
	{
		try
		{
			socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, callback, obj);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult(ex.Message);
		}
	}

	public static OperateResult BeginReceiveResult(this Socket socket, AsyncCallback callback)
	{
		return socket.BeginReceiveResult(callback, socket);
	}

	public static OperateResult<int> EndReceiveResult(this Socket socket, IAsyncResult ar)
	{
		try
		{
			return OperateResult.CreateSuccessResult(socket.EndReceive(ar));
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<int>(ex.Message);
		}
	}

	public static string[] SplitDot(this string str)
	{
		return str.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
	}

	public static string ToJsonString(this object obj, Formatting formatting = Formatting.Indented)
	{
		return JsonConvert.SerializeObject(obj, formatting);
	}

	public static byte[] GetPEMPrivateKey(this RSACryptoServiceProvider rsa)
	{
		return RSAHelper.GetPrivateKeyFromRSA(rsa);
	}

	public static byte[] GetPEMPublicKey(this RSACryptoServiceProvider rsa)
	{
		return RSAHelper.GetPublicKeyFromRSA(rsa);
	}

	public static byte[] EncryptLargeData(this RSACryptoServiceProvider rsa, byte[] data)
	{
		return RSAHelper.EncryptLargeDataByRSA(rsa, data);
	}

	public static byte[] DecryptLargeData(this RSACryptoServiceProvider rsa, byte[] data)
	{
		return RSAHelper.DecryptLargeDataByRSA(rsa, data);
	}

	public static void Write(this MemoryStream ms, byte[] buffer)
	{
		if (buffer != null)
		{
			ms.Write(buffer, 0, buffer.Length);
		}
	}

	public static void WriteReverse(this MemoryStream ms, ushort value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		byte b = bytes[0];
		bytes[0] = bytes[1];
		bytes[1] = b;
		ms.Write(bytes);
	}

	public static int SetKeepAlive(this Socket socket, int keepAliveTime, int keepAliveInterval)
	{
		byte[] array = new byte[12];
		BitConverter.GetBytes((keepAliveTime >= 0) ? 1 : 0).CopyTo(array, 0);
		BitConverter.GetBytes(keepAliveTime).CopyTo(array, 4);
		BitConverter.GetBytes(keepAliveInterval).CopyTo(array, 8);
		try
		{
			return socket.IOControl(IOControlCode.KeepAliveValues, array, null);
		}
		catch
		{
			return 0;
		}
	}

	public static void IniSerialByFormatString(this SerialPort serialPort, string format)
	{
		string[] array = format.Split(new char[2] { '-', ';' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 0)
		{
			return;
		}
		int num = 0;
		if (!Regex.IsMatch(array[0], "^[0-9]+$"))
		{
			serialPort.PortName = array[0];
			num = 1;
		}
		if (num < array.Length)
		{
			serialPort.BaudRate = Convert.ToInt32(array[num++]);
		}
		if (num < array.Length)
		{
			serialPort.DataBits = Convert.ToInt32(array[num++]);
		}
		if (num < array.Length)
		{
			string text = array[num++].ToUpper();
			if (1 == 0)
			{
			}
			Parity parity = text switch
			{
				"E" => Parity.Even, 
				"O" => Parity.Odd, 
				"N" => Parity.None, 
				_ => Parity.Space, 
			};
			if (1 == 0)
			{
			}
			serialPort.Parity = parity;
		}
		if (num < array.Length)
		{
			string text2 = array[num++];
			if (1 == 0)
			{
			}
			StopBits stopBits = text2 switch
			{
				"0" => StopBits.None, 
				"2" => StopBits.Two, 
				"1" => StopBits.One, 
				_ => StopBits.OnePointFive, 
			};
			if (1 == 0)
			{
			}
			serialPort.StopBits = stopBits;
		}
	}

	public static string ToFormatString(this SerialPort serialPort)
	{
		return HslHelper.ToFormatString(serialPort.PortName, serialPort.BaudRate, serialPort.DataBits, serialPort.Parity, serialPort.StopBits);
	}

	public static byte[] GetBytes(this Random random, int length)
	{
		byte[] array = new byte[length];
		random.NextBytes(array);
		return array;
	}

	public static byte[] ReverseByWord(this byte[] inBytes)
	{
		return SoftBasic.BytesReverseByWord(inBytes);
	}

	public static bool StartsWithAndNumber(this string address, string code)
	{
		if (address.StartsWith(code, StringComparison.OrdinalIgnoreCase))
		{
			if (address.Length <= code.Length)
			{
				return false;
			}
			if (char.IsNumber(address[code.Length]))
			{
				return true;
			}
			if (address.Length > code.Length + 1 && address[code.Length] == '-' && char.IsNumber(address[code.Length + 1]))
			{
				return true;
			}
		}
		return false;
	}

	public static bool StartsWithAndNumber(this string address, string[] code)
	{
		if (code == null)
		{
			return false;
		}
		for (int i = 0; i < code.Length; i++)
		{
			if (address.StartsWithAndNumber(code[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static bool StartsWith(this string address, string[] code)
	{
		if (code == null)
		{
			return false;
		}
		for (int i = 0; i < code.Length; i++)
		{
			if (address.StartsWith(code[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static bool EndsWith(this string str, string[] value)
	{
		if (value == null)
		{
			return false;
		}
		for (int i = 0; i < value.Length; i++)
		{
			if (str.EndsWith(value[i], StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Contains(this string str, string[] value)
	{
		if (value == null)
		{
			return false;
		}
		for (int i = 0; i < value.Length; i++)
		{
			if (str.Contains(value[i]))
			{
				return true;
			}
		}
		return false;
	}
}
