using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Instrument.DLT.Helper;

public class DLT645Helper
{
	public static bool CheckReceiveDataComplete(MemoryStream ms)
	{
		byte[] array = ms.ToArray();
		if (array.Length < 10)
		{
			return false;
		}
		int num = FindHeadCode68H(array);
		if (num < 0)
		{
			return false;
		}
		if (array.Length < num + 10)
		{
			return false;
		}
		if (array[num + 9] + 12 + num == array.Length && array[array.Length - 1] == 22)
		{
			return true;
		}
		return false;
	}

	public static OperateResult<byte[]> GetAddressByteFromString(string address)
	{
		if (address == null || address.Length == 0)
		{
			return new OperateResult<byte[]>(StringResources.Language.DLTAddressCannotNull);
		}
		if (address.Length > 12)
		{
			return new OperateResult<byte[]>(StringResources.Language.DLTAddressCannotMoreThan12);
		}
		if (!Regex.IsMatch(address, "^[0-9A-A]+$"))
		{
			return new OperateResult<byte[]>(StringResources.Language.DLTAddressMatchFailed);
		}
		if (address.Length < 12)
		{
			address = address.PadLeft(12, '0');
		}
		return OperateResult.CreateSuccessResult(address.ToHexBytes().AsEnumerable().Reverse()
			.ToArray());
	}

	public static OperateResult<byte[]> BuildDlt645EntireCommand(string address, byte control, byte[] dataArea)
	{
		if (dataArea == null)
		{
			dataArea = new byte[0];
		}
		OperateResult<byte[]> addressByteFromString = GetAddressByteFromString(address);
		if (!addressByteFromString.IsSuccess)
		{
			return addressByteFromString;
		}
		byte[] array = new byte[12 + dataArea.Length];
		array[0] = 104;
		addressByteFromString.Content.CopyTo(array, 1);
		array[7] = 104;
		array[8] = control;
		array[9] = (byte)dataArea.Length;
		if (dataArea.Length != 0)
		{
			dataArea.CopyTo(array, 10);
			for (int i = 0; i < dataArea.Length; i++)
			{
				array[i + 10] += 51;
			}
		}
		int num = 0;
		for (int j = 0; j < array.Length - 2; j++)
		{
			num += array[j];
		}
		array[array.Length - 2] = (byte)num;
		array[array.Length - 1] = 22;
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult CheckResponseCS(byte[] response, int index)
	{
		if (response.Length > 2 + index)
		{
			int num = 0;
			for (int i = index; i < response.Length - 2; i++)
			{
				num += response[i];
			}
			num = (byte)num;
			if (num == response[response.Length - 2])
			{
				return OperateResult.CreateSuccessResult();
			}
			return new OperateResult($"CS check failed, need[{response[response.Length - 2]}] actual[{num}]");
		}
		return new OperateResult("Receive length too short: " + response.ToHexString());
	}

	public static OperateResult<string, byte[]> AnalysisBytesAddress(DLT645Type type, string address, string defaultStation, ushort length = 1)
	{
		try
		{
			string value = defaultStation;
			byte[] array = null;
			int index = 0;
			if (type == DLT645Type.DLT2007)
			{
				array = ((length == 1) ? new byte[4] : new byte[5]);
				if (length != 1)
				{
					array[4] = (byte)length;
				}
			}
			else
			{
				array = ((length == 1) ? new byte[2] : new byte[3]);
				if (length != 1)
				{
					array[0] = (byte)length;
					index = 1;
				}
			}
			if (address.IndexOf(';') > 0)
			{
				string[] array2 = address.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i].StartsWith("s="))
					{
						value = array2[i].Substring(2);
					}
					else
					{
						array2[i].ToHexBytes().AsEnumerable().Reverse()
							.ToArray()
							.CopyTo(array, index);
					}
				}
			}
			else
			{
				address.ToHexBytes().AsEnumerable().Reverse()
					.ToArray()
					.CopyTo(array, index);
			}
			return OperateResult.CreateSuccessResult(value, array);
		}
		catch (Exception ex)
		{
			return new OperateResult<string, byte[]>("Address prase wrong: " + ex.Message);
		}
	}

	public static OperateResult<string, int> AnalysisIntegerAddress(string address, string defaultStation)
	{
		try
		{
			string value = defaultStation;
			int value2 = 0;
			if (address.IndexOf(';') > 0)
			{
				string[] array = address.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].StartsWith("s="))
					{
						value = array[i].Substring(2);
					}
					else
					{
						value2 = Convert.ToInt32(array[i]);
					}
				}
			}
			else
			{
				value2 = Convert.ToInt32(address);
			}
			return OperateResult.CreateSuccessResult(value, value2);
		}
		catch (Exception ex)
		{
			return new OperateResult<string, int>(ex.Message);
		}
	}

	public static OperateResult CheckResponse(IDlt645 dlt, byte[] send, byte[] response)
	{
		if (response.Length < 9)
		{
			return new OperateResult(StringResources.Language.ReceiveDataLengthTooShort);
		}
		OperateResult operateResult = CheckResponseCS(response, 0);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = CheckStation(send, response);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if ((response[8] & 0x40) == 64)
		{
			if (response.Length < 11)
			{
				return new OperateResult(StringResources.Language.ReceiveDataLengthTooShort);
			}
			byte b = response[10];
			if (dlt.DLTType == DLT645Type.DLT2007)
			{
				if (b.GetBoolByIndex(0))
				{
					return new OperateResult(b, StringResources.Language.DLTErrorInfoBit0);
				}
				if (b.GetBoolByIndex(1))
				{
					return new OperateResult(b, StringResources.Language.DLTErrorInfoBit1);
				}
				if (b.GetBoolByIndex(2))
				{
					return new OperateResult(b, StringResources.Language.DLTErrorInfoBit2);
				}
				if (b.GetBoolByIndex(3))
				{
					return new OperateResult(b, StringResources.Language.DLTErrorInfoBit3);
				}
				if (b.GetBoolByIndex(4))
				{
					return new OperateResult(b, StringResources.Language.DLTErrorInfoBit4);
				}
				if (b.GetBoolByIndex(5))
				{
					return new OperateResult(b, StringResources.Language.DLTErrorInfoBit5);
				}
				if (b.GetBoolByIndex(6))
				{
					return new OperateResult(b, StringResources.Language.DLTErrorInfoBit6);
				}
				if (b.GetBoolByIndex(7))
				{
					return new OperateResult(b, StringResources.Language.DLTErrorInfoBit7);
				}
				return new OperateResult(b, StringResources.Language.UnknownError);
			}
			if (b.GetBoolByIndex(0))
			{
				return new OperateResult(b, StringResources.Language.DLT1997ErrorInfoBit0);
			}
			if (b.GetBoolByIndex(1))
			{
				return new OperateResult(b, StringResources.Language.DLT1997ErrorInfoBit1);
			}
			if (b.GetBoolByIndex(2))
			{
				return new OperateResult(b, StringResources.Language.DLT1997ErrorInfoBit2);
			}
			if (b.GetBoolByIndex(4))
			{
				return new OperateResult(b, StringResources.Language.DLT1997ErrorInfoBit4);
			}
			if (b.GetBoolByIndex(5))
			{
				return new OperateResult(b, StringResources.Language.DLT1997ErrorInfoBit5);
			}
			if (b.GetBoolByIndex(6))
			{
				return new OperateResult(b, StringResources.Language.DLT1997ErrorInfoBit6);
			}
			return new OperateResult(b, StringResources.Language.UnknownError);
		}
		return OperateResult.CreateSuccessResult();
	}

	private static OperateResult CheckStation(byte[] send, byte[] response)
	{
		if (send.Length < 8)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (response.Length < 8)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (send[1] == 170 && send[2] == 170 && send[3] == 170 && send[4] == 170 && send[5] == 170 && send[6] == 170)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (send[1] == 153 && send[2] == 153 && send[3] == 153 && send[4] == 153 && send[5] == 153 && send[6] == 153)
		{
			return OperateResult.CreateSuccessResult();
		}
		if (send[1] == response[1] && send[2] == response[2] && send[3] == response[3] && send[4] == response[4] && send[5] == response[5] && send[6] == response[6])
		{
			return OperateResult.CreateSuccessResult();
		}
		if (send[1] == response[6] && send[2] == response[5] && send[3] == response[4] && send[4] == response[3] && send[5] == response[2] && send[6] == response[1])
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult("Station check failed, need: " + send.SelectMiddle(1, 6).ToHexString() + " But Actual: " + response.SelectMiddle(1, 6).ToHexString());
	}

	public static int FindHeadCode68H(byte[] buffer)
	{
		if (buffer == null)
		{
			return -1;
		}
		for (int i = 0; i < buffer.Length; i++)
		{
			if (buffer[i] == 104)
			{
				return i;
			}
		}
		return -1;
	}

	private static OperateResult<byte[]> ReadWithAddress(IDlt645 dlt, string address, byte[] dataArea)
	{
		OperateResult<byte[]> operateResult = BuildDlt645EntireCommand(address, (byte)((dlt.DLTType != DLT645Type.DLT2007) ? 1u : 17u), dataArea);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = dlt.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = CheckResponse(dlt, operateResult.Content, operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult3);
		}
		try
		{
			if (dlt.DLTType == DLT645Type.DLT2007)
			{
				if (operateResult2.Content.Length < 16)
				{
					return OperateResult.CreateSuccessResult(new byte[0]);
				}
				return OperateResult.CreateSuccessResult(operateResult2.Content.SelectMiddle(14, operateResult2.Content.Length - 16));
			}
			if (operateResult2.Content.Length < 14)
			{
				return OperateResult.CreateSuccessResult(new byte[0]);
			}
			return OperateResult.CreateSuccessResult(operateResult2.Content.SelectMiddle(12, operateResult2.Content.Length - 14));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("ReadWithAddress failed: " + ex.Message + Environment.NewLine + "Source: " + operateResult2.Content.ToHexString(' '));
		}
	}

	public static OperateResult<byte[]> Read(IDlt645 dlt, string address, ushort length)
	{
		OperateResult<string, byte[]> operateResult = AnalysisBytesAddress(dlt.DLTType, address, dlt.Station, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return ReadWithAddress(dlt, operateResult.Content1, operateResult.Content2);
	}

	public static OperateResult<string[]> ReadStringArray(IDlt645 dlt, string address)
	{
		bool reverse = HslHelper.ExtractBooleanParameter(ref address, "reverse", defaultValue: true);
		OperateResult<string, byte[]> operateResult = AnalysisBytesAddress(dlt.DLTType, address, dlt.Station, 1);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadWithAddress(dlt, operateResult.Content1, operateResult.Content2);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult2);
		}
		return DLTTransform.TransStringsFromDLt(dlt.DLTType, operateResult2.Content, operateResult.Content2, reverse);
	}

	public static OperateResult<double[]> ReadDouble(IDlt645 dlt, string address, ushort length)
	{
		OperateResult<string[]> operateResult = ReadStringArray(dlt, address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double[]>(operateResult);
		}
		try
		{
			return OperateResult.CreateSuccessResult((from m in operateResult.Content.Take(length)
				select double.Parse(m)).ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<double[]>("double.Parse failed: " + ex.Message + Environment.NewLine + "Source: " + operateResult.Content.ToArrayString());
		}
	}

	public static OperateResult Function1C(IDlt645 dlt, string password, string opCode, string station, byte controlType, DateTime validTime)
	{
		byte[] array = new byte[8] { controlType, 0, 0, 0, 0, 0, 0, 0 };
		validTime.ToString("ss-mm-HH-dd-MM-yy").ToHexBytes().CopyTo(array, 2);
		byte[] array2 = null;
		byte[] dataArea = ((dlt.DLTType != DLT645Type.DLT2007) ? array : SoftBasic.SpliceArray<byte>(password.ToHexBytes(), opCode.ToHexBytes(), array));
		OperateResult<byte[]> operateResult = BuildDlt645EntireCommand(string.IsNullOrEmpty(station) ? dlt.Station : station, 28, dataArea);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = dlt.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckResponse(dlt, operateResult.Content, operateResult2.Content);
	}

	public static OperateResult Write(IDlt645 dlt, string password, string opCode, string address, byte[] value)
	{
		OperateResult<string, byte[]> operateResult = AnalysisBytesAddress(dlt.DLTType, address, dlt.Station, 1);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = null;
		byte[] dataArea = ((dlt.DLTType != DLT645Type.DLT2007) ? SoftBasic.SpliceArray<byte>(operateResult.Content2, value) : SoftBasic.SpliceArray<byte>(operateResult.Content2, password.ToHexBytes(), opCode.ToHexBytes(), value));
		OperateResult<byte[]> operateResult2 = BuildDlt645EntireCommand(operateResult.Content1, (byte)((dlt.DLTType == DLT645Type.DLT2007) ? 20u : 4u), dataArea);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = dlt.ReadFromCoreServer(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return CheckResponse(dlt, operateResult2.Content, operateResult3.Content);
	}

	public static OperateResult Write(IDlt645 dlt, string password, string opCode, string address, string[] value)
	{
		bool reverse = HslHelper.ExtractBooleanParameter(ref address, "reverse", defaultValue: true);
		OperateResult<string, byte[]> operateResult = AnalysisBytesAddress(dlt.DLTType, address, dlt.Station, 1);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = DLTTransform.TransDltFromStrings(dlt.DLTType, value, operateResult.Content2, reverse);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return Write(dlt, password, opCode, address, operateResult2.Content);
	}

	public static OperateResult<string> ReadAddress(IDlt645 dlt)
	{
		OperateResult<byte[]> operateResult = BuildDlt645EntireCommand("AAAAAAAAAAAA", 19, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = dlt.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		OperateResult operateResult3 = CheckResponse(dlt, operateResult.Content, operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult3);
		}
		dlt.Station = operateResult2.Content.SelectMiddle(1, 6).AsEnumerable().Reverse()
			.ToArray()
			.ToHexString();
		return OperateResult.CreateSuccessResult(operateResult2.Content.SelectMiddle(1, 6).AsEnumerable().Reverse()
			.ToArray()
			.ToHexString());
	}

	public static OperateResult WriteAddress(IDlt645 dlt, string address)
	{
		OperateResult<byte[]> addressByteFromString = GetAddressByteFromString(address);
		if (!addressByteFromString.IsSuccess)
		{
			return addressByteFromString;
		}
		OperateResult<byte[]> operateResult = BuildDlt645EntireCommand("AAAAAAAAAAAA", (byte)((dlt.DLTType == DLT645Type.DLT2007) ? 21u : 10u), addressByteFromString.Content);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = dlt.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = CheckResponse(dlt, operateResult.Content, operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		if (SoftBasic.IsTwoBytesEquel(operateResult2.Content.SelectMiddle(1, 6), GetAddressByteFromString(address).Content))
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult(StringResources.Language.DLTErrorWriteReadCheckFailed);
	}

	public static OperateResult BroadcastTime(IDlt645 dlt, DateTime dateTime)
	{
		string value = $"{dateTime.Second:D2}{dateTime.Minute:D2}{dateTime.Hour:D2}{dateTime.Day:D2}{dateTime.Month:D2}{dateTime.Year % 100:D2}";
		OperateResult<byte[]> operateResult = BuildDlt645EntireCommand("999999999999", (byte)((dlt.DLTType == DLT645Type.DLT2007) ? 8u : 8u), value.ToHexBytes());
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return dlt.ReadFromCoreServer(operateResult.Content, hasResponseData: false);
	}

	public static OperateResult FreezeCommand(IDlt645 dlt, string dataArea)
	{
		OperateResult<string, byte[]> operateResult = AnalysisBytesAddress(dlt.DLTType, dataArea, dlt.Station, 1);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = BuildDlt645EntireCommand(operateResult.Content1, 22, operateResult.Content2);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if (operateResult.Content1 == "999999999999")
		{
			return dlt.ReadFromCoreServer(operateResult2.Content, hasResponseData: false);
		}
		OperateResult<byte[]> operateResult3 = dlt.ReadFromCoreServer(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return CheckResponse(dlt, operateResult2.Content, operateResult3.Content);
	}

	private static OperateResult<byte[]> BuildChangeBaudRateCommand(IDlt645 dlt, string baudRate, out byte code)
	{
		code = 0;
		OperateResult<string, int> operateResult = AnalysisIntegerAddress(baudRate, dlt.Station);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (dlt.DLTType == DLT645Type.DLT2007)
		{
			switch (operateResult.Content2)
			{
			case 600:
				code = 2;
				break;
			case 1200:
				code = 4;
				break;
			case 2400:
				code = 8;
				break;
			case 4800:
				code = 16;
				break;
			case 9600:
				code = 32;
				break;
			case 19200:
				code = 64;
				break;
			default:
				return new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction);
			}
		}
		else
		{
			switch (operateResult.Content2)
			{
			case 300:
				code = 2;
				break;
			case 600:
				code = 4;
				break;
			case 2400:
				code = 16;
				break;
			case 4800:
				code = 32;
				break;
			case 9600:
				code = 64;
				break;
			default:
				return new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction);
			}
		}
		return BuildDlt645EntireCommand(operateResult.Content1, (byte)((dlt.DLTType == DLT645Type.DLT2007) ? 23u : 12u), new byte[1] { code });
	}

	public static OperateResult ChangeBaudRate(IDlt645 dlt, string baudRate)
	{
		byte code;
		OperateResult<byte[]> operateResult = BuildChangeBaudRateCommand(dlt, baudRate, out code);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = dlt.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = CheckResponse(dlt, operateResult.Content, operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		if (operateResult2.Content[10] == code)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult(StringResources.Language.DLTErrorWriteReadCheckFailed);
	}

	private static async Task<OperateResult<byte[]>> ReadWithAddressAsync(IDlt645 dlt, string address, byte[] dataArea)
	{
		OperateResult<byte[]> command = BuildDlt645EntireCommand(address, (byte)((dlt.DLTType != DLT645Type.DLT2007) ? 1u : 17u), dataArea);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await dlt.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = CheckResponse(dlt, command.Content, read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		try
		{
			if (dlt.DLTType == DLT645Type.DLT2007)
			{
				if (read.Content.Length < 16)
				{
					return OperateResult.CreateSuccessResult(new byte[0]);
				}
				return OperateResult.CreateSuccessResult(read.Content.SelectMiddle(14, read.Content.Length - 16));
			}
			if (read.Content.Length < 14)
			{
				return OperateResult.CreateSuccessResult(new byte[0]);
			}
			return OperateResult.CreateSuccessResult(read.Content.SelectMiddle(12, read.Content.Length - 14));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("ReadWithAddress failed: " + ex.Message + Environment.NewLine + "Source: " + read.Content.ToHexString(' '));
		}
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IDlt645 dlt, string address, ushort length)
	{
		OperateResult<string, byte[]> analysis = AnalysisBytesAddress(dlt.DLTType, address, dlt.Station, length);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(analysis);
		}
		return await ReadWithAddressAsync(dlt, analysis.Content1, analysis.Content2);
	}

	public static async Task<OperateResult<double[]>> ReadDoubleAsync(IDlt645 dlt, string address, ushort length)
	{
		OperateResult<string[]> read = await ReadStringArrayAsync(dlt, address);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double[]>(read);
		}
		try
		{
			return OperateResult.CreateSuccessResult((from m in read.Content.Take(length)
				select double.Parse(m)).ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<double[]>("double.Parse failed: " + ex.Message + Environment.NewLine + "Source: " + read.Content.ToArrayString());
		}
	}

	public static async Task<OperateResult<string[]>> ReadStringArrayAsync(IDlt645 dlt, string address)
	{
		bool reverse = HslHelper.ExtractBooleanParameter(ref address, "reverse", defaultValue: true);
		OperateResult<string, byte[]> analysis = AnalysisBytesAddress(dlt.DLTType, address, dlt.Station, 1);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(analysis);
		}
		OperateResult<byte[]> read = await ReadWithAddressAsync(dlt, analysis.Content1, analysis.Content2);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(read);
		}
		return DLTTransform.TransStringsFromDLt(dlt.DLTType, read.Content, analysis.Content2, reverse);
	}

	public static async Task<OperateResult> WriteAsync(IDlt645 dlt, string password, string opCode, string address, byte[] value)
	{
		OperateResult<string, byte[]> analysis = AnalysisBytesAddress(dlt.DLTType, address, dlt.Station, 1);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(analysis);
		}
		OperateResult<byte[]> command = BuildDlt645EntireCommand(dataArea: (dlt.DLTType != DLT645Type.DLT2007) ? SoftBasic.SpliceArray<byte>(analysis.Content2, value) : SoftBasic.SpliceArray<byte>(analysis.Content2, password.ToHexBytes(), opCode.ToHexBytes(), value), address: analysis.Content1, control: (byte)((dlt.DLTType == DLT645Type.DLT2007) ? 20u : 4u));
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await dlt.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckResponse(dlt, command.Content, read.Content);
	}

	public static async Task<OperateResult> WriteAsync(IDlt645 dlt, string password, string opCode, string address, string[] value)
	{
		bool reverse = HslHelper.ExtractBooleanParameter(ref address, "reverse", defaultValue: true);
		OperateResult<string, byte[]> analysis = AnalysisBytesAddress(dlt.DLTType, address, dlt.Station, 1);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(analysis);
		}
		OperateResult<byte[]> command = DLTTransform.TransDltFromStrings(dlt.DLTType, value, analysis.Content2, reverse);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		return await WriteAsync(dlt, password, opCode, address, command.Content);
	}

	public static async Task<OperateResult<string>> ReadAddressAsync(IDlt645 dlt)
	{
		OperateResult<byte[]> command = BuildDlt645EntireCommand("AAAAAAAAAAAA", 19, null);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(command);
		}
		OperateResult<byte[]> read = await dlt.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult check = CheckResponse(dlt, command.Content, read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(check);
		}
		dlt.Station = read.Content.SelectMiddle(1, 6).AsEnumerable().Reverse()
			.ToArray()
			.ToHexString();
		return OperateResult.CreateSuccessResult(read.Content.SelectMiddle(1, 6).AsEnumerable().Reverse()
			.ToArray()
			.ToHexString());
	}

	public static async Task<OperateResult> WriteAddressAsync(IDlt645 dlt, string address)
	{
		OperateResult<byte[]> add = GetAddressByteFromString(address);
		if (!add.IsSuccess)
		{
			return add;
		}
		OperateResult<byte[]> command = BuildDlt645EntireCommand("AAAAAAAAAAAA", (byte)((dlt.DLTType == DLT645Type.DLT2007) ? 21u : 10u), add.Content);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await dlt.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = CheckResponse(dlt, command.Content, read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		if (SoftBasic.IsTwoBytesEquel(read.Content.SelectMiddle(1, 6), GetAddressByteFromString(address).Content))
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult(StringResources.Language.DLTErrorWriteReadCheckFailed);
	}

	public static async Task<OperateResult> BroadcastTimeAsync(IDlt645 dlt, DateTime dateTime, Func<byte[], bool, bool, Task<OperateResult<byte[]>>> func)
	{
		OperateResult<byte[]> command = BuildDlt645EntireCommand("999999999999", dataArea: $"{dateTime.Second:D2}{dateTime.Minute:D2}{dateTime.Hour:D2}{dateTime.Day:D2}{dateTime.Month:D2}{dateTime.Year % 100:D2}".ToHexBytes(), control: (byte)((dlt.DLTType == DLT645Type.DLT2007) ? 8u : 8u));
		if (!command.IsSuccess)
		{
			return command;
		}
		return await func(command.Content, arg2: false, arg3: true);
	}

	public static async Task<OperateResult> FreezeCommandAsync(DLT645OverTcp dlt, string dataArea)
	{
		OperateResult<string, byte[]> analysis = AnalysisBytesAddress(dlt.DLTType, dataArea, dlt.Station, 1);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(analysis);
		}
		OperateResult<byte[]> command = BuildDlt645EntireCommand(analysis.Content1, 22, analysis.Content2);
		if (!command.IsSuccess)
		{
			return command;
		}
		if (analysis.Content1 == "999999999999")
		{
			return await dlt.ReadFromCoreServerAsync(command.Content, hasResponseData: false, usePackAndUnpack: true);
		}
		OperateResult<byte[]> read = await dlt.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckResponse(dlt, command.Content, read.Content);
	}

	public static async Task<OperateResult> ChangeBaudRateAsync(IDlt645 dlt, string baudRate)
	{
		byte code;
		OperateResult<byte[]> command = BuildChangeBaudRateCommand(dlt, baudRate, out code);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await dlt.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = CheckResponse(dlt, command.Content, read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		if (read.Content[10] == code)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult(StringResources.Language.DLTErrorWriteReadCheckFailed);
	}
}
