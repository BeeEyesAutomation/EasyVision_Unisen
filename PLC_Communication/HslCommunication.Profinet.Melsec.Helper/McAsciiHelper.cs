using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Address;

namespace HslCommunication.Profinet.Melsec.Helper;

public class McAsciiHelper
{
	public static byte[] PackMcCommand(IReadWriteMc mc, byte[] mcCore)
	{
		byte[] array = SoftBasic.BuildAsciiBytesFrom(mc.TargetIOStation);
		byte[] array2 = new byte[22 + mcCore.Length];
		array2[0] = 53;
		array2[1] = 48;
		array2[2] = 48;
		array2[3] = 48;
		array2[4] = SoftBasic.BuildAsciiBytesFrom(mc.NetworkNumber)[0];
		array2[5] = SoftBasic.BuildAsciiBytesFrom(mc.NetworkNumber)[1];
		array2[6] = SoftBasic.BuildAsciiBytesFrom(mc.PLCNumber)[0];
		array2[7] = SoftBasic.BuildAsciiBytesFrom(mc.PLCNumber)[1];
		array2[8] = array[0];
		array2[9] = array[1];
		array2[10] = array[2];
		array2[11] = array[3];
		array2[12] = SoftBasic.BuildAsciiBytesFrom(mc.NetworkStationNumber)[0];
		array2[13] = SoftBasic.BuildAsciiBytesFrom(mc.NetworkStationNumber)[1];
		array2[14] = SoftBasic.BuildAsciiBytesFrom((ushort)(array2.Length - 18))[0];
		array2[15] = SoftBasic.BuildAsciiBytesFrom((ushort)(array2.Length - 18))[1];
		array2[16] = SoftBasic.BuildAsciiBytesFrom((ushort)(array2.Length - 18))[2];
		array2[17] = SoftBasic.BuildAsciiBytesFrom((ushort)(array2.Length - 18))[3];
		array2[18] = 48;
		array2[19] = 48;
		array2[20] = 49;
		array2[21] = 48;
		mcCore.CopyTo(array2, 22);
		return array2;
	}

	public static byte[] ExtractActualDataHelper(byte[] response, bool isBit)
	{
		if (isBit)
		{
			return response.Select((byte m) => (m != 48) ? ((byte)1) : ((byte)0)).ToArray();
		}
		return MelsecHelper.TransAsciiByteArrayToByteArray(response);
	}

	public static OperateResult CheckResponseContent(byte[] content)
	{
		if (content == null || content.Length < 22)
		{
			return new OperateResult(StringResources.Language.ReceiveDataLengthTooShort + "22, Content: " + SoftBasic.GetAsciiStringRender(content));
		}
		ushort num = Convert.ToUInt16(Encoding.ASCII.GetString(content, 18, 4), 16);
		if (num != 0)
		{
			return new OperateResult(num, MelsecHelper.GetErrorDescription(num));
		}
		return OperateResult.CreateSuccessResult();
	}

	public static byte[] BuildAsciiReadMcCoreCommand(McAddressData addressData, bool isBit)
	{
		byte[] obj = new byte[20]
		{
			48, 52, 48, 49, 48, 48, 48, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};
		obj[7] = (byte)(isBit ? 49u : 48u);
		obj[8] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[0];
		obj[9] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[1];
		obj[10] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[0];
		obj[11] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[1];
		obj[12] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[2];
		obj[13] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[3];
		obj[14] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[4];
		obj[15] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[5];
		obj[16] = SoftBasic.BuildAsciiBytesFrom(addressData.Length)[0];
		obj[17] = SoftBasic.BuildAsciiBytesFrom(addressData.Length)[1];
		obj[18] = SoftBasic.BuildAsciiBytesFrom(addressData.Length)[2];
		obj[19] = SoftBasic.BuildAsciiBytesFrom(addressData.Length)[3];
		return obj;
	}

	public static byte[] BuildAsciiReadMcCoreExtendCommand(McAddressData addressData, ushort extend, bool isBit)
	{
		byte[] obj = new byte[32]
		{
			48, 52, 48, 49, 48, 48, 56, 0, 48, 48,
			74, 0, 0, 0, 48, 48, 48, 0, 0, 0,
			0, 0, 0, 0, 0, 48, 48, 48, 0, 0,
			0, 0
		};
		obj[7] = (byte)(isBit ? 49u : 48u);
		obj[11] = SoftBasic.BuildAsciiBytesFrom(extend)[1];
		obj[12] = SoftBasic.BuildAsciiBytesFrom(extend)[2];
		obj[13] = SoftBasic.BuildAsciiBytesFrom(extend)[3];
		obj[17] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[0];
		obj[18] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[1];
		obj[19] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[0];
		obj[20] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[1];
		obj[21] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[2];
		obj[22] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[3];
		obj[23] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[4];
		obj[24] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[5];
		obj[28] = SoftBasic.BuildAsciiBytesFrom(addressData.Length)[0];
		obj[29] = SoftBasic.BuildAsciiBytesFrom(addressData.Length)[1];
		obj[30] = SoftBasic.BuildAsciiBytesFrom(addressData.Length)[2];
		obj[31] = SoftBasic.BuildAsciiBytesFrom(addressData.Length)[3];
		return obj;
	}

	public static byte[] BuildAsciiWriteWordCoreCommand(McAddressData addressData, byte[] value)
	{
		value = MelsecHelper.TransByteArrayToAsciiByteArray(value);
		byte[] array = new byte[20 + value.Length];
		array[0] = 49;
		array[1] = 52;
		array[2] = 48;
		array[3] = 49;
		array[4] = 48;
		array[5] = 48;
		array[6] = 48;
		array[7] = 48;
		array[8] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[0];
		array[9] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[1];
		array[10] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[0];
		array[11] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[1];
		array[12] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[2];
		array[13] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[3];
		array[14] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[4];
		array[15] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[5];
		array[16] = SoftBasic.BuildAsciiBytesFrom((ushort)(value.Length / 4))[0];
		array[17] = SoftBasic.BuildAsciiBytesFrom((ushort)(value.Length / 4))[1];
		array[18] = SoftBasic.BuildAsciiBytesFrom((ushort)(value.Length / 4))[2];
		array[19] = SoftBasic.BuildAsciiBytesFrom((ushort)(value.Length / 4))[3];
		value.CopyTo(array, 20);
		return array;
	}

	public static byte[] BuildAsciiWriteBitCoreCommand(McAddressData addressData, bool[] value)
	{
		if (value == null)
		{
			value = new bool[0];
		}
		byte[] array = value.Select((bool m) => (byte)(m ? 49u : 48u)).ToArray();
		byte[] array2 = new byte[20 + array.Length];
		array2[0] = 49;
		array2[1] = 52;
		array2[2] = 48;
		array2[3] = 49;
		array2[4] = 48;
		array2[5] = 48;
		array2[6] = 48;
		array2[7] = 49;
		array2[8] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[0];
		array2[9] = Encoding.ASCII.GetBytes(addressData.McDataType.AsciiCode)[1];
		array2[10] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[0];
		array2[11] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[1];
		array2[12] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[2];
		array2[13] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[3];
		array2[14] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[4];
		array2[15] = MelsecHelper.BuildBytesFromAddress(addressData.AddressStart, addressData.McDataType)[5];
		array2[16] = SoftBasic.BuildAsciiBytesFrom((ushort)value.Length)[0];
		array2[17] = SoftBasic.BuildAsciiBytesFrom((ushort)value.Length)[1];
		array2[18] = SoftBasic.BuildAsciiBytesFrom((ushort)value.Length)[2];
		array2[19] = SoftBasic.BuildAsciiBytesFrom((ushort)value.Length)[3];
		array.CopyTo(array2, 20);
		return array2;
	}

	public static byte[] BuildAsciiReadRandomWordCommand(McAddressData[] address)
	{
		byte[] array = new byte[12 + address.Length * 8];
		array[0] = 48;
		array[1] = 52;
		array[2] = 48;
		array[3] = 51;
		array[4] = 48;
		array[5] = 48;
		array[6] = 48;
		array[7] = 48;
		array[8] = SoftBasic.BuildAsciiBytesFrom((byte)address.Length)[0];
		array[9] = SoftBasic.BuildAsciiBytesFrom((byte)address.Length)[1];
		array[10] = 48;
		array[11] = 48;
		for (int i = 0; i < address.Length; i++)
		{
			array[i * 8 + 12] = Encoding.ASCII.GetBytes(address[i].McDataType.AsciiCode)[0];
			array[i * 8 + 13] = Encoding.ASCII.GetBytes(address[i].McDataType.AsciiCode)[1];
			array[i * 8 + 14] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[0];
			array[i * 8 + 15] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[1];
			array[i * 8 + 16] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[2];
			array[i * 8 + 17] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[3];
			array[i * 8 + 18] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[4];
			array[i * 8 + 19] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[5];
		}
		return array;
	}

	public static byte[] BuildAsciiReadRandomCommand(McAddressData[] address)
	{
		byte[] array = new byte[12 + address.Length * 12];
		array[0] = 48;
		array[1] = 52;
		array[2] = 48;
		array[3] = 54;
		array[4] = 48;
		array[5] = 48;
		array[6] = 48;
		array[7] = 48;
		array[8] = SoftBasic.BuildAsciiBytesFrom((byte)address.Length)[0];
		array[9] = SoftBasic.BuildAsciiBytesFrom((byte)address.Length)[1];
		array[10] = 48;
		array[11] = 48;
		for (int i = 0; i < address.Length; i++)
		{
			array[i * 12 + 12] = Encoding.ASCII.GetBytes(address[i].McDataType.AsciiCode)[0];
			array[i * 12 + 13] = Encoding.ASCII.GetBytes(address[i].McDataType.AsciiCode)[1];
			array[i * 12 + 14] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[0];
			array[i * 12 + 15] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[1];
			array[i * 12 + 16] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[2];
			array[i * 12 + 17] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[3];
			array[i * 12 + 18] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[4];
			array[i * 12 + 19] = MelsecHelper.BuildBytesFromAddress(address[i].AddressStart, address[i].McDataType)[5];
			array[i * 12 + 20] = SoftBasic.BuildAsciiBytesFrom(address[i].Length)[0];
			array[i * 12 + 21] = SoftBasic.BuildAsciiBytesFrom(address[i].Length)[1];
			array[i * 12 + 22] = SoftBasic.BuildAsciiBytesFrom(address[i].Length)[2];
			array[i * 12 + 23] = SoftBasic.BuildAsciiBytesFrom(address[i].Length)[3];
		}
		return array;
	}

	public static OperateResult<byte[]> BuildAsciiReadMemoryCommand(string address, ushort length)
	{
		try
		{
			uint value = uint.Parse(address);
			byte[] array = new byte[20]
			{
				48, 54, 49, 51, 48, 48, 48, 48, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0
			};
			SoftBasic.BuildAsciiBytesFrom(value).CopyTo(array, 8);
			SoftBasic.BuildAsciiBytesFrom(length).CopyTo(array, 16);
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildAsciiReadSmartModule(ushort module, string address, ushort length)
	{
		try
		{
			uint value = uint.Parse(address);
			byte[] array = new byte[24]
			{
				48, 54, 48, 49, 48, 48, 48, 48, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			SoftBasic.BuildAsciiBytesFrom(value).CopyTo(array, 8);
			SoftBasic.BuildAsciiBytesFrom(length).CopyTo(array, 16);
			SoftBasic.BuildAsciiBytesFrom(module).CopyTo(array, 20);
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static byte[] BuildAsciiReadTag(string[] tags, ushort[] lengths, bool isBit = false)
	{
		if (tags.Length != lengths.Length)
		{
			throw new Exception(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(Encoding.ASCII.GetBytes("041A"));
		memoryStream.Write(Encoding.ASCII.GetBytes("0000"));
		memoryStream.Write(Encoding.ASCII.GetBytes(tags.Length.ToString("X4")));
		memoryStream.Write(Encoding.ASCII.GetBytes("0000"));
		for (int i = 0; i < tags.Length; i++)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(tags[i]);
			memoryStream.Write(SoftBasic.BuildAsciiBytesFrom((ushort)(bytes.Length / 2)));
			for (int j = 0; j < bytes.Length; j++)
			{
				memoryStream.Write(SoftBasic.BuildAsciiBytesFrom(bytes[j]));
			}
			if (isBit)
			{
				memoryStream.Write(Encoding.ASCII.GetBytes("00"));
			}
			else
			{
				memoryStream.Write(Encoding.ASCII.GetBytes("01"));
			}
			memoryStream.Write(Encoding.ASCII.GetBytes("00"));
			memoryStream.Write(SoftBasic.BuildAsciiBytesFrom((ushort)(lengths[i] * 2)));
		}
		return memoryStream.ToArray();
	}

	public static byte[] BuildAsciiWriteTag(string tag, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(Encoding.ASCII.GetBytes("141A"));
		memoryStream.Write(Encoding.ASCII.GetBytes("0000"));
		memoryStream.Write(Encoding.ASCII.GetBytes("0001"));
		memoryStream.Write(Encoding.ASCII.GetBytes("0000"));
		byte[] bytes = Encoding.Unicode.GetBytes(tag);
		memoryStream.Write(SoftBasic.BuildAsciiBytesFrom((ushort)(bytes.Length / 2)));
		for (int i = 0; i < bytes.Length; i++)
		{
			memoryStream.Write(SoftBasic.BuildAsciiBytesFrom(bytes[i]));
		}
		memoryStream.Write(Encoding.ASCII.GetBytes("01"));
		memoryStream.Write(Encoding.ASCII.GetBytes("00"));
		memoryStream.Write(SoftBasic.BuildAsciiBytesFrom((ushort)data.Length));
		memoryStream.Write(Encoding.ASCII.GetBytes(SoftBasic.BytesReverseByWord(data).ToHexString()));
		return memoryStream.ToArray();
	}

	public static OperateResult<byte[]> ExtraTagData(byte[] content)
	{
		try
		{
			int num = Convert.ToInt32(Encoding.ASCII.GetString(content, 0, 4), 16);
			int num2 = 4;
			List<byte> list = new List<byte>(20);
			for (int i = 0; i < num; i++)
			{
				int num3 = Convert.ToInt32(Encoding.ASCII.GetString(content, num2 + 4, 4), 16);
				list.AddRange(SoftBasic.BytesReverseByWord(Encoding.ASCII.GetString(content, num2 + 8, num3 * 2).ToHexBytes()));
				num2 += 8 + num3 * 2;
			}
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message + " Source:" + SoftBasic.ByteToHexString(content, ' '));
		}
	}

	public static OperateResult<byte[]> ReadTags(IReadWriteMc mc, string[] tags, ushort[] length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] send = BuildAsciiReadTag(tags, length);
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
		byte[] coreResult = BuildAsciiReadTag(tags, length);
		OperateResult<byte[]> read = await mc.ReadFromCoreServerAsync(coreResult);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return ExtraTagData(mc.ExtractActualData(read.Content, isBit: false));
	}

	public static OperateResult WriteTag(IReadWriteMc mc, string tag, byte[] data)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] send = BuildAsciiWriteTag(tag, data);
		return mc.ReadFromCoreServer(send);
	}

	public static async Task<OperateResult> WriteTagAsync(IReadWriteMc mc, string tag, byte[] data)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		byte[] coreResult = BuildAsciiWriteTag(tag, data);
		return await mc.ReadFromCoreServerAsync(coreResult);
	}
}
