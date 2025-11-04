using System;
using HslCommunication.Profinet.Fuji;

namespace HslCommunication.Core.Address;

public class FujiSPBAddress : DeviceAddressDataBase
{
	public string TypeCode { get; set; }

	public int BitIndex { get; set; }

	public string GetWordAddress()
	{
		return TypeCode + FujiSPBHelper.AnalysisIntegerAddress(base.AddressStart);
	}

	public string GetWriteBoolAddress()
	{
		int num = base.AddressStart * 2;
		int num2 = BitIndex;
		if (num2 >= 8)
		{
			num++;
			num2 -= 8;
		}
		return $"{TypeCode}{FujiSPBHelper.AnalysisIntegerAddress(num)}{num2:X2}";
	}

	public int GetBitIndex()
	{
		return base.AddressStart * 16 + BitIndex;
	}

	public static OperateResult<FujiSPBAddress> ParseFrom(string address)
	{
		return ParseFrom(address, 0);
	}

	public static OperateResult<FujiSPBAddress> ParseFrom(string address, ushort length)
	{
		FujiSPBAddress fujiSPBAddress = new FujiSPBAddress();
		fujiSPBAddress.Length = length;
		try
		{
			fujiSPBAddress.BitIndex = HslHelper.GetBitIndexInformation(ref address);
			switch (address[0])
			{
			case 'X':
			case 'x':
				fujiSPBAddress.TypeCode = "01";
				fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'Y':
			case 'y':
				fujiSPBAddress.TypeCode = "00";
				fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'M':
			case 'm':
				fujiSPBAddress.TypeCode = "02";
				fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'L':
			case 'l':
				fujiSPBAddress.TypeCode = "03";
				fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'T':
			case 't':
				if (address[1] == 'N' || address[1] == 'n')
				{
					fujiSPBAddress.TypeCode = "0A";
					fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
					break;
				}
				if (address[1] == 'C' || address[1] == 'c')
				{
					fujiSPBAddress.TypeCode = "04";
					fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
					break;
				}
				throw new Exception(StringResources.Language.NotSupportedDataType);
			case 'C':
			case 'c':
				if (address[1] == 'N' || address[1] == 'n')
				{
					fujiSPBAddress.TypeCode = "0B";
					fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
					break;
				}
				if (address[1] == 'C' || address[1] == 'c')
				{
					fujiSPBAddress.TypeCode = "05";
					fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
					break;
				}
				throw new Exception(StringResources.Language.NotSupportedDataType);
			case 'D':
			case 'd':
				fujiSPBAddress.TypeCode = "0C";
				fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'R':
			case 'r':
				fujiSPBAddress.TypeCode = "0D";
				fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'W':
			case 'w':
				fujiSPBAddress.TypeCode = "0E";
				fujiSPBAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<FujiSPBAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
		return OperateResult.CreateSuccessResult(fujiSPBAddress);
	}
}
