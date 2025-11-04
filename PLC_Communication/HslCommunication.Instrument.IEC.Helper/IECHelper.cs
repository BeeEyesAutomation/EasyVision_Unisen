using System;
using System.Collections.Generic;
using System.IO;

namespace HslCommunication.Instrument.IEC.Helper;

public class IECHelper
{
	public const byte IEC104ControlStartDT = 7;

	public const byte IEC104ControlStopDT = 19;

	public const byte IEC104ControlTestFR = 67;

	public static byte[] PackIEC104Message(byte controlField1, byte controlField2, byte controlField3, byte controlField4, byte[] asdu)
	{
		byte[] array = new byte[6 + ((asdu != null) ? asdu.Length : 0)];
		array[0] = 104;
		array[1] = (byte)(array.Length - 2);
		array[2] = controlField1;
		array[3] = controlField2;
		array[4] = controlField3;
		array[5] = controlField4;
		if (asdu != null && asdu.Length != 0)
		{
			asdu.CopyTo(array, 6);
		}
		return array;
	}

	internal static byte[] PackIEC104Message(ushort controlField1, ushort controlField2, byte[] asdu)
	{
		return PackIEC104Message(BitConverter.GetBytes(controlField1)[0], BitConverter.GetBytes(controlField1)[1], BitConverter.GetBytes(controlField2)[0], BitConverter.GetBytes(controlField2)[1], asdu);
	}

	public static byte[] GetAbsoluteTimeScale(DateTime dateTime, bool valid)
	{
		byte[] array = new byte[7]
		{
			BitConverter.GetBytes(dateTime.Millisecond + dateTime.Second * 1000)[0],
			BitConverter.GetBytes(dateTime.Millisecond + dateTime.Second * 1000)[1],
			BitConverter.GetBytes(dateTime.Minute)[0],
			0,
			0,
			0,
			0
		};
		if (!valid)
		{
			array[2] = (byte)(array[2] | 0x80);
		}
		array[3] = BitConverter.GetBytes(dateTime.Hour)[0];
		int num = 1;
		switch (dateTime.DayOfWeek)
		{
		case DayOfWeek.Monday:
			num = 1;
			break;
		case DayOfWeek.Tuesday:
			num = 2;
			break;
		case DayOfWeek.Wednesday:
			num = 3;
			break;
		case DayOfWeek.Thursday:
			num = 4;
			break;
		case DayOfWeek.Friday:
			num = 5;
			break;
		case DayOfWeek.Saturday:
			num = 6;
			break;
		case DayOfWeek.Sunday:
			num = 7;
			break;
		}
		array[4] = BitConverter.GetBytes(dateTime.Day + num * 32)[0];
		array[5] = BitConverter.GetBytes(dateTime.Month)[0];
		array[6] = BitConverter.GetBytes(dateTime.Year - 2000)[0];
		return array;
	}

	public static DateTime PraseTimeFromAbsoluteTimeScale(byte[] source, int index)
	{
		int year = (source[index + 6] & 0x7F) + 2000;
		int month = source[index + 5] & 0xF;
		int day = source[index + 4] & 0x1F;
		int hour = source[index + 3] & 0x1F;
		int minute = source[index + 2] & 0x3F;
		int num = BitConverter.ToUInt16(source, index);
		return new DateTime(year, month, day, hour, minute, num / 1000, num % 1000);
	}

	public static byte[] BuildFrameSMessage(int receiveID)
	{
		receiveID *= 2;
		return PackIEC104Message(1, (ushort)receiveID, null);
	}

	public static byte[] BuildFrameUMessage(byte controlField)
	{
		return PackIEC104Message(controlField, 0, 0, 0, null);
	}

	public static byte[] BuildFrameIMessage(int sendID, int receiveID, byte typeId, byte variableStructureQualifier, ushort reason, ushort station, byte[] body)
	{
		sendID *= 2;
		receiveID *= 2;
		byte[] array = new byte[6 + ((body != null) ? body.Length : 0)];
		array[0] = typeId;
		array[1] = variableStructureQualifier;
		array[2] = BitConverter.GetBytes(reason)[0];
		array[3] = BitConverter.GetBytes(reason)[1];
		array[4] = BitConverter.GetBytes(station)[0];
		array[5] = BitConverter.GetBytes(station)[1];
		if (body != null && body.Length != 0)
		{
			body.CopyTo(array, 6);
		}
		return PackIEC104Message((ushort)sendID, (ushort)receiveID, array);
	}

	public static byte[] BuildWriteIec(byte type, ushort reason, ushort station, ushort address, byte[] value)
	{
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(type);
		memoryStream.WriteByte(1);
		memoryStream.Write(BitConverter.GetBytes(reason));
		memoryStream.Write(BitConverter.GetBytes(station));
		memoryStream.Write(BitConverter.GetBytes(address));
		memoryStream.WriteByte(0);
		memoryStream.Write(value);
		if (type == 45 || type == 46 || type == 47)
		{
			return memoryStream.ToArray();
		}
		if (type == 48)
		{
			return memoryStream.ToArray();
		}
		memoryStream.WriteByte(0);
		return memoryStream.ToArray();
	}

	public static List<IecValueObject<T>> ParseYaoCeValue<T>(IEC104MessageEventArgs message, Func<byte[], int, T> trans, int unitLength)
	{
		bool flag = message.TypeID >= 30;
		List<IecValueObject<T>> list = new List<IecValueObject<T>>();
		DateTime time = DateTime.MinValue;
		int num = ((message.TypeID == 21) ? 3 : 4);
		if (message.IsAddressContinuous)
		{
			if (message.TypeID != 21)
			{
				unitLength++;
			}
			if (flag && message.Body.Length >= message.InfoObjectCount * unitLength + 3 + 7)
			{
				time = PraseTimeFromAbsoluteTimeScale(message.Body, message.InfoObjectCount * unitLength + 3);
			}
			ushort num2 = BitConverter.ToUInt16(message.Body, 0);
			for (int i = 0; i < message.InfoObjectCount; i++)
			{
				int num3 = unitLength * i + 3;
				if (num3 >= message.Body.Length)
				{
					return list;
				}
				IecValueObject<T> iecValueObject = new IecValueObject<T>();
				iecValueObject.Address = num2 + i;
				iecValueObject.Value = trans(message.Body, num3);
				if (message.TypeID != 21)
				{
					iecValueObject.Quality = message.Body[num3 + unitLength - 1];
				}
				if (flag)
				{
					iecValueObject.Time = time;
				}
				list.Add(iecValueObject);
			}
		}
		else
		{
			if (flag && message.Body.Length >= message.InfoObjectCount * (num + unitLength) + 7)
			{
				time = PraseTimeFromAbsoluteTimeScale(message.Body, message.InfoObjectCount * (num + unitLength));
			}
			for (int j = 0; j < message.InfoObjectCount; j++)
			{
				int num4 = (num + unitLength) * j;
				if (num4 >= message.Body.Length)
				{
					return list;
				}
				IecValueObject<T> iecValueObject2 = new IecValueObject<T>();
				iecValueObject2.Address = BitConverter.ToUInt16(message.Body, num4);
				iecValueObject2.Value = trans(message.Body, num4 + 3);
				if (message.TypeID != 21)
				{
					iecValueObject2.Quality = message.Body[num4 + 3 + unitLength];
				}
				if (flag)
				{
					iecValueObject2.Time = time;
				}
				list.Add(iecValueObject2);
			}
		}
		return list;
	}
}
