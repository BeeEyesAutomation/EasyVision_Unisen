using System;

namespace HslCommunication.Core.Address;

public class YokogawaLinkAddress : DeviceAddressDataBase
{
	public int DataCode { get; set; }

	public byte[] GetAddressBinaryContent()
	{
		return new byte[6]
		{
			BitConverter.GetBytes(DataCode)[1],
			BitConverter.GetBytes(DataCode)[0],
			BitConverter.GetBytes(base.AddressStart)[3],
			BitConverter.GetBytes(base.AddressStart)[2],
			BitConverter.GetBytes(base.AddressStart)[1],
			BitConverter.GetBytes(base.AddressStart)[0]
		};
	}

	public override void Parse(string address, ushort length)
	{
		OperateResult<YokogawaLinkAddress> operateResult = ParseFrom(address, length);
		if (operateResult.IsSuccess)
		{
			base.AddressStart = operateResult.Content.AddressStart;
			base.Length = operateResult.Content.Length;
			DataCode = operateResult.Content.DataCode;
		}
	}

	public override string ToString()
	{
		int dataCode = DataCode;
		if (1 == 0)
		{
		}
		string result = dataCode switch
		{
			49 => "CN" + base.AddressStart, 
			33 => "TN" + base.AddressStart, 
			24 => "X" + base.AddressStart, 
			25 => "Y" + base.AddressStart, 
			9 => "I" + base.AddressStart, 
			5 => "E" + base.AddressStart, 
			13 => "M" + base.AddressStart, 
			20 => "T" + base.AddressStart, 
			3 => "C" + base.AddressStart, 
			12 => "L" + base.AddressStart, 
			4 => "D" + base.AddressStart, 
			2 => "B" + base.AddressStart, 
			6 => "F" + base.AddressStart, 
			18 => "R" + base.AddressStart, 
			22 => "V" + base.AddressStart, 
			26 => "Z" + base.AddressStart, 
			23 => "W" + base.AddressStart, 
			_ => base.AddressStart.ToString(), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<YokogawaLinkAddress> ParseFrom(string address, ushort length)
	{
		try
		{
			int num = 0;
			int num2 = 0;
			if (address.StartsWith("CN") || address.StartsWith("cn"))
			{
				num = 49;
				num2 = int.Parse(address.Substring(2));
			}
			else if (address.StartsWith("TN") || address.StartsWith("tn"))
			{
				num = 33;
				num2 = int.Parse(address.Substring(2));
			}
			else if (address.StartsWith("X") || address.StartsWith("x"))
			{
				num = 24;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("Y") || address.StartsWith("y"))
			{
				num = 25;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("I") || address.StartsWith("i"))
			{
				num = 9;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("E") || address.StartsWith("e"))
			{
				num = 5;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("M") || address.StartsWith("m"))
			{
				num = 13;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("T") || address.StartsWith("t"))
			{
				num = 20;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("C") || address.StartsWith("c"))
			{
				num = 3;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("L") || address.StartsWith("l"))
			{
				num = 12;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("D") || address.StartsWith("d"))
			{
				num = 4;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("B") || address.StartsWith("b"))
			{
				num = 2;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("F") || address.StartsWith("f"))
			{
				num = 6;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("R") || address.StartsWith("r"))
			{
				num = 18;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("V") || address.StartsWith("v"))
			{
				num = 22;
				num2 = int.Parse(address.Substring(1));
			}
			else if (address.StartsWith("Z") || address.StartsWith("z"))
			{
				num = 26;
				num2 = int.Parse(address.Substring(1));
			}
			else
			{
				if (!address.StartsWith("W") && !address.StartsWith("w"))
				{
					throw new Exception(StringResources.Language.NotSupportedDataType);
				}
				num = 23;
				num2 = int.Parse(address.Substring(1));
			}
			return OperateResult.CreateSuccessResult(new YokogawaLinkAddress
			{
				DataCode = num,
				AddressStart = num2,
				Length = length
			});
		}
		catch (Exception ex)
		{
			return new OperateResult<YokogawaLinkAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}
}
