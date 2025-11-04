using System;
using System.Collections.Generic;
using System.IO;

namespace HslCommunication.Instrument.IEC;

public class IecValueServerObject<T>
{
	private Func<IecValueObject<T>, byte[]> _trans;

	private byte _typeID;

	public bool IsAddressContinuous { get; private set; }

	public byte TypeID => _typeID;

	public List<IecValueObject<T>> Values { get; private set; }

	public Action<IecValueServerObject<T>, IecValueObject<T>> OnIecValueChanged { get; set; }

	public IecValueServerObject(List<IecValueObject<T>> value, byte typeID, Func<IecValueObject<T>, byte[]> trans)
	{
		_typeID = typeID;
		_trans = trans;
		SetValues(value);
	}

	public IecValueServerObject(ushort address, int length, byte typeID, Func<IecValueObject<T>, byte[]> trans)
	{
		_typeID = typeID;
		_trans = trans;
		SetValues(address, length);
	}

	public void SetValues(List<IecValueObject<T>> values)
	{
		Values = values;
		IsAddressContinuous = false;
		values.ForEach(delegate(IecValueObject<T> m)
		{
			m.OnValueChanged = Iec104ValueChanged;
		});
	}

	public void SetValues(ushort address, int length)
	{
		Values = new List<IecValueObject<T>>();
		for (int i = 0; i < length; i++)
		{
			IecValueObject<T> iecValueObject = new IecValueObject<T>();
			iecValueObject.Address = address + i;
			iecValueObject.OnValueChanged = Iec104ValueChanged;
			Values.Add(iecValueObject);
		}
		IsAddressContinuous = true;
	}

	private List<byte[]> GetAsduCommandByList(List<IecValueObject<T>> iecValues, bool isAddressContinuous, byte reason, byte station)
	{
		List<byte[]> list = new List<byte[]>();
		int i = 0;
		int num = _trans(iecValues[0]).Length + ((!isAddressContinuous) ? 3 : 0);
		if (_typeID != 1 && _typeID != 3)
		{
			num++;
		}
		if (isAddressContinuous)
		{
			int num3;
			for (; i < iecValues.Count; i += num3)
			{
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.WriteByte(_typeID);
				int num2 = 128;
				int val = 240 / num;
				num3 = Math.Min(iecValues.Count - i, val);
				if (num3 > 127)
				{
					num3 = 127;
				}
				num2 |= num3;
				memoryStream.WriteByte((byte)num2);
				memoryStream.WriteByte(reason);
				memoryStream.WriteByte(0);
				memoryStream.WriteByte(station);
				memoryStream.WriteByte(0);
				memoryStream.Write(BitConverter.GetBytes((ushort)iecValues[i].Address));
				memoryStream.WriteByte(0);
				for (int j = 0; j < num3; j++)
				{
					memoryStream.Write(_trans(iecValues[j + i]));
					if (_typeID != 1 && _typeID != 3)
					{
						memoryStream.WriteByte(iecValues[j + i].Quality);
					}
				}
				list.Add(memoryStream.ToArray());
			}
		}
		else
		{
			int num5;
			for (; i < iecValues.Count; i += num5)
			{
				MemoryStream memoryStream2 = new MemoryStream();
				memoryStream2.WriteByte(_typeID);
				int num4 = 0;
				int val2 = 243 / num;
				num5 = Math.Min(iecValues.Count - i, val2);
				if (num5 > 127)
				{
					num5 = 127;
				}
				num4 |= num5;
				memoryStream2.WriteByte((byte)num4);
				memoryStream2.WriteByte(reason);
				memoryStream2.WriteByte(0);
				memoryStream2.WriteByte(station);
				memoryStream2.WriteByte(0);
				for (int k = 0; k < num5; k++)
				{
					memoryStream2.Write(BitConverter.GetBytes((ushort)iecValues[k + i].Address));
					memoryStream2.WriteByte(0);
					memoryStream2.Write(_trans(iecValues[k + i]));
					if (_typeID != 1 && _typeID != 3)
					{
						memoryStream2.WriteByte(iecValues[k + i].Quality);
					}
				}
				list.Add(memoryStream2.ToArray());
			}
		}
		return list;
	}

	public List<byte[]> GetAsduCommand(byte reason, byte station)
	{
		return GetAsduCommandByList(Values, IsAddressContinuous, reason, station);
	}

	public byte[] GetAsduBreakOut(IecValueObject<T> iecValue, byte station)
	{
		return GetAsduCommandByList(new List<IecValueObject<T>> { iecValue }, isAddressContinuous: false, 3, station)[0];
	}

	private void Iec104ValueChanged(IecValueObject<T> iecValue)
	{
		OnIecValueChanged?.Invoke(this, iecValue);
	}
}
