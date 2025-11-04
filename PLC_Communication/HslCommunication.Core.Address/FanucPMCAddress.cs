using System;

namespace HslCommunication.Core.Address;

public class FanucPMCAddress : DeviceAddressDataBase
{
	public int DataCode { get; set; }

	public int AddressEnd { get; set; }

	public static OperateResult<FanucPMCAddress> ParseFrom(string address, ushort length)
	{
		FanucPMCAddress fanucPMCAddress = new FanucPMCAddress();
		try
		{
			switch (address[0])
			{
			case 'G':
			case 'g':
				fanucPMCAddress.DataCode = 0;
				break;
			case 'F':
			case 'f':
				fanucPMCAddress.DataCode = 1;
				break;
			case 'Y':
			case 'y':
				fanucPMCAddress.DataCode = 2;
				break;
			case 'X':
			case 'x':
				fanucPMCAddress.DataCode = 3;
				break;
			case 'A':
			case 'a':
				fanucPMCAddress.DataCode = 4;
				break;
			case 'R':
			case 'r':
				fanucPMCAddress.DataCode = 5;
				break;
			case 'T':
			case 't':
				fanucPMCAddress.DataCode = 6;
				break;
			case 'K':
			case 'k':
				fanucPMCAddress.DataCode = 7;
				break;
			case 'C':
			case 'c':
				fanucPMCAddress.DataCode = 8;
				break;
			case 'D':
			case 'd':
				fanucPMCAddress.DataCode = 9;
				break;
			case 'E':
			case 'e':
				fanucPMCAddress.DataCode = 12;
				break;
			default:
				return new OperateResult<FanucPMCAddress>(StringResources.Language.NotSupportedDataType);
			}
			fanucPMCAddress.AddressStart = Convert.ToInt32(address.Substring(1));
			fanucPMCAddress.AddressEnd = fanucPMCAddress.AddressStart + length - 1;
			fanucPMCAddress.Length = length;
			if (fanucPMCAddress.AddressEnd < fanucPMCAddress.AddressStart)
			{
				fanucPMCAddress.AddressEnd = fanucPMCAddress.AddressStart;
			}
			return OperateResult.CreateSuccessResult(fanucPMCAddress);
		}
		catch (Exception ex)
		{
			return new OperateResult<FanucPMCAddress>(StringResources.Language.NotSupportedDataType + " : " + ex.Message);
		}
	}
}
