using System;
using System.Collections.Generic;
using HslCommunication.Core;

namespace HslCommunication.Profinet.Siemens;

public class SiemensDateTime
{
	public static readonly DateTime SpecMinimumDateTime = new DateTime(1990, 1, 1);

	public static readonly DateTime SpecMaximumDateTime = new DateTime(2089, 12, 31, 23, 59, 59, 999);

	public static OperateResult<DateTime> FromByteArray(byte[] bytes)
	{
		try
		{
			return OperateResult.CreateSuccessResult(FromByteArrayImpl(bytes));
		}
		catch (Exception ex)
		{
			return new OperateResult<DateTime>("Prase DateTime failed: " + ex.Message);
		}
	}

	public static OperateResult<DateTime> GetDTLTime(IByteTransform byteTransform, byte[] buffer, int index)
	{
		try
		{
			int year = byteTransform.TransInt16(buffer, index);
			int month = buffer[index + 2];
			int day = buffer[index + 3];
			int hour = buffer[index + 5];
			int minute = buffer[index + 6];
			int second = buffer[index + 7];
			int millisecond = byteTransform.TransInt32(buffer, index + 8) / 1000 / 1000;
			return OperateResult.CreateSuccessResult(new DateTime(year, month, day, hour, minute, second, millisecond));
		}
		catch (Exception ex)
		{
			return new OperateResult<DateTime>("GetDTLTime failed: " + ex.Message);
		}
	}

	public static byte[] GetBytesFromDTLTime(IByteTransform byteTransform, DateTime dateTime)
	{
		byte[] array = new byte[12];
		byteTransform.TransByte((short)dateTime.Year).CopyTo(array, 0);
		array[2] = (byte)dateTime.Month;
		array[3] = (byte)dateTime.Day;
		array[4] = 5;
		array[5] = (byte)dateTime.Hour;
		array[6] = (byte)dateTime.Minute;
		array[7] = (byte)dateTime.Second;
		byteTransform.TransByte(dateTime.Millisecond * 1000 * 1000).CopyTo(array, 8);
		return array;
	}

	public static DateTime[] ToArray(byte[] bytes)
	{
		if (bytes.Length % 8 != 0)
		{
			throw new ArgumentOutOfRangeException("bytes", bytes.Length, $"Parsing an array of DateTime requires a multiple of 8 bytes of input data, input data is '{bytes.Length}' long.");
		}
		int num = bytes.Length / 8;
		DateTime[] array = new DateTime[bytes.Length / 8];
		for (int i = 0; i < num; i++)
		{
			array[i] = FromByteArrayImpl(new ArraySegment<byte>(bytes, i * 8, 8).Array);
		}
		return array;
	}

	private static DateTime FromByteArrayImpl(IList<byte> bytes)
	{
		if (bytes.Count != 8)
		{
			throw new ArgumentOutOfRangeException("bytes", bytes.Count, $"Parsing a DateTime requires exactly 8 bytes of input data, input data is {bytes.Count} bytes long.");
		}
		int year = ByteToYear(bytes[0]);
		int month = AssertRangeInclusive(DecodeBcd(bytes[1]), 1, 12, "month");
		int day = AssertRangeInclusive(DecodeBcd(bytes[2]), 1, 31, "day of month");
		int hour = AssertRangeInclusive(DecodeBcd(bytes[3]), 0, 23, "hour");
		int minute = AssertRangeInclusive(DecodeBcd(bytes[4]), 0, 59, "minute");
		int second = AssertRangeInclusive(DecodeBcd(bytes[5]), 0, 59, "second");
		int num = AssertRangeInclusive(DecodeBcd(bytes[6]), 0, 99, "first two millisecond digits");
		int num2 = AssertRangeInclusive(bytes[7] >> 4, 0, 9, "third millisecond digit");
		int num3 = AssertRangeInclusive(bytes[7] & 0xF, 1, 7, "day of week");
		return new DateTime(year, month, day, hour, minute, second, num * 10 + num2);
		static int AssertRangeInclusive(int input, byte min, byte max, string field)
		{
			if (input < min)
			{
				throw new ArgumentOutOfRangeException("input", input, $"Value '{input}' is lower than the minimum '{min}' allowed for {field}.");
			}
			if (input > max)
			{
				throw new ArgumentOutOfRangeException("input", input, $"Value '{input}' is higher than the maximum '{max}' allowed for {field}.");
			}
			return input;
		}
		static int ByteToYear(byte bcdYear)
		{
			int num4 = DecodeBcd(bcdYear);
			if (num4 < 90)
			{
				return num4 + 2000;
			}
			if (num4 >= 100)
			{
				throw new ArgumentOutOfRangeException("bcdYear", bcdYear, $"Value '{num4}' is higher than the maximum '99' of S7 date and time representation.");
			}
			return num4 + 1900;
		}
		static int DecodeBcd(byte input)
		{
			return 10 * (input >> 4) + (input & 0xF);
		}
	}

	public static byte[] ToByteArray(DateTime dateTime)
	{
		if (dateTime < SpecMinimumDateTime)
		{
			throw new ArgumentOutOfRangeException("dateTime", dateTime, $"Date time '{dateTime}' is before the minimum '{SpecMinimumDateTime}' supported in S7 date time representation.");
		}
		if (dateTime > SpecMaximumDateTime)
		{
			throw new ArgumentOutOfRangeException("dateTime", dateTime, $"Date time '{dateTime}' is after the maximum '{SpecMaximumDateTime}' supported in S7 date time representation.");
		}
		return new byte[8]
		{
			EncodeBcd(MapYear(dateTime.Year)),
			EncodeBcd(dateTime.Month),
			EncodeBcd(dateTime.Day),
			EncodeBcd(dateTime.Hour),
			EncodeBcd(dateTime.Minute),
			EncodeBcd(dateTime.Second),
			EncodeBcd(dateTime.Millisecond / 10),
			(byte)((dateTime.Millisecond % 10 << 4) | DayOfWeekToInt(dateTime.DayOfWeek))
		};
		static int DayOfWeekToInt(DayOfWeek dayOfWeek)
		{
			return (int)(dayOfWeek + 1);
		}
		static byte EncodeBcd(int value)
		{
			return (byte)((value / 10 << 4) | (value % 10));
		}
		static byte MapYear(int year)
		{
			return (byte)((year < 2000) ? (year - 1900) : (year - 2000));
		}
	}

	public static byte[] ToByteArray(DateTime[] dateTimes)
	{
		List<byte> list = new List<byte>(dateTimes.Length * 8);
		foreach (DateTime dateTime in dateTimes)
		{
			list.AddRange(ToByteArray(dateTime));
		}
		return list.ToArray();
	}
}
