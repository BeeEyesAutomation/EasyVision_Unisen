using System;

namespace HslCommunication.Core.Address;

public class VigorAddress : DeviceAddressDataBase
{
	public byte DataCode { get; set; }

	public override string ToString()
	{
		return base.AddressStart.ToString();
	}

	public static OperateResult<VigorAddress> ParseFrom(string address, ushort length, bool isBit)
	{
		VigorAddress vigorAddress = new VigorAddress();
		try
		{
			vigorAddress.Length = length;
			if (isBit)
			{
				if (address.StartsWith("SM") || address.StartsWith("sm"))
				{
					vigorAddress.DataCode = 148;
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(2));
				}
				else if (address.StartsWith("TS") || address.StartsWith("ts"))
				{
					vigorAddress.DataCode = 153;
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(2));
				}
				else if (address.StartsWith("TC") || address.StartsWith("tc"))
				{
					vigorAddress.DataCode = 152;
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(2));
				}
				else if (address.StartsWith("CS") || address.StartsWith("cs"))
				{
					vigorAddress.DataCode = 157;
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(2));
				}
				else if (address.StartsWith("CC") || address.StartsWith("cc"))
				{
					vigorAddress.DataCode = 156;
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(2));
				}
				else if (address.StartsWith("X") || address.StartsWith("x"))
				{
					vigorAddress.DataCode = 144;
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(1));
				}
				else if (address.StartsWith("Y") || address.StartsWith("y"))
				{
					vigorAddress.DataCode = 145;
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(1));
				}
				else if (address.StartsWith("M") || address.StartsWith("m"))
				{
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(1));
					if (vigorAddress.AddressStart >= 9000)
					{
						vigorAddress.AddressStart = 0;
						vigorAddress.DataCode = 148;
					}
					else
					{
						vigorAddress.DataCode = 146;
					}
				}
				else
				{
					if (!address.StartsWith("S") && !address.StartsWith("s"))
					{
						return new OperateResult<VigorAddress>(StringResources.Language.NotSupportedDataType);
					}
					vigorAddress.DataCode = 147;
					vigorAddress.AddressStart = Convert.ToInt32(address.Substring(1));
				}
			}
			else if (address.StartsWith("SD") || address.StartsWith("sd"))
			{
				vigorAddress.DataCode = 161;
				vigorAddress.AddressStart = Convert.ToInt32(address.Substring(2));
			}
			else if (address.StartsWith("D") || address.StartsWith("d"))
			{
				vigorAddress.AddressStart = Convert.ToInt32(address.Substring(1));
				if (vigorAddress.AddressStart >= 9000)
				{
					vigorAddress.DataCode = 161;
					vigorAddress.AddressStart -= 9000;
				}
				else
				{
					vigorAddress.DataCode = 160;
				}
			}
			else if (address.StartsWith("R") || address.StartsWith("r"))
			{
				vigorAddress.DataCode = 162;
				vigorAddress.AddressStart = Convert.ToInt32(address.Substring(1));
			}
			else if (address.StartsWith("T") || address.StartsWith("t"))
			{
				vigorAddress.DataCode = 168;
				vigorAddress.AddressStart = Convert.ToInt32(address.Substring(1));
			}
			else
			{
				if (!address.StartsWith("C") && !address.StartsWith("c"))
				{
					throw new Exception(StringResources.Language.NotSupportedDataType);
				}
				vigorAddress.AddressStart = Convert.ToInt32(address.Substring(1));
				if (vigorAddress.AddressStart >= 200)
				{
					vigorAddress.DataCode = 173;
				}
				else
				{
					vigorAddress.DataCode = 172;
				}
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<VigorAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
		return OperateResult.CreateSuccessResult(vigorAddress);
	}
}
