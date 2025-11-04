using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Net;

namespace HslCommunication.Profinet.Melsec.Helper;

public class McHelper
{
	public static int GetReadWordLength(McType type)
	{
		if (type == McType.McBinary || type == McType.McRBinary)
		{
			return 950;
		}
		return 460;
	}

	public static int GetReadBoolLength(McType type)
	{
		if (type == McType.McBinary || type == McType.McRBinary)
		{
			return 7168;
		}
		return 3584;
	}

	public static OperateResult<byte[]> Read(IReadWriteMc mc, string address, ushort length)
	{
		if (address.StartsWith("s=") || address.StartsWith("S="))
		{
			if (mc.McType == McType.McBinary)
			{
				return McBinaryHelper.ReadTags(mc, new string[1] { address.Substring(2) }, new ushort[1] { length });
			}
			if (mc.McType == McType.MCAscii)
			{
				return McAsciiHelper.ReadTags(mc, new string[1] { address.Substring(2) }, new ushort[1] { length });
			}
			return new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction);
		}
		if ((mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "ext=[0-9]+;", RegexOptions.IgnoreCase))
		{
			string value = Regex.Match(address, "ext=[0-9]+;").Value;
			ushort extend = ushort.Parse(Regex.Match(value, "[0-9]+").Value);
			return ReadExtend(mc, extend, address.Substring(value.Length), length);
		}
		if ((mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "mem=", RegexOptions.IgnoreCase))
		{
			return ReadMemory(mc, address.Substring(4), length);
		}
		if ((mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "module=[0-9]+;", RegexOptions.IgnoreCase))
		{
			string value2 = Regex.Match(address, "module=[0-9]+;").Value;
			ushort module = ushort.Parse(Regex.Match(value2, "[0-9]+").Value);
			return ReadSmartModule(mc, module, address.Substring(value2.Length), (ushort)(length * 2));
		}
		OperateResult<McAddressData> operateResult = mc.McAnalysisAddress(address, length, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		List<byte> list = new List<byte>();
		ushort num = 0;
		while (num < length)
		{
			ushort num2 = (ushort)Math.Min(length - num, GetReadWordLength(mc.McType));
			operateResult.Content.Length = num2;
			byte[] send = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadMcCoreCommand(operateResult.Content, isBit: false) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadMcCoreCommand(operateResult.Content, isBit: false) : ((mc.McType == McType.McRBinary) ? MelsecMcRNet.BuildReadMcCoreCommand(operateResult.Content, isBit: false) : null)));
			OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(send);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			list.AddRange(mc.ExtractActualData(operateResult2.Content, isBit: false));
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

	public static OperateResult Write(IReadWriteMc mc, string address, byte[] value)
	{
		if (address.StartsWith("s=") || address.StartsWith("S="))
		{
			if (mc.McType == McType.McBinary)
			{
				return McBinaryHelper.WriteTag(mc, address.Substring(2), value);
			}
			if (mc.McType == McType.MCAscii)
			{
				return McAsciiHelper.WriteTag(mc, address.Substring(2), value);
			}
		}
		OperateResult<McAddressData> operateResult = mc.McAnalysisAddress(address, 0, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] send = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildWriteWordCoreCommand(operateResult.Content, value) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiWriteWordCoreCommand(operateResult.Content, value) : ((mc.McType == McType.McRBinary) ? MelsecMcRNet.BuildWriteWordCoreCommand(operateResult.Content, value) : null)));
		OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(send);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult<bool> ReadBool(IReadWriteMc mc, string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadBool(mc, address, 1));
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteMc mc, string address, ushort length, bool supportWordAdd = true)
	{
		if ((mc.McType == McType.McBinary && address.StartsWith("s=")) || address.StartsWith("S="))
		{
			return McBinaryHelper.ReadBoolTag(mc, address.Substring(2), length);
		}
		if (supportWordAdd && address.IndexOf('.') > 0)
		{
			return HslHelper.ReadBool(mc, address, length);
		}
		OperateResult<McAddressData> operateResult = mc.McAnalysisAddress(address, length, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		List<bool> list = new List<bool>();
		ushort num = 0;
		while (num < length)
		{
			ushort num2 = (ushort)Math.Min(length - num, GetReadBoolLength(mc.McType));
			operateResult.Content.Length = num2;
			byte[] send = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadMcCoreCommand(operateResult.Content, isBit: true) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadMcCoreCommand(operateResult.Content, isBit: true) : ((mc.McType == McType.McRBinary) ? MelsecMcRNet.BuildReadMcCoreCommand(operateResult.Content, isBit: true) : null)));
			OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(send);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult2);
			}
			list.AddRange((from m in mc.ExtractActualData(operateResult2.Content, isBit: true)
				select m == 1).Take(num2).ToArray());
			num += num2;
			operateResult.Content.AddressStart += num2;
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult Write(IReadWriteMc mc, string address, bool[] values, bool supportWordAdd = true)
	{
		if (supportWordAdd && mc.EnableWriteBitToWordRegister && address.Contains("."))
		{
			return ReadWriteNetHelper.WriteBoolWithWord(mc, address, values);
		}
		OperateResult<McAddressData> operateResult = mc.McAnalysisAddress(address, 0, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		byte[] send = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildWriteBitCoreCommand(operateResult.Content, values) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiWriteBitCoreCommand(operateResult.Content, values) : ((mc.McType == McType.McRBinary) ? MelsecMcRNet.BuildWriteBitCoreCommand(operateResult.Content, values) : null)));
		OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(send);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<byte[]>> ReadAsync(IReadWriteMc mc, string address, ushort length)
	{
		if ((mc.McType == McType.McBinary && address.StartsWith("s=")) || address.StartsWith("S="))
		{
			return await McBinaryHelper.ReadTagsAsync(mc, new string[1] { address.Substring(2) }, new ushort[1] { length });
		}
		if ((mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "ext=[0-9]+;", RegexOptions.IgnoreCase))
		{
			string extStr = Regex.Match(address, "ext=[0-9]+;").Value;
			ushort ext = ushort.Parse(Regex.Match(extStr, "[0-9]+").Value);
			return await ReadExtendAsync(mc, ext, address.Substring(extStr.Length), length);
		}
		if ((mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "mem=", RegexOptions.IgnoreCase))
		{
			return await ReadMemoryAsync(mc, address.Substring(4), length);
		}
		if ((mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "module=[0-9]+;", RegexOptions.IgnoreCase))
		{
			string moduleStr = Regex.Match(address, "module=[0-9]+;").Value;
			ushort module = ushort.Parse(Regex.Match(moduleStr, "[0-9]+").Value);
			return await ReadSmartModuleAsync(mc, module, address.Substring(moduleStr.Length), (ushort)(length * 2));
		}
		OperateResult<McAddressData> addressResult = mc.McAnalysisAddress(address, length, isBit: false);
		if (!addressResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(addressResult);
		}
		List<byte> bytesContent = new List<byte>();
		ushort alreadyFinished = 0;
		while (alreadyFinished < length)
		{
			ushort readLength = (ushort)Math.Min(length - alreadyFinished, GetReadWordLength(mc.McType));
			addressResult.Content.Length = readLength;
			byte[] command = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadMcCoreCommand(addressResult.Content, isBit: false) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadMcCoreCommand(addressResult.Content, isBit: false) : ((mc.McType == McType.McRBinary) ? MelsecMcRNet.BuildReadMcCoreCommand(addressResult.Content, isBit: false) : null)));
			OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(command);
			if (!read.IsSuccess)
			{
				return read;
			}
			bytesContent.AddRange(mc.ExtractActualData(read.Content, isBit: false));
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

	public static async Task<OperateResult> WriteAsync(IReadWriteMc mc, string address, byte[] value)
	{
		if ((mc.McType == McType.McBinary && address.StartsWith("s=")) || address.StartsWith("S="))
		{
			return await McBinaryHelper.WriteTagAsync(mc, address.Substring(2), value);
		}
		OperateResult<McAddressData> addressResult = mc.McAnalysisAddress(address, 0, isBit: false);
		if (!addressResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(addressResult);
		}
		byte[] coreResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildWriteWordCoreCommand(addressResult.Content, value) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiWriteWordCoreCommand(addressResult.Content, value) : ((mc.McType == McType.McRBinary) ? MelsecMcRNet.BuildWriteWordCoreCommand(addressResult.Content, value) : null)));
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
		if (!read.IsSuccess)
		{
			return read;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static async Task<OperateResult<bool[]>> ReadBoolAsync(IReadWriteMc mc, string address, ushort length, bool supportWordAdd = true)
	{
		if ((mc.McType == McType.McBinary && address.StartsWith("s=")) || address.StartsWith("S="))
		{
			return await McBinaryHelper.ReadBoolTagAsync(mc, address.Substring(2), length);
		}
		if (supportWordAdd && address.IndexOf('.') > 0)
		{
			return await HslHelper.ReadBoolAsync(mc, address, length);
		}
		OperateResult<McAddressData> addressResult = mc.McAnalysisAddress(address, length, isBit: true);
		if (!addressResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(addressResult);
		}
		List<bool> boolContent = new List<bool>();
		ushort alreadyFinished = 0;
		while (alreadyFinished < length)
		{
			ushort readLength = (ushort)Math.Min(length - alreadyFinished, GetReadBoolLength(mc.McType));
			addressResult.Content.Length = readLength;
			byte[] coreResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadMcCoreCommand(addressResult.Content, isBit: true) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadMcCoreCommand(addressResult.Content, isBit: true) : ((mc.McType == McType.McRBinary) ? MelsecMcRNet.BuildReadMcCoreCommand(addressResult.Content, isBit: true) : null)));
			OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			boolContent.AddRange((from m in mc.ExtractActualData(read.Content, isBit: true)
				select m == 1).Take(readLength).ToArray());
			alreadyFinished += readLength;
			addressResult.Content.AddressStart += readLength;
		}
		return OperateResult.CreateSuccessResult(boolContent.ToArray());
	}

	public static async Task<OperateResult> WriteAsync(IReadWriteMc mc, string address, bool[] values, bool supportWordAdd = true)
	{
		if (supportWordAdd && mc.EnableWriteBitToWordRegister && address.Contains("."))
		{
			return await ReadWriteNetHelper.WriteBoolWithWordAsync(mc, address, values).ConfigureAwait(continueOnCapturedContext: false);
		}
		OperateResult<McAddressData> addressResult = mc.McAnalysisAddress(address, 0, isBit: true);
		if (!addressResult.IsSuccess)
		{
			return addressResult;
		}
		byte[] coreResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildWriteBitCoreCommand(addressResult.Content, values) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiWriteBitCoreCommand(addressResult.Content, values) : ((mc.McType == McType.McRBinary) ? MelsecMcRNet.BuildWriteBitCoreCommand(addressResult.Content, values) : null)));
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
		if (!read.IsSuccess)
		{
			return read;
		}
		return OperateResult.CreateSuccessResult();
	}

	public static OperateResult<byte[]> ReadRandom(IReadWriteMc mc, string[] address)
	{
		McAddressData[] array = new McAddressData[address.Length];
		for (int i = 0; i < address.Length; i++)
		{
			OperateResult<McAddressData> operateResult = McAddressData.ParseMelsecFrom(address[i], 1, isBit: false);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			array[i] = operateResult.Content;
		}
		byte[] send = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadRandomWordCommand(array) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadRandomWordCommand(array) : null));
		OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(send);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(operateResult2.Content, isBit: false));
	}

	public static OperateResult<byte[]> ReadRandom(IReadWriteMc mc, string[] address, ushort[] length)
	{
		if (length.Length != address.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		McAddressData[] array = new McAddressData[address.Length];
		for (int i = 0; i < address.Length; i++)
		{
			OperateResult<McAddressData> operateResult = McAddressData.ParseMelsecFrom(address[i], length[i], isBit: false);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			array[i] = operateResult.Content;
		}
		byte[] send = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadRandomCommand(array) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadRandomCommand(array) : null));
		OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(send);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(operateResult2.Content, isBit: false));
	}

	public static OperateResult<short[]> ReadRandomInt16(IReadWriteMc mc, string[] address)
	{
		OperateResult<byte[]> operateResult = ReadRandom(mc, address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<short[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(mc.ByteTransform.TransInt16(operateResult.Content, 0, address.Length));
	}

	public static OperateResult<ushort[]> ReadRandomUInt16(IReadWriteMc mc, string[] address)
	{
		OperateResult<byte[]> operateResult = ReadRandom(mc, address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(mc.ByteTransform.TransUInt16(operateResult.Content, 0, address.Length));
	}

	public static async Task<OperateResult<byte[]>> ReadRandomAsync(IReadWriteMc mc, string[] address)
	{
		McAddressData[] mcAddressDatas = new McAddressData[address.Length];
		for (int i = 0; i < address.Length; i++)
		{
			OperateResult<McAddressData> addressResult = McAddressData.ParseMelsecFrom(address[i], 1, isBit: false);
			if (!addressResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(addressResult);
			}
			mcAddressDatas[i] = addressResult.Content;
		}
		byte[] coreResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadRandomWordCommand(mcAddressDatas) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadRandomWordCommand(mcAddressDatas) : null));
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(read.Content, isBit: false));
	}

	public static async Task<OperateResult<byte[]>> ReadRandomAsync(IReadWriteMc mc, string[] address, ushort[] length)
	{
		if (length.Length != address.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		McAddressData[] mcAddressDatas = new McAddressData[address.Length];
		for (int i = 0; i < address.Length; i++)
		{
			OperateResult<McAddressData> addressResult = McAddressData.ParseMelsecFrom(address[i], length[i], isBit: false);
			if (!addressResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(addressResult);
			}
			mcAddressDatas[i] = addressResult.Content;
		}
		byte[] coreResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadRandomCommand(mcAddressDatas) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadRandomCommand(mcAddressDatas) : null));
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(read.Content, isBit: false));
	}

	public static async Task<OperateResult<short[]>> ReadRandomInt16Async(IReadWriteMc mc, string[] address)
	{
		OperateResult<byte[]> read = await ReadRandomAsync(mc, address);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<short[]>(read);
		}
		return OperateResult.CreateSuccessResult(mc.ByteTransform.TransInt16(read.Content, 0, address.Length));
	}

	public static async Task<OperateResult<ushort[]>> ReadRandomUInt16Async(IReadWriteMc mc, string[] address)
	{
		OperateResult<byte[]> read = await ReadRandomAsync(mc, address);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort[]>(read);
		}
		return OperateResult.CreateSuccessResult(mc.ByteTransform.TransUInt16(read.Content, 0, address.Length));
	}

	public static OperateResult<byte[]> ReadMemory(IReadWriteMc mc, string address, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> operateResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadMemoryCommand(address, length) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadMemoryCommand(address, length) : null));
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(operateResult2.Content, isBit: false));
	}

	public static async Task<OperateResult<byte[]>> ReadMemoryAsync(IReadWriteMc mc, string address, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> coreResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadMemoryCommand(address, length) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadMemoryCommand(address, length) : null));
		if (!coreResult.IsSuccess)
		{
			return coreResult;
		}
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(read.Content, isBit: false));
	}

	public static OperateResult<byte[]> ReadSmartModule(IReadWriteMc mc, ushort module, string address, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> operateResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadSmartModule(module, address, length) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadSmartModule(module, address, length) : null));
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(operateResult2.Content, isBit: false));
	}

	public static async Task<OperateResult<byte[]>> ReadSmartModuleAsync(IReadWriteMc mc, ushort module, string address, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[]> coreResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadSmartModule(module, address, length) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadSmartModule(module, address, length) : null));
		if (!coreResult.IsSuccess)
		{
			return coreResult;
		}
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(read.Content, isBit: false));
	}

	public static OperateResult<byte[]> ReadExtend(IReadWriteMc mc, ushort extend, string address, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<McAddressData> operateResult = mc.McAnalysisAddress(address, length, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] send = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadMcCoreExtendCommand(operateResult.Content, extend, isBit: false) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadMcCoreExtendCommand(operateResult.Content, extend, isBit: false) : null));
		OperateResult<byte[]> operateResult2 = mc.ReadFromCoreServer(send);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(operateResult2.Content, isBit: false));
	}

	public static async Task<OperateResult<byte[]>> ReadExtendAsync(IReadWriteMc mc, ushort extend, string address, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<McAddressData> addressResult = mc.McAnalysisAddress(address, length, isBit: false);
		if (!addressResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(addressResult);
		}
		byte[] coreResult = ((mc.McType == McType.McBinary) ? McBinaryHelper.BuildReadMcCoreExtendCommand(addressResult.Content, extend, isBit: false) : ((mc.McType == McType.MCAscii) ? McAsciiHelper.BuildAsciiReadMcCoreExtendCommand(addressResult.Content, extend, isBit: false) : null));
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(mc.ExtractActualData(read.Content, isBit: false));
	}

	public static OperateResult RemoteRun(IReadWriteMc mc)
	{
		return (mc.McType == McType.McBinary) ? mc.ReadFromCoreServer(new byte[8] { 1, 16, 0, 0, 1, 0, 0, 0 }) : ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("1001000000010000")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
	}

	public static OperateResult RemoteStop(IReadWriteMc mc)
	{
		return (mc.McType == McType.McBinary) ? mc.ReadFromCoreServer(new byte[6] { 2, 16, 0, 0, 1, 0 }) : ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("100200000001")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
	}

	public static OperateResult RemoteReset(IReadWriteMc mc)
	{
		return (mc.McType == McType.McBinary) ? mc.ReadFromCoreServer(new byte[6] { 6, 16, 0, 0, 1, 0 }) : ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("100600000001")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
	}

	public static OperateResult<string> ReadPlcType(IReadWriteMc mc)
	{
		OperateResult<byte[]> operateResult = ((mc.McType == McType.McBinary) ? mc.ReadFromCoreServer(new byte[4] { 1, 1, 0, 0 }) : ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("01010000")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction)));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(operateResult.Content, 0, 16).TrimEnd());
	}

	public static OperateResult ErrorStateReset(IReadWriteMc mc)
	{
		return (mc.McType == McType.McBinary) ? mc.ReadFromCoreServer(new byte[4] { 23, 22, 0, 0 }) : ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("16170000")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
	}

	public static async Task<OperateResult> RemoteRunAsync(IReadWriteMc mc)
	{
		OperateResult<byte[]> result;
		if (mc.McType == McType.McBinary)
		{
			result = await mc.ReadFromCoreServerAsync(new byte[8] { 1, 16, 0, 0, 1, 0, 0, 0 });
		}
		else
		{
			OperateResult<byte[]> operateResult = ((mc.McType == McType.MCAscii) ? (await mc.ReadFromCoreServerAsync(Encoding.ASCII.GetBytes("1001000000010000"))) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
			OperateResult<byte[]> operateResult2 = operateResult;
			result = operateResult2;
		}
		return result;
	}

	public static async Task<OperateResult> RemoteStopAsync(IReadWriteMc mc)
	{
		OperateResult<byte[]> result;
		if (mc.McType == McType.McBinary)
		{
			result = await mc.ReadFromCoreServerAsync(new byte[6] { 2, 16, 0, 0, 1, 0 });
		}
		else
		{
			OperateResult<byte[]> operateResult = ((mc.McType == McType.MCAscii) ? (await mc.ReadFromCoreServerAsync(Encoding.ASCII.GetBytes("100200000001"))) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
			OperateResult<byte[]> operateResult2 = operateResult;
			result = operateResult2;
		}
		return result;
	}

	public static async Task<OperateResult> RemoteResetAsync(IReadWriteMc mc)
	{
		OperateResult<byte[]> result;
		if (mc.McType == McType.McBinary)
		{
			result = await mc.ReadFromCoreServerAsync(new byte[6] { 6, 16, 0, 0, 1, 0 });
		}
		else
		{
			OperateResult<byte[]> operateResult = ((mc.McType == McType.MCAscii) ? (await mc.ReadFromCoreServerAsync(Encoding.ASCII.GetBytes("100600000001"))) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
			OperateResult<byte[]> operateResult2 = operateResult;
			result = operateResult2;
		}
		return result;
	}

	public static async Task<OperateResult<string>> ReadPlcTypeAsync(IReadWriteMc mc)
	{
		OperateResult<byte[]> operateResult;
		if (mc.McType == McType.McBinary)
		{
			operateResult = await mc.ReadFromCoreServerAsync(new byte[4] { 1, 1, 0, 0 });
		}
		else
		{
			OperateResult<byte[]> operateResult2 = ((mc.McType == McType.MCAscii) ? (await mc.ReadFromCoreServerAsync(Encoding.ASCII.GetBytes("01010000"))) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
			OperateResult<byte[]> operateResult3 = operateResult2;
			operateResult = operateResult3;
		}
		OperateResult<byte[]> read = operateResult;
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(read.Content, 0, 16).TrimEnd());
	}

	public static async Task<OperateResult> ErrorStateResetAsync(IReadWriteMc mc)
	{
		OperateResult<byte[]> result;
		if (mc.McType == McType.McBinary)
		{
			result = await mc.ReadFromCoreServerAsync(new byte[4] { 23, 22, 0, 0 });
		}
		else
		{
			OperateResult<byte[]> operateResult = ((mc.McType == McType.MCAscii) ? (await mc.ReadFromCoreServerAsync(Encoding.ASCII.GetBytes("16170000"))) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
			OperateResult<byte[]> operateResult2 = operateResult;
			result = operateResult2;
		}
		return result;
	}
}
