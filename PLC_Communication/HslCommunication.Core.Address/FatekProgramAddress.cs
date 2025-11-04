using System;

namespace HslCommunication.Core.Address;

public class FatekProgramAddress : DeviceAddressDataBase
{
	public string DataCode { get; set; }

	public override void Parse(string address, ushort length)
	{
		OperateResult<FatekProgramAddress> operateResult = ParseFrom(address, length);
		if (operateResult.IsSuccess)
		{
			base.AddressStart = operateResult.Content.AddressStart;
			base.Length = operateResult.Content.Length;
			DataCode = operateResult.Content.DataCode;
		}
	}

	public override string ToString()
	{
		if (DataCode == "X" || DataCode == "Y" || DataCode == "M" || DataCode == "S" || DataCode == "T" || DataCode == "C" || DataCode == "RT" || DataCode == "RC")
		{
			return DataCode + base.AddressStart.ToString("D4");
		}
		return DataCode + base.AddressStart.ToString("D5");
	}

	public static OperateResult<FatekProgramAddress> ParseFrom(string address, ushort length)
	{
		try
		{
			FatekProgramAddress fatekProgramAddress = new FatekProgramAddress();
			switch (address[0])
			{
			case 'X':
			case 'x':
				fatekProgramAddress.DataCode = "X";
				fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'Y':
			case 'y':
				fatekProgramAddress.DataCode = "Y";
				fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'M':
			case 'm':
				fatekProgramAddress.DataCode = "M";
				fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'S':
			case 's':
				fatekProgramAddress.DataCode = "S";
				fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'T':
			case 't':
				fatekProgramAddress.DataCode = "T";
				fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'C':
			case 'c':
				fatekProgramAddress.DataCode = "C";
				fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'D':
			case 'd':
				fatekProgramAddress.DataCode = "D";
				fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				break;
			case 'R':
			case 'r':
				if (address[1] == 'T' || address[1] == 't')
				{
					fatekProgramAddress.DataCode = "RT";
					fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
				}
				else if (address[1] == 'C' || address[1] == 'c')
				{
					fatekProgramAddress.DataCode = "RC";
					fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(2), 10);
				}
				else
				{
					fatekProgramAddress.DataCode = "R";
					fatekProgramAddress.AddressStart = Convert.ToUInt16(address.Substring(1), 10);
				}
				break;
			default:
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
			return OperateResult.CreateSuccessResult(fatekProgramAddress);
		}
		catch (Exception ex)
		{
			return new OperateResult<FatekProgramAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}
}
