using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Profinet.Omron;

namespace HslCommunication.Profinet.AllenBradley;

public class AllenBradleyConnectedCipNet : OmronConnectedCipNet
{
	public AllenBradleyConnectedCipNet()
	{
	}

	public AllenBradleyConnectedCipNet(string ipAddress, int port = 44818)
		: base(ipAddress, port)
	{
	}

	protected override int GetMaxTransferBytes()
	{
		return 1980;
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
			if (operateResult.Content.Length >= 6)
			{
				int count = base.ByteTransform.TransInt32(operateResult.Content, 2);
				return OperateResult.CreateSuccessResult(encoding.GetString(operateResult.Content, 6, count));
			}
			return OperateResult.CreateSuccessResult(encoding.GetString(operateResult.Content));
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(ex.Message + " Source: " + operateResult.Content.ToHexString(' '));
		}
	}

	public override OperateResult Write(string address, string value, Encoding encoding)
	{
		if (string.IsNullOrEmpty(value))
		{
			value = string.Empty;
		}
		byte[] bytes = encoding.GetBytes(value);
		OperateResult operateResult = Write(address + ".LEN", bytes.Length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		byte[] value2 = SoftBasic.ArrayExpandToLengthEven(bytes);
		return WriteTag(address + ".DATA[0]", 194, value2, bytes.Length);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		OperateResult<byte[]> read = await ReadAsync(address, length);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		if (read.Content.Length >= 6)
		{
			return OperateResult.CreateSuccessResult(encoding.GetString(count: base.ByteTransform.TransInt32(read.Content, 2), bytes: read.Content, index: 6));
		}
		return OperateResult.CreateSuccessResult(encoding.GetString(read.Content));
	}

	public override async Task<OperateResult> WriteAsync(string address, string value, Encoding encoding)
	{
		if (string.IsNullOrEmpty(value))
		{
			value = string.Empty;
		}
		byte[] data = encoding.GetBytes(value);
		OperateResult write = await WriteAsync(address + ".LEN", data.Length);
		if (!write.IsSuccess)
		{
			return write;
		}
		return await WriteTagAsync(value: SoftBasic.ArrayExpandToLengthEven(data), address: address + ".DATA[0]", typeCode: 194, length: data.Length);
	}

	public override string ToString()
	{
		return $"AllenBradleyConnectedCipNet[{IpAddress}:{Port}]";
	}
}
