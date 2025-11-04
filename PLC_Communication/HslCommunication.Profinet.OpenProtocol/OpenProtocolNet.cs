using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.OpenProtocol;

public class OpenProtocolNet : TcpNetCommunication
{
	private Timer timer;

	private ParameterSetMessages parameterSetMessages;

	private JobMessage jobMessage;

	private TighteningResultMessages tighteningResultMessages;

	private ToolMessages toolMessages;

	private TimeMessages timeMessages;

	private int revisonOnConnected;

	public bool KeepAliveMessageEnable { get; set; } = true;

	public int[] ExtraSubscribeMID { get; set; }

	public int RevisonOnConnected
	{
		get
		{
			return revisonOnConnected;
		}
		set
		{
			revisonOnConnected = value;
		}
	}

	public bool AutoAckControllerMessage { get; set; } = false;

	public ParameterSetMessages ParameterSetMessages => parameterSetMessages;

	public JobMessage JobMessage => jobMessage;

	public TighteningResultMessages TighteningResultMessages => tighteningResultMessages;

	public ToolMessages ToolMessages => toolMessages;

	public TimeMessages TimeMessages => timeMessages;

	public event EventHandler<OpenEventArgs> OnReceivedOpenMessage;

	public OpenProtocolNet()
	{
		revisonOnConnected = 1;
		CommunicationPipe.UseServerActivePush = true;
		timer = new Timer(ThreadKeepAlive, null, 10000, 10000);
		parameterSetMessages = new ParameterSetMessages(this);
		jobMessage = new JobMessage(this);
		tighteningResultMessages = new TighteningResultMessages(this);
		toolMessages = new ToolMessages(this);
		timeMessages = new TimeMessages(this);
		LogMsgFormatBinary = false;
	}

	public OpenProtocolNet(string ipAddress, int port = 4545)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new OpenProtocolMessage();
	}

	private void ThreadKeepAlive(object state)
	{
		if (!CommunicationPipe.IsConnectError() && KeepAliveMessageEnable)
		{
			OperateResult<byte[]> operateResult = BuildReadCommand(9999, 1, -1, -1, null);
			if (operateResult.IsSuccess)
			{
				CommunicationPipe.Send(operateResult.Content);
			}
		}
	}

	protected override OperateResult InitializationOnConnect()
	{
		CommunicationPipe.UseServerActivePush = true;
		if (revisonOnConnected >= 0)
		{
			OperateResult<byte[]> operateResult = BuildReadCommand(1, revisonOnConnected, -1, -1, null);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult);
			}
			OperateResult operateResult2 = CommunicationPipe.Send(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult2);
			}
			OperateResult<byte[]> operateResult3 = CommunicationPipe.ReceiveMessage(GetNewNetMessage(), null, useActivePush: false);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>("InitializationOnConnect failed", operateResult3);
			}
			string text = Encoding.ASCII.GetString(operateResult3.Content);
			if (text.Substring(4, 4) == "0002")
			{
				return base.InitializationOnConnect();
			}
			return new OperateResult("Failed:" + text.Substring(4, 4));
		}
		return base.InitializationOnConnect();
	}

	protected override OperateResult ExtraOnDisconnect()
	{
		OperateResult<byte[]> operateResult = BuildReadCommand(3, 1, -1, -1, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return ReadFromCoreServer(CommunicationPipe, operateResult.Content, hasResponseData: true, usePackAndUnpack: true);
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		CommunicationPipe.UseServerActivePush = true;
		if (revisonOnConnected >= 0)
		{
			OperateResult<byte[]> command = BuildReadCommand(1, revisonOnConnected, -1, -1, null);
			if (!command.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(command);
			}
			OperateResult send = await CommunicationPipe.SendAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
			if (!send.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(send);
			}
			OperateResult<byte[]> receive = await CommunicationPipe.ReceiveMessageAsync(GetNewNetMessage(), null, useActivePush: false).ConfigureAwait(continueOnCapturedContext: false);
			if (!receive.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>("InitializationOnConnect failed", receive);
			}
			string reply = Encoding.ASCII.GetString(receive.Content);
			if (reply.Substring(4, 4) == "0002")
			{
				return await base.InitializationOnConnectAsync();
			}
			return new OperateResult("Failed:" + reply.Substring(4, 4));
		}
		return await base.InitializationOnConnectAsync();
	}

	protected virtual int DecideSubscribeData(int mid)
	{
		if (mid == 15 || mid == 35 || mid == 52 || mid == 61 || mid == 71 || mid == 74 || mid == 76 || mid == 91 || mid == 101)
		{
			return mid + 1;
		}
		if (mid == 106 || mid == 107)
		{
			return 108;
		}
		if (mid == 121 || mid == 122 || mid == 123 || mid == 124)
		{
			return 125;
		}
		switch (mid)
		{
		case 152:
			return 153;
		case 211:
			return 212;
		case 217:
			return 218;
		case 221:
			return 222;
		case 242:
			return 243;
		case 251:
			return 252;
		case 401:
			return 402;
		case 421:
			return 422;
		default:
			if (ExtraSubscribeMID == null)
			{
				return -1;
			}
			if (ExtraSubscribeMID.Contains(mid))
			{
				return mid + 1;
			}
			return -1;
		}
	}

	protected override bool DecideWhetherQAMessage(CommunicationPipe pipe, OperateResult<byte[]> receive)
	{
		if (receive.Content.Length >= 20)
		{
			int num = Convert.ToInt32(Encoding.ASCII.GetString(receive.Content, 4, 4));
			bool flag = receive.Content[11] == 48;
			if (num == 9999)
			{
				return false;
			}
			int num2 = DecideSubscribeData(num);
			if (num2 > 0)
			{
				if (flag || AutoAckControllerMessage)
				{
					pipe.Send(BuildReadCommand(num2, 1, -1, -1, null).Content);
				}
				this.OnReceivedOpenMessage?.Invoke(this, new OpenEventArgs(Encoding.ASCII.GetString(receive.Content).TrimEnd(default(char))));
				return false;
			}
		}
		return base.DecideWhetherQAMessage(pipe, receive);
	}

	[HslMqttApi(Description = "使用自定义的命令读取数据，需要指定每个参数信息，然后返回字符串数据内容，根据实际的功能码，解析出实际的数据信息")]
	public OperateResult<string> ReadCustomer(int mid, int revison, int stationId, int spindleId, List<string> parameters)
	{
		if (parameters == null)
		{
			parameters = new List<string>();
		}
		OperateResult<byte[]> operateResult = BuildReadCommand(mid, revison, stationId, spindleId, parameters);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		OperateResult operateResult3 = CheckRequestReplyMessages(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult3);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(operateResult2.Content));
	}

	public async Task<OperateResult<string>> ReadCustomerAsync(int mid, int revison, int stationId, int spindleId, List<string> parameters)
	{
		if (parameters == null)
		{
			parameters = new List<string>();
		}
		OperateResult<byte[]> command = BuildReadCommand(mid, revison, stationId, spindleId, parameters);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(command);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult check = CheckRequestReplyMessages(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(check);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(read.Content));
	}

	public override string ToString()
	{
		return $"OpenProtocolNet[{IpAddress}:{Port}]";
	}

	public static OperateResult<byte[]> BuildReadCommand(int mid, int revison, int stationId, int spindleId, List<string> parameters)
	{
		if (mid < 0 || mid > 9999)
		{
			return new OperateResult<byte[]>("Mid must be between 0 - 9999");
		}
		if (revison < 0 || revison > 999)
		{
			return new OperateResult<byte[]>("revison must be between 0 - 999");
		}
		if (stationId > 9)
		{
			return new OperateResult<byte[]>("stationId must be between 0 - 9");
		}
		if (spindleId > 99)
		{
			return new OperateResult<byte[]>("spindleId must be between 0 - 99");
		}
		int count = 0;
		parameters?.ForEach(delegate(string m)
		{
			count += m.Length;
		});
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append((20 + count).ToString("D4"));
		stringBuilder.Append(mid.ToString("D4"));
		stringBuilder.Append(revison.ToString("D3"));
		stringBuilder.Append('0');
		stringBuilder.Append((stationId < 0) ? "  " : stationId.ToString("D2"));
		stringBuilder.Append((spindleId < 0) ? "  " : spindleId.ToString("D2"));
		stringBuilder.Append(' ');
		stringBuilder.Append(' ');
		stringBuilder.Append(' ');
		stringBuilder.Append(' ');
		if (parameters != null)
		{
			for (int num = 0; num < parameters.Count; num++)
			{
				stringBuilder.Append(parameters[num]);
			}
		}
		stringBuilder.Append('\0');
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static OperateResult<byte[]> BuildOpenProtocolMessage(int mid, int revison, int ack, int stationId, int spindleId, bool withIndex, params string[] parameters)
	{
		if (mid < 0 || mid > 9999)
		{
			return new OperateResult<byte[]>("Mid must be between 0 - 9999");
		}
		if (revison < 0 || revison > 999)
		{
			return new OperateResult<byte[]>("revison must be between 0 - 999");
		}
		if (stationId > 9)
		{
			return new OperateResult<byte[]>("stationId must be between 0 - 9");
		}
		if (spindleId > 99)
		{
			return new OperateResult<byte[]>("spindleId must be between 0 - 99");
		}
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(mid.ToString("D4"));
		stringBuilder.Append(revison.ToString("D3"));
		stringBuilder.Append((ack < 0) ? " " : ack.ToString("D1"));
		stringBuilder.Append((stationId < 0) ? "  " : stationId.ToString("D2"));
		stringBuilder.Append((spindleId < 0) ? "  " : spindleId.ToString("D2"));
		stringBuilder.Append(' ');
		stringBuilder.Append(' ');
		stringBuilder.Append(' ');
		stringBuilder.Append(' ');
		if (parameters != null)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				if (withIndex)
				{
					stringBuilder.Append((i + 1).ToString("D2"));
					stringBuilder.Append(parameters[i]);
					num += 2 + parameters[i].Length;
				}
				else
				{
					stringBuilder.Append(parameters[i]);
					num += parameters[i].Length;
				}
			}
		}
		stringBuilder.Append('\0');
		stringBuilder.Insert(0, (20 + num).ToString("D4"));
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	public static string GetErrorText(int code)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			1 => "Invalid data", 
			2 => "Parameter set ID not present", 
			3 => "Parameter set can not be set.", 
			4 => "Parameter set not running", 
			6 => "VIN upload subscription already exists", 
			7 => "VIN upload subscription does not exists", 
			8 => "VIN input source not granted", 
			9 => "Last tightening result subscription already exists", 
			10 => "Last tightening result subscription does not exist", 
			11 => "Alarm subscription already exists", 
			12 => "Alarm subscription does not exist", 
			13 => "Parameter set selection subscription already exists", 
			14 => "Parameter set selection subscription does not exist", 
			15 => "Tightening ID requested not found", 
			16 => "Connection rejected protocol busy", 
			17 => "Job ID not present", 
			18 => "Job info subscription already exists", 
			19 => "Job info subscription does not exist", 
			20 => "Job can not be set", 
			21 => "Job not running", 
			22 => "Not possible to execute dynamic Job request", 
			23 => "Job batch decrement failed", 
			30 => "Controller is not a sync Master/station controller", 
			31 => "Multi-spindle status subscription already exists", 
			32 => "Multi-spindle status subscription does not exist", 
			33 => "Multi-spindle result subscription already exists", 
			34 => "Multi-spindle result subscription does not exist", 
			40 => "Job line control info subscription already exists", 
			41 => "Job line control info subscription does not exist", 
			42 => "Identifier input source not granted", 
			43 => "Multiple identifiers work order subscription already exists", 
			44 => "Multiple identifiers work order subscription does not exist", 
			50 => "Status external monitored inputs subscription already exists", 
			51 => "Status external monitored inputs subscription does not exist", 
			52 => "IO device not connected", 
			53 => "Faulty IO device ID", 
			58 => "No alarm present", 
			59 => "Tool currently in use", 
			60 => "No histogram available", 
			70 => "Calibration failed", 
			79 => "Command failed", 
			80 => "Audi emergency status subscription exists", 
			81 => "Audi emergency status subscription does not exist", 
			82 => "Automatic/Manual mode subscribe already exist", 
			83 => "Automatic/Manual mode subscribe does not exist", 
			84 => "The relay function subscription already exists", 
			85 => "The relay function subscription does not exist", 
			86 => "The selector socket info subscription already exist", 
			87 => "The selector socket info subscription does not exist", 
			88 => "The digin info subscription already exist", 
			89 => "The digin info subscription does not exist", 
			90 => "Lock at bach done subscription already exist", 
			91 => "Lock at bach done subscription does not exist", 
			92 => "Open protocol commands disabled", 
			93 => "Open protocol commands disabled subscription already exists", 
			94 => "Open protocol commands disabled subscription does not exist", 
			95 => "Reject request, PowerMACS is in manual mode", 
			96 => "Client already connected", 
			97 => "MID revision unsupported", 
			98 => "Controller internal request timeout", 
			99 => "Unknown MID", 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static string GetMid9998Text(int err)
	{
		if (1 == 0)
		{
		}
		string result = err switch
		{
			1 => "Invalid length", 
			2 => "Invalid revision = Not equal to an ASCII number 0 to 99", 
			3 => "Invalid sequence number = Not next expected.", 
			4 => "Inconsistency of “Number of messages”, “Message number”", 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static OperateResult CheckRequestReplyMessages(byte[] reply)
	{
		try
		{
			string text = Encoding.ASCII.GetString(reply, 4, 4);
			if (text == "0004")
			{
				string text2 = Encoding.ASCII.GetString(reply, 20, 4);
				int num = Convert.ToInt32(Encoding.ASCII.GetString(reply, 24, 2));
				if (num == 0)
				{
					return OperateResult.CreateSuccessResult();
				}
				return new OperateResult(num, "The request MID " + text2 + " Select parameter set failed: " + GetErrorText(num));
			}
			if (text == "9998")
			{
				string text3 = Encoding.ASCII.GetString(reply, 20, 4);
				int num2 = Convert.ToInt32(Encoding.ASCII.GetString(reply, 24, reply.Length - 24 - 1));
				if (num2 == 0)
				{
					return OperateResult.CreateSuccessResult();
				}
				return new OperateResult(num2, "The request MID " + text3 + " failed: " + GetMid9998Text(num2));
			}
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
	}
}
