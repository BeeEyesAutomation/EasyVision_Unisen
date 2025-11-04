using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Profinet.Cimon.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Cimon;

public class CimonHmiProtocol : DeviceTcpNet
{
	private byte frameNo = 1;

	public byte FrameNo
	{
		get
		{
			return frameNo;
		}
		set
		{
			frameNo = value;
		}
	}

	public CimonHmiProtocol()
	{
		base.WordLength = 2;
		IpAddress = "127.0.0.1";
		Port = 10260;
		base.ByteTransform = new ReverseWordTransform();
	}

	public CimonHmiProtocol(string ip, int port = 10260)
		: this()
	{
		IpAddress = ip;
		Port = port;
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return CimonHelper.ExtractActualData(response);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte[]> operateResult = CimonHelper.BuildReadByteCommand(frameNo, address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte[]> operateResult = CimonHelper.BuildWriteByteCommand(frameNo, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		if (Regex.IsMatch(address, "D[0-9]+\\.[0-9a-fA-F]"))
		{
			return HslHelper.ReadBool(this, address, length, 16, reverseByWord: true);
		}
		OperateResult<byte[]> operateResult = CimonHelper.BuildReadBitCommand(frameNo, address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.Select((byte m) => m != 0).ToArray());
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		OperateResult<byte[]> operateResult = CimonHelper.BuildWriteBitCommand(FrameNo, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<byte[]> command = CimonHelper.BuildReadByteCommand(frameNo, address, length);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(command);
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte[]> command = CimonHelper.BuildWriteByteCommand(frameNo, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		if (Regex.IsMatch(address, "D[0-9]+\\.[0-9a-fA-F]"))
		{
			return await HslHelper.ReadBoolAsync(this, address, length, 16, reverseByWord: true);
		}
		OperateResult<byte[]> command = CimonHelper.BuildReadBitCommand(frameNo, address, length);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(command);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.Select((byte m) => m != 0).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		OperateResult<byte[]> command = CimonHelper.BuildWriteBitCommand(FrameNo, address, value);
		if (!command.IsSuccess)
		{
			return command;
		}
		return await ReadFromCoreServerAsync(command.Content);
	}

	public override string ToString()
	{
		return $"CimonHmiProtocol<{CommunicationPipe}>";
	}
}
