using System;

namespace HslCommunication.Core.Address;

public class FujiSPHAddress : DeviceAddressDataBase
{
	public byte TypeCode { get; set; }

	public int BitIndex { get; set; }

	public static OperateResult<FujiSPHAddress> ParseFrom(string address)
	{
		return ParseFrom(address, 0);
	}

	public static OperateResult<FujiSPHAddress> ParseFrom(string address, ushort length)
	{
		FujiSPHAddress fujiSPHAddress = new FujiSPHAddress();
		try
		{
			switch (address[0])
			{
			case 'M':
			case 'm':
			{
				string[] array2 = address.SplitDot();
				switch (int.Parse(array2[0].Substring(1)))
				{
				case 1:
					fujiSPHAddress.TypeCode = 2;
					break;
				case 3:
					fujiSPHAddress.TypeCode = 4;
					break;
				case 10:
					fujiSPHAddress.TypeCode = 8;
					break;
				default:
					throw new Exception(StringResources.Language.NotSupportedDataType);
				}
				fujiSPHAddress.AddressStart = Convert.ToInt32(array2[1]);
				if (array2.Length > 2)
				{
					fujiSPHAddress.BitIndex = HslHelper.CalculateBitStartIndex(array2[2]);
				}
				break;
			}
			case 'I':
			case 'Q':
			case 'i':
			case 'q':
			{
				string[] array = address.SplitDot();
				fujiSPHAddress.TypeCode = 1;
				fujiSPHAddress.AddressStart = Convert.ToInt32(array[0].Substring(1));
				if (array.Length > 1)
				{
					fujiSPHAddress.BitIndex = HslHelper.CalculateBitStartIndex(array[1]);
				}
				break;
			}
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<FujiSPHAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
		return OperateResult.CreateSuccessResult(fujiSPHAddress);
	}
}
