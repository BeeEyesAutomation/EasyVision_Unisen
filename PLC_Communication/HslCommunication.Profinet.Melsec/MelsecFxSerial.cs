using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Pipe;
using HslCommunication.Profinet.Melsec.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecFxSerial : DeviceSerialPort, IMelsecFxSerial, IReadWriteNet
{
	public bool IsNewVersion { get; set; }

	public bool AutoChangeBaudRate { get; set; } = false;

	public MelsecFxSerial()
	{
		base.ByteTransform = new RegularByteTransform();
		base.WordLength = 1;
		IsNewVersion = true;
		base.ByteTransform.IsStringReverseByteWord = true;
		LogMsgFormatBinary = false;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new MelsecFxSerialMessage();
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		int num = MelsecFxSerialHelper.FindSTXIndex(response);
		if (num > 0)
		{
			return OperateResult.CreateSuccessResult(response.RemoveBegin(num));
		}
		return OperateResult.CreateSuccessResult(response);
	}

	public override OperateResult Open()
	{
		if (!(CommunicationPipe is PipeSerialPort pipeSerialPort))
		{
			return new OperateResult("PipeSerialPort get failed");
		}
		int baudRate = pipeSerialPort.GetPipe().BaudRate;
		if (AutoChangeBaudRate && baudRate != 9600)
		{
			pipeSerialPort.GetPipe().BaudRate = 9600;
			OperateResult operateResult = pipeSerialPort.OpenCommunication();
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			for (int i = 0; i < 3; i++)
			{
				OperateResult<byte[]> operateResult2 = pipeSerialPort.ReadFromCoreServer(GetNewNetMessage(), new byte[1] { 5 }, hasResponseData: true);
				if (!operateResult2.IsSuccess)
				{
					return operateResult2;
				}
				if (operateResult2.Content.Length >= 1 && operateResult2.Content[0] == 6)
				{
					break;
				}
				if (i == 2)
				{
					return new OperateResult("check 0x06 back before send data failed!");
				}
			}
			if (1 == 0)
			{
			}
			byte[] array = baudRate switch
			{
				115200 => new byte[6] { 2, 65, 53, 3, 55, 57 }, 
				57600 => new byte[6] { 2, 65, 51, 3, 55, 55 }, 
				38400 => new byte[6] { 2, 65, 50, 3, 55, 54 }, 
				19200 => new byte[6] { 2, 65, 49, 3, 55, 53 }, 
				_ => new byte[6] { 2, 65, 53, 3, 55, 57 }, 
			};
			if (1 == 0)
			{
			}
			byte[] sendValue = array;
			OperateResult<byte[]> operateResult3 = pipeSerialPort.ReadFromCoreServer(GetNewNetMessage(), sendValue, hasResponseData: true);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			if (operateResult3.Content.Length < 1 || operateResult3.Content[0] != 6)
			{
				return new OperateResult("check 0x06 back after send data failed!");
			}
			pipeSerialPort.CloseCommunication();
			pipeSerialPort.GetPipe().BaudRate = baudRate;
		}
		return base.Open();
	}

	protected override OperateResult InitializationOnConnect()
	{
		if (AutoChangeBaudRate)
		{
			return CommunicationPipe.ReadFromCoreServer(GetNewNetMessage(), new byte[1] { 5 }, hasResponseData: true);
		}
		return base.InitializationOnConnect();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return MelsecFxSerialHelper.Read(this, address, length, IsNewVersion);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return MelsecFxSerialHelper.Write(this, address, value, IsNewVersion);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return MelsecFxSerialHelper.ReadBool(this, address, length, IsNewVersion);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return MelsecFxSerialHelper.Write(this, address, value);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return MelsecFxSerialHelper.Write(this, address, value, IsNewVersion);
	}

	[HslMqttApi]
	public OperateResult ActivePlc()
	{
		return MelsecFxSerialHelper.ActivePlc(this);
	}

	public async Task<OperateResult> ActivePlcAsync()
	{
		return await MelsecFxSerialHelper.ActivePlcAsync(this);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await Task.Run(() => Write(address, value));
	}

	public override string ToString()
	{
		return $"MelsecFxSerial[{CommunicationPipe}]";
	}
}
