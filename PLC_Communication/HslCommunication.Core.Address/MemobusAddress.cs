using System;

namespace HslCommunication.Core.Address;

public class MemobusAddress : DeviceAddressDataBase
{
	public byte SFC { get; set; }

	public byte MFC { get; set; }

	public override string ToString()
	{
		return base.AddressStart.ToString();
	}

	public static OperateResult<MemobusAddress> ParseFrom(string address, bool isBit)
	{
		try
		{
			MemobusAddress memobusAddress = new MemobusAddress();
			memobusAddress.MFC = (byte)HslHelper.ExtractParameter(ref address, "mfc", 32);
			memobusAddress.SFC = (byte)HslHelper.ExtractParameter(ref address, "x", isBit ? 1 : 3);
			if (address.IndexOf('.') > 0)
			{
				int num = address.IndexOf('.');
				memobusAddress.AddressStart = Convert.ToInt32(address.Substring(0, num)) * 16 + HslHelper.CalculateBitStartIndex(address.Substring(num + 1));
				if (memobusAddress.SFC == 1)
				{
					memobusAddress.SFC = 3;
				}
			}
			else
			{
				memobusAddress.AddressStart = ushort.Parse(address);
			}
			return OperateResult.CreateSuccessResult(memobusAddress);
		}
		catch (Exception ex)
		{
			return new OperateResult<MemobusAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}
}
