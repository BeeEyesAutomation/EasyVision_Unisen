using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Secs.Helper;
using HslCommunication.Secs.Message;
using HslCommunication.Secs.Types;

namespace HslCommunication.Secs;

public class SecsHsms : NetworkDoubleBase, ISecs
{
	public delegate void OnSecsMessageReceivedDelegate(object sender, SecsMessage secsMessage);

	private Encoding stringEncoding = Encoding.Default;

	private SoftIncrementCount incrementCount;

	private List<uint> identityQAs = new List<uint>();

	public ushort DeviceID { get; set; }

	public Gem Gem { get; set; }

	public bool InitializationS0F0 { get; set; } = false;

	public Encoding StringEncoding
	{
		get
		{
			return stringEncoding;
		}
		set
		{
			stringEncoding = value;
		}
	}

	public bool AutoBackS1F1 { get; set; } = true;

	public event OnSecsMessageReceivedDelegate OnSecsMessageReceived;

	public SecsHsms()
	{
		incrementCount = new SoftIncrementCount(4294967295L, 1L);
		base.ByteTransform = new ReverseBytesTransform();
		base.UseServerActivePush = true;
		Gem = new Gem(this);
	}

	public SecsHsms(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SecsHsmsMessage();
	}

	protected override OperateResult InitializationOnConnect(Socket socket)
	{
		if (InitializationS0F0)
		{
			Send(socket, Secs1.BuildHSMSMessage(ushort.MaxValue, 0, 0, 1, (uint)incrementCount.GetCurrentValue(), null, wBit: false));
		}
		return base.InitializationOnConnect(socket);
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync(Socket socket)
	{
		if (InitializationS0F0)
		{
			await SendAsync(socket, Secs1.BuildHSMSMessage(ushort.MaxValue, 0, 0, 1, (uint)incrementCount.GetCurrentValue(), null, wBit: false));
		}
		return await base.InitializationOnConnectAsync(socket);
	}

	protected override bool DecideWhetherQAMessage(Socket socket, OperateResult<byte[]> receive)
	{
		if (!receive.IsSuccess)
		{
			return false;
		}
		byte[] content = receive.Content;
		SecsMessage secsMessage = null;
		try
		{
			secsMessage = new SecsMessage(content, 4);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), "DecideWhetherQAMessage.SecsMessage.cor", ex);
			return false;
		}
		secsMessage.StringEncoding = stringEncoding;
		if (secsMessage.StreamNo == 0 && secsMessage.FunctionNo == 0 && secsMessage.BlockNo % 2 == 1)
		{
			Send(socket, Secs1.BuildHSMSMessage(ushort.MaxValue, 0, 0, (ushort)(secsMessage.BlockNo + 1), secsMessage.MessageID, null, wBit: false));
			return false;
		}
		if (secsMessage.FunctionNo % 2 == 0 && secsMessage.FunctionNo != 0)
		{
			bool flag = false;
			lock (identityQAs)
			{
				flag = identityQAs.Remove(secsMessage.MessageID);
			}
			if (flag)
			{
				return flag;
			}
		}
		if (AutoBackS1F1)
		{
			if (secsMessage.StreamNo == 1 && secsMessage.FunctionNo == 13)
			{
				SendByCommand(1, 14, new SecsValue(new object[2]
				{
					new byte[1],
					SecsValue.EmptyListValue()
				}).ToSourceBytes(), back: false);
				return false;
			}
			if (secsMessage.StreamNo == 2 && secsMessage.FunctionNo == 17)
			{
				SendByCommand(2, 18, new SecsValue(DateTime.Now.ToString("yyyyMMddHHmmssff")), back: false);
				return false;
			}
			if (secsMessage.StreamNo == 1 && secsMessage.FunctionNo == 1)
			{
				SendByCommand(1, 2, SecsValue.EmptyListValue(), back: false);
				return false;
			}
		}
		this.OnSecsMessageReceived?.Invoke(this, secsMessage);
		return false;
	}

	public OperateResult<uint> SendByCommand(byte stream, byte function, byte[] data, bool back)
	{
		uint num = (uint)incrementCount.GetCurrentValue();
		byte[] send = Secs1.BuildHSMSMessage(DeviceID, stream, function, 0, num, data, back);
		return ReadFromCoreServer(send, hasResponseData: false).Convert(num);
	}

	public OperateResult<uint> SendByCommand(byte stream, byte function, SecsValue data, bool back)
	{
		return SendByCommand(stream, function, data.ToSourceBytes(stringEncoding), back);
	}

	public OperateResult SendByCommand(SecsMessage secsMessage, SecsValue data, bool back)
	{
		byte[] send = Secs1.BuildHSMSMessage(DeviceID, secsMessage.StreamNo, (byte)(secsMessage.FunctionNo + 1), 0, secsMessage.MessageID, data.ToSourceBytes(stringEncoding), back);
		return ReadFromCoreServer(send, hasResponseData: false);
	}

	public OperateResult<SecsMessage> ReadSecsMessage(byte stream, byte function, byte[] data, bool back)
	{
		uint num = (uint)incrementCount.GetCurrentValue();
		lock (identityQAs)
		{
			identityQAs.Add(num);
		}
		OperateResult<byte[]> operateResult = ReadFromCoreServer(Secs1.BuildHSMSMessage(DeviceID, stream, function, 0, num, data, back));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<SecsMessage>(operateResult);
		}
		return OperateResult.CreateSuccessResult(new SecsMessage(operateResult.Content, 4)
		{
			StringEncoding = stringEncoding
		});
	}

	public OperateResult<SecsMessage> ReadSecsMessage(byte stream, byte function, SecsValue data, bool back)
	{
		return ReadSecsMessage(stream, function, data.ToSourceBytes(stringEncoding), back);
	}

	public OperateResult SendControlMessage(byte pType, byte sType)
	{
		byte[] send = Secs1.BuildHSMSMessage(ushort.MaxValue, 0, 0, (ushort)(pType * 256 + sType), (uint)incrementCount.GetCurrentValue(), null, wBit: true);
		return ReadFromCoreServer(send, hasResponseData: false);
	}

	public async Task<OperateResult> SendByCommandAsync(byte stream, byte function, byte[] data, bool back)
	{
		byte[] command = Secs1.BuildHSMSMessage(DeviceID, stream, function, 0, (uint)incrementCount.GetCurrentValue(), data, back);
		return await ReadFromCoreServerAsync(command, hasResponseData: false);
	}

	public async Task<OperateResult> SendByCommandAsync(byte stream, byte function, SecsValue data, bool back)
	{
		return await SendByCommandAsync(stream, function, data.ToSourceBytes(stringEncoding), back);
	}

	public async Task<OperateResult<SecsMessage>> ReadSecsMessageAsync(byte stream, byte function, byte[] data, bool back)
	{
		uint identityQA = (uint)incrementCount.GetCurrentValue();
		lock (identityQAs)
		{
			identityQAs.Add(identityQA);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(Secs1.BuildHSMSMessage(DeviceID, stream, function, 0, identityQA, data, back));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<SecsMessage>(read);
		}
		return OperateResult.CreateSuccessResult(new SecsMessage(read.Content, 4)
		{
			StringEncoding = stringEncoding
		});
	}

	public async Task<OperateResult<SecsMessage>> ReadSecsMessageAsync(byte stream, byte function, SecsValue data, bool back)
	{
		return await ReadSecsMessageAsync(stream, function, data.ToSourceBytes(stringEncoding), back);
	}

	public override string ToString()
	{
		return $"SecsHsms[{IpAddress}:{Port}]";
	}
}
