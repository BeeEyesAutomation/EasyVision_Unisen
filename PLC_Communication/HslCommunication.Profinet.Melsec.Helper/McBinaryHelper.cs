using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Address;

namespace HslCommunication.Profinet.Melsec.Helper;

public class McBinaryHelper
{
	public static byte[] PackMcCommand(IReadWriteMc mc, byte[] mcCore)
	{
		byte[] array = new byte[11 + mcCore.Length];
		array[0] = 80;
		array[1] = 0;
		array[2] = mc.NetworkNumber;
		array[3] = mc.PLCNumber;
		array[4] = BitConverter.GetBytes(mc.TargetIOStation)[0];
		array[5] = BitConverter.GetBytes(mc.TargetIOStation)[1];
		array[6] = mc.NetworkStationNumber;
		array[7] = (byte)((array.Length - 9) % 256);
		array[8] = (byte)((array.Length - 9) / 256);
		array[9] = 10;
		array[10] = 0;
		mcCore.CopyTo(array, 11);
		return array;
	}

	public static OperateResult CheckResponseContentHelper(byte[] content)
	{
		if (content == null || content.Length < 11)
		{
			return new OperateResult(StringResources.Language.ReceiveDataLengthTooShort + "11, Content: " + content.ToHexString(' '));
		}
		ushort num = BitConverter.ToUInt16(content, 9);
		if (num != 0)
		{
			return new OperateResult<byte[]>(num, MelsecHelper.GetErrorDescription(num));
		}
		return OperateResult.CreateSuccessResult();
	}

	public static byte[] BuildReadMcCoreCommand(McAddressData addressData, bool isBit)
	{
		return new byte[10]
		{
			1,
			4,
			(byte)(isBit ? 1u : 0u),
			0,
			BitConverter.GetBytes(addressData.AddressStart)[0],
			BitConverter.GetBytes(addressData.AddressStart)[1],
			BitConverter.GetBytes(addressData.AddressStart)[2],
			(byte)addressData.McDataType.DataCode,
			(byte)(addressData.Length % 256),
			(byte)(addressData.Length / 256)
		};
	}

	public static byte[] BuildWriteWordCoreCommand(McAddressData addressData, byte[] value)
	{
		if (value == null)
		{
			value = new byte[0];
		}
		byte[] array = new byte[10 + value.Length];
		array[0] = 1;
		array[1] = 20;
		array[2] = 0;
		array[3] = 0;
		array[4] = BitConverter.GetBytes(addressData.AddressStart)[0];
		array[5] = BitConverter.GetBytes(addressData.AddressStart)[1];
		array[6] = BitConverter.GetBytes(addressData.AddressStart)[2];
		array[7] = (byte)addressData.McDataType.DataCode;
		array[8] = (byte)(value.Length / 2 % 256);
		array[9] = (byte)(value.Length / 2 / 256);
		value.CopyTo(array, 10);
		return array;
	}

	public static byte[] BuildWriteBitCoreCommand(McAddressData addressData, bool[] value)
	{
		if (value == null)
		{
			value = new bool[0];
		}
		byte[] array = MelsecHelper.TransBoolArrayToByteData(value);
		byte[] array2 = new byte[10 + array.Length];
		array2[0] = 1;
		array2[1] = 20;
		array2[2] = 1;
		array2[3] = 0;
		array2[4] = BitConverter.GetBytes(addressData.AddressStart)[0];
		array2[5] = BitConverter.GetBytes(addressData.AddressStart)[1];
		array2[6] = BitConverter.GetBytes(addressData.AddressStart)[2];
		array2[7] = (byte)addressData.McDataType.DataCode;
		array2[8] = (byte)(value.Length % 256);
		array2[9] = (byte)(value.Length / 256);
		array.CopyTo(array2, 10);
		return array2;
	}

	public static byte[] BuildReadMcCoreExtendCommand(McAddressData addressData, ushort extend, bool isBit)
	{
		return new byte[17]
		{
			1,
			4,
			(byte)(isBit ? 129u : 128u),
			0,
			0,
			0,
			BitConverter.GetBytes(addressData.AddressStart)[0],
			BitConverter.GetBytes(addressData.AddressStart)[1],
			BitConverter.GetBytes(addressData.AddressStart)[2],
			(byte)addressData.McDataType.DataCode,
			0,
			0,
			BitConverter.GetBytes(extend)[0],
			BitConverter.GetBytes(extend)[1],
			249,
			(byte)(addressData.Length % 256),
			(byte)(addressData.Length / 256)
		};
	}

	public static byte[] BuildReadRandomWordCommand(McAddressData[] address)
	{
		byte[] array = new byte[6 + address.Length * 4];
		array[0] = 3;
		array[1] = 4;
		array[2] = 0;
		array[3] = 0;
		array[4] = (byte)address.Length;
		array[5] = 0;
		for (int i = 0; i < address.Length; i++)
		{
			array[i * 4 + 6] = BitConverter.GetBytes(address[i].AddressStart)[0];
			array[i * 4 + 7] = BitConverter.GetBytes(address[i].AddressStart)[1];
			array[i * 4 + 8] = BitConverter.GetBytes(address[i].AddressStart)[2];
			array[i * 4 + 9] = (byte)address[i].McDataType.DataCode;
		}
		return array;
	}

	public static byte[] BuildReadRandomCommand(McAddressData[] address)
	{
		byte[] array = new byte[6 + address.Length * 6];
		array[0] = 6;
		array[1] = 4;
		array[2] = 0;
		array[3] = 0;
		array[4] = (byte)address.Length;
		array[5] = 0;
		for (int i = 0; i < address.Length; i++)
		{
			array[i * 6 + 6] = BitConverter.GetBytes(address[i].AddressStart)[0];
			array[i * 6 + 7] = BitConverter.GetBytes(address[i].AddressStart)[1];
			array[i * 6 + 8] = BitConverter.GetBytes(address[i].AddressStart)[2];
			array[i * 6 + 9] = (byte)address[i].McDataType.DataCode;
			array[i * 6 + 10] = (byte)(address[i].Length % 256);
			array[i * 6 + 11] = (byte)(address[i].Length / 256);
		}
		return array;
	}

	public static byte[] BuildReadTag(string[] tags, ushort[] lengths, bool isBit = false)
	{
		if (tags.Length != lengths.Length)
		{
			throw new Exception(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(26);
		memoryStream.WriteByte(4);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(BitConverter.GetBytes(tags.Length)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(tags.Length)[1]);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		for (int i = 0; i < tags.Length; i++)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(tags[i]);
			memoryStream.WriteByte(BitConverter.GetBytes(bytes.Length / 2)[0]);
			memoryStream.WriteByte(BitConverter.GetBytes(bytes.Length / 2)[1]);
			memoryStream.Write(bytes, 0, bytes.Length);
			if (isBit)
			{
				memoryStream.WriteByte(0);
			}
			else
			{
				memoryStream.WriteByte(1);
			}
			memoryStream.WriteByte(0);
			memoryStream.WriteByte(BitConverter.GetBytes(lengths[i] * 2)[0]);
			memoryStream.WriteByte(BitConverter.GetBytes(lengths[i] * 2)[1]);
		}
		byte[] result = memoryStream.ToArray();
		memoryStream.Dispose();
		return result;
	}

	public static byte[] BuildWriteTag(string tag, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(26);
		memoryStream.WriteByte(20);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(BitConverter.GetBytes(1)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(1)[1]);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		byte[] bytes = Encoding.Unicode.GetBytes(tag);
		memoryStream.WriteByte(BitConverter.GetBytes(bytes.Length / 2)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(bytes.Length / 2)[1]);
		memoryStream.Write(bytes, 0, bytes.Length);
		memoryStream.WriteByte(1);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(BitConverter.GetBytes(data.Length)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(data.Length)[1]);
		memoryStream.Write(data);
		return memoryStream.ToArray();
	}

	public static OperateResult<byte[]> BuildReadMemoryCommand(string address, ushort length)
	{
		try
		{
			uint value = uint.Parse(address);
			return OperateResult.CreateSuccessResult(new byte[10]
			{
				19,
				6,
				0,
				0,
				BitConverter.GetBytes(value)[0],
				BitConverter.GetBytes(value)[1],
				BitConverter.GetBytes(value)[2],
				BitConverter.GetBytes(value)[3],
				(byte)(length % 256),
				(byte)(length / 256)
			});
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildReadSmartModule(ushort module, string address, ushort length)
	{
		try
		{
			uint value = uint.Parse(address);
			return OperateResult.CreateSuccessResult(new byte[12]
			{
				1,
				6,
				0,
				0,
				BitConverter.GetBytes(value)[0],
				BitConverter.GetBytes(value)[1],
				BitConverter.GetBytes(value)[2],
				BitConverter.GetBytes(value)[3],
				(byte)(length % 256),
				(byte)(length / 256),
				BitConverter.GetBytes(module)[0],
				BitConverter.GetBytes(module)[1]
			});
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> ExtraTagData(byte[] content)
	{
		try
		{
			int num = BitConverter.ToUInt16(content, 0);
			int num2 = 2;
			List<byte> list = new List<byte>(20);
			for (int i = 0; i < num; i++)
			{
				int num3 = BitConverter.ToUInt16(content, num2 + 2);
				list.AddRange(SoftBasic.ArraySelectMiddle(content, num2 + 4, num3));
				num2 += 4 + num3;
			}
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message + " Source:" + SoftBasic.ByteToHexString(content, ' '));
		}
	}

	public static byte[] ExtractActualDataHelper(byte[] response, bool isBit)
	{
		if (response == null || response.Length == 0)
		{
			return response;
		}
		if (isBit)
		{
			byte[] array = new byte[response.Length * 2];
			for (int i = 0; i < response.Length; i++)
			{
				if ((response[i] & 0x10) == 16)
				{
					array[i * 2] = 1;
				}
				if ((response[i] & 1) == 1)
				{
					array[i * 2 + 1] = 1;
				}
			}
			return array;
		}
		return response;
	}

	public static OperateResult<byte[]> ReadTags(IReadWriteMc mc, string[] tags, ushort[] length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] send = BuildReadTag(tags, length);
		OperateResult<byte[]> operateResult = mc.ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return ExtraTagData(mc.ExtractActualData(operateResult.Content, isBit: false));
	}

	public static async Task<OperateResult<byte[]>> ReadTagsAsync(IReadWriteMc mc, string[] tags, ushort[] length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] coreResult = BuildReadTag(tags, length);
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return ExtraTagData(mc.ExtractActualData(read.Content, isBit: false));
	}

	public static OperateResult<bool[]> ReadBoolTag(IReadWriteMc mc, string tag, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<bool[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] send = BuildReadTag(new string[1] { tag }, new ushort[1] { length }, isBit: true);
		OperateResult<byte[]> operateResult = mc.ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ExtraTagData(mc.ExtractActualData(operateResult.Content, isBit: false));
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.ToBoolArray().SelectBegin(length));
	}

	public static async Task<OperateResult<bool[]>> ReadBoolTagAsync(IReadWriteMc mc, string tag, ushort length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<bool[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] coreResult = BuildReadTag(new string[1] { tag }, new ushort[1] { length }, isBit: true);
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		OperateResult<byte[]> extra = ExtraTagData(mc.ExtractActualData(read.Content, isBit: false));
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(extra);
		}
		return OperateResult.CreateSuccessResult(extra.Content.ToBoolArray().SelectBegin(length));
	}

	public static OperateResult WriteTag(IReadWriteMc mc, string tag, byte[] data)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] send = BuildWriteTag(tag, data);
		return mc.ReadFromCoreServer(send);
	}

	public static async Task<OperateResult> WriteTagAsync(IReadWriteMc mc, string tag, byte[] data)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] coreResult = BuildWriteTag(tag, data);
		return await mc.ReadFromCoreServerAsync(coreResult);
	}
}
