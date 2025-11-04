using System;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.AllenBradley;

public class AllenBradleySLCNet : DeviceTcpNet
{
	public uint SessionHandle { get; protected set; }

	public AllenBradleySLCNet()
	{
		base.WordLength = 2;
		base.ByteTransform = new RegularByteTransform();
	}

	public AllenBradleySLCNet(string ipAddress, int port = 44818)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new AllenBradleySLCMessage();
	}

	protected override OperateResult InitializationOnConnect()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, "01 01 00 00 00 00 00 00 00 00 00 00 00 04 00 05 00 00 00 00 00 00 00 00 00 00 00 00".ToHexBytes(), hasResponseData: true, usePackAndUnpack: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content.Length >= 8)
		{
			SessionHandle = base.ByteTransform.TransUInt32(operateResult.Content, 4);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(CommunicationPipe, "01 01 00 00 00 00 00 00 00 00 00 00 00 04 00 05 00 00 00 00 00 00 00 00 00 00 00 00".ToHexBytes(), hasResponseData: true, usePackAndUnpack: true).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		if (read.Content.Length >= 8)
		{
			SessionHandle = base.ByteTransform.TransUInt32(read.Content, 4);
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte[]> operateResult = BuildReadCommand(address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(PackCommand(operateResult.Content));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = ExtraActualContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult(operateResult3.Content);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = BuildWriteCommand(address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(PackCommand(operateResult.Content));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = ExtraActualContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult(operateResult3.Content);
	}

	[HslMqttApi("ReadBool", "")]
	public override OperateResult<bool> ReadBool(string address)
	{
		address = AnalysisBitIndex(address, out var bitIndex);
		OperateResult<byte[]> operateResult = Read(address, 1);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.ToBoolArray()[bitIndex]);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		OperateResult<byte[]> operateResult = BuildWriteCommand(address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(PackCommand(operateResult.Content));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = ExtraActualContent(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult(operateResult3.Content);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<byte[]> command = BuildReadCommand(address, length);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(PackCommand(command.Content));
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> extra = ExtraActualContent(read.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult(extra.Content);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> command = BuildWriteCommand(address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(PackCommand(command.Content));
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> extra = ExtraActualContent(read.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult(extra.Content);
	}

	public override async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		address = AnalysisBitIndex(address, out var bitIndex);
		OperateResult<byte[]> read = await ReadAsync(address, 1);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.ToBoolArray()[bitIndex]);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		OperateResult<byte[]> command = BuildWriteCommand(address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(PackCommand(command.Content));
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult<byte[]> extra = ExtraActualContent(read.Content);
		if (!extra.IsSuccess)
		{
			return extra;
		}
		return OperateResult.CreateSuccessResult(extra.Content);
	}

	private byte[] PackCommand(byte[] coreCmd)
	{
		byte[] array = new byte[28 + coreCmd.Length];
		array[0] = 1;
		array[1] = 7;
		array[2] = (byte)(coreCmd.Length / 256);
		array[3] = (byte)(coreCmd.Length % 256);
		BitConverter.GetBytes(SessionHandle).CopyTo(array, 4);
		coreCmd.CopyTo(array, 28);
		return array;
	}

	public static string AnalysisBitIndex(string address, out int bitIndex)
	{
		bitIndex = 0;
		int num = address.IndexOf('/');
		if (num < 0)
		{
			num = address.IndexOf('.');
		}
		if (num > 0)
		{
			bitIndex = int.Parse(address.Substring(num + 1));
			address = address.Substring(0, num);
		}
		return address;
	}

	public static OperateResult<byte[]> BuildReadCommand(string address, int length)
	{
		OperateResult<AllenBradleySLCAddress> operateResult = AllenBradleySLCAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (length < 2)
		{
			length = 2;
		}
		if (operateResult.Content.DataCode == 142)
		{
			operateResult.Content.AddressStart /= 2;
		}
		byte[] obj = new byte[14]
		{
			0, 5, 0, 0, 15, 0, 0, 1, 162, 0,
			0, 0, 0, 0
		};
		obj[9] = (byte)length;
		obj[10] = (byte)operateResult.Content.DbBlock;
		obj[11] = operateResult.Content.DataCode;
		byte[] array = obj;
		BitConverter.GetBytes((ushort)operateResult.Content.AddressStart).CopyTo(array, 12);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteCommand(string address, byte[] value)
	{
		OperateResult<AllenBradleySLCAddress> operateResult = AllenBradleySLCAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (operateResult.Content.DataCode == 142)
		{
			operateResult.Content.AddressStart /= 2;
		}
		byte[] array = new byte[18 + value.Length];
		array[0] = 0;
		array[1] = 5;
		array[2] = 0;
		array[3] = 0;
		array[4] = 15;
		array[5] = 0;
		array[6] = 0;
		array[7] = 1;
		array[8] = 171;
		array[9] = byte.MaxValue;
		array[10] = BitConverter.GetBytes(value.Length)[0];
		array[11] = BitConverter.GetBytes(value.Length)[1];
		array[12] = (byte)operateResult.Content.DbBlock;
		array[13] = operateResult.Content.DataCode;
		BitConverter.GetBytes((ushort)operateResult.Content.AddressStart).CopyTo(array, 14);
		array[16] = byte.MaxValue;
		array[17] = byte.MaxValue;
		value.CopyTo(array, 18);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteCommand(string address, bool value)
	{
		address = AnalysisBitIndex(address, out var bitIndex);
		OperateResult<AllenBradleySLCAddress> operateResult = AllenBradleySLCAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (operateResult.Content.DataCode == 142)
		{
			operateResult.Content.AddressStart /= 2;
		}
		int value2 = 1 << bitIndex;
		byte[] obj = new byte[20]
		{
			0, 5, 0, 0, 15, 0, 0, 1, 171, 255,
			2, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};
		obj[12] = (byte)operateResult.Content.DbBlock;
		obj[13] = operateResult.Content.DataCode;
		byte[] array = obj;
		BitConverter.GetBytes((ushort)operateResult.Content.AddressStart).CopyTo(array, 14);
		array[16] = BitConverter.GetBytes(value2)[0];
		array[17] = BitConverter.GetBytes(value2)[1];
		if (value)
		{
			array[18] = BitConverter.GetBytes(value2)[0];
			array[19] = BitConverter.GetBytes(value2)[1];
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> ExtraActualContent(byte[] content)
	{
		if (content.Length < 36)
		{
			return new OperateResult<byte[]>(StringResources.Language.ReceiveDataLengthTooShort + content.ToHexString(' '));
		}
		return OperateResult.CreateSuccessResult(content.RemoveBegin(36));
	}
}
