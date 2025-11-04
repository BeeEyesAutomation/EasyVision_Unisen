using System;
using System.Text;

namespace HslCommunication.Core.Address;

public class ModbusAddress : DeviceAddressDataBase
{
	public int Station { get; set; }

	public int Function { get; set; }

	public int WriteFunction { get; set; }

	public ModbusAddress()
	{
		Station = -1;
		Function = -1;
		WriteFunction = -1;
		base.AddressStart = 0;
	}

	public ModbusAddress(string address)
	{
		Station = -1;
		Function = -1;
		WriteFunction = -1;
		base.AddressStart = 0;
		Parse(address, 1);
	}

	public ModbusAddress(string address, byte function)
	{
		Station = -1;
		WriteFunction = -1;
		Function = function;
		base.AddressStart = 0;
		Parse(address, 1);
	}

	public ModbusAddress(string address, byte station, byte function)
	{
		WriteFunction = -1;
		Function = function;
		Station = station;
		base.AddressStart = 0;
		Parse(address, 1);
	}

	public override void Parse(string address, ushort length)
	{
		base.Length = length;
		if (address.IndexOf(';') < 0)
		{
			base.AddressStart = ushort.Parse(address);
			return;
		}
		string[] array = address.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].StartsWith("s=", StringComparison.OrdinalIgnoreCase))
			{
				Station = byte.Parse(array[i].Substring(2));
			}
			else if (array[i].StartsWith("x=", StringComparison.OrdinalIgnoreCase))
			{
				Function = byte.Parse(array[i].Substring(2));
			}
			else if (array[i].StartsWith("w=", StringComparison.OrdinalIgnoreCase))
			{
				WriteFunction = byte.Parse(array[i].Substring(2));
			}
			else
			{
				base.AddressStart = ushort.Parse(array[i]);
			}
		}
	}

	public ModbusAddress AddressAdd(int value)
	{
		return new ModbusAddress
		{
			Station = Station,
			Function = Function,
			WriteFunction = WriteFunction,
			AddressStart = base.AddressStart + value
		};
	}

	public ModbusAddress AddressAdd()
	{
		return AddressAdd(1);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (Station >= 0)
		{
			stringBuilder.Append("s=" + Station + ";");
		}
		if (Function == 2 || Function == 4 || Function > 6)
		{
			stringBuilder.Append("x=" + Function + ";");
		}
		if (WriteFunction > 0)
		{
			stringBuilder.Append("w=" + WriteFunction + ";");
		}
		stringBuilder.Append(base.AddressStart.ToString());
		return stringBuilder.ToString();
	}
}
