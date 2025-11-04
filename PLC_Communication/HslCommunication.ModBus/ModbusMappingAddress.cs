using System;
using HslCommunication.Core;
using HslCommunication.Core.Address;

namespace HslCommunication.ModBus;

public class ModbusMappingAddress
{
	private static int ParseBitAddress(string address, int wordLength = 16)
	{
		int num = address.IndexOf('.');
		if (num > 0)
		{
			return Convert.ToInt32(address.Substring(0, num)) * wordLength + HslHelper.CalculateBitStartIndex(address.Substring(num + 1));
		}
		return Convert.ToInt32(address) * wordLength;
	}

	public static OperateResult<string> Delta_AS(string address, byte modbusCode)
	{
		try
		{
			string text = string.Empty;
			OperateResult<int> operateResult = HslHelper.ExtractParameter(ref address, "s");
			if (operateResult.IsSuccess)
			{
				text = $"s={operateResult.Content};";
			}
			if (modbusCode == 1 || modbusCode == 15 || modbusCode == 5)
			{
				if (address.StartsWith("SM") || address.StartsWith("sm"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) + 16384));
				}
				if (address.StartsWith("HC") || address.StartsWith("hc"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) + 64512));
				}
				if (address.StartsWith("S") || address.StartsWith("s"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 20480));
				}
				if (address.StartsWith("X") || address.StartsWith("x"))
				{
					return OperateResult.CreateSuccessResult(text + "x=2;" + (ParseBitAddress(address.Substring(1)) + 24576));
				}
				if (address.StartsWith("Y") || address.StartsWith("y"))
				{
					return OperateResult.CreateSuccessResult(text + (ParseBitAddress(address.Substring(1)) + 40960));
				}
				if (address.StartsWith("T") || address.StartsWith("t"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 57344));
				}
				if (address.StartsWith("C") || address.StartsWith("c"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 61440));
				}
				if (address.StartsWith("M") || address.StartsWith("m"))
				{
					return OperateResult.CreateSuccessResult(text + Convert.ToInt32(address.Substring(1)));
				}
				if (address.StartsWith("D") && address.Contains("."))
				{
					return OperateResult.CreateSuccessResult(text + address);
				}
			}
			else
			{
				if (address.StartsWith("SR") || address.StartsWith("sr"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) + 49152));
				}
				if (address.StartsWith("HC") || address.StartsWith("hc"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) + 64512));
				}
				if (address.StartsWith("D") || address.StartsWith("d"))
				{
					return OperateResult.CreateSuccessResult(text + Convert.ToInt32(address.Substring(1)));
				}
				if (address.StartsWith("X") || address.StartsWith("x"))
				{
					return OperateResult.CreateSuccessResult(text + "x=4;" + (Convert.ToInt32(address.Substring(1)) + 32768));
				}
				if (address.StartsWith("Y") || address.StartsWith("y"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 40960));
				}
				if (address.StartsWith("C") || address.StartsWith("c"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 61440));
				}
				if (address.StartsWith("T") || address.StartsWith("t"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 57344));
				}
				if (address.StartsWith("E") || address.StartsWith("e"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 65024));
				}
			}
			return new OperateResult<string>(StringResources.Language.NotSupportedDataType);
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(ex.Message);
		}
	}

	public static OperateResult<string> WeCon_Lx5v(string address, byte modbusCode)
	{
		try
		{
			string text = string.Empty;
			OperateResult<int> operateResult = HslHelper.ExtractParameter(ref address, "s");
			if (operateResult.IsSuccess)
			{
				text = $"s={operateResult.Content};";
			}
			if (modbusCode == 1 || modbusCode == 15 || modbusCode == 5)
			{
				if (address.StartsWithAndNumber("T"))
				{
					return OperateResult.CreateSuccessResult(text + Convert.ToInt32(address.Substring(1)));
				}
				if (address.StartsWithAndNumber("C"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 1536));
				}
				if (address.StartsWithAndNumber("LC"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) + 2560));
				}
				if (address.StartsWithAndNumber("HSC"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(3)) + 3584));
				}
				if (address.StartsWithAndNumber("M"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 4096));
				}
				if (address.StartsWithAndNumber("SM"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) + 20480));
				}
				if (address.StartsWithAndNumber("S"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 49152));
				}
				if (address.StartsWithAndNumber("X"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1), 8) + 57344));
				}
				if (address.StartsWithAndNumber("Y"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1), 8) + 61440));
				}
				if (ModbusHelper.TransPointAddressToModbus(text, address, new string[3] { "D", "SD", "R" }, new int[3] { 4096, 20480, 32768 }, int.Parse, out var newAddress))
				{
					return OperateResult.CreateSuccessResult(newAddress);
				}
			}
			else
			{
				if (address.StartsWithAndNumber("T"))
				{
					return OperateResult.CreateSuccessResult(text + Convert.ToInt32(address.Substring(1)));
				}
				if (address.StartsWithAndNumber("C"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 1536));
				}
				if (address.StartsWithAndNumber("LC"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) * 2 + 2560));
				}
				if (address.StartsWithAndNumber("HSC"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(3)) * 2 + 3584));
				}
				if (address.StartsWithAndNumber("D"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 4096));
				}
				if (address.StartsWithAndNumber("SD"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) + 20480));
				}
				if (address.StartsWithAndNumber("R"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 32768));
				}
			}
			throw new Exception(StringResources.Language.NotSupportedDataType);
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}

	public static OperateResult<string> Invt_Ts(string address, byte modbusCode)
	{
		try
		{
			string text = string.Empty;
			OperateResult<int> operateResult = HslHelper.ExtractParameter(ref address, "s");
			if (operateResult.IsSuccess)
			{
				text = $"s={operateResult.Content};";
			}
			if (modbusCode == 1 || modbusCode == 15 || modbusCode == 5)
			{
				if (address.StartsWithAndNumber("M"))
				{
					return OperateResult.CreateSuccessResult(text + Convert.ToInt32(address.Substring(1)));
				}
				if (address.StartsWithAndNumber("S"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 32768));
				}
				if (address.StartsWithAndNumber("X"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1), 8) + 40960));
				}
				if (address.StartsWithAndNumber("Y"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1), 8) + 45056));
				}
				if (address.StartsWithAndNumber("T"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 49152));
				}
				if (address.StartsWithAndNumber("C"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 49664));
				}
				if (ModbusHelper.TransPointAddressToModbus(text, address, new string[2] { "D", "R" }, new int[2] { 0, 32768 }, int.Parse, out var newAddress))
				{
					return OperateResult.CreateSuccessResult(newAddress);
				}
			}
			else
			{
				if (address.StartsWithAndNumber("T"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 57344));
				}
				if (address.StartsWithAndNumber("C"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 57856));
				}
				if (address.StartsWithAndNumber("Z"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(2)) * 2 + 58368));
				}
				if (address.StartsWithAndNumber("D"))
				{
					return OperateResult.CreateSuccessResult(text + Convert.ToInt32(address.Substring(1)));
				}
				if (address.StartsWithAndNumber("R"))
				{
					return OperateResult.CreateSuccessResult(text + (Convert.ToInt32(address.Substring(1)) + 32768));
				}
				if (address.StartsWithAndNumber("M"))
				{
					return OperateResult.CreateSuccessResult(text + "x=1;" + Convert.ToInt32(address.Substring(1)));
				}
			}
			throw new Exception(StringResources.Language.NotSupportedDataType);
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
	}
}
