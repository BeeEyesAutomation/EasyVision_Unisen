using System;
using System.Linq;
using System.Text;

namespace HslCommunication.Profinet.Yamatake.Helper;

public class DigitronCPLHelper
{
	public static OperateResult<byte[]> BuildReadCommand(byte station, string address, ushort length)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('\u0002');
			stringBuilder.Append(station.ToString("X2"));
			stringBuilder.Append("00XRS,");
			stringBuilder.Append(ushort.Parse(address).ToString());
			stringBuilder.Append("W,");
			stringBuilder.Append(length.ToString());
			stringBuilder.Append('\u0003');
			int num = 0;
			for (int i = 0; i < stringBuilder.Length; i++)
			{
				num += stringBuilder[i];
			}
			stringBuilder.Append(((byte)(256 - num % 256)).ToString("X2"));
			stringBuilder.Append("\r\n");
			return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Address wrong: " + ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteCommand(byte station, string address, byte[] value)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('\u0002');
			stringBuilder.Append(station.ToString("X2"));
			stringBuilder.Append("00XWS,");
			stringBuilder.Append(ushort.Parse(address).ToString());
			stringBuilder.Append("W");
			for (int i = 0; i < value.Length / 2; i++)
			{
				short num = BitConverter.ToInt16(value, i * 2);
				stringBuilder.Append(",");
				stringBuilder.Append(num.ToString());
			}
			stringBuilder.Append('\u0003');
			int num2 = 0;
			for (int j = 0; j < stringBuilder.Length; j++)
			{
				num2 += stringBuilder[j];
			}
			stringBuilder.Append(((byte)(256 - num2 % 256)).ToString("X2"));
			stringBuilder.Append("\r\n");
			return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Address wrong: " + ex.Message);
		}
	}

	public static byte[] PackResponseContent(byte station, int err, byte[] value, byte dataType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('\u0002');
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("00X");
		stringBuilder.Append(err.ToString("D2"));
		if (err == 0 && value != null)
		{
			for (int i = 0; i < value.Length / 2; i++)
			{
				if (dataType == 87)
				{
					short num = BitConverter.ToInt16(value, i * 2);
					stringBuilder.Append(",");
					stringBuilder.Append(num.ToString());
				}
				else
				{
					ushort num2 = BitConverter.ToUInt16(value, i * 2);
					stringBuilder.Append(",");
					stringBuilder.Append(num2.ToString());
				}
			}
		}
		stringBuilder.Append('\u0003');
		int num3 = 0;
		for (int j = 0; j < stringBuilder.Length; j++)
		{
			num3 += stringBuilder[j];
		}
		stringBuilder.Append(((byte)(256 - num3 % 256)).ToString("X2"));
		stringBuilder.Append("\r\n");
		return Encoding.ASCII.GetBytes(stringBuilder.ToString());
	}

	public static string GetErrorText(int err)
	{
		if (1 == 0)
		{
		}
		string result = err switch
		{
			40 => StringResources.Language.YamatakeDigitronCPL40, 
			41 => StringResources.Language.YamatakeDigitronCPL41, 
			42 => StringResources.Language.YamatakeDigitronCPL42, 
			43 => StringResources.Language.YamatakeDigitronCPL43, 
			44 => StringResources.Language.YamatakeDigitronCPL44, 
			45 => StringResources.Language.YamatakeDigitronCPL45, 
			46 => StringResources.Language.YamatakeDigitronCPL46, 
			47 => StringResources.Language.YamatakeDigitronCPL47, 
			48 => StringResources.Language.YamatakeDigitronCPL48, 
			99 => StringResources.Language.YamatakeDigitronCPL99, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<byte[]> ExtraActualResponse(byte[] response)
	{
		try
		{
			int num = Convert.ToInt32(Encoding.ASCII.GetString(response, 6, 2));
			if (num > 0)
			{
				return new OperateResult<byte[]>(num, GetErrorText(num));
			}
			int num2 = 8;
			for (int i = 8; i < response.Length; i++)
			{
				if (response[i] == 3)
				{
					num2 = i;
					break;
				}
			}
			int num3 = ((response[8] == 44) ? 9 : 8);
			if (num2 - num3 > 0)
			{
				string[] source = Encoding.ASCII.GetString(response, num3, num2 - num3).Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				short[] array = source.Select((string m) => short.Parse(m)).ToArray();
				byte[] array2 = new byte[array.Length * 2];
				for (int num4 = 0; num4 < array.Length; num4++)
				{
					BitConverter.GetBytes(array[num4]).CopyTo(array2, num4 * 2);
				}
				return OperateResult.CreateSuccessResult(array2);
			}
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Data wrong: " + ex.Message + Environment.NewLine + "Source: " + response.ToHexString(' '));
		}
	}
}
