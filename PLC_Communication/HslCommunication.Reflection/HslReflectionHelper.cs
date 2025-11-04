using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Enthernet.Redis;
using Newtonsoft.Json.Linq;

namespace HslCommunication.Reflection;

public class HslReflectionHelper
{
	public static HslDeviceAddressAttribute GetHslDeviceAddressAttribute(Type deviceType, PropertyInfo property)
	{
		object[] customAttributes = property.GetCustomAttributes(typeof(HslDeviceAddressAttribute), inherit: false);
		if (customAttributes == null)
		{
			return null;
		}
		HslDeviceAddressAttribute hslDeviceAddressAttribute = null;
		for (int i = 0; i < customAttributes.Length; i++)
		{
			HslDeviceAddressAttribute hslDeviceAddressAttribute2 = (HslDeviceAddressAttribute)customAttributes[i];
			if (hslDeviceAddressAttribute2.DeviceType != null && hslDeviceAddressAttribute2.DeviceType == deviceType)
			{
				hslDeviceAddressAttribute = hslDeviceAddressAttribute2;
				break;
			}
		}
		if (hslDeviceAddressAttribute == null)
		{
			for (int j = 0; j < customAttributes.Length; j++)
			{
				HslDeviceAddressAttribute hslDeviceAddressAttribute3 = (HslDeviceAddressAttribute)customAttributes[j];
				if (hslDeviceAddressAttribute3.DeviceType == null)
				{
					hslDeviceAddressAttribute = hslDeviceAddressAttribute3;
					break;
				}
			}
		}
		return hslDeviceAddressAttribute;
	}

	public static HslDeviceAddressAttribute[] GetHslDeviceAddressAttributeArray(Type deviceType, PropertyInfo property)
	{
		object[] customAttributes = property.GetCustomAttributes(typeof(HslDeviceAddressAttribute), inherit: false);
		if (customAttributes == null)
		{
			return null;
		}
		List<HslDeviceAddressAttribute> list = new List<HslDeviceAddressAttribute>();
		for (int i = 0; i < customAttributes.Length; i++)
		{
			HslDeviceAddressAttribute hslDeviceAddressAttribute = (HslDeviceAddressAttribute)customAttributes[i];
			if (hslDeviceAddressAttribute.DeviceType != null && hslDeviceAddressAttribute.DeviceType == deviceType)
			{
				list.Add(hslDeviceAddressAttribute);
			}
		}
		if (list.Count == 0)
		{
			for (int j = 0; j < customAttributes.Length; j++)
			{
				HslDeviceAddressAttribute hslDeviceAddressAttribute2 = (HslDeviceAddressAttribute)customAttributes[j];
				if (hslDeviceAddressAttribute2.DeviceType == null)
				{
					list.Add(hslDeviceAddressAttribute2);
				}
			}
		}
		return list.ToArray();
	}

	public static OperateResult<T> Read<T>(IReadWriteNet readWrite) where T : class, new()
	{
		Type typeFromHandle = typeof(T);
		object obj = typeFromHandle.Assembly.CreateInstance(typeFromHandle.FullName);
		PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo propertyInfo in array2)
		{
			Type propertyType = propertyInfo.PropertyType;
			if (propertyType == typeof(string[]))
			{
				HslDeviceAddressAttribute[] hslDeviceAddressAttributeArray = GetHslDeviceAddressAttributeArray(readWrite.GetType(), propertyInfo);
				if (hslDeviceAddressAttributeArray == null || hslDeviceAddressAttributeArray.Length == 0)
				{
					continue;
				}
				string[] array3 = new string[hslDeviceAddressAttributeArray.Length];
				for (int j = 0; j < hslDeviceAddressAttributeArray.Length; j++)
				{
					OperateResult<string> operateResult = readWrite.ReadString(hslDeviceAddressAttributeArray[j].Address, (ushort)((hslDeviceAddressAttributeArray[j].Length < 0) ? 1u : ((uint)hslDeviceAddressAttributeArray[j].Length)), hslDeviceAddressAttributeArray[j].GetEncoding());
					if (!operateResult.IsSuccess)
					{
						return OperateResult.CreateFailedResult<T>(operateResult);
					}
					array3[j] = operateResult.Content;
				}
				propertyInfo.SetValue(obj, array3, null);
				continue;
			}
			HslDeviceAddressAttribute hslDeviceAddressAttribute = GetHslDeviceAddressAttribute(readWrite.GetType(), propertyInfo);
			if (hslDeviceAddressAttribute == null)
			{
				continue;
			}
			if (propertyType == typeof(byte))
			{
				MethodInfo method = readWrite.GetType().GetMethod("ReadByte", new Type[1] { typeof(string) });
				if (method == null)
				{
					return new OperateResult<T>(readWrite.GetType().Name + " not support read byte value. ");
				}
				OperateResult<byte> operateResult2 = (OperateResult<byte>)method.Invoke(readWrite, new object[1] { hslDeviceAddressAttribute.Address });
				if (!operateResult2.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult2);
				}
				propertyInfo.SetValue(obj, operateResult2.Content, null);
			}
			else if (propertyType == typeof(short))
			{
				OperateResult<short> operateResult3 = readWrite.ReadInt16(hslDeviceAddressAttribute.Address);
				if (!operateResult3.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult3);
				}
				propertyInfo.SetValue(obj, operateResult3.Content, null);
			}
			else if (propertyType == typeof(short[]))
			{
				OperateResult<short[]> operateResult4 = readWrite.ReadInt16(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult4.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult4);
				}
				propertyInfo.SetValue(obj, operateResult4.Content, null);
			}
			else if (propertyType == typeof(ushort))
			{
				OperateResult<ushort> operateResult5 = readWrite.ReadUInt16(hslDeviceAddressAttribute.Address);
				if (!operateResult5.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult5);
				}
				propertyInfo.SetValue(obj, operateResult5.Content, null);
			}
			else if (propertyType == typeof(ushort[]))
			{
				OperateResult<ushort[]> operateResult6 = readWrite.ReadUInt16(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult6.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult6);
				}
				propertyInfo.SetValue(obj, operateResult6.Content, null);
			}
			else if (propertyType == typeof(int))
			{
				OperateResult<int> operateResult7 = readWrite.ReadInt32(hslDeviceAddressAttribute.Address);
				if (!operateResult7.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult7);
				}
				propertyInfo.SetValue(obj, operateResult7.Content, null);
			}
			else if (propertyType == typeof(int[]))
			{
				OperateResult<int[]> operateResult8 = readWrite.ReadInt32(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult8.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult8);
				}
				propertyInfo.SetValue(obj, operateResult8.Content, null);
			}
			else if (propertyType == typeof(uint))
			{
				OperateResult<uint> operateResult9 = readWrite.ReadUInt32(hslDeviceAddressAttribute.Address);
				if (!operateResult9.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult9);
				}
				propertyInfo.SetValue(obj, operateResult9.Content, null);
			}
			else if (propertyType == typeof(uint[]))
			{
				OperateResult<uint[]> operateResult10 = readWrite.ReadUInt32(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult10.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult10);
				}
				propertyInfo.SetValue(obj, operateResult10.Content, null);
			}
			else if (propertyType == typeof(long))
			{
				OperateResult<long> operateResult11 = readWrite.ReadInt64(hslDeviceAddressAttribute.Address);
				if (!operateResult11.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult11);
				}
				propertyInfo.SetValue(obj, operateResult11.Content, null);
			}
			else if (propertyType == typeof(long[]))
			{
				OperateResult<long[]> operateResult12 = readWrite.ReadInt64(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult12.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult12);
				}
				propertyInfo.SetValue(obj, operateResult12.Content, null);
			}
			else if (propertyType == typeof(ulong))
			{
				OperateResult<ulong> operateResult13 = readWrite.ReadUInt64(hslDeviceAddressAttribute.Address);
				if (!operateResult13.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult13);
				}
				propertyInfo.SetValue(obj, operateResult13.Content, null);
			}
			else if (propertyType == typeof(ulong[]))
			{
				OperateResult<ulong[]> operateResult14 = readWrite.ReadUInt64(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult14.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult14);
				}
				propertyInfo.SetValue(obj, operateResult14.Content, null);
			}
			else if (propertyType == typeof(float))
			{
				OperateResult<float> operateResult15 = readWrite.ReadFloat(hslDeviceAddressAttribute.Address);
				if (!operateResult15.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult15);
				}
				propertyInfo.SetValue(obj, operateResult15.Content, null);
			}
			else if (propertyType == typeof(float[]))
			{
				OperateResult<float[]> operateResult16 = readWrite.ReadFloat(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult16.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult16);
				}
				propertyInfo.SetValue(obj, operateResult16.Content, null);
			}
			else if (propertyType == typeof(double))
			{
				OperateResult<double> operateResult17 = readWrite.ReadDouble(hslDeviceAddressAttribute.Address);
				if (!operateResult17.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult17);
				}
				propertyInfo.SetValue(obj, operateResult17.Content, null);
			}
			else if (propertyType == typeof(double[]))
			{
				OperateResult<double[]> operateResult18 = readWrite.ReadDouble(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult18.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult18);
				}
				propertyInfo.SetValue(obj, operateResult18.Content, null);
			}
			else if (propertyType == typeof(string))
			{
				OperateResult<string> operateResult19 = readWrite.ReadString(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)), hslDeviceAddressAttribute.GetEncoding());
				if (!operateResult19.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult19);
				}
				propertyInfo.SetValue(obj, operateResult19.Content, null);
			}
			else if (propertyType == typeof(byte[]))
			{
				OperateResult<byte[]> operateResult20 = readWrite.Read(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult20.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult20);
				}
				propertyInfo.SetValue(obj, operateResult20.Content, null);
			}
			else if (propertyType == typeof(bool))
			{
				OperateResult<bool> operateResult21 = readWrite.ReadBool(hslDeviceAddressAttribute.Address);
				if (!operateResult21.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult21);
				}
				propertyInfo.SetValue(obj, operateResult21.Content, null);
			}
			else if (propertyType == typeof(bool[]))
			{
				OperateResult<bool[]> operateResult22 = readWrite.ReadBool(hslDeviceAddressAttribute.Address, (ushort)((hslDeviceAddressAttribute.Length < 0) ? 1u : ((uint)hslDeviceAddressAttribute.Length)));
				if (!operateResult22.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult22);
				}
				propertyInfo.SetValue(obj, operateResult22.Content, null);
			}
		}
		return OperateResult.CreateSuccessResult((T)obj);
	}

	public static OperateResult Write<T>(T data, IReadWriteNet readWrite) where T : class, new()
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		Type typeFromHandle = typeof(T);
		PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo propertyInfo in array2)
		{
			Type propertyType = propertyInfo.PropertyType;
			if (propertyType == typeof(string[]))
			{
				HslDeviceAddressAttribute[] hslDeviceAddressAttributeArray = GetHslDeviceAddressAttributeArray(readWrite.GetType(), propertyInfo);
				if (hslDeviceAddressAttributeArray == null || hslDeviceAddressAttributeArray.Length == 0)
				{
					continue;
				}
				string[] array3 = (string[])propertyInfo.GetValue(data, null);
				for (int j = 0; j < hslDeviceAddressAttributeArray.Length; j++)
				{
					OperateResult operateResult = readWrite.Write(hslDeviceAddressAttributeArray[j].Address, array3[j], hslDeviceAddressAttributeArray[j].GetEncoding());
					if (!operateResult.IsSuccess)
					{
						return operateResult;
					}
				}
				continue;
			}
			HslDeviceAddressAttribute hslDeviceAddressAttribute = GetHslDeviceAddressAttribute(readWrite.GetType(), propertyInfo);
			if (hslDeviceAddressAttribute == null)
			{
				continue;
			}
			if (propertyType == typeof(byte))
			{
				MethodInfo method = readWrite.GetType().GetMethod("Write", new Type[2]
				{
					typeof(string),
					typeof(byte)
				});
				if (method == null)
				{
					return new OperateResult<T>(readWrite.GetType().Name + " not support write byte value. ");
				}
				byte b = (byte)propertyInfo.GetValue(data, null);
				OperateResult operateResult2 = (OperateResult)method.Invoke(readWrite, new object[2] { hslDeviceAddressAttribute.Address, b });
				if (!operateResult2.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult2);
				}
			}
			else if (propertyType == typeof(short))
			{
				short value = (short)propertyInfo.GetValue(data, null);
				OperateResult operateResult3 = readWrite.Write(hslDeviceAddressAttribute.Address, value);
				if (!operateResult3.IsSuccess)
				{
					return operateResult3;
				}
			}
			else if (propertyType == typeof(short[]))
			{
				short[] values = (short[])propertyInfo.GetValue(data, null);
				OperateResult operateResult4 = readWrite.Write(hslDeviceAddressAttribute.Address, values);
				if (!operateResult4.IsSuccess)
				{
					return operateResult4;
				}
			}
			else if (propertyType == typeof(ushort))
			{
				ushort value2 = (ushort)propertyInfo.GetValue(data, null);
				OperateResult operateResult5 = readWrite.Write(hslDeviceAddressAttribute.Address, value2);
				if (!operateResult5.IsSuccess)
				{
					return operateResult5;
				}
			}
			else if (propertyType == typeof(ushort[]))
			{
				ushort[] values2 = (ushort[])propertyInfo.GetValue(data, null);
				OperateResult operateResult6 = readWrite.Write(hslDeviceAddressAttribute.Address, values2);
				if (!operateResult6.IsSuccess)
				{
					return operateResult6;
				}
			}
			else if (propertyType == typeof(int))
			{
				int value3 = (int)propertyInfo.GetValue(data, null);
				OperateResult operateResult7 = readWrite.Write(hslDeviceAddressAttribute.Address, value3);
				if (!operateResult7.IsSuccess)
				{
					return operateResult7;
				}
			}
			else if (propertyType == typeof(int[]))
			{
				int[] values3 = (int[])propertyInfo.GetValue(data, null);
				OperateResult operateResult8 = readWrite.Write(hslDeviceAddressAttribute.Address, values3);
				if (!operateResult8.IsSuccess)
				{
					return operateResult8;
				}
			}
			else if (propertyType == typeof(uint))
			{
				uint value4 = (uint)propertyInfo.GetValue(data, null);
				OperateResult operateResult9 = readWrite.Write(hslDeviceAddressAttribute.Address, value4);
				if (!operateResult9.IsSuccess)
				{
					return operateResult9;
				}
			}
			else if (propertyType == typeof(uint[]))
			{
				uint[] values4 = (uint[])propertyInfo.GetValue(data, null);
				OperateResult operateResult10 = readWrite.Write(hslDeviceAddressAttribute.Address, values4);
				if (!operateResult10.IsSuccess)
				{
					return operateResult10;
				}
			}
			else if (propertyType == typeof(long))
			{
				long value5 = (long)propertyInfo.GetValue(data, null);
				OperateResult operateResult11 = readWrite.Write(hslDeviceAddressAttribute.Address, value5);
				if (!operateResult11.IsSuccess)
				{
					return operateResult11;
				}
			}
			else if (propertyType == typeof(long[]))
			{
				long[] values5 = (long[])propertyInfo.GetValue(data, null);
				OperateResult operateResult12 = readWrite.Write(hslDeviceAddressAttribute.Address, values5);
				if (!operateResult12.IsSuccess)
				{
					return operateResult12;
				}
			}
			else if (propertyType == typeof(ulong))
			{
				ulong value6 = (ulong)propertyInfo.GetValue(data, null);
				OperateResult operateResult13 = readWrite.Write(hslDeviceAddressAttribute.Address, value6);
				if (!operateResult13.IsSuccess)
				{
					return operateResult13;
				}
			}
			else if (propertyType == typeof(ulong[]))
			{
				ulong[] values6 = (ulong[])propertyInfo.GetValue(data, null);
				OperateResult operateResult14 = readWrite.Write(hslDeviceAddressAttribute.Address, values6);
				if (!operateResult14.IsSuccess)
				{
					return operateResult14;
				}
			}
			else if (propertyType == typeof(float))
			{
				float value7 = (float)propertyInfo.GetValue(data, null);
				OperateResult operateResult15 = readWrite.Write(hslDeviceAddressAttribute.Address, value7);
				if (!operateResult15.IsSuccess)
				{
					return operateResult15;
				}
			}
			else if (propertyType == typeof(float[]))
			{
				float[] values7 = (float[])propertyInfo.GetValue(data, null);
				OperateResult operateResult16 = readWrite.Write(hslDeviceAddressAttribute.Address, values7);
				if (!operateResult16.IsSuccess)
				{
					return operateResult16;
				}
			}
			else if (propertyType == typeof(double))
			{
				double value8 = (double)propertyInfo.GetValue(data, null);
				OperateResult operateResult17 = readWrite.Write(hslDeviceAddressAttribute.Address, value8);
				if (!operateResult17.IsSuccess)
				{
					return operateResult17;
				}
			}
			else if (propertyType == typeof(double[]))
			{
				double[] values8 = (double[])propertyInfo.GetValue(data, null);
				OperateResult operateResult18 = readWrite.Write(hslDeviceAddressAttribute.Address, values8);
				if (!operateResult18.IsSuccess)
				{
					return operateResult18;
				}
			}
			else if (propertyType == typeof(string))
			{
				string value9 = (string)propertyInfo.GetValue(data, null);
				OperateResult operateResult19 = readWrite.Write(hslDeviceAddressAttribute.Address, value9, hslDeviceAddressAttribute.GetEncoding());
				if (!operateResult19.IsSuccess)
				{
					return operateResult19;
				}
			}
			else if (propertyType == typeof(byte[]))
			{
				byte[] value10 = (byte[])propertyInfo.GetValue(data, null);
				OperateResult operateResult20 = readWrite.Write(hslDeviceAddressAttribute.Address, value10);
				if (!operateResult20.IsSuccess)
				{
					return operateResult20;
				}
			}
			else if (propertyType == typeof(bool))
			{
				bool value11 = (bool)propertyInfo.GetValue(data, null);
				OperateResult operateResult21 = readWrite.Write(hslDeviceAddressAttribute.Address, value11);
				if (!operateResult21.IsSuccess)
				{
					return operateResult21;
				}
			}
			else if (propertyType == typeof(bool[]))
			{
				bool[] value12 = (bool[])propertyInfo.GetValue(data, null);
				OperateResult operateResult22 = readWrite.Write(hslDeviceAddressAttribute.Address, value12);
				if (!operateResult22.IsSuccess)
				{
					return operateResult22;
				}
			}
		}
		return OperateResult.CreateSuccessResult(data);
	}

	public static T PraseStructContent<T>(byte[] buffer, int startIndex, IByteTransform byteTransform) where T : class, new()
	{
		Type typeFromHandle = typeof(T);
		object obj = typeFromHandle.Assembly.CreateInstance(typeFromHandle.FullName);
		PraseStructContent(obj, buffer, startIndex, byteTransform);
		return (T)obj;
	}

	public static void PraseStructContent(object obj, byte[] buffer, int startIndex, IByteTransform byteTransform)
	{
		PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo propertyInfo in array2)
		{
			object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(HslStructAttribute), inherit: false);
			if (customAttributes == null)
			{
				continue;
			}
			HslStructAttribute hslStructAttribute = ((customAttributes.Length != 0) ? ((HslStructAttribute)customAttributes[0]) : null);
			if (hslStructAttribute == null)
			{
				continue;
			}
			Type propertyType = propertyInfo.PropertyType;
			if (propertyType == typeof(byte))
			{
				propertyInfo.SetValue(obj, buffer[startIndex + hslStructAttribute.Index], null);
			}
			else if (propertyType == typeof(byte[]))
			{
				propertyInfo.SetValue(obj, buffer.SelectMiddle(startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(short))
			{
				propertyInfo.SetValue(obj, byteTransform.TransInt16(buffer, startIndex + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(short[]))
			{
				propertyInfo.SetValue(obj, byteTransform.TransInt16(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(ushort))
			{
				propertyInfo.SetValue(obj, byteTransform.TransUInt16(buffer, startIndex + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(ushort[]))
			{
				propertyInfo.SetValue(obj, byteTransform.TransUInt16(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(int))
			{
				propertyInfo.SetValue(obj, byteTransform.TransInt32(buffer, startIndex + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(int[]))
			{
				propertyInfo.SetValue(obj, byteTransform.TransInt32(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(uint))
			{
				propertyInfo.SetValue(obj, byteTransform.TransUInt32(buffer, startIndex + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(uint[]))
			{
				propertyInfo.SetValue(obj, byteTransform.TransUInt32(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(long))
			{
				propertyInfo.SetValue(obj, byteTransform.TransInt64(buffer, startIndex + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(long[]))
			{
				propertyInfo.SetValue(obj, byteTransform.TransInt64(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(ulong))
			{
				propertyInfo.SetValue(obj, byteTransform.TransUInt64(buffer, startIndex + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(ulong[]))
			{
				propertyInfo.SetValue(obj, byteTransform.TransUInt64(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(float))
			{
				propertyInfo.SetValue(obj, byteTransform.TransSingle(buffer, startIndex + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(float[]))
			{
				propertyInfo.SetValue(obj, byteTransform.TransSingle(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(double))
			{
				propertyInfo.SetValue(obj, byteTransform.TransDouble(buffer, startIndex + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(double[]))
			{
				propertyInfo.SetValue(obj, byteTransform.TransDouble(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length), null);
			}
			else if (propertyType == typeof(string))
			{
				Encoding uTF = Encoding.UTF8;
				uTF = (string.IsNullOrEmpty(hslStructAttribute.Encoding) ? Encoding.ASCII : (hslStructAttribute.Encoding.Equals("ASCII", StringComparison.OrdinalIgnoreCase) ? Encoding.ASCII : (hslStructAttribute.Encoding.Equals("UNICODE", StringComparison.OrdinalIgnoreCase) ? Encoding.Unicode : (hslStructAttribute.Encoding.Equals("ANSI", StringComparison.OrdinalIgnoreCase) ? Encoding.Default : (hslStructAttribute.Encoding.Equals("UTF8", StringComparison.OrdinalIgnoreCase) ? Encoding.UTF8 : (hslStructAttribute.Encoding.Equals("BIG-UNICODE", StringComparison.OrdinalIgnoreCase) ? Encoding.BigEndianUnicode : ((!hslStructAttribute.Encoding.Equals("GB2312", StringComparison.OrdinalIgnoreCase)) ? Encoding.GetEncoding(hslStructAttribute.Encoding) : Encoding.GetEncoding("GB2312"))))))));
				if (hslStructAttribute.StringMode == 0)
				{
					propertyInfo.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslStructAttribute.Index, hslStructAttribute.Length, uTF), null);
				}
				else if (hslStructAttribute.StringMode == 1)
				{
					propertyInfo.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslStructAttribute.Index + 1, buffer[startIndex + hslStructAttribute.Index], uTF), null);
				}
				else if (hslStructAttribute.StringMode == 2)
				{
					propertyInfo.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslStructAttribute.Index + 2, buffer[startIndex + hslStructAttribute.Index + 1], uTF), null);
				}
				else if (hslStructAttribute.StringMode == 3)
				{
					propertyInfo.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslStructAttribute.Index + 2, byteTransform.TransUInt16(buffer, startIndex + hslStructAttribute.Index), uTF), null);
				}
				else if (hslStructAttribute.StringMode == 4)
				{
					propertyInfo.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslStructAttribute.Index + 4, byteTransform.TransUInt16(buffer, startIndex + hslStructAttribute.Index + 2), uTF), null);
				}
				else if (hslStructAttribute.StringMode == 5)
				{
					propertyInfo.SetValue(obj, byteTransform.TransString(buffer, startIndex + hslStructAttribute.Index + 4, byteTransform.TransInt32(buffer, startIndex + hslStructAttribute.Index), uTF), null);
				}
			}
			else if (propertyType == typeof(bool))
			{
				propertyInfo.SetValue(obj, buffer.GetBoolByIndex(startIndex * 8 + hslStructAttribute.Index), null);
			}
			else if (propertyType == typeof(bool[]))
			{
				bool[] array3 = new bool[hslStructAttribute.Length];
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = buffer.GetBoolByIndex(startIndex * 8 + hslStructAttribute.Index + j);
				}
				propertyInfo.SetValue(obj, array3, null);
			}
			else
			{
				object obj2 = propertyType.Assembly.CreateInstance(propertyType.FullName);
				PraseStructContent(obj2, buffer, startIndex + hslStructAttribute.Index, byteTransform);
				propertyInfo.SetValue(obj, obj2, null);
			}
		}
	}

	public static void SetPropertyExp<T, K>(PropertyInfo propertyInfo, T obj, K objValue)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "obj");
		ParameterExpression parameterExpression2 = Expression.Parameter(propertyInfo.PropertyType, "objValue");
		MethodCallExpression body = Expression.Call(parameterExpression, propertyInfo.GetSetMethod(), parameterExpression2);
		Expression<Action<T, K>> expression = Expression.Lambda<Action<T, K>>(body, new ParameterExpression[2] { parameterExpression, parameterExpression2 });
		expression.Compile()(obj, objValue);
	}

	public static async Task<OperateResult<T>> ReadAsync<T>(IReadWriteNet readWrite) where T : class, new()
	{
		Type type = typeof(T);
		object obj = type.Assembly.CreateInstance(type.FullName);
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo property in array2)
		{
			Type propertyType = property.PropertyType;
			if (propertyType == typeof(string[]))
			{
				HslDeviceAddressAttribute[] hslAttributes = GetHslDeviceAddressAttributeArray(readWrite.GetType(), property);
				if (hslAttributes == null || hslAttributes.Length == 0)
				{
					continue;
				}
				string[] strings = new string[hslAttributes.Length];
				for (int j = 0; j < hslAttributes.Length; j++)
				{
					OperateResult<string> valueResult8 = await readWrite.ReadStringAsync(hslAttributes[j].Address, (ushort)((hslAttributes[j].Length < 0) ? 1u : ((uint)hslAttributes[j].Length)), hslAttributes[j].GetEncoding());
					if (!valueResult8.IsSuccess)
					{
						return OperateResult.CreateFailedResult<T>(valueResult8);
					}
					strings[j] = valueResult8.Content;
				}
				property.SetValue(obj, strings, null);
				continue;
			}
			object[] attribute = property.GetCustomAttributes(typeof(HslDeviceAddressAttribute), inherit: false);
			if (attribute == null)
			{
				continue;
			}
			HslDeviceAddressAttribute hslAttribute = GetHslDeviceAddressAttribute(readWrite.GetType(), property);
			if (hslAttribute == null)
			{
				continue;
			}
			if (propertyType == typeof(byte))
			{
				MethodInfo readByteMethod = readWrite.GetType().GetMethod("ReadByteAsync", new Type[1] { typeof(string) });
				if (readByteMethod == null)
				{
					return new OperateResult<T>(readWrite.GetType().Name + " not support read byte value. ");
				}
				object obj2 = readByteMethod.Invoke(readWrite, new object[1] { hslAttribute.Address });
				if (!(obj2 is Task readByteTask))
				{
					return new OperateResult<T>(readWrite.GetType().Name + " not task type result. ");
				}
				await readByteTask;
				OperateResult<byte> valueResult11 = readByteTask.GetType().GetProperty("Result").GetValue(readByteTask, null) as OperateResult<byte>;
				if (!valueResult11.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult11);
				}
				property.SetValue(obj, valueResult11.Content, null);
			}
			else if (propertyType == typeof(short))
			{
				OperateResult<short> valueResult14 = await readWrite.ReadInt16Async(hslAttribute.Address);
				if (!valueResult14.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult14);
				}
				property.SetValue(obj, valueResult14.Content, null);
			}
			else if (propertyType == typeof(short[]))
			{
				OperateResult<short[]> valueResult15 = await readWrite.ReadInt16Async(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult15.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult15);
				}
				property.SetValue(obj, valueResult15.Content, null);
			}
			else if (propertyType == typeof(ushort))
			{
				OperateResult<ushort> valueResult16 = await readWrite.ReadUInt16Async(hslAttribute.Address);
				if (!valueResult16.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult16);
				}
				property.SetValue(obj, valueResult16.Content, null);
			}
			else if (propertyType == typeof(ushort[]))
			{
				OperateResult<ushort[]> valueResult19 = await readWrite.ReadUInt16Async(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult19.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult19);
				}
				property.SetValue(obj, valueResult19.Content, null);
			}
			else if (propertyType == typeof(int))
			{
				OperateResult<int> valueResult20 = await readWrite.ReadInt32Async(hslAttribute.Address);
				if (!valueResult20.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult20);
				}
				property.SetValue(obj, valueResult20.Content, null);
			}
			else if (propertyType == typeof(int[]))
			{
				OperateResult<int[]> valueResult21 = await readWrite.ReadInt32Async(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult21.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult21);
				}
				property.SetValue(obj, valueResult21.Content, null);
			}
			else if (propertyType == typeof(uint))
			{
				OperateResult<uint> valueResult22 = await readWrite.ReadUInt32Async(hslAttribute.Address);
				if (!valueResult22.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult22);
				}
				property.SetValue(obj, valueResult22.Content, null);
			}
			else if (propertyType == typeof(uint[]))
			{
				OperateResult<uint[]> valueResult23 = await readWrite.ReadUInt32Async(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult23.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult23);
				}
				property.SetValue(obj, valueResult23.Content, null);
			}
			else if (propertyType == typeof(long))
			{
				OperateResult<long> valueResult24 = await readWrite.ReadInt64Async(hslAttribute.Address);
				if (!valueResult24.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult24);
				}
				property.SetValue(obj, valueResult24.Content, null);
			}
			else if (propertyType == typeof(long[]))
			{
				OperateResult<long[]> valueResult25 = await readWrite.ReadInt64Async(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult25.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult25);
				}
				property.SetValue(obj, valueResult25.Content, null);
			}
			else if (propertyType == typeof(ulong))
			{
				OperateResult<ulong> valueResult26 = await readWrite.ReadUInt64Async(hslAttribute.Address);
				if (!valueResult26.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult26);
				}
				property.SetValue(obj, valueResult26.Content, null);
			}
			else if (propertyType == typeof(ulong[]))
			{
				OperateResult<ulong[]> valueResult27 = await readWrite.ReadUInt64Async(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult27.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult27);
				}
				property.SetValue(obj, valueResult27.Content, null);
			}
			else if (propertyType == typeof(float))
			{
				OperateResult<float> valueResult28 = await readWrite.ReadFloatAsync(hslAttribute.Address);
				if (!valueResult28.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult28);
				}
				property.SetValue(obj, valueResult28.Content, null);
			}
			else if (propertyType == typeof(float[]))
			{
				OperateResult<float[]> valueResult29 = await readWrite.ReadFloatAsync(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult29.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult29);
				}
				property.SetValue(obj, valueResult29.Content, null);
			}
			else if (propertyType == typeof(double))
			{
				OperateResult<double> valueResult30 = await readWrite.ReadDoubleAsync(hslAttribute.Address);
				if (!valueResult30.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult30);
				}
				property.SetValue(obj, valueResult30.Content, null);
			}
			else if (propertyType == typeof(double[]))
			{
				OperateResult<double[]> valueResult31 = await readWrite.ReadDoubleAsync(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult31.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult31);
				}
				property.SetValue(obj, valueResult31.Content, null);
			}
			else if (propertyType == typeof(string))
			{
				OperateResult<string> valueResult32 = await readWrite.ReadStringAsync(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)), hslAttribute.GetEncoding());
				if (!valueResult32.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult32);
				}
				property.SetValue(obj, valueResult32.Content, null);
			}
			else if (propertyType == typeof(byte[]))
			{
				OperateResult<byte[]> valueResult33 = await readWrite.ReadAsync(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult33.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult33);
				}
				property.SetValue(obj, valueResult33.Content, null);
			}
			else if (propertyType == typeof(bool))
			{
				OperateResult<bool> valueResult34 = await readWrite.ReadBoolAsync(hslAttribute.Address);
				if (!valueResult34.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult34);
				}
				property.SetValue(obj, valueResult34.Content, null);
			}
			else if (propertyType == typeof(bool[]))
			{
				OperateResult<bool[]> valueResult35 = await readWrite.ReadBoolAsync(hslAttribute.Address, (ushort)((hslAttribute.Length < 0) ? 1u : ((uint)hslAttribute.Length)));
				if (!valueResult35.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult35);
				}
				property.SetValue(obj, valueResult35.Content, null);
			}
		}
		return OperateResult.CreateSuccessResult((T)obj);
	}

	public static async Task<OperateResult> WriteAsync<T>(T data, IReadWriteNet readWrite) where T : class, new()
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		Type type = typeof(T);
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo property in array2)
		{
			Type propertyType = property.PropertyType;
			if (propertyType == typeof(string[]))
			{
				HslDeviceAddressAttribute[] hslAttributes = GetHslDeviceAddressAttributeArray(readWrite.GetType(), property);
				if (hslAttributes == null || hslAttributes.Length == 0)
				{
					continue;
				}
				string[] strings = (string[])property.GetValue(data, null);
				for (int j = 0; j < hslAttributes.Length; j++)
				{
					OperateResult writeResult21 = await readWrite.WriteAsync(hslAttributes[j].Address, strings[j], hslAttributes[j].GetEncoding());
					if (!writeResult21.IsSuccess)
					{
						return writeResult21;
					}
				}
				continue;
			}
			object[] attribute = property.GetCustomAttributes(typeof(HslDeviceAddressAttribute), inherit: false);
			if (attribute == null)
			{
				continue;
			}
			HslDeviceAddressAttribute hslAttribute = GetHslDeviceAddressAttribute(readWrite.GetType(), property);
			if (hslAttribute == null)
			{
				continue;
			}
			if (propertyType == typeof(byte))
			{
				MethodInfo method = readWrite.GetType().GetMethod("WriteAsync", new Type[2]
				{
					typeof(string),
					typeof(byte)
				});
				if (method == null)
				{
					return new OperateResult<T>(readWrite.GetType().Name + " not support write byte value. ");
				}
				byte value = (byte)property.GetValue(data, null);
				object obj = method.Invoke(readWrite, new object[2] { hslAttribute.Address, value });
				if (!(obj is Task writeTask))
				{
					return new OperateResult(readWrite.GetType().Name + " not task type result. ");
				}
				await writeTask;
				OperateResult valueResult = writeTask.GetType().GetProperty("Result").GetValue(writeTask, null) as OperateResult;
				if (!valueResult.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(valueResult);
				}
			}
			else if (propertyType == typeof(short))
			{
				OperateResult writeResult22 = await readWrite.WriteAsync(value: (short)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult22.IsSuccess)
				{
					return writeResult22;
				}
			}
			else if (propertyType == typeof(short[]))
			{
				OperateResult writeResult23 = await readWrite.WriteAsync(values: (short[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult23.IsSuccess)
				{
					return writeResult23;
				}
			}
			else if (propertyType == typeof(ushort))
			{
				OperateResult writeResult24 = await readWrite.WriteAsync(value: (ushort)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult24.IsSuccess)
				{
					return writeResult24;
				}
			}
			else if (propertyType == typeof(ushort[]))
			{
				OperateResult writeResult25 = await readWrite.WriteAsync(values: (ushort[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult25.IsSuccess)
				{
					return writeResult25;
				}
			}
			else if (propertyType == typeof(int))
			{
				OperateResult writeResult26 = await readWrite.WriteAsync(value: (int)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult26.IsSuccess)
				{
					return writeResult26;
				}
			}
			else if (propertyType == typeof(int[]))
			{
				OperateResult writeResult27 = await readWrite.WriteAsync(values: (int[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult27.IsSuccess)
				{
					return writeResult27;
				}
			}
			else if (propertyType == typeof(uint))
			{
				OperateResult writeResult28 = await readWrite.WriteAsync(value: (uint)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult28.IsSuccess)
				{
					return writeResult28;
				}
			}
			else if (propertyType == typeof(uint[]))
			{
				OperateResult writeResult29 = await readWrite.WriteAsync(values: (uint[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult29.IsSuccess)
				{
					return writeResult29;
				}
			}
			else if (propertyType == typeof(long))
			{
				OperateResult writeResult30 = await readWrite.WriteAsync(value: (long)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult30.IsSuccess)
				{
					return writeResult30;
				}
			}
			else if (propertyType == typeof(long[]))
			{
				OperateResult writeResult31 = await readWrite.WriteAsync(values: (long[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult31.IsSuccess)
				{
					return writeResult31;
				}
			}
			else if (propertyType == typeof(ulong))
			{
				OperateResult writeResult32 = await readWrite.WriteAsync(value: (ulong)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult32.IsSuccess)
				{
					return writeResult32;
				}
			}
			else if (propertyType == typeof(ulong[]))
			{
				OperateResult writeResult33 = await readWrite.WriteAsync(values: (ulong[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult33.IsSuccess)
				{
					return writeResult33;
				}
			}
			else if (propertyType == typeof(float))
			{
				OperateResult writeResult34 = await readWrite.WriteAsync(value: (float)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult34.IsSuccess)
				{
					return writeResult34;
				}
			}
			else if (propertyType == typeof(float[]))
			{
				OperateResult writeResult35 = await readWrite.WriteAsync(values: (float[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult35.IsSuccess)
				{
					return writeResult35;
				}
			}
			else if (propertyType == typeof(double))
			{
				OperateResult writeResult36 = await readWrite.WriteAsync(value: (double)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult36.IsSuccess)
				{
					return writeResult36;
				}
			}
			else if (propertyType == typeof(double[]))
			{
				OperateResult writeResult37 = await readWrite.WriteAsync(values: (double[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult37.IsSuccess)
				{
					return writeResult37;
				}
			}
			else if (propertyType == typeof(string))
			{
				OperateResult writeResult38 = await readWrite.WriteAsync(value: (string)property.GetValue(data, null), address: hslAttribute.Address, encoding: hslAttribute.GetEncoding());
				if (!writeResult38.IsSuccess)
				{
					return writeResult38;
				}
			}
			else if (propertyType == typeof(byte[]))
			{
				OperateResult writeResult39 = await readWrite.WriteAsync(value: (byte[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult39.IsSuccess)
				{
					return writeResult39;
				}
			}
			else if (propertyType == typeof(bool))
			{
				OperateResult writeResult40 = await readWrite.WriteAsync(value: (bool)property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult40.IsSuccess)
				{
					return writeResult40;
				}
			}
			else if (propertyType == typeof(bool[]))
			{
				OperateResult writeResult41 = await readWrite.WriteAsync(value: (bool[])property.GetValue(data, null), address: hslAttribute.Address);
				if (!writeResult41.IsSuccess)
				{
					return writeResult41;
				}
			}
		}
		return OperateResult.CreateSuccessResult(data);
	}

	internal static void SetPropertyObjectValue(PropertyInfo property, object obj, string value)
	{
		Type propertyType = property.PropertyType;
		if (propertyType == typeof(short))
		{
			property.SetValue(obj, short.Parse(value), null);
		}
		else if (propertyType == typeof(ushort))
		{
			property.SetValue(obj, ushort.Parse(value), null);
		}
		else if (propertyType == typeof(int))
		{
			property.SetValue(obj, int.Parse(value), null);
		}
		else if (propertyType == typeof(uint))
		{
			property.SetValue(obj, uint.Parse(value), null);
		}
		else if (propertyType == typeof(long))
		{
			property.SetValue(obj, long.Parse(value), null);
		}
		else if (propertyType == typeof(ulong))
		{
			property.SetValue(obj, ulong.Parse(value), null);
		}
		else if (propertyType == typeof(float))
		{
			property.SetValue(obj, float.Parse(value), null);
		}
		else if (propertyType == typeof(double))
		{
			property.SetValue(obj, double.Parse(value), null);
		}
		else if (propertyType == typeof(string))
		{
			property.SetValue(obj, value, null);
		}
		else if (propertyType == typeof(byte))
		{
			property.SetValue(obj, byte.Parse(value), null);
		}
		else if (propertyType == typeof(bool))
		{
			property.SetValue(obj, bool.Parse(value), null);
		}
		else
		{
			property.SetValue(obj, value, null);
		}
	}

	internal static void SetPropertyObjectValueArray(PropertyInfo property, object obj, string[] values)
	{
		Type propertyType = property.PropertyType;
		if (propertyType == typeof(short[]))
		{
			property.SetValue(obj, values.Select((string m) => short.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<short>))
		{
			property.SetValue(obj, values.Select((string m) => short.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(ushort[]))
		{
			property.SetValue(obj, values.Select((string m) => ushort.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<ushort>))
		{
			property.SetValue(obj, values.Select((string m) => ushort.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(int[]))
		{
			property.SetValue(obj, values.Select((string m) => int.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<int>))
		{
			property.SetValue(obj, values.Select((string m) => int.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(uint[]))
		{
			property.SetValue(obj, values.Select((string m) => uint.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<uint>))
		{
			property.SetValue(obj, values.Select((string m) => uint.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(long[]))
		{
			property.SetValue(obj, values.Select((string m) => long.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<long>))
		{
			property.SetValue(obj, values.Select((string m) => long.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(ulong[]))
		{
			property.SetValue(obj, values.Select((string m) => ulong.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<ulong>))
		{
			property.SetValue(obj, values.Select((string m) => ulong.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(float[]))
		{
			property.SetValue(obj, values.Select((string m) => float.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<float>))
		{
			property.SetValue(obj, values.Select((string m) => float.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(double[]))
		{
			property.SetValue(obj, values.Select((string m) => double.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(double[]))
		{
			property.SetValue(obj, values.Select((string m) => double.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(string[]))
		{
			property.SetValue(obj, values, null);
		}
		else if (propertyType == typeof(List<string>))
		{
			property.SetValue(obj, new List<string>(values), null);
		}
		else if (propertyType == typeof(byte[]))
		{
			property.SetValue(obj, values.Select((string m) => byte.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<byte>))
		{
			property.SetValue(obj, values.Select((string m) => byte.Parse(m)).ToList(), null);
		}
		else if (propertyType == typeof(bool[]))
		{
			property.SetValue(obj, values.Select((string m) => bool.Parse(m)).ToArray(), null);
		}
		else if (propertyType == typeof(List<bool>))
		{
			property.SetValue(obj, values.Select((string m) => bool.Parse(m)).ToList(), null);
		}
		else
		{
			property.SetValue(obj, values, null);
		}
	}

	public static OperateResult<T> Read<T>(RedisClient redis) where T : class, new()
	{
		Type typeFromHandle = typeof(T);
		object obj = typeFromHandle.Assembly.CreateInstance(typeFromHandle.FullName);
		PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		List<PropertyInfoKeyName> list = new List<PropertyInfoKeyName>();
		List<PropertyInfoHashKeyName> list2 = new List<PropertyInfoHashKeyName>();
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo propertyInfo in array2)
		{
			object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(HslRedisKeyAttribute), inherit: false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				HslRedisKeyAttribute hslRedisKeyAttribute = (HslRedisKeyAttribute)customAttributes[0];
				list.Add(new PropertyInfoKeyName(propertyInfo, hslRedisKeyAttribute.KeyName));
				continue;
			}
			customAttributes = propertyInfo.GetCustomAttributes(typeof(HslRedisListItemAttribute), inherit: false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				HslRedisListItemAttribute hslRedisListItemAttribute = (HslRedisListItemAttribute)customAttributes[0];
				OperateResult<string> operateResult = redis.ReadListByIndex(hslRedisListItemAttribute.ListKey, hslRedisListItemAttribute.Index);
				if (!operateResult.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult);
				}
				SetPropertyObjectValue(propertyInfo, obj, operateResult.Content);
				continue;
			}
			customAttributes = propertyInfo.GetCustomAttributes(typeof(HslRedisListAttribute), inherit: false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				HslRedisListAttribute hslRedisListAttribute = (HslRedisListAttribute)customAttributes[0];
				OperateResult<string[]> operateResult2 = redis.ListRange(hslRedisListAttribute.ListKey, hslRedisListAttribute.StartIndex, hslRedisListAttribute.EndIndex);
				if (!operateResult2.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult2);
				}
				SetPropertyObjectValueArray(propertyInfo, obj, operateResult2.Content);
			}
			else
			{
				customAttributes = propertyInfo.GetCustomAttributes(typeof(HslRedisHashFieldAttribute), inherit: false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					HslRedisHashFieldAttribute hslRedisHashFieldAttribute = (HslRedisHashFieldAttribute)customAttributes[0];
					list2.Add(new PropertyInfoHashKeyName(propertyInfo, hslRedisHashFieldAttribute.HaskKey, hslRedisHashFieldAttribute.Field));
				}
			}
		}
		if (list.Count > 0)
		{
			OperateResult<string[]> operateResult3 = redis.ReadKey(list.Select((PropertyInfoKeyName m) => m.KeyName).ToArray());
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<T>(operateResult3);
			}
			for (int num = 0; num < list.Count; num++)
			{
				SetPropertyObjectValue(list[num].PropertyInfo, obj, operateResult3.Content[num]);
			}
		}
		if (list2.Count > 0)
		{
			var enumerable = from m in list2
				group m by m.KeyName into g
				select new
				{
					Key = g.Key,
					Values = g.ToArray()
				};
			foreach (var item in enumerable)
			{
				if (item.Values.Length == 1)
				{
					OperateResult<string> operateResult4 = redis.ReadHashKey(item.Key, item.Values[0].Field);
					if (!operateResult4.IsSuccess)
					{
						return OperateResult.CreateFailedResult<T>(operateResult4);
					}
					SetPropertyObjectValue(item.Values[0].PropertyInfo, obj, operateResult4.Content);
					continue;
				}
				OperateResult<string[]> operateResult5 = redis.ReadHashKey(item.Key, item.Values.Select((PropertyInfoHashKeyName m) => m.Field).ToArray());
				if (!operateResult5.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(operateResult5);
				}
				for (int num2 = 0; num2 < item.Values.Length; num2++)
				{
					SetPropertyObjectValue(item.Values[num2].PropertyInfo, obj, operateResult5.Content[num2]);
				}
			}
		}
		return OperateResult.CreateSuccessResult((T)obj);
	}

	public static OperateResult Write<T>(T data, RedisClient redis) where T : class, new()
	{
		Type typeFromHandle = typeof(T);
		PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		List<PropertyInfoKeyName> list = new List<PropertyInfoKeyName>();
		List<PropertyInfoHashKeyName> list2 = new List<PropertyInfoHashKeyName>();
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo propertyInfo in array2)
		{
			object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(HslRedisKeyAttribute), inherit: false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				HslRedisKeyAttribute hslRedisKeyAttribute = (HslRedisKeyAttribute)customAttributes[0];
				list.Add(new PropertyInfoKeyName(propertyInfo, hslRedisKeyAttribute.KeyName, propertyInfo.GetValue(data, null).ToString()));
				continue;
			}
			customAttributes = propertyInfo.GetCustomAttributes(typeof(HslRedisHashFieldAttribute), inherit: false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				HslRedisHashFieldAttribute hslRedisHashFieldAttribute = (HslRedisHashFieldAttribute)customAttributes[0];
				list2.Add(new PropertyInfoHashKeyName(propertyInfo, hslRedisHashFieldAttribute.HaskKey, hslRedisHashFieldAttribute.Field, propertyInfo.GetValue(data, null).ToString()));
			}
		}
		if (list.Count > 0)
		{
			OperateResult operateResult = redis.WriteKey(list.Select((PropertyInfoKeyName m) => m.KeyName).ToArray(), list.Select((PropertyInfoKeyName m) => m.Value).ToArray());
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		if (list2.Count > 0)
		{
			var enumerable = from m in list2
				group m by m.KeyName into g
				select new
				{
					Key = g.Key,
					Values = g.ToArray()
				};
			foreach (var item in enumerable)
			{
				if (item.Values.Length == 1)
				{
					OperateResult operateResult2 = redis.WriteHashKey(item.Key, item.Values[0].Field, item.Values[0].Value);
					if (!operateResult2.IsSuccess)
					{
						return operateResult2;
					}
					continue;
				}
				OperateResult operateResult3 = redis.WriteHashKey(item.Key, item.Values.Select((PropertyInfoHashKeyName m) => m.Field).ToArray(), item.Values.Select((PropertyInfoHashKeyName m) => m.Value).ToArray());
				if (operateResult3.IsSuccess)
				{
					continue;
				}
				return operateResult3;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<T>> ReadAsync<T>(RedisClient redis) where T : class, new()
	{
		Type type = typeof(T);
		object obj = type.Assembly.CreateInstance(type.FullName);
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		List<PropertyInfoKeyName> keyPropertyInfos = new List<PropertyInfoKeyName>();
		List<PropertyInfoHashKeyName> propertyInfoHashKeys = new List<PropertyInfoHashKeyName>();
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo property in array2)
		{
			object[] attributes4 = property.GetCustomAttributes(typeof(HslRedisKeyAttribute), inherit: false);
			object[] array3 = attributes4;
			if (array3 != null && array3.Length != 0)
			{
				HslRedisKeyAttribute attribute4 = (HslRedisKeyAttribute)attributes4[0];
				keyPropertyInfos.Add(new PropertyInfoKeyName(property, attribute4.KeyName));
				continue;
			}
			attributes4 = property.GetCustomAttributes(typeof(HslRedisListItemAttribute), inherit: false);
			object[] array4 = attributes4;
			if (array4 != null && array4.Length != 0)
			{
				HslRedisListItemAttribute attribute5 = (HslRedisListItemAttribute)attributes4[0];
				OperateResult<string> read2 = await redis.ReadListByIndexAsync(attribute5.ListKey, attribute5.Index);
				if (!read2.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(read2);
				}
				SetPropertyObjectValue(property, obj, read2.Content);
				continue;
			}
			attributes4 = property.GetCustomAttributes(typeof(HslRedisListAttribute), inherit: false);
			object[] array5 = attributes4;
			if (array5 != null && array5.Length != 0)
			{
				HslRedisListAttribute attribute6 = (HslRedisListAttribute)attributes4[0];
				OperateResult<string[]> read3 = await redis.ListRangeAsync(attribute6.ListKey, attribute6.StartIndex, attribute6.EndIndex);
				if (!read3.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(read3);
				}
				SetPropertyObjectValueArray(property, obj, read3.Content);
			}
			else
			{
				attributes4 = property.GetCustomAttributes(typeof(HslRedisHashFieldAttribute), inherit: false);
				object[] array6 = attributes4;
				if (array6 != null && array6.Length != 0)
				{
					HslRedisHashFieldAttribute attribute7 = (HslRedisHashFieldAttribute)attributes4[0];
					propertyInfoHashKeys.Add(new PropertyInfoHashKeyName(property, attribute7.HaskKey, attribute7.Field));
				}
			}
		}
		if (keyPropertyInfos.Count > 0)
		{
			OperateResult<string[]> readKeys2 = await redis.ReadKeyAsync(keyPropertyInfos.Select((PropertyInfoKeyName m) => m.KeyName).ToArray());
			if (!readKeys2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<T>(readKeys2);
			}
			for (int j = 0; j < keyPropertyInfos.Count; j++)
			{
				SetPropertyObjectValue(keyPropertyInfos[j].PropertyInfo, obj, readKeys2.Content[j]);
			}
		}
		if (propertyInfoHashKeys.Count > 0)
		{
			var tmp = from m in propertyInfoHashKeys
				group m by m.KeyName into g
				select new
				{
					Key = g.Key,
					Values = g.ToArray()
				};
			foreach (var item in tmp)
			{
				if (item.Values.Length == 1)
				{
					OperateResult<string> readKey = await redis.ReadHashKeyAsync(item.Key, item.Values[0].Field);
					if (!readKey.IsSuccess)
					{
						return OperateResult.CreateFailedResult<T>(readKey);
					}
					SetPropertyObjectValue(item.Values[0].PropertyInfo, obj, readKey.Content);
					continue;
				}
				OperateResult<string[]> readKeys3 = await redis.ReadHashKeyAsync(item.Key, item.Values.Select((PropertyInfoHashKeyName m) => m.Field).ToArray());
				if (!readKeys3.IsSuccess)
				{
					return OperateResult.CreateFailedResult<T>(readKeys3);
				}
				for (int i2 = 0; i2 < item.Values.Length; i2++)
				{
					SetPropertyObjectValue(item.Values[i2].PropertyInfo, obj, readKeys3.Content[i2]);
				}
			}
		}
		return OperateResult.CreateSuccessResult((T)obj);
	}

	public static async Task<OperateResult> WriteAsync<T>(T data, RedisClient redis) where T : class, new()
	{
		Type type = typeof(T);
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		List<PropertyInfoKeyName> keyPropertyInfos = new List<PropertyInfoKeyName>();
		List<PropertyInfoHashKeyName> propertyInfoHashKeys = new List<PropertyInfoHashKeyName>();
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo property in array2)
		{
			object[] attributes2 = property.GetCustomAttributes(typeof(HslRedisKeyAttribute), inherit: false);
			object[] array3 = attributes2;
			if (array3 != null && array3.Length != 0)
			{
				HslRedisKeyAttribute attribute2 = (HslRedisKeyAttribute)attributes2[0];
				keyPropertyInfos.Add(new PropertyInfoKeyName(property, attribute2.KeyName, property.GetValue(data, null).ToString()));
				continue;
			}
			attributes2 = property.GetCustomAttributes(typeof(HslRedisHashFieldAttribute), inherit: false);
			object[] array4 = attributes2;
			if (array4 != null && array4.Length != 0)
			{
				HslRedisHashFieldAttribute attribute3 = (HslRedisHashFieldAttribute)attributes2[0];
				propertyInfoHashKeys.Add(new PropertyInfoHashKeyName(property, attribute3.HaskKey, attribute3.Field, property.GetValue(data, null).ToString()));
			}
		}
		if (keyPropertyInfos.Count > 0)
		{
			OperateResult writeResult2 = await redis.WriteKeyAsync(keyPropertyInfos.Select((PropertyInfoKeyName m) => m.KeyName).ToArray(), keyPropertyInfos.Select((PropertyInfoKeyName m) => m.Value).ToArray());
			if (!writeResult2.IsSuccess)
			{
				return writeResult2;
			}
		}
		if (propertyInfoHashKeys.Count > 0)
		{
			var tmp = from m in propertyInfoHashKeys
				group m by m.KeyName into g
				select new
				{
					Key = g.Key,
					Values = g.ToArray()
				};
			foreach (var item in tmp)
			{
				if (item.Values.Length == 1)
				{
					OperateResult writeResult3 = await redis.WriteHashKeyAsync(item.Key, item.Values[0].Field, item.Values[0].Value);
					if (!writeResult3.IsSuccess)
					{
						return writeResult3;
					}
					continue;
				}
				OperateResult writeResult4 = await redis.WriteHashKeyAsync(item.Key, item.Values.Select((PropertyInfoHashKeyName m) => m.Field).ToArray(), item.Values.Select((PropertyInfoHashKeyName m) => m.Value).ToArray());
				if (writeResult4.IsSuccess)
				{
					continue;
				}
				return writeResult4;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public static object[] GetParametersFromJson(ISessionContext context, HttpListenerRequest request, ParameterInfo[] parameters, string json)
	{
		JObject jObject = (string.IsNullOrEmpty(json) ? new JObject() : JObject.Parse(json));
		object[] array = new object[parameters.Length];
		for (int i = 0; i < parameters.Length; i++)
		{
			string text = parameters[i].Name;
			if (!jObject.ContainsKey(text))
			{
				if (text == "value" && jObject.ContainsKey("values"))
				{
					text = "values";
				}
				else if (text == "values" && jObject.ContainsKey("value"))
				{
					text = "value";
				}
			}
			if (parameters[i].ParameterType == typeof(byte))
			{
				array[i] = jObject.Value<byte>(text);
			}
			else if (parameters[i].ParameterType == typeof(short))
			{
				array[i] = jObject.Value<short>(text);
			}
			else if (parameters[i].ParameterType == typeof(ushort))
			{
				array[i] = jObject.Value<ushort>(text);
			}
			else if (parameters[i].ParameterType == typeof(int))
			{
				array[i] = jObject.Value<int>(text);
			}
			else if (parameters[i].ParameterType == typeof(uint))
			{
				array[i] = jObject.Value<uint>(text);
			}
			else if (parameters[i].ParameterType == typeof(long))
			{
				array[i] = jObject.Value<long>(text);
			}
			else if (parameters[i].ParameterType == typeof(ulong))
			{
				array[i] = jObject.Value<ulong>(text);
			}
			else if (parameters[i].ParameterType == typeof(double))
			{
				array[i] = jObject.Value<double>(text);
			}
			else if (parameters[i].ParameterType == typeof(float))
			{
				array[i] = jObject.Value<float>(text);
			}
			else if (parameters[i].ParameterType == typeof(bool))
			{
				array[i] = jObject.Value<bool>(text);
			}
			else if (parameters[i].ParameterType == typeof(string))
			{
				array[i] = jObject.Value<string>(text);
			}
			else if (parameters[i].ParameterType == typeof(DateTime))
			{
				array[i] = jObject.Value<DateTime>(text);
			}
			else if (parameters[i].ParameterType == typeof(byte[]))
			{
				array[i] = jObject.Value<string>(text).ToHexBytes();
			}
			else if (parameters[i].ParameterType == typeof(short[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<short>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(ushort[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<ushort>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(int[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<int>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(uint[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<uint>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(long[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<long>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(ulong[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<ulong>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(float[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<float>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(double[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<double>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(bool[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<bool>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(string[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<string>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(DateTime[]))
			{
				array[i] = (from m in jObject[text].ToArray()
					select m.Value<DateTime>()).ToArray();
			}
			else if (parameters[i].ParameterType == typeof(ISessionContext))
			{
				array[i] = context;
			}
			else if (parameters[i].ParameterType == typeof(HttpListenerRequest))
			{
				array[i] = request;
			}
			else if (parameters[i].ParameterType.IsArray)
			{
				array[i] = ((JArray)jObject[text]).ToObject(parameters[i].ParameterType);
			}
			else if (parameters[i].ParameterType == typeof(JObject))
			{
				try
				{
					array[i] = (JObject)jObject[text];
				}
				catch
				{
					array[i] = JObject.Parse(jObject.Value<string>(text));
				}
			}
			else
			{
				try
				{
					array[i] = jObject[text].ToObject(parameters[i].ParameterType);
				}
				catch
				{
					array[i] = JObject.Parse(jObject.Value<string>(text)).ToObject(parameters[i].ParameterType);
				}
			}
		}
		return array;
	}

	public static object[] GetParametersFromUrl(ISessionContext context, HttpListenerRequest request, ParameterInfo[] parameters, string url)
	{
		if (url.IndexOf('?') > 0)
		{
			url = url.Substring(url.IndexOf('?') + 1);
		}
		string[] array = url.Split(new char[1] { '&' }, StringSplitOptions.RemoveEmptyEntries);
		Dictionary<string, string> dictionary = new Dictionary<string, string>(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]) && array[i].IndexOf('=') > 0)
			{
				dictionary.Add(array[i].Substring(0, array[i].IndexOf('=')).Trim(' '), array[i].Substring(array[i].IndexOf('=') + 1));
			}
		}
		object[] array2 = new object[parameters.Length];
		for (int j = 0; j < parameters.Length; j++)
		{
			if (parameters[j].ParameterType == typeof(byte))
			{
				array2[j] = byte.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(short))
			{
				array2[j] = short.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(ushort))
			{
				array2[j] = ushort.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(int))
			{
				array2[j] = int.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(uint))
			{
				array2[j] = uint.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(long))
			{
				array2[j] = long.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(ulong))
			{
				array2[j] = ulong.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(double))
			{
				array2[j] = double.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(float))
			{
				array2[j] = float.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(bool))
			{
				array2[j] = bool.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(string))
			{
				array2[j] = dictionary[parameters[j].Name];
			}
			else if (parameters[j].ParameterType == typeof(DateTime))
			{
				array2[j] = DateTime.Parse(dictionary[parameters[j].Name]);
			}
			else if (parameters[j].ParameterType == typeof(byte[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToHexBytes();
			}
			else if (parameters[j].ParameterType == typeof(short[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<short>();
			}
			else if (parameters[j].ParameterType == typeof(ushort[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<ushort>();
			}
			else if (parameters[j].ParameterType == typeof(int[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<int>();
			}
			else if (parameters[j].ParameterType == typeof(uint[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<uint>();
			}
			else if (parameters[j].ParameterType == typeof(long[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<long>();
			}
			else if (parameters[j].ParameterType == typeof(ulong[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<ulong>();
			}
			else if (parameters[j].ParameterType == typeof(float[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<float>();
			}
			else if (parameters[j].ParameterType == typeof(double[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<double>();
			}
			else if (parameters[j].ParameterType == typeof(bool[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<bool>();
			}
			else if (parameters[j].ParameterType == typeof(string[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<string>();
			}
			else if (parameters[j].ParameterType == typeof(DateTime[]))
			{
				array2[j] = dictionary[parameters[j].Name].ToStringArray<DateTime>();
			}
			else if (parameters[j].ParameterType == typeof(ISessionContext))
			{
				array2[j] = context;
			}
			else if (parameters[j].ParameterType == typeof(HttpListenerRequest))
			{
				array2[j] = request;
			}
			else
			{
				array2[j] = JToken.Parse(dictionary[parameters[j].Name]).ToObject(parameters[j].ParameterType);
			}
		}
		return array2;
	}

	public static JObject GetParametersFromJson(MethodInfo method, ParameterInfo[] parameters)
	{
		JObject jObject = new JObject();
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].ParameterType == typeof(byte))
			{
				jObject.Add(parameters[i].Name, new JValue((uint)(parameters[i].HasDefaultValue ? ((byte)parameters[i].DefaultValue) : 0)));
			}
			else if (parameters[i].ParameterType == typeof(short))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((short)parameters[i].DefaultValue) : 0));
			}
			else if (parameters[i].ParameterType == typeof(ushort))
			{
				jObject.Add(parameters[i].Name, new JValue((uint)(parameters[i].HasDefaultValue ? ((ushort)parameters[i].DefaultValue) : 0)));
			}
			else if (parameters[i].ParameterType == typeof(int))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((int)parameters[i].DefaultValue) : 0));
			}
			else if (parameters[i].ParameterType == typeof(uint))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((uint)parameters[i].DefaultValue) : 0));
			}
			else if (parameters[i].ParameterType == typeof(long))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((long)parameters[i].DefaultValue) : 0));
			}
			else if (parameters[i].ParameterType == typeof(ulong))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((ulong)parameters[i].DefaultValue) : 0));
			}
			else if (parameters[i].ParameterType == typeof(double))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((double)parameters[i].DefaultValue) : 0.0));
			}
			else if (parameters[i].ParameterType == typeof(float))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((float)parameters[i].DefaultValue) : 0f));
			}
			else if (parameters[i].ParameterType == typeof(bool))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue && (bool)parameters[i].DefaultValue));
			}
			else if (parameters[i].ParameterType == typeof(string))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((string)parameters[i].DefaultValue) : ""));
			}
			else if (parameters[i].ParameterType == typeof(DateTime))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((DateTime)parameters[i].DefaultValue).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
			}
			else if (parameters[i].ParameterType == typeof(byte[]))
			{
				jObject.Add(parameters[i].Name, new JValue(parameters[i].HasDefaultValue ? ((byte[])parameters[i].DefaultValue).ToHexString() : "00 1A 2B 3C 4D"));
			}
			else if (parameters[i].ParameterType == typeof(short[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((short[])parameters[i].DefaultValue) : new short[3] { 1, 2, 3 }));
			}
			else if (parameters[i].ParameterType == typeof(ushort[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((ushort[])parameters[i].DefaultValue) : new ushort[3] { 1, 2, 3 }));
			}
			else if (parameters[i].ParameterType == typeof(int[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((int[])parameters[i].DefaultValue) : new int[3] { 1, 2, 3 }));
			}
			else if (parameters[i].ParameterType == typeof(uint[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((uint[])parameters[i].DefaultValue) : new uint[3] { 1u, 2u, 3u }));
			}
			else if (parameters[i].ParameterType == typeof(long[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((long[])parameters[i].DefaultValue) : new long[3] { 1L, 2L, 3L }));
			}
			else if (parameters[i].ParameterType == typeof(ulong[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((ulong[])parameters[i].DefaultValue) : new ulong[3] { 1uL, 2uL, 3uL }));
			}
			else if (parameters[i].ParameterType == typeof(float[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((float[])parameters[i].DefaultValue) : new float[3] { 1f, 2f, 3f }));
			}
			else if (parameters[i].ParameterType == typeof(double[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((double[])parameters[i].DefaultValue) : new double[3] { 1.0, 2.0, 3.0 }));
			}
			else if (parameters[i].ParameterType == typeof(bool[]))
			{
				jObject.Add(parameters[i].Name, new JArray(parameters[i].HasDefaultValue ? ((bool[])parameters[i].DefaultValue) : new bool[3] { true, false, false }));
			}
			else if (parameters[i].ParameterType == typeof(string[]))
			{
				string name = parameters[i].Name;
				object[] array = (parameters[i].HasDefaultValue ? ((string[])parameters[i].DefaultValue) : new string[3] { "1", "2", "3" });
				object[] content = array;
				jObject.Add(name, new JArray(content));
			}
			else if (parameters[i].ParameterType == typeof(DateTime[]))
			{
				string name2 = parameters[i].Name;
				object[] array2;
				if (!parameters[i].HasDefaultValue)
				{
					object[] array = new string[1] { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
					array2 = array;
				}
				else
				{
					object[] array = ((DateTime[])parameters[i].DefaultValue).Select((DateTime m) => m.ToString("yyyy-MM-dd HH:mm:ss")).ToArray();
					array2 = array;
				}
				object[] content2 = array2;
				jObject.Add(name2, new JArray(content2));
			}
			else if (!(parameters[i].ParameterType == typeof(ISessionContext)) && !(parameters[i].ParameterType == typeof(HttpListenerRequest)))
			{
				if (parameters[i].ParameterType.IsArray)
				{
					jObject.Add(parameters[i].Name, parameters[i].HasDefaultValue ? JToken.FromObject(parameters[i].DefaultValue) : JToken.FromObject(GetObjFromArrayParameterType(parameters[i].ParameterType)));
				}
				else
				{
					jObject.Add(parameters[i].Name, JToken.FromObject(parameters[i].HasDefaultValue ? parameters[i].DefaultValue : Activator.CreateInstance(parameters[i].ParameterType)));
				}
			}
		}
		return jObject;
	}

	private static object GetObjFromArrayParameterType(Type parameterType)
	{
		Type type = null;
		Type[] genericArguments = parameterType.GetGenericArguments();
		type = ((genericArguments.Length == 0) ? parameterType.GetElementType() : genericArguments[0]);
		Array array = Array.CreateInstance(type, 3);
		for (int i = 0; i < 3; i++)
		{
			array.SetValue(Activator.CreateInstance(type), i);
		}
		return array;
	}

	public static OperateResult<string> GetOperateResultJsonFromObj(object obj)
	{
		if (obj is OperateResult operateResult)
		{
			OperateResult<string> operateResult2 = new OperateResult<string>();
			operateResult2.IsSuccess = operateResult.IsSuccess;
			operateResult2.ErrorCode = operateResult.ErrorCode;
			operateResult2.Message = operateResult.Message;
			if (operateResult.IsSuccess)
			{
				PropertyInfo property = obj.GetType().GetProperty("Content");
				if (property != null)
				{
					object value = property.GetValue(obj, null);
					if (value != null)
					{
						operateResult2.Content = value.ToJsonString();
					}
					return operateResult2;
				}
				PropertyInfo property2 = obj.GetType().GetProperty("Content1");
				if (property2 == null)
				{
					return operateResult2;
				}
				PropertyInfo property3 = obj.GetType().GetProperty("Content2");
				if (property3 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				PropertyInfo property4 = obj.GetType().GetProperty("Content3");
				if (property4 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null),
						Content2 = property3.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				PropertyInfo property5 = obj.GetType().GetProperty("Content4");
				if (property5 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null),
						Content2 = property3.GetValue(obj, null),
						Content3 = property4.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				PropertyInfo property6 = obj.GetType().GetProperty("Content5");
				if (property6 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null),
						Content2 = property3.GetValue(obj, null),
						Content3 = property4.GetValue(obj, null),
						Content4 = property5.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				PropertyInfo property7 = obj.GetType().GetProperty("Content6");
				if (property7 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null),
						Content2 = property3.GetValue(obj, null),
						Content3 = property4.GetValue(obj, null),
						Content4 = property5.GetValue(obj, null),
						Content5 = property6.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				PropertyInfo property8 = obj.GetType().GetProperty("Content7");
				if (property8 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null),
						Content2 = property3.GetValue(obj, null),
						Content3 = property4.GetValue(obj, null),
						Content4 = property5.GetValue(obj, null),
						Content5 = property6.GetValue(obj, null),
						Content6 = property7.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				PropertyInfo property9 = obj.GetType().GetProperty("Content8");
				if (property9 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null),
						Content2 = property3.GetValue(obj, null),
						Content3 = property4.GetValue(obj, null),
						Content4 = property5.GetValue(obj, null),
						Content5 = property6.GetValue(obj, null),
						Content6 = property7.GetValue(obj, null),
						Content7 = property8.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				PropertyInfo property10 = obj.GetType().GetProperty("Content9");
				if (property10 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null),
						Content2 = property3.GetValue(obj, null),
						Content3 = property4.GetValue(obj, null),
						Content4 = property5.GetValue(obj, null),
						Content5 = property6.GetValue(obj, null),
						Content6 = property7.GetValue(obj, null),
						Content7 = property8.GetValue(obj, null),
						Content8 = property9.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				PropertyInfo property11 = obj.GetType().GetProperty("Content10");
				if (property11 == null)
				{
					operateResult2.Content = new
					{
						Content1 = property2.GetValue(obj, null),
						Content2 = property3.GetValue(obj, null),
						Content3 = property4.GetValue(obj, null),
						Content4 = property5.GetValue(obj, null),
						Content5 = property6.GetValue(obj, null),
						Content6 = property7.GetValue(obj, null),
						Content7 = property8.GetValue(obj, null),
						Content8 = property9.GetValue(obj, null),
						Content9 = property10.GetValue(obj, null)
					}.ToJsonString();
					return operateResult2;
				}
				operateResult2.Content = new
				{
					Content1 = property2.GetValue(obj, null),
					Content2 = property3.GetValue(obj, null),
					Content3 = property4.GetValue(obj, null),
					Content4 = property5.GetValue(obj, null),
					Content5 = property6.GetValue(obj, null),
					Content6 = property7.GetValue(obj, null),
					Content7 = property8.GetValue(obj, null),
					Content8 = property9.GetValue(obj, null),
					Content9 = property10.GetValue(obj, null),
					Content10 = property11.GetValue(obj, null)
				}.ToJsonString();
				return operateResult2;
			}
			return operateResult2;
		}
		return OperateResult.CreateSuccessResult((obj == null) ? string.Empty : obj.ToJsonString());
	}

	public static List<HslAddressProperty> GetHslPropertyInfos(Type valueType, Type deviceType, object obj, IByteTransform byteTransform)
	{
		List<HslAddressProperty> list = new List<HslAddressProperty>();
		PropertyInfo[] properties = valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		int num = 0;
		PropertyInfo[] array = properties;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo propertyInfo in array2)
		{
			HslDeviceAddressAttribute hslDeviceAddressAttribute = GetHslDeviceAddressAttribute(deviceType, propertyInfo);
			if (hslDeviceAddressAttribute == null)
			{
				continue;
			}
			HslAddressProperty hslAddressProperty = new HslAddressProperty();
			hslAddressProperty.PropertyInfo = propertyInfo;
			hslAddressProperty.DeviceAddressAttribute = hslDeviceAddressAttribute;
			hslAddressProperty.ByteOffset = num;
			Type propertyType = propertyInfo.PropertyType;
			if (propertyType == typeof(byte))
			{
				num++;
				if (obj != null)
				{
					hslAddressProperty.Buffer = new byte[1] { (byte)propertyInfo.GetValue(obj, null) };
				}
			}
			else if (propertyType == typeof(short))
			{
				num += 2;
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((short)propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(short[]))
			{
				num += 2 * ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((short[])propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(ushort))
			{
				num += 2;
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((ushort)propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(ushort[]))
			{
				num += 2 * ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((ushort[])propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(int))
			{
				num += 4;
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((int)propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(int[]))
			{
				num += 4 * ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((int[])propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(uint))
			{
				num += 4;
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((uint)propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(uint[]))
			{
				num += 4 * ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((uint[])propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(long))
			{
				num += 8;
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((long)propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(long[]))
			{
				num += 8 * ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((long[])propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(ulong))
			{
				num += 8;
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((ulong)propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(ulong[]))
			{
				num += 8 * ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((ulong[])propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(float))
			{
				num += 4;
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((float)propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(float[]))
			{
				num += 4 * ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((float[])propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(double))
			{
				num += 8;
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((double)propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(double[]))
			{
				num += 8 * ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((double[])propertyInfo.GetValue(obj, null));
				}
			}
			else if (propertyType == typeof(string))
			{
				num += ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = byteTransform.TransByte((string)propertyInfo.GetValue(obj, null), Encoding.ASCII);
				}
			}
			else if (propertyType == typeof(byte[]))
			{
				num += ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = (byte[])propertyInfo.GetValue(obj, null);
				}
			}
			else if (propertyType == typeof(bool))
			{
				num++;
				if (obj != null)
				{
					hslAddressProperty.Buffer = ((!(bool)propertyInfo.GetValue(obj, null)) ? new byte[1] : new byte[1] { 1 });
				}
			}
			else if (propertyType == typeof(bool[]))
			{
				num += ((hslDeviceAddressAttribute.Length <= 0) ? 1 : hslDeviceAddressAttribute.Length);
				if (obj != null)
				{
					hslAddressProperty.Buffer = ((bool[])propertyInfo.GetValue(obj, null)).Select((bool m) => (byte)(m ? 1u : 0u)).ToArray();
				}
			}
			hslAddressProperty.ByteLength = num - hslAddressProperty.ByteOffset;
			list.Add(hslAddressProperty);
		}
		return list;
	}

	public static void SetPropertyValueFrom(IByteTransform byteTransform, object obj, List<HslAddressProperty> properties, byte[] buffer)
	{
		foreach (HslAddressProperty property in properties)
		{
			Type propertyType = property.PropertyInfo.PropertyType;
			object obj2 = null;
			if (propertyType == typeof(byte))
			{
				obj2 = buffer[property.ByteOffset];
			}
			else if (propertyType == typeof(short))
			{
				obj2 = byteTransform.TransInt16(buffer, property.ByteOffset);
			}
			else if (propertyType == typeof(short[]))
			{
				obj2 = byteTransform.TransInt16(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(ushort))
			{
				obj2 = byteTransform.TransUInt16(buffer, property.ByteOffset);
			}
			else if (propertyType == typeof(ushort[]))
			{
				obj2 = byteTransform.TransUInt16(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(int))
			{
				obj2 = byteTransform.TransInt32(buffer, property.ByteOffset);
			}
			else if (propertyType == typeof(int[]))
			{
				obj2 = byteTransform.TransInt32(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(uint))
			{
				obj2 = byteTransform.TransUInt32(buffer, property.ByteOffset);
			}
			else if (propertyType == typeof(uint[]))
			{
				obj2 = byteTransform.TransUInt32(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(long))
			{
				obj2 = byteTransform.TransInt64(buffer, property.ByteOffset);
			}
			else if (propertyType == typeof(long[]))
			{
				obj2 = byteTransform.TransInt64(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(ulong))
			{
				obj2 = byteTransform.TransUInt64(buffer, property.ByteOffset);
			}
			else if (propertyType == typeof(ulong[]))
			{
				obj2 = byteTransform.TransUInt64(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(float))
			{
				obj2 = byteTransform.TransSingle(buffer, property.ByteOffset);
			}
			else if (propertyType == typeof(float[]))
			{
				obj2 = byteTransform.TransSingle(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(double))
			{
				obj2 = byteTransform.TransDouble(buffer, property.ByteOffset);
			}
			else if (propertyType == typeof(double[]))
			{
				obj2 = byteTransform.TransDouble(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(string))
			{
				obj2 = Encoding.ASCII.GetString(buffer, property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(byte[]))
			{
				obj2 = buffer.SelectMiddle(property.ByteOffset, property.DeviceAddressAttribute.GetDataLength());
			}
			else if (propertyType == typeof(bool))
			{
				obj2 = buffer[property.ByteOffset] != 0;
			}
			else if (propertyType == typeof(bool[]))
			{
				obj2 = (from m in buffer.SelectMiddle(property.ByteOffset, property.DeviceAddressAttribute.GetDataLength())
					select m != 0).ToArray();
			}
			if (obj2 != null)
			{
				property.PropertyInfo.SetValue(obj, obj2, null);
			}
		}
	}
}
