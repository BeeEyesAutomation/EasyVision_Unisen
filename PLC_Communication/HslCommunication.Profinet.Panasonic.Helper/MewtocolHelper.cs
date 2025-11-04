using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Profinet.Panasonic.Helper;

public class MewtocolHelper
{
	private static bool CheckBoolOnWordAddress(string address)
	{
		return Regex.IsMatch(address, "^(s=[0-9]+;)?(D|LD|DT|F)[0-9]+\\.[0-9]+$", RegexOptions.IgnoreCase);
	}

	public static OperateResult<bool> ReadBool(IReadWriteDevice plc, byte station, string address)
	{
		if (CheckBoolOnWordAddress(address))
		{
			return ByteTransformHelper.GetResultFromArray(HslHelper.ReadBool(plc, address, 1));
		}
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = PanasonicHelper.BuildReadOneCoil(station, address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult2);
		}
		return ByteTransformHelper.GetResultFromArray(PanasonicHelper.ExtraActualBool(operateResult2.Content));
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteDevice plc, byte station, string address, ushort length)
	{
		if (CheckBoolOnWordAddress(address))
		{
			return HslHelper.ReadBool(plc, address, length);
		}
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<string, int> operateResult = PanasonicHelper.AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		OperateResult<List<byte[]>> operateResult2 = PanasonicHelper.BuildReadCommand(station, address, length, isBit: true);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < operateResult2.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult3 = plc.ReadFromCoreServer(operateResult2.Content[i]);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			OperateResult<byte[]> operateResult4 = PanasonicHelper.ExtraActualData(operateResult3.Content);
			if (!operateResult4.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult4);
			}
			list.AddRange(operateResult4.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray().ToBoolArray().SelectMiddle(operateResult.Content2 % 16, length));
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteDevice plc, byte station, string[] address)
	{
		OperateResult<List<byte[]>> operateResult = PanasonicHelper.BuildReadCoils(station, address);
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
			OperateResult<bool[]> operateResult3 = PanasonicHelper.ExtraActualBool(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			list.AddRange(operateResult3.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult Write(IReadWriteDevice plc, byte station, string address, bool value)
	{
		if (CheckBoolOnWordAddress(address))
		{
			return HslHelper.WriteBool(plc, address, new bool[1] { value });
		}
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = PanasonicHelper.BuildWriteOneCoil(station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return PanasonicHelper.ExtraActualData(operateResult2.Content);
	}

	public static OperateResult Write(IReadWriteDevice plc, byte station, string address, bool[] values)
	{
		if (CheckBoolOnWordAddress(address))
		{
			return HslHelper.WriteBool(plc, address, values);
		}
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<string, int> operateResult = PanasonicHelper.AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		if (operateResult.Content2 % 16 != 0)
		{
			return new OperateResult(StringResources.Language.PanasonicAddressBitStartMulti16);
		}
		if (values.Length % 16 != 0)
		{
			return new OperateResult(StringResources.Language.PanasonicBoolLengthMulti16);
		}
		byte[] values2 = SoftBasic.BoolArrayToByte(values);
		OperateResult<byte[]> operateResult2 = PanasonicHelper.BuildWriteCommand(station, address, values2);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = plc.ReadFromCoreServer(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return PanasonicHelper.ExtraActualData(operateResult3.Content);
	}

	public static OperateResult Write(IReadWriteDevice plc, byte station, string[] address, bool[] value)
	{
		OperateResult<List<byte[]>> operateResult = PanasonicHelper.BuildWriteCoils(station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult operateResult3 = PanasonicHelper.ExtraActualData(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult<byte[]> Read(IReadWriteDevice plc, byte station, string address, ushort length)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<List<byte[]>> operateResult = PanasonicHelper.BuildReadCommand(station, address, length, isBit: false);
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
				return operateResult2;
			}
			OperateResult<byte[]> operateResult3 = PanasonicHelper.ExtraActualData(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			list.AddRange(operateResult3.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult Write(IReadWriteDevice plc, byte station, string address, byte[] value)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = PanasonicHelper.BuildWriteCommand(station, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return PanasonicHelper.ExtraActualData(operateResult2.Content);
	}

	public static async Task<OperateResult<bool>> ReadBoolAsync(IReadWriteDevice plc, byte station, string address)
	{
		if (CheckBoolOnWordAddress(address))
		{
			return ByteTransformHelper.GetResultFromArray(await HslHelper.ReadBoolAsync(plc, address, 1).ConfigureAwait(continueOnCapturedContext: false));
		}
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> command = PanasonicHelper.BuildReadOneCoil(station, address);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(command);
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(read);
		}
		return ByteTransformHelper.GetResultFromArray(PanasonicHelper.ExtraActualBool(read.Content));
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteDevice plc, byte station, string address, ushort length)
	{
		if (CheckBoolOnWordAddress(address))
		{
			return await HslHelper.ReadBoolAsync(plc, address, length).ConfigureAwait(continueOnCapturedContext: false);
		}
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<string, int> analysis = PanasonicHelper.AnalysisAddress(address);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(analysis);
		}
		OperateResult<List<byte[]>> command = PanasonicHelper.BuildReadCommand(station, address, length, isBit: true);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content[i]).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult<byte[]> extra = PanasonicHelper.ExtraActualData(read.Content);
			if (!extra.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(extra);
			}
			list.AddRange(extra.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray().ToBoolArray().SelectMiddle(analysis.Content2 % 16, length));
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteDevice plc, byte station, string[] address)
	{
		OperateResult<List<byte[]>> command = PanasonicHelper.BuildReadCoils(station, address);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		List<bool> list = new List<bool>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content[i]).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult<bool[]> extra = PanasonicHelper.ExtraActualBool(read.Content);
			if (!extra.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(extra);
			}
			list.AddRange(extra.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, byte station, string address, bool value)
	{
		if (CheckBoolOnWordAddress(address))
		{
			return HslHelper.WriteBool(plc, address, new bool[1] { value });
		}
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> command = PanasonicHelper.BuildWriteOneCoil(station, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		return PanasonicHelper.ExtraActualData(read.Content);
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, byte station, string address, bool[] values)
	{
		if (CheckBoolOnWordAddress(address))
		{
			return HslHelper.WriteBool(plc, address, values);
		}
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<string, int> analysis = PanasonicHelper.AnalysisAddress(address);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(analysis);
		}
		if (analysis.Content2 % 16 != 0)
		{
			return new OperateResult(StringResources.Language.PanasonicAddressBitStartMulti16);
		}
		if (values.Length % 16 != 0)
		{
			return new OperateResult(StringResources.Language.PanasonicBoolLengthMulti16);
		}
		OperateResult<byte[]> command = PanasonicHelper.BuildWriteCommand(values: SoftBasic.BoolArrayToByte(values), station: station, address: address);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		return PanasonicHelper.ExtraActualData(read.Content);
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, byte station, string[] address, bool[] value)
	{
		OperateResult<List<byte[]>> command = PanasonicHelper.BuildWriteCoils(station, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content[i]).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				return read;
			}
			OperateResult extra = PanasonicHelper.ExtraActualData(read.Content);
			if (!extra.IsSuccess)
			{
				return extra;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteDevice plc, byte station, string address, ushort length)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<List<byte[]>> command = PanasonicHelper.BuildReadCommand(station, address, length, isBit: false);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content[i]).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				return read;
			}
			OperateResult<byte[]> extra = PanasonicHelper.ExtraActualData(read.Content);
			if (!extra.IsSuccess)
			{
				return extra;
			}
			list.AddRange(extra.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, byte station, string address, byte[] value)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> command = PanasonicHelper.BuildWriteCommand(station, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		return PanasonicHelper.ExtraActualData(read.Content);
	}

	private static OperateResult<string> GetPlcType(byte[] data)
	{
		try
		{
			string text = Encoding.ASCII.GetString(data, 0, 2);
			if (1 == 0)
			{
			}
			OperateResult<string> result = text switch
			{
				"03" => OperateResult.CreateSuccessResult("FP3"), 
				"02" => OperateResult.CreateSuccessResult("FP5"), 
				"05" => OperateResult.CreateSuccessResult("FP-E"), 
				_ => OperateResult.CreateSuccessResult(text), 
			};
			if (1 == 0)
			{
			}
			return result;
		}
		catch (Exception ex)
		{
			return new OperateResult<string>("Get plctype failed : " + ex.Message + Environment.NewLine + "Source: " + data.ToHexString());
		}
	}

	public static OperateResult<string> ReadPlcType(IReadWriteDevice plc, byte station)
	{
		OperateResult<byte[]> operateResult = PanasonicHelper.BuildReadPlcModel(station);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = PanasonicHelper.ExtraActualData(operateResult2.Content, parseData: false);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult3);
		}
		return GetPlcType(operateResult3.Content);
	}
}
