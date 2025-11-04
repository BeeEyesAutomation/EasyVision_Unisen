using System;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Siemens;

public class SiemensFetchWriteNet : DeviceTcpNet
{
	public SiemensFetchWriteNet()
	{
		base.WordLength = 2;
		base.ByteTransform = new ReverseBytesTransform();
	}

	public SiemensFetchWriteNet(string ipAddress, int port)
	{
		base.WordLength = 2;
		IpAddress = ipAddress;
		Port = port;
		base.ByteTransform = new ReverseBytesTransform();
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FetchWriteMessage();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte[]> operateResult = BuildReadCommand(address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = CheckResponseContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult3);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.ArrayRemoveBegin(operateResult2.Content, 16));
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteCommand(address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = CheckResponseContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult3);
		}
		return OperateResult.CreateSuccessResult();
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<byte[]> command = BuildReadCommand(address, length);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = CheckResponseContent(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.ArrayRemoveBegin(read.Content, 16));
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> command = BuildWriteCommand(address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> write = await ReadFromCoreServerAsync(command.Content);
		if (!write.IsSuccess)
		{
			return write;
		}
		OperateResult check = CheckResponseContent(write.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi("ReadByte", "")]
	public OperateResult<byte> ReadByte(string address)
	{
		return ByteTransformHelper.GetResultFromArray(Read(address, 1));
	}

	[HslMqttApi("WriteByte", "")]
	public OperateResult Write(string address, byte value)
	{
		return Write(address, new byte[1] { value });
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadAsync(address, 1));
	}

	public async Task<OperateResult> WriteAsync(string address, byte value)
	{
		return await WriteAsync(address, new byte[1] { value });
	}

	public override string ToString()
	{
		return $"SiemensFetchWriteNet[{IpAddress}:{Port}]";
	}

	private static int CalculateAddressStarted(string address)
	{
		if (address.IndexOf('.') < 0)
		{
			return Convert.ToInt32(address);
		}
		string[] array = address.Split('.');
		return Convert.ToInt32(array[0]);
	}

	private static OperateResult CheckResponseContent(byte[] content)
	{
		if (content == null || content.Length < 9)
		{
			return new OperateResult(StringResources.Language.ReceiveDataLengthTooShort + "9, Content: " + content.ToHexString(' '));
		}
		if (content[8] != 0)
		{
			return new OperateResult(content[8], StringResources.Language.SiemensWriteError + content[8]);
		}
		return OperateResult.CreateSuccessResult();
	}

	private static OperateResult<byte, int, ushort> AnalysisAddress(string address)
	{
		OperateResult<byte, int, ushort> operateResult = new OperateResult<byte, int, ushort>();
		try
		{
			operateResult.Content3 = 0;
			if (address[0] == 'I')
			{
				operateResult.Content1 = 3;
				operateResult.Content2 = CalculateAddressStarted(address.Substring(1));
			}
			else if (address[0] == 'Q')
			{
				operateResult.Content1 = 4;
				operateResult.Content2 = CalculateAddressStarted(address.Substring(1));
			}
			else if (address[0] == 'M')
			{
				operateResult.Content1 = 2;
				operateResult.Content2 = CalculateAddressStarted(address.Substring(1));
			}
			else if (address[0] == 'D' || address.Substring(0, 2) == "DB")
			{
				operateResult.Content1 = 1;
				string[] array = address.Split('.');
				if (address[1] == 'B')
				{
					operateResult.Content3 = Convert.ToUInt16(array[0].Substring(2));
				}
				else
				{
					operateResult.Content3 = Convert.ToUInt16(array[0].Substring(1));
				}
				if (operateResult.Content3 > 255)
				{
					operateResult.Message = StringResources.Language.SiemensDBAddressNotAllowedLargerThan255;
					return operateResult;
				}
				operateResult.Content2 = CalculateAddressStarted(address.Substring(address.IndexOf('.') + 1));
			}
			else if (address[0] == 'T')
			{
				operateResult.Content1 = 7;
				operateResult.Content2 = CalculateAddressStarted(address.Substring(1));
			}
			else
			{
				if (address[0] != 'C')
				{
					operateResult.Message = StringResources.Language.NotSupportedDataType;
					operateResult.Content1 = 0;
					operateResult.Content2 = 0;
					operateResult.Content3 = 0;
					return operateResult;
				}
				operateResult.Content1 = 6;
				operateResult.Content2 = CalculateAddressStarted(address.Substring(1));
			}
		}
		catch (Exception ex)
		{
			operateResult.Message = ex.Message;
			return operateResult;
		}
		operateResult.IsSuccess = true;
		return operateResult;
	}

	public static OperateResult<byte[]> BuildReadCommand(string address, ushort count)
	{
		OperateResult<byte, int, ushort> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] obj = new byte[16]
		{
			83, 53, 16, 1, 3, 5, 3, 8, 0, 0,
			0, 0, 0, 0, 0, 0
		};
		obj[8] = operateResult.Content1;
		obj[9] = (byte)operateResult.Content3;
		obj[10] = (byte)(operateResult.Content2 / 256);
		obj[11] = (byte)(operateResult.Content2 % 256);
		byte[] array = obj;
		if (operateResult.Content1 == 1 || operateResult.Content1 == 6 || operateResult.Content1 == 7)
		{
			if (count % 2 != 0)
			{
				return new OperateResult<byte[]>(StringResources.Language.SiemensReadLengthMustBeEvenNumber);
			}
			array[12] = BitConverter.GetBytes(count / 2)[1];
			array[13] = BitConverter.GetBytes(count / 2)[0];
		}
		else
		{
			array[12] = BitConverter.GetBytes(count)[1];
			array[13] = BitConverter.GetBytes(count)[0];
		}
		array[14] = byte.MaxValue;
		array[15] = 2;
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteCommand(string address, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		OperateResult<byte, int, ushort> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[16 + data.Length];
		array[0] = 83;
		array[1] = 53;
		array[2] = 16;
		array[3] = 1;
		array[4] = 3;
		array[5] = 3;
		array[6] = 3;
		array[7] = 8;
		array[8] = operateResult.Content1;
		array[9] = (byte)operateResult.Content3;
		array[10] = (byte)(operateResult.Content2 / 256);
		array[11] = (byte)(operateResult.Content2 % 256);
		if (operateResult.Content1 == 1 || operateResult.Content1 == 6 || operateResult.Content1 == 7)
		{
			if (data.Length % 2 != 0)
			{
				return new OperateResult<byte[]>(StringResources.Language.SiemensReadLengthMustBeEvenNumber);
			}
			array[12] = BitConverter.GetBytes(data.Length / 2)[1];
			array[13] = BitConverter.GetBytes(data.Length / 2)[0];
		}
		else
		{
			array[12] = BitConverter.GetBytes(data.Length)[1];
			array[13] = BitConverter.GetBytes(data.Length)[0];
		}
		array[14] = byte.MaxValue;
		array[15] = 2;
		Array.Copy(data, 0, array, 16, data.Length);
		return OperateResult.CreateSuccessResult(array);
	}
}
