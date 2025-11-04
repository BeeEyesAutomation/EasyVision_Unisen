using System;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Fuji;

public class FujiCommandSettingType : DeviceTcpNet
{
	private bool dataSwap = false;

	public bool DataSwap
	{
		get
		{
			return dataSwap;
		}
		set
		{
			dataSwap = value;
			if (value)
			{
				base.ByteTransform = new RegularByteTransform();
			}
			else
			{
				base.ByteTransform = new ReverseBytesTransform();
			}
		}
	}

	public FujiCommandSettingType()
	{
		base.ByteTransform = new ReverseBytesTransform();
		base.WordLength = 2;
	}

	public FujiCommandSettingType(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FujiCommandSettingTypeMessage();
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return UnpackResponseContentHelper(send, response);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte[]> operateResult = BuildReadCommand(address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteCommand(address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<byte[]> bulid = BuildReadCommand(address, length);
		if (!bulid.IsSuccess)
		{
			return bulid;
		}
		return await ReadFromCoreServerAsync(bulid.Content);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> bulid = BuildWriteCommand(address, value);
		if (!bulid.IsSuccess)
		{
			return bulid;
		}
		return await ReadFromCoreServerAsync(bulid.Content);
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
		return $"FujiCommandSettingType[{IpAddress}:{Port}]";
	}

	public static OperateResult<byte[]> BuildReadCommand(string address, ushort length)
	{
		OperateResult<FujiCommandSettingTypeAddress> operateResult = FujiCommandSettingTypeAddress.ParseFrom(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(new byte[9]
		{
			0,
			0,
			0,
			operateResult.Content.DataCode,
			4,
			BitConverter.GetBytes(operateResult.Content.AddressStart)[0],
			BitConverter.GetBytes(operateResult.Content.AddressStart)[1],
			BitConverter.GetBytes(operateResult.Content.Length)[0],
			BitConverter.GetBytes(operateResult.Content.Length)[1]
		});
	}

	public static OperateResult<byte[]> BuildWriteCommand(string address, byte[] value)
	{
		OperateResult<FujiCommandSettingTypeAddress> operateResult = FujiCommandSettingTypeAddress.ParseFrom(address, 0);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[9 + value.Length];
		array[0] = 1;
		array[1] = 0;
		array[2] = 0;
		array[3] = operateResult.Content.DataCode;
		array[4] = (byte)(4 + value.Length);
		array[5] = BitConverter.GetBytes(operateResult.Content.AddressStart)[0];
		array[6] = BitConverter.GetBytes(operateResult.Content.AddressStart)[1];
		array[7] = BitConverter.GetBytes(operateResult.Content.Length)[0];
		array[8] = BitConverter.GetBytes(operateResult.Content.Length)[0];
		value.CopyTo(array, 9);
		return OperateResult.CreateSuccessResult(array);
	}

	public static string GetErrorText(int error)
	{
		if (1 == 0)
		{
		}
		string result = error switch
		{
			18 => "Write of data to the program area", 
			32 => "Non-existing CMND code", 
			33 => "Input data is not in the order of data corresponding to CMND", 
			34 => "Operation only from the loader is effective. Operation from any other node is disabled", 
			36 => "A non-existing module has been specified", 
			50 => "An address out of the memory size has been specified", 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult<byte[]> UnpackResponseContentHelper(byte[] send, byte[] response)
	{
		try
		{
			if (response[1] != 0)
			{
				return new OperateResult<byte[]>(GetErrorText(response[1]));
			}
			if (response[0] == 1)
			{
				return OperateResult.CreateSuccessResult(new byte[0]);
			}
			if (response.Length < 10)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataLengthTooShort + "10, Source: " + response.ToHexString(' '));
			}
			return OperateResult.CreateSuccessResult(response.RemoveBegin(10));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("UnpackResponseContentHelper failed: " + ex.Message + " Source: " + response.ToHexString(' '));
		}
	}
}
