using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;

namespace HslCommunication.Robot.KUKA;

public class KukaTcpNet : NetworkDoubleBase, IRobotNet
{
	public KukaTcpNet()
	{
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
		LogMsgFormatBinary = false;
	}

	public KukaTcpNet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	public override OperateResult<byte[]> ReadFromCoreServer(Socket socket, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
	{
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + (LogMsgFormatBinary ? send.ToHexString(' ') : SoftBasic.GetAsciiStringRender(send)));
		OperateResult operateResult = Send(socket, send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (base.ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<byte[]> operateResult2 = Receive(socket, -1, base.ReceiveTimeOut);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? operateResult2.Content.ToHexString(' ') : SoftBasic.GetAsciiStringRender(operateResult2.Content)));
		return OperateResult.CreateSuccessResult(operateResult2.Content);
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(Socket socket, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
	{
		byte[] sendValue = (usePackHeader ? PackCommandWithHeader(send) : send);
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + (LogMsgFormatBinary ? sendValue.ToHexString(' ') : SoftBasic.GetAsciiStringRender(sendValue)));
		OperateResult sendResult = await SendAsync(socket, sendValue);
		if (!sendResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(sendResult);
		}
		if (base.ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<byte[]> resultReceive = await ReceiveAsync(socket, -1, base.ReceiveTimeOut);
		if (!resultReceive.IsSuccess)
		{
			return resultReceive;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? resultReceive.Content.ToHexString(' ') : SoftBasic.GetAsciiStringRender(resultReceive.Content)));
		return UnpackResponseContent(sendValue, resultReceive.Content);
	}

	[HslMqttApi(ApiTopic = "ReadRobotByte", Description = "Read the data content of the Kuka robot according to the input variable name")]
	public OperateResult<byte[]> Read(string address)
	{
		return ByteTransformHelper.GetResultFromOther(ReadFromCoreServer(Encoding.UTF8.GetBytes(BuildReadCommands(address))), ExtractActualData);
	}

	[HslMqttApi(ApiTopic = "ReadRobotString", Description = "Read all the data information of the Kuka robot, return the string information, decode by ANSI, need to specify the variable name")]
	public OperateResult<string> ReadString(string address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(Read(address), Encoding.Default.GetString);
	}

	[HslMqttApi(ApiTopic = "WriteRobotByte", Description = "Write the original data content according to the variable name of the Kuka robot")]
	public OperateResult Write(string address, byte[] value)
	{
		return Write(address, Encoding.Default.GetString(value));
	}

	[HslMqttApi(ApiTopic = "WriteRobotString", Description = "Writes ansi-encoded string data information based on the variable name of the Kuka robot")]
	public OperateResult Write(string address, string value)
	{
		return Write(new string[1] { address }, new string[1] { value });
	}

	[HslMqttApi(ApiTopic = "WriteRobotStrings", Description = "Write multiple UTF8 encoded string data information according to the variable name of the Kuka robot")]
	public OperateResult Write(string[] address, string[] value)
	{
		return ReadCmd(BuildWriteCommands(address, value));
	}

	private OperateResult ReadCmd(string cmd)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(Encoding.UTF8.GetBytes(cmd));
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		string text = Encoding.UTF8.GetString(operateResult.Content);
		if (text.Contains("err"))
		{
			return new OperateResult("Result contains err: " + text);
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi(Description = "Start the specified program of the robot")]
	public OperateResult StartProgram(string program)
	{
		return ReadCmd("03" + program);
	}

	[HslMqttApi(Description = "Reset current program")]
	public OperateResult ResetProgram()
	{
		return ReadCmd("0601");
	}

	[HslMqttApi(Description = "Stop current program")]
	public OperateResult StopProgram()
	{
		return ReadCmd("0621");
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string address)
	{
		return ByteTransformHelper.GetResultFromOther(await ReadFromCoreServerAsync(Encoding.UTF8.GetBytes(BuildReadCommands(address))), ExtractActualData);
	}

	public async Task<OperateResult<string>> ReadStringAsync(string address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(await ReadAsync(address), Encoding.Default.GetString);
	}

	public async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await WriteAsync(address, Encoding.Default.GetString(value));
	}

	public async Task<OperateResult> WriteAsync(string address, string value)
	{
		return await WriteAsync(new string[1] { address }, new string[1] { value });
	}

	public async Task<OperateResult> WriteAsync(string[] address, string[] value)
	{
		return await ReadCmdAsync(BuildWriteCommands(address, value));
	}

	private async Task<OperateResult> ReadCmdAsync(string cmd)
	{
		OperateResult<byte[]> write = await ReadFromCoreServerAsync(Encoding.UTF8.GetBytes(cmd));
		if (!write.IsSuccess)
		{
			return write;
		}
		string msg = Encoding.UTF8.GetString(write.Content);
		if (msg.Contains("err"))
		{
			return new OperateResult("Result contains err: " + msg);
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult> StartProgramAsync(string program)
	{
		return await ReadCmdAsync("03" + program);
	}

	public async Task<OperateResult> ResetProgramAsync()
	{
		return await ReadCmdAsync("0601");
	}

	public async Task<OperateResult> StopProgramAsync()
	{
		return await ReadCmdAsync("0621");
	}

	private OperateResult<byte[]> ExtractActualData(byte[] response)
	{
		return OperateResult.CreateSuccessResult(response);
	}

	public override string ToString()
	{
		return $"KukaTcpNet[{IpAddress}:{Port}]";
	}

	public static string BuildReadCommands(string[] address)
	{
		if (address == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder("00");
		for (int i = 0; i < address.Length; i++)
		{
			stringBuilder.Append(address[i] ?? "");
			if (i != address.Length - 1)
			{
				stringBuilder.Append(",");
			}
		}
		return stringBuilder.ToString();
	}

	public static string BuildReadCommands(string address)
	{
		return BuildReadCommands(new string[1] { address });
	}

	public static string BuildWriteCommands(string[] address, string[] values)
	{
		if (address == null || values == null)
		{
			return string.Empty;
		}
		if (address.Length != values.Length)
		{
			throw new Exception(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		StringBuilder stringBuilder = new StringBuilder("01");
		for (int i = 0; i < address.Length; i++)
		{
			stringBuilder.Append(address[i] + "=");
			stringBuilder.Append(values[i] ?? "");
			if (i != address.Length - 1)
			{
				stringBuilder.Append(",");
			}
		}
		return stringBuilder.ToString();
	}

	public static string BuildWriteCommands(string address, string value)
	{
		return BuildWriteCommands(new string[1] { address }, new string[1] { value });
	}
}
