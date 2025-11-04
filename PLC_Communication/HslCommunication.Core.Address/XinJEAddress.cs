using System;

namespace HslCommunication.Core.Address;

public class XinJEAddress : DeviceAddressDataBase
{
	public byte DataCode { get; set; }

	public byte Station { get; set; }

	public int CriticalAddress { get; set; }

	public XinJEAddress()
	{
	}

	public XinJEAddress(byte dataCode, int address, int criticalAddress, byte station)
	{
		DataCode = dataCode;
		base.AddressStart = address;
		CriticalAddress = criticalAddress;
		Station = station;
	}

	public override string ToString()
	{
		return base.AddressStart.ToString();
	}

	public static OperateResult<XinJEAddress> ParseFrom(string address, ushort length, byte defaultStation)
	{
		OperateResult<XinJEAddress> operateResult = ParseFrom(address, defaultStation);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<XinJEAddress>(operateResult);
		}
		operateResult.Content.Length = length;
		return OperateResult.CreateSuccessResult(operateResult.Content);
	}

	public static OperateResult<XinJEAddress> ParseFrom(string address, byte defaultStation)
	{
		try
		{
			byte station = (byte)HslHelper.ExtractParameter(ref address, "s", defaultStation);
			if (address.StartsWith("HSCD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(139, int.Parse(address.Substring(4)), int.MaxValue, station));
			}
			if (address.StartsWith("ETD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(133, int.Parse(address.Substring(3)), 0, station));
			}
			if (address.StartsWith("HSD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(140, int.Parse(address.Substring(3)), 1024, station));
			}
			if (address.StartsWith("HTD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(137, int.Parse(address.Substring(3)), 1024, station));
			}
			if (address.StartsWith("HCD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(138, int.Parse(address.Substring(3)), 1024, station));
			}
			if (address.StartsWith("SFD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(142, int.Parse(address.Substring(3)), 4096, station));
			}
			if (address.StartsWith("HSC"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(12, int.Parse(address.Substring(3)), int.MaxValue, station));
			}
			if (address.StartsWith("SD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(131, int.Parse(address.Substring(2)), 4096, station));
			}
			if (address.StartsWith("TD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(129, int.Parse(address.Substring(2)), 4096, station));
			}
			if (address.StartsWith("CD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(130, int.Parse(address.Substring(2)), 4096, station));
			}
			if (address.StartsWith("HD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(136, int.Parse(address.Substring(2)), 6144, station));
			}
			if (address.StartsWith("FD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(141, int.Parse(address.Substring(2)), 8192, station));
			}
			if (address.StartsWith("ID"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(134, int.Parse(address.Substring(2)), 0, station));
			}
			if (address.StartsWith("QD"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(135, int.Parse(address.Substring(2)), 0, station));
			}
			if (address.StartsWith("SM"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(13, int.Parse(address.Substring(2)), 4096, station));
			}
			if (address.StartsWith("ET"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(7, int.Parse(address.Substring(2)), 0, station));
			}
			if (address.StartsWith("HM"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(8, int.Parse(address.Substring(2)), 6144, station));
			}
			if (address.StartsWith("HS"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(9, int.Parse(address.Substring(2)), int.MaxValue, station));
			}
			if (address.StartsWith("HT"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(10, int.Parse(address.Substring(2)), 1024, station));
			}
			if (address.StartsWith("HC"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(11, int.Parse(address.Substring(2)), 1024, station));
			}
			if (address.StartsWith("D"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(128, int.Parse(address.Substring(1)), 20480, station));
			}
			if (address.StartsWith("M"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(3, int.Parse(address.Substring(1)), 20480, station));
			}
			if (address.StartsWith("T"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(5, int.Parse(address.Substring(1)), 4096, station));
			}
			if (address.StartsWith("C"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(6, int.Parse(address.Substring(1)), 4096, station));
			}
			if (address.StartsWith("Y"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(2, Convert.ToInt32(address.Substring(1), 8), int.MaxValue, station));
			}
			if (address.StartsWith("X"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(1, Convert.ToInt32(address.Substring(1), 8), int.MaxValue, station));
			}
			if (address.StartsWith("S"))
			{
				return OperateResult.CreateSuccessResult(new XinJEAddress(4, int.Parse(address.Substring(1)), 8000, station));
			}
			throw new Exception(StringResources.Language.NotSupportedDataType);
		}
		catch (Exception ex)
		{
			return new OperateResult<XinJEAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}
}
