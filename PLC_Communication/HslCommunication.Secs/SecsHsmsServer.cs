using System.Text;
using System.Threading;
using HslCommunication.BasicFramework;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Secs.Helper;
using HslCommunication.Secs.Message;
using HslCommunication.Secs.Types;

namespace HslCommunication.Secs;

public class SecsHsmsServer : CommunicationServer
{
	public delegate void SecsMessageReceivedDelegate(object sender, PipeSession session, SecsMessage message);

	private Encoding stringEncoding = Encoding.Default;

	private SoftIncrementCount incrementCount;

	private ushort deviceId = 1;

	private object lockObject = new object();

	private AutoResetEvent autoResetEvent = new AutoResetEvent(initialState: false);

	private uint sendMessageId = 0u;

	private bool waitMessageBack = true;

	private SecsMessage readSecsMessage = null;

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

	public ushort DeviceId
	{
		get
		{
			return deviceId;
		}
		set
		{
			deviceId = value;
		}
	}

	public event SecsMessageReceivedDelegate OnSecsMessageReceived;

	public SecsHsmsServer()
	{
		incrementCount = new SoftIncrementCount(4294967295L, 0L);
		base.OnPipeMessageReceived += SecsHsmsServer_OnPipeMessageReceived;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SecsHsmsMessage();
	}

	private void SecsHsmsServer_OnPipeMessageReceived(PipeSession session, byte[] buffer)
	{
		SecsMessage secsMessage = new SecsMessage(buffer, 4);
		secsMessage.StringEncoding = stringEncoding;
		base.LogNet?.WriteDebug(ToString(), $"[{session.Communication}] {StringResources.Language.Receive}：{buffer.ToHexString(' ')}");
		if (secsMessage.StreamNo == 0 && secsMessage.FunctionNo == 0 && secsMessage.BlockNo % 2 == 1)
		{
			session.Communication.Send(Secs1.BuildHSMSMessage(ushort.MaxValue, 0, 0, (ushort)(secsMessage.BlockNo + 1), secsMessage.MessageID, null, wBit: false));
		}
		if (waitMessageBack && secsMessage.MessageID == sendMessageId && secsMessage.FunctionNo != 0 && secsMessage.FunctionNo % 2 == 0)
		{
			readSecsMessage = secsMessage;
			autoResetEvent.Set();
		}
		RaiseDataReceived(this, session, secsMessage);
	}

	private void RaiseDataReceived(object source, PipeSession session, SecsMessage message)
	{
		this.OnSecsMessageReceived?.Invoke(source, session, message);
	}

	private OperateResult SendToCommunicationPipe(CommunicationPipe pipe, byte[] data)
	{
		base.LogNet?.WriteDebug(ToString(), $"[{pipe}] {StringResources.Language.Send}：{data.ToHexString(' ')}");
		return pipe.Send(data);
	}

	public OperateResult SendByCommand(PipeSession session, SecsMessage receiveMessage, byte stream, byte function, byte[] data)
	{
		byte[] data2 = Secs1.BuildHSMSMessage(receiveMessage.DeviceID, stream, function, 0, receiveMessage.MessageID, data, wBit: false);
		return SendToCommunicationPipe(session.Communication, data2);
	}

	public OperateResult SendByCommand(PipeSession session, SecsMessage receiveMessage, byte stream, byte function, byte[] data, bool wBit)
	{
		byte[] data2 = Secs1.BuildHSMSMessage(receiveMessage.DeviceID, stream, function, 0, receiveMessage.MessageID, data, wBit);
		return SendToCommunicationPipe(session.Communication, data2);
	}

	public OperateResult SendByCommand(PipeSession session, SecsMessage receiveMessage, byte stream, byte function, SecsValue data)
	{
		return SendByCommand(session, receiveMessage, stream, function, (data == null) ? new byte[0] : data.ToSourceBytes(stringEncoding));
	}

	public OperateResult SendByCommand(PipeSession session, SecsMessage receiveMessage, byte stream, byte function, SecsValue data, bool wBit)
	{
		return SendByCommand(session, receiveMessage, stream, function, (data == null) ? new byte[0] : data.ToSourceBytes(stringEncoding), wBit);
	}

	public OperateResult PublishSecsMessage(byte stream, byte function, SecsValue data)
	{
		return PublishSecsMessage(stream, function, data, wBit: false);
	}

	public OperateResult PublishSecsMessage(byte stream, byte function, SecsValue data, bool wBit)
	{
		uint messageId = (uint)incrementCount.GetCurrentValue();
		return PublishSecsMessage(stream, function, data, messageId, wBit);
	}

	public OperateResult PublishSecsMessage(byte stream, byte function, SecsValue data, uint messageId, bool wBit)
	{
		if (data == null)
		{
			data = new SecsValue();
		}
		PipeSession[] pipeSessions = GetPipeSessions();
		for (int i = 0; i < pipeSessions.Length; i++)
		{
			byte[] data2 = Secs1.BuildHSMSMessage(deviceId, stream, function, 0, messageId, data.ToSourceBytes(stringEncoding), wBit);
			OperateResult operateResult = SendToCommunicationPipe(pipeSessions[i].Communication, data2);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public OperateResult PublishSecsMessage(PipeSession session, byte stream, byte function, SecsValue data, bool wBit)
	{
		if (data == null)
		{
			data = new SecsValue();
		}
		uint messageID = (uint)incrementCount.GetCurrentValue();
		byte[] data2 = Secs1.BuildHSMSMessage(deviceId, stream, function, 0, messageID, data.ToSourceBytes(stringEncoding), wBit);
		return SendToCommunicationPipe(session.Communication, data2);
	}

	public OperateResult<SecsMessage> ReadSecsMessage(PipeSession pipe, byte stream, byte function, SecsValue data, bool wBit, int timeout = 5000)
	{
		lock (lockObject)
		{
			uint messageID = (uint)incrementCount.GetCurrentValue();
			if (data == null)
			{
				data = new SecsValue();
			}
			byte[] data2 = Secs1.BuildHSMSMessage(deviceId, stream, function, 0, messageID, data.ToSourceBytes(stringEncoding), wBit);
			sendMessageId = messageID;
			waitMessageBack = true;
			OperateResult operateResult = SendToCommunicationPipe(pipe.Communication, data2);
			if (!operateResult.IsSuccess)
			{
				waitMessageBack = false;
				return OperateResult.CreateFailedResult<SecsMessage>(operateResult);
			}
			bool flag = autoResetEvent.WaitOne(timeout);
			waitMessageBack = false;
			if (flag)
			{
				return OperateResult.CreateSuccessResult(readSecsMessage);
			}
			return new OperateResult<SecsMessage>($"Read Timeout {timeout} ms");
		}
	}

	public override string ToString()
	{
		return $"SecsHsmsServer[{base.Port}]";
	}
}
