using System;
using HslCommunication.Profinet.Omron;

namespace HslCommunication.Core.Address;

public class OmronFinsAddress : DeviceAddressDataBase
{
	public byte BitCode { get; set; }

	public byte WordCode { get; set; }

	public override void Parse(string address, ushort length)
	{
		OperateResult<OmronFinsAddress> operateResult = ParseFrom(address, length, OmronPlcType.CSCJ);
		if (operateResult.IsSuccess)
		{
			base.AddressStart = operateResult.Content.AddressStart;
			base.Length = operateResult.Content.Length;
			BitCode = operateResult.Content.BitCode;
			WordCode = operateResult.Content.WordCode;
		}
	}

	public static OperateResult<OmronFinsAddress> ParseFrom(string address)
	{
		return ParseFrom(address, 0, OmronPlcType.CSCJ);
	}

	private static int CalculateBitIndex(string address)
	{
		string[] array = address.SplitDot();
		int num = ushort.Parse(array[0]) * 16;
		if (array.Length > 1)
		{
			num += HslHelper.CalculateBitStartIndex(array[1]);
		}
		return num;
	}

	public static OperateResult<OmronFinsAddress> ParseFrom(string address, ushort length, OmronPlcType plcType)
	{
		OmronFinsAddress omronFinsAddress = new OmronFinsAddress();
		try
		{
			omronFinsAddress.Length = length;
			if (address.StartsWith("DR", StringComparison.OrdinalIgnoreCase))
			{
				if (plcType == OmronPlcType.CV)
				{
					omronFinsAddress.WordCode = 156;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2)) + 48;
				}
				else
				{
					omronFinsAddress.WordCode = 188;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2)) + 8192;
				}
			}
			else if (address.StartsWith("IR", StringComparison.OrdinalIgnoreCase))
			{
				omronFinsAddress.WordCode = 220;
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2)) + 4096;
			}
			else if (address.StartsWith("DM", StringComparison.OrdinalIgnoreCase))
			{
				omronFinsAddress.BitCode = OmronFinsDataType.DM.BitCode;
				omronFinsAddress.WordCode = OmronFinsDataType.DM.WordCode;
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2));
			}
			else if (address.StartsWith("TIM") || address.StartsWith("tim"))
			{
				if (plcType == OmronPlcType.CV)
				{
					omronFinsAddress.BitCode = 1;
					omronFinsAddress.WordCode = 129;
				}
				else
				{
					omronFinsAddress.BitCode = OmronFinsDataType.TIM.BitCode;
					omronFinsAddress.WordCode = OmronFinsDataType.TIM.WordCode;
				}
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(3));
			}
			else if (address.StartsWith("CNT") || address.StartsWith("cnt"))
			{
				if (plcType == OmronPlcType.CV)
				{
					omronFinsAddress.BitCode = 1;
					omronFinsAddress.WordCode = 129;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(3)) + 32768;
				}
				else
				{
					omronFinsAddress.BitCode = OmronFinsDataType.TIM.BitCode;
					omronFinsAddress.WordCode = OmronFinsDataType.TIM.WordCode;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(3)) + 524288;
				}
			}
			else if (address.StartsWith("CIO") || address.StartsWith("cio"))
			{
				if (plcType == OmronPlcType.CV)
				{
					omronFinsAddress.BitCode = 0;
					omronFinsAddress.WordCode = 128;
				}
				else
				{
					omronFinsAddress.BitCode = OmronFinsDataType.CIO.BitCode;
					omronFinsAddress.WordCode = OmronFinsDataType.CIO.WordCode;
				}
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(3));
			}
			else if (address.StartsWith("WR") || address.StartsWith("wr"))
			{
				omronFinsAddress.BitCode = OmronFinsDataType.WR.BitCode;
				omronFinsAddress.WordCode = OmronFinsDataType.WR.WordCode;
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2));
			}
			else if (address.StartsWith("HR") || address.StartsWith("hr"))
			{
				omronFinsAddress.BitCode = OmronFinsDataType.HR.BitCode;
				omronFinsAddress.WordCode = OmronFinsDataType.HR.WordCode;
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2));
			}
			else if (address.StartsWith("AR") || address.StartsWith("ar"))
			{
				if (plcType == OmronPlcType.CV)
				{
					omronFinsAddress.BitCode = 0;
					omronFinsAddress.WordCode = 128;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2)) + 45056;
				}
				else
				{
					omronFinsAddress.BitCode = OmronFinsDataType.AR.BitCode;
					omronFinsAddress.WordCode = OmronFinsDataType.AR.WordCode;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2));
				}
			}
			else if (address.StartsWith("CF") || address.StartsWith("cf"))
			{
				omronFinsAddress.BitCode = 7;
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(2));
			}
			else if (address.StartsWith("EM") || address.StartsWith("em") || address.StartsWith("E") || address.StartsWith("e"))
			{
				if (address.IndexOf('.') > 0)
				{
					string[] array = address.SplitDot();
					int num = Convert.ToInt32(array[0].Substring((address[1] != 'M' && address[1] != 'm') ? 1 : 2), 16);
					if (num < 16)
					{
						omronFinsAddress.BitCode = (byte)(32 + num);
						if (plcType == OmronPlcType.CV)
						{
							omronFinsAddress.WordCode = (byte)(144 + num);
						}
						else
						{
							omronFinsAddress.WordCode = (byte)(160 + num);
						}
					}
					else
					{
						omronFinsAddress.BitCode = (byte)(224 + num - 16);
						omronFinsAddress.WordCode = (byte)(96 + num - 16);
					}
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(address.IndexOf('.') + 1));
				}
				else
				{
					omronFinsAddress.BitCode = 10;
					omronFinsAddress.WordCode = 152;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring((address[1] != 'M' && address[1] != 'm') ? 1 : 2));
				}
			}
			else if (address.StartsWith("D") || address.StartsWith("d"))
			{
				omronFinsAddress.BitCode = OmronFinsDataType.DM.BitCode;
				omronFinsAddress.WordCode = OmronFinsDataType.DM.WordCode;
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(1));
			}
			else if (address.StartsWith("C") || address.StartsWith("c"))
			{
				if (plcType == OmronPlcType.CV)
				{
					omronFinsAddress.BitCode = 0;
					omronFinsAddress.WordCode = 128;
				}
				else
				{
					omronFinsAddress.BitCode = OmronFinsDataType.CIO.BitCode;
					omronFinsAddress.WordCode = OmronFinsDataType.CIO.WordCode;
				}
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(1));
			}
			else if (address.StartsWith("W") || address.StartsWith("w"))
			{
				omronFinsAddress.BitCode = OmronFinsDataType.WR.BitCode;
				omronFinsAddress.WordCode = OmronFinsDataType.WR.WordCode;
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(1));
			}
			else if (address.StartsWith("H") || address.StartsWith("h"))
			{
				omronFinsAddress.BitCode = OmronFinsDataType.HR.BitCode;
				omronFinsAddress.WordCode = OmronFinsDataType.HR.WordCode;
				omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(1));
			}
			else
			{
				if (!address.StartsWith("A") && !address.StartsWith("a"))
				{
					throw new Exception(StringResources.Language.NotSupportedDataType);
				}
				if (plcType == OmronPlcType.CV)
				{
					omronFinsAddress.BitCode = 0;
					omronFinsAddress.WordCode = 128;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(1)) + 45056;
				}
				else
				{
					omronFinsAddress.BitCode = OmronFinsDataType.AR.BitCode;
					omronFinsAddress.WordCode = OmronFinsDataType.AR.WordCode;
					omronFinsAddress.AddressStart = CalculateBitIndex(address.Substring(1));
				}
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<OmronFinsAddress>(DeviceAddressDataBase.GetUnsupportedAddressInfo(address, ex));
		}
		return OperateResult.CreateSuccessResult(omronFinsAddress);
	}
}
