using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Serial;

namespace HslCommunication.Profinet.FATEK.Helper;

public class FatekProgramHelper
{
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

	public static bool CheckReceiveDataComplete(MemoryStream ms)
	{
		byte[] array = ms.ToArray();
		if (array.Length < 5)
		{
			return false;
		}
		return array[array.Length - 1] == 3;
	}

	public static byte[] PackFatekCommand(byte station, string cmd)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('\u0002');
		stringBuilder.Append(station.ToString("X2"));
		byte[] array = new byte[6 + cmd.Length];
		array[0] = 2;
		array[1] = SoftBasic.BuildAsciiBytesFrom(station)[0];
		array[2] = SoftBasic.BuildAsciiBytesFrom(station)[1];
		Encoding.ASCII.GetBytes(cmd).CopyTo(array, 3);
		SoftLRC.CalculateAccAndFill(array, 0, 3);
		array[array.Length - 1] = 3;
		return array;
	}

	public static OperateResult<List<byte[]>> BuildReadWordCommand(byte station, string address, ushort length)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FatekProgramAddress> operateResult = FatekProgramAddress.ParseFrom(address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<List<byte[]>>();
		}
		List<byte[]> list = new List<byte[]>();
		int[] array = SoftBasic.SplitIntegerToArray(length, 64);
		for (int i = 0; i < array.Length; i++)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("46");
			stringBuilder.Append(array[i].ToString("X2"));
			if (operateResult.Content.DataCode.StartsWith("X") || operateResult.Content.DataCode.StartsWith("Y") || operateResult.Content.DataCode.StartsWith("M") || operateResult.Content.DataCode.StartsWith("S") || operateResult.Content.DataCode.StartsWith("T") || operateResult.Content.DataCode.StartsWith("C"))
			{
				stringBuilder.Append("W");
			}
			stringBuilder.Append(operateResult.Content.ToString());
			list.Add(PackFatekCommand(station, stringBuilder.ToString()));
			if (operateResult.Content.DataCode.StartsWith("X") || operateResult.Content.DataCode.StartsWith("Y") || operateResult.Content.DataCode.StartsWith("M") || operateResult.Content.DataCode.StartsWith("S") || operateResult.Content.DataCode.StartsWith("T") || operateResult.Content.DataCode.StartsWith("C"))
			{
				operateResult.Content.AddressStart += array[i] * 16;
			}
			else
			{
				operateResult.Content.AddressStart += array[i];
			}
		}
		return OperateResult.CreateSuccessResult(list);
	}

	public static OperateResult<List<byte[]>> BuildReadBoolCommand(byte station, string address, ushort length)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FatekProgramAddress> operateResult = FatekProgramAddress.ParseFrom(address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<List<byte[]>>();
		}
		List<byte[]> list = new List<byte[]>();
		int[] array = SoftBasic.SplitIntegerToArray(length, 255);
		for (int i = 0; i < array.Length; i++)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("44");
			stringBuilder.Append(array[i].ToString("X2"));
			stringBuilder.Append(operateResult.Content.ToString());
			list.Add(PackFatekCommand(station, stringBuilder.ToString()));
			operateResult.Content.AddressStart += array[i];
		}
		return OperateResult.CreateSuccessResult(list);
	}

	public static byte[] ExtraResponse(byte[] response, ushort length)
	{
		byte[] array = new byte[length * 2];
		for (int i = 0; i < array.Length / 2; i++)
		{
			ushort value = Convert.ToUInt16(Encoding.ASCII.GetString(response, i * 4 + 6, 4), 16);
			BitConverter.GetBytes(value).CopyTo(array, i * 2);
		}
		return array;
	}

	public static OperateResult<byte[]> BuildWriteBoolCommand(byte station, string address, bool[] value)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FatekProgramAddress> operateResult = FatekProgramAddress.ParseFrom(address, 0);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<byte[]>();
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("45");
		stringBuilder.Append(value.Length.ToString("X2"));
		stringBuilder.Append(operateResult.Content.ToString());
		for (int i = 0; i < value.Length; i++)
		{
			stringBuilder.Append(value[i] ? "1" : "0");
		}
		return OperateResult.CreateSuccessResult(PackFatekCommand(station, stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildWriteByteCommand(byte station, string address, byte[] value)
	{
		station = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<FatekProgramAddress> operateResult = FatekProgramAddress.ParseFrom(address, 0);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<byte[]>();
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("47");
		stringBuilder.Append((value.Length / 2).ToString("X2"));
		if (operateResult.Content.DataCode.StartsWith("X") || operateResult.Content.DataCode.StartsWith("Y") || operateResult.Content.DataCode.StartsWith("M") || operateResult.Content.DataCode.StartsWith("S") || operateResult.Content.DataCode.StartsWith("T") || operateResult.Content.DataCode.StartsWith("C"))
		{
			stringBuilder.Append("W");
		}
		stringBuilder.Append(operateResult.Content.ToString());
		byte[] array = new byte[value.Length * 2];
		for (int i = 0; i < value.Length / 2; i++)
		{
			SoftBasic.BuildAsciiBytesFrom(BitConverter.ToUInt16(value, i * 2)).CopyTo(array, 4 * i);
		}
		stringBuilder.Append(Encoding.ASCII.GetString(array));
		return OperateResult.CreateSuccessResult(PackFatekCommand(station, stringBuilder.ToString()));
	}

	public static OperateResult CheckResponse(byte[] content)
	{
		try
		{
			if (content[0] != 2)
			{
				return new OperateResult(content[0], "Write Faild:" + SoftBasic.ByteToHexString(content, ' '));
			}
			if (content[5] != 48)
			{
				return new OperateResult(content[5], GetErrorDescriptionFromCode((char)content[5]));
			}
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			return new OperateResult("CheckResponse failed: " + ex.Message + Environment.NewLine + "Source: " + content.ToHexString(' '));
		}
	}

	public static string GetErrorDescriptionFromCode(char code)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			'2' => StringResources.Language.FatekStatus02, 
			'3' => StringResources.Language.FatekStatus03, 
			'4' => StringResources.Language.FatekStatus04, 
			'5' => StringResources.Language.FatekStatus05, 
			'6' => StringResources.Language.FatekStatus06, 
			'7' => StringResources.Language.FatekStatus07, 
			'9' => StringResources.Language.FatekStatus09, 
			'A' => StringResources.Language.FatekStatus10, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<byte[]> Read(IReadWriteDevice device, byte station, string address, ushort length)
	{
		OperateResult<List<byte[]>> operateResult = BuildReadWordCommand(station, address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		int[] array = SoftBasic.SplitIntegerToArray(length, 64);
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = device.ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult2);
			}
			OperateResult operateResult3 = CheckResponse(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3.ConvertFailed<byte[]>();
			}
			list.AddRange(ExtraResponse(operateResult2.Content, (ushort)array[i]));
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteDevice device, byte station, string address, ushort length)
	{
		OperateResult<List<byte[]>> command = BuildReadWordCommand(station, address, length);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		List<byte> content = new List<byte>();
		int[] splits = SoftBasic.SplitIntegerToArray(length, 64);
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(read);
			}
			OperateResult check = CheckResponse(read.Content);
			if (!check.IsSuccess)
			{
				return check.ConvertFailed<byte[]>();
			}
			content.AddRange(ExtraResponse(read.Content, (ushort)splits[i]));
		}
		return OperateResult.CreateSuccessResult(content.ToArray());
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
		OperateResult operateResult3 = CheckResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
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
		OperateResult check = CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteDevice device, byte station, string address, ushort length)
	{
		OperateResult<List<byte[]>> operateResult = BuildReadBoolCommand(station, address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		List<bool> list = new List<bool>();
		int[] array = SoftBasic.SplitIntegerToArray(length, 255);
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = device.ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult2);
			}
			OperateResult operateResult3 = CheckResponse(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3.ConvertFailed<bool[]>();
			}
			if (operateResult2.Content.Length < 6 + array[i])
			{
				return new OperateResult<bool[]>(StringResources.Language.ReceiveDataLengthTooShort + " Source: " + operateResult2.Content.ToHexString(' '));
			}
			list.AddRange(from m in operateResult2.Content.SelectMiddle(6, array[i])
				select m == 49);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteDevice device, byte station, string address, ushort length)
	{
		OperateResult<List<byte[]>> command = BuildReadBoolCommand(station, address, length);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		List<bool> content = new List<bool>();
		int[] splits = SoftBasic.SplitIntegerToArray(length, 255);
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(command.Content[i]);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult check = CheckResponse(read.Content);
			if (!check.IsSuccess)
			{
				return check.ConvertFailed<bool[]>();
			}
			if (read.Content.Length < 6 + splits[i])
			{
				return new OperateResult<bool[]>(StringResources.Language.ReceiveDataLengthTooShort + " Source: " + read.Content.ToHexString(' '));
			}
			content.AddRange(from m in read.Content.SelectMiddle(6, splits[i])
				select m == 49);
		}
		return OperateResult.CreateSuccessResult(content.ToArray());
	}

	public static OperateResult Write(IReadWriteDevice device, byte station, string address, bool[] value)
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
		OperateResult operateResult3 = CheckResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice device, byte station, string address, bool[] value)
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
		OperateResult check = CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult Run(IReadWriteDevice device, byte station)
	{
		return device.ReadFromCoreServer(PackFatekCommand(station, "411")).Then(CheckResponse);
	}

	public static async Task<OperateResult> RunAsync(IReadWriteDevice device, byte station)
	{
		return (await device.ReadFromCoreServerAsync(PackFatekCommand(station, "411"))).Then(CheckResponse);
	}

	public static OperateResult Stop(IReadWriteDevice device, byte station)
	{
		return device.ReadFromCoreServer(PackFatekCommand(station, "410")).Then(CheckResponse);
	}

	public static async Task<OperateResult> StopAsync(IReadWriteDevice device, byte station)
	{
		return (await device.ReadFromCoreServerAsync(PackFatekCommand(station, "410"))).Then(CheckResponse);
	}

	public static OperateResult<bool[]> ReadStatus(IReadWriteDevice device, byte station)
	{
		OperateResult<byte[]> operateResult = device.ReadFromCoreServer(PackFatekCommand(station, "40"));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		OperateResult operateResult2 = CheckResponse(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2.ConvertFailed<bool[]>();
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(operateResult.Content, 6, 2).ToHexBytes().ToBoolArray());
	}

	public static async Task<OperateResult<bool[]>> ReadStatusAsync(IReadWriteDevice device, byte station)
	{
		OperateResult<byte[]> read = await device.ReadFromCoreServerAsync(PackFatekCommand(station, "40"));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		OperateResult check = CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return check.ConvertFailed<bool[]>();
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(read.Content, 6, 2).ToHexBytes().ToBoolArray());
	}
}
