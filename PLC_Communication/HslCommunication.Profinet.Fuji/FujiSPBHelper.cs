using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;

namespace HslCommunication.Profinet.Fuji;

public class FujiSPBHelper
{
	public static string AnalysisIntegerAddress(int address)
	{
		string text = address.ToString("D4");
		return text.Substring(2) + text.Substring(0, 2);
	}

	public static string CalculateAcc(string data)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(data);
		int num = 0;
		for (int i = 0; i < bytes.Length; i++)
		{
			num += bytes[i];
		}
		return num.ToString("X4").Substring(2);
	}

	public static OperateResult<byte[]> BuildReadCommand(byte station, string address, ushort length)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FujiSPBAddress> operateResult = FujiSPBAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return BuildReadCommand(station, operateResult.Content, length);
	}

	public static OperateResult<byte[]> BuildReadCommand(byte station, FujiSPBAddress address, ushort length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(':');
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("09");
		stringBuilder.Append("FFFF");
		stringBuilder.Append("00");
		stringBuilder.Append("00");
		stringBuilder.Append(address.GetWordAddress());
		stringBuilder.Append(AnalysisIntegerAddress(length));
		stringBuilder.Append("\r\n");
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildReadCommand(byte station, string[] address, ushort[] length, bool isBool)
	{
		if (address == null || length == null)
		{
			return new OperateResult<byte[]>("Parameter address or length can't be null");
		}
		if (address.Length != length.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(':');
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append((6 + address.Length * 4).ToString("X2"));
		stringBuilder.Append("FFFF");
		stringBuilder.Append("00");
		stringBuilder.Append("04");
		stringBuilder.Append("00");
		stringBuilder.Append(address.Length.ToString("X2"));
		for (int i = 0; i < address.Length; i++)
		{
			station = (byte)HslHelper.ExtractParameter(ref address[i], "s", station);
			OperateResult<FujiSPBAddress> operateResult = FujiSPBAddress.ParseFrom(address[i]);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			stringBuilder.Append(operateResult.Content.TypeCode);
			stringBuilder.Append(length[i].ToString("X2"));
			stringBuilder.Append(AnalysisIntegerAddress(operateResult.Content.AddressStart));
		}
		stringBuilder[1] = station.ToString("X2")[0];
		stringBuilder[2] = station.ToString("X2")[1];
		stringBuilder.Append("\r\n");
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildWriteByteCommand(byte station, string address, byte[] value)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FujiSPBAddress> operateResult = FujiSPBAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(':');
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("00");
		stringBuilder.Append("FFFF");
		stringBuilder.Append("01");
		stringBuilder.Append("00");
		stringBuilder.Append(operateResult.Content.GetWordAddress());
		stringBuilder.Append(AnalysisIntegerAddress(value.Length / 2));
		stringBuilder.Append(value.ToHexString());
		stringBuilder[3] = ((stringBuilder.Length - 5) / 2).ToString("X2")[0];
		stringBuilder[4] = ((stringBuilder.Length - 5) / 2).ToString("X2")[1];
		stringBuilder.Append("\r\n");
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildWriteBoolCommand(byte station, string address, bool value)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FujiSPBAddress> operateResult = FujiSPBAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if ((address.StartsWith("X") || address.StartsWith("Y") || address.StartsWith("M") || address.StartsWith("L") || address.StartsWith("TC") || address.StartsWith("CC")) && address.IndexOf('.') < 0)
		{
			operateResult.Content.BitIndex = operateResult.Content.AddressStart % 16;
			operateResult.Content.AddressStart = (ushort)(operateResult.Content.AddressStart / 16);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(':');
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("00");
		stringBuilder.Append("FFFF");
		stringBuilder.Append("01");
		stringBuilder.Append("02");
		stringBuilder.Append(operateResult.Content.GetWriteBoolAddress());
		stringBuilder.Append(value ? "01" : "00");
		stringBuilder[3] = ((stringBuilder.Length - 5) / 2).ToString("X2")[0];
		stringBuilder[4] = ((stringBuilder.Length - 5) / 2).ToString("X2")[1];
		stringBuilder.Append("\r\n");
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> CheckResponseData(byte[] content)
	{
		try
		{
			if (content[0] != 58)
			{
				return new OperateResult<byte[]>(content[0], "Read Faild:" + SoftBasic.ByteToHexString(content, ' '));
			}
			string text = Encoding.ASCII.GetString(content, 9, 2);
			if (text != "00")
			{
				return new OperateResult<byte[]>(Convert.ToInt32(text, 16), GetErrorDescriptionFromCode(text));
			}
			if (content[content.Length - 2] == 13 && content[content.Length - 1] == 10)
			{
				content = content.RemoveLast(2);
			}
			return OperateResult.CreateSuccessResult(content.RemoveBegin(11));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("CheckResponseData failed: " + ex.Message + Environment.NewLine + "Source: " + content.ToHexString(' '));
		}
	}

	public static string GetErrorDescriptionFromCode(string code)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			"01" => StringResources.Language.FujiSpbStatus01, 
			"02" => StringResources.Language.FujiSpbStatus02, 
			"03" => StringResources.Language.FujiSpbStatus03, 
			"04" => StringResources.Language.FujiSpbStatus04, 
			"05" => StringResources.Language.FujiSpbStatus05, 
			"06" => StringResources.Language.FujiSpbStatus06, 
			"07" => StringResources.Language.FujiSpbStatus07, 
			"09" => StringResources.Language.FujiSpbStatus09, 
			"0C" => StringResources.Language.FujiSpbStatus0C, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<byte[]> Read(IReadWriteDevice device, byte station, string address, ushort length)
	{
		OperateResult<byte[]> operateResult = BuildReadCommand(station, address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = device.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = CheckResponseData(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult3);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(operateResult3.Content.RemoveBegin(4)).ToHexBytes());
	}

	public static OperateResult Write(IReadWriteDevice device, byte station, string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteByteCommand(station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = device.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckResponseData(operateResult2.Content);
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteDevice device, byte station, string address, ushort length)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FujiSPBAddress> operateResult = FujiSPBAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		if ((address.StartsWith("X") || address.StartsWith("Y") || address.StartsWith("M") || address.StartsWith("L") || address.StartsWith("TC") || address.StartsWith("CC")) && address.IndexOf('.') < 0)
		{
			operateResult.Content.BitIndex = operateResult.Content.AddressStart % 16;
			operateResult.Content.AddressStart = (ushort)(operateResult.Content.AddressStart / 16);
		}
		ushort length2 = (ushort)((operateResult.Content.GetBitIndex() + length - 1) / 16 - operateResult.Content.GetBitIndex() / 16 + 1);
		OperateResult<byte[]> operateResult2 = BuildReadCommand(station2, operateResult.Content, length2);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = device.ReadFromCoreServer(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult3);
		}
		OperateResult<byte[]> operateResult4 = CheckResponseData(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult4);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(operateResult4.Content.RemoveBegin(4)).ToHexBytes().ToBoolArray()
			.SelectMiddle(operateResult.Content.BitIndex, length));
	}

	public static OperateResult Write(IReadWriteDevice device, byte station, string address, bool value)
	{
		OperateResult<byte[]> operateResult = BuildWriteBoolCommand(station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = device.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckResponseData(operateResult2.Content);
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteDevice device, byte station, string address, ushort length)
	{
		OperateResult<byte[]> command = BuildReadCommand(station, address, length);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		OperateResult<byte[]> check = CheckResponseData(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(check.Content.RemoveBegin(4)).ToHexBytes());
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice device, byte station, string address, byte[] value)
	{
		OperateResult<byte[]> command = BuildWriteByteCommand(station, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckResponseData(read.Content);
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteDevice device, byte station, string address, ushort length)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FujiSPBAddress> addressAnalysis = FujiSPBAddress.ParseFrom(address);
		if (!addressAnalysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(addressAnalysis);
		}
		if ((address.StartsWith("X") || address.StartsWith("Y") || address.StartsWith("M") || address.StartsWith("L") || address.StartsWith("TC") || address.StartsWith("CC")) && address.IndexOf('.') < 0)
		{
			addressAnalysis.Content.BitIndex = addressAnalysis.Content.AddressStart % 16;
			addressAnalysis.Content.AddressStart = (ushort)(addressAnalysis.Content.AddressStart / 16);
		}
		OperateResult<byte[]> command = BuildReadCommand(length: (ushort)((addressAnalysis.Content.GetBitIndex() + length - 1) / 16 - addressAnalysis.Content.GetBitIndex() / 16 + 1), station: stat, address: addressAnalysis.Content);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		OperateResult<byte[]> check = CheckResponseData(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(check);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(check.Content.RemoveBegin(4)).ToHexBytes().ToBoolArray()
			.SelectMiddle(addressAnalysis.Content.BitIndex, length));
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice device, byte station, string address, bool value)
	{
		OperateResult<byte[]> command = BuildWriteBoolCommand(station, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckResponseData(read.Content);
	}
}
