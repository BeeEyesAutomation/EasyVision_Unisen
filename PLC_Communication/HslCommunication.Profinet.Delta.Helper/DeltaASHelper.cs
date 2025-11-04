using System;
using HslCommunication.Core;

namespace HslCommunication.Profinet.Delta.Helper;

public class DeltaASHelper
{
	private static int ParseDeltaBitAddress(string address)
	{
		int num = address.IndexOf('.');
		if (num > 0)
		{
			return Convert.ToInt32(address.Substring(0, num)) * 16 + HslHelper.CalculateBitStartIndex(address.Substring(num + 1));
		}
		return Convert.ToInt32(address) * 16;
	}

	public static OperateResult<string> ParseDeltaASAddress(string address, byte modbusCode)
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
					return OperateResult.CreateSuccessResult(text + "x=2;" + (ParseDeltaBitAddress(address.Substring(1)) + 24576));
				}
				if (address.StartsWith("Y") || address.StartsWith("y"))
				{
					return OperateResult.CreateSuccessResult(text + (ParseDeltaBitAddress(address.Substring(1)) + 40960));
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
}
