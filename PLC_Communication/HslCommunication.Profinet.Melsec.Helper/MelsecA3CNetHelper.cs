using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Net;

namespace HslCommunication.Profinet.Melsec.Helper;

public class MelsecA3CNetHelper
{
	public static byte[] PackCommand(IReadWriteA3C plc, byte[] mcCommand, byte station = 0)
	{
		MemoryStream memoryStream = new MemoryStream();
		if (plc.Format != 3)
		{
			memoryStream.WriteByte(5);
		}
		else
		{
			memoryStream.WriteByte(2);
		}
		if (plc.Format == 2)
		{
			memoryStream.WriteByte(48);
			memoryStream.WriteByte(48);
		}
		memoryStream.WriteByte(70);
		memoryStream.WriteByte(57);
		memoryStream.WriteByte(SoftBasic.BuildAsciiBytesFrom(station)[0]);
		memoryStream.WriteByte(SoftBasic.BuildAsciiBytesFrom(station)[1]);
		memoryStream.WriteByte(48);
		memoryStream.WriteByte(48);
		memoryStream.WriteByte(70);
		memoryStream.WriteByte(70);
		memoryStream.WriteByte(48);
		memoryStream.WriteByte(48);
		memoryStream.Write(mcCommand, 0, mcCommand.Length);
		if (plc.Format == 3)
		{
			memoryStream.WriteByte(3);
		}
		if (plc.SumCheck)
		{
			byte[] array = memoryStream.ToArray();
			int num = 0;
			for (int i = 1; i < array.Length; i++)
			{
				num += array[i];
			}
			memoryStream.WriteByte(SoftBasic.BuildAsciiBytesFrom((byte)num)[0]);
			memoryStream.WriteByte(SoftBasic.BuildAsciiBytesFrom((byte)num)[1]);
		}
		if (plc.Format == 4)
		{
			memoryStream.WriteByte(13);
			memoryStream.WriteByte(10);
		}
		byte[] result = memoryStream.ToArray();
		memoryStream.Dispose();
		return result;
	}

	private static int GetErrorCodeOrDataStartIndex(IReadWriteA3C plc)
	{
		int result = 11;
		switch (plc.Format)
		{
		case 1:
			result = 11;
			break;
		case 2:
			result = 13;
			break;
		case 3:
			result = 15;
			break;
		case 4:
			result = 11;
			break;
		}
		return result;
	}

	public static OperateResult<byte[]> ExtraReadActualResponse(IReadWriteA3C plc, byte[] response)
	{
		try
		{
			int errorCodeOrDataStartIndex = GetErrorCodeOrDataStartIndex(plc);
			if (plc.Format == 1 || plc.Format == 2 || plc.Format == 4)
			{
				if (response[0] == 21)
				{
					int num = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
					return new OperateResult<byte[]>(num, MelsecHelper.GetErrorDescription(num));
				}
				if (response[0] != 2)
				{
					return new OperateResult<byte[]>(response[0], "Read Faild:" + SoftBasic.GetAsciiStringRender(response));
				}
			}
			else if (plc.Format == 3)
			{
				string text = Encoding.ASCII.GetString(response, 11, 4);
				if (text == "QNAK")
				{
					int num2 = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
					return new OperateResult<byte[]>(num2, MelsecHelper.GetErrorDescription(num2));
				}
				if (text != "QACK")
				{
					return new OperateResult<byte[]>(response[0], "Read Faild:" + SoftBasic.GetAsciiStringRender(response));
				}
			}
			int num3 = -1;
			for (int i = errorCodeOrDataStartIndex; i < response.Length; i++)
			{
				if (response[i] == 3)
				{
					num3 = i;
					break;
				}
			}
			if (num3 == -1)
			{
				num3 = response.Length;
			}
			return OperateResult.CreateSuccessResult(response.SelectMiddle(errorCodeOrDataStartIndex, num3 - errorCodeOrDataStartIndex));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("ExtraReadActualResponse Wrong:" + ex.Message + Environment.NewLine + "Source: " + response.ToHexString(' '));
		}
	}

	private static OperateResult CheckWriteResponse(IReadWriteA3C plc, byte[] response)
	{
		int errorCodeOrDataStartIndex = GetErrorCodeOrDataStartIndex(plc);
		try
		{
			if (plc.Format == 1 || plc.Format == 2)
			{
				if (response[0] == 21)
				{
					int num = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
					return new OperateResult<byte[]>(num, MelsecHelper.GetErrorDescription(num));
				}
				if (response[0] != 6)
				{
					return new OperateResult<byte[]>(response[0], "Write Faild:" + SoftBasic.GetAsciiStringRender(response));
				}
			}
			else if (plc.Format == 3)
			{
				if (response[0] != 2)
				{
					return new OperateResult<byte[]>(response[0], "Write Faild:" + SoftBasic.GetAsciiStringRender(response));
				}
				string text = Encoding.ASCII.GetString(response, 11, 4);
				if (text == "QNAK")
				{
					int num2 = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
					return new OperateResult<byte[]>(num2, MelsecHelper.GetErrorDescription(num2));
				}
				if (text != "QACK")
				{
					return new OperateResult<byte[]>(response[0], "Write Faild:" + SoftBasic.GetAsciiStringRender(response));
				}
			}
			else if (plc.Format == 4)
			{
				if (response[0] == 21)
				{
					int num3 = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
					return new OperateResult<byte[]>(num3, MelsecHelper.GetErrorDescription(num3));
				}
				if (response[0] != 6)
				{
					return new OperateResult<byte[]>(response[0], "Write Faild:" + SoftBasic.GetAsciiStringRender(response));
				}
			}
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("CheckWriteResponse failed: " + ex.Message + Environment.NewLine + "Content: " + SoftBasic.GetAsciiStringRender(response));
		}
	}

	public static OperateResult<byte[]> Read(IReadWriteA3C plc, string address, ushort length)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<McAddressData> operateResult = McAddressData.ParseMelsecFrom(address, length, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		ushort num = 0;
		while (num < length)
		{
			ushort num2 = (ushort)Math.Min(length - num, McHelper.GetReadWordLength(McType.MCAscii));
			operateResult.Content.Length = num2;
			byte[] mcCommand = McAsciiHelper.BuildAsciiReadMcCoreCommand(operateResult.Content, isBit: false);
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(PackCommand(plc, mcCommand, station));
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult<byte[]> operateResult3 = ExtraReadActualResponse(plc, operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			list.AddRange(MelsecHelper.TransAsciiByteArrayToByteArray(operateResult3.Content));
			num += num2;
			if (operateResult.Content.McDataType.DataType == 0)
			{
				operateResult.Content.AddressStart += num2;
			}
			else
			{
				operateResult.Content.AddressStart += num2 * 16;
			}
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteA3C plc, string address, ushort length)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<McAddressData> addressResult = McAddressData.ParseMelsecFrom(address, length, isBit: false);
		if (!addressResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(addressResult);
		}
		List<byte> bytesContent = new List<byte>();
		ushort alreadyFinished = 0;
		while (alreadyFinished < length)
		{
			ushort readLength = (ushort)Math.Min(length - alreadyFinished, McHelper.GetReadWordLength(McType.MCAscii));
			addressResult.Content.Length = readLength;
			byte[] command = McAsciiHelper.BuildAsciiReadMcCoreCommand(addressResult.Content, isBit: false);
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(PackCommand(plc, command, stat));
			if (!read.IsSuccess)
			{
				return read;
			}
			OperateResult<byte[]> check = ExtraReadActualResponse(plc, read.Content);
			if (!check.IsSuccess)
			{
				return check;
			}
			bytesContent.AddRange(MelsecHelper.TransAsciiByteArrayToByteArray(check.Content));
			alreadyFinished += readLength;
			if (addressResult.Content.McDataType.DataType == 0)
			{
				addressResult.Content.AddressStart += readLength;
			}
			else
			{
				addressResult.Content.AddressStart += readLength * 16;
			}
		}
		return OperateResult.CreateSuccessResult(bytesContent.ToArray());
	}

	public static OperateResult Write(IReadWriteA3C plc, string address, byte[] value)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<McAddressData> operateResult = McAddressData.ParseMelsecFrom(address, 0, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] mcCommand = McAsciiHelper.BuildAsciiWriteWordCoreCommand(operateResult.Content, value);
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(PackCommand(plc, mcCommand, station));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckWriteResponse(plc, operateResult2.Content);
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteA3C plc, string address, byte[] value)
	{
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<McAddressData> addressResult = McAddressData.ParseMelsecFrom(address, 0, isBit: false);
		if (!addressResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(addressResult);
		}
		byte[] command = McAsciiHelper.BuildAsciiWriteWordCoreCommand(addressResult.Content, value);
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(PackCommand(plc, command, stat));
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckWriteResponse(plc, read.Content);
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteA3C plc, string address, ushort length)
	{
		if (address.IndexOf('.') > 0)
		{
			return HslHelper.ReadBool(plc, address, length);
		}
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<McAddressData> operateResult = McAddressData.ParseMelsecFrom(address, length, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		List<bool> list = new List<bool>();
		ushort num = 0;
		while (num < length)
		{
			ushort num2 = (ushort)Math.Min(length - num, McHelper.GetReadBoolLength(McType.MCAscii));
			operateResult.Content.Length = num2;
			byte[] mcCommand = McAsciiHelper.BuildAsciiReadMcCoreCommand(operateResult.Content, isBit: true);
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(PackCommand(plc, mcCommand, station));
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult2);
			}
			OperateResult<byte[]> operateResult3 = ExtraReadActualResponse(plc, operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult3);
			}
			list.AddRange(operateResult3.Content.Select((byte m) => m == 49).ToArray());
			num += num2;
			operateResult.Content.AddressStart += num2;
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteA3C plc, string address, ushort length)
	{
		if (address.IndexOf('.') > 0)
		{
			return await HslHelper.ReadBoolAsync(plc, address, length);
		}
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<McAddressData> addressResult = McAddressData.ParseMelsecFrom(address, length, isBit: true);
		if (!addressResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(addressResult);
		}
		List<bool> boolContent = new List<bool>();
		ushort alreadyFinished = 0;
		while (alreadyFinished < length)
		{
			ushort readLength = (ushort)Math.Min(length - alreadyFinished, McHelper.GetReadBoolLength(McType.MCAscii));
			addressResult.Content.Length = readLength;
			byte[] command = McAsciiHelper.BuildAsciiReadMcCoreCommand(addressResult.Content, isBit: true);
			OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(PackCommand(plc, command, stat));
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			OperateResult<byte[]> check = ExtraReadActualResponse(plc, read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(check);
			}
			boolContent.AddRange(check.Content.Select((byte m) => m == 49).ToArray());
			alreadyFinished += readLength;
			addressResult.Content.AddressStart += readLength;
		}
		return OperateResult.CreateSuccessResult(boolContent.ToArray());
	}

	public static OperateResult Write(IReadWriteA3C plc, string address, bool[] value)
	{
		if (plc.EnableWriteBitToWordRegister && address.Contains("."))
		{
			return ReadWriteNetHelper.WriteBoolWithWord(plc, address, value);
		}
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<McAddressData> operateResult = McAddressData.ParseMelsecFrom(address, 0, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		byte[] mcCommand = McAsciiHelper.BuildAsciiWriteBitCoreCommand(operateResult.Content, value);
		OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(PackCommand(plc, mcCommand, station));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return CheckWriteResponse(plc, operateResult2.Content);
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteA3C plc, string address, bool[] value)
	{
		if (plc.EnableWriteBitToWordRegister && address.Contains("."))
		{
			return await ReadWriteNetHelper.WriteBoolWithWordAsync(plc, address, value).ConfigureAwait(continueOnCapturedContext: false);
		}
		byte stat = (byte)HslHelper.ExtractParameter(ref address, "s", plc.Station);
		OperateResult<McAddressData> addressResult = McAddressData.ParseMelsecFrom(address, 0, isBit: true);
		if (!addressResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(addressResult);
		}
		byte[] command = McAsciiHelper.BuildAsciiWriteBitCoreCommand(addressResult.Content, value);
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(PackCommand(plc, command, stat));
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckWriteResponse(plc, read.Content);
	}

	public static OperateResult RemoteRun(IReadWriteA3C plc)
	{
		OperateResult<byte[]> operateResult = plc.ReadFromCoreServer(PackCommand(plc, Encoding.ASCII.GetBytes("1001000000010000"), plc.Station));
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return CheckWriteResponse(plc, operateResult.Content);
	}

	public static OperateResult RemoteStop(IReadWriteA3C plc)
	{
		OperateResult<byte[]> operateResult = plc.ReadFromCoreServer(PackCommand(plc, Encoding.ASCII.GetBytes("100200000001"), plc.Station));
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return CheckWriteResponse(plc, operateResult.Content);
	}

	public static OperateResult<string> ReadPlcType(IReadWriteA3C plc)
	{
		OperateResult<byte[]> operateResult = plc.ReadFromCoreServer(PackCommand(plc, Encoding.ASCII.GetBytes("01010000"), plc.Station));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ExtraReadActualResponse(plc, operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(operateResult2.Content, 0, 16).TrimEnd());
	}

	public static async Task<OperateResult> RemoteRunAsync(IReadWriteA3C plc)
	{
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(PackCommand(plc, Encoding.ASCII.GetBytes("1001000000010000"), plc.Station));
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckWriteResponse(plc, read.Content);
	}

	public static async Task<OperateResult> RemoteStopAsync(IReadWriteA3C plc)
	{
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(PackCommand(plc, Encoding.ASCII.GetBytes("100200000001"), plc.Station));
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckWriteResponse(plc, read.Content);
	}

	public static async Task<OperateResult<string>> ReadPlcTypeAsync(IReadWriteA3C plc)
	{
		OperateResult<byte[]> read = await plc.ReadFromCoreServerAsync(PackCommand(plc, Encoding.ASCII.GetBytes("01010000"), plc.Station));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult<byte[]> check = ExtraReadActualResponse(plc, read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(check);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(check.Content, 0, 16).TrimEnd());
	}
}
