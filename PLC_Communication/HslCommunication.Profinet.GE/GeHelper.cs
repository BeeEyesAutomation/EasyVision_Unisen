using System;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Address;

namespace HslCommunication.Profinet.GE;

public class GeHelper
{
	public static OperateResult<byte[]> BuildReadCoreCommand(long id, byte code, byte[] data)
	{
		byte[] obj = new byte[56]
		{
			2, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			6, 192, 0, 0, 0, 0, 16, 14, 0, 0,
			1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0
		};
		obj[2] = BitConverter.GetBytes(id)[0];
		obj[3] = BitConverter.GetBytes(id)[1];
		obj[42] = code;
		byte[] array = obj;
		data.CopyTo(array, 43);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildReadCommand(long id, GeSRTPAddress address)
	{
		if (address.DataCode == 10 || address.DataCode == 12 || address.DataCode == 8)
		{
			address.Length /= 2;
		}
		return BuildReadCoreCommand(id, 4, new byte[5]
		{
			address.DataCode,
			BitConverter.GetBytes(address.AddressStart)[0],
			BitConverter.GetBytes(address.AddressStart)[1],
			BitConverter.GetBytes(address.Length)[0],
			BitConverter.GetBytes(address.Length)[1]
		});
	}

	public static OperateResult<byte[]> BuildReadCommand(long id, string address, ushort length, bool isBit)
	{
		OperateResult<GeSRTPAddress> operateResult = GeSRTPAddress.ParseFrom(address, length, isBit);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return BuildReadCommand(id, operateResult.Content);
	}

	public static OperateResult<byte[]> BuildWriteCommand(long id, GeSRTPAddress address, byte[] value)
	{
		int num = address.Length;
		if (address.DataCode == 10 || address.DataCode == 12 || address.DataCode == 8)
		{
			num /= 2;
		}
		byte[] array = new byte[56 + value.Length];
		array[0] = 2;
		array[1] = 0;
		array[2] = BitConverter.GetBytes(id)[0];
		array[3] = BitConverter.GetBytes(id)[1];
		array[4] = BitConverter.GetBytes(value.Length)[0];
		array[5] = BitConverter.GetBytes(value.Length)[1];
		array[9] = 2;
		array[17] = 2;
		array[18] = 0;
		array[30] = 9;
		array[31] = 128;
		array[36] = 16;
		array[37] = 14;
		array[40] = 1;
		array[41] = 1;
		array[42] = 2;
		array[48] = 1;
		array[49] = 1;
		array[50] = 7;
		array[51] = address.DataCode;
		array[52] = BitConverter.GetBytes(address.AddressStart)[0];
		array[53] = BitConverter.GetBytes(address.AddressStart)[1];
		array[54] = BitConverter.GetBytes(num)[0];
		array[55] = BitConverter.GetBytes(num)[1];
		value.CopyTo(array, 56);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteCommand(long id, string address, byte[] value)
	{
		OperateResult<GeSRTPAddress> operateResult = GeSRTPAddress.ParseFrom(address, (ushort)value.Length, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return BuildWriteCommand(id, operateResult.Content, value);
	}

	public static OperateResult<byte[]> BuildWriteCommand(long id, string address, bool[] value)
	{
		OperateResult<GeSRTPAddress> operateResult = GeSRTPAddress.ParseFrom(address, (ushort)value.Length, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		bool[] array = new bool[operateResult.Content.AddressStart % 8 + value.Length];
		value.CopyTo(array, operateResult.Content.AddressStart % 8);
		return BuildWriteCommand(id, operateResult.Content, SoftBasic.BoolArrayToByte(array));
	}

	public static OperateResult<byte[]> ExtraResponseContent(byte[] content)
	{
		try
		{
			if (content[0] != 3)
			{
				return new OperateResult<byte[]>(content[0], StringResources.Language.UnknownError + " Source:" + content.ToHexString(' '));
			}
			if (content[31] == 212)
			{
				ushort num = BitConverter.ToUInt16(content, 42);
				if (num != 0)
				{
					return new OperateResult<byte[]>(num, StringResources.Language.UnknownError);
				}
				return OperateResult.CreateSuccessResult(content.SelectMiddle(44, 6));
			}
			if (content[31] == 148)
			{
				return OperateResult.CreateSuccessResult(content.RemoveBegin(56));
			}
			return new OperateResult<byte[]>("Extra Wrong:" + StringResources.Language.UnknownError + " Source:" + content.ToHexString(' '));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Extra Wrong:" + ex.Message + " Source:" + content.ToHexString(' '));
		}
	}

	public static OperateResult<DateTime> ExtraDateTime(byte[] content)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<DateTime>(StringResources.Language.InsufficientPrivileges);
		}
		try
		{
			return OperateResult.CreateSuccessResult(new DateTime(int.Parse(content[5].ToString("X2")) + 2000, int.Parse(content[4].ToString("X2")), int.Parse(content[3].ToString("X2")), int.Parse(content[2].ToString("X2")), int.Parse(content[1].ToString("X2")), int.Parse(content[0].ToString("X2"))));
		}
		catch (Exception ex)
		{
			return new OperateResult<DateTime>(ex.Message + " Source:" + content.ToHexString(' '));
		}
	}

	public static OperateResult<string> ExtraProgramName(byte[] content)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<string>(StringResources.Language.InsufficientPrivileges);
		}
		try
		{
			return OperateResult.CreateSuccessResult(Encoding.UTF8.GetString(content, 18, 16).Trim(default(char)));
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(ex.Message + " Source:" + content.ToHexString(' '));
		}
	}
}
