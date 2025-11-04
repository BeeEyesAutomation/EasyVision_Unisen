using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Serial;

namespace HslCommunication.Profinet.Melsec.Helper;

public class MelsecFxLinksHelper
{
	public static byte[] PackCommandWithHeader(IReadWriteFxLinks plc, byte[] command)
	{
		if (command.Length > 3 && command[0] == 5)
		{
			return command;
		}
		byte[] array = command;
		if (plc.SumCheck)
		{
			array = new byte[command.Length + 2];
			command.CopyTo(array, 0);
			SoftLRC.CalculateAccAndFill(array, 0, 2);
		}
		if (plc.Format == 1)
		{
			return SoftBasic.SpliceArray<byte>(new byte[1] { 5 }, array);
		}
		if (plc.Format == 4)
		{
			return SoftBasic.SpliceArray<byte>(new byte[1] { 5 }, array, new byte[2] { 13, 10 });
		}
		return SoftBasic.SpliceArray<byte>(new byte[1] { 5 }, array);
	}

	public static OperateResult<List<byte[]>> BuildReadCommand(byte station, string address, ushort length, bool isBool, byte waitTime = 0)
	{
		OperateResult<MelsecFxLinksAddress> operateResult = MelsecFxLinksAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<List<byte[]>>(operateResult);
		}
		int[] array = SoftBasic.SplitIntegerToArray(length, isBool ? 256 : 64);
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < array.Length; i++)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(station.ToString("X2"));
			stringBuilder.Append("FF");
			if (isBool)
			{
				stringBuilder.Append("BR");
			}
			else if (operateResult.Content.AddressStart >= 10000)
			{
				stringBuilder.Append("QR");
			}
			else
			{
				stringBuilder.Append("WR");
			}
			stringBuilder.Append(waitTime.ToString("X"));
			stringBuilder.Append(operateResult.Content.ToString());
			if (array[i] == 256)
			{
				stringBuilder.Append("00");
			}
			else
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			list.Add(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
			operateResult.Content.AddressStart += array[i];
		}
		return OperateResult.CreateSuccessResult(list);
	}

	public static OperateResult<byte[]> BuildWriteBoolCommand(byte station, string address, bool[] value, byte waitTime = 0)
	{
		OperateResult<MelsecFxLinksAddress> operateResult = MelsecFxLinksAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("FF");
		stringBuilder.Append("BW");
		stringBuilder.Append(waitTime.ToString("X"));
		stringBuilder.Append(operateResult.Content.ToString());
		stringBuilder.Append(value.Length.ToString("X2"));
		for (int i = 0; i < value.Length; i++)
		{
			stringBuilder.Append(value[i] ? "1" : "0");
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildWriteByteCommand(byte station, string address, byte[] value, byte waitTime = 0)
	{
		OperateResult<MelsecFxLinksAddress> operateResult = MelsecFxLinksAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("FF");
		if (operateResult.Content.AddressStart >= 10000)
		{
			stringBuilder.Append("QW");
		}
		else
		{
			stringBuilder.Append("WW");
		}
		stringBuilder.Append(waitTime.ToString("X"));
		stringBuilder.Append(operateResult.Content.ToString());
		stringBuilder.Append((value.Length / 2).ToString("X2"));
		byte[] array = new byte[value.Length * 2];
		for (int i = 0; i < value.Length / 2; i++)
		{
			SoftBasic.BuildAsciiBytesFrom(BitConverter.ToUInt16(value, i * 2)).CopyTo(array, 4 * i);
		}
		stringBuilder.Append(Encoding.ASCII.GetString(array));
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildStart(byte station, byte waitTime = 0)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("FF");
		stringBuilder.Append("RR");
		stringBuilder.Append(waitTime.ToString("X"));
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildStop(byte station, byte waitTime = 0)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("FF");
		stringBuilder.Append("RS");
		stringBuilder.Append(waitTime.ToString("X"));
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildReadPlcType(byte station, byte waitTime = 0)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(station.ToString("X2"));
		stringBuilder.Append("FF");
		stringBuilder.Append("PC");
		stringBuilder.Append(waitTime.ToString("X"));
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<string> GetPlcTypeFromCode(string code)
	{
		if (1 == 0)
		{
		}
		OperateResult<string> result = code switch
		{
			"F2" => OperateResult.CreateSuccessResult("FX1S"), 
			"8E" => OperateResult.CreateSuccessResult("FX0N"), 
			"8D" => OperateResult.CreateSuccessResult("FX2/FX2C"), 
			"9E" => OperateResult.CreateSuccessResult("FX1N/FX1NC"), 
			"9D" => OperateResult.CreateSuccessResult("FX2N/FX2NC"), 
			"F4" => OperateResult.CreateSuccessResult("FX3G"), 
			"F3" => OperateResult.CreateSuccessResult("FX3U/FX3UC"), 
			"98" => OperateResult.CreateSuccessResult("A0J2HCPU"), 
			"A1" => OperateResult.CreateSuccessResult("A1CPU /A1NCPU"), 
			"A2" => OperateResult.CreateSuccessResult("A2CPU/A2NCPU/A2SCPU"), 
			"92" => OperateResult.CreateSuccessResult("A2ACPU"), 
			"93" => OperateResult.CreateSuccessResult("A2ACPU-S1"), 
			"9A" => OperateResult.CreateSuccessResult("A2CCPU"), 
			"82" => OperateResult.CreateSuccessResult("A2USCPU"), 
			"83" => OperateResult.CreateSuccessResult("A2CPU-S1/A2USCPU-S1"), 
			"A3" => OperateResult.CreateSuccessResult("A3CPU/A3NCPU"), 
			"94" => OperateResult.CreateSuccessResult("A3ACPU"), 
			"A4" => OperateResult.CreateSuccessResult("A3HCPU/A3MCPU"), 
			"84" => OperateResult.CreateSuccessResult("A3UCPU"), 
			"85" => OperateResult.CreateSuccessResult("A4UCPU"), 
			"AB" => OperateResult.CreateSuccessResult("AJ72P25/R25"), 
			"8B" => OperateResult.CreateSuccessResult("AJ72LP25/BR15"), 
			_ => new OperateResult<string>(StringResources.Language.NotSupportedDataType + " Code:" + code), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static string GetErrorText(int error)
	{
		if (1 == 0)
		{
		}
		string result = error switch
		{
			2 => StringResources.Language.MelsecFxLinksError02, 
			3 => StringResources.Language.MelsecFxLinksError03, 
			6 => StringResources.Language.MelsecFxLinksError06, 
			7 => StringResources.Language.MelsecFxLinksError07, 
			10 => StringResources.Language.MelsecFxLinksError0A, 
			16 => StringResources.Language.MelsecFxLinksError10, 
			24 => StringResources.Language.MelsecFxLinksError18, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<byte[]> CheckPlcResponse(byte[] response)
	{
		try
		{
			if (response[0] == 21)
			{
				int num = Convert.ToInt32(Encoding.ASCII.GetString(response, 5, 2), 16);
				return new OperateResult<byte[]>(num, GetErrorText(num));
			}
			if (response[0] != 2 && response[0] != 6)
			{
				return new OperateResult<byte[]>(response[0], "Check command failed: " + SoftBasic.GetAsciiStringRender(response));
			}
			if (response[0] == 6)
			{
				return OperateResult.CreateSuccessResult(new byte[0]);
			}
			int num2 = -1;
			for (int i = 5; i < response.Length; i++)
			{
				if (response[i] == 3)
				{
					num2 = i;
					break;
				}
			}
			if (num2 == -1)
			{
				num2 = response.Length;
			}
			return OperateResult.CreateSuccessResult(response.SelectMiddle(5, num2 - 5));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Check Plc Response failed Error: " + ex.Message + " Source: " + SoftBasic.GetAsciiStringRender(response));
		}
	}

	private static OperateResult<byte[]> ExtraResponse(byte[] response)
	{
		try
		{
			byte[] array = new byte[response.Length / 2];
			for (int i = 0; i < array.Length / 2; i++)
			{
				ushort value = Convert.ToUInt16(Encoding.ASCII.GetString(response, i * 4, 4), 16);
				BitConverter.GetBytes(value).CopyTo(array, i * 2);
			}
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Extra source data failed: " + ex.Message + Environment.NewLine + "Source: " + response.ToHexString(' '));
		}
	}

	public static OperateResult<byte[]> Read(IReadWriteFxLinks plc, string address, ushort length)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<List<byte[]>> operateResult = BuildReadCommand(station, address, length, isBool: false, plc.WaittingTime);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult2);
			}
			OperateResult<byte[]> operateResult3 = CheckPlcResponse(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			OperateResult<byte[]> operateResult4 = ExtraResponse(operateResult3.Content);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			list.AddRange(operateResult4.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult Write(IReadWriteFxLinks plc, string address, byte[] value)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<byte[]> operateResult = BuildWriteByteCommand(station, address, value, plc.WaittingTime);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = CheckPlcResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteFxLinks plc, string address, ushort length)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<List<byte[]>> command = BuildReadCommand(stat, address, length, isBool: false, plc.WaittingTime);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		List<byte> result = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(read);
			}
			OperateResult<byte[]> extra = CheckPlcResponse(read.Content);
			if (!extra.IsSuccess)
			{
				return extra;
			}
			OperateResult<byte[]> content = ExtraResponse(extra.Content);
			if (!content.IsSuccess)
			{
				return content;
			}
			result.AddRange(content.Content);
		}
		return OperateResult.CreateSuccessResult(result.ToArray());
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteFxLinks plc, string address, byte[] value)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<byte[]> command = BuildWriteByteCommand(stat, address, value, plc.WaittingTime);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> extra = CheckPlcResponse(read.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteFxLinks plc, string address, ushort length)
	{
		if (address.IndexOf('.') > 0)
		{
			return HslHelper.ReadBool(plc, address, length);
		}
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<List<byte[]>> operateResult = BuildReadCommand(station, address, length, isBool: true, plc.WaittingTime);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		List<bool> list = new List<bool>();
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult2);
			}
			OperateResult<byte[]> operateResult3 = CheckPlcResponse(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			list.AddRange(operateResult3.Content.Select((byte m) => m == 49).ToArray());
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult Write(IReadWriteFxLinks plc, string address, bool[] value)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<byte[]> operateResult = BuildWriteBoolCommand(station, address, value, plc.WaittingTime);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = CheckPlcResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteFxLinks plc, string address, ushort length)
	{
		if (address.IndexOf('.') > 0)
		{
			return await HslHelper.ReadBoolAsync(plc, address, length);
		}
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<List<byte[]>> command = BuildReadCommand(stat, address, length, isBool: true, plc.WaittingTime);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		List<bool> result = new List<bool>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult<byte[]> extra = CheckPlcResponse(read.Content);
			if (!extra.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(extra);
			}
			result.AddRange(extra.Content.Select((byte m) => m == 49).ToArray());
		}
		return OperateResult.CreateSuccessResult(result.ToArray());
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteFxLinks plc, string address, bool[] value)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<byte[]> command = BuildWriteBoolCommand(stat, address, value, plc.WaittingTime);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> extra = CheckPlcResponse(read.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult StartPLC(IReadWriteFxLinks plc, string parameter = "")
	{
		byte station = (byte)HslHelper.ExtractParameter(ref parameter, "s", plc.Station);
		OperateResult<byte[]> operateResult = BuildStart(station, plc.WaittingTime);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = CheckPlcResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult StopPLC(IReadWriteFxLinks plc, string parameter = "")
	{
		byte station = (byte)HslHelper.ExtractParameter(ref parameter, "s", plc.Station);
		OperateResult<byte[]> operateResult = BuildStop(station, plc.WaittingTime);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = CheckPlcResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult<string> ReadPlcType(IReadWriteFxLinks plc, string parameter = "")
	{
		byte station = (byte)HslHelper.ExtractParameter(ref parameter, "s", plc.Station);
		OperateResult<byte[]> operateResult = BuildReadPlcType(station, plc.WaittingTime);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = CheckPlcResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult3);
		}
		return GetPlcTypeFromCode(Encoding.ASCII.GetString(operateResult2.Content, 5, 2));
	}

	public static async Task<OperateResult> StartPLCAsync(IReadWriteFxLinks plc, string parameter = "")
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref parameter, "s", plc.Station);
		OperateResult<byte[]> command = BuildStart(stat, plc.WaittingTime);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> extra = CheckPlcResponse(read.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult> StopPLCAsync(IReadWriteFxLinks plc, string parameter = "")
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref parameter, "s", plc.Station);
		OperateResult<byte[]> command = BuildStop(stat, plc.WaittingTime);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> extra = CheckPlcResponse(read.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<string>> ReadPlcTypeAsync(IReadWriteFxLinks plc, string parameter = "")
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref parameter, "s", plc.Station);
		OperateResult<byte[]> command = BuildReadPlcType(stat, plc.WaittingTime);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(command);
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult<byte[]> extra = CheckPlcResponse(read.Content);
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(extra);
		}
		return GetPlcTypeFromCode(Encoding.ASCII.GetString(read.Content, 5, 2));
	}
}
