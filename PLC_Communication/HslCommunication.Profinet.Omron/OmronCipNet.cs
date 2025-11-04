using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Net;
using HslCommunication.Profinet.AllenBradley;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Omron;

public class OmronCipNet : AllenBradleyNet
{
	public OmronCipNet()
	{
	}

	public OmronCipNet(string ipAddress, int port = 44818)
		: base(ipAddress, port)
	{
	}

	private bool CheckAddressArrayTag(string address)
	{
		return Regex.IsMatch(address, "\\[[0-9]+\\]$");
	}

	protected override bool GetBoolWritePadding()
	{
		return true;
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		if (length > 1)
		{
			if (CheckAddressArrayTag(address))
			{
				return Read(new string[1] { address }, new ushort[1] { length });
			}
			return Read(new string[1] { address }, new ushort[1] { 1 });
		}
		return Read(new string[1] { address }, new ushort[1] { length });
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		if (address.StartsWith("i="))
		{
			return base.ReadBool(address, length);
		}
		OperateResult<byte[]> operateResult = Read(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		if (CheckAddressArrayTag(address))
		{
			return OperateResult.CreateSuccessResult(operateResult.Content.Select((byte m) => m != 0).Take(length).ToArray());
		}
		return OperateResult.CreateSuccessResult(SoftBasic.ByteToBoolArray(operateResult.Content, length));
	}

	public override OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		OperateResult<byte[]> operateResult = Read(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		try
		{
			int count = base.ByteTransform.TransUInt16(operateResult.Content, 0);
			return OperateResult.CreateSuccessResult(encoding.GetString(operateResult.Content, 2, count));
		}
		catch (Exception ex)
		{
			return new OperateResult<string>("Parse failed: " + ex.Message + Environment.NewLine + "Source: " + operateResult.Content.ToHexString(' '));
		}
	}

	protected override int GetWriteValueLength(string address, int length)
	{
		return (!CheckAddressArrayTag(address)) ? 1 : length;
	}

	public override OperateResult<T> ReadStruct<T>(string address, ushort length)
	{
		return ReadWriteNetHelper.ReadStruct<T>(this, address, length, base.ByteTransform, 2);
	}

	public override OperateResult Write(string address, string value, Encoding encoding)
	{
		if (string.IsNullOrEmpty(value))
		{
			value = string.Empty;
		}
		byte[] array = SoftBasic.SpliceArray<byte>(new byte[2], SoftBasic.ArrayExpandToLengthEven(encoding.GetBytes(value)));
		array[0] = BitConverter.GetBytes(array.Length - 2)[0];
		array[1] = BitConverter.GetBytes(array.Length - 2)[1];
		return WriteTag(address, 208, array);
	}

	[HslMqttApi("WriteByte", "")]
	public override OperateResult Write(string address, byte value)
	{
		return WriteTag(address, 209, new byte[1] { value });
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		if (length > 1)
		{
			if (CheckAddressArrayTag(address))
			{
				return await ReadAsync(new string[1] { address }, new ushort[1] { length });
			}
			return await ReadAsync(new string[1] { address }, new ushort[1] { 1 });
		}
		return await ReadAsync(new string[1] { address }, new ushort[1] { length });
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		if (address.StartsWith("i="))
		{
			return await base.ReadBoolAsync(address, length);
		}
		OperateResult<byte[]> read = await ReadAsync(address, length);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		if (CheckAddressArrayTag(address))
		{
			return OperateResult.CreateSuccessResult(read.Content.Select((byte m) => m != 0).Take(length).ToArray());
		}
		return OperateResult.CreateSuccessResult(SoftBasic.ByteToBoolArray(read.Content, length));
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		OperateResult<byte[]> read = await ReadAsync(address, length);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		try
		{
			return OperateResult.CreateSuccessResult(encoding.GetString(count: base.ByteTransform.TransUInt16(read.Content, 0), bytes: read.Content, index: 2));
		}
		catch (Exception ex)
		{
			return new OperateResult<string>("Parse failed: " + ex.Message + Environment.NewLine + "Source: " + read.Content.ToHexString(' '));
		}
	}

	public override async Task<OperateResult> WriteAsync(string address, string value, Encoding encoding)
	{
		if (string.IsNullOrEmpty(value))
		{
			value = string.Empty;
		}
		byte[] data = SoftBasic.SpliceArray<byte>(new byte[2], SoftBasic.ArrayExpandToLengthEven(encoding.GetBytes(value)));
		data[0] = BitConverter.GetBytes(data.Length - 2)[0];
		data[1] = BitConverter.GetBytes(data.Length - 2)[1];
		return await WriteTagAsync(address, 208, data);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte value)
	{
		return await WriteTagAsync(address, 209, new byte[1] { value });
	}

	public override string ToString()
	{
		return $"OmronCipNet[{IpAddress}:{Port}]";
	}
}
