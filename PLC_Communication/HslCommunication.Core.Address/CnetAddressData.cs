using System;

namespace HslCommunication.Core.Address;

public class CnetAddressData : DeviceAddressDataBase
{
	public string DataCode { get; set; }

	public string DataType { get; set; }

	public static OperateResult<CnetAddressData> ParseFrom(string address)
	{
		return ParseFrom(address, 0);
	}

	public static OperateResult<CnetAddressData> ParseFrom(string address, ushort length)
	{
		CnetAddressData cnetAddressData = new CnetAddressData();
		try
		{
			cnetAddressData.Length = length;
			if (address[1] == 'X')
			{
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<CnetAddressData>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
		return OperateResult.CreateSuccessResult(cnetAddressData);
	}
}
