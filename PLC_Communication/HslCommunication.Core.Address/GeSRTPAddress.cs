using System;

namespace HslCommunication.Core.Address;

public class GeSRTPAddress : DeviceAddressDataBase
{
	public byte DataCode { get; set; }

	public override void Parse(string address, ushort length)
	{
		OperateResult<GeSRTPAddress> operateResult = ParseFrom(address, length, isBit: false);
		if (operateResult.IsSuccess)
		{
			base.AddressStart = operateResult.Content.AddressStart;
			base.Length = operateResult.Content.Length;
			DataCode = operateResult.Content.DataCode;
		}
	}

	public static OperateResult<GeSRTPAddress> ParseFrom(string address, bool isBit)
	{
		return ParseFrom(address, 0, isBit);
	}

	public static OperateResult<GeSRTPAddress> ParseFrom(string address, ushort length, bool isBit)
	{
		GeSRTPAddress geSRTPAddress = new GeSRTPAddress();
		try
		{
			geSRTPAddress.Length = length;
			if (address.StartsWith("AI") || address.StartsWith("ai"))
			{
				if (isBit)
				{
					return new OperateResult<GeSRTPAddress>(StringResources.Language.GeSRTPNotSupportBitReadWrite);
				}
				geSRTPAddress.DataCode = 10;
				geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
			}
			else if (address.StartsWith("AQ") || address.StartsWith("aq"))
			{
				if (isBit)
				{
					return new OperateResult<GeSRTPAddress>(StringResources.Language.GeSRTPNotSupportBitReadWrite);
				}
				geSRTPAddress.DataCode = 12;
				geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
			}
			else if (address.StartsWith("R") || address.StartsWith("r"))
			{
				if (isBit)
				{
					return new OperateResult<GeSRTPAddress>(StringResources.Language.GeSRTPNotSupportBitReadWrite);
				}
				geSRTPAddress.DataCode = 8;
				geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(1));
			}
			else if (address.StartsWith("SA") || address.StartsWith("sa"))
			{
				geSRTPAddress.DataCode = (byte)(isBit ? 78u : 24u);
				geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
			}
			else if (address.StartsWith("SB") || address.StartsWith("sb"))
			{
				geSRTPAddress.DataCode = (byte)(isBit ? 80u : 26u);
				geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
			}
			else if (address.StartsWith("SC") || address.StartsWith("sc"))
			{
				geSRTPAddress.DataCode = (byte)(isBit ? 82u : 28u);
				geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(2));
			}
			else
			{
				if (address[0] == 'I' || address[0] == 'i')
				{
					geSRTPAddress.DataCode = (byte)(isBit ? 70u : 16u);
				}
				else if (address[0] == 'Q' || address[0] == 'q')
				{
					geSRTPAddress.DataCode = (byte)(isBit ? 72u : 18u);
				}
				else if (address[0] == 'M' || address[0] == 'm')
				{
					geSRTPAddress.DataCode = (byte)(isBit ? 76u : 22u);
				}
				else if (address[0] == 'T' || address[0] == 't')
				{
					geSRTPAddress.DataCode = (byte)(isBit ? 74u : 20u);
				}
				else if (address[0] == 'S' || address[0] == 's')
				{
					geSRTPAddress.DataCode = (byte)(isBit ? 84u : 30u);
				}
				else
				{
					if (address[0] != 'G' && address[0] != 'g')
					{
						throw new Exception(StringResources.Language.NotSupportedDataType);
					}
					geSRTPAddress.DataCode = (byte)(isBit ? 86u : 56u);
				}
				geSRTPAddress.AddressStart = Convert.ToInt32(address.Substring(1));
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<GeSRTPAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
		if (geSRTPAddress.AddressStart == 0)
		{
			return new OperateResult<GeSRTPAddress>(StringResources.Language.GeSRTPAddressCannotBeZero);
		}
		if (geSRTPAddress.AddressStart > 0)
		{
			geSRTPAddress.AddressStart--;
		}
		return OperateResult.CreateSuccessResult(geSRTPAddress);
	}
}
