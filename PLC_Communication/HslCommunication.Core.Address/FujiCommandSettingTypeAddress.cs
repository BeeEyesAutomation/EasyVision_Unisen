using System;
using System.Text.RegularExpressions;

namespace HslCommunication.Core.Address;

public class FujiCommandSettingTypeAddress : DeviceAddressDataBase
{
	public byte DataCode { get; set; }

	public string AddressHeader { get; set; }

	public override void Parse(string address, ushort length)
	{
		base.Parse(address, length);
	}

	public override string ToString()
	{
		return AddressHeader + base.AddressStart;
	}

	public static OperateResult<FujiCommandSettingTypeAddress> ParseFrom(string address, ushort length)
	{
		try
		{
			FujiCommandSettingTypeAddress fujiCommandSettingTypeAddress = new FujiCommandSettingTypeAddress();
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (address.IndexOf('.') < 0)
			{
				Match match = Regex.Match(address, "^[A-Z]+");
				if (!match.Success)
				{
					return new OperateResult<FujiCommandSettingTypeAddress>(StringResources.Language.NotSupportedDataType);
				}
				empty = match.Value;
				empty2 = address.Substring(empty.Length);
			}
			else
			{
				string[] array = address.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				if (array[0][0] != 'W')
				{
					return new OperateResult<FujiCommandSettingTypeAddress>(StringResources.Language.NotSupportedDataType);
				}
				empty = array[0];
				empty2 = array[1];
			}
			fujiCommandSettingTypeAddress.AddressHeader = empty;
			fujiCommandSettingTypeAddress.AddressStart = Convert.ToInt32(empty2);
			fujiCommandSettingTypeAddress.Length = length;
			switch (empty)
			{
			case "TS":
				fujiCommandSettingTypeAddress.DataCode = 10;
				break;
			case "TR":
				fujiCommandSettingTypeAddress.DataCode = 11;
				break;
			case "CS":
				fujiCommandSettingTypeAddress.DataCode = 12;
				break;
			case "CR":
				fujiCommandSettingTypeAddress.DataCode = 13;
				break;
			case "BD":
				fujiCommandSettingTypeAddress.DataCode = 14;
				break;
			case "WL":
				fujiCommandSettingTypeAddress.DataCode = 20;
				break;
			case "B":
				fujiCommandSettingTypeAddress.DataCode = 0;
				break;
			case "M":
				fujiCommandSettingTypeAddress.DataCode = 1;
				break;
			case "K":
				fujiCommandSettingTypeAddress.DataCode = 2;
				break;
			case "F":
				fujiCommandSettingTypeAddress.DataCode = 3;
				break;
			case "A":
				fujiCommandSettingTypeAddress.DataCode = 4;
				break;
			case "D":
				fujiCommandSettingTypeAddress.DataCode = 5;
				break;
			case "S":
				fujiCommandSettingTypeAddress.DataCode = 8;
				break;
			default:
				if (empty.StartsWith("W"))
				{
					int num = Convert.ToInt32(empty.Substring(1));
					if (num == 9)
					{
						fujiCommandSettingTypeAddress.DataCode = 9;
						break;
					}
					if (num >= 21 && num <= 26)
					{
						fujiCommandSettingTypeAddress.DataCode = (byte)num;
						break;
					}
					if (num >= 30 && num <= 109)
					{
						fujiCommandSettingTypeAddress.DataCode = (byte)num;
						break;
					}
					if (num >= 120 && num <= 123)
					{
						fujiCommandSettingTypeAddress.DataCode = (byte)num;
						break;
					}
					if (num == 125)
					{
						fujiCommandSettingTypeAddress.DataCode = (byte)num;
						break;
					}
					throw new Exception(StringResources.Language.NotSupportedDataType);
				}
				throw new Exception(StringResources.Language.NotSupportedDataType);
			}
			return OperateResult.CreateSuccessResult(fujiCommandSettingTypeAddress);
		}
		catch (Exception ex)
		{
			return new OperateResult<FujiCommandSettingTypeAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}
}
