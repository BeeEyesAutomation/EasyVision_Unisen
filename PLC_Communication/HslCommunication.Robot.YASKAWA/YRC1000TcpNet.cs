using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;
using HslCommunication.Robot.YASKAWA.Helper;

namespace HslCommunication.Robot.YASKAWA;

public class YRC1000TcpNet : NetworkDoubleBase, IRobotNet
{
	public YRCType Type { get; set; } = YRCType.YRC1000;

	public YRC1000TcpNet(string ipAddress, int port)
	{
		IpAddress = ipAddress;
		Port = port;
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
		LogMsgFormatBinary = false;
	}

	[HslMqttApi(ApiTopic = "ReadRobotByte", Description = "Read the robot's original byte data information according to the address")]
	public OperateResult<byte[]> Read(string address)
	{
		OperateResult<string> operateResult = ReadString(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(operateResult.Content));
	}

	[HslMqttApi(ApiTopic = "ReadRobotString", Description = "Read the string data information of the robot based on the address")]
	public OperateResult<string> ReadString(string address)
	{
		if (address.IndexOf('.') >= 0 || address.IndexOf(':') >= 0 || address.IndexOf(';') >= 0)
		{
			string[] array = address.Split('.', ':', ';');
			return ReadByCommand(array[0], array[1]);
		}
		return ReadByCommand(address, null);
	}

	[HslMqttApi(ApiTopic = "WriteRobotByte", Description = "According to the address, to write the device related bytes data")]
	public OperateResult Write(string address, byte[] value)
	{
		return Write(address, Encoding.ASCII.GetString(value));
	}

	[HslMqttApi(ApiTopic = "WriteRobotString", Description = "According to the address, to write the device related string data")]
	public OperateResult Write(string address, string value)
	{
		return ReadByCommand(address, value);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string address)
	{
		OperateResult<string> read = await ReadStringAsync(address);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(read.Content));
	}

	public async Task<OperateResult<string>> ReadStringAsync(string address)
	{
		if (address.IndexOf('.') >= 0 || address.IndexOf(':') >= 0 || address.IndexOf(';') >= 0)
		{
			string[] commands = address.Split('.', ':', ';');
			return await ReadByCommandAsync(commands[0], commands[1]);
		}
		return await ReadByCommandAsync(address, null);
	}

	public async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await WriteAsync(address, Encoding.ASCII.GetString(value));
	}

	public async Task<OperateResult> WriteAsync(string address, string value)
	{
		return await ReadByCommandAsync(address, value);
	}

	protected override OperateResult InitializationOnConnect(Socket socket)
	{
		OperateResult<string> operateResult = ReadFromCoreServer(socket, "CONNECT Robot_access KeepAlive:-1\r\n");
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content == "OK:YR Information Server(Ver) Keep-Alive:-1.\r\n")
		{
			return OperateResult.CreateSuccessResult();
		}
		if (!operateResult.Content.StartsWith("OK:"))
		{
			return new OperateResult(operateResult.Content);
		}
		isPersistentConn = false;
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync(Socket socket)
	{
		OperateResult<string> read = await ReadFromCoreServerAsync(socket, "CONNECT Robot_access KeepAlive:-1\r\n");
		if (!read.IsSuccess)
		{
			return read;
		}
		if (read.Content == "OK:YR Information Server(Ver) Keep-Alive:-1.\r\n")
		{
			return OperateResult.CreateSuccessResult();
		}
		if (!read.Content.StartsWith("OK:"))
		{
			return new OperateResult(read.Content);
		}
		isPersistentConn = false;
		return OperateResult.CreateSuccessResult();
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SpecifiedCharacterMessage(13, 10);
	}

	protected OperateResult<string> ReadFromCoreServer(Socket socket, string send)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(socket, Encoding.Default.GetBytes(send));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.Default.GetString(operateResult.Content));
	}

	[HslMqttApi(Description = "Read the device information according to the instructions. If the command data is empty, pass in null. Note that all commands do not have a newline character")]
	public OperateResult<string> ReadByCommand(string command, string commandData)
	{
		pipeSocket.PipeLockEnter();
		OperateResult<Socket> availableSocket = GetAvailableSocket();
		if (!availableSocket.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			base.AlienSession?.Offline();
			pipeSocket.PipeLockLeave();
			return OperateResult.CreateFailedResult<string>(availableSocket);
		}
		string send = (string.IsNullOrEmpty(commandData) ? ("HOSTCTRL_REQUEST " + command + " 0\r\n") : $"HOSTCTRL_REQUEST {command} {commandData.Length + 1}\r\n");
		OperateResult<string> operateResult = ReadFromCoreServer(availableSocket.Content, send);
		if (!operateResult.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			base.AlienSession?.Offline();
			pipeSocket.PipeLockLeave();
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		if (!operateResult.Content.StartsWith("OK:"))
		{
			if (!isPersistentConn)
			{
				availableSocket.Content?.Close();
			}
			pipeSocket.PipeLockLeave();
			return new OperateResult<string>(operateResult.Content.Remove(operateResult.Content.Length - 2));
		}
		if (!string.IsNullOrEmpty(commandData))
		{
			byte[] bytes = Encoding.ASCII.GetBytes(commandData + "\r");
			base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + SoftBasic.ByteToHexString(bytes, ' '));
			OperateResult operateResult2 = Send(availableSocket.Content, bytes);
			if (!operateResult2.IsSuccess)
			{
				availableSocket.Content?.Close();
				pipeSocket.IsSocketError = true;
				base.AlienSession?.Offline();
				pipeSocket.PipeLockLeave();
				return OperateResult.CreateFailedResult<string>(operateResult2);
			}
		}
		OperateResult<byte[]> operateResult3 = ReceiveCommandLineFromSocket(availableSocket.Content, 13, base.ReceiveTimeOut);
		if (!operateResult3.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			base.AlienSession?.Offline();
			pipeSocket.PipeLockLeave();
			return OperateResult.CreateFailedResult<string>(operateResult3);
		}
		string text = Encoding.ASCII.GetString(operateResult3.Content);
		if (string.IsNullOrEmpty(text))
		{
			if (!isPersistentConn)
			{
				availableSocket.Content?.Close();
			}
			pipeSocket.PipeLockLeave();
			return new OperateResult<string>("Return is Null");
		}
		if (text.StartsWith("ERROR:"))
		{
			if (!isPersistentConn)
			{
				availableSocket.Content?.Close();
			}
			pipeSocket.PipeLockLeave();
			Receive(availableSocket.Content, 1);
			return YRCHelper.ExtraErrorMessage(text);
		}
		if (text.StartsWith("0000\r"))
		{
			if (!isPersistentConn)
			{
				availableSocket.Content?.Close();
			}
			Receive(availableSocket.Content, 1);
			pipeSocket.PipeLockLeave();
			return OperateResult.CreateSuccessResult("0000");
		}
		if (!isPersistentConn)
		{
			availableSocket.Content?.Close();
		}
		pipeSocket.PipeLockLeave();
		return OperateResult.CreateSuccessResult(text.Remove(text.Length - 1));
	}

	protected async Task<OperateResult<string>> ReadFromCoreServerAsync(Socket socket, string send)
	{
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(socket, Encoding.Default.GetBytes(send));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		return OperateResult.CreateSuccessResult(Encoding.Default.GetString(read.Content));
	}

	public async Task<OperateResult<string>> ReadByCommandAsync(string command, string commandData)
	{
		await Task.Run(delegate
		{
			pipeSocket.PipeLockEnter();
		});
		OperateResult<Socket> resultSocket = await GetAvailableSocketAsync();
		if (!resultSocket.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			base.AlienSession?.Offline();
			pipeSocket.PipeLockLeave();
			return OperateResult.CreateFailedResult<string>(resultSocket);
		}
		OperateResult<string> readCommand = await ReadFromCoreServerAsync(send: string.IsNullOrEmpty(commandData) ? ("HOSTCTRL_REQUEST " + command + " 0\r\n") : $"HOSTCTRL_REQUEST {command} {commandData.Length + 1}\r\n", socket: resultSocket.Content);
		if (!readCommand.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			base.AlienSession?.Offline();
			pipeSocket.PipeLockLeave();
			return OperateResult.CreateFailedResult<string>(readCommand);
		}
		if (!readCommand.Content.StartsWith("OK:"))
		{
			if (!isPersistentConn)
			{
				resultSocket.Content?.Close();
			}
			pipeSocket.PipeLockLeave();
			return new OperateResult<string>(readCommand.Content.Remove(readCommand.Content.Length - 2));
		}
		if (!string.IsNullOrEmpty(commandData))
		{
			byte[] send2 = Encoding.ASCII.GetBytes(commandData + "\r");
			base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + SoftBasic.ByteToHexString(send2, ' '));
			OperateResult sendResult2 = await SendAsync(resultSocket.Content, send2);
			if (!sendResult2.IsSuccess)
			{
				resultSocket.Content?.Close();
				pipeSocket.IsSocketError = true;
				base.AlienSession?.Offline();
				pipeSocket.PipeLockLeave();
				return OperateResult.CreateFailedResult<string>(sendResult2);
			}
		}
		OperateResult<byte[]> resultReceive2 = await ReceiveCommandLineFromSocketAsync(resultSocket.Content, 13, base.ReceiveTimeOut);
		if (!resultReceive2.IsSuccess)
		{
			pipeSocket.IsSocketError = true;
			base.AlienSession?.Offline();
			pipeSocket.PipeLockLeave();
			return OperateResult.CreateFailedResult<string>(resultReceive2);
		}
		string commandDataReturn = Encoding.ASCII.GetString(resultReceive2.Content);
		if (string.IsNullOrEmpty(commandDataReturn))
		{
			if (!isPersistentConn)
			{
				resultSocket.Content?.Close();
			}
			pipeSocket.PipeLockLeave();
			return new OperateResult<string>("Return is Null");
		}
		if (commandDataReturn.StartsWith("ERROR:"))
		{
			if (!isPersistentConn)
			{
				resultSocket.Content?.Close();
			}
			pipeSocket.PipeLockLeave();
			await ReceiveAsync(resultSocket.Content, 1);
			return YRCHelper.ExtraErrorMessage(commandDataReturn);
		}
		if (commandDataReturn.StartsWith("0000\r"))
		{
			if (!isPersistentConn)
			{
				resultSocket.Content?.Close();
			}
			await ReceiveAsync(resultSocket.Content, 1);
			pipeSocket.PipeLockLeave();
			return OperateResult.CreateSuccessResult("0000");
		}
		if (!isPersistentConn)
		{
			resultSocket.Content?.Close();
		}
		pipeSocket.PipeLockLeave();
		return OperateResult.CreateSuccessResult(commandDataReturn.Remove(commandDataReturn.Length - 1));
	}

	[HslMqttApi(Description = "Read the alarm information of the robot")]
	public OperateResult<string> ReadALARM()
	{
		return ReadByCommand("RALARM", null);
	}

	[HslMqttApi(Description = "Read the coordinate data information of the robot")]
	public OperateResult<string> ReadPOSJ()
	{
		return ReadByCommand("RPOSJ", null);
	}

	[HslMqttApi(Description = "指定坐标系的当前值读取。并且可以指定外部轴的有无。")]
	public OperateResult<YRCRobotData> ReadPOSC(int coordinate, bool hasExteralAxis)
	{
		OperateResult<string> operateResult = ReadByCommand("RPOSC", string.Format("{0},{1}", coordinate, hasExteralAxis ? "1" : "0"));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<YRCRobotData>(operateResult);
		}
		return OperateResult.CreateSuccessResult(new YRCRobotData(Type, operateResult.Content));
	}

	[HslMqttApi(Description = "模式状态，循环状态，动作状态，报警错误状态，伺服状态的读取。")]
	public OperateResult<bool[]> ReadStats()
	{
		OperateResult<string> operateResult = ReadByCommand("RSTATS", null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.ToStringArray<byte>().ToBoolArray());
	}

	[HslMqttApi(Description = "读取当前的程序名，行编号，步编号。")]
	public OperateResult<string> ReadJSeq()
	{
		return ReadByCommand("RJSEQ", null);
	}

	[HslMqttApi(Description = "读取指定用户的坐标数据。")]
	public OperateResult<string> ReadUFrame(int frame)
	{
		return ReadByCommand("RUFRAME", $"{frame}");
	}

	[HslMqttApi(Description = "读取机器人的字节型变量的数据，需要传入变量的编号。")]
	public OperateResult<string> ReadByteVariable(string variableAdderss)
	{
		return ReadByCommand("SAVEV", "0," + variableAdderss);
	}

	[HslMqttApi(Description = "读取机器人的整型变量的数据，需要传入变量的编号")]
	public OperateResult<string> ReadIntegerVariable(string variableAdderss)
	{
		return ReadByCommand("SAVEV", "1," + variableAdderss);
	}

	[HslMqttApi(Description = "读取机器人的双精度整型变量的数据，需要传入变量的编号")]
	public OperateResult<string> ReadDoubleIntegerVariable(string variableAdderss)
	{
		return ReadByCommand("SAVEV", "2," + variableAdderss);
	}

	[HslMqttApi(Description = "读取机器人的实数变量的数据，需要传入变量的编号")]
	public OperateResult<string> ReadRealVariable(string variableAdderss)
	{
		return ReadByCommand("SAVEV", "3," + variableAdderss);
	}

	[HslMqttApi(Description = "读取机器人的字符串变量的数据，需要传入变量的编号")]
	public OperateResult<string> ReadStringVariable(string variableAdderss)
	{
		return ReadByCommand("SAVEV", "7," + variableAdderss);
	}

	[HslMqttApi(Description = "进行HOLD 的 ON/OFF 操作，状态参数 False: OFF，True: ON")]
	public OperateResult Hold(bool status)
	{
		return ReadByCommand("HOLD", status ? "1" : "0");
	}

	[HslMqttApi(Description = "对机械手的报警进行复位")]
	public OperateResult Reset()
	{
		return ReadByCommand("RESET", null);
	}

	[HslMqttApi(Description = "进行错误取消")]
	public OperateResult Cancel()
	{
		return ReadByCommand("CANCEL", null);
	}

	[HslMqttApi(Description = "选择模式。模式编号为1:示教模式，2:再现模式")]
	public OperateResult Mode(int number)
	{
		return ReadByCommand("MODE", number.ToString());
	}

	[HslMqttApi(Description = "选择循环。循环编号 1:步骤，2:1循环，3:连续自动")]
	public OperateResult Cycle(int number)
	{
		return ReadByCommand("CYCLE", number.ToString());
	}

	[HslMqttApi(Description = "进行伺服电源的ON/OFF操作，状态参数 False: OFF，True: ON")]
	public OperateResult Svon(bool status)
	{
		return ReadByCommand("SVON", status ? "1" : "0");
	}

	[HslMqttApi(Description = "设定示教编程器和 I/O的操作信号的联锁。 状态参数 False: OFF，True: ON")]
	public OperateResult HLock(bool status)
	{
		return ReadByCommand("HLOCK", status ? "1" : "0");
	}

	[HslMqttApi(Description = "接受消息数据时， 在YRC1000的示教编程器的远程画面下显示消息若。若不是远程画面时，强制切换到远程画面。显示MDSP命令的消息。")]
	public OperateResult MSDP(string message)
	{
		return ReadByCommand("MDSP", message);
	}

	[HslMqttApi(Description = "开始程序。操作时指定程序名时，此程序能附带对应主程序，则从该程序的开头开始执行。如果没有指定，则从前行开始执行")]
	public OperateResult Start(string programName = null)
	{
		return ReadByCommand("START", programName);
	}

	[HslMqttApi(Description = "删除指定的程序。指定「*」 时， 删除当前登录的所有程序。指定「 删除程序名称」 时，仅删除指定的程序。")]
	public OperateResult Delete(string programName = null)
	{
		return ReadByCommand("DELETE", programName);
	}

	[HslMqttApi(Description = "指定的程序设定为主程序。设定主程序的同时执行程序也被设定。")]
	public OperateResult SetMJ(string programName = null)
	{
		return ReadByCommand("SETMJ", programName);
	}

	[HslMqttApi(Description = "设定执行程序的名称和行编号。")]
	public OperateResult JSeq(string programName, int line)
	{
		return ReadByCommand("JSEQ", $"{programName},{line}");
	}

	public OperateResult MoveJ(YRCRobotData robotData)
	{
		return ReadByCommand("MOVJ", robotData.ToWriteString(Type));
	}

	[HslMqttApi(Description = "读取I/O 信号。 I/O 数据是每8个点输出，所以读出接点数是8的倍数。")]
	public OperateResult<bool[]> IORead(int address, int length)
	{
		OperateResult<string> operateResult = ReadByCommand("IOREAD", $"{address},{length}");
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		byte[] inBytes = operateResult.Content.ToStringArray<byte>();
		return OperateResult.CreateSuccessResult(inBytes.ToBoolArray());
	}

	[HslMqttApi(Description = "写入I/O信号状态，写入接点数请指定8的倍数。IO 信号的网络写入仅可是（ #27010 ～ #29567）。")]
	public OperateResult IOWrite(int address, bool[] value)
	{
		if (value == null || value.Length % 8 != 0)
		{
			return new OperateResult("Parameter [value] can't be null or length must be 8 *N");
		}
		byte[] array = value.ToByteArray();
		StringBuilder stringBuilder = new StringBuilder($"{address},{value.Length}");
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(",");
			stringBuilder.Append(array[i].ToString());
		}
		return ReadByCommand("IOWRITE", stringBuilder.ToString());
	}

	public async Task<OperateResult<string>> ReadALARMAsync()
	{
		return await ReadByCommandAsync("RALARM", null);
	}

	public async Task<OperateResult<string>> ReadPOSJAsync()
	{
		return await ReadByCommandAsync("RPOSJ", null);
	}

	public async Task<OperateResult<YRCRobotData>> ReadPOSCAsync(int coordinate, bool hasExteralAxis)
	{
		OperateResult<string> read = await ReadByCommandAsync("RPOSC", string.Format("{0},{1}", coordinate, hasExteralAxis ? "1" : "0"));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<YRCRobotData>(read);
		}
		return OperateResult.CreateSuccessResult(new YRCRobotData(Type, read.Content));
	}

	public async Task<OperateResult<bool[]>> ReadStatsAsync()
	{
		OperateResult<string> read = await ReadByCommandAsync("RSTATS", null);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.ToStringArray<byte>().ToBoolArray());
	}

	public async Task<OperateResult<string>> ReadJSeqAsync()
	{
		return await ReadByCommandAsync("RJSEQ", null);
	}

	public async Task<OperateResult<string>> ReadUFrameAsync(int frame)
	{
		return await ReadByCommandAsync("RUFRAME", $"{frame}");
	}

	public async Task<OperateResult<string>> ReadByteVariableAsync(string variableAdderss)
	{
		return await ReadByCommandAsync("SAVEV", "0," + variableAdderss);
	}

	public async Task<OperateResult<string>> ReadIntegerVariableAsync(string variableAdderss)
	{
		return await ReadByCommandAsync("SAVEV", "1," + variableAdderss);
	}

	public async Task<OperateResult<string>> ReadDoubleIntegerVariableAsync(string variableAdderss)
	{
		return await ReadByCommandAsync("SAVEV", "2," + variableAdderss);
	}

	public async Task<OperateResult<string>> ReadRealVariableAsync(string variableAdderss)
	{
		return await ReadByCommandAsync("SAVEV", "3," + variableAdderss);
	}

	public async Task<OperateResult<string>> ReadStringVariableAsync(string variableAdderss)
	{
		return await ReadByCommandAsync("SAVEV", "7," + variableAdderss);
	}

	public async Task<OperateResult> HoldAsync(bool status)
	{
		return await ReadByCommandAsync("HOLD", status ? "1" : "0");
	}

	public async Task<OperateResult> ResetAsync()
	{
		return await ReadByCommandAsync("RESET", null);
	}

	public async Task<OperateResult> CancelAsync()
	{
		return await ReadByCommandAsync("CANCEL", null);
	}

	public async Task<OperateResult> ModeAsync(int number)
	{
		return await ReadByCommandAsync("MODE", number.ToString());
	}

	public async Task<OperateResult> CycleAsync(int number)
	{
		return await ReadByCommandAsync("CYCLE", number.ToString());
	}

	public async Task<OperateResult> SvonAsync(bool status)
	{
		return await ReadByCommandAsync("SVON", status ? "1" : "0");
	}

	public async Task<OperateResult> HLockAsync(bool status)
	{
		return await ReadByCommandAsync("HLOCK", status ? "1" : "0");
	}

	public async Task<OperateResult> MSDPAsync(string message)
	{
		return await ReadByCommandAsync("MDSP", message);
	}

	public async Task<OperateResult> StartAsync(string programName = null)
	{
		return await ReadByCommandAsync("START", programName);
	}

	public async Task<OperateResult> DeleteAsync(string programName = null)
	{
		return await ReadByCommandAsync("DELETE", programName);
	}

	public async Task<OperateResult> SetMJAsync(string programName = null)
	{
		return await ReadByCommandAsync("SETMJ", programName);
	}

	public async Task<OperateResult> JSeqAsync(string programName, int line)
	{
		return await ReadByCommandAsync("JSEQ", $"{programName},{line}");
	}

	public async Task<OperateResult> MoveJAsync(YRCRobotData robotData)
	{
		return await ReadByCommandAsync("MOVJ", robotData.ToWriteString(Type));
	}

	public async Task<OperateResult<bool[]>> IOReadAsync(int address, int length)
	{
		OperateResult<string> read = await ReadByCommandAsync("IOREAD", $"{address},{length}");
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		byte[] buffer = read.Content.ToStringArray<byte>();
		return OperateResult.CreateSuccessResult(buffer.ToBoolArray());
	}

	public async Task<OperateResult> IOWriteAsync(int address, bool[] value)
	{
		if (value == null || value.Length % 8 != 0)
		{
			return new OperateResult("Parameter [value] can't be null or length must be 8 *N");
		}
		byte[] buffer = value.ToByteArray();
		StringBuilder sb = new StringBuilder($"{address},{value.Length}");
		for (int i = 0; i < buffer.Length; i++)
		{
			sb.Append(",");
			sb.Append(buffer[i].ToString());
		}
		return await ReadByCommandAsync("IOWRITE", sb.ToString());
	}

	public override string ToString()
	{
		return $"YRC1000TcpNet Robot[{IpAddress}:{Port}]";
	}
}
