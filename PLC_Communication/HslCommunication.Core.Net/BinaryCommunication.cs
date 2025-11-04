using System.Collections.Generic;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Pipe;
using HslCommunication.LogNet;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Net;

public class BinaryCommunication
{
	protected bool LogMsgFormatBinary = true;

	private CommunicationPipe communicationPipe;

	private string connectionId;

	private byte[] sendbyteBefore = null;

	private string sendBefore = string.Empty;

	[HslMqttApi(HttpMethod = "GET", Description = "The unique ID number of the current connection. The default is a 20-digit guid code plus a random number.")]
	public string ConnectionId
	{
		get
		{
			return connectionId;
		}
		set
		{
			connectionId = value;
		}
	}

	public string SendBeforeHex
	{
		get
		{
			return sendBefore;
		}
		set
		{
			sendBefore = value;
			if (string.IsNullOrEmpty(value))
			{
				sendbyteBefore = null;
			}
			else
			{
				sendbyteBefore = value.ToHexBytes();
			}
		}
	}

	public virtual CommunicationPipe CommunicationPipe
	{
		get
		{
			return communicationPipe;
		}
		set
		{
			communicationPipe = value;
		}
	}

	public ILogNet LogNet { get; set; }

	[HslMqttApi(HttpMethod = "GET", Description = "Gets or sets the time to receive server feedback, and if it is a negative number, does not receive feedback")]
	public int ReceiveTimeOut
	{
		get
		{
			return communicationPipe.ReceiveTimeOut;
		}
		set
		{
			communicationPipe.ReceiveTimeOut = value;
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Get or set the time required to rest before officially receiving the data from the other party. When it is set to 0, no rest is required.")]
	public int SleepTime
	{
		get
		{
			return CommunicationPipe.SleepTime;
		}
		set
		{
			CommunicationPipe.SleepTime = value;
		}
	}

	public BinaryCommunication()
	{
		connectionId = SoftBasic.GetUniqueStringByGuidAndRandom();
	}

	protected virtual INetMessage GetNewNetMessage()
	{
		return null;
	}

	protected virtual bool DecideWhetherQAMessage(CommunicationPipe pipe, OperateResult<byte[]> receive)
	{
		return true;
	}

	protected virtual OperateResult InitializationOnConnect()
	{
		if (communicationPipe.UseServerActivePush)
		{
			communicationPipe.DecideWhetherQAMessageFunction = DecideWhetherQAMessage;
			communicationPipe.StartReceiveBackground(GetNewNetMessage());
		}
		return OperateResult.CreateSuccessResult();
	}

	protected virtual OperateResult ExtraOnDisconnect()
	{
		return OperateResult.CreateSuccessResult();
	}

	protected virtual void ExtraAfterReadFromCoreServer(OperateResult read)
	{
	}

	protected virtual async Task<OperateResult> InitializationOnConnectAsync()
	{
		if (communicationPipe.UseServerActivePush)
		{
			communicationPipe.DecideWhetherQAMessageFunction = DecideWhetherQAMessage;
			communicationPipe.StartReceiveBackground(GetNewNetMessage());
		}
		return await Task.FromResult(OperateResult.CreateSuccessResult()).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected virtual async Task<OperateResult> ExtraOnDisconnectAsync()
	{
		return await Task.FromResult(OperateResult.CreateSuccessResult()).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual byte[] PackCommandWithHeader(byte[] command)
	{
		return command;
	}

	public virtual OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		return OperateResult.CreateSuccessResult(response);
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(byte[] send)
	{
		return ReadFromCoreServer(send, hasResponseData: true, usePackAndUnpack: true);
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(IEnumerable<byte[]> send)
	{
		return NetSupport.ReadFromCoreServer(send, ReadFromCoreServer);
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(byte[] send, bool hasResponseData, bool usePackAndUnpack)
	{
		OperateResult<byte[]> operateResult = new OperateResult<byte[]>();
		OperateResult operateResult2 = communicationPipe.CommunicationLock.EnterLock(communicationPipe.ReceiveTimeOut);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>($"{communicationPipe}", operateResult2);
		}
		try
		{
			OperateResult<bool> operateResult3 = communicationPipe.OpenCommunication();
			if (!operateResult3.IsSuccess)
			{
				communicationPipe.CommunicationLock.LeaveLock();
				return OperateResult.CreateFailedResult<byte[]>($"{communicationPipe}", operateResult3);
			}
			if (operateResult3.Content && communicationPipe.GetType() != typeof(PipeDebugRemote))
			{
				OperateResult operateResult4 = InitializationOnConnect();
				if (!operateResult4.IsSuccess)
				{
					communicationPipe.CommunicationLock.LeaveLock();
					return OperateResult.CreateFailedResult<byte[]>($"{communicationPipe}", operateResult4);
				}
			}
			operateResult = ReadFromCoreServer(communicationPipe, send, hasResponseData, usePackAndUnpack);
			ExtraAfterReadFromCoreServer(operateResult);
			communicationPipe.CommunicationLock.LeaveLock();
		}
		catch
		{
			communicationPipe.CommunicationLock.LeaveLock();
			throw;
		}
		if (!communicationPipe.IsPersistentConnection)
		{
			ExtraOnDisconnect();
			communicationPipe.CloseCommunication();
		}
		return operateResult;
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(CommunicationPipe pipe, byte[] send, bool hasResponseData, bool usePackAndUnpack)
	{
		if (usePackAndUnpack)
		{
			send = PackCommandWithHeader(send);
		}
		if (sendbyteBefore != null)
		{
			OperateResult operateResult = pipe.Send(sendbyteBefore);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>($"{pipe}", operateResult);
			}
			LogSendMessage(sendbyteBefore);
		}
		LogSendMessage(send);
		OperateResult<byte[]> operateResult2 = pipe.ReadFromCoreServer(GetNewNetMessage(), send, hasResponseData, LogRevcMessage);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>($"{pipe}", operateResult2);
		}
		if (!usePackAndUnpack)
		{
			return operateResult2;
		}
		OperateResult<byte[]> operateResult3 = UnpackResponseContent(send, operateResult2.Content);
		if (!operateResult3.IsSuccess && operateResult3.ErrorCode == int.MinValue)
		{
			operateResult3.ErrorCode = 10000;
		}
		return operateResult3;
	}

	public virtual async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(byte[] send)
	{
		return await ReadFromCoreServerAsync(send, hasResponseData: true, usePackAndUnpack: true).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(IEnumerable<byte[]> send)
	{
		return await NetSupport.ReadFromCoreServerAsync(send, ReadFromCoreServerAsync).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(byte[] send, bool hasResponseData, bool usePackAndUnpack)
	{
		new OperateResult<byte[]>();
		OperateResult operateResult = (HslHelper.UseAsyncLock ? (await Task.Run(() => communicationPipe.CommunicationLock.EnterLock(communicationPipe.ReceiveTimeOut)).ConfigureAwait(continueOnCapturedContext: false)) : communicationPipe.CommunicationLock.EnterLock(communicationPipe.ReceiveTimeOut));
		OperateResult enter = operateResult;
		if (!enter.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>($"{communicationPipe}", enter);
		}
		OperateResult<byte[]> read;
		try
		{
			OperateResult<bool> pipe = await communicationPipe.OpenCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (!pipe.IsSuccess)
			{
				communicationPipe.CommunicationLock.LeaveLock();
				return OperateResult.CreateFailedResult<byte[]>($"{communicationPipe}", pipe);
			}
			if (pipe.Content && communicationPipe.GetType() != typeof(PipeDebugRemote))
			{
				OperateResult ini = await InitializationOnConnectAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (!ini.IsSuccess)
				{
					communicationPipe.CommunicationLock.LeaveLock();
					return OperateResult.CreateFailedResult<byte[]>($"{communicationPipe}", ini);
				}
			}
			read = await ReadFromCoreServerAsync(communicationPipe, send, hasResponseData, usePackAndUnpack).ConfigureAwait(continueOnCapturedContext: false);
			ExtraAfterReadFromCoreServer(read);
			communicationPipe.CommunicationLock.LeaveLock();
		}
		catch
		{
			communicationPipe.CommunicationLock.LeaveLock();
			throw;
		}
		if (!communicationPipe.IsPersistentConnection)
		{
			await ExtraOnDisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
			await communicationPipe.CloseCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		return read;
	}

	public virtual async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(CommunicationPipe pipe, byte[] send, bool hasResponseData, bool usePackAndUnpack)
	{
		if (usePackAndUnpack)
		{
			send = PackCommandWithHeader(send);
		}
		if (sendbyteBefore != null)
		{
			OperateResult sendBefore = await communicationPipe.SendAsync(sendbyteBefore).ConfigureAwait(continueOnCapturedContext: false);
			if (!sendBefore.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>($"{pipe}", sendBefore);
			}
			LogSendMessage(sendbyteBefore);
		}
		LogSendMessage(send);
		OperateResult<byte[]> read = await pipe.ReadFromCoreServerAsync(GetNewNetMessage(), send, hasResponseData, LogRevcMessage).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>($"{pipe}", read);
		}
		if (!usePackAndUnpack)
		{
			return read;
		}
		OperateResult<byte[]> unpack = UnpackResponseContent(send, read.Content);
		if (!unpack.IsSuccess && unpack.ErrorCode == int.MinValue)
		{
			unpack.ErrorCode = 10000;
		}
		return unpack;
	}

	protected virtual string GetLogTextFromBinary(PipeSession session, byte[] content)
	{
		return LogMsgFormatBinary ? content.ToHexString(' ') : SoftBasic.GetAsciiStringRender(content);
	}

	protected void LogSendMessage(byte[] content)
	{
		LogSendMessage(content, null);
	}

	protected void LogSendMessage(byte[] content, PipeSession session)
	{
		if (content != null)
		{
			string text = ((session == null) ? string.Empty : $"<{session.Communication}> ");
			LogNet?.WriteDebug(ToString(), text + StringResources.Language.Send + " : " + GetLogTextFromBinary(session, content));
		}
	}

	protected void LogRevcMessage(byte[] content)
	{
		LogRevcMessage(content, null);
	}

	protected void LogRevcMessage(byte[] content, PipeSession session)
	{
		if (content != null)
		{
			string text = ((session == null) ? string.Empty : $"<{session.Communication}> ");
			LogNet?.WriteDebug(ToString(), text + StringResources.Language.Receive + " : " + GetLogTextFromBinary(session, content));
		}
	}

	public OperateResult SetDtuPipe(PipeDtuNet pipe)
	{
		if (pipe != null)
		{
			if (string.IsNullOrEmpty(ConnectionId))
			{
				ConnectionId = pipe.DTU;
			}
			CommunicationPipe = pipe;
			if (!pipe.IsConnectError())
			{
				return InitializationOnConnect();
			}
			return new OperateResult("Session dtu[" + pipe.DTU + "] net error");
		}
		return new OperateResult("pipe is NULL");
	}

	public async Task<OperateResult> SetDtuPipeAsync(PipeDtuNet pipe)
	{
		if (pipe != null)
		{
			if (string.IsNullOrEmpty(ConnectionId))
			{
				ConnectionId = pipe.DTU;
			}
			CommunicationPipe = pipe;
			if (!pipe.IsConnectError())
			{
				return await InitializationOnConnectAsync();
			}
			return new OperateResult("Session dtu[" + pipe.DTU + "] net error");
		}
		return new OperateResult("pipe is NULL");
	}
}
