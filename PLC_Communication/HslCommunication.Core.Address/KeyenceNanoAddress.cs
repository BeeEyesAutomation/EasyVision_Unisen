using System;

namespace HslCommunication.Core.Address;

public class KeyenceNanoAddress : DeviceAddressDataBase
{
	public string DataCode { get; set; }

	public int SplitLength { get; set; }

	public KeyenceNanoAddress()
	{
	}

	public KeyenceNanoAddress(string dataCode, int address, int splits)
	{
		DataCode = dataCode;
		base.AddressStart = address;
		SplitLength = splits;
	}

	public string GetAddressStartFormat()
	{
		switch (DataCode)
		{
		case "":
		case "CR":
		case "MR":
		case "LR":
			return (base.AddressStart >= 16) ? $"{base.AddressStart / 16}{base.AddressStart % 16:D2}" : $"{base.AddressStart % 16}";
		case "B":
		case "VB":
		case "W":
			return base.AddressStart.ToString("X");
		default:
			return base.AddressStart.ToString();
		}
	}

	public bool IsBitAddressDefault()
	{
		switch (DataCode)
		{
		case "":
		case "R":
		case "B":
		case "CR":
		case "MR":
		case "LR":
		case "VB":
			return true;
		default:
			return false;
		}
	}

	public override void Parse(string address, ushort length)
	{
		OperateResult<KeyenceNanoAddress> operateResult = ParseFrom(address, length);
		if (operateResult.IsSuccess)
		{
			base.AddressStart = operateResult.Content.AddressStart;
			DataCode = operateResult.Content.DataCode;
			SplitLength = operateResult.Content.SplitLength;
		}
	}

	private static int CalculateAddress(string address)
	{
		int num = 0;
		if (address.IndexOf(".") < 0)
		{
			if (address.Length <= 2)
			{
				return Convert.ToInt32(address);
			}
			return Convert.ToInt32(address.Substring(0, address.Length - 2)) * 16 + Convert.ToInt32(address.Substring(address.Length - 2));
		}
		num = Convert.ToInt32(address.Substring(0, address.IndexOf("."))) * 16;
		string bit = address.Substring(address.IndexOf(".") + 1);
		return num + HslHelper.CalculateBitStartIndex(bit);
	}

	public static OperateResult<KeyenceNanoAddress> ParseFrom(string address, ushort length)
	{
		try
		{
			if (address.StartsWith("CTH") || address.StartsWith("cth"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("CTH", Convert.ToInt32(address.Substring(3)), 2));
			}
			if (address.StartsWith("CTC") || address.StartsWith("ctc"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("CTC", Convert.ToInt32(address.Substring(3)), 4));
			}
			if (address.StartsWith("CR") || address.StartsWith("cr"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("CR", CalculateAddress(address.Substring(2)), 256));
			}
			if (address.StartsWith("MR") || address.StartsWith("mr"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("MR", CalculateAddress(address.Substring(2)), 256));
			}
			if (address.StartsWith("LR") || address.StartsWith("lr"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("LR", CalculateAddress(address.Substring(2)), 256));
			}
			if (address.StartsWith("DM") || address.StartsWith("dm"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("DM", Convert.ToInt32(address.Substring(2)), 256));
			}
			if (address.StartsWith("CM") || address.StartsWith("cm"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("CM", Convert.ToInt32(address.Substring(2)), 256));
			}
			if (address.StartsWith("TM") || address.StartsWith("tm"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("TM", Convert.ToInt32(address.Substring(2)), 256));
			}
			if (address.StartsWith("VM") || address.StartsWith("vm"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("VM", Convert.ToInt32(address.Substring(2)), 256));
			}
			if (address.StartsWith("VB") || address.StartsWith("vb"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("VB", Convert.ToInt32(address.Substring(2), 16), 256));
			}
			if (address.StartsWith("EM") || address.StartsWith("em"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("EM", Convert.ToInt32(address.Substring(2)), 256));
			}
			if (address.StartsWith("FM") || address.StartsWith("fm"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("FM", Convert.ToInt32(address.Substring(2)), 256));
			}
			if (address.StartsWith("ZF") || address.StartsWith("zf"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("ZF", Convert.ToInt32(address.Substring(2)), 256));
			}
			if (address.StartsWith("AT") || address.StartsWith("at"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("AT", Convert.ToInt32(address.Substring(2)), 8));
			}
			if (address.StartsWith("TS") || address.StartsWith("ts"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("TS", Convert.ToInt32(address.Substring(2)), 64));
			}
			if (address.StartsWith("TC") || address.StartsWith("tc"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("TC", Convert.ToInt32(address.Substring(2)), 64));
			}
			if (address.StartsWith("CC") || address.StartsWith("cc"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("CC", Convert.ToInt32(address.Substring(2)), 64));
			}
			if (address.StartsWith("CS") || address.StartsWith("cs"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("CS", Convert.ToInt32(address.Substring(2)), 64));
			}
			if (address.StartsWith("W") || address.StartsWith("w"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("W", Convert.ToInt32(address.Substring(1), 16), 256));
			}
			if (address.StartsWith("Z") || address.StartsWith("z"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("Z", Convert.ToInt32(address.Substring(1)), 12));
			}
			if (address.StartsWith("R") || address.StartsWith("r"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("", CalculateAddress(address.Substring(1)), 256));
			}
			if (address.StartsWith("B") || address.StartsWith("b"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("B", Convert.ToInt32(address.Substring(1), 16), 256));
			}
			if (address.StartsWith("T") || address.StartsWith("t"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("T", Convert.ToInt32(address.Substring(1)), 64));
			}
			if (address.StartsWith("C") || address.StartsWith("c"))
			{
				return OperateResult.CreateSuccessResult(new KeyenceNanoAddress("C", Convert.ToInt32(address.Substring(1)), 64));
			}
			throw new Exception(StringResources.Language.NotSupportedDataType);
		}
		catch (Exception ex)
		{
			return new OperateResult<KeyenceNanoAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}
}
