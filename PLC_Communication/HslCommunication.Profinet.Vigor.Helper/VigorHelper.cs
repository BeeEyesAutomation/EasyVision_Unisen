using System.Collections.Generic;
using System.Threading.Tasks;
using HslCommunication.Core;

namespace HslCommunication.Profinet.Vigor.Helper;

public class VigorHelper
{
	public static OperateResult<byte[]> Read(IReadWriteDevice plc, byte station, string address, ushort length)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<List<byte[]>> operateResult = VigorVsHelper.BuildReadCommand(station2, address, length, isBool: false);
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
			OperateResult<byte[]> operateResult3 = VigorVsHelper.CheckResponseContent(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			list.AddRange(operateResult3.Content);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteDevice plc, byte station, string address, ushort length)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<List<byte[]>> operateResult = VigorVsHelper.BuildReadCommand(station2, address, length, isBool: true);
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
			OperateResult<byte[]> operateResult3 = VigorVsHelper.CheckResponseContent(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			list.AddRange(operateResult3.Content.ToBoolArray().SelectBegin(length));
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult Write(IReadWriteDevice plc, byte station, string address, byte[] value)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = VigorVsHelper.BuildWriteWordCommand(station2, address, value);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return VigorVsHelper.CheckResponseContent(operateResult2.Content);
	}

	public static OperateResult Write(IReadWriteDevice plc, byte station, string address, bool[] value)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = VigorVsHelper.BuildWriteBoolCommand(station2, address, value);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return VigorVsHelper.CheckResponseContent(operateResult2.Content);
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteDevice plc, byte station, string address, ushort length)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<List<byte[]>> command = VigorVsHelper.BuildReadCommand(stat, address, length, isBool: false);
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
			OperateResult<byte[]> check = VigorVsHelper.CheckResponseContent(read.Content);
			if (!check.IsSuccess)
			{
				return check;
			}
			result.AddRange(check.Content);
		}
		return OperateResult.CreateSuccessResult(result.ToArray());
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteDevice plc, byte station, string address, ushort length)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<List<byte[]>> command = VigorVsHelper.BuildReadCommand(stat, address, length, isBool: true);
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
			OperateResult<byte[]> check = VigorVsHelper.CheckResponseContent(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(check);
			}
			result.AddRange(check.Content.ToBoolArray().SelectBegin(length));
		}
		return OperateResult.CreateSuccessResult(result.ToArray());
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, byte station, string address, byte[] value)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> command = VigorVsHelper.BuildWriteWordCommand(stat, address, value);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return VigorVsHelper.CheckResponseContent(read.Content);
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteDevice plc, byte station, string address, bool[] value)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> command = VigorVsHelper.BuildWriteBoolCommand(stat, address, value);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return VigorVsHelper.CheckResponseContent(read.Content);
	}
}
