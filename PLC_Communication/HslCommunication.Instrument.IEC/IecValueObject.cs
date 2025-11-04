using System;
using System.Collections.Generic;
using HslCommunication.Instrument.IEC.Helper;

namespace HslCommunication.Instrument.IEC;

public class IecValueObject<T>
{
	private T m_value;

	public T Value
	{
		get
		{
			return m_value;
		}
		set
		{
			bool flag = false;
			ref T value2 = ref m_value;
			object obj = value;
			if (!value2.Equals(obj))
			{
				flag = true;
			}
			m_value = value;
			if (flag)
			{
				OnValueChanged?.Invoke(this);
			}
		}
	}

	public byte Quality { get; set; }

	public DateTime Time { get; set; }

	public int Address { get; set; }

	public Action<IecValueObject<T>> OnValueChanged { get; set; }

	public object Tag { get; set; }

	public static List<IecValueObject<byte>> ParseYaoXinValue(IEC104MessageEventArgs message)
	{
		bool flag = message.WithTimeInfo();
		List<IecValueObject<byte>> list = new List<IecValueObject<byte>>();
		DateTime time = DateTime.MinValue;
		if (message.IsAddressContinuous)
		{
			if (flag && message.Body.Length >= message.InfoObjectCount + 3 + 7)
			{
				time = IECHelper.PraseTimeFromAbsoluteTimeScale(message.Body, message.InfoObjectCount + 3);
			}
			ushort num = BitConverter.ToUInt16(message.Body, 0);
			for (int i = 0; i < message.InfoObjectCount; i++)
			{
				int num2 = 3 + i;
				if (num2 >= message.Body.Length)
				{
					return list;
				}
				IecValueObject<byte> iecValueObject = new IecValueObject<byte>();
				iecValueObject.Address = num + i;
				iecValueObject.Value = (byte)(message.Body[num2] & 0xF);
				iecValueObject.Quality = (byte)(message.Body[num2] & 0xF0);
				if (flag)
				{
					iecValueObject.Time = time;
				}
				list.Add(iecValueObject);
			}
		}
		else
		{
			if (flag && message.Body.Length >= message.InfoObjectCount * 4 + 7)
			{
				time = IECHelper.PraseTimeFromAbsoluteTimeScale(message.Body, message.InfoObjectCount * 4);
			}
			for (int j = 0; j < message.InfoObjectCount; j++)
			{
				int num3 = 4 * j;
				if (num3 >= message.Body.Length)
				{
					return list;
				}
				IecValueObject<byte> iecValueObject2 = new IecValueObject<byte>();
				iecValueObject2.Address = BitConverter.ToUInt16(message.Body, num3);
				iecValueObject2.Value = (byte)(message.Body[num3 + 3] & 0xF);
				iecValueObject2.Quality = (byte)(message.Body[num3 + 3] & 0xF0);
				if (flag)
				{
					iecValueObject2.Time = time;
				}
				list.Add(iecValueObject2);
			}
		}
		return list;
	}

	public static List<IecValueObject<short>> ParseInt16Value(IEC104MessageEventArgs iEC104Message)
	{
		return IECHelper.ParseYaoCeValue(iEC104Message, BitConverter.ToInt16, 2);
	}

	public static List<IecValueObject<float>> ParseFloatValue(IEC104MessageEventArgs iEC104Message)
	{
		return IECHelper.ParseYaoCeValue(iEC104Message, BitConverter.ToSingle, 4);
	}

	public static List<IecValueObject<uint>> ParseUInt32Value(IEC104MessageEventArgs iEC104Message)
	{
		return IECHelper.ParseYaoCeValue(iEC104Message, BitConverter.ToUInt32, 4);
	}
}
