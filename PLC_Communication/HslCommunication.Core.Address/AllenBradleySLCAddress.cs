using System;

namespace HslCommunication.Core.Address;

public class AllenBradleySLCAddress : DeviceAddressDataBase
{
	public byte DataCode { get; set; }

	public ushort DbBlock { get; set; }

	public override void Parse(string address, ushort length)
	{
		OperateResult<AllenBradleySLCAddress> operateResult = ParseFrom(address, length);
		if (operateResult.IsSuccess)
		{
			base.AddressStart = operateResult.Content.AddressStart;
			base.Length = operateResult.Content.Length;
			DataCode = operateResult.Content.DataCode;
			DbBlock = operateResult.Content.DbBlock;
		}
	}

	public override string ToString()
	{
		byte dataCode = DataCode;
		if (1 == 0)
		{
		}
		string result = dataCode switch
		{
			142 => $"A{DbBlock}:{base.AddressStart}", 
			133 => $"B{DbBlock}:{base.AddressStart}", 
			137 => $"N{DbBlock}:{base.AddressStart}", 
			138 => $"F{DbBlock}:{base.AddressStart}", 
			141 => $"ST{DbBlock}:{base.AddressStart}", 
			132 => $"S{DbBlock}:{base.AddressStart}", 
			135 => $"C{DbBlock}:{base.AddressStart}", 
			131 => $"I{DbBlock}:{base.AddressStart}", 
			130 => $"O{DbBlock}:{base.AddressStart}", 
			136 => $"R{DbBlock}:{base.AddressStart}", 
			134 => $"T{DbBlock}:{base.AddressStart}", 
			145 => $"L{DbBlock}:{base.AddressStart}", 
			_ => base.AddressStart.ToString(), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<AllenBradleySLCAddress> ParseFrom(string address)
	{
		return ParseFrom(address, 0);
	}

	public static OperateResult<AllenBradleySLCAddress> ParseFrom(string address, ushort length)
	{
		if (!address.Contains(":"))
		{
			return new OperateResult<AllenBradleySLCAddress>("Address can't find ':', example : A9:0");
		}
		string[] array = address.Split(':');
		try
		{
			AllenBradleySLCAddress allenBradleySLCAddress = new AllenBradleySLCAddress();
			switch (array[0][0])
			{
			case 'A':
				allenBradleySLCAddress.DataCode = 142;
				break;
			case 'B':
				allenBradleySLCAddress.DataCode = 133;
				break;
			case 'N':
				allenBradleySLCAddress.DataCode = 137;
				break;
			case 'F':
				allenBradleySLCAddress.DataCode = 138;
				break;
			case 'S':
				if (array[0].Length > 1 && array[0][1] == 'T')
				{
					allenBradleySLCAddress.DataCode = 141;
				}
				else
				{
					allenBradleySLCAddress.DataCode = 132;
				}
				break;
			case 'C':
				allenBradleySLCAddress.DataCode = 135;
				break;
			case 'I':
				allenBradleySLCAddress.DataCode = 131;
				break;
			case 'O':
				allenBradleySLCAddress.DataCode = 130;
				break;
			case 'R':
				allenBradleySLCAddress.DataCode = 136;
				break;
			case 'T':
				allenBradleySLCAddress.DataCode = 134;
				break;
			case 'L':
				allenBradleySLCAddress.DataCode = 145;
				break;
			default:
				throw new Exception("Address code wrong, must be A,B,N,F,S,C,I,O,R,T,ST,L");
			}
			switch (allenBradleySLCAddress.DataCode)
			{
			case 132:
				allenBradleySLCAddress.DbBlock = (ushort)((array[0].Length == 1) ? 2 : ushort.Parse(array[0].Substring(1)));
				break;
			case 130:
				allenBradleySLCAddress.DbBlock = (ushort)((array[0].Length != 1) ? ushort.Parse(array[0].Substring(1)) : 0);
				break;
			case 131:
				allenBradleySLCAddress.DbBlock = (ushort)((array[0].Length == 1) ? 1 : ushort.Parse(array[0].Substring(1)));
				break;
			case 141:
				allenBradleySLCAddress.DbBlock = (ushort)((array[0].Length == 2) ? 1 : ushort.Parse(array[0].Substring(2)));
				break;
			default:
				allenBradleySLCAddress.DbBlock = ushort.Parse(array[0].Substring(1));
				break;
			}
			allenBradleySLCAddress.AddressStart = ushort.Parse(array[1]);
			return OperateResult.CreateSuccessResult(allenBradleySLCAddress);
		}
		catch (Exception ex)
		{
			return new OperateResult<AllenBradleySLCAddress>("Wrong Address format: " + ex.Message);
		}
	}
}
