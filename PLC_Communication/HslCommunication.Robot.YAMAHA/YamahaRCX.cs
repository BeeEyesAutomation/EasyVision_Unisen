using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Reflection;

namespace HslCommunication.Robot.YAMAHA;

public class YamahaRCX : TcpNetCommunication
{
	public YamahaRCX()
	{
		base.ReceiveTimeOut = 30000;
		LogMsgFormatBinary = false;
	}

	public YamahaRCX(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(13, 10);
	}

	private int CalculateReceiveTimes(byte[] buffer)
	{
		int num = 0;
		for (int i = 0; i < buffer.Length - 1; i++)
		{
			if (buffer[i] == 13 && buffer[i + 1] == 10)
			{
				num++;
				i++;
			}
		}
		return num;
	}

	private string GetErrorText(string err)
	{
		string text = err.Substring(3);
		if (text == "14.000")
		{
			return "通信中断错误 (Communicate disconnected)";
		}
		return string.Empty;
	}

	public override OperateResult<byte[]> ReadFromCoreServer(CommunicationPipe pipe, byte[] send, bool hasResponseData, bool usePackAndUnpack)
	{
		if (usePackAndUnpack)
		{
			send = PackCommandWithHeader(send);
		}
		LogSendMessage(send);
		OperateResult<byte[]> operateResult = pipe.ReadFromCoreServer(GetNewNetMessage(), send, hasResponseData: false, base.LogRevcMessage);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		int num = CalculateReceiveTimes(send);
		MemoryStream memoryStream = new MemoryStream();
		int num2 = 0;
		while (true)
		{
			OperateResult<byte[]> operateResult2 = pipe.ReadFromCoreServer(GetNewNetMessage(), new byte[0], hasResponseData, base.LogRevcMessage);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			string text = Encoding.ASCII.GetString(operateResult2.Content);
			if (text.StartsWith("Welcome to ", StringComparison.OrdinalIgnoreCase) || text == "\r\n")
			{
				continue;
			}
			memoryStream.Write(operateResult2.Content);
			if (text.StartsWith("OK\r\n", StringComparison.OrdinalIgnoreCase))
			{
				num2++;
				if (num2 >= num)
				{
					break;
				}
			}
			if (text.StartsWith("END\r\n", StringComparison.OrdinalIgnoreCase))
			{
				num2++;
				if (num2 >= num)
				{
					break;
				}
			}
			if (!text.StartsWith("NG=", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			num2++;
			if (num2 < num)
			{
				continue;
			}
			if (text.EndsWith("\r\n"))
			{
				text = text.RemoveLast(2);
			}
			return new OperateResult<byte[]>("faild: " + text + GetErrorText(text));
		}
		if (!usePackAndUnpack)
		{
			return OperateResult.CreateSuccessResult(memoryStream.ToArray());
		}
		OperateResult<byte[]> operateResult3 = UnpackResponseContent(send, memoryStream.ToArray());
		if (!operateResult3.IsSuccess && operateResult3.ErrorCode == int.MinValue)
		{
			operateResult3.ErrorCode = 10000;
		}
		return operateResult3;
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(CommunicationPipe pipe, byte[] send, bool hasResponseData, bool usePackAndUnpack)
	{
		if (usePackAndUnpack)
		{
			send = PackCommandWithHeader(send);
		}
		LogSendMessage(send);
		OperateResult<byte[]> cmdSend = await pipe.ReadFromCoreServerAsync(GetNewNetMessage(), send, hasResponseData: false, base.LogRevcMessage);
		if (!cmdSend.IsSuccess)
		{
			return cmdSend;
		}
		int count = CalculateReceiveTimes(send);
		MemoryStream ms = new MemoryStream();
		int received = 0;
		while (true)
		{
			OperateResult<byte[]> cmdRecv = await pipe.ReadFromCoreServerAsync(GetNewNetMessage(), new byte[0], hasResponseData, base.LogRevcMessage);
			if (!cmdRecv.IsSuccess)
			{
				return cmdRecv;
			}
			string str = Encoding.ASCII.GetString(cmdRecv.Content);
			if (str.StartsWith("Welcome to ", StringComparison.OrdinalIgnoreCase) || str == "\r\n")
			{
				continue;
			}
			ms.Write(cmdRecv.Content);
			if (str.StartsWith("OK\r\n", StringComparison.OrdinalIgnoreCase))
			{
				received++;
				if (received >= count)
				{
					break;
				}
			}
			if (str.StartsWith("END\r\n", StringComparison.OrdinalIgnoreCase))
			{
				received++;
				if (received >= count)
				{
					break;
				}
			}
			if (!str.StartsWith("NG=", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			received++;
			if (received < count)
			{
				continue;
			}
			if (str.EndsWith("\r\n"))
			{
				str = str.RemoveLast(2);
			}
			return new OperateResult<byte[]>("faild: " + str + GetErrorText(str));
		}
		if (!usePackAndUnpack)
		{
			return OperateResult.CreateSuccessResult(ms.ToArray());
		}
		OperateResult<byte[]> unpack = UnpackResponseContent(send, ms.ToArray());
		if (!unpack.IsSuccess && unpack.ErrorCode == int.MinValue)
		{
			unpack.ErrorCode = 10000;
		}
		return unpack;
	}

	[Obsolete]
	public async Task<OperateResult<string[]>> ReadCommandAsync(string command, int lines)
	{
		return await ReadCommandAsync(command);
	}

	public async Task<OperateResult<string[]>> ReadCommandAsync(string command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (!command.EndsWith("\r\n"))
		{
			command += "\r\n";
		}
		byte[] buffer = Encoding.ASCII.GetBytes(command);
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(buffer);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(read);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(read.Content).Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
	}

	public async Task<OperateResult> ResetAsync()
	{
		return await ReadCommandAsync("@ RESET ");
	}

	public async Task<OperateResult> RunAsync()
	{
		return await ReadCommandAsync("@ RUN ");
	}

	public async Task<OperateResult> StopAsync()
	{
		return await ReadCommandAsync("@ STOP ");
	}

	public async Task<OperateResult<int>> ReadMotorStatusAsync()
	{
		return GetTValueHelper(await ReadCommandAsync("@?MOTOR "), Convert.ToInt32);
	}

	public async Task<OperateResult<int>> ReadModeStatusAsync()
	{
		return GetTValueHelper(await ReadCommandAsync("@?MODE "), Convert.ToInt32);
	}

	public async Task<OperateResult<float[]>> ReadJointsAsync()
	{
		return GetTValueHelper(await ReadCommandAsync("@?WHERE "), ConvertToSingleHelper);
	}

	public async Task<OperateResult<int>> ReadEmergencyStatusAsync()
	{
		return GetTValueHelper(await ReadCommandAsync("@?EMG "), Convert.ToInt32);
	}

	public async Task<OperateResult<bool[]>> ReadDIAsync(int index)
	{
		return GetBoolArray(await ReadCommandAsync($"@?DI{index}()"));
	}

	public async Task<OperateResult<bool[]>> ReadDOAsync(int index)
	{
		return GetBoolArray(await ReadCommandAsync($"@?DO{index}()"));
	}

	[Obsolete]
	public OperateResult<string[]> ReadCommand(string command, int lines)
	{
		return ReadCommand(command);
	}

	[HslMqttApi(Description = "The method of reading the specified command requires the specified command and the line number information of the received command")]
	public OperateResult<string[]> ReadCommand(string command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (!command.EndsWith("\r\n"))
		{
			command += "\r\n";
		}
		byte[] bytes = Encoding.ASCII.GetBytes(command);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(bytes);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(operateResult.Content).Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
	}

	[HslMqttApi(Description = "Specify the program reset information to reset all programs. When the program is restarted")]
	public OperateResult Reset()
	{
		return ReadCommand("@ RESET ");
	}

	[HslMqttApi(Description = "Execute the program to run. Execute all RUN state programs.")]
	public OperateResult Run()
	{
		return ReadCommand("@ RUN ");
	}

	public OperateResult Load(string program, int taskId)
	{
		return ReadCommand($"＠ LOAD <{program}>, T{taskId}");
	}

	[HslMqttApi(Description = "The execution program stops. Execute all STOP state programs.")]
	public OperateResult Stop()
	{
		return ReadCommand("@ STOP ");
	}

	private string GetJogXYCommand(int axis, int robot = 1, bool tail = false)
	{
		StringBuilder stringBuilder = new StringBuilder("@ JOGXY ");
		if (robot != 1)
		{
			stringBuilder.Append($"[{robot}] ");
		}
		stringBuilder.Append(Math.Abs(axis).ToString());
		stringBuilder.Append((axis > 0) ? "+" : "-");
		if (tail)
		{
			stringBuilder.Append("\r\n");
		}
		return stringBuilder.ToString();
	}

	public OperateResult SendJogXY(int axis, int robot = 1)
	{
		return CommunicationPipe.Send(Encoding.ASCII.GetBytes(GetJogXYCommand(axis, robot, tail: true)));
	}

	public OperateResult JogXY(int axis, int robot = 1)
	{
		OperateResult<string[]> operateResult = ReadCommand(GetJogXYCommand(axis, robot));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(operateResult);
		}
		return operateResult;
	}

	[HslMqttApi(Description = "Get the motor power status, return 0: motor power off; 1: motor power on; 2: motor power on + all robot servos on")]
	public OperateResult<int> ReadMotorStatus()
	{
		return GetTValueHelper(ReadCommand("@?MOTOR "), Convert.ToInt32);
	}

	[HslMqttApi(Description = "Read mode status")]
	public OperateResult<int> ReadModeStatus()
	{
		return GetTValueHelper(ReadCommand("@?MODE "), Convert.ToInt32);
	}

	[HslMqttApi(Description = "Read the basic data information of the joint")]
	public OperateResult<float[]> ReadJoints()
	{
		return GetTValueHelper(ReadCommand("@?WHERE "), ConvertToSingleHelper);
	}

	[HslMqttApi(Description = "Read emergency stop state, 0: normal state, 1: emergency stop state")]
	public OperateResult<int> ReadEmergencyStatus()
	{
		return GetTValueHelper(ReadCommand("@?EMG "), Convert.ToInt32);
	}

	[HslMqttApi(Description = "Read the input point information and return a bool array")]
	public OperateResult<bool[]> ReadDI(int index)
	{
		return GetBoolArray(ReadCommand($"@?DI{index}()"));
	}

	[HslMqttApi(Description = "Read the output point information and return a bool array")]
	public OperateResult<bool[]> ReadDO(int index)
	{
		return GetBoolArray(ReadCommand($"@?DO{index}()"));
	}

	private OperateResult<bool[]> GetBoolArray(OperateResult<string[]> read)
	{
		return GetTValueHelper(read, delegate(string m)
		{
			int num = Convert.ToInt32(m);
			return new byte[1] { (byte)num }.ToBoolArray();
		});
	}

	private OperateResult<T> GetTValueHelper<T>(OperateResult<string[]> read, Func<string, T> func)
	{
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(read);
		}
		string text = string.Empty;
		try
		{
			text = read.Content[0];
			return OperateResult.CreateSuccessResult(func(text));
		}
		catch (Exception ex)
		{
			return new OperateResult<T>(ex.Message + " Source: " + text);
		}
	}

	private float[] ConvertToSingleHelper(string content)
	{
		return (from m in content.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
			select Convert.ToSingle(m)).ToArray();
	}

	public override string ToString()
	{
		return $"YamahaRCX[{IpAddress}:{Port}]";
	}
}
