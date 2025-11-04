using System;

namespace HslCommunication.Core.Address;

public class MelsecFxLinksAddress : DeviceAddressDataBase
{
	public string TypeCode { get; set; }

	public override void Parse(string address, ushort length)
	{
		base.Parse(address, length);
	}

	public override string ToString()
	{
		string typeCode = TypeCode;
		string text = typeCode;
		if (text == "X" || text == "Y")
		{
			return TypeCode + Convert.ToString(base.AddressStart, 8).PadLeft((base.AddressStart >= 10000) ? 6 : 4, '0');
		}
		return TypeCode + base.AddressStart.ToString("D" + (((base.AddressStart >= 10000) ? 7 : 5) - TypeCode.Length));
	}

	public static OperateResult<MelsecFxLinksAddress> ParseFrom(string address)
	{
		return ParseFrom(address, 0);
	}

	public static OperateResult<MelsecFxLinksAddress> ParseFrom(string address, ushort length)
	{
		MelsecFxLinksAddress melsecFxLinksAddress = new MelsecFxLinksAddress();
		melsecFxLinksAddress.Length = length;
		try
		{
			switch (address[0])
			{
			case 'X':
			case 'x':
				melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 8);
				melsecFxLinksAddress.TypeCode = "X";
				break;
			case 'Y':
			case 'y':
				melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 8);
				melsecFxLinksAddress.TypeCode = "Y";
				break;
			case 'M':
			case 'm':
				melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				melsecFxLinksAddress.TypeCode = "M";
				break;
			case 'S':
			case 's':
				melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				melsecFxLinksAddress.TypeCode = "S";
				break;
			case 'T':
			case 't':
				if (address[1] == 'S' || address[1] == 's')
				{
					melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
					melsecFxLinksAddress.TypeCode = "TS";
					break;
				}
				if (address[1] == 'N' || address[1] == 'n')
				{
					melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
					melsecFxLinksAddress.TypeCode = "TN";
					break;
				}
				throw new Exception(StringResources.Language.NotSupportedDataType);
			case 'C':
			case 'c':
				if (address[1] == 'S' || address[1] == 's')
				{
					melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
					melsecFxLinksAddress.TypeCode = "CS";
					break;
				}
				if (address[1] == 'N' || address[1] == 'n')
				{
					melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
					melsecFxLinksAddress.TypeCode = "CN";
					break;
				}
				throw new Exception(StringResources.Language.NotSupportedDataType);
			case 'D':
			case 'd':
				melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				melsecFxLinksAddress.TypeCode = "D";
				break;
			case 'R':
			case 'r':
				melsecFxLinksAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				melsecFxLinksAddress.TypeCode = "R";
				break;
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
			return OperateResult.CreateSuccessResult(melsecFxLinksAddress);
		}
		catch (Exception ex)
		{
			return new OperateResult<MelsecFxLinksAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}
}
